using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.Accountability.MasterPage
{
    public partial class AccountabilityMaster : System.Web.UI.MasterPage
    {
        public string WorkGroupCode
        {
            get
            {
                return "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //string sid = ERPWAuthentication.SID;
            //string conpanycode = ERPWAuthentication.CompanyCode;
        }
    }
}