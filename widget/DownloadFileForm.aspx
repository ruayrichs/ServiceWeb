<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DownloadFileForm.aspx.cs" Inherits="ServiceWeb.widget.DownloadFileForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="<%= Page.ResolveUrl("~/bootstrap/dist/css/bootstrap.min.css") %>" rel="stylesheet" />
    <script src="<%= Page.ResolveUrl("~/vendor/jquery/jquery.min.js") %>" type="text/javascript"></script>
    <style>
        body {
            background: #eee;
        }

        .file-download {
            width: 80%;
            margin: 100px auto;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.4);
            background: #fff;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="file-download">
            <h4>
                <img src="<%= Page.ResolveUrl("~/images/f1-logo.png") %>" alt="Link logo" style="height: 45px; margin-top: -10px;" />
                <asp:Label ID="lblFilename" Text="File download" runat="server" />                
            </h4>
            <hr style="margin: 10px 0;" />
            <div runat="server" id="divDownload">
                การดาวน์โหลดไฟล์จะเริ่มต้นขึ้นใน 1-2 วินาที หากการดาวน์โหลดไฟล์ไม่ปรากฏ คุณสามารถเลือกดาวน์โหลดด้วยตนเองโดยคลิกข้อความด้านล่างนี้
                <br />
                <asp:LinkButton Text="คลิกที่นี่เพื่อดาวน์โหลด" ID="btnDownload" ClientIDMode="Static" runat="server" OnClick="btnDownload_Click" />
                <hr style="margin: 10px 0;" />

            </div>
            <div runat="server" id="divDel" style="display: none;">
                การดาวน์โหลดไฟล์จะเริ่มต้นขึ้นใน 1-2 วินาที 
            </div>
            <div runat="server" id="divEmpty" style="color: red">
                ไม่พบไฟล์ที่จะดาวน์โหลด
            </div>
        </div>
        <div class="hide">
            <asp:TextBox runat="server" ID="txtFileName" />
            <asp:TextBox runat="server" ID="txtFilePath" />
        </div>
        <script>
            $(document).ready(function () {
                $("#btnDownload")[0].click();
            });
        </script>
    </form>
</body>
</html>
