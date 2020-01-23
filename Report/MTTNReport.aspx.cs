using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using Newtonsoft.Json;
using ServiceWeb.auth;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.Report
{
    public partial class MTTNReport : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return /*ERPWAuthentication.Permission.ReportView ||*/ ERPWAuthentication.Permission.AllPermission || ERPWAuthentication.Permission.ReportMTTN;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.ReportModify || ERPWAuthentication.Permission.AllPermission;
        }

        private ERPW.Lib.Service.Report.ReportDAO mttn = new ERPW.Lib.Service.Report.ReportDAO();
        private ReportUnity reportunity = new ReportUnity();

        //DataTable report
        //{
        //    get { return Session["MTTNReport.Report"] == null ? null : (DataTable)Session["MTTNReport.Report"]; }
        //    set { Session["MTTNReport.Report"] = value; }
        //}

        DataTable dtSCType
        {
            get { return Session["ServiceCallCriteria.SCFC_SCType"] == null ? null : (DataTable)Session["ServiceCallCriteria.SCFC_SCType"]; }
            set { Session["ServiceCallCriteria.SCFC_SCType"] = value; }
        }

        DataTable ddl_datatable;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetddlSctype();
            }

        }

        private void GetddlSctype()
        {
            _mttn_ddl_ticket_type.Items.Clear();
            ddl_datatable = AfterSaleService.getInstance().getSearchDoctype("", ERPWAuthentication.CompanyCode, false, false);
            _mttn_ddl_ticket_type.Items.Add(new ListItem("", ""));
            _mttn_ddl_ticket_type.AppendDataBoundItems = true;
            _mttn_ddl_ticket_type.DataTextField = "Description";
            _mttn_ddl_ticket_type.DataValueField = "DocumentTypeCode";
            _mttn_ddl_ticket_type.DataSource = ddl_datatable;
            _mttn_ddl_ticket_type.DataBind();
        }

        public void clear_text()
        {
            mttn_incident_no.Text = string.Empty;
            _mttn_ddl_ticket_type.SelectedValue = string.Empty;
            AutoCompleteCustomer.SelectedValue = string.Empty;
            mttn_opendate.Text = string.Empty;
            mttn_opendate_to.Text = string.Empty;
            mttn_responding_date.Text = string.Empty;
            mttn_responding_date_to.Text = string.Empty;
            AutoCompleteEmployee_Assignee.SelectedValue = string.Empty;
            mttn_group.Text = string.Empty;
            mttn_time.Text = string.Empty;
        }

        private DataTable calculate_time(DataTable datatable)
        {
            for (int index = 0; index < datatable.Rows.Count; index++)
            {
                try
                {
                    datatable.Rows[index]["Time"] = "";
                    string opendate = (string)datatable.Rows[index]["OpenDate"];
                    string responding_date = (string)datatable.Rows[index]["Responding_Date"];

                    datatable.Rows[index]["Time"] = reportunity.ConvertToTimeString(opendate, responding_date);
                    
                    if (!string.IsNullOrEmpty(opendate))
                        datatable.Rows[index]["OpenDate"] = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(opendate);
                    if (!string.IsNullOrEmpty(responding_date))
                        datatable.Rows[index]["Responding_Date"] = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(responding_date);
                }
                catch (Exception e1)
                {
                    System.Diagnostics.Debug.WriteLine(e1);
                }
            }
            return datatable;
        }

        protected void mttn_btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                bool FilterOwner = false;
                bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);
                string OwnerGroupCode = "";
                if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
                {
                    OwnerGroupCode = ERPWAuthentication.Permission.OwnerGroupCode;
                }


                string incident_no = mttn_incident_no.Text.ToString();
                string ticket_type = _mttn_ddl_ticket_type.SelectedValue;
                string customer_name = AutoCompleteCustomer.SelectedValue.ToString();
                string opendate_ = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttn_opendate.Text.ToString());
                string opendate_to = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttn_opendate_to.Text.ToString());
                //System.Diagnostics.Debug.WriteLine(opendate_ + " " + opendate_to);
                string responding_date = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttn_responding_date.Text.ToString());
                string responding_date_to = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttn_responding_date_to.Text.ToString());
                //string assignee = mttn_assignee.Text.ToString();
                string assignee = AutoCompleteEmployee_Assignee.SelectedValue.ToString();
                string group = mttn_group.Text.ToString();
                string time = mttn_time.Text.ToString();
                //System.Diagnostics.Debug.WriteLine("ticket type : " + _mttn_ddl_ticket_type.SelectedItem);

                DataTable report = mttn.getMTTNdata_v2(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    incident_no,
                    ticket_type,
                    customer_name,
                    opendate_,
                    opendate_to,
                    responding_date,
                    responding_date_to,
                    assignee,
                    group,
                    time,
                    OwnerGroupCode
                    );

                report = reportunity.incidentNoFormater(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    report,
                    "IncidentNO"
                    );

                if (!report.Columns.Contains("Time"))
                {
                    report.Columns.Add("Time", typeof(string));
                }

                report = calculate_time(report);
                divDataJson.InnerHtml = JsonConvert.SerializeObject(report);
                //rptSearchSale.DataSource = report;
                //rptSearchSale.DataBind();
                //ClientService.DoJavascript("$('#tableItems').DataTable({});");
                ClientService.DoJavascript("afterSearch();");
                udpnItems.Update();
                clear_text();
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

        //export excel
        private CustomerService serviceCRM = CustomerService.getInstance();

        private void ExampleRepeater()
        {
            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);
            string OwnerGroupCode = "";
            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                OwnerGroupCode = ERPWAuthentication.Permission.OwnerGroupCode;
            }

            string incident_no = mttn_incident_no.Text.ToString();
            string ticket_type = _mttn_ddl_ticket_type.SelectedValue;
            string customer_name = AutoCompleteCustomer.SelectedValue.ToString();
            string opendate_ = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttn_opendate.Text.ToString());
            string opendate_to = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttn_opendate_to.Text.ToString());
            string responding_date = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttn_responding_date.Text.ToString());
            string responding_date_to = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttn_responding_date_to.Text.ToString());
            string assignee = AutoCompleteEmployee_Assignee.SelectedValue.ToString();
            string group = mttn_group.Text.ToString();
            string time = mttn_time.Text.ToString();

            DataTable report = mttn.getMTTNdata_v2(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                incident_no,
                ticket_type,
                customer_name,
                opendate_,
                opendate_to,
                responding_date,
                responding_date_to,
                assignee,
                group,
                time,
                OwnerGroupCode
                );

            report = reportunity.incidentNoFormater(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                report,
                "IncidentNO"
                );

            if (!report.Columns.Contains("Time"))
            {
                report.Columns.Add("Time", typeof(string));
            }

            report = calculate_time(report);
            Session["report.excel.Report_Excel_Export_Datatable"] = report;
            Session["report.excel.Report_Excel_Export_Name"] = "MTTNReport";
            ClientService.DoJavascript("exportExcelAPI();");
        }
        protected void ui_export_button_Click(object sender, EventArgs e)
        {
            try
            {
                ExampleRepeater();
                //System.Diagnostics.Debug.WriteLine("export success");
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debug.WriteLine("export fail: " + ex);
                ClientService.AGError(ex.Message);
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }
    }
}