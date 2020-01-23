using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.crm.usercontrol
{
    public partial class modalAddNewContact : System.Web.UI.UserControl
    {
        private CustomerService service = CustomerService.getInstance();

        public delegate void afterSendtoPostBackCurrentPage(object sender, EventArgs e);

        public event afterSendtoPostBackCurrentPage afterSavedata;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void bindAuthorizationContact()
        {
            ddlAuthorizationContact.DataValueField = "Code";
            ddlAuthorizationContact.DataTextField = "Name";
            ddlAuthorizationContact.DataSource = MasterConfigLibrary.GetInstance().GetMasterConfigAuthorizationContact(ERPWAuthentication.SID);
            ddlAuthorizationContact.DataBind();
            ddlAuthorizationContact.Items.Insert(0, new ListItem("เลือก", ""));
        }


        public void SetData(string customer,string contactItem)
        {
            if (ddlAuthorizationContact.Items.Count <= 0)
            {
                bindAuthorizationContact();
            }
            List<ContactEntity> listContact = new List<ContactEntity>();

            if (!String.IsNullOrEmpty(customer) && !String.IsNullOrEmpty(contactItem))
            {
                listContact = service.getListContactRefCustomer(
                 ERPWAuthentication.SID,
                 ERPWAuthentication.CompanyCode,
                 customer,
                 contactItem,"");
            }

            ContactEntity en = new ContactEntity();
            if (listContact.Count > 0)
            {
                en = listContact[0];
                hdfCustomerCode.Value = en.BPCODE;
                hdfContactItem.Value = en.ITEMNO;
                hdfContactType.Value = en.BPTYPE;
            }
            txtName.Text = en.NAME1;
            txtNickName.Text = en.NickName;
            txtPOSITION.Text = en.POSITION;
            txtPhone.Text = en.phone.Trim();
            txtEmail.Text = en.email.Trim();
            txtRemark.Text = en.REMARK;

            try
            {
                ddlAuthorizationContact.SelectedValue = en.AUTH_CONTACT;
            }
            catch (Exception ex)
            {
                ddlAuthorizationContact.SelectedIndex = 0;
            }

            try
            {
                ddlActiveStatus.SelectedValue = en.ActiveStatus.ToString().ToUpper();
            }
            catch (Exception)
            {
                ddlActiveStatus.SelectedIndex = 0;
            }

            udpAddNewContact.Update();
            ClientService.DoJavascript("$('#modal-CreateNewContact').modal('show');");
        }

        protected void btnSetData_Click(object sender, EventArgs e)
        {
            try
            {
                SetData(hdfCustomerCode.Value, hdfContactItem.Value);
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

        protected void btnAddNewContact_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtName.Text) && String.IsNullOrEmpty(txtNickName.Text))
                {
                    throw new Exception("Please input Name OR NickName!");
                }

                if (string.IsNullOrEmpty(ddlAuthorizationContact.SelectedValue))
                {
                    throw new Exception("Please select authorization contact!");
                }

                List<ContactEntity> listContact = new List<ContactEntity>();
                listContact.Add(new ContactEntity() {
                    BPCODE = hdfCustomerCode.Value,
                    BPTYPE = hdfContactType.Value,
                    ITEMNO = hdfContactItem.Value,
                    POSITION = txtPOSITION.Text,
                    NAME1 = txtName.Text,
                    NAME2 = "",
                    NickName = txtNickName.Text,
                    phone = txtPhone.Text,
                    email = txtEmail.Text,
                    REMARK = txtRemark.Text,
                    AUTH_CONTACT = ddlAuthorizationContact.SelectedValue,
                    ActiveStatus = Convert.ToBoolean(ddlActiveStatus.SelectedValue)
                });
                List<ContactEntity> listAddNew = new List<ContactEntity>();
                if (!String.IsNullOrEmpty(hdfCustomerCode.Value.Trim()) && !String.IsNullOrEmpty(hdfContactItem.Value.Trim()))
                {
                    service.updateContactRefCustomer(
                        ERPWAuthentication.SID
                        , ERPWAuthentication.CompanyCode
                        , ERPWAuthentication.EmployeeCode
                        ,ERPWAuthentication.UserName
                        , listContact);
                }
                else 
                {
                   listAddNew = service.saveAddNewContact(
                                      ERPWAuthentication.SID
                                      , ERPWAuthentication.CompanyCode
                                      , ERPWAuthentication.EmployeeCode
                                      , listContact);
                }
                ClientService.DoJavascript("$('#modal-CreateNewContact').modal('hide');");
                ClientService.AGSuccess("Save success.");

                afterSendtoPostBackCurrentPage handlerPostBack = afterSavedata;
                if (handlerPostBack != null)
                {
                    sender = listAddNew;
                    handlerPostBack(sender, new EventArgs());
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

    }
}