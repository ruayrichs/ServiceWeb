using Agape.Lib.DBService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ServiceWeb.Service
{
    public class LoginService
    {
        private static LoginService _instance;
        public static LoginService getInstance()
        {
            if (_instance == null)
            {
                _instance = new LoginService();
            }
            return _instance;
        }
        DBService dbService = new DBService();

        public string getUrlDefaultByBranchCode(string SID, string CompanyCode, string BranchCode)
        {
            string UrlDefault = "";
            string sql = @"select * from master_branch_mapping_User where SID='" + SID + "' and CompanyCode='" + CompanyCode + "' and BranchCode='" + BranchCode + "'";
            
            DataTable dt = dbService.selectDataFocusone(sql);

            if(dt.Rows.Count > 0)
            {
                UrlDefault = dt.Rows[0]["UrlDefault"].ToString();
            }

            return !string.IsNullOrEmpty(UrlDefault) ? UrlDefault : "~/Default.aspx";
        }
    }
}