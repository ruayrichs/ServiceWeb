<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="CSharpTraining.aspx.cs" Inherits="ServiceWeb.CSharpTraining" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
    </div>
    <br />
    <div class="container">
        <div class="card">
            <br />
            <h5 class="card-header">รายการผู้ติดต่อสมาชิก</h5>
            <div class="col-6 form-inline">
                <br />
                <asp:TextBox runat="server" CssClass="form-control small" ID="txtSearch" placeholder="ชื่อ" />
            </div>
            <br />
              <div>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpUpdate">
            <ContentTemplate>
                <asp:Button Text="ค้นหา" runat="server" ID="btnSearch"
                    CssClass="btn btn-primary"
                    OnClick="btnSearch_Click" 
                    OnClientClick="AGLoading(true);"
                     />  
                <button type="button" class="btn btn-info btn-md" data-toggle="modal" data-target="#myModal">Open Modal</button>                
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    <hr />
    <div>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpDataList">
            <ContentTemplate>
                <table class="table table-bordered">
                    <tr>
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
                                    <asp:Button Text="Edit" runat="server" ID="btnEdit" OnClick="btnEdit_Click" />
                                </td>
                                <td>
                                    <asp:Button Text="Delete" runat="server" ID="btnDelete" OnClick="btnDelete_Click" 
                                        CommandArgument='<%# Eval("CustomerCode") %>' />
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



          <%--<!-- Modal-->
           <div class="modal fade" id="myModal" role="dialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                   <div class="container">
                        <h2>Basic Card</h2>
                        <div class="card">
                            <div class="card-body">
                                <h5 class="card-header">รายการผู้ติดต่อสมาชิก</h5>
                                <div class="row">
                                    <div class="col">
                                        <form>
                                            <div class="form-group">
                                                <label for="exampletype">ประเภท</label>
                                                <asp:DropDownList runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="ทั้งหมด" />
                                                    <asp:ListItem Text="item 1" />
                                                    <asp:ListItem Text="item 2" />
                                                </asp:DropDownList>
                                                <label for="exampleBandwidth">เขตการขาย</label>
                                                <asp:DropDownList runat="server" CssClass="form-control">
                                                     <asp:ListItem>ภาคใต้</asp:ListItem>
                                                     <asp:ListItem>ภาคกลาง</asp:ListItem>
                                                     <asp:ListItem>ภาคอีสาน</asp:ListItem>
                                                     <asp:ListItem>ภาเหนือ</asp:ListItem>
                                                     <asp:ListItem>ภาคอีสาน</asp:ListItem>
                                                </asp:DropDownList>
                                                <label>เบอร์โทรศัพท์</label>
                                                <input type="text" class="form-control" id="TelPhone" placeholder="ค้นหาตามเบอร์โทรศัพท์">
                                                <label for="contact">ประเภทผู้ติดต่อ</label>
                                                <asp:DropDownList runat="server" CssClass="form-control">
                                                    <asp:ListItem>ผู้รับจ้างชั่วคราว</asp:ListItem>
                                                    <asp:ListItem>คู่ค้าร่วม</asp:ListItem>
                                                    <asp:ListItem>คู่แข่งขันทางธุรกิจ</asp:ListItem>
                                                    <asp:ListItem>ลูกค้า</asp:ListItem>
                                                    <asp:ListItem>เจ้าหน้าที่บริษัท</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="container">
                                                <div class="row">
                                                    <div class="col">
                                                        <input type="checkbox" class="form-check-input" id="Check1">
                                                        <label class="form-check-label" for="exampleCheck1">Table View</label>
                                                    </div>

                                                    <div class="col">
                                                        <input type="checkbox" class="form-check-input" id="Check2">
                                                        <label class="form-check-label" for="exampleCheck1">Card View</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <button type="submit" class="btn btn-primary">บันทึก</button>
                                            <button type="submit" class="btn btn-warning">ปิด</button>

                                        </form>
                                    </div>

                                    <div class="col">
                                        <form>
                                            <div class="form-group">
                                                <label>รหัสลูกค้า / ชื่อลูกค้า</label>
                                                <input type="text" class="form-control" id="FindName" placeholder="ค้นหาตามชื่อ">
                                                <label for="exampleBandwidth">เขตการขาย</label>
                                                <label for="contact">จังหวัด</label>
                                                <asp:DropDownList runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="ค้นหาตามจังหวัด" />
                                                    <asp:ListItem Text="item 1" />
                                                    <asp:ListItem Text="item 2" />
                                                </asp:DropDownList>
                                                <label>E-mail</label>
                                                <input type="text" class="form-control" id="FindMail" placeholder="ค้นหาตาม Email">
                                                <label>Universal Structure</label>
                                                <asp:DropDownList runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="เลือก Universal" />
                                                    <asp:ListItem Text="item 1" />
                                                    <asp:ListItem Text="item 2" />
                                                </asp:DropDownList>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>--%>
</asp:Content>
