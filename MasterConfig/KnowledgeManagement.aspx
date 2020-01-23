<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="KnowledgeManagement.aspx.cs" Inherits="ServiceWeb.MasterConfig.KnowledgeManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-knowledge-management").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    <div>
        <div style="padding-bottom:12px">
           <%-- <asp:button runat="server" text="New Knowlegde" cssclass="btn btn-primary"/>--%>
            <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#add-modal">
               <i class="fa fa-plus-circle"></i>&nbsp;&nbsp; New Knowlegde
            </button>
        </div>
        <div class="card shadow">
            <div class="card-header">
                <h4>Knowledge Management</h4>
            </div>
            <div class="card-body">
                <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="table-responsive">
                            <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                <thead>
                                    <tr>
                                        <%--<th></th>--%>
                                        <th></th>
                                        <th>Code</th>
                                        <th>Name</th>
                                        <th>Created By</th>
                                        <th>Created On</th>
                                        <th>Updated By</th>
                                        <th>Updated On</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptItem" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <%--<td class="text-center"><i class="fa fa-edit fa-lg text-dark mx-1 AUTH_MODIFY"></i></td>--%>
                                                <td class="text-nowrap text-center">
                                                    <i class="fa fa-edit fa-lg text-dark mx-1 AUTH_MODIFY" onclick="updateKM('<%# Eval("message_group") %>');"></i>
                                                    <i class="fa fa-times-circle-o fa-lg text-danger mr-1 AUTH_MODIFY" onclick="deleteKM('<%# Eval("message_group") %>');"></i>

                                                </td>
                                                <td class="text-nowrap"><%# Eval("message_group") %></td>
                                                <td class="text-nowrap"><%# Eval("group_name") %></td>
                                                <td class="text-nowrap"><%# Eval("created_by") %></td>
                                                <td class="text-nowrap"><%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("created_on").ToString()) %></td>
                                                <td class="text-nowrap"><%# Eval("updated_by") %></td>
                                                <td class="text-nowrap"><%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("updated_on").ToString()) %></td>
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

        <!-- modal -->
        <!--Add Modal -->
        <div class="modal fade" id="add-modal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
          <div class="modal-dialog" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">New Knowledge</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body">
                  <asp:UpdatePanel ID="update_udp" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                  <div><label><strong>Code</strong></label></div>
                  <asp:textbox runat="server" id="message_group_input" cssclass="form-control form-control-sm required" placeholder="Text" />
                  <div><label><strong>Name</strong></label></div>
                  <asp:textbox runat="server" id="group_name_input" cssclass="form-control form-control-sm required" placeholder="Text" />
                  <div><label><strong>OwnerGroup</strong></label></div>
                  <asp:DropDownList runat="server" ID="ownerGroupCode_input" CssClass="form-control form-control-sm required">

                  </asp:DropDownList>
                        </ContentTemplate></asp:UpdatePanel>
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                <%--<button type="button" class="btn btn-success">Save</button>--%>
                   <asp:button runat="server" text="Save" cssclass="btn btn-success" id="save_btn" onclick="save_btn_click"/>
              </div>
            </div>
          </div>
        </div>

        <!-- hidden -->
        <div class="d-none">
            <asp:textbox runat="server" id="code_input" ClientIDMode="Static" />
            <%--<asp:button runat="server" id="delete_button" Text="delete"/>--%>
             <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#update-modal" id="update_button">
               Update
            </button>
            <asp:button runat="server" id="readone" ClientIDMode="Static" OnClick="readone_Click" Text="readone"/>
            <asp:button runat="server" id="delete_button" ClientIDMode="Static" OnClick="delete_button_Click" Text="Delete"/>
        </div>

        <!--Update Modal -->
        <div class="modal fade" id="update-modal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
          <div class="modal-dialog" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="example-update-modal">Update Knowledge</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body">
                  <asp:UpdatePanel ID="udpUpdate" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                  <div><label><strong>Code</strong></label></div>
                  <asp:textbox runat="server" id="update_massage_group_input" cssclass="form-control form-control-sm required" placeholder="Text" />
                  <div><label><strong>Name</strong></label></div>
                  <asp:textbox runat="server" id="update_group_name_input" cssclass="form-control form-control-sm required" placeholder="Text" />
                  <div><label><strong>OwnerGroup</strong></label></div>
                  
                  <asp:DropDownList runat="server" id="update_ownerGroup" cssclass="form-control form-control-sm required">
                  </asp:DropDownList>
                        </ContentTemplate></asp:UpdatePanel>
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                <%--<button type="button" class="btn btn-success">Save</button>--%>
                   <asp:button runat="server" text="Save" cssclass="btn btn-success" id="updateDesc_button" onclick="updateDesc_button_Click"/>
              </div>
            </div>
          </div>
        </div>
    </div>
    <script>
        var massage_group;
        function HelloWorld() {
            console.log("Hello");
        };
        function afterLoad() {
            $(document).ready(function () {
                $('#tableItems').DataTable();
            });
        };
        function updateKM(massage_group) {
            this.massage_group = massage_group;
            document.getElementById('<%= code_input.ClientID %>').value = this.massage_group;
            console.log(massage_group);
            document.getElementById('<%= readone.ClientID %>').click();
            //document.getElementById('update_button').click();
        };
        function openupdatemodal() {
            document.getElementById("update_button").click(); // Click on the checkbox

        };
        function deleteKM(massage_group) {
            this.massage_group = massage_group;
            document.getElementById('<%= code_input.ClientID %>').value = this.massage_group;
            console.log(massage_group);
            document.getElementById('<%= delete_button.ClientID %>').click();
        };
    </script>
</asp:Content>
