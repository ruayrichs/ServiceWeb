using agape.lib.constant;
using agape.proxy.data.dataset;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ServiceWeb.Asset.API.Class;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Service;
using ServiceWeb.auth;
using ERPW.Lib.Master.Config;
using ERPW.Lib.F1WebService.ICMUtils;

namespace ServiceWeb.crm.Master.Asset
{
    public partial class AssetMasterDetail : AbstractsSANWebpage
    {
        private AssetServiceLibrary assetlibrary = AssetServiceLibrary.getInstance();
        private MasterConfigLibrary masterserver = MasterConfigLibrary.GetInstance();     
        private ICMUtils icmUtils = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        private AssetStructureModel StructureService = new AssetStructureModel();

        private DBService dbService = new DBService();

        #region Session

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

        public bool isCreateMode
        {
            get { return (AssetMode == ApplicationSession.CREATE_MODE_STRING); }
         }
       
        //protected DataTable dtAllEmp
        //{
        //    get
        //    {
        //        if (Session["AssetMasterDetail_dtAllEmp"] == null)
        //        {
        //            Session["AssetMasterDetail_dtAllEmp"] = activityService.getEmployeeDetailAllCompany(ERPWAuthentication.CompanyCode, ERPWAuthentication.SID);
        //        }

        //        return (DataTable)Session["AssetMasterDetail_dtAllEmp"];
        //    }
        //    set { Session["AssetMasterDetail_dtAllEmp"] = value; }
        //}

        #endregion

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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //dtAllEmp = null;

                    bindingDropDownList();
                    setBinding();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        private void setGellery(string assetCode)
        {
            List<GalleryEntities> list = assetlibrary.getGallery(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, assetCode);

            string json = JSONUtil.GetJson(list);

            ClientService.DoJavascript("bindingGallery(" + json + ",'" + assetCode + "');");
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
            ddlBranch.Items.Insert(0, new ListItem("เลือก", ""));

            DataTable dtDepartment = assetService.getDepartment(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            ddlDepartment.DataSource = dtDepartment;
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new ListItem("เลือก", ""));

            DataTable dtCurrency = assetService.getCurrency(ERPWAuthentication.SID);
            ddlCurrency.DataSource = dtCurrency;
            ddlCurrency.DataBind();
            ddlCurrency.Items.Insert(0, new ListItem("เลือก", ""));

            ddlAssetGroup.DataSource = dtAssetGroup;
            ddlAssetGroup.DataBind();
            ddlAssetGroup.Items.Insert(0, new ListItem("เลือก", ""));

            ddlAssetType.DataSource = dtAssetType;
            ddlAssetType.DataBind();
            ddlAssetType.Items.Insert(0, new ListItem("เลือก", ""));

            ddlAssetCategory1.DataSource = dtAssetCategory1;
            ddlAssetCategory1.DataBind();
            ddlAssetCategory1.Items.Insert(0, new ListItem("เลือก", ""));

            ddlAssetCategory2.DataSource = dtAssetCategory2;
            ddlAssetCategory2.DataBind();
            ddlAssetCategory2.Items.Insert(0, new ListItem("เลือก", ""));

            ddlLocation1.DataSource = dtAssetLocation1;
            ddlLocation1.DataBind();
            ddlLocation1.Items.Insert(0, new ListItem("เลือก", ""));

            ddlLocation2.DataSource = dtAssetLocation2;
            ddlLocation2.DataBind();
            ddlLocation2.Items.Insert(0, new ListItem("เลือก", ""));

            ddlRoom.DataSource = dtAssetRoom;
            ddlRoom.DataBind();
            ddlRoom.Items.Insert(0, new ListItem("เลือก", ""));

            ddlCostCenter.DataSource = masterserver.GetCostCenterMaster(ERPWAuthentication.SID,ERPWAuthentication.CompanyCode);
            ddlCostCenter.DataBind();
            ddlCostCenter.Items.Insert(0, new ListItem("เลือก", ""));
        }
        private DataTable dtRelation { get; set; }
        private void setBinding()
        {
            //if (AssetMode == ApplicationSession.CREATE_MODE_STRING)
            //{
            //    btnCreateAsset.Visible = true;
            //    btnSaveAsset.Visible = false;

            //}
            //else
            //{
            //    btnCreateAsset.Visible = false;
            //    btnSaveAsset.Visible = true;

            //}

            foreach (DataRow dr in beanAsset.am_master_asset_subcode.Rows)
            {
                dtRelation = StructureService.GetAssetRelation(
                    ERPWAuthentication.SID, 
                    ERPWAuthentication.CompanyCode,
                    dr["AssetCode"].ToString() + dr["AssetSubCode"].ToString());

                rptRelation.DataSource = dtRelation;
                rptRelation.DataBind();
            }

            foreach (DataRow dr in beanAsset.am_master_asset_header.Rows)
            {
                txtAssetCode.Text = dr["AssetCode"].ToString();
                ddlBranch.SelectedValue = dr["BranchCode"].ToString();

                if (AssetMode == ApplicationSession.CHANGE_MODE_STRING)
                {
                    setGellery(dr["AssetCode"].ToString());
                }
            }
            foreach (DataRow dr in beanAsset.am_master_asset_general1.Rows)
            {
                ddlAssetGroup.SelectedValue = dr["AssetGroup"].ToString();
                ddlAssetType.SelectedValue = dr["AssetType"].ToString();
                ddlAssetCategory1.SelectedValue = dr["AssetCategory1"].ToString();
                ddlAssetCategory2.SelectedValue = dr["AssetCategory2"].ToString();
                ddlCurrency.SelectedValue = dr["CURRENCYCODE"].ToString();
                txtAssetReceiveDate.Text = Validation.Convert2DateDisplay(dr["AcquistionDate"].ToString());
            }
            foreach (DataRow dr in beanAsset.am_master_asset_general2.Rows)
            {
                ddlOwner.SelectedValue = dr["AssetOwner"].ToString();
                ddlLocation1.SelectedValue = dr["Location1"].ToString();
                ddlLocation2.SelectedValue = dr["Location2"].ToString();
                ddlRoom.SelectedValue = dr["Room"].ToString();
                ddlDepartment.SelectedValue = dr["Department"].ToString();
                ddlCostCenter.SelectedValue = dr["CostCenter"].ToString();
            }
            foreach (DataRow dr in beanAsset.am_master_asset_subcode.Rows)
            {
                txtAssetName.Text = dr["AssetSubCodeDescription"].ToString();

                decimal assetValue = 0;
                decimal.TryParse(dr["AssetValue"].ToString(), out assetValue);

                txtAssetValue.Text = assetValue.ToString("#,##0.00");
            }
            updateDataAssetMaster.Update();
            updatePanelValue.Update();
        }

