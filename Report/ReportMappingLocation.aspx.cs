using ERPW.Lib.Authentication;
using ERPW.Lib.Service.Report;
using Newtonsoft.Json;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls;

namespace ServiceWeb.Report
{
    public partial class ReportMappingLocation : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return /*ERPWAuthentication.Permission.ReportView ||*/ ERPWAuthentication.Permission.AllPermission || ERPWAuthentication.Permission.ReportReportLocation;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.ReportModify || ERPWAuthentication.Permission.AllPermission;
        }


        LocationReport LocateRep = new LocationReport();
        //DataTable dt
        //{
        //    get { return Session["ReportMappingLocation.Report"] == null ? null : (DataTable)Session["ReportMappingLocation.Report"]; }
        //    set { Session["ReportMappingLocation.Report"] = value; }
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDDLLocation();
                BindDDLEmployee();
            }
     
        }
        private void bindData()
        {

           // DataTable dt = new DataTable();
            string getCI_CODE = txtCICode.Text;
            string getCI_Name = txtCIName.Text;
            string getLocationddl = Locationddl.SelectedValue;
            string getEmployeeddl = Employeeddl.SelectedValue;

            //dt = LocateRep.getLocation(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, getCI_CODE, getCI_Name,
            //    getLocationddl, getEmployeeddl
            //);

            // --- Final Data --- //
            List<ChangeLocationReportEntity> LocationReport = LocateRep.getReportEquipmentChangeLocationLog(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                getCI_CODE, getCI_Name,
                getLocationddl,
                getEmployeeddl
            );
            // --- Final Data --- //       
            DataTable dt = LocationReport.toDataTable();
            divDataJson.InnerHtml = JsonConvert.SerializeObject(dt);

            //LocationReportDetail.DataSource = LocationReport;
            //LocationReportDetail.DataBind();
            UpdateTableCusDetail.Update();
            ClientService.DoJavascript("afterSearch();");
        }
        private void BindDDLLocation()
        {
            DataTable dt = LocateRep.getLocationMaster(ERPWAuthentication.SID);

            Locationddl.DataValueField = "LocationCode";
            Locationddl.DataTextField = "LocateName";

            Locationddl.DataSource = dt;
            Locationddl.DataBind();

            Locationddl.Items.Insert(0, new ListItem("เลือก", ""));

        }
        private void BindDDLEmployee()
        {
            DataTable dt = LocateRep.getEmployee(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);

            Employeeddl.DataValueField = "EmployeeCode";
            Employeeddl.DataTextField = "EmployeeName";

            Employeeddl.DataSource = dt;
            Employeeddl.DataBind();

            Employeeddl.Items.Insert(0, new ListItem("เลือก", ""));

        }
        private void ExampleRepeater()
        {
            string getCI_CODE = txtCICode.Text;
            string getCI_Name = txtCIName.Text;
            string getLocationddl = Locationddl.SelectedValue;
            string getEmployeeddl = Employeeddl.SelectedValue;
            List<ChangeLocationReportEntity> LocationReport = LocateRep.getReportEquipmentChangeLocationLog(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                getCI_CODE, getCI_Name,
                getLocationddl,
                getEmployeeddl
            );
            // --- Final Data --- //       
            DataTable dt = LocationReport.toDataTable();
            Session["report.excel.Report_Excel_Export_Datatable"] = dt;
            Session["report.excel.Report_Excel_Export_Name"] = "Report_CI_Location";
            ClientService.DoJavascript("exportExcelAPI();");
        }

        protected void btn_search(object sender, EventArgs e)
        {
            try
            {
                bindData();
                ClientService.DoJavascript("scrollToTable();");
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
        protected void ui_export_button_Click(object sender, EventArgs e)
        {
            try
            {
                ExampleRepeater();
                System.Diagnostics.Debug.WriteLine("exported");
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
    }
}
