using ERPW.Lib.Authentication;
using ERPW.Lib.Authentication.Entity;
using ERPW.Lib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterPage
{
    public partial class MasterPage : System.Web.UI.MasterPage
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


        public bool IsFilterOwner
        { 
            get { return (Master as HeaderMaster).IsFilterOwner; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void TicketRedirecPage(string BusinessObject)
        {

            Session["TK_BusinessObject"] = BusinessObject;
            Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/ServiceCallFastEntryCriteria.aspx"));
        }

        protected void btnLink_TicketService_Click(object sender ,EventArgs e) {
            try
            {
                LinkButton btnL = (sender as LinkButton);
                string BusinessObject = "";

                if (btnL.CommandArgument == "I")
                {
                    BusinessObject = ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_INCIDENT;
                }
                if (btnL.CommandArgument == "R")
                {
                    BusinessObject = ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_REQUEST;
                }
                if (btnL.CommandArgument == "P")
                {
                    BusinessObject = ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_PROBLEM;
                }

                TicketRedirecPage(BusinessObject);
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
            finally
            {
                ClientService.AGLoading(false);
            }

        }
    }
}