<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="EmailMessageCriteria.aspx.cs" Inherits="ServiceWeb.MasterConfig.EmailMessageCriteria" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
     <%--<nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <button type="button" class="btn btn-primary mb-1 AUTH_MODIFY" onclick="$(this).next().click();"><i class="fa fa-plus-circle"></i>&nbsp;&nbsp;สร้างข้อมูลหลักข้อความในอีเมล์</button>
                    <asp:Button ID="btnCreate" OnClick="btnCreate_Click" CssClass="d-none" Text="" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>--%>
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-email-message").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Email Template</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                    <thead>
                                        <tr>
                                            <th class="text-nowrap"></th>
                                            <th class="text-nowrap">Code</th>
                                            <th>Topik email</th>
                                            <th>Event</th>
                                            <th class="text-nowrap">Create on</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptItems" runat="server">
                                            <ItemTemplate>

                                                <tr class="c-pointer">
                                                    <td class="text-nowrap text-center align-middle">
                                                        <i class="fa fa-edit fa-lg text-dark mx-1 AUTH_MODIFY" title="Edit" onclick="$(this).next().click();"></i>                                                        
                                                        <asp:Button ID="btnEdit" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnEdit_Click" CommandArgument='<%# Eval("rowkey") %>' />
                                                    </td>
                                                    <td>
                                                        <%# Eval("messageCode") %>
                                                    </td>
                                                    <td>
                                                        <%# Eval("emailSubject") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("eventname") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("displayCreatedOn") %>
                                                       <%-- <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("CREATED_ON").ToString()) %>--%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <script>
        function bindingDataTableJS() {
            $("#tableItems").dataTable({
                columnDefs: [{
                    "orderable": false,
                    "targets": [0]
                }],
                "order": [[1, "asc"]]
            });
        }
    </script>

</asp:Content>
