<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="RequestActivateAppClient.aspx.cs" Inherits="ServiceWeb.RequestTransection.RequestActivateAppClient" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-req-active-app-client").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>

    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Request Activate App Client</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                    <thead>
                                        <tr>
                                            <th class="text-nowrap"></th>
                                            <th class="text-nowrap">Req. Date</th>
                                            <th class="text-nowrap">App ID</th>
                                            <th class="text-nowrap">App. Permission Key</th>
                                            <th class="text-nowrap">Cop. Permission key</th>
                                            <th class="text-nowrap">Acception Status</th>
                                            <th class="text-nowrap">Acception By</th>
                                            <th class="text-nowrap">Acception Date</th>
                                            <th class="text-nowrap">Activation Status</th>
                                            <th class="text-nowrap">Activation Date</th>
                                            <th class="text-nowrap">Email</th>
                                            <th class="text-nowrap">Remark</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptItems" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="text-nowrap text-center align-middle">
                                                        <%#  Eval("AcceptionStatus").ToString() == "WAITING" ? "<button type='button' class='btn btn-success btn-sm mb-1 AUTH_MODIFY' onclick='confirmApprove(this,\""+Eval("SEQ").ToString()+"\",\""+Eval("ApplicationID").ToString()+"\",\""+Eval("ApplicationPermissionKey").ToString()+"\",\""+Eval("CorporatePermissionKey").ToString()+"\",\""+Eval("Email").ToString()+"\");'><i class='fa fa-check'></i>&nbsp;Approve</button>&nbsp;<button type='button' class='btn btn-danger btn-sm mb-1 AUTH_MODIFY' onclick='confirmReject(this,\""+Eval("SEQ").ToString()+"\",\""+Eval("ApplicationID").ToString()+"\",\""+Eval("CorporatePermissionKey").ToString()+"\",\""+Eval("Email").ToString()+"\");'><i class='fa fa-times'></i>&nbsp;Reject</button>": ""%>
                                             
                                                        
                                                    </td>
                                                    <td class="text-nowrap">
                                                      <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("RequestDateTime").ToString())%>
                                                    </td>
                                                    
                                                    <td class="text-nowrap">
                                                      <%# Eval("ApplicationID") %>
                                                    </td> 
                                                     <td class="text-nowrap">
                                                      <%# Eval("ApplicationPermissionKey") %>
                                                    </td> 
                                                    <td class="text-nowrap">
                                                      <%# Eval("CorporatePermissionKey") %>
                                                    </td> 
                                                    <td class="text-nowrap">
                                                      <%# Eval("AcceptionStatus") %>
                                                    </td> 
                                                    <td class="text-nowrap">
                                                      <%# Eval("AcceptionBy") %>
                                                    </td> 
                                                    <td class="text-nowrap">
                                                      <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("AcceptionDateTime").ToString()) %>
                                                    </td> 
                                                    <td class="text-nowrap">
                                                      <%# Eval("ActivationStatus") %>
                                                    </td> 
                                                    <td class="text-nowrap">
                                                      <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("ActivationDateTime").ToString()) %>
                                                    </td> 
                                                    <td class="text-nowrap">
                                                      <%# Eval("Email") %>
                                                    </td> 
                                                    <td class="text-nowrap">
                                                      <%# Eval("Remark") %>
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

    <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                     <asp:HiddenField ID="hddSeq" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hddAppId" runat="server" ClientIDMode="Static" />
                    
                    <asp:HiddenField ID="hddCopPerKey" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hddEmail" runat="server" ClientIDMode="Static" />
                    <asp:Button Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone"
                        ID="btnApprove" ClientIDMode="Static"
                        OnClick="btnApprove_Click" OnClientClick="AGLoading(true);" />
                    <asp:Button Text="" runat="server" CssClass="d-none ticket-allow-editor ticket-allow-editor-everyone"
                        ID="btnReject" ClientIDMode="Static"
                        OnClick="btnReject_Click" OnClientClick="AGLoading(true);" />
                </ContentTemplate>
            </asp:UpdatePanel>
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


        function confirmApprove(obj,seq,appid,appperkey,copperkey,email) {
            
             if (AGConfirm(obj, 'Confirm Approve ?')) {
                 $("#<%=hddSeq.ClientID %>").val(seq);
                 $("#<%=hddAppId.ClientID %>").val(appid);
                
                 $("#<%=hddCopPerKey.ClientID %>").val(copperkey);
                 $("#<%=hddEmail.ClientID %>").val(email);
                 $("#<%=btnApprove.ClientID %>").click();
             
             }
             
        }
        function confirmReject(obj,seq,appid,copperkey,email) {
            
             if (AGConfirm(obj, 'Confirm Reject ?')) {
                 $("#<%=hddSeq.ClientID %>").val(seq);
                  $("#<%=hddAppId.ClientID %>").val(appid);
                 $("#<%=hddCopPerKey.ClientID %>").val(copperkey);
                 $("#<%=hddEmail.ClientID %>").val(email);
                  $("#<%=btnReject.ClientID %>").click();
               
             }
            
        }
    </script>
</asp:Content>
