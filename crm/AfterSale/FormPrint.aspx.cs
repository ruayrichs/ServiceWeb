using agape.lib.constant;
using Agape.FocusOne.Utilities;
using Agape.Lib.Web.Bean.CS;
using Agape.Lib.Web.Bean.MM;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Master.Entity;
using ERPW.Lib.Service;
using ServiceWeb.auth;
using ServiceWeb.Service;
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

namespace ServiceWeb.crm.AfterSale
{
    public partial class FormPrint : AbstractsSANWebpage
    {

        //ServiceTicketLibrary lib = new ServiceTicketLibrary();
        //Agape.Lib.DBService.DBService dbService = new Agape.Lib.DBService.DBService();
        //private MasterConfigLibrary libconfig = MasterConfigLibrary.GetInstance();

        //private string _DocType;
        //private string DocType
        //{
        //    get
        //    {
        //        if (_DocType == null)
        //        {
        //            if (serviceCallEntity.cs_servicecall_header.Rows.Count > 0)
        //            {
        //                foreach (DataRow dr in serviceCallEntity.cs_servicecall_header.Rows)
        //                {
        //                    _DocType = Convert.ToString(dr["DocType"]);
        //                }
        //            }
        //            else
        //            {
        //                _DocType = (string)Session["SCT_created_doctype_code" + idGen];
        //            }
        //        }
        //        return _DocType;
        //    }
        //}

        //private string _businessObject;
        //private string businessObject
        //{
        //    get
        //    {
        //        if (_businessObject == null)
        //        {
        //            _businessObject = lib.GetBusinessObjectFromTicketType(SID, DocType);
        //        }
        //        return _businessObject;
        //    }
        //}

