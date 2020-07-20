using Agape.FocusOne.Utilities;
using ServiceWeb.auth;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using ERPW.Lib.Master.Entity;
using SNA.Lib.POS.utils;
using ERPW.Lib.Master;
using System.Web.Script.Serialization;
using ERPW.Lib.Master.Config;
using System.Web.Configuration;
using Agape.Lib.DBService;
using System.Text.RegularExpressions;
using ServiceWeb.Service;

namespace POSWeb.crm
{
    public partial class CustProfileList : AbstractsSANWebpage
    {
        DBService db = new DBService();
        private ERPW.Lib.Master.CustomerService libCustomer = new ERPW.Lib.Master.CustomerService();
        private MasterConfigLibrary masterservice = MasterConfigLibrary.GetInstance();
        public string WorkGroupCode = "20170121162748444411";
        private OwnerService ownerService = new OwnerService();

        protected override Boolean ProgramPermission_CanView()
        {
            return Permission.ContactView || Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return Permission.ContactModify || Permission.AllPermission;
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                bindOwnerService();
                loadCustomerDocType();
                loadContactAuthorization();
                bindDropdownSaleDistrict();
                bindCriteriaCreateCustoemr();
                //loadCustomerList();
            }
        }

        #region Bind & Load Data
        private void loadCustomerDocType() 
        {
            DataTable dt = libCustomer.getCustomerDocType(SID, CompanyCode);

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
        }
        private void loadContactAuthorization()
        {
            ddlContactAuthorization.DataTextField = "Name";
            ddlContactAuthorization.DataValueField = "Code";
            ddlContactAuthorization.DataSource = masterservice.GetMasterConfigAuthorizationContact(SID);
            ddlContactAuthorization.DataBind();
            ddlContactAuthorization.Items.Insert(0, new ListItem("All", ""));
        }

