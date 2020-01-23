<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/ServiceTicketMasterPage.master" ValidateRequest="false" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="ServiceCallTransactionChange.aspx.cs"
    Inherits="ServiceWeb.crm.AfterSale.ServiceCallTransactionChange" %>

<%@ Register Src="~/widget/usercontrol/AttachFileUserControl.ascx" TagName="AttachFileUserControl" TagPrefix="sna" %>
<%@ Register Src="~/Accountability/UserControl/ApprovalProcedureControl.ascx" TagPrefix="sna" TagName="ApprovalProcedureControl" %>
<%@ Register Src="~/Accountability/UserControl/ApproveStateGateControl.ascx" TagPrefix="sna" TagName="ApproveStateGateControl" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteCustomer.ascx" TagPrefix="ag" TagName="AutoCompleteCustomer" %>
<%@ Register Src="~/LinkFlowChart/FlowChartDiagramRelationControl.ascx" TagPrefix="ag" TagName="FlowChartDiagramRelationControl" %>
<%@ Register Src="~/UserControl/ChangeLogControl.ascx" TagPrefix="sna" TagName="ChangeLogControl" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEmployee.ascx" TagPrefix="ag" TagName="AutoCompleteEmployee" %>
<%@ Register Src="~/crm/usercontrol/modalAddNewContact.ascx" TagPrefix="ag" TagName="modalAddNewContact" %>
<%@ Register Src="~/widget/usercontrol/AutoCompleteControl.ascx" TagPrefix="ag" TagName="AutoCompleteControl" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEquipment.ascx" TagPrefix="ag" TagName="AutoCompleteEquipment" %>
<%@ Register Src="~/widget/usercontrol/SearchHelpCIControl.ascx" TagPrefix="ag" TagName="SearchHelpCIControl" %>
<%@ Register Src="~/widget/usercontrol/SearchCustomerControl.ascx" TagPrefix="ag" TagName="SearchCustomerControl" %>
<%@ Register Src="~/UserControl/ActivitySendMailModal.ascx" TagPrefix="ag" TagName="ActivitySendMailModal" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="<%= Page.ResolveUrl("~/AGFramework/chat/Activity-Chatting-1.7.css?vs=20181223.1") %>" rel="stylesheet" />
    <link href="<%= Page.ResolveUrl("~/crm/AfterSale/Lib/ServiceCall.css?vs=20190924") %>" rel="stylesheet" />
    <link href="<%= Page.ResolveUrl("~/js/FlipClock-master/compiled/flipclock.css") %>" rel="stylesheet" />
    <style>
        .ticket-allow-editor,
        .ticket-allow-editor-everyone {
        }

        .your-clock {
            zoom: 0.4;
            -moz-transform: scale(0.4);
        }

        .flip-clock-wrapper {
            margin: 0 !important;
        }

            .flip-clock-wrapper ul {
                height: 80px;
            }

        .flip-clock-divider {
            height: 90px;
        }

        .flip-clock-dot.bottom {
            bottom: 25px;
        }

        .flip-clock-label {
            display: none !important;
        }

        .headerdocred {
            color: red;
        }

        .headerdocgreen {
            color: green;
        }

        #icon-lock-ticket {
            padding: 0.5px 7px !important;
        }
    </style>
    <script>
        var TicketType = "<%= (string)Session["SCT_created_doctype_code"] %>";
        var CustomerCode = "<%= (string)Session["SCT_created_cust_code"] %>";
        var AreaCode = "<%= AreaCode %>";
        var FocusOneLinkProfileImage = "<%= ServiceWeb.Service.PublicAuthentication.FocusOneLinkProfileImage %>";
        var EMP_FullNameEN = "<%= ERPW.Lib.Authentication.ERPWAuthentication.FullNameEN %>";
        var isPageChange = true;
        var appCreateMode = '<%= mode_stage == agape.lib.constant.ApplicationSession.CREATE_MODE_STRING %>'.toLowerCase() == 'true';



        function confirmSaveAddKnowledgeMapping(obj) {
            if (AGConfirm(obj, "Confirm Save Knowledge.")) {
                AGLoading(true);
                return true;
            }
            return false;
        }

        function setModesaveClick(obj) {
            if ($(".item-card-ci-select").length == 0) {
                AGError("Please select Configuration Item.");
                $("#nav-item-tab").click();
                return;
            }
            if ($(".item-card-customer-select").length == 0) {
                AGError("Please select Affected Customer.");
                $("#nav-effectcus-tab").click();
                return;

            }

            if (appCreateMode) {
                if (prepareDateForSaveEmpParticipant_Change("OwnerParticipant")) {
                    saveClick(obj, '<%= isOBJECT_CHANGE %>');
                }
            } else {
                saveClick(obj, '<%= isOBJECT_CHANGE %>');
            }
        }


        var LINK_ANALYTICS = "LINK";
        var LINK_ANALYTICS_PATH = servictWebDomainName + "Analytics/";
        var ERPW_PROGRAM_ID = "<%= ERPW.Lib.Master.LogServiceLibrary.PROGRAM_ID_SERVICE_CALL %>";
        var ERPW_REFERENCE_ID = "<%= hddDocnumberTran.Value %>";
        var ERPW_REFERENCE_PAGE_MODE = "<%= mode_stage %>";

        (function (i, s, o, g, r, a, m) {
            i['SNAAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
                m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', LINK_ANALYTICS_PATH + 'Analytics.js', 'sna');

    </script>
    <div id="panel-ticket-detail">
        <div class="pull-right" style="margin-top: 10px; margin-left: 7px;">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button Text="" runat="server" CssClass="d-none" OnClientClick="return false;" />
                    <%--อย่าเอาออก ตั้งใว้ดักตอนกด Enter ใน Textbox ไม่อย่างนั้นมันจะไปเข้าปุ่ม Save--%>

                    <button type="button" runat="server" class="btn btn-danger btn-sm mb-1 ticket-allow-editor ticket-allow-editor-everyone" onclick="$(this).next().click();">
                        <i class="fa fa-close"></i>&nbsp;&nbsp;Exit
                    </button>
                    <asp:Button ID="btnClosePage" OnClick="btnClosePage_Click" ClientIDMode="Static"
                        Text="" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" runat="server" />
                    <%--OnClientClick="return AGConfirmClosePage(this);"--%>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <nav class="navbar nav-header-action sticky-top bg-white" id="nav-ticket-button-control"
            style="margin-left: 1px; margin-right: 1px; top: 0px;">
            <div class="pull-left">
                <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <a class="d-none btn btn-warning btn-sm mb-1" href="ServiceCallChangeCriteria.aspx"><i class="fa fa-arrow-circle-left"></i>&nbsp;&nbsp;Back</a>
                        <% if (mode_stage == agape.lib.constant.ApplicationSession.CREATE_MODE_STRING)
                            { %>
                        <button type="button" class="btn btn-warning btn-sm mb-1" onclick="$(this).next().click();">
                            <i class="fa fa-refresh"></i>
                            &nbsp;&nbsp;Clear
                        </button>
                        <asp:Button Text="" runat="server" CssClass="d-none" ID="btnResetToDefault"
                            OnClick="btnResetToDefault_Click" OnClientClick="return confirmResetValueDefault(this);" />
                        <% } %>

                        <% if ((mode_stage == agape.lib.constant.ApplicationSession.CHANGE_MODE_STRING ||
                                 mode_stage == agape.lib.constant.ApplicationSession.DISPLAY_MODE_STRING) &&
                                 !IsDocTicketStatusClose && !IsDocTicketStatusCancel && !IsDocTicketStatusRollBack)
                            { %>
                        <button type="button" class="btn btn-info btn-sm mb-1 ticket-allow-editor ticket-allow-editor-everyone" onclick="$(this).next().click();">
                            <i class="fa fa-refresh"></i>
                            &nbsp;&nbsp;Refresh
                        </button>
                        <asp:Button Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone"
                            ID="brnRefreshData" ClientIDMode="Static" OnClientClick="AGLoading(true);" OnClick="brnRefreshData_Click" />
                        <% } %>
                        <% if (mode_stage == agape.lib.constant.ApplicationSession.CHANGE_MODE_STRING &&
                                 !IsDocTicketStatusClose && !IsDocTicketStatusCancel && !IsDocTicketStatusRollBack)
                            { %>
                        <button type="button" class="btn btn-primary btn-sm mb-1 AUTH_MODIFY" onclick="openModalCreateNewTicket();">
                            <i class="fa fa-file-o"></i>
                            &nbsp;&nbsp;Create Reference Ticket
                        </button>
                        <% } %>

                        <% if (mode_stage != agape.lib.constant.ApplicationSession.DISPLAY_MODE_STRING)
                            { %>

                        <button type="button" id="btnConfirmClientTran" runat="server" class="btn btn-success btn-sm mb-1 AUTH_MODIFY" onclick="setModesaveClick(this);">
                            <i class="fa fa-save"></i>&nbsp;&nbsp;Save
                        </button>
                        <asp:Button ID="btnConfirmTran" runat="server" CssClass="d-none" OnClick="btnConfirmTran_click" />
                        <% } %>
                        <% if (mode_stage == agape.lib.constant.ApplicationSession.CHANGE_MODE_STRING && !IsDocTicketStatusClose && !IsDocTicketStatusCancel && !IsDocTicketStatusRollBack)
                            { %>
                        <button type="button" class="btn btn-info btn-sm mb-1 d-none AUTH_MODIFY" onclick="$('#Modal-UpdateStatus').modal('show');">
                            <i class="fa fa-tasks"></i>&nbsp;&nbsp;Update Status
                        </button>
                        <button type="button" class="btn btn-success btn-sm mb-1 AUTH_MODIFY" onclick="$('#Modal-CloseTicket').modal('show');">
                            <i class="fa fa-check-square-o"></i>&nbsp;&nbsp;Close Ticket
                        </button>
                        <button type="button" class="btn btn-danger btn-sm mb-1 AUTH_MODIFY" onclick="$(this).next().click();">
                            <i class="fa fa-ban"></i>&nbsp;&nbsp;Cancel
                        </button>
                        <asp:Button Text="Cancel" runat="server" CssClass="d-none AUTH_MODIFY" ID="btn_CanceldocTran_v2"
                            OnClientClick="return confirmCancelTicket(this);" OnClick="btnCancelDocTran_click" />
                        <% } %>
                        <%--<asp:Button ID="btn_CanceldocTran" runat="server" CssClass="btn btn-danger btn-sm mb-1" 
                        Visible="false" OnClientClick="AGLoading(true);" OnClick="btnCancelDocTran_click" />--%>
                        <% if (ShowWorkFlowWithoutCreate)
                            { %>
                        <button type="button" class="btn btn-info btn-sm mb-1 ticket-allow-editor-everyone" onclick="showInitiativeModal('modal-workflow');"><i class="fa fa-sitemap"></i>&nbsp;&nbsp;Work Flow</button>
                        <% } %>
                        <% if (mode_stage == agape.lib.constant.ApplicationSession.CHANGE_MODE_STRING &&
                                 !IsDocTicketStatusClose && !IsDocTicketStatusCancel && !IsDocTicketStatusRollBack)
                            { %>
                        <button type="button" class="btn btn-danger btn-sm mb-1 AUTH_MODIFY" onclick="confirmResetTicket(this)">
                            <i class="fa fa-refresh"></i>&nbsp;&nbsp; Reset WorkFlow 
                        </button>
                        <asp:Button Text="" runat="server" CssClass="d-none" ID="btnResetTicket" CommandArgument="Reset" ClientIDMode="Static" OnClientClick="" OnClick="btnResetTicket_Click" />
                        <% } %>
                        <% if (mode_stage == agape.lib.constant.ApplicationSession.CHANGE_MODE_STRING && (IsDocTicketStatusClose || IsDocTicketStatusCancel || IsDocTicketStatusRollBack))
                            { %>
                        <button type="button" id="Button1" runat="server" class="btn btn-danger btn-sm mb-1" onclick="$(this).next().click();">
                            <i class="fa fa-refresh"></i>&nbsp;&nbsp;Re-Open Ticket
                        </button>
                        <asp:Button Text="Re-Open Ticket" runat="server" ID="btnReOpenTicket" CommandArgument="Reopen" CssClass="btn btn-warning btn-sm mb-1 d-none"
                            OnClick="btnReOpenTicket_Click" OnClientClick="return confirmReOpenTicket(this);" />
                        <% } %>
                        <% if (mode_stage == agape.lib.constant.ApplicationSession.CHANGE_MODE_STRING && !IsDocTicketStatusCancel && !IsDocTicketStatusRollBack)
                            { %>
                        <button type="button" id="btnAddKnowledge" class="btn btn-primary btn-sm mb-1 AUTH_MODIFY" onclick="$('#<%= btnOpenModalAddKM.ClientID  %>').click();">
                            <i class="fa fa-plus-circle"></i>&nbsp;&nbsp;Add KM
                        </button>
                        <% } %>

                        <% if (mode_stage == agape.lib.constant.ApplicationSession.CHANGE_MODE_STRING && IsDocTicketStatusClose)
                            { %>
                        <button type="button" id="btnOpenCreateKM" runat="server" class="btn btn-success btn-sm mb-1" onclick="$(this).next().click();">
                            <i class="fa fa-file-o"></i>&nbsp;&nbsp;Create KM
                        </button>
                        <asp:Button ID="btnOpenCreateKMRefTicket" OnClick="btnOpenCreateKMRefTicket_Click" OnClientClick="AGLoading(true);" Text="Create KM ref Ticket" CssClass="d-none" runat="server" />
                        <% } %>

                        <% if (mode_stage == agape.lib.constant.ApplicationSession.DISPLAY_MODE_STRING)
                            { %>
                        <button type="button" runat="server" class="btn btn-info btn-sm mb-1 ticket-allow-editor ticket-allow-editor-everyone" onclick="$(this).next().click();">
                            <i class="fa fa-pencil-square-o"></i>&nbsp;&nbsp;Enable Edit
                        </button>
                        <asp:Button ID="btnRequestChange" OnClick="btnRequestChange_Click" OnClientClick="AGLoading(true);"
                            Text="" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" runat="server" />
                        <% } %>
                        <% if (mode_stage == agape.lib.constant.ApplicationSession.CHANGE_MODE_STRING)
                            { %>
                        <button type="button" runat="server" class="btn btn-info btn-sm mb-1 ticket-allow-editor ticket-allow-editor-everyone" onclick="$(this).next().click();">
                            <i class="fa fa-eye"></i>&nbsp;&nbsp;Disable Edit
                        </button>
                        <asp:Button ID="btnRequestDisplayMode" ClientIDMode="Static" OnClick="btnRequestDisplayMode_Click" OnClientClick="AGLoading(true);"
                            Text="" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" runat="server" />


                        <% } %>

                        <%if (mode_stage == agape.lib.constant.ApplicationSession.CHANGE_MODE_STRING && !IsDocTicketStatusClose && !IsDocTicketStatusCancel && !IsDocTicketStatusRollBack)
                            {%>

                        <button type="button" class="btn btn-danger btn-sm mb-1 AUTH_MODIFY" onclick="$(this).next().click();">
                            <i class="fa fa-ban"></i>&nbsp;&nbsp;Roll Back
                        </button>
                        <asp:Button Text="Rollback" runat="server" CssClass="d-none AUTH_MODIFY" ID="btnRollBack"
                            OnClientClick="return confirmRollBackTicket(this);" OnClick="btnRollBackTicket_Click" />
                        <%} %>

                        <asp:Button ID="btnSelectIncidentArea" runat="server" CssClass="d-none" ClientIDMode="Static"
                            OnClientClick="AGLoading(true);" OnClick="btnSelectIncidentArea_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="pull-right">
                <div class="your-clock d-none"></div>
            </div>
        </nav>

        <div class="d-none">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpHiddenCode">
                <ContentTemplate>
                    <asp:HiddenField ID="hdfCustomerCode" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdfAreaCode" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hddDocnumberTran" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hddTicketDocType" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hddTicketStatus" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hddCheckSaveChangeTicketStatus" runat="server" Value="false" ClientIDMode="Static" />
                    <asp:HiddenField ID="hddTicketStatus_Old" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hddTicketStatus_New" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hddCustomerCode" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hddDoctype_OpenRelation" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hddFiscalYear_OpenRelation" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hddTicketNo_OpenRelation" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hddAuthenEdit" runat="server" ClientIDMode="Static" Value="false" />
                    <asp:HiddenField ID="hddBusinessObject" runat="server" />
                    <asp:HiddenField ID="hddEmployeeFirstView" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hddPageTicketMode" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hddAnalytics_Row_Key" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hddIDCurentTabView" runat="server" ClientIDMode="Static" />

                    <asp:DropDownList runat="server" CssClass="ticket-allow-editor" ID="ddlTicketStatus_Temp" ClientIDMode="Static">
                    </asp:DropDownList>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone"
                        ID="btnUpdateStatus_FormPostRemark" ClientIDMode="Static"
                        OnClick="btnUpdateStatus_FormPostRemark_Click" OnClientClick="AGLoading(true);" />
                    <asp:Button Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone"
                        ID="btnUpdateLog_FormPostRemark" ClientIDMode="Static"
                        OnClick="btnUpdateLog_FormPostRemark_Click" OnClientClick="AGLoading(true);" />
                    <asp:Button Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone"
                        ID="btnOpenTicketRelation" ClientIDMode="Static"
                        OnClick="btnOpenTicketRelation_Click" OnClientClick="AGLoading(true);" />
                    <asp:Button Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone"
                        ID="btnAddNewEquipment" ClientIDMode="Static"
                        OnClick="btnAddNewEquipment_Click" OnClientClick="AGLoading(true);" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="row">
            <div class="col">
                <div class="card shadow">
                    <div class="card-header">
                        <span class="h5 mb-0" id="lal_headerdoc"><asp:Literal id="ltrTicketType" Text="Ticket Service" runat="server" /> Change Order</span>
                        <asp:Label Text="" runat="server" ID="lbl_docnumberTran" CssClass="h5 mb-0" />
                        <asp:CheckBox ID="chkMajorIncident" runat="server" Text="Major Incident" CssClass="form-check" Style="float: right;" />
                    </div>
                    <div class="card-body">
                        <nav>
                            <div class="nav nav-tabs" id="nav-tab" role="tablist">
                                <a class="header-tab nav-item nav-link active" id="nav-header-tab" data-toggle="tab" href="#nav-header" role="tab" aria-controls="nav-header" aria-selected="true">Overview</a>
                                <a class="header-tab nav-item nav-link" id="nav-participants-tab" data-toggle="tab" href="#nav-participants" role="tab" aria-controls="nav-participants" aria-selected="true">Owner Service</a>
                                <a class="header-tab nav-item nav-link" id="nav-item-tab" data-toggle="tab" href="#nav-item" role="tab" aria-controls="nav-item" aria-selected="false">Configuration Item
                                <asp:UpdatePanel ID="updcountitem" runat="server" UpdateMode="Conditional" class="d-none">
                                    <ContentTemplate>
                                        <label id="lb_menu_count_itemTran" runat="server" class="badge badge-primary"></label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                </a>
                                <a class="header-tab nav-item nav-link" id="nav-effectcus-tab" data-toggle="tab" href="#nav-effectcus" role="tab" aria-controls="nav-effectcus" aria-selected="true">Affected Client</a>

                                <a id="lichatTran" runat="server" class="header-tab nav-item nav-link d-none" data-toggle="tab" href="#nav-chat" role="tab" aria-controls="nav-chat" aria-selected="false">Chat</a>
                                <a id="liDate-Time-logTran" class="header-tab nav-item nav-link <%= mode_stage == agape.lib.constant.ApplicationSession.CREATE_MODE_STRING ? " d-none " : "" %>"
                                    data-toggle="tab" href="#nav-Date-Time-log" role="tab" aria-controls="nav-Date-Time-log" aria-selected="true"
                                    onclick="$('#btnIsLoad_DateTimeLog').click();">Date-Time Information</a>
                                <a id="liKnowledgeTran" class="header-tab nav-item nav-link <%= mode_stage == agape.lib.constant.ApplicationSession.CREATE_MODE_STRING ? " d-none " : "" %>"
                                    data-toggle="tab" href="#nav-knowledge" role="tab" aria-controls="nav-knowledge" aria-selected="true"
                                    onclick="$('#btnIsLoad_Knowledge').click();">Knowledge Management</a>
                                <a id="liattachTran" class="header-tab nav-item nav-link <%= mode_stage == agape.lib.constant.ApplicationSession.CREATE_MODE_STRING ? " d-none " : "" %>"
                                    data-toggle="tab" href="#nav-attachment" role="tab" aria-controls="nav-attachment" aria-selected="false"
                                    onclick="$('#btnIsLoad_Attachment').click();">Attachment
                                <asp:UpdatePanel ID="updcountfile" runat="server" UpdateMode="Conditional" class="d-none">
                                    <ContentTemplate>
                                        <label id="lb_menu_count_fileTran" runat="server" class="badge badge-primary"></label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                </a>
                                <a id="lichangelogTran" class="header-tab nav-item nav-link <%= mode_stage == agape.lib.constant.ApplicationSession.CREATE_MODE_STRING ? " d-none " : "" %>"
                                    data-toggle="tab" href="#nav-changelog" role="tab" aria-controls="nav-changelog" aria-selected="true"
                                    onclick="$('#btnIsLoad_TicketChangeLog').click();">Change Log</a>
                            </div>
                        </nav>
                        <br />
                        <div class="tab-content p-0" id="nav-tabContent" style="max-height: calc(100vh - 220px); overflow: hidden auto;">
                            <div class="tab-pane fade show active" id="nav-header" role="tabpanel" aria-labelledby="nav-header-tab">
                                <asp:UpdatePanel ID="panelUpdate" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="form-row">
                                            <div class="form-group col-lg-9 col-md-12 col-sm-12">
                                                <div class="card">
                                                    <div class="card-body card-body-sm">
                                                        <div class="form-row">
                                                            <div class="form-group col-md-8 col-sm-12 m-0">
                                                                <div class="form-row">
                                                                    <div class="form-group col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                                                        <asp:Label runat="server" ID="labelDocumentType" Text="Ticket Type"></asp:Label>
                                                                        <input id="_txt_doctypeTran" type="text" class="form-control form-control-sm" runat="server" disabled="disabled" />
                                                                    </div>
                                                                    <div class="form-group col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                                                        <asp:Label runat="server" ID="labelDocumentNumber" Text="Ticket No."></asp:Label>
                                                                        <input id="_txt_docnumberTran" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static"
                                                                            disabled="disabled" />
                                                                    </div>
                                                                    <div class="form-group col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                                                        <asp:Label Text="Ticket Status" runat="server" ID="_lbl_TicketStatusTran" />
                                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpTicketStatusTran">
                                                                            <ContentTemplate>
                                                                                <input id="_txt_TicketStatusTran" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static"
                                                                                    disabled="disabled" />
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                    <div class="form-group col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                                                        <asp:Label runat="server" ID="labelDocumentDate" Text="Ticket Date"></asp:Label>
                                                                        <input id="_txt_docdateTran" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static"
                                                                            disabled="disabled" />
                                                                    </div>
                                                                    <div class="form-group col-lg-8 col-md-12 col-sm-12 col-xs-12">
                                                                        <asp:UpdatePanel ID="udpRefer" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <label>Reference External Ticket</label>
                                                                                <asp:TextBox ID="tbRefer" runat="server" CssClass="form-control form-control-sm ticket-allow-editor"></asp:TextBox>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                    <div class="form-group col-lg-4 col-md-8 col-sm-4 col-xs-12">
                                                                        <asp:Label ID="labelCreatedBy" runat="server" Text="Created By"></asp:Label>
                                                                        <asp:TextBox ID="tbCreatedBy" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                                                        <label>Created On</label>
                                                                        <asp:TextBox ID="tbCreatedOn" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                                                        <asp:CheckBox ID="CABCheck" runat="server" Text="CAB" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-group col-md-4 col-sm-12 m-0">
                                                                <div class="form-row">
                                                                    <div class="form-group col-lg-7 col-md-12 col-sm-8 col-xs-12">
                                                                        <label>Plan Start Date</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tbStartDate" runat="server" CssClass="form-control form-control-sm date-picker ticket-allow-editor required"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group col-lg-5 col-md-12 col-sm-4 col-xs-12">
                                                                        <label>Time</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-control form-control-sm time-picker ticket-allow-editor required"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group col-lg-7 col-md-12 col-sm-8 col-xs-12">
                                                                        <label>Plan End Date</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tbEndDate" runat="server" CssClass="form-control form-control-sm date-picker ticket-allow-editor required" onchange="CheckCalendar()"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group col-lg-5 col-md-12 col-sm-4 col-xs-12">
                                                                        <label>Time</label>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-control form-control-sm time-picker ticket-allow-editor required"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group col-lg-3 col-md-12 col-sm-12">
                                                <div class="card">
                                                    <div class="card-body card-body-sm">
                                                        <div class="form-row">
                                                            <div class="form-group col-lg-12 col-md-4 col-sm-6 col-xs-12">
                                                                <asp:Label ID="lbWorkFlowStats" runat="server" Text="Work Flow Status"></asp:Label>
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpWorkFlowStatus">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="tbWorkFlowStatus" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                            <div class="form-group col-lg-12 col-md-4 col-sm-6 col-xs-12">
                                                                <asp:Label ID="lbApprovalStatus" runat="server" Text="Wait Approval By"></asp:Label>
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpApprovalStatus">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="tbApprovalStatus" runat="server" CssClass="form-control form-control-sm" Enabled="false" Text="-"></asp:TextBox>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                            <div class="form-group col-lg-12 col-md-4 col-sm-12 col-xs-12">
                                                                <asp:Label ID="lbAccountability" runat="server" Text="Accountability" />
                                                                <asp:Panel runat="server" ID="panelAccountability">
                                                                    <asp:DropDownList runat="server" ID="ddlAccountability" CssClass="form-control form-control-sm required">
                                                                    </asp:DropDownList>
                                                                </asp:Panel>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group col-md-12 col-sm-12 d-none">
                                                <div class="card">
                                                    <div class="card-body card-body-sm">
                                                        <asp:UpdatePanel ID="udpnProblem" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="form-row">
                                                                    <div class="form-group col-lg-3 col-md-6 col-sm-6 col-xs-12">
                                                                        <label>
                                                                            <asp:Label Text="Problem Group" runat="server" ID="lblEquipmentProblemGroupDesc" /></label>
                                                                        <asp:DropDownList ID="ddlProblemGroup" runat="server" CssClass="form-control form-control-sm" onchange="$(this).next().click();"></asp:DropDownList>
                                                                        <asp:Button ID="btnChangeProblemGroup" runat="server" CssClass="d-none"
                                                                            OnClientClick="AGLoading(true);" OnClick="btnChangeProblemGroup_Click" />
                                                                    </div>
                                                                    <div class="form-group col-lg-3 col-md-6 col-sm-6 col-xs-12">
                                                                        <label>
                                                                            <asp:Label Text="Problem Type" runat="server" ID="lblEquipmentProblemTypeDesc" /></label>
                                                                        <asp:DropDownList ID="ddlProblemType" onchange="$(this).next().click();" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                                                                        <asp:Button ID="btnChangeProblemType" runat="server" CssClass="d-none"
                                                                            OnClientClick="AGLoading(true);" OnClick="btnChangeProblemType_Click" />
                                                                    </div>
                                                                    <div class="form-group col-lg-3 col-md-6 col-sm-6 col-xs-12">
                                                                        <label>
                                                                            <asp:Label Text="Problem Source" runat="server" ID="lblEquipmentProblemSourceDesc" /></label>
                                                                        <asp:DropDownList ID="ddlProblemSource" onchange="$(this).next().click();" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                                                                        <asp:Button ID="btnChangeProblemSource" runat="server" CssClass="d-none"
                                                                            OnClientClick="AGLoading(true);" OnClick="btnChangeProblemSource_Click" />
                                                                    </div>
                                                                    <div class="form-group col-lg-3 col-md-6 col-sm-6 col-xs-12">
                                                                        <label>Contact Source</label>
                                                                        <asp:DropDownList ID="ddlServiceSource" onchange="$(this).next().click();" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                                                                        <asp:Button ID="btnChangeServiceSource" runat="server" CssClass="d-none"
                                                                            OnClientClick="AGLoading(true);" OnClick="btnChangeServiceSource_Click" />
                                                                    </div>

                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group col-md-12 col-sm-12">
                                                <div class="card">
                                                    <div class="card-body card-body-sm">
                                                        <asp:UpdatePanel ID="udpnSeverity" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="form-row">
                                                                    <div class="form-group col-lg-4 col-md-6 col-sm-4 col-xs-12">
                                                                        <label>Reference Doc SO</label>
                                                                        <asp:TextBox ID="txtRef_SO" runat="server" CssClass="form-control form-control-sm ticket-allow-editor"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group col-lg-4 col-md-6 col-sm-4 col-xs-12">
                                                                        <label>Reference Doc QT</label>
                                                                        <asp:TextBox ID="txtRef_QT" runat="server" CssClass="form-control form-control-sm ticket-allow-editor"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group col-lg-4 col-md-6 col-sm-4 col-xs-12">
                                                                        <label>Reference Doc PO</label>
                                                                        <asp:TextBox ID="txtRef_PO" runat="server" CssClass="form-control form-control-sm ticket-allow-editor"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group col-sm-4">
                                                                        <asp:Label ID="lebelImpact" runat="server" Text="Impact"></asp:Label>
                                                                        <asp:DropDownList ID="ddlImpact" runat="server" class="form-control form-control-sm required" AutoPostBack="true"
                                                                            OnSelectedIndexChanged="ddlImpact_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="form-group col-sm-4">                                                                        
                                                                        <asp:Label ID="labelUrgency" runat="server" Text="Urgency"></asp:Label>
                                                                        <asp:DropDownList ID="ddlUrgency" runat="server" class="form-control form-control-sm required" AutoPostBack="true"
                                                                            OnSelectedIndexChanged="ddlUrgency_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="form-group col-sm-4">
                                                                        <asp:Label runat="server" ID="labelPriority" Text="Priority"></asp:Label>
                                                                        <asp:DropDownList ID="_ddl_priorityTran" runat="server" class="form-control form-control-sm required"
                                                                            DataTextField="Description" DataValueField="PriorityCode">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <asp:UpdatePanel ID="udpRemark" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="form-group control-max-length">
                                                                    <asp:Label runat="server" ID="labelProblem" Text="Subject"></asp:Label>
                                                                    <small style="color: #aaa;">&nbsp;(<span class="Count-MaxLength-Remark"><%= txt_problem_topic.Text.Length %></span>/1000)</small>
                                                                    <asp:TextBox ID="txt_problem_topic" runat="server" CssClass="form-control form-control-sm required required ticket-allow-editor"
                                                                        onkeypress="return countMaxLengthRemark(event)" onkeyup="validateMaxLengthRemark(event);"
                                                                        Rows="3" Style="resize: none;" data-maxlength="1000"></asp:TextBox>
                                                                    <asp:HiddenField runat="server" ID="hddOldValue_problem_topic" />
                                                                </div>

                                                                <div class="form-group control-max-length">
                                                                    <asp:Label runat="server" ID="labelDescription" Text="Description"></asp:Label>
                                                                    <small style="color: #aaa;">&nbsp;(<span class="Count-MaxLength-Remark"><%= tbEquipmentRemark.Text.Length %></span>/8000)</small>
                                                                    <asp:TextBox ID="tbEquipmentRemark" runat="server" CssClass="form-control form-control-sm ticket-allow-editor"
                                                                        onkeypress="return countMaxLengthRemark(event)" onkeyup="validateMaxLengthRemark(event);"
                                                                        TextMode="MultiLine" size="15" Rows="3" Style="resize: none; height: 250px;" data-maxlength="8000"></asp:TextBox>
                                                                    <asp:HiddenField runat="server" ID="hddOldValue_EquipmentRemark" />
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="d-none">
                                            <label for="input" class="col-lg-2 col-md-2 col-sm-2" style="padding-bottom: 10px;">
                                                <asp:Label runat="server" ID="labelCompany" Text="บริษัท"></asp:Label>
                                            </label>
                                            <div class="col-lg-4 col-md-4 col-sm-4" style="padding-bottom: 10px;">
                                                <input id="_txt_companyTran" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static" disabled="disabled" />
                                            </div>

                                            <asp:Label runat="server" ID="labelDocumentStatus" Text="Ticket Status"></asp:Label>
                                            <input id="_txt_docstatusTran" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static"
                                                disabled="disabled" />

                                            <asp:Label runat="server" ID="labelFiscalYear" Text="Fiscal Year"></asp:Label>
                                            <input id="_txt_fiscalyear" type="text" class="form-control form-control-sm" runat="server" disabled="disabled" />
                                        </div>

                                        <script>
                                            var count = 0;
                                            function CheckCalendar() {
                                                if (count === 2) {
                                                    if (String(document.getElementById('<%= tbStartDate.ClientID %>').value).length === 0) {
                                                        AGMessage("กรุณากรอกข้อมูล Start date ให้ครบ");
                                                    } else {
                                                        if (String(document.getElementById('<%= tbEndDate.ClientID %>').value).length !== 10) {
                                                            AGMessage("กรุณากรอกข้อมูล End date ให้ครบ");
                                                        } else {
                                                            <%--var dateStart = new Date(String(document.getElementById('<%= tbStartDate.ClientID %>').value));
                                                            var dateEnd = new Date(String(document.getElementById('<%= tbEndDate.ClientID %>').value));
                                                            alert(String(document.getElementById('<%= tbEndDate.ClientID %>').value))
                                                            if (dateEnd < dateStart) {
                                                                AGMessage("ข้อมูล End Date ผิดพลาด");
                                                                document.getElementById('<%= tbEndDate.ClientID %>').value = "";
                                                            }--%>
                                                            //new solution 
                                                            var strstartdate = String(document.getElementById('<%= tbStartDate.ClientID %>').value).split("/");
                                                            var strendate = String(document.getElementById('<%= tbEndDate.ClientID %>').value).split("/");
                                                            if (parseInt(strendate[2]) > parseInt(strstartdate[2])) { } else {
                                                                if (parseInt(strendate[1]) > parseInt(strstartdate[1])) { } else {
                                                                    if (parseInt(strendate[0]) >= parseInt(strstartdate[0])) { } else {
                                                                        AGMessage("ข้อมูล End Date ผิดพลาด");
                                                                        document.getElementById('<%= tbEndDate.ClientID %>').value = "";
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    count = 0;
                                                } else {
                                                    count++;
                                                }
                                            }
                                        </script>

                                        <div>
                                            <div class="row d-none">
                                                <div class="col-md-12">
                                                    <div class="row d-none">
                                                        <label for="input" class="col-lg-2 col-md-2 col-sm-2" style="padding-bottom: 10px;">
                                                            <asp:Label runat="server" ID="labelContactPhoneNo" Text="หมายเลขโทรศัพท์"></asp:Label>
                                                        </label>
                                                        <div class="col-lg-4 col-md-4 col-sm-4" style="padding-bottom: 10px;">
                                                            <asp:DropDownList ID="_ddl_contact_phone_noTran" runat="server" class="form-control form-control-sm"
                                                                ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="row d-none">
                                                        <label for="input" class="col-lg-2 col-md-2 col-sm-2" style="padding-bottom: 10px;">
                                                            <asp:Label runat="server" ID="labelContactEmail" Text="อีเมล์ผู้ติดต่อ"></asp:Label>
                                                        </label>
                                                        <div class="col-lg-4 col-md-4 col-sm-4" style="padding-bottom: 10px;">
                                                            <asp:DropDownList ID="_ddl_contact_emailTran" runat="server" class="form-control form-control-sm" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <label for="input" class="col-lg-2 col-md-2 col-sm-2" style="padding-bottom: 10px;">
                                                            <asp:Label runat="server" ID="labelContactAddress" Text="ที่อยู่ผู้ติดต่อ"></asp:Label>
                                                        </label>
                                                        <div class="col-lg-4 col-md-4 col-sm-4" style="padding-bottom: 10px;">
                                                            <asp:DropDownList ID="_ddl_contact_addressTran" runat="server" class="form-control form-control-sm" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="row d-none">
                                                        <label for="input" class="col-lg-2 col-md-2 col-sm-2" style="padding-bottom: 10px;">
                                                            <asp:Label runat="server" ID="labelProjectCode" Text="โปรเจ็ค"></asp:Label>
                                                            <span class="">
                                                                <asp:Label runat="server" ID="label2" Text="*"></asp:Label>
                                                            </span>
                                                        </label>
                                                        <div class="col-lg-4 col-md-4 col-sm-4" style="padding-bottom: 10px;">
                                                            <asp:DropDownList ID="_ddl_projectcode" runat="server" class="form-control form-control-sm" onchange="$(this).next().click();">
                                                            </asp:DropDownList>
                                                            <asp:Button ID="btnProjectCodeChange" runat="server" CssClass="d-none" OnClick="btnProjectCodeChange_Click"
                                                                OnClientClick="AGLoading(true);" ClientIDMode="Static" />
                                                        </div>
                                                        <label for="input" class="col-lg-2 col-md-2 col-sm-2" style="padding-bottom: 10px;">
                                                            <asp:Label runat="server" ID="labelProjectElement" Text="ขั้นตอนงาน"></asp:Label>
                                                        </label>
                                                        <div class="col-lg-4 col-md-4 col-sm-4" style="padding-bottom: 10px;">
                                                            <asp:DropDownList ID="_ddl_project_elementTran" runat="server" class="form-control form-control-sm" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <div class="pt-0 <%= mode_stage == agape.lib.constant.ApplicationSession.CREATE_MODE_STRING ? " d-none " : "" %>">
                                    <div class="card border-info">
                                        <div class="card-header bg-info text-white">
                                            <h6 class="mb-0">Tickets Relation</h6>
                                        </div>
                                        <div class="card-body">
                                            <div>
                                                <ag:flowchartdiagramrelationcontrol runat="server" id="FlowChartDiagramRelationControl"
                                                    relationtype="TICKET" requiredrelation="false" cssclass_add="col-md-6 col-sm-12" />

                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button Text="save" runat="server" ID="saveRelation" CssClass="hide" ClientIDMode="Static"
                                                            OnClick="saveRelation_Click" OnClientClick="AGLoading(true);" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div>
                                                <div style="min-height: 100px; max-height: 50vh; overflow: auto;" id="hierarchyReferFrom"></div>

                                                <% if (mode_stage != agape.lib.constant.ApplicationSession.CREATE_MODE_STRING)
                                                    {
                                                %>
                                                <script>
                                                    $(document).ready(function () {
                                                        bindHierarchyReferFrom();
                                                        controlDisplayEditTicketRelation(false);
                                                    });
                                                </script>
                                                <% } %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <%-- Dev Edit--%>
                            <div class="tab-pane fade" id="nav-participants" role="tabpanel" aria-labelledby="nav-participants-tab">

                                <div class="card" style="border-bottom: none;">
                                    <div class="card-body">
                                        <div class="form-row">
                                            <div class="form-group col-sm-12">
                                                <asp:UpdatePanel ID="udpnOwnerService" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <label>Owner Service</label>
                                                        <asp:DropDownList ID="ddlOwnerGroupService" CssClass="form-control form-control-sm required" runat="server"
                                                            OnSelectedIndexChanged="ddlOwnerGroupService_SelectedIndexChanged" AutoPostBack="true" onchange="AGLoading(true);">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>



                                    </div>
                                </div>
                                <div class="mat-box <%= mode_stage == agape.lib.constant.ApplicationSession.CREATE_MODE_STRING ? "" : " d-none " %>"
                                    style="border-top-left-radius: unset; border-top-right-radius: unset;">
                                    <div class="row">
                                        <div class="col-lg-6">
                                            <label>Select Employee</label>
                                            <ag:autocompleteemployee runat="server" id="AutoCompleteEmployee"
                                                cssclass="form-control form-control-sm ticket-allow-editor" />
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <style>
                                                .img-box-transfer {
                                                    float: none !important;
                                                    display: inline-block !important;
                                                    width: 38px !important;
                                                    height: 38px !important;
                                                }
                                            </style>
                                            <div style="overflow-x: auto;">
                                                <asp:UpdatePanel ID="udpTransfer" UpdateMode="Conditional" runat="server">
                                                    <ContentTemplate>
                                                        <asp:HiddenField ID="hddOwner_ListEMPCode" runat="server" ClientIDMode="Static" />
                                                        <table id="table-OwnerParticipant" class="table table-bordered table-striped table-hover table-sm">
                                                            <tr>
                                                                <th class="nowrap" style="width: 45px;">Main</th>
                                                                <th class="nowrap" style="width: 80px;">Image</th>
                                                                <th class="nowrap">Employee Code</th>
                                                                <th>Employee Name</th>
                                                                <th class="nowrap" style="width: 40px;">Del</th>
                                                            </tr>
                                                            <asp:Repeater runat="server" ID="rptOperationTransfer">
                                                                <ItemTemplate>
                                                                    <tr class="row-emp row-default" data-event="DEFAULT">
                                                                        <td class="text-center">
                                                                            <input type="radio" name="rdo-TransferMain" class="ticket-allow-editor"
                                                                                data-default-main="<%# Convert.ToBoolean(Eval("IsMain")) ? "true" : "false"%>"
                                                                                value="<%# Eval("empCode")%>" <%# Convert.ToBoolean(Eval("IsMain")) ? " checked " : ""%> />
                                                                        </td>
                                                                        <td class="text-center">
                                                                            <span class="img-box-ini-style img-box-transfer" style="background-image: url('<%# ServiceWeb.Service.UserImageService.getImgProfile(Eval("empCode").ToString()).Image_64 %>'), url(/images/user.png);"></span>
                                                                        </td>
                                                                        <td class="nowrap" data-column="emp-code" data-value="<%# Eval("empCode")%>" data-name="<%# Eval("empName")%>">
                                                                            <%# Eval("empCode")%>
                                                                        </td>
                                                                        <td>
                                                                            <%# Eval("empName")%>
                                                                        </td>
                                                                        <td class="text-center" onclick="removeRowEmpParticipant(this);">
                                                                            <a class="fa fa-trash" style="color: red;" href="Javascript:;"></a>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpParticipants">
                                    <ContentTemplate>
                                        <div class="mat-box <%= mode_stage == agape.lib.constant.ApplicationSession.CREATE_MODE_STRING ? " d-none " : "" %>"
                                            style="border-top-left-radius: unset; border-top-right-radius: unset;">
                                            <%-- Dev Module--%>
                                            
                                            <div class="text-right">
                                                <asp:Button ID="btnTransferChangeOrder" runat="server" CssClass="btn btn-warning btn-sm AUTH_MODIFY" Text="Transfer" OnClick="btnTransferChangeOrder_Click" OnClientClick="AGLoading(true);" />
                                            </div>
                                           
                                            <hr />
                                             <asp:Label ID="lbTierName" runat="server" CssClass="page-header mb-2"></asp:Label>
                                            <%-- Dev Module--%>
                                               <div class="col-md-12" style="margin-bottom: 15px;">
                                                    <div class="row">
                                                        <span class="col-md-12"><strong><span class="text-primary"><asp:Label ID="slaTitle" runat="server" text=""/></span></strong></span>
                                                        <span class="col-md-12"><strong>Detail : </strong><asp:Label ID="slaDetail" runat="server" text=""/></span>
                                                      <span class="col-md-12"><strong>Violation Time : </strong><asp:Label ID="slaTime" runat="server" text=""/></span>
                                                     <span class="col-md-12"><strong>Start date/Time : </strong><asp:Label ID="slaStartDateTime" runat="server" text=""/></span>
                                                        <span class="col-md-12"><strong>Violation Date/Time : </strong><asp:Label ID="slaEndDate" runat="server" text=""/></span>
                                                    </div>
                                                </div>
                                            <div class="form-row">
                                                <div class="col-md-4">
                                                    <div>
                                                        <label>ผู้รับมอบหมายหลัก</label>
                                                    </div>
                                                    <div class="mat-box" style="overflow-y: auto; background: aliceblue; border-left: 3px solid #4f7ea7;">
                                                        <asp:Panel ID="PanelShowMain" runat="server" CssClass="row">
                                                            <asp:Repeater ID="rptMainDelegate" runat="server">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField runat="server" ID="hddMainLinkID" Value='<%# Eval("EmployeeCode")%>' />
                                                                    <div class="col-md-2">
                                                                        <div class="img-box-ini-style" style="float: none; width: 38px; height: 38px; border-radius: 0; background-image: url('<%# ServiceWeb.Service.UserImageService.getImgProfile(Eval("EmployeeCode").ToString()).Image_64 %>'), url(/images/user.png);"></div>
                                                                    </div>
                                                                    <div class="col-md-10">
                                                                        <div class="one-line" style="font-weight: bold;">
                                                                            <asp:Label ID="lbMainDelegate" runat="server" Title='<%# Eval("FullName_EN")%>' Text='<%# Eval("FullName_EN")%>'></asp:Label>
                                                                        </div>
                                                                        <div class="one-line">
                                                                            <asp:Label ID="lbMainCharacter" runat="server" Title='<%# Eval("RoleDescript")%>' Text='<%# Eval("RoleDescript")%>'></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </asp:Panel>
                                                        <asp:Panel ID="PanelHideMain" runat="server" CssClass="row">
                                                            <% if (rptMainDelegate.Items.Count == 0)
                                                                { %>
                                                            <div class="col-md-2">
                                                                <div class="img-box-ini-style" style="float: none; width: 38px; height: 38px; border-radius: 0; background-image: url('<%= ServiceWeb.Service.PublicAuthentication.FocusOneLinkProfileImage %>');"></div>
                                                            </div>
                                                            <div class="col-md-10">
                                                                <div class="one-line" style="font-weight: bold;">
                                                                    <asp:Label ID="Label1" runat="server" Text='ไม่ได้ระบุ'></asp:Label>
                                                                </div>
                                                            </div>
                                                            <% } %>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                                <div class="col-md-8">
                                                    <div>
                                                        <label>ผู้รับมอบหมายอื่น</label>
                                                    </div>
                                                    <div class="mat-box" style="overflow-y: auto; background: lightgoldenrodyellow; border-left: 3px solid rgba(220, 220, 10, 0.45);">
                                                        <asp:Panel ID="PanelShowOther" runat="server" CssClass="row">
                                                            <asp:Repeater ID="rptOtherDelegate" runat="server">
                                                                <ItemTemplate>
                                                                    <div class="col-md-4 overview-init-owner <%# Container.ItemIndex < 3 ? "" : " d-none d-none-default " %>" style="display: inline-block;">
                                                                        <asp:HiddenField runat="server" ID="hddParLinkID" Value='<%# Eval("EmployeeCode")%>' />
                                                                        <div class="row">
                                                                            <div class="col-md-3">
                                                                                <div class="img-box-ini-style" style="float: none; width: 38px; height: 38px; border-radius: 0; background-image: url('<%# ServiceWeb.Service.UserImageService.getImgProfile(Eval("EmployeeCode").ToString()).Image_64 %>'), url(/images/user.png);"></div>
                                                                            </div>
                                                                            <div class="col-md-9">
                                                                                <div class="one-line" style="font-weight: bold;">
                                                                                    <asp:Label ID="lbOtherDelegate" runat="server" Title='<%# Eval("fullname")%>' Text='<%# Eval("fullname")%>'></asp:Label>
                                                                                </div>
                                                                                <div class="one-line">
                                                                                    <asp:Label ID="lbOtherCharacter" runat="server" Title='<%# Eval("HierarchyDesc") %>' Text='<%# Eval("HierarchyDesc")%>'></asp:Label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </asp:Panel>

                                                        <asp:Panel runat="server" ID="panelShowMore" Visible="false">
                                                            <div class="panel-load-more" onclick="showMoreParticipant(this);">
                                                                <div class="row">
                                                                    <div class="col-md-12">แสดงเพิ่มเติม...</div>
                                                                </div>
                                                            </div>
                                                            <div class="panel-load-more" onclick="hideMoreParticipant(this);" style="display: none;">
                                                                <div class="row">
                                                                    <div class="col-md-12">แสดงน้อยลง...</div>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                        <asp:Panel ID="PanelhideOther" runat="server">
                                                            <% if (rptOtherDelegate.Items.Count == 0)
                                                                { %>
                                                            <div class="col-md-4">
                                                                <div class="row">
                                                                    <div class="col-md-3">
                                                                        <div class="img-box-ini-style" style="float: none; width: 38px; height: 38px; border-radius: 0; background-image: url('<%= Page.ResolveUrl("~") %>images/user.png');"></div>
                                                                    </div>
                                                                    <div class="col-md-9">
                                                                        <div class="one-line" style="font-weight: bold;">
                                                                            <asp:Label ID="Label4" runat="server" Text='ไม่ได้ระบุ'></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <% } %>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </div>
               
                                            <asp:Panel runat="server" ID="panelFeedActivityComment" CssClass="feed-activity-command"
                                                ClientIDMode="Static">
                                                <asp:HiddenField runat="server" ID="hddAobjectlink" Value="" ClientIDMode="Static" />
                                                <asp:HiddenField runat="server" ID="hddHidePostRemark" Value="false" />
                                                <div style="cursor: pointer;" onclick="OpenChatBox(this, $('#hddAobjectlink').val(),true);">
                                                    <i class="fa fa-comments fa-fw"></i>
                                                    <b>ความคิดเห็น&nbsp;<span class="count-remark">
                                                        <asp:Label Text="0" runat="server" ID="lbl_xCountRemark" />

                                                    </span>
                                                    </b>
                                                </div>
                                                <div style="cursor: pointer; display: none;" onclick="CloseChatBox(this);">
                                                    <i class="fa fa-comments fa-fw"></i>
                                                    <b>ซ่อนความคิดเห็น</b>
                                                </div>
                                            </asp:Panel>
                                            <div class="ag-remarker"></div>
                            </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <%-- Dev Edit--%>

                            <div class="tab-pane fade" id="nav-item" role="tabpanel" aria-labelledby="nav-item-tab">

                                <div class="form-row">
                                    <div class="form-row col-sm-12">

                                        <div class="col-sm-6">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <button type="button" id="btnaddCI" class="btn btn-primary btn-sm mb-1 AUTH_MODIFY" onclick="$('#<%= btnOpenModalSelectConfigurationItem.ClientID  %>').click();">
                                                        <i class="fa fa-plus-circle"></i>&nbsp;&nbsp;Add CI
                                                    </button>
                                                    <asp:Button ID="btnOpenModalSelectConfigurationItem" Text="Add" class="d-none" OnClientClick="showInitiativeModal('modalSearchHelpConfigurationItem');" runat="server" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="col-sm-6">

                                            <!-- <ag:autocompleteequipment runat="server" id="AutoCompleteEquipment" cssclass="form-control form-control-sm"
                                            afterselectedchange="addNewEquipment();" />-->
                                        </div>
                                    </div>
                                    <br />
                                    <div class="form-group col-lg-12">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpLiatCI">
                                            <ContentTemplate>
                                                <asp:Repeater runat="server" ID="rptEquipment">
                                                    <ItemTemplate>
                                                        <div class="card item-card-ci-select">
                                                            <div class="card-body">
                                                                <div class="form-row">
                                                                    <div class="form-group <%= mode_stage == agape.lib.constant.ApplicationSession.CREATE_MODE_STRING ? " col-10 " : " col-12 " %>">
                                                                        <label>Configuration Item No.</label>
                                                                        <button type="button" class="btn btn-info btn-sm mb-1 ticket-allow-editor-everyone float-right" onclick="$(this).next().click();">Edit</button>
                                                                        
                                                                        <asp:Button ID="btnOpenDetailEquipment_Click" ClientIDMode="Static" OnClick="btnOpenDetailEquipment_Click" OnClientClick="AGLoading(true);"
                                                                        Text='<%# Eval("EquipmentCode") %>' CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" runat="server" />

                                                                        <asp:TextBox ID="txtEquipmentNo" runat="server" CssClass="form-control form-control-sm"
                                                                            Enabled="false" Text='<%# Eval("EquipmentCode") + " - " + Eval("EquipmentName") %>'></asp:TextBox>
                                                                        <asp:HiddenField runat="server" ID="hddEquipmentCode" Value='<%# Eval("EquipmentCode") %>' />
                                                                    </div>
                                                                    <% if (mode_stage == agape.lib.constant.ApplicationSession.CREATE_MODE_STRING)
                                                                        { %>
                                                                    <div class="form-group col-2">
                                                                        <label>&nbsp;</label>
                                                                        <div class="text-right">
                                                                            <button type="button" class="btn btn-danger btn-sm mb-1"
                                                                                onclick="removeItemEquipment(this, '<%# Eval("EquipmentCode").ToString().Replace("'", "\'") + " - " + Eval("EquipmentName").ToString().Replace("'", "\'") %>');">
                                                                                <i class="fa fa-trash"></i>&nbsp;&nbsp;Remove
                                                                            </button>
                                                                            <asp:Button Text="" runat="server" CssClass="d-none" ID="btnRemoveItemEquipment"
                                                                                CommandArgument='<%# Eval("EquipmentCode") %>'
                                                                                OnClick="btnRemoveItemEquipment_Click" OnClientClick="AGLoading(true);" />
                                                                        </div>
                                                                    </div>
                                                                    <% } %>
                                                                </div>
                                                                <div class="form-row">
                                                                    <div class="form-group col-md-3 col-sm-6">
                                                                        <label>Family</label>
                                                                        <asp:TextBox ID="tbEquipmentType" runat="server" CssClass="form-control form-control-sm"
                                                                            Enabled="false" Text='<%# Eval("EquipmentTypeDesc") %>'></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group col-md-3 col-sm-6">
                                                                        <label>Class</label>
                                                                        <asp:TextBox ID="tbEquipmentClass" runat="server" CssClass="form-control form-control-sm"
                                                                            Enabled="false" Text='<%# Eval("ClassName") %>'></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group col-md-3 col-sm-6">
                                                                        <label>Category</label>
                                                                        <asp:TextBox ID="tbEquipmentCategory" runat="server" CssClass="form-control form-control-sm"
                                                                            Enabled="false" Text='<%# Eval("CategoryName") %>'></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group col-md-3 col-sm-6">
                                                                        <label>Serial No.</label>
                                                                        <asp:TextBox ID="tbSerialNo" runat="server" CssClass="form-control form-control-sm"
                                                                            Text='<%# getSerialNoCI(Eval("EquipmentCode").ToString()) %>'></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div class="col-lg-5 d-none">
                                        <div class="form-group">
                                            <div style="width: 100%;">
                                                <div style="width: 100px; display: inline-flex;">
                                                    <label>Incident Area</label>
                                                </div>
                                                <div style="width: calc(100% - 110px); display: inline-flex; text-align: right;" class="">
                                                    <span id="lbl-incident-area-selected" class="text-primary float-right font-weight-bold one-line"></span>
                                                </div>
                                            </div>
                                            <div class="card">
                                                <div id="card-incident-area" class="card-body" style="min-height: 333px; max-height: 333px; padding: 0 10px; overflow-y: auto;">
                                                    <div style="position: sticky; top: 0px; z-index: 1; background-color: #fff;">
                                                        <input type="text" name="name" id="filter-able-IncidentArea" autocomplete="off" onkeyup="IncidentAreaFilterAble(this);"
                                                            style="top: 8px; position: relative; z-index: 1; height: 30px; padding: 3px 10px; font-size: 14px;"
                                                            class="form-control filter-able input-sm ticket-allow-editor" placeholder="Search..">

                                                        <div class="search-result-box" style="top: 40px;">
                                                            <div class="search-result-command">
                                                                <i class="fa fa-caret-left pull-left hand" onclick="IncidentAreaFilterAbleFocusClick('PREV');"></i>
                                                                <span class="search-result-count">
                                                                    <span class="search-result-command-at">0</span>
                                                                    /
                                                                <span class="search-result-command-all">0</span>
                                                                </span>
                                                                <i class="fa fa-caret-right pull-right hand" onclick="IncidentAreaFilterAbleFocusClick('NEXT');"></i>
                                                            </div>
                                                            <span class="search-result"></span>
                                                        </div>
                                                    </div>
                                                    <div style="position: relative; margin-top: 5px;">
                                                        <div id="hierarchyIncidentTicket"></div>
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:UpdatePanel ID="udpUpdateAreaCode" UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <asp:HiddenField runat="server" ID="hddIncidentAreaCode" ClientIDMode="Static" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <script>
                                            $(document).ready(function () {
                                                //bindHierarchyIncidentTicket();
                                                hederdoctrancolor()
                                            });

                                            function hederdoctrancolor() {
                                                var IsClose = '<%= IsCallTicketStatusClose.ToString().ToLower() %>' == 'true';
                                                var IsCancel = '<%= IsCallTicketStatusCancel.ToString().ToLower() %>' == 'true';

                                                if (IsClose) {
                                                    $("#lal_headerdoc").addClass("headerdocgreen")
                                                    $("#<%= lbl_docnumberTran.ClientID %>").addClass("headerdocgreen")
                                                }
                                                else if (IsCancel) {
                                                    $("#lal_headerdoc").addClass("headerdocred")
                                                    $("#<%= lbl_docnumberTran.ClientID %>").addClass("headerdocred")
                                                }
                                                else {
                                                    if ($('#<%= tbEndDate.ClientID %>').val() == '' && $('#<%= txtEndTime.ClientID %>').val() == '') {
                                                        return;
                                                    }
                                                    var arrPlanDate = $('#<%= tbEndDate.ClientID %>').val().split('/');
                                                    var arrPlanTime = $('#<%= txtEndTime.ClientID %>').val().split(':');
                                                    var planEndDateTime = parseInt(arrPlanDate[2] + arrPlanDate[1] + arrPlanDate[0] + arrPlanTime[0] + arrPlanTime[1] + '00');
                                                    var curDateTime = parseInt(new Date().format('yyyyMMddHHmmss'));
                                                    if (curDateTime > planEndDateTime) {
                                                        $("#lal_headerdoc").addClass("headerdocred")
                                                        $("#<%= lbl_docnumberTran.ClientID %>").addClass("headerdocred")
                                                    }
                                                }

                                                <%--var status = $('#_txt_TicketStatusTran').val();
                                                var dateplan = $("#<%= tbEndDate.ClientID %>").val()

                                                var datep = new Date(<%=Agape.FocusOne.Utilities.Validation.Convert2DateDB(tbEndDate.Text) %>);
                                                 
                                                console.log(datep);
                                                
                                                if (status === "14 : Cancel") {

                                                    $("#lal_headerdoc").addClass("headerdocred")
                                                    $("#<%= lbl_docnumberTran.ClientID %>").addClass("headerdocred")
                                                } else if (status === "13 : Close") {
                                                    $("#lal_headerdoc").addClass("headerdocgreen")
                                                    $("#<%= lbl_docnumberTran.ClientID %>").addClass("headerdocgreen")
                                                }--%>
                                            }

                                            function headerdoctrandatecolor() {
                                                $("#lal_headerdoc").addClass("headerdocred")
                                                $("#<%= lbl_docnumberTran.ClientID %>").addClass("headerdocred")
                                            }
                                        </script>
                                    </div>

                                </div>
                                <br />
                            </div>

                            <div class="tab-pane fade" id="nav-effectcus" role="tabpanel" aria-labelledby="nav-effectcus-tab">
                                <div class="form-row">

                                    <div class="col-sm-6">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <button type="button" id="btnaddEffect" class="btn btn-primary btn-sm mb-1 AUTH_MODIFY" onclick="$('#<%= btnOpenModalSearchHelpCustomer.ClientID  %>').click();">
                                                    <i class="fa fa-plus-circle"></i>&nbsp;&nbsp;Add Eff
                                                </button>
                                                <asp:Button ID="btnOpenModalSearchHelpCustomer" Text="Add" class="d-none" OnClientClick="showInitiativeModal('modalSearchHelpCustomer');" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-sm-6">
                                    </div>
                                </div>
                                <br />

                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updateCus">
                                    <ContentTemplate>
                                        <asp:Repeater runat="server" ID="cusLists">
                                            <ItemTemplate>
                                                <div class="card item-card-customer-select">
                                                    <div class="card-body">
                                                        <div class="form-row">
                                                            <div class="form-group <%= mode_stage == agape.lib.constant.ApplicationSession.CREATE_MODE_STRING ? " col-10 " : " col-12 " %>">
                                                                <label>Client No.</label>
                                                                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control form-control-sm"
                                                                    Enabled="false" Text='<%# Eval("CustomerCode") + " : " + Eval("CustomerName") %>'></asp:TextBox>
                                                                <asp:HiddenField runat="server" ID="hddCustomerCode" Value='<%# Eval("CustomerCode") %>' />
                                                            </div>
                                                            <% if (mode_stage == agape.lib.constant.ApplicationSession.CREATE_MODE_STRING)
                                                                { %>
                                                            <div class="form-group col-2">
                                                                <label>&nbsp;</label>
                                                                <div class="text-right">
                                                                    <button type="button" class="btn btn-danger btn-sm mb-1"
                                                                        onclick="removeItemEquipment(this, '<%# Eval("CustomerCode").ToString().Replace("'", "\'") + " - " + Eval("CustomerName").ToString().Replace("'", "\'") %>');">
                                                                        <i class="fa fa-trash"></i>&nbsp;&nbsp;Remove
                                                                    </button>
                                                                    <asp:Button Text="" runat="server" CssClass="d-none" ID="Button2"
                                                                        CommandArgument='<%# Eval("CustomerCode") %>'
                                                                        OnClick="btnRemoveEffectedCustomer_Click" OnClientClick="AGLoading(true);" />
                                                                </div>
                                                            </div>
                                                            <% } %>
                                                        </div>
                                                        <%--<div class="form-row hide">
                                                            <div class="form-group col-lg-6 col-sm-12">
                                                                <label>Contact</label>
                                                                <ag:AutoCompleteControl runat="server" ID="_ddl_contact_person_search"
                                                                    CustomViewCode="contact"
                                                                    TODO_FunctionJS="loadcontactDetail();" CssClass="form-control form-control-sm" />
                                                            </div>
                                                            <div class="form-group col-lg-3 col-sm-12">
                                                                <label>Contact E-Mail</label>
                                                                <asp:TextBox ID="txtContactEmail_Ef" Enabled="false" runat="server"
                                                                    CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                            <div class="form-group col-lg-3 col-sm-12">
                                                                <label>Contact Phone</label>
                                                                <asp:TextBox ID="txtContactPhone_Ef" Enabled="false" runat="server"
                                                                    CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>--%>
                                                    </div>
                                                </div>
                                                <br />

                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button runat="server" CssClass="d-none" ID="btnDetailCustomerEffect"
                                        OnClick="btnSelectContactBindDetail_Click" ClientIDMode="Static" />

                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="tab-pane fade" id="nav-attachment" role="tabpanel" aria-labelledby="liattachTran">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpIsLoad_Attachment">
                                    <ContentTemplate>
                                        <asp:CheckBox Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" ID="chkIsLoad_Attachment" />
                                        <asp:Button Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" ID="btnIsLoad_Attachment"
                                            data-tabload="nav-attachment" ClientIDMode="Static"
                                            OnClientClick="AGLoading(true);" OnClick="btnIsLoad_Attachment_Click"/>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="d-none">
                                    <sna:attachfileusercontrol id="AttachFileUserControl" runat="server"></sna:attachfileusercontrol>
                                </div>
                                <div class="row">
                                    <div class="col-12">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpLogFileAttachment">
                                            <ContentTemplate>
                                                <table class="table table-striped table-hover table-sm" id="TableLogAttachmentFile">
                                                    <thead>
                                                        <tr>
                                                            <th>#</th>
                                                            <th>File Name</th>
                                                            <th>Uplaod By</th>
                                                            <th>Date Time Upload</th>
                                                            <th>Download</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater runat="server" ID="rptLogFileAttachment">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="text-center">
                                                                        <%# Container.ItemIndex + 1 %>
                                                                    </td>
                                                                    <td>
                                                                        <%# Eval("FileName") %>
                                                                    </td>
                                                                    <td>
                                                                        <%# Eval("CREATE_BY_FULLNAME") %>
                                                                    </td>
                                                                    <td>
                                                                        <%# Eval("CREATE_ON") %>
                                                                    </td>
                                                                    <td class="text-center">
                                                                        <a href="<%# Eval("ContentUrl") %>" target="_blank" download="<%# Eval("FileName") %>">
                                                                            <i class="fa fa-download"></i>
                                                                        </a>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane fade" id="nav-chat" role="tabpanel" aria-labelledby="lichatTran">
                                <asp:UpdatePanel ID="updRemarkService" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div id="ag-remark"></div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="tab-pane fade" id="nav-Date-Time-log" role="tabpanel" aria-labelledby="lichangelogTran">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpIsLoad_DateTimeLog">
                                    <ContentTemplate>
                                        <asp:CheckBox Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" ID="chkIsLoad_DateTimeLog" />
                                        <asp:Button Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" ID="btnIsLoad_DateTimeLog"
                                            data-tabload="nav-Date-Time-log" ClientIDMode="Static"
                                            OnClientClick="AGLoading(true);" OnClick="btnIsLoad_DateTimeLog_Click"/>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpLogDateTime">
                                    <ContentTemplate>
                                        <div class="form-row">
                                            <div class="form-group col-md-6 col-sm-12 m-0">
                                                <div class="form-row">
                                                    <div class="form-group col-md-6 col-sm-6 col-xs-12">
                                                        <label>Open Date-Time</label>
                                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" Enabled="false"
                                                            ID="txtlog_OpenDateTime" />
                                                    </div>
                                                    <div class="form-group col-md-6 col-sm-6 col-xs-12">
                                                        <label>Resolve Date-Time</label>
                                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" Enabled="false"
                                                            ID="txtLog_ResolveDateTime" />
                                                    </div>
                                                    <div class="form-group col-md-6 col-sm-6 col-xs-12">
                                                        <label>Last Modified Date-Time</label>
                                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" Enabled="false"
                                                            ID="txtlog_LastModifiedDateTime" />
                                                    </div>
                                                    <div class="form-group col-md-6 col-sm-6 col-xs-12">
                                                        <label>Close Date</label>
                                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" Enabled="false"
                                                            ID="txtLog_CloseDateTime" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group col-md-6 col-sm-12 m-0">
                                                <div class="form-row">
                                                    <div class="form-group col-md-6 col-sm-6 col-xs-12">
                                                        <label>Total Time</label>
                                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" Enabled="false"
                                                            ID="txtLog_TotalTime" />
                                                    </div>
                                                    <div class="form-group col-md-6 col-sm-6 col-xs-12">
                                                        <label>Stop Time</label>
                                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" Enabled="false"
                                                            ID="txtLog_StopTime" />
                                                    </div>
                                                    <div class="form-group col-md-6 col-sm-6 col-xs-12">
                                                        <label>Total Time (Without stop)</label>
                                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" Enabled="false"
                                                            ID="txtLog_TotalTime_WithoutStop" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="tab-pane fade" id="nav-knowledge" role="tabpanel" aria-labelledby="liknowledgeTran">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpIsLoad_Knowledge">
                                    <ContentTemplate>
                                        <asp:CheckBox Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" ID="chkIsLoad_Knowledge" />
                                        <asp:Button Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" ID="btnIsLoad_Knowledge"
                                            data-tabload="nav-knowledge" ClientIDMode="Static"
                                            OnClientClick="AGLoading(true);" OnClick="btnIsLoad_Knowledge_Click"/>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="udpKnowledgeData" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <div class="table-responsive">
                                            <table id="table-dataview-KnowledgeManagement" style="width: 100%;" class="table table-bordered table-striped table-hover table-sm nowrap">
                                                <thead>
                                                    <tr>
                                                        <th class="text-center"></th>
                                                        <th>Knowledge Group</th>
                                                        <th>Knowledge ID</th>
                                                        <th>Keyword</th>
                                                        <th>Subject</th>
                                                        <th>Detail</th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hhdKnowledgeIDRefTicketNO" ClientIDMode="Static" runat="server" />
                                        <asp:Button ID="btnRedirectKnowledgeIDRefTicketNO" CssClass="d-none" ClientIDMode="Static" OnClick="btnRedirectKnowledgeIDRefTicketNO_Click" OnClientClick="AGLoading(true);" Text="" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="tab-pane fade" id="nav-changelog" role="tabpanel" aria-labelledby="lichangelogTran">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpIsLoad_TicketChangeLog">
                                    <ContentTemplate>
                                        <asp:CheckBox Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" ID="chkIsLoad_TicketChangeLog" />
                                        <asp:Button Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" ID="btnIsLoad_TicketChangeLog" 
                                            data-tabload="nav-changelog" ClientIDMode="Static"
                                            OnClientClick="AGLoading(true);" OnClick="btnIsLoad_TicketChangeLog_Click"/>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <sna:changelogcontrol id="TicketChangeLog" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="initiative-model-control-slide-panel" id="modal-workflow">
            <div class="initiative-model-control-body-content z-depth-3">
                <div>
                    <div class="initiative-model-control-header">
                        <div class="mat-box-initaive-control">
                            <div class="pull-right">
                                <i class="fa fa-close hand" onclick="closeInitiativeModal('modal-workflow');"></i>
                            </div>
                            <div class="one-line">
                                <label class="text-warning">
                                    Work Flow
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="initiative-model-control-contant">
                        <div class="panel-body-initiative-master">
                            <div class="panel-content-initiative-master">
                                <div class="mat-box-initaive-control tab-initiative-control">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpWorkflowDetail">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <sna:approvestategatecontrol runat="server" id="ApproveStateGateControl" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <sna:approvalprocedurecontrol runat="server" id="ApprovalProcedureControl" />
                                                    <!-- Dev ส่วนนี้-->
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="initiative-model-bottom">
                        <div class="text-right">
                            <a class="water-button" onclick="closeInitiativeModal('modal-workflow');"><i class="fa fa-close"></i>&nbsp;Close</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="initiative-model-control-slide-panel" id="modal-CreateNewTicketReferent">
            <div class="initiative-model-control-body-content z-depth-3">
                <div>
                    <div class="initiative-model-control-header">
                        <div class="mat-box-initaive-control">
                            <div class="pull-right">
                                <i class="fa fa-close hand" onclick="closeInitiativeModal('modal-CreateNewTicketReferent');"></i>
                            </div>
                            <div class="one-line">
                                <label class="text-warning">
                                    Create Reference Ticket No.
                                <asp:Label Text="" runat="server" ID="lblTicketNo_Modal" />
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="initiative-model-control-contant">
                        <div class="panel-body-initiative-master">
                            <div class="panel-content-initiative-master">
                                <div class="mat-box-initaive-control tab-initiative-control">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpPanelCreatedNew">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <label>Client</label>
                                                    <ag:autocompletecustomer id="CustomerSelect" runat="server" notautobindcomplete="true"
                                                        cssclass="form-control form-control-sm required-popup ticket-allow-editor">
                                                    </ag:autocompletecustomer>
                                                </div>
                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <label>Ticket Type</label>
                                                    <asp:DropDownList ID="_ddl_sctype" runat="server" class="form-control form-control-sm required-popup ticket-allow-editor"
                                                        ClientIDMode="Static">
                                                    </asp:DropDownList>

                                                </div>
                                                <div class="col-md-2 col-sm-4 col-xs-12 d-none">
                                                    <label>Fiscal Year</label>
                                                    <input id="_txt_year" type="text" class="form-control form-control-sm required-popup ticket-allow-editor" runat="server" clientidmode="Static"
                                                        onkeypress='return event.charCode >= 48 && event.charCode <= 57' />
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:HiddenField runat="server" ID="hddEquepmentCodeRef" ClientIDMode="Static" />
                                            <asp:Button Text="" runat="server" CssClass="d-none" ID="btnCreateNewTicketRef"
                                                OnClick="btnCreateNewTicketRef_Click" ClientIDMode="Static" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="initiative-model-bottom">
                        <div class="text-right">
                            <span class="water-button" onclick="newTicketRefModalClick(this)"><i class="fa fa-file-o"></i>&nbsp;Create Ticket</span>
                            <a class="water-button" onclick="closeInitiativeModal('modal-CreateNewTicketReferent');"><i class="fa fa-close"></i>&nbsp;Close</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%--Knowledge Management--%>
        <div class="initiative-model-control-slide-panel" id="modal-addKnowledgeManagement">
            <div class="initiative-model-control-body-content z-depth-3">
                <div>
                    <div class="initiative-model-control-header">
                        <div class="mat-box-initaive-control">
                            <div class="pull-right">
                                <i class="fa fa-close hand" onclick="closeInitiativeModal('modal-addKnowledgeManagement');"></i>
                            </div>
                            <div class="one-line">
                                <label class="text-warning">
                                    Add Knowledge
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="initiative-model-control-contant">
                        <div class="panel-body-initiative-master">
                            <div class="panel-content-initiative-master">
                                <div class="mat-box-initaive-control tab-initiative-control">
                                    <div class="form-row">
                                        <div class="form-group col-sm-6">
                                            <label>Knowledge Group</label>
                                            <asp:DropDownList runat="server" ID="ddlGroup" CssClass="form-control form-control-sm ticket-allow-editor">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group col-sm-6">
                                            <label>Search Text </label>
                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm ticket-allow-editor"></asp:TextBox>
                                        </div>
                                    </div>
                                    <button type="button" class="btn btn-sm btn-info" onclick="inactiveRequireField();$('#<%= btnSearchDataKnowledge.ClientID  %>').click();"><i class="fa fa-search"></i>&nbsp;&nbsp;Search</button>
                                    <asp:UpdatePanel ID="udpsearchButton" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnSearchDataKnowledge" runat="server" CssClass="d-none" OnClick="btnSearchDataKnowledge_Click" OnClientClick="AGLoading(true);" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                    <hr class="mb-0" />

                                    <div class="form-row">
                                        <div class="form-group col-sm-12">
                                            <br />
                                            <asp:UpdatePanel ID="udpAddKnowledgeData" UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <asp:HiddenField ID="hhdDataSourceSelectAddKnowledgeManagement" ClientIDMode="Static" runat="server" />
                                                    <div class="table-responsive">

                                                        <table id="table-dataview-AddKnowledgeManagement" style="width: 100%;" class="table table-bordered table-striped table-hover table-sm nowrap">
                                                            <thead>
                                                                <tr>
                                                                    <th class="text-center text-nowrap">Select</th>
                                                                    <th>Knowledge Group</th>
                                                                    <th>Knowledge ID</th>
                                                                    <th>Keyword</th>
                                                                    <th>Subject</th>
                                                                    <th>Detail</th>
                                                                </tr>
                                                            </thead>
                                                            <asp:Repeater ID="rptAddKnowledgeManagement" OnItemDataBound="rptAddKnowledgeManagement_ItemDataBound" runat="server">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td class="text-center text-nowrap" data-key="<%# Eval("ObjectID")%>">
                                                                            <asp:CheckBox ID="chkSelectAddKnowledge" CssClass="ticket-allow-editor" Checked="false" Text="" runat="server" />
                                                                        </td>
                                                                        <td><%# Eval("KMGroupName") %></td>
                                                                        <td><%# Eval("ObjectID") %></td>
                                                                        <td><%# Eval("PrimaryKeyWord") %></td>
                                                                        <td><%# Eval("Description") %></td>
                                                                        <td><%# Eval("Details") %></td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="initiative-model-bottom">
                        <div class="text-right">
                            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <span class="water-button" onclick="inactiveRequireField();$(this).next().click();"><i class="fa fa-file-o AUTH_MODIFY"></i>&nbsp;Save</span>
                                    <asp:Button ID="btnSaveAddNewKnowledge" CssClass="d-none" OnClick="btnSaveAddNewKnowledge_Click" OnClientClick="return confirmSaveAddKnowledgeMapping(this);" Text="save" runat="server" />
                                    <a class="water-button" onclick="closeInitiativeModal('modal-addKnowledgeManagement');"><i class="fa fa-close"></i>&nbsp;Close</a>
                                    <asp:Button ID="btnOpenModalAddKM" CssClass="d-none" OnClick="btnOpenModalAddKM_Click" OnClientClick="AGLoading(true);" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%--modal create ticket--%>
        <div class="initiative-model-control-slide-panel" id="modal-CreateDataKM">
            <div class="initiative-model-control-body-content z-depth-3">
                <div>
                    <div class="initiative-model-control-header">
                        <div class="mat-box-initaive-control">
                            <div class="pull-right">
                                <i class="fa fa-close hand" onclick="inactiveRequireField();closeInitiativeModal('modal-CreateDataKM');"></i>
                            </div>
                            <div class="one-line">
                                <label class="text-warning">
                                    Create Knowledge Management
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="initiative-model-control-contant">
                        <div class="panel-body-initiative-master">
                            <div class="panel-content-initiative-master">
                                <div class="mat-box-initaive-control tab-initiative-control">

                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCreateDetail">
                                        <ContentTemplate>
                                            <div class="form-row">
                                                <asp:HiddenField ID="hhdKeyAobjectlink" runat="server" />
                                                <div class="form-group col-md-6">
                                                    <label>Knowledge Group</label>
                                                    <asp:DropDownList runat="server" ID="ddlGroupModal" CssClass="form-control form-control-sm ticket-allow-editor required">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="form-group col-md-6">
                                                    <label>Keyword</label>
                                                    <asp:TextBox ID="txtKeywordModal" CssClass="form-control form-control-sm ticket-allow-editor" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="form-group col-md-6">
                                                    <label>Subject</label>
                                                    <asp:TextBox ID="txtSubjectModal" TextMode="MultiLine" Rows="3" CssClass="form-control form-control-sm ticket-allow-editor required" runat="server" />
                                                </div>
                                                <div class="form-group col-md-6">
                                                    <label>Detail</label>
                                                    <asp:TextBox ID="txtDetailModal" TextMode="MultiLine" Rows="3" CssClass="form-control form-control-sm ticket-allow-editor" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="form-group col-md-6">
                                                    <label>Symtom</label>
                                                    <asp:TextBox ID="txtSymtomModal" TextMode="MultiLine" Rows="3" CssClass="form-control form-control-sm ticket-allow-editor" runat="server" />
                                                </div>
                                                <div class="form-group col-md-6">
                                                    <label>Cause</label>
                                                    <asp:TextBox ID="txtCauseModal" TextMode="MultiLine" Rows="3" CssClass="form-control form-control-sm ticket-allow-editor" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="form-group col-md-12">
                                                    <label>Solution</label>
                                                    <asp:TextBox ID="txtSolutionModal" TextMode="MultiLine" Rows="3" CssClass="form-control form-control-sm ticket-allow-editor" runat="server" />
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button runat="server" CssClass="d-none" ID="btnSubmitRequireKM"
                                                OnClick="btnSubmitRequireKM_Click" ClientIDMode="Static" />
                                            <asp:Button Text="" runat="server" CssClass="d-none" ID="btnCreateKMDetail"
                                                OnClick="btnCreateKMDetail_Click" ClientIDMode="Static" OnClientClick="return CreateKMDetailRefTicketModalClick(this);" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="initiative-model-bottom">
                        <div class="text-right">
                            <span class="water-button" onclick="submitCheckRequireField();"><i class="fa fa-file-o AUTH_MODIFY"></i>&nbsp;Save</span>
                            <a class="water-button" onclick="inactiveRequireField();closeInitiativeModal('modal-CreateDataKM');"><i class="fa fa-close"></i>&nbsp;Close</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%-- Modal Transfer --%>
        <div class="initiative-model-control-slide-panel" id="modal-TransferAddParticipant">
            <div class="initiative-model-control-body-content z-depth-3">
                <div>
                    <div class="initiative-model-control-header">
                        <div class="mat-box-initaive-control">
                            <div class="pull-right">
                                <i class="fa fa-close hand" onclick="closeInitiativeModal('modal-TransferAddParticipant');"></i>
                            </div>
                            <div class="one-line">
                                <label class="text-warning">
                                    Tranfer Ticket.
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="initiative-model-control-contant">
                        <div class="panel-body-initiative-master">
                            <div class="panel-content-initiative-master">
                                <div class="mat-box-initaive-control tab-initiative-control">
                                    <div class="row">
                                        <div class="col-lg-6">
                                            <label>Select Employee</label>
                                            <ag:autocompleteemployee runat="server" id="AutoCompleteEmployee_Transfer"
                                                cssclass="form-control form-control-sm ticket-allow-editor" />
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <style>
                                                .img-box-transfer {
                                                    float: none !important;
                                                    display: inline-block !important;
                                                    width: 38px !important;
                                                    height: 38px !important;
                                                }
                                            </style>
                                            <div style="overflow-x: auto;">
                                                <asp:UpdatePanel ID="udpTransferChangeorder" UpdateMode="Conditional" runat="server">
                                                    <ContentTemplate>
                                                        <asp:HiddenField ID="hddTransferChangeOrder_TaskName" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddTransferChangeOrder_TierCode" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddTransferChangeOrder_Tier" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddTransferChangeOrder_AOBJECTLINK" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddTransferChangeOrder_ListEMPCode" runat="server" ClientIDMode="Static" />
                                                        <%--<table id="table-search-EmployeeParticipant" class="table table-bordered table-striped table-hover table-sm">
                                                        <thead>
                                                            <tr>
                                                                <th class="nowrap" style="width: 60px;"></th>
                                                                <th class="nowrap" style="width: 80px;"></th>
                                                                <th class="nowrap">Employee Code</th>
                                                                <th>Employee Name</th>
                                                            </tr>
                                                        </thead>
                                                    </table>

                                                    <hr />--%>

                                                        <table id="table-TransferParticipantChangeOrder" class="table table-bordered table-striped table-hover table-sm">
                                                            <tr>
                                                                <th class="nowrap" style="width: 45px;">Main</th>
                                                                <th class="nowrap" style="width: 80px;">Image</th>
                                                                <th class="nowrap">Employee Code</th>
                                                                <th>Employee Name</th>
                                                                <th class="nowrap" style="width: 40px;">Del</th>
                                                            </tr>
                                                            <asp:Repeater runat="server" ID="rptOperationTransferChangeorder">
                                                                <ItemTemplate>
                                                                    <tr class="row-emp row-default" data-event="DEFAULT">
                                                                        <td class="text-center">
                                                                            <input type="radio" name="rdo-TransferMain" class="ticket-allow-editor"
                                                                                data-default-main="<%# Convert.ToBoolean(Eval("IsMain")) ? "true" : "false"%>"
                                                                                value="<%# Eval("empCode")%>" <%# Convert.ToBoolean(Eval("IsMain")) ? " checked " : ""%> />
                                                                        </td>
                                                                        <td class="text-center">
                                                                            <span class="img-box-ini-style img-box-transfer" style="background-image: url('<%= Page.ResolveUrl("~") %>images/user.png');"></span>
                                                                        </td>
                                                                        <td class="nowrap" data-column="emp-code" data-value="<%# Eval("empCode")%>" data-name="<%# Eval("empName")%>">
                                                                            <%# Eval("empCode")%>
                                                                        </td>
                                                                        <td>
                                                                            <%# Eval("empName")%>
                                                                        </td>
                                                                        <td class="text-center" onclick="removeRowEmpParticipant(this);">
                                                                            <a class="fa fa-trash" style="color: red;" href="Javascript:;"></a>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="initiative-model-bottom">
                        <div class="text-right">
                            <asp:UpdatePanel ID="udpSaveContact" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <span class="water-button" onclick="$(this).next().click();"><i class="fa fa-file-o AUTH_MODIFY"></i>&nbsp;Save</span>
                                    <asp:Button ID="btnTransferAddParticipantChangeOrder" CssClass="d-none" OnClick="btnTransferAddParticipantChangeOrder_Click" OnClientClick="return prepareDateForSaveEmpParticipant_Changeorder(this, 'TransferParticipantChangeOrder');" Text="save" runat="server" />
                                    <a class="water-button" onclick="closeInitiativeModal('modal-TransferAddParticipant');"><i class="fa fa-close"></i>&nbsp;Close</a>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="Modal-CloseTicket" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Close Ticket</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-row">
                            <div class="col-12">
                                <label>
                                    <b>Remark</b>
                                </label>
                                <asp:TextBox runat="server" ID="txtRemarkCloseTicket" CssClass="form-control input-sm ticket-allow-editor"
                                    TextMode="MultiLine" Rows="4" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <button type="button" class="btn btn-default mb-1" data-dismiss="modal">
                                    <i class="fa fa-times fa-fw"></i>
                                    Close
                                </button>
                                <button type="button" class="btn btn-success mb-1 AUTH_MODIFY" onclick="return clickCloseWork(this);">
                                    <i class="fa fa-check-square-o fa-fw"></i>
                                    Close Ticket
                                </button>
                                <asp:Button Text="" runat="server" CssClass="hide" ID="btnCloseTicket"
                                    OnClick="btnCloseTicket_Click" OnClientClick="AGLoading(true);" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div class="d-none">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCodeidentityInitiative">
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="hddInitiativeIOLinkID" />
                    <asp:Button Text="" runat="server" ID="btnRebindWorkFlow" ClientIDMode="Static" CssClass=" ticket-allow-editor ticket-allow-editor-everyone"
                        OnClick="btnRebindWorkFlow_Click" OnClientClick="AGLoading(true);" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <%--21/11/2561 modal configuration item (by born kk) --%>
        <div class="initiative-model-control-slide-panel" id="modalSearchHelpConfigurationItem">
            <div class="initiative-model-control-body-content z-depth-3">
                <div>
                    <div class="initiative-model-control-header">
                        <div class="mat-box-initaive-control">
                            <div class="pull-right">
                                <i class="fa fa-close hand" onclick="closeInitiativeModal('modalSearchHelpConfigurationItem');"></i>
                            </div>
                            <div class="one-line">
                                <label class="text-warning">
                                    Select Configuration Item
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="initiative-model-control-contant">
                        <div class="panel-body-initiative-master">
                            <div class="panel-content-initiative-master">
                                <div class="mat-box-initaive-control tab-initiative-control">
                                    <ag:SearchHelpCIControl runat="server" ID="SearchHelpCIControl" />
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional" class="d-none">
                                        <ContentTemplate>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <%-----%>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="initiative-model-bottom">
                        <div class="text-right">
                            <a class="water-button AUTH_MODIFY" onclick="saveCI()" runat="server"><i class="fa fa-save"></i>&nbsp;Save</a>
                            <a class="water-button" onclick="closeInitiativeModal('modalSearchHelpConfigurationItem');"><i class="fa fa-close"></i>&nbsp;Close</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%--21/11/2561 modal effected customer (by born kk) --%>
        <div class="initiative-model-control-slide-panel" id="modalSearchHelpCustomer">
            <div class="initiative-model-control-body-content z-depth-3">
                <div>
                    <div class="initiative-model-control-header">
                        <div class="mat-box-initaive-control">
                            <div class="pull-right">
                                <i class="fa fa-close hand" onclick="closeInitiativeModal('modalSearchHelpCustomer');"></i>
                            </div>
                            <div class="one-line">
                                <label class="text-warning">
                                    Select Affected Client
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="initiative-model-control-contant">
                        <div class="panel-body-initiative-master">
                            <div class="panel-content-initiative-master">
                                <div class="mat-box-initaive-control tab-initiative-control">
                                    <%-----%><ag:SearchCustomerControl runat="server" id="SearchCustomerControl" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="initiative-model-bottom">
                        <div class="text-right">
                            <a class="water-button AUTH_MODIFY" onclick="saveCustomer()"><i class="fa fa-save"></i>&nbsp;Save</a>
                            <a class="water-button" onclick="closeInitiativeModal('modalSearchHelpCustomer');"><i class="fa fa-close"></i>&nbsp;Close</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional" class="d-none">
            <ContentTemplate>
                <asp:Button ID="btnSaveCI" runat="server" OnClick="btnSaveCI_Click" OnClientClick="AGLoading(true);" ClientIDMode="Static" />
                <asp:Button ID="btnSaveCustomer" runat="server" OnClick="btnSaveEffectedCustomer_Click" OnClientClick="AGLoading(true);" ClientIDMode="Static" />

            </ContentTemplate>
        </asp:UpdatePanel>


    </div>

    <script src="<%= Page.ResolveUrl("~/AGFramework/chat/Activity-Chatting.js?vs=20190113") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("~/AGFramework/chat/linkify.js?vs=20190113") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("~/AGapeGalleryFinal/agape-gallery-3.0.js?vs=20190113") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("~/crm/AfterSale/Lib/ServiceCall.js?vs=20190924") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("~/js/FlipClock-master/compiled/flipclock.min.js?vs=20190113") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("~/iris-ws.js?vs=20190113") %>"></script>
    <script async="" src="<%= Page.ResolveUrl("~/Analytics/Analytics.js?vs=20190113") %>"></script>

    <script>
        var wsURL = "<%= ServiceWeb.Service.TriggerService.WS_TRIGGER_RESULT %>";

        // Ws comes from the auto-served '/iris-ws.js'
        var socket = new Ws(wsURL)
        socket.OnConnect(function () {
            socket.Emit("clientJoin", "tokenClientRegisterKey")
            console.log("Status : Connected.");
        });

        socket.OnDisconnect(function () {
            console.log("Status : Disconnected.");
        });

        // read events from the server
        socket.On("ticker", function (msg) {
            checkTrigger(msg);
        });

        function checkTrigger(msg) {
            var data = JSON.parse(msg);
            if (data.TransectionID != null && data.TransectionID != undefined && data.TransectionID != "") {
                if ($("input[type='hidden'][value='" + data.TransectionID + "']").length > 0) {
                    setTimeout(function () {
                        $("#btnSelectIncidentArea").click(); // ใช้ Event เดียวกับตอนเลือก Incident Area เลยยืมปุ่มมาใช้
                    }, 10000);
                }
            }
        }

        $(document).ready(function () {
            var mode = "<%= mode_stage %>";
            if (mode == "<%= agape.lib.constant.ApplicationSession.CREATE_MODE_STRING %>") {
                $('.your-clock').removeClass("d-none");
                var clock = $('.your-clock').FlipClock({
                    clockFace: 'MinuteCounter'
                });
            }

            loadCurentTabView();
        });

        function saveCI() {
            $('#btnSaveCI').click();
        }

        function saveCustomer() {
            $('#btnSaveCustomer').click();

        }

        function submitCheckRequireField() {
            activeRequireField();
            $("#btnSubmitRequireKM").click();
        }
        function CreateKMDetailRefTicketModalClick(obj) {
            if (AGConfirm(obj, "Save Confirm")) {
                AGLoading(true);
                return true;
            }
            return false;
        }
        function activeRequireField() {
            $(".required").prop('required', true);
        }

        function inactiveRequireField() {
            $(".required").prop('required', false);
        }

        function loadcontactDetail() {
            $("#btnDetailCustomerEffect").click();
        }

    </script>
    <ag:ActivitySendMailModal runat="server" id="ActivitySendMailModal" />
</asp:Content>

