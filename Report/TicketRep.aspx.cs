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
    }
}