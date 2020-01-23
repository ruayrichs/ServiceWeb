<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="ServiceWeb.TTM_Training.TestPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function myTest() {
            var data={
                request1: "something1",
                request2: "something2"
            }
            $.ajax({
                type: 'POST',
                url: "<%= Page.ResolveUrl("~/API/TTM_TestAPI/TestAPI.aspx") %>",
                data: data,
                success: function(data){
                    //console.log(JSON.stringify(data));
                    //data;
                    var result = JSON.parse(data);
                    console.log(result);
                },
                error: function (error) {
                    console.log(error);
                }
            })
        };
        function webOnLoad() {
            //myTest();
        };

        $(document).ready(function () { webOnLoad(); });
    </script>
    <div id="body">
        <asp:TextBox runat="server" ID="password_input" placeholder="passwword" CssClass="password_input" Enabled="false" />
        <style>
            .password_input{
                border:none;
                background:none;
            }
        </style>
        <asp:TextBox runat="server" ID="length_input" placeholder="length"/>
        <asp:Button runat="server" ID="genPWD_button" Text="gen password" CssClass="btn btn-success" OnClick="genPWD_button_Click"/><br />
        
        <asp:TextBox runat="server" ID="email_input" placeholder="reciver email address"/>
        <asp:Button runat="server" ID="send_button" Text="Send Now!!" CssClass="btn btn-success" OnClick="send_button_Click"/><br />
        
    </div>
</asp:Content>
