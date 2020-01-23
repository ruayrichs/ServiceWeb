<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="PageTast.aspx.cs" Inherits="ServiceWeb.PageTast" %>

<%@ Register Src="~/UserControl/ActivitySendMailModal.ascx" TagPrefix="uc1" TagName="ActivitySendMailModal" %>


<%--<%@ Register Src="~/widget/usercontrol/SearchHelpCIControl.ascx" TagPrefix="uc1" TagName="SearchHelpCIControl" %>
<%@ Register Src="~/widget/usercontrol/SearchCustomerControl.ascx" TagPrefix="uc1" TagName="SearchCustomerControl" %>--%>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script>
        function myTest() {

            var data = { // Set Paramiter Request : คือการส่งค่าผ่าน URl
                request1: "Something request 1",
                request2: "Something request 2",
            }

            $.ajax({
                type: 'POST', // Request Type เป็น POST หรือ GET
                url: "/API/WebFormTestAPI.aspx", // กำหนดที่อยู่ API
                data: data, // ส่ง Request ที่กำหนดใว้
                success: function (data) {
                    var result = JSON.parse(data); // เอาผลลัพที่ได้มาแปลงเป็น Json Object แล้วเอาไปใช้ทำ Process ได้เลยครับ
                    // data :: คือตัวแปลที่ Return มาจาก API

                    console.log(result.OverviewDataReport);
                    console.log(result.BarChartDataReport);
                    console.log(result.PieChartDataReport);
                    console.log(result.StatusDataReport);
                    console.log(result.PiorityDataReport);
                },
                error: function (error) {
                    // error :: คือ Object Error :: มีอะไรเยอะแยะไปหมด :: ** ไม่ต้องสนใจก็ได้ถ้า Function ใน API ถูก
                    console.log(error);
                }
            });
        }


        function teatAPIAppClientRequest(AppID, CorpoKey, Email) {
            var data = { // Set Paramiter Request : คือการส่งค่าผ่าน URl
                ApplicationID: AppID,
                CorporatePermissionKey: CorpoKey,
                Email: Email
            }

            $.ajax({
                type: 'POST', // Request Type เป็น POST หรือ GET
                url: "/API/AppClient/AppClientRequestActivation.aspx", // กำหนดที่อยู่ API
                data: data, // ส่ง Request ที่กำหนดใว้
                success: function (data) {
                    console.log(data);
                    var result = JSON.parse(data); // เอาผลลัพที่ได้มาแปลงเป็น Json Object แล้วเอาไปใช้ทำ Process ได้เลยครับ
                    // data :: คือตัวแปลที่ Return มาจาก API

                    console.log(result);
                },
                error: function (error) {
                    // error :: คือ Object Error :: มีอะไรเยอะแยะไปหมด :: ** ไม่ต้องสนใจก็ได้ถ้า Function ใน API ถูก
                    console.log(error);
                }
            });
        }

        function teatAPIAppClientActivate(AppID, CorpoKey, Email, AppKey) {
            var data = { // Set Paramiter Request : คือการส่งค่าผ่าน URl
                ApplicationID: AppID,
                CorporatePermissionKey: CorpoKey,
                ApplicationPermissionKey: AppKey,
                Email: Email
            }

            $.ajax({
                type: 'POST', // Request Type เป็น POST หรือ GET
                url: "/API/AppClient/AppClientActivation.aspx", // กำหนดที่อยู่ API
                data: data, // ส่ง Request ที่กำหนดใว้
                success: function (data) {
                    console.log(data);
                    var result = JSON.parse(data); // เอาผลลัพที่ได้มาแปลงเป็น Json Object แล้วเอาไปใช้ทำ Process ได้เลยครับ
                    // data :: คือตัวแปลที่ Return มาจาก API

                    console.log(result);
                },
                error: function (error) {
                    // error :: คือ Object Error :: มีอะไรเยอะแยะไปหมด :: ** ไม่ต้องสนใจก็ได้ถ้า Function ใน API ถูก
                    console.log(error);
                }
            });
        }
    </script>

    <div>App Id <input type="text" name="name" value="AppID-01" id="txtAppID" /></div>
    <br />
    <div>Corpo Key <input type="text" name="name" value="FEF35089-3D87-4633-A505-F6CA39" id="txtCorpoKey" /></div>
    <br />
    <div>Email <input type="text" name="name" value="kolabanlong@gmail.com" id="txtEmail" /></div>
    <br />
    <div>App Key <input type="text" name="name" value="" id="txtAppKey" /></div>
    <br />
    <br />
    <button type="button" onclick="teatAPIAppClientRequest($('#txtAppID').val(), $('#txtCorpoKey').val(), $('#txtEmail').val());">
        Test API Request
    </button>
    &nbsp;
    &nbsp;
    &nbsp;
    &nbsp;
    <button type="button" onclick="teatAPIAppClientActivate($('#txtAppID').val(), $('#txtCorpoKey').val(), $('#txtEmail').val(), $('#txtAppKey').val());">
        Test API Activate
    </button>
    &nbsp;
    &nbsp;
    &nbsp;
    &nbsp;
    <br />
    <hr />
    <br />
    Seq
    <asp:TextBox runat="server" ID="txtSEQ" />
    <br />
    <br />
    <asp:Button Text="Test Approve" runat="server" ID="Button1" OnClick="Button1_Click" />
    &nbsp;
    &nbsp;
    &nbsp;
    &nbsp;
    <asp:Button Text="Test Reject" runat="server" ID="Button2" OnClick="Button2_Click" />
    <br />
    <hr />
    <br />
    <asp:Button Text="text" runat="server" ID="btnTestrecurring" OnClick="btnTestrecurring_Click" />
    <%--<div class="form-row">
        <div class="col-6">
            <asp:TextBox runat="server" Text="Text" CssClass="form-control" 
                ID="txtText1" />
        </div>
        <div class="col-6">
            <asp:DropDownList runat="server" CssClass="form-control" ID="ddlType" >
                <asp:ListItem Text="text 1" Value="val1" />
                <asp:ListItem Text="text 2" />
                <asp:ListItem Text="text 3" />
            </asp:DropDownList>
        </div>
    </div>
    <br />
    <div class="form-row">
        <div class="col-12">
            <asp:Button Text="text" runat="server" CssClass="btn btn-primary"
                OnClick="btnSubmit_Click" ID="btnSubmit" ClientIDMode="Static" />
            <asp:Button Text="Search" runat="server" ID="btnSearch" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
        </div>
    </div>
    <div class="form-row">
        <div class="col-12">
            <table class="table table-bordered">
                <asp:Repeater runat="server" ID="rptCusDetail">
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("CustomerCode") %></td>
                            <td><%# Eval("CustomerName") %></td>
                            <td><%# Eval("CustomerType") %></td>
                            <td><%# Eval("SaleArea") %></td>
                            <td><%# Eval("Province") %></td>
                            <td><%# Eval("Tel") %></td>
                            <td><%# Eval("Email") %></td>
                            <td><%# Eval("ContactType") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>--%>
    <%--<div>
        <script>
            function myfunction() {
                $("").val();
            }
        </script>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:SearchHelpCIControl runat="server" id="SearchHelpCIControl" />
            </ContentTemplate>        
        </asp:UpdatePanel>
    </div>--%>

    <%--<div>
        <uc1:SearchCustomerControl runat="server" id="SearchCustomerControl" />
    </div>--%>

    <%--<div>
        <input type="file" name="txtfile" id="txtfile" value="" multiple="multiple" />
    </div>
    <div>
        <button type="button" onclick="myTest()" >Save</button>
    </div>
    <script>
        function myTest() {
            var files = document.getElementById('txtfile').files; //Files[0] = 1st file
            var data = new FormData();
            if (files.length > 0) {
                for (var i = 0; i < files.length; i++) {
                    data.append('UploadedFiles', files[i], files[i].name);
                }
            }
            data.append("uploadType", "FILE");
            data.append("message", "message");
            data.append("aobj", "zaantest");

            $.ajax({
                type: 'POST',
                url: servictWebDomainName + "widget/AJAXFileUploadAPI.aspx",
                data: data,
                cache: false,
                contentType: false,
                processData: false,
                success: function (data) {
                    console.log("success \n" + JSON.stringify(data));
                },
                error: function (error) {
                    console.log("Error \n" + JSON.stringify(error));
                }
            });
        }
    </script>--%>

