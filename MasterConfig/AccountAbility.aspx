<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="AccountAbility.aspx.cs" Inherits="ServiceWeb.MasterConfig.AccountAbility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-account-ability").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Add new Level</h4>
                    <button type="button" class="close" data-dismiss="modal">×</button>
                </div>
                <div class="modal-body">
                    <b>Event Description</b>
                    <input type="text" id="txtBoxEventDesc" class="form-control" runat="server" />
                </div>
                <div class="modal-footer">
                    <asp:Button CssClass="btn btn-success AUTH_MODIFY" OnClick="btnSave_Click" OnClientClick="AGLoading(true);" runat="server" Text="Add" />
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <button type="button" class="btn btn-primary mb-1 AUTH_MODIFY" onclick="$(this).next().click();">
                        <i class="fa fa-plus-circle"></i>&nbsp;&nbsp;Add New Level
                    </button>
                    <asp:Button data-toggle="modal" data-target="#myModal" class="btn btn-success mb-2 AUTH_MODIFY d-none" runat="server" Text="New" />
                    <button type="button" class="btn btn-danger mb-1 AUTH_MODIFY" onclick="$(this).next().click();">
                        <i class="fa fa-trash"></i>&nbsp;&nbsp;Remove last Level
                    </button>
                    <asp:Button class="btn btn-danger mb-2 AUTH_MODIFY d-none" runat="server" OnClientClick="return AGConfirm(this,'Delete, Are you sure? !!');AGLoading(true);"
                        OnClick="btnDelete_Click" Text="Delete" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>
    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Workflow Level</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="udpMasterConfig" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table id="tableMaster" class="table table-striped table-bordered table-sm">
                                    <tr>
                                        <th>Event Code
                                        </th>
                                        <th>Event Description
                                        </th>
                                        <th>Event Type
                                        </th>
                                        <th>Level From
                                        </th>
                                        <th>Level To
                                        </th>
                                        <th>SEQ
                                        </th>

                                    </tr>
                                    <asp:Repeater ID="tableData" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" Text='<%# Eval("EventCode") %>'></asp:Label>


                                                </td>

                                                <td>
                                                    <asp:Label runat="server" Text='<%# Eval("EventDesc") %>'></asp:Label>

                                                </td>

                                                <td>
                                                    <asp:Label runat="server" Text='<%# Eval("EventType") %>'></asp:Label>

                                                </td>

                                                <td>
                                                    <asp:Label runat="server" Text='<%# Eval("LevelFrom") %>'></asp:Label>

                                                </td>

                                                <td>
                                                    <asp:Label runat="server" Text='<%# Eval("LevelTo") %>'></asp:Label>

                                                </td>
                                                <td>
                                                    <asp:Label ID="txtSEQ" runat="server" Text=' <%# Eval("SEQ") %>'></asp:Label>

                                                </td>

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
</asp:Content>
