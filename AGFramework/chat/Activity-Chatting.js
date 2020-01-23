var _EMAIL_INCOMING = "EMAIL_INCOMING";
var _EMAIL_OUTGOING = "EMAIL_OUTGOING";

var onReplySuccessWithCreateActivity;
var onCallPostQuoteMessage;
var onReplySuccessWithAnswerOnlineForm;
var postDataWithCreateActivity;
var activityRemarkReplyBoxAttachGlobalFiles = [];
var activityRemarkEditModeAttachGlobalFiles = [];
var activityRemarkReplyBoxAttachGlobalFilesUploadType = "FILE";
var currentMousePos = { x: -1, y: -1 };
var afterLoadMoerFocusReply = undefined;


$(document).mousemove(function (event) {
    currentMousePos.x = event.pageX;
    currentMousePos.y = event.pageY;
});

function removeHighlightFunction() {
    $(".remark-highlight-command-shortcut,.remark-highlight-command-container").remove();
}

function clearHightFunctionSelect() {
    if (window.getSelection) {
        if (window.getSelection().empty) {  // Chrome
            window.getSelection().empty();
        } else if (window.getSelection().removeAllRanges) {  // Firefox
            window.getSelection().removeAllRanges();
        }
    } else if (document.selection) {  // IE?
        document.selection.empty();
    }
}

function GetMessageTypeIcon(messageType) {
    if (messageType == "WIP") {
        return '<input type="button" class="btn  btn-warning  btn-sm btn-wip ticket-allow-editor ticket-allow-editor-everyone" value="WIP" style="zoom: 0.85;width:50px;margin-right:10px;margin-bottom:10px;" />';
    }
    else if (messageType == "CHECK_IN") {
        return '<input type="button" class="btn  btn-sm btn-checkin ticket-allow-editor ticket-allow-editor-everyone" value="Check-In" style="width:65px;margin-right:10px;margin-bottom:10px;" />';
    }
    else if (messageType == "CHECK_OUT") {
        return '<input type="button" class="btn  btn-sm btn-checkin ticket-allow-editor ticket-allow-editor-everyone" value="Check-Out" style="width:65px;margin-right:10px;margin-bottom:10px;" />';
    }
    else if (messageType == "Highlight") {
        return '<input type="button" class="btn btn-warning btn-sm btn-highlight ticket-allow-editor ticket-allow-editor-everyone" value="Highlight" style="zoom: 0.85;width:75px;margin-right:10px;margin-bottom:10px;" />';
    }

    return "";
}

function WIPMessage(isWip) {
    if (isWip) {
        return GetMessageTypeIcon("WIP");
    }
    return "";
}

function generateGUID() {
    var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx-'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
    return guid;
}

function generateShortGUID() {
    var guid = 'xxxxxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
    return guid;
}

function RemarkMessageTextBuilder(message, type) {
    if (message == null || message == undefined) {
        return "";
    }


    var rest = $("<div/>", {
        html: GetMessageTypeIcon(type)
    });
    var allowEdit = true;

    var messagePlain = "";
    var messageRef = "";
    var strRef = message.split("[!!--");
    if (strRef.length > 1) {
        messagePlain = strRef[0];
        messageRef = strRef[1].split("--!!]").join("");
        message = messageRef;
    }

    var strArr = message.split("[!--");




    if (strArr.length > 1) {
        var strLeft = strArr[0];
        var strRight = strArr[1].split("--!]").join("");

        rest.append("<p class='remark-text-container'></p>");

        var linkTo = $("<a/>", {
            href: "javascript:;",
            html: "<i class='fa fa-external-link-square' style='margin-right:10px'></i>" + strLeft
        });
        linkTo.bind("click", {
            aobj: strRight,
            snaid: strRight.substring(0, 7),
            subject: strLeft
        }, function (e) {
            openActivityDetailModelFrame(e.data.aobj, e.data.subject);
        });
        rest.append(linkTo);

        allowEdit = false;
    }
    else {
        rest.append("<span class='remark-text-container'>" + convertToHTMLText(message) + "</span>");
    }

    if (messagePlain != undefined && messagePlain != null && messagePlain.trim() != "") {
        rest.prepend(convertToHTMLText(messagePlain));
    }

    return {
        element: rest,
        allowEdit: allowEdit,
        message: rest.html()
    };
}

function ajaxPostUploadFiles(event, datas) {
    var finalPostData = datas.finalPostData;
    var panelReply = datas.panelReply;
    var onReplySuccess = datas.onReplySuccess;
    var guid = datas.guid;
    var aobjectlink = datas.aobjectlink;
    var callBackPostSuccess = datas.callBackPostSuccess;

    $(".system-message-comment-container-reply-file-container").AGWhiteLoading(true, "Uploading files");
    //event.stopPropagation(); // Stop stuff happening
    //event.preventDefault(); // Totally stop stuff happening           

    setTimeout(function () {
        // Create a formdata object and add the files
        var data = new FormData();
        var files = activityRemarkReplyBoxAttachGlobalFiles;
        if (files.length > 0) {
            for (var i = 0; i < files.length; i++) {
                data.append('UploadedFiles', files[i].file, files[i].file.name);
            }
        }
        var remark = finalPostData.key.remarkMessage
        remark = remark.replace(new RegExp('<', 'g'), "&lt;");
        remark = remark.replace(new RegExp('>', 'g'), "&gt;");
        data.append("uploadType", activityRemarkReplyBoxAttachGlobalFilesUploadType);
        data.append("message", remark);
        data.append("aobj", aobjectlink);
        activityRemarkReplyBoxAttachGlobalFiles = [];

        $.ajax({
            url: servictWebDomainName + "widget/AJAXFileUploadAPI.aspx",
            type: "POST",
            data: data,
            //async: false,
            success: function (msg) {
                activityRemarkReplyBoxAttachGlobalFiles = [];
                $(".system-message-comment-container-reply-file-container").AGWhiteLoading(false);
                $(".system-message-comment-container-reply-file-container").html("").hide();
                $(".reply-attach").show();
                ajaxPostDataRemark({
                    finalPostData: finalPostData,
                    panelReply: panelReply,
                    onReplySuccess: onReplySuccess,
                    callBackPostSuccess: callBackPostSuccess,
                    guid: guid,
                    attachFileKey: msg,
                    files: files
                });
            },
            cache: false,
            contentType: false,
            processData: false
        });
    }, 1000);
}

function ajaxPostDataRemark(datas) {
    var finalPostData = datas.finalPostData;
    var panelReply = datas.panelReply == undefined ? $("body") : datas.panelReply;
    var onReplySuccess = datas.onReplySuccess;
    var callBackPostSuccess = datas.callBackPostSuccess;
    var guid = datas.guid;
    var attachFileKey = datas.attachFileKey == undefined ? "" : datas.attachFileKey;
    var files = datas.files;
    
    finalPostData.key.attachFileKey = attachFileKey;
    finalPostData.key.remarkMessage = finalPostData.key.remarkMessage.split('<').join('&lt;').split('>').join('&gt;');
    finalPostData.key.quoteMessage = finalPostData.key.quoteMessage.split('<').join('&lt;').split('>').join('&gt;');

    if (onCallPostQuoteMessage && typeof (onCallPostQuoteMessage) === "function") {
        finalPostData.key.isQuote = true;

        var QuoteDatas = onCallPostQuoteMessage();

        if (QuoteDatas == undefined) {
            return;
        }

        finalPostData.key.quoteType = QuoteDatas.quoteType == undefined ? "CUSTOM" : QuoteDatas.quoteType;
        finalPostData.key.quoteMessage = QuoteDatas.quoteMessage == undefined ? "" : QuoteDatas.quoteMessage;
        onCallPostQuoteMessage = undefined;
    }

    $.ajax({
        url: finalPostData.url,
        type: "POST",
        data: finalPostData.key,
        success: function (datas) {
            panelReply.AGWhiteLoading(false);
            if (datas.message == "S") {
                if (onReplySuccess != false && typeof (onReplySuccess) == "function") {
                    onReplySuccess({
                        postingGUID: guid,
                        aobjectlink: finalPostData.key.aobj
                    });
                }

                if (callBackPostSuccess && typeof (callBackPostSuccess) === "function") {
                    callBackPostSuccess(finalPostData.key.remarkMessage, files, finalPostData);
                }

                $(".ddl-option-type-remark").val("REMARK");
            }
            else {
                alert("Cannot post remark " + datas.message);
            }
        },
        error: function () {
            panelReply.AGWhiteLoading(false);
            alert("Can not port remark");
        }
    });
}

function highlightSuccessCreateActivity(guid, aobj, subject) {
    var refMessage = $("#highlight-tab-panel-content-activity-from-control" + guid).val();
    //var elt = $("#create-activity-frame-" + guid);
    //var quoteElt = elt.closest(".remark-highlight-command-box");
    var messageQuote = $(".remark-highlight-command-box-quote-message:visible").html();

    var onReplySuccess = onReplySuccessWithCreateActivity;
    var _PostData = postDataWithCreateActivity;


    var finalMessage = refMessage + " [!!--" + subject + " [!--" + aobj + "--!]--!!]";
    _PostData.key.isQuote = true;
    _PostData.key.quoteMessage = convertToSystemText(messageQuote);
    _PostData.key.quoteType = "ACTIVITY";
    _PostData.key.remarkMessage = finalMessage;
    _PostData.key.remarkType = "REMARK";
    _PostData.key.sendMail = false;


    ajaxPostDataRemark({
        finalPostData: _PostData,
        onReplySuccess: onReplySuccess,
        callBackPostSuccess: callBackPostSuccess
    });

    if (window.parent > 0) {
        window.parent.closeIfraneCreateActivity();
    } else {
        closeIfraneCreateActivity();
    }
    removeHighlightFunction();
}

function highlightCloseCreateActivity(guid) {
    if (window.parent > 0) {
        window.parent.closeIfraneCreateActivity();
    } else {
        closeIfraneCreateActivity();
    }
    removeHighlightFunction();
}

function highlightShowCreateActivity(guid) {
    var quoteElt = $(".remark-highlight-command-box:visible");
    var messageQuote = quoteElt.find(".remark-highlight-command-box-quote-message").html();
    return convertToSystemText(messageQuote);
}

function highlightShowCreateActivityGetDocnumber(guid) {
    var quoteElt = $(".remark-highlight-command-box:visible");
    var result = quoteElt.find(".remark-highlight-command-box-quote-refdoc").html();

    return result;
}

function convertDangerSignToDisplay(rest) {
    rest = rest.split('&lt;').join('<');
    rest = rest.split('&gt;').join('>');
    return rest;
}

function convertDangerSignToSystem(rest) {
    rest = rest.split('<').join('&lt;');
    rest = rest.split('>').join('&gt;');
    return rest;
}

function htmlDecodeWithLineBreaks(html) {
    var breakToken = '_______break_______',
        lineBreakedHtml = html.replace(/<br\s?\/?>/gi, breakToken).replace(/<p\.*?>(.*?)<\/p>/gi, breakToken + '$1' + breakToken);
    return $('<div>').html(lineBreakedHtml).text().replace(new RegExp(breakToken, 'g'), '\n');
}

