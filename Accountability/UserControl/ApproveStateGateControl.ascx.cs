using Agape.FocusOne.Utilities;
using ServiceWeb.Accountability.Service;
using ERPW.Lib.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPW.Lib.Service.Workflow;
using Agape.Lib.Web.Bean.CS;
using ServiceWeb.Service;
using ServiceWeb.crm.AfterSale;
using SNA.Lib.Initiative;
using ERPW.Lib.Service;
using ERPW.Lib.Service.Workflow.Entity;
using ERPW.Lib.Master;
using ERPW.Lib.Master.Entity;
using System.Web.UI.HtmlControls;

namespace ServiceWeb.Accountability.UserControl
{
    public partial class ApproveStateGateControl : System.Web.UI.UserControl
    {
        string ThisPage = "ApproveStateGateControl";
        public string WorkGroupCode
        {
            get
            {
                return hddWorkGroupCodeApprovalControl.Value;
            }
            set
            {
                hddWorkGroupCodeApprovalControl.Value = value;
                udpCodeidentityInitiativeAS.Update();
            }
        }
        public string InitiativeCode
        {
            get
            {
                return hddInitiativeCodeApprovalControl.Value;
            }
            set
            {
                hddInitiativeCodeApprovalControl.Value = value;
                udpCodeidentityInitiativeAS.Update();
            }
        }

        public string StategateCode
        {
            get
            {
                return hddWorkStategateApprovalControl.Value;
            }
            set
            {
                hddWorkStategateApprovalControl.Value = value;
                udpCodeidentityInitiativeAS.Update();
            }
        }

        public string TicketCode 
        {
            get
            {
                return hddTicketCodeApprovalControl.Value;
            }
            set
            {
                hddTicketCodeApprovalControl.Value = value;
                udpCodeidentityInitiativeAS.Update();
            }
        }

        public string TicketStatusCodeTarget
        {
            get
            {
                return hddTikcetStatusCodeTargetApprovalControl.Value;
            }
            set
            {
                hddTikcetStatusCodeTargetApprovalControl.Value = value;
                udpCodeidentityInitiativeAS.Update();
            }
        }

        public string TicketDocumentType
        {
            get
            {
                return hddTicketDocumentType.Value;
            }
            set
            {
                hddTicketDocumentType.Value = value;
                udpCodeidentityInitiativeAS.Update();
            }
        }

        public string TicketFiscalYear
        {
            get
            {
                return hddTicketFiscalYear.Value;
            }
            set
            {
                hddTicketFiscalYear.Value = value;
                udpCodeidentityInitiativeAS.Update();
            }
        }

        public string TicketDocumentNo
        {
            get
            {
                return hddTicketDocumentNo.Value;
            }
            set
            {
                hddTicketDocumentNo.Value = value;
                udpCodeidentityInitiativeAS.Update();
            }
        }

        public string TicketStatusCodeOld
        {
            get
            {
                return hddTicketStatusCodeOld.Value;
            }
            set
            {
                hddTicketStatusCodeOld.Value = value;
                udpCodeidentityInitiativeAS.Update();
            }
        }

        //private InitiativeManagementCenter initService = InitiativeManagementCenter.getInstance();
        #region TableName
        private const string ERPW_WF = "ERPW_WF";
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Trig()
        {
            try
            {
                string CurStateGate = "L0";
                string[] StateGate = WorkflowService.getInstance().GetApprovalHeaderPresent(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    WorkGroupCode, 
                    InitiativeCode
                ).Split(',');

                if (StateGate.Length == 3)
                {
                    CurStateGate = StateGate[0];
                }
                else
                {
                    CurStateGate = "L5";
                }

                pnlControls.Visible = CurStateGate.Equals(StategateCode);
                udpControls.Update();

                if (CurStateGate.Equals(StategateCode))
                {
                    ControlState();
                }


            }
            catch (Exception ex)
            {
                ClientService.AGError(ObjectUtil.Err(ex.Message));
            }
        }

