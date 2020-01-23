using Agape.FocusOne.Utilities;
using Agape.Lib.Web.Bean.MM;
using Newtonsoft.Json;
using ServiceWeb.auth;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using agape.lib.constant;
using ERPW.Lib.F1WebService.ICMUtils;
using Agape.Lib.Web.Bean.CS;

namespace ServiceWeb
{
    public partial class NextMaintenanceCalendar : AbstractsSANWebpage//System.Web.UI.Page
    {
        protected List<CalendarNextMaintenance> calendarEvent;
        protected List<CalendarNextMaintenance> orderChangeEvent;
        protected List<CalendarNextMaintenance> eventAll;
        ServiceTicketLibrary ticketLibbrary = new ServiceTicketLibrary();
        CalendarService serviceCalendar = new CalendarService();
        private ICMUtils ICMService = ERPW.Lib.F1WebService.F1WebService.getICMUtils();
        
        #region Service
        EquipmentService ServiceEquitment = new EquipmentService();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                DataTable dt = serviceCalendar.getNextMaintenanceTime();
                DataTable dtonChange = serviceCalendar.getAllChangeOrder();
                string customerCode = (string)Session["SCT_created_cust_code"];

                calendarEvent = (from rw in dt.AsEnumerable()
                                 select new CalendarNextMaintenance()
                                  {
                                  title = Convert.ToString(rw["EquipmentCode"]),
                                  start = ConvertDateStrFormat(Convert.ToString(rw["NextMaintenanceDate"]), Convert.ToString(rw["NextMaintenanceTime"])),
                                  color = "#8AEBF5",
                                  haveUrl = false
                                 }).ToList();


                orderChangeEvent = (from rw in dtonChange.AsEnumerable()
                                    select new CalendarNextMaintenance()
                                    {
                                        id = Convert.ToString(rw["CallerID"]),
                                        title = ticketLibbrary.GetDocumentTypeDesc(SID, CompanyCode, Convert.ToString(rw["Doctype"])) + " " +
                                                ticketLibbrary.ReplaceTicketNumberToDisplay(Convert.ToString(rw["PrefixCode"]), Convert.ToString(rw["CallerID"])) + " : " +
                                                Convert.ToString(rw["HeaderText"]),
                                        start = ConvertDateStrFormat(Convert.ToString(rw["PlanStartDate"]), Convert.ToString(rw["PlanStartTime"])),
                                        end = ConvertDateStrFormat(Convert.ToString(rw["PlanEndDate"]), Convert.ToString(rw["PlanEndTime"])),
                                        color = "#FBE78F",
                                        doctype = Convert.ToString(rw["Doctype"]),
                                        docnumber = Convert.ToString(rw["CallerID"]),
                                        fiscalyear = Convert.ToString(rw["Fiscalyear"]),
                                        customerCode = customerCode,
                                        tooltipTitle = ticketLibbrary.GetDocumentTypeDesc(SID, CompanyCode, Convert.ToString(rw["Doctype"])) + " " +
                                                  ticketLibbrary.ReplaceTicketNumberToDisplay(Convert.ToString(rw["PrefixCode"]), Convert.ToString(rw["CallerID"])),
                                        tooltipContent = Convert.ToString(rw["HeaderText"]),
                                        haveUrl = true
                                    }).ToList();

                calendarEvent.AddRange(orderChangeEvent);

            }
           

        }
        private void getdataToedit(string doctype, string docnumber, string fiscalyear)
        {
            string customer = (string)Session["SCT_created_cust_code"];//CustomerSelect.SelectedValue;
            string idGen = redirectViewToTicketDetail(customer, doctype, docnumber, fiscalyear);

            string PageRedirect = ServiceTicketLibrary.GetInstance().getPageTicketRedirect(
                SID,
                (Session["ServicecallEntity" + idGen] as tmpServiceCallDataSet).cs_servicecall_header.Rows[0]["Doctype"].ToString()
            );
            if (!String.IsNullOrEmpty(idGen))
            {
                ClientService.DoJavascript("goToEdit('" + Page.ResolveUrl("~/crm/AfterSale/" + PageRedirect + "?id=" + idGen) + "');");
            }
        }

        private string ConvertDateStrFormat(string date, string time)
        {
            if (time == "")
            {
                time = "000000";
            }
            string datetimeStr = date + " " + time;
            DateTime dt = DateTime.ParseExact(datetimeStr, "yyyyMMdd HHmmss", CultureInfo.InvariantCulture);
            String dt_str_format = String.Format("{0:s}", dt);
            return dt_str_format;
        }

        public string redirectViewToTicketDetail(string customerCode, string doctype, string docnumber, string fiscalyear)
        {
            string idGen = "";
            Object[] objParam = new Object[] { "1500117",
                    (string)Session[ApplicationSession.USER_SESSION_ID],
                    CompanyCode,doctype,docnumber,fiscalyear};

            DataSet[] objDataSet = new DataSet[] { new tmpServiceCallDataSet() };
            DataSet objReturn = ICMService.ICMDataSetInvoke(objParam, objDataSet);
            if (objReturn != null)
            {
                idGen = Guid.NewGuid().ToString().Substring(0, 8);
                tmpServiceCallDataSet serviceTempCallEntity = new tmpServiceCallDataSet();
                serviceTempCallEntity.Merge(objReturn.Copy());
                Session["ServicecallEntity" + idGen] = serviceTempCallEntity;
                Session["SCT_created_cust_code" + idGen] = customerCode;//Customer
                Session["SC_MODE" + idGen] = ApplicationSession.DISPLAY_MODE_STRING;
            }
            return idGen;
        }

        protected void btnCalendar_Click(object sender, EventArgs e)
        {
            string ticket_params = hhdModeCalendar.Value;
            string[] words = ticket_params.Split('|');
            getdataToedit(words[0], words[1], words[2]);
        }
    }
    public class CalendarNextMaintenance
    {
        public bool haveUrl { get; set; }
        public string tooltipTitle { get; set; }
        public string tooltipContent { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string color { get; set; }
        public string doctype { get; set; }
        public string docnumber { get; set; }
        public string fiscalyear { get; set; }
        public string id { get; set; }
        public string customerCode { get; set; }
    }
}