function convertToSystemText(input) {
    var rest = input;
    rest = htmlDecodeWithLineBreaks(rest);

    rest = rest.split("<br>").join("\n")
    rest = rest.split("<br/>").join("\n");
    rest = rest.split("'").join("\'");
    rest = rest.split('"').join('\"');
    rest = convertDangerSignToSystem(rest);

    return rest;
}

function convertToTextboxText(input) {
    var rest = input;

    rest = rest.split("<br>").join("\n")
    rest = rest.split("<br/>").join("\n");
    rest = rest.split("'").join("\'");
    rest = rest.split('"').join('\"');
    rest = convertDangerSignToDisplay(rest);

    var div = $("<div/>", {
        html: rest,
        css: {
            display: "none"
        }
    });

    div.find("a").each(function () {
        $(this).before($(this).text());
        $(this).remove();
    });
    rest = div.html();

    return rest;
}

function convertToHTMLText(input) {
    var rest = input;
    rest = rest.split("\n").join("<br>");

    try {
        rest = linkify(rest);
    }
    catch (e) {
        rest = rest;
    }

    return rest.trim();
}

function ActivityDownloadFileForm(obj) {
    var path = $(obj).attr("data-path");
    var name = $(obj).attr("data-name");
    var downloadForm = $("<form method='post' action='/widget/DownloadFileForm.aspx' target='_blank' />");
    var fileName = $("<input type='hidden' name='FILENAME' value='" + name + "'/>");
    var filePath = $("<input type='hidden' name='FILEPATH' value='" + path + "'/>");
    downloadForm.appendTo($('body'));
    downloadForm.append(fileName, filePath);
    downloadForm.submit();
    downloadForm.remove();
}

$.fn.activityRemark = function (datas) {
    activityRemarkReplyBoxAttachGlobalFiles = [];
    var allowDivider = datas.allowDivider == undefined ? true : datas.allowDivider;
    var allowHighlight = datas.allowHighlight == undefined ? true : datas.allowHighlight;
    var isContinue = datas.isContinue == undefined ? false : datas.isContinue;
    var onContinue = datas.onContinue;
    var onAfterReply = datas.onAfterReply;
    var callbackData = datas.callbackData;
    var onRendered = datas.onRendered;
    var getData = datas.getData;
    var postData = datas.postData;
    var editData = datas.editData;
    var getOnlineForm = datas.getOnlineForm;
    var getRefEmailContent = datas.getRefEmailContent;
    var callBackRefEmailContent = datas.callBackRefEmailContent;
    var aobjectlink = datas.aobjectlink;
    var pagemode = datas.pagemode;
    var islazyLoad = datas.islazyLoad == undefined ? false : datas.islazyLoad;
    var isEasyMode = datas.isEasyMode == undefined ? false : datas.isEasyMode;
    var loadMoreSeq = datas.loadMoreSeq == undefined ? "" : datas.loadMoreSeq;
    var isLoadNewMessage = datas.isLoadNewMessage == undefined ? false : datas.isLoadNewMessage;
    var isLoadMore = datas.isLoadMore == undefined ? false : datas.isLoadMore;
    var lazyLoadTotal = datas.lazyLoadTotal == undefined ? 0 : parseInt(datas.lazyLoadTotal);
    var myImagePath = datas.myImagePath == undefined ? "" : datas.myImagePath;
    var callbackPickRemark = datas.callbackPickRemark;
    var callBackPostSuccess = datas.callBackPostSuccess;
    var callBackInitSuccess = datas.callBackInitSuccess;
    

    getData.key.seq = loadMoreSeq;
    getData.key.isLoadNewMessage = isLoadNewMessage;


    var container = $(this);
    $.ajax({
        url: getData.url,
        data: getData.key,
        success: function (data) {

            if (data.TotalRemark != undefined) {
                lazyLoadTotal = data.TotalRemark;
                data = data.Remarks;
                islazyLoad = true;
            }

            container.show();
            container.activityRemarkBuilder({
                isLoadMore: isLoadMore,
                isLoadNewMessage: isLoadNewMessage,
                data: data,                
                allowDivider: allowDivider,
                allowHighlight: allowHighlight,
                postData: postData,
                editData: editData,
                getOnlineForm: getOnlineForm,
                getRefEmailContent: getRefEmailContent,
                callBackRefEmailContent: callBackRefEmailContent,
                callbackPickRemark: callbackPickRemark,
                callBackPostSuccess: callBackPostSuccess,
                callBackInitSuccess: callBackInitSuccess,
                aobjectlink: aobjectlink,
                pagemode: pagemode,
                islazyLoad: islazyLoad,
                isEasyMode: isEasyMode,
                myImagePath: myImagePath,
                lazyLoadTotal: lazyLoadTotal,
                onReplySuccess: function (result) {
                    var loadNewBtn = $(".new-message-trigger[data-new-message-trigger-aobjectlink='" + result.aobjectlink + "']");
                    loadNewBtn.addClass("trigger-post");
                    loadNewBtn.click();
                    $("#" + result.postingGUID).remove();

                    if (onAfterReply != undefined && typeof (onAfterReply) == "function") {
                        onAfterReply(result);
                    }
                },
                onLoadMore: function (curSeq, isLoadNewMessage) {
                    container.activityRemark({
                        allowDivider: allowDivider,
                        allowHighlight: allowHighlight,
                        getData: getData,
                        postData: postData,
                        getRefEmailContent: getRefEmailContent,
                        callBackRefEmailContent: callBackRefEmailContent,
                        callbackPickRemark: callbackPickRemark,
                        editData: editData,
                        aobjectlink: aobjectlink,
                        pagemode: pagemode,
                        loadMoreSeq: curSeq,
                        isLoadNewMessage: isLoadNewMessage,
                        isLoadMore: !isLoadNewMessage,
                        islazyLoad: true,
                        lazyLoadTotal: lazyLoadTotal
                    });
                }
            });
            if (isContinue) {
                if (onContinue != undefined && typeof (onContinue) == "function") {
                    onContinue();
                }
            }
            if (onRendered != undefined && typeof (onRendered) == "function") {
                onRendered();
            }
            if (callbackData != undefined) {
                $("#" + callbackData.postingGUID).remove();
            }
        }
    });
}

$.fn.loadReplyComment = function (aobjectlink, onLoadMore) {
    if (typeof (afterLoadMoerFocusReply) == "function") {
        afterLoadMoerFocusReply();
    }

    var container = $(this);
    var url = servictWebDomainName + "framework/ag-activity-remark/api/";
    var data = {
        q: "get-reply",
        obj: aobjectlink
    };
    $.ajax({
        url: url,
        data: data,
        success: function (datas) {
            console.log(datas);

            if (datas.TotalRemark > 0) {
                
                var panel = $("<div>", {
                    class: "text-info"
                }).css({
                    float: "right",
                    marginTop: "-45px",
                    cursor: "pointer"
                });

                var div = $("<div>", {
                    class: "btn-group dropleft"
                });

                var btn = $("<button>", {
                    type: "button",
                    class: "btn btn-warning btn-sm dropdown-toggle ticket-allow-editor ticket-allow-editor-everyone",
                    "data-toggle": "dropdown",
                    "aria-haspopup": "true",
                    "aria-expanded": "false",
                    html: " " + datas.TotalRemark + " Reply"
                });

                var ddl = $("<div>", {
                    class: "dropdown-menu",
                    "x-placement": "left-start"
                }).css({
                    position: "absolute",
                    transform: "translate3d(-190px, 0px, 0px)",
                    top: "0px",
                    left: "0px",
                    "will-change": "transform"
                });

                for (var i = 0; i < datas.TotalRemark; i++) {
                    var data = datas.Remarks[i];

                    var item = $("<a>", {
                        class: "dropdown-item tk-reply-remark",
                        href: "Javascript:;",
                        html: data.CreatorFullname + ' ได้ตอบกลับข้อความของคุณเวลา ' + data.CreatedOn,
                        "data-seq-reply": data.Key
                    });

                    item.bind("click", function () {
                        var CurrentSEQ = container.find(".system-message-comment-container-remark-more").attr("data-current-seq");
                        var ReplySEQ = $(this).attr("data-seq-reply");

                        if ($(".system-message-comment-container-remark[data-message-seq='" + ReplySEQ + "']").length > 0) {
                            $(".system-message-comment-container-remark[data-message-seq='" + ReplySEQ + "']").addClass("focus-comment-reply");

                            $('html,body').animate({
                                scrollTop: $(".system-message-comment-container-remark[data-message-seq='" + ReplySEQ + "']").offset().top - 110
                            });

                            setTimeout(function () {
                                $(".focus-comment-reply").removeClass("focus-comment-reply");
                            }, 3500);
                        } else {
                            //container.find(".system-message-comment-container-remark-more").click();
                            if (onLoadMore != undefined && typeof (onLoadMore) == "function") {
                                afterLoadMoerFocusReply = function () {
                                    var CurrentSEQ = container.find(".system-message-comment-container-remark-more").attr("data-current-seq");
                                    if ($(".system-message-comment-container-remark[data-message-seq='" + ReplySEQ + "']").length > 0) {
                                        $(".system-message-comment-container-remark[data-message-seq='" + ReplySEQ + "']").addClass("focus-comment-reply");
                                        $('html,body').animate({
                                            scrollTop: $(".system-message-comment-container-remark[data-message-seq='" + ReplySEQ + "']").offset().top - 110
                                        });
                                        setTimeout(function () {
                                            $(".focus-comment-reply").removeClass("focus-comment-reply");
                                        }, 3500);
                                        afterLoadMoerFocusReply = undefined;
                                    } else {
                                        onLoadMore(CurrentSEQ, false);
                                    }
                                }

                                onLoadMore(CurrentSEQ, false);
                            }
                            //$(this).attr("data-current-seq", CurrentSEQ)
                            //$(this).activityRemarkLoadMore(onLoadMore);
                        }
                    });

                    ddl.append(item);
                }

                div.append(btn);
                div.append(ddl);
                panel.append(div);
                container.prepend(panel);
            }
        }
    });
    
}

