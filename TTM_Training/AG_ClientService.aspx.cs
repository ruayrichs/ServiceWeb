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
    public partial class AG_ClientService : AbstractsSANWebpage
    {
        Customer_Training lib = new Customer_Training();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void Message_Alert()
        {
            // Function Message Alert จะรับ string
            ClientService.AGMessage("Message");
            ClientService.AGSuccess("Message Success");
            ClientService.AGInfo("Message Infomation");
            ClientService.AGError("Message Error");

            // Function Loading จะรับ true, false
            ClientService.AGLoading(true); // true เปิดตัวโหลด, false ปิดตัวโหลด
        }

        private void bindData()
        {
            List<Customer_Training.Customer_Model> en = lib.getDataCustomer_Training(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                ""
            );

            rptCusDetail.DataSource = en;
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