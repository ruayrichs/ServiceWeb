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
    public partial class ProblemType : AbstractsSANWebpage
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
                    BindOwnerGroup();
                    bindDropdowCriteria();
                    BindingData();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void BindOwnerGroup()
        { 
            DataTable dt = lib.GetMasterConfigOwnerGroup(ERPWAuthentication.SID,ERPWAuthentication.CompanyCode,"");
            ddlOwnerGroup.DataTextField = "OwnerGroupName";
            ddlOwnerGroup.DataValueField = "OwnerGroupCode";
            ddlOwnerGroup.DataSource = dt;
            ddlOwnerGroup.DataBind();
            ddlOwnerGroup.Items.Insert(0, new ListItem("Select", ""));
        }

        private void bindDropdowCriteria()
        {
            ddlBusinessObject.DataTextField = "xValue";
            ddlBusinessObject.DataValueField = "xKey";
            ddlBusinessObject.DataSource = lib.getBusinessObjectConfig(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            ddlBusinessObject.DataBind();
            ddlBusinessObject.Items.Insert(0, new ListItem("Select", ""));
        }


        private void BindingData()
        {

            DataTable dt = lib.GetMasterConfigProblem(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
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
                ddlBusinessObject.SelectedValue = "";
                ddlProblemGroup.ClearSelection();
                ddlOwnerGroup.SelectedIndex = 0;

                tbCode.Enabled = true;
                ddlBusinessObject.Enabled = true;
                ddlProblemGroup.Enabled = true;

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
                string code = hdfEditCode.Value.Trim();
                string group = hdfGropCode.Value.Trim();
                string business = hdfBusiness.Value.Trim();
                string ownergroupcode = hdfOwnerGropCode.Value.Trim();
                hdfEditCode.Value = "";
                hdfGropCode.Value = "";
                hdfBusiness.Value = "";
                hdfOwnerGropCode.Value = "";

                DataTable dt = lib.GetMasterConfigProblem(ERPWAuthentication.SID,ERPWAuthentication.CompanyCode, business, group, code,"");

                if (dt.Rows.Count > 0)
                {
                    hdfMode.Value = ApplicationSession.CHANGE_MODE_STRING;
                    tbCode.Text = dt.Rows[0]["TYPECODE"].ToString();
                    tbName.Text = dt.Rows[0]["TYPENAME"].ToString();
                    ddlBusinessObject.SelectedValue = business;
                    DataTable dtGroup = lib.GetMasterConfigGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ddlBusinessObject.SelectedValue.Trim(), "");
                    
                    ddlProblemGroup.DataSource = dtGroup;
                    ddlProblemGroup.DataTextField = "GROUPNAME";
                    ddlProblemGroup.DataValueField = "GROUPCODE";
                    ddlProblemGroup.DataBind();
                    ddlProblemGroup.Items.Insert(0, new ListItem("Select", ""));
                    ddlProblemGroup.SelectedValue = group;

                    try
                    {
                        ddlOwnerGroup.SelectedValue = ownergroupcode;
                    }
                    catch (Exception)
                    {

                    }

                    tbCode.Enabled = false;
                    ddlBusinessObject.Enabled = false;
                    ddlProblemGroup.Enabled = false;
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
                string businessObj = ddlBusinessObject.SelectedValue.Trim();
                string GroupCode = ddlProblemGroup.SelectedValue.Trim();
                string OwnerGroupCode = ddlOwnerGroup.SelectedValue.Trim();
                string code = tbCode.Text.Trim();
                string name = tbName.Text.Trim();

                if (hdfMode.Value.Equals(ApplicationSession.CREATE_MODE_STRING))
                {
                    lib.CreateMasterConfigProblem(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, businessObj, GroupCode, code, name, OwnerGroupCode, ERPWAuthentication.UserName);
                }
                else if (hdfMode.Value.Equals(ApplicationSession.CHANGE_MODE_STRING))
                {
                    lib.UpdateMasterConfigProblem(ERPWAuthentication.SID, businessObj, GroupCode, code, name, OwnerGroupCode, ERPWAuthentication.UserName);
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

                string businessObj = (rpt.FindControl("hdfBusinessObject") as HiddenField).Value;
                string GroupCode = (rpt.FindControl("hdfGroupCode") as HiddenField).Value;
                string OwnerGroupCode = (rpt.FindControl("hdfOwnerCode") as HiddenField).Value;
                string code = btn.CommandArgument;
                lib.DeleteMasterConfigProblem(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, businessObj, GroupCode, code, OwnerGroupCode);
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


        #region Set Data Dropdown

        protected void btnSetGroupData_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = lib.GetMasterConfigGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ddlBusinessObject.SelectedValue.Trim(), "");
                ddlProblemGroup.DataSource = dt;
                ddlProblemGroup.DataTextField = "GROUPNAME";
                ddlProblemGroup.DataValueField = "GROUPCODE";
                ddlProblemGroup.DataBind();
                ddlProblemGroup.Items.Insert(0, new ListItem("Select", ""));
                if (dt.Rows.Count == 1)
                {
                    ddlProblemGroup.SelectedValue = dt.Rows[0]["GROUPCODE"].ToString();
                }
                udpn.Update();
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

        #endregion
    }
}