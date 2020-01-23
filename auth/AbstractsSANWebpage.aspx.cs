using agape.lib.authorization.enumer;
using agape.lib.authorization.service;
using agape.lib.constant;
using agape.lib.web.configuration.utils;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using Newtonsoft.Json;
using ServiceWeb.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.F1WebService.UserLoginService;
using ERPW.Lib.Authentication.Entity;
using System.Web.Configuration;

namespace ServiceWeb.auth
{
    public partial class AbstractsSANWebpage : System.Web.UI.Page
    {
        #region ERPW Authentication
        public string _SID;
        public string SID
        {
            get
            {
                if (string.IsNullOrEmpty(_SID))
                {
                    _SID = ERPWAuthentication.SID;
                }
                return _SID;
            }
        }

        public string _CompanyCode;
        public string CompanyCode
        {
            get
            {
                if (string.IsNullOrEmpty(_CompanyCode))
                {
                    _CompanyCode = ERPWAuthentication.CompanyCode;
                }
                return _CompanyCode;
            }
        }

        public string _UserName;
        public string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_UserName))
                {
                    _UserName = ERPWAuthentication.UserName;
                }
                return _UserName;
            }
        }

        public string _EmployeeCode;
        public string EmployeeCode
        {
            get
            {
                if (string.IsNullOrEmpty(_EmployeeCode))
                {
                    _EmployeeCode = ERPWAuthentication.EmployeeCode;
                }
                return _EmployeeCode;
            }
        }

        public string _FullNameEN;
        public string FullNameEN
        {
            get
            {
                if (string.IsNullOrEmpty(_FullNameEN))
                {
                    _FullNameEN = ERPWAuthentication.FullNameEN;
                }
                return _FullNameEN;
            }
        }

        public string _FullNameTH;
        public string FullNameTH
        {
            get
            {
                if (string.IsNullOrEmpty(_FullNameTH))
                {
                    _FullNameTH = ERPWAuthentication.FullNameTH;
                }
                return _FullNameTH;
            }
        }

        public string _CompanyName;
        public string CompanyName
        {
            get
            {
                if (string.IsNullOrEmpty(_CompanyName))
                {
                    _CompanyName = ERPWAuthentication.CompanyName;
                }
                return _CompanyName;
            }
        }

        public AuthenticationPermission _Permission;
        public AuthenticationPermission Permission
        {
            get
            {
                if (_Permission == null)
                {
                    _Permission = ERPWAuthentication.Permission;
                }
                return _Permission;
            }
        }
        #endregion


        public bool IsAllFeature
        {
            get
            {
                bool _IsAllFeature = false;
                bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Allow_All_Feature"], out _IsAllFeature);
                return _IsAllFeature;
            }
        }

        
        //public bool IsFilterOwner
        //{
        //    get
        //    {
        //        bool _IsFilterOwner = false;
        //        bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out _IsFilterOwner);
        //        if (Permission.AllPermission)
        //        {
        //            _IsFilterOwner = false;
        //        }
        //        return _IsFilterOwner;
        //    }
        //}

        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        private UserLoginService loginService = ERPW.Lib.F1WebService.F1WebService.getUserLoginService();
        //private AuthorizationService AuthService = new AuthorizationService();

        public string AUTH_MODIFY = "AUTH_MODIFY";

        public string AUTH_CREATE = "AUTH_CREATE";
        public  string AUTH_UPDATE = "AUTH_UPDATE";
        public  string AUTH_DELETE = "AUTH_DELETE";
        public  string AUTH_COPY = "AUTH_COPY";
        public  string AUTH_PRINT = "AUTH_PRINT";
        public  string AUTH_CANCEL = "AUTH_CANCEL";
        public  string AUTH_REVERSE = "AUTH_REVERSE";
        public  string AUTH_INPUT_IMG = "AUTH_INPUT_IMG";


        #region getMenu new Structure

        protected virtual String getURL()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        protected virtual String getProgramID()
        {
            return null;
        }

        protected virtual Boolean ProgramPermission_CanView()
        {
            return true;
        }
        protected virtual Boolean ProgramPermission_CanModify()
        {
            return true;
        }

        private MenuService menu;

        public MenuService menuObj
        {
            get
            {
                
                if (menu != null)
                {
                    return menu;
                }
                menu = MenuService.getInStance().getAccessPageByProgramID(ERPWAuthentication.SID, ERPWAuthentication.UserName, getProgramID());
                return menu;
            }
        }


        #endregion

        public AbstractsSANWebpage()
        {
            this.Load += new EventHandler(this.Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {            
            ERPWAutoLoginService.CheckSessionAndAutoLogin();

            if (string.IsNullOrEmpty(ERPWAuthentication.SID))
            {
                Response.Redirect(Page.ResolveUrl("~/Login.aspx"));
                return;
            }

            if (!IsPostBack)
            {
                //menu = null;
                checkAuthorization();
            }
            else 
            {
              reMoveButtonAuthorization();
            }
        }
        //protected void Page_Error(object sender, EventArgs e)
        //{
        //    Exception objErr = Server.GetLastError();
        //    LinkWebErrorLog.SaveErrorLog(objErr);
        //}
        public void AgroLoading(bool isShow)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "cagld", "agroLoading(" + isShow.ToString().ToLower() + ");", true);
        }
        public void AgroDownloadFile(string fileName, string fileStream)
        {
            Session["AGRO_DOWNLOAD_FILENAME"] = fileName;
            Session["AGRO_DOWNLOAD_FILESTREAM"] = fileStream;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "agdlf", "agroDownloadFile();", true);
        }
        protected void InitializeCulture()
        {
            if (Session[ApplicationSession.USER_SESSION_LANG] != null)
            {
                String cultureName = Convert.ToString(Session[ApplicationSession.USER_SESSION_LANG]);
                if (String.IsNullOrEmpty(cultureName))
                {
                    return;
                }
                Page.UICulture = cultureName;
                //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(cultureName);
                //Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);
            }
            //base.InitializeCulture();
            //Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        }

        #region Authorization
        private void checkAuthorization()
        {
            if (!ProgramPermission_CanView())
            {
               
                if (IsCallback)
                {
                    Response.RedirectLocation = Page.ResolveUrl("~/auth/NoAuthorizePage.aspx");
                }
                else
                {
                    HttpCookie cookName = new HttpCookie("Cookie_AUTH_URL");
                    cookName.Value = getURL();
                    Response.Cookies.Add(cookName);
                    Response.Redirect(Page.ResolveUrl("~/auth/NoAuthorizePage.aspx"), true);
                }
            }
            else 
            {
                reMoveButtonAuthorization();
            }
        }
        private Boolean hasAuthorizePage()
        {
            if (ERPWAuthentication.SID == "AGP")
            {
                return true;
            }
            else
            {
                if (menuObj.CanDisplay)
                {
                    return true;
                }

                //UserLoginService.UserSessionBean userBean = (UserLoginService.UserSessionBean)Session[ApplicationSession.USER_SESSION_BEAN];
                //if (userBean == null)
                //    return false;
                if (ERPWAuthentication.EmployeeCode == null || ERPWAuthentication.EmployeeCode == "")
                    return false;

                return false;

               
            }
        }
        
        private void reMoveButtonAuthorization()
        {
            if (!ProgramPermission_CanModify())
            {
                ClientService.DoJavascript(@"setTimeout(function () { 
                    $('." + AUTH_MODIFY + @"').attr('type','button').removeAttr('ondragend').removeAttr('ondragstart').removeAttr('data-toggle').removeAttr('href').removeAttr('onclick').unbind('click').click(function () {
                        AGMessage('ไม่สามารถใช้งานเมนูดังกล่าวได้ กรุณาติดต่อเจ้าหน้าที่เพื่อเปิดใช้สิทธิ์ในหน้าจอนี้ !!');
                        });
                    $('." + AUTH_MODIFY + @"').prop('disabled', true)
                    $('." + AUTH_INPUT_IMG + @"').attr('type','image');
                }, 10);");
            }

            return;
            #region 
            List<string> listRemove = new List<string>();

            if (getURL() == "/Sales/DeliveryOrderCriteria.aspx" && menuObj.CanCreate)
            {
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["CanCreateDO"] != null)
                {
                    bool res = true;
                    bool.TryParse(System.Web.Configuration.WebConfigurationManager.AppSettings["CanCreateDO"], out res);

                    menuObj.CanCreate = res;
                }
            }

            if (!menuObj.CanCreate)
            {
                listRemove.Add("." + AUTH_CREATE);
            }

            if (!menuObj.CanChange)
            {
                listRemove.Add("." + AUTH_UPDATE);
            }

            if (!menuObj.CanDelete)
            {
                listRemove.Add("." + AUTH_DELETE);
            }
            if (!menuObj.CanCopy)
            {
                listRemove.Add("." + AUTH_COPY);
            }

            if (!menuObj.CanPrint)
            {
                listRemove.Add("." + AUTH_PRINT);
            }

            if (!menuObj.CanCancel)
            {
                listRemove.Add("." + AUTH_CANCEL);
            }

            if (!menuObj.CanReverse)
            {
                listRemove.Add("." + AUTH_REVERSE);
            }

            if (listRemove.Count > 0)
            {
                //ClientService.DoJavascript("$('" + String.Join(" , ", listRemove.ToArray()) + "').remove();");
                ClientService.DoJavascript(@"setTimeout(function () { 
                                    $('" + String.Join(" , ", listRemove.ToArray()) + @"').attr('type','button').removeAttr('ondragend').removeAttr('ondragstart').removeAttr('data-toggle').removeAttr('href').removeAttr('onclick').unbind('click').click(function () {
                                        AGMessage('ไม่สามารถใช้งานเมนูดังกล่าวได้ กรุณาติดต่อเจ้าหน้าที่เพื่อเปิดใช้สิทธิ์ในหน้าจอนี้ !!');
                                        });
                                    $('." + AUTH_INPUT_IMG + @"').attr('type','image');
                                }, 1001);");
            }
            #endregion;
        }

        public void checkValidCreate()
        {
            if (!menuObj.CanCreate)
            {
                throwNewException();
            }
        }

        public void checkValidUpdate()
        {
            if (!menuObj.CanChange)
            {
                throwNewException();
            }
        }

        private void throwNewException()
        {
            throw new Exception("ไม่สามารถใช้งานเมนูดังกล่าวได้ กรุณาติดต่อเจ้าหน้าที่เพื่อเปิดใช้สิทธิ์ในหน้าจอนี้ !!");
        }
        
        #endregion

        #region Event Button
        //private static string AUTH_All = "*";
        //private static string AUTH_CREATE = "01";
        //private static string AUTH_CHANGE = "02";
        //private static string AUTH_DISPLAY = "03";
        //private static string AUTH_COPY = "04";
        //private static string AUTH_DELETE = "05";
        //private static string AUTH_PRINT = "06";
        //private static string AUTH_CANCEL = "07";
        //private static string AUTH_REVERSE = "10";


        public Boolean canCreate()
        {
            if (ERPWAuthentication.EmployeeCode == null || ERPWAuthentication.EmployeeCode == "")
                return false;

            return menuObj.CanCreate;
            //Boolean hasAuthorize = AuthService.hasAuthorize(ERPWAuthentication.SID, Session[ApplicationSession.USER_SESSION_ID].ToString(),
            //    ERPWAuthentication.EmployeeCode, getProgramID(), AUTH_CREATE);

            //return hasAuthorize;
        }
        public Boolean canUpdate()
        {
            if (ERPWAuthentication.EmployeeCode == null || ERPWAuthentication.EmployeeCode == "")
                return false;

            return menuObj.CanChange;
            //Boolean hasAuthorize = AuthService.hasAuthorize(ERPWAuthentication.SID, Session[ApplicationSession.USER_SESSION_ID].ToString(),
            //    ERPWAuthentication.EmployeeCode, getProgramID(), AUTH_CHANGE);

            //return hasAuthorize;
        }
        public Boolean canDelete()
        {
            if (ERPWAuthentication.EmployeeCode == null || ERPWAuthentication.EmployeeCode == "")
                return false;

            return menuObj.CanDelete;

            //Boolean hasAuthorize = AuthService.hasAuthorize(ERPWAuthentication.SID, Session[ApplicationSession.USER_SESSION_ID].ToString(),
            //    ERPWAuthentication.EmployeeCode, getProgramID(), AUTH_DELETE);

            //return hasAuthorize;
        }
        public Boolean canDisplay()
        {
            if (ERPWAuthentication.EmployeeCode == null || ERPWAuthentication.EmployeeCode == "")
                return false;

            return menuObj.CanDisplay;
            //Boolean hasAuthorize = AuthService.hasAuthorize(ERPWAuthentication.SID, Session[ApplicationSession.USER_SESSION_ID].ToString(),
            //    ERPWAuthentication.EmployeeCode, getProgramID(), AUTH_DISPLAY);

            //return hasAuthorize;
        }
        public Boolean canCopy()
        {
            if (ERPWAuthentication.EmployeeCode == null || ERPWAuthentication.EmployeeCode == "")
                return false;

            return menuObj.CanCopy;
            //Boolean hasAuthorize = AuthService.hasAuthorize(ERPWAuthentication.SID, Session[ApplicationSession.USER_SESSION_ID].ToString(),
            //    ERPWAuthentication.EmployeeCode, getProgramID(), AUTH_COPY);

            //return hasAuthorize;
        }
        public Boolean canPrint()
        {
            //UserLoginService.UserSessionBean userBean = (UserLoginService.UserSessionBean)Session[ApplicationSession.USER_SESSION_BEAN];
            //if (userBean == null)
            //    return false;

            //Boolean hasAuthorize = AuthService.hasAuthorize(userBean.Systemid, userBean.Sessionid,
            //    userBean.Username, getProgramID(), AUTH_PRINT);

            //return hasAuthorize;
            // เนื่องจากของ PAN (Auth ไม่ได้ Control ถึง Print)
            return true;
        }
        public Boolean canCancel()
        {
            if (ERPWAuthentication.EmployeeCode == null || ERPWAuthentication.EmployeeCode == "")
                return false;

            return menuObj.CanCancel;
            //Boolean hasAuthorize = AuthService.hasAuthorize(ERPWAuthentication.SID, Session[ApplicationSession.USER_SESSION_ID].ToString(),
            //    ERPWAuthentication.EmployeeCode, getProgramID(), AUTH_CANCEL);

            //return hasAuthorize;
        }
        public Boolean canReverse()
        {
            if (ERPWAuthentication.EmployeeCode == null || ERPWAuthentication.EmployeeCode == "")
                return false;
            return menuObj.CanReverse;
            //Boolean hasAuthorize = AuthService.hasAuthorize(ERPWAuthentication.SID, Session[ApplicationSession.USER_SESSION_ID].ToString(),
            //    ERPWAuthentication.EmployeeCode, getProgramID(), AUTH_REVERSE);

            //return hasAuthorize;
        }

        #endregion

        #region save log Page_Error 

        protected void Page_Error(object sender, EventArgs e)
        {
            Exception objErr = Server.GetLastError();
            try
            {
                string CreatedOn = Validation.getCurrentServerStringDateTimeMillisecond();

                ErrorLogs err = new ErrorLogs();
                err.Message = objErr.Message.Replace("'", "\"");
                err.Source = objErr.Source.Replace("'", "\"");

                err.EmployeeCode = ERPWAuthentication.EmployeeCode;
                //err.LinkID = "";
                err.SID = ERPWAuthentication.SID;

                err.CreatedOn = CreatedOn;
                err.CreatedOnYear = CreatedOn.Substring(0, 4);
                err.CreatedOnMonth = CreatedOn.Substring(4, 2);
                err.CreatedOnDay = CreatedOn.Substring(6, 2);
                err.CreatedOnTime = CreatedOn.Substring(8, 6);

                err.TargetSite = HttpContext.Current.Request.Url.ToString();

                InsertErrorLogs(err);
            }
            catch { }
        }

        private void InsertErrorLogs(ErrorLogs err)
        {
            string sql = @"INSERT INTO [" + WebConfigHelper.getDatabaseSNAName() + @"].[dbo].[SNA_LINK_WEB_ERROR_LOG]
                                   ([Message]
                                   ,[Source]
                                   ,[TargetSite]
                                   ,[CreatedOn]
                                   ,[CreatedOnYear]
                                   ,[CreatedOnMonth]
                                   ,[CreatedOnDay]
                                   ,[CreatedOnTime]
                                   ,[SID]
                                   ,[LinkID]
                                   ,[EmployeeCode]
                                   ,[Remark])
                             VALUES (";

            sql += "'" + err.Message + "',";
            sql += "'" + err.Source + "',";
            sql += "'" + err.TargetSite + "',";
            sql += "'" + err.CreatedOn + "',";
            sql += "'" + err.CreatedOnYear + "',";
            sql += "'" + err.CreatedOnMonth + "',";
            sql += "'" + err.CreatedOnDay + "',";
            sql += "'" + err.CreatedOnTime + "',";
            sql += "'" + err.SID + "',";
            sql += "'" + err.LinkID + "',";
            sql += "'" + err.EmployeeCode + "',";
            sql += "'" + err.Remark + "');";

            new DBService().executeSQL(sql);
        }
        private class ErrorLogs
        {
            public string Message { get; set; }
            public string Source { get; set; }
            public string TargetSite { get; set; }
            public string CreatedOn { get; set; }
            public string CreatedOnYear { get; set; }
            public string CreatedOnMonth { get; set; }
            public string CreatedOnDay { get; set; }
            public string CreatedOnTime { get; set; }
            public string SID { get; set; }
            public string LinkID { get; set; }
            public string EmployeeCode { get; set; }
            public string Remark { get; set; }

        }
        #endregion


    }
}