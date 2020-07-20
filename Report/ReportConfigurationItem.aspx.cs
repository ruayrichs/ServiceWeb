using agape.lib.web.ICMV2;
using Agape.FocusOne.Utilities;
using Agape.Lib.Web.Bean.MM;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Service.Entity;
using Newtonsoft.Json;
using ServiceWeb.auth;
using ServiceWeb.Service;
using SNA.Lib.POS.utils;
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
    public partial class ReportConfigurationItem : AbstractsSANWebpage//System.Web.UI.Page
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{

        //}

        //private List<EquipmentService.EquipmentItemData> eItemData { get; set; }        

        protected override Boolean ProgramPermission_CanView()
        {
            return Permission.ConfigurationItemView || Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return Permission.ConfigurationItemModify || Permission.AllPermission;
        }

        protected override string getProgramID()
        {
            return LogServiceLibrary.PROGRAM_ID_EQUIPMENT;
        }

        private EquipmentService ServiceEquipment = new EquipmentService();
        private UniversalService ServiceUniversal = new UniversalService();
        private ICMUtils icmUtil = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        private MasterConfigLibrary masterConfig = MasterConfigLibrary.GetInstance();
        private OwnerService ownerService = new OwnerService();

        private DataTable dtEquipmentType_;
        private DataTable dtEquipmentType
        {
            get
            {
                if (dtEquipmentType_ == null)
                {
                    dtEquipmentType_ = ServiceUniversal.getEquipmentType(
                        SID,
                        CompanyCode
                    );
                }
                return dtEquipmentType_;
            }
        }

        private DataTable dtEquipmentStatus_;
        private DataTable dtEquipmentStatus
        {
            get
            {
                if (dtEquipmentStatus_ == null)
                {
                    Object[] objParam = new Object[] { "0800066", POSDocumentHelper.getSessionId(SID, UserName) };
                    DataSet[] ds = new DataSet[] { };
                    DataSet objReturn = icmUtil.ICMDataSetInvoke(objParam, ds);
                    if (objReturn.Tables.Count > 0)
                    {
                        dtEquipmentStatus_ = objReturn.Tables[0].DefaultView.ToTable(true, "StatusCode", "StatusName");
                    }
                    else
                    {
                        dtEquipmentStatus_ = new DataTable();
                    }
                }

                return dtEquipmentStatus_;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindDataDDLEquipmentType();
                //bindDataEquipmentType();
                BindDDLStatus();
                bindEMClass();
                bindOwnerService();
                bindDdlAsset();
                autoSearchData_Click();
            }
        }

        private void bindDataDDLEquipmentType()
        {
            ddlEquipmentType.DataSource = dtEquipmentType;
            ddlEquipmentType.DataBind();

            ddlEquipmentType.Items.Insert(0, new ListItem("All", ""));

            ddlEquipmentType_Created.DataSource = dtEquipmentType;
            ddlEquipmentType_Created.DataBind();
            ddlEquipmentType_Created.Items.Insert(0, new ListItem("เลือก", ""));
        }

        private void BindDDLStatus()
        {
            DataTable dt = dtEquipmentStatus;
            ddlEquipmentStatus.DataTextField = "StatusName";
            ddlEquipmentStatus.DataValueField = "StatusCode";
            ddlEquipmentStatus.DataSource = dt;
            ddlEquipmentStatus.DataBind();
            ddlEquipmentStatus.Items.Insert(0, new ListItem("All", ""));

            ddlStatus.DataTextField = "StatusName";
            ddlStatus.DataValueField = "StatusCode";
            ddlStatus.DataSource = dt;
            ddlStatus.DataBind();
            try
            {
                ddlEquipmentStatus.SelectedValue = "";
                ddlStatus.SelectedValue = "N";
            }
            catch (Exception) { }
        }
        private void bindEMClass()
        {
            DataTable dt = ServiceEquipment.getEMClass(SID);
            ddlEMClass.DataSource = dt;
            ddlEMClass.DataValueField = "ClassCode";
            ddlEMClass.DataTextField = "ClassName";
            ddlEMClass.DataBind();
            ddlEMClass.Items.Insert(0, new ListItem("เลือก", ""));

            ddlSearch_EMClass.DataSource = dt;
            ddlSearch_EMClass.DataValueField = "ClassCode";
            ddlSearch_EMClass.DataTextField = "ClassName";
            ddlSearch_EMClass.DataBind();
            ddlSearch_EMClass.Items.Insert(0, new ListItem("All", ""));

        }
        private void bindOwnerService()
        {

            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);

            // #Edit for Multi OwnerService CI

            if (FilterOwner && !Permission.AllPermission)
            {
                ddlOwnerService.Items.Clear();
                ddlOwnerService.Items.Insert(0,
                    new ListItem(
                        Permission.OwnerGroupName,
                        Permission.OwnerGroupCode
                    )
                );

                ddlOwnerService_Created.Items.Clear();
                ddlOwnerService_Created.Items.Insert(0,
                    new ListItem(
                        Permission.OwnerGroupName,
                        Permission.OwnerGroupCode
                    )
                );


                // #Edit for Multi OwnerService Customer

                DataTable dtDataUserOwnerService = ownerService.getMappingOwner(UserName);//get data ownerService


                ddlOwnerService.DataTextField = "OwnerGroupName";
                ddlOwnerService.DataValueField = "OwnerService";
                ddlOwnerService.DataSource = dtDataUserOwnerService;
                ddlOwnerService.DataBind();
                ddlOwnerService.SelectedIndex = 0;

                ddlOwnerService_Created.DataTextField = "OwnerGroupName";
                ddlOwnerService_Created.DataValueField = "OwnerService";
                ddlOwnerService_Created.DataSource = dtDataUserOwnerService;
                ddlOwnerService_Created.DataBind();
                ddlOwnerService_Created.SelectedIndex = 0;

                if (dtDataUserOwnerService.Rows.Count == 1)
                {
                    ddlOwnerService.Enabled = false;
                    ddlOwnerService.CssClass = "form-control form-control-sm";

                    ddlOwnerService_Created.Enabled = false;
                    ddlOwnerService_Created.CssClass = "form-control form-control-sm";
                }
            }
            else
            {

                DataTable dtOwner = masterConfig.GetMasterConfigOwnerGroup(SID, CompanyCode, "");
                ddlOwnerService_Created.DataTextField = "OwnerGroupName";
                ddlOwnerService_Created.DataValueField = "OwnerGroupCode";
                ddlOwnerService_Created.DataSource = dtOwner;
                ddlOwnerService_Created.DataBind();
                ddlOwnerService_Created.Items.Insert(0, new ListItem("เลือก", ""));

                ddlOwnerService.DataTextField = "OwnerGroupName";
                ddlOwnerService.DataValueField = "OwnerGroupCode";
                ddlOwnerService.DataSource = dtOwner;
                ddlOwnerService.DataBind();
                ddlOwnerService.Items.Insert(0, new ListItem("All", ""));
            }
        }

        private void CheckSingleQuote()
        {
            string txtECode = txtEquipmentCode.Text;
            string txtEName = txtEquipmentName.Text;
            int Count = 0;
            //txtEquipmentCode.Text;
            foreach (char c in txtECode)
            {
                if (txtECode.Contains("'"))
                {
                    Count++;
                }
            }
            if (Count > 0)
            {
                txtECode = "null";
                txtEquipmentCode.Text = txtECode.Replace("null", "\"");
            }
            //txtEquipmentCode.Text;
            foreach (char c in txtEName)
            {
                if (txtEName.Contains("'"))
                {
                    Count++;
                }
            }
            if (Count > 0)
            {
                txtEName = "null";
                txtEquipmentName.Text = txtEName.Replace("null", "\"");
            }
        }

        private void bindDataEquipment()
        {

            CheckSingleQuote();

            DataTable ciSelect = new DataTable();
            List<EquipmentService.EquipmentItemData> listEquipmentItem = ServiceEquipment.getListEquipment(
                SID,
                CompanyCode,
                txtEquipmentCode.Text,
                txtEquipmentName.Text,
                ddlEquipmentType.SelectedValue,
                ddlEquipmentStatus.SelectedValue,
                ddlSearch_EMClass.SelectedValue,
                ddlSearch_Category.SelectedValue,
                ddlOwnerService.SelectedValue,
                txtSerialNo.Text,
                Session["Search.Send.Mail"].ToString(),
                txtTimeSendMail.Text,
                Session["User.Name"].ToString(),
                txtxValue001.Text,
                ciSelect
            );

            var dataSource = listEquipmentItem.Select(s => new
            {
                s.EquipmentCode,
                s.Description,
                s.EquipmentTypeName,
                s.Status,
                s.EquipmentClassName,
                s.CategoryCode,
                s.OwnerGroupName,
                s.xValue001,
                s.xValue002,
                s.xValue003,
                s.xValue004,
                s.xValue005,
                s.ModelNumber,
                s.ManufacturerSerialNO,
                s.BeginGuarantee,
                s.EndGuaranty,
                s.BeginWarrantee,
                s.EndWarrantee,
                s.CiLocation
            });

            //this.eItemData = listEquipmentItem;
            //Session["Export_Excel_CI_Datatable"] = listEquipmentItem; 

            divTranslaterStatus.InnerHtml = JsonConvert.SerializeObject(dtEquipmentStatus);
            divJsonEquipmentList.InnerHtml = JsonConvert.SerializeObject(dataSource);
            udpListEquipment.Update();
            ClientService.DoJavascript("afterSearch();");
        }

        public string TranslaterStatus(string Code)
        {
            foreach (DataRow dr in dtEquipmentStatus.Rows)
            {
                if (Convert.ToString(dr["StatusCode"]) == Code)
                {
                    return Convert.ToString(dr["StatusName"]);
                }
            }

            return Code;
        }
        public string TranslaterEquipmentType(string Code)
        {
            foreach (DataRow dr in dtEquipmentType.Select("MaterialGroupCode = '" + Code + "'"))
            {
                Code = dr["Description"].ToString();
            }

            return Code;
        }

        protected void autoSearchData_Click() 
        {
            try
            {
                object sender = new object();
                Button btn = new Button();
                btn.Text = "Search";

                Session["Search.Send.Mail"] = "Search";
                Session["User.Name"] = UserName;

                bindDataEquipment();
                if (btn.Text == "Update Send Mail")
                {
                    ClientService.AGSuccess("บันทึกสำเร็จ");
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

        protected void btnSearchData_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                Session["Search.Send.Mail"] = btn.Text;
                Session["User.Name"] = UserName;

                bindDataEquipment();
                if (btn.Text == "Update Send Mail")
                {
                    ClientService.AGSuccess("บันทึกสำเร็จ");
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

        #region Created Equipment
        private tmpEquipmentSetupDataSet dsEquipment
        {
            get
            {
                if (Session["UniversalServiceMaster.tmpEquipmentSetupDataSet.dsEquipment"] == null)
                {
                    Session["UniversalServiceMaster.tmpEquipmentSetupDataSet.dsEquipment"] = new tmpEquipmentSetupDataSet();
                }
                return (tmpEquipmentSetupDataSet)Session["UniversalServiceMaster.tmpEquipmentSetupDataSet.dsEquipment"];
            }
            set
            {
                Session["UniversalServiceMaster.tmpEquipmentSetupDataSet.dsEquipment"] = value;
            }
        }

        protected void btnOpenModalCreated_Click(object sender, EventArgs e)
        {
            try
            {
                clearTextCreateEquipment();
                ClientService.DoJavascript("showInitiativeModal('modalAddEquipment');");
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
        protected void btnSaveNewEquipment_Click(object sender, EventArgs e)
        {
            try
            {
                Session["ServiceWeb.Page.Equipment.EquipmentCode"] = hddEquipmentCode.Value;
                Session["ServiceWeb.Page.Equipment.Page_Mode"] = hddPage_Mode.Value;

                ValidatField();
                if (ValidateDuplicateEquipmentCode())
                {
                    throw new Exception("ไม่สามารถสร้างรายการที่มี Configuration Item Code ซ้ำกันได้");
                }

                string CreateDate = Validation.getCurrentServerStringDateTime();
                prepareDataEquipment(CreateDate);
                string EquipmentCode = saveDataEquipment();
                clearTextCreateEquipment();

                ClientService.DoJavascript("closeInitiativeModal('modalAddEquipment');");
                ClientService.AGSuccess("สร้าง Configuration Item No " + EquipmentCode + " สำเร็จ.");
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

        protected void btnSetDoctypeSwitchComfigMode_Click(object sender, EventArgs e)
        {
            try
            {
                bool isGen = isGenDocAuto;
                txtEquipmentCode_Created.Enabled = !isGen;
                if (isGen)
                {
                    txtEquipmentCode_Created.CssClass = "form-control form-control-sm ";
                }
                else
                {
                    txtEquipmentCode_Created.CssClass = "form-control form-control-sm required";
                }
                udpCreateEquipment.Update();

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


        private bool isGenDocAuto
        {
            get
            {
                if (string.IsNullOrEmpty(ddlEquipmentType_Created.SelectedValue))
                {
                    return false;
                }
                return ServiceUniversal.isAutoNumberRefEquipmentType(SID, CompanyCode, ddlEquipmentType_Created.SelectedValue);
            }
        }
        private void ValidatField()
        {
            List<string> listErr = new List<string>();
            if (!isGenDocAuto)
            {
                if (string.IsNullOrEmpty(txtEquipmentCode_Created.Text))
                {
                    listErr.Add("กรุณากรอก Configuration Item Code");
                }
            }

            if (string.IsNullOrEmpty(txtEquipmentName_Created.Text))
            {
                listErr.Add("กรุณากรอก Configuration Item Name");
            }
            if (string.IsNullOrEmpty(ddlEquipmentType_Created.SelectedValue))
            {
                listErr.Add("กรุณาเลือก Configuration Item Type");
            }
            if (string.IsNullOrEmpty(txtEquipmentDateFrom.Text) || string.IsNullOrEmpty(txtEquipmentDateTo.Text))
            {
                listErr.Add("กรุณาเลือกระบุวันที่");
            }
            else
            {
                int DateForm = Convert.ToInt32(Validation.Convert2DateDB(txtEquipmentDateFrom.Text));
                int DateTo = Convert.ToInt32(Validation.Convert2DateDB(txtEquipmentDateTo.Text));
                if (DateTo < DateForm)
                {
                    listErr.Add("วันที่เริ่มต้องไม่น้อยกว่าวันที่สิ้นสุด");
                }
            }

            if (string.IsNullOrEmpty(ddlEMClass.SelectedValue))
            {
                listErr.Add("กรุณา EM Class");
            }
            if (string.IsNullOrEmpty(ddlCategory.SelectedValue))
            {
                listErr.Add("กรุณาเลือก Category");
            }
            if (string.IsNullOrEmpty(ddlStatus.SelectedValue))
            {
                listErr.Add("กรุณาเลือก Status");
            }

            if (listErr.Count > 0)
            {
                throw new Exception("<div>" + string.Join("</br>", listErr) + "</div>");
            }
        }

        private Boolean ValidateDuplicateEquipmentCode()
        {
            List<EquipmentService.EquipmentItemData> listEquipmentItem = ServiceEquipment.getListEquipment(
                SID,
                CompanyCode,
                txtEquipmentCode_Created.Text,
                "",
                "",
                ""
            );
            if (listEquipmentItem.Where(w => w.EquipmentCode.Equals(txtEquipmentCode_Created.Text)).Count() > 0)
            {
                return true;
            }

            return false;
        }

        private void prepareDataEquipment(string CreateDate)
        {
            dsEquipment = new tmpEquipmentSetupDataSet();
            #region master_equipment
            DataRow drNew = dsEquipment.master_equipment.NewRow();
            drNew["SID"] = SID;
            drNew["CompanyCode"] = CompanyCode;
            drNew["EquipmentCode"] = txtEquipmentCode_Created.Text;
            drNew["EquipmentType"] = ddlEquipmentType_Created.SelectedValue;
            drNew["Description"] = txtEquipmentName_Created.Text;
            drNew["Status"] = ddlStatus.SelectedValue;
            drNew["CategoryCode"] = ddlCategory.SelectedValue;
            drNew["Valid_From"] = Validation.Convert2DateDB(txtEquipmentDateFrom.Text);
            drNew["Valid_To"] = Validation.Convert2DateDB(txtEquipmentDateTo.Text);
            drNew["CREATED_ON"] = CreateDate;
            drNew["CREATED_BY"] = EmployeeCode;
            dsEquipment.master_equipment.Rows.Add(drNew);
            #endregion

            #region
            //DataRow drOwner = dsEquipment.master_equipment_owner_assignment.NewRow();
            //drOwner["SID"] = SID;
            //drOwner["CompanyCode"] = CompanyCode;
            //drOwner["LineNumber"] = "001";
            //drOwner["ActiveStatus"] = "True";
            //drOwner["BeginDate"] = "20180101";//Validation.Convert2DateDB(txtOwnerBiginDate.Text);
            //drOwner["EndDate"] = "20181231";//Validation.Convert2DateDB(txtOwnerEndDate.Text);
            //drOwner["OwnerCode"] = "N0110021"; //hddCustomerCode.Value;
            //drOwner["OwnerDesc"] = "IRPC Public Company Limited";//lblOwnerAssignment.Text;
            //drOwner["OwnerType"] = "01";
            //drOwner["CREATED_ON"] = CreateDate;
            //drOwner["CREATED_BY"] = EmployeeCode;
            //dsEquipment.master_equipment_owner_assignment.Rows.Add(drOwner);
            #endregion

            #region add row Empty
            if (dsEquipment.master_equipment_general.Rows.Count == 0)
            {
                DataRow dr_add = dsEquipment.master_equipment_general.NewRow();
                dr_add["SID"] = SID;
                dr_add["CompanyCode"] = CompanyCode;
                dr_add["EquipmentCode"] = txtEquipmentCode_Created.Text;
                dr_add["EquipmentClass"] = ddlEMClass.SelectedValue;
                dr_add["EquipmentObjectType"] = ddlOwnerService_Created.SelectedValue;
                dr_add["CREATED_ON"] = CreateDate;
                dr_add["CREATED_BY"] = EmployeeCode;
                dsEquipment.master_equipment_general.Rows.Add(dr_add);
            }
            if (dsEquipment.master_equipment_location.Rows.Count == 0)
            {
                DataRow dr_add = dsEquipment.master_equipment_location.NewRow();
                dr_add["SID"] = SID;
                dr_add["CompanyCode"] = CompanyCode;
                dr_add["EquipmentCode"] = txtEquipmentCode_Created.Text;
                dr_add["CREATED_ON"] = CreateDate;
                dr_add["CREATED_BY"] = EmployeeCode;
                dsEquipment.master_equipment_location.Rows.Add(dr_add);
            }
            if (dsEquipment.master_equipment_organization.Rows.Count == 0)
            {
                DataRow dr_add = dsEquipment.master_equipment_organization.NewRow();
                dr_add["SID"] = SID;
                dr_add["CompanyCode"] = CompanyCode;
                dr_add["AccAsset1"] = hddAccountAssignmentBox_AssetCode.Value;
                dr_add["EquipmentCode"] = txtEquipmentCode_Created.Text;
                dr_add["CREATED_ON"] = CreateDate;
                dr_add["CREATED_BY"] = EmployeeCode;
                dsEquipment.master_equipment_organization.Rows.Add(dr_add);
            }

            // Zaan Comment Code 04.12.2018
            //if (dsEquipment.master_equipment_sale_dist.Rows.Count == 0)
            //{
            //    DataRow dr_add = dsEquipment.master_equipment_sale_dist.NewRow();
            //    dr_add["SID"] = SID;
            //    dr_add["CompanyCode"] = CompanyCode;
            //    dr_add["LineNumber"] = "0001";
            //    dr_add["EquipmentCode"] = txtEquipmentCode_Created.Text;
            //    dr_add["CREATED_ON"] = CreateDate;
            //    dr_add["CREATED_BY"] = EmployeeCode;
            //    dsEquipment.master_equipment_sale_dist.Rows.Add(dr_add);
            //}
            #endregion

        }

        private string saveDataEquipment()
        {
            Object[] objParam = new Object[] { "0800037", POSDocumentHelper.getSessionId(SID, UserName) };
            DataSet[] objDataSetChange = new DataSet[] { dsEquipment };
            Object Result = icmUtil.ICMPrimitiveInvoke(objParam, objDataSetChange);

            string ResultCode = "";
            string[] arrResult = Result.ToString().Split('#');
            if (arrResult.Length == 2)
            {
                ResultCode = arrResult[1];
            }

            return ResultCode;
        }

        private void clearTextCreateEquipment()
        {
            ddlEquipmentType_Created.SelectedIndex = 0;
            ddlOwnerService_Created.SelectedIndex = 0;
            txtEquipmentName_Created.Text = "";
            txtEquipmentCode_Created.Text = "";
            txtEquipmentCode_Created.CssClass = "form-control form-control-sm";
            txtEquipmentCode_Created.Enabled = false;
            //txtEquipmentDateFrom.Text = "";
            //txtEquipmentDateTo.Text = "";
            txtEquipmentDateFrom.Text = Validation.Convert2DateDisplay(Validation.getCurrentServerStringDateTime().Substring(0, 8));
            txtEquipmentDateTo.Text = "01/12/9999";
            ddlEMClass.SelectedIndex = 0;
            ddlCategory.SelectedIndex = 0;
            try
            {
                ddlStatus.SelectedValue = "N";
            }
            catch (Exception)
            {

            }
            udpCreateEquipment.Update();
        }

        #endregion

        protected void btnOpenDetailEquipment_Click(object sender, EventArgs e)
        {
            try
            {
                Session["ServiceWeb.Page.Equipment.EquipmentCode"] = hddEquipmentCode.Value;
                Session["ServiceWeb.Page.Equipment.Page_Mode"] = hddPage_Mode.Value;
                ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/Master/Equipment/EquipmentDetail.aspx") + "')");
                //Response.Redirect(Page.ResolveUrl("~/crm/Master/Equipment/EquipmentDetail.aspx"), false);
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnCheckRequiredForSave_Click(object sender, EventArgs e)
        {
            ClientService.DoJavascript("$('#btnSaveNewEquipment').click();");
        }

        private void ExampleRepeater()
        {
            DataTable ciSelect = new DataTable();
            //List<EquipmentService.EquipmentItemData> listEquipmentItem = (List<EquipmentService.EquipmentItemData>)Session["Export_Excel_CI_Datatable"];
            List<EquipmentService.EquipmentItemData> listEquipmentItem = ServiceEquipment.getListEquipment(
                SID,
                CompanyCode,
                txtEquipmentCode.Text,
                txtEquipmentName.Text,
                ddlEquipmentType.SelectedValue,
                ddlEquipmentStatus.SelectedValue,
                ddlSearch_EMClass.SelectedValue,
                ddlSearch_Category.SelectedValue,
                ddlOwnerService.SelectedValue,
                txtSerialNo.Text,
                "",
                "",
                "",
                "",
                ciSelect
            );

            CIModel ciModel;
            List<CIModel> ciList = new List<CIModel>();
            ciList.Clear();
            for (int index = 0; index < listEquipmentItem.Count; index++)
            {
                //
                ciModel = new CIModel();
                ciModel.ConfigurationItemCode = listEquipmentItem[index].EquipmentCode;
                ciModel.ConfigurationItemName = listEquipmentItem[index].Description;
                ciModel.Family = listEquipmentItem[index].EquipmentTypeName;
                ciModel.Class = listEquipmentItem[index].EquipmentClassName;
                ciModel.OwnerService = listEquipmentItem[index].OwnerGroupName;
                ciModel.Location = listEquipmentItem[index].Location;
                ciModel.Floor = "";
                ciModel.Room = listEquipmentItem[index].Room; ;
                ciModel.Shelf = listEquipmentItem[index].Shelf;

                ciModel.ModelNumber = listEquipmentItem[index].ModelNumber;
                ciModel.ManufacturerSerialNO = listEquipmentItem[index].ManufacturerSerialNO;
                ciModel.BeginGuarantee = listEquipmentItem[index].BeginGuarantee;
                ciModel.EndGuaranty = listEquipmentItem[index].EndGuaranty;
                ciModel.BeginWarrantee = listEquipmentItem[index].BeginWarrantee;
                ciModel.EndWarrantee = listEquipmentItem[index].EndWarrantee;
                ciModel.CiLocation = listEquipmentItem[index].CiLocation;
                ciModel.xValue001 = listEquipmentItem[index].xValue001;
                ciModel.xValue002 = listEquipmentItem[index].xValue002;
                ciModel.xValue003 = listEquipmentItem[index].xValue003;
                ciModel.xValue004 = listEquipmentItem[index].xValue004;
                ciModel.xValue005 = listEquipmentItem[index].xValue005;
                //
                ciList.Add(ciModel);
            }

            Session["Export_Excel_CI_Datatable"] = ciList.toDataTable();
            Session["Export_Excel_CI_Name"] = "Configuration Item";
            ClientService.DoJavascript("exportExcelAPI();");
        }

        protected void exportData_Click(object sender, EventArgs e)
        {
            try
            {
                ExampleRepeater();
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


        // 22/01/2562 add func selection chage for set CI and CI family (by born kk)
        protected void SelectionCIFamily_Change(object sender, EventArgs e)
        {
            string eqGroup = ddlEquipmentType.SelectedValue;
            DataTable dt = ServiceEquipment.getEMClass(SID);
            DataTable dtSelect = new DataTable();
            if (dt.Select("EquipmentGroup = '" + eqGroup + "'").Count() > 0)
            {

                dtSelect = dt.Select("EquipmentGroup = '" + eqGroup + "'").CopyToDataTable<DataRow>();
                ddlSearch_EMClass.DataSource = dtSelect;
                ddlSearch_EMClass.DataValueField = "ClassCode";
                ddlSearch_EMClass.DataTextField = "ClassName";
                ddlSearch_EMClass.DataBind();
                ddlSearch_EMClass.Items.Insert(0, new ListItem("All", ""));
            }
            else
            {
                ddlSearch_EMClass.DataSource = dtSelect;
                ddlSearch_EMClass.DataBind();
                ddlSearch_EMClass.Items.Insert(0, new ListItem("All", ""));

            }

            updDdlEmclassSearch.Update();


        }

        protected void SelectionCI_Change(object sender, EventArgs e)
        {
            string classCode = ddlSearch_EMClass.SelectedValue;
            DataTable dt = ServiceEquipment.getEMClass(SID);
            DataRow[] dtr = dt.Select("ClassCode = '" + classCode + "'");
            if (dtr.Count() > 0)
            {
                ddlEquipmentType.SelectedValue = dtr[0]["EquipmentGroup"].ToString();
            }
            udpddlEquipmenttype.Update();
        }

        protected void SelectionCIFamilyCreate_Change(object sender, EventArgs e)
        {
            string eqGroup = ddlEquipmentType_Created.SelectedValue;
            DataTable dt = ServiceEquipment.getEMClass(SID);
            DataTable dtSelect = new DataTable();
            if (dt.Select("EquipmentGroup = '" + eqGroup + "'").Count() > 0)
            {

                dtSelect = dt.Select("EquipmentGroup = '" + eqGroup + "'").CopyToDataTable<DataRow>();
                ddlEMClass.DataSource = dtSelect;
                ddlEMClass.DataValueField = "ClassCode";
                ddlEMClass.DataTextField = "ClassName";
                ddlEMClass.DataBind();
                ddlEMClass.Items.Insert(0, new ListItem("เลือก", ""));
            }
            else
            {
                ddlEMClass.DataSource = dtSelect;
                ddlEMClass.DataBind();
                ddlEMClass.Items.Insert(0, new ListItem("เลือก", ""));

            }
            ClientService.AGLoading(false);

        }
        protected void SelectionCICreate_Change(object sender, EventArgs e)
        {
            string classCode = ddlEMClass.SelectedValue;
            DataTable dt = ServiceEquipment.getEMClass(SID);
            DataRow[] dtr = dt.Select("ClassCode = '" + classCode + "'");
            if (dtr.Count() > 0)
            {
                ddlEquipmentType_Created.SelectedValue = dtr[0]["EquipmentGroup"].ToString();
            }
            ClientService.DoJavascript("inactiveRequireField();");
            btnSetDoctypeSwitchComfigMode_Click(null, EventArgs.Empty);
        }


        private void bindDdlAsset()
        {
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

            //ddlAsset.DataValueField = "AssetCode";
            //ddlAsset.DataTextField = "AssetSubCodeDescription";
            //ddlAsset.DataSource = dt;
            //ddlAsset.DataBind();
            //ddlAsset.Items.Insert(0, new ListItem("เลือก", ""));

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
    }
}