<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="TicketStatusAuto.aspx.cs" Inherits="ServiceWeb.MasterConfig.TicketStatusAuto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-ticket-status-auto").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
     <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <button type="button" class="btn btn-primary mb-1 AUTH_MODIFY" onclick="openCreate();"><i class="fa fa-plus-circle"></i>&nbsp;&nbsp;New Ticket Status Auto</button>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>

    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Ticket Status Auto</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive pl-2 pr-2">
                                <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                    <thead>
                                        <tr>
                                            <th class="text-nowrap"></th>
                                            <th class="text-nowrap">Ticket Status Begin </th>
                                            <th class="text-nowrap">Ticket Status Target </th>
                                            <th class="text-nowrap">Delay Time </th>
                                            <th class="text-nowrap">Working Status</th>
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
                                                        <i class="fa fa-edit fa-lg text-dark mx-1 AUTH_MODIFY" title="Edit" onclick="openEdit('<%# Eval("TicketStatusCodeBegin") %>');"></i>                                                        
                                                        <i class="fa fa-times-circle-o fa-lg text-danger mr-1 AUTH_MODIFY" title="Delete" onclick="confirmDelete(this);"></i>
                                                        <asp:Button ID="btnDelete" runat="server" CssClass="d-none AUTH_MODIFY"  OnClick="btnDelete_Click" CommandArgument='<%# Eval("TicketStatusCodeBegin") %>' />
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("TicketDocStatusDescBegin") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("TicketDocStatusDescTarget")%>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# ConvertToTime(Eval("DelayTime").ToString(), false) %>
                                                    </td>
                                                     <td class="text-nowrap">
                                                        <%# formatWorkingStatus(Eval("WorkingStatus").ToString()) %>
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
    <div class="modal fade" id="modal-master-config-auto" tabindex="-1" role="dialog" aria-labelledby="modal-header" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modal-header">Ticket Status Auto</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="udpn" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" DefaultButton="btnSave">
                            <div class="form-group">
                                <label>Ticket Status Begin &nbsp;</label>
                                <asp:DropDownList ID="ddlstatusbegin" CssClass="form-control form-control-sm" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Ticket Status Target &nbsp;</label>
                                <asp:DropDownList ID="ddlstatustarget" CssClass="form-control form-control-sm" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">      
                                <label>Delay Time (Minutes) &nbsp;<span class="errDelayTime" style="color: red"></span></label>
                                <asp:TextBox ID="tbdelaytime" placeholder="Number" ClientIDMode="Static" Style="text-align: right;" CssClass="form-control required" runat="server"
                                    onkeypress="return isNumberic(event,'errDelayTime');" />
                            </div>
                            <div class="form-group">
                               <label>Working Status</label>&nbsp;&nbsp;
                               <asp:CheckBox ID="chkIsWorking" Text="" runat="server" />
                            </div>
                            <asp:HiddenField ID="hdfMode" runat="server" />       
                            <asp:HiddenField ID="hdfEditCode" runat="server" />                                                        
                            <asp:Button ID="btnSetCreate" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnSetCreate_Click" AutoPostBack="false"/>                      
                            <asp:Button ID="btnSave" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnSave_Click" />
                            <asp:Button ID="btnSetEdit" runat="server"  CssClass="d-none AUTH_MODIFY" OnClick="btnSetEdit_Click" AutoPostBack="false"/>
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
        function openCreate() {
            inactiveRequireField();
            $("#<%= btnSetCreate.ClientID %>").click();
            AGLoading(true);
        }

        function openModal(mode) {
            $(".modal-body").find("input[type='text']").keypress(function (event) {
                // Number 13 is the "Enter" key on the keyboard
                if (event.keyCode == 13) {
                    // Trigger the button element with a click                    
                    activeRequireField();
                }
            });
            $("#modal-header").html(mode + " Ticket Status Auto");
            $("#modal-master-config-auto").modal("show");

            if (mode == "New") {
                setTimeout(function () {
                    $("#<%= ddlstatusbegin.ClientID %>").focus();
                }, 500);
            } else {
                setTimeout(function () {
                    $("#<%= ddlstatustarget.ClientID %>").focus();
                }, 500);
            }
        }

        function closeModal(mode) {
            $("#modal-master-config-auto").modal("hide");
            AGSuccess(mode + " Success.");
        }

        function saveClick() {
            activeRequireField();
            $("#<%= btnSave.ClientID %>").click();
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
            AGLoading(true);
        }

        function confirmDelete(sender) {
            inactiveRequireField();
            if (AGConfirm(sender, "Confirm Delete")) {
                $(sender).next().click();
            }
        }

        function isNumberic(evt, span) {
            //Check numberic only
            var r = (evt.keyCode >= 48 && evt.keyCode <= 57)
            if (!r) {
                $("." + span).html("Digits Only").show().fadeOut("slow");
                return r;
            }

            return r;
        }
    </script>
</asp:Content>
