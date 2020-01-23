
$('head').append('<link href="' + servictWebDomainName + 'AGapeGalleryFinal/agape-gallery-3.0.css" rel="stylesheet" />');
$('head').append('<link href="' + servictWebDomainName + 'AGapeGalleryFinal/lightGallery-master/dist/css/lightgallery.css" rel="stylesheet">');

$.getScript(servictWebDomainName + "AGapeGalleryFinal/lightGallery-master/dist/js/lightgallery.js", function () {
    $.getScript(servictWebDomainName + "AGapeGalleryFinal/lightGallery-master/dist/js/lg-fullscreen.js");
    $.getScript(servictWebDomainName + "AGapeGalleryFinal/lightGallery-master/dist/js/lg-thumbnail.js");
    $.getScript(servictWebDomainName + "AGapeGalleryFinal/lightGallery-master/dist/js/lg-zoom.js");
    $.getScript(servictWebDomainName + "AGapeGalleryFinal/lightGallery-master/dist/js/lg-hash.js");
    $.getScript(servictWebDomainName + "AGapeGalleryFinal/lightGallery-master/dist/js/lg-pager.js");
    $.getScript(servictWebDomainName + "AGapeGalleryFinal/lightGallery-master/dist/js/lg-video.js");
    $.getScript(servictWebDomainName + "AGapeGalleryFinal/lightGallery-master/dist/js/lg-autoplay.js");
    $.getScript(servictWebDomainName + "AGapeGalleryFinal/lightGallery-master/lib/jquery.mousewheel.min.js");
});
//$.getScript("/agapegalleryfinal/lightGallery-master/dist/js/lg-fullscreen.js");
//$.getScript("/agapegalleryfinal/lightGallery-master/dist/js/lg-thumbnail.js");
//$.getScript("/agapegalleryfinal/lightGallery-master/dist/js/lg-zoom.js");
//$.getScript("/agapegalleryfinal/lightGallery-master/dist/js/lg-hash.js");
//$.getScript("/agapegalleryfinal/lightGallery-master/dist/js/lg-pager.js");

//$.getScript("/agapegalleryfinal/lightGallery-master/dist/js/lg-video.js");
//$.getScript("/agapegalleryfinal/lightGallery-master/dist/js/lg-autoplay.js");

function ActivityDownloadFileForm(obj) {
    var path = $(obj).attr("data-path");
    var name = $(obj).attr("data-name");
    var downloadForm = $("<form method='post' action='/widget/DownloadFileForm.aspx' target='_blank' style='display:none' />");
    var fileName = $("<input type='hidden' name='FILENAME' value='" + name + "'/>");
    var filePath = $("<input type='hidden' name='FILEPATH' value='" + path + "'/>");
    downloadForm.appendTo($('body'));
    downloadForm.append(fileName, filePath);
    downloadForm.submit();
    downloadForm.remove();
}


function StartLightGallery(imageDatas, index) {
    $(window).focus();
    $(".light-gallery-temp").remove();
    if (imageDatas != undefined && imageDatas.length > 0) {
        var ul = $("<ul/>", {
            class: "light-gallery-temp list-unstyled",
            css: {
                display: "none"
            }
        });
        for (var i = 0; i < imageDatas.length; i++) {
            var url = imageDatas[i];
            var li = $("<li/>", {
                "data-responsive": url,
                "data-src": url
            });

            var a = $("<a/>");
            li.append(a);

            var img = $("<img/>", {
                src: url
            });
            a.append(img);
            ul.append(li);
        }
        $("body").append(ul);
        ul.lightGallery();
        if (index != undefined) {
            ul.find("img:eq(" + index + ")").click();
        } else {
            ul.find("img:first").click();
        }
    }
}

