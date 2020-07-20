using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API
{
    public partial class DashboardAPI : System.Web.UI.Page
    {
        #region object
        //private DashboardLib dashboardLib = new DashboardLib();
        //private DashboardAPILib dashboardAPILib = new DashboardAPILib();
        //private DateTime cdt = DateTime.Now;

        //private string _day;
        //private string _month;
        //private string _year;
        //private string _SID;
        private string _OptionData;
        //private string _CompanyCode;
        //private DashboardFinalDataModel _DashboardFinalData;

        ////private string day { get
        ////    {
        ////        if (string.IsNullOrEmpty(_day))
        ////        {
        ////            if (cdt.Day < 10)
        ////            {
        ////                _day = "0"+ cdt.Day.ToString();
        ////            }
        ////            else
        ////            {
        ////                _day = cdt.Day.ToString();
        ////            }                    

        ////        }
        ////        return _day;
        ////    } }
        ////private string month
        ////{
        ////    get
        ////    {
        ////        if (string.IsNullOrEmpty(_month))
        ////        {
        ////            if (cdt.Month < 10)
        ////            {
        ////                _month = "0"+ cdt.Month.ToString();
        ////            }
        ////            else
        ////            {
        ////                _month = cdt.Month.ToString();
        ////            }
                    
        ////        }
        ////        return _month;
        ////    }
        ////}
        ////private string year
        ////{
        ////    get
        ////    {
        ////        return cdt.Year.ToString();
        ////    }
        ////}
        //private string SID
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_SID))
        //            _SID = ERPWAuthentication.SID;
        //        return _SID;
        //    }
        //}
        private string OptionData { get
            {
                if (string.IsNullOrEmpty(_OptionData))
                {
                    _OptionData = Request.QueryString["optiondata"];
                }
                return _OptionData;
            } }
        //private string CompanyCode
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_CompanyCode))
        //            _CompanyCode = ERPWAuthentication.CompanyCode;
        //        return _CompanyCode;
        //    }
        //}
        //private DashboardFinalDataModel DashboardFinalData
        //{
        //    get
        //    {
        //        if (_DashboardFinalData == null)
        //            _DashboardFinalData = dashboardAPILib.PreparFinanDataDashboard(SID, CompanyCode, OptionData);
        //        return _DashboardFinalData;
        //    }
        //}
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            //receivePOST();
            loadData();
        }

        /// <summary>
        /// This DashBoardData API
        /// </summary>
        private void loadData()
        {
            DashboardLib dashboardLib = new DashboardLib();

            bool _IsFilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out _IsFilterOwner);
            if (ERPWAuthentication.Permission.AllPermission || ERPWAuthentication.Permission.DashboardViewAll)
            {
                _IsFilterOwner = false;
            }
           
            DashboardFinalDataModel DashboardFinalData = dashboardLib.PreparFinanDataDashboard(
                ERPWAuthentication.SID, ERPWAuthentication.CompanyCode,
                OptionData, _IsFilterOwner, ERPWAuthentication.Permission.OwnerGroupCode, ERPWAuthentication.EmployeeCode
            );

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            var json = serializer.Serialize(DashboardFinalData);
            Response.Write(json);
        }

        //private void receivePOST()
        //{
        //    System.Diagnostics.Debug.WriteLine(optiondata);
        //}
    }
}