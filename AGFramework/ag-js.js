function AGLoading(flag, message) {
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

function AGError(message, inputTimeout) {
    var timeout = 1;

    if (inputTimeout) {
        timeout = inputTimeout;
    }

    setTimeout(function () {
        swal({
            title: "",
            text: message,
            type: "error",
            customClass: 'swal-wide',
            html: true
        });
    }, timeout);
}

function AGInfo(message, inputTimeout) {
    var timeout = 1;

    if (inputTimeout) {
        timeout = inputTimeout;
    }

    setTimeout(function () {
        swal({
            title: "",
            text: message,
            type: "info",
            customClass: 'swal-wide',
            html: true
        });
    }, timeout);
}

function AGSuccess(message, inputTimeout) {
    var timeout = 1;

    if (inputTimeout) {
        timeout = inputTimeout;
    }

    setTimeout(function () {
        swal({
            title: "",
            text: message,
            type: "success",
            customClass: 'swal-wide',
            html: true
        });
    }, timeout);
}

function AGMessage(message, inputTimeout) {
    var timeout = 1;

    if (inputTimeout) {
        timeout = inputTimeout;
    }

    setTimeout(function () {
        swal({
            title: "",
            text: message,
            type: "",
            customClass: 'swal-wide',
            html: true
        });
    }, timeout);
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

$.fn.AGActivityRemark = function (aobjectlink, myImageUrl, callBackPostSuccess, callBackInitSuccess) {

    var container = $("<div/>", {
        class: "system-message-comment-container"
    });

    $(this).append(container);

    $(container).activityRemark({
        myImagePath: myImageUrl,
        aobjectlink: aobjectlink,
        allowDivider: false,
        islazyLoad: true,
        lazyLoadTotal: 5,
        callBackPostSuccess: callBackPostSuccess,
        callBackInitSuccess: callBackInitSuccess,
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
        },
        editData: {
            url: servictWebDomainName + "framework/ag-activity-remark/api/",
            key: {
                q: "editremark"
            }         
        },
        getRefEmailContent: {
            url: servictWebDomainName + 'framework/ag-activity-remark/api/SystemMessageFormAPI.aspx',
            key: {
                type: "getrefemail",
                aobj: aobjectlink
            }
            //refEmailCode : AUTO_POST
        }
    });
}

function bindDateTimeControl() {
    setDateTimePicker();
    setDatePicker();
    setTimePicker();
}

function setDateTimePicker() {
    $(".datetime-picker").datetimepicker({
        format: "dd/mm/yyyy hh:ii",
        autoclose: true,
        todayBtn: true,
        minuteStep: 10
    });
    $(".datetime-picker").next(".input-group-append").click(function () {
        $(this).prev().focus();
    });    
}

function setDatePicker() {
    $(".date-picker").datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayBtn: "linked",
        todayHighlight: true        
    });
    $(".date-picker").next(".input-group-append").click(function () {
        $(this).prev().focus();
    });
    $(".date-picker").attr("autocomplete", "off");
}

function setTimePicker() {
    $(".time-picker").datetimepicker({
        format: "hh:ii",
        minuteStep: 5,
        autoclose: true,
        minView: 0,
        maxView: 1,
        startView: 1,
        showHeader: false,
        pickerPosition: "top-right"
    });
    $(".time-picker").next(".input-group-append").click(function () {
        $(this).prev().focus();
    });
    $(".time-picker").attr("autocomplete", "off");
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

function OpenSessionTimedOutFade() {
    var isInIframe = (window.location != window.parent.location) ? true : false;
    if (isInIframe) {
        window.parent.OpenSessionTimedOutFade();
    } else {
        $('.agro-loading-error').fadeIn();
    }
}