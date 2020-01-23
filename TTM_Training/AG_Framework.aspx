<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="AG_Framework.aspx.cs" Inherits="ServiceWeb.TTM_Training.AG_Framework" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .tool-circle{
            position:fixed;
            cursor:pointer;
            width:50px;
            height:50px;
            right:10px;
            top:80px;
            background:#009688;
            color:#fff;
            border-radius:50%;
            text-align:center;
            font-weight:600;
            font-size:20px;
            padding-top:11px;
            z-index:100;
        }
        .show-material-box{
            background:#eee;
            padding:10px;
            text-align:center;
        }
        .show-material-box .mat-box{
            margin-bottom:40px;
        }
        .ht{
            
        }
        .ht .btn{
            margin-bottom:4px;
        }
        pre{
            margin-top:15px;
            display:block;
            word-break: normal;
            word-wrap: break-word;
            white-space:pre-wrap;
            color: #c7254e;
            background-color: #f9f2f4;
            border-radius: 4px;
            padding:5px 10px;
            margin:0;
            height:auto;
            min-height:initial;
        }
        .inline-block{
            margin:0;
            display:inline-block;
        }
        .new-line:before{
            content:'\a';
            white-space:pre;
        }
        .new-line:after{
            display:inline-block;
            content:'\a';
        }
        .tab-1:after{
            padding-left:20px;
        }
        .tab-2:after{
            padding-left:40px;
        }
        .tab-3:after{
            padding-left:60px;
        }
        .tab-4:after{
            padding-left:80px;
        }
        .tab-5:after{
            padding-left:100px;
        }
        
        .mat-box {
            border-radius: 5px;
            border: 1px solid;
            border-color: #e5e6e9 #dfe0e4 #d0d1d5;
            background: #fff;
            padding: 15px;
            margin-bottom: 10px;
        }
    </style>

    
    <a href="#js-panel" class="tool-circle z-depth-2">
        JS
    </a>

    <a href="#css-panel" class="tool-circle z-depth-2" style="top:140px">
        CSS
    </a>

    <a href="#asp-panel" class="tool-circle z-depth-2" style="top:200px">
        ASP
    </a>


    <div class="container">

        <div class="mat-box ht" id="js-panel">
            <h3 style="margin-top:0;">
                JS
            </h3>
            <a href="#smartpopup" class="btn btn-primary">smart-popup</a>
            <a href="#loading" class="btn btn-primary">loading</a>
            <a href="#loading-message" class="btn btn-primary">loading-message</a>
            <a href="#message-dialog" class="btn btn-primary">message-dialog</a>
            <a href="#success-dialog" class="btn btn-primary">success-dialog</a>
            <a href="#info-dialog" class="btn btn-primary">info-dialog</a>
            <a href="#error-dialog" class="btn btn-primary">error-dialog</a>
            <a href="#confirm-dialog" class="btn btn-primary">confirm-dialog</a>
            <%--<a href="#validator" class="btn btn-primary">validator</a>--%>
            <%--<a href="#activity-remark" class="btn btn-primary">activity-remark</a>--%>
        </div>

        <div class="form-row" style="margin-bottom:0;">
            <div class="col-md-12">
                 <div class="mat-box">
                    <h3 style="margin-top:0;">
                        <a id="smartpopup" href="javascript:;">#smart-popup</a><br />
                    </h3>

                    <style>
                        .smart-popup{
                            display:inline-block;
                            position:relative;
                        }
                        .smart-popup .smart-popup-icon{
                            font-size:20px;
                            color:#777;
                            display:inline;
                            cursor:pointer;
                        }
                        .smart-popup .smart-popup-dialog{
                            display:none;
                            position:absolute;
                            top:100%;
                            padding:15px;
                            background:#FAFAFA;
                            z-index:10000;
                            box-shadow: 0 12px 15px 0 rgba(0, 0, 0, 0.24), 0 17px 50px 0 rgba(0, 0, 0, 0.19);
                        }
                    </style>

                    <div class="smart-popup" data-side="right">
                        <div class="smart-popup-icon">
                            <i class="fa fa-home"></i>
                        </div>
                        <div class="smart-popup-dialog">
                            <div style="height:100px;width:100px;"></div>
                        </div>
                    </div>
                    <div class="smart-popup" data-side="right">
                        <div class="smart-popup-icon">
                            <i class="fa fa-cog"></i>
                        </div>
                        <div class="smart-popup-dialog">
                            <div style="height:400px;width:400px;"></div>
                        </div>
                    </div>

                     <script>
                         $.fn.smartPopUp = function () {
                             var elts = $(this);
                             elts.each(function () {
                                 var elt = $(this);
                                 var side = elt.attr("data-side");
                                 side = side == undefined ? "right" : side;
                                 var btn = elt.find(".smart-popup-icon");
                                 var dialog = elt.find(".smart-popup-dialog");
                                 dialog.show();
                                 elt.attr("data-width", (dialog.width() + 30));
                                 elt.attr("data-height", (dialog.height() + 30));
                                 dialog.hide();

                                 if (side == "left") {
                                     dialog.css({
                                         left: "auto",
                                         right: "100%"
                                     });
                                 } else {
                                     dialog.css({
                                         left: "100%",
                                         right: "auto"
                                     });
                                 }

                                 dialog.click(function (e) {
                                     e.stopPropagation();
                                 });

                                 btn.click(function (e) {
                                     $(".smart-popup .smart-popup-dialog").hide();
                                     var dialog = $(this).closest(".smart-popup").find(".smart-popup-dialog");
                                     dialog.css({
                                         width: 0,
                                         height: 0
                                     }).show().animate({
                                         width: dialog.closest(".smart-popup").attr("data-width"),
                                         height: dialog.closest(".smart-popup").attr("data-height")
                                     }, 200);
                                     e.stopPropagation()
                                 });
                             });

                             $(document).click(function () {
                                 $(".smart-popup .smart-popup-dialog").animate({
                                     width: 0,
                                     height: 0
                                 }, 100, function () {
                                     $(".smart-popup .smart-popup-dialog").hide();
                                 });
                             });
                         };
                         $(".smart-popup").smartPopUp();
                     </script>
                </div>
            </div>
        </div>

        <div class="form-row" style="margin-bottom:0;">
            <div class="col-md-6">
                 <div class="mat-box">
                    <h3 style="margin-top:0;">
                        <a id="loading" href="javascript:;">#loading</a><br />
                    </h3>
                    <span class="btn btn-success btn-sm" onclick="AGLoading(true);">Try now</span>
                    <br /><br />
                    javascript : 
                    <pre>AGLoading(true);</pre>
                    <br />
                    C# : 
                    <pre>ClientService.AGLoading(true);</pre>

                </div>
            </div>
            <div class="col-md-6">
                <div class="mat-box">

                    <h3 style="margin-top:0;">
                        <a id="loading-message" href="javascript:;">#loading-message</a><br />
                    </h3>
                    <span class="btn btn-success btn-sm" onclick="AGLoading(true,'Loading message');">Try now</span>
                    <br /><br />
                    javascript : 
                    <pre>AGLoading(true,"Loading message");</pre>
                    <br />
                    C# : 
                    <pre>ClientService.AGLoading(true,"Loading message");</pre>


                </div>
            </div>
            <div class="col-md-6">
                <div class="mat-box">

                    <h3 style="margin-top:0;">
                        <a id="message-dialog" href="javascript:;">#message-dialog</a><br />
                    </h3>
                    <span class="btn btn-success btn-sm" onclick='AGMessage("Message !!");'>Try now</span>
                    <br /><br />
                    javascript : 
                    <pre>AGMessage("Message !!");</pre>
                    <br />
                    C# : 
                    <pre>ClientService.AGMessage("Message !!");</pre>

                </div>
            </div>
            <div class="col-md-6">
                <div class="mat-box">

                    <h3 style="margin-top:0;">
                        <a id="success-dialog" href="javascript:;">#success-dialog</a><br />
                    </h3>
                    <span class="btn btn-success btn-sm" onclick='AGSuccess("Message !!");'>Try now</span>
                    <br /><br />
                    javascript : 
                    <pre>AGSuccess("Message !!");</pre>
                    <br />
                    C# : 
                    <pre>ClientService.AGSuccess("Message !!");</pre>
        
                </div>
            </div>
            <div class="col-md-6">
                <div class="mat-box">

                    <h3 style="margin-top:0;">
                        <a id="info-dialog" href="javascript:;">#info-dialog</a><br />
                    </h3>
                    <span class="btn btn-success btn-sm" onclick='AGInfo("Message !!");'>Try now</span>
                    <br /><br />
                    javascript : 
                    <pre>AGInfo("Message !!");</pre>
                    <br />
                    C# : 
                    <pre>ClientService.AGInfo("Message !!");</pre>
        
                </div>
            </div>
            <div class="col-md-6">
                <div class="mat-box">

                    <h3 style="margin-top:0;">
                        <a id="error-dialog" href="javascript:;">#error-dialog</a><br />
                    </h3>
                    <span class="btn btn-success btn-sm" onclick='AGError("Message !!");'>Try now</span>
                    <br /><br />
                    javascript : 
                    <pre>AGError("Message !!");</pre>
                    <br />
                    C# : 
                    <pre>ClientService.AGError("Message !!");</pre>
         
                </div>
            </div>
        </div>
       
       <div class="mat-box">

            <h3 style="margin-top:0;">
            <a id="confirm-dialog" href="javascript:;">#confirm-dialog</a><br />
        </h3>
        <span class="btn btn-success btn-sm" onclick='AGConfirm(this,"Message !!");'>Try now</span>
        <br /><br />
        javascript (return true or false) : 
        <pre>AGConfirm(this,"Message !!");</pre> 
        <br />
        ASP Button Example: 
        <pre><span><</span>asp:Button Text="text" OnClientClick="return AGConfirm(this,'Message !!');" runat="server" /></pre>
      
    </div>
        
        
        
        
        
        <%--<div class="mat-box">

            <h3 style="margin-top:0;">
                <a id="validator" href="javascript:;">#validator</a><br />
            </h3>

            <div class="form-row">
                <div class="col-md-7">
                    <label class="text-success">Validate require field</label>
                    <p>
                        - ใส่ class <b>"require"</b> ให้กับ textbox dropdown หรือ input ที่ต้องการ validate บังคับกรอกข้อมูล
                    </p>
                    <label class="text-success">Validate format</label>
                    <p>
                        - ใส่ class <b>"email"</b> ให้กับ textbo ที่ต้องการ validate ว่าเป็น format email
                    </p>
                    <p>
                        - ใส่ class <b>"username"</b> ให้กับ textbox ที่ต้องการ validate ว่าเป็น format username
                    </p>
                    <p>
                        - ใส่ class <b>"password"</b> ให้กับ textbox ที่ต้องการ validate ว่าเป็น format password
                    </p>
            
                    <label class="text-success">Validate password และ verifi password</label>
                    <p>
                        1. ใส่ class <b>"password"</b> ให้กับ textbox ที่ต้องการ validate ว่าเป็น format password
                    </p>
                    <p>
                        2. ใส่ class <b>"verify-password"</b> ให้กับ textbox ตัวที่ 2 ที่ต้องการ validate ว่าเป็น verify password
                    </p>
                
                </div>
                <div class="col-md-4 sign-up">
                    <h2>
                        <span id="lbText">Ex.สมัครเข้าร่วมกับเรา</span>
                    </h2>

                    <input type="text" id="txtUserName" class="form-control input-lg require" placeholder="ชื่อ">

                    <input type="text" id="txtUserSureName" class="form-control input-lg require" placeholder="นามสกุล">

                    <input type="text" id="txtEmail" class="form-control input-lg require email" placeholder="อีเมล">

                    <input id="txtPassWord" class="form-control input-lg require password" type="password" placeholder="รหัสผ่าน">
                    
                    <input id="txtVerifiPassWord" class="form-control input-lg require verify-password" type="password" placeholder="ยืนยันรหัสผ่าน">

                    <input type="submit" value="สมัครสมาชิก" onclick="return PostValidator(this);" id="btnSubmit" class="btn btn-primary btn-lg">

                    <style>
                        .sign-up .form-control{
                            margin-bottom:10px;
                        }
                    </style>

                    <script>
                        function PostValidator(sender) {
                            if (AGValidator(sender, $(".sign-up"))) {
                                agroLoading(true);
                                return true;
                            }
                            return false;
                        }
                    </script>
                </div>
            </div>

            <i><b>method</b> : AGValidator(sender);</i> 
            <b>
                **return boolean
            </b>

