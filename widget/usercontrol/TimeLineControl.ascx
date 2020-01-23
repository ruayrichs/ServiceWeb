<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TimeLineControl.ascx.cs" Inherits="ServiceWeb.widget.usercontrol.TimeLineControl" %>

<div class="hide">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpOption">
        <ContentTemplate>
            <asp:HiddenField runat="server" Value="false" ID="hddIsHasFile" />
            <asp:HiddenField runat="server" Value="false" ID="hddIsHyperLink" />
            <asp:HiddenField runat="server" Value="false" ID="hddIsDateTime" />
            <asp:HiddenField runat="server" Value="false" ID="hddKeyAobjectlink" />
            <asp:HiddenField runat="server" Value="" ID="hddProgramID" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

    <link href="<%= Page.ResolveUrl("~/Lib-tablemodel/FeasibilityFinancialProjectionV2.css") %>" rel="stylesheet" />
	
	<script src="<%= Page.ResolveUrl("~/AGFramework/chat/Activity-Chatting.js?vs=20190113") %>" type="text/javascript"></script>
	<script src="<%= Page.ResolveUrl("~/AGFramework/chat/linkify.js?vs=20190113") %>"> type="text/javascript"</script>
	<script src="<%= Page.ResolveUrl("~/AGapeGalleryFinal/agape-gallery-3.0.js?vs=20190113") %>" type="text/javascript"></script>
	<script src="<%= Page.ResolveUrl("~/AGFramework/ag-js.js?vs=20190113") %>" type="text/javascript"></script>
	<link href="<%= Page.ResolveUrl("~/AGFramework/chat/Activity-Chatting-1.7.css") %>" rel="stylesheet">

<style>
    .timeline-control {
        background-color: #474e5d;
        font-family: Helvetica, sans-serif;
        padding:5px;
    }

        /* The actual timeline (the vertical ruler) */
        .timeline-control .timeline-tm {
            position: relative;
            max-width: 1200px;
            margin: 0 auto;
        }

            /* The actual timeline (the vertical ruler) */
            .timeline-control .timeline-tm::after {
                content: '';
                position: absolute;
                width: 6px;
                background-color: white;
                top: 0;
                bottom: 0;
                left: 50%;
                margin-left: -3px;
            }

        /* Container around content */
        .timeline-control .container-tm {
            padding: 10px 40px;
            position: relative;
            background-color: inherit;
            width: 50%;
        }

            /* The circles on the timeline */
            .timeline-control .container-tm::after {
                content: '';
                position: absolute;
                width: 25px;
                height: 25px;
                right: -17px;
                background-color: white;
                border: 4px solid #FF9F55;
                top: 15px;
                border-radius: 50%;
                z-index: 1;
            }

        /* Place the container to the left */
        .timeline-control .left-tm {
            left: -5px;
        }

        /* Place the container to the right */
        .timeline-control .right-tm {
            left: calc(50% - -5px);
        }

        /* Add arrows to the left container (pointing right) */
        .timeline-control .left-tm::before {
            content: " ";
            height: 0;
            position: absolute;
            top: 22px;
            width: 0;
            z-index: 1;
            right: 30px;
            border: medium solid white;
            border-width: 10px 0 10px 10px;
            border-color: transparent transparent transparent white;
        }

        /* Add arrows to the right container (pointing left) */
        .timeline-control .right-tm::before {
            content: " ";
            height: 0;
            position: absolute;
            top: 22px;
            width: 0;
            z-index: 1;
            left: 30px;
            border: medium solid white;
            border-width: 10px 10px 10px 0;
            border-color: transparent white transparent transparent;
        }

        /* Fix the circle for containers on the right side */
        .timeline-control .right-tm::after {
            left: -18px;
        }

        /* The actual content */
        .timeline-control .content-tm {
            padding: 20px 30px;
            background-color: white;
            position: relative;
            border-radius: 6px;
        }

    /* Media queries - Responsive timeline on screens less than 600px wide */
    @media screen and (max-width: 800px) {
        /* Place the timelime to the left */
        .timeline-control .timeline-tm::after {
            left: 31px;
        }

        /* Full-width containers */
        .timeline-control .container-tm {
            width: 100%;
            padding-left: 70px;
            padding-right: 25px;
        }

            /* Make sure that all arrows are pointing leftwards */
            .timeline-control .container-tm::before {
                left: 60px;
                border: medium solid white;
                border-width: 10px 10px 10px 0;
                border-color: transparent white transparent transparent;
            }

        /* Make sure all circles are at the same spot */
        .timeline-control .left-tm::after,
        .timeline-control .right-tm::after {
            left: 23px;
        }

        /* Make all right containers behave like the left ones */
        .timeline-control .right-tm {
            left: calc(0% - 5px);
        }
    }

    .header-datetime-tm {
        margin-bottom: 10px;
        padding-bottom: 5px;
        border-bottom: 1px dashed #aaa;
    }

    /*


    .system-message-comment-container-remark-img {
        width: 40px;
        height: 40px;
        border-radius: 0;
        position: absolute;
        top: 10px;
        left: 10px;
        background-position: center center;
        -webkit-background-size: cover;
        -moz-background-size: cover;
        background-size: cover;
        -o-background-size: cover;
        border: 1px solid;
        border-color: #e5e6e9 #dfe0e4 #d0d1d5;
    }
    .system-message-comment-container-reply small {
        color: #fff;
    }*/
	.system-message-comment-container{
		background-color: #fff;
	}
    .system-message-comment-container-remark-storage {
        display: none;
    }

	.system-message-attach-image-block,
    .system-message-attach-file-block {
        cursor: pointer;
    }
    .fa-icon-type {
        font-size: 30px;
        margin-top: 5px;
    }
