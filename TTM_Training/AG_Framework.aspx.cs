using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Entity;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.TTM_Training
{
    public partial class AG_Framework : AbstractsSANWebpage
    {
        private CustomerService serviceCRM = CustomerService.getInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ExampleRepeater();
            }
        }

        private void ExampleRepeater()
        {
            List<CustomerProfile> dtCustomer = serviceCRM.SearchCustomerAllData(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                "","", "", "", "", "", "", "", "", "", null
            );
            dtCustomer = dtCustomer.Take(25).ToList();
            rptCard.DataSource = dtCustomer;
            rptCard.DataBind();
        }

        protected void rptCard_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string hiddenKey = (e.Item.FindControl("hiddenKey") as HiddenField).Value;
            // -------- logic ----------------
        }
    }
}