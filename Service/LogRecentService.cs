using Agape.Lib.DBService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ServiceWeb.Service
{
    public class LogRecentService
    {
        private static LogRecentService _instance;
        public static LogRecentService getInstance()
        {
            if (_instance == null)
            {
                _instance = new LogRecentService();
            }
            return _instance;
        }
        DBService dbService = new DBService();

        public DataTable getLogRecent(string SID, string CompanyCode, string Category, string EmployeeCode)
        {
            string sql = @"select top 9 head.Category,head.Docnumber,head.Doctype,head.Fiscalyear,item.Created_On,item.Url from Crm_Log_Recent head
                            outer apply
                            (
                            select top 1 * from Crm_Log_Recent d
                            where d.Category = head.Category and d.Docnumber = head.Docnumber
                            order by d.Created_On desc
                            )item
                            where head.SID='" + SID + "' and head.CompanyCode='" + CompanyCode + "' and head.Created_By='" + EmployeeCode + "'";

                    if (!string.IsNullOrEmpty(Category))
                    {
                        sql += "and head.Category='" + Category + "'";
                    }

                    sql += "group by head.Category,head.Docnumber,head.Doctype,head.Fiscalyear,item.Created_On,item.Url order by Created_On desc";

            DataTable dt = dbService.selectDataFocusone(sql);

            dt.Columns.Add("Customer", typeof(System.String)).DefaultValue = "";
            dt.Columns.Add("Remark", typeof(System.String)).DefaultValue = "";

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Category"].ToString() == "SalesTransaction")
                {
                    sql = "select * from sd_so_header where SID='" + SID + "' and SaleDocument='" + dr["Docnumber"] + 
                        "' and FiscalYear='" + dr["Fiscalyear"] + "' and Stypecode='" + dr["Doctype"] + "'";

                    DataTable dtSO = dbService.selectDataFocusone(sql);

                    if (dtSO.Rows.Count > 0)
                    {
                        dr["Customer"] = dtSO.Rows[0]["SoldToParty"] + " " + dtSO.Rows[0]["SoldToDesc"];
                        dr["Remark"] = dtSO.Rows[0]["HeaderText"].ToString();
                    }
                }
            }

            return dt;
        }

        public void SaveLogRecent(string SID, string CompanyCode, string LogCode, string Category, string Docnumber, string Doctype, string Fiscalyear, string Url, string Event, string CreatedBy, string CreatedOn)
        {
            string sql = @"insert into Crm_Log_Recent (SID,CompanyCode,LogCode,Category,Docnumber,Doctype,Fiscalyear,Url,Event,Created_By,Created_On) values ('" + SID + "','" + CompanyCode + @"'
                            ,'" + LogCode + "','" + Category + "','" + Docnumber + "','" + Doctype + "','" + Fiscalyear + "','" + Url + "','" + Event + "','" + CreatedBy + "','" + CreatedOn + "')";

            dbService.selectDataFocusone(sql);
        }
    }
}