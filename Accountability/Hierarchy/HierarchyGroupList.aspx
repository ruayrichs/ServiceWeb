<%@ Page Title="" Language="C#" MasterPageFile="~/Accountability/MasterPage/AccountabilityMaster.master" AutoEventWireup="true"  CodeBehind="HierarchyGroupList.aspx.cs" Inherits="ServiceWeb.Accountability.Hierarchy.HierarchyGroupList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-role-hierarchy").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <span class="page-header">
        <button type="button" style="margin-top:-10px;" class="btn btn-primary AUTH_MODIFY" data-toggle="modal" data-target="#ModalAddHierarchyGroup">
            <i class="fa fa-plus"></i>
            Create new hierarchy group
        </button>
        
    </span>
    <div>
          <div class="card shadow">
            <div class="card-header">
                <h5 class="mb-0">Role Hierarchy</h5>
            </div>


            <div class="card-body panel-body-customer-search PANEL-DEFAULT-BUTTON">
                  <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpList">
            <ContentTemplate>
                <table class="table table-striped table-hover table-bordered">
                    <tr>
                        <th>Hierarchy Group
                        </th>
                        <th style="text-align: center; width: 60px;">Manage
                        </th>
                        <th style="text-align: center; width: 60px;">Delete
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptHierarchy">
                        <ItemTemplate>
                            <tr class="c-pointer" data-key="groupcode=<%# Eval("HIERARCHYGROUPCODE") %>&WorkGroupCode=<%# Request["WorkGroupCode"] %>">
                                
                                <td onclick="RedirectPageLinkToDetail(this);">
                                    <%# Eval("HIERARCHYGROUPNAME") %>
                                </td>
                                <td class="text-center" onclick="RedirectPageLinkToDetail(this);">
                                    <a href="HierarchyDetail.aspx?groupcode=<%# Eval("HIERARCHYGROUPCODE") %>&WorkGroupCode=<%# Request["WorkGroupCode"] %>"><i class="fa fa-cogs"></i></a>
                                </td>
                                <td  class="text-center">
                                    <i class="fa fa-trash hand" onclick="$(this).next().click();"></i>
                                    <asp:Button Text="Del" runat="server" CssClass="btn btn-danger btn-sm hide" ID="btnDeleteGroup"
                                        CommandArgument='<%# Eval("HIERARCHYGROUPCODE") %>' OnClick="btnDeleteGroup_Click" OnClientClick="return ConfirmDelete();" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>

            </div>
          </div>
      
    </div>

    <script>
        function ConfirmDelete() {
            if (AGConfirm('คุณต้องการลบใช่หรือไม่')) {
                AGLoading(true);
                event.preventDefault();
                return true;
            } else {
                return false;
            }
        }

        function RedirectPageLinkToDetail(obj)
        {
            var paramstring = $(obj).closest("tr").data("key");
            var dataKey = "";
            var Question = "";
            if (paramstring != null && paramstring != undefined && paramstring != "")
            {
                Question = "?";
            }
            var url = 'HierarchyDetail.aspx' + Question + paramstring;
            var form = $('<form action="' + url + '" method="post">' +
              '<input type="text" name="api_url" value="' + dataKey + '" />' +
              '</form>');
            $('body').append(form);
            form.submit();
        }
    </script>

    <div id="ModalAddHierarchyGroup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">เพิ่มกลุ่ม Hierarchy</h4>
                    <button type="button" class="close" data-dismiss="modal">×</button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpAddGroup">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-12 col-md-6">
                                    <label>
                                        Hierarchy group code
                                    </label>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtCode" placeholder="Hierarchy group code" MaxLength="20" />
                                </div>
                                <div class="col-sm-12 col-md-6">
                                    <label>
                                        Hierarchy group name
                                    </label>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtName" placeholder="Hierarchy group name" MaxLength="500" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button Text="Save" runat="server" ID="btnAddGroup" CssClass="btn btn-primary AUTH_MODIFY"
                                OnClientClick="AGLoading(true);" OnClick="btnAddGroup_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
