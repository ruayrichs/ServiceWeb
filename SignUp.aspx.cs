using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using ERPW.Lib.WebConfig;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb
{
    public partial class SignUp : System.Web.UI.Page
    {
        private string SID = "";
        private string CompanyCode = "";
        private DateTime cdt = DateTime.Now;
        private string _year;
        private string _month;
        private string _day;
        private string _hour;
        private string _minute;
        private string _seconds;

        private string year
        {
            get
            {
                if (string.IsNullOrEmpty(_year))
                {
                    _year = cdt.Year.ToString();
                }
                return _year;
            }
        }
        private string month
        {
            get
            {
                if (string.IsNullOrEmpty(_month))
                {
                    if (cdt.Month < 10)
                    {
                        _month = "0" + cdt.Month.ToString();
                    }
                    else
                    {
                        _month = cdt.Month.ToString();
                    }

                }
                return _month;
            }
        }
        private string day
        {
            get
            {
                if (string.IsNullOrEmpty(_day))
                {
                    if (cdt.Day < 10)
                    {
                        _day = "0" + cdt.Day.ToString();
                    }
                    else
                    {
                        _day = cdt.Day.ToString();
                    }

                }
                return _day;
            }
        }
        private string hour
        {
            get
            {
                if (string.IsNullOrEmpty(_hour))
                {
                    if (cdt.Hour < 10)
                    {
                        _hour = "0" + cdt.Hour.ToString();
                    }
                    else
                    {
                        _hour = cdt.Hour.ToString();
                    }

                }
                return _hour;
            }
        }
        private string minute
        {
            get
            {
                if (string.IsNullOrEmpty(_minute))
                {
                    if (cdt.Minute < 10)
                    {
                        _minute = "0" + cdt.Minute.ToString();
                    }
                    else
                    {
                        _minute = cdt.Minute.ToString();
                    }

                }
                return _minute;
            }
        }
        private string seconds
        {
            get
            {
                if (string.IsNullOrEmpty(_seconds))
                {
                    if (cdt.Second < 10)
                    {
                        _seconds = "0" + cdt.Second.ToString();
                    }
                    else
                    {
                        _seconds = cdt.Second.ToString();
                    }

                }
                return _seconds;
            }
        }
        private BusinessOwnerLib businessOwnerLib = new BusinessOwnerLib();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void createNewUser()
        {
            string crdt = year + month + day + hour + minute + seconds;
            System.Diagnostics.Debug.WriteLine("current date time is: " + crdt);
            BusinessOwnerEN rawData = new BusinessOwnerEN();
            rawData.FirstName = fname_inp.Text;
            rawData.LastName = lname_inp.Text;
            rawData.BusinessOwner = BusinessOwner_inp.Text;
            rawData.Username = uname_inp.Text;
            rawData.Email = email_inp.Text;
            rawData.Phone = phone_inp.Text;
            rawData.Status = "WAITING";
            rawData.SignUp_DateTime = crdt;//today
            rawData.UpdateStatus_On = crdt;//today
            rawData.UpdateStatus_By = "";
            rawData.Activation = "False";
            rawData.Activation_DateTime = "";//
            businessOwnerLib.createRow(SID, CompanyCode, rawData);

            ClientService.DoJavascript("afterCreate();");
            ClientService.AGSuccessRedirect(
                "Thank you for your  Signing up. Please check your account password via email.",
                Page.ResolveUrl("~/Login.aspx")
            );
            //Response.Redirect("/Success.aspx");
        }

        protected void createUser_btn_Click(object sender, EventArgs e)
        {
            string fname;// = fname_inp.Text;
            string lname;// = lname_inp.Text;
            string uname;// = uname_inp.Text;
            try
            {
                if (!IsReCaptchValid())
                {
                    udpRechapcha.Update();
                    throw new Exception("Captcha verification failed");
                }

                // firstname vv
                if (string.IsNullOrEmpty(fname_inp.Text))
                {
                    throw new Exception("Enter Firstname !!");
                }
                if (!checkDataText(fname_inp.Text))
                {
                    throw new Exception("Firstname must be english");
                }
                if (fname_inp.Text.Length > 30)
                {
                    throw new Exception("Firstname limit 30 character");
                }
                fname = fname_inp.Text;
                // lastname vv
                if (string.IsNullOrEmpty(lname_inp.Text))
                {
                    throw new Exception("Enter Lastname !!");
                }
                if (!checkDataText(lname_inp.Text))
                {
                    throw new Exception("Lastname must be english");
                }
                if (lname_inp.Text.Length > 30)
                {
                    throw new Exception("Lastname limit 30 character");
                }
                lname = lname_inp.Text;
                // bussinessowner vv
                if (string.IsNullOrEmpty(BusinessOwner_inp.Text))
                {
                    throw new Exception("Enter BussinessOwner !!");
                }
                if (BusinessOwner_inp.Text.Length > 50)
                {
                    throw new Exception("BussinessOwner limit 50 character");
                }
                // username vv
                if (string.IsNullOrEmpty(uname_inp.Text))
                {
                    throw new Exception("Enter username !!");
                }
                if (!checkDataText(uname_inp.Text))
                {
                    throw new Exception("Username Name must be english");
                }
                if (uname_inp.Text.Length > 20)
                {
                    throw new Exception("Username limit 20 character");
                }
                if (!businessOwnerLib.isUsernameUsed(ERPWebConfig.GetSID(), ERPWebConfig.GetCompany(), uname_inp.Text))
                {
                    throw new Exception("Username already used by another user !!");
                }uname = uname_inp.Text;
                // email vv
                if (string.IsNullOrEmpty(email_inp.Text))
                {
                    throw new Exception("Enter email !!");
                }
                if (!businessOwnerLib.isEmailUsed(ERPWebConfig.GetSID(), ERPWebConfig.GetCompany(), email_inp.Text))
                {
                    throw new Exception("Email already used by another user !!");
                }

                createNewUser();
                ClientService.AGSuccess("Create Success");
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            //finally
            //{
            //    ClientService.AGLoading(false);
            //}
        }

        private bool checkDataText(string input)
        {
            bool IsValit = true;
            string InputCheck = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+|~-=\\`{}[]:\";'<>?,./";
            for (int i = 0; i < input.Trim().Length && IsValit; i++)
            {
                if (InputCheck.IndexOf(input[i].ToString()) >= 0)
                {
                    IsValit = true;
                }
                else
                {
                    IsValit = false;
                }
            }
            return IsValit;
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