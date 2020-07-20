using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service.Report;
using Newtonsoft.Json;
using ServiceWeb.auth;
using SNAWeb.Analytics;
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
    public partial class TicketRep : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return /*ERPWAuthentication.Permission.ReportView ||*/ ERPWAuthentication.Permission.AllPermission || ERPWAuthentication.Permission.ReportTicketAnalysis;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.ReportModify || ERPWAuthentication.Permission.AllPermission;
        }

        TicketAnalysisRepLib Tic_Lib = new TicketAnalysisRepLib();
        AnalyticsService Analytics_Lib = new AnalyticsService();
        private ReportUnity reportunity = new ReportUnity();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string curDateTime = Validation.getCurrentServerDate();

                tbDateTimeIn.Text = curDateTime;
                tbDateTimeOut.Text = curDateTime;
            }
        }
        private void bindData()
        {

            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);
            string OwnerGroupCode = "";
            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                OwnerGroupCode = ERPWAuthentication.Permission.OwnerGroupCode;
            }

            string getPageMode = ddlPageModetype.SelectedValue;
            string gettxtDateTimeInView = Agape.FocusOne.Utilities.Validation.Convert2DateDB(tbDateTimeIn.Text);
            string gettxtDateTimeOutView = Agape.FocusOne.Utilities.Validation.Convert2DateDB(tbDateTimeOut.Text);
            string gettxtEmployee = AutoCompleteEmployee.SelectedValue;
            if (string.IsNullOrEmpty(AutoCompleteEmployee.SelectedDisplay))
            {
                gettxtEmployee = "";
            }

            TimeSpan time = TimeSpan.FromSeconds(0);
            string str = time.ToString(@"hh\ mm\ ss\ fff");
            
            //string gettxtDateTimeIn = String.Concat(gettxtDateTimeInView, str.Replace(" ", ""));
            //string gettxtDateTimeOut = String.Concat(gettxtDateTimeOutView, str.Replace(str, "235959999"));
            DataTable dt = Tic_Lib.getData(
                ERPWAuthentication.SID, ERPWAuthentication.CompanyCode,
                getPageMode, gettxtDateTimeInView, gettxtDateTimeOutView,
                gettxtEmployee, OwnerGroupCode
            );

            //DataTable dtCloned = dt.Clone();
            //dtCloned.Columns["LiveOn"].DataType = typeof(string);
            //foreach (DataRow row in dt.Rows)
            //{
            //    dtCloned.ImportRow(row);
            //}
            //dtCloned = calculate_time(dt);

            //dt.Columns["LiveOn"].DataType = typeof(string);
            dt = calculate_time(dt);

            divDataJson.InnerHtml = JsonConvert.SerializeObject(dt);

            //TicketRepeater.DataSource = dtCloned;
            //TicketRepeater.DataBind();
            //ClientService.DoJavascript("$('#repTB').DataTable({});");
            ClientService.DoJavascript("afterSearch();");
            UpdateTable.Update();

            if (check)
            {
                export = dt;
            }
        }
        protected void btn_search(object sender, EventArgs e)
        {
            try
            {
                bindData();               
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
        protected void btn_udpDateTimeOut(object sender, EventArgs e)
        {
            try
            {
                //Button btn = sender as Button;
                //string Row_Key = btn.CommandArgument;
                string Row_Key = hddKey.Value;

                AnalyticsService.UpdateExitPage(Row_Key, Validation.getCurrentServerStringDateTimeMillisecond());
                UpdateTable.Update();
                bindData();

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

        private DataTable calculate_time(DataTable datatable)
        {
            for (int index = 0; index < datatable.Rows.Count; index++)
            {
                string opendate = datatable.Rows[index]["LiveOn"].ToString();
                string dateIn = datatable.Rows[index]["DateTimeIn"] + "";
                string dateOut = datatable.Rows[index]["DateTimeOut"] + "";

                datatable.Rows[index]["LiveOn"] = reportunity.ConvertToTimeLiveOn(opendate);

                if (!string.IsNullOrEmpty(dateIn))
                    datatable.Rows[index]["DateTimeIn"] = Validation.Convert2DateTimeDisplay(dateIn);
                if (!string.IsNullOrEmpty(dateOut))
                    datatable.Rows[index]["DateTimeOut"] = Validation.Convert2DateTimeDisplay(dateOut);

            }
            return datatable;
        }

        //======================== export data
        private void ExampleRepeater(DataTable dt)
        {
            DataTable ticketReport = new DataTable();

            ticketReport.Clear();
            ticketReport.Columns.Add("Employee Name");
            ticketReport.Columns.Add("PageName");
            ticketReport.Columns.Add("OS");
            ticketReport.Columns.Add("Browser");
            ticketReport.Columns.Add("Mobile");
            ticketReport.Columns.Add("LiveOn");
            ticketReport.Columns.Add("Date Time In");
            ticketReport.Columns.Add("Date Time Out");
            ticketReport.Columns.Add("ReferenceID");
            ticketReport.Columns.Add("Reference Page Mode");

            foreach (DataRow dr in dt.Rows)
            {
                if ("CHANGE" == dr["ReferencePageMode"].ToString() && dr["DateTimeOut"].ToString() == "")
                {
                    dr["DateTimeOut"] = "Update Date Time Out";
                }

                if (dr["LastName"].ToString() != "")
                {
                    dr["FirstName"] = dr["FirstName"].ToString() + " " + dr["LastName"].ToString();
                }

                DataRow _ravi = ticketReport.NewRow();
                _ravi["Employee Name"] = dr["FirstName"].ToString();
                _ravi["PageName"] = dr["PageName"].ToString();
                _ravi["OS"] = dr["OS"].ToString();
                _ravi["Browser"] = dr["Browser"].ToString();
                _ravi["Mobile"] = dr["Mobile"].ToString();
                _ravi["LiveOn"] = dr["LiveOn"].ToString();
                _ravi["Date Time In"] = dr["DateTimeIn"].ToString();
                _ravi["Date Time Out"] = dr["DateTimeOut"].ToString();
                _ravi["ReferenceID"] = dr["ReferenceID"].ToString();
                _ravi["Reference Page Mode"] = dr["ReferencePageMode"].ToString();
                ticketReport.Rows.Add(_ravi);
            }

            Session["report.excel.Report_Excel_Export_Datatable"] = ticketReport;
            Session["report.excel.Report_Excel_Export_Name"] = "Ticket Analysis";
            ClientService.DoJavascript("exportExcelAPI();");
        }

        private bool check = false;
        DataTable export = new DataTable();
        protected void ui_export_button_Click(object sender, EventArgs e)
        {
            try
            {
                check = true;
                bindData();
                ExampleRepeater(export);
                System.Diagnostics.Debug.WriteLine("export success");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("export fail: " + ex);
                throw;
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }
        //======================== \export data
    }
}