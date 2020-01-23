<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="ServiceWeb.SignUp" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/css/bootstrap.min.css" integrity="sha384-GJzZqFGwb1QTTN6wy59ffF1BuGJpLSa9DkKMp0DgiMDm4iYMj70gZWKYbI706tWS" crossorigin="anonymous">
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.6/umd/popper.min.js" integrity="sha384-wHAiFfRlMFy6i5SRaxvfOCifBUQy1xHdJ/yoi7FRNXMRBu5WHdZYu1hA6ZOblgut" crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/js/bootstrap.min.js" integrity="sha384-B0UglyR+jN6CkvvICOB2joaf5I4l3gm9GU6Hc1og6Ls7i6U/mkkaduKaBhlAXv9k" crossorigin="anonymous"></script>
    <!-- Sweet Alert -->
    <link href="<%= Page.ResolveUrl("~/Framework/sweetalert/dist/sweetalert.css") %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="<%= Page.ResolveUrl("~/Framework/sweetalert/dist/sweetalert.min.js?vs=20190113") %>"></script>

    <script src="<%= Page.ResolveUrl("~/js/agape-javascript-framework.js?vs=20190113") %>"></script>

    <script> var servictWebDomainName = '<%= Page.ResolveUrl("~") %>'; </script>
    <script>
        function afterCreate() {
            document.getElementById('<%=fname_inp.ClientID %>').value = '';
            document.getElementById('<%=lname_inp.ClientID %>').value = '';
            document.getElementById('<%=BusinessOwner_inp.ClientID %>').value = '';
            document.getElementById('<%=uname_inp.ClientID %>').value = '';
            document.getElementById('<%=email_inp.ClientID %>').value = '';
            document.getElementById('<%=phone_inp.ClientID %>').value = '';
        };
    </script>
    <style>
        .content {
            position: absolute;
            left: 50%;
            top: 50%;
            -webkit-transform: translate(-50%, -50%);
            transform: translate(-50%, -50%);
        }

        .full-screen {
            background-image: url('<%= Page.ResolveUrl("~") %>images/writing.jpg');
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
    </style>
</head>
<body>
    <div class="full-screen">
        <form id="form1" runat="server">
            <div>
                <asp:ScriptManager ID="_ScriptManager" runat="server" ScriptMode="Release" AsyncPostBackTimeout="360000"></asp:ScriptManager>
            </div>
            <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <style>
                        .demo {
                            -webkit-box-shadow: 0px 0px 0px 5px #A0A0A0, inset 0px 10px 27px -8px #141414, inset 0px -10px 27px -8px #A31925, 5px 5px 40px 2px rgba(0,0,0,0.6); 
                            box-shadow: 0px 0px 0px 5px #A0A0A0, inset 0px 10px 27px -8px #141414, inset 0px -10px 27px -8px #A31925, 5px 5px 40px 2px rgba(0,0,0,0.6);
                            }
                    </style>
                    <div class="col-sm-4 content">
                        <div style="text-align: center;color: #fff;margin-bottom: 25px;">
                            <h2>Business Sign Up</h2>
                        </div>
                        <div class="card">
                            <div class="card-body">
                                <div class="form-row">
                                    <div class="col-6">
                                        <asp:TextBox runat="server" ID="fname_inp" placeholder="First Name" CssClass="form-control form-control" ClientIDMode="Static" required="true" /><br />
                                    </div>
                                    <div class="col-6">
                                        <asp:TextBox runat="server" ID="lname_inp" placeholder="Last Name" CssClass="form-control form-control" ClientIDMode="Static" required="true" /><br />
                                    </div>
                                </div>
                        
                                <asp:TextBox runat="server" ID="BusinessOwner_inp" placeholder="Business Owner" CssClass="form-control form-control" ClientIDMode="Static" required="true" /><br />
                                <asp:TextBox runat="server" ID="uname_inp" placeholder="Username" CssClass="form-control form-control" ClientIDMode="Static" required="true" /><br />
                                <asp:TextBox runat="server" ID="email_inp" AutoCompleteType="Email" placeholder="Email" CssClass="form-control form-control" 
                                    ClientIDMode="Static" required="true" TextMode="Email" ValidateRequestMode="Enabled" /><br />
                                <asp:TextBox runat="server" ID="phone_inp" placeholder="Phone" CssClass="form-control form-control" ClientIDMode="Static" 
                                    required="true" /><br />
                                <div style="text-align: center">
                                    <!-- Recapcha vv -->
                                    <asp:UpdatePanel ID="udpRechapcha" runat="server">
                                        <ContentTemplate>
                                            <div style="margin-bottom: 8px;display:none;">
                                                <recaptcha:RecaptchaControl ID="recaptcha" runat="server" PublicKey="6Leq5ZIUAAAAAIYaj7MiC-Uou-AnQxnE59vjGEFs" PrivateKey="6Leq5ZIUAAAAAHKtavbQcmWV5woX5ObQV0Uukvo3" />
                                            </div>
                                            <div style="text-align:center;">
                                                <script src='https://www.google.com/recaptcha/api.js'></script>
                                                    <div class="g-recaptcha" data-sitekey="6Leq5ZIUAAAAAIYaj7MiC-Uou-AnQxnE59vjGEFs" style="display: inline-block;"></div>
                                                <noscript>
                                                <div>
                                                    <div style="width: 302px; height: 422px; position: relative;">
                                                        <div style="width: 302px; height: 422px; position: absolute;">
                                                        <iframe src="https://www.google.com/recaptcha/api/fallback?k=6Leq5ZIUAAAAAIYaj7MiC-Uou-AnQxnE59vjGEFs"
                                                                frameborder="0" scrolling="no"
                                                                style="width: 302px; height:422px; border-style: none;">
                                                        </iframe>
                                                        </div>
                                                    </div>
                                                    <div style="width: 300px; height: 60px; border-style: none;
                                                                    bottom: 12px; left: 25px; margin: 0px; padding: 0px; right: 25px;
                                                                    background: #f9f9f9; border: 1px solid #c1c1c1; border-radius: 3px;">
                                                        <textarea id="g-recaptcha-response" name="g-recaptcha-response"
                                                                    class="g-recaptcha-response"
                                                                    style="width: 250px; height: 40px; border: 1px solid #c1c1c1;
                                                                            margin: 10px 25px; padding: 0px; resize: none;" >
                                                        </textarea>
                                                    </div>
                                                </div>
                                                </noscript>

                                                <div id="ReCaptchContainer"></div>  
                                                <label id="lblMessage" runat="server" clientidmode="static"></label>  
                                                <br />  
                                                <%--<button type="button" >Submit</button>--%>  
                                                <%--<script src="https://www.google.com/recaptcha/api.js" async defer></script>--%>   
                                                <%--<script  src="https://code.jquery.com/jquery-3.2.1.min.js"></script>--%>  
 
                                                <script type="text/javascript">
                                                    var your_site_key = '6Leq5ZIUAAAAAIYaj7MiC-Uou-AnQxnE59vjGEFs';
                                                    var renderRecaptcha = function () {
                                                        grecaptcha.render('ReCaptchContainer', {
                                                            'sitekey': your_site_key,
                                                            'callback': reCaptchaCallback,
                                                            theme: 'light', //light or dark    
                                                            type: 'image',// image or audio    
                                                            size: 'normal'//normal or compact    
                                                        });
                                                    };

                                                    var reCaptchaCallback = function (response) {
                                                        if (response !== '') {
                                                            jQuery('#lblMessage').css('color', 'green').html('Success');
                                                        }
                                                    };

                                                    //jQuery('button[type="button"]').click(function (e) {
                                                    //jQuery('button[type="button"]').click(function (e) {

                                                    //});

                                                    function validateReset() {
                                                        if (validateResetEmail()) {
                                                            var message = 'Please checck the checkbox';
                                                            if (typeof (grecaptcha) != 'undefined') {
                                                                var response = grecaptcha.getResponse();
                                                                (response.length === 0) ? (message = 'Captcha verification failed') : (message = 'Success!');
                                                            }

                                                            if (message.toLowerCase() == 'success!') {
                                                                return true;
                                                            } else {
                                                                jQuery('#lblMessage').html(message);
                                                                jQuery('#lblMessage').css('color', (message.toLowerCase() == 'success!') ? "green" : "red");

                                                                alertMessage("Captcha verification failed");
                                                                return false;
                                                            }

                                                            return message.toLowerCase() == 'success!';
                                                        }

                                                        return false;
                                                    }

                                                    function validateResetEmail() {
                                                        var email = $('#_tb_Mail').val();
                                                        if (email == "") {
                                                            alertMessage("กรุณาระบุอีเมล์");
                                                            return false;
                                                        }
                                                        return true;
                                                    }
                                                </script>  
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <!-- Recapcha ^^ -->
                                    <asp:Button runat="server" ID="createUser_btn" OnClick="createUser_btn_Click" Text="Register" CssClass="btn btn-success" />
                                    <a href="Login.aspx" class="btn btn-primary">Login</a>
                                </div>
                            </div>
                        </div>                        
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>
