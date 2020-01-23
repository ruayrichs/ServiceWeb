
using agape.lib.constant;
using ERPLink.Lib.Service;
using ERPW.Lib.Authentication;
using SNA.Lib.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterPage
{
    public partial class IncludeLibraryMaster : System.Web.UI.MasterPage
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientService.DoJavascript("bindDateTimeControl();");

            if (!IsPostBack)
            {
               
            }
        }
    }
}