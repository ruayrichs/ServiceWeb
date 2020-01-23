
var MAX_UPLOAD_FILESIZE = 50000000;
var agUploadVideoAttachGlobalFiles = [];
var agUploadVideoGallery = [];

$.fn.agUploadVideoAttach = function (datas) {

    var urlPost = datas.urlPost;
    var urlLoad = datas.urlLoad;
    var urlDelete = datas.urlDelete;
    var urlDragDrop = datas.urlDragDrop;
    var objectKey = datas.objectKey;

    agUploadVideoAttachGlobalFiles = [];

    var container = $(this);

    var topPanel = $("<div/>", {
        css: {
            borderBottom: "1px solid #ccc",
            margin: 10,
            marginTop: 0,
            paddingBottom: 5,
            fontSize: 26,
            fontWeight: "bold"
        }
    });

    var textHeader = $("<div/>", {
        css: {
            display: "inline-block",
            width: "50%"
        },
        html: "Video Upload"
    });

    var panelButton = $("<div/>", {
        class: "top-panel-button-post",
        css: {
            display: "inline-block",
            width: "50%",
            textAlign: "right"
        }
    });

    var addFile = $("<button/>", {
        class: "btn btn-success",
        html: "Add",
        type: "button",
        id: "btnAddUploadFile",
        width: 100
    });

    panelButton.append(addFile);

    topPanel.append(textHeader, panelButton);

    var fileBox = $("<div/>", {
        css: {
            margin: 10
        }
    });

    container.html("");
    container.append(topPanel, fileBox);

    var form = $("#agUpload-video-file-form");
    if (form.length == 0) {
        form = $("<form method='post' style='padding-left:100px' action='/widget/AJAXFileUploadAPI.aspx' id='agUpload-video-file-form' enctype='multipart/form-data' />");
    }

    form.html("").hide();

    // video can select only one file / uploading 
    var fileUpload = $("<input type='file' id='FilePath' name='FilePath' />");

    fileUpload.attr("accept", "video/*");

    form.append(fileUpload);

    $("body").append(form);

    var form = document.getElementById('agUpload-video-file-form');

    // Add events
    $(fileUpload).bind("change", {
        container: container,
        uploadType: "video"
    }, function (e) {
        var fLength = $(fileUpload).get(0).files.length;
        if (fLength > 0) {
            for (var i = 0; i < $(fileUpload).get(0).files.length; i++) {
                var filesize = $(fileUpload).get(0).files[i].size;
                if (filesize > MAX_UPLOAD_FILESIZE) {
                    AGError("Please upload file less than 30 MB!!");
                    break;
                }
           
                agUploadVideoAttachGlobalFiles.push($(fileUpload).get(0).files[i]);
                fileBox.agUploadVideoAttachAppend($(fileUpload).get(0).files[i], urlPost, urlLoad, urlDelete, urlDragDrop, objectKey, e.data.container);
            }

            e.data.container.show();
        }
    });

    addFile.click(function () {

        $(fileUpload).click();
    });

    $(fileUpload).click();

    // Load saved video and display in screen
    if (urlLoad != undefined) {
        agUploadVideoLoadFiles(urlLoad, urlDelete, urlDragDrop, container);
    }

}

$.fn.agUploadVideoAttachAppend = function (file, postData, loadData, deleteData, moveData, objectKey, mainContainer) {
    var container = $(this);

    var imgBlock = $("<div/>", {
        class: "agUpload-video-block"
    });

    var imgBlockDetail = $("<div/>", {
        class: "agUpload-video-block-img"
    });

    imgBlock.append(imgBlockDetail);

    var fReader = new FileReader();
    fReader.readAsDataURL(file);
    fReader.onloadend = function (event) {

        if (agUploadVideoAttachGlobalFiles.length > 0 && container.prev().find(".postButton").length == 0) {
            var btnPost = $("<button/>", {
                class: "btn btn-primary postButton",
                html: "Upload",
                type: "button",
                css: {
                    width: 100,
                    marginLeft: 5
                }
            }); 

            var btnAdd = $("#btnAddUploadFile");
            btnAdd.css("display", "none");

//            var blockVideo = $("<video/>", {
//                css: {
//                    width: 250,
//                    height: 200,
//                    
//                    verticalAlign: "baseline"
//                },
//                controls: true,
//                autobuffer: 'auto',
//                preload: "auto",
//                src: event.target.result
//            });
            var blockVideo = '<video width="250" height="200" controls> ' +
	            '<source src="' + event.target.result + '" type="video/mp4" /> ' +
            ' </video>';

            imgBlockDetail.append(blockVideo);


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

                    agUploadVideoPostFiles(event, {
                        finalPostData: e.data.postData,
                        loadData: e.data.loadData,
                        deleteData: e.data.deleteData,
                        moveData: e.data.moveData,
                        guid: guid,
                        objectKey: e.data.objectKey,
                        mainContainer: e.data.container
                    });
                }

            });

        }
    }

    container.prepend(imgBlock);
}


function generateGUID() {
    var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx-'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
    return guid;
}

