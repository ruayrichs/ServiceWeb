using agape.lib.constant;
using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.WebConfig;
using Link.Lib.Model.Model.Timeline;
using Newtonsoft.Json;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API
{
    public partial class ReplyEmailToPostCommentAPI : System.Web.UI.Page
    {
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
        protected void Page_Load(object sender, EventArgs e)
        {
            Boolean LoginByPermission = false;
            if (string.IsNullOrEmpty(ERPWAuthentication.EmployeeCode))
            {
                LoginByPermission = true;
            }

            if (!checkPermission())
            {
                ResultPost resultTierZero = new ResultPost();
                resultTierZero.ResultMessage = "No Permission.";
                resultTierZero.PostSuccess = false;
                string responseJson = JsonConvert.SerializeObject(resultTierZero);
                return;
            }

            List<String> listResponse = new List<string>();
            Response.ContentType = "application/json";

            // TODO

            //string result = postRemark(null);
            //listResponse.Add(result);

            string Channel = !string.IsNullOrEmpty(Request["Channel"]) ? Request["Channel"] : Request.Headers["Channel"];

            //string JsonData = !string.IsNullOrEmpty(Request["JsonData"]) ? Request["JsonData"] : Request.Headers["JsonData"];
            //List<ObjectsEmailData> enData = JsonConvert.DeserializeObject<List<ObjectsEmailData>>(JsonData);
            //enData.ForEach(r =>
            //{
            //    string result = postRemark(r);
            //    listResponse.Add(result);
            //});

            if (Channel == TierZeroLibrary.TIER_ZERO_CHANNEL_EMAIL)
            {
                string JsonData = !string.IsNullOrEmpty(Request["JsonData"]) ? Request["JsonData"] : Request.Headers["JsonData"];
                List<ObjectsEmailData> enData = JsonConvert.DeserializeObject<List<ObjectsEmailData>>(JsonData);
                enData.ForEach(r =>
                {
                    string result = postRemark(r);
                    listResponse.Add(result);
                });
            }
            else
            {

            }

            Response.Write("[" + string.Join(",", listResponse) + "]");
            if (LoginByPermission)
            {
                Session.Abandon();
            }
        }

        private string postRemark(ObjectsEmailData data)
        {
            ResultPost responseResult = new ResultPost();
            try
            {
                string activityID = AfterSaleService.getInstance().getLastActivityServiceCall(CompanyCode, data.MessageID, "", "");

                //ServiceTicketLibrary.GetInstance().PostActivityRemarkRefEmail(
                //    SID, CompanyCode, EmployeeCode, activityID, 
                //    ServiceTicketLibrary.ActivityRemarkRefEmail.INCOMING, data.RefID
                //);


                Timeline attachment = new Timeline();
                string refEmailCode = Guid.NewGuid().ToString("D") + activityID;

                ServiceTicketLibrary.GetInstance().PostActivityRemarkEmail(
                    SID,
                    CompanyCode,
                    CompanyCode,
                    data.MessageID,
                    activityID,
                    refEmailCode,
                    data.EmailFrom,
                    EmployeeCode,
                    new List<string> { data.EmailFrom },
                    new List<string>(),
                    HttpUtility.UrlDecode(data.EmailSubject),
                    HttpUtility.UrlDecode(data.EmailBody),
                    null,
                    ServiceTicketLibrary.ActivityRemarkRefEmail.INCOMING,
                    FullNameEN
                );

                responseResult.PostSuccess = true;
                responseResult.ResultMessage = "Success";
                responseResult.ErrorException = "";
            }
            catch (Exception ex)
            {
                responseResult.PostSuccess = false;
                responseResult.ResultMessage = "Error";
                responseResult.ErrorException = ex.Message;
            }

            string responseJson = JsonConvert.SerializeObject(responseResult);
            return responseJson;
        }

        private bool checkPermission()
        {
            bool HasPermission = false;
            ERPW_API_Permission_Token_Key_DAO libPermission = new ERPW_API_Permission_Token_Key_DAO();
            string PermissionKey = !string.IsNullOrEmpty(Request["PermissionKey"]) ? Request["PermissionKey"] : Request.Headers["PermissionKey"];
            DataTable dtPermission = libPermission.getOneByKey(PermissionKey);

            string _SID = "";
            string _UserName = "";

            if (dtPermission.Rows.Count > 0)
            {
                _SID = dtPermission.Rows[0]["SID"].ToString();
                _UserName = dtPermission.Rows[0]["UserName"].ToString();
            }

            try
            {
                string Channel = !string.IsNullOrEmpty(Request["Channel"]) ? Request["Channel"] : Request.Headers["Channel"];

                if (string.IsNullOrEmpty(ERPWAuthentication.UserName))
                {
                    if (Channel == TierZeroLibrary.TIER_ZERO_CHANNEL_WEB)
                    {
                        if (Request.UrlReferrer != null && Request.UrlReferrer.HostNameType == UriHostNameType.Dns)
                        {
                            IPAddress[] listIP = Dns.GetHostAddresses(Request.UrlReferrer.Host);
                        }
                    }
                    else if (Channel == TierZeroLibrary.TIER_ZERO_CHANNEL_EMAIL)
                    {

                    }
                    else if (Channel == TierZeroLibrary.TIER_ZERO_CHANNEL_SYSTEM)
                    {

                    }

                    HasPermission = true;
                    if (HasPermission)
                    {
                        ERPWAutoLoginService.getCurrentInstance().AutoLoginAPI(_SID, _UserName);
                    }
                }
            }
            catch (Exception)
            {
                HasPermission = false;
            }

            return true;
        }

        protected class ResultPost
        {
            public string ResultMessage { get; set; }
            public bool PostSuccess { get; set; }
            public string ErrorException { get; set; }
        }        

        protected class ObjectsEmailData
        {
            public string MessageID { get; set; }
            public string EmailFrom { get; set; }
            public string EmailSubject { get; set; }
            public string EmailBody { get; set; }
        }
    }
}