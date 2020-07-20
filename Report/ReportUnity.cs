using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ERPW.Lib.Service;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

                    // stoptime
                    string StopDate;
                    string RestartDate;
                    string assignDateTime;
                    string resolveDateTime;
                    double sumStopSeconds = 0;

                    StopDate = report.Rows[index]["StopDate"].ToString();
                    RestartDate = report.Rows[index]["RestartDate"].ToString();

                    assignDateTime = (string)report.Rows[index]["assignDateTime"];
                    resolveDateTime = (string)report.Rows[index]["resolveDateTime"];

                    string stopTime = "";
                    string totalTime = "";
                    string totalTimeWithoutStop = "";
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

                    DateTime stop = ObjectUtil.ConvertDateTimeDBToDateTime(StopDate);
                    DateTime restart = ObjectUtil.ConvertDateTimeDBToDateTime(RestartDate);

                    TimeSpan ts = restart - stop;

                    sumStopSeconds += ts.TotalSeconds;

                    stopTime = ConvertToTime(sumStopSeconds.ToString(), true);


                    #region Calculate total time
                    if (assignDateTime != "" && resolveDateTime != "")
                    {
                        DateTime assign = ObjectUtil.ConvertDateTimeDBToDateTime(assignDateTime);
                        DateTime resolve = ObjectUtil.ConvertDateTimeDBToDateTime(resolveDateTime);

                        TimeSpan tss = resolve - assign;

                        totalTime = ConvertToTime(tss.TotalSeconds.ToString(), true);
                        totalTimeWithoutStop = ConvertToTime((tss.TotalSeconds - sumStopSeconds).ToString(), true);

                        report.Rows[index]["MTRSWTime"] = totalTimeWithoutStop;
                    }
                    #endregion


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

        Agape.Lib.DBService.DBService DBService = new Agape.Lib.DBService.DBService();
        public DataTable ticketreport_add_calculate_stop_and_overdue_time(string SID, string CompanyCode, DataTable report)
        {
            report.Columns.Add("StopTime", typeof(string));
            report.Columns.Add("OverdueTime", typeof(string));
     
            string sqlStop = "SELECT * FROM cs_servicecall_stop_timer WHERE SID = '" + SID + "' AND CompanyCode = '" + CompanyCode + "' ORDER BY xLineNo ASC";
            DataTable dtStop = DBService.selectDataFocusone(sqlStop);

            string sql = @"
                SELECT * FROM (select xa.ServiceDocNo,  xb.EndDateTime,  xb.MainDelegate, xb.TicketCode
										from (
										  select SNAID, DOCYEAR, ServiceDocNo, max(Tier) as Tier
										  from CRM_SERVICECALL_MAPPING_ACTIVITY
										  where SNAID = '" + CompanyCode + @"'
										  group by SNAID, DOCYEAR, ServiceDocNo
										) a
										inner join CRM_SERVICECALL_MAPPING_ACTIVITY xa
										  on  xa.SNAID = a.SNAID 
										  AND xa.DOCYEAR = a.DOCYEAR 
										  AND xa.ServiceDocNo = a.ServiceDocNo 
										  AND xa.Tier = a.Tier
  
										left join ticket_service_header xb
										  on xa.AOBJECTLINK = xb.TicketCode
										  and xb.SID = '" + SID + @"' 
										  AND xb.CompanyCode = '" + CompanyCode + @"'
				                          AND xb.EndDateTime != '') z ";
            
            DataTable dtTicket = DBService.selectDataFocusone(sql);

            foreach (DataRow dr in report.Rows)
            {
                string ticketEndDateTime = "";
                string resolveDateTime = "";
                string overduetime = "";
                string stopTime = "";
                string ticketNO = "";
                double sumStopSeconds = 0;

                resolveDateTime = dr["resolveDateTime"].ToString();
                ticketNO = dr["IncidentNO"].ToString();

                #region Get stop

                DataTable dtStopquery = new DataTable();

                var bfDtStopQuery = dtStop.AsEnumerable()
                            .Where(r => r.Field<string>("CallerID") == ticketNO);

                if (bfDtStopQuery.Any())
                {
                    dtStopquery = bfDtStopQuery.CopyToDataTable();

                    if (dtStopquery.Rows.Count > 0)
                    {
                        DataRow[] drrStopNotStart = dtStop.Select("RestartDate = ''");

                        if (drrStopNotStart.Length > 0)
                        {
                            string currentDateTime = Validation.getCurrentServerStringDateTime();

                            drrStopNotStart[0]["RestartDate"] = currentDateTime.Substring(0, 8);
                            drrStopNotStart[0]["RestartTime"] = currentDateTime.Substring(8, 6);
                        }

                        foreach (DataRow drStop in dtStopquery.Rows)
                        {
                            DateTime stop = ObjectUtil.ConvertDateTimeDBToDateTime(drStop["StopDate"].ToString() + drStop["StopTime"].ToString());
                            DateTime restart = ObjectUtil.ConvertDateTimeDBToDateTime(drStop["RestartDate"].ToString() + drStop["RestartTime"].ToString());

                            TimeSpan ts = restart - stop;

                            sumStopSeconds += ts.TotalSeconds;
                        }
                    }

                    stopTime = ConvertToTime(sumStopSeconds.ToString(), true);

                }

                #endregion

                #region Calculate Overdue time
                DataTable dtTicketQuery = new DataTable();

                var bfdtTicketQuery = dtTicket.AsEnumerable()
                            .Where(r => r.Field<string>("ServiceDocNo") == ticketNO);

                if (bfdtTicketQuery.Any())
                {
                    dtTicketQuery = bfdtTicketQuery.CopyToDataTable();

                    if (dtTicketQuery.Rows.Count > 0)
                    {
                        ticketEndDateTime = dtTicketQuery.Rows[0].Field<String>("EndDateTime").ToString();


                        if (ticketEndDateTime != "" && resolveDateTime != "")
                        {
                            DateTime overdue = ObjectUtil.ConvertDateTimeDBToDateTime(ticketEndDateTime);
                            DateTime resolve = ObjectUtil.ConvertDateTimeDBToDateTime(resolveDateTime);

                            TimeSpan ts = resolve - overdue;
                            if (ts.TotalSeconds <= 0)
                            {
                                overduetime = "0";
                            }
                            else
                            {
                                overduetime = ConvertToTime(ts.TotalSeconds.ToString(), true);
                            }
                        }

                    }
                }
                #endregion

                dr["OverdueTime"] = overduetime;
                dr["StopTime"] = stopTime;

            };

            return report;
        }

        public string ConvertToTime(string time, bool returnEmpty)
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

            if (returnEmpty)
            {
                return "";
            }

            return "ไม่กำหนด";
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