$.fn.LightGalleryContainer = function () {
    $(this).addClass("agape-gallery-box");

    var imageDatas = [];
    var allImages = $(this).find(".panel-feed-remark-content-image");

    allImages.each(function () {
        imageDatas.push($(this).attr("data-image"));
    });

    allImages.each(function (index) {
        $(this).unbind("click").bind("click", {
            imageDatas: imageDatas,
            index: index
        }, function (e) {
            StartLightGallery(e.data.imageDatas, e.data.index);
        });


        if (index == 0) {
            $(this).addClass("main-child");
        }

        if (imageDatas.length >= 4) {
            if (index == 1) {
                $(this).css("width", "33.33%");
            }
            if (index == 2 || index == 3) {
                $(this).addClass("pull-right").css("width", "33.33%");
            }
            if (index == 3) {
                $(this).prev().before($(this));
            }
            if (index > 3) {
                $(this).hide();
            }
        }
        else if (imageDatas.length >= 3) {
            if (index == 2) {
                $(this).addClass("pull-right");
            }
            if (index > 2) {
                $(this).hide();
            }
        } else if (imageDatas.length == 2) {

            if (index == 1) {
                $(this).addClass("main-child pull-right");
            }
            if (index == 0 || index == 1) {
                $(this).css("width", "50%");
            }
        }

        if (imageDatas.length > 4 && index == 3) {
            $(this).append($("<div/>", {
                class: "feed-more-image",
                html: "+" + (imageDatas.length - 4)
            }));
        }

    });


    var allVideos = $(this).find(".panel-feed-remark-content-video");
    var countVideo = allVideos.length;
    var countMoreVideo = 0;
    if (countVideo > 1) {
        allVideos.each(function (index) {
            var video = $(this);
            var isHalf = false;
            if (countVideo == 2) {
                isHalf = true;
            } else {
                if (index > 0) {
                    if (index == (countVideo - 1) && index % 2 != 0) {
                        video.css({
                            width: "100%"
                        });
                    } else {
                        isHalf = true;
                    }
                }
            }

            if (isHalf) {
                video.css({
                    width: "50%",
                    height: 200
                });
                if (index % 2 == 0) {
                    video.addClass("pull-right");
                }
            }

            if (index > 2) {
                video.addClass("video-toggle");
                video.hide();
                countMoreVideo++;
            }

            //video.media();
        });
    }

    if (countMoreVideo > 0) {
        var moreVideo = $("<div/>", {
            css: {
                padding: "3px 10px",
                border: "1px solid #ccc",
                borderRadius: 3,
                cursor: "pointer",
                background: "#f7f7f7",
                textAlign: "center",
                color: "#3B5998"
            },
            html: "<span class='toggle-label' style='display:none'>ซ่อน</span>วีดีโอเพิ่มเติม (+" + countMoreVideo + ")",
            click: function () {
                $(this).closest(".agape-gallery-box").find(".video-toggle").fadeToggle();
                $(this).find(".toggle-label").toggle();
            }
        });
        $(this).append(moreVideo);
    }

}

$.fn.aGepeGalleryContainer = function () {
    var container = $(this);
    var allImages = container.find(".system-message-attach-image-block");

    var imageDatas = [];
    allImages.each(function () {
        var img = $(this);
        imageDatas.push(img.attr("data-image"));
    });

    allImages.each(function (index) {
        $(this).unbind("click").bind("click", {
            imageDatas: imageDatas,
            index: index
        }, function (e) {
            StartLightGallery(e.data.imageDatas, e.data.index);
        });
    });


}

