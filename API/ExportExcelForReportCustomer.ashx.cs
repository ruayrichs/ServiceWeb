using Agape.FocusOne.Utilities;
using ClosedXML.Excel;
using ERPW.Lib.Master.Entity;
using ERPW.Lib.Service.Entity;
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
    /// Summary description for ExportExcelForReportCustomer
    /// </summary>
    public class ExportExcelForReportCustomer : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            DataTable dtExport = context.Session["report.excel.Report_Excel_Export_Datatable"] as DataTable;
            ReportCustomerDetail customerProfile = context.Session["report.excel.Report_Excel_Export_CustomerProfile"] as ReportCustomerDetail;
            //System.Diagnostics.Debug.WriteLine("[api]report on: " + customerProfile.customerReportMonth);
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
                var ws = wb.Worksheets.Add(reportName);

                //customer detail
                ws.Cell(1, 1).Value = "ชื่อบริษัท";
                ws.Cell(2, 1).Value = "สถานที่";
                ws.Cell(3, 1).Value = "ประเภทบริการ";
                ws.Cell(4, 1).Value = "Carrier/Curcuit ID";
                ws.Cell(5, 1).Value = "รายงานประจำเดือน";
                //
                ws.Cell(1, 2).Value = customerProfile.customerName;
                ws.Cell(2, 2).Value = customerProfile.customerAddress;
                ws.Cell(3, 2).Value = customerProfile.customerEquipment;
                ws.Cell(4, 2).Value = customerProfile.customerCCID;
                ws.Cell(5, 2).Value = customerProfile.customerReportMonth;
                //
                ws.Cell(7, 1).Value = "Incident No.";
                ws.Cell(7, 2).Value = "Open Date";
                ws.Cell(7, 3).Value = "Resolved Date";
                ws.Cell(7, 4).Value = "Close Log";
                ws.Cell(7, 5).Value = "Duration Time";
                for(int index = 1; index <= 5; index++)
                {
                    ws.Cell(7, index).Style.Font.Bold = true;
                    ws.Cell(7, index).Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1);
                    ws.Cell(7, index).Style.Font.FontColor= XLColor.White;
                }
                //var ws = wb.Worksheets.Add(dtExport, reportName);
                ws.Cell(8, 1).InsertData(dtExport.AsEnumerable());
                //wb.Worksheets.Add(dtExport);                
                
                ws.Column(4).AdjustToContents();
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
    }
}