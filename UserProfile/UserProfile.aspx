<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="UserProfile.aspx.cs" Inherits="ServiceWeb.UserProfile.UserProfile" %>

<%@ Register Src="~/UserControl/AGapeGallery/UploadGallery/UploadGallery.ascx" TagPrefix="uc1" TagName="UploadGallery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style type="text/css">
        #table-coverage > tbody > tr:nth-child(2) > td {
            padding-top: 1.25rem;
        }

        #table-coverage > tbody > tr > td {
            border-top: 0;
        }

            #table-coverage > tbody > tr > td.card-header {
                padding: .75rem .3rem;
            }

            #table-coverage > tbody > tr > td:first-child {
                padding-left: 1.25rem;
            }

            #table-coverage > tbody > tr > td:last-child {
                padding-right: 1.25rem;
            }
    </style>
    <style>
        .box-show {
            display: inline-block;
            width: 100px;
            text-align: center;
            height: 60px;
            padding: 10px 4px 0;
            margin: 0 5px 8px 0;
            text-transform: uppercase;
            border: 1px solid #cccccc;
            color: #707070;
            font-size: 10px;
            font-weight: 300;
            line-height: 14px;
        }

        .image-block-add {
            display: inline-block;
            vertical-align: top;
        }

        .add-image {
            background: transparent;
            width: 60px;
            height: 60px;
            display: inline-block;
            margin-right: 5px;
            margin-bottom: 5px;
            border: 2px dashed #009688;
            border-radius: 50%;
            background-position: center center;
            -webkit-background-size: cover;
            -moz-background-size: cover;
            background-size: cover;
            -o-background-size: cover;
            position: relative;
            text-align: center;
            padding-top: 28%;
            font-size: 16px;
        }

        .text-effect {
            opacity: 0.7;
            color: #009688;
        }

        .text-effect-label {
            font-weight: bold;
            font-size: 16px;
            padding-left: 10px;
        }

        .panel-feed-activity-card-top {
            padding: 10px;
            border-bottom: 1px solid #d0d1d5;
            border-color: #e5e6e9 #dfe0e4;
            position: relative;
        }

        .panel-feed-activity-card-bottom {
            padding: 15px;
            background: #F6F7F8;
        }

        .panel-feed-activity-card-footer {
            padding: 15px;
        }

        .panel-feed-activity-card-footer-inner {
            line-height: 21px;
            max-height: 105px;
            overflow-y: hidden;
        }

        .panel-feed-activity-card-title-pic {
            width: 100%;
            height: 400px;
            background-position: center center;
            -webkit-background-size: cover;
            -moz-background-size: cover;
            background-size: cover;
            -o-background-size: cover;
            background-color: #FCFCFC;
            position: relative;
            background-attachment: scroll;
        }

        .social-relation .panel-feed-activity-card {
            margin-top: 15px;
        }

            .social-relation .panel-feed-activity-card:first-child {
                margin-top: 0;
            }

        .panel-feed-activity-card {
            background: #fff;
            border: 1px solid;
            border-color: #e5e6e9 #dfe0e4 #d0d1d5;
            border-radius: 3px;
        }

        .green-text {
            color: #009688;
        }

        .grey-text-bold {
            color: #A9A4A4;
            font-weight: bold;
        }

        .icon-action {
            opacity: 0.5;
            cursor: pointer;
        }

            .icon-action:hover {
                opacity: 0.8;
            }

        .text-ellipsis {
            word-break: break-all;
            text-overflow: ellipsis;
            overflow-x: hidden;
            white-space: nowrap;
        }
        .txt_otherposition
        {
            margin-left:5%;
            width:95% !important;
        }
        
        .title-course-List {
            position: relative;
            display: block;
            color: #009688;
            width: 100%;
            padding: 7px 3px;
            font-size: 14px;
            font-style: normal;
            line-height: 20px;
            margin-left: 0px;
            overflow: hidden;
            word-break: break-all;
        }

         .background-profile{
            margin-bottom: 10px;
            padding-top: 20px;
            background: rgba(0,0,0,1);
            background: -moz-linear-gradient(top, rgba(0,0,0,1) 0%, rgba(0,0,0,1) 0%, rgba(36,36,36,1) 22%, rgba(145,145,145,1) 42%, rgba(255,255,255,1) 70%, rgba(255,255,255,1) 84%, rgba(255,255,255,1) 100%);
            background: -webkit-gradient(left top, left bottom, color-stop(0%, rgba(0,0,0,1)), color-stop(0%, rgba(0,0,0,1)), color-stop(22%, rgba(36,36,36,1)), color-stop(42%, rgba(145,145,145,1)), color-stop(70%, rgba(255,255,255,1)), color-stop(84%, rgba(255,255,255,1)), color-stop(100%, rgba(255,255,255,1)));
            background: -webkit-linear-gradient(top, rgba(0,0,0,1) 0%, rgba(0,0,0,1) 0%, rgba(36,36,36,1) 22%, rgba(145,145,145,1) 42%, rgba(255,255,255,1) 70%, rgba(255,255,255,1) 84%, rgba(255,255,255,1) 100%);
            background: -o-linear-gradient(top, rgba(0,0,0,1) 0%, rgba(0,0,0,1) 0%, rgba(36,36,36,1) 22%, rgba(145,145,145,1) 42%, rgba(255,255,255,1) 70%, rgba(255,255,255,1) 84%, rgba(255,255,255,1) 100%);
            background: -ms-linear-gradient(top, rgba(0,0,0,1) 0%, rgba(0,0,0,1) 0%, rgba(36,36,36,1) 22%, rgba(145,145,145,1) 42%, rgba(255,255,255,1) 70%, rgba(255,255,255,1) 84%, rgba(255,255,255,1) 100%);
            background: linear-gradient(to bottom, rgba(0,0,0,1) 0%, rgba(0,0,0,1) 0%, rgba(36,36,36,1) 22%, rgba(145,145,145,1) 42%, rgba(255,255,255,1) 70%, rgba(255,255,255,1) 84%, rgba(255,255,255,1) 100%);
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#000000', endColorstr='#ffffff', GradientType=0 );
        }
         .image-box {
    background-position: center center;
    -webkit-background-size: cover;
    -moz-background-size: cover;
    background-size: cover;
    -o-background-size: cover;
    border-radius: 0;
    border: 1px solid;
    border-color: #e5e6e9 #dfe0e4 #d0d1d5;
}
         .dot_noconn {
          height: 15px;
          width: 15px;
          background-color: #bbb;
          border-radius: 50%;
          display: inline-block;
        }
         .dot_conn {
          height: 15px;
          width: 15px;
          background-color: forestgreen;
          border-radius: 50%;
          display: inline-block;
        }
    </style>

    <div class="container mat-box" style="padding: 0;">
        <div>
            <asp:HiddenField ID="hfdStudentLinkId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdfMyLinkId" runat="server" ClientIDMode="Static" />
            <div class="row">
                <div class="col-md-12">
                    <div class="background-profile">
                        <div id="divImageProfile_Redesign" runat="server" class="image-box" style="width: 175px;height: 175px;margin-top:-6px;margin: 0 auto;border: none;border-radius: 50%;">
                        </div>
                        <asp:UpdatePanel runat="server" ID="udpTitle" UpdateMode="Conditional" style="text-align:center;">
                            <ContentTemplate>
                                <h1>
                                    <asp:Label id="lbHeaderName" runat="server" style="font-size: 36px;"></asp:Label>&nbsp;
                                    <% if(ISMyProFile){ %>
                                    <span class="fa fa-pencil icon-action hide" onclick="showAddProfile();"></span>
                                    <% } %>
                                </h1>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div>
                            <hr />
                        </div>
                    </div>
                    <div style="padding: 0px 50px 20px 50px;">
                        <div id="panel-add-profile" class="row">
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-4" style="padding:5px;display: none;">
                                        <div id="divImageProfile" runat="server" class="image-box" style="width: 72px; height: 72px;margin-top:-6px"></div>
                                    </div>
                                    <div class="col-md-12" style="line-height: 20px;height: 100px;">
                                        <asp:UpdatePanel runat="server" ID="udpName" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="text-ellipsis" style="overflow:hidden;">
                                                    <strong><asp:Label id="lbName" runat="server" style="color:#009688;font-size:16px;"></asp:Label></strong>
                                                </div>
                                                <asp:Label id="lbResume" runat="server">Classroom teacher Ladue Middle School St. Louis, MO</asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <% if(ISMyProFile){ %>
                                        <i onclick="showAddProfile();" class="btn btn-success btn-xs" style="cursor:pointer;font-size: 12px;margin-top:10px;"><i class="fa fa-cog"></i>&nbsp;Edit My Profile</i>                                
                                        <% } %>

                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" style="display: none;">
                                            <ContentTemplate>
                                               <%-- <asp:LinkButton ID="btnFollow" runat="server" CssClass="btn btn-success btn-xs"  OnClientClick="agroLoading(true);" Onclick="btnFollow_Click">
                                                    <i class="fa fa-eye"></i>&nbsp;&nbsp;ติดตาม
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnCancelFollow" runat="server" CssClass="btn btn-danger btn-xs"  OnClientClick="agroLoading(true);" Onclick="btnCancelFollow_Click">
                                                    <i class="fa fa-eye-slash"></i>&nbsp;&nbsp;เลิกติดตาม
                                                </asp:LinkButton>--%>   
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6" style="border-left: 1px solid #ddd;">
                                <asp:UpdatePanel runat="server" ID="udpProfile" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="" style="color:#a9a4a4;">
                                            <div style="padding-bottom:5px;">
                                                <i class="fa fa-envelope fa-fw" aria-hidden="true" style="font-size:20px;"></i>
                                                <span id="span-mail"><asp:Label id="lbEmail" runat="server"></asp:Label></span>
                                            </div>
                                            <div style="padding-bottom:5px;">
                                                <i class="fa fa-university fa-fw" aria-hidden="true" style="font-size:20px;"></i>
                                                <span id="span-position"><asp:Label id="lbPosition" runat="server"></asp:Label></span>
                                            </div>
                                            <div style="padding-bottom:5px;">
                                                <i class="fa fa-facebook-square fa-fw" aria-hidden="true" style="font-size:20px;"></i>
                                                <span id="span-fb"><asp:Label id="lbfacebook" runat="server"></asp:Label></span>
                                            </div>
                                            <div style="padding-bottom:5px;">
                                                <i class="fa fa-instagram fa-fw" aria-hidden="true" style="font-size:20px;"></i>
                                                <span id="span-ig"><asp:Label id="lbinstagram" runat="server"></asp:Label></span>
                                            </div>
                                            <div style="padding-bottom:5px;">
                                                <i class="fa fa-twitter-square fa-fw" aria-hidden="true" style="font-size:20px;"></i>
                                                <span id="span-twitter"><asp:Label id="lbtwitter" runat="server"></asp:Label></span>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div id="panel-add-profile-selected" class="" style="display:none;">
                            <div class="" style="display: inline-block;width: 109px;vertical-align: top;">
                                <div style="width: 72px; height: 72px;">
                                    <uc1:uploadgallery runat="server" id="UploadPictureTitle" accepttype="image" multiplemode="false" previewwidth="72px" previewheight="72px" />
                                </div>
                            </div>
                            <div class="" style="display: inline-block;width: calc(100% - 115px);">
                                <div class="row">
                                    <span class="col-md-2 col-lg-1" style="font-weight: bold;">ชื่อ</span>
                                    <div class="col-md-4 col-lg-5">
                                        <div class="text-ellipsis">
                                            <span style="font-size: 18px;"><asp:Textbox ID="txtFirstNameEdit" runat="server"  placeholder="กรอกชื่อของคุณ" CssClass="form-control"></asp:Textbox></span>
                                        </div> 
                                    </div>
                                    <span class="col-md-2 col-lg-1" style="font-weight: bold;">นามสกุล</span>
                                    <div class="col-md-4 col-lg-5">
                                        <div class="text-ellipsis">
                                            <span style="font-size: 18px;"><asp:Textbox ID="txtLastNameEdit" runat="server"  placeholder="กรอกนามสกุลของคุณ" CssClass="form-control"></asp:Textbox></span>
                                        </div> 
                                    </div>
                                </div>
                                <div class="row" style="margin-top: 10px;">
                                    <span class="col-md-2 col-lg-1" style="font-weight: bold;">คำทักทาย</span>
                                    <div class="col-md-10 col-lg-11">
                                        <asp:Textbox ID="txtResumeEdit" runat="server" ClientIDMode="static" placeholder="กรอกประวัติโดยย่อของคุณ" CssClass="form-control"></asp:Textbox>
                                    </div>
                                </div>
                                <div class="row" style="margin-top: 10px;">
                                    <span class="col-md-2 col-lg-1" style="font-weight: bold;">ตำแหน่ง</span>
                                    <div class="col-md-4 col-lg-5">
                                        <asp:DropDownList ID="ddlPostionEdit" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-6 col-lg-6">
                                        <div class="text-ellipsis">
                                            <asp:TextBox ID="txtPositionOtherEdit" runat="server" ClientIDMode="static" placeholder="อาชีพอื่น ๆ"
                                                 CssClass="form-control" Enabled="false"></asp:Textbox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="margin-top: 10px;">
                                    <span class="col-md-2 col-lg-1" style="font-weight: bold;">Facebook</span>
                                    <div class="col-md-4 col-lg-5">
                                        <div class="text-ellipsis">
                                            <asp:Textbox ID="txtFaceBookEdit" runat="server" ClientIDMode="static"  placeholder="กรอกชื่อบัญชีของ facebook"   CssClass="form-control"></asp:Textbox>
                                        </div>
                                    </div>
                                    <span class="col-md-2 col-lg-1" style="font-weight: bold;">Instagram</span>
                                    <div class="col-md-4 col-lg-5">
                                        <div class="text-ellipsis">
                                            <asp:Textbox ID="txtInstagramEdit" runat="server" ClientIDMode="static"  placeholder="กรอกชื่อบัญชีของ Instagram" CssClass="form-control"></asp:Textbox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="margin-top: 10px;">
                                    <span class="col-md-2 col-lg-1" style="font-weight: bold;">Twitter</span>
                                    <div class="col-md-4 col-lg-5">
                                        <div class="text-ellipsis">
                                            <asp:Textbox ID="txtTwitterEdit" runat="server" ClientIDMode="static"  placeholder="กรอกชื่อบัญชีของ Twitter"  CssClass="form-control"></asp:Textbox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="margin-top: 10px;">
                                    <div class="col-lg-offset-1 col-md-offset-2 col-lg-11 col-md-10" style="padding-left: 5px;">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default btn-xs" OnClientClick="hideAddProfile();" Text="ยกเลิก" />
                                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary btn-xs" OnClientClick="agroLoading(true);" OnClick="btnSave_Click" Text="บันทึก" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="panel-add-profile-line" class="row p-1">
                            <div class="col-md-6">
                                <div class="row">
                                    <h6>:: Linked Line Account ::</h6>
                                </div>
                                <% if (ISExistLineLink)
                                { %>
                                <div class="row d-flex align-items-center">                        
                                    <span><b>Status : </b>Linked</span><i class="fa fa-link fa-fw" aria-hidden="true" style="font-size:15px;"></i>
                                </div>
                                <% if (ISMyProFile)
                                { %>
                                <div class="row d-flex mt-2 align-items-end">
                                    <button onclick="removeLine();" type="button" class="btn btn-danger">unlink</button>
                                </div>
                                <asp:Button ID="btnUnlink" runat="server" OnClick="btnUnlink_Click" CssClass="d-none"/>
                                <% } %>
                                <% }
                                else
                                { %>
                                <div class="row d-flex align-items-center">                           
                                    <span><b>Status : </b>Not linked</span><i class="fa fa-chain-broken fa-fw" aria-hidden="true" style="font-size:15px;"></i>
                                </div>
                                <% if (ISMyProFile)
                                { %>
                                <div class="row d-flex mt-2 align-items-end">
                                    <button onclick="loginLine();" type="button" class="btn btn-primary">Link Account</button>
                                </div>
                                <% } %>
                                <%} %>                               
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--<script type="text/javascript">


        $(document).ready(function () {

        });
    </script>--%>
     <script>
        function generateCircle(id) {
            var el = document.getElementById(id); // get canvas

            var percent = Math.floor((Math.random() * 100) + 1);

            var options = {
                percent: percent || 25, //el.getAttribute('data-percent') || 25,
                size: el.getAttribute('data-size') || 100,
                lineWidth: el.getAttribute('data-line') || 10,
                rotate: el.getAttribute('data-rotate') || 0
            }

            var canvas = document.createElement('canvas');
            var span = document.createElement('span');
            span.textContent = options.percent + '%';

            if (typeof (G_vmlCanvasManager) !== 'undefined') {
                G_vmlCanvasManager.initElement(canvas);
            }

            var ctx = canvas.getContext('2d');
            canvas.width = canvas.height = options.size;


            el.appendChild(span);
            el.appendChild(canvas);
         

            ctx.translate(options.size / 2, options.size / 2); // change center
            ctx.rotate((-1 / 2 + options.rotate / 180) * Math.PI); // rotate -90 deg

            //imd = ctx.getImageData(0, 0, 240, 240);
            var radius = (options.size - options.lineWidth) / 2;

            var drawCircle = function (color, lineWidth, percent) {
                percent = Math.min(Math.max(0, percent || 1), 1);
                ctx.beginPath();
                ctx.arc(0, 0, radius, 0, Math.PI * 2 * percent, false);
                ctx.strokeStyle = color;
                ctx.lineCap = 'butt'; // butt, round or square
                ctx.lineWidth = lineWidth
                ctx.stroke();
            };

            drawCircle('#a9a4a4', options.lineWidth, 100 / 100);
            drawCircle('#2F343B', options.lineWidth, options.percent / 100);
        }

         function showAddProfile() {
            $("#panel-add-profile").hide();
            $("#panel-add-profile-line").hide();
            $("#panel-add-profile-selected").show();
        }

        function hideAddProfile() {

            $("#panel-add-profile-selected").hide();
            $("#panel-add-profile").show();
            $("#panel-add-profile-line").show();

         }

         function loginLine() {
             var redirect = '<%= ConfigurationManager.AppSettings["LINE_LOGIN_API_RE_DIRECT"] %>'
             var CLIENT_ID = '<%= ConfigurationManager.AppSettings["LINE_LOGIN_CHANNEL_ID"] %>'
             var EmployeeCode = '<%= GetEmpCode() %>'
             window.location.replace('https://access.line.me/oauth2/v2.1/authorize?response_type=code&client_id=' + CLIENT_ID + '&redirect_uri=' + redirect + '&state=' + EmployeeCode + '&bot_prompt=aggressive&scope=profile%20openid')
         }

         function removeLine() {
             $("#<%= btnUnlink.ClientID %>").click();
         }

    </script>
    <script>
        function changePositionEdit(obj) {
            if ($(obj).val() == "") {

                $("#<%=txtPositionOtherEdit.ClientID%>").attr("disabled", false);
            }
            else {
                $("#<%=txtPositionOtherEdit.ClientID%>").attr("disabled", true);
            }
        }

        function clearjQueryCache() {
            for (var x in jQuery.cache) {
                delete jQuery.cache[x];
            }
            setTimeout(function () { location.reload(); }, 500);
        }
    </script>
</asp:Content>
