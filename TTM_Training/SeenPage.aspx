<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="SeenPage.aspx.cs" Inherits="ServiceWeb.TTM_Training.SeenPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="container">
            <div class="row col-4">
                <h5>รายการลูกค้า</h5>
                <br><br>
                <h5>ชื่อ</h5>
                <asp:TextBox runat="server" CssClass="form-control" ID="search_customer_name" />
            </div>
            <asp:Button runat="server" Text="ค้นหา" CssClass="btn btn-primary" ID="customer_search" OnClick="customer_search_Click"/>
            <button type="button" class="btn btn-success" data-toggle="modal" data-target="#myModal">สร้าง</button>

            <!-- Modal -->
            <div id="myModal" class="modal fade" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">สร้างลูกค้า</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-4">
                                    <span>ชื่อ</span>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="customer_name"/>
                                    <span>เบอร์โทรศัพ</span>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="customer_tel"/>
                                    <span>เขตการขาย</span>
                                    <asp:DropDownList id="DDL_customer_salearea" runat="server" CssClass="form-control">
                                        <asp:ListItem Enabled="true" Text="เลือก เขตการขาย" Value="-1"/>
                                        <asp:ListItem Text="ภาคเหนือ" Value="ภาคเหนือ"/>
                                        <asp:ListItem Text="ภาคใต้" Value="ภาคใต้"/>
                                        <asp:ListItem Text="ภาคตะวันออก" Value="ภาคตะวันออก"/>
                                        <asp:ListItem Text="ภาคตะวันตก" Value="ภาคตะวันตก"/>
                                        <asp:ListItem Text="ภาคตะวันออกเฉียงเหนือ" Value="ภาคตะวันออกเฉียงเหนือ" />
                                    </asp:DropDownList>
                                </div>
                                <div class="col-4">
                                    <span>ประเภท</span>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="customer_type"/>
                                    <span>อีเมล</span>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="customer_email"/>
                                </div>
                                <div class="col-4">
                                    <span>จังหวัด</span>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="customer_province"/>
                                    <span>ประเภทผู้ติดต่อ</span>
                                    <asp:DropDownList id="DDL_customer_contracttype" runat="server" CssClass="form-control">
                                        <asp:ListItem Enabled="true" Text="เลือก เขตการขาย" Value="-1"/>
                                        <asp:ListItem Text="ผู้รับจ้างชั่วคราว" Value="ผู้รับจ้างชั่วคราว"/>
                                        <asp:ListItem Text="คู่ค้าร่วม" Value="คู่ค้าร่วม"/>
                                        <asp:ListItem Text="คู่แข่งขันทางธุรกิจ" Value="คู่แข่งขันทางธุรกิจ"/>
                                        <asp:ListItem Text="ลูกค้า" Value="ลูกค้า"/>
                                        <asp:ListItem Text="เจ้าหน้าที่บริษัท" Value="เจ้าหน้าที่บริษัท" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <button type="button" class="btn btn-warning" data-dismiss="modal">ออก</button>
                            <!-- button type="button" class="btn btn-primary" data-dismiss="modal">บันทึก</ -->
                            <asp:Button runat="server" Text="บันทึก" CssClass="btn btn-primary" ID="btn_customer_save" OnClick="btn_customer_save_Click"/>
                        </div>
                    </div>
                </div>
            </div>
            <!-- end modal -->
        </div>

        <!-- search result -->
        <div>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpDataList">
                <ContentTemplate>
                    <table class="table table-bordered">
                        <tr>
                            <th>แก้ไข</th>
                            <th>ลบ</th>
                            <th>รหัส</th>
                            <th>ชื่อ</th>
                            <th>ประเภท</th>
                            <th>จังหวัด</th>
                            <th>เบอร์โทร</th>
                            <th>อีเมล์</th>
                            <th>ประเภทผู้ติดต่อ</th>
                            <th>เขตการขาย</th>
                        </tr>
                        <asp:Repeater runat="server" ID="rptCusDetail">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Button Text="Edit" runat="server" ID="btnEdit" CommandArgument='<%# Eval("CustomerCode") %>' OnClick="btnEdit_Click" />
                                    </td>
                                    <td>
                                        <asp:Button Text="Delete" runat="server" ID="btnDelete" CommandArgument='<%# Eval("CustomerCode") %>' OnClick="btnDelete_Click" />
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
    </div>
</asp:Content>
