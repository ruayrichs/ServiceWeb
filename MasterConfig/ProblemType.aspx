﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="ProblemType.aspx.cs" Inherits="ServiceWeb.MasterConfig.ProblemType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-problem-type").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
     <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <button type="button" class="btn btn-primary mb-1 AUTH_MODIFY" onclick="openCreate();"><i class="fa fa-plus-circle"></i>&nbsp;&nbsp;New Problem Type</button>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>

    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Problem Type</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                    <thead>
                                        <tr>
                                            <th class="text-nowrap"></th>
                                            <th class="text-nowrap">Problem Group</th>
                                            <th class="text-nowrap">Problem Type</th>
                                            <th class="text-nowrap">Owner Group</th>
                                            <th class="text-nowrap">Created By</th>
                                            <th class="text-nowrap">Created On</th>
                                            <th class="text-nowrap">Updated By</th>
                                            <th class="text-nowrap">Updated On</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptItems" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="text-nowrap text-center align-middle">
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <i class="fa fa-edit fa-lg text-dark mx-1 AUTH_MODIFY" title="Edit" onclick="openEdit('<%# Eval("BUSINESSOBJECT") %>','<%# Eval("GROUPCODE") %>','<%# Eval("TYPECODE") %>','<%# Eval("OwnerGroupCode") %>');"></i>                                                        
                                                                <i class="fa fa-times-circle-o fa-lg text-danger mr-1 AUTH_MODIFY" title="Delete" onclick="confirmDelete(this);"></i>
                                                                <asp:Button ID="btnDelete" runat="server" CssClass="d-none AUTH_MODIFY"  OnClick="btnDelete_Click" CommandArgument='<%# Eval("TYPECODE") %>' />
                                                                <asp:HiddenField ID="hdfBusinessObject" Value='<%# Eval("BUSINESSOBJECT") %>' runat="server" />
                                                                <asp:HiddenField ID="hdfGroupCode" Value='<%# Eval("GROUPCODE") %>' runat="server" />
                                                                <asp:HiddenField ID="hdfOwnerCode" Value='<%# Eval("OwnerGroupCode") %>' runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("GROUPCODE") +" : " + Eval("GROUPNAME") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("TYPECODE") +" : " + Eval("TYPENAME") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("OwnerGroupName") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("CREATED_BY") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("CREATED_ON").ToString()) %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("UPDATED_BY") %>
                                                    </td>
                                                    <td class="text-nowrap">
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
                    <h5 class="modal-title" id="modal-header">Problem Type</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="udpn" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" DefaultButton="btnSave">
                                <div class="form-group" runat="server">
                                    <label>Business Object</label>
                                    <asp:DropDownList ID="ddlBusinessObject" onchange="$('#btnSetGroupData').click();" CssClass="form-control form-control-sm required" runat="server">
                                    </asp:DropDownList>
                                    <asp:Button ID="btnSetGroupData" ClientIDMode="Static" OnClick="btnSetGroupData_Click" OnClientClick="inactiveRequireField();AGLoading(true);" CssClass="d-none" runat="server" />
                                </div>
                                <div class="form-group">
                                    <label>Owner Group Service</label>
                                    <asp:DropDownList ID="ddlOwnerGroup" CssClass="form-control form-control-sm required" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group" runat="server">
                                    <label>Problem Group</label>
                                    <asp:DropDownList ID="ddlProblemGroup" CssClass="form-control form-control-sm required" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label>Code</label>
                                    <asp:TextBox ID="tbCode" placeholder="Text" runat="server" CssClass="form-control form-control-sm required"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Name</label>
                                    <asp:TextBox ID="tbName" placeholder="Text" runat="server" CssClass="form-control form-control-sm required"></asp:TextBox>
                                </div>
                                
                                <asp:HiddenField ID="hdfMode" runat="server" />
                                <asp:HiddenField ID="hdfEditCode" runat="server" />
                                <asp:HiddenField ID="hdfBusiness" runat="server" />
                                <asp:HiddenField ID="hdfGropCode" runat="server" />
                                <asp:HiddenField ID="hdfOwnerGropCode" runat="server" />
                                
                                <asp:Button ID="btnSetCreate" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnSetCreate_Click" />
                                <asp:Button ID="btnSetEdit" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnSetEdit_Click" />
                                <asp:Button ID="btnSave" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnSave_Click" OnClientClick="AGLoading(true);" />
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-success AUTH_MODIFY" onclick="saveClick();">Save</button>
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

        function openEdit(business,group,code,owncode) {
            inactiveRequireField();
            $("#<%= hdfBusiness.ClientID %>").val(business);
            $("#<%= hdfGropCode.ClientID %>").val(group);
            $("#<%= hdfEditCode.ClientID %>").val(code);
            $("#<%= hdfOwnerGropCode.ClientID %>").val(owncode);
            $("#<%= btnSetEdit.ClientID %>").click();
        }

        function openModal(mode) {
            $(".modal-body").find("input[type='text']").keypress(function (event) {
                // Number 13 is the "Enter" key on the keyboard
                if (event.keyCode == 13) {
                    // Trigger the button element with a click                    
                    activeRequireField();
                }
            });
            $("#modal-header").html(mode + " Problem Type");
            $("#modal-master-config").modal("show");

            if (mode == "New") {
                setTimeout(function () {
                    $("#<%= tbCode.ClientID %>").focus();
                }, 500);
            } else {
                setTimeout(function () {
                    $("#<%= tbName.ClientID %>").focus();
                }, 500);
            }
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

        function saveClick() {
            activeRequireField();
            $("#<%= btnSave.ClientID %>").click();
        }
    </script>

</asp:Content>