$.fn.activityRemarkBuilder = function (objectData) {
    var datas = objectData.data;
    var _message = objectData.focusMessage;
    var _dateTime = objectData.focusMessageOn;
    var pointIndex = objectData.pointIndex;
    var containerCss = objectData.containerCss;
    var allowDivider = objectData.allowDivider == undefined ? true : objectData.allowDivider;
    var allowHighlight = objectData.allowHighlight == undefined ? true : objectData.allowHighlight;
    var postData = objectData.postData;
    var editData = objectData.editData;
    var aobjectlink = objectData.aobjectlink;
    var getOnlineForm = objectData.getOnlineForm;
    var getRefEmailContent = objectData.getRefEmailContent;
    var callBackRefEmailContent = objectData.callBackRefEmailContent;
    var callbackPickRemark = objectData.callbackPickRemark;
    var callBackPostSuccess = objectData.callBackPostSuccess;
    var callBackInitSuccess = objectData.callBackInitSuccess;

    var pagemode = objectData.pagemode;
    var islazyLoad = objectData.islazyLoad;
    var isEasyMode = objectData.isEasyMode;

    var isLoadMore = objectData.isLoadMore;
    var isLoadNewMessage = objectData.isLoadNewMessage;
    var onLoadMore = objectData.onLoadMore;
    var lazyLoadTotal = objectData.lazyLoadTotal;
    var myImagePath = objectData.myImagePath;

    var onReplySuccess = objectData.onReplySuccess == undefined ? false : objectData.onReplySuccess;
    var enableEditor = objectData.enableEditor == undefined ? true : objectData.enableEditor;
    var enableReply = objectData.enableReply == undefined ? true : objectData.enableReply;
    var container = $(this);
    var isPointed = false;
    var pointer = null;

    if (containerCss != undefined) {
        container.css(containerCss);
    }

    if (!container.hasClass("system-message-comment-builder")) {
        container.addClass("system-message-comment-builder");
    }

    var messageStorage = $(container).find(".system-message-comment-container-remark-storage");
    if (messageStorage.length == 0) {
        messageStorage = $("<div/>", {
            class: "system-message-comment-container-remark-storage " + (isEasyMode ? "easy-mode" : "")
        });
        container.append(messageStorage);
    }

    var messageDisplay;
    if (isLoadMore || isLoadNewMessage) {
        messageDisplay = messageStorage.find(".system-message-comment-container-remark-display");
    } else {
        messageDisplay = $("<div/>", {
            class: "system-message-comment-container-remark-display"
        });
        messageStorage.find(".system-message-comment-container-remark-display").remove();
        messageStorage.append(messageDisplay);
    }


    var loadmoreSortElement = null;

    for (var i = 0; i < datas.length; i++) {

        var messageType = datas[i].MessageType;

        if (messageStorage.find(".system-message-comment-container-remark[data-message-seq='" + datas[i].Key + "']").length > 0)
            continue;

        var p = $("<p/>", {
            class: "system-message-comment-container-remark",
            "data-message-type": messageType,
            "data-message-seq": datas[i].Key,
            css: {
                borderBottom: allowDivider ? "1px solid #ccc" : "none"
            }
        });

        if (isLoadNewMessage) {
            p.css({
                borderLeft: "2px solid #0085A1"
            });
        }

        if (callbackPickRemark != undefined) {
            p.bind("click", {
                remarkData: datas[i]
            }, function (e) {
                callbackPickRemark(e.data.remarkData);
            })
        }

        if (allowHighlight) {
            p.activityRemarkHighlight({
                postData: postData,
                remarkSeq: datas[i].Key,
                onReplySuccess: onReplySuccess,
                aobjectlink: aobjectlink,
                pagemode: pagemode,
                callBackPostSuccess: callBackPostSuccess
            });
        }

        var dateTime = $("<span/>", {
            class: "system-message-comment-container-remark-date",
            html: datas[i].CreatedOn
        });



        var isMyRemark = datas[i].CreatorLinkID == datas[i].MyLinkID;

        if (messageType == _EMAIL_INCOMING || messageType == _EMAIL_OUTGOING) {

            var img;
            if (messageType == _EMAIL_INCOMING) {
                img = $("<img/>", {
                    class: "system-message-comment-container-remark-img",
                    src: servictWebDomainName + "images/icon/mail.png?version=1.2",
                    css: {
                        borderRadius: 0
                    }
                });
            } else {
                img = $("<div/>", {
                    class: "system-message-comment-container-remark-img",
                    css: {
                        backgroundImage: "url(" + servictWebDomainName.slice(0, -1) + datas[i].Image + ")"
                    }
                });
            }

            var senderName = datas[i].CreatorFullname;
            if (getRefEmailContent == undefined) {
                senderName = "Email";
            }
            else {
                if (messageType == _EMAIL_OUTGOING)
                    senderName = datas[i].CreatorFullname;
            }

            var fullname = $("<span/>", {
                class: "system-message-comment-container-remark-fullname",
                html: senderName
            });


            var arrowMail = "";
            if (getRefEmailContent != undefined) {
                arrowMail = $("<i/>", {
                    class: "system-message-mail-arrow fa " + (messageType == _EMAIL_INCOMING ? "fa-mail-forward arrow-in" : "fa-mail-reply arrow-out")
                });
            }

            p.append(img, arrowMail, fullname, dateTime);
            if (getRefEmailContent != undefined) {
                p.addClass("system-message-mailview-container");
                p.activityRemarkMailView(aobjectlink, datas[i], getRefEmailContent, callBackRefEmailContent);
            } else {
                var previewMail = $("<p/>", {
                    class: "system-message-comment-container-remark-message",
                    html: messageType == _EMAIL_INCOMING ? "Imcoming email" : "Outgoing email"
                });
                var url = "/timeattendance/ActivityManagementReDesign.aspx?aobj=" + aobjectlink + "&snaid=" + (aobjectlink.substring(0, 7));
                previewMail.append("<a style='margin-left:5px' href='" + url + "' ><i class='fa fa-search'></i> View detail</a>");
                p.append(previewMail);
            }

        } else {

            var MessageBuilder = RemarkMessageTextBuilder(datas[i].MessageText, messageType);

            var fullname = $("<span/>", {
                class: "system-message-comment-container-remark-fullname",
                html: datas[i].CreatorFullname + " "
            });

            var message = $("<p/>", {
                class: "system-message-comment-container-remark-message"
            });

            message.append(MessageBuilder.element);

            var img = $("<div/>", {
                class: "system-message-comment-container-remark-img",
                css: {
                    backgroundImage: "url(" + servictWebDomainName.slice(0, -1) + datas[i].Image + ")"
                }
            });

            var messageOption = $("<i/>", {
                class: "fa fa-reply pull-right fa-option-activity-remark",
                css: {
                    color: "#0085A1",
                    cursor: "pointer",
                    marginLeft: 10
                }
            });
            message.append(messageOption);
            messageOption.bind("click", {
                postData: postData,
                remarkSeq: datas[i].Key,
                onReplySuccess: onReplySuccess,
                aobjectlink: aobjectlink,
                pagemode: pagemode
            }, function (e) {
                e.data.postData.replyForm = e.data.remarkSeq;
                $(this).closest(".system-message-comment-container-remark").activityRemarkHighlightStartUp(e, {
                    onReplySuccess: e.data.onReplySuccess,
                    postData: e.data.postData,
                    aobjectlink: e.data.aobjectlink,
                    pagemode: e.data.pagemode,
                    callBackPostSuccess: callBackPostSuccess//,
                    //replyForm: e.data.remarkSeq
                }, true); 
            })

            if (isMyRemark) {
                if (enableEditor && editData != undefined && MessageBuilder.allowEdit && messageType != "CHECK_IN") {
                    var remarkEditor = $("<i/>", {
                        class: "fa fa-edit pull-right fa-edit-activity-remark",
                        css: {
                            color: "#0085A1",
                            cursor: "pointer"
                        }
                    });
                    remarkEditor.bind("click", {
                        code: datas[i].Key,
                        parent: message
                    }, function (e) {
                        var msg = $(e.data.parent).find(".remark-text-container").clone();
                        var isWip = $(e.data.parent).find(".btn-wip").length > 0;

                        $(e.data.parent).activityRemarkEditor({
                            text: convertToSystemText(msg.html()),
                            code: e.data.code,
                            isWip: isWip,
                            editData: editData,
                            aobjectlink: aobjectlink
                        });
                    });
                    message.append(remarkEditor);
                }
            }



            p.append(img, fullname, dateTime);

            if (datas[i].QuoteMessage != undefined && datas[i].QuoteMessage != "") {
                var quoteMessage = $("<div/>", {
                    class: "system-message-quote",
                    html: convertToHTMLText(datas[i].QuoteMessage)
                });
                p.append(quoteMessage);
            }

            p.append(message);
        }

        if (datas[i].RemarkEventDetail != null && datas[i].RemarkEventDetail != "") {
            var EventDetail = $("<p/>", {
                class: "alert alert-info system-message-comment-container-event-message",
                html: "<i class='fa fa-info-circle'></i> <span class='event-message-text'>ได้ทำการ" + datas[i].RemarkEventDetail.split("\n").join("<br>") + "</span> ",
                css: {
                    fontStyle: "italic",
                    margin: "5px 0",
                    padding: "10px 20px",
                    borderLeft: "3px solid #31708F",
                    borderRadius: 0
                }
            });

            p.find(".system-message-comment-container-remark-message").before(EventDetail);
            //p.find(".fa-edit-activity-remark").remove();
        }

        if (isMyRemark) {
            p.addClass("system-message-comment-container-remark-owner");
        }


        if (datas[i].MessageType == "ONLINEFORM") {
            var answer = "<a style='color:orange;display:block;margin-bottom:10px' target='_blank' href='" + servictWebDomainName + "questionnaire/viewansquestionnairedetail.aspx?id=" + datas[i].AnswerFormKey + "'><i class='fa fa-file-text-o'></i> ตอบแบบฟอร์มออนไลน์ : " + datas[i].AnswerFormName + "</a>";
            p.append(answer);
        }

        //if (datas[i].Files != null && datas[i].Files != undefined && datas[i].Files.length > 0) {
        p.activityRemarkAppendMessageAttachment(datas[i].Files);
        //}


        if (_message != undefined && _dateTime != undefined && !isPointed && datas[i].MessageText.split("<br>").join("").split("\n").join("").split(" ").join("").match(_message.split("<br>").join("").split("\n").join("").split(" ").join("")) && _dateTime.substring(0, 16) == datas[i].CreatedOn.substring(0, 16)) {
            var arrow = $("<i/>", {
                class: "fa fa-caret-right system-message-comment-arrow"
            });
            //p.append(arrow);
            img.css({
                top: 10,
                left: 10
            });
            dateTime.css({
                color: "#aaa"
            });
            p.addClass("system-message-comment-pointed");
            isPointed = true;
            if (pointIndex != undefined && pointIndex == "element") {
                pointer = message;
            }
        }

        if (datas[i].UpdatedOn != undefined && datas[i].UpdatedOn != null && datas[i].UpdatedOn != "") {
            message.activityRemarkEditedOn(datas[i].UpdatedOn);
        }


        if (isLoadMore) {
            if (loadmoreSortElement == null) {
                messageDisplay.prepend(p);
            }
            else {
                loadmoreSortElement.after(p);
            }
            loadmoreSortElement = p;
        } else {
            messageDisplay.append(p);
        }

        if (pointIndex != undefined && pointIndex == "last") {
            pointer = message;
        }

        //try {
        //    p.aGepeGalleryContainer(MessageBuilder.message);
        //}
        //catch (e) { }
    }

    try {
        container.aGepeGalleryContainer(MessageBuilder.message);
    }
    catch (e) { }

    messageStorage.find(".system-message-comment-container-remark-more").remove();

    if (islazyLoad && messageStorage.find(".system-message-comment-container-remark").length < lazyLoadTotal) {

        var lastSeqMore = messageStorage.find(".system-message-comment-container-remark").length == 0 ? "0"
            : messageStorage.find(".system-message-comment-container-remark:first").attr("data-message-seq");

        var more = $("<div/>", {
            class: "system-message-comment-container-remark-more",
            html: "ดูความคิดเห็นก่อนหน้า",
            css: {
            },
            "data-current-seq": lastSeqMore
        });
        messageDisplay.prepend(more);
        $(more).activityRemarkLoadMore(onLoadMore); 
    }

    var isContinueFocusReply = messageStorage.find(".new-message-trigger.trigger-post").length > 0;

    messageStorage.find(".new-message-trigger").remove();

    var lastSeqLoadNew = messageStorage.find(".system-message-comment-container-remark").length == 0 ? "0"
            : messageStorage.find(".system-message-comment-container-remark:last").attr("data-message-seq");

    var newMessageTrigger = $("<div/>", {
        class: "new-message-trigger",
        //html: "NEW MESSAGE TRIGGER",
        "data-new-message-trigger-seq": lastSeqLoadNew,
        "data-new-message-trigger-aobjectlink": aobjectlink
    });
    messageDisplay.prepend(newMessageTrigger);
    $(newMessageTrigger).activityRemarkLoadNewMessage(onLoadMore);


    if (enableReply && postData != undefined && container.find(".system-message-comment-container-reply").length == 0) {
        container.activityRemarkReplyBox(postData, onReplySuccess, allowDivider,
            aobjectlink, isEasyMode, myImagePath, getOnlineForm,
            callBackRefEmailContent, callBackPostSuccess);
    }
    else {
        var oldReplyBox = container.find(".system-message-comment-container-reply");
        if (oldReplyBox.is(":focus"))
            isContinueFocusReply = true;

        if (datas.length > 0) {
            container.append(oldReplyBox);
        }
    }

    if (isContinueFocusReply) {
        container.find(".system-message-comment-container-reply textarea").focus();
    }


    if (isLoadNewMessage) {
        //$("html,body").scrollTop(container.find(".system-message-comment-container-remark:last").offset().top);
    }

    if (callBackInitSuccess && typeof (callBackInitSuccess) === "function") {
        //var _e = {
        //    postData: postData,
        //    remarkSeq: -1,
        //    onReplySuccess: onReplySuccess,
        //    aobjectlink: aobjectlink,
        //    pagemode: pagemode
        //};
        var _data = {
            postData: postData,
            remarkSeq: -1,
            onReplySuccess: onReplySuccess,
            aobjectlink: aobjectlink,
            pagemode: pagemode,
            callBackPostSuccess: callBackPostSuccess
        };

        callBackInitSuccess(container, _data);
    }

    container.loadReplyComment(aobjectlink, onLoadMore);

    return pointer;

}

