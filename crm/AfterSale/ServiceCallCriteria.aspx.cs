﻿using agape.lib.constant;
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
using System.Web.UI.WebControls;
using ERPW.Lib.Master.Config;
using Newtonsoft.Json.Linq;
using SNA.Lib.POS.utils;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Master;
using System.Web.Configuration;
using ERPW.Lib.Authentication.Entity;
using ERPW.Lib.Master.Entity;

namespace ServiceWeb.crm.AfterSale
{
    public partial class ServiceCallCriteria : AbstractsSANWebpage //System.Web.UI.Page
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

        protected override Boolean ProgramPermission_CanView()
        {
            return Permission.IncidentView
                || Permission.ProblemView
                || Permission.RequestView
                || Permission.ChangeOrderView
                || Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return Permission.IncidentModify
                || Permission.ProblemModify
                || Permission.RequestModify
                || Permission.ChangeOrderModify
                || Permission.AllPermission;
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
        private OwnerService ownerService = new OwnerService();

        private tmpServiceCallDataSet serviceCallEntity
        {
            get { return Session["ServicecallEntity"] == null ? new tmpServiceCallDataSet() : (tmpServiceCallDataSet)Session["ServicecallEntity"]; }
            set { Session["ServicecallEntity"] = value; }
        }

        #region DT Session Member Data

        DataTable dtTempAttachfile
        {
            get
            {
                return Session["dtTempAttachfile"] == null ? null : (DataTable)Session["dtTempAttachfile"];
            }
            set { Session["dtTempAttachfile"] = value; }
        }

        DataTable dtDataAssign
        {
            get { return Session["ServiceCallCriteria.SCFC_dtDataAssign"] == null ? null : (DataTable)Session["ServiceCallCriteria.SCFC_dtDataAssign"]; }
            set { Session["ServiceCallCriteria.SCFC_dtDataAssign"] = value; }
        }

        DataTable dtDataResolution
        {
            get { return Session["ServiceCallCriteria.SCFC_dtDataResolution"] == null ? null : (DataTable)Session["ServiceCallCriteria.SCFC_dtDataResolution"]; }
            set { Session["ServiceCallCriteria.SCFC_dtDataResolution"] = value; }
        }

        DataTable dtDataSearch
        {
            get { return Session["ServiceCallCriteria.SCFC_dtDataSearch"] == null ? null : (DataTable)Session["ServiceCallCriteria.SCFC_dtDataSearch"]; }
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

        DataTable dtSCType
        {
            get { return Session["ServiceCallCriteria.SCFC_SCType"] == null ? null : (DataTable)Session["ServiceCallCriteria.SCFC_SCType"]; }
            set { Session["ServiceCallCriteria.SCFC_SCType"] = value; }
        }

        DataTable dtContactPerson
        {
            get { return Session["ServiceCallCriteria.SCFC_dtContactPerson"] == null ? null : (DataTable)Session["ServiceCallCriteria.SCFC_dtContactPerson"]; }
            set { Session["ServiceCallCriteria.SCFC_dtContactPerson"] = value; }
        }

        DataTable dtGroup
        {
            get { return Session["ServiceCallCriteria.SCFC_dtGroup"] == null ? null : (DataTable)Session["ServiceCallCriteria.SCFC_dtGroup"]; }
            set { Session["ServiceCallCriteria.SCFC_dtGroup"] = value; }
        }

        DataTable dtProblemType
        {
            get { return Session["ServiceCallCriteria.SCFC_dtProblemType"] == null ? null : (DataTable)Session["ServiceCallCriteria.SCFC_dtProblemType"]; }
            set { Session["ServiceCallCriteria.SCFC_dtProblemType"] = value; }
        }

        private DataTable _dtStatus;
        private DataTable dtStatus
        {
            get
            {
                if (_dtStatus == null)
                {
                    _dtStatus = AfterSaleService.getInstance().getCallStatus("");
                }
                return _dtStatus;
            }
        }

        private DataTable _dtTicketDocStatus;
        private DataTable dtTicketDocStatus
        {
            get
            {
                if (_dtTicketDocStatus == null)
                {
                    var NotChage = lib.GetTicketDocStatus(SID, CompanyCode, false);
                    var Chage = lib.GetTicketDocStatus(SID, CompanyCode, true);

                    _dtTicketDocStatus = NotChage;
                    if (NotChage != Chage)
                    {
                        foreach (DataRow dtRow in NotChage.Rows)
                        {
                            for (int i = Chage.Rows.Count - 1; i >= 0; i--)
                            {
                                DataRow dtRoww = Chage.Rows[i];
                                if (dtRoww["DocumentStatusDesc"].ToString() == dtRow["DocumentStatusDesc"].ToString())
                                    dtRoww.Delete();
                            }
                            Chage.AcceptChanges();
                        }

                        _dtTicketDocStatus.Merge(Chage);

                        DataView dv = _dtTicketDocStatus.DefaultView;
                        dv.Sort = "DocumentStatusDesc asc";
                        _dtTicketDocStatus = dv.ToTable();
                    }
                }
                return _dtTicketDocStatus;
            }
        }


        private DataTable _dtPriority;
        private DataTable dtPriority
        {
            get
            {
                if (_dtPriority == null)
                {
                    _dtPriority = lib.GetSeverity(
                        SID, "", "", ""
                    );
                }
                return _dtPriority;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadinitData();
                setDefaultSearcCriteria();
                //GetddlSctype();
                GetddlStatus();
                setDefaulsearchPageLoad();

                _ddl_status.SelectedValue = "";//_ddl_status.SelectedValue = "01";
                _ddl_ticket_Doc_Status.SelectedValue = ""; //_ddl_ticket_Doc_Status.SelectedValue = "00";

                Session["SCT_created_equipment"] = null;
            }
        }

        #region Bind Data To Screen
        private void loadinitData()
        {
            dtTempAttachfile = AfterSaleService.getInstance().getCountAttachFile(SID, CompanyCode, "");
            DataTable _dtDataAssign = AfterSaleService.getInstance().getServicecallAssignForYou(SID, CompanyCode, EmployeeCode);
            DataTable _dtDataResolution = AfterSaleService.getInstance().getServicecallWaitForClose(SID, CompanyCode, EmployeeCode);

            if (!_dtDataAssign.Columns.Contains("total_attachfile"))
                _dtDataAssign.Columns.Add("total_attachfile");
            if (!_dtDataAssign.Columns.Contains("total_messagechat"))
                _dtDataAssign.Columns.Add("total_messagechat");
            if (!_dtDataResolution.Columns.Contains("total_attachfile"))
                _dtDataResolution.Columns.Add("total_attachfile");
            if (!_dtDataResolution.Columns.Contains("total_messagechat"))
                _dtDataResolution.Columns.Add("total_messagechat");

            dtDataAssign = _dtDataAssign.Clone();
            dtDataResolution = _dtDataResolution.Clone();

            foreach (DataRow dr in _dtDataAssign.Rows)
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

            foreach (DataRow dr in _dtDataResolution.Rows)
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

                dtSCType = AfterSaleService.getInstance().getSearchDoctype(
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


        private void GetddlStatus()
        {
            _ddl_status.Items.Clear();

            _ddl_status.Items.Add(new ListItem("", ""));
            _ddl_status.AppendDataBoundItems = true;
            _ddl_status.DataTextField = "Description";
            _ddl_status.DataValueField = "NAME";
            _ddl_status.DataSource = dtStatus;
            _ddl_status.DataBind();
        }


        #endregion

        private void GetTicketDocStatus()
        {
            //dtTicketDocStatus = lib.GetTicketDocStatus(SID, CompanyCode, false);

            _ddl_ticket_Doc_Status.DataTextField = "DocumentStatusDesc";
            _ddl_ticket_Doc_Status.DataValueField = "DocumentStatus";
            _ddl_ticket_Doc_Status.DataSource = dtTicketDocStatus;
            _ddl_ticket_Doc_Status.DataBind();
            _ddl_ticket_Doc_Status.Items.Insert(0, new ListItem("", ""));
        }
        private void getcontact_person()
        {

            DataTable dt = new DataTable();

            if ("" == "")
            {
                dt.Columns.Add("BOBJECTLINK");
                dt.Columns.Add("NAME1");
                dt.Columns.Add("email");
                dt.Columns.Add("phone");
                dt.Columns.Add("remark");
            }
            else
            {
                dt = AfterSaleService.getInstance().getContactPerson(CompanyCode, "");
            }


            GetcontactDetailForScreen();
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
            string customer = (string)Session["SCT_created_cust_code"];//CustomerSelect.SelectedValue;
            string idGen = redirectViewToTicketDetail(customer, doctype, docnumber, fiscalyear);

            string PageRedirect = ServiceTicketLibrary.GetInstance().getPageTicketRedirect(
                SID,
                (Session["ServicecallEntity" + idGen] as tmpServiceCallDataSet).cs_servicecall_header.Rows[0]["Doctype"].ToString()
            );
            if (!String.IsNullOrEmpty(idGen))
            {
                ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen) + "');");
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


            Session["SCT_created_remark" + idGen] = null;
            serviceCallEntity = new tmpServiceCallDataSet();
            Session["ServicecallEntity" + idGen] = new tmpServiceCallDataSet();
            Session["SC_MODE" + idGen] = ApplicationSession.CREATE_MODE_STRING;


            dtTempDoc.Clear();
            ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/ServiceCallTransaction.aspx?id=" + idGen) + "');");
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";
                if (!string.IsNullOrEmpty(message))
                    throw new Exception(message);

                UniversalService universalService = new UniversalService();
                bool hasTicket = universalService.CheckOpenTicket(SID, CompanyCode, "", "", "");

                if (hasTicket)
                {
                    ClientService.DoJavascript("warningClick('" + "" + "', '" + "" + "', '" + "" + "');");
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

            if ("" == "")
            {
                ClientService.DoJavascript("loadEquipmentWithoutCondition" + "" + "(); loadCustomerWithoutCondition" + "" + "();");
            }
            else
            {
                DataTable dt = universalService.GetEquipmentCustomerAssignment(SID, CompanyCode, "", "");

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
                    if ("" != "")
                    {
                        var en = result.Find(x => x.code == "");
                        if (en != null)
                        {
                            defaultValue = en.display;
                        }
                    }
                }

                string responseJson = JsonConvert.SerializeObject(result);
                ClientService.DoJavascript("bindAutoCompleteEquipment" + "" + "(" + responseJson + ", '" + defaultValue + "',false);");
            }
        }

        protected void btnBindContactCus_Click(object sender, EventArgs e)
        {
            //do after select customer name
            try
            {
                GetEquipmentMappingOwner();
                getcontact_person();
                //}
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

                if ("" == "")
                {
                    ClientService.DoJavascript("loadEquipmentWithoutCondition" + "" + "();");

                }
                else
                {
                    DataTable dt = universalService.GetEquipmentCustomerAssignment(SID, CompanyCode, "", "");

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
                        if ("" != "")
                        {
                            var en = result.Find(x => x.code == "");
                            if (en != null)
                            {
                                defaultValue = en.display;
                            }
                        }
                    }

                    string responseJson = JsonConvert.SerializeObject(result);
                    ClientService.DoJavascript("bindAutoCompleteCustomer" + "" + "(" + responseJson + ", '" + defaultValue + "',false);");
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
            DataTable dt = dtPriority.Clone();
            DataRow[] drr = dtPriority.Select("ImpactCode='" + "" + "' and UrgencyCode='" + "" + "' ");
            if (drr.Length > 0)
            {
                dt = drr.CopyToDataTable();
            }
            else
            {
                dt = dtPriority.DefaultView.ToTable(true, "PriorityCode", "Description");
            }
        }

        protected void ddlSelectBindPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetSeverity();
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
            ERPW.Lib.Master.ContactEntity en = new ERPW.Lib.Master.ContactEntity();

            if (!string.IsNullOrEmpty(""))
            {
                List<ERPW.Lib.Master.ContactEntity> listen = serviceCustomer.getListContactRefCustomer(
                    SID,
                    CompanyCode,
                    "",
                    "",
                    ""
                );
                if (listen.Count > 0)
                {
                    en = listen[0];
                }
            }
            else
            {

            }


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

        List<IncidentAreaEnrityx> mListIncidentArea
        {
            get
            {
                if (Session["ServiceCallFast_IncidentAreaEnrity_mListIncidentArea"] == null)
                {
                    Session["ServiceCallFast_IncidentAreaEnrity_mListIncidentArea"] = new List<IncidentAreaEnrityx>();
                }
                return (List<IncidentAreaEnrityx>)Session["ServiceCallFast_IncidentAreaEnrity_mListIncidentArea"];
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

                string fiscalyear = _txt_year_search.Value.Trim();
                string customercode = AutoCustomerSearch.SelectedValue;
                string priority = ddlPrioritySearch.SelectedValue;
                List<string> listDoctype = new List<string>();
                //string doctype = _ddl_sctype_search.SelectedValue;
                string datefrom = string.IsNullOrEmpty(ctrlDateFrom.Text) ? "" : Validation.Convert2DateDB(ctrlDateFrom.Text);
                string dateto = string.IsNullOrEmpty(ctrlDateTo.Text) ? "" : Validation.Convert2DateDB(ctrlDateTo.Text);

                string timefrom = string.IsNullOrEmpty(ctrlTimeFrom.Text) ? "" : Validation.Convert2TimeDB(ctrlTimeFrom.Text);
                string timeto = string.IsNullOrEmpty(ctrlTimeTo.Text) ? "" : Validation.Convert2TimeDB(ctrlTimeTo.Text);

                string contactname = _ddl_contact_person_search.SelectValue;
                string status = _ddl_ticket_Doc_Status.SelectedValue; //string status = _ddl_status.SelectedValue;
                string docStatucActive = _ddl_document_Doc_Status.SelectedValue;
                string docnumber = _txt_docnumber.Value;

                if (string.IsNullOrEmpty(_ddl_sctype_search.SelectedValue))
                {
                    foreach (ListItem item in _ddl_sctype_search.Items)
                    {
                        if (!string.IsNullOrEmpty(item.Value))
                        {
                            listDoctype.Add(item.Value);
                        }
                    }
                }
                else
                {
                    listDoctype.Add(_ddl_sctype_search.SelectedValue);
                }

                dtDataSearch = lib.SearchTicketList(
                    SID, CompanyCode, listDoctype, fiscalyear, docnumber, status,
                    docStatucActive, datefrom, dateto, ddlImpactSearch.SelectedValue,
                    ddlUrgencySearch.SelectedValue, priority,
                    AutoEquipmentSearch.SelectedValue, customercode, 
                    txtSearchSubject.Text.ToString(), ddlOwnerGroupService.SelectedValue,
                    ddlProblemGroup.SelectedValue, ddlProblemType.SelectedValue,
                    ddlProblemSource.SelectedValue, ddlServiceSource.SelectedValue,
                    _ddl_contact_person_search.SelectText, timefrom, timeto,
                    txtContactEmail_search.Text, txtSearchDiscription.Text
                );

                //string jsonData = JsonConvert.SerializeObject(dtDataSearch);


                //rptSearchSale.DataSource = dtDataSearch;
                //rptSearchSale.DataBind();
                DataTable _dtDataSearch = dtDataSearch.DefaultView.ToTable(
                    false,
                    "DOCDATE",
                    "EndDateTime",
                    "DOCTIME",
                    "DocumentTypeDesc",
                    "CallerIDDisplay",
                    "CallerID",
                    "CallStatus",
                    "DocStatusDesc",
                    "HeaderText",
                    "CustomerName",
                    "ContractPersonName",
                    "EquipmentName",
                    "ImpactName",
                    "UrgencyName",
                    "PriorityName",
                    "OwnerGroupName",
                    "FullName_EN"
                );

                divDataJson.InnerHtml = JsonConvert.SerializeObject(_dtDataSearch).Replace("<", "&lt;").Replace(">", "&gt;");
                udpnItems.Update();

                ClientService.DoJavascript("afterSearch();");
                ClientService.DoJavascript("scrollToTable();");
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
                if (_ddl_sctype_search.Items.Count > 0)
                {

                }
                else
                {
                }

                try
                {
                    _ddl_document_Doc_Status.SelectedValue = "01";
                    _ddl_ticket_Doc_Status.SelectedValue = "00";
                }
                catch (Exception)
                {
                }
                udpDefauleSearch.Update();
                udpDefauleSearch2.Update();
                //udpDefauleSearch3.Update();

                ClientService.DoJavascript("showInitiativeModal('modalSearchHelpTicketCriteria');");
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
                string impactCode = ddlImpactSearch.SelectedValue;
                string urgencyCode = ddlUrgencySearch.SelectedValue;

                DataRow[] drr = dtPriority.Select("ImpactCode='" + impactCode + "' and UrgencyCode='" + urgencyCode + "' ");
                if (drr.Length > 0)
                {
                    ddlPrioritySearch.SelectedValue = drr[0]["PriorityCode"].ToString();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }



        #region Load data properties
        private void setDefaultSearcCriteria()
        {

            _ddl_sctype_search.Items.Clear();
            GetddlSctype();

            _ddl_sctype_search.AppendDataBoundItems = true;
            _ddl_sctype_search.DataTextField = "Description";
            _ddl_sctype_search.DataValueField = "DocumentTypeCode";
            _ddl_sctype_search.DataSource = dtSCType;
            _ddl_sctype_search.DataBind();
            if (dtSCType.Rows.Count > 0)
            {
                _ddl_sctype_search.Items.Insert(0, new ListItem("", ""));
            }


            GetTicketDocStatus();

            DataTable dt = lib.GetImpactMaster(SID);
            ddlImpactSearch.DataTextField = "ImpactName";
            ddlImpactSearch.DataValueField = "ImpactCode";
            ddlImpactSearch.DataSource = dt;
            ddlImpactSearch.DataBind();
            ddlImpactSearch.Items.Insert(0, new ListItem("", ""));


            dt = lib.GetUrgencyMaster(SID);
            ddlUrgencySearch.DataTextField = "UrgencyName";
            ddlUrgencySearch.DataValueField = "UrgencyCode";
            ddlUrgencySearch.DataSource = dt;
            ddlUrgencySearch.DataBind();
            ddlUrgencySearch.Items.Insert(0, new ListItem("", ""));

            DataTable dtTemp = dtPriority.Clone();
            if (dtPriority.Rows.Count > 0)
            {
                dtTemp = dtPriority.DefaultView.ToTable(true, "PriorityCode", "Description");
            }
            ddlPrioritySearch.DataSource = dtTemp;
            ddlPrioritySearch.DataBind();
            ddlPrioritySearch.Items.Insert(0, new ListItem("", ""));

            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);

            //if (FilterOwner && !Permission.AllPermission)
            //{
            //    ddlOwnerGroup.Items.Clear();
            //    ddlOwnerGroup.Items.Insert(0, 
            //        new ListItem(
            //            Permission.OwnerGroupName, 
            //            Permission.OwnerGroupCode
            //        )
            //    );
            //    ddlOwnerGroup.Enabled = false;
            //    ddlOwnerGroup.CssClass = "form-control form-control-sm";
            //}
            //else
            //{
            //    DataTable dtOwnerGroup = config.GetMasterConfigOwnerGroup(SID, CompanyCode, "");
            //    ddlOwnerGroup.DataSource = dtOwnerGroup;
            //    ddlOwnerGroup.DataBind();
            //    ddlOwnerGroup.Items.Insert(0, new ListItem("", ""));
            //}

            GetOwnerGroupService();

            dt = config.getIncidentAreaRawData(SID);
            mListIncidentArea = JsonConvert.DeserializeObject<List<IncidentAreaEnrityx>>(JsonConvert.SerializeObject(dt));
            dt.Clear();
            //setDefaultIncidentArea();
        }
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
        private void setDefaulsearchPageLoad()
        {
            _ddl_contact_person_search.initialDataAutoComplete(new DataTable(), "", "");
            ddlOwnerGroupService.SelectedIndex = 0;
            ddlProblemGroup.SelectedIndex = 0;
            ddlProblemType.SelectedIndex = 0;
            ddlProblemSource.SelectedIndex = 0;
            ddlServiceSource.SelectedIndex = 0;
            //txtProblemGroup.initialDataAutoComplete(new DataTable(), "", "");
            //txtProblemType.initialDataAutoComplete(new DataTable(), "", "");
            //txtProblemSource.initialDataAutoComplete(new DataTable(), "", "");
            //txtContactSource.initialDataAutoComplete(new DataTable(), "", "");


        }
        private void getcontact_person_search()
        {
            string custcode = AutoCustomerSearch.SelectedValue.Trim();

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
                dt = AfterSaleService.getInstance().getContactPerson(CompanyCode, custcode);
            }

            _ddl_contact_person_search.initialDataAutoComplete(dt, "BOBJECTLINK", "NAME1", false, "email", "phone", "remark");
            GetcontactDetailForScreen_search();
            //updContactCus.Update();
        }
        private void GetcontactDetailForScreen_search()
        {
            string bobjectid = _ddl_contact_person_search.SelectValue;
            ERPW.Lib.Master.ContactEntity en = new ERPW.Lib.Master.ContactEntity();

            if (!string.IsNullOrEmpty(bobjectid))
            {
                List<ERPW.Lib.Master.ContactEntity> listen = serviceCustomer.getListContactRefCustomer(
                    SID,
                    CompanyCode,
                    AutoCustomerSearch.SelectedValue,
                    "",
                    bobjectid
                );
                if (listen.Count > 0)
                {
                    en = listen[0];

                    txtContactPhone_search.Text = en.phone.Trim();
                    txtContactEmail_search.Text = en.email.Trim();
                    txtContactremark_search.Text = en.REMARK;
                }
            }
            else
            {
                txtContactPhone_search.Text = "";
                txtContactEmail_search.Text = "";
                txtContactremark_search.Text = "";
            }

            udpContactDetailSearch.Update();
        }

        #endregion

        #region Select Customer OR Equipment for Search

        protected void btnBindContactForSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GetEquipmentMappingOwner_search();
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
                GetOwnerMappingEquipment_search();
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

        private void GetEquipmentMappingOwner_search()
        {
            string equipmentCode = AutoEquipmentSearch.SelectedValue.Trim();
            string customerCode = AutoCustomerSearch.SelectedValue.Trim();

            if (customerCode == "")
            {
                ClientService.DoJavascript("loadEquipmentWithoutCondition" + AutoEquipmentSearch.ClientID + "(); loadCustomerWithoutCondition" + AutoCustomerSearch.ClientID + "();");
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
                AutoEquipmentSearch.SelectedDisplay = defaultValue;
                ClientService.DoJavascript("bindAutoCompleteEquipment" + AutoEquipmentSearch.ClientID + "(" + responseJson + ", '" + defaultValue + "',false);");
            }
        }

        private void GetOwnerMappingEquipment_search()
        {
            string equipmentCode = AutoEquipmentSearch.SelectedValue.Trim();
            string customerCode = AutoCustomerSearch.SelectedValue.Trim();

            if (equipmentCode == "")
            {
                ClientService.DoJavascript("loadEquipmentWithoutCondition" + AutoEquipmentSearch.ClientID + "(); loadCustomerWithoutCondition" + AutoCustomerSearch.ClientID + "();");
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
                AutoCustomerSearch.SelectedDisplay = defaultValue;
                ClientService.DoJavascript("bindAutoCompleteCustomer" + AutoCustomerSearch.ClientID + "(" + responseJson + ", '" + defaultValue + "',false);");
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

                btnLoadCustomerEquipment_Click(sender, e);

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

                // #Edit for Multi OwnerService CriteriaEquipmentSearch

                if (FilterOwner && !Permission.AllPermission)
                {
                    DataTable dtDataUserOwnerService = ownerService.getMappingOwner(UserName);//get data ownerService


                    ddlOwnerService.Items.Clear();
                    ddlOwnerService.Items.Insert(0,
                        new ListItem(
                            Permission.OwnerGroupName,
                            Permission.OwnerGroupCode
                        )
                    );

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

                //ddlOwnerService.DataTextField = "OwnerGroupName";
                //ddlOwnerService.DataValueField = "OwnerGroupCode";
                //ddlOwnerService.DataSource = config.GetMasterConfigOwnerGroup(SID, CompanyCode, "");
                //ddlOwnerService.DataBind();
                //ddlOwnerService.Items.Insert(0, new ListItem("All", ""));
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

            if (!string.IsNullOrEmpty(ddlOwnerService_SearchCustomer.SelectedValue))
            {
                //ListCustomer = ListCustomer.Where(w => w.OwnerService == ddlOwnerService.SelectedValue).ToList(); old code
                ListCustomer = ListCustomer.Where(w => w.OwnerServiceCode == ddlOwnerService_SearchCustomer.SelectedValue).ToList();

            }

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

            // #Edit for Multi OwnerService CriteriaCustomerSearch


            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);


            if (FilterOwner && !Permission.AllPermission)
            {
                DataTable dtDataUserOwnerService = ownerService.getMappingOwner(UserName);//get data ownerService

                ddlOwnerService.Items.Clear();
                ddlOwnerService.Items.Insert(0,
                    new ListItem(
                        Permission.OwnerGroupName,
                        Permission.OwnerGroupCode
                    )
                );

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

        #region I
        //private MasterConfigLibrary libconfig = MasterConfigLibrary.GetInstance();

        private void GetOwnerGroupService()
        {
            //DataTable dtOwner = config.GetMasterConfigOwnerGroup(SID, CompanyCode, "");
            //ddlOwnerGroupService.DataValueField = "OwnerGroupCode";
            //ddlOwnerGroupService.DataTextField = "OwnerGroupName";
            //ddlOwnerGroupService.DataSource = dtOwner;
            //ddlOwnerGroupService.DataBind();
            //ddlOwnerGroupService.Items.Insert(0, new ListItem("", ""));

            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);

            // #Edit for Multi OwnerService setDefaultSearcCriteria

            if (FilterOwner && !Permission.AllPermission)
            {
                ddlOwnerGroupService.Items.Clear();
                ddlOwnerGroupService.Items.Insert(0,
                    new ListItem(
                        Permission.OwnerGroupName,
                        Permission.OwnerGroupCode
                    )
                );

               
                DataTable dtDataUserOwnerService = ownerService.getMappingOwner(UserName);//get data ownerService

                ddlOwnerGroupService.DataTextField = "OwnerGroupName";
                ddlOwnerGroupService.DataValueField = "OwnerService";
                ddlOwnerGroupService.DataSource = dtDataUserOwnerService;
                ddlOwnerGroupService.DataBind();
                ddlOwnerGroupService.SelectedIndex = 0;

                if (dtDataUserOwnerService.Rows.Count == 1)
                {
                    ddlOwnerGroupService.Enabled = false;
                    ddlOwnerGroupService.CssClass = "form-control form-control-sm";
                }
             
            }
            else
            {
                DataTable dtOwnerGroup = config.GetMasterConfigOwnerGroup(SID, CompanyCode, "");
                ddlOwnerGroupService.DataSource = dtOwnerGroup;
                ddlOwnerGroupService.DataBind();
                ddlOwnerGroupService.Items.Insert(0, new ListItem("", ""));
            }

            GetProblemGroup();
            GetProblemType(ddlProblemGroup.SelectedValue);
            GetProblemSource(ddlProblemGroup.SelectedValue, ddlProblemType.SelectedValue);
            GetServiceSource(ddlProblemGroup.SelectedValue, ddlProblemType.SelectedValue, ddlProblemSource.SelectedValue);
        }


        private void GetProblemGroup()
        {
            string oldValue = ddlProblemGroup.Items.Count > 0 ? ddlProblemGroup.SelectedValue : "";

            string _doctype = "";

            foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
            {
                _doctype = Convert.ToString(dr["DocType"]);
            }
            //string businessObject = lib.GetBusinessObjectFromTicketType(SID, _doctype);
            //mBusinessObject = businessObject;

            List<IncidentAreaEntity.AreaGroup> En = config.getIncedentAreaGroup(
                SID, CompanyCode,
                "", //mBusinessObject, 
                ddlOwnerGroupService.SelectedValue
            );

            if (!string.IsNullOrEmpty(ddlOwnerGroupService.SelectedValue))
                En = En.Where(w => w.ChildrenOwnerGroup.Contains(ddlOwnerGroupService.SelectedValue)).ToList();
            En.ForEach(r => { r.GroupName = r.GroupCode + " : " + r.GroupName; });

            ddlProblemGroup.DataValueField = "GroupCode";
            ddlProblemGroup.DataTextField = "GroupName";
            ddlProblemGroup.DataSource = En.Select(s => new { s.GroupCode, s.GroupName }).Distinct().ToList();
            ddlProblemGroup.DataBind();
            ddlProblemGroup.Items.Insert(0, new ListItem("", ""));

            if (ddlProblemGroup.Items.FindByValue(oldValue) != null)
            {
                ddlProblemGroup.SelectedValue = oldValue;
            }
            else
            {
                ddlProblemGroup.SelectedIndex = 0;
            }
        }

        private void GetProblemType(string problemGroup)
        {
            string oldValue = ddlProblemType.Items.Count > 0 ? ddlProblemType.SelectedValue : "";

            List<IncidentAreaEntity.AreaType> En = new List<IncidentAreaEntity.AreaType>();
            DataTable dt = new DataTable();

            //if (!string.IsNullOrEmpty(problemGroup))
            //{
            En = config.getIncedentAreaType(
                SID, CompanyCode,
                "", //mBusinessObject,
                ddlOwnerGroupService.SelectedValue, true, problemGroup
            );
            if (!string.IsNullOrEmpty(ddlOwnerGroupService.SelectedValue))
                En = En.Where(w => w.ChildrenOwnerGroup.Contains(ddlOwnerGroupService.SelectedValue)).ToList();
            //}
            En.ForEach(r => { r.TypeName = r.TypeCode + " : " + r.TypeName; });

            ddlProblemType.DataValueField = "TypeCode";
            ddlProblemType.DataTextField = "TypeName";
            ddlProblemType.DataSource = En.Select(s => new { s.TypeCode, s.TypeName }).Distinct().ToList();
            ddlProblemType.DataBind();
            ddlProblemType.Items.Insert(0, new ListItem("", ""));

            if (ddlProblemType.Items.FindByValue(oldValue) != null)
            {
                ddlProblemType.SelectedValue = oldValue;
            }
            else
            {
                ddlProblemType.SelectedIndex = 0;
            }
        }

        private void GetProblemSource(string IncidentGroup, string IncidentType)
        {
            string oldValue = ddlProblemSource.Items.Count > 0 ? ddlProblemSource.SelectedValue : "";

            List<IncidentAreaEntity.AreaSource> En = new List<IncidentAreaEntity.AreaSource>();

            //if (!string.IsNullOrEmpty(IncidentGroup) && !string.IsNullOrEmpty(IncidentType))
            //{
            En = config.getIncedentAreaSource(
                SID, CompanyCode,
                "", //mBusinessObject,
                ddlOwnerGroupService.SelectedValue, true, IncidentGroup, IncidentType
            );
            if (!string.IsNullOrEmpty(ddlOwnerGroupService.SelectedValue))
                En = En.Where(w => w.ChildrenOwnerGroup.Contains(ddlOwnerGroupService.SelectedValue)).ToList();
            //}
            En.ForEach(r => { r.SourceName = r.SourceCode + " : " + r.SourceName; });

            ddlProblemSource.DataValueField = "SourceCode";
            ddlProblemSource.DataTextField = "SourceName";
            ddlProblemSource.DataSource = En.Select(s => new { s.SourceCode, s.SourceName }).Distinct().ToList();
            ddlProblemSource.DataBind();
            ddlProblemSource.Items.Insert(0, new ListItem("", ""));

            if (ddlProblemSource.Items.FindByValue(oldValue) != null)
            {
                ddlProblemSource.SelectedValue = oldValue;
            }
            else
            {
                ddlProblemSource.SelectedIndex = 0;
            }
        }

        private void GetServiceSource(string IncidentGroup, string IncidentType, string IncidentSource)
        {
            string oldValue = ddlServiceSource.Items.Count > 0 ? ddlServiceSource.SelectedValue : "";

            List<IncidentAreaEntity.AreaContactSource> En = new List<IncidentAreaEntity.AreaContactSource>();

            //if (!string.IsNullOrEmpty(IncidentGroup)
            //    && !string.IsNullOrEmpty(IncidentType)
            //    && !string.IsNullOrEmpty(IncidentSource))
            //{
            En = config.getIncedentAreaContactSource(
                SID, CompanyCode,
                "", //mBusinessObject,
                ddlOwnerGroupService.SelectedValue, IncidentGroup, IncidentType, IncidentSource
            );
            if (!string.IsNullOrEmpty(ddlOwnerGroupService.SelectedValue))
                En = En.Where(w => w.ChildrenOwnerGroup.Contains(ddlOwnerGroupService.SelectedValue)).ToList();
            //}
            En.ForEach(r => { r.ContactSourceName = r.ContactSourceCode + " : " + r.ContactSourceName; });

            ddlServiceSource.DataValueField = "ContactSourceCode";
            ddlServiceSource.DataTextField = "ContactSourceName";
            ddlServiceSource.DataSource = En.Select(s => new { s.ContactSourceCode, s.ContactSourceName }).Distinct().ToList();
            ddlServiceSource.DataBind();
            ddlServiceSource.Items.Insert(0, new ListItem("", ""));

            if (ddlServiceSource.Items.FindByValue(oldValue) != null)
            {
                ddlServiceSource.SelectedValue = oldValue;
            }
            else
            {
                ddlServiceSource.SelectedIndex = 0;
            }
        }

        #endregion

        #region Change Area Selected

        protected void ddlOwnerGroupService_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetProblemGroup();
                GetProblemType(ddlProblemGroup.SelectedValue);
                GetProblemSource(ddlProblemGroup.SelectedValue, ddlProblemType.SelectedValue);
                GetServiceSource(ddlProblemGroup.SelectedValue, ddlProblemType.SelectedValue, ddlProblemSource.SelectedValue);
                udpnProblem.Update();
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

        protected void btnChangeProblemGroup_Click(object sender, EventArgs e)
        {
            try
            {
                GetProblemType(ddlProblemGroup.SelectedValue);
                GetProblemSource(ddlProblemGroup.SelectedValue, ddlProblemType.SelectedValue);
                GetServiceSource(ddlProblemGroup.SelectedValue, ddlProblemType.SelectedValue, ddlProblemSource.SelectedValue);
                //bindTierOperationRefIncidentArea();
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

        protected void btnChangeProblemType_Click(object sender, EventArgs e)
        {
            try
            {
                GetProblemSource(ddlProblemGroup.SelectedValue, ddlProblemType.SelectedValue);
                GetServiceSource(ddlProblemGroup.SelectedValue, ddlProblemType.SelectedValue, ddlProblemSource.SelectedValue);
                //bindTierOperationRefIncidentArea();
                //SetOwnerGroupServiceRefIncidentArea(ddlProblemGroup.SelectedValue , ddlProblemType.SelectedValue);
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

        protected void btnChangeProblemSource_Click(object sender, EventArgs e)
        {
            try
            {
                GetServiceSource(ddlProblemGroup.SelectedValue, ddlProblemType.SelectedValue, ddlProblemSource.SelectedValue);
                //bindTierOperationRefIncidentArea();
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

        protected void btnChangeServiceSource_Click(object sender, EventArgs e)
        {
            try
            {
                //bindTierOperationRefIncidentArea();
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
    }

    [Serializable]
    public class IncidentAreaEnrityx
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
