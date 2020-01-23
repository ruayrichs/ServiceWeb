using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.TTM_Training
{
    public partial class SendEmail : System.Web.UI.Page
    {
        private string _SID;// = ERPWAuthentication.SID;
        private string _CompanyCode;// = ERPWAuthentication.CompanyCode;

        private string SID { get
            {
                if (string.IsNullOrEmpty(_SID))
                {
                    _SID = ERPWAuthentication.SID;
                }
                return _SID;
            } }
        private string CompanyCode { get
            {
                if (string.IsNullOrEmpty(_CompanyCode))
                {
                    _CompanyCode = ERPWAuthentication.CompanyCode;
                }
                return _CompanyCode;
            } }
        private BusinessOwnerLib lib = new BusinessOwnerLib();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void sendmail()
        {
            string email = email_inp.Text;
            string tel = tel_inp.Text;
            try
            {
                lib.sendOTPToEmail(SID, CompanyCode, "", "", email, tel);
                ClientService.AGSuccess("Send Email Success");
            }
            catch (Exception err)
            {
                ClientService.AGError(err.Message);
            }
        }

        private void genpass()
        {
            string password = lib.CreatePassword(6);
            genpass_inp.Text = password;
            udpnItems.Update();
        }

        protected void genpass_btn_Click(object sender, EventArgs e)
        {
            genpass();
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            sendmail();
        }

        private void prepareSendMail()
        {
            string Email = email2_inp.Text;
            string SEQ = seq_inp.Text;
            string Status = status_inp.SelectedValue;
            string Password = lib.CreatePassword(6);

            BusinessOwnerEN rawData = lib.readOneBySEQ(SID, CompanyCode, SEQ);
            rawData.Status = Status;
            rawData.Email = Email;
            if (rawData.Status.Equals("APPROVE"))
            {
                lib.approveUser(SID, CompanyCode, rawData, Password);//send email to user
                //updateSEQ(SEQ, Status); //update data
            }
            if (rawData.Status.Equals("REJECT"))
            {
                lib.rejectUser(SID, CompanyCode, rawData); //send email to user
                //updateSEQ(SEQ, Status); //update data
            }
            ClientService.AGSuccess("send email success");
        }

        protected void sendEmail_btn_Click(object sender, EventArgs e)
        {
            prepareSendMail();
        }
    }
}