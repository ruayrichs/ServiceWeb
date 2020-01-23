$.fn.smartSearchKeyUp = function () {
    var formControl = $(this).find(".form-control-smart-search");
    formControl.click();
}

$.fn.noDataMatch = function () {
    var formPopUp = $(this).find(".smart-search-popup");
    formPopUp.find(".smart-search-popup-empty").html('No matching data.');
    formPopUp.find(".smart-search-popup-empty").show();
    formPopUp.find(".smart-search-popup-result").hide();
}

$.fn.smartSearch = function (datas) {
    var elt = $(this);
    var formControl = $(this).find(".form-control-smart-search");
    var formPopUp = $(this).find(".smart-search-popup");
    var formIcon = $(this).find(".smart-search-icon");
    var onSelected = datas.onSelected;
    var onAfterSelected = datas.onAfterSelected;
    var onKeyUp = datas.onKeyUp;
    var enableMutiSelect = datas.enableMutiSelect == undefined ? false : datas.enableMutiSelect;
    var width = datas.width == undefined ? "100%" : datas.width;
    var left = datas.left == undefined ? "0" : datas.left;

    if (formPopUp.find(".smart-search-popup-close").length <= 0) {
        formPopUp.prepend(
            "<div class='text-right smart-search-popup-close'>" +
                (enableMutiSelect ? "<span class='pull-left enable-multiple-selection'>Enable multiple selection</span>" : "") +
                "<i class='fa fa-remove smart-search-popup-close-btn'></i>" +
            "</div>" +
            "<div class='text-center smart-search-popup-empty'>" +
                "no matching data" +
            "</div>"
        ).css({
            width: width
        });
    }

    formControl.attr("autocomplete", "off");

    formControl.unbind("keyup").bind("keyup",function (e) {
        var code = e.keyCode || e.which;
        if (
            code != 38 &&
            code != 40 &&
            code != 9 &&
            code != 13) {
            onSearchTextChange(this.value);
        }
    });

    formControl.unbind("keydown").bind("keydown",function (e) {
        var code = e.keyCode || e.which;
        if (code == 38) {
            onKeyArrowUpDown("up");
        }
        else if (code == 40) {
            onKeyArrowUpDown("down");
        }
        else if(code == 13)
        {
            onKeyChoose();
        }
        else if (code == 9) {
            onSearchBlur();
            return;
        }
        else {
            return;
        }
        e.preventDefault();
    });

    formControl.unbind("click").bind("click",function (e) {
        onSearchFocus();
        e.stopPropagation();
        e.preventDefault();
    });

    formIcon.unbind("click").bind("click", function (e) {
        onSearchFocus();
        e.stopPropagation();
        e.preventDefault();
    });

    formPopUp.unbind("click").bind("click", function (e) {
        e.stopPropagation();
        e.preventDefault();
    });

    formPopUp.find(".smart-search-popup-close .smart-search-popup-close-btn").unbind("click").bind("click",function () {
        $(this).closest(formPopUp).fadeOut();
    });

    formPopUp.find(".smart-search-popup-result-row").unbind("click").bind("click",function (e) {
        var code = $(this).attr("data-code");
        var value = $(this).attr("data-value") == undefined ? "" : $(this).attr("data-value");
        onSelected(code, value, $(this));
        if (!enableMutiSelect) {
            onSearchBlur();
        }
    });

    $(document).click(function () {
        onSearchBlur();
    });

    function onSearchFocus() {
        formPopUp.show();
        $(formControl).select();
        var val = "";

        if (onKeyUp != null && onKeyUp != undefined) {
            val = $(formControl).val();
            if (formPopUp.find(".smart-search-popup-empty").html() != "No matching data.") {
                onSearchTextChange(val);
            }
        }
        else {
            val = enableMutiSelect ? $(formControl).val() : "";
            onSearchTextChange(val);
        }        
    }

    function onSearchBlur() {
        if (formPopUp.is(":visible")) {
            $(formControl).val(onAfterSelected());
            formPopUp.hide();
        }
    }

    function onKeyChoose() {
        var focus = formPopUp.find(".smart-search-popup-result-row-focus");
        if (focus.length > 0) {
            focus.first().click();
        }
    }

    function onKeyArrowUpDown(key) {
        formPopUp.show();
        var focus = formPopUp.find(".smart-search-popup-result-row-focus");
        if (focus.length > 0) {
            var elt;
            if (key == "up") {
                elt = focus.first().prevAll(":visible").first();
            }
            if (key == "down") {
                elt = focus.first().nextAll(":visible").first();
            }
            if (elt.length > 0) {
                focus.first().removeClass("smart-search-popup-result-row-focus");
                elt.addClass("smart-search-popup-result-row-focus");

                scrollToRow(elt);
            }
        }
    }

    function scrollToRow(elt) {
        if (elt.length == 0)
            return;

        var visibleIndex = 0;
        formPopUp.find(".smart-search-popup-result-row:visible").each(function (index) {
            if ($(this).hasClass("smart-search-popup-result-row-focus")) {
                visibleIndex = index;
            }
        });
        formPopUp.find(".smart-search-popup-result").scrollTop(visibleIndex * elt.height());
    }

    function onSearchTextChange(val) {
        formPopUp.find(".smart-search-popup-empty").hide();
        formPopUp.find(".smart-search-popup-result").show();
        formPopUp.show();

        var row = formPopUp.find(".smart-search-popup-result-row");
        if (val.trim() == "") {
            row.each(function () {
                $(this).find(".smart-search-popup-result-code").html($(this).find(".smart-search-popup-result-code-temp").html());
                $(this).find(".smart-search-popup-result-value").html($(this).find(".smart-search-popup-result-value-temp").html());
            });
            row.show();
        }
        else {
            row.each(function () {
                var code = $(this).find(".smart-search-popup-result-code-temp").html();
                var value = $(this).find(".smart-search-popup-result-value-temp").html();
                if (code.toLowerCase().match(val.toLowerCase()) || value.toLowerCase().match(val.toLowerCase())) {
                    $(this).show();
                    code = highlight(code, val);
                    value = highlight(value, val);
                }
                else {
                    $(this).hide();
                }

                $(this).find(".smart-search-popup-result-code").html(code);
                $(this).find(".smart-search-popup-result-value").html(value);
            });
        }

        if (formPopUp.find(".smart-search-popup-result-row:visible").length > 0) {
            formPopUp.find(".smart-search-popup-empty").hide();
            formPopUp.find(".smart-search-popup-result").show();
            
            var isScroll = false
            if (!enableMutiSelect || formPopUp.find(".smart-search-popup-result-row-focus:visible").length == 0) {
                row.removeClass("smart-search-popup-result-row-focus");
                isScroll = true;
            }
            if (!enableMutiSelect || formPopUp.find(".smart-search-popup-result-row-focus").length == 0) {
                formPopUp.find(".smart-search-popup-result-row:visible").first().addClass("smart-search-popup-result-row-focus");
            }
            if (isScroll) {
                scrollToRow(formPopUp.find(".smart-search-popup-result-row-focus").first());
            }
        }
        else {
            row.removeClass("smart-search-popup-result-row-focus");
            formPopUp.find(".smart-search-popup-empty").show();
            formPopUp.find(".smart-search-popup-result").hide();

            if (onKeyUp != null && onKeyUp != undefined) {

                formPopUp.find(".smart-search-popup-empty").html("<img src='../../../images/loadmessage.gif' alt='Alternate Text' style='width: 20px; margin-right: 5px;' /><b><i>กำลังค้นหา... " + val + "</i></b>");

                var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx-'.replace(/[xy]/g, function (c) {
                    var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                    return v.toString(16);
                });

                formPopUp.find(".smart-search-result-box").remove();

                var resultBox = $("<div/>", {
                    class: "smart-search-result-box",
                    id: "search-" + guid
                });

                formPopUp.find(".smart-search-popup-result").append(resultBox);

                setTimeout(function () {
                    if ($("#search-" + guid).length > 0) {
                        onKeyUp(val);
                    }
                }, 3000);                             
            }            
        }
    }   

    function highlight(data, search) {
        return data.replace(new RegExp("(" + preg_quote(search) + ")", 'gi'), "<span style='background:yellow'>$1<span>");
    }

    function preg_quote(str) {
        return (str + '').replace(/([\\\.\+\*\?\[\^\]\$\(\)\{\}\=\!\<\>\|\:])/g, "\\$1");
    }
}
