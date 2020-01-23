using agape.lib.web.configuration.utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Authentication;
using ServiceWeb.Service;
using ERPW.Lib.F1WebService;
using ERPW.Lib.WebConfig;
using ERPW.Lib.F1WebService.UserLoginService;
using agape.lib.constant;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using System.Data;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Entity;
using ServiceWeb.crm.AfterSale;
using ERPW.Lib.Service;
using Agape.Lib.Web.Bean.CS;
using System.Web.Configuration;
using agape.entity.UserProfile;
using System.Globalization;

namespace ServiceWeb
{
    public partial class Login : System.Web.UI.Page
    {
        public const String ERROR_CODE_NEED_INVITE = "NEED_INVITE";

        private SNA.Lib.Transaction.UserLoginService.UserLoginService loginService = SNA.Lib.Transaction.WSHelper.getUserLoginService();
        private DBService dbService = new DBService();

        public bool IsFilterOwner
        {
            get
            {
                bool _IsFilterOwner = false;
                bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out _IsFilterOwner);
                return _IsFilterOwner;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkmodelogin.SelectedValue == "1")
                {
                    _Login();
                }
                else if (checkmodelogin.SelectedValue == "2")
                {
                    LoginByActiveDirectory();
                    
                }
                
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {

        }
        protected void LoginByActiveDirectory()
        {
            string sid = "";
            string company = "";
            string userName = txtEmail.Text;

            try
            {
                sid = ERPWebConfig.GetSID();
                company = ERPWebConfig.GetCompany();
                bool AuthenByActiveDirectory = false;
                
                
                AuthenByActiveDirectory = ActiveDirectoryService.authenActiveDirectory(userName, txtPassword.Text,sid,company);
                if (AuthenByActiveDirectory)
                {
                    SaveLogLogin(sid, ERPWAuthentication.CompanyCode, userName, "LOGIN", AuthenByActiveDirectory);
                    if (!string.IsNullOrEmpty(Request["q"]))
                    {
                        switch (Request["q"])
                        {
                            case "ticket":
                                {
                                    GetDataToedit(sid, ERPWAuthentication.CompanyCode, Request["key"]);
                                    break;
                                }
                            default:
                                {
                                    //ERPWAuthentication.Permission.DefaultPage                        
                                    Response.Redirect(Page.ResolveUrl("~/Default.aspx"), true);
                                    break;
                                }
                        }
                    }
                    else
                    {
                        //ERPWAuthentication.Permission.DefaultPage
                        Response.Redirect(Page.ResolveUrl("~" + ERPWAuthentication.Permission.DefaultPage), true);
                    }
                }
                else
                {
                    ClientService.AGMessage("ไม่สามารถ เข้าสู่ระบบได้");
                }

            }
            catch (Exception ex)
            {
                string errMessage = getErrorMessage(ex);
                ClientService.AGError(ObjectUtil.Err(errMessage));
                
            }
            
        }
       
