using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using Newtonsoft.Json;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.Report
{
    public partial class UserAnalyst : AbstractsSANWebpage
    {
        private EmployeeService empService = EmployeeService.GetInstance();

        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission || /*ERPWAuthentication.Permission.ReportView ||*/ ERPWAuthentication.Permission.ReportUserAnalyst;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.ReportModify || ERPWAuthentication.Permission.AllPermission;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadCustomerDocType();
            }
        }

        private void loadCustomerDocType()
        {
            DataTable dt = empService.searchEmployeeGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode,"","");

            foreach (DataRow dr in dt.Rows)
            {
                dr["Name"] = ObjectUtil.PrepareCodeAndDescription(
                    dr["Code"].ToString(),
                    dr["Name"].ToString()
                );
            }

            ddlEmployeeGroup.DataValueField = "Code";
            ddlEmployeeGroup.DataTextField = "Name";
            ddlEmployeeGroup.DataSource = dt;
            ddlEmployeeGroup.DataBind();
            ddlEmployeeGroup.Items.Insert(0, new ListItem("ทั้งหมด", ""));
        }

        private void loadCustomerList()
        {

            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);
            string OwnerGroupCode = "";
            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                OwnerGroupCode = ERPWAuthentication.Permission.OwnerGroupCode;
            }

            DataTable ListCustomer = empService.searchEngineEmployee(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                ddlEmployeeGroup.SelectedValue,
                txtEmployeeCodeName.Text,
                OwnerGroupCode
            );

            divJsonEmployeeList.InnerHtml = JsonConvert.SerializeObject(ListCustomer);
            //rptCustProfileModeTable.DataSource = dtCustomer;
            //rptCustProfileModeTable.DataBind();

            upPanelProfileList.Update();
            ClientService.DoJavascript("afterSearch();");
        }

        protected void btnSearchData_Click(object sender, EventArgs e)
        {
            try
            {
                loadCustomerList();
                ClientService.DoJavascript("scrollToTable();");
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

        protected void btnRedirectPage_Click(object sender, EventArgs e)
        {
            try
            {
                string guid = Guid.NewGuid().ToString().Substring(0,8);
                Session["UserAnalystDetail_" + guid] = hhdEmployeeCode.Value;
                Response.Redirect(Page.ResolveUrl("~/Report/UserAnalystDetail.aspx?id=" + guid));
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