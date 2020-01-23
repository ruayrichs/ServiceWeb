<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="OwnerService.aspx.cs" Inherits="ServiceWeb.MasterConfig.OwnerService" %>

<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEmployee.ascx" TagPrefix="uc1" TagName="AutoCompleteEmployee" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-owner-service").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <button type="button" class="btn btn-primary mb-1 AUTH_MODIFY" onclick="openCreate();"><i class="fa fa-plus-circle"></i>&nbsp;&nbsp;New Owner Service</button>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>

    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Owner Service</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                    <thead>
                                        <tr>
                                            <th class="text-nowrap"></th>
                                            <th class="text-nowrap">Owner Group</th>
                                            <th class="text-nowrap">E-Mail</th>
                                            <th class="text-nowrap">Role</th>
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
                                                        <i class="fa fa-edit fa-lg text-dark mx-1 AUTH_MODIFY" title="Edit" onclick="openEdit('<%# Eval("OwnerGroupCode") %>');"></i>
                                                        <i class="fa fa-times-circle-o fa-lg text-danger mr-1 AUTH_MODIFY" title="Delete" onclick="confirmDelete(this);"></i>
                                                        <asp:Button ID="btnDelete" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnDelete_Click" CommandArgument='<%# Eval("OwnerGroupCode") %>' />
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("OwnerGroupCode") +" : "+Eval("OwnerGroupName") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("Email") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("desc") %>
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
                    <h5 class="modal-title" id="modal-header">Problem Service</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="udpnnav" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <nav>
                                <div class="nav nav-tabs" id="nav-tab" role="tablist">
                                    <a class="nav-item nav-link" id="nav-header-tab" data-toggle="tab" href="#nav-header" role="tab" aria-controls="nav-header" aria-selected="true">Owner Detail</a>
                                    <a class="nav-item nav-link" id="nav-item-tab" data-toggle="tab" href="#nav-item" role="tab" aria-controls="nav-item" aria-selected="false">Role</a>
                                    <a class="nav-item nav-link" id="nav-cab-tab" data-toggle="tab" href="#nav-cab" role="tab" aria-controls="nav-cab" aria-selected="false">CAB</a>
                                    <a class="nav-item nav-link" id="navpermissiontab" runat="server" data-toggle="tab" href="#nav-permiss" role="tab" aria-controls="nav-permiss" aria-selected="false">Corperate Permission</a>
                                </div>
                            </nav>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="tab-content p-0" id="nav-tabContent">
                        <div class="tab-pane fade show active" id="nav-header" role="tabpanel" aria-labelledby="nav-header-tab">
                            <div>
                                <asp:UpdatePanel ID="udpn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel runat="server" xDefaultButton="btnSave">
                                            <div class="form-group">
                                                <label>Code</label>
                                                <asp:TextBox ID="tbCode" placeholder="Text" runat="server" CssClass="form-control form-control-sm required"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label>Name</label>
                                                <asp:TextBox ID="tbName" placeholder="Text" runat="server" CssClass="form-control form-control-sm required"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label>E-Mail</label>
                                                <asp:TextBox ID="tbMail" placeholder="Text" runat="server" CssClass="form-control form-control-sm required"></asp:TextBox>
                                            </div>
                                        </asp:Panel>

                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="tab-pane fade" id="nav-item" role="tabpanel" aria-labelledby="nav-item-tab">
                                <div>
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpn2">
                                        <ContentTemplate>
                                            <div>
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updEventObject">
                                                    <ContentTemplate>
                                                        <label>
                                                            Role
                                                        </label>
                                                        <asp:DropDownList ID="ddlParticipantsDescription" runat="server" ClientIDMode="Static" CssClass="form-control ddlParticipants-role"
                                                            OnSelectedIndexChanged="btnChangeSubProjectObject_Click" onchange="AGLoading(true);" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="form-row">
                                                <div class="col-sm-12 col-md-12">
                                                    <br />
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpMainDelegate">
                                                        <ContentTemplate>
                                                            <label>Main Delegate</label>
                                                            <table class="table table-sm table-striped">
                                                                <asp:Repeater runat="server" ID="rptMainDelegate">
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td style="width: 100px;">
                                                                                <img class='image-box-card z-depth-1' data-toggle='tooltip' data-placement='top' style="width: 45px; height: 45px;"
                                                                                    src='<%# getPathImgEmployeeForEmployeeCode(Convert.ToString(Eval("EmployeeCode")), Convert.ToString(Eval("UpdateOnEmployee"))) %>'
                                                                                    title='<%# Eval("FullName_TH") %> (<%# Eval("FullName_EN") %>)' />
                                                                            </td>
                                                                            <td>
                                                                                <%# Eval("FullName_TH") %> (<%# Eval("FullName_EN") %>)
                                                                    <div>
                                                                        <small>Employee Code : <%# Eval("EmployeeCode") %></small>
                                                                    </div>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>

                                                                <% if (rptMainDelegate.Items.Count == 0)
                                                                    { %>
                                                                <tr>
                                                                    <td colspan="2" class="alert alert-info">None</td>
                                                                </tr>
                                                                <% } %>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-sm-12 col-md-12">
                                                    <br />
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpParticipants">
                                                        <ContentTemplate>
                                                            <label>Participants Delegate</label>
                                                            <table class="table table-sm table-striped">
                                                                <asp:Repeater runat="server" ID="rptParticipants">
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td style="width: 100px;">
                                                                                <img class='image-box-card z-depth-1' data-toggle='tooltip' data-placement='top' style="width: 45px; height: 45px;"
                                                                                    src='<%# getPathImgEmployeeForEmployeeCode(Convert.ToString(Eval("EmployeeCode")), Convert.ToString(Eval("UpdateOnEmployee"))) %>'
                                                                                    title='<%# Eval("FullName_TH") %> (<%# Eval("FullName_EN") %>)' />
                                                                            </td>
                                                                            <td>
                                                                                <%# Eval("FullName_TH") %> (<%# Eval("FullName_EN") %>)
                                                                    <div>
                                                                        <small>Employee Code : <%# Eval("EmployeeCode") %></small>
                                                                    </div>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                                <% if (rptParticipants.Items.Count == 0)
                                                                    { %>
                                                                <tr>
                                                                    <td colspan="2" class="alert alert-info">None</td>
                                                                </tr>
                                                                <% } %>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                            <div class="tab-pane fade" id="nav-cab" role="tabpanel" aria-labelledby="nav-cab-tab">
                                <label style="padding-top: 10px;">Select Employee</label>
                                <div class="input-group" style="flex-wrap: nowrap;">
                                    <uc1:AutoCompleteEmployee runat="server" CssClass="form-control form-control-sm" id="AutoCompleteEmployee" />
                                    <div class="input-group-append">
                                        <button type="button" class="btn btn-info AUTH_MODIFY btn-sm" onclick="addClick();">Add</button>
                                    </div>
                                </div>
                                <br>
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpnEmp">
                                    <ContentTemplate>
                                        <table class="table table-sm table-striped">
                                            <asp:Repeater runat="server" ID="rptEmp">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="width: 100px;">
                                                            <img class='image-box-card z-depth-1' data-toggle='tooltip' data-placement='top' style="width: 45px; height: 45px;"
                                                                src='<%# getPathImgEmployeeForEmployeeCode(Convert.ToString(Eval("EmployeeListID")), Convert.ToString(Eval("EmployeeListDesc"))) %>'
                                                                title='<%# Eval("EmployeeListDesc") %>' />
                                                        </td>
                                                        <td>
                                                            <%# Eval("EmployeeListDesc") %>
                                                            <asp:HiddenField runat="server" ID="hddEmployeeListDesc" Value='<%# Eval("EmployeeListDesc") %>' />
                                                            <div>
                                                                <small>Employee Code : <%# Eval("EmployeeListID") %></small>

                                                                <asp:HiddenField runat="server" ID="hddEmployeeListID" Value='<%# Eval("EmployeeListID") %>' />
                                                            </div>
                                                        </td>
                                                        <td class="text-center" onclick="removeRowEmpParticipant(this);">
                                                            <asp:Button Text="Delete" runat="server" ID="DelEmpbtn"
                                                                CssClass="btn btn-sm btn-danger" Style="vertical-align: middle; margin-top: 6px;" OnClick="DelEmpList" CommandArgument='<%#Container.ItemIndex%>' />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <asp:Button ID="AddEmpbtn" runat="server" class="d-none AUTH_MODIFY btn btn-info btn-sm" OnClick="AddEmpList" Text="Add" />
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="tab-pane fade" id="nav-permiss" role="tabpanel" aria-labelledby="navpermissiontab">
                                <label style="padding-top: 10px;">Corporate Permission Key</label>

                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpnPermission">
                                    <ContentTemplate>
                                        <div>
                                            <asp:TextBox ID="tbCorporatePermissionKey" placeholder="Permission Key" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                        </div>
                                        <br>
                                        <div>
                                            <asp:Button ID="GenKey" runat="server" class="AUTH_MODIFY btn btn-primary" OnClick="GenKey_Click" Text="" />
                                        </div>
                                        <br>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>



                    </div>
                    <asp:UpdatePanel ID="udpn1" runat="server" UpdateMode="Conditional">

                        <ContentTemplate>
                            <asp:Panel runat="server" DefaultButton="btnSave">
                                <asp:HiddenField ID="hdfMode" runat="server" />
                                <asp:HiddenField ID="hdfEditCode" runat="server" />
                                <asp:Button ID="btnSave" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnSave_Click" />
                                <asp:Button ID="btnSetCreate" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnSetCreate_Click" />
                                <asp:Button ID="btnSetEdit" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnSetEdit_Click" />
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        <button type="button" class="btn btn-success AUTH_MODIFY" onclick="saveClick();">Save</button>
                    </div>
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

        function openEdit(code) {
            inactiveRequireField();
            $("#<%= hdfEditCode.ClientID %>").val(code);
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
            $("#modal-header").html(mode + " Owner Service");
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

            $('#nav-header-tab').click();
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
        function addClick() {
            $("#<%= AddEmpbtn.ClientID %>").click();
        }


        function saveClick() {
            activeRequireField();
            $("#<%= btnSave.ClientID %>").click();
        }

      
    </script>

</asp:Content>
