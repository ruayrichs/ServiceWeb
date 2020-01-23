using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using Newtonsoft.Json;
using ServiceWeb.Report;
using System.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API.v1
{
    public partial class GetTicketMyWorkAPI : System.Web.UI.Page
    {
        private ServiceTicketLibrary lib = new ServiceTicketLibrary();
        private AppClientLibrary libAppClient = AppClientLibrary.GetInstance();
        private ReportUnity report_unity = new ReportUnity();

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
            DataTable dt = lib.GetMyWorkAPI(
                SID,
                CompanyCode,
                EmployeeCode,
                Validation.getCurrentServerStringDateTime(),
                Request["dataStatus"]
            );

            dt = report_unity.incidentNoFormater(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, dt, "TicketNoDisplay");
            Response.Write(JsonConvert.SerializeObject(dt));

        }
    }
}