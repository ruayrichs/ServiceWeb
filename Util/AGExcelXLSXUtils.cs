using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ServiceWeb.Util
{
    public class AGExcelXLSXUtils
    {


        public static Stream WriteFileXLSXOmlyToStream(DataTable P_TableToWrite, string PathFile, string lc_FileType, String p_EnCode, bool includeHeader)
        {
            Stream SpreadsheetStream = new MemoryStream();

            SpreadsheetDocument xl = SpreadsheetDocument.Create(SpreadsheetStream, SpreadsheetDocumentType.Workbook);

            WorkbookPart wbp = xl.AddWorkbookPart();
            WorksheetPart wsp = wbp.AddNewPart<WorksheetPart>();
            FileVersion fv = new FileVersion();
            fv.ApplicationName = "Microsoft Office Excel";
            Workbook _wb = new Workbook();
            Worksheet _ws = new Worksheet();
            SheetData _sd = new SheetData();

            UInt32 index = 1;
            if (includeHeader)
            {
                _sd.Append(CreateColumnHeaderRawData(1, P_TableToWrite.Columns));
                index = 2;
            }

            foreach (DataRow List in P_TableToWrite.Rows)
            {
                Row r = new Row();
                r.RowIndex = index;
                foreach (DataColumn Value in P_TableToWrite.Columns)
                {
                    bool isNum = false;
                    string value = "";

                    if (Value.DataType == typeof(System.Decimal) || Value.DataType == typeof(System.Double))
                    {
                        isNum = true;
                    }
                    Cell c = new Cell();
                    value = List[Value.ColumnName].ToString();
                    if (isNum)
                    {
                        c.CellValue = new CellValue(value);
                    }
                    else
                    {
                        c.DataType = CellValues.InlineString;
                        InlineString x = new InlineString();
                        x.Text = new Text(value);
                        c.InlineString = x;
                    }

                    r.Append(c);
                }
                _sd.Append(r);
                index++;
            }

            SheetFormatProperties sheetFormatProperties = new SheetFormatProperties()
            {
                DefaultColumnWidth = 24,
                DefaultRowHeight = 15D
            };
            _ws.Append(sheetFormatProperties);
            _ws.Append(_sd);
            wsp.Worksheet = _ws;
            wsp.Worksheet.Save();
            Sheets sheets = new Sheets();
            Sheet sheet = new Sheet();
            sheet.Name = "Sheet1";
            sheet.SheetId = 1;
            sheet.Id = wbp.GetIdOfPart(wsp);
            sheets.Append(sheet);
            _wb.Append(fv);
            _wb.Append(sheets);

            xl.WorkbookPart.Workbook = _wb;
            xl.WorkbookPart.Workbook.Save();
            xl.Close();

            return SpreadsheetStream;
        }

        private static void initStyle(WorkbookPart workbookPart)
        {
            var stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            stylesPart.Stylesheet = new Stylesheet();
            stylesPart.Stylesheet.Fills = new Fills();

            // Create cell format list
            stylesPart.Stylesheet.CellFormats = new CellFormats();
            // empty one for index 0, seems to be required
            stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat());

            // Create Light Green fill
            var formatLightGreen = new PatternFill() { PatternType = PatternValues.Solid };
            formatLightGreen.ForegroundColor = new ForegroundColor { Rgb = HexBinaryValue.FromString("F1F8E0") };
            formatLightGreen.BackgroundColor = new BackgroundColor { Indexed = 64 };

            // Append fills to list
            stylesPart.Stylesheet.Fills.AppendChild(new Fill { PatternFill = formatLightGreen }); // Green gets fillid = 3
            stylesPart.Stylesheet.Fills.Count = 1;


            // cell format for failed tests, Styleindex = 1, Red fill and bold text
            stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 1, BorderId = 2, FillId = 2, ApplyFill = true }).AppendChild(new Alignment { Horizontal = HorizontalAlignmentValues.Center });
            stylesPart.Stylesheet.Save();
            //row.AppendChild(new Cell() { CellValue = new CellValue(element), DataType = CellValues.String, StyleIndex = 1 });
        }

        private static Row CreateColumnHeaderRawData(UInt32 index, DataColumnCollection resultValue)
        {

            Row r = new Row();
            r.RowIndex = index;

            Cell c;
            foreach (DataColumn Header in resultValue)
            {
                string strHeader = "";

                strHeader = Header.ColumnName;

                c = new Cell();
                c.DataType = CellValues.String;
                c.CellValue = new CellValue(strHeader);
                r.Append(c);
            }

            return r;
        }

        public static DataSet convertExcelToDataTable(Stream strm, string fileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(strm, false))
            {
                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> IEsheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                List<Sheet> sheets = new List<Sheet>(IEsheets);
                string relationshipId = ((Sheet)sheets[0]).Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> IErows = sheetData.Descendants<Row>();
                List<Row> rows = new List<Row>(IErows);

                foreach (Cell cell in ((Row)rows[0]))
                {
                    dt.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                }

                foreach (Row row in rows) //this will also include your header row...
                {
                    DataRow tempRow = dt.NewRow();

                    int columnIndex = 0;
                    foreach (Cell cell in row.Descendants<Cell>())
                    {
                        // Gets the column index of the cell with data
                        int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                        cellColumnIndex--; //zero based index
                        if (columnIndex < cellColumnIndex)
                        {
                            do
                            {
                                tempRow[columnIndex] = ""; //Insert blank data here;
                                columnIndex++;
                            }
                            while (columnIndex < cellColumnIndex);
                        }
                        tempRow[columnIndex] = GetCellValue(spreadSheetDocument, cell);

                        columnIndex++;
                    }
                    dt.Rows.Add(tempRow);

                }
            }
            dt.Rows.RemoveAt(0); //...so i'm taking it out here

            ds.Tables.Add(dt);
            return ds;
        }

        /// <summary>
        /// Given a cell name, parses the specified cell to get the column name.
        /// </summary>
        /// <param name="cellReference">Address of the cell (ie. B2)</param>
        /// <returns>Column Name (ie. B)</returns>
        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            return match.Value;
        }
        /// <summary>
        /// Given just the column name (no row index), it will return the zero based column index.
        /// Note: This method will only handle columns with a length of up to two (ie. A to Z and AA to ZZ). 
        /// A length of three can be implemented when needed.
        /// </summary>
        /// <param name="columnName">Column Name (ie. A or AB)</param>
        /// <returns>Zero based index if the conversion was successful; otherwise null</returns>
        public static int? GetColumnIndexFromName(string columnName)
        {

            //return columnIndex;
            string name = columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;
        }

        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = "";
            if (cell.DataType != null && cell.DataType.Value == CellValues.InlineString)
            {
                return cell.InlineString.Text.Text;
            }
            else
                if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                {
                    try
                    {
                        value = cell.CellValue.InnerXml;
                        return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                    }
                    catch (Exception)
                    {
                        return string.Empty;
                    }
                }
                else if (cell.CellValue == null)
                {
                    return value;
                }
                else
                {
                    value = cell.CellValue.InnerXml;
                    return value;
                }
        }
    }
}