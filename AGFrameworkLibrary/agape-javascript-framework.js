
var isInIframe = (window.location != window.parent.location) ? true : false;

function OpenSessionTimedOutFade() {
    if (isInIframe) {
        window.parent.OpenSessionTimedOutFade();
    } else {
        $('.agro-loading-error').fadeIn();
    }
}

function RedirectPage(page) {
    if (isInIframe) {
        window.location.href = page;
    } else {
        window.location.href = page;
    }
}

function AGLoading(flag, message) {

    if (isInIframe) {
        try {
            window.parent.AGLoading(flag, message);
            return;
        }
        catch (e) { }
    }

    var loadingImage = "/images/focusone-loading.gif?vs=1.2";

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
                bottom: 0,
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
                height: 80
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
    AGAlertTimeout(function () {
        swal({
            title: "",
            text: message,
            type: "error",
            customClass: 'swal-wide',
            html: true
        });
    });
}

function AGInfo(message) {
    AGAlertTimeout(function () {
        swal({
            title: "",
            text: message,
            type: "info",
            customClass: 'swal-wide',
            html: true
        });
    });
}


function AGSuccess(message) {
    AGAlertTimeout(function () {
        swal({
            title: "",
            text: message,
            type: "success",
            customClass: 'swal-wide',
            confirmButtonText: "ตกลง",
            html: true
        })
    });
}
function AGSuccess(message, idBtnAfterClickOK) {
    swal({
        title: "",
        text: message,
        type: "success",
        customClass: 'swal-wide',
        html: true
    }, function (isConfirm) {
        var btn = $("#" + idBtnAfterClickOK);
        if ($(btn).length > 0) {
            $(btn).click();
        }

    });
}
function AGMessage(message) {
    AGAlertTimeout(function () {
        swal({
            title: "",
            text: message,
            type: "",
            customClass: 'swal-wide',
            html: true
        });
    });
}

function AGAlertTimeout(fnc) {
    setTimeout(function () {
        fnc();
    }, 500);
}

var ConfirmFlag = undefined;

function AGConfirm(sender, message) {
    if (message == undefined && typeof (sender) == "string") {
        message = sender;
        sender = $(window.event.target);
    }

    if (ConfirmFlag == undefined) {
        AGAlertTimeout(function () {
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
            src: "/images/loadmessage.gif",
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

$.fn.AGActivityRemark = function (aobjectlink, myImageUrl, isEasyMode, allowDivider, showFavorite, isContinue, reverse) {

    var container = $("<div/>", {
        class: "system-message-comment-container"
    });

    $(this).append(container);

    container.activityRemark({
        myImagePath: myImageUrl,
        aobjectlink: aobjectlink,
        pagemode: "ACTIVITY",
        allowDivider: allowDivider == undefined ? true : allowDivider,
        islazyLoad: true,
        isContinue: isContinue == undefined ? true : isContinue,
        showFavorite: showFavorite == undefined ? true : showFavorite,
        isEasyMode: isEasyMode == undefined ? false : isEasyMode,
        reverse: reverse == undefined ? true : reverse,
        onContinue: function () {
        },
        getData: {
            url: "/framework/ag-activity-remark/api/",
            key: {
                q: "taskremark-lazyload",
                obj: aobjectlink,

            }
        },
        postData: {
            url: '/framework/ag-activity-remark/api/',
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
            url: "/framework/ag-activity-remark/api/",
            key: {
                q: "editremark"
            }
            //remarkKey : AUTO_POST
            //remarkMessage : AUTO_POST
        },
        favoriteData: {
            url: "/framework/ag-activity-remark/api/",
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
    setTimePicker();
}

function setTimePicker() {
    var timePicker = $(".time-picker");
    if (timePicker.length > 0) {
        timePicker.datetimepicker({
            format: "hh:ii",
            minuteStep: 5,
            autoclose: true,
            minView: 0,
            maxView: 1,
            startView: 1,
        });
        //timePicker.on('changeDate', function (ev) {
        //    setTimeout(function (dp) {
        //        var val = dp.val().split(':');
        //        var newVal = val[0] + ":00";
        //        dp.val(newVal);
        //    }, 100, $(this));
        //});
        timePicker.each(function () {
            $(this).data('datetimepicker').picker.addClass('timepicker');
        });
        timePicker.next(".input-group-addon").click(function () {
            $(this).prev().focus();
        });

        $("#timepicker-css").remove();
        $("head").append($("<style/>", {
            id: "timepicker-css",
            html: ".timepicker{overflow: hidden;}.timepicker .datetimepicker-hours,.timepicker .datetimepicker-minutes > table{margin-top: -30px;}"
        }));
    }
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
}

function setDatePicker() {
    $(".date-picker").datetimepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayBtn: true,
        minView: 2
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
        if (!_disableAutoLoading && $(".ag-loading").length == 0 && !$(".agro-loading").is(":visible") && !isInIframe) {
            AGLoading(true, "กำลังโหลด");
            $(".ag-loading img").remove();
        }
    }
    catch (e) { }
}