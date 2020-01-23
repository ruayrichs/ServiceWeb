<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="ManageUserSessionApp.aspx.cs" Inherits="ServiceWeb.MasterConfig.ManageUserSessionApp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-sessionmanagement").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>

    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Session Management</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-row">
                                <div class="form-group col-md-6">
                                    <label>From</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control form-control-sm date-picker"></asp:TextBox>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="from-group col-md-6">
                                    <label>To</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control form-control-sm date-picker"></asp:TextBox>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="from-group col-md-6">
                                    <asp:Button Text="Search" runat="server" CssClass="btn btn-primary" ID="btnSearchData"
                                        OnClick="btnSearchData_Click" OnClientClick="AGLOading(true);" />
                                    <asp:Button Text="Remove Session" runat="server" ID="btnRemoveSession" CssClass="btn btn-danger"
                                        OnClick="btnRemoveSession_Click" OnClientClick="AGLoading(true);" />
                                </div>
                            </div>

                            <br />
                            <script>
                                function CheckAllItem(obj) {
                                    if ($(obj).prop("checked")) {
                                        $(obj).closest('table').find('.chk-remove-session').find('input').prop('checked', true);
                                    } else {
                                        $(obj).closest('table').find('.chk-remove-session').find('input').prop('checked', false);
                                    }
                                }
                            </script>
                            <div class="table-responsive">
                                <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                    <thead>
                                        <tr>
                                            <th class="text-nowrap text-center">
                                                <input type="checkbox" name="" value="" 
                                                    onclick="CheckAllItem(this);" />
                                            </th>
                                            <th>Session ID</th>
                                            <th class="text-nowrap">User Name</th>
                                            <th class="text-nowrap">Focusone App Port</th>
                                            <th class="text-nowrap">Date Time</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptItems" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="text-nowrap text-center align-middle">
                                                        <asp:CheckBox Text="" runat="server" ID="chkRemoveSession" CssClass="chk-remove-session" />
                                                        <asp:HiddenField runat="server" ID="hddSessionID" Value='<%# Eval("SESSIONID") %>' />
                                                        <asp:HiddenField runat="server" ID="hddUserName" Value='<%# Eval("USERNAME") %>' />
                                                        <asp:HiddenField runat="server" ID="hddTerminal" Value='<%# Eval("TERMINAL") %>' />
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("SESSIONID") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("USERNAME") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("TERMINAL") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("DateTimeSession") %>
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
</asp:Content>
