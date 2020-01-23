
using SNA.Lib.crm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SNA.Lib.crm.entity;
using System.Data;
using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;

namespace POSWeb.crm.usercontrol
{
    public partial class ctrlCustProfileEdit : System.Web.UI.UserControl
    {
        private CRMService serviceCRM = CRMService.getInstance();
        public ContactCustomer contactCustomer
        {
            get
            {
                ContactCustomer curContact = (ContactCustomer)Session["CustProfileDetail.ContactCustomer"];
                return curContact;
            }
            set
            {
                Session["CustProfileDetail.ContactCustomer"] = value;
            }
        }

        public Boolean ISMyProFile
        {
            get
            {
                return true;
            }

        }

        public String bpcode
        {
            get
            {
                return Request.QueryString["id"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (!String.IsNullOrEmpty(Request.QueryString["contactcode"]))
                {
                    contactCustomer = serviceCRM.getUserProfile("", ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, bpcode, Request.QueryString["contactcode"]);
                }
                else
                {
                    contactCustomer = serviceCRM.getUserProfile("", ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, bpcode);
                }
                SetTextBox();
            }
        }

        private void SetTextBox()
        {
            // Edit Profile
            txtFirstNameEdit.Text = contactCustomer.FirstName;
            txtLastNameEdit.Text = contactCustomer.LastName;
            txtNicknameEdit.Text = contactCustomer.NickName;
            txtWorkPositionEdit.Text = contactCustomer.WorkPosition;
            txtPhone.Text = contactCustomer.Phone;
            txtEmailEdit.Text = contactCustomer.Email;
            txtFaceBookEdit.Text = contactCustomer.SocialFacebook;
            txtInstagramEdit.Text = contactCustomer.SocialInstagram;
            txtTwitterEdit.Text = contactCustomer.SocialTweeter;
            ddlGender.SelectedValue = contactCustomer.Gender;
        }

        protected void btnSaveProfileSocial_Click(object sender, EventArgs e)
        {
            try
            {
                serviceCRM.updateCustomerProfileSocial(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, contactCustomer.BObjectLink, bpcode,
                    txtFirstNameEdit.Text, txtLastNameEdit.Text, txtNicknameEdit.Text, txtEmailEdit.Text, txtWorkPositionEdit.Text, txtPhone.Text,
                    txtFaceBookEdit.Text, txtInstagramEdit.Text, txtTwitterEdit.Text, ddlGender.SelectedValue);

                contactCustomer = serviceCRM.getUserProfile("", ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, bpcode);

                ClientService.AGSuccess("บันทึกสำเร็จ");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }
        
    }
}