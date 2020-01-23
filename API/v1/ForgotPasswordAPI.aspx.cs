using ERPW.Lib.F1WebService;
using ERPW.Lib.F1WebService.ChangePwdService;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Constant;
using ERPW.Lib.Service;
using ERPW.Lib.WebConfig;
using Newtonsoft.Json;
using ServiceWeb.API.Model.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;





//namespace ServiceWeb.API.v1
//{
//    public partial class ForgotPasswordAPI : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {

//        }
//    }
//}


namespace ServiceWeb.API.v1
{
    public partial class ForgotPasswordAPI : System.Web.UI.Page
    {
        private UserManagementService userservice = UserManagementService.getInstance();
        private string Email { get { return Request["Email"]; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            CommonResponseModel response = new CommonResponseModel();
            try
            {
                resetPassword();

                response.resultCode = ConfigurationConstant.API_RESULT_CODE_SUCCESS;
                response.message = "Change Password Success. Please check your account password via email.";
                response.resultTime = DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                response.resultCode = ConfigurationConstant.API_RESULT_CODE_ERROR;
                response.message = ex.Message;
                response.resultTime = DateTime.Now.ToString();
            }
            string srResponse = JsonConvert.SerializeObject(response);
            Response.Write(srResponse);
        }

        private void resetPassword()
        {
            if (string.IsNullOrEmpty(Email))
                throw new Exception("กรุณาระบุอีเมล์");

            ResetPassword();
        }


        private void ResetPassword()
        {
            try
            {
                string sid = ERPWebConfig.GetSID();
                string company = ERPWebConfig.GetCompany();
                string username = "";
                string empname = "";
                DataTable dt = userservice.getUserByEmail(sid, company, Email.Trim());
                if (dt.Rows.Count > 0)
                {
                    username = Convert.ToString(dt.Rows[0]["Username"]);
                    empname = Convert.ToString(dt.Rows[0]["FirstName"]);
                }
                else
                {
                    throw new Exception("ไม่พบ E-Mail : " + Email + " ในระบบ");
                }
                string resetpassword = Membership.GeneratePassword(10, 0); //GeneratePSWHelper.GeneratePassword(10);
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
                        throw new Exception("Cannot Change Password !!");
                    }
                }

                //Reset to send Email
                NotificationLibrary.GetInstance().sendResetPasswordToEmail(sid, company, Email, resetpassword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}