</style>

<div class="<%= ClientID %>">
    <script>

        function SaveLogRemark(remark, files) {
            try {
                var arr_FileName = [];
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];
                    arr_FileName.push(file.file.name);
                }

                $("#<%= hdd_LogRemark.ClientID %>").val(remark);
                $("#<%= hdd_LogListFile.ClientID %>").val(arr_FileName.toString());
            } catch (e) {

            }

            $("#<%= btnSaveLog.ClientID %>").click();
            AGLoading(true);
        }

        function reloadTimeLine(a, b) {
            $("#btnReloadTimeLine").click();
            AGLoading(true);
        }

        function getremarkservice(aobjectlink) {
            $("#ag-remark").children().remove();
            $("#ag-remark").AGActivityRemark(aobjectlink, "<%=  ServiceWeb.Service.PublicAuthentication.FocusOneLinkProfileImage %>", SaveLogRemark);
            
        }

        function ReLoadEquipmentLog() {
            try {
                $("#btnReloadLog").click();
            } catch (e) {

            }
        }
    </script>

    <div class="timeline-control">
        <asp:UpdatePanel ID="updRemarkService" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="ag-remark" style="padding: 15px 25px 0px 25px;"></div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <hr />
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpTimeLine">
            <ContentTemplate>
                <asp:Panel runat="server" ID="panelTimeLine">
                    <div class="timeline-tm">
                        <asp:Repeater runat="server" ID="rptTimeLine" OnItemDataBound="rptTimeLine_ItemDataBound">
                            <ItemTemplate>
                                <div class="container-tm <%# Container.ItemIndex % 2 == 0 ? " left-tm " : " right-tm " %>">
                                    <div class="content-tm">
                                        <h4 class="header-datetime-tm"><%# Eval("Date_Display") %></h4>
                                        <h6 style="margin-bottom: 10px;"><%# Eval("Title") %></h6>
                                        <div>
                                            <p>
                                                <%# Eval("Description") %>
                                            </p>
                                            <asp:HiddenField runat="server" ID="hddDataKey" Value='<%# Eval("Date_DB") %>' />
                                            <asp:Repeater runat="server" ID="rptListFile">
                                                <ItemTemplate>
                                                    <div class="file-attachment"
                                                        data-name="<%# Eval("FileName") %>"
                                                        data-link="<%# Eval("FileUrl") %>">
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="panelEmpty" Visible="false">
                    <br />
                    <div class="alert alert-info">
                        No Timeline File Attachment.
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="hide">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button Text="" runat="server" ID="btnReloadTimeLine" ClientIDMode="Static"
                        OnClick="btnReloadTimeLine_Click" CssClass="d-none" />

                    
                    <asp:Button Text="" runat="server" ID="btnSaveLog"
                        OnClick="btnSaveLog_Click" CssClass="d-none" />
                    <asp:HiddenField runat="server" ID="hdd_LogRemark" />
                    <asp:HiddenField runat="server" ID="hdd_LogListFile" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<script>
    var arrWordType = ["doc", "dot", "wbk", "docx", "docm", "dotx", "dotm", "docb"];
    var arrExcelType = ["xls", "xlt", "xlm", "xlsx", "xlsm", "xltx", "xltm"];
    var arrPowerpoitType = ["ppt", "pot", "pptx", "pptm", "potx", "potm", "ppam", "ppsx", "ppsm", "sldx", "sldm"];
    var arrPdfType = ["pdf"];
    var arrVideoType = ["mp4", "webm", "flv", "vob", "avi", "wmv", "amv", "m4p", "3gp"];
    var arrAudioType = ["mp3", "mp2"];
    var arrImgType = ["jpg", "jpeg", "png", "bmp", "gif", "svg"];

    function afterBinderTimeline() {
        $(".<%= ClientID %> .file-attachment").each(function () {
            var name = $(this).attr("data-name");
            var link = $(this).attr("data-link");

            var arrFile, fileType;
            try {
                arrFile = name.split('.');
                fileType = arrFile[arrFile.length - 1];
            } catch (e) { }

            if (arrImgType.indexOf(fileType) !== -1) {
                $(this).addClass("system-message-attach-image-block");
                $(this).css({
                    "background-image": "url('" + servictWebDomainName.slice(0, -1) + link + "')"
                });
            } else {
                $(this).addClass("system-message-attach-file-block");
                $(this).html(
                    "<div style='margin-bottom:5px'><i class='fa " + getIconFileType(name) + " fa-icon-type'></i></div><span class='four-line'>" + name + "</span>"
                );
            }

            $(this).on("click", function () {
                var file_path = link;
                var a = document.createElement('A');
                a.href = file_path;
                a.target = '_blank';
                a.download = file_path.substr(file_path.lastIndexOf('/') + 1);
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);
                //window.open(link, 'download');
            });
        });
    }

    function getIconFileType(file) {
        var arrFile, fileType;

        try {
            arrFile = file.split('.');
            fileType = arrFile[arrFile.length - 1];
        } catch (e) { }

        var faIcon = "fa-file-text-o";
        if (arrExcelType.indexOf(fileType) !== -1) {
            faIcon = "fa-file-excel-o text-success ";
        }
        else if (arrWordType.indexOf(fileType) !== -1) {
            faIcon = "fa-file-word-o text-primary ";
        }
        else if (arrPowerpoitType.indexOf(fileType) !== -1) {
            faIcon = "fa-file-powerpoint-o text-warning ";
        }
        else if (arrPdfType.indexOf(fileType) !== -1) {
            faIcon = "fa-file-pdf-o text-danger ";
        }
        else if (arrVideoType.indexOf(fileType) !== -1) {
            faIcon = "fa-file-video-o text-info ";
        }
        else if (arrAudioType.indexOf(fileType) !== -1) {
            faIcon = "fa-file-audio-o text-info ";
        }
        else {
            faIcon = "fa-file-text-o";
        }

        return faIcon;
    }

</script>
