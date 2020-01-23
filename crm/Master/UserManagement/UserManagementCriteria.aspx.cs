using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.crm.Master.UserManagement
{
    public partial class UserManagementCriteria : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission || ERPWAuthentication.Permission.UserManagementView;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission || ERPWAuthentication.Permission.UserManagementModify;
        }

        private UserManagementService userservice = UserManagementService.getInstance();


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
            try
            {
                if (!Page.IsPostBack)
                {
                    BindingData();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void BindingData()
        {
            List<UserManagementService.DataModel> listUserName = userservice.searchDataUserName(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, UserManagementService.SYSTEM_ERP, "");

            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                listUserName = listUserName.Where(w => w.OwnerService == OwnerGroupCode).ToList();
            }
            
            rptItems.DataSource = listUserName;
            rptItems.DataBind();
            udpnItems.Update();
            ClientService.DoJavascript("bindingDataTableJS();");
        }
        
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string idGen = Guid.NewGuid().ToString().Substring(0, 8);
                Session["EmailMessageCriteria_rowkey" + idGen] = "";
                Response.Redirect("UserManagementMaster.aspx?id=" + idGen);
            }
            catch (Exception ex)
            {
                //ClientService.AGError(ex.Message);
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string rowkey = (sender as Button).CommandArgument;
                string idGen = Guid.NewGuid().ToString().Substring(0, 8);
                Session["userid_" + idGen] = rowkey;
                Response.Redirect("UserManagementMaster.aspx?id=" + idGen);
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