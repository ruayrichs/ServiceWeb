using agape.lib.constant;
using agape.lib.web.ICMV2;
using Agape.FocusOne.Utilities;
using Agape.Lib.Web.Bean.CS;
using Agape.Lib.Web.Bean.MM;
using ServiceWeb.auth;
using ServiceWeb.crm.AfterSale;
using ServiceWeb.LinkFlowChart.Service;
using ServiceWeb.Service;
using ServiceWeb.widget.usercontrol;
using ERPW.Lib.Authentication;
using SNA.Lib.crm;
using SNA.Lib.Master;
using SNA.Lib.POS.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Master;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Service;
using ERPW.Lib.Master.Config;
using Newtonsoft.Json;

namespace ServiceWeb.crm.Master.Equipment
{
    public partial class EquipmentDetail : AbstractsSANWebpage //System.Web.UI.Page
    {
        string ThisPage = "EquipmentDetail";

        protected override string getProgramID()
        {
            return LogServiceLibrary.PROGRAM_ID_EQUIPMENT;
        }

        public bool FilterOwner
        {
            get
            {
                bool _FilterOwner = false;
                bool.TryParse(System.Web.Configuration.WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out _FilterOwner);
                return _FilterOwner;
            }
        }

        #region Service        
        private ICMUtils icmUtil = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        private UniversalService ServiceUniversal = new UniversalService();
        private EquipmentService ServiceEquipment = new EquipmentService();
        private LogServiceLibrary ServiceLog = new LogServiceLibrary();
        SNAMasterService masterService = new SNAMasterService();        
        private ERPW.Lib.Master.EmployeeService empService = ERPW.Lib.Master.EmployeeService.GetInstance();
        private MasterConfigLibrary erpwMaster = MasterConfigLibrary.GetInstance();
        #endregion

        #region Data set
        private tmpEquipmentSetupDataSet dsEquipment
        {
            get
            {
                if (Session["EquipmentDetail.tmpEquipmentSetupDataSet.dsEquipment"] == null)
                {
                    Session["EquipmentDetail.tmpEquipmentSetupDataSet.dsEquipment"] = new tmpEquipmentSetupDataSet();
                }
                return (tmpEquipmentSetupDataSet)Session["EquipmentDetail.tmpEquipmentSetupDataSet.dsEquipment"];
            }
            set
            {
                Session["EquipmentDetail.tmpEquipmentSetupDataSet.dsEquipment"] = value;
            }
        }

        private tmpServiceCallDataSet serviceCallEntity
        {
            get
            {
                return Session["ServicecallEntity"] == null
                ? new tmpServiceCallDataSet()
                : (tmpServiceCallDataSet)Session["ServicecallEntity"];
            }
            set { Session["ServicecallEntity"] = value; }
        }
               
        #endregion

        public string qrcodeurl
        {
            get
            {
                return String.Concat(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), "/crm/Master/Equipment/EquipmentDetail.aspx?id=", txtEquipmentCode.Text);
            }
        }

        #region DataTable
        private DataTable dtPropertiesSelect
        {
            get
            {
                if (Session["properties_dtPropertiesSelect"] == null)
                {
                    Session["properties_dtPropertiesSelect"] = CRMService.getInstance().getSelectedValue(SID, "*");
                }

                return (DataTable)Session["properties_dtPropertiesSelect"];
            }
            set { Session["properties_dtPropertiesSelect"] = value; }
        }

        DataTable dtTempDoc
        {
            get
            {
                if (Session["SC_dtTempDoc"] == null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("doctype");
                    dt.Columns.Add("docnumber");
                    dt.Columns.Add("docfiscalyear");
                    dt.Columns.Add("indexnumber");
                    Session["SC_dtTempDoc"] = dt;
                }
                return (DataTable)Session["SC_dtTempDoc"];
            }
            set { Session["SC_dtTempDoc"] = value; }
        }

        DataTable dtDataSearch
        {
            get { return Session["EquipmentDetail.SCFC_dtDataSearch"] == null ? null : (DataTable)Session["EquipmentDetail.SCFC_dtDataSearch"]; }
            set { Session["EquipmentDetail.SCFC_dtDataSearch"] = value; }
        }

        DataTable dtTempAttachfile
        {
            get
            {
                return Session["dtTempAttachfile"] == null ? null : (DataTable)Session["dtTempAttachfile"];
            }
            set { Session["dtTempAttachfile"] = value; }
        }

        #endregion

        public string mode_stage
        {
            get
            {
                if (Session["SC_MODE"] == null)
                { Session["SC_MODE"] = ApplicationSession.CREATE_MODE_STRING; }
                return (string)Session["SC_MODE"];
            }
            set { Session["SC_MODE"] = value; }
        }
        public string EquipmentCode { 
            get {
                if (!String.IsNullOrEmpty(Request["id"]))
                {
                    return Request["id"] as string;
                }
                return !String.IsNullOrEmpty(Session["ServiceWeb.Page.Equipment.EquipmentCode"] as string) ? Session["ServiceWeb.Page.Equipment.EquipmentCode"] as string : "none" ; 
            } 
        }
        public string Page_Mode {
            get {
                if (Request["id"] != null)
                {
                    return null;
                }
                return Session["ServiceWeb.Page.Equipment.Page_Mode"] as string; 
            } 
        }

        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dsEquipment = new tmpEquipmentSetupDataSet();

                setDataDefult();

                bindAllDropdown();
               
                bindWarrantyCriteria();

