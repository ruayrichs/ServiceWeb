using agape.lib.constant;
using Agape.FocusOne.Utilities;
using Agape.Lib.Web.Bean.CS;
using ERPW.Lib.Service;
using ServiceWeb.Accountability.Service;
using ServiceWeb.LinkFlowChart.Service;
using ServiceWeb.Service;
using ERPW.Lib.Authentication;
using SNA.Lib.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceWeb.auth;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Entity;
using ServiceWeb.KM;
using Newtonsoft.Json;
using ERPW.Lib.Service.Entity;
using Newtonsoft.Json.Linq;
using SNAWeb.Analytics;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Service.Workflow;
using ERPW.Lib.Service.Workflow.Entity;
using ERPW.Lib.F1WebService.ICMUtils;
using ServiceWeb.widget.usercontrol;
using System.Web.Configuration;
using Agape.Lib.DBService;
using ERPW.Lib.Authentication.Entity;
using System.IO;
using System.Diagnostics;
using AjaxControlToolkit;

namespace ServiceWeb.crm.AfterSale
{
    public partial class ServiceCallTransaction : AbstractsSANWebpage
    {
        string ThisPage = "ServiceCallTransaction";
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



        protected override Boolean ProgramPermission_CanView()
        {
            string businessObject = mBusinessObject;
            if (string.IsNullOrEmpty(businessObject))
            {
                //string _doctype = "";

                //if (serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
                //    {
                //        _doctype = Convert.ToString(dr["DocType"]);
                //    }
                //}
                //else
                //{
                //    _doctype = DocType;
                //}

                businessObject = lib.GetBusinessObjectFromTicketType(SID, DocType);
            }

            if (string.IsNullOrEmpty(businessObject))
            { ClientService.OpenSessionTimedOutFade(); return false; }
            else if (businessObject == ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_INCIDENT)
            { return Permission.IncidentView || Permission.AllPermission; }
            else if (businessObject == ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_REQUEST)
            { return Permission.RequestView || Permission.AllPermission; }
            else if (businessObject == ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_PROBLEM)
            { return Permission.ProblemView || Permission.AllPermission; }
            else
            { return false; }
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            string businessObject = mBusinessObject;
            if (string.IsNullOrEmpty(businessObject))
            {
                //string _doctype = "";

                //if (serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
                //    {
                //        _doctype = Convert.ToString(dr["DocType"]);
                //    }
                //}
                //else
                //{
                //    _doctype = DocType;
                //}

                businessObject = lib.GetBusinessObjectFromTicketType(SID, DocType);
            }

            if (string.IsNullOrEmpty(businessObject))
            { return false; }
            else if (businessObject == ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_INCIDENT)
            { return Permission.IncidentModify || Permission.AllPermission; }
            else if (businessObject == ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_REQUEST)
            { return Permission.RequestModify || Permission.AllPermission; }
            else if (businessObject == ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_PROBLEM)
            { return Permission.ProblemModify || Permission.AllPermission; }
            else
            { return false; }
        }

        #region 
        private string _idGen;
        public string idGen
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

                if (_serviceCallEntity.cs_servicecall_header.PrimaryKey.Length != 6)
                    _serviceCallEntity.cs_servicecall_header.PrimaryKey = new DataColumn[] { 
                        _serviceCallEntity.cs_servicecall_header.Columns["SID"],
                        _serviceCallEntity.cs_servicecall_header.Columns["CompanyCode"],
                        _serviceCallEntity.cs_servicecall_header.Columns["CallerID"],
                        _serviceCallEntity.cs_servicecall_header.Columns["CustomerCode"],
                        _serviceCallEntity.cs_servicecall_header.Columns["Doctype"],
                        _serviceCallEntity.cs_servicecall_header.Columns["Fiscalyear"]
                    };

                if (_serviceCallEntity.cs_servicecall_item.PrimaryKey.Length != 4)
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

        Agape.Lib.DBService.DBService DBService = new Agape.Lib.DBService.DBService();
        UniversalService universalService = new UniversalService();
        ServiceLibrary libService = new ServiceLibrary();
        ServiceTicketLibrary lib = new ServiceTicketLibrary();
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        AccountabilityService accountabilityService = new AccountabilityService();
        public const string WorkGroupCode = "20170121162748444411";
        LogServiceLibrary libLog = new LogServiceLibrary();
        private MasterConfigLibrary libconfig = MasterConfigLibrary.GetInstance();

        KMServiceLibrary libkm = KMServiceLibrary.getInstance();
        WorkflowService libWorkFlow = WorkflowService.getInstance();
        EquipmentService libCI = new EquipmentService();
        private TierZeroLibrary libTierZero = TierZeroLibrary.GetInstance();        

        private const string TicketTypeChangeOrder = "C";

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

        public bool PageEnableEdit
        {
            get
            {
                if (hddPageTicketMode.Value == "Display" || hddPageTicketMode.Value == "")
                {
                    return false;
                }

                return true;
            }
        }

