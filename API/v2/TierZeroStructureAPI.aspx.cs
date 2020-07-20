using agape.lib.constant;
using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Master.Entity;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using ERPW.Lib.WebConfig;
using Link.Lib.Model.Model.Timeline;
using Newtonsoft.Json;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API.v2
{
    public partial class TierZeroStructureAPI : System.Web.UI.Page
    {
        private TierZeroService tierZeroService = new TierZeroService();
        private TierZeroLibrary libTierZero = TierZeroLibrary.GetInstance();
        private TierService tierService = TierService.getInStance();
        private EquipmentService CIService = new EquipmentService();
        private ServiceTicketLibrary libServiceTicket = new ServiceTicketLibrary();
        private AppClientLibrary libAppClient = AppClientLibrary.GetInstance();
        private MasterConfigLibrary libconfig = MasterConfigLibrary.GetInstance();

        #region Member Varible

        public String WorkGroupCode
        {
            get
            {
                return "20170121162748444411";
            }
        }

        private string _SID;
        private string SID
        {
            get
            {
                if (string.IsNullOrEmpty(_SID))
                    _SID = !string.IsNullOrEmpty(ERPWAuthentication.SID) ? ERPWAuthentication.SID : ERPWebConfig.GetSID(); // "555";
                return _SID;
            }
        }

        private string _CompanyCode;
        private string CompanyCode
        {
            get
            {
                if (string.IsNullOrEmpty(_CompanyCode))
                    _CompanyCode = !string.IsNullOrEmpty(ERPWAuthentication.CompanyCode) ? ERPWAuthentication.CompanyCode : ERPWebConfig.GetCompany(); // "INET";
                return _CompanyCode;
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

        private string _SessionID;
        private string SessionID
        {
            get
            {
                if (string.IsNullOrEmpty(_SessionID))
                    _SessionID = !string.IsNullOrEmpty((string)Session[ApplicationSession.USER_SESSION_ID]) ? (string)Session[ApplicationSession.USER_SESSION_ID] : ""; // "1928297577";
                return _SessionID;
            }
        }

        private List<EquipmentService.EquipmentMappingFirstOwner> _ListEquipmentMappingFirstOwner;
        private List<EquipmentService.EquipmentMappingFirstOwner> ListEquipmentMappingFirstOwner
        {
            get
            {
                if (_ListEquipmentMappingFirstOwner == null)
                {
                    _ListEquipmentMappingFirstOwner = CIService.getListEquipmentMappingFirstOwner(
                        SID,
                        CompanyCode
                    );
                }
                return _ListEquipmentMappingFirstOwner;
            }
        }

        private TierZeroLibrary.KeywordPatternModel _KeySplit;
        private TierZeroLibrary.KeywordPatternModel KeySplit
        {
            get
            {
                if (_KeySplit == null)
                {
                    _KeySplit = new TierZeroLibrary.KeywordPatternModel();

                    var DatasKeySplit = TierZeroLibrary.GetInstance().getKeywordPatternConfig(
                        SID,
                        CompanyCode,
                        TierZeroLibrary.KEY_WORD_EVENT.SPLIT
                    );

                    if (DatasKeySplit.Count > 0)
                    {
                        _KeySplit.KeyWord = DatasKeySplit.First().KeyWord;
                        _KeySplit.KeyWordEvent = DatasKeySplit.First().KeyWordEvent;
                    }
                }
                return _KeySplit;
            }
        }

        private List<TierZeroLibrary.KeywordPatternModel> _ListKeyWordEvent_Create;
        private List<TierZeroLibrary.KeywordPatternModel> ListKeyWordEvent_Create
        {
            get
            {
                if (_ListKeyWordEvent_Create == null)
                {
                    _ListKeyWordEvent_Create = TierZeroLibrary.GetInstance().getKeywordPatternConfig(
                        SID,
                        CompanyCode,
                        TierZeroLibrary.KEY_WORD_EVENT.CREATETICKET
                    );
                }
                return _ListKeyWordEvent_Create;
            }
        }

        private List<TierZeroLibrary.KeywordPatternModel> _ListKeyWordEvent_Update;
        private List<TierZeroLibrary.KeywordPatternModel> ListKeyWordEvent_Update
        {
            get
            {
                if (_ListKeyWordEvent_Update == null)
                {
                    _ListKeyWordEvent_Update = TierZeroLibrary.GetInstance().getKeywordPatternConfig(
                        SID,
                        CompanyCode,
                        TierZeroLibrary.KEY_WORD_EVENT.UPDATETICKET
                    );
                }
                return _ListKeyWordEvent_Update;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            List<String> listResponse = new List<string>();
            Boolean LoginByPermission = false;
            if (string.IsNullOrEmpty(ERPWAuthentication.EmployeeCode))
            {
                LoginByPermission = true;
            }

            try
            {
                if (!checkPermission())
                {
                    throw new Exception("No Permission.");
                }

                Response.ContentType = "application/json";
                string Channel = GetRequestKey("Channel"); //!string.IsNullOrEmpty(Request["Channel"]) ? Request["Channel"] : Request.Headers["Channel"];
                string PermissionKey = GetRequestKey("PermissionKey"); //!string.IsNullOrEmpty(Request["PermissionKey"]) ? Request["PermissionKey"] : Request.Headers["PermissionKey"];
                string TicketType = GetRequestKey("TicketType");
                string Priority = GetRequestKey("Priority");
                string OwnerService = GetRequestKey("OwnerService");
                string RequestGroup = GetRequestKey("RequestGroup");
                string RequestType = GetRequestKey("RequestType");
                string RequestSource = GetRequestKey("RequestSource");
                string ContactSource = GetRequestKey("ContactSource");
                
                //Valid Owner & Area input
                InputDataAPI ownerInput = new InputDataAPI()
                {
                    OwnerService = OwnerService,
                    RequestGroup = RequestGroup,
                    RequestType = RequestType,
                    RequestSource = RequestSource,
                    ContactSource = ContactSource
                };
                validDataForOwnerService(ValidTicketType(TicketType), ownerInput);

                if (Channel == TierZeroLibrary.TIER_ZERO_CHANNEL_EMAIL)
                {
                    string JsonData = !string.IsNullOrEmpty(Request["JsonData"]) ? Request["JsonData"] : Request.Headers["JsonData"];
                    List<ObjectsEmailData> enData = JsonConvert.DeserializeObject<List<ObjectsEmailData>>(JsonData);

                    enData.ForEach(r =>
                    {
                        string ResponseStr = "";
                        TierZeroEn Data = new TierZeroEn();
                        Data.PermissionKey = PermissionKey;
                        Data.Channel = Channel;
                        Data.EMail = r.EmailFrom;
                        Data.CustomerCode = "";
                        Data.CustomerName = "";
                        Data.TelNo = "";
                        Data.Subject = HttpUtility.UrlDecode(r.EmailSubject);
                        Data.Detail = HttpUtility.UrlDecode(r.EmailBody);
                        Data.Status = TierZeroLibrary.TIER_ZERO_STATUS_OPEN;
                        Data.TicketNumber = "";
                        Data.TicketType = ValidTicketType(TicketType);
                        //addon
                        Data.Priority = !string.IsNullOrEmpty(Priority) ? Priority : "03";
                        Data.OwnerService = OwnerService;
                        Data.RequestGroup = RequestGroup;
                        Data.RequestType = RequestType;
                        Data.RequestSource = RequestSource;
                        Data.ContactSource = ContactSource;

                        checkPatternInform checkPattern = checkPatternInformCreated(HttpUtility.UrlDecode(r.EmailSubject));
                        if (checkPattern.isMatPattern)
                        {
                            if (checkPattern.PatternMode == ApplicationSession.CREATE_MODE_STRING)
                            {
                                Data.CustomerCode = checkPattern.CustomerCode;
                                Data.CustomerName = checkPattern.CustomerName;
                                Data.EquipmentNo = checkPattern.EquipmentCode;
                                ResponseStr = AddTierZeroItem(Data, Channel);
                            }
                            else if (checkPattern.PatternMode == ApplicationSession.CHANGE_MODE_STRING)
                            {
                                Data.CustomerCode = checkPattern.CustomerCode;
                                Data.CustomerName = checkPattern.CustomerName;
                                Data.EquipmentNo = checkPattern.EquipmentCode;
                                ResponseStr = UpdateTicketFromPatern(Data);

                            }
                            else
                            {
                                ResponseStr = AddTierZeroItem(Data, Channel);
                            }

                        }
                        else
                        {
                            ResponseStr = AddTierZeroItem(Data, Channel);
                        }

                        listResponse.Add(ResponseStr);
                    });
                }
                else
                {
                    TierZeroEn Data = new TierZeroEn();

                    Data.Channel = GetRequestKey("Channel");// !string.IsNullOrEmpty(Request["Channel"]) ? Request["Channel"] : Request.Headers["Channel"];
                    Data.PermissionKey = PermissionKey;

                    Data.EMail = GetRequestKey("EMail");//!string.IsNullOrEmpty(Request["EMail"]) ? Request["EMail"] : Request.Headers["EMail"];
                    if (Data.EMail == null) Data.EMail = "";

                    Data.CustomerCode = GetRequestKey("CustomerCode");//!string.IsNullOrEmpty(Request["CustomerCode"]) ? Request["CustomerCode"] : Request.Headers["CustomerCode"];
                    if (Data.CustomerCode == null) Data.CustomerCode = "";

                    Data.CustomerName = GetRequestKey("CustomerName");//!string.IsNullOrEmpty(Request["CustomerName"]) ? Request["CustomerName"] : Request.Headers["CustomerName"];
                    if (Data.CustomerName == null) Data.CustomerName = "";

                    Data.TelNo = GetRequestKey("TelNo");//!string.IsNullOrEmpty(Request["TelNo"]) ? Request["TelNo"] : Request.Headers["TelNo"];
                    if (Data.TelNo == null) Data.TelNo = "";

                    Data.Subject = GetRequestKey("Subject");//!string.IsNullOrEmpty(Request["Subject"]) ? Request["Subject"] : Request.Headers["Subject"];
                    if (Data.Subject == null) Data.Subject = "";
                    Data.Subject = HttpUtility.UrlDecode(Data.Subject);

                    string Detail = GetRequestKey("Detail");//!string.IsNullOrEmpty(Request["Detail"]) ? Request["Detail"] : Request.Headers["Detail"];
                    if (Detail == null) Detail = "";
                    Data.Detail = HttpUtility.UrlDecode(Detail);

                    Data.Status = TierZeroLibrary.TIER_ZERO_STATUS_OPEN;
                    Data.TicketNumber = "";
                    Data.TicketType = ValidTicketType(TicketType);
                    //addon
                    Data.Priority = !string.IsNullOrEmpty(Priority) ? Priority : "03";
                    Data.OwnerService = OwnerService;
                    Data.RequestGroup = RequestGroup;
                    Data.RequestType = RequestType;
                    Data.RequestSource = RequestSource;
                    Data.ContactSource = ContactSource;

                    Data.EquipmentNo = GetRequestKey("Product"); //!string.IsNullOrEmpty(Request["Product"]) ? Request["Product"] : Request.Headers["Product"];

                    if (Data.EquipmentNo == null)
                    {
                        Data.EquipmentNo = "";
                    }

                    Data.UploadType = GetRequestKey("uploadType"); //!string.IsNullOrEmpty(Request["uploadType"]) ? Request["uploadType"] : Request.Headers["uploadType"];
                    Data.MessageUpload = GetRequestKey("message"); //!string.IsNullOrEmpty(Request["message"]) ? Request["message"] : Request.Headers["message"];

                    Data.RoomID = GetRequestKey("RoomID"); //!string.IsNullOrEmpty(Request["RoomID"]) ? Request["RoomID"] : Request.Headers["RoomID"];

                    string ResponseStr = AddTierZeroItem(Data, Channel);
                    if (Channel == TierZeroLibrary.TIER_ZERO_CHANNEL_WEB)
                    {
                        ResultCreateTierZero enResult = JsonConvert.DeserializeObject<ResultCreateTierZero>(ResponseStr);
                        if (enResult == null && enResult.ResultTicket == null && !string.IsNullOrEmpty(enResult.ResultTicket.TicketNo))
                        {
                            string otpid = GetRequestKey("otpid");//!string.IsNullOrEmpty(Request["otpid"]) ? Request["otpid"] : Request.Headers["otpid"];
                            string ObjKeyTicketNo = enResult.ResultTicket.Fiscalyear + enResult.ResultTicket.TicketType + enResult.ResultTicket.TicketNo;
                            TierZeroLibrary.GetInstance().updateOTPUseInProcess(SID, CompanyCode, ObjKeyTicketNo, Data.EMail, Data.TelNo, otpid);
                        }
                    }
                    listResponse.Add(ResponseStr);
                }

                Response.Write("[" + string.Join(",", listResponse) + "]");
                
            }
            catch (Exception ex)
            {
                ResultCreateTierZero resultTierZero = new ResultCreateTierZero();
                resultTierZero.ResultMessage = ex.Message;
                resultTierZero.CreatedSuccess = false;
                string responseJson = JsonConvert.SerializeObject(resultTierZero);
                listResponse.Add(responseJson);
                Response.Write("[" + string.Join(",", listResponse) + "]");
            }
            finally
            {
                if (LoginByPermission)
                {
                    Session.Abandon();
                }
            }

        }

        private bool checkPermission()
        {
            bool HasPermission = false;

            try
            {
                string Channel = !string.IsNullOrEmpty(Request["Channel"]) ? Request["Channel"] : Request.Headers["Channel"];
                if (Channel != TierZeroLibrary.TIER_ZERO_CHANNEL_APPCLIENT)
                {
                    ERPW_API_Permission_Token_Key_DAO libPermission = new ERPW_API_Permission_Token_Key_DAO();
                    string PermissionKey = !string.IsNullOrEmpty(Request["PermissionKey"]) ? Request["PermissionKey"] : Request.Headers["PermissionKey"];
                    DataTable dtPermission = libPermission.getOneByKey(PermissionKey);

                    if (dtPermission.Rows.Count > 0)
                    {
                        _SID = dtPermission.Rows[0]["SID"].ToString();
                        _UserName = dtPermission.Rows[0]["UserName"].ToString();
                    }

                    //string Channel = !string.IsNullOrEmpty(Request["Channel"]) ? Request["Channel"] : Request.Headers["Channel"];
                    if (string.IsNullOrEmpty(ERPWAuthentication.UserName))
                    {
                        HasPermission = true;
                        SystemModeControlService.SystemModeEntities mode = SystemModeControlService.getInstanceMode("Link");
                        ERPWAutoLoginService loginService = new ERPWAutoLoginService(_SID, _UserName, mode);
                    }
                    else if (!string.IsNullOrEmpty(_UserName) && !string.IsNullOrEmpty(ERPWAuthentication.UserName))
                    {
                        HasPermission = true;
                    }
                    else
                    {
                        HasPermission = false;
                    }
                }
                else
                {
                    string CorpoKey = !string.IsNullOrEmpty(Request["CorporatePermissionKey"]) ? Request["CorporatePermissionKey"] : Request.Headers["CorporatePermissionKey"];
                    string AppKey = !string.IsNullOrEmpty(Request["ApplicationPermissionKey"]) ? Request["ApplicationPermissionKey"] : Request.Headers["ApplicationPermissionKey"];
                    string AppID = !string.IsNullOrEmpty(Request["ApplicationID"]) ? Request["ApplicationID"] : Request.Headers["ApplicationID"];
                    //libAppClient.chec
                    if (libAppClient.checkAuthenCreatedTicket(SID, CompanyCode, CorpoKey, AppKey, AppID))
                    {
                        HasPermission = true;



                        SystemModeControlService.SystemModeEntities mode = SystemModeControlService.getInstanceMode("Link");
                        ERPWAutoLoginService loginService = new ERPWAutoLoginService("555", "focusone", mode);
                    }
                    else
                    {
                        HasPermission = false;
                    }
                }

            }
            catch (Exception)
            {
                HasPermission = false;
            }

            return HasPermission;
        }

        protected string AddTierZeroItem(TierZeroEn Data, string channel)
        {
            ResultCreateTierZero resultTierZero = new ResultCreateTierZero();
            try
            {

                Data.SEQ = libTierZero.InsertTierZeroItem(SID, CompanyCode, EmployeeCode, Data);
                resultTierZero.ResultMessage = "Create Tier 0 Success.";
                resultTierZero.CreatedSuccess = true;

                try
                {
                    if (!string.IsNullOrEmpty(Data.CustomerCode) ||
                        !string.IsNullOrEmpty(Data.EMail) ||
                        !string.IsNullOrEmpty(Data.TelNo))
                    {
                        List<CustomerProfile> ListCustomer = ERPW.Lib.Master.CustomerService.getInstance().SearchCustomer(
                            SID,
                            CompanyCode,
                            Data.CustomerCode,
                            Data.EMail,
                            Data.TelNo
                        );
                        
                        if (!string.IsNullOrEmpty(Data.CustomerCode))
                        {
                            ListCustomer = ListCustomer.Where(w => w.CustomerCode.Equals(Data.CustomerCode)).ToList();
                        }

                        if (ListCustomer.Count > 0)
                        {
                            bool responseToCustomer = false;

                            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["RECURRING_RESPONSE"]))
                            {
                                bool.TryParse(ConfigurationManager.AppSettings["RECURRING_RESPONSE"], out responseToCustomer);
                            }

                            if (responseToCustomer && channel == TierZeroLibrary.TIER_ZERO_CHANNEL_EMAIL)
                            {
                                ReplyMailToCustomer(Data, ListCustomer.First());

                                Data.Status = TierZeroLibrary.TIER_ZERO_STATUS_OPEN;
                                Data.TicketNumber = "";
                                Data.TicketType = "";
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(Data.EquipmentNo))
                                {
                                    Data.OwnerService = ServiceTicketLibrary.LookUpTable(
                                        "EquipmentObjectType",
                                        "master_equipment_general",
                                        "where SID = '" + SID + @"' AND CompanyCode = '" + CompanyCode + "' AND EquipmentCode = '" + Data.EquipmentNo + "'"
                                    );
                                }
                                resultTierZero.ResultTicket = tierZeroService.AutoCreatedTicketFormTierZero(
                                    SessionID, SID, CompanyCode,
                                    Data, ListCustomer.First().CustomerCode, EmployeeCode
                                );

                                new Thread(() =>
                                {
                                    try
                                    {
                                        //Save Attach File
                                        PostUploadFile(resultTierZero.ResultTicket.ObjectLink, Data.UploadType, Data.MessageUpload);
                                        //end
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }).Start();

                                Data.Status = TierZeroLibrary.TIER_ZERO_STATUS_CREATED;
                                Data.TicketNumber = resultTierZero.ResultTicket.TicketNo;
                                Data.TicketType = resultTierZero.ResultTicket.TicketType;

                            }

                            DataTable dtAdditional = ERPW.Lib.Master.CustomerService.getInstance().GetCustomerERPWDataDetailAdditional(SID, CompanyCode, ListCustomer.First().CustomerCode);
                            if (dtAdditional.Rows.Count > 0)
                            {
                                resultTierZero.responsible_organization = Convert.ToString(dtAdditional.Rows[0]["ResponsibleOrganization"]);
                                resultTierZero.critical = Convert.ToString(dtAdditional.Rows[0]["CustomerCritical"]);
                            }
                            
                            new Thread(() =>
                            {
                                try
                                {
                                    TierZeroLibrary.GetInstance().UpdateTierZeroDetail(
                                        SID, CompanyCode, Data.SEQ, Data.TicketType, Data.TicketNumber, Data.Status,
                                        ListCustomer.First().CustomerCode, ListCustomer.First().CustomerName, EmployeeCode
                                    );
                                }
                                catch (Exception ex)
                                {

                                }
                            }).Start();


                            
                        }
                        else
                        {
                            resultTierZero.ResultTicket = new ResultAutoCreateTicket();
                            resultTierZero.ResultTicket.CreatedSuccess = false;
                            resultTierZero.ResultTicket.ResultMessage = "Can't Auto Create Service Ticket.";
                            resultTierZero.ResultTicket.TicketNo = "";
                            resultTierZero.ResultTicket.TicketNoDisplay = "";
                            resultTierZero.ResultTicket.TicketType = "";
                        }
                    }
                    else
                    {
                        resultTierZero.ResultTicket = new ResultAutoCreateTicket();
                        resultTierZero.ResultTicket.CreatedSuccess = false;
                        resultTierZero.ResultTicket.ResultMessage = "Can't Auto Create Service Ticket.";
                        resultTierZero.ResultTicket.TicketNo = "";
                        resultTierZero.ResultTicket.TicketNoDisplay = "";
                        resultTierZero.ResultTicket.TicketType = "";
                    }
                }
                catch (Exception ex)
                {
                    resultTierZero.ResultTicket = new ResultAutoCreateTicket();
                    resultTierZero.ResultTicket.CreatedSuccess = false;
                    resultTierZero.ResultTicket.ResultMessage = "Can't Auto Create Service Ticket.[" + ex.Message + "]";
                    resultTierZero.ResultTicket.TicketNo = "";
                    resultTierZero.ResultTicket.TicketNoDisplay = "";
                    resultTierZero.ResultTicket.TicketType = "";
                }
                //TierZeroLibrary.GetInstance().UpdateTierZeroStatus(SID, CompanyCode, Data.SEQ, TierZeroLibrary.TIER_ZERO_STATUS_CREATED);

                string responseJson = JsonConvert.SerializeObject(resultTierZero);
                return responseJson;
                //Response.Write(responseJson);
            }
            catch (Exception ex)
            {
                resultTierZero.ResultMessage = "Can't Create Tier 0.";
                resultTierZero.ErrorException = ex.Message;
                resultTierZero.CreatedSuccess = false;
                resultTierZero.ResultTicket = new ResultAutoCreateTicket();
                resultTierZero.ResultTicket.CreatedSuccess = false;
                resultTierZero.ResultTicket.ResultMessage = "Can't Auto Create Service Ticket.[" + ex.Message + "]";
                resultTierZero.ResultTicket.TicketNo = "";
                resultTierZero.ResultTicket.TicketNoDisplay = "";
                resultTierZero.ResultTicket.TicketType = "";

                string responseJson = JsonConvert.SerializeObject(resultTierZero);
                return responseJson;
                //Response.Write(responseJson);
            }
        }

        private string UpdateTicketFromPatern(TierZeroEn Data)
        {
            string ResponseStr = "";
            List<TierZeroEn> listTierZero = TierZeroLibrary.GetInstance().getTierZeroList(
                SID,
                CompanyCode,
                TierZeroLibrary.TIER_ZERO_STATUS_CREATED
            );

            listTierZero = listTierZero
                .Where(w =>
                    w.EquipmentNo == Data.EquipmentNo &&
                    w.Ticket_CallStatus == ServiceTicketLibrary.SERVICE_CALL_STATUS_OPEN
                )
                .OrderByDescending(o => o.SEQ)
                .ToList();

            if (listTierZero.Count == 1)
            {
                string activityID = AfterSaleService.getInstance().getLastActivityServiceCall(
                    CompanyCode, listTierZero.First().TicketNumber, "", ""
                );

                string ticketType = listTierZero.First().TicketType;
                string ticketNo = listTierZero.First().TicketNumber;
                string ticketYear = listTierZero.First().FiscalYear;
                string tierDesc = ServiceTicketLibrary.LookUpTable(
                    "Subject", "ticket_service_header",
                    @"where SID = '" + SID + @"' 
                        and CompanyCode = '" + CompanyCode + @"'
                        and TicketCode = '" + activityID + @"'"
                ) + " [Auto By System]";

                AfterSaleService.getInstance().ResolveTicket(
                    SID, CompanyCode,
                    ticketType, ticketNo, ticketYear, tierDesc,
                    EmployeeCode, FullNameEN
                );

                //NotificationLibrary.GetInstance().TicketAlertEvent(
                //    NotificationLibrary.EVENT_TYPE.TICKET_RESOLVE,
                //    SID,
                //    CompanyCode,
                //    ticketNo,
                //    EmployeeCode
                //);
            }
            //else if (listTierZero.Count > 1)
            //{

            //}
            else
            {
                string Channel = !string.IsNullOrEmpty(Request["Channel"]) ? Request["Channel"] : Request.Headers["Channel"];
                Data.EquipmentNo = "";
                Data.CustomerCode = "";
                Data.CustomerName = "";
                ResponseStr = AddTierZeroItem(Data, Channel);
            }
            //listTierZero.Where(w => )

            return ResponseStr;
        }

        private void ReplyMailToCustomer(TierZeroEn en, CustomerProfile enCustomer)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["RECURRING_RESPONSE_ADDRESS"]))
            {
                string URL = ConfigurationManager.AppSettings["RECURRING_RESPONSE_ADDRESS"] + "?key=" + en.SEQ;

                string filePath = HttpContext.Current.Server.MapPath("~//EmailTemplate//ServiceTicketReplyCustomer.html");
                string mailContent = System.IO.File.ReadAllText(filePath);

                string tempTicketNo = "TK" + en.SEQ.PadLeft(8, '0');

                mailContent = mailContent.Replace("{!#TICKETNO#!}", tempTicketNo);
                mailContent = mailContent.Replace("{!#CUSTOMER#!}", "คุณ" + enCustomer.CustomerName);
                mailContent = mailContent.Replace("{!#SUBJECT#!}", en.Subject);
                mailContent = mailContent.Replace("{!#DETAIL#!}", en.Detail);
                mailContent = mailContent.Replace("{!#URL#!}", URL);

                NotificationLibrary.GetInstance().SendResponseTicketToCustomer(SID, CompanyCode, en.EMail, mailContent);
            }
        }

        private static String getObjectLinkFromBody(String mailBody)
        {
            int startIndex = mailBody.LastIndexOf("message ID : [");
            int endIndex = mailBody.LastIndexOf("]");

            if ((startIndex < 0) || (endIndex < 0) || (startIndex >= endIndex))
            {
                return "";
            }

            return mailBody.Substring(startIndex + "message ID : [".Length, endIndex - startIndex - "message ID : [".Length);
        }

        private checkPatternInform checkPatternInformCreated(string MailSubject)
        {
            checkPatternInform enPattern = new checkPatternInform();
            bool isMatPattern = false;

            if (KeySplit.KeyWord.Length == 0)
            {
                return new checkPatternInform();
            }
            List<string> listMailSubject = MailSubject.Split(KeySplit.KeyWord[0]).ToList();
            if (listMailSubject.Count > 1)
            {
                if (!isMatPattern)
                {
                    ListKeyWordEvent_Create.ForEach(r =>
                    {
                        if (listMailSubject[1].Trim().IndexOf(r.KeyWord) >= 0)
                        {
                            isMatPattern = true;
                            enPattern.PatternMode = ApplicationSession.CREATE_MODE_STRING;
                        }
                        if (isMatPattern)
                        {
                            return;
                        }
                    });
                }

                if (!isMatPattern)
                {
                    ListKeyWordEvent_Update.ForEach(r =>
                    {
                        if (listMailSubject[1].Trim().IndexOf(r.KeyWord) >= 0)
                        {
                            isMatPattern = true;
                            enPattern.PatternMode = ApplicationSession.CHANGE_MODE_STRING;
                        }
                        if (isMatPattern)
                        {
                            return;
                        }
                    });
                }

                ListEquipmentMappingFirstOwner.ForEach(r =>
                {
                    if (listMailSubject[0].Trim() == r.EquipmentName)
                    {
                        enPattern.EquipmentCode = r.EquipmentCode;
                        enPattern.EquipmentName = r.EquipmentName;
                        enPattern.CustomerCode = r.CustomerCode;
                        enPattern.CustomerName = r.CustomerName;
                    }
                });
            }
            enPattern.isMatPattern = isMatPattern;

            return enPattern;
        }

        #region Convert To Request & Properites

        private string GetRequestKey(string keyCode)
        {
            return !string.IsNullOrEmpty(Request[keyCode]) ? Request[keyCode] : Request.Headers[keyCode];
        }

        private string ValidTicketType(string TicketTypeCode)
        {
            if (!("I").Equals(TicketTypeCode,StringComparison.CurrentCultureIgnoreCase) 
                && !("R").Equals(TicketTypeCode, StringComparison.CurrentCultureIgnoreCase)
                && !("P").Equals(TicketTypeCode, StringComparison.CurrentCultureIgnoreCase))
            {
                return "I";
            }
            return TicketTypeCode.ToUpper();
        }

        private void validDataForOwnerService(string ticketType, InputDataAPI model)
        {
            //Check Owner Service
            if (!string.IsNullOrEmpty(model.OwnerService))
            {
                DataTable dtOwner = MasterConfigLibrary.GetInstance().GetMasterConfigOwnerGroup(SID, CompanyCode, model.OwnerService);
                if (dtOwner.Rows.Count <= 0)
                {
                    throw new Exception("ไม่พบ Owner Service Code " + model.OwnerService + " ในระบบ!");
                }
            }

            string buObjectCode = libServiceTicket.GetBusinessObjectFromTicketType(SID, ticketType);

            #region Check RequestGroup
            if (!string.IsNullOrEmpty(model.RequestGroup))
            {
                List<IncidentAreaEntity.AreaGroup> En = libconfig.getIncedentAreaGroup(
                SID, CompanyCode, buObjectCode, model.OwnerService
                 );
                if (!string.IsNullOrEmpty(model.OwnerService))
                    En = En.Where(w => w.ChildrenOwnerGroup.Contains(model.OwnerService)).ToList();

                if (En.Find(x => x.GroupCode == model.RequestGroup) == null)
                {
                    throw new Exception("ไม่พบ RequestGroup " + model.RequestGroup + " ในระบบ จาก Owner[" + model.OwnerService + "]!");
                }
            }
            else if (!string.IsNullOrEmpty(model.RequestType)
                || !string.IsNullOrEmpty(model.RequestSource)
                || !string.IsNullOrEmpty(model.ContactSource))
            {
                throw new Exception("ไม่สามารถระบุ RequestType[" + model.RequestType + "] -> RequestSource[" + model.RequestSource + "] -> ContactSource[" + model.ContactSource + "] ได้เนื่องจาก RequestGroup เป็นค่าว่าง!");
            }

            #endregion

            #region Check RequestType

            if (!string.IsNullOrEmpty(model.RequestType))
            {
                List<IncidentAreaEntity.AreaType> En = libconfig.getIncedentAreaType(
                    SID, CompanyCode,
                    buObjectCode, model.OwnerService, true, model.RequestGroup
                );
                if (!string.IsNullOrEmpty(model.OwnerService))
                    En = En.Where(w => w.ChildrenOwnerGroup.Contains(model.OwnerService)).ToList();

                if (En.Find(x => x.TypeCode == model.RequestType) == null)
                {
                    throw new Exception("ไม่พบ RequestType " + model.RequestType + " ในระบบ จาก RequestGroup[" + model.RequestGroup + "]!");
                }
            }
            else if (!string.IsNullOrEmpty(model.RequestSource)
                || !string.IsNullOrEmpty(model.ContactSource))
            {
                throw new Exception("ไม่สามารถระบุ RequestSource[" + model.RequestSource + "] -> ContactSource[" + model.ContactSource + "] ได้เนื่องจาก RequestType เป็นค่าว่าง!");
            }

            #endregion

            #region Check RequestSource

            if (!string.IsNullOrEmpty(model.RequestSource))
            {
                List<IncidentAreaEntity.AreaSource> En = libconfig.getIncedentAreaSource(
                   SID, CompanyCode,
                   buObjectCode, model.OwnerService, true, model.RequestGroup, model.RequestType
               );
                if (!string.IsNullOrEmpty(model.OwnerService))
                    En = En.Where(w => w.ChildrenOwnerGroup.Contains(model.OwnerService)).ToList();

                if (En.Find(x => x.SourceCode == model.RequestSource) == null)
                {
                    throw new Exception("ไม่พบ RequestSource " + model.RequestSource + " ในระบบ จาก RequestType[" + model.RequestType + "]!");
                }
            }
            else if (!string.IsNullOrEmpty(model.ContactSource))
            {
                throw new Exception("ไม่สามารถระบุ ContactSource[" + model.ContactSource + "] ได้เนื่องจาก RequestSource เป็นค่าว่าง!");
            }

            #endregion
            
            #region Check RequestSource

            if (!string.IsNullOrEmpty(model.ContactSource))
            {
                List<IncidentAreaEntity.AreaContactSource> En = libconfig.getIncedentAreaContactSource(
                    SID, CompanyCode,
                    buObjectCode, model.OwnerService, model.RequestGroup, model.RequestType, model.RequestSource
                );
                if (!string.IsNullOrEmpty(model.OwnerService))
                    En = En.Where(w => w.ChildrenOwnerGroup.Contains(model.OwnerService)).ToList();

                if (En.Find(x => x.ContactSourceCode == model.ContactSource) == null)
                {
                    throw new Exception("ไม่พบ ContactSource " + model.ContactSource + " ในระบบ จาก RequestSource[" + model.RequestSource + "]!");
                }
            }

            #endregion

        }

        #endregion

        #region Attach Files

        private string PostUploadFile(string aobjectLink, string uploadType, string Message)
        {
            if (string.IsNullOrEmpty(aobjectLink))
            {
                return "";
            }
            string TimeLineKey = "";
            try
            {
                HttpFileCollection Files = Request.Files;
                if (Files != null && Files.Count > 0)
                {
                    string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                            (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                    string UploadFilePath = Server.MapPath("~/managefile/" + SID + "/kmfile/assets/");
                    if (!Directory.Exists(UploadFilePath))
                    {
                        Directory.CreateDirectory(UploadFilePath);
                    }
                    string UploadFileUrl = Domain + "/managefile/" + SID + "/kmfile/assets/";

                    //string aobjectLink = Request["aobj"];
                    //string uploadType = Request["uploadType"];
                    //string Message = Request["message"];
                    string dateTime = Validation.getCurrentServerStringDateTime();
                    TimeLineKey = aobjectLink + "_" + uploadType + "_" + dateTime;


                    int type = 0;
                    int assetType = 0;
                    if (uploadType.Equals("IMAGE"))
                    {
                        type = Timeline.TYPE_IMAGE;
                        assetType = TimelineAsset.TYPE_IMAGE;
                    }
                    else //FILE
                    {
                        type = Timeline.TYPE_ATTACH_FILE;
                        assetType = TimelineAsset.TYPE_FILE;
                    }

                    Timeline timeLine = new Timeline();
                    timeLine.SID = SID;
                    timeLine.CompanyCode = CompanyCode;
                    timeLine.ObjectLink = aobjectLink;
                    timeLine.TimelineKey = TimeLineKey;
                    timeLine.Type = type.ToString();
                    timeLine.Message = Message;
                    timeLine.ContentUri = "";
                    timeLine.ContentUrl = "";
                    timeLine.Status = "";
                    timeLine.Latitude = "";
                    timeLine.Longitude = "";
                    timeLine.Address = "";
                    timeLine.CreatorId = EmployeeCode;
                    timeLine.CreatorName = FullNameEN;
                    timeLine.LinkId = EmployeeCode;
                    timeLine.EmployeeCode = EmployeeCode;
                    timeLine.CreatedOn = dateTime;

                    ERPW.Lib.Service.ServiceLibrary libService = new ERPW.Lib.Service.ServiceLibrary();
                    libService.AddTimeline(SID, CompanyCode, timeLine);
                    PostAssetAndSaveFile(Files, TimeLineKey, uploadType, assetType, UploadFilePath, UploadFileUrl);

                    libService.UpdateAttachFileKeyToRemark(SID, CompanyCode, aobjectLink, TimeLineKey);
                }

                return TimeLineKey;
            }
            catch
            {
                return "";
            }
        }

        private void PostAssetAndSaveFile(HttpFileCollection Files, string timeLineKey, string uploadType, int assetType
            , string UploadFilePath, string UploadFileUrl)
        {
            for (int i = 0; i < Files.Count; i++)
            {
                string Filekey = uploadType + "_" + Validation.getCurrentServerStringDateTime() + i.ToString();
                string savedFileName = SaveFile(Files[i], Filekey, UploadFilePath);
                if (!string.IsNullOrEmpty(savedFileName))
                {
                    TimelineAsset asset = new TimelineAsset();
                    asset.SID = SID;
                    asset.CompanyCode = CompanyCode;
                    asset.ObjectLink = timeLineKey;
                    asset.AssetKey = Filekey;
                    asset.Type = assetType.ToString();
                    asset.ContentUri = UploadFilePath + savedFileName;
                    asset.ContentUrl = UploadFileUrl + savedFileName;
                    asset.Latitude = "";
                    asset.Longitude = "";
                    asset.Address = "";
                    asset.CreatedBy = EmployeeCode;
                    asset.CreatedOn = Validation.getCurrentServerStringDateTime();

                    ERPW.Lib.Service.ServiceLibrary libService = new ERPW.Lib.Service.ServiceLibrary();
                    libService.AddTimelineAsset(SID, CompanyCode, asset);
                }
            }
        }

        private string SaveFile(HttpPostedFile file, string FileKey, string UploadFilePath)
        {
            try
            {
                string savePath = UploadFilePath;
                string extension = System.IO.Path.GetExtension(file.FileName);
                string realName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                string fileName = ReplaceInvalidFilenameChar(realName) + "_" + Validation.getCurrentServerStringDateTimeMillisecond() + extension;

                savePath += fileName;
                file.SaveAs(savePath);

                if (Request["createThumb"] != null)
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(savePath);
                    System.Drawing.Image thumb = image.GetThumbnailImage(24, 24, () => false, IntPtr.Zero);
                    string tempFileName = fileName.Replace(extension, "");
                    savePath = savePath.Replace(fileName, tempFileName + "_thumb" + extension);
                    thumb.Save(Path.ChangeExtension(savePath, "png"));
                }

                return fileName;
            }
            catch
            {
                return "";
            }
        }

        private string ReplaceInvalidFilenameChar(string input)
        {
            string illegal = input;
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            illegal = r.Replace(illegal, "");
            illegal = Regex.Replace(input, @"[^\w\d]", "");
            return illegal;
        }

        #endregion

        protected class ResultCreateTierZero
        {
            public string ResultMessage { get; set; }
            public bool CreatedSuccess { get; set; }
            public string ErrorException { get; set; }
            public string UserHostAddress { get { return HttpContext.Current.Request.UserHostAddress; } }
            public string UserHostName { get { return HttpContext.Current.Request.UserHostName; } }
            public string responsible_organization { get; set; }
            public string critical { get; set; }

            public ResultAutoCreateTicket ResultTicket { get; set; }
        }

        protected class ObjectsEmailData
        {
            public string EmailFrom { get; set; }
            public string EmailSubject { get; set; }
            public string EmailBody { get; set; }
        }

        protected class checkPatternInform
        {
            public bool isMatPattern { get; set; }
            public string PatternMode { get; set; }
            public string EquipmentCode { get; set; }
            public string EquipmentName { get; set; }
            public string CustomerCode { get; set; }
            public string CustomerName { get; set; }
        }

        public class InputDataAPI
        {
            public string OwnerService { get; set; }
            public string RequestGroup { get; set; }
            public string RequestType { get; set; }
            public string RequestSource { get; set; }
            public string ContactSource { get; set; }
        }
    }
}