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
    public partial class TestPage : System.Web.UI.Page
    {
        //private Cs_servicecall_header header = new Cs_servicecall_header();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsCallback)
            {
                bool result = false;
                //result = showDateinCMD();
            }
            
        }

        private bool showDateinCMD()
        {
            //System.Diagnostics.Debug.WriteLine(header.select().Count);
            DateTime cdt = DateTime.Now;
            string year = cdt.Year.ToString();
            string month = cdt.Month.ToString();
            if (Int32.Parse(month) < 10)
            {
                month = 0 + month;
            }
            string day = cdt.Day.ToString();
            if (Int32.Parse(day) < 10)
            {
                day = 0 + day;
            }
            string hour = cdt.Hour.ToString();
            if (Int32.Parse(hour) < 10)
            {
                hour = 0 + hour;
            }
            string minute = cdt.Minute.ToString();
            if (Int32.Parse(minute) < 10)
            {
                minute = 0 + minute;
            }
            string seconds = cdt.Second.ToString();
            if (Int32.Parse(seconds) < 10)
            {
                seconds = 0 + seconds;
            }
            string cdate = year + month + day + hour + minute + seconds;
            System.Diagnostics.Debug.WriteLine(cdate);
            return true;
        }

        protected void genPWD_button_Click(object sender, EventArgs e)
        {
            int length = 0 ;
            try
            {
                length = int.Parse(length_input.Text);
            }catch(Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
                length = 8;
            }
            BusinessOwnerLib businessOwnerLib = new BusinessOwnerLib();

            password_input.Text = businessOwnerLib.CreatePassword(true, true, true, true, false, length);
        }

        protected void send_button_Click(object sender, EventArgs e)
        {
            BusinessOwnerEN businessOwnerEN = new BusinessOwnerEN();
            businessOwnerEN.DisplayUpdated_BY_FullNameEn = @"Mr. Fname Lname";
            businessOwnerEN.Username = "9user";
            businessOwnerEN.UpdateStatus_On = getNowDateTime();
            businessOwnerEN.BusinessOwnerCode = "xxxx";
            businessOwnerEN.BusinessOwner = "yyyy";
            businessOwnerEN.Email = email_input.Text;

            BusinessOwnerLib businessOwnerLib = new BusinessOwnerLib();
            businessOwnerLib.approveUser("555", "INET", businessOwnerEN, password_input.Text);
        }

        private string getNowDateTime()
        {
            DateTime cdt = DateTime.Now;
            string year = cdt.Year.ToString();
            string month = cdt.Month.ToString();
            if (Int32.Parse(month) < 10)
            {
                month = 0 + month;
            }
            string day = cdt.Day.ToString();
            if (Int32.Parse(day) < 10)
            {
                day = 0 + day;
            }
            string hour = cdt.Hour.ToString();
            if (Int32.Parse(hour) < 10)
            {
                hour = 0 + hour;
            }
            string minute = cdt.Minute.ToString();
            if (Int32.Parse(minute) < 10)
            {
                minute = 0 + minute;
            }
            string seconds = cdt.Second.ToString();
            if (Int32.Parse(seconds) < 10)
            {
                seconds = 0 + seconds;
            }
            //string cdate = 
            return year + month + day + hour + minute + seconds;
        }
    }
}