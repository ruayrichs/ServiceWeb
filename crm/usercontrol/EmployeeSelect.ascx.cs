using Agape.Lib.DBService;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POSWeb.crm.usercontrol
{
    public partial class EmployeeSelect : System.Web.UI.UserControl
    {
        DBService dbservice = new DBService();
        public string SelectedValue
        {
            get
            {
                return txtSelectedEmployeeValue.Text;
            }
            set
            {
                ClientService.DoJavascript("setEmployeeSelect" + ClientID + "('" + value + "');");
                txtSelectedEmployeeValue.Text = value;
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
            ClientService.DoJavascript("EmployeeSelect" + ClientID + "();");
            if (!IsPostBack)
            {
                DataBind();
            }
        }

        public void DataBind()
        {

            string sql = @" SELECT [SID] ,[CompanyCode],[EmployeeCode],[FirstName]
                            ,[LastName],[FirstName_TH],[LastName_TH]
                            FROM [master_employee] 
            
                            where [SID]='"+ERPWAuthentication.SID+"' and CompanyCode='"+ERPWAuthentication.CompanyCode+"' ";

            DataTable dtEmployee = dbservice.selectDataFocusone(sql);
            rptEmployees.DataSource = dtEmployee;
            rptEmployees.DataBind();
        }
    }
}