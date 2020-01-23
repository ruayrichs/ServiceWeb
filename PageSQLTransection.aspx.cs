using Agape.Lib.DBService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb
{
    public partial class PageSQLTransection : System.Web.UI.Page
    {
        DBService db = new DBService();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSelectData_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = db.selectDataFocusone(txtSQL_Script.Text);
                panelResult.InnerHtml = ConvertDataTableToHTML(dt);
                udpData.Update();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        public static string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table class='table table-bordered'>";
            //add header row
            html += "<thead>";
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<th>" + dt.Columns[i].ColumnName + "</th>";
            html += "</tr>";
            html += "</thead>";
            //add rows
            html += "<tbody>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</tbody>";
            html += "</table>";
            return html;
        }
    }
}