<pre>
<span><</span>input type="submit" value="สมัครสมาชิก" <b style="color:red;">onclick="return AGValidator(this);"</b> id="btnSubmit" class="btn btn-primary btn-lg">
</pre>

            <br /> <br />
            <i><b>method : </b> AGValidator(sender,<b style="color:green">parent</b>);</i>
            <b>
                **เรียกใช้แบบกำหนด Container ที่จะให้ Validate
            </b>

<pre>
<span><</span>input type="submit" value="สมัครสมาชิก" <b style="color:red;">onclick="return AGValidator(this,<b style="color:green">$('.container')</b>);"</b> id="btnSubmit" class="btn btn-primary btn-lg">
</pre>


            <h5>Example</h5>
            <pre>
<span><</span>div class="form-row">
    <span><</span>div class="col-md-4 col-md-offset-4 sign-up">
        <span><</span>h2>
            <span><</span>span id="lbText">สมัครเข้าร่วมกับเรา<span><</span>/span>
        <span><</span>/h2>

        <span><</span>input type="text" id="txtUserName" class="form-control input-lg <b>require</b>" placeholder="ชื่อ">

        <span><</span>input type="text" id="txtUserSureName" class="form-control input-lg <b>require</b>" placeholder="นามสกุล">

        <span><</span>input type="text" id="txtEmail" class="form-control input-lg <b>require</b> <b>email</b>" placeholder="อีเมล">

        <span><</span>input id="txtPassWord" class="form-control input-lg <b>require</b> <b>password</b>" type="password" placeholder="รหัสผ่าน">
                    
        <span><</span>input id="txtVerifiPassWord" class="form-control input-lg <b>require</b> <b>verify-password</b>" type="password" placeholder="ยืนยันรหัสผ่าน">

        <span><</span>input type="submit" value="สมัครสมาชิก" onclick="<b>return PostValidator(this);</b>" id="btnSubmit" class="btn btn-primary btn-lg">

    <span><</span>/div>
