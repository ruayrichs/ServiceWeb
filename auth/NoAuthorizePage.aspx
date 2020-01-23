<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="NoAuthorizePage.aspx.cs" Inherits="ServiceWeb.auth.NoAuthorizePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="padding: 20px; text-align: center; height: 450px;">
        <div class="alert alert-danger" style="text-align: center;">
            ไม่สามารถใช้งานหน้า <asp:Label ID="lblUrlAUTH" CssClass="text-primary" Text="" runat="server" /> ดังกล่าวได้ กรุณาติดต่อเจ้าหน้าที่เพื่อเปิดใช้สิทธิ์ในหน้าจอนี้ !!
        </div>
    </div>
</asp:Content>
