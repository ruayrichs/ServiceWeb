using agape.lib.constant;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using Agape.Lib.Link.Mobile.Model;
using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Master.Entity;
using ERPW.Lib.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API.ServiceTicketAPI
{
    public partial class TriggerAPI : System.Web.UI.Page
    {
        string ThisPage = "TriggerAPI";

        private const string TRANSACTION_ID = "transectionID";

        private const string ACTION_TRIGGER_ESCALATE = "action_trigger_escalate";

        private DBService dbService = new DBService();

        private MasterConfigLibrary libmasterConfig = new MasterConfigLibrary();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SwitchEvent();
            }
            catch (Exception ex)
            {
                JObject response = new JObject();
                AGResponse.generateError(response, ex);
                AGResponse.generate(response, HttpStatusCode.Unauthorized);
                Response.Write(response);

                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(ex.Message, EventLogEntryType.Information, 101, 1);
                }
            }
        }

        private void SwitchEvent()
        {
            string action = "";

            ERPW_TRIGGER_STATUS triggerData = new ERPW_TRIGGER_STATUS();

            Stream receiveStream = Request.InputStream;
            StreamReader readStream = new StreamReader(receiveStream);

            string data = readStream.ReadToEnd();

            readStream.Close();

            if (data != "")
            {
                ResponseTrigger en = JsonConvert.DeserializeObject<ResponseTrigger>(data);

                if (!string.IsNullOrEmpty(en.TransectionID))
                {
                    string[] datasTransection = en.TransectionID.Split('|');
                    if (datasTransection.Length == 2)
                    {
                        action = datasTransection[0];
                    }
                    else
                    {
                        triggerData = TriggerService.GetInstance().GetTriggerData(en.TransectionID);
                        //using (EventLog eventLog = new EventLog("Application"))
                        //{
                        //    eventLog.Source = "Application";
                        //    eventLog.WriteEntry("Log message TriggerStatus : " + triggerData.TriggerStatus, EventLogEntryType.Information, 101, 1);
                        //}
                        if (triggerData.TriggerStatus == TriggerService.TRIGGER_STATUS_CANCEL)
                        {
                            //using (EventLog eventLog = new EventLog("Application"))
                            //{
                            //    eventLog.Source = "Application";
                            //    eventLog.WriteEntry("Log message return", EventLogEntryType.Information, 101, 1);
                            //}
                            return;
                        }

                        if (triggerData.TriggerStatus == TriggerService.TRIGGER_STATUS_PAUSE)
                        {
                            return;
                        }
                        else if (triggerData.TriggerStatus == TriggerService.TRIGGER_STATUS_CONTINUE)
                        {
                            TriggerService.GetInstance().updateDataTriggerEscalateAndBeforeOverdue(triggerData.SID, triggerData.CompanyCode, triggerData.TransactionID, TriggerService.TRIGGER_STATUS_START);
                            return;
                        }
                        else
                        {
                            if (triggerData != null)
                            {
                                action = triggerData.TriggerAction;

                                TriggerService.GetInstance().UpdateTrigger(en.TransectionID, en.StartTime, en.StopTime);
                            }
                        }
                    }
                }
            }

            switch (action)
            {
                case TriggerService.TRIGGER_ACTION_TICKET_START:

                case TriggerService.TRIGGER_ACTION_TICKET_ESCALATE:
                    TriggerEscalate(triggerData);
                    break;
                case TriggerService.TRIGGER_ACTION_TICKET_ESCALATE_MANUALLY:
                    break;
                case TriggerService.TRIGGER_ACTION_TICKET_UPDATE_COMMENT:
                    break;
                case TriggerService.TRIGGER_ACTION_TICKET_BEFORE_OVERDUE:
                    TriggerBefore(triggerData);
                    break;
                case TriggerService.TRIGGER_ACTION_TICKET_STATUS_AUTO_UPDATE:
                    TriggerUpdateStatus(triggerData);
                    break;
                default:
                    throw new Exception("access denied.");
            }
        }

        #region Trigger SLA
        private void TriggerBefore(ERPW_TRIGGER_STATUS triggerData)
        {
            JObject response = new JObject();

            try
            {

                DBService dbService = new DBService();
                DataTable dtTier = new DataTable();
                // Check ticket is closed
                string sql = @"SELECT CallStatus FROM cs_servicecall_header 
                              WHERE SID = '" + triggerData.SID + "' AND CompanyCode = '" + triggerData.CompanyCode + @"'
                              AND DocType = '" + triggerData.DocumentType + "' AND FiscalYear = '" + triggerData.FiscalYear + @"' 
                              AND CallerID = '" + triggerData.DocumentNo + "' AND CallStatus = '" + ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE + "'";

                DataTable dt = dbService.selectDataFocusone(sql);

                if (dt.Rows.Count == 0) // not found = don't close
                {
                    string cut_transationID = triggerData.TransactionID.Replace("befOverdue", "");

                    sql = @"SELECT * FROM CRM_SERVICECALL_MAPPING_ACTIVITY 
                                   WHERE SNAID = '" + triggerData.CompanyCode + "' AND AOBJECTLINK = '" + cut_transationID + @"' 
                                   AND DOCYEAR = '" + triggerData.FiscalYear + "' AND ServiceDocNo = '" + triggerData.DocumentNo + "'";

                    DataTable dtTicket = dbService.selectDataFocusone(sql);

                    if (dtTicket.Rows.Count > 0)
                    {
                        dtTier = AfterSaleService.getInstance().getTierOperation(triggerData.SID, dtTicket.Rows[0]["TierCode"].ToString(), triggerData.DocumentNo);

                        if (dtTier.Rows.Count > 0)
                        {
                            DataRow[] drr = dtTier.Select("Tier = '" + dtTicket.Rows[0]["Tier"] + "'");

                            if (drr.Length > 0)
                            {
                                string currentSequence = drr[0]["sequence"].ToString();

                                NotificationLibrary.GetInstance().TicketAlertEventWithData(
                                      NotificationLibrary.EVENT_TYPE.TICKET_BEFORE_OVERDUE,
                                      triggerData.SID,
                                      triggerData.CompanyCode,
                                      triggerData.DocumentNo,
                                      "System Auto",
                                      ThisPage,
                                      drr[0]
                                  );
                            }
                        }
                    }
                }

                AGResponse.generate(response, HttpStatusCode.OK);


            }
            catch (Exception e)
            {
                AGResponse.generateError(response, e);
                AGResponse.generate(response, HttpStatusCode.BadRequest);

                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(e.Message, EventLogEntryType.Information, 101, 1);
                }
            }

            Response.Write(response);
        }
        #endregion 

        private void TriggerEscalate(ERPW_TRIGGER_STATUS triggerData)
        {
            JObject response = new JObject();

            try
            {
                bool escalate = false;
                bool overDue = false;

                string ticketSubject = "";

                string tierCode = "";
                string escalateTier = "";
                string escalateTierDesc = "";

                double resolutionMinutes = 0;
                double requesterTime = 0;

                DBService dbService = new DBService();
                DataTable dtTier = new DataTable();
                // Check ticket is closed
                string sql = @"SELECT CallStatus FROM cs_servicecall_header 
                              WHERE SID = '" + triggerData.SID + "' AND CompanyCode = '" + triggerData.CompanyCode + @"'
                              AND DocType = '" + triggerData.DocumentType + "' AND FiscalYear = '" + triggerData.FiscalYear + @"' 
                              AND CallerID = '" + triggerData.DocumentNo + "' AND CallStatus = '" + ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE + "'";

                DataTable dt = dbService.selectDataFocusone(sql);

                    if (dt.Rows.Count == 0) // not found = don't close
                    {
                        sql = @"SELECT * FROM CRM_SERVICECALL_MAPPING_ACTIVITY 
                                   WHERE SNAID = '" + triggerData.CompanyCode + "' AND AOBJECTLINK = '" + triggerData.TransactionID + @"' 
                                   AND DOCYEAR = '" + triggerData.FiscalYear + "' AND ServiceDocNo = '" + triggerData.DocumentNo + "'";

                        DataTable dtTicket = dbService.selectDataFocusone(sql);

                        if (dtTicket.Rows.Count > 0)
                        {
                            dtTier = AfterSaleService.getInstance().getTierOperation(triggerData.SID, dtTicket.Rows[0]["TierCode"].ToString(), triggerData.DocumentNo);

                            if (dtTier.Rows.Count > 0)
                            {
                                DataRow[] drr = dtTier.Select("Tier = '" + dtTicket.Rows[0]["Tier"] + "'");

                                if (drr.Length > 0)
                                {
                                    ticketSubject = drr[0]["Remark"].ToString();

                                    string currentSequence = drr[0]["sequence"].ToString();

                                    drr = dtTier.Select("sequence > " + currentSequence, "sequence ASC");

                                    if (drr.Length > 0) // Check has next tier
                                    {
                                        if (string.IsNullOrEmpty(drr[0]["AOBJECTLINK"].ToString())) // empty = don't escalate
                                        {
                                            escalate = true;
                                            tierCode = drr[0]["TierCode"].ToString();
                                            escalateTier = drr[0]["Tier"].ToString();
                                            escalateTierDesc = drr[0]["TierDescription"].ToString();

                                            double.TryParse(drr[0]["Resolution"].ToString(), out resolutionMinutes);
                                            double.TryParse(drr[0]["Requester"].ToString(), out requesterTime);
                                        }
                                    }
                                    else
                                    {
                                        overDue = true;
                                    }
                                }
                            }
                        }
                    }

                    if (escalate && escalateTier != "")
                    {
                        string incidentArea = "";
                        string equipmentCode = "";

                        // Get equipment and incident area
                        sql = @"SELECT ObjectID FROM cs_servicecall_header 
                            WHERE SID = '" + triggerData.SID + "' AND CompanyCode = '" + triggerData.CompanyCode + @"' 
                            AND DocType = '" + triggerData.DocumentType + "' AND CallerID = '" + triggerData.DocumentNo + @"' 
                            AND FiscalYear = '" + triggerData.FiscalYear + "'";

                        DataTable dtHeader = dbService.selectDataFocusone(sql);

                        if (dtHeader.Rows.Count > 0)
                        {
                            sql = @"SELECT EquipmentNo, IncidentArea FROM cs_servicecall_item 
                                WHERE SID = '" + triggerData.SID + "' AND CompanyCode = '" + triggerData.CompanyCode + @"' 
                                AND ObjectID = '" + dtHeader.Rows[0]["ObjectID"] + "' AND xLineNo = '001'";

                            DataTable dtItem = dbService.selectDataFocusone(sql);

                            if (dtItem.Rows.Count > 0)
                            {
                                incidentArea = dtItem.Rows[0]["IncidentArea"].ToString();
                                equipmentCode = dtItem.Rows[0]["EquipmentNo"].ToString();
                            }
                        }

                        try
                        {
                            string MainDelegate = "";
                            List<string> participantsArray = new List<string>();

                            string TierCode = "";
                            string Tier = "";
                            string OwnerGroupService = ServiceLibrary.LookUpTable("QueueOption", "cs_servicecall_item", "where ObjectID = '" +
                                ServiceLibrary.LookUpTable("ObjectID", "cs_servicecall_header", "where CallerID = '" + triggerData.DocumentNo + "'") +
                            "'");

                            DataTable dtMain = new DataTable();
                            DataTable dtParticipant = new DataTable();

                            foreach (DataRow drTier in dtTier.Rows)
                            {
                                if (!string.IsNullOrEmpty(TierCode) && !string.IsNullOrEmpty(Tier))
                                {
                                    continue;
                                }

                                if (string.IsNullOrEmpty(drTier["AOBJECTLINK"].ToString()))
                                {
                                    TierCode = drTier["TierCode"].ToString();
                                    Tier = drTier["Tier"].ToString();
                                }
                            }

                            //using (EventLog eventLog = new EventLog("Application"))
                            //{
                            //    eventLog.Source = "Application";
                            //    eventLog.WriteEntry(
                            //        "Log TierCode : " + TierCode + 
                            //        ", Tier : " + Tier + 
                            //        ", OwnerGroupService : " + OwnerGroupService +
                            //        ", SID : " + triggerData.SID +
                            //        ", CompanyCode : " + triggerData.CompanyCode +
                            //        ", DocumentType : " + triggerData.DocumentType
                            //        , EventLogEntryType.Information, 101, 1
                            //    );
                            //}

                            dtMain = AfterSaleService.getInstance().GetTierMainDelegate(
                                triggerData.SID, triggerData.CompanyCode,
                                TierCode, Tier, triggerData.DocumentType,
                                "", OwnerGroupService
                            );

                            dtParticipant = AfterSaleService.getInstance().GetTierParticipants(
                                triggerData.SID, triggerData.CompanyCode,
                                TierCode, Tier, triggerData.DocumentType,
                                "", OwnerGroupService
                            );

                            foreach (DataRow drMain in dtMain.Rows)
                            {
                                MainDelegate = drMain["EmployeeCode"].ToString();
                            }

                            //using (EventLog eventLog = new EventLog("Application"))
                            //{
                            //    eventLog.Source = "Application";
                            //    eventLog.WriteEntry(
                            //        "Log MainDelegate : " + MainDelegate
                            //        , EventLogEntryType.Information, 101, 1
                            //    );
                            //}

                            foreach (DataRow drParticipant in dtParticipant.Rows)
                            {
                                participantsArray.Add(drParticipant["EmployeeCode"].ToString());
                            }

                            //using (EventLog eventLog = new EventLog("Application"))
                            //{
                            //    eventLog.Source = "Application";
                            //    eventLog.WriteEntry(
                            //        "Log participantsArray : " + string.Join(", ", participantsArray)
                            //        , EventLogEntryType.Information, 101, 1
                            //    );
                            //}


                            // Escalate to next tier
                            AfterSaleService.getInstance().EscalateTicket(
                                triggerData.SID, triggerData.CompanyCode, triggerData.DocumentType, triggerData.DocumentNo,
                                triggerData.FiscalYear, tierCode, escalateTier, escalateTierDesc, resolutionMinutes,
                                requesterTime,
                                incidentArea, equipmentCode, ticketSubject, "System Auto", "", "System Auto", false,
                                MainDelegate, participantsArray.ToArray(), null, null);

                            //AfterSaleService.getInstance().SetTriggerBeforeOverdue(
                            //        "", triggerData.DocumentType, triggerData.DocumentNo
                            //    );

                    }
                        catch (Exception ex)
                        {
                            using (EventLog eventLog = new EventLog("Application"))
                            {
                                eventLog.Source = "Application";
                                eventLog.WriteEntry("Log message " + ex.Message, EventLogEntryType.Information, 101, 1);
                            }

                            // Escalate to next tier
                            AfterSaleService.getInstance().EscalateTicket(
                                triggerData.SID, triggerData.CompanyCode, triggerData.DocumentType, triggerData.DocumentNo,
                                triggerData.FiscalYear, tierCode, escalateTier, escalateTierDesc, resolutionMinutes,
                                requesterTime,
                                incidentArea, equipmentCode, ticketSubject, "System Auto", "", "System Auto", false);
                        }
                    }

                    bool overDueActive = overDue;
                    if (overDue)
                    {
                        sql = @"select b.EventType from cs_servicecall_header a
                        inner join ERPW_TICKET_STATUS b
                        on a.SID = b.SID
                        and a.CompanyCode = b.CompanyCode
                        and a.Docstatus = b.TicketStatusCode
                        where a.SID = '" + triggerData.SID + @"' 
                        and a.CompanyCode = '" + triggerData.CompanyCode + @"' 
                        and a.CallerID = '" + triggerData.DocumentNo + "'";

                        DataTable dtEventTypeStatus = dbService.selectDataFocusone(sql);
                        string EventTypeStatus = dtEventTypeStatus.Rows[0]["EventType"].ToString();
                        if (EventTypeStatus == "CANCEL" || EventTypeStatus == "CANCEL_CHANGE" || EventTypeStatus == "CLOSED" || EventTypeStatus == "CLOSED_CHANGE")
                        {
                            overDueActive = false;
                        }
                    }

                    if (overDueActive)
                    {
                        NotificationLibrary.GetInstance().TicketAlertEvent(
                            NotificationLibrary.EVENT_TYPE.TICKET_OVERDUE,
                            triggerData.SID,
                            triggerData.CompanyCode,
                            triggerData.DocumentNo,
                            "System Auto",
                            ThisPage
                        );
                    }

                    if (triggerData.DocumentType == "CI")
                    {
                        NotificationLibrary.GetInstance().TicketAlertEvent(
                            NotificationLibrary.EVENT_TYPE.CI_MA_OVERDUE,
                            triggerData.SID,
                            triggerData.CompanyCode,
                            triggerData.DocumentNo,
                            "System Auto",
                            ThisPage + "_CI"
                        );
                    }         

                AGResponse.generate(response, HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                AGResponse.generateError(response, e);
                AGResponse.generate(response, HttpStatusCode.BadRequest);

                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(e.Message, EventLogEntryType.Information, 101, 1);
                }
            }

            Response.Write(response);
        }

        private void TriggerUpdateStatus(ERPW_TRIGGER_STATUS triggerData)
        {
            JObject response = new JObject();

            try
            {
                string[] splitTransactionID = triggerData.TransactionID.Split('|');
                string statusBegin = splitTransactionID[1];
                string statusTarget = splitTransactionID[2];

                DataTable dtAUSConfig = libmasterConfig.GetMasterConfigTicketStatusAuto(triggerData.SID, triggerData.CompanyCode, statusBegin);

                if (dtAUSConfig.Rows.Count > 0)
                {
                    bool isWorking = Convert.ToBoolean(dtAUSConfig.Rows[0]["WorkingStatus"].ToString());
                    DataTable TicketStatusTargetData = libmasterConfig.GetMasterConfigTicketStatus(triggerData.SID, triggerData.CompanyCode, statusTarget, null);

                    if (isWorking && TicketStatusTargetData.Rows.Count > 0)
                    {
                        string TICKET_STATUS_EVENT_TARGET_TYPE = TicketStatusTargetData.Rows[0]["EventType"].ToString();
                        string sql = @"SELECT ObjectID FROM cs_servicecall_header 
                            WHERE SID = '" + triggerData.SID + "' AND CompanyCode = '" + triggerData.CompanyCode + @"' 
                            AND DocType = '" + triggerData.DocumentType + "' AND CallerID = '" + triggerData.DocumentNo + @"' 
                            AND FiscalYear = '" + triggerData.FiscalYear + "'" + @"
                            AND Docstatus = '" + statusBegin + "'";

                        DataTable dtHeader = dbService.selectDataFocusone(sql);

                        if (dtHeader.Rows.Count > 0)
                        {
                            switch (TICKET_STATUS_EVENT_TARGET_TYPE)
                            {
                                case ServiceTicketLibrary.TICKET_STATUS_EVENT_INPROGRESS:
                                    doTicketInprogess(triggerData, statusBegin);
                                    break;
                                case ServiceTicketLibrary.TICKET_STATUS_EVENT_CLOSED:
                                    doTicketClosed(triggerData);
                                    break;
                                default:
                                    throw new Exception("access denied.");
                            }
                        }
                    } else
                    {
                        TriggerService.GetInstance().CancelTrigger(triggerData.TransactionID);
                    }

                }

                Response.Write(response);
                AGResponse.generate(response, HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                AGResponse.generateError(response, e);
                AGResponse.generate(response, HttpStatusCode.BadRequest);

                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(e.Message, EventLogEntryType.Information, 101, 1);
                }
            }  
            
        }

        ServiceTicketLibrary lib = new ServiceTicketLibrary();

        #region inprogress status
        private void doTicketInprogess(ERPW_TRIGGER_STATUS triggerData, string statusBegin)
        {
            string TICKET_STATUS_EVENT_INPROGRESS_CODE = lib.GetTicketStatusFromEvent(triggerData.SID, triggerData.CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_INPROGRESS);
            AfterSaleService.getInstance().UpdateStatus(
                triggerData.SID, 
                triggerData.CompanyCode,
                TICKET_STATUS_EVENT_INPROGRESS_CODE,
                triggerData.DocumentType,
                triggerData.FiscalYear,
                triggerData.DocumentNo,
                triggerData.Created_By, 
                Validation.getCurrentServerStringDateTime());

            string TICKET_STATUS_EVENT_BEGIN_DESC = ServiceTicketLibrary.GetTicketDocStatusDesc(triggerData.SID, triggerData.CompanyCode, statusBegin);


            string quoteMessage = "Update status from \"" + TICKET_STATUS_EVENT_BEGIN_DESC + "\" to \"" + ServiceTicketLibrary.TICKET_STATUS_EVENT_INPROGRESS_DESC + "\"";
            List<logValue_OldNew> enLog = new List<logValue_OldNew>();
            enLog.Add(new logValue_OldNew
            {
                Value_Old = "",
                Value_New = quoteMessage,
                TableName = "",
                FieldName = "",
                AccessCode = LogServiceLibrary.AccessCode_Change
            });
            SaveLog(triggerData, enLog);

            NotificationLibrary.GetInstance().TicketAlertEvent(
               NotificationLibrary.EVENT_TYPE.TICKET_UPDATESTATUS,
               triggerData.SID,
               triggerData.CompanyCode,
               triggerData.DocumentNo,
               triggerData.Created_By,
               ThisPage
           );

        }
        #endregion

        #region close status
        private void doTicketClosed(ERPW_TRIGGER_STATUS triggerData)
        {
            string TICKET_STATUS_EVENT_CLOSED_CODE = lib.GetTicketStatusFromEvent(triggerData.SID, triggerData.CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CLOSED);
            string sql = @"SELECT * FROM cs_servicecall_header 
                            WHERE SID = '" + triggerData.SID + "' AND CompanyCode = '" + triggerData.CompanyCode + @"' 
                            AND DocType = '" + triggerData.DocumentType + "' AND CallerID = '" + triggerData.DocumentNo + @"' 
                            AND FiscalYear = '" + triggerData.FiscalYear + "'";


            DataTable dtHeader = dbService.selectDataFocusone(sql);

            string objectID = dtHeader.Rows[0]["ObjectID"].ToString();
                 
            string sqlItem = @"SELECT * FROM cs_servicecall_item 
                            WHERE SID = '" + triggerData.SID + "' AND CompanyCode = '" + triggerData.CompanyCode + @"' 
                            AND ObjectID = '" + objectID + "'";

            DataTable dtItem = dbService.selectDataFocusone(sqlItem);

            List<logValue_OldNew> enLog = new List<logValue_OldNew>();

            foreach (DataRow drHeader in dtHeader.Rows)
            {
                string textOldValue = "";
                if (Convert.ToString(drHeader["AffectSLA"]) == "")
                {
                    textOldValue = "";
                }
                else if (Convert.ToString(drHeader["AffectSLA"]) == "True")
                {
                    textOldValue = "Affect SLA";
                }
                else if (Convert.ToString(drHeader["AffectSLA"]) == "False")
                {
                    textOldValue = "Not Affect SLA";
                }

                string textNewValue = "";

                enLog.Add(new logValue_OldNew
                {
                    Value_Old = textOldValue,
                    Value_New = textNewValue,
                    TableName = "cs_servicecall_header",
                    FieldName = "Affect SLA",
                    AccessCode = LogServiceLibrary.AccessCode_Change
                });
            }

            foreach (DataRow drItem in dtItem.Rows)
            {
                enLog.Add(new logValue_OldNew
                {
                    Value_Old = Convert.ToString(drItem["SummaryProblem"]),
                    Value_New = "",
                    TableName = "cs_servicecall_item",
                    FieldName = "Summary Problem",
                    AccessCode = LogServiceLibrary.AccessCode_Change
                });
  
                enLog.Add(new logValue_OldNew
                {
                    Value_Old = Convert.ToString(drItem["SummaryCause"]),
                    Value_New = "",
                    TableName = "cs_servicecall_item",
                    FieldName = "Summary Cause",
                    AccessCode = LogServiceLibrary.AccessCode_Change
                });


                enLog.Add(new logValue_OldNew
                {
                    Value_Old = Convert.ToString(drItem["SummaryResolution"]),
                    Value_New = "",
                    TableName = "cs_servicecall_item",
                    FieldName = "Summary Resolution",
                    AccessCode = LogServiceLibrary.AccessCode_Change
                });

            }

            #region close servicecall
            string canceldate = Validation.getCurrentServerStringDateTime().Substring(0, 8);
            string canceltime = Validation.getCurrentServerDateTime().ToString("HHmmss");
            string cancelby = triggerData.Created_By;
            string cancelcomment = triggerData.Created_By;
            string close_status = ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE;

            if (dtItem.Rows.Count != 0)
            {
                saveCloseDetailtoItem(
                    objectID,
                    canceldate, 
                    canceltime,
                    cancelcomment,
                    cancelby,
                    close_status
                    );
                // get new data after update
                dtItem = dbService.selectDataFocusone(sqlItem);

            }

            bool isopen = false;

            foreach (DataRow dr in dtItem.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if ("".Equals(dr["CloseStatus"].ToString()) || "01".Equals(dr["CloseStatus"].ToString()))
                    {
                        isopen = true;
                    }
                }
            }

            if (!isopen)
            {
                

                saveCloseDetailtoHeader(
                    triggerData.DocumentNo,
                    ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE,
                    TICKET_STATUS_EVENT_CLOSED_CODE,
                    cancelby,
                    Validation.getCurrentServerStringDateTime()
                    );

                // get new data after update
                dtHeader = dbService.selectDataFocusone(sql);
            }

            #endregion

            enLog.Add(new logValue_OldNew
            {
                Value_Old = "",
                Value_New = "Close Ticket",
                TableName = "",
                FieldName = "",
                AccessCode = LogServiceLibrary.AccessCode_Change
            });
            SaveLog(triggerData, enLog);   

            AfterSaleService.getInstance().UpdateStatus(
                triggerData.SID,
                triggerData.CompanyCode,
                TICKET_STATUS_EVENT_CLOSED_CODE,
                triggerData.DocumentType,
                triggerData.FiscalYear,
                triggerData.DocumentNo,
                triggerData.Created_By,
                Validation.getCurrentServerStringDateTime());

            
            NotificationLibrary.GetInstance().TicketAlertEvent(
                NotificationLibrary.EVENT_TYPE.TICKET_CLOSE,
                triggerData.SID,
                triggerData.CompanyCode,
                triggerData.DocumentNo,
                triggerData.Created_By,
                ThisPage
            );
        }
        private void saveCloseDetailtoItem(string ObjectId, string canceldate, string canceltime, string cancelcomment, string cancelby, string close_status )
        {
            string where = "WHERE ObjectID = '" + ObjectId + "'";
            string sql = @"UPDATE cs_servicecall_item 
                            SET ClosedOnDate = '" + canceldate + @"'
                            ,ClosedOnTime = '" + canceltime + @"'
                            ,CloseComment = '" + cancelcomment + @"'
                            ,CloseBy = '" + cancelby + @"'
                            ,CloseStatus = '" + close_status + "' " + where;

            dbService.executeSQLForFocusone(sql);
        }
        private void saveCloseDetailtoHeader(string DocumentNo, string CallStatus, string Docstatus, string UPDATED_BY, string UPDATED_ON)
        {
            string where = "WHERE CallerID = '" + DocumentNo + "'";
            string sql = @"UPDATE cs_servicecall_header
                            SET CallStatus = '" + CallStatus + @"'
                            ,Docstatus = '" + Docstatus + @"'
                            ,UPDATED_BY = '" + UPDATED_BY + @"'
                            ,UPDATED_ON = '" + UPDATED_ON + "' " + where;

            dbService.executeSQLForFocusone(sql);
        }

        #endregion

        private void SaveLog(ERPW_TRIGGER_STATUS triggerData, List<logValue_OldNew> enLog)
        {

            if (enLog.Count == 0)
            {
                return;
            }

            string DocType = triggerData.DocumentType;

            List<Main_LogService> en = AfterSaleService.getInstance().SaveLogTicket(triggerData.SID, DocType, triggerData.FiscalYear, triggerData.DocumentNo, triggerData.CompanyCode,
                triggerData.Created_By, enLog);

        }


    }
}