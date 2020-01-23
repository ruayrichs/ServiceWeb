using agape.lib.constant;
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
    public partial class AssetLocation : AbstractsSANWebpage
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
                    BindDropdown();
                    BindingData();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void BindDropdown()
        {
            DataTable dt = lib.GetCostCenterMaster(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);

            foreach (DataRow dr in dt.Rows)
            {
                dr["COSTCENTERNAME"] = dr["COSTCENTERCODE"].ToString().Trim() + " : " + dr["COSTCENTERNAME"].ToString().Trim();
            }

            ddlCostCenter.DataValueField = "COSTCENTERCODE";
            ddlCostCenter.DataTextField = "COSTCENTERNAME";            
            ddlCostCenter.DataSource = dt;
            ddlCostCenter.DataBind();
            ddlCostCenter.Items.Insert(0, new ListItem("", ""));
        }

        private void BindingData()
        {
            DataTable dt = lib.GetMasterConfigAssetLocation(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);

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

                tbCode.Text = "";
                tbName.Text = "";
                ddlCostCenter.SelectedValue = "";

                tbCode.Enabled = true;

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
                string code = hdfEditCode.Value;

                hdfEditCode.Value = "";

                DataTable dt = lib.GetMasterConfigAssetLocation(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, code);

                if (dt.Rows.Count > 0)
                {
                    hdfMode.Value = ApplicationSession.CHANGE_MODE_STRING;

                    tbCode.Text = dt.Rows[0]["AssetLocation"].ToString();
                    tbName.Text = dt.Rows[0]["Description"].ToString();
                    ddlCostCenter.SelectedValue = dt.Rows[0]["CostCenterCode"].ToString();

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
                string costCenter = ddlCostCenter.SelectedValue;

                if (hdfMode.Value.Equals(ApplicationSession.CREATE_MODE_STRING))
                {
                    lib.CreateMasterConfigAssetLocation(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, code, name, costCenter, ERPWAuthentication.UserName);
                }
                else if (hdfMode.Value.Equals(ApplicationSession.CHANGE_MODE_STRING))
                {
                    lib.UpdateMasterConfigAssetLocation(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, code, name, costCenter, ERPWAuthentication.UserName);
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

                string code = btn.CommandArgument;

                lib.DeleteMasterConfigAssetLocation(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, code);

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