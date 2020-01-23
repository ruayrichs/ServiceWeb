using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Agape.FocusOne.Utilities;
using Agape.Lib.DBService;
using SNAWeb.Analytics;
using ERPW.Lib.Authentication;

public partial class Analytics_AnalyticsUpdate : System.Web.UI.Page
{
    DBService dbService = new DBService();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "application/json";

        AnalyticsService.UpdateExitPage(
            ERPWAuthentication.CompanyCode,
            ERPWAuthentication.SID,
            ERPWAuthentication.EmployeeCode,
            Request["Row_key"],
            Request["LiveOn"],
            Validation.getCurrentServerStringDateTimeMillisecond()
        );
    }
}