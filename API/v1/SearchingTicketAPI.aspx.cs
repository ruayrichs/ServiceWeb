using Agape.Lib.DBService;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.WebConfig;
using Newtonsoft.Json;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;





//namespace ServiceWeb.API.v1
//{
//    public partial class SearchingTicketAPI : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {

//        }
//    }
//}


namespace ServiceWeb.API.v1
{
    public partial class SearchingTicketAPI : System.Web.UI.Page
    {
        private ServiceTicketLibrary lib = new ServiceTicketLibrary();
        private AppClientLibrary libAppClient = AppClientLibrary.GetInstance();

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
            string jsonRespond = "";
            CommonResponse response = new CommonResponse();
            if (!checkPermission())
            {
                response.ResultCode = "Error";
                response.Message = "No Permission.";
                response.Datas = new DataTable();
            }
            else
            {
                response = TicketTrackingDetail();
            }
            Response.Write(JsonConvert.SerializeObject(response, Formatting.Indented));
        }

        private CommonResponse TicketTrackingDetail()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                string Channel = !string.IsNullOrEmpty(Request["Channel"]) ? Request["Channel"] : Request.Headers["Channel"];
                string PermissionKey = !string.IsNullOrEmpty(Request["PermissionKey"]) ? Request["PermissionKey"] : Request.Headers["PermissionKey"];

                // "" || "xxx"
                string ticketNumber = !string.IsNullOrEmpty(Request["ticketNumber"]) ? Request["ticketNumber"] : Request.Headers["ticketNumber"];

                // "" no search || "01" Active || "00" Inactive
                string activeStatuc = !string.IsNullOrEmpty(Request["activeStatuc"]) ? Request["activeStatuc"] : Request.Headers["activeStatuc"];

                // "" no search || "20190401" format yyyymmdd
                string dateFrom = !string.IsNullOrEmpty(Request["dateFrom"]) ? Request["dateFrom"] : Request.Headers["dateFrom"];

                // "" no search || "20190401" format yyyymmdd
                string dateTo = !string.IsNullOrEmpty(Request["dateTo"]) ? Request["dateTo"] : Request.Headers["dateTo"];

                // "" no search || "1359" format hhMM
                string timeFrom = !string.IsNullOrEmpty(Request["timeFrom"]) ? Request["timeFrom"] : Request.Headers["timeFrom"];

                // "" no search || "1430" format hhMM
                string timeTo = !string.IsNullOrEmpty(Request["timeTo"]) ? Request["timeTo"] : Request.Headers["timeTo"];

                // "" no search || "xxx" format hhMM
                string subject = !string.IsNullOrEmpty(Request["subject"]) ? Request["subject"] : Request.Headers["subject"];


                List<string> listBusinessObject = new List<string>();
                if (ERPWAuthentication.Permission.IncidentView)
                {
                    listBusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_INCIDENT);
                }
                if (ERPWAuthentication.Permission.ProblemView)
                {
                    listBusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_PROBLEM);
                }
                if (ERPWAuthentication.Permission.RequestView)
                {
                    listBusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_REQUEST);
                }
                if (ERPWAuthentication.Permission.ChangeOrderView)
                {
                    listBusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_CHANGE);
                }

                List<string> listTicketType = ServiceTicketLibrary.GetInstance().getListTicketType(
                    SID,
                    listBusinessObject,
                    OwnerCode
                );

                DataTable dtDataSearch = lib.GetTicketList(
                    SID, CompanyCode,
                    listTicketType,
                    "", //"fiscalyear", 
                    ticketNumber, //"docnumber",
                    "", //"docStatus",
                    activeStatuc, //"docStatucActive", 
                    dateFrom, //"datefrom", 
                    dateTo, //"dateto", 
                    "", //"ddlImpactSearch.SelectedValue",
                    "", //"ddlUrgencySearch.SelectedValue", 
                    "", //"priority",
                    "", //"AutoEquipmentSearch.SelectedValue",
                    "", //"customercode", 
                    subject, //"txtSearchSubject.Text.ToString()",
                    OwnerCode,
                    "", //"txtProblemGroup.SelectValue", 
                    "", //"txtProblemType.SelectValue", 
                    "", //"txtProblemSource.SelectValue", 
                    "", //"txtContactSource.SelectValue",
                    "", //"_ddl_contact_person_search.SelectText", 
                    timeFrom, //"timefrom", 
                    timeTo //"timeto"
                );
                response.Datas = dtDataSearch;

                response.ResultCode = "Success";
                response.Message = "Search success " + dtDataSearch.Rows.Count + " result.";
            }
            catch (Exception ex)
            {

                response.ResultCode = "Error";
                response.Message = ex.Message;
                response.Datas = new DataTable();
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
                        _UserName = dtPermission.Rows[0]["UserName"].ToString();
                        _EmployeeCode = dtPermission.Rows[0]["EmployeeCode"].ToString();
                        _OwnerCode = dtPermission.Rows[0]["OwnerService"].ToString();
                    }

                    if (!string.IsNullOrEmpty(_UserName))
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

        protected class CommonResponse
        {
            public string ResultCode { get; set; }
            public string Message { get; set; }
            public DataTable Datas { get; set; }

        }
    }
}