<span><</span>/div>

<span><</span>script>
    function PostValidator(sender) {
        if (<b>AGValidator(sender, $(".sign-up"))</b>) {
            agroLoading(true);
            return true;
        }
        return false;
    }
<span><</span>/script>
</pre>

        </div>--%>

        <%--<div class="mat-box">
            <h3 style="margin-top:0;">
                <a id="activity-remark" href="javascript:;">#activity-remark</a><br />
            </h3>

            <div class="form-row">
                <div class="col-md-12">

                    <button type="button" class="btn btn-success btn-sm" data-toggle="modal" data-target="#ex-remark">Try now</button>

                        <div id="ex-remark" class="modal fade" role="dialog">
                            <div class="modal-dialog">

                                <!-- Modal content-->
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h4 class="modal-title">Activity remarks</h4>
                                        <button type="button" class="close" data-dismiss="modal">×</button>
                                    </div>
                                    <div class="modal-body">
                                        <div id="ag-remark"></div>
                                        <script>
                                            $("#ag-remark").AGActivityRemark("5700012TH0000001020160701003", "<%= ServiceWeb.Service.PublicAuthentication.FocusOneLinkProfileImage %>");
                                        </script>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                    </div>
                                </div>

                            </div>
                        </div>

                    <br />
                    <br />
                    <i>
                        <b>method</b> :
                        AGActivityRemark(aobjectlink,myImageUrl);
                    </i>
