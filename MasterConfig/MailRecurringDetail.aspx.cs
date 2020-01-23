using Agape.FocusOne.Utilities;
using ERPW.Lib.Authentication;
using ERPW.Lib.Master.Config;
using ServiceWeb.auth;
using ServiceWeb.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceWeb.MasterConfig
{
    public partial class MailRecurringDetail : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        private RecurringLibrary lib = new RecurringLibrary();

        private string ClientPageID
        {
            get { return Request["pid"]; }
        }

        public DataTable EmailRuledt
        {
            get
            {
                DataTable dt = (DataTable)Session["EmailRuledt" + ClientPageID];

                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("SID", typeof(string));
                    dt.Columns.Add("COMPANYCODE", typeof(string));
                    dt.Columns.Add("BATCH_ID", typeof(string));
                    dt.Columns.Add("RULE_CODE", typeof(string));
                    dt.Columns.Add("SEQ", typeof(int));
                    dt.Columns.Add("MAIL_FROM", typeof(string));
                    dt.Columns.Add("MAIL_TO", typeof(string));
                    dt.Columns.Add("FORWARD_OWNER", typeof(string));
                    dt.Columns.Add("FORWARD_LINKID", typeof(string));
                    dt.Columns.Add("FORWARD_OTHER", typeof(string));
                    dt.Columns.Add("FORWARD_LINKID_Name", typeof(string));
                    dt.Columns.Add("FORWARD_OTHER_Name", typeof(string));
                    dt.Columns.Add("ACTIVE", typeof(string));
                    dt.Columns.Add("created_by", typeof(string));
                    dt.Columns.Add("created_on", typeof(string));
                    Session["EmailRuledt" + ClientPageID] = dt;
                }

                return dt;
            }
            set { Session["EmailRuledt" + ClientPageID] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    EmailRuledt.Clear();                  

                    if (Session["batchid" + ClientPageID] != null)
                    {
                        EditDataBind(Session["batchid" + ClientPageID].ToString());
                    }
                    else
                    {
                        rptItems.DataSource = EmailRuledt;
                        rptItems.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void EditDataBind(string batchId)
        {
            DataTable dtemail = lib.GetDataEdit(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, batchId);

            tbObjective.Text = dtemail.Rows[0]["REMARKS"].ToString();
            tbPop3Server.Text = dtemail.Rows[0]["POP3_HOST"].ToString();
            tbPop3Port.Text = dtemail.Rows[0]["POP3_PORT"].ToString();
            chkUseSSL.Checked = dtemail.Rows[0]["useSSL"].ToString() == "True" ? true : false;
            tbPop3User.Text = dtemail.Rows[0]["POP3_USERNAME"].ToString();
            tbPop3Password.Text = dtemail.Rows[0]["POP3_PASSWORD"].ToString();

            int seqmax = 0;

            DataTable dtrule = lib.GetDataRuleEdit(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, batchId);

            for (int i = 0; dtrule.Rows.Count > i; i++)
            {
                EmailRuledt.Rows.Add(dtrule.Rows[i]["SID"], dtrule.Rows[i]["COMPANYCODE"], dtrule.Rows[i]["BATCH_ID"], dtrule.Rows[i]["RULE_CODE"],
                    dtrule.Rows[i]["SEQ"], dtrule.Rows[i]["MAIL_FROM"], dtrule.Rows[i]["MAIL_TO"]
                    , "", "", "", "", "", dtrule.Rows[i]["ACTIVE"].ToString() == "True" ? "1" : "0"
                    , dtrule.Rows[i]["created_by"], dtrule.Rows[i]["created_on"]);

                int.TryParse(dtrule.Rows[i]["SEQ"].ToString(), out seqmax);
            }

            tbSequence.Text = (seqmax + 1).ToString();

            rptItems.DataSource = EmailRuledt;
            rptItems.DataBind();
        }

        protected void btnValidateSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;

                switch (btn.ID)
                {
                    case "btnValidateSave":
                        ClientService.DoJavascript("saveEmailRecurring();");
                        break;
                    case "btnValidateAddSeq":
                        ClientService.DoJavascript("addSequence();");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private string GetBatchID()
        {
            string batchID = lib.GetBatchID(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode);

            return "RE" + batchID.PadLeft(5, '0');
        }

        private string GetRuleCode()
        {
            int rulecode = EmailRuledt.Rows.Count + 1;

            return "RC" + rulecode.ToString().PadLeft(5, '0');
        }

        private void ClearRule()
        {
            tbSequence.Text = (EmailRuledt.Rows.Count + 1).ToString();
            tbMailFrom.Text = "";
        }

        protected void btnAddSequence_Click(object sender, EventArgs e)
        {
            try
            {
                EmailRuledt.Rows.Add(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, GetBatchID(), GetRuleCode(),
                    tbSequence.Text, tbMailFrom.Text, tbPop3Server.Text
                    , "", "", "", "", "", "1"
                    , ERPWAuthentication.UserName, Validation.getCurrentServerStringDateTime());

                rptItems.DataSource = EmailRuledt;
                rptItems.DataBind();

                ClearRule();
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

        protected void btnDeleteSequence_Click(object sender, EventArgs e)
        {
            try
            {
                string[] keys = (sender as Button).CommandArgument.Split('|');

                string batchId = keys[0];
                string ruleCode = keys[1];

                DataRow[] drr = EmailRuledt.Select("BATCH_ID = '" + batchId + "' and RULE_CODE = '" + ruleCode + "'");

                for (int i = drr.Length - 1; i >= 0; i--)
                {
                    if (drr[i].RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    drr[i].Delete();
                }

                rptItems.DataSource = EmailRuledt;
                rptItems.DataBind();

                ClearRule();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string ActiveChk = "1";

                String useSSL = chkUseSSL.Checked ? "1" : "0";

                string batchId = "";

                if (Session["batchid" + ClientPageID] != null)
                {
                    batchId = Session["batchid" + ClientPageID].ToString();

                    lib.EditRecurringEmail(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, batchId, batchId + "POP", ActiveChk, useSSL,
                        "", "", "", "", "", "", tbObjective.Text, ERPWAuthentication.UserName,
                        Validation.getCurrentServerStringDateTime(), "", tbPop3Server.Text, tbPop3Port.Text, tbPop3User.Text, tbPop3Password.Text, EmailRuledt);                  
                }
                else
                {
                    batchId = GetBatchID();

                    lib.SaveRecurringEmail(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, batchId, batchId + "POP", ActiveChk, useSSL,
                        "", "", "", "", "", "", tbObjective.Text, ERPWAuthentication.UserName,
                        Validation.getCurrentServerStringDateTime(), "", tbPop3Server.Text, tbPop3Port.Text, tbPop3User.Text, tbPop3Password.Text, EmailRuledt);
                }

                try
                {
                    // restart window service abd
                    // Commend for skip between devolpment
                    // *************************************************************************************************
                    RecurringWinSHelper.restartService(ERPWAuthentication.SID);
                    // *************************************************************************************************
                }
                catch(Exception exx)
                {

                }

                string redirect = Page.ResolveUrl("~/MasterConfig/MailRecurringDetail.aspx?pid="+ ClientPageID);
                                
                Session["batchid" + ClientPageID] = batchId;

                ClientService.AGSuccessRedirect("Save Success.", redirect);
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