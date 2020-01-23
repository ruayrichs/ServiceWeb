using Agape.FocusOne.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.widget.usercontrol
{
    public partial class AssetAgendaCtrl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string ToDay
        {
            get
            {
                string[] date = Validation.Convert2DateDisplay(Validation.getCurrentServerStringDateTime()).Split('/');
                return date[2] + "-" + date[1] + "-" + date[0];
            }
        }
    }
}