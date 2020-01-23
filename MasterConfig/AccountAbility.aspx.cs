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
    public partial class AccountAbility : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }
        MasterConfigLibrary libMasConf = new MasterConfigLibrary();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dataBinding();
            }



        }

        private void dataBinding()
        {
            DataTable datatable = libMasConf.GetAccountAbility();
            tableData.DataSource = datatable;
            tableData.DataBind();
            udpMasterConfig.Update();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string strSEQ = ((tableData.Items[tableData.Items.Count - 1] as RepeaterItem)
                     .FindControl("txtSEQ") as Label).Text;
                libMasConf.DeleteAccountAbility(strSEQ);
                ClientService.AGSuccess("ลบสำเร็จ");
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
        protected void btnSave_Click(object sender, EventArgs e)
        {


            try
            {

               string strSEQ = ((tableData.Items[tableData.Items.Count - 1] as RepeaterItem)
                    .FindControl("txtSEQ") as Label).Text;

                if (txtBoxEventDesc.Value.Trim().Length > 0)
                {
                    libMasConf.AddAccountAbility(strSEQ, txtBoxEventDesc.Value.Trim());
                    ClientService.AGSuccess("บันทึกสำเร็จ");
                    txtBoxEventDesc.Value = "";
                    dataBinding();
                }
                else
                {
                    ClientService.AGError("กรุณาใส่ตัวอักษร");
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
    }
}