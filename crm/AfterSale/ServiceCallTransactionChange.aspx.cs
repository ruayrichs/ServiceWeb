using agape.lib.constant;
using Agape.FocusOne.Utilities;
using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Authentication;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Master.Entity;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using ERPW.Lib.Service.Workflow;
using ERPW.Lib.Service.Workflow.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceWeb.Accountability.Service;
using ServiceWeb.auth;
using ServiceWeb.KM;
using ServiceWeb.LinkFlowChart.Service;
using ServiceWeb.Service;
using SNAWeb.Analytics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceWeb.UserControl.AutoComplete;
using ServiceWeb.widget.usercontrol;
using System.Globalization;
using Agape.Lib.DBService;
using ERPW.Lib.Authentication.Entity;
using System.Diagnostics;
using System.IO;

namespace ServiceWeb.crm.AfterSale
{
    public partial class ServiceCallTransactionChange : AbstractsSANWebpage
    {
        string ThisPage = "ServiceCallTransactionChange";


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

        //public string _CompanyName;
        //public string CompanyName
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_CompanyName))
        //        {
        //            _CompanyName = ERPWAuthentication.CompanyName;
        //        }
        //        return _CompanyName;
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

        #region
        protected override Boolean ProgramPermission_CanView()
        {
            return Permission.ChangeOrderView || Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return Permission.ChangeOrderModify || Permission.AllPermission;
        }

        private string _idGen;
        private string idGen
        {
            get
            {
                if (string.IsNullOrEmpty(_idGen))
                {
                    _idGen = Request["id"];
                }
                return _idGen;
            }
        }
        #endregion

        private tmpServiceCallDataSet _serviceCallEntity;
        private tmpServiceCallDataSet serviceCallEntity
        {
            get
            {
                if (_serviceCallEntity == null)
                {
                    if (Session["ServicecallEntity" + idGen] == null)
                    {
                        Session["ServicecallEntity" + idGen] = new tmpServiceCallDataSet();
                    }

                    _serviceCallEntity = (tmpServiceCallDataSet)Session["ServicecallEntity" + idGen];
                }

                if (_serviceCallEntity.cs_servicecall_header.PrimaryKey.Length == 6)
                    _serviceCallEntity.cs_servicecall_header.PrimaryKey = new DataColumn[] { 
                        _serviceCallEntity.cs_servicecall_header.Columns["SID"],
                        _serviceCallEntity.cs_servicecall_header.Columns["CompanyCode"],
                        _serviceCallEntity.cs_servicecall_header.Columns["CallerID"],
                        _serviceCallEntity.cs_servicecall_header.Columns["CustomerCode"],
                        _serviceCallEntity.cs_servicecall_header.Columns["Doctype"],
                        _serviceCallEntity.cs_servicecall_header.Columns["Fiscalyear"]
                    };

                if (_serviceCallEntity.cs_servicecall_item.PrimaryKey.Length == 4)
                    _serviceCallEntity.cs_servicecall_item.PrimaryKey = new DataColumn[] { 
                        _serviceCallEntity.cs_servicecall_item.Columns["SID"],
                        _serviceCallEntity.cs_servicecall_item.Columns["CompanyCode"],
                        _serviceCallEntity.cs_servicecall_item.Columns["xLineNo"],
                        _serviceCallEntity.cs_servicecall_item.Columns["ObjectID"]
                    };

                return _serviceCallEntity;
            }
            set
            {
                Session["ServicecallEntity" + idGen] = value;
                if (_serviceCallEntity != null)
                {
                    _serviceCallEntity = value;
                }
            }
        }
        private string _DocType;
        private string DocType
        {
            get
            {
                if (_DocType == null)
                {
                    if (serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
                    {
                        foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
                        {
                            _DocType = Convert.ToString(dr["DocType"]);
                        }
                    }
                    else
                    {
                        _DocType = (string)Session["SCT_created_doctype_code" + idGen];
                    }
                }
                return _DocType;
            }
        }

        private string _businessObject;
        private string businessObject
        {
            get
            {
                if (_businessObject == null)
                {
                    _businessObject = lib.GetBusinessObjectFromTicketType(SID, DocType);
                }
                return _businessObject;
            }
        }

        private DataTable servicecallCustomer { get; set; }
        Agape.Lib.DBService.DBService DBService = new Agape.Lib.DBService.DBService();
        UniversalService universalService = new UniversalService();
        ServiceLibrary libService = new ServiceLibrary();
        ServiceTicketLibrary lib = new ServiceTicketLibrary();
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        AccountabilityService accountabilityService = new AccountabilityService();
        public const string WorkGroupCode = "20170121162748444411";
        LogServiceLibrary libLog = new LogServiceLibrary();
        private MasterConfigLibrary libconfig = MasterConfigLibrary.GetInstance();
        private ERPW.Lib.Master.CustomerService serviceCustomer = ERPW.Lib.Master.CustomerService.getInstance();
        EquipmentService libCI = new EquipmentService();
        KMServiceLibrary libkm = KMServiceLibrary.getInstance();
        WorkflowService libWorkFlow = WorkflowService.getInstance();

        public string _mode_stage;
        public string mode_stage
        {
            get
            {
                if (_mode_stage == null)
                {
                    if (Session["SC_MODE" + idGen] == null)
                    {
                        Session["SC_MODE" + idGen] = ApplicationSession.CREATE_MODE_STRING;
                        _mode_stage = ApplicationSession.CREATE_MODE_STRING;
                    }
                    else
                    {
                        _mode_stage = (string)Session["SC_MODE" + idGen];
                    }
                }
                return _mode_stage;
            }
            set
            {
                Session["SC_MODE" + idGen] = value;
                _mode_stage = value;
            }
        }


        private string _lastAobject;
        private string lastAobject
        {
            get
            {
                if (_lastAobject == null)
                {
                    _lastAobject = AfterSaleService.getInstance().getLastActivityServiceCall(
                        CompanyCode,
                        hddDocnumberTran.Value,
                        EquipmentSelect,
                        EquipmentItemNo
                    );
                }
                return _lastAobject;
            }
        }

        private int countActivity
        {
            get { return Session["SCT_countActivity" + idGen] == null ? 0 : (int)Session["SCT_countActivity" + idGen]; }
            set { Session["SCT_countActivity" + idGen] = value; }
        }

        public string CustomerCode
        {
            get { return hdfCustomerCode.Value; }
            set { hdfCustomerCode.Value = value; }
        }

        public string AreaCode
        {
            get { return hdfAreaCode.Value; }
            set { hdfAreaCode.Value = value; }
        }

        public string mBusinessObject
        {
            get { return hddBusinessObject.Value; }
            set { hddBusinessObject.Value = value; }
        }

        public Boolean IsCallTicketStatusClose
        {
            get
            {
                if (serviceCallEntity.cs_servicecall_header == null || serviceCallEntity.cs_servicecall_header.Rows.Count == 0)
                {
                    return false;
                }

                return serviceCallEntity.cs_servicecall_header.Rows[0]["CallStatus"].ToString() == ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE;
            }
        }

        public Boolean IsCallTicketStatusCancel
        {
            get
            {
                if (serviceCallEntity.cs_servicecall_header == null || serviceCallEntity.cs_servicecall_header.Rows.Count == 0)
                {
                    return false;
                }

                return serviceCallEntity.cs_servicecall_header.Rows[0]["CallStatus"].ToString() == ServiceTicketLibrary.SERVICE_CALL_STATUS_CANCEL;
            }
        }

        public Boolean IsDocTicketStatusClose
        {
            get
            {
                if (serviceCallEntity.cs_servicecall_header == null || serviceCallEntity.cs_servicecall_header.Rows.Count == 0)
                {
                    return false;
                }

                string Docstatus = serviceCallEntity.cs_servicecall_header.Rows[0]["Docstatus"].ToString();

                return Docstatus == lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CLOSED_BUSINESS_CHANGE);
            }
        }


        public Boolean IsDocTicketStatusCancel
        {
            get
            {
                if (serviceCallEntity.cs_servicecall_header == null || serviceCallEntity.cs_servicecall_header.Rows.Count == 0)
                {
                    return false;
                }
                string Docstatus = serviceCallEntity.cs_servicecall_header.Rows[0]["Docstatus"].ToString();
                return Docstatus == lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CANCEL_BUSINESS_CHANGE);
            }
        }

        public Boolean IsDocTicketStatusRollBack
        {
            get
            {
                if (serviceCallEntity.cs_servicecall_header == null || serviceCallEntity.cs_servicecall_header.Rows.Count == 0)
                {
                    return false;
                }
                string Docstatus = serviceCallEntity.cs_servicecall_header.Rows[0]["Docstatus"].ToString();
                return Docstatus == lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_ROLL_BACK_BUSINESS_CHANGE);
            }
        }

        #region - dt -
        //DataTable dtEmpTemp
        //{
        //    get { return Session["SCT_dtEmpTemp" + idGen] == null ? null : (DataTable)Session["SCT_dtEmpTemp" + idGen]; }
        //    set { Session["SCT_dtEmpTemp" + idGen] = value; }
        //}

        //DataTable dtEmpCodedtEmpCode
        //{
        //    get { return Session["SCT_dtEmpCode" + idGen] == null ? null : (DataTable)Session["SCT_dtEmpCode" + idGen]; }
        //    set { Session["SCT_dtEmpCode" + idGen] = value; }
        //}

        //DataTable dtGrouTemp
        //{
        //    get { return Session["SCT_dtGrouTemp" + idGen] == null ? null : (DataTable)Session["SCT_dtGrouTemp" + idGen]; }
        //    set { Session["SCT_dtGrouTemp" + idGen] = value; }
        //}

        DataTable _dtGrouTemp;
        DataTable dtGrouTemp
        {
            get
            {
                if (_dtGrouTemp == null)
                {
                    _dtGrouTemp = AfterSaleService.getInstance().GetDTProblemGroup(SID, businessObject);
                }
                return _dtGrouTemp;
            }
        }

        //DataTable dtSCType
        //{
        //    get { return Session["SCT_SCType" + idGen] == null ? null : (DataTable)Session["SCT_SCType" + idGen]; }
        //    set { Session["SCT_SCType" + idGen] = value; }
        //}

        DataTable _dtSCType;
        DataTable dtSCType
        {
            get
            {
                if (_dtSCType == null)
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

                    _dtSCType = AfterSaleService.getInstance().getSearchDoctype(
                        "",
                        CompanyCode,
                        listBusinessObject
                    );
                }

                return _dtSCType;
            }
        }

        //DataTable dtCustomer
        //{
        //    get { return Session["SCT_dtCustomer" + idGen] == null ? null : (DataTable)Session["SCT_dtCustomer" + idGen]; }
        //    set { Session["SCT_dtCustomer" + idGen] = value; }
        //}

        DataTable _dtCustomer;
        DataTable dtCustomer
        {
            get
            {
                if (_dtCustomer == null)
                {
                    _dtCustomer = AfterSaleService.getInstance().getSearchCustomerCode("", CompanyCode);
                }
                return _dtCustomer;
            }
        }

        //DataTable dtDocstatus
        //{
        //    get { return Session["SCT_dtDocstatus" + idGen] == null ? null : (DataTable)Session["SCT_dtDocstatus" + idGen]; }
        //    set { Session["SCT_dtDocstatus" + idGen] = value; }
        //}

        DataTable _dtDocstatus;
        DataTable dtDocstatus
        {
            get
            {
                if (_dtDocstatus == null)
                {
                    _dtDocstatus = AfterSaleService.getInstance().getCallStatus("");
                }
                return _dtDocstatus;
            }
        }

        //DataTable dtTicketDocStatus
        //{
        //    get { return Session["SCT_dtTicketDocStatus" + idGen] == null ? null : (DataTable)Session["SCT_dtTicketDocStatus" + idGen]; }
        //    set { Session["SCT_dtTicketDocStatus" + idGen] = value; }
        //}

        DataTable _dtTicketDocStatus;
        DataTable dtTicketDocStatus
        {
            get
            {

                if (_dtTicketDocStatus == null)
                {
                    _dtTicketDocStatus = lib.GetTicketDocStatus(SID, CompanyCode, true);
                }
                return _dtTicketDocStatus;
            }
        }

        //DataTable dtContactPerson
        //{
        //    get { return Session["SCT_dtContactPerson" + idGen] == null ? null : (DataTable)Session["SCT_dtContactPerson" + idGen]; }
        //    set { Session["SCT_dtContactPerson" + idGen] = value; }
        //}

        DataTable _dtContactPerson;
        DataTable dtContactPerson
        {
            get
            {
                if (_dtContactPerson == null)
                {
                    string custcode = CustomerCode_session == null ? "" : CustomerCode_session;
                    _dtContactPerson = AfterSaleService.getInstance().getContactPerson(CompanyCode, custcode);
                }

                return _dtContactPerson;
            }
        }

        //DataTable dtContactPhone
        //{
        //    get { return Session["SCT_dtContactPhone" + idGen] == null ? null : (DataTable)Session["SCT_dtContactPhone" + idGen]; }
        //    set { Session["SCT_dtContactPhone" + idGen] = value; }
        //}

        //DataTable dtContactEmail
        //{
        //    get { return Session["SCT_dtContactEmail" + idGen] == null ? null : (DataTable)Session["SCT_dtContactEmail" + idGen]; }
        //    set { Session["SCT_dtContactEmail" + idGen] = value; }
        //}

        //DataTable dtContactAddress
        //{
        //    get { return Session["SCT_dtContactAddress" + idGen] == null ? null : (DataTable)Session["SCT_dtContactAddress" + idGen]; }
        //    set { Session["SCT_dtContactAddress" + idGen] = value; }
        //}

        //DataTable dtProjectCode
        //{
        //    get { return Session["SCT_dtProjectCode" + idGen] == null ? null : (DataTable)Session["SCT_dtProjectCode" + idGen]; }
        //    set { Session["SCT_dtProjectCode" + idGen] = value; }
        //}

        DataTable _dtProjectCode;
        DataTable dtProjectCode
        {
            get
            {
                if (_dtProjectCode == null)
                {
                    _dtProjectCode = AfterSaleService.getInstance().getProjectCode(SID, WorkGroupCode, CustomerCode_session == null ? "" : CustomerCode_session.ToString());
                }
                return _dtProjectCode;
            }
        }

        //DataTable dtProjectElement
        //{
        //    get { return Session["SCT_dtProjectElement" + idGen] == null ? null : (DataTable)Session["SCT_dtProjectElement" + idGen]; }
        //    set { Session["SCT_dtProjectElement" + idGen] = value; }
        //}

        DataTable _dtProjectElement;
        DataTable dtProjectElement
        {
            get
            {
                if (_dtProjectElement == null)
                {
                    string projectcode = _ddl_projectcode.SelectedIndex != -1 ? _ddl_projectcode.SelectedValue : "";
                    _dtProjectElement = AfterSaleService.getInstance().getProjectElement(SID, CompanyCode, projectcode);
                }

                return _dtProjectElement;
            }
        }

        //DataTable dtEquipment
        //{
        //    get { return Session["SCT_dtEquipment" + idGen] == null ? null : (DataTable)Session["SCT_dtEquipment" + idGen]; }
        //    set { Session["SCT_dtEquipment" + idGen] = value; }
        //}

        //DataTable dtProblemTypeTemp
        //{
        //    get { return Session["SCT_dtProblemTypeTemp" + idGen] == null ? null : (DataTable)Session["SCT_dtProblemTypeTemp" + idGen]; }
        //    set { Session["SCT_dtProblemTypeTemp" + idGen] = value; }
        //}

        //DataTable dtGroup
        //{
        //    get { return Session["SCT_dtGroup" + idGen] == null ? null : (DataTable)Session["SCT_dtGroup" + idGen]; }
        //    set { Session["SCT_dtGroup" + idGen] = value; }
        //}

        DataTable dtTier
        {
            get { return Session["SCT_dtTier" + idGen] == null ? null : (DataTable)Session["SCT_dtTier" + idGen]; }
            set { Session["SCT_dtTier" + idGen] = value; }
        }
        #endregion

        //public string EquipmentSelect
        //{
        //    get { return Session["EquipmentSelect_saleservice" + idGen] == null ? serviceCallEntity.cs_servicecall_item.Rows[0]["EquipmentNo"].ToString() : (string)Session["EquipmentSelect_saleservice" + idGen]; }
        //    set { Session["EquipmentSelect_saleservice" + idGen] = value; }
        //}

        public string _EquipmentSelect;
        public string EquipmentSelect
        {
            get
            {
                if (_EquipmentSelect == null)
                {
                    _EquipmentSelect = Session["EquipmentSelect_saleservice" + idGen] == null
                        ? serviceCallEntity.cs_servicecall_item.Rows[0]["EquipmentNo"].ToString()
                        : (string)Session["EquipmentSelect_saleservice" + idGen];
                }
                return _EquipmentSelect;
            }
            set
            {
                Session["EquipmentSelect_saleservice" + idGen] = value;
                _EquipmentSelect = value;
            }
        }

        //public string EquipmentItemNo
        //{
        //    get { return Session["EquipmentItemNo_saleservice" + idGen] == null ? serviceCallEntity.cs_servicecall_item.Rows[0]["xLineNo"].ToString() : (string)Session["EquipmentItemNo_saleservice" + idGen]; }
        //    set { Session["EquipmentItemNo_saleservice" + idGen] = value; }
        //}

        public string _EquipmentItemNo;
        public string EquipmentItemNo
        {
            get
            {
                if (_EquipmentItemNo == null)
                {
                    _EquipmentItemNo = Session["EquipmentItemNo_saleservice" + idGen] == null
                        ? serviceCallEntity.cs_servicecall_item.Rows[0]["xLineNo"].ToString()
                        : (string)Session["EquipmentItemNo_saleservice" + idGen];
                }
                return _EquipmentItemNo;
            }
            set
            {
                Session["EquipmentItemNo_saleservice" + idGen] = value;
                _EquipmentItemNo = value;
            }
        }

        public bool ShowWorkFlow
        {
            get { return Convert.ToBoolean(Session["ServiceCallTransaction_ShowWorkFlow" + idGen]); }
            set { Session["ServiceCallTransaction_ShowWorkFlow" + idGen] = value; }
        }

        public bool ShowWorkFlowWithoutCreate
        {
            get { return Convert.ToBoolean(Session["ServiceCallTransaction_ShowWorkFlowWithoutCreate" + idGen]); }
            set { Session["ServiceCallTransaction_ShowWorkFlowWithoutCreate" + idGen] = value; }
        }

        public bool AlowWorkFlow
        {
            get { return (bool)Session["ServiceCallTransaction_AlowWorkFlow" + idGen]; }
            set { Session["ServiceCallTransaction_AlowWorkFlow" + idGen] = value; }
        }

        public string _CustomerCode_session;
        public string CustomerCode_session
        {
            get
            {
                if (_CustomerCode_session == null)
                {
                    _CustomerCode_session = (string)Session["SCT_created_cust_code" + idGen];
                }
                return _CustomerCode_session;
            }
            set
            {
                Session["SCT_created_cust_code" + idGen] = value;
                _CustomerCode_session = value;
            }
        }

        public string _IDCurentTabView;
        public string IDCurentTabView
        {
            get
            {
                if (_IDCurentTabView == null)
                {
                    _IDCurentTabView = (string)Session["SCT_IDCurentTabView" + idGen];
                }
                return _IDCurentTabView;
            }
            set
            {
                Session["SCT_IDCurentTabView" + idGen] = value;
                _IDCurentTabView = value;
            }
        }

        #region Function On Page
        protected void Page_Init(object sender, EventArgs e)
        {
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("start Page_Load");
            Stopwatch sw = Stopwatch.StartNew();

            if (!IsPostBack)
            {
                hddIDCurentTabView.Value = IDCurentTabView;

                if (mode_stage != ApplicationSession.CREATE_MODE_STRING)
                { 
                    try
                    {
                        serviceCallEntity.cs_servicecall_header.First().CAB =
                            ServiceTicketLibrary.LookUpTable(
                                "CAB",
                                "cs_servicecall_header",
                                @"where SID = '" + serviceCallEntity.cs_servicecall_header.First().SID + @"' 
                                            AND CompanyCode = '" + serviceCallEntity.cs_servicecall_header.First().CompanyCode + @"'
                                            AND Doctype = '" + serviceCallEntity.cs_servicecall_header.First().Doctype + @"'
                                            AND Fiscalyear = '" + serviceCallEntity.cs_servicecall_header.First().Fiscalyear + @"' 
                                            AND CallerID = '" + serviceCallEntity.cs_servicecall_header.First().CallerID + @"'"
                            );
                        serviceCallEntity.cs_servicecall_header.AcceptChanges();
                    }
                    catch (Exception)
                    {

                    }
                }

                initData();

                if (mode_stage == ApplicationSession.CREATE_MODE_STRING)
                {
                    initData_CreateMode();
                }
                else if (mode_stage == ApplicationSession.CHANGE_MODE_STRING || mode_stage == ApplicationSession.DISPLAY_MODE_STRING)
                {
                    initData_ChangeMode();
                }

                initData_AfterInitMode();

            }
            createHeaderEffectedCustomerDataSet();
            if (Session["SCT_created_doctype_code" + idGen] == null)
            {
                ClientService.OpenSessionTimedOutFade();
            }
            sw.Stop();
            Debug.WriteLine("Total Time taken: {0}ms", sw.Elapsed.TotalMilliseconds);

            countMenu();
            compareDate();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (mode_stage == ApplicationSession.CREATE_MODE_STRING)
            {
                hddPageTicketMode.Value = "Create";
                galleryLoad.CssClass = "";
            }
            else if (mode_stage == ApplicationSession.DISPLAY_MODE_STRING)
            {
                hddPageTicketMode.Value = "Display";
            }
            else if (mode_stage == ApplicationSession.CHANGE_MODE_STRING)
            {
                hddPageTicketMode.Value = "Change";
            }

            string docStatus = "";
            foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
            {
                docStatus = dr["Docstatus"].ToString();
            }

            string SERVICE_DOC_STATUS_RESOLVE = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE_BUSINESS_CHANGE);
            string SERVICE_DOC_STATUS_CLOSED = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CLOSED_BUSINESS_CHANGE);
            string SERVICE_DOC_STATUS_CANCEL = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CANCEL_BUSINESS_CHANGE);

            Boolean IsFirstView = true;
            if (mode_stage == ApplicationSession.CHANGE_MODE_STRING || mode_stage == ApplicationSession.DISPLAY_MODE_STRING)
            {
                AnalyticsEmployee enAnalytics = AnalyticsService.getTackingPageTicketIsAuthenEdit(
                    SID,
                    CompanyCode,
                    hddDocnumberTran.Value,
                    EmployeeCode
                );
                IsFirstView = enAnalytics.IsAuthenEdit && mode_stage == ApplicationSession.CHANGE_MODE_STRING;

                if (!IsFirstView && mode_stage == ApplicationSession.CHANGE_MODE_STRING)
                {
                    Session["SCF_MSGERROR" + idGen] = "Change Mode Lock By " + enAnalytics.firstEmployeeName;
                    btnRequestDisplayMode_Click(null, null);
                }

                hddEmployeeFirstView.Value = enAnalytics.firstEmployeeName;
                udpHiddenCode.Update();
            }

            hddAuthenEdit.Value = IsFirstView.ToString();
            if (mode_stage == ApplicationSession.CHANGE_MODE_STRING ||
                mode_stage == ApplicationSession.DISPLAY_MODE_STRING ||
                countActivity > 0 ||
                docStatus == SERVICE_DOC_STATUS_CLOSED ||
                docStatus == SERVICE_DOC_STATUS_CANCEL ||
                !IsFirstView)
            {
                if (mode_stage == ApplicationSession.CHANGE_MODE_STRING)
                {
                    ClientService.DoJavascript("controlInputEnable();");
                }
                else
                {
                    ClientService.DoJavascript("controlInputDisable();");
                }
            }
            else
            {
                ClientService.DoJavascript("controlInputEnable();");
            }

            try
            {
                if (mode_stage == ApplicationSession.CHANGE_MODE_STRING)
                {
                    ClientService.DoJavascript("ddlTicketStatus_Change();");
                }

                //ddlTicketStatus_Temp.Items[0].Attributes.Add("hidden", "hidden");
                //ddlTicketStatus_Temp.Items.FindByValue(SERVICE_DOC_STATUS_RESOLVE).Attributes.Add("hidden", "hidden");
                ddlTicketStatus_Temp.Items.FindByValue(SERVICE_DOC_STATUS_CLOSED).Attributes.Add("hidden", "hidden");
                ddlTicketStatus_Temp.Items.FindByValue(SERVICE_DOC_STATUS_CANCEL).Attributes.Add("hidden", "hidden");
                udpHiddenCode.Update();
            }
            catch (Exception)
            {

            }
        }
        #endregion

        private void GetAreaCodeFromDocumentType(string ticketType)
        {
            AreaCode = lib.GetAreaCodeFromTicketType(SID, ticketType);
        }

        private void bindDataPopupCreateNewTicketReferent()
        {
            string _doctype = "", _Customer = "";

            foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
            {
                _doctype = Convert.ToString(dr["DocType"]);
                _Customer = Convert.ToString(dr["CustomerCode"]);
            }

            List<ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource> result = new List<ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource>();

            DataTable dt = universalService.GetEquipmentCustomerAssignment(SID, CompanyCode, "", _Customer);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                string customerName = lib.PrepareNameAndForiegnName(dr["CustomerName"].ToString(), dr["ForeignName"].ToString());

                result.Add(new ServiceWeb.API.AutoCompleteAPI.AutoCompleteSource
                {
                    code = dr["OwnerCode"].ToString(),
                    desc = customerName,
                    display = customerName == "" ? dr["OwnerCode"].ToString() : dr["OwnerCode"] + " : " + customerName
                });

                string defaultValue = result[0].display;

                string responseJson = JsonConvert.SerializeObject(result);

                ClientService.DoJavascript("bindAutoCompleteCustomer(" + responseJson + ", '" + defaultValue + "', true);");
            }

            GC.Collect();

            _txt_year.Value = Validation.getCurrentServerStringDateTime().Substring(0, 4);

            _ddl_sctype.Items.Clear();
            _ddl_sctype.Items.Add(new ListItem("", ""));
            _ddl_sctype.AppendDataBoundItems = true;
            _ddl_sctype.DataTextField = "Description";
            _ddl_sctype.DataValueField = "DocumentTypeCode";
            _ddl_sctype.DataSource = dtSCType;
            _ddl_sctype.DataBind();

            try
            {
                string businessObject = lib.GetBusinessObjectFromTicketType(SID, _doctype);

                _ddl_sctype.SelectedValue = lib.getReferenceConfigTicketType(SID, businessObject);
            }
            catch (Exception)
            {
                _ddl_sctype.SelectedIndex = 0;
            }
        }

        private void CreatedNewTicket(string equipmentCode, bool isReference)
        {
            string idGen = Guid.NewGuid().ToString().Substring(0, 8);

            string docType = "";
            string docTypeDesc = "";
            string customerCode = "";
            string customerName = "";
            string fiscalYear = "";

            string oldDocType = "";
            string oldFiscalYear = "";

            if (serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
            {
                oldDocType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
                oldFiscalYear = serviceCallEntity.cs_servicecall_header.Rows[0]["FiscalYear"].ToString();

                docType = _ddl_sctype.SelectedValue;
                docTypeDesc = GetSCTypeDesc(docType);
                customerCode = CustomerSelect.SelectedValue;
                customerName = GetCustomerDesc(customerCode);
                fiscalYear = _txt_year.Value;
            }

            Session["SCT_created_ticket_ref_code" + idGen] = hddDocnumberTran.Value;

            Session["SCT_created_doctype_code" + idGen] = docType;
            Session["SCT_created_doctype_desc" + idGen] = docTypeDesc;
            Session["SCT_created_cust_code" + idGen] = customerCode;
            Session["SCT_created_cust_desc" + idGen] = customerName;
            Session["SCT_created_contact_code" + idGen] = "";
            Session["SCT_created_contact_desc" + idGen] = "";
            Session["SCT_created_fiscalyear" + idGen] = fiscalYear;
            Session["SCT_created_equipment" + idGen] = equipmentCode;
            Session["SCT_created_remark" + idGen] = null;
            Session["SCT_created_impact" + idGen] = null;
            Session["SCT_created_urgency" + idGen] = null;
            Session["SCT_created_priority" + idGen] = null;

            if (isReference)
            {
                Session["SCT_created_ref" + idGen] = true;
                Session["SCT_created_ref_doctype" + idGen] = oldDocType;
                Session["SCT_created_ref_fiscalyear" + idGen] = oldFiscalYear;
            }

            //serviceCallEntity = new tmpServiceCallDataSet();
            Session["ServicecallEntity" + idGen] = new tmpServiceCallDataSet();

            //mode_stage = ApplicationSession.CREATE_MODE_STRING;
            Session["SC_MODE" + idGen] = ApplicationSession.CREATE_MODE_STRING;

            string PageRedirect = lib.getPageTicketRedirect(
                SID,
                docType/*serviceCallEntity.cs_servicecall_header.Rows[0]["Doctype"].ToString()*/
            );
            Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen));
        }

        #region initial Data
        private void initData()
        {
            ShowWorkFlow = false;
            ShowWorkFlowWithoutCreate = false;
            AlowWorkFlow = false;
            countActivity = 0;

            Session["SCT_GROUPCODE" + idGen] = null;
            clearScreen();
            initDataTemp();
            //GetContact();
            GetProject();
            GetImpact();
            GetUrgency();
            GetTicketDocStatus();

            //AutoCompleteEmployee.AfterSelectedChange = "bindDataTableInEmployeeParticipantSelected(v, 'TransferParticipantChangeOrder', '" + AutoCompleteEmployee.ClientID + "');";
            AutoCompleteEmployee.AfterSelectedChange = "bindDataTableInEmployeeParticipantSelected(v, 'OwnerParticipant', '" + AutoCompleteEmployee.ClientID + "');";
            AutoCompleteEmployee_Transfer.AfterSelectedChange = "bindDataTableInEmployeeParticipantSelected(v, 'TransferParticipantChangeOrder', '" + AutoCompleteEmployee_Transfer.ClientID + "');";
        }

        private void initData_CreateMode()
        {
            panelFeedActivityComment.Visible = false;
            serviceCallEntity = new tmpServiceCallDataSet();

            initCreated();
            bindDataAccountability();
            if (Session["SCT_created_accountability" + idGen] != null)
            {


                //ddlAccountability.ClearSelection();
                //ddlAccountability.Items.FindByValue(Session["SCT_created_accountability" + idGen].ToString()).Selected = true;
                ddlAccountability.SelectedValue = Session["SCT_created_accountability" + idGen].ToString();
            }
            if (Session["SCT_created_problemgroup" + idGen] != null)
            {
                ddlProblemGroup.SelectedValue = Session["SCT_created_problemgroup" + idGen].ToString();
            }
            if (Session["SCT_created_problemtype" + idGen] != null)
            {
                ddlProblemType.SelectedValue = Session["SCT_created_problemtype" + idGen].ToString();
            }
            if (Session["SCT_created_problemsource" + idGen] != null)
            {
                ddlProblemSource.SelectedValue = Session["SCT_created_problemsource" + idGen].ToString();
            }
            if (Session["SCT_created_servicesource" + idGen] != null)
            {
                ddlServiceSource.SelectedValue = Session["SCT_created_servicesource" + idGen].ToString();
            }
            if (Session["SCT_created_proplemtopic" + idGen] != null)
            {
                txt_problem_topic.Text = Session["SCT_created_proplemtopic" + idGen].ToString();
            }
            if (Session["SCT_created_equipmentremark" + idGen] != null)
            {
                tbEquipmentRemark.Text = Session["SCT_created_equipmentremark" + idGen].ToString();
            }

            if (Session["SCT_created_ownergroupservice" + idGen] != null)
            {
                ddlOwnerGroupService.ClearSelection();
                ddlOwnerGroupService.Items.FindByValue(Session["SCT_created_ownergroupservice" + idGen].ToString()).Selected = true;
                ddlOwnerGroupService_SelectedIndexChanged(null, null);
                //ddlOwnerGroupService.SelectedValue = ;
            }


            if (Session["SCT_created_planstartdate" + idGen] != null)
            {
                tbStartDate.Text = Session["SCT_created_planstartdate" + idGen].ToString();
            }
            if (Session["SCT_created_planenddate" + idGen] != null)
            {
                tbEndDate.Text = Session["SCT_created_planenddate" + idGen].ToString();
            }
            if (Session["SCT_created_planstarttime" + idGen] != null)
            {
                txtStartTime.Text = Session["SCT_created_planstarttime" + idGen].ToString();
            }
            if (Session["SCT_created_planendtime" + idGen] != null)
            {
                txtEndTime.Text = Session["SCT_created_planendtime" + idGen].ToString();
            }



            if (Session["SCT_created_equipment_List" + idGen] != null && (Session["SCT_created_equipment_List" + idGen] as List<string>).Count > 0)
            {
                List<string> listCI = Session["SCT_created_equipment_List" + idGen] as List<string>;
                listCI.ForEach(r =>
                {
                    addEquipment(r);
                });
                bindDataCI();
            }
            else if (Session["SCT_created_equipment" + idGen] != null && Session["SCT_created_equipment" + idGen].ToString() != "")
            {
                addEquipment(Session["SCT_created_equipment" + idGen].ToString());
                bindDataCI();
            }

            if (Session["SCT_created_effectCus_List" + idGen] != null && (Session["SCT_created_effectCus_List" + idGen] as List<string>).Count > 0)
            {
                createHeaderEffectedCustomerDataSet();
                List<string> listEffectCus = Session["SCT_created_effectCus_List" + idGen] as List<string>;
                listEffectCus.ForEach(r =>
                {
                    addEffectedCustomer(r);
                });
                bindCustomer();
            }


        }

        private void initData_ChangeMode()
        {
            panelFeedActivityComment.Visible = true;
            GetEquipment_V2();
            GetProblemGroup();
            GetOwnerGroupService();
            GetReferDocument();
            mapdataToscreen();
            controlscreen();
            BindDataOwnerOperation();
            bindDataPopupCreateNewTicketReferent();
            bindTicketRefDoc();
            getDataRelation();
            bindCustomerChangeMode();

            #region Workflow
            bindDataAccountability();
            BindDataWorkflow();
            #endregion

            #region Knowledge Management
            if (chkIsLoad_Knowledge.Checked)
                bindKnowledgeIDRefTicketNO();
            //bindDefaultSearchAddKnowledge();
            #endregion
        }

        private void initData_AfterInitMode()
        {

            if (Session["SCF_SAVESUCCESS" + idGen] != null)
            {
                string message = Session["SCF_SAVESUCCESS" + idGen].ToString();
                Session["SCF_SAVESUCCESS" + idGen] = null;
                ClientService.AGSuccess(message);
            }
            if (Session["SCF_MSGERROR" + idGen] != null)
            {
                string message = Session["SCF_MSGERROR" + idGen].ToString();
                Session["SCF_MSGERROR" + idGen] = null;
                ClientService.AGError(message);
            }

            if (chkIsLoad_TicketChangeLog.Checked)
                bindDataChangeLog();
            if (chkIsLoad_Attachment.Checked)
                BindDataLogFileAttachment();
        }

        void initDataTemp()
        {
            string _doctype = "";

            foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
            {
                _doctype = Convert.ToString(dr["DocType"]);
            }
            //string businessObject = lib.GetBusinessObjectFromTicketType(SID, _doctype);
            //mBusinessObject = businessObject;
            //List<string> listBusinessObject = new List<string>();
            //if (Permission.IncidentView)
            //{
            //    listBusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_INCIDENT);
            //}
            //if (Permission.ProblemView)
            //{
            //    listBusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_PROBLEM);
            //}
            //if (Permission.RequestView)
            //{
            //    listBusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_REQUEST);
            //}
            //if (Permission.ChangeOrderView)
            //{
            //    listBusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_CHANGE);
            //}
            //dtSCType = AfterSaleService.getInstance().getSearchDoctype(
            //         "",
            //         CompanyCode,
            //         listBusinessObject
            //     );
            // dtSCType = AfterSaleService.getInstance().getSearchDoctype("", CompanyCode);
            //dtCustomer = AfterSaleService.getInstance().getSearchCustomerCode("", CompanyCode);
            //dtGrouTemp = AfterSaleService.getInstance().GetDTProblemGroup(SID, businessObject);
            //dtDocstatus = AfterSaleService.getInstance().getCallStatus("");
            //dtTicketDocStatus = lib.GetTicketDocStatus(SID, CompanyCode, true);
        }

        private void initTextLabel(string TicketType)
        {
            string businessObject = lib.GetBusinessObjectFromTicketType(SID, TicketType);

            lblEquipmentProblemGroupDesc.Text = ServiceTicketLibrary.GetBusinessObjectDesc(businessObject) + " Group";
            lblEquipmentProblemTypeDesc.Text = ServiceTicketLibrary.GetBusinessObjectDesc(businessObject) + " Type";
            lblEquipmentProblemSourceDesc.Text = ServiceTicketLibrary.GetBusinessObjectDesc(businessObject) + " Source";

        }

        protected void clearScreen()
        {
            //_ddl_contact_person.SetValue = "";
            _ddl_contact_phone_noTran.SelectedIndex = 0;
            _ddl_contact_emailTran.SelectedIndex = 0;
            _ddl_contact_addressTran.SelectedIndex = 0;
            _ddl_projectcode.SelectedIndex = 0;
            _ddl_project_elementTran.SelectedIndex = 0;
            txt_problem_topic.Text = "";

            ddlAccountability.SelectedIndex = 0;
            hddIncidentAreaCode.Value = "";
            //tbCallbackDate.Text = "";
            //tbCallbackTime.Text = "";
            //update
            tbStartDate.Text = "";
            tbEndDate.Text = "";
            //update
            tbRefer.Text = "";
            tbEquipmentRemark.Text = "";
        }
        #endregion

        #region Save
        protected void btnConfirmTran_click(object sender, EventArgs e)
        {
            try
            {
                if (mode_stage == ApplicationSession.CREATE_MODE_STRING)
                {
                    CreateServiceCall();
                }
                else
                {
                    List<string> validationRS = validateForm();

                    if (validationRS.Count > 0)
                    {
                        string validationSTR = string.Join("<br/>", validationRS.ToArray());
                        throw new Exception(validationSTR);
                    }

                    mapScreenItemToEntityUpdate();
                    PrepareEquipment();

                    string sessionid = (string)Session[ApplicationSession.USER_SESSION_ID];
                    Object[] objParam = new Object[] { "1500141", sessionid };

                    DataSet[] objDataSet = new DataSet[] { serviceCallEntity };
                    DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
                    if (objReturn != null && objReturn.Tables.Count > 0)
                    {
                        string returnCode = "";
                        string returnMessage = "";
                        returnCode = (objReturn.Tables["MessageResult"] == null || objReturn.Tables["MessageResult"].Rows.Count <= 0)
                                      ? "E" : objReturn.Tables["MessageResult"].Rows[0]["code"].ToString();
                        Session["SC_Callerid" + idGen] = returnMessage = (objReturn.Tables["MessageResult"] == null || objReturn.Tables["MessageResult"].Rows.Count <= 0)
                            ? "" : objReturn.Tables["MessageResult"].Rows[0]["Message"].ToString();

                        if ("E".Equals(returnCode))
                        {
                            string message = "Save Error : " + returnMessage;

                            throw new Exception(message);
                        }
                        else
                        {
                            lib.updateTicketRefDoc(
                                SID,
                                CompanyCode,
                                hddDocnumberTran.Value,
                                txtRef_SO.Text,
                                txtRef_QT.Text,
                                txtRef_PO.Text,
                                EmployeeCode
                            );


                            List<logValue_OldNew> enLog = new List<logValue_OldNew>();
                            if (hddOldValue_problem_topic.Value != txt_problem_topic.Text)
                            {
                                enLog.Add(new logValue_OldNew
                                {
                                    Value_Old = hddOldValue_problem_topic.Value,
                                    Value_New = txt_problem_topic.Text,
                                    TableName = "cs_servicecall_header",
                                    FieldName = "Subject",
                                    AccessCode = LogServiceLibrary.AccessCode_Change
                                });
                            }

                            if (hddOldValue_EquipmentRemark.Value != tbEquipmentRemark.Text)
                            {
                                enLog.Add(new logValue_OldNew
                                {
                                    Value_Old = hddOldValue_EquipmentRemark.Value,
                                    Value_New = tbEquipmentRemark.Text,
                                    TableName = "cs_servicecall_item",
                                    FieldName = "Description",
                                    AccessCode = LogServiceLibrary.AccessCode_Change
                                });
                            }
                            SaveLog(enLog);

                            //AttachFileUserControl.DocType = (string)Session["SCT_created_doctype_code" + idGen];
                            //AttachFileUserControl.DocYear = (string)Session["SCT_created_fiscalyear" + idGen];
                            //AttachFileUserControl.DocNumber = hddDocnumberTran.Value.Trim();
                            //AttachFileUserControl.Commit(hddDocnumberTran.Value.Trim());
                            serviceCallEntity = new tmpServiceCallDataSet();

                            Session["SCF_SAVESUCCESS" + idGen] = "Update Success Document Number : " + _txt_docnumberTran.Value;
                            getdataToedit((string)Session["SCT_created_doctype_code" + idGen], hddDocnumberTran.Value.Trim(), (string)Session["SCT_created_fiscalyear" + idGen]);
                        }
                    }
                    else
                    {
                        lib.updateTicketRefDoc(
                            SID,
                            CompanyCode,
                            hddDocnumberTran.Value,
                            txtRef_SO.Text,
                            txtRef_QT.Text,
                            txtRef_PO.Text,
                            EmployeeCode
                        );


                        List<logValue_OldNew> enLog = new List<logValue_OldNew>();
                        if (hddOldValue_problem_topic.Value != txt_problem_topic.Text)
                        {
                            enLog.Add(new logValue_OldNew
                            {
                                Value_Old = hddOldValue_problem_topic.Value,
                                Value_New = txt_problem_topic.Text,
                                TableName = "cs_servicecall_header",
                                FieldName = "Subject",
                                AccessCode = LogServiceLibrary.AccessCode_Change
                            });
                        }

                        if (hddTicketStatus_Old.Value != ddlTicketStatus_Change.Text)
                        {
                            var i = 0;
                            foreach (ListItem li in ddlTicketStatus_Change.Items)
                            {
                                if (li.Value == hddTicketStatus_Old.Value)
                                {
                                    break;
                                }
                                i++;
                            }
                            enLog.Add(new logValue_OldNew
                            {
                                Value_Old = ddlTicketStatus_Change.Items[i].Text,
                                Value_New = ddlTicketStatus_Change.Items[ddlTicketStatus_Change.SelectedIndex].Text,
                                TableName = "cs_servicecall_header",
                                FieldName = "Ticket Status",
                                AccessCode = LogServiceLibrary.AccessCode_Updata_Status
                            });
                        }

                        if (hddOldValue_EquipmentRemark.Value != tbEquipmentRemark.Text)
                        {
                            enLog.Add(new logValue_OldNew
                            {
                                Value_Old = hddOldValue_EquipmentRemark.Value,
                                Value_New = tbEquipmentRemark.Text,
                                TableName = "cs_servicecall_item",
                                FieldName = "Description",
                                AccessCode = LogServiceLibrary.AccessCode_Change
                            });
                        }
                        SaveLog(enLog);

                        //AttachFileUserControl.DocType = (string)Session["SCT_created_doctype_code" + idGen];
                        //AttachFileUserControl.DocYear = (string)Session["SCT_created_fiscalyear" + idGen];
                        //AttachFileUserControl.DocNumber = hddDocnumberTran.Value.Trim();
                        //AttachFileUserControl.Commit(hddDocnumberTran.Value.Trim());
                        serviceCallEntity = new tmpServiceCallDataSet();
                        Session["SCF_SAVESUCCESS" + idGen] = "Update Success Document Number : " + _txt_docnumberTran.Value;
                        getdataToedit((string)Session["SCT_created_doctype_code" + idGen], hddDocnumberTran.Value.Trim(), (string)Session["SCT_created_fiscalyear" + idGen]);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientService.AGLoading(false);
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {

            }
        }

        protected void CreateServiceCall()
        {
            try
            {
                List<string> validationRS = validateForm();

                if (validationRS.Count > 0)
                {
                    string validationSTR = string.Join("<br/>", validationRS.ToArray());
                    throw new Exception(validationSTR);
                }

                getDataSave();

                PrepareEquipment();

                DataSet objReturn = new DataSet();
                string returnCode = "";
                string returnMessage = "";
                addInitData();

                string sessionid = (string)Session[ApplicationSession.USER_SESSION_ID];
                Object[] objParam = new Object[] { "1500138", sessionid };


                DataSet[] objDataSet = new DataSet[] { serviceCallEntity };
                objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
                if (objReturn != null && objReturn.Tables.Count > 0)
                {
                    returnCode = (objReturn.Tables["MessageResult"] == null || objReturn.Tables["MessageResult"].Rows.Count <= 0)
                                      ? "E" : objReturn.Tables["MessageResult"].Rows[0]["code"].ToString();
                    Session["SC_Callerid" + idGen] = returnMessage = (objReturn.Tables["MessageResult"] == null || objReturn.Tables["MessageResult"].Rows.Count <= 0)
                        ? "" : objReturn.Tables["MessageResult"].Rows[0]["Message"].ToString();

                    if ("E".Equals(returnCode))
                    {
                        string message = "Save Error : " + returnMessage;

                        throw new Exception(message);
                    }
                    else
                    {
                        Session["SCT_created_equipment" + idGen] = null;

                        //clearScreen();
                        //AttachFileUserControl.DocType = (string)Session["SCT_created_doctype_code" + idGen];
                        //AttachFileUserControl.DocYear = (string)Session["SCT_created_fiscalyear" + idGen];
                        //AttachFileUserControl.DocNumber = returnMessage.Trim();
                        //AttachFileUserControl.UpdateHeaderOnCommit = true;
                        //AttachFileUserControl.Commit(returnMessage.Trim());
                        serviceCallEntity = new tmpServiceCallDataSet();

                        string ticketType = (string)Session["SCT_created_doctype_code" + idGen];
                        string ticketNo = returnMessage.Trim();
                        string ticketYear = (string)Session["SCT_created_fiscalyear" + idGen];
                        string remark = txt_problem_topic.Text;

                        string displayDocNumber = AfterSaleService.getInstance().GetTicketNoForDisplay(
                            SID, CompanyCode, ticketType, returnMessage
                        );
                        List<ContactList> listCon = getContactSelect();
                        ServiceTicketLibrary libticket = new ServiceTicketLibrary();
                        prepareEffectedCustomer();
                        libticket.addEffectedCustomer(SID, CompanyCode, EmployeeCode, ticketNo, servicecallCustomer);
                        setContactList();
                        setDetailContact(listCon);
                        if (Session["ModeReset" + idGen] != null)
                        {
                            try
                            {
                                string canceldate = Validation.getCurrentServerStringDateTime().Substring(0, 8);
                                string canceltime = Validation.getCurrentServerDateTime().ToString("HHmmss");
                                string cancelby = EmployeeCode;

                                string ticketTypeCancel = Session["ticketType" + idGen].ToString();
                                string ticketNoCancel = Session["ticketNo" + idGen].ToString();
                                string ticketYearCancel = Session["ticketYear" + idGen].ToString();

                                AfterSaleService.getInstance().saveLogCanceledTicket(SID, CompanyCode,
                                     ticketTypeCancel, ticketNoCancel, ticketYearCancel, EmployeeCode, FullNameEN
                                );

                                Object[] objParam_Old = new Object[] { "1500117",
                                    (string)Session[ApplicationSession.USER_SESSION_ID],
                                    CompanyCode, ticketTypeCancel, ticketNoCancel, ticketYearCancel
                                };
                                DataSet[] objDataSet_Old = new DataSet[] { serviceCallEntity };
                                DataSet objReturn_Old = ICMService.ICMDataSetInvoke(objParam_Old, objDataSet_Old);

                                if (objReturn_Old != null)
                                {
                                    tmpServiceCallDataSet serviceCallEntity_Old = new tmpServiceCallDataSet();
                                    serviceCallEntity_Old.Merge(objReturn_Old.Copy());
                                    saveCancelCall(canceldate, canceltime, cancelby, "", serviceCallEntity_Old);
                                }

                                string doctran = Session["DocnumberTran" + idGen].ToString();
                                /*  NotificationLibrary.GetInstance().TicketAlertEvent(
                                      NotificationLibrary.EVENT_TYPE.TICKET_CANCEL,
                                      SID,
                                      CompanyCode,
                                      doctran,
                                      EmployeeCode
                                  );*/

                                Session["ModeReset" + idGen] = null;
                                Session["ticketType" + idGen] = null;
                                Session["ticketNo" + idGen] = null;
                                Session["ticketYear" + idGen] = null;
                                Session["DocnumberTran" + idGen] = null;

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
                        Session["SCF_SAVESUCCESS" + idGen] = "Save Success Document Number : " + displayDocNumber;

                        lib.updateTicketRefDoc(
                            SID,
                            CompanyCode,
                            ticketNo,
                            txtRef_SO.Text,
                            txtRef_QT.Text,
                            txtRef_PO.Text,
                            EmployeeCode
                        );

                        AssignWork_Change(ticketType, ticketNo, ticketYear, remark);

                        CreateWorkflow(ticketNo, "");

                        Boolean IsAutoWorkflow = accountabilityService.getConfigStructureIsAutoWorkflow(
                            SID, "", ddlAccountability.SelectedValue
                        );
                        if (IsAutoWorkflow)
                        {
                            string DocStatusStart = ServiceTicketLibrary.GetInstance().GetTicketStatusFromEvent(
                                SID,
                                CompanyCode,
                                ServiceTicketLibrary.TICKET_STATUS_EVENT_START_BUSINESS_CHANGE
                            );
                            saveUpdateTicketDocStatus(DocStatusStart, ticketType, ticketNo, ticketYear);
                        }

                        string ticket_ref_code = (Session["SCT_created_ticket_ref_code" + idGen] != null ? (Session["SCT_created_ticket_ref_code" + idGen] as string) : "");
                        if (!string.IsNullOrEmpty(ticket_ref_code))
                        {
                            LinkFlowChartService.insertNewItemRelation(
                                SID,
                                CompanyCode,
                                LinkFlowChartService.ItemGroup_TICKET,
                                ticketNo,
                                ticket_ref_code,
                                "",
                                EmployeeCode,
                                Validation.getCurrentServerStringDateTime(),
                                CustomerCode
                            );
                            Session["SCT_created_ticket_ref_code" + idGen] = null;
                        }


                        NotificationLibrary.GetInstance().TicketAlertEvent(
                            NotificationLibrary.EVENT_TYPE.ChangeOrder_Approval,
                            SID,
                            CompanyCode,
                            ticketNo + "|L0",
                            EmployeeCode,
                            ThisPage + "_CreateTicket"
                        );



                        getdataToedit(ticketType, ticketNo, ticketYear, true, false, ApplicationSession.DISPLAY_MODE_STRING);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<string> validateForm()
        {
            List<string> _rs = new List<string>();
            return _rs;
        }

        private string AssignWork_Change(string ticketType, string ticketNo, string ticketYear, string remark)
        {
            string ticketServiceCode = "";

            List<listOperationTransfer> listEmp = JsonConvert.DeserializeObject<List<listOperationTransfer>>(hddOwner_ListEMPCode.Value);
            string MainDelegate = listEmp
                .Where(w => w.IsMain)
                .First()
                .EmpCode;

            string[] participantsArray = listEmp
                .Where(w => !w.IsMain && !w.Event.Equals("REMOVE"))
                .Select(s => s.EmpCode)
                .ToArray();


            double resolutionTime = 0;
            DateTime planDateTimeStart = ObjectUtil.ConvertDateTimeDBToDateTime(Validation.Convert2DateTimeDB(tbStartDate.Text + " " + txtStartTime.Text + ":00"));
            DateTime planDateTimeEnd = ObjectUtil.ConvertDateTimeDBToDateTime(Validation.Convert2DateTimeDB(tbEndDate.Text + " " + txtEndTime.Text + ":00"));
            TimeSpan ts = planDateTimeEnd - planDateTimeStart;
            resolutionTime = ts.TotalSeconds;

            //UploadGallery
            string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
            string UploadFileUrl = Domain + "/managefile/" + ERPWAuthentication.SID + "/kmfile/assets/";
            string UploadFilePath = Server.MapPath("~/managefile/" + ERPWAuthentication.SID + "/kmfile/assets/");
            if (!Directory.Exists(UploadFilePath))
            {
                Directory.CreateDirectory(UploadFilePath);
            }

            UploadGallery.SavePath = "/managefile/" + ERPWAuthentication.SID + "/kmfile/assets/";
            DataTable dtFile = UploadGallery.SaveFiles();
            //\UploadGallery

            ticketServiceCode = AfterSaleService.getInstance().StartTicketChange(
                SID,
                CompanyCode,
                ticketType, ticketNo, ticketYear,
                ddlOwnerGroupService.SelectedItem.Text,
                MainDelegate, participantsArray, remark,
                UserName,
                EmployeeCode,
                FullNameEN,
                resolutionTime,
                Validation.Convert2DateTimeDB(tbStartDate.Text + " " + txtStartTime.Text + ":00"),
                dtFile, UploadFileUrl, UploadFilePath
            );

            return ticketServiceCode;
        }

        public void getDataSave()
        {
            if (serviceCallEntity.cs_servicecall_header.Rows.Count <= 0)
            {
                throw new Exception("No Data found in Header!!");
            }

            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< GetData Header >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            string doctype = Session["SCT_created_doctype_code" + idGen] == null ? "" : (string)Session["SCT_created_doctype_code" + idGen];
            string fiscalyear = Session["SCT_created_fiscalyear" + idGen] == null ? "" : (string)Session["SCT_created_fiscalyear" + idGen];
            DataRow drHeader = serviceCallEntity.cs_servicecall_header.Rows[0];

            drHeader["sid"] = SID;
            drHeader["DocType"] = doctype;
            drHeader["Fiscalyear"] = fiscalyear;
            drHeader["DOCDATE"] = Validation.Convert2DateDB(Validation.getCurrentServerDate());
            drHeader["CompanyCode"] = CompanyCode;
            drHeader["CustomerCode"] = (string)Session["SCT_created_cust_code" + idGen];
            drHeader["CREATED_BY"] = EmployeeCode;
            drHeader["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
            drHeader["ContractPersonTel"] = _ddl_contact_phone_noTran.SelectedValue;
            //drHeader["ContractPersonName"] = _ddl_contact_person.SelectText;
            drHeader["Email"] = _ddl_contact_emailTran.SelectedValue;
            drHeader["Address"] = _ddl_contact_addressTran.SelectedValue;
            drHeader["HeaderText"] = txt_problem_topic.Text;
            drHeader["ProjectCode"] = _ddl_projectcode.SelectedValue;
            drHeader["ProjectElement"] = _ddl_project_elementTran.SelectedValue;
            drHeader["ReferenceDocument"] = tbRefer.Text;
            drHeader["Impact"] = ddlImpact.SelectedValue;
            drHeader["Urgency"] = ddlUrgency.SelectedValue;
            drHeader["Priority"] = _ddl_priorityTran.SelectedValue;
            drHeader["MajorIncident"] = chkMajorIncident.Checked.ToString();
            drHeader["CAB"] = CABCheck.Checked;

            //if (tbCallbackDate.Text != "")
            //{
            //    if (tbCallbackTime.Text == "")
            //    {
            //        throw new Exception("Call back time is required.");
            //    }

            //    drHeader["CallbackDate"] = Validation.Convert2DateDB(tbCallbackDate.Text);
            //    drHeader["CallbackTime"] = Validation.Convert2TimeDB(tbCallbackTime.Text + ":00");
            //}
            //else
            //{
            //    drHeader["CallbackDate"] = "";
            //    drHeader["CallbackTime"] = "";
            //}
        }

        private void addInitData()
        {
            if (serviceCallEntity.cs_servicecall_Contactdetail_Header.Rows.Count <= 0)
            {
                DataTable dtContactHeader = serviceCallEntity.cs_servicecall_Contactdetail_Header;
                DataRow drContactHeader = dtContactHeader.NewRow();
                drContactHeader["sid"] = SID;
                drContactHeader["ObjectID"] = Session["ObjectID" + idGen];
                dtContactHeader.Rows.Add(drContactHeader);
            }
        }

        protected void getdataToedit(string doctype, string docnumber, string fiscalyear,
            bool IsAutoRedirect = true, bool genNewId = false, string ApplicationMode = null)
        {
            if (string.IsNullOrEmpty(ApplicationMode))
            {
                ApplicationMode = mode_stage;
            }

            Object[] objParam = new Object[] { "1500117",
                    (string)Session[ApplicationSession.USER_SESSION_ID],
                    CompanyCode,doctype,docnumber,fiscalyear};

            DataSet[] objDataSet = new DataSet[] { serviceCallEntity };
            DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);

            if (objReturn != null)
            {
                if (IsAutoRedirect)
                {
                    if (genNewId)
                    {
                        string idGen = Guid.NewGuid().ToString().Substring(0, 8);
                        Session["ServicecallEntity" + idGen] = new tmpServiceCallDataSet();
                        (Session["ServicecallEntity" + idGen] as tmpServiceCallDataSet).Merge(objReturn.Copy());
                        Session["SC_MODE" + idGen] = ApplicationMode;

                        string PageRedirect = lib.getPageTicketRedirect(
                            SID,
                            (Session["ServicecallEntity" + idGen] as tmpServiceCallDataSet).cs_servicecall_header.Rows[0]["Doctype"].ToString()
                        );
                        Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen));
                    }
                    else
                    {
                        serviceCallEntity = new tmpServiceCallDataSet();
                        serviceCallEntity.Merge(objReturn.Copy());
                        mode_stage = ApplicationMode;

                        string PageRedirect = lib.getPageTicketRedirect(
                            SID,
                            serviceCallEntity.cs_servicecall_header.Rows[0]["Doctype"].ToString()
                        );
                        Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen));
                    }
                }
                else
                {
                    serviceCallEntity = new tmpServiceCallDataSet();
                    serviceCallEntity.Merge(objReturn.Copy());
                    mode_stage = ApplicationMode;
                }
            }
        }

        protected void mapScreenItemToEntityUpdate()
        {
            foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
            {
                dr["HeaderText"] = txt_problem_topic.Text;
                dr["ContractPersonTel"] = _ddl_contact_phone_noTran.SelectedValue;
                //dr["ContractPersonName"] = _ddl_contact_person.SelectText;
                dr["Email"] = _ddl_contact_emailTran.SelectedValue;
                dr["Address"] = _ddl_contact_addressTran.SelectedValue;
                dr["ProjectCode"] = _ddl_projectcode.SelectedValue;
                dr["ProjectElement"] = _ddl_project_elementTran.SelectedValue;
                dr["ReferenceDocument"] = tbRefer.Text;
                dr["Impact"] = ddlImpact.SelectedValue;
                dr["Urgency"] = ddlUrgency.SelectedValue;
                dr["Priority"] = _ddl_priorityTran.SelectedValue;
                dr["MajorIncident"] = chkMajorIncident.Checked.ToString();
                dr["Docstatus"] = ddlTicketStatus_Change.SelectedValue;
                //if (tbCallbackDate.Text != "")
                //{
                //    if (tbCallbackTime.Text == "")
                //    {
                //        throw new Exception("Call back time is required.");
                //    }

                //    dr["CallbackDate"] = Validation.Convert2DateDB(tbCallbackDate.Text);
                //    dr["CallbackTime"] = Validation.Convert2TimeDB(tbCallbackTime.Text + ":00");
                //}
                //else
                //{
                //    dr["CallbackDate"] = "";
                //    dr["CallbackTime"] = "";
                //}
            }
        }
        #endregion

        #region Create
        void initCreated()
        {
            lblTicketNo_Modal.Text = "";

            if (Session["SCT_created_ref" + idGen] == null)
            {
                defaultData();
            }
            else
            {
                string sessionId = (string)Session[ApplicationSession.USER_SESSION_ID];
                string refTicketType = (string)Session["SCT_created_ref_doctype" + idGen];
                string refTicketYear = (string)Session["SCT_created_ref_fiscalyear" + idGen];
                string refTicketNo = (string)Session["SCT_created_ticket_ref_code" + idGen];
                string newTicketType = (string)Session["SCT_created_doctype_code" + idGen];
                string newTicketYear = (string)Session["SCT_created_fiscalyear" + idGen];
                string newCustomerCode = (string)Session["SCT_created_cust_code" + idGen];

                serviceCallEntity = lib.CopyTicket(sessionId, SID, CompanyCode,
                    refTicketType, refTicketYear, refTicketNo,
                    newTicketType, newTicketYear, newCustomerCode);

                Session["SCT_created_ref" + idGen] = null;
                Session["SCT_created_ref_doctype" + idGen] = null;
                Session["SCT_created_ref_fiscalyear" + idGen] = null;
            }

            GetEquipment_V2();
            GetProblemGroup();
            GetOwnerGroupService();
            GetReferDocument();

            mapdataToscreen();

            if (chkIsLoad_Attachment.Checked)
                setAttachMent("", false
                    , (string)Session["SCT_created_doctype_code" + idGen], true
                    , "", true
                    , (string)Session["SCT_created_fiscalyear" + idGen], true);
        }

        private void defaultData()
        {
            serviceCallEntity = new tmpServiceCallDataSet();//dsNew;

            if (!serviceCallEntity.cs_servicecall_item.Columns.Contains("Severity"))
            {
                serviceCallEntity.cs_servicecall_item.Columns.Add("Severity", typeof(string));
            }

            string docType = Session["SCT_created_doctype_code" + idGen] == null ? "" : (string)Session["SCT_created_doctype_code" + idGen];
            string fiscalyear = Session["SCT_created_fiscalyear" + idGen] == null ? "" : (string)Session["SCT_created_fiscalyear" + idGen];
            string obj = SID + docType + fiscalyear + getCallerID();

            if (serviceCallEntity.cs_servicecall_header.Rows.Count <= 0)
            {
                DataRow dr = serviceCallEntity.cs_servicecall_header.NewRow();
                dr["sid"] = SID;
                dr["CompanyCode"] = CompanyCode;
                dr["CustomerCode"] = (string)Session["SCT_created_cust_code" + idGen];
                dr["ContractPersonName"] = Session["SCT_created_contact_desc" + idGen] == null ? "" : (string)Session["SCT_created_contact_desc" + idGen];
                dr["CallerID"] = "";
                dr["ObjectID"] = Session["ObjectID" + idGen] = obj;
                dr["DocType"] = docType;
                dr["Fiscalyear"] = fiscalyear;
                dr["DOCDATE"] = Validation.getCurrentServerStringDateTime().Substring(0, 8);
                dr["CallStatus"] = ServiceTicketLibrary.SERVICE_CALL_STATUS_OPEN;
                dr["Docstatus"] = lib.GetTicketStatusFromEvent(
                    SID,
                    CompanyCode,
                    ServiceTicketLibrary.TICKET_STATUS_EVENT_START_BUSINESS_CHANGE
                );
                if (Session["SCT_created_remark" + idGen] != null)
                {
                    dr["HeaderText"] = (string)Session["SCT_created_remark" + idGen];
                }
                if (Session["SCT_created_impact" + idGen] != null)
                {
                    dr["Impact"] = (string)Session["SCT_created_impact" + idGen];
                }
                if (Session["SCT_created_urgency" + idGen] != null)
                {
                    dr["Urgency"] = (string)Session["SCT_created_urgency" + idGen];
                }
                if (Session["SCT_created_priority" + idGen] != null)
                {
                    dr["Priority"] = (string)Session["SCT_created_priority" + idGen];
                }

                serviceCallEntity.cs_servicecall_header.Rows.Add(dr);

                Session["responsecall_objid" + idGen] = obj;

                ShowWorkFlowWithoutCreate = true;
                AlowWorkFlow = true;
            }
        }

        private String getCallerID()
        {
            int numrows = serviceCallEntity.cs_servicecall_item.Rows.Count;
            numrows++;

            return Convert.ToString(numrows).PadLeft(4, '0');
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
                , null, false);//, true, EntityUtils.ACTION_CREATE
        }
        #endregion

        #region Edit
        protected void mapdataToscreen()
        {
            string objectID = "";//ไอดีสำหรับใช้กับ funtion getStartDateTime ใน afterseleservice
            foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
            {
                string displayDocNumber = "";

                if (!string.IsNullOrEmpty(dr["DocType"].ToString()) && !string.IsNullOrEmpty(dr["CallerID"].ToString()))
                {
                    displayDocNumber = AfterSaleService.getInstance().GetTicketNoForDisplay(SID, CompanyCode,
                    dr["DocType"].ToString(), dr["CallerID"].ToString());
                }

                _txt_docnumberTran.Value = displayDocNumber; //dr["CallerID"].ToString();
                lbl_docnumberTran.Text = " : " + displayDocNumber;

                ////Lebel////
                string[] TicketType = GetSCTypeDesc(dr["DocType"].ToString()).Split(':');
                ltrTicketType.Text = TicketType[1];
                //labelDocumentType.Text = TicketType[1] + " Type";
                //labelDocumentNumber.Text = TicketType[1] + " No.";
                //_lbl_TicketStatusTran.Text = TicketType[1] + " Satatus";
                //labelCreatedBy.Text = TicketType[1] + " Created By";
                //lebelImpact.Text = TicketType[1] + " Impact";
                //labelUrgency.Text = TicketType[1] + " Urgency";
                //labelPriority.Text = TicketType[1] + " Priority";
                //labelProblem.Text = TicketType[1] + " Subject";
                //labelDescription.Text = TicketType[1] + " Description";
                ////Lebel////

                hddDocnumberTran.Value = dr["CallerID"].ToString();
                hddTicketStatus.Value = dr["Docstatus"].ToString();
                hddTicketStatus_Old.Value = dr["Docstatus"].ToString();
                hddTicketStatus_New.Value = dr["Docstatus"].ToString();

                lblTicketNo_Modal.Text = dr["CallerID"].ToString();

                txt_problem_topic.Text = dr["HeaderText"].ToString();
                hddOldValue_problem_topic.Value = dr["HeaderText"].ToString();

                _txt_doctypeTran.Value = GetSCTypeDesc(dr["DocType"].ToString());
                hddTicketDocType.Value = dr["DocType"].ToString();

                initTextLabel(dr["DocType"].ToString());
                objectID = dr["ObjectID"].ToString();//เก็บไอดี ไป qry เวลาของ ticket
                _txt_fiscalyear.Value = dr["FiscalYear"].ToString();
                /* if (string.IsNullOrEmpty(dr["CustomerCode"].ToString()))
                 {
                     _ddl_contact_person.Enabled = false;

                 }
                 else
                 {
                     _ddl_contact_person.Enabled = true;

                 }*/

                hddCustomerCode.Value = dr["CustomerCode"].ToString();

                //update
                hddTransferChangeOrder_TaskName.Value = dr["HeaderText"].ToString();
                //update

                CustomerCode = dr["CustomerCode"].ToString();

                GetAreaCodeFromDocumentType(dr["DocType"].ToString());

                ShowWorkFlow = true;
                ShowWorkFlowWithoutCreate = true;
                AlowWorkFlow = true;

                Session["SCT_created_cust_code" + idGen] = dr["CustomerCode"].ToString();
                Session["SCT_created_doctype_code" + idGen] = dr["DocType"].ToString();
                Session["SCT_created_fiscalyear" + idGen] = dr["Fiscalyear"].ToString();
                Session["responsecall_objid" + idGen] = dr["ObjectID"].ToString();

                //GetContactPerson();

                // _ddl_contact_person.SetValueFromName = dr["ContractPersonName"].ToString();

                // GetcontactDetailForScreen();

                _ddl_contact_phone_noTran.SelectedValue = dr["ContractPersonTel"].ToString();
                _ddl_contact_emailTran.SelectedValue = dr["Email"].ToString();
                _ddl_contact_addressTran.SelectedValue = dr["Address"].ToString();
                _ddl_projectcode.SelectedValue = dr["ProjectCode"].ToString();

                GetProjectElement();

                _ddl_project_elementTran.SelectedValue = dr["ProjectElement"].ToString();

                _txt_companyTran.Value = CompanyName;
                _txt_docdateTran.Value = Validation.Convert2DateTimeDisplay(dr["CREATED_ON"].ToString());//Validation.Convert2DateDisplay(dr["DOCDATE"].ToString());
                _txt_docstatusTran.Value = GetDocStatusDesc(dr["CallStatus"].ToString());
                _txt_TicketStatusTran.Value = dr["Docstatus"].ToString() + " : " + ServiceTicketLibrary.GetTicketDocStatusDesc(SID, CompanyCode, dr["Docstatus"].ToString());
                ddlTicketStatus_Change.SelectedValue = dr["Docstatus"].ToString();

                //dev
                if (dr["CallStatus"].ToString() == ServiceTicketLibrary.SERVICE_CALL_STATUS_CANCEL || dr["CallStatus"].ToString() == ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE)
                {
                    hddHidePostRemark.Value = true.ToString();
                }
                else
                {
                    hddHidePostRemark.Value = false.ToString();
                }
                //dev

                tbRefer.Text = dr["ReferenceDocument"].ToString();
                ddlImpact.SelectedValue = dr["Impact"].ToString();
                ddlUrgency.SelectedValue = dr["Urgency"].ToString();

                GetSeverity();

                _ddl_priorityTran.SelectedValue = dr["Priority"].ToString();

                tbCreatedBy.Text = GetEmployeeNameWithCode(dr["CREATED_BY"].ToString());

                bool majorIncident = false;
                bool getCAB = false;
                
                bool.TryParse(dr["MajorIncident"].ToString(), out majorIncident);

                chkMajorIncident.Checked = majorIncident;

                bool.TryParse(dr["CAB"].ToString(), out getCAB);
                CABCheck.Checked = getCAB;

                //ddlAffectSLA.SelectedValue = dr["AffectSLA"].ToString();

                //tbCallbackDate.Text = Validation.Convert2DateDisplay(dr["CallbackDate"].ToString());

                //if (!string.IsNullOrEmpty(dr["CallbackTime"].ToString()))
                //{
                //    tbCallbackTime.Text = Validation.Convert2TimeDisplay(dr["CallbackTime"].ToString()).Substring(0, 5);
                //}
                //else
                //{
                //    tbCallbackTime.Text = "";
                //}

                if (chkIsLoad_Attachment.Checked)
                    setAttachMent("", false
                        , (string)Session["SCT_created_doctype_code" + idGen], true
                        , hddDocnumberTran.Value.Trim(), true
                        , (string)Session["SCT_created_fiscalyear" + idGen], true);
            }

            if (serviceCallEntity.cs_servicecall_item.Rows.Count > 0)
            {
                bindDataCI();

                DataRow dr = serviceCallEntity.cs_servicecall_item.Rows[0];
                try
                {
                    ddlProblemGroup.SelectedValue = dr["ProblemGroup"].ToString();
                    ddlProblemType.SelectedValue = dr["ProblemTypeCode"].ToString();
                    ddlProblemSource.SelectedValue = dr["OriginCode"].ToString();
                    ddlServiceSource.SelectedValue = dr["CallTypeCode"].ToString();
                    ddlOwnerGroupService.SelectedValue = dr["QueueOption"].ToString();

                    if (!string.IsNullOrEmpty(dr["PlanStartDate"].ToString()))
                        tbStartDate.Text = Validation.Convert2DateDisplay(dr["PlanStartDate"].ToString());
                    if (!string.IsNullOrEmpty(dr["PlanEndDate"].ToString()))
                        tbEndDate.Text = Validation.Convert2DateDisplay(dr["PlanEndDate"].ToString());
                    if (!string.IsNullOrEmpty(dr["PlanStartTime"].ToString()))
                    {
                        string[] dateTimeStart = Validation.Convert2TimeDisplay(dr["PlanStartTime"].ToString()).Split(':');
                        if (dateTimeStart.Length >= 2)
                        {
                            txtStartTime.Text = dateTimeStart[0] + ":" + dateTimeStart[1];
                        }
                        else
                        {
                            txtStartTime.Text = "";
                        }
                    }
                    if (!string.IsNullOrEmpty(dr["PlanEndTime"].ToString()))
                    {
                        string[] dateTimeEnd = Validation.Convert2TimeDisplay(dr["PlanEndTime"].ToString()).Split(':');
                        if (dateTimeEnd.Length >= 2)
                        {
                            txtEndTime.Text = dateTimeEnd[0] + ":" + dateTimeEnd[1];
                        }
                        else
                        {
                            txtEndTime.Text = "";
                        }
                    }

                }
                catch (Exception)
                {

                }

                tbEquipmentRemark.Text = dr["Remark"].ToString();
                hddOldValue_EquipmentRemark.Value = dr["Remark"].ToString();

                /* tbSummaryProblem.Text = dr["SummaryProblem"].ToString();
                 tbSummaryCause.Text = dr["SummaryCause"].ToString();
                 tbSummaryResolution.Text = dr["SummaryResolution"].ToString();*/

                hddIncidentAreaCode.Value = Convert.ToString(dr["IncidentArea"]);
                udpUpdateAreaCode.Update();
            }

            if (chkIsLoad_DateTimeLog.Checked)
                bindDataTicketDateTimeLog();


            //======== binding data SLA Time ====================================================================
            DataTable dt_slaTime = AfterSaleService.getInstance().getSlaTime(hddTicketDocType.Value, _ddl_priorityTran.SelectedValue);
            DataTable dt_slaStart = AfterSaleService.getInstance().getStartDateTime(objectID);
            if (dt_slaTime.Rows.Count > 0 && dt_slaStart.Rows.Count > 0)
            {

                lbTierName.Text = AfterSaleService.getInstance().getTierName(SID, WorkGroupCode, dt_slaTime.Rows[0]["TierCode"].ToString());
                //Title
                slaTitle.Text = dt_slaStart.Rows[0]["Subject"].ToString();
                //detail
                slaDetail.Text = dt_slaStart.Rows[0]["Remark"].ToString();
                // time violation
                slaTime.Text = ConvertToTime(dt_slaTime.Rows[0]["Resolution"].ToString());

                //start ticket date time
                try
                {
                    slaStartDateTime.Text = calculateStartDateTime(
                                                                convertFomatDate(dt_slaStart.Rows[0]["CreatedOnDate"].ToString()),
                                                                convertFomatTime(dt_slaStart.Rows[0]["CreatedOnTime"].ToString())
                                                                );
                    //violation time
                    slaEndDate.Text = calculateViolationTime(
                                                            convertFomatDate(dt_slaStart.Rows[0]["CreatedOnDate"].ToString()),
                                                            convertFomatTime(dt_slaStart.Rows[0]["CreatedOnTime"].ToString()),
                                                            dt_slaTime.Rows[0]["Resolution"].ToString()
                                                            );
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
            //================================================================================================
        }
        public string convertFomatDate(string _date)
        {
            char[] chr = _date.ToCharArray();
            string yaer = new string(chr, 0, 4);
            string mont = new string(chr, 4, 2);
            string day = new string(chr, 6, 2);
            return day + '/' + mont + '/' + yaer;
        }
        public string convertFomatTime(string _time)
        {
            char[] chr = _time.ToCharArray();
            int hr = Convert.ToInt32( new string(chr, 0, 2));
            int min = Convert.ToInt32(new string(chr, 2, 2));
            int sec = Convert.ToInt32(new string(chr, 4, 2));
            return ((hr * 60 * 60) + (min * 60) + sec).ToString();
        }
        public string calculateStartDateTime(string _createdOnDate, string _createdOnTime)
        {
           
            DateTime fomatDate = DateTime.ParseExact(_createdOnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //DateTime fomatDate = Convert.ToDateTime(_createdOnDate);
            DateTime slaStartDateTime = fomatDate.Add(TimeSpan.FromSeconds(Convert.ToDouble(_createdOnTime)));
            return slaStartDateTime.ToString(); 
        }
        public string calculateViolationTime(string _createdOnDate, string _createdOnTime, string resolutionTime)
        {
            DateTime fomatDate = DateTime.ParseExact(_createdOnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //DateTime fomatDate = Convert.ToDateTime(_createdOnDate);
            DateTime startDateTime = fomatDate.Add(TimeSpan.FromSeconds(Convert.ToDouble(_createdOnTime)));
            DateTime violationDateTime = startDateTime.Add(TimeSpan.FromSeconds(Convert.ToDouble(resolutionTime)));
            return violationDateTime.ToString();
        }

        protected void controlscreen()
        {
            string status = "", docnumber = "", docStatus = "";
            foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
            {
                docnumber = dr["CallerID"].ToString();
                status = dr["CallStatus"].ToString();
                docStatus = dr["Docstatus"].ToString();
            }

            _txt_docstatusTran.Value = GetDocStatusDesc(status);
            _txt_TicketStatusTran.Value = docStatus + " : " + ServiceTicketLibrary.GetTicketDocStatusDesc(SID, CompanyCode, docStatus);
            ddlTicketStatus_Change.SelectedValue = docStatus;

            if (ServiceTicketLibrary.SERVICE_CALL_STATUS_CANCEL.Equals(status) || ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE.Equals(status))
            {
                btnConfirmClientTran.Visible = false;

                /*ddlAffectSLA.Enabled = false;
                tbSummaryProblem.Enabled = false;
                tbSummaryCause.Enabled = false;
                tbSummaryResolution.Enabled = false;*/
                tbRefer.Enabled = false;
                tbStartDate.Enabled = false;
                tbEndDate.Enabled = false;

                txt_problem_topic.Enabled = false;
                tbEquipmentRemark.Enabled = false;
                //update

                //update
            }
            else if (ServiceTicketLibrary.SERVICE_CALL_STATUS_OPEN.Equals(status) && !string.IsNullOrEmpty(docnumber))
            {
                btnConfirmClientTran.Visible = true;
            }
            else
            {
                btnConfirmClientTran.Visible = true;
            }
        }

        public string getSerialNoCI(string EquipmentCode)
        {
            foreach (DataRow drCI in serviceCallEntity.cs_servicecall_item.Select("EquipmentNo = '" + EquipmentCode + "'"))
            {
                return Convert.ToString(drCI["SerialNo"]);
            }

            return "";
        }
        #endregion


        /* private void GetContact()
         {

             string custcode = Session["SCT_created_cust_code" + idGen] == null ? "" : (string)Session["SCT_created_cust_code" + idGen];//_ddl_customer_code.SelectedIndex != -1 ? _ddl_customer_code.SelectedValue : "";
             dtContactPerson = AfterSaleService.getInstance().getContactPerson(CompanyCode, custcode);
             _ddl_contact_person.initialDataAutoComplete(dtContactPerson, "BOBJECTLINK", "NAME1", false, "email", "phone", "remark");
             udpContactRefresh.Update();
         }*/

        /* private void GetcontactDetailForScreen()
         {
             string bobjectid = _ddl_contact_person.SelectValue;
             ERPW.Lib.Master.ContactEntity en = new ERPW.Lib.Master.ContactEntity();
             if (!string.IsNullOrEmpty(bobjectid))
             {
                 List<ERPW.Lib.Master.ContactEntity> listen = ERPW.Lib.Master.CustomerService.getInstance().getListContactRefCustomer(
                 SID, CompanyCode
                 , CustomerCode
                 , ""
                 , bobjectid);
                 if (listen.Count > 0)
                 {
                     en = listen[0];
                 }
             }

         }*/

        private void GetTicketDocStatus()
        {
            ddlTicketStatus_Temp.DataTextField = "DocumentStatusDesc";
            ddlTicketStatus_Temp.DataValueField = "DocumentStatus";
            ddlTicketStatus_Temp.DataSource = dtTicketDocStatus;
            ddlTicketStatus_Temp.DataBind();

            ddlTicketStatus_Change.DataTextField = "DocumentStatusDesc";
            ddlTicketStatus_Change.DataValueField = "DocumentStatus";
            ddlTicketStatus_Change.DataSource = dtTicketDocStatus;
            ddlTicketStatus_Change.DataBind();
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
            DataTable dtPriority = lib.GetPriorityMaster(SID);
            _ddl_priorityTran.Items.Clear();
            _ddl_priorityTran.Items.Add(new ListItem("", ""));
            _ddl_priorityTran.AppendDataBoundItems = true;
            _ddl_priorityTran.DataSource = dtPriority;
            _ddl_priorityTran.DataBind();
        }

        private void GetProject()
        {
            _ddl_projectcode.Items.Clear();
            //dtProjectCode = AfterSaleService.getInstance().getProjectCode(SID, WorkGroupCode, Session["SCT_created_cust_code" + idGen] == null ? "" : Session["SCT_created_cust_code" + idGen].ToString());
            _ddl_projectcode.Items.Add(new ListItem("", ""));
            _ddl_projectcode.AppendDataBoundItems = true;
            _ddl_projectcode.DataTextField = "Description";
            _ddl_projectcode.DataValueField = "ProjectCode";
            _ddl_projectcode.DataSource = dtProjectCode;
            _ddl_projectcode.DataBind();
        }

        private void GetProjectElement()
        {
            _ddl_project_elementTran.Items.Clear();
            string projectcode = _ddl_projectcode.SelectedIndex != -1 ? _ddl_projectcode.SelectedValue : "";
            //dtProjectElement = AfterSaleService.getInstance().getProjectElement(SID, CompanyCode, projectcode);
            _ddl_project_elementTran.Items.Add(new ListItem("", ""));
            _ddl_project_elementTran.AppendDataBoundItems = true;
            _ddl_project_elementTran.DataTextField = "ELEMENTDESC";
            _ddl_project_elementTran.DataValueField = "BOMID";
            _ddl_project_elementTran.DataSource = dtProjectElement;
            _ddl_project_elementTran.DataBind();
        }

        /*private void GetContactPerson()
        {
            string custcode = Session["SCT_created_cust_code" + idGen] == null ? "" : (string)Session["SCT_created_cust_code" + idGen];//_ddl_customer_code.SelectedIndex != -1 ? _ddl_customer_code.SelectedValue : "";
            dtContactPerson = AfterSaleService.getInstance().getContactPerson(CompanyCode, custcode);
            _ddl_contact_person.initialDataAutoComplete(dtContactPerson, "BOBJECTLINK", "NAME1", false, "email", "phone", "remark");


            udpContactRefresh.Update();
        }*/

        private void GetReferDocument()
        {
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

        private string GetCustomerDesc(string code)
        {
            DataRow[] drr = dtCustomer.Select("CustomerCode='" + code + "'");
            string desc = "";
            if (drr.Length > 0)
            {
                desc = drr[0]["CustomerName"].ToString();
            }
            return desc;
        }

        private string GetDocStatusDesc(string code)
        {
            DataRow[] drr = dtDocstatus.Select("Name='" + code + "'");
            string desc = "";
            if (drr.Length > 0)
            {
                desc = drr[0]["Description"].ToString();
            }
            return desc;
        }

        //public string GetEquipmentName(object code)
        //{
        //    DataRow[] drr = dtEquipment.Select("EquipmentCode='" + code + "'");
        //    string desc = "";
        //    if (drr.Length > 0)
        //    {
        //        desc = drr[0]["Description"].ToString();
        //    }
        //    return desc;
        //}

        public string GetProblemGroupDesc(object code)
        {
            DataRow[] drr = dtGrouTemp.Select("GROUPCODE='" + code + "'");
            string desc = "";
            if (drr.Length > 0)
            {
                desc = drr[0]["GROUPNAME"].ToString();
            }
            return desc;
        }

        //public string GetProblemTypeDesc(object code)
        //{
        //    DataRow[] drr = dtProblemTypeTemp.Select("Name='" + code + "'");
        //    string desc = "";
        //    if (drr.Length > 0)
        //    {
        //        desc = drr[0]["Description"].ToString();
        //    }
        //    return desc;
        //}

        private void bindTicketRefDoc()
        {
            DataTable dt = lib.getTicketRefDoc(
                SID,
                CompanyCode,
                hddDocnumberTran.Value
            );

            if (dt.Rows.Count > 0)
            {
                txtRef_SO.Text = Convert.ToString(dt.Rows[0]["RefDocSO"]);
                txtRef_QT.Text = Convert.ToString(dt.Rows[0]["RefDocQT"]);
                txtRef_PO.Text = Convert.ToString(dt.Rows[0]["RefDocPO"]);
            }
        }
        #region Modalitem BindDropDown

        private void GetEquipment_V2()
        {
        }

        private void GetProblemGroup()
        {
            string _doctype = "";

            foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
            {
                _doctype = Convert.ToString(dr["DocType"]);
            }
            string businessObject = lib.GetBusinessObjectFromTicketType(SID, _doctype);
            mBusinessObject = businessObject;

            List<IncidentAreaEntity.AreaGroup> En = libconfig.getIncedentAreaGroup(
                SID, CompanyCode,
                mBusinessObject, ""
            );

            if (!string.IsNullOrEmpty(ddlOwnerGroupService.SelectedValue))
                En = En.Where(w => w.ChildrenOwnerGroup.Contains(ddlOwnerGroupService.SelectedValue)).ToList();

            ddlProblemGroup.DataValueField = "GroupCode";
            ddlProblemGroup.DataTextField = "GroupName";
            ddlProblemGroup.DataSource = En;
            ddlProblemGroup.DataBind();
            ddlProblemGroup.Items.Insert(0, new ListItem("", ""));
        }

        private void GetProblemType(string problemGroup)
        {
            List<IncidentAreaEntity.AreaType> En = new List<IncidentAreaEntity.AreaType>();
            DataTable dt = new DataTable();

            if (!string.IsNullOrEmpty(problemGroup))
            {
                En = libconfig.getIncedentAreaType(
                    SID, CompanyCode,
                    mBusinessObject, "", true, problemGroup
                );
                if (!string.IsNullOrEmpty(ddlOwnerGroupService.SelectedValue))
                    En = En.Where(w => w.ChildrenOwnerGroup.Contains(ddlOwnerGroupService.SelectedValue)).ToList();
            }

            ddlProblemType.DataValueField = "TypeCode";
            ddlProblemType.DataTextField = "TypeName";
            ddlProblemType.DataSource = En;
            ddlProblemType.DataBind();
            ddlProblemType.Items.Insert(0, new ListItem("", ""));
        }

        private void GetProblemSource(string IncidentGroup, string IncidentType)
        {
            List<IncidentAreaEntity.AreaSource> En = new List<IncidentAreaEntity.AreaSource>();

            if (!string.IsNullOrEmpty(IncidentGroup) && !string.IsNullOrEmpty(IncidentType))
            {
                En = libconfig.getIncedentAreaSource(
                    SID, CompanyCode,
                    mBusinessObject, "", true, IncidentGroup, IncidentType
                );
                if (!string.IsNullOrEmpty(ddlOwnerGroupService.SelectedValue))
                    En = En.Where(w => w.ChildrenOwnerGroup.Contains(ddlOwnerGroupService.SelectedValue)).ToList();
            }

            ddlProblemSource.DataValueField = "SourceCode";
            ddlProblemSource.DataTextField = "SourceName";
            ddlProblemSource.DataSource = En;
            ddlProblemSource.DataBind();
            ddlProblemSource.Items.Insert(0, new ListItem("", ""));
        }

        private void GetServiceSource(string IncidentGroup, string IncidentType, string IncidentSource)
        {
            List<IncidentAreaEntity.AreaContactSource> En = new List<IncidentAreaEntity.AreaContactSource>();

            if (!string.IsNullOrEmpty(IncidentGroup)
                && !string.IsNullOrEmpty(IncidentType)
                && !string.IsNullOrEmpty(IncidentSource))
            {
                En = libconfig.getIncedentAreaContactSource(
                    SID, CompanyCode,
                    mBusinessObject, "", IncidentGroup, IncidentType, IncidentSource
                );
                if (!string.IsNullOrEmpty(ddlOwnerGroupService.SelectedValue))
                    En = En.Where(w => w.ChildrenOwnerGroup.Contains(ddlOwnerGroupService.SelectedValue)).ToList();
            }

            ddlServiceSource.DataValueField = "ContactSourceCode";
            ddlServiceSource.DataTextField = "ContactSourceName";
            ddlServiceSource.DataSource = En;
            ddlServiceSource.DataBind();
            ddlServiceSource.Items.Insert(0, new ListItem("", ""));
        }

        private void GetOwnerGroupService()
        {
            DataTable dtOwner = libconfig.GetMasterConfigOwnerGroup(SID, CompanyCode, "");
            ddlOwnerGroupService.DataValueField = "OwnerGroupCode";
            ddlOwnerGroupService.DataTextField = "OwnerGroupName";
            ddlOwnerGroupService.DataSource = dtOwner;
            ddlOwnerGroupService.DataBind();
            ddlOwnerGroupService.Items.Insert(0, new ListItem("", ""));

            udpnOwnerService.Update();
            udpnProblem.Update();
        }

        public bool isOBJECT_CHANGE
        {
            get
            {
                string _doctype = "";
                foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
                {
                    _doctype = Convert.ToString(dr["DocType"]);
                }
                string businessObject = lib.GetBusinessObjectFromTicketType(SID, _doctype);
                return ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_CHANGE.Equals(businessObject);
            }
        }

        private void SetOwnerGroupServiceRefIncidentArea(string IncidentGroup, string IncidentType)
        {
            try
            {
                string OwnerGroupCode = libconfig.geteOwnerGroupCodeRefConfigIncidentArea(
                   SID,
                   mBusinessObject,
                   IncidentGroup,
                   IncidentType
                   );
                ddlOwnerGroupService.SelectedValue = OwnerGroupCode;
            }
            catch (Exception ex)
            {
                ddlOwnerGroupService.SelectedIndex = 0;
            }
            udpnOwnerService.Update();
        }


        #endregion

        protected void btnContactPersonChange_Click(object sender, EventArgs e)
        {
            try
            {
                //GetcontactDetailForScreen();
                panelUpdate.Update();
                //   udpContactRefresh.Update();
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

        protected void btnProjectCodeChange_Click(object sender, EventArgs e)
        {
            try
            {
                GetProjectElement();
                panelUpdate.Update();
                // udpContactRefresh.Update();
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

        public void btnCancelDocTran_click(object sender, EventArgs e)
        {
            try
            {
                string canceldate = Validation.getCurrentServerStringDateTime().Substring(0, 8);
                string canceltime = Validation.getCurrentServerDateTime().ToString("HHmmss");
                string cancelby = EmployeeCode;

                string tierDesc = "";
                string ticketType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
                string ticketNo = serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString();
                string ticketYear = serviceCallEntity.cs_servicecall_header.Rows[0]["FiscalYear"].ToString();

                AfterSaleService.getInstance().saveLogCanceledTicket(SID, CompanyCode,
                    ticketType, ticketNo, ticketYear, EmployeeCode, FullNameEN
                );

                saveCancelCall(canceldate, canceltime, cancelby, "");

                NotificationLibrary.GetInstance().TicketAlertEvent(
                    NotificationLibrary.EVENT_TYPE.TICKET_CANCEL,
                    SID,
                    CompanyCode,
                    hddDocnumberTran.Value,
                    EmployeeCode,
                    ThisPage
                );

                getdataToedit((string)Session["SCT_created_doctype_code" + idGen], hddDocnumberTran.Value.Trim(), (string)Session["SCT_created_fiscalyear" + idGen]);
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

        private void countMenu()
        {
            if (serviceCallEntity != null)
            {
                lb_menu_count_itemTran.InnerText = serviceCallEntity.cs_servicecall_item.Rows.Count.ToString();
            }
            else
            {
                lb_menu_count_itemTran.InnerText = "0";
            }

            if (string.IsNullOrEmpty(hddDocnumberTran.Value))
            {
                lb_menu_count_fileTran.InnerText = "(0)";
            }
            else
            {
                lb_menu_count_fileTran.InnerText = "(" + AttachFileUserControl.countFile + ")";
            }

            updcountitem.Update();
            updcountfile.Update();
        }

        protected string GetEmployeeNameWithCode(string empcode)
        {
            string _name = "";

            string sql = @"SELECT FirstName, LastName FROM master_employee WHERE SID = '" + SID + "' AND CompanyCode = '" + CompanyCode + @"' 
                           AND EmployeeCode = '" + empcode + "'";

            DataTable dt = DBService.selectDataFocusone(sql);

            if (dt.Rows.Count > 0)
            {
                return empcode + " : " + dt.Rows[0]["FirstName"] + " " + dt.Rows[0]["LastName"];
            }

            return _name;
        }

        private void BindDataOwnerOperation()
        {
            try
            {
                string Fiscalyear = serviceCallEntity.cs_servicecall_header.Rows[0]["Fiscalyear"].ToString();
                string RoleConfig = "";
                DataTable dtOwner = libconfig.GetMasterConfigOwnerGroup(
                    SID,
                    CompanyCode,
                    ddlOwnerGroupService.SelectedValue
                );

                if (dtOwner.Rows.Count > 0)
                {
                    RoleConfig = Convert.ToString(dtOwner.Rows[0]["RoleConfig"]);
                }

                DataTable dtOwnerOperation = AfterSaleService.getInstance().getOwnerOperation(
                    CompanyCode,
                    Fiscalyear,
                    hddDocnumberTran.Value
                );

                if (dtOwnerOperation.Rows.Count > 0)
                {
                    string AobjectLink = Convert.ToString(dtOwnerOperation.Rows[0]["AOBJECTLINK"]);
                    lbl_xCountRemark.Text = Convert.ToString(dtOwnerOperation.Rows[0]["xCountRemark"]);
                    ActivitySendMailModal.refAobjectlink = AobjectLink;
                    ActivitySendMailModal.refTicketNo = hddDocnumberTran.Value;
                    DataTable dtMain = AfterSaleService.getInstance().getSLASetMain(
                        SID,
                        CompanyCode,
                        RoleConfig,
                        AobjectLink
                    );

                    DataTable dtParticipant = lib.getListEmpParticipantsRefTicket_Change(
                        SID,
                        CompanyCode,
                        AobjectLink,
                        hddDocnumberTran.Value,
                        Fiscalyear,
                        hddTicketDocType.Value,
                        RoleConfig
                    );


                    rptMainDelegate.DataSource = dtMain;
                    rptMainDelegate.DataBind();

                    rptOtherDelegate.DataSource = dtParticipant;
                    rptOtherDelegate.DataBind();

                    if (dtParticipant.Rows.Count > 3)
                    {
                        panelShowMore.Visible = true;
                    }

                    panelFeedActivityComment.Visible = true;
                    hddAobjectlink.Value = AobjectLink;
                    udpParticipants.Update();
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

        protected void saveCancelCall(string canceldate, string canceltime, string cancelby, string cancelcomment,
            tmpServiceCallDataSet dsServiceCall = null)
        {
            try
            {
                if (dsServiceCall == null)
                {
                    dsServiceCall = (tmpServiceCallDataSet)serviceCallEntity.Copy();
                }

                DataTable dtHeader = dsServiceCall.cs_servicecall_header;
                DataTable dtItem = dsServiceCall.cs_servicecall_item;
                DataRow[] drHeader = null;

                if (string.IsNullOrEmpty((string)Session["ticketNo" + idGen]))
                {
                    drHeader = dtHeader.Select("CallerID='" + hddDocnumberTran.Value + "'");
                }
                else
                {
                    drHeader = dtHeader.Select("CallerID='" + ((string)Session["ticketNo" + idGen]) + "'");
                }

                if (drHeader.Length != 0)
                {
                    string SERVICE_DOC_STATUS_CANCEL = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CANCEL_BUSINESS_CHANGE);

                    drHeader[0]["Docstatus"] = SERVICE_DOC_STATUS_CANCEL;
                    drHeader[0]["CallStatus"] = ServiceTicketLibrary.SERVICE_CALL_STATUS_CANCEL;
                    drHeader[0]["CancelDate"] = canceldate;
                    drHeader[0]["CancelTime"] = canceltime;
                    drHeader[0]["CancelBy"] = cancelby;
                    drHeader[0]["CancelComment"] = cancelcomment;
                }

                foreach (DataRow drItem in dtItem.Rows)
                {
                    drItem["CloseStatus"] = ServiceTicketLibrary.SERVICE_CALL_STATUS_CANCEL;
                }

                saveAssignCancelCloseResponseResolution("1500545", "Cancel Ticket", cancelby, dsServiceCall);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void saveReOpenCall(string ReOpenBy)
        {
            try
            {
                DataTable dtHeader = serviceCallEntity.cs_servicecall_header;
                DataTable dtItem = serviceCallEntity.cs_servicecall_item;
                DataRow[] drHeader = dtHeader.Select("CallerID='" + hddDocnumberTran.Value + "'");
                if (drHeader.Length != 0)
                {
                    string SERVICE_DOC_STATUS_INPROGRESS = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_INPROGRESS_BUSINESS_CHANGE);

                    drHeader[0]["Docstatus"] = SERVICE_DOC_STATUS_INPROGRESS;
                    drHeader[0]["CallStatus"] = ServiceTicketLibrary.SERVICE_CALL_STATUS_OPEN;
                    drHeader[0]["CancelDate"] = "";
                    drHeader[0]["CancelTime"] = "";
                    drHeader[0]["CancelBy"] = "";

                }

                foreach (DataRow drItem in dtItem.Rows)
                {
                    drItem["CloseStatus"] = "01";
                    drItem["ResolutionOnDate"] = "";
                    drItem["ResolutionOnTime"] = "";
                    drItem["ClosedOnDate"] = "";
                    drItem["ClosedOnTime"] = "";
                }

                saveAssignCancelCloseResponseResolution("1500545", "Re-Open Ticket", ReOpenBy);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void saveUpdateTicketDocStatus(string DocStatusCode)
        {
            string ticketType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
            string ticketNo = serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString();
            string ticketYear = serviceCallEntity.cs_servicecall_header.Rows[0]["FiscalYear"].ToString();
            saveUpdateTicketDocStatus(DocStatusCode, ticketType, ticketNo, ticketYear);
        }
        protected void saveUpdateTicketDocStatus(string DocStatusCode, string ticketType, string ticketNo, string ticketYear)
        {
            try
            {
                AfterSaleService.getInstance().UpdateStatus(SID, CompanyCode, DocStatusCode, ticketType, ticketYear, ticketNo,
                    UserName, Validation.getCurrentServerStringDateTime());

                _txt_docstatusTran.Value = GetDocStatusDesc(DocStatusCode);
                ddlTicketStatus_Change.SelectedValue = DocStatusCode;
                _txt_TicketStatusTran.Value = DocStatusCode + " : " +
                    ServiceTicketLibrary.GetTicketDocStatusDesc(
                    SID,
                    CompanyCode,
                    DocStatusCode
                );
                udpTicketStatusTran.Update();

                getdataToedit(ticketType, ticketNo, ticketYear, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void saveCloseCall2(string closedate, string closetime, string closeby, string closecomment, string close_status)
        {
            try
            {
                string comcode = CompanyCode;
                string lineno = (Session["responsecall_lineno" + idGen] != null ? Session["responsecall_lineno" + idGen].ToString() : "");
                string _Close_on = (closedate != "" ? Validation.Convert2DateDisplay(closedate).ToString() : "");
                string _Close_ontime = (closetime != "" ? Validation.Convert2TimeDisplay(closetime).ToString() : "");

                if (!(string.IsNullOrEmpty(comcode) && string.IsNullOrEmpty(lineno)))
                {
                    saveAssignCancelCloseResponseResolution("1500546", "Close Ticket", closeby);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void saveAssignCancelCloseResponseResolution(string ReflectionCode, string _Message, string empcode,
            tmpServiceCallDataSet dsServiceCall = null)
        {
            try
            {
                if (dsServiceCall == null)
                {
                    dsServiceCall = (tmpServiceCallDataSet)serviceCallEntity.Copy();
                }

                if (!string.IsNullOrEmpty(ReflectionCode))
                {
                    DataSet objReturn = new DataSet();
                    string returnMessage = "";

                    string sessionid = (string)Session[ApplicationSession.USER_SESSION_ID];
                    Object[] objParam = new Object[] { ReflectionCode, sessionid };
                    DataSet[] objDataSet = new DataSet[] { dsServiceCall };
                    objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);

                    returnMessage = (objReturn.Tables["MessageResult"] == null || objReturn.Tables["MessageResult"].Rows.Count <= 0)
                         ? "" : objReturn.Tables["MessageResult"].Rows[0]["Message"].ToString();

                    if (returnMessage.ToString() != "")
                    {
                        throw new Exception(_Message + " Error : " + returnMessage);
                    }
                    else
                    {
                        if (_Message != "")
                        {
                            string message = _Message + " Success.";

                            Session["SCF_SAVESUCCESS" + idGen] = message;
                        }
                    }
                }
                else
                {
                    throw new Exception("กรุณาทำการบันทึกเอกสารก่อนครับ/ค่ะ");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCloseWork_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (sender as Button);
                string SERVICE_DOC_STATUS_INPROGRESS = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_INPROGRESS_BUSINESS_CHANGE);
                if (hddTicketStatus_Old.Value != SERVICE_DOC_STATUS_INPROGRESS)
                {
                    ClientService.DoJavascript("focusRemarkBox(true);");
                    throw new Exception("Please update status to In progress before Resolve!");
                }

                HiddenField hddTier = (sender as Button).Parent.FindControl("hddTier") as HiddenField;

                DataRow[] drsTier = dtTier.Select("Tier='" + hddTier.Value + "'");

                string tierDesc = "";

                if (drsTier.Length > 0)
                {
                    tierDesc = drsTier[0]["TierDescription"].ToString();
                }

                string ticketType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
                string ticketNo = serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString();
                string ticketYear = serviceCallEntity.cs_servicecall_header.Rows[0]["FiscalYear"].ToString();

                AfterSaleService.getInstance().ResolveTicket(SID, CompanyCode,
                    ticketType, ticketNo, ticketYear, tierDesc, EmployeeCode, FullNameEN
                );

                string displayTicketNo = AfterSaleService.getInstance().GetTicketNoForDisplay(SID, CompanyCode, ticketType, ticketNo);

                Session["SCF_SAVESUCCESS" + idGen] = "Ticket No. " + displayTicketNo + " has been resolved by \"" + tierDesc.Trim() + "\"";

                getdataToedit(ticketType, ticketNo, ticketYear);

                NotificationLibrary.GetInstance().TicketAlertEvent(
                    NotificationLibrary.EVENT_TYPE.TICKET_RESOLVE,
                    SID,
                    CompanyCode,
                    hddDocnumberTran.Value,
                    EmployeeCode,
                    ThisPage
                );

                if (chkIsLoad_TicketChangeLog.Checked)
                    bindDataChangeLog();
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

        public string styleMatBoxHasActivity(string aobjectlink)
        {
            string style = "padding-bottom:0px;";

            if (!string.IsNullOrEmpty(aobjectlink) && hddDocnumberTran.Value != "")
            {
                if (aobjectlink == lastAobject)
                {
                    style += "background-color: #FFFDE7;";
                }
            }
            return style;
        }

        public string displayContantCustomer(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = "<span style='color:#aaa;'>ไม่ได้ระบุ</span>";
            }

            return text;
        }

        #region Workflow
        private string CreateWorkflow(string docnumber, String remark)
        {
            string DateIn = Validation.getCurrentServerStringDateTime().Substring(0, 8);

            WorkflowHeaderModel enWorkflowHeader = new WorkflowHeaderModel
            {
                EMPCODE = EmployeeCode,
                EMPNAME = FullNameEN,
                EMPSURNAME = "",
                DOCNUMBER = docnumber,
                PROJECTCODE = "",
                SUBPROJECT = ddlAccountability.SelectedValue,
                JOBDESCRIPTION = "Start Work Flow : " + DateIn,
                REMARKS = remark,
                CREATED_BY = EmployeeCode,
                CREATED_ON = Validation.getCurrentServerStringDateTime()
            };

            string aobjectlink = libWorkFlow.CreatedWorkflowHeader(
                SID,
                CompanyCode,
                DateIn,
                enWorkflowHeader
            );


            return aobjectlink;
        }

        protected void btnRebindWorkFlow_Click(object sender, EventArgs e)
        {
            try
            {
                string InitiativeCode = BindDataWorkflow();
                if (chkIsLoad_TicketChangeLog.Checked)
                    bindDataChangeLog();

                ApprovalHeader en = WorkflowService.getInstance().GetApprovalHeaderPresentObject(
                    SID,
                    CompanyCode,
                    "",
                    InitiativeCode
                );
                if (en.WorkflowSuccess)
                {

                    string DocStatusStart = ServiceTicketLibrary.GetInstance().GetTicketStatusFromEvent(
                        SID,
                        CompanyCode,
                        ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE_BUSINESS_CHANGE
                    );
                    saveUpdateTicketDocStatus(DocStatusStart);
                }
                else
                {
                    string DocStatusStart = ServiceTicketLibrary.GetInstance().GetTicketStatusFromEvent(
                        SID,
                        CompanyCode,
                        ServiceTicketLibrary.TICKET_STATUS_EVENT_IMPLEMENT_BUSINESS_CHANGE
                    );
                    if (DocStatusStart != "")
                    {
                        saveUpdateTicketDocStatus(DocStatusStart);
                    }
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

        private string BindDataWorkflow()
        {
            #region Workflow
            // Workflow
            string AobjLink = libWorkFlow.getActivityAobjectlinkRefWorkflow(
                "",
                "", //WorkGroupCode, 
                hddDocnumberTran.Value
            );

            DataTable dtWF = libWorkFlow.getWorkflow(
                "",
                "", //WorkGroupCode, 
                AobjLink
            );
            if (dtWF.Rows.Count > 0)
            {
                ddlAccountability.SelectedValue = Convert.ToString(dtWF.Rows[0]["SUBPROJECT"]);
                ApprovalProcedureControl.BindData(AobjLink, "");


                string[] StateGate_Cur = libWorkFlow.GetApprovalHeaderPresent(
                    SID,
                    CompanyCode,
                    "", //WorkGroupCode,
                    AobjLink
                ).Split(',');

                if (StateGate_Cur.Length > 0)
                {
                    ApproveStateGateControl.WorkGroupCode = "";
                    ApproveStateGateControl.InitiativeCode = AobjLink;
                    ApproveStateGateControl.StategateCode = StateGate_Cur[0];
                    ApproveStateGateControl.Trig();
                    ApproveStateGateControl.TicketCode = hddDocnumberTran.Value;
                }

                ApprovalHeader StateGate = libWorkFlow.GetApprovalHeaderPresentObject(
                    SID,
                    CompanyCode,
                    "", //WorkGroupCode,
                    AobjLink
                );

                if (!string.IsNullOrEmpty(StateGate.StategateFrom))
                {
                    DataTable dtStategate = libWorkFlow.getInitiativeApproveHeader(SID, AobjLink);
                    if (StateGate.ApproveStarted)
                    {
                        tbWorkFlowStatus.Text = "Wait for " + libWorkFlow.getDescriptionEventObject(
                            StateGate.StategateTo
                        ) + " approval (Step Up)";

                        preperListParticipantsApproval(AobjLink, StateGate.StategateFrom, StateGate.StategateTo);
                    }
                    else if (StateGate.RequestApprovaDowngrade)
                    {
                        tbWorkFlowStatus.Text = "Wait for " + libWorkFlow.getDescriptionEventObject(
                            StateGate.StategateTo
                        ) + " approval (Step Down)";

                        preperListParticipantsApproval(AobjLink, StateGate.StategateFrom, StateGate.StategateTo);
                    }
                    else
                    {
                        if (StateGate.StategateFrom == "L0" && !StateGate.ApproveStatus)
                        {
                            tbWorkFlowStatus.Text = "Workflow Not Start.";
                        }
                        else if (StateGate.WorkflowSuccess)
                        {
                            tbWorkFlowStatus.Text = "Workflow Success";
                        }
                        else
                        {
                            tbWorkFlowStatus.Text = libWorkFlow.getDescriptionEventObject(
                                StateGate.StategateFrom
                            ) + " approved";
                        }
                    }
                }
                else
                {
                    tbWorkFlowStatus.Text = "Workflow Success";
                }

                udpWorkflowDetail.Update();
                udpWorkFlowStatus.Update();
                udpApprovalStatus.Update();
            }
            #endregion
            return AobjLink;
        }

        private void bindDataAccountability()
        {
            DataTable dtAcc = accountabilityService.getAccountabilityStructureV2(SID, "");
            DataTable dtDocMapAcc = Accountability.Service.DocTypeMapAccountabilityService.getInstance().GetDocumentTypeMappingAccountabilityDataByDocTypeCode(SID,CompanyCode, serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString());
            DataTable dt = new DataTable();
            dt.Columns.Add("DataText");
            dt.Columns.Add("DataValue");
            foreach (DataRow row in dtDocMapAcc.Rows) {
                DataRow[] dtr =  dtAcc.Select("DataValue = '"+row["AccountabilityCode"]+"'");
                if (dtr.Count() > 0 ) {
                    dt.Rows.Add(dtr[0]["DataText"].ToString(), dtr[0]["DataValue"].ToString());
                }
               
            }

            ddlAccountability.DataSource = dt;
            ddlAccountability.DataTextField = "DataText";
            ddlAccountability.DataValueField = "DataValue";
            ddlAccountability.DataBind();
            ddlAccountability.Items.Insert(0, new ListItem("Please select", ""));

            ddlAccountability.Enabled = mode_stage == ApplicationSession.CREATE_MODE_STRING;
            ddlAccountability.SelectedValue = "";

            panelAccountability.Visible = AlowWorkFlow;
        }

        private void preperListParticipantsApproval(string AobjLink, string StategateFrom, string StategateTo)
        {
            DataTable dtParticipants = libWorkFlow.getApprovalParticipantsForApprovalProcedure(
                SID,
                CompanyCode,
                "",
                AobjLink,
                StategateFrom,
                StategateTo,
                "",
                "",
                "",
                ddlAccountability.SelectedValue
            );

            List<string> listParticipants = new List<string>();
            foreach (DataRow dr in dtParticipants.Select("Status = 'WAITTING'"))
            {
                listParticipants.Add(dr["fullnameEN"].ToString());
            }
            if (listParticipants.Count > 0)
            {
                tbApprovalStatus.Text = string.Join(", ", listParticipants);
            }
            else
            {
                tbApprovalStatus.Text = "-";
            }
        }
        #endregion

        public string ConvertToTime(string time)
        {
            return ConvertToTime(time, false);
        }

        public string ConvertToTime(string time, bool returnEmpty)
        {
            if (!string.IsNullOrEmpty(time))
            {
                double xTime = 0;
                double.TryParse(time, out xTime);

                if (xTime > 0)
                {
                    TimeSpan t = TimeSpan.FromSeconds(xTime);

                    string answer = "";
                    if (t.Days > 0)
                    {
                        answer += t.Days + " วัน ";
                    }
                    if (t.Days > 0 || t.Hours > 0)
                    {
                        answer += t.Hours + " ชม ";
                    }
                    if (t.Days > 0 || t.Hours > 0 || t.Minutes > 0)
                    {
                        answer += t.Minutes + " นาที ";
                    }
                    if (t.Days > 0 || t.Hours > 0 || t.Minutes > 0 || t.Seconds > 0)
                    {
                        answer += t.Seconds + " วินาที ";
                    }

                    return answer;
                }
            }

            if (returnEmpty)
            {
                return "";
            }

            return "ไม่กำหนด";
        }

        public string DisplayTicketDateTime(string ticketCode, string dateTime)
        {
            if (!string.IsNullOrEmpty(ticketCode) && !string.IsNullOrEmpty(dateTime) && dateTime.Length == 14)
            {
                return Validation.Convert2DateTimeDisplay(dateTime).Substring(0, 16);
            }

            return "-";
        }

        #region CI
        private void bindDataCI()
        {
            List<string> listEquipment = new List<string>();
            foreach (DataRow drCI in serviceCallEntity.cs_servicecall_item.Rows)
            {
                listEquipment.Add(Convert.ToString(drCI["EquipmentNo"]));
            }
            DataTable dtCI = libCI.getEquipmentDetail(SID, CompanyCode, listEquipment);
            rptEquipment.DataSource = dtCI;
            rptEquipment.DataBind();
            udpLiatCI.Update();
        }

        protected void btnAddNewEquipment_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(AutoCompleteEquipment.SelectedValue))
                {
                    return;
                }
                addEquipment(AutoCompleteEquipment.SelectedValue);
                bindDataCI();
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

        
        protected void btnRemoveItemEquipment_Click(object sender, EventArgs e)
        {
            try
            {
                ClientService.AGLoading(true);
                string EquipmentCode = (sender as Button).CommandArgument;
                removeEquipment(EquipmentCode);
                SearchHelpCIControl.removeCIformSearchwithButton(EquipmentCode);
                bindDataCI();
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

        private void addEquipment(string EquipmentCode)
        {
            if (serviceCallEntity.cs_servicecall_item.Select("EquipmentNo = '" + EquipmentCode + "'").Count() > 0)
            {
                return;
                //throw new Exception("CI นี้ได้ถูกเลือกแล้ว");
            }

            DataTable EQInfo = AfterSaleService.getInstance().getSearchEQInfo("", EquipmentCode);
            string currDate = Validation.getCurrentServerStringDateTime().Substring(0, 8);
            string currTime = Validation.getCurrentServerStringDateTime().Substring(8, 6);
            string CustomerCode = serviceCallEntity.cs_servicecall_header.Rows[0]["CustomerCode"].ToString();
            string DocType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
            string Fiscalyear = serviceCallEntity.cs_servicecall_header.Rows[0]["Fiscalyear"].ToString();
            string CallerID = serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString();
            string ObjectID = serviceCallEntity.cs_servicecall_header.Rows[0]["ObjectID"].ToString();
            int xCount = 0;
            if (serviceCallEntity.cs_servicecall_item.Count > 0)
            {
                int.TryParse(serviceCallEntity.cs_servicecall_item.OrderByDescending(o => o.xLineNo).First().xLineNo, out xCount);
                xCount++;
            }
            else
            {
                xCount = serviceCallEntity.cs_servicecall_item.Rows.Count + 1;
            }

            DataRow[] dataRowEmpty = serviceCallEntity.cs_servicecall_item.Select("EquipmentNo = ''");
            if (dataRowEmpty.Count() == 0)
            {
                DataRow drNew = serviceCallEntity.cs_servicecall_item.NewRow();
                string xLineNo = xCount.ToString().PadLeft(3, '0');
                drNew["SID"] = SID;
                drNew["CompanyCode"] = CompanyCode;
                drNew["CustomerCode"] = CustomerCode;
                drNew["DocType"] = DocType;
                drNew["Fiscalyear"] = Fiscalyear;
                drNew["CallerID"] = "";//CallerID;
                drNew["ObjectID"] = ObjectID;
                drNew["xLineNo"] = xLineNo;
                drNew["BObjectID"] = ObjectID + xLineNo;
                drNew["CloseStatus"] = "01";
                drNew["CreatedOnDate"] = currDate;
                drNew["CreatedOnTime"] = currTime;
                drNew["ProblemDate"] = currDate;
                drNew["ProblemTime"] = currTime;
                drNew["CREATED_BY"] = EmployeeCode;
                drNew["CREATED_ON"] = currDate + currTime;
                drNew["RefContractNo"] = "";
                drNew["ClosedOnDate"] = "";
                drNew["ClosedOnTime"] = "";
                drNew["EndDate"] = "";
                drNew["AssignDate"] = "";
                drNew["AssignTime"] = "";
                drNew["ResponseOnDate"] = "";
                drNew["ResponseOnTime"] = "";
                drNew["ResolutionOnDate"] = "";
                drNew["ResolutionOnTime"] = "";
                drNew["TechnicianDate"] = "";
                drNew["TechnicianTime"] = "";

                drNew["ProblemGroup"] = ddlProblemGroup.SelectedValue;
                drNew["ProblemTypeCode"] = ddlProblemType.SelectedValue;
                drNew["OriginCode"] = ddlProblemSource.SelectedValue;
                drNew["CallTypeCode"] = ddlServiceSource.SelectedValue;
                drNew["QueueOption"] = ddlOwnerGroupService.SelectedValue;
                drNew["Remark"] = tbEquipmentRemark.Text;
                /* drNew["SummaryProblem"] = tbSummaryProblem.Text;
                 drNew["SummaryCause"] = tbSummaryCause.Text;
                 drNew["SummaryResolution"] = tbSummaryResolution.Text;*/
                drNew["IncidentArea"] = hddIncidentAreaCode.Value;

                foreach (DataRow drCI_Info in EQInfo.Select("EquipmentCode = '" + EquipmentCode + "'"))
                {
                    drNew["ItemCode"] = Convert.ToString(drCI_Info["MaterialNo"]);
                    drNew["MaterialDesc"] = Convert.ToString(drCI_Info["MaterialName"]);
                    drNew["EquipmentNo"] = EquipmentCode;
                    drNew["EquipmentDesc"] = Convert.ToString(drCI_Info["EquipmentName"]);
                }
                serviceCallEntity.cs_servicecall_item.Rows.Add(drNew);
            }
            else
            {
                dataRowEmpty[0]["CreatedOnDate"] = currDate;
                dataRowEmpty[0]["CreatedOnTime"] = currTime;
                dataRowEmpty[0]["ProblemDate"] = currDate;
                dataRowEmpty[0]["ProblemTime"] = currTime;
                dataRowEmpty[0]["CREATED_BY"] = EmployeeCode;
                dataRowEmpty[0]["CREATED_ON"] = currDate + currTime;

                dataRowEmpty[0]["ProblemGroup"] = ddlProblemGroup.SelectedValue;
                dataRowEmpty[0]["ProblemTypeCode"] = ddlProblemType.SelectedValue;
                dataRowEmpty[0]["OriginCode"] = ddlProblemSource.SelectedValue;
                dataRowEmpty[0]["CallTypeCode"] = ddlServiceSource.SelectedValue;
                dataRowEmpty[0]["QueueOption"] = ddlOwnerGroupService.SelectedValue;
                dataRowEmpty[0]["Remark"] = tbEquipmentRemark.Text;
                dataRowEmpty[0]["IncidentArea"] = hddIncidentAreaCode.Value;

                foreach (DataRow drCI_Info in EQInfo.Select("EquipmentCode = '" + EquipmentCode + "'"))
                {
                    dataRowEmpty[0]["ItemCode"] = Convert.ToString(drCI_Info["MaterialNo"]);
                    dataRowEmpty[0]["MaterialDesc"] = Convert.ToString(drCI_Info["MaterialName"]);
                    dataRowEmpty[0]["EquipmentNo"] = EquipmentCode;
                    dataRowEmpty[0]["EquipmentDesc"] = Convert.ToString(drCI_Info["EquipmentName"]);
                }
            }
        }

        private void removeEquipment(string EquipmentCode)
        {
            bool IsRemoved = false;
            for (int i = serviceCallEntity.cs_servicecall_item.Rows.Count - 1; i >= 0; i--)
            {
                if (IsRemoved)
                {
                    continue;
                }
                DataRow dr = serviceCallEntity.cs_servicecall_item.Rows[i];
                if (dr["EquipmentNo"].ToString() == EquipmentCode)
                {
                    serviceCallEntity.cs_servicecall_item.Rows[i].Delete();      
                    return;
                }
            }
        }

        private void PrepareEquipment()
        {
            string currDate = Validation.getCurrentServerStringDateTime().Substring(0, 8);
            string currTime = Validation.getCurrentServerStringDateTime().Substring(8, 6);
            string CustomerCode = serviceCallEntity.cs_servicecall_header.Rows[0]["CustomerCode"].ToString();
            string DocType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
            string Fiscalyear = serviceCallEntity.cs_servicecall_header.Rows[0]["Fiscalyear"].ToString();
            string CallerID = serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString();
            string ObjectID = serviceCallEntity.cs_servicecall_header.Rows[0]["ObjectID"].ToString();

            Dictionary<string, string> listItemEquipment = new Dictionary<string, string>();
            foreach (RepeaterItem item in rptEquipment.Items)
            {
                HiddenField txtEquipmentNo = item.FindControl("hddEquipmentCode") as HiddenField;
                TextBox tbSerialNo = item.FindControl("tbSerialNo") as TextBox;
                listItemEquipment.Add(txtEquipmentNo.Value, tbSerialNo.Text);
            }

            int xCount = 0;
            foreach (DataRow dr in serviceCallEntity.cs_servicecall_item.Rows)
            {
                xCount++;
                string xLineNo = xCount.ToString().PadLeft(3, '0');
                string CI_Code = dr["EquipmentNo"].ToString();

                dr["xLineNo"] = xLineNo;
                dr["BObjectID"] = ObjectID + xLineNo;
                dr["CreatedOnDate"] = currDate;
                dr["CreatedOnTime"] = currTime;
                dr["ProblemDate"] = currDate;
                dr["ProblemTime"] = currTime;
                dr["CREATED_ON"] = currDate + currTime;
                dr["ProblemGroup"] = ddlProblemGroup.SelectedValue;
                dr["ProblemTypeCode"] = ddlProblemType.SelectedValue;
                dr["OriginCode"] = ddlProblemSource.SelectedValue;
                dr["CallTypeCode"] = ddlServiceSource.SelectedValue;
                dr["QueueOption"] = ddlOwnerGroupService.SelectedValue;
                dr["Remark"] = tbEquipmentRemark.Text;
                /*dr["SummaryProblem"] = tbSummaryProblem.Text;
                dr["SummaryCause"] = tbSummaryCause.Text;
                dr["SummaryResolution"] = tbSummaryResolution.Text;*/
                dr["IncidentArea"] = hddIncidentAreaCode.Value;

                //string[] strstartdate = ((tbStartDate.Text).ToString()).Split('/');
                //string newstrstartdate = strstartdate[2] + strstartdate[1] + strstartdate[0];
                //string[] strenddate = ((tbEndDate.Text).ToString()).Split('/');
                //string newstrenddate = strstartdate[2] + strstartdate[1] + strstartdate[0];

                string newstrstartdate = "";
                string newstrenddate = "";
                string newstarttime = "";
                string newendtime = "";
                if (!string.IsNullOrEmpty(tbStartDate.Text))
                {
                    newstrstartdate = Validation.Convert2DateDB(tbStartDate.Text);
                }
                if (!string.IsNullOrEmpty(tbEndDate.Text))
                {
                    newstrenddate = Validation.Convert2DateDB(tbEndDate.Text);
                }

                if (!string.IsNullOrEmpty(txtStartTime.Text))
                {
                    newstarttime = Validation.Convert2TimeDB(txtStartTime.Text);
                }
                if (!string.IsNullOrEmpty(txtEndTime.Text))
                {
                    newendtime = Validation.Convert2TimeDB(txtEndTime.Text);
                }
                dr["PlanStartDate"] = newstrstartdate;
                dr["PlanEndDate"] = newstrenddate;
                dr["PlanStartTime"] = newstarttime;
                dr["PlanEndTime"] = newendtime;

                var dataSerialNo = listItemEquipment.Where(w => w.Key.Equals(CI_Code));
                if (dataSerialNo.Count() > 0)
                {
                    dr["SerialNo"] = dataSerialNo.First().Value.Trim();
                }
                else
                {
                    dr["SerialNo"] = "";
                }
            }

            /*if (serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
            {
                serviceCallEntity.cs_servicecall_header.Rows[0]["AffectSLA"] = ddlAffectSLA.SelectedValue;
            }*/
        }
        #endregion

        #region Change Area Selected

        protected void btnChangeProblemGroup_Click(object sender, EventArgs e)
        {
            try
            {
                GetProblemType(ddlProblemGroup.SelectedValue);
                GetProblemSource(ddlProblemGroup.SelectedValue, ddlProblemType.SelectedValue);
                GetServiceSource(ddlProblemGroup.SelectedValue, ddlProblemType.SelectedValue, ddlProblemSource.SelectedValue);
                bindTierOperationRefIncidentArea();
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
                SetOwnerGroupServiceRefIncidentArea(ddlProblemGroup.SelectedValue, ddlProblemType.SelectedValue);
                bindTierOperationRefIncidentArea();
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
                bindTierOperationRefIncidentArea();
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
                bindTierOperationRefIncidentArea();
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

        private void bindTierOperationRefIncidentArea()
        {
            string IncidentAreaCode = ddlProblemGroup.SelectedValue + ddlProblemType.SelectedValue + ddlProblemSource.SelectedValue + ddlServiceSource.SelectedValue;
            if (!hddIncidentAreaCode.Value.Equals(IncidentAreaCode))
            {
                hddIncidentAreaCode.Value = IncidentAreaCode;
                udpUpdateAreaCode.Update();
                ddlOwnerGroupService_SelectedIndexChanged(null, null);
            }
        }
        #endregion

        private void GetSeverity()
        {
            string impactCode = ddlImpact.SelectedValue;
            string urgencyCode = ddlUrgency.SelectedValue;

            DataTable dt = lib.GetSeverity(SID, "", impactCode, urgencyCode);

            _ddl_priorityTran.DataSource = dt;
            _ddl_priorityTran.DataBind();

            if (dt.Rows.Count > 0)
            {
                _ddl_priorityTran.SelectedIndex = 0;
            }
            udpnSeverity.Update();
        }

        protected void ddlImpact_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetSeverity();
                ddlOwnerGroupService_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        protected void ddlUrgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetSeverity();
                ddlOwnerGroupService_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        protected void btnReOpenTicket_Click(object sender, EventArgs e)
        {
            try
            {
                string mode = (sender as Button).CommandArgument;

                ResetTicket(mode);
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

        #region SLA Event
        protected void btnCreateNewTicketRef_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_ddl_sctype.SelectedValue))
                {
                    throw new Exception("กรุณาระบุ Ticket Type");
                }
                if (string.IsNullOrEmpty(_txt_year.Value))
                {
                    throw new Exception("กรุณาระบุ Fiscal Year");
                }

                if (!string.IsNullOrEmpty(hddEquepmentCodeRef.Value))
                {
                    string equipmentCode = hddEquepmentCodeRef.Value;
                    string customerCode = CustomerSelect.SelectedValue;

                    DataTable dtCheckEquipment = universalService.GetEquipmentCustomerAssignment(SID, CompanyCode,
                        equipmentCode, customerCode);

                    if (dtCheckEquipment.Rows.Count == 0)
                    {
                        throw new Exception("Equipment " + equipmentCode + " is not assigned to customer " + customerCode + " : " + GetCustomerDesc(customerCode));
                    }

                    CreatedNewTicket(equipmentCode, true);
                }
                else
                {
                    CreatedNewTicket(null, true);
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

        protected void btnSelectIncidentArea_Click(object sender, EventArgs e)
        {
            try
            {
                ddlOwnerGroupService_SelectedIndexChanged(null, null);
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

        protected void btnUpdateStatus_FormPostRemark_Click(object sender, EventArgs e)
        {
            try
            {
                if (hddTicketStatus_Old.Value != hddTicketStatus_New.Value)
                {
                    string SERVICE_DOC_STATUS_RESOLVE = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE_BUSINESS_CHANGE);


                    saveUpdateTicketDocStatus(hddTicketStatus_New.Value);
                    if (hddTicketStatus_Old.Value == SERVICE_DOC_STATUS_RESOLVE)
                    {
                        lib.reOpenTicketTask_SLA(SID, CompanyCode, hddDocnumberTran.Value);
                        ClientService.DoJavascript("focusRemarkBox(true);");
                    }

                    hddTicketStatus_Old.Value = hddTicketStatus_New.Value;
                    hddTicketStatus.Value = hddTicketStatus_New.Value;

                    NotificationLibrary.GetInstance().TicketAlertEvent(
                        NotificationLibrary.EVENT_TYPE.TICKET_UPDATESTATUS,
                        SID,
                        CompanyCode,
                        hddDocnumberTran.Value,
                        EmployeeCode,
                        ThisPage
                    );

                    udpHiddenCode.Update();
                }

                if (chkIsLoad_TicketChangeLog.Checked)
                    bindDataChangeLog();
                if (chkIsLoad_Attachment.Checked)
                    BindDataLogFileAttachment();
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

        protected void btnCloseTicket_Click(object sender, EventArgs e)
        {
            try
            {

                string DocStatusResolve = ServiceTicketLibrary.GetInstance().GetTicketStatusFromEvent(
                    SID,
                    CompanyCode,
                    ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE_BUSINESS_CHANGE
                );
                if (DocStatusResolve != hddTicketStatus.Value)
                {
                    throw new Exception("Please approve workflow before close change order.");
                }

                List<logValue_OldNew> enLog = new List<logValue_OldNew>();

                #region close servicecall
                string canceldate = Validation.getCurrentServerStringDateTime().Substring(0, 8);
                string canceltime = Validation.getCurrentServerDateTime().ToString("HHmmss");
                string cancelby = EmployeeCode;
                string cancelcomment = txtRemarkCloseTicket.Text; //_txt_popup_remark.InnerText;
                string close_status = ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE;

                DataTable dtHeader = serviceCallEntity.cs_servicecall_header;
                //DataTable dtItem = serviceCallEntity.cs_servicecall_item;
                //DataRow[] drcurrentRow = dtItem.Select("xLineNo='" + (string)Session["responsecall_lineno" + idGen] + "'");

                foreach (DataRow dr in serviceCallEntity.cs_servicecall_item.Rows)
                {
                    dr["ClosedOnDate"] = canceldate;
                    dr["ClosedOnTime"] = canceltime;
                    dr["CloseComment"] = cancelcomment;
                    dr["CloseBy"] = cancelby;
                    dr["CloseStatus"] = close_status;
                }

                //if (drcurrentRow.Length != 0)
                //{
                //    drcurrentRow[0]["ClosedOnDate"] = canceldate;
                //    drcurrentRow[0]["ClosedOnTime"] = canceltime;
                //    drcurrentRow[0]["CloseComment"] = cancelcomment;
                //    drcurrentRow[0]["CloseBy"] = cancelby;
                //    drcurrentRow[0]["CloseStatus"] = close_status;
                //}

                bool isopen = false;

                foreach (DataRow dr in serviceCallEntity.cs_servicecall_item.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        if ("".Equals(dr["CloseStatus"].ToString()) || "01".Equals(dr["CloseStatus"].ToString()))
                        {
                            isopen = true;
                        }
                    }
                }

                if (!isopen)
                {
                    string SERVICE_DOC_STATUS_CLOSED = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CLOSED_BUSINESS_CHANGE);

                    dtHeader.Rows[0]["CallStatus"] = ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE;
                    dtHeader.Rows[0]["Docstatus"] = SERVICE_DOC_STATUS_CLOSED;
                    dtHeader.Rows[0]["UPDATED_BY"] = cancelby;
                    dtHeader.Rows[0]["UPDATED_ON"] = Validation.getCurrentServerStringDateTime();
                }

                PrepareEquipment();
                saveCloseCall2(canceldate, canceltime, cancelby, cancelcomment, close_status);
                #endregion

                enLog.Add(new logValue_OldNew
                {
                    Value_Old = "",
                    Value_New = "Close Ticket",
                    TableName = "",
                    FieldName = "",
                    AccessCode = LogServiceLibrary.AccessCode_Change
                });
                SaveLog(enLog);

                NotificationLibrary.GetInstance().TicketAlertEvent(
                    NotificationLibrary.EVENT_TYPE.TICKET_CLOSE,
                    SID,
                    CompanyCode,
                    hddDocnumberTran.Value,
                    EmployeeCode,
                    ThisPage
                );

                getdataToedit((string)Session["SCT_created_doctype_code" + idGen], hddDocnumberTran.Value.Trim(), (string)Session["SCT_created_fiscalyear" + idGen]);
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

        #region Authen
        private bool IsAuthen(DataTable dtMain, DataTable dtParticipant, string AuthenType)
        {
            bool IsAuthen = false;
            string Condition = "EmployeeCode = '" + EmployeeCode + "' and " + AuthenType + " = 'TRUE'";
            foreach (DataRow dr in dtMain.Select(Condition))
            {
                IsAuthen = true;
            }
            foreach (DataRow dr in dtParticipant.Select(Condition))
            {
                IsAuthen = true;
            }

            return IsAuthen;
        }
        #endregion

        #region Bind Data Change Log & Save Log
        protected void btnUpdateLog_FormPostRemark_Click(object sender, EventArgs e)
        {
            try
            {
                NotificationLibrary.GetInstance().TicketAlertEvent(
                    NotificationLibrary.EVENT_TYPE.TICKET_COMMENT,
                    SID,
                    CompanyCode,
                    hddDocnumberTran.Value,
                    EmployeeCode,
                    ThisPage
                );

                if (chkIsLoad_TicketChangeLog.Checked)
                    bindDataChangeLog();
                if (chkIsLoad_Attachment.Checked)
                    BindDataLogFileAttachment();
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

        private void bindDataChangeLog()
        {
            if (serviceCallEntity.cs_servicecall_header == null || serviceCallEntity.cs_servicecall_header.Rows.Count <= 0)
            {
                return;
            }

            string DocType = "";
            string fiscalyear = "";
            string docnumber = "";

            foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
            {
                DocType = dr["DocType"].ToString();
                fiscalyear = dr["FiscalYear"].ToString();
                docnumber = dr["CallerID"].ToString();
            }

            DataTable dt = libLog.GetTicketLog(SID, CompanyCode, docnumber, fiscalyear, DocType);

            string AobjLink = libWorkFlow.getActivityAobjectlinkRefWorkflow(
                "",
                "", //WorkGroupCode, 
                hddDocnumberTran.Value
            );
            DataTable dtApprovalProcedureLog = WorkflowService.getInstance().getInitiativeApproveLog(AobjLink);
            foreach (DataRow drWF in dtApprovalProcedureLog.Rows)
            {
                DataRow drNew = dt.NewRow();
                string msgWF = ApprovalProcedureControl.controlMassage(
                    Convert.ToString(drWF["Event"]),
                    Convert.ToString(drWF["ApproveType"])
                );
                string DateTimeWF = Validation.Convert2DateTimeDisplay(Convert.ToString(drWF["created_on"]));
                string dateWF = DateTimeWF.Split(' ')[0];
                string timeWF = DateTimeWF.Split(' ')[1];

                drNew["objpkrec"] = "";
                drNew["accesscode"] = "Workflow Message";
                drNew["access_by"] = Convert.ToString(drWF["FullnameEN"]);
                drNew["access_date"] = dateWF;
                drNew["access_time"] = timeWF;
                drNew["itemnumber"] = "";
                drNew["sfieldname"] = "";
                drNew["soldvalue"] = "";
                drNew["snewvalue"] = drWF["Message"] + " [Process " + msgWF + "]";
                dt.Rows.Add(drNew);
            }

            foreach (DataRow dr in dt.Rows)
            {
                dr["soldvalue"] = TranslateLog(Convert.ToString(dr["sfieldname"]), Convert.ToString(dr["soldvalue"]));
                dr["snewvalue"] = TranslateLog(Convert.ToString(dr["sfieldname"]), Convert.ToString(dr["snewvalue"]));
            }

            TicketChangeLog.BindingLog(dt);

            udpLogDateTime.Update();
        }

        private void BindDataLogFileAttachment()
        {
            DataTable dt = libLog.getFileAttashmentLog(SID, CompanyCode, hddDocnumberTran.Value);
            rptLogFileAttachment.DataSource = dt;
            rptLogFileAttachment.DataBind();
            udpLogFileAttachment.Update();
            ClientService.DoJavascript("dataTableLogAttachmentFile();");
        }

        public string TranslateLog(string FieldName, string Value)
        {
            switch (FieldName)
            {
                case "Docstatus":
                    Value = Value + " : " + ServiceTicketLibrary.GetTicketDocStatusDesc(SID, CompanyCode, Value);
                    break;
            }
            return Value;
        }

        public void SaveLog(List<logValue_OldNew> enLog)
        {
            try
            {
                if (enLog.Count == 0)
                {
                    return;
                }

                string DocType = "";

                foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
                {
                    DocType = dr["DocType"].ToString();
                }

                List<Main_LogService> en = AfterSaleService.getInstance().SaveLogTicket(SID, DocType, _txt_fiscalyear.Value, hddDocnumberTran.Value, CompanyCode,
                    UserName, enLog);

                if (en.Count > 0)
                {
                    if (chkIsLoad_TicketChangeLog.Checked)
                        bindDataChangeLog();
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

        private void bindDataTicketDateTimeLog()
        {
            string assignDateTime = "";
            string resolveDateTime = "";
            string closeDateTime = "";
            string lastModifiedDateTime = "";
            string stopTime = "";
            string totalTime = "";
            string totalTimeWithoutStop = "";
            string ticketEndDateTime = "";
            double sumStopMinutes = 0;
            string overduetime = "";
            string ticketNo = serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString();

            #region Get assign, resolve, close, EndDatetime
            foreach (DataRow dr in serviceCallEntity.cs_servicecall_item.Rows)
            {
                assignDateTime = dr["AssignDate"].ToString() + dr["AssignTime"].ToString();
                resolveDateTime = dr["ResolutionOnDate"].ToString() + dr["ResolutionOnTime"].ToString();
                closeDateTime = dr["ClosedOnDate"].ToString() + dr["ClosedOnTime"].ToString();
            }
            DataTable dtTicket = AfterSaleService.getInstance().GetEndDateTimelByTicketNumber(SID, CompanyCode, ticketNo);
            if (dtTicket.Rows.Count > 0)
            {
                ticketEndDateTime = dtTicket.Rows[0].Field<String>("EndDateTime").ToString();
            }
            #endregion

            #region Calculate Overdue time
            if (ticketEndDateTime != "" && resolveDateTime != "")
            {
                DateTime overdue = ObjectUtil.ConvertDateTimeDBToDateTime(ticketEndDateTime);
                DateTime resolve = ObjectUtil.ConvertDateTimeDBToDateTime(resolveDateTime);

                TimeSpan ts = resolve - overdue;
                if (ts.TotalSeconds <= 0)
                {
                    overduetime = "0";
                }
                else
                {
                    overduetime = ConvertToTime(ts.TotalSeconds.ToString(), true);
                }
            }
            #endregion

            #region Get last modified
            string ticketType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
       
            string ticketYear = serviceCallEntity.cs_servicecall_header.Rows[0]["FiscalYear"].ToString();

            DataTable dt = libLog.GetTicketLog(SID, CompanyCode, ticketNo, ticketYear, ticketType);

            DataRow[] drr = dt.Select("accesscode <> 'Initial'");

            if (drr.Length > 0)
            {
                lastModifiedDateTime = drr[0]["access_date"] + " " + drr[0]["access_time"];
            }
            #endregion

            #region Get stop
            string where = @" WHERE SID = '" + SID + "' AND CompanyCode = '" + CompanyCode + @"' 
                              AND Doctype = '" + ticketType + "' AND Fiscalyear = '" + ticketYear + "' AND CallerID = '" + ticketNo + "'";

            string sqlStop = "SELECT * FROM cs_servicecall_stop_timer " + where + " ORDER BY xLineNo ASC";

            DataTable dtStop = DBService.selectDataFocusone(sqlStop);

            if (dtStop.Rows.Count > 0)
            {
                DataRow[] drrStopNotStart = dtStop.Select("RestartDate = ''");

                if (drrStopNotStart.Length > 0)
                {
                    string currentDateTime = Validation.getCurrentServerStringDateTime();

                    drrStopNotStart[0]["RestartDate"] = currentDateTime.Substring(0, 8);
                    drrStopNotStart[0]["RestartTime"] = currentDateTime.Substring(8, 6);
                }

                foreach (DataRow drStop in dtStop.Rows)
                {
                    DateTime stop = ObjectUtil.ConvertDateTimeDBToDateTime(drStop["StopDate"].ToString() + drStop["StopTime"].ToString());
                    DateTime restart = ObjectUtil.ConvertDateTimeDBToDateTime(drStop["RestartDate"].ToString() + drStop["RestartTime"].ToString());

                    TimeSpan ts = restart - stop;

                    sumStopMinutes += ts.TotalMinutes;
                }
            }

            stopTime = ConvertToTime(sumStopMinutes.ToString(), true);

            #endregion

            #region Calculate total time
            if (assignDateTime != "" && resolveDateTime != "")
            {
                DateTime assign = ObjectUtil.ConvertDateTimeDBToDateTime(assignDateTime);
                DateTime resolve = ObjectUtil.ConvertDateTimeDBToDateTime(resolveDateTime);

                TimeSpan ts = resolve - assign;

                totalTime = ConvertToTime(ts.TotalMinutes.ToString(), true);
                totalTimeWithoutStop = ConvertToTime((ts.TotalMinutes - sumStopMinutes).ToString(), true);
            }
            #endregion
            // Overdue 
            txtLog_OverdueTime.Text = overduetime;

            // Assign & created
            txtlog_OpenDateTime.Text = assignDateTime == "" ? "" : Validation.Convert2DateTimeDisplay(assignDateTime).Substring(0, 16);
            tbCreatedOn.Text = assignDateTime == "" ? "" : Validation.Convert2DateTimeDisplay(assignDateTime).Substring(0, 16);

            // Resolve
            txtLog_ResolveDateTime.Text = resolveDateTime == "" ? "" : Validation.Convert2DateTimeDisplay(resolveDateTime).Substring(0, 16);

            // Total
            txtLog_TotalTime.Text = totalTime;

            // Stop time
            txtLog_StopTime.Text = stopTime;

            // Last modified
            txtlog_LastModifiedDateTime.Text = lastModifiedDateTime == "" ? "" : lastModifiedDateTime.Substring(0, 16);

            // Close
            txtLog_CloseDateTime.Text = closeDateTime == "" ? "" : Validation.Convert2DateTimeDisplay(closeDateTime).Substring(0, 16);

            // Total without stop
            txtLog_TotalTime_WithoutStop.Text = totalTimeWithoutStop;

            udpLogDateTime.Update();
        }
        #endregion

        #region bind Contect For Add
        protected void MyEventHandlerFunction_afterSaveRebindPostBack(object sender, EventArgs e)
        {
            //GetContact();
            List<ERPW.Lib.Master.ContactEntity> listAddNew = (List<ERPW.Lib.Master.ContactEntity>)sender;
            if (listAddNew != null && listAddNew.Count > 0)
            {
                ERPW.Lib.Master.ContactEntity en = listAddNew.Last();
                if (!string.IsNullOrEmpty(en.NAME1) || !string.IsNullOrEmpty(en.NickName))
                {
                    string BOBJLINK = en.AOBJECTLINK + en.ITEMNO;
                    // _ddl_contact_person.SetValue = BOBJLINK;
                }
            }
            btnContactPersonChange_Click(null, null);
        }

        #endregion

        #region Ticket Relation

        private void getDataRelation()
        {
            if (IsCallTicketStatusClose || IsCallTicketStatusCancel)
            {
                FlowChartDiagramRelationControl.AlowEditMode = false;
            }
            else
            {
                FlowChartDiagramRelationControl.AlowEditMode = true;
            }
            string CustomerCode = hddCustomerCode.Value;
            string TicketType_Problem = ServiceLibrary.LookUpTable(
                "TicketType",
                "ERPW_BUSINESSOBJECT_MAPPING_TICKET_TYPE",
                "WHERE SID = '" + SID + "' AND BusinessObject = '" + ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_PROBLEM + "'"
            );
            if (TicketType_Problem == hddTicketDocType.Value)
            {
                CustomerCode = null;
            }

            LinkFlowChartService.DiagramRelation dataEn = LinkFlowChartService.getDiagramRelation(
                 SID,
                 CompanyCode,
                 hddDocnumberTran.Value,
                 LinkFlowChartService.ItemGroup_TICKET,
                 CustomerCode
             );

            List<string> listTicket = dataEn.chindNode.Select(s => s.ItemCode).ToList();
            DataTable dtDocType = new DBService().selectDataFocusone(
                @"select Doctype from cs_servicecall_header where CallerID in ('" + string.Join("', '", listTicket) + "')"
            );
            List<string> listDocType = dtDocType.AsEnumerable().Select(s => Convert.ToString(s["Doctype"])).ToList();

            DataTable dtPrefix = lib.getDataPrefixDocType(SID, CompanyCode, listDocType);
            dataEn.chindNode.ForEach(r =>
            {
                if (!string.IsNullOrEmpty(r.ItemCode))
                {
                    DataRow[] drr = dtPrefix.Select("'" + r.ItemCode + "' like PrefixCode + '%'");
                    if (drr.Length > 0)
                    {
                        string prefix = drr[0]["PrefixCode"].ToString();

                        string ticketNoDisplay = r.ItemCode;
                        //for (int i = 0; i < prefix.Length; i++)
                        //{
                        //    ticketNoDisplay = ticketNoDisplay.Replace(prefix[i].ToString(), "");
                        //}

                        r.ItemDescription = ServiceTicketLibrary.GetInstance().ReplaceTicketNumberToDisplay(prefix, ticketNoDisplay) + " : " + r.ItemDescription;//  prefix + Convert.ToInt32(ticketNoDisplay) + " : " + r.ItemDescription;
                    }
                }

                if (!string.IsNullOrEmpty(r.ParentItemCode))
                {
                    DataRow[] drr_Parrent = dtPrefix.Select("'" + r.ParentItemCode + "' like PrefixCode + '%'");
                    if (drr_Parrent.Length > 0)
                    {
                        string prefix = drr_Parrent[0]["PrefixCode"].ToString();

                        string ticketNoDisplay = r.ParentItemCode;
                        //for (int i = 0; i < prefix.Length; i++)
                        //{
                        //    ticketNoDisplay = ticketNoDisplay.Replace(prefix[i].ToString(), "");
                        //}

                        //r.ParentItemDescription = prefix + Convert.ToInt32(ticketNoDisplay) + " : " + r.ParentItemDescription;
                        r.ParentItemDescription = ServiceTicketLibrary.GetInstance().ReplaceTicketNumberToDisplay(prefix, ticketNoDisplay) + " : " + r.ItemDescription;//  prefix + Convert.ToInt32(ticketNoDisplay) + " : " + r.ItemDescription;
                    }
                }
            });

            dataEn.parentNode.ForEach(r =>
            {
                if (!string.IsNullOrEmpty(r.ItemCode))
                {
                    DataRow[] drr = dtPrefix.Select("'" + r.ItemCode + "' like PrefixCode + '%'");
                    if (drr.Length > 0)
                    {
                        string prefix = drr[0]["PrefixCode"].ToString();

                        string ticketNoDisplay = r.ItemCode;
                        //for (int i = 0; i < prefix.Length; i++)
                        //{
                        //    ticketNoDisplay = ticketNoDisplay.Replace(prefix[i].ToString(), "");
                        //}

                        //r.ItemDescription = prefix + Convert.ToInt32(ticketNoDisplay) + " : " + r.ItemDescription;
                        r.ItemDescription = ServiceTicketLibrary.GetInstance().ReplaceTicketNumberToDisplay(prefix, ticketNoDisplay) + " : " + r.ItemDescription;//  prefix + Convert.ToInt32(ticketNoDisplay) + " : " + r.ItemDescription;
                    }
                }

                if (!string.IsNullOrEmpty(r.ParentItemCode))
                {
                    DataRow[] drr_Parrent = dtPrefix.Select("'" + r.ParentItemCode + "' like PrefixCode + '%'");
                    if (drr_Parrent.Length > 0)
                    {
                        string prefix = drr_Parrent[0]["PrefixCode"].ToString();

                        string ticketNoDisplay = r.ParentItemCode;
                        //for (int i = 0; i < prefix.Length; i++)
                        //{
                        //    ticketNoDisplay = ticketNoDisplay.Replace(prefix[i].ToString(), "");
                        //}

                        //r.ParentItemDescription = prefix + Convert.ToInt32(ticketNoDisplay) + " : " + r.ParentItemDescription;
                        r.ParentItemDescription = ServiceTicketLibrary.GetInstance().ReplaceTicketNumberToDisplay(prefix, ticketNoDisplay) + " : " + r.ItemDescription;//  prefix + Convert.ToInt32(ticketNoDisplay) + " : " + r.ItemDescription;
                    }
                }
            });

            if (dataEn.parentNode.Count + dataEn.chindNode.Count > 0)
            {
                FlowChartDiagramRelationControl.URLNodeRedirect = "#";
                //FlowChartDiagramRelationControl.OtherKey = CustomerCode;
                FlowChartDiagramRelationControl.nodeActive = hddDocnumberTran.Value;
                FlowChartDiagramRelationControl.listParentDiagram = dataEn.parentNode;
                FlowChartDiagramRelationControl.listChildDiagram = dataEn.chindNode;
                FlowChartDiagramRelationControl.initFlowChartDiagram();
            }
        }

        protected void saveRelation_Click(object sender, EventArgs e)
        {
            try
            {
                LinkFlowChartService.updateDataDiagram(
                    SID,
                    CompanyCode,
                    LinkFlowChartService.ItemGroup_TICKET,
                    hddDocnumberTran.Value,
                    FlowChartDiagramRelationControl.listDiagramDataSave,
                    EmployeeCode,
                    null //CustomerCode
                );

                getDataRelation();
                ClientService.DoJavascript("bindHierarchyReferFrom();controlDisplayEditTicketRelation(false);");
                ClientService.AGSuccess("บันทึกสำเร็จ");
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

        protected void btnOpenTicketRelation_Click(object sender, EventArgs e)
        {
            try
            {
                getdataToedit(
                    hddDoctype_OpenRelation.Value,
                    hddTicketNo_OpenRelation.Value,
                    hddFiscalYear_OpenRelation.Value,
                    true, true
                );
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

        #region Knowledge Management

        private void bindKnowledgeIDRefTicketNO()
        {
            string DocType = "";
            string fiscalyear = "";
            string ticketNO = "";
            string customerCode = "";
            foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
            {
                DocType = Convert.ToString(dr["DocType"]);
                fiscalyear = Convert.ToString(dr["FiscalYear"]);
                ticketNO = Convert.ToString(dr["CallerID"]);
                customerCode = Convert.ToString(dr["CustomerCode"]);
            }

            #region bind Criteria
            List<KMCriteriaEntity> listData = libkm.GetKMGroup(SID);
            ddlGroup.DataValueField = "xKey";
            ddlGroup.DataTextField = "xValue";
            ddlGroup.DataSource = listData;
            ddlGroup.DataBind();
            ddlGroup.Items.Insert(0, new ListItem("เลือก", ""));

            ddlGroupModal.DataValueField = "xKey";
            ddlGroupModal.DataTextField = "xValue";
            ddlGroupModal.DataSource = listData;
            ddlGroupModal.DataBind();
            ddlGroupModal.Items.Insert(0, new ListItem("เลือก", ""));

            #endregion

            List<KMHeaderData> listKm = new List<KMHeaderData>();
            listKm = KMServiceLibrary.getInstance().getKMHeaderRefTicketNumber(SID, CompanyCode,
                customerCode,
                DocType,
                fiscalyear,
                ticketNO);
            listKm.ForEach(r =>
            {
                r.CREATED_ON = Validation.Convert2DateTimeDisplay(r.CREATED_ON);
            });


            List<string> listKM = listKm.Select(x => x.ObjectID).Distinct().ToList();
            hhdDataSourceSelectAddKnowledgeManagement.Value = JsonConvert.SerializeObject(listKM);
            udpAddKnowledgeData.Update();

            JArray data = JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(listKm));
            udpKnowledgeData.Update();
            ClientService.DoJavascript("bindDataKnowledgeManagementRefTicketNumber(" + data + ");");

        }

        private void bindDefaultSearchAddKnowledge()
        {
            List<KMHeaderData> listKm = new List<KMHeaderData>();
            rptAddKnowledgeManagement.DataSource = listKm;
            rptAddKnowledgeManagement.DataBind();
            udpAddKnowledgeData.Update();
            ClientService.DoJavascript("bindDataKnowledgeManagementRefTicketNumberForAdd();");
        }

        protected void btnSearchDataKnowledge_Click(object sender, EventArgs e)
        {
            try
            {
                List<KMHeaderData> listKm = new List<KMHeaderData>();
                string _where = libkm.getWhereCondition("", "", "", "", ddlGroup.SelectedValue, txtSearch.Text);
                listKm = libkm.getKMHeaderCondition(SID, CompanyCode, _where);
                listKm.ForEach(r =>
                {
                    r.CREATED_ON = Validation.Convert2DateTimeDisplay(r.CREATED_ON);
                });
                mListKnowledgeID = JsonConvert.DeserializeObject<List<string>>(hhdDataSourceSelectAddKnowledgeManagement.Value);
                rptAddKnowledgeManagement.DataSource = listKm;
                rptAddKnowledgeManagement.DataBind();
                udpAddKnowledgeData.Update();
                ClientService.DoJavascript("bindDataKnowledgeManagementRefTicketNumberForAdd();");
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

        List<string> mListKnowledgeID;
        protected void rptAddKnowledgeManagement_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            KMHeaderData en = (KMHeaderData)e.Item.DataItem;
            if (mListKnowledgeID.Find(x => x.Equals(en.ObjectID)) != null)
            {
                (e.Item.FindControl("chkSelectAddKnowledge") as CheckBox).Checked = true;
            }
        }

        protected void btnOpenModalAddKM_Click(object sender, EventArgs e)
        {
            try
            {
                bindDefaultSearchAddKnowledge();
                ClientService.DoJavascript("showInitiativeModal('modal-addKnowledgeManagement');");
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
        protected void btnRedirectKnowledgeIDRefTicketNO_Click(object sender, EventArgs e)
        {
            try
            {
                KnowledgeManagementDetail obj = new KnowledgeManagementDetail();
                string id = obj.GUID_RERUEST;
                Session[obj.SessionNameDefault + id] = hhdKnowledgeIDRefTicketNO.Value;
                ClientService.DoJavascript("redirectKnowledgeManagementNewPage('" + id + "');");
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
        protected void btnSaveAddNewKnowledge_Click(object sender, EventArgs e)
        {
            try
            {
                string DocType = "";
                string fiscalyear = "";
                string ticketNO = "";
                string customerCode = "";
                foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
                {
                    DocType = Convert.ToString(dr["DocType"]);
                    fiscalyear = Convert.ToString(dr["FiscalYear"]);
                    ticketNO = Convert.ToString(dr["CallerID"]);
                    customerCode = Convert.ToString(dr["CustomerCode"]);
                }
                List<string> listKnowledgeID = JsonConvert.DeserializeObject<List<string>>(hhdDataSourceSelectAddKnowledgeManagement.Value);
                libkm.saveTiketKnowledgeMaping(SID, CompanyCode, EmployeeCode,
                    customerCode,
                    DocType,
                    fiscalyear,
                    ticketNO,
                    listKnowledgeID);
                if (chkIsLoad_Knowledge.Checked)
                    bindKnowledgeIDRefTicketNO();
                if (chkIsLoad_TicketChangeLog.Checked)
                    bindDataChangeLog();
                ClientService.AGSuccess("Save Knowledge Ref Tickit " + _txt_docnumberTran.Value + " success.");
                ClientService.DoJavascript("closeInitiativeModal('modal-addKnowledgeManagement');");
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

        #region Create KM

        protected void btnOpenCreateKMRefTicket_Click(object sender, EventArgs e)
        {
            try
            {
                txtKeywordModal.Text = "";
                txtSubjectModal.Text = txt_problem_topic.Text;
                txtDetailModal.Text = tbEquipmentRemark.Text;
                /*txtSymtomModal.Text = tbSummaryProblem.Text;
                txtCauseModal.Text = tbSummaryCause.Text;
                txtSolutionModal.Text = tbSummaryResolution.Text;*/
                ddlGroupModal.SelectedValue = "";
                udpCreateDetail.Update();
                ClientService.DoJavascript("showInitiativeModal('modal-CreateDataKM');");
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

        protected void btnCreateKMDetail_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = "";
                if (string.IsNullOrEmpty(ddlGroupModal.SelectedValue))
                {
                    msg += "KM Group is empty!<br/>";
                }

                if (string.IsNullOrEmpty(txtSubjectModal.Text.Trim()))
                {
                    msg += "Subject is empty!<br/>";
                }

                if (!string.IsNullOrEmpty(msg))
                {
                    throw new Exception(msg);
                }

                #region Create KM
                KMHeaderData en = new KMHeaderData();
                en.KMGroup = ddlGroupModal.SelectedValue;
                en.KMGroupName = "";
                en.ObjectID = "";
                en.PrimaryKeyWord = txtKeywordModal.Text;
                en.Description = txtSubjectModal.Text;
                en.AttachmentID = Guid.NewGuid().ToString(); //hhdKeyAobjectlink.Value;

                en.ObjectItem = "001";
                en.Details = txtDetailModal.Text;
                en.Solution = txtSolutionModal.Text;
                en.Symptom = txtSymtomModal.Text;
                en.Cause = txtCauseModal.Text;

                string KMOBjectID = libkm.saveKMManagement(
                    SID,
                    CompanyCode,
                    EmployeeCode,
                    UserName,
                    en);
                #endregion

                #region Mapping Ticket Ref  KMID
                string DocType = "";
                string fiscalyear = "";
                string ticketNO = "";
                string customerCode = "";
                foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
                {
                    DocType = Convert.ToString(dr["DocType"]);
                    fiscalyear = Convert.ToString(dr["FiscalYear"]);
                    ticketNO = Convert.ToString(dr["CallerID"]);
                    customerCode = Convert.ToString(dr["CustomerCode"]);
                }
                List<string> listKnowledgeID = libkm.getKMHeaderRefTicketNumber(SID, CompanyCode
                    , customerCode, DocType, fiscalyear, ticketNO).Select(x => x.ObjectID).ToList();
                listKnowledgeID.Add(KMOBjectID);
                libkm.saveTiketKnowledgeMaping(SID, CompanyCode, EmployeeCode,
                   customerCode,
                   DocType,
                   fiscalyear,
                   ticketNO,
                   listKnowledgeID);
                #endregion

                string id = setDataToRedirectKMPageRefGUID(KMOBjectID);
                ClientService.DoJavascript("inactiveRequireField();closeInitiativeModal('modal-CreateDataKM');");
                ClientService.AGSuccessRedirect("Created knowledge " + KMOBjectID + "  success.", "/KM/KnowledgeManagementDetail.aspx?id=" + id);
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

        protected void btnSubmitRequireKM_Click(object sender, EventArgs e)
        {
            ClientService.DoJavascript("$('#btnCreateKMDetail').click();");
        }

        private string setDataToRedirectKMPageRefGUID(string knowledgeID)
        {
            KnowledgeManagementDetail obj = new KnowledgeManagementDetail();
            string id = obj.GUID_RERUEST;
            Session[obj.SessionNameDefault + id] = knowledgeID;
            return id;
        }
        #endregion

        protected void brnRefreshData_Click(object sender, EventArgs e)
        {
            try
            {
                IDCurentTabView = hddIDCurentTabView.Value;
                getdataToedit(
                    (string)Session["SCT_created_doctype_code" + idGen],
                    hddDocnumberTran.Value.Trim(),
                    (string)Session["SCT_created_fiscalyear" + idGen]
                );
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

        protected void btnResetToDefault_Click(object sender, EventArgs e)
        {
            try
            {
                Session["ServicecallEntity" + idGen] = new tmpServiceCallDataSet();

                ShowWorkFlow = false;
                ShowWorkFlowWithoutCreate = false;
                AlowWorkFlow = false;
                countActivity = 0;
                Session["SCT_GROUPCODE" + idGen] = null;

                clearScreen();

                if (mode_stage == ApplicationSession.CREATE_MODE_STRING)
                {
                    initData_CreateMode();
                }

                ddlProblemGroup.SelectedValue = "";
                btnChangeProblemGroup_Click(null, null);

                udpnSeverity.Update();
                udpnProblem.Update();
                udpRefer.Update();
                udpRemark.Update();
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

        protected void ddlOwnerGroupService_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dtOwner = libconfig.GetMasterConfigOwnerGroup(
                    SID,
                    CompanyCode,
                    ddlOwnerGroupService.SelectedValue
                );

                if (dtOwner.Rows.Count > 0 && !string.IsNullOrEmpty(ddlOwnerGroupService.SelectedValue))
                {
                    string RoleConfig = Convert.ToString(dtOwner.Rows[0]["RoleConfig"]);

                    DataTable dtMain = CharacterService.getInstance().getRoleMappingEmployee(
                        SID,
                        CompanyCode,
                        RoleConfig,
                        true
                    );
                    DataTable dtParticipant = CharacterService.getInstance().getRoleMappingEmployee(
                        SID,
                        CompanyCode,
                        RoleConfig,
                        false
                    );

                    DataTable dt = new DataTable();
                    dt.Columns.Add("empCode");
                    dt.Columns.Add("empName");
                    dt.Columns.Add("empImg");
                    dt.Columns.Add("IsMain");

                    foreach (DataRow drMain in dtMain.Rows)
                    {
                        DataRow drNew = dt.Rows.Add();
                        drNew["empCode"] = drMain["EmployeeCode"].ToString();
                        drNew["empName"] = drMain["FullName_EN"].ToString();
                        drNew["empImg"] = "/images/user.png";
                        drNew["IsMain"] = "true";
                    }
                    foreach (DataRow drParticipant in dtParticipant.Rows)
                    {
                        DataRow drNew = dt.Rows.Add();
                        drNew["empCode"] = drParticipant["EmployeeCode"].ToString();
                        drNew["empName"] = drParticipant["FullName_EN"].ToString();
                        drNew["empImg"] = "/images/user.png";
                        drNew["IsMain"] = "false";
                    }

                    rptOperationTransfer.DataSource = dt;
                    rptOperationTransfer.DataBind();
                }
                else
                {
                    rptOperationTransfer.DataSource = new DataTable();
                    rptOperationTransfer.DataBind();
                }
                udpTransfer.Update();
                udpParticipants.Update();
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

        //Add module 30 / 10 / 2018
        protected void btnTransferChangeOrder_Click(object sender, EventArgs e)
        {

            try
            {
                //update Module
                DataTable dtOwnerOperation = AfterSaleService.getInstance().getOwnerOperation(
                    CompanyCode,
                    _txt_fiscalyear.Value,
                    hddDocnumberTran.Value
                );
                hddTransferChangeOrder_AOBJECTLINK.Value = Convert.ToString(dtOwnerOperation.Rows[0]["AOBJECTLINK"]);
                hddTransferChangeOrder_TierCode.Value = Convert.ToString(dtOwnerOperation.Rows[0]["TierCode"]);
                hddTransferChangeOrder_Tier.Value = Convert.ToString(dtOwnerOperation.Rows[0]["Tier"]);
                //update Module

                System.Diagnostics.Debug.WriteLine(dtOwnerOperation.Rows[0]["AOBJECTLINK"]);
                System.Diagnostics.Debug.WriteLine(dtOwnerOperation.Rows[0]["TierCode"]);
                System.Diagnostics.Debug.WriteLine(dtOwnerOperation.Rows[0]["Tier"]);
                hddTransferChangeOrder_ListEMPCode.Value = "";
                udpTransferChangeorder.Update();
                AutoCompleteEmployee_Transfer.SelectedValueRefresh = "";
                LoadOperationTransfer(hddTransferChangeOrder_AOBJECTLINK.Value);
                ClientService.DoJavascript("showInitiativeModal('modal-TransferAddParticipant');");
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

        private void LoadOperationTransfer(string AobjectLink)
        {
            //update
            string Fiscalyear = serviceCallEntity.cs_servicecall_header.Rows[0]["Fiscalyear"].ToString();
            string RoleConfig = "";
            DataTable dtOwner = libconfig.GetMasterConfigOwnerGroup(
                SID,
                CompanyCode,
                ddlOwnerGroupService.SelectedValue
            );

            if (dtOwner.Rows.Count > 0)
            {
                RoleConfig = Convert.ToString(dtOwner.Rows[0]["RoleConfig"]);
            }

            DataTable dtMainChangeOrder = AfterSaleService.getInstance().getSLASetMain(
                    SID,
                    CompanyCode,
                    RoleConfig,
                    AobjectLink
                );

            DataTable dtParticipantChangeOrder = lib.getListEmpParticipantsRefTicket_Change(
                SID,
                CompanyCode,
                AobjectLink,
                hddDocnumberTran.Value,
                Fiscalyear,
                hddTicketDocType.Value,
                RoleConfig
            );

            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("empCode");
            dtNew.Columns.Add("empName");
            dtNew.Columns.Add("empImg");
            dtNew.Columns.Add("IsMain");

            foreach (DataRow drMain in dtMainChangeOrder.Rows)
            {
                DataRow drNew = dtNew.Rows.Add();
                drNew["empCode"] = drMain["EmployeeCode"].ToString();
                drNew["empName"] = drMain["FullName_EN"].ToString();
                drNew["empImg"] = "/images/user.png";
                drNew["IsMain"] = "true";
            }
            foreach (DataRow drParticipant in dtParticipantChangeOrder.Rows)
            {
                DataRow drNew = dtNew.Rows.Add();
                drNew["empCode"] = drParticipant["EmployeeCode"].ToString();
                drNew["empName"] = drParticipant["fullname"].ToString();
                drNew["empImg"] = "/images/user.png";
                drNew["IsMain"] = "false";
            }

            rptOperationTransferChangeorder.DataSource = dtNew;
            rptOperationTransferChangeorder.DataBind();
            udpTransferChangeorder.Update();
            //update
        }

        private string getTierCode()
        {
            string priorityCode = _ddl_priorityTran.Items.Count > 0 ? _ddl_priorityTran.SelectedValue : "";

            return AfterSaleService.getInstance().GetTierCode(
                SID,
                CompanyCode,
                CustomerCode,
                EquipmentSelect,
                priorityCode
            );
        }

        protected void btnTransferAddParticipantChangeOrder_Click(object sender, EventArgs e)
        {
            try
            {

                //HiddenField hddTierCode = e.Item.FindControl("hddTierCode") as HiddenField;
                //HiddenField hddTier = e.Item.FindControl("hddTier") as HiddenField;

                List<listOperationTransfer> listEmp = JsonConvert.DeserializeObject<List<listOperationTransfer>>(hddTransferChangeOrder_ListEMPCode.Value);
                lib.saveEmployeeParticipantRefTicket(
                    SID,
                    CompanyCode,
                    hddDocnumberTran.Value,
                    hddTransferChangeOrder_TierCode.Value,
                    hddTransferChangeOrder_Tier.Value,
                    EmployeeCode,
                    FullNameTH,
                    hddTransferChangeOrder_AOBJECTLINK.Value,
                    listEmp
                );

                NotificationLibrary.GetInstance().TicketAlertEvent(
                    NotificationLibrary.EVENT_TYPE.TICKET_TRANSFER,
                    SID,
                    CompanyCode,
                    hddDocnumberTran.Value,
                    EmployeeCode,
                    ThisPage
                );

                BindDataOwnerOperation();
                ClientService.DoJavascript("closeInitiativeModal('modal-TransferAddParticipant');");
                return;
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
        public bool checkCICodeInDB(string cicode)
        {
            List<string> listCICodeDB = (from DataRow dr in serviceCallEntity.cs_servicecall_item
                             where dr.RowState == DataRowState.Unchanged
                             select dr["EquipmentNo"].ToString()).ToList();

            return listCICodeDB.Contains(cicode);
        }

        //Add module 30 / 10 / 2018

        // 21/11/2561  function event add configuration item (by born kk)
        protected void btnSaveCI_Click(object sender, EventArgs e)
        {

            try
            {
                List<string> listCICode = SearchHelpCIControl.listCICode;
                List<string> listCICodeOld = new List<string>();


                if (mode_stage == ApplicationSession.CREATE_MODE_STRING)
                {
                    listCICodeOld = (from DataRow dr in serviceCallEntity.cs_servicecall_item
                                                  select dr["EquipmentNo"].ToString()).ToList();
                    
                } else if (mode_stage == ApplicationSession.CHANGE_MODE_STRING)
                {
                    listCICodeOld = (from DataRow dr in serviceCallEntity.cs_servicecall_item
                                     where dr.RowState != DataRowState.Unchanged
                                     select dr["EquipmentNo"].ToString()).ToList();
                }

                // clear unselete ci
                foreach (string cicodeold in listCICodeOld)
                {
                    if (listCICode.Contains(cicodeold))
                    {
                        continue;
                    }
                    removeEquipment(cicodeold);
                }

                foreach (var CICode in listCICode)
                {
                    //System.Console.WriteLine(CICode);
                    addEquipment(CICode);
                    //bindDataCI();
                }
                bindDataCI();


            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
            finally
            {
                ClientService.DoJavascript("closeInitiativeModal('modalSearchHelpConfigurationItem');");
                ClientService.AGLoading(false);
            }
        }

        // 21/11/2561  function event add effected customer (by born kk)
        protected void btnSaveEffectedCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> listCustomer = SearchCustomerControl.listCustomerCode;
                foreach (var CustomerCode in listCustomer)
                {
                    //System.Console.WriteLine(CustomerCode);
                    addEffectedCustomer(CustomerCode);
                    // addEquipment(CICode);
                    // bindDataCI();
                }
                List<ContactList> listCon = getContactSelect();
                bindCustomer();
                setContactList();
                setDetailContact(listCon);
            }
            catch (Exception ex)
            {
                ClientService.AGLoading(false);
                ClientService.AGError(ex.Message);
            }
            finally
            {
                ClientService.DoJavascript("closeInitiativeModal('modalSearchHelpCustomer');");
                ClientService.AGLoading(false);
            }
        }

        protected void btnRemoveEffectedCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                string CustomerCode = (sender as Button).CommandArgument;
                List<ContactList> listCon = getContactSelect();
                removeEffectedCustomer(CustomerCode);
                bindCustomer();
                setContactList();
                setDetailContact(listCon);
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

        private void addEffectedCustomer(string CustomerCode)
        {


            Service.CustomerService libCus = new Service.CustomerService();
            DataRow newrow = servicecallCustomer.NewRow();
            newrow["CustomerCode"] = CustomerCode;
            newrow["CustomerName"] = libCus.getCustomerName(SID, CompanyCode, CustomerCode);
            servicecallCustomer.Rows.Add(newrow);

        }

        private void removeEffectedCustomer(string CustomerCode)
        {
            prepareEffectedCustomer();
            for (int i = servicecallCustomer.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = servicecallCustomer.Rows[i];
                if (row["CustomerCode"].ToString() == CustomerCode)
                {
                    servicecallCustomer.Rows[i].Delete();
                    return;
                }

            }

        }

        private void createHeaderEffectedCustomerDataSet()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CustomerCode");
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("ContactCode");
            dt.Columns.Add("Email");
            dt.Columns.Add("Tel");
            servicecallCustomer = dt;
        }

        private void bindCustomer()
        {
            setDDLSelectAccountability(servicecallCustomer);

            cusLists.DataSource = servicecallCustomer;
            cusLists.DataBind();
            updateCus.Update();

        }

        private void setDDLSelectAccountability(DataTable servicecallCustomer)
        {
            if (servicecallCustomer.Rows.Count > 0)
            {
                string customerCode = !String.IsNullOrEmpty(servicecallCustomer.Rows[0]["CustomerCode"].ToString()) ? servicecallCustomer.Rows[0]["CustomerCode"].ToString() : servicecallCustomer.Rows[0]["CustomerCode"].ToString();
                CustomerProfile customerProfile = ERPW.Lib.Master.CustomerService.getInstance().SearchCustomerDataByCustomerCode(
                        SID,
                        CompanyCode,
                        customerCode
                );

                if (customerProfile != null)
                {
                    if (ddlAccountability.Items.FindByValue(customerProfile.Accountability) != null)
                    {
                        ddlAccountability.SelectedValue = customerProfile.Accountability;
                    }
                    else
                    {
                        ddlAccountability.SelectedValue = "";
                    }  
                }
                else
                {
                    ddlAccountability.SelectedValue = "";
                }

            } 
            else
            {
                ddlAccountability.SelectedValue = "";
            }
            updDDLAcc.Update();
        }

        private void bindCustomerChangeMode()
        {
            DataTable dt = lib.getDateEffectedCustomer(
                SID,
                CompanyCode,
                hddDocnumberTran.Value
            );
            cusLists.DataSource = dt;
            cusLists.DataBind();
            updateCus.Update();
        }

        private void prepareEffectedCustomer()
        {
            Service.CustomerService libCus = new Service.CustomerService();
            foreach (RepeaterItem item in cusLists.Items)
            {
                HiddenField txtCustomerCode = item.FindControl("hddCustomerCode") as HiddenField;
                //AutoCompleteControl control = item.FindControl("_ddl_contact_person_search") as AutoCompleteControl;


                DataRow newrow = servicecallCustomer.NewRow();
                newrow["CustomerCode"] = txtCustomerCode.Value;
                //newrow["ContactCode"]  = control.SelectValue;
                newrow["CustomerName"] = libCus.getCustomerName(SID, CompanyCode, txtCustomerCode.Value);
                servicecallCustomer.Rows.Add(newrow);

            }
        }

        private void setContactList()
        {
            foreach (RepeaterItem item in cusLists.Items)
            {
                HiddenField txtCustomerCode = item.FindControl("hddCustomerCode") as HiddenField;
                //AutoCompleteControl control = item.FindControl("_ddl_contact_person_search") as AutoCompleteControl;
                DataTable dt = new DataTable();
                //string selectValue = control.SelectValue;
                //string selecttxt = control.SelectText;

                if (txtCustomerCode.Value == "")
                {
                    dt.Columns.Add("BOBJECTLINK");
                    dt.Columns.Add("NAME1");
                    dt.Columns.Add("email");
                    dt.Columns.Add("phone");
                    dt.Columns.Add("remark");
                }
                else
                {
                    dt = AfterSaleService.getInstance().getContactPerson(CompanyCode, txtCustomerCode.Value);
                }
                //control.initialDataAutoComplete(dt, "BOBJECTLINK", "NAME1", false, "email", "phone", "remark");
            }


        }

        [Serializable]
        public class ContactList
        {
            public string customerCode { get; set; }
            public string contactCode { get; set; }
            public string contactName { get; set; }

        }
        private List<ContactList> getContactSelect()
        {

            List<ContactList> list = new List<ContactList>();
            foreach (RepeaterItem item in cusLists.Items)
            {
                HiddenField txtCustomerCode = item.FindControl("hddCustomerCode") as HiddenField;
                //AutoCompleteControl control = item.FindControl("_ddl_contact_person_search") as AutoCompleteControl;
                ContactList con = new ContactList();

                //con.contactCode = control.SelectValue;
                con.customerCode = txtCustomerCode.Value;
                //con.contactName = control.SelectText;
                //string selecttxt = control.SelectText;
                //if (selecttxt != "")
                //{
                //    list.Add(con);
                //}
            }
            return list;
        }


        private void setDetailContact(List<ContactList> listCon)
        {
            foreach (ContactList con in listCon)
            {
                ERPW.Lib.Master.ContactEntity en = new ERPW.Lib.Master.ContactEntity();
                foreach (RepeaterItem item in cusLists.Items)
                {
                    HiddenField txtCustomerCode = item.FindControl("hddCustomerCode") as HiddenField;
                    //AutoCompleteControl control = item.FindControl("_ddl_contact_person_search") as AutoCompleteControl;

                    TextBox txtEmail = item.FindControl("txtContactEmail_Ef") as TextBox;
                    TextBox txtPhone = item.FindControl("txtContactPhone_Ef") as TextBox;
                    if (con.customerCode == txtCustomerCode.Value)
                    {
                        //control.SetValue = con.contactCode;
                        //control.SelectValue = con.contactCode;
                        //control.SelectText = con.contactName;

                        List<ERPW.Lib.Master.ContactEntity> listen = serviceCustomer.getListContactRefCustomer(
                            SID,
                            CompanyCode,
                            con.customerCode,
                            "",
                            con.contactCode
                        );
                        if (listen.Count > 0)
                        {
                            en = listen[0];

                            txtPhone.Text = en.phone.Trim();
                            txtEmail.Text = en.email.Trim();

                        }
                        break;
                    }
                }
            }
            updateCus.Update();

        }
        protected void btnSelectContactBindDetail_Click(object sender, EventArgs e)
        {
            try
            {
                List<ContactList> listCon = getContactSelect();
                setDetailContact(listCon);

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
        private void getContactCustomer()
        {

        }

        public void MailAlertWorkflow()
        {
            NotificationLibrary.GetInstance().TicketAlertEvent(
                NotificationLibrary.EVENT_TYPE.ChangeOrder_Approval,
                SID,
                CompanyCode,
                hddDocnumberTran.Value,
                EmployeeCode,
                ThisPage
            );
        }

        public void MailAlertOwner()
        {
            NotificationLibrary.GetInstance().TicketAlertEvent(
                NotificationLibrary.EVENT_TYPE.OWNER,
                SID,
                CompanyCode,
                hddDocnumberTran.Value,
                EmployeeCode,
                ThisPage
            );
        }
        protected void btnRequestChange_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean IsFirstView = true;
                AnalyticsEmployee enAnalytics = AnalyticsService.getTackingPageTicketIsAuthenEdit(
                    SID,
                    CompanyCode,
                    hddDocnumberTran.Value,
                    EmployeeCode
                );
                IsFirstView = enAnalytics.IsAuthenEdit;

                if (enAnalytics.IsAuthenEdit)
                {
                    IDCurentTabView = hddIDCurentTabView.Value;
                    getdataToedit(hddTicketDocType.Value, hddDocnumberTran.Value, _txt_fiscalyear.Value,
                        true, false, ApplicationSession.CHANGE_MODE_STRING);
                }
                else
                {
                    throw new Exception("Change Mode Lock By " + enAnalytics.firstEmployeeName);
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

        protected void btnRequestDisplayMode_Click(object sender, EventArgs e)
        {
            try
            {
                IDCurentTabView = hddIDCurentTabView.Value;
                getdataToedit(
                    hddTicketDocType.Value, hddDocnumberTran.Value, _txt_fiscalyear.Value,
                    true, false, ApplicationSession.DISPLAY_MODE_STRING
                );
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


        //20/12/2561 reset ticket (by born kk)
        protected void btnResetTicket_Click(object sender, EventArgs e)
        {

            try
            {
                string mode = (sender as Button).CommandArgument;

                ResetTicket(mode);
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
        //20/12/2561 function reset ticket (by born kk)
        private void ResetTicket(string mode)
        {
            string idGen = Guid.NewGuid().ToString().Substring(0, 8);

            string docType = "";
            string docTypeDesc = "";
            string customerCode = "";
            string customerName = "";
            string fiscalYear = "";

            string oldDocType = "";
            string oldFiscalYear = "";

            if (serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
            {
                oldDocType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
                oldFiscalYear = serviceCallEntity.cs_servicecall_header.Rows[0]["FiscalYear"].ToString();

                docType = hddTicketDocType.Value;
                docTypeDesc = GetSCTypeDesc(docType);
                customerCode = CustomerSelect.SelectedValue;
                customerName = GetCustomerDesc(customerCode);
                fiscalYear = _txt_year.Value;
            }

            //Session["SCT_created_ticket_ref_code" + idGen] = hddDocnumberTran.Value;

            Session["SCT_created_doctype_code" + idGen] = docType;
            docTypeDesc = GetSCTypeDesc(docType);
            Session["SCT_created_doctype_desc" + idGen] = docTypeDesc;

            Session["SCT_created_fiscalyear" + idGen] = fiscalYear;
            List<string> listItemEquipment = new List<string>();
            foreach (RepeaterItem item in rptEquipment.Items)
            {
                HiddenField txtEquipmentNo = item.FindControl("hddEquipmentCode") as HiddenField;

                listItemEquipment.Add(txtEquipmentNo.Value);
            }
            Session["SCT_created_equipment_List" + idGen] = listItemEquipment;

            List<string> listEffectCus = new List<string>();
            foreach (RepeaterItem item in cusLists.Items)
            {
                HiddenField txtCustomerCode = item.FindControl("hddCustomerCode") as HiddenField;
                listEffectCus.Add(txtCustomerCode.Value);

            }

            Session["SCT_created_effectCus_List" + idGen] = listEffectCus;
            Session["SCT_created_remark" + idGen] = null;
            Session["SCT_created_impact" + idGen] = ddlImpact.SelectedValue;
            Session["SCT_created_urgency" + idGen] = ddlUrgency.SelectedValue;
            Session["SCT_created_priority" + idGen] = _ddl_priorityTran.SelectedValue;
            Session["SCT_created_accountability" + idGen] = ddlAccountability.SelectedValue;
            Session["SCT_created_problemgroup" + idGen] = ddlProblemGroup.SelectedValue;
            Session["SCT_created_problemtype" + idGen] = ddlProblemType.SelectedValue;
            Session["SCT_created_problemsource" + idGen] = ddlProblemSource.SelectedValue;
            Session["SCT_created_servicesource" + idGen] = ddlServiceSource.SelectedValue;
            Session["SCT_created_proplemtopic" + idGen] = txt_problem_topic.Text;
            Session["SCT_created_equipmentremark" + idGen] = tbEquipmentRemark.Text;
            Session["SCT_created_ownergroupservice" + idGen] = ddlOwnerGroupService.SelectedValue;
            Session["SCT_created_planstartdate" + idGen] = tbStartDate.Text;
            Session["SCT_created_planenddate" + idGen] = tbEndDate.Text;
            Session["SCT_created_planstarttime" + idGen] = txtStartTime.Text;
            Session["SCT_created_planendtime" + idGen] = txtEndTime.Text;



            //serviceCallEntity = new tmpServiceCallDataSet();
            Session["ServicecallEntity" + idGen] = new tmpServiceCallDataSet();

            //mode_stage = ApplicationSession.CREATE_MODE_STRING;
            Session["SC_MODE" + idGen] = ApplicationSession.CREATE_MODE_STRING;


            string PageRedirect = lib.getPageTicketRedirect(
                SID,
                serviceCallEntity.cs_servicecall_header.Rows[0]["Doctype"].ToString()
            );
            if (mode.Equals("Reset"))
            {
                Session["ModeReset" + idGen] = ApplicationSession.CREATE_MODE_STRING + "RESET";
                Session["ticketType" + idGen] = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
                Session["ticketNo" + idGen] = serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString();
                Session["ticketYear" + idGen] = serviceCallEntity.cs_servicecall_header.Rows[0]["FiscalYear"].ToString();
                Session["DocnumberTran" + idGen] = hddDocnumberTran.Value;
            }



            Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen));
        }

        //2/1/2562 function check date 
        private void compareDate() {
            string datestimes = Agape.FocusOne.Utilities.Validation.Convert2DateDB(tbEndDate.Text)+ Agape.FocusOne.Utilities.Validation.Convert2TimeDB(txtEndTime.Text);
            

            string dt = DateTime.Now.ToString("yyyyMMddhhmmss");
            if (Int64.Parse( dt) > Int64.Parse( datestimes)) {
                ClientService.DoJavascript("headerdoctrandatecolor();");
            }




        }



        //24/01/2562 roll back Doc (by born kk)
        protected void btnRollBackTicket_Click(object sender ,EventArgs e) {
            try
            {
                string Docstatus = serviceCallEntity.cs_servicecall_header.Rows[0]["Docstatus"].ToString();
                string SERVICE_DOC_STATUS_RESOLVE = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE_BUSINESS_CHANGE);
                if (Docstatus != SERVICE_DOC_STATUS_RESOLVE)
                {
                    throw new Exception("Can not Roll Back");
                }
           

                string rollbackdate = Validation.getCurrentServerStringDateTime().Substring(0, 8);
                string rollbacktime = Validation.getCurrentServerDateTime().ToString("HHmmss");
                string rollbackby = EmployeeCode;

                string tierDesc = "";
                string ticketType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
                string ticketNo = serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString();
                string ticketYear = serviceCallEntity.cs_servicecall_header.Rows[0]["FiscalYear"].ToString();

                /*AfterSaleService.getInstance().saveLogCanceledTicket(SID, CompanyCode,
                    ticketType, ticketNo, ticketYear, EmployeeCode, FullNameEN
                );*/

                saverollbackdoc(rollbackdate, rollbacktime, rollbackby);

                NotificationLibrary.GetInstance().TicketAlertEvent(
                    NotificationLibrary.EVENT_TYPE.TICKET_UPDATESTATUS,
                    SID,
                    CompanyCode,
                    hddDocnumberTran.Value,
                    EmployeeCode,
                    ThisPage
                );

                getdataToedit((string)Session["SCT_created_doctype_code" + idGen], hddDocnumberTran.Value.Trim(), (string)Session["SCT_created_fiscalyear" + idGen]);
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



        private void saverollbackdoc(string rollbackdate, string rollbacktime, string rollbackby,tmpServiceCallDataSet dsServiceCall = null) {
            try
            {
                if (dsServiceCall == null)
                {
                    dsServiceCall = (tmpServiceCallDataSet)serviceCallEntity.Copy();
                }

                DataTable dtHeader = dsServiceCall.cs_servicecall_header;
                DataTable dtItem = dsServiceCall.cs_servicecall_item;
                DataRow[] drHeader = null;

                if (string.IsNullOrEmpty((string)Session["ticketNo" + idGen]))
                {
                    drHeader = dtHeader.Select("CallerID='" + hddDocnumberTran.Value + "'");
                }
                else
                {
                    drHeader = dtHeader.Select("CallerID='" + ((string)Session["ticketNo" + idGen]) + "'");
                }

                if (drHeader.Length != 0)
                {
                    string SERVICE_DOC_STATUS_ROLLBACK = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_ROLL_BACK_BUSINESS_CHANGE);

                    drHeader[0]["Docstatus"] = SERVICE_DOC_STATUS_ROLLBACK;
                    drHeader[0]["CallStatus"] = ServiceTicketLibrary.SERVICE_CALL_STATUS_CANCEL;
                    
                }

                foreach (DataRow drItem in dtItem.Rows)
                {
                    drItem["CloseStatus"] = ServiceTicketLibrary.SERVICE_CALL_STATUS_CANCEL;
                }

                saveAssignCancelCloseResponseResolution("1500545", "Roll Back Ticket", rollbackby, dsServiceCall);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public void clearSessionTicket()
        {
            //Session["SCT_created_doctype_code" + idGen];
            Session.Remove("SCT_created_doctype_code" + idGen);
            Session.Remove("SCT_created_doctype_desc" + idGen);
            Session.Remove("SCT_created_cust_code" + idGen);
            Session.Remove("SCT_created_cust_desc" + idGen);
            Session.Remove("SCT_created_contact_code" + idGen);
            Session.Remove("SCT_created_contact_desc" + idGen);
            Session.Remove("SCT_created_fiscalyear" + idGen);
            Session.Remove("SCT_created_remark" + idGen);
            Session.Remove("SCT_created_equipment" + idGen);
            Session.Remove("SCT_created_impact" + idGen);
            Session.Remove("SCT_created_urgency" + idGen);
            Session.Remove("SCT_created_priority" + idGen);
            Session.Remove("ServicecallEntity" + idGen);
            Session.Remove("SC_MODE" + idGen);
            Session.Remove("ServiceCallTransaction_ShowWorkFlow" + idGen);
            Session.Remove("ServiceCallTransaction_ShowWorkFlowWithoutCreate" + idGen);
            Session.Remove("ServiceCallTransaction_AlowWorkFlow" + idGen);
            Session.Remove("SCT_countActivity" + idGen);
            Session.Remove("SCT_GROUPCODE" + idGen);
            Session.Remove("SCT_SCType" + idGen);
            Session.Remove("SCT_dtCustomer" + idGen);
            Session.Remove("SCT_dtGrouTemp" + idGen);
            Session.Remove("SCT_dtDocstatus" + idGen);
            Session.Remove("SCT_dtTicketDocStatus" + idGen);
            Session.Remove("SCT_dtContactPerson" + idGen);
            Session.Remove("SCT_dtProjectCode" + idGen);
            Session.Remove("ObjectID" + idGen);
            Session.Remove("responsecall_objid" + idGen);
            Session.Remove("SCT_dtProjectElement" + idGen);
            Session.Remove("SCT_dtTier" + idGen);
            Session.Remove("dtMaterialMaster" + idGen);
            Session.Remove("dtMaterialPurchase" + idGen);
            Session.Remove("SERVICECALL00001" + idGen);
            Session.Remove("SERVICECALL00002" + idGen);
            Session.Remove("SERVICECALL00003" + idGen);
            Session.Remove("SCT_BObjectID" + idGen);
            Session.Remove("SC_Callerid" + idGen);
            Session.Remove("SCF_SAVESUCCESS" + idGen);
            Session.Remove("EquipmentSelect_saleservice" + idGen);
            Session.Remove("EquipmentItemNo_saleservice" + idGen);
            Session.Remove("responsecall_lineno" + idGen);
        }

        protected void btnClosePage_Click(object sender, EventArgs e)
        {
            clearSessionTicket();
            ClientService.DoJavascript("window.close();");
        }

        protected void btnOpenDetailEquipment_Click(object sender, EventArgs e)
        {
            try
            {
                Session["ServiceWeb.Page.Equipment.EquipmentCode"] = (sender as Button).Text;
                Session["ServiceWeb.Page.Equipment.Page_Mode"] = "Edit";
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

        #region After Load
        protected void btnIsLoad_Attachment_Click(object sender, EventArgs e)
        {
            try
            {
                if (!chkIsLoad_Attachment.Checked)
                {
                    // todo
                    BindDataLogFileAttachment();

                    AttachFileUserControl.DocType = DocType;
                    AttachFileUserControl.DocYear = (string)Session["SCT_created_fiscalyear" + idGen];
                    AttachFileUserControl.DocNumber = hddDocnumberTran.Value.Trim();
                    AttachFileUserControl.Commit(hddDocnumberTran.Value.Trim());

                    setAttachMent("", false
                       , DocType, true
                       , hddDocnumberTran.Value.Trim(), true
                       , (string)Session["SCT_created_fiscalyear" + idGen], true);

                    chkIsLoad_Attachment.Checked = true;
                    udpIsLoad_Attachment.Update();
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

        protected void btnIsLoad_DateTimeLog_Click(object sender, EventArgs e)
        {
            try
            {
                if (!chkIsLoad_DateTimeLog.Checked)
                {
                    // todo
                    bindDataTicketDateTimeLog();

                    chkIsLoad_DateTimeLog.Checked = true;
                    udpIsLoad_DateTimeLog.Update();
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

        protected void btnIsLoad_Knowledge_Click(object sender, EventArgs e)
        {
            try
            {
                if (!chkIsLoad_Knowledge.Checked)
                {
                    // todo
                    bindKnowledgeIDRefTicketNO();

                    chkIsLoad_Knowledge.Checked = true;
                    udpIsLoad_Knowledge.Update();
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

        protected void btnIsLoad_TicketChangeLog_Click(object sender, EventArgs e)
        {
            try
            {
                if (!chkIsLoad_TicketChangeLog.Checked)
                {
                    // todo
                    bindDataChangeLog();

                    chkIsLoad_TicketChangeLog.Checked = true;
                    udpIsLoad_TicketChangeLog.Update();
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
        #endregion

        protected void btnOpenModalSelectConfigurationItem_Click(object sender, EventArgs e)
        {
            SearchHelpCIControl.openAddCI();
        }
    }
}