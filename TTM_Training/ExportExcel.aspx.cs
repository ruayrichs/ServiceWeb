using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.TTM_Training
{
    public partial class ExportExcel : System.Web.UI.Page
    {
        private CustomerService serviceCRM = CustomerService.getInstance();
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        private void ExampleRepeater()
        {
            List<CustomerProfile> dtCustomer = serviceCRM.SearchCustomerAllData(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                "", "", "", "", "", "", "", "", "", "", null
            );

            DataTable dt = dtCustomer.toDataTable();
            Session["report.excel.Report_Excel_Export_Datatable"] = dt;
            Session["report.excel.Report_Excel_Export_Name"] = "Report Customer";
            ClientService.DoJavascript("exportExcelAPI();");
        }

        protected void btnExpoet_Click(object sender, EventArgs e)
        {
            try
            {
                ExampleRepeater();
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