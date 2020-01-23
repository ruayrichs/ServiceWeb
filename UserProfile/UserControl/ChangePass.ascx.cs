using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using SNA.Lib.Authentication;
using agape.entity.UserProfile;
using SNA.Lib.Master;
using agape.lib.constant;
using ERPW.Lib.F1WebService;
using ERPW.Lib.F1WebService.ChangePwdService;
using ERPW.Lib.WebConfig;
using ERPW.Lib.Authentication;
using ServiceWeb.auth;
//using SNA.Lib.Transaction.UserLoginService;
//using SNA.Lib.Transaction;
//using SNA.Lib.Transaction.ChangePwdService;
//using AreaP21.Service;

namespace ServiceWeb.UserProfile.usercontrol
{
    public partial class ChangePass : System.Web.UI.UserControl
    {
        //UserLoginService.UserLoginService loginService = new UserLoginService.UserLoginService();
        //private UserLoginService loginService = WSHelper.getUserLoginService();
        SNAMasterService SNAMasterService = new SNAMasterService(); 

        public UserProfileBean UserProfileBean
        {
            get
            {
                return (UserProfileBean)Session[ApplicationSession.EMPLOYEE_SESSION_BEAN];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ValidatePassword();
                //SNAAuthentication.ChangeNewPassword(SNAAuthentication.EmployeeCode, SNAAuthentication.CompanyCode, txtConfirmPassword.Value);
                Change_Password();

                ClientService.AGSuccess("แก้ไข Password เรียบร้อย");
                //ScriptManager.RegisterStartupScript(upanelChangePassword, upanelChangePassword.GetType(), "msgbox", "AGMessage('แก้ไข Password เรียบร้อย');", true);
                //ClientService.DoJavascript("location.href = '/';");
            }
            catch (Exception ex)
            {
                ClientService.AGMessage(ex.Message.Replace("'", ""));
                //ClientService.AGError(ex.Message.Replace("'", ""));
                //ScriptManager.RegisterStartupScript(upanelChangePassword, upanelChangePassword.GetType(), "msgbox", "AGMessage('" + ex.Message.Replace("'", "") + "');", true);
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }

        private void ValidatePassword()
        {
            ///SNAAuthentication.CheckPassword(SNAAuthentication.Email, txtOldPassword.Value, SNAAuthentication.CompanyCode);
            if (txtNewPassword.Value.CompareTo(txtConfirmPassword.Value) != 0)
            {
                throw new Exception("รหัสผ่านใหม่ไม่ ตรงกับ ยืนยันรหัสผ่าน");
            }
            if (txtConfirmPassword.Value == "")
            {
                throw new Exception("กรุณากรอกรหัสผ่านใหม่");
            }
            if (txtNewPassword.Value == "")
            {
                throw new Exception("กรุณากรอก ยืนยันรหัสผ่าน");
            }
            ////// Check Password
            List<string> listMes = new List<string>();

            if (txtNewPassword.Value.Length < 8)
            {
                listMes.Add("กรุณาระบุรหัสผ่านไม่น้อยกว่า 8 ตัวอักษร!!");
            }

            string LowerCase = "abcdefghijklmnopqrstuvwxyz";
            string UperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string NumCase = "1234567890";
            string SpecialCase = "!@#$%^&*()_+|~-=\\`{}[]:\";'<>?,./";

            bool IsLowerCase = false;
            bool IsUperCase = false;
            bool IsNumCase = false;
            bool IsSpecialCase = false;
            for (int i = 0; i < txtNewPassword.Value.Length; i++)
            {
                string pass = txtNewPassword.Value[i].ToString();

                if (LowerCase.IndexOf(pass) >= 0)
                {
                    IsLowerCase = true;
                }
                if (UperCase.IndexOf(pass) >= 0)
                {
                    IsUperCase = true;
                }
                if (NumCase.IndexOf(pass) >= 0)
                {
                    IsNumCase = true;
                }
                if (SpecialCase.IndexOf(pass) >= 0)
                {
                    IsSpecialCase = true;
                }
            }

            if (!IsLowerCase)
            {
                listMes.Add("รหัสผ่านต้องเป็นตัวอักษรพิมเล็กอย่างน้อย 1 ตัว");
            }
            if (!IsUperCase)
            {
                listMes.Add("รหัสผ่านต้องเป็นตัวอักษรพิมใหญ่อย่างน้อย 1 ตัว");
            }
            if (!IsNumCase)
            {
                listMes.Add("รหัสผ่านต้องเป็นตัวเลขอย่างน้อย 1 ตัว");
            }
            //if (isPasswordhavespecialChar(txtPassword.Text.Trim()))
            if (!IsSpecialCase)
            {
                listMes.Add("รหัสผ่านต้องอักขระพิเศษอย่างน้อย 1 ตัว");
            }
            
            //AbstractsSANWebpage AbstractsSANWebpage = new AbstractsSANWebpage();

            //if (AbstractsSANWebpage.IsAllFeature)
            //{
                
            //}
            if (listMes.Count > 0)
            {
                throw new Exception(String.Join("<br>", listMes));
            }
            /////////// Check Password
        }

        private void Change_Password()
        {
            try
            {
                string sid = ERPWAuthentication.SID;
                string username = ERPWAuthentication.UserName;

                string resetpassword = txtConfirmPassword.Value.Trim();// GeneratePSWHelper.GeneratePassword(10);
                string empname = ERPWAuthentication.FullNameTH;

                #region - check old password -
                //loginService.Url = ConfigurationManager.AppSettings["SNAWeb_Mother_UserLoginService"];
                //String callResult = loginService.Login(sid, username, txtOldPassword.Value.Trim());
                string callResult = F1WebService.getUserLoginService().Login(sid, username, txtOldPassword.Value.Trim());
                if (!string.IsNullOrEmpty(callResult) && callResult.Split('#')[0].Equals("S"))
                {

                }
                else
                {
                    throw new Exception("รหัสผ่านเดิมไม่ถูกต้อง");
                }
                #endregion
                //ChangePwdService PwdService = WSHelper.getChangePwdServiceMother();
                ChangePwdService PwdService = F1WebService.getChangePwdService();
                //string _result = PwdService.ChangePassword(sid, username, resetpassword);
                string _result = PwdService.ChangePassword(sid, username, resetpassword);

                if (!string.IsNullOrEmpty(_result))
                {
                    if (!string.IsNullOrEmpty(_result) && _result.Split('#')[0].Equals("S"))
                    {
                        //PopUpMessage.Text = "Change Password Success !!";
                    }
                    else
                    {
                        throw new Exception(_result.Split('#')[1]);
                        //PopUpMessage.Text = "Cannot Change Password !!";
                    }
                }
                //ส่งเมล์ยืนยันการเปลี่ยนรหัส
                //SNAMasterService.ConfirmChangePassword(UserProfileBean.Email, empname, resetpassword, "");

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(upanelChangePassword, upanelChangePassword.GetType(), "msgbox", "AGMessage('" + ex.Message.Replace("'", "") + "');", true);
            }
        }
    }
}