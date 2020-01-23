using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using agape.lib.constant;

namespace ServiceWeb.MasterConfig
{
    public partial class EmployeeMappingRole : System.Web.UI.Page
    {
        private MasterConfigLibrary lib = new MasterConfigLibrary();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindingData();
                    Binddatarole();
                    BinddataOwnerGroup();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }


        private void BindingData()
        {
            DataTable db = lib.GetRoleMappingEmployee(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);

            rptItems.DataSource = db;
            rptItems.DataBind();

            updmaprole.Update();

            ClientService.DoJavascript("bindingDataTableJS();");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;

                string Employeecode = btn.CommandArgument;

                lib.DeleteMappingRole(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, Employeecode);

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

        protected void btnSetEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string employeecode = empcode.Value;
                string rcode = rolecode.Value;
                string OwnerCode = hddOwnerCode.Value;
                if (employeecode != "" && rcode != "")
                {
                    hdfMode.Value = ApplicationSession.CHANGE_MODE_STRING;
                    AutoCompleteEmployee.Enabled = false;
                    AutoCompleteEmployee.SelectedValueRefresh = employeecode;
                    ddl_role_code.SelectedValue = rcode;
                    ddlOwnerGroupService.SelectedValue = OwnerCode;
                    updmodal.Update();
                    ClientService.DoJavascript("openModal('Edit');");
                }
                else
                {
                    hdfMode.Value = ApplicationSession.CHANGE_MODE_STRING;
                    AutoCompleteEmployee.Enabled = false;
                    AutoCompleteEmployee.SelectedValueRefresh = employeecode;
                    ddl_role_code.SelectedValue = rcode;
                    ddlOwnerGroupService.SelectedValue = OwnerCode;
                    updmodal.Update();
                    ClientService.DoJavascript("openModal('Edit');");
                    //ClientService.AGMessage("กรุณากรอกข้อมูลให้ครบ");
                }
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string rcode = ddl_role_code.SelectedValue;
                string emcode = AutoCompleteEmployee.SelectedValue;
                if (string.IsNullOrEmpty(rcode))
                {
                    throw new Exception("Input Role please");
                }
                if (string.IsNullOrEmpty(emcode))
                {
                    throw new Exception("Input Employee please");
                }

                if (hdfMode.Value.Equals(ApplicationSession.CREATE_MODE_STRING))
                {
                    lib.InsertMappingCode(
                        ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, emcode, rcode,
                        ddlOwnerGroupService.SelectedValue, ERPWAuthentication.EmployeeCode
                    );
                }
                else if (hdfMode.Value.Equals(ApplicationSession.CHANGE_MODE_STRING))
                {
                    lib.UpdateMappingRole(
                        ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, rcode, emcode,
                        ddlOwnerGroupService.SelectedValue, ERPWAuthentication.EmployeeCode
                    );
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

        protected void btnSetCreate_Click(object sender, EventArgs e)
        {
            try
            {
                hdfMode.Value = ApplicationSession.CREATE_MODE_STRING;

                AutoCompleteEmployee.Enabled = true;
                AutoCompleteEmployee.SelectedValueRefresh = "";
                ddl_role_code.SelectedValue = "";
                ddlOwnerGroupService.SelectedValue = "";

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

        private void Binddatarole()
        {
            DataTable dt = lib.GetRoleCode(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            ddl_role_code.DataValueField = "RoleCode";
            ddl_role_code.DataTextField = "RoleName";

            ddl_role_code.DataSource = dt;
            ddl_role_code.DataBind();

            ddl_role_code.Items.Insert(0, new ListItem("Select", ""));
        }
        private void BinddataOwnerGroup()
        {
            DataTable dt = lib.GetMasterConfigOwnerGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "");
            ddlOwnerGroupService.DataValueField = "OwnerGroupCode";
            ddlOwnerGroupService.DataTextField = "OwnerGroupName";

            ddlOwnerGroupService.DataSource = dt;
            ddlOwnerGroupService.DataBind();

            ddlOwnerGroupService.Items.Insert(0, new ListItem("Select", ""));
        }
    }
}