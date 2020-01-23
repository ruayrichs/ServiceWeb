using ERPW.Lib.F1WebService.ICMUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ServiceWeb.Service
{
    public class WSHelper
    {
        private static DataSet GetWhereCondition(String SID, String Where)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("key", typeof(string));
            dt.Columns.Add("value", typeof(string));
            DataRow dr;

            if (Where.Length > 0)
            {
                if (Where.IndexOf('#') == 0)
                {
                    string _where = Where.TrimStart('#');
                    dr = dt.NewRow();
                    dr["key"] = "SPECIALWHEREICM";
                    dr["value"] = _where;
                    dt.Rows.Add(dr);
                    ds.Tables.Add(dt);
                    return ds;
                }
            }
            if (!string.IsNullOrEmpty(SID))
            {
                dr = dt.NewRow();
                dr["key"] = "SID";
                dr["value"] = SID;
                dt.Rows.Add(dr);
            }
            String[] where = Where.Split('~');
            foreach (String st in where)
            {
                if (!string.IsNullOrEmpty(st))
                {
                    String[] param = st.Split('=');
                    if (param[0].ToString().Trim().ToUpper() != "SID")
                    {

                        dr = dt.NewRow();
                        dr["key"] = param[0].ToString().Trim().ToUpper();
                        dr["value"] = param[1].ToString().Trim();
                        dt.Rows.Add(dr);
                    }
                }
            }
            ds.Tables.Add(dt);
            return ds;
        }

        public static DataTable GetSearchData(String SID, String SearchID, String WhereCondition)
        {
            try
            {
                ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();

                Object[] objParam = new Object[] { "0600008", SearchID };
                DataSet[] objDataSet = new DataSet[] { GetWhereCondition(SID, WhereCondition) };
                DataSet objOutput = ICMService.ICMDataSetInvoke(objParam, objDataSet);
                return (DataTable)objOutput.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}