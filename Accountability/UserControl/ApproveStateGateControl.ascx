<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApproveStateGateControl.ascx.cs" Inherits="ServiceWeb.Accountability.UserControl.ApproveStateGateControl" %>

<script>
    function clickApprovalRequestCancelInitiativeModelControlAS<%= ClientID %>(sender) {
        $('#<%= txtRemarkInitiativeWeeklyStatusModelControlAS.ClientID %>').val('');
        $("#<%= btnApprovelRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnApprovelRequestCancelInitiativeAS.ClientID %>").show();
        $("#<%= btnRejectRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnConfirmRequestApprovelInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnRequestDowngradeInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnSaveRequestCancelWeeklyStatusModelControlAS.ClientID %>").hide();

        $("#<%= btnApprovelRequestDowngradeInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestDowngradeInitiativeAS.ClientID %>").hide();

        $('#RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS<%= ClientID %>').fadeIn();
        return false;
    }

    function clickRejectRequestCancelInitiativeModelControlAS<%= ClientID %>(sender) {
        $('#<%= txtRemarkInitiativeWeeklyStatusModelControlAS.ClientID %>').val('');

        $("#<%= btnApprovelRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnApprovelRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestCancelInitiativeAS.ClientID %>").show();
        $("#<%= btnConfirmRequestApprovelInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnRequestDowngradeInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnSaveRequestCancelWeeklyStatusModelControlAS.ClientID %>").hide();

        $("#<%= btnApprovelRequestDowngradeInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestDowngradeInitiativeAS.ClientID %>").hide();

        $('#RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS<%= ClientID %>').fadeIn();
        return false;
    }

    function clickApprovalRequestDowngradeInitiativeModelControlAS<%= ClientID %>(sender) {
        $('#<%= txtRemarkInitiativeWeeklyStatusModelControlAS.ClientID %>').val('');

        $("#<%= btnApprovelRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnApprovelRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnConfirmRequestApprovelInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnRequestDowngradeInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnSaveRequestCancelWeeklyStatusModelControlAS.ClientID %>").hide();

        $("#<%= btnApprovelRequestDowngradeInitiativeAS.ClientID %>").show();
        $("#<%= btnRejectRequestDowngradeInitiativeAS.ClientID %>").hide();

        $('#RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS<%= ClientID %>').fadeIn();
        return false;
    }

    function clickRejectRequestDowngradeInitiativeModelControlAS<%= ClientID %>(sender) {
        $('#<%= txtRemarkInitiativeWeeklyStatusModelControlAS.ClientID %>').val('');

        $("#<%= btnApprovelRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnApprovelRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnConfirmRequestApprovelInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnRequestDowngradeInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnSaveRequestCancelWeeklyStatusModelControlAS.ClientID %>").hide();

        $("#<%= btnApprovelRequestDowngradeInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestDowngradeInitiativeAS.ClientID %>").show();

        $('#RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS<%= ClientID %>').fadeIn();
        return false;
    }

    function clickRequestDowngradeWeeklyStatusModelControlAS<%= ClientID %>(sender) {
        $('#<%= txtRemarkInitiativeWeeklyStatusModelControlAS.ClientID %>').val('');

        $("#<%= btnApprovelRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnApprovelRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnConfirmRequestApprovelInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnRequestDowngradeInitiativeModelControlAS.ClientID %>").show();
        $("#<%= btnSaveRequestCancelWeeklyStatusModelControlAS.ClientID %>").hide();

        $("#<%= btnApprovelRequestDowngradeInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestDowngradeInitiativeAS.ClientID %>").hide();

        $('#RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS<%= ClientID %>').fadeIn();
        return false;
    }

    function clickApprovalWeeklyStatusModelControlAS<%= ClientID %>(sender) {
        $('#<%= txtRemarkInitiativeWeeklyStatusModelControlAS.ClientID %>').val('');

        $("#<%= btnApprovelRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnApprovelRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnConfirmRequestApprovelInitiativeModelControlAS.ClientID %>").show();
        $("#<%= btnRequestDowngradeInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnSaveRequestCancelWeeklyStatusModelControlAS.ClientID %>").hide();

        $("#<%= btnApprovelRequestDowngradeInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestDowngradeInitiativeAS.ClientID %>").hide();

        $('#RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS<%= ClientID %>').fadeIn();
        return false;
    }

    function clickRequestCancelInitiativeModelControlAS<%= ClientID %>(sender) {
        $('#<%= txtRemarkInitiativeWeeklyStatusModelControlAS.ClientID %>').val('');

        $("#<%= btnApprovelRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnApprovelRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnConfirmRequestApprovelInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnRequestDowngradeInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnSaveRequestCancelWeeklyStatusModelControlAS.ClientID %>").show();

        $("#<%= btnApprovelRequestDowngradeInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestDowngradeInitiativeAS.ClientID %>").hide();

        $('#RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS<%=  ClientID %>').fadeIn();
        return false;
    }

    function clickApprovalRequestUpdateInitiativeModelControlAS<%= ClientID %>(sender) {
        $('#<%= txtRemarkInitiativeWeeklyStatusModelControlAS.ClientID %>').val('');

        $("#<%= btnApprovelRequestApprvelInitiativeAS.ClientID %>").show();
        $("#<%= btnRejectRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnApprovelRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnConfirmRequestApprovelInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnRequestDowngradeInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnSaveRequestCancelWeeklyStatusModelControlAS.ClientID %>").hide();

        $("#<%= btnApprovelRequestDowngradeInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestDowngradeInitiativeAS.ClientID %>").hide();

        $('#RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS<%= ClientID %>').fadeIn();
        return false;
    }

    function clickRejectRequestUpdateInitiativeModelControlAS<%= ClientID %>(sender) {
        $('#<%= txtRemarkInitiativeWeeklyStatusModelControlAS.ClientID %>').val('');

        $("#<%= btnApprovelRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestApprvelInitiativeAS.ClientID %>").show();
        $("#<%= btnApprovelRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnConfirmRequestApprovelInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnRequestDowngradeInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnSaveRequestCancelWeeklyStatusModelControlAS.ClientID %>").hide();

        $("#<%= btnApprovelRequestDowngradeInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestDowngradeInitiativeAS.ClientID %>").hide();

        $('#RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS<%= ClientID %>').fadeIn();
        return false;
    }

    //update
    function clickDocumentlControlAS<%= ClientID %>(sender) {
        $('#RemarkRequestCancelDocumentControlAS<%= ClientID %>').fadeIn();
        return false;
    }
    //update


    function clickCancelRequestApprovalInitiativeModelControlAS<%= ClientID %>(sender, mode) {
        $('#<%= txtRemarkInitiativeWeeklyStatusModelControlAS.ClientID %>').val('');
        $("#<%= btnApprovelRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestApprvelInitiativeAS.ClientID %>").hide();
        $("#<%= btnApprovelRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestCancelInitiativeAS.ClientID %>").hide();
        $("#<%= btnConfirmRequestApprovelInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnRequestDowngradeInitiativeModelControlAS.ClientID %>").hide();
        $("#<%= btnSaveRequestCancelWeeklyStatusModelControlAS.ClientID %>").hide();
        $("#<%= btnApprovelRequestDowngradeInitiativeAS.ClientID %>").hide();
        $("#<%= btnRejectRequestDowngradeInitiativeAS.ClientID %>").hide();

        if (mode == 'UPGRADE') {
            $("#<%= btnSaveCancelRequestApprovalInitiativeModelControlAS.ClientID %>").show();
        }
        else if (mode == 'DOWNGRADE') {
            $("#<%= btnSaveCancelRequestDowngradeInitiativeModelControlAS.ClientID %>").show();
        }
        else if (mode == 'CANCEL') {
            $("#<%= btnSaveCancelRequestCancelInitiativeModelControlAS.ClientID %>").show();
        }

    $('#RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS<%= ClientID %>').fadeIn();
        return false;
    }

