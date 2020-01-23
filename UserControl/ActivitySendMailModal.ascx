<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActivitySendMailModal.ascx.cs" Inherits="ServiceWeb.UserControl.ActivitySendMailModal" %>

<%@ Register Src="~/UserControl/AGapeGallery/UploadGallery/UploadGallery.ascx" TagPrefix="link" TagName="UploadGallery" %>
<%@ Register Src="~/AGHtmlEditor/AGHtmlEditorControl.ascx" TagPrefix="link" TagName="AGHtmlEditorControl" %>

<style>
    #modelSendEmail .row {
        padding-top: 0;
    }

    #modelSendEmail .left-addon {
        width: 25px;
        text-align: center;
        display: block;
    }
</style>
<script>
    var enbleAutoBindActivity = true;
    function ActivityManagementMainScript() {
        return false;
    }
    function ActivityManagementMainScriptLoadinboxFlag() {
        var isLoad = true;
        isLoad = true;
        return isLoad;
    }

    function startModelSendEmail() {
        $('#modelSendEmail').modal('show');
        //EmailAotoComplete();
    }
    function EmailAotoComplete() {
        var availableTags = [
            "ActionScript",
            "AppleScript",
            "Asp",
            "BASIC",
            "C",
            "C++",
            "Clojure",
            "COBOL",
            "ColdFusion",
            "Erlang",
            "Fortran",
            "Groovy",
            "Haskell",
            "Java",
            "JavaScript",
            "Lisp",
            "Perl",
            "PHP",
            "Python",
            "Ruby",
            "Scala",
            "Scheme"
        ];
        $(".email-autocomplete-box").autocomplete({
            source: availableTags
        });
    }
