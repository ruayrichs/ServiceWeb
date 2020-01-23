<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="AG_ClientService.aspx.cs" Inherits="ServiceWeb.TTM_Training.AG_ClientService" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpUpdate">
            <ContentTemplate>
                <asp:Button Text="Search" runat="server" ID="btnSearch"
                    CssClass="btn btn-primary"
                    OnClick="btnSearch_Click" 
                    OnClientClick="AGLoading(true);"
                     /> 
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
    <script>
    </script>
</asp:Content>