</script>
<div class="hide">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCodeidentityInitiativeAS">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hddInitiativeCodeApprovalControl" />
            <asp:HiddenField runat="server" ID="hddWorkGroupCodeApprovalControl" />
            <asp:HiddenField runat="server" ID="hddWorkStategateApprovalControl" />
            <asp:HiddenField runat="server" ID="hddTicketCodeApprovalControl" />
            <asp:HiddenField runat="server" ID="hddTikcetStatusCodeTargetApprovalControl" />
            <asp:HiddenField runat="server" ID="hddTicketDocumentType" />
            <asp:HiddenField runat="server" ID="hddTicketFiscalYear" />
            <asp:HiddenField runat="server" ID="hddTicketDocumentNo" />
            <asp:HiddenField runat="server" ID="hddTicketStatusCodeOld" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<style>
    .pnls-control-stategate-approval{
        margin: -15px -15px 20px -15px;
        background: #BBBBBB;
        padding: 10px 15px;
        padding-left:20px;
        padding-bottom:15px;
    }

    .popup-remark-initiative {
        z-index: 50000;
        position: fixed;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.701961);
        top: 0px;
        left: 0px;
        display: none;
    }

        .popup-remark-initiative .remark-content {
            width: 530px;
            margin: auto;
            background: #fff;
            padding: 20px;
            margin-top: 150px;
            border: 1px solid #ccc;
            border-radius: 5px;
        }

            .popup-remark-initiative .remark-content .remark-header {
                border-bottom: 1px solid #ccc;
                padding-bottom: 5px;
            }

            .popup-remark-initiative .remark-content .remark-body {
                padding-bottom: 5px;
                padding-top: 5px;
                margin-top: 5px;
            }

            .popup-remark-initiative .remark-content .remark-footer {
                text-align: right;
                padding-top: 20px;
            }
