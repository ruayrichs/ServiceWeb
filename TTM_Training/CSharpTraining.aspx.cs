using ERPW.Lib.Authentication;
using ERPW.Lib.Master;
using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb
{
    public partial class CSharpTraining :  AbstractsSANWebpage
    {
        Customer_Training customerLib = new Customer_Training();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void bindData()
        {
            string Gettxt = txtSearch.Text;
            List<Customer_Training.Customer_Model> ListView = new List<Customer_Training.Customer_Model>();           
            if (Gettxt == "")
            {
                ListView = customerLib.getDataCustomer_Training(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                ""
                  );
            }else
            {
                ListView = customerLib.getDataCustomer_Training(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                Gettxt
                  );
            }          
            rptCusDetail.DataSource = ListView;
            rptCusDetail.DataBind();
            udpDataList.Update(); // สั่งให้ Update Html ภายใน UpdatePanel ID udpDataList
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                bindData();
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (sender as Button);
                string CusCode = btn.CommandArgument;
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