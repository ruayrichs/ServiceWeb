using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using Newtonsoft.Json;
using ServiceWeb.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API
{
    public partial class Tier0API : System.Web.UI.Page
    {
        private TierZeroLibrary tierlib = new TierZeroLibrary();
        private ServiceWeb.Report.ReportUnity report_unity = new ReportUnity();
        private string _SID;
        private string _CompanyCode;
        private string _SEQ;
        private string SID { get
            {
                if (string.IsNullOrEmpty(_SID))
                {
                    _SID = ERPWAuthentication.SID;
                }
                return _SID;
            } }
        private string CompanyCode { get
            {
                if (string.IsNullOrEmpty(_CompanyCode))
                {
                    _CompanyCode = ERPWAuthentication.CompanyCode;
                }
                return _CompanyCode;
            } }
        private string SEQ { get
            {
                if (string.IsNullOrEmpty(_SEQ))
                {
                    _SEQ = Request.QueryString["SEQ"];
                }
                return _SEQ;
            } }

        private TierZeroLibrary libTierZero = new TierZeroLibrary();

        protected void Page_Load(object sender, EventArgs e)
        {
            initData();
        }
        protected void initData()
        {
            DataTable dt = new DataTable();
            List<TierZeroEn> lists = tierlib.getTier0Desc(SID, CompanyCode, SEQ);

            List<DataKeyValue> datasTicketNumber = new List<DataKeyValue>();
            lists.ForEach(r =>
            {
                if (!string.IsNullOrEmpty(r.TicketNumber))
                {
                    datasTicketNumber.Add(new DataKeyValue { Key = r.TicketNumber, Value = r.TicketNumber });
                }
            });
            dt = datasTicketNumber.ToList().toDataTable();
            dt = report_unity.incidentNoFormater(SID, CompanyCode, dt, "Value");
            datasTicketNumber = dt.toList<DataKeyValue>();

            //dt = lists.toDataTable();
            //dt = report_unity.incidentNoFormater(SID, CompanyCode, dt, "TicketNumber");
            //lists = dt.toList<TierZeroEn>();

            for (int index = 0; index < lists.Count; index++)
            {
                //lists[index].CREATED_ON = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(lists[index].CREATED_ON);
                //lists[index].Channel = FormatChannel(lists[index].Channel);
                //lists[index].Status = FormatStatus(lists[index].Status);

                try
                {
                    if (!string.IsNullOrEmpty(lists[index].TicketNumber))
                        lists[index].TicketNumber = datasTicketNumber.FindAll(f => f.Key == lists[index].TicketNumber).First().Value;
                }
                catch (Exception)
                {
                }

                lists[index].CREATED_ON = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(lists[index].CREATED_ON);
                lists[index].Channel = FormatChannel(lists[index].Channel);
                lists[index].ControlDisplayStatus = FormatStatus(lists[index].Status);
                lists[index].ControlDisplay = (!IsCanCreatedTicket(lists[index].Status)).ToString();
            }
            var json = new JavaScriptSerializer().Serialize(lists);
            Response.Write(json);
        }

        protected string FormatChannel(string strChannel)
        {
            return TierZeroLibrary.getChannelDescription(strChannel);
        }
        protected string FormatStatus(string strStatus)
        {
            return TierZeroLibrary.getStatusDescription(strStatus);
        }
        public bool IsCanCreatedTicket(string strStatus)
        {
            return TierZeroLibrary.TIER_ZERO_STATUS_OPEN == strStatus;
        }

        protected class DataKeyValue
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}