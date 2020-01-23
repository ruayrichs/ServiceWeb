using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;


/// <summary>
/// Summary description for ExportFinalDataTableService
/// </summary>
public class ExportFinalDataTable
{
    public const string BaseStyle = "display";
    public const string BaseStyleNoStylingClasses = "";
    public const string BaseStyleCellBorders = "cell-border";
    public const string BaseStyleCompact = "display compact";
    public const string BaseStyleHover = "hover";
    public const string BaseStyleOrderColumn = "order-column";
    public const string BaseStyleRowBorders = "row-border";
    public const string BaseStyleStripe = "stripe";
    public const string Bootstrap = "table table-striped table-bordered";

    private string style = BaseStyle;

    public string Style
    {
        get { return style; }
        set { style = value; }
    }


    private string HeaderString = "<thead><tr>";
    private string FooterString = "<tfoot><tr>";
    private string BodyString = "<tbody>";

    public ExportFinalDataTable()
    {
        Style = BaseStyle;
    }

    public ExportFinalDataTable(string style)
    {
        Style = style;
    }


    public void AddHeaderAndFooterColumn(string ColumnName)
    {
        HeaderString += "<th style='text-align:left'>" + ColumnName + "</th>";
        FooterString += "<th style='text-align:left'>" + ColumnName + "</th>";
    }

    public void AddHeaderAndFooterColumn(string ColumnName, string CustomStyle)
    {
        HeaderString += "<th style='text-align:left; " + CustomStyle + "'>" + ColumnName + "</th>";
        FooterString += "<th style='text-align:left; " + CustomStyle + "'>" + ColumnName + "</th>";
    }
    public void AddHeaderAndFooterColumn(string ColumnName, string CustomStyle,string p_class)
    {
        HeaderString += "<th class='" + p_class + "' style='text-align:left; " + CustomStyle + "'>" + ColumnName + "</th>";
        FooterString += "<th class='" + p_class + "' style='text-align:left; " + CustomStyle + "'>" + ColumnName + "</th>";
    }
    public void AddDataRow(ExportFinalDataTableRowData DataRowObject)
    {
        BodyString += DataRowObject.GetRowString();
    }

    public string InitialDataTabelUI(bool showFooter)
    {
        return InitialDataTabelUI(showFooter, true, true);
    }
    public string InitialDataTabelUI(bool showFooter, bool paging, bool searching)
    {
        string guid = Guid.NewGuid().ToString("D");

        StringBuilder options = new StringBuilder();
        options.Append("'paging': " + Convert.ToString(paging).ToLower());
        options.Append(", 'searching': " + Convert.ToString(searching).ToLower());

        string Result = "";
        //Result += "<table id='" + guid + "' class='datatable-grid display' cellspacing='0'>";
        Result += "<table id='" + guid + "' class='datatable-grid " + Style + "' cellspacing='0'>";
        Result += HeaderString + "</tr></thead>";
        if (showFooter)
        {
            Result += FooterString + "</tr></tfooter>";
        }
        Result += BodyString + "</tbody>";
        Result += "</table>";

        System.Web.UI.Page page = System.Web.HttpContext.Current.Handler as System.Web.UI.Page;
        ScriptManager.RegisterStartupScript(page, page.GetType(), "bdg" + guid
            , "$('#" + guid + "').dataTable({" + options.ToString() + "});", true);

        return Result;
    }

    public void CallInitialScriptHandle()
    {
        System.Web.UI.Page page = System.Web.HttpContext.Current.Handler as System.Web.UI.Page;
        ScriptManager.RegisterStartupScript(page, page.GetType(), "bdg", "$('.datatable-grid').dataTable();", true);
    }

    [Serializable]
    public static class ColumnStyle
    {
        public static string TextLeft = "text-align:left;";
        public static string TextRight = "text-align:right;";
        public static string TextCenter = "text-align:center;";
        public static string TextBold = "font-weight:600";
    }
    [Serializable]
    public class ExportFinalDataTableRowData
    {
        private string DataRowString = "<tr>";
        public void AddColumnData(string DataString)
        {
            DataRowString += "<td>" + DataString + "</td>";
        }
        public void AddColumnData(string DataString, int DecimalPlace)
        {
            DataString = String.Format("{0:N" + DecimalPlace + "}", Convert.ToDecimal(DataString));
            DataRowString += "<td>" + DataString + "</td>";
        }
        public void AddColumnData(string DataString, int DecimalPlace, string CustomStyle)
        {
            DataString = String.Format("{0:N" + DecimalPlace + "}", Convert.ToDecimal(DataString));
            DataRowString += "<td style='" + CustomStyle + "'>" + DataString + "</td>";
        }
        public void AddColumnData(string DataString, string CustomStyle)
        {
            DataRowString += "<td style='" + CustomStyle + "'>" + DataString + "</td>";
        }
        public void AddColumnDataWithEvent(string DataString, string CustomStyle,string Event)
        {
            DataRowString += "<td style='" + CustomStyle + "' " + Event + ">" + DataString + "</td>";
        }
        public void AddColumnData(string DataString, string CustomStyle,string tooltip)
        {
            DataRowString += "<td style='" + CustomStyle + "' title='"+tooltip+"'>" + DataString + "</td>";
        }
        public void AddColumnDataCustom(string tdStyle, string tdCustom)
        {
            DataRowString += "<td " + tdStyle + ">" + tdCustom + "</td>";
        }
        public string GetRowString()
        {
            string ReturnString = DataRowString + "</tr>";
            return DataRowString;
        }
    }
}
