
function AGLoading(flag,message) {
    var loadingImage = servictWebDomainName + "images/focusone-loading.gif?vs=1.2";

    if (!flag) {
        $(".ag-loading").fadeOut(function () {
            $(".ag-loading").remove();
        });
    } else {

        var container = $("<div/>", {
            class: "ag-loading",
            css: {
                position: "fixed",
                zIndex: 5000000,
                top: 0,
                left: 0,
                right: 0,
                bottom:0,
                background: "rgba(0, 0, 0, 0.55)",
                lineHeight: "100%",
                textAlign: "center",
                display: "none"
            }
        });

        var table = $("<table/>", {
            css: {
                tableLayout: "fixed",
                width: "100%",
                height: "100%"
            }
        });
        table.appendTo(container);

        var tr = $("<tr/>");
        tr.appendTo(table);

        var td = $("<td/>", {
            css: {
                width: "100%",
                height: "100%",
                verticalAlign: "middle"
            }
        });
        td.appendTo(tr);

        var image = $("<img/>", {
            src: loadingImage,
            css: {
                width: 80,
                height:80
            }
        });
        image.appendTo(td);

        if (message != undefined) {
            var msg = $("<div/>", {
                css: {
                    marginTop: 15,
                    fontWeight: 600,
                    color: "#eee"
                },
                html: message + "..."
            });
            td.append(msg);
        }

        $("body").append(container);
        container.fadeIn();
    }
}

function AGError(message) {
    swal({
        title: "",
        text: message,
        type: "error",
        customClass: 'swal-wide',
        html: true
    });
}

function AGInfo(message) {
    swal({
        title: "",
        text: message,
        type: "info",
        customClass: 'swal-wide',
        html: true
    });
}

function AGSuccess(message) {
    swal({
        title: "",
        text: message,
        type: "success",
        customClass: 'swal-wide',
        html: true
    });
}

function AGMessage(message) {
    swal({
        title: "",
        text: message,
        type: "",
        customClass: 'swal-wide',
        html: true
    });
}

var ConfirmFlag = undefined;

function AGConfirm(sender, message) {
    if (message == undefined && typeof (sender) == "string") {
        message = sender;
        sender = $(window.event.target);
    }

    if (ConfirmFlag == undefined) {
        swal({
            title: "",
            text: message,
            type: "warning",
            showCancelButton: true,
            html: true,
            confirmButtonText: "OK"
        },
        function (isConfirm) {
            ConfirmFlag = isConfirm;
            $(sender).click();
        });

        return false;
    } else {
        var xConfirm = ConfirmFlag;
        ConfirmFlag = undefined;
        return xConfirm;
    }
}

$.fn.AGWhiteLoading = function (flag, msg) {
    var elt = $(this);
    if (flag) {
        var loading = $("<div/>", {
            class: "agp-white-loading",
            css: {
                zIndex: 10,
                background: "rgba(255, 255, 255, 0.68)",
                position: "absolute",
                top: 0,
                left: 0,
                right: 0,
                height: "100%",
                width: "100%",
                padding: 20,
                textAlign: "center",
                paddingTop: 150
            }
        });
        $(loading).append($("<img/>", {
            src: servictWebDomainName + "images/loadmessage.gif",
            css: {
                marginTop: -2,
                width: 20,
                height: 20
            }
        }));
        $(loading).append($("<label/>", {
            html: (msg == undefined ? "Loading content" : msg) + "...",
            css: {
                marginLeft: 10
            }
        }));
        $(loading).css("padding-top", elt.height() / 2);
        $(elt).append(loading);
        $(elt).addClass("position-relative");
    }
    else {
        elt.find(".agp-white-loading").remove();
        elt.removeClass("position-relative");
    }
}

$.fn.AGActivityRemark = function (aobjectlink,myImageUrl) {

    var container = $("<div/>", {
        class: "system-message-comment-container"
    });

    $(this).append(container);

    $(container).activityRemark({
        myImagePath: myImageUrl,
        aobjectlink: aobjectlink,
        pagemode: "ACTIVITY",
        allowDivider: true,
        islazyLoad: true,
        isContinue: true,
        showFavorite: true,
        reverse:true,
        onContinue: function () {
        },
        getData: {
            url: servictWebDomainName + "framework/ag-activity-remark/api/",
            key: {
                q: "taskremark-lazyload",
                obj: aobjectlink,

            }
        },
        postData: {
            url: servictWebDomainName + 'framework/ag-activity-remark/api/',
            key: {
                q: "postremark",
                aobj: aobjectlink
            }
            //isQuote : AUTO_POST
            //quoteMessage : AUTO_POST
            //quoteType : AUTO_POST
            //remarkMessage : AUTO_POST
            //remarkType : AUTO_POST
            //sendMail : AUTO_POST
        },
        editData: {
            url: servictWebDomainName + "framework/ag-activity-remark/api/",
            key: {
                q: "editremark"
            }
            //remarkKey : AUTO_POST
            //remarkMessage : AUTO_POST
        },
        favoriteData: {
            url: servictWebDomainName + "framework/ag-activity-remark/api/",
            key: {
                q: "favorite",
                aobjectlink: aobjectlink,
                classID: "",
                courseID: ""
            }
            //messageSeq : AUTO_POST
        }
    });
}

function bindDateTimeControl() {
    setDateTimePicker();
    setDatePicker();
}

function setDateTimePicker() {
    $(".datetime-picker").datetimepicker({
        format: "dd/mm/yyyy hh:ii",
        autoclose: true,
        todayBtn: true,
        minuteStep: 10
    });
    $(".datetime-picker").next(".input-group-addon").click(function () {
        $(this).prev().focus();
    });


    $(".date-picker").datetimepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayBtn: true,
        minView: 'month'
    });
    $(".date-picker").next(".input-group-addon").click(function () {
        $(this).prev().focus();
    });
}

function setDatePicker() {
    $(".date-picker").datetimepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayBtn: true
    });
    $(".date-picker").next(".input-group-addon").click(function () {
        $(this).prev().focus();
    });
}

var _disableAutoLoading = false;

function disableAutoLoading(flag) {
    _disableAutoLoading = flag;
}

window.onbeforeunload = function () {
    try {
        if (!_disableAutoLoading && $(".ag-loading").length == 0) {
            AGLoading(true, "กำลังโหลด");
            $(".ag-loading img").remove();
        }
    }
    catch (e) { }
}