using ERPW.Lib.Authentication;
using System;
using System.Data;
using ERPW.Lib.Service.Report;
using ServiceWeb.Report;
using ServiceWeb.auth;
using System.Web.Configuration;
using Newtonsoft.Json;

namespace ServiceWeb.MasterPage
{
    public partial class CIMaintenanceReport : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return /*ERPWAuthentication.Permission.ReportView ||*/ ERPWAuthentication.Permission.AllPermission || ERPWAuthentication.Permission.ReportCIMaintenance;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.ReportModify || ERPWAuthentication.Permission.AllPermission;
        }

        CIReport CI_rep = new CIReport();
        private ReportUnity reportunity = new ReportUnity();
        //DataTable dt
        //{
        //    get { return Session["CIMaintenanceReport.Report"] == null ? null : (DataTable)Session["CIMaintenanceReport.Report"]; }
        //    set { Session["CIMaintenanceReport.Report"] = value; }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {

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

            string getCI_CODE = txtCICode.Text;
            string getCI_Name = txtCIName.Text;
            string getMaintenanceStartDateFrom = Agape.FocusOne.Utilities.Validation.Convert2DateDB(txtMaintenanceStartDateFrom.Text);
            string getMaintenanceEndDateFrom = Agape.FocusOne.Utilities.Validation.Convert2DateDB(txtMaintenanceEndDateFrom.Text);
            string getNextMaintenanceDueDate = Agape.FocusOne.Utilities.Validation.Convert2DateDB(txtNextMaintenanceDate.Text);
            string getMaintenanceStartDateTo = Agape.FocusOne.Utilities.Validation.Convert2DateDB(txtMaintenanceStartDateTo.Text);
            string getMaintenanceEndDateTo = Agape.FocusOne.Utilities.Validation.Convert2DateDB(txtMaintenanceEndDateTo.Text);
            getddlMaintenanceType = ddlMaintenanceType.SelectedValue;

            //DataTable dt = new DataTable();

            DataTable dt = CI_rep.getData(
                ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, 
                getCI_CODE, getCI_Name, getMaintenanceStartDateFrom,
                getMaintenanceEndDateFrom, getNextMaintenanceDueDate, getMaintenanceStartDateTo,
                getMaintenanceEndDateTo, getddlMaintenanceType, OwnerGroupCode
            );

            dt = calculate_time(dt);
            divDataJson.InnerHtml = JsonConvert.SerializeObject(dt);
            //TableCusDetail.DataSource = dt;
            //TableCusDetail.DataBind();
            UpdateTableCusDetail.Update();
            //ClientService.DoJavascript("$('#rep').DataTable({});");
            ClientService.DoJavascript("afterSearch();");
        }

        private DataTable calculate_time(DataTable datatable)
        {
            for (int index = 0; index < datatable.Rows.Count; index++)
            {
                try
                {

                    string BeginGuarantee = (string)datatable.Rows[index]["BeginGuarantee"];
                    string EndGuaranty = (string)datatable.Rows[index]["EndGuaranty"];
                    string NextMaintenanceDate = (string)datatable.Rows[index]["NextMaintenanceDate"];
                    string LastMaintenanceDate = (string)datatable.Rows[index]["LastMaintenanceDate"];
                    string CatagoryType = (string)datatable.Rows[index]["CategoryCode"];
                    if (CatagoryType == "01")
                    {
                        datatable.Rows[index]["CategoryCode"] = "Premium";
                    }
                    else if (CatagoryType == "02")
                    {
                        datatable.Rows[index]["CategoryCode"] = "Medium";
                    }
                    else if (CatagoryType == "00")
                    {
                        datatable.Rows[index]["CategoryCode"] = "-";
                    }

                    if (!string.IsNullOrEmpty(BeginGuarantee))
                        datatable.Rows[index]["BeginGuarantee"] = Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(BeginGuarantee);
                    if (!string.IsNullOrEmpty(EndGuaranty))
                        datatable.Rows[index]["EndGuaranty"] = Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(EndGuaranty);
                    if (!string.IsNullOrEmpty(NextMaintenanceDate))
                        datatable.Rows[index]["NextMaintenanceDate"] = Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(NextMaintenanceDate);
                    if (!string.IsNullOrEmpty(LastMaintenanceDate))
                        datatable.Rows[index]["LastMaintenanceDate"] = Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(LastMaintenanceDate);
                }
                catch (Exception e1)
                {
                    System.Diagnostics.Debug.WriteLine(e1);
                }
            }
            return datatable;
        }

        private void ExampleRepeater()
        {
            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);
            string OwnerGroupCode = "";
            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                OwnerGroupCode = ERPWAuthentication.Permission.OwnerGroupCode;
            }

            string getCI_CODE = txtCICode.Text;
            string getCI_Name = txtCIName.Text;
            string getMaintenanceStartDateFrom = Agape.FocusOne.Utilities.Validation.Convert2DateDB(txtMaintenanceStartDateFrom.Text);
            string getMaintenanceEndDateFrom = Agape.FocusOne.Utilities.Validation.Convert2DateDB(txtMaintenanceEndDateFrom.Text);
            string getNextMaintenanceDueDate = Agape.FocusOne.Utilities.Validation.Convert2DateDB(txtNextMaintenanceDate.Text);
            string getMaintenanceStartDateTo = Agape.FocusOne.Utilities.Validation.Convert2DateDB(txtMaintenanceStartDateTo.Text);
            string getMaintenanceEndDateTo = Agape.FocusOne.Utilities.Validation.Convert2DateDB(txtMaintenanceEndDateTo.Text);
            getddlMaintenanceType = ddlMaintenanceType.SelectedValue;

            //DataTable dt = new DataTable();

            DataTable dt = CI_rep.getData(
                ERPWAuthentication.SID, ERPWAuthentication.CompanyCode,
                getCI_CODE, getCI_Name, getMaintenanceStartDateFrom,
                getMaintenanceEndDateFrom, getNextMaintenanceDueDate, getMaintenanceStartDateTo,
                getMaintenanceEndDateTo, getddlMaintenanceType, OwnerGroupCode
            );

            dt = calculate_time(dt);

            Session["report.excel.Report_Excel_Export_Datatable"] = dt;
            Session["report.excel.Report_Excel_Export_Name"] = "Report_Warranty";
            ClientService.DoJavascript("exportExcelAPI();");
        }

        protected void btn_search(object sender, EventArgs e)
        {
            try
            {
                bindData();
                //dt = calculate_time(dt);
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
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                ExampleRepeater();
                //System.Diagnostics.Debug.WriteLine("exported");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }
        public string MaintenanceEndDateFrom { get; set; }
        public string MaintenanceStartDateTo { get; set; }
        public string MaintenanceStartDateFrom { get; set; }
        public string MaintenanceEndDateTo { get; set; }
        public string MaintenanceType { get; set; }
        public string NextMaintenanceDueDate { get; set; }
        public string CI_Name { get; set; }
        public string getddlMaintenanceType { get; set; }
    }
}