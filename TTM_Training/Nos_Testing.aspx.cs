 using System;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using ServiceWeb.auth;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ServiceWeb.TTM_Training
{
    public partial class Nos_Testing : AbstractsSANWebpage
    {
        Customer_Training ObjectCus = new Customer_Training();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void Getdata()
        {
            string textsearch = txt_search.Text;
            List<Customer_Training.Customer_Model> Listdata = new List<Customer_Training.Customer_Model>();
            if (String.IsNullOrEmpty(textsearch)) {
                Listdata = ObjectCus.getDataCustomer_Training(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                ""
                );
            }
            else
            {
                Listdata = ObjectCus.getDataCustomer_Training(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                textsearch
                );
            }
            TableCusDetail.DataSource = Listdata;
            TableCusDetail.DataBind();
            UpdateTableCusDetail.Update();
        }

        protected void btn_search(object sender, EventArgs e)
        {
            try
            {
                Getdata();
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

        protected void btn_create(object sender, EventArgs e)
        {
            try
            {
                string getvaluedropdown = TypeConCus.Text;
                System.Diagnostics.Debug.WriteLine(getvaluedropdown);
                Customer_Training.Customer_Model ModelCus = new Customer_Training.Customer_Model();
                ModelCus.CustomerName = Convert.ToString(Name.Text);
                ModelCus.CustomerType = Convert.ToString(Type.Text);
                ModelCus.Province = Convert.ToString(Province.Text);
                ModelCus.Tel = Convert.ToString(Phone.Text);
                ModelCus.Email = Convert.ToString(Email.Text);
                ModelCus.ContactType = Convert.ToString(TypeConCus.Text);
                ModelCus.SaleArea = Convert.ToString(TypeSell.Text);

                ObjectCus.addDataCustomer_Training(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    ModelCus
                );
                Getdata();
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

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (sender as Button);
                string IdCus = btn.CommandArgument;
                Customer_Training.Customer_Model ModelCus = new Customer_Training.Customer_Model();
                Getdata();
                ModelCus = ObjectCus.getCustomerDetailByCustomerCode_Training(
                     ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    IdCus
                    );
                Cusid.Text = ModelCus.CustomerCode;
                Nameed.Text = ModelCus.CustomerName;
                Typeed.Text = ModelCus.CustomerType;
                Provinceed.Text = ModelCus.Province;
                Phoneed.Text = ModelCus.Tel;
                Emailed.Text = ModelCus.Email;
                TypeConCus.SelectedValue = ModelCus.ContactType;
                TypeSelled.SelectedValue = ModelCus.SaleArea;
                EditPanel.Update();
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

        protected void btn_delete_cus(object sender, EventArgs e)
        {
            try
            {
                Button btn = (sender as Button);
                string IdCus = btn.CommandArgument;
                ObjectCus.deleteDataCustomer_Training(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    IdCus
                );
                Getdata();
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

        protected void btn_save_edit(object sender, EventArgs e)
        {
            try
            {
                Customer_Training.Customer_Model ModelCus = new Customer_Training.Customer_Model();
                ModelCus.CustomerCode = Cusid.Text;
                ModelCus.CustomerName = Nameed.Text;
                ModelCus.CustomerType = Typeed.Text;
                ModelCus.Province = Provinceed.Text;
                ModelCus.Tel = Phoneed.Text;
                ModelCus.ContactType = TypeConCused.Text;
                ModelCus.Email = Emailed.Text;
                ModelCus.SaleArea = TypeSelled.Text;
                ObjectCus.updateDataCustomer_Training(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    ModelCus
                    );
                Getdata();
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