<%--<link href="-/AGFramework/chat/Activity-redesign-1.2.css" rel="stylesheet" />
    <link href="-/AGFramework/chat/Activity-Chatting-1.7.css" rel="stylesheet" />

    <script src="-/AGFramework/chat/Activity-redesign.js"></script>
    <script src="-/AGFramework/chat/Activity-Chatting.js"></script>

    <div>
        <button type="button" onclick="sendCustomEmail('subject', 'myName')">email</button>
    </div>
    <uc1:ActivitySendMailModal runat="server" id="ActivitySendMailModal" />

    
    <div>
        <button type="button" onclick="myTest()">Test API Mail</button>
    </div>
    <script>
        function myTest() {
            var data = {
                Channel: "1",
                PermissionKey: "EACDD371-E833-45D8-8D8E-D75FF1",
                JsonData: '[{"EmailFrom":"kolabanlong@gmail.com","EmailSubject":"Test+New+API","EmailBody":"Test+Na+JaNote+%3a+This+message+is+from+the+sender+who+use+new+communication+platform+Named+%e2%80%9cLINK%e2%80%9d+message+ID+%3a+%5bI-0000000125%5d%c2%a0If+you+remove+this+content+the+message+will+not+communicate+with+the+system.%c2%a0%c2%a0%0d%0a","MessageID":"I-0000000125"}]'
            }

            $.ajax({
                type: 'POST',
                url: servictWebDomainName + "api/ReplyEmailToPostCommentAPI.aspx",
                data: data,
                success: function (data) {
                    console.log("success \n" + JSON.stringify(data));
                },
                error: function (error) {
                    console.log("Error \n" + JSON.stringify(error));
                }
            });
        }
    </script>--%>
</asp:Content>
