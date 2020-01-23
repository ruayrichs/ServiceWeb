using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.WebConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API
{
    public partial class TierZeroProblemInformAPI : System.Web.UI.Page
    {
        private TierZeroLibrary tierservice = TierZeroLibrary.GetInstance();
        private UniversalService universalService = new UniversalService();
        private ServiceTicketLibrary lib = new ServiceTicketLibrary();

        #region Status Code
        private const String STATUS_SUCCESS = "S";
        private const String STATUS_ERROR = "E";
        private const String STATUS_WARNING = "W";
        #endregion

        #region Event Code

        #endregion

        #region Private Primary Key

        private string sid { get { return !String.IsNullOrEmpty(ERPWebConfig.GetSID()) ? ERPWebConfig.GetSID() : Request["sid"]; } }
        private string companycode { get { return !String.IsNullOrEmpty(ERPWebConfig.GetCompany()) ? ERPWebConfig.GetCompany() : Request["companycode"]; } }
        private string username { get { return !String.IsNullOrEmpty(ERPWAuthentication.UserName) ? ERPWAuthentication.UserName : "focusone"; } }

        private string email { get { return Request["email"]; } }
        private string telno { get { return Request["telno"]; } }
        //string otpid = Request["otpid"];

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.ContentType = "application/json";
                switch (Request["action"])
                {
                    case "vaid_contact":
                        checkValidEmail();
                        break;
                    case "otp_generate":
                        createOtpToEmail();
                        break;
                    case "password_verify":
                        checkPasswordMapping();
                        break;
                    case "otp_verify":
                        checkOtpMapping();
                        break;
                    default:
                        throw new Exception("Process not found!!");
                }
            }
            catch (Exception ex)
            {
                JObject result = new JObject();
                result.Add("status", STATUS_ERROR);
                result.Add("message", ObjectUtil.Err(ex.Message));
                result.Add("result", "");
                Response.Write(result);
            }
        }

        private void checkValidEmail()
        {
            string email = Request["email"];
            string telno = Request["telno"];

            tierservice.checkValidCustomer(sid, companycode, email, telno);

            ResponseResult("Success.", "");
        }

        private void createOtpToEmail()
        {
            string email = Request["email"];
            string telno = Request["telno"];
            tierservice.createOtpForSendEmail(sid, companycode, email, telno, username);
            ResponseResult("Send OTP to email success.", "");
        }

        private void checkPasswordMapping()
        {
            string password = Request["password"];

            DataTable dtotp = tierservice.verifyPassword(sid, companycode, email, telno, password);

            if (dtotp.Rows.Count <= 0)
            {
                throw new Exception("Your password not found in system !!");
            }

            string customerCode = Convert.ToString(dtotp.Rows[0]["CUSTOMER_CODE"]);
            DataTable dt = universalService.GetEquipmentCustomerAssignment(sid, companycode, "", customerCode);

            JArray listequip = new JArray();
            foreach (DataRow dr in dt.Rows)
            {
                string equipmentName = lib.PrepareCodeAndDescription(dr["EquipmentCode"].ToString(), dr["EquipmentName"].ToString());
                JObject obj = new JObject();
                obj.Add("code", dr["EquipmentCode"].ToString());
                obj.Add("desc", dr["EquipmentCode"].ToString());
                obj.Add("display", equipmentName);
                listequip.Add(obj);
            }

            JObject endata = new JObject();
            endata.Add("equipment", listequip);
            ResponseResult("Password is correct", endata);

        }

        private void checkOtpMapping()
        {
            string otpid = Request["otpid"];
            DataTable dtotp = tierservice.verifyOTP(sid, companycode, email, telno, otpid);
            if (dtotp.Rows.Count <= 0)
            {
                throw new Exception("Otp " + otpid + " not found in system !!");
            }
            string customerCode = Convert.ToString(dtotp.Rows[0]["CUSTOMER_CODE"]);
            DataTable dt = universalService.GetEquipmentCustomerAssignment(sid, companycode, "", customerCode);

            JArray listequip = new JArray();
            foreach (DataRow dr in dt.Rows)
            {
                string equipmentName = lib.PrepareCodeAndDescription(dr["EquipmentCode"].ToString(), dr["EquipmentName"].ToString());
                JObject obj = new JObject();
                obj.Add("code", dr["EquipmentCode"].ToString());
                obj.Add("desc", dr["EquipmentCode"].ToString());
                obj.Add("display", equipmentName);
                listequip.Add(obj);
            }

            JObject endata = new JObject();
            endata.Add("equipment", listequip);
            ResponseResult("OTP is correct", endata);

        }

        private void ResponseResult<T>(string message, T Result)
        {
            JArray data = new JArray();
            JObject result = new JObject();
            result.Add("status", STATUS_SUCCESS);
            result.Add("message", message);
            result.Add("result", JsonConvert.SerializeObject(Result));
            Response.Write(result.ToString());
            GC.Collect();
        }
    }
}