        private void ControlState()
        {
            btnDowngradedStateGateAS.Visible = false;

            btnApprovelCancelInitiativeAS.Visible = false;
            btnRejectCancelInitiativeAS.Visible = false;

            btnApprovelDowngradedInitiativeAS.Visible = false;
            btnRejectDowngradedInitiativeAS.Visible = false;

            btnApproveAS.Visible = false;
            btnRejectAS.Visible = false;

            //update
            btnDocumentAS.Visible = false;
            //update

            btnCancelRequestApprovalInitiative.Visible = false;
            btnCancelRequestDowngradeInitiative.Visible = false;
            btnCancelRequestCancelInitiative.Visible = false;

            pnlApprovalButtonApproveOrRejectAS.Visible = true;
            pnlApprovalButtonCommand.Visible = true;

            // for check upgrade
            string[] StateGate = WorkflowService.getInstance().GetApprovalHeaderPresent(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, WorkGroupCode, InitiativeCode).Split(',');
            // for check downgrade
            string[] DownGradedStateGete = WorkflowService.getInstance().GetdowngradedHeaderPresent(
                ERPWAuthentication.SID,
                ERPWAuthentication.CompanyCode,
                WorkGroupCode,
                InitiativeCode
                ).Split(',');
            // for check wait cancel
            DataTable dt = WorkflowService.getInstance().getInitiative(
                "", 
                WorkGroupCode,
                InitiativeCode
            );
            bool IsInitiativeOwner = dt.Rows.Count > 0 ? dt.Rows[0]["EMPCODE"].Equals(ERPWAuthentication.EmployeeCode) : false;

            btnUpdateStateGateAS.Visible = IsInitiativeOwner;
            btnDowngradedStateGateAS.Visible = IsInitiativeOwner;
            btnRequestCancelInitiativeAS.Visible = IsInitiativeOwner;

            if (StateGate.Length == 3)
            {
                hddStateGateAS.Value = StateGate[0];
                hddStateGateEndAS.Value = StateGate[1];
                hddApproveStartedAS.Value = StateGate[2];
            }

            if (!hddStateGateAS.Value.Equals("L0"))
            {
                btnDowngradedStateGateAS.Visible = true && IsInitiativeOwner;
            }
            else
            {
                btnDowngradedStateGateAS.Visible = false;

            }

            if (DownGradedStateGete.Length > 2)
            {

                hddDownGradedStateGeteFromAS.Value = DownGradedStateGete[0];
                hddDownGradedStateGeteToAS.Value = DownGradedStateGete[1];
                hddDownGradedStateGeteStatusAS.Value = DownGradedStateGete[2];

                if (!DownGradedStateGete[2].Equals("TRUE"))
                {
                    string DownFrom = DownGradedStateGete[1];
                    string DownTo = DownGradedStateGete[0];

                    btnDowngradedStateGateAS.Text = "Submited for step down approval";
                }
                else
                {
                    // cancel Downgrade
                    btnCancelRequestDowngradeInitiative.Visible = IsInitiativeOwner;
                }
            }

            if (hddDownGradedStateGeteStatusAS.Value == "" ? false : Convert.ToBoolean(hddDownGradedStateGeteStatusAS.Value) 
                && WorkflowService.getInstance().CheckIsPersonParicipantApproveDowngrade(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, WorkGroupCode, InitiativeCode, hddStateGateAS.Value, hddStateGateEndAS.Value, ERPWAuthentication.EmployeeCode))
            {
                lbTextApproveAS.Text = "<b class='text-primary'>Submited for  step down approval</b> : " +
                    WorkflowService.getInstance().CheckPersonParicipant(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode, 
                    InitiativeCode
                );

                btnApprovelDowngradedInitiativeAS.Visible = true;
                btnRejectDowngradedInitiativeAS.Visible = true;
            }


            if (Convert.ToBoolean(hddApproveStartedAS.Value))
            {
                if (WorkflowService.getInstance().CheckIsPersonParicipantApproveUpgrade(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, WorkGroupCode, InitiativeCode, hddStateGateAS.Value, hddStateGateEndAS.Value, ERPWAuthentication.EmployeeCode))
                {
                    lbTextApproveAS.Text = "<b class='text-primary'>Submited for " + StateGate[1] + " approval</b> : " + WorkflowService.getInstance().CheckPersonParicipant(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, InitiativeCode);
                    btnApproveAS.Visible = true;
                    btnRejectAS.Visible = true;
                    btnDocumentAS.Visible = true;
                }

                // cancel Upgrade
                btnCancelRequestApprovalInitiative.Visible = IsInitiativeOwner;
                btnUpdateStateGateAS.Visible = false;
                btnDowngradedStateGateAS.Visible = false;
                btnRequestCancelInitiativeAS.Visible = false;
            }

            if (StateGate.Length == 3)
            {
                btnUpdateStateGateAS.Text = "Submited for " + StateGate[1] + " approval";
            }

            foreach (DataRow dr in dt.Rows)
            {
                bool IsParticipantCancel = WorkflowService.getInstance().CheckIsPersonParicipantApproveCancel(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    WorkGroupCode,
                    InitiativeCode,
                    ERPWAuthentication.EmployeeCode
                );
                if (dr["XSTATUS"].Equals("WAITCANCEL") && IsParticipantCancel)
                {
                    lbTextApproveAS.Text = "<b class='text-primary'>Submited for cancel document approval</b> : " + WorkflowService.getInstance().CheckPersonParicipant(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, InitiativeCode);
                    btnSaveRequestCancelWeeklyStatusModelControlAS.Visible = false;

                    btnApprovelCancelInitiativeAS.Visible = true;
                    btnRejectCancelInitiativeAS.Visible = true;

                    // cancel request cancel
                    btnCancelRequestCancelInitiative.Visible = IsInitiativeOwner;
                }
                else
                {
                    btnSaveRequestCancelWeeklyStatusModelControlAS.Visible = true;
                }
            }

            pnlApprovalButtonApproveOrRejectAS.Visible =
                        btnApprovelCancelInitiativeAS.Visible
                        || btnRejectCancelInitiativeAS.Visible
                        || btnApprovelDowngradedInitiativeAS.Visible
                        || btnRejectDowngradedInitiativeAS.Visible
                        || btnApproveAS.Visible
                        || btnRejectAS.Visible;

            pnlApprovalButtonCommand.Visible = !pnlApprovalButtonApproveOrRejectAS.Visible;

            panelbtnCancelRequestApproval.Visible =
                btnCancelRequestApprovalInitiative.Visible ||
                btnCancelRequestDowngradeInitiative.Visible ||
                btnCancelRequestCancelInitiative.Visible;

            udpControls.Update();
        }
        #region update status
        ServiceTicketLibrary lib = new ServiceTicketLibrary();
        private void updateTicketStatusToTarget(string SID, string CompanyCode, string DocumentType, string FiscalYear, string DocumentNo, string CreateBy, string statusOld)
        {
            string TICKET_STATUS_EVENT_CODE = TicketStatusCodeTarget;
            ServiceCallTransaction c = (ServiceCallTransaction)this.Parent.Page;

            if (!String.IsNullOrEmpty(TICKET_STATUS_EVENT_CODE))
            {
                AfterSaleService.getInstance().UpdateStatus(
                SID,
                CompanyCode,
                TICKET_STATUS_EVENT_CODE,
                DocumentType,
                FiscalYear,
                DocumentNo,
                c.UserName,
                Validation.getCurrentServerStringDateTime());


                //string TICKET_STATUS_EVENT_OLD_DESC = ServiceTicketLibrary.GetTicketDocStatusDesc(SID, CompanyCode, statusOld);
                //string TICKET_STATUS_EVENT_NEW_DESC = ServiceTicketLibrary.GetTicketDocStatusDesc(SID, CompanyCode, TICKET_STATUS_EVENT_CODE);


                //string quoteMessage = "Update status from \"" + TICKET_STATUS_EVENT_OLD_DESC + "\" to \"" + TICKET_STATUS_EVENT_NEW_DESC + "\"";
                //List<logValue_OldNew> enLog = new List<logValue_OldNew>();
                //enLog.Add(new logValue_OldNew
                //{
                //    Value_Old = "",
                //    Value_New = quoteMessage,
                //    TableName = "",
                //    FieldName = "",
                //    AccessCode = LogServiceLibrary.AccessCode_Updata_Status
                //});
                //SaveLog(SID
                //    , CompanyCode
                //    , DocumentType
                //    , FiscalYear
                //    , DocumentNo
                //    , CreateBy
                //    , enLog);

                postremark(DocumentNo, statusOld, TICKET_STATUS_EVENT_CODE, "");

                string SERVICE_DOC_STATUS_RESPONSECUSTOMER = lib.GetTicketStatusFromEvent(SID, CompanyCode, ServiceTicketLibrary.TICKET_STATUS_EVENT_RESPONSECUSTOMER);

                if (TICKET_STATUS_EVENT_CODE == SERVICE_DOC_STATUS_RESPONSECUSTOMER)
                {
                    string ResponseDate = Validation.getCurrentServerStringDateTime().Substring(0, 8);
                    string ResponseTime = Validation.getCurrentServerDateTime().ToString("HHmmss");
                    string ResponseBy = CreateBy;

                    c.saveTimetampResponseToCustomer(ResponseDate, ResponseTime, ResponseBy);
                }

                HiddenField hddTicketStatus_Old = c.parenthddTicketStatus_Old as HiddenField;
                HiddenField hddTicketStatus_New = c.parenthddTicketStatus_New as HiddenField;
                HiddenField hddTicketStatus = c.parenthddTicketStatus as HiddenField;
                UpdatePanel udpHiddenCode = c.parentudpHiddenCode as UpdatePanel;
                HtmlInputText _txt_TicketStatusTran = c.parent_txt_TicketStatusTran as HtmlInputText;
                UpdatePanel udpTicketStatusTran = c.parentudpTicketStatusTran as UpdatePanel;
                UpdatePanel updPerson = c.parentupdPerson as UpdatePanel;

                hddTicketStatus_New.Value = TICKET_STATUS_EVENT_CODE;
                hddTicketStatus_Old.Value = hddTicketStatus_New.Value;
                hddTicketStatus.Value = hddTicketStatus_New.Value; 
                _txt_TicketStatusTran.Value = TICKET_STATUS_EVENT_CODE + " : " + ServiceTicketLibrary.GetTicketDocStatusDesc(SID, CompanyCode, TICKET_STATUS_EVENT_CODE);
                ;
                udpTicketStatusTran.Update();
                udpHiddenCode.Update();
                updPerson.Update();
            }

        }
        private void postremark(string ticketNumber, string statusCodeOld, string statusCodeNew, string remarkMessage)
        {
            ServiceLibrary libService = new ServiceLibrary();

            string aobjectlink = AfterSaleService.getInstance().getAobjectLinkByTicketNumber(ticketNumber);
            if (string.IsNullOrEmpty(aobjectlink))
            {
                throw new Exception("access denied.");
            }

            string comment = remarkMessage;
            string type = "REMARK";

            string curDateTime = Validation.getCurrentServerStringDateTimeMillisecond();

            string messageQuote = "";
            string quoteType = "";

            if (!string.IsNullOrEmpty(statusCodeNew) && statusCodeNew != statusCodeOld)
            {
                quoteType = "UPDATESTATUS";
                messageQuote = @"Update status from " + "\"" + ServiceTicketLibrary.GetTicketDocStatusDesc(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, statusCodeOld) + "\"" + " to " + "\"" + ServiceTicketLibrary.GetTicketDocStatusDesc(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, statusCodeNew) + "\"";
                //messageQuote = messageQuote.Replace("'", "\"");
            }

            libService.SaveActivityDetail(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    ERPWAuthentication.CompanyCode,
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.FullNameEN,
                    aobjectlink,
                    "",
                    comment,
                    "",
                    curDateTime,
                    type,
                    quoteType,
                    messageQuote,
                    ""
                );
        }
        private void SaveLog(string SID, string CompanyCode, string DocumentType, string FiscalYear, string DocumentNo, string CreateBy, List<logValue_OldNew> enLog)
        {

            if (enLog.Count == 0)
            {
                return;
            }

            string DocType = DocumentType;

            List<Main_LogService> en = AfterSaleService.getInstance().SaveLogTicket(SID, DocType, FiscalYear, DocumentNo, CompanyCode,
                CreateBy, enLog);

        }
        #endregion
        protected void btnConfirmRequestApprovelInitiativeModelControl_Click(object sender, EventArgs e)
        {
            try
            {
                if (WorkflowService.getInstance().CheckApprovalParicipant(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, WorkGroupCode, InitiativeCode, hddStateGateAS.Value))
                {
                    WorkflowService.getInstance().updateStatusParticipantsV2(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    WorkGroupCode,
                    InitiativeCode,
                    hddStateGateAS.Value,
                    hddStateGateEndAS.Value);

                    WorkflowService.getInstance().updateStatusParticipantsDownGradeV2(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    WorkGroupCode,
                    InitiativeCode,
                    hddStateGateAS.Value,
                    hddStateGateEndAS.Value);

                    WorkflowService.getInstance().UpdateApprovalHeaderApproveStarted(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode, InitiativeCode,
                        hddStateGateAS.Value,
                        ERPWAuthentication.EmployeeCode,
                        Validation.getCurrentServerStringDateTime()
                        );

                    string curDateTime = Validation.getCurrentServerStringDateTimeMillisecond();
                    WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        "",
                        "",
                        ERPWAuthentication.EmployeeCode,
                        ERPWAuthentication.FullNameTH,
                        InitiativeCode,
                        hddStateGateAS.Value,
                        "APPROVAL_UPGARDE_STATEGATE",
                        "REQUEST",
                        txtRemarkInitiativeWeeklyStatusModelControlAS.Text,
                        curDateTime
                    );

                    sendEmail("REQUEST_APPROVAL_UPGARDE_STATEGATE");

                    ClientService.AGSuccess("ขออนุมัติสำเร็จ");
                    ControlState();

                    //ClientService.DoJavascript("SaveBeforeRequest();");
                    ClientService.DoJavascript("reloadInitiativeModalControlOnSave()");
                }
                else
                {
                    string curDateTimeMillisec = Validation.getCurrentServerStringDateTimeMillisecond();
                    string curDateTime = Validation.getCurrentServerStringDateTime();

                    WorkflowService.getInstance().UpdateApprovalHeaderApproveStarted(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode, InitiativeCode,
                        hddStateGateAS.Value,
                        ERPWAuthentication.EmployeeCode,
                        curDateTime
                        );


                    WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        "",
                        "",
                        ERPWAuthentication.EmployeeCode,
                        ERPWAuthentication.FullNameTH,
                        InitiativeCode,
                        hddStateGateAS.Value,
                        "APPROVAL_UPGARDE_STATEGATE",
                        "REQUEST",
                        txtRemarkInitiativeWeeklyStatusModelControlAS.Text,
                        curDateTimeMillisec
                    );

                    WorkflowService.getInstance().updateStatusHeader(
                        "",
                        ERPWAuthentication.EmployeeCode,
                        ERPWAuthentication.FullNameEN,
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode,
                        InitiativeCode,
                        hddStateGateAS.Value,
                        hddStateGateEndAS.Value,
                        curDateTime
                    );

                    WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        "",
                        "",
                        ERPWAuthentication.EmployeeCode,
                        ERPWAuthentication.FullNameTH,
                        InitiativeCode,
                        hddStateGateAS.Value,
                        "APPROVAL_UPGARDE_STATEGATE",
                        "APPROVE",
                        "Auto Approve by System",
                        curDateTimeMillisec
                    );

