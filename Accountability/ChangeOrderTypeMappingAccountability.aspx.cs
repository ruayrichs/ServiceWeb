using agape.lib.constant;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service.Workflow;
using ServiceWeb.Accountability.Service;
using ServiceWeb.auth;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.Accountability
{
    public partial class ChangeOrderTypeMappingAccountability : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }
        AccountabilityService accountabilityService = new AccountabilityService();
        DocTypeMapAccountabilityService docAccservice = new DocTypeMapAccountabilityService();
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
        private void initData()
        {
            DataTable dtDocMapAcc = docAccservice.GetDoccumentTypeMappingAccountabilityData(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            DataTable dtSCType = AfterSaleService.getInstance().getSearchDoctype("", ERPWAuthentication.CompanyCode, true, true);
            DataTable dtAccountability = accountabilityService.getAccountabilityStructureV2(ERPWAuthentication.SID, "");
            dtDocMapAcc.Columns.Add("DocDesc");
            dtDocMapAcc.Columns.Add("AccDesc");
            foreach (DataRow row in dtDocMapAcc.Rows)
            {

                DataRow[] rowDocType = dtSCType.Select("DocumentTypeCode = '" + row["DocTypeCode"].ToString() + "'");
                DataRow[] rowAccountability = dtAccountability.Select("DataValue = '" + row["AccountabilityCode"].ToString() + "'");
                if (rowDocType.Count() > 0 && rowAccountability.Count() > 0)
                {
                    row["DocDesc"] = rowDocType[0]["Description"].ToString();
                    row["AccDesc"] = rowAccountability[0]["DataText"].ToString();
                }

            }

            rptItems.DataSource = dtDocMapAcc;
            rptItems.DataBind();
            udpnItems.Update();
            ClientService.DoJavascript("bindingDataTableJS();");
        }

        protected void btnAddDocTypeMapAccountability_click(object sender, EventArgs e)
        {


        }

        private void SetAccountability()
        {
            DataTable dt = accountabilityService.getAccountabilityStructureV2(ERPWAuthentication.SID, "");

            ddlAccountability.DataSource = dt;
            ddlAccountability.DataTextField = "DataText";
            ddlAccountability.DataValueField = "DataValue";
            ddlAccountability.DataBind();
            ddlAccountability.Items.Insert(0, new ListItem("Please select", ""));
        }

        private void SetDocumentType()
        {

            DataTable dtSCType = AfterSaleService.getInstance().getSearchDoctype("", ERPWAuthentication.CompanyCode, true, true);

            ddlDocType.DataSource = dtSCType;
            ddlDocType.DataTextField = "Description";
            ddlDocType.DataValueField = "DocumentTypeCode";
            ddlDocType.DataBind();
            ddlDocType.Items.Insert(0, new ListItem("Please select", ""));
        }

        protected void btnDeleteDoctypeMapAccountability_click(object sender, EventArgs e)
        {
            try
            {
                string[] code = (sender as Button).CommandArgument.ToString().Split(',');
            docAccservice.DeleteDocumentTypeMappingAccountability(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, code[0], code[1]);

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
        protected void btnSetCreate_Click(object sender, EventArgs e)
        {
            try
            {
                hdfMode.Value = ApplicationSession.CREATE_MODE_STRING;
                SetAccountability();
                SetDocumentType();
                ddlDocType.SelectedValue = "";
                ddlAccountability.SelectedValue = "";
                

                ddlAccountability.Enabled = true;
                ddlDocType.Enabled = true;

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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string docTypeCode = ddlDocType.SelectedValue;
                string AccountabilityCode = ddlAccountability.SelectedValue;

                if (hdfMode.Value.ToString().Equals(ApplicationSession.CREATE_MODE_STRING))
                {
                    docAccservice.AddDoccumentTypeMappingAccountability(
                   ERPWAuthentication.SID,
                   ERPWAuthentication.CompanyCode,
                   docTypeCode,
                   AccountabilityCode,
                   ERPWAuthentication.UserName);
                }
                else if (hdfMode.Value.ToString().Equals(ApplicationSession.CHANGE_MODE_STRING))
                {
                    string[] code = hdfEditCode.Value.ToString().Split(',');
                   
                    docAccservice.EditDocumentTypeMappingAccountability(ERPWAuthentication.SID,
                                                                        ERPWAuthentication.CompanyCode,code[0],code[1],docTypeCode,AccountabilityCode);
                }

                initData();

                ClientService.DoJavascript("closeModal('Save');");
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().Contains("Violation of PRIMARY KEY")  && ex.Message.ToString().Contains("duplicate key") )
                {
                    ClientService.AGError("Duplicated Mapping");
                }
                else {
                    ClientService.AGError(ex.Message);
                }
               
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }


        protected void btnSetEdit_Click(object sender,EventArgs e) {
            try
            {
                hdfMode.Value = ApplicationSession.CHANGE_MODE_STRING;
                string[] code =  hdfEditCode.Value.ToString().Split(',');
                SetAccountability();
                SetDocumentType();
                ddlDocType.SelectedValue = code[0];
                ddlAccountability.SelectedValue = code[1];
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

    }
}