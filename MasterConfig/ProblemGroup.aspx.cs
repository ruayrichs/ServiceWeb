using agape.lib.constant;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Master.Entity;
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
    public partial class ProblemGroup : AbstractsSANWebpage
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
                    BindingData();
                    BindOwnerGroup();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void BindOwnerGroup()
        {
            DataTable dt = lib.GetMasterConfigOwnerGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "");
            ddlOwnerGroup.DataTextField = "OwnerGroupName";
            ddlOwnerGroup.DataValueField = "OwnerGroupCode";
            ddlOwnerGroup.DataSource = dt;
            ddlOwnerGroup.DataBind();
            ddlOwnerGroup.Items.Insert(0, new ListItem("Select", ""));
        }
        private void BindingData()
        {
            DataTable dt = lib.GetMasterConfigGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);

            rptItems.DataSource = dt;
            rptItems.DataBind();
            udpnItems.Update();

            List<MasterConfigEntity> listOBJ = lib.getBusinessObjectConfig(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            ddlBusinessObject.DataTextField = "xValue";
            ddlBusinessObject.DataValueField = "xKey";
            ddlBusinessObject.DataSource = listOBJ;
            ddlBusinessObject.DataBind();
            ddlBusinessObject.Items.Insert(0, new ListItem("Select", ""));


            ClientService.DoJavascript("bindingDataTableJS();");
        }

        protected void btnSetCreate_Click(object sender, EventArgs e)
        {
            try
            {
                hdfMode.Value = ApplicationSession.CREATE_MODE_STRING;

                tbCode.Text = "";
                tbName.Text = "";
                ddlBusinessObject.SelectedValue = "";
                ddlOwnerGroup.SelectedIndex = 0;

                tbCode.Enabled = true;
                ddlBusinessObject.Enabled = true;

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
                string businessObj = hdfBusinessOBjCode.Value.Trim();

                hdfEditCode.Value = "";
                hdfBusinessOBjCode.Value = "";

                DataTable dt = lib.GetMasterConfigGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, businessObj, code);

                if (dt.Rows.Count > 0)
                {
                    hdfMode.Value = ApplicationSession.CHANGE_MODE_STRING;

                    tbCode.Text = dt.Rows[0]["GroupCode"].ToString();
                    tbName.Text = dt.Rows[0]["GroupName"].ToString();
                    ddlBusinessObject.SelectedValue = dt.Rows[0]["BUSINESSOBJECT"].ToString().Trim();

                    try
                    {
                        ddlOwnerGroup.SelectedValue = dt.Rows[0]["OwnerGroupCode"].ToString();
                    }
                    catch (Exception)
                    {
                        ddlOwnerGroup.SelectedIndex = 0;
                    }

                    tbCode.Enabled = false;
                    ddlBusinessObject.Enabled = false;

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
                string business = ddlBusinessObject.SelectedValue.Trim();
                string code = tbCode.Text.Trim();
                string name = tbName.Text.Trim();
                string OwnerGroupCode = ddlOwnerGroup.SelectedValue.Trim();


                if (hdfMode.Value.Equals(ApplicationSession.CREATE_MODE_STRING))
                {
                    lib.CreateMasterConfigGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, business, code, name, OwnerGroupCode, ERPWAuthentication.UserName);
                }
                else if (hdfMode.Value.Equals(ApplicationSession.CHANGE_MODE_STRING))
                {
                    lib.UpdateMasterConfigGroup(ERPWAuthentication.SID, business, code, name, OwnerGroupCode, ERPWAuthentication.UserName);
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
                RepeaterItem rpt = btn.NamingContainer as RepeaterItem;
                string BusinessOBj = (rpt.FindControl("hdfBusinessObject") as HiddenField).Value; 
                string code = btn.CommandArgument;

                lib.DeleteMasterConfigGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, BusinessOBj, code);

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