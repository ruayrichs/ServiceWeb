<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="PositionMaster.aspx.cs" Inherits="ServiceWeb.MasterConfig.PositionMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            console.log(onav);
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-prosition-master").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <button type="button" class="btn btn-primary mb-1 AUTH_MODIFY" onclick="$('#master-data').modal('show');"><i class="fa fa-plus-circle"></i>&nbsp;&nbsp;Create</button>
                    <%--<asp:Button ID="btnCreate" OnClick="btnCreate_Click" CssClass="d-none" Text="" runat="server" />--%>
                    <%--<asp:Button Text="Serach" ID="Button1" CssClass="btn btn-primary" OnClick="btnSearch_Click" runat="server" />--%>
                <%--<span class="btn btn-success" onclick="$('#master-data').modal('show');">
                  <i class="fa fa-pencil"></i>&nbsp;&nbsp;Create
                </span>--%>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>

    <div class="card shadow">
        <div class="card-header">
            <h5 class="mb-0">Position Master</h5>
        </div>
        <div class="card-body PANEL-DEFAULT-BUTTON">
            <div class="d-none">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField runat="server" ID="PermissionKey" />
                        <asp:HiddenField runat="server" ID="hddPage_Mode" />
                        <asp:HiddenField ID="hdfEditCode" runat="server" />
                        <asp:Button ID="btnSetCreate" runat="server" CssClass="AUTH_MODIFY" OnClick="btnSetCreate_Click" />
                        <%--<asp:Button ID="btnSearch" runat="server" CssClass="AUTH_MODIFY" OnClick="btnSearch_Click" />--%>
                        <asp:Button runat="server" ID="btnSetEdit" CssClass="AUTH_MODIFY" OnClick="btnSetEdit_Click" OnClientClick="AGLoading(true);" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <%--<button type="button" class="btn btn-info mb-1 AUTH_MODIFY" onclick="searchbtn();"><i class="fa fa-search"></i>&nbsp;&nbsp;Search</button>--%>
            
            <hr />
            <div id="divSearch">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updateList">
                    <ContentTemplate>
                        <div class="table-responsive">
                            <table id="tableItems" class="table table-bordered table-striped table-hover table-sm" style="width: 100%;">
                                <thead>
                                    <tr>
                                        <th class="text-nowrap">Edit</th>
                                        <th class="text-nowrap">Delete</th>
                                        <th class="text-nowrap">Position Code</th>
                                        <th class="text-nowrap">Position Name</th>
                                        <th class="text-nowrap">Description</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptSearch" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td class="text-nowrap text-center align-middle">
                                                    <i class="fa fa-edit fa-lg text-dark mx-1 AUTH_MODIFY" title="Edit" onclick="$(this).next().click();"></i>
                                                                <asp:Button ID="btnEdit" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnEdit_Click" CommandArgument='<%# Eval("PositionCode") %>' />
                                                </td>
                                                <td class="text-nowrap text-center align-middle">
                                                    <i class="fa fa-trash fa-lg text-dark mx-1 AUTH_DELETE" title="Delete" onclick="$(this).next().click();"></i>
                                                                <asp:Button ID="btnDelete" runat="server" CssClass="d-none AUTH_DELETE" OnClick="btnDelete_Click" CommandArgument='<%# Eval("PositionCode") %>' />
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("PositionCode") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("PositionName") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("Description") %>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                        <%--<div style="display: none;" runat="server" id="divTranslaterStatus" clientidmode="Static">[]</div>
                        <div style="display: none;" runat="server" id="divJsonEquipmentList" clientidmode="Static">[]</div>--%>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div class="modal fade" id="master-data">
    <div class="modal-dialog">
        <div class="modal-content">
            <asp:UpdatePanel ID="updatePanelPositionModol" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-header">
                    <h5 class="modal-title" id="modal-header">Position Master</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-lg-12">
                                    <label>
                                        <asp:Label runat="server" ID="label13" Text="Position Code"></asp:Label>
                                    </label>
                                    <span class="requireItem">*</span>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:TextBox ID="txtPositionCodeModol" CssClass="form-control interger-controlset" runat="server" />

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <label>
                                        <asp:Label runat="server" ID="label14" Text="Position Name"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:TextBox ID="txtPositionName" CssClass="form-control interger-control" runat="server" />

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <label>
                                        <asp:Label runat="server" ID="label2" Text="Description"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:TextBox ID="txtDescription" CssClass="form-control interger-control" runat="server" />

                                </div>
                            </div>        
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">
                           Close</button>
                        <asp:Button Text="Save" runat="server" ID="btnCreatePosition" CssClass="btn btn-success"
                            OnClick="btnCreatePosition_Click" OnClientClick="return AGConfirm('ยืนยันการบันทึก');" />
                        <asp:Button Text="Save" runat="server" ID="btnEditPosition" CssClass="btn btn-success" OnClick="btnEditPosition_Click"
                            Visible="false" OnClientClick="return AGConfirm('ยืนยันการบันทึก');" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
    <script>
        $(document).ready(function () {
            $('#tableItems').DataTable({});
        });
        function setDataTable() {
            $('#tableItems').DataTable({});
        };
        function openPermission(PermissionKey) {
            inactiveRequireField();
            $("#<%= PermissionKey.ClientID %>").val(PermissionKey);
                $("#<%= btnSetEdit.ClientID %>").click();
            }
            function openCreate() {
                inactiveRequireField();
                $("#<%= btnSetCreate.ClientID %>").click();
        }
        <%--function searchbtn() {
            inactiveRequireField();
            $("#<%= btnSearch.ClientID %>").click();
            }--%>
    </script>
</asp:Content>