<pre>
<span><<span>script>
    AGActivityRemark(aobjectlink,myImageUrl);
<span><<span>/script>
</pre>

                    <br />

                    <b>Example (Live preview on try now modal)</b>
<pre>
<span><<span>div id="ag-remark"><span><<span>/div>
    <span><<span>script>
        $("#ag-remark").AGActivityRemark("5700012TH0000001020160701003", "/images/user.png");
    <span><<span>/script>
<span><<span>/div>
</pre>

                </div>
            </div>

            
        </div>--%>


        <div class="mat-box ht" id="css-panel">
            <h3 style="margin-top:0;">
                CSS
            </h3>
            <a href="#datetime-picker" class="btn btn-primary">date, datetime-picker</a>
            <%--<a href="#material-box" class="btn btn-primary">material-box</a>--%>
            <a href="#header" class="btn btn-primary">header</a>
            <a href="#button" class="btn btn-primary">button</a>
            <a href="#dropdown" class="btn btn-primary">dropdown</a>
            <a href="#text-color" class="btn btn-primary">text-color</a>
            <a href="#text-align" class="btn btn-primary">text-align</a>
            <a href="#alert" class="btn btn-primary">alert</a>
            <a href="#panel" class="btn btn-primary">panel</a>
            <a href="#modal" class="btn btn-primary">modal</a>
            <a href="#column-responsive" class="btn btn-primary">column-responsive</a>
            <a href="#form-control" class="btn btn-primary">form-control</a>
            <a href="#table" class="btn btn-primary">table</a>
        </div>

        <div class="mat-box">

            <!-- Material Box-->
            <h3 style="margin-top:0;">
                <a id="datetime-pickerx" href="javascript:;">#date, datetime-picker</a><br />
            </h3>

            <div class="form-row">
                <div class="col-md-6">
                    <label>.date-picker</label>
                    <div class="input-group">
                        <input type="text" name="name" class="form-control date-picker" />
                        <span class="input-group-append hand">
                            <i class="fa fa-calendar input-group-text"></i>
                        </span>
                    </div>
                    <pre>
<span><</span>div class="input-group">
    <span><</span>input type="text" name="name" class="form-control date-picker" />
    <span><</span>span class="input-group-append hand">
        <span><</span>i class="fa fa-calendar input-group-text"><span><</span>/i>
    <span><</span>/span>