$.fn.activityRemarkAppendMessageAttachment = function (files, rebindGallery) {
    var p = $(this);

    var row = $("<div/>", {
        class: "system-message-attach-files-container",
        css: {
            margin: "10px 0",
        }
    });
    p.append(row);


    if (files != undefined) {
        for (var x = 0; x < files.length; x++) {
            var file = files[x];
            var customType = file.Type;
            var fileName = file.Url.split("/")[file.Url.split("/").length - 1];

            if (customType == "LOCATION") {
                var address = $("<p/>", {
                    html: file.Thumbnail
                });
                address.appendTo(row);

                var mapData = JSON.parse(file.Data);
                var location = mapData.latitude + "," + mapData.longitude;
                var mapSrc = "https://maps.google.com/maps?q=" + location + "&hl=es;z=16&output=embed&disableDefaultUI=false";
                var map = $("<iframe  />", {
                    css: {
                        width: "100%",
                        maxWidth: 400,
                        height: 250,
                        border: 0,
                        display: "block",
                        margin: "10px 0"
                    },
                    frameborder: 0,
                    allowfullscreen: true
                });
                map.appendTo(row);

                var desc = p.find(".remark-text-container");
                desc.appendTo(p);
                desc.css("margin-bottom", "20px");

                map.prop("src", mapSrc);
            } else {

                var attachFiles;
                if (fileName.toLowerCase().match(/.(jpg|jpeg|png|gif|bmp|svg)$/i)) {
                    var a = $("<a/>", {
                        class: "system-message-attach-files",
                    });
                    var col = $("<div/>", {
                        class: "system-message-attach-image-block",
                        "data-image": file.Url,
                        css: {
                            backgroundImage: "url('" + servictWebDomainName.slice(0, -1) + file.Url + "')"
                        }
                    });
                    row.append(a);
                    a.append(col);
                    attachFiles = a;
                }
                else if (fileName.toLowerCase().match(/.(mp3)$/i)) {
                    var a = $("<a/>", {
                        class: "system-message-attach-files",
                        css: {
                            cursor: "default"
                        }
                    });

                    var col = $("<div/>", {
                        class: "system-message-attach-file-block",
                    });

                    var playBox = $("<div/>", {
                        css: {
                            padding: 15
                        }
                    });
                    playBox.appendTo(col);

                    var audio = $("<audio/>", {
                        controls: true,
                        css: {
                            display: "none"
                        }
                    });

                    audio.append($("<source/>", {
                        type: "audio/mpeg",
                        src: file.Url
                    }));

                    audio.get(0).onended = function () {
                        $(this).next().removeClass("fa-play").removeClass("fa-pause").addClass("fa-play");
                    }

                    audio.appendTo(playBox);

                    var togglePlay = $("<span/>", {
                        class: "fa fa-3x fa-play",
                        css: {
                            cursor: "pointer",
                            color: "#333"
                        },
                        click: function () {
                            if ($(this).hasClass("fa-play")) {
                                $(this).prev().get(0).play();
                            } else {
                                $(this).prev().get(0).pause();
                            }
                            $(this).toggleClass("fa-play").toggleClass("fa-pause");
                        }
                    });

                    togglePlay.appendTo(playBox);

                    playBox.after("<span style='color:#333'>Voice Streamimg</span>");

                    row.append(a);
                    a.append(col);
                    attachFiles = a;
                }
                else if (fileName.toLowerCase().match(/.(mp4)$/i)) {
                    var a = $("<a/>", {
                        class: "system-message-attach-files",
                        css: {
                            cursor: "default"
                        }
                    });

                    var col = $("<div/>", {
                        class: "system-message-attach-file-block",
                        css: {
                            padding: 0
                        }
                    });

                    var video = $("<video/>", {
                        controls: true,
                        css: {
                            width: 120,
                            height: 120,
                            background: "#000"
                        }
                    });

                    video.append($("<source/>", {
                        type: "video/mp4",
                        src: "http://espresso.p21academic.com" + file.Url
                    }));


                    video.appendTo(col);

                    row.append(a);
                    a.append(col);
                    attachFiles = a;
                }
                else {

                    var a = $("<a/>", {
                        class: "system-message-attach-files",
                        "data-file-url": file.Url,
                        "data-path": file.Path,
                        "data-name": fileName,
                        "title": fileName,
                        click: function () {
                            try {
                                ActivityDownloadFileForm(this);
                                return false;
                            } catch (e) { }
                        }
                    });
                    var col = $("<div/>", {
                        class: "system-message-attach-file-block",
                        html: "<div style='margin-bottom:5px'><i class='fa " + getIconFileType(fileName) + " fs-26'></i></div><span class='four-line'>" + fileName + "</span>"
                    });
                    row.append(a);
                    a.append(col);
                    attachFiles = a;
                }

                var removeElt = $("<i/>", {
                    class: "fa fa-remove file-remover",
                    "data-key": file.Key,
                    "data-asset-key": file.AssetKey
                });
                attachFiles.append(removeElt);
                removeElt.activityRemarkEditFileRemove();
            }
        }

        if (rebindGallery) {
            try {
                row.aGepeGalleryContainer("");
            }
            catch (e) { }
        }        
    }

    var blockAdder = $("<div/>", {
        class: "system-message-attach-image-block-adder",
        html: "<i class='fa fa-plus'></i>",
        css: {
            display: "none"
        }
    });

    blockAdder.bind("click", {
        fileContainer: row
    }, function (e) {
        e.data.fileContainer.activityRemarkReplyBoxAttach("FILE");
    });

    row.append(blockAdder);
}

$.fn.activityRemarkMailView = function (aobjectlink, data, getRefEmailContent, callBackRefEmailContent) {
    var container = $(this);
    var mailView = $("<p/>", {
        "data-email-aobjectlink": aobjectlink,
        "data-email-ref": data.RefEmailCode,
        class: "system-message-comment-container-remark-message system-message-mail-view",
        html: "กำลังดึงเนื้อหาอีเมลล์..."
    });


    if (getRefEmailContent != undefined) {
        container.append(mailView);
        getRefEmailContent.key.refEmailCode = data.RefEmailCode;
        getRefEmailContent.key.includeEmailBody = false;
        $.ajax({
            url: getRefEmailContent.url,
            data: getRefEmailContent.key,
            success: function (datas) {
                for (var i = 0; i < datas.length; i++) {
                    var mail = datas[i];
                    var mailContainer = container.find(".system-message-mail-view[data-email-ref='" + mail.message_id + "']");
                    var type = mailContainer.closest(".system-message-comment-container-remark").attr("data-message-type");
                    var showHeader = "";
                    if (type == _EMAIL_INCOMING) {
                        showHeader = "<b>Receiver: </b>"
                        showHeader += mail.EMAIL_BASE;
                    } else {
                        showHeader = data.fullname;
                    }
                    mailContainer.closest(".system-message-comment-container-remark")
                        .find(".system-message-comment-container-remark-fullname").html(showHeader);

                    mailContainer.html("");

                    var mailTop = $("<div/>", {
                        class: "mail-top"
                    });
                    mailTop.appendTo(mailContainer);

                    var table = $("<table/>", {
                        class: "mail-table"
                    });

                    table.appendTo(mailTop);

                    if (type == _EMAIL_INCOMING) {
                        // row
                        var tr = $("<tr/>");
                        tr.appendTo(table);

                        // col 1
                        td = $("<td/>", {
                            class: "mail-line-subject"
                        });
                        td.appendTo(tr);
                        td.append($("<span/>", {
                            html: "From"
                        }));

                        // col 2
                        td = $("<td/>");
                        td.appendTo(tr);
                        td.append($("<span/>", {
                            html: mail.EMAIL_FROM,
                            class: "ref-email-from"
                        }));
                    }

                    if (type == _EMAIL_OUTGOING) {

                        if (mail.EMAIL_TO != null && mail.EMAIL_TO.trim() != "") {
                            // row
                            tr = $("<tr/>");
                            tr.appendTo(table);

                            // col 1
                            td = $("<td/>", {
                                class: "mail-line-subject"
                            });
                            td.appendTo(tr);
                            td.append($("<span/>", {
                                html: "To"
                            }));

                            // col 2
                            td = $("<td/>");
                            td.appendTo(tr);
                            td.append($("<span/>", {
                                html: mail.EMAIL_TO,
                                class: "ref-email-to"
                            }));
                        }
                    }

                    if (mail.EMAIL_CC != null && mail.EMAIL_CC.trim() != "") {
                        // row
                        tr = $("<tr/>");
                        tr.appendTo(table);

                        // col 1
                        td = $("<td/>", {
                            class: "mail-line-subject"
                        });
                        td.appendTo(tr);
                        td.append($("<span/>", {
                            html: "CC"
                        }));

                        // col 2
                        td = $("<td/>");
                        td.appendTo(tr);
                        td.append($("<span/>", {
                            html: mail.EMAIL_CC,
                            class: "ref-email-cc"
                        }));
                    }

                    // row
                    tr = $("<tr/>");
                    tr.appendTo(table);

                    // col 1
                    td = $("<td/>", {
                        class: "mail-line-subject"
                    });
                    td.appendTo(tr);
                    td.append($("<span/>", {
                        html: "Subject"
                    }));

                    // col 2
                    td = $("<td/>");
                    td.appendTo(tr);
                    td.append($("<span/>", {
                        html: mail.EMAIL_SUBJECT,
                        class: "ref-email-subject"
                    }));

                    // row
                    tr = $("<tr/>");
                    tr.appendTo(table);

                    // col 1
                    td = $("<td/>", {
                        class: "mail-line-subject"
                    });
                    td.appendTo(tr);
                    td.append($("<span/>", {
                        html: "Body"
                    }));

                    // col 2

                    var mailBodyContent = $("<span/>", {
                        class: "ref-email-body"
                    })

                    td = $("<td/>");
                    td.appendTo(tr);
                    td.append(mailBodyContent);

                    mailBodyContent.activityRemarkMailViewShowContent(mail, data.MessageType, callBackRefEmailContent);


                    if (mail.attachment != null && mail.attachment != undefined && mail.attachment.length > 0) {
                        container.activityRemarkAppendMessageAttachment(datas[i].attachment);
                        try {
                            container.closest(".system-message-comment-builder").aGepeGalleryContainer("");
                        }
                        catch (e) { }
                    }
                }
            }
        });
    }
}

