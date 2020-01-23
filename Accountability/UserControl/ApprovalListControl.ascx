<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApprovalListControl.ascx.cs" Inherits="ServiceWeb.Accountability.UserControl.ApprovalListControl" %>

<script>
    function refreshPageApprove() {
        $("#btnRefreshDataApprove").click();
    }
    function refreshPageApproveNoload() {
        $("#btnRefreshDataApproveNoload").click();
    }
</script>

<div class="hide" style="display: none !important;">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCodeidentityInitiative">
        <ContentTemplate>
            <asp:Button ID="btnRefreshDataApprove" ClientIDMode="Static" OnClick="btnRefreshDataApprove_Click" OnClientClick="agroLoading(true);" runat="server" />
            <asp:Button ID="btnRefreshDataApproveNoload" ClientIDMode="Static" OnClick="btnRefreshDataApproveNoload_Click" runat="server" />
            <asp:HiddenField runat="server" ID="hddWorkGroupCode" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="ini-approval-zone">

    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpTableUpgrade">
        <ContentTemplate>
            <asp:Panel runat="server" ID="panelListUpgrade" CssClass="card mb-3">
                <div class="card-header">
                    <b>Document for step up approval</b>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table id="tableItemsstepup" class="table table-sm table-hover table-striped" style="background-color: #fff; margin: 0;">
                            <thead>
                                <tr>
                                    <th style="width: 100px">Doc Number</th>
                                    <th>Work Flow Status</th>
                                    <th>Accountability</th>
                                    <th style="width: 150px">Requested On</th>
                                    <th style="width: 80px">Approve</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater runat="server" ID="rptListUpgrade">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <b>
                                                    <%# Eval("DocNumberDisplay") %>
                                                </b>
                                            </td>
                                            <td>
                                                <%# Eval("WorkFlowStatus") %>
                                                <%-- <%# GetStatusStateGate(Eval("AOBJECTLINK").ToString()) %>--%>
                                            </td>
                                            <td>
                                                <%# Eval("SUBPROJECT") %>
                                            </td>
                                            <td>
                                                <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("DateTime").ToString()) %>
                                            </td>
                                            <td style="text-align: center;">
                                                <i class="fa fa-gavel" style="cursor: pointer;" onclick="$(this).next().click();"></i>
                                                <asp:Button Text="" runat="server" Style="display: none !important;" ID="btnOpenDocument"
                                                    OnClick="btnOpenDocument_Click" OnClientClick="AGLoading(true);"
                                                    CommandArgument='<%# Eval("DOCNUMBER") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <%--<asp:Panel runat="server" Visible="false" ID="panelNoListUpgrade">
                                    <tr>
                                        <td style="text-align: center;" colspan="4">ไม่พบข้อมูล
                                        </td>
                                    </tr>
                                </asp:Panel>--%>
                            </tbody>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpTableDowngrade">
        <ContentTemplate>
            <asp:Panel runat="server" ID="panelDowngrade" CssClass="card mb-3 d-none">
                <div class="card-header">
                    <b>Document for step down approval</b>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table id="tableItemsstepdown" class="table table-sm table-hover table-striped" style="background-color: #fff; margin: 0;">
                            <thead>
                                <tr>
                                    <th style="width: 100px">Doc Number</th>
                                    <th>Level</th>
                                    <th style="width: 150px">Requested On</th>
                                    <th style="width: 80px">Approve</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater runat="server" ID="rptlistDowngrade">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <b>
                                                    <%# Eval("DocNumberDisplay") %>
                                                </b>
                                            </td>
                                            <td>
                                                <%# Eval("WorkFlowStatus") %>
                                                <%-- <%# GetStatusStateGate(Eval("AOBJECTLINK").ToString()) %>--%>
                                            </td>
                                            <td>
                                                <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("DateTime").ToString()) %>
                                            </td>
                                            <td style="text-align: center;">
                                                <i class="fa fa-gavel" style="cursor: pointer;" onclick="$(this).next().click();"></i>
                                                <asp:Button Text="" runat="server" Style="display: none !important;" ID="btnOpenDocument"
                                                    OnClick="btnOpenDocument_Click" OnClientClick="AGLoading(true);"
                                                    CommandArgument='<%# Eval("DOCNUMBER") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <%--<asp:Panel runat="server" Visible="false" ID="panelNolistDowngrade">
                                    <tr>
                                        <td style="text-align: center;" colspan="4">ไม่พบข้อมูล
                                        </td>
                                    </tr>
                                </asp:Panel>--%>
                            </tbody>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpTableCancel">
        <ContentTemplate>
            <asp:Panel runat="server" ID="panelCancel" CssClass="card mb-3 d-none">
                <div class="card-header">
                    <b>Document for cancellation approval</b>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table id="tableItemsecancelapproval" class="table table-sm table-hover table-striped" style="background-color: #fff; margin: 0;">
                            <thead>
                                <tr>
                                    <th style="width: 100px">Doc Number</th>
                                    <th>Level</th>
                                    <th style="width: 150px">Requested On</th>
                                    <th style="width: 80px">Approve</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater runat="server" ID="rptListCancel">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <b>
                                                    <%# Eval("DocNumberDisplay") %>
                                                </b>
                                            </td>
                                            <td>
                                                <%# Eval("WorkFlowStatus") %>
                                                <%-- <%# GetCurrentStateGate(Eval("AOBJECTLINK").ToString()) %>--%>
                                            </td>
                                            <td>
                                                <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("DateTime").ToString()) %>
                                            </td>
                                            <td style="text-align: center;">
                                                <i class="fa fa-gavel" style="cursor: pointer;" onclick="$(this).next().click();"></i>
                                                <asp:Button Text="" runat="server" Style="display: none !important;" ID="btnOpenDocument"
                                                    OnClick="btnOpenDocument_Click" OnClientClick="AGLoading(true);"
                                                    CommandArgument='<%# Eval("DOCNUMBER") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <%--<asp:Panel runat="server" Visible="false" ID="panelNoListCancel">
                                    <tr>
                                        <td style="text-align: center;" colspan="4">ไม่พบข้อมูล
                                        </td>
                                    </tr>
                                </asp:Panel>--%>
                            </tbody>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpMyNextApproval">
        <ContentTemplate>
            <asp:Panel runat="server" ID="panelListMyNextApproval" CssClass="card mb-3">
                <div class="card-header">
                    <b>Next My Document approval</b>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table id="tableItemsenextapproval" class="table table-sm table-hover table-striped" style="background-color: #fff; margin: 0;">
                            <thead>
                                <tr>
                                    <th style="width: 100px">Doc Number</th>
                                    <th>Work Flow Status</th>
                                    <th>Accountability</th>
                                    <th style="width: 150px">Requested On</th>
                                    <th style="width: 80px">View</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater runat="server" ID="rptListMyNextApproval">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <b>
                                                    <%# Eval("DocNumberDisplay") %>
                                                </b>
                                            </td>
                                            <td>
                                                <%# Eval("WorkFlowStatus") %>
                                                <%-- <%# GetStatusStateGate(Eval("AOBJECTLINK").ToString()) %>--%>
                                            </td>
                                            <td>
                                                <%# Eval("SUBPROJECT") %>
                                            </td>
                                            <td>
                                                <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("DateTime").ToString()) %>
                                            </td>
                                            <td style="text-align: center;">
                                                <i class="fa fa-search" style="cursor: pointer;" onclick="$(this).next().click();"></i>
                                                <asp:Button Text="" runat="server" Style="display: none !important;" ID="btnOpenDocument"
                                                    OnClick="btnOpenDocument_Click" OnClientClick="AGLoading(true);"
                                                    CommandArgument='<%# Eval("DOCNUMBER") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <%--<asp:Panel runat="server" Visible="false" ID="panelNoListNextApproval">
                                    <tr>
                                        <td style="text-align: center;" colspan="4">ไม่พบข้อมูล
                                        </td>
                                    </tr>
                                </asp:Panel>--%>
                            </tbody>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpListMyApproved">
        <ContentTemplate>
            <asp:Panel runat="server" ID="panelListMyApproved" CssClass="card mb-3">
                <div class="card-header">
                    <b>My Document Approved</b>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table id="tableItemsemydocumentpproval" class="table table-sm table-hover table-striped" style="background-color: #fff; margin: 0;">
                            <thead>
                                <tr>
                                    <th style="width: 100px">Doc Number</th>
                                    <th>Work Flow Status</th>
                                    <th>Accountability</th>
                                    <th style="width: 150px">Requested On</th>
                                    <th style="width: 80px">View</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater runat="server" ID="rptListMyApproved">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <b>
                                                    <%# Eval("DocNumberDisplay") %>
                                                </b>
                                            </td>
                                            <td>
                                                <%# Eval("WorkFlowStatus") %>
                                                <%-- <%# GetStatusStateGate(Eval("AOBJECTLINK").ToString()) %>--%>
                                            </td>
                                            <td>
                                                <%# Eval("SUBPROJECT") %>
                                            </td>
                                            <td>
                                                <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("DateTime").ToString()) %>
                                            </td>
                                            <td style="text-align: center;">
                                                <i class="fa fa-search" style="cursor: pointer;" onclick="$(this).next().click();"></i>
                                                <asp:Button Text="" runat="server" Style="display: none !important;" ID="btnOpenDocument"
                                                    OnClick="btnOpenDocument_Click" OnClientClick="AGLoading(true);"
                                                    CommandArgument='<%# Eval("DOCNUMBER") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <%--<asp:Panel runat="server" Visible="false" ID="panelNoListMyApproved">
                                    <tr>
                                        <td style="text-align: center;" colspan="4">ไม่พบข้อมูล
                                        </td>
                                    </tr>
                                </asp:Panel>--%>
                            </tbody>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<script>
    function toggleShowTableListApproval(obj) {
        var parentTab = $(obj).closest(".panel-group-table");
        $(parentTab).each(function () {
            $(this).find(".card-body").slideToggle();
        });
    }
    function hideIsNoApproval() {

    }
    function bindingDataTableStepUp() {
        $("#tableItemsstepup").dataTable({
            columnDefs: [{
                "orderable": false,
                "targets": [0]
            }],
            "order": [[1, "asc"]]
        });
    }
    function bindingDataTableStepDown() {
        $("#tableItemsstepdown").dataTable({
            columnDefs: [{
                "orderable": false,
                "targets": [0]
            }],
            "order": [[1, "asc"]]
        });
    }
    function bindingDataTableNextApproval() {
        $("#tableItemsenextapproval").dataTable({
            columnDefs: [{
                "orderable": false,
                "targets": [0]
            }],
            "order": [[1, "asc"]]
        });
    }
    function bindingDataTableCancelApproval() {
        $("#tableItemsecancelapproval").dataTable({
            columnDefs: [{
                "orderable": false,
                "targets": [0]
            }],
            "order": [[1, "asc"]]
        });
    }
    function bindingDataTableMyDocument() {
        $("#tableItemsemydocumentpproval").dataTable({
            columnDefs: [{
                "orderable": false,
                "targets": [0]
            }],
            "order": [[1, "asc"]]
        });
    }
    function goToEdit(url) {
                    window.open(url, '_blank', 'location=yes,height=840,width=1100,scrollbars=yes,status=yes');
                  
                }
</script>
