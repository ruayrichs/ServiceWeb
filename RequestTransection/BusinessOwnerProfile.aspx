<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="BusinessOwnerProfile.aspx.cs" Inherits="ServiceWeb.RequestTransection.BusinessOwnerProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-business-owner-profile").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>

    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h4 class="mb-0">Business Owner Profile</h4>
                </div>
                <div class="card-body">
                    <div>
                        <div class="form-row">
                            <div class="form-group col-sm-6">
                                <label>Business Owner</label>
                                <asp:Label ID="LabelBusinessOwner" runat="server" CssClass="form-control form-control-sm" style="background-color: #eee;"></asp:Label>
                            </div>
                            <div class="form-group col-sm-6">
                                <label>Corporate Permission Key</label>
                                <asp:Label ID="LabelCopPerKey" runat="server" CssClass="form-control form-control-sm" style="background-color: #eee;"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div>
                        <div class="card border-info mb-3 ">
                            <div class=" card-header bg-info text-white">
                                <b>History List Corporate Permission Key</b>
                            </div>
                            <div class="card-body PANEL-DEFAULT-BUTTON">
                                <div class="table-responsive">
                                    <div id="tableArea-1">
                                        <asp:UpdatePanel ID="udpnListCopPerKeyItems" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="table-responsive">
                                                    <table id="tableListCopPerKeyItems" class="table table-bordered table-striped table-hover table-sm">
                                                        <thead>
                                                            <tr>
                                                                <th class="text-nowrap">Line Number</th>
                                                                <th class="text-nowrap">Corporate Permission Key</th>
                                                                <th class="text-nowrap">Statuse</th>
                                                                <th class="text-nowrap">Gennerate By</th>
                                                                <th class="text-nowrap">Gennerate Datatime</th>
                                                                <th class="text-nowrap">Update By</th>
                                                                <th class="text-nowrap">Update Datatime</th>

                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:Repeater ID="rptListCopPerKeyItems" runat="server">
                                                                <ItemTemplate>
                                                                    <tr class="table-hover">
                                                                        <td class="text-nowrap"><%# Eval("LineNumber") %></td>
                                                                        <td class="text-nowrap"><%# Eval("CorporatePermissionKey") %></td>
                                                                        <td class="text-nowrap"><%# Eval("Active") %></td>
                                                                        <td class="text-nowrap"><%# Eval("FirstName") + " " + Eval("LastName")%></td>
                                                                        <td class="text-nowrap"><%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("Created_On").ToString()) %></td>
                                                                        <td class="text-nowrap"><%# Eval("Updated_By") %></td>
                                                                        <td class="text-nowrap"><%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("Updated_On").ToString()) %></td>
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
                        </div>
                    </div>
                    <div>
                        <div class="card  mb-3 ">
                            <div class=" card-header  ">
                                <b>List Application Permission Key</b>
                            </div>
                            <div class="card-body PANEL-DEFAULT-BUTTON">
                                <div class="table-responsive">
                                    <div id="tableArea-2">
                                        <asp:UpdatePanel ID="udpnListAppPerKeyItems" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="table-responsive">
                                                    <table id="tableListAppPerKeyItems" class="table table-bordered table-striped table-hover table-sm">
                                                        <thead>
                                                            <tr>
                                                                <th class="text-nowrap">Application ID</th>
                                                                <th class="text-nowrap">Application Permission Key</th>
                                                                <th class="text-nowrap">Statuse</th>
                                                                <th class="text-nowrap">Gennerate By</th>
                                                                <th class="text-nowrap">Gennerate Datatime</th>
                                                                <th class="text-nowrap">Updated By</th>
                                                                <th class="text-nowrap">Updated Datatime</th>

                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:Repeater ID="rptListAppPerKeyItem" runat="server">
                                                                <ItemTemplate>
                                                                    <tr class="table-hover">
                                                                        <td class="text-nowrap"><%# Eval("ApplicationID") %></td>
                                                                        <td class="text-nowrap"><%# Eval("ApplicationPermissionKey") %></td>
                                                                        <td class="text-nowrap"><%# Eval("Active") %></td>
                                                                        <td class="text-nowrap"><%# Eval("FirstName") + " " + Eval("LastName") %></td>
                                                                        <td class="text-nowrap"><%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("Created_On").ToString()) %></td>
                                                                        <td class="text-nowrap"><%# Eval("Updated_By") %></td>
                                                                        <td class="text-nowrap"><%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("Updated_On").ToString()) %></td>
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
                        </div>
                    </div>
                    <div>
                        <div class="card border-warning mb-3 ">
                            <div class=" card-header bg-warning text-white">
                                <b>List request activation Application</b>
                            </div>
                            <div class="card-body PANEL-DEFAULT-BUTTON">
                                <div class="table-responsive">
                                    <div id="tableArea-3">
                                        <asp:UpdatePanel ID="udpnListReqActiveAppItems" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="table-responsive">
                                                    <table id="tableListReqActiveAppItems" class="table table-bordered table-striped table-hover table-sm">
                                                        <thead>
                                                            <tr>
                                                                <th class="text-nowrap">Req. Date</th>
                                                                <th class="text-nowrap">App ID</th>
                                                                <th class="text-nowrap">App. Permission Key</th>
                                                                <th class="text-nowrap">Cop. Permission key</th>
                                                                <th class="text-nowrap">Acception Status</th>
                                                                <th class="text-nowrap">Acception By</th>
                                                                <th class="text-nowrap">Acception Date</th>
                                                                <th class="text-nowrap">Activation Status</th>
                                                                <th class="text-nowrap">Activation Date</th>
                                                                <th class="text-nowrap">Email</th>
                                                                <th class="text-nowrap">Remark</th>

                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:Repeater ID="rptListReqActiveAppItems" runat="server">
                                                                <ItemTemplate>
                                                                    <tr class="table-hover">
                                                                        <td class="text-nowrap">
                                                                            <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("RequestDateTime").ToString())%>
                                                                        </td>

                                                                        <td class="text-nowrap">
                                                                            <%# Eval("ApplicationID") %>
                                                                        </td>
                                                                        <td class="text-nowrap">
                                                                            <%# Eval("ApplicationPermissionKey") %>
                                                                        </td>
                                                                        <td class="text-nowrap">
                                                                            <%# Eval("CorporatePermissionKey") %>
                                                                        </td>
                                                                        <td class="text-nowrap">
                                                                            <%# Eval("AcceptionStatus") %>
                                                                        </td>
                                                                        <td class="text-nowrap">
                                                                            <%# Eval("FirstName") + " " + Eval("LastName") %>
                                                                        </td>
                                                                        <td class="text-nowrap">
                                                                            <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("AcceptionDateTime").ToString()) %>
                                                                        </td>
                                                                        <td class="text-nowrap">
                                                                            <%# Eval("ActivationStatus") %>
                                                                        </td>
                                                                        <td class="text-nowrap">
                                                                            <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("ActivationDateTime").ToString()) %>
                                                                        </td>
                                                                        <td class="text-nowrap">
                                                                            <%# Eval("Email") %>
                                                                        </td>
                                                                        <td class="text-nowrap">
                                                                            <%# Eval("Remark") %>
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
                        </div>
                    </div>




                </div>

            </div>

        </div>

    </div>

    <script>

        function bindingDataTableJSCopPerKey() {
            $("#tableListCopPerKeyItems").dataTable({
                columnDefs: [{
                    "orderable": false,
                    "targets": [0]
                }],
                "order": [[1, "asc"]]
            });
        }
        function bindingDataTableJSAppPerKey() {
            $("#tableListAppPerKeyItems").dataTable({
                columnDefs: [{
                    "orderable": false,
                    "targets": [0]
                }],
                "order": [[1, "asc"]]
            });
        }
        function bindingDataTableJSReqApp() {
            $("#tableListReqActiveAppItems").dataTable({
                columnDefs: [{
                    "orderable": false,
                    "targets": [0]
                }],
                "order": [[1, "asc"]]
            });
        }
    </script>
</asp:Content>
