using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ERPW.Lib.Service;
using ERPW.Lib.WebConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace ServiceWeb.Service
{
    public class TriggerService
    {
        private string TRIGGER_POST_URL = WebConfigurationManager.AppSettings["TRIGGER_POST_URL"];        
        private string TRIGGER_RESPONSE_URL = WebConfigurationManager.AppSettings["TRIGGER_RESPONSE_URL"];
        private const string TRIGGER_TOKEN_KEY = "eyJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoib25lY2hhdF90ZXN0MSIsInR5cGUiOjIsImV4cCI6NDY4MDEzNjIwMCwiaWF0IjoxNTU1OTk4NDc0LCJpc3MiOiJPbmVDaGF0In0.uKjbdgdR9aFKn6bWm1rXFTMYGUYOq_ABtMuqIvpNUYY";

        public static string WS_TRIGGER_RESULT = WebConfigurationManager.AppSettings["WS_TRIGGER_RESULT"];

        public const string TRIGGER_STATUS_START = "START";
        public const string TRIGGER_STATUS_CANCEL = "CANCEL";
        public const string TRIGGER_STATUS_FINISH = "FINISH";
        public const string TRIGGER_STATUS_PAUSE = "PAUSE";
        public const string TRIGGER_STATUS_CONTINUE = "CONTINUE";

        public const string TRIGGER_ACTION_TICKET_START = "TICKET_START";
        public const string TRIGGER_ACTION_TICKET_BEFORE_OVERDUE = "TICKET_BEFORE_OVERDUE";
        public const string TRIGGER_ACTION_TICKET_ESCALATE = "TICKET_ESCALATE";
        public const string TRIGGER_ACTION_TICKET_ESCALATE_MANUALLY = "TICKET_ESCALATE_MANUALLY";
        public const string TRIGGER_ACTION_TICKET_UPDATE_COMMENT = "UPDATE_COMMENT";
        public const string TRIGGER_ACTION_TICKET_CUSTOMER_CALLBACK = "TICKET_CUSTOMER_CALLBACK";
        public const string TRIGGER_ACTION_TICKET_STATUS_AUTO_UPDATE = "TICKET_STATUS_AUTO_UPDATE";

        private DBService dbService = new DBService();

        private static TriggerService _instance;
        public static TriggerService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TriggerService();
            }
            return _instance;
        }

        #region Private Method
        private void PostToAPI(string url, PostTrigger param)
        {
            string json = JsonConvert.SerializeObject(param);

            using (var wb = new WebClient())
            {
                Uri uri = new Uri(url);                
                wb.Headers[HttpRequestHeader.Authorization] = "Bearer " + TRIGGER_TOKEN_KEY;
                string res = wb.UploadString(uri, "POST", json);
            }
        }

        private void StartTrigger(string transactionId, string postingType, string documentType, string documentNo, string fiscalYear, string targetTime, string action, string createdBy)
        {
            if (!string.IsNullOrEmpty(TRIGGER_POST_URL) && !string.IsNullOrEmpty(TRIGGER_RESPONSE_URL))
            {
                string sid = ERPWebConfig.GetSID();
                string company = ERPWebConfig.GetCompany();

                string sendingDateTime = Validation.getCurrentServerStringDateTime();

                string sql = @"INSERT INTO [dbo].[ERPW_TRIGGER_STATUS]
                                      ([SID]
                                      ,[CompanyCode]
                                      ,[TransactionID]
                                      ,[PostingType]
                                      ,[DocumentType]
                                      ,[DocumentNo]
                                      ,[FiscalYear]
                                      ,[SendingDateTime]
                                      ,[TargetTime]
                                      ,[TriggerAction]
                                      ,[TriggerStartDateTime]
                                      ,[TriggerResponseDateTime]
                                      ,[TriggerStatus]
                                      ,[Created_By]
                                      ,[Created_On])
                                VALUES
                                      ('" + sid + @"'
                                      ,'" + company + @"'
                                      ,'" + transactionId + @"'
                                      ,'" + postingType + @"'
                                      ,'" + documentType + @"'
                                      ,'" + documentNo + @"'
                                      ,'" + fiscalYear + @"'
                                      ,'" + sendingDateTime + @"'
                                      ,'" + targetTime + @"'
                                      ,'" + action + @"'
                                      ,''
                                      ,''
                                      ,'" + TRIGGER_STATUS_START + @"'
                                      ,'" + createdBy + @"'
                                      ,'" + sendingDateTime + @"')";

                dbService.executeSQLForFocusone(sql);

                new Thread(() =>
                {
                    try
                    {
                        PostTrigger en = new PostTrigger();
                        en.transectionID = transactionId;
                        en.targetTime = targetTime;
                        en.url = TRIGGER_RESPONSE_URL;
                        //en.targetMode = action;

                        PostToAPI(TRIGGER_POST_URL, en);

                    }
                    catch (Exception ex)
                    {
                        using (EventLog eventLog = new EventLog("Application"))
                        {
                            eventLog.Source = "Application";
                            eventLog.WriteEntry("Error : Trigger Service  = " + ex.Message, EventLogEntryType.Error, 101, 1);
                        }
                    }
                }).Start();

            }
        }

        private void SendDataTrigger(string transactionId, string postingType, string documentType, string documentNo, string fiscalYear, string targetTime, string action, string createdBy)
        {
            if (!string.IsNullOrEmpty(TRIGGER_POST_URL) && !string.IsNullOrEmpty(TRIGGER_RESPONSE_URL))
            {
                string sid = ERPWebConfig.GetSID();
                string company = ERPWebConfig.GetCompany();

                string sendingDateTime = Validation.getCurrentServerStringDateTime();

                string sql = @"INSERT INTO [dbo].[ERPW_TRIGGER_STATUS]
                                      ([SID]
                                      ,[CompanyCode]
                                      ,[TransactionID]
                                      ,[PostingType]
                                      ,[DocumentType]
                                      ,[DocumentNo]
                                      ,[FiscalYear]
                                      ,[SendingDateTime]
                                      ,[TargetTime]
                                      ,[TriggerAction]
                                      ,[TriggerStartDateTime]
                                      ,[TriggerResponseDateTime]
                                      ,[TriggerStatus]
                                      ,[Created_By]
                                      ,[Created_On])
                                VALUES
                                      ('" + sid + @"'
                                      ,'" + company + @"'
                                      ,'" + transactionId + @"'
                                      ,'" + postingType + @"'
                                      ,'" + documentType + @"'
                                      ,'" + documentNo + @"'
                                      ,'" + fiscalYear + @"'
                                      ,'" + sendingDateTime + @"'
                                      ,'" + targetTime + @"'
                                      ,'" + action + @"'
                                      ,''
                                      ,''
                                      ,'" + TRIGGER_STATUS_START + @"'
                                      ,'" + createdBy + @"'
                                      ,'" + sendingDateTime + @"')";

                dbService.executeSQLForFocusone(sql);

                new Thread(() =>
                {
                    try
                    {
                        PostTrigger en = new PostTrigger();
                        en.transectionID = transactionId;
                        en.targetTime = targetTime;
                        en.url = TRIGGER_RESPONSE_URL;
                        //en.targetMode = action;

                        PostToAPI(TRIGGER_POST_URL, en);

                    }
                    catch (Exception ex)
                    {
                        using (EventLog eventLog = new EventLog("Application"))
                        {
                            eventLog.Source = "Application";
                            eventLog.WriteEntry("Error : Trigger Service  = " + ex.Message, EventLogEntryType.Error, 101, 1);
                        }
                    }
                }).Start();

            }
        }

        #endregion

        public void UpdateTrigger(string transactionId, string triggerStart, string triggerStop)
        {
            string sid = ERPWebConfig.GetSID();
            string company = ERPWebConfig.GetCompany();

            if (triggerStart.Length == 25)
            {
                triggerStart = triggerStart.Substring(0, 19);
            }

            if (triggerStop.Length == 25)
            {
                triggerStop = triggerStop.Substring(0, 19);
            }

            DateTime startDate = new DateTime();
            DateTime.TryParseExact(triggerStart, "yyyy-MM-dd'T'HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out startDate);

            DateTime stopDate = new DateTime();
            DateTime.TryParseExact(triggerStop, "yyyy-MM-dd'T'HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out stopDate);

            //Convert utc to local time
            //startDate = startDate.ToLocalTime();
            //stopDate = stopDate.ToLocalTime();

            string startDateTime = Validation.Convert2DateTimeDB(startDate.ToString("dd/MM/yyyy HH:mm:ss"));
            string endDateTime = Validation.Convert2DateTimeDB(stopDate.ToString("dd/MM/yyyy HH:mm:ss"));

            string sql = @"UPDATE [dbo].[ERPW_TRIGGER_STATUS] 
                                    SET [TriggerStartDateTime] = '" + startDateTime + @"'
                                       ,[TriggerResponseDateTime] = '" + endDateTime + @"'
                                       ,[TriggerStatus] = '" + TRIGGER_STATUS_FINISH + @"'
                                    WHERE SID = '" + sid + @"' 
                                      AND CompanyCode = '" + company + @"' 
                                      AND TransactionID = '" + transactionId + "'";

            dbService.executeSQLForFocusone(sql);
        }

        public void CancelTrigger(string transactionId)
        {
            string sid = ERPWebConfig.GetSID();
            string company = ERPWebConfig.GetCompany();

            string sql = @"UPDATE [dbo].[ERPW_TRIGGER_STATUS] 
                                    SET [TriggerStatus] = '" + TRIGGER_STATUS_CANCEL + @"'
                                    WHERE SID = '" + sid + @"' 
                                      AND CompanyCode = '" + company + @"' 
                                      AND TransactionID = '" + transactionId + "'";

            dbService.executeSQLForFocusone(sql);
        }

        public void CancelTriggerCI(string TransactionId, string DocumentType, string EquipmentNo)
        {
            if (!string.IsNullOrEmpty(TRIGGER_POST_URL) && !string.IsNullOrEmpty(TRIGGER_RESPONSE_URL))
            {
                string sid = ERPWebConfig.GetSID();
                string company = ERPWebConfig.GetCompany();

                string sql = @"UPDATE [dbo].[ERPW_TRIGGER_STATUS] 
                                    SET [TriggerStatus] = '" + TRIGGER_STATUS_CANCEL + @"'
                                    WHERE SID = '" + sid + @"' 
                                      AND CompanyCode = '" + company + @"'
                                      AND TransactionID != '" + TransactionId + @"'
                                      AND DocumentType = '" + DocumentType + @"'
                                      AND DocumentNo = '" + EquipmentNo + @"'
                                      AND TriggerStatus = '" + TRIGGER_STATUS_START + "'";

                dbService.executeSQLForFocusone(sql);
            }
        }

        public ERPW_TRIGGER_STATUS GetTriggerData(string transactionId)
        {
            string sid = ERPWebConfig.GetSID();
            string company = ERPWebConfig.GetCompany();

            DataTable dt = dbService.selectDataFocusone("SELECT * FROM ERPW_TRIGGER_STATUS WHERE SID = '" + sid + "' AND CompanyCode = '" + company + "' AND TransactionID = '" + transactionId + "'");

            string json = JsonConvert.SerializeObject(dt);

            ERPW_TRIGGER_STATUS en = JsonConvert.DeserializeObject<ERPW_TRIGGER_STATUS>(json.TrimStart('[').TrimEnd(']'));

            return en;
        }

        public void StartTicket(string transactionId, string ticketType, string ticketNo, string fiscalYear, string targetTime, string createdBy)
        {
            SendDataTrigger(transactionId, ServiceLibrary.BUSINESS_SERVICE_CALL, ticketType, ticketNo, fiscalYear, targetTime, TRIGGER_ACTION_TICKET_BEFORE_OVERDUE, createdBy);
        }

        public void EscalateTicket(string transactionId, string ticketType, string ticketNo, string fiscalYear, string targetTime, string createdBy)
        {
            StartTrigger(transactionId, ServiceLibrary.BUSINESS_SERVICE_CALL, ticketType, ticketNo, fiscalYear, targetTime, TRIGGER_ACTION_TICKET_ESCALATE, createdBy);
        }
        // ======================================   create trigger for alert email ====================================================================================
        public void AlertEmail(string transactionId, string ticketType, string ticketNo, string fiscalYear, string targetTime, string createdBy)
        {
            StartTrigger(transactionId, ServiceLibrary.BUSINESS_SERVICE_CALL, ticketType, ticketNo, fiscalYear, targetTime, TRIGGER_ACTION_TICKET_START, createdBy);
        }
        //==============================================================================================================================================================
        public void ManuallyEscalateTicket(string transactionId)
        {
            new Thread(() =>
            {
                try
                {
                    PostTrigger en = new PostTrigger();
                    en.transectionID = TRIGGER_ACTION_TICKET_ESCALATE_MANUALLY + "|" + transactionId;
                    en.targetTime = "1";
                    en.url = TRIGGER_RESPONSE_URL;
                    //en.targetMode = TRIGGER_ACTION_TICKET_UPDATE_COMMENT;

                    PostToAPI(TRIGGER_POST_URL, en);

                }
                catch (Exception ex)
                {
                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry("Error : Trigger Service  = " + ex.Message, EventLogEntryType.Error, 101, 1);
                    }
                }
            }).Start();
        }

        public void CommentTicket(string transactionId)
        {
            new Thread(() =>
            {
                try
                {
                    PostTrigger en = new PostTrigger();
                    en.transectionID = TRIGGER_ACTION_TICKET_UPDATE_COMMENT + "|" + transactionId;
                    en.targetTime = "1";
                    en.url = TRIGGER_RESPONSE_URL;
                    //en.targetMode = TRIGGER_ACTION_TICKET_UPDATE_COMMENT;

                    PostToAPI(TRIGGER_POST_URL, en);

                }
                catch (Exception ex)
                {
                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry("Error : Trigger Service  = " + ex.Message, EventLogEntryType.Error, 101, 1);
                    }
                }
            }).Start();
            //StartTrigger("transactionId", ServiceLibrary.BUSINESS_SERVICE_CALL, "x", "ticketNo", "2019", "1", TRIGGER_ACTION_TICKET_UPDATE_COMMENT, "focusone");

//            if (!string.IsNullOrEmpty(TRIGGER_POST_URL) && !string.IsNullOrEmpty(TRIGGER_RESPONSE_URL))
//            {
//                string sid = ERPWebConfig.GetSID();
//                string company = ERPWebConfig.GetCompany();

//                string sendingDateTime = Validation.getCurrentServerStringDateTime();

//                string sql = @"INSERT INTO [dbo].[ERPW_TRIGGER_STATUS]
//                                      ([SID]
//                                      ,[CompanyCode]
//                                      ,[TransactionID]
//                                      ,[PostingType]
//                                      ,[DocumentType]
//                                      ,[DocumentNo]
//                                      ,[FiscalYear]
//                                      ,[SendingDateTime]
//                                      ,[TargetTime]
//                                      ,[TriggerAction]
//                                      ,[TriggerStartDateTime]
//                                      ,[TriggerResponseDateTime]
//                                      ,[TriggerStatus]
//                                      ,[Created_By]
//                                      ,[Created_On])
//                                VALUES
//                                      ('" + sid + @"'
//                                      ,'" + company + @"'
//                                      ,'" + "" + @"'
//                                      ,'" + postingType + @"'
//                                      ,'" + documentType + @"'
//                                      ,'" + documentNo + @"'
//                                      ,'" + fiscalYear + @"'
//                                      ,'" + sendingDateTime + @"'
//                                      ,'" + targetTime + @"'
//                                      ,'" + action + @"'
//                                      ,''
//                                      ,''
//                                      ,'" + TRIGGER_STATUS_START + @"'
//                                      ,'" + createdBy + @"'
//                                      ,'" + sendingDateTime + @"')";

//                dbService.executeSQLForFocusone(sql);

//                new Thread(() =>
//                {
//                    try
//                    {
//                        PostTrigger en = new PostTrigger();
//                        en.transectionID = "";
//                        en.targetTime = "1";
//                        en.url = TRIGGER_RESPONSE_URL;

//                        PostToAPI(TRIGGER_POST_URL, en);

//                    }
//                    catch (Exception ex)
//                    {
//                        using (EventLog eventLog = new EventLog("Application"))
//                        {
//                            eventLog.Source = "Application";
//                            eventLog.WriteEntry("Error : Trigger Service  = " + ex.Message, EventLogEntryType.Error, 101, 1);
//                        }
//                    }
//                }).Start();
//            }
        }

        public void UpdateTicketStatus(string transactionId, string ticketType, string ticketNo, string fiscalYear, string targetTime, string createdBy)
        {
            StartTrigger(transactionId, ServiceLibrary.BUSINESS_SERVICE_CALL, ticketType, ticketNo, fiscalYear, targetTime, TRIGGER_ACTION_TICKET_STATUS_AUTO_UPDATE, createdBy);
        }

        #region method for New EndDateTime [stop timer]
        public void updateDataTriggerEscalateAndBeforeOverdue(string sid, string companyCode, string transactionID, string triggerStatus)
        {
            string sql = @"
                update ERPW_TRIGGER_STATUS 
                set TriggerStatus = '" + triggerStatus + @"'
                where SID = '" + sid + @"' 
                AND CompanyCode = '" + companyCode + @"'
                AND (TransactionID = '" + transactionID + @"' or TransactionID = '" + transactionID + "befOverdue')";
            dbService.executeSQLForFocusone(sql);
        }
        #endregion
    }

    [Serializable]
    public class PostTrigger
    {
        public string transectionID { get; set; }
        public string targetTime { get; set; }
        public string url { get; set; }
        //public string targetMode { get; set; }
    }

    [Serializable]
    public class ResponseTrigger
    {
        public string TransectionID { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }
    }

    [Serializable]
    public class ERPW_TRIGGER_STATUS
    {
        public string SID { get; set; }
        public string CompanyCode { get; set; }
        public string TransactionID { get; set; }
        public string PostingType { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string FiscalYear { get; set; }
        public string SendingDateTime { get; set; }
        public string TargetTime { get; set; }
        public string TriggerAction { get; set; }
        public string TriggerStartDateTime { get; set; }
        public string TriggerResponseDateTime { get; set; }
        public string TriggerStatus { get; set; }
        public string Created_By { get; set; }
        public string Created_On { get; set; }
    }
}