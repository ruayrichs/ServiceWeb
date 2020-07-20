using agape.lib.constant;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.WebConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace ServiceWeb.API
{
    public class AbstractWebAPI : System.Web.UI.Page
    {
        public String WorkGroupCode
        {
            get
            {
                return "20170121162748444411";
            }
        }
        private Boolean _LoginByPermission { get; set; }
        private dynamic _dynamicInputs { get; set; }

        #region properties user member varible

        private string _pSID;
        public string _SID
        {
            get
            {
                if (string.IsNullOrEmpty(_pSID))
                    _pSID = !string.IsNullOrEmpty(ERPWAuthentication.SID) ? ERPWAuthentication.SID : ERPWebConfig.GetSID(); // "555";
                return _pSID;
            }
        }

        private string _pCompanyCode;
        public string _CompanyCode
        {
            get
            {
                if (string.IsNullOrEmpty(_pCompanyCode))
                    _pCompanyCode = !string.IsNullOrEmpty(ERPWAuthentication.CompanyCode) ? ERPWAuthentication.CompanyCode : ERPWebConfig.GetCompany(); // "INET";
                return _pCompanyCode;
            }
        }

        private string _pEmployeeCode;
        public string _EmployeeCode
        {
            get
            {
                if (string.IsNullOrEmpty(_pEmployeeCode))
                    _pEmployeeCode = !string.IsNullOrEmpty(ERPWAuthentication.EmployeeCode) ? ERPWAuthentication.EmployeeCode : ""; // "EMP010000003";
                return _pEmployeeCode;
            }
        }

        private string _pUserName;
        public string _UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_pUserName))
                    _pUserName = !string.IsNullOrEmpty(ERPWAuthentication.UserName) ? ERPWAuthentication.UserName : ""; // "focusone";
                return _pUserName;
            }
        }

        private string _pFullNameEN;
        public string _FullNameEN
        {
            get
            {
                if (string.IsNullOrEmpty(_pFullNameEN))
                    _pFullNameEN = !string.IsNullOrEmpty(ERPWAuthentication.FullNameEN) ? ERPWAuthentication.FullNameEN : ""; // "Focus One Administrator";
                return _pFullNameEN;
            }
        }
        
        private string _pFullNameTH;
        public string _FullNameTH
        {
            get
            {
                if (string.IsNullOrEmpty(_pFullNameTH))
                    _pFullNameTH = !string.IsNullOrEmpty(ERPWAuthentication.FullNameTH) ? ERPWAuthentication.FullNameTH : ""; // "focusone";
                return _pFullNameTH;
            }
        }

        private string _pSessionID;
        public string _SessionID
        {
            get
            {
                if (string.IsNullOrEmpty(_pSessionID))
                    _pSessionID = !string.IsNullOrEmpty((string)Session[ApplicationSession.USER_SESSION_ID]) ? (string)Session[ApplicationSession.USER_SESSION_ID] : ""; // "1928297577";
                return _pSessionID;
            }
        }

        private string _pOwnerCode;
        public string _OwnerCode
        {
            get
            {
                if (string.IsNullOrEmpty(_pOwnerCode))
                    _pOwnerCode = !string.IsNullOrEmpty(ERPWAuthentication.Permission.OwnerGroupCode) ? ERPWAuthentication.Permission.OwnerGroupCode : ""; // "focusone";
                return _pOwnerCode;
            }
        }



        #endregion

        #region Parameter Input Request
        
        //from Web
        private string _pChannel;
        public string _Channel
        {
            get
            {
                if (string.IsNullOrEmpty(_pChannel))
                    _pChannel = !string.IsNullOrEmpty(Request["Channel"]) ? Request["Channel"] : Request.Headers["Channel"];
                return _pChannel;
            }
        }

        private string _pPermissionKey;
        private string _PermissionKey
        {
            get
            {
                if (string.IsNullOrEmpty(_pPermissionKey))
                    _pPermissionKey = !string.IsNullOrEmpty(Request["PermissionKey"]) ? Request["PermissionKey"] : Request.Headers["PermissionKey"];
                return _pPermissionKey;
            }
        }


        //From Application
        private string _pCorpoKey;
        private string _CorpoKey
        {
            get
            {
                if (string.IsNullOrEmpty(_pCorpoKey))
                    _pCorpoKey = !string.IsNullOrEmpty(Request["CorporatePermissionKey"]) ? Request["CorporatePermissionKey"] : Request.Headers["CorporatePermissionKey"];
                return _pCorpoKey;
            }
        }

        private string _pAppPermissionKey;
        private string _AppPermissionKey
        {
            get
            {
                if (string.IsNullOrEmpty(_pAppPermissionKey))
                    _pAppPermissionKey = !string.IsNullOrEmpty(Request["ApplicationPermissionKey"]) ? Request["ApplicationPermissionKey"] : Request.Headers["ApplicationPermissionKey"];
                return _pAppPermissionKey;
            }
        }
        
        private string _pApplicationID;
        private string _ApplicationID
        {
            get
            {
                if (string.IsNullOrEmpty(_pApplicationID))
                    _pApplicationID = !string.IsNullOrEmpty(Request["ApplicationID"]) ? Request["ApplicationID"] : Request.Headers["ApplicationID"];
                return _pApplicationID;
            }
        }
        
        #endregion

        public AbstractWebAPI()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _LoginByPermission = false;
                if (string.IsNullOrEmpty(ERPWAuthentication.EmployeeCode))
                {
                    _LoginByPermission = true;
                }

                #region Set Value Case Raw Json
                
                _dynamicInputs = null;
                if (!Request.ContentType.Contains("form-data"))
                {
                    using (Stream receiveStream = Request.InputStream)
                    {
                        using (StreamReader readStream = new StreamReader(receiveStream, Request.ContentEncoding))
                        {
                            try
                            {
                                _dynamicInputs = JsonConvert.DeserializeObject<dynamic>(readStream.ReadToEnd());
                            }
                            catch (Exception)
                            {
                                _dynamicInputs = null;
                            }

                        }
                    }

                    if (_dynamicInputs != null)
                    {
                        _pChannel = Convert.ToString(_dynamicInputs["Channel"]);
                        _pPermissionKey = Convert.ToString(_dynamicInputs["PermissionKey"]);
                        _pCorpoKey = Convert.ToString(_dynamicInputs["CorporatePermissionKey"]);
                        _pAppPermissionKey = Convert.ToString(_dynamicInputs["ApplicationPermissionKey"]);
                        _pApplicationID = Convert.ToString(_dynamicInputs["ApplicationID"]);
                    }
                }
                #endregion
            }
        }
        
        public bool checkPermission()
        {
            bool HasPermission = false;
            try
            {
                string Channel = _Channel;
                if (Channel != TierZeroLibrary.TIER_ZERO_CHANNEL_APPCLIENT)
                {
                    ERPW_API_Permission_Token_Key_DAO libPermission = new ERPW_API_Permission_Token_Key_DAO();
                    string PermissionKey = _PermissionKey;
                    DataTable dtPermission = libPermission.getOneByKey(PermissionKey);

                    if (dtPermission.Rows.Count > 0)
                    {
                        _pSID = Convert.ToString(dtPermission.Rows[0]["SID"]);
                        _pCompanyCode = Convert.ToString(dtPermission.Rows[0]["CompanyCode"]);
                        _pUserName = Convert.ToString(dtPermission.Rows[0]["UserName"]);
                        _pEmployeeCode = Convert.ToString(dtPermission.Rows[0]["EmployeeCode"]);
                        _pOwnerCode = Convert.ToString(dtPermission.Rows[0]["OwnerService"]);
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
                    string CorpoKey = _CorpoKey;
                    string AppKey = _AppPermissionKey;
                    string AppID = _ApplicationID;
                    //libAppClient.chec
                    if (AppClientLibrary.GetInstance().checkAuthenCreatedTicket(_SID, _CompanyCode, CorpoKey, AppKey, AppID))
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

        public T GetInputDataModel<T>(HttpRequest xRequest) where T : class
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                if (_dynamicInputs != null)
                {
                    JObject results = JsonConvert.DeserializeObject<JObject>(Convert.ToString(_dynamicInputs), settings);
                    foreach (string p in xRequest.QueryString.AllKeys)
                    {
                        if (!results.ContainsKey(p))
                        {
                            results.Add(p, Request[p]);
                        }
                    }
                    return JsonConvert.DeserializeObject<T>(results.ToString(), settings);
                }
                else
                {
                    JObject results = new JObject();
                    foreach (string p in xRequest.QueryString.AllKeys)
                    {
                        if (p != null)
                        {
                            results.Add(p, Request[p]);
                        }
                    }
                    foreach (string p in xRequest.Form.AllKeys)
                    {
                        if (p != null && !results.ContainsKey(p))
                        {
                            results.Add(p, Request[p]);
                        }
                    }
                    return JsonConvert.DeserializeObject<T>(results.ToString(), settings);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        #region Response Result Json

        #region Response OK

        public void OK(string message)
        {
            WriteResult<string>(HttpStatusCode.OK, message,null);
        }
        public void OK<T>(string message,T result)
        {
            WriteResult(HttpStatusCode.OK,message, result);
        }

        #endregion

        #region Response BadRequest

        public void BadRequest(string message)
        {
            WriteResult<string>(HttpStatusCode.BadRequest, message, null);
        }

        public void BadRequest<T>(string message, T result)
        {
            WriteResult(HttpStatusCode.BadRequest, message, result);
        }

        #endregion
        
        private void WriteResult<T>(HttpStatusCode status,string message,T result)
        {
            JObject jObj = new JObject();
            jObj.Add("status", status.ToString());
            jObj.Add("message", message);

            #region Check Type Result

            if (result != null)
            {
                if (result.GetType() == typeof(string))
                {
                    jObj.Add("result", Convert.ToString(result));
                }
                else
                {
                    string jsondata = JsonConvert.SerializeObject(result);
                    JToken token = JToken.Parse(jsondata);
                    if (token is JArray)
                    {
                        jObj.Add("result", JsonConvert.DeserializeObject<JArray>(jsondata));
                    }
                    else
                    {
                        jObj.Add("result", JsonConvert.DeserializeObject<JObject>(jsondata));
                    }
                }
            }
            
            #endregion

            jObj.Add("result_time", DateTime.Now.ToString("dd/mm/yyyy HH:mm:ss tt (K)"));
            Response.ContentType = "application/json";
            Response.StatusCode = (int)status;
            Response.Write(jObj);
            if (_LoginByPermission)
            {
                Session.Abandon();
            }
        }
        
        #endregion

    }
}