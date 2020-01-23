<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="TrainPage.aspx.cs" Inherits="ServiceWeb.TTM_Training.TrainPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:textbox runat="server" /><br/>
        <asp:button runat="server" text="button" /><br/>
        <asp:dropdownlist runat="server">
            <asp:listitem text="list1text" value="list1value" />
            <asp:listitem text="list2text" value="list2value" />
        </asp:dropdownlist><br/>
        <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table>
                    <thead>
                        <tr>
                            <th>callstatus</th>
                            <th>callerID</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptItem" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td class="text-nowrap"><%# Eval("CallStatus") %></td>
                                    <td class="text-nowrap"><%# Eval("CallerID") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
