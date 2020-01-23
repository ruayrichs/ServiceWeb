using agape.lib.constant;
using Agape.FocusOne.Utilities;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Constant;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using ERPW.Lib.WebConfig;
using Newtonsoft.Json;
using ServiceWeb.API.Model.Common;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;





//namespace ServiceWeb.API.v1
//{
//    public partial class SignUpAPI : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {

//        }
//    }
//}


namespace ServiceWeb.API.v1
{
    public partial class SignUpAPI : System.Web.UI.Page
    {
        private string sid { get { return ERPWebConfig.GetSID(); } }
        private string companycode { get { return ERPWebConfig.GetCompany(); } }

        private string email { get { return Request["Email"]; } }
        private string username { get { return Request["Username"]; } }
        private string password { get { return Request["Password"]; } }
        private string firstname { get { return Request["Firstname"]; } }
        private string lastname { get { return Request["Lastname"]; } }
        private string ownercode { get { return Request["Ownercode"]; } }

        private string empGroup { get { return "EMP01"; } }
        private string dateStart { get { return Validation.Convert2DateDB(Validation.getCurrentServerDate()); } }
        private string dateEnd { get { return (DateTime.Now.Year + 20).ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0'); } }
        private string empRole { get { return "EndUser"; } }
        private string mDefuault_Profileid { get { return "000001"; } }

        private UserManagementService userservice = UserManagementService.getInstance();
        private ERPW_API_Permission_Token_Key_DAO pkey_dao = new ERPW_API_Permission_Token_Key_DAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            CommonResponseModel response = new CommonResponseModel();
            try
            {
                if (string.IsNullOrEmpty(email)) throw new Exception("Please input email.");
                if (string.IsNullOrEmpty(username)) throw new Exception("Please input username.");
                if (string.IsNullOrEmpty(password)) throw new Exception("Please input password.");
                if (string.IsNullOrEmpty(firstname)) throw new Exception("Please input firstname.");
                if (string.IsNullOrEmpty(lastname)) throw new Exception("Please input lastname.");
                if (string.IsNullOrEmpty(ownercode)) throw new Exception("Please input business owner code.");

                if (!checkOwnerCode())
                {
                    throw new Exception("Business owner '" + ownercode + "' not found.");
                }
                if (checkEmail())
                {
                    throw new Exception("Email '" + email + "' is already exists.");
                }
                if (checkUsername())
                {
                    throw new Exception("Username '" + username + "' is already exists.");
                }

                SystemModeControlService.SystemModeEntities mode = SystemModeControlService.getInstanceMode("Link");
                ERPWAutoLoginService loginService = new ERPWAutoLoginService(ERPWebConfig.GetSID(), "system_s1", mode);

                registerEndUser();

                Response.Cookies["SystemControlService_SID"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["SystemControlService_Email"].Expires = DateTime.Now.AddDays(-1);
                Session.Abandon();

                response.resultCode = ConfigurationConstant.API_RESULT_CODE_SUCCESS;
                response.message = "Registration Success."; //Please check your account password via email.
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

        private bool checkEmail()
        {
            bool isValit = false;

            string DataOwnerCode = ServiceLibrary.LookUpTable("Email", "master_employee_address", " where SID = '" + sid + "' AND CompanyCode = '" + companycode + "' AND Email = '" + email + "' ");

            if (string.IsNullOrEmpty(DataOwnerCode))
            {
                isValit = false;
            }
            else
            {
                isValit = true;
            }

            return isValit;
        }

        private bool checkUsername()
        {
            bool isValit = false;

            string DataOwnerCode = ServiceLibrary.LookUpTable("UserName", "master_employee", " where SID = '" + sid + "' AND CompanyCode = '" + companycode + "' AND UserName = '" + username + "' ");

            if (string.IsNullOrEmpty(DataOwnerCode))
            {
                isValit = false;
            }
            else
            {
                isValit = true;
            }

            return isValit;
        }

        private bool checkOwnerCode()
        {
            bool isValit = false;

            string DataOwnerCode = ServiceLibrary.LookUpTable("OwnerGroupCode", "ERPW_OWNER_GROUP", " where SID = '" + sid + "' AND CompanyCode = '" + companycode + "' AND OwnerGroupCode = '" + ownercode + "' ");

            if (string.IsNullOrEmpty(DataOwnerCode))
            {
                isValit = false;
            }
            else
            {
                isValit = true;
            }

            return isValit;
        }

        public void registerEndUser()
        {
            #region create new user
            prepareApproveUser(sid, companycode, password, ownercode);
            #endregion

            #region create permission key
            ERPW_API_Permission_Token_Key pkey_model = new ERPW_API_Permission_Token_Key();
            pkey_model.Active = "true";
            pkey_model.ChannelRequest = ConfigurationConstant.TIER_ZERO_CHANNEL_MOBILE;
            pkey_model.CompanyCode = companycode;
            pkey_model.Created_By = "";
            pkey_model.Created_On = "";
            pkey_model.EmployeeCode = username;
            pkey_model.EndDate = dateEnd;
            pkey_model.IPAddress = "0.0.0.0";
            pkey_model.PermissionKey = generatePermissionKey(30);
            pkey_model.ProgramName = "Mobile S1";
            pkey_model.Remark = "Register";
            pkey_model.SID = sid;
            pkey_model.StartDate = dateStart;
            pkey_model.Updated_By = "";
            pkey_model.Updated_On = "";
            pkey_dao.addRow(pkey_model);
            #endregion
        }

        private string generatePermissionKey(int length)
        {
            return Guid.NewGuid().ToString("N").Substring(0, length);
        }

        public void prepareApproveUser(string SID, string CompanyCode, string Password, string OwnerServiceCode)
        {
            UserManagementService.DataModel en = prepareEntityFromUI(Password, OwnerServiceCode);
            en.startdate = dateStart;
            en.enddate = dateEnd;

            #region Valid data
            List<string> listMes = new List<string>();
            if (string.IsNullOrEmpty(en.userid))
            {
                listMes.Add("กรุณาระบุชื่อผู้ใช้งาน!!");
            }

            if (listMes.Count > 0)
            {
                throw new Exception(String.Join("<br>", listMes));
            }

            if (string.IsNullOrEmpty(en.userid))
            {
                List<string> listError = new List<string>();
                DataTable dtCheck = userservice.getUserByEmailORUserID(SID, CompanyCode, en.Email, en.userid);
                foreach (DataRow dr in dtCheck.Rows)
                {
                    if (en.userid == Convert.ToString(dr["UserName"]))
                    {
                        listError.Add("มีข้อมูลชื่อผู้ใช้งาน " + en.userid + " อยู่ในระบบแล้ว !! <br> กรุณาตั้งชื่อผู้ใช้ใหม่ !!");
                    }
                    if (en.Email == Convert.ToString(dr["Email"]))
                    {
                        listError.Add("มีข้อมูล " + Convert.ToString(dr["UserName"]) + " ใช้อีเมล์ " + en.Email + " อยู่ในระบบแล้ว !! <br> กรุณาตั้งใช้อีเมล์ใหม่ !!");
                    }
                }

                if (listError.Count > 0)
                {
                    throw new Exception(String.Join("<br>", listError));
                }
            }

            #endregion

            string sessionid = (string)Session[ApplicationSession.USER_SESSION_ID];
            en = userservice.saveUserSystem(UserManagementService.SYSTEM_ERP, sessionid, SID, CompanyCode, username, en);
        }

        private UserManagementService.DataModel prepareEntityFromUI(string Password, string OwnerServiceCode)
        {
            UserManagementService.DataModel en = new UserManagementService.DataModel();
            en.userid = username;
            en.username = username;
            en.description = firstname + " " + lastname;
            en.startdate = dateStart;
            en.enddate = dateEnd;
            en.USERSTATUS = "A";
            en.password = Password;
            en.RoleCode = empRole;
            en.OwnerService = OwnerServiceCode;

            en.listRole = new List<UserManagementService.AuthRole>();
            en.listRole.Add(new UserManagementService.AuthRole
            {
                profileid = mDefuault_Profileid,
                RoleID = "",
                RoleName = "",
                userid = en.userid
            });


            //Employee
            en.EmployeeGroup = empGroup;
            en.EmployeeCode = "";
            en.NamePreFix = "";
            en.FirstName = firstname;
            en.LastName = lastname;
            en.FirstName_TH = firstname;
            en.LastName_TH = lastname;
            en.Email = email;
            en.MobilePhone = "";
            en.TelephoneNumber = "";
            en.Department = "";
            en.PositionCode = "";

            //tab ข้อมูลส่วนตัว
            en.Gender = "";
            en.SocialID = "";
            en.NationalityCode = "";
            en.OtherNationalityCode = "";
            en.BirthDate = "";
            en.BirthPlace = "";
            en.BirthCity = "";
            en.BirthCountry = "";
            en.LanguageCode = "";
            en.ReligionCode = "";
            en.MarryStatus = "";
            int nCountOfChild = 0;
            int.TryParse("0", out nCountOfChild);
            en.NoChild = nCountOfChild.ToString();

            //tab ข้อมูลที่อยู่
            en.HouseNumber = "";
            en.Street = "";
            en.Locality = "";
            en.Amphur = "";
            en.CityCode = "";
            en.CountryCode = "";
            en.ZipCode = "";

            return en;
        }
    }
}
