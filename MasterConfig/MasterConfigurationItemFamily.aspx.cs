using ERPW.Lib.Authentication;
using ServiceWeb.auth;
using agape.lib.constant;
using ERPW.Lib.Master.Config;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ServiceWeb.MasterConfig
{
    public partial class MasterConfigurationItemFamily : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

       Service.EquipmentService eqservice =  new Service.EquipmentService();

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!Page.IsPostBack)
                {
                    initData();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void initData() {
            DataTable dtConfItemFamily =  eqservice.getConfigurationItemFamily(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode
                );

            rptItems.DataSource = dtConfItemFamily;
            rptItems.DataBind();
            udpnItems.Update();
            ClientService.DoJavascript("bindingDataTableJS();");

        }

        protected void btnSetCreate_Click(object sender ,EventArgs e) {
            try { 
                hdfMode.Value = ApplicationSession.CREATE_MODE_STRING;
                txtMaterialGroup.Text = "";
                txtPrefix.Text = "";
                txtDesc.Text = "";
                txtStart.Text = "";
                txtEnd.Text = "";
                chbExternalOrNot.Checked = false;
                chbFreeDeline.Checked = false;
                chbExternalOrNot.Enabled = true;
                chbFreeDeline.Enabled = false;
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

        protected void btnSave_Click(object sender ,EventArgs e) {
            try
            {

                string materialGroup = txtMaterialGroup.Text;
                string desc = txtDesc.Text;
                string prefix = txtPrefix.Text; 
                string strat = txtStart.Text;
                string end = txtEnd.Text;
                bool externalOrNot = chbExternalOrNot.Checked;
                bool freeDeline = chbFreeDeline.Checked;
                if (strat.Length != end.Length) {
                    throw new Exception("Start และ End จำนวนตัวอักษรต้องเท่ากัน");
                }
                string sessionid = (string)Session[ApplicationSession.USER_SESSION_ID];
                if (hdfMode.Value.Equals(ApplicationSession.CREATE_MODE_STRING))
                {
                    
                    eqservice.AddConfigurationItemFamily(ERPWAuthentication.SID,
                                                        ERPWAuthentication.CompanyCode,
                                                        ERPWAuthentication.EmployeeCode,
                                                        sessionid,materialGroup,desc,prefix,strat,end, externalOrNot,freeDeline);
                }
                else if (hdfMode.Value.Equals(ApplicationSession.CHANGE_MODE_STRING))
                {
                    eqservice.UpdateConfigurationItemFamily(ERPWAuthentication.SID,
                                                        ERPWAuthentication.CompanyCode,
                                                        ERPWAuthentication.EmployeeCode,
                                                        sessionid, materialGroup, desc, prefix, strat, end, externalOrNot, freeDeline);   
                }

                initData();

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

        protected void btnSetEdit_Click(object sender ,EventArgs e) {
            try
            {
                hdfMode.Value = ApplicationSession.CHANGE_MODE_STRING;
                string[] data = hdfEditCode.Value.ToString().Split('|');
                txtMaterialGroup.Text = data[0];
                txtDesc.Text = data[1];
                txtPrefix.Text = data[2];              
                txtStart.Text = data[3];
                txtEnd.Text = data[4];
                chbExternalOrNot.Checked = bool.Parse(data[5]);
                chbFreeDeline.Checked = bool.Parse(data[6]);
                
                if (bool.Parse(data[5]))
                {

                    chbFreeDeline.Enabled = true;
                }
                else {
                    chbFreeDeline.Enabled = false;
                }

                ClientService.DoJavascript("openModal('Edit');");
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


        protected void btnDeleteCongigurationItemFamily_Click(object sender,EventArgs e) {
            try { 
                string materialGroup = (sender as Button).CommandArgument.ToString();
                string sessionid = (string)Session[ApplicationSession.USER_SESSION_ID];
                eqservice.RemoveConfigurationItemFamily(ERPWAuthentication.SID,
                                                        ERPWAuthentication.CompanyCode,
                                                        sessionid,
                                                        materialGroup);
                initData();
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
        protected void Check_Clicked(object sender ,EventArgs e) {

            if (chbExternalOrNot.Checked == true)
            {
                chbFreeDeline.Enabled = true;
            }
            else {

                chbFreeDeline.Checked = false;
                chbFreeDeline.Enabled = false;
            }

        }
    }
}