using ServiceWeb.auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Net;
using Agape.FocusOne.Utilities;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using ERPW.Lib.WebConfig;
namespace ServiceWeb.RequestTransection
{
    public partial class RequestActivateAppClient : AbstractsSANWebpage
    {
        private ERPW.Lib.Service.AppClientLibrary appClientLib = new ERPW.Lib.Service.AppClientLibrary();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                initData();
            }
        }

        private NotificationConfiguration GetNotificationConfig(string SID, string CompanyCode)
        {
            NotificationConfiguration notiConfig = new NotificationConfiguration();
            try
            {
                List<EmailConfig> ListEn = MasterEmailConfigLib.GetInstance().GetEmail_Host_Config(
                    SID,
                    CompanyCode,
                    MasterEmailConfigLib.HostEvent_TICKET
                );

                foreach (EmailConfig en in ListEn)
                {
                    int SMTPPort = 0;
                    int.TryParse(en.SMTPPort, out SMTPPort);

                    notiConfig.SMTPHost = en.SMTPHost;
                    notiConfig.SMTPPort = SMTPPort;
                    notiConfig.SMTPUseDefaultCredentials = en.SMTPUseDefaultCredentials;
                    notiConfig.SMTPCredentialsUsername = en.SMTPCredentialsUsername;
                    notiConfig.SMTPCredentialsPassword = en.SMTPCredentialsPassword;
                    notiConfig.MailFromSender = en.MailFromSender;
                    notiConfig.DefaultMailFrom = en.DefaultMailFrom;
                    notiConfig.DefaultMailFromDisplayName = en.DefaultMailFromDisplayName;
                    notiConfig.EnableSsl = en.EnableSsl;
                }
            }
            catch (Exception)
            {
                //notiConfig.SMTPHost = ConfigurationManager.AppSettings["MAIL_CONFIG_HOST"];                                                 //"203.151.201.196";//"smtp.gmail.com"; //
                //notiConfig.SMTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["MAIL_CONFIG_PORT"]);                                //25;//25;//587; //
                //notiConfig.SMTPUseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["MAIL_CONFIG_CREDENTIALS"]);      //false;
                //notiConfig.SMTPCredentialsUsername = ConfigurationManager.AppSettings["MAIL_CONFIG_USERNAME"];                              //"no-reply-ticket@inet.co.th";//"services.focusone@gmail.com"; //
                //notiConfig.SMTPCredentialsPassword = ConfigurationManager.AppSettings["MAIL_CONFIG_PASSWORD"];                              //"mail@zimbra02";//"AAaa1q2w3e4r"; //
                //notiConfig.MailFromSender = Convert.ToBoolean(ConfigurationManager.AppSettings["MAIL_CONFIG_MAILFROMSENDER"]);              //false;
                //notiConfig.DefaultMailFrom = ConfigurationManager.AppSettings["MAIL_CONFIG_USERNAME"];                                      //"services.focusone@gmail.com"; //
                //notiConfig.DefaultMailFromDisplayName = "Ticket Notification (No-Reply)";
                //notiConfig.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["MAIL_CONFIG_ENABLESSL"]);   

                //notiConfig.SMTPHost = "203.151.201.196";//"smtp.gmail.com"; //
                //notiConfig.SMTPPort = 25;//25;//587; //
                //notiConfig.SMTPUseDefaultCredentials = false;
                //notiConfig.SMTPCredentialsUsername = "no-reply-ticket@inet.co.th";//"services.focusone@gmail.com"; //
                //notiConfig.SMTPCredentialsPassword = "mail@zimbra02";
                //notiConfig.MailFromSender = false;
                //notiConfig.DefaultMailFrom = "no-reply-ticket@inet.co.th";
                //notiConfig.DefaultMailFromDisplayName = "Ticket Notification (No-Reply)";
                //notiConfig.EnableSsl = false;                        //true;
            }

            return notiConfig;
        }
        private void initData() {
            List<AppClientModel.RequestActivationModel> listAppClientPermisKey = appClientLib.getDataRequestApplicationPermissionKeyModel(
                ERPWebConfig.GetSID(), ERPWebConfig.GetCompany(), ""
            );
            rptItems.DataSource = listAppClientPermisKey;
            rptItems.DataBind();
            udpnItems.Update();
            ClientService.DoJavascript("bindingDataTableJS();");
        }


        protected void btnApprove_Click(object sender,EventArgs e) {
            try {
                string seq = hddSeq.Value;
                string appID = hddAppId.Value;
                
                string copPerKey = hddCopPerKey.Value;
                
                string datetime = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDB(DateTime.Now.ToString());
           
                string appPerKey = appClientLib.AcceptionRequestAppClientPermission(ERPWAuthentication.SID,ERPWAuthentication.CompanyCode,seq,ERPWAuthentication.EmployeeCode, datetime);
                initData();

                string body = @"<h4>Email Approval</h4><br/>ผู้ทำรายการ&nbsp;:&nbsp;"+ERPWAuthentication.FullNameTH+ @"<br/>เวลาที่ทำรายการ&nbsp;:&nbsp;"+ DateTime.Now.ToString()+ @"<br/>Application ID&nbsp;:&nbsp;"+appID+@"<br/>Corporate Permission Key&nbsp;:&nbsp;"+copPerKey+@"<br/>Application Permission Key&nbsp;:&nbsp;"+appPerKey;
                string subject = "Email Reject";
                string email = hddEmail.Value;
                sendToEmail(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, subject, body, email);
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
        protected void btnReject_Click(object sender, EventArgs e){
            try
            {
                string seq = hddSeq.Value;
                string appID = hddAppId.Value;
                
                string copPerKey = hddCopPerKey.Value;
                string datetime = Agape.FocusOne.Utilities.Validation.Convert2DateTimeDB(DateTime.Now.ToString());
                appClientLib.RejectRequestAppClientPermission(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, seq, ERPWAuthentication.EmployeeCode, datetime);
                initData();
               
                string body = @"<h4>Email Reject</h4><br/>ผู้ทำรายการ&nbsp;:&nbsp;" + ERPWAuthentication.FullNameTH + @"<br/>เวลาที่ทำรายการ&nbsp;:&nbsp;" + DateTime.Now.ToString() + @"<br/>Application ID&nbsp;:&nbsp;" + appID + @"<br/>Corporate Permission Key&nbsp;:&nbsp;" + copPerKey;
                string subject = "Email Reject";
                string email = hddEmail.Value;
                sendToEmail(ERPWAuthentication.SID,ERPWAuthentication.CompanyCode,subject,body,email);
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

        public void sendToEmail(string sid, string companyCode, string subject, string body, string email)
        {
            NotificationConfiguration notiConfig = GetNotificationConfig(sid, companyCode);

            SmtpClient smtpClient = GetSmtpClient(notiConfig);
            MailMessage mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.Normal;
            mailMessage.From = new MailAddress(notiConfig.DefaultMailFrom, notiConfig.DefaultMailFromDisplayName);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(body);
            mailMessage.To.Add(new MailAddress(email));
            mailMessage.Subject = subject;//mailContent.Subject;
            mailMessage.Body = sb.ToString();//mailContent.Body;

            SendMail(smtpClient, mailMessage, email);
        }

        private SmtpClient GetSmtpClient(NotificationConfiguration notiConfig)
        {
            SmtpClient smtpClient = new SmtpClient(notiConfig.SMTPHost, notiConfig.SMTPPort);
            smtpClient.UseDefaultCredentials = notiConfig.SMTPUseDefaultCredentials;
            smtpClient.Credentials = new System.Net.NetworkCredential(notiConfig.SMTPCredentialsUsername, notiConfig.SMTPCredentialsPassword);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = notiConfig.EnableSsl;

            return smtpClient;
        }

        private void SendMail(SmtpClient smtpClient, MailMessage mailMessage, string senderEmployeeCode)
        {
            new Thread(() =>
            {
                try
                {
                    log("START_SEND", mailMessage, "", senderEmployeeCode);

                    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    smtpClient.Send(mailMessage);

                    log("SEND_SUCCESS", mailMessage, "", senderEmployeeCode);
                }
                catch (Exception ex)
                {
                    //TODO: Log send mail error
                    log("SEND_ERROR", mailMessage, ex.Message, senderEmployeeCode);
                }
            }).Start();
        }
        public void log(string action, MailMessage mailMessage, string logMessage, string createBy)
        {
            try
            {
                Agape.Lib.DBService.DBService dbService = new Agape.Lib.DBService.DBService();
                string mailFrom = mailMessage.From.Address;
                string subject = mailMessage.Subject;

                List<string> mailToList = new List<string>();
                List<string> mailCCList = new List<string>();
                List<string> mailBCCList = new List<string>();

                if (mailMessage.To != null && mailMessage.To.Count > 0)
                {
                    for (int i = 0; i < mailMessage.To.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(mailMessage.To[i].Address) && !mailToList.Contains(mailMessage.To[i].Address))
                        {
                            mailToList.Add(mailMessage.To[i].Address);
                        }
                    }
                }

                if (mailMessage.CC != null && mailMessage.CC.Count > 0)
                {
                    for (int i = 0; i < mailMessage.CC.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(mailMessage.CC[i].Address) && !mailCCList.Contains(mailMessage.CC[i].Address))
                        {
                            mailCCList.Add(mailMessage.CC[i].Address);
                        }
                    }
                }

                if (mailMessage.Bcc != null && mailMessage.Bcc.Count > 0)
                {
                    for (int i = 0; i < mailMessage.Bcc.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(mailMessage.Bcc[i].Address) && !mailBCCList.Contains(mailMessage.Bcc[i].Address))
                        {
                            mailBCCList.Add(mailMessage.Bcc[i].Address);
                        }
                    }
                }

                string mailTo = string.Join(", ", mailToList);
                string mailCC = string.Join(", ", mailCCList);
                string mailBCC = string.Join(", ", mailBCCList);


                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("INSERT INTO link_email_log");
                queryBuilder.AppendLine("(Action, MailFrom, MailTo, MailCC, MailBCC");
                queryBuilder.AppendLine(", Subject, LogMessage, CREATED_BY, CREATED_ON)");
                queryBuilder.AppendLine("VALUES");
                queryBuilder.AppendLine("('" + action + "'");
                queryBuilder.AppendLine(", '" + mailFrom + "'");
                queryBuilder.AppendLine(", '" + mailTo.ToString() + "'");
                queryBuilder.AppendLine(", '" + mailCC.ToString() + "'");
                queryBuilder.AppendLine(", '" + mailBCC.ToString() + "'");
                queryBuilder.AppendLine(", '" + replaceText(subject) + "'");
                queryBuilder.AppendLine(", '" + replaceText(logMessage) + "'");
                queryBuilder.AppendLine(", '" + createBy + "'");
                queryBuilder.AppendLine(", '" + Validation.getCurrentServerDateTime().ToString("yyyyMMddHHmmss") + "')");

                dbService.executeSQLForFocusone(queryBuilder.ToString());
            }
            catch { }
        }
        private static String replaceText(String inputText)
        {
            if (String.IsNullOrEmpty(inputText))
            {
                return "";
            }

            return inputText.Replace("\'", "\"").Replace("'", "''");
        }

    }
}