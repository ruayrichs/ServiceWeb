<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="SLAMappingChanelTier0.aspx.cs" Inherits="ServiceWeb.MasterConfig.SLAMappingChanelTier0" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-sla-mapping").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
        </div>
    </nav>

    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">SLA Mapping Chanel Tier 0</h5>
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
                                            <th>Name</th>
                                            <th class="text-nowrap">Created By</th>
                                            <th class="text-nowrap">Created On</th>
                                            <th class="text-nowrap">Updated By</th>
                                            <th class="text-nowrap">Updated On</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptItems" runat="server">
                                            <ItemTemplate>

                                                <tr class="c-pointer" data-key="<%# Eval("ChanelCode") %>">
                                                    <td class="text-nowrap text-center align-middle">
                                                        <i class="fa fa-edit fa-lg text-dark mx-1 AUTH_MODIFY" title="Edit" onclick="openEdit('<%# Eval("ChanelCode") %>');"></i>
                                                    </td>
                                                    <td class="text-nowrap" onclick="openEditRow(this);">
                                                        <%# Eval("ChanelCode") %>
                                                    </td>
                                                    <td onclick="openEditRow(this);">
                                                        <%# Eval("ChanelName") %>
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
                    <h5 class="modal-title" id="modal-header">sfsfsfImpact</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="udpn" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" DefaultButton="btnSave">
                                <div class="form-group">
                                    <label>Code</label>
                                    <asp:TextBox ID="tbCode" placeholder="Text" runat="server" CssClass="form-control form-control-sm required"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Name</label>
                                    <asp:TextBox ID="tbName" placeholder="Text" runat="server" CssClass="form-control form-control-sm required"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>SLA Group</label>
                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlSLA">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label>Priority</label>
                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlPriority">
                                    </asp:DropDownList>
                                </div>


                                <asp:HiddenField ID="hdfMode" runat="server" />
                                <asp:HiddenField ID="hdfEditCode" runat="server" />
                                <asp:Button ID="btnSave" runat="server" CssClass="d-none" OnClick="btnSave_Click" />
                                <asp:Button ID="btnSetEdit" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnSetEdit_Click" />
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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

        function openEdit(code) {
            inactiveRequireField();
            $("#<%= hdfEditCode.ClientID %>").val(code);
            $("#<%= btnSetEdit.ClientID %>").click();
        }

        function openEditRow(obj) {
            var code = $(obj).closest("tr").data("key");
            openEdit(code);
        }

        function openModal(mode) {
            $(".modal-body").find("input[type='text']").keypress(function (event) {
                // Number 13 is the "Enter" key on the keyboard
                if (event.keyCode == 13) {
                    // Trigger the button element with a click                    
                    activeRequireField();
                }
            });
            $("#modal-header").html(mode + " Impact");
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
        function saveClick() {
            activeRequireField();
            $("#<%= btnSave.ClientID %>").click();
        }
    </script>
</asp:Content>
