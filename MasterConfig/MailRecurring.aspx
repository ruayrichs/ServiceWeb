<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="MailRecurring.aspx.cs" Inherits="ServiceWeb.MasterConfig.MailRecurring" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-email-recurring").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <button type="button" class="btn btn-primary mb-1 AUTH_MODIFY" onclick="editClick(this);"><i class="fa fa-plus-circle"></i>&nbsp;&nbsp;New E-Mail Recurring</button>
                    <asp:Button ID="btnCreate" runat="server" CssClass="d-none" OnClick="btnEdit_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>

    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">E-Mail Recurring</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                    <thead>
                                        <tr>
                                            <th class="text-nowrap"></th>
                                            <th class="text-nowrap">Active</th>
                                            <th class="text-nowrap">Pop 3 User</th>
                                            <th>Objective</th>                                         
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptItems" runat="server">
                                            <ItemTemplate>

                                                <tr>
                                                    <td class="text-nowrap text-center align-middle">
                                                        <i class="fa fa-edit fa-lg text-dark mx-1 c-pointer AUTH_MODIFY" title="Edit" onclick="editClick(this);"></i>
                                                        <asp:Button ID="btnEdit" runat="server" CssClass="d-none" OnClick="btnEdit_Click" CommandArgument='<%# Eval("BATCH_ID") %>' />
                                                        <i class="fa fa-times-circle-o fa-lg text-danger c-pointer mr-1 AUTH_MODIFY" title="Delete" onclick="confirmDelete(this);"></i>
                                                        <asp:Button ID="btnDelete" runat="server" CssClass="d-none" OnClick="btnDelete_Click" CommandArgument='<%# Eval("BATCH_ID") + "|" + Eval("POP3_CODE") %>' />
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("ACTIVE") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("POP3_USERNAME") %>
                                                    </td>
                                                    <td>
                                                        <%# Eval("REMARKS") %>
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

    <script type="text/javascript">
        function bindingDataTableJS() {
            $("#tableItems").dataTable({
                columnDefs: [{
                    "orderable": false,
                    "targets": [0]
                }],
                "order": [[1, "asc"]]
            });
        }

        function editClick(sender) {
            AGLoading(true);
            $(sender).next().click();
        }

        function confirmDelete(sender) {            
            if (AGConfirm(sender, "Confirm Delete")) {
                AGLoading(true);
                $(sender).next().click();
            }
        }
    </script>

</asp:Content>
