<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="UserManagementCriteria.aspx.cs" Inherits="ServiceWeb.crm.Master.UserManagement.UserManagementCriteria" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-user-management").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>

   <nav class="navbar nav-header-action sticky-top bg-white <%= IsAllFeature ? "" : "d-none" %>" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <% if(IsAllFeature) { %> 
                    <button type="button" class="btn btn-primary mb-1 AUTH_MODIFY" onclick="$(this).next().click();"><i class="fa fa-plus-circle"></i>&nbsp;&nbsp;Create</button>
                    <asp:Button ID="btnCreate" OnClick="btnCreate_Click" CssClass="d-none" Text="" runat="server" />
                    <% } %>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>
    


    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">User Management</h5>
                </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="col-md-12">
                            <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="table-responsive">
                                        <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                            <thead>
                                                <tr>
                                                    <th class="text-nowrap" style="width: 50px"></th>
                                                    <th class="text-nowrap">Username</th>
                                                    <th class="text-nowrap">Name</th>
                                                    <th class="text-nowrap">Department</th>
                                                    <th class="text-nowrap">Start date</th>
                                                    <th class="text-nowrap">End date</th>
                                                    <th class="text-nowrap">Status</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="rptItems" runat="server">
                                                    <ItemTemplate>

                                                        <tr class="c-pointer">
                                                            <td class="text-nowrap text-center align-middle">
                                                                <i class="fa fa-edit fa-lg text-dark mx-1 AUTH_MODIFY" title="Edit" onclick="$(this).next().click();"></i>
                                                                <asp:Button ID="btnEdit" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnEdit_Click" CommandArgument='<%# Eval("userid") %>' />
                                                            </td>
                                                            <td>
                                                                <%# Eval("userid") %>
                                                            </td>
                                                            <td>
                                                                <%# Eval("FirstName") + " " + Eval("LastName") %>
                                                            </td>
                                                            <td>
                                                                <%# Eval("Department") %>
                                                            </td>
                                                            <td class="text-nowrap">
                                                                <%# String.IsNullOrEmpty(Convert.ToString(Eval("startdate"))) ? "" : Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Convert.ToString(Eval("startdate")).Substring(0,8)) %>
                                                            </td>
                                                            <td class="text-nowrap">
                                                                <%# String.IsNullOrEmpty(Convert.ToString(Eval("enddate"))) ? "" : Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Convert.ToString(Eval("enddate")).Substring(0,8)) %>
                                                            </td>
                                                            <td class="text-nowrap">
                                                                <%# Convert.ToString(Eval("USERSTATUS")) == "A" ?"Active":"Inactive" %>
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
