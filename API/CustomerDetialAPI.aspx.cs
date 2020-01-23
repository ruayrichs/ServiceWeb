using Agape.Lib.DBService;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.WebConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceWeb.Service;
using System;
using System.Data;

namespace ServiceWeb.API
{
    public partial class CustomerDetialAPI : System.Web.UI.Page
    {
        private  DBService dbservice = new DBService();
        #region Status Code
        private const String STATUS_SUCCESS = "S";
        private const String STATUS_ERROR = "E";
        private const String STATUS_WARNING = "W";
        #endregion

        #region Primary Key
        private  string _SID;
        private  string SID
        {
            get
            {
                if (string.IsNullOrEmpty(_SID))
                    _SID = !string.IsNullOrEmpty(ERPWAuthentication.SID) ? ERPWAuthentication.SID : ERPWebConfig.GetSID(); // "555";
                return _SID;
            }
        }

        private  string _CompanyCode;
        private  string CompanyCode
        {
            get
            {
                if (string.IsNullOrEmpty(_CompanyCode))
                    _CompanyCode = !string.IsNullOrEmpty(ERPWAuthentication.CompanyCode) ? ERPWAuthentication.CompanyCode : ERPWebConfig.GetCompany(); // "INET";
                return _CompanyCode;
            }
        }

        private  string _UserName;
        private  string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_UserName))
                    _UserName = !string.IsNullOrEmpty(ERPWAuthentication.UserName) ? ERPWAuthentication.UserName : ""; // "focusone";
                return _UserName;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Response.ContentType = "application/json";
                string Channel = !string.IsNullOrEmpty(Request["Channel"]) ? Request["Channel"] : Request.Headers["Channel"];
                string PermissionKey = !string.IsNullOrEmpty(Request["PermissionKey"]) ? Request["PermissionKey"] : Request.Headers["PermissionKey"];
                string Search = !string.IsNullOrEmpty(Request["Search"]) ? Request["Search"] : Request.Headers["Search"];

                searchCusomer(
                    PermissionKey,
                    Search
                );
            }
            catch (Exception ex)
            {
                ResponseResultError(ex.Message);
            }
        }
        
        public void searchCusomer(string p, string Search)
        {
            Boolean LoginByPermission = false;
            if (string.IsNullOrEmpty(ERPWAuthentication.EmployeeCode))
            {
                LoginByPermission = true;
            }
            checkPermission(p);
            string sql = "";
            if (!string.IsNullOrEmpty(Search))
            {
                sql = @"
                        and (
	                        a.CustomerCode like '%"+ Search + @"%' 
	                        or a.CustomerName like '%" + Search + @"%' 
	                        or b.TelNo1 like '%" + Search + @"%' 
	                        or b.Mobile like '%" + Search + @"%' 
	                        or b.EMail like '%" + Search + @"%' 

	                        or d.NAME1 like '%" + Search + @"%' 
	                        or d.NickName like '%" + Search + @"%' 
	                        or e.PHONENUMBER like '%" + Search + @"%' 
	                        or f.EMAIL like '%" + Search + @"%' 
	                        ) ";
            }
                
            sql = @"SELECT a.CustomerCode, a.CustomerName
                    FROM master_customer a with (nolock)
                    INNER JOIN master_customer_general b with (nolock)
		                on a.SID = b.SID AND a.CompanyCode = b.CompanyCode AND a.CustomerCode = b.CustomerCode
	                left join CONTACT_MASTER c with (nolock)
		                on a.SID = c.SID and a.CompanyCode = c.COMPANYCODE and a.CustomerCode = c.BPCODE
	                left join CONTACT_DETAILS d with (nolock)
		                on c.SID = d.SID AND c.AOBJECTLINK = d.AOBJECTLINK
	                left join CONTACT_PHONE e with (nolock)
		                on c.SID = e.SID AND d.BOBJECTLINK = e.BOBJECTLINK
	                left join CONTACT_EMAIL f with (nolock)
		                on d.SID = f.SID AND  d.BOBJECTLINK = f.BOBJECTLINK
                    WHERE a.SID = '"+ SID + "' AND a.CompanyCode = '"+ CompanyCode + @"' and b.Active = 'True'
	                "+ sql + @"
	                group by a.CustomerCode, a.CustomerName ";
            DataTable dt = dbservice.selectDataFocusone(sql);
            ResponseResultArray(STATUS_SUCCESS, "", dt);
            if (LoginByPermission)
            {
                Session.Abandon();
            }
           
        }

        #region Properties Data 
        private  void checkPermission(string PermissionKey)
        {
            bool HasPermission = false;
            ERPW_API_Permission_Token_Key_DAO libPermission = new ERPW_API_Permission_Token_Key_DAO();
            DataTable dtPermission = libPermission.getOneByKey(PermissionKey);
            if (dtPermission.Rows.Count > 0)
            {
                _SID = dtPermission.Rows[0]["SID"].ToString();
                _CompanyCode = dtPermission.Rows[0]["CompanyCode"].ToString();
                _UserName = dtPermission.Rows[0]["UserName"].ToString();

            }

            if (string.IsNullOrEmpty(ERPWAuthentication.UserName))
            {
                SystemModeControlService.SystemModeEntities mode = SystemModeControlService.getInstanceMode("Link");
                ERPWAutoLoginService loginService = new ERPWAutoLoginService(_SID, _UserName, mode);
                HasPermission = true;
            }
            else if (!string.IsNullOrEmpty(_UserName) && !string.IsNullOrEmpty(ERPWAuthentication.UserName))
            {
                HasPermission = true;
            }

            if (!HasPermission)
            {
                throw new Exception("No Permission.");
            }
        }
        #endregion

        #region Respons Display

        private void ResponseResultError(string message)
        {
            JArray data = new JArray();
            JObject result = new JObject();
            result.Add("status", STATUS_ERROR);
            result.Add("message", message);
            result.Add("result", "");
            Response.Write(result.ToString());
            GC.Collect();
        }

        private void ResponseResultArray<T>(string statuscode, string message,T Result)
        {
            JObject result = new JObject();
            result.Add("status", statuscode);
            result.Add("message", message);
            result.Add("result", JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(Result)));
            Response.Write(result.ToString());
            GC.Collect();
        }

        #endregion
    }
}