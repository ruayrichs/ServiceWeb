using ServiceWeb.Accountability.MasterPage;
using ServiceWeb.Accountability.Service;
using ServiceWeb.auth;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Service.Workflow;
using ServiceWeb.MasterConfig.MasterPage;
using ERPW.Lib.Service;
using System.Web.Configuration;

namespace ServiceWeb.Accountability.Character
{
    public partial class MasterCharacter : AbstractsSANWebpage
    {

        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission || ERPWAuthentication.Permission.RoleConfigView;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission || ERPWAuthentication.Permission.RoleConfigModify;
        }

        public string WorkGroupCode
        {
            get
            {
                return (Master as AccountabilityMaster).WorkGroupCode;
            }
        }

        DataTable DTGridView
        {
            get
            {
                if (Session["Character_DTGridView"] == null)
                { Session["Character_DTGridView"] = new DataTable(); }
                return (DataTable)Session["Character_DTGridView"];
            }
            set { Session["Character_DTGridView"] = value; }
        }


        public bool FilterOwner
        {
            get
            {
                bool _FilterOwner = false;
                bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out _FilterOwner);
                return _FilterOwner;
            }
        }

        public string OwnerGroupCode
        {
            get
            {
                string _OwnerGroupCode = "";
                if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
                {
                    _OwnerGroupCode = ERPWAuthentication.Permission.OwnerGroupCode;
                }
                return _OwnerGroupCode;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            DTGridView = CharacterService.getInstance().getCharacterWeb(ERPWAuthentication.SID, WorkGroupCode);

            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                DTGridView.DefaultView.RowFilter = "HierarchyType = '" + OwnerGroupCode + "'";
                DTGridView = DTGridView.DefaultView.ToTable();
                DTGridView.DefaultView.RowFilter = string.Empty;
            }

            gvData.DataSource = DTGridView;
            gvData.DataBind();
            gvUpdatePanel.Update();
        }

        private void GetDataPopup()
        {
            DataTable dt = HierarchyService.getInstance().getHierarchyGroup();
            rptHierarchyGroup.DataSource = dt;
            rptHierarchyGroup.DataBind();
            updModalHierarchy.Update();
        }

        private void GetDataPopupMapping()
        {
            LastHierarchyCode(hddStructureCode.Value);
            DataTable dt = CharacterService.getInstance().getCharacterWithPerson(
                ERPWAuthentication.SID, 
                ERPWAuthentication.CompanyCode, 
                WorkGroupCode, 
                LastHierarchyCode(hddStructureCode.Value)
            );
            rptInitiativeOwner.DataSource = dt;
            rptInitiativeOwner.DataBind();
            udpTableOwner.Update();
        }

        protected void rptHierarchy_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rptHierarchyType = (Repeater)e.Item.FindControl("rptHierarchyType");
            HiddenField hddHierarchyGroupCode = (HiddenField)e.Item.FindControl("hddHierarchyGroupCode");

            DataTable dt = HierarchyService.getInstance().getHierarchyType(hddHierarchyGroupCode.Value);
            rptHierarchyType.DataSource = dt;
            rptHierarchyType.DataBind();
        }

        protected void showPopupClick(object sender, EventArgs e)
        {
            try
            {
                clearData();
                GetDataPopup();
                //btnCreateModal.Visible = true;
                //btnEditModal.Visible = false;

                //txtPopCharacterCode.Enabled = true;
                //txtPopCharacterDesc.Enabled = true;

                ClientService.DoJavascript("showModalMasterCharacter();");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AgroLoading(false);
            }
        }

        protected void LinkbtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                string code = btn.CommandArgument;
                CharacterService.getInstance().DeleteCharacter(ERPWAuthentication.SID, code, WorkGroupCode);
                GetData();
                ClientService.AGSuccess("ลบสำเร็จ");
            }
            catch (Exception ex)
            {

                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AgroLoading(false);
            }
        }

        protected void btnCreateModal_Click(object sender, EventArgs e)
        {
            try
            {
                //if (txtPopCharacterCode.Text != "" && txtPopCharacterDesc.Text != "")
                //{
                //    CharacterService.getInstance().AddCharacter(ERPWAuthentication.SID, txtPopCharacterCode.Text, txtPopCharacterDesc.Text, WorkGroupCode);
                //    GetData();
                //    clearData();
                //    ClientService.DoJavascript("hideModal();");
                //    ClientService.AGSuccess("สร้างสำเร็จ");
                //}
                //else
                //{
                //    ClientService.AGError("กรุณากรอกข้อมูลให้ครบ");
                //}
            }
            catch (Exception ex)
            {
                ClientService.AgroLoading(false);
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AgroLoading(false);
            }
        }

        protected void btnEditModal_Click(object sender, EventArgs e)
        {
            try
            {
                //if (txtPopCharacterDesc.Text != "")
                //{
                //    CharacterService.getInstance().EditCharacter(ERPWAuthentication.SID, txtPopCharacterCode.Text, txtPopCharacterDesc.Text, WorkGroupCode);
                //    GetData();
                //    clearData();
                //    ClientService.DoJavascript("hideModal();");
                //    ClientService.AGSuccess("บันทึกสำเร็จ");
                //}
                //else
                //{
                //    ClientService.AGError("กรุณากรอกข้อมูลให้ครบ");
                //}
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AgroLoading(false);
            }
        }

        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            gvData.DataSource = DTGridView;
            gvData.DataBind();
        }

        private void clearData()
        {
            //txtPopCharacterCode.Text = "";
            //txtPopCharacterDesc.Text = "";
        }

        protected void btnAddHierarchyType_Click(object sender, EventArgs e)
        {
            try
            {
                HiddenField hddHierarchyGroupCode2 = ((Button)sender).Parent.FindControl("hddHierarchyGroupCode2") as HiddenField;
                HiddenField hddHierarchyTypeCode = ((Button)sender).Parent.FindControl("hddHierarchyTypeCode") as HiddenField;
                Label lbHierarchyTypeName = ((Button)sender).Parent.FindControl("lbHierarchyTypeName") as Label;
                Guid Character = Guid.NewGuid();
                CharacterService.getInstance().AddCharacter(ERPWAuthentication.SID, Character.ToString(), WorkGroupCode, hddHierarchyGroupCode2.Value, hddHierarchyTypeCode.Value);
                GetData();
                ClientService.DoJavascript("hideModalMasterCharacter();");
                ClientService.AGSuccess("สร้างสำเร็จ");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AgroLoading(false);
            }
        }

        protected void btnBindData_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(hddStructureCode.Value))
                {
                    GetDataPopupMapping();
                    ClientService.DoJavascript("document.getElementById('panelFirstLoad').style.display = 'none';");
                    ClientService.DoJavascript("document.getElementById('panelContent').style.display = 'block';");
                }
                else
                {
                    ClientService.DoJavascript("document.getElementById('panelFirstLoad').style.display = 'block';");
                    ClientService.DoJavascript("document.getElementById('panelContent').style.display = 'none';");
                }

            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AgroLoading(false);
            }
        }

        private string LastHierarchyCode(string StructureCode)
        {
            string[] Structure = StructureCode.Split('|');
            int lastHierarchy = Structure.Length - 1;
            string HierarchyCode = Structure[lastHierarchy].ToString();

            return HierarchyCode;
        }

        protected void btnSaveEmpOwner_Click(object sender, EventArgs e)
        {
            try
            {
                CharacterService.getInstance().AddCharacterWithPerson(
                    ERPWAuthentication.SID, 
                    WorkGroupCode, 
                    SmartSearchMainDelegate.SelectedCode, 
                    LastHierarchyCode(hddStructureCode.Value), 
                    CharacterCode.Value,
                    chkMainDelegate.Checked
                );

                ReorganizationService.GetInstance().ReorganizationTicket(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode, 
                    hddStructureCode.Value,
                    SmartSearchMainDelegate.SelectedCode,
                    SmartSearchMainDelegate.SelectedValue,
                    ReorganizationService.Action_Type.Add,
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.FullNameEN
                );

                chkMainDelegate.Checked = false;
                SmartSearchMainDelegate.SelectedCode = "";
                SmartSearchMainDelegate.rebindSmartSearch();

                GetDataPopupMapping();
                ClientService.AGSuccess("บันทึกสำเร็จ");
                ClientService.DoJavascript("closeEmpOwner();");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.DoJavascript("$('#modalAddEmp').modal('hide');");
                ClientService.AGLoading(false);
            }
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                string[] code = (sender as Button).CommandArgument.Split(',');
                CharacterService.getInstance().DeleteCharacterWithPerson(code[0], code[1], code[2], code[3]);
                GetDataPopupMapping();


                ReorganizationService.GetInstance().ReorganizationTicket(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    hddStructureCode.Value,
                    code[2],
                    "",
                    ReorganizationService.Action_Type.Delete,
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.FullNameEN
                );


                ClientService.AGSuccess("ลบสำเร็จ");
                ClientService.DoJavascript("closeEmpOwner();");
                //accountabilityService.DeleteInitiativeOwner(ERPWAuthentication.SID,
                //    ERPWAuthentication.CompanyCode,
                //    code[0],
                //    code[1],
                //    WorkGroupCode);
                //
                //bindDataInitiativeOwner();
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

        protected void chkMainDelegate_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (sender as CheckBox);
                CharacterService.getInstance().setMainDelegate(
                    ERPWAuthentication.SID,
                    WorkGroupCode,
                    chk.CssClass,
                    LastHierarchyCode(hddStructureCode.Value),
                    CharacterCode.Value
                );

                GetDataPopupMapping();
                ClientService.AGSuccess("บันทึกสำเร็จ");
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

        protected void chkAuthenTran_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (sender as CheckBox);
                string EmpCode = chk.CssClass;

                CharacterService.getInstance().updateAuthenticationTransferTask(
                    ERPWAuthentication.SID,
                    EmpCode,
                    LastHierarchyCode(hddStructureCode.Value),
                    CharacterCode.Value,
                    chk.Checked
                );

                GetDataPopupMapping();
                ClientService.AGSuccess("บันทึกสำเร็จ");
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

        protected void chkAuthenClose_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (sender as CheckBox);
                string EmpCode = chk.CssClass;

                CharacterService.getInstance().updateAuthenticationCloseTask(
                    ERPWAuthentication.SID,
                    EmpCode,
                    LastHierarchyCode(hddStructureCode.Value),
                    CharacterCode.Value,
                    chk.Checked
                );

                GetDataPopupMapping();
                ClientService.AGSuccess("บันทึกสำเร็จ");
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