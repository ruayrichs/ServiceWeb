using Agape.FocusOne.Utilities;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ServiceWeb.API
{
    /// <summary>
    /// Summary description for ExportExcelCI
    /// </summary>
    public class ExportExcelCI : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            DataTable dtExport = context.Session["Export_Excel_CI_Datatable"] as DataTable;
            string reportName = "";
            string filename = "";

            if (!string.IsNullOrEmpty((context.Session["Export_Excel_CI_Name"] as string)))
            {
                reportName = (context.Session["Export_Excel_CI_Name"] as string);
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

            context.Session["Export_Excel_CI_Datatable"] = null;
            context.Session["Export_Excel_CI_Name"] = null;
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
    }
}