$.fn.activityRemarkMailViewShowContent = function (mail, MessageType, callBackRefEmailContent) {
    var mailBodyContent = $(this);

    var btn = $("<a/>", {
        href: "javascript:;",
        html: "<i class='fa fa-envelope-o'></i> แสดงเนื้อหาอีเมลล์"
    });
    btn.appendTo(mailBodyContent);
    btn.bind("click", {
        fileName: mail.PATH_EMAIL
    }, function (e) {
        var src = "/widget/iframeactivityemailcontent.aspx?filename=" + e.data.fileName;
        var modal = $(this).next();
        modal.modal("show");
        modal.find("iframe").prop("src", src);
    });

    var modal = $("<div/>", {
        class: "modal fade mail-modal"
    });
    modal.appendTo(mailBodyContent);

    var dialog = $("<div/>", {
        class: "modal-dialog modal-lg",
        css: {
            width: "98%"
        }
    });
    dialog.appendTo(modal);

    var content = $("<div/>", {
        class: "modal-content"
    });
    content.appendTo(dialog);

    var header = $("<div/>", {
        class: "modal-header",
        html: "<h4 class='modal-title'>" + mail.EMAIL_SUBJECT + "</h4><button type='button' class='close' data-dismiss='modal'>&times;</button>"
    });
    header.appendTo(content);

    var body = $("<div/>", {
        class: "modal-body"
    });
    body.appendTo(content);

    var footer = $("<div/>", {
        class: "modal-footer",
        html: "<button type='button' class='btn btn-default' data-dismiss='modal'>Close</button>"
    });
    footer.appendTo(content);

    var frame = $("<iframe/>", {
        class: "mail-frame-body",
        css: {
            width: "100%",
            height: window.innerHeight - 270,
            border: "none"
        }
    });
    frame.appendTo(body);

    //if (MessageType == _EMAIL_INCOMING) {
    var btnReply = $("<span/>", {
        class: "btn btn-primary d-none",
        html: "<i class='fa fa-mail-reply'></i> Reply"
    });
    footer.prepend(btnReply);
    if (callBackRefEmailContent != undefined) {
        btnReply.bind("click", {
            getRefEmailContent: mail.getRefEmailContent,
            MessageType: MessageType
        }, function (e) {
            var mailType = e.data.MessageType;

            var mailCon = $(this).closest('.system-message-mail-view');
            var returnData = {};
            returnData.action = mailType == _EMAIL_INCOMING ? "REPLY" : "FORWARD";
            returnData.aobjectlink = mailCon.attr("data-email-aobjectlink");
            returnData.refEmailCode = mailCon.attr("data-email-ref");
            returnData.from = mailCon.find(".ref-email-from").html();
            returnData.to = mailCon.find(".ref-email-to").html();
            returnData.cc = mailCon.find(".ref-email-cc").html();
            returnData.subject = mailCon.find(".ref-email-subject").html();


            agroLoading(true);
            e.data.getRefEmailContent.key.includeEmailBody = true;
            $.ajax({
                url: e.data.getRefEmailContent.url,
                data: e.data.getRefEmailContent.key,
                success: function (mailData) {
                    agroLoading(false);
                    for (var i = 0; i < mailData.length; i++) {
                        returnData.detail = mailData[i].EMAIL_MESSAGE;
                    }
                    mailCon.find(".mail-modal").modal("hide");
                    callBackRefEmailContent(returnData);
                }
            });

        });
    } else {
        btnReply.attr("disabled", true);
    }
    //}
}

$.fn.activityRemarkLoadMore = function (onLoadMore) {
    $(this).bind("click", {
        onLoadMore: onLoadMore
    }, function (e) {
        $(this).html("กำลังโหลดข้อความก่อนหน้า...").css("color", "#777");
        var onLoadMore = e.data.onLoadMore;
        var curSeq = $(this).attr("data-current-seq");
        if (onLoadMore != undefined && typeof (onLoadMore) == "function") {
            onLoadMore(curSeq, false);
        }
    });
}

$.fn.activityRemarkLoadNewMessage = function (onLoadMore) {
    $(this).bind("click", {
        onLoadMore: onLoadMore
    }, function (e) {
        var onLoadMore = e.data.onLoadMore;
        var curSeq = $(this).attr("data-new-message-trigger-seq");
        if (onLoadMore != undefined && typeof (onLoadMore) == "function") {
            onLoadMore(curSeq, true);
        }
    });
}

$.fn.activityRemarkReplyBoxPasteImage = function (_fileContaciner) {
    var inputImagePaste = $(this)[0]; 

    var CLIPBOARD = new CLIPBOARD_CLASS();
    CLIPBOARD.setContainer(_fileContaciner);

    function CLIPBOARD_CLASS() {
        var _self = this;

        //handlers
        inputImagePaste.addEventListener('paste', function (e) { _self.paste_auto(e); }, false);

        var container = null;

        this.setContainer = function (inputContainer) {
            container = inputContainer;
        }
        //on paste
        this.paste_auto = function (e) {
            if (e.clipboardData) {
                var items = e.clipboardData.items;
                if (!items) return;

                //access data directly

                var hasTextPlain = false;
                var clipboardImage = null;
                for (var i = 0; i < items.length; i++) {
                    if (items[i].type == "text/plain") {
                        hasTextPlain = true;
                        break;
                    }

                    if (items[i].type.indexOf("image") !== -1) {
                        var blob = items[i].getAsFile();
                        clipboardImage = blob;
                    }
                }
                if (clipboardImage != null && !hasTextPlain) {
                    var fileContaciner = container.activityRemarkReplyBoxAttachContainerGenerator();
                    var guid = generateGUID();
                    activityRemarkReplyBoxAttachGlobalFiles.push({
                        file: clipboardImage,
                        guid: guid
                    });
                    fileContaciner.activityRemarkReplyBoxAttachAppendImage(clipboardImage, guid);
                    container.show();
                }
            }
        };
    }
}

