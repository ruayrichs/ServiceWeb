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
using ERPW.Lib.Service.Report;
using ERPW.Lib.Service.Report.Entity;
using ERPW.Lib.Authentication.Entity;

namespace ServiceWeb.crm.AfterSale
{
    public partial class ServiceCallFastEntryCriteria : AbstractsSANWebpage //System.Web.UI.Page
    {
        //public string _SID;
        //public string SID
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_SID))
        //        {
        //            _SID = ERPWAuthentication.SID;
        //        }
        //        return _SID;
        //    }
        //}

        //public string _CompanyCode;
        //public string CompanyCode
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_CompanyCode))
        //        {
        //            _CompanyCode = ERPWAuthentication.CompanyCode;
        //        }
        //        return _CompanyCode;
        //    }
        //}

        //public string _UserName;
        //public string UserName
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_UserName))
        //        {
        //            _UserName = ERPWAuthentication.UserName;
        //        }
        //        return _UserName;
        //    }
        //}

        //public string _EmployeeCode;
        //public string EmployeeCode
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_EmployeeCode))
        //        {
        //            _EmployeeCode = ERPWAuthentication.EmployeeCode;
        //        }
        //        return _EmployeeCode;
        //    }
        //}

        //public string _FullNameEN;
        //public string FullNameEN
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_FullNameEN))
        //        {
        //            _FullNameEN = ERPWAuthentication.FullNameEN;
        //        }
        //        return _FullNameEN;
        //    }
        //}

        //public string _FullNameTH;
        //public string FullNameTH
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_FullNameTH))
        //        {
        //            _FullNameTH = ERPWAuthentication.FullNameTH;
        //        }
        //        return _FullNameTH;
        //    }
        //}

        //public AuthenticationPermission _Permission;
        //public AuthenticationPermission Permission
        //{
        //    get
        //    {
        //        if (_Permission == null)
        //        {
        //            _Permission = ERPWAuthentication.Permission;
        //        }
        //        return _Permission;
        //    }
        //}

        private string _TK_BusinessObject;
        private string TK_BusinessObject
        {
            get
            {
                if (_TK_BusinessObject == null)
                {
                    _TK_BusinessObject = (string)Session["TK_BusinessObject"];
                }
                return _TK_BusinessObject;
            }
        }

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

        protected override Boolean ProgramPermission_CanView()
        {
            if (string.IsNullOrEmpty(TK_BusinessObject))
            { ClientService.OpenSessionTimedOutFade(); return false; }
            else if (TK_BusinessObject == ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_INCIDENT)
            { return Permission.IncidentView || Permission.AllPermission; }
            else if (TK_BusinessObject == ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_REQUEST)
            { return Permission.RequestView || Permission.AllPermission; }
            else if (TK_BusinessObject == ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_PROBLEM)
            { return Permission.ProblemView || Permission.AllPermission; }
            else
            { return false; }
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            if (string.IsNullOrEmpty(TK_BusinessObject))
                return false;
            else if (TK_BusinessObject == ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_INCIDENT)
                return Permission.IncidentModify || Permission.AllPermission;
            else if (TK_BusinessObject == ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_REQUEST)
                return Permission.RequestModify || Permission.AllPermission;
            else if (TK_BusinessObject == ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_PROBLEM)
                return Permission.ProblemModify || Permission.AllPermission;
            else
                return false;
        }

        protected override string getProgramID()
        {
            return null;
        }


        private ServiceTicketLibrary lib = new ServiceTicketLibrary();
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        private UniversalService universalService = new UniversalService();
        private MasterConfigLibrary config = MasterConfigLibrary.GetInstance();
        private EquipmentService ServiceEquipment = new EquipmentService();
        private ERPW.Lib.Master.CustomerService serviceCustomer = ERPW.Lib.Master.CustomerService.getInstance();
        public string workGroupCode = "20170121162748444411";
        public static string CHAT_BUSINESSOBJ = "SC";
        private OwnerService ownerService = new OwnerService();

        #region DT Session Member Data

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
            //set { Session["dtTempAttachfile"] = value; }
        }

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

        private DataTable _dtDataSearch;
        private DataTable dtDataSearch
        {
            get
            {
                if (_dtDataSearch == null)
                {
                    _dtDataSearch = (DataTable)Session["ServiceCallCriteria.SCFC_dtDataSearch"];
                }
                return _dtDataSearch; 
            }
            set { Session["ServiceCallCriteria.SCFC_dtDataSearch"] = value; }
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

        private DataTable _dtSCType;
        private DataTable dtSCType
        {
            get
            {
                if (_dtSCType == null)
                {
                    _dtSCType = AfterSaleService.getInstance().getSearchDoctype(
                        "",
                        CompanyCode,
                        TK_BusinessObject
                    );
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool FilterOwner = false;
                bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);

                if (FilterOwner)
                {
                    equipmentSelect.CssClass = "form-control form-control-sm required";
                }

                loadinitData();
                GetddlSctype();

                GetImpact();
                GetUrgency();
                GetPriority();


                _txt_year.Value = Validation.getCurrentServerDateTime().Year.ToString();
                string FnameLname = CustomerSelectNew.Text = ServiceLibrary.LookUpTable(
                    "FirstName+' '+LastName AS FnameLname",
                    "master_employee",
                    "where SID = '" + SID + "' AND CompanyCode = '" + CompanyCode + "' AND EmployeeCode = '" + EmployeeCode + "'"
                );
                CustomerSelectNew.Text = ServiceLibrary.LookUpTable(
                    "CustomerCode",
                    "master_customer",
                    "where SID = '" + SID + "' AND CompanyCode = '" + CompanyCode + "' AND CustomerName = '" + FnameLname + "'"
                );

                Session["SCT_created_equipment"] = null;
                _ddl_contact_person.initialDataAutoComplete(new DataTable(), "", "", false);

                btnSearch_Click(null, null);
                
                ClientService.DoJavascript("UserLV("+ (Permission.LevelControl) +");");
            }
        }

        #region Bind Data To Screen
        private void loadinitData()
        {
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

            try
            {
                if (TK_BusinessObject == null)
                {
                    Response.Redirect(Page.ResolveUrl("~/"));
                }
                else
                {
                    _ddl_sctype.Items.Clear();

                    if (dtSCType.Rows.Count > 1)
                    {
                        _ddl_sctype.Items.Add(new ListItem("", ""));
                    }

                    _ddl_sctype.AppendDataBoundItems = true;
                    _ddl_sctype.DataTextField = "Description";
                    _ddl_sctype.DataValueField = "DocumentTypeCode";
                    _ddl_sctype.DataSource = dtSCType;
                    _ddl_sctype.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Redirect(Page.ResolveUrl("~/"));
            }

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

        private void GetImpact(List<string> lstImpact)
        {
            DataTable dt = lib.GetImpactMaster(SID);

            dt = (from DataRow dr in dt.Rows
                  where lstImpact.Contains(dr["ImpactCode"].ToString())
                  select dr).CopyToDataTable();

            ddlImpact.DataTextField = "ImpactName";
            ddlImpact.DataValueField = "ImpactCode";
            ddlImpact.DataSource = dt;
            ddlImpact.DataBind();
            ddlImpact.Items.Insert(0, new ListItem("", ""));
        }

        private void GetUrgency(List<string> lstUrgency)
        {
            DataTable dt = lib.GetUrgencyMaster(SID);

            dt = (from DataRow dr in dt.Rows
                  where lstUrgency.Contains(dr["UrgencyCode"].ToString())
                  select dr).CopyToDataTable();

            ddlUrgency.DataTextField = "UrgencyName";
            ddlUrgency.DataValueField = "UrgencyCode";
            ddlUrgency.DataSource = dt;
            ddlUrgency.DataBind();
            ddlUrgency.Items.Insert(0, new ListItem("", ""));
        }

        private void GetPriority(List<string> lstPriority)
        {
            DataTable dt = dtPriority.Clone();

            if (dtPriority.Rows.Count > 0)
            {
                dt = dtPriority.DefaultView.ToTable(true, "PriorityCode", "Description");
            }

            dt = (from DataRow dr in dt.Rows
                  where lstPriority.Contains(dr["PriorityCode"].ToString())
                  select dr).CopyToDataTable();

            ddlPriority.DataSource = dt;
            ddlPriority.DataBind();
            ddlPriority.Items.Insert(0, new ListItem("", ""));

            if (dtPriority.Rows.Count > 0)
            {
                ddlPriority.SelectedIndex = 0;
            }

        }

        #endregion


        private void getcontact_person()
        {
            //if (CustomerSelect.SelectedValue == "")
            //{
            //    CustomerSelect.SelectedValue = CustomerSelectNew.Text;
            //}

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
                ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/ServiceCallTransaction.aspx?id=" + idGen) + "');");
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

        private void CreateTicketNext()
        {
            try
            {
                string message = "";
                if (string.IsNullOrEmpty(_ddl_sctype.SelectedValue))
                    message += "กรุณาระบุ ประเภทใบแจ้งบริการ <br/>";
                if (string.IsNullOrEmpty(CustomerSelect.SelectedValue))
                    message += "กรุณาระบุ ลูกค้า <br/>";
                if (string.IsNullOrEmpty(_txt_year.Value))
                    message += "กรุณาระบุ ปีเอกสาร <br/>";
                if (!string.IsNullOrEmpty(message))
                    throw new Exception(message);

                UniversalService universalService = new UniversalService();

                string equipmentCode = equipmentSelect.SelectedValue;
                string customerCode = CustomerSelect.SelectedValue;

                bool hasTicket = universalService.CheckOpenTicket(SID, CompanyCode,
                    _ddl_sctype.SelectedValue, CustomerSelect.SelectedValue, equipmentSelect.SelectedValue
                );

                universalService.CheckDoctypeCreateTicket(
                    SID, CompanyCode, _ddl_sctype.SelectedValue
                );

                if (hasTicket)
                {
                    //ClientService.DoJavascript("warningClick('" + _ddl_sctype.SelectedValue + "', '" + CustomerSelect.SelectedValue + "', '" + equipmentSelect.SelectedValue + "');");
                    CreateTicket();
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

        private void CreateTicket()
        {
            if (CustomerSelect.SelectedValue == "")
            {
                CustomerSelect.SelectedValue = CustomerSelectNew.Text;
            }
            //if (equipmentSelect.SelectedValue != "")
            //{
            //    equipmentSelect.SelectedValue = "";
            //}
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
            Session["SCT_created_impact" + idGen] = ddlImpact.SelectedValue;
            Session["SCT_created_urgency" + idGen] = ddlUrgency.SelectedValue;
            Session["SCT_created_priority" + idGen] = ddlPriority.SelectedValue;
            Session["ServicecallEntity" + idGen] = new tmpServiceCallDataSet();
            Session["SC_MODE" + idGen] = ApplicationSession.CREATE_MODE_STRING;
            Session["SCT_created_subject" + idGen] = txt_problem_topic.Text;
            Session["SCT_created_description" + idGen] = tbEquipmentRemark.Text;


            dtTempDoc.Clear();
            ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/ServiceCallTransaction.aspx?id=" + idGen) + "');");
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            CreateTicketNext();
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
            //if (CustomerSelect.SelectedValue == "")
            //{
            //    CustomerSelect.SelectedValue = CustomerSelectNew.Text;
            //}

            string equipmentCode = equipmentSelect.SelectedValue.Trim();
            string customerCode = CustomerSelect.SelectedValue.Trim();

            if (customerCode == "")
            {
                ClientService.DoJavascript("loadEquipmentWithoutCondition" + equipmentSelect.ClientID + "(); loadCustomerWithoutCondition" + CustomerSelect.ClientID + "();");
            }
            else
            {
                DataTable dt = universalService.GetEquipmentCustomerAssignment(SID, CompanyCode, "", customerCode);
                DataRow drr = dt.Rows[0];

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
                    //defaultValue = result[0].display;
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
                if (equipmentSelect.SelectedValue != "")
                {
                    foreach (var iResult in result)
                    {
                        if (equipmentSelect.SelectedValue == iResult.code)
                        {
                            defaultValue = equipmentSelect.SelectedDisplay;
                        }
                    }
                }

                string responseJson = JsonConvert.SerializeObject(result);
                equipmentSelect.SelectedDisplay = defaultValue;
                //if (equipmentSelect.SelectedValue == "") 
                //{
                //    equipmentSelect.SelectedValue = drr["EquipmentCode"].ToString(); 
                //}
                ClientService.DoJavascript("bindAutoCompleteEquipment" + equipmentSelect.ClientID + "(" + responseJson + ", '" + defaultValue + "',false);");
            }
        }

        private bool firstOpen;

        private void btnBindContactCus_Next()
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
                if (equipmentSelect.SelectedValue != "" && CustomerSelect.SelectedValue != "")
                {
                    if (firstOpen)
                    {
                        CreateTicketNext();
                    }
                    firstOpen = false;
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AGLoading(false);
                ClientService.DoJavascript("CloseloadAllPanel();");
            }
        }

        protected void btnBindContactCus_Click(object sender, EventArgs e)
        {
            firstOpen = false;
            btnBindContactCus_Next();
        }

        protected void btnBindContactCusAuto_Click(object sender, EventArgs e)
        {
            firstOpen = true;
            btnBindContactCus_Next();
        }

        protected void btnLoadCustomerEquipment_Click(object sender, EventArgs e)
        {
            try
            {
                string equipmentCode = equipmentSelect.SelectedValue.Trim();
                string customerCode = CustomerSelect.SelectedValue.Trim();
                if (equipmentCode == "")
                {
                    ClientService.DoJavascript("loadEquipmentWithoutCondition" + equipmentSelect.ClientID + "();");
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

                    //if (result.Count == 1)
                    //{
                    //    defaultValue = result[0].display;
                    //}
                    //else
                    //{
                    //    if (customerCode != "")
                    //    {
                    //        var en = result.Find(x => x.code == customerCode);
                    //        if (en != null)
                    //        {
                    //            defaultValue = en.display;
                    //        }
                    //    }
                    //}
                    if (CustomerSelect.SelectedValue != "")
                    {
                        foreach (var iResult in result)
                        {
                            if (CustomerSelect.SelectedValue == iResult.code)
                            {
                                defaultValue = CustomerSelect.SelectedDisplay;
                            }
                        }
                    }

                    string responseJson = JsonConvert.SerializeObject(result);
                    CustomerSelect.SelectedDisplay = defaultValue;
                    ClientService.DoJavascript("bindAutoCompleteCustomer" + CustomerSelect.ClientID + "(" + responseJson + ", '" + defaultValue + "',false);");
                    getcontact_person();
                }

                if (equipmentCode == "" && customerCode == "")
                {
                    ClientService.DoJavascript("loadCustomerWithoutCondition" + CustomerSelect.ClientID + "();");
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
            DataTable tmpdtPriority = dtPriority;
            string equipmentCode = equipmentSelect.SelectedValue.Trim();
            DataTable dtSeverityEquipment = ServiceEquipment.getSeverityConfigByEquipment(SID, CompanyCode, equipmentCode);
            if (dtSeverityEquipment.Rows.Count > 0)
            {
                List<string> lstPriority = (from DataRow dr in dtSeverityEquipment.Rows select dr["PriorityCode"].ToString()).ToList();

                tmpdtPriority = (from DataRow dr in dtPriority.Rows
                      where lstPriority.Contains(dr["PriorityCode"].ToString())
                      select dr)
                      .CopyToDataTable();
            }

            DataRow[] drr = tmpdtPriority.Select("ImpactCode='" + impactCode + "' and UrgencyCode='" + urgencyCode + "' ");
            if (drr.Length > 0)
            {
                dt = drr.CopyToDataTable();
            }
            else
            {
                dt = tmpdtPriority.DefaultView.ToTable(true, "PriorityCode", "Description");
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

            //if (CustomerSelect.SelectedValue == "")
            //{
            //    CustomerSelect.SelectedValue = CustomerSelectNew.Text;
            //}

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
                    new List<string> { TK_BusinessObject }
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

                string DocstatusResolve = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE);

                //dtDataSearch.DefaultView.RowFilter = "Docstatus <> '" + DocstatusResolve + "'";
                //dtDataSearch = dtDataSearch.DefaultView.ToTable();
                //dtDataSearch.DefaultView.RowFilter = "";

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

        protected void btnSelectContactBindDetail_Search_Click(object sender, EventArgs e)
        {
            try
            {
                GetcontactDetailForScreen_search();
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

        protected void btnIncidentAreaFilter_Click(object sender, EventArgs e)
        {
            try
            {
                //setDefaultIncidentArea(hhdModeEventFilter.Value.Trim());
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

        protected void BindDefautlPrioritySearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }



        #region Load data properties
        private void setDefaultSearcCriteria()
        {
            if (TK_BusinessObject == null)
            {
                Response.Redirect(Page.ResolveUrl("~/"));
            }
            else
            {
                
            }
        }
        
        private void getcontact_person_search()
        {
            string custcode = ""; // AutoCustomerSearch.SelectedValue.Trim();

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

            GetcontactDetailForScreen_search();
        }
        private void GetcontactDetailForScreen_search()
        {
            
        }

        #endregion

        #region Select Customer OR Equipment for Search

        protected void btnBindContactForSearch_Click(object sender, EventArgs e)
        {
            try
            {
                getcontact_person_search();
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

        protected void btnBindMappingCustomerForSearch_Click(object sender, EventArgs e)
        {
            try
            {
                getcontact_person_search();
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

                bool FilterOwner = false;
                bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);

                if (FilterOwner && !Permission.AllPermission)
                {
                    ddlOwnerService.Items.Clear();
                    ddlOwnerService.Items.Insert(0,
                        new ListItem(
                            Permission.OwnerGroupName,
                            Permission.OwnerGroupCode
                        )
                    );

                   
                    // #Edit OwnerService Incident
                    DataTable dtDataUserOwnerService = ownerService.getMappingOwner(UserName);//get data ownerService

                    ddlOwnerService.DataTextField = "OwnerGroupName";
                    ddlOwnerService.DataValueField = "OwnerService";
                    ddlOwnerService.DataSource = dtDataUserOwnerService;
                    ddlOwnerService.DataBind();
                    ddlOwnerService.SelectedIndex = 0;

                   

                    if (dtDataUserOwnerService.Rows.Count == 1)
                    {
                        ddlOwnerService.Enabled = false;
                        ddlOwnerService.CssClass = "form-control form-control-sm";


                    }
                 
                }
                else
                {
                    DataTable dtOwnerGroup = config.GetMasterConfigOwnerGroup(SID, CompanyCode, "");
                    ddlOwnerService.DataTextField = "OwnerGroupName";
                    ddlOwnerService.DataValueField = "OwnerGroupCode";
                    ddlOwnerService.DataSource = dtOwnerGroup;
                    ddlOwnerService.DataBind();
                    ddlOwnerService.Items.Insert(0, new ListItem("", ""));

                 
                }
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
                    getcontact_person_search();
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
                ClientService.DoJavascript("CloseloadAllPanel();");
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

            if (!string.IsNullOrEmpty(ddlOwnerService_SearchCustomer.SelectedValue))
            {
                ListCustomer = ListCustomer.Where(w => w.OwnerServiceCode == ddlOwnerService_SearchCustomer.SelectedValue).ToList();
            }

            var dataSource = ListCustomer.Select(s => new
            {
                s.CustomerCode,
                s.FullName,
                s.CustomerGroup,
                s.CustomerGroupDesc,
                s.Address,
                s.TaxID,
                s.TelNo1,
                s.Mobile,
                s.EMail,
                s.Active,
                s.CustomerCritical,
                s.OwnerService,
                s.ResponsibleOrganization
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
            txtPhoneMobile.Text = "";
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

            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);

            if (FilterOwner && !Permission.AllPermission)
            {
         
                ddlOwnerService_SearchCustomer.Items.Clear();
                ddlOwnerService_SearchCustomer.Items.Insert(0,
                    new ListItem(
                        Permission.OwnerGroupName,
                        Permission.OwnerGroupCode
                    )
                );
                // #Edit OwnerService Incident Customer
                DataTable dtDataUserOwnerService = ownerService.getMappingOwner(UserName);//get data ownerService

                ddlOwnerService_SearchCustomer.DataTextField = "OwnerGroupName";
                ddlOwnerService_SearchCustomer.DataValueField = "OwnerService";
                ddlOwnerService_SearchCustomer.DataSource = dtDataUserOwnerService;
                ddlOwnerService_SearchCustomer.DataBind();
                ddlOwnerService_SearchCustomer.SelectedIndex = 0;


                if (dtDataUserOwnerService.Rows.Count == 1)
                {
                    
                    ddlOwnerService_SearchCustomer.Enabled = false;
                    ddlOwnerService_SearchCustomer.CssClass = "form-control form-control-sm";
                }

            }
            else
            {
                DataTable dtOwnerGroup = config.GetMasterConfigOwnerGroup(SID, CompanyCode, "");

                ddlOwnerService_SearchCustomer.DataTextField = "OwnerGroupName";
                ddlOwnerService_SearchCustomer.DataValueField = "OwnerGroupCode";
                ddlOwnerService_SearchCustomer.DataSource = dtOwnerGroup;
                ddlOwnerService_SearchCustomer.DataBind();
                ddlOwnerService_SearchCustomer.Items.Insert(0, new ListItem("", ""));
            }

            udpUpdateCustomerCriteria.Update();
            JArray data = new JArray();
            udpSearchCustomerCriteria.Update();
            ClientService.DoJavascript("afterSearchBindCustomerCriteria(" + data + ");");
        }


        #endregion

        #endregion
        private void setDDLCriteria()
        {
            string equipmentCode = equipmentSelect.SelectedValue.Trim();
            DataTable dt = ServiceEquipment.getSeverityConfigByEquipment(SID, CompanyCode, equipmentCode);

            if (dt.Rows.Count > 0)
            {
                List<string> lstImpact = (from DataRow dr in dt.Rows select dr["ImpactCode"].ToString()).ToList();
                List<string> lstUrgency = (from DataRow dr in dt.Rows select dr["UrgencyCode"].ToString()).ToList();
                List<string> lstPriority = (from DataRow dr in dt.Rows select dr["PriorityCode"].ToString()).ToList();

                GetImpact(lstImpact);
                GetUrgency(lstUrgency);
                GetPriority(lstPriority);


            }
            else
            {
                GetImpact();
                GetUrgency();
                GetPriority();
            }
            udpnProblem.Update();
        }
        protected void btnLoadPriorityEquipment_Click(object sender, EventArgs e)
        {
            try
            {
                setDDLCriteria();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        protected void ddlSelectBindPriority_ImpactSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string impactCode = ddlImpact.SelectedValue;
                string urgencyCode = ddlUrgency.SelectedValue;

                DataTable dt = dtPriority.Clone();
                DataTable tmpdtPriority = dtPriority;
                string equipmentCode = equipmentSelect.SelectedValue.Trim();
                DataTable dtSeverityEquipment = ServiceEquipment.getSeverityConfigByEquipment(SID, CompanyCode, equipmentCode);
                if (dtSeverityEquipment.Rows.Count > 0)
                {
                    List<string> lstPriority = (from DataRow dr in dtSeverityEquipment.Rows select dr["PriorityCode"].ToString()).ToList();

                    tmpdtPriority = (from DataRow dr in dtPriority.Rows
                                     where lstPriority.Contains(dr["PriorityCode"].ToString())
                                     select dr)
                          .CopyToDataTable();

                    if (!String.IsNullOrEmpty(impactCode))
                    {
                        DataTable tmpdtImpactCheck = (from DataRow dr in tmpdtPriority.Rows
                                                      where dr["ImpactCode"].ToString() == impactCode
                                                      select dr)
                          .CopyToDataTable();

                        List<string> lstUrgency = (from DataRow dr in tmpdtImpactCheck.Rows select dr["UrgencyCode"].ToString()).ToList();

                        GetUrgency(lstUrgency);

                        if (lstUrgency.Contains(urgencyCode))
                        {
                            ddlUrgency.SelectedValue = urgencyCode;
                        }
                        else
                        {
                            ddlUrgency.SelectedValue = "";
                        }
                    }  
                }

                DataRow[] drr = tmpdtPriority.Select("ImpactCode='" + impactCode + "' and UrgencyCode='" + urgencyCode + "' ");
                
                if (drr.Length > 0)
                {
                    dt = drr.CopyToDataTable();
                }
                else
                {
                    dt = tmpdtPriority.DefaultView.ToTable(true, "PriorityCode", "Description");
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

                udpnProblem.Update();

            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        protected void ddlSelectBindPriority_UrgencySelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string impactCode = ddlImpact.SelectedValue;
                string urgencyCode = ddlUrgency.SelectedValue;

                DataTable dt = dtPriority.Clone();
                DataTable tmpdtPriority = dtPriority;
                string equipmentCode = equipmentSelect.SelectedValue.Trim();
                DataTable dtSeverityEquipment = ServiceEquipment.getSeverityConfigByEquipment(SID, CompanyCode, equipmentCode);
                if (dtSeverityEquipment.Rows.Count > 0)
                {
                    List<string> lstPriority = (from DataRow dr in dtSeverityEquipment.Rows select dr["PriorityCode"].ToString()).ToList();

                    tmpdtPriority = (from DataRow dr in dtPriority.Rows
                                     where lstPriority.Contains(dr["PriorityCode"].ToString())
                                     select dr)
                          .CopyToDataTable();

                    if (!String.IsNullOrEmpty(urgencyCode))
                    {
                        DataTable tmpdtUrgencyCheck = (from DataRow dr in tmpdtPriority.Rows
                                                       where dr["UrgencyCode"].ToString() == urgencyCode
                                                       select dr)
                          .CopyToDataTable();

                        List<string> lstImpact = (from DataRow dr in tmpdtUrgencyCheck.Rows select dr["ImpactCode"].ToString()).ToList();

                        GetImpact(lstImpact);

                        if (lstImpact.Contains(impactCode))
                        {
                            ddlImpact.SelectedValue = impactCode;
                        }
                        else
                        {
                            ddlImpact.SelectedValue = "";
                        }
                    } 
                }

                DataRow[] drr = tmpdtPriority.Select("ImpactCode='" + impactCode + "' and UrgencyCode='" + urgencyCode + "' ");

                if (drr.Length > 0)
                {
                    dt = drr.CopyToDataTable();
                }
                else
                {
                    dt = tmpdtPriority.DefaultView.ToTable(true, "PriorityCode", "Description");
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

                udpnProblem.Update();

            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        protected void btnCriteriaReset_Click(object sender, EventArgs e)
        {
            try
            {
                setDDLCriteria();
            }
            catch(Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }
    }

    [Serializable]
    public class IncidentAreaEnrity
    {
        public string GROUPCODE { get; set; }
        public string GROUPNAME { get; set; }
        public string TYPECODE { get; set; }
        public string TYPENAME { get; set; }
        public string SOURCCODE { get; set; }
        public string SOURCENAME { get; set; }
        public string AREACODE { get; set; }
        public string AREANAME { get; set; }

        public string GTCODE { get; set; }
        public string GTSCODE { get; set; }

        public string GTSACODE { get; set; }
    }
}