        private void getDataAfterSave(string assetCode)
        {
            beanAsset = new MasterAsset();

            Object[] objParam = new Object[] { 
                    "LI00228", 
                    (string)Session[ApplicationSession.USER_SESSION_ID], 
                    ERPWAuthentication.CompanyCode,
                    assetCode,
                    "00"
                };

            DataSet[] objDataSet = new DataSet[] { };

            DataSet objResult = icmUtils.ICMDataSetInvoke(objParam, objDataSet);

            if (objResult != null)
            {
                beanAsset.Merge(objResult);
            }
            else
            {
                throw new Exception("ไม่สามารถทำรายการได้");
            }
        }

        protected void btnSaveEditeAsset_Click(object sender, EventArgs e)
        {
            MasterAsset tempDS = ObjectUtil.Copy<MasterAsset>(beanAsset);

            try
            {
                validateForSaveData();
                prepareSave();

                string _result = changeAsset();

                if (_result.Contains("S#"))
                {
                    string assetCode = _result.Replace("S#", "").Trim();

                    getDataAfterSave(assetCode);

                    setBinding();

                    ClientService.AGSuccess("Update Success.<br />Document Number : " + assetCode);
                }
                else
                {
                    beanAsset = tempDS;

                    ClientService.AGError(ObjectUtil.Err(_result.Replace("E#", "")));
                }
            }
            catch (Exception ex)
            {
                beanAsset = tempDS;

                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        protected void btnSeveCreateAsset_Click(object sender, EventArgs e)
        {
            MasterAsset tempDS = ObjectUtil.Copy<MasterAsset>(beanAsset);

            try
            {
                validateForSaveData();
                prepareSave();

                string _result = createAsset();

                if (_result.Contains("S#"))
                {
                    AssetMode = ApplicationSession.CHANGE_MODE_STRING;

                    string assetCode = _result.Replace("S#", "").Trim();

                    getDataAfterSave(assetCode);

                    setBinding();

                    ClientService.AGSuccess("Create Success.<br />Document Number : " + assetCode);
                }
                else
                {
                    beanAsset = tempDS;

                    ClientService.AGError(ObjectUtil.Err(_result.Replace("E#", "")));
                }
            }
            catch (Exception ex)
            {
                beanAsset = tempDS;

                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        #region FUN Save

        private void validateForSaveData()
        {
            string msg = "";
            if (string.IsNullOrEmpty(txtAssetName.Text.Trim()))
            {
                msg = "กรุณาระบุ Asset Name! <br/>";
            }

            if(string.IsNullOrEmpty(txtAssetReceiveDate.Text.Trim()))
            {
                msg += "กรุณาระบุ Asset Receive Date! <br/>";
            }

            if (msg != "")
            {
                throw new Exception(msg);
            }
        }
        private void prepareSave()
        {
            if (AssetMode == ApplicationSession.CREATE_MODE_STRING)
            {
                foreach (DataRow dr in beanAsset.am_master_asset_header.Rows)
                {
                    dr["AssetCode"] = txtAssetCode.Text;
                    dr["BranchCode"] = ddlBranch.SelectedValue;
                }
                foreach (DataRow dr in beanAsset.am_master_asset_general1.Rows)
                {
                    dr["AssetCode"] = txtAssetCode.Text;
                    dr["AssetGroup"] = ddlAssetGroup.SelectedValue;
                }
                foreach (DataRow dr in beanAsset.am_master_asset_general2.Rows)
                {
                    dr["AssetCode"] = txtAssetCode.Text;
                    dr["CostCenter"] = ddlCostCenter.SelectedValue;
                }
                foreach (DataRow dr in beanAsset.am_master_asset_subcode.Rows)
                {
                    dr["AssetCode"] = txtAssetCode.Text;
                }
            }


            foreach (DataRow dr in beanAsset.am_master_asset_general1.Rows)
            {
                dr["AssetType"] = ddlAssetType.SelectedValue;
                dr["AssetCategory1"] = ddlAssetCategory1.SelectedValue;
                dr["AssetCategory2"] = ddlAssetCategory2.SelectedValue;
                dr["CURRENCYCODE"] = ddlCurrency.SelectedValue;
                dr["AcquistionDate"] = Validation.Convert2DateDB(txtAssetReceiveDate.Text);
                dr["UPDATED_BY"] = ERPWAuthentication.EmployeeCode;
                dr["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
            }
            foreach (DataRow dr in beanAsset.am_master_asset_general2.Rows)
            {
                dr["AssetOwner"] = ddlOwner.SelectedValue;
                dr["Location1"] = ddlLocation1.SelectedValue;
                dr["Location2"] = ddlLocation2.SelectedValue;
                dr["Room"] = ddlRoom.SelectedValue;
                dr["Department"] = ddlDepartment.SelectedValue;
                dr["UPDATED_BY"] = ERPWAuthentication.EmployeeCode;
                dr["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
            }
            foreach (DataRow dr in beanAsset.am_master_asset_subcode.Rows)
            {
                dr["AssetSubCodeDescription"] = txtAssetName.Text;

                decimal assetValue = 0;
                decimal.TryParse(txtAssetValue.Text, out assetValue);

                dr["AssetValue"] = assetValue;
                dr["UPDATED_BY"] = ERPWAuthentication.EmployeeCode;
                dr["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
            }
        }

        #region FUN Create Asset

        private string createAsset()
        {
            Object[] objParam = new Object[] { "LI00221", (string)Session[ApplicationSession.USER_SESSION_ID] };

            DataSet[] objDataSet = new DataSet[] { beanAsset };

            Object objResult = icmUtils.ICMPrimitiveInvoke(objParam, objDataSet);

            return objResult.ToString();
        }

        #endregion

        #region FUN Change Asset
        private string changeAsset()
        {
            Object[] objParam = new Object[] { "LI00247", (string)Session[ApplicationSession.USER_SESSION_ID] };

            DataSet[] objDataSet = new DataSet[] { beanAsset };

            Object objResult = icmUtils.ICMPrimitiveInvoke(objParam, objDataSet);

            return objResult.ToString();
        }

        #endregion
        #endregion

        //private string getEmpName(string linkID)
        //{
        //    if (!string.IsNullOrEmpty(linkID))
        //    {
        //        DataRow[] drr = dtAllEmp.Select("LINKID='" + linkID + "'");

        //        if (drr.Length > 0)
        //        {
        //            linkID = drr[0]["xThaiName"].ToString() == "" ? drr[0]["xEngName"].ToString() : drr[0]["xThaiName"].ToString();
        //        }
        //    }

        //    return linkID;
        //}

        #region Relation

        protected void udpnPicture_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request["__EVENTTARGET"]) && Request["__EVENTTARGET"] == udpnPicture.ClientID)
                {
                    string param = Request["__EVENTARGUMENT"];

                    setGellery(param);
                }
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

        public string LevelNodeRelation(string Relation)
        {
            string style = "";

            if (Relation.Equals("Parent"))
            {
                style = "margin-left: 0px;cursor:pointer;";
            }
            else if (Relation.Equals("This"))
            {
                style = "margin-left: 40px";
            }
            else if (Relation.Equals("Child"))
            {
                style = "margin-left: 80px;cursor:pointer;";
            }

            return style;
        }

        public string LevelNodeRelationLine(string Relation, int index)
        {
            string style = "";

            if (Relation.Equals("This"))
            {
                style = "margin-left: 60px;";
            }
            else if (Relation.Equals("Child") && index > 0 && index < dtRelation.Rows.Count - 1)
            {
                style = "margin-left: 60px;height: 100px;margin-top: -65px;";
            }
            //else if (Relation.Equals("Child") && index == 1)
            //{
            //    style = "margin-left: 60px;height: 100px;margin-top: -65px;";
            //}

            if (index == dtRelation.Rows.Count - 1)
            {
                style = "display:none;";
            }

            return style;
        }

        protected void linkAssetNode_Click(object sender, EventArgs e)
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

        #endregion
    }
}