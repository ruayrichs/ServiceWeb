using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig
{
    public partial class MailRecurring : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        private RecurringLibrary lib = new RecurringLibrary();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindingData();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void BindingData()
        {
            DataTable dt = lib.GetEmailRecurring(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);

            rptItems.DataSource = dt;
            rptItems.DataBind();

            udpnItems.Update();

            ClientService.DoJavascript("bindingDataTableJS();");
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;

                string clientPageId = Guid.NewGuid().ToString().Substring(0, 8);

                if (!string.IsNullOrEmpty(btn.CommandArgument))
                {
                    Session["batchid" + clientPageId] = btn.CommandArgument;
                }

                Response.Redirect(Page.ResolveUrl("~/MasterConfig/MailRecurringDetail.aspx?pid=" + clientPageId));
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;

                string[] keys = btn.CommandArgument.Split('|');

                string batchId = keys[0];
                string pop3Code = keys[1];

                lib.DeleteEmailRecurring(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, batchId, pop3Code);

                ClientService.AGSuccess("Deleted Success.");

                BindingData();
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