        private string _sc_bobjectid;
        private string sc_bobjectid
        {
            get 
            {
                if (_sc_bobjectid == null)
                {
                    _sc_bobjectid = (string)Session["SCT_BObjectID" + idGen];
                }
                return _sc_bobjectid;
            }
            set 
            { 
                Session["SCT_BObjectID" + idGen] = value;
                _sc_bobjectid = value;
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

                return Docstatus == lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CLOSED);
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
                return Docstatus == lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CANCEL);
            }
        }

        #region - dt -
        //DataTable _dtEmpTemp;
        //DataTable dtEmpTemp
        //{
        //    get
        //    {
        //        if (dtEmpTemp == null)
        //        {
        //            dtEmpTemp = (DataTable)Session["SCT_dtEmpTemp" + idGen];
        //        }
        //        return dtEmpTemp; 

        //        //return Session["SCT_dtEmpTemp" + idGen] == null ? null : (DataTable)Session["SCT_dtEmpTemp" + idGen];
        //    }
        //    set 
        //    { 
        //        Session["SCT_dtEmpTemp" + idGen] = value;
        //    }
        //}

        //DataTable dtEmpCode
        //{
        //    get { return Session["SCT_dtEmpCode" + idGen] == null ? null : (DataTable)Session["SCT_dtEmpCode" + idGen]; }
        //    set { Session["SCT_dtEmpCode" + idGen] = value; }
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

        DataTable _dtTicketDocStatus;
        DataTable dtTicketDocStatus
        {
            get
            {

                if (_dtTicketDocStatus == null)
                {
                    _dtTicketDocStatus = lib.GetTicketDocStatus(SID, CompanyCode, false);
                }
                return _dtTicketDocStatus;
            }
        }

        DataTable _dtContactPerson;
        DataTable dtContactPerson
        {
            get 
            {
                if (_dtContactPerson == null)
                {
                    string custcode = CustomerCode_session == null ? "" : CustomerCode_session;
                    if (mode_stage == ApplicationSession.CREATE_MODE_STRING)
                    {
                        _dtContactPerson = AfterSaleService.getInstance().getContactPerson(CompanyCode, custcode, "TRUE");
                    }
                    else
                    {
                        _dtContactPerson = AfterSaleService.getInstance().getContactPerson(CompanyCode, custcode);
                    }
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

        private DataTable _dtMaterialMaster;
        private DataTable dtMaterialMaster
        {
            get 
            {
                if (_dtMaterialMaster == null)
                {
                    _dtMaterialMaster = materialService.getInSatnce().getMaterialByMatCode(SID, "");
                }
                return _dtMaterialMaster;
            }
        }

        //private DataTable _dtMaterialPurchase;
        //private DataTable dtMaterialPurchase
        //{
        //    get 
        //    {
        //        if (_dtMaterialPurchase == null)
        //        {
        //            _dtMaterialPurchase = AfterSaleService.getInstance().GetTicketMaterial(
        //                SID,
        //                CompanyCode,
        //                serviceCallEntity.cs_servicecall_header.Rows[0]["Fiscalyear"].ToString(),
        //                serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString(),
        //                serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString()
        //            );
        //        }
        //        return _dtMaterialPurchase;
        //    }
        //}

        private DataTable dtMaterialPurchase
        {
            get { return (DataTable)Session["dtMaterialPurchase" + idGen]; }
            set { Session["dtMaterialPurchase" + idGen] = value; }
        }

        protected string _isCritical;
        protected bool isCritical
        {
            get
            {
                if (string.IsNullOrEmpty(_isCritical))
                {
                    string cusCri = ServiceLibrary.LookUpTable(
                        "CustomerCritical", 
                        "ERPW_Master_Customer_General_Data", 
                        "WHERE SID = '" + SID + "' AND CompanyCode = '" + CompanyCode + "' AND CustomerCode = '" + CustomerCode + "'"
                    );
                    _isCritical = false.ToString();
                    if (cusCri == "CRITICAL")
                    {
                        _isCritical = true.ToString();
                    }
                }
                return Convert.ToBoolean(_isCritical);
            }
        }
        #endregion      

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
            //get { return false; }
            get { return Convert.ToBoolean(Session["ServiceCallTransaction_ShowWorkFlow" + idGen]); }
            set { Session["ServiceCallTransaction_ShowWorkFlow" + idGen] = value; }
        }

        public bool ShowWorkFlowWithoutCreate
        {
            //get { return false; }
            get { return Convert.ToBoolean(Session["ServiceCallTransaction_ShowWorkFlowWithoutCreate" + idGen]); }
            set { Session["ServiceCallTransaction_ShowWorkFlowWithoutCreate" + idGen] = value; }
        }

        public bool AlowWorkFlow
        {
            //get { return false; }
            get { return Convert.ToBoolean(Session["ServiceCallTransaction_AlowWorkFlow" + idGen]); }
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
            modalAddNewContact.afterSavedata += MyEventHandlerFunction_afterSaveRebindPostBack;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            Debug.WriteLine("start Page_Load");
            Stopwatch sw = Stopwatch.StartNew();

            if (!IsPostBack)
            {
                bool FilterOwner = false;
                bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);
                if (FilterOwner)
                {
                    ddlEquipmentNo.CssClass = "form-control form-control-sm required";
                }

                hddIDCurentTabView.Value = IDCurentTabView;
                Stopwatch sw2 = Stopwatch.StartNew();
                initData();
                sw2.Stop();
                Debug.WriteLine("Total Time taken initData: {0}ms", sw2.Elapsed.TotalMilliseconds);


                sw2 = Stopwatch.StartNew();
                if (mode_stage == ApplicationSession.CREATE_MODE_STRING)
                {
                    initData_CreateMode();                    
                }
                else if (mode_stage == ApplicationSession.CHANGE_MODE_STRING || mode_stage == ApplicationSession.DISPLAY_MODE_STRING)
                {
                    initData_ChangeMode();
                }
                sw2.Stop();
                Debug.WriteLine("Total Time taken mode_stage "+ mode_stage + ": {0}ms", sw2.Elapsed.TotalMilliseconds);

                initData_AfterInitMode();

                checkResolveStatus();
            }

            sw.Stop();
            Debug.WriteLine("Total Time taken: {0}ms", sw.Elapsed.TotalMilliseconds);

            if (string.IsNullOrEmpty(DocType))
            {
                //if (!string.IsNullOrEmpty(hddTicketDocType.Value) && 
                //    !string.IsNullOrEmpty(hddDocnumberTran.Value) && 
                //    !string.IsNullOrEmpty(_txt_fiscalyear.Value))
                //{

                //    string ApplicationMode = "Display";
                //    if (hddPageTicketMode.Value == "Create")
                //    {
                //        ApplicationMode = ApplicationSession.CREATE_MODE_STRING;
                //    }
                //    else if (hddPageTicketMode.Value == "Display")
                //    {
                //        ApplicationMode = ApplicationSession.DISPLAY_MODE_STRING;
                //    }
                //    else if (hddPageTicketMode.Value == "Change")
                //    {
                //        ApplicationMode = ApplicationSession.CHANGE_MODE_STRING;
                //    }

                //    getdataToedit(
                //        hddTicketDocType.Value,
                //        hddDocnumberTran.Value,
                //        _txt_fiscalyear.Value,
                //        true,
                //        false,
                //        ApplicationMode
                //    );
                //}
                //else
                {
                    ClientService.OpenSessionTimedOutFade();
                }
            }
            countMenu();
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

            string SERVICE_DOC_STATUS_RESOLVE = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE);
            string SERVICE_DOC_STATUS_CLOSED = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CLOSED);
            string SERVICE_DOC_STATUS_CANCEL = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CANCEL);

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
                ClientService.DoJavascript("controlInputDisable();");
            }
            else
            {
                ClientService.DoJavascript("controlInputEnable();");
            }

            try
            {
                ddlTicketStatus_Temp.Items[0].Attributes.Add("hidden", "hidden");
                ddlTicketStatus_Temp.Items.FindByValue(SERVICE_DOC_STATUS_RESOLVE).Attributes.Add("hidden", "hidden");
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
            mode_stage = ApplicationSession.CREATE_MODE_STRING;

            string PageRedirect = lib.getPageTicketRedirect(
                SID,
                docType
            );

            Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen));
        }

        #region initial Data
        private void initData()
        {
            ShowWorkFlow = true;
            ShowWorkFlowWithoutCreate = true;
            AlowWorkFlow = true;
            countActivity = 0;

            Session["SCT_GROUPCODE" + idGen] = null;
            clearScreen();
            initDataTemp();
            GetContact();
            GetProject();
            GetImpact();
            GetUrgency();
            GetTicketDocStatus();

            AutoCompleteEmployee.AfterSelectedChange = "bindDataTableInEmployeeParticipantSelected(v, 'TransferParticipant', '" + AutoCompleteEmployee.ClientID + "');";
            AutoCompleteEmployee_Escalate.AfterSelectedChange = "bindDataTableInEmployeeParticipantSelected(v, 'EscalateSetParticipant', '" + AutoCompleteEmployee_Escalate.ClientID + "');";
            
        }

        private void initData_CreateMode()
        {
            serviceCallEntity = new tmpServiceCallDataSet();

            initCreated();

            if (Session["SCT_created_equipment" + idGen] != null && Session["SCT_created_equipment" + idGen].ToString() != "")
            {
                if (ddlEquipmentNo.Items.FindByValue(Session["SCT_created_equipment" + idGen].ToString()) != null)
                {
                    ddlEquipmentNo.SelectedValue = Session["SCT_created_equipment" + idGen].ToString();

                    EquipmentSelected();
                }
                else
                {
                    SetDefaultRowEquipment();
                    BindDataTierOperation();
                }
            }
            else
            {
                SetDefaultRowEquipment();
                BindDataTierOperation();
            }

            bindDataAccountability();

            GetMaterialPurchase();
            BindingMaterialPurchase();
            setHeaderPropertyValue();
            GetPropertyValueItem();
            BindingPropertyValue();

        }

        private void initData_ChangeMode()
        {
            Stopwatch sw = Stopwatch.StartNew();
            GetEquipment_V2();
            sw.Stop();
            Debug.WriteLine("Total Time taken GetEquipment_V2: {0}ms", sw.Elapsed.TotalMilliseconds);

            sw = Stopwatch.StartNew();
            GetProblemGroup();
            sw.Stop();
            Debug.WriteLine("Total Time taken GetProblemGroup: {0}ms", sw.Elapsed.TotalMilliseconds);


            sw = Stopwatch.StartNew();
            GetOwnerGroupService();
            sw.Stop();
            Debug.WriteLine("Total Time taken GetOwnerGroupService: {0}ms", sw.Elapsed.TotalMilliseconds);


            sw = Stopwatch.StartNew();
            GetReferDocument();
            sw.Stop();
            Debug.WriteLine("Total Time taken GetReferDocument: {0}ms", sw.Elapsed.TotalMilliseconds);


            //sw = Stopwatch.StartNew();
            mapdataToscreen();
            //sw.Stop();
            //Debug.WriteLine("Total Time taken mapdataToscreen: {0}ms", sw.Elapsed.TotalMilliseconds);


            sw = Stopwatch.StartNew();
            controlscreen();
            sw.Stop();
            Debug.WriteLine("Total Time taken controlscreen: {0}ms", sw.Elapsed.TotalMilliseconds);


            sw = Stopwatch.StartNew();
            if (dtTier == null)
            {
                BindDataTierOperation();
            }
            sw.Stop();
            Debug.WriteLine("Total Time taken BindDataTierOperation: {0}ms", sw.Elapsed.TotalMilliseconds);


            sw = Stopwatch.StartNew();
            bindDataPopupCreateNewTicketReferent();
            sw.Stop();
            Debug.WriteLine("Total Time taken bindDataPopupCreateNewTicketReferent: {0}ms", sw.Elapsed.TotalMilliseconds);


            sw = Stopwatch.StartNew();
            getDataRelation();
            sw.Stop();
            Debug.WriteLine("Total Time taken getDataRelation: {0}ms", sw.Elapsed.TotalMilliseconds);


            sw = Stopwatch.StartNew();
            #region Workflow
            // Workflow
            bindDataAccountability();
            BindDataWorkflow();
            #endregion
            sw.Stop();
            Debug.WriteLine("Total Time taken Workflow: {0}ms", sw.Elapsed.TotalMilliseconds);


            sw = Stopwatch.StartNew();
            #region Knowledge Management
            //bindKnowledgeIDRefTicketNO();
            //bindDefaultSearchAddKnowledge();
            #endregion
            sw.Stop();
            Debug.WriteLine("Total Time taken Knowledge Management: {0}ms", sw.Elapsed.TotalMilliseconds);


            sw = Stopwatch.StartNew();
            GetMaterialPurchase();
            BindingMaterialPurchase();
            sw.Stop();
            Debug.WriteLine("Total Time taken MaterialPurchase: {0}ms", sw.Elapsed.TotalMilliseconds);


            sw = Stopwatch.StartNew();
            setHeaderPropertyValue();
            GetPropertyValueItem();
            sw.Stop();
            Debug.WriteLine("Total Time taken setHeaderPropertyValue: {0}ms", sw.Elapsed.TotalMilliseconds);


            sw = Stopwatch.StartNew();
            BindingPropertyValue();
            sw.Stop();
            Debug.WriteLine("Total Time taken BindingPropertyValue: {0}ms", sw.Elapsed.TotalMilliseconds);


            //hddAobjectlink.Value = AfterSaleService.getInstance().getActivityFromDocno("", _txt_docnumberTran.Value, _txt_docdateTran.Value.Substring(6, 4));
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

            Stopwatch sw = Stopwatch.StartNew();
            // turn speed 20191111
            //bindDataChangeLog();
            sw.Stop();
            Debug.WriteLine("Total Time taken bindDataChangeLog: {0}ms", sw.Elapsed.TotalMilliseconds);


            sw = Stopwatch.StartNew();
            // turn speed 20191111
            //BindDataLogFileAttachment();
            sw.Stop();
            Debug.WriteLine("Total Time taken BindDataLogFileAttachment: {0}ms", sw.Elapsed.TotalMilliseconds);

        }

        void initDataTemp()
        {
            string _doctype = "";

            foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
            {
                _doctype = Convert.ToString(dr["DocType"]);
            }
            //string businessObject = lib.GetBusinessObjectFromTicketType(SID, _doctype);
            mBusinessObject = businessObject;

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
            //dtSCType = AfterSaleService.getInstance().getSearchDoctype(
            //    "",
            //    CompanyCode,
            //    listBusinessObject
            //);

            //dtSCType = AfterSaleService.getInstance().getSearchDoctype("", CompanyCode);
            //dtCustomer = AfterSaleService.getInstance().getSearchCustomerCode("", CompanyCode);
            //dtGrouTemp = AfterSaleService.getInstance().GetDTProblemGroup(SID, businessObject);
            //dtProblemTypeTemp = AfterSaleService.getInstance().GetDTProblemType(SID,businessObject,"","");
            //dtDocstatus = AfterSaleService.getInstance().getCallStatus("");
            //dtTicketDocStatus = lib.GetTicketDocStatus(SID, CompanyCode, false);
        }

        private void initTextLabel(string TicketType)
        {
            //string businessObject = lib.GetBusinessObjectFromTicketType(SID, TicketType);

            string BusinessObjectDesc = ServiceTicketLibrary.GetBusinessObjectDesc(businessObject);

            lblEquipmentProblemGroupDesc.Text = BusinessObjectDesc + " Group";
            lblEquipmentProblemTypeDesc.Text = BusinessObjectDesc + " Type";
            lblEquipmentProblemSourceDesc.Text = BusinessObjectDesc + " Source";

        }

        protected void clearScreen()
        {
            _ddl_contact_person.SetValue = "";
            _ddl_contact_phone_noTran.SelectedIndex = 0;
            _ddl_contact_emailTran.SelectedIndex = 0;
            _ddl_contact_addressTran.SelectedIndex = 0;
            _ddl_projectcode.SelectedIndex = 0;
            _ddl_project_elementTran.SelectedIndex = 0;
            txt_problem_topic.Text = "";

            ddlAccountability.SelectedIndex = 0;
            //hddIncidentAreaCode.Value = "";
            tbSerialNo.Text = "";
            tbCallbackDate.Text = "";
            tbCallbackTime.Text = "";
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
                    List<logValue_OldNew> enLog = new List<logValue_OldNew>();

                    foreach (DataRow drHeader in serviceCallEntity.cs_servicecall_header.Rows)
                    {
                        if (Convert.ToString(drHeader["AffectSLA"]) != ddlAffectSLA.SelectedValue)
                        {

                            string textOldValue = "";
                            if (Convert.ToString(drHeader["AffectSLA"]) == "")
                            {
                                textOldValue = "";
                            }
                            else if (Convert.ToString(drHeader["AffectSLA"]) == "True")
                            {
                                textOldValue = "Affect SLA";
                            }
                            else if (Convert.ToString(drHeader["AffectSLA"]) == "False")
                            {
                                textOldValue = "Not Affect SLA";
                            }

                            string textNewValue = ddlAffectSLA.SelectedItem.Text;

                            enLog.Add(new logValue_OldNew
                            {
                                Value_Old = textOldValue,
                                Value_New = textNewValue,
                                TableName = "cs_servicecall_header",
                                FieldName = "Affect SLA",
                                AccessCode = LogServiceLibrary.AccessCode_Change
                            });
                        }
                    }

                    foreach (DataRow drItem in serviceCallEntity.cs_servicecall_item.Rows)
                    {
                        if (Convert.ToString(drItem["SummaryProblem"]) != tbSummaryProblem.Text)
                        {
                            enLog.Add(new logValue_OldNew
                            {
                                Value_Old = Convert.ToString(drItem["SummaryProblem"]),
                                Value_New = tbSummaryProblem.Text,
                                TableName = "cs_servicecall_item",
                                FieldName = "Summary Problem",
                                AccessCode = LogServiceLibrary.AccessCode_Change
                            });
                        }
                        if (Convert.ToString(drItem["SummaryCause"]) != tbSummaryCause.Text)
                        {
                            enLog.Add(new logValue_OldNew
                            {
                                Value_Old = Convert.ToString(drItem["SummaryCause"]),
                                Value_New = tbSummaryCause.Text,
                                TableName = "cs_servicecall_item",
                                FieldName = "Summary Cause",
                                AccessCode = LogServiceLibrary.AccessCode_Change
                            });
                        }
                        if (Convert.ToString(drItem["SummaryResolution"]) != tbSummaryResolution.Text)
                        {
                            enLog.Add(new logValue_OldNew
                            {
                                Value_Old = Convert.ToString(drItem["SummaryResolution"]),
                                Value_New = tbSummaryResolution.Text,
                                TableName = "cs_servicecall_item",
                                FieldName = "Summary Resolution",
                                AccessCode = LogServiceLibrary.AccessCode_Change
                            });
                        }
                    }

                    List<string> validationRS = validateForm();

                    if (validationRS.Count > 0)
                    {
                        string validationSTR = string.Join("<br/>", validationRS.ToArray());
                        throw new Exception(validationSTR);
                    }
                    validateDuplicatePropertyValue();

                    mapScreenItemToEntityUpdate();
                    PrepareEquipment(false);

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

                            // Turn Speed 20191111
                            //AttachFileUserControl.DocType = DocType;
                            //AttachFileUserControl.DocYear = (string)Session["SCT_created_fiscalyear" + idGen];
                            //AttachFileUserControl.DocNumber = hddDocnumberTran.Value.Trim();
                            //AttachFileUserControl.Commit(hddDocnumberTran.Value.Trim());
                            serviceCallEntity = new tmpServiceCallDataSet();

                            PrepareDataMaterialToTable();
                            DBService.SaveTransactionForFocusone(dtMaterialPurchase);

                            PreparePropertyValue();
                            SavePropertyValue((string)Session["SCT_created_fiscalyear" + idGen],DocType, hddDocnumberTran.Value.Trim());
                            Session["SCF_SAVESUCCESS" + idGen] = "Update Success Document Number : " + _txt_docnumberTran.Value;
                            getdataToedit(DocType, hddDocnumberTran.Value.Trim(), (string)Session["SCT_created_fiscalyear" + idGen]);
                        }
                    }
                    else
                    {
                        //List<logValue_OldNew> enLog = new List<logValue_OldNew>();
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

                        // turn speed 20191111
                        //AttachFileUserControl.DocType = DocType;
                        //AttachFileUserControl.DocYear = (string)Session["SCT_created_fiscalyear" + idGen];
                        //AttachFileUserControl.DocNumber = hddDocnumberTran.Value.Trim();
                        //AttachFileUserControl.Commit(hddDocnumberTran.Value.Trim());
                        serviceCallEntity = new tmpServiceCallDataSet();

                        PrepareDataMaterialToTable();
                        DBService.SaveTransactionForFocusone(dtMaterialPurchase);
                        PreparePropertyValue();
                        SavePropertyValue((string)Session["SCT_created_fiscalyear" + idGen], DocType, hddDocnumberTran.Value.Trim());
                        Session["SCF_SAVESUCCESS" + idGen] = "Update Success Document Number : " + _txt_docnumberTran.Value;
                        getdataToedit(DocType, hddDocnumberTran.Value.Trim(), (string)Session["SCT_created_fiscalyear" + idGen]);
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
                validateDuplicatePropertyValue();
                getDataSave();

                PrepareEquipment(false);

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
                        AttachFileUserControl.DocType = DocType;
                        AttachFileUserControl.DocYear = (string)Session["SCT_created_fiscalyear" + idGen];
                        AttachFileUserControl.DocNumber = returnMessage.Trim();
                        AttachFileUserControl.UpdateHeaderOnCommit = true;
                        AttachFileUserControl.Commit(returnMessage.Trim());
                        serviceCallEntity = new tmpServiceCallDataSet();

                        string ticketType = DocType;
                        string ticketNo = returnMessage.Trim();
                        string ticketYear = (string)Session["SCT_created_fiscalyear" + idGen];
                        string remark = txt_problem_topic.Text;

                        string displayDocNumber = AfterSaleService.getInstance().GetTicketNoForDisplay(
                            SID, CompanyCode, ticketType, returnMessage
                        );
                        PreparePropertyValue();
                        SavePropertyValue(ticketYear,ticketType,ticketNo);
                        Session["SCF_SAVESUCCESS" + idGen] = "Save Success Document Number : " + displayDocNumber;


                        //if (!string.IsNullOrEmpty(ddlEquipmentNo.SelectedValue))
                        //{
                        //}

                        AssignWork_SLA(ticketType, ticketNo, ticketYear, remark);

                        NotificationLibrary.GetInstance().TicketAlertEvent(
                            NotificationLibrary.EVENT_TYPE.TICKET_OPEN,
                            SID,
                            CompanyCode,
                            returnMessage,
                            EmployeeCode,
                            ThisPage
                        );

                        CreateWorkflow(ticketNo, "");

                        if ((string)Session["SC_MODE_REF" + idGen] == ServiceTicketLibrary.TICKET_CREATE_MODE_TIERZERO)
                        {
                            libTierZero.UpdateTierZeroDetail(
                                SID,
                                CompanyCode,
                                (string)Session["SC_MODE_REF_KEY" + idGen],
                                ticketType,
                                ticketNo,
                                TierZeroLibrary.TIER_ZERO_STATUS_CREATED,
                                CustomerCode,
                                "",
                                EmployeeCode
                            );
                        }

                        string ticket_ref_code = (Session["SCT_created_ticket_ref_code" + idGen] != null ? (Session["SCT_created_ticket_ref_code" + idGen] as string) : "");
                        //ClientService.DoJavascript("createdHierarchyRefer('" + ticketNo + "', '" + ticket_ref_code + "');");
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

                        PrepareDataMaterialToTable();

                        foreach (DataRow dr in dtMaterialPurchase.Rows)
                        {
                            dr["TicketNo"] = ticketNo;
                        }

                        DBService.SaveTransactionForFocusone(dtMaterialPurchase);

                        NotificationLibrary.GetInstance().TicketAlertEvent(
                            NotificationLibrary.EVENT_TYPE.ChangeOrder_Approval,
                            SID,
                            CompanyCode,
                            ticketNo + "|L0",
                            EmployeeCode,
                            ThisPage + "Change_CreateTicket"
                        );

                        getdataToedit(ticketType, ticketNo, ticketYear, true, false, ApplicationSession.CHANGE_MODE_STRING);
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

            //if (_ddl_priorityTran.SelectedIndex == -1)
            //{
            //    _rs.Add("กรุณาระบุ ลำดับความสำคัญ");
            //}
            //if (_ddl_servierityTran.SelectedIndex == -1 || _ddl_servierityTran.SelectedValue == "")
            //{
            //    _rs.Add("กรุณาระบุ ความเร่งด่วน");
            //}
            //if (_ddl_projectcode.SelectedIndex == -1 || string.IsNullOrEmpty(_ddl_projectcode.SelectedValue))
            //{
            //    _rs.Add("กรุณาระบุ โปรเจ็ค");
            //}
            //if (serviceCallEntity.Tables["cs_servicecall_item"].Select("", "", DataViewRowState.CurrentRows).Length <= 0)
            //{
            //    _rs.Add("กรุณาระบุ Item อย่่างน้อย 1 รายการ");
            //}                

            return _rs;
        }

        public void getDataSave()
        {
            if (serviceCallEntity.cs_servicecall_header.Rows.Count <= 0)
            {
                throw new Exception("No Data found in Header!!");
            }

            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< GetData Header >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            string doctype = Session["SCT_created_doctype_code" + idGen] == null ? "" : DocType;
            string fiscalyear = Session["SCT_created_fiscalyear" + idGen] == null ? "" : (string)Session["SCT_created_fiscalyear" + idGen];
            DataRow drHeader = serviceCallEntity.cs_servicecall_header.Rows[0];

            drHeader["sid"] = SID;
            drHeader["DocType"] = doctype;
            drHeader["Fiscalyear"] = fiscalyear;
            drHeader["DOCDATE"] = Validation.Convert2DateDB(Validation.getCurrentServerDate());
            drHeader["CompanyCode"] = CompanyCode;
            drHeader["CustomerCode"] = CustomerCode_session;
            drHeader["CREATED_BY"] = EmployeeCode;
            drHeader["CREATED_ON"] = Validation.getCurrentServerStringDateTime();
            drHeader["ContractPersonTel"] = _ddl_contact_phone_noTran.SelectedValue;
            drHeader["ContractPersonName"] = _ddl_contact_person.SelectText;
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

            if (tbCallbackDate.Text != "")
            {
                if (tbCallbackTime.Text == "")
                {
                    throw new Exception("Call back time is required.");
                }

                drHeader["CallbackDate"] = Validation.Convert2DateDB(tbCallbackDate.Text);
                drHeader["CallbackTime"] = Validation.Convert2TimeDB(tbCallbackTime.Text + ":00");
            }
            else
            {
                drHeader["CallbackDate"] = "";
                drHeader["CallbackTime"] = "";
            }

            //int round = 1;
            //foreach (RepeaterItem rptItem in rptContact.Items)
            //{
            //    DataRow drContact = serviceCallEntity.Tables["cs_servicecall_contract"].NewRow();
            //    drContact["SID"] = SID;
            //    drContact["ObjectID"] = Session["ObjectID"].ToString();
            //    drContact["ItemNo"] = (round++).ToString().PadLeft(4, '0');
            //    drContact["PersonName"] = (rptItem.FindControl("lbContactCode") as Label).Text;
            //    drContact["EMail"] = (rptItem.FindControl("lbContactEmail") as Label).Text;
            //    drContact["Telephone"] = (rptItem.FindControl("lbContactPhone") as Label).Text;
            //    drContact["PersonAddress"] = (rptItem.FindControl("lbContactAddress") as Label).Text;
            //    drContact["Remark"] = (rptItem.FindControl("lbContactRemark") as Label).Text;
            //    drContact["CREATED_BY"] = EmployeeCode;
            //    drContact["CREATED_ON"] = Validation.getCurrentServerStringDateTime();

            //    serviceCallEntity.Tables["cs_servicecall_contract"].Rows.Add(drContact);
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
                        //Response.-Redirect("~/crm/AfterSale/ServiceCallTransaction.aspx?id=" + idGen);
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
                dr["ContractPersonName"] = _ddl_contact_person.SelectText;
                dr["Email"] = _ddl_contact_emailTran.SelectedValue;
                dr["Address"] = _ddl_contact_addressTran.SelectedValue;
                dr["ProjectCode"] = _ddl_projectcode.SelectedValue;
                dr["ProjectElement"] = _ddl_project_elementTran.SelectedValue;
                dr["ReferenceDocument"] = tbRefer.Text;
                dr["Impact"] = ddlImpact.SelectedValue;
                dr["Urgency"] = ddlUrgency.SelectedValue;
                dr["Priority"] = _ddl_priorityTran.SelectedValue;
                dr["MajorIncident"] = chkMajorIncident.Checked.ToString();
                //dr["Docstatus"] = _ddl_ticket_Doc_Status.SelectedValue;

                if (tbCallbackDate.Text != "")
                {
                    if (tbCallbackTime.Text == "")
                    {
                        throw new Exception("Call back time is required.");
                    }

                    dr["CallbackDate"] = Validation.Convert2DateDB(tbCallbackDate.Text);
                    dr["CallbackTime"] = Validation.Convert2TimeDB(tbCallbackTime.Text + ":00");
                }
                else
                {
                    dr["CallbackDate"] = "";
                    dr["CallbackTime"] = "";
                }
            }

            //int maxValue = 1;
            //foreach (RepeaterItem rptItem in rptContact.Items)
            //{
            //    DataRow drContact = serviceCallEntity.Tables["cs_servicecall_contract"].NewRow();

            //    if (serviceCallEntity.Tables["cs_servicecall_contract"].Select("PersonName = '" + (rptItem.FindControl("lbContactCode") as Label).Text + "'").Count() == 0)
            //    {
            //        if (serviceCallEntity.Tables["cs_servicecall_contract"].Rows.Count > 0)
            //        {
            //            maxValue = Convert.ToInt32(serviceCallEntity.Tables["cs_servicecall_contract"].Compute("max([Itemno])", string.Empty)) + 1;
            //        }

            //        drContact["SID"] = SID;
            //        drContact["ObjectID"] = serviceCallEntity.Tables["cs_servicecall_header"].Rows[0]["ObjectID"];
            //        drContact["ItemNo"] = (rptItem.FindControl("lbItemNo") as Label).Text == "" ? maxValue.ToString().PadLeft(4, '0') : (rptItem.FindControl("lbItemNo") as Label).Text;
            //        drContact["PersonName"] = (rptItem.FindControl("lbContactCode") as Label).Text;
            //        drContact["EMail"] = (rptItem.FindControl("lbContactPhone") as Label).Text;
            //        drContact["Telephone"] = (rptItem.FindControl("lbContactEmail") as Label).Text;
            //        drContact["PersonAddress"] = (rptItem.FindControl("lbContactAddress") as Label).Text;
            //        drContact["Remark"] = (rptItem.FindControl("lbContactRemark") as Label).Text;
            //        drContact["CREATED_BY"] = EmployeeCode;
            //        drContact["CREATED_ON"] = Validation.getCurrentServerStringDateTime();

            //        serviceCallEntity.Tables["cs_servicecall_contract"].Rows.Add(drContact);
            //    }
            //}
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
                string newTicketType = DocType;
                string newTicketYear = (string)Session["SCT_created_fiscalyear" + idGen];
                string newCustomerCode = CustomerCode_session;

                serviceCallEntity = lib.CopyTicket(sessionId, SID, CompanyCode,
                    refTicketType, refTicketYear, refTicketNo,
                    newTicketType, newTicketYear, newCustomerCode);

                //Session["SCT_created_ref" + idGen] = null;
                //Session["SCT_created_ref_doctype" + idGen] = null;
                //Session["SCT_created_ref_fiscalyear" + idGen] = null;
            }

            GetEquipment_V2();
            GetProblemGroup();
            GetOwnerGroupService();
            GetReferDocument();

            mapdataToscreen();

            if (chkIsLoad_Attachment.Checked)
                setAttachMent("", false
                    , DocType, true
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

            string docType = Session["SCT_created_doctype_code" + idGen] == null ? "" : DocType;
            string fiscalyear = Session["SCT_created_fiscalyear" + idGen] == null ? "" : (string)Session["SCT_created_fiscalyear" + idGen];
            string obj = SID + docType + fiscalyear + getCallerID();

            if (serviceCallEntity.cs_servicecall_header.Rows.Count <= 0)
            {
                DataRow dr = serviceCallEntity.cs_servicecall_header.NewRow();
                dr["sid"] = SID;
                dr["CompanyCode"] = CompanyCode;
                dr["CustomerCode"] = CustomerCode_session;
                dr["ContractPersonName"] = Session["SCT_created_contact_desc" + idGen] == null ? "" : (string)Session["SCT_created_contact_desc" + idGen];
                dr["CallerID"] = "";
                dr["ObjectID"] = Session["ObjectID" + idGen] = obj;
                dr["DocType"] = docType;
                dr["Fiscalyear"] = fiscalyear;
                dr["DOCDATE"] = Validation.getCurrentServerStringDateTime().Substring(0, 8);
                dr["CallStatus"] = ServiceTicketLibrary.SERVICE_CALL_STATUS_OPEN;
                dr["CREATED_BY"] = EmployeeCode;

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

                if (dr["DocType"].ToString() == TicketTypeChangeOrder)
                {
                    ShowWorkFlowWithoutCreate = true;
                    AlowWorkFlow = true;
                }

                if (Session["SCT_created_description" + idGen] != null)
                {
                    tbEquipmentRemark.Text = (string)Session["SCT_created_description" + idGen];
                }
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
            //Session["listAttachmentUserControl"] = null;
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
            Stopwatch sw = Stopwatch.StartNew();

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

                hddDocnumberTran.Value = dr["CallerID"].ToString();
                hddTicketStatus.Value = dr["Docstatus"].ToString();
                hddTicketStatus_Old.Value = dr["Docstatus"].ToString();
                hddTicketStatus_New.Value = dr["Docstatus"].ToString();

                lblTicketNo_Modal.Text = dr["CallerID"].ToString();

                txt_problem_topic.Text = dr["HeaderText"].ToString();
                hddOldValue_problem_topic.Value = dr["HeaderText"].ToString();

                _txt_doctypeTran.Value = GetSCTypeDesc(dr["DocType"].ToString());

                ////Lebel////
                
                string []TicketType = GetSCTypeDesc(dr["DocType"].ToString()).Split(':');
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

                hddTicketDocType.Value = dr["DocType"].ToString();

                initTextLabel(dr["DocType"].ToString());

                _txt_fiscalyear.Value = dr["FiscalYear"].ToString();
                _txt_customerTran.Value = GetCustomerDesc(dr["CustomerCode"].ToString());
                _txt_customertran_responsible_organization.Value = ServiceTicketLibrary.LookUpTable(
                    "ResponsibleOrganization", 
                    "ERPW_Master_Customer_General_Data",
                    "where SID = '" + SID + "' AND CompanyCode = '" + CompanyCode + "' AND CustomerCode = '" + dr["CustomerCode"].ToString() + "'"
                );
                hddCustomerCode.Value = dr["CustomerCode"].ToString();

                CustomerCode = dr["CustomerCode"].ToString();

                sw = Stopwatch.StartNew();
                GetAreaCodeFromDocumentType(dr["DocType"].ToString());
                sw.Stop();
                Debug.WriteLine("Total Time taken GetAreaCodeFromDocumentType: {0}ms", sw.Elapsed.TotalMilliseconds);

                if (dr["DocType"].ToString() == TicketTypeChangeOrder)
                {
                    ShowWorkFlow = true;
                    ShowWorkFlowWithoutCreate = true;
                    AlowWorkFlow = true;
                }

                Session["SCT_created_cust_code" + idGen] = dr["CustomerCode"].ToString();
                Session["SCT_created_doctype_code" + idGen] = dr["DocType"].ToString();
                Session["SCT_created_fiscalyear" + idGen] = dr["Fiscalyear"].ToString();
                Session["responsecall_objid" + idGen] = dr["ObjectID"].ToString();

                sw = Stopwatch.StartNew();
                GetContactPerson();
                _ddl_contact_person.SetValueFromName = dr["ContractPersonName"].ToString();
                GetcontactDetailForScreen();
                sw.Stop();
                Debug.WriteLine("Total Time taken GetContactPerson: {0}ms", sw.Elapsed.TotalMilliseconds);


                _ddl_contact_phone_noTran.SelectedValue = dr["ContractPersonTel"].ToString();
                _ddl_contact_emailTran.SelectedValue = dr["Email"].ToString();
                _ddl_contact_addressTran.SelectedValue = dr["Address"].ToString();
                _ddl_projectcode.SelectedValue = dr["ProjectCode"].ToString();

                sw = Stopwatch.StartNew();
                GetProjectElement();
                sw.Stop();
                Debug.WriteLine("Total Time taken GetProjectElement: {0}ms", sw.Elapsed.TotalMilliseconds);


                _ddl_project_elementTran.SelectedValue = dr["ProjectElement"].ToString();

                _txt_companyTran.Value = CompanyName;
                _txt_docdateTran.Value = Validation.Convert2DateDisplay(dr["DOCDATE"].ToString());
                _txt_docstatusTran.Value = GetDocStatusDesc(dr["CallStatus"].ToString());
                _txt_TicketStatusTran.Value = dr["Docstatus"].ToString() + " : " + ServiceTicketLibrary.GetTicketDocStatusDesc(SID, CompanyCode, dr["Docstatus"].ToString());

                tbRefer.Text = dr["ReferenceDocument"].ToString();
                ddlImpact.SelectedValue = dr["Impact"].ToString();
                ddlUrgency.SelectedValue = dr["Urgency"].ToString();

                sw = Stopwatch.StartNew();
                GetSeverity();
                sw.Stop();
                Debug.WriteLine("Total Time taken GetSeverity: {0}ms", sw.Elapsed.TotalMilliseconds);

                _ddl_priorityTran.SelectedValue = dr["Priority"].ToString();

                tbCreatedBy.Text = GetEmployeeNameWithCode(dr["CREATED_BY"].ToString());
                //tbCreatedOn.Text = Validation.Convert2DateTimeDisplay(dr["CREATED_ON"].ToString());

                bool majorIncident = false;
                bool.TryParse(dr["MajorIncident"].ToString(), out majorIncident);

                chkMajorIncident.Checked = majorIncident;

                ddlAffectSLA.SelectedValue = dr["AffectSLA"].ToString();

                tbCallbackDate.Text = Validation.Convert2DateDisplay(dr["CallbackDate"].ToString());

                if (!string.IsNullOrEmpty(dr["CallbackTime"].ToString()))
                {
                    tbCallbackTime.Text = Validation.Convert2TimeDisplay(dr["CallbackTime"].ToString()).Substring(0, 5);
                }
                else
                {
                    tbCallbackTime.Text = "";
                }

                if (chkIsLoad_Attachment.Checked)
                    setAttachMent("", false
                       , DocType, true
                       , hddDocnumberTran.Value.Trim(), true
                       , (string)Session["SCT_created_fiscalyear" + idGen], true);
            }

            if (serviceCallEntity.cs_servicecall_item.Rows.Count > 0)
            {
                DataRow dr = serviceCallEntity.cs_servicecall_item.Rows[0];

                ddlEquipmentNo.SelectedValue = dr["EquipmentNo"].ToString();

                if (mode_stage == ApplicationSession.CHANGE_MODE_STRING || mode_stage == ApplicationSession.DISPLAY_MODE_STRING)
                {
                    if (string.IsNullOrEmpty(ddlEquipmentNo.SelectedValue))
                    {
                        ddlEquipmentNo.CssClass = ddlEquipmentNo.CssClass + " ticket-allow-editor";
                        btnEquipmentChange.CssClass = btnEquipmentChange.CssClass + " ticket-allow-editor";
                        tbSerialNo.CssClass = tbSerialNo.CssClass + " ticket-allow-editor";
                        ddlOwnerGroupService.CssClass = ddlOwnerGroupService.CssClass + " ticket-allow-editor";
                        ddlProblemGroup.CssClass = ddlProblemGroup.CssClass + " ticket-allow-editor";
                        btnChangeProblemGroup.CssClass = btnChangeProblemGroup.CssClass + " ticket-allow-editor";
                        ddlProblemType.CssClass = ddlProblemType.CssClass + " ticket-allow-editor";
                        btnChangeProblemType.CssClass = btnChangeProblemType.CssClass + " ticket-allow-editor";
                        ddlProblemSource.CssClass = ddlProblemSource.CssClass + " ticket-allow-editor";
                        btnChangeProblemSource.CssClass = btnChangeProblemSource.CssClass + " ticket-allow-editor";
                        ddlServiceSource.CssClass = ddlServiceSource.CssClass + " ticket-allow-editor";
                        btnChangeServiceSource.CssClass = btnChangeServiceSource.CssClass + " ticket-allow-editor";

                        ddlImpact.CssClass = "form-control form-control-sm required ticket-allow-editor";
                        ddlUrgency.CssClass = "form-control form-control-sm required ticket-allow-editor";

                        ddlTransfer_SLAGroup.CssClass = ddlTransfer_SLAGroup.CssClass + " ticket-allow-editor";
                        ddlEscalate_SLAGroup.CssClass = ddlEscalate_SLAGroup.CssClass + " ticket-allow-editor";

                        udpnOwnerService.Update();
                        udpnEquipment.Update();
                        udpnEQDetail.Update();
                        udpnProblem.Update();
                    }

                    try
                    {
                        if (string.IsNullOrEmpty(ddlImpact.SelectedValue))
                        {
                            ddlImpact.CssClass = "form-control form-control-sm required ticket-allow-editor";
                        }

                        if (string.IsNullOrEmpty(ddlImpact.SelectedValue))
                        {
                            ddlUrgency.CssClass = "form-control form-control-sm required ticket-allow-editor";
                        }
                    }
                    catch (Exception)
                    {
                        
                    }
                }

                tbSerialNo.Text = dr["SerialNo"].ToString();

                ddlOwnerGroupService.SelectedValue = dr["QueueOption"].ToString();

                sw = Stopwatch.StartNew();
                BindProblemDetail(
                    dr["ProblemGroup"].ToString(),
                    dr["ProblemTypeCode"].ToString(),
                    dr["OriginCode"].ToString()
                );
                sw.Stop();
                Debug.WriteLine("Total Time taken: {0}ms", sw.Elapsed.TotalMilliseconds);

                try
                {
                    ddlProblemGroup.SelectedValue = dr["ProblemGroup"].ToString();
                    ddlProblemType.SelectedValue = dr["ProblemTypeCode"].ToString();
                    ddlProblemSource.SelectedValue = dr["OriginCode"].ToString();
                    ddlServiceSource.SelectedValue = dr["CallTypeCode"].ToString();
                }
                catch (Exception)
                {

                }

                tbEquipmentRemark.Text = dr["Remark"].ToString();
                hddOldValue_EquipmentRemark.Value = dr["Remark"].ToString();
                tbSummaryProblem.Text = dr["SummaryProblem"].ToString();
                tbSummaryCause.Text = dr["SummaryCause"].ToString();
                tbSummaryResolution.Text = dr["SummaryResolution"].ToString();
                //ddlOwnerGroupService.SelectedValue = 
                //hddIncidentAreaCode.Value = Convert.ToString(dr["IncidentArea"]); // Zaan Todo
                //udpUpdateAreaCode.Update();

                sw = Stopwatch.StartNew();
                ItemRowChange(dr["xLineNo"].ToString());
                sw.Stop();
                Debug.WriteLine("Total Time taken ItemRowChange: {0}ms", sw.Elapsed.TotalMilliseconds);

                sw = Stopwatch.StartNew();
                if (chkIsLoad_CIRelation.Checked)
                    GetEquipmentRelation(dr["EquipmentNo"].ToString());
                sw.Stop();
                Debug.WriteLine("Total Time taken GetEquipmentRelation: {0}ms", sw.Elapsed.TotalMilliseconds);

                if (string.IsNullOrEmpty(dr["EquipmentNo"].ToString()))
                {
                    ddlOwnerGroupService.Enabled = true;
                }
                else
                {
                    ddlOwnerGroupService.Enabled = false;
                }
                udpnOwnerService.Update();
            }
            else
            {
                ddlOwnerGroupService.Enabled = true;
                udpnOwnerService.Update();
            }

            sw = Stopwatch.StartNew();
            // turn speed 20191111
            //bindDataTicketDateTimeLog();
            sw.Stop();
            Debug.WriteLine("Total Time taken bindDataTicketDateTimeLog: {0}ms", sw.Elapsed.TotalMilliseconds);

            if (string.IsNullOrEmpty(ddlEquipmentNo.SelectedValue) && string.IsNullOrEmpty(ddlOwnerGroupService.SelectedValue))
            {
                ddlTransfer_OwnerService.CssClass = ddlTransfer_OwnerService.CssClass + " ticket-allow-editor";
                ddlEscalate_OwnerService.CssClass = ddlEscalate_OwnerService.CssClass + " ticket-allow-editor";
            }
        }

        protected void controlscreen()
        {
            string status = "", docnumber = "", docStatus = "";

            ddlEquipmentNo.Enabled = true;

            foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
            {
                docnumber = dr["CallerID"].ToString();
                status = dr["CallStatus"].ToString();
                docStatus = dr["Docstatus"].ToString();
            }

            _txt_docstatusTran.Value = GetDocStatusDesc(status);
            _txt_TicketStatusTran.Value = docStatus + " : " + ServiceTicketLibrary.GetTicketDocStatusDesc(SID, CompanyCode, docStatus);

            if (ServiceTicketLibrary.SERVICE_CALL_STATUS_CANCEL.Equals(status) || ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE.Equals(status))
            {
                //btn_CanceldocTran.Visible = false;
                btnConfirmClientTran.Visible = false;
                ddlEquipmentNo.Enabled = false;

                ddlAffectSLA.Enabled = false;
                tbSummaryProblem.Enabled = false;
                tbSummaryCause.Enabled = false;
                tbSummaryResolution.Enabled = false;
                tbRefer.Enabled = false;
                tbCallbackDate.Enabled = false;
                tbCallbackTime.Enabled = false;
                txt_problem_topic.Enabled = false;
                tbEquipmentRemark.Enabled = false;
            }
            else if (ServiceTicketLibrary.SERVICE_CALL_STATUS_OPEN.Equals(status) && !string.IsNullOrEmpty(docnumber))
            {
                //btn_CanceldocTran.Visible = true;
                btnConfirmClientTran.Visible = true;
            }
            else
            {
                //btn_CanceldocTran.Visible = false;
                btnConfirmClientTran.Visible = true;
            }
        }

        #endregion


        private void GetContact()
        {

            //string custcode = CustomerCode_session == null ? "" : CustomerCode_session;//_ddl_customer_code.SelectedIndex != -1 ? _ddl_customer_code.SelectedValue : "";
            //dtContactPerson = AfterSaleService.getInstance().getContactPerson(CompanyCode, custcode);
            _ddl_contact_person.initialDataAutoComplete(dtContactPerson, "BOBJECTLINK", "NAME1", false, "email", "phone", "remark");
            udpContactRefresh.Update();
        }

        // <Contact Phone for Print>
        public string CtPhone;
        // <Contact Phone for Print>

        private void GetcontactDetailForScreen()
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

            txtContactPhone.Text = en.phone.Trim();
            // <Contact Phone for Print>
            CtPhone = en.phone.Trim();
            // <Contact Phone for Print>
            txtContactEmail.Text = en.email.Trim();
            txtContactRemark.Text = en.REMARK.Trim();

        }

        //private void GetcontactPhone()
        //{
        //    _ddl_contact_phone_noTran.Items.Clear();
        //    string bobjectid = _ddl_contact_person.SelectedIndex != -1 ? _ddl_contact_person.SelectedValue : "";
        //    dtContactPhone = AfterSaleService.getInstance().getContactPhon(bobjectid);
        //    _ddl_contact_phone_noTran.Items.Add(new ListItem("", ""));
        //    _ddl_contact_phone_noTran.AppendDataBoundItems = true;
        //    _ddl_contact_phone_noTran.DataTextField = "PHONENUMBER";
        //    _ddl_contact_phone_noTran.DataValueField = "PHONENUMBER";
        //    _ddl_contact_phone_noTran.DataSource = dtContactPhone;
        //    _ddl_contact_phone_noTran.DataBind();
        //}

        //private void GetContactEmail()
        //{
        //    _ddl_contact_emailTran.Items.Clear();
        //    string bobjectid = _ddl_contact_person.SelectedIndex != -1 ? _ddl_contact_person.SelectedValue : "";
        //    dtContactEmail = AfterSaleService.getInstance().getContactEmail(bobjectid);
        //    _ddl_contact_emailTran.Items.Add(new ListItem("", ""));
        //    _ddl_contact_emailTran.AppendDataBoundItems = true;
        //    _ddl_contact_emailTran.DataTextField = "EMAIL";
        //    _ddl_contact_emailTran.DataValueField = "EMAIL";
        //    _ddl_contact_emailTran.DataSource = dtContactEmail;
        //    _ddl_contact_emailTran.DataBind();
        //}

        //private void GetContactAddress()
        //{
        //    _ddl_contact_addressTran.Items.Clear();
        //    string bobjectid = _ddl_contact_person.SelectedIndex != -1 ? _ddl_contact_person.SelectedValue : "";
        //    dtContactAddress = AfterSaleService.getInstance().getContactAddress(bobjectid);
        //    _ddl_contact_addressTran.Items.Add(new ListItem("", ""));
        //    _ddl_contact_addressTran.AppendDataBoundItems = true;
        //    _ddl_contact_addressTran.DataTextField = "ADDRESSTEXT";
        //    _ddl_contact_addressTran.DataValueField = "ADDRESSTEXT";
        //    _ddl_contact_addressTran.DataSource = dtContactAddress;
        //    _ddl_contact_addressTran.DataBind();
        //}

        private void GetTicketDocStatus()
        {
            //string SERVICE_DOC_STATUS_RESOLVE = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE);
            //string SERVICE_DOC_STATUS_CLOSED = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CLOSED);
            //string SERVICE_DOC_STATUS_CANCEL = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CANCEL);            

            ddlTicketStatus_Temp.DataTextField = "DocumentStatusDesc";
            ddlTicketStatus_Temp.DataValueField = "DocumentStatus";
            ddlTicketStatus_Temp.DataSource = dtTicketDocStatus;
            ddlTicketStatus_Temp.DataBind();

            //ddlTicketStatus_Temp.Items.FindByValue(SERVICE_DOC_STATUS_RESOLVE).Attributes.Add("hidden", "hidden");
            //ddlTicketStatus_Temp.Items.FindByValue(SERVICE_DOC_STATUS_CLOSED).Attributes.Add("hidden", "hidden");
            //ddlTicketStatus_Temp.Items.FindByValue(SERVICE_DOC_STATUS_CANCEL).Attributes.Add("hidden", "hidden");
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
            //dtProjectCode = AfterSaleService.getInstance().getProjectCode(SID, WorkGroupCode, CustomerCode_session == null ? "" : CustomerCode_session.ToString());
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
            //string projectcode = _ddl_projectcode.SelectedIndex != -1 ? _ddl_projectcode.SelectedValue : "";
            //dtProjectElement = AfterSaleService.getInstance().getProjectElement(SID, CompanyCode, projectcode);
            _ddl_project_elementTran.Items.Add(new ListItem("", ""));
            _ddl_project_elementTran.AppendDataBoundItems = true;
            _ddl_project_elementTran.DataTextField = "ELEMENTDESC";
            _ddl_project_elementTran.DataValueField = "BOMID";
            _ddl_project_elementTran.DataSource = dtProjectElement;
            _ddl_project_elementTran.DataBind();
        }

        private void GetContactPerson()
        {
            //string custcode = CustomerCode_session == null ? "" : CustomerCode_session;//_ddl_customer_code.SelectedIndex != -1 ? _ddl_customer_code.SelectedValue : "";
            //dtContactPerson = AfterSaleService.getInstance().getContactPerson(CompanyCode, custcode);
            _ddl_contact_person.initialDataAutoComplete(dtContactPerson, "BOBJECTLINK", "NAME1", false, "email", "phone", "remark");

            //try
            //{
            //    _ddl_contact_person.SelectedValue = Session["SCT_created_contact_code"] == null ? "" : (string)Session["SCT_created_contact_code"];
            //}
            //catch (Exception)
            //{

            //}

            udpContactRefresh.Update();
        }

        private void GetReferDocument()
        {
            //DataTable dt = AfterSaleService.getInstance().getServiceCall(SID, CompanyCode,
            //        serviceCallEntity.Tables["cs_servicecall_header"].Rows[0]["CustomerCode"].ToString(),
            //        serviceCallEntity.Tables["cs_servicecall_header"].Rows[0]["Fiscalyear"].ToString(),
            //        serviceCallEntity.Tables["cs_servicecall_header"].Rows[0]["DocType"].ToString(),
            //        "", "", "", "", "", "", "", "");

            //ddlRefer.DataTextField = "CallerID";
            //ddlRefer.DataValueField = "CallerID";
            //ddlRefer.DataSource = dt;
            //ddlRefer.DataBind();
            //ddlRefer.Items.Insert(0, new ListItem("", ""));
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

        //private string GetTicketDocStatusDesc(string code)
        //{
        //    DataRow[] drr = dtTicketDocStatus.Select("code='" + code + "'");
        //    string desc = "";
        //    if (drr.Length > 0)
        //    {
        //        desc = drr[0]["Description"].ToString();
        //    }
        //    return desc;
        //}

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

        #region Modalitem BindDropDown

        private void GetEquipment_V2()
        {
            DataTable dt = universalService.GetEquipmentCustomerAssignment(SID, CompanyCode, "", serviceCallEntity.cs_servicecall_header.Rows[0]["CustomerCode"].ToString());

            dt.Columns.Add("CodeWithDesc");

            foreach (DataRow dr in dt.Rows)
            {
                dr["CodeWithDesc"] = libService.PrepareCodeAndDescription(dr["EquipmentCode"].ToString(), dr["EquipmentName"].ToString());
            }

            ddlEquipmentNo.DataValueField = "EquipmentCode";
            ddlEquipmentNo.DataTextField = "CodeWithDesc";
            ddlEquipmentNo.DataSource = dt;
            ddlEquipmentNo.DataBind();
            ddlEquipmentNo.Items.Insert(0, new ListItem("", ""));
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
            mBusinessObject = businessObject;

            List<IncidentAreaEntity.AreaGroup> En = libconfig.getIncedentAreaGroup(
                SID, CompanyCode,
                mBusinessObject, ddlOwnerGroupService.SelectedValue
            );

            if (!string.IsNullOrEmpty(ddlOwnerGroupService.SelectedValue))
                En = En.Where(w => w.ChildrenOwnerGroup.Contains(ddlOwnerGroupService.SelectedValue)).ToList();

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

            if (!string.IsNullOrEmpty(problemGroup))
            {
                En = libconfig.getIncedentAreaType(
                    SID, CompanyCode,
                    mBusinessObject, ddlOwnerGroupService.SelectedValue, true, problemGroup
                );
                if (!string.IsNullOrEmpty(ddlOwnerGroupService.SelectedValue))
                    En = En.Where(w => w.ChildrenOwnerGroup.Contains(ddlOwnerGroupService.SelectedValue)).ToList();
            }

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

            if (!string.IsNullOrEmpty(IncidentGroup) && !string.IsNullOrEmpty(IncidentType))
            {
                En = libconfig.getIncedentAreaSource(
                    SID, CompanyCode,
                    mBusinessObject, ddlOwnerGroupService.SelectedValue, true, IncidentGroup, IncidentType
                );
                if (!string.IsNullOrEmpty(ddlOwnerGroupService.SelectedValue))
                    En = En.Where(w => w.ChildrenOwnerGroup.Contains(ddlOwnerGroupService.SelectedValue)).ToList();
            }

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

            if (!string.IsNullOrEmpty(IncidentGroup)
                && !string.IsNullOrEmpty(IncidentType)
                && !string.IsNullOrEmpty(IncidentSource))
            {
                En = libconfig.getIncedentAreaContactSource(
                    SID, CompanyCode,
                    mBusinessObject, ddlOwnerGroupService.SelectedValue, IncidentGroup, IncidentType, IncidentSource
                );
                if (!string.IsNullOrEmpty(ddlOwnerGroupService.SelectedValue))
                    En = En.Where(w => w.ChildrenOwnerGroup.Contains(ddlOwnerGroupService.SelectedValue)).ToList();
            }

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

        private void GetOwnerGroupService()
        {
            DataTable dtOwner = libconfig.GetMasterConfigOwnerGroup(SID, CompanyCode, "");
            ddlOwnerGroupService.DataValueField = "OwnerGroupCode";
            ddlOwnerGroupService.DataTextField = "OwnerGroupName";
            ddlOwnerGroupService.DataSource = dtOwner;
            ddlOwnerGroupService.DataBind();
            ddlOwnerGroupService.Items.Insert(0, new ListItem("", ""));

            ddlEscalate_OwnerService.DataValueField = "OwnerGroupCode";
            ddlEscalate_OwnerService.DataTextField = "OwnerGroupName";
            ddlEscalate_OwnerService.DataSource = dtOwner;
            ddlEscalate_OwnerService.DataBind();
            ddlEscalate_OwnerService.Items.Insert(0, new ListItem("", ""));

            ddlTransfer_OwnerService.DataValueField = "OwnerGroupCode";
            ddlTransfer_OwnerService.DataTextField = "OwnerGroupName";
            ddlTransfer_OwnerService.DataSource = dtOwner;
            ddlTransfer_OwnerService.DataBind();
            ddlTransfer_OwnerService.Items.Insert(0, new ListItem("", ""));

            DataTable dtSLAGroup = TierService.getInStance().searchTierGorupMaster(
                SID, WorkGroupCode, "", ""
            );
            ddlEscalate_SLAGroup.DataValueField = "TierGroupCode";
            ddlEscalate_SLAGroup.DataTextField = "TierGroupDescription";
            ddlEscalate_SLAGroup.DataSource = dtSLAGroup;
            ddlEscalate_SLAGroup.DataBind();
            ddlEscalate_SLAGroup.Items.Insert(0, new ListItem("", ""));

            ddlTransfer_SLAGroup.DataValueField = "TierGroupCode";
            ddlTransfer_SLAGroup.DataTextField = "TierGroupDescription";
            ddlTransfer_SLAGroup.DataSource = dtSLAGroup;
            ddlTransfer_SLAGroup.DataBind();
            ddlTransfer_SLAGroup.Items.Insert(0, new ListItem("", ""));

            udpnOwnerService.Update();
            udpEscalate_OwnerService.Update();
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
                //string businessObject = lib.GetBusinessObjectFromTicketType(SID, _doctype);
                return ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_CHANGE.Equals(businessObject);
            }
        }

        //private void SetOwnerGroupServiceRefIncidentArea(string IncidentGroup, string IncidentType)
        //{
        //    try
        //    {
        //        string OwnerGroupCode = libconfig.geteOwnerGroupCodeRefConfigIncidentArea(
        //           SID,
        //           mBusinessObject,
        //           IncidentGroup,
        //           IncidentType
        //           );
        //        ddlOwnerGroupService.SelectedValue = OwnerGroupCode;
        //    }
        //    catch (Exception ex)
        //    {
        //        ddlOwnerGroupService.SelectedIndex = 0;
        //    }
        //    udpnOwnerService.Update();
        //}


        #endregion

        protected void btnContactPersonChange_Click(object sender, EventArgs e)
        {
            try
            {
                GetcontactDetailForScreen();
                panelUpdate.Update();
                udpContactRefresh.Update();
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
                udpContactRefresh.Update();
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

        protected void btnCancelDocTran_click(object sender, EventArgs e)
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

                getdataToedit(DocType, hddDocnumberTran.Value.Trim(), (string)Session["SCT_created_fiscalyear" + idGen]);
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

        private void bindEquipmentMappingContact(string StructureCode, string CustomerCode, string EquipmentCode)
        {
            DataTable dt = universalService.getEquipmentMappingContact(
                    SID,
                    CompanyCode,
                    WorkGroupCode,
                    StructureCode,
                    CustomerCode,
                    EquipmentCode
                );

            rptEquipmentMappingContact.DataSource = dt;
            rptEquipmentMappingContact.DataBind();
            udpEquipmentMappingContact.Update();
        }

        private void BindEquipmentDetail(string equipmentCode)
        {
            string sql = @"SELECT a.EquipmentCode
	                        ,a.EquipmentType
	                        ,b.Description AS EquipmentTypeDesc
	                        ,a.CategoryCode
	                        ,CASE a.CategoryCode 
		                        WHEN '00' THEN 'Main Equipment'
		                        WHEN '01' THEN 'Sub Equipment'
		                        ELSE 'Virtual Equipment'
		                        END AS CategoryName
	                        ,c.EquipmentClass
	                        ,d.ClassName
                        FROM master_equipment a
                        INNER JOIN master_config_material_doctype b ON a.SID = b.SID
	                        --AND a.CompanyCode = b.Companycode
	                        AND a.EquipmentType = b.MaterialGroupCode
                        LEFT JOIN master_equipment_general c ON a.SID = c.SID
	                        AND a.CompanyCode = c.CompanyCode
	                        AND a.EquipmentCode = c.EquipmentCode
                        LEFT JOIN master_equipment_class d ON c.SID = d.SID
	                        AND c.EquipmentClass = d.ClassCode
                        WHERE a.SID = '" + SID + @"'
	                        AND a.CompanyCode = '" + CompanyCode + @"'
	                        AND a.EquipmentCode = '" + equipmentCode + @"'";

            DataTable dt = DBService.selectDataFocusone(sql);

            string equipmentType = "";
            string equipmentClass = "";
            string equipmentCategory = "";

            if (dt.Rows.Count > 0)
            {
                equipmentType = dt.Rows[0]["EquipmentType"] + " : " + dt.Rows[0]["EquipmentTypeDesc"];
                equipmentClass = dt.Rows[0]["EquipmentClass"] + " : " + dt.Rows[0]["ClassName"];
                equipmentCategory = dt.Rows[0]["CategoryCode"] + " : " + dt.Rows[0]["CategoryName"];
            }

            tbEquipmentType.Text = equipmentType;
            tbEquipmentClass.Text = equipmentClass;
            tbEquipmentCategory.Text = equipmentCategory;

            udpnEQDetail.Update();
        }

        private void ItemRowChange(string lineno)
        {
            if (serviceCallEntity.cs_servicecall_item.Select("", "", DataViewRowState.CurrentRows).Length > 0)
            {
                DataRow[] dr = serviceCallEntity.cs_servicecall_item.Select("xLineNo='" + lineno + "'");

                EquipmentSelect = dr[0]["EquipmentNo"].ToString();
                EquipmentItemNo = dr[0]["xLineNo"].ToString();
                BindEquipmentDetail(EquipmentSelect);

                //if (!(dtTier != null && dtTier.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dtTier.Rows[0]["AOBJECTLINK"]))))
                //{
                BindDataTierOperation();
                //}

                //bindEquipmentMappingContact("", dr[0]["CustomerCode"].ToString(), EquipmentSelect);

                if (dr.Length > 0)
                {
                    sc_bobjectid = dr[0]["BObjectID"].ToString();
                    Session["responsecall_lineno" + idGen] = dr[0]["xLineNo"].ToString();
                    serviceCallEntity.cs_servicecall_subject.DefaultView.RowFilter = "ObjectID='" + sc_bobjectid + "'";
                }

                updPerson.Update();
            }
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

        private void BindDataTierOperation()
        {
            try
            {
                countround = 1;
                string tierCode = "";

                if (mode_stage == ApplicationSession.CREATE_MODE_STRING && string.IsNullOrEmpty(EquipmentSelect))
                {
                    string Default_SLAGroup = ServiceLibrary.LookUpTable(
                        "Default_SLAGroup",
                        "ERPW_BUSINESSOBJECT_MAPPING_TICKET_TYPE",
                        "WHERE SID = '" + SID + "' AND TicketType = '" + hddTicketDocType.Value + "'"
                    );

                    DataTable dtTier_Default = TierService.getInStance().searchTierMaster(
                        SID,
                        WorkGroupCode,
                        "", "",
                        Default_SLAGroup
                    );

                    dtTier_Default.DefaultView.RowFilter = "PriorityCode = '" + _ddl_priorityTran.SelectedValue + "'";
                    dtTier_Default = dtTier_Default.DefaultView.ToTable();
                    dtTier_Default.DefaultView.RowFilter = string.Empty;

                    if (dtTier_Default.Rows.Count > 0)
                    {
                        tierCode = dtTier_Default.Rows[0]["tierCode"].ToString();
                    }                   
                }
                else
                {
                    if (serviceCallEntity.cs_servicecall_item.Rows.Count == 0)
                    {
                        return;
                    }

                    tierCode = getTierCode();
                }

                lbTierName.Text = AfterSaleService.getInstance().getTierName(SID, WorkGroupCode, tierCode);
                dtTier = AfterSaleService.getInstance().getTierOperation(SID, tierCode, hddDocnumberTran.Value);
                countActivity = AfterSaleService.getInstance().countActivityServiceCall(CompanyCode, hddDocnumberTran.Value, EquipmentSelect, EquipmentItemNo);

                rptOperation.DataSource = dtTier;
                rptOperation.DataBind();
                getEmailDefaultCustomEmail();

                updPerson.Update();
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

        private string getTierCode()
        {
            string priorityCode = _ddl_priorityTran.Items.Count > 0 ? _ddl_priorityTran.SelectedValue : "";

            string TierCode = AfterSaleService.getInstance().GetTierCode(
                SID,
                CompanyCode,
                CustomerCode,
                EquipmentSelect,
                priorityCode
            );

            if (string.IsNullOrEmpty(TierCode))
            {
                List<TierZeroEn> listTierZero = libTierZero.getTierZeroList(
                    SID,
                    CompanyCode,
                    TierZeroLibrary.TIER_ZERO_STATUS_CREATED
                ).Where(w => w.TicketNumber == hddDocnumberTran.Value).ToList();

                if (listTierZero.Count > 0)
                {
                    DataTable dtSLAConfig = MasterConfigLibrary.GetInstance().GetMasterConfigImpactGetSLA(
                        SID,
                        listTierZero.First().Channel
                    );

                    DataTable dtTier = TierService.getInStance().searchTierMaster(
                        SID,
                        WorkGroupCode,
                        "", "",
                        dtSLAConfig.Rows[0]["SLA_GroupCode"].ToString()
                    );

                    dtTier.DefaultView.RowFilter = "PriorityCode = '" + _ddl_priorityTran.SelectedValue + "'";
                    dtTier = dtTier.DefaultView.ToTable();
                    dtTier.DefaultView.RowFilter = string.Empty;

                    if (dtTier.Rows.Count > 0)
                    {
                        TierCode = dtTier.Rows[0]["tierCode"].ToString();
                    }                    
                }
                else
                {
                    string Default_SLAGroup = ServiceLibrary.LookUpTable(
                        "Default_SLAGroup",
                        "ERPW_BUSINESSOBJECT_MAPPING_TICKET_TYPE",
                        "WHERE SID = '" + SID + "' AND TicketType = '" + hddTicketDocType.Value + "'"
                    );

                    DataTable dtTier_Default = TierService.getInStance().searchTierMaster(
                        SID,
                        WorkGroupCode,
                        "", "",
                        Default_SLAGroup
                    );

                    dtTier_Default.DefaultView.RowFilter = "PriorityCode = '" + _ddl_priorityTran.SelectedValue + "'";
                    dtTier_Default = dtTier_Default.DefaultView.ToTable();
                    dtTier_Default.DefaultView.RowFilter = string.Empty;

                    if (dtTier_Default.Rows.Count > 0)
                    {
                        TierCode = dtTier_Default.Rows[0]["tierCode"].ToString();
                    }                    
                }
            }

            return TierCode;
        }

        public string GetNextTierCode(int index)
        {
            try
            {
                return dtTier.Rows[index + 1]["TierCode"].ToString();
            }
            catch
            {
                return "";
            }
        }
        public string GetNextTier(int index)
        {
            try
            {
                return dtTier.Rows[index + 1]["Tier"].ToString();
            }
            catch
            {
                return "";
            }
        }

        public string GetNextRole(int index)
        {
            try
            {
                return dtTier.Rows[index + 1]["Role"].ToString();
            }
            catch
            {
                return "";
            }
        }

        public int countround = 1;
        protected void rptOperation_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rptMainDelegate = e.Item.FindControl("rptMainDelegate") as Repeater;
            Repeater rptOtherDelegate = e.Item.FindControl("rptOtherDelegate") as Repeater;

            HiddenField hddTierCode = e.Item.FindControl("hddTierCode") as HiddenField;
            HiddenField hddTier = e.Item.FindControl("hddTier") as HiddenField;
            HiddenField hddHeaderStatus = e.Item.FindControl("hddHeaderStatus") as HiddenField;
            HiddenField hddHidePostRemark = e.Item.FindControl("hddHidePostRemark") as HiddenField;
            HiddenField hddOwnerService_Select = e.Item.FindControl("hddOwnerService_Select") as HiddenField;

            Button btnTransfer = e.Item.FindControl("btnTransfer") as Button;
            Button btnForwardWork = e.Item.FindControl("btnForwardWork") as Button;
            Button btnAssignWork = e.Item.FindControl("btnAssignWork") as Button;
            Button btnCloseWork = e.Item.FindControl("btnCloseWork") as Button;

            Panel panelFeedActivityComment = e.Item.FindControl("panelFeedActivityComment") as Panel;

            Panel PanelShowMain = e.Item.FindControl("PanelShowMain") as Panel;
            Panel PanelHideMain = e.Item.FindControl("PanelHideMain") as Panel;

            Panel PanelShowOther = e.Item.FindControl("PanelShowOther") as Panel;
            Panel panelShowMore = e.Item.FindControl("panelShowMore") as Panel;
            Panel PanelHideOther = e.Item.FindControl("PanelHideOther") as Panel;

            HiddenField hhdAOBJECTLINK = e.Item.FindControl("hhdAOBJECTLINK") as HiddenField;

            string tierCode = hddTierCode.Value;
            string CallStatus = "";
            string ticketType = "";
            if (serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
            {
                ticketType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
                CallStatus = serviceCallEntity.cs_servicecall_header.Rows[0]["CallStatus"].ToString();
            }

            string OwnerGroupCode = ddlOwnerGroupService.SelectedValue;
            if (!string.IsNullOrEmpty(hddOwnerService_Select.Value))
            {
                OwnerGroupCode = hddOwnerService_Select.Value;
            }

            DataTable dtMain = AfterSaleService.getInstance().GetTierMainDelegate(
                SID, CompanyCode,
                tierCode, hddTier.Value, ticketType,
                //hddIncidentAreaCode.Value,
                hhdAOBJECTLINK.Value, OwnerGroupCode
            );

            DataTable dtParticipant = lib.getListEmpParticipantsRefTicket(
                SID, CompanyCode,
                hhdAOBJECTLINK.Value, hddDocnumberTran.Value,
                tierCode, hddTier.Value,
                _txt_fiscalyear.Value, ticketType,
                hddOwnerService_Select.Value
            );
            if (dtParticipant.Rows.Count == 0)
            {
                dtParticipant = AfterSaleService.getInstance().GetTierParticipants(
                    SID, CompanyCode,
                    tierCode, hddTier.Value, ticketType,
                    //hddIncidentAreaCode.Value,
                    hhdAOBJECTLINK.Value, ddlOwnerGroupService.SelectedValue
                );
            }
            else
            {
                if (dtMain.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtParticipant.Rows)
                    {
                        dr["HierarchyDesc"] = dtMain.Rows[0]["HierarchyDesc"];
                    }
                }
            }


            

            rptMainDelegate.DataSource = dtMain;
            rptMainDelegate.DataBind();

            rptOtherDelegate.DataSource = dtParticipant;
            rptOtherDelegate.DataBind();

            if (dtMain.Rows.Count > 0)
            {
                PanelShowMain.Style["display"] = "";
                PanelHideMain.Style["display"] = "none";
            }
            else
            {
                PanelHideMain.Style["display"] = "";
                PanelShowMain.Style["display"] = "none";
            }

            if (dtParticipant.Rows.Count > 3)
            {
                panelShowMore.Visible = true;
            }
            else
            {
                panelShowMore.Visible = false;
            }

            if (dtParticipant.Rows.Count > 0)
            {
                PanelShowOther.Style["display"] = "";
                PanelHideOther.Style["display"] = "none";
            }
            else
            {
                PanelHideOther.Style["display"] = "";
                PanelShowOther.Style["display"] = "none";
            }

            btnAssignWork.Attributes.Add("disabled", "disabled");
            btnForwardWork.Attributes.Add("disabled", "disabled");
            btnCloseWork.Attributes.Add("disabled", "disabled");
            btnTransfer.Attributes.Add("disabled", "disabled");

            btnForwardWork.CommandArgument = "FALSE";
            //btnCloseWork.CommandArgument = "FALSE";
            btnTransfer.CommandArgument = "FALSE";

            if (countround == 1)
            {
                if (countActivity == 0)
                {
                    btnForwardWork.Style["display"] = "none";
                    btnTransfer.Style["display"] = "none";
                    btnCloseWork.Style["display"] = "none";

                    if (!string.IsNullOrEmpty(hddDocnumberTran.Value))
                    {
                        btnAssignWork.Attributes.Remove("disabled");
                    }
                }
                else
                {
                    btnAssignWork.Style["display"] = "none";

                    if (countActivity == countround)
                    {
                        if (CallStatus != ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE && CallStatus != ServiceTicketLibrary.SERVICE_CALL_STATUS_CANCEL)
                        {
                            btnForwardWork.Attributes.Remove("disabled");
                            btnTransfer.Attributes.Remove("disabled");
                            btnCloseWork.Attributes.Remove("disabled");

                            bool isTransfer = false;

                            if (!IsAuthen(dtMain, dtParticipant, CharacterService.Authen_TransferTask))
                            {
                                btnForwardWork.Attributes.Add("disabled", "disabled");
                                btnForwardWork.CommandArgument = "FALSE";

                                btnTransfer.Attributes.Add("disabled", "disabled");
                                btnTransfer.CommandArgument = "FALSE";
                            }
                            else
                            {
                                btnForwardWork.CommandArgument = "TRUE";
                                isTransfer = true;
                            }

                            if (!IsAuthen(dtMain, dtParticipant, CharacterService.Authen_CloseTask))
                            {
                                //btnCloseWork.Attributes.Add("disabled", "disabled");
                                //btnCloseWork.CommandArgument = "FALSE";
                            }
                            else
                            {
                                //btnCloseWork.CommandArgument = "TRUE";
                                isTransfer = true;
                            }

                            if (!isTransfer)
                            {
                                btnTransfer.Attributes.Add("disabled", "disabled");
                                btnTransfer.CommandArgument = "FALSE";
                            }
                            else
                            {
                                btnTransfer.CommandArgument = "TRUE";
                            }
                        }
                    }
                }
            }
            else
            {
                btnAssignWork.Style["display"] = "none";

                if (countActivity == countround)
                {
                    if (CallStatus != ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE && CallStatus != ServiceTicketLibrary.SERVICE_CALL_STATUS_CANCEL)
                    {
                        btnForwardWork.Attributes.Remove("disabled");
                        btnCloseWork.Attributes.Remove("disabled");
                        btnTransfer.Attributes.Remove("disabled");
                        bool isTransfer = false;

                        if (!IsAuthen(dtMain, dtParticipant, CharacterService.Authen_TransferTask))
                        {
                            btnForwardWork.Attributes.Add("disabled", "disabled");
                            btnForwardWork.CommandArgument = "FALSE";
                        }
                        else
                        {
                            isTransfer = true;
                            btnForwardWork.CommandArgument = "TRUE";
                        }

                        if (!IsAuthen(dtMain, dtParticipant, CharacterService.Authen_CloseTask))
                        {
                            //btnCloseWork.Attributes.Add("disabled", "disabled");
                            //btnCloseWork.CommandArgument = "FALSE";
                        }
                        else
                        {
                            isTransfer = true;
                            //btnCloseWork.CommandArgument = "TRUE";
                        }

                        if (!isTransfer)
                        {
                            btnTransfer.Attributes.Add("disabled", "disabled");
                            btnTransfer.CommandArgument = "FALSE";
                        }
                        else
                        {
                            btnTransfer.CommandArgument = "TRUE";
                        }
                    }
                }
            }


            if (countround == dtTier.Rows.Count)
            {
                btnForwardWork.Style["display"] = "none";

                if (CallStatus == ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE)
                {
                    btnCloseWork.Attributes.Add("disabled", "disabled");
                    btnTransfer.Style["display"] = "none";
                }
            }

            if (hddHeaderStatus.Value.ToLower() == "close")
            {
                btnAssignWork.Attributes.Add("disabled", "disabled");
                btnForwardWork.Attributes.Add("disabled", "disabled");
                btnCloseWork.Attributes.Add("disabled", "disabled");
                btnTransfer.Attributes.Add("disabled", "disabled");

                btnForwardWork.CommandArgument = "FALSE";
                //btnCloseWork.CommandArgument = "FALSE";
                btnTransfer.CommandArgument = "FALSE";
            }

            if (lastAobject == hhdAOBJECTLINK.Value &&
                (CallStatus == ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE ||
                CallStatus == ServiceTicketLibrary.SERVICE_CALL_STATUS_CANCEL))
            {
                hddHidePostRemark.Value = true.ToString();
            }
            else
            {
                hddHidePostRemark.Value = false.ToString();
            }
            if (lastAobject == hhdAOBJECTLINK.Value)
            {
                panelFeedActivityComment.Visible = true;

                //===================================== SET rating ==================================================
                string ticketNo = serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString();
                string emCode = dtMain.Rows[0]["EmployeeCode"].ToString();

                //set ค่าให้ hidden field
                hddDataRating_TicketCode.Value = ticketNo;
                hddDataRating_EmpCode.Value = emCode;
                
                //========================================================================================================
            }
            else
            {
                panelFeedActivityComment.Visible = false;
            }

            countround++;
        }

        protected void saveTimetampResponseToCustomer(string ResponseDate, string ResponseTime, string ResponseBy)
        {
            try
            {
                foreach (DataRow dr in serviceCallEntity.cs_servicecall_item.Rows)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ResponseOnDate"])))
                    {
                        return;
                    }

                    dr["ResponseOnDate"] = ResponseDate;
                    dr["ResponseOnTime"] = ResponseTime;
                    dr["ResponseBy"] = ResponseBy;
                }

                foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
                {
                    dr["Docstatus"] = hddTicketStatus_New.Value;
                }

                saveAssignCancelCloseResponseResolution("1500545", "Response To Customer", ResponseBy);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void saveCancelCall(string canceldate, string canceltime, string cancelby, string cancelcomment)
        {
            try
            {
                DataTable dtHeader = serviceCallEntity.cs_servicecall_header;
                DataTable dtItem = serviceCallEntity.cs_servicecall_item;
                DataRow[] drHeader = dtHeader.Select("CallerID='" + hddDocnumberTran.Value + "'");
                if (drHeader.Length != 0)
                {
                    string SERVICE_DOC_STATUS_CANCEL = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CANCEL);

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

                saveAssignCancelCloseResponseResolution("1500545", "Cancel Ticket", cancelby);
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
                    string SERVICE_DOC_STATUS_INPROGRESS = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_INPROGRESS);

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
            try
            {
                string ticketType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
                string ticketNo = serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString();
                string ticketYear = serviceCallEntity.cs_servicecall_header.Rows[0]["FiscalYear"].ToString();

                AfterSaleService.getInstance().UpdateStatus(SID, CompanyCode, DocStatusCode, ticketType, ticketYear, ticketNo,
                    UserName, Validation.getCurrentServerStringDateTime());

                _txt_docstatusTran.Value = GetDocStatusDesc(DocStatusCode);
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

        protected void saveAssignCancelCloseResponseResolution(string ReflectionCode, string _Message, string empcode)
        {
            try
            {
                if (!string.IsNullOrEmpty(ReflectionCode))
                {
                    DataSet objReturn = new DataSet();
                    string returnMessage = "";

                    string sessionid = (string)Session[ApplicationSession.USER_SESSION_ID];
                    Object[] objParam = new Object[] { ReflectionCode, sessionid };
                    DataSet[] objDataSet = new DataSet[] { serviceCallEntity };
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

        protected void btnAssignWork_Click(object sender, EventArgs e)
        {
            try
            {
                //if (string.IsNullOrEmpty(ddlEquipmentNo.SelectedValue))
                //{
                //    throw new Exception("กรุณาระบุ Equipment");
                //}
                //if (string.IsNullOrEmpty(ddlOwnerGroupService.SelectedValue))
                //{
                //    throw new Exception("กรุณาระบุ Owner Service");
                //}
                //if (string.IsNullOrEmpty(hddIncidentAreaCode.Value))
                //{
                //    throw new Exception("กรุณาระบุ Incident Area");
                //}

                string ticketType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
                string ticketNo = serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString();
                string ticketYear = serviceCallEntity.cs_servicecall_header.Rows[0]["FiscalYear"].ToString();
                string remark = txt_problem_topic.Text;

                string ticketCode = AssignWork_SLA(ticketType, ticketNo, ticketYear, remark);
                BindDataTierOperation();

                ClientService.AGSuccess("Start ticket success.");

                ClientService.DoJavascript("getremarkservice('" + ticketCode + "');");
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

        private string AssignWork_SLA(string ticketType, string ticketNo, string ticketYear, string remark)
        {
            string ticketCode = "";
            string SLAGroupCode = "";
            string OwnerGroupCode = "";
            Boolean AutoTriggerEscalate = true;

            if (dtTier.Rows.Count > 0)
            {
                string tierCode = dtTier.Rows[0]["TierCode"].ToString();
                string tier = dtTier.Rows[0]["Tier"].ToString();
                string tierDesc = dtTier.Rows[0]["TierDescription"].ToString();
                SLAGroupCode = dtTier.Rows[0]["SLAGroupCode_Select"].ToString();

                if (string.IsNullOrEmpty(ddlOwnerGroupService.SelectedValue))
                {
                    AutoTriggerEscalate = false;
                }
                if (string.IsNullOrEmpty(ddlEquipmentNo.SelectedValue))
                {
                    SLAGroupCode = dtTier.Rows[0]["TierGroupCode"].ToString();
                }

              

                double resolutionTime = 0;

                double.TryParse(dtTier.Rows[0]["Resolution"].ToString(), out resolutionTime);

                double requesterTime = 0;

                double.TryParse(dtTier.Rows[0]["Requester"].ToString(), out requesterTime);


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

                ticketCode = AfterSaleService.getInstance().StartTicket(
                    SID, CompanyCode, ticketType, ticketNo, ticketYear,
                    tierCode, tier, tierDesc, resolutionTime,
                    requesterTime,
                    ddlOwnerGroupService.SelectedValue,
                    //hddIncidentAreaCode.Value, 
                    ddlEquipmentNo.SelectedValue, remark,
                    UserName, EmployeeCode, FullNameEN,
                    AutoTriggerEscalate, SLAGroupCode,
                    dtFile, UploadFileUrl, UploadFilePath
                );

                SnapDataLibrary.getInstance().saveDataSnap(
                    SID,
                    CompanyCode,
                    ticketCode,
                    SLAGroupCode,
                    OwnerGroupCode,
                    SnapDataLibrary.SNAP_EVENT_OBJECT_OPEN,
                    "",
                    EmployeeCode
                );
            }

            return ticketCode;
        }

        #region Escalate
        protected void btnForwardWork_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (sender as Button);

                if (!string.IsNullOrEmpty(btn.CommandArgument) && !Convert.ToBoolean(btn.CommandArgument))
                {
                    throw new Exception("คุณไม่มีสิทธิ์ส่งต่องาน");
                }

                RepeaterItem rpt = btn.NamingContainer as RepeaterItem;
                HiddenField hhdAOBJECTLINK = rpt.FindControl("hhdAOBJECTLINK") as HiddenField;
                HiddenField hddSLAGroup = rpt.FindControl("hddSLAGroup") as HiddenField;
                HiddenField hddTierCode = rpt.FindControl("hddTierCode") as HiddenField;
                HiddenField hddTierCodeNext = rpt.FindControl("hddTierCodeNext") as HiddenField;
                HiddenField hddTier = rpt.FindControl("hddTier") as HiddenField;
                HiddenField hddTierNext = rpt.FindControl("hddTierNext") as HiddenField;

                if (!AfterSaleService.CheckCurrentTier(CompanyCode, hddDocnumberTran.Value, hhdAOBJECTLINK.Value))
                {
                    btnSelectIncidentArea_Click(null, null);
                    ClientService.AGMessage("There are changes in the tier, Please do the transaction again.");
                    return;
                }

                string SLAGroupCode = "";
                if (string.IsNullOrEmpty(ddlEquipmentNo.SelectedValue))
                {
                    SLAGroupCode = hddSLAGroup.Value;
                }
                else
                {
                    string priorityCode = _ddl_priorityTran.Items.Count > 0 ? _ddl_priorityTran.SelectedValue : "";
                    SLAGroupCode = AfterSaleService.getInstance().GetSLAGroupCode(
                        SID,
                        CompanyCode,
                        CustomerCode,
                        EquipmentSelect,
                        priorityCode
                    );
                }

                ddlEscalate_OwnerService.SelectedValue = ddlOwnerGroupService.SelectedValue;
                udpEscalate_OwnerService.Update();
                ddlEscalate_SLAGroup.SelectedValue = SLAGroupCode;
                udpEscalate_SLAGroup.Update();

                hddEscalate_AOBJECTLINK.Value = hhdAOBJECTLINK.Value;
                hddEscalate_TierCode.Value = hddTierCodeNext.Value;
                hddEscalate_SLAGroup.Value = SLAGroupCode;
                hddEscalate_SLAGroupName.Value = ddlEscalate_SLAGroup.SelectedItem.Text;
                hddEscalate_TierNext.Value = hddTierNext.Value;
                hddEscalate_Tier.Value = hddTier.Value;
                hddEscalate_ListEMPCode.Value = "";
                hddTransfer_ListEMPCode.Value = "";
                udpEscalateSetParticipant.Update();
                AutoCompleteEmployee_Escalate.SelectedValueRefresh = "";


                string ticketType = "";
                if (serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
                {
                    ticketType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
                }

                DataTable dtMain = AfterSaleService.getInstance().GetTierMainDelegate(
                    SID, CompanyCode,
                    hddEscalate_TierCode.Value, hddEscalate_TierNext.Value, ticketType,
                    "", ddlOwnerGroupService.SelectedValue
                );

                DataTable dtParticipant = AfterSaleService.getInstance().GetTierParticipants(
                    SID, CompanyCode,
                    hddEscalate_TierCode.Value, hddEscalate_TierNext.Value, ticketType,
                    "", ddlOwnerGroupService.SelectedValue
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
                    drNew["empName"] = drMain["fullname"].ToString();
                    drNew["empImg"] = "/images/user.png";
                    drNew["IsMain"] = "true";
                }
                foreach (DataRow drParticipant in dtParticipant.Rows)
                {
                    DataRow drNew = dt.Rows.Add();
                    drNew["empCode"] = drParticipant["EmployeeCode"].ToString();
                    drNew["empName"] = drParticipant["fullname"].ToString();
                    drNew["empImg"] = "/images/user.png";
                    drNew["IsMain"] = "false";
                }

                rptEscalateSetParticipant.DataSource = dt;
                rptEscalateSetParticipant.DataBind();
                udpEscalateSetParticipant.Update();

                ClientService.DoJavascript("showInitiativeModal('modal-EscalateSetParticipant');");
                //string current_time = Validation.getCurrentServerDateTime().ToString("HHmmss");
                //string current_date = Validation.getCurrentServerStringDateTime().Substring(0, 8);

                //HiddenField hddTierNext = (sender as Button).Parent.FindControl("hddTierNext") as HiddenField;
                //HiddenField hddRoleNext = (sender as Button).Parent.FindControl("hddRoleNext") as HiddenField;
                //HiddenField hddTierCode = (sender as Button).Parent.FindControl("hddTierCode") as HiddenField;
                //HiddenField hddTier = (sender as Button).Parent.FindControl("hddTier") as HiddenField;
                //HiddenField hhdAOBJECTLINK = (sender as Button).Parent.FindControl("hhdAOBJECTLINK") as HiddenField;

                //DataRow[] drsTier = dtTier.Select("Tier='" + hddTierNext.Value + "'");

                //string tierDesc = "";
                //double resolutionTime = 0;

                //if (drsTier.Length > 0)
                //{
                //    tierDesc = drsTier[0]["TierDescription"].ToString();
                //    double.TryParse(drsTier[0]["Resolution"].ToString(), out resolutionTime);
                //}

                //string ticketType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
                //string ticketNo = serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString();
                //string ticketYear = serviceCallEntity.cs_servicecall_header.Rows[0]["FiscalYear"].ToString();

                //string aobjectLink = AfterSaleService.getInstance().EscalateTicket(SID, CompanyCode,
                //    ticketType, ticketNo, ticketYear, hddTierCode.Value, hddTierNext.Value, tierDesc, resolutionTime,
                //    hddIncidentAreaCode.Value, EquipmentSelect, txt_problem_topic.Text, UserName, EmployeeCode, FullNameEN, true);

                //updRemarkService.Update();

                //bindDataChangeLog();
                //BindDataLogFileAttachment();
                //BindDataTierOperation();

                //ClientService.AGSuccess("Escalate ticket success.");
                //ClientService.DoJavascript("getremarkservice('" + aobjectLink + "');");
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

        protected void btnEscalateSetParticipant_Click(object sender, EventArgs e)
        {
            try
            {
                if (!AfterSaleService.CheckCurrentTier(CompanyCode, hddDocnumberTran.Value, hddEscalate_AOBJECTLINK.Value))
                {
                    btnSelectIncidentArea_Click(null, null);
                    ClientService.AGMessage("There are changes in the tier, Please do the transaction again.");
                    return;
                }

                List<listOperationTransfer> listEmp = JsonConvert.DeserializeObject<List<listOperationTransfer>>(hddTransfer_ListEMPCode.Value);

                string current_time = Validation.getCurrentServerDateTime().ToString("HHmmss");
                string current_date = Validation.getCurrentServerStringDateTime().Substring(0, 8);

                HiddenField hddTierNext = hddEscalate_TierNext;
                //HiddenField hddRoleNext = hddEscalate_;
                HiddenField hddTierCode = hddEscalate_TierCode;
                HiddenField hddTier = hddEscalate_Tier;
                HiddenField hhdAOBJECTLINK = hddEscalate_AOBJECTLINK;

                DataRow[] drsTier = dtTier.Select("Tier='" + hddTierNext.Value + "'");

                string tierDesc = "";
                double resolutionTime = 0;
                double requesterTime = 0;

                if (drsTier.Length > 0)
                {
                    tierDesc = drsTier[0]["TierDescription"].ToString();
                    double.TryParse(drsTier[0]["Resolution"].ToString(), out resolutionTime);
                }

                string ticketType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
                string ticketNo = serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString();
                string ticketYear = serviceCallEntity.cs_servicecall_header.Rows[0]["FiscalYear"].ToString();

                string MainDelegate = listEmp
                    .Where(w => w.IsMain)
                    .First()
                    .EmpCode;

                string[] participantsArray = listEmp
                    .Where(w => !w.IsMain && !w.Event.Equals("REMOVE"))
                    .Select(s => s.EmpCode)
                    .ToArray();

                string OwnerServiceCode = null;
                string SLAGroup = null;
                if (hddEscalate_SLAGroup.Value != ddlEscalate_SLAGroup.SelectedValue)
                {
                    SLAGroup = ddlEscalate_SLAGroup.SelectedValue;
                }

                if (ddlEscalate_OwnerService.SelectedValue != ddlOwnerGroupService.SelectedValue)
                {
                    OwnerServiceCode = ddlEscalate_OwnerService.SelectedValue;

                    DataTable dtTierSelect = TierService.getInStance().searchTierMaster(
                        SID,
                        WorkGroupCode,
                        "", "",
                        ddlEscalate_SLAGroup.SelectedValue
                    );
                    dtTierSelect.DefaultView.RowFilter = "PriorityCode = '" + _ddl_priorityTran.SelectedValue + "'";
                    dtTierSelect = dtTierSelect.DefaultView.ToTable();
                    dtTierSelect.DefaultView.RowFilter = string.Empty;

                    DataTable dtTierSelecDetail = AfterSaleService.getInstance().getTierItem(
                        SID,
                        WorkGroupCode,
                        dtTierSelect.Rows[0]["TierCode"].ToString()
                    );
                    dtTierSelecDetail.DefaultView.RowFilter = "Tier = '" + hddTierNext.Value + "'";
                    dtTierSelecDetail = dtTierSelecDetail.DefaultView.ToTable();
                    dtTierSelecDetail.DefaultView.RowFilter = string.Empty;

                    double.TryParse(dtTierSelecDetail.Rows[0]["Resolution"].ToString(), out resolutionTime);
                }
                string aobjectLink = "";
                try
                {
                    aobjectLink = AfterSaleService.getInstance().EscalateTicket(
                        SID, CompanyCode,
                        ticketType, ticketNo, ticketYear, hddTierCode.Value, hddTierNext.Value, tierDesc, resolutionTime,
                        requesterTime,
                        ddlOwnerGroupService.SelectedValue, /*hddIncidentAreaCode.Value,*/ EquipmentSelect, txt_problem_topic.Text, UserName,
                        EmployeeCode, FullNameEN, true,
                        MainDelegate, participantsArray, OwnerServiceCode, SLAGroup
                    );
                }
                catch (Exception ex)
                {
                    btnSelectIncidentArea_Click(null, null);
                    ClientService.AGMessage("There are changes in the tier, Please do the transaction again.");
                    return;
                }

                AfterSaleService.getInstance().updateCurDateTimeActivityActionManual(
                    SID, 
                    CompanyCode, 
                    hhdAOBJECTLINK.Value
                );

                // TODO 
                TriggerService.GetInstance().CancelTrigger(hhdAOBJECTLINK.Value);
                TriggerService.GetInstance().ManuallyEscalateTicket(hhdAOBJECTLINK.Value);

                string logMessage = "";
                if (hddEscalate_SLAGroup.Value != ddlEscalate_SLAGroup.SelectedValue)
                {
                    logMessage += "Change SLA Group '" + hddEscalate_SLAGroupName.Value + "' to '" + ddlEscalate_SLAGroup.SelectedItem.Text + "'.";
                }

                if (ddlEscalate_OwnerService.SelectedValue != ddlOwnerGroupService.SelectedValue)
                {
                    //if (!string.IsNullOrEmpty(logMessage))
                    //{
                    //    logMessage += " /n ";
                    //}

                    logMessage += " Change Owner Service '" + ddlOwnerGroupService.SelectedItem.Text + "' to '" + ddlEscalate_OwnerService.SelectedItem.Text + "'.";
                }

                SnapDataLibrary.getInstance().saveDataSnap(
                    SID,
                    CompanyCode,
                    aobjectLink,
                    SnapDataLibrary.SNAP_EVENT_OBJECT_ESCALATE,
                    ddlEscalate_SLAGroup.SelectedValue,
                    ddlEscalate_OwnerService.SelectedValue,
                    logMessage,
                    EmployeeCode
                );

                if (!string.IsNullOrEmpty(logMessage))
                {
                    lib.SaveActivityDetail(SID, CompanyCode, CompanyCode,
                        EmployeeCode, EmployeeCode, FullNameEN,
                        aobjectLink, "", logMessage, "", Validation.getCurrentServerStringDateTimeMillisecond(),
                        "Escalate", "", "", "");
                }

                NotificationLibrary.GetInstance().TicketAlertEvent(
                    NotificationLibrary.EVENT_TYPE.TICKET_ESCALATE,
                    SID,
                    CompanyCode,
                    hddDocnumberTran.Value,
                    EmployeeCode,
                    ThisPage
                );

                updRemarkService.Update();

                if (chkIsLoad_TicketChangeLog.Checked)
                    bindDataChangeLog();

                if (chkIsLoad_Attachment.Checked)
                    BindDataLogFileAttachment();

                BindDataTierOperation();

                ClientService.AGSuccess("Escalate ticket success.");
                ClientService.DoJavascript("getremarkservice('" + aobjectLink + "');");
                ClientService.DoJavascript("closeInitiativeModal('modal-EscalateSetParticipant');");
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

        protected void ddlEscalate_OwnerService_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string ticketType = "";
                string TierCode = hddEscalate_TierCode.Value;

                if (serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
                {
                    ticketType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
                }

                if (hddEscalate_SLAGroup.Value != ddlEscalate_SLAGroup.SelectedValue)
                {

                    DataTable dtTierSelect = TierService.getInStance().searchTierMaster(
                        SID,
                        WorkGroupCode,
                        "", "",
                        ddlEscalate_SLAGroup.SelectedValue
                    );
                    dtTierSelect.DefaultView.RowFilter = "PriorityCode = '" + _ddl_priorityTran.SelectedValue + "'";
                    dtTierSelect = dtTierSelect.DefaultView.ToTable();
                    dtTierSelect.DefaultView.RowFilter = string.Empty;
                    TierCode = dtTierSelect.Rows[0]["tierCode"].ToString();
                }

                DataTable dtMain = AfterSaleService.getInstance().GetTierMainDelegate(
                    SID, CompanyCode,
                    TierCode, hddEscalate_TierNext.Value, ticketType,
                    /*hddIncidentAreaCode.Value,*/ "",
                    ddlEscalate_OwnerService.SelectedValue
                );

                DataTable dtParticipant = AfterSaleService.getInstance().GetTierParticipants(
                    SID, CompanyCode,
                    TierCode, hddEscalate_TierNext.Value, ticketType,
                    /*hddIncidentAreaCode.Value,*/ "",
                    ddlEscalate_OwnerService.SelectedValue
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
                    drNew["empName"] = drMain["fullname"].ToString();
                    drNew["empImg"] = "/images/user.png";
                    drNew["IsMain"] = "true";
                }
                foreach (DataRow drParticipant in dtParticipant.Rows)
                {
                    DataRow drNew = dt.Rows.Add();
                    drNew["empCode"] = drParticipant["EmployeeCode"].ToString();
                    drNew["empName"] = drParticipant["fullname"].ToString();
                    drNew["empImg"] = "/images/user.png";
                    drNew["IsMain"] = "false";
                }

                rptEscalateSetParticipant.DataSource = dt;
                rptEscalateSetParticipant.DataBind();
                udpEscalateSetParticipant.Update();
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

        protected void btnCloseWork_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (sender as Button);
                //string SERVICE_DOC_STATUS_INPROGRESS = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_INPROGRESS);
                //if (hddTicketStatus_Old.Value != SERVICE_DOC_STATUS_INPROGRESS)
                //{
                //    ClientService.DoJavascript("focusRemarkBox(true);");
                //    throw new Exception("Please update status to In progress before Resolve!");
                //}
                
                HiddenField hddTier = (sender as Button).Parent.FindControl("hddTier") as HiddenField;
                HiddenField hhdAOBJECTLINK = (sender as Button).Parent.FindControl("hhdAOBJECTLINK") as HiddenField;

                if (!AfterSaleService.CheckCurrentTier(CompanyCode, hddDocnumberTran.Value, hhdAOBJECTLINK.Value) ||
                    AfterSaleService.CheckResolveTier(SID, CompanyCode, hhdAOBJECTLINK.Value))
                {
                    btnSelectIncidentArea_Click(null, null);
                    ClientService.AGMessage("There are changes in the tier, Please do the transaction again.");
                    return;
                }

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

                NotificationLibrary.GetInstance().TicketAlertEvent(
                    NotificationLibrary.EVENT_TYPE.TICKET_RESOLVE,
                    SID,
                    CompanyCode,
                    hddDocnumberTran.Value,
                    EmployeeCode,
                    ThisPage
                );

                TriggerService.GetInstance().CancelTrigger(hhdAOBJECTLINK.Value);
                getdataToedit(ticketType, ticketNo, ticketYear);
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
                //string lastAobject = AfterSaleService.getInstance().getLastActivityServiceCall(
                //    CompanyCode, hddDocnumberTran.Value, EquipmentSelect, EquipmentItemNo);

                if (aobjectlink == lastAobject)
                {
                    style += "background-color: #FFFDE7;";
                }
            }
            return style;
        }
        public bool showButtonCustomMail(string aobjectlink)
        {
            if (aobjectlink == lastAobject)
            {
                ActivitySendMailModal.CustomerCode = CustomerCode;
                ActivitySendMailModal.refAobjectlink = aobjectlink;
                ActivitySendMailModal.refTicketNo = hddDocnumberTran.Value;

                return true;
            }
            else { return false; }
        }

        private void getEmailDefaultCustomEmail()
        {
            ERPW.Lib.Master.CustomerService libCus = ERPW.Lib.Master.CustomerService.getInstance();
            List<string> listMailTo = new List<string>();
            List<string> listMailCC = new List<string>();
            List<string> listEmpCode = new List<string>();


            //try
            //{
            //    listMailTo.AddRange(
            //        NotificationLibrary.GetInstance().getEmailDefaultConfig(SID, CompanyCode, "TO")
            //    );
            //}
            //catch (Exception)
            //{
                
            //}
            
            //listMailCC.AddRange(
            //    libCus.getContactEmail(
            //        SID, CompanyCode,
            //        hddCustomerCode.Value, _ddl_contact_person.SelectValue
            //    )
            //);

            //try
            //{
            //    listMailCC.AddRange(
            //        NotificationLibrary.GetInstance().getEmailDefaultConfig(SID, CompanyCode, "CC")
            //    );
            //}
            //catch (Exception)
            //{
            //}

            foreach (RepeaterItem itemOperation in rptOperation.Items)
            {
                HiddenField hhdAOBJECTLINK = itemOperation.FindControl("hhdAOBJECTLINK") as HiddenField;
                if (string.IsNullOrEmpty(hhdAOBJECTLINK.Value))
                {
                    continue;
                }

                Repeater rptMainDelegate = itemOperation.FindControl("rptMainDelegate") as Repeater;
                Repeater rptOtherDelegate = itemOperation.FindControl("rptOtherDelegate") as Repeater;

                foreach (RepeaterItem itemMail in rptMainDelegate.Items)
                {
                    HiddenField hddMainLinkID = itemMail.FindControl("hddMainLinkID") as HiddenField;
                    listEmpCode.Add(hddMainLinkID.Value);
                }
                foreach (RepeaterItem itemOther in rptOtherDelegate.Items)
                {
                    HiddenField hddParLinkID = itemOther.FindControl("hddParLinkID") as HiddenField;
                    listEmpCode.Add(hddParLinkID.Value);
                }
            }

            listMailCC = EmployeeService.GetInstance().getListEmailEmployee(
                SID, CompanyCode,
                listEmpCode
            );

            //ActivitySendMailModal.defaultMailTo(listMailTo);
            //ActivitySendMailModal.defaultMailCC(listMailCC);

            //////////////////////////////////////////////////////////////////////////////////

            ActivitySendMailModal.EmailOwner = string.Join(",",
                ServiceTicketLibrary.LookUpTable(
                    "EMail",
                    "ERPW_OWNER_GROUP",
                    "where SID = '" + SID + "' AND CompanyCode = '" + CompanyCode + "' AND OwnerGroupCode = '" + ddlOwnerGroupService.SelectedValue + "'"
                )    
            );

            ActivitySendMailModal.EmailMainAssignee = string.Join(",", listMailTo);
            ActivitySendMailModal.EmailOtherAssignee = string.Join(",", listMailCC);

            ActivitySendMailModal.EmailCustomer = string.Join(",",
                ServiceTicketLibrary.LookUpTable(
                    "EMail", 
                    "master_customer_general", 
                    "where SID = '" + SID + "' AND CompanyCode = '" + CompanyCode + "' AND CustomerCode = '" + CustomerCode + "'"
                )
            );
            ActivitySendMailModal.EmailContact = string.Join(",", txtContactEmail.Text);
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
                JOBDESCRIPTION = "Test Start Work Flow : " + DateIn,
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
                BindDataWorkflow();
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

        private void BindDataWorkflow()
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
                AobjLink);
            if (dtWF.Rows.Count > 0)
            {
                ddlAccountability.SelectedValue = Convert.ToString(dtWF.Rows[0]["SUBPROJECT"]);
                ApprovalProcedureControl.BindData(AobjLink, "");

                string[] StateGate = libWorkFlow.GetApprovalHeaderPresent(
                    SID,
                    CompanyCode,
                    "", //WorkGroupCode,
                    AobjLink
                ).Split(',');

                if (StateGate.Length > 0)
                {
                    ApproveStateGateControl.WorkGroupCode = "";
                    ApproveStateGateControl.InitiativeCode = AobjLink;
                    ApproveStateGateControl.StategateCode = StateGate[0];
                    ApproveStateGateControl.Trig();
                    ApproveStateGateControl.TicketCode = hddDocnumberTran.Value;
                }
                //if (StateGate.Length >= 3)
                //{
                //    DataTable dtStategate = libWorkFlow.getInitiativeApproveHeader(AobjLink);

                //    if (StateGate[2] == "TRUE")
                //    {

                //    }
                //    else
                //    {
                //        tbWorkFlowStatus.Text = libWorkFlow.getDescriptionEventObject(
                //            StateGate[1]
                //        );
                //    }
                //}

                //ApproveStateGateControl_L0.WorkGroupCode = "";
                //ApproveStateGateControl_L0.InitiativeCode = AobjLink;
                //ApproveStateGateControl_L0.StategateCode = "L0";
                //ApproveStateGateControl_L0.Trig();

                //ApproveStateGateControl_L1.WorkGroupCode = "";
                //ApproveStateGateControl_L1.InitiativeCode = AobjLink;
                //ApproveStateGateControl_L1.StategateCode = "L1";
                //ApproveStateGateControl_L1.Trig();

                //ApproveStateGateControl_L2.WorkGroupCode = "";
                //ApproveStateGateControl_L2.InitiativeCode = AobjLink;
                //ApproveStateGateControl_L2.StategateCode = "L2";
                //ApproveStateGateControl_L2.Trig();

                //ApproveStateGateControl_L3.WorkGroupCode = "";
                //ApproveStateGateControl_L3.InitiativeCode = AobjLink;
                //ApproveStateGateControl_L3.StategateCode = "L3";
                //ApproveStateGateControl_L3.Trig();

                //ApproveStateGateControl_L4.WorkGroupCode = "";
                //ApproveStateGateControl_L4.InitiativeCode = AobjLink;
                //ApproveStateGateControl_L4.StategateCode = "L4";
                //ApproveStateGateControl_L4.Trig();

                udpWorkflowDetail.Update();
            }

            ApprovalHeader wfStateGate = libWorkFlow.GetApprovalHeaderPresentObject(
                    SID,
                    CompanyCode,
                    "", //WorkGroupCode,
                    AobjLink
                );

            if (!string.IsNullOrEmpty(wfStateGate.StategateFrom))
            {
                DataTable dtStategate = libWorkFlow.getInitiativeApproveHeader(SID, AobjLink);
                if (wfStateGate.ApproveStarted)
                {
                    tbWorkFlowStatus.Text = "Wait for " + libWorkFlow.getDescriptionEventObject(
                        wfStateGate.StategateTo
                    ) + " approval (Step Up)";

                    preperListParticipantsApproval(AobjLink, wfStateGate.StategateFrom, wfStateGate.StategateTo);
                }
                else if (wfStateGate.RequestApprovaDowngrade)
                {
                    tbWorkFlowStatus.Text = "Wait for " + libWorkFlow.getDescriptionEventObject(
                        wfStateGate.StategateTo
                    ) + " approval (Step Down)";

                    preperListParticipantsApproval(AobjLink, wfStateGate.StategateFrom, wfStateGate.StategateTo);
                }
                else
                {
                    if (wfStateGate.StategateFrom == "L0" && !wfStateGate.ApproveStatus)
                    {
                        tbWorkFlowStatus.Text = "Workflow Not Start.";
                    }
                    else if (wfStateGate.WorkflowSuccess)
                    {
                        tbWorkFlowStatus.Text = "Workflow Success";
                    }
                    else
                    {
                        tbWorkFlowStatus.Text = libWorkFlow.getDescriptionEventObject(
                            wfStateGate.StategateFrom
                        ) + " approved";
                    }
                }
            }
            else
            {
                tbWorkFlowStatus.Text = "Workflow Success";
            }

            #endregion
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

        private void bindDataAccountability()
        {
            DataTable dt = accountabilityService.getAccountabilityStructureV2(SID, "");

            ddlAccountability.DataSource = dt;
            ddlAccountability.DataTextField = "DataText";
            ddlAccountability.DataValueField = "DataValue";
            ddlAccountability.DataBind();
            ddlAccountability.Items.Insert(0, new ListItem("Please select", ""));

            ddlAccountability.Enabled = mode_stage == ApplicationSession.CREATE_MODE_STRING;
            ddlAccountability.SelectedValue = "";

            panelAccountability.Visible = AlowWorkFlow;
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

        private void BindProblemDetail(string IncidentGroup, string IncidentType, string IncidentSource)
        {
            GetProblemType(IncidentGroup);
            GetProblemSource(IncidentGroup, IncidentType);
            GetServiceSource(IncidentGroup, IncidentType, IncidentSource);
        }

        private void SetDefaultRowEquipment()
        {
            if (serviceCallEntity.cs_servicecall_item.Rows.Count == 0)
            {
                string currDate = Validation.getCurrentServerStringDateTime().Substring(0, 8);
                string currTime = Validation.getCurrentServerStringDateTime().Substring(8, 6);

                DataRow drNew = serviceCallEntity.cs_servicecall_item.NewRow();
                drNew["SID"] = SID;
                drNew["CompanyCode"] = CompanyCode;
                drNew["CustomerCode"] = serviceCallEntity.cs_servicecall_header.Rows[0]["CustomerCode"].ToString();
                drNew["DocType"] = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
                drNew["Fiscalyear"] = serviceCallEntity.cs_servicecall_header.Rows[0]["Fiscalyear"].ToString();
                drNew["CallerID"] = serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString();
                drNew["ObjectID"] = serviceCallEntity.cs_servicecall_header.Rows[0]["ObjectID"].ToString();
                drNew["xLineNo"] = "001";
                drNew["BObjectID"] = drNew["ObjectID"] + "001";
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
                drNew["IncidentArea"] = ""; // hddIncidentAreaCode.Value;
                serviceCallEntity.cs_servicecall_item.Rows.Add(drNew);
            }
        }

        private void PrepareEquipment(bool prepareOwner = true)
        {
            SetDefaultRowEquipment();

            DataRow dr = serviceCallEntity.cs_servicecall_item.Rows[0];

            sc_bobjectid = dr["BObjectID"].ToString();

            string equipmentCode = ddlEquipmentNo.SelectedValue;
            string equipmentDesc = "";
            string materialCode = "";
            string materialDesc = "";
            string OwnerSeviceCode = "";

            if (equipmentCode != "")
            {
                udpnOwnerService.Update();

                DataTable EQInfo = AfterSaleService.getInstance().getSearchEQInfo("", equipmentCode);

                if (EQInfo.Rows.Count > 0)
                {
                    equipmentDesc = Convert.ToString(EQInfo.Rows[0]["EquipmentName"]);
                    materialCode = Convert.ToString(EQInfo.Rows[0]["MaterialNo"]);
                    materialDesc = Convert.ToString(EQInfo.Rows[0]["MaterialName"]);
                }

                OwnerSeviceCode = libCI.getEquipmentMappingOwnerService(SID, CompanyCode, equipmentCode);
                ddlOwnerGroupService.SelectedValue = OwnerSeviceCode;
                udpnOwnerService.Update();
            }
            else
            {
                //ddlOwnerGroupService.SelectedValue = "";

                if (!string.IsNullOrEmpty(dr["QueueOption"].ToString()) && prepareOwner)
                {
                    ddlOwnerGroupService.SelectedValue = dr["QueueOption"].ToString();
                }
                udpnOwnerService.Update();

                //try { ddlProblemGroup.SelectedIndex = 0; } catch (Exception) { }
                //try { ddlProblemType.SelectedIndex = 0; ddlProblemType.Items.Clear(); } catch (Exception) { }
                //try { ddlProblemSource.SelectedIndex = 0; ddlProblemSource.Items.Clear(); } catch (Exception) { }
                //try { ddlServiceSource.SelectedIndex = 0; ddlServiceSource.Items.Clear(); } catch (Exception) { }
                //udpnProblem.Update();
            }

            dr["ItemCode"] = materialCode;
            dr["MaterialDesc"] = materialDesc;
            dr["EquipmentNo"] = equipmentCode;
            dr["EquipmentDesc"] = equipmentDesc;
            dr["SerialNo"] = tbSerialNo.Text.Trim();
            dr["ProblemGroup"] = ddlProblemGroup.SelectedValue;
            dr["ProblemTypeCode"] = ddlProblemType.SelectedValue;
            dr["OriginCode"] = ddlProblemSource.SelectedValue;
            dr["CallTypeCode"] = ddlServiceSource.SelectedValue;
            dr["QueueOption"] = ddlOwnerGroupService.SelectedValue;
            dr["Remark"] = tbEquipmentRemark.Text;
            dr["SummaryProblem"] = tbSummaryProblem.Text;
            dr["SummaryCause"] = tbSummaryCause.Text;
            dr["SummaryResolution"] = tbSummaryResolution.Text;
            dr["IncidentArea"] = ""; //hddIncidentAreaCode.Value;

            if (serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
            {
                serviceCallEntity.cs_servicecall_header.Rows[0]["AffectSLA"] = ddlAffectSLA.SelectedValue;
            }
        }

        private void GetEquipmentRelation(string equipmentCode)
        {
            LinkFlowChartService.DiagramRelation dataEn = LinkFlowChartService.getDiagramRelation(
                 SID,
                 CompanyCode,
                 equipmentCode,
                 LinkFlowChartService.ItemGroup_EQUIPMENT
             );

            //if (mode_stage == ApplicationSession.CREATE_MODE_STRING)
            //{
            //    FlowChartDiagramRelationControl.URLNodeRedirect = "JavaScript:;";
            //}
            //else
            //{
            //    FlowChartDiagramRelationControl.URLNodeRedirect = "JavaScript:openModalCreateNewTicket('{#ID}');";
            //}
            //FlowChartDiagramRelationControl.nodeActive = equipmentCode;

            //if (dataEn.parentNode.Count + dataEn.chindNode.Count > 0)
            //{
            //    dataEn.parentNode = new List<LinkFlowChartService.FlowChartRelation>();

            //    FlowChartDiagramRelationControl.listParentDiagram = dataEn.parentNode;
            //    FlowChartDiagramRelationControl.listChildDiagram = dataEn.chindNode;
            //    FlowChartDiagramRelationControl.initFlowChartDiagram();
            //}

            DataTable dt = new DataTable();
            dt.Columns.Add("parent");
            dt.Columns.Add("parentCode");
            dt.Columns.Add("relation");
            dt.Columns.Add("child");
            dt.Columns.Add("childCode");

            if (equipmentCode != "")
            {
                dataEn.parentNode.ForEach(r =>
                {
                    if (r.Level == 1)
                    {
                        DataRow drNew = dt.NewRow();
                        drNew["parent"] = r.ItemCode + " : " + r.ItemDescription;
                        drNew["parentCode"] = r.ItemCode;
                        drNew["relation"] = r.RelationDesc;
                        drNew["child"] = ddlEquipmentNo.SelectedItem.Text;
                        drNew["childCode"] = "";
                        dt.Rows.Add(drNew);
                    }
                });

                dataEn.chindNode.ForEach(r =>
                {
                    if (r.Level == 1)
                    {
                        DataRow drNew = dt.NewRow();
                        drNew["parent"] = ddlEquipmentNo.SelectedItem.Text;
                        drNew["parentCode"] = "";
                        drNew["relation"] = r.RelationDesc;
                        drNew["child"] = r.ItemCode + " : " + r.ItemDescription;
                        drNew["childCode"] = r.ItemCode;
                        dt.Rows.Add(drNew);
                    }
                });
            }

            rptEquipmentRelation.DataSource = dt;
            rptEquipmentRelation.DataBind();

            udpnEquipmentRelation.Update();
        }

        private void EquipmentSelected()
        {
            PrepareEquipment();

            ItemRowChange("001");

            if (!AlowWorkFlow && mode_stage != ApplicationSession.CREATE_MODE_STRING)
            {
                if (chkIsLoad_CIRelation.Checked)
                    GetEquipmentRelation(ddlEquipmentNo.SelectedValue);
            }
        }

        protected void btnEquipmentChange_Click(object sender, EventArgs e)
        {
            try
            {
                EquipmentSelected();

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

        #region Change Area Selected

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

        //private void bindTierOperationRefIncidentArea()
        //{
        //    BindDataTierOperation();
        //    //string IncidentAreaCode = ddlProblemGroup.SelectedValue + ddlProblemType.SelectedValue + ddlProblemSource.SelectedValue + ddlServiceSource.SelectedValue;
        //    //if (!hddIncidentAreaCode.Value.Equals(IncidentAreaCode))
        //    //{
        //    //    hddIncidentAreaCode.Value = IncidentAreaCode;
        //    //    udpUpdateAreaCode.Update();
        //    //    BindDataTierOperation();
        //    //    //ClientService.DoJavascript("bindHierarchyIncidentTicket();");
        //    //}
        //}
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
                BindDataTierOperation();
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
                BindDataTierOperation();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        //protected void btnCreateRelationTicket_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Button btn = sender as Button;

        //        if (!string.IsNullOrEmpty(btn.CommandArgument))
        //        {
        //            string equipmentCode = btn.CommandArgument;
        //            string customerCode = "";

        //            if (serviceCallEntity != null && serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
        //            {
        //                customerCode = serviceCallEntity.cs_servicecall_header.Rows[0]["CustomerCode"].ToString();
        //            }

        //            DataTable dtCheckEquipment = universalService.GetEquipmentCustomerAssignment(SID, CompanyCode,
        //                equipmentCode, customerCode);

        //            if (dtCheckEquipment.Rows.Count == 0)
        //            {
        //                throw new Exception("Equipment " + equipmentCode + " is not assigned to customer " + customerCode + " : " + GetCustomerDesc(customerCode));
        //            }

        //            CreatedNewTicket(equipmentCode);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientService.AGError(ex.Message);
        //    }
        //    finally
        //    {
        //        ClientService.AGLoading(false);
        //    }
        //}

        protected void btnReOpenTicket_Click(object sender, EventArgs e)
        {
            try
            {
                //lib.reOpenTicket(
                //    SID,
                //    CompanyCode,
                //    hddDocnumberTran.Value,
                //    (string)Session["SCT_created_cust_code"+ idGen],
                //    (string)Session["SCT_created_fiscalyear"+ idGen],
                //    (string)Session["SCT_created_doctype_code"+ idGen],
                //    EmployeeCode
                //);

                saveReOpenCall(EmployeeCode);
                lib.reOpenTicketTask_SLA(SID, CompanyCode, hddDocnumberTran.Value);

                List<logValue_OldNew> enLog = new List<logValue_OldNew>();
                enLog.Add(new logValue_OldNew
                {
                    Value_Old = "",
                    Value_New = "Re-Open Ticket",
                    TableName = "",
                    FieldName = "",
                    AccessCode = LogServiceLibrary.AccessCode_Change
                });
                SaveLog(enLog);

                Session["SCF_SAVESUCCESS" + idGen] = "Re-Open Ticket Success Document Number : " + _txt_docnumberTran.Value;
                getdataToedit(DocType, hddDocnumberTran.Value.Trim(), (string)Session["SCT_created_fiscalyear" + idGen]);
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
                if (string.IsNullOrEmpty(CustomerSelect.SelectedValue))
                {
                    throw new Exception("กรุณาระบุ Customer");
                }
                if (string.IsNullOrEmpty(_txt_year.Value))
                {
                    throw new Exception("กรุณาระบุ Fiscal Year");
                }

                if (!string.IsNullOrEmpty(hddEquepmentCodeRef.Value))
                {
                    string equipmentCode = hddEquepmentCodeRef.Value;
                    string customerCode = CustomerSelect.SelectedValue;

                    //if (serviceCallEntity != null && serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
                    //{
                    //    customerCode = serviceCallEntity.cs_servicecall_header.Rows[0]["CustomerCode"].ToString();
                    //}

                    // Zaan Unlock CI
                    //DataTable dtCheckEquipment = universalService.GetEquipmentCustomerAssignment(SID, CompanyCode,
                    //    equipmentCode, customerCode);

                    //if (dtCheckEquipment.Rows.Count == 0)
                    //{
                    //    throw new Exception("Equipment " + equipmentCode + " is not assigned to customer " + customerCode + " : " + GetCustomerDesc(customerCode));
                    //}

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
                BindDataTierOperation();
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
                    string SERVICE_DOC_STATUS_RESOLVE = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE);
                    string SERVICE_DOC_STATUS_RESPONSECUSTOMER = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_RESPONSECUSTOMER);


                    saveUpdateTicketDocStatus(hddTicketStatus_New.Value);
                    if (hddTicketStatus_Old.Value == SERVICE_DOC_STATUS_RESOLVE)
                    {
                        lib.reOpenTicketTask_SLA(SID, CompanyCode, hddDocnumberTran.Value);
                        BindDataTierOperation();
                        ClientService.DoJavascript("focusRemarkBox(true);");
                    }
                    if (hddTicketStatus_New.Value == SERVICE_DOC_STATUS_RESPONSECUSTOMER)
                    {
                        string ResponseDate = Validation.getCurrentServerStringDateTime().Substring(0, 8);
                        string ResponseTime = Validation.getCurrentServerDateTime().ToString("HHmmss");
                        string ResponseBy = EmployeeCode;

                        saveTimetampResponseToCustomer(ResponseDate, ResponseTime, ResponseBy);
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
                

                
                if (ddlAffectSLA.SelectedValue.ToString().Trim() == "" || tbSummaryProblem.Text.Trim() == ""  || tbSummaryCause.Text.Trim() =="" || tbSummaryResolution.Text.Trim() == "") {
                    ClientService.DoJavascript("tabSummaryIsNotNull();");
                    throw new Exception("All input in summary tab is not null");
                }

                string DocStatusResolve = ServiceTicketLibrary.GetInstance().GetTicketStatusFromEvent(
                    SID,
                    CompanyCode,
                    ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE
                );
                if (DocStatusResolve != hddTicketStatus.Value)
                {
                    throw new Exception("Please resolve ticket");
                }

                List<logValue_OldNew> enLog = new List<logValue_OldNew>();
                foreach (DataRow drHeader in serviceCallEntity.cs_servicecall_header.Rows)
                {
                    if (Convert.ToString(drHeader["AffectSLA"]) != ddlAffectSLA.SelectedValue)
                    {

                        string textOldValue = "";
                        if (Convert.ToString(drHeader["AffectSLA"]) == "")
                        {
                            textOldValue = "";
                        }
                        else if (Convert.ToString(drHeader["AffectSLA"]) == "True")
                        {
                            textOldValue = "Affect SLA";
                        }
                        else if (Convert.ToString(drHeader["AffectSLA"]) == "False")
                        {
                            textOldValue = "Not Affect SLA";
                        }

                        string textNewValue = ddlAffectSLA.SelectedItem.Text;

                        enLog.Add(new logValue_OldNew
                        {
                            Value_Old = textOldValue,
                            Value_New = textNewValue,
                            TableName = "cs_servicecall_header",
                            FieldName = "Affect SLA",
                            AccessCode = LogServiceLibrary.AccessCode_Change
                        });
                    }
                }

                foreach (DataRow drItem in serviceCallEntity.cs_servicecall_item.Rows)
                {
                    if (Convert.ToString(drItem["SummaryProblem"]) != tbSummaryProblem.Text)
                    {
                        enLog.Add(new logValue_OldNew
                        {
                            Value_Old = Convert.ToString(drItem["SummaryProblem"]),
                            Value_New = tbSummaryProblem.Text,
                            TableName = "cs_servicecall_item",
                            FieldName = "Summary Problem",
                            AccessCode = LogServiceLibrary.AccessCode_Change
                        });
                    }
                    if (Convert.ToString(drItem["SummaryCause"]) != tbSummaryCause.Text)
                    {
                        enLog.Add(new logValue_OldNew
                        {
                            Value_Old = Convert.ToString(drItem["SummaryCause"]),
                            Value_New = tbSummaryCause.Text,
                            TableName = "cs_servicecall_item",
                            FieldName = "Summary Cause",
                            AccessCode = LogServiceLibrary.AccessCode_Change
                        });
                    }
                    if (Convert.ToString(drItem["SummaryResolution"]) != tbSummaryResolution.Text)
                    {
                        enLog.Add(new logValue_OldNew
                        {
                            Value_Old = Convert.ToString(drItem["SummaryResolution"]),
                            Value_New = tbSummaryResolution.Text,
                            TableName = "cs_servicecall_item",
                            FieldName = "Summary Resolution",
                            AccessCode = LogServiceLibrary.AccessCode_Change
                        });
                    }
                }

                #region close servicecall
                string canceldate = Validation.getCurrentServerStringDateTime().Substring(0, 8);
                string canceltime = Validation.getCurrentServerDateTime().ToString("HHmmss");
                string cancelby = EmployeeCode;
                string cancelcomment = txtRemarkCloseTicket.Text; //_txt_popup_remark.InnerText;
                string close_status = ServiceTicketLibrary.SERVICE_CALL_STATUS_CLOSE;

                DataTable dtHeader = serviceCallEntity.cs_servicecall_header;
                DataTable dtItem = serviceCallEntity.cs_servicecall_item;
                DataRow[] drcurrentRow = dtItem.Select("xLineNo='" + (string)Session["responsecall_lineno" + idGen] + "'");

                if (drcurrentRow.Length != 0)
                {
                    drcurrentRow[0]["ClosedOnDate"] = canceldate;
                    drcurrentRow[0]["ClosedOnTime"] = canceltime;
                    drcurrentRow[0]["CloseComment"] = cancelcomment;
                    drcurrentRow[0]["CloseBy"] = cancelby;
                    drcurrentRow[0]["CloseStatus"] = close_status;
                    //drcurrentRow[0]["Docstatus"] = ServiceTicketLibrary.SERVICE_DOC_STATUS_CLOSED;
                }

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
                    string SERVICE_DOC_STATUS_CLOSED = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_CLOSED);

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

                getdataToedit(DocType, hddDocnumberTran.Value.Trim(), (string)Session["SCT_created_fiscalyear" + idGen]);
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
            string Condition = "EmployeeCode = '" + EmployeeCode + "'";
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

        //stringDateTimeForm & stringDateTimeTo format 20181031120000 (YYYYMMDDhhmmss)
        public string ConvertToTimeString(string stringDateTimeForm, string stringDateTimeTo)
        {
            DateTime DateTimeForm = ObjectUtil.ConvertDateTimeDBToDateTime(stringDateTimeForm);
            DateTime DateTimeTo = ObjectUtil.ConvertDateTimeDBToDateTime(stringDateTimeTo);

            TimeSpan ts = DateTimeTo - DateTimeForm;
            string time = ts.TotalSeconds.ToString();

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
            else
            {
                return "";
            }

            return "ไม่กำหนด";
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

            double sumStopSeconds = 0;

            #region Get assign, resolve, close
            foreach (DataRow dr in serviceCallEntity.cs_servicecall_item.Rows)
            {
                assignDateTime = dr["AssignDate"].ToString() + dr["AssignTime"].ToString();
                resolveDateTime = dr["ResolutionOnDate"].ToString() + dr["ResolutionOnTime"].ToString();
                closeDateTime = dr["ClosedOnDate"].ToString() + dr["ClosedOnTime"].ToString();
            }
            #endregion

            #region Get last modified
            string ticketType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
            string ticketNo = serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString();
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

                    sumStopSeconds += ts.TotalSeconds;
                }
            }

            stopTime = ConvertToTime(sumStopSeconds.ToString(), true);

            #endregion

            #region Calculate total time
            if (assignDateTime != "" && resolveDateTime != "")
            {
                DateTime assign = ObjectUtil.ConvertDateTimeDBToDateTime(assignDateTime);
                DateTime resolve = ObjectUtil.ConvertDateTimeDBToDateTime(resolveDateTime);

                TimeSpan ts = resolve - assign;

                totalTime = ConvertToTime(ts.TotalSeconds.ToString(), true);
                totalTimeWithoutStop = ConvertToTime((ts.TotalSeconds - sumStopSeconds).ToString(), true);
            }
            #endregion

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

        #region Transfer Participants

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnsender = sender as Button;
                RepeaterItem rpt = btnsender.NamingContainer as RepeaterItem;
                HiddenField hhdAOBJECTLINK = rpt.FindControl("hhdAOBJECTLINK") as HiddenField;
                HiddenField hddTierCode = rpt.FindControl("hddTierCode") as HiddenField;
                HiddenField hddTier = rpt.FindControl("hddTier") as HiddenField;
                HiddenField hddSLAGroup = rpt.FindControl("hddSLAGroup") as HiddenField;
                HiddenField hddOwnerService_Select = rpt.FindControl("hddOwnerService_Select") as HiddenField;
                HiddenField hddSLAGroupCode_Select = rpt.FindControl("hddSLAGroupCode_Select") as HiddenField;

                if (!AfterSaleService.CheckCurrentTier(CompanyCode, hddDocnumberTran.Value, hhdAOBJECTLINK.Value))
                {
                    btnSelectIncidentArea_Click(null, null);
                    ClientService.AGMessage("There are changes in the tier, Please do the transaction again.");
                    return;
                }

                if (string.IsNullOrEmpty(hddSLAGroupCode_Select.Value))
                    ddlTransfer_SLAGroup.SelectedValue = hddSLAGroup.Value;
                else
                    ddlTransfer_SLAGroup.SelectedValue = hddSLAGroupCode_Select.Value;

                if (string.IsNullOrEmpty(hddOwnerService_Select.Value))
                    ddlTransfer_OwnerService.SelectedValue = ddlOwnerGroupService.SelectedValue;
                else
                    ddlTransfer_OwnerService.SelectedValue = hddOwnerService_Select.Value;

                UpdatePanel3.Update();
                UpdatePanel5.Update();

                hddTransfer_SLAGroup.Value = hddSLAGroup.Value;
                hddTransfer_SLAGroupName.Value = ddlTransfer_SLAGroup.SelectedItem.Text;
                hddTransfer_OwnerService.Value = ddlTransfer_OwnerService.SelectedValue;
                hddTransfer_OwnerServiceName.Value = ddlTransfer_OwnerService.SelectedItem.Text;
                hddTransfer_AOBJECTLINK.Value = hhdAOBJECTLINK.Value;
                hddTransfer_TierCode.Value = hddTierCode.Value;
                hddTransfer_Tier.Value = hddTier.Value;
                hddTransfer_ListEMPCode.Value = "";
                udpTransfer.Update();
                AutoCompleteEmployee.SelectedValueRefresh = "";
                LoadOperationTransfer(hddTransfer_AOBJECTLINK.Value);
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
            if (dtTier == null)
            {
                string tierCode = getTierCode();
                dtTier = AfterSaleService.getInstance().getTierOperation(SID, tierCode, hddDocnumberTran.Value);
            }

            foreach (DataRow dr in dtTier.Select("AOBJECTLINK = '" + AobjectLink + "'"))
            {
                hddTransfer_TaskName.Value = dr["Subject"].ToString();
                string tierCode = dr["TierCode"].ToString();
                string Tier = dr["Tier"].ToString();
                string CallStatus = "";
                string ticketType = "";

                if (serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
                {
                    ticketType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
                    CallStatus = serviceCallEntity.cs_servicecall_header.Rows[0]["CallStatus"].ToString();
                }

                DataTable dtMain = AfterSaleService.getInstance().GetTierMainDelegate(
                    SID, CompanyCode,
                    tierCode, Tier, ticketType,
                    AobjectLink, ddlTransfer_OwnerService.SelectedValue
                );

                DataTable dtParticipant = lib.getListEmpParticipantsRefTicket(
                    SID, CompanyCode,
                    AobjectLink, hddDocnumberTran.Value,
                    tierCode, Tier,
                    _txt_fiscalyear.Value, ticketType,
                    ddlTransfer_OwnerService.SelectedValue
                );

                if (dtParticipant.Rows.Count == 0)
                {
                    dtParticipant = AfterSaleService.getInstance().GetTierParticipants(
                        SID, CompanyCode,
                        tierCode, Tier, ticketType,
                        AobjectLink, ddlTransfer_OwnerService.SelectedValue
                    );
                }
                DataTable dt = new DataTable();
                dt.Columns.Add("empCode");
                dt.Columns.Add("empName");
                dt.Columns.Add("empImg");
                dt.Columns.Add("IsMain");

                foreach (DataRow drMain in dtMain.Rows)
                {
                    DataRow drNew = dt.Rows.Add();
                    drNew["empCode"] = drMain["EmployeeCode"].ToString();
                    drNew["empName"] = drMain["fullname"].ToString();
                    drNew["empImg"] = "/images/user.png";
                    drNew["IsMain"] = "true";
                }
                foreach (DataRow drParticipant in dtParticipant.Rows)
                {
                    DataRow drNew = dt.Rows.Add();
                    drNew["empCode"] = drParticipant["EmployeeCode"].ToString();
                    drNew["empName"] = drParticipant["fullname"].ToString();
                    drNew["empImg"] = "/images/user.png";
                    drNew["IsMain"] = "false";
                }

                rptOperationTransfer.DataSource = dt;
                rptOperationTransfer.DataBind();
                udpTransfer.Update();
            }

        }

        protected void btnTransferAddParticipant_Click(object sender, EventArgs e)
        {
            try
            {

                if (!AfterSaleService.CheckCurrentTier(CompanyCode, hddDocnumberTran.Value, hddTransfer_AOBJECTLINK.Value))
                {
                    btnSelectIncidentArea_Click(null, null);
                    ClientService.AGMessage("There are changes in the tier, Please do the transaction again.");
                    return;
                }
                //HiddenField hddTierCode = e.Item.FindControl("hddTierCode") as HiddenField;
                //HiddenField hddTier = e.Item.FindControl("hddTier") as HiddenField;

                List<listOperationTransfer> listEmp = JsonConvert.DeserializeObject<List<listOperationTransfer>>(hddTransfer_ListEMPCode.Value);

                if (ddlTransfer_SLAGroup.SelectedValue != hddTransfer_SLAGroup.Value
                    || ddlTransfer_OwnerService.SelectedValue != hddTransfer_OwnerService.Value)
                {
                    string logMessage = "";
                    if (ddlTransfer_SLAGroup.SelectedValue != hddTransfer_SLAGroup.Value)
                    {
                        logMessage += "Change SLA Group '" + hddTransfer_SLAGroupName.Value + "' to '" + ddlTransfer_SLAGroup.SelectedItem.Text + "'.";
                    }

                    if (ddlTransfer_OwnerService.SelectedValue != hddTransfer_OwnerService.Value)
                    {
                        //if (!string.IsNullOrEmpty(logMessage))
                        //{
                        //    logMessage += " /n ";
                        //}

                        logMessage += " Change Owner Service '" + hddTransfer_OwnerServiceName.Value + "' to '" + ddlTransfer_OwnerService.SelectedItem.Text + "'.";
                    }

                    double resolutionTime = 0;
                    double.TryParse(hddTransfer_resolutionTimes.Value, out resolutionTime);

                    lib.saveNewGroupEmployeeTransfer(
                        SID,
                        CompanyCode,
                        hddDocnumberTran.Value,
                        hddTransfer_TierCode.Value,
                        hddTransfer_Tier.Value,
                        EmployeeCode,
                        FullNameTH,
                        hddTransfer_AOBJECTLINK.Value,
                        listEmp,
                        ddlTransfer_SLAGroup.SelectedValue,
                        ddlTransfer_OwnerService.SelectedValue,
                        resolutionTime
                    );

                    // listEmp.Where(w => !string.IsNullOrEmpty(w.EventDesc)).Select(s => s.EventDesc).ToList()
                    logMessage += " \n" + string.Join("\n", listEmp.Where(w => !string.IsNullOrEmpty(w.EventDesc)).Select(s => s.EventDesc).ToList());

                    if (!string.IsNullOrEmpty(logMessage))
                    {
                        lib.SaveActivityDetail(
                            SID, CompanyCode, CompanyCode,
                            EmployeeCode, EmployeeCode, FullNameEN,
                            hddTransfer_AOBJECTLINK.Value, "", logMessage, "", Validation.getCurrentServerStringDateTimeMillisecond(),
                            "Transfer", "", "", ""
                        );
                    }

                    SnapDataLibrary.getInstance().saveDataSnap(
                        SID,
                        CompanyCode,
                        hddTransfer_AOBJECTLINK.Value,
                        SnapDataLibrary.SNAP_EVENT_OBJECT_TRANSFER,
                        ddlTransfer_SLAGroup.SelectedValue,
                        ddlTransfer_OwnerService.SelectedValue,
                        logMessage,
                        EmployeeCode
                    );
                }
                else
                {
                    lib.saveEmployeeParticipantRefTicket(
                        SID,
                        CompanyCode,
                        hddDocnumberTran.Value,
                        hddTransfer_TierCode.Value,
                        hddTransfer_Tier.Value,
                        EmployeeCode,
                        FullNameTH,
                        hddTransfer_AOBJECTLINK.Value,
                        listEmp
                    );

                    //string logMessage = string.Join("\n", listEmp.Where(w => !string.IsNullOrEmpty(w.EventDesc)).Select(s => s.EventDesc).ToList());

                    // if (!string.IsNullOrEmpty(logMessage))
                    // {
                    //     lib.SaveActivityDetail(
                    //         SID, CompanyCode, CompanyCode,
                    //         EmployeeCode, EmployeeCode, FullNameEN,
                    //         hddTransfer_AOBJECTLINK.Value, "", logMessage, "", Validation.getCurrentServerStringDateTimeMillisecond(),
                    //         "Transfer", "", "", ""
                    //     );
                    // }
                }

                NotificationLibrary.GetInstance().TicketAlertEvent(
                    NotificationLibrary.EVENT_TYPE.TICKET_TRANSFER,
                    SID,
                    CompanyCode,
                    hddDocnumberTran.Value,
                    EmployeeCode,
                    ThisPage
                );

                BindDataTierOperation();
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

        protected void ddlTransfer_OwnerService_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string ticketType = "";
                string TierCode = hddTransfer_TierCode.Value;

                if (serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
                {
                    ticketType = serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString();
                }

                if (hddTransfer_SLAGroup.Value != ddlTransfer_SLAGroup.SelectedValue)
                {

                    DataTable dtTierSelect = TierService.getInStance().searchTierMaster(
                        SID,
                        WorkGroupCode,
                        "", "",
                        ddlTransfer_SLAGroup.SelectedValue
                    );
                    dtTierSelect.DefaultView.RowFilter = "PriorityCode = '" + _ddl_priorityTran.SelectedValue + "'";
                    dtTierSelect = dtTierSelect.DefaultView.ToTable();
                    dtTierSelect.DefaultView.RowFilter = string.Empty;
                    TierCode = dtTierSelect.Rows[0]["tierCode"].ToString();

                    DataTable dtTierSelecDetail = AfterSaleService.getInstance().getTierItem(
                        SID,
                        WorkGroupCode,
                        dtTierSelect.Rows[0]["TierCode"].ToString()
                    );
                    dtTierSelecDetail.DefaultView.RowFilter = "Tier = '" + hddTransfer_Tier.Value + "'";
                    dtTierSelecDetail = dtTierSelecDetail.DefaultView.ToTable();
                    dtTierSelecDetail.DefaultView.RowFilter = string.Empty;

                    hddTransfer_resolutionTimes.Value = dtTierSelecDetail.Rows[0]["Resolution"].ToString();
                }

                DataTable dtMain = AfterSaleService.getInstance().GetTierMainDelegate(
                    SID, CompanyCode,
                    TierCode, "", ticketType,
                    /*hddIncidentAreaCode.Value,*/ "",
                    ddlTransfer_OwnerService.SelectedValue
                );

                DataTable dtParticipant = AfterSaleService.getInstance().GetTierParticipants(
                    SID, CompanyCode,
                    TierCode, hddTransfer_Tier.Value, ticketType,
                    /*hddIncidentAreaCode.Value,*/ "",
                    ddlTransfer_OwnerService.SelectedValue
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
                    drNew["empName"] = drMain["fullname"].ToString();
                    drNew["empImg"] = "/images/user.png";
                    drNew["IsMain"] = "true";
                }
                foreach (DataRow drParticipant in dtParticipant.Rows)
                {
                    DataRow drNew = dt.Rows.Add();
                    drNew["empCode"] = drParticipant["EmployeeCode"].ToString();
                    drNew["empName"] = drParticipant["fullname"].ToString();
                    drNew["empImg"] = "/images/user.png";
                    drNew["IsMain"] = "false";
                }

                rptOperationTransfer.DataSource = dt;
                rptOperationTransfer.DataBind();
                udpTransfer.Update();
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

        #region bind Contect For Add
        protected void MyEventHandlerFunction_afterSaveRebindPostBack(object sender, EventArgs e)
        {
            GetContact();
            List<ERPW.Lib.Master.ContactEntity> listAddNew = (List<ERPW.Lib.Master.ContactEntity>)sender;
            if (listAddNew != null && listAddNew.Count > 0)
            {
                ERPW.Lib.Master.ContactEntity en = listAddNew.Last();
                if (!string.IsNullOrEmpty(en.NAME1) || !string.IsNullOrEmpty(en.NickName))
                {
                    string BOBJLINK = en.AOBJECTLINK + en.ITEMNO;
                    _ddl_contact_person.SetValue = BOBJLINK;
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

                        //r.ParentItemDescription = ServiceTicketLibrary.GetInstance().ReplaceTicketNumberToDisplay(prefix, ticketNoDisplay);//  prefix + Convert.ToInt32(ticketNoDisplay) + " : " + r.ParentItemDescription;
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
                    null//CustomerCode
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


            //divJsonListDataKnowledgeManagement.InnerHtml = JsonConvert.SerializeObject(listKm);
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
                //if (txtSearch.Text != "" || !String.IsNullOrEmpty(ddlGroup.SelectedValue))
                //{
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
                //List<KMCriteriaEntity> listData = libkm.GetKMGroup(SID);
                //ddlGroupModal.DataValueField = "xKey";
                //ddlGroupModal.DataTextField = "xValue";
                //ddlGroupModal.DataSource = listData;
                //ddlGroupModal.DataBind();
                txtKeywordModal.Text = "";
                txtSubjectModal.Text = txt_problem_topic.Text;
                txtDetailModal.Text = tbEquipmentRemark.Text;
                txtSymtomModal.Text = tbSummaryProblem.Text;
                txtCauseModal.Text = tbSummaryCause.Text;
                txtSolutionModal.Text = tbSummaryResolution.Text;
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
                    DocType,
                    hddDocnumberTran.Value.Trim(),
                    (string)Session["SCT_created_fiscalyear" + idGen],
                    true, false, mode_stage
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

                ShowWorkFlow = true;
                ShowWorkFlowWithoutCreate = true;
                AlowWorkFlow = true;
                countActivity = 0;
                Session["SCT_GROUPCODE" + idGen] = null;

                clearScreen();

                if (mode_stage == ApplicationSession.CREATE_MODE_STRING)
                {
                    initData_CreateMode();
                }
                //initData_AfterInitMode();

                ddlProblemGroup.SelectedValue = "";
                btnChangeProblemGroup_Click(null, null);

                udpnEQDetail.Update();
                udpnEquipment.Update();
                udpnSeverity.Update();
                udpnProblem.Update();
                udpRefer.Update();
                udpRemark.Update();

                //Session["SC_MODE" + idGen] = ApplicationSession.CREATE_MODE_STRING;
                //Session["ServicecallEntity" + idGen] = new tmpServiceCallDataSet();
                //Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/ServiceCallTransaction.aspx?id=" + idGen));
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

        protected void btnBackPage_Click(object sender, EventArgs e)
        {
            try
            {
                string businessObject = mBusinessObject;
                if (string.IsNullOrEmpty(businessObject))
                {
                    string _doctype = "";

                    if (serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
                    {
                        foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
                        {
                            _doctype = Convert.ToString(dr["DocType"]);
                        }
                    }
                    else
                    {
                        _doctype = DocType;
                    }

                    businessObject = lib.GetBusinessObjectFromTicketType(SID, _doctype);
                }

                Session["TK_BusinessObject"] = businessObject;
                Response.Redirect(Page.ResolveUrl("~/crm/AfterSale/ServiceCallFastEntryCriteria.aspx"), true);
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

        #region Material Purchase

        private DataTable GetMaterialUOM(string materialCode)
        {
            DataTable dt = materialService.getInSatnce().GetMaterialSalesUnit(SID, materialCode);            
         
            return dt;
        }

        private void GetMaterialPurchase()
        {
            //dtMaterialMaster = materialService.getInSatnce().getMaterialByMatCode(SID, "");

            DataTable DTSourceMaterial = new DataTable();
            DTSourceMaterial.Columns.Add("code");
            DTSourceMaterial.Columns.Add("desc");
            DTSourceMaterial.Columns.Add("display");            

            foreach (DataRow dr in dtMaterialMaster.Rows)
            {
                DataRow drNew = DTSourceMaterial.NewRow();
                drNew["code"] = Convert.ToString(dr["ItmNumber"]);
                drNew["desc"] = Convert.ToString(dr["ItmDescription"]);
                drNew["display"] = Convert.ToString(dr["ItmNumber"]) + " : " + Convert.ToString(dr["ItmDescription"]);                
                DTSourceMaterial.Rows.Add(drNew);
            }

            hddJsonMat.Value = JsonConvert.SerializeObject(DTSourceMaterial);

            dtMaterialPurchase = AfterSaleService.getInstance().GetTicketMaterial(
                SID,
                CompanyCode,
                serviceCallEntity.cs_servicecall_header.Rows[0]["Fiscalyear"].ToString(),
                serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString(),
                serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString());
        }

        private void PrepareDataMaterialToTable()
        {
            foreach (RepeaterItem e in rptMaterial.Items)
            {
                HiddenField hdfItemNo = e.FindControl("hdfItemNo") as HiddenField;
                
                DataRow[] drr = dtMaterialPurchase.Select("ItemNo = '" + hdfItemNo.Value + "'");

                foreach (DataRow dr in drr)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }
                  
                    TextBox tbUnitPrice = e.FindControl("tbUnitPrice") as TextBox;
                    TextBox tbQty = e.FindControl("tbQty") as TextBox;

                    TextBox hdfMaterialCode = e.FindControl("hdfMaterialCode") as TextBox;
                    DropDownList ddlUom = e.FindControl("ddlUom") as DropDownList;

                    decimal unitPrice = 0, qty = 0;
                    decimal.TryParse(tbUnitPrice.Text, out unitPrice);
                    decimal.TryParse(tbQty.Text, out qty);

                    dr["MaterialCode"] = hdfMaterialCode.Text.Trim();
                    dr["UnitPrice"] = unitPrice;
                    dr["Qty"] = qty;
                    dr["UOM"] = ddlUom.SelectedValue;
                }
            }
        }

        private void BindingMaterialPurchase(bool update = true)
        {
            rptMaterial.DataSource = dtMaterialPurchase;
            rptMaterial.DataBind();

            if (update)
            {
                udpnMaterial.Update();
            }            
        }

        protected void rptMaterial_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                HiddenField hdfItemNo = e.Item.FindControl("hdfItemNo") as HiddenField;

                DataRow[] drr = dtMaterialPurchase.Select("ItemNo = '" + hdfItemNo.Value + "'");

                foreach (DataRow dr in drr)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    HiddenField hdfMaterialUOM = e.Item.FindControl("hdfMaterialUOM") as HiddenField;

                    TextBox tbMaterial = e.Item.FindControl("tbMaterial") as TextBox;
                    DropDownList ddlUom = e.Item.FindControl("ddlUom") as DropDownList;

                    ClientService.DoJavascript("initAutoCompleteMaterial('#" + tbMaterial.ClientID + "', '" + dr["MaterialCode"] + "');");

                    DataTable dt = GetMaterialUOM(dr["MaterialCode"].ToString().Trim());

                    ddlUom.DataSource = dt;
                    ddlUom.DataBind();

                    if (hdfMaterialUOM.Value != "")
                    {
                        DataRow[] drrUOM = dt.Select("UCODE = '" + hdfMaterialUOM.Value + "'");

                        if (drrUOM.Length > 0)
                        {
                            ddlUom.SelectedValue = hdfMaterialUOM.Value;
                        }
                    }
                }               
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        protected void btnAddRowMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                PrepareDataMaterialToTable();

                string itemNo = "001";

                if (dtMaterialPurchase.Rows.Count > 0)
                {
                    itemNo = Convert.ToString(Convert.ToInt32(dtMaterialPurchase.Compute("MAX(ItemNo)", "")) + 1).PadLeft(3, '0');
                }

                DataRow dr = dtMaterialPurchase.NewRow();
                dr["ItemNo"] = itemNo;
                dtMaterialPurchase.Rows.Add(dr);

                BindingMaterialPurchase(false);
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

        protected void btnGetMaterialUOM_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;

                RepeaterItem rptItem = btn.NamingContainer as RepeaterItem;

                TextBox hdfMaterialCode = rptItem.FindControl("hdfMaterialCode") as TextBox;
                DropDownList ddlUom = rptItem.FindControl("ddlUom") as DropDownList;

                DataTable dt = GetMaterialUOM(hdfMaterialCode.Text.Trim());
                
                ddlUom.DataSource = dt;
                ddlUom.DataBind();
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

        protected void btnDeleteRowMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;

                string itemNo = btn.CommandArgument;

                DataRow[] drr = dtMaterialPurchase.Select("ItemNo = '" + itemNo + "'");

                if (drr.Length > 0)
                {
                    PrepareDataMaterialToTable();

                    drr[0].Delete();

                    BindingMaterialPurchase(false);
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

        #region Property Value 
        //04/01/2562 by born kk
        private void setHeaderPropertyValue() {

            AfterSaleService after = new AfterSaleService();
            List<AfterSaleService.ERPWPropertyValueHeader> lhdpv = after.GetERPWPropertyHeader(SID, CompanyCode);
            
            foreach (AfterSaleService.ERPWPropertyValueHeader prop in lhdpv) {
                DataTable dt = new DataTable();
                dt.TableName = prop.PostingType + prop.HeaderCode + idGen;
                dt.Columns.Add("ItemNo");
                dt.Columns.Add("PostingType");
                dt.Columns.Add("HeaderCode");
                dt.Columns.Add("ItemCode");
                dt.Columns.Add("ItemDesc");
                dt.Columns.Add("Value");
                Session[prop.PostingType + prop.HeaderCode+ idGen] = dt;
            }
            rptHeaderProperty.DataSource = lhdpv;
            rptHeaderPropertyDeatail.DataSource = lhdpv;
            rptHeaderProperty.DataBind();
            rptHeaderPropertyDeatail.DataBind();

        }

        protected void btnAddNewRowPropertyValueItem_click(object sender ,EventArgs e) {
            PreparePropertyValue();
            try {
                string postheaderselect = hddPostHeader.Value;
                foreach (RepeaterItem item in rptHeaderPropertyDeatail.Items)
                {
                    HiddenField postheader = item.FindControl("hddPostHeaderInRpt") as HiddenField;
                    HiddenField postingtype = item.FindControl("hddPostigTypeInRpt") as HiddenField;
                    HiddenField headercode = item.FindControl("hddHeaderCodeInRpt") as HiddenField;
                    if (postheader.Value == postheaderselect)
                    {
                        DataTable dt = Session[postheader.Value+ idGen] as DataTable;
                        string itemNo = "00001";
                        if (dt.Rows.Count == 5 ) {
                            break;
                        }
                        if (dt.Rows.Count > 0 )
                        {
                            itemNo = Convert.ToString(Convert.ToInt32(dt.Compute("MAX(ItemNo)", "")) + 1).PadLeft(5, '0');
                        }
                        DataRow row = dt.NewRow();
                        row["ItemNo"] = itemNo;
                        row["PostingType"] = postingtype.Value;
                        row["HeaderCode"] = headercode.Value;
                        row["ItemCode"] = itemNo;
                        dt.Rows.Add(row);
                        Session[postheader.Value+ idGen] = dt;
                        break;
                    }

                }
               
                BindingPropertyValue();
            } catch(Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
            finally
            {
                ClientService.AGLoading(false);
            }
           
        }

        private void BindingPropertyValue() {
           
            foreach (RepeaterItem item in rptHeaderPropertyDeatail.Items) {
                Repeater rpt = item.FindControl("rptPropertyItem") as Repeater;
                HiddenField postheader = item.FindControl("hddPostHeaderInRpt") as HiddenField;
                UpdatePanel udp = item.FindControl("udpPropertyDetail") as UpdatePanel;
                DataTable dt = Session[postheader.Value+ idGen] as DataTable;
                rpt.DataSource = dt;
                rpt.DataBind();
                udp.Update();
                dt = new DataTable();
            }
        }

        private void PreparePropertyValue() {
            foreach (RepeaterItem item in rptHeaderPropertyDeatail.Items)
            {
                Repeater rpt = item.FindControl("rptPropertyItem") as Repeater;
                HiddenField postheader = item.FindControl("hddPostHeaderInRpt") as HiddenField;
                DataTable dt = Session[postheader.Value + idGen] as DataTable;
                foreach (RepeaterItem itemInRpt in rpt.Items) {
                    HiddenField hdfItemNo = itemInRpt.FindControl("hdfItemNo") as HiddenField;
                    DataRow[] dtr = dt.Select("ItemNo = '" + hdfItemNo.Value + "'");
                    foreach (DataRow row in dtr) {
                        if (row.RowState == DataRowState.Deleted)
                        {
                            continue;
                        }

                        TextBox txtValue = itemInRpt.FindControl("txtValue") as TextBox;
                        row["Value"] = txtValue.Text;
                    }
                }
                Session[postheader.Value + idGen] = dt;
            }
        }

        private void SavePropertyValue(string fiscalYear, string documentType, string documentNo) {
            
                foreach (RepeaterItem item in rptHeaderPropertyDeatail.Items)
                {
                    HiddenField postheader = item.FindControl("hddPostHeaderInRpt") as HiddenField;
                HiddenField postingtype = item.FindControl("hddPostigTypeInRpt") as HiddenField;
                HiddenField headercode = item.FindControl("hddHeaderCodeInRpt") as HiddenField;
                DataTable dt = Session[postheader.Value + idGen] as DataTable;
                dt.AcceptChanges();
                AfterSaleService after = new AfterSaleService();
                after.addERPWPropertyValue(
                        SID,
                        CompanyCode,
                        fiscalYear,
                        documentType,
                        documentNo,
                           dt,
                           CompanyCode,
                           postingtype.Value,
                           headercode.Value
                        );

                }
            
            
            
        }

        private void GetPropertyValueItem() {
            AfterSaleService after = new AfterSaleService();
            foreach (RepeaterItem item in rptHeaderPropertyDeatail.Items)
            {
                HiddenField postheader = item.FindControl("hddPostHeaderInRpt") as HiddenField;
                HiddenField postingtype = item.FindControl("hddPostigTypeInRpt") as HiddenField;
                HiddenField headercode = item.FindControl("hddHeaderCodeInRpt") as HiddenField;
                Session[postheader.Value + idGen] = after.GetErpwPropertyValueItem(
                    SID,
                    CompanyCode,
                    serviceCallEntity.cs_servicecall_header.Rows[0]["Fiscalyear"].ToString(),
                serviceCallEntity.cs_servicecall_header.Rows[0]["DocType"].ToString(),
                serviceCallEntity.cs_servicecall_header.Rows[0]["CallerID"].ToString(),
                postingtype.Value,
                    headercode.Value
                    );
            }
        }

        protected void btnDeleteRowPropertyValue_click(object sender ,EventArgs e) {
            try
            {
                PreparePropertyValue();
                Button btn = sender as Button;
                string itemNo = btn.CommandArgument;
                string postheaderselect = hddPostHeader.Value;
                foreach (RepeaterItem item in rptHeaderPropertyDeatail.Items)
                {
                    Repeater rpt = item.FindControl("rptPropertyItem") as Repeater;
                    HiddenField postheader = item.FindControl("hddPostHeaderInRpt") as HiddenField;
                    DataTable dt = Session[postheader.Value + idGen] as DataTable;
                    if (postheader.Value == postheaderselect)
                    {
                        DataRow[] dtr = dt.Select("ItemNo = '" + itemNo + "'");
                        if (dtr.Length > 0)
                        {
                            
                            dtr[0].Delete();
                        }
                        Session[postheader.Value + idGen] = dt;
                        break;
                    }
                }
                BindingPropertyValue();

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

        private void validateDuplicatePropertyValue() {
            List<string> listValue = new List<string>();

            foreach (RepeaterItem item in rptHeaderPropertyDeatail.Items)
            {
                Repeater rpt = item.FindControl("rptPropertyItem") as Repeater;
                HiddenField headerDesc = item.FindControl("hddHeaderDescInRpt") as HiddenField;
             
                foreach (RepeaterItem itemInRpt in rpt.Items)
                {
                    TextBox txtValue = itemInRpt.FindControl("txtValue") as TextBox;
                    string value = txtValue.Text;
                    listValue.Add(value);
                }
                var query = listValue.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => new { Element = y.Key, Counter = y.Count() })
              .ToList();
                if (query.Count>0) {
                    if (query[0].Counter > 0) {
                        throw new Exception("Document Number : " + _txt_docnumberTran.Value + "\n Configuration Item tab "+ headerDesc.Value + " \n { " + query[0].Element+ " } ซ้ำ กรุณากรอกใหม่");
                    }
                }
            }

            
        }
        #endregion

        protected void ddlOwnerGroupService_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindDataTierOperation();
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

        protected void btnIsLoad_CIRelation_Click(object sender, EventArgs e)
        {
            try
            {
                if (!chkIsLoad_CIRelation.Checked)
                {
                    // todo
                    GetEquipmentRelation(ddlEquipmentNo.SelectedValue);

                    chkIsLoad_CIRelation.Checked = true;
                    udpIsLoad_CIRelation.Update();
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

        // add Reviews Tap
        protected void btn_reviews_Click(object sender, EventArgs e)
        {
            try
            {
                    // todo
                    bindDataReview();

                    chkIsLoad_reviews.Checked = true;
                    udpIsLoad_reviews.Update();
               
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

        private void bindDataReview()
        {
            string ticketNo = hddDataRating_TicketCode.Value;
            string emCode = hddDataRating_EmpCode.Value;

            DataTable rating = AfterSaleService.getInstance().getRating(ticketNo, emCode);

            if(rating.Rows.Count != 0)
            {
                Rating1.CurrentRating = Convert.ToInt32(rating.Rows[0]["Rating"]);
                tbCommentforReviews.Text = rating.Rows[0]["Comment"].ToString();
            }
           
            udp_reviews_detail.Update();

        }

        protected void OnRatingChanged(object sender, RatingEventArgs e)
        {
            
        }
        //=========================================== review funtion ===========================================================
        protected void btnSubmitReviews_Click(object sender, EventArgs e)
        {
            string ticketNo = hddDataRating_TicketCode.Value;
            string emCode = hddDataRating_EmpCode.Value;
            string commentStr = tbCommentforReviews.Text.Trim();
            if (commentStr != "" && Rating1.CurrentRating != 0)
            {
                AfterSaleService.getInstance().saveRating(ticketNo, emCode, Rating1.CurrentRating.ToString() , tbCommentforReviews.Text);

                string valueStr = "Ticket rewiewed , Rating:" + Rating1.CurrentRating.ToString();
                List<logValue_OldNew> enLog = new List<logValue_OldNew>();
                enLog.Add(new logValue_OldNew
                {
                    Value_Old = "",
                    Value_New = valueStr,
                    TableName = "",
                    FieldName = "",
                    AccessCode = LogServiceLibrary.AccessCode_Change
                });
                SaveLog(enLog);

                btnSubmitReviews.Visible = false;
                tbCommentforReviews.Enabled = false;
                Rating1.ReadOnly = true;

                ClientService.AGSuccess("Save Success");
                ClientService.AGLoading(false);
            }
            else
            {
                ClientService.AGError("Please fill up this form.");
                ClientService.AGLoading(false);
            }
            
          
        }
        //========================================================================================================================================
        //===================================================== chechk status ticket for review ==================================================
        protected void checkResolveStatus()
        {
            string ticketNo = hddDataRating_TicketCode.Value;
            string emCode = hddDataRating_EmpCode.Value;
            string SERVICE_DOC_STATUS_RESOLVE = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_RESOLVE);
            //เช็คสถานะ resolve เพื่ออนุญาติการแก้ไข 
            if (hddTicketStatus_New.Value != SERVICE_DOC_STATUS_RESOLVE)
            {
                udp_reviews_detail.Visible = false;
                tbCommentforReviews.Enabled = true;
                btnSubmitReviews.Visible = true;
                Rating1.ReadOnly = false;
                
            }
            else
            {
               
                DataTable rating = AfterSaleService.getInstance().getRating(ticketNo, emCode);
                if (rating.Rows.Count != 0)
                {

                    btnSubmitReviews.Visible = false;
                    tbCommentforReviews.Enabled = false;
                    Rating1.ReadOnly = true;
                }

            }
        }
        //======================================================================================================================================
    }
}










































