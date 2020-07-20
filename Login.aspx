<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ServiceWeb.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!-- Bootstrap Core CSS -->
    <link href="<%= Page.ResolveUrl("~/css/bootstrap.min.css") %>" rel="stylesheet" />

    <!-- Theme CSS -->
    <link href="<%= Page.ResolveUrl("~/css/clean-blog.min.css?vs=20180124") %>" rel="stylesheet">

    <!-- Framework Master Style -->
    <link href="<%= Page.ResolveUrl("~/css/master-style.css?vs=20180124") %>" rel="stylesheet" />

    <!-- jQuery -->
    <script src="<%= Page.ResolveUrl("~/js/jquery.min.js?vs=20190113") %>"></script>
    
    <!-- Bootstrap Core JavaScript -->
    <script src="<%= Page.ResolveUrl("~/js/bootstrap.min.js") %>"></script>

    <!-- Sweet Alert -->
    <link href="<%= Page.ResolveUrl("~/Framework/sweetalert/dist/sweetalert.css") %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Framework/sweetalert/dist/sweetalert.min.js?vs=20190113") %>"></script>

    <script src="<%= Page.ResolveUrl("~/js/agape-javascript-framework.js?vs=20190113") %>"></script>

    <script> var servictWebDomainName = '<%= Page.ResolveUrl("~") %>'; </script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <style>
                .form-control {
                    background: rgba(255, 255, 255, 0.8);
                }

                .full-screen {
                    background-image: url('<%= Page.ResolveUrl("~") %>images/login-image.jpg');
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
                        background: rgba(0, 0, 0, 0.6);
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
                     .radiobutton {
                    padding-bottom: 10px;
                }
            </style>

            
            <asp:ScriptManager ID="_ScriptManager" runat="server" ScriptMode="Release" AsyncPostBackTimeout="360000">
            </asp:ScriptManager>

            <div class="full-screen">
                <div class="middle">
                    <div class="middle-cell">
                        <div class="container">
                            <div class="row">
                                <div class="col-md-6 visible-md visible-lg">
                                    <div class="logo">
                                        <h2>Services Management
                                        </h2>
                                        <p>
                                            <b></b>
                                        </p>
                                        <p>
                                        </p>
                                        <p>
                                            Powered by : Link Team - A-Gape Consulting (Thailand) Co.,Ltd.
                                        <a class="text-warning" href="http://www.focusonesoftware.com/">http://www.focusonesoftware.com/</a>
                                        </p>
                                    </div>
                                </div>
                                <div class="col-md-5 col-md-offset-1">
                                    <div class="sign-in">
                                        <asp:Panel runat="server" DefaultButton="btnLogin">
                                            <h2>
                                                <asp:Label ID="lbText" runat="server" Text="Sign In" Style="color: #fff;"></asp:Label>
                                            </h2>

                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control input-lg require" placeholder="Username or E-mail"></asp:TextBox>
                                            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" CssClass="form-control input-lg require" placeholder="Password"></asp:TextBox>
                                            <%--=======================select mode login===============================--%>
                                            <div >
                                                <div  >
                                                    <h5>
                                                    <asp:Label ID="Labelmode" runat="server" Text="Login Mode" Style="color: #fff;"></asp:Label>
                                                    </h5>
                                                </div>
                                                <div  class="radiobutton">
                                                    <asp:RadioButtonList id="checkmodelogin" runat="server" RepeatDirection="Vertical"  Width="100%">
                                                        <asp:ListItem value="1" Selected="True"> Login Local </asp:ListItem>
                                                        <asp:ListItem value="2"> Intregrate Active Directory</asp:ListItem>
                                                        <asp:ListItem value="3"> Login with ONE ID</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>
                                            <%--========================================================================--%>
                                            <button id="Button1" type="button" class="btn btn-primary btn-lg" onclick="$(this).next().click();" runat="server"
                                                style="width: 100%;">
                                                <i class="fa fa-sign-in" style="font-size: 18px"></i>&nbsp;Sign In
                                            </button>
                                            <asp:Button ID="btnLogin" OnClientClick="AGLoading(true);" ClientIDMode="Static" OnClick="btnLogin_Click" runat="server" CssClass="hide" />
                                            <br />
                                            <div style="margin-top: 10px;">
                                                <!--
                                                <a href="#">
                                                    <asp:Label runat="server" ID="labelSignUpHere" Text="สมัครเป็นสมาชิก"></asp:Label>
                                                </a>
                                                |-->
                                                <strong>
                                                    <a href="ForgetPassword.aspx">
                                                        <asp:Label runat="server" ID="labelForget" Text="Forgot Password"></asp:Label>
                                                    </a>
                                                    <% if (IsFilterOwner) { %>
                                                    |
                                                    <a href="SignUp.aspx">
                                                        <asp:Label runat="server" ID="labelBO" Text="Business Sign Up"></asp:Label>
                                                    </a>
                                                     <% } %>
                                                </strong>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            <!-- Modal Change Password -->
            <div id="modal-change-password" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
                <div class="modal-dialog modal-sm" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close text-danger" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title">Change Password</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="form-group col-lg-12">
                                    <label>รหัสผ่านใหม่</label>
                                    <asp:TextBox ID="tbNewPassword" runat="server" ClientIDMode="Static" CssClass="form-control" TextMode="Password" autocomplete="new-password"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-12">
                                    <label>ยืนยันรหัสผ่าน</label>
                                    <asp:TextBox ID="tbConfirmPassword" runat="server" ClientIDMode="Static" CssClass="form-control" TextMode="Password" autocomplete="new-password"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                    <button type="button" class="btn btn-success" onclick="confirmChangePassword();">Confirm</button>
                                    <asp:Button ID="btnChangePassword" runat="server" ClientIDMode="Static" CssClass="hidden" OnClick="btnChangePassword_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>

            <script>
                function PostValidator(sender) {
                    if (AGValidator(sender, $(".middle-cell"))) {
                        AGLoading(true);
                        return true;
                    }
                    return false;
                }

                function goToEdit(url) {
                    window.open(url, '_blank', 'location=yes,height=840,width=1100,scrollbars=yes,status=yes');
                }

                $(document).ready(function () {
                    $("#tbNewPassword, #tbConfirmPassword").keyup(function (event) {
                        if (event.keyCode === 13) {
                            // Cancel the default action, if needed
                            event.preventDefault();
                            // Trigger the button element with a click
                            confirmChangePassword();
                        }
                    });
                });

                function confirmChangePassword() {
                    AGLoading(true);
                    $("#btnChangePassword").click();
                }
            </script>
        </div>
    </form>
</body>
</html>
