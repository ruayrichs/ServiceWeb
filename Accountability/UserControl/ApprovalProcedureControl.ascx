<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApprovalProcedureControl.ascx.cs" Inherits="ServiceWeb.Accountability.UserControl.ApprovalProcedureControl" %>
<%--<%@ Register Src="~/UserControl/ActivitySendMailModal.ascx" TagPrefix="ag" TagName="ActivitySendMailModal" %>--%>
<div class="hide">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCodeidentityInitiative">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hddInitiativeCode" />
            <asp:HiddenField runat="server" ID="hddWorkGroupCode" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:CheckBox Text="text" runat="server" ID="chkLoadData"
                CssClass="check-load-data-initiative-control" data-panel="panelApprovalProcedureModelControl" />
            <asp:CheckBox Text="text" runat="server" ID="chkKeepDataSession"
                CssClass="chack-keep-data-initiative-control" data-panel="panelApprovalProcedureModelControl" />
            <asp:CheckBox Text="text" runat="server" ID="chkSaveDataSession"
                CssClass="check-save-data-initiative-control" data-panel="panelApprovalProcedureModelControl" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<%--<link href="-/Accountability/Lib/agape-tree-menu.css" rel="stylesheet" />
<script src="-/Accountability/Lib/agape-tree-menu.js"></script>--%>
<style>
    .text-init-cancel {
        color: #b30000 !important;
    }

    .img-circle-box {
        border: 1px solid #ccc;
        background-size: 100% auto;
        overflow-x: hidden;
        overflow-y: hidden;
        margin: 0 auto;
        position: absolute;
        left: 0;
        top: 0;
        width: 40px;
        height: 40px;
    }

    .table-list-view {
        margin-bottom: -1px;
    }

        .table-list-view tr td {
            text-overflow: ellipsis;
            overflow-x: hidden;
            white-space: nowrap;
        }

    .title-tab-tr th {
        background-color: #505050 !important;
        color: #fff;
    }

    .header-tab-tr th {
        background-color: #bbb !important;
    }

    .table-s .aa:nth-child(odd) td {
        background-color: gray !important;
    }

    .img-box-ini-style {
        float: left;
        background-size: cover;
        background-position: center center;
        -webkit-background-size: cover;
        -moz-background-size: cover;
        background-size: cover;
        -o-background-size: cover;
        width: 60px;
        height: 60px;
        display: block;
        border: 1px solid #ccc;
        margin-right: 5px;
    }

    .background-success {
        background-color: #dff0d8;
    }

    .background-warning {
        background-color: #f9f2f4;
    }

    .img-box-ini-style {
        float: left;
        background-size: cover;
        background-position: center center;
        -webkit-background-size: cover;
        -moz-background-size: cover;
        background-size: cover;
        -o-background-size: cover;
        width: 60px;
        height: 60px;
        display: block;
        border: 1px solid #ccc;
        margin-right: 5px;
    }
