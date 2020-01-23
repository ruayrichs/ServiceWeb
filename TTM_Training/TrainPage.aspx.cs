using ERPW.Lib.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.TTM_Training
{
    public partial class TrainPage : System.Web.UI.Page
    {
        private TestLibForTrain lib = new TestLibForTrain();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                init();
            }
        }

        private void init()
        {
            DataTable dt = new DataTable();
            dt = lib.readAll();
            rptItem.DataSource = dt;
            rptItem.DataBind();
            udpnItems.Update();
        }
    }
}