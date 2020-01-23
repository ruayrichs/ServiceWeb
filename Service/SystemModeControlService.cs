
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ServiceWeb.Service
{
    public class SystemModeControlService
    {
        public static bool ISIndividual
        {
            get
            {
                return ERPWAuthentication.SID == "AGP";
            }
        }

        public static string getCookiesLanguage()
        {
            HttpCookie lan_Cookie = HttpContext.Current.Request.Cookies["SystemControlService_language"];

            if (lan_Cookie == null)
            {
                return "";
            }
            else
            {
                return lan_Cookie.Value;
            }
        }

        public static void setCookiesLanguage(string lan)
        {
            HttpCookie lan_Cookie = new HttpCookie("SystemControlService_language");
            DateTime now = DateTime.Now;
            lan_Cookie.Value = lan;
            lan_Cookie.Expires = now.AddYears(10);
            HttpContext.Current.Response.Cookies.Add(lan_Cookie);
        }

        public static void LoginSystemMode(string SystemModeCode, string MotherSID, string SID, string Email)
        {
            LoginSystemModeExecute(SystemModeCode, MotherSID, SID, Email, false, false);
        }

        public static void LoginSystemMode(string SystemModeCode, string MotherSID, string SID, string Email, bool isIndividual)
        {
            LoginSystemModeExecute(SystemModeCode, MotherSID, SID, Email, true, isIndividual);
        }

        public static void LoginSystemModeWithRedirect(string SystemModeCode, string MotherSID, string SID, string Email, bool withRedirect)
        {
            LoginSystemModeExecute(SystemModeCode, MotherSID, SID, Email, withRedirect, false);
        }

        private static void LoginSystemModeExecute(string SystemModeCode, string MotherSID, string SID, string Email, bool withRedirect, bool isIndividual)
        {
            HttpCookie myCurrentMode = new HttpCookie("SystemControlService_CurrentMode");
            DateTime now = DateTime.Now;
            myCurrentMode.Value = SystemModeCode;
            myCurrentMode.Expires = now.AddYears(10);
            HttpContext.Current.Response.Cookies.Add(myCurrentMode);

            HttpCookie myEmail = new HttpCookie("SystemControlService_Email");
            myEmail.Value = Email;
            myEmail.Expires = now.AddYears(10);
            HttpContext.Current.Response.Cookies.Add(myEmail);

            HttpCookie mySID = new HttpCookie("SystemControlService_SID");
            mySID.Value = SID;
            mySID.Expires = now.AddYears(10);
            HttpContext.Current.Response.Cookies.Add(mySID);

            HttpCookie myMotherSID = new HttpCookie("SystemControlService_Mother_SID");
            myMotherSID.Value = MotherSID;
            myMotherSID.Expires = now.AddYears(10);
            HttpContext.Current.Response.Cookies.Add(myMotherSID);

            if (!withRedirect)
                return;

            HttpContext.Current.Response.Redirect("~/member/", true);
        }

        public static string GetMotherSID(string RequestSID)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme + Uri.SchemeDelimiter;
            string host = HttpContext.Current.Request.Url.Host;
            string port = (HttpContext.Current.Request.Url.Port == 80 || HttpContext.Current.Request.Url.Port == 443)
                ? "" : HttpContext.Current.Request.Url.Port.ToString();

            //================ Hard Code ===============
            string motherSID = "001";
            //string motherSID = LinkVerificationService.getInstance().callLinkLoginGetMotherSID(RequestSID, protocol, host, port);

            return motherSID;
        }

        public static SystemModeEntities GetCurrentMode
        {
            get
            {
                string CurrentMode = "";
                HttpCookie myCookie = new HttpCookie("SystemControlService_CurrentMode");
                myCookie.Domain = HttpContext.Current.Request.Url.Host;
                myCookie = HttpContext.Current.Request.Cookies["SystemControlService_CurrentMode"];

                if (myCookie != null)
                    CurrentMode = myCookie.Value;
                return getInstanceMode(CurrentMode);
            }
        }

        public static string GetCurrentLoggedInEmail
        {
            get
            {
                string Email = "";
                HttpCookie myCookie = new HttpCookie("SystemControlService_Email");
                myCookie.Domain = HttpContext.Current.Request.Url.Host;
                myCookie = HttpContext.Current.Request.Cookies["SystemControlService_Email"];

                if (myCookie != null)
                    Email = myCookie.Value;
                return Email;
            }
        }

        public static string GetCurrentLoggedInSID
        {
            get
            {
                string SID = "";
                HttpCookie myCookie = new HttpCookie("SystemControlService_SID");
                myCookie.Domain = HttpContext.Current.Request.Url.Host;
                myCookie = HttpContext.Current.Request.Cookies["SystemControlService_SID"];

                if (myCookie != null)
                    SID = myCookie.Value;
                return SID;
            }
        }

        public static string GetCurrentLoggedInMotherSID
        {
            get
            {
                string SID = "";
                HttpCookie myCookie = new HttpCookie("SystemControlService_Mother_SID");
                myCookie.Domain = HttpContext.Current.Request.Url.Host;
                myCookie = HttpContext.Current.Request.Cookies["SystemControlService_Mother_SID"];

                if (myCookie != null)
                    SID = myCookie.Value;
                return SID;
            }
        }

        public static SystemModeEntities ConstantEmptySystemMode
        {
            get
            {
                return new SystemModeEntities
                {
                    Mode = "",
                    DefaultPage = "~/Login.aspx",
                    LoginPage = "~/Login.aspx"
                };
            }
        }

        public static SystemModeEntities ConstantLinkSystemMode
        {
            get
            {
                string urlMother = ConfigurationManager.AppSettings["SNAWeb_Mother_login"];
                return new SystemModeEntities
                {
                    Mode = "Link",
                    //DefaultPage = "~/TimeAttendance/ActivityManagementReDesign.aspx",

                    DefaultPage = "~/Default.aspx",
                    LoginPage = urlMother + "SelectLogin.aspx"
                };
            }
        }

        public static SystemModeEntities ConstantIndividualSystemMode
        {
            get
            {
                string urlMother = ConfigurationManager.AppSettings["SNAWeb_Mother_login"];
                return new SystemModeEntities
                {
                    Mode = "AGP",
                    //DefaultPage = "~/TimeAttendance/ActivityManagement.aspx",

                    DefaultPage = "~/DefaultIndividual.aspx",
                    LoginPage = urlMother + "Login.aspx"
                };
            }
        }

        public static SystemModeEntities ConstantAgriculturistSystemMode
        {
            get
            {
                string urlMother = ConfigurationManager.AppSettings["SNAWeb_Mother_login"];
                return new SystemModeEntities
                {
                    Mode = "Agriculturist",
                    DefaultPage = "~/web/LinkDashboard.aspx",
                    LoginPage = urlMother + "Login.aspx"
                };
            }
        }

        public static SystemModeEntities ConstantCustomerSystemMode
        {
            get
            {
                string urlMother = ConfigurationManager.AppSettings["SNAWeb_Mother_login"];
                return new SystemModeEntities
                {
                    Mode = "Customer",
                    DefaultPage = "~/Customer/ServiceCall.aspx",
                    LoginPage = urlMother + "SelectLogin.aspx"
                };
            }
        }

        public static SystemModeEntities ConstantSupplierSystemMode
        {
            get
            {
                string urlMother = ConfigurationManager.AppSettings["SNAWeb_Mother_login"];
                return new SystemModeEntities
                {
                    Mode = "Supplier",
                    DefaultPage = "~/SupplierMode/POActivityManagement.aspx",
                    LoginPage = urlMother + "SelectLogin.aspx"
                };
            }
        }

        public static SystemModeEntities getInstanceMode(string SystemModeCode)
        {

            SystemModeEntities Mode = new SystemModeEntities();

            if (SystemModeCode.Equals(ConstantLinkSystemMode.Mode))
                Mode = ConstantLinkSystemMode;

            else if (SystemModeCode.Equals(ConstantAgriculturistSystemMode.Mode))
                Mode = ConstantAgriculturistSystemMode;

            else if (SystemModeCode.Equals(ConstantCustomerSystemMode.Mode))
                Mode = ConstantCustomerSystemMode;

            else if (SystemModeCode.Equals(ConstantSupplierSystemMode.Mode))
                Mode = ConstantSupplierSystemMode;
            else if (SystemModeCode.Equals(ConstantIndividualSystemMode.Mode))
                Mode = ConstantIndividualSystemMode;
            else
                Mode = ConstantEmptySystemMode;

            return Mode;
        }

        public static bool isPrivateWeb()
        {
            bool isPrivate = false;
            string strPrivate = ConfigurationManager.AppSettings["isprivate"];
            if (strPrivate != null)
            {
                isPrivate = ConfigurationManager.AppSettings["isprivate"].ToLower() == "true" ? true : false;
            }
            return isPrivate;
        }

        [Serializable]
        public class SystemModeEntities
        {
            public string Mode { get; set; }
            public string DefaultPage { get; set; }
            public string LoginPage { get; set; }
        }

        public static string getHost()
        {
            //string host = HttpContext.Current.Request.Url.Host;
            string host = "crm001.p21academic.com";
            //string host = "202.170.123.222";
            return host;
        }

        public static string getPort()
        {
            //string port = HttpContext.Current.Request.Url.Port == 80 ? "" : HttpContext.Current.Request.Url.Port.ToString();
            string port = "";
            //string port = "7306";
            return port;
        }
    }
}