</style>

<asp:UpdatePanel runat="server" ID="udpControls" UpdateMode="Conditional">
     <ContentTemplate>
        <asp:Panel runat="server" ID="pnlControls" CssClass="pnls-control-stategate-approval" Visible="false">
            <asp:HiddenField ID="hddStateGateAS" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hddStateGateEndAS" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hddApproveStartedAS" runat="server"></asp:HiddenField>
            <asp:HiddenField runat="server" ID="hddDownGradedStateGeteFromAS" />
            <asp:HiddenField runat="server" ID="hddDownGradedStateGeteToAS" />
            <asp:HiddenField runat="server" ID="hddDownGradedStateGeteStatusAS" />
            <div style="padding-bottom:5px;"">
                <asp:Label ID="lbTextApproveAS" runat="server"></asp:Label>
            </div>
            <div style="display:inline-block;">
                <asp:Panel runat="server" ID="pnlApprovalButtonCommand">  <%--CssClass="col-sm-12"--%>
                    <label>
                        <asp:Label ID="lbStateGateAS" CssClass="hide" runat="server"></asp:Label>
                        Request approval
                    </label>
                    <br />
                    <asp:Button ID="btnUpdateStateGateAS" runat="server" CssClass="btn btn-sm btn-primary" 
                        OnClientClick="$(this).next().click(); return false;" />
                    <span class="hide" onclick="clickApprovalWeeklyStatusModelControlAS<%= ClientID %>(this);"></span>

                    <asp:Button ID="btnDowngradedStateGateAS" runat="server" CssClass="btn btn-sm btn-warning" 
                        OnClientClick="$(this).next().click(); return false;" />
                    <span class="hide" onclick="clickRequestDowngradeWeeklyStatusModelControlAS<%= ClientID %>(this);"></span>
                    
                    
                    <asp:Button Text="Submited for cancel document approval." CssClass="btn btn-sm btn-danger" runat="server" 
                        OnClientClick="$(this).next().click(); return false;" ID="btnRequestCancelInitiativeAS" />
                    <span class="hide" onclick="clickRequestCancelInitiativeModelControlAS<%= ClientID %>(this);"></span>
                </asp:Panel>
            </div>

            <div style="display:inline-block;">
                <asp:Panel runat="server" ID="pnlApprovalButtonApproveOrRejectAS">
                    

                    <asp:Button Text="Approve" runat="server" CssClass="ticket-allow-editor-everyone btn btn-sm btn-success initiative-approvel-management" ID="btnApprovelCancelInitiativeAS" Visible="false"
                        OnClientClick="$(this).next().click(); return false;" />
                    <span class="hide" onclick="clickApprovalRequestCancelInitiativeModelControlAS<%= ClientID %>(this);"></span>
                    
                    <asp:Button Text="Reject" runat="server" CssClass="ticket-allow-editor-everyone btn btn-sm btn-danger initiative-approvel-management" ID="btnRejectCancelInitiativeAS" Visible="false"
                        OnClientClick="$(this).next().click(); return false;" />
                    <span class="hide" onclick="clickRejectRequestCancelInitiativeModelControlAS<%= ClientID %>(this);"></span>

                    <asp:Button ID="btnApprovelDowngradedInitiativeAS" runat="server" Text="Approve" CssClass="ticket-allow-editor-everyone btn btn-sm btn-success initiative-approvel-management" Visible="false"
                        OnClientClick="$(this).next().click(); return false;" />
                    <span class="hide" onclick="clickApprovalRequestDowngradeInitiativeModelControlAS<%= ClientID %>(this);"></span>
                    
                    <asp:Button Text="Reject" runat="server" CssClass="ticket-allow-editor-everyone btn btn-sm btn-danger initiative-approvel-management" ID="btnRejectDowngradedInitiativeAS" Visible="false"
                        OnClientClick="$(this).next().click(); return false;" />
                    <span class="hide" onclick="clickRejectRequestDowngradeInitiativeModelControlAS<%= ClientID %>(this);"></span>

                    <asp:Button Text="Approve" ID="btnApproveAS" runat="server" CssClass="ticket-allow-editor-everyone btn btn-sm btn-success initiative-approvel-management" Visible="false"
                        OnClientClick="$(this).next().click(); return false;" />
                    <span class="hide" onclick="clickApprovalRequestUpdateInitiativeModelControlAS<%= ClientID %>(this);"></span>    
                    
                    <asp:Button Text="Reject" ID="btnRejectAS" runat="server" CssClass="ticket-allow-editor-everyone btn btn-sm btn-danger initiative-approvel-management" Visible="false"
                        OnClientClick="$(this).next().click(); return false;" />
                    <span class="hide" onclick="clickRejectRequestUpdateInitiativeModelControlAS<%= ClientID %>(this);"></span>

                    <%-- update document --%>
                    <asp:Button Text="Document" ID="btnDocumentAS" runat="server" CssClass="ticket-allow-editor-everyone btn btn-sm btn-info initiative-approvel-management" Visible="false"
                        OnClientClick="$(this).next().click(); return false;" />
                    <span class="hide" onclick="clickDocumentlControlAS<%= ClientID %>(this);"></span>
                    <%-- update document --%>
                </asp:Panel>
            </div>
            <%-- button cancel request--%>
            <div style="display:none;">
                <asp:panel runat="server" ID="panelbtnCancelRequestApproval">
                    <asp:Button Text="Cancel Request" runat="server" CssClass="btn btn-warning btn-sm initiative-approvel-management"
                           OnClientClick="$(this).next().click(); return false;" ID="btnCancelRequestApprovalInitiative" />
                    <span class="hide" onclick="clickCancelRequestApprovalInitiativeModelControlAS<%= ClientID %>(this, 'UPGRADE');"></span>

                    <asp:Button Text="Cancel Request" runat="server" CssClass="btn btn-warning btn-sm initiative-approvel-management"
                           OnClientClick="$(this).next().click(); return false;" ID="btnCancelRequestDowngradeInitiative"/>
                    <span class="hide" onclick="clickCancelRequestApprovalInitiativeModelControlAS<%= ClientID %>(this, 'DOWNGRADE');"></span>

                    <asp:Button Text="Cancel Request" runat="server" CssClass="btn btn-warning btn-sm initiative-approvel-management"
                           OnClientClick="$(this).next().click(); return false;" ID="btnCancelRequestCancelInitiative" />
                    <span class="hide" onclick="clickCancelRequestApprovalInitiativeModelControlAS<%= ClientID %>(this, 'CANCEL');"></span>
                </asp:panel>
            </div>
            <%-- button cancel request--%>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<%-- update popup document --%>
