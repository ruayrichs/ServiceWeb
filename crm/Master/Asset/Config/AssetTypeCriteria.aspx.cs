using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ServiceWeb.auth;
using ERPW.Lib.Authentication;
using SNA.Lib.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.crm.Master.Asset.Config
{
    public partial class AssetTypeCriteria : AbstractsSANWebpage //System.Web.UI.Page
    {

        #region Service
        DBService db = new DBService();
        AssetService assetService = new AssetService();

        #endregion

        #region DataTable

        protected DataTable DTGridView
        {
            get
            {
                if (Session["AssetTypeCriteria.DTGridView"] == null)
                {
                    Session["AssetTypeCriteria.DTGridView"] = new DataTable();
                }
                return (DataTable)Session["AssetTypeCriteria.DTGridView"];
            }
            set { Session["AssetTypeCriteria.DTGridView"] = value; }
        }
        #endregion

        #region DataTable DropDown

        protected DataTable dtAssetGroup
        {
            get
            {
                if (Session["AssetTypeCriteria_dtAssetGroup"] == null)
                {
                    Session["AssetTypeCriteria_dtAssetGroup"] = assetService.getAssetGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "");
                }

                return (DataTable)Session["AssetTypeCriteria_dtAssetGroup"];
            }
            set { Session["AssetTypeCriteria_dtAssetGroup"] = value; }
        }


        #endregion

        private void bindingDropDownList()
        {
            ddlAssetGroup.DataSource = dtAssetGroup;
            ddlAssetGroup.DataBind();
            ddlAssetGroup.Items.Insert(0, new ListItem("", ""));

            ddlAssetGroupModal.DataSource = dtAssetGroup;
            ddlAssetGroupModal.DataBind();
            ddlAssetGroupModal.Items.Insert(0, new ListItem("", ""));
        }

        protected void getgvAssetType()
        {
            // string sql = "select * from am_define_assettype where Companycode = '" + ERPWAuthentication.CompanyCode + "' and SID = '" + ERPWAuthentication.SID + "'";
            string sql = "";
            sql += " select a.* ,(d.Description )as AssetGroupName ";
            sql += " from am_define_assettype as a ";
            sql += " left outer join am_define_assetgroup as d ";
            sql += " on a.SID = d.SID and a.CompanyCode = d.CompanyCode and a.AssetGroup = d.AssetGroup ";
            sql += "  where a.SID = '" + ERPWAuthentication.SID + "'  and a.COMPANYCode ='" + ERPWAuthentication.CompanyCode + "' ";


            if (!String.IsNullOrEmpty(txtGroupCodeCode.Text))
            {
                sql += " and a.GroupCode LIKE '%" + txtGroupCodeCode.Text + "%' ";
            }
            if (!String.IsNullOrEmpty(txtGroupCodeName.Text))
            {
                sql += " and a.GroupName LIKE '%" + txtGroupCodeName.Text + "%' ";
            }
            if (!String.IsNullOrEmpty(ddlAssetGroup.SelectedValue))
            {
                sql += " and a.AssetGroup = '" + ddlAssetGroup.SelectedValue + "' ";
            }

            if ((!String.IsNullOrEmpty(txtGroupCode.Text)) && (!String.IsNullOrEmpty(ddlAssetGroupModal.SelectedValue)))
            {
                sql += " and a.GroupCode = '" + txtGroupCode.Text + "' and a.AssetGroup = '" + ddlAssetGroupModal.SelectedValue + "' ";
            }
            DTGridView = db.selectDataFocusone(sql);

            gvData.DataSource = DTGridView;
            gvData.DataBind();
            updatePanel.Update();
            ClearItem();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtAssetGroup = null;
                bindingDropDownList();
                getgvAssetType();
            }
        }

        protected void btnSreach_Click(object sender, EventArgs e)
        {
            getgvAssetType();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            ClearItem();
            txtAssetGroupModal.Visible = false;
            ddlAssetGroupModal.Visible = true;
            btnCreateAssetType.Visible = true;
            btnEditAssetType.Visible = false;
            updatePanelModol.Update();
            ClientService.DoJavascript("$('#master-data').modal('show');");
        }

        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvData.PageIndex = e.NewPageIndex;
                gvData.DataSource = DTGridView;
                gvData.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), "msgbox", "alertMessage('" + ObjectUtil.Err(ex.Message) + "');", true);
            }
        }

        protected void GridgvData_EditItem(object sender, EventArgs e)
        {
            string code = Convert.ToString(((LinkButton)sender).CommandName);
            string[] str = code.Split(',');

            DataRow[] drrP = DTGridView.Select("GroupCode ='" + str[0] + "' and AssetGroup =  '" + str[1] + "' ");
            if (drrP.Length > 0)
            {
                DataRow drP = drrP[0];
                txtGroupCode.Text = Convert.ToString(drP["GroupCode"]);
                txtGroupCode.Enabled = false;
                txtGroupName.Text = Convert.ToString(drP["GroupName"]);
                txtAssetGroupModal.Text = Convert.ToString(drP["AssetGroup"]);
                txtAssetGroupModal.Enabled = false;
            }
            txtAssetGroupModal.Visible = true;
            ddlAssetGroupModal.Visible = false;
            btnCreateAssetType.Visible = false;
            btnEditAssetType.Visible = true;
            updatePanelModol.Update();
            ClientService.DoJavascript("$('#master-data').modal('show');");
        }


        protected void GridgvData_DeleteItem(object sender, EventArgs e)
        {
            string code = Convert.ToString(((LinkButton)sender).CommandName);
            string[] str = code.Split(',');
            string sql = "Delete FROM am_define_assettype ";
            sql += " where SID = '" + ERPWAuthentication.SID + "' and CompanyCode='" + ERPWAuthentication.CompanyCode + "' ";
            sql += " and GroupCode ='" + str[0] + "' ";
            sql += " and AssetGroup = '" + str[1] + "'";
            db.selectDataFocusone(sql);

            ClientService.DoJavascript("AGSuccess('Change success.')");
            getgvAssetType();
        }


        protected void btnEditAssetType_Click(object sender, EventArgs e)
        {
            string sql = "Update am_define_assettype SET ";
            sql += " GroupName = '" + txtGroupName.Text + "' ";
            sql += ", UPDATED_BY ='" + ERPWAuthentication.EmployeeCode + "' ";
            sql += ", UPDATED_ON = '" + Validation.getCurrentServerStringDateTime() + "' ";
            sql += " where SID = '" + ERPWAuthentication.SID + "' and CompanyCode='" + ERPWAuthentication.CompanyCode + "' ";
            sql += " and GroupCode ='" + txtGroupCode.Text + "' ";
            sql += " and AssetGroup = '" + txtAssetGroupModal.Text + "'";

            db.selectDataFocusone(sql);

            ClientService.DoJavascript("$('.modal').modal('hide');");
            ClientService.DoJavascript("AGSuccess('Change success.')");
            getgvAssetType();


        }

        protected void btnCreateAssetType_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtGroupCode.Text))
                {
                    throw new Exception("กรุณากรอก Group Code.");
                }
                if (String.IsNullOrEmpty(ddlAssetGroupModal.SelectedValue))
                {
                    throw new Exception("กรุณาเลือก Asset Group.");
                }

                string sql = "Insert into am_define_assettype ";
                sql += " (SID,CompanyCode,GroupCode,GroupName,CREATED_BY,CREATED_ON,AssetGroup) ";
                sql += " Values ";
                sql += " ('" + ERPWAuthentication.SID + "'";
                sql += ", '" + ERPWAuthentication.CompanyCode + "'";
                sql += ", '" + txtGroupCode.Text + "'";
                sql += ", '" + txtGroupName.Text + "'";
                sql += ", '" + ERPWAuthentication.EmployeeCode + "'";
                sql += ", '" + Validation.getCurrentServerStringDateTime() + "'";
                sql += ", '" + ddlAssetGroupModal.SelectedValue + "')";
                db.selectDataFocusone(sql);

                ClientService.DoJavascript("$('.modal').modal('hide');");
                ClientService.DoJavascript("AGSuccess('Create success.')");
                getgvAssetType();
            }
            catch (Exception ex)
            {

                ClientService.DoJavascript("AGError('" + ObjectUtil.Err(ex.Message) + "')");
            }
        }

        protected void ClearItem()
        {
            txtGroupCode.Text = "";
            txtGroupCode.Enabled = true;
            txtGroupName.Text = "";
            txtGroupName.Enabled = true;

            ddlAssetGroupModal.SelectedIndex = 0;
        }
    }
}