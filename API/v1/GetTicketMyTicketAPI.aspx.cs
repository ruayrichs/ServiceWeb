using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
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
    public partial class GetTicketMyTicketAPI : System.Web.UI.Page
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
            DataSet dsFinalDatas = new DataSet();
            dsFinalDatas.Tables.Add("my_ticket");

            DataSet ds = lib.GetMyTicket(
                SID,
                CompanyCode,
                EmployeeCode,
                Request["dataStatus"] // "" == ดึง Ticket ทั้งหมด // active == ดึง Ticket ที่ยังไม่ Close หรือ Cancel // inactive == ดึง Ticket ที่ Close หรือ Cancel
            );

            DataTable dt = ds.Tables["my_ticket"];
            dt.DefaultView.ToTable(true, "TicketNoDisplay");
            dt = report_unity.incidentNoFormater(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, dt, "TicketNoDisplay");
            dt = dt.DefaultView.ToTable(
                true,
                "Doctype",
                "CallerID",
                "Fiscalyear",
                "CustomerCode",
                "StartDateTime",
                "EndDateTime",
                "DocumentTypeDesc",
                "TicketNoDisplay",
                "HeaderText",
                "CustomerName",
                "StatusCode",
                "StatusDesc",
                "OwnerGroupService",
                "CustomerCritical",
                "WorkFlowStatus"
            );
            dsFinalDatas.Tables["my_ticket"].Merge(dt);
            Response.Write(JsonConvert.SerializeObject(dsFinalDatas));
        }
    }
}