                    sendEmail("COMPLETE_UPGARDE_STATEGATE");

                    ControlState();

                    //ClientService.DoJavascript("SaveBeforeRequest();");
                    ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "')");
                }
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
        protected void btnSaveRequestCancelWeeklyStatusModelControl_Click(object sender, EventArgs e)
        {
            try
            {
                string AobjLink = InitiativeCode;

                if (WorkflowService.getInstance().checkApproveFinancialParticipant(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, WorkGroupCode, InitiativeCode, "Cancel"))
                {
                    WorkflowService.getInstance().UpdatedXstatusInitiative(
                        "",
                        WorkGroupCode,
                        InitiativeCode,
                        "WAITCANCEL",
                        ERPWAuthentication.EmployeeCode);

                    string curDateTime = Validation.getCurrentServerStringDateTimeMillisecond();
                    WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        "",
                        "",
                        ERPWAuthentication.EmployeeCode,
                        ERPWAuthentication.FullNameTH,
                        InitiativeCode,
                        hddStateGateAS.Value,
                        "APPROVAL_CANCEL_INITIATIVE",
                        "REQUEST",
                        txtRemarkInitiativeWeeklyStatusModelControlAS.Text, curDateTime
                    );
                    ControlState();

                    sendEmail("REQUEST_APPROVAL_CANCEL_INITIATIVE");
                    ClientService.AGSuccess("ขอยกเลิก Intitiative สำเร็จ");

                    //ClientService.DoJavascript("SaveBeforeRequest();");
                    ClientService.DoJavascript("reloadInitiativeModalControlOnSave()");
                }
                else
                {
                    WorkflowService.getInstance().UpdatedXstatusInitiative(
                        "",
                        WorkGroupCode,
                        InitiativeCode,
                        "WAITCANCEL",
                        ERPWAuthentication.EmployeeCode
                    );

                    string curDateTime = Validation.getCurrentServerStringDateTimeMillisecond();
                    WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        "",
                        "",
                        ERPWAuthentication.EmployeeCode,
                        ERPWAuthentication.FullNameTH,
                        InitiativeCode,
                        hddStateGateAS.Value,
                        "APPROVAL_CANCEL_INITIATIVE",
                        "REQUEST",
                        txtRemarkInitiativeWeeklyStatusModelControlAS.Text, 
                        curDateTime
                    );

                    WorkflowService.getInstance().SaveActivityXStatus( // Fuction Update Status Initiative
                        InitiativeCode,
                        "CANCELED",
                        "",
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode
                    );

                    WorkflowService.getInstance().UpdateInitiativeCanceledOn(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode,
                        InitiativeCode,
                        ERPWAuthentication.EmployeeCode
                    );

                    WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        "",
                        "",
                        ERPWAuthentication.EmployeeCode,
                        ERPWAuthentication.FullNameTH,
                        InitiativeCode,
                        hddStateGateAS.Value,
                        "APPROVAL_CANCEL_INITIATIVE",
                        "APPROVE",
                        "Auto Approved by System",
                        curDateTime
                    );

                    sendEmail("COMPLETE_CANCEL_INITIATIVE");

                    ControlState();

                    //ClientService.DoJavascript("SaveBeforeRequest();");
                    ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "');");
                }
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

        protected void btnApprovelRequestApprvelInitiative_Click(object sender, EventArgs e)
        {
            try
            {
                string curDateTimeMillisec = Validation.getCurrentServerStringDateTimeMillisecond();
                string curDateTime = Validation.getCurrentServerStringDateTime();

                WorkflowService.getInstance().updateStatusParticipants(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    WorkGroupCode,
                    InitiativeCode,
                    hddStateGateAS.Value,
                    hddStateGateEndAS.Value,
                    ERPWAuthentication.EmployeeCode);


                WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    "",
                    "",
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.FullNameTH,
                    InitiativeCode,
                    hddStateGateAS.Value,
                    "APPROVAL_UPGARDE_STATEGATE",
                    "APPROVE",
                    txtRemarkInitiativeWeeklyStatusModelControlAS.Text,
                    curDateTimeMillisec
                );

                //sendEmail("APPROVE_UPGARDE_STATEGATE");


                if (WorkflowService.getInstance().CheckApproveGroup(InitiativeCode, hddStateGateAS.Value, hddStateGateEndAS.Value))
                {
                    WorkflowService.getInstance().updateStatusHeader(
                        "",
                        ERPWAuthentication.EmployeeCode,
                        ERPWAuthentication.FullNameEN,
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode, InitiativeCode,
                        hddStateGateAS.Value,
                        hddStateGateEndAS.Value,
                        curDateTime
                    );

                    updateTicketStatusToTarget(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    TicketDocumentType,
                    TicketFiscalYear,
                    TicketDocumentNo,
                    ERPWAuthentication.EmployeeCode,
                    TicketStatusCodeOld
                    );

                    //sendEmail("COMPLETE_UPGARDE_STATEGATE");

                    ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "')");
                }


                string SuccessApprove = ServiceTicketLibrary.LookUpTable(
                "COMPLETED_ON",
                "ERPW_ACCOUNTABILITY_DOCUMENT_APPROVAL_HEADER",
                "WHERE SID='" + ERPWAuthentication.SID + "' AND COMPANYCODE='" + ERPWAuthentication.CompanyCode + "' AND INITIATIVECODE='" + InitiativeCode + "' ORDER BY COMPLETED_ON"
                );

                if (SuccessApprove != "")
                {
                    NotificationLibrary.GetInstance().TicketAlertEvent(
                            NotificationLibrary.EVENT_TYPE.TICKET_UPDATESTATUS,
                            ERPWAuthentication.SID,
                            ERPWAuthentication.CompanyCode,
                            TicketCode,
                            ERPWAuthentication.EmployeeCode,
                            ThisPage + "_SuccessApprove"
                        );
                    SuccessApprove = "SuccessApprove";
                }
                if (SuccessApprove != "SuccessApprove") {

                    NotificationLibrary.GetInstance().TicketAlertEvent(
                                NotificationLibrary.EVENT_TYPE.ChangeOrder_Approval,
                                ERPWAuthentication.SID,
                                ERPWAuthentication.CompanyCode,
                                TicketCode + "|" + hddStateGateEndAS.Value,
                                ERPWAuthentication.EmployeeCode,
                                ThisPage
                            );
                }

                ControlState();           

                ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "');");
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

        //update button confirm

        protected void btnApprovelRequestDocumentAS_Click(object sender, EventArgs e)
        {
            try
            {
                string curDateTime = Validation.getCurrentServerStringDateTimeMillisecond();
                WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    "",
                    "",
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.FullNameTH,
                    InitiativeCode,
                    hddStateGateAS.Value,
                    "REQUEST_DOC_APPROVAL",
                    "REQUEST_DOC",
                    txtRemarkInitiativeWeeklyStatusModelControlAS.Text,
                    curDateTime
                );
                ControlState();

                ServiceCallTransactionChange c = (ServiceCallTransactionChange)this.Parent.Page;
                c.MailAlertOwner();

                ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "');");
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
        //update button confirm

        protected void btnRejectRequestApprvelInitiative_Click(object sender, EventArgs e)
        {
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            try
            {
                string curDateTime = Validation.getCurrentServerStringDateTimeMillisecond();

                WorkflowService.getInstance().RejectApproveStartedHeader(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    WorkGroupCode,
                    InitiativeCode,
                    hddStateGateAS.Value,
                    hddStateGateEndAS.Value);

                WorkflowService.getInstance().RejectApproveStartedParticipants(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode,
                    WorkGroupCode,
                    InitiativeCode,
                    hddStateGateAS.Value,
                    hddStateGateEndAS.Value,
                    ERPWAuthentication.EmployeeCode);

                WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    "",
                    "",
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.FullNameTH,
                    InitiativeCode,
                    hddStateGateAS.Value,
                    "APPROVAL_UPGARDE_STATEGATE",
                    "REJECT",
                    txtRemarkInitiativeWeeklyStatusModelControlAS.Text,
                    curDateTime
                );

                // cancel
                //new ServiceCallTransactionChange().btnCancelDocTran_click(null,null);
                if (sPath == "/crm/AfterSale/ServiceCallTransactionChange.aspx")
                {
                    ServiceCallTransactionChange c = (ServiceCallTransactionChange)this.Parent.Page;
                    c.btnCancelDocTran_click(null, null);
                    c.MailAlertOwner();
                }
                else if(sPath == "/crm/AfterSale/ServiceCallTransaction.aspx") 
                {
                    ServiceCallTransaction c = (ServiceCallTransaction)this.Parent.Page;
                    c.btnCancelDocTran_click(null, null);
                    c.MailAlertOwner();
                }
                else
                { ClientService.AGError(ObjectUtil.Err(sPath)); }
                // cancel

                sendEmail("REJECT_UPGARDE_STATEGATE");

                ControlState();

                ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "');");
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

        protected void btnApprovelRequestCancelInitiative_Click(object sender, EventArgs e)
        {
            try
            {
                string CurDateTime = Validation.getCurrentServerStringDateTimeMillisecond();

                WorkflowService.getInstance().UpdateApproveFinancial(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, WorkGroupCode, InitiativeCode, "Cancel", ERPWAuthentication.EmployeeCode, "APPROVE", txtRemarkInitiativeWeeklyStatusModelControlAS.Text);



                WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    "",
                    "",
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.FullNameTH,
                    InitiativeCode,
                    hddStateGateAS.Value,
                    "APPROVAL_CANCEL_INITIATIVE",
                    "APPROVE",
                    txtRemarkInitiativeWeeklyStatusModelControlAS.Text,
                    CurDateTime
                );

                sendEmail("APPROVE_CANCEL_INITIATIVE");

                if (WorkflowService.getInstance().checkStatusGroupApproveFinancial(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, WorkGroupCode, InitiativeCode, "Cancel"))
                {
                    WorkflowService.getInstance().SaveActivityXStatus( // Fuction Update Status Initiative
                        InitiativeCode,
                        "CANCELED",
                        "",
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode
                    );

                    WorkflowService.getInstance().UpdateInitiativeCanceledOn(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode,
                        InitiativeCode,
                        ERPWAuthentication.EmployeeCode
                    );

                    sendEmail("COMPLETE_CANCEL_INITIATIVE");

                    ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "');");
                }

                ControlState();

                ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "');");

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
        protected void btnRejectRequestCancelInitiative_Click(object sender, EventArgs e)
        {
            try
            {
                string CurDateTime = Validation.getCurrentServerStringDateTimeMillisecond();

                WorkflowService.getInstance().UpdatedXstatusInitiative(
                            "",
                            WorkGroupCode,
                            InitiativeCode,
                            "",
                            ERPWAuthentication.EmployeeCode);

                WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    "",
                    "",
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.FullNameTH,
                    InitiativeCode,
                    hddStateGateAS.Value,
                    "APPROVAL_CANCEL_INITIATIVE",
                    "REJECT",
                    txtRemarkInitiativeWeeklyStatusModelControlAS.Text,
                    CurDateTime
                );

                sendEmail("REJECT_CANCEL_INITIATIVE");

                ControlState();

                ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "');");
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

        protected void btnRequestDowngradeInitiativeModelControl_Click(object sender, EventArgs e)
        {
            try
            {
                string curDateTime = Validation.getCurrentServerStringDateTimeMillisecond();

                int start = int.Parse(hddStateGateAS.Value.Substring(1, 1));
                int end = int.Parse(hddStateGateEndAS.Value.Substring(1, 1));

                string StartStateGate = hddStateGateAS.Value.Substring(0, 1) + (start - 1);
                string EndStateGate = hddStateGateEndAS.Value.Substring(0, 1) + (end - 1);

                if (WorkflowService.getInstance().CheckApprovalParicipant(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode, WorkGroupCode, InitiativeCode, hddDownGradedStateGeteFromAS.Value))
                {
                    WorkflowService.getInstance().updateStatusParticipantsV2(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode,
                        InitiativeCode,
                        StartStateGate,
                        EndStateGate);

                    WorkflowService.getInstance().updateStatusParticipantsDownGradeV2(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode,
                        InitiativeCode,
                        StartStateGate,
                        EndStateGate);

                    WorkflowService.getInstance().updateStatusDownGradeInitiative(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode,
                        InitiativeCode,
                        hddDownGradedStateGeteFromAS.Value,
                        hddDownGradedStateGeteToAS.Value,
                        ERPWAuthentication.EmployeeCode,
                        curDateTime);

                    WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        "",
                        "",
                        ERPWAuthentication.EmployeeCode,
                        ERPWAuthentication.FullNameTH,
                        InitiativeCode,
                        hddDownGradedStateGeteFromAS.Value,
                        "APPROVAL_DOWNGARDE_STATEGATE",
                        "REQUEST", txtRemarkInitiativeWeeklyStatusModelControlAS.Text,
                        curDateTime
                    );

                    sendEmail("REQUEST_APPROVAL_DOWNGARDE_STATEGATE");
                    ClientService.AGSuccess("ขออนุมัติสำเร็จ");
                    ControlState();

                    //ClientService.DoJavascript("SaveBeforeRequest();");
                    ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "');");
                }
                else
                {
                    WorkflowService.getInstance().updateStatusDownGradeInitiative(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode,
                        InitiativeCode,
                        hddDownGradedStateGeteFromAS.Value,
                        hddDownGradedStateGeteToAS.Value,
                        ERPWAuthentication.EmployeeCode,
                        curDateTime);

                    WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        "",
                        "",
                        ERPWAuthentication.EmployeeCode,
                        ERPWAuthentication.FullNameTH,
                        InitiativeCode,
                        hddDownGradedStateGeteFromAS.Value,
                        "APPROVAL_DOWNGARDE_STATEGATE",
                        "REQUEST",
                        txtRemarkInitiativeWeeklyStatusModelControlAS.Text,
                        curDateTime
                    );

                    WorkflowService.getInstance().ApprovalDowngradeStartedHeader(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode,
                        InitiativeCode,
                        hddDownGradedStateGeteFromAS.Value);

                    WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        "",
                        "",
                        ERPWAuthentication.EmployeeCode,
                        ERPWAuthentication.FullNameTH,
                        InitiativeCode,
                        hddDownGradedStateGeteFromAS.Value,
                        "APPROVAL_DOWNGARDE_STATEGATE",
                        "APPROVE", 
                        "Auto Approve by System", 
                        curDateTime
                    );

                    sendEmail("COMPLETE_DOWNGARDE_STATEGATE");

                    ControlState();

                    //ClientService.DoJavascript("SaveBeforeRequest();");
                    ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "');");
                }
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

        protected void btnApprovelRequestDowngradeInitiative_Click(object sender, EventArgs e)
        {
            try
            {
                string curDateTime = Validation.getCurrentServerStringDateTimeMillisecond();

                WorkflowService.getInstance().updateStatusParticipantsDownGrade(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    WorkGroupCode,
                    InitiativeCode,
                    hddDownGradedStateGeteFromAS.Value,
                    hddDownGradedStateGeteToAS.Value,
                    ERPWAuthentication.EmployeeCode);


                WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    "",
                    "",
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.FullNameTH,
                    InitiativeCode,
                    hddDownGradedStateGeteFromAS.Value,
                    "APPROVAL_DOWNGARDE_STATEGATE",
                    "APPROVE",
                    txtRemarkInitiativeWeeklyStatusModelControlAS.Text,
                    curDateTime
                );

                sendEmail("APPROVE_DOWNGARDE_STATEGATE");

                if (WorkflowService.getInstance().CheckCancelApproveGroup(InitiativeCode, hddDownGradedStateGeteFromAS.Value, hddDownGradedStateGeteToAS.Value))
                {
                    WorkflowService.getInstance().ApprovalDowngradeStartedHeader(
                        ERPWAuthentication.SID,
                        ERPWAuthentication.CompanyCode,
                        WorkGroupCode,
                        InitiativeCode,
                        hddDownGradedStateGeteFromAS.Value);

                    sendEmail("COMPLETE_DOWNGARDE_STATEGATE");

                    ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "');");
                }

                ClientService.AGSuccess("Save success");
                ControlState();

                ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "');");
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

        protected void btnRejectRequestDowngradeInitiative_Click(object sender, EventArgs e)
        {
            try
            {
                string curDateTime = Validation.getCurrentServerStringDateTimeMillisecond();

                int start = int.Parse(hddStateGateAS.Value.Substring(1, 1));
                int end = int.Parse(hddStateGateEndAS.Value.Substring(1, 1));

                string StartStateGate = hddStateGateAS.Value.Substring(0, 1) + (start - 1);
                string EndStateGate = hddStateGateEndAS.Value.Substring(0, 1) + (end - 1);

                WorkflowService.getInstance().RejectDowngradeStartedHeader(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    WorkGroupCode,
                    InitiativeCode,
                    hddDownGradedStateGeteFromAS.Value,
                    hddDownGradedStateGeteToAS.Value);

                WorkflowService.getInstance().RejectApproveDownGradeParticipants(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode,
                    WorkGroupCode,
                    InitiativeCode,
                    StartStateGate,
                    EndStateGate,
                    ERPWAuthentication.EmployeeCode);

                WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    "",
                    "",
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.FullNameTH,
                    InitiativeCode,
                    hddDownGradedStateGeteFromAS.Value,
                    "APPROVAL_DOWNGARDE_STATEGATE",
                    "REJECT",
                    txtRemarkInitiativeWeeklyStatusModelControlAS.Text,
                    curDateTime
                );

                sendEmail("REJECT_DOWNGARDE_STATEGATE");

                ClientService.AGSuccess("Save success");
                ControlState();

                ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "');");
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

        private void sendEmail(string EventMode)
        {
            //InitiativeEmailService.getInstance().SendInitiativeEMail(
            //    ERPWAuthentication.SID,
            //    "",
            //    ERPWAuthentication.CompanyCode,
            //    WorkGroupCode,
            //    InitiativeCode,
            //    EventMode,
            //    ERPWAuthentication.EmployeeCode
            //);
        }

        protected void btnSaveCancelRequestApprovalInitiativeModelControlAS_Click(object sender, EventArgs e)
        {
            try
            {
                string curDateTime = Validation.getCurrentServerStringDateTimeMillisecond();

                WorkflowService.getInstance().RejectApproveStartedHeader(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    WorkGroupCode,
                    InitiativeCode,
                    hddStateGateAS.Value,
                    hddStateGateEndAS.Value);

                WorkflowService.getInstance().RejectApproveStartedParticipants(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode,
                    WorkGroupCode,
                    InitiativeCode,
                    hddStateGateAS.Value,
                    hddStateGateEndAS.Value,
                    ERPWAuthentication.EmployeeCode);

                WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    "",
                    "",
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.FullNameTH,
                    InitiativeCode,
                    hddStateGateAS.Value,
                    "APPROVAL_UPGARDE_STATEGATE",
                    "CANCEL",
                    txtRemarkInitiativeWeeklyStatusModelControlAS.Text,
                    curDateTime
                );

                sendEmail("CANCEL_REQUEST_UPGARDE_STATEGATE");

                ControlState();
                ClientService.AGSuccess("Cancel success");
                ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "');");
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

        protected void btnSaveCancelRequestDowngradeInitiativeModelControlAS_Click(object sender, EventArgs e)
        {
            try
            {
                string curDateTime = Validation.getCurrentServerStringDateTimeMillisecond();

                int start = int.Parse(hddStateGateAS.Value.Substring(1, 1));
                int end = int.Parse(hddStateGateEndAS.Value.Substring(1, 1));

                string StartStateGate = hddStateGateAS.Value.Substring(0, 1) + (start - 1);
                string EndStateGate = hddStateGateEndAS.Value.Substring(0, 1) + (end - 1);

                WorkflowService.getInstance().RejectDowngradeStartedHeader(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    WorkGroupCode,
                    InitiativeCode,
                    hddDownGradedStateGeteFromAS.Value,
                    hddDownGradedStateGeteToAS.Value);

                WorkflowService.getInstance().RejectApproveDownGradeParticipants(ERPWAuthentication.SID, ERPWAuthentication.CompanyCode,
                    WorkGroupCode,
                    InitiativeCode,
                    StartStateGate,
                    EndStateGate,
                    ERPWAuthentication.EmployeeCode);

                WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    "",
                    "",
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.FullNameTH,
                    InitiativeCode,
                    hddDownGradedStateGeteFromAS.Value,
                    "APPROVAL_DOWNGARDE_STATEGATE",
                    "CANCEL",
                    txtRemarkInitiativeWeeklyStatusModelControlAS.Text,
                    curDateTime
                );

                sendEmail("CANCEL_REQUEST_DOWNGARDE_STATEGATE");

                ControlState();
                ClientService.AGSuccess("Cancel success");
                ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "');");
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

        protected void btnSaveCancelRequestCancelInitiativeModelControlAS_Click(object sender, EventArgs e)
        {
            try
            {
                string CurDateTime = Validation.getCurrentServerStringDateTimeMillisecond();

                WorkflowService.getInstance().UpdatedXstatusInitiative(
                    "",
                    WorkGroupCode,
                    InitiativeCode,
                    "",
                    ERPWAuthentication.EmployeeCode
                );
                WorkflowService.getInstance().UpdateApproveFinancial(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    WorkGroupCode,
                    InitiativeCode,
                    "Cancel",
                    ERPWAuthentication.EmployeeCode,
                    "CANCEL",
                    txtRemarkInitiativeWeeklyStatusModelControlAS.Text
                );


                WorkflowService.getInstance().SaveActivitySystemMessageForWorkflow(
                    ERPWAuthentication.SID,
                    ERPWAuthentication.CompanyCode,
                    "",
                    "",
                    ERPWAuthentication.EmployeeCode,
                    ERPWAuthentication.FullNameTH,
                    InitiativeCode,
                    hddStateGateAS.Value,
                    "APPROVAL_CANCEL_INITIATIVE",
                    "CANCEL",
                    txtRemarkInitiativeWeeklyStatusModelControlAS.Text,
                    CurDateTime
                );

                sendEmail("CANCEL_REQUEST_CANCEL_INITIATIVE");

                ControlState();
                ClientService.AGSuccess("Cancel success");
                ClientService.DoJavascript("reloadInitiativeModalControl('" + InitiativeCode + "', '" + WorkGroupCode + "');");
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
    }
}