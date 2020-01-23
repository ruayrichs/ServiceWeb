using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.TTM_Training
{
    public partial class SeenPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //indata();
        }

        private void indata()
        {
            string SID = ERPWAuthentication.SID;
            string Companycode = ERPWAuthentication.CompanyCode;

            Customer_Training ct = new Customer_Training();
            List<Customer_Training.Customer_Model> listCustomer = new List<Customer_Training.Customer_Model>();
            listCustomer = ct.getDataCustomer_Training(SID, Companycode, "");

            rptCusDetail.DataSource = listCustomer;
            rptCusDetail.DataBind();
            udpDataList.Update(); // สั่งให้ Update Html ภายใน UpdatePanel ID udpDataList
        }

        protected void btn_customer_save_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = customer_name.Text;
                string customerTel = customer_tel.Text;
                //string customerSaleArea = customer_salearea.Text;
                string customerSaleArea = DDL_customer_salearea.Text;
                string customerType = customer_type.Text;
                string customerEmail = customer_email.Text;
                string customerProvince = customer_province.Text;
                string customerContractType = DDL_customer_contracttype.Text;

                Customer_Training ct = new Customer_Training();
                Customer_Training.Customer_Model en = new Customer_Training.Customer_Model();
                en.CustomerName = customerName;
                en.Tel = customerTel;
                en.SaleArea = customerSaleArea;
                en.CustomerType = customerType;
                en.Email = customerEmail;
                en.Province = customerProvince;
                en.ContactType = customerContractType;

                System.Diagnostics.Debug.WriteLine(en.CustomerName);
                System.Diagnostics.Debug.WriteLine(en.Tel);
                System.Diagnostics.Debug.WriteLine(en.SaleArea);
                System.Diagnostics.Debug.WriteLine(en.CustomerType);
                System.Diagnostics.Debug.WriteLine(en.Email);
                System.Diagnostics.Debug.WriteLine(en.Province);
                System.Diagnostics.Debug.WriteLine(en.ContactType);

                string SID = ERPWAuthentication.SID;
                string Companycode = ERPWAuthentication.CompanyCode;

                ct.addDataCustomer_Training(SID, Companycode, en);
            }
            catch (Exception e1)
            {
                //Console.WriteLine(e1);
                System.Diagnostics.Debug.WriteLine(e1);
            }
        }

        protected void customer_search_Click(object sender, EventArgs e)
        {
            string name = search_customer_name.Text;

            Customer_Training ct = new Customer_Training();

            string SID = ERPWAuthentication.SID;
            string Companycode = ERPWAuthentication.CompanyCode;

            List<Customer_Training.Customer_Model> listCustomer = new List<Customer_Training.Customer_Model>();
            listCustomer = ct.getDataCustomer_Training(SID, Companycode, name);
            //ct.getDataCustomer_Training

            rptCusDetail.DataSource = listCustomer;
            rptCusDetail.DataBind();
            udpDataList.Update(); // สั่งให้ Update Html ภายใน UpdatePanel ID udpDataList
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //string name = search_customer_name.Text;
            Button btn = (sender as Button);
            string CustomerCode = btn.CommandArgument;

            Customer_Training ct = new Customer_Training();

            string SID = ERPWAuthentication.SID;
            string Companycode = ERPWAuthentication.CompanyCode;

            ct.deleteDataCustomer_Training(SID, Companycode, CustomerCode);

            System.Diagnostics.Debug.WriteLine("delete success");

            string name = search_customer_name.Text;

            List<Customer_Training.Customer_Model> listCustomer = new List<Customer_Training.Customer_Model>();
            listCustomer = ct.getDataCustomer_Training(SID, Companycode, name);

            rptCusDetail.DataSource = listCustomer;
            rptCusDetail.DataBind();
            udpDataList.Update(); // สั่งให้ Update Html ภายใน UpdatePanel ID udpDataList
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Button btn = (sender as Button);
            string CustomerCode = btn.CommandArgument;
            string SID = ERPWAuthentication.SID;
            string Companycode = ERPWAuthentication.CompanyCode;

            Customer_Training ct = new Customer_Training();
            ct.getCustomerDetailByCustomerCode_Training(SID, Companycode, CustomerCode);


            System.Diagnostics.Debug.WriteLine("edit success");
        }
    }
}