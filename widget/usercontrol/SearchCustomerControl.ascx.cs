using ERPW.Lib.Service;
using Newtonsoft.Json;
using ServiceWeb.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using ERPW.Lib.Master.Config;
using Newtonsoft.Json.Linq;
using ERPW.Lib.F1WebService.ICMUtils;
using ERPW.Lib.Master;

namespace ServiceWeb.widget.usercontrol
{
    public partial class SearchCustomerControl : System.Web.UI.UserControl
    {
        private ServiceTicketLibrary lib = new ServiceTicketLibrary();
        //private SNA.Lib.Transaction.ChatService chatService = SNA.Lib.Transaction.ChatService.getInstance();        
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        private UniversalService universalService = new UniversalService();
        private MasterConfigLibrary config = MasterConfigLibrary.GetInstance();
        private EquipmentService ServiceEquipment = new EquipmentService();
        private ERPW.Lib.Master.CustomerService serviceCustomer = ERPW.Lib.Master.CustomerService.getInstance();
        public string workGroupCode = "20170121162748444411";
        public static string CHAT_BUSINESSOBJ = "SC";
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public List<string> listCustomerCode
        {
            get
            {
                List<string> listcode = new List<string>();
                if (!string.IsNullOrEmpty(txtSearchCusSelect.Text))
                {
                    listcode = txtSearchCusSelect.Text.Split(',').ToList();
                }
                return listcode;
            }
        }

        private void setlistCustomerCodeDefult()
        {
            listCustomerCode.Clear();
            string CI = txtSearchCusSelect.Text.ToString();
            string[] CIList = CI.Split(',');
            for (int index = 0; index < CIList.Length; index++)
            {
                System.Diagnostics.Debug.WriteLine(CIList[index]);
                listCustomerCode.Add(CIList[index]);
            }
        }

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
               ERPWAuthentication.SID,
               ERPWAuthentication.CompanyCode,
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
                DataTable dt = serviceCustomer.getCustomerDocType(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);
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
                ddlSaleDistrict.DataSource = serviceCustomer.getSaleArea(ERPWAuthentication.SID, "");
                ddlSaleDistrict.DataBind();
                ddlSaleDistrict.Items.Insert(0, new ListItem("All", ""));

                ddlContactAuthorization.DataTextField = "Name";
                ddlContactAuthorization.DataValueField = "Code";
                ddlContactAuthorization.DataSource = config.GetMasterConfigAuthorizationContact(ERPWAuthentication.SID);
                ddlContactAuthorization.DataBind();
                ddlContactAuthorization.Items.Insert(0, new ListItem("All", ""));
            }
            udpUpdateCustomerCriteria.Update();
            JArray data = new JArray();
            udpSearchCustomerCriteria.Update();
            ClientService.DoJavascript("afterSearchBindCustomerCriteria(" + data + ");");
        }
    }
}