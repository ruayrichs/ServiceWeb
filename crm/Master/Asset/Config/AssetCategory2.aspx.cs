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
    public partial class AssetCategory2 : AbstractsSANWebpage //System.Web.UI.Page
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
                if (Session["AssetCategory2_dtAssetGroup"] == null)
                {
                    Session["AssetCategory2_dtAssetGroup"] = assetService.getAssetGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "");
                }

                return (DataTable)Session["AssetCategory2_dtAssetGroup"];
            }
            set { Session["AssetCategory2_dtAssetGroup"] = value; }
        }

        protected DataTable dtAssetType
        {
            get
            {
                if (Session["AssetCategory2_dtAssetType"] == null)
                {
                    Session["AssetCategory2_dtAssetType"] = assetService.getAssetType(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "", "");
                }

                return (DataTable)Session["AssetCategory2_dtAssetType"];
            }
            set { Session["AssetCategory2_dtAssetType"] = value; }
        }

        protected DataTable dtAssetCategory1
        {
            get
            {
                if (Session["AssetCategory2_dtAssetCategory1"] == null)
                {
                    Session["AssetCategory2_dtAssetCategory1"] = assetService.getAssetCategory1(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "", "", "");
                }

                return (DataTable)Session["AssetCategory2_dtAssetCategory1"];
            }
            set { Session["AssetCategory2_dtAssetCategory1"] = value; }
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

            ddlAssetType.DataSource = dtAssetType;
            ddlAssetType.DataBind();
            ddlAssetType.Items.Insert(0, new ListItem("", ""));

            ddlAssetTypeModal.DataSource = dtAssetType;
            ddlAssetTypeModal.DataBind();
            ddlAssetTypeModal.Items.Insert(0, new ListItem("", ""));

            ddlAssetCategory1.DataSource = dtAssetCategory1;
            ddlAssetCategory1.DataBind();
            ddlAssetCategory1.Items.Insert(0, new ListItem("", ""));

            ddlAssetCategory1Modal.DataSource = dtAssetCategory1;
            ddlAssetCategory1Modal.DataBind();
            ddlAssetCategory1Modal.Items.Insert(0, new ListItem("", ""));

        }

        protected void getgvAssetCategory2()
        {
            // string sql = "select * from am_define_assetcategory2 where Companycode = '" + ERPWAuthentication.CompanyCode + "' and SID = '" + ERPWAuthentication.SID + "'";
            string sql = "";
            sql += " select a.* ,(b.Description) as  AssetCategoryName ,(c.GroupName) as AssetTypeName ,(d.Description )as AssetGroupName ";
            sql += " from am_define_assetcategory2 as a ";
            sql += " left outer join am_define_assetcategory1 as b ";
            sql += " on a.SID = b.SID and a.CompanyCode = b.CompanyCode and a.AssetCategory1 = b.AssetCategory ";
            sql += " left outer join am_define_assettype as c ";
            sql += " on a.SID = c.SID and a.CompanyCode = c.CompanyCode and a.AssetType = c.GroupCode ";
            sql += " left outer join am_define_assetgroup as d ";
            sql += " on a.SID = d.SID and a.CompanyCode = d.CompanyCode and a.AssetGroup = d.AssetGroup ";
            sql += "  where a.SID = '" + ERPWAuthentication.SID + "' and a.COMPANYCode ='" + ERPWAuthentication.CompanyCode + "' ";



            if (!String.IsNullOrEmpty(txtAssetCategory.Text))
            {
                sql += " and a.AssetCategory LIKE '%" + txtAssetCategory.Text + "%' ";
            }
            if (!String.IsNullOrEmpty(txtDescription.Text))
            {
                sql += " and a.Description LIKE '%" + txtDescription.Text + "%' ";
            }
            if (!String.IsNullOrEmpty(ddlAssetGroup.SelectedValue))
            {
                sql += " and a.AssetGroup = '" + ddlAssetGroup.SelectedValue + "' ";
            }
            if (!String.IsNullOrEmpty(ddlAssetType.SelectedValue))
            {
                sql += " and a.AssetType = '" + ddlAssetType.SelectedValue + "' ";
            }
            if (!String.IsNullOrEmpty(ddlAssetCategory1.SelectedValue))
            {
                sql += " and a.AssetCategory1 = '" + ddlAssetCategory1.SelectedValue + "' ";
            }


            if ((!String.IsNullOrEmpty(txtAssetCategoryModal.Text)) && (!String.IsNullOrEmpty(ddlAssetGroupModal.SelectedValue)) && (!String.IsNullOrEmpty(ddlAssetTypeModal.SelectedValue)) && (!String.IsNullOrEmpty(ddlAssetCategory1Modal.SelectedValue)))
            {
                sql += " and a.AssetCategory = '" + txtAssetCategoryModal.Text + "' and a.AssetGroup = '" + ddlAssetGroupModal.SelectedValue + "' ";
                sql += " and a.AssetType ='" + ddlAssetTypeModal.SelectedValue + "' and a.AssetCategory1 ='" + ddlAssetCategory1Modal.SelectedValue + "' ";
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
                dtAssetType = null;
                dtAssetCategory1 = null;
                bindingDropDownList();
                getgvAssetCategory2();
            }
        }

        protected void btnSreach_Click(object sender, EventArgs e)
        {
            getgvAssetCategory2();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            ClearItem();
            ddlAssetTypeModal.Enabled = true;
            ddlAssetGroupModal.Enabled = true;
            ddlAssetCategory1Modal.Enabled = true;
            btnCreateAssetCategory2.Visible = true;
            btnEditAssetCategory2.Visible = false;
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

            DataRow[] drr = DTGridView.Select("AssetCategory ='" + str[0] + "' and AssetGroup =  '" + str[1] + "' and AssetType ='" + str[2] + "' and AssetCategory1 = '" + str[3] + "' ");
            if (drr.Length > 0)
            {
                DataRow dr = drr[0];
                txtAssetCategoryModal.Text = Convert.ToString(dr["AssetCategory"]);
                txtAssetCategoryModal.Enabled = false;
                txtDescriptionModal.Text = Convert.ToString(dr["Description"]);
                ddlAssetGroupModal.SelectedValue = Convert.ToString(dr["AssetGroup"]);
                ddlAssetGroupModal.Enabled = false;
                ddlAssetTypeModal.SelectedValue = Convert.ToString(dr["AssetType"]);
                ddlAssetTypeModal.Enabled = false;
                ddlAssetCategory1Modal.SelectedValue = Convert.ToString(dr["AssetCategory1"]);
                ddlAssetCategory1Modal.Enabled = false;

            }
            btnCreateAssetCategory2.Visible = false;
            btnEditAssetCategory2.Visible = true;
            updatePanelModol.Update();
            ClientService.DoJavascript("$('#master-data').modal('show');");
        }


        protected void GridgvData_DeleteItem(object sender, EventArgs e)
        {
            string code = Convert.ToString(((LinkButton)sender).CommandName);
            string[] str = code.Split(',');
            string sql = "Delete FROM am_define_assetcategory2 ";
            sql += " where SID = '" + ERPWAuthentication.SID + "' and CompanyCode='" + ERPWAuthentication.CompanyCode + "' ";
            sql += " and AssetCategory ='" + str[0] + "' ";
            sql += " and AssetGroup = '" + str[1] + "'";
            sql += " and AssetType = '" + str[2] + "'";
            sql += " and AssetCategory1 = '" + str[3] + "'";
            db.selectDataFocusone(sql);

            ClientService.DoJavascript("AGSuccess('Change success.')");
            getgvAssetCategory2();
        }


        protected void btnEditAssetCategory2_Click(object sender, EventArgs e)
        {
            string sql = "Update am_define_assetcategory2 SET ";
            sql += " Description = '" + txtDescriptionModal.Text + "' ";
            sql += ", UPDATED_BY ='" + ERPWAuthentication.EmployeeCode + "' ";
            sql += ", UPDATED_ON = '" + Validation.getCurrentServerStringDateTime() + "' ";
            sql += " where SID = '" + ERPWAuthentication.SID + "' and CompanyCode='" + ERPWAuthentication.CompanyCode + "' ";
            sql += " and AssetCategory ='" + txtAssetCategoryModal.Text + "' ";
            sql += " and AssetGroup = '" + ddlAssetGroupModal.SelectedValue + "'";
            sql += " and AssetType = '" + ddlAssetTypeModal.SelectedValue + "'";
            sql += " and AssetCategory1 = '" + ddlAssetCategory1Modal.SelectedValue + "'";

            db.selectDataFocusone(sql);

            ClientService.DoJavascript("$('.modal').modal('hide');");
            ClientService.DoJavascript("AGSuccess('Change success.')");
            getgvAssetCategory2();


        }

        protected void btnCreateAssetCategory2_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtAssetCategoryModal.Text))
                {
                    throw new Exception("กรุณากรอก Asset Category.");
                }
                if (String.IsNullOrEmpty(ddlAssetGroupModal.SelectedValue))
                {
                    throw new Exception("กรุณาเลือก Asset Group.");
                }
                if (String.IsNullOrEmpty(ddlAssetTypeModal.SelectedValue))
                {
                    throw new Exception("กรุณาเลือก Asset Type.");
                }
                if (String.IsNullOrEmpty(ddlAssetCategory1Modal.SelectedValue))
                {
                    throw new Exception("กรุณาเลือก Asset Category 1.");
                }
                string sql = "Insert into am_define_assetcategory2 ";
                sql += " (SID,CompanyCode,AssetCategory,Description,CREATED_BY,CREATED_ON,AssetGroup,AssetType,AssetCategory1) ";
                sql += " Values ";
                sql += " ('" + ERPWAuthentication.SID + "'";
                sql += ", '" + ERPWAuthentication.CompanyCode + "'";
                sql += ", '" + txtAssetCategoryModal.Text + "'";
                sql += ", '" + txtDescriptionModal.Text + "'";
                sql += ", '" + ERPWAuthentication.EmployeeCode + "'";
                sql += ", '" + Validation.getCurrentServerStringDateTime() + "'";
                sql += ", '" + ddlAssetGroupModal.SelectedValue + "'";
                sql += ", '" + ddlAssetTypeModal.SelectedValue + "' ";
                sql += ", '" + ddlAssetCategory1Modal.SelectedValue + "')";

                db.selectDataFocusone(sql);

                ClientService.DoJavascript("$('.modal').modal('hide');");
                ClientService.DoJavascript("AGSuccess('Create success.')");
                getgvAssetCategory2();
            }
            catch (Exception ex)
            {

                ClientService.DoJavascript("AGError('" + ObjectUtil.Err(ex.Message) + "')");
            }
        }

        protected void ClearItem()
        {
            txtAssetCategoryModal.Text = "";
            txtAssetCategoryModal.Enabled = true;
            txtDescriptionModal.Text = "";
            txtDescriptionModal.Enabled = true;

            ddlAssetGroupModal.SelectedValue = "";
            ddlAssetGroupModal.SelectedIndex = 0;
            ddlAssetTypeModal.SelectedValue = "";
            ddlAssetTypeModal.SelectedIndex = 0;
            ddlAssetCategory1Modal.SelectedValue = "";
            ddlAssetCategory1Modal.SelectedIndex = 0;
        }
    }
}