                if (string.IsNullOrEmpty(EquipmentCode))
                {
                    btnOpenDiagram.Visible = false;
                    PrepareDataEquipmentHeaderFirstLoad();
                    getproperties();
                }
                else
                {
                    bindDefaultToScreen();
                }

                Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(new DataTable(), "", "");
            }

            //SmartPagingOwnerAssignment.SelectedPageChange += PagingSelectedPageChangeOwnerAssignment;
        }

        #region Paging OwnerAssignment
        private void PagingSelectedPageChangeOwnerAssignment(object sender, int pageIndex)
        {
            //SmartPagingOwnerAssignment.PageIndex = pageIndex;
            bindDataTableOwnerAssignment();
        }


        #endregion

        private void bindDefaultToScreen()
        {
            btnOpenDiagram.Visible = true;
            BindDataEquipment();
           
            bindRelationship();
            LoadListTicket();
            loadTimeLineFileAttachment(); 
        }

        private void getproperties()
        {
            DataTable dt = masterService.getConfigProperties(SID, "EQ");
            dt.Columns.Add("xValue");
            dt.Columns.Add("ObjectID");
            rptAttributes.DataSource = dt;
            rptAttributes.DataBind();
            udpAttributes.Update();
        }

        protected void rptAttributes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hddselectedcode = e.Item.FindControl("hddselectedcode") as HiddenField;
            HiddenField hddxvalue = e.Item.FindControl("hddxvalue") as HiddenField;
            if (hddselectedcode.Value != "")
            {
                DataTable dtselected = getdropdown(hddselectedcode.Value);

                DropDownList ddlproperties = e.Item.FindControl("ddlproperties") as DropDownList;
                ddlproperties.DataTextField = "xValue";
                ddlproperties.DataValueField = "DetailCode";
                ddlproperties.DataSource = dtselected;
                ddlproperties.DataBind();
                ddlproperties.SelectedValue = hddxvalue.Value;
                ddlproperties.Style["display"] = "";
            }

            if (!Permission.ConfigurationItemModify)
            {
                TextBox txtdata = e.Item.FindControl("txtdata") as TextBox;
                txtdata.Enabled = false;
            }
        }

        private DataTable getdropdown(string selectedcode)
        {
            DataTable dt = new DataTable();

            if (!string.IsNullOrEmpty(selectedcode))
            {
                DataRow[] drr = dtPropertiesSelect.Select("Code='" + selectedcode + "'");

                dt = dtPropertiesSelect.Clone();

                dt.Rows.Add("", "", "", "", "", "", "", "", "");

                foreach (DataRow dr in drr)
                {
                    dt.ImportRow(dr);
                }
            }
            else
            {
                dt = dtPropertiesSelect.Copy();
            }

            return dt;
        }

        private void UpdateProperties()
        {
            //if (!mode.Equals("Edit"))
            //{
            for (int i = 0; i < rptAttributes.Items.Count; i++)
            {
                string EquipmentCode = txtEquipmentCode.Text;
                HiddenField hddselectedcode = rptAttributes.Items[i].FindControl("hddselectedcode") as HiddenField;
                HiddenField hddobjectid = rptAttributes.Items[i].FindControl("hddobjectid") as HiddenField;
                HiddenField hddsid = rptAttributes.Items[i].FindControl("hddsid") as HiddenField;
                HiddenField hddpropertiescode = rptAttributes.Items[i].FindControl("hddpropertiescode") as HiddenField;
                string xValues = "";
                if (string.IsNullOrEmpty(hddselectedcode.Value))
                {
                    TextBox txtdata = rptAttributes.Items[i].FindControl("txtdata") as TextBox;
                    xValues = txtdata.Text;
                }
                else
                {
                    DropDownList ddlproperties = rptAttributes.Items[i].FindControl("ddlproperties") as DropDownList;
                    xValues = ddlproperties.SelectedValue;
                }


                DataRow[] drr = dsEquipment.master_equipment_properties.Select("SID='" + hddsid.Value + "' and ObjectID='" + hddobjectid.Value + "' and PropertiesCode='" + hddpropertiescode.Value + "'", "", DataViewRowState.CurrentRows);
                if (drr.Length > 0)
                {
                    if (drr[0]["xValue"].ToString() == "")
                    {
                        drr[0]["xValue"] = xValues;
                        drr[0].AcceptChanges();
                        drr[0].SetAdded();
                    }
                    else
                    {
                        drr[0]["xValue"] = xValues;
                    }

                }
                else
                {
                    DataRow dr = dsEquipment.master_equipment_properties.NewRow();
                    dr["SID"] = hddsid.Value;
                    dr["ObjectID"] = hddobjectid.Value;
                    dr["PropertiesCode"] = hddpropertiescode.Value;
                    dr["xValue"] = xValues;
                    dr["Created_By"] = EmployeeCode;
                    dr["Created_On"] = Validation.getCurrentServerStringDateTime();

                    dsEquipment.master_equipment_properties.Rows.Add(dr);
                }
            }
            //}
            //else
            //{
            //    for (int i = 0; i < rptAttributes.Items.Count; i++)
            //    {
            //        DataRow dr = dsEquipment.master_equipment_properties.NewRow();
            //        string EquipmentCode = txtEquipmentCode.Text;

            //        HiddenField hddsid = rptAttributes.Items[i].FindControl("hddsid") as HiddenField;
            //        HiddenField hddobjectid = rptAttributes.Items[i].FindControl("hddobjectid") as HiddenField;
            //        HiddenField hddselectedcode = rptAttributes.Items[i].FindControl("hddselectedcode") as HiddenField;
            //        HiddenField hddpropertiescode = rptAttributes.Items[i].FindControl("hddpropertiescode") as HiddenField;
            //        string xValues = "";
            //        if (string.IsNullOrEmpty(hddselectedcode.Value))
            //        {
            //            TextBox txtdata = rptAttributes.Items[i].FindControl("txtdata") as TextBox;
            //            xValues = txtdata.Text;
            //        }
            //        else
            //        {
            //            DropDownList ddlproperties = rptAttributes.Items[i].FindControl("ddlproperties") as DropDownList;
            //            xValues = ddlproperties.SelectedValue;
            //        }

            //        DataRow[] drr = dsEquipment.master_equipment_properties.Select("SID='" + hddsid.Value + "' and ObjectID='" + hddobjectid.Value + "' and PropertiesCode='" + hddpropertiescode.Value + "'","", DataViewRowState.CurrentRows);
            //        DataRow drData = null;
            //        if (drr.Length > 0)
            //        {
            //            drr[0]["xValue"] = xValues;
            //        }
            //    }
            //}
        }

        public Boolean isSelectedValue(Object obj)
        {
            if (obj != null)
            {
                string type = obj.ToString();
                return type != "";
            }
            else
            {
                return false;
            }
        }

        #region First Load
        private void setDataDefult()
        {
            txtAccountAssignmentBox_CompanyCode.Text = CompanyCode;
            txtAccountAssignmentBox_CompanyName.Text = CompanyName;
            txtCompanyCode.Text = CompanyCode + " : " + CompanyName;            

            txtGeneralBox_Start_UpDate.Text = Validation.getCurrentServerDate();
            txtReferenceDataBox_AcquisitionDate.Text = Validation.getCurrentServerDate();
        }

        private void PrepareDataEquipmentHeaderFirstLoad()
        {
            #region master_equipment
            if (dsEquipment.master_equipment.Rows.Count == 0)
            {
                DataRow drNew = dsEquipment.master_equipment.NewRow();
                drNew["SID"] = SID;
                drNew["CompanyCode"] = CompanyCode;
                drNew["EquipmentType"] = ddlEquipmentType.SelectedValue;
                drNew["Description"] = txtEquipmentName.Text;
                drNew["Status"] = "N";
                drNew["Valid_From"] = Validation.Convert2DateDB(txtEquipmentDateFrom.Text);
                drNew["Valid_To"] = Validation.Convert2DateDB(txtEquipmentDateTo.Text);
                drNew["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
                drNew["CREATED_BY"] = EmployeeCode;
                dsEquipment.master_equipment.Rows.Add(drNew);
            }
            else
            {
                dsEquipment.master_equipment.Rows[0]["Reference"] = txtManufacturerDataBox_Reference.Text;
                dsEquipment.master_equipment.Rows[0]["EquipmentType"] = ddlEquipmentType.SelectedValue;
                dsEquipment.master_equipment.Rows[0]["Description"] = txtEquipmentName.Text;
                dsEquipment.master_equipment.Rows[0]["Status"] = ddlStatus.SelectedValue;
                dsEquipment.master_equipment.Rows[0]["Valid_From"] = Validation.Convert2DateDB(txtEquipmentDateFrom.Text);
                dsEquipment.master_equipment.Rows[0]["Valid_To"] = Validation.Convert2DateDB(txtEquipmentDateTo.Text);
            }

            #endregion
        }

        private void BindDataEquipment()
        {
            Object[] objParam = new Object[] { "0800044", POSDocumentHelper.getSessionId(SID, UserName),SID
                , UserName,CompanyCode,EquipmentCode };
            DataSet[] objDataSetChange = new DataSet[] { dsEquipment };
            DataSet objReturn = icmUtil.ICMDataSetInvoke(objParam, objDataSetChange);

            if (objReturn != null && objReturn.Tables.Count > 0)
            {
                dsEquipment = new tmpEquipmentSetupDataSet();
                dsEquipment.Merge(objReturn);
                dsEquipment.AcceptChanges();
            }

            foreach (DataRow drr in dsEquipment.master_equipment.Rows)
            {
                txtEquipmentCode.Text = drr["EquipmentCode"].ToString();
                txtManufacturerDataBox_Reference.Text = drr["Reference"].ToString();
                txtEquipmentName.Text = drr["Description"].ToString();
                ddlEquipmentType.SelectedValue = drr["EquipmentType"].ToString();
                txtEquipmentDateFrom.Text = Validation.Convert2DateDisplay(drr["Valid_From"].ToString());
                txtEquipmentDateTo.Text = Validation.Convert2DateDisplay(drr["Valid_To"].ToString());
                ddlStatus.SelectedValue = drr["Status"].ToString();
                ddlCategory.SelectedValue = drr["CategoryCode"].ToString();
                txtGeneralBox_ActiveBy.Text = drr["ActiveBy"].ToString(); // unknow
                txtGeneralBox_ActiveTime.Text = string.IsNullOrEmpty(drr["ActiveTime"] as string) || (drr["ActiveTime"] as string) == "NULL" ? "" : Validation.Convert2DateDisplay(drr["ActiveTime"].ToString());
                txtGeneralBox_ActiveDate.Text = string.IsNullOrEmpty(drr["ActiveDate"] as string) || (drr["ActiveDate"] as string) == "NULL" ? "" : Validation.Convert2TimeDisplay(drr["ActiveDate"].ToString());
                bindLogEquipment(drr["EquipmentCode"].ToString());
            }

            dsEquipment.master_equipment_properties.DefaultView.Sort = "PropertiesCode ASC";

            rptAttributes.DataSource = dsEquipment.master_equipment_properties;
            rptAttributes.DataBind();
            udpAttributes.Update();
            SelectionCIFamily_Change(null, EventArgs.Empty);
            foreach (DataRow dr in dsEquipment.master_equipment_general.Rows)
            {
                ddlEMClass.SelectedValue = dr["EquipmentClass"].ToString();
                txtGeneralBox_Weight.Text = dr["Weight"].ToString();
                ddlGeneralBox_Weight_WeightUnit.SelectedValue = dr["WeightUnit"].ToString();
                txtGeneralBox_Size_Dimension.Text = dr["SizeDimension"].ToString();
                //txtGeneralBox_MaterialNo.Text = dr[""].ToString(); // unknow
                txtGeneralBox_Start_UpDate.Text = Validation.Convert2DateDisplay(dr["StartupDate"].ToString());
                txtReferenceDataBox_AcquisitionValue.Text = dr["AcquisitionValue"].ToString();
                txtReferenceDataBox_CategoryCode.Text = dr["CategoryCode"].ToString(); // unknow
                txtReferenceDataBox_AcquisitionDate.Text = Validation.Convert2DateDisplay(dr["AcquisitionDate"].ToString());
                txtManufacturerDataBox_Manufacturer.Text = dr["ManufacturerNO"].ToString();
                txtManufacturerDataBox_AuthorizeGroup.Text = dr["AuthorizeGroup"].ToString(); // unknow
                txtManufacturerDataBox_ManufacturerCountry.Text = dr["ManufacturerCountry"].ToString();
                txtManufacturerDataBox_ModelNo.Text = dr["ModelNumber"].ToString();
                txtManufacturerDataBox_Constr_Yr.Text = dr["ConstructYear"].ToString();
                txtManufacturerDataBox_Constr_Mn.Text = dr["ConstructMonth"].ToString();
                txtManufacturerDataBox_MenuPartNo.Text = dr["ManufacturerPartNO"].ToString();
                txtManufacturerDataBox_InventoryNO.Text = dr["InventoryNO"].ToString(); // unknow
                txtManufacturerDataBox_SerialNo.Text = dr["ManufacturerSerialNO"].ToString();
                try
                {
                    ddlOwnerService.SelectedValue = dr["EquipmentObjectType"].ToString();;
                }
                catch (Exception)
                {
                }
                //txtGeneralBox_MaterialNo.Text = dr[""].ToString();
            }
            #region Equipment.master_equipment_location
            foreach (DataRow dr in dsEquipment.master_equipment_location.Rows)
            {
                // Zaan // ddlLocationDataBox_Plant_Code.SelectedValue = dr["MainPlant"].ToString();
                txtLocationDataBox_Flow_Phase.Text = dr["MainPlant"].ToString();

                // Zaan // ddlLocationDataBox_Location_Code.SelectedValue = dr["Location"].ToString();
                txtLocationDataBox_Location.Text = dr["Location"].ToString();

                // Zaan // ddlLocationDataBox_WorkCenter_Code.SelectedValue = dr["WorkCenter"].ToString();
                txtLocationDataBox_Cabinet.Text = dr["WorkCenter"].ToString();

                // Bank // For ITG
                txtLocationDataBox_CategoryCode.Text = dr["CategoryCode"].ToString();

                //txtLocationDataBox_Plant_2.Text = dr[""].ToString(); // unknow
                BindDDLStorageLocation(); // unknow
                udpLocationDataBox_Location.Update();

                //txtLocationDataBox_Location_2.Text = dr[""].ToString(); // unknow
                txtLocationDataBox_Room.Text = dr["Room"].ToString();
                txtLocationDataBox_Shelf.Text = dr["PlantSection"].ToString(); // unknow
                
                txtLocationDataBox_Slot.Text = dr["SortField"].ToString(); // unknow
                txtAddressBox_Name_1.Text = dr["AddressName1"].ToString();
                txtAddressBox_Name_2.Text = dr["AddressName2"].ToString();
                txtAddressBox_Address_Zip.Text = dr["AddressZip"].ToString();
                txtAddressBox_Address_City.Text = dr["AddressCity"].ToString();
                txtAddressBox_Address_Code_1.Text = dr["AddressCountryCode1"].ToString();
                txtAddressBox_Address_Code_2.Text = dr["AddressCountryCode2"].ToString();
                txtAddressBox_Street.Text = dr["AddressStreet"].ToString();
                txtAddressBox_Telephone.Text = dr["AddressTelephone"].ToString();
                txtAddressBox_Fax.Text = dr["AddressFax"].ToString();
            }
            #endregion

            foreach (DataRow dr in dsEquipment.master_equipment_organization.Rows)
            {
                txtAccountAssignmentBox_CompanyCode.Text = CompanyCode;
                txtAccountAssignmentBox_CompanyName.Text = CompanyName;
                ddlAccountAssignmentBox_BusinessArea_Code.SelectedValue = dr["AccBusinessArea"].ToString();
                //txtAccountAssignmentBox_BusinessArea_2.Text = dr[""].ToString(); // unknow

                //ddlAccountAssignmentBox_Asset.SelectedValue = dr["AccAsset1"].ToString();
                hddAccountAssignmentBox_AssetCode.Value = dr["AccAsset1"].ToString();
                txtAccountAssignmentBox_AssetDesc.Text = getDescriptionAsset(dr["AccAsset1"].ToString());

                ddlAccountAssignmentBox_CoustCenter.SelectedValue = dr["AccCostCenter1"].ToString();
                txtResponsibilitiesBox_PlanningPlant.Text = dr["ResPlanningPlant"].ToString();
                txtResponsibilitiesBox_PlannerGroup.Text = dr["ResPlannerGroup"].ToString();
                txtResponsibilitiesBox_MainWorkCtr_1.Text = dr["ResMainWorkCtr1"].ToString();
                txtResponsibilitiesBox_MainWorkCtr_2.Text = dr["ResMainWorkCtr2"].ToString();
                txtResponsibilitiesBox_CatalogProfile.Text = dr["ResCatalogProfile"].ToString();
            }

            foreach (DataRow dr in dsEquipment.master_equipment_sale_dist.Rows)
            {
                ddlSalesAndDistribution_BillingDocTypeCode.SelectedValue = dr["BLDoctype"].ToString();
                //txtSalesAndDistribution_BillingDocTypeName.Text = dr[""].ToString(); // unknow
                txtSalesAndDistribution_BillingDocYear.Text = dr["BLFiscalyear"].ToString();
                ddlSalesAndDistribution_BillingDocNumber.SelectedValue = dr["BLDocnumber"].ToString();
                ddlSalesAndDistribution_SalseOrganization.SelectedValue = dr["SalesOrgCode"].ToString();
                ddlSalesAndDistribution_DistChanal.SelectedValue = dr["DistChannel"].ToString();
                ddlSalesAndDistribution_Division.SelectedValue = dr["Division"].ToString();
                ddlSalesAndDistribution_SaleOffice.SelectedValue = dr["SalesOffice"].ToString();
                ddlSalesAndDistribution_SaleGroup.SelectedValue = dr["SalesGroup"].ToString();
                txtPartnerDataBox_Sold_ToParty.Text = dr["SoldToParty1"].ToString();
                //txtPartnerDataBox_Ship_ToParty.Text = dr["ResCatalogProfile"].ToString(); // unknow

                txtLicenseBox_LicenseNumber.Text = dr["LicenseNumber"].ToString();
            }



            DataTable dtwarranty = ServiceEquipment.getWarrantyData(SID,CompanyCode,EquipmentCode);
            foreach (DataRow dr in dtwarranty.Rows)
            {
                txtMaintenanceStartDate.Text = Validation.Convert2DateDisplay(dr["BeginGuarantee"].ToString());
                txtMaintenanceEndDate.Text = Validation.Convert2DateDisplay(dr["EndGuaranty"].ToString());
                txtPeriod.Text = dr["Period"].ToString();
                txtNextMaintenanceDate.Text = !string.IsNullOrEmpty(dr["NextMaintenanceDate"].ToString()) ? Validation.Convert2DateDisplay(dr["NextMaintenanceDate"].ToString()) : "";
                txtNextMaintenanceTime.Text = !string.IsNullOrEmpty(dr["NextMaintenanceTime"].ToString()) ? Validation.Convert2TimeDisplay(dr["NextMaintenanceTime"].ToString()).Substring(0,5) : "";
                txtLastMaintenanceDate.Text = !string.IsNullOrEmpty(Convert.ToString(dr["LastMaintenanceDate"])) ? Validation.Convert2DateDisplay(Convert.ToString(dr["LastMaintenanceDate"])) : "";
                txtLastMaintenanceTime.Text = !string.IsNullOrEmpty(Convert.ToString(dr["LastMaintenanceTime"])) ? Validation.Convert2TimeDisplay(Convert.ToString(dr["LastMaintenanceTime"])).Substring(0, 5) : "";
                txtWarrantyStartDate.Text = !string.IsNullOrEmpty(dr["BeginWarrantee"].ToString()) ? Validation.Convert2DateDisplay(dr["BeginWarrantee"].ToString()) : "";
                txtWarrantyEndDate.Text = !string.IsNullOrEmpty(dr["EndWarrantee"].ToString()) ? Validation.Convert2DateDisplay(dr["EndWarrantee"].ToString()) : "";
                txtDaySend.Text = dr["DaySend"].ToString();

                DataTable dtTrigger = ServiceEquipment.getWarrantyTrigger(SID, CompanyCode, EquipmentCode);
                foreach ( DataRow drr in dtTrigger.Rows)
                {
                    CheckBoxDaySend.Checked = true;
                    txtShowDaySend.Text = drr["SendBack"].ToString();
                }

                try
                {
                    ddlMaintenanceType.SelectedValue = dr["CategoryCode"].ToString();
                }
                catch (Exception)
                {
                    ddlMaintenanceType.SelectedIndex = 0;
                }
            }
           // //Edit 06/11/2561
           // DataTable dtMapping = ServiceEquipment.getMappingLineNumber(
           //    SID,
           //    CompanyCode,
           //    EquipmentCode
           //);
            //foreach (DataRow drr in dtMapping.Rows)
            //{
            //    //DropDownList1.SelectedValue = drr["LocationCode"].ToString();
            //    txtPlantID.Text = drr["Plant"].ToString();
            //    txtLocationID.Text = drr["Location"].ToString();
            //    txtRoomID.Text = drr["Room"].ToString();
            //    txtShelfID.Text = drr["Shelf"].ToString();
            //    txtWorkCenterID.Text = drr["WorkCenter"].ToString();
            //    txtSlotID.Text = drr["Slot"].ToString();
            //    txtAddressZip.Text = drr["AddressZipCode"].ToString();
            //    txtAddressCity.Text = drr["AddressCity"].ToString();
            //    txtAddressID1.Text = drr["AddressName1"].ToString();
            //    txtAddressID2.Text = drr["AddressName2"].ToString();
            //    txtStreetID.Text = drr["AddressStreet"].ToString();
            //    txtTelephoneID.Text = drr["AddressTelephone"].ToString();
            //    txtFaxID.Text = drr["AddressFax"].ToString();
            //    //Execute for check if data was change
            //    //DropDownList1Hidden.Text = drr["LocationCode"].ToString();
            //}

            UpdatePanel1.Update();//Edit 06/11/2561
            udppanelHeader.Update();
            udpGeneral.Update();
            //udpLocationDataBox_Location.Update();
            udpOganization.Update();
            udpPanelSale.Update();
            udpTableWarranty.Update();

            //bindDataTableOwnerAssignment();
            bindDataTableSerialDetail();
            //bindDataTicket();
        }

        #region bind ddl Data
        private void bindAllDropdown()
        {
            bindDataEquipmentType();
            BindDDLEMClass();
            
            BindDDLWeightUnit();

            //Edit 01/11/2561
            //BindDDLLocation();
            //setLastestLocation();
            BindDDLPlant();
            BindDDLStorageLocation();
            BindDDLWorkCenter();
            BindDDLBusinessArea();
            BindDDLAsset();
            BindDDL();
            BindDDLBillingDocType();
            BindDDLSalseOrganization();
            BindDDLDistChanal();
            BindDDLDivision();
            BindDDLSaleOffice();
            BindDDLSaleGroup();
            BindDDLStatus();
            BindDDLCoustCenter();
            BindDDl_OwnerService();
            BindDLLSLAGroup();
        }

        private void BindDDLEMClass()
        {
            DataTable dt = ServiceEquipment.getEMClass(SID);
            ddlEMClass.DataSource = dt;
            ddlEMClass.DataValueField = "ClassCode";
            ddlEMClass.DataTextField = "ClassName";
            ddlEMClass.DataBind();
        }
        private void bindDataEquipmentType()
        {
            DataTable dt = ServiceUniversal.getEquipmentType(SID, CompanyCode);
            ddlEquipmentType.DataSource = dt;
            ddlEquipmentType.DataBind();
        }

        private void BindDLLSLAGroup()
        {
            ddlSLAGroup.DataSource = dtSLAGroupKeyValue;
            ddlSLAGroup.DataBind();
            ddlSLAGroup.Items.Insert(0, new ListItem("เลือก", ""));
        }


        private void BindDDLWeightUnit()
        {
            DataTable dt = ServiceEquipment.getWeightMaster(SID);

            ddlGeneralBox_Weight_WeightUnit.DataValueField = "UCODE";
            ddlGeneralBox_Weight_WeightUnit.DataTextField = "DetailDescription";

            ddlGeneralBox_Weight_WeightUnit.DataSource = dt;
            ddlGeneralBox_Weight_WeightUnit.DataBind();

            ddlGeneralBox_Weight_WeightUnit.Items.Insert(0, new ListItem("เลือก", ""));
        }
        //Edit 01/11/2561
        //private void BindDDLLocation()
        //{            
        //    DataTable dt = ServiceEquipment.getLocationMaster(SID);

        //    DropDownList1.DataValueField = "LocationCode";
        //    DropDownList1.DataTextField = "LocateName";

        //    DropDownList1.DataSource = dt;
        //    DropDownList1.DataBind();

        //    DropDownList1.Items.Insert(0, new ListItem("เลือก", ""));

        //}

        //21/01/2562 set last locaton CI by born kk 
        //private void setLastestLocation() {
        //   DataTable dt =  ServiceEquipment.LogMappingMaxOfCI(SID,CompanyCode, EquipmentCode);
        //    foreach (DataRow row in dt.Rows) {
        //        DropDownList1.ClearSelection();
        //        DropDownList1.SelectedValue = row["LocationCode"].ToString();
        //    }
        //    DropDownList1_SelectedIndexChanged(null,EventArgs.Empty);
        //}
        #region Bind Un use dropdown
        private void BindDDLPlant()
        {
            //DataTable dt = ServiceEquipment.getPlantMaster(SID);

            //ddlLocationDataBox_Plant_Code.DataValueField = "PLANTCODE";
            //ddlLocationDataBox_Plant_Code.DataTextField = "DetailDescription";

            //ddlLocationDataBox_Plant_Code.DataSource = dt;
            //ddlLocationDataBox_Plant_Code.DataBind();

            //ddlLocationDataBox_Plant_Code.Items.Insert(0, new ListItem("เลือก", ""));

        }


        private void BindDDLStorageLocation()
        {
            //DataTable dt = ServiceEquipment.getStorageLocationMaster(
            //    SID,
            //    ddlLocationDataBox_Plant_Code.SelectedValue
            //);

            //ddlLocationDataBox_Location_Code.DataValueField = "STORAGELOCCODE";
            //ddlLocationDataBox_Location_Code.DataTextField = "DetailDescription";

            //ddlLocationDataBox_Location_Code.DataSource = dt;
            //ddlLocationDataBox_Location_Code.DataBind();

            //ddlLocationDataBox_Location_Code.Items.Insert(0, new ListItem("เลือก", ""));
        }

        private void BindDDLWorkCenter()
        {
            //DataTable dt = ServiceEquipment.getWorkCenterMaster(SID);

            //ddlLocationDataBox_WorkCenter_Code.DataValueField = "workCenter";
            //ddlLocationDataBox_WorkCenter_Code.DataTextField = "DetailDescription";

            //ddlLocationDataBox_WorkCenter_Code.DataSource = dt;
            //ddlLocationDataBox_WorkCenter_Code.DataBind();

            //ddlLocationDataBox_WorkCenter_Code.Items.Insert(0, new ListItem("เลือก", ""));
        }
        #endregion

        private void BindDDLBusinessArea()
        {
            DataTable dt = ServiceEquipment.getBusinessAreaMaster(SID);

            ddlAccountAssignmentBox_BusinessArea_Code.DataValueField = "BusinessAreaCode";
            ddlAccountAssignmentBox_BusinessArea_Code.DataTextField = "DetailDescription";

            ddlAccountAssignmentBox_BusinessArea_Code.DataSource = dt;
            ddlAccountAssignmentBox_BusinessArea_Code.DataBind();

            ddlAccountAssignmentBox_BusinessArea_Code.Items.Insert(0, new ListItem("เลือก", ""));
        }

        private void BindDDLAsset()
        {
            // To slow
            //string where = "#WHERE A.SID = '" + SID + "' AND A.CompanyCode = '" + CompanyCode + "'";
            //DataTable dt = ICMUtilHelper.GetSearchData(
            //    SID,
            //    "SHJASSETMASTER",
            //    where
            //);

            //foreach (DataRow dr in dt.Rows)
            //{
            //    dr["AssetSubCodeDescription"] = ObjectUtil.PrepareCodeAndDescription(dr["AssetCode"].ToString(), dr["AssetSubCodeDescription"].ToString());
            //}

            //ddlAccountAssignmentBox_Asset.DataValueField = "AssetCode";
            //ddlAccountAssignmentBox_Asset.DataTextField = "AssetSubCodeDescription";
            //ddlAccountAssignmentBox_Asset.DataSource = dt;
            //ddlAccountAssignmentBox_Asset.DataBind();
            //ddlAccountAssignmentBox_Asset.Items.Insert(0, new ListItem("เลือก", ""));
        }

        private void BindDataAsset()
        {
            // To slow
            string where = "#WHERE A.SID = '" + SID + "' AND A.CompanyCode = '" + CompanyCode + "'";
            DataTable dt = ICMUtilHelper.GetSearchData(
                SID,
                "SHJASSETMASTER",
                where
            );
            rptListAsset.DataSource = dt;
            rptListAsset.DataBind();
            udpAssetList.Update();
            ClientService.DoJavascript("openSearchHelpAsset();");
        }

        private void BindDDL()
        {
            //string where = "#WHERE A.SID = '" + SID + "' AND A.CompanyCode = '" + CompanyCode + "'";
            //DataTable dt = ICMUtilHelper.GetSearchData(
            //    SID,
            //    "SHJASSETMASTER",
            //    where
            //);

        }

        private void BindDDLBillingDocType()
        {
            DataTable dt = ServiceEquipment.getDoctypeMaster(
                SID,
                CompanyCode
            );

            ddlSalesAndDistribution_BillingDocTypeCode.DataValueField = "DocumentTypeCode";
            ddlSalesAndDistribution_BillingDocTypeCode.DataTextField = "DetailDescription";
            ddlSalesAndDistribution_BillingDocTypeCode.DataSource = dt;
            ddlSalesAndDistribution_BillingDocTypeCode.DataBind();
            ddlSalesAndDistribution_BillingDocTypeCode.Items.Insert(0, new ListItem("เลือก", ""));
        }

        private void BindDDLBillingDocNumber()
        {
            DataTable dt = ServiceEquipment.getBillingDocnumberByDoctype(
                SID,
                ddlSalesAndDistribution_BillingDocTypeCode.SelectedValue
            );

            ddlSalesAndDistribution_BillingDocNumber.DataValueField = "SaleDocument";
            ddlSalesAndDistribution_BillingDocNumber.DataTextField = "DetailDescription";
            ddlSalesAndDistribution_BillingDocNumber.DataSource = dt;
            ddlSalesAndDistribution_BillingDocNumber.DataBind();
            ddlSalesAndDistribution_BillingDocNumber.Items.Insert(0, new ListItem("เลือก", ""));
        }

        private void BindDDLSalseOrganization()
        {
            DataTable dt = ServiceEquipment.getSalseOrganizationMaster(SID);

            ddlSalesAndDistribution_SalseOrganization.DataValueField = "SORGCODE";
            ddlSalesAndDistribution_SalseOrganization.DataTextField = "DetailDescription";

            ddlSalesAndDistribution_SalseOrganization.DataSource = dt;
            ddlSalesAndDistribution_SalseOrganization.DataBind();

            ddlSalesAndDistribution_SalseOrganization.Items.Insert(0, new ListItem("เลือก", ""));
        }
        private void BindDDLDistChanal()
        {
            DataTable dt = ServiceEquipment.getDistChanalMaster(SID);

            ddlSalesAndDistribution_DistChanal.DataValueField = "SCHANNELCODE";
            ddlSalesAndDistribution_DistChanal.DataTextField = "DetailDescription";

            ddlSalesAndDistribution_DistChanal.DataSource = dt;
            ddlSalesAndDistribution_DistChanal.DataBind();

            ddlSalesAndDistribution_DistChanal.Items.Insert(0, new ListItem("เลือก", ""));
        }
        private void BindDDLDivision()
        {
            DataTable dt = ServiceEquipment.getDivisionMaster(SID);

            ddlSalesAndDistribution_Division.DataValueField = "SDIVCODE";
            ddlSalesAndDistribution_Division.DataTextField = "DetailDescription";

            ddlSalesAndDistribution_Division.DataSource = dt;
            ddlSalesAndDistribution_Division.DataBind();

            ddlSalesAndDistribution_Division.Items.Insert(0, new ListItem("เลือก", ""));
        }
        private void BindDDLSaleOffice()
        {
            DataTable dt = ServiceEquipment.getSaleOfficeMaster(SID);

            ddlSalesAndDistribution_SaleOffice.DataValueField = "SOFFICECODE";
            ddlSalesAndDistribution_SaleOffice.DataTextField = "DetailDescription";

            ddlSalesAndDistribution_SaleOffice.DataSource = dt;
            ddlSalesAndDistribution_SaleOffice.DataBind();

            ddlSalesAndDistribution_SaleOffice.Items.Insert(0, new ListItem("เลือก", ""));
        }
        private void BindDDLSaleGroup()
        {
            DataTable dt = ServiceEquipment.getSaleGroupMaster(SID);

            ddlSalesAndDistribution_SaleGroup.DataValueField = "SGROUPCODE";
            ddlSalesAndDistribution_SaleGroup.DataTextField = "DetailDescription";

            ddlSalesAndDistribution_SaleGroup.DataSource = dt;
            ddlSalesAndDistribution_SaleGroup.DataBind();

            ddlSalesAndDistribution_SaleGroup.Items.Insert(0, new ListItem("เลือก", ""));
        }
        private void BindDDLCoustCenter()
        {
            DataTable dt = ServiceEquipment.getCostcenterMaster(SID);

            ddlAccountAssignmentBox_CoustCenter.DataValueField = "COSTCENTERCODE";
            ddlAccountAssignmentBox_CoustCenter.DataTextField = "DetailDescription";

            ddlAccountAssignmentBox_CoustCenter.DataSource = dt;
            ddlAccountAssignmentBox_CoustCenter.DataBind();

            ddlAccountAssignmentBox_CoustCenter.Items.Insert(0, new ListItem("เลือก", ""));
        }
        private void BindDDLStatus()
        {
            //POSDocumentHelper.getSessionId(SID, UserName)
            Object[] objParam = new Object[] { "0800066", POSDocumentHelper.getSessionId(SID, UserName) };
            DataSet[] ds = new DataSet[] { };
            DataSet objReturn = icmUtil.ICMDataSetInvoke(objParam, ds);

            if (objReturn.Tables.Count > 0)
            {
                DataTable dt = objReturn.Tables[0].DefaultView.ToTable(true, "StatusCode", "StatusName");

                ddlStatus.DataTextField = "StatusName";
                ddlStatus.DataValueField = "StatusCode";
                ddlStatus.DataSource = dt;
                ddlStatus.DataBind();

                try
                {
                    ddlStatus.SelectedValue = "N";
                }
                catch (Exception) { }
            }
        }
        private void BindDDl_OwnerService()
        {
            ddlOwnerService.DataTextField = "OwnerGroupName";
            ddlOwnerService.DataValueField = "OwnerGroupCode";
            ddlOwnerService.DataSource = erpwMaster.GetMasterConfigOwnerGroup(SID, CompanyCode, "");
            ddlOwnerService.DataBind();
        }

        #endregion

        #endregion

        //Edit 06/11/2561       
        #region Save & Prepare Data
        protected void btnSaveEquipment_Click(object sender, EventArgs e)
        {

            /// แยกเป็น Function saveEquipmentLocationMapping() แทน

            #region Old Save
            try
            {
                #region Validate
                if (!string.IsNullOrEmpty(txtNextMaintenanceDate.Text) && !string.IsNullOrEmpty(txtLastMaintenanceDate.Text))
                {
                    Int64 dateNext = 0, dateLast = 0;
                    Int64.TryParse(Validation.Convert2DateDB(txtNextMaintenanceDate.Text), out dateNext);
                    Int64.TryParse(Validation.Convert2DateDB(txtLastMaintenanceDate.Text), out dateLast);
                    if (dateNext < dateLast)
                    {
                        throw new Exception("Next Maintenance ต้องมากกว่าหรือเท่ากับ Last Maintenance!");
                    }
                }

                #endregion

                PrepareDataEquipmentHeaderFirstLoad();
                //UploadGallery.SavePath = "/managefile/" + PublicAuthentication.SID + "/upload_files/equipment/";
                //DataTable dt = UploadGallery.SaveFiles();
                //foreach (DataRow dr in dt.Rows)
                //{
                //    dsEquipment.master_equipment.Rows[0]["PicturePart"] = dr["PhysicalFileName"].ToString();
                //}

                prepareDataEquipmentDetail();
                //PrepareDataTableOwnerAssignment();
                PrepareDataTableSerialDetail();
                UpdateProperties();
                if (dsEquipment.master_equipment_owner_assignment.Columns.Contains("_PagingService_RowNum"))
                {
                    dsEquipment.master_equipment_owner_assignment.Columns.Remove("_PagingService_RowNum");
                }
                saveDataEquipment();

                if (!string.IsNullOrEmpty(EquipmentCode))
                {
                    saveDataEquipmentWarranty(EquipmentCode);
                }
                //saveEquipmentLocationMapping();

                dsEquipment = new tmpEquipmentSetupDataSet();
                bindDefaultToScreen();
                ClientService.DoJavascript("upDateCheckBoxDaySend();");
                ClientService.DoJavascript("setValueID();");
                ClientService.AGSuccess("บันทึกสำเร็จ");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AGLoading(false);
            }
            #endregion
        }

        //private void saveEquipmentLocationMapping()
        //{
        //    if (DropDownList1Hidden.Text != DropDownList1.Text)
        //    {
        //        int GenItemNo = 0;
        //        DataTable dtCIMapping = new DataTable();
        //        DataRow drNewCIMapping = dtCIMapping.NewRow();
        //        string LineNumber = GenItemNo.ToString().PadLeft(3, '0');
        //        string dateTime = Validation.getCurrentServerStringDateTime();
        //        dtCIMapping.Columns.Add("SID");
        //        dtCIMapping.Columns.Add("CompanyCode");
        //        dtCIMapping.Columns.Add("LineNumber");
        //        dtCIMapping.Columns.Add("EquipmentCode");
        //        dtCIMapping.Columns.Add("LocationCode");
        //        dtCIMapping.Columns.Add("CREATED_BY");
        //        dtCIMapping.Columns.Add("CREATED_ON");
        //        drNewCIMapping["SID"] = SID;
        //        drNewCIMapping["CompanyCode"] = CompanyCode;
        //        drNewCIMapping["LineNumber"] = LineNumber;
        //        drNewCIMapping["EquipmentCode"] = EquipmentCode;
        //        drNewCIMapping["LocationCode"] = DropDownList1.SelectedValue;
        //        drNewCIMapping["CREATED_BY"] = EmployeeCode;
        //        drNewCIMapping["CREATED_ON"] = dateTime;
        //        dtCIMapping.Rows.Add(drNewCIMapping);
        //        ServiceEquipment.SaveCILocation(SID, CompanyCode,
        //            LineNumber, EquipmentCode, DropDownList1.SelectedValue, EmployeeCode, dateTime);
        //    }
        //    else
        //    {
        //        ClientService.AGSuccess("บันทึกสำเร็จ");
        //    }
        //}

        private string logChang()
        {
            DataTable dtLog = ServiceLog.GetEquipmentLogTop(
                    SID,
                    CompanyCode,
                    EquipmentCode
                );

            string log = "";
            foreach (DataRow dtRows in dtLog.Rows)
            {
                log = dtRows["LOGOBJCODE"].ToString();
            }
            return log;
        }

        private string saveDataEquipment()
        {
            string ResultCode = "";
            if (Page_Mode == null || !Page_Mode.Equals("Edit"))
            {
                Object[] objParam = new Object[] { "0800037", POSDocumentHelper.getSessionId(SID, UserName) };
                DataSet[] objDataSetChange = new DataSet[] { dsEquipment };
                Object Result = icmUtil.ICMPrimitiveInvoke(objParam, objDataSetChange);

                string[] arrResult = Result.ToString().Split('#');
                if (arrResult.Length == 2)
                {
                    if (arrResult[1].Equals("E"))
                    {

                    }
                    ResultCode = arrResult[1];
                }
                saveDataEquipmentWarranty(ResultCode);
                Response.Redirect("EquipmentDetail.aspx?code=" + ResultCode + "&mode=Edit");
            }
            else
            {
                Object[] objParam = new Object[] { "0800040", POSDocumentHelper.getSessionId(SID, UserName) };
                DataSet[] objDataSetChange = new DataSet[] { dsEquipment };
                
                string log = logChang();
                
                Object Result = icmUtil.ICMPrimitiveInvoke(objParam, objDataSetChange);

                string logTo = logChang();

                if (log != logTo)
                {
                    NotificationLibrary.GetInstance().TicketAlertEvent(
                        NotificationLibrary.EVENT_TYPE.CI_Change_Log,
                        SID,
                        CompanyCode,
                        EquipmentCode + "|" + log + "|" + logTo,
                        "System Auto",
                        ThisPage + "_CI_LOG"
                    );
                }

                string[] arrResult = Result.ToString().Split('#');
                if (arrResult.Length == 2)
                {
                    if (arrResult[0].Substring(0, 1).Equals("E"))
                    {
                        //throw new Exception(arrResult[1]);
                    }
                    else
                    {
                        //ResultCode = arrResult[1];
                    }
                }
            }

            return ResultCode;
        }
        private void saveDataEquipmentWarranty(string EquipmentCode)
        {
            #region Structure
            DataTable dtwarranty = new DataTable();
            dtwarranty.Columns.Add("SID");
            dtwarranty.Columns.Add("CompanyCode");
            dtwarranty.Columns.Add("EquipmentCode");
            dtwarranty.Columns.Add("CategoryCode");
            dtwarranty.Columns.Add("BeginGuarantee");
            dtwarranty.Columns.Add("EndGuaranty");
            dtwarranty.Columns.Add("BeginWarrantee");
            dtwarranty.Columns.Add("EndWarrantee");
            dtwarranty.Columns.Add("Period");
            dtwarranty.Columns.Add("NextMaintenanceDate");
            dtwarranty.Columns.Add("NextMaintenanceTime");
            dtwarranty.Columns.Add("LastMaintenanceDate");
            dtwarranty.Columns.Add("LastMaintenanceTime");
            dtwarranty.Columns.Add("CREATED_BY");
            dtwarranty.Columns.Add("UPDATED_BY");
            dtwarranty.Columns.Add("CREATED_ON");
            dtwarranty.Columns.Add("UPDATED_ON");
            dtwarranty.Columns.Add("DaySend");
            #endregion
            #region Warranty
            string dateTime = Validation.getCurrentServerStringDateTime();
            DataRow drNewWar = dtwarranty.NewRow();
            drNewWar["SID"] = SID;
            drNewWar["CompanyCode"] = CompanyCode;
            drNewWar["EquipmentCode"] = EquipmentCode;
            drNewWar["BeginGuarantee"] = Validation.Convert2DateDB(txtMaintenanceStartDate.Text);
            drNewWar["EndGuaranty"] = Validation.Convert2DateDB(txtMaintenanceEndDate.Text);
            Int16 nPeriod = 0;
            Int16.TryParse(txtPeriod.Text, out nPeriod);
            drNewWar["Period"] = nPeriod.ToString();
            drNewWar["CategoryCode"] = ddlMaintenanceType.SelectedValue;
            string NextMaintenanceDate = !string.IsNullOrEmpty(txtNextMaintenanceDate.Text) ? Validation.Convert2DateDB(txtNextMaintenanceDate.Text) : "";
            string NextMaintenanceTime = !string.IsNullOrEmpty(txtNextMaintenanceTime.Text) ? Validation.Convert2TimeDB(txtNextMaintenanceTime.Text + ":00") : "";
            string LastMaintenanceDate = !string.IsNullOrEmpty(txtLastMaintenanceDate.Text) ? Validation.Convert2DateDB(txtLastMaintenanceDate.Text) : "";
            string LastMaintenanceTime = !string.IsNullOrEmpty(txtLastMaintenanceTime.Text) ? Validation.Convert2TimeDB(txtLastMaintenanceTime.Text + ":00") : "";
            drNewWar["NextMaintenanceDate"] = NextMaintenanceDate;
            drNewWar["NextMaintenanceTime"] = NextMaintenanceTime;
            drNewWar["LastMaintenanceDate"] = LastMaintenanceDate;
            drNewWar["LastMaintenanceTime"] = LastMaintenanceTime;
            drNewWar["BeginWarrantee"] = Validation.Convert2DateDB(txtWarrantyStartDate.Text);
            drNewWar["EndWarrantee"] = Validation.Convert2DateDB(txtWarrantyEndDate.Text);
            drNewWar["CREATED_BY"] = EmployeeCode;
            drNewWar["CREATED_ON"] = dateTime;
            drNewWar["UPDATED_BY"] = EmployeeCode;
            drNewWar["UPDATED_ON"] = dateTime;
            drNewWar["DaySend"] = txtDaySend.Text;
            dtwarranty.Rows.Add(drNewWar);
            ServiceEquipment.saveWarrantyMaster(dtwarranty);

            if (CheckBoxDaySend.Checked == true && txtDaySend.Text != "")
            {
                var TimeSec = "";
                DataTable countwarranty = ServiceEquipment.countWarrantyData(SID, CompanyCode, EquipmentCode, txtDaySend.Text);
                foreach (DataRow dtRows in countwarranty.Rows)
                {
                   TimeSec = dtRows["TargetTime"].ToString();
                }
                var Year = DateTime.Now.Year.ToString();
                String TransactionID = Guid.NewGuid().ToString();
                TriggerService.GetInstance().EscalateTicket(
                    TransactionID, "CI", EquipmentCode, Year, TimeSec, UserName
                );

                TriggerService.GetInstance().CancelTriggerCI(
                    TransactionID, "CI", EquipmentCode
                );
            }
            else
            {
                TriggerService.GetInstance().CancelTriggerCI(
                    "", "CI", EquipmentCode
                );
            }
            #endregion
        }
        private void prepareDataEquipmentDetail()
        {
            string CreateDate = Validation.getCurrentServerStringDateTime();
            double weight = 0;
            double.TryParse(txtGeneralBox_Weight.Text, out weight);
            #region General
            if (dsEquipment.master_equipment_general.Rows.Count == 0)
            {
                DataRow drGeneral = dsEquipment.master_equipment_general.NewRow();

                drGeneral["SID"] = SID;
                drGeneral["CompanyCode"] = CompanyCode;
                //drGeneral["EquipmentCode"] = ""; // unknow
                //drGeneral["CategoryCode"] = ""; // unknow
                drGeneral["EquipmentClass"] = ddlEMClass.SelectedValue; // unknow
                drGeneral["EquipmentObjectType"] = ddlOwnerService.SelectedValue;
                drGeneral["AuthorizeGroup"] = txtManufacturerDataBox_AuthorizeGroup.Text; // unknow
                drGeneral["Weight"] = weight;
                drGeneral["WeightUnit"] = ddlGeneralBox_Weight_WeightUnit.SelectedValue;
                drGeneral["SizeDimension"] = txtGeneralBox_Size_Dimension.Text;
                drGeneral["InventoryNO"] = txtManufacturerDataBox_InventoryNO.Text; // unknow
                drGeneral["StartupDate"] = Validation.Convert2DateDB(txtGeneralBox_Start_UpDate.Text);
                drGeneral["AcquisitionValue"] = txtReferenceDataBox_AcquisitionValue.Text; 
                drGeneral["CategoryCode"] = txtReferenceDataBox_CategoryCode.Text; // unknow
                drGeneral["AcquisitionDate"] = Validation.Convert2DateDB(txtReferenceDataBox_AcquisitionDate.Text);
                drGeneral["ManufacturerNO"] = txtManufacturerDataBox_Manufacturer.Text;
                drGeneral["ModelNumber"] = txtManufacturerDataBox_ModelNo.Text;
                drGeneral["ManufacturerPartNO"] = txtManufacturerDataBox_MenuPartNo.Text;
                drGeneral["ManufacturerSerialNO"] = txtManufacturerDataBox_SerialNo.Text;
                drGeneral["ManufacturerCountry"] = txtManufacturerDataBox_ManufacturerCountry.Text;
                drGeneral["ConstructYear"] = txtManufacturerDataBox_Constr_Yr.Text;
                drGeneral["ConstructMonth"] = txtManufacturerDataBox_Constr_Mn.Text;
                drGeneral["CREATED_BY"] = EmployeeCode;
                drGeneral["CREATED_ON"] = CreateDate;

                dsEquipment.master_equipment_general.Rows.Add(drGeneral);
            }
            else
            {
                dsEquipment.master_equipment_general.Rows[0]["Weight"] = weight;
                dsEquipment.master_equipment_general.Rows[0]["WeightUnit"] = ddlGeneralBox_Weight_WeightUnit.SelectedValue;
                dsEquipment.master_equipment_general.Rows[0]["SizeDimension"] = txtGeneralBox_Size_Dimension.Text;
                dsEquipment.master_equipment_general.Rows[0]["InventoryNO"] = txtManufacturerDataBox_InventoryNO.Text; // unknow
                dsEquipment.master_equipment_general.Rows[0]["StartupDate"] = Validation.Convert2DateDB(txtGeneralBox_Start_UpDate.Text);
                dsEquipment.master_equipment_general.Rows[0]["AcquisitionValue"] = txtReferenceDataBox_AcquisitionValue.Text;
                dsEquipment.master_equipment_general.Rows[0]["CategoryCode"] = txtReferenceDataBox_CategoryCode.Text; // unknow
                dsEquipment.master_equipment_general.Rows[0]["AcquisitionDate"] = Validation.Convert2DateDB(txtReferenceDataBox_AcquisitionDate.Text);
                dsEquipment.master_equipment_general.Rows[0]["ManufacturerNO"] = txtManufacturerDataBox_Manufacturer.Text;
                dsEquipment.master_equipment_general.Rows[0]["AuthorizeGroup"] = txtManufacturerDataBox_AuthorizeGroup.Text; // unknow
                dsEquipment.master_equipment_general.Rows[0]["ModelNumber"] = txtManufacturerDataBox_ModelNo.Text;
                dsEquipment.master_equipment_general.Rows[0]["ManufacturerPartNO"] = txtManufacturerDataBox_MenuPartNo.Text;
                dsEquipment.master_equipment_general.Rows[0]["ManufacturerSerialNO"] = txtManufacturerDataBox_SerialNo.Text;
                dsEquipment.master_equipment_general.Rows[0]["ManufacturerCountry"] = txtManufacturerDataBox_ManufacturerCountry.Text;
                dsEquipment.master_equipment_general.Rows[0]["ConstructYear"] = txtManufacturerDataBox_Constr_Yr.Text;
                dsEquipment.master_equipment_general.Rows[0]["ConstructMonth"] = txtManufacturerDataBox_Constr_Mn.Text;
                dsEquipment.master_equipment_general.Rows[0]["EquipmentClass"] = ddlEMClass.SelectedValue; // unknow
                dsEquipment.master_equipment_general.Rows[0]["EquipmentObjectType"] = ddlOwnerService.SelectedValue;
            }

            #endregion

            #region Location
            if (dsEquipment.master_equipment_location.Rows.Count == 0)
            {
                DataRow drLocation = dsEquipment.master_equipment_location.NewRow();

                drLocation["SID"] = SID;
                drLocation["CompanyCode"] = CompanyCode;
                //drLocation["EquipmentCode"] = ""; // unknow
                //drLocation["CategoryCode"] = ""; // unknow
                drLocation["MainPlant"] = txtLocationDataBox_Flow_Phase.Text; // Zaan // ddlLocationDataBox_Plant_Code.SelectedValue;
                drLocation["PlantSection"] = txtLocationDataBox_Shelf.Text;
                drLocation["Location"] = txtLocationDataBox_Location.Text; // Zaan // ddlLocationDataBox_Location_Code.SelectedValue;
                drLocation["Room"] = txtLocationDataBox_Room.Text;
                drLocation["WorkCenter"] = txtLocationDataBox_Cabinet.Text; // Zaan // ddlLocationDataBox_WorkCenter_Code.SelectedValue;
                drLocation["CategoryCode"] = txtLocationDataBox_CategoryCode.Text; // Bank // For ITG
                drLocation["ABCIndication"] = 0; // unknow
                drLocation["SortField"] = txtLocationDataBox_Slot.Text; // unknow
                drLocation["AddressName1"] = txtAddressBox_Name_1.Text;
                drLocation["AddressName2"] = txtAddressBox_Name_2.Text;
                drLocation["AddressStreet"] = txtAddressBox_Street.Text;
                drLocation["AddressZip"] = txtAddressBox_Address_Zip.Text;
                drLocation["AddressCity"] = txtAddressBox_Address_City.Text;
                drLocation["AddressCountryCode1"] = txtAddressBox_Address_Code_1.Text;
                drLocation["AddressCountryCode2"] = txtAddressBox_Address_Code_2.Text;
                drLocation["AddressTelephone"] = txtAddressBox_Telephone.Text;
                drLocation["AddressFax"] = txtAddressBox_Fax.Text;
                drLocation["CREATED_BY"] = EmployeeCode;
                drLocation["CREATED_ON"] = CreateDate;

                dsEquipment.master_equipment_location.Rows.Add(drLocation);
            }
            else
            {
                dsEquipment.master_equipment_location.Rows[0]["MainPlant"] = txtLocationDataBox_Flow_Phase.Text; // Zaan // ddlLocationDataBox_Plant_Code.SelectedValue;
                dsEquipment.master_equipment_location.Rows[0]["PlantSection"] = txtLocationDataBox_Shelf.Text;
                dsEquipment.master_equipment_location.Rows[0]["Location"] = txtLocationDataBox_Location.Text; // Zaan // ddlLocationDataBox_Location_Code.SelectedValue;
                dsEquipment.master_equipment_location.Rows[0]["Room"] = txtLocationDataBox_Room.Text;
                dsEquipment.master_equipment_location.Rows[0]["WorkCenter"] = txtLocationDataBox_Cabinet.Text; // Zaan // ddlLocationDataBox_WorkCenter_Code.SelectedValue;
                dsEquipment.master_equipment_location.Rows[0]["CategoryCode"] = txtLocationDataBox_CategoryCode.Text; // Bank // For ITG
                dsEquipment.master_equipment_location.Rows[0]["ABCIndication"] = 0; // unknow
                dsEquipment.master_equipment_location.Rows[0]["SortField"] = txtLocationDataBox_Slot.Text; // unknow
                dsEquipment.master_equipment_location.Rows[0]["AddressName1"] = txtAddressBox_Name_1.Text;
                dsEquipment.master_equipment_location.Rows[0]["AddressName2"] = txtAddressBox_Name_2.Text;
                dsEquipment.master_equipment_location.Rows[0]["AddressStreet"] = txtAddressBox_Street.Text;
                dsEquipment.master_equipment_location.Rows[0]["AddressZip"] = txtAddressBox_Address_Zip.Text;
                dsEquipment.master_equipment_location.Rows[0]["AddressCity"] = txtAddressBox_Address_City.Text;
                dsEquipment.master_equipment_location.Rows[0]["AddressCountryCode1"] = txtAddressBox_Address_Code_1.Text;
                dsEquipment.master_equipment_location.Rows[0]["AddressCountryCode2"] = txtAddressBox_Address_Code_2.Text;
                dsEquipment.master_equipment_location.Rows[0]["AddressTelephone"] = txtAddressBox_Telephone.Text;
                dsEquipment.master_equipment_location.Rows[0]["AddressFax"] = txtAddressBox_Fax.Text;
            }

            #endregion

            #region Oganization
            if (dsEquipment.master_equipment_organization.Rows.Count == 0)
            {
                DataRow drOganization = dsEquipment.master_equipment_organization.NewRow();

                drOganization["SID"] = SID;
                drOganization["CompanyCode"] = CompanyCode;
                drOganization["AccCompanyCode"] = ""; // unknow
                drOganization["AccBusinessArea"] = ddlAccountAssignmentBox_BusinessArea_Code.SelectedValue;

                //drOganization["AccAsset1"] = ddlAccountAssignmentBox_Asset.SelectedValue;
                drOganization["AccAsset1"] = hddAccountAssignmentBox_AssetCode.Value;
                
                //drOganization["AccAsset2"] = ddlAccountAssignmentBox_Asset.SelectedItem.Text;
                drOganization["AccCostCenter1"] = ddlAccountAssignmentBox_CoustCenter.SelectedValue;
                drOganization["AccCostCenter2"] = ddlAccountAssignmentBox_CoustCenter.SelectedItem.Text;
                drOganization["AccWBSElement"] = ""; // unknow
                drOganization["AccStandgOrder"] = ""; // unknow
                drOganization["AccSettleOrder"] = ""; // unknow
                drOganization["ResPlanningPlant"] = txtResponsibilitiesBox_PlanningPlant.Text;
                drOganization["ResPlannerGroup"] = txtResponsibilitiesBox_PlannerGroup.Text;
                drOganization["ResMainWorkCtr1"] = txtResponsibilitiesBox_MainWorkCtr_1.Text;
                drOganization["ResMainWorkCtr2"] = txtResponsibilitiesBox_MainWorkCtr_2.Text;
                drOganization["ResCatalogProfile"] = txtResponsibilitiesBox_CatalogProfile.Text;
                drOganization["CREATED_BY"] = EmployeeCode;
                drOganization["CREATED_ON"] = CreateDate;

                dsEquipment.master_equipment_organization.Rows.Add(drOganization);
            }
            else
            {
                dsEquipment.master_equipment_organization.Rows[0]["AccCompanyCode"] = ""; // unknow
                dsEquipment.master_equipment_organization.Rows[0]["AccBusinessArea"] = ddlAccountAssignmentBox_BusinessArea_Code.SelectedValue;

                //dsEquipment.master_equipment_organization.Rows[0]["AccAsset1"] = ddlAccountAssignmentBox_Asset.SelectedValue;
                dsEquipment.master_equipment_organization.Rows[0]["AccAsset1"] = hddAccountAssignmentBox_AssetCode.Value;

                //dsEquipment.master_equipment_organization.Rows[0]["AccAsset2"] = ddlAccountAssignmentBox_Asset.SelectedItem.Text;
                dsEquipment.master_equipment_organization.Rows[0]["AccCostCenter1"] = ddlAccountAssignmentBox_CoustCenter.SelectedValue;
                dsEquipment.master_equipment_organization.Rows[0]["AccCostCenter2"] = "";
                dsEquipment.master_equipment_organization.Rows[0]["AccWBSElement"] = ""; // unknow
                dsEquipment.master_equipment_organization.Rows[0]["AccStandgOrder"] = ""; // unknow
                dsEquipment.master_equipment_organization.Rows[0]["AccSettleOrder"] = ""; // unknow
                dsEquipment.master_equipment_organization.Rows[0]["ResPlanningPlant"] = txtResponsibilitiesBox_PlanningPlant.Text;
                dsEquipment.master_equipment_organization.Rows[0]["ResPlannerGroup"] = txtResponsibilitiesBox_PlannerGroup.Text;
                dsEquipment.master_equipment_organization.Rows[0]["ResMainWorkCtr1"] = txtResponsibilitiesBox_MainWorkCtr_1.Text;
                dsEquipment.master_equipment_organization.Rows[0]["ResMainWorkCtr2"] = txtResponsibilitiesBox_MainWorkCtr_2.Text;
                dsEquipment.master_equipment_organization.Rows[0]["ResCatalogProfile"] = txtResponsibilitiesBox_CatalogProfile.Text;
            }

            #endregion

            #region Sale & dist
            // Zaan Comment Code 04.12.2018
            //if (dsEquipment.master_equipment_sale_dist.Rows.Count == 0)
            //{
            //    DataRow drSalse_Dist = dsEquipment.master_equipment_sale_dist.NewRow();

            //    drSalse_Dist["SID"] = SID;
            //    drSalse_Dist["CompanyCode"] = CompanyCode;
            //    drSalse_Dist["SalesOrgCode"] = ddlSalesAndDistribution_SalseOrganization.SelectedValue;
            //    drSalse_Dist["DistChannel"] = ddlSalesAndDistribution_DistChanal.SelectedValue;
            //    drSalse_Dist["Division"] = ddlSalesAndDistribution_Division.SelectedValue;
            //    drSalse_Dist["SalesOffice"] = ddlSalesAndDistribution_SaleOffice.SelectedValue;
            //    drSalse_Dist["SalesGroup"] = ddlSalesAndDistribution_SaleGroup.SelectedValue;
            //    drSalse_Dist["LicenseNumber"] = txtLicenseBox_LicenseNumber.Text;
            //    drSalse_Dist["SoldToParty1"] = txtPartnerDataBox_Sold_ToParty.Text;
            //    drSalse_Dist["SoldToParty2"] = ""; // unknow
            //    drSalse_Dist["SoldToParty3"] = ""; // unknow
            //    drSalse_Dist["BLDocnumber"] = ddlSalesAndDistribution_BillingDocNumber.SelectedValue;
            //    drSalse_Dist["BLDoctype"] = ddlSalesAndDistribution_BillingDocTypeCode.SelectedValue;
            //    drSalse_Dist["BLFiscalyear"] = txtSalesAndDistribution_BillingDocYear.Text;
            //    drSalse_Dist["CREATED_BY"] = EmployeeCode;
            //    drSalse_Dist["CREATED_ON"] = CreateDate;

            //    dsEquipment.master_equipment_sale_dist.Rows.Add(drSalse_Dist);
            //}
            //else
            //{
            //    dsEquipment.master_equipment_sale_dist.Rows[0]["SalesOrgCode"] = ddlSalesAndDistribution_SalseOrganization.SelectedValue;
            //    dsEquipment.master_equipment_sale_dist.Rows[0]["DistChannel"] = ddlSalesAndDistribution_DistChanal.SelectedValue;
            //    dsEquipment.master_equipment_sale_dist.Rows[0]["Division"] = ddlSalesAndDistribution_Division.SelectedValue;
            //    dsEquipment.master_equipment_sale_dist.Rows[0]["SalesOffice"] = ddlSalesAndDistribution_SaleOffice.SelectedValue;
            //    dsEquipment.master_equipment_sale_dist.Rows[0]["SalesGroup"] = ddlSalesAndDistribution_SaleGroup.SelectedValue;
            //    dsEquipment.master_equipment_sale_dist.Rows[0]["LicenseNumber"] = txtLicenseBox_LicenseNumber.Text;
            //    dsEquipment.master_equipment_sale_dist.Rows[0]["SoldToParty1"] = txtPartnerDataBox_Sold_ToParty.Text;
            //    dsEquipment.master_equipment_sale_dist.Rows[0]["SoldToParty2"] = ""; // unknow
            //    dsEquipment.master_equipment_sale_dist.Rows[0]["SoldToParty3"] = ""; // unknow
            //    dsEquipment.master_equipment_sale_dist.Rows[0]["BLDocnumber"] = ddlSalesAndDistribution_BillingDocNumber.SelectedValue;
            //    dsEquipment.master_equipment_sale_dist.Rows[0]["BLDoctype"] = ddlSalesAndDistribution_BillingDocTypeCode.SelectedValue;
            //    dsEquipment.master_equipment_sale_dist.Rows[0]["BLFiscalyear"] = txtSalesAndDistribution_BillingDocYear.Text;
            //}

            #endregion

        }

        private void PrepareDataTableOwnerAssignment()
        {
            string CreateDate = Validation.getCurrentServerStringDateTime();

            #region owner assignment

            //foreach (RepeaterItem Items in rptTableOwnerAssignment.Items)
            //{
            //    Label lblOwnerAssignmentBox_ItemNo = Items.FindControl("lblOwnerAssignmentBox_ItemNo") as Label;
            //    DropDownList ddlOwnerAssignmentBox_OwnerType = Items.FindControl("ddlOwnerAssignmentBox_OwnerType") as DropDownList;
            //    TextBox txtOwnerAssignmentBox_BeginDate = Items.FindControl("txtOwnerAssignmentBox_BeginDate") as TextBox;
            //    TextBox txtOwnerAssignmentBox_EndDate = Items.FindControl("txtOwnerAssignmentBox_EndDate") as TextBox;
            //    CheckBox chkOwnerAssignmentBox_ActiveStatus = Items.FindControl("chkOwnerAssignmentBox_ActiveStatus") as CheckBox;
            //    AutoCompleteControl Complete_OwnerAssignmentBox_CustomerSelect = Items.FindControl("Complete_OwnerAssignmentBox_CustomerSelect") as AutoCompleteControl;
            //    DropDownList ddlSLAGroup = Items.FindControl("ddlSLAGroup") as DropDownList;
            //    //DropDownList ddlTicketType = Items.FindControl("ddlTicketType") as DropDownList;

            //    foreach (DataRow drOwner in dsEquipment.master_equipment_owner_assignment.Select("LineNumber = '" + lblOwnerAssignmentBox_ItemNo.Text + "'"))
            //    {
            //        drOwner["SID"] = SID;
            //        drOwner["CompanyCode"] = CompanyCode;
            //        drOwner["LineNumber"] = lblOwnerAssignmentBox_ItemNo.Text;
            //        drOwner["ActiveStatus"] = chkOwnerAssignmentBox_ActiveStatus.Checked ? "True" : "False";
            //        drOwner["BeginDate"] = Validation.Convert2DateDB(txtOwnerAssignmentBox_BeginDate.Text);
            //        drOwner["EndDate"] = Validation.Convert2DateDB(txtOwnerAssignmentBox_EndDate.Text);
            //        drOwner["OwnerCode"] = Complete_OwnerAssignmentBox_CustomerSelect.SelectValue;
            //        drOwner["OwnerDesc"] = Complete_OwnerAssignmentBox_CustomerSelect.SelectText;
            //        drOwner["OwnerType"] = ddlOwnerAssignmentBox_OwnerType.SelectedValue;
            //        drOwner["CREATED_ON"] = CreateDate;
            //        drOwner["CREATED_BY"] = EmployeeCode;
            //        drOwner["SLAGroupCode"] = ddlSLAGroup.SelectedValue;
            //        //drOwner["TicketType"] = ddlTicketType.SelectedValue;
            //    }
            //}


            //foreach (RepeaterItem Items in rptOwnerAssignmentList.Items)
            //{
            //    Label lblOwnerAssignmentBox_ItemNo = Items.FindControl("lblOwnerAssignmentBox_ItemNo") as Label;
            //    HiddenField hddOwnerAssignmentBox_OwnerType = Items.FindControl("hddOwnerAssignmentBox_OwnerType") as HiddenField;
            //    HiddenField hddOwnerAssignmentBox_OwnerCode = Items.FindControl("hddOwnerAssignmentBox_OwnerCode") as HiddenField;
            //    HiddenField hddOwnerAssignmentBox_BeginDate = Items.FindControl("hddOwnerAssignmentBox_BeginDate") as HiddenField;
            //    HiddenField hddOwnerAssignmentBox_EndDate = Items.FindControl("hddOwnerAssignmentBox_EndDate") as HiddenField;
            //    HiddenField hddOwnerAssignmentBox_SLAGroup = Items.FindControl("hddOwnerAssignmentBox_SLAGroup") as HiddenField;
            //    HiddenField hddOwnerAssignmentBox_ActiveStatus = Items.FindControl("hddOwnerAssignmentBox_ActiveStatus") as HiddenField;

            //    foreach (DataRow drOwner in dsEquipment.master_equipment_owner_assignment.Select("LineNumber = '" + lblOwnerAssignmentBox_ItemNo.Text + "'"))
            //    {
            //        drOwner["SID"] = SID;
            //        drOwner["CompanyCode"] = CompanyCode;
            //        drOwner["LineNumber"] = lblOwnerAssignmentBox_ItemNo.Text;
            //        drOwner["ActiveStatus"] = hddOwnerAssignmentBox_ActiveStatus.Value;
            //        drOwner["BeginDate"] = hddOwnerAssignmentBox_BeginDate.Value;
            //        drOwner["EndDate"] = hddOwnerAssignmentBox_EndDate.Value;
            //        drOwner["OwnerCode"] = hddOwnerAssignmentBox_OwnerCode.Value;
            //        drOwner["OwnerDesc"] = hddOwnerAssignmentBox_OwnerCode.Value;
            //        drOwner["OwnerType"] = hddOwnerAssignmentBox_OwnerType.Value;
            //        drOwner["CREATED_ON"] = CreateDate;
            //        drOwner["CREATED_BY"] = EmployeeCode;
            //        drOwner["SLAGroupCode"] = hddOwnerAssignmentBox_SLAGroup.Value;
            //    }
            //}
            #endregion
        }



        private void PrepareDataTableSerialDetail()
        {
            string CreateDate = Validation.getCurrentServerStringDateTime();

            foreach (RepeaterItem Items in rptTableSerialData.Items)
            {
                Label lblTableSerialData_ItemNo = Items.FindControl("lblTableSerialData_ItemNo") as Label;
                DropDownList ddlTableSerialData_SerialNo = Items.FindControl("ddlTableSerialData_SerialNo") as DropDownList;
                TextBox txtTableSerialData_EffectiveFrom = Items.FindControl("txtTableSerialData_EffectiveFrom") as TextBox;
                TextBox txtTableSerialData_EffectiveTo = Items.FindControl("txtTableSerialData_EffectiveTo") as TextBox;
                CheckBox chkTableSerialData_ActiveStatus = Items.FindControl("chkTableSerialData_ActiveStatus") as CheckBox;

                AutoCompleteControl Complete_TableSerialData_MaterialCode = Items.FindControl("Complete_TableSerialData_MaterialCode") as AutoCompleteControl;

                foreach (DataRow dr in dsEquipment.master_equipment_serial_detail.Select("LineNumber = '" + lblTableSerialData_ItemNo.Text + "'"))
                {
                    dr["SID"] = SID;
                    dr["CompanyCode"] = CompanyCode;
                    dr["LineNumber"] = lblTableSerialData_ItemNo.Text;
                    dr["EffectiveFrom"] = Validation.Convert2DateDB(txtTableSerialData_EffectiveFrom.Text);
                    dr["EffectiveTo"] = Validation.Convert2DateDB(txtTableSerialData_EffectiveTo.Text);
                    dr["ActiveStatus"] = chkTableSerialData_ActiveStatus.Checked ? "True" : "False";
                    dr["MaterialCode"] = Complete_TableSerialData_MaterialCode.SelectValue;
                    dr["MaterialName"] = Complete_TableSerialData_MaterialCode.SelectText;
                    dr["SerialNo"] = ddlTableSerialData_SerialNo.SelectedValue;
                    dr["CREATED_BY"] = EmployeeCode;
                    dr["CREATED_ON"] = CreateDate;
                }
            }
        }
        #endregion

        #region Table Owner Assignment

        #region bind Table Owner Assignment

        #region Get DataTable For Set To screen

        //private DataTable _dtCustomer;
        //private DataTable dtCustomer
        //{
        //    get
        //    {
        //        if (_dtCustomer == null)
        //        {
        //            _dtCustomer = AfterSaleService.getInstance().GetCustomerData();
        //        }
        //        return _dtCustomer;
        //    }
        //}

        //private DataTable _dtVendor;
        //private DataTable dtVendor
        //{
        //    get
        //    {
        //        if (_dtVendor == null)
        //        {
        //            _dtVendor = AfterSaleService.getInstance().getSearchVendor(
        //                SID,
        //                CompanyCode,
        //                ""
        //            );
        //        }
        //        return _dtVendor;
        //    }
        //}

        //private DataTable _dtEmployee;
        //private DataTable dtEmployee 
        //{
        //    get
        //    {
        //        if (_dtEmployee == null)
        //        {
        //            _dtEmployee = empService.searchEmployee(SID, CompanyCode, "");
        //        }
        //        return _dtEmployee;
        //    }
        //}

        //private DataTable _dtSoldToParty;
        //private DataTable dtSoldToParty 
        //{
        //    get
        //    {
        //        if (_dtSoldToParty == null)
        //        {
        //            _dtSoldToParty = ServiceEquipment.getCustomerSoldToPartyRefEquipment(SID, CompanyCode, EquipmentCode);
        //        }
        //        return _dtSoldToParty;
        //    }
        //}

        //private DataTable _dtShipToParty;
        //private DataTable dtShipToParty
        //{
        //    get
        //    {
        //        if (_dtShipToParty == null)
        //        {
        //            _dtShipToParty = ServiceEquipment.getCustomerShipToPartyRefEquipment(SID, CompanyCode, EquipmentCode);
        //        }
        //        return _dtShipToParty;
        //    }
        //}

        //private DataTable _dtBillToParty;
        //private DataTable dtBillToParty
        //{
        //    get
        //    {
        //        if (_dtBillToParty == null)
        //        {
        //            _dtBillToParty = ServiceEquipment.getCustomerBillToPartyRefEquipment(SID, CompanyCode, EquipmentCode);
        //        }
        //        return _dtBillToParty;
        //    }
        //}

        //private DataTable _dtTicketType;
        //private DataTable dtTicketType
        //{
        //    get 
        //    {
        //        if (_dtTicketType == null)
        //        {
        //            _dtTicketType = AfterSaleService.getInstance().getSearchDoctype("", CompanyCode);
        //        }
        //        return _dtTicketType;
        //    }
        //}


        //private DataTable _dtSLAGroup;
        //private DataTable dtSLAGroup
        //{
        //    get
        //    {
        //        if (_dtSLAGroup == null)
        //        {
        //            _dtSLAGroup = AfterSaleService.getInstance().GetTierGroup(SID);
        //        }

        //        return _dtSLAGroup;
        //    }
        //}

        private List<KeyValue> _dtCustomerKeyValue;
        private List<KeyValue> dtCustomerKeyValue
        {
            get
            {
                if (_dtCustomerKeyValue == null)
                {
                    _dtCustomerKeyValue = AfterSaleService.getInstance().GetCustomerDataKeyValue(SID, CompanyCode);
                }
                return _dtCustomerKeyValue;
            }
        }

        private List<KeyValue> _dtVendorKeyValue;
        private List<KeyValue> dtVendorKeyValue
        {
            get
            {
                if (_dtVendorKeyValue == null)
                {
                    _dtVendorKeyValue = AfterSaleService.getInstance().getSearchVendorKeyValue(
                        SID,
                        CompanyCode
                    );
                }
                return _dtVendorKeyValue;
            }
        }

        private List<KeyValue> _dtEmployeeKeyValue;
        private List<KeyValue> dtEmployeeKeyValue
        {
            get
            {
                if (_dtEmployeeKeyValue == null)
                {
                    _dtEmployeeKeyValue = AfterSaleService.getInstance().getSearchEmployeeKeyValue(
                        SID,
                        CompanyCode
                    );
                }
                return _dtEmployeeKeyValue;
            }
        }

        private List<KeyValue> _dtShipToPartyKeyValue;
        private List<KeyValue> dtShipToPartyKeyValue
        {
            get
            {
                if (_dtShipToPartyKeyValue == null)
                {
                    _dtShipToPartyKeyValue = AfterSaleService.getInstance().getCustomerShipToPartyRefEquipment(SID, CompanyCode, EquipmentCode);
                }
                return _dtShipToPartyKeyValue;
            }
        }

        private List<KeyValue> _dtSoldToPartyKeyValue;
        private List<KeyValue> dtSoldToPartyKeyValue
        {
            get
            {
                if (_dtSoldToPartyKeyValue == null)
                {
                    _dtSoldToPartyKeyValue = AfterSaleService.getInstance().getCustomerSoldToPartyRefEquipment(SID, CompanyCode, EquipmentCode);
                }
                return _dtSoldToPartyKeyValue;
            }
        }

        private List<KeyValue> _dtBillToPartyKeyValue;
        private List<KeyValue> dtBillToPartyKeyValue
        {
            get
            {
                if (_dtBillToPartyKeyValue == null)
                {
                    _dtBillToPartyKeyValue = AfterSaleService.getInstance().getCustomerBillToPartyRefEquipment(SID, CompanyCode, EquipmentCode);
                }
                return _dtBillToPartyKeyValue;
            }
        }

        private List<KeyValue> _dtSLAGroupKeyValue;
        private List<KeyValue> dtSLAGroupKeyValue
        {
            get
            {
                if (_dtSLAGroupKeyValue == null)
                {
                    _dtSLAGroupKeyValue = AfterSaleService.getInstance().GetTierGroupKeyValue(SID);
                }

                return _dtSLAGroupKeyValue;
            }
        }

        private List<KeyValue> _datasOwnerType;
        private List<KeyValue> datasOwnerType
        {
            get
            {
                if (_datasOwnerType == null)
                {
                    _datasOwnerType = new List<KeyValue>();
                    _datasOwnerType.Add(new KeyValue { Key = "01", Value = "Customer" });
                    _datasOwnerType.Add(new KeyValue { Key = "02", Value = "Vendor" });
                    _datasOwnerType.Add(new KeyValue { Key = "03", Value = "Employee" });
                    _datasOwnerType.Add(new KeyValue { Key = "ST", Value = "SoldToParty" });
                    _datasOwnerType.Add(new KeyValue { Key = "SH", Value = "ShipToParty" });
                    _datasOwnerType.Add(new KeyValue { Key = "BP", Value = "BillToParty" });
                }
                return _datasOwnerType;
            }
        }

        public string getDescriptionOwnerType(string OwnerTypeCode)
        {
            string desc = "";
            var data = datasOwnerType.Where(w => w.Key == OwnerTypeCode);

            if (data.Count() > 0)
                desc = data.First().Value;
            else
                desc = OwnerTypeCode;

            return desc;
        }

        public string getDescriptionOwnerCode(string OwnerTypeCode, string OwnerCode)
        {
            string desc = OwnerCode;

            if (OwnerTypeCode == "01")//Customer
            {
                var data = dtCustomerKeyValue.Where(w => w.Key == OwnerCode);
                if (data.Count() > 0)
                    desc += " : " + data.First().Value;
            }
            else if (OwnerTypeCode == "02")//Vendor
            {
                var data = dtVendorKeyValue.Where(w => w.Key == OwnerCode);
                if (data.Count() > 0)
                    desc += " : " + data.First().Value;
            }
            else if (OwnerTypeCode == "03")//Employee
            {
                var data = dtEmployeeKeyValue.Where(w => w.Key == OwnerCode);
                if (data.Count() > 0)
                    desc += " : " + data.First().Value;
            }
            else if (OwnerTypeCode == "ST")
            {
                var data = dtShipToPartyKeyValue.Where(w => w.Key == OwnerCode);
                if (data.Count() > 0)
                    desc += " : " + data.First().Value;
            }
            else if (OwnerTypeCode == "SH")
            {
                var data = dtSoldToPartyKeyValue.Where(w => w.Key == OwnerCode);
                if (data.Count() > 0)
                    desc += " : " + data.First().Value;
            }
            else if (OwnerTypeCode == "BP")
            {
                var data = dtBillToPartyKeyValue.Where(w => w.Key == OwnerCode);
                if (data.Count() > 0)
                    desc += " : " + data.First().Value;
            }


            return desc;
        }

        public string getDescriptionSLAGroup(string SLAGroupCode)
        {
            string desc = "";
            var data = dtSLAGroupKeyValue.Where(w => w.Key == SLAGroupCode);

            if (data.Count() > 0)
                desc = data.First().Value;
            else
                desc = SLAGroupCode;

            return desc;
        }

        public string getDescriptionAsset(string AssetCode)
        {
            if (string.IsNullOrEmpty(AssetCode))
            {
                return "";
            }

            string desc = "";
            var data = AfterSaleService.getInstance().getAssetKeyValue(SID, CompanyCode, AssetCode);

            if (data.Count() > 0)
                desc = AssetCode + " : " + data.First().Value;
            else
                desc = AssetCode;

            return desc;
        }
        #endregion

        private void bindDataTableOwnerAssignment()
        {
            //rptTableOwnerAssignment.DataSource = dsEquipment.master_equipment_owner_assignment;
            //rptTableOwnerAssignment.DataBind();
            //udpTableOwnerAssignment.Update();
            //txtDatasOwnerAssignment.Text = JsonConvert.SerializeObject(dsEquipment.master_equipment_owner_assignment);

            dsEquipment.master_equipment_owner_assignment.DefaultView.Sort = "LineNumber asc";
            DataTable dtCopyDataOwnerAssign = dsEquipment.master_equipment_owner_assignment.Copy();
            dtCopyDataOwnerAssign.AcceptChanges();
            string JsonDataOwner = JsonConvert.SerializeObject(dtCopyDataOwnerAssign);
            List<MasterEquipmentOwnerAssignmentModel> datas = JsonConvert.DeserializeObject<List<MasterEquipmentOwnerAssignmentModel>>(JsonDataOwner);

            datas.ForEach(r =>
            {
                r.OwnerTypeDesc = getDescriptionOwnerType(r.OwnerType);
                r.OwnerCodeDesc = getDescriptionOwnerCode(r.OwnerType, r.OwnerCode);
                r.SLAGroupDesc = getDescriptionSLAGroup(r.SLAGroupCode);

                if (!string.IsNullOrEmpty(r.BeginDate))
                {
                    r.BeginDate = Validation.Convert2DateDisplay(r.BeginDate);
                }
                if (!string.IsNullOrEmpty(r.EndDate))
                {
                    r.EndDate = Validation.Convert2DateDisplay(r.EndDate);
                }
            });

            var datasFinal = datas.Select(s => new { 
                s.LineNumber, 
                s.OwnerTypeDesc, 
                s.OwnerCodeDesc, 
                s.BeginDate, 
                s.EndDate, 
                s.SLAGroupDesc, 
                s.ActiveStatus 
            });

            panel_data_owner_assign.InnerHtml = JsonConvert.SerializeObject(datasFinal);


            //rptOwnerAssignmentList.DataSource = dsEquipment.master_equipment_owner_assignment;
            //rptOwnerAssignmentList.DataBind();

            udpTableOwnerAssignment.Update();
            ClientService.DoJavascript("bindingDataTableOwnerAssignment();");

            //SmartPagingOwnerAssignment.RepeaterControl = rptTableOwnerAssignment;
            //SmartPagingOwnerAssignment.UpdatePanelControl = udpTableOwnerAssignment;
            //SmartPagingOwnerAssignment.DataPageSize = 10;
            //SmartPagingOwnerAssignment.DataSource = dsEquipment.master_equipment_owner_assignment;
            //SmartPagingOwnerAssignment.DataBind();
        }
        
        //protected void rptTableOwnerAssignment_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    DropDownList ddlOwnerAssignmentBox_OwnerType = e.Item.FindControl("ddlOwnerAssignmentBox_OwnerType") as DropDownList;
        //    HiddenField hddOwnerAssignmentBox_OwnerType = e.Item.FindControl("hddOwnerAssignmentBox_OwnerType") as HiddenField;
        //    HiddenField hddOwnerAssignmentBox_OwnerCode = e.Item.FindControl("hddOwnerAssignmentBox_OwnerCode") as HiddenField;
            
        //    HiddenField hdfSLAGroup = e.Item.FindControl("hdfSLAGroup") as HiddenField;
        //    HiddenField hdfTicketType = e.Item.FindControl("hdfTicketType") as HiddenField;

        //    AutoCompleteControl Complete_OwnerAssignmentBox_CustomerSelect = e.Item.FindControl("Complete_OwnerAssignmentBox_CustomerSelect") as AutoCompleteControl;
            
        //    #region Set AutoComplete
        //    if (hddOwnerAssignmentBox_OwnerType.Value == "01")//Customer
        //    {
        //        Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
        //            dtCustomer,
        //            "CustomerCode",
        //            "CustomerName",
        //            true
        //        );
        //    }
        //    else if (hddOwnerAssignmentBox_OwnerType.Value == "02")//Vendor
        //    {
        //        Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
        //            dtVendor,
        //            "VendorCode",
        //            "VendorName",
        //            true
        //        );
        //    }
        //    else if (hddOwnerAssignmentBox_OwnerType.Value == "03")//Employee
        //    {
        //        Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
        //            dtEmployee,
        //            "EmployeeCode",
        //            "FullName",
        //            true
        //        );
        //    }
        //    else if (hddOwnerAssignmentBox_OwnerType.Value == "ST")
        //    {
        //        Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
        //            dtSoldToParty,
        //            "CustomerCode",
        //            "CustomerName",
        //            true
        //        );
        //    }
        //    else if (hddOwnerAssignmentBox_OwnerType.Value == "SH")
        //    {
        //        Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
        //            dtShipToParty,
        //            "CustomerCode",
        //            "CustomerName",
        //            true
        //        );
        //    }
        //    else if (hddOwnerAssignmentBox_OwnerType.Value == "BP")
        //    {
        //        Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
        //            dtBillToParty,
        //            "CustomerCode",
        //            "CustomerName",
        //            true
        //        );
        //    }
        //    #endregion


        //    Complete_OwnerAssignmentBox_CustomerSelect.SetValue = hddOwnerAssignmentBox_OwnerCode.Value;

        //    ddlOwnerAssignmentBox_OwnerType.SelectedValue = hddOwnerAssignmentBox_OwnerType.Value;
        //    if (FilterOwner)
        //    {
        //        ddlOwnerAssignmentBox_OwnerType.Items.FindByValue("02").Enabled = false;
        //        ddlOwnerAssignmentBox_OwnerType.Items.FindByValue("03").Enabled = false;
        //        ddlOwnerAssignmentBox_OwnerType.Items.FindByValue("ST").Enabled = false;
        //        ddlOwnerAssignmentBox_OwnerType.Items.FindByValue("SH").Enabled = false;
        //        ddlOwnerAssignmentBox_OwnerType.Items.FindByValue("BP").Enabled = false;
        //    }

        //    DropDownList ddlSLAGroup = e.Item.FindControl("ddlSLAGroup") as DropDownList;
        //    ddlSLAGroup.DataSource = dtSLAGroup;
        //    ddlSLAGroup.DataBind();
        //    ddlSLAGroup.Items.Insert(0, new ListItem("", ""));
        //    ddlSLAGroup.SelectedValue = hdfSLAGroup.Value;

        //    //DropDownList ddlTicketType = e.Item.FindControl("ddlTicketType") as DropDownList;
        //    //ddlTicketType.DataSource = dtTicketType;
        //    //ddlTicketType.DataBind();
        //    //ddlTicketType.Items.Insert(0, new ListItem("", ""));
        //    //ddlTicketType.SelectedValue = hdfTicketType.Value;

        //}

        #endregion

        #region Manage Rows Table Owner Assignment
        protected void btnAddRowTableOwnerAssignment_Click(object sender, EventArgs e)
        {
            try
            {
                txtOwnerAssignmentBox_ItemNo.Text = "#####";
                ddlOwnerAssignmentBox_OwnerType.SelectedIndex = 0;

                Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
                   new DataTable(),
                   "Key",
                   "Value",
                   true
               );

                txtOwnerAssignmentBox_BeginDate.Text = Validation.getCurrentServerDate();
                txtOwnerAssignmentBox_EndDate.Text = "31/12/9999";
                ddlSLAGroup.SelectedIndex = 0;
                chkOwnerAssignmentBox_ActiveStatus.Checked = true;
                udpDataOwnerAssign.Update();

                ClientService.DoJavascript("$('#modal-owner-assignment').modal('show');");

                //AddRowTableOwnerAssignment();
                //SmartPagingOwnerAssignment.GoToLastPage();
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
        protected void btnRemoveRowTableOwnerAssignment_Click(object sender, EventArgs e)
        {
            try
            {
                string ItemNo = hddOwnerAssign_LineNumber.Value;
                foreach (DataRow dr in dsEquipment.master_equipment_owner_assignment.Select("LineNumber = '" + ItemNo + "'"))
                {
                    dr.Delete();
                }

                bindDataTableOwnerAssignment();
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

        protected void btnEditRowTableOwnerAssignment_Click(object sender, EventArgs e)
        {
            try
            {
                string ItemNo = hddOwnerAssign_LineNumber.Value;
                string OwnerType = "";
                string OwnerCode = "";
                string BeginDate = "";
                string EndDate = "";
                string SLAGroup = "";
                string ActiveStatus = "";

                foreach (DataRow dr in dsEquipment.master_equipment_owner_assignment.Select("LineNumber = '" + ItemNo + "'"))
                {
                    OwnerType = (dr["OwnerType"] as string);
                    OwnerCode = (dr["OwnerCode"] as string);
                    BeginDate = (dr["BeginDate"] as string);
                    EndDate = (dr["EndDate"] as string);
                    SLAGroup = (dr["SLAGroupCode"] as string);
                    ActiveStatus = (dr["ActiveStatus"] as string);
                }
                
                //Button btn = (sender as Button);
                //RepeaterItem rptItem = btn.Parent.Parent.Parent as RepeaterItem;

                //Label lblOwnerAssignmentBox_ItemNo = rptItem.FindControl("lblOwnerAssignmentBox_ItemNo") as Label;
                //HiddenField hddOwnerAssignmentBox_OwnerType = rptItem.FindControl("hddOwnerAssignmentBox_OwnerType") as HiddenField;
                //HiddenField hddOwnerAssignmentBox_OwnerCode = rptItem.FindControl("hddOwnerAssignmentBox_OwnerCode") as HiddenField;
                //HiddenField hddOwnerAssignmentBox_BeginDate = rptItem.FindControl("hddOwnerAssignmentBox_BeginDate") as HiddenField;
                //HiddenField hddOwnerAssignmentBox_EndDate = rptItem.FindControl("hddOwnerAssignmentBox_EndDate") as HiddenField;
                //HiddenField hddOwnerAssignmentBox_SLAGroup = rptItem.FindControl("hddOwnerAssignmentBox_SLAGroup") as HiddenField;
                //HiddenField hddOwnerAssignmentBox_ActiveStatus = rptItem.FindControl("hddOwnerAssignmentBox_ActiveStatus") as HiddenField;

                txtOwnerAssignmentBox_ItemNo.Text = ItemNo;
                ddlOwnerAssignmentBox_OwnerType.SelectedValue = OwnerType;

                #region Set AutoComplete
                if (ddlOwnerAssignmentBox_OwnerType.SelectedValue == "01")
                {
                    Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
                       dtCustomerKeyValue.toDataTable(),
                       "Key",
                       "Value",
                       true
                   );
                }
                else if (ddlOwnerAssignmentBox_OwnerType.SelectedValue == "02")
                {
                    Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
                        dtVendorKeyValue.toDataTable(),
                        "Key",
                        "Value",
                        true
                    );
                }
                else if (ddlOwnerAssignmentBox_OwnerType.SelectedValue == "03")//Employee
                {
                    Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
                        dtEmployeeKeyValue.toDataTable(),
                        "Key",
                        "Value",
                        true
                    );
                }
                else if (ddlOwnerAssignmentBox_OwnerType.SelectedValue == "ST")
                {
                    Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
                        dtSoldToPartyKeyValue.toDataTable(),
                        "Key",
                        "Value",
                        true
                    );
                }
                else if (ddlOwnerAssignmentBox_OwnerType.SelectedValue == "SH")
                {
                    Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
                        dtShipToPartyKeyValue.toDataTable(),
                        "Key",
                        "Value",
                        true
                    );
                }
                else if (ddlOwnerAssignmentBox_OwnerType.SelectedValue == "BP")
                {
                    Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
                        dtBillToPartyKeyValue.toDataTable(),
                        "Key",
                        "Value",
                        true
                    );
                }
                else
                {
                    Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
                       new DataTable(),
                       "Key",
                       "Value",
                       true
                   );
                }
                #endregion

                Complete_OwnerAssignmentBox_CustomerSelect.SetValue = OwnerCode;
                txtOwnerAssignmentBox_BeginDate.Text = Validation.Convert2DateDisplay(BeginDate);
                txtOwnerAssignmentBox_EndDate.Text = Validation.Convert2DateDisplay(EndDate);
                ddlSLAGroup.SelectedValue = SLAGroup;
                chkOwnerAssignmentBox_ActiveStatus.Checked = ActiveStatus.ToLower() == "true";
                udpDataOwnerAssign.Update();

                ClientService.DoJavascript("$('#modal-owner-assignment').modal('show');");
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

        protected void btnSaveOwnerAssignmentModal_Click(object sender, EventArgs e)
        {
            try
            {
                string curDate = Validation.getCurrentServerStringDateTime();
                if (txtOwnerAssignmentBox_ItemNo.Text == "#####" || txtOwnerAssignmentBox_ItemNo.Text == "")
                {

                    DataRow dr = dsEquipment.master_equipment_owner_assignment.NewRow();

                    int GenItemNo = 0;
                    if (dsEquipment.master_equipment_owner_assignment.Rows.Count > 0)
                    {
                        //GenItemNo = Convert.ToInt32(
                        //    dsEquipment.master_equipment_owner_assignment.Select("LineNumber = max(LineNumber)")[0]["LineNumber"]
                        //) + 1;

                        //GenItemNo = dsEquipment.master_equipment_owner_assignment.Max(m => Convert.ToInt32(m.LineNumber)) + 1;

                        DataTable dtCopy = dsEquipment.master_equipment_owner_assignment.Copy();
                        dtCopy.AcceptChanges();
                        GenItemNo = Convert.ToInt32(
                            dtCopy.Select("LineNumber = max(LineNumber)")[0]["LineNumber"]
                        ) + 1;
                    }

                    dr["SID"] = SID;
                    dr["CompanyCode"] = CompanyCode;
                    dr["LineNumber"] = GenItemNo.ToString().PadLeft(5, '0');
                    dr["ActiveStatus"] = chkOwnerAssignmentBox_ActiveStatus.Checked ? "True" : "False";
                    dr["BeginDate"] = Validation.Convert2DateDB(txtOwnerAssignmentBox_BeginDate.Text);
                    dr["EndDate"] = Validation.Convert2DateDB(txtOwnerAssignmentBox_EndDate.Text);
                    dr["OwnerCode"] = Complete_OwnerAssignmentBox_CustomerSelect.SelectValue;
                    dr["OwnerDesc"] = Complete_OwnerAssignmentBox_CustomerSelect.SelectText;
                    dr["OwnerType"] = ddlOwnerAssignmentBox_OwnerType.SelectedValue;
                    dr["CREATED_ON"] = curDate;
                    dr["CREATED_BY"] = EmployeeCode;
                    dr["SLAGroupCode"] = ddlSLAGroup.SelectedValue;
                    if (!string.IsNullOrEmpty(EquipmentCode))
                    {
                        dr["EquipmentCode"] = EquipmentCode;
                    }

                    dsEquipment.master_equipment_owner_assignment.Rows.Add(dr);
                    if (dsEquipment.master_equipment_owner_assignment.Columns.Contains("_PagingService_RowNum"))
                    {
                        dr["_PagingService_RowNum"] = GenItemNo + 1;
                    }
                }
                else
                {
                    foreach (DataRow dr in dsEquipment.master_equipment_owner_assignment.Select("LineNumber = '" + txtOwnerAssignmentBox_ItemNo.Text + "'"))
                    {
                        dr["ActiveStatus"] = chkOwnerAssignmentBox_ActiveStatus.Checked ? "True" : "False";
                        dr["BeginDate"] = Validation.Convert2DateDB(txtOwnerAssignmentBox_BeginDate.Text);
                        dr["EndDate"] = Validation.Convert2DateDB(txtOwnerAssignmentBox_EndDate.Text);
                        dr["OwnerCode"] = Complete_OwnerAssignmentBox_CustomerSelect.SelectValue;
                        dr["OwnerDesc"] = Complete_OwnerAssignmentBox_CustomerSelect.SelectText;
                        dr["OwnerType"] = ddlOwnerAssignmentBox_OwnerType.SelectedValue;
                        dr["UPDATED_ON"] = curDate;
                        dr["UPDATED_BY"] = EmployeeCode;
                        dr["SLAGroupCode"] = ddlSLAGroup.SelectedValue;
                    }
                }
                ClientService.DoJavascript("$('#modal-owner-assignment').modal('hide');");
                bindDataTableOwnerAssignment();
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

        private void AddRowTableOwnerAssignment()
        {
            PrepareDataTableOwnerAssignment();
            DataRow dr = dsEquipment.master_equipment_owner_assignment.NewRow();

            int GenItemNo = 0;
            if (dsEquipment.master_equipment_owner_assignment.Rows.Count > 0)
            {
                //GenItemNo = Convert.ToInt32(
                //    dsEquipment.master_equipment_owner_assignment.Select("LineNumber = max(LineNumber)")[0]["LineNumber"]
                //) + 1;
                GenItemNo = dsEquipment.master_equipment_owner_assignment.Max(m => Convert.ToInt32(m.LineNumber)) + 1;
            }

            dr["SID"] = SID;
            dr["CompanyCode"] = CompanyCode;
            dr["LineNumber"] = GenItemNo.ToString().PadLeft(5, '0');
            dr["BeginDate"] = Validation.Convert2DateDB(Validation.getCurrentServerDate());

            if (!string.IsNullOrEmpty(EquipmentCode))
            {
                dr["EquipmentCode"] = EquipmentCode;
            }

            dsEquipment.master_equipment_owner_assignment.Rows.Add(dr);
            if (dsEquipment.master_equipment_owner_assignment.Columns.Contains("_PagingService_RowNum"))
            {
                dr["_PagingService_RowNum"] = GenItemNo + 1;
            }
            bindDataTableOwnerAssignment();
        }

        #endregion

        #endregion

        #region Table Serial detail
        #region bind Table Serial detail
        private void bindDataTableSerialDetail()
        {
            rptTableSerialData.DataSource = dsEquipment.master_equipment_serial_detail;
            rptTableSerialData.DataBind();
            udpTableSerialData.Update();
        }
        protected void rptTableSerialData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hddTableSerialData_MaterialCode = e.Item.FindControl("hddTableSerialData_MaterialCode") as HiddenField;
            HiddenField hddTableSerialData_SerialNo = e.Item.FindControl("hddTableSerialData_SerialNo") as HiddenField;
            DropDownList ddlTableSerialData_SerialNo = e.Item.FindControl("ddlTableSerialData_SerialNo") as DropDownList;

            AutoCompleteControl Complete_TableSerialData_MaterialCode = e.Item.FindControl("Complete_TableSerialData_MaterialCode") as AutoCompleteControl;
            Complete_TableSerialData_MaterialCode.initialDataAutoComplete(dtMaterial, "ItmNumber", "ItmDescription", true);
            Complete_TableSerialData_MaterialCode.SetValue = hddTableSerialData_MaterialCode.Value;
            Complete_TableSerialData_MaterialCode.TODO_FunctionJS = "$('#control-" + Complete_TableSerialData_MaterialCode.ClientID + "').closest('td').find('.btnRebindSerialNo').click()";

            bindDDlSerialNO(hddTableSerialData_MaterialCode.Value, ddlTableSerialData_SerialNo);
            try
            {
                ddlTableSerialData_SerialNo.SelectedValue = hddTableSerialData_SerialNo.Value;
            }
            catch (Exception) { }
        }
        #endregion

        #region Manage Rows Table Serial detail
        protected void btnAddRowTableSerialData_Click(object sender, EventArgs e)
        {
            try
            {
                AddRowTableSerialDetail();
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
        protected void btnRemoveRowTableSerialData_Click(object sender, EventArgs e)
        {
            try
            {
                PrepareDataTableSerialDetail();

                string ItemNo = (sender as Button).CommandArgument;
                foreach (DataRow dr in dsEquipment.master_equipment_serial_detail.Select("LineNumber = '" + ItemNo + "'"))
                {
                    dr.Delete();
                }

                bindDataTableSerialDetail();
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
        private void AddRowTableSerialDetail()
        {
            PrepareDataTableSerialDetail();
            DataRow dr = dsEquipment.master_equipment_serial_detail.NewRow();

            int GenItemNo = 0;
            if (dsEquipment.master_equipment_serial_detail.Rows.Count > 0)
            {
                GenItemNo = Convert.ToInt32(
                    dsEquipment.master_equipment_serial_detail.Select("LineNumber = max(LineNumber)")[0]["LineNumber"]
                ) + 1;
            }

            dr["SID"] = SID;
            dr["CompanyCode"] = CompanyCode;
            dr["LineNumber"] = GenItemNo.ToString().PadLeft(3, '0');
            dr["EffectiveFrom"] = Validation.Convert2DateDB(Validation.getCurrentServerDate());

            if (!string.IsNullOrEmpty(EquipmentCode))
            {
                dr["EquipmentCode"] = EquipmentCode;
            }

            dsEquipment.master_equipment_serial_detail.Rows.Add(dr);

            bindDataTableSerialDetail();
        }
        #endregion

        #region DDL In Table
        private DataTable _dtMaterial;
        private DataTable dtMaterial
        {
            get
            {
                if (_dtMaterial == null)
                {
                    _dtMaterial = materialService.getInSatnce().getMaterialGeneral_V2(
                        SID,
                        CompanyCode,
                        "", "", "", "True", ""
                    );
                }
                return _dtMaterial;
            }
        }

        private void bindDDlSerialNO(string _matcode, DropDownList ddlTableSerialData_SerialNo)
        {
            string where = @"#where A.SID = '" + SID + @"'
                             and A.Materialcode = '" + _matcode + @"' 
                             and B.SerialNo <> '' 
                             and B.SerialNo not in 
                               (
                                  select SerialNo from master_equipment_serial_detail 
                                  where sid = '" + SID + @"' 
                                  and MaterialCode = '" + _matcode + @"'
                               )";

            DataTable dt = ICMUtilHelper.GetSearchData(
                SID,
                "GETSERIALSETTLEJOINITEM",
                where
            );

            ddlTableSerialData_SerialNo.DataValueField = "SerialNo";
            ddlTableSerialData_SerialNo.DataTextField = "SerialNo";

            ddlTableSerialData_SerialNo.DataSource = dt;
            ddlTableSerialData_SerialNo.DataBind();

            ddlTableSerialData_SerialNo.Items.Insert(0, new ListItem("เลือก", ""));
            (ddlTableSerialData_SerialNo.Parent.Parent as UpdatePanel).Update();
        }
        #endregion

        #endregion


        #region On DDL Changed
        protected void ddlSalesAndDistribution_BillingDocTypeCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLBillingDocNumber();
            udpPanelSale.Update();
        }

        protected void ddlSalesAndDistribution_BillingDocNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = ServiceEquipment.getBillingDocnumberByDoctype(
                SID,
                ddlSalesAndDistribution_BillingDocTypeCode.SelectedValue
            );

            foreach (DataRow dr in dt.Select("SaleDocument = '" + ddlSalesAndDistribution_BillingDocNumber.SelectedValue + "'"))
            {
                txtSalesAndDistribution_BillingDocYear.Text = dr["FiscalYear"].ToString();
            }

            udpPanelSale.Update();
        }

        //Edit 01/11/2561
        //protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DataTable dt = ServiceEquipment.getLocationMasterddl(
        //        SID,
        //        DropDownList1.SelectedValue
        //    );
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        txtPlantID.Text = dr["Plant"].ToString();
        //        txtLocationID.Text = dr["Location"].ToString();
        //        txtRoomID.Text = dr["Room"].ToString();
        //        txtShelfID.Text = dr["Shelf"].ToString();
        //        txtWorkCenterID.Text = dr["WorkCenter"].ToString();
        //        txtSlotID.Text = dr["Slot"].ToString();
        //        txtAddressZip.Text = dr["AddressZipCode"].ToString();
        //        txtAddressCity.Text = dr["AddressCity"].ToString();
        //        txtAddressID1.Text = dr["AddressName1"].ToString();
        //        txtAddressID2.Text = dr["AddressName2"].ToString();
        //        txtStreetID.Text = dr["AddressStreet"].ToString();
        //        txtTelephoneID.Text = dr["AddressTelephone"].ToString();
        //        txtFaxID.Text = dr["AddressFax"].ToString();
        //    }
        //    UpdatePanel1.Update();
        //}

        protected void ddlLocationDataBox_Plant_Code_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindDDLStorageLocation();
                udpLocationDataBox_Location.Update();
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

        // 22/01/2562 add func selection chage for set CI and CI family (by born kk)
        protected void SelectionCIFamily_Change(object sender, EventArgs e)
        {
            string eqGroup = ddlEquipmentType.SelectedValue;
            DataTable dt = ServiceEquipment.getEMClass(SID);
            DataTable dtSelect = new DataTable();
            if (dt.Select("EquipmentGroup = '" + eqGroup + "'").Count() > 0)
            {

                dtSelect = dt.Select("EquipmentGroup = '" + eqGroup + "'").CopyToDataTable<DataRow>();
                ddlEMClass.DataSource = dtSelect;
                ddlEMClass.DataValueField = "ClassCode";
                ddlEMClass.DataTextField = "ClassName";
                ddlEMClass.DataBind();
                
            }
            else
            {
                ddlEMClass.DataSource = dtSelect;
                ddlEMClass.DataBind();
               

            }
        }
        protected void SelectionCI_Change(object sender, EventArgs e)
        {
            string classCode = ddlEMClass.SelectedValue;
            DataTable dt = ServiceEquipment.getEMClass(SID);
            DataRow[] dtr = dt.Select("ClassCode = '" + classCode + "'");
            if (dtr.Count() > 0)
            {
                ddlEquipmentType.SelectedValue = dtr[0]["EquipmentGroup"].ToString();
            }
        }

        #endregion

        protected void btnRebindSerialNo_Click(object sender, EventArgs e)
        {
            try
            {
                AutoCompleteControl Complete_TableSerialData_MaterialCode = (sender as Button).Parent.Parent.FindControl("Complete_TableSerialData_MaterialCode") as AutoCompleteControl;
                DropDownList ddlTableSerialData_SerialNo = (sender as Button).Parent.Parent.FindControl("ddlTableSerialData_SerialNo") as DropDownList;
                bindDDlSerialNO(Complete_TableSerialData_MaterialCode.SelectValue, ddlTableSerialData_SerialNo);
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

        #region
        #endregion

        private void bindRelationship()
        {
            LinkFlowChartService.DiagramRelation dataEn = LinkFlowChartService.getDiagramRelation(
                 SID,
                 CompanyCode,
                 EquipmentCode,
                 LinkFlowChartService.ItemGroup_EQUIPMENT
             );

            if (dataEn.parentNode.Count + dataEn.chindNode.Count > 0)
            {
                dataEn.parentNode = dataEn.parentNode.Where(w => w.Level <= 1).ToList();
                dataEn.chindNode = dataEn.chindNode.Where(w => w.Level <= 1).ToList();

                FlowChartDiagramRelationControl.URLNodeRedirect = "/crm/Master/Equipment/EquipmentDetail.aspx?code={#ID}&mode=Edit";
                FlowChartDiagramRelationControl.nodeActive = EquipmentCode;
                FlowChartDiagramRelationControl.listParentDiagram = dataEn.parentNode;
                FlowChartDiagramRelationControl.listChildDiagram = dataEn.chindNode;
                FlowChartDiagramRelationControl.initFlowChartDiagram();
            }
        }

        #region Panel History
        private void LoadListTicket()
        {
            dtTempAttachfile = AfterSaleService.getInstance().getCountAttachFile(
                SID,
                CompanyCode,
                ""
            );

            DataTable _dtDataSearch = AfterSaleService.getInstance().getServiceCall(
                SID, CompanyCode,
                "", "", "", "", "", "", "", "", "", "",
                " and B.EquipmentNo = '" + EquipmentCode + "'"
            );

            if (!_dtDataSearch.Columns.Contains("total_attachfile"))
                _dtDataSearch.Columns.Add("total_attachfile");
            if (!_dtDataSearch.Columns.Contains("total_messagechat"))
                _dtDataSearch.Columns.Add("total_messagechat");

            dtDataSearch = _dtDataSearch.Clone();

            foreach (DataRow dr in _dtDataSearch.Rows)
            {
                DataRow[] drr = dtDataSearch.Select("CallerID='" + dr["CallerID"].ToString() + "'");
                if (drr.Length <= 0)
                {
                    dr["total_attachfile"] = getCountAttachFile(dr["CallerID"].ToString(), dr["Fiscalyear"].ToString());
                    dr["total_messagechat"] = "0";
                    dtDataSearch.ImportRow(dr);
                }
            }

            foreach (DataRow dr in dtDataSearch.Rows)
            {
                dr["DOCDATE"] = Validation.Convert2RadDateDisplay(dr["DOCDATE"].ToString()).ToString("dd/MM/yyyy");
            }

            rptSearchSale.DataSource = dtDataSearch;
            rptSearchSale.DataBind();

            udpnItems.Update();

            ClientService.DoJavascript("afterSearch();");
        }

        private string getCountAttachFile(string callid, string fiscalyear)
        {
            string _total = "";
            if (callid != null && callid.ToString() != "")
            {
                DataRow[] drr = dtTempAttachfile.Select("CallerID='" + callid + "' and Fiscalyear='" + fiscalyear + "'");
                if (drr.Length > 0)
                {
                    _total = drr[0]["totalfile"].ToString();
                }
            }
            return _total;
        }

        protected void btnOwnerAssignmentBox_LoadOwner_Click(object sender, EventArgs e)
        {
            try
            {
                //DropDownList ddlOwnerType = (sender as Button).Parent.FindControl("ddlOwnerAssignmentBox_OwnerType") as DropDownList;

                //UpdatePanel udpBox_CustomerSelect = (sender as Button).Parent.FindControl("udpBox_CustomerSelect") as UpdatePanel;
                //AutoCompleteControl Complete_OwnerAssignmentBox_CustomerSelect = (sender as Button).Parent.FindControl("Complete_OwnerAssignmentBox_CustomerSelect") as AutoCompleteControl;

                #region Set AutoComplete
                if (ddlOwnerAssignmentBox_OwnerType.SelectedValue == "01")
                {
                    Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
                       dtCustomerKeyValue.toDataTable(),
                       "Key",
                       "Value",
                       true
                   );
                }
                else if (ddlOwnerAssignmentBox_OwnerType.SelectedValue == "02")
                {
                    Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
                        dtVendorKeyValue.toDataTable(),
                        "Key",
                        "Value",
                        true
                    );
                }
                else if (ddlOwnerAssignmentBox_OwnerType.SelectedValue == "03")//Employee
                {
                    Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
                        dtEmployeeKeyValue.toDataTable(),
                        "Key",
                        "Value",
                        true
                    );
                }
                else if (ddlOwnerAssignmentBox_OwnerType.SelectedValue == "ST")
                {
                    Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
                        dtSoldToPartyKeyValue.toDataTable(),
                        "Key",
                        "Value",
                        true
                    );
                }
                else if (ddlOwnerAssignmentBox_OwnerType.SelectedValue == "SH")
                {
                    Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
                        dtShipToPartyKeyValue.toDataTable(),
                        "Key",
                        "Value",
                        true
                    );
                }
                else if (ddlOwnerAssignmentBox_OwnerType.SelectedValue == "BP")
                {
                    Complete_OwnerAssignmentBox_CustomerSelect.initialDataAutoComplete(
                        dtBillToPartyKeyValue.toDataTable(),
                        "Key",
                        "Value",
                        true
                    );
                }
                #endregion

                Complete_OwnerAssignmentBox_CustomerSelect.SetValue = "";
                udpBox_CustomerSelect.Update();
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

        protected void getdataToedit(string doctype, string docnumber, string fiscalyear,string customer)
        {
            ServiceCallFastEntryCriteria link = new ServiceCallFastEntryCriteria();
            string idGen = link.redirectViewToTicketDetail(customer, doctype, docnumber, fiscalyear);
            if (!String.IsNullOrEmpty(idGen))
            {
                ServiceTicketLibrary lib_TK = new ServiceTicketLibrary();
                string PageRedirect = lib_TK.getPageTicketRedirect(
                    SID,
                    (Session["ServicecallEntity" + idGen] as tmpServiceCallDataSet).cs_servicecall_header.Rows[0]["Doctype"].ToString()
                );
                Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen));
                //Response.-Redirect("/crm/AfterSale/ServiceCallTransaction.aspx?id=" + idGen, false);
            }

            //Object[] objParam = new Object[] { "1500117",
            //        (string)Session[ApplicationSession.USER_SESSION_ID],
            //        CompanyCode,doctype,docnumber,fiscalyear};

            //DataSet[] objDataSet = new DataSet[] { serviceCallEntity };
            //DataSet objReturn = icmUtil.ICMDataSetInvoke(objParam, objDataSet);
            //if (objReturn != null)
            //{
            //    serviceCallEntity = new tmpServiceCallDataSet();
            //    serviceCallEntity.Merge(objReturn.Copy());
            //    mode_stage = ApplicationSession.CHANGE_MODE_STRING;
            //    Response-.Redirect("~/crm/AfterSale/ServiceCallTransaction.aspx", false);
            //}
        }

        protected void btnLinkTransactionSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string CallerID = hddCallerID_Criteria.Value;
                DataRow[] drr = dtDataSearch.Select("CallerID='" + CallerID + "'");
                dtTempDoc.Clear();
                int i = 1;
                foreach (DataRow dr in dtDataSearch.Rows)
                {
                    DataRow drt = dtTempDoc.NewRow();
                    drt["doctype"] = dr["Doctype"].ToString();
                    drt["docnumber"] = dr["CallerID"].ToString();
                    drt["docfiscalyear"] = dr["Fiscalyear"].ToString();
                    drt["indexnumber"] = i++;
                    dtTempDoc.Rows.Add(drt);
                }

                if (drr.Length > 0)
                {
                    getdataToedit(drr[0]["Doctype"].ToString(), drr[0]["CallerID"].ToString(), drr[0]["Fiscalyear"].ToString(), drr[0]["CustomerCode"].ToString());
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

        protected void btnLinkAttachSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string[] command = (sender as Button).CommandArgument.ToString().Split(',');

                lbHeadAttach.Text = command[1] + " : " + command[2];

                DataRow[] drr = dtDataSearch.Select("CallerID='" + command[0] + "'");
                if (drr.Length > 0)
                {
                    setAttachMent("", false
                  , drr[0]["Doctype"].ToString(), true
                  , drr[0]["CallerID"].ToString(), true
                  , drr[0]["Fiscalyear"].ToString(), true);
                }

                ClientService.DoJavascript("$('#sale-after-attachfile').modal('show');");

                updAttachFile.Update();
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

        private void setAttachMent(
          String employeeCode, bool req_emp,
          String doc_type, bool req_doctype,
          String doc_number, bool req_doc_number,
          String doc_year, bool req_doc_year)
        {
            AttachFileUserControl.init(
                SID, true
                , employeeCode, req_emp
                , doc_type, req_doctype
                , doc_type, req_doctype
                , doc_number, req_doc_number
                , doc_year, req_doc_year
                , null, false);

            AttachFileUserControl.rebind();
        }
        
        #endregion

        #region Panel file Attachment
        private void loadTimeLineFileAttachment()
        {
            TimeLineControl.ProgramID = LogServiceLibrary.PROGRAM_ID_EQUIPMENT;
            TimeLineControl.KeyAobjectlink = EquipmentCode;
            TimeLineControl.bindDataTimeLineHasFile();
        }
        #endregion

        #region Equipment Log
        private void bindLogEquipment(string EquipmentCode) 
        {
            DataTable dtLog = ServiceLog.GetEquipmentLog(
                SID,
                CompanyCode,
                EquipmentCode
            );
            EquipmentChangeLog.BindingLog(dtLog);
        }

        protected void btnReloadLog_Click(object sender, EventArgs e)
        {
            try
            {
                bindLogEquipment(EquipmentCode);
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
        #endregion

        #region Panel Warranty

        private void bindWarrantyCriteria()
        {
            ddlMaintenanceType.DataValueField = "Code";
            ddlMaintenanceType.DataTextField = "Description";
            ddlMaintenanceType.DataSource = erpwMaster.GetMasterConfigWarranty(SID);
            ddlMaintenanceType.DataBind();
            ddlMaintenanceType.Items.Insert(0, new ListItem("เลือก", ""));
        }

        protected void btnWarrantyNextMaintenanceRefStartDate_Click(object sender, EventArgs e)
        {
            try
            {
                string StartGuaranty = string.IsNullOrEmpty(txtMaintenanceStartDate.Text) ? "0" : Validation.Convert2DateDB(txtMaintenanceStartDate.Text);
                string EndGuaranty = string.IsNullOrEmpty(txtMaintenanceEndDate.Text) ? "0" : Validation.Convert2DateDB(txtMaintenanceEndDate.Text);

                if (string.IsNullOrEmpty(txtMaintenanceStartDate.Text) && string.IsNullOrEmpty(txtMaintenanceEndDate.Text))
                {
                    txtMaintenanceEndDate.Text = "";
                    txtPeriod.Text = "";
                    txtNextMaintenanceDate.Text = "";
                    txtNextMaintenanceTime.Text = ""; 
                    return;
                }

                if (Convert.ToInt32(StartGuaranty) > Convert.ToInt32(EndGuaranty) && EndGuaranty != "0")
                {
                    txtMaintenanceEndDate.Text = "";
                    throw new Exception("Maintenance Start over Maintenance End ");
                }
                string NextMaintenance = CallWarrantyNextMaintenance();
                if (!string.IsNullOrEmpty(NextMaintenance))
                {
                    if (Convert.ToInt32(NextMaintenance.Substring(0, 8)) > Convert.ToInt32(EndGuaranty) && EndGuaranty != "0")
                    {
                        txtMaintenanceEndDate.Text = "";
                        throw new Exception("Next Maintenance over Maintenance End ");
                    }
                    txtNextMaintenanceDate.Text = Validation.Convert2DateDisplay(NextMaintenance);
                    txtNextMaintenanceTime.Text = !string.IsNullOrEmpty(txtNextMaintenanceTime.Text) ? txtNextMaintenanceTime.Text : "00:00";
                }
            }
            catch (Exception ex)
            {
                
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                udpTableWarranty.Update();
                ClientService.AGLoading(false);
            }
        }

        protected void btnWarrantyNextMaintenanceRefEndDate_Click(object sender, EventArgs e)
        {
            try
            {
                string StartGuaranty = string.IsNullOrEmpty(txtMaintenanceStartDate.Text) ? "0" : Validation.Convert2DateDB(txtMaintenanceStartDate.Text);
                string EndGuaranty = string.IsNullOrEmpty(txtMaintenanceEndDate.Text) ? "0" : Validation.Convert2DateDB(txtMaintenanceEndDate.Text);

                if (string.IsNullOrEmpty(txtMaintenanceStartDate.Text) && string.IsNullOrEmpty(txtMaintenanceEndDate.Text))
                {
                    txtMaintenanceEndDate.Text = "";
                    txtPeriod.Text = "";
                    txtNextMaintenanceDate.Text = "";
                    txtNextMaintenanceTime.Text = "";
                    return;
                }

                if (Convert.ToInt32(StartGuaranty) > Convert.ToInt32(EndGuaranty))
                {
                    txtMaintenanceEndDate.Text = "";
                    throw new Exception("Maintenance Start over Maintenance End ");
                }


                string NextMaintenance = CallWarrantyNextMaintenance();
                if (!string.IsNullOrEmpty(NextMaintenance))
                {
                    if (Convert.ToInt32(NextMaintenance.Substring(0, 8)) > Convert.ToInt32(EndGuaranty))
                    {
                        txtMaintenanceEndDate.Text = "";
                        udpTableWarranty.Update();
                        throw new Exception("Next Maintenance over Maintenance End ");
                    }
                    txtNextMaintenanceDate.Text = Validation.Convert2DateDisplay(NextMaintenance);
                    txtNextMaintenanceTime.Text = !string.IsNullOrEmpty(txtNextMaintenanceTime.Text) ? txtNextMaintenanceTime.Text : "00:00";
                }
                
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                udpTableWarranty.Update();
                ClientService.AGLoading(false);
            }
        }

        protected void btnWarrantyNextMaintenanceRefPeriod_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(txtMaintenanceStartDate.Text) && string.IsNullOrEmpty(txtMaintenanceEndDate.Text))
                {
                    txtMaintenanceEndDate.Text = "";
                    txtPeriod.Text = "";
                    txtNextMaintenanceDate.Text = "";
                    txtNextMaintenanceTime.Text = "";
                    throw new Exception("Place input Maintenance strat and Maintenance end !");
                }

                string NextMaintenance = CallWarrantyNextMaintenance();
                if (!string.IsNullOrEmpty(NextMaintenance))
                {
                    string EndGuaranty = string.IsNullOrEmpty(txtMaintenanceEndDate.Text) ? "0" : Validation.Convert2DateDB(txtMaintenanceEndDate.Text);
                    if (Convert.ToInt32(NextMaintenance.Substring(0, 8)) > Convert.ToInt32(EndGuaranty))
                    {
                        txtPeriod.Text = "";
                        txtNextMaintenanceDate.Text = "";
                        txtNextMaintenanceTime.Text = "";
                        throw new Exception("Next Maintenance over Maintenance End ");
                    }
                    txtNextMaintenanceDate.Text = Validation.Convert2DateDisplay(NextMaintenance);
                    txtNextMaintenanceTime.Text = !string.IsNullOrEmpty(txtNextMaintenanceTime.Text) ? txtNextMaintenanceTime.Text : "00:00";
                        
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                udpTableWarranty.Update();
                ClientService.AGLoading(false);
            }
        }

        private string CallWarrantyNextMaintenance()
        {
            txtNextMaintenanceDate.Text = "";
            txtNextMaintenanceTime.Text = "";
            string BeginGuarantee = string.IsNullOrEmpty(txtMaintenanceStartDate.Text) ? "" : Validation.Convert2DateDB(txtMaintenanceStartDate.Text);
            string EndGuaranty = string.IsNullOrEmpty(txtMaintenanceEndDate.Text) ? "" : Validation.Convert2DateDB(txtMaintenanceEndDate.Text);
            string Period = txtPeriod.Text;
            return ServiceEquipment.getCurrentNextMaintenanceDate(BeginGuarantee, EndGuaranty, "", Period);
        }

        #endregion

        //04/02/2562 show ticket of ci by born kk
        private void bindDataTicket() {
            ServiceTicketLibrary lib = new ServiceTicketLibrary();
            //DataTable dt = lib.GetTicketList(
            //        SID, CompanyCode, "", "", "", "",
            //        "", "", "", "", "", "",
            //        txtEquipmentCode.Text, "", "", "",
            //        "", "", "", "",
            //        "",false,false
            //    );

            // 10/06/2563 fixed query ticket by coffee kk
            DataTable dt = lib.GetTicketList(
               SID, CompanyCode, "", "", "", "",
               "", "", "", "", "", "",
               EquipmentCode, "", "", "",
               "", "", "", "",
               "", false, false
           );

            rptListTicketItems.DataSource = dt;
            rptListTicketItems.DataBind();
            udpListTicketItems.Update();
            ClientService.DoJavascript("bindingDataTableJSTicket();");


        }

        public string GetRowColorAssign(string status)
        {
            string result = "";

            switch (status)
            {
                case ServiceTicketLibrary.SERVICE_CALL_STATUS_CANCEL:
                    result = "text-danger";
                    break;
                case ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE:
                    result = "text-success";
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
        }


        protected void openTicketNewWindow_Click(object sender , EventArgs e) {
            try
            {
                string CallerID = hddCallerID.Value;
                //DataRow[] drr = dtDataSearch.Select("CallerID='" + CallerID + "'");
                DataTable dt = AfterSaleService.getInstance().GetTicketDetailByTicketNumber(SID, CompanyCode, CallerID);
                if (dt.Rows.Count > 0)
                {
                    dtTempDoc.Clear();
                    int i = 1;
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow drt = dtTempDoc.NewRow();
                        drt["doctype"] = dr["Doctype"].ToString();
                        drt["docnumber"] = dr["CallerID"].ToString();
                        drt["docfiscalyear"] = dr["Fiscalyear"].ToString();
                        drt["indexnumber"] = i++;
                        dtTempDoc.Rows.Add(drt);
                    }
                    
                    getdataGoToTicketNewWindow(dt.Rows[0]["Doctype"].ToString(), dt.Rows[0]["CallerID"].ToString(), dt.Rows[0]["Fiscalyear"].ToString(), dt.Rows[0]["CustomerCode"].ToString());
                   
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


        private void getdataGoToTicketNewWindow(string doctype, string docnumber, string fiscalyear, string customer) {
            ServiceCallFastEntryCriteria link = new ServiceCallFastEntryCriteria();
            string idGen = link.redirectViewToTicketDetail(customer, doctype, docnumber, fiscalyear);
            if (!String.IsNullOrEmpty(idGen))
            {
                ServiceTicketLibrary lib_TK = new ServiceTicketLibrary();
                string PageRedirect = lib_TK.getPageTicketRedirect(
                    SID,
                    (Session["ServicecallEntity" + idGen] as tmpServiceCallDataSet).cs_servicecall_header.Rows[0]["Doctype"].ToString()
                );
                ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen) + "');");
               
            }

        }

        protected void btnSearchAsset_Click(object sender, EventArgs e)
        {
            try
            {
                BindDataAsset();
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

        protected void btnLoadBindingOwnerAssignment_Click(object sender, EventArgs e)
        {
            try
            {
                bindDataTableOwnerAssignment();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        protected void btnLoadBindingTicket_Click(object sender, EventArgs e)
        {
            try
            {
                bindDataTicket();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        protected class MasterEquipmentOwnerAssignmentModel
        {
            public string LineNumber { get; set; }

            public string OwnerType { get; set; }
            public string OwnerTypeDesc { get; set; }

            public string OwnerCode { get; set; }
            public string OwnerCodeDesc { get; set; }

            public string BeginDate { get; set; }
            public string EndDate { get; set; }

            public string SLAGroupCode { get; set; }
            public string SLAGroupDesc { get; set; }

            public string ActiveStatus { get; set; }
        }

        public string AttributesFormatModify(int showDigitNum, string attributesValue)
        {
            if (
                !Permission.ConfigurationItemAttributes && 
                !Permission.ConfigurationItemModify &&
                !String.IsNullOrEmpty(attributesValue)
                )
            {

                int replaceDigitNum = attributesValue.Length - showDigitNum;
                string replacePart = attributesValue.Substring(0, replaceDigitNum);
                attributesValue = attributesValue.Replace(replacePart, new string('X', replaceDigitNum));
            }
            return attributesValue;
        }

    }
}