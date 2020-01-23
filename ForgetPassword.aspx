<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgetPassword.aspx.cs" Inherits="ServiceWeb.ForgetPassword" %>

<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FocusoneLink - Powered by A-Gape Consulting Thailand</title>
    <link rel="stylesheet" type="text/css" href="/css/default_app.css" />
    <link href="/bootstrap/docs/assets/css/docs.min.css" rel="stylesheet" type="text/css" />
    <link href="/bootstrap/docs/assets/css/docs.css" rel="stylesheet" type="text/css" />
    <link href="/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="/js/kendoui-extended-api-master/styles/kendo.ext.css" rel="stylesheet">
    <link href="<%=Page.ResolveUrl("~/Styles/Kendo/examples-offline.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/Styles/Kendo/kendo.common.min.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/Styles/Kendo/kendo.silver.min.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/Styles/Kendo/kendo.rtl.min.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/Styles/Kendo/kendo.default.min.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/Styles/Kendo/kendo.common-bootstrap.min.css")%>"
        rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/Styles/Kendo/kendo.bootstrap.min.css")%>" rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/js/kendoui-extended-api-master/styles/kendo.ext.css")%>"
        rel="stylesheet" />
    <link href="<%=Page.ResolveUrl("~/Styles/Kendo/kendo.default.min.css")%>" rel="stylesheet" />
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/js/kendo/jquery.min.js") %>"></script>
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/js/kendo/kendo.web.min.js") %>"></script>
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/js/kendo/kendo.validator.min.js") %>"></script>
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/js/kendo/kendo_console.js") %>"></script>
    <script type="text/javascript" src="<%=Page.ResolveUrl("~/js/kendoui-extended-api-master/js/kendo.web.ext.js") %>"></script>
     <!-- Sweet Alert -->
    <link href="/vendor/sweetalert/dist/sweetalert.css" rel="stylesheet" type="text/css" />
    <script src="/vendor/sweetalert/dist/sweetalert.min.js" type="text/javascript"></script>
    <!--  -->
    <script src="/AGFramework/ag-js.js?version=20181223" type="text/javascript"></script>
    <link href="/AGFramework/ag-css.css?vs=20180626" rel="stylesheet" type="text/css">
</head>
<body>
    <script type="text/javascript">


        function validateEmail(email) {
            var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        }
        function alertMessage(message) {
            kendo.ui.ExtAlertDialog.show({ title: "ข้อความ!", message: message + "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", width: "auto", height: "auto" });
        }
        function alertMessageRedirect(message, url) {
            kendo.ui.ExtAlertDialog.show({
                title: "ข้อความ!",
                message: message + "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",
                width: "auto",
                height: "auto",
                buttons: [{
                    name: "รับทราบ",
                    click: function () {
                        window.location.href = url;
                    }
                }]
            });
        }
    </script>
    <style>
        .float-right
        {
            float: right;
        }
        .table-padding2 tr td
        {
            padding: 2px;
        }
        body{
            background-image: url(/images/writing.jpg) !important;
            background-position: top center;
            -webkit-background-size: cover;
            -moz-background-size: cover;
            background-size: cover;
            -o-background-size: cover;
            background-attachment: fixed;
        }
        .form-control {
            background: rgba(255, 255, 255, 0.6);
            margin-bottom: 8px;
        }
        .middle {
            position: relative;
            display: table;
            width: 100%;
            height: 100%;
        }
        .middle-cell {
            display: table-cell;
            vertical-align: middle;
            height: 100vh;
            width: 100vw;
        }
        .require-field-alert {
            border-color: #ffb4b4;
            background-color: rgba(255, 231, 231, 0.6);
        }
        #recaptcha_widget_div{
            background-color: rgba(255, 255, 255, 0.6);
            border-radius: 3px;
        }
        #recaptcha_area{
            margin: 0 auto;
        }
    </style>


    <form runat="server">
        

        <div class="container middle">
            <div class="middle-cell">
                <div class="row">
                    <div class="col-md-4 col-md-offset-4">
                        <h2 style="color: #fff;font-weight: 900;margin-top: 0px;">
                            Reset Password
                        </h2>
                        <asp:TextBox ID="_tb_Mail" runat="server" onchange="return validateEmail(this.value)"
                            ClientIDMode="Static" CssClass="form-control" placeholder="Email"></asp:TextBox>
                        <div style="margin-bottom: 8px;display:none;">
                            <recaptcha:RecaptchaControl ID="recaptcha" runat="server" PublicKey="6Leq5ZIUAAAAAIYaj7MiC-Uou-AnQxnE59vjGEFs"
                                PrivateKey="6Leq5ZIUAAAAAHKtavbQcmWV5woX5ObQV0Uukvo3" />
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

                        <asp:Button ID="_btnReset" runat="server" Text="ยืนยันการแก้ไขรหัสสมาชิกผ่านอีเมล์" ClientIDMode="Static"
                            style="width: 100%;box-shadow: 0 2px 5px 0 rgba(0, 0, 0, 0.16), 0 2px 10px 0 rgba(0, 0, 0, 0.12);"
                            OnClick="_btnReset_Click" CssClass="btn btn-warning float-right" OnClientClick="return validateReset();" /> <%----%>
                    </div>
                </div>
            </div>
        </div>

    <div style="width: 100%; text-align: center;display:none;">
        <div class="panel panel-primary" style="width: 500px; margin: 0 auto; text-align: left;">
            <div class="panel-heading">
                <h3 class="panel-title">
                    Reset Password !!!</h3>
            </div>
            <div class="panel-body">
                <table class="form">
                    <tr>
                        <td style="width: 120px;">
                            <label for="input">
                                รหัสการใช้งาน<span class="required">*</span></label>
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="input">
                                อีเมลล์<span class="required">*</span></label>
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                            
                        </td>
                    </tr>
                </table>
                <div class="line margin-top10">
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

