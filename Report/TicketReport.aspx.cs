using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Service;
using Newtonsoft.Json;
using ServiceWeb.auth;
using ServiceWeb.crm.AfterSale;
using ServiceWeb.Service;
using ServiceWeb.UserControl.AutoComplete;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.Report
{
    public partial class TicketReport : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return /*ERPWAuthentication.Permission.ReportView ||*/ ERPWAuthentication.Permission.AllPermission || ERPWAuthentication.Permission.ReportTicketReport;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.ReportModify || ERPWAuthentication.Permission.AllPermission;
        }

        private ERPW.Lib.Service.Report.ReportDAO report = new ERPW.Lib.Service.Report.ReportDAO();
        private ServiceTicketLibrary lib = new ServiceTicketLibrary();
        private ReportUnity report_unity = new ReportUnity();
        private UniversalService universalService = new UniversalService();
        AutoCompleteEquipment AutoEquipmentSearch = new AutoCompleteEquipment();
        AutoCompleteCustomer AutoCustomerSearch = new AutoCompleteCustomer();

        List<IncidentAreaEnrity> _mListIncidentArea;
        List<IncidentAreaEnrity> mListIncidentArea
        {
            get
            {
                if (_mListIncidentArea == null)
                {
                    DataTable dt = MasterConfigLibrary.GetInstance().getIncidentAreaRawData(ERPWAuthentication.SID);
                    _mListIncidentArea = JsonConvert.DeserializeObject<List<IncidentAreaEnrity>>(JsonConvert.SerializeObject(dt));
                }
                return _mListIncidentArea;
            }
        }

        DataTable dtPriority
        {
            get { return Session["ServiceCallCriteria.SCT_dtPriority"] == null ? null : (DataTable)Session["ServiceCallCriteria.SCT_dtPriority"]; }
            set { Session["ServiceCallCriteria.SCT_dtPriority"] = value; }

        }

        //DataTable ticketReport
        //{
        //    get { return Session["TicketReport.Report"] == null ? null : (DataTable)Session["TicketReport.Report"]; }
        //    set { Session["TicketReport.Report"] = value; }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                get_ticket_type();
                GetTicketStatus();
                GetImpact();
                GetUrgency();
                GetPriority();

                setDefaulsearchPageLoad();
                setDefaultIncidentArea();

                bindOwnerService();
            }
        }

        private void bindOwnerService()
        {

            bool FilterOwner = false;
            bool.TryParse(WebConfigurationManager.AppSettings["ERPW_Config_Filter_Owner"], out FilterOwner);

            if (FilterOwner && !ERPWAuthentication.Permission.AllPermission)
            {
                ddlOwnerGroup.Items.Clear();
                ddlOwnerGroup.Items.Insert(0,
                    new ListItem(
                        ERPWAuthentication.Permission.OwnerGroupName,
                        ERPWAuthentication.Permission.OwnerGroupCode
                    )
                );
                ddlOwnerGroup.Enabled = false;
                ddlOwnerGroup.CssClass = "form-control form-control-sm";
            }
            else
            {
                DataTable dtOwnerGroup = MasterConfigLibrary.GetInstance().GetMasterConfigOwnerGroup(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "");
                ddlOwnerGroup.DataSource = dtOwnerGroup;
                ddlOwnerGroup.DataBind();
                ddlOwnerGroup.Items.Insert(0, new ListItem("", ""));
                ddlOwnerGroup.Enabled = true;
            }
        }

        protected void ui_search_button_Click(object sender, EventArgs e)
        {
            try
            {

                string configuration_no = AutoCompleteEquipment.SelectedValue;
                if (string.IsNullOrEmpty(AutoCompleteEquipment.SelectedDisplay))
                {
                    configuration_no = "";
                }
                string ticket_type = ui_ticket_type.Text;
                string customer_code = AutoCompleteCustomer.SelectedValue;
                if (string.IsNullOrEmpty(AutoCompleteCustomer.SelectedDisplay))
                {
                    customer_code = "";
                }
                string document_status_code = ui_document_status.SelectedValue;
                string ticket_status_code = ui_ticket_status.SelectedValue;
                
                string date_from = "";
                string date_to = "";

                if (!string.IsNullOrEmpty(ui_opendate_from.Text))
                    date_from = Agape.FocusOne.Utilities.Validation.Convert2DateDB(ui_opendate_from.Text);
                if (!string.IsNullOrEmpty(ui_opendate_from.Text))
                    date_to = Agape.FocusOne.Utilities.Validation.Convert2DateDB(ui_opendate_to.Text);

                string impact_code = ui_impact_code.SelectedValue;
                string urgency_code = ui_urgency_code.SelectedValue;
                string priority_code = ui_priority_code.SelectedValue;
                //System.Diagnostics.Debug.WriteLine(impact_code + "\n" + urgency_code + "\n" + priority_code);
                string incident_group = txtProblemGroup.SelectValue;
                string incident_type = txtProblemType.SelectValue;
                string incident_source = txtProblemSource.SelectValue;

                string contract_source = txtContactSource.SelectValue;
                string owner_service_code = "";// ui_owner_service_code.SelectedValue;

                string created_by = AutoCompleteEmployee.SelectedValue;
                if (string.IsNullOrEmpty(AutoCompleteEmployee.SelectedDisplay))
                {
                    created_by = "";
                }

                //DataTable datatable = new DataTable();
                DataTable ticketReport = report.ticket_report_v2(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    configuration_no,
                    ticket_type,
                    customer_code,
                    document_status_code,
                    ticket_status_code,
                    date_from,
                    date_to,
                    impact_code,
                    urgency_code,
                    priority_code,
                    incident_group,
                    incident_type,
                    incident_source,
                    contract_source,
                    owner_service_code,
                    created_by,
                    ddlOwnerGroup.SelectedValue,
                    txtResponsibleOrganization.Text,
                    txtContactEmail.Text,
                    txtDescription.Text
                ).toDataTable();

                ticketReport = report_unity.incidentNoFormater(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    ticketReport,
                    "IncidentNO"
                    );

                ticketReport = report_unity.ticketreport_calculate_timeV2(ticketReport);

                foreach (DataRow dr in ticketReport.Rows)
                {
                    dr["Subject"] = ReplaceHexadecimalSymbols(dr["Subject"].ToString());
                    dr["Description"] = ReplaceHexadecimalSymbols(dr["Description"].ToString());
                    dr["Close_Log"] = ReplaceHexadecimalSymbols(dr["Close_Log"].ToString());
                }

                divDataJson.InnerHtml = JsonConvert.SerializeObject(ticketReport).Replace("<", "&lt;").Replace(">", "&gt;");

                //rptSearchSale.DataSource = ticketReport;
                //rptSearchSale.DataBind();
                //ClientService.DoJavascript("$('#tableItems').DataTable();");
                ClientService.DoJavascript("afterSearch();");
                udpnItems.Update();

                //ExportToExcel(ticketReport);
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

        static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }

        private void get_ticket_type()
        {
            //ui_ticket_type.Items.Clear();
            DataTable dt = AfterSaleService.getInstance().getSearchDoctype("", ERPWAuthentication.CompanyCode, false, false);
            ui_ticket_type.Items.Add(new ListItem("", ""));
            ui_ticket_type.AppendDataBoundItems = true;
            ui_ticket_type.DataTextField = "Description";
            ui_ticket_type.DataValueField = "DocumentTypeCode";
            ui_ticket_type.DataSource = dt;
            ui_ticket_type.DataBind();
        }

        private void GetTicketStatus()
        {
            DataTable dt = lib.GetTicketDocStatus(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, false);
            ui_ticket_status.DataTextField = "DocumentStatusDesc";
            ui_ticket_status.DataValueField = "DocumentStatus";
            ui_ticket_status.DataSource = dt;
            ui_ticket_status.DataBind();
            ui_ticket_status.Items.Insert(0, new ListItem("", ""));
        }

        private void GetImpact()
        {
            DataTable dt = lib.GetImpactMaster(ERPWAuthentication.SID);
            ui_impact_code.DataTextField = "ImpactName";
            ui_impact_code.DataValueField = "ImpactCode";
            ui_impact_code.DataSource = dt;
            ui_impact_code.DataBind();
            ui_impact_code.Items.Insert(0, new ListItem("", ""));
        }

        private void GetUrgency()
        {
            DataTable dt = lib.GetUrgencyMaster(ERPWAuthentication.SID);
            ui_urgency_code.DataTextField = "UrgencyName";
            ui_urgency_code.DataValueField = "UrgencyCode";
            ui_urgency_code.DataSource = dt;
            ui_urgency_code.DataBind();
            ui_urgency_code.Items.Insert(0, new ListItem("", ""));
        }

        private void GetPriority()
        {
            DataTable dt = lib.GetPriorityMaster(ERPWAuthentication.SID);
            ui_priority_code.DataTextField = "Description";
            ui_priority_code.DataValueField = "PriorityCode";
            ui_priority_code.DataSource = dt;
            ui_priority_code.DataBind();
            ui_priority_code.Items.Insert(0, new ListItem("", ""));
        }

        private void setDefaultIncidentArea(string Event = "")
        {

            bool isGroup = true; bool isType = true; bool isSource = true; bool isArea = true;
            string sGroup = txtProblemGroup.SelectValue;
            string sType = txtProblemType.SelectValue;
            string sSource = txtProblemSource.SelectValue;
            List<IncidentAreaEnrity> listTemp = JsonConvert.DeserializeObject<List<IncidentAreaEnrity>>(JsonConvert.SerializeObject(mListIncidentArea));

            #region Swich mode Fillter
            if (("INCIDENT_GROUP").Equals(Event))
            {
                isGroup = false;
            }
            else if (("INCIDENT_TYPE").Equals(Event))
            {
                isGroup = false;
                isType = false;
            }
            else if (("INCIDENT_SOURCE").Equals(Event))
            {
                isGroup = false;
                isType = false;
                isSource = false;
            }
            #endregion

            #region Where Condation
            if (!string.IsNullOrEmpty(sGroup) && !isGroup)
            {
                listTemp = listTemp.FindAll(x => x.GROUPCODE == sGroup);
            }
            if (!string.IsNullOrEmpty(sType) && !isType)
            {
                listTemp = listTemp.FindAll(x => x.GTCODE == sType);
            }
            if (!string.IsNullOrEmpty(sSource) && !isSource)
            {
                listTemp = listTemp.FindAll(x => x.GTSCODE == sSource);
            }
            #endregion


            #region Perpare Data to Screen
            DataTable dtGroup = new DataTable();
            DataTable dtType = new DataTable();
            DataTable dtSource = new DataTable();
            DataTable dtContact = new DataTable();
            if (listTemp.Count > 0)
            {
                if (isGroup)
                {
                    var group = listTemp
                        .Select(x => new { x.GROUPCODE, x.GROUPNAME })
                        .Distinct()
                        .ToList()
                        .FindAll(y => !string.IsNullOrEmpty(y.GROUPNAME));

                    dtGroup = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(group));
                    group = null;
                }
                if (isType)
                {
                    var listtype = listTemp
                        .Select(x => new { x.GTCODE, x.TYPENAME })
                        .Distinct()
                        .ToList()
                        .FindAll(y => !string.IsNullOrEmpty(y.TYPENAME));
                    dtType = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(listtype));

                    foreach (DataRow dr in dtType.Rows)
                    {
                        dr["GTCODE"] = dr["GTCODE"].ToString().Substring(2, 2);
                    }

                    listtype = null;
                }
                if (isSource)
                {
                    var listSource = listTemp
                        .Select(x => new { x.GTSCODE, x.SOURCENAME })
                        .Distinct()
                        .ToList()
                        .FindAll(y => !string.IsNullOrEmpty(y.SOURCENAME));

                    dtSource = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(listSource));

                    foreach (DataRow dr in dtSource.Rows)
                    {
                        dr["GTSCODE"] = dr["GTSCODE"].ToString().Substring(4, 2);
                    }

                    listSource = null;
                }

                if (isArea)
                {
                    var listContact = listTemp
                        .Select(x => new { x.AREACODE, x.AREANAME })
                        .Distinct()
                        .ToList()
                        .FindAll(y => !string.IsNullOrEmpty(y.AREANAME));

                    dtContact = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(listContact));
                    listContact = null;
                }
                listTemp = null;
            }
            if (isGroup)
            {
                txtProblemGroup.initialDataAutoComplete(dtGroup, "GROUPCODE", "GROUPNAME", true);
            }
            if (isType)
            {
                txtProblemType.initialDataAutoComplete(dtType, "GTCODE", "TYPENAME", true);
            }
            if (isSource)
            {
                txtProblemSource.initialDataAutoComplete(dtSource, "GTSCODE", "SOURCENAME", true);
            }
            if (isArea)
            {
                txtContactSource.initialDataAutoComplete(dtContact, "AREACODE", "AREANAME", true);
            }
            #endregion
        }

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

        protected void btnIncidentAreaFilter_Click(object sender, EventArgs e)
        {
            try
            {
                setDefaultIncidentArea(hhdModeEventFilter.Value.Trim());
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
                DataTable dt = universalService.GetEquipmentCustomerAssignment(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "", customerCode);

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
                DataTable dt = universalService.GetEquipmentCustomerAssignment(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, equipmentCode, "");

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
                dt = AfterSaleService.getInstance().getContactPerson(ERPWAuthentication.CompanyCode, custcode);
            }

            //_ddl_contact_person_search.initialDataAutoComplete(dt, "BOBJECTLINK", "NAME1", false, "email", "phone", "remark");
            //GetcontactDetailForScreen_search();
            //updContactCus.Update();
        }

        private void setDefaulsearchPageLoad()
        {
            //_ddl_contact_person_search.initialDataAutoComplete(new DataTable(), "", "");
            txtProblemGroup.initialDataAutoComplete(new DataTable(), "", "");
            txtProblemType.initialDataAutoComplete(new DataTable(), "", "");
            txtProblemSource.initialDataAutoComplete(new DataTable(), "", "");
            txtContactSource.initialDataAutoComplete(new DataTable(), "", "");
        }

        //export excel
        private CustomerService serviceCRM = CustomerService.getInstance();

        private void ExampleRepeater()
        {
            string configuration_no = AutoCompleteEquipment.SelectedValue;
            if (string.IsNullOrEmpty(AutoCompleteEquipment.SelectedDisplay))
            {
                configuration_no = "";
            }
            string ticket_type = ui_ticket_type.Text;
            string customer_code = AutoCompleteCustomer.SelectedValue;
            if (string.IsNullOrEmpty(AutoCompleteCustomer.SelectedDisplay))
            {
                customer_code = "";
            }
            string document_status_code = ui_document_status.SelectedValue;
            string ticket_status_code = ui_ticket_status.SelectedValue;

            string date_from = "";
            string date_to = "";

            if (!string.IsNullOrEmpty(ui_opendate_from.Text))
                date_from = Agape.FocusOne.Utilities.Validation.Convert2DateDB(ui_opendate_from.Text);
            if (!string.IsNullOrEmpty(ui_opendate_from.Text))
                date_to = Agape.FocusOne.Utilities.Validation.Convert2DateDB(ui_opendate_to.Text);

            string impact_code = ui_impact_code.SelectedValue;
            string urgency_code = ui_urgency_code.SelectedValue;
            string priority_code = ui_priority_code.SelectedValue;
            string incident_group = txtProblemGroup.SelectValue;
            string incident_type = txtProblemType.SelectValue;
            string incident_source = txtProblemSource.SelectValue;

            string contract_source = txtContactSource.SelectValue;
            string owner_service_code = "";// ui_owner_service_code.SelectedValue;

            string created_by = AutoCompleteEmployee.SelectedValue;
            if (string.IsNullOrEmpty(AutoCompleteEmployee.SelectedDisplay))
            {
                created_by = "";
            }

            //DataTable dt = dtCustomer.toDataTable();
            DataTable ticketReport = report.ticket_report_v2(
                   ERPWAuthentication.SID,
                   ERPWAuthentication.CompanyCode,
                   configuration_no,
                   ticket_type,
                   customer_code,
                   document_status_code,
                   ticket_status_code,
                   date_from,
                   date_to,
                   impact_code,
                   urgency_code,
                   priority_code,
                   incident_group,
                   incident_type,
                   incident_source,
                   contract_source,
                   owner_service_code,
                   created_by,
                   ddlOwnerGroup.SelectedValue,
                    txtResponsibleOrganization.Text,
                    txtContactEmail.Text,
                    txtDescription.Text
               ).toDataTable();

            ticketReport = report_unity.incidentNoFormater(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                ticketReport,
                "IncidentNO"
                );

            ticketReport = report_unity.ticketreport_calculate_timeV2(ticketReport);

            Session["report.excel.Report_Excel_Export_Datatable"] = ticketReport;
            Session["report.excel.Report_Excel_Export_Name"] = "Ticket Report";
            ClientService.DoJavascript("exportExcelAPI();");
        }

        protected void ui_export_button_Click(object sender, EventArgs e)
        {
            try
            {
                ExampleRepeater();
                System.Diagnostics.Debug.WriteLine("export success");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("export fail: " + ex);
                throw;
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }
    }
}