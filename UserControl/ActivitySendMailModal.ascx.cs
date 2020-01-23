using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using Link.Lib.Model.Model.Timeline;
using Newtonsoft.Json;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.UserControl
{
    public partial class ActivitySendMailModal : System.Web.UI.UserControl
    {
        public const String SENDEMAIL_TYPE_ACTIVITY = "ACTIVITY";

        public const String SENDEMAIL_TYPE_CUSTOM = "CUSTOM";

        public string refAobjectlink
        {
            get
            {
                return txtHiddenAObjectlink_MAIL.Text;
            }
            set
            {
                txtHiddenAObjectlink_MAIL.Text = value;
                udpDataConf.Update();
            }
        }

        public string refTicketNo
        {
            get
            {
                return txtHiddenTicketNo_MAIL.Text;
            }
            set
            {
                txtHiddenTicketNo_MAIL.Text = value;
                udpDataConf.Update();
            }
        }


        public string EmailOwner
        {
            get
            {
                return txtEmailOwner.Text;
            }
            set
            {
                txtEmailOwner.Text = value;
                udpDatasEmail.Update();
            }
        }
        public string EmailMainAssignee
        {
            get
            {
                return txtEmailMainAssignee.Text;
            }
            set
            {
                txtEmailMainAssignee.Text = value;
                udpDatasEmail.Update();
            }
        }
        public string EmailOtherAssignee
        {
            get
            {
                return txtEmailOtherAssignee.Text;
            }
            set
            {
                txtEmailOtherAssignee.Text = value;
                udpDatasEmail.Update();
            }
        }
        public string EmailCustomer
        {
            get
            {
                return txtEmailCustomer.Text;
            }
            set
            {
                txtEmailCustomer.Text = value;
                udpDatasEmail.Update();
            }
        }
        public string EmailContact
        {
            get
            {
                return txtEmailContact.Text;
            }
            set
            {
                txtEmailContact.Text = value;
                udpDatasEmail.Update();
                bindListContact(true);
            }
        }

        public string CustomerCode
        {
            get
            {
                return txtCustomerCode.Text;
            }
            set
            {
                txtCustomerCode.Text = value;
                udpDatasEmail.Update();
            }
        }


        //DataTable _dtContactPerson;
        //DataTable dtContactPerson
        //{
        //    get
        //    {
        //        if (_dtContactPerson == null)
        //        {
        //            DataTable dtContact = AfterSaleService.getInstance().getContactPerson(ERPWAuthentication.CompanyCode, CustomerCode);

        //            _dtContactPerson = new DataTable();
        //            _dtContactPerson.Columns.Add("ContactEmail", typeof(string));
        //            _dtContactPerson.Columns.Add("ContactDisplay", typeof(string));

        //            _dtContactPerson.Rows.Add("", "เลือก");
        //            foreach (DataRow dr in dtContact.Rows)
        //            {
        //                _dtContactPerson.Rows.Add(dr["email"].ToString().Trim(), dr["NAME1"] + " <" + dr["email"].ToString().Trim() + ">");
        //            }
        //        }
        //        return _dtContactPerson;
        //    }
        //}
        //private bool isNullSessionHandle()
        //{
        //    if (string.IsNullOrEmpty(ERPWAuthentication.SID))
        //    {
        //        try
        //        {
        //            new LinkAutoLoginService();
        //        }
        //        catch
        //        {
        //            ClientService.DoJavascript("ErrorAPIHandel('ses');");
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (isNullSessionHandle())
            //    return;
        }
        protected void btnSendMailCustom_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> listEmailTo = txtAllSendMailContainer.Text.Split(',').ToList();
                List<string> listEmailCC = txtAllSendMailContainer_CC.Text.Split(',').ToList();

                processCaseEmailActivity(listEmailTo, listEmailCC);
            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
            finally
            {
                ClientService.AgroLoading(false);
            }
        }

        public void defaultMailTo(List<string> mailTo)
        {
            mailTo = mailTo.Distinct().ToList();

            if (mailTo.Count > 0)
                ClientService.DoJavascript("setDefaultMailTo(['" + string.Join("', '", mailTo) + "']);");
            else
                ClientService.DoJavascript("setDefaultMailTo([]);");
        }

        public void defaultMailCC(List<string> mailCC)
        {
            mailCC = mailCC.Distinct().ToList();

            if (mailCC.Count > 0)
                ClientService.DoJavascript("setDefaultMailCC(['" + string.Join("', '", mailCC) + "']);");
            else
                ClientService.DoJavascript("setDefaultMailCC([]);");
        }

        private void processCaseEmailActivity(List<string> listEmailTo, List<string> listEmailCC)
        {
            string subJect = Request.Form[txtSubjectSendMail.UniqueID];
            string remark = txtRemarkSendMail.HTML;//.Replace("\n", "<br>") + "<hr>";
            string content = remark;

            string refAobjectlink = txtHiddenAObjectlink_MAIL.Text;
            string refEmailCode = Guid.NewGuid().ToString("D") + refAobjectlink;

            DataTable dtFiles = uploadGallery.SaveFiles();

            string SendActivityRefFileJSON = txtAllSendMailRefFiles.Text.Trim();
            if (!string.IsNullOrEmpty(SendActivityRefFileJSON))
            {
                List<ActivitySendMailRefFile> ListRefFile = JsonConvert.DeserializeObject<List<ActivitySendMailRefFile>>(SendActivityRefFileJSON);
                foreach (var item in ListRefFile)
                {
                    DataRow drNew = dtFiles.NewRow();
                    drNew["FileName"] = item.name;
                    drNew["FileExtension"] = Path.GetExtension(item.url).Replace(".", "");
                    drNew["PhysicalFileName"] = item.name;
                    drNew["PhysicalFilePath"] = item.url;
                    dtFiles.Rows.Add(drNew);
                }
            }

            Timeline attachment = SaveEmailActivityAttachment(dtFiles, refAobjectlink, refEmailCode);

            List<EmailConfig> ListEn = MasterEmailConfigLib.GetInstance().GetEmail_Host_Config(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                MasterEmailConfigLib.HostEvent_TICKET
            );

            ServiceTicketLibrary.GetInstance().PostActivityRemarkEmail(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                ERPWAuthentication.CompanyCode,
                refTicketNo,
                refAobjectlink,
                refEmailCode,
                ListEn.First().DefaultMailFromDisplayName,
                ERPWAuthentication.EmployeeCode,
                listEmailTo,
                listEmailCC,
                subJect,
                content,
                attachment,
                ServiceTicketLibrary.ActivityRemarkRefEmail.OUTGOING,
                ERPWAuthentication.FullNameEN
            );

            string ticketStatus = ServiceLibrary.LookUpTable(
                "Docstatus",
                "cs_servicecall_header",
                "where SID = '" + ERPWAuthentication.SID + "' AND CompanyCode = '" + ERPWAuthentication.CompanyCode + "' AND CallerID = '" + refTicketNo + "'"
            );

            if (rdoContact.Checked || rdoRespondCus.Checked)
            {
                string SERVICE_DOC_STATUS_OPEN = ServiceTicketLibrary.GetInstance().GetTicketStatusFromEvent(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    ServiceTicketLibrary.TICKET_STATUS_EVENT_START
                );

                if (ticketStatus == SERVICE_DOC_STATUS_OPEN)
                {
                    string SERVICE_DOC_STATUS_RESPONSECUSTOMER = ServiceTicketLibrary.GetInstance().GetTicketStatusFromEvent(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        ServiceTicketLibrary.TICKET_STATUS_EVENT_RESPONSECUSTOMER
                    );

                    ClientService.DoJavascript("updateStatusAutoBySendMail('" + SERVICE_DOC_STATUS_RESPONSECUSTOMER + @"');");

                    //NotificationLibrary.GetInstance().TicketAlertEvent(
                    //    NotificationLibrary.EVENT_TYPE.TICKET_UPDATESTATUS,
                    //    SID,
                    //    CompanyCode,
                    //    hddDocnumberTran.Value,
                    //    EmployeeCode
                    //);
                }
            }

            
            
            ClientService.DoJavascript("successSendCustomEmail();loadActivityDetailServerCall('" + txtHiddenAObjectlink_MAIL.Text + "');");
        }

        private Timeline SaveEmailActivityAttachment(DataTable dtFiles, string aobjectLink, string refEmailCode)
        {
            int type = Timeline.TYPE_ATTACH_FILE;
            string dateTime = Validation.getCurrentServerStringDateTime();

            Timeline timeLine = new Timeline();
            timeLine.SID = ERPWAuthentication.SID;
            timeLine.CompanyCode = ERPWAuthentication.CompanyCode;
            timeLine.ObjectLink = aobjectLink;
            timeLine.TimelineKey = refEmailCode;
            timeLine.Type = type.ToString();
            timeLine.Message = "";
            timeLine.ContentUri = "";
            timeLine.ContentUrl = "";
            timeLine.Status = "";
            timeLine.Latitude = "";
            timeLine.Longitude = "";
            timeLine.Address = "";
            timeLine.CreatorId = ERPWAuthentication.EmployeeCode;
            timeLine.CreatorName = ERPWAuthentication.FullNameTH;
            timeLine.LinkId = ERPWAuthentication.EmployeeCode;
            timeLine.EmployeeCode = ERPWAuthentication.EmployeeCode;
            timeLine.CreatedOn = dateTime;

            List<TimelineAsset> assets = SaveEmailActivityAttachmentAsset(dtFiles, timeLine.TimelineKey);
            timeLine.TimelineAssets = assets;

            return timeLine;
        }

        private List<TimelineAsset> SaveEmailActivityAttachmentAsset(DataTable Files, string timeLineKey)
        {
            var Request = HttpContext.Current.Request;
            var Server = HttpContext.Current.Server;
            string Domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                           (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

            List<TimelineAsset> assetList = new List<TimelineAsset>();

            int counter = 0;
            foreach (DataRow dr in Files.Rows)
            {
                string Filekey = "FILE" + "_" + Validation.getCurrentServerStringDateTime() + counter.ToString();
                string fileName = dr["PhysicalFileName"].ToString();
                string filePath = Server.MapPath("~" + dr["PhysicalFilePath"].ToString());
                string FileUrl = Domain + dr["PhysicalFilePath"].ToString();

                if (!string.IsNullOrEmpty(fileName))
                {
                    string fileExtensions = Path.GetExtension(fileName).Replace(".", "").ToLower();
                    string[] imageExtensions = new string[] { "jpg", "png", "jpeg", "bmp", "gif" };
                    int assetType = 0;
                    if (imageExtensions.Contains(fileExtensions))
                    {
                        assetType = TimelineAsset.TYPE_IMAGE;
                    }
                    else
                    {
                        assetType = TimelineAsset.TYPE_FILE;
                    }

                    TimelineAsset asset = new TimelineAsset();
                    asset.SID = ERPWAuthentication.SID;
                    asset.CompanyCode = ERPWAuthentication.CompanyCode;
                    asset.ObjectLink = timeLineKey;
                    asset.AssetKey = Filekey;
                    asset.Type = assetType.ToString();
                    asset.ContentUri = filePath;
                    asset.ContentUrl = FileUrl;
                    asset.Latitude = "";
                    asset.Longitude = "";
                    asset.Address = "";
                    asset.CreatedBy = ERPWAuthentication.EmployeeCode;
                    asset.CreatedOn = Validation.getCurrentServerStringDateTime();

                    assetList.Add(asset);
                }
                counter++;
            }
            return assetList;
        }

        public class ActivitySendMailRefFile
        {
            public string url { get; set; }
            public string name { get; set; }
            public string uploader { get; set; }
            public string source { get; set; }
        }

        protected void rdoCustomMail_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton rdo = (sender as RadioButton);
                if (rdo.ID == "rdoAssignee")
                {
                    DataTable dt = ServiceTicketLibrary.GetInstance().GetTicketDetail(
                        ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, refTicketNo
                    );

                    string content = NotificationLibrary.GetInstance().getContentEmailEmp(
                         ERPW.Lib.Service.NotificationLibrary.EVENT_TYPE.TICKET_COMMENT,
                         ERPWAuthentication.SID,
                         ERPWAuthentication.CompanyCode,
                         refTicketNo,
                         dt
                     );
                    content = content.Replace("[Comment]", "[Custom Mail]");

                    txtRemarkSendMail.HTML = content;
                    txtRemarkSendMail.Init();
                    udpHtmlEditer.Update();

                    defaultMailTo(EmailMainAssignee.Split(',').ToList());
                    defaultMailCC(EmailOtherAssignee.Split(',').ToList());
                }
                else if (rdo.ID == "rdoOwner")
                {
                    DataTable dt = ServiceTicketLibrary.GetInstance().GetTicketDetail(
                        ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, refTicketNo
                    );

                    string content = NotificationLibrary.GetInstance().getContentEmailEmp(
                         ERPW.Lib.Service.NotificationLibrary.EVENT_TYPE.TICKET_COMMENT,
                         ERPWAuthentication.SID,
                         ERPWAuthentication.CompanyCode,
                         refTicketNo,
                         dt
                     );
                    content = content.Replace("[Comment]", "[Custom Mail]");

                    txtRemarkSendMail.HTML = content;
                    txtRemarkSendMail.Init();
                    udpHtmlEditer.Update();

                    defaultMailTo(EmailOwner.Split(',').ToList());
                    defaultMailCC(new List<string>());
                }
                else if (rdo.ID == "rdoContact")
                {
                    DataTable dt = ServiceTicketLibrary.GetInstance().GetTicketDetail(
                        ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, refTicketNo
                    );

                    string subject = NotificationLibrary.GetInstance().getContentEmailClientByEvent(
                         ERPW.Lib.Service.NotificationLibrary.EVENT_TYPE.CUSTOM_MAIL,
                         dt,
                         true
                     );
                    string content = NotificationLibrary.GetInstance().getContentEmailClientByEvent(
                         ERPW.Lib.Service.NotificationLibrary.EVENT_TYPE.CUSTOM_MAIL,
                         dt
                     );
                    content = content.Replace("[Open]", "");

                    txtSubjectSendMail.Text = subject;
                    udpSubject.Update();

                    txtRemarkSendMail.HTML = content;
                    txtRemarkSendMail.Init();
                    udpHtmlEditer.Update();



                    List<string> listMailTo = new List<string>();
                    List<string> listMailCC = new List<string>();

                    //if (string.IsNullOrEmpty(EmailContact))
                    //{
                    //    listMailTo.AddRange(EmailCustomer.Split(',').ToList());
                    //    listMailCC.AddRange(new List<string>());
                    //}
                    //else
                    //{
                    //    listMailTo.AddRange(EmailContact.Split(',').ToList());
                    //    listMailCC.AddRange(EmailCustomer.Split(',').ToList());
                    //}

                    try
                    {
                        listMailTo.AddRange(
                            NotificationLibrary.GetInstance().getEmailDefaultConfig(
                            ERPWAuthentication.SID, 
                            ERPWAuthentication.CompanyCode, 
                            "TO")
                        );
                    }
                    catch (Exception)
                    {

                    }

                    try
                    {
                        listMailCC.AddRange(
                            NotificationLibrary.GetInstance().getEmailDefaultConfig(
                            ERPWAuthentication.SID, 
                            ERPWAuthentication.CompanyCode, 
                            "CC")
                        );
                    }
                    catch (Exception)
                    {
                    }


                    defaultMailTo(listMailTo);
                    defaultMailCC(listMailCC);

                    ClientService.DoJavascript("getEmailFromDescription('.txt-ticket-Descript');");
                }
                else if (rdo.ID == "rdoRespondCus")
                {
                    DataTable dt = ServiceTicketLibrary.GetInstance().GetTicketDetail(
                        ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, refTicketNo
                    );

                    string subject = NotificationLibrary.GetInstance().getContentEmailClientByEvent(
                         ERPW.Lib.Service.NotificationLibrary.EVENT_TYPE.CUSTOM_MAIL_2,
                         dt,
                         true
                     );
                    string content = NotificationLibrary.GetInstance().getContentEmailClientByEvent(
                         ERPW.Lib.Service.NotificationLibrary.EVENT_TYPE.CUSTOM_MAIL_2,
                         dt
                     );
                    content = content.Replace("[Open]", "");
                    //content = content.Replace("<", "");
                    //content = content.Replace(">", "");

                    txtSubjectSendMail.Text = subject;
                    udpSubject.Update();

                    txtRemarkSendMail.HTML = content;
                    txtRemarkSendMail.Init();
                    udpHtmlEditer.Update();


                    List<string> listMailTo = new List<string>();
                    List<string> listMailCC = new List<string>();

                    //if (string.IsNullOrEmpty(EmailContact))
                    //{
                    //    listMailTo.AddRange(EmailCustomer.Split(',').ToList());
                    //    listMailCC.AddRange(new List<string>());
                    //}
                    //else
                    //{
                    //    listMailTo.AddRange(EmailContact.Split(',').ToList());
                    //    listMailCC.AddRange(EmailCustomer.Split(',').ToList());
                    //}

                    try
                    {
                        listMailTo.AddRange(
                            NotificationLibrary.GetInstance().getEmailDefaultConfig(
                            ERPWAuthentication.SID,
                            ERPWAuthentication.CompanyCode,
                            "TO")
                        );
                    }
                    catch (Exception)
                    {

                    }

                    try
                    {
                        listMailCC.AddRange(
                            NotificationLibrary.GetInstance().getEmailDefaultConfig(
                            ERPWAuthentication.SID,
                            ERPWAuthentication.CompanyCode,
                            "CC")
                        );
                    }
                    catch (Exception)
                    {
                    }

                    defaultMailTo(listMailTo);
                    defaultMailCC(listMailCC);


                    ClientService.DoJavascript("getEmailFromDescription('.txt-ticket-Descript');");
                }
                else if (rdo.ID == "rdoCustom")
                {
                    txtRemarkSendMail.HTML = "";
                    txtRemarkSendMail.Init();
                    udpHtmlEditer.Update();

                    defaultMailTo(new List<string>());
                    defaultMailCC(new List<string>());
                }
                else
                {
                    txtRemarkSendMail.HTML = "";
                    txtRemarkSendMail.Init();
                    udpHtmlEditer.Update();

                    defaultMailTo(new List<string>());
                    defaultMailCC(new List<string>());
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

        public void bindListContact(bool isSelectMailContact)
        {
            if (isSelectMailContact)
            {
                pnlFromSendMail.CssClass = "form-group col-lg-6";
                pnlSelectMailContact.Visible = true;

                //ddlListContact.DataValueField = "ContactEmail";
                //ddlListContact.DataTextField = "ContactDisplay";
                //ddlListContact.DataSource = dtContactPerson;
                //ddlListContact.DataBind();

                DataTable dt = ERPW.Lib.Master.CustomerService.getInstance().getListContactDetailByCustomer(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    CustomerCode,
                    false
                );

                rptLisContact.DataSource = dt;
                rptLisContact.DataBind();
            }
            else
            {
                pnlFromSendMail.CssClass = "form-group col-lg-12";
                pnlSelectMailContact.Visible = false;
            }

            udpMailPanel.Update();
        }

    }
}