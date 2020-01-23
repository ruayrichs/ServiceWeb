using Agape.FocusOne.Utilities;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace ServiceWeb.API
{
    public class ExportExcelAPI : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            DataTable dtExport = context.Session["report.excel.Report_Excel_Export_Datatable"] as DataTable;
            foreach (DataRow dr in dtExport.Rows)
            {
                for (int i = 0; i < dr.ItemArray.Count(); i++)
                {
                    string dataValue = Convert.ToString(dr[i]);
                    if (!string.IsNullOrEmpty(dataValue))
                    {
                        dr[i] = ReplaceHexadecimalSymbols(dataValue);
                    }
                }
            }

            string reportName = "";
            string filename = "";

            if (!string.IsNullOrEmpty((context.Session["report.excel.Report_Excel_Export_Name"] as string)))
            {
                reportName = (context.Session["report.excel.Report_Excel_Export_Name"] as string);
                filename = reportName + "_" + Validation.getCurrentServerStringDateTime();
            }
            else
            {
                reportName = "ReportExcelExport";
                filename = reportName + "_" + Validation.getCurrentServerStringDateTime();
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dtExport, reportName);

                ws.Columns().AdjustToContents();

                string myName = context.Server.UrlEncode(filename + ".xlsx");
                MemoryStream stream = GetStream(wb);// The method is defined below
                context.Response.Clear();
                context.Response.Buffer = true;
                context.Response.AddHeader("content-disposition", "attachment; filename=" + myName);
                context.Response.ContentType = "application/vnd.ms-excel";
                context.Response.BinaryWrite(stream.ToArray());
                context.Response.End();
            }

            context.Session["report.excel.Report_Excel_Export_Datatable"] = null;
            context.Session["report.excel.Report_Excel_Export_Name"] = null;
        }

        public MemoryStream GetStream(XLWorkbook excelWorkbook)
        {
            MemoryStream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }
    }
}