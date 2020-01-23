<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="TestRPT.aspx.cs" Inherits="ServiceWeb.TestRPT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <asp:Repeater runat="server" ID="rpt1" OnItemDataBound="rpt1_ItemDataBound">
            <ItemTemplate>
                <div class="col-12">
                    <%# Eval("col2") %>
                    <asp:HiddenField runat="server" ID="hdd1" Value='<%# Eval("col1") %>' />

                    <asp:Repeater runat="server" ID="rpt2" OnItemDataBound="rpt2_ItemDataBound">
                        <ItemTemplate>
                            <asp:Panel runat="server" ID="panel1">
                                <div style="margin-left: 10px;">
                                    <%# Eval("rpt2_col2") %>
                                </div>
                            </asp:Panel>
                            <div style="margin-left: 10px;">
                                <%# Eval("col1") %>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
