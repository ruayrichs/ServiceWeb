using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        MasterWorkingTimeConfigLib workingtimeConfigLib = new MasterWorkingTimeConfigLib();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    bindData();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }

        }
        public string ConvertdbDateToDisplayDate(string dbDate)
        {
            DateTime datetime_date = DateTime.ParseExact(dbDate,
                                    "yyyyMMdd",
                                     CultureInfo.CurrentCulture);
            string dispDate = String.Format("{0:yyyy/MM/dd}", datetime_date);
            return dispDate;
        }
        private void bindData()
        {
            List<string> lstHolidays = workingtimeConfigLib.GetAllHolidays(
                       ERPWAuthentication.SID,
                       ERPWAuthentication.CompanyCode,
                       MasterWorkingTimeConfigLib.Holidays_TYPE
                   );
            
            if(lstHolidays.Count > 0)
            {
                List<string> lstDispDate = new List<string>();
                foreach (string dbDate in lstHolidays)
                {
                    lstDispDate.Add("'"+ConvertdbDateToDisplayDate(dbDate)+"'");
                }
                string strHolidays = string.Join(",", lstDispDate.ToArray());
                
                Page.ClientScript.RegisterStartupScript(GetType(), "holiday", "$(function(){ $('#holiday').datepicker('update', "+ strHolidays + ");}); ", true);
            }

            List<WorkingTimeConfig> lst_wtEn = workingtimeConfigLib.GetWorkingTime_Config(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode
                );
            if (lst_wtEn.Count > 0)
            {
                WorkingTimeConfig wtEn = lst_wtEn[0];
                txtStartTime.Text = ConvertToDisplay(wtEn.StartTime);
                txtEndTime.Text = ConvertToDisplay(wtEn.EndTime);
                string strWorkday = string.Join(",", convertDBtoShow(wtEn.Workday).ToArray());
                Page.ClientScript.RegisterStartupScript(GetType(), "weekday", "$(function(){$('#weekdays').weekdays({selectedIndexes: [" + strWorkday + "]});}); ", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "weekday", "$(function(){$('#weekdays').weekdays()}); ", true);
            }
        }
        public string CovertToDB(string dispTime)
        {
            DateTime datetime_time = DateTime.ParseExact(dispTime,
                                      "HH:mm",
                                       CultureInfo.InvariantCulture);
            string dbTime = Validation.Convert2TimeDB(datetime_time);
            return dbTime;
        }

        public string ConvertToDisplay(string dbTime)
        {
            DateTime datetime_time = DateTime.ParseExact(dbTime,
                                     "HHmmss",
                                      CultureInfo.InvariantCulture);
            string dispTime =  String.Format("{0:HH:mm}", datetime_time);
            return dispTime;
        }

        public List<int> convertDBtoShow(string dbWorkday)
        {
            List<int> lstIndexWorkday = new List<int>();

            for (int i = 0; i < 7; i++)
            {
                if (dbWorkday[i] == '1')
                {
                    lstIndexWorkday.Add(i);
                }
            }
            return lstIndexWorkday;
        }

        public void validInputData()
        {
            if (string.IsNullOrEmpty(hdfDaypickValue.Value))
            {
                throw new Exception("กรุณาระบุ WorkDay");
            }
            if (string.IsNullOrEmpty(txtStartTime.Text))
            {
                throw new Exception("กรุณาระบุ Start Time");
            }
            if (string.IsNullOrEmpty(txtEndTime.Text))
            {
                throw new Exception("กรุณาระบุ End Time");
            }
        }

        public void validWorktime()
        {
            DateTime dt_starttime = DateTime.ParseExact(txtStartTime.Text,
                                   "HH:mm",
                                    CultureInfo.InvariantCulture);
            DateTime dt_endtime = DateTime.ParseExact(txtEndTime.Text,
                            "HH:mm",
                            CultureInfo.InvariantCulture);
            if (dt_starttime > dt_endtime)
            {
                throw new Exception("กรุณาระบุ Start Time, Endtime ให้ถูกต้อง");
            }
        }

        private string generateWorkday(string[] arrWorkday)
        {
            string resultworkday = "";
            foreach (string day in listWorkDay)
            {
                if (arrWorkday.Contains(day))
                {
                    resultworkday += "1";
                }
                else
                {
                    resultworkday += "0";
                }
            }

            return resultworkday; // WorkDay is 0000000 [Sun-Sat (7 digit)]; 1 => workday, 0 => dayoff 
        }

        private List<string> listWorkDay = new List<string>() { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        protected void Btn_Work_Config_Click(object sender, EventArgs e)
        {
            try
            {
                validInputData();
                validWorktime();
                string rawWorkday = hdfDaypickValue.Value;
                string[] arrWorkday = rawWorkday.Split(',');
                string resultworkday = generateWorkday(arrWorkday);

                string holidays = holidayCchap.Value;
                string[] holidaysplit = holidays.Split(',');
                List<string> lstholidays = new List<string>(holidaysplit);
                workingtimeConfigLib.AddHoliday(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        MasterWorkingTimeConfigLib.Holidays_TYPE,
                        lstholidays,
                        ERPWAuthentication.EmployeeCode
                    );

                WorkingTimeConfig wtEn = new WorkingTimeConfig();
                wtEn.Workday = resultworkday;
                wtEn.StartTime = CovertToDB(txtStartTime.Text);
                wtEn.EndTime = CovertToDB(txtEndTime.Text);

                workingtimeConfigLib.SetWorkingTime_Config(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    wtEn,
                    ERPWAuthentication.EmployeeCode
                );

                ClientService.AGSuccess("บันทึกสำเร็จ");
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