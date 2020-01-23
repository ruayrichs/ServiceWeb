using ERPW.Lib.Service.Workflow;
using ServiceWeb.Accountability.MasterPage;
using ServiceWeb.Accountability.Service;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Authentication;
using ServiceWeb.MasterConfig.MasterPage;

namespace ServiceWeb.Accountability.Hierarchy
{
    public partial class HierarchyGroupList : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        public string WorkGroupCode
        {
            get
            {
                return (Master as AccountabilityMaster).WorkGroupCode;
            }
        }


        HierarchyService hierarchyservice = HierarchyService.getInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                try
                {
                    bindHierarchyGroup();
                }
                catch (Exception ex)
                {
                    ClientService.AGError(ex.Message);
                }
            }
        }

        private void bindHierarchyGroup()
        {
            DataTable dt = hierarchyservice.getHierarchyGroup();

            rptHierarchy.DataSource = dt;
            rptHierarchy.DataBind();
            udpList.Update();
        }

        protected void btnAddGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCode.Text) && !string.IsNullOrEmpty(txtName.Text))
                {
                    bool chackCode = hierarchyservice.chackCodeHierarchyGroup(txtCode.Text);

                    if (chackCode)
                    {
                        hierarchyservice.InsertHierarchyGroup(txtCode.Text, txtName.Text);
                        txtCode.Text = "";
                        txtName.Text = "";
                        udpAddGroup.Update();
                        bindHierarchyGroup();
                        ClientService.DoJavascript("$('#ModalAddHierarchyGroup').modal('hide')");
                        ClientService.AGSuccess("บันทึกสำเร็จ");
                    }
                    else
                    {
                        ClientService.AGMessage("Hierarchy group code<br /> \"" + txtCode.Text + "\" <br />มีอยู่แล้ว กรุณากรอกใหม่");
                    }
                }
                else
                {
                    ClientService.AGMessage("กรุณากรอกข้อมูลให้ครบถ้วน");
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

        protected void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            try
            {
                string code = (sender as Button).CommandArgument;
                hierarchyservice.DeleteHierarchyGroup(code);
                bindHierarchyGroup();
                ClientService.AGSuccess("ลบสำเร็จ");
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