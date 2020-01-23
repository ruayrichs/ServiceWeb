using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using ERPW.Lib.Master.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig
{
    public partial class EmailMessageDetail : System.Web.UI.Page
    {
        private const String M_GROUPCODE = "EMAIL_MESSAGE";
        private string Guid { get { return Request["id"]; } }
        private String rowkey
        {
            get { return Session["EmailMessageCriteria_rowkey"+ Guid] == null ? "" : (String)Session["EmailMessageCriteria_rowkey"+ Guid]; }
            set { Session["EmailMessageCriteria_rowkey"+ Guid] = value; }
        }

        private MasterConfigLibrary libmaster = MasterConfigLibrary.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!(String.IsNullOrEmpty(rowkey)))
                    {
                        initData(rowkey);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void initData(String rowkey)
        {
            ActivityEmailMessage entity = libmaster.getActivityEmailMessage(ERPWAuthentication.SID,rowkey);
            txtMessageCode.Enabled = false;
            txtMessageCode.Text = entity.messageCode;
            txtEventCode.Text = entity.eventcode;
            txtRemark.Text = entity.remark;
            txtEventName.Text = entity.eventname;
            txtEmailSubject.Text = entity.emailSubject;
            HtmlEditor.HTML = entity.emailContentHTML;
            txtCreatedOn.Text = entity.displayCreatedOn;
            txtCreatedBy.Text = entity.createdBy;
            txtUpdatedOn.Text = entity.displayUpdatedOn;
            txtUpdatedBy.Text = entity.updatedBy;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> listMes = new List<string>();
                if (string.IsNullOrEmpty(txtMessageCode.Text.Trim()))
                {
                    listMes.Add("กรุณาระบุรหัสข้อความ !!");
                }
                if (string.IsNullOrEmpty(txtEmailSubject.Text.Trim()))
                {
                    listMes.Add("กรุณาระบุหัวเรื่องอีเมลล์ !!");
                }
                
                if (listMes.Count > 0)
                {
                    throw new Exception(String.Join("<br>", listMes));
                }
                ActivityEmailMessage entity = createEntityFromUI();
                
                bool iscreate = libmaster.isCreateEmailMessage(entity.rowkey);
                if (iscreate)
                {
                    libmaster.insertActivityEmailMessage(entity);
                }
                else
                {
                    libmaster.updateActivityEmailMessage(entity.rowkey, entity);
                }
                rowkey = entity.rowkey;
                HtmlEditor.WriteFileConfig(entity.sid, M_GROUPCODE, entity.rowkey);
                ClientService.AGSuccessRedirect("บันทึกสำเร็จ", "EmailMessageDetail.aspx?id="+ Guid);
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

        protected void btnValidateSave_Click(object sender, EventArgs e)
        {
            ClientService.DoJavascript("saveEmailMessage();");
        }

        private ActivityEmailMessage createEntityFromUI()
        {
            ActivityEmailMessage entity = new ActivityEmailMessage();
            entity.sid = ERPWAuthentication.SID;
            entity.messageCode = txtMessageCode.Text;
            entity.remark = txtRemark.Text;
            entity.emailSubject = txtEmailSubject.Text;
            entity.emailContent = HtmlEditor.GetConfigFilePathTempFile(entity.sid, M_GROUPCODE, entity.sid+ entity.messageCode);
            entity.ContentPhysicalPath = HtmlEditor.GetConfigFilePathTempFile(entity.sid, M_GROUPCODE, entity.sid + entity.messageCode,true);
            entity.emailContentHTML = HtmlEditor.HTML;
            entity.eventcode = txtEventCode.Text;
            entity.createdBy = ERPWAuthentication.UserName;
            entity.updatedBy = ERPWAuthentication.UserName;
            return entity;
        }
    }
}