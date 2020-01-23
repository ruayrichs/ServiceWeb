using agape.lib.constant;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.API.v1;
using ERPW.Lib.Service.Entity;
using ERPW.Lib.Service.Entity.API;
using ERPW.Lib.WebConfig;
using Newtonsoft.Json;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API.v1
{
    public partial class CustomerDetailAPI : System.Web.UI.Page
    {
        #region properties
        private AppClientLibrary libAppClient = AppClientLibrary.GetInstance();
        private CustomerDashboardLib customerDashboardLib = new CustomerDashboardLib();
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


        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            initData();
        }

        //method here
        private void initData()
        {
            string responseJson = "";
            string CustomerCode = !string.IsNullOrEmpty(Request["CustomerCode"]) ? Request["CustomerCode"] : Request.Headers["CustomerCode"];
            List<String> listResponse = new List<string>();
            Boolean LoginByPermission = false;
            if (string.IsNullOrEmpty(ERPWAuthentication.EmployeeCode))
            {
                LoginByPermission = true;
            }

            if (!checkPermission())
            {
                responseJson = "{\"message\":\"invalid permission key\"}";
                Response.Write(responseJson);
                return;
            }

            Response.ContentType = "application/json";
            #region to do
            //CustomerDashboardFinalDataModel customerDashboardFinalDataModel = new CustomerDashboardFinalDataModel();
            //customerDashboardFinalDataModel = customerDashboardLib.PreparFinanDataDashboard(SID, CompanyCode, CustomerCode);
            //responseJson = JsonConvert.SerializeObject(customerDashboardFinalDataModel);
            CustomerProfileLib customerProfileLib = new CustomerProfileLib(SID, CompanyCode, CustomerCode);
            ClientDetail dataClientDetail = new ClientDetail();
            dataClientDetail = customerProfileLib.finalDateModelClientDetail(SID, CompanyCode, CustomerCode);
            var json = new JavaScriptSerializer().Serialize(dataClientDetail);
            Response.Write(json);
            //Response.Write(responseJson);
            #endregion
            if (LoginByPermission)
            {
                Session.Abandon();
            }
        }

        #region check permission
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
                    if (libAppClient.checkAuthenCreatedTicket(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode,CorpoKey, AppKey, AppID))
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
        #endregion
    }
}