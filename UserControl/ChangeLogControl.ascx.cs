using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.UserControl
{
    public partial class ChangeLogControl : System.Web.UI.UserControl
    {
        private LogServiceLibrary ServiceLog = new LogServiceLibrary();
     
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                                
            }
        }

        public void BindingLog(DataTable dt)
        {
            dt.Columns.Add("DateTime_DB");            

            foreach (DataRow dr in dt.Rows)
            {
                dr["DateTime_DB"] = Validation.Convert2DateDB(Convert.ToString(dr["access_date"])) +
                    Validation.Convert2TimeDB(Convert.ToString(dr["access_time"]));             
            }

            rptChangeLog.DataSource = dt;
            rptChangeLog.DataBind();

            udpnChangeLog.Update();

            ClientService.DoJavascript("afterSearchChangLog();");
           
        }
    }
}