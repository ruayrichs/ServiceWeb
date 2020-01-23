using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POSWeb.UserControl
{
    public partial class SmartPaging : System.Web.UI.UserControl
    {
        public delegate void SmartPagingSelectedPageChange(object sender, int pageIndex);
        public event SmartPagingSelectedPageChange SelectedPageChange;

        private DataTable _DataSource;
        private Repeater _RepeaterControl = null;
        private UpdatePanel _UpdatePanelControl = null;
        public int TotalPage
        {
            get
            {
                return (int)Math.Ceiling((double)TotalDataSourceRows / DataPageSize);
            }
        }
        public bool IsChangePageIndex = false;
        public bool IsShowPageIndex(int index)
        {
            int start = 0;
            int end = 0;
            start = PageIndex - 2;
            end = PageIndex + 2;

            if (start < 1)
            {
                end += 1 - start;
            }
            if (end > TotalPage)
            {
                start += TotalPage - end;
            }

            if ((index >= start && index <= end) || index == 1 || index == TotalPage)
                return true;
            return false;
        }
        public int PageIndex
        {
            get
            {
                return Convert.ToInt32(hddPageIndex.Value);
            }
            set
            {
                if (!hddPageIndex.Value.Equals(value.ToString()))
                    IsChangePageIndex = true;
                hddPageIndex.Value = value.ToString();
            }
        }
        public Repeater RepeaterControl
        {
            get
            {
                return _RepeaterControl;
            }
            set
            {
                _RepeaterControl = value;
            }
        }
        public UpdatePanel UpdatePanelControl
        {
            get
            {
                return _UpdatePanelControl;
            }
            set
            {
                _UpdatePanelControl = value;
            }
        }
        public int DataPageSize
        {
            get
            {
                return Convert.ToInt32(hddDataPageSize.Value);
            }
            set
            {
                hddDataPageSize.Value = value.ToString();
            }
        }

        public int TotalDataSourceRows
        {
            get
            {
                return Convert.ToInt32(hddTotalDataSourceRow.Value);
            }
            set
            {
                hddTotalDataSourceRow.Value = value.ToString();
            }
        }
        public DataTable DataSource
        {
            get
            {
                return _DataSource;
            }
            set
            {
                _DataSource = value;
                if (!_DataSource.Columns.Contains("_PagingService_RowNum"))
                {
                    _DataSource.Columns.Add("_PagingService_RowNum", typeof(Int32));
                    int rowCounter = 1;
                    foreach (DataRow dr in _DataSource.Rows)
                    {
                        dr["_PagingService_RowNum"] = rowCounter++;
                    }
                }
                hddTotalDataSourceRow.Value = _DataSource.Rows.Count.ToString();
            }
        }

        public void DataBind()
        {
            if (PageIndex < 1)
            {
                PageIndex = 1;
            }
            if (PageIndex > TotalPage)
            {
                PageIndex = TotalPage;
            }

            PageIndex = PageIndex == 0 ? 1 : PageIndex;
            int endRowNum = PageIndex * DataPageSize;
            int startRowNum = endRowNum - (DataPageSize - 1);
            DataSource.DefaultView.RowFilter = "_PagingService_RowNum >= " + startRowNum + " and _PagingService_RowNum <= " + endRowNum;

            if (RepeaterControl != null)
            {
                RepeaterControl.DataSource = DataSource.DefaultView.ToTable();
                RepeaterControl.DataBind();
            }

            if (UpdatePanelControl != null)
            {
                UpdatePanelControl.Update();
                if (IsChangePageIndex)
                {
                    ClientService.DoJavascript("try{SmartPaging" + ID + "('" + UpdatePanelControl.ClientID + "');}catch(e){}");
                }
            }
            BindPage();
        }

        public void DataBind(int pageIndex)
        {
            PageIndex = pageIndex;
            DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientService.AGLoading(false);
        }
        private void BindPage()
        {
            int[] arr = new int[TotalPage];
            rptSmartPaging.DataSource = arr;
            rptSmartPaging.DataBind();
            udpSmartPagging.Update();

            ClientService.DoJavascript("$('#" + ID + "').toggle(" + (rptSmartPaging.Items.Count > 1).ToString().ToLower() + ");");
        }
        protected void btnPrevPage_Click(object sender, EventArgs e)
        {
            int NewPage = PageIndex - 1;
            if (SelectedPageChange != null)
            {
                SelectedPageChange(this, NewPage);
            }
        }
        protected void btnNextPage_Click(object sender, EventArgs e)
        {
            int NewPage = PageIndex + 1;
            if (SelectedPageChange != null)
            {
                SelectedPageChange(this, NewPage);
            }
        }
        protected void btnSelectPage_Click(object sender, EventArgs e)
        {
            int NewPage = Convert.ToInt32((sender as Button).CommandArgument);
            if (SelectedPageChange != null)
            {
                SelectedPageChange(this, NewPage);
            }
        }

        protected void btnLastPage_Click(object sender, EventArgs e)
        {
            int NewPage = TotalPage;
            if (SelectedPageChange != null)
            {
                SelectedPageChange(this, NewPage);
            }
        }
        protected void btnFirstPage_Click(object sender, EventArgs e)
        {
            int NewPage = 1;
            if (SelectedPageChange != null)
            {
                SelectedPageChange(this, NewPage);
            }
        }

        public void GoToFirstPage()
        {
            btnFirstPage_Click(null, null);
        }
        public void GoToLastPage()
        {
            btnLastPage_Click(null, null);
        }
    }
}