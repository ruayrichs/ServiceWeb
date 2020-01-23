using agape.lib.constant;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ERPW.Lib.Authentication;
using ERPW.Lib.F1WebService;
using System;
using System.Data;
using System.DirectoryServices;
using System.Globalization;
using System.Web;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using System.Collections.Generic;

namespace ServiceWeb.Service
{
    public class ActiveDirectoryService
    {

        public static bool connectStatus(string server,string port,string baseDN, string username, string password)
        {
            bool status = false;
            string myADSPath = "LDAP://" + server + ':'+ port +'/' + baseDN;
            DirectoryEntry directoryEntry = new DirectoryEntry(myADSPath);
            directoryEntry.Username = username;
            directoryEntry.Password = password;
            
            // Validate with Guid
            try
            {
               var tmp = directoryEntry.Guid;
               status = true;

            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
            
            return status;

        }
        public static bool authenActiveDirectory(string userName, string passWord, string sid, string companyCode)
        {
            bool isLogin = false;
            
            //====================== get data from AD Config DATABASE =============================
            MasterActiveDirectoryConfigLib Obj = new MasterActiveDirectoryConfigLib();
            List<ActiveDirectoryConfig> ListADConfigEn = Obj.GetAD_Host_Config(
               sid,
               companyCode,
               MasterActiveDirectoryConfigLib.HostEvent_AUTH
            );
            if (ListADConfigEn.Count > 0)
            {
                ActiveDirectoryConfig AD_En = ListADConfigEn[0];
                
                

                string strCommu = "LDAP://"+AD_En.ADIPAddress.ToString()+':'+AD_En.ADPort;
                string userStr = (AD_En.ADDomain.ToString() + "\\" + userName);
                DirectoryEntry entry = new DirectoryEntry(strCommu, userStr, passWord);
                DirectorySearcher search = new DirectorySearcher(entry);
                //search.Filter = ("(SAMAccountName="+ userName + ")");
                search.Filter = "(&(objectClass=user)(cn=" + userName + "))";
                SearchResult result = search.FindOne();

                if (result != null)
                {
                    if (!serchUser(userName))
                    {
                        migrateDB(result, userName, sid, companyCode);
                    }
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
            }
                
            return isLogin;
        }
        public static string GetProperty(SearchResult searchResult, string PropertyName)
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
        protected static bool serchUser(string userName)
        {
            DBService dbService = new DBService();
            string sql = @"SELECT EmployeeCode from master_employee WHERE EmployeeCode = '" + userName + "'";
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
        //================================ migration User infomation from AD Server to  SQL ========================================
        protected static void migrateDB(SearchResult result, string userName, string sid, string companyCode)
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
            string CREATED_BY = "";
            string UPDATED_BY = "";
            string CostCenterGroup = "";
            string FloatCode = "";
            string VendorCode = "";
            string CustomerCode = "";
            string ActiveOn = "False";
            string IsSaleRepresent = "False";
            string roleCode = "EndUser"; //role end-user

            string userEmail = GetProperty(result, "userprincipalname");



            string firstName = GetProperty(result, "givenName");
            string dateStr = DateTime.Now.ToString("yyyyMMddHHmmss", new CultureInfo("en-US"));
            string fisyear = DateTime.Now.ToString("yyyy", new CultureInfo("en-US"));
            string empGroup = "EMP01";
            string sqlMasterEM = @"INSERT INTO master_employee
                               VALUES ('" + sid + "','" + companyCode + "','" + userName + @"',
                                    '" + EmployeeHierarchy + "','" + NamePreFix + "','" + firstName + "','" + LastName + @"',
                                    '" + FirstName_TH + "','" + LastName_TH + "','" + Position + "','" + Branch + @"',
                                    '" + StartDate + "','" + ToDate + "','" + PersonAssignCode + "','" + PersonArea + @"',
                                    '" + CostCenter + "','" + EEGroupCode + "','" + EESubGroupCode + "','" + ChangeDate + @"',
                                    '" + ChangeDateDetail + "','" + CREATED_BY + "','" + UPDATED_BY + "','" + dateStr + "','" + dateStr + @"',
                                    '" + userName + "','" + CostCenterGroup + "','" + FloatCode + "','" + VendorCode + "','" + CustomerCode + @"',
                                    '" + empGroup + "','" + fisyear + "','','" + ActiveOn + "','" + IsSaleRepresent + "')";
            DBService dbService = new DBService();

            dbService.selectDataFocusone(sqlMasterEM);

            string sqlAuth_user = @"INSERT INTO auth_user
                                VALUES ('" + sid + "','" + userName + "','" + userName + "','" + firstName + "','" + dateStr + "','AD_USER','" + dateStr + "','UPDATE_BY_AD_USER','','')";
            dbService.selectDataFocusone(sqlAuth_user);

            string sqlAuthProfileMapping = @"INSERT INTO auth_userprofilemapping
                                                VALUES('" + sid + "','" + userName + "','000001','AD_USER','" + dateStr + "','AD_USER','AD_USER')";
            dbService.selectDataFocusone(sqlAuthProfileMapping);

            string sqlRole_Mapping_Employee = @"INSERT INTO ERPW_Role_Maping_Employee
                                                   VALUES ('" + sid + "','" + companyCode + "','" + userName + "','" + roleCode + "','CREATE_AD_USER','" + dateStr + "','UPDATE_BY_AD_USER','" + dateStr + "','')";
            dbService.selectDataFocusone(sqlRole_Mapping_Employee);

            string sqlMaster_employee_address = @"INSERT INTO master_employee_address
                                                    VALUES ('" + sid + "','" + companyCode + "','" + userName + "','','','','','','','','THA','','','CREATE_BY_AD_USER','UPDATE_BY_AD_USER','" + dateStr + "','" + dateStr + "','" + userEmail + "','','" + empGroup + "')";
            dbService.selectDataFocusone(sqlMaster_employee_address);
        }

    }
}