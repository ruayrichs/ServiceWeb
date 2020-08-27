using agape.lib.constant;
using Agape.FocusOne.Utilities;
using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Entity;
using ERPW.Lib.Service;
using ServiceWeb.auth;
using ERPW.Lib.Authentication;
using SNA.Lib.crm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Service.Entity;
using System.Web.Script.Serialization;
using ServiceWeb.crm.AfterSale;
using ERPW.Lib.F1WebService.ICMUtils;
using System.Web.Configuration;
using ERPW.Lib.Master.Config;
using SNA.Lib.POS.utils;
using Agape.Lib.DBService;
using Newtonsoft.Json;
using ERPW.Lib.Service.Workflow;

namespace ServiceWeb.crm
{
    public partial class CustomerProfileDetail : AbstractsSANWebpage
    {

        protected override Boolean ProgramPermission_CanView()
        {
            return Permission.ContactView || Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return Permission.ContactModify || Permission.AllPermission;
        }

        private ERPW.Lib.Service.Report.TicketAnalysis ta = new ERPW.Lib.Service.Report.TicketAnalysis(); // ta ticket analysis
        private DataTable dt { get; set; }
        DBService db = new DBService();
        #region Service
        private CustomerService serviceCustomer = CustomerService.getInstance();
        private Service.EquipmentService ServiceEquipment = new Service.EquipmentService();
        private ServiceTicketLibrary libServiceTicket = new ServiceTicketLibrary();        
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        private ERPW.Lib.Master.CustomerService libCustomer = new ERPW.Lib.Master.CustomerService();
        #endregion

        #region newDashboard
        private CustomerGeneralLibrary customerGeneralLib = new CustomerGeneralLibrary();

        private DataTable _dtGeneralDataService;
        private DataTable dtGeneralDataService
        {
            get
            {
                if (_dtGeneralDataService == null)
                {
                    _dtGeneralDataService = libCustomer.getCustomerGeneralDataService(SID, CompanyCode, CustomerCode);
                }
                return _dtGeneralDataService;
            }
        }

        protected string _isCritical;
        protected bool isCritical
        {
            get
            {
                if (string.IsNullOrEmpty(_isCritical))
                {
                    _isCritical = false.ToString();
                    if (dtGeneralDataService.Rows.Count > 0 && (dtGeneralDataService.Rows[0]["CustomerCritical"] as string) == "CRITICAL")
                    {
                        _isCritical = true.ToString();
                    }
                }
                return Convert.ToBoolean(_isCritical);
            }
        }
        private CustomerDashboardLib customerLib = new CustomerDashboardLib();
        private string _CustomerCode = null;
        public string getCustomerCode
        {
            get
            {
                if (_CustomerCode == null)
                {
                    _CustomerCode = Request.QueryString["id"];
                }
                return _CustomerCode;
            }
        }

        //private CustomerDashboardFinalDataModel _DashboardFinalData;
        //private CustomerDashboardFinalDataModel DashboardFinalData
        //{
        //    get
        //    {
        //        if (_DashboardFinalData == null)
        //            _DashboardFinalData = customerLib.PreparFinanDataDashboard(SID, CompanyCode, getCustomerCode);
        //        return _DashboardFinalData;
        //    }
        //}
        #endregion

        #region data session
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
        private tmpServiceCallDataSet serviceCallEntity
        {
            get { return Session["ServicecallEntity"] == null ? new tmpServiceCallDataSet() : (tmpServiceCallDataSet)Session["ServicecallEntity"]; }
            set { Session["ServicecallEntity"] = value; }
        }
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
        #endregion

        #region Data Source
        public String CustomerCode
        {
            get
            {
                return Request["id"].Trim();
            }
        }

        private CustomerProfile _CustomerProfile = null;
        public CustomerProfile CustomerProfile
        {
            get
            {
                if (_CustomerProfile == null)
                {
                    _CustomerProfile = serviceCustomer.SearchCustomerDataByCustomerCode(
                        SID,
                        CompanyCode,
                        CustomerCode
                    );

                    if (_CustomerProfile == null)
                    {
                        _CustomerProfile = new CustomerProfile();
                    }
                }
                return _CustomerProfile;
            }
        }

        private listTicketServiceEn _ListTicketEn;
        private listTicketServiceEn ListTicketEn
        {
            get
            {
                if (_ListTicketEn == null)
                {
                    _ListTicketEn = libServiceTicket.GetAllTaskByCustomer(
                       SID,
                       CompanyCode,
                       CustomerCode
                   );
                }
                return _ListTicketEn;
            }
        }

        private DataTable _dtEquipment;
        private DataTable dtEquipment
        {
            get
            {
                if (_dtEquipment == null)
                {
                    _dtEquipment = ServiceEquipment.getListEquipmentByOwner(
                        SID,
                        CompanyCode,
                        "01",
                        CustomerCode
                    );
                }
                return _dtEquipment;
            }
        }


        private DataTable _dtSCType;
        private DataTable dtSCType
        {
            get
            {
                if (_dtSCType == null)
                {
                    _dtSCType = ServiceWeb.Service.AfterSaleService.getInstance().getSearchDoctype(
                        "", CompanyCode
                    );
                }
                return _dtSCType;
            }
        }
        DataTable dtSCType2
        {
            get { return Session["ServiceCallCriteria.SCFC_SCType"] == null ? null : (DataTable)Session["ServiceCallCriteria.SCFC_SCType"]; }
            set { Session["ServiceCallCriteria.SCFC_SCType"] = value; }
        }
        public class Address
        {
            public string PropertyCode { get; set; }
            public string Description { get; set; }
            public string PropertyValue { get; set; }
        }
        public class AddressListDetail
        {
            public List<Address> address { get; set; }
        }
        #endregion

        #region 
        protected void Page_Init(object sender, EventArgs e)
        {
            modalAddNewContact.afterSavedata += MyEventHandlerFunction_afterSaveRebindPostBack;
        }

        protected void MyEventHandlerFunction_afterSaveRebindPostBack(object sender, EventArgs e)
        {
            bindListContact();
            bindDataCustomerChangeLog();
        }
        #endregion
        private ICMUtils icmUtil = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
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
                ControlUI();

