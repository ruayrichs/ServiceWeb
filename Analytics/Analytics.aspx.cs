using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using SNAWeb.Analytics;
using ERPW.Lib.Authentication;
using Newtonsoft.Json;

public partial class Analytics_Analytics : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ERPWAuthentication.EmployeeCode))
            return;

        Analytics ant = new Analytics
        {
            EmployeeCode = ERPWAuthentication.EmployeeCode,
            CompanyCode = ERPWAuthentication.CompanyCode,
            SID = ERPWAuthentication.SID,
            PageName = Request["PageName"],
            PathName = Request["PathName"],
            Host = Request["Host"],
            Port = Request["Port"],
            FromUrl = Request["FromUrl"],
            OS = Request["OS"],
            Browser = Request["Browser"],
            BrowserVersion = Request["BrowserVersion"],
            Mobile = Request["Mobile"],
            Flash = Request["Flash"],
            Cookies = Request["Cookies"],
            Screen = Request["Screen"],
            LiveOn = Convert.ToInt32(Request["LiveOn"]),
            Lat = Request["Lat"],
            Long = Request["Long"],
            Acc = Request["Acc"],
            CountryCode = Request["CountryCode"],
            CountryName = Request["CountryName"],
            City = Request["City"],
            Region = Request["Region"],
            RegionName = Request["RegionName"],
            Language = Request["Language"],
            ISP = Request["ISP"],
            IP = Request["IP"],
            DeviceName = "",
            DateTimeIn = Validation.getCurrentServerStringDateTimeMillisecond(),
            Row_key = Request["Row_key"],
            ProgramID = Request["ProgramID"],
            ReferenceID = Request["ReferenceID"],
            ReferencePageMode = Request["ReferencePageMode"]
        };

        AnalyticsService.CreateNewRowAnalytics(ant);

        if (!string.IsNullOrEmpty(Request["ReferenceID"]))
        {
            AnalyticsEmployee enAnalytics = AnalyticsService.getTackingPageTicketIsAuthenEdit(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                Request["ReferenceID"],
                ERPWAuthentication.EmployeeCode
            );
            Response.Write(JsonConvert.SerializeObject(enAnalytics));
        }
    }
}