</style>


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

            <asp:DropDownList runat="server" CssClass="ticket-allow-editor" ID="ddlTicketStatus_Temp" ClientIDMode="Static">
            </asp:DropDownList>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpApprovalProcedureModelControl">
        <ContentTemplate>
            <div>
                <div class="row hide">
                    <div class="col-md-12">
                        <asp:UpdatePanel runat="server" ID="updateButton" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="text-right">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" Text="Save" OnClientClick="AGLoading(true);" OnClick="btnSave_Click" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:UpdatePanel runat="server" ID="udpStateGate" UpdateMode="Conditional">
                            <ContentTemplate>

                                <style>
                                    .approval-block {
                                        background: #F9F9F9;
                                        margin-bottom: 10px;
                                        border: 1px solid;
                                        border-color: #e5e6e9 #dfe0e4 #d0d1d5;
                                        border-radius: 3px;
                                    }

                                    .approval-block-header {
                                        padding: 5px 10px;
                                        background: #505050;
                                        color: #fff;
                                        font-weight: 600;
                                    }

                                    .approval-block-header-state {
                                        background: #F9F9F9;
                                        margin-left: 5px;
                                        padding: 0 3px;
                                    }

                                    .approval-block-content {
                                        padding: 10px;
                                    }

                                    .approval-history-box {
                                        overflow-y: auto;
                                        background: #FAFFBD;
                                    }

                                    .participant-state {
                                        color: #aaa;
                                        display: none;
                                    }

                                    .participant-state-upgrade-control .participant-state-upgrade {
                                        display: block;
                                    }

                                    .participant-state-downgrade-control .participant-state-downgrade {
                                        display: block;
                                    }

                                    .participant-state-waiting-control .participant-state-waiting {
                                        display: block;
                                    }
                                </style>

                                <script>
                                    function approvalhistoryBoxHeight() {
                                        $(".approval-history-box").each(function () {
                                            $(this).height($(this).closest(".approval-block-content").find(".approval-block-content-approver").height() - 67);
                                        });
                                        $(".approval-block-content").each(function () {
                                            $(this).find(".Structurename").each(function () {
                                                if ($(this).find(".apprval-state-approve").length > 0) {

                                                    $(this).find(".apprval-state-waiting").css("color", "#0085A1").html("Passed");
                                                }
                                            });
                                        });
                                    }
                                    function approvalTabClick(obj, tab) {
                                        $("#approval-tab .nav-tabs li a").removeClass("active");
                                        $(obj).addClass("active");

                                        $("#approval-tab .tab-content .tab-pane").removeClass("in").removeClass("active");
                                        $("#approval-tab .tab-content #" + tab).addClass("in").addClass("active");

                                        approvalhistoryBoxHeight();
                                    }
                                </script>

                                <div id="approval-tab">
                                    <ul class="nav nav-tabs hide">
                                        <li class="nav-item">
                                            <a class="nav-link active" id="stateGate" href="javascript:;" onclick="approvalTabClick(this,'approval-menu-1');">Stage gate
                                            </a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link" id="objectEvent" href="javascript:;" onclick="approvalTabClick(this,'approval-menu-2');">Financial approval
                                            </a>
                                        </li>
                                    </ul>

                                    <div class="tab-content">
                                        <div id="approval-menu-1" class="tab-pane in active">
                                            <br />
                                            <asp:Repeater runat="server" ID="rptApprovalProcedureStateGate" OnItemDataBound="rptApprovalProcedureStateGate_ItemDataBound">
                                                <ItemTemplate>
                                                    <asp:Panel ID="pnStateGate" runat="server">
                                                        <div class="approval-block
                                                    <%#
                                                        Eval("APPROVESTARTED").ToString().Trim().ToUpper().Equals("TRUE")
                                                        || (
                                                            Eval("APPROVESTATUS").ToString().Trim().ToUpper().Equals("TRUE")
                                                            && Eval("APPROVESTARTED").ToString().Trim().ToUpper().Equals("FALSE")
                                                            && Eval("REQUESTAPPROVADOWNGRADE").ToString().Trim().ToUpper().Equals("FALSE")
                                                            )
                                                        ? "participant-state-upgrade-control"
                                                        : (
                                                            Eval("REQUESTAPPROVADOWNGRADE").ToString().Trim().ToUpper().Equals("TRUE")
                                                            ? "participant-state-downgrade-control"
                                                            : "participant-state-waiting-control"
                                                        )
                                                    %>">

                                                            <div class="approval-block-header">
                                                                <%# Eval("STATEGATETO_DESC") %> approval

                                                                <span class="approval-block-header-state
                                                            <%# 
                                                
                                                            Eval("APPROVESTARTED").ToString().Trim().ToUpper().Equals("TRUE")
                                                            ? "text-success"
                                                            : (
                                                                Eval("REQUESTAPPROVADOWNGRADE").ToString().Trim().ToUpper().Equals("TRUE")
                                                                ? "text-danger"
                                                                : (
                                                                    Eval("APPROVESTATUS").ToString().Trim().ToUpper().Equals("TRUE") 
                                                                    ? "text-primary"
                                                                    : "text-warning"
                                                                )
                                                            )
                                                    
                                                            %>">
                                                                    <%# 
                                                    
                                                            Eval("APPROVESTARTED").ToString().Trim().ToUpper().Equals("TRUE")
                                                            ? "Wait for Approve (upgrade)"
                                                            : (
                                                                Eval("REQUESTAPPROVADOWNGRADE").ToString().Trim().ToUpper().Equals("TRUE")
                                                                ? "Wait for Approve (downgrade)"
                                                                : (
                                                                    Eval("APPROVESTATUS").ToString().Trim().ToUpper().Equals("TRUE") 
                                                                    ? "Approved"
                                                                    : "Not Start"
                                                                )
                                                            )
                                                    
                                                                    %>
                                                                </span>

                                                                <span class="fa fa-envelope <%# Eval("APPROVESTARTED").ToString().Trim().ToUpper().Equals("TRUE") || Eval("REQUESTAPPROVADOWNGRADE").ToString().Trim().ToUpper().Equals("TRUE")
                                                                ? "" : "hide" %>"
                                                                    style="cursor: pointer; float: right; padding-top: 2px;" onclick="$(this).next().click();">Send alert mail</span>

                                                                <asp:Button ID="btnSendGroupNotApproved" runat="server" CssClass="initiative-approvel-management hide ticket-allow-editor ticket-allow-editor-everyone" CommandArgument='<%# Eval("INITIATIVECODE")+","+Eval("STATEGATEFROM")+","+Eval("STATEGATETO") %>'
                                                                    OnClick="btnSendGroupNotApproved_Click"/>

                                                                <asp:HiddenField runat="server" ID="hddApproveStartedforhide" Value='<%# Eval("APPROVESTARTED") %>' />
                                                                <asp:HiddenField runat="server" ID="hddRequestApproveDowngradeforhide" Value='<%# Eval("REQUESTAPPROVADOWNGRADE") %>' />
                                                                <asp:HiddenField runat="server" ID="hddApprovalProcedureStateGateFrom" Value='<%# Eval("STATEGATEFROM") %>' />
                                                                <asp:HiddenField runat="server" ID="hddApprovalProcedureStateGateTo" Value='<%# Eval("STATEGATETO") %>' />
                                                            </div>
                                                            <div class="approval-block-content">
                                                                <div class="row">
                                                                    <div class="col-sm-8 approval-block-content-approver" style="min-height: 100px;">
                                                                        <asp:Repeater runat="server" ID="rptApprovalProcedureStateGateInner" OnItemDataBound="rptApprovalProcedureStateGateInner_ItemDataBound">
                                                                            <ItemTemplate>
                                                                                <label>
                                                                                    <%# Eval("StructureName") %>
                                                                                </label>
                                                                                <asp:HiddenField runat="server" ID="hddSTATEGATETO" Value='<%# Eval("STATEGATETO") %>' />
                                                                                <asp:HiddenField runat="server" ID="hddSTATEGATEFROM" Value='<%# Eval("STATEGATEFROM") %>' />
                                                                                <asp:HiddenField runat="server" ID="hddApprovalProcedureStateGateInnerStructure" Value='<%# Eval("StructureCode") %>' />
                                                                               
                                                                                <asp:Repeater runat="server" ID="rptApprovalProcedureStateGateInnerRole" OnItemDataBound="rptApprovalProcedureStateGateInnerRole_ItemDataBound">
                                                                                    <ItemTemplate>
                                                                                        <div class="Structurename" data-structure="<%# Eval("RoleName") %>">
                                                                                            <asp:HiddenField runat="server" ID="hddApprovalProcedureStateGateInnerRoleName" Value='<%# Eval("RoleName") %>' />
                                                                                            <asp:HiddenField runat="server" ID="hddStatusAPforhide" Value='<%# Eval("StatusAP") %>' />
                                                                                            <div class="mat-box <%# 
                                                                                        Eval("StatusAP").ToString().Trim().ToUpper().Equals("APPROVED")
                                                                                        ? "background-success" : (Eval("StatusAP").ToString().Trim().ToUpper().Equals("REJECTED") ? "background-warning" : "" ) %>"
                                                                                                style="padding-top: 8px; padding-bottom: 0;">
                                                                                                <p>
                                                                                                    <b class="text-primary"><%# Eval("RoleName") %></b>
                                                                                                    
                                                                                                    <asp:Label ID="lbEnvelopebt" runat="server" CssClass="fa fa-envelope" Style="cursor: pointer; float: right; padding-top: 2px;" onclick="$(this).next().click();">
                                                                                                        Send alert mail</asp:Label>
                                                                                                    <asp:Button ID="btnSendNotApproved" runat="server" CssClass="initiative-approvel-management hide ticket-allow-editor ticket-allow-editor-everyone" CommandArgument='<%# Eval("RoleCode") %>'
                                                                                                        OnClick="btnSendNotApproved_Click" />
                                                                                               <%--   <button id="hiddenbtn" type="button" class="btn ticket-allow-editor ticket-allow-editor-everyone"
                                                                                                        onclick="sendCustomEmail('<%= _docnumber%>', '<%= ERPW.Lib.Authentication.ERPWAuthentication.FullNameEN %>')">
                                                                                                        Send Mail</button>--%>
                                                                                                </p>
                                                                                                <div class="row">
                                                                                                    <asp:Repeater runat="server" ID="rptApprovalProcedureStateGateInnerStructure">
                                                                                                        <ItemTemplate>
                                                                                                            <div class="col-lg-6 one-line" style="padding: 0 5px; margin-bottom: 15px;">
                                                                                                                <div class="img-box-ini-style" style="width: 40px; height: 40px; border-radius: 0; background-image: url('<%# ServiceWeb.Service.UserImageService.getImgProfile(Eval("EmployeeCode").ToString()).Image_64 %>'),url('/images/user.png');"></div>

                                                                                                                <div>
                                                                                                                    <%# Eval("fullnameEN") %>
                                                                                                                    <asp:HiddenField runat="server" ID="hddApprovalProcedure_EmployeeCode" Value='<%# Eval("EmployeeCode") %>' />
                                                                                                                </div>
                                                                                                                <div class="participant-state participant-state-waiting">
                                                                                                                    <%--Waiting--%>
                                                                                                                    <%# TranslateParticipantsStatus(Eval("STATUS"),Eval("Approved_On")) %>
                                                                                                                </div>
                                                                                                                <div class="participant-state participant-state-upgrade">
                                                                                                                    <%# TranslateParticipantsStatus(Eval("STATUS"),Eval("Approved_On")) %>
                                                                                                                </div>
                                                                                                                <div class="participant-state participant-state-downgrade">
                                                                                                                    <%# TranslateParticipantsStatus(Eval("STATUSDOWNGRADE"),Eval("ApprovedDownGate_On")) %>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:Repeater>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                </asp:Repeater>


                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </div>
                                                                    <div class="col-sm-4">
                                                                        <label>History</label>
                                                                        <div class="mat-box approval-history-box">
                                                                            <asp:Repeater runat="server" ID="rptApprovalProcedureLog">
                                                                                <ItemTemplate>
                                                                                    <p>
                                                                                        <small>
                                                                                            <%# Container.ItemIndex + 1 %>.
                                                                                            <b>
                                                                                                <span class="text-muted"><%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Convert.ToString(Eval("created_on"))) %></span>
                                                                                                <span class="text-primary"><%# Eval("FullnameEN") %></span>
                                                                                                <%# Eval("Message") %>
                                                                                                <span class="<%# controlColorMassage(Eval("Event").ToString(), Eval("ApproveType").ToString()) %>">[Process <%# controlMassage(Eval("Event").ToString(), Eval("ApproveType").ToString()) %>]
                                                                                                </span>
                                                                                            </b>

                                                                                        </small>
                                                                                    </p>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                            <div class="text-center">
                                                                                <asp:Label Text="No log" runat="server" ID="lblApprovalProcedureLogEmpty" Visible="false" />
                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <%--</div>
                                        <div id="approval-menu-2" class="tab-pane active">--%>
                                            <br />
                                            <asp:Repeater runat="server" ID="rptApprovalProcedureEventObject" OnItemDataBound="rptApprovalProcedureEventObject_ItemDataBound">
                                                <ItemTemplate>
                                                    <div class="approval-block">
                                                        <div class="approval-block-header">
                                                            <%# Eval("EventDesc") %>
                                                            <span class="approval-block-header-state
                                                            <%# Convert.ToBoolean(Eval("EventApproving")) ? "text-success" : (Convert.ToBoolean(Eval("EventApproved")) ? "text-primary" :  "text-warning") %>">
                                                                <%# Convert.ToBoolean(Eval("EventApproving")) ? "Wait for Approve" : (Convert.ToBoolean(Eval("EventApproved")) ? "Approved" :  "Not Start") %>
                                                            </span>

                                                            <span class="fa fa-envelope <%# Convert.ToBoolean(Eval("EventApproving")) ? "" : "hide" %>"
                                                                style="cursor: pointer; float: right; padding-top: 2px;" onclick="$(this).next().click();">Send alert mail</span>
                                                            <asp:Button ID="btnSendAllGroupNotApprovedFinancial" runat="server" CssClass="initiative-approvel-management hide" CommandArgument='<%# Eval("EventCode") %>'
                                                                OnClick="btnSendAllGroupNotApprovedFinancial_Click" OnClientClick="AGLoading(true);" />

                                                            <asp:HiddenField runat="server" ID="hddEventApproving" Value='<%# Eval("EventApproving") %>' />
                                                            <asp:HiddenField runat="server" ID="hddApprovalProcedureEventObject" Value='<%# Eval("EventCode") %>' />
                                                        </div>
                                                        <div class="approval-block-content">
                                                            <div class="row">
                                                                <div class="col-sm-8 approval-block-content-approver">
                                                                    <asp:Repeater runat="server" ID="rptApprovalProcedureEventObjectInner" OnItemDataBound="rptApprovalProcedureEventObjectInner_ItemDataBound">
                                                                        <ItemTemplate>
                                                                            <label>
                                                                                <%# Eval("StructureName") %>
                                                                            </label>

                                                                            <asp:HiddenField runat="server" ID="hddApprovalProcedureEventObjectInnerStructure" Value='<%# Eval("StructureCode") %>' />


                                                                            <asp:Repeater runat="server" ID="rptApprovalProcedureEventObjectInnerRole" OnItemDataBound="rptApprovalProcedureEventObjectInnerRole_ItemDataBound">
                                                                                <ItemTemplate>
                                                                                    <%--แบงกรอบ--%>
                                                                                    <div class="Structurename event-object-approval" data-structure="<%# Eval("RoleName") %>">
                                                                                        <asp:HiddenField runat="server" ID="hddApprovalProcedureEventObjectInnerRoleName" Value='<%# Eval("RoleName") %>' />
                                                                                        <div class="mat-box"
                                                                                            style="padding-top: 8px; padding-bottom: 0;">
                                                                                            <p>
                                                                                                <b class="text-primary"><%# Eval("RoleName") %></b>
                                                                                                <asp:HiddenField runat="server" ID="hddStatusAPFinancial" Value='<%# Eval("StatusAP") %>' />

                                                                                                <asp:Label ID="lbEnvelopebtFinancial" runat="server" CssClass="fa fa-envelope" Style="cursor: pointer; float: right; padding-top: 2px;" onclick="$(this).next().click();">
                                                                                                Send alert mail</asp:Label>
                                                                                                <asp:Button ID="btnSendNotApprovedFinancial" runat="server" CssClass="initiative-approvel-management hide" CommandArgument='<%# Eval("RoleCode") %>'
                                                                                                    OnClick="btnSendNotApprovedFinancial_Click" OnClientClick="AGLoading(true);" />
                                                                                            </p>
                                                                                            <div class="row">
                                                                                                <asp:Repeater runat="server" ID="rptApprovalProcedureEventObjectInnerStructure">
                                                                                                    <ItemTemplate>
                                                                                                        <div class="col-lg-6 one-line" style="padding: 0 5px; margin-bottom: 15px;">
                                                                                                            <div class="img-box-ini-style" style="width: 40px; height: 40px; border-radius: 0; background-image: url('<%= Page.ResolveUrl("~") %>images/user.png'),url('<%= Page.ResolveUrl("~") %>images/user.png');"></div>

                                                                                                            <div>
                                                                                                                <%# Eval("fullname") %>
                                                                                                            </div>
                                                                                                            <div>
                                                                                                                <%--คน--%>
                                                                                                                <%# TranslateParticipantsStatus(Eval("STATUS"),Eval("Approved_On")) %>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </ItemTemplate>
                                                                                                </asp:Repeater>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>


                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                                <div class="col-sm-4">
                                                                    <label>History</label>
                                                                    <div class="mat-box approval-history-box">
                                                                        <asp:Repeater runat="server" ID="rptApprovalProcedureEventObjectLog">
                                                                            <ItemTemplate>
                                                                                <p>
                                                                                    <small>
                                                                                        <%# Container.ItemIndex + 1 %>.
                                                                                <b>
                                                                                    <span class="text-muted"><%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Convert.ToString(Eval("created_on"))) %></span>
                                                                                    <span class="text-primary"><%# Eval("FullName") %></span>
                                                                                    <%# controlApproveTypeMassage(Eval("ApproveType").ToString(),Eval("Message").ToString()) %>
                                                                                </b>

                                                                                    </small>
                                                                                </p>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                        <div class="text-center">
                                                                            <asp:Label Text="No log" runat="server" ID="lblApprovalProcedureEventObjectLogEmpty" Visible="false" />
                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div id="modalAddEmp" class="modal fade">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal">×</button>
                                        <h4 class="modal-title">เพิ่มผู้เกี่ยวข้อง</h4>
                                    </div>
                                    <div class="modal-body">

                                        <div style="margin-bottom: 10px;">
                                            <label>
                                                Participants Description
                                            </label>
                                            <asp:DropDownList ID="ddlParticipantsDesc" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div style="margin-bottom: 10px;">
                                            <label>
                                                Character Description
                                            </label>
                                            <div class="input-group" style="width: 100%">
                                                <input type="text" readonly name="name" style="background: #fff;" onclick="openSubProjectHierarchy(event); bindHierarchyCharacter(this);" id="txtSubProjectDescription" runat="server" clientidmode="Static" class="form-control blue-require-style" />
                                                <span style="cursor: pointer;" class="input-group-addon" onclick="removeSubProjectSelected();">
                                                    <i class="fa fa-remove pull-right"></i>
                                                </span>
                                                <span style="cursor: pointer;" class="input-group-addon" onclick="openSubProjectHierarchy(event);bindHierarchyCharacter(this);">
                                                    <i class="fa fa-list"></i>
                                                </span>
                                                <div onclick="event.stopPropagation();" class="pane-subproject-container" style="display: none; z-index: 30000; position: absolute; padding: 10px; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.37); background: #fff; left: 0px; top: 100%; padding-top: 0px; width: 100%;">
                                                    <div>
                                                        <i class="fa fa-remove pull-right" style="color: #aaa; margin-top: 10px; cursor: pointer;" onclick="$('.pane-subproject-container').hide();"></i>
                                                    </div>
                                                    <div class="pane-subproject" style="overflow-y: auto; padding: 10px; max-height: 250px; padding-top: 0px; margin-top: 35px; margin-bottom: 20px">
                                                    </div>
                                                    <div class="text-center pane-subproject-empty" style="padding: 10px 20px; border: 1px solid #ccc; margin-top: 35px">
                                                        ไม่พบบทบาท
                                                    </div>
                                                </div>
                                                <asp:UpdatePanel ID="upnSubproject" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div style="display: none">

                                                            <asp:TextBox runat="server" ClientIDMode="Static" CssClass="form-control ticket-allow-editor" ID="txtChangeFolderSubProject" meta:resourcekey="txtChangeFolderSubProjectResource1" />

                                                            <asp:Button Text="text" ID="btnChangeSubProject" OnClick="btnChangeSubProject_Click"
                                                                ClientIDMode="Static" runat="server" meta:resourcekey="btnChangeSubProjectResource1" />

                                                            <asp:DropDownList onchange="subProjectChange();" runat="server" ID="ddlSubProject" ClientIDMode="Static" CssClass="form-control"
                                                                Width="100%" meta:resourcekey="ddlSubProjectResource1">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <script>
                                                    //bindHierarchyCharacter();
                                                    function openPopUp(id) {
                                                        $("#modalAddEmp").modal("show");
                                                        $(".modal-backdrop").fadeOut();
                                                        console.log(id);
                                                        $("#hddID").val(id);
                                                    }
                                                    function closePopUp() {
                                                        $("#modalAddEmp").modal("hide");
                                                        $('.modal-backdrop').fadeOut();
                                                    }
                                                    function openSubProjectHierarchy(e) {
                                                        $(".pane-subproject-container").fadeIn();
                                                        e.stopPropagation();
                                                    }
                                                    function loading() {
                                                        $(".pane-subproject-container").AGWhiteLoading(true);
                                                    }
                                                    function hierarchyLoading() {
                                                        removeLoading();
                                                        $(".pane-subproject-container").AGWhiteLoading(true);
                                                    }
                                                    function removeLoading() {
                                                        $(".pane-subproject-container").AGWhiteLoading(false);
                                                    }
                                                    function bindHierarchyCharacter(evt) {
                                                        var apiUrl = servictWebDomainName + "HierarchyMaster/API/HirearchyStructureAPI.aspx";
                                                        var hierarchytype = $(evt).parent().parent().parent().find("#ddlParticipantsDesc").val()
                                                        console.log(hierarchytype);
                                                        hierarchyLoading();
                                                        $.ajax({
                                                            url: apiUrl,
                                                            data: {
                                                                request: "list",
                                                                hierarchyType: hierarchytype
                                                            },
                                                            success: function (datas) {
                                                                if (datas != "") {
                                                                    $(".pane-subproject-empty").hide();
                                                                }
                                                                $(".pane-subproject").aGapeTreeMenu({
                                                                    data: datas,
                                                                    rootText: "Root",
                                                                    rootCode: "",
                                                                    rootCount: 0,
                                                                    navigateText: "Create structure",
                                                                    onlyFolder: false,
                                                                    share: false,
                                                                    moveItem: false,
                                                                    selecting: false,
                                                                    emptyFolder: true,
                                                                    onClick: function (result) {
                                                                        if (result.id != "") {
                                                                            $("#txtChangeFolderSubProject").val(result.id);
                                                                            $(".pane-subproject-container").hide();
                                                                            $("#btnChangeSubProject").click();
                                                                        }
                                                                    }
                                                                });
                                                                removeLoading();
                                                            }
                                                        });
                                                    }
                                                    function removeSubProjectSelected() {
                                                        if ($("#txtSubProjectDescription").val() != "ไม่พบบทบาท") {
                                                            $("#txtSubProjectDescription").val("เลือกบทบาท");
                                                        }
                                                    }
                                                    function bindDataName() {
                                                        var idd = $("#hddID").val();
                                                        var namee = $("#hddName").val();
                                                        console.log(namee);
                                                        $("#" + idd).val(namee);
                                                    }
                                                </script>
                                            </div>
                                        </div>
                                        <div class="text-right">
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:HiddenField ID="hddID" runat="server" ClientIDMode="Static" />
                                                    <asp:HiddenField ID="hddName" runat="server" ClientIDMode="Static" />
                                                    <asp:Button Text="เพิ่ม" CssClass="btn btn-primary" ID="btnAddActor" runat="server" OnClick="btnAddActor_Click" />
                                                    &nbsp;&nbsp;
                                            <span onclick="closePopUp();" class="btn btn-warning">ยกเลิก</span>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        function checkPanelLoadModel() {
            return;
        }
        function setBackgroundSuccessEventObjectApproval() {
            $(".event-object-approval").each(function () {
                var chkSuccess = $(this).find(".text-success");
                if (chkSuccess.length > 0) {
                    var firstDIV = $(this).find("div");
                    $(firstDIV[0]).addClass("background-success");
                }
            });
        }
    </script>
    <%--<ag:ActivitySendMailModal runat="server" id="ActivitySendMailModal" />--%>
</div>
