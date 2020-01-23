using agape.entity.UserProfile;
using agape.lib.constant;
using agape.lib.web.configuration.utils;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ServiceWeb.Service;
using SNA.Lib.Authentication;
using SNA.Lib.Security;
using SNA.Lib.Transaction;
using SNA.Lib.Transaction.UserLoginService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb
{
    public partial class LoginSpider : System.Web.UI.Page
    {
        //SNA.Lib.Transaction.LinkUtils.LinkUtils serviceLink = WSHelper.getLinkUtilsMother();
        //SNA.Lib.Transaction.UserLoginService.UserLoginService loginService = WSHelper.getUserLoginService();
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        private void AutoLoginSpider()
        {
            try
            {
                if (Request.QueryString.Count == 0)
                {
                    throw new Exception("no logged in");
                }
                string mode = Request.QueryString["mode"];
                string id = Request.QueryString["id"];//.Replace(' ', '+');
                string sc = Request.QueryString["sc"];//.Replace(' ', '+');
                string sid = Request.QueryString["sid"];
                string lang = Request.QueryString["lang"];
                if (lang == null || lang == "")
                { Session[ApplicationSession.USER_SESSION_LANG] = "en-US"; }
                else { Session[ApplicationSession.USER_SESSION_LANG] = lang; }
                //string userDecrypt = AGStringCipher.Decrypt(id, sc);

                string email = AGStringCipher.Decrypt(id, sc);

                SystemModeControlService.SystemModeEntities modeEn = SystemModeControlService.getInstanceMode(mode);

                new ERPWAutoLoginService(sid, email, modeEn);

                SystemModeControlService.LoginSystemModeWithRedirect(mode, "", sid, email, false);
                //isLogin = true;
                //SaveLogLogin(sid, ERPWAuthentication.CompanyCode, userName, "LOGIN", isLogin);
                Response.Redirect("/Default.aspx", true);
            }
            catch (System.Threading.ThreadAbortException tae)
            {
                // do nothing
            }
            catch (SoapException soe)
            {
                ClientService.AlertRedirect(ObjectUtil.Err(soe.StackTrace), "/login.aspx");
                //Response.Redirect("/login.aspx");

            }
            catch (Exception ex)
            {
                ClientService.AlertRedirect(ObjectUtil.Err(ex.StackTrace), "/login.aspx");
                //Response.Redirect("/login.aspx");
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        protected void btnLoginSpider_Click(object sender, EventArgs e)
        {
            //if (SystemModeControlService.getLoginMode() == SystemModeControlService.LOGIN_MODE_F1)
            //{
            //    processLoginF1();
            //}
            //else
            //{
                AutoLoginSpider();
            //}
        }

        private void processLoginF1()
        {
            //try
            //{
            //    if (Request.QueryString.Count == 0)
            //    {
            //        throw new Exception("no logged in");
            //    }
            //    string mode = Request.QueryString["mode"];
            //    string id = Request.QueryString["id"];//.Replace(' ', '+');
            //    string sc = Request.QueryString["sc"];//.Replace(' ', '+');
            //    string sid = Request.QueryString["sid"];
            //    string lang = Request.QueryString["lang"];
            //    if (lang == null || lang == "")
            //    { Session[ApplicationSession.USER_SESSION_LANG] = "en-US"; }
            //    else { Session[ApplicationSession.USER_SESSION_LANG] = lang; }
            //    //string userDecrypt = AGStringCipher.Decrypt(id, sc);

            //    string Username = AGStringCipher.Decrypt(id, sc);


            //    DataTable dt = getUsernameF1(sid, Username);
            //    if (dt.Rows.Count == 0)
            //    {
            //        throw new Exception("Invalid Username : " + Username);
            //    }

            //    DataRow[] drArr = dt.Select("");
            //    #region update  last login date
            //    LinkAutoLoginService.updateLastLogingDate(sid, Username);
            //    #endregion
            //    string sessionid = loginService.AddUserSession(sid, Username);
            //    addUserSession(sid, sessionid, Username);
            //    UserProfileBean profile = new UserProfileBean();
            //    profile.EmployeeCode = drArr.Length > 0 ? drArr[0]["EmployeeCode"].ToString() : dt.Rows[0]["EmployeeCode"].ToString();
            //    profile.FirstName = drArr.Length > 0 ? drArr[0]["FirstName"].ToString() : dt.Rows[0]["FirstName"].ToString();
            //    profile.LastName = drArr.Length > 0 ? drArr[0]["LastName"].ToString() : dt.Rows[0]["LastName"].ToString();
            //    profile.FirstName_TH = drArr.Length > 0 ? drArr[0]["FirstName_TH"].ToString() : dt.Rows[0]["FirstName_TH"].ToString();
            //    profile.LastName_TH = drArr.Length > 0 ? drArr[0]["LastName_TH"].ToString() : dt.Rows[0]["LastName_TH"].ToString();
            //    profile.Position = "";
            //    profile.Branch = "";
            //    profile.BranchName = "";
            //    profile.TelephoneNumber = "";
            //    profile.MobilePhone = drArr.Length > 0 ? drArr[0]["MobilePhone"].ToString() : dt.Rows[0]["MobilePhone"].ToString();
            //    profile.Email = drArr.Length > 0 ? drArr[0]["Email"].ToString() : dt.Rows[0]["Email"].ToString();
            //    profile.PositionName = "";
            //    profile.CompanyCode = drArr.Length > 0 ? drArr[0]["CompanyCode"].ToString() : dt.Rows[0]["CompanyCode"].ToString();
            //    profile.CompanyName = drArr.Length > 0 ? drArr[0]["CompanyName"].ToString() : dt.Rows[0]["CompanyName"].ToString();
            //    profile.BuareaCode = "";
            //    profile.LocCurrency = "";
            //    profile.PlantCode = "";
            //    profile.CostCenterCode = "";
            //    profile.CostCenterName = "";
            //    profile.FloatCode = "";
            //    profile.FloatName = "";
            //    profile.TelExt = "";
            //    profile.Department = "";
            //    profile.CountryCode = "";
            //    profile.CountryName = "";
            //    profile.LocCurrencyName = "";
            //    profile.FiscalYear = Validation.getFiscalYear(Validation.getCurrentServerDate());
            //    profile.StorageCode = "";
            //    SNAAuthentication.SetSNALinkAuthenticationForF1(dt, profile, "", "", "", "", "", Validation.getCurrentServerStringDateTime());

            //    Response.Redirect("/", true);
            //}
            //catch (System.Threading.ThreadAbortException tae)
            //{
            //    // do nothing
            //}
            //catch (SoapException soe)
            //{
            //    ClientService.AlertRedirect(ObjectUtil.Err(soe.StackTrace), "/login.aspx");
            //    //Response.Redirect("/login.aspx");

            //}
            //catch (Exception ex)
            //{
            //    ClientService.AlertRedirect(ObjectUtil.Err(ex.StackTrace), "/login.aspx");
            //    //Response.Redirect("/login.aspx");
            //}
            //finally
            //{
            //    ClientService.AGLoading(false);
            //}
        }

        //private void addUserSession(string sid, string sessionId, string username)
        //{
        //    if (string.IsNullOrEmpty(sessionId))
        //        throw new Exception("Invalid username/password");

        //    UserSessionBean sessionBean = loginService.getUserSessionBean(sessionId);
        //    Session[ApplicationSession.USER_SESSION_BEAN] = sessionBean;
        //    Session[ApplicationSession.USER_SESSION_ID] = sessionId;
        //    Session[ApplicationSession.USER_SESSION_SID] = sid;
        //    //Session[ApplicationSession.USER_SESSION_LANG] = "th-TH";
        //    Session[ApplicationSession.USER_SESSION_TYPE] = "";
        //    Session[ApplicationSession.USER_SESSION_LOGIN] = username;
        //    Session["username_login_date"] = Validation.getCurrentServerDate();
        //    Session["username_login_time"] = Validation.getCurrentServerTime();

        //}

        public static DataTable getUsernameF1(string sid, string username)
        {
            DBService db = new DBService();
            DataTable dt = null;

            string sql = @"	select a.sid,isnull(b.CompanyCode,'') as CompanyCode,isnull(d.Name,'') as CompanyName
                    ,a.username as LINKID,'' as LINKNAME,'' as LINKTYPE,isnull(b.EmployeeCode,'') as LINKOBJECT
                    ,a.username as LINKREFUSER,isnull(b.FirstName,'') as FirstName
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
            dt = db.selectDataFocusone(sql);

            return dt;
        }
    }
}