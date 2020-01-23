using ERPW.Lib.Authentication;
using ERPW.Lib.Authentication.Entity;
using ServiceWeb.MasterPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig.MasterPage
{
    public partial class MasterPageConfig : System.Web.UI.MasterPage
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

        public string WorkGroupCode
        {
            get
            {
                return "";
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}