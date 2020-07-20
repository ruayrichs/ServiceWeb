using System;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Entity;
using ERPW.Lib.Authentication;
using ServiceWeb.auth;
using System.Data;
using System.Collections.Generic;

namespace ServiceWeb.MasterConfig
{
    public partial class MasterEmailConfig : AbstractsSANWebpage
    {
        protected override Boolean ProgramPermission_CanView()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }

        protected override Boolean ProgramPermission_CanModify()
        {
            return ERPWAuthentication.Permission.AllPermission;
        }
        MasterEmailConfigLib Obj = new MasterEmailConfigLib();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                   dataBinding();
                }
            }
            catch (Exception ex)
            {
                ClientService.AGError(ex.Message);
            }
        }

        private void dataCheckInput()
        {
            if (string.IsNullOrEmpty(tbHost.Text))
            {
                throw new Exception("กรุุณาระบุ Host");
            }
            if (string.IsNullOrEmpty(tbPort.Text))
            {
                throw new Exception("กรุณาระบุ Port");
            }
            if (string.IsNullOrEmpty(tbUsername.Text))
            {
                throw new Exception("กรุณาระบุ User name");
            }
            if (string.IsNullOrEmpty(tbPassword.Text))
            {
                throw new Exception("กรุณาระบุ Password");
            }
            if (string.IsNullOrEmpty(tbMailFrom.Text))
            {
                throw new Exception("กรุณาระบุ Default Mail From");
            }
            if (string.IsNullOrEmpty(tbDisplay.Text))
            {
                throw new Exception("กรุณาระบุ Default Mail From Display Name");
            }

        }
        private void dataBinding()
        {
            List<EmailConfig> ListEmailConfigEn = Obj.GetEmail_Host_Config(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                MasterEmailConfigLib.HostEvent_TICKET
            );

            if (ListEmailConfigEn.Count > 0)
            {
                EmailConfig EmailEntityAdd = ListEmailConfigEn[0];
                tbHost.Text = EmailEntityAdd.SMTPHost;
                tbPort.Text = EmailEntityAdd.SMTPPort;
                tbUsername.Text = EmailEntityAdd.SMTPCredentialsUsername;
                tbPassword.Text = EmailEntityAdd.SMTPCredentialsPassword;
                tbMailFrom.Text = EmailEntityAdd.DefaultMailFrom;
                tbDisplay.Text = EmailEntityAdd.DefaultMailFromDisplayName;
                CBcredentials.Checked = EmailEntityAdd.SMTPUseDefaultCredentials;
                CBsenders.Checked = EmailEntityAdd.MailFromSender;
                CBEnableSSL.Checked = EmailEntityAdd.EnableSsl;
                CBTicketOpen.Checked = EmailEntityAdd.AlertEventTicketOpen;
                CBTicketTransfer.Checked = EmailEntityAdd.AlertEventTicketTransfer;
                CBTicketEscalate.Checked = EmailEntityAdd.AlertEventTicketEscalate;
                CBTicketUpdateStatus.Checked = EmailEntityAdd.AlertEventTicketUpdatestatus;
                CBTicketComment.Checked = EmailEntityAdd.AlertEventTicketComment;
                CBTicketResolve.Checked = EmailEntityAdd.AlertEventTicketResolve;
                CBTicketClose.Checked = EmailEntityAdd.AlertEventTicketClose;
                CBTicketCancel.Checked = EmailEntityAdd.AlertEventTicketCancle;
                CBEV_RespondCustomer.Checked = EmailEntityAdd.AlertEventRespondCustomer;
                CBEV_EventToOwner.Checked = EmailEntityAdd.AlertEventToOwner;
                CBEV_ChangeOrderApproval.Checked = EmailEntityAdd.AlertEventChangeOrderApproval;
                CBTicketOpen2Customer.Checked = EmailEntityAdd.AlertEventTicketOpenToCustomer;
                CBTicketComment2Customer.Checked = EmailEntityAdd.AlertEventTicketCommentToCustomer;
                CBTicketUpdatestatus2Customer.Checked = EmailEntityAdd.AlertEventTicketUpdatestatusToCustomer;
                CBTicketEscalate2Customer.Checked = EmailEntityAdd.AlertEventTicketEscalateToCustomer;
                CBTicketTransfer2Customer.Checked = EmailEntityAdd.AlertEventTicketTransferToCustomer;
                CBTicketOverDue2Customer.Checked = EmailEntityAdd.AlertEventTicketOverDueToCustomer;
                CBTicketResolve2Customer.Checked = EmailEntityAdd.AlertEventTicketResolveToCustomer;
                CBTicketApproval2Customer.Checked = EmailEntityAdd.AlertEventTicketApprovalToCustomer;
                CBTicketClose2Customer.Checked = EmailEntityAdd.AlertEventTicketCloseToCustomer;
                CBTicketCancel2Customer.Checked = EmailEntityAdd.AlertEventTicketCancelToCustomer;
                CBTicketOverDue.Checked = EmailEntityAdd.AlertEventOverDue;
                CBTCIChangeLog2Customer.Checked = EmailEntityAdd.AlertEventCIChangeLogToCustomer;
            }
        }

        protected void btn_create_Click(object sender, EventArgs e)
        {
            try
            {
                dataCheckInput();
                EmailConfig EmailEntity = new EmailConfig();
                EmailEntity.SMTPHost = tbHost.Text;
                EmailEntity.SMTPPort = tbPort.Text;
                EmailEntity.HostEvent = "TICKET";
                EmailEntity.SMTPUseDefaultCredentials = CBcredentials.Checked;
                EmailEntity.MailFromSender = CBsenders.Checked;
                EmailEntity.SMTPCredentialsUsername = tbUsername.Text;
                EmailEntity.SMTPCredentialsPassword = tbPassword.Text;
                EmailEntity.DefaultMailFrom = tbMailFrom.Text;
                EmailEntity.DefaultMailFromDisplayName = tbDisplay.Text;
                EmailEntity.EnableSsl = CBEnableSSL.Checked;
                EmailEntity.AlertEventTicketOpen = CBTicketOpen.Checked;
                EmailEntity.AlertEventTicketTransfer = CBTicketTransfer.Checked;
                EmailEntity.AlertEventTicketEscalate = CBTicketEscalate.Checked;
                EmailEntity.AlertEventTicketUpdatestatus = CBTicketUpdateStatus.Checked;
                EmailEntity.AlertEventTicketComment = CBTicketComment.Checked;
                EmailEntity.AlertEventTicketResolve = CBTicketResolve.Checked;
                EmailEntity.AlertEventTicketClose = CBTicketClose.Checked;
                EmailEntity.AlertEventTicketCancle = CBTicketCancel.Checked;
                EmailEntity.AlertEventRespondCustomer = CBEV_RespondCustomer.Checked;
                EmailEntity.AlertEventToOwner = CBEV_EventToOwner.Checked;
                EmailEntity.AlertEventChangeOrderApproval = CBEV_ChangeOrderApproval.Checked;
                EmailEntity.AlertEventTicketOpenToCustomer = CBTicketOpen2Customer.Checked;
                EmailEntity.AlertEventTicketCommentToCustomer = CBTicketComment2Customer.Checked;
                EmailEntity.AlertEventTicketUpdatestatusToCustomer = CBTicketUpdatestatus2Customer.Checked;
                EmailEntity.AlertEventTicketEscalateToCustomer = CBTicketEscalate2Customer.Checked;
                EmailEntity.AlertEventTicketTransferToCustomer = CBTicketTransfer2Customer.Checked;
                EmailEntity.AlertEventTicketOverDueToCustomer = CBTicketOverDue2Customer.Checked;
                EmailEntity.AlertEventTicketResolveToCustomer = CBTicketResolve2Customer.Checked;
                EmailEntity.AlertEventTicketApprovalToCustomer = CBTicketApproval2Customer.Checked;
                EmailEntity.AlertEventTicketCloseToCustomer = CBTicketClose2Customer.Checked;
                EmailEntity.AlertEventTicketCancelToCustomer = CBTicketCancel2Customer.Checked;
                EmailEntity.AlertEventOverDue = CBTicketOverDue.Checked;
                EmailEntity.AlertEventCIChangeLogToCustomer = CBTCIChangeLog2Customer.Checked;

                Obj.addEmail_Host_Config(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    EmailEntity,
                    MasterEmailConfigLib.HostEvent_TICKET,
                    ERPWAuthentication.EmployeeCode
                );

                ClientService.AGSuccess("บันทึกสำเร็จ");
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