$.fn.aGepeGallery = function (datas) {

    var countImg = datas.index == undefined ? 0 : datas.index;
    var totol = datas.images.length;
    var rotate = 0;
    if (totol > 0) {
        $(this).html("").addClass("agape-gallery").hide().fadeIn(function () {
            $("body").addClass("agape-gallery-clear-body");
        });

        var container = $("<div/>", {
            class: "agape-gallery-container"
        });

        var divHeader = $("<div/>", {
            class: "header",
            html: datas.title
        });

        $(divHeader).append($("<span>", {
            class: "remover",
            html: "&#10006;",
            click: function () {
                $(".agape-gallery").fadeOut(function () {
                    $(".agape-gallery").remove();
                    $("body").removeClass("agape-gallery-clear-body");
                });
            }
        }));

        var containerImage = $("<div>", {
            class: "agape-image-container"
        });

        var contentImage = $("<img/>", {
            class: "agape-image",
            src: datas.images[countImg].url
        });

        var divBody = $("<div/>", {
            class: "body"
        });

        $(containerImage).append(contentImage);

        var imageIndexDetail = $("<b>", {
            class: "image-index-detail",
            html: (countImg + 1) + "/" + totol,
            css: {
                float: "right",
                marginTop: -20,
                marginRight: -20,
                color: "#fff"
            }
        });

        $(divBody).append(imageIndexDetail, containerImage);

        var divFooter = $("<div/>", {
            class: "footer"
        });

        var FooterDetail = $("<p/>", {
            class: "footer-detail",
            html: datas.images[countImg].detail == undefined ? "" : datas.images[countImg].detail
        });

        $(divFooter).append(FooterDetail);

        $(divFooter).append($("<input/>", {
            type: "button",
            value: "Previous",
            click: function () {
                rotate = 0;
                if (countImg > 0) {
                    $(".agape-image-container").animate({
                        marginRight: -3000
                    }, function () {
                        $(".agape-image-container").remove();

                        var containerImage = $("<div>", {
                            class: "agape-image-container"
                        });

                        var contentImage = $("<img/>", {
                            class: "agape-image",
                            src: datas.images[countImg].url
                        });

                        $(containerImage).append(contentImage);
                        $(divBody).append(containerImage);
                        $(containerImage).draggable();

                        $(".image-index-detail").html((countImg + 1) + "/" + totol);
                        $(".footer-detail").html(datas.images[countImg].detail == undefined ? "" : datas.images[countImg].detail);
                    });

                    countImg--;
                }
            }
        })).append($("<input/>", {
            css: {
                marginLeft: 10
            },
            type: "button",
            value: "Zoom In",
            click: function () {
                var width = $(".agape-image").width();
                var height = $(".agape-image").height();
                var percentWidth = 40 / 100 * width;
                var percentHeight = 40 / 100 * height;
                var newWidth = width + percentWidth;
                var newHeight = height + percentHeight;
                $(".agape-image").animate({
                    width: newWidth,
                    height: newHeight,
                }, 300);
            }
        })).append($("<input/>", {
            css: {
                marginLeft: 10
            },
            type: "button",
            value: "Zoom Out",
            click: function () {
                var width = $(".agape-image").width();
                var height = $(".agape-image").height();
                var percentWidth = 40 / 100 * width;
                var percentHeight = 40 / 100 * height;
                var newWidth = width - percentWidth;
                var newHeight = height - percentHeight;
                if (newHeight > 200) {
                    $(".agape-image").animate({
                        width: newWidth,
                        height: newHeight,
                    }, 300, function () {
                        $(".agape-image-container").animate({
                            left: 0,
                            top: 0
                        });
                    });
                }
            }
        })).append($("<input/>", {
            css: {
                marginLeft: 10
            },
            type: "button",
            value: "Rotate",
            click: function () {
                rotateImage();
            }
        })).append($("<input/>", {
            css: {
                marginLeft: 10
            },
            type: "button",
            value: "Fit",
            click: function () {
                rotate = 0;
                $(".agape-image-container").remove();

                var containerImage = $("<div>", {
                    class: "agape-image-container"
                });

                var contentImage = $("<img/>", {
                    class: "agape-image",
                    src: datas.images[countImg].url
                });

                $(containerImage).append(contentImage);
                $(divBody).append(containerImage);
                $(containerImage).draggable();
            }
        })).append($("<input/>", {
            css: {
                marginLeft: 10
            },
            type: "button",
            value: "Next",
            click: function () {
                rotate = 0;
                if (countImg < datas.images.length - 1) {
                    $(".agape-image-container").animate({
                        marginLeft: -3000
                    }, function () {
                        $(".agape-image-container").remove();

                        var containerImage = $("<div>", {
                            class: "agape-image-container"
                        });

                        var contentImage = $("<img/>", {
                            class: "agape-image",
                            src: datas.images[countImg].url
                        });

                        $(containerImage).append(contentImage);
                        $(divBody).append(containerImage);
                        $(containerImage).draggable();

                        $(".image-index-detail").html((countImg + 1) + "/" + totol);
                        $(".footer-detail").html(datas.images[countImg].detail == undefined ? "" : datas.images[countImg].detail);
                    });

                    countImg++;
                }
            }
        }));

        $(container).append(divHeader, divBody, divFooter);

        $(this).append(container);

        $(containerImage).draggable();

        var RotateTime = 90;
        function rotateImage() {
            if (RotateTime > 0) {
                RotateTime--;
                rotate++;

                $(".agape-image").css({
                    "-ms-transform": "rotate(" + rotate + "deg)",
                    "-webkit-transform": "rotate(" + rotate + "deg)",
                    "transform": "rotate(" + rotate + "deg)"
                });

                setTimeout(function () {
                    rotateImage();
                }, 1);
            }
            else {
                RotateTime = 90;
            }
        }
    }
};