        private void CheckSingleQuotePanelI()
        {
            string txtCPhone = txtPhone.Text;
            string txtCAddress = txtAddress.Text;
            string txtCEmail = txtEmail.Text;
            string txtCTaxID = txtTaxID.Text;
            int Count = 0;
            //txtPhone.Text;
            foreach (char c in txtCPhone)
            {
                if (txtCPhone.Contains("'"))
                {
                    Count++;
                }
            }
            if (Count > 0)
            {
                txtCPhone = "null";
                txtPhone.Text = txtCPhone.Replace("null", "\"");
            }
            //txtAddress.Text;
            foreach (char c in txtCAddress)
            {
                if (txtCAddress.Contains("'"))
                {
                    Count++;
                }
            }
            if (Count > 0)
            {
                txtCAddress = "null";
                txtAddress.Text = txtCAddress.Replace("null", "\"");
            }
            //txtEmail.Text;
            foreach (char c in txtCEmail)
            {
                if (txtCEmail.Contains("'"))
                {
                    Count++;
                }
            }
            if (Count > 0)
            {
                txtCEmail = "null";
                txtEmail.Text = txtCEmail.Replace("null", "\"");
            }
            //txtTaxID.Text;
            foreach (char c in txtCTaxID)
            {
                if (txtCTaxID.Contains("'"))
                {
                    Count++;
                }
            }
            if (Count > 0)
            {
                txtCTaxID = "null";
                txtTaxID.Text = txtCTaxID.Replace("null", "\"");
            }
        }
        private void CheckSingleQuotePanelII()
        {
            string txtContName = txtContactName.Text;
            string txtContNickName = txtContactNickName.Text;
            string txtContPOSITION = txtContactPOSITION.Text;
            string txtContRemark = txtContactRemark.Text;
            string txtContEmail = txtContactEmail.Text;
            int Count = 0;
            //txtContactName.Text;
            foreach (char c in txtContName)
            {
                if (txtContName.Contains("'"))
                {
                    Count++;
                }
            }
            if (Count > 0)
            {
                txtContName = "null";
                txtContactName.Text = txtContName.Replace("null", "\"");
            }
            //txtContactNickName.Text;
            foreach (char c in txtContNickName)
            {
                if (txtContNickName.Contains("'"))
                {
                    Count++;
                }
            }
            if (Count > 0)
            {
                txtContNickName = "null";
                txtContactNickName.Text = txtContNickName.Replace("null", "\"");
            }
            //txtContactPOSITION.Text;
            foreach (char c in txtContPOSITION)
            {
                if (txtContPOSITION.Contains("'"))
                {
                    Count++;
                }
            }
            if (Count > 0)
            {
                txtContPOSITION = "null";
                txtContactPOSITION.Text = txtContPOSITION.Replace("null", "\"");
            }
            //txtContactRemark.Text;
            foreach (char c in txtContRemark)
            {
                if (txtContRemark.Contains("'"))
                {
                    Count++;
                }
            }
            if (Count > 0)
            {
                txtContRemark = "null";
                txtContactRemark.Text = txtContRemark.Replace("null", "\"");
            }

            //txtContactEmail.Text;
            foreach (char c in txtContEmail)
            {
                if (txtContEmail.Contains("'"))
                {
                    Count++;
                }
            }
            if (Count > 0)
            {
                txtContEmail = "null";
                txtContactEmail.Text = txtContEmail.Replace("null", "\"");
            }
        }
        private void loadCustomerList()
        {
            CheckSingleQuotePanelII();
            ContactEntity contact = new ContactEntity();
            contact.NAME1 = txtContactName.Text;
            contact.NickName = txtContactNickName.Text;
            contact.POSITION = txtContactPOSITION.Text;
            contact.REMARK = txtContactRemark.Text;
            contact.email = txtContactEmail.Text;
            contact.phone = txtContactPhone.Text;
            contact.AUTH_CONTACT = ddlContactAuthorization.SelectedValue;

            CheckSingleQuotePanelI();
            var ListCustomer = libCustomer.SearchCustomerAllData(
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
                txtTaxID.Text,
                ddlCustomerActive.SelectedValue,
                contact
            );

            if (!string.IsNullOrEmpty(ddlOwnerService.SelectedValue))
            {
                ListCustomer = ListCustomer.Where(w => w.OwnerServiceCode == ddlOwnerService.SelectedValue).ToList();
            }

            ListCustomer.ForEach(r =>
            {
                r.Address = ReplaceHexadecimalSymbols(r.Address);
            });

            var dataSource = ListCustomer.Select(s => new {
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

            divJsonCustomerList.InnerHtml = JsonConvert.SerializeObject(dataSource).Replace("<", "&lt;").Replace(">", "&gt;");
            //rptCustProfileModeTable.DataSource = dtCustomer;
            //rptCustProfileModeTable.DataBind();
                   
            upPanelProfileList.Update();

            ClientService.DoJavascript("afterSearch();");
        }
      
        private void bindDropdownSaleDistrict()
        {
            ddlSaleDistrict.DataSource = libCustomer.getSaleArea(SID, "");
            ddlSaleDistrict.DataBind();
            ddlSaleDistrict.Items.Insert(0, new ListItem("All", ""));
        }

       

        #endregion

        protected void btnSearchData_Click(object sender, EventArgs e)
        {
            try
            {
                loadCustomerList();

                ClientService.DoJavascript("scrollToTable();");
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

        public string ConvertDateTimeToDisplay(string date) 
        {
            return Validation.Convert2DateTimeDisplay(date);
        }       

        public string ConvertEmailManage(Object Email)
        {
            string srtEmail = Email.ToString();

            if (String.IsNullOrEmpty(srtEmail))
            {
                srtEmail = srtEmail.Replace(",", " ");
            }

            return srtEmail;
        }

        public string setNameCustomer_Contact(string customerCode, string customerName,
            string contactNickname, string contactName, bool IsMember)
        {
            if (IsMember)
            {
                return customerCode + " - " + customerName;
            }
            else
            {
                return contactNickname + " - " + contactName;
            }
            return "";
        }

        public DataTable dtCloudList
        {
            get
            {
                if (Session["CloudSystemList.dtCloudList"] == null)
                {
                    Session["CloudSystemList.dtCloudList"] = new DataTable();
                }
                return (DataTable)Session["CloudSystemList.dtCloudList"];
            }
            set { Session["CloudSystemList.dtCloudList"] = value; }
        }

        #region Create Customer

        protected void btnSetDataOpenModalCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsAllFeature)
                {
                    bindPropertiesForCreate();
                    ClientService.DoJavascript("loadEmployeeWithoutCondition"+AutoCompleteEmployee.ClientID+"();showInitiativeModal('modal-EditCustomerDetail');");
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

        protected void btnSetCustomerCodeRefConfig_Click(object sender, EventArgs e)
        {
            try
            {
                bool isGenCode = false;
                if (!string.IsNullOrEmpty(_ddl_CD_CustomerGroup.SelectedValue))
                {
                    isGenCode = libCustomer.isAutoGenerateCustomerCode(
                   SID,
                   CompanyCode,
                   _ddl_CD_CustomerGroup.SelectedValue
                   );
                }

                _txt_CD_CustomerCode.Enabled = isGenCode;
                if (isGenCode)
                {
                    _txt_CD_CustomerCode.CssClass = "form-control form-control-sm required";
                }
                else 
                {
                    _txt_CD_CustomerCode.CssClass = "form-control form-control-sm";
                }
                udpCustomerCodeRefConfig.Update();
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

        protected void btnCheckRequireFieldCrate_Click(object sender, EventArgs e)
        {
            ClientService.DoJavascript("updateCustomerDetailRefModalClick($('#" + btnCreateCustomerDetail.ClientID + "'));");
        }
        protected void btnCreateCustomerDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(_txt_CD_CustomerName.Text.Trim()))
                {
                    throw new Exception("Please input customer name!");
                }

                DataTable dtCustomerName = db.selectData("SELECT CustomerName FROM master_customer WHERE CustomerName = '" + _txt_CD_CustomerName.Text.Trim() + "' ");
                //DataTable dtFaxID = db.selectData("SELECT FederalTaxID as TaxID FROM master_customer WHERE FederalTaxID = '" + _txt_CD_CustomerTaxID.Value + "' ");
                DataTable dtFaxID = new DataTable();
                if (dtCustomerName.Rows.Count > 0)
                {
                    ClientService.AGError("Invalid Create customer please check Client Name");
                }
                //else if (_txt_CD_CustomerTaxID.Value != "") //dtFaxID.Rows.Count > 0 &&  un check Tax ID
                //{
                //    ClientService.AGError("The customer has already been assigned, please check Tax ID");
                //}
                else
                {
                    CustomerCreateEntity enCustomer = new CustomerCreateEntity();
                    string CustomerCode = _txt_CD_CustomerCode.Text;
                    #region Create Customer

                    enCustomer.CustomerGroup = _ddl_CD_CustomerGroup.SelectedValue;
                    enCustomer.CustomerCode = CustomerCode;
                    enCustomer.CustomerName = _txt_CD_CustomerName.Text;
                    enCustomer.ForeignName = _txt_CD_ForeignName.Text;
                    enCustomer.TaxID = _txt_CD_CustomerTaxID.Value;
                    enCustomer.TelNo1 = _txt_CD_CustomerPhone.Value;
                    enCustomer.Mobile = _txt_CD_CustomerPhoneMoblie.Value;
                    enCustomer.EMail = _txt_CD_CustomerEmail.Value;
                    enCustomer.SalesEmployee = AutoCompleteEmployee.SelectedValue;
                    enCustomer.OwnerService = ddlOwnerService_Created.SelectedValue;

                    string AddressCode = "001";
                    AddressListDetail addressDetail = new JavaScriptSerializer().Deserialize<AddressListDetail>(hddJsonAddress.Value);
                    ContactCustomerERPW.AddressInfo listAddress = libCustomer.getCustomerAddress(
                        SID,
                        CompanyCode,
                        CustomerCode,
                        AddressCode);
                    for (int i = 0; i < listAddress.listAddressProperty.Count; i++)
                    {
                        listAddress.listAddressProperty[i].PropertyValue = addressDetail.address[i].PropertyValue;
                    }


                    string SessionID = POSDocumentHelper.getSessionId(SID, UserName);
                    libCustomer.saveCreateCustomer(SessionID, SID, CompanyCode
                        , EmployeeCode
                        , enCustomer
                        , listAddress);

                    #endregion

                    ClientService.DoJavascript("closeInitiativeModal('modal-EditCustomerDetail');");
                    ClientService.AGSuccess("Create customer " + _txt_CD_CustomerName.Text + " success");
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

        private void bindCriteriaCreateCustoemr()
        {
            DataTable dt = libCustomer.getCustomerDocType(SID, CompanyCode);
            foreach (DataRow dr in dt.Rows)
            {
                dr["Description"] = ObjectUtil.PrepareCodeAndDescription(
                    dr["CustomerGroupCode"].ToString(),
                    dr["Description"].ToString()
                );
            }
            _ddl_CD_CustomerGroup.DataValueField = "CustomerGroupCode";
            _ddl_CD_CustomerGroup.DataTextField = "Description";
            _ddl_CD_CustomerGroup.DataSource = dt;
            _ddl_CD_CustomerGroup.DataBind();
            _ddl_CD_CustomerGroup.Items.Insert(0, new ListItem("เลือก", ""));
        }

        private void bindPropertiesForCreate()
        {
            ContactCustomerERPW.AddressInfo AddressDetail = libCustomer.getCustomerAddressForCreate(
                SID,
                CompanyCode
                );
            rptAddAddress.DataSource = AddressDetail.listAddressProperty;
            rptAddAddress.DataBind();
            _ddl_CD_CustomerGroup.SelectedIndex = 0;
            _txt_CD_CustomerCode.Text = "";
            _txt_CD_CustomerName.Text = "";
            _txt_CD_ForeignName.Text = "";
            AutoCompleteEmployee.SelectedValueRefresh = "";
            _txt_CD_CustomerTaxID.Value = "";
            _txt_CD_CustomerPhone.Value = "";
            _txt_CD_CustomerPhoneMoblie.Value = "";
            _txt_CD_CustomerEmail.Value = "";

            hddJsonAddress.Value = "";
            udpCustomerCodeRefConfig.Update();
            udpCustomerCreate.Update();
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



        private void bindOwnerService()
        {
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

                ddlOwnerService_Created.Items.Clear();
                ddlOwnerService_Created.Items.Insert(0,
                    new ListItem(
                        Permission.OwnerGroupName,
                        Permission.OwnerGroupCode
                    )
                );
              

                // #Edit for Multi OwnerService Customer

                DataTable dtDataUserOwnerService = ownerService.getMappingOwner(UserName);//get data ownerService


                ddlOwnerService.DataTextField = "OwnerGroupName";
                ddlOwnerService.DataValueField = "OwnerService";
                ddlOwnerService.DataSource = dtDataUserOwnerService;
                ddlOwnerService.DataBind();
                ddlOwnerService.SelectedIndex = 0;

                ddlOwnerService_Created.DataTextField = "OwnerGroupName";
                ddlOwnerService_Created.DataValueField = "OwnerService";
                ddlOwnerService_Created.DataSource = dtDataUserOwnerService;
                ddlOwnerService_Created.DataBind();
                ddlOwnerService_Created.SelectedIndex = 0;

                if (dtDataUserOwnerService.Rows.Count == 1)
                {
                    ddlOwnerService.Enabled = false;
                    ddlOwnerService.CssClass = "form-control form-control-sm";

                    ddlOwnerService_Created.Enabled = false;
                    ddlOwnerService_Created.CssClass = "form-control form-control-sm";
                }   
             
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


                ddlOwnerService_Created.DataTextField = "OwnerGroupName";
                ddlOwnerService_Created.DataValueField = "OwnerGroupCode";
                ddlOwnerService_Created.DataSource = dtOwner;
                ddlOwnerService_Created.DataBind();
                ddlOwnerService_Created.Items.Insert(0, new ListItem("", ""));
                
            }
        }

        static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }
    }
}