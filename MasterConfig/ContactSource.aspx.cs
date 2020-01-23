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
    public partial class ContactSource : AbstractsSANWebpage
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
                    bindBusinessObjectSelected();
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
            DataTable dt = lib.GetMasterConfigContact(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);

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

                ddlBusinessObject.SelectedValue = "";
                ddlOwnerGroup.SelectedIndex = 0;
                bindProblemGroupSelected();
                bindProblemTypeSelected();
                bindProblemSourceSelected();
                tbCode.Text = "";
                tbName.Text = "";

                ddlBusinessObject.Enabled = true;
                ddlProblemGroup.Enabled = true;
                ddlProblemType.Enabled = true;
                ddlProblemSource.Enabled = true;
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
                string businessObj = hdfBusiness.Value.Trim();
                string groupCode = hdfGropCode.Value.Trim();
                string typeCode = hdfTypeCode.Value.Trim();
                string sourceCode = hdfSourceCode.Value.Trim();
                string code = hdfEditCode.Value.Trim();

                hdfBusiness.Value = "";
                hdfGropCode.Value = "";
                hdfTypeCode.Value = "";
                hdfSourceCode.Value = "";
                hdfEditCode.Value = "";

                DataTable dt = lib.GetMasterConfigContact(
                    SID, 
                    CompanyCode, 
                    businessObj, groupCode, 
                    typeCode, sourceCode, code, ""
                );

                if (dt.Rows.Count > 0)
                {
                    hdfMode.Value = ApplicationSession.CHANGE_MODE_STRING;


                    ddlBusinessObject.SelectedValue = dt.Rows[0]["BUSINESSOBJECT"].ToString().Trim();
                    bindProblemGroupSelected();
                    ddlProblemGroup.SelectedValue = dt.Rows[0]["GROUPCODE"].ToString().Trim();
                    bindProblemTypeSelected();
                    ddlProblemType.SelectedValue = dt.Rows[0]["TYPECODE"].ToString().Trim();
                    bindProblemSourceSelected();
                    ddlProblemSource.SelectedValue = dt.Rows[0]["SOURCECODE"].ToString().Trim();
                    tbCode.Text = dt.Rows[0]["AREACODE"].ToString().Trim();
                    tbName.Text = dt.Rows[0]["AREANAME"].ToString().Trim();

                    try
                    {
                        ddlOwnerGroup.SelectedValue = dt.Rows[0]["OwnerGroupCode"].ToString();
                    }
                    catch (Exception)
                    {

                    }

                    ddlBusinessObject.Enabled = false;
                    ddlProblemGroup.Enabled = false;
                    ddlProblemType.Enabled = false;
                    ddlProblemSource.Enabled = false;
                    tbCode.Enabled = false;

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
                string businessObj = ddlBusinessObject.SelectedValue.Trim();
                string groupCode = ddlProblemGroup.SelectedValue.Trim();
                string typeCode = ddlProblemType.SelectedValue.Trim();
                string sourceCode = ddlProblemSource.SelectedValue.Trim();
                string code = tbCode.Text.Trim();
                string name = tbName.Text.Trim();
                string OwnerGroupCode = ddlOwnerGroup.SelectedValue.Trim();

                if (hdfMode.Value.Equals(ApplicationSession.CREATE_MODE_STRING))
                {
                    lib.CreateMasterConfigContact(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, businessObj, groupCode, typeCode, sourceCode, code, name
                        ,OwnerGroupCode, ERPWAuthentication.UserName);
                }
                else if (hdfMode.Value.Equals(ApplicationSession.CHANGE_MODE_STRING))
                {
                    lib.UpdateMasterConfigContact(ERPWAuthentication.SID, businessObj, groupCode, typeCode, sourceCode, code, name
                        ,OwnerGroupCode, ERPWAuthentication.UserName);
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
                string groupCode = (rpt.FindControl("hdfGroupCode") as HiddenField).Value;
                string typeCode = (rpt.FindControl("hdfTypeCode_") as HiddenField).Value;
                string sourceCode = (rpt.FindControl("hdfSourceCode_") as HiddenField).Value;
                string OwnerGroupCode = (rpt.FindControl("hdfOwnerCode") as HiddenField).Value;
                string code = btn.CommandArgument;

                lib.DeleteMasterConfigContact(SID,businessObj,groupCode,typeCode,sourceCode, code, OwnerGroupCode);

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
                bindProblemGroupSelected();
                bindProblemTypeSelected();
                bindProblemSourceSelected();
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

        protected void btnSetGroupType_Click(object sender, EventArgs e)
        {
            try
            {
                bindProblemTypeSelected();
                bindProblemSourceSelected();
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

        protected void btnSetGroupSource_Click(object sender, EventArgs e)
        {
            try
            {
                bindProblemSourceSelected();
                udpn.Update();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        private void bindBusinessObjectSelected()
        {
            List<MasterConfigEntity> listBusiness = lib.getBusinessObjectConfig(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            ddlBusinessObject.DataTextField = "xValue";
            ddlBusinessObject.DataValueField = "xKey";
            ddlBusinessObject.DataSource = listBusiness;
            ddlBusinessObject.DataBind();
            ddlBusinessObject.Items.Insert(0, new ListItem("Select", ""));
            if (listBusiness.Count == 1)
            {
                ddlBusinessObject.SelectedValue = listBusiness[0].xKey;
            }
        }

        private void bindProblemGroupSelected()
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(ddlBusinessObject.SelectedValue.Trim()))
            {
                dt = lib.GetMasterConfigGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ddlBusinessObject.SelectedValue.Trim(), "");
            }
            ddlProblemGroup.DataSource = dt;
            ddlProblemGroup.DataTextField = "GROUPNAME";
            ddlProblemGroup.DataValueField = "GROUPCODE";
            ddlProblemGroup.DataBind();
            ddlProblemGroup.Items.Insert(0, new ListItem("Select", ""));
            if (dt.Rows.Count == 1)
            {
                ddlProblemGroup.SelectedValue = dt.Rows[0]["GROUPCODE"].ToString();
            }
        }
        private void bindProblemTypeSelected()
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(ddlBusinessObject.SelectedValue) && !string.IsNullOrEmpty(ddlProblemGroup.SelectedValue))
            {
                dt = lib.GetMasterConfigProblem(ERPWAuthentication.SID
                    , ERPWAuthentication.CompanyCode
                    , ddlBusinessObject.SelectedValue.Trim()
                    , ddlProblemGroup.SelectedValue.Trim()
                    , "", ddlOwnerGroup.SelectedValue);
            }
            ddlProblemType.DataSource = dt;
            ddlProblemType.DataTextField = "TYPENAME";
            ddlProblemType.DataValueField = "TYPECODE";
            ddlProblemType.DataBind();
            ddlProblemType.Items.Insert(0, new ListItem("Select", ""));
            if (dt.Rows.Count == 1)
            {
                ddlProblemType.SelectedValue = dt.Rows[0]["TYPECODE"].ToString();
            }
        }

        private void bindProblemSourceSelected()
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(ddlBusinessObject.SelectedValue.Trim()) 
                && !string.IsNullOrEmpty(ddlProblemGroup.SelectedValue.Trim())
                && !string.IsNullOrEmpty(ddlProblemType.SelectedValue.Trim()))
            {
                dt = lib.GetMasterConfigSource(ERPWAuthentication.SID
                    ,ERPWAuthentication.CompanyCode
                    , ddlBusinessObject.SelectedValue.Trim()
                    , ddlProblemGroup.SelectedValue.Trim()
                    , ddlProblemType.SelectedValue.Trim()
                    ,"", ddlOwnerGroup.SelectedValue);
            }
            ddlProblemSource.DataTextField = "SOURCENAME";
            ddlProblemSource.DataValueField = "SOURCECODE";
            ddlProblemSource.DataSource = dt;
            ddlProblemSource.DataBind();
            ddlProblemSource.Items.Insert(0, new ListItem("Select", ""));
            if (dt.Rows.Count == 1)
            {
                ddlProblemSource.SelectedValue = dt.Rows[0]["SOURCECODE"].ToString();
            }
        }


        #endregion

        
    }
}