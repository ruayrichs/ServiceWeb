var onReplySuccessWithCreateActivity;
var onReplySuccessWithAnswerOnlineForm;
var postDataWithCreateActivity;
var activityRemarkReplyBoxAttachGlobalFiles = [];
var activityRemarkReplyBoxAttachGlobalFilesUploadType = "";
var currentMousePos = { x: -1, y: -1 };

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

function WIPMessage(isWip) {
    if (isWip) {
        return '<input type="button" class="btn  btn-warning  btn-xs btn-wip" value="WIP" style="width:50px;margin-right:10px;margin-bottom:10px;" />';
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

    var rest = WIPMessage(type == "WIP");
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

        rest += "<span class='remark-text-container'></span>";
        rest += "<a class='' href='/timeattendance/ActivityManagementReDesign.aspx?aobj=" + strRight + "&snaid=" + strRight.substring(0, 7) + "' target='_blank'><i class='fa fa-external-link-square' style='margin-right:10px'></i>" + strLeft + "</a>";
        allowEdit = false;
    }
    else {
        rest += "<span class='remark-text-container'>" + convertToHTMLText(message) + "</span>";
    }

    if (messagePlain != "") {
        rest = convertToHTMLText(messagePlain) + "<br>" + rest;
    }

    return {
        message: rest,
        allowEdit: allowEdit
    };
}

function ajaxPostUploadFiles(event, datas) {
    var finalPostData = datas.finalPostData;
    var panelReply = datas.panelReply;
    var onReplySuccess = datas.onReplySuccess;
    var guid = datas.guid;
    var aobjectlink = datas.aobjectlink;

    $(".system-message-comment-container-reply-file-container").AGWhiteLoading(true, "Uploading files");
    //event.stopPropagation(); // Stop stuff happening
    //event.preventDefault(); // Totally stop stuff happening           

    setTimeout(function () {
        // Create a formdata object and add the files
        var data = new FormData();
        var files = activityRemarkReplyBoxAttachGlobalFiles;
        if (files.length > 0) {
            for (var i = 0; i < files.length; i++) {
                data.append('UploadedFiles', files[i], files[i].name);
            }
        }
        data.append("uploadType", activityRemarkReplyBoxAttachGlobalFilesUploadType);
        data.append("message", finalPostData.key.remarkMessage);
        data.append("aobj", aobjectlink);
        activityRemarkReplyBoxAttachGlobalFiles = [];

        $.ajax({
            url: "/widget/AJAXFileUploadAPI.aspx",
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
                    guid: guid,
                    attachFileKey: msg
                });
            },
            cache: false,
            contentType: false,
            processData: false
        });
    },1000);
}

