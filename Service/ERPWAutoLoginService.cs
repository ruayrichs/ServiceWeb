﻿using agape.entity.UserProfile;
using agape.lib.constant;
using agape.proxy.data.dataset;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;

using ERPW.Lib.Authentication;
using ERPW.Lib.Authentication.Entity;
using ERPW.Lib.F1WebService;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.F1WebService.UserLoginService;
using ERPW.Lib.WebConfig;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Configuration;
using System.Globalization;
using System.DirectoryServices.AccountManagement;
using System.Configuration;
using ERPW.Lib.Master;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

namespace ServiceWeb.Service
{
    public class ERPWAutoLoginService
    {
        private UserLoginService loginService = ERPW.Lib.F1WebService.F1WebService.getUserLoginService();        
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        private DBService dbService = new DBService();
      
        private string SID = "";
        private string UserName = "";
        private string SystemMode = "";

        private static ERPWAutoLoginService _instance;

        public static ERPWAutoLoginService getCurrentInstance()
        {
            if (_instance == null)
            {
                _instance = new ERPWAutoLoginService();
            }

            return _instance;
        }

        public static void CheckSessionAndAutoLogin()
        {
            if (string.IsNullOrEmpty(ERPWAuthentication.EmployeeCode))
            {
                try
                {
                    new ERPWAutoLoginService();
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Response.Redirect((HttpContext.Current.Handler as Page).ResolveUrl("~/Login.aspx"));
                    HttpContext.Current.Response.End();
                }
            }
        }

        public ERPWAutoLoginService()
        {
            SystemModeControlService.SystemModeEntities mode = SystemModeControlService.GetCurrentMode;

            string _requestSID = SystemModeControlService.GetCurrentLoggedInSID;
            string _userName = SystemModeControlService.GetCurrentLoggedInEmail;

            if (string.IsNullOrEmpty(_userName))
            {
                throw new Exception("ไม่สามารถทำการ Auto Login ได้ เนื่องจากไม่พบชื่อผู้ใช้ในคุกกี้ หรือคุกกี้หมดอายุ");
            }
            if (string.IsNullOrEmpty(_requestSID))
            {
                throw new Exception("ไม่สามารถทำการ Auto Login ได้ เนื่องจากไม่พบ System ID ในคุกกี้ หรือคุกกี้หมดอายุ");
            }

            AutoLogin(_requestSID, _userName, mode);
        }

        public ERPWAutoLoginService(string requestSID, string username, SystemModeControlService.SystemModeEntities mode)
        {
            AutoLogin(requestSID, username, mode);
        }

        public void AutoLoginAPI(string requestSID, string requestUserName)
        {
            SystemModeControlService.SystemModeEntities mode = SystemModeControlService.getInstanceMode("Link");

            SID = requestSID;
            UserName = requestUserName;
            SystemMode = mode.Mode;

            string companyCode = ERPWebConfig.GetCompany();

            string sessionId = F1WebService.getUserLoginService().AddUserSession(SID, UserName);

            AddUserSession(sessionId, UserName);

            ERPWAuthentication.SetERPWAuthentication(SID, companyCode, UserName);

            GetDataImg(companyCode, ERPWAuthentication.EmployeeCode);

        }
        private void AutoLogin(string requestSID, string requestUserName, SystemModeControlService.SystemModeEntities mode)
        {
            SID = requestSID;
            UserName = requestUserName;
            SystemMode = mode.Mode;

            string companyCode = ERPWebConfig.GetCompany();

            string sessionId = F1WebService.getUserLoginService().AddUserSession(SID, UserName);

            AddUserSession(sessionId, UserName);

            ERPWAuthentication.SetERPWAuthentication(SID, companyCode, UserName);

            #region check Active InActive User
            bool isInActive = false;
            string err_msg = "";
            DataTable dtCheckActive = dbService.selectDataFocusone(
                "select * from master_employee_branchandposition " +
                "where SID='" + SID + "' " +
                "and CompanyCode='" + companyCode + "' " +
                "and EmployeeCode='" + ERPWAuthentication.EmployeeCode + "'");

            if (dtCheckActive.Rows.Count == 0 || dtCheckActive.Rows[0]["Status"].ToString().ToLower() != "Active".ToLower())
            {
                isInActive = true;
                err_msg = ("User Name : " + ERPWAuthentication.UserName + " ข้อมูลพนักงานถูก InActive ไม่สามารถเข้าใช้งานได้");
            }
            else if (string.IsNullOrEmpty(Convert.ToString(dtCheckActive.Rows[0]["StartDate"])) ||
                string.IsNullOrEmpty(Convert.ToString(dtCheckActive.Rows[0]["ToDate"])))
            {
                isInActive = true;
                err_msg = ("User Name : " + ERPWAuthentication.UserName + " ข้อมูลพนักงานหมดอายุการใช้งาน");
            }
            else if (Convert.ToInt32(dtCheckActive.Rows[0]["ToDate"]) < Convert.ToInt32(Validation.getCurrentServerStringDateTime().Substring(0, 8)))
            {
                isInActive = true;
                err_msg = ("User Name : " + ERPWAuthentication.UserName + " ข้อมูลพนักงานหมดอายุการใช้งาน");
            }
            else if (Convert.ToInt32(dtCheckActive.Rows[0]["StartDate"]) > Convert.ToInt32(Validation.getCurrentServerStringDateTime().Substring(0, 8)))
            {
                isInActive = true;
                err_msg = ("User Name : " + ERPWAuthentication.UserName + " " +
                    "ข้อมูลพนักงานจะสามารถใช้งานได้ในวันที่ " +
                    "" + Validation.Convert2DateDisplay(Validation.getCurrentServerStringDateTime().Substring(0, 8)));
            }
            if (isInActive)
            {
                HttpContext.Current.Response.Cookies["SystemControlService_SID"].Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies["SystemControlService_Email"].Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Session.Abandon();
                throw new Exception(err_msg);
            }
            #endregion

            GetDataImg(companyCode, ERPWAuthentication.EmployeeCode);
        }

        protected void GetDataImg(string companycode, string employeecode)
        {
            string attendanceYear = Validation.getCurrentServerStringDateTime().Substring(0, 4);

            v_EmployeeDataset empCallDataSet = new v_EmployeeDataset();

            Object[] objParam = new Object[] {
                icmconstants.ICM_CONST_HR_SELECTFROMEMPLOYEE,
                HttpContext.Current.Session[ApplicationSession.USER_SESSION_ID],
                HttpContext.Current.Session[ApplicationSession.USER_SESSION_SID],
                "",companycode,employeecode,attendanceYear,"" };            
            DataSet[] objDataSet = new DataSet[] { empCallDataSet };
            DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);

            if (objReturn != null)
            {
                empCallDataSet.Clear();
                empCallDataSet.Merge(objReturn);
                if (empCallDataSet.master_employee_personal_data.Rows.Count > 0)
                {
                    if (empCallDataSet.master_employee_personal_data.Rows[0]["ImageByte"] != null && empCallDataSet.master_employee_personal_data.Rows[0]["ImageByte"].ToString() != "")
                    {
                        System.Web.HttpContext.Current.Session["Image_Profile"] = (Byte[])empCallDataSet.master_employee_personal_data.Rows[0]["ImageByte"];
                        HttpContext.Current.Session["Image_Profile"] = (Byte[])empCallDataSet.master_employee_personal_data.Rows[0]["ImageByte"];
                    }
                }
            }
        }

        public void AddUserSession(string sessionId, string username)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new Exception("Session ID Expired.");
            }

            UserSessionBean sessionBean = loginService.getUserSessionBean(sessionId);

            HttpContext.Current.Session[ApplicationSession.USER_SESSION_BEAN] = sessionBean;
            HttpContext.Current.Session[ApplicationSession.USER_SESSION_ID] = sessionId;
            HttpContext.Current.Session[ApplicationSession.USER_SESSION_SID] = SID;
            HttpContext.Current.Session[ApplicationSession.USER_SESSION_TYPE] = "";
            HttpContext.Current.Session[ApplicationSession.USER_SESSION_LOGIN] = username;
            HttpContext.Current.Session["username_login_date"] = Validation.getCurrentServerDate();
            HttpContext.Current.Session["username_login_time"] = Validation.getCurrentServerTime();
        }

        public static void updateLastLogingDate(string sid, string username)
        {
            string currentDateTime = Validation.getCurrentServerStringDateTime();
            string _date = currentDateTime.Substring(0, 8);
            string _time = currentDateTime.Substring(8, 6);

            string sql = "update usr01 set Lastlogin_date='" + _date + "',LastLogin_time='" + _time + "' where sid='" + sid + "' and username='" + username + "' ";
            DBService dbS = new DBService();
            dbS.executeSQLForFocusone(sql);
        }

        public string addNewSessionIDBeforeF1(string sid, string username)
        {
            string sessionid = loginService.AddUserSession(sid, username);
            HttpContext.Current.Session[ApplicationSession.USER_SESSION_ID] = sessionid;
            return sessionid;
        }

        //=================================== Authentication Actice directory ====================================
        public static bool authenActiveDirectory(string userName, string passWord,string sid,string companyCode)
        {
            bool isLogin = false;
             string domainName = "sos.local";
                string strCommu;
                
                strCommu = "LDAP://203.151.101.250";
                string userStr = (domainName + "\\" + userName);
                DirectoryEntry entry = new DirectoryEntry(strCommu, userStr, passWord);
                DirectorySearcher search = new DirectorySearcher(entry);
                //search.Filter = ("(SAMAccountName="+ userName + ")");
                search.Filter = "(&(objectClass=user)(cn=" + userName + "))";
                SearchResult result = search.FindOne();
               

            //string connection = ConfigurationManager.ConnectionStrings["ADConnection"].ToString();
            //DirectorySearcher dssearch = new DirectorySearcher(connection);
            //dssearch.Filter = "(sAMAccountName=" + userName + ")";
            //SearchResult sresult = dssearch.FindOne();
            //DirectoryEntry dsresult = sresult.GetDirectoryEntry();
            //string email = dsresult.Properties["mail"][0].ToString();

            if (result != null)
                {
                    if (!serchUser(userName))
                    {
                        migrateDB(result, userName, sid, companyCode);
                    }
                //SystemModeControlService.SystemModeEntities mode = SystemModeControlService.getInstanceMode("Link");
                //SystemMode = mode.Mode;

                string sessionId = F1WebService.getUserLoginService().AddUserSession(sid, userName);
                //UserSessionBean sessionBean = loginService.getUserSessionBean(sessionId);
               
                //HttpContext.Current.Session[ApplicationSession.USER_SESSION_BEAN] = sessionBean;
                HttpContext.Current.Session[ApplicationSession.USER_SESSION_ID] = sessionId;
                HttpContext.Current.Session[ApplicationSession.USER_SESSION_SID] = sid;
                HttpContext.Current.Session[ApplicationSession.USER_SESSION_TYPE] = "";
                HttpContext.Current.Session[ApplicationSession.USER_SESSION_LOGIN] = userName;
                HttpContext.Current.Session["username_login_date"] = Validation.getCurrentServerDate();
                HttpContext.Current.Session["username_login_time"] = Validation.getCurrentServerTime();

                ERPWAuthentication.SetERPWAuthentication(sid, companyCode, userName);

                //GetDataImg(companyCode, ERPWAuthentication.EmployeeCode);
                //AutoLoginAPI(sid,userName);
                 isLogin = true;
                }

            return isLogin;
        }
        public static string GetProperty(SearchResult searchResult,string PropertyName)
        {
            if (searchResult.Properties.Contains(PropertyName))
            {
                return searchResult.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        protected static void migrateDB (SearchResult result,string userName,string sid, string companyCode)
        {

            string EmployeeHierarchy = "";
            string NamePreFix = "";
            string LastName = "";
            string FirstName_TH = "";
            string LastName_TH = "";
            string Position = "";
            string Branch = "";
            string StartDate = "";
            string ToDate = "";
            string PersonAssignCode = "";
            string PersonArea = "";
            string CostCenter = "";
            string EEGroupCode = "";
            string EESubGroupCode = "";
            string ChangeDate = "";
            string ChangeDateDetail = "";
            string CREATED_BY = "AD_USER";
            string UPDATED_BY = "";
            string CostCenterGroup = "";
            string FloatCode = "";
            string VendorCode = "";
            string CustomerCode = "";
            string ActiveOn = "False";
            string IsSaleRepresent = "False";
            string roleCode = "EndUser"; //role end user

            string userEmail = GetProperty(result, "userprincipalname");



                string firstName = GetProperty(result, "givenName");
                string dateStr = DateTime.Now.ToString("yyyyMMddHHmmss", new CultureInfo("en-US"));
                string fisyear = DateTime.Now.ToString("yyyy", new CultureInfo("en-US"));
                string empGroup = "EMP01";
                string sqlMasterEM = @"INSERT INTO master_employee
                               VALUES ('"+sid+"','"+companyCode+"','"+userName+@"',
                                    '"+ EmployeeHierarchy + "','"+ NamePreFix + "','"+firstName+"','"+LastName+@"',
                                    '"+ FirstName_TH + "','"+ LastName_TH + "','"+ Position + "','"+Branch+@"',
                                    '"+ StartDate + "','"+ ToDate + "','"+ PersonAssignCode + "','"+ PersonArea +@"',
                                    '"+ CostCenter + "','"+ EEGroupCode + "','"+ EESubGroupCode + "','"+ ChangeDate +@"',
                                    '"+ ChangeDateDetail + "','"+ CREATED_BY + "','"+ UPDATED_BY + "','" +dateStr+"','"+dateStr+@"',
                                    '"+ userName + "','"+ CostCenterGroup + "','"+ FloatCode + "','"+ VendorCode + "','"+ CustomerCode +@"',
                                    '"+empGroup+"','"+fisyear+ "','','"+ ActiveOn + "','"+ IsSaleRepresent + "')";
                DBService dbService = new DBService();

                dbService.selectDataFocusone(sqlMasterEM);

            string sqlAuth_user = @"INSERT INTO auth_user
                                VALUES ('"+sid+"','"+userName+"','"+userName+"','"+firstName+"','"+dateStr+"','AD_USER','"+dateStr+"','UPDATE_BY_AD_USER','','')";
            dbService.selectDataFocusone(sqlAuth_user);

            string currentDateTime = Validation.getCurrentServerStringDateTime();
            string _current_date = currentDateTime.Substring(0, 8);
            string _current_time = currentDateTime.Substring(8, 6);
            string sql_usr01 = @"INSERT INTO usr01
                                VALUES ('" + sid + "','" + userName + "','" + _current_date + "','" + _current_time + "','" + _current_date + "','" + _current_time + "','A','" + UserManagementService.SYSTEM_ERP + "','C','" + currentDateTime + "', 'AD_USER', '', '')";
            dbService.selectDataFocusone(sql_usr01);

            string sqlAuthProfileMapping = @"INSERT INTO auth_userprofilemapping
                                                VALUES('"+sid+"','"+userName+"','000001','AD_USER','"+dateStr+"','AD_USER','AD_USER')";
            dbService.selectDataFocusone(sqlAuthProfileMapping);

            string sqlRole_Mapping_Employee = @"INSERT INTO ERPW_Role_Maping_Employee
                                                   VALUES ('"+sid+"','"+companyCode+"','"+userName+"','"+roleCode+"','CREATE_AD_USER','"+dateStr+ "','UPDATE_BY_AD_USER','" + dateStr + "','')";
            dbService.selectDataFocusone(sqlRole_Mapping_Employee);

            string sqlMaster_employee_address = @"INSERT INTO master_employee_address
                                                    VALUES ('"+sid+"','"+companyCode+"','"+userName+"','','','','','','','','THA','','','CREATE_BY_AD_USER','UPDATE_BY_AD_USER','"+dateStr+"','"+dateStr+"','"+userEmail+"','','"+empGroup+"')";
            dbService.selectDataFocusone(sqlMaster_employee_address);
        } 
        protected static bool serchUser(string userName)
        {
            DBService dbService = new DBService();
            string sql = @"SELECT EmployeeCode from master_employee WHERE EmployeeCode = '"+userName+"'";
            DataTable dt = dbService.selectDataFocusone(sql);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        #region Login ONEID

        private bool IsOneIdUserAlreadyExists(string userName)
        {
            string sql = @"SELECT EmployeeCode from master_employee WHERE UserName = '" + userName + "' AND CREATED_BY = 'ONEID_USER'";
            DataTable dt = dbService.selectDataFocusone(sql);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public string getAPIErrorMsg(WebException we)
        {
            var httpWebResponse = (HttpWebResponse)we.Response;
            using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                JObject loginResObj = JObject.Parse(result);
                string errMsg = (string)loginResObj["errorMessage"];
                return errMsg;
            }
        }
        public string oneIdLogin(string userName, string pwd, string sid, string companyCode)
        {
            OneIdAPILoginReqModel loginReqModel = new OneIdAPILoginReqModel
            {
                grant_type = "password",
                client_id = ConfigurationManager.AppSettings["ONE_ID_CLIENT_ID"],
                client_secret = ConfigurationManager.AppSettings["ONE_ID_CLIENT_SECRET"],
                username = userName,
                password = pwd
            };

            JObject loginResObj = oneIdApiLogin(loginReqModel);
            string accessTokenStr = (string)loginResObj["access_token"];
            string oneidUser = (string)loginResObj["username"];
            string oneIdAccountId = (string)loginResObj["account_id"];
            string userNameInDb = oneidUser.Length <= 14 ? string.Concat(oneidUser, "@ONEID") : string.Concat(oneIdAccountId, "@ONEID"); // ONEID username มีได้ตั้งแต่ 6 - 100 characters แต่ local db เก็บได้แค่ 20 characters  

            if (!IsOneIdUserAlreadyExists(userNameInDb)) // check user info in db
            {
                JObject profileResObjStr = getProfileOneId(accessTokenStr);
                migrateDB(profileResObjStr, sid, companyCode, userNameInDb);
            }

            string sessionId = F1WebService.getUserLoginService().AddUserSession(sid, userNameInDb);
            //UserSessionBean sessionBean = loginService.getUserSessionBean(sessionId);

            //HttpContext.Current.Session[ApplicationSession.USER_SESSION_BEAN] = sessionBean;
            HttpContext.Current.Session[ApplicationSession.USER_SESSION_ID] = sessionId;
            HttpContext.Current.Session[ApplicationSession.USER_SESSION_SID] = sid;
            HttpContext.Current.Session[ApplicationSession.USER_SESSION_TYPE] = "";
            HttpContext.Current.Session[ApplicationSession.USER_SESSION_LOGIN] = userNameInDb;
            HttpContext.Current.Session["username_login_date"] = Validation.getCurrentServerDate();
            HttpContext.Current.Session["username_login_time"] = Validation.getCurrentServerTime();

            ERPWAuthentication.SetERPWAuthentication(sid, companyCode, userNameInDb);

            return userNameInDb;
        }
        private JObject getProfileOneId(string accessToken)
        {
            string API_URL = ConfigurationManager.AppSettings["ONE_ID_API_GET_USER_PROFILE_URL"];

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(API_URL);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers["Authorization"] = "Bearer " + accessToken;
            httpWebRequest.Method = "GET";

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                JObject profiledata = JObject.Parse(result);
                return profiledata;
            }
        }
        private JObject oneIdApiLogin(OneIdAPILoginReqModel loginReqModel)
        {
            string API_URL = ConfigurationManager.AppSettings["ONE_ID_API_LOGIN_URL"];

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(API_URL);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var json = JsonConvert.SerializeObject(loginReqModel);

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //if (httpResponse.StatusCode == HttpStatusCode.Unauthorized || httpResponse.StatusCode == HttpStatusCode.BadRequest)
            //{
            //    throw new Exception(httpResponse.StatusDescription);
            //}

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                JObject loginResObj = JObject.Parse(result);
                return loginResObj;
            }
        }
        private void migrateDB(JObject profiledata, string sid, string companyCode, string userNameInDb)
        {
            string EmployeeHierarchy = "";
            string NamePreFix = "";
            string firstName = !String.IsNullOrEmpty((string)profiledata["first_name_eng"]) ? (string)profiledata["first_name_eng"] : "";
            string LastName = !String.IsNullOrEmpty((string)profiledata["last_name_eng"]) ? (string)profiledata["last_name_eng"] : "";
            string FirstName_TH = !String.IsNullOrEmpty((string)profiledata["first_name_th"]) ? (string)profiledata["first_name_th"] : "";
            string LastName_TH = !String.IsNullOrEmpty((string)profiledata["last_name_th"]) ? (string)profiledata["last_name_th"] : "";
            string Position = "";
            string Branch = "";
            string StartDate = "";
            string ToDate = "";
            string PersonAssignCode = "";
            string PersonArea = "";
            string CostCenter = "";
            string EEGroupCode = "";
            string EESubGroupCode = "";
            string ChangeDate = "";
            string ChangeDateDetail = "";
            string CREATED_BY = "ONEID_USER";
            string UPDATED_BY = "ONEID_USER";
            string CostCenterGroup = "";
            string FloatCode = "";
            string VendorCode = "";
            string CustomerCode = "";
            string ActiveOn = "False";
            string IsSaleRepresent = "False";
            string roleCode = "EndUser"; //role end-user
            string userEmail = !String.IsNullOrEmpty((string)profiledata["thai_email"]) ? (string)profiledata["thai_email"] : "";
            string mobliePhone = !String.IsNullOrEmpty((string)profiledata["mobile"][0]["mobile_no"]) ? (string)profiledata["mobile"][0]["mobile_no"] : "";
            string empcode = userNameInDb;
            string userName = userNameInDb;

            //validateEmailAlreadyExists(sid, companyCode, userEmail);

            string dateStr = DateTime.Now.ToString("yyyyMMddHHmmss", new CultureInfo("en-US"));
            string fisyear = DateTime.Now.ToString("yyyy", new CultureInfo("en-US"));
            string empGroup = "EMP01";
            string sqlMasterEM = @"INSERT INTO master_employee
                               VALUES ('" + sid + "','" + companyCode + "','" + empcode + @"',
                                    '" + EmployeeHierarchy + "','" + NamePreFix + "','" + firstName + "','" + LastName + @"',
                                    '" + FirstName_TH + "','" + LastName_TH + "','" + Position + "','" + Branch + @"',
                                    '" + StartDate + "','" + ToDate + "','" + PersonAssignCode + "','" + PersonArea + @"',
                                    '" + CostCenter + "','" + EEGroupCode + "','" + EESubGroupCode + "','" + ChangeDate + @"',
                                    '" + ChangeDateDetail + "','" + CREATED_BY + "','" + UPDATED_BY + "','" + dateStr + "','" + dateStr + @"',
                                    '" + userName + "','" + CostCenterGroup + "','" + FloatCode + "','" + VendorCode + "','" + CustomerCode + @"',
                                    '" + empGroup + "','" + fisyear + "','','" + ActiveOn + "','" + IsSaleRepresent + "')";
            DBService dbService = new DBService();

            dbService.selectDataFocusone(sqlMasterEM);

            string currentDateTime = Validation.getCurrentServerStringDateTime();
            string _current_date = currentDateTime.Substring(0, 8);
            string _current_time = currentDateTime.Substring(8, 6);
            string sql_usr01 = @"INSERT INTO usr01
                                VALUES ('" + sid + "','" + userName + "','" + _current_date + "','" + _current_time + "','" + _current_date + "','" + _current_time + "','A','" + UserManagementService.SYSTEM_ERP + "','C','" + currentDateTime + "', '" + CREATED_BY + "', '', '')";
            dbService.selectDataFocusone(sql_usr01);

            string sqlAuth_user = @"INSERT INTO auth_user
                                VALUES ('" + sid + "','" + userName + "','" + userName + "','" + String.Concat(firstName, " ", LastName) + "','" + dateStr + "','" + CREATED_BY + "','" + dateStr + "','" + UPDATED_BY + "','','')";
            dbService.selectDataFocusone(sqlAuth_user);

            string sqlAuthProfileMapping = @"INSERT INTO auth_userprofilemapping
                                                VALUES('" + sid + "','" + userName + "','000001','" + CREATED_BY + "','" + dateStr + "', NULL, NULL)";
            dbService.selectDataFocusone(sqlAuthProfileMapping);

            string sqlRole_Mapping_Employee = @"INSERT INTO ERPW_Role_Maping_Employee
                                                   VALUES ('" + sid + "','" + companyCode + "','" + empcode + "','" + roleCode + "','" + CREATED_BY + "','" + dateStr + "','" + UPDATED_BY + "','" + dateStr + "','')";
            dbService.selectDataFocusone(sqlRole_Mapping_Employee);

            string sqlMaster_employee_address = @"INSERT INTO master_employee_address
                                                    VALUES ('" + sid + "','" + companyCode + "','" + empcode + "','','','','','','','','THA','','" + mobliePhone + "','" + CREATED_BY + "','" + UPDATED_BY + "','" + dateStr + "','" + dateStr + "','" + userEmail + "','','" + empGroup + "')";
            dbService.selectDataFocusone(sqlMaster_employee_address);
        }

        #endregion


    }
}