$.fn.activityRemarkReplyBox = function (postData, onReplySuccess, allowDivider, aobjectlink, isEasyMode, myImagePath, getOnlineForm, callBackRefEmailContent, callBackPostSuccess) {

    var container = $(this);

    var pSpace = $("<p/>", {
        css: {
            paddingTop: 150
        }
    });

    var pReply = $("<div/>", {
        class: "system-message-comment-container-reply"
    });

    var textArea = $("<textarea/>", {
        class: "form-control form-control-sm",
        css: {
            resize: "none"
        },
        rows: 4,
        keyup: function () {
            if ($(this).val().length > 10000) {
                $(this).closest(".system-message-comment-container-reply").find(".btn-submit-to-post").attr("disabled", true);
                $(this).closest(".system-message-comment-container-reply").find(".post-reply-message-length").parent().css("color", "#fa5949");
            } else {
                $(this).closest(".system-message-comment-container-reply").find(".btn-submit-to-post").removeAttr("disabled");
                $(this).closest(".system-message-comment-container-reply").find(".post-reply-message-length").parent().css("color", "");
            }
            $(this).closest(".system-message-comment-container-reply").find(".post-reply-message-length").html($(this).val().length);
        }
    });


    var fileContainer = $("<div/>", {
        class: "system-message-comment-container-reply-file-container"
    });

    var subCon = $("<p/>", {
        class: "text-right",
        css: {
            marginTop: 10,
            marginBottom: 0,
            paddingBottom: 43
        }
    });

    var msgLength = $("<span/>", {
        class: "pull-left",
        css: {
            marginTop: -8
        },
        html: "<small>(<span class='post-reply-message-length'>0</span>/10,000)</small>"
    });

    subCon.append(msgLength);


    var submit = $("<span/>", {
        class: "btn btn-primary btn-sm pull-right activity-chatting-fullmode btn-submit-to-post",
        html: "เพิ่มความคิดเห็น",
        css: {
            boxShadow: "none"
        }
    });

    submit.bind("click", {
        container: container,
        postData: postData,
        pReply: pReply,
        textArea: textArea,
        onReplySuccess: onReplySuccess,
        allowDivider: allowDivider,
        aobjectlink: aobjectlink,
        callBackPostSuccess: callBackPostSuccess
    }, function (e) {
        e.preventDefault();
        var elt = $(this);
        var commentRemark = e.data.textArea.val().trim();
        if (commentRemark == "" && activityRemarkReplyBoxAttachGlobalFiles.length == 0) {
            AGMessage("กรุณาระบุความคิดเห็น");
            //alert("กรุณาระบุความคิดเห็น");
        }
        else {
            if (postData != undefined) {
                var quoteElt = e.data.container.closest(".remark-highlight-command-box");
                var messageQuote = quoteElt.find(".remark-highlight-command-box-quote-message").html();
                var quoteType = quoteElt.find(".remark-highlight-command-box-quote-type").html();
                var remarkType = elt.parent().find("select").val(); 
                var isQuote = messageQuote != undefined;
                var guid = generateGUID();

                if (activityRemarkReplyBoxAttachGlobalFiles.length > 0) {
                    commentRemark = commentRemark; // + " (" + activityRemarkReplyBoxAttachGlobalFiles.length + " Attachments)";
                }

                if (isQuote) {
                    e.data.pReply.AGWhiteLoading(false);
                    e.data.pReply.AGWhiteLoading(true, "Posting remark");
                }
                else {
                    messageQuote = "";
                    quoteType = "";
                    e.data.pReply.activityRemarkReplyBoxPosting(guid, commentRemark, remarkType, e.data.allowDivider);
                }

                e.data.textArea.val("");

                var _PostData = postData;
                _PostData.key.isQuote = isQuote;
                _PostData.key.quoteMessage = convertToSystemText(messageQuote);
                _PostData.key.quoteType = quoteType;
                _PostData.key.remarkMessage = commentRemark;
                _PostData.key.remarkType = remarkType;
                if (isQuote) {
                    _PostData.key.RefCode = postData.replyForm;
                } else {
                    _PostData.key.RefCode = "";
                }
                _PostData.key.sendMail = elt.parent().find("input").prop("checked");

                if (
                    //$("#activity-remark-file-form").length > 0 && 
                    activityRemarkReplyBoxAttachGlobalFiles.length > 0) {
                    ajaxPostUploadFiles(e, {
                        finalPostData: _PostData,
                        panelReply: e.data.pReply,
                        onReplySuccess: e.data.onReplySuccess,
                        callBackPostSuccess: e.data.callBackPostSuccess,
                        guid: guid,
                        aobjectlink: aobjectlink
                    });
                }
                else {
                    ajaxPostDataRemark({
                        finalPostData: _PostData,
                        panelReply: e.data.pReply,
                        onReplySuccess: e.data.onReplySuccess,
                        callBackPostSuccess: e.data.callBackPostSuccess,
                        guid: guid
                    });
                }
            }
        }
    })

    var chkMail = $("<input/>", {
        type: "checkbox",
        css: {
            marginRight: 5
        }
    });

    var spanMail = $("<span/>", {
        class: "activity-chatting-fullmode",
        css: {
            float: "left"
        }
    });

    var spanMailLabel = $("<span/>", {
        html: "ส่งอีเมล",
        click: function () {
            $(this).prev().click();
        }
    });

    spanMail.append(chkMail, spanMailLabel);


    //var imageAttach = $("<i/>", {
    //    class: "reply-attach fa fa-picture-o pull-right",
    //    title: "Attach pictures"
    //});

    //imageAttach.bind("click", {
    //    fileContainer: fileContainer
    //}, function (e) {
    //    e.data.fileContainer.activityRemarkReplyBoxAttach("IMAGE");
    //});



    var btnGroup = $("<div/>", {
        class: "btn-group reply-attach pull-right",
        css: {
            marginRight: 10
        }
    });

    var imgAttach = $("<span/>", {
        class: "btn btn-default btn-sm",
        css: {
            borderColor: "#ccc"
        },
        html: "<i class='fa fa-picture-o' style='color:#89BE4A'></i>  Photo"//
    });
    imgAttach.appendTo(btnGroup);
    imgAttach.bind("click", {
        fileContainer: fileContainer
    }, function (e) {
        e.data.fileContainer.activityRemarkReplyBoxAttach("IMAGE");
    });

    var fileAttach = $("<span/>", {
        class: "btn btn-default btn-sm",
        css: {
            borderColor: "#ccc"
        },
        html: "<i class='fa fa-file-text' style='color:#FFCC3D'></i> File"
    });
    fileAttach.appendTo(btnGroup);
    fileAttach.bind("click", {
        fileContainer: fileContainer
    }, function (e) {
        e.data.fileContainer.activityRemarkReplyBoxAttach("FILE");
    });

    //var btnHighlight = $("<span/>", {
    //    class: "btn btn-default btn-sm",
    //    css: {
    //        borderColor: "#ccc"
    //    },
    //    html: '<input id="chkHighlight" type="checkbox" name="chkHighlight"><span onclick="$(this).prev().click();"> Highlight</span>'
    //});
    //btnHighlight.appendTo(btnGroup);

    var select = $("<select/>", {
        class: "form-control form-control-sm pull-right activity-chatting-fullmode ddl-option-type-remark ticket-allow-editor ticket-allow-editor-everyone",
        css: {
            display: "none",
            width: 150,
            marginRight: 20,
            color: "#aaa",
            cursor: "pointer"
        }
    });

    select.append("<option value='REMARK' selected='selected'>Comment</option>");
    select.append("<option value='Highlight'>Highlight</option>");
    //select.append("<option value='Reply' hidden='hidden'>Reply</option>");
    //select.append("<option value='WIP'>Work In Process</option>");

    //subCon.append(spanMail);
    subCon.append(submit, select);
    if (aobjectlink != undefined) {
        //subCon.append(imageAttach, fileAttach);
        subCon.append(btnGroup);

        if (callBackRefEmailContent != undefined) {

            var sendMail = $("<i/>", {
                class: "reply-attach fa fa-envelope-o pull-right hide",
                title: "Send mail",
                css: {
                    marginRight: 10
                }
            });

            sendMail.bind("click", {
                aobjectlink: aobjectlink
            }, function (e) {
                var returnData = {};
                returnData.action = "NEWEMAIL";
                returnData.aobjectlink = e.data.aobjectlink;
                returnData.refEmailCode = "";
                returnData.from = "";
                returnData.cc = "";
                returnData.to = "";
                returnData.subject = "";
                returnData.detail = "";
                callBackRefEmailContent(returnData);
            })

            subCon.append(sendMail);
        }
    }

    var onlineFormAttach = null;

    //========================== Activity Online Form =========================
    if (getOnlineForm != undefined && typeof ("function")) {

        onlineFormAttach = $("<div/>", {
            class: "reply-attach pull-right dropup hide",
            title: "Online form",
            css: {
                marginRight: 10,
                display: "none"
            }
        });

        var innerContainer = $("<div/>", {
            css: {
                position: "relative"
            }
        });

        onlineFormAttach.append(innerContainer);

        innerContainer.append($("<i/>", {
            class: "fa fa-file-text-o dropdown-toggle",
            "data-toggle": "dropdown"
        }));

        innerContainer.activityRemarkReplyBoxOnlineFormFunction(
            onlineFormAttach,
            getOnlineForm.url,
            getOnlineForm.key,
            onReplySuccess
        );

        subCon.append(onlineFormAttach);
    }


    pReply.append(textArea, fileContainer, subCon);
    container.append(pReply);

    if (myImagePath != "") {
        var img = $("<div/>", {
            class: "system-message-comment-container-remark-img",
            css: {
                backgroundImage: "url(" + servictWebDomainName.slice(0, -1) + myImagePath + ")",
                position: "absolute",
                borderRadius: "50%"
            }
        });
        pReply.append(img).css({
            paddingTop: 5
        });

    }


    textArea.activityRemarkReplyBoxPasteImage(fileContainer);

    if (isEasyMode) {

        msgLength.css({
            margin: 0
        });

        pReply.css({
            paddingBottom: 30
        });

        $(".activity-chatting-fullmode").hide();
        textArea.attr("rows", 1).css({
            paddingRight: 70,
            width: "100%",
            resize: "none",
            borderRadius: 4
        }).removeClass("form-control");

        var iconCss = {
            marginTop: -5,
            marginRight: 0,
            fontSize: 13,
            height: 26,
            padding: "3px 10px"
        };

        if (onlineFormAttach != null) {
            onlineFormAttach.css(iconCss);
        }

        imgAttach.css(iconCss);
        fileAttach.css(iconCss);
        btnGroup.css({ marginRight: 0 });

        iconCss.marginRight = 50;

        subCon.css("padding", "0");

        textArea.keydown(function (e) {
            if (e.keyCode == 13) {
                if (e.shiftKey) {
                    this.value = this.value + "\n";
                    e.stopPropagation();
                }
                else {
                    $(this).parent().find(".btn-primary").click();
                    e.stopPropagation();
                }
                return false;
            }
        }).keyup(function () {
            $(this).attr("rows", $(this).val().split('\n').length);
        });
    }
}

$.fn.activityRemarkReplyBoxOnlineFormFunction = function (elt, url, key, onReplySuccess) {
    var container = $(this);
    $.ajax({
        url: url,
        data: key,
        success: function (datas) {
            if (datas.length > 0) {
                $(elt).show();
                onReplySuccessWithAnswerOnlineForm = onReplySuccess;
            }
            container.activityRemarkReplyBoxOnlineForm(datas);
        }
    });
}

$.fn.activityRemarkReplyBoxOnlineForm = function (datas) {

    var ul = $("<ul/>", {
        class: "dropdown-menu"
    });
    for (var i = 0; i < datas.length; i++) {
        var data = datas[i];
        var li = $("<li/>");
        var a = $("<a/>", {
            href: "#",
            html: "ตอบแบบฟอร์ม : " + data.Subject
        });

        a.bind("click", {
            datas: data
        }, function (e) {
            try {
                OpemOnlineFormModal(
                    e.data.datas.SID,
                    e.data.datas.CompanyCode,
                    e.data.datas.OnlineFormKey,
                    e.data.datas.AOBJECTLINK
                );
            } catch (e) { }
            return false;
        });

        li.append(a);
        ul.append(li);
    }
    $(this).append(ul);
}

$.fn.activityRemarkReplyBoxAttachContainerGenerator = function () {
    var container = $(this);

    var fileBox;
    if (container.hasClass("system-message-attach-files-container")) {
        fileBox = container;
    } else {

        fileBox = container.find(".reply-attach-file-box");
        if (fileBox.length == 0) {
            fileBox = $("<div/>", {
                class: "reply-attach-file-box",
                css: {
                    padding: 10
                }
            });

            var blockAdder = $("<div/>", {
                class: "system-message-attach-image-block-adder",
                html: "<i class='fa fa-plus'></i>"
            });

            blockAdder.bind("click", {
                fileContainer: container
            }, function (e) {
                e.data.fileContainer.activityRemarkReplyBoxAttach("FILE");
            });

            fileBox.append(blockAdder);
        }

        container.append(fileBox);
    }

    return fileBox;
}

$.fn.activityRemarkReplyBoxAttach = function (uploadType) {
    var container = $(this);
    var iconAttach = container.closest(".system-message-comment-container-reply").find(".reply-attach");
    var commandBox = $("<div/>", {
        class: "text-right"
    });

    var fileBox = container.activityRemarkReplyBoxAttachContainerGenerator();

    var form = $("#activity-remark-file-form");
    if (form.length == 0) {
        form = $("<form method='post' style='padding-left:100px' action='/widget/AJAXFileUploadAPI.aspx' id='activity-remark-file-form' enctype='multipart/form-data' />");
    }
    form.html("").hide();

    var fileAccept = uploadType == "IMAGE" ? "image/*" : "*";
    var fileUpload = $("<input type='file' id='FilePath' name='FilePath' accept='" + fileAccept + "' multiple/>");


    form.append(fileUpload);
    $("body").append(form);

    var form = document.getElementById('activity-remark-file-form');

    // Add events
    $(fileUpload).bind("change", {
        container: container,
        iconAttach: iconAttach
    }, function (e) {
        if ($(fileUpload).get(0).files[0].size > 20000000) {
            AGMessage("Maximam file size 20Mb!");
            return;
        }

        var fLength = $(fileUpload).get(0).files.length;
        if (fLength > 0) {
            for (var i = 0; i < $(fileUpload).get(0).files.length; i++) {
                var guid = generateGUID();

                if (fileBox.closest(".system-message-comment-container-remark").attr("data-message-seq") != undefined) {
                    activityRemarkEditModeAttachGlobalFiles.push({
                        file: $(fileUpload).get(0).files[i],
                        guid: guid,
                        editSeq: fileBox.closest(".system-message-comment-container-remark").attr("data-message-seq")
                    });
                } else {
                    activityRemarkReplyBoxAttachGlobalFiles.push({
                        file: $(fileUpload).get(0).files[i],
                        guid: guid
                    });
                }
                fileBox.activityRemarkReplyBoxAttachAppendImage($(fileUpload).get(0).files[i], guid);
            }
            e.data.container.show();
        }
    });

    $(fileUpload).click();

}