</script>
<div class="modal fade" id="modelSendEmail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" style="z-index: 10000"
    aria-hidden="false" data-backdrop="static">
    <div class="modal-dialog" style="width: 95% !important; max-width: 95% !important;">
        <div class="modal-content" id="divModalContent">
            <div class="modal-header">
                <h4 class="modal-title" style="text-align: left;">
                    <asp:Label runat="server" ID="labelSendEmail" Text="&#3626;&#3656;&#3591;&#3629;&#3637;&#3648;&#3617;&#3621;"
                        meta:resourcekey="labelSendEmailResource1"></asp:Label>
                    <span class="pull-right" id="labelTypeSendEmail" style="padding-right: 10px"></span>
                    <asp:HiddenField runat="server" ID="hidTypeSendEmail" ClientIDMode="Static" />
                </h4>
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    &times;</button>
            </div>
            <div class="modal-body">
                <div>
                    <div class="form-row">
                        <div class="form-group col-lg-12">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <label class="d-none">
                                        <asp:RadioButton Text="" runat="server" ID="rdoAssignee" onchange="AGLoading(true);"
                                            GroupName="rdo-custom-mail" OnCheckedChanged="rdoCustomMail_CheckedChanged" AutoPostBack="true" />
                                        &nbsp;                                                                                      
                                        Assignee                                                                                    
                                        &nbsp;&nbsp;&nbsp;                                                                          
                                    </label>
                                    <label class="d-none">
                                        <asp:RadioButton Text="" runat="server" ID="rdoOwner" onchange="AGLoading(true);"
                                            GroupName="rdo-custom-mail" OnCheckedChanged="rdoCustomMail_CheckedChanged" AutoPostBack="true" />
                                        &nbsp;                                                                                      
                                        Owner Service                                                                               
                                        &nbsp;&nbsp;&nbsp;                                                                          
                                    </label>
                                    <label>
                                        <asp:RadioButton Text="" runat="server" ID="rdoContact" onchange="AGLoading(true);"
                                            GroupName="rdo-custom-mail" OnCheckedChanged="rdoCustomMail_CheckedChanged" AutoPostBack="true" />
                                        &nbsp;                                                                                      
                                        Link Down
                                        &nbsp;&nbsp;&nbsp;                                                                          
                                    </label>
                                    <label>
                                        <asp:RadioButton Text="" runat="server" ID="rdoRespondCus" onchange="AGLoading(true);"
                                            GroupName="rdo-custom-mail" OnCheckedChanged="rdoCustomMail_CheckedChanged" AutoPostBack="true" />
                                        &nbsp;                                                                                      
                                        E-mail
                                        &nbsp;&nbsp;&nbsp;                                                                          
                                    </label>
                                    <label>
                                        <asp:RadioButton Text="" runat="server" ID="rdoCustom" onchange="AGLoading(true);" Checked="true"
                                            GroupName="rdo-custom-mail" OnCheckedChanged="rdoCustomMail_CheckedChanged" AutoPostBack="true" />
                                        &nbsp;
                                        Custom
                                        &nbsp;&nbsp;&nbsp;
                                    </label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group  col-lg-12">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpSubject">
                                <ContentTemplate>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <asp:Label runat="server" ID="labelSubjectSendMail"
                                                CssClass="input-group-text"
                                                Text="&#3648;&#3619;&#3639;&#3656;&#3629;&#3591;"
                                                meta:resourcekey="labelSubjectSendMailResource1"></asp:Label>
                                        </div>
                                        <asp:TextBox runat="server" ID="txtSubjectSendMail" ClientIDMode="Static"
                                            CssClass="form-control form-control-sm" meta:resourcekey="txtSubjectSendMailResource1" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpMailPanel">
                        <ContentTemplate>
                            <div class="form-row">
                                <asp:Panel runat="server" CssClass="form-group col-lg-12" ID="pnlFromSendMail">
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <asp:Label runat="server" ID="labelFromSendMail"
                                                CssClass="input-group-text"
                                                Text="&#3650;&#3604;&#3618;" meta:resourcekey="labelFromSendMailResource1"></asp:Label>
                                        </div>
                                        <asp:TextBox runat="server" ID="txtFromSendMail" ReadOnly="True" ClientIDMode="Static"
                                            CssClass="form-control form-control-sm" meta:resourcekey="txtFromSendMailResource1" />
                                    </div>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="form-group col-lg-6" ID="pnlSelectMailContact" Visible="false">
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <asp:Label runat="server" ID="label1" CssClass="input-group-text" Text="Contact"></asp:Label>
                                        </div>
                                        <div class="dropdown" style="width: calc(100% - 77px);">
                                            <button type="button" class="form-control dropdown-toggle" data-toggle="dropdown" style="width: 100%; text-align: right;">
                                            </button>
                                            <div class="dropdown-menu" style="max-height: 400px; overflow: auto; width: 100%;">
                                                <asp:Repeater runat="server" ID="rptLisContact">
                                                    <ItemTemplate>
                                                        <%# Container.ItemIndex > 0 ? "<hr style='margin: 5px 0;' />" : "" %>
                                                        <a class="dropdown-item" href="Javascript:;" onclick="addMailToContainerFromddl('<%# Convert.ToString(Eval("email")) %>');">
                                                            <p>Name : <%# Convert.ToString(Eval("NAME1")) %></p>
                                                            <p>Email : <%# Convert.ToString(Eval("email")) %></p>
                                                            <p>Service : <%# Convert.ToString(Eval("NickName")) %></p>
                                                            <p>Authorize Level : <%# Convert.ToString(Eval("AUTH_CONTACT_NAME")) %></p>
                                                            <p style="margin: 0;">Remark : <%# string.IsNullOrEmpty(Convert.ToString(Eval("Remark"))) ? "" : Convert.ToString(Eval("Remark")).Replace("\n", "<br />") %></p>
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                        <%--<asp:DropDownList runat="server" CssClass="form-control hide" ID="ddlListContact"
                                            onchange="addMailToContainerFromddl(this);">
                                        </asp:DropDownList>--%>
                                    </div>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <div>
                        <style>
                            .box-email > div {
                                margin: auto 3px;
                                outline: 1px solid #ccc;
                                margin-bottom: 15px;
                                min-height: 50px;
                                padding: 3px 10px;
                            }

                            .drag-drop-highlight > div {
                                /*margin-bottom: 5px;*/
                                outline: 2px dashed #009688 !important;
                                /*padding: 10px;*/
                            }

                            .drag-drop-highlight > div {
                                /*min-height: 50px;*/
                            }

                            .ui-draggable {
                                z-index: 100;
                            }
                        </style>
                        <script>
                            $(document).ready(function () {
                                EmailDragDropFunction();
                            });

                            function EmailDragDropFunction() {
                                $(".to-email-list,.cc-email-list").draggable({
                                    drag: function (event, ui) {
                                        onDrag(event, ui, this);
                                    },
                                    stop: function (event, ui) {
                                        onDragEnd(event, ui, $(this).parent());
                                    }
                                });
                                $("#divContainerEmail,#divContainerEmail_CC").droppable({
                                    drop: function (event, ui) {
                                        onDrop(event, ui, this);
                                    }
                                });

                                function onDrag(event, ui, target) {
                                    if (!$("#divEmail,#divEmail_CC").hasClass("drag-drop-highlight")) {
                                        setTimeout(function () {
                                            $("#divEmail,#divEmail_CC").addClass("drag-drop-highlight");
                                        }, 100);
                                    }
                                }

                                function onDragEnd(event, ui, target) {
                                    $("#divEmail,#divEmail_CC").removeClass("drag-drop-highlight");

                                    var items = ui.helper.clone().css({
                                        left: "auto",
                                        top: "auto"
                                    });

                                    items.removeClass("to-email-list");
                                    items.removeClass("cc-email-list");

                                    if ($(target).attr("id") == "divContainerEmail") {
                                        items.find("i.fa.fa-remove").bind("click", function () {
                                            removeToContainerEmail(this);
                                        });
                                        items.addClass("to-email-list");
                                    } else if ($(target).attr("id") == "divContainerEmail_CC") {
                                        items.find("i.fa.fa-remove").bind("click", function () {
                                            removeCCContainerEmail(this);
                                        });
                                        items.addClass("cc-email-list");
                                    }

                                    ui.helper.remove();

                                    $(items).draggable({
                                        drag: function (event, ui) {
                                            onDrag(event, ui, $(this));
                                        },
                                        stop: function (event, ui) {
                                            onDragEnd(event, ui, $(this));
                                        }
                                    });

                                    $(target).append(items);
                                }

                                function onDrop(event, ui, target) {
                                    $("#divEmail,#divEmail_CC").removeClass("drag-drop-highlight");

                                    var items = ui.draggable.clone().css({
                                        left: "auto",
                                        top: "auto"
                                    });

                                    if ($(target).attr("id") == "divContainerEmail") {
                                        var itemMail = items.find('.container-email-cc');
                                        if (itemMail.length == 0) {
                                            itemMail = items.find('.container-email-to');
                                        }
                                        console.log(itemMail);

                                        var mailObject = addMailObjectContainer(
                                            "To: ",
                                            "to-email-list",
                                            "container-email-to",
                                            itemMail.html(),
                                            removeToContainerEmail
                                        );

                                        $("#divContainerEmail").append(mailObject);
                                        EmailDragDropFunction();

                                    } else if ($(target).attr("id") == "divContainerEmail_CC") {
                                        var itemMail = items.find('.container-email-to');
                                        if (itemMail.length == 0) {
                                            itemMail = items.find('.container-email-cc');
                                        }
                                        console.log(itemMail);

                                        var mailObject = addMailObjectContainer(
                                            "CC: ",
                                            "cc-email-list",
                                            "container-email-cc",
                                            itemMail.html(),
                                            removeCCContainerEmail
                                        );

                                        $("#divContainerEmail_CC").append(mailObject);
                                        EmailDragDropFunction();
                                    }

                                    ui.draggable.remove();
                                }
                            }

                            function getEmailFromDescription(idTextDescription) {
                                var arrDesc = $(idTextDescription).val().split('\n');

                                var rowMailFrom = '';
                                var rowMailTo = '';
                                var rowMailCC = '';

                                for (var i = 0; i < arrDesc.length; i++) {
                                    var row = arrDesc[i];
                                    if (rowMailFrom == '' && (row.indexOf("From:") == 0 || row.indexOf("จาก:") == 0)) {
                                        rowMailTo = row;
                                    }

                                    //if (rowMailTo == '' && row.indexOf("To:") == 0) {
                                    //    //rowMailTo = row;
                                    //}

                                    if (rowMailCC == '' && (row.indexOf("Cc:") == 0 | row.indexOf("สำเนา:") == 0)) {
                                        rowMailCC = row;
                                    }
                                }

                                var arrMailFrom = getEmailList(rowMailFrom);
                                var arrMailTo = getEmailList(rowMailTo);
                                var arrMailCC = getEmailList(rowMailCC);

                                for (var i = 0; i < arrMailFrom.length; i++) {
                                    if (arrMailFrom[i].trim() == '') {
                                        continue;
                                    }

                                    var mailObject = addMailObjectContainer(
                                        "To: ",
                                        "to-email-list",
                                        "container-email-to",
                                        arrMailFrom[i],
                                        removeToContainerEmail
                                    );

                                    $("#divContainerEmail").append(mailObject);
                                    EmailDragDropFunction();
                                }
                                for (var i = 0; i < arrMailTo.length; i++) {
                                    if (arrMailTo[i].trim() == '') {
                                        continue;
                                    }

                                    var mailObject = addMailObjectContainer(
                                        "To: ",
                                        "to-email-list",
                                        "container-email-to",
                                        arrMailTo[i],
                                        removeToContainerEmail
                                    );

                                    $("#divContainerEmail").append(mailObject);
                                    EmailDragDropFunction();
                                }
                                for (var i = 0; i < arrMailCC.length; i++) {
                                    if (arrMailCC[i].trim() == '') {
                                        continue;
                                    }

                                    var mailObject = addMailObjectContainer(
                                        "CC: ",
                                        "cc-email-list",
                                        "container-email-cc",
                                        arrMailCC[i],
                                        removeCCContainerEmail
                                    );

                                    $("#divContainerEmail_CC").append(mailObject);
                                    EmailDragDropFunction();
                                }

                                function getEmailList(rowMail) {
                                    var datasMail = []
                                    var arrRowMail = rowMail.split(',');
                                    for (var i = 0; i < arrRowMail.length; i++) {
                                        var textMail = arrRowMail[i];
                                        if (textMail.indexOf('<') >= 0) {
                                            var xStart = textMail.indexOf('<') + 1;
                                            var xEnd = textMail.indexOf('>');
                                            datasMail.push(textMail.substring(xStart, xEnd));
                                        } else {
                                            datasMail.push(textMail.trim());
                                        }

                                    }
                                    return datasMail;
                                }
                            }
                        </script>
                    </div>

                    <!-- Mail To -->
                    <div class="form-row" style="padding-bottom: 0;">
                        <asp:Panel CssClass="form-group  col-lg-12" DefaultButton="btnAddEmail" runat="server" meta:resourcekey="Panel1Resource1">
                            <div class="input-group">
                                <div class="input-group-prepend">
                                    <span class="input-group-text">To</span>
                                </div>
                                <asp:TextBox placeHolder="example@email.com" runat="server" ID="txtAddEmail" ClientIDMode="Static"
                                    CssClass="form-control form-control-sm email-autocomplete-box" />
                                <span class="input-group-append hand" onclick="$('#btnAddEmail').click();">
                                    <i class="fa fa-plus input-group-text"></i>
                                </span>
                            </div>
                            <asp:Button ID="btnAddEmail" Style="display: none" Text="&#3648;&#3614;&#3636;&#3656;&#3617;"
                                Width="65px" CssClass="btn btn-primary" OnClientClick="addMailToContainer(); return false;"
                                ClientIDMode="Static" runat="server" />
                        </asp:Panel>
                    </div>
                    <div id="divEmail" class="box-email">
                        <div id="divContainerEmail" class="form-row">
                        </div>
                    </div>

                    <!-- Mail CC -->
                    <div class="form-row" style="padding-bottom: 0;">
                        <asp:Panel CssClass="form-group  col-lg-12" DefaultButton="btnAddEmail_CC" runat="server">
                            <div class="input-group">
                                <div class="input-group-prepend">
                                    <span class="input-group-text">CC</span>
                                </div>
                                <asp:TextBox placeHolder="example@email.com" runat="server" ID="txtAddEmail_CC" ClientIDMode="Static"
                                    CssClass="form-control form-control-sm email-autocomplete-box" />

                                <span class="input-group-append hand" onclick="$('#btnAddEmail_CC').click();">
                                    <i class="fa fa-plus input-group-text"></i>
                                </span>
                            </div>
                            <asp:Button ID="btnAddEmail_CC" Style="display: none" Text="&#3648;&#3614;&#3636;&#3656;&#3617;"
                                Width="65px" CssClass="btn btn-primary" OnClientClick="addMailCCContainer(); return false;"
                                ClientIDMode="Static" runat="server" />
                        </asp:Panel>
                    </div>
                    <div id="divEmail_CC" class="box-email">
                        <div id="divContainerEmail_CC" class="form-row">
                        </div>
                    </div>

                    <!-- Mail Ref Resource -->
                    <div class="form-row">
                        <div class="form-group  col-lg-12 activity-email-html-editor">
                            <label class="hide">
                                <asp:Label runat="server" ID="labelRemarkSendMail" Text="&#3648;&#3614;&#3636;&#3656;&#3617;&#3648;&#3605;&#3636;&#3617;"
                                    meta:resourcekey="labelRemarkSendMailResource1"></asp:Label>
                            </label>

                            <style>
                                .note-editor note-frame panel panel-default {
                                    border-radius: 0 !important;
                                }

                                .activity-email-ref-source {
                                    border: 1px solid #d7d7d7;
                                    border-bottom: none;
                                    background: #f5f5f5;
                                    padding: 5px;
                                    position: relative;
                                }

                                    .activity-email-ref-source .send-mail-file-badge {
                                        background: #009688;
                                        position: absolute;
                                        right: -10px;
                                        top: -5px;
                                    }

                                        .activity-email-ref-source .send-mail-file-badge[data-number='0'] {
                                            display: none;
                                        }

                                    .activity-email-ref-source .btn {
                                        box-shadow: none;
                                    }

                                .activity-email-ref-source-remark,
                                .activity-email-ref-source-files {
                                    position: absolute;
                                    left: 7px;
                                    top: 37px;
                                    width: 450px;
                                    background: #fff;
                                    overflow-y: auto;
                                    padding: 10px;
                                    z-index: 20000;
                                    display: none;
                                }

                                .activity-email-ref-source-files {
                                    left: 87px;
                                    width: 600px;
                                }

                                .activity-email-ref-source-remark-box,
                                .activity-email-ref-source-files-box {
                                    overflow-y: auto;
                                    max-height: 350px;
                                    min-height: 50px;
                                }

                                .activity-email-ref-source-files table td,
                                .activity-email-ref-source-files table th {
                                    overflow-x: hidden;
                                    white-space: nowrap;
                                    text-overflow: ellipsis;
                                }

                                .activity-email-ref-source-remark .remark-highlight-command-container,
                                .activity-email-ref-source-remark .system-message-mailview-container,
                                .activity-email-ref-source-remark .system-message-comment-container .system-message-attach-files-container,
                                .activity-email-ref-source-remark .system-message-comment-container-reply,
                                .activity-email-ref-source-remark .system-message-comment-container-remark-message .fa-pencil {
                                    display: none !important;
                                }

                                .activity-email-ref-source-remark .system-message-comment-container,
                                .activity-email-ref-source-remark .system-message-comment-container .system-message-comment-container-remark-owner {
                                    background: #fff;
                                }

                                .activity-email-ref-source-remark .system-message-comment-container-remark:hover {
                                    background: #f7f7f7 !important;
                                    cursor: pointer;
                                }

                                .activity-email-ref-source-remark .system-message-comment-container {
                                    padding: 0;
                                    border-left: none;
                                    box-shadow: none;
                                }
                            </style>
                            <script>
                                function sendCustomEmail(subject, myName) {
                                    startModelSendEmail();

                                    $('#txtSubjectSendMail').val(subject);
                                    //$("#divContainerEmail").html("");
                                    $("#txtAddEmail").val("");

                                    $("#txtRemarkSendMail").val("");
                                    $('#txtFromSendMail').val(myName);
                                    $("#labelTypeSendEmail").html("Custom");
                                    $("#hidTypeSendEmail").val("CUSTOM");

                                    // Prepare value
                                    //sendMailPrepareAllCode();
                                }

                                function openEmailActivityRefRemark() {
                                    closeEmailActivityRefFiles();
                                    var container = $(".activity-email-html-editor .activity-email-ref-source-remark .activity-email-ref-source-remark-box");
                                    container.html("");
                                    container.closest(".activity-email-ref-source-remark").show();
                                    container.AGWhiteLoading(true, "Loading remark");
                                    loadActivityDetailRemarkComment(
                                        $(".txtHiddenAObjectlink_MAIL").val(),
                                        container,
                                        function (remarkData) {

                                            var div = $("<div/>", {
                                                class: "activity-email-html-editor-table-remark",
                                                css: {
                                                    padding: 10,
                                                    background: "#f7f7f7",
                                                    border: "1px solid #ddd"
                                                }
                                            });

                                            var table = $("<table/>", {
                                                css: {
                                                    width: "100%",
                                                    tableLayout: "fixed"
                                                }
                                            });
                                            table.appendTo(div);

                                            // row 1
                                            var tr = $("<tr/>");
                                            tr.appendTo(table);
                                            var td = $("<th/>", {
                                                html: remarkData.CreatorFullname,
                                                css: {
                                                    textAlign: "left"
                                                }
                                            });
                                            td.appendTo(tr);

                                            td = $("<td/>", {
                                                html: remarkData.CreatedOn,
                                                css: {
                                                    color: "#aaa",
                                                    textAlign: "right"
                                                }
                                            });
                                            td.appendTo(tr);

                                            // row 2
                                            tr = $("<tr/>");
                                            tr.appendTo(table);
                                            td = $("<td/>", {
                                                html: remarkData.MessageText.split('\n').join('<br>'),
                                                colspan: 2
                                            });
                                            td.appendTo(tr);

                                            var editorContainer = $(".activity-email-html-editor .note-editable");
                                            if (editorContainer.find(".activity-email-html-editor-table-remark").length > 0) {
                                                editorContainer.find(".activity-email-html-editor-table-remark:last")
                                                    .after("<br>", div);
                                            }
                                            else {
                                                editorContainer.prepend("<br>", div, "<br>");
                                            }
                                            editorContainer.focus();

                                            closeEmailActivityRefRemark();
                                        },
                                        function () {
                                            container.AGWhiteLoading(false);
                                            if (container.find(".system-message-comment-container-remark:visible").length == 0) {
                                                container.append($("<div/>", {
                                                    class: "text-center",
                                                    html: "ไม่พบความคิดเห็นที่อ้างอิงได้",
                                                    css: {
                                                        padding: 10,
                                                        border: "1px solid #ccc"
                                                    }
                                                }));
                                            }
                                        }
                                    );

                                    $(document).click(function () {
                                        closeEmailActivityRefRemark();
                                    });
                                }
                                function closeEmailActivityRefRemark() {
                                    $(".activity-email-html-editor .activity-email-ref-source-remark").hide();
                                }
                                function openEmailActivityRefFiles() {
                                    closeEmailActivityRefRemark();
                                    var container = $(".activity-email-html-editor .activity-email-ref-source-files .activity-email-ref-source-files-box");
                                    container.closest(".activity-email-ref-source-files").show();

                                    if (container.find("table").length == 0) {
                                        container.AGWhiteLoading(true, "Loading files");

                                        var files = [];
                                        $("#pop_attachfile .activity-attachment-link").each(function () {
                                            files.push({
                                                url: $(this).attr("href").split('\\').join('/'),
                                                name: $(this).attr("data-file-name"),
                                                uploader: $(this).attr("data-uploader"),
                                                source: "ACTIVITY"
                                            });
                                        });
                                        $("#remark-table .system-message-attach-files-container").each(function () {
                                            var uploader = $(this).closest(".system-message-comment-container-remark")
                                                .find(".system-message-comment-container-remark-fullname").html();

                                            $(this).find(".system-message-attach-files").each(function () {
                                                var img = $(this).find(".system-message-attach-image-block");
                                                if (img.length > 0) {
                                                    var imgUrl = img.attr("data-image");
                                                    files.push({
                                                        url: imgUrl,
                                                        name: imgUrl.split('/')[imgUrl.split('/').length - 1],
                                                        uploader: uploader,
                                                        source: "REMARK"
                                                    });
                                                }

                                                var otherFile = $(this).find(".system-message-attach-file-block");
                                                if (otherFile.length > 0) {
                                                    files.push({
                                                        url: $(this).attr("data-file-url"),
                                                        name: $(this).attr("data-name"),
                                                        uploader: uploader,
                                                        source: "REMARK"
                                                    });
                                                }
                                            });
                                        });



                                        var table = $("<table/>", {
                                            class: "table table-striped table-hover",
                                            css: {
                                                tableLayout: "fixed"
                                            }
                                        });
                                        table.appendTo(container);

                                        // row header
                                        var tr = $("<tr/>");
                                        tr.appendTo(table);

                                        var td = $("<th/>", {
                                            css: {
                                                width: 20
                                            }
                                        });
                                        td.appendTo(tr);

                                        var td = $("<th/>", {
                                            html: "Filename",
                                        });
                                        td.appendTo(tr);

                                        var td = $("<th/>", {
                                            html: "Upload by",
                                            css: {
                                                width: 120
                                            }
                                        });
                                        td.appendTo(tr);

                                        var td = $("<th/>", {
                                            class: "text-center",
                                            html: "<i class='fa fa-download'></i>",
                                            css: {
                                                width: 30,
                                                paddingTop: 12
                                            }
                                        });
                                        td.appendTo(tr);

                                        if (files.length == 0) {
                                            var tr = $("<tr/>");
                                            tr.appendTo(table);

                                            var td = $("<td/>", {
                                                class: "text-center",
                                                colspan: 4,
                                                html: "ไม่พบไฟล์ใดๆในกิจกรรมนี้"
                                            });
                                            td.appendTo(tr);
                                        }

                                        for (var i = 0; i < files.length; i++) {
                                            var guid = generateGUID();
                                            var file = files[i];
                                            file.guid = guid;

                                            var tr = $("<tr/>", {
                                                title: file.name + " [Upload by : " + file.uploader + "]",
                                                css: {
                                                    cursor: "pointer"
                                                },
                                                click: function () {
                                                    $(this).find(":checkbox").click();
                                                }
                                            });
                                            tr.appendTo(table);

                                            var td = $("<td/>", {
                                                class: "text-center"
                                            });
                                            td.appendTo(tr);

                                            var checkbox = $("<input />", {
                                                type: "checkbox"
                                            });
                                            checkbox.appendTo(td);
                                            checkbox.bind("click", {
                                                file: file
                                            }, function (e) {
                                                toggleEmailActivityRefFile(e.data.file);
                                                e.stopPropagation();
                                            })

                                            var td = $("<td/>", {
                                                html: file.name
                                            });
                                            td.appendTo(tr);

                                            var td = $("<td/>", {
                                                html: file.uploader
                                            });
                                            td.appendTo(tr);

                                            var td = $("<td/>", {
                                                class: "text-center",
                                                css: {
                                                    paddingTop: 12
                                                }
                                            });
                                            td.appendTo(tr);

                                            var preview = $("<a/>", {
                                                target: "_blank",
                                                href: "javascript:;",
                                                "data-path": file.url,
                                                "data-name": file.name,
                                                html: "<i class='fa fa-download'></i>"
                                            });
                                            preview.bind("click", {
                                                file: file
                                            }, function (e) {
                                                ActivityDownloadFileForm(this);
                                                e.stopPropagation();
                                            });

                                            preview.appendTo(td);

                                        }
                                        container.AGWhiteLoading(false);

                                        $(document).click(function () {
                                            closeEmailActivityRefFiles();
                                        });
                                    }
                                }
                                function closeEmailActivityRefFiles() {
                                    $(".activity-email-html-editor .activity-email-ref-source-files").hide();
                                }
                                function toggleEmailActivityRefFile(data) {
                                    var fileSelected = $("#txtAllSendMailRefFiles").val();
                                    if (fileSelected == "") {
                                        fileSelected = [];
                                    }
                                    else {
                                        fileSelected = $.parseJSON(fileSelected);
                                    }

                                    var guid = data.guid;
                                    function fileSelectedFilter(value) {
                                        return value.guid == guid;
                                    }
                                    var getSelected = fileSelected.filter(fileSelectedFilter);
                                    if (getSelected.length == 0) {
                                        fileSelected.push(data);
                                    } else {
                                        function removeFileSelectedFilter(value) {
                                            return value.guid != guid;
                                        }
                                        fileSelected = fileSelected.filter(removeFileSelectedFilter);
                                    }
                                    $(".send-mail-file-badge").html(fileSelected.length).attr("data-number", fileSelected.length);
                                    $("#txtAllSendMailRefFiles").val(JSON.stringify(fileSelected));
                                }
                            </script>

                            <div class="activity-email-ref-source d-none">
                                <div class="activity-email-ref-source-remark z-depth-2" onclick="event.stopPropagation();">
                                    <label>เพิ่มความคิดเห็น (คลิ๊กที่ความคิดเห็นเพื่อเพิ่ม)</label>
                                    <div class="activity-email-ref-source-remark-box system-message-comment-container"></div>
                                </div>

                                <div class="activity-email-ref-source-files z-depth-2" onclick="event.stopPropagation();">
                                    <label>เลือกไฟล์แนบจากรายการ</label>
                                    <div style="margin-top: 10px;" class="activity-email-ref-source-files-box"></div>
                                </div>

                                <span class="btn btn-default btn-sm" onclick="openEmailActivityRefRemark();event.stopPropagation();">
                                    <i class="fa fa-comments"></i>
                                    Remark
                                </span>
                                <span style="position: relative" class="btn btn-default btn-sm" onclick="openEmailActivityRefFiles();event.stopPropagation();">
                                    <i class="fa fa-file-o"></i>
                                    Attachment
                                    <span class="badge send-mail-file-badge" data-number="0">0</span>
                                </span>
                            </div>
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpHtmlEditer">
                                <ContentTemplate>
                                    <link:aghtmleditorcontrol id="txtRemarkSendMail" runat="server" hideParamButton="true"></link:aghtmleditorcontrol>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group  col-lg-6 ">
                            <div class="d-none">
                                <link:uploadgallery id="uploadGallery" multiplemode="true" previewheight="50px" previewwidth="50px" runat="server"></link:uploadgallery>
                            </div>
                        </div>
                        <div class="form-group  col-lg-6 text-right" style="padding-top: 10px;">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpDataConf">
                                <ContentTemplate>
                                    <div class="d-none">
                                        <asp:TextBox runat="server" ID="txtHiddenAObjectlink_MAIL" CssClass="txtHiddenAObjectlink_MAIL" />
                                        <asp:TextBox runat="server" ID="txtHiddenTicketNo_MAIL" CssClass="txtHiddenTicketNo_MAIL" />
                                        <asp:TextBox runat="server" ID="txtHiddenCompanyCode_MAIL" CssClass="txtHiddenCompanyCode_MAIL" />
                                        <asp:TextBox runat="server" ID="txtHiddenRowkey_MAIL" CssClass="txtHiddenRowkey_MAIL" />

                                        <asp:TextBox runat="server" ClientIDMode="Static" ID="txtAllSendMailRefFiles" />
                                        <asp:TextBox runat="server" ClientIDMode="Static" ID="txtAllSendMailContainer" />
                                        <asp:TextBox runat="server" ClientIDMode="Static" ID="txtAllSendMailContainer_CC" />
                                    </div>

                                    <asp:Button ID="btnSendMailCustom" OnClick="btnSendMailCustom_Click" Text="&#3626;&#3656;&#3591;"
                                        runat="server" Width="100px" CssClass="btn btn-success" OnClientClick="return prepareSendmail();"
                                        meta:resourcekey="btnSendMailCustomResource1" />
                                    <input type="button" name="name" value="&#3618;&#3585;&#3648;&#3621;&#3636;&#3585;"
                                        style="width: 100px;" onclick="$('#modelSendEmail').modal('hide');" class="btn btn-danger" />

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="d-none">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpDatasEmail">
                        <ContentTemplate>
                            <asp:TextBox runat="server" TextMode="MultiLine" ID="txtEmailOwner" />

                            <asp:TextBox runat="server" TextMode="MultiLine" ID="txtEmailMainAssignee" />
                            <asp:TextBox runat="server" TextMode="MultiLine" ID="txtEmailOtherAssignee" />

                            <asp:TextBox runat="server" TextMode="MultiLine" ID="txtEmailCustomer" />
                            <asp:TextBox runat="server" TextMode="MultiLine" ID="txtEmailContact" />

                            <asp:TextBox runat="server" TextMode="MultiLine" ID="txtCustomerCode" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</div>

