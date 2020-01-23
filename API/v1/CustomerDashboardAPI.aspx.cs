using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API.v1
{
    public partial class CustomerDashboardAPI : System.Web.UI.Page
    {
        private CustomerDashboardLib lib = new CustomerDashboardLib();

        private string _SID;
        private string SID
        {
            get
            {
                if (string.IsNullOrEmpty(_SID))
                    _SID = ERPWAuthentication.SID;
                return _SID;
            }
        }

        private string _CompanyCode;
        private string CompanyCode
        {
            get
            {
                if (string.IsNullOrEmpty(_CompanyCode))
                    _CompanyCode = ERPWAuthentication.CompanyCode;
                return _CompanyCode;
            }
        }

        private string _EmployeeCode;
        private string EmployeeCode
        {
            get
            {
                if (string.IsNullOrEmpty(_EmployeeCode))
                    _EmployeeCode = ERPWAuthentication.EmployeeCode;
                return _EmployeeCode;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string CustomerCode = Request["CustomerCode"];
            Response.Write(
                JsonConvert.SerializeObject(
                    lib.getFinanDataDashboard_Count(SID, CompanyCode, CustomerCode)
                )
            );
        }
    }
}