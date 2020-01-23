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
    public partial class AssetSubLocation : AbstractsSANWebpage
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
            DataTable dtLocation = lib.GetMasterConfigAssetLocation(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);

            foreach (DataRow dr in dtLocation.Rows)
            {
                dr["Description"] = dr["AssetLocation"] + " : " + dr["Description"];
            }

            ddlLocation.DataSource = dtLocation;
            ddlLocation.DataValueField = "AssetLocation";
            ddlLocation.DataTextField = "Description";
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, new ListItem("", ""));
        }

        private void BindingData()
        {
            DataTable dt = lib.GetMasterConfigSubLocation(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);

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
                ddlLocation.SelectedValue = "";

                tbCode.Enabled = true;
                ddlLocation.Enabled = true;

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

                hdfEditCode.Value = "";

                DataTable dt = lib.GetMasterConfigSubLocation(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, keys[0], keys[1]);

                if (dt.Rows.Count > 0)
                {
                    hdfMode.Value = ApplicationSession.CHANGE_MODE_STRING;

                    ddlLocation.SelectedValue = dt.Rows[0]["AssetLocation1"].ToString();
                    tbCode.Text = dt.Rows[0]["AssetLocation2"].ToString();
                    tbName.Text = dt.Rows[0]["Description"].ToString();

                    tbCode.Enabled = false;
                    ddlLocation.Enabled = false;

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
                string code1 = ddlLocation.SelectedValue.Trim();

                if (hdfMode.Value.Equals(ApplicationSession.CREATE_MODE_STRING))
                {
                    lib.CreateMasterConfigSubLocation(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, code, name, code1, ERPWAuthentication.UserName);
                }
                else if (hdfMode.Value.Equals(ApplicationSession.CHANGE_MODE_STRING))
                {                    
                    lib.UpdateMasterConfigSubLocation(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, code, name, code1, ERPWAuthentication.UserName);
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

                lib.DeleteMasterConfigSubLocation(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, keys[0], keys[1]);

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