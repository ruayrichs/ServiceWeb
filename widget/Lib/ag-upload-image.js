$.getScript("/AGapegallery/agape-gallery-3.0.js");

var agUploadImageAttachGlobalFiles = [];
var agUploadImageGallery = [];

$.fn.agUploadImageAttach = function (datas) {

    var urlPost = datas.urlPost;
    var urlLoad = datas.urlLoad;
    var urlDelete = datas.urlDelete;
    var urlDragDrop = datas.urlDragDrop;
    var objectKey = datas.objectKey;

    agUploadImageAttachGlobalFiles = [];

    var container = $(this);   

    var topPanel = $("<div/>", {       
        css: {                    
            borderBottom: "1px solid #ccc",
            margin: 10,
            marginTop: 0,
            paddingBottom: 10,
            fontSize: 26,
            fontWeight: "bold"
        }
    });

    var textHeader = $("<div/>", {
        css: {
            display: "inline-block",
            width: "50%"
        },
        html: "Image Upload"
    });

    var btnPost = $("<div/>", {
        class: "top-panel-button-post",
        css: {
            display: "inline-block",
            width: "50%",
            textAlign: "right"
        }
    });

    topPanel.append(textHeader, btnPost);

    var fileBox = $("<div/>", {
        css: {          
            margin: 10
        }
    });   

    container.html("");
    container.append(topPanel, fileBox);

    var addFile = $("<div/>", {
        class: "agUpload-image-block-add",
        css: {          
            cursor: "pointer"
        }
    });

    var imgBlock = $("<div/>", {
        class: "agUpload-add-image",
        html: "<i class='fa fa-plus' style='opacity: .3;'></i>",
        title: "Choose image to upload"
    });

    addFile.append(imgBlock);    

    imgBlock.mouseenter(function () {
        $(this).find("i").css("opacity", ".6");
    }).mouseleave(function () {
        $(this).find("i").css("opacity", ".3");
    });        

    fileBox.append(addFile);

    var form = $("#agUpload-image-file-form");
    if (form.length == 0) {
        form = $("<form method='post' style='padding-left:100px' action='/widget/AJAXFileUploadAPI.aspx' id='agUpload-image-file-form' enctype='multipart/form-data' />");
    }
    form.html("").hide();

    var fileUpload = $("<input type='file' id='FilePath' name='FilePath' multiple/>");

    fileUpload.attr("accept", "image/*");

    form.append(fileUpload);

    $("body").append(form);

    var form = document.getElementById('agUpload-image-file-form');

    // Add events
    $(fileUpload).bind("change", {
        container: container,      
        uploadType: "IMAGE"
    }, function (e) {
        var fLength = $(fileUpload).get(0).files.length;       
        if (fLength > 0) {
            for (var i = 0; i < $(fileUpload).get(0).files.length; i++) {
                agUploadImageAttachGlobalFiles.push($(fileUpload).get(0).files[i]);                
                fileBox.agUploadImageAttachAppendImage($(fileUpload).get(0).files[i], urlPost, urlLoad, urlDelete, urlDragDrop, objectKey, e.data.container);
            }

            e.data.container.show();
        }
    });

    addFile.click(function () {
        $(fileUpload).click();
    });
    $(fileUpload).click();

    if (urlLoad != undefined) {
        agUploadImageLoadFiles(urlLoad, urlDelete, urlDragDrop, container);
    }
}

