<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="SendEmail.aspx.cs" Inherits="ServiceWeb.TTM_Training.SendEmail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:textbox runat="server" id="genpass_inp" />
            <asp:button runat="server" text="gen password" id="genpass_btn" onclick="genpass_btn_Click" />
            <asp:textbox runat="server" id="email_inp" placeholder="email" />
            <asp:textbox runat="server" id="tel_inp" placeholder="tel" />
            <asp:button runat="server" text="send_btn" ID="send_btn" OnClick="Unnamed_Click" /><br />
            <div>
                <div>UpdateSEQ and SendEmail</div>
                <asp:textbox runat="server" id="email2_inp" placeholder="email" /></br>
                <asp:textbox runat="server" id="seq_inp" placeholder="SEQ" /></br>
                <asp:DropDownList runat="server" ID="status_inp">
                    <asp:ListItem Text="APPROVE" Value="APPROVE" />
                    <asp:ListItem Text="REJECT" Value="REJECT" />
                </asp:DropDownList></br>
                <asp:button runat="server" text="send mail" ID="sendEmail_btn" OnClick="sendEmail_btn_Click" /><br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
