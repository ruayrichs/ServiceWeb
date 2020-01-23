using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using Newtonsoft.Json;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Master.Entity;
using ERPW.Lib.Service.Entity;
using System.Threading;

namespace ServiceWeb.Service
{
    public class TierZeroService
    {
        private DBService dbService = new Agape.Lib.DBService.DBService();
        private static TierZeroService _instance;
        private DataTable dtData;

        public String WorkGroupCode
        {
            get
            {
                return "20170121162748444411";
            }
        }

        private string _EmployeeCode;
        private string EmployeeCode
        {
            get
            {
                if (string.IsNullOrEmpty(_EmployeeCode))
                    _EmployeeCode = !string.IsNullOrEmpty(ERPWAuthentication.EmployeeCode) ? ERPWAuthentication.EmployeeCode : ""; // "EMP010000003";
                return _EmployeeCode;
            }
        }

        private string _UserName;
        private string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_UserName))
                    _UserName = !string.IsNullOrEmpty(ERPWAuthentication.UserName) ? ERPWAuthentication.UserName : ""; // "focusone";
                return _UserName;
            }
        }

        private string _FullNameEN;
        private string FullNameEN
        {
            get
            {
                if (string.IsNullOrEmpty(_FullNameEN))
                    _FullNameEN = !string.IsNullOrEmpty(ERPWAuthentication.FullNameEN) ? ERPWAuthentication.FullNameEN : ""; // "Focus One Administrator";
                return _FullNameEN;
            }
        }

        public static TierZeroService getInStance()
        {
            if (_instance == null)
            {
                _instance = new TierZeroService();
            }
            return _instance;
        }

        //        private List<TierZero> getTierZeroList(string sid, string CompanyCode, string Status)
        //        {
        //            string sql = @"SELECT [SID]
        //                          ,[CompanyCode]
        //                          ,[SEQ]
        //                          ,[Channel]
        //                          ,[EMail]
        //                          ,[CustomerCode]
        //                          ,[CustomerName]
        //                          ,[TelNo]
        //                          ,[Subject]
        //                          ,[Detail]
        //                          ,[Status]
        //                          ,[TicketNumber]
        //                          ,[TicketType]
        //                          ,[CREATED_BY]
        //                          ,[CREATED_ON]
        //                          ,[UPDATED_BY]
        //                          ,[UPDATED_ON]
        //                          ,EquipmentCode
        //                      FROM [ERPW_Service_tier0]
        //                    where sid='" + sid + "' AND CompanyCode = '" + CompanyCode + "'";
        //            if (!string.IsNullOrEmpty(Status))
        //            {
        //                sql += " AND Status = '" + Status + "'";
        //            }
        //            DataTable dt = dbService.selectDataFocusone(sql);
        //            List<TierZero> momaster = JsonConvert.DeserializeObject<List<TierZero>>(JsonConvert.SerializeObject(dt));
        //            return momaster;
        //        }

        //        public TierZeroEn getTierZeroDetail(string sid, string CompanyCode, string SEQ)
        //        {
        //            TierZeroEn Result = new TierZeroEn();
        //            string sql = @"SELECT [SID]
        //                          ,[CompanyCode]
        //                          ,[SEQ]
        //                          ,[Channel]
        //                          ,[EMail]
        //                          ,[CustomerCode]
        //                          ,[CustomerName]
        //                          ,[TelNo]
        //                          ,[Subject]
        //                          ,[Detail]
        //                          ,[Status]
        //                          ,[TicketNumber]
        //                          ,[TicketType]
        //                          ,[CREATED_BY]
        //                          ,[CREATED_ON]
        //                          ,[UPDATED_BY]
        //                          ,[UPDATED_ON]
        //                          ,EquipmentCode
        //                      FROM [ERPW_Service_tier0]
        //                    where sid='" + sid + "' AND CompanyCode = '" + CompanyCode + "' AND SEQ =" + SEQ;
        //            DataTable dt = dbService.selectDataFocusone(sql);
        //            List<TierZeroEn> momaster = JsonConvert.DeserializeObject<List<TierZeroEn>>(JsonConvert.SerializeObject(dt));
        //            if (momaster.Count > 0)
        //            {
        //                Result = momaster.First();
        //            }
        //            return Result;
        //        }

        //        public void UpdateTierZeroStatus(string SID, string Companycode, string SEQ, string Value)
        //        {
        //            string DBDate = Validation.getCurrentServerDate();
        //            string UserName = ERPWAuthentication.UserName;

        //            string sql = @"UPDATE ERPW_Service_tier0
        //                           SET Status = '" + Value + @"'
        //                              ,UPDATED_BY = '" + UserName + @"'
        //                              ,UPDATED_ON = '" + DBDate + @"'
        //                         WHERE SID = '" + SID + @"'
        //                        AND CompanyCode = '" + Companycode + @"'
        //                        AND SEQ = " + SEQ;
        //            dbService.executeSQLForFocusone(sql);
        //        }

        //        public string InsertTierZeroItem(string SID, string Companycode, TierZero Value)
        //        {
        //            string DBDate = Validation.getCurrentServerDate();
        //            string UserName = ERPWAuthentication.UserName;
        //            //string UserName = "Focusone";
        //            int SEQ = countRowData(SID, Companycode) + 1;
        //            string sql = @"INSERT INTO ERPW_Service_tier0
        //                           (
        //                                SID
        //                               ,CompanyCode
        //                               ,SEQ
        //                               ,Channel
        //                               ,EMail
        //                               ,CustomerCode
        //                               ,CustomerName
        //                               ,TelNo
        //                               ,Subject
        //                               ,Detail
        //                               ,Status
        //                               ,TicketNumber
        //                               ,TicketType
        //                               ,CREATED_BY
        //                               ,CREATED_ON
        //                               ,UPDATED_BY
        //                               ,UPDATED_ON
        //                               ,EquipmentCode
        //                            )
        //                            VALUES 
        //                            (
        //                                '" + SID+@"'
        //                                ,'" + Companycode + @"'
        //                                ," + SEQ + @"
        //                                ,'" + Value.Channel + @"'
        //                                ,'" + Value.EMail + @"'
        //                                ,'" + Value.CustomerCode + @"'
        //                                ,'" + Value.CustomerName + @"'
        //                                ,'" + Value.TelNo + @"'
        //                                ,'" + Value.Subject + @"'
        //                                ,'" + Value.Detail + @"'
        //                                ,'" + Value.Status + @"'
        //                                ,'" + Value.TicketNumber + @"'
        //                                ,'" + Value.TicketType + @"'
        //                                ,'" + ERPWAuthentication.UserName + @"'
        //                                ,'" + Validation.Convert2DateDB(Validation.getCurrentServerDate()) + @"'
        //                                ,''
        //                                ,''
        //                                ,'" + Value.EquipmentCode + @"'
        //                            )";
        //            dbService.executeSQLForFocusone(sql);
        //            return SEQ.ToString();
        //        }

        //        public int countRowData(string Sid, string Companycode)
        //        {
        //            int result = 0;
        //            string sql = @"SELECT MAX(seq) as Result FROM ERPW_Service_tier0
        //                    where sid='" + Sid + "' AND CompanyCode = '" + Companycode + "'";
        //            DataTable dt = dbService.selectDataFocusone(sql);
        //            if (dt.Rows.Count > 0)
        //            {
        //                int.TryParse(dt.Rows[0]["Result"].ToString(), out result);
        //            }
        //            return result;
        //        }

        //        public static string FormatChannel(string strChannel)
        //        {
        //            switch (strChannel)
        //            {
        //                case "1": return "E-Mail";
        //                case "2": return "Web";
        //                case "3": return "System";
        //                default: return "System";
        //            }
        //        }

        //        public static string FormatStatus(string strStatus)
        //        {
        //            switch (strStatus)
        //            {
        //                case "0": return "Open";
        //                case "1": return "Closed with Completed";
        //                case "2": return "Closed with Cancel";
        //                case "3": return "Ticket Created";
        //                default: return "Open";
        //            }
        //        }

        private string AssignWork_SLA(string SID, string CompanyCode, string ticketType, string ticketNo, string ticketYear, string remark,
            string TierZeroChannel, string equipmentNo, string OwnerGroupCode)
        {
            TierService tierService = TierService.getInStance();

            string ticketCode = "";
            string SLAGroupCode = "";
            //string OwnerGroupCode = "";

            DataTable dtSLAConfig = ERPW.Lib.Master.Config.MasterConfigLibrary.GetInstance().GetMasterConfigImpactGetSLA(
                SID, TierZeroChannel
            );

            SLAGroupCode = dtSLAConfig.Rows[0]["SLA_GroupCode"].ToString();

            DataTable dtTier = tierService.searchTierMaster(
                SID,
                WorkGroupCode,
                "", "",
                SLAGroupCode
            );

            dtTier.DefaultView.RowFilter = "PriorityCode = '" + dtSLAConfig.Rows[0]["Priority"].ToString() + "'";
            dtTier = dtTier.DefaultView.ToTable();
            dtTier.DefaultView.RowFilter = string.Empty;

            dtTier = AfterSaleService.getInstance().getTierOperation(
                SID, dtTier.Rows[0]["tierCode"].ToString(), ""
            );

            if (dtTier.Rows.Count > 0)
            {
                string tierCode = dtTier.Rows[0]["TierCode"].ToString();
                string tier = dtTier.Rows[0]["Tier"].ToString();
                string tierDesc = dtTier.Rows[0]["TierDescription"].ToString();
                string Resolution = dtTier.Rows[0]["Resolution"].ToString();
                string Requester = dtTier.Rows[0]["Requester"].ToString();
                double resolutionTime = 0;

                double.TryParse(Resolution, out resolutionTime);

                double requesterTime = 0;

                double.TryParse(Requester, out requesterTime);

                double allResolutionTime = 0;

                for (int i = 0; i < dtTier.Rows.Count; i++)
                {
                    allResolutionTime += Convert.ToDouble(dtTier.Rows[i]["Resolution"].ToString());
                }


                ticketCode = AfterSaleService.getInstance().StartTicket(
                    SID, CompanyCode,
                    ticketType, ticketNo, ticketYear,
                    tierCode, tier, tierDesc, resolutionTime,
                    requesterTime,
                    OwnerGroupCode,
                    equipmentNo,
                    remark,
                    UserName, EmployeeCode, FullNameEN,
                    true,
                    SLAGroupCode
                );


                new Thread(() =>
                {
                    try
                    {
                        ERPW.Lib.Service.SnapDataLibrary.getInstance().saveDataSnap(
                            SID,
                            CompanyCode,
                            ticketCode,
                            ERPW.Lib.Service.SnapDataLibrary.SNAP_EVENT_OBJECT_OPEN,
                            SLAGroupCode,
                            OwnerGroupCode,
                            "",
                            EmployeeCode
                        );
                    }
                    catch (Exception ex)
                    {

                    }
                }).Start();
            }

            return ticketCode;
        }

        public ResultAutoCreateTicket AutoCreatedTicketFormTierZero(string SessionID, string SID, string CompanyCode,
            TierZeroEn Data, string customerCode, string createdBy)
        {
            ResultAutoCreateTicket Result = new ResultAutoCreateTicket();
            ERPW.Lib.F1WebService.ICMUtils.ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
            tmpServiceCallDataSet serviceCallEntity = new tmpServiceCallDataSet();

            #region default Data
            string currDate = Validation.getCurrentServerStringDateTime().Substring(0, 8);
            string currTime = Validation.getCurrentServerStringDateTime().Substring(8, 6);
            string Fiscalyear = currDate.Substring(0, 4);
            string ticketNo = "";
            string ContractName = "";
            string ticketType = "";
            string Priority = "";

            List<string> conditionSelectContact = new List<string>();
            if (!string.IsNullOrEmpty(Data.EMail))
                conditionSelectContact.Add(" email like '%" + Data.EMail + "%' ");
            if (!string.IsNullOrEmpty(Data.TelNo))
                conditionSelectContact.Add(" phone like '%" + Data.TelNo + "%' ");

            if (conditionSelectContact.Count > 0)
            {
                DataTable dtContactDetail = ERPW.Lib.Master.CustomerService.getInstance().getListContactDetailByCustomer(
                    SID,
                    CompanyCode,
                    customerCode,
                    false
                );

                dtContactDetail.DefaultView.RowFilter = string.Join(" or ", conditionSelectContact);
                DataTable dtContactDetailTemp = dtContactDetail.DefaultView.ToTable();
                if (dtContactDetailTemp.Rows.Count > 0)
                {
                    ContractName = Convert.ToString(dtContactDetailTemp.Rows[0]["NAME1"]);
                }
            }

            DataTable dtDoctypeType = AfterSaleService.getInstance().getSearchDoctype(
                "",
                CompanyCode,
                ERPW.Lib.Service.ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_INCIDENT,
                SID
            );
            if (dtDoctypeType.Rows.Count > 0)
            {
                ticketType = Convert.ToString(dtDoctypeType.Rows[0]["DocumentTypeCode"]);
            }

            DataTable dtSLAConfig = ERPW.Lib.Master.Config.MasterConfigLibrary.GetInstance().GetMasterConfigImpactGetSLA(
                SID, Data.Channel
            );
            if (dtSLAConfig.Rows.Count > 0)
            {
                Priority = dtSLAConfig.Rows[0]["Priority"].ToString();
            }

            #region header
            string objectId = SID + ticketType + Fiscalyear + "0001";

            DataRow drHeaderNew = serviceCallEntity.cs_servicecall_header.NewRow();
            drHeaderNew["sid"] = SID;
            drHeaderNew["CompanyCode"] = CompanyCode;
            drHeaderNew["CustomerCode"] = customerCode;
            drHeaderNew["ContractPersonName"] = ContractName;
            drHeaderNew["ContractPersonTel"] = "";
            drHeaderNew["Email"] = "";
            drHeaderNew["Address"] = "";
            drHeaderNew["CallerID"] = "";
            drHeaderNew["ObjectID"] = objectId;
            drHeaderNew["DocType"] = ticketType;
            drHeaderNew["Fiscalyear"] = Fiscalyear;
            drHeaderNew["DOCDATE"] = currDate;
            drHeaderNew["CallStatus"] = ERPW.Lib.Service.ServiceTicketLibrary.SERVICE_CALL_STATUS_OPEN;
            drHeaderNew["HeaderText"] = Data.Subject;
            drHeaderNew["Impact"] = ""; // "02";   // 02 : Medium      // Hard Code
            drHeaderNew["Urgency"] = ""; // "02";  // 02 : Medium      // Hard Code
            drHeaderNew["Priority"] = Priority; // "03"; // 03 : Medium (P3) // Hard Code
            drHeaderNew["ProjectCode"] = "";
            drHeaderNew["ProjectElement"] = "";
            drHeaderNew["ReferenceDocument"] = "";
            drHeaderNew["MajorIncident"] = false.ToString();
            drHeaderNew["CallbackDate"] = "";
            drHeaderNew["CallbackTime"] = "";
            drHeaderNew["CREATED_BY"] = createdBy;
            drHeaderNew["CREATED_ON"] = currDate + currTime;
            serviceCallEntity.cs_servicecall_header.Rows.Add(drHeaderNew);
            #endregion

            #region item
            DataRow drItemNew = serviceCallEntity.cs_servicecall_item.NewRow();
            drItemNew["SID"] = SID;
            drItemNew["CompanyCode"] = CompanyCode;
            drItemNew["CustomerCode"] = serviceCallEntity.cs_servicecall_header.Rows[0]["CustomerCode"].ToString();
            drItemNew["DocType"] = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
            drItemNew["Fiscalyear"] = serviceCallEntity.cs_servicecall_header.Rows[0]["Fiscalyear"].ToString();
            drItemNew["CallerID"] = serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString();
            drItemNew["ObjectID"] = serviceCallEntity.cs_servicecall_header.Rows[0]["ObjectID"].ToString();
            drItemNew["xLineNo"] = "001";
            drItemNew["BObjectID"] = drItemNew["ObjectID"] + "001";
            drItemNew["CloseStatus"] = "01";
            drItemNew["CreatedOnDate"] = currDate;
            drItemNew["CreatedOnTime"] = currTime;
            drItemNew["ProblemDate"] = currDate;
            drItemNew["ProblemTime"] = currTime;
            drItemNew["CREATED_BY"] = createdBy;
            drItemNew["CREATED_ON"] = currDate + currTime;
            drItemNew["RefContractNo"] = "";
            drItemNew["ClosedOnDate"] = "";
            drItemNew["ClosedOnTime"] = "";
            drItemNew["EndDate"] = "";
            drItemNew["AssignDate"] = "";
            drItemNew["AssignTime"] = "";
            drItemNew["ResponseOnDate"] = "";
            drItemNew["ResponseOnTime"] = "";
            drItemNew["ResolutionOnDate"] = "";
            drItemNew["ResolutionOnTime"] = "";
            drItemNew["TechnicianDate"] = "";
            drItemNew["TechnicianTime"] = "";
            drItemNew["IncidentArea"] = "";
            drItemNew["ItemCode"] = "";
            drItemNew["MaterialDesc"] = "";
            drItemNew["EquipmentDesc"] = "";
            drItemNew["SerialNo"] = "";
            drItemNew["ProblemGroup"] = "";
            drItemNew["ProblemTypeCode"] = "";
            drItemNew["OriginCode"] = "";
            drItemNew["CallTypeCode"] = "";
            drItemNew["QueueOption"] = Data.OwnerService;
            drItemNew["SummaryProblem"] = "";
            drItemNew["SummaryCause"] = "";
            drItemNew["SummaryResolution"] = "";
            drItemNew["IncidentArea"] = "";
            serviceCallEntity.cs_servicecall_item.Rows.Add(drItemNew);

            string equipmentCode = Data.EquipmentNo;
            string equipmentDesc = "";
            string materialCode = "";
            string materialDesc = "";

            if (equipmentCode != "")
            {
                DataTable EQInfo = AfterSaleService.getInstance().getSearchEQInfo(SID, CompanyCode, "", equipmentCode);

                if (EQInfo.Rows.Count > 0)
                {
                    equipmentDesc = Convert.ToString(EQInfo.Rows[0]["EquipmentName"]);
                    materialCode = Convert.ToString(EQInfo.Rows[0]["MaterialNo"]);
                    materialDesc = Convert.ToString(EQInfo.Rows[0]["MaterialName"]);
                }
            }

            DataRow dr = serviceCallEntity.cs_servicecall_item.Rows[0];
            dr["ItemCode"] = materialCode;
            dr["MaterialDesc"] = materialDesc;
            dr["EquipmentNo"] = equipmentCode;
            dr["EquipmentDesc"] = equipmentDesc;
            dr["Remark"] = Data.Detail;

            #endregion

            #region Contact Header
            DataTable dtContactHeader = serviceCallEntity.cs_servicecall_Contactdetail_Header;
            DataRow drContactHeader = dtContactHeader.NewRow();
            drContactHeader["sid"] = SID;
            drContactHeader["ObjectID"] = serviceCallEntity.cs_servicecall_header.Rows[0]["ObjectID"].ToString();
            dtContactHeader.Rows.Add(drContactHeader);
            #endregion
            #endregion

            DataSet objReturn = new DataSet();
            string returnCode = "";
            string returnMessage = "";

            Object[] objParam = new Object[] { "1500138", SessionID };
            DataSet[] objDataSet = new DataSet[] { serviceCallEntity };
            objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
            if (objReturn != null && objReturn.Tables.Count > 0)
            {
                returnCode = (objReturn.Tables["MessageResult"] == null || objReturn.Tables["MessageResult"].Rows.Count <= 0)
                    ? "E" : objReturn.Tables["MessageResult"].Rows[0]["code"].ToString();

                returnMessage = (objReturn.Tables["MessageResult"] == null || objReturn.Tables["MessageResult"].Rows.Count <= 0)
                    ? "" : objReturn.Tables["MessageResult"].Rows[0]["Message"].ToString();

                if ("E".Equals(returnCode))
                {
                    string message = "Save Error : " + returnMessage;
                    Result.CreatedSuccess = false;
                    Result.ResultMessage = message;
                    Result.TicketNo = "";
                    Result.TicketNoDisplay = "";
                    Result.TicketType = ticketType;
                    Result.Fiscalyear = Fiscalyear;
                }
                else
                {
                    ticketNo = returnMessage.Trim();

                    string TicketNoDisplay = AfterSaleService.getInstance().GetTicketNoForDisplay(
                        SID, CompanyCode, ticketType, ticketNo
                    );

                    string ObjectLink = AssignWork_SLA(
                        SID, CompanyCode, ticketType, ticketNo, Fiscalyear, Data.Subject, Data.Channel, Data.EquipmentNo, Data.OwnerService
                     );

                    Result.TicketNo = ticketNo;
                    Result.TicketNoDisplay = TicketNoDisplay;
                    Result.TicketType = ticketType;
                    Result.Fiscalyear = Fiscalyear;
                    Result.ObjectLink = ObjectLink;
                    Result.CreatedSuccess = true;
                    Result.ResultMessage = "Auto Create Service Ticket Success, Ticket Number " + TicketNoDisplay;
                }
            }

            return Result;
        }

    }

    #region class entities structure

    //public class TierZero
    //{
    //    public string SID { get; set; }
    //    public string CompanyCode { get; set; }
    //    public string SEQ { get; set; }
    //    public string Channel { get; set; }
    //    public string EMail { get; set; }
    //    public string CustomerCode { get; set; }
    //    public string CustomerName { get; set; }
    //    public string TelNo { get; set; }
    //    public string Subject { get; set; }
    //    public string Detail { get; set; }
    //    public string Status { get; set; }
    //    public string TicketNumber { get; set; }
    //    public string TicketType { get; set; }
    //    public string EquipmentNo { get; set; }
    //    public string CREATED_BY { get; set; }
    //    public string CREATED_ON { get; set; }
    //    public string UPDATED_BY { get; set; }
    //    public string UPDATED_ON { get; set; }

    //    public string UploadType { get; set; }
    //    public string MessageUpload { get; set; }
    //    public string EquipmentCode { get; set; }
    //}

    [Serializable]
    public class ResultAutoCreateTicket
    {
        public string TicketNo { get; set; }
        public string TicketNoDisplay { get; set; }
        public string TicketType { get; set; }
        public string Fiscalyear { get; set; }
        public string ObjectLink { get; set; }
        public bool CreatedSuccess { get; set; }
        public string ResultMessage { get; set; }
    }

    #endregion
}