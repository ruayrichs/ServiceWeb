using ERPW.Lib.Authentication;
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
    public partial class MTTRReport : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return /*ERPWAuthentication.Permission.ReportView ||*/ ERPWAuthentication.Permission.AllPermission || ERPWAuthentication.Permission.ReportMTTR;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.ReportModify || ERPWAuthentication.Permission.AllPermission;
        }

        private ERPW.Lib.Service.Report.ReportDAO mttr = new ERPW.Lib.Service.Report.ReportDAO();
        private ReportUnity reportunity = new ReportUnity();
        DataTable dtSCType;

        //DataTable report
        //{
        //    get { return Session["MTRSReport.Report"] == null ? null : (DataTable)Session["MTRSReport.Report"]; }
        //    set { Session["MTRSReport.Report"] = value; }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetddlSctype();
            }

        }
        private void GetddlSctype()
        {
            mttr_ticket_type.Items.Clear();
            dtSCType = AfterSaleService.getInstance().getSearchDoctype("", ERPWAuthentication.CompanyCode, false, false);
            mttr_ticket_type.Items.Add(new ListItem("", ""));
            mttr_ticket_type.AppendDataBoundItems = true;
            mttr_ticket_type.DataTextField = "Description";
            mttr_ticket_type.DataValueField = "DocumentTypeCode";
            mttr_ticket_type.DataSource = dtSCType;
            mttr_ticket_type.DataBind();
        }
        private DataTable calculate_time(DataTable datatable)
        {
            for (int index = 0; index < datatable.Rows.Count; index++)
            {
                try
                {
                    datatable.Rows[index]["Time"] = "";
                    string opendate = (string)datatable.Rows[index]["Open_Date"];
                    string resolved_date = (string)datatable.Rows[index]["Resolved_Date"];
                    string close_log = (string)datatable.Rows[index]["Close_Log"];

                    datatable.Rows[index]["Time"] = reportunity.ConvertToTimeString(opendate, resolved_date);

                    if (!string.IsNullOrEmpty(opendate))
                        datatable.Rows[index]["Open_Date"] = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(opendate);
                    if (!string.IsNullOrEmpty(resolved_date))
                        datatable.Rows[index]["Resolved_Date"] = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(resolved_date);
                    if (!string.IsNullOrEmpty(close_log))
                        datatable.Rows[index]["Close_Log"] = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(close_log);
                }
                catch (Exception e1)
                {
                    System.Diagnostics.Debug.WriteLine(e1);
                }
            }
            return datatable;
        }

        public void clear_text()
        {
            mttr_incident_no.Text = string.Empty;
            mttr_ticket_type.SelectedValue = string.Empty;
            AutoCompleteCustomer.SelectedValue = string.Empty;
            mttr_opendate_from.Text = string.Empty;
            mttr_opendate_to.Text = string.Empty;
            mttr_resolved_date_from.Text = string.Empty;
            mttr_resolved_date_to.Text = string.Empty;
            mttr_close_date_from.Text = string.Empty;
            mttr_close_date_to.Text = string.Empty;
            AutoCompleteEmployee_Assignee_Resolved.SelectedValue = string.Empty;
            mttr_resolved_group.Text = string.Empty;
            mttr_time.Text = string.Empty;
        }

        protected void mttr_btn_search_Click(object sender, EventArgs e)
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

                string incident_no = mttr_incident_no.Text.ToString();
                string ticket_type = mttr_ticket_type.SelectedValue.ToString();
                string customer_code = AutoCompleteCustomer.SelectedValue.ToString();
                string opendate_from = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttr_opendate_from.Text.ToString());
                string opendate_to = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttr_opendate_to.Text.ToString());
                string resolved_date_from = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttr_resolved_date_from.Text.ToString());
                string resolved_date_to = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttr_resolved_date_to.Text.ToString());
                string close_date_from = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttr_close_date_from.Text.ToString());
                string close_date_to = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttr_close_date_to.Text.ToString());
                string assignee_resolved_code = AutoCompleteEmployee_Assignee_Resolved.SelectedValue.ToString();
                string resolved_group = mttr_resolved_group.Text.ToString();
                string time = mttr_time.Text.ToString();
                System.Diagnostics.Debug.WriteLine("ticket type : " + ticket_type);
                //DataTable dt_ticketlist
                DataTable report = mttr.getMTRSdata_v2(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    incident_no,
                    ticket_type,
                    customer_code,
                    opendate_from,
                    opendate_to,
                    resolved_date_from,
                    resolved_date_to,
                    close_date_from,
                    close_date_to,
                    assignee_resolved_code,
                    resolved_group,
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
        private void ExampleRepeater()
        {
            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);
            string OwnerGroupCode = "";
            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                OwnerGroupCode = ERPWAuthentication.Permission.OwnerGroupCode;
            }

            string incident_no = mttr_incident_no.Text.ToString();
            string ticket_type = mttr_ticket_type.SelectedValue.ToString();
            string customer_code = AutoCompleteCustomer.SelectedValue.ToString();
            string opendate_from = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttr_opendate_from.Text.ToString());
            string opendate_to = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttr_opendate_to.Text.ToString());
            string resolved_date_from = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttr_resolved_date_from.Text.ToString());
            string resolved_date_to = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttr_resolved_date_to.Text.ToString());
            string close_date_from = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttr_close_date_from.Text.ToString());
            string close_date_to = Agape.FocusOne.Utilities.Validation.Convert2DateDB(mttr_close_date_to.Text.ToString());
            string assignee_resolved_code = AutoCompleteEmployee_Assignee_Resolved.SelectedValue.ToString();
            string resolved_group = mttr_resolved_group.Text.ToString();
            string time = mttr_time.Text.ToString();
            System.Diagnostics.Debug.WriteLine("ticket type : " + ticket_type);
            //DataTable dt_ticketlist
            DataTable report = mttr.getMTRSdata_v2(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                incident_no,
                ticket_type,
                customer_code,
                opendate_from,
                opendate_to,
                resolved_date_from,
                resolved_date_to,
                close_date_from,
                close_date_to,
                assignee_resolved_code,
                resolved_group,
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
            Session["report.excel.Report_Excel_Export_Name"] = "MTRSReport";
            ClientService.DoJavascript("exportExcelAPI();");
        }
        protected void ui_export_button_Click(object sender, EventArgs e)
        {
            try
            {
                ExampleRepeater();
                System.Diagnostics.Debug.WriteLine("export success");
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