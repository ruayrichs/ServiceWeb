using ERPW.Lib.Master;
using ERPW.Lib.Master.Entity;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using ERPW.Lib.WebConfig;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.crm.AfterSale
{
    public partial class CustomerForm : System.Web.UI.Page
    {
        TierZeroService lib = new TierZeroService();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    SetData();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void SetData()
        {
            string sid = ERPWebConfig.GetSID();
            string companyCode = ERPWebConfig.GetCompany();

            TierZeroEn en = TierZeroLibrary.GetInstance().getTierZeroDetail(sid, companyCode, Request["key"]);

            if (en.SEQ != null)
            {
                if (en.Status == ERPW.Lib.Service.TierZeroLibrary.TIER_ZERO_STATUS_CREATED)
                {
                    lbMessage.Text = "Your ticket has been created. Thank you for your information.";
                    ClientService.DoJavascript("$('#panel-information').show(); $('#panel-create').hide();");
                }
                else
                {
                    ContactEntity contact = new ContactEntity();
                    contact.email = en.EMail;

                    string tempTicketNo = "TK" + Request["key"].PadLeft(8, '0');

                    tbSeq.Text = tempTicketNo;
                    tbCustomer.Text = "คุณ" + en.CustomerName;
                    tbEmail.Text = en.EMail;
                    tbSubject.Text = en.Subject;
                    tbDetail.Text = StripHTML(en.Detail);                  

                    DataTable dt = AfterSaleService.getInstance().getSearchEQInfo(sid, companyCode, en.CustomerCode, "");

                    ddlEquipment.DataTextField = "EquipmentName";
                    ddlEquipment.DataValueField = "EquipmentCode";
                    ddlEquipment.DataSource = dt;
                    ddlEquipment.DataBind();
                    ddlEquipment.Items.Insert(0, new ListItem("", ""));

                    ClientService.DoJavascript("$('#panel-information').hide(); $('#panel-create').show();");
                }                
            }
            else
            {
                lbMessage.Text = "Not found your information.";           
                ClientService.DoJavascript("$('#panel-information').show(); $('#panel-create').hide();");
            }
        }

        private string StripHTML(string HTMLText, bool decode = true)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);

            var stripped = reg.Replace(HTMLText, "");

            string result = decode ? HttpUtility.HtmlDecode(stripped) : stripped;

            result = RecursiveReplaceNewLineBeforeText(result);

            return result;
        }

        private string RecursiveReplaceNewLineBeforeText(string text)
        {
            if (text.IndexOf('\r') == 0 || text.IndexOf('\n') == 0)
            {
                text = text.Substring(2, text.Length - 2);

                text = RecursiveReplaceNewLineBeforeText(text);
            }
         
            return text;
        }

        private static string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string sid = ERPWebConfig.GetSID();
                string companyCode = ERPWebConfig.GetCompany();
                string sessionId = SNA.Lib.POS.utils.POSDocumentHelper.getSessionId(sid);

                TierZeroEn en = TierZeroLibrary.GetInstance().getTierZeroDetail(sid, companyCode, Request["key"]);

                en.Subject = tbSubject.Text.Trim();
                en.Detail = tbDetail.Text.Trim();
                en.EquipmentNo = ddlEquipment.SelectedValue;

                ResultAutoCreateTicket enResult = TierZeroService.getInStance().AutoCreatedTicketFormTierZero(sessionId, sid, companyCode, en, en.CustomerCode, "focusone");

                if (enResult.CreatedSuccess)
                {
                    en.Status = ERPW.Lib.Service.TierZeroLibrary.TIER_ZERO_STATUS_CREATED;
                    en.TicketNumber = enResult.TicketNo;
                    en.TicketType = enResult.TicketType;

                    ERPW.Lib.Service.TierZeroLibrary.GetInstance().UpdateTierZeroDetail(sid, companyCode, en.SEQ, en.TicketType, en.TicketNumber, en.Status,
                               en.CustomerCode, en.CustomerName, "");

                    Response.Redirect(Page.Request.Url.ToString());
                }
                else
                {
                    ClientService.AGError(enResult.ResultMessage);
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
            finally
            {
                ClientService.AGLoading(false);
            }
        }
    }
}