function ajaxPostDataRemark(datas) {
    var finalPostData = datas.finalPostData;
    var panelReply = datas.panelReply == undefined ? $("body") : datas.panelReply;
    var onReplySuccess = datas.onReplySuccess;
    var guid = datas.guid;
    var attachFileKey = datas.attachFileKey == undefined ? "" : datas.attachFileKey;

    finalPostData.key.attachFileKey = attachFileKey;
    finalPostData.key.remarkMessage = finalPostData.key.remarkMessage.split('<').join('&lt;').split('>').join('&gt;');
    finalPostData.key.quoteMessage = finalPostData.key.quoteMessage.split('<').join('&lt;').split('>').join('&gt;');


    $.ajax({
        url: finalPostData.url,
        type: "POST",
        data: finalPostData.key,
        success: function (datas) {
            panelReply.AGWhiteLoading(false);
            if (datas.message == "S") {
                if (onReplySuccess != false && typeof (onReplySuccess) == "function") {
                    onReplySuccess({
                        postingGUID: guid
                    });
                }
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
    var elt = $("#create-activity-frame-" + guid);
    var quoteElt = elt.closest(".remark-highlight-command-box");
    var messageQuote = quoteElt.find(".remark-highlight-command-box-quote-message").html();

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
        onReplySuccess: onReplySuccess
    });


    $(elt).fadeOut(function () {
        removeHighlightFunction();
    });
}

function highlightCloseCreateActivity(guid) {
    $("#create-activity-frame-" + guid).fadeOut(function () {
        $("#script-create-activity-" + guid).remove();
    });
}

function highlightShowCreateActivity(guid) {
    var elt = $("#create-activity-frame-" + guid);
    var quoteElt = elt.closest(".remark-highlight-command-box");
    var messageQuote = quoteElt.find(".remark-highlight-command-box-quote-message").html();
    elt.fadeIn();
    return convertToSystemText(messageQuote);
}

function highlightShowCreateActivityGetDocnumber(guid) {
    var elt = $("#create-activity-frame-" + guid);
    var quoteElt = elt.closest(".remark-highlight-command-box");
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

function convertToSystemText(input) {
    var rest = input;
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
    downloadForm.append(fileName, filePath);
    downloadForm.appendTo($("body"));
    downloadForm.submit();
    downloadForm.remove();
}

$.fn.activityRemark = function (datas) {
    activityRemarkReplyBoxAttachGlobalFiles = [];
    var allowDivider = datas.allowDivider == undefined ? true : datas.allowDivider;
    var allowHighlight = datas.allowHighlight == undefined ? true : datas.allowHighlight;
    var isContinue = datas.isContinue == undefined ? false : datas.isContinue;
    var onContinue = datas.onContinue;
    var onChooseFavorite = datas.onChooseFavorite;
    var onRemoveFavorite = datas.onRemoveFavorite;
    var callbackData = datas.callbackData;
    var onRendered = datas.onRendered;
    var getData = datas.getData;
    var postData = datas.postData;
    var favoriteData = datas.favoriteData;
    var showFavorite = datas.showFavorite;
    var editData = datas.editData;
    var getOnlineForm = datas.getOnlineForm;
    var aobjectlink = datas.aobjectlink;
    var pagemode = datas.pagemode;
    var islazyLoad = datas.islazyLoad == undefined ? false : datas.islazyLoad;
    var isEasyMode = datas.isEasyMode == undefined ? false : datas.isEasyMode;
    var loadMoreSeq = datas.loadMoreSeq == undefined ? "" : datas.loadMoreSeq;
    var isLoadMore = datas.isLoadMore == undefined ? false : datas.isLoadMore;
    var lazyLoadTotal = datas.lazyLoadTotal == undefined ? 0 : parseInt(datas.lazyLoadTotal);
    var myImagePath = datas.myImagePath == undefined ? "" : datas.myImagePath;
    var isReplySuccess = datas.isReplySuccess == undefined ? false : datas.isReplySuccess;
    var reverse = datas.reverse == undefined ? false : datas.reverse;

    getData.key.seq = loadMoreSeq;

    var container = $(this);
    $.ajax({
        url: getData.url,
        data: getData.key,
        success: function (data) {


            if (data.ActivityRemark != undefined) {
                lazyLoadTotal = data.Totals;
                data = data.ActivityRemark;
            }

            container.show();
            container.activityRemarkBuilder({
                isLoadMore:isLoadMore,
                data: data,
                postData: postData,
                allowDivider: allowDivider,
                allowHighlight: allowHighlight,
                postData: postData,
                favoriteData: favoriteData,
                showFavorite:showFavorite,
                editData: editData,
                getOnlineForm:getOnlineForm,
                aobjectlink: aobjectlink,
                pagemode: pagemode,
                islazyLoad: islazyLoad,
                isEasyMode: isEasyMode,
                myImagePath:myImagePath,
                lazyLoadTotal: lazyLoadTotal,
                isContinue: isContinue,
                isReplySuccess: isReplySuccess,
                reverse:reverse,
                onChooseFavorite: onChooseFavorite,
                onRemoveFavorite:onRemoveFavorite,
                onReplySuccess: function (result) {
                    container.activityRemark({
                        allowDivider: allowDivider,
                        allowHighlight: allowHighlight,
                        isContinue: true,
                        onContinue: onContinue,
                        callbackData: result,
                        getData: getData,
                        postData: postData,
                        favoriteData: favoriteData,
                        showFavorite: showFavorite,
                        onChooseFavorite: onChooseFavorite,
                        onRemoveFavorite:onRemoveFavorite,
                        editData: editData,
                        aobjectlink: aobjectlink,
                        pagemode: pagemode,
                        myImagePath: myImagePath,
                        lazyLoadTotal: lazyLoadTotal,
                        islazyLoad: islazyLoad,
                        isReplySuccess: true,
                        reverse: reverse
                    });
                },
                onLoadMore: function (curSeq) {
                    container.activityRemark({
                        allowDivider: allowDivider,
                        allowHighlight: allowHighlight,
                        getData: getData,
                        postData: postData,
                        favoriteData: favoriteData,
                        showFavorite: showFavorite,
                        onChooseFavorite: onChooseFavorite,
                        onRemoveFavorite:onRemoveFavorite,
                        editData: editData,
                        aobjectlink: aobjectlink,
                        pagemode: pagemode,
                        loadMoreSeq: curSeq,
                        isLoadMore: true,
                        islazyLoad: true,
                        lazyLoadTotal: lazyLoadTotal,
                        reverse: reverse
                    });
                }
            });
            if (isContinue) {
                if (onContinue != undefined && typeof (onContinue) == "function") {
                    onContinue(aobjectlink);
                }
            }
            if (isReplySuccess) {
                container.find(".system-message-comment-container-reply textarea").focus();
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

$.fn.activityRemarkBuilder = function (objectData) {
    var datas = objectData.data;
    var _message = objectData.focusMessage;
    var _dateTime = objectData.focusMessageOn;
    var pointIndex = objectData.pointIndex;
    var containerCss = objectData.containerCss;
    var allowDivider = objectData.allowDivider == undefined ? true : objectData.allowDivider; 
    var allowHighlight = objectData.allowHighlight == undefined ? true : objectData.allowHighlight;
    var postData = objectData.postData;
    var favoriteData = objectData.favoriteData;
    var showFavorite = objectData.showFavorite;
    var onChooseFavorite = objectData.onChooseFavorite;
    var onRemoveFavorite = objectData.onRemoveFavorite;
    var editData = objectData.editData;
    var aobjectlink = objectData.aobjectlink;
    var getOnlineForm = objectData.getOnlineForm;

    var pagemode = objectData.pagemode;
    var islazyLoad = objectData.islazyLoad;
    var isEasyMode = objectData.isEasyMode;

    var isLoadMore = objectData.isLoadMore;
    var onLoadMore = objectData.onLoadMore;
    var lazyLoadTotal = objectData.lazyLoadTotal;
    var myImagePath = objectData.myImagePath;

    var isContinue = objectData.isContinue;
    var isReplySuccess = objectData.isReplySuccess;

    var reverse = objectData.reverse;

    var onReplySuccess = objectData.onReplySuccess == undefined ? false : objectData.onReplySuccess;
    var enableEditor = objectData.enableEditor == undefined ? true : objectData.enableEditor;
    var enableReply = objectData.enableReply == undefined ? true : objectData.enableReply;
    var container = $(this);
    var isPointed = false;
    var pointer = null;

    if (containerCss != undefined) {
        container.css(containerCss);
    }


    var messageStorage = $(container).find(".system-message-comment-container-remark-storage");
    if (messageStorage.length == 0) {
        messageStorage = $("<div/>", {
            class: "system-message-comment-container-remark-storage"
        });
        container.append(messageStorage);
    }

    var messageDisplay;
    if (isLoadMore || isReplySuccess) {
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
        messageStorage.find(".system-message-comment-container-remark[data-remark-seq='" + datas[i].Key + "']").remove();
        var p = $("<p/>", {
            "data-remark-seq": datas[i].Key,
            class: "system-message-comment-container-remark"
        });

        if (allowHighlight) {
            p.activityRemarkHighlight({
                postData: postData,
                remarkSeq: datas[i].Key,
                onReplySuccess: onReplySuccess,
                aobjectlink: aobjectlink,
                pagemode: pagemode
            });
        }

        var dateTime = $("<span/>", {
            class: "system-message-comment-container-remark-date",
            html: datas[i].CreatedOn
        });

        var fullname = $("<span/>", {
            class: "system-message-comment-container-remark-fullname",
            html: datas[i].CreatorFullname
        });

        fullname.prepend("<i class='fa fa-caret-left'></i>");

        var MessageBuilder = RemarkMessageTextBuilder(datas[i].MessageText, datas[i].MessageType);
        
        var message = $("<p/>", {
            class: "system-message-comment-container-remark-message",
            html: MessageBuilder.message
        });

        var img = $("<img/>", {
            class: "system-message-comment-container-remark-img",
            src: datas[i].Image
        });

        var isMyRemark = datas[i].CreatorLinkID == datas[i].MyLinkID;

        if (isMyRemark) {

            p.addClass("system-message-comment-container-remark-owner");

            if (enableEditor && editData != undefined && MessageBuilder.allowEdit) {
                var remarkEditor = $("<i/>", {
                    class: "fa fa-pencil pull-right",
                    css: {
                        color: "#aaa",
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
                        editData: editData
                    });
                });
                message.append(remarkEditor);
            }
        }

        if (showFavorite) {

            var countFav = datas[i].CountFavorite == undefined ? 0 : datas[i].CountFavorite;
            var isFav = datas[i].IsFavorite == undefined ? 0 : datas[i].IsFavorite;

            var fav = $("<div/>", {
                class: "system-message-comment-container-remark-fav"
            });
            p.append(fav);

            var bookmark = $("<i/>", {
                class: "fa fa-bookmark " + (isFav ? "active" : ""),
                "data-fav-seq": datas[i].Key
            });
            fav.append(bookmark);

            var score = $("<span/>",{
                class: "system-message-comment-container-remark-score " + (countFav > 0 ? "active" : ""),
                html: "(" + countFav + ")"
            });

            fav.append(score);

            if (favoriteData != undefined) {
                bookmark.bind("click", {
                    favoriteData: favoriteData
                }, function (e) {
                    var datas = e.data.favoriteData.key;
                    datas.messageSeq = $(this).attr("data-fav-seq");
                    $.ajax({
                        url: e.data.favoriteData.url,
                        type: "post",
                        data: datas,
                        success: function (rest) {
                            console.log(rest);
                            if (rest.message == "S") {
                                var favBook = $("[data-fav-seq='" + rest.seq + "']");
                                favBook.addClass("active");
                                favBook.parent().find(".system-message-comment-container-remark-score").html("(" + rest.count + ")")
                                    .addClass("active");
                            }
                        }
                    });
                });
            }

            if (onChooseFavorite != undefined && typeof (onChooseFavorite) == "function") {
                var btnAddFav = $("<span/>", {
                    class: "system-message-comment-container-remark-fav-add btn btn-sm btn-primary",
                    html:"<i class='fa fa-plus'></i> Choose"
                });

                message.append(btnAddFav);

                btnAddFav.bind("click", {
                    onChooseFavorite: onChooseFavorite,
                    seq: datas[i].Key
                }, function (e) {
                    e.data.onChooseFavorite(e.data.seq);
                });
            }

            if (onRemoveFavorite != undefined && typeof (onRemoveFavorite) == "function") {
                var btnRemoveFav = $("<span/>", {
                    class: "system-message-comment-container-remark-fav-add btn btn-sm btn-danger",
                    html: "<i class='fa fa-remove'></i> remove"
                });

                message.append(btnRemoveFav);

                btnRemoveFav.bind("click", {
                    onRemoveFavorite: onRemoveFavorite,
                    seq: datas[i].Key
                }, function (e) {
                    e.data.onRemoveFavorite(e.data.seq);
                });
            }
        }



        p.append(img, fullname, dateTime);

        if (datas[i].QuoteMessage != undefined && datas[i].QuoteMessage != "") {
            var quoteMessage = $("<div/>", {
                class: "system-message-quote",
                html: "<div class='system-message-quote-inner'>" + convertToHTMLText(datas[i].QuoteMessage) + "</div>"
            });
            p.addClass("has-quote");
            p.append(quoteMessage);
        }
        
        p.append(message);

        if (datas[i].MessageType == "ONLINEFORM")
        {
            var answer = "<a style='color:orange;display:block;margin-bottom:10px' target='_blank' href='/questionnaire/viewansquestionnairedetail.aspx?id=" + datas[i].AnswerFormKey + "'><i class='fa fa-file-text-o'></i> ตอบแบบฟอร์มออนไลน์ : " + datas[i].AnswerFormName + "</a>";
            p.append(answer);
        }

        if (datas[i].Files != null && datas[i].Files != undefined && datas[i].Files.length > 0) {
            var row = $("<div/>", {
                class:"system-message-attach-files-container"
            });
            p.append(row);
            var files = datas[i].Files;
            for (var x = 0; x < files.length; x++) {
                var file = files[x];
                
                var attachFiles;
                if (file.Type == "IMAGE") {
                    var a = $("<a/>", {
                        class: "system-message-attach-files",
                    });
                    var col = $("<div/>", {
                        class: "system-message-attach-image-block", 
                        "data-image":file.Url,
                        css: {
                            backgroundImage: "url('" + file.Url + "')"
                        }
                    });
                    row.append(a);
                    a.append(col);
                    attachFiles = a;
                }
                else {
                    var fileName = file.Url.split("/")[file.Url.split("/").length - 1];
                    var a = $("<a/>", {
                        class:"system-message-attach-files",
                        "data-path": file.Path,
                        "data-name": fileName,
                        click: function () {
                            try {
                                ActivityDownloadFileForm(this);
                                return false;
                            } catch (e) { }
                        }
                    });
                    var col = $("<div/>", {
                        class: "system-message-attach-file-block",
                        html: "<i class='fa fa-file-text-o'></i> " + fileName
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

        try {
            p.aGepeGalleryContainer(MessageBuilder.message);
        }
        catch (e) { }
    }

    messageStorage.find(".system-message-comment-container-remark-more").remove();

    if (islazyLoad && datas.length > 0 && messageStorage.find(".system-message-comment-container-remark").length < lazyLoadTotal) {
        var more = $("<div/>", {
            class: "system-message-comment-container-remark-more",
            html: "ดูความคิดเห็นก่อนหน้า",
            css: {
            },
            "data-current-seq": datas[0].Key
        });
        messageDisplay.prepend(more);
        $(more).activityRemarkLoadMore(onLoadMore);
    }

    if (enableReply && postData != undefined && container.find(".system-message-comment-container-reply").length == 0) {
        container.activityRemarkReplyBox(postData, onReplySuccess, allowDivider, aobjectlink, isEasyMode, myImagePath, getOnlineForm, reverse);
    }
    else {
        container.append(container.find(".system-message-comment-container-reply"));
    }

    if (reverse) {
        container.activityRemarkReverse();
    }

    return pointer;

}

$.fn.activityRemarkReverse = function () {
    var container = $(this);
    var replyBox = $(".system-message-comment-container-reply");
    container.prepend(replyBox);
    var displayElt = container.find(".system-message-comment-container-remark-display");
    displayElt.css({
        marginTop:10
    });
    replyBox.css({
        marginTop:-5
    });
    var allMessage = displayElt.find(".system-message-comment-container-remark");
    var seqArray = [];
    $(allMessage).each(function () {
        seqArray.push($(this).attr("data-remark-seq"));
    });
    seqArray = seqArray.sort(function (a, b) {
        var valueA = parseInt(a);
        var valueB = parseInt(b);
        if (valueA < valueB) {
            return 1;
        }
        if (valueA > valueB) {
            return -1;
        }
        return 0;
    });
    for (var i = 0; i < seqArray.length; i++) {
        displayElt.append($(".system-message-comment-container-remark[data-remark-seq='" + seqArray[i] + "']"))
    }
    displayElt.append(displayElt.find(".system-message-comment-container-remark-more"));
}

$.fn.activityRemarkLoadMore = function (onLoadMore) {
    $(this).bind("click", {
        onLoadMore: onLoadMore
    },function (e) {
        $(this).html("กำลังโหลดข้อความก่อนหน้า...").css("color","#777");
        var onLoadMore = e.data.onLoadMore;
        var curSeq = $(this).attr("data-current-seq");
        if (onLoadMore != undefined && typeof (onLoadMore) == "function") {
            onLoadMore(curSeq);
        }
    });
}

$.fn.activityRemarkReplyBox = function (postData, onReplySuccess, allowDivider, aobjectlink, isEasyMode, myImagePath, getOnlineForm, reverse) {
   
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
        class: "form-control",
        placeholder:"Message here...",
        rows:4
    });

    var fileContainer = $("<div/>", {
        class: "system-message-comment-container-reply-file-container"
    });

    var subCon = $("<p/>", {
        class: "text-right",
        css: {
            marginTop: 10,
            marginBottom: 0,
            paddingBottom:43
        }
    });


    var submit = $("<span/>", {
        class: "btn btn-primary pull-right activity-chatting-fullmode",
        html: "เพิ่มความคิดเห็น",
        css: {
            boxShadow:"none"
        }
    });

    submit.bind("click", {
        container:container,
        postData: postData,
        pReply: pReply,
        textArea: textArea,
        onReplySuccess: onReplySuccess,
        allowDivider: allowDivider,
        aobjectlink: aobjectlink
    }, function (e) {
        var elt = $(this);
        var commentRemark = e.data.textArea.val().trim();
        if (commentRemark == "" && activityRemarkReplyBoxAttachGlobalFiles.length == 0) {
            alert("กรุณาระบุความคิดเห็น");
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
                    commentRemark = commentRemark + " (" + activityRemarkReplyBoxAttachGlobalFiles.length + " Attachments)";
                }

                if(isQuote){
                    e.data.pReply.AGWhiteLoading(false);
                    e.data.pReply.AGWhiteLoading(true, "Posting remark");
                }
                else {
                    messageQuote = "";
                    quoteType = "";
                    e.data.pReply.activityRemarkReplyBoxPosting(guid, commentRemark, remarkType, e.data.allowDivider, reverse);
                }

                var h = $(e.data.container).height();
                if (parent != undefined && typeof (parent.setheightContainerIFrame) == "function") {
                    parent.setheightContainerIFrame(h + 70);
                }

                e.data.textArea.val("");

                var _PostData = postData;
                _PostData.key.isQuote = isQuote;
                _PostData.key.quoteMessage = convertToSystemText(messageQuote);
                _PostData.key.quoteType = quoteType;
                _PostData.key.remarkMessage = commentRemark;
                _PostData.key.remarkType = remarkType;
                _PostData.key.sendMail = elt.parent().find("input").prop("checked");

                if ($("#activity-remark-file-form").length > 0 && activityRemarkReplyBoxAttachGlobalFiles.length > 0) {
                    ajaxPostUploadFiles(e,{
                        finalPostData: _PostData,
                        panelReply: e.data.pReply,
                        onReplySuccess: e.data.onReplySuccess,
                        guid: guid,
                        aobjectlink: aobjectlink
                    });
                }
                else {
                    ajaxPostDataRemark({
                        finalPostData: _PostData,
                        panelReply: e.data.pReply,
                        onReplySuccess: e.data.onReplySuccess,
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
        class:"activity-chatting-fullmode",
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


    var imageAttach = $("<i/>", {
        class: "reply-attach fa fa-picture-o pull-right",
        title: "Attach pictures"
    });

    imageAttach.bind("click", {
        fileContainer: fileContainer
    }, function (e) {
        e.data.fileContainer.activityRemarkReplyBoxAttach("IMAGE");
    });


    var fileAttach = $("<i/>", {
        class: "reply-attach fa fa-paperclip pull-right",
        title: "Attach files",
        css: {
            marginRight:10
        }
    });

    fileAttach.bind("click", {
        fileContainer: fileContainer
    }, function (e) {
        e.data.fileContainer.activityRemarkReplyBoxAttach("FILE");
    })

    // abd: donot show comment drop down 
    var select = $("<select/>", {
        class: "form-control pull-right activity-chatting-fullmode",
        css: {
            //  display: "inline-block",
            display: "none",
            width: 200,
            marginRight: 20,
            color: "#aaa",
            cursor:"pointer"
        }
    });
 
    select.append("<option value='REMARK'>Comment</option>");
    //select.append("<option value='WIP'>Work In Process</option>");
    // subCon.append(spanMail, submit, select);

    subCon.append(submit, select);
 
    if (aobjectlink != undefined) {
        subCon.append(imageAttach, fileAttach);
    }

    var onlineFormAttach = null;

    //========================== Activity Online Form =========================
    if (getOnlineForm != undefined && typeof ("function")) {

        onlineFormAttach = $("<div/>", {
            class: "reply-attach pull-right dropup",
            title: "Attach files",
            css: {
                marginRight: 10,
                display:"none"
            }
        });

        var innerContainer = $("<div/>", {
            css: {
                position:"relative"
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


    pReply.append(textArea,fileContainer, subCon);
    container.append(pReply);

    if (myImagePath != "") {
        var img = $("<img/>", {
            class: "system-message-comment-container-remark-img easy-mode-" + isEasyMode,
            src: myImagePath,
            css: {
                position: "absolute"
            }
        });
        pReply.append(img).css({
            paddingTop:5
        });
    }

    if (isEasyMode) {
        $(".activity-chatting-fullmode").hide();
        textArea.attr("rows", 1).css({
            paddingRight: 70,
            paddingLeft:10,
            width: "100%",
            borderRadius: 5,
            borderColor:"#ccc",
        }).removeClass("form-control");

        pReply.css({
            position:"relative"
        });

        subCon.css({
            padding:0,
            position: "absolute",
            bottom: 20,
            right:10
        });

        $(".system-message-comment-container-reply .reply-attach").css({
            fontSize:16
        });

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
            href:"#",
            html: "ตอบแบบฟอร์ม : " + data.Subject
        });

        a.bind("click", {
            datas: data
        }, function (e) {
            try{
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

$.fn.activityRemarkReplyBoxAttach = function (uploadType) {
    var container = $(this);
    var iconAttach = container.closest(".system-message-comment-container-reply").find(".reply-attach");
    var commandBox = $("<div/>", {
        class: "text-right"
    });

    var fileBox = $("<div/>", {
        css: {
            padding: 10,
            borderTop: "1px solid #ccc",
            marginTop:10
        }
    });

    container.html("");
    container.append(commandBox, fileBox);

    var addFile = $("<span/>", {
        css: {
            padding: "8px 10px",
            background: "#ccc",
            cursor:"pointer"
        },
        html:"<i class='fa fa-plus'></i> Add " + uploadType.toLowerCase()
    });

    var canCel = $("<span/>", {
        css: {
            padding: "8px 10px",
            background: "#ccc",
            marginLeft: 10,
            cursor: "pointer"
        },
        html: "<i class='fa fa-remove'></i> Cancel"
    });

    canCel.bind("click", {
        container: container,
        iconAttach: iconAttach
    }, function (e) {
        activityRemarkReplyBoxAttachGlobalFiles = [];
        e.data.container.html("").hide();
        e.data.iconAttach.show();
    });

    commandBox.append(addFile, canCel);

    var form = $("#activity-remark-file-form");
    if (form.length == 0) {
        form = $("<form method='post' style='padding-left:100px' action='/widget/AJAXFileUploadAPI.aspx' id='activity-remark-file-form' enctype='multipart/form-data' />");
    }
    form.html("").hide();

    var fileUpload = $("<input type='file' id='FilePath' name='FilePath' multiple/>");

    if (uploadType == "IMAGE") {
        fileUpload.attr("accept", "image/*");
    }
    activityRemarkReplyBoxAttachGlobalFilesUploadType = uploadType;

    form.append(fileUpload);
    $("body").append(form);

    var form = document.getElementById('activity-remark-file-form');

    // Add events
    $(fileUpload).bind("change", {
        container: container,
        iconAttach: iconAttach,
        uploadType: uploadType
    }, function (e) {
        var fLength = $(fileUpload).get(0).files.length;
        if (fLength > 0) {
            for (var i = 0; i < $(fileUpload).get(0).files.length; i++) {
                activityRemarkReplyBoxAttachGlobalFiles.push($(fileUpload).get(0).files[i]);
                if (e.data.uploadType == "IMAGE") {
                    fileBox.activityRemarkReplyBoxAttachAppendImage($(fileUpload).get(0).files[i]);
                }
                else {
                    fileBox.activityRemarkReplyBoxAttachAppendFile($(fileUpload).get(0).files[i].name);
                }
            }
            e.data.container.show();
            e.data.iconAttach.hide();
        }
    });

    addFile.click(function () {
        $(fileUpload).click();
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

$.fn.activityRemarkReplyBoxAttachAppendImage = function (file) {
    var container = $(this);
    var imgBlock = $("<div/>", {
        class: "system-message-attach-image-block",
        css: { 
            width: 100,
            height:100
        }
    });
    var fReader = new FileReader();
    fReader.readAsDataURL(file);
    fReader.onloadend = function (event) {
        //var img = document.getElementById(imgID);
        //img.src = event.target.result;
        imgBlock.css("background-image", "url(" + event.target.result + ")");
    }
    container.append(imgBlock);
}

$.fn.activityRemarkReplyBoxPosting = function (guid, message, messageType, allowDivider, reverse) {
    var p = $("<p/>", {
        id:guid,
        class: "system-message-comment-container-remark system-message-comment-container-remark-posting"
    });

    var fullname = $("<span/>", {
        class: "system-message-comment-container-remark-fullname"
    });

    $(fullname).append($("<img/>", {
        css: {
            marginTop:-2,
            width: 20,
            height: 20
        },
        src: "/images/loadmessage.gif"
    }));

    $(fullname).append($("<label/>", {
        html:"Posting Remark...",
        css: {
            marginLeft: 5,
            color:"#aaa"
        }
    }));

    var showMessage = WIPMessage("WIP" == messageType) + "<span class='remark-text-container'>" + (message == null ? "" : convertToHTMLText(message)) + "</span>"

    var messagepane = $("<p/>", {
        class: "system-message-comment-container-remark-message",
        html: message == "" ? "<br>" : showMessage
    });

    var img = $("<img/>", {
        class: "system-message-comment-container-remark-img",
        src: "/images/user.png"
    });

    
    p.append(img, fullname, messagepane);


    if (reverse) {
        $(this).after(p);
    } else {
        $(this).before(p);
    }
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
    var obj = $(this);
    obj.hide();

    var container = $("<div/>", {
        css: {
            position: "relative"
        }
    });

    var control = $("<textarea/>", {
        class: "form-control",
        rows: 7,
        text: convertToTextboxText(remarkText),
        mouseup: function (e) {
            e.stopPropagation();
        }
    });

    var applyBox = $("<span/>", {
        class: "activity-chatting-command-box",
        css: {
            right: 47
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

    obj.parent().find(".system-message-attach-files-container .file-remover").show();

    function cancelEdit() {
        obj.show().next().remove();
        obj.AGWhiteLoading(false);
        obj.parent().find(".system-message-attach-files-container .file-remover").hide();
        obj.parent().find(".system-message-attach-files-container .system-message-attach-files").show();
    }

    function successEdit(stamp, WIPMessage) {
        var convertDangerSign = convertDangerSignToSystem(control.val().trim());
        var returnMsg = WIPMessage + convertToHTMLText(convertDangerSign);
        obj.find(".remark-text-container").html(returnMsg);
        obj.activityRemarkEditedOn(stamp);
        cancelEdit();
    }

    function applyEdit(WIPMessage) {
        obj.AGWhiteLoading(true);
        var _EditData = editData;
        _EditData.key.remarkKey = remarkCode;
        _EditData.key.remarkMessage = convertToSystemText(control.val())

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
                if(datas[0].Result == "S"){
                    successEdit(datas[0].Stamp, WIPMessage);
                }
                else {
                    alertMessage("เกิดข้อผิดพลาดไม่สามารถทำรายการได้ : " + datas[0].Stamp);
                    cancelEdit();
                }
                
            },
            error: function () {
                cancelEdit();
            }
        });
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

    $(this).bind("mouseup", {
        onReplySuccess: onReplySuccess,
        postData: postData,
        aobjectlink: aobjectlink,
        pagemode: pagemode
    }, function (e) {
        var text = "";
        if (window.getSelection) {
            text = window.getSelection().toString();
        } else if (document.selection && document.selection.type != "Control") {
            text = document.selection.createRange().text;
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
                html: e.data.aobjectlink
            });            

            var highlightFunctionBox = $("<div/>", {
            });

            highlightFunctionBox.activityRemarkHighlightFunction({
                postData: e.data.postData,
                onReplySuccess: e.data.onReplySuccess,
                aobjectlink: e.data.aobjectlink,
                pagemode: e.data.pagemode
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
        }

        e.stopPropagation();
    });

    $(document).mouseup(function (e) {
        if ($(e.target).closest(".system-message-comment-container-reply").length > 0) {
            return;
        }
        if ($(e.target).closest(".remark-highlight-command-box").length == 0) {
            if ($(".remark-highlight-command-container").length > 0) {
                clearHightFunctionSelect();
            }
            removeHighlightFunction();
        }
    });
}

$.fn.activityRemarkHighlightFunction = function (datas) {
    var container = $(this);
    var postData = datas.postData;
    var onReplySuccess = datas.onReplySuccess;
    var aobjectlink = datas.aobjectlink;
    var pagemode = datas.pagemode;

    var tabContainer = $("<div/>", {
        class: "highlight-tab-panel-container",
        css: {
            marginTop:20
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
                SwapTabPanel(this,"highlight-tab-panel-content-reply");
            }
        });
        tabHeader.append(replyHeader);

        var replyBox = $("<div/>", {
            class: "highlight-tab-panel-content-element highlight-tab-panel-content-reply",
            css: {
                paddingBottom:43
            }
        });
        
        replyBox.activityRemarkReplyBox(postData, function () {
            removeHighlightFunction();
            if (onReplySuccess != undefined && onReplySuccess != false && typeof (onReplySuccess) == "function") {
                onReplySuccess();
            }
        }, false, aobjectlink);
        tabContent.append(replyBox);

        // =================== create ref activity box =====================

        onReplySuccessWithCreateActivity = onReplySuccess;
        postDataWithCreateActivity = postData;
        
        var createActHeader = $("<span/>", {
            html: "Create Activity",
            click: function () {
                SwapTabPanel(this, "highlight-tab-panel-content-activity");
            }
        });
        tabHeader.append(createActHeader);

        var createActbox = $("<div/>", {
            class: "highlight-tab-panel-content-element highlight-tab-panel-content-activity"
        });


        createActbox.activityRemarkActivityCreator({           
            aobjectlink: aobjectlink,
            pagemode: pagemode
        });

        tabContent.append(createActbox);
    }

    
    tabContainer.append(tabHeader,tabContent);
    container.append(tabContainer);
    container.find(".highlight-tab-panel-header span").first().addClass("is-active");
    container.find(".highlight-tab-panel-content-element").first().show();
    
    function SwapTabPanel(obj,className) {
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
            marginTop:10
        }
    });
    container.append(table);

    var row = $("<tr/>");
    table.append(row);

    var col1 = $("<td/>", {
        css: {
            width: "25%",
            padding: "0 5px",
            paddingLeft:0
        }
    });
    var col2 = $("<td/>", {
        css: {
            width: "25%",
            padding: "0 5px"
        }
    });
    var col3 = $("<td/>", {
        css: {
            width: "25%",
            padding: "0 5px",
            paddingRight: 0
        }
    });
    var col4 = $("<td/>", {
        css: {
            width: "25%",
            padding: "0 5px",
            paddingRight: 0
        }
    });
    row.append(col1, col2, col3, col4);

    var btnTask = $("<span/>", {
        class: "btn btn-primary",
        css:{
            width: "100%",
            fontWeight:600
        },
        html: "<i class='fa fa-edit' style='color: #fff;'></i>&nbsp;Task",
        click: function () {
            openFrame("task", aobjectlink, pagemode);
        }
    });
    col1.append(btnTask);

    var btnMeeting = $("<span/>", {
        class: "btn btn-primary",
        css: {
            width: "100%",
            fontWeight: 600
        },
        html: "<i class='fa fa-users' style='color: #fff;'></i>&nbsp;Meeting",
        click: function () {
            openFrame("meeting", aobjectlink, pagemode);
        }
    });
    col2.append(btnMeeting);

    var btnNote = $("<span/>", {
        class: "btn btn-primary",
        css: {
            width: "100%",
            fontWeight: 600
        },
        html: "<i class='fa fa-list' style='color: #fff;'></i>&nbsp;Note",
        click: function () {
            openFrame("note", aobjectlink, pagemode);
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
            openFrame("sale", aobjectlink, pagemode);
        }
    });
    col4.append(btnSaleTask);

    var ifrane = $("<iframe/>", {
        class: "create-activity-frame",
        id: "create-activity-frame-" + guid
    });

    container.append(ifrane);

    function openFrame(type, aobjectlink, pagemode) {
        var mode = "";
        if (type == "note")
            mode = "createNote";
        if (type == "task")
            mode = "createTask";
        if (type == "meeting")
            mode = "createMeeting";
        if (type == "sale")
            mode = "createSales";

        if (aobjectlink != null && aobjectlink != undefined && aobjectlink != "" && pagemode == "ACTIVITY") {
            var src = '/widget/PopupCreateActivityReDesign.aspx?mode=' + mode + '&functionNumber=' + guid + '&aobj=' + aobjectlink + '&rowkey=quoteActivity';
            ifrane.prop("src", src);
        }
        else {
            var src = '/widget/PopupCreateActivityReDesign.aspx?mode=' + mode + '&functionNumber=' + guid;
            ifrane.prop("src", src);
        }

    }
}