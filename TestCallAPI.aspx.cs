using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
//using ERPW.Lib.Master.Config;
//using ERPW.Lib.Service;
//using JobSchedulerManager.service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb
{
    public partial class TestCallAPI : System.Web.UI.Page
    {
        //RequestPermissionKeyList lib = new RequestPermissionKeyList();
        protected void Page_Load(object sender, EventArgs e)
        {
            //lib.getPermission("", "", "", "", "", "");
        }

        protected void btnTestAPI_Click(object sender, EventArgs e)
        {

            NotificationLibrary.GetInstance().TicketAlertEvent(NotificationLibrary.EVENT_TYPE.TICKET_OPEN,
                ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, "I-0000000141", ERPWAuthentication.EmployeeCode
            );

            return;
            //String sid = "555"; //context.JobDetail.Key.Group;
            //String companyCode = "INET"; //Convert.ToString(ConfigurationManager.AppSettings["RECURR_COMPANY_CODE"]);
            //String batchId = "RE00014"; // context.JobDetail.Key.Name;
            //JobServiceTierZero.retrieveEmailAndSendTierZero(sid, companyCode, batchId);

            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:39871");
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("SID", "555");
            //client.DefaultRequestHeaders.Add("CompanyCode", "INET");
            //client.DefaultRequestHeaders.Add("EmployeeCode", "EMP010000003");
            //client.DefaultRequestHeaders.Add("UserName", "focusone");
            //client.DefaultRequestHeaders.Add("FullNameEN", "Focus One Administrator");
            //client.DefaultRequestHeaders.Add("USER_SESSION_ID", "1928297577");
            //client.DefaultRequestHeaders.Add("Channel", "1");
            //client.DefaultRequestHeaders.Add("EMail", "kolabanlong@gmail.com");
            //client.DefaultRequestHeaders.Add("CustomerCode", "");
            //client.DefaultRequestHeaders.Add("CustomerName", "");
            //client.DefaultRequestHeaders.Add("TelNo", "");
            //client.DefaultRequestHeaders.Add("Subject", "Test API");
            //client.DefaultRequestHeaders.Add("Detail", "Test @ C# Call API");

            //HttpResponseMessage response = client.GetAsync("API/TierZeroStructureAPI.aspx").Result;
            //if (response.IsSuccessStatusCode)
            //{

            //}
            //else
            //{

            //}
        }
    }
}