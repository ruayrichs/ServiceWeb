using agape.lib.constant;
using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Authentication;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Service;
using ERPW.Lib.WebConfig;
using Newtonsoft.Json;
using ServiceWeb.API.Model.Respond;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API.v1
{
    public partial class TicketDetailAPI : System.Web.UI.Page
    {
        private ServiceTicketLibrary lib = new ServiceTicketLibrary();
        private AppClientLibrary libAppClient = AppClientLibrary.GetInstance();
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();

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

        private string _EmployeeCode;
        private string EmployeeCode
        {
            get
            {
                if (string.IsNullOrEmpty(_EmployeeCode))
                    _EmployeeCode = !string.IsNullOrEmpty(ERPWAuthentication.EmployeeCode) ? ERPWAuthentication.EmployeeCode : ""; // "focusone";
                return _EmployeeCode;
            }
        }

        private string _FullNameTH;
        private string FullNameTH
        {
            get
            {
                if (string.IsNullOrEmpty(_FullNameTH))
                    _FullNameTH = !string.IsNullOrEmpty(ERPWAuthentication.FullNameTH) ? ERPWAuthentication.FullNameTH : ""; // "focusone";
                return _FullNameTH;
            }
        }

        private string _FullNameEN;
        private string FullNameEN
        {
            get
            {
                if (string.IsNullOrEmpty(_FullNameEN))
                    _FullNameEN = !string.IsNullOrEmpty(ERPWAuthentication.FullNameEN) ? ERPWAuthentication.FullNameEN : ""; // "focusone";
                return _FullNameEN;
            }
        }

        private string _OwnerCode;
        private string OwnerCode
        {
            get
            {
                if (string.IsNullOrEmpty(_OwnerCode))
                    _OwnerCode = !string.IsNullOrEmpty(ERPWAuthentication.Permission.OwnerGroupCode) ? ERPWAuthentication.Permission.OwnerGroupCode : ""; // "focusone";
                return _OwnerCode;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TicketDetailResponseModel response = new TicketDetailResponseModel();
            if (!checkPermission())
            {
                response.resultCode = "Error";
                response.message = "No Permission.";
            }
            else
            {
                response = getDataTicketDetail();
            }
            Response.Write(JsonConvert.SerializeObject(response, Formatting.Indented));
        }

        private TicketDetailResponseModel getDataTicketDetail()
        {
            TicketDetailResponseModel response = new TicketDetailResponseModel();
            try
            {
                string docnumber = !string.IsNullOrEmpty(Request["TicketNumber"]) ? Request["TicketNumber"] : Request.Headers["TicketNumber"];
                string doctype = ServiceTicketLibrary.LookUpTable("Doctype", "cs_servicecall_header", "where SID = '" + SID + "' AND CompanyCode = '" + CompanyCode + "' and CallerID = '" + docnumber + "'");
                string fiscalyear = ServiceTicketLibrary.LookUpTable("Fiscalyear", "cs_servicecall_header", "where SID = '" + SID + "' AND CompanyCode = '" + CompanyCode + "' and CallerID = '" + docnumber + "'");

                if (string.IsNullOrEmpty(doctype))
                {
                    throw new Exception("Ticket not found.");
                }

                Object[] objParam = new Object[] { "1500117",
                    (string)Session[ApplicationSession.USER_SESSION_ID],
                    CompanyCode, doctype, docnumber, fiscalyear
                };

                DataSet[] objDataSet = new DataSet[] { new tmpServiceCallDataSet() };
                DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
                if (objReturn != null)
                {
                    tmpServiceCallDataSet serviceTempCallEntity = new tmpServiceCallDataSet();
                    response.data = new TicketDetailModel();
                    serviceTempCallEntity.Merge(objReturn.Copy());

                    if (serviceTempCallEntity.cs_servicecall_header.Rows.Count > 0)
                    {
                        var data = serviceTempCallEntity.cs_servicecall_header.First();

                        response.data.TicketType = data.Doctype;
                        response.data.TicketNumber = data.CallerID;
                        response.data.TicketNumberDisplay = data.CallerID; // TODO
                        response.data.TicketStatusCode = data.Docstatus;
                        response.data.TicketStatusDesc = ServiceLibrary.LookUpTable(
                            "TicketStatusDesc",
                            "ERPW_TICKET_STATUS",
                            "where SID = '" + SID + @"' AND CompanyCode = '" + CompanyCode + @"' AND TicketStatusCode = '" + data.Docstatus + "'"
                        ); // TODO

                        response.data.TicketDate = data.DOCDATE;
                        response.data.TicketCallBackDate = data.CallbackDate;
                        response.data.TicketCallBackTime = data.CallbackTime;
                        response.data.ReferenceExternalTicket = data.ReferenceDocument;
                        response.data.TicketCreatedBy = ServiceLibrary.LookUpTable(
                            "FirstName + ' ' + LastName as FullName",
                            "master_employee",
                            "where SID = '" + SID + @"' AND CompanyCode = '" + CompanyCode + @"' AND EmployeeCode = '" + data.Docstatus + "'"
                        ); //data.CREATED_BY; //TODO
                        response.data.TicketCreatedDateTime = data.CREATED_ON;

                        response.data.CustomerCode = data.CustomerCode;
                        response.data.CustomerName = data.CustomerDesc;
                        response.data.ContactCode = data.ContractPersonName;
                        response.data.ContactName = data.ContractPersonName;
                        response.data.ContactEmail = data.Email; //TODO
                        response.data.ContactPhone = data.ContractPersonTel;
                        response.data.ContactRemark = ""; //TODO

                        response.data.TicketImpact = data.Impact;
                        response.data.TicketUrgency = data.Urgency;
                        response.data.TicketPriority = data.Priority;
                        response.data.TicketSubject = data.HeaderText;

                        response.data.AffectSLA = data.AffectSLA;
                    }

                    if (serviceTempCallEntity.cs_servicecall_item.Rows.Count > 0)
                    {
                        var data = serviceTempCallEntity.cs_servicecall_item.First();

                        response.data.TicketDescription = data.Remark;
                        response.data.CICode = data.EquipmentNo;
                        if (!string.IsNullOrEmpty(data.EquipmentNo))
                        {
                            response.data.CIName = data.EquipmentDesc;
                        }
                        else
                        {
                            response.data.CIName = "";
                        }

                        response.data.CIFamily = ""; //TODO
                        response.data.CIClass = ""; //TODO
                        response.data.CICategory = ""; //TODO
                        response.data.CISerialNo = ""; //TODO

                        response.data.OwnerService = data.QueueOption;
                        response.data.IncidentGroup = data.ProblemGroup;
                        response.data.IncidentType = data.ProblemTypeCode;
                        response.data.IncidentSource = data.OriginCode;
                        response.data.ContactSource = data.CallTypeCode;

                        response.data.SummaryProblem = data.SummaryProblem;
                        response.data.SummaryCause = data.SummaryCause;
                        response.data.SummaryResolution = data.SummaryResolution;
                    }

                    string aobjectlink = AfterSaleService.getInstance().getAobjectLinkByTicketNumber(
                        response.data.TicketNumber
                    );
                    response.data.AssignTo = new List<AssignToModel>();
                    response.data.AssignTo.AddRange(AfterSaleService.getInstance().getAssignTo_MainDelegate_ByAobjectLink(
                        SID, CompanyCode, aobjectlink
                    ));
                    response.data.AssignTo.AddRange(AfterSaleService.getInstance().getAssignTo_Participants_ByAobjectLink(
                        SID, CompanyCode, aobjectlink
                    ));


                    response.resultCode = "Success";
                    response.message = "Search success.";
                }
                else
                {
                    throw new Exception("Ticket not found.");
                }
            }
            catch (Exception ex)
            {
                response.resultCode = "Error";
                response.message = ex.Message;
            }

            return response;
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
                        _EmployeeCode = dtPermission.Rows[0]["EmployeeCode"].ToString();
                        _OwnerCode = dtPermission.Rows[0]["OwnerService"].ToString();
                    }

                    if (!string.IsNullOrEmpty(_UserName))
                    {
                        HasPermission = true;
                        SystemModeControlService.SystemModeEntities mode = SystemModeControlService.getInstanceMode("Link");
                        ERPWAutoLoginService loginService = new ERPWAutoLoginService(_SID, _UserName, mode);
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

    }
}

