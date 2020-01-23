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
    public partial class EquipmentClass : AbstractsSANWebpage
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
        private Service.EquipmentService eqservice = new Service.EquipmentService();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindingData();
                    setddlCIFamily();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void BindingData()
        {
            DataTable dt = lib.GetMasterConfigClass(ERPWAuthentication.SID);
            DataTable dtConfItemFamily = eqservice.getConfigurationItemFamily(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode
                );

            dt.Columns.Add("EquipmentGroupDesc");

            foreach (DataRow row in dt.Rows) {

                DataRow[] dtr = dtConfItemFamily.Select("MaterialGroupCode = '" + row["EquipmentGroup"].ToString()+"'");
                if (dtr.Count() > 0) {
                    row["EquipmentGroupDesc"] = dtr[0]["Description"].ToString();
                }

            }

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
                ddlCIFamily.ClearSelection();

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

                DataTable dt = lib.GetMasterConfigClass(ERPWAuthentication.SID, code);

                if (dt.Rows.Count > 0)
                {
                    hdfMode.Value = ApplicationSession.CHANGE_MODE_STRING;

                    tbCode.Text = dt.Rows[0]["ClassCode"].ToString();
                    tbName.Text = dt.Rows[0]["ClassName"].ToString();
                    ddlCIFamily.SelectedValue = dt.Rows[0]["EquipmentGroup"].ToString();
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
                string eqGroup = ddlCIFamily.SelectedValue;

                if (hdfMode.Value.Equals(ApplicationSession.CREATE_MODE_STRING))
                {
                    lib.CreateMasterConfigClass(ERPWAuthentication.SID, code, name,eqGroup, ERPWAuthentication.UserName);
                }
                else if (hdfMode.Value.Equals(ApplicationSession.CHANGE_MODE_STRING))
                {
                    lib.UpdateMasterConfigClass(ERPWAuthentication.SID, code, name,eqGroup, ERPWAuthentication.UserName);
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


                lib.DeleteMasterConfigClass(ERPWAuthentication.SID, code);

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


        private void setddlCIFamily() {
            
            DataTable dtConfItemFamily = eqservice.getConfigurationItemFamily(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode
                );

            ddlCIFamily.DataSource = dtConfItemFamily;
            ddlCIFamily.DataTextField = "Description";
            ddlCIFamily.DataValueField = "MaterialGroupCode";
            ddlCIFamily.DataBind();
            ddlCIFamily.Items.Insert(0,new ListItem("", ""));
        }
    }
}