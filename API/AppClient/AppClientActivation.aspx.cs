using Agape.FocusOne.Utilities;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using ERPW.Lib.WebConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API.AppClient
{
    public partial class AppClientActivation : System.Web.UI.Page
    {
        AppClientLibrary libAppClient = AppClientLibrary.GetInstance();

        private string _SID;
        private string SID
        {
            get
            {
                if (string.IsNullOrEmpty(_SID))
                    _SID = ERPWebConfig.GetSID();
                return _SID;
            }
        }

        private string _CompanyCode;
        private string CompanyCode
        {
            get
            {
                if (string.IsNullOrEmpty(_CompanyCode))
                    _CompanyCode = ERPWebConfig.GetCompany();
                return _CompanyCode;
            }
        }

        private string _ApplicationID;
        private string ApplicationID
        {
            get
            {
                if (string.IsNullOrEmpty(_ApplicationID))
                    _ApplicationID = !string.IsNullOrEmpty(Request["ApplicationID"])
                        ? Request["ApplicationID"]
                        : Request.Headers["ApplicationID"];
                return _ApplicationID;
            }
        }

        private string _CorporatePermissionKey;
        private string CorporatePermissionKey
        {
            get
            {
                if (string.IsNullOrEmpty(_CorporatePermissionKey))
                    _CorporatePermissionKey = !string.IsNullOrEmpty(Request["CorporatePermissionKey"])
                        ? Request["CorporatePermissionKey"]
                        : Request.Headers["CorporatePermissionKey"];
                return _CorporatePermissionKey;
            }
        }

        private string _ApplicationPermissionKey;
        private string ApplicationPermissionKey
        {
            get
            {
                if (string.IsNullOrEmpty(_ApplicationPermissionKey))
                    _ApplicationPermissionKey = !string.IsNullOrEmpty(Request["ApplicationPermissionKey"])
                        ? Request["ApplicationPermissionKey"]
                        : Request.Headers["ApplicationPermissionKey"];
                return _ApplicationPermissionKey;
            }
        }

        private string _Email;
        private string Email
        {
            get
            {
                if (string.IsNullOrEmpty(_Email))
                    _Email = !string.IsNullOrEmpty(Request["Email"])
                        ? Request["Email"]
                        : Request.Headers["Email"];
                return _Email;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string ResponseResult = "[]";
            ResponseResult = requestActivationAppClient();
            Response.Write(ResponseResult);
        }

        private string requestActivationAppClient()
        {
            string RequestDateTime = Validation.getCurrentServerStringDateTime();
            AppClientModel.ResponseResultModel result = new AppClientModel.ResponseResultModel();
            result.ApplicationID = ApplicationID;
            result.CorporatePermissionKey = CorporatePermissionKey;
            result.Email = Email;
            result.RequestDateTime = RequestDateTime;

            try
            {
                if (!libAppClient.checkAuthenCorporatePermissionKey(SID, CompanyCode, CorporatePermissionKey))
                {
                    throw new Exception("Corporate Permission Key \"" + CorporatePermissionKey + "\" not found.");
                }
                //else if (!libAppClient.checkAuthenEmail(SID, CompanyCode, Email))
                //{
                //    throw new Exception("Email \"" + Email + "\" not found.");
                //}
                else if (!libAppClient.checkActivatedApplicationID(SID, CompanyCode, ApplicationID))
                {
                    throw new Exception("Application ID \"" + ApplicationID + "\" can't be use.");
                }
                else
                {
                    AppClientLibrary.GetInstance().ActivationRequestAppClientPermission(
                        ERPWebConfig.GetSID(),
                        ERPWebConfig.GetCompany(),
                        ApplicationID,
                        CorporatePermissionKey,
                        ApplicationPermissionKey,
                        RequestDateTime
                    );

                    result.RequestSuccess = true;
                    result.RequestResultMessage = "#S : Requesting activation successfully, please wait for approval.";
                }

            }
            catch (Exception ex)
            {
                result.RequestSuccess = false;
                result.RequestResultMessage = "#E : " + ex.Message;
            }

            return JsonConvert.SerializeObject(result);
        }

    }
}