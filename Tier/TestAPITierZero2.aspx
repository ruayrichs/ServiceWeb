<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestAPITierZero2.aspx.cs" Inherits="ServiceWeb.Tier.TestAPITierZero2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://code.jquery.com/jquery-1.9.1.min.js"></script>
    <script>
        $(document).ready(function () {
            //saveTierZerItem('2', 'kolabanlong@gmail.com', '', '', '01234567892', 'Subject 1', 'Detail 1', '', '', '');
            //testAIPTypeEmail();
        });

        function callAPITestTypeWeb() {
            saveTierZerItem(
                '2',
                $("#txtEmail").val(),
                '',
                '',
                '',
                $("#txtSubject").val(),
                '',
                '',
                '',
                '',
                $("#txtPermissionKey").val()
            );
        }
        
        function callAPITestTypeEmail() {
            var postData = {
                Channel: "1",
                PermissionKey: $("#txtPermissionKey").val(),
                JsonData: '[{EmailFrom: "'+$("#txtEmail").val()+'",EmailSubject: "'+$("#txtSubject").val()+'",EmailBody: ""}]'
            };
            $.ajax({
                type: "POST",
                url: "/" + "API/TierZeroStructureAPI.aspx",
                data: postData,
                success: function (data) {
                    console.log(data);
                }
            });
        }

        function saveTierZerItem(Channel, EMail, CustomerCode, CustomerName, TelNo, Subject, Detail, Status, TicketNumber, TicketType, PermissionKey) {

            var postData = {
                Channel: Channel,
                EMail: EMail,
                CustomerCode: CustomerCode,
                CustomerName: CustomerName,
                TelNo: TelNo,
                Subject: Subject,
                Detail: Detail,
                Status: Status,
                TicketNumber: TicketNumber,
                TicketType: TicketType,
                PermissionKey: PermissionKey
            };
            $.ajax({
                type: "POST",
                url: "/" + "API/TierZeroStructureAPI.aspx",
                data: postData,
                success: function (data) {
                    console.log(data);
                }
            });
        }

        function testAIPTypeEmailOpen() {
            var postData = {
                Channel: "1",
                PermissionKey: "EACDD371-E833-45D8-8D8E-D75FF1",
                JsonData: '[{EmailFrom: "xxx@mail.com",EmailSubject: "HWP001384|Down",EmailBody: "xxxx"}]'
            };
            $.ajax({
                type: "POST",
                url: "/" + "API/TierZeroStructureAPI.aspx",
                data: postData,
                success: function (data) {
                    console.log(data);
                }
            });
        }

        function testAIPTypeEmailClose() {
            var postData = {
                Channel: "1",
                PermissionKey: "EACDD371-E833-45D8-8D8E-D75FF1",
                JsonData: '[{EmailFrom: "xxx@mail.com",EmailSubject: "HWP001384|Up",EmailBody: "xxxx"}]'
            };
            $.ajax({
                type: "POST",
                url: "/" + "API/TierZeroStructureAPI.aspx",
                data: postData,
                success: function (data) {
                    console.log(data);
                }
            });
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%--<div>
            Channel
            <input type="text" name="name" value="1" id="txtChannel" />
        </div>--%>
        <div>
            PermissionKey
            <input type="text" name="name" value="" id="txtPermissionKey" />
        </div>
        <div>
            Subject
            <input type="text" name="name" value="" id="txtSubject" />
        </div>
        <div>
            Email
            <input type="text" name="name" value="" id="txtEmail" />
        </div>
        
        <div>
            <button type="button" onclick="callAPITestTypeWeb();">Web</button>
            &nbsp;
            &nbsp;
            &nbsp;
            &nbsp;
            <button type="button" onclick="callAPITestTypeEmail();">Email</button>
        </div>

        <div>
            <button type="button" onclick="testAIPTypeEmailOpen();">Open</button>
            &nbsp;
            &nbsp;
            &nbsp;
            &nbsp;
            <button type="button" onclick="testAIPTypeEmailClose();">Close</button>
        </div>
    </div>
    </form>
    
</body>
</html>
