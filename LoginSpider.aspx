<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginSpider.aspx.cs" Inherits="ServiceWeb.LoginSpider" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!-- Bootstrap Core CSS -->
    <link href="/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Theme CSS -->
    <link href="/css/clean-blog.min.css?vs=20180124" rel="stylesheet">

    <!-- Framework Master Style -->
    <link href="/css/master-style.css?vs=20180124" rel="stylesheet" />

    <!-- jQuery -->
    <script src="/js/jquery.min.js"></script>

    <!-- Sweet Alert -->
    <link href="/Framework/sweetalert/dist/sweetalert.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/Framework/sweetalert/dist/sweetalert.min.js"></script>

    <script src="/js/agape-javascript-framework.js?version=1.2"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="_ScriptManager" ScriptMode="Release" AsyncPostBackTimeout="360000">
            </asp:ScriptManager>
        <div class="container">
            <style>
                .form-control {
                    background: rgba(255, 255, 255, 0.8);
                }

                .full-screen {
                    background-image: url('/images/login-image.jpg');
                    background-position: top center;
                    -webkit-background-size: cover;
                    -moz-background-size: cover;
                    background-size: cover;
                    -o-background-size: cover;
                    background-attachment: fixed;
                    background-repeat: no-repeat;
                    position: fixed;
                    left: 0;
                    right: 0;
                    top: 0;
                    bottom: 0;
                }

                .middle {
                    position: relative;
                    display: table;
                    width: 100%;
                    height: 100%;
                    line-height: 100%;
                }

                .middle-cell {
                    display: table-cell;
                    vertical-align: middle;
                    height: 100%;
                    width: 100%;
                }

                .sign-in {
                    background: rgba(0, 0, 0, 0.3);
                    padding: 2px 8px;
                    color: #fff;
                }

                    .sign-in a {
                        color: #fff;
                    }

                @media(min-width:768px) {
                    .sign-in {
                        background: rgba(0, 0, 0, 0.3);
                        padding: 20px 30px;
                        border-radius: 5px;
                    }
                }

                .sign-in .form-control,
                .sign-in h4 {
                    margin-bottom: 15px;
                }

                .sign-in h2 {
                    margin-top: 0;
                }

                .logo {
                    color: #fff;
                    text-shadow: 2px 1px 2px #000000;
                }

                    .logo h2 {
                        margin-top: 0;
                        font-weight: 900;
                    }

                    .logo h3 {
                        margin: 10px 0;
                    }
            </style>


            <div class="full-screen">
                <div class="middle">
                    <div class="middle-cell">
                        <div class="container">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="logo">
                                        <h2>
                                           
                                             Service Desk Management
                                        </h2>
                                        <p>
                                            <b>
                                            </b>
                                        </p>
                                        <p>
                                          
                                        </p>
                                        <p>
                                            Powered by : Link Team - A-Gape Consulting (Thailand) Co.,Ltd.
                                        <a class="text-warning" href="http://www.focusonesoftware.com/">http://www.focusonesoftware.com/</a>
                                        </p>

                                        <div style="display:none">
                                            <asp:UpdatePanel ID="udpn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
 <asp:Button Text="text" ID="btnLoginSpider" ClientIDMode="Static" OnClick="btnLoginSpider_Click" runat="server" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                           
                                        </div>
                                        <script>
                                            $(document).ready(function () {
                                                $("#btnLoginSpider").click();
                                            });
                                        </script>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
