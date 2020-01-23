<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="Nos_Testing.aspx.cs" Inherits="ServiceWeb.TTM_Training.Nos_Testing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpUpdate">
            <ContentTemplate>
                <div>
                    <asp:TextBox ID="txt_search" TextMode="multiline" runat="server" /></div>
                <br />
                <asp:Button Text="Search" runat="server" ID="btnSearch"
                    CssClass="btn btn-primary"
                    OnClick="btn_search"
                    OnClientClick="AGLoading(true);" />
                <button type="button" class="btn btn-info" data-toggle="modal" data-target="#myModal">Create Customer</button>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <hr />
    <div>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdateTableCusDetail">
            <ContentTemplate>
                <table class="table table-bordered">
                    <tr>
                        <th>แก้ไข</th>
                        <th>ลบข้อมูล</th>
                        <th>รหัส</th>
                        <th>ชื่อ</th>
                        <th>ประเภท</th>
                        <th>จังหวัด</th>
                        <th>เบอร์โทร</th>
                        <th>อีเมล์</th>
                        <th>ประเภทผู้ติดต่อ</th>
                        <th>เขตการขาย</th>
                    </tr>
                    <asp:Repeater runat="server" ID="TableCusDetail">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Button Text="Edit" runat="server" ID="editcus"
                                        CssClass="btn btn-primary"
                                        OnClick="btnEdit_Click"
                                        CommandArgument='<%# Eval("CustomerCode") %>'
                                        data-toggle="modal"
                                        data-target="#myModaledit" />
                                </td>
                                <td>
                                    <asp:Button Text="delete" runat="server" ID="deletecus"
                                        CssClass="btn btn-primary"
                                        OnClick="btn_delete_cus"
                                        CommandArgument='<%# Eval("CustomerCode") %>'
                                        OnClientClick="AGLoading(true);" />
                                </td>
                                <td><%# Eval("CustomerCode") %></td>
                                <td><%# Eval("CustomerName") %></td>
                                <td><%# Eval("CustomerType") %></td>
                                <td><%# Eval("Province") %></td>
                                <td><%# Eval("Tel") %></td>
                                <td><%# Eval("Email") %></td>
                                <td><%# Eval("ContactType") %></td>
                                <td><%# Eval("SaleArea") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!-- Main Modal-->
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Modal Header</h4>
                    <button type="button" class="close" data-dismiss="modal">×</button>
                </div>
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanelCreate">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div>ชื่อ :
                                <asp:TextBox ID="Name" runat="server" /></div>
                            <div>ประเภท :
                                <asp:TextBox ID="Type" runat="server" /></div>
                            <div>จังหวัด :
                                <asp:TextBox ID="Province" runat="server" /></div>
                            <div>เบอร์โทรศัพท์ :
                                <asp:TextBox ID="Phone" runat="server" /></div>
                            <div>อีเมล :
                                <asp:TextBox ID="Email" runat="server" /></div>
                            <div>
                                ประเภทผู้ติดต่อ :
                                <asp:DropDownList ID="TypeConCus" runat="server">
                                    <asp:ListItem>ผู้รับจ้างชั่วคราว</asp:ListItem>
                                    <asp:ListItem>คู่ค้าร่วม</asp:ListItem>
                                    <asp:ListItem>คู่แข่งขันทางธุรกิจ</asp:ListItem>
                                    <asp:ListItem>ลูกค้า</asp:ListItem>
                                    <asp:ListItem>เจ้าหน้าที่บริษัท</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div>
                                เขตการขาย :
                                <asp:DropDownList ID="TypeSell" runat="server">
                                    <asp:ListItem>ภาคใต้</asp:ListItem>
                                    <asp:ListItem>ภาคกลาง</asp:ListItem>
                                    <asp:ListItem>ภาคอีสาน</asp:ListItem>
                                    <asp:ListItem>ภาเหนือ</asp:ListItem>
                                    <asp:ListItem>ภาคอีสาน</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-footer">
                    <asp:Button Text="บันทึก" runat="server" ID="btncreate"
                        CssClass="btn btn-primary"
                        OnClick="btn_create"
                        OnClientClick="AGLoading(true);" />
                    <button type="button" class="btn btn-default" data-dismiss="modal">ปิด</button>
                </div>
            </div>
        </div>
    </div>
    <!-- Main Modal-->

     <div id="myModaledit" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Modal Header</h4>
                    <button type="button" class="close" data-dismiss="modal">×</button>
                </div>
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="EditPanel">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div>รหัสลูกค้า :
                                <asp:TextBox ID="Cusid" runat="server" /></div>
                            <div>ชื่อ :
                                <asp:TextBox ID="Nameed" runat="server" /></div>
                            <div>ประเภท :
                                <asp:TextBox ID="Typeed" runat="server" /></div>
                            <div>จังหวัด :
                                <asp:TextBox ID="Provinceed" runat="server" /></div>
                            <div>เบอร์โทรศัพท์ :
                                <asp:TextBox ID="Phoneed" runat="server" /></div>
                            <div>อีเมล :
                                <asp:TextBox ID="Emailed" runat="server" /></div>
                            <div>
                                ประเภทผู้ติดต่อ :
                                <asp:DropDownList ID="TypeConCused" runat="server">
                                    <asp:ListItem>ผู้รับจ้างชั่วคราว</asp:ListItem>
                                    <asp:ListItem>คู่ค้าร่วม</asp:ListItem>
                                    <asp:ListItem>คู่แข่งขันทางธุรกิจ</asp:ListItem>
                                    <asp:ListItem>ลูกค้า</asp:ListItem>
                                    <asp:ListItem>เจ้าหน้าที่บริษัท</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div>
                                เขตการขาย :
                                <asp:DropDownList ID="TypeSelled" runat="server">
                                    <asp:ListItem>ภาคใต้</asp:ListItem>
                                    <asp:ListItem>ภาคกลาง</asp:ListItem>
                                    <asp:ListItem>ภาคอีสาน</asp:ListItem>
                                    <asp:ListItem>ภาเหนือ</asp:ListItem>
                                    <asp:ListItem>ภาคอีสาน</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="modal-footer">
                    <asp:Button Text="บันทึก" runat="server" ID="btnsaveedit"
                        CssClass="btn btn-primary"
                        OnClick="btn_save_edit"
                        OnClientClick="AGLoading(true);" />
                    <button type="button" class="btn btn-default" data-dismiss="modal">ปิด</button>
                </div>
            </div>
        </div>
    </div>
    <script>
</script>
</asp:Content>
