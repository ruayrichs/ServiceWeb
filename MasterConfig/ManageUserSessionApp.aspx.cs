using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.F1WebService;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig
{
    public partial class ManageUserSessionApp : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //bindDatas();
            }
        }

        private void bindDatas()
        {
            DataTable dt = F1WebService.getUserLoginService().getAllSessionInfo("555");
            dt.Columns.Add("DateTimeSession", typeof(string));
            dt.Columns.Add("DateDB", typeof(string));

            foreach (DataRow dr in dt.Rows)
            {
                DateTime dataTime = DateTime.ParseExact(
                    (dr["LOGINDATE"].ToString() + " " + dr["LOGINTIME"].ToString()), 
                    "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture
                );


                string dataDateTimeSession = (
                    dataTime.Year.ToString() +
                    dataTime.Month.ToString().PadLeft(2, '0') +
                    dataTime.Day.ToString().PadLeft(2, '0') +
                    dataTime.Hour.ToString().PadLeft(2, '0') +
                    dataTime.Minute.ToString().PadLeft(2, '0') +
                    dataTime.Second.ToString().PadLeft(2, '0')
                 );

                dr["DateDB"] = dataDateTimeSession.Substring(0, 8);
                dr["DateTimeSession"] = Validation.Convert2DateTimeDisplay(dataDateTimeSession);
            }

            string strWhere = " 1 = 1 ";
            if (!string.IsNullOrEmpty(txtDateFrom.Text))
            {
                strWhere += " and DateDB >= '" + Validation.Convert2DateDB(txtDateFrom.Text) + "' ";
            }
            if (!string.IsNullOrEmpty(txtDateTo.Text))
            {
                strWhere += " and DateDB <= '" + Validation.Convert2DateDB(txtDateTo.Text) + "' ";
            }

            dt.DefaultView.RowFilter = strWhere;
            dt = dt.DefaultView.ToTable();

            rptItems.DataSource = dt;
            rptItems.DataBind();
            udpnItems.Update();
        }

        protected void btnRemoveSession_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RepeaterItem item in rptItems.Items)
                {
                    CheckBox chk = item.FindControl("chkRemoveSession") as CheckBox;
                    if (chk.Checked)
                    {
                        HiddenField hddSessionID = item.FindControl("hddSessionID") as HiddenField;
                        HiddenField hddUserName = item.FindControl("hddUserName") as HiddenField;
                        HiddenField hddTerminal = item.FindControl("hddTerminal") as HiddenField;

                        F1WebService.getUserLoginService().removeUserSessionBeanBySessionID(
                            ERPWAuthentication.SID,
                            hddSessionID.Value,
                            hddUserName.Value,
                            hddTerminal.Value
                        );
                    }
                }

                bindDatas();
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

        protected void btnSearchData_Click(object sender, EventArgs e)
        {
            try
            {
                bindDatas();
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
    }
}