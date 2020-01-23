using ServiceWeb.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.widget.usercontrol
{
    public partial class CustomerSelect : System.Web.UI.UserControl
    {
        public string SelectedValue
        {
            get
            {
                return txtSelectedValue.Text;
            }
            set
            {
                ClientService.DoJavascript("setCustomerSelect" + ClientID + "('" + value + "');");
                txtSelectedValue.Text = value;
            }
        }
        public string SelectedText
        {
            get
            {
                return txtSelectedText.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClientService.DoJavascript("CustomerSelect" + ClientID + "();");
                DataBind();
            }
        }

        public void DataBind()
        {
            DataTable dtCustomer = AfterSaleService.getInstance().getSearchCustomerCode("", ERPWAuthentication.CompanyCode);
            DataRow newRow = dtCustomer.NewRow();
            newRow[0] = "";
            newRow[1] = "กรุณาเลือกข้อมูลลูกค้า";
            dtCustomer.Rows.InsertAt(newRow, 0);

            rptCustomers.DataSource = dtCustomer;
            rptCustomers.DataBind();
        }

        public void RebindData()
        {
            ClientService.DoJavascript("CustomerSelect" + ClientID + "();");

            DataTable dtCustomer = AfterSaleService.getInstance().getSearchCustomerCode("", ERPWAuthentication.CompanyCode);
            DataRow newRow = dtCustomer.NewRow();
            newRow[0] = "";
            newRow[1] = "กรุณาเลือกข้อมูลลูกค้า";
            dtCustomer.Rows.InsertAt(newRow, 0);

            rptCustomers.DataSource = dtCustomer;
            rptCustomers.DataBind();
        }
    }
}