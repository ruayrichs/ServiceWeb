<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Born_page.aspx.cs" Inherits="ServiceWeb.TTM_Training.Born_page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
       <div class="container">
              <div class="col-6 form-inline">
                   <asp:TextBox Text="" runat="server" CssClass="form-control small" ID="txtSearch" placeholder="ชื่อ"/>
              </div>
   
            <br />
            <div class="col-12 form-inline">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpUpdate">
                    <ContentTemplate>
                            <asp:Button Text="ค้นหา" runat="server" CssClass="btn btn-primary "
                        ID="btnSearch"   OnClick="btnSearch_Click" />
                            <asp:Button Text="สร้างใหม่" runat="server" CssClass="btn "
                        ID="btnNew" data-toggle="modal" data-target="#customer" style="background-color:darkblue;color: #FFFFFF;" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <br />
        </div>




        <!-- Modal -->
        <div class="modal fade" ID="customer" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" ClientIDMode="Static">
          <div class="modal-dialog" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">รายชื่อผู้ติดต่อ/สมาชิก</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
                <div class="modal-content">
                     <div class="container card">
                <div class="card-body">
                <div class="col-12 row">
                    <div class="col-6">
                         <asp:Label Text ="ชื่อลูกค้า" runat="server" CssClass="font-weight-bold small"/>
                        <asp:TextBox  Text="" runat="server" CssClass="form-control" 
                        ID="txtName" placeholder="ชื่อ" />
                        <br />
                        <asp:Label Text ="ประเภท" runat="server" CssClass="font-weight-bold"/>
                        <asp:DropDownList runat="server" CssClass="form-control small text-primary"  ID="txtType" >
                            <asp:ListItem Text="ทั้งหมด" />
                            <asp:ListItem Text="text" />
                            <asp:ListItem Text="text" />
                        </asp:DropDownList>
                        <br />
                        <asp:Label Text ="เขตการขาย" runat="server" CssClass="font-weight-bold"/>
                        <asp:DropDownList runat="server" CssClass="form-control small text-primary" ID="txtArea">
                            <asp:ListItem Text="เลือกทั้งหมด" />
                            <asp:ListItem Text="text" />
                            <asp:ListItem Text="text" />
                        </asp:DropDownList>
                        <br />
                        <asp:Label Text ="เบอร์โทรศัพท์" runat="server" CssClass="font-weight-bold"/>
                        <asp:TextBox  Text="" runat="server" CssClass="form-control small"  
                        ID="txtTel" placeholder="เบอร์โทรศัพท์" />
                        
                        
               
                    </div>
                    <div class="col-6">
                       <asp:Label Text ="ประเภทผู้ติดต่อ" runat="server" CssClass="font-weight-bold small"/>
                        <asp:DropDownList runat="server" CssClass="form-control small text-primary" ID="txtConType" >
                            <asp:ListItem Text="กรุณาเลือก" />
                            <asp:ListItem Text="text" />
                            <asp:ListItem Text="text" />
                        </asp:DropDownList>
                        <br />
                        <asp:Label Text ="จังหวัด" runat="server" CssClass="font-weight-bold small"/>
                        <asp:TextBox  Text="" runat="server" CssClass="form-control" 
                        ID="txtProvine" placeholder="จังหวัด" />
                        <br />
                        <asp:Label Text ="E-mail" runat="server" CssClass="font-weight-bold small"/>
                        <asp:TextBox  Text="" runat="server" CssClass="form-control" 
                        ID="txtMail" placeholder="emal" />
                    </div>
                </div>
               
         
                </div>
            </div>
                </div>
              <div class="modal-footer">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                    <ContentTemplate>
                            <asp:Button Text="Close" runat="server" CssClass="btn btn-danger "
                        ID="btnClose" data-dismiss="modal"    />
                            <asp:Button Text="Save" runat="server" CssClass="btn "
                        ID="btnSave"  style="background-color:green;color: #FFFFFF;"  OnClick="btnSave_Click"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
                
              </div>
            </div>
          </div>
        </div>

    <!-- Modal Edit-->
     
            <ContentTemplate>
                     <div class="modal fade" ID="customer_edit" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" ClientIDMode="Static">
          <div class="modal-dialog" role="document">
              <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="update_edit">
                   <ContentTemplate>
                          <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">แก้ไข รายชื่อผู้ติดต่อ/สมาชิก</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
                <div class="modal-content">
                     <div class="container card">
                <div class="card-body">
                <div class="col-12 row">
                    <div class="col-6">
                         <asp:Label Text ="ชื่อลูกค้า" runat="server" CssClass="font-weight-bold small"/>
                        <asp:TextBox  Text="" runat="server" CssClass="form-control" 
                        ID="txtEditName" placeholder="ชื่อ" />
                        <br />
                        <asp:Label Text ="ประเภท" runat="server" CssClass="font-weight-bold" />
                        <asp:DropDownList runat="server" CssClass="form-control small text-primary" ID="txtEditType">
                            <asp:ListItem Text="ทั้งหมด" />
                            <asp:ListItem Text="text" />
                            <asp:ListItem Text="text" />
                        </asp:DropDownList>
                        <br />
                        <asp:Label Text ="เขตการขาย" runat="server" CssClass="font-weight-bold"/>
                        <asp:DropDownList runat="server" CssClass="form-control small text-primary" ID="txtEditArea">
                            <asp:ListItem Text="เลือกทั้งหมด" />
                            <asp:ListItem Text="text" />
                            <asp:ListItem Text="text" />
                        </asp:DropDownList>
                        <br />
                        <asp:Label Text ="เบอร์โทรศัพท์" runat="server" CssClass="font-weight-bold"/>
                        <asp:TextBox  Text="" runat="server" CssClass="form-control small"  
                        ID="txtEditTel" placeholder="เบอร์โทรศัพท์" />
                    </div>
                    <div class="col-6">
                       <asp:Label Text ="ประเภทผู้ติดต่อ" runat="server" CssClass="font-weight-bold small"/>
                        <asp:DropDownList runat="server" CssClass="form-control small text-primary" ID="txtEditConType" >
                            <asp:ListItem Text="กรุณาเลือก" />
                            <asp:ListItem Text="text" />
                            <asp:ListItem Text="text" />
                        </asp:DropDownList>
                        <br />
                        <asp:Label Text ="จังหวัด" runat="server" CssClass="font-weight-bold small"/>
                        <asp:TextBox  Text="" runat="server" CssClass="form-control" 
                        ID="txtEditProvine" placeholder="จังหวัด" />
                        <br />
                        <asp:Label Text ="E-mail" runat="server" CssClass="font-weight-bold small"/>
                        <asp:TextBox  Text="" runat="server" CssClass="form-control" 
                        ID="txtEditEmail" placeholder="emal" />
                    </div>
                </div>
               
         
                </div>

            </div>
                </div>
              <div class="modal-footer">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                    <ContentTemplate>
                            <asp:Button Text="Close" runat="server" CssClass="btn btn-danger "
                        ID="Button1" data-dismiss="modal"    />
                            <asp:Button Text="Save" runat="server" CssClass="btn "
                        ID="Button2" data-dismiss="modal" style="background-color:green;color: #FFFFFF;"  OnClick="btnSaveEdit_Click"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
                
              </div>
            </div>
                              </ContentTemplate>

     </asp:UpdatePanel>
          </div>
        </div>


   
          <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="update_cutomers_list">
            <ContentTemplate>
                  <div class="container">
            <table class="table">
              <thead>
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
              </thead>
              <tbody>
                <asp:Repeater runat="server" ID="customer_lists">
                     <ItemTemplate>
                            <tr>
                                <td><asp:Button Text="แก้ไข" runat="server" CssClass="btn btn-secondary "
                        ID="btnEdit" data-toggle="modal" data-target="#customer_edit"  OnClick="btnEdit_Click"  CommandArgument='<%# Eval("CustomerCode") %>' /></td>
                                <td>
                                    <asp:Button Text="ลบ" runat="server" CssClass="btn btn-danger "
                        ID="btnDelete" OnClick="btnDelete_Click"   CommandArgument='<%# Eval("CustomerCode") %>' /></td>
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
              </tbody>
            </table>
        </div>

            </ContentTemplate>
          </asp:UpdatePanel>
      

</asp:Content>
