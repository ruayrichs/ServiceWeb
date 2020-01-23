using agape.lib.constant;
using agape.lib.web.configuration.utils;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ERPW.Lib.Master;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SNAWeb.Analytics
{
    public class AnalyticsService
    {
        private static DBService db = new DBService();

        public static List<AnalyticsList> GetAnalyticsList(int TopRow, string CompanyCode, string SID, string EmployeeCode, string DateFromDisplay, string DateToDisplay)
        {
            string sql = @"select top " + TopRow + @" ant.* ,emp.FirstName_TH + ' ' + emp.LastName_TH as FullName
                            from LINK_ANALYTICS ant WITH (NOLOCK) 
                            left join master_employee emp WITH (NOLOCK) on emp.CompanyCode = ant.CompanyCode
                            and ant.sid = emp.SID and emp.EmployeeCode = ant.EmployeeCode
                            where ant.CompanyCode = '" + CompanyCode + "' and ant.SID = '" + SID + "'";

            if (!string.IsNullOrEmpty(EmployeeCode))
            {
                sql += " and ant.EmployeeCode = '" + EmployeeCode + "' ";
            }

            if (!string.IsNullOrEmpty(DateFromDisplay))
            {
                string dateTime = Validation.Convert2DateDB(DateFromDisplay);
                sql += " and ant.DateTimeIn >= " + dateTime + "000000000";
            }

            if (!string.IsNullOrEmpty(DateToDisplay))
            {
                string dateTime = Validation.Convert2DateDB(DateToDisplay);
                sql += " and ant.DateTimeIn <= " + dateTime + "235959999";
            }

            sql += " order by ant.DateTimeIn desc";

            List<AnalyticsList> ListResult = JsonConvert.DeserializeObject<List<AnalyticsList>>(JsonConvert.SerializeObject(db.selectDataFocusone(sql)));

            return ListResult;
        }


        public static AnalyticsEmployee getTackingPageTicketIsAuthenEdit(string SID, string CompanyCode,
            string ReferenceID, string EmployeeCode, string ReferencePageMode = ApplicationSession.CHANGE_MODE_STRING)
        {
            AnalyticsEmployee enAnalytics = new AnalyticsEmployee();
            List<AnalyticsList> en = getTackingPageAnalytic(SID, CompanyCode, ReferenceID, ReferencePageMode);
            if (en.Count > 0)
            {
                AnalyticsList firstView = en.First();
                enAnalytics.firstEmployeeCode = firstView.EmployeeCode;
                enAnalytics.firstEmployeeName = firstView.FullName;

                if (firstView.EmployeeCode == EmployeeCode)
                {
                    enAnalytics.IsAuthenEdit = true;
                }
                else
                {
                    enAnalytics.IsAuthenEdit = false;
                }
            }
            else
            {
                enAnalytics.IsAuthenEdit = true;
            }

            return enAnalytics;
        }
        private static List<AnalyticsList> getTackingPageAnalytic(string SID, string CompanyCode,
            string ReferenceID, string ReferencePageMode)
        {
            DateTime _TimeOut = DateTime.Now.AddMinutes(-15);
            string DateTimeIn_TimeOut = _TimeOut.Year.ToString()
                + _TimeOut.Month.ToString().PadLeft(2, '0')
                + _TimeOut.Day.ToString().PadLeft(2, '0')
                + _TimeOut.Hour.ToString().PadLeft(2, '0')
                + _TimeOut.Minute.ToString().PadLeft(2, '0')
                + _TimeOut.Second.ToString().PadLeft(2, '0')
                + _TimeOut.Millisecond.ToString().PadLeft(3, '0');

            string sql = @"SELECT a.* , b.FirstName + ' ' + b.LastName as FullName
                           FROM LINK_ANALYTICS a WITH (NOLOCK) 
                           Inner join master_employee b WITH (NOLOCK) 
                              on a.SID = b.SID
                              and a.CompanyCode = b.CompanyCode
                              and a.EmployeeCode = b.EmployeeCode
                           where a.SID = '" + SID + @"'
                              and a.CompanyCode = '" + CompanyCode + @"'
                              and a.ProgramID = '" + LogServiceLibrary.PROGRAM_ID_SERVICE_CALL + @"'
                              and a.ReferenceID = '" + ReferenceID + @"'
                              and a.ReferencePageMode = '" + ReferencePageMode + @"'
                              and a.DateTimeOut is null
                              and a.DateTimeIn > '" + DateTimeIn_TimeOut + @"'
                           order by a.DateTimeIn asc";

            List<AnalyticsList> ListResult = JsonConvert.DeserializeObject<List<AnalyticsList>>(JsonConvert.SerializeObject(db.selectDataFocusone(sql)));
            return ListResult;
        }

        public static void UpdateExitPage(string RowKey, string DateTimeMillisecondOut)
        {
            string sql_Select = @"select * from LINK_ANALYTICS WITH (NOLOCK)  Where Row_key = '" + RowKey + "'";
            List<AnalyticsList> ListResult = JsonConvert.DeserializeObject<List<AnalyticsList>>(
                JsonConvert.SerializeObject(db.selectDataFocusone(sql_Select))
            );

            if (ListResult.Count > 0)
            {
                AnalyticsList ResultEn = ListResult.First();

                DateTime DateTimeForm = ObjectUtil.ConvertDateTimeDBToDateTime(ResultEn.DateTimeIn);
                DateTime DateTimeTo = ObjectUtil.ConvertDateTimeDBToDateTime(DateTimeMillisecondOut);
                TimeSpan ts = DateTimeTo - DateTimeForm;
                double LiveOn = ts.TotalSeconds;

                UpdateExitPage(
                    ResultEn.CompanyCode,
                    ResultEn.SID, 
                    ResultEn.EmployeeCode,
                    ResultEn.Row_key,
                    LiveOn.ToString(), 
                    DateTimeMillisecondOut
                );
            }
        }

        public static void UpdateExitPage(string CompanyCode, string SID, string EmployeeCode, string RowKey, string LiveOn, string DateTimeOut)
        {
            //string sql = " update LINK_ANALYTICS set LiveOn = '" + LiveOn + "',DateTimeOut = '" + DateTimeOut +
            //         "' where CompanyCode = '" + CompanyCode + "' and SID = '" + SID +
            //         "' and EmployeeCode = '" + EmployeeCode + "' and Row_key = '" + RowKey + "';";


            string sql = @" update LINK_ANALYTICS 
                                set LiveOn = '" + LiveOn + @"'
                                ,DateTimeOut = '" + DateTimeOut + @"' 
                            where EmployeeCode = '" + EmployeeCode + @"' 
                                and Row_key = '" + RowKey + "';";

            db.executeSQLForFocusone(sql);
        }
        public static void CreateNewRowAnalytics(Analytics _Analytics)
        {
            Dictionary<string, string> dict = GetEntityProperties(_Analytics);
            string Columns = string.Join(",", dict.Keys);
            string Values = string.Join(",", dict.Values);
            string sql = "insert into LINK_ANALYTICS (" + Columns + ") values(" + Values + ");";

            db.executeSQLForFocusone(sql);
        }
        public static Dictionary<string, string> GetEntityProperties(Object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.DeclaredOnly |
                                               BindingFlags.Public |
                                               BindingFlags.Instance);

            Dictionary<string, string> ReturnDict = new Dictionary<string, string>();
            foreach (PropertyInfo i in properties)
            {
                ReturnDict.Add(i.Name, "'" + Convert.ToString(i.GetValue(obj, null)) + "'");
            }

            return ReturnDict;
        }
    }
    [Serializable]
    public class Analytics
    {
        public string CompanyCode { get; set; }
        public string SID { get; set; }
        public string EmployeeCode { get; set; }
        public string PageName { get; set; }
        public string PathName { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string FromUrl { get; set; }
        public string OS { get; set; }
        public string Browser { get; set; }
        public string BrowserVersion { get; set; }
        public string Mobile { get; set; }
        public string Flash { get; set; }
        public string Cookies { get; set; }
        public string Screen { get; set; }
        public int LiveOn { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Acc { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string RegionName { get; set; }
        public string Language { get; set; }
        public string ISP { get; set; }
        public string IP { get; set; }
        public string DeviceName { get; set; }
        public string DateTimeIn { get; set; }
        public string Row_key { get; set; }
        public string ProgramID { get; set; }
        public string ReferenceID { get; set; }
        public string ReferencePageMode { get; set; }
        
    }

    [Serializable]
    public class AnalyticsList : Analytics
    {
        public string FullName { get; set; }
    }

    [Serializable]
    public class AnalyticsEmployee
    {
        public Boolean IsAuthenEdit { get; set; }
        public string firstEmployeeCode { get; set; }
        public string firstEmployeeName { get; set; }
    }
}