<script>

    function addMailObjectContainer(prefix, containerClass, mailClass, value, removeFunction) {
        var container = $("<div/>", {
            class: "col-md-4 form-group " + containerClass
        });
        var box = $("<div/>", {
            class: "one-line",
            css: {
                padding: 5,
                paddingRight: 20,
                paddingLeft: 15,
                border: "1px solid #ccc",
                marginTop: 5,
                borderRadius: 5,
                background: "#f7f7f7"
            },
            title: value,
            html: prefix
        });
        box.appendTo(container);
        var mailBox = $("<span/>", {
            class: mailClass,
            html: value
        });
        mailBox.appendTo(box);

        var remove = $("<i/>", {
            class: "fa fa-remove",
            css: {
                cursor: "pointer",
                position: "absolute",
                right: 15,
                top: 14
            },
            click: function () {
                removeFunction(this);
            }
        });
        remove.appendTo(box);

        return container;
    }


    function addMailToContainerFromddl(email) {
        if (email.trim() != '') {
            var datas = email.split(',')

            for (var i = 0; i < datas.length; i++) {
                if (datas[i].trim() == '') {
                    continue;
                }
                var mailObject = addMailObjectContainer(
                    "To: ",
                    "to-email-list",
                    "container-email-to",
                    datas[i].trim(),
                    removeToContainerEmail
                );

                $("#divContainerEmail").append(mailObject);
                EmailDragDropFunction();
            }
        }
    }

    function addMailToContainer() {
        if ($("#txtAddEmail").val().trim() != '') {
            $("#divEmail").show();

            var mailObject = addMailObjectContainer(
                "To: ",
                "to-email-list",
                "container-email-to",
                $("#txtAddEmail").val(),
                removeToContainerEmail
            );

            $("#divContainerEmail").append(mailObject);
            EmailDragDropFunction();
        }
        $("#txtAddEmail").val("");
    }

    function addMailCCContainer() {
        if ($("#txtAddEmail_CC").val().trim() != '') {
            $("#divEmail_CC").show();

            var mailObject = addMailObjectContainer(
                "CC: ",
                "cc-email-list",
                "container-email-cc",
                $("#txtAddEmail_CC").val(),
                removeCCContainerEmail
            );

            $("#divContainerEmail_CC").append(mailObject);
            EmailDragDropFunction();
        }
        $("#txtAddEmail_CC").val("");
    }

    function setDefaultMailTo(arrMailTo) {
        $("#divContainerEmail").html('');
        for (var i = 0; i < arrMailTo.length; i++) {
            var mailTo = arrMailTo[i].trim();
            if (mailTo != "") {
                var mailObject = addMailObjectContainer(
                    "To: ",
                    "to-email-list",
                    "container-email-to",
                    mailTo,
                    removeToContainerEmail
                );
                $("#divContainerEmail").append(mailObject);
                EmailDragDropFunction();
            }
        }
    }

    function setDefaultMailCC(arrMailCC) {
        $("#divContainerEmail_CC").html('');
        for (var i = 0; i < arrMailCC.length; i++) {
            var mailCC = arrMailCC[i].trim();
            if (mailCC != "") {
                var mailObject = addMailObjectContainer(
                    "CC: ",
                    "cc-email-list",
                    "container-email-cc",
                    mailCC,
                    removeCCContainerEmail
                );

                $("#divContainerEmail_CC").append(mailObject);
                EmailDragDropFunction();
            }
        }
    }

    function removeToContainerEmail(obj) {
        $(obj).closest(".to-email-list").remove();
        if ($(".container-email-to").length == 0) {
            //$("#divEmail").hide();
        }
    }

    function removeCCContainerEmail(obj) {
        $(obj).closest(".cc-email-list").remove();
        if ($(".container-email-cc").length == 0) {
            //$("#divEmail_CC").hide();
        }
    }

    function prepareSendmailList() {
        var mail = [];
        $(".container-email-to").each(function () {
            mail.push($(this).html());
        });
        $("#txtAllSendMailContainer").val(mail);

        var cc = [];
        $(".container-email-cc").each(function () {
            cc.push($(this).html());
        });
        $("#txtAllSendMailContainer_CC").val(cc);

        return {
            to: mail.join(';'),
            cc: cc.join(';')
        }
    }

    function prepareSendmail() {
        if ($(".container-email-to").length == 0) {
            AGMessage("กรุณาเพิ่มอีเมลที่คุณจะส่งถึง");
            return false;
        }
        if (confirm("ยืนยันจะส่งอีเมลนี้หรือไม่")) {
            prepareSendmailList();
            AGLoading(true);
        }
        else
            return false;

    }

    function successSendCustomEmail() {
        isSendMailEvent = true;
        AGMessage("ส่งอีเมลเรียบร้อยแล้ว");
        $('#modelSendEmail').modal('hide');
    }

    function loadActivityDetailServerCall(aObjectLink) {
        if ($(".btn-OpenChatBox-").is(":visible")) {
            $(".btn-OpenChatBox-" + aObjectLink).click();
        } else {
            $(".btn-CloseChatBox-" + aObjectLink).click();
            setTimeout(function () { $(".btn-OpenChatBox-" + aObjectLink).click(); }, 200);
        }
    }

    function updateStatusAutoBySendMail(statusCode) {
        $("#ddl_ticket_doc_status_remark").val(statusCode);
        $("#ddl_ticket_doc_status_remark").change();
        setTimeout(function () {
            $("#btnUpdateStatus_FormPostRemark").click();
        }, 100);
    }
</script>