        //private string _idGen;
        //public string idGen
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_idGen))
        //        {
        //            _idGen = Request["id"];
        //        }
        //        return _idGen;
        //    }
        //}


        //public string _mode_stage;
        //public string mode_stage
        //{
        //    get
        //    {
        //        if (_mode_stage == null)
        //        {
        //            if (Session["SC_MODE" + idGen] == null)
        //            {
        //                Session["SC_MODE" + idGen] = ApplicationSession.CREATE_MODE_STRING;
        //                _mode_stage = ApplicationSession.CREATE_MODE_STRING;
        //            }
        //            else
        //            {
        //                _mode_stage = (string)Session["SC_MODE" + idGen];
        //            }
        //        }
        //        return _mode_stage;
        //    }
        //    set
        //    {
        //        Session["SC_MODE" + idGen] = value;
        //        _mode_stage = value;
        //    }
        //}


        //public string _CustomerCode_session;
        //public string CustomerCode_session
        //{
        //    get
        //    {
        //        if (_CustomerCode_session == null)
        //        {
        //            _CustomerCode_session = (string)Session["SCT_created_cust_code" + idGen];
        //        }
        //        return _CustomerCode_session;
        //    }
        //    set
        //    {
        //        Session["SCT_created_cust_code" + idGen] = value;
        //        _CustomerCode_session = value;
        //    }
        //}



        //private tmpServiceCallDataSet _serviceCallEntity;
        //private tmpServiceCallDataSet serviceCallEntity
        //{
        //    get
        //    {
        //        if (_serviceCallEntity == null)
        //        {
        //            if (Session["ServicecallEntity" + idGen] == null)
        //            {
        //                Session["ServicecallEntity" + idGen] = new tmpServiceCallDataSet();
        //            }

        //            _serviceCallEntity = (tmpServiceCallDataSet)Session["ServicecallEntity" + idGen];
        //        }

        //        if (_serviceCallEntity.cs_servicecall_header.PrimaryKey.Length != 6)
        //            _serviceCallEntity.cs_servicecall_header.PrimaryKey = new DataColumn[] {
        //                _serviceCallEntity.cs_servicecall_header.Columns["SID"],
        //                _serviceCallEntity.cs_servicecall_header.Columns["CompanyCode"],
        //                _serviceCallEntity.cs_servicecall_header.Columns["CallerID"],
        //                _serviceCallEntity.cs_servicecall_header.Columns["CustomerCode"],
        //                _serviceCallEntity.cs_servicecall_header.Columns["Doctype"],
        //                _serviceCallEntity.cs_servicecall_header.Columns["Fiscalyear"]
        //            };

        //        if (_serviceCallEntity.cs_servicecall_item.PrimaryKey.Length != 4)
        //            _serviceCallEntity.cs_servicecall_item.PrimaryKey = new DataColumn[] {
        //                _serviceCallEntity.cs_servicecall_item.Columns["SID"],
        //                _serviceCallEntity.cs_servicecall_item.Columns["CompanyCode"],
        //                _serviceCallEntity.cs_servicecall_item.Columns["xLineNo"],
        //                _serviceCallEntity.cs_servicecall_item.Columns["ObjectID"]
        //            };

        //        return _serviceCallEntity;
        //    }
        //    set
        //    {
        //        Session["ServicecallEntity" + idGen] = value;
        //        if (_serviceCallEntity != null)
        //        {
        //            _serviceCallEntity = value;
        //        }
        //    }
        //}


        //#region MyRegion

        //DataTable _dtGrouTemp;
        //DataTable dtGrouTemp
        //{
        //    get
        //    {
        //        if (_dtGrouTemp == null)
        //        {
        //            _dtGrouTemp = AfterSaleService.getInstance().GetDTProblemGroup(SID, businessObject);
        //        }
        //        return _dtGrouTemp;
        //    }
        //}

        //DataTable _dtSCType;
        //DataTable dtSCType
        //{
        //    get
        //    {
        //        if (_dtSCType == null)
        //        {

        //            List<string> listBusinessObject = new List<string>();
        //            if (Permission.IncidentView)
        //            {
        //                listBusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_INCIDENT);
        //            }
        //            if (Permission.ProblemView)
        //            {
        //                listBusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_PROBLEM);
        //            }
        //            if (Permission.RequestView)
        //            {
        //                listBusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_REQUEST);
        //            }
        //            if (Permission.ChangeOrderView)
        //            {
        //                listBusinessObject.Add(ServiceTicketLibrary.SERVICE_BUSINESS_OBJECT_CHANGE);
        //            }

        //            _dtSCType = AfterSaleService.getInstance().getSearchDoctype(
        //                "",
        //                CompanyCode,
        //                listBusinessObject
        //            );
        //        }

        //        return _dtSCType;
        //    }
        //}

        //DataTable _dtCustomer;
        //DataTable dtCustomer
        //{
        //    get
        //    {
        //        if (_dtCustomer == null)
        //        {
        //            _dtCustomer = AfterSaleService.getInstance().getSearchCustomerCode("", CompanyCode);
        //        }
        //        return _dtCustomer;
        //    }
        //}

        //DataTable _dtDocstatus;
        //DataTable dtDocstatus
        //{
        //    get
        //    {
        //        if (_dtDocstatus == null)
        //        {
        //            _dtDocstatus = AfterSaleService.getInstance().getCallStatus("");
        //        }
        //        return _dtDocstatus;
        //    }
        //}

        //DataTable _dtTicketDocStatus;
        //DataTable dtTicketDocStatus
        //{
        //    get
        //    {

        //        if (_dtTicketDocStatus == null)
        //        {
        //            _dtTicketDocStatus = lib.GetTicketDocStatus(SID, CompanyCode, false);
        //        }
        //        return _dtTicketDocStatus;
        //    }
        //}

        //DataTable _dtContactPerson;
        //DataTable dtContactPerson
        //{
        //    get
        //    {
        //        if (_dtContactPerson == null)
        //        {
        //            string custcode = CustomerCode_session == null ? "" : CustomerCode_session;
        //            if (mode_stage == ApplicationSession.CREATE_MODE_STRING)
        //            {
        //                _dtContactPerson = AfterSaleService.getInstance().getContactPerson(CompanyCode, custcode, "TRUE");
        //            }
        //            else
        //            {
        //                _dtContactPerson = AfterSaleService.getInstance().getContactPerson(CompanyCode, custcode);
        //            }
        //        }

        //        return _dtContactPerson;
        //    }
        //}

        //DataTable dtTier
        //{
        //    get { return Session["SCT_dtTier" + idGen] == null ? null : (DataTable)Session["SCT_dtTier" + idGen]; }
        //    set { Session["SCT_dtTier" + idGen] = value; }
        //}
        //#endregion

        //#region MyRegion

        //protected string GetEmployeeNameWithCode(string empcode)
        //{
        //    string _name = "";

        //    string sql = @"SELECT FirstName, LastName FROM master_employee WHERE SID = '" + SID + "' AND CompanyCode = '" + CompanyCode + @"' 
        //                   AND EmployeeCode = '" + empcode + "'";

        //    DataTable dt = dbService.selectDataFocusone(sql);

        //    if (dt.Rows.Count > 0)
        //    {
        //        return empcode + " : " + dt.Rows[0]["FirstName"] + " " + dt.Rows[0]["LastName"];
        //    }

        //    return _name;
        //}

        //private string GetSCTypeDesc(string code)
        //{
        //    DataRow[] drr = dtSCType.Select("DocumentTypeCode='" + code + "'");
        //    string desc = "";
        //    if (drr.Length > 0)
        //    {
        //        desc = drr[0]["Description"].ToString();
        //    }
        //    return desc;
        //}

        //#endregion

        //#region MyRegion

        //public string TicketNumber
        //{
        //    get { return serviceCallEntity.cs_servicecall_header.First().CallerID; }
        //}

        //public string CreatedBy
        //{
        //    get { return GetEmployeeNameWithCode(serviceCallEntity.cs_servicecall_header.First().CREATED_BY); }
        //}

        //public string DateTimeCreated
        //{
        //    get { return Validation.Convert2DateTimeDisplay(serviceCallEntity.cs_servicecall_header.First().CREATED_ON); }
        //}

        //public string Address
        //{
        //    get { return ""; }
        //}

        //public string TicketType
        //{
        //    get { return GetSCTypeDesc(serviceCallEntity.cs_servicecall_header.First().Doctype); ; }
        //}

        //public string OwnerService
        //{
        //    get
        //    {
        //        DataTable dtOwner = libconfig.GetMasterConfigOwnerGroup(SID, CompanyCode, "");
        //        foreach (DataRow dr in dtOwner.Rows)
        //        {
        //            if (dr["OwnerGroupCode"].ToString() == serviceCallEntity.cs_servicecall_item.First().QueueOption)
        //            {
        //                return dr["OwnerGroupName"].ToString();
        //            }
        //        }
        //        return serviceCallEntity.cs_servicecall_item.First().QueueOption;
        //    }
        //}

        //public string CustomerCode
        //{
        //    get { return serviceCallEntity.cs_servicecall_header.First().CustomerCode; }
        //}

        //public string ClientName
        //{
        //    get { return serviceCallEntity.cs_servicecall_header.First().CustomerDesc; }
        //}

        //public string ContactName
        //{
        //    get { return serviceCallEntity.cs_servicecall_header.First().ContractPersonName; }
        //}

        //public string ContactPhone
        //{
        //    get { return serviceCallEntity.cs_servicecall_header.First().ContractPersonTel; }
        //}

        //public string CallBackDateTime
        //{
        //    get { return Validation.Convert2DateTimeDisplay(serviceCallEntity.cs_servicecall_header.First().CallbackDate + serviceCallEntity.cs_servicecall_header.First().CallbackTime); }
        //}

        //public string SerialNo
        //{
        //    get { return serviceCallEntity.cs_servicecall_item.First().SerialNo; }
        //}

        //public string CIName
        //{
        //    get { return serviceCallEntity.cs_servicecall_item.First().EquipmentDesc; }
        //}

        //public string CICode
        //{
        //    get { return serviceCallEntity.cs_servicecall_item.First().EquipmentNo; }
        //}

        //public string Subject
        //{
        //    get { return serviceCallEntity.cs_servicecall_header.First().HeaderText; }
        //}

        //public string Description
        //{
        //    get { return serviceCallEntity.cs_servicecall_item.First().Remark; }
        //}

        ////++++++++=ADDRESS=++++++++
        //private ERPW.Lib.Master.CustomerService serviceCustomer = ERPW.Lib.Master.CustomerService.getInstance();

        //private CustomerProfile _CustomerProfile = null;
        //public CustomerProfile CustomerProfile
        //{
        //    get
        //    {
        //        if (_CustomerProfile == null)
        //        {
        //            _CustomerProfile = serviceCustomer.SearchCustomerDataByCustomerCode(
        //                SID,
        //                CompanyCode,
        //                CustomerCode
        //            );

        //            if (_CustomerProfile == null)
        //            {
        //                _CustomerProfile = new CustomerProfile();
        //            }
        //        }
        //        return _CustomerProfile;
        //    }
        //}
        //private void bindDataCustomerDetail()
        //{
        //    if (CustomerProfile != null)
        //    {
        //        //CustomerProfile.Address;              
        //    }
        //}
        ////=========APP Version==========
        //private tmpEquipmentSetupDataSet dsEquipment
        //{
        //    get
        //    {
        //        if (Session["EquipmentDetail.tmpEquipmentSetupDataSet.dsEquipment.form"] == null)
        //        {
        //            Session["EquipmentDetail.tmpEquipmentSetupDataSet.dsEquipment.form"] = new tmpEquipmentSetupDataSet();
        //        }
        //        return (tmpEquipmentSetupDataSet)Session["EquipmentDetail.tmpEquipmentSetupDataSet.dsEquipment.form"];
        //    }
        //    set
        //    {
        //        Session["EquipmentDetail.tmpEquipmentSetupDataSet.dsEquipment.form"] = value;
        //    }
        //}

        //private ICMUtils icmUtil = ERPW.Lib.F1WebService.F1WebService.getICMUtils();

        //SNAMasterService masterService = new SNAMasterService();

        //private void getproperties()
        //{
        //    DataTable dt = masterService.getConfigProperties(SID, "EQ");
        //    dt.Columns.Add("xValue");
        //    dt.Columns.Add("ObjectID");
        //    rptAttributes.DataSource = dt;
        //    rptAttributes.DataBind();
        //    //udpAttributes.Update();
        //}
        //private DataTable dtPropertiesSelect
        //{
        //    get
        //    {
        //        if (Session["properties_dtPropertiesSelect"] == null)
        //        {
        //            Session["properties_dtPropertiesSelect"] = CRMService.getInstance().getSelectedValue(SID, "*");
        //        }

        //        return (DataTable)Session["properties_dtPropertiesSelect"];
        //    }
        //    set { Session["properties_dtPropertiesSelect"] = value; }
        //}

        //private DataTable getdropdown(string selectedcode)
        //{
        //    DataTable dt = new DataTable();

        //    if (!string.IsNullOrEmpty(selectedcode))
        //    {
        //        DataRow[] drr = dtPropertiesSelect.Select("Code='" + selectedcode + "'");

        //        dt = dtPropertiesSelect.Clone();

        //        dt.Rows.Add("", "", "", "", "", "", "", "", "");

        //        foreach (DataRow dr in drr)
        //        {
        //            dt.ImportRow(dr);
        //        }
        //    }
        //    else
        //    {
        //        dt = dtPropertiesSelect.Copy();
        //    }

        //    return dt;
        //}

        //public Boolean isSelectedValue(Object obj)
        //{
        //    if (obj != null)
        //    {
        //        string type = obj.ToString();
        //        return type != "";
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}


        //private void BindDataEquipment()
        //{
        //    Object[] objParam = new Object[] { "0800044", POSDocumentHelper.getSessionId(SID, UserName),SID
        //        , UserName,CompanyCode,CICode };
        //    DataSet[] objDataSetChange = new DataSet[] { dsEquipment };
        //    DataSet objReturn = icmUtil.ICMDataSetInvoke(objParam, objDataSetChange);

        //    if (objReturn != null && objReturn.Tables.Count > 0)
        //    {
        //        dsEquipment = new tmpEquipmentSetupDataSet();
        //        dsEquipment.Merge(objReturn);
        //        dsEquipment.AcceptChanges();
        //    }

        //    //dsEquipment.master_equipment_properties.DefaultView.Sort = "PropertiesCode ASC";

        //    rptAttributes.DataSource = dsEquipment.master_equipment_properties;
        //    rptAttributes.DataBind();
        //    //udpAttributes.Update();
        //    //SelectionCIFamily_Change(null, EventArgs.Empty);

        //}

        ////=======

        //public string OwnerServiceMethod()
        //{
        //    string NoOwnerService = ServiceTicketLibrary.LookUpTable(
        //        "EquipmentObjectType",
        //        "master_equipment_general",
        //        "where SID = '" + SID + "' and CompanyCode = '" + CompanyCode + "' and EquipmentCode = '" + CICode + "'"
        //    );

        //    string NameOwnerService = ServiceTicketLibrary.LookUpTable(
        //        "OwnerGroupName",
        //        "ERPW_OWNER_GROUP",
        //        "where SID = '" + SID + "' and CompanyCode = '" + CompanyCode + "' and OwnerGroupCode = '" + NoOwnerService + "'"
        //    );
        //    return NameOwnerService;
        //}
        //public string TIDMethod()
        //{
        //    string NoTID = ServiceTicketLibrary.LookUpTable(
        //        "ChangeCurrency",
        //        "master_customer",
        //        "where SID = '" + SID + "' and CompanyCode = '" + CompanyCode + "' and CustomerCode = '" + CustomerCode + "'"
        //    );

        //    return NoTID;
        //}

        //public string MIDMethod()
        //{
        //    string NoMID = ServiceTicketLibrary.LookUpTable(
        //        "FederalTaxID",
        //        "master_customer",
        //        "where SID = '" + SID + "' and CompanyCode = '" + CompanyCode + "' and CustomerCode = '" + CustomerCode + "'"
        //    );

        //    return NoMID;
        //}

        //public string OwnerServiceCI
        //{
        //    get { return OwnerServiceMethod(); }
        //}

        ////public string Mid
        ////{
        ////    get { return MIDMethod(); }
        ////}

        ////public string Tid
        ////{
        ////    get { return TIDMethod(); }
        ////}
        //#endregion

        //private void GetcontactDetailForScreen()
        //{
        //    string bobjectid = "555INETC10045001";
        //    ERPW.Lib.Master.ContactEntity en = new ERPW.Lib.Master.ContactEntity();
        //    if (!string.IsNullOrEmpty(bobjectid))
        //    {
        //        List<ERPW.Lib.Master.ContactEntity> listen = ERPW.Lib.Master.CustomerService.getInstance().getListContactRefCustomer(
        //        SID, CompanyCode
        //        , CustomerCode
        //        , ""
        //        , bobjectid);
        //        if (listen.Count > 0)
        //        {
        //            en = listen[0];
        //        }
        //    }

        //    //txtContactPhonePrint.Text = en.phone.Trim();
        //    //txtContactEmail.Text = en.email.Trim();
        //    //txtContactRemark.Text = en.REMARK.Trim();

        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            //bindDataCustomerDetail();
            //BindDataEquipment();







            ///test///
            //getproperties();
            //GetcontactDetailForScreen();
            //string NoOwnerService = ServiceTicketLibrary.LookUpTable(
            //    "EquipmentObjectType",
            //    "master_equipment_general",
            //    "where SID = '"+ SID + "' and CompanyCode = '" + CompanyCode + "' and EquipmentCode = '" + CICode + "'"
            //);

            //string NameOwnerService = ServiceTicketLibrary.LookUpTable(
            //    "OwnerGroupName",
            //    "ERPW_OWNER_GROUP",
            //    "where SID = '" + SID + "' and CompanyCode = '" + CompanyCode + "' and OwnerGroupCode = '" + NoOwnerService + "'"
            //);
            ///test///
        }
    }
}