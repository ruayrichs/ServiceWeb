<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="RequestPermissionKeyList.aspx.cs" Inherits="ServiceWeb.MasterConfig.RequestPermissionKeyList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-request-permission-key").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <div class="card shadow">
        <div class="card-header">
            <h5 class="mb-0">Request Permission Key</h5>
        </div>
        <div class="card-body PANEL-DEFAULT-BUTTON">
            <div class="form-row">
                <div class="form-group col-sm-6">
                    <label>Chanel</label>
                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlChanel">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Email" Value="1" />
                        <asp:ListItem Text="Web" Value="2" />
                        <asp:ListItem Text="System" Value="3" />
                    </asp:DropDownList>
                </div>
                <div class="form-group col-sm-6">
                    <label>
                        Status
                    </label>
                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlStatus">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Active" Value="true" />
                        <asp:ListItem Text="Inactive" Value="false" />
                    </asp:DropDownList>
                </div>
                <div class="form-group col-md-6 col-sm-12">
                    <label>
                        Start Date
                    </label>
                    <div class="input-group">
                        <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker"
                            ID="txtStartDate"
                            placeholder="dd/mm/yyy" />
                        <span class="input-group-append hand">
                            <i class="fa fa-calendar input-group-text"></i>
                        </span>
                    </div>
                </div>
                <div class="form-group col-md-6 col-sm-12">
                    <label>
                        End Date
                    </label>
                    <div class="input-group">
                        <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker"
                            ID="txtEndDate"
                            placeholder="dd/mm/yyy" />
                        <span class="input-group-append hand">
                            <i class="fa fa-calendar input-group-text"></i>
                        </span>
                    </div>
                </div>
            </div>
            <div class="d-none">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField runat="server" ID="PermissionKey" />
                        <asp:HiddenField runat="server" ID="hddPage_Mode" />
                        <asp:HiddenField ID="hdfEditCode" runat="server" />
                        <asp:Button ID="btnSetCreate" runat="server" CssClass="AUTH_MODIFY" OnClick="btnSetCreate_Click" />
                        <asp:Button ID="btnSearch" runat="server" CssClass="AUTH_MODIFY" OnClick="btnSearch_Click" />
                        <asp:Button runat="server" ID="btnSetEdit" CssClass="AUTH_MODIFY" OnClick="btnSetEdit_Click" OnClientClick="AGLoading(true);" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <button type="button" class="btn btn-info mb-1 AUTH_MODIFY" onclick="searchbtn();"><i class="fa fa-search"></i>&nbsp;&nbsp;Search</button>
            <button type="button" class="btn btn-primary mb-1 AUTH_MODIFY" onclick="openCreate();"><i class="fa fa-plus-circle"></i>&nbsp;&nbsp;Create</button>


            <hr />
            <div id="divSearch">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updateList">
                    <ContentTemplate>
                        <div class="table-responsive">
                            <table id="tableItems" class="table table-bordered table-striped table-hover table-sm" style="width: 100%;">
                                <thead>
                                    <tr>
                                        <th class="text-nowrap">Edit</th>
                                        <th class="text-nowrap">IPAddress</th>
                                        <th class="text-nowrap">Start Date</th>
                                        <th class="text-nowrap">End Date</th>
                                        <th class="text-nowrap">Program Name</th>
                                        <th class="text-nowrap">Chanel Request</th>
                                        <th class="text-nowrap">Active</th>
                                        <th class="text-nowrap">Employee Code</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptSearch" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td class="text-nowrap text-center align-middle">
                                                    <a class="AUTH_MODIFY" href="#" onclick="openPermission('<%# Eval("PermissionKey") %>')">
                                                        <i class="fa fa-edit fa-lg text-dark mx-1"></i>
                                                    </a>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("IPAddress") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("StartDate").ToString()) %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("EndDate").ToString()) %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("ProgramName") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# (string)Eval("ChanelRequest") =="1" ? "E-mail" :
                                                                    (string)Eval("ChanelRequest") =="2" ? "Web" :
                                                                    "System"%>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# (string)Eval("Active") =="true" ? "Active" : "Inactive"%>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("EmployeeCode") %>
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
    <script>
        $(document).ready(function () {
            $('#tableItems').DataTable({});
        });
        function openPermission(PermissionKey) {
            inactiveRequireField();
            $("#<%= PermissionKey.ClientID %>").val(PermissionKey);
                $("#<%= btnSetEdit.ClientID %>").click();
            }
            function openCreate() {
                inactiveRequireField();
                $("#<%= btnSetCreate.ClientID %>").click();
        }
        function searchbtn() {
            inactiveRequireField();
            $("#<%= btnSearch.ClientID %>").click();
            }
    </script>
</asp:Content>