function agUploadVideoPostFiles(event, datas) {
    var finalPostData = datas.finalPostData;
    var loadData = datas.loadData;
    var deleteData = datas.deleteData;
    var moveData = datas.moveData;
    var guid = datas.guid;
    var objectKey = datas.objectKey;
    var mainContainer = datas.mainContainer;


    $(mainContainer).AGWhiteLoading(true, "Uploading Video");

    setTimeout(function () {
        // Create a formdata object and add the files
        var data = new FormData();
        var files = agUploadVideoAttachGlobalFiles;
        if (files.length > 0) {
            for (var i = 0; i < files.length; i++) {
                data.append('UploadedFiles', files[i], files[i].name);
            }
        }

        data.append("uploadType", "VIDEO");
        data.append("message", "");
        data.append("aobj", objectKey);

        agUploadVideoAttachGlobalFiles = [];

        $.ajax({
            url: finalPostData,
            type: "POST",
            data: data,
            //async: false,
            success: function (msg) {
                agUploadVideoAttachGlobalFiles = [];
                $(mainContainer).AGWhiteLoading(false);
           
                if ($(mainContainer).find(".agUpload-video-block").length > 0) {
                    $(mainContainer).find(".agUpload-video-block").remove();
                }
                if ($(mainContainer).find(".postButton").length > 0) {
                    $(mainContainer).find(".postButton").remove();
                }

                if (loadData != undefined) {
                    agUploadVideoLoadFiles(loadData, deleteData, moveData, mainContainer);
                    var btnAdd = $("#btnAddUploadFile");
                    btnAdd.css("display", "block");

                    AGSuccess("upload video success");
                }
            },
            cache: false,
            contentType: false,
            processData: false
        });
    }, 1000);
}


function agUploadVideoLoadFiles(urlLoad, urlDelete, urlDragDrop, mainContainer) {

    // abd 
     $(mainContainer).AGWhiteLoading(true, "Loading Video");

    $.ajax({
        url: urlLoad,
        success: function (data) {
            
 
                if (data.length > 0) {
                    var jsonArr = $.parseJSON(data);
                    if (jsonArr.length > 0) {
                        mainContainer.agUploadVideoAppendGallery(jsonArr, urlLoad, urlDelete, urlDragDrop);
                    } else {
                        $(mainContainer).find(".panelGalleryVideoHeader").remove();
                        $(mainContainer).find(".agUploadVideoFileBoxGallery").remove();
                        $(mainContainer).AGWhiteLoading(false);
                    }
                } else {
                      $(mainContainer).find(".panelGalleryVideoHeader").remove();
                    $(mainContainer).find(".agUploadVideoFileBoxGallery").remove();
                    $(mainContainer).AGWhiteLoading(false);
                }
        },
        error: function (request, status, error) {
            alert(request.responseText);
        }
    });
}




$.fn.agUploadVideoAppendGallery = function (datas, urlLoad, urlDelete, urlDragDrop) {

    var container = $(this);

    var fileBox = $("<div/>", {
        class: "agUploadVideoFileBoxGallery",
        css: {
            margin: 10
        }
    });

    if (container.find(".panelGalleryVideoHeader").length == 0) {
//        var topPanel = $("<div/>", {
//            class: "panelGalleryVideoHeader",
//            css: {
//                borderBottom: "1px solid #ccc",
//                margin: 10,
//                marginTop: 0,
//                paddingBottom: 10,
//                fontSize: 26,
//                fontWeight: "bold"
//            }
//        });

//        var textHeader = $("<div/>", {
//            css: {
//                display: "inline-block",
//                width: "100%"
//            },
//            html: "ABD Video " + datas.length
//        });

//        topPanel.append(textHeader);
//          container.append(topPanel, fileBox);
        container.append(fileBox);
    } else {
        fileBox = container.find(".agUploadVideoFileBoxGallery");
        fileBox.html("");
    }

    agUploadImageGallery = [];

    for (i = 0; i < datas.length; i++) {
        var videoBlock = $("<div/>", {
            class: "agUpload-video-block  col-md-4" 
        });

        var videoBlockDetail = $("<div/>", {
            class: "agUpload-video-block-img",
            css: {
                cursor: "pointer"
            },
            "data-index": i,
            "data-key": datas[i].key_object_link
        });

        videoBlock.append(videoBlockDetail);

        var videoSrc = "\\managefile\\" + datas[i].sid + datas[i].file_path;
// old block video logic 
//        var blockVideo = $("<video/>", {
//            css: {
//                width: 250,
//                height: 200,
//                
//                verticalAlign: "baseline"
//            },
//            autobuffer: 'auto',
//            preload: 'auto',
//            controls: true,
//            src: videoSrc
//        });

        var blockVideo = '<video width="250" height="200" controls> ' +
	        '<source src="' + videoSrc + '" type="video/mp4" /> ' +
        ' </video>';

        videoBlockDetail.append(blockVideo);

        fileBox.append(videoBlock);
    }
    
    
    $(container).AGWhiteLoading(false);
}