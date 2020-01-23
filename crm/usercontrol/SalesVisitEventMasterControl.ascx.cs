using ServiceWeb.widget.usercontrol;
using ERPW.Lib.Authentication;
using SNA.Lib.crm;
using SNA.Lib.crm.entity;
using SNA.Lib.crm.entity.salevisit;
using SNA.Lib.crm.SaleVisitMaster;
using SNA.Lib.Initiative;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POSWeb.crm.usercontrol
{
    public partial class SalesVisitEventMasterControl : System.Web.UI.UserControl
    {
        private CRMService serviceCRM = CRMService.getInstance();
        private EventMappingMasterService serviceEvent = EventMappingMasterService.getInstance();
        private EventMasterService serviceEventLst = EventMasterService.getInstance();
        private SaleVisitAccountabilityService accountService = new SaleVisitAccountabilityService();
        private F1LinkReference.F1LinkReference lc_lib = new F1LinkReference.F1LinkReference();

        private string _SID;
        public string SID
        {
            get
            {
                return _SID == null ? _SID = ERPWAuthentication.SID : _SID;
            }
        }

        private string _CompanyCode;
        public string CompanyCode
        {
            get
            {
                return _CompanyCode == null ? _CompanyCode = ERPWAuthentication.CompanyCode : _CompanyCode;
            }
        }

        private List<ContactCustomer> customerlst { get; set; }
        private DataTable dtOwner { get; set; }
        private DataTable dtAccounts { get; set; }

        public string CustomerCode
        {
            get
            {
                return hddCustomerCode.Value;
            }
            set
            {
                hddCustomerCode.Value = value;
                udpCode.Update();
            }
        }
        public string CustomerName
        {
            get
            {
                return hddCustomerName.Value;
            }
            set
            {
                hddCustomerName.Value = value;
                udpCode.Update();
            }
        }
        List<SaleVisitMasterModel.EventMapping> lstMaster = new List<SaleVisitMasterModel.EventMapping>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getEventList();
            }
        }

        private List<EventMasterModel> GetMasterEvent()
        {
            List<EventMasterModel> lstEvent = serviceEventLst.GetMasterEvent();
            return lstEvent;
        }

        private DataTable AccountAbility()
        {
            //      DataTable dt = accountService.getAccountabilityStructureV2(
            //    ERPWAuthentication.SID,
            //    "PEAK-POC", //WorkGroupCode
            //    ERPWAuthentication.EmployeeCode  //EmployeeCode
            //);
            //**************HARD CDOE*********************************
            DataTable dt = accountService.getAccountabilityStructureV2(
                ERPWAuthentication.SID,
                "PEAK-POC", //WorkGroupCode
                ERPWAuthentication.EmployeeCode  //EmployeeCode
            );
            dtAccounts = dt;
            return dt;
        }
        private DataTable OwnerLinkID()
        {
            DataTable dt = new DataTable();
            dt = lc_lib.GetAllEmployeeList(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
            dtOwner = dt;
            return dt;
        }
        private List<EventMasterModel> getEventMasterModel()
        {
            List<EventMasterModel> lstEvent = new List<EventMasterModel>();
            List<string> listOldEvent = lstMaster.Select(s => s.EVENT_CODE).ToList();
            lstEvent = GetMasterEvent();
            lstEvent = lstEvent.Where(w => !listOldEvent.Contains(w.EventCode)).ToList();
            lstEvent.ForEach(x => { x.EventName = x.EventCode.ToString() + " " + x.EventName.ToString(); });
            return lstEvent;
        }

        private void bindDropdown(List<SaleVisitMasterModel.EventMapping> lstEventMapping)
        {
            try
            {
                ClientService.AGLoading(false);
                List<ContactCustomer> Customerlst = new List<ContactCustomer>();
                DataTable dtOwner = new DataTable();
                DataTable dtaccount = new DataTable();
                List<EventMasterModel> lstEvent = getEventMasterModel();

                //List<string> listOldEvent = lstMaster.Select(s => s.EVENT_CODE).ToList();
                //lstEvent = GetMasterEvent();
                //lstEvent = lstEvent.Where(w => !listOldEvent.Contains(w.EventCode)).ToList();
                //lstEvent.ForEach(x => { x.EventName = x.EventCode.ToString() + " " + x.EventName.ToString(); });

                ddlEventCode.DataValueField = "EventCode";
                ddlEventCode.DataTextField = "EventName";
                ddlEventCode.DataSource = lstEvent;
                ddlEventCode.DataBind();
                ddlEventCode.Items.Insert(0, new ListItem("-- เลือก --", ""));

                dtaccount = AccountAbility();
                ddlAccountAbility.DataSource = dtaccount;
                ddlAccountAbility.DataTextField = "DataText";
                ddlAccountAbility.DataValueField = "DataValue";
                ddlAccountAbility.DataBind();
                ddlAccountAbility.Items.Insert(0, new ListItem("-- เลือก --", ""));

                dtOwner = OwnerLinkID();
                dtOwner.Columns.Add("FullName");
                foreach (DataRow p in dtOwner.Rows)
                {
                    p["FullName"] = p["FirstName_TH"].ToString() + " " + p["LastName_TH"].ToString();
                }
                AutoCompleteOwnerControl.initialDataAutoComplete(dtOwner, "LinkId", "FullName");
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
        private void getEventList()
        {
            try
            {
                lstMaster = serviceEvent.GetEventList(ERPWAuthentication.SID, CustomerCode);
                bindDropdown(lstMaster);

                foreach (var p in lstMaster)
                {
                    p._CUSTOMER_CODE = CustomerName;

                    var rowsLink = dtOwner.Select("LinkId = '" + p.OWNER_LINKID + "'").ToList();
                    if (rowsLink.Count > 0)
                    {
                        p._OWNER_LINKID = rowsLink[0][13].ToString();
                    }
                    else 
                    { 
                        p._OWNER_LINKID = null;
                    }

                    var rowsAccount = dtAccounts.Select("DataValue = '" + p.ACCOUNTABILITY + "'").ToList();
                    if (rowsAccount.Count > 0)
                    {
                        p._ACCOUNTABILITY = rowsAccount[0][0].ToString();
                    }
                    else
                    { 
                        p._ACCOUNTABILITY = null; 
                    }
                }
                List<EventMasterModel> lstEvent = getEventMasterModel();
                List<string> listEventIsMap = lstMaster.Select(s => s.EVENT_CODE).ToList();
                foreach (EventMasterModel en in lstEvent)
                {
                    if (listEventIsMap.Contains(en.EventCode))
                    {
                        continue;
                    }
                    lstMaster.Add(new SaleVisitMasterModel.EventMapping
                    {
                        EVENT_CODE = en.EventCode,
                        EventName = en.EventName,
                        _CUSTOMER_CODE = CustomerName,
                        CUSTOMER_CODE = CustomerCode,
                        _ACCOUNTABILITY = "",
                        ACCOUNTABILITY = "",
                        _OWNER_LINKID = "",
                        OWNER_LINKID = "",
                        IsNew = true,
                        flag = false,
                        SID = SID
                    });
                }

                rptItemsMaster.DataSource = lstMaster;
                rptItemsMaster.DataBind();
                udpTable.Update();
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }
        protected void rptItemsMaster_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            listEmployeeActivityCC = new List<EmployeeActivityCC>();

            string AccountabilityCode = DataBinder.Eval(e.Item.DataItem, "ACCOUNTABILITY").ToString();
            string OwnerInkID = DataBinder.Eval(e.Item.DataItem, "OWNER_LINKID").ToString();

            Repeater rptInitiativeOwner = (Repeater)e.Item.FindControl("rptInitiativeOwner");
            Repeater rptStructureParticipant = (Repeater)e.Item.FindControl("rptStructureParticipant");
            //Repeater rptParticipants = (Repeater)e.Item.FindControl("rptParticipants");
            bindInitiativeOwner(rptInitiativeOwner, AccountabilityCode, OwnerInkID);
            bindParticipants(rptStructureParticipant, AccountabilityCode);
            
            DropDownList ddlAccountItm = (DropDownList)e.Item.FindControl("ddlAccountItm");
            ddlAccountItm.DataTextField = "DataText";
            ddlAccountItm.DataValueField = "DataValue";
            ddlAccountItm.DataSource = dtAccounts;
            ddlAccountItm.DataBind();
            ddlAccountItm.Items.Insert(0, new ListItem("-- เลือก --", ""));
            ddlAccountItm.SelectedValue = AccountabilityCode != null ? AccountabilityCode : "";

            AutoCompleteControl searchOwner = e.Item.FindControl("ddlOwnerItm") as AutoCompleteControl;
            searchOwner.initialDataAutoComplete(dtOwner, "LinkId", "FullName");
            if (!string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "OWNER_LINKID").ToString()))
            {
                searchOwner.SetValue = DataBinder.Eval(e.Item.DataItem, "OWNER_LINKID").ToString();
            }
        }
        protected void btnUpdateItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaleVisitMasterModel.EventMapping models = new SaleVisitMasterModel.EventMapping();

                Button btn = (Button)sender;
                RepeaterItem repItem = (RepeaterItem)btn.NamingContainer;
                string Code = (sender as Button).CommandArgument;
                string EVENT_CODE = (repItem.FindControl("hhdEVENT_CODE") as HiddenField).Value;
                string CUSTOMER_CODE = (repItem.FindControl("hhdCUSTOMER_CODE") as HiddenField).Value;
                AutoCompleteControl searchOwner = repItem.FindControl("ddlOwnerItm") as AutoCompleteControl;
                string ACCOUNTABILITY = (repItem.FindControl("ddlAccountItm") as DropDownList).SelectedValue;
                string OWNER_LINKID = searchOwner.SelectValue;
                bool IsActive = !(repItem.FindControl("chkActive") as CheckBox).Checked;

                if (String.IsNullOrEmpty(OWNER_LINKID))
                {
                    throw new Exception("กรุณาข้อมูล.");
                }
                if (String.IsNullOrEmpty(ACCOUNTABILITY))
                {
                    throw new Exception("กรุณาข้อมูล.");
                }

                if (Convert.ToBoolean(btn.CommandName))
                {
                    models.SID = ERPWAuthentication.SID;
                    models.EVENT_CODE = EVENT_CODE;
                    models.ACCOUNTABILITY = ACCOUNTABILITY;
                    models.CUSTOMER_CODE = CUSTOMER_CODE;
                    models.OWNER_LINKID = OWNER_LINKID;

                    serviceEvent.CreateEventMaster(models);
                    serviceEvent.UpdateFlag(EVENT_CODE, CustomerCode, IsActive.ToString());
                }
                else
                {

                    models.SID = ERPWAuthentication.SID;
                    models.EVENT_CODE = EVENT_CODE;
                    models.ACCOUNTABILITY = ACCOUNTABILITY;
                    models.CUSTOMER_CODE = CUSTOMER_CODE;
                    models.OWNER_LINKID = OWNER_LINKID;

                    serviceEvent.UpdateEventMaster(models);
                    serviceEvent.UpdateFlag(EVENT_CODE, CustomerCode, IsActive.ToString());
                }

                getEventList();
                ClientService.AGSuccess("อัพเดทข้อมูลสำเร็จ");
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
        protected void btnsaveMaster_Click(object sender, EventArgs e)
        {
            try
            {
                SaleVisitMasterModel.EventMapping models = new SaleVisitMasterModel.EventMapping();

                if (String.IsNullOrEmpty(ddlEventCode.SelectedValue))
                {
                    throw new Exception("กรุณากรอกข้อมูล.");
                }
                if (String.IsNullOrEmpty(ddlAccountAbility.SelectedValue))
                {
                    throw new Exception("กรุณากรอกข้อมูล.");
                }
                if (String.IsNullOrEmpty(AutoCompleteOwnerControl.SelectValue))
                {
                    throw new Exception("กรุณากรอกข้อมูล.");
                }
                if (String.IsNullOrEmpty(CustomerCode))
                {
                    throw new Exception("กรุณากรอกข้อมูล.");
                }
                models.SID = ERPWAuthentication.SID;
                models.EVENT_CODE = ddlEventCode.SelectedValue;
                models.ACCOUNTABILITY = ddlAccountAbility.SelectedValue;
                models.CUSTOMER_CODE = CustomerCode;
                models.OWNER_LINKID = AutoCompleteOwnerControl.SelectValue;
                serviceEvent.CreateEventMaster(models);
                AutoCompleteOwnerControl.SetValue = "";
                udpAddNewItem.Update();
                getEventList();
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
        protected void btnUpdateFlag_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                RepeaterItem repItem = (RepeaterItem)btn.NamingContainer;
                HiddenField hhdEVENT_CODE = (repItem.FindControl("hhdEVENT_CODE") as HiddenField);
                HiddenField hhdCUSTOMER_CODE = (repItem.FindControl("hhdCUSTOMER_CODE") as HiddenField);
                string flag = (sender as Button).CommandArgument;
                serviceEvent.UpdateFlag(hhdEVENT_CODE.Value, hhdCUSTOMER_CODE.Value, flag);
                getEventList();
                ClientService.AGSuccess("แก้ไขข้อมูลสำเร็จ");
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


        #region Accountibility
        CRMCallCardService callCardService = new CRMCallCardService();
        AccountabilityService initAccountService = new AccountabilityService();

        public int RuntimeRepeaterItemCount = 0;
        private string WorkGroupCode = "PEAK-POC";

        private List<EmployeeActivityCC> _listEmployeeActivityCC;
        private List<EmployeeActivityCC> listEmployeeActivityCC
        {
            get
            {
                if (_listEmployeeActivityCC == null)
                {
                    _listEmployeeActivityCC = new List<EmployeeActivityCC>();
                }
                return _listEmployeeActivityCC as List<EmployeeActivityCC>;
            }
            set
            {
                _listEmployeeActivityCC = value;
            }
        }

        private void bindInitiativeOwner(Repeater rpt, string AccountabilityCode, string OwnerLinkID)
        {
            DataTable dtOwn = initAccountService.getDataInitiativeOwnerWithInheritNode(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                AccountabilityCode,
                WorkGroupCode
            );

            rpt.DataSource = dtOwn;
            rpt.DataBind();
        }

        private void bindParticipants(Repeater rpt, string WorkStructureCode)
        {
            DataTable dtParticipants = initAccountService.getWorkStructureParticipant(
                "",
                ERPWAuthentication.SID,
                WorkGroupCode,
                WorkStructureCode
            );

            rpt.DataSource = dtParticipants;
            rpt.DataBind();
        }

        protected void rptStructureParticipant_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rptStructureParticipantRole = e.Item.FindControl("rptStructureParticipantRole") as Repeater;
            HiddenField hddStructureCode = e.Item.FindControl("hddStructureCode") as HiddenField;

            DataTable dt = InitiativeService.getInstance().getHierarchyParticipant(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, WorkGroupCode, hddStructureCode.Value);
            foreach (DataRow dr in dt.Rows)
            {
                if (listEmployeeActivityCC.Where(w => w.EmployeeCode.Equals(dr["EmployeeCode"].ToString())).ToList().Count == 0)
                {
                    listEmployeeActivityCC.Add(new EmployeeActivityCC
                    {
                        EmployeeCode = dr["EmployeeCode"].ToString(),
                        LinkID = dr["LINKID"].ToString()
                    });
                }
            }
            RuntimeRepeaterItemCount = dt.Rows.Count;
            rptStructureParticipantRole.DataSource = dt;
            rptStructureParticipantRole.DataBind();
        }

        public class EmployeeActivityCC
        {
            public string EmployeeCode { get; set; }
            public string LinkID { get; set; }
        }
        #endregion
    }
}