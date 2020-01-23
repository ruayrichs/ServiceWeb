using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Master.Entity;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig
{
    public partial class EmailMessageCriteria : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }
        
        private MasterConfigLibrary lib = MasterConfigLibrary.GetInstance();
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
            List<ActivityEmailMessage> listDataSearch = lib.searchActivityEmailMessage(ERPWAuthentication.SID,"","");
            rptItems.DataSource = listDataSearch;
            rptItems.DataBind();
            udpnItems.Update();
            ClientService.DoJavascript("bindingDataTableJS();");
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string idGen = Guid.NewGuid().ToString().Substring(0, 8);
                Session["EmailMessageCriteria_rowkey" + idGen] = "";
                Response.Redirect("EmailMessageDetail.aspx?id=" + idGen);
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string rowkey = (sender as Button).CommandArgument;
                string  idGen = Guid.NewGuid().ToString().Substring(0, 8);
                Session["EmailMessageCriteria_rowkey"+ idGen] = rowkey;
                Response.Redirect("EmailMessageDetail.aspx?id="+ idGen);
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
            finally
            {

            }
        }
        
    }
}