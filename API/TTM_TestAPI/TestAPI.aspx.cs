using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API.TTM_TestAPI
{
    public partial class TestAPI : System.Web.UI.Page
    {
        private DashboardLib dashboardLib = new DashboardLib();

        private string _SID;
        private string _CompanyCode;
        private DashboardFinalDataModel _DashboardFinalData;

        private string SID
        {
            get
            {
                if (string.IsNullOrEmpty(_SID))
                    _SID = ERPWAuthentication.SID;
                return _SID;
            }
        }
        private string CompanyCode
        {
            get
            {
                if (string.IsNullOrEmpty(_CompanyCode))
                    _CompanyCode = ERPWAuthentication.CompanyCode;
                return _CompanyCode;
            }
        }
        private DashboardFinalDataModel DashboardFinalData
        {
            get
            {
                if (_DashboardFinalData == null)
                    _DashboardFinalData = dashboardLib.PreparFinanDataDashboard(SID, CompanyCode);
                return _DashboardFinalData;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            loadData();
        }
        /// <summary>
        /// For Testing
        /// </summary>
        private void loadData()
        {
            //string jsonResult = @"hello world";
            var json = new JavaScriptSerializer().Serialize(DashboardFinalData);
            //json = JsonConvert.SerializeObject(DashboardFinalData);
            Response.Write(json);
        }
    }
}