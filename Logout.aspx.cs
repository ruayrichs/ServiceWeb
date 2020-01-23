using agape.lib.constant;
using ERPW.Lib.Authentication;
using ERPW.Lib.F1WebService;
using ERPW.Lib.Master;
using ERPW.Lib.WebConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ERPWAuthentication.SID))
            {
                SaveLogLogin();
            }
            string sid = ERPWebConfig.GetSID();
            string sessionID = (string)Session[ApplicationSession.USER_SESSION_ID];
            F1WebService.getUserLoginService().removeUserSessionBeanByUserName(sid, ERPWAuthentication.UserName);
            F1WebService.getUserLoginService().removeUserSessionBean(sessionID);

            Session.Abandon();
            Response.Redirect(Page.ResolveUrl("~/login.aspx"));
        }

        [WebMethod]
        public static int LogoutCheck()
        {
            if (string.IsNullOrEmpty(ERPWAuthentication.EmployeeCode))
            {
                return 0;
            }
            return 1;
        }

        private void SaveLogLogin()
        {
            LogServiceLibrary log = new LogServiceLibrary();
            log.saveLogUserAccess(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ERPWAuthentication.EmployeeCode, ERPWAuthentication.UserName
                , true
                , "LOGOUT");
        }
    }
}