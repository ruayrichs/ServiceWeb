using agape.lib.web.configuration.utils;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceWeb.Service
{
    public class LinkWebErrorLog
    {
        public static void SaveErrorLog(Exception objErr)
        {
            try
            {
                string CreatedOn = Validation.getCurrentServerStringDateTimeMillisecond();

                ErrorLogs err = new ErrorLogs();
                err.Message = objErr.Message.Replace("'", "\"");
                err.Source = objErr.Source.Replace("'", "\"");

                err.EmployeeCode = string.IsNullOrEmpty(ERPWAuthentication.EmployeeCode) ? "EMPTY_EMPCODE" : ERPWAuthentication.EmployeeCode;
                err.LinkID = string.IsNullOrEmpty("") ? "EMPTY_LINKID" : "";
                err.SID = string.IsNullOrEmpty(ERPWAuthentication.SID) ? "EMPTY_SID" : ERPWAuthentication.SID;

                err.CreatedOn = CreatedOn;
                err.CreatedOnYear = CreatedOn.Substring(0, 4);
                err.CreatedOnMonth = CreatedOn.Substring(4, 2);
                err.CreatedOnDay = CreatedOn.Substring(6, 2);
                err.CreatedOnTime = CreatedOn.Substring(8, 6);

                err.TargetSite = HttpContext.Current.Request.Url.ToString();

                InsertErrorLogs(err);
            }
            catch { }
        }

        private static void InsertErrorLogs(ErrorLogs err)
        {
            string sql = @"INSERT INTO [" + WebConfigHelper.getDatabaseSNAName() + @"].[dbo].[SNA_LINK_WEB_ERROR_LOG]
                               ([Message]
                               ,[Source]
                               ,[TargetSite]
                               ,[CreatedOn]
                               ,[CreatedOnYear]
                               ,[CreatedOnMonth]
                               ,[CreatedOnDay]
                               ,[CreatedOnTime]
                               ,[SID]
                               ,[LinkID]
                               ,[EmployeeCode]
                               ,[Remark])
                         VALUES (";

            sql += "'" + err.Message + "',";
            sql += "'" + err.Source + "',";
            sql += "'" + err.TargetSite + "',";
            sql += "'" + err.CreatedOn + "',";
            sql += "'" + err.CreatedOnYear + "',";
            sql += "'" + err.CreatedOnMonth + "',";
            sql += "'" + err.CreatedOnDay + "',";
            sql += "'" + err.CreatedOnTime + "',";
            sql += "'" + err.SID + "',";
            sql += "'" + err.LinkID + "',";
            sql += "'" + err.EmployeeCode + "',";
            sql += "'" + err.Remark + "');";

            new DBService().executeSQL(sql);
        }
        
        [Serializable]
        private class ErrorLogs
        {
            public string Message { get; set; }
            public string Source { get; set; }
            public string TargetSite { get; set; }
            public string CreatedOn { get; set; }
            public string CreatedOnYear { get; set; }
            public string CreatedOnMonth { get; set; }
            public string CreatedOnDay { get; set; }
            public string CreatedOnTime { get; set; }
            public string SID { get; set; }
            public string LinkID { get; set; }
            public string EmployeeCode { get; set; }
            public string Remark { get; set; }

        }
    }

}