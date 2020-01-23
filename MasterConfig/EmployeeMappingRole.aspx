<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="EmployeeMappingRole.aspx.cs" Inherits="ServiceWeb.MasterConfig.EmployeeMappingRole" %>

<%@ Register Src="~/widget/usercontrol/AutoCompleteControl.ascx" TagPrefix="uc1" TagName="AutoCompleteControl" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEmployee.ascx" TagPrefix="uc1" TagName="AutoCompleteEmployee" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-employee-permission").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <contenttemplate>
                <%--<button type="button" class="btn btn-primary mb-1" data-toggle="modal" data-target="#modal-master-config"><i class="fa fa-plus-circle"></i>&nbsp;&nbsp;New Role Mapping</button>--%>
                <button type="button" class="btn btn-primary mb-1" onclick="openCreate();"><i class="fa fa-plus-circle"></i>&nbsp;&nbsp;New Employee Permission</button>
            </contenttemplate>
        </div>
    </nav>

    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Role Mapping</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="updmaprole" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table id="tablemappingrole" class="table table-bordered table-striped table-hover table-sm">
                                    <thead>
                                        <tr>
                                            <th class="text-nowrap"></th>
                                            <th class="text-nowrap">Employee Code</th>
                                            <th>Employee Name</th>
                                            <%--<th class="text-nowrap">Role Code</th>--%>
                                            <th class="text-nowrap">Role Name</th>
                                            <th class="text-nowrap">Created By</th>
                                            <th class="text-nowrap">Created On</th>
                                            <th class="text-nowrap">Updated By</th>
                                            <th class="text-nowrap">Updated On</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        
                                        <asp:Repeater ID="rptItems" runat="server">
                                            <ItemTemplate>
                                                <tr class="c-pointer" data-key="<%# Eval("EmployeeCode") %>" data-role="<%# Eval("RoleCode") %>" data-owner="<%# Eval("OwnerService") %>">
                                                    <td class="text-nowrap text-center align-middle">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <i class="fa fa-edit fa-lg text-dark mx-1" title="Edit" onclick="openEdit('<%# Eval("EmployeeCode") %>','<%# Eval("RoleCode") %>','<%# Eval("OwnerService") %>');"></i>
                                                                <i class="fa fa-times-circle-o fa-lg text-danger mr-1" title="Delete" onclick="confirmDelete(this);"></i>
                                                                <asp:Button ID="btnDelete" runat="server" CssClass="d-none" OnClick="btnDelete_Click"
                                                                    CommandArgument='<%# Eval("EmployeeCode") %>' OnClientClick="AGLoading(true);" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td class="text-nowrap" onclick="openEditRow(this);">
                                                        <%# Eval("EmployeeCode") %>
                                                    </td>
                                                    <td onclick="openEditRow(this);">
                                                        <%# Eval("FirstName") %> &nbsp; &nbsp; <%# Eval("LastName") %>
                                                    </td>
                                                    <%--<td class="text-nowrap" onclick="openEditRow(this);">
                                                        <%# Eval("RoleCode") %>
                                                    </td>--%>
                                                    <td class="text-nowrap" onclick="openEditRow(this);">
                                                        <%# Eval("RoleName") %>
                                                    </td>
                                                    <td class="text-nowrap" onclick="openEditRow(this);">
                                                        <%# Eval("CREATED_BY") %>
                                                    </td>
                                                    <td class="text-nowrap" onclick="openEditRow(this);">
                                                        <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("CREATED_ON").ToString()) %>
                                                    </td>
                                                    <td class="text-nowrap" onclick="openEditRow(this);">
                                                        <%# Eval("UPDATED_BY") %>
                                                    </td>
                                                    <td class="text-nowrap" onclick="openEditRow(this);">
                                                        <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("UPDATED_ON").ToString()) %>
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


    <!-- Modal -->
    <div class="modal fade" id="modal-master-config" tabindex="-1" role="dialog" aria-labelledby="modal-header" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modal-header">New Role Mapping</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:Panel runat="server">
                        <asp:UpdatePanel ID="updmodal" runat="server" UpdateMode="Conditional">
                            <contenttemplate>
                                <div class="form-group" style="padding-bottom: 10px;">
                                    <label for="sel1">Role</label>
                                    <asp:DropDownList ID="ddl_role_code" runat="server" class="form-control form-control-sm required"
                                        ClientIDMode="Static">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group" style="padding-bottom: 10px;">
                                    <label for="sel1">Owner Group Service</label>
                                    <asp:DropDownList ID="ddlOwnerGroupService" runat="server" class="form-control form-control-sm"
                                        ClientIDMode="Static">
                                    </asp:DropDownList>
                                </div>

                                <asp:HiddenField ID="empcode" runat="server" />
                                <asp:HiddenField ID="rolecode" runat="server" />
                                <asp:HiddenField ID="hddOwnerCode" runat="server" />
                                
                                <asp:HiddenField ID="hdfMode" runat="server" />
                                <asp:Button ID="btnSetEdit" runat="server" CssClass="d-none" OnClick="btnSetEdit_Click" />
                                <asp:Button ID="btnSetCreate" runat="server" CssClass="d-none" OnClick="btnSetCreate_Click" />
                                <asp:Button ID="btnSave" runat="server" CssClass="d-none" OnClick="btnSave_Click" />
                            </contenttemplate>
                        </asp:UpdatePanel>
                        <div class="form-group">
                            <label for="sel2">Employee</label>
                            <uc1:AutoCompleteEmployee runat="server" id="AutoCompleteEmployee" CssClass="form-control form-control-sm ticket-allow-editor required" />
                        </div>
                    </asp:Panel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-success" onclick="saveClick();">Save</button>
                </div>
            </div>
        </div>
    </div>
    <script>
        function bindingDataTableJS() {
            $("#tablemappingrole").dataTable({
                columnDefs: [{
                    "orderable": false,
                    "targets": [0]
                }],
                "order": [[1, "asc"]]
            });
        }
        function closeModal(mode) {
            $("#modal-master-config").modal("hide");
            AGSuccess(mode + " Success.");
        }
        function confirmDelete(sender) {
            inactiveRequireField();
            if (AGConfirm(sender, "Confirm Delete")) {
                $(sender).next().click();
            }
        }


        function activeRequireField() {
            $(".required").prop('required', true);
        }

        function inactiveRequireField() {
            $(".required").prop('required', false);
        }

        function openCreate() {
            inactiveRequireField();
            $("#<%= btnSetCreate.ClientID %>").click();
        }

        function openEdit(ecode,rcode, ownercode) {
            inactiveRequireField();
            $("#<%= empcode.ClientID %>").val(ecode);
            $("#<%= rolecode.ClientID %>").val(rcode);
            $("#<%= hddOwnerCode.ClientID %>").val(ownercode);
            $("#<%= btnSetEdit.ClientID %>").click();
        }

        function openEditRow(obj) {
            var code = $(obj).closest("tr").data("key");
            var role = $(obj).closest("tr").data("role");
            var ownercode = $(obj).closest("tr").data("owner");
            openEdit(code, role, ownercode);
        }

        function openModal(mode) {
            $(".modal-body").find("input[type='text']").keypress(function (event) {
                // Number 13 is the "Enter" key on the keyboard
                if (event.keyCode == 13) {
                    //alert("nos");
                    // Trigger the button element with a click                    
                    activeRequireField();
                }
            });
            activeRequireField();
            $("#modal-header").html(mode + " Role Mapping");
            $("#modal-master-config").modal("show");

            if (mode == "New") {
                setTimeout(function () {
                    $("#<%= AutoCompleteEmployee.ClientID %>").focus();
                }, 500);
            } else {
                setTimeout(function () {
                    $("#<%= ddl_role_code.ClientID %>").focus();
                }, 500);
            }
        }

        function inactiveRequireField() {
            $(".required").prop('required', false);
        }

        function saveClick() {
            activeRequireField();
            $("#<%= btnSave.ClientID %>").click();
        }
    </script>

</asp:Content>
