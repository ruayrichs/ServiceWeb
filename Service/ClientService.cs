
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

public class ClientService
{
    private static void DoScript(string Script)
    {
        System.Web.UI.Page page = System.Web.HttpContext.Current.Handler as System.Web.UI.Page;
        ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString("D"), "$(document).ready(function(){" + Script + "});", true);
    }

    private static void DoScriptWithoutReady(string Script)
    {
        System.Web.UI.Page page = System.Web.HttpContext.Current.Handler as System.Web.UI.Page;
        ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString("D"), Script, true);
    }

    private static string ScriptRedirect(string Url, bool showLoading)
    {
        return @"$(document).click(function(){" + (showLoading ? "AGLoading(true);" : "") + "location.href = '" + Url + "';});";
    }
    private static string ScriptRedirectCurrentPage(bool showLoading)
    {
        return @"$(document).click(function(){" + (showLoading ? "AGLoading(true);" : "") + "location.reload();});";
    }
    private static string SweetAlertRedirect(string Url, bool showLoading)
    {
        return @"$('.sweet-alert button.confirm').click(function(){" + (showLoading ? "AGLoading(true);" : "") + "location.href = '" + Url + "';});";

    }
    private static string SweetAlertRedirectCurrentPage(bool showLoading)
    {
        return @"$('.sweet-alert button.confirm').click(function(){" + (showLoading ? "AGLoading(true);" : "") + "location.reload();});";
        
    }
    public static void Alert(string Message)
    {
        DoScript("AGMessage('" + ObjectUtil.Err(Message) + "', 250);");
    }
    public static void AlertRedirect(string Message, string Url)
    {
        DoScript("AGMessage('" + ObjectUtil.Err(Message) + "', 250);" + ScriptRedirect(Url, false));
    }
    public static void AlertRedirectCurrentPage(string Message)
    {
        DoScript("AGMessage('" + ObjectUtil.Err(Message) + "', 250);" + ScriptRedirectCurrentPage(false));
    }
    public static void AlertRedirectWithLoading(string Message, string Url)
    {
        DoScript("AGMessage('" + ObjectUtil.Err(Message) + "', 250);" + ScriptRedirect(Url, true));
    }
    public static void RedirectAfterDocumentClick(string Url)
    {
        DoScript(ScriptRedirect(Url, false));
    }

    public static void RedirectCurrentPage()
    {
        DoScript("location.reload();");
    }
    public static void RedirectCurrentPageAfterDocumentClick()
    {
        DoScript(ScriptRedirectCurrentPage(false));
    }
    public static void AlertSaveSuccess(string Url)
    {
        AlertRedirect("บันทึกเรียบร้อยแล้ว", Url);
    }

    public static void AgroLoading(bool isShow)
    {
        AGLoading(isShow);
    }
    public static void AGLoading(bool isShow)
    {
        DoScript("AGLoading(" + isShow.ToString().ToLower() + ");");
    }

    public static void AGLoading(bool isShow, string Message)
    {
        DoScript("AGLoading(" + isShow.ToString().ToLower() + ",'" + Message + "');");
    }

    public static void DoJavascript(string script)
    {
        DoScript(script);
    }

    public static void DoJavascript(string script, bool withDocReady)
    {
        if (withDocReady)
            DoScript(script);
        else
            DoScriptWithoutReady(script);
    }

    public static void AGMessage(string message)
    {
        DoScript("AGMessage('" + message + "', 250);");
    }

    public static void AGSuccess(string message)
    {
        DoScript("AGSuccess('" + message + "', 250);");
    }

    public static void AGSuccess(string message, string jsScript)
    {
        string endScript = @"$('.sweet-alert button.confirm').click(function(){ " + jsScript + " });";

        DoScript("AGSuccess('" + message + "', 250);" + endScript);
    }

    public static void AGError(string message)
    {
        DoScript("AGError('" + ObjectUtil.Err(message) + "', 250);");
    }

    public static void AGInfo(string message)
    {
        DoScript("AGInfo('" + message + "', 250);");
    }

    public static void AGSuccessRedirect(string Message, string Url)
    {
        DoScript("AGSuccess('" + ObjectUtil.Err(Message) + "', 250);" + SweetAlertRedirect(Url, false));
    }

    //public static void AGSuccessWithClickBtn(string message,string idbtnforclick)
    //{
    //    DoScript("AGSuccess('" + message + "','" + idbtnforclick + "');");
    //}

    public static void AGSuccessRedirectCurrentPage(string Message)
    {
        DoScript("AGSuccess('" + ObjectUtil.Err(Message) + "', 250);" + SweetAlertRedirectCurrentPage(false));
    }

    public static void OpenSessionTimedOutFade()
    {
        DoScript("OpenSessionTimedOutFade();");
    }
}