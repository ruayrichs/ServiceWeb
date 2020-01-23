using agape.lib.fobgp.service;
using Agape.FocusOne.Utilities;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace ServiceWeb.Util
{
    public class ReportUtils
    {
        public static ReportDocument getReportDocument2(string p_sid, string p_dllName, string p_reportName, string p_objectiveGroup, string p_objectiveCode)
        {
            ReportDocument temp = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            bool isExternalForm = false;

            if (!string.IsNullOrEmpty(p_objectiveGroup) && !string.IsNullOrEmpty(p_objectiveCode))
            {
                isExternalForm = checkExternalForm(p_sid, p_objectiveGroup, p_objectiveCode);
            }

            if (isExternalForm)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"dllReport\ReportFile\" + p_reportName + ".rpt";

                if (System.IO.File.Exists(filePath))
                {
                    temp.Load(filePath);
                }
                else
                {
                    throw new Exception("External crystal report file \"" + p_reportName + ".rpt\" is not found.");
                }
            }
            else
            {
                Assembly report = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + @"dllReport\" + p_dllName + ".dll");
                Type[] type = report.GetExportedTypes();
                bool _found = false;

                foreach (Type t in type)
                {
                    if (t.Name.Equals(p_reportName))
                    {
                        Object o = Activator.CreateInstance(t);
                        temp = (ReportDocument)o;
                        _found = true;
                    }
                    if (_found)
                    {
                        break;
                    }
                }
            }

            return temp;
        }

        public static DataSet getReportSchema2(string p_dllName, string p_reportName)
        {
            try
            {
                DataSet temp = new DataSet();
                Assembly report = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + @"dllReport\" + p_dllName + ".dll");
                Type[] type = report.GetExportedTypes();
                bool _found = false;
                foreach (Type t in type)
                {
                    if (t.Name.Equals(p_reportName))
                    {
                        Object o = Activator.CreateInstance(t);
                        temp = (DataSet)o;
                        _found = true;
                    }
                    if (_found)
                    {
                        break;
                    }
                }
                return temp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable getObjectiveList(string sid, string p_objectivegroup, string documentType, string companyCode)
        {
            fobgpService serviceFobgp = new fobgpService();


            StringBuilder sqlcmd = new StringBuilder();
            sqlcmd.AppendLine("select b.* , (select count(c.IDLOG) from LOG_REPORT c where c.OBJECTIVEGROUP = a.OBJECTIVEGROUP and c.OBJECTIVECODE = a.OBJECTIVECODE) as NO ");
            sqlcmd.AppendLine("from REPORT_MAPPING_FORM_docType a  ");
            sqlcmd.AppendLine("inner join REPORT_MAPPING_FORM b on a.SID =b.SID and a.ObjectiveGroup = b.OBJECTIVEGROUP and a.ObjectiveCode = b.OBJECTIVECODE ");
            sqlcmd.AppendLine("where a.SID ='" + sid + "' and a.OBJECTIVEGROUP='" + p_objectivegroup + "' and a.DocTypeCode='" + documentType + "' and CompanyCode='" + companyCode + "'");
            DataTable dt = new DataTable();
            DataSet ds = serviceFobgp.selectData(sqlcmd.ToString());
            if (ds.Tables.Count > 0)
            {
                if (!ds.Tables[0].Columns.Contains("PRINTDATE"))
                {
                    ds.Tables[0].Columns.Add("PRINTDATE");
                }
                if (!ds.Tables[0].Columns.Contains("PRINTTIME"))
                {
                    ds.Tables[0].Columns.Add("PRINTTIME");
                }
                if (!ds.Tables[0].Columns.Contains("PRINTBY"))
                {
                    ds.Tables[0].Columns.Add("PRINTBY");
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sqlcmd = new StringBuilder();
                    sqlcmd.AppendLine("select b.PrintDate,b.PrintTIME,b.PRINTBY from REPORT_MAPPING_FORM_DocType a ");
                    sqlcmd.AppendLine("left outer join LOG_REPORT b on a.SID = b.SID and a.OBJECTIVEGROUP = b.OBJECTIVEGROUP and a.OBJECTIVECODE = b.OBJECTIVECODE ");
                    sqlcmd.AppendLine("where b.IDLOG =(select MAX(d.idlog) from LOG_REPORT d where d.OBJECTIVEGROUP = a.OBJECTIVEGROUP and d.OBJECTIVECODE = a.OBJECTIVECODE) ");
                    sqlcmd.AppendLine(" and a.OBJECTIVEGROUP='" + dr["OBJECTIVEGROUP"].ToString() + "' and a.OBJECTIVECODE='" + dr["OBJECTIVECODE"].ToString() + "' ");
                    DataTable dt_result = new DataTable();
                    DataSet ds2 = serviceFobgp.selectData(sqlcmd.ToString());
                    if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        dr["PRINTDATE"] = Validation.Convert2DateDisplay(ds2.Tables[0].Rows[0]["PRINTDATE"].ToString());
                        dr["PRINTTIME"] = Validation.Convert2TimeDisplay(ds2.Tables[0].Rows[0]["PRINTTIME"].ToString());
                        dr["PRINTBY"] = ds2.Tables[0].Rows[0]["PRINTBY"].ToString();
                    }
                }
                dt = ds.Tables[0];
            }
            return dt;

        }

        private static bool checkExternalForm(string sid, string objectiveGroup, string objectiveCode)
        {
            fobgpService serviceFobgp = new fobgpService();

            bool res = false;

            string sql = "SELECT [RPTISFILE] FROM [REPORT_MAPPING_FORM] WHERE [SID]='" + sid + "' AND [OBJECTIVEGROUP]='" + objectiveGroup + "' AND [OBJECTIVECODE]='" + objectiveCode + "'";

            DataTable dt = serviceFobgp.selectData(sql).Tables[0];

            if (dt.Rows.Count > 0)
            {
                bool.TryParse(dt.Rows[0]["RPTISFILE"].ToString(), out res);
            }

            return res;
        }
    }
}