        private string GetUserName(string sid, string companyCode)
        {
            string user = txtEmail.Text.Trim();

            string sql = "SELECT UserName FROM master_employee WHERE SID = '" + sid + "' AND CompanyCode = '" + companyCode + "' AND UserName = '" + user + "'";

            DataTable dt = dbService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                return user;
            }
            else
            {
                sql = @"SELECT a.UserName, b.Email
                        FROM master_employee a
                        LEFT OUTER JOIN master_employee_address b ON a.SID = b.SID
	                        AND a.CompanyCode = b.CompanyCode
	                        AND a.EmployeeCode = b.EmployeeCode
                        WHERE a.SID = '" + sid + @"'
    	                    AND a.CompanyCode = '" + companyCode + @"'
	                        AND b.Email = '" + user + "'";

                dt = dbService.selectDataFocusone(sql);

                if (dt.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(dt.Rows[0]["UserName"].ToString()))
                    {
                        throw new Exception("Username " + user + " not found in system.");
                    }

                    return dt.Rows[0]["UserName"].ToString().Trim();
                }
                else
                {
                    throw new Exception("Username " + user + " not found in system.");
                }
            }            
        }

        protected void _Login()
        {
            string sid = "";
            string company = "";
            string username = "";
            string password = "";
            bool isLogin = false;
            try
            {
                if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
                {
                    throw new Exception("กรุณาระบุ Username");
                }

                if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    throw new Exception("กรุณาระบุ Password");
                }

                sid = ERPWebConfig.GetSID();
                company = ERPWebConfig.GetCompany();
                username = GetUserName(sid, company);
                password = txtPassword.Text.Trim();

                //1. Validate User Login                

                String callResult = "";

                try
                {
                    //string sessionID = F1WebService.getSessionId(sid, username);
                    //UserSessionBean dataBeen = F1WebService.getUserLoginService().getUserSessionBean(sessionID);
                    //F1WebService.getUserLoginService().removeUserSessionBean(sessionID);


                    callResult = F1WebService.getUserLoginService().Login(sid, username, password);

                }
                catch (Exception ex)
                {
                    //VisibleCaptcha(true);
                    throw ex;
                }
                if (!string.IsNullOrEmpty(callResult) && callResult.Split('#')[0].Equals("S"))
                {
                    //2. Validate Password Expired
                    //bool expired = false; //F1WebService.getUserLoginService().CheckPasswordExpired(sid, username, password);
                    bool expired = loginService.CheckPasswordExpired(sid, username, password);
                   
                    //if (!expired)
                    //{
                    //    #region update  last login date
                    //    //LinkAutoLoginService.updateLastLogingDate(sid, Username);
                    //    #endregion
                    //    //SaveLogLogin(sid, company, username);
                    //    ProcessLogin(sid, username, ref isLogin);
                    //}

                    if (!expired)
                    {
                        ProcessLogin(sid, username, ref isLogin);
                    }
                    else
                    {
                        ClientService.DoJavascript("$('#modal-change-password').modal('show');");
                    }
                }
                else if (!string.IsNullOrEmpty(callResult) && callResult.Split('#')[0].Equals("E"))
                {
                    throw new Exception(callResult);
                }

                else
                {
                    throw new Exception("Invalid Username / Password");
                }

                
            }
            catch (System.Threading.ThreadAbortException tae)
            {
                // do nothing
            }
            catch (SoapException soe)
            {
                String errMessage = getErrorMessage(soe);
                ClientService.AGError(ObjectUtil.Err(errMessage));
            }
            catch (Exception ex)
            {
                String errMessage = getErrorMessage(ex);
                ClientService.AGError(ObjectUtil.Err(errMessage));
            }
            finally
            {
                if (!isLogin)
                {
                    SaveLogLogin(sid, company, username, "LOGIN", isLogin);
                }
                ClientService.AGLoading(false);
            }

        }

        private void ProcessLogin(String sid, String userName,ref bool isLogin)
        {
            string mode = ConfigurationHelper.getValue("system.mode");

            SystemModeControlService.SystemModeEntities modeEn = SystemModeControlService.getInstanceMode(mode);

            new ERPWAutoLoginService(sid, userName, modeEn);         

            SystemModeControlService.LoginSystemModeWithRedirect(mode, "", sid, userName, false);
            isLogin = true;
            SaveLogLogin(sid, ERPWAuthentication.CompanyCode, userName, "LOGIN", isLogin);

            if (!string.IsNullOrEmpty(Request["q"]))
            {
                switch (Request["q"])
                {
                    case "ticket": 
                    {
                        GetDataToedit(sid, ERPWAuthentication.CompanyCode, Request["key"]);
                        break;
                    }
                    default:
                    {
                            //ERPWAuthentication.Permission.DefaultPage                        
                        Response.Redirect(Page.ResolveUrl("~/Default.aspx"), true);
                        break;
                    }
                }
            }
            else
            {
                //ERPWAuthentication.Permission.DefaultPage
                Response.Redirect(Page.ResolveUrl("~"+ ERPWAuthentication.Permission.DefaultPage), true);
            }
        }

        protected void GetDataToedit(string sid, string companyCode, string docnumber)
        {
            DataTable dt = AfterSaleService.getInstance().getDataServiceTicketHeader(sid, companyCode, docnumber);
            if (dt.Rows.Count > 0)
            {
                string doctype = Convert.ToString(dt.Rows[0]["Doctype"]);
                string fiscalyear = Convert.ToString(dt.Rows[0]["Fiscalyear"]);
                string customer = Convert.ToString(dt.Rows[0]["CustomerCode"]);

                ServiceCallFastEntryCriteria link = new ServiceCallFastEntryCriteria();
                string idGen = link.redirectViewToTicketDetail(customer, doctype, docnumber, fiscalyear);
                if (!String.IsNullOrEmpty(idGen))
                {
                    ServiceTicketLibrary libServiceTicket = new ServiceTicketLibrary();
                    string PageRedirect = libServiceTicket.getPageTicketRedirect(
                        ERPWAuthentication.SID,
                        (Session["ServicecallEntity" + idGen] as tmpServiceCallDataSet).cs_servicecall_header.Rows[0]["Doctype"].ToString()
                    );
                    //Response.-Redirect(Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen));
                    ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen) + "');");
                }
            }
            else
            {
                Response.Redirect(Page.ResolveUrl("~/Default.aspx"), true);
            }
        }

        private String getErrorMessage(Exception ex)
        {
            String errMessage = ex.Message;
            if (errMessage.IndexOf("--->") > -1)
            {

                if (errMessage.IndexOf("at Link") > -1)
                {
                    int messageLength = errMessage.IndexOf("at Link") - 7 - errMessage.IndexOf("--->") + 4;
                    errMessage = errMessage.Substring(errMessage.IndexOf("--->") + 4, messageLength);
                }
                else
                {
                    errMessage = errMessage.Substring(errMessage.IndexOf("--->") + 4);
                }


            }

            return errMessage;
        }

        private void SaveLogLogin(string sid, string companyCode, string username,string ACCESS_MODE, bool ACCESS_STATUS)
        {
            string employeecode = ERPWAuthentication.EmployeeCode;
            new LogServiceLibrary().saveLogUserAccess(sid, companyCode, employeecode, username, ACCESS_STATUS, ACCESS_MODE);
            #region Save Log Login
            //List<Main_LogService> listEn = new List<Main_LogService>();
            //List<Detail_LogService> listDetail = new List<Detail_LogService>();
            //string PROGRAM_ID = "";
            //listDetail.Add(new Detail_LogService()
            //{
            //    FieldName = "",
            //    ItemNumber = "",
            //    NewValue = "",
            //    OldValue = "",
            //});
            //listEn.Add(new Main_LogService
            //{
            //    LOGOBJCODE = PROGRAM_ID,
            //    PROGOBJECT = "",
            //    ACCESSCODE = LogServiceLibrary.AccessCode_Create,
            //    OBJPKREC = sid + company + username,
            //    APPLTYPE = "T",
            //    Access_By = username,
            //    Access_Date = Validation.getCurrentServerStringDateTime().Substring(0, 8),
            //    Access_Time = Validation.getCurrentServerStringDateTime().Substring(8, 6),
            //    listDetail = listDetail
            //});
            //new LogServiceLibrary().SaveLog(sid, PROGRAM_ID, "T", listEn);
            #endregion
        }


        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            bool isLogin = false;
            string sid = ERPWebConfig.GetSID();
            string company = ERPWebConfig.GetCompany();
            string userName = txtEmail.Text.Trim();
            string newPassword = tbNewPassword.Text.Trim();
            string confirmPassword = tbConfirmPassword.Text.Trim();

            try
            {
                ValidatePassword(newPassword, confirmPassword);

                if (newPassword == "")
                {
                    throw new Exception("กรุณาระบุรหัสผ่านใหม่");
                }

                if (confirmPassword == "")
                {
                    throw new Exception("กรุณาระบุยืนยันรหัสผ่าน");
                }

                if (newPassword != confirmPassword)
                {
                    throw new Exception("ยืนยันรหัสผ่านไม่ตรงกับรหัสผ่านใหม่");
                }


                string callResult = ERPW.Lib.F1WebService.F1WebService.getChangePwdService().ChangePassword(sid, userName, confirmPassword);

                if (!string.IsNullOrEmpty(callResult))
                {
                    if (!string.IsNullOrEmpty(callResult) && callResult.Split('#')[0].Equals("S"))
                    {
                        DataTable dt = getUsernameF1(sid, userName);

                        ProcessLogin(sid, userName, ref isLogin);

                        Response.Redirect("/Default.aspx", true);
                    }
                    else if (!string.IsNullOrEmpty(callResult) && callResult.Split('#')[0].Equals("E"))
                    {
                        throw new Exception(callResult);
                    }
                    else
                    {
                        throw new Exception("ไม่สามารถเปลี่ยนรหัสผ่านได้");
                    }
                }
                else
                {
                    throw new Exception("ไม่สามารถเปลี่ยนรหัสผ่านได้");
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
            finally
            {
                ClientService.AGLoading(false);
                if (!isLogin)
                {
                    SaveLogLogin(sid, company, userName, "LOGIN", isLogin);
                }
                ClientService.AGLoading(false);
            }
        }


        private void ValidatePassword(string newPassword, string confirmPassword)
        {
            ///SNAAuthentication.CheckPassword(SNAAuthentication.Email, txtOldPassword.Value, SNAAuthentication.CompanyCode);
            if (newPassword.CompareTo(confirmPassword) != 0)
            {
                throw new Exception("รหัสผ่านใหม่ไม่ ตรงกับ ยืนยันรหัสผ่าน");
            }
            if (confirmPassword == "")
            {
                throw new Exception("กรุณากรอกรหัสผ่านใหม่");
            }
            if (newPassword == "")
            {
                throw new Exception("กรุณากรอก ยืนยันรหัสผ่าน");
            }
            ////// Check Password
            List<string> listMes = new List<string>();

            if (newPassword.Length < 8)
            {
                listMes.Add("กรุณาระบุรหัสผ่านไม่น้อยกว่า 8 ตัวอักษร!!");
            }

            string LowerCase = "abcdefghijklmnopqrstuvwxyz";
            string UperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string NumCase = "1234567890";
            string SpecialCase = "!@#$%^&*()_+|~-=\\`{}[]:\";'<>?,./";

            bool IsLowerCase = false;
            bool IsUperCase = false;
            bool IsNumCase = false;
            bool IsSpecialCase = false;
            for (int i = 0; i < newPassword.Length; i++)
            {
                string pass = newPassword[i].ToString();

                if (LowerCase.IndexOf(pass) >= 0)
                {
                    IsLowerCase = true;
                }
                if (UperCase.IndexOf(pass) >= 0)
                {
                    IsUperCase = true;
                }
                if (NumCase.IndexOf(pass) >= 0)
                {
                    IsNumCase = true;
                }
                if (SpecialCase.IndexOf(pass) >= 0)
                {
                    IsSpecialCase = true;
                }
            }

            if (!IsLowerCase)
            {
                listMes.Add("รหัสผ่านต้องเป็นตัวอักษรพิมเล็กอย่างน้อย 1 ตัว");
            }
            if (!IsUperCase)
            {
                listMes.Add("รหัสผ่านต้องเป็นตัวอักษรพิมใหญ่อย่างน้อย 1 ตัว");
            }
            if (!IsNumCase)
            {
                listMes.Add("รหัสผ่านต้องเป็นตัวเลขอย่างน้อย 1 ตัว");
            }
            //if (isPasswordhavespecialChar(txtPassword.Text.Trim()))
            if (!IsSpecialCase)
            {
                listMes.Add("รหัสผ่านต้องอักขระพิเศษอย่างน้อย 1 ตัว");
            }

            if (listMes.Count > 0)
            {
                throw new Exception(String.Join("<br>", listMes));
            }
            /////////// Check Password
        }
        private DataTable getUsernameF1(string sid, string username)
        {
            //DBService db = new DBService();
            DataTable dt = null;

            string sql = @"	select a.sid,isnull(b.CompanyCode,'') as CompanyCode,isnull(d.Name,'') as CompanyName
                    ,a.userid as LINKID,'' as LINKNAME,'' as LINKTYPE,isnull(b.EmployeeCode,'') as LINKOBJECT
                    ,a.userid as LINKREFUSER,isnull(b.FirstName,'') as FirstName
                    ,isnull(b.LastName,'') as LastName,isnull(b.EmployeeCode,'') as EmployeeCode,isnull(c.Email,'') as Email
                    ,isnull(b.FirstName_TH,'') as FirstName_TH,isnull(b.LastName_TH,'') as LastName_TH,isnull(c.MobilePhone,'') as MobilePhone
                    ,'' as LinkTag
                     from auth_user a
                    left outer join master_employee b
                    on a.sid = b.sid and a.userid = b.UserName
                    left outer join master_employee_address c
                    on b.sid = c.sid and b.CompanyCode = c.CompanyCode
                    and b.EmployeeCode = c.EmployeeCode
                    left outer join master_company d
                    on b.sid = d.sid and b.CompanyCode = d.ID
                    where a.sid='" + sid + "' and a.UserID='" + username + "'";
            dt = dbService.selectDataFocusone(sql);

            return dt;
        }
    }
}