using ServiceWeb.auth;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Agape.FocusOne.Utilities;
using Agape.Lib.Web.Bean.MM;
using SNA.Lib.POS.utils;
using Newtonsoft.Json;
using ERPW.Lib.Master;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Authentication;


namespace ServiceWeb.crm.Master.Equipment
{
    public partial class Equipment_CI_Request_Criteria : AbstractsSANWebpage //System.Web.UI.Page
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.ConfigurationItemView || ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.ConfigurationItemModify || ERPWAuthentication.Permission.AllPermission;
        }

        protected override string getProgramID()
        {
            return LogServiceLibrary.PROGRAM_ID_EQUIPMENT;
        }

        private EquipmentService ServiceEquipment = new EquipmentService();
        private UniversalService ServiceUniversal = new UniversalService();        
        private ICMUtils icmUtil = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        private MasterConfigLibrary masterConfig = MasterConfigLibrary.GetInstance();

        private DataTable dtEquipmentType_;
        private DataTable dtEquipmentType
        {
            get
            {
                if (dtEquipmentType_ == null)
                {
                    dtEquipmentType_ = ServiceUniversal.getEquipmentType(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode
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
                    Object[] objParam = new Object[] { "0800066", POSDocumentHelper.getSessionId(ERPWAuthentication.SID, ERPWAuthentication.UserName) };
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
            DataTable dt = ServiceEquipment.getEMClass(ERPWAuthentication.SID);
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
            DataTable dtOwner = masterConfig.GetMasterConfigOwnerGroup(ERPWAuthentication.SID,ERPWAuthentication.CompanyCode,"");
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
        private void bindDataEquipment()
        {
            List<EquipmentService.CI_Item> listCIItem = ServiceEquipment.getListCI(
                ERPWAuthentication.SID,
                txtCIBatch.Text,
                txtSONumber.Text,
                txtShipToCode.Text,
                txtShipToName.Text,
                txtMaterialCode.Text,
                txtMeterialName.Text,
                "",
                "",
                "",
                ""
            );
            foreach (EquipmentService.CI_Item dr in listCIItem)
            {
                if (string.IsNullOrEmpty(Convert.ToString(dr.StartDate)))
                {
                    dr.StartDate = "";
                }
                if (string.IsNullOrEmpty(Convert.ToString(dr.EndDate)))
                {
                    dr.EndDate = "";
                }
            }

            var dataSource = listCIItem;
            //var dataSource = listCIItem.Select(s => new
            //{
            //    s.EquipmentCode,
            //    s.Description,
            //    s.EquipmentTypeName,
            //    s.Status,
            //    s.EquipmentClassName,
            //    s.CategoryCode,
            //    s.OwnerGroupName
            //});

            //divTranslaterStatus.InnerHtml = JsonConvert.SerializeObject(dtEquipmentStatus);
            //divJsonEquipmentList.InnerHtml = JsonConvert.SerializeObject(dataSource);
            
            rptItems.DataSource = dataSource;
            rptItems.DataBind();
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

        protected void btnSearchData_Click(object sender, EventArgs e)
        {
            try
            {
                bindDataEquipment();
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
        public List<CI_Item_Detail> getDetailCIList() {
            List <CI_Item_Detail> ListCheck = new List<CI_Item_Detail> { };
            foreach (RepeaterItem i in rptItems.Items)
            {
                CheckBox chk = i.FindControl("_chk_select") as CheckBox;
                if (chk.Checked)
                {
                    
                    string matname = (i.FindControl("hddList_matname") as HiddenField).Value.ToString();
                    string CIBatch = (i.FindControl("hddList_ci") as HiddenField).Value.ToString();
                    string SONumber = (i.FindControl("hddList_so") as HiddenField).Value.ToString();
                    string SOType = (i.FindControl("hddList_type") as HiddenField).Value.ToString();
                    string SoItemNo = (i.FindControl("hddList_item") as HiddenField).Value.ToString();
                    string FiscalYear = (i.FindControl("hddList_FiscalYear") as HiddenField).Value.ToString();
                    string ShipToCode = (i.FindControl("hddList_ShipToCode") as HiddenField).Value.ToString();
                    string ShipToName = (i.FindControl("hddList_ShipToName") as HiddenField).Value.ToString();
                    string StartDate = (i.FindControl("hddList_startdate") as HiddenField).Value.ToString();
                    string EndDate = (i.FindControl("hddList_enddate") as HiddenField).Value.ToString();
                    ListCheck.Add(new CI_Item_Detail
                    {
                        CIBatch = CIBatch,
                        SONumber = SONumber,
                        SOType = SOType,
                        SoItemNo = SoItemNo,
                        MeterialName = matname,
                        ShipToName = ShipToName,
                        StartDate = StartDate,
                        EndDate = EndDate,
                        Fiscalyear = FiscalYear,
                        ShipToCode = ShipToCode
                    });
                }
            }
            return ListCheck;
        }
        protected void btnOpenModalCreated_Click(object sender, EventArgs e)
        {
            try
            {
                clearTextCreateEquipment();
                string mat_str = "";
                ListCheck = new List<CI_Item_Detail> { };
                bool chk_count = false;
                //List<string> listso = new List<string> { };

                foreach (RepeaterItem i in rptItems.Items)
                {
                    CheckBox chk = i.FindControl("_chk_select") as CheckBox;
                    if (chk.Checked)
                    {
                        chk_count = true;
                        string matname = (i.FindControl("hddList_matname") as HiddenField).Value.ToString();
                        string CIBatch = (i.FindControl("hddList_ci") as HiddenField).Value.ToString();
                        string SONumber = (i.FindControl("hddList_so") as HiddenField).Value.ToString();
                        string SOType = (i.FindControl("hddList_type") as HiddenField).Value.ToString();
                        string SoItemNo = (i.FindControl("hddList_item") as HiddenField).Value.ToString();
                        string FiscalYear = (i.FindControl("hddList_FiscalYear") as HiddenField).Value.ToString();
                        string ShipToCode = (i.FindControl("hddList_ShipToCode") as HiddenField).Value.ToString();
                        string ShipToName = (i.FindControl("hddList_ShipToName") as HiddenField).Value.ToString();
                        string StartDate = (i.FindControl("hddList_startdate") as HiddenField).Value.ToString();
                        string EndDate = (i.FindControl("hddList_enddate") as HiddenField).Value.ToString();
                        if (string.IsNullOrEmpty(mat_str))
                        {
                            mat_str = matname;
                        }
                        else
                        {
                            mat_str += "," + matname;
                        }
                        ListCheck.Add(new CI_Item_Detail {
                            CIBatch = CIBatch,
                            SONumber = SONumber,
                            SOType = SOType,
                            SoItemNo = SoItemNo,
                            MeterialName = matname,
                            ShipToName = ShipToName,
                            StartDate = StartDate,
                            EndDate = EndDate,
                            Fiscalyear = FiscalYear,
                            ShipToCode = ShipToCode
                        });
                    }
                }
                //List<CI_Item_Detail> temp = ListCheck.Distinct();
                if (ListCheck.Select(s => s.ShipToCode).ToList().GroupBy(g => g).Count() > 1)
                {
                    throw new Exception("รายการที่เลือก มีผู้ซื้อมากกว่า 1");
                }

                if (ListCheck.Select(s => s.SONumber).ToList().GroupBy(g => g).Count() > 1)
                {
                    throw new Exception("รายการที่เลือก มี SO Number มากกว่า 1");
                }
                if (chk_count) {
                    txtEquipmentName_Created.Text = mat_str;
                    ClientService.DoJavascript("showInitiativeModal('modalAddEquipment');");
                }
                else
                {
                    ClientService.AGError("กรุณาเลือก CI ที่ต้องการสร้าง Equipment");
                }
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
                List<CI_Item_Detail> List = getDetailCIList();
                prepareDataEquipment(CreateDate, List);
               
                string EquipmentCode = saveDataEquipment();
                foreach(var i in List)
                {
                    ServiceEquipment.UpdateCIItem(ERPWAuthentication.SID,i.CIBatch,i.SONumber,i.SOType,i.SoItemNo,i.Fiscalyear);
                }

                clearTextCreateEquipment();

                ClientService.DoJavascript("closeInitiativeModal('modalAddEquipment');");
                bindDataEquipment();
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
                return ServiceUniversal.isAutoNumberRefEquipmentType(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, ddlEquipmentType_Created.SelectedValue);
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
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
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

        private void prepareDataEquipment(string CreateDate, List<CI_Item_Detail> List)
        {
            dsEquipment = new tmpEquipmentSetupDataSet();
            #region master_equipment
            DataRow drNew = dsEquipment.master_equipment.NewRow();
            drNew["SID"] = ERPWAuthentication.SID;
            drNew["CompanyCode"] = ERPWAuthentication.CompanyCode;
            drNew["EquipmentCode"] = txtEquipmentCode_Created.Text;
            drNew["EquipmentType"] = ddlEquipmentType_Created.SelectedValue;
            drNew["Description"] = txtEquipmentName_Created.Text;
            drNew["Status"] = ddlStatus.SelectedValue;
            drNew["CategoryCode"] = ddlCategory.SelectedValue;
            drNew["Valid_From"] = Validation.Convert2DateDB(txtEquipmentDateFrom.Text);
            drNew["Valid_To"] = Validation.Convert2DateDB(txtEquipmentDateTo.Text);
            drNew["CREATED_ON"] = CreateDate;
            drNew["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
            dsEquipment.master_equipment.Rows.Add(drNew);
            #endregion
            CI_Item_Detail itemF = List.First();
            #region
            DataRow drOwner = dsEquipment.master_equipment_owner_assignment.NewRow();
            drOwner["SID"] = ERPWAuthentication.SID;
            drOwner["CompanyCode"] = ERPWAuthentication.CompanyCode;
            drOwner["LineNumber"] = "001";
            drOwner["ActiveStatus"] = "True";
            drOwner["BeginDate"] = itemF.StartDate;//Validation.Convert2DateDB(txtOwnerBiginDate.Text);
            drOwner["EndDate"] = itemF.EndDate;//Validation.Convert2DateDB(txtOwnerEndDate.Text);
            drOwner["OwnerCode"] = itemF.ShipToCode; //hddCustomerCode.Value;
            drOwner["OwnerDesc"] = itemF.ShipToName;//lblOwnerAssignment.Text;
            drOwner["OwnerType"] = "01";
            drOwner["CREATED_ON"] = CreateDate;
            drOwner["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
            dsEquipment.master_equipment_owner_assignment.Rows.Add(drOwner);
            #endregion

            #region add row Empty
            if (dsEquipment.master_equipment_general.Rows.Count == 0)
            {
                DataRow dr_add = dsEquipment.master_equipment_general.NewRow();
                dr_add["SID"] = ERPWAuthentication.SID;
                dr_add["CompanyCode"] = ERPWAuthentication.CompanyCode;
                dr_add["EquipmentCode"] = txtEquipmentCode_Created.Text;
                dr_add["EquipmentClass"] = ddlEMClass.SelectedValue;
                dr_add["EquipmentObjectType"] = ddlOwnerService_Created.SelectedValue;
                dr_add["CREATED_ON"] = CreateDate;
                dr_add["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
                dsEquipment.master_equipment_general.Rows.Add(dr_add);
            }
            if (dsEquipment.master_equipment_location.Rows.Count == 0)
            {
                DataRow dr_add = dsEquipment.master_equipment_location.NewRow();
                dr_add["SID"] = ERPWAuthentication.SID;
                dr_add["CompanyCode"] = ERPWAuthentication.CompanyCode;
                dr_add["EquipmentCode"] = txtEquipmentCode_Created.Text;
                dr_add["CREATED_ON"] = CreateDate;
                dr_add["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
                dsEquipment.master_equipment_location.Rows.Add(dr_add);
            }
            if (dsEquipment.master_equipment_organization.Rows.Count == 0)
            {
                DataRow dr_add = dsEquipment.master_equipment_organization.NewRow();
                dr_add["SID"] = ERPWAuthentication.SID;
                dr_add["CompanyCode"] = ERPWAuthentication.CompanyCode;
                dr_add["EquipmentCode"] = txtEquipmentCode_Created.Text;
                dr_add["CREATED_ON"] = CreateDate;
                dr_add["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
                dsEquipment.master_equipment_organization.Rows.Add(dr_add);
            }
            int equipmentitemno = 1, length = 4 ;

            foreach (var i in List)
            {
                DataRow dr_add = dsEquipment.master_equipment_sale_dist.NewRow();
                dr_add["SID"] = ERPWAuthentication.SID;
                dr_add["CompanyCode"] = ERPWAuthentication.CompanyCode;
                dr_add["EquipmentCode"] = txtEquipmentCode_Created.Text;
                dr_add["CREATED_ON"] = CreateDate;
                dr_add["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
                dr_add["BLDocnumber"] = i.SONumber;
                dr_add["BLDoctype"] = i.SOType;
                dr_add["BLFiscalyear"] = i.Fiscalyear;
                //dr_add["CompanyCode"] = i.ShipToCode;
                dr_add["BLItemNo"] = i.SoItemNo;
                string lineno = equipmentitemno.ToString().PadLeft(length, '0');
                dr_add["LineNumber"] = lineno;
                dsEquipment.master_equipment_sale_dist.Rows.Add(dr_add);
                equipmentitemno++;
            }
            //if (dsEquipment.master_equipment_sale_dist.Rows.Count == 0)
            //{
            //    DataRow dr_add = dsEquipment.master_equipment_sale_dist.NewRow();
            //    dr_add["SID"] = ERPWAuthentication.SID;
            //    dr_add["CompanyCode"] = ERPWAuthentication.CompanyCode;
            //    dr_add["EquipmentCode"] = txtEquipmentCode_Created.Text;
            //    dr_add["LineNumber"] = "0001";
            //    dr_add["CREATED_ON"] = CreateDate;
            //    dr_add["CREATED_BY"] = ERPWAuthentication.EmployeeCode;
            //    dr_add["BLDocnumber"] = "";
            //    dr_add["BLDoctype"] = "";
            //    dr_add["BLFiscalyear"] = "";
            //    dr_add["CompanyCode"] = "";
            //    dr_add["BLItemNo"] = "";

            //    dsEquipment.master_equipment_sale_dist.Rows.Add(dr_add);
            //}
            #endregion

        }

        private string saveDataEquipment()
        {
            Object[] objParam = new Object[] { "0800037", POSDocumentHelper.getSessionId(ERPWAuthentication.SID, ERPWAuthentication.UserName) };
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

        private void updateListItem()
        {
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
                //Session["ServiceWeb.Page.Equipment.EquipmentCode"] = hddEquipmentCode.Value;
                //Session["ServiceWeb.Page.Equipment.Page_Mode"] = hddPage_Mode.Value;
                //Response.-Redirect("~/crm/Master/Equipment/EquipmentDetail.aspx", false);
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void btnCheckRequiredForSave_Click(object sender, EventArgs e)
        {
            ClientService.DoJavascript("$('#btnSaveNewEquipment').click();"); 
        }


        #region Class
        public List<CI_Item_Detail> ListCheck;

        [Serializable]
        public class CI_Item_Detail
        {
            public string CIBatch { get; set; }
            public string SONumber { get; set; }
            public string SOType { get; set; }
            public string SoItemNo { get; set; }
            public string MeterialName { get; set; }
            public string ShipToCode { get; set; }
            public string ShipToName { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string Fiscalyear { get; set; }

        }
        #endregion


    }
}