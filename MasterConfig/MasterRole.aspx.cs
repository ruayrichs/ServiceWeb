using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Master.Entity;
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
    public partial class MasterRole : System.Web.UI.Page
    {
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
                string myScriptValue = "$('#tableSort').DataTable();";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myScriptName", myScriptValue, true);

                dataBinding();
            }           
        }

        private void dataBinding()
        {
            DataTable datatable = libMasConf.GetMasterRole(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            tableData.DataSource = datatable;
            tableData.DataBind();
            updateTable.Update();
            txBoxRoleCode.Text = "";
            txBoxRoleName.Text = "";
            drDownDefaultPage.Text = "";            
            updateNewRole.Update();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txBoxRoleName.Text.Trim().Length>0 && txBoxRoleCode.Text.Trim().Length > 0)
                {
                    ERPWRoleMaster roleMaster = new ERPWRoleMaster();
                    roleMaster.rolecode = txBoxRoleCode.Text;
                    roleMaster.rolename = txBoxRoleName.Text;                                     
                    roleMaster.allPerission = "False";
                    roleMaster.incidentvew = "False";
                    roleMaster.incidentmodify = "False";
                    roleMaster.requestview = "False";
                    roleMaster.requestmodify = "False";
                    roleMaster.problemview = "False";
                    roleMaster.problemmodify = "False";
                    roleMaster.changeorderview = "False";
                    roleMaster.changeordermodify = "False";
                    roleMaster.configuretionitemview = "False";
                    roleMaster.configuretionitemmodify = "False";
                    roleMaster.configuretionitemattributes = "False";
                    roleMaster.contactview = "False";
                    roleMaster.contactmodify = "False";
                    roleMaster.km_view = "False";
                    roleMaster.km_modify = "False";
                    roleMaster.reportview = "False";
                    roleMaster.reportmodify = "False";

                    roleMaster.reportExportData = "False";
                    roleMaster.reportUserAnalyst = "False";
                    roleMaster.reportCIMaintenance = "False";
                    roleMaster.reportReportClient = "False";
                    roleMaster.reportReportLocation = "False";
                    roleMaster.reportMTTN = "False";
                    roleMaster.reportMTTR = "False";
                    roleMaster.reportTicketReport = "False";
                    roleMaster.reportTicketAnalysis = "False";

                    roleMaster.TierZeroView = "False";
                    roleMaster.TierZeroModify = "False";
                    roleMaster.DashboardView = "False";
                    roleMaster.DashboardViewAll = "False";
                    roleMaster.DashboardModify = "False";
                    roleMaster.SLAView = "False";
                    roleMaster.SLAModify = "False";
                    roleMaster.IncidentAreaView = "False";
                    roleMaster.IncidentAreaModify = "False";
                    roleMaster.UserManagementView = "False";
                    roleMaster.UserManagementModify = "False";
                    roleMaster.RoleConfigView = "False";
                    roleMaster.RoleConfigModify = "False";
                    roleMaster.DefaultPage = drDownDefaultPage.SelectedItem.Value;
                    roleMaster.LevelControl = "0";
                    roleMaster.MyQueueView = "False";
                    roleMaster.MyGroupView = "False";
                    roleMaster.MyOverDueView = "False";
                    roleMaster.CalendarView = "False";
                    libMasConf.AddMasterRole(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, roleMaster);
                    ClientService.AGSuccess("เรียบร้อยแล้ว");
                    dataBinding();
                    
                    ClientService.DoJavascript(" $('#' + 'myModal').modal('hide');");
                }
                else
                {
                    ClientService.AGError("กรอกข้อมูลไม่ครบ");
                }
                
            }catch(Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
           
            try
            {
                Button btn = sender as Button;
                //ClientService.AGInfo(btn.CommandArgument);

                DataTable dt = new DataTable();
                dt.Columns.Add("TicketTypeCode");
                dt.Columns.Add("BusinessOject");

                foreach (RepeaterItem item in tableData.Items)
                {

                        // ClientService.AGInfo((item.FindControl("cbAllPermission") as CheckBox).Checked.ToString());
                        ERPWRoleMaster roleMaster = new ERPWRoleMaster();
                       // roleMaster.rolecode = (item.FindControl("btnRoleCode") as Button).CommandArgument;

                        HiddenField txtEquipmentNo = item.FindControl("hddRoleCode") as HiddenField;                        
                    string str = (txtEquipmentNo.Value).ToString();
                        roleMaster.rolecode = (txtEquipmentNo.Value).ToString();
                        roleMaster.allPerission = (item.FindControl("cbAllPermission") as CheckBox).Checked.ToString();
                        roleMaster.HomeView = (item.FindControl("cbHomeView") as CheckBox).Checked.ToString();
                        roleMaster.MyQueueView = (item.FindControl("cbMyQueueView") as CheckBox).Checked.ToString();
                        roleMaster.MyGroupView = (item.FindControl("cbMyGroupView") as CheckBox).Checked.ToString();
                        roleMaster.MyOverDueView = (item.FindControl("cbMyOverDueView") as CheckBox).Checked.ToString();
                        roleMaster.CalendarView = (item.FindControl("cbCalendarView") as CheckBox).Checked.ToString();
                        roleMaster.SearchView = (item.FindControl("cbSearchView") as CheckBox).Checked.ToString();
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
                    if ((item.FindControl("cbConfigurationItemModify") as CheckBox).Checked)
                    {
                        (item.FindControl("cbConfigurationItemAttributes") as CheckBox).Checked = true;
                    }
                    roleMaster.configuretionitemattributes = (item.FindControl("cbConfigurationItemAttributes") as CheckBox).Checked.ToString();
                    roleMaster.contactview = (item.FindControl("cbContactView") as CheckBox).Checked.ToString();
                        roleMaster.contactmodify = (item.FindControl("cbContactModify") as CheckBox).Checked.ToString();
                        roleMaster.km_view = (item.FindControl("cbKM_View") as CheckBox).Checked.ToString();
                        roleMaster.km_modify = (item.FindControl("cbKM_Modify") as CheckBox).Checked.ToString();
                        roleMaster.reportview = (item.FindControl("cbReportView") as CheckBox).Checked.ToString();
                        roleMaster.reportmodify = (item.FindControl("cbReportModify") as CheckBox).Checked.ToString();

                        roleMaster.reportExportData = (item.FindControl("cbReportExportData") as CheckBox).Checked.ToString();
                        roleMaster.reportUserAnalyst = (item.FindControl("cbReportUserAnalyst") as CheckBox).Checked.ToString();
                        roleMaster.reportCIMaintenance = (item.FindControl("cbReportCIMaintenance") as CheckBox).Checked.ToString();
                        roleMaster.reportReportClient = (item.FindControl("cbReportReportClient") as CheckBox).Checked.ToString();
                        roleMaster.reportReportLocation = (item.FindControl("cbReportReportLocation") as CheckBox).Checked.ToString();
                        roleMaster.reportMTTN = (item.FindControl("cbReportMTTN") as CheckBox).Checked.ToString();
                        roleMaster.reportMTTR = (item.FindControl("cbReportMTTR") as CheckBox).Checked.ToString();
                        roleMaster.reportTicketReport = (item.FindControl("cbReportTicketReport") as CheckBox).Checked.ToString();
                        roleMaster.reportTicketAnalysis = (item.FindControl("cbReportTicketAnalysis") as CheckBox).Checked.ToString();

                        roleMaster.TierZeroView = (item.FindControl("cbTierZeroView") as CheckBox).Checked.ToString();
                        roleMaster.TierZeroModify = (item.FindControl("cbTierZeroModify") as CheckBox).Checked.ToString();
                        roleMaster.DashboardView = (item.FindControl("cbDashBoardView") as CheckBox).Checked.ToString();
                        roleMaster.DashboardViewAll = (item.FindControl("cbDashBoardViewAll") as CheckBox).Checked.ToString();
                    roleMaster.DashboardModify = (item.FindControl("cbDashBoardModify") as CheckBox).Checked.ToString();
                    roleMaster.SLAView = (item.FindControl("cbSLAView") as CheckBox).Checked.ToString();
                    roleMaster.SLAModify = (item.FindControl("cbSLAModify") as CheckBox).Checked.ToString();
                    roleMaster.IncidentAreaView = (item.FindControl("cbIncidentAreaView") as CheckBox).Checked.ToString();
                    roleMaster.IncidentAreaModify = (item.FindControl("cbIncidentAreaModify") as CheckBox).Checked.ToString();
                    roleMaster.UserManagementView = (item.FindControl("cbUserManagementView") as CheckBox).Checked.ToString();
                    roleMaster.UserManagementModify = (item.FindControl("cbUserManagementModify") as CheckBox).Checked.ToString();
                    roleMaster.RoleConfigView = (item.FindControl("cbRoleConfigView") as CheckBox).Checked.ToString();
                    roleMaster.RoleConfigModify = (item.FindControl("cbRoleConfigModify") as CheckBox).Checked.ToString();
                    roleMaster.DefaultPage = (item.FindControl("ddDefaultPage") as DropDownList).SelectedValue;
                    roleMaster.LevelControl = (item.FindControl("ddLevelControl") as DropDownList).SelectedValue;
                    libMasConf.UpdateMasterRole(roleMaster);
                        
                       
                       

                }
                ClientService.AGSuccess("ทำการอัพเดทเรียบร้อย");
                dataBinding();
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
                libMasConf.DeleteMasterRole(btn.CommandArgument);
                ClientService.AGSuccess("ลบสำเร็จ");
                dataBinding();
            }
            catch(Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
            finally
            {
                ClientService.AGLoading(false);
            }
           
        }

        protected void tableData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            DropDownList ddDefaultPage = (DropDownList)e.Item.FindControl("ddDefaultPage");
            DropDownList ddLevelControl = (DropDownList)e.Item.FindControl("ddLevelControl");
            try
            {
                ddDefaultPage.SelectedValue = Convert.ToString(dr["DefaultPage"]);
                ddLevelControl.SelectedValue = Convert.ToString(dr["LevelControl"]);
            }
            catch (Exception)
            {

            }
            
        }
    }
}