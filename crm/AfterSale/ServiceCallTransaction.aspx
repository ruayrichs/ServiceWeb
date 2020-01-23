<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/ServiceTicketMasterPage.master" ValidateRequest="false" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="ServiceCallTransaction.aspx.cs" Inherits="ServiceWeb.crm.AfterSale.ServiceCallTransaction" %>


<%@ Register Src="~/widget/usercontrol/AttachFileUserControl.ascx" TagName="AttachFileUserControl" TagPrefix="sna" %>
<%@ Register Src="~/Accountability/UserControl/ApprovalProcedureControl.ascx" TagPrefix="sna" TagName="ApprovalProcedureControl" %>
<%@ Register Src="~/Accountability/UserControl/ApproveStateGateControl.ascx" TagPrefix="sna" TagName="ApproveStateGateControl" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteCustomer.ascx" TagPrefix="ag" TagName="AutoCompleteCustomer" %>
<%@ Register Src="~/LinkFlowChart/FlowChartDiagramRelationControl.ascx" TagPrefix="ag" TagName="FlowChartDiagramRelationControl" %>
<%@ Register Src="~/UserControl/ChangeLogControl.ascx" TagPrefix="sna" TagName="ChangeLogControl" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEmployee.ascx" TagPrefix="ag" TagName="AutoCompleteEmployee" %>
<%@ Register Src="~/crm/usercontrol/modalAddNewContact.ascx" TagPrefix="ag" TagName="modalAddNewContact" %>
<%@ Register Src="~/widget/usercontrol/AutoCompleteControl.ascx" TagPrefix="ag" TagName="AutoCompleteControl" %>
<%@ Register Src="~/UserControl/ActivitySendMailModal.ascx" TagPrefix="ag" TagName="ActivitySendMailModal" %>
<%@ Register Src="~/UserControl/AGapeGallery/UploadGallery/UploadGallery.ascx" TagPrefix="ag" TagName="UploadGallery" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


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

        #table-material .tt-menu {                      
            bottom: 100% !important;
            top: auto !important;
        }

        #icon-lock-ticket {
            padding: 0.5px 7px !important;
        }
        .box-highlight {
            color: red !important;
            font-weight: 900 !important;
            background-color: yellow !important;
        }
        
 
    </style>
    <script>
        function confirmRating(obj) {
            if (AGConfirm(obj, 'Are you sure you want to review this ticket? (Cannot be edited.)')) {
                $(".btnSave").click();
                return true;
            } else {
                return false;
            }
        }
        function onCommentClick() {
            //console.log("hello onCommentClick");
            var cd = new Date();
            var cdate = cd.getFullYear().toString() + (cd.getMonth() + 1).toString() + cd.getDate().toString();
            var cdatetime = cd.getHours().toString() + cd.getMinutes().toString();

            var cdx = cdate + cdatetime;

            var xd = document.getElementsByClassName("end_date");
            var input = document.getElementById('_txt_docstatusTran').value;
            var datetime;
            var date;
            var time;
            var index;
            var title;
            for (index = 0; index < xd.length; index++) {
                try {
                    // did it whene end not equal -
                    if (xd[index].innerText.toString() !== "-") {
                        datetime = xd[index].innerText.toString().split(' ');
                        //console.log(datetime);
                        date = datetime[0].split('/');
                        date = date[0] + date[1] + date[3];
                        //console.log(date);
                        date = date[2] + date[1] + date[0];
                        time = datetime[1].split(':');
                        time = time[0] + time[1];
                        time = time[0] + time[1];
                        console.log("sys date: " + cdx);
                        console.log("tic date: " + date + time);
                        //console.log(date);
                        //console.log(time);
                        console.log("status" + input);
                        if (input.includes("Close")) {
                            if (parseInt(cdx) > parseInt(date + time)) {
                                console.log("true");
                                xd[index].style.color = "green";
                                title = document.getElementById("ticket_span");
                                title.style.color = "green";
                                title = document.getElementById("ticketName_span");
                                title.style.color = "green";
                            } else {
                                console.log("false");
                            }


                            //} catch (err) {
                            //    console.log("something error");
                            //}
                        }
                        else if (input.includes("Cancel")) {
                            if (parseInt(cdx) > parseInt(date + time)) {
                                console.log("true");
                                xd[index].style.color = "red";
                                title = document.getElementById("ticket_span");
                                title.style.color = "red";
                                title = document.getElementById("ticketName_span");
                                title.style.color = "red";
                            } else {
                                console.log("false");
                            }

                            //} catch (err) {
                            //    console.log("something error");
                            //}
                        }


                    }

                } catch (err) {
                    console.log("something error");
                }

            }

        };


        function webOnLoad() {
            //console.log("hello web onload");
            onCommentClick();
            loadCurentTabView();

            
        };

        //window.onload = webOnLoad();
        $(document).ready(function () { webOnLoad(); })

        var isPageChange = false;
        var TicketType = "<%= (string)Session["SCT_created_doctype_code"] %>";
        var CustomerCode = "<%= (string)Session["SCT_created_cust_code"] %>";

        var AreaCode = "<%= AreaCode %>";
        var FocusOneLinkProfileImage = "<%= ServiceWeb.Service.PublicAuthentication.FocusOneLinkProfileImage %>";
        var EMP_FullNameEN = "<%= FullNameEN %>";

        function loadcontactDetailBySelectedFormServiceCallTransaction() {
            $("#<%= btnContactPersonChange.ClientID %>").click();
        }

        function confirmSaveAddKnowledgeMapping(obj) {
            if (AGConfirm(obj, "Confirm Save Knowledge.")) {
                AGLoading(true);
                return true;
            }
            return false;
        }

        function setModesaveClick(obj) {
            saveClick(obj, '<%= isOBJECT_CHANGE %>');
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
        <div class="pull-right" style="margin-top: 10px; margin-left: 7px;width: 55px;">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" style="position: fixed;right: 16px;z-index:100;">
                <ContentTemplate>
                    <asp:Button Text="" runat="server" CssClass="d-none" OnClientClick="return false;" />
                    <%--อย่าเอาออก ตั้งใว้ดักตอนกด Enter ใน Textbox ไม่อย่างนั้นมันจะไปเข้าปุ่ม Exit--%>

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
                        <asp:Button Text="" runat="server" CssClass="d-none" OnClientClick="return false;" />
                        <%--อย่าเอาออก ตั้งใว้ดักตอนกด Enter ใน Textbox ไม่อย่างนั้นมันจะไปเข้าปุ่ม Save--%>
                        <button type="button" class="d-none btn btn-warning btn-sm mb-1 ticket-allow-editor ticket-allow-editor-everyone" onclick="$(this).next().click();">
                            <i class="fa fa-arrow-circle-left"></i>&nbsp;&nbsp;Back
                        </button>
                        <asp:Button Text="" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" runat="server" ID="btnBackPage"
                            OnClick="btnBackPage_Click" OnClientClick="AGLoading(true);" />
                        <%-- href="ServiceCallFastEntryCriteria.aspx"--%>

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
                             !IsDocTicketStatusClose && !IsDocTicketStatusCancel)
                            { %>
                        <button type="button" class="btn btn-info btn-sm mb-1 ticket-allow-editor ticket-allow-editor-everyone" onclick="$(this).next().click();">
                            <i class="fa fa-refresh"></i>
                            &nbsp;&nbsp;Refresh
                        </button>
                        <asp:Button Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone"
                            ID="brnRefreshData" ClientIDMode="Static" OnClientClick="AGLoading(true);" OnClick="brnRefreshData_Click" />
                        <% } %>
                        <% if (mode_stage == agape.lib.constant.ApplicationSession.CHANGE_MODE_STRING &&
                             !IsDocTicketStatusClose && !IsDocTicketStatusCancel)
                            { %>
                        <button type="button" class="btn btn-primary btn-sm mb-1" onclick="openModalCreateNewTicket();">
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
                        <% if (mode_stage == agape.lib.constant.ApplicationSession.CHANGE_MODE_STRING && !IsDocTicketStatusClose && !IsDocTicketStatusCancel)
                            { %>
                        <button type="button" class="btn btn-info btn-sm mb-1 d-none" onclick="$('#Modal-UpdateStatus').modal('show');">
                            <i class="fa fa-tasks"></i>&nbsp;&nbsp;Update Status
                        </button>
                        <button type="button" class="btn btn-success btn-sm mb-1" onclick="$('#Modal-CloseTicket').modal('show');">
                            <i class="fa fa-check-square-o"></i>&nbsp;&nbsp;Close Ticket
                        </button>
                        <button type="button" class="btn btn-danger btn-sm mb-1" onclick="$(this).next().click();">
                            <i class="fa fa-ban"></i>&nbsp;&nbsp;Cancel
                        </button>
                        <asp:Button Text="Cancel" runat="server" CssClass="d-none" ID="btn_CanceldocTran_v2"
                            OnClientClick="return confirmCancelTicket(this);" OnClick="btnCancelDocTran_click" />
                        <% } %>
                        <%--<asp:Button ID="btn_CanceldocTran" runat="server" CssClass="btn btn-danger btn-sm mb-1" 
                        Visible="false" OnClientClick="AGLoading(true);" OnClick="btnCancelDocTran_click" />--%>
                        <% if (ShowWorkFlowWithoutCreate)
                            { %>
                        <button type="button" class="btn btn-info btn-sm mb-1 ticket-allow-editor-everyone" onclick="showInitiativeModal('modal-workflow');"><i class="fa fa-sitemap"></i>&nbsp;&nbsp;Work Flow</button>
                        <% } %>

                        <% if (mode_stage == agape.lib.constant.ApplicationSession.CHANGE_MODE_STRING && (IsDocTicketStatusClose || IsDocTicketStatusCancel))
                            { %>
                        <button type="button" id="Button1" runat="server" class="btn btn-danger btn-sm mb-1" onclick="$(this).next().click();">
                            <i class="fa fa-refresh"></i>&nbsp;&nbsp;Re-Open Ticket
                        </button>
                        <asp:Button Text="Re-Open Ticket" runat="server" ID="btnReOpenTicket" CssClass="btn btn-warning btn-sm mb-1 d-none"
                            OnClick="btnReOpenTicket_Click" OnClientClick="return confirmReOpenTicket(this);" />
                        <% } %>
                        <% if (mode_stage == agape.lib.constant.ApplicationSession.CHANGE_MODE_STRING && !IsDocTicketStatusCancel)
                            { %>
                        <button type="button" id="btnAddKnowledge" class="btn btn-primary btn-sm mb-1" onclick="$('#<%= btnOpenModalAddKM.ClientID  %>').click();">
                            <i class="fa fa-plus-circle"></i>&nbsp;&nbsp;Add KM
                        </button>

                        <%--BUTTON PRINT--%>
                        <button type="button" id="btnPrint" class="btn btn-primary btn-sm mb-1 d-none" onclick="FunctionPrint().click();">
                            <i class="fa fa-print"></i>&nbsp;&nbsp;Print
                        </button>
                        <script>
                            function FunctionPrint() {
                                window.open("FormPrint.aspx?id=<%= idGen %>&ctp=<%= CtPhone %>");
                            }
                        </script>
                        <%--BUTTON PRINT--%>

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
                        <asp:Button ID="btnRequestDisplayMode" OnClick="btnRequestDisplayMode_Click" ClientIDMode="Static" OnClientClick="AGLoading(true);"
                            Text="" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" runat="server" />
                        <% } %>
                        
                        <asp:Button ID="btnSelectIncidentArea" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" ClientIDMode="Static"
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="row" onload="webOnLoad()">
            <div class="col">
                <div class="card shadow">
                    <div class="card-header">
                        <span id="ticket_span" class="h5 mb-0"><asp:Literal id="ltrTicketType" Text="Ticket Service" runat="server" /> Ticket Service</span>
                        <span id="ticketName_span">
                            <asp:Label Text="" runat="server" ID="lbl_docnumberTran" CssClass="h5 mb-0" />
                        </span>
                        <asp:CheckBox ID="chkMajorIncident" runat="server" Text="Major Incident" CssClass="form-check" Style="float: right;" />
                    </div>
                    <div class="card-body">
                        <nav>
                            <div class="nav nav-tabs" id="nav-tab" role="tablist">
                                <a class="header-tab nav-item nav-link active" id="nav-header-tab" data-toggle="tab" href="#nav-header" role="tab" aria-controls="nav-header" aria-selected="true">Overview</a>
                                <a class="header-tab nav-item nav-link" id="nav-item-tab" data-toggle="tab" href="#nav-item" role="tab" aria-controls="nav-item" aria-selected="false">Configuration Item
                                <asp:UpdatePanel ID="updcountitem" runat="server" UpdateMode="Conditional" class="d-none">
                                    <%-- "d-inline-block --%>
                                    <ContentTemplate>
                                        <label id="lb_menu_count_itemTran" runat="server" class="badge badge-primary"></label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                </a>
                                <a class="header-tab nav-item nav-link" id="nav-sla-tab" data-toggle="tab" href="#nav-sla" role="tab" aria-controls="nav-sla" aria-selected="false" onclick="onCommentClick();">Comment</a>
                                <a class="header-tab nav-item nav-link" id="nav-summary-tab" data-toggle="tab" href="#nav-summary" role="tab" aria-controls="nav-summary" aria-selected="false">Summary</a>
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
                                <%--add Reviews tab--%>
                                
                                    <a id="reviewsTran" class="header-tab nav-item nav-link <%= mode_stage == agape.lib.constant.ApplicationSession.CREATE_MODE_STRING ? " d-none " : "" %>"
                                        data-toggle="tab" href="#nav-reviews" role="tab" aria-controls="nav-reviews" aria-selected="true"
                                        onclick="$('#btn_reviews').click();">Reviews</a>
                                
                            </div>
                        </nav>
                        <br />
                        <div class="tab-content p-0" id="nav-tabContent" style="max-height: calc(100vh - 220px); overflow: hidden auto;">
                            <div class="tab-pane fade show active" id="nav-header" role="tabpanel" aria-labelledby="nav-header-tab">
                                <asp:UpdatePanel ID="panelUpdate" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div>
                                            <div class="d-none">
                                                <label for="input" class="col-lg-2 col-md-2 col-sm-2" style="padding-bottom: 10px;">
                                                    <asp:Label runat="server" ID="labelCompany" Text="บริษัท"></asp:Label>
                                                </label>
                                                <div class="col-lg-4 col-md-4 col-sm-4" style="padding-bottom: 10px;">
                                                    <input id="_txt_companyTran" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static" disabled="disabled" />
                                                </div>
                                            </div>

                                            <div class="form-row">
                                                <div class="form-group col-md-6 col-sm-12">
                                                    <div class="card">
                                                        <div class="card-body card-body-sm">
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
                                                                <div class="form-group col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                                                    <label>Call Back Date</label>
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="tbCallbackDate" runat="server" CssClass="form-control form-control-sm date-picker ticket-allow-editor"></asp:TextBox>
                                                                        <div class="input-group-append">
                                                                            <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                                                    <label>Time</label>
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="tbCallbackTime" runat="server" CssClass="form-control form-control-sm time-picker ticket-allow-editor"></asp:TextBox>
                                                                        <div class="input-group-append">
                                                                            <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    <asp:UpdatePanel ID="udpRefer" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <label>Reference External Ticket</label>
                                                                            <asp:TextBox ID="tbRefer" runat="server" CssClass="form-control form-control-sm ticket-allow-editor"></asp:TextBox>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                                <div class="form-group col-lg-8 col-md-8 col-sm-8 col-xs-12">                                                                    
                                                                    <asp:Label ID="labelCreatedBy" runat="server" Text="Created By"></asp:Label>
                                                                    <asp:TextBox ID="tbCreatedBy" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                                                </div>
                                                                <div class="form-group col-lg-4 col-md-4 col-sm-4 col-xs-12">
                                                                    <label>Created On</label>
                                                                    <asp:TextBox ID="tbCreatedOn" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group col-md-6 col-sm-12">
                                                    <div class="card">
                                                        <div class="card-body card-body-sm">
                                                            <div class="form-row">
                                                                <div class="form-group col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    <asp:Label runat="server" ID="labelCustomer" Text="Client"></asp:Label>
                                                                    <% if (isCritical)
                                                                       { %>
                                                                    <img src="/images/icon/flag-red-512.png" width="25" height="25" style="float: right;">
                                                                    <% } %>
                                                                    <script>
                                                                        function CustomerTranClick() {
                                                                            let ddlid = document.getElementById("_txt_customerTran").value;
                                                                            ddlid = ddlid.split(" : ")[0];
                                                                            let url = "<%= Page.ResolveUrl("~/crm/CustomerProfileDetail.aspx") %>?id=" + ddlid;
                                                                            goToEdit(url);
                                                                        }

                                                                        function goToEdit(url) {
                                                                            var height = document.documentElement.clientHeight;
                                                                            window.open(url, '_blank', 'location=yes,height=' + height + ',width=1200,scrollbars=yes,status=yes');
                                                                        }
                                                                    </script>
                                                                    <a href="javascript:CustomerTranClick()" >
                                                                    <input id="_txt_customerTran" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static"
                                                                        disabled="disabled" />
                                                                    </a>
                                                                </div>
                                                                <div class="form-group col-lg-6 col-md-7 col-sm-12 col-xs-12">
                                                                    <asp:Label runat="server" ID="labelContactPerson" Text="Contact"></asp:Label>
                                                                    <ag:AutoCompleteControl runat="server" id="_ddl_contact_person"
                                                                        CustomViewCode="contact"
                                                                        TODO_FunctionJS="loadcontactDetailBySelectedFormServiceCallTransaction();" CssClass="form-control form-control-sm" />
                                                                    <asp:UpdatePanel ID="udpContactRefresh" UpdateMode="Conditional" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:Button ID="btnContactPersonChange" runat="server" CssClass="d-none" OnClick="btnContactPersonChange_Click"
                                                                                ClientIDMode="Static" />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                                <div class="form-group col-lg-6 col-md-5 col-sm-12 col-xs-12">
                                                                    <label>Responsible Organization</label>
                                                                    <input id="_txt_customertran_responsible_organization" type="text" class="form-control form-control-sm box-highlight"
                                                                        style="color: red !important;font-weight: 900 !important; background-color: yellow !important;" runat="server" 
                                                                        clientidmode="Static" disabled="disabled" />
                                                                </div>
                                                                <div class="form-group col-lg-7 col-md-12 col-sm-7 col-xs-12">
                                                                    <label>Contact E-Mail</label>
                                                                    <asp:TextBox ID="txtContactEmail" Enabled="false" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                </div>
                                                                <div class="form-group col-lg-5 col-md-12 col-sm-5 col-xs-12">
                                                                    <label>Contact Phone</label>
                                                                    <asp:TextBox ID="txtContactPhone" Enabled="false" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                </div>
                                                                <div class="form-group col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    <label>Contact Remark</label>
                                                                    <asp:TextBox ID="txtContactRemark" Enabled="false" runat="server"
                                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group col-md-12 col-sm-12">
                                                    <div class="card">
                                                        <div class="card-body card-body-sm">
                                                            <asp:UpdatePanel ID="udpnSeverity" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <div class="form-row">
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
                                                                        <small style="color: #aaa;">&nbsp;(<span class="Count-MaxLength-Remark"><%= txt_problem_topic.Text.Length%></span>/1000)</small>
                                                                        <asp:TextBox ID="txt_problem_topic" runat="server" CssClass="form-control form-control-sm required ticket-allow-editor"
                                                                            onkeypress="return countMaxLengthRemark(event)" onkeyup="validateMaxLengthRemark(event);"
                                                                            Rows="1" Style="resize: none;" data-maxlength="1000"></asp:TextBox>
                                                                        <asp:HiddenField runat="server" ID="hddOldValue_problem_topic" />
                                                                    </div>

                                                                    <div class="form-group control-max-length">
                                                                        <asp:Label runat="server" ID="labelDescription" Text="Description"></asp:Label>
                                                                        <small style="color: #aaa;">&nbsp;(<span class="Count-MaxLength-Remark"><%= tbEquipmentRemark.Text.Length%></span>/8000)</small>
                                                                        <asp:TextBox ID="tbEquipmentRemark" runat="server" CssClass="form-control form-control-sm ticket-allow-editor txt-ticket-Descript"
                                                                            onkeypress="return countMaxLengthRemark(event)" onkeyup="validateMaxLengthRemark(event);"
                                                                            TextMode="MultiLine" Rows="3" Style="resize: none; height: 250px;" data-maxlength="8000"></asp:TextBox>
                                                                        <asp:Panel ID="galleryLoad" runat="server" CssClass="d-none">
                                                                            <br />
                                                                            <ag:uploadgallery runat="server" id="UploadGallery" MultipleMode="true" />
                                                                            <label>Upload your images or files here.</label>
                                                                        </asp:Panel>                                                                        

                                                                        <asp:HiddenField runat="server" ID="hddOldValue_EquipmentRemark" />
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="">
                                                <asp:Label runat="server" ID="labelDocumentStatus" Text="Ticket Status"></asp:Label>
                                                <input id="_txt_docstatusTran" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static"
                                                    disabled="disabled" />

                                                <asp:Label ID="lbApprovalStatus" runat="server" Text="Wait Approval By"></asp:Label>
                                                <asp:TextBox ID="tbApprovalStatus" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>

                                                <asp:Label ID="lbWorkFlowStats" runat="server" Text="Work Flow Status"></asp:Label>
                                                <asp:TextBox ID="tbWorkFlowStatus" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>

                                                <asp:Label runat="server" ID="labelFiscalYear" Text="Fiscal Year"></asp:Label>
                                                <input id="_txt_fiscalyear" type="text" class="form-control form-control-sm" runat="server" disabled="disabled" />

                                                <asp:Panel runat="server" ID="panelAccountability" CssClass="form-group">
                                                    <asp:Label ID="lbAccountability" runat="server" Text="Accountability" />
                                                    <asp:DropDownList runat="server" ID="ddlAccountability" CssClass="form-control form-control-sm">
                                                    </asp:DropDownList>
                                                </asp:Panel>
                                            </div>

                                            <%--<div class="form-row">
                                            <div class="form-group col-md-2 col-sm-6">
                                                 </div>
                                            <div class="form-group col-md-2 col-sm-6">                                            
                                            </div>
                                            <div class="form-group col-md-3 col-sm-6">
                                            </div>
                                            <div class="form-group col-lg-3 col-md-3 col-sm-6<%= ShowWorkFlowWithoutCreate ? "" : " d-none"%>">
                                                
                                            </div>
                                            <div class="form-group col-lg-2 col-md-2 col-sm-6<%= ShowWorkFlowWithoutCreate ? "" : " d-none"%>">
                                            </div>

                                        </div>
                                        <div class="form-row hide">
                                            <div class="form-group col-md-4 col-sm-6">
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="form-group col-md-4 col-sm-4">
                                            </div>
                                            <div class="form-group col-md-3 col-sm-12">
                                            </div>
                                            <div class="form-group col-md-3 col-sm-12">
                                            </div>
                                            <div class="form-group col-md-2 col-sm-12">
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="form-group col-md-2 col-sm-4">
                                            </div>
                                            <div class="form-group col-md-2 col-sm-4">
                                            </div>
                                            <div class="form-group col-md-3 col-sm-4">
                                            </div>
                                            <div class="form-group col-md-5 col-sm-12">
                                            </div>
                                        </div>--%>


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
                                                <ag:FlowChartDiagramRelationControl runat="server" id="FlowChartDiagramRelationControl"
                                                    RelationType="TICKET" RequiredRelation="false" CssClass_Add="col-md-6 col-sm-12" />

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
                            <div class="tab-pane fade" id="nav-item" role="tabpanel" aria-labelledby="nav-item-tab">

                                <div class="">
                                    <div class="form-row">
                                        <div class="col-lg-6">

                                            <div class="card">
                                                <div class="card-body">
                                                    <div class="form-row">
                                                        <div class="form-group col-sm-12">
                                                            <asp:UpdatePanel ID="udpnEquipment" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <label>Configuration Item No.</label>
                                                                    <asp:DropDownList ID="ddlEquipmentNo" runat="server" CssClass="form-control form-control-sm"
                                                                        onchange="changeEquipment(this);" ClientIDMode="Static">
                                                                    </asp:DropDownList>
                                                                    <div class="d-none">
                                                                        <asp:Button ID="btnEquipmentChange" runat="server" CssClass="d-none"
                                                                            OnClientClick="AGLoading(true);" OnClick="btnEquipmentChange_Click" />
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <asp:UpdatePanel ID="udpnEQDetail" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="form-row">
                                                                <div class="form-group col-sm-6">
                                                                    <label>Family</label>
                                                                    <asp:TextBox ID="tbEquipmentType" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                                                </div>
                                                                <div class="form-group col-sm-6">
                                                                    <label>Class</label>
                                                                    <asp:TextBox ID="tbEquipmentClass" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                                                </div>
                                                                <div class="form-group col-sm-6">
                                                                    <label>Category</label>
                                                                    <asp:TextBox ID="tbEquipmentCategory" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                                                </div>
                                                                <div class="form-group col-sm-6">
                                                                    <label>Serial No.</label>
                                                                    <asp:TextBox ID="tbSerialNo" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-6">
                                            <div class="card">
                                                <div class="card-body">

                                                    <asp:UpdatePanel ID="udpnProblem" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="form-row">
                                                                <div class="form-group col-sm-12">
                                                                    <asp:UpdatePanel ID="udpnOwnerService" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <label>Owner Service</label>
                                                                            <asp:DropDownList ID="ddlOwnerGroupService" CssClass="form-control form-control-sm" runat="server"
                                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlOwnerGroupService_SelectedIndexChanged" onchange="AGLoading(true);">
                                                                            </asp:DropDownList>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                            <div class="form-row">
                                                                <div class="form-group col-sm-6">
                                                                    <label>
                                                                        <asp:Label Text="Problem Group" runat="server" ID="lblEquipmentProblemGroupDesc" />
                                                                    </label>
                                                                    <asp:DropDownList ID="ddlProblemGroup" runat="server" CssClass="form-control form-control-sm" onchange="$(this).next().click();"></asp:DropDownList>
                                                                    <asp:Button ID="btnChangeProblemGroup" runat="server" CssClass="d-none"
                                                                        OnClientClick="AGLoading(true);" OnClick="btnChangeProblemGroup_Click" />
                                                                </div>
                                                                <div class="form-group col-sm-6">
                                                                    <label>
                                                                        <asp:Label Text="Problem Type" runat="server" ID="lblEquipmentProblemTypeDesc" />
                                                                    </label>
                                                                    <asp:DropDownList ID="ddlProblemType" onchange="$(this).next().click();" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                                                                    <asp:Button ID="btnChangeProblemType" runat="server" CssClass="d-none"
                                                                        OnClientClick="AGLoading(true);" OnClick="btnChangeProblemType_Click" />
                                                                </div>
                                                                <div class="form-group col-sm-6">
                                                                    <label>
                                                                        <asp:Label Text="Problem Source" runat="server" ID="lblEquipmentProblemSourceDesc" />
                                                                    </label>
                                                                    <asp:DropDownList ID="ddlProblemSource" onchange="$(this).next().click();" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                                                                    <asp:Button ID="btnChangeProblemSource" runat="server" CssClass="d-none"
                                                                        OnClientClick="AGLoading(true);" OnClick="btnChangeProblemSource_Click" />
                                                                </div>
                                                                <div class="form-group col-sm-6">
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
                                                <%--<asp:UpdatePanel ID="udpUpdateAreaCode" UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <asp:HiddenField runat="server" ID="hddIncidentAreaCode" ClientIDMode="Static" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>--%>
                                            </div>
                                            <script>
                                                $(document).ready(function () {
                                                    //bindHierarchyIncidentTicket();
                                                });
                                            </script>
                                        </div>

                                    </div>
                                    <br />
                                </div>

                                <%-- Contact Mapping Equipment --%>
                                <div class="pt-0">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="card">
                                                <div class="card-header">
                                                    <nav>
                                                        <div class="nav nav-tabs card-header-tabs" id="nav-equipment-tab" role="tablist">
                                                            <!--<a class="nav-item nav-link active" id="nav-sla-tab" data-toggle="tab" href="#nav-sla" role="tab" aria-controls="nav-sla" aria-selected="false">SLA</a>-->
                                                            <%--<a class="nav-item nav-link active" id="nav-contact-tab" data-toggle="tab" href="#nav-contact" role="tab" aria-controls="nav-contact" aria-selected="true">Contact</a>--%>
                                                            <a class="nav-item nav-link active" id="nav-material-tab" data-toggle="tab" href="#nav-material" role="tab" aria-controls="nav-material" aria-selected="true">Material</a>
                                                            <% if (!AlowWorkFlow && mode_stage != agape.lib.constant.ApplicationSession.CREATE_MODE_STRING)
                                                                { %>
                                                            <a class="nav-item nav-link" id="nav-relation-tab" data-toggle="tab" 
                                                                href="#nav-relation" role="tab" aria-controls="nav-relation" aria-selected="false"
                                                                onclick="$('#btnIsLoad_CIRelation').click();">Configuration Item Relation</a>
                                                            <% } %>    
                                                            
                                                                    <asp:Repeater ID="rptHeaderProperty" runat="server">
                                                                                    <ItemTemplate>
                                                                                        <a class="nav-item nav-link" id="nav-<%# Eval("PostingType") %><%# Eval("HeaderCode") %>-tab" data-toggle="tab" href="#nav-<%# Eval("PostingType") %><%# Eval("HeaderCode") %>" role="tab" aria-controls="nav-<%# Eval("PostingType") %><%# Eval("HeaderCode") %>" aria-selected="false"><%# Eval("HeaderDescription") %></a>
                                                                                    </ItemTemplate>
                                                                    </asp:Repeater>

                                                            
                                                        </div>
                                                    </nav>
                                                </div>
                                                <div class="card-body">
                                                    <div class="tab-content" id="nav-equipmentContent">
                                                        <div class="tab-pane fade d-none" id="nav-contact" role="tabpanel" aria-labelledby="nav-contact-tab">
                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpEquipmentMappingContact">
                                                                <ContentTemplate>
                                                                    <div class="table-responsive">
                                                                        <table class="table table-bordered table-striped table-hover table-sm dataTable">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th class="text-nowrap">ชื่อผู้ติดต่อ</th>
                                                                                    <th class="text-nowrap">ประเภทผู้ติดต่อ</th>
                                                                                    <th class="text-nowrap">หมายเลขโทรศัพท์</th>
                                                                                    <th class="text-nowrap">อีเมล์ผู้ติดต่อ</th>
                                                                                    <th class="text-nowrap">ตำแหน่ง</th>
                                                                                    <th>หมายเหตุ</th>
                                                                                </tr>
                                                                            </thead>
                                                                            <asp:Repeater runat="server" ID="rptEquipmentMappingContact">
                                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                        <td class="text-nowrap"><%# Eval("Name1") + " " + Eval("Name2")%></td>
                                                                                        <td class="text-nowrap"><%# displayContantCustomer(Eval("ContactTypeName").ToString())%></td>
                                                                                        <td class="text-nowrap"><%# displayContantCustomer(Eval("PHONENUMBER").ToString())%></td>
                                                                                        <td class="text-nowrap"><%# displayContantCustomer(Eval("EMAIL").ToString())%></td>
                                                                                        <td class="text-nowrap"><%# displayContantCustomer(Eval("POSITION").ToString())%></td>
                                                                                        <td><%# displayContantCustomer(Eval("Remark").ToString())%></td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                            <% if (rptEquipmentMappingContact.Items.Count == 0)
                                                                                { %>
                                                                            <tr>
                                                                                <td colspan="6" style="text-align: center; background-color: lemonchiffon;">ไม่พบข้อมูล
                                                                                </td>
                                                                            </tr>
                                                                            <% } %>
                                                                        </table>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="tab-pane fade show active" id="nav-material" role="tabpanel" aria-labelledby="nav-material-tab">
                                                            <asp:UpdatePanel ID="udpnMaterial" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:HiddenField ID="hddJsonMat" runat="server" ClientIDMode="Static" />
                                                                    <asp:Button ID="btnAddRowMaterial" runat="server" CssClass="d-none" OnClick="btnAddRowMaterial_Click" />
                                                                    <div>
                                                                        <table id="table-material" class="table table-bordered table-striped table-hover table-sm dataTable">
                                                                            <thead>
                                                                                <th class="text-nowrap text-center">#</th>
                                                                                <th>Material</th>
                                                                                <th class="text-nowrap">Price / Unit</th>
                                                                                <th class="text-nowrap">QTY</th>
                                                                                <th class="text-nowrap">UOM</th>
                                                                                <th class="text-nowrap<%= PageEnableEdit ? "" : " d-none" %>"></th>
                                                                            </thead>
                                                                            <tbody>
                                                                                <asp:Repeater ID="rptMaterial" runat="server" OnItemDataBound="rptMaterial_ItemDataBound">
                                                                                    <ItemTemplate>
                                                                                        <tr>
                                                                                            <td class="text-nowrap text-center align-middle">
                                                                                                <%# Convert.ToInt32(Eval("ItemNo")) %>
                                                                                                <asp:HiddenField ID="hdfItemNo" runat="server" Value='<%# Eval("ItemNo") %>' />
                                                                                            </td>
                                                                                            <td>                                                                                                                                                                                                                                                                                                                                                                                                
                                                                                                <asp:TextBox ID="hdfMaterialCode" runat="server" CssClass="hdfMaterialCode d-none" Value='<%# Eval("MaterialCode") %>' />
                                                                                                <asp:TextBox ID="tbMaterial" runat="server" CssClass="form-control form-control-sm ticket-allow-editor"></asp:TextBox>                                                                                                
                                                                                            </td>
                                                                                            <td style="width: 120px;">
                                                                                                <asp:TextBox ID="tbUnitPrice" runat="server" CssClass="form-control form-control-sm text-right ticket-allow-editor"
                                                                                                    Text='<%# Eval("UnitPrice", "{0:N2}") %>' onkeypress="return isNumberKey(event);" onchange="decimalFormatNumber(this, 2);">
                                                                                                </asp:TextBox>
                                                                                            </td>
                                                                                            <td style="width: 100px;">
                                                                                                <asp:TextBox ID="tbQty" runat="server" CssClass="form-control form-control-sm text-right ticket-allow-editor"
                                                                                                    Text='<%# Eval("Qty", "{0:N2}") %>' onkeypress="return isNumberKey(event);" onchange="decimalFormatNumber(this, 2);">
                                                                                                </asp:TextBox>
                                                                                            </td>
                                                                                            <td style="width: 120px;">
                                                                                                <asp:UpdatePanel ID="udpnUOM" runat="server" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:Button ID="btnGetMaterialUOM" runat="server" CssClass="btn-get-uom d-none" OnClick="btnGetMaterialUOM_Click" />
                                                                                                        <asp:DropDownList ID="ddlUom" runat="server" CssClass="form-control form-control-sm ticket-allow-editor"
                                                                                                            DataValueField="UCODE" DataTextField="xDisplay">
                                                                                                        </asp:DropDownList>
                                                                                                        <asp:HiddenField ID="hdfMaterialUOM" runat="server" Value='<%# Eval("UOM") %>' />
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>                                                                                                
                                                                                            </td>
                                                                                            <td class="text-nowrap text-center align-middle<%= PageEnableEdit ? "" : " d-none" %>">
                                                                                                <i class="fa fa-trash-o text-danger c-pointer" title="Delete Row" onclick="materialConfirmDeleteRow(this);"></i>
                                                                                                <asp:Button ID="btnDeleteRowMaterial" runat="server" CssClass="d-none ticket-allow-editor" CommandArgument='<%# Eval("ItemNo") %>' OnClick="btnDeleteRowMaterial_Click" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                </asp:Repeater>
                                                                            </tbody>
                                                                            <tfoot>
                                                                                <tr class="c-pointer bg-info text-white<%= PageEnableEdit ? "" : " d-none" %>" onclick="addMaterialRow();">
                                                                                    <td class="text-nowrap text-center">
                                                                                        <i class="fa fa-plus-square"></i>
                                                                                    </td>
                                                                                    <td colspan="5">
                                                                                        Add Row
                                                                                    </td>
                                                                                </tr>
                                                                            </tfoot>
                                                                        </table>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="tab-pane fade" id="nav-relation" role="tabpanel" aria-labelledby="nav-relation-tab">
                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpIsLoad_CIRelation">
                                                                <ContentTemplate>
                                                                    <asp:CheckBox Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" ID="chkIsLoad_CIRelation" />
                                                                    <asp:Button Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" ID="btnIsLoad_CIRelation"
                                                                        data-tabload="nav-Date-Time-log" ClientIDMode="Static"
                                                                        OnClientClick="AGLoading(true);" OnClick="btnIsLoad_CIRelation_Click"/>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>

                                                            <asp:UpdatePanel ID="udpnEquipmentRelation" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <div class="table-responsive">
                                                                        <table id="table-eq-relation" class="table table-bordered table-striped table-hover table-sm dataTable nowrap">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th>Configuration Item Parent</th>
                                                                                    <th>Relation</th>
                                                                                    <th>Configuration Item Child</th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                                <asp:Repeater ID="rptEquipmentRelation" runat="server">
                                                                                    <ItemTemplate>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <%# Eval("parent")%>
                                                                                                <button type="button" class="btn btn-primary btn-sm pull-right <%# string.IsNullOrEmpty(Eval("parentCode").ToString()) ? " d-none" : ""%>"
                                                                                                    onclick="openModalCreateNewTicket('<%# Eval("parentCode")%>');">
                                                                                                    Create Ticket</button>
                                                                                            </td>
                                                                                            <td>
                                                                                                <%# Eval("relation")%>
                                                                                            </td>
                                                                                            <td>
                                                                                                <%# Eval("child")%>
                                                                                                <button type="button" class="btn btn-primary btn-sm pull-right <%# string.IsNullOrEmpty(Eval("childCode").ToString()) ? " d-none" : ""%>"
                                                                                                    onclick="openModalCreateNewTicket('<%# Eval("childCode")%>');">
                                                                                                    Create Ticket</button>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                </asp:Repeater>
                                                                                <% if (rptEquipmentRelation.Items.Count == 0)
                                                                                    { %>
                                                                                <tr>
                                                                                    <td colspan="3" style="text-align: center; background-color: lemonchiffon;">ไม่พบข้อมูล
                                                                                    </td>
                                                                                </tr>
                                                                                <% } %>
                                                                            </tbody>
                                                                        </table>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>

                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:HiddenField ID="hddPostHeader" runat="server" ClientIDMode="Static" Value=''/>
                                                                <asp:Button ID="btnNewRowPropertyItem" runat="server" CssClass="d-none" OnClick="btnAddNewRowPropertyValueItem_click" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>

                                                        <asp:Repeater ID="rptHeaderPropertyDeatail" runat="server">
                                                                    <ItemTemplate>
                                                                        <div class="tab-pane fade " id="nav-<%# Eval("PostingType") %><%# Eval("HeaderCode") %>" role="tabpanel" aria-labelledby="nav-<%# Eval("PostingType") %><%# Eval("HeaderCode") %>-tab">
                                                                            <asp:UpdatePanel ID="udpPropertyDetail" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:HiddenField ID="hddPostHeaderInRpt" runat="server"  Value='<%# String.Concat(Eval("PostingType"),Eval("HeaderCode") ) %>'/>
                                                                                    <asp:HiddenField ID="hddHeaderCodeInRpt" runat="server"  Value='<%# Eval("HeaderCode") %>'/>
                                                                                    <asp:HiddenField ID="hddPostigTypeInRpt" runat="server"  Value='<%# Eval("PostingType") %>'/>
                                                                                    <asp:HiddenField ID="hddHeaderDescInRpt" runat="server"  Value='<%# Eval("HeaderDescription") %>'/>
                                                                                    <div>
                                                                                        <table id="table-<%# Eval("PostingType") %><%# Eval("HeaderCode") %>" class="table table-bordered table-striped table-hover table-sm dataTable">
                                                                                            <thead>
                                                                                                <th class="text-nowrap text-center">#</th>
                                                                                                <th>Value</th>
                                                                                
                                                                                            </thead>
                                                                                            <tbody>
                                                                                                <asp:Repeater ID="rptPropertyItem" runat="server" >
                                                                                                    <ItemTemplate>
                                                                                                        <tr>
                                                                                                            <td class="text-nowrap text-center align-middle">
                                                                                                                <%# Convert.ToInt32(Eval("ItemNo")) %>
                                                                                                                <asp:HiddenField ID="hdfItemNo" runat="server" Value='<%# Eval("ItemNo") %>' />
                                                                                                            </td>
                                                                                                            <td>                                                                                                                                                                                                                                                                                                                                                                                                                                  
                                                                                                                <asp:TextBox ID="txtValue" runat="server" CssClass="form-control form-control-sm ticket-allow-editor" Text='<%# Eval("Value") %>'></asp:TextBox>                                                                                                
                                                                                                            </td>
                                                                                           
                                                                                                            <td class="text-nowrap text-center align-middle<%= PageEnableEdit ? "" : " d-none" %>">
                                                                                                                <i class="fa fa-trash-o text-danger c-pointer" title="Delete Row" onclick="deleteRowPropertyValue('<%# Eval("PostingType") %><%# Eval("HeaderCode") %>');propertyValueConfirmDeleteRow(this);"></i>
                                                                                                                <asp:Button ID="Button4" runat="server" CssClass="d-none ticket-allow-editor" CommandArgument='<%# Eval("ItemNo") %>' OnClick="btnDeleteRowPropertyValue_click"    />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </ItemTemplate>
                                                                                                </asp:Repeater>
                                                                                            </tbody>
                                                                                            <tfoot>
                                                                                                <tr class="c-pointer bg-info text-white<%= PageEnableEdit ? "" : " d-none" %>" onclick="addNewRowPropertyValue('<%# Eval("PostingType") %><%# Eval("HeaderCode") %>');">
                                                                                                    <td class="text-nowrap text-center">
                                                                                                        <i class="fa fa-plus-square"></i>
                                                                                                    </td>
                                                                                                    <td colspan="5">
                                                                                                        Add Row
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tfoot>
                                                                                        </table>
                                                                                    </div>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </div>
                                                                    </ItemTemplate>
                                                        </asp:Repeater>
                                                       


                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="tab-pane fade" id="nav-summary" role="tabpanel" aria-labelledby="nav-summary-tab">
                                <div class="pb-0">

                                    <div class="form-group">
                                        <label>Affect SLA</label>
                                        <asp:DropDownList ID="ddlAffectSLA" runat="server" CssClass="form-control form-control-sm ticket-allow-editor" ClientIDMode="Static">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Affect SLA" Value="True"></asp:ListItem>
                                            <asp:ListItem Text="Not Affect SLA" Value="False"></asp:ListItem>
                                        </asp:DropDownList>
                                        <%--<asp:HiddenField runat="server" ID="hddOldValue_AffectSLA" />--%>
                                    </div>

                                    <div class="form-group control-max-length">
                                        <label>Summary Problem</label>
                                        <small style="color: #aaa;">&nbsp;(<span class="Count-MaxLength-Remark"><%= tbSummaryProblem.Text.Length %></span>/500)</small>
                                        <asp:TextBox ID="tbSummaryProblem" ClientIDMode="Static" runat="server" CssClass="form-control form-control-sm ticket-allow-editor"
                                            onkeypress="return countMaxLengthRemark(event)" onkeyup="validateMaxLengthRemark(event);"
                                            TextMode="MultiLine" Rows="2" Style="resize: none;" data-maxlength="500"></asp:TextBox>
                                        <%--<asp:HiddenField runat="server" ID="hddOldValue_SummaryProblem" />--%>
                                    </div>

                                    <div class="form-group control-max-length">
                                        <label>Summary Cause</label>
                                        <small style="color: #aaa;">&nbsp;(<span class="Count-MaxLength-Remark"><%= tbSummaryCause.Text.Length %></span>/500)</small>
                                        <asp:TextBox ID="tbSummaryCause" ClientIDMode="Static" runat="server" CssClass="form-control form-control-sm ticket-allow-editor"
                                            onkeypress="return countMaxLengthRemark(event)" onkeyup="validateMaxLengthRemark(event);"
                                            TextMode="MultiLine" Rows="2" Style="resize: none;" data-maxlength="500"></asp:TextBox>
                                        <%--<asp:HiddenField runat="server" ID="hddOldValue_SummaryCause" />--%>
                                    </div>

                                    <div class="form-group control-max-length">
                                        <label>Summary Resolution</label>
                                        <small style="color: #aaa;">&nbsp;(<span class="Count-MaxLength-Remark"><%= tbSummaryResolution.Text.Length %></span>/500)</small>
                                        <asp:TextBox ID="tbSummaryResolution" runat="server" ClientIDMode="Static" CssClass="form-control form-control-sm ticket-allow-editor"
                                            onkeypress="return countMaxLengthRemark(event)" onkeyup="validateMaxLengthRemark(event);"
                                            TextMode="MultiLine" Rows="2" Style="resize: none;" data-maxlength="500"></asp:TextBox>
                                        <%--<asp:HiddenField runat="server" ID="hddOldValue_SummaryResolution" />--%>
                                    </div>

                                </div>
                            </div>

                            <div class="tab-pane fade" id="nav-sla" role="tabpanel" aria-labelledby="nav-sla-tab">
                                <div class="carousel-inner">
                                    <div class="item active">

                                        <asp:UpdatePanel ID="updPerson" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                            <ContentTemplate>

                                                <asp:Label ID="lbTierName" runat="server" CssClass="page-header mb-2"></asp:Label>

                                                <asp:Repeater ID="rptOperation" runat="server" OnItemDataBound="rptOperation_OnItemDataBound">
                                                    <ItemTemplate>
                                                        <div class="mat-box" style='<%# styleMatBoxHasActivity(Eval("AOBJECTLINK").ToString())%>'>
                                                            <div class="form-row">
                                                                <asp:HiddenField ID="hddSLAGroup" runat="server" Value='<%# Eval("TierGroupCode")%>'></asp:HiddenField>
                                                                <asp:HiddenField ID="hddTierCode" runat="server" Value='<%# Eval("TierCode")%>'></asp:HiddenField>
                                                                <asp:HiddenField ID="hddTierCodeNext" runat="server" Value='<%# GetNextTierCode(Container.ItemIndex)%>'></asp:HiddenField>
                                                                <asp:HiddenField ID="hddTier" runat="server" Value='<%# Eval("Tier")%>'></asp:HiddenField>
                                                                <asp:HiddenField ID="hddTierNext" runat="server" Value='<%# GetNextTier(Container.ItemIndex)%>'></asp:HiddenField>
                                                                <asp:HiddenField ID="hddRoleNext" runat="server" Value='<%# GetNextRole(Container.ItemIndex)%>'></asp:HiddenField>
                                                                <asp:HiddenField ID="hddHeaderStatus" runat="server" Value='<%# Eval("HeaderStatus")%>'></asp:HiddenField>
                                                                <asp:HiddenField ID="hhdAOBJECTLINK" Value='<%# Eval("AOBJECTLINK")%>' runat="server" />
                                                                <asp:HiddenField ID="hddOwnerService_Select" Value='<%# Eval("OwnerService_Select")%>' runat="server" />
                                                                <asp:HiddenField ID="hddSLAGroupCode_Select" Value='<%# Eval("SLAGroupCode_Select")%>' runat="server" />
                                                                <div class="col-md-12">
                                                                    <div class="row">
                                                                        <div class="col-md-6" style="color: #4f7ea7;">
                                                                            <asp:Label ID="lbTierLevelName" runat="server" CssClass="btn btn-sm btn-outline" Style="cursor: default; padding-left: 0;"><strong>หน่วยงานที่รับผิดชอบ</strong> : <%# Eval("TierDescription")%></asp:Label>
                                                                        </div>
                                                                        <div class="col-md-6 text-right">
                                                                            <button type="button" class="btn btn-info btn-sm ticket-allow-editor ticket-allow-editor-everyone <%# showButtonCustomMail(Eval("AOBJECTLINK").ToString()) ? "" : "d-none" %>"
                                                                                onclick="sendCustomEmail('Inform Link : (บริษัท) (Site) | (Circuit Carrier)', '<%= FullNameEN %>')">
                                                                                Send Mail</button>
                                                                            <asp:Button ID="btnAssignWork" runat="server" CssClass="btn btn-success btn-sm" Text="Start" OnClick="btnAssignWork_Click" OnClientClick="AGLoading(true);" />
                                                                            <asp:Button ID="btnTransfer" runat="server" CssClass="btn btn-warning btn-sm AUTH_MODIFY" Text="Transfer" OnClick="btnTransfer_Click" OnClientClick="AGLoading(true);" />
                                                                            <asp:Button ID="btnForwardWork" runat="server" CssClass="btn btn-primary btn-sm AUTH_MODIFY" Text="Escalate" OnClick="btnForwardWork_Click" />
                                                                            <asp:Button ID="btnCloseWork" runat="server" CssClass="btn btn-success btn-sm AUTH_MODIFY" Text="Resolve" OnClick="btnCloseWork_Click" OnClientClick="return resolveTicketClick(this);" />
                                                                        </div>
                                                                    </div>
                                                                    <hr style="margin-top: 15px; margin-bottom: 10px;" />
                                                                </div>
                                                                <div class="col-md-12" style="margin-bottom: 15px;">
                                                                    <div class="row">
                                                                        <asp:Label ID="lbJobdescription" runat="server" CssClass="col-md-12"><strong><span class="text-primary"><%# Eval("Subject").ToString() == "" ? "ยังไม่ได้เปิดงาน" : Eval("Subject")%></span></strong></asp:Label>
                                                                        <%--<asp:Label ID="lbJobdescription" runat="server" CssClass="col-md-12"><strong>ชื่องาน : <span class="text-primary"><%# Eval("Subject").ToString() == "" ? "ยังไม่ได้เปิดงาน" : Eval("Subject")%></span></strong></asp:Label>--%>
                                                                        <asp:Label ID="lbRemarks" runat="server" CssClass="col-md-12"><strong>รายละเอียด : </strong><%# string.IsNullOrEmpty(Eval("AOBJECTLINK").ToString()) ? "-" : (Eval("Remark").ToString() == "" ? "ไม่ระบุรายละเอียด" : Eval("Remark"))%></asp:Label>
                                                                        <span class="col-md-12"><strong>Violation Time : </strong><%# ConvertToTime(Eval("Resolution").ToString())%></span>
                                                                        <span class="col-md-12"><strong>Start Date : </strong><%# DisplayTicketDateTime(Eval("AOBJECTLINK").ToString(), ("StartDateTime").ToString())%></span>
                                                                        <span class="col-md-12"><strong>End Date : </strong><span id="end_date" class="end_date"><%# DisplayTicketDateTime(Eval("AOBJECTLINK").ToString(), Eval("EndDateTime").ToString())%></span></span>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div>
                                                                        <label>Assignee</label>
                                                                    </div>
                                                                    <div class="mat-box" style="overflow-y: auto; background: aliceblue; border-left: 3px solid #4f7ea7;">
                                                                        <asp:Panel ID="PanelShowMain" runat="server" CssClass="row">
                                                                            <asp:Repeater ID="rptMainDelegate" runat="server">
                                                                                <ItemTemplate>
                                                                                    <asp:HiddenField runat="server" ID="hddMainLinkID" Value='<%# Eval("EmployeeCode")%>' />
                                                                                    <div class="col-md-2">
                                                                                        <div class="img-box-ini-style" style="float: none; width: 38px; height: 38px; border-radius: 0; background-image: url(<%# ServiceWeb.Service.UserImageService.getImgProfile(Eval("EmployeeCode").ToString()).Image_64 %>), url(/images/user.png);"></div>
                                                                                    </div>
                                                                                    <div class="col-md-10">
                                                                                        <div class="one-line" style="font-weight: bold;">
                                                                                            <asp:Label ID="lbMainDelegate" runat="server" Title='<%# Eval("fullname")%>' Text='<%# Eval("fullname")%>'></asp:Label>
                                                                                        </div>
                                                                                        <div class="one-line">
                                                                                            <asp:Label ID="lbMainCharacter" runat="server" Title='<%# Eval("HierarchyDesc")%>' Text='<%# Eval("HierarchyDesc")%>'></asp:Label>
                                                                                        </div>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                        </asp:Panel>
                                                                        <asp:Panel ID="PanelHideMain" runat="server" CssClass="row">
                                                                            <div class="col-md-2">
                                                                                <div class="img-box-ini-style" style="float: none; width: 38px; height: 38px; border-radius: 0; background-image: url('<%= Page.ResolveUrl("~") %>images/user.png');"></div>
                                                                            </div>
                                                                            <div class="col-md-10">
                                                                                <div class="one-line" style="font-weight: bold;">
                                                                                    <asp:Label ID="Label1" runat="server" Text='ไม่ได้ระบุ'></asp:Label>
                                                                                </div>
                                                                            </div>
                                                                        </asp:Panel>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-8">
                                                                    <div>
                                                                        <label>Other Assignee</label>
                                                                    </div>
                                                                    <div class="mat-box" style="overflow-y: auto; background: lightgoldenrodyellow; border-left: 3px solid rgba(220, 220, 10, 0.45);">
                                                                        <asp:Panel ID="PanelShowOther" runat="server" CssClass="row">
                                                                            <asp:Repeater ID="rptOtherDelegate" runat="server">
                                                                                <ItemTemplate>
                                                                                    <div class="col-md-4 overview-init-owner <%# Container.ItemIndex < 3 ? "" : " d-none d-none-default " %>" style="display: inline-block; margin-bottom: 5px;">
                                                                                        <asp:HiddenField runat="server" ID="hddParLinkID" Value='<%# Eval("EmployeeCode")%>' />
                                                                                        <div class="row">
                                                                                            <div class="col-md-3">
                                                                                                <div class="img-box-ini-style" style="float: none; width: 38px; height: 38px; border-radius: 0; background-image: url(<%# ServiceWeb.Service.UserImageService.getImgProfile(Eval("EmployeeCode").ToString()).Image_64 %>), url(/images/user.png);"></div>
                                                                                            </div>
                                                                                            <div class="col-md-9">
                                                                                                <div class="one-line" style="font-weight: bold;">
                                                                                                    <asp:Label ID="lbOtherDelegate" runat="server" Title='<%# Eval("fullname")%>' Text='<%# Eval("fullname")%>'></asp:Label>
                                                                                                </div>
                                                                                                <div class="one-line">
                                                                                                    <asp:Label ID="lbOtherCharacter" runat="server" Title='<%# Eval("HierarchyDesc")%>' Text='<%# Eval("HierarchyDesc")%>'></asp:Label>
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
                                                                        </asp:Panel>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <asp:Panel runat="server" ID="panelFeedActivityComment" CssClass="feed-activity-command"
                                                                ClientIDMode="Static" Style='<%# "display:" + (string.IsNullOrEmpty(Eval("AOBJECTLINK").ToString()) ? "none": "") + ";" %>'>
                                                                <asp:HiddenField runat="server" ID="hddHidePostRemark" Value="false" />
                                                                <div class="btn-OpenChatBox btn-OpenChatBox-<%# Eval("AOBJECTLINK")%>" style="cursor: pointer;" onclick="OpenChatBox(this,'<%# Eval("AOBJECTLINK")%>');">
                                                                    <i class="fa fa-comments fa-fw"></i>
                                                                    <b>ความคิดเห็น&nbsp;<span class="count-remark"><%# Convert.ToInt32(Eval("xCountRemark")) > 0 ? "(" + Eval("xCountRemark") + " ความคิดเห็น)" : ""%></span></b>
                                                                </div>
                                                                <div class="btn-CloseChatBox-<%# Eval("AOBJECTLINK") %>" style="cursor: pointer; display: none;" onclick="CloseChatBox(this);">
                                                                    <i class="fa fa-comments fa-fw"></i>
                                                                    <b>ซ่อนความคิดเห็น</b>
                                                                </div>
                                                            </asp:Panel>
                                                            <div class="ag-remarker"></div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>

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
                                    <sna:AttachFileUserControl ID="AttachFileUserControl" runat="server"></sna:AttachFileUserControl>
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
                                        <%--<div style="display: none;" runat="server" id="divJsonListDataKnowledgeManagement" clientidmode="Static">[]</div>--%>
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
                                <%--<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpIsLoad_">
                                    <ContentTemplate>
                                        <asp:CheckBox Text="" runat="server" CssClass="d-none" ID="chkIsLoad_" />
                                        <asp:Button Text="" runat="server" CssClass="d-none" ID="btnIsLoad_"
                                            data-tabload="nav-changelog" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>

                                <sna:ChangeLogControl id="TicketChangeLog" runat="server" />
                            </div>
                             <%--add Reviews coffee--%>
                            <div class="tab-pane fade" id="nav-reviews" role="tabpanel" aria-labelledby="reviewsTran">
                                <style>
                                    .blankstar {
                                        background-image: url('<%= Page.ResolveUrl("~") %>images/EmptyStar.png');
                                        /*background-color: black;*/
                                        width: 32px;
                                        height: 32px;
                                    }

                                    .waitingstar {
                                        background-image: url('<%= Page.ResolveUrl("~") %>images/SavedStar.png');
                                        /*background-color: red;*/
                                        width: 32px;
                                        height: 32px;
                                    }

                                    .shiningstar {
                                        background-image: url('<%= Page.ResolveUrl("~") %>images/FilledStar.png');
                                        /*background-color: green;*/
                                        width: 32px;
                                        height: 32px;
                                    }
                                </style>
                                <asp:HiddenField ID="hddDataRating_TicketCode" runat="server" ClientIDMode="Inherit" EnableViewState="false" />
                                <asp:HiddenField ID="hddDataRating_EmpCode" runat="server" ClientIDMode="Inherit" EnableViewState="false" />
                                <asp:UpdatePanel ID="udp_reviews_detail" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                     
                                       <span class="text-info"><h3>Work Rating</h3></span>
                                        <cc1:Rating ID="Rating1" runat="server" StarCssClass="blankstar"
                                            WaitingStarCssClass="waitingstar" FilledStarCssClass="shiningstar"
                                            EmptyStarCssClass="blankstar" OnChanged="OnRatingChanged">
                                        </cc1:Rating>
                                         <p class="pt-3"></p>
                                        <p class="pt-3">
                                        <asp:TextBox ID="tbCommentforReviews"  rows="5" TextMode="multiline" runat="server" Enabled="true" CssClass="form-control form-control-sm ticket-allow-editor-everyone ticket-allow-editor" />
                                        </p>
                                         
                                        <asp:Button ID="btnSubmitReviews" 
                                            runat="server" OnClick="btnSubmitReviews_Click" 
                                            CssClass="btn btn-info btn-sm mb-1 ticket-allow-editor ticket-allow-editor-everyone"
                                            Text="Submit"  
                                            ClientIDMode="Static"
                                            OnClientClick="return confirmRating(this); AGLoading(true);"
                                            Width="10%"/>
                                         
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpIsLoad_reviews">
                                    <ContentTemplate>
                                        <asp:CheckBox Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" ID="chkIsLoad_reviews" />
                                        <asp:Button Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone" ID="btn_reviews"
                                            data-tabload="nav-reviews" ClientIDMode="Static"
                                            OnClientClick="AGLoading(true);" OnClick="btn_reviews_Click"/>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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
                                                    <sna:ApproveStateGateControl runat="server" id="ApproveStateGateControl" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <sna:ApprovalProcedureControl runat="server" id="ApprovalProcedureControl" />
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
                                                    <ag:AutoCompleteCustomer ID="CustomerSelect" runat="server" NotAutoBindComplete="true"
                                                        CssClass="form-control form-control-sm required-popup ticket-allow-editor">
                                                    </ag:AutoCompleteCustomer>
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

        <%--transfer--%>
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
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel3">
                                                <ContentTemplate>
                                                    <label>SLA Group</label>
                                                    <asp:DropDownList ID="ddlTransfer_SLAGroup" CssClass="form-control form-control-sm"
                                                        runat="server" OnSelectedIndexChanged="ddlTransfer_OwnerService_SelectedIndexChanged" onchange="AGLoading(true);" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="col-lg-6">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <label>Owner Service</label>
                                                    <asp:DropDownList ID="ddlTransfer_OwnerService" CssClass="form-control form-control-sm"
                                                        runat="server" OnSelectedIndexChanged="ddlTransfer_OwnerService_SelectedIndexChanged" onchange="AGLoading(true);" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-6">
                                            <label>Select Employee</label>
                                            <ag:AutoCompleteEmployee runat="server" id="AutoCompleteEmployee"
                                                CssClass="form-control form-control-sm ticket-allow-editor" />
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
                                                        <asp:HiddenField ID="hddTransfer_TaskName" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddTransfer_SLAGroup" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddTransfer_SLAGroupName" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddTransfer_TierCode" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddTransfer_Tier" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddTransfer_AOBJECTLINK" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddTransfer_ListEMPCode" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddTransfer_OwnerService" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddTransfer_OwnerServiceName" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddTransfer_resolutionTimes" runat="server" ClientIDMode="Static" Value="0" />
                                                        <%--<table id="table-search-EmployeeParticipant" class="table table-bordered table-striped table-hover table-sm">
                                                    <asp:HiddenField ID="hddTransfer_" runat="server" ClientIDMode="Static" />
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

                                                        <table id="table-TransferParticipant" class="table table-bordered table-striped table-hover table-sm">
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
                                    <span class="water-button" onclick="$(this).next().click();"><i class="fa fa-file-o"></i>&nbsp;Save</span>
                                    <asp:Button ID="btnTransferAddParticipant" CssClass="d-none" OnClick="btnTransferAddParticipant_Click" OnClientClick="return prepareDateForSaveEmpParticipant(this, 'TransferParticipant');" Text="save" runat="server" />
                                    <a class="water-button" onclick="closeInitiativeModal('modal-TransferAddParticipant');"><i class="fa fa-close"></i>&nbsp;Close</a>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <%--Escalate--%>
        <div class="initiative-model-control-slide-panel" id="modal-EscalateSetParticipant">
            <div class="initiative-model-control-body-content z-depth-3">
                <div>
                    <div class="initiative-model-control-header">
                        <div class="mat-box-initaive-control">
                            <div class="pull-right">
                                <i class="fa fa-close hand" onclick="closeInitiativeModal('modal-EscalateSetParticipant');"></i>
                            </div>
                            <div class="one-line">
                                <label class="text-warning">
                                    Escalate Ticket.
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
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpEscalate_SLAGroup">
                                                <ContentTemplate>
                                                    <label>SLA Group</label>
                                                    <asp:DropDownList ID="ddlEscalate_SLAGroup" CssClass="form-control form-control-sm"
                                                        runat="server" OnSelectedIndexChanged="ddlEscalate_OwnerService_SelectedIndexChanged" onchange="AGLoading(true);" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="col-lg-6">
                                            <asp:UpdatePanel ID="udpEscalate_OwnerService" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <label>Owner Service</label>
                                                    <asp:DropDownList ID="ddlEscalate_OwnerService" CssClass="form-control form-control-sm"
                                                        runat="server" OnSelectedIndexChanged="ddlEscalate_OwnerService_SelectedIndexChanged" onchange="AGLoading(true);" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-6">
                                            <label>Select Employee</label>
                                            <ag:AutoCompleteEmployee runat="server" id="AutoCompleteEmployee_Escalate"
                                                CssClass="form-control form-control-sm ticket-allow-editor" />
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
                                                <asp:UpdatePanel ID="udpEscalateSetParticipant" UpdateMode="Conditional" runat="server">
                                                    <ContentTemplate>
                                                        <asp:HiddenField ID="hddEscalate_TaskName" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddEscalate_SLAGroup" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddEscalate_SLAGroupName" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddEscalate_TierCode" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddEscalate_TierNext" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddEscalate_Tier" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddEscalate_AOBJECTLINK" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hddEscalate_ListEMPCode" runat="server" ClientIDMode="Static" />

                                                        <table id="table-EscalateSetParticipant" class="table table-bordered table-striped table-hover table-sm">
                                                            <tr>
                                                                <th class="nowrap" style="width: 45px;">Main</th>
                                                                <th class="nowrap" style="width: 80px;">Image</th>
                                                                <th class="nowrap">Employee Code</th>
                                                                <th>Employee Name</th>
                                                                <th class="nowrap" style="width: 40px;">Del</th>
                                                            </tr>
                                                            <asp:Repeater runat="server" ID="rptEscalateSetParticipant">
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
                            <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <span class="water-button" onclick="$(this).next().click();"><i class="fa fa-file-o"></i>&nbsp;Save</span>
                                    <asp:Button ID="btnEscalateSetParticipant" CssClass="d-none" OnClick="btnEscalateSetParticipant_Click" OnClientClick="return prepareDateForSaveEmpParticipant(this, 'EscalateSetParticipant');" Text="save" runat="server" />
                                    <a class="water-button" onclick="closeInitiativeModal('modal-EscalateSetParticipant');"><i class="fa fa-close"></i>&nbsp;Close</a>
                                </ContentTemplate>
                            </asp:UpdatePanel>
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
                                    <span class="water-button" onclick="inactiveRequireField();$(this).next().click();"><i class="fa fa-file-o"></i>&nbsp;Save</span>
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

        <%--modal Create Contact--%>
        <ag:modalAddNewContact runat="server" id="modalAddNewContact" />

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
                            <span class="water-button" onclick="submitCheckRequireField();"><i class="fa fa-file-o"></i>&nbsp;Save</span>
                            <a class="water-button" onclick="inactiveRequireField();closeInitiativeModal('modal-CreateDataKM');"><i class="fa fa-close"></i>&nbsp;Close</a>
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
                                <button type="button" class="btn btn-default mb-1" id="btn-close-modal-close-ticket" data-dismiss="modal">
                                    <i class="fa fa-times fa-fw"></i>
                                    Close
                                </button>
                                <button type="button"  class="btn btn-success mb-1" onclick="return clickCloseWork(this);">
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
                    <asp:Button Text="" runat="server" ID="btnRebindWorkFlow" ClientIDMode="Static"
                        OnClick="btnRebindWorkFlow_Click" OnClientClick="AGLoading(true);" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <script src="<%= Page.ResolveUrl("~/AGFramework/chat/Activity-Chatting.js?vs=20190113") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("~/AGFramework/chat/linkify.js?vs=20190113") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("~/AGapeGalleryFinal/agape-gallery-3.0.js?vs=20190113") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("~/crm/AfterSale/Lib/ServiceCall.js?vs=20190925") %>" type="text/javascript"></script>
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
                var datasTransection = data.TransectionID.split("|");
                if (datasTransection.length == 2) {
                    if ("<%= ServiceWeb.Service.TriggerService.TRIGGER_ACTION_TICKET_UPDATE_COMMENT %>" == datasTransection[0]) {
                        //OpenChatBox($(".btn-OpenChatBox-" + datasTransection[1])[0], datasTransection[1]);
                        triggerActivityRemark(datasTransection[1]);
                    }

                    if ("<%= ServiceWeb.Service.TriggerService.TRIGGER_ACTION_TICKET_ESCALATE_MANUALLY %>" == datasTransection[0]) {
                        if ($("input[type='hidden'][value='" + datasTransection[1] + "']").length > 0) {
                            setTimeout(function () {
                                closeInitiativeModal('modal-TransferAddParticipant');
                                closeInitiativeModal('modal-EscalateSetParticipant');
                                $("#btnSelectIncidentArea").click(); // ใช้ Event เดียวกับตอนเลือก Incident Area เลยยืมปุ่มมาใช้
                            }, 1000);
                        }
                    }
                } else {
                    if ($("input[type='hidden'][value='" + data.TransectionID + "']").length > 0) {
                        setTimeout(function () {
                            $("#btnSelectIncidentArea").click(); // ใช้ Event เดียวกับตอนเลือก Incident Area เลยยืมปุ่มมาใช้
                        }, 1000);
                    }
                }
            }
        }


        function triggerActivityRemark(roomCode) {
            if ($(".new-message-trigger[data-new-message-trigger-aobjectlink='" + roomCode + "']").length > 0) {
                $(".new-message-trigger[data-new-message-trigger-aobjectlink='" + roomCode + "']").click();
                return true;
            }
            return false;
        }

        $(document).ready(function () {
            var mode = "<%= mode_stage %>";
            if (mode == "<%= agape.lib.constant.ApplicationSession.CREATE_MODE_STRING %>") {
                $('.your-clock').removeClass("d-none");
                var clock = $('.your-clock').FlipClock({
                    clockFace: 'MinuteCounter'
                });
            }
        });

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

        function addMaterialRow() {
            AGLoading(true);            
            $("#<%= btnAddRowMaterial.ClientID %>").click();
        }
        function addNewRowPropertyValue(code) {
            AGLoading(true);
            $("#<%=hddPostHeader.ClientID %>").val(code);
            $("#<%= btnNewRowPropertyItem.ClientID %>").click();
        }

        function deleteRowPropertyValue(code) {
            $("#<%=hddPostHeader.ClientID %>").val(code);
        }
        function getDataSourceMaterial(keySearch) {
            var dataSource = [];
            var dataMat = JSON.parse($("#hddJsonMat").val());

            for (var i = 0; (dataSource.length < 1000 && dataMat.length > i) ; i++) {
                var data = dataMat[i];
                if (keySearch && !(data.code.indexOf(keySearch) < 0 || data.desc.indexOf(keySearch) < 0)) {
                    continue;
                }
                dataSource.push(data);
            }

            return dataSource;
        }

        function getSearchResultMaterial(datas, str) {
            var selectCode = datas.select('*').where('code').match(str).fetch();
            var selectName = datas.select('*').where('desc').match(str).fetch();
            var selectDisplay = datas.select('*').where('display').match(str).fetch();
            var selectResult = [];

            if (selectCode.length > 0) {
                for (var i = 0; i < selectCode.length; i++) {
                    selectResult.push(selectCode[i]);
                }
            }

            if (selectName.length > 0) {
                for (var i = 0; i < selectName.length; i++) {
                    var hasCode = jQuery.grep(selectResult, function (element) {
                        return element.code == selectName[i].code;
                    });

                    if (hasCode.length == 0) {
                        selectResult.push(selectName[i]);
                    }
                }
            }

            if (selectDisplay.length > 0) {
                for (var i = 0; i < selectDisplay.length; i++) {
                    var hasCode = jQuery.grep(selectResult, function (element) {
                        return element.code == selectDisplay[i].code;
                    });

                    if (hasCode.length == 0) {
                        selectResult.push(selectDisplay[i]);
                    }
                }
            }

            return selectResult;
        }

        function initAutoCompleteMaterial(obj, defaultValue) {
            bindAutoCompleteMaterial(
                obj,
                getDataSourceMaterial(),
                defaultValue
            );
        }

        function bindAutoCompleteMaterial(obj, data, defaultValue) {            
            var txtValue = $(obj).closest("td").find(".hdfMaterialCode").first();
        
            var DB = new JQL(data);

            $(obj).typeahead('destroy');
            $(obj).typeahead({
                hint: true,
                highlight: true,
                minLength: 0
            }, {
                limit: 20,
                templates: {
                    pending: '<div class="text-danger" style="padding: 2px 10px; line-height: 24px;">Result not found.</div>',
                    suggestion: function (data) {
                        return '<div>' + data.display + '</div>';
                    }
                },
                source: function (str, callback, serverCallback) {
                    var selectResult = getSearchResultMaterial(DB, str);

                    // Search ข้อมูลใหม่ถ้า Select TOP (1000) แล้วไม่เจอ
                    if ((DB.data_source.length >= 1000) && selectResult.length == 0) {
                        datas = getDataSourceMaterial(str);
                        DB = new JQL(datas);
                        selectResult = getSearchResultMaterial(DB, str);
                        serverCallback(selectResult);
                    } else {
                        callback(selectResult);
                    }
                },
                display: function (data) {
                    return data.display;
                }
            });

            $(obj).bind('typeahead:change', function (e, v) {
                if (v.trim() == "") {
                    txtValue.val("");
                    
                    // Todo Other Function
                    materialSelectedChange(txtValue);
                }
            });

            $(obj).bind('typeahead:select typeahead:autocomplete', function (e, v) {
                txtValue.val(v.code);              

                // Todo Other Function
                materialSelectedChange(txtValue);
            });

            if (!defaultValue) {
                defaultValue = "";
            } else {
                defaultValue = getSearchResultMaterial(
                    new JQL(JSON.parse($("#hddJsonMat").val())),
                    defaultValue
                )[0].display;
            }

            $(obj).typeahead('val', defaultValue);

            if (defaultValue != "") {
                var temp = defaultValue.split(":");
                var code = temp[0].trim();
               
                txtValue.val(code);              
            }
        }

        function materialSelectedChange(sender) {
            //AGLoading(false);
            AGLoading(true);
            $(sender).closest("tr").find(".btn-get-uom:first").click();
            //$(event.target.closest("tr")).find("button:first").click();
        }

        function materialConfirmDeleteRow(sender) {
            if (AGConfirm(sender, "Confirm Delete Row")) {
                AGLoading(true);
                $(sender).next().click();
            }
        }

        function propertyValueConfirmDeleteRow(sender) {
            if (AGConfirm(sender, "Confirm Delete Row")) {
                AGLoading(true);
                $(sender).next().click();
            }
        }

        function tabSummaryIsNotNull() {
            $("#<%=ddlAffectSLA.ClientID %>").addClass("required");
            $("#<%=tbSummaryProblem.ClientID %>").addClass("required");
            $("#<%=tbSummaryCause.ClientID %>").addClass("required");
            $("#<%=tbSummaryResolution.ClientID %>").addClass("required");
            $("#nav-summary-tab").click();
            $("#btn-close-modal-close-ticket").click();
            
        }

        function AGConfirmClosePage(obj) {
            if (AGConfirm(obj, "Close Page Confirm")) {
                AGLoading(true);
                return true;
            }
            return false;
        }

    </script>
    
    <script>
        

        // Count Down Page
        //var panelTimeCountDown, momentOfTime, countDownDate, countdownfunction;
        //panelTimeCountDown = document.getElementById("demo");
        //momentOfTime = new Date();
        //myTimeSpan = 15 * 60 * 1000;
        //momentOfTime.setTime(momentOfTime.getTime() + myTimeSpan);
        //countDownDate = momentOfTime.getTime();

        //countdownfunction = setInterval(function () {
        //    var now = new Date().getTime();
        //    var distance = countDownDate - now;

        //    var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        //    var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        //    var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        //    var seconds = Math.floor((distance % (1000 * 60)) / 1000);

        //    panelTimeCountDown.innerHTML = minutes + "m " + seconds + "s ";

        //    if (distance < 0) {
        //        clearInterval(countdownfunction);
        //        panelTimeCountDown.innerHTML = "EXPIRED";
        //    }
        //}, 1000);
    </script>
    <ag:ActivitySendMailModal runat="server" id="ActivitySendMailModal" />

</asp:Content>
