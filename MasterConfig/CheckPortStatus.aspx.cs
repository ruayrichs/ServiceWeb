using ERPW.Lib.Authentication;
using ServiceWeb.auth;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig
{
    public partial class CheckPortStatus : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        private CheckPortService libCheckPort = new CheckPortService();
        protected void Page_Load(object sender, EventArgs e)
        {
            bindListPort();
        }

        private void bindListPort()
        {
            DataTable dt = libCheckPort.getDataPort();
            dt.Columns.Add("Status", typeof(string));

            foreach (DataRow dr in dt.Rows)
            {
                if (libCheckPort.CheckPortStatus(Convert.ToString(dr["Host"]), Convert.ToInt32(dr["Port"])))
                {
                    dr["Status"] = "fa-check-circle-o text-success";
                }
                else
                {
                    dr["Status"] = "fa-times-circle-o text-danger";
                }
            }
            rptItems.DataSource = dt;
            rptItems.DataBind();
        }
    }
}