using Agape.Lib.DBService;
using ERPW.Lib.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ServiceWeb.Report
{
    public class ReportUnity
    {
        public string ConvertToTimeString(string stringDateTimeForm, string stringDateTimeTo)
        {
            if (!String.IsNullOrEmpty(stringDateTimeForm) && !String.IsNullOrEmpty(stringDateTimeTo))
            {
                DateTime DateTimeForm = ObjectUtil.ConvertDateTimeDBToDateTime(stringDateTimeForm);
                DateTime DateTimeTo = ObjectUtil.ConvertDateTimeDBToDateTime(stringDateTimeTo);

                TimeSpan ts = DateTimeTo - DateTimeForm;
                string time = ts.TotalSeconds.ToString();

                if (!string.IsNullOrEmpty(time))
                {
                    double xTime = 0;
                    double.TryParse(time, out xTime);

                    if (xTime > 0)
                    {
                        TimeSpan t = TimeSpan.FromSeconds(xTime);

                        string answer = "";
                        if (t.Days > 0)
                        {
                            answer += t.Days + " วัน ";
                        }
                        if (t.Days > 0 || t.Hours > 0)
                        {
                            answer += t.Hours + " ชม ";
                        }
                        if (t.Days > 0 || t.Hours > 0 || t.Minutes > 0)
                        {
                            answer += t.Minutes + " นาที ";
                        }
                        if (t.Days > 0 || t.Hours > 0 || t.Minutes > 0 || t.Seconds > 0)
                        {
                            answer += t.Seconds + " วินาที ";
                        }

                        return answer;
                    }
                }
                else
                {
                    return "";
                }
            }

            return "ไม่กำหนด";
        }

        public string ConvertToTimeLiveOn(string time)
        {

            if (!string.IsNullOrEmpty(time))
            {
                double xTime = 0;
                double.TryParse(time, out xTime);

                if (xTime > 0)
                {
                    TimeSpan t = TimeSpan.FromSeconds(xTime);

                    string answer = "";
                    if (t.Days > 0)
                    {
                        answer += t.Days + " วัน ";
                    }
                    if (t.Hours > 0)
                    {
                        answer += t.Hours + " ชม ";
                    }
                    if (t.Minutes > 0)
                    {
                        answer += t.Minutes + " นาที ";
                    }
                    if (t.Seconds > 0)
                    {
                        answer += t.Seconds + " วินาที ";
                    }

                    return answer;
                }
            }
            return "";

        }

        public string ConvertToDurationTimeString(string stringDateTimeForm, string stringDateTimeTo)
        {
            if (!String.IsNullOrEmpty(stringDateTimeForm) && !String.IsNullOrEmpty(stringDateTimeTo))
            {
                DateTime DateTimeForm = ObjectUtil.ConvertDateTimeDBToDateTime(stringDateTimeForm);
                DateTime DateTimeTo = ObjectUtil.ConvertDateTimeDBToDateTime(stringDateTimeTo);

                TimeSpan ts = DateTimeTo - DateTimeForm;
                string time = ts.TotalSeconds.ToString();

                if (!string.IsNullOrEmpty(time))
                {
                    double xTime = 0;
                    double.TryParse(time, out xTime);

                    if (xTime > 0)
                    {
                        TimeSpan t = TimeSpan.FromSeconds(xTime);

                        string answer = "";
                        if (t.Days > 0)
                        {
                            if (t.Days < 10)
                            {
                                answer += "0";
                            }
                            answer += t.Days + " Day ";
                        }
                        else
                        {
                            answer += "00 Day ";
                        }
                        if (t.Hours > 0)
                        {
                            if (t.Hours < 10)
                            {
                                answer += "0";
                            }
                            answer += t.Hours;
                        }
                        else
                        {
                            answer += "00";
                        }
                        if (t.Minutes > 0)
                        {
                            answer += ":";
                            if (t.Minutes < 10)
                            {
                                answer += "0";
                            }
                            answer += t.Minutes;
                        }
                        else
                        {
                            answer += ":00";
                        }
                        //if (t.Seconds > 0)
                        //{
                        //    answer += ":";
                        //    if (t.Seconds < 10)
                        //    {
                        //        answer += "0";
                        //    }
                        //    answer += t.Seconds;
                        //}
                        //else
                        //{
                        //    answer += ":00";
                        //}

                        return answer;
                    }
                }
                else
                {
                    return "";
                }
            }

            return "ไม่กำหนด";
        }

        public DataTable newConvertToTimeString(
            DataTable report,
            int currentRow,
            string stringDateTimeForm,
            string stringDateTimeTo,
            string hour,
            string minute,
            string second
            )
        {
            if (!String.IsNullOrEmpty(stringDateTimeForm) && !String.IsNullOrEmpty(stringDateTimeTo))
            {
                DateTime DateTimeForm = ObjectUtil.ConvertDateTimeDBToDateTime(stringDateTimeForm);
                DateTime DateTimeTo = ObjectUtil.ConvertDateTimeDBToDateTime(stringDateTimeTo);

                TimeSpan ts = DateTimeTo - DateTimeForm;
                string time = ts.TotalSeconds.ToString();

                if (!string.IsNullOrEmpty(time))
                {
                    double xTime = 0;
                    double.TryParse(time, out xTime);

                    if (xTime > 0)
                    {
                        TimeSpan t = TimeSpan.FromSeconds(xTime);

                        if (t.Hours > 0)
                        {
                            report.Rows[currentRow][hour] = ((t.Days * 24) + t.Hours).ToString() + " ชั่วโมง";
                        }
                        if (t.Minutes > 0)
                        {
                            report.Rows[currentRow][minute] = (t.Minutes).ToString() + " นาที";
                        }
                        if (t.Seconds > 0)
                        {
                            report.Rows[currentRow][second] = (t.Seconds).ToString() + " วินาที";
                        }

                        return report;
                    }
                }
                else
                {
                    report.Rows[currentRow][hour] = "";
                    report.Rows[currentRow][minute] = "";
                    report.Rows[currentRow][second] = "";
                    return report;
                }
            }
            else
            {
                report.Rows[currentRow][hour] = "ไม่กำหนด";
                report.Rows[currentRow][minute] = "ไม่กำหนด";
                report.Rows[currentRow][second] = "ไม่กำหนด";
            }

            return report;
        }

        public DataTable ticketreport_calculate_time(DataTable report)
        {
            //mttn time respondingdate - opendate
            string OpenDate;
            string responding_date;
            // mtrs time resolveddate - opendate
            string Resolved;

            // closed date
            string close_date;
            try
            {
                //add columns
                report.Columns.Add("MTTNHour", typeof(string));
                report.Columns.Add("MTTNMinute", typeof(string));
                report.Columns.Add("MTTNSecond", typeof(string));
                report.Columns.Add("MTRSHour", typeof(string));
                report.Columns.Add("MTRSMinute", typeof(string));
                report.Columns.Add("MTRSSecond", typeof(string));
                for (int index = 0; index < report.Rows.Count; index++)
                {
                    OpenDate = (string)report.Rows[index]["Open_Date"];
                    //mttn
                    responding_date = (string)report.Rows[index]["Responding_Date"];
                    //mtrs
                    Resolved = (string)report.Rows[index]["Resolved_Date"];

                    //
                    close_date = (string)report.Rows[index]["Closed_Date"];
                    //get mttn
                    report = newConvertToTimeString(
                        report,
                        index,
                        OpenDate,
                        responding_date,
                        "MTTNHour",
                        "MTTNMinute",
                        "MTTNSecond"
                        );

                    //get mtrs
                    report = newConvertToTimeString(
                        report,
                        index,
                        OpenDate,
                        Resolved,
                        "MTRSHour",
                        "MTRSMinute",
                        "MTRSSecond"
                        );

                    report.Rows[index]["Open_Date"] = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(OpenDate);
                    report.Rows[index]["Responding_Date"] = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(responding_date);
                    report.Rows[index]["Resolved_Date"] = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Resolved);
                    report.Rows[index]["Closed_Date"] = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(close_date);
                }
            }
            catch (Exception e1)
            {
                System.Diagnostics.Debug.WriteLine(e1);
            }
            return report;
        }

        /// <summary>
        /// create new column(mttntime, mtrstime) in datatable
        /// and calculate use time
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public DataTable ticketreport_calculate_timeV2(DataTable report)
        {
            //mttn time respondingdate - opendate
            string OpenDate;
            string responding_date;
            // mtrs time resolveddate - opendate
            string Resolved;

            // closed date
            string close_date;
            try
            {
                //add columns
                //report.Columns.Add("MTTNHour", typeof(string));
                //report.Columns.Add("MTTNMinute", typeof(string));
                //report.Columns.Add("MTTNSecond", typeof(string));
                //report.Columns.Add("MTRSHour", typeof(string));
                //report.Columns.Add("MTRSMinute", typeof(string));
                //report.Columns.Add("MTRSSecond", typeof(string));
                if (!report.Columns.Contains("MTTNTime"))
                {
                    report.Columns.Add("MTTNTime", typeof(string));
                }

                if (!report.Columns.Contains("MTRSTime"))
                {
                    report.Columns.Add("MTRSTime", typeof(string));
                }

                for (int index = 0; index < report.Rows.Count; index++)
                {
                    OpenDate = (string)report.Rows[index]["Open_Date"];
                    //mttn
                    responding_date = (string)report.Rows[index]["Responding_Date"];
                    //mtrs
                    Resolved = (string)report.Rows[index]["Resolved_Date"];

                    //
                    close_date = (string)report.Rows[index]["Closed_Date"];
                    //get mttn
                    //report = newConvertToTimeString(
                    //    report,
                    //    index,
                    //    OpenDate,
                    //    responding_date,
                    //    "MTTNHour",
                    //    "MTTNMinute",
                    //    "MTTNSecond"
                    //    );
                    //if (!string.IsNullOrEmpty(OpenDate) && !string.IsNullOrEmpty(responding_date))
                    report.Rows[index]["MTTNTime"] = ConvertToTimeString(OpenDate, responding_date);

                    //get mtrs
                    //report = newConvertToTimeString(
                    //    report,
                    //    index,
                    //    OpenDate,
                    //    Resolved,
                    //    "MTRSHour",
                    //    "MTRSMinute",
                    //    "MTRSSecond"
                    //    );
                    if (!string.IsNullOrEmpty(OpenDate) && !string.IsNullOrEmpty(Resolved))
                        report.Rows[index]["MTRSTime"] = ConvertToTimeString(OpenDate, Resolved);

                    if (!string.IsNullOrEmpty(OpenDate))
                        report.Rows[index]["Open_Date"] = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(OpenDate);
                    if (!string.IsNullOrEmpty(responding_date))
                        report.Rows[index]["Responding_Date"] = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(responding_date);
                    if (!string.IsNullOrEmpty(Resolved))
                        report.Rows[index]["Resolved_Date"] = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Resolved);
                    if (!string.IsNullOrEmpty(close_date))
                        report.Rows[index]["Closed_Date"] = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(close_date);
                }
            }
            catch (Exception e1)
            {
                System.Diagnostics.Debug.WriteLine(e1);
            }
            return report;
        }

        public DataTable incidentNoFormater(string SID, string CompanyCode, DataTable dt, string displayIncidentNo = "IncidentNo")
        {
            try
            {
                List<string> listTicket = dt.AsEnumerable().Select(s => Convert.ToString(s[displayIncidentNo])).ToList();
                DataTable dtDocType = new DBService().selectDataFocusone(
                    @"select Doctype from cs_servicecall_header where CallerID in ('" + string.Join("', '", listTicket) + "')"
                );
                List<string> listDocType = dtDocType.AsEnumerable().Select(s => Convert.ToString(s["Doctype"])).ToList();

                DataTable dtPrefix = ServiceTicketLibrary.GetInstance().getDataPrefixDocType(SID, CompanyCode, listDocType);
                foreach (DataRow dr in dt.Rows)
                {
                    string TicketNo = Convert.ToString(dr[displayIncidentNo]);
                    string ticketNoDisplay = "";
                    DataRow[] drr = dtPrefix.Select("'" + TicketNo + "' like PrefixCode + '%'");

                    if (drr.Length > 0)
                    {
                        string prefix = drr[0]["PrefixCode"].ToString();

                        ticketNoDisplay = TicketNo;
                        //for (int i = 0; i < prefix.Length; i++)
                        //{
                        //    ticketNoDisplay = ticketNoDisplay.Replace(prefix[i].ToString(), "");
                        //}

                        ticketNoDisplay = ServiceTicketLibrary.GetInstance().ReplaceTicketNumberToDisplay(prefix, ticketNoDisplay);// prefix + Convert.ToInt32(ticketNoDisplay);
                    }
                    else
                    {
                        ticketNoDisplay = TicketNo;
                    }

                    dr[displayIncidentNo] = ticketNoDisplay;
                }
            }
            catch (Exception e1)
            {
                System.Diagnostics.Debug.WriteLine(e1.ToString());
            }
            return dt;
        }
    }
}