<span><</span>/div>
                    </pre>
                </div>
                <div class="col-md-6">
                    <label>.datetime-picker</label>
                    <div class="input-group">
                        <input type="text" name="name" class="form-control datetime-picker" />
                        <span class="input-group-append hand">
                            <i class="fa fa-calendar input-group-text"></i>
                        </span>
                    </div>
                    <pre>
<span><</span>div class="input-group">
    <span><</span>input type="text" name="name" class="form-control datetime-picker" />
    <span><</span>span class="input-group-append hand">
        <span><</span>i class="fa fa-calendar input-group-text"><span><</span>/i>
    <span><</span>/span>
<span><</span>/div>
                    </pre>
                </div>
            </div>

        </div>

        <%--<div class="mat-box">

            <!-- Material Box-->
            <h3 style="margin-top:0;">
                <a id="material-box" href="javascript:;">#material-box</a><br />
            </h3>
            <div class="show-material-box">
                <div class="form-row">
                    <div class="col-sm-4">
                        <div class="mat-box">
                            <b>mat-box<span class="text-danger">*</span></b>
                            <pre><span><</span>div class="mat-box"><span>...<</span>/div></pre>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="mat-box z-depth-0">
                            <b>mat-box z-depth-0</b>
                            <pre><span><</span>div class="mat-box z-depth-0"><span>...<</span>/div></pre>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="mat-box z-depth-1">
                            <b>mat-box z-depth-1</b>
                            <pre><span><</span>div class="mat-box z-depth-1"><span>...<</span>/div></pre>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="mat-box z-depth-1-half">
                            <b>mat-box z-depth-1-half</b>
                            <pre><span><</span>div class="mat-box z-depth-1-half"><span>...<</span>/div></pre>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="mat-box z-depth-2">
                            <b>mat-box z-depth-2</b>
                            <pre><span><</span>div class="mat-box z-depth-2"><span>...<</span>/div></pre>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="mat-box z-depth-3">
                            <b>mat-box z-depth-3</b>
                            <pre><span><</span>div class="mat-box z-depth-3"><span>...<</span>/div></pre>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="mat-box z-depth-4">
                            <b>mat-box z-depth-4</b>
                            <pre><span><</span>div class="mat-box z-depth-4"><span>...<</span>/div></pre>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="mat-box z-depth-5">
                            <b>mat-box z-depth-5</b>
                            <pre><span><</span>div class="mat-box z-depth-5"><span>...<</span>/div></pre>
                        </div>
                    </div>
                </div>


            </div>

        </div>--%>
        <div class="mat-box">
           

            <!-- Header -->
            <h3>
                <a id="header" href="javascript:;">#header</a><br />
            </h3>

            <div class="form-row">
                <div class="col-md-6">
                    <h1>H1 - Header Title สวัสดีภาษาไทย</h1>
                    <h2>H2 - Header Title สวัสดีภาษาไทย</h2>
                    <h3>H3 - Header Title สวัสดีภาษาไทย<span class="text-danger">*</span></h3>
                    <h4>H4 - Header Title สวัสดีภาษาไทย</h4>
                    <h5>H5 - Header Title สวัสดีภาษาไทย</h5>
                    <h6>H6 - Header Title สวัสดีภาษาไทย</h6>
                </div>
                <div class="col-md-6">
<pre>
<span><</span>h1>...<span><</span>/h1>
</pre>
<pre>
<span><</span>h2>...<span><</span>/h2>
</pre>
<pre>
<span><</span>h3>...<span><</span>/h3>
</pre>
<pre>
<span><</span>h4>...<span><</span>/h4>
</pre>
<pre>
<span><</span>h5>...<span><</span>/h5>
</pre>
<pre>
<span><</span>h6>...<span><</span>/h6>
</pre>
                </div>
            </div>

        </div>
        <div class="mat-box">

            <!-- Button -->
            <h3>
                <a id="button" href="javascript:;">#button</a><br />
            </h3>

            <span class="btn btn-default">Default</span>
            <span class="btn btn-primary">Primary</span>
            <span class="btn btn-success">Success</span>
            <span class="btn btn-info">Info</span>
            <span class="btn btn-warning">Warning</span>
            <span class="btn btn-danger">Danger</span>
            <br /><br />
            <span class="btn btn-default" disabled>Default</span>
            <span class="btn btn-primary" disabled>Primary</span>
            <span class="btn btn-success" disabled>Success</span>
            <span class="btn btn-info" disabled>Info</span>
            <span class="btn btn-warning" disabled>Warning</span>
            <span class="btn btn-danger" disabled>Danger</span>
            <br /><br />
