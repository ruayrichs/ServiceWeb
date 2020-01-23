using ERPW.Lib.F1WebService;
using ERPW.Lib.F1WebService.ChangePwdService;
using ERPW.Lib.Master;
using ERPW.Lib.Service;
using ERPW.Lib.WebConfig;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb
{
    public partial class ForgetPassword : System.Web.UI.Page
    {
        private UserManagementService userservice = UserManagementService.getInstance();
        private BusinessOwnerLib libBusinessOwner = new BusinessOwnerLib();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {

            }
            //SKWebMasterPage mast = (SKWebMasterPage)this.Master;// (SKWebMasterPage)Master.FindControl("ContentPlaceHolder1");
            //mast.showMenu = false;
            //mast.pageName = "ลืมรหัสผ่าน";
        }

        protected void _btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!Page.IsValid)
                if (!IsReCaptchValid())
                {
                    throw new Exception("Captcha verification failed");
                }

                if (string.IsNullOrEmpty(_tb_Mail.Text))
                    throw new Exception("กรุณาระบุอีเมล์");

                ResetPassword();

                //Session["reset_success"] = "ระบบได้ส่งเมล์ยืนยันรีเซ็ตรหัสผ่านให้ " + _tb_Mail.Text + " เรียบร้อยแล้ว<br/>หากท่านไม่ได้รับอีเมลล์จากระบบ กรุณาตรวจสอบในเมล์ขยะ (Junk Mail) หรือ Bulk หรือ SPAM ของท่าน";

                string _msg = "ระบบได้ส่งเมล์ยืนยันรีเซ็ตรหัสผ่านให้ " + _tb_Mail.Text + " เรียบร้อยแล้ว<br/>หากท่านไม่ได้รับอีเมลล์จากระบบ กรุณาตรวจสอบในเมล์ขยะ (Junk Mail) หรือ Bulk หรือ SPAM ของท่าน";
                ClientService.AGSuccessRedirect(_msg, "/Login.aspx");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void ResetPassword()
        {
            try
            {
                string sid = ERPWebConfig.GetSID();
                string company = ERPWebConfig.GetCompany();
                string username = "";
                string empname = "";
                DataTable dt = userservice.getUserByEmail(sid, company,_tb_Mail.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    username = Convert.ToString(dt.Rows[0]["Username"]);
                    empname = Convert.ToString(dt.Rows[0]["FirstName"]);
                }
                else
                {
                    throw new Exception("ไม่พบ E-Mail : " + _tb_Mail.Text + " ในระบบ");
                }
                string resetpassword = libBusinessOwner.CreatePassword(10); //Membership.GeneratePassword(10, 0); //GeneratePSWHelper.GeneratePassword(10);
                ChangePwdService PwdService = F1WebService.getChangePwdService();
                string _result = PwdService.ChangePassword(sid, username, resetpassword);
                if (!string.IsNullOrEmpty(_result))
                {
                    if (!string.IsNullOrEmpty(_result) && _result.Split('#')[0].Equals("S"))
                    {
                        //PopUpMessage.Text = "Change Password Success !!";
                    }
                    else
                    {
                        //PopUpMessage.Text = "Cannot Change Password !!";
                    }
                }

                //Reset to send Email
                NotificationLibrary.GetInstance().sendResetPasswordToEmail(sid, company, _tb_Mail.Text.Trim(), resetpassword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsReCaptchValid()
        {
            var result = false;
            var captchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = "6Leq5ZIUAAAAAHKtavbQcmWV5woX5ObQV0Uukvo3";
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
        }
    }
}