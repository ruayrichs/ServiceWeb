using ERPW.Lib.Master;
using Newtonsoft.Json;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb
{
    public partial class PhoneSearch : AbstractsSANWebpage//System.Web.UI.Page
    {
        private ERPW.Lib.Master.CustomerService libCustomer = new ERPW.Lib.Master.CustomerService();
        protected void Page_Load(object sender, EventArgs e)
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
        protected void btnSearchByPhone_Click(object sender, EventArgs e)
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
            txtContactPhone.Text = Request["ContactPhone"];

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

            txtPhone.Text = Request["Phone"];
            txtPhoneMobile.Text = Request["PhoneMobile"];            

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
                ListCustomer = ListCustomer.Where(w => w.OwnerService == ddlOwnerService.SelectedValue).ToList();
            }

            ListCustomer.ForEach(r =>
            {
                r.Address = ReplaceHexadecimalSymbols(r.Address);
            });

            var dataSource = ListCustomer.Select(s => new
            {
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
                s.CustomerCritical
            });

            divJsonCustomerList.InnerHtml = JsonConvert.SerializeObject(dataSource).Replace("<", "&lt;").Replace(">", "&gt;");
            //rptCustProfileModeTable.DataSource = dtCustomer;
            //rptCustProfileModeTable.DataBind();

            upPanelProfileList.Update();

            ClientService.DoJavascript("afterSearch();");
        }

        static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }
    }
}