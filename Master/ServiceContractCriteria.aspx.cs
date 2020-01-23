using agape.lib.constant;
using Agape.FocusOne.Utilities;
using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceWeb.auth;

namespace ServiceWeb.Master
{
    public partial class ServiceContractCriteria : AbstractsSANWebpage
    {
        private ServiceContractLibrary lib = new ServiceContractLibrary();

        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        private string SessionID
        {
            get { return (string)Session[ApplicationSession.USER_SESSION_ID]; }           
        }

        private string SID
        {
            get { return ERPWAuthentication.SID; }
        }

        private string CompanyCode
        {
            get { return ERPWAuthentication.CompanyCode; }
        }

        private void AddBlankItem(DropDownList ddl)
        {
            ddl.Items.Insert(0, new ListItem("", ""));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    SetDropDownList();
                    DefaultCriteria();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void DefaultCriteria()
        {
            tbCompany.Text = CompanyCode + " : " + ERPWAuthentication.CompanyName;
            tbFiscalYear.Text = Validation.getCurrentServerStringDateTime().Substring(0, 4);
        }

        private void SetDropDownList()
        {            
            ddlDocumentType.DataSource = lib.GetDocumentType(SID, CompanyCode);
            ddlDocumentType.DataBind();            

            ddlStatus.DataSource = lib.GetDocumentStatus(SID);
            ddlStatus.DataBind();

            AddBlankItem(ddlStatus);
            AddBlankItem(ddlDocumentType);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = lib.GetServiceContractCriteria(SID, CompanyCode);

                rptItems.DataSource = dt;
                rptItems.DataBind();

                udpnItems.Update();

                ClientService.DoJavascript("afterSearch();");
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

        private void PrepareSessionAndRedirect(string mode, ServiceContractDataSet bean)
        {
            string pid = Guid.NewGuid().ToString().Substring(0, 8);

            Session[ServiceContractLibrary.SESSION_MODE + pid] = mode;
            Session[ServiceContractLibrary.SESSION_BEAN + pid] = bean;

            Response.Redirect("ServiceContract.aspx?pid=" + pid);
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceContractDataSet bean = lib.NewServiceContractBean(
                    SID, 
                    CompanyCode, 
                    hdfCustomerCode.Value.Trim(), 
                    ddlDocumentType.SelectedValue.Trim(), 
                    tbFiscalYear.Text.Trim(), 
                    ERPWAuthentication.UserName);

                PrepareSessionAndRedirect(ApplicationSession.CREATE_MODE_STRING, bean);
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        protected void btnEditServiceContact_Click(object sender, EventArgs e)
        {
            try
            {
                string[] key = hddKeyEdit.Value.Split('|');

                string documentType = key[0];
                string fiscalYear = key[1];
                string documentNo = key[2];
                string customerCode = key[3];

                ServiceContractDataSet bean = lib.GetServiceContractBean(SessionID, SID, CompanyCode, customerCode, documentNo, documentType, fiscalYear, ERPWAuthentication.UserName);

                PrepareSessionAndRedirect(ApplicationSession.CHANGE_MODE_STRING, bean);
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }
    }
}