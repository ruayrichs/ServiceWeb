<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="TestCallAPI.aspx.cs" Inherits="ServiceWeb.TestCallAPI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button Text="Test API" runat="server" ID="btnTestAPI"
                    OnClick="btnTestAPI_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