<pre>
<span><</span>span class="btn btn-default">...<span><</span>/span>
</pre>
<pre>
<span><</span>span class="btn btn-primary">...<span><</span>/span>
</pre>
<pre>
<span><</span>span class="btn btn-success">...<span><</span>/span>
</pre>
<pre>
<span><</span>span class="btn btn-info">...<span><</span>/span>
</pre>
<pre>
<span><</span>span class="btn btn-warning">...<span><</span>/span>
</pre>
<pre>
<span><</span>span class="btn btn-danger">...<span><</span>/span>
</pre>

        </div>
        <div class="mat-box">

            <!-- Dropdown Toggle -->
            <h3>
                <a id="dropdown" href="javascript:;">#dropdown</a><br />
            </h3>

            <div class="btn-group">
                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Default <span class="caret"></span></button>
                <ul class="dropdown-menu">
                    <li><a href="#">Action</a></li>
                    <li><a href="#">Another action</a></li>
                    <li><a href="#">Something else here</a></li>
                    <li role="separator" class="divider"></li>
                    <li><a href="#">Separated link</a></li>
                </ul>
            </div>
            <div class="btn-group">
                <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Primary <span class="caret"></span></button>
                <ul class="dropdown-menu">
                    <li><a href="#">Action</a></li>
                    <li><a href="#">Another action</a></li>
                    <li><a href="#">Something else here</a></li>
                    <li role="separator" class="divider"></li>
                    <li><a href="#">Separated link</a></li>
                </ul>
            </div>
            <div class="btn-group">
                <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Success <span class="caret"></span></button>
                <ul class="dropdown-menu">
                    <li><a href="#">Action</a></li>
                    <li><a href="#">Another action</a></li>
                    <li><a href="#">Something else here</a></li>
                    <li role="separator" class="divider"></li>
                    <li><a href="#">Separated link</a></li>
                </ul>
            </div>
            <div class="btn-group">
                <button type="button" class="btn btn-info dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Info <span class="caret"></span></button>
                <ul class="dropdown-menu">
                    <li><a href="#">Action</a></li>
                    <li><a href="#">Another action</a></li>
                    <li><a href="#">Something else here</a></li>
                    <li role="separator" class="divider"></li>
                    <li><a href="#">Separated link</a></li>
                </ul>
            </div>
            <div class="btn-group">
                <button type="button" class="btn btn-warning dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Warning <span class="caret"></span></button>
                <ul class="dropdown-menu">
                    <li><a href="#">Action</a></li>
                    <li><a href="#">Another action</a></li>
                    <li><a href="#">Something else here</a></li>
                    <li role="separator" class="divider"></li>
                    <li><a href="#">Separated link</a></li>
                </ul>
            </div>
            <div class="btn-group">
                <button type="button" class="btn btn-danger dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Danger <span class="caret"></span></button>
                <ul class="dropdown-menu">
                    <li><a href="#">Action</a></li>
                    <li><a href="#">Another action</a></li>
                    <li><a href="#">Something else here</a></li>
                    <li role="separator" class="divider"></li>
                    <li><a href="#">Separated link</a></li>
                </ul>
            </div>
            <br /><br />
            <pre>
<span><</span>div class="btn-group">
    <span><</span>button type="button" class="btn btn-danger dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        Danger <span><</span>span class="caret"><span><</span>/span>
    <span><</span>/button>
    <span><</span>ul class="dropdown-menu">
        <span><</span>li><span><</span>a href="#">Action<span><</span>/a><span><</span>/li>
        <span><</span>li><span><</span>a href="#">Another action<span><</span>/a><span><</span>/li>
        <span><</span>li><span><</span>a href="#">Something else here<span><</span>/a><span><</span>/li>
        <span><</span>li role="separator" class="divider"><span><</span>/li>
        <span><</span>li><span><</span>a href="#">Separated link<span><</span>/a><span><</span>/li>
    <span><</span>/ul>
<span><</span>/div>
</pre>

        </div>
        <div class="mat-box">

            <!-- Text Color -->
            <h3>
                <a id="text-color" href="javascript:;">#text-color</a><br />
            </h3>

            <p style="font-size:20px;">
                <span class="text-primary">
                    text-primary
                </span>,
                <span class="text-success">
                    text-success
                </span>,
                <span class="text-info">
                    text-info
                </span>,
                <span class="text-warning">
                    text-warning
                </span>,
                <span class="text-danger">
                    text-danger
                </span>
            </p>

            <pre>
<span><</span>span class="text-primary">
    text-primary
<span><</span>/span>
<span><</span>span class="text-success">
    text-success
<span><</span>/span>
<span><</span>span class="text-info">
    text-info
<span><</span>/span>
<span><</span>span class="text-warning">
    text-warning
<span><</span>/span>
<span><</span>span class="text-danger">
    text-danger
