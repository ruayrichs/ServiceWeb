<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="BusinessOwnerRequest.aspx.cs" Inherits="ServiceWeb.RequestTransection.BusinessOwnerRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-business-owner").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    <div>
        <asp:UpdatePanel ID="udpHidden" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="d-none">
                    <asp:TextBox runat="server" ID="seq_inp" Text="" ClientIDMode="Static" />
                    <asp:TextBox runat="server" ID="status_inp" Text="" ClientIDMode="Static" />
                    <asp:Button runat="server" ID="confirm_btn" Text="Confirm" OnClick="changeStatus_Click" ClientIDMode="Static" />
                    <button type="button" id="open_modal_btn" data-toggle="modal" data-target="#userdesc_modal">
                        Open Modal
                    </button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="card mb-4 shadow-sm">
            <div class="card-header">
                <h4 class="my-0 font-weight-normal" >BusinessOwnerRequest</h4>
            </div>
            <div class="card-body PANEL-DEFAULT-BUTTON">
                <div id="tableArea">
                    <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                    <thead>
                                        <tr>
                                            <th class="text-nowrap"></th>
                                            <th class="text-nowrap"></th>
                                            <th class="text-nowrap">SEQ</th>
                                            <th class="text-nowrap">FirstName</th>
                                            <th class="text-nowrap">LastName</th>
                                            <th class="text-nowrap">Business Owner</th>
                                            <th class="text-nowrap">Username</th>
                                            <th class="text-nowrap">Email</th>
                                            <th class="text-nowrap">Phone</th>
                                            <th class="text-nowrap">SignUp DateTime</th>
                                            <th class="text-nowrap">Status</th>
                                            <th class="text-nowrap">UpdateStatus By</th>
                                            <th class="text-nowrap">UpdateStatus On</th>
                                            <th class="text-nowrap">Activation</th>
                                            <th class="text-nowrap">Activation DateTime</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptSEQItem" runat="server">
                                            <ItemTemplate>
                                                <tr class="table-hover">
                                                    <td class="text-nowrap"><button type="button" class="btn btn-success btn-sm <%# Eval("DisplayControlApproveButton") %>" onclick="onchangeStatus('<%# Eval("SEQ") %>','APPROVE');approveConfirmRequest(this);" >APPROVE</button></td>
                                                    <td class="text-nowrap"><button type="button" class="btn btn-danger btn-sm <%# Eval("DisplayControlRejectButton") %>" onclick="onchangeStatus('<%# Eval("SEQ") %>','REJECT');rejectConfirmRequest(this);" >REJECT</button></td>
                                                    <th class="text-nowrap"><%# Eval("SEQ") %></th>
                                                    <td class="text-nowrap"><%# Eval("FirstName") %></td>
                                                    <td class="text-nowrap"><%# Eval("LastName") %></td>
                                                    <td class="text-nowrap"><%# Eval("BusinessOwner") %></td>
                                                    <td class="text-nowrap"><%# Eval("Username") %></td>
                                                    <td class="text-nowrap"><%# Eval("Email") %></td>
                                                    <td class="text-nowrap"><%# Eval("Phone") %></td>
                                                    <td class="text-nowrap"><%# Eval("SignUp_DateTime") %></td>
                                                    <td class="text-nowrap"><%# Eval("Status") %></td>
                                                    <td class="text-nowrap"><%# Eval("UpdateStatus_By") %></td>
                                                    <td class="text-nowrap"><%# Eval("UpdateStatus_On") %></td>
                                                    <td class="text-nowrap"><%# Eval("Activation") %></td>
                                                    <td class="text-nowrap"><%# Eval("Activation_DateTime") %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <!-- Modal -->
        <div>
                <div class="modal fade" id="userdesc_modal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="exampleModalLabel">User description</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body" >
                                <div class="form-row">
                                    <div class="form-group col-sm-6">
                                        <label>Owner Group Code</label>
                                        <asp:TextBox runat="server" ID="OwnerGroupCode_inp" CssClass="form-control form-control-sm" 
                                            placeholder="Number Owner Group Code" Enabled="true" onkeypress="return isNumberKey(event);" MaxLength="5" />
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="form-group col-sm-6">
                                        <div><label>Employee Group</label></div>
                                        <div><asp:DropDownList runat="server" ID="employee_group_inp" CssClass="form-control form-control-sm"></asp:DropDownList></div>
                                    </div>
                                    <div class="form-group col-sm-6">
                                        <div><label>Role</label></div>
                                        <div><asp:DropDownList runat="server" ID="position_inp" CssClass="form-control form-control-sm" ></asp:DropDownList></div>
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="form-group col-sm-6">
                                        <div><label>Start Date</label></div>
                                        <div class="input-group">
                                            <asp:TextBox ID="startDate_inp" runat="server" CssClass="form-control form-control-sm date-picker ticket-allow-editor"></asp:TextBox>
                                            <div class="input-group-append">
                                                <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-sm-6">
                                        <div><label>End Date</label></div>
                                        <div class="input-group">
                                            <asp:TextBox ID="endDate_inp" runat="server" CssClass="form-control form-control-sm date-picker ticket-allow-editor"></asp:TextBox>
                                            <div class="input-group-append">
                                                <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                                <%--<button type="button" class="btn btn-primary">Yes</button>--%>
                                <asp:UpdatePanel ID="udpModal" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button runat="server" OnClick="updateSEQ_Click" CssClass="btn btn-primary" ID="changeStatus_btn" Text="OK" ClientIDMode="Static" OnClientClick="AGLoading(true);"/>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    </div>
    <script type="text/javascript">
        //var seq = null;
        function onchangeStatus(seq, status) {
            console.log("before SEQ:" + seq + " Status: " + status);
            //this.seq = document.getElementById(seq_inp).value;
            document.getElementById('<%= seq_inp.ClientID %>').value = seq;
            document.getElementById('<%= status_inp.ClientID %>').value = status;
            //document.getElementById('open_modal_btn').click();
            //document.getElementById('<%= confirm_btn.ClientID %>').click();
        };
    </script>
    <script>
        function approveConfirmRequest(obj) {
            if (AGConfirm(obj, 'Are you Sure ?')) {
                document.getElementById('open_modal_btn').click(); //open modal
                //document.getElementById('<%= confirm_btn.ClientID %>').click();
                return true;
            }
            return false;
        };
        function rejectConfirmRequest(obj) {
            if (AGConfirm(obj, 'Are you Sure ?')) {
                AGLoading(true);
                document.getElementById('<%= confirm_btn.ClientID %>').click();
                return true;
            }
            return false;
        };
        function afterLoad() {
            $(document).ready(function () {
                $('#tableItems').DataTable();
            });
        };
    </script>
</asp:Content>
