using agape.lib.constant;
using Agape.FocusOne.Utilities;
using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Service;
using Newtonsoft.Json;
using ServiceWeb.auth;
using ServiceWeb.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Master.Config;
using Newtonsoft.Json.Linq;
using ERPW.Lib.Master.Entity;
using SNA.Lib.POS.utils;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Master;
using System.Web.Configuration;

namespace ServiceWeb.crm.AfterSale
{
    public partial class ServiceCallChangeCriteria : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return Permission.ChangeOrderView || Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return Permission.ChangeOrderModify || Permission.AllPermission;
        }

        protected override string getProgramID()
        {
            return null;
        }


        private ServiceTicketLibrary lib = new ServiceTicketLibrary();
        //private SNA.Lib.Transaction.ChatService chatService = SNA.Lib.Transaction.ChatService.getInstance();        
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        private UniversalService universalService = new UniversalService();
        private MasterConfigLibrary config = MasterConfigLibrary.GetInstance();
        private EquipmentService ServiceEquipment = new EquipmentService();
        private ERPW.Lib.Master.CustomerService serviceCustomer = ERPW.Lib.Master.CustomerService.getInstance();
        public string workGroupCode = "20170121162748444411";
        public static string CHAT_BUSINESSOBJ = "SC";

        //private tmpServiceCallDataSet serviceCallEntity
        //{
        //    get { return Session["ServicecallEntity"] == null ? new tmpServiceCallDataSet() : (tmpServiceCallDataSet)Session["ServicecallEntity"]; }
        //    set { Session["ServicecallEntity"] = value; }
        //}

        public string _SCT_created_cust_code;
        public string SCT_created_cust_code
        {
            get
            {
                if (_SCT_created_cust_code == null)
                {
                    _SCT_created_cust_code = (string)Session["SCT_created_cust_code"];
                }
                return _SCT_created_cust_code;
            }
            set
            {
                Session["SCT_created_cust_code"] = value;
            }
        }

        #region DT Session Member Data

        //DataTable dtTempAttachfile
        //{
        //    get
        //    {
        //        return Session["dtTempAttachfile"] == null ? null : (DataTable)Session["dtTempAttachfile"];
        //    }
        //    set { Session["dtTempAttachfile"] = value; }
        //}

        private DataTable _dtTempAttachfile;
        private DataTable dtTempAttachfile
        {
            get
            {
                if (_dtTempAttachfile == null)
                {
                    _dtTempAttachfile = AfterSaleService.getInstance().getCountAttachFile(SID, CompanyCode, "");
                }
                return _dtTempAttachfile;
            }
        }

        //DataTable dtDataAssign
        //{
        //    get { return Session["ServiceCallChangeCriteria.SCFC_dtDataAssign"] == null ? null : (DataTable)Session["ServiceCallChangeCriteria.SCFC_dtDataAssign"]; }
        //    set { Session["ServiceCallChangeCriteria.SCFC_dtDataAssign"] = value; }
        //}

        private DataTable _dtDataAssign;
        private DataTable dtDataAssign
        {
            get
            {
                if (_dtDataAssign == null)
                {
                    _dtDataAssign = AfterSaleService.getInstance().getServicecallAssignForYou(SID, CompanyCode, EmployeeCode);

                    if (!_dtDataAssign.Columns.Contains("total_attachfile"))
                        _dtDataAssign.Columns.Add("total_attachfile");
                    if (!_dtDataAssign.Columns.Contains("total_messagechat"))
                        _dtDataAssign.Columns.Add("total_messagechat");
                }
                return _dtDataAssign;
            }
        }

        //DataTable dtDataResolution
        //{
        //    get { return Session["ServiceCallChangeCriteria.SCFC_dtDataResolution"] == null ? null : (DataTable)Session["ServiceCallChangeCriteria.SCFC_dtDataResolution"]; }
        //    set { Session["ServiceCallChangeCriteria.SCFC_dtDataResolution"] = value; }
        //}

        private DataTable _dtDataResolution;
        private DataTable dtDataResolution
        {
            get
            {
                if (_dtDataResolution == null)
                {
                    _dtDataResolution = AfterSaleService.getInstance().getServicecallWaitForClose(SID, CompanyCode, EmployeeCode);

                    if (!_dtDataResolution.Columns.Contains("total_attachfile"))
                        _dtDataResolution.Columns.Add("total_attachfile");
                    if (!_dtDataResolution.Columns.Contains("total_messagechat"))
                        _dtDataResolution.Columns.Add("total_messagechat");
                }
                return _dtDataResolution;
            }
            //set { Session["ServiceCallCriteria.SCFC_dtDataResolution"] = value; }
        }

        DataTable dtDataSearch
        {
            get { return Session["ServiceCallChangeCriteria.SCFC_dtDataSearch"] == null ? null : (DataTable)Session["ServiceCallChangeCriteria.SCFC_dtDataSearch"]; }
            set { Session["ServiceCallChangeCriteria.SCFC_dtDataSearch"] = value; }
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

        //DataTable dtSCType
        //{
        //    get { return Session["ServiceCallChangeCriteria.SCFC_SCType"] == null ? null : (DataTable)Session["ServiceCallChangeCriteria.SCFC_SCType"]; }
        //    set { Session["ServiceCallChangeCriteria.SCFC_SCType"] = value; }
        //}

        //DataTable dtContactPerson
        //{
        //    get { return Session["ServiceCallChangeCriteria.SCFC_dtContactPerson"] == null ? null : (DataTable)Session["ServiceCallChangeCriteria.SCFC_dtContactPerson"]; }
        //    set { Session["ServiceCallChangeCriteria.SCFC_dtContactPerson"] = value; }
        //}

        //DataTable dtGroup
        //{
        //    get { return Session["ServiceCallChangeCriteria.SCFC_dtGroup"] == null ? null : (DataTable)Session["ServiceCallChangeCriteria.SCFC_dtGroup"]; }
        //    set { Session["ServiceCallChangeCriteria.SCFC_dtGroup"] = value; }
        //}

        //DataTable dtProblemType
        //{
        //    get { return Session["ServiceCallChangeCriteria.SCFC_dtProblemType"] == null ? null : (DataTable)Session["ServiceCallChangeCriteria.SCFC_dtProblemType"]; }
        //    set { Session["ServiceCallChangeCriteria.SCFC_dtProblemType"] = value; }
        //}

        //DataTable dtStatus
        //{
        //    get { return Session["ServiceCallChangeCriteria.SCFC_dtStatus"] == null ? null : (DataTable)Session["ServiceCallChangeCriteria.SCFC_dtStatus"]; }
        //    set { Session["ServiceCallChangeCriteria.SCFC_dtStatus"] = value; }
        //}

        //DataTable dtTicketDocStatus
        //{
        //    get { return Session["SCT_dtTicketDocStatus"] == null ? null : (DataTable)Session["SCT_dtTicketDocStatus"]; }
        //    set { Session["SCT_dtTicketDocStatus"] = value; }
        //}


        //DataTable dtPriority
        //{
        //    get { return Session["ServiceCallChangeCriteria.SCT_dtPriority"] == null ? null : (DataTable)Session["ServiceCallChangeCriteria.SCT_dtPriority"]; }
        //    set { Session["ServiceCallChangeCriteria.SCT_dtPriority"] = value; }

        //}

        private DataTable _dtSCType;
        private DataTable dtSCType
        {
            get
            {
                if (_dtSCType == null)
                {
                    _dtSCType = AfterSaleService.getInstance().getSearchDoctype("", CompanyCode, true, true);
                }
                return _dtSCType;
            }
        }


        private DataTable _dtPriority;
        private DataTable dtPriority
        {
            get
            {
                if (_dtPriority == null)
                {
                    _dtPriority = lib.GetSeverity(SID, "", "", "");
                }
                return _dtPriority;
            }
        }
        #endregion

        //public string mode_stage
        //{
        //    get
        //    {
        //        if (Session["SC_MODE"] == null)
        //        { Session["SC_MODE"] = ApplicationSession.CREATE_MODE_STRING; }
        //        return (string)Session["SC_MODE"];
        //    }
        //    set { Session["SC_MODE"] = value; }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadinitData();
                GetddlSctype();
                //GetddlStatus();
                //GetTicketDocStatus();

                GetImpact();
                GetUrgency();
                GetPriority();

                //setDefaulsearchPageLoad();

                _txt_year.Value = Validation.getCurrentServerDateTime().Year.ToString();
                //_ddl_status.SelectedValue = "";//_ddl_status.SelectedValue = "01";
                //_ddl_ticket_Doc_Status.SelectedValue = "00";

                Session["SCT_created_equipment"] = null;
                _ddl_contact_person.initialDataAutoComplete(new DataTable(), "", "", false);

                btnSearch_Click(null, null);
            }
        }

        #region Bind Data To Screen
        private void loadinitData()
        {
            //dtTempAttachfile = AfterSaleService.getInstance().getCountAttachFile(SID, CompanyCode, "");
            //DataTable _dtDataAssign = AfterSaleService.getInstance().getServicecallAssignForYou(SID, CompanyCode, EmployeeCode);
            //DataTable _dtDataResolution = AfterSaleService.getInstance().getServicecallWaitForClose(SID, CompanyCode, EmployeeCode);

            //if (!_dtDataAssign.Columns.Contains("total_attachfile"))
            //    _dtDataAssign.Columns.Add("total_attachfile");
            //if (!_dtDataAssign.Columns.Contains("total_messagechat"))
            //    _dtDataAssign.Columns.Add("total_messagechat");
            //if (!_dtDataResolution.Columns.Contains("total_attachfile"))
            //    _dtDataResolution.Columns.Add("total_attachfile");
            //if (!_dtDataResolution.Columns.Contains("total_messagechat"))
            //    _dtDataResolution.Columns.Add("total_messagechat");

            //dtDataAssign = _dtDataAssign.Clone();
            //dtDataResolution = _dtDataResolution.Clone();

            foreach (DataRow dr in dtDataAssign.Rows)
            {
                DataRow[] drr = dtDataAssign.Select("CallerID='" + dr["CallerID"].ToString() + "'");
                if (drr.Length <= 0)
                {
                    dr["DOCDATE"] = Validation.Convert2RadDateDisplay(dr["DOCDATE"].ToString()).ToString("dd/MM/yyyy");
                    dr["total_attachfile"] = getCountAttachFile(dr["CallerID"].ToString(), dr["Fiscalyear"].ToString());
                    dr["total_messagechat"] = getCountMessageChatter(dr["Doctype"].ToString(), dr["CallerID"].ToString(), dr["Fiscalyear"].ToString());
                    dtDataAssign.ImportRow(dr);
                }
            }

            foreach (DataRow dr in dtDataResolution.Rows)
            {
                DataRow[] drr = dtDataResolution.Select("CallerID='" + dr["CallerID"].ToString() + "'");
                if (drr.Length <= 0)
                {
                    dr["DOCDATE"] = Validation.Convert2RadDateDisplay(dr["DOCDATE"].ToString()).ToString("dd/MM/yyyy");
                    dr["total_attachfile"] = getCountAttachFile(dr["CallerID"].ToString(), dr["Fiscalyear"].ToString());
                    dr["total_messagechat"] = getCountMessageChatter(dr["Doctype"].ToString(), dr["CallerID"].ToString(), dr["Fiscalyear"].ToString());
                    dtDataResolution.ImportRow(dr);
                }
            }
        }

        private void GetddlSctype()
        {
            _ddl_sctype.Items.Clear();
            //dtSCType = AfterSaleService.getInstance().getSearchDoctype("", CompanyCode, true, true);
            _ddl_sctype.Items.Add(new ListItem("", ""));
            _ddl_sctype.AppendDataBoundItems = true;
            _ddl_sctype.DataTextField = "Description";
            _ddl_sctype.DataValueField = "DocumentTypeCode";
            _ddl_sctype.DataSource = dtSCType;
            _ddl_sctype.DataBind();
        }

        private void GetImpact()
        {
            DataTable dt = lib.GetImpactMaster(SID);
            ddlImpact.DataTextField = "ImpactName";
            ddlImpact.DataValueField = "ImpactCode";
            ddlImpact.DataSource = dt;
            ddlImpact.DataBind();
            ddlImpact.Items.Insert(0, new ListItem("", ""));
        }

        private void GetUrgency()
        {
            DataTable dt = lib.GetUrgencyMaster(SID);
            ddlUrgency.DataTextField = "UrgencyName";
            ddlUrgency.DataValueField = "UrgencyCode";
            ddlUrgency.DataSource = dt;
            ddlUrgency.DataBind();
            ddlUrgency.Items.Insert(0, new ListItem("", ""));
        }

        private void GetPriority()
        {
            //DataTable dt = lib.GetPriorityMaster(SID);
            //ddlPriority.DataTextField = "Description";
            //ddlPriority.DataValueField = "PriorityCode";
            //ddlPriority.DataSource = dt;
            //ddlPriority.DataBind();
            //ddlPriority.Items.Insert(0, new ListItem("", ""));

            //dtPriority = lib.GetSeverity(SID, "", "", "");
            DataTable dt = dtPriority.Clone();
            if (dtPriority.Rows.Count > 0)
            {
                dt = dtPriority.DefaultView.ToTable(true, "PriorityCode", "Description");
            }
            ddlPriority.DataSource = dt;
            ddlPriority.DataBind();
            ddlPriority.Items.Insert(0, new ListItem("", ""));

            if (dtPriority.Rows.Count > 0)
            {
                ddlPriority.SelectedIndex = 0;
            }

        }

        //private void GetddlStatus()
        //{
        //    _ddl_status.Items.Clear();

        //    dtStatus = AfterSaleService.getInstance().getCallStatus("");
        //    _ddl_status.Items.Add(new ListItem("", ""));
        //    _ddl_status.AppendDataBoundItems = true;
        //    _ddl_status.DataTextField = "Description";
        //    _ddl_status.DataValueField = "NAME";
        //    _ddl_status.DataSource = dtStatus;
        //    _ddl_status.DataBind();
        //}


        #endregion

        //private void GetTicketDocStatus()
        //{
        //    //string BusinessObject = lib.GetBusinessObjectFromTicketType(SID, ticketType);
        //    dtTicketDocStatus = lib.GetTicketDocStatus(SID, CompanyCode, true);

        //    _ddl_ticket_Doc_Status.DataTextField = "DocumentStatusDesc";
        //    _ddl_ticket_Doc_Status.DataValueField = "DocumentStatus";
        //    _ddl_ticket_Doc_Status.DataSource = dtTicketDocStatus;
        //    _ddl_ticket_Doc_Status.DataBind();
        //    _ddl_ticket_Doc_Status.Items.Insert(0, new ListItem("", ""));
        //}

        protected void _ddl_customer_code_SelectedIndexChanged(object sender, EventArgs e)
        {
            //getcontact_person();
        }

        private void getcontact_person()
        {
            string custcode = CustomerSelect.SelectedValue.Trim();

            DataTable dt = new DataTable();

            if (custcode == "")
            {
                dt.Columns.Add("BOBJECTLINK");
                dt.Columns.Add("NAME1");
                dt.Columns.Add("email");
                dt.Columns.Add("phone");
                dt.Columns.Add("remark");
            }
            else
            {
                dt = AfterSaleService.getInstance().getContactPerson(CompanyCode, custcode, "TRUE");
            }

            _ddl_contact_person.initialDataAutoComplete(dt, "BOBJECTLINK", "NAME1", false, "email", "phone", "remark");
            GetcontactDetailForScreen();
            //updContactCus.Update();
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

        private string getCountMessageChatter(string p_doctype, string p_docnumber, string p_fiscalyear)
        {
            return "0";

            //Zaan Comment out
            //string mess = "";
            //DataTable dt = chatService.getConfChatterGroup(SID, ServiceCallFastEntryCriteria.CHAT_BUSINESSOBJ, p_doctype, p_docnumber, p_fiscalyear);
            //if (dt.Rows.Count > 0)
            //{
            //    string groupcode = dt.Rows[0]["group_code"].ToString();
            //    DataTable dtm = AfterSaleService.getInstance().countMessageChatter(groupcode);
            //    if (dtm.Rows.Count > 0)
            //        mess = dtm.Rows[0]["sumMessage"].ToString();
            //}
            //return mess;
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


        private void getdataToedit(string doctype, string docnumber, string fiscalyear)
        {
            string customer = SCT_created_cust_code;//CustomerSelect.SelectedValue;
            string idGen = redirectViewToTicketDetail(customer, doctype, docnumber, fiscalyear);
            if (!String.IsNullOrEmpty(idGen))
            {
                ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/ServiceCallTransactionChange.aspx?id=" + idGen) + "');");
                //Response.-Redirect("/crm/AfterSale/ServiceCallTransactionChange.aspx?id=" + idGen, false);
            }
        }

        //ใช้หน้าอื่นด้วย[KnowledgeManagementDetail, CustomerProfileDetail, Default, ApprovalListControl.ascx]
        public string redirectViewToTicketDetail(string customerCode, string doctype, string docnumber, string fiscalyear)
        {
            string idGen = "";
            Object[] objParam = new Object[] { "1500117",
                    (string)Session[ApplicationSession.USER_SESSION_ID],
                    CompanyCode,doctype,docnumber,fiscalyear};

            DataSet[] objDataSet = new DataSet[] { new tmpServiceCallDataSet() };
            DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
            if (objReturn != null)
            {
                idGen = Guid.NewGuid().ToString().Substring(0, 8);
                tmpServiceCallDataSet serviceTempCallEntity = new tmpServiceCallDataSet();
                serviceTempCallEntity.Merge(objReturn.Copy());
                Session["ServicecallEntity" + idGen] = serviceTempCallEntity;
                Session["SCT_created_cust_code" + idGen] = customerCode;//Customer
                Session["SC_MODE" + idGen] = ApplicationSession.DISPLAY_MODE_STRING;
            }
            return idGen;
        }

        private void CreateTicket()
        {
            string idGen = Guid.NewGuid().ToString().Substring(0, 8);

            Session["SCT_created_doctype_code" + idGen] = _ddl_sctype.SelectedValue;
            Session["SCT_created_doctype_desc" + idGen] = _ddl_sctype.SelectedItem.Text;
            Session["SCT_created_cust_code" + idGen] = CustomerSelect.SelectedValue;//Customer
            Session["SCT_created_cust_desc" + idGen] = CustomerSelect.SelectedText;
            Session["SCT_created_contact_code" + idGen] = _ddl_contact_person.SelectValue;
            Session["SCT_created_contact_desc" + idGen] = _ddl_contact_person.SelectText;
            Session["SCT_created_fiscalyear" + idGen] = _txt_year.Value;
            Session["SCT_created_remark" + idGen] = null;
            Session["SCT_created_equipment" + idGen] = equipmentSelect.SelectedValue;
            if (string.IsNullOrEmpty(hddListCISelect.Value))
            {
                Session["SCT_created_equipment_List" + idGen] = new List<string>();
            }
            else
            {
                List<string> listCI = hddListCISelect.Value.Split(',').ToList();
                listCI = listCI.GroupBy(g => g).Select(s => s.Key).ToList();
                Session["SCT_created_equipment_List" + idGen] = listCI;
            }
            Session["SCT_created_impact" + idGen] = ddlImpact.SelectedValue;
            Session["SCT_created_urgency" + idGen] = ddlUrgency.SelectedValue;
            Session["SCT_created_priority" + idGen] = ddlPriority.SelectedValue;
            //serviceCallEntity = new tmpServiceCallDataSet();
            Session["ServicecallEntity" + idGen] = new tmpServiceCallDataSet();
            //mode_stage = ApplicationSession.CREATE_MODE_STRING;
            Session["SC_MODE" + idGen] = ApplicationSession.CREATE_MODE_STRING;


            dtTempDoc.Clear();
            ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/ServiceCallTransactionChange.aspx?id=" + idGen) + "');");
            //Response.-Redirect("~/crm/AfterSale/ServiceCallTransactionChange.aspx?id=" + idGen);
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";
                if (string.IsNullOrEmpty(_ddl_sctype.SelectedValue))
                    message += "กรุณาระบุ ประเภทใบแจ้งบริการ <br/>";
                //if (string.IsNullOrEmpty(CustomerSelect.SelectedValue))
                //    message += "กรุณาระบุ ลูกค้า <br/>";
                if (string.IsNullOrEmpty(_txt_year.Value))
                    message += "กรุณาระบุ ปีเอกสาร <br/>";
                if (!string.IsNullOrEmpty(message))
                    throw new Exception(message);

                UniversalService universalService = new UniversalService();

                string equipmentCode = equipmentSelect.SelectedValue;
                string customerCode = CustomerSelect.SelectedValue;
                //DataTable dt = universalService.GetEquipmentCustomerAssignment(SID, CompanyCode, equipmentCode, customerCode);
                //if (dt.Rows.Count <= 0)
                //{
                //    throw new Exception("The Configuration Item= " + equipmentCode + " and customer= " + customerCode + " not accord.");
                //}

                bool hasTicket = universalService.CheckOpenTicket(SID, CompanyCode,
                    _ddl_sctype.SelectedValue, CustomerSelect.SelectedValue, equipmentSelect.SelectedValue);

                universalService.CheckDoctypeCreateTicket(
                    SID, CompanyCode, _ddl_sctype.SelectedValue
                );

                if (hasTicket)
                {
                    ClientService.DoJavascript("warningClick('" + _ddl_sctype.SelectedValue + "', '" + CustomerSelect.SelectedValue + "', '" + equipmentSelect.SelectedValue + "');");
                }
                else
                {
                    CreateTicket();
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
                    getdataToedit(drr[0]["Doctype"].ToString(), drr[0]["CallerID"].ToString(), drr[0]["Fiscalyear"].ToString());
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

        private void GetEquipmentMappingOwner()
        {
            string equipmentCode = equipmentSelect.SelectedValue.Trim();
            string customerCode = CustomerSelect.SelectedValue.Trim();

            if (customerCode == "")
            {
                ClientService.DoJavascript("loadEquipmentWithoutCondition" + equipmentSelect.ClientID + "(); loadCustomerWithoutCondition" + CustomerSelect.ClientID + "();");
            }
            else
            {
                DataTable dt = universalService.GetEquipmentCustomerAssignment(SID, CompanyCode, "", customerCode);

                List<ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource> result = new List<ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource>();

                foreach (DataRow dr in dt.Rows)
                {
                    string equipmentName = lib.PrepareCodeAndDescription(dr["EquipmentCode"].ToString(), dr["EquipmentName"].ToString());

                    result.Add(new ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource
                    {
                        code = dr["EquipmentCode"].ToString(),
                        desc = dr["EquipmentName"].ToString(),
                        display = equipmentName
                    });
                }

                GC.Collect();

                string defaultValue = "";

                if (result.Count == 1)
                {
                    defaultValue = result[0].display;
                }
                else
                {
                    if (equipmentCode != "")
                    {
                        var en = result.Find(x => x.code == equipmentCode);
                        if (en != null)
                        {
                            defaultValue = en.display;
                        }
                    }
                }

                string responseJson = JsonConvert.SerializeObject(result);
                equipmentSelect.SelectedDisplay = defaultValue;
                ClientService.DoJavascript("bindAutoCompleteEquipment" + equipmentSelect.ClientID + "(" + responseJson + ", '" + defaultValue + "',false);");
            }
        }

        protected void btnBindContactCus_Click(object sender, EventArgs e)
        {
            try
            {
                GetEquipmentMappingOwner();
                _ddl_contact_person.SetValue = "";
                getcontact_person();
                if (String.IsNullOrEmpty(_ddl_contact_person.SelectValue))
                {
                    txtContactPhone.Text = "";
                    txtContactEmail.Text = "";
                    txtContactremark.Text = "";
                    udpContactDetail.Update();
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

        protected void btnLoadCustomerEquipment_Click(object sender, EventArgs e)
        {
            try
            {
                string equipmentCode = equipmentSelect.SelectedValue.Trim();
                string customerCode = CustomerSelect.SelectedValue.Trim();

                if (equipmentCode == "")
                {
                    ClientService.DoJavascript("loadEquipmentWithoutCondition" + equipmentSelect.ClientID + "(); loadCustomerWithoutCondition" + CustomerSelect.ClientID + "();");
                }
                else
                {
                    DataTable dt = universalService.GetEquipmentCustomerAssignment(SID, CompanyCode, equipmentCode, "");

                    List<ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource> result = new List<ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource>();

                    foreach (DataRow dr in dt.Rows)
                    {
                        string customerName = lib.PrepareNameAndForiegnName(dr["CustomerName"].ToString(), dr["ForeignName"].ToString());

                        result.Add(new ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource
                        {
                            code = dr["OwnerCode"].ToString(),
                            desc = customerName,
                            display = customerName == "" ? dr["OwnerCode"].ToString() : dr["OwnerCode"] + " : " + customerName
                        });
                    }

                    GC.Collect();

                    string defaultValue = "";

                    if (result.Count == 1)
                    {
                        defaultValue = result[0].display;
                    }
                    else
                    {
                        if (customerCode != "")
                        {
                            var en = result.Find(x => x.code == customerCode);
                            if (en != null)
                            {
                                defaultValue = en.display;
                            }
                        }
                    }

                    string responseJson = JsonConvert.SerializeObject(result);
                    CustomerSelect.SelectedDisplay = defaultValue;
                    ClientService.DoJavascript("bindAutoCompleteCustomer" + CustomerSelect.ClientID + "(" + responseJson + ", '" + defaultValue + "',false);");
                    getcontact_person();
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

        private void GetSeverity()
        {
            string impactCode = ddlImpact.SelectedValue;
            string urgencyCode = ddlUrgency.SelectedValue;


            DataTable dt = dtPriority.Clone();
            DataRow[] drr = dtPriority.Select("ImpactCode='" + impactCode + "' and UrgencyCode='" + urgencyCode + "' ");
            if (drr.Length > 0)
            {
                dt = drr.CopyToDataTable();
            }
            else
            {
                dt = dtPriority.DefaultView.ToTable(true, "PriorityCode", "Description");
            }
            ddlPriority.Items.Clear();
            ddlPriority.DataSource = dt;
            ddlPriority.DataBind();
            ddlPriority.Items.Insert(0, new ListItem("", ""));
            if (drr.Length == 1)
            {
                ddlPriority.SelectedValue = drr[0]["PriorityCode"].ToString();
            }
            else
            {
                ddlPriority.SelectedValue = "";
            }
        }

        protected void ddlSelectBindPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetSeverity();
                udpnProblem.Update();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        protected void btnCreateWithWarning_Click(object sender, EventArgs e)
        {
            try
            {
                CreateTicket();
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

        private void GetcontactDetailForScreen()
        {
            string bobjectid = _ddl_contact_person.SelectValue;
            ERPW.Lib.Master.ContactEntity en = new ERPW.Lib.Master.ContactEntity();

            if (!string.IsNullOrEmpty(bobjectid))
            {
                List<ERPW.Lib.Master.ContactEntity> listen = serviceCustomer.getListContactRefCustomer(
                    SID,
                    CompanyCode,
                    CustomerSelect.SelectedValue,
                    "",
                    bobjectid
                );
                if (listen.Count > 0)
                {
                    en = listen[0];

                    txtContactPhone.Text = en.phone.Trim();
                    txtContactEmail.Text = en.email.Trim();
                    txtContactremark.Text = en.REMARK;
                }
            }
            else
            {
                txtContactPhone.Text = "";
                txtContactEmail.Text = "";
                txtContactremark.Text = "";
            }

            udpContactDetail.Update();
        }

        protected void btnSelectContactBindDetail_Click(object sender, EventArgs e)
        {
            try
            {
                GetcontactDetailForScreen();
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

        #region Search Criteria

        #region Member Data Properties

        List<IncidentAreaEnrity> mListIncidentArea
        {
            get
            {
                if (Session["ServiceCallFast_IncidentAreaEnrity_mListIncidentArea"] == null)
                {
                    Session["ServiceCallFast_IncidentAreaEnrity_mListIncidentArea"] = new List<IncidentAreaEnrity>();
                }
                return (List<IncidentAreaEnrity>)Session["ServiceCallFast_IncidentAreaEnrity_mListIncidentArea"];
            }
            set
            {
                Session["ServiceCallFast_IncidentAreaEnrity_mListIncidentArea"] = value;
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
                    DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, ds);
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

        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //Session["SCT_created_cust_code"] = CustomerSelect.SelectedValue;
                //string fiscalyear = _txt_year_search.Value.Trim();
                //string customercode = AutoCustomerSearch.SelectedValue;
                //string priority = ddlPrioritySearch.SelectedValue;
                //string doctype = _ddl_sctype_search.SelectedValue;
                //string datefrom = string.IsNullOrEmpty(ctrlDateFrom.Text) ? "" : Validation.Convert2DateDB(ctrlDateFrom.Text);
                //string dateto = string.IsNullOrEmpty(ctrlDateTo.Text) ? "" : Validation.Convert2DateDB(ctrlDateTo.Text);
                //string contactname = _ddl_contact_person_search.SelectValue;
                //string status = _ddl_ticket_Doc_Status.SelectedValue; //string status = _ddl_status.SelectedValue;
                //string docStatucActive = "01";//_ddl_document_Doc_Status.SelectedValue;
                //string docnumber = _txt_docnumber.Value;

                SCT_created_cust_code = CustomerSelect.SelectedValue;
                string fiscalyear = ""; //_txt_year_search.Value.Trim();
                string customercode = ""; //AutoCustomerSearch.SelectedValue;
                string priority = ""; //ddlPrioritySearch.SelectedValue;
                string doctype = ""; //_ddl_sctype_search.SelectedValue;
                string datefrom = Validation.Convert2DateDB(Validation.getCurrentServerDate()); //string.IsNullOrEmpty(ctrlDateFrom.Text) ? "" : Validation.Convert2DateDB(ctrlDateFrom.Text);
                string dateto = ""; //string.IsNullOrEmpty(ctrlDateTo.Text) ? "" : Validation.Convert2DateDB(ctrlDateTo.Text);
                string contactname = ""; //_ddl_contact_person_search.SelectValue;
                string status = ""; //_ddl_ticket_Doc_Status.SelectedValue; //string status = _ddl_status.SelectedValue;
                string docStatucActive = "01"; //_ddl_document_Doc_Status.SelectedValue;
                string docnumber = ""; //_txt_docnumber.Value;
                string OwnerGroupCode = "";

                List<string> listDocType = lib.getListTicketType(
                    SID,
                    new List<string> { ServiceLibrary.SERVICE_BUSINESS_OBJECT_CHANGE }
                );

                bool FilterOwner = false;
                bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);

                if (FilterOwner && !Permission.AllPermission)
                {
                    OwnerGroupCode = Permission.OwnerGroupCode;
                }

                dtDataSearch = lib.SearchTicketList(
                    SID,
                    CompanyCode,
                    listDocType,
                    fiscalyear,
                    docnumber,
                    status,
                    docStatucActive,
                    datefrom,
                    dateto,
                    "", //ddlImpactSearch.SelectedValue,
                    "", //ddlUrgencySearch.SelectedValue,
                    priority,
                    "", //AutoEquipmentSearch.SelectedValue, 
                    customercode,
                    "", //txtSearchSubject.Text
                    OwnerGroupCode, //ddlOwnerGroup.SelectedValue, 
                    "", //txtProblemGroup.SelectValue, 
                    "", //txtProblemType.SelectValue, 
                    "", //txtProblemSource.SelectValue, 
                    "", //txtContactSource.SelectValue,
                    "", //_ddl_contact_person_search.SelectText, 
                    "",
                    "",
                    "",
                    ""
                );

                string DocstatusResolve = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE_BUSINESS_CHANGE);
                dtDataSearch.DefaultView.RowFilter = "Docstatus <> '" + DocstatusResolve + "'";
                dtDataSearch = dtDataSearch.DefaultView.ToTable();
                dtDataSearch.DefaultView.RowFilter = "";

                rptSearchSale.DataSource = dtDataSearch;
                rptSearchSale.DataBind();
                udpnItems.Update();

                ClientService.DoJavascript("afterSearch();");
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

        protected void btnOpenModalSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_ddl_sctype_search.Items.Count > 0)
                //{
                //    _ddl_sctype_search.SelectedIndex = 0;

                //}
                //else
                //{
                //    setDefaultSearcCriteria();
                //}

                //try
                //{
                //    _ddl_document_Doc_Status.SelectedValue = "01";
                //}
                //catch (Exception)
                //{
                //}
                //udpDefauleSearch.Update();
                //udpDefauleSearch2.Update();
                //udpDefauleSearch3.Update();

                //ClientService.DoJavascript("showInitiativeModal('modalSearchHelpTicketCriteria');");
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

        //protected void btnSelectContactBindDetail_Search_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        GetcontactDetailForScreen_search();
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

        //protected void btnIncidentAreaFilter_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //setDefaultIncidentArea(hhdModeEventFilter.Value.Trim());
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

        //protected void BindDefautlPrioritySearch_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //string impactCode = ddlImpactSearch.SelectedValue;
        //        //string urgencyCode = ddlUrgencySearch.SelectedValue;

        //        //DataRow[] drr = dtPriority.Select("ImpactCode='" + impactCode + "' and UrgencyCode='" + urgencyCode + "' ");
        //        //if (drr.Length > 0)
        //        //{
        //        //    ddlPrioritySearch.SelectedValue = drr[0]["PriorityCode"].ToString();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientService.AGError(ex.Message);
        //    }
        //}



        #region Load data properties
        //private void setDefaultSearcCriteria()
        //{
        //    _ddl_sctype_search.Items.Clear();
        //    dtSCType = AfterSaleService.getInstance().getSearchDoctype("", CompanyCode, true, true);
        //    _ddl_sctype_search.AppendDataBoundItems = true;
        //    _ddl_sctype_search.DataTextField = "Description";
        //    _ddl_sctype_search.DataValueField = "DocumentTypeCode";
        //    _ddl_sctype_search.DataSource = dtSCType;
        //    _ddl_sctype_search.DataBind();
        //    _ddl_sctype_search.Items.Insert(0, new ListItem("", ""));

        //    GetTicketDocStatus();

        //    DataTable dt = lib.GetImpactMaster(SID);
        //    ddlImpactSearch.DataTextField = "ImpactName";
        //    ddlImpactSearch.DataValueField = "ImpactCode";
        //    ddlImpactSearch.DataSource = dt;
        //    ddlImpactSearch.DataBind();
        //    ddlImpactSearch.Items.Insert(0, new ListItem("", ""));


        //    dt = lib.GetUrgencyMaster(SID);
        //    ddlUrgencySearch.DataTextField = "UrgencyName";
        //    ddlUrgencySearch.DataValueField = "UrgencyCode";
        //    ddlUrgencySearch.DataSource = dt;
        //    ddlUrgencySearch.DataBind();
        //    ddlUrgencySearch.Items.Insert(0, new ListItem("", ""));

        //    DataTable dtTemp = dtPriority.Clone();
        //    if (dtPriority.Rows.Count > 0)
        //    {
        //        dtTemp = dtPriority.DefaultView.ToTable(true, "PriorityCode", "Description");
        //    }
        //    ddlPrioritySearch.DataSource = dtTemp;
        //    ddlPrioritySearch.DataBind();
        //    ddlPrioritySearch.Items.Insert(0, new ListItem("", ""));


        //    DataTable dtOwnerGroup = config.GetMasterConfigOwnerGroup(SID, CompanyCode, "");
        //    ddlOwnerGroup.DataSource = dtOwnerGroup;
        //    ddlOwnerGroup.DataBind();
        //    ddlOwnerGroup.Items.Insert(0, new ListItem("", ""));

        //    dt = config.getIncidentAreaRawData(SID);
        //    mListIncidentArea = JsonConvert.DeserializeObject<List<IncidentAreaEnrity>>(JsonConvert.SerializeObject(dt));
        //    dt.Clear();
        //    setDefaultIncidentArea();
        //}
        //private void setDefaultIncidentArea(string Event = "")
        //{

        //    bool isGroup = true; bool isType = true; bool isSource = true; bool isArea = true;
        //    string sGroup = txtProblemGroup.SelectValue;
        //    string sType = txtProblemType.SelectValue;
        //    string sSource = txtProblemSource.SelectValue;
        //    List<IncidentAreaEnrity> listTemp = JsonConvert.DeserializeObject<List<IncidentAreaEnrity>>(JsonConvert.SerializeObject(mListIncidentArea));

        //    #region Swich mode Fillter
        //    if (("INCIDENT_GROUP").Equals(Event))
        //    {
        //        isGroup = false;
        //    }
        //    else if (("INCIDENT_TYPE").Equals(Event))
        //    {
        //        isGroup = false;
        //        isType = false;
        //    }
        //    else if (("INCIDENT_SOURCE").Equals(Event))
        //    {
        //        isGroup = false;
        //        isType = false;
        //        isSource = false;
        //    }
        //    #endregion

        //    #region Where Condation
        //    if (!string.IsNullOrEmpty(sGroup) && !isGroup)
        //    {
        //        listTemp = listTemp.FindAll(x => x.GROUPCODE == sGroup);
        //    }
        //    if (!string.IsNullOrEmpty(sType) && !isType)
        //    {
        //        listTemp = listTemp.FindAll(x => x.GTCODE == sType);
        //    }
        //    if (!string.IsNullOrEmpty(sSource) && !isSource)
        //    {
        //        listTemp = listTemp.FindAll(x => x.GTSCODE == sSource);
        //    }
        //    #endregion


        //    #region Perpare Data to Screen
        //    DataTable dtGroup = new DataTable();
        //    DataTable dtType = new DataTable();
        //    DataTable dtSource = new DataTable();
        //    DataTable dtContact = new DataTable();
        //    if (listTemp.Count > 0)
        //    {
        //        if (isGroup)
        //        {
        //            var group = listTemp.Select(x => new { x.GROUPCODE, x.GROUPNAME }).Distinct().ToList().FindAll(y => !string.IsNullOrEmpty(y.GROUPNAME));
        //            dtGroup = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(group));
        //            group = null;
        //        }
        //        if (isType)
        //        {
        //            var listtype = listTemp.Select(x => new { x.GTCODE, x.TYPENAME }).Distinct().ToList().FindAll(y => !string.IsNullOrEmpty(y.TYPENAME));
        //            dtType = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(listtype));
        //            listtype = null;
        //        }
        //        if (isSource)
        //        {
        //            var listSource = listTemp.Select(x => new { x.GTSCODE, x.SOURCENAME }).Distinct().ToList().FindAll(y => !string.IsNullOrEmpty(y.SOURCENAME));
        //            dtSource = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(listSource));
        //            listSource = null;
        //        }

        //        if (isArea)
        //        {
        //            var listContact = listTemp.Select(x => new { x.AREACODE, x.AREANAME }).Distinct().ToList().FindAll(y => !string.IsNullOrEmpty(y.AREANAME));
        //            dtContact = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(listContact));
        //            listContact = null;
        //        }
        //        listTemp = null;
        //    }
        //    if (isGroup)
        //    {
        //        txtProblemGroup.initialDataAutoComplete(dtGroup, "GROUPCODE", "GROUPNAME", true);
        //    }
        //    if (isType)
        //    {
        //        txtProblemType.initialDataAutoComplete(dtType, "GTCODE", "TYPENAME", true);
        //    }
        //    if (isSource)
        //    {
        //        txtProblemSource.initialDataAutoComplete(dtSource, "GTSCODE", "SOURCENAME", true);
        //    }
        //    if (isArea)
        //    {
        //        txtContactSource.initialDataAutoComplete(dtContact, "AREACODE", "AREANAME", true);
        //    }
        //    #endregion
        //}
        //private void setDefaulsearchPageLoad()
        //{
        //    _ddl_contact_person_search.initialDataAutoComplete(new DataTable(), "", "");
        //    txtProblemGroup.initialDataAutoComplete(new DataTable(), "", "");
        //    txtProblemType.initialDataAutoComplete(new DataTable(), "", "");
        //    txtProblemSource.initialDataAutoComplete(new DataTable(), "", "");
        //    txtContactSource.initialDataAutoComplete(new DataTable(), "", "");
        //}
        //private void getcontact_person_search()
        //{
        //    string custcode = AutoCustomerSearch.SelectedValue.Trim();

        //    DataTable dt = new DataTable();

        //    if (custcode == "")
        //    {
        //        dt.Columns.Add("BOBJECTLINK");
        //        dt.Columns.Add("NAME1");
        //        dt.Columns.Add("email");
        //        dt.Columns.Add("phone");
        //        dt.Columns.Add("remark");
        //    }
        //    else
        //    {
        //        dt = AfterSaleService.getInstance().getContactPerson(CompanyCode, custcode);
        //    }

        //    _ddl_contact_person_search.initialDataAutoComplete(dt, "BOBJECTLINK", "NAME1", false, "email", "phone", "remark");
        //    GetcontactDetailForScreen_search();
        //    //updContactCus.Update();
        //}
        //private void GetcontactDetailForScreen_search()
        //{
        //    string bobjectid = _ddl_contact_person_search.SelectValue;
        //    ERPW.Lib.Master.ContactEntity en = new ERPW.Lib.Master.ContactEntity();

        //    if (!string.IsNullOrEmpty(bobjectid))
        //    {
        //        List<ERPW.Lib.Master.ContactEntity> listen = serviceCustomer.getListContactRefCustomer(
        //            SID,
        //            CompanyCode,
        //            AutoCustomerSearch.SelectedValue,
        //            "",
        //            bobjectid
        //        );
        //        if (listen.Count > 0)
        //        {
        //            en = listen[0];

        //            txtContactPhone_search.Text = en.phone.Trim();
        //            txtContactEmail_search.Text = en.email.Trim();
        //            txtContactremark_search.Text = en.REMARK;
        //        }
        //    }
        //    else
        //    {
        //        txtContactPhone_search.Text = "";
        //        txtContactEmail_search.Text = "";
        //        txtContactremark_search.Text = "";
        //    }

        //    udpContactDetailSearch.Update();
        //}

        #endregion

        #region Select Customer OR Equipment for Search

        //protected void btnBindContactForSearch_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //GetEquipmentMappingOwner_search();
        //        //getcontact_person_search();
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

        //protected void btnBindMappingCustomerForSearch_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        GetOwnerMappingEquipment_search();
        //        getcontact_person_search();
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

        //private void GetEquipmentMappingOwner_search()
        //{
        //    string equipmentCode = AutoEquipmentSearch.SelectedValue.Trim();
        //    string customerCode = AutoCustomerSearch.SelectedValue.Trim();

        //    if (customerCode == "")
        //    {
        //        ClientService.DoJavascript("loadEquipmentWithoutCondition" + AutoEquipmentSearch.ClientID + "(); loadCustomerWithoutCondition" + AutoCustomerSearch.ClientID + "();");
        //    }
        //    else
        //    {
        //        DataTable dt = universalService.GetEquipmentCustomerAssignment(SID, CompanyCode, "", customerCode);

        //        List<ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource> result = new List<ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource>();

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            string equipmentName = lib.PrepareCodeAndDescription(dr["EquipmentCode"].ToString(), dr["EquipmentName"].ToString());

        //            result.Add(new ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource
        //            {
        //                code = dr["EquipmentCode"].ToString(),
        //                desc = dr["EquipmentName"].ToString(),
        //                display = equipmentName
        //            });
        //        }

        //        GC.Collect();

        //        string defaultValue = "";

        //        if (result.Count == 1)
        //        {
        //            defaultValue = result[0].display;
        //        }
        //        else
        //        {
        //            if (equipmentCode != "")
        //            {
        //                var en = result.Find(x => x.code == equipmentCode);
        //                if (en != null)
        //                {
        //                    defaultValue = en.display;
        //                }
        //            }
        //        }

        //        string responseJson = JsonConvert.SerializeObject(result);
        //        AutoEquipmentSearch.SelectedDisplay = defaultValue;
        //        ClientService.DoJavascript("bindAutoCompleteEquipment" + AutoEquipmentSearch.ClientID + "(" + responseJson + ", '" + defaultValue + "',false);");
        //    }
        //}

        //private void GetOwnerMappingEquipment_search()
        //{
        //    string equipmentCode = AutoEquipmentSearch.SelectedValue.Trim();
        //    string customerCode = AutoCustomerSearch.SelectedValue.Trim();

        //    if (equipmentCode == "")
        //    {
        //        ClientService.DoJavascript("loadEquipmentWithoutCondition" + AutoEquipmentSearch.ClientID + "(); loadCustomerWithoutCondition" + AutoCustomerSearch.ClientID + "();");
        //    }
        //    else
        //    {
        //        DataTable dt = universalService.GetEquipmentCustomerAssignment(SID, CompanyCode, equipmentCode, "");

        //        List<ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource> result = new List<ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource>();

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            string customerName = lib.PrepareNameAndForiegnName(dr["CustomerName"].ToString(), dr["ForeignName"].ToString());

        //            result.Add(new ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource
        //            {
        //                code = dr["OwnerCode"].ToString(),
        //                desc = customerName,
        //                display = customerName == "" ? dr["OwnerCode"].ToString() : dr["OwnerCode"] + " : " + customerName
        //            });
        //        }

        //        GC.Collect();

        //        string defaultValue = "";

        //        if (result.Count == 1)
        //        {
        //            defaultValue = result[0].display;
        //        }
        //        else
        //        {
        //            if (customerCode != "")
        //            {
        //                var en = result.Find(x => x.code == customerCode);
        //                if (en != null)
        //                {
        //                    defaultValue = en.display;
        //                }
        //            }
        //        }

        //        string responseJson = JsonConvert.SerializeObject(result);
        //        AutoCustomerSearch.SelectedDisplay = defaultValue;
        //        ClientService.DoJavascript("bindAutoCompleteCustomer" + AutoCustomerSearch.ClientID + "(" + responseJson + ", '" + defaultValue + "',false);");
        //    }

        //}
        #endregion

        #region search Configuration Item

        protected void btnOpenModalSelectConfigurationItem_Click(object sender, EventArgs e)
        {
            try
            {
                bindDataCriteriaEquipmentSearch();
                ClientService.DoJavascript("showInitiativeModal('modalSearchHelpConfigurationItem');");
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

        protected void btnSearchDataConfigurationItem_Click(object sender, EventArgs e)
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

        protected void btnSelectConfigurationItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (hhdModeSearch.Value.Equals("SEARCH"))
                {
                    //AutoEquipmentSearch.SelectedValue = hddConfigurationItemCode.Value;
                    //GetOwnerMappingEquipment_search();
                }
                else
                {
                    equipmentSelect.SelectedValue = hddConfigurationItemCode.Value;
                    btnLoadCustomerEquipment_Click(sender, e);
                }
                ClientService.DoJavascript("closeInitiativeModal('modalSearchHelpConfigurationItem');");
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

        private void bindDataEquipment()
        {
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
                s.OwnerGroupName
            });
            JArray data = JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(dataSource));
            JArray datastatus = JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(dtEquipmentStatus));
            udpSearchConfigurationItem.Update();
            ClientService.DoJavascript("afterSearchBindEquipmentCriteria(" + data + "," + datastatus + ");");
        }


        private void bindDataCriteriaEquipmentSearch()
        {
            txtEquipmentCode.Text = "";
            txtEquipmentName.Text = "";
            ddlEquipmentStatus.SelectedIndex = 0;

            if (ddlEquipmentType.Items.Count > 1)
            {
                ddlEquipmentType.SelectedIndex = 0;
                ddlSearch_EMClass.SelectedIndex = 0;
                ddlSearch_Category.SelectedIndex = 0;

            }
            else
            {
                ddlEquipmentType.DataTextField = "Description";
                ddlEquipmentType.DataValueField = "MaterialGroupCode";
                ddlEquipmentType.DataSource = universalService.getEquipmentType(SID, CompanyCode);
                ddlEquipmentType.DataBind();
                ddlEquipmentType.Items.Insert(0, new ListItem("All", ""));

                ddlSearch_EMClass.DataTextField = "ClassName";
                ddlSearch_EMClass.DataValueField = "ClassCode";
                ddlSearch_EMClass.DataSource = ServiceEquipment.getEMClass(SID);
                ddlSearch_EMClass.DataBind();
                ddlSearch_EMClass.Items.Insert(0, new ListItem("All", ""));

                DataTable dt = dtEquipmentStatus;
                ddlEquipmentStatus.DataTextField = "StatusName";
                ddlEquipmentStatus.DataValueField = "StatusCode";
                ddlEquipmentStatus.DataSource = dt;
                ddlEquipmentStatus.DataBind();
                ddlEquipmentStatus.Items.Insert(0, new ListItem("All", ""));

                ddlOwnerService.DataTextField = "OwnerGroupName";
                ddlOwnerService.DataValueField = "OwnerGroupCode";
                ddlOwnerService.DataSource = config.GetMasterConfigOwnerGroup(SID, CompanyCode, "");
                ddlOwnerService.DataBind();
                ddlOwnerService.Items.Insert(0, new ListItem("All", ""));
            }
            udpUpdateConfigurationCriteria.Update();

            JArray data = new JArray();
            udpSearchConfigurationItem.Update();
            ClientService.DoJavascript("afterSearchBindEquipmentCriteria(" + data + ");");
        }

        #endregion

        #region Search Customer

        protected void btnOpenModalSelectCustomerCriteria_Click(object sender, EventArgs e)
        {
            try
            {
                bindDataCriteriaCustomerSearch();
                ClientService.DoJavascript("showInitiativeModal('modalSearchHelpCustomerDetail');");
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

        protected void btnSearchCustomerCriteria_Click(object sender, EventArgs e)
        {
            try
            {
                bindDataCustomer();
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

        protected void btnSelectCustomerCriteria_Click(object sender, EventArgs e)
        {
            try
            {
                if (hhdModeSearch.Value.Equals("SEARCH"))
                {
                    //GetEquipmentMappingOwner_search();
                    //AutoCustomerSearch.SelectedValue = hddCustomerCodeSelected.Value;
                    //_ddl_contact_person_search.SetValue = "";
                    //getcontact_person_search();
                }
                else
                {
                    CustomerSelect.SelectedValue = hddCustomerCodeSelected.Value;
                    btnBindContactCus_Click(sender, e);
                }
                ClientService.DoJavascript("closeInitiativeModal('modalSearchHelpCustomerDetail');");
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

        private void bindDataCustomer()
        {
            ContactEntity contact = new ContactEntity();
            contact.NAME1 = txtContactNameSearchCustomer.Text;
            contact.NickName = txtContactNickNameSearchCustomer.Text;
            contact.POSITION = txtContactPOSITIONSearchCustomer.Text;
            contact.REMARK = txtContactRemarkSearchCustomer.Text;
            contact.email = txtContactEmailSearchCustomer.Text;
            contact.phone = txtContactPhoneSearchCustomer.Text;
            contact.AUTH_CONTACT = ddlContactAuthorization.SelectedValue;

            var ListCustomer = serviceCustomer.SearchCustomerAllData(
               SID,
               CompanyCode,
               txtFirstname.Text,
               txtFirstname.Text,
               ddlCustomerGroup.SelectedValue,
               ddlSaleDistrict.SelectedValue,
               txtAddress.Text,
               txtPhone.Text,
               txtPhoneMobile.Text,
               txtEmail.Text,
               "",
               "",
               contact
           );

            var dataSource = ListCustomer.Select(s => new
            {
                s.CustomerCode,
                s.FullName,
                s.CustomerGroup,
                s.CustomerGroupDesc,
                s.Address,
                s.TelNo1,
                s.EMail
            });

            JArray data = JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(dataSource));
            udpSearchCustomerCriteria.Update();
            ClientService.DoJavascript("afterSearchBindCustomerCriteria(" + data + ");");
        }
        private void bindDataCriteriaCustomerSearch()
        {
            txtFirstname.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";

            txtContactNameSearchCustomer.Text = "";
            txtContactNickNameSearchCustomer.Text = "";
            txtContactPhoneSearchCustomer.Text = "";
            txtContactEmailSearchCustomer.Text = "";
            txtContactPOSITIONSearchCustomer.Text = "";
            txtContactRemarkSearchCustomer.Text = "";

            if (ddlCustomerGroup.Items.Count > 1)
            {
                ddlCustomerGroup.SelectedIndex = 0;
                ddlSaleDistrict.SelectedIndex = 0;
                ddlContactAuthorization.SelectedIndex = 0;
            }
            else
            {
                DataTable dt = serviceCustomer.getCustomerDocType(SID, CompanyCode);
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Description"] = ObjectUtil.PrepareCodeAndDescription(
                        dr["CustomerGroupCode"].ToString(),
                        dr["Description"].ToString()
                    );
                }
                ddlCustomerGroup.DataValueField = "CustomerGroupCode";
                ddlCustomerGroup.DataTextField = "Description";
                ddlCustomerGroup.DataSource = dt;
                ddlCustomerGroup.DataBind();
                ddlCustomerGroup.Items.Insert(0, new ListItem("All", ""));

                ddlSaleDistrict.DataValueField = "SAREACODE";
                ddlSaleDistrict.DataTextField = "CodeAndDesc";
                ddlSaleDistrict.DataSource = serviceCustomer.getSaleArea(SID, "");
                ddlSaleDistrict.DataBind();
                ddlSaleDistrict.Items.Insert(0, new ListItem("All", ""));

                ddlContactAuthorization.DataTextField = "Name";
                ddlContactAuthorization.DataValueField = "Code";
                ddlContactAuthorization.DataSource = config.GetMasterConfigAuthorizationContact(SID);
                ddlContactAuthorization.DataBind();
                ddlContactAuthorization.Items.Insert(0, new ListItem("All", ""));
            }
            udpUpdateCustomerCriteria.Update();
            JArray data = new JArray();
            udpSearchCustomerCriteria.Update();
            ClientService.DoJavascript("afterSearchBindCustomerCriteria(" + data + ");");
        }
        #endregion


        #endregion

    }
}