<span><</span>/span>
</pre>

        </div>
        <div class="mat-box">
            <!-- Text Align -->
            <h3>
                <a id="text-align" href="javascript:;">#text-align</a><br />
            </h3>
            <div class="mat-box">
                <div class="text-left">
                    <b>text-left</b>
                </div>
            </div>
            <div class="mat-box">
                <div class="text-center">
                    <b>text-center</b>
                </div>
            </div>
            <div class="mat-box">
                <div class="text-right">
                    <b>text-right</b>
                </div>
            </div>

        </div>
        <div class="mat-box">

            <!-- Alert -->
            <h3>
                <a id="alert" href="javascript:;">#alert</a><br />
            </h3>

            <div class="alert  alert-success">
                alert alert-success
            </div>
            <div class="alert  alert-info">
                alert alert-info
            </div>
            <div class="alert  alert-warning">
                alert alert-warning
            </div>
            <div class="alert  alert-danger">
                alert alert-danger
            </div>

        </div>
        <div class="mat-box">

            <!-- Panel -->
            <h3>
                <a id="panel" href="javascript:;">#panel</a><br />
            </h3>

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Panel title</h3>
                </div>
                <div class="panel-body">Panel content </div>
            </div>
            <div class="panel panel-success">
                <div class="panel-heading">
                    <h3 class="panel-title">Panel title</h3>
                </div>
                <div class="panel-body">Panel content </div>
            </div>
            <div class="panel panel-info">
                <div class="panel-heading">
                    <h3 class="panel-title">Panel title</h3>
                </div>
                <div class="panel-body">Panel content </div>
            </div>
            <div class="panel panel-warning">
                <div class="panel-heading">
                    <h3 class="panel-title">Panel title</h3>
                </div>
                <div class="panel-body">Panel content </div>
            </div>
            <div class="panel panel-danger">
                <div class="panel-heading">
                    <h3 class="panel-title">Panel title</h3>
                </div>
                <div class="panel-body">Panel content </div>
            </div>

        </div>
        <div class="mat-box">

            <!-- Modal -->
            <h3>
                <a id="modal" href="javascript:;">#modal</a><br />
            </h3>
            <button type="button" class="btn btn-info" data-toggle="modal" data-target="#myModal">Open Modal</button>

            <div id="myModal" class="modal fade" role="dialog">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Modal Header</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>
                        <div class="modal-body">
                            <p>Some text in the modal.</p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>

                </div>
            </div>

            <br /><br />

            <pre>
<span><</span>button type="button" class="btn btn-info" data-toggle="modal" data-target="#myModal">Open Modal<span><</span>/button>

<span><</span>div id="myModal" class="modal fade" role="dialog">
    <span><</span>div class="modal-dialog">

        <span><</span>!-- Modal content-->
        <span><</span>div class="modal-content">
            <span><</span>div class="modal-header">
                <span><</span>h4 class="modal-title">Modal Header<span><</span>/h4>
                <span><</span>button type="button" class="close" data-dismiss="modal">&times;<span><</span>/button>
            <span><</span>/div>
            <span><</span>div class="modal-body">
                <span><</span>p>Some text in the modal.<span><</span>/p>
            <span><</span>/div>
            <span><</span>div class="modal-footer">
                <span><</span>button type="button" class="btn btn-default" data-dismiss="modal">Close<span><</span>/button>
            <span><</span>/div>
        <span><</span>/div>

    <span><</span>/div>
