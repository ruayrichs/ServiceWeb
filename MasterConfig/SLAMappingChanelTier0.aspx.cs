using agape.lib.constant;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using ServiceWeb.auth;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig
{
    public partial class SLAMappingChanelTier0 : AbstractsSANWebpage /*System.Web.UI.Page*/
    {

        private MasterConfigLibrary lib = new MasterConfigLibrary();
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
            try
            {
                if (!Page.IsPostBack)
                {
                    BindingData();
                    BindDDLLocation();
                    BindDDLPriority();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }
        private void BindingData()
        {
            DataTable dt = lib.GetMasterConfigImpactGetSLA(ERPWAuthentication.SID);

            rptItems.DataSource = dt;
            rptItems.DataBind();

            udpnItems.Update();

            ClientService.DoJavascript("bindingDataTableJS();");
        }
       
        protected void btnSetEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string code = hdfEditCode.Value;

                hdfEditCode.Value = "";

                DataTable dt = lib.GetMasterConfigImpactGetSLA(ERPWAuthentication.SID, code);

                if (dt.Rows.Count > 0)
                {
                    hdfMode.Value = ApplicationSession.CHANGE_MODE_STRING;

                    tbCode.Text = dt.Rows[0]["ChanelCode"].ToString();
                    tbName.Text = dt.Rows[0]["ChanelName"].ToString();
                    try
                    {
                        ddlSLA.Text = dt.Rows[0]["SLA_GroupCode"].ToString();
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        ddlPriority.Text = dt.Rows[0]["Priority"].ToString();
                    }
                    catch (Exception)
                    {
                    }

                    tbCode.Enabled = false;

                    udpn.Update();

                    ClientService.DoJavascript("openModal('Edit');");
                }
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string code = tbCode.Text.Trim();
                string name = tbName.Text.Trim();
                string ddlSLAtxt = ddlSLA.SelectedValue.ToString();
                string ddlPrioritytxt = ddlPriority.SelectedValue.ToString();

               
                if (hdfMode.Value.Equals(ApplicationSession.CHANGE_MODE_STRING))
                {
                    lib.UpdateMasterConfigImpactWithSLA(ERPWAuthentication.SID, code, ddlSLAtxt, name, ddlPrioritytxt);
                }

                BindingData();

                ClientService.DoJavascript("closeModal('Save');");
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
        private void BindDDLLocation()
        {
            DataTable dt = lib.getTierGroupCode(ERPWAuthentication.SID);
            ddlSLA.DataSource = dt;
            ddlSLA.DataTextField = "TierGroupDescription";
            ddlSLA.DataValueField = "TierGroupCode";
            ddlSLA.DataBind();
            ddlSLA.Items.Insert(0, new ListItem("เลือก", ""));

        }
        private void BindDDLPriority()
        {
            DataTable dt = lib.getpriorityGroupCode(ERPWAuthentication.SID);
            ddlPriority.DataSource = dt;
            ddlPriority.DataTextField = "Description";
            ddlPriority.DataValueField = "PriorityCode";
            ddlPriority.DataBind();
            ddlPriority.Items.Insert(0, new ListItem("เลือก", ""));

        }
    }
}