<div id="RemarkRequestCancelDocumentControlAS<%= ClientID %>" class="popup-remark-initiative RemarkRequestCancelDocumentControlAS">
    <div class="remark-content">
        <div class="remark-header">
            <span>กรุณาระบุหมายเหตุการขอเอกสาร
            </span>
        </div>
        <div class="remark-body">
            <label>หมายเหตุ</label>
            <asp:TextBox runat="server" ID="txtRemarkDocumentControlAS" TextMode="MultiLine" Rows="10" CssClass="ticket-allow-editor-everyone form-control initiative-approvel-management ticket-allow-editor" placeholder="กรุณาระบุหมายเหตุ" />
        </div>
        <div class="remark-footer">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <%--อนุมัติเลื่อนขั้น Stage Gate--%>
                    <asp:Button runat="server" Text="Confirm" CssClass="ticket-allow-editor-everyone btn btn-success initiative-approvel-management" ID="btnApprovelRequestDocumentAS"
                        OnClientClick="AGLoading(true);$('.RemarkRequestCancelDocumentControlAS').fadeOut();"
                        OnClick="btnApprovelRequestDocumentAS_Click" Style="margin-right: 10px; width: 150px;" />

                    <input type="button" name="name" id="btncloseModalCancelDocumentAS" value="Cancel" class="ticket-allow-editor-everyone btn btn-danger initiative-approvel-management"
                        style="width: 150px" onclick="$('.RemarkRequestCancelDocumentControlAS').fadeOut();" />

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<%-- update popup document --%>