<span><</span>/div>
            </pre>

        </div>
        <div class="mat-box">

            <!-- Column responsive-->
            <h3 style="margin-top:0;">
                <a id="column-responsive" href="javascript:;">#column-responsive</a><br />
            </h3>

            <label class="text-success">4 Column on wide screen</label>
            <div class="form-row">
                <div class="col-sm-6 col-md-4 col-lg-3">
                    <div class="mat-box text-center">
                        <b>col-sm-6 col-md-4 col-lg-3</b>
                    </div>
                </div>
                <div class="col-sm-6 col-md-4 col-lg-3">
                    <div class="mat-box text-center">
                        <b>col-sm-6 col-md-4 col-lg-3</b>
                    </div>
                </div>
                <div class="col-sm-6 col-md-4 col-lg-3">
                    <div class="mat-box text-center">
                        <b>col-sm-6 col-md-4 col-lg-3</b>
                    </div>
                </div>
                <div class="col-sm-6 col-md-4 col-lg-3">
                    <div class="mat-box text-center">
                        <b>col-sm-6 col-md-4 col-lg-3</b>
                    </div>
                </div>
            </div>

            <label class="text-success">3 Column on wide screen</label>
            <div class="form-row">
                <div class="col-md-6 col-lg-4">
                    <div class="mat-box text-center">
                        <b>col-md-6 col-lg-4</b>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4">
                    <div class="mat-box text-center">
                        <b>col-md-6 col-lg-4</b>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4">
                    <div class="mat-box text-center">
                        <b>col-md-6 col-lg-4</b>
                    </div>
                </div>
            </div>

            <label class="text-success">2 Column on wide screen</label>
            <div class="form-row">
                <div class="col-lg-6">
                    <div class="mat-box text-center">
                        <b>col-lg-6</b>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="mat-box text-center">
                        <b>col-lg-6</b>
                    </div>
                </div>
            </div>

            <label class="text-success">3 Column on wide screen with central contents</label>
            <div class="form-row">
                <div class="col-lg-3 col-md-4">
                    <div class="mat-box text-center">
                        <b>col-lg-3 col-md-4</b>
                    </div>
                </div>
                <div class="col-lg-6 col-md-8">
                    <div class="mat-box text-center">
                        <b>col-lg-6 col-md-8</b>
                    </div>
                </div>
                <div class="col-lg-3 col-md-8 col-lg-offset-0  col-md-offset-4">
                    <div class="mat-box text-center">
                        <b>col-lg-3 col-md-8 col-lg-offset-0  col-md-offset-4</b>
                    </div>
                </div>
            </div>

        </div>
        <div class="mat-box">

            <!-- Form Control -->
            <h3 style="margin-top:0;">
                <a id="form-control" href="javascript:;">#form-control</a><br />
            </h3>

            <div class="form-row">
                <div class="col-sm-6">
                    <label>ราคา (บาท)</label>
                    <input type="text" class="form-control" placeholder="textbox1" name="name" value="" />
                </div>
                <div class="col-sm-6">
                    <label>ค้นหา</label>
                    <div class="input-group">
                        <input type="text" class="form-control" placeholder="textbox1" name="name" value="" />
                        <span class="input-group-append text-center">
                            <i class="fa fa-search input-group-text"></i>
                        </span>
                    </div>
                </div>
            </div>

        </div>
        <div class="mat-box">


            <!-- Table -->
            <h3 style="margin-top:0;">
                <a id="table" href="javascript:;">#table</a><br />
            </h3>

            <table class="table table-striped table-hover table-bordered table-ag">
                <tr>
                    <th>
                         Header1
                    </th>
                    <th>
                         Header2
                    </th>
                    <th style="width:80px;" class="text-center">
                        Manage
                    </th>
                </tr>
                <tr>
                    <td>
                         context 1-1
                    </td>
                    <td>
                         context 1-2
                    </td>
                    <td class="text-center">
                        <i class="fa fa-pencil hand"></i>
                    </td>
                </tr>
                <tr>
                    <td>
                         context 2-1
                    </td>
                    <td>
                         context 2-2
                    </td>
                    <td class="text-center">
                        <i class="fa fa-pencil hand"></i>
                    </td>
                </tr>
            </table>

<pre>
<span><</span>table class="table table-striped table-hover table-bordered table-ag">
        .......
<span><</span>/table>
</pre>

        </div>


        <div class="mat-box ht" id="asp-panel">
            <h3 style="margin-top:0;">
                ASP
            </h3>
            <a href="#asp-repeater" class="btn btn-primary">asp-repeater</a>
        </div>


        <div class="mat-box">

            <!-- Repeater -->
            <h3 style="margin-top:0;">
                <a id="asp-repeater" href="javascript:;">#asp-repeater</a><br />
            </h3>

            <div class="form-row">
                <asp:Repeater runat="server" id="rptCard" OnItemDataBound="rptCard_ItemDataBound">
                    <ItemTemplate>
                        <div class="col-sm-6 col-md-4 col-lg-3">
                            <div class="mat-box">
                                <asp:HiddenField id="hiddenKey" runat="server" Value='<%# Eval("CustomerCode") %>' />
                                <%# Eval("CustomerName") %>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

<pre>
<span><</span>div class="form-row">
    <span><</span>asp:Repeater runat="server" id="rptCard" OnItemDataBound="rptCard_ItemDataBound">
        <span><</span>ItemTemplate>
            <span><</span>div class="col-sm-6 col-md-4 col-lg-3">
                <span><</span>div class="mat-box">
                    <span><</span>asp:HiddenField id="hiddenKey" runat="server"  Value='<span><</span>%# Eval("CustomerCode") %>' />
                    <span><</span>%# Eval("CustomerName") %>
                <span><</span>/div>
            <span><</span>/div>
        <span><</span>/ItemTemplate>
    <span><</span>/asp:Repeater>
<span><</span>/div>
</pre>
        </div>
    </div>
</asp:Content>
