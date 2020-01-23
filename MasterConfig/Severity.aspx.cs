using agape.lib.constant;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using ServiceWeb.auth;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig.MasterPage
{
    public partial class Severity : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        private MasterConfigLibrary lib = new MasterConfigLibrary();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindDropDown();
                    BindingData();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        public void BindDropDown()
        {
            DataTable dtUrgency = lib.GetMasterConfigUrgency(ERPWAuthentication.SID);

            foreach (DataRow dr in dtUrgency.Rows)
            {
                dr["UrgencyName"] = dr["UrgencyCode"] + " : " + dr["UrgencyName"];
            }

            ddlUrgency.DataSource = dtUrgency;
            ddlUrgency.DataValueField = "UrgencyCode";
            ddlUrgency.DataTextField = "UrgencyName";
            ddlUrgency.DataBind();
            ddlUrgency.Items.Insert(0, new ListItem("", ""));

            DataTable dtImpact = lib.GetMasterConfigImpact(ERPWAuthentication.SID);

            foreach (DataRow dr in dtImpact.Rows)
            {
                dr["ImpactName"] = dr["ImpactCode"] + " : " + dr["ImpactName"];
            }

            ddlImpact.DataSource = dtImpact;
            ddlImpact.DataValueField = "ImpactCode";
            ddlImpact.DataTextField = "ImpactName";
            ddlImpact.DataBind();
            ddlImpact.Items.Insert(0, new ListItem("", ""));

            DataTable dtPriority = lib.GetMasterConfigPriority(ERPWAuthentication.SID);

            foreach (DataRow dr in dtPriority.Rows)
            {
                dr["Description"] = dr["PriorityCode"] + " : " + dr["Description"];
            }

            ddlPriority.DataSource = dtPriority;
            ddlPriority.DataValueField = "PriorityCode";
            ddlPriority.DataTextField = "Description";
            ddlPriority.DataBind();
            ddlPriority.Items.Insert(0, new ListItem("", ""));
        }

        private void BindingData()
        {
            DataTable dt = lib.GetMasterConfigSeverity(ERPWAuthentication.SID);

            rptItems.DataSource = dt;
            rptItems.DataBind();

            udpnItems.Update();

            ClientService.DoJavascript("bindingDataTableJS();");
        }

        protected void btnSetCreate_Click(object sender, EventArgs e)
        {
            try
            {
                hdfMode.Value = ApplicationSession.CREATE_MODE_STRING;

                ddlUrgency.SelectedValue = "";
                ddlPriority.SelectedValue = "";
                ddlImpact.SelectedValue = "";

                ddlImpact.Enabled = true;
                ddlUrgency.Enabled = true;

                udpn.Update();

                ClientService.DoJavascript("openModal('New');");
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

        protected void btnSetEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string[] keys = hdfEditCode.Value.Split('|');

                DataTable dt = lib.GetMasterConfigSeverity(ERPWAuthentication.SID, keys[0], keys[1]);

                if (dt.Rows.Count > 0)
                {
                    hdfMode.Value = ApplicationSession.CHANGE_MODE_STRING;

                    ddlImpact.SelectedValue = dt.Rows[0]["ImpactCode"].ToString();
                    ddlUrgency.SelectedValue = dt.Rows[0]["UrgencyCode"].ToString();
                    ddlPriority.SelectedValue = dt.Rows[0]["PriorityCode"].ToString();

                    ddlImpact.Enabled = false;
                    ddlUrgency.Enabled = false;

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
                string code = ddlImpact.SelectedValue.Trim();
                string code1 = ddlUrgency.SelectedValue.Trim();
                string code2 = ddlPriority.SelectedValue.Trim();

                if (hdfMode.Value.Equals(ApplicationSession.CREATE_MODE_STRING))
                {
                    lib.CreateMasterConfigSeverity(ERPWAuthentication.SID, code, code1, code2);
                }
                else if (hdfMode.Value.Equals(ApplicationSession.CHANGE_MODE_STRING))
                {                  
                    lib.UpdateMasterConfigSeverity(ERPWAuthentication.SID, code, code1, code2);
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;

                string[] keys = btn.CommandArgument.Split('|');

                lib.DeleteMasterConfigSeverity(ERPWAuthentication.SID, keys[0], keys[1]);

                BindingData();

                ClientService.DoJavascript("closeModal('Delete');");
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