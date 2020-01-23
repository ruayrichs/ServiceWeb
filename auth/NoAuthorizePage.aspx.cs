using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.auth
{
    public partial class NoAuthorizePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["Cookie_AUTH_URL"] != null)
            {
                var value = Request.Cookies["Cookie_AUTH_URL"].Value;
                lblUrlAUTH.Text = value;
            }
            
        }
    }
}