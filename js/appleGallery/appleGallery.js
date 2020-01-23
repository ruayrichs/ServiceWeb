var activityRemarkReplyBoxAttachGlobalFiles = [];
var activityRemarkReplyBoxAttachGlobalFilesUploadType = "";

$.fn.appleGallery = function (datas) {

    var aobjectlink = datas.aobjectlink;
    var onAfterSave = datas.onAfterSave;

    var pReply = $("<div/>", {
        class: "system-message-comment-container-reply"
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

    var imageAttach = $("<i/>", {
        class: "reply-attach fa fa-picture-o pull-right",
        title: "Attach pictures",
        css: {
            marginRight: 0
        }
    });

    imageAttach.bind("click", {
        fileContainer: fileContainer
    }, function (e) {
        e.data.fileContainer.activityRemarkReplyBoxAttach("IMAGE", aobjectlink, pReply, onAfterSave);
    });

    subCon.append(imageAttach);
    pReply.append(fileContainer, subCon);

    var imgData = datas.gelleryData;

    var container = $(this);

    if (imgData.length > 0) {

        var totWidth = 0;
        var positions = new Array();

        var body = $("<div/>", {
            class: "appleBody"
        });

        var main = $("<div/>", {
            class: "appleMain"
        });

        var gallery = $("<div/>", {
            id: "appleGallery"
        });

        body.append(main);
        main.append(gallery);
        container.append(body);

        var slide = $("<div/>", {
            id: "appleSlides"
        });

        var menu = $("<div/>", {
            id: "appleMenu"
        });

        var menuUL = $("<ul/>", {
        });

        for (var i = 0; i < imgData.length; i++) {

            var itemSlide = $("<div/>", {
                class: "slide slide" + i
            });

            if (i != 0) {
                itemSlide.css("display", "none");
            }

            //var img = $("<img/>", {
            //    src: imgData[i].Image,
            //    alt: imgData[i].ImageName,
            //    class: "timeline-image-img-" + aobjectlink,
            //    css: {
            //        maxHeight: 400
            //    }
            //});

            var img = "<img src='" + imgData[i].Image + "' alt='" + imgData[i].ImageName + "' class='timeline-image-img-" + aobjectlink + "' style='max-height: 400px; cursor: pointer;' onclick=\"openGallery(" + i + ",'" + aobjectlink + "','');\" />";

            itemSlide.append(img);
            slide.append(itemSlide);

            if (i == 0) {
                menuUL.append('<li class="fbar">&nbsp;</li>');
            }

            var menuItem = $("<li/>", {
                class: "menuItem"
            });

            var menuItemHref = $("<a/>", {
                href: ""
            });

            var imgMenu = $("<img/>", {
                src: imgData[i].ImageThumb,
                alt: "thumbnail"
            });

            menuItemHref.append(imgMenu);
            menuItem.append(menuItemHref);
            menuUL.append(menuItem);

            //positions[i] = i;
            //totWidth += img.width();
        }

        menu.append(menuUL);
        gallery.append(slide, menu);

        container.append(pReply);
        /* This code is executed after the DOM has been completely loaded */

        $('#appleSlides').width("100%");

        /* Change the cotnainer div's width to the exact width of all the slides combined */                     

        $('#appleMenu ul li a').click(function (e) {

            /* On a thumbnail click */

            $('li.menuItem').removeClass('act').addClass('inact');
            $(this).parent().addClass('act');

            var pos = $(this).parent().prevAll('.menuItem').length;

            $("#appleSlides .slide").each(function () {
                if ($(this).hasClass("slide" + pos)) {
                    $(this).fadeIn(1000);
                }
                else {
                    $(this).hide();
                }
            });

            //$('#appleSlides').stop().animate({ marginLeft: -positions[pos] + 'px' }, 450);
            /* Start the sliding animation */

            e.preventDefault();
            /* Prevent the default action of the link */


            // Stopping the auto-advance if an icon has been clicked:
            //if (!keepScroll) clearInterval(itvl);
        });

        $('#appleMenu ul li.menuItem:first').addClass('act').siblings().addClass('inact');
        /* On page load, mark the first thumbnail as active */



        /*****
         *
         *	Enabling auto-advance.
         *
         ****/

        //var current = 1;
        //function autoAdvance() {
        //    if (current == -1) return false;

        //    $('#appleMenu ul li a').eq(current % $('#appleMenu ul li a').length).trigger('click', [true]);	// [true] will be passed as the keepScroll parameter of the click function on line 28
        //    current++;
        //}

        // The number of seconds that the slider will auto-advance in:

        //var changeEvery = 10;

        //var itvl = setInterval(function () { autoAdvance() }, changeEvery * 1000);

        /* End of customizations */
    }
    else {
        container.append(pReply);
    }
}

$.fn.activityRemarkReplyBoxAttach = function (uploadType, aobjectlink, pReply, onAfterSave) {
    var container = $(this);
    var iconAttach = container.closest(".system-message-comment-container-reply").find(".reply-attach");
    var commandBox = $("<div/>", {
        class: "text-right"
    });

    var fileBox = $("<div/>", {
        css: {
            padding: 10,
            borderTop: "1px solid #ccc",
            marginTop: 10
        }
    });

    container.html("");
    container.append(commandBox, fileBox);

    var addFile = $("<span/>", {
        css: {
            padding: "8px 10px",
            background: "#ccc",
            cursor: "pointer"
        },
        html: "<i class='fa fa-plus'></i> Add " + uploadType.toLowerCase()
    });

    var conFirmFile = $("<span/>", {
        css: {
            padding: "8px 10px",
            background: "#ccc",
            marginLeft: 10,
            cursor: "pointer"
        },
        html: "<i class='fa fa-check'></i> Confirm"
    });

    conFirmFile.bind("click", {
        pReply: pReply,
        aobjectlink: aobjectlink,
        onAfterSave: onAfterSave
    }, function (e) {
        if (aobjectlink != undefined) {

            var guid = generateGUID();

            if ($("#activity-remark-file-form").length > 0 && activityRemarkReplyBoxAttachGlobalFiles.length > 0) {
                ajaxPostUploadFiles(event, {
                    panelReply: e.data.pReply,
                    guid: guid,
                    aobjectlink: aobjectlink,
                    onAfterSave: e.data.onAfterSave
                });
            }
        }
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

    commandBox.append(addFile, conFirmFile, canCel);

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

$.fn.activityRemarkReplyBoxAttachAppendImage = function (file) {
    var container = $(this);
    var imgBlock = $("<div/>", {
        class: "system-message-attach-image-block",
        css: {
            width: 100,
            height: 100
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

function generateGUID() {
    var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx-'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
    return guid;
}

function ajaxPostUploadFiles(event, datas) {  
    var panelReply = datas.panelReply;   
    var guid = datas.guid;
    var aobjectlink = datas.aobjectlink;
    var onAfterSave = datas.onAfterSave;
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
        data.append("message", "");
        data.append("aobj", aobjectlink);
        data.append("createThumb", true);
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

                if (onAfterSave != null && onAfterSave != undefined) {
                    //agroLoading(true);
                    AGLoading(true);
                    onAfterSave(aobjectlink);
                }
            },
            cache: false,
            contentType: false,
            processData: false
        });
    }, 1000);
}