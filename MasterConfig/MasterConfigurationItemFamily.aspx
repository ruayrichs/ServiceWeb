<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="MasterConfigurationItemFamily.aspx.cs" Inherits="ServiceWeb.MasterConfig.MasterConfigurationItemFamily" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
        <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-master-conf-fam").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <button type="button" class="btn btn-primary mb-1 AUTH_MODIFY" onclick="openCreate();"><i class="fa fa-plus-circle"></i>&nbsp;&nbsp;New Master Configuration Item Family</button>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>

    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Master Configuration Item Family</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                    <thead>
                                        <tr>
                                            <th class="text-nowrap"></th>
                                            <th>Material Group</th>
                                            <th>Description</th>
                                            <th>Prefix</th>
                                            <th>Start</th>
                                            <th>End</th>
                                            <th>ExternalOrNot</th>
                                            <th>FreeDeline</th>
                                           
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptItems" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="text-nowrap text-center align-middle">
                                                        <i class="fa fa-edit fa-lg text-dark mx-1 AUTH_MODIFY" title="Edit" onclick="openEdit('<%# Eval("MaterialGroupCode")+"|"+ Eval("Description")+"|"+Eval("PrefixCode") +"|"+Eval("xStart")+"|"+Eval("xEnd")+"|"+Eval("ExternalOrNot")+"|"+Eval("FreeDefine") %>');"></i>
                                                        <i class="fa fa-times-circle-o fa-lg text-danger mr-1 AUTH_MODIFY" title="Delete" onclick="confirmDelete(this);"></i>
                                                        <asp:Button ID="btnDelete" runat="server" CssClass="d-none AUTH_MODIFY" CommandArgument='<%# Eval("MaterialGroupCode") %>' OnClick="btnDeleteCongigurationItemFamily_Click" OnClientClick="AGLoading(true);"/>
                                                    </td>
                                                    <td><%# Eval("MaterialGroupCode") %></td>
                                                    <td><%# Eval("Description") %></td>
                                                    <td><%# Eval("PrefixCode") %></td>
                                                    <td><%# Eval("xStart") %></td>
                                                    <td><%# Eval("xEnd") %></td>
                                                    <td><%# Eval("ExternalOrNot") %></td>
                                                    <td><%# Eval("FreeDefine") %></td>

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



     <div class="modal fade" id="modal-master-conf-family" tabindex="-1" role="dialog" aria-labelledby="modal-header" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modal-header">Configuration Item Family</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="udpn" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" DefaultButton="btnSave">
                                <div class="row form-group">
                                    <div class="form-group col-sm-6">
                                        <label>Material Group </label>
                                        <asp:TextBox  ID="txtMaterialGroup" runat="server" CssClass="form-control form-control-sm required" Text=""/>
                                    </div>
                                    <div class="form-group col-sm-6">
                                        <label>Description</label>
                                        <asp:TextBox  ID="txtDesc" runat="server" CssClass="form-control form-control-sm required" Text=""/>
                                    </div>
                                </div>
                                
                                <div class="row form-group">
                                    <div class="form-group col-sm-6">
                                        <label>Start</label>
                                        <asp:TextBox  ID="txtStart" runat="server" CssClass="form-control form-control-sm required" Text="" type="number"/>
                                    </div>
                                    <div class="form-group col-sm-6">
                                        <label>End</label>
                                        <asp:TextBox  ID="txtEnd" runat="server" CssClass="form-control form-control-sm required" Text="" type="number"/>
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="form-group col-sm-6">
                                        <label>Prefix</label>
                                        <asp:TextBox  ID="txtPrefix" runat="server" CssClass="form-control form-control-sm" Text=""/>
                                    </div>
                                    
                                </div>
                                <div class=" form-group">
                                    <div class="form-check form-check-inline ">
                                        <asp:CheckBox ID="chbExternalOrNot" runat="server" Text=""  CssClass="form-check-input position-static" AutoPostBack ="true"  OnCheckedChanged="Check_Clicked"></asp:CheckBox>
                                       <asp:Label runat="server" CssClass="form-check-label" for="chbFreDeline">ExternalOrNot</asp:Label>
                                    </div>
                                    <div class="form-check form-check-inline ">
                                         <asp:CheckBox ID="chbFreeDeline" runat="server" Text="" CssClass="form-check-input position-static"></asp:CheckBox>
                                        <asp:Label  runat="server" CssClass="form-check-label" for="chbFreDeline">FreeDeline</asp:Label>
                                    </div>
                                </div>
                                
                                    
                               
                                <asp:HiddenField ID="hdfMode" runat="server" />
                                <asp:HiddenField ID="hdfEditCode" runat="server" />
                                <asp:Button ID="btnSetCreate" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnSetCreate_Click" OnClientClick="AGLoading(true);" />
                                <asp:Button ID="btnSetEdit" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnSetEdit_Click" OnClientClick="AGLoading(true);"/>
                                <asp:Button ID="btnSave" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnSave_Click" OnClientClick="AGLoading(true);"/>
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
            

            $("#modal-header").html(mode + " Master Configuration Item Family");
            $("#modal-master-conf-family").modal("show");         
        }
        function closeModal(mode) {
            $("#modal-master-conf-family").modal("hide");
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
        function setchbFreeDeline() {
            $("#<%= chbFreeDeline.ClientID %>").prop('disabled', true);
            
        }

        function openEdit(code) {
            inactiveRequireField();
            $("#<%= hdfEditCode.ClientID %>").val(code);
            $("#<%= btnSetEdit.ClientID %>").click();
        }

    </script>

</asp:Content>
