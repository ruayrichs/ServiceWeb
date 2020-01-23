using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb
{
    public partial class TestRPT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindRPT();
            }
        }

        private void bindRPT()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("col1");
            dt.Columns.Add("col2");

            for (int i = 0; i < 10; i++)
            {
                DataRow drNew = dt.NewRow();
                drNew["col1"] = i + 1;
                drNew["col2"] = "Row " + (i + 1);
                dt.Rows.Add(drNew);
            }

            rpt1.DataSource = dt;
            rpt1.DataBind();
        }

        protected void rpt1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt2 = e.Item.FindControl("rpt2") as Repeater;
            HiddenField hdd1 = e.Item.FindControl("hdd1") as HiddenField;

            DataTable dt = new DataTable();
            dt.Columns.Add("col1");
            dt.Columns.Add("rpt2_col2");

            for (int i = 0; i < 10; i++)
            {
                DataRow drNew = dt.NewRow();
                drNew["col1"] = i + 1;
                drNew["rpt2_col2"] = "Row " + hdd1.Value + "." + (i + 1);
                dt.Rows.Add(drNew);
            }

            rpt2.DataSource = dt;
            rpt2.DataBind();
        }

        protected void rpt2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Panel panel1 = e.Item.FindControl("panel1") as Panel;
            panel1.Visible = false;
        }
    }
}