<div id="RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS<%= ClientID %>" class="popup-remark-initiative RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS">
    <div class="remark-content">
        <div class="remark-header">
            <span>กรุณาระบุหมายเหตุการเปลียนแปลง
            </span>
        </div>
        <div class="remark-body">
            <label>หมายเหตุ</label>
            <asp:TextBox runat="server" ID="txtRemarkInitiativeWeeklyStatusModelControlAS" TextMode="MultiLine" Rows="10" CssClass="ticket-allow-editor-everyone form-control initiative-approvel-management ticket-allow-editor" placeholder="กรุณาระบุหมายเหตุ" />
        </div>
        <div class="remark-footer">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <%--อนุมัติเลื่อนขั้น Stage Gate--%>
                    <asp:Button runat="server" Text="Confirm" CssClass="ticket-allow-editor-everyone btn btn-success initiative-approvel-management" ID="btnApprovelRequestApprvelInitiativeAS"
                        OnClientClick="AGLoading(true);$('.RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS').fadeOut();"
                        OnClick="btnApprovelRequestApprvelInitiative_Click" Style="margin-right: 10px; width: 150px; display: none;" />
                    <%--ไม่อนุมัติเลื่อนขั้น Stage Gate--%>
                    <asp:Button runat="server" Text="Confirm" CssClass="ticket-allow-editor-everyone btn btn-success initiative-approvel-management" ID="btnRejectRequestApprvelInitiativeAS"
                        OnClientClick="AGLoading(true);$('.RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS').fadeOut();"
                        OnClick="btnRejectRequestApprvelInitiative_Click" Style="margin-right: 10px; width: 150px; display: none;" />
                    <%--อนุมัติคำขอCancel Initiative--%>
                    <asp:Button runat="server" Text="Confirm" CssClass="ticket-allow-editor-everyone btn btn-success initiative-approvel-management" ID="btnApprovelRequestCancelInitiativeAS"
                        OnClientClick="AGLoading(true);$('.RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS').fadeOut();"
                        OnClick="btnApprovelRequestCancelInitiative_Click" Style="margin-right: 10px; width: 150px; display: none;" />
                    <%--ไม่อนุมัติคำขอCancel Initiative--%>
                    <asp:Button runat="server" Text="Confirm" CssClass="ticket-allow-editor-everyone btn btn-success initiative-approvel-management" ID="btnRejectRequestCancelInitiativeAS"
                        OnClientClick="AGLoading(true);$('.RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS').fadeOut();"
                        OnClick="btnRejectRequestCancelInitiative_Click" Style="margin-right: 10px; width: 150px; display: none;" />
                    <%--ขอเลือนขั้น Stage Gate--%>
                    <asp:Button runat="server" Text="Confirm" CssClass="ticket-allow-editor-everyone btn btn-success initiative-approvel-management" ID="btnConfirmRequestApprovelInitiativeModelControlAS"
                        OnClientClick="AGLoading(true);$('.RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS').fadeOut();"
                        OnClick="btnConfirmRequestApprovelInitiativeModelControl_Click" Style="margin-right: 10px; width: 150px; display: none;" />
                    <%--อนุมัติลดขั้น Stage Gate--%>
                    <asp:Button runat="server" Text="Confirm" CssClass="ticket-allow-editor-everyone btn btn-success initiative-approvel-management" ID="btnApprovelRequestDowngradeInitiativeAS"
                        OnClientClick="AGLoading(true);$('.RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS').fadeOut();"
                        OnClick="btnApprovelRequestDowngradeInitiative_Click" Style="margin-right: 10px; width: 150px; display: none;" />
                    <%--ไม่อนุมัติลดขั้น Stage Gate--%>
                    <asp:Button runat="server" Text="Confirm" CssClass="ticket-allow-editor-everyone btn btn-success initiative-approvel-management" ID="btnRejectRequestDowngradeInitiativeAS"
                        OnClientClick="AGLoading(true);$('.RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS').fadeOut();"
                        OnClick="btnRejectRequestDowngradeInitiative_Click" Style="margin-right: 10px; width: 150px; display: none;" />

                    <%--ขออนุมัติลดขั้น Stage Gate--%>
                    <asp:Button runat="server" Text="Confirm" CssClass="ticket-allow-editor-everyone btn btn-success initiative-approvel-management" ID="btnRequestDowngradeInitiativeModelControlAS"
                        OnClientClick="AGLoading(true);$('.RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS').fadeOut();"
                        OnClick="btnRequestDowngradeInitiativeModelControl_Click" Style="margin-right: 10px; width: 150px; display: none;" />
                    <%--ขอCancel Initiative--%>
                    <asp:Button runat="server" Text="Confirm" CssClass="ticket-allow-editor-everyone btn btn-success initiative-approvel-management" ID="btnSaveRequestCancelWeeklyStatusModelControlAS"
                        OnClientClick="AGLoading(true);$('.RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS').fadeOut();"
                        OnClick="btnSaveRequestCancelWeeklyStatusModelControl_Click" Style="margin-right: 10px; width: 150px; display: none;" />

                    <%--ขอCancel Request Approval Initiative--%>
                    <asp:Button runat="server" Text="Confirm" CssClass="ticket-allow-editor-everyone btn btn-success initiative-approvel-management" ID="btnSaveCancelRequestApprovalInitiativeModelControlAS"
                        OnClientClick="AGLoading(true);$('.RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS').fadeOut();"
                        OnClick="btnSaveCancelRequestApprovalInitiativeModelControlAS_Click" Style="margin-right: 10px; width: 150px; display: none;" />

                    <%--ขอCancel Request Downgrade Initiative--%>
                    <asp:Button runat="server" Text="Confirm" CssClass="ticket-allow-editor-everyone btn btn-success initiative-approvel-management" ID="btnSaveCancelRequestDowngradeInitiativeModelControlAS"
                        OnClientClick="AGLoading(true);$('.RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS').fadeOut();"
                        OnClick="btnSaveCancelRequestDowngradeInitiativeModelControlAS_Click" Style="margin-right: 10px; width: 150px; display: none;" />

                    <%--ขอCancel Request Cancel Initiative--%>
                    <asp:Button runat="server" Text="Confirm" CssClass="ticket-allow-editor-everyone btn btn-success initiative-approvel-management" ID="btnSaveCancelRequestCancelInitiativeModelControlAS"
                        OnClientClick="AGLoading(true);$('.RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS').fadeOut();"
                        OnClick="btnSaveCancelRequestCancelInitiativeModelControlAS_Click" Style="margin-right: 10px; width: 150px; display: none;" />


                    <input type="button" name="name" id="btncloseModalCancelWeeklyStatusModelControlAS" value="Cancel" class="ticket-allow-editor-everyone btn btn-danger initiative-approvel-management"
                        style="width: 150px" onclick="$('.RemarkRequestCancelInitiativeModalWeeklyStatusModelControlAS').fadeOut();" />

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>

<script>
    function SaveBeforeRequest() {
        try {
            rebindWorkflowDetail();
        } catch (e) {
            window.location.href = window.location.href;
        }
    }
    function reloadInitiativeModalControl() {
        try {
            rebindWorkflowDetail();
        } catch (e) {
            window.location.href = window.location.href;
        }
    }
    function reloadInitiativeModalControlOnSave() {

        try {
            rebindWorkflowDetail();
        } catch (e) {
            window.location.href = window.location.href;
        }
    }

    function reloadTicket() {
        if ($("#btnUpdateLog_FormPostRemark").length > 0) {
            $("#btnUpdateLog_FormPostRemark").click();
        }
    }
</script>