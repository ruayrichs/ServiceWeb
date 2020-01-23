using agape.lib.constant;
using agape.proxy.data.dataset;
using Agape.FocusOne.Utilities;
using ServiceWeb.auth;
using ERPW.Lib.Authentication;
using SNA.Lib.Transaction;
using SNA.Lib.Transaction.ICMUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.crm.Master.Asset
{
    public partial class AssetMasterCriteria : AbstractsSANWebpage //System.Web.UI.Page
    {
        private AssetService assetService = new AssetService();
        private ICMUtils icmUtils = WSHelper.getICMUtils();
        protected DataTable DTGridView
        {
            get
            {
                if (Session["AssetMasterCriteria_DTGridView"] == null)
                {
                    Session["AssetMasterCriteria_DTGridView"] = new DataTable();
                }
                return (DataTable)Session["AssetMasterCriteria_DTGridView"];
            }
            set { Session["AssetMasterCriteria_DTGridView"] = value; }
        }
        protected MasterAsset beanAsset
        {
            get { return (MasterAsset)Session["AssetMaster_beanAsset"]; }
            set { Session["AssetMaster_beanAsset"] = value; }
        }
        protected String AssetMode
        {
            get
            {
                if (Session["AssetMaster_AssetMode"] == null)
                {
                    Session["AssetMaster_AssetMode"] = ApplicationSession.CREATE_MODE_STRING;
                }
                return (String)Session["AssetMaster_AssetMode"];
            }
            set { Session["AssetMaster_AssetMode"] = value; }
        }
        protected DataTable dtDelegate
        {
            get { return (DataTable)Session["AssetMasterDetail_dtDelegate"]; }
            set { Session["AssetMasterDetail_dtDelegate"] = value; }
        }

        #region DataTable DropDown
        protected DataTable dtAssetGroup
        {
            get
            {
                if (Session["AssetMaster_dtAssetGroup"] == null)
                {
                    Session["AssetMaster_dtAssetGroup"] = assetService.getAssetGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "");
                }

                return (DataTable)Session["AssetMaster_dtAssetGroup"];
            }
            set { Session["AssetMaster_dtAssetGroup"] = value; }
        }
        protected DataTable dtAssetType
        {
            get
            {
                if (Session["AssetMaster_dtAssetType"] == null)
                {
                    Session["AssetMaster_dtAssetType"] = assetService.getAssetType(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "", "");
                }

                return (DataTable)Session["AssetMaster_dtAssetType"];
            }
            set { Session["AssetMaster_dtAssetType"] = value; }
        }
        protected DataTable dtAssetCategory1
        {
            get
            {
                if (Session["AssetMaster_dtAssetCategory1"] == null)
                {
                    Session["AssetMaster_dtAssetCategory1"] = assetService.getAssetCategory1(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "", "", "");
                }

                return (DataTable)Session["AssetMaster_dtAssetCategory1"];
            }
            set { Session["AssetMaster_dtAssetCategory1"] = value; }
        }
        protected DataTable dtAssetCategory2
        {
            get
            {
                if (Session["AssetMaster_dtAssetCategory2"] == null)
                {
                    Session["AssetMaster_dtAssetCategory2"] = assetService.getAssetCategory2(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "", "", "", "");
                }

                return (DataTable)Session["AssetMaster_dtAssetCategory2"];
            }
            set { Session["AssetMaster_dtAssetCategory2"] = value; }
        }
        protected DataTable dtAssetLocation1
        {
            get
            {
                if (Session["AssetMaster_dtAssetLocation1"] == null)
                {
                    Session["AssetMaster_dtAssetLocation1"] = assetService.getAssetLocation1(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "");
                }

                return (DataTable)Session["AssetMaster_dtAssetLocation1"];
            }
            set { Session["AssetMaster_dtAssetLocation1"] = value; }
        }
        protected DataTable dtAssetLocation2
        {
            get
            {
                if (Session["AssetMaster_dtAssetLocation2"] == null)
                {
                    Session["AssetMaster_dtAssetLocation2"] = assetService.getAssetLocation2(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "", "");
                }

                return (DataTable)Session["AssetMaster_dtAssetLocation2"];
            }
            set { Session["AssetMaster_dtAssetLocation2"] = value; }
        }
        protected DataTable dtAssetRoom
        {
            get
            {
                if (Session["AssetMaster_dtAssetRoom"] == null)
                {
                    Session["AssetMaster_dtAssetRoom"] = assetService.getAssetRoom(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "", "", "");
                }

                return (DataTable)Session["AssetMaster_dtAssetRoom"];
            }
            set { Session["AssetMaster_dtAssetRoom"] = value; }
        }
        #endregion

        protected void getDataAsset()
        {
            DTGridView = assetService.GetAssetMaster(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode
                , ddlAssetGroup.SelectedValue, ddlBranch.SelectedValue, ddlAssetType.SelectedValue, ddlAssetCategory1.SelectedValue
                , txtAssetCode.Text, ddlAssetCategory2.SelectedValue, txtAssetName.Text, ddlLocation1.SelectedValue
                , ddlOwner.SelectedValue, ddlLocation2.SelectedValue, ddlDepartment.SelectedValue, ddlRoom.SelectedValue, ddlStatus.SelectedValue);

            gvAssetMaster.DataSource = DTGridView;
            gvAssetMaster.DataBind();
            udpnItems.Update();
            ClientService.DoJavascript("afterSearch();");
            //updatePanelgvAssetMaster.Update();
        }

        private void bindingDropDownList()
        {
            dtAssetGroup = null;
            dtAssetType = null;
            dtAssetCategory1 = null;
            dtAssetCategory2 = null;
            dtAssetLocation1 = null;
            dtAssetLocation2 = null;
            dtAssetRoom = null;

            DataTable dtBranch = assetService.getBUArea(ERPWAuthentication.SID);
            ddlBranch.DataSource = dtBranch;
            ddlBranch.DataBind();
            ddlBranch.Items.Insert(0, new ListItem("", ""));

            DataTable dtDepartment = assetService.getDepartment(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            ddlDepartment.DataSource = dtDepartment;
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new ListItem("", ""));

            ddlAssetGroup.DataSource = dtAssetGroup;
            ddlAssetGroup.DataBind();
            ddlAssetGroup.Items.Insert(0, new ListItem("", ""));

            ddlAssetType.DataSource = dtAssetType;
            ddlAssetType.DataBind();
            ddlAssetType.Items.Insert(0, new ListItem("", ""));

            ddlAssetCategory1.DataSource = dtAssetCategory1;
            ddlAssetCategory1.DataBind();
            ddlAssetCategory1.Items.Insert(0, new ListItem("", ""));

            ddlAssetCategory2.DataSource = dtAssetCategory2;
            ddlAssetCategory2.DataBind();
            ddlAssetCategory2.Items.Insert(0, new ListItem("", ""));

            ddlLocation1.DataSource = dtAssetLocation1;
            ddlLocation1.DataBind();
            ddlLocation1.Items.Insert(0, new ListItem("", ""));

            ddlLocation2.DataSource = dtAssetLocation2;
            ddlLocation2.DataBind();
            ddlLocation2.Items.Insert(0, new ListItem("", ""));

            ddlRoom.DataSource = dtAssetRoom;
            ddlRoom.DataBind();
            ddlRoom.Items.Insert(0, new ListItem("", ""));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    bindingDropDownList();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                getDataAsset();
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

        private void initialNewBean()
        {
            beanAsset = new MasterAsset();

            DataRow dr;

            dr = beanAsset.am_master_asset_header.NewRow();
            dr["SID"] = ERPWAuthentication.SID;
            dr["CompanyCode"] = ERPWAuthentication.CompanyCode;
            dr["AssetCode"] = "";
            dr["BranchCode"] = ddlBranch.SelectedValue;
            dr["SubCodeQty"] = 1;
            dr["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
            dr["UPDATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
            beanAsset.am_master_asset_header.Rows.Add(dr);

            dr = beanAsset.am_master_asset_subcode.NewRow();
            dr["SID"] = ERPWAuthentication.SID;
            dr["CompanyCode"] = ERPWAuthentication.CompanyCode;
            dr["AssetCode"] = "";
            dr["AssetSubCode"] = "00";
            dr["AssetSubCodeDescription"] = txtAssetName.Text;
            dr["Quantity"] = 1;
            dr["Uom"] = "EA";
            dr["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
            dr["UPDATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
            beanAsset.am_master_asset_subcode.Rows.Add(dr);

            dr = beanAsset.am_master_asset_depreciation.NewRow();
            dr["SID"] = ERPWAuthentication.SID;
            dr["CompanyCode"] = ERPWAuthentication.CompanyCode;
            dr["AssetCode"] = "";
            dr["AssetSubCode"] = "00";
            dr["DepreMethod"] = assetService.getDepreciationMethod(ERPWAuthentication.SID).Rows[0]["calculation_code"].ToString();
            dr["DepreBookType"] = "00";
            dr["DepreDate"] = Validation.getCurrentServerStringDateTime().Substring(0, 8);
            dr["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
            dr["UPDATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
            beanAsset.am_master_asset_depreciation.Rows.Add(dr);

            dr = beanAsset.am_master_asset_general1.NewRow();
            dr["SID"] = ERPWAuthentication.SID;
            dr["CompanyCode"] = ERPWAuthentication.CompanyCode;
            dr["AssetCode"] = "";
            dr["AssetSubCode"] = "00";
            dr["AssetGroup"] = ddlAssetGroup.SelectedValue;
            dr["AssetType"] = ddlAssetType.SelectedValue;
            dr["AssetCategory1"] = ddlAssetCategory1.SelectedValue;
            dr["AssetCategory2"] = ddlAssetCategory2.SelectedValue;
            dr["AcquistionDate"] = Validation.getCurrentServerStringDateTime().Substring(0, 8);
            dr["CURRENCYCODE"] = assetService.getCompanyCurrency(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            dr["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
            dr["UPDATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
            beanAsset.am_master_asset_general1.Rows.Add(dr);

            dr = beanAsset.am_master_asset_general2.NewRow();
            dr["SID"] = ERPWAuthentication.SID;
            dr["CompanyCode"] = ERPWAuthentication.CompanyCode;
            dr["AssetCode"] = "";
            dr["AssetSubCode"] = "00";
            dr["AssetOwner"] = String.IsNullOrEmpty(ddlOwner.SelectedValue) ? ERPWAuthentication.EmployeeCode : ddlOwner.SelectedValue;
            dr["Location1"] = ddlLocation1.SelectedValue;
            dr["Location2"] = ddlLocation2.SelectedValue;
            dr["Room"] = ddlRoom.SelectedValue;
            dr["Department"] = ddlDepartment.SelectedValue;
            dr["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
            dr["UPDATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
            beanAsset.am_master_asset_general2.Rows.Add(dr);

            #region Default Bean
            dr = beanAsset.am_master_asset_warranty.NewRow();
            dr["SID"] = ERPWAuthentication.SID;
            dr["CompanyCode"] = ERPWAuthentication.CompanyCode;
            dr["AssetCode"] = "";
            dr["AssetSubCode"] = "00";
            dr["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
            dr["UPDATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
            beanAsset.am_master_asset_warranty.Rows.Add(dr);

            dr = beanAsset.am_master_asset_taxpaymentdata.NewRow();
            dr["SID"] = ERPWAuthentication.SID;
            dr["CompanyCode"] = ERPWAuthentication.CompanyCode;
            dr["AssetCode"] = "";
            dr["AssetSubCode"] = "00";
            dr["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
            dr["UPDATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
            beanAsset.am_master_asset_taxpaymentdata.Rows.Add(dr);

            dr = beanAsset.am_master_asset_leasing.NewRow();
            dr["SID"] = ERPWAuthentication.SID;
            dr["CompanyCode"] = ERPWAuthentication.CompanyCode;
            dr["AssetCode"] = "";
            dr["AssetSubCode"] = "00";
            dr["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
            dr["UPDATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
            beanAsset.am_master_asset_leasing.Rows.Add(dr);

            dr = beanAsset.am_master_asset_insurance.NewRow();
            dr["SID"] = ERPWAuthentication.SID;
            dr["CompanyCode"] = ERPWAuthentication.CompanyCode;
            dr["AssetCode"] = "";
            dr["AssetSubCode"] = "00";
            dr["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
            dr["UPDATED_BY"] = ERPWAuthentication.EmployeeCode;
            dr["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
            beanAsset.am_master_asset_insurance.Rows.Add(dr);
            #endregion
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string _msg = "";
                if (string.IsNullOrEmpty(ddlAssetGroup.SelectedValue))
                {
                    _msg = "กรุณาระบุ Asset Group.";
                }
                if (string.IsNullOrEmpty(ddlBranch.SelectedValue))
                {
                    _msg += _msg == "" ? "" : "<br />";
                    _msg += "กรุณาระบุ Branch.";
                }

                if (_msg != "")
                {
                    throw new Exception(_msg);
                }

                initialNewBean();
                AssetMode = ApplicationSession.CREATE_MODE_STRING;
                Response.Redirect("AssetMasterDetail.aspx");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        protected void Edit_ItemGV(object sender, EventArgs e)
        {
            try
            {
                string code = Convert.ToString(((LinkButton)sender).CommandName);

                beanAsset = new MasterAsset();

                Object[] objParam = new Object[] { 
                    "LI00228", 
                    (string)Session[ApplicationSession.USER_SESSION_ID], 
                    ERPWAuthentication.CompanyCode,
                    code,
                    "00"
                };

                DataSet[] objDataSet = new DataSet[] { };

                DataSet objResult = icmUtils.ICMDataSetInvoke(objParam, objDataSet);

                if (objResult != null)
                {
                    beanAsset.Merge(objResult);

                    dtDelegate = new DataTable();
                    dtDelegate = assetService.getAssetMappingEmployee(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, code);

                    AssetMode = ApplicationSession.CHANGE_MODE_STRING;

                    Response.Redirect("AssetMasterDetail.aspx");
                }
                else
                {
                    throw new Exception("ไม่สามารถทำรายการได้");
                }

            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        private void inactiveAsset(string assetCode)
        {
            if (!string.IsNullOrEmpty(assetCode))
            {
                MasterAsset inactiveBean = new MasterAsset();

                Object[] objParam = new Object[] { 
                    "LI00250", 
                    (string)Session[ApplicationSession.USER_SESSION_ID], 
                    ERPWAuthentication.CompanyCode,
                    assetCode,
                    "00"
                };

                DataSet[] objDataSet = new DataSet[] { };

                DataSet objResult = icmUtils.ICMDataSetInvoke(objParam, objDataSet);

                if (objResult != null)
                {
                    inactiveBean.Merge(objResult);

                    Object[] objParam2 = new Object[] { "LI00252", (string)Session[ApplicationSession.USER_SESSION_ID] };

                    DataSet[] objDataSet2 = new DataSet[] { inactiveBean };

                    String objResult2 = (String)icmUtils.ICMPrimitiveInvoke(objParam2, objDataSet2);

                    if (objResult2.Contains("S#"))
                    {
                        getDataAsset();

                        ClientService.AGSuccess("In-Active Success.<br />Document Number : " + assetCode);
                    }
                    else
                    {
                        ClientService.AGError(ObjectUtil.Err(objResult2.Replace("E#", "")));
                    }
                }
            }
            else
            {
                throw new Exception("Asset code not found.");
            }
        }

        protected void Delete_ItemGV(object sender, EventArgs e)
        {
            try
            {
                string code = Convert.ToString(((LinkButton)sender).CommandName);

                inactiveAsset(code);
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



        #region  Convert Properties

        public string convertToFormatNumber(object xValue)
        {
            if (xValue == null)
            {
                return "0.00";
            }
            Decimal dValue = 0;
            Decimal.TryParse(xValue.ToString(),out dValue);
            return dValue.ToString("#,##0.00");

        }

        #endregion

    }
}