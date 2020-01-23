using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.Report
{
    public partial class ExportDataReport : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return /*ERPWAuthentication.Permission.ReportView ||*/ ERPWAuthentication.Permission.AllPermission || ERPWAuthentication.Permission.ReportExportData;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.ReportModify || ERPWAuthentication.Permission.AllPermission;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnExportDataCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> excludeCustomerGroup = new List<string>();
                if (!string.IsNullOrEmpty(hddExportCustomerExclude.Value))
                {
                    excludeCustomerGroup = hddExportCustomerExclude.Value.Split(',').ToList();
                }

                DataTable dtExport = CustomerService.getInstance().ExportDataCustomer(
                   ERPWAuthentication.SID,
                   ERPWAuthentication.CompanyCode,
                   excludeCustomerGroup
                );

                Session["report.excel.Report_Excel_Export_Datatable"] = dtExport;
                Session["report.excel.Report_Excel_Export_Name"] = "Export_Data_Client_and_Contact";
                ClientService.DoJavascript("exportExcelAPI();");
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