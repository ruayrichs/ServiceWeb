using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.TTM_Training
{
    public partial class Born_page : AbstractsSANWebpage
    {
        Customer_Training customer_lib = new Customer_Training();
        protected void Page_Load(object sender, EventArgs e)
        {
            searchCustomerByName("");
        }

        private void addCustomer(Customer_Training.Customer_Model medel) {
            customer_lib.addDataCustomer_Training(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                medel);
         
        }
        private void deleteCustomer(string customerCode) {
            customer_lib.deleteDataCustomer_Training(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                customerCode);

        }
        private void searchCustomerByName(string Name) {
            List<Customer_Training.Customer_Model> customer_list = customer_lib.getDataCustomer_Training(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                Name);
            customer_lists.DataSource = customer_list;
            customer_lists.DataBind();
            update_cutomers_list.Update();
        }
        private void editCustomer(Customer_Training.Customer_Model cus_model) {
            customer_lib.updateDataCustomer_Training(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                cus_model
                );
        }
        protected void btnSearch_Click(object sender, EventArgs e) {
            try
            {
                searchCustomerByName(txtSearch.Text);
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
        protected void btnSave_Click(object sender, EventArgs e) {
            try {
                Customer_Training.Customer_Model customer_model = new Customer_Training.Customer_Model();
                customer_model.CustomerName = txtName.Text;
                customer_model.CustomerType = txtType.Text;
                customer_model.Province = txtProvine.Text;
                customer_model.SaleArea = txtArea.Text;
                customer_model.Tel = txtTel.Text;
                customer_model.Email = txtMail.Text;
                customer_model.ContactType = txtConType.Text;
                addCustomer(customer_model);
                searchCustomerByName("");
            }
            catch (Exception ex) {
                ClientService.AGError(ex.Message);
            }
            finally {
                ClientService.AGLoading(false);
            }
           

        }
        protected void btnDelete_Click(object sender,EventArgs e) {
            try
            {
               
               Button btn = (sender as Button);
                string CusCode = btn.CommandArgument;
                deleteCustomer(CusCode);
                ClientService.AGMessage("deleted");
                searchCustomerByName("");
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
        protected void btnEdit_Click(object sender,EventArgs e) {
            try
            {
                
                Button btn = (sender as Button);
                string CusCode = btn.CommandArgument;
                
                Customer_Training.Customer_Model customer_model = customer_lib.getCustomerDetailByCustomerCode_Training(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    CusCode
                    );
               
                txtEditName.Text = customer_model.CustomerName;
                txtEditConType.SelectedItem.Text = customer_model.ContactType;
                txtEditArea.SelectedItem.Text = customer_model.SaleArea;
                txtEditEmail.Text = customer_model.Email;
                txtEditType.SelectedItem.Text = customer_model.CustomerType;

                txtEditProvine.Text = customer_model.Province;
                txtEditTel.Text = customer_model.Tel;
                update_edit.Update();

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
        protected void btnSaveEdit_Click(object sender,EventArgs e) {
            try {
                ClientService.AGMessage("sssssss");
                Customer_Training.Customer_Model customer_model = new Customer_Training.Customer_Model();
            customer_model.CustomerName = txtEditName.Text;
            customer_model.CustomerType = txtEditType.Text;
            customer_model.Province = txtEditProvine.Text;
            customer_model.SaleArea = txtEditArea.Text;
            customer_model.Tel = txtEditTel.Text;
            customer_model.Email = txtEditEmail.Text;
            customer_model.ContactType = txtEditConType.Text;
            editCustomer(customer_model);
            searchCustomerByName("");

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
    }
}