$.fn.activityRemarkReplyBoxAttachAppendFile = function (fileName) {
    var container = $(this);
    var fileblock = $("<div/>", {
        class: "system-message-attach-file-block",
        html: "<i class='fa fa-file-text-o'></i> " + fileName
    });
    container.append(fileblock);
}

$.fn.activityRemarkReplyBoxAttachAppendImage = function (file, guid) {
    var fileType = file.type;
    var fileName = file.name;


    var container = $(this);
    var imgBlock = $("<div/>", {
        class: "system-message-attach-image-block"
    });

    if (fileType.match("image/")) {

        var fReader = new FileReader();
        fReader.readAsDataURL(file);
        fReader.onloadend = function (event) {
            //var img = document.getElementById(imgID);
            //img.src = event.target.result;
            imgBlock.css("background-image", "url(" + event.target.result + ")");
        }
    } else {
        imgBlock.html("<div style='margin-bottom:5px'><i class='fa " + getIconFileType(fileName) + " fs-26'></i></div>" + fileName);
    }

    var imgBlockContainer = $("<span/>", {
        css: {
            position: "relative",
            display: "inline-block"
        }
    });
    var removeElt = $("<i/>", {
        class: "fa fa-remove file-insert-remover",
        css: {
            display: "inline"
        },
        "data-guid": guid,
        click: function () {
            var delGuid = $(this).attr("data-guid");
            var newArrayFiles = [];
            for (var i = 0; i < activityRemarkReplyBoxAttachGlobalFiles.length; i++) {
                if (activityRemarkReplyBoxAttachGlobalFiles[i].guid != delGuid) {
                    newArrayFiles.push(activityRemarkReplyBoxAttachGlobalFiles[i]);
                }
            }
            activityRemarkReplyBoxAttachGlobalFiles = newArrayFiles;
            if (activityRemarkReplyBoxAttachGlobalFiles.length == 0) {
                $(this).closest(".system-message-comment-container-reply-file-container").hide();
            }
            $(this).parent().remove();

        }
    });
    removeElt.appendTo(imgBlockContainer);

    imgBlockContainer.append(imgBlock);
    container.find(".system-message-attach-image-block-adder").before(imgBlockContainer);
}

$.fn.activityRemarkReplyBoxPosting = function (guid, message, messageType, allowDivider) {
    var p = $("<p/>", {
        id: guid,
        class: "system-message-comment-container-remark",
        css: {
            borderBottom: allowDivider ? "1px solid #ccc" : "none",
            background: "#faf9f9"
        }
    });

    var fullname = $("<span/>", {
        class: "system-message-comment-container-remark-fullname"
    });

    $(fullname).append($("<img/>", {
        css: {
            marginTop: -2,
            width: 20,
            height: 20
        },
        src: servictWebDomainName + "images/loadmessage.gif"
    }));

    $(fullname).append($("<label/>", {
        html: "Posting Remark...",
        css: {
            marginLeft: 5,
            color: "#aaa"
        }
    }));

    var showMessage = GetMessageTypeIcon(messageType) + "<span class='remark-text-container'>" + (message == null ? "" : convertToHTMLText(message)) + "</span>"

    var messagepane = $("<p/>", {
        class: "system-message-comment-container-remark-message",
        html: message == "" ? "<br>" : showMessage
    });

    var img = $("<img/>", {
        class: "system-message-comment-container-remark-img",
        src: servictWebDomainName + "images/user.png"
    });

    p.append(img, fullname, messagepane);
    $(this).before(p);
}

$.fn.activityRemarkEditedOn = function (dateTime) {
    var obj = $(this);
    obj.find(".remark-text-updated").remove();
    var updatedPanel = $("<p/>", {
        class: "remark-text-updated",
        css: {
            marginTop: 10
        }
    });
    var updatedOn = $("<small/>", {
        css: {
            color: "#aaa"
        },
        html: "แก้ไขล่าสุดเมื่อ : " + dateTime
    });

    updatedPanel.append(updatedOn);
    obj.append(updatedPanel);
}

$.fn.activityRemarkEditor = function (datas) {
    var remarkText = datas.text;
    var remarkCode = datas.code;
    var isWip = datas.isWip;
    var editData = datas.editData;
    var aobjectlink = datas.aobjectlink;
    var obj = $(this);
    var remarkBox = obj.closest(".system-message-comment-container-remark")
    obj.hide();

    var container = $("<div/>", {
        css: {
            position: "relative"
        }
    });

    var control = $("<textarea/>", {
        class: "form-control form-control-sm",
        css: {
            "resize": "none"
        },
        rows: 7,
        text: convertToTextboxText(remarkText),
        mouseup: function (e) {
            e.stopPropagation();
        }
    });

    var applyBox = $("<span/>", {
        class: "activity-chatting-command-box",
        css: {
            right: 47,
            color: "#fff",
            background: "#009688",
            borderColor: "#009688"
        },
        click: function () {
            var showMessageType = WIPMessage(isWip);
            applyEdit(showMessageType);
        }
    });

    var applyIcon = $("<i/>", {
        class: "fa fa-check"
    });

    var cancelBox = $("<span/>", {
        class: "activity-chatting-command-box",
        css: {
            color: "#fff",
            background: "#E65041",
            borderColor: "#E65041"
        },
        click: function () {
            cancelEdit();
        }
    });

    var cancelIcon = $("<i/>", {
        class: "fa fa-remove"
    });

    cancelBox.append(cancelIcon);
    applyBox.append(applyIcon);
    container.append(control, applyBox, cancelBox);
    obj.after(container);

    obj.parent().find(".system-message-attach-files-container .file-remover,.system-message-attach-files-container .system-message-attach-image-block-adder").show();    

    function cancelEdit() {
        obj.show().next().remove();
        obj.AGWhiteLoading(false);
        obj.parent().find(".system-message-attach-files-container .file-remover,.system-message-attach-files-container .system-message-attach-image-block-adder").hide();        
        obj.parent().find(".system-message-attach-files-container .system-message-attach-files").show();
    }

    function successEdit(stamp, WIPMessage) {
        var convertDangerSign = convertDangerSignToSystem(control.val().trim());
        var returnMsg = WIPMessage + convertToHTMLText(convertDangerSign);
        obj.find(".remark-text-container").html(returnMsg);
        obj.activityRemarkEditedOn(stamp);
        cancelEdit();
    }

    function applyEdit(WIPMessage, editAttachFile) {


        function xFilter(value) {
            return value.editSeq == remarkCode;
        }

        var xFiltered = activityRemarkEditModeAttachGlobalFiles.filter(xFilter);


        if (xFiltered.length > 0) {
            remarkBox.AGWhiteLoading(true, "Uploading files");
            ajaxPostUploadFilesForEditMode();
        } else {
            remarkBox.AGWhiteLoading(true, "Saving your data");

            var _EditData = editData;
            _EditData.key.remarkKey = remarkCode;
            _EditData.key.remarkMessage = convertToSystemText(control.val());
            if (editAttachFile != undefined) {
                _EditData.key.editAttachFile = editAttachFile;
            }

            var removeFileElt = obj.parent().find(".system-message-attach-files-container .system-message-attach-files:not(:visible)");

            var removeFileKey = [];
            removeFileElt.each(function () {
                var rem = $(this).find(".file-remover");
                removeFileKey.push({
                    key: rem.attr("data-key"),
                    assetKey: rem.attr("data-asset-key")
                });
                $(this).remove();
            });

            _EditData.key.removeFileKey = JSON.stringify(removeFileKey);

            $.ajax({
                url: _EditData.url,
                type: "POST",
                data: _EditData.key,
                success: function (datas) {
                    if (datas[0].Result == "S") {
                        successEdit(datas[0].Stamp, WIPMessage);

                        remarkBox.find(".system-message-attach-files-container").remove();
                        if (datas[0].File != undefined) {
                            remarkBox.activityRemarkAppendMessageAttachment(JSON.parse(datas[0].File), true);
                        }
                    }
                    else {
                        alertMessage("เกิดข้อผิดพลาดไม่สามารถทำรายการได้ : " + datas[0].Stamp);
                        cancelEdit();
                    }
                    remarkBox.AGWhiteLoading(false);
                },
                error: function () {
                    cancelEdit();
                }
            });
        }
    }

    function ajaxPostUploadFilesForEditMode() {
        var remarkMessage = convertToSystemText(control.val());

        //$(".system-message-comment-container-reply-file-container").AGWhiteLoading(true, "Uploading files");      

        setTimeout(function () {
            // Create a formdata object and add the files
            var data = new FormData();
            var files = activityRemarkEditModeAttachGlobalFiles;
            if (files.length > 0) {
                for (var i = 0; i < files.length; i++) {
                    data.append('UploadedFiles', files[i].file, files[i].file.name);
                }
            }
            data.append("uploadType", activityRemarkReplyBoxAttachGlobalFilesUploadType);
            data.append("message", remarkMessage);
            data.append("aobj", aobjectlink);


            $.ajax({
                url: servictWebDomainName + "widget/AJAXFileUploadAPI.aspx",
                type: "POST",
                data: data,
                //async: false,
                success: function (msg) {
                    var newAttachArray = [];
                    for (var i = 0; i < activityRemarkEditModeAttachGlobalFiles.length; i++) {
                        var attachObj = activityRemarkEditModeAttachGlobalFiles[i];
                        if (remarkCode != attachObj.editSeq) {
                            newAttachArray.push(newAttachArray);
                        }
                    }
                    activityRemarkEditModeAttachGlobalFiles = newAttachArray;

                    var showMessageType = WIPMessage(isWip);
                    applyEdit(showMessageType, msg);
                    //$(".system-message-comment-container-reply-file-container").AGWhiteLoading(false);
                },
                cache: false,
                contentType: false,
                processData: false
            });
        }, 1000);
    }
}

$.fn.activityRemarkEditFileRemove = function () {
    var elt = $(this);
    elt.click(function (e) {
        $(this).closest(".system-message-attach-files").hide();
        e.stopPropagation();
    });
}

$.fn.activityRemarkHighlight = function (datas) {
    var postData = datas.postData;
    var seq = datas.remarkSeq;
    var onReplySuccess = datas.onReplySuccess;
    var aobjectlink = datas.aobjectlink;
    var pagemode = datas.pagemode;
    var callBackPostSuccess = datas.callBackPostSuccess;

    $(this).bind("mouseup", {
        onReplySuccess: onReplySuccess,
        postData: postData,
        aobjectlink: aobjectlink,
        pagemode: pagemode,
        callBackPostSuccess: callBackPostSuccess,
        remarkSeq: seq
    }, function (e) {
        e.data.postData.replyForm = e.data.remarkSeq;
        $(this).activityRemarkHighlightStartUp(e, {
            onReplySuccess: e.data.onReplySuccess,
            postData: e.data.postData,
            aobjectlink: e.data.aobjectlink,
            pagemode: e.data.pagemode,
            callBackPostSuccess: e.data.callBackPostSuccess//,
            //replyForm: e.data.remarkSeq
        });
    });

    $(document).mouseup(function (e) {
        if ($(e.target).closest(".system-message-comment-container-reply").length > 0) {
            return;
        }
    });
}

