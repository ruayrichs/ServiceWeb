<%@ Page Title="" Language="C#" MasterPageFile="~/Accountability/MasterPage/AccountabilityMaster.master" AutoEventWireup="true" CodeBehind="ChangeOrderTypeMappingAccountability.aspx.cs" Inherits="ServiceWeb.Accountability.ChangeOrderTypeMappingAccountability" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-role-change-map-acc").className = "nav-link active";
        };
        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <button type="button" class="btn btn-primary mb-1 AUTH_MODIFY" onclick="openCreate();"><i class="fa fa-plus-circle"></i>&nbsp;&nbsp;New Mapping</button>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>

     <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Document Type Mapping Accountability</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                    <thead>
                                        <tr>
                                            <th class="text-nowrap"></th>
                                            <th>Document Type Code</th>
                                            <th>Accountability Code</th>
                                            <th>Created By</th>
                                            <th>Created On</th>
                                           
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptItems" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="text-nowrap text-center align-middle">
                                                        <i class="fa fa-edit fa-lg text-dark mx-1 AUTH_MODIFY" title="Edit" onclick="openEdit('<%# Eval("DocTypeCode")+","+Eval("AccountabilityCode") %>');"></i>
                                                        <i class="fa fa-times-circle-o fa-lg text-danger mr-1 AUTH_MODIFY" title="Delete" onclick="confirmDelete(this);"></i>
                                                        <asp:Button ID="btnDelete" runat="server" CssClass="d-none AUTH_MODIFY" CommandArgument='<%# Eval("DocTypeCode")+","+Eval("AccountabilityCode") %>' OnClick="btnDeleteDoctypeMapAccountability_click" />
                                                    </td>
                                                    
                                                    <td>
                                                       <%# Eval("DocDesc") %>
                                                    </td>
                                                    <td>
                                                       <%# Eval("AccountabilityCode") +" : "+Eval("AccDesc") %>
                                                    </td>
                                                   <td>
                                                       <%# Eval("Created_By") %>
                                                    </td>
                                                    <td>
                                                        <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("Created_On").ToString()) %>
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
    <div class="modal fade" id="modal-doc-map-acc" tabindex="-1" role="dialog" aria-labelledby="modal-header" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modal-header">Create</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>

                 <div class="modal-body">
                    <asp:UpdatePanel ID="udpn" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                          
                                <asp:Panel runat="server" DefaultButton="btnSave" ID="panelID">
                                <div class="form-group">
                                    <label>Document Type</label>
                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm required" ID="ddlDocType">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label>Accountability</label>
                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm required" ID="ddlAccountability">
                                    </asp:DropDownList>
                                </div>
                                <asp:HiddenField ID="hdfMode" runat="server" />
                                <asp:HiddenField ID="hdfEditCode" runat="server" />
                                <asp:Button ID="btnSetCreate" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnSetCreate_Click" OnClientClick="AGLoading(true);" />
                                    <asp:Button ID="btnSetEdit" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnSetEdit_Click" OnClientClick="AGLoading(true);" />
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
        function inactiveRequireField() {
            $(".required").prop('required', false);
        }

        function openCreate() {
            inactiveRequireField();
            $("#<%= btnSetCreate.ClientID %>").click();
        }
        function openModal(mode) {
            

            $("#modal-header").html(mode + " Mapping");
            $("#modal-doc-map-acc").modal("show");         
        }
        function closeModal(mode) {
            $("#modal-doc-map-acc").modal("hide");
            AGSuccess(mode + " Success.");
        }
        function activeRequireField() {
            $(".required").prop('required', true);
        }
         function saveClick() {
            activeRequireField();
            $("#<%= btnSave.ClientID %>").click();
        }
         function confirmDelete(sender) {
            inactiveRequireField();
            if (AGConfirm(sender, "Confirm Delete")) {
                $(sender).next().click();
            }
        }
        function openEdit(code) {
            inactiveRequireField();
            $("#<%= hdfEditCode.ClientID %>").val(code);
            $("#<%= btnSetEdit.ClientID %>").click();
        }

    </script>

</asp:Content>
