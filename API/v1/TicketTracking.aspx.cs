using Agape.Lib.DBService;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.WebConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.API.v1
{
    public partial class TicketTracking : System.Web.UI.Page
    {
        DBService dbService = new DBService();

        private string _SID;
        private string SID
        {
            get
            {
                if (string.IsNullOrEmpty(_SID))
                    _SID = !string.IsNullOrEmpty(ERPWAuthentication.SID) ? ERPWAuthentication.SID : ERPWebConfig.GetSID(); // "555";
                return _SID;
            }
        }

        private string _CompanyCode;
        private string CompanyCode
        {
            get
            {
                if (string.IsNullOrEmpty(_CompanyCode))
                    _CompanyCode = !string.IsNullOrEmpty(ERPWAuthentication.CompanyCode) ? ERPWAuthentication.CompanyCode : ERPWebConfig.GetCompany(); // "INET";
                return _CompanyCode;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string jsonRespond = JsonConvert.SerializeObject(TicketTrackingDetail(), Formatting.Indented);
            Response.Write(jsonRespond);
        }

        private CommonResponse TicketTrackingDetail()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                string TicketNumber = !string.IsNullOrEmpty(Request["TicketNumber"]) ? Request["TicketNumber"] : Request.Headers["TicketNumber"];
                string Email = !string.IsNullOrEmpty(Request["Email"]) ? Request["Email"] : Request.Headers["Email"];

                string PrefixCode = "";
                string Number = "";
                DataTable dtPrefix = ServiceTicketLibrary.GetInstance().getDataPrefixDocType(SID, CompanyCode, new List<string>());
                DataRow[] drr = dtPrefix.Select("'" + TicketNumber + "' like PrefixCode + '%'");
                if (drr.Length > 0)
                {
                    PrefixCode = Convert.ToString(drr[0]["PrefixCode"]);
                    Number = TicketNumber.Replace(PrefixCode, "");
                }
                else
                {
                    throw new Exception("Ticket number " + TicketNumber + " not found");
                }

                DataTable dt = GetDataTicket(TicketNumber, PrefixCode, Number);
                if (dt.Rows.Count > 0)
                {
                    string CustomerCode = Convert.ToString(dt.Rows[0]["CustomerCode"]);

                    DataTable dtContact = checkAuthenTracking(SID, CompanyCode, CustomerCode, Email);
                    if (dtContact.Rows.Count > 0)
                    {
                        response.Data = new DataTicket();
                        response.ResultCode = "Success";
                        response.Message = "Success";

                        response.Data.TicketNumber = TicketNumber;
                        response.Data.StatusCode = Convert.ToString(dt.Rows[0]["Docstatus"]);
                        response.Data.StatusDescription = Convert.ToString(dt.Rows[0]["TicketStatusDesc"]);
                        response.Data.CustomerName = Convert.ToString(dtContact.Rows[0]["CustomerName"]);
                        response.Data.ContactName = Convert.ToString(dtContact.Rows[0]["ContactName"]);
                        response.Data.ContactEmail = Convert.ToString(dtContact.Rows[0]["ContactName"]);
                    }
                    else
                    {
                        throw new Exception("Email " + Email + " not found");
                    }
                }
                else
                {
                    throw new Exception("Ticket number " + TicketNumber + " not found");
                }
            }
            catch (Exception ex)
            {

                response.ResultCode = "Error";
                response.Message = ex.Message;
            }

            return response;
        }

        private DataTable GetDataTicket(string TicketNumber, string PrefixCode, string Number)
        {
            string sql = @"select a.CallerID, a.Docstatus, b.TicketStatusDesc, a.CustomerCode, a.SID, a.CompanyCode
                            from cs_servicecall_header a with (nolock)
                            left join ERPW_TICKET_STATUS b with (nolock)
                              on a.SID = b.SID
                              and a.CompanyCode = b.CompanyCode
                              and a.Docstatus = b.TicketStatusCode
                            where (CallerID like '" + PrefixCode + "%' and  CallerID like '%" + Number + "') or CallerID = '" + TicketNumber + "'";

            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        private DataTable checkAuthenTracking(string SID, string CompanyCode, string CustomerCode, string Email)
        {
            string sql = @"SELECT a.CustomerCode, a.CustomerName, d.NAME1 as ContactName
                            FROM master_customer a with (nolock)
                            INNER JOIN master_customer_general b with (nolock)
                              on a.SID = b.SID
                              AND a.CompanyCode = b.CompanyCode 
                              AND a.CustomerCode = b.CustomerCode
                            left join CONTACT_MASTER c with (nolock)
                              on a.SID = c.SID 
                              and a.CompanyCode = c.COMPANYCODE
                              and a.CustomerCode = c.BPCODE
                            left join CONTACT_DETAILS d with (nolock)
                              on c.SID = d.SID 
                              AND c.AOBJECTLINK = d.AOBJECTLINK
                            left join CONTACT_EMAIL f with (nolock)
                              on d.SID = f.SID 
                              AND  d.BOBJECTLINK = f.BOBJECTLINK
                            WHERE a.SID = '" + SID + @"' 
                              AND a.CompanyCode = '" + CompanyCode + @"' 
                              and b.Active = 'True'
                              and a.CustomerCode like '" + CustomerCode + @"' 
                              and f.EMAIL like '" + Email + @"' ";
            DataTable dt = dbService.selectDataFocusone(sql);
            return dt;
        }

        protected class CommonResponse
        {
            public string ResultCode { get; set; }
            public string Message { get; set; }
            public DataTicket Data { get; set; }

        }
        protected class DataTicket
        {
            public string TicketNumber { get; set; }
            public string StatusCode { get; set; }
            public string StatusDescription { get; set; }
            public string CustomerName { get; set; }
            public string ContactName { get; set; }
            public string ContactEmail { get; set; }
        }
    }

}