$.fn.activityRemarkHighlightStartUp = function (e, data, isFireEvent) {
    var text = "";
    //var replyForm = data.replyForm;

    if (window.getSelection) {
        text = window.getSelection().toString();
    } else if (document.selection && document.selection.type != "Control") {
        text = document.selection.createRange().text;
    }

    if (isFireEvent) {
        var name = $(this).find(".system-message-comment-container-remark-fullname").html();
        var date = $(this).find(".system-message-comment-container-remark-date").html();
        var sq = $(this).find(".system-message-comment-container-event-message .event-message-text").html();
        var q = $(this).find(".system-message-quote").html();
        var message = $(this).find(".system-message-comment-container-remark-message .remark-text-container").html();

        var textArr = [];
        textArr.push(name + " " + date + "\n");
        if (sq != undefined && sq.trim() != "") {
            textArr.push(convertToTextboxText(sq));
        }
        if (q != undefined && q.trim() != "") {
            textArr.push(convertToTextboxText(q));
        }
        if (message != undefined && message.trim() != "") {
            textArr.push(convertToTextboxText(message));
        }
        text = textArr.join("\n");
    }

    removeHighlightFunction();
    if (text.split("\n").join("").trim() != "") {

        var shortCut = $("<i/>", {
            class: "fa fa-th remark-highlight-command-shortcut",
            css: {
                left: currentMousePos.x,
                top: currentMousePos.y - 50
            },
            mouseup: function (e) {
                $(".remark-highlight-command-container").fadeIn();
                activityRemarkReplyBoxAttachGlobalFiles = [];
                $(this).remove();
                e.stopPropagation();
            }
        });

        var container = $("<div/>", {
            class: "remark-highlight-command-container ui-widget-content",
            css: {
                display: "none"
            }
        });

        var hBox = $("<div/>", {
            class: "remark-highlight-command-box"
        });

        var innerBox = $("<div/>");

        var closer = $("<i/>", {
            class: "fa fa-remove pull-right",
            css: {
                cursor: "pointer",
                marginRight: -10,
                color: "#aaa"
            },
            click: function () {
                removeHighlightFunction();
                activityRemarkReplyBoxAttachGlobalFiles = [];
            }
        });

        var header = $("<p/>", {
            html: "Highlight Functions"
        });

        var qoute = $("<div/>", {
            class: "remark-highlight-command-box-quote",
            mousedown: function (e) {
                e.stopPropagation();
            },
            mouseup: function (e) {
                e.stopPropagation();
            }
        });

        var msg = $("<span/>", {
            class: "remark-highlight-command-box-quote-message",
            html: convertToHTMLText(text)
        });

        var qType = $("<span/>", {
            class: "remark-highlight-command-box-quote-type",
            css: {
                display: "none"
            },
            html: "CUSTOM"
        });

        var refDocument = $("<span/>", {
            class: "remark-highlight-command-box-quote-refdoc",
            css: {
                display: "none"
            },
            html: data.aobjectlink
        });

        var highlightFunctionBox = $("<div/>", {
        });

        highlightFunctionBox.activityRemarkHighlightFunction({
            postData: data.postData,
            onReplySuccess: data.onReplySuccess,
            aobjectlink: data.aobjectlink,
            pagemode: data.pagemode,
            callBackPostSuccess: data.callBackPostSuccess//,
            //replyForm: replyForm
        });

        qoute.append(msg, qType, refDocument);
        innerBox.append(closer, header, qoute, highlightFunctionBox);
        hBox.append(innerBox);
        container.append(hBox);
        $("body").append(shortCut, container);

        try {
            $(container).draggable();
        }
        catch (e) { }

        if (isFireEvent) {
            shortCut.mouseup();
        }
    }

    e.stopPropagation();
}

$.fn.activityRemarkHighlightFunction = function (datas) {
    var container = $(this);
    var postData = datas.postData;
    var onReplySuccess = datas.onReplySuccess;
    var aobjectlink = datas.aobjectlink;
    var pagemode = datas.pagemode;
    var callBackPostSuccess = datas.callBackPostSuccess;
    //var replyForm = datas.replyForm;

    var tabContainer = $("<div/>", {
        class: "highlight-tab-panel-container",
        css: {
            marginTop: 20
        }
    });

    var tabHeader = $("<div/>", {
        class: "highlight-tab-panel-header"
    });
    var tabContent = $("<div/>", {
        class: "highlight-tab-panel-content"
    });

    if (postData != undefined) {
        // =================== reply box =====================
        var replyHeader = $("<span/>", {
            html: "Quote",
            click: function () {
                SwapTabPanel(this, "highlight-tab-panel-content-reply");
            }
        });
        tabHeader.append(replyHeader);

        var replyBox = $("<div/>", {
            class: "highlight-tab-panel-content-element highlight-tab-panel-content-reply",
            css: {
                display: "block"
            }
        });
        //if (replyForm) {
        //    postData.replyForm = replyForm;
        //}

        replyBox.activityRemarkReplyBox(postData, function () {
            removeHighlightFunction();
            if (onReplySuccess != undefined && onReplySuccess != false && typeof (onReplySuccess) == "function") {

                onReplySuccess({ aobjectlink: aobjectlink });
            }
        }, false, aobjectlink, undefined, undefined, undefined, undefined, callBackPostSuccess);

        //replyBox.find(".ddl-option-type-remark").hide();
        //if (replyForm) {
        //    replyBox.find(".ddl-option-type-remark").val('Reply');
        //    replyBox.find(".ddl-option-type-remark").prop("disabled", true);
        //}
        tabContent.append(replyBox);

        // =================== create ref activity box =====================

        //onReplySuccessWithCreateActivity = onReplySuccess;
        //postDataWithCreateActivity = postData;

        //var createActHeader = $("<span/>", {
        //    html: "Create Activity",
        //    click: function () {
        //        SwapTabPanel(this, "highlight-tab-panel-content-activity");
        //    }
        //});
        //tabHeader.append(createActHeader);

        //var createActbox = $("<div/>", {
        //    class: "highlight-tab-panel-content-element highlight-tab-panel-content-activity"
        //});


        //createActbox.activityRemarkActivityCreator({           
        //    aobjectlink: aobjectlink,
        //    pagemode: pagemode
        //});

        //tabContent.append(createActbox);
    }


    tabContainer.append(tabHeader, tabContent);
    container.append(tabContainer);
    container.find(".highlight-tab-panel-header span").first().addClass("is-active");
    container.find(".highlight-tab-panel-content-element").first().show();

    function SwapTabPanel(obj, className) {
        var elt = $(obj);
        container.find(".highlight-tab-panel-content-element").hide();
        container.find(".highlight-tab-panel-header span").removeClass("is-active");
        elt.addClass("is-active");
        $("." + className).show();
        $("." + className).find("textarea").focus();
    }
}

$.fn.activityRemarkActivityCreator = function (datas) {
    var aobjectlink = datas.aobjectlink;
    var pagemode = datas.pagemode;
    var guid = generateShortGUID();
    var container = $(this);

    var textArrea = $("<textarea/>", {
        class: "form-control",
        id: "highlight-tab-panel-content-activity-from-control" + guid,
        rows: 4
    });
    container.append(textArrea);

    var table = $("<table/>", {
        css: {
            width: "100%",
            resize: "vertical",
            marginTop: 10
        }
    });
    container.append(table);

    var row = $("<tr/>");
    table.append(row);

    var col1 = $("<td/>", {
        css: {
            width: "33.33%",
            //width: "25%",
            padding: "0 5px",
            paddingLeft: 0
        }
    });
    var col2 = $("<td/>", {
        css: {
            width: "33.33%",
            //width: "25%",
            padding: "0 5px"
        }
    });
    var col3 = $("<td/>", {
        css: {
            width: "33.34%",
            //width: "25%",
            padding: "0 5px",
            paddingRight: 0
        }
    });
    var col4 = $("<td/>", {
        css: {
            width: "25%",
            padding: "0 5px",
            paddingRight: 0,
            display: "none"
        }
    });
    row.append(col1, col2, col3, col4);

    var btnTask = $("<span/>", {
        class: "btn btn-default",
        css: {
            width: "100%",
            fontWeight: 600
        },
        html: "<i class='fa fa-file-text fa-fwd' style='color:#00B294'></i>&nbsp;Task",
        click: function () {
            openFrameMasterPage("task", aobjectlink, pagemode);
        }
    });
    col1.append(btnTask);

    var btnMeeting = $("<span/>", {
        class: "btn btn-default",
        css: {
            width: "100%",
            fontWeight: 600
        },
        html: "<i class='fa fa-handshake-o fa-fwd' style='color:#EA5041'></i>&nbsp;Meeting",
        click: function () {
            openFrameMasterPage("meeting", aobjectlink, pagemode);
        }
    });
    col2.append(btnMeeting);

    var btnNote = $("<span/>", {
        class: "btn btn-default",
        css: {
            width: "100%",
            fontWeight: 600
        },
        html: "<i class='fa fa-sticky-note fa-fwd' style='color:#F1C513'></i>&nbsp;Note",
        click: function () {
            openFrameMasterPage("note", aobjectlink, pagemode);
        }
    });
    col3.append(btnNote);

    var btnSaleTask = $("<span/>", {
        class: "btn btn-primary",
        css: {
            width: "100%",
            fontWeight: 600
        },
        html: "<i class='fa fa-briefcase' style='color: #fff;'></i>&nbsp;Sale Task",
        click: function () {
            openFrameMasterPage("sale", aobjectlink, pagemode);
        }
    });
    col4.append(btnSaleTask);



    function openFrameMasterPage(type, aobjectlink, pagemode) {
        var mode = "";
        if (type == "note")
            mode = "createNote";
        if (type == "task")
            mode = "createTask";
        if (type == "meeting")
            mode = "createMeeting";
        if (type == "sale")
            mode = "createSales";
        var src = "";
        if (aobjectlink != null && aobjectlink != undefined && aobjectlink != "" && pagemode == "ACTIVITY") {
            src = '/widget/PopupCreateActivityReDesign.aspx?isQuote=true&mode=' + mode + '&functionNumber=' + guid + '&aobj=' + aobjectlink + '&rowkey=quoteActivity';
        }
        else {
            src = '/widget/PopupCreateActivityReDesign.aspx?isQuote=true&mode=' + mode + '&functionNumber=' + guid;
        }

        if (window.parent.length > 0) {
            window.parent.createActivityStart(src);
        } else {
            var ifca = document.getElementById("iframeCreateActivity");
            ifca.src = src;
        }
        //$("#iframeCreateActivity").prop("src", src);
        //agroLoading(true);
    }
}

function getIconFileType(file) {
    var arrFile, fileType;

    try {
        arrFile = file.split('.');
        fileType = arrFile[arrFile.length - 1];
    } catch (e) { }

    var arrWordType = ["doc", "dot", "wbk", "docx", "docm", "dotx", "dotm", "docb"];
    var arrExcelType = ["xls", "xlt", "xlm", "xlsx", "xlsm", "xltx", "xltm"];
    var arrPowerpoitType = ["ppt", "pot", "pptx", "pptm", "potx", "potm", "ppam", "ppsx", "ppsm", "sldx", "sldm"];
    var arrPdfType = ["pdf"];
    var arrVideoType = ["mp4", "webm", "flv", "vob", "avi", "wmv", "amv", "m4p", "3gp"];
    var arrAudioType = ["mp3", "mp2"];
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