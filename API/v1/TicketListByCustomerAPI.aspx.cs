using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using Newtonsoft.Json;
using ServiceWeb.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API.v1
{
    public partial class TicketListByCustomerAPI : System.Web.UI.Page
    {
        private CustomerDashboardLib lib = new CustomerDashboardLib();
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
            string CustomerCode = Request["CustomerCode"];
            Int64 curDateTime = Int64.Parse(Validation.getCurrentServerStringDateTime());
            List<CustomerDashboardFinalDataModel.TicketListTable> datasTicket = lib.getFinanDataDashboard(
                SID, CompanyCode, CustomerCode
            );

            DataTable dt = report_unity.incidentNoFormater(
                ERPWAuthentication.SID, 
                ERPWAuthentication.CompanyCode,
                datasTicket.toDataTable(),
                "TicketNo4Display"
            );

            datasTicket = JsonConvert.DeserializeObject<List<CustomerDashboardFinalDataModel.TicketListTable>>(JsonConvert.SerializeObject(dt));

            TicketListModel datas = new TicketListModel();
            datas.OpenTask = datasTicket.Where(
                w =>
                w.CallStatus == "01"
            ).ToList();

            datas.DelayRisk = datasTicket.Where(
                w =>
                w.EndDateTime != null
                &&
                curDateTime > Int64.Parse(w.EndDateTime)
            ).ToList();

            datas.SuccessTask = datasTicket.Where(
                w =>
                w.CallStatus == "02"
            ).ToList();

            datas.AllTask = datasTicket;

            Response.Write(JsonConvert.SerializeObject(datas));
        }
        protected class TicketListModel
        {
            public List<CustomerDashboardFinalDataModel.TicketListTable> OpenTask = new List<CustomerDashboardFinalDataModel.TicketListTable>();
            public List<CustomerDashboardFinalDataModel.TicketListTable> DelayRisk = new List<CustomerDashboardFinalDataModel.TicketListTable>();
            public List<CustomerDashboardFinalDataModel.TicketListTable> SuccessTask = new List<CustomerDashboardFinalDataModel.TicketListTable>();
            public List<CustomerDashboardFinalDataModel.TicketListTable> AllTask = new List<CustomerDashboardFinalDataModel.TicketListTable>();
        }
    }
}