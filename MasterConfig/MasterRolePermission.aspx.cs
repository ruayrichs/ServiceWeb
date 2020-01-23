using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Master.Entity;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig
{
    public partial class MasterRolePermission : AbstractsSANWebpage//System.Web.UI.Page
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.RoleConfigView || ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.RoleConfigModify || ERPWAuthentication.Permission.AllPermission;
        }

        MasterConfigLibrary libMasConf = new MasterConfigLibrary();
        public bool IsFilterOwner
        {
            get
            {
                bool _IsFilterOwner = false;
                bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out _IsFilterOwner);
                return _IsFilterOwner;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                init();
            }
        }

        private bool init()
        {
            try
            {
                DataTable datatable = libMasConf.GetMasterRole(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
                itemsRPT.DataSource = datatable;
                itemsRPT.DataBind();
                udpTable.Update();

                ClientService.DoJavascript("afterLoad();");
                return true;
            }catch(Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
                return false;
            }
        }

        protected void update_btn_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;

                foreach (RepeaterItem item in itemsRPT.Items)
                {

                    ERPWRoleMaster roleMaster = new ERPWRoleMaster();
                    HiddenField txtEquipmentNo = item.FindControl("hddRoleCode") as HiddenField;
                    //string str = (txtEquipmentNo.Value).ToString();
                    roleMaster.rolecode = (txtEquipmentNo.Value).ToString();
                    roleMaster.allPerission = (item.FindControl("cbAllPermission") as CheckBox).Checked.ToString();
                    roleMaster.incidentvew = (item.FindControl("cbIncidentView") as CheckBox).Checked.ToString();
                    roleMaster.incidentmodify = (item.FindControl("cbIncidentModify") as CheckBox).Checked.ToString();
                    roleMaster.requestview = (item.FindControl("cbRequestView") as CheckBox).Checked.ToString();
                    roleMaster.requestmodify = (item.FindControl("cbRequestModify") as CheckBox).Checked.ToString();
                    roleMaster.problemview = (item.FindControl("cbProblemView") as CheckBox).Checked.ToString();
                    roleMaster.problemmodify = (item.FindControl("cbProblemModify") as CheckBox).Checked.ToString();
                    roleMaster.changeorderview = (item.FindControl("cbChangeOrderView") as CheckBox).Checked.ToString();
                    roleMaster.changeordermodify = (item.FindControl("cbChangeOrderModify") as CheckBox).Checked.ToString();
                    roleMaster.configuretionitemview = (item.FindControl("cbConfigurationItemView") as CheckBox).Checked.ToString();
                    roleMaster.configuretionitemmodify = (item.FindControl("cbConfigurationItemModify") as CheckBox).Checked.ToString();
                    roleMaster.contactview = (item.FindControl("cbContactView") as CheckBox).Checked.ToString();
                    roleMaster.contactmodify = (item.FindControl("cbContactModify") as CheckBox).Checked.ToString();
                    roleMaster.km_view = (item.FindControl("cbKM_View") as CheckBox).Checked.ToString();
                    roleMaster.km_modify = (item.FindControl("cbKM_Modify") as CheckBox).Checked.ToString();
                    roleMaster.reportview = (item.FindControl("cbReportView") as CheckBox).Checked.ToString();
                    roleMaster.reportmodify = (item.FindControl("cbReportModify") as CheckBox).Checked.ToString();
                    roleMaster.TierZeroView = (item.FindControl("cbTierZeroView") as CheckBox).Checked.ToString();
                    roleMaster.TierZeroModify = (item.FindControl("cbTierZeroModify") as CheckBox).Checked.ToString();
                    roleMaster.DashboardView = (item.FindControl("cbDashBoardView") as CheckBox).Checked.ToString();
                    roleMaster.DashboardModify = (item.FindControl("cbDashBoardModify") as CheckBox).Checked.ToString();
                    roleMaster.SLAView = (item.FindControl("cbSLAView") as CheckBox).Checked.ToString();
                    roleMaster.SLAModify = (item.FindControl("cbSLAModify") as CheckBox).Checked.ToString();
                    roleMaster.IncidentAreaView = (item.FindControl("cbIncidentAreaView") as CheckBox).Checked.ToString();
                    roleMaster.IncidentAreaModify = (item.FindControl("cbIncidentAreaModify") as CheckBox).Checked.ToString();
                    roleMaster.UserManagementView = (item.FindControl("cbUserManagementView") as CheckBox).Checked.ToString();
                    roleMaster.UserManagementModify = (item.FindControl("cbUserManagementModify") as CheckBox).Checked.ToString();
                    roleMaster.RoleConfigView = (item.FindControl("cbRoleConfigView") as CheckBox).Checked.ToString();
                    roleMaster.RoleConfigModify = (item.FindControl("cbRoleConfigModify") as CheckBox).Checked.ToString();
                    libMasConf.UpdateMasterRole(roleMaster);
                }
                ClientService.AGSuccess("ทำการอัพเดทเรียบร้อย");
                //dataBinding();
                
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
            finally
            {
                ClientService.AGLoading(false);
            }
            init();
        }
    }
}