                bindOwnerService();
                bindDataAccountability();
                //setChartTicketAnalysis();
                hddAddressCodeEdit.Value = "001";
                bindDataCustomerDetail();
                bindTicketList();
                bindListContact();
                //bindListEquipment();
                bindDataPopupCreateNewTicketReferent();
                bindDataPopupEditCustomerDetail();
                bindDataCustomerChangeLog();
                //bindDataCI();
   

                if (dtGeneralDataService.Rows.Count > 0)
                {
                    chkCriticalCustomer.Checked = isCritical;
                    _txt_CD_ResponsibleOrganization.Value = (dtGeneralDataService.Rows[0]["ResponsibleOrganization"] as string);
                }
            }
        }

        AccountabilityService accountabilityService = new AccountabilityService();
        private void bindDataAccountability()
        {
            DataTable dtAcc = accountabilityService.getAccountabilityStructureV2(SID, "");

            ddlAccountability.DataSource = dtAcc;
            ddlAccountability.DataTextField = "DataText";
            ddlAccountability.DataValueField = "DataValue";
            ddlAccountability.DataBind();
            ddlAccountability.Items.Insert(0, new ListItem("", ""));
            ddlAccountability.SelectedValue = "";
        }

        #region Popup Create Ticket
        private void GetddlSctype()
        {

            try
            {
                List<string> listBusinessObject = new List<string>();
                if (Permission.IncidentView)
                {
                    listBusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_INCIDENT);
                }
                if (Permission.ProblemView)
                {
                    listBusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_PROBLEM);
                }
                if (Permission.RequestView)
                {
                    listBusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_REQUEST);
                }
                if (Permission.ChangeOrderView)
                {
                    listBusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_CHANGE);
                }

                dtSCType2 = ServiceWeb.Service.AfterSaleService.getInstance().getSearchDoctype(
                     "",
                     CompanyCode,
                     listBusinessObject
                 );

            }
            catch (Exception ex)
            {
                Response.Redirect(Page.ResolveUrl("~/"));
            }

        }

        private void bindDataPopupCreateNewTicketReferent()
        {
            lblCustomerDetail.Text = lblCustomerName.Text;
            _txt_year.Value = Validation.getCurrentServerDateTime().Year.ToString();

            ddlEquipment.Items.Clear();
            ddlEquipment.Items.Add(new ListItem("", ""));
            ddlEquipment.AppendDataBoundItems = true;
            ddlEquipment.DataTextField = "Description";
            ddlEquipment.DataValueField = "EquipmentCode";
            ddlEquipment.DataSource = dtEquipment;
            ddlEquipment.DataBind();
            GetddlSctype();
            _ddl_sctype.Items.Clear();
            _ddl_sctype.Items.Add(new ListItem("", ""));
            _ddl_sctype.AppendDataBoundItems = true;
            _ddl_sctype.DataTextField = "Description";
            _ddl_sctype.DataValueField = "DocumentTypeCode";
            _ddl_sctype.DataSource = dtSCType2;
            _ddl_sctype.DataBind();

            udpPanelCreatedNew.Update();
        }
        private void bindDataPopupEditCustomerDetail()
        {
            _txt_CD_CustomerName.Text = lblCustomerName.Text;
            _txt_CD_ForeignName.Text = "";
            //_txt_year.Value = Validation.getCurrentServerDateTime().Year.ToString();

            ddlEquipment.Items.Clear();
            ddlEquipment.Items.Add(new ListItem("", ""));
            ddlEquipment.AppendDataBoundItems = true;
            ddlEquipment.DataTextField = "Description";
            ddlEquipment.DataValueField = "EquipmentCode";
            ddlEquipment.DataSource = dtEquipment;
            ddlEquipment.DataBind();
            GetddlSctype();
            _ddl_sctype.Items.Clear();
            _ddl_sctype.Items.Add(new ListItem("", ""));
            _ddl_sctype.AppendDataBoundItems = true;
            _ddl_sctype.DataTextField = "Description";
            _ddl_sctype.DataValueField = "DocumentTypeCode";
            _ddl_sctype.DataSource = dtSCType2;
            _ddl_sctype.DataBind();

            DataTable dt = libCustomer.getCustomerDocType(SID, CompanyCode);
            _ddl_CD_CustomerGroup.DataValueField = "CustomerGroupCode";
            _ddl_CD_CustomerGroup.DataTextField = "Description";
            _ddl_CD_CustomerGroup.DataSource = dt;
            _ddl_CD_CustomerGroup.DataBind();
            //_ddl_CD_CustomerGroup.Items.Insert(0, new ListItem("ทั้งหมด", ""));
            

            if (CustomerProfile != null)
            {
                _txt_CD_CustomerCode.Text = CustomerProfile.CustomerCode;
                //_txt_CD_CustomerName.Text = CRMService.getInstance().getCustomerName(
                //    SID,
                //    CompanyCode,
                //    CustomerProfile.CustomerCode
                //);
                _txt_CD_CustomerName.Text = CustomerProfile.CustomerName;
                _txt_CD_ForeignName.Text = CustomerProfile.ForeignName;
                _ddl_CD_CustomerGroup.SelectedValue = CustomerProfile.CustomerGroup;
                ddlOwnerService.SelectedValue = CustomerProfile.OwnerService;
                ddlAccountability.SelectedValue = CustomerProfile.Accountability;

                AutoCompleteEmployee.SelectedValue = displayMember(CustomerProfile.SaleEmployeeCode);

                bool isActive = false;
                bool.TryParse(CustomerProfile.Active, out isActive);
                chkCustomerActive.Checked = isActive;

                _txt_CD_CustomerPhone.Value = "";
                _txt_CD_CustomerPhoneMoblie.Value = "";

                if (string.IsNullOrEmpty(CustomerProfile.TelNo1))
                {
                    string[] phone = CustomerProfile.TelNo1.Split(',');
                    if (phone.Length > 1)
                    {
                        _txt_CD_CustomerPhone.Value = phone[0];

                    }
                    else
                    {
                        _txt_CD_CustomerPhone.Value = CustomerProfile.TelNo1;
                    }
                }
                else
                {
                    _txt_CD_CustomerPhone.Value = CustomerProfile.TelNo1;
                }
                _txt_CD_CustomerPhoneMoblie.Value = CustomerProfile.Mobile;

                //_txt_CD_CustomerAddress.Value = displayMember(CustomerProfile.Address);
                if (string.IsNullOrEmpty(CustomerProfile.EMail))
                {
                    string[] Email = CustomerProfile.EMail.Split(',');
                    if (Email.Length > 1)
                    {
                        _txt_CD_CustomerEmail.Value = Email[0];
                    }
                    else
                    {
                        _txt_CD_CustomerEmail.Value = displayMember(CustomerProfile.EMail);
                    }
                }
                else
                {
                    _txt_CD_CustomerEmail.Value = displayMember(CustomerProfile.EMail);
                }

                if (string.IsNullOrEmpty(CustomerProfile.TaxID))
                {
                    string[] TaxID = CustomerProfile.TaxID.Split(',');
                    if (TaxID.Length > 1)
                    {
                        _txt_CD_CustomerTaxID.Value = TaxID[0];
                    }
                    else
                    {
                        _txt_CD_CustomerTaxID.Value = displayMember(CustomerProfile.TaxID);
                    }
                }
                else
                {
                    _txt_CD_CustomerTaxID.Value = displayMember(CustomerProfile.TaxID);
                    _txt_CD_CustomerTID.Value = displayMember(CustomerProfile.TaxTID);
                }

            }
            setDataAddressDetail(CustomerCode);
            udpPanelCreatedNew.Update();
        }

        protected void setDataAddressDetail(string CustomerCode)
        {
            //HiddenField hddAddressCode = (HiddenField)e.Item.FindControl("hddAddressCode");
            //Repeater rptDetail = (Repeater)e.Item.FindControl("rptDetail");
            //Repeater rptAddressEdit = (Repeater)e.Item.FindControl("rptAddressEdit");

            ContactCustomerERPW.AddressInfo AddressDetail = serviceCustomer.getCustomerAddress(
                SID,
                CompanyCode,
                CustomerCode, hddAddressCodeEdit.Value);
            rptAddAddress.DataSource = AddressDetail.listAddressProperty;
            rptAddAddress.DataBind();
            //rptAddressEdit.DataSource = AddressDetail.listAddressProperty;
            //rptAddressEdit.DataBind();
        }

        //protected void AddnewAddress()
        //{
        //    try
        //    {
        //        ContactCustomerERPW.AddressInfo listAddress = serviceCustomer.getCustomerAddress(
        //        SID,
        //        CompanyCode,
        //        CustomerCode,
        //        "");

        //        AddressListDetail addressDetail = new JavaScriptSerializer().Deserialize<AddressListDetail>(hddJsonAddress.Value);

        //        for (int i = 0; i < listAddress.listAddressProperty.Count; i++)
        //        {
        //            listAddress.listAddressProperty[i].PropertyValue = addressDetail.address[i].PropertyValue;
        //        }

        //        serviceCustomer.createNewCustomerAddress(SID,
        //            CompanyCode,
        //            CustomerCode,
        //            EmployeeCode,
        //            "",
        //            listAddress.listAddressProperty
        //            );
        //        hddJsonAddress.Value = "";
        //        //bindDataAddress();
        //        ClientService.AGSuccess("บันทึกสำเร็จ");
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientService.AGError(ObjectUtil.Err(ex.Message));
        //    }
        //    finally
        //    {
        //        ClientService.AGLoading(false);
        //    }

        //}
        protected void EditAddress()
        {
            try
            {
                AddressListDetail addressDetail = new JavaScriptSerializer().Deserialize<AddressListDetail>(hddJsonAddress.Value);
                Repeater rptAddAddress = (Repeater)FindControl("rptDetail");
                ContactCustomerERPW.AddressInfo listAddress = serviceCustomer.getCustomerAddress(
                    SID,
                    CompanyCode,
                    CustomerCode,
                    hddAddressCodeEdit.Value);

                for (int i = 0; i < listAddress.listAddressProperty.Count; i++)
                {
                    listAddress.listAddressProperty[i].PropertyValue = addressDetail.address[i].PropertyValue;
                }

                serviceCustomer.updateCustomerAddress(SID,
                    CompanyCode,
                    CustomerCode,
                    EmployeeCode,
                    hddAddressCodeEdit.Value,
                    listAddress.listAddressProperty
                    );

                hddJsonAddress.Value = "";
                hddAddressCodeEdit.Value = "";

                //bindDataAddress();
                //ClientService.AGSuccess("บันทึกสำเร็จ");

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

        protected void btnCreateNewTicketRef_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_ddl_sctype.SelectedValue))
                {
                    throw new Exception("กรุณาระบุ Ticket Type");
                }
                if (string.IsNullOrEmpty(ddlEquipment.SelectedValue))
                {
                    throw new Exception("กรุณาระบุ Equipment");
                }
                if (string.IsNullOrEmpty(_txt_year.Value))
                {
                    throw new Exception("กรุณาระบุ Fiscal Year");
                }
                CreatedNewTicket();

                //if (!string.IsNullOrEmpty(hddEquepmentCodeRef.Value))
                //{
                //    string equipmentCode = hddEquepmentCodeRef.Value;
                //    string customerCode = "";

                //    if (serviceCallEntity != null && serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
                //    {
                //        customerCode = serviceCallEntity.cs_servicecall_header.Rows[0]["CustomerCode"].ToString();
                //    }

                //    DataTable dtCheckEquipment = universalService.GetEquipmentCustomerAssignment(SID, CompanyCode,
                //        equipmentCode, customerCode);

                //    if (dtCheckEquipment.Rows.Count == 0)
                //    {
                //        throw new Exception("Equipment " + equipmentCode + " is not assigned to customer " + customerCode + " : " + GetCustomerDesc(customerCode));
                //    }

                //    CreatedNewTicket(equipmentCode);
                //}
                //else
                //{
                //    CreatedNewTicket(null);
                //}

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

        private void CreatedNewTicket()
        {
           
            
            string idGen = Guid.NewGuid().ToString().Substring(0, 8);
            //if (serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
            //{
            //    docType = _ddl_sctype.SelectedValue;
            //    docTypeDesc = GetSCTypeDesc(docType);
            //    customerName = lblCustomerName.Text;
            //    fiscalYear = _txt_year.Value;
            //}

            Session["SCT_created_doctype_code"+idGen] = _ddl_sctype.SelectedValue; 
            Session["SCT_created_doctype_desc" + idGen] = GetSCTypeDesc(_ddl_sctype.SelectedValue);
            Session["SCT_created_cust_code" + idGen] = CustomerCode;
            Session["SCT_created_cust_desc" + idGen] = lblCustomerName.Text;
            Session["SCT_created_fiscalyear" + idGen] = _txt_year.Value;
            Session["SCT_created_remark" + idGen] = null;
            Session["SCT_created_equipment" + idGen] = ddlEquipment.SelectedValue;
            Session["ServicecallEntity" + idGen] = new tmpServiceCallDataSet();
            Session["SC_MODE" + idGen] = ApplicationSession.CREATE_MODE_STRING;
          

            string PageRedirect = ServiceTicketLibrary.GetInstance().getPageTicketRedirect(
                    SID,
                    _ddl_sctype.SelectedValue
                );
            if (PageRedirect.Equals("ServiceCallTransaction.aspx"))
            {
                ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen) + "');");
            }
            else if (PageRedirect.Equals("ServiceCallTransactionChange.aspx"))
            {
                ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen) + "');");
            }
            else
            {
                Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen));
            }

            //ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/ServiceCallTransaction.aspx") + "');");
            //Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/ServiceCallTransaction.aspx"));
        }

        private string GetSCTypeDesc(string code)
        {
            DataRow[] drr = dtSCType.Select("DocumentTypeCode='" + code + "'");
            string desc = "";
            if (drr.Length > 0)
            {
                desc = drr[0]["Description"].ToString();
            }
            return desc;
        }
        #endregion 

        #region Customer Detail Left Panel
        private void bindDataCustomerDetail()
        {
            if (CustomerProfile != null)
            {
                lblCustomerCode.Text = CustomerProfile.CustomerCode;
                lblCustomerName.Text = CRMService.getInstance().getCustomerName(
                    SID,
                    CompanyCode,
                    CustomerProfile.CustomerCode
                );
                lblCustomerCode.ToolTip = lblCustomerCode.Text;
                lblCustomerName.ToolTip = lblCustomerName.Text;

                #region Check Active

                bool isActive = false;
                bool.TryParse(CustomerProfile.Active, out isActive);
                string statusActive = "red";
                if (isActive)
                {
                    statusActive = "#00ff4e";
                }
                iconCustomerActiveStatus.Style["color"] = statusActive;

                #endregion


                lblCustomerGroup.Text = displayMember(CustomerProfile.CustomerGroupDesc);
                lblCustomerSaleAdmin.Text = displayMember(CustomerProfile.SaleEmployeeName_En);

                lblCustomerAddress.Text = displayMember(CustomerProfile.Address);

                if (string.IsNullOrEmpty(CustomerProfile.TelNo1))
                {
                    string[] phone = CustomerProfile.TelNo1.Split(',');
                    if (phone.Length > 1)
                    {
                        lblCustomerPhone.Text = displayMember(phone[0]);
                    }
                    else
                    {
                        lblCustomerPhone.Text = displayMember(CustomerProfile.TelNo1);
                    }
                }
                else
                {
                    lblCustomerPhone.Text = displayMember(CustomerProfile.TelNo1);
                }

                if (string.IsNullOrEmpty(CustomerProfile.Mobile))
                {
                    string[] mobile = CustomerProfile.Mobile.Split(',');
                    if (mobile.Length > 1)
                    {
                        lblCustomerPhoneMobile.Text = displayMember(mobile[0]);
                    }
                    else
                    {
                        lblCustomerPhoneMobile.Text = displayMember(CustomerProfile.Mobile);
                    }
                }
                else 
                {
                    lblCustomerPhoneMobile.Text = displayMember(CustomerProfile.Mobile);
                }

                if (string.IsNullOrEmpty(CustomerProfile.EMail))
                {
                    string[] Email = CustomerProfile.EMail.Split(',');
                    if (Email.Length > 1)
                    {
                        lblCustomerEmail.Text = Email[0];
                    }
                    else
                    {
                        lblCustomerEmail.Text = displayMember(CustomerProfile.EMail);
                    }
                }
                else
                {
                    lblCustomerEmail.Text = displayMember(CustomerProfile.EMail);
                }

                if (string.IsNullOrEmpty(CustomerProfile.TaxID))
                {
                    string[] TaxID = CustomerProfile.TaxID.Split(',');
                    if (TaxID.Length > 1)
                    {
                        lblCustomerTaxID.Text = TaxID[0];
                    }
                    else
                    {
                        lblCustomerTaxID.Text = displayMember(CustomerProfile.TaxID);
                    }
                }
                else
                {
                    lblCustomerTaxID.Text = displayMember(CustomerProfile.TaxID);
                }

                panelCustomerGroup.ToolTip = "กลุ่มลูกค้า : " + lblCustomerGroup.Text;
                panelCustomerSaleAdmin.ToolTip = "เซลล์ที่ดูแล : " + lblCustomerSaleAdmin.Text;
                panelCustomerTaxID.ToolTip = "Tax ID : " + lblCustomerTaxID.Text;
                panelCustomerPhone.ToolTip = "เบอร์โทร : " + lblCustomerPhone.Text;
                panelCustomerPhoneMobile.ToolTip = "เบอร์มือถือ : " + lblCustomerPhoneMobile.Text;
                panelCustomerEmail.ToolTip = "อีเมล์ : " + lblCustomerEmail.Text;
                panelCustomerAddress.ToolTip = "ที่อยู่ลูกค้า : " + lblCustomerAddress.Text;

                udpCardCustomerDetail.Update();
            }
        }

        public String displayMember(String dataValue)
        {
            if (string.IsNullOrEmpty(dataValue))
            {
                return "-";
            }
            return dataValue;
        }

        private void bindOwnerService()
        {
            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);

            if (FilterOwner && !Permission.AllPermission && !Permission.ContactModify)
            {

                ddlOwnerService.Items.Clear();
                ddlOwnerService.Items.Insert(0,
                    new ListItem(
                        Permission.OwnerGroupName,
                        Permission.OwnerGroupCode
                    )
                );
                ddlOwnerService.Enabled = false;
                ddlOwnerService.CssClass = "form-control form-control-sm";
            }
            else
            {
                DataTable dtOwner = MasterConfigLibrary.GetInstance().GetMasterConfigOwnerGroup(
                    SID, CompanyCode, ""
                );

                ddlOwnerService.DataTextField = "OwnerGroupName";
                ddlOwnerService.DataValueField = "OwnerGroupCode";
                ddlOwnerService.DataSource = dtOwner;
                ddlOwnerService.DataBind();
                ddlOwnerService.Items.Insert(0, new ListItem("All", ""));
            }
        }
        #endregion 

        #region Tab Dashboard
        //private string getRaWCurrentDate()
        //{
        //    DateTime cdt = DateTime.Now;
        //    string year = cdt.Year.ToString();
        //    string month = cdt.Month.ToString();
        //    if (Int32.Parse(month) < 10)
        //    {
        //        month = 0 + month;
        //    }
        //    string day = cdt.Day.ToString();
        //    if (Int32.Parse(day) < 10)
        //    {
        //        day = 0 + day;
        //    }
        //    string hour = cdt.Hour.ToString();
        //    if (Int32.Parse(hour) < 10)
        //    {
        //        hour = 0 + hour;
        //    }
        //    string minute = cdt.Minute.ToString();
        //    if (Int32.Parse(minute) < 10)
        //    {
        //        minute = 0 + minute;
        //    }
        //    string seconds = cdt.Second.ToString();
        //    if (Int32.Parse(seconds) < 10)
        //    {
        //        seconds = 0 + seconds;
        //    }
        //    string cdate = year + month + day + hour + minute + seconds;
        //    //System.Diagnostics.Debug.WriteLine(cdate);
        //    return cdate;
        //}
        private void bindTicketList()
        {
            //lblCountOpen.Text = DashboardFinalData.OverviewDataReport.CountTicketOpen.ToString();
            //lblCountDelay.Text = DashboardFinalData.OverviewDataReport.CountTicketDelay.ToString();
            //lblCountSuccess.Text = DashboardFinalData.OverviewDataReport.CountTicketFinish.ToString();
            //lblCountAll.Text = DashboardFinalData.OverviewDataReport.CountTicketAll.ToString();

            //rptOpenTask.DataSource = DashboardFinalData.TicketListTableReport.Where(
            //    w =>
            //    w.CallStatus == "01"
            //    ).ToList();
            //rptOpenTask.DataBind();
            
            //rptDelayRisk.DataSource = DashboardFinalData.TicketListTableReport.Where(
            //    w =>
            //    w.EndDateTime != null
            //    &&
            //    Int64.Parse(Validation.getCurrentServerStringDateTime()) > Int64.Parse(w.EndDateTime)
            //    ).ToList();
            //rptDelayRisk.DataBind();

            //rptSuccessTask.DataSource = DashboardFinalData.TicketListTableReport.Where(
            //    w =>
            //    w.CallStatus == "02"
            //    ).ToList();
            //rptSuccessTask.DataBind();

            //rptAllTask.DataSource = DashboardFinalData.TicketListTableReport;
            //rptAllTask.DataBind();
        }

        protected void btnLinkTransactionSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string CallerID = hddCallerID_Criteria.Value;
                List<TicketServiceEn> enList = ListTicketEn.ListTicket_All.Where(w =>
                    w.CallerID.Equals(CallerID)
                ).ToList();

                dtTempDoc.Clear();
                int i = 1;
                enList.ForEach(r => {
                    DataRow drt = dtTempDoc.NewRow();
                    drt["doctype"] = r.Doctype;
                    drt["docnumber"] = r.CallerID;
                    drt["docfiscalyear"] = r.Fiscalyear;
                    drt["indexnumber"] = i++;
                    dtTempDoc.Rows.Add(drt);
                });

                if (enList.Count > 0)
                {
                    getdataToedit(enList[0].Doctype, enList[0].CallerID, enList[0].Fiscalyear);
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

        protected void getdataToedit(string doctype, string docnumber, string fiscalyear)
        {

            ServiceCallFastEntryCriteria link = new ServiceCallFastEntryCriteria();
            string idGen = link.redirectViewToTicketDetail(CustomerCode, doctype, docnumber, fiscalyear);
            if (!String.IsNullOrEmpty(idGen))
            {
                ServiceTicketLibrary lib_TK = new ServiceTicketLibrary();
                string PageRedirect = lib_TK.getPageTicketRedirect(
                    SID,
                    (Session["ServicecallEntity" + idGen] as tmpServiceCallDataSet).cs_servicecall_header.Rows[0]["Doctype"].ToString()
                );
                ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen) + "');");
                //Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen));
                //Response.-Redirect("/crm/AfterSale/ServiceCallTransaction.aspx?id=" + idGen, false);
            }

            //Object[] objParam = new Object[] { "1500117",
            //        (string)Session[ApplicationSession.USER_SESSION_ID],
            //        CompanyCode,doctype,docnumber,fiscalyear};

            //DataSet[] objDataSet = new DataSet[] { serviceCallEntity };
            //DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
            //if (objReturn != null)
            //{
            //    serviceCallEntity = new tmpServiceCallDataSet();
            //    serviceCallEntity.Merge(objReturn.Copy());
            //    mode_stage = ApplicationSession.CHANGE_MODE_STRING;
            //    Response.-Redirect("~/crm/AfterSale/ServiceCallTransaction.aspx", false);
            //}
        }

        public string ConvertToTime(string strDateTimeTask)
        {
            string iString = Validation.Convert2DateTimeDisplay(strDateTimeTask);

            DateTime DateTimeNow = DateTime.Now;
            DateTime DateTimeTask = DateTime.ParseExact(iString, "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-US"));

            double TotalSeconds = (DateTimeNow - DateTimeTask).TotalSeconds;
            TimeSpan t = TimeSpan.FromSeconds(TotalSeconds);

            string answer = "";
            if (t.Days > 0)
            {
                answer += t.Days + " วัน ";
            }
            if (t.Hours > 0)
            {
                answer += t.Hours + " ชม ";
            }
            if (t.Minutes > 0)
            {
                answer += t.Minutes + " นาที ";
            }
            if (t.Seconds > 0)
            {
                answer += t.Seconds + " วินาที ";
            }

            return answer;
        }
        #endregion 

        #region Tab Contact
        private void bindListContact()
        {
            DataTable dt = serviceCustomer.getListContactDetailByCustomer(
                SID,
                CompanyCode,
                CustomerCode,
                ERPWAuthentication.Permission.RoleCode == "ROLE001"// false //!ERPWAuthentication.Permission.ContactModify
            );

            rptLisContact.DataSource = dt;
            rptLisContact.DataBind();
            udpContactData.Update();
        }

        #endregion 

        #region Tab Equipment
        private void bindListEquipment()
        {
            List<ClientMappingCIModel> datas = new List<ClientMappingCIModel>();
            datas = JsonConvert.DeserializeObject<List<ClientMappingCIModel>>(
                JsonConvert.SerializeObject(dtEquipment).Replace("<", "&lt;").Replace(">", "&gt;")
            );

            datas.ForEach(r =>
            {
                r.EquipmentClass = displayMember(r.EquipmentClass);

                if (string.IsNullOrEmpty(r.BeginDate))
                    r.BeginDate = "";
                else
                    r.BeginDate = Validation.Convert2DateDisplay(r.BeginDate);

                if (string.IsNullOrEmpty(r.EndDate))
                    r.EndDate = "";
                else
                    r.EndDate = Validation.Convert2DateDisplay(r.EndDate);

                r.DataTicketValueAnalytic = getDataTicketValueAnalytic(r.EquipmentCode);
                r.DataTicketValueAnalytic_Success = getDataTicketValueAnalytic_Success(r.EquipmentCode);
            });

            divDataJson.InnerHtml = JsonConvert.SerializeObject(datas);

            //rptListEquipment.DataSource = dtEquipment;
            //rptListEquipment.DataBind();
            udpListEquipment.Update();

            ClientService.DoJavascript("bindListEquipment();");
        }

        private string _TicketLabelAnalytic;
        public string TicketLabelAnalytic
        {
            get
            {
                if (string.IsNullOrEmpty(_TicketLabelAnalytic))
                {
                    DateTime dataDate = DateTime.Now;
                    string LastMonth_1 = dataDate.Month + "/" + dataDate.Year;
                    string LastMonth_2 = dataDate.AddMonths(-1).Month + "/" + dataDate.AddMonths(-1).Year;
                    string LastMonth_3 = dataDate.AddMonths(-2).Month + "/" + dataDate.AddMonths(-2).Year;
                    _TicketLabelAnalytic = "[\"" + LastMonth_3 + "\", \"" + LastMonth_2 + "\", \"" + LastMonth_1 + "\"]";
                }
                return _TicketLabelAnalytic;
            }
        }
        public string getDataTicketValueAnalytic(string equipmentCode)
        {
            DateTime dataDate = DateTime.Now;

            string value_LastMonth_1 = ListTicketEn.ListTicket_All.Where(w =>
                w.EquipmentNo.Equals(equipmentCode) &&
                w.CREATED_ON.Substring(0, 4).Equals(dataDate.Year.ToString()) &&
                w.CREATED_ON.Substring(4, 2).Equals(dataDate.Month.ToString().PadLeft(2, '0'))
            ).Count().ToString();

            string value_LastMonth_2 = ListTicketEn.ListTicket_All.Where(w =>
                w.EquipmentNo.Equals(equipmentCode) &&
                w.CREATED_ON.Substring(0, 4).Equals(dataDate.AddMonths(-1).Year.ToString()) &&
                w.CREATED_ON.Substring(4, 2).Equals(dataDate.AddMonths(-1).Month.ToString().PadLeft(2, '0'))
            ).Count().ToString();

            string value_LastMonth_3 = ListTicketEn.ListTicket_All.Where(w =>
                w.EquipmentNo.Equals(equipmentCode) &&
                w.CREATED_ON.Substring(0, 4).Equals(dataDate.AddMonths(-2).Year.ToString()) &&
                w.CREATED_ON.Substring(4, 2).Equals(dataDate.AddMonths(-2).Month.ToString().PadLeft(2, '0'))
            ).Count().ToString();

            return "[" + value_LastMonth_3 + ", " + value_LastMonth_2 + ", " + value_LastMonth_1 + "]";
        }
        public string getDataTicketValueAnalytic_Success(string equipmentCode)
        {
            DateTime dataDate = DateTime.Now;

            string value_LastMonth_1 = ListTicketEn.ListTicket_All.Where(w =>
                w.EquipmentNo.Equals(equipmentCode) &&
                w.StatusCode.Equals("finish") &&
                w.CREATED_ON.Substring(0, 4).Equals(dataDate.Year.ToString()) &&
                w.CREATED_ON.Substring(4, 2).Equals(dataDate.Month.ToString().PadLeft(2, '0'))
            ).Count().ToString();

            string value_LastMonth_2 = ListTicketEn.ListTicket_All.Where(w =>
                w.EquipmentNo.Equals(equipmentCode) &&
                w.StatusCode.Equals("finish") &&
                w.CREATED_ON.Substring(0, 4).Equals(dataDate.AddMonths(-1).Year.ToString()) &&
                w.CREATED_ON.Substring(4, 2).Equals(dataDate.AddMonths(-1).Month.ToString().PadLeft(2, '0'))
            ).Count().ToString();

            string value_LastMonth_3 = ListTicketEn.ListTicket_All.Where(w =>
                w.EquipmentNo.Equals(equipmentCode) &&
                w.StatusCode.Equals("finish") &&
                w.CREATED_ON.Substring(0, 4).Equals(dataDate.AddMonths(-2).Year.ToString()) &&
                w.CREATED_ON.Substring(4, 2).Equals(dataDate.AddMonths(-2).Month.ToString().PadLeft(2, '0'))
            ).Count().ToString();

            return "[" + value_LastMonth_3 + ", " + value_LastMonth_2 + ", " + value_LastMonth_1 + "]";
        }
        #endregion

        #region Tab Change Log

        private void bindDataCustomerChangeLog()
        {
            DataTable dt = new LogServiceLibrary().GetCustomerLog(SID, CompanyCode, CustomerCode);
            CustomerChangeLog.BindingLog(dt);
        }

        #endregion

        protected void btnUpdateCustomerDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsAllFeature)
                {
                    if (string.IsNullOrEmpty(_txt_CD_CustomerName.Text))
                    {
                        throw new Exception("กรุณาระบุชื่อลูกค้า");
                    }
                    DataTable dtCustomerName = db.selectData("SELECT CustomerName FROM master_customer WHERE CustomerName = '" + _txt_CD_CustomerName.Text.Trim() + "' ");
                    //DataTable dtFaxID = db.selectData("SELECT FederalTaxID FROM master_customer WHERE FederalTaxID = '" + _txt_CD_CustomerTaxID.Value + "' ");

                    if (dtCustomerName.Rows.Count > 0 && dtCustomerName.Rows.Count != 1)
                    {
                        ClientService.AGError("Invalid Create customer please check Client Name");
                    }
                    //else if (dtFaxID.Rows.Count > 0 && dtFaxID.Rows.Count != 1 && _txt_CD_CustomerTaxID.Value != "")
                    //{
                    //    ClientService.AGError("Invalid Create customer please check Tax ID");
                    //}
                    else
                    {
                        UpdatadataCustomer();
                        ClientService.AGSuccess("บันทึกสำเร็จ");
                    }
                }
                else
                {
                    UpdatadataCustomer();
                    ClientService.AGSuccess("บันทึกสำเร็จ");
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

        private void UpdatadataCustomer()
        {
            if (IsAllFeature)
            {
                string docType = "";
                string docTypeDesc = "";
                string customerName = "";
                string fiscalYear = "";
                string CustomerCritical = "";
                string updateby = EmployeeCode;
                string CustomeroCode = _txt_CD_CustomerCode.Text;
                string customername = _txt_CD_CustomerName.Text;
                string foreignname = _txt_CD_ForeignName.Text;
                string customergroup = _ddl_CD_CustomerGroup.SelectedValue;
                string CustomerSaleAdmins = AutoCompleteEmployee.SelectedValue;
                string OwnerService = ddlOwnerService.SelectedValue;
                bool isActive = chkCustomerActive.Checked;
                //string customerAddress = _txt_CD_CustomerAddress.Value;
                string customerAddress = "";
                string customertaxID = _txt_CD_CustomerTaxID.Value;
                string customerTID = _txt_CD_CustomerTID.Value;
                string customerphone = _txt_CD_CustomerPhone.Value;
                string customerphoneMobile = _txt_CD_CustomerPhoneMoblie.Value;
                string customeremail = _txt_CD_CustomerEmail.Value;
                string updateon = Validation.getCurrentServerStringDateTime();
                string Remark2 = "";
                
                string accountability = ddlAccountability.SelectedValue;

                if (chkCriticalCustomer.Checked)
                {
                    Remark2 = chkCriticalCustomer.Value;
                }
                serviceCustomer.UpdatadataCustomer(SID, CompanyCode, CustomeroCode, customername, customergroup, CustomerSaleAdmins, customerAddress, customerAddress
                    , customertaxID, customerTID, customerphone, customerphoneMobile
                    , customeremail, updateby, updateon, foreignname, isActive, OwnerService, Remark2, accountability
                );
                EditAddress();

                if (chkCriticalCustomer.Checked)
                {
                    CustomerCritical = chkCriticalCustomer.Value;
                }
                else
                {
                    CustomerCritical = "";
                }

                serviceCustomer.UpdatadataCustomerServiceWeb(
                    SID, CompanyCode, CustomeroCode, OwnerService, CustomerCritical
                );

                serviceCustomer.updateCustomerGeneralDataService(
                    SID, CompanyCode, CustomeroCode, CustomerCritical, _txt_CD_ResponsibleOrganization.Value, EmployeeCode
                );

                bindDataCustomerDetail();
                bindDataCustomerChangeLog();
                ClientService.DoJavascript("closeInitiativeModal('modal-EditCustomerDetail');");

            }
            else
            {
                string CustomeroCode = _txt_CD_CustomerCode.Text;
                string OwnerService = ddlOwnerService.SelectedValue;
                string CustomerCritical = "";
                if (chkCriticalCustomer.Checked)
                {
                    CustomerCritical = chkCriticalCustomer.Value;
                }
                else
                {
                    CustomerCritical = "";
                }

                serviceCustomer.UpdatadataCustomerServiceWeb(
                    SID, CompanyCode, CustomeroCode, OwnerService, CustomerCritical
                );

                serviceCustomer.updateCustomerGeneralDataService(
                    SID, CompanyCode, CustomeroCode, CustomerCritical, _txt_CD_ResponsibleOrganization.Value, EmployeeCode
                );

                bindDataCustomerDetail();
                bindDataCustomerChangeLog();
                ClientService.DoJavascript("closeInitiativeModal('modal-EditCustomerDetail');");
            }
        }

        #region Convert Properties

        private List<MasterConfigEntity> _AddressMapping;
        private List<MasterConfigEntity> AddressMapping
        {
            get
            {
                if (_AddressMapping == null)
                {
                    _AddressMapping = new List<MasterConfigEntity>();
                    _AddressMapping.Add(new MasterConfigEntity() { xKey = "00", xValue = "Text TH" });
                    _AddressMapping.Add(new MasterConfigEntity() { xKey = "01", xValue = "Number" });
                    _AddressMapping.Add(new MasterConfigEntity() { xKey = "02", xValue = "Number" });
                    _AddressMapping.Add(new MasterConfigEntity() { xKey = "03", xValue = "Text TH" });
                    _AddressMapping.Add(new MasterConfigEntity() { xKey = "04", xValue = "Text TH" });
                    _AddressMapping.Add(new MasterConfigEntity() { xKey = "05", xValue = "Text TH" });
                    _AddressMapping.Add(new MasterConfigEntity() { xKey = "06", xValue = "Text TH" });
                    _AddressMapping.Add(new MasterConfigEntity() { xKey = "07", xValue = "Text TH" });
                    _AddressMapping.Add(new MasterConfigEntity() { xKey = "08", xValue = "Number" });
                }
                return _AddressMapping;
            }
        }


        public string GetPlaceHolderAddress(object xValue)
        {
            var en = AddressMapping.Find(x => x.xKey == xValue.ToString());
            return en == null ? "" : en.xValue;
        }
        #endregion

        //public void setChartTicketAnalysis()
        //{

        //    //string id = Request.QueryString["id"];
        //    //System.Diagnostics.Debug.WriteLine("id: " + Request.QueryString["id"]);
        //    dt = ta.ticketAnalysis(Request.QueryString["id"]);

        //    //data2.Text = dt.Rows[0]["close"].ToString();
        //    rptMyChart.DataSource = dt;
        //    rptMyChart.DataBind();
        //    udpticketAnalysis.Update();
        //    //
        //}
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
            finally
            {
                ClientService.AGLoading(false);
            }
        }
        //protected void btnOpenDetailEquipment_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Session["ServiceWeb.Page.Equipment.EquipmentCode"] = hddEquipmentCode.Value;
        //        Session["ServiceWeb.Page.Equipment.Page_Mode"] = hddPage_Mode.Value;
        //        ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/Master/Equipment/EquipmentDetail.aspx") + "')");
        //        //Response.Redirect(Page.ResolveUrl("~/crm/Master/Equipment/EquipmentDetail.aspx"), false);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //14/01/2562 export excel contact data by born kk
        protected void ExportExcelContactData_click(object sender,EventArgs e) {
            try
            {
                DataTable dt = serviceCustomer.getListContactDetailByCustomer(
                   SID,
                   CompanyCode,
                   CustomerCode,
                   ERPWAuthentication.Permission.RoleCode == "ROLE001" // false // !ERPWAuthentication.Permission.ContactModify
                );
                DataView view = new DataView(dt);
                DataTable ContactDataExportTable = view.ToTable(true, "NAME1", "AUTH_CONTACT_NAME", "email", "phone", "NickName", "POSITION", "REMARK");
                ContactDataExportTable.Columns["NAME1"].ColumnName = "ชื่อ-สกุล";
                ContactDataExportTable.Columns["AUTH_CONTACT_NAME"].ColumnName = "สิทธิ์ที่ได้รับ*";
                ContactDataExportTable.Columns["email"].ColumnName = "อีเมล์";
                ContactDataExportTable.Columns["phone"].ColumnName = "เบอร์ติดต่อ";
                ContactDataExportTable.Columns["NickName"].ColumnName = "Service";
                ContactDataExportTable.Columns["POSITION"].ColumnName = "ตำแหน่ง";
                ContactDataExportTable.Columns["REMARK"].ColumnName = "หมายเหตุ";

                Session["report.excel.Report_Excel_Export_Datatable"] = ContactDataExportTable;
                Session["report.excel.Report_Excel_Export_Name"] = "ContactDataReport";
                ClientService.DoJavascript("downloadExcelContactData();");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
            finally {
                ClientService.AGLoading(false);
            }
        }



        //06/02/2562 data configuration item by born kk
        private void bindDataCI() 
        {
             ServiceWeb.Service.EquipmentService lib = new ServiceWeb.Service.EquipmentService();

             dt = lib.getListCIOfCustomer(SID, CompanyCode, CustomerCode, "01");
            dt.Columns.Add("StatusDesc");
            dt.Columns.Add("CategoryDesc");
            foreach (DataRow dtr in dt.Rows) {

                dtr["StatusDesc"] = TranslaterEMStatus( dtr["Status"].ToString());
                
                dtr["CategoryDesc"] = TranslaterEMCategory(dtr["CategoryCode"].ToString());
            }


            rptListCIItems.DataSource = dt;
            rptListCIItems.DataBind();
            udpnListCIItems.Update();
            
            ClientService.DoJavascript("bindingDataTableJSCI();");
        }
        private string TranslaterEMStatus(string status) {
           DataRow[] dtr = dtEquipmentStatus.Select("StatusCode = '"+status+"'");
            if (dtr.Count() > 0) {
                return dtr[0]["StatusName"].ToString();
            }
            return "";
        }

        private string TranslaterEMCategory(string code)
        {
            if (code == "00")
            {
                return "Main Configuration Item";
            }
            //if (code == "01") {
            //    return "Sub Configuration Item";
            //}
            if (code == "02")
            {
                return "Virtual Configuration Item";
            }
            return code;
        }

        private void ControlUI()
        {
            if (IsAllFeature)
            {
                _txt_CD_CustomerName.Enabled = true;
                _txt_CD_ForeignName.Enabled = true;
                AutoCompleteEmployee.Enabled = true;
                chkCustomerActive.Disabled = false;
                _txt_CD_CustomerTaxID.Disabled = false;
                _txt_CD_CustomerPhone.Disabled = false;
                _txt_CD_CustomerPhoneMoblie.Disabled = false;
                _txt_CD_CustomerEmail.Disabled = false;
            }
            else
            {
                _txt_CD_CustomerName.Enabled = false;
                _txt_CD_ForeignName.Enabled = false;
                AutoCompleteEmployee.Enabled = false;
                chkCustomerActive.Disabled = true;
                _txt_CD_CustomerTaxID.Disabled = true;
                _txt_CD_CustomerPhone.Disabled = true;
                _txt_CD_CustomerPhoneMoblie.Disabled = true;
                _txt_CD_CustomerEmail.Disabled = true;
            }
        }

        public class ClientMappingCIModel
        {
            public Int32 ItemIndex { get; set; }
            public string EquipmentCode { get; set; }
            public string Description { get; set; }
            public string EquipmentClass { get; set; }
            public string BeginDate { get; set; }
            public string EndDate { get; set; }
            public string DataTicketValueAnalytic { get; set; }
            public string DataTicketValueAnalytic_Success { get; set; }
        }
    }
}