$.fn.agUploadImageAttachAppendImage = function (file, postData, loadData, deleteData, moveData, objectKey, mainContainer) {
    var container = $(this);
    var imgBlock = $("<div/>", {
        class: "agUpload-image-block"      
    });

    var imgBlockDetail = $("<div/>", {
        class: "agUpload-image-block-img"
    });

    imgBlock.append(imgBlockDetail);

    var fReader = new FileReader();
    fReader.readAsDataURL(file);
    fReader.onloadend = function (event) {        

        var img = $("<img/>", {
            css: {
                width: 100,
                height: 100,
                verticalAlign: "baseline"
            },
            src: event.target.result
        });        

        var del = $("<div/>", {
            class: "agUpload-image-block-delete",
            css: {
                display: "none"
            }
        });

        var delButton = $("<span/>", {
            class: "agUpload-image-block-delete-button",
            html: "<i class='fa fa-remove' title='Remove'></i>"
        });

        del.append(delButton);

        imgBlockDetail.append(img, del);

        delButton.click(function () {

            agUploadImageAttachGlobalFiles = jQuery.grep(agUploadImageAttachGlobalFiles, function (value) {
                return value != file;
            });

            imgBlock.remove();

            if (agUploadImageAttachGlobalFiles.length == 0) {
                if (container.prev().find(".postButton").length > 0) {
                    container.prev().find(".postButton").remove();
                }
            }
        });

        if (agUploadImageAttachGlobalFiles.length > 0 && container.prev().find(".postButton").length == 0) {
            var btnPost = $("<button/>", {
                class: "btn btn-primary postButton",
                html: "Upload",
                type: "button",
                width: 100
            });

            container.prev().find(".top-panel-button-post").append(btnPost);

            btnPost.bind("click", {
                container: mainContainer,
                postData: postData,
                loadData: loadData,
                deleteData: deleteData,
                moveData: moveData,
                objectKey: objectKey
            }, function (e) {

                var elt = $(this);
                
                if (postData != undefined) {

                    var guid = generateGUID();

                    agUploadImagePostFiles(event, {
                        finalPostData: e.data.postData,
                        loadData: e.data.loadData,
                        deleteData: e.data.deleteData,
                        moveData : e.data.moveData,
                        guid: guid,
                        objectKey: e.data.objectKey,
                        mainContainer: e.data.container
                    });
                }

            });
        }
    }

    container.prepend(imgBlock);    

    imgBlock.mouseenter(function () {
        $(this).find(".agUpload-image-block-delete").css("display", "block");
    }).mouseleave(function () {
        $(this).find(".agUpload-image-block-delete").css("display", "none");
    });
}

function generateGUID() {
    var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx-'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
    return guid;
}

function agUploadImagePostFiles(event, datas) {
    var finalPostData = datas.finalPostData;   
    var loadData = datas.loadData;
    var deleteData = datas.deleteData;
    var moveData = datas.moveData;
    var guid = datas.guid;
    var objectKey = datas.objectKey;
    var mainContainer = datas.mainContainer;

    $(mainContainer).AGWhiteLoading(true, "Uploading images");

    setTimeout(function () {
        // Create a formdata object and add the files
        var data = new FormData();
        var files = agUploadImageAttachGlobalFiles;
        if (files.length > 0) {
            for (var i = 0; i < files.length; i++) {
                data.append('UploadedFiles', files[i], files[i].name);
            }
        }

        data.append("uploadType", "IMAGE");
        data.append("message", "");
        data.append("aobj", objectKey);

        agUploadImageAttachGlobalFiles = [];

        $.ajax({
            url: finalPostData,
            type: "POST",
            data: data,
            //async: false,
            success: function (msg) {
                agUploadImageAttachGlobalFiles = [];
                $(mainContainer).AGWhiteLoading(false);

                if ($(mainContainer).find(".agUpload-image-block").length > 0) {
                    $(mainContainer).find(".agUpload-image-block").remove();
                }
                if ($(mainContainer).find(".postButton").length > 0) {
                    $(mainContainer).find(".postButton").remove();
                }

                if (loadData != undefined) {
                    agUploadImageLoadFiles(loadData, deleteData, moveData, mainContainer);
                }
            },
            cache: false,
            contentType: false,
            processData: false
        });
    }, 1000);
}

function agUploadImageLoadFiles(urlLoad, urlDelete, urlDragDrop, mainContainer) {

    $(mainContainer).AGWhiteLoading(true, "Loading images");

    $.ajax({
        url: urlLoad,            
        success: function (data) {            
            if (data.length > 0) {
                var jsonArr = $.parseJSON(data);
                if (jsonArr.length > 0) {
                    mainContainer.agUploadImageAppendGallery(jsonArr, urlLoad, urlDelete, urlDragDrop);
                } else {
                    $(mainContainer).find(".panelGalleryHeader").remove();
                    $(mainContainer).find(".agUploadImageFileBoxGallery").remove();
                    $(mainContainer).AGWhiteLoading(false);
                }
            } else {
                $(mainContainer).find(".panelGalleryHeader").remove();
                $(mainContainer).find(".agUploadImageFileBoxGallery").remove();
                $(mainContainer).AGWhiteLoading(false);
            }
        }
    });
}

function agUploadImageDeleteFile(urlLoad, urlDelete, urlDragDrop, dataKey, mainContainer) {

    $(mainContainer).AGWhiteLoading(true, "Deleting images");

    var finalUrlDelete = urlDelete;

    if (dataKey != undefined) {
        finalUrlDelete += "&dataKey=" + dataKey;
    }

    $.ajax({
        url: finalUrlDelete,
        success: function () {
            $(mainContainer).AGWhiteLoading(false);
            if (urlLoad != undefined) {                
                agUploadImageLoadFiles(urlLoad, urlDelete, urlDragDrop, mainContainer);
            }
        }
    });
}

$.fn.agUploadImageAppendGallery = function (datas, urlLoad, urlDelete, urlDragDrop) {

    var container = $(this);

    var fileBox = $("<div/>", {
        class: "agUploadImageFileBoxGallery",
        css: {
            margin: 10
        }
    });

    if (container.find(".panelGalleryHeader").length == 0) {
        var topPanel = $("<div/>", {
            class: "panelGalleryHeader",
            css: {
                borderBottom: "1px solid #ccc",
                margin: 10,
                marginTop: 0,
                paddingBottom: 10,
                fontSize: 26,
                fontWeight: "bold"
            }
        });

        var textHeader = $("<div/>", {
            css: {
                display: "inline-block",
                width: "100%"
            },
            html: "Gallery"
        });

        topPanel.append(textHeader);

        container.append(topPanel, fileBox);
    } else {
        fileBox = container.find(".agUploadImageFileBoxGallery");
        fileBox.html("");
    }    
    
    agUploadImageGallery = [];

    for (i = 0; i < datas.length; i++) {
        var imgBlock = $("<div/>", {
            class: "agUpload-image-block"
        });

        var imgBlockDetail = $("<div/>", {
            class: "agUpload-image-block-img",
            css: {
                cursor: "pointer"
            },
            "data-index": i,
            "data-key": datas[i].key_object_link
        });

        imgBlock.append(imgBlockDetail);

        var imageSrc = "\\managefile\\" + datas[i].sid + datas[i].file_path;

        agUploadImageGallery.push(imageSrc);

        var img = $("<img/>", {
            css: {
                width: 100,
                height: 100,
                verticalAlign: "baseline"
            },
            src: imageSrc
        });

        var del = $("<div/>", {
            class: "agUpload-image-block-delete",
            css: {
                display: "none",
                zIndex: 1
            }
        });

        imgBlockDetail.append(img, del);

        if (urlDelete != undefined && urlDelete != "") {
            var delButton = $("<span/>", {
                class: "agUpload-image-block-delete-button",
                css: {
                    zIndex: 2,
                    opacity: 0
                },
                html: "<i class='fa fa-remove' title='Remove'></i>"
            });

            imgBlockDetail.append(delButton);
        }              

        fileBox.append(imgBlock);

        imgBlock.mouseenter(function () {
            $(this).find(".agUpload-image-block-delete").css("display", "block");
            $(this).find(".agUpload-image-block-delete-button").css("opacity", "1");
        }).mouseleave(function () {
            $(this).find(".agUpload-image-block-delete").css("display", "none");
            $(this).find(".agUpload-image-block-delete-button").css("opacity", "0");
        });
    }

    $(container).find(".agUploadImageFileBoxGallery").find(".agUpload-image-block-img").each(function () {

        var dataKey = $(this).attr("data-key");               

        $(this).find(".agUpload-image-block-delete").click(function () {

            var index = $(this).parent().attr("data-index");

            StartLightGallery(agUploadImageGallery, index);
        });

        $(this).find(".agUpload-image-block-delete-button").click(function () {
            if (confirm("Confirm to delete this picture ?")) {
                agUploadImageDeleteFile(urlLoad, urlDelete, urlDragDrop, dataKey, container);
            }
        });
    });

    if (urlDragDrop != undefined && urlDragDrop != "") {
        fileBox.sortable({
            stop: function () {               
                $(this).agUploadImageMoveFile(urlDragDrop);
            }
        });

        $(fileBox).disableSelection();
    }

    $(container).AGWhiteLoading(false);
}

$.fn.agUploadImageMoveFile = function (urlDragDrop) {

    var container = $(this);
    var i = 0;

    agUploadImageGallery = [];

    var objKey = [];

    $(container).find(".agUpload-image-block-img").each(function () {
        $(this).attr("data-index", i);

        var src = $(this).find("img").attr("src");

        agUploadImageGallery.push(src);

        var dataKey = $(this).attr("data-key");

        objKey.push(dataKey);

        i++;
    });

    $.ajax({
        url: urlDragDrop + "&arrKey=" + objKey.join(','),
        success: function () {

        }
    });
}