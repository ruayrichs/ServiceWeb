using agape.lib.constant;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ERPW.Lib.Authentication;
using ERPW.Lib.F1WebService;
using ERPW.Lib.Master;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace ServiceWeb.Service
{
    public class OneIDService
    {
        private DBService dbService = new DBService();
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
                                                   VALUES ('" + sid + "','" + companyCode + "','" + empcode + "','" + roleCode + "','" + CREATED_BY +"','" + dateStr + "','" + UPDATED_BY + "','" + dateStr + "','')";
            dbService.selectDataFocusone(sqlRole_Mapping_Employee);

            string sqlMaster_employee_address = @"INSERT INTO master_employee_address
                                                    VALUES ('" + sid + "','" + companyCode + "','" + empcode + "','','','','','','','','THA','','" + mobliePhone + "','" + CREATED_BY + "','" + UPDATED_BY + "','" + dateStr + "','" + dateStr + "','" + userEmail + "','','" + empGroup + "')";
            dbService.selectDataFocusone(sqlMaster_employee_address);
        }
        private void validateEmailAlreadyExists(string sid, string companyCode, string userEmail)
        {
            DataTable dtCheck = UserManagementService.getInstance().getUserByEmailORUserID(sid, companyCode, userEmail, "");
            List<string> listError = new List<string>();
            foreach (DataRow dr in dtCheck.Rows)
            {
                if (userEmail == Convert.ToString(dr["Email"]))
                {
                    listError.Add("มีข้อมูล " + Convert.ToString(dr["UserName"]) + " ใช้อีเมล์ " + userEmail + " อยู่ในระบบแล้ว !! <br> กรุณาใช้ ONEID ใหม่ !!");
                }
            }

            if (listError.Count > 0)
            {
                throw new Exception(String.Join("<br>", listError));
            }
        }
    }

    #region Data Model

    public class OneIdAPILoginReqModel 
    {
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string username { get; set; }
        public string password { get; set; }

    }

    #endregion

}