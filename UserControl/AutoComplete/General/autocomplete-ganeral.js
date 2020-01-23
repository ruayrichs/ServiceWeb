
function bindAutocompleteGeneral(clientid, data, defaultValue, isLoadNewData) {
    if (data == null || data == undefined)
    {
        data = [];
    }
    $(".panel-autocomplete-" + clientid).AGWhiteLoading(true);
    $(".panel-autocomplete-" + clientid).find(".agp-white-loading").find("label").html("");

    var displaybox = $(".panel-autocomplete-" + clientid).find(".Description");
    var codebox = $(".panel-autocomplete-" + clientid).find(".Code");
    var namebox = $(".panel-autocomplete-" + clientid).find(".Name");
    var actionValue = $(".panel-autocomplete-" + clientid).find(".ActionCase").val();
    var AfterSelectedChange = $(".panel-autocomplete-" + clientid).find(".AfterSelectedChange").val();
    var callBack = AfterSelectedChange == "" || AfterSelectedChange == null ? undefined : AfterSelectedChange;
    var timeout;

    var LoadServerRefEvent = true;
    if (isLoadNewData != null && isLoadNewData != undefined) {
        LoadServerRefEvent = isLoadNewData;
    }

    var DB = new JQL(data);

    $(displaybox).typeahead('destroy');
    $(displaybox).typeahead({
        hint: true,
        highlight: true,
        minLength: 0
    }, {
        limit: 20,
        templates: {
            pending: '<div class="text-danger" style="padding: 2px 10px; line-height: 24px;">Result not found.</div>',
            suggestion: function (data) {
                return '<div>' + (data.detail == null || data.detail == undefined || data.detail == "" ? data.display : data.detail) + '</div>';
            }
        },
        source: function (str, callback, serverCallback) {

            var selectResult = getSearchResultAutocompleteGeneral(DB, str);

            // Search ข้อมูลใหม่ถ้า Select TOP (1000) แล้วไม่เจอ หรือ น้อยกว่า 20
            //if (selectResult.length == 0) {
            if (LoadServerRefEvent && selectResult.length < 20) {

                var nTimeout = 500;
                if (str == "") {
                    nTimeout = 0;
                }

                if (timeout) {
                    clearTimeout(timeout);
                    timeout = null;
                }
                
                timeout = setTimeout(function () {

                        setTimeout(function () {
                            $(".panel-autocomplete-" + clientid).AGWhiteLoading(true);
                            $(".panel-autocomplete-" + clientid).find(".agp-white-loading").find("label").html("");
                        }, 500);
                        var postData = {
                            actionCase: actionValue,
                            keySearch: str
                        };

                        $.ajax({
                            type: "POST",
                            url: servictWebDomainName + "API/AutoCompleteGeneralAPI.aspx",
                            data: postData,
                            success: function (data) {
                                DB = new JQL(data);
                                selectResult = getSearchResultAutocompleteGeneral(DB, str);
                                serverCallback(selectResult);
                                setTimeout(function () {
                                    $(".panel-autocomplete-" + clientid).AGWhiteLoading(false);
                                }, 500);
                            }
                        });

                }, nTimeout);
                

                
            } else {
                callback(selectResult);
            }
        },
        display: function (data) {
            return data.display;
        }
    });

    $(displaybox).bind('typeahead:change', function (e, v) {

        if (v != null && v != undefined)
        {
            if (v.trim() == "") {
                if ($(codebox).val() != "") {
                    $(codebox).val("");
                    $(namebox).val("");
                    try {
                        //TODO Outher function
                        if (callBack) {
                            eval(callBack);
                        }
                    } catch (e) { }
                }
                $(namebox).val("");
            }
        }
    });

    $(displaybox).bind('typeahead:select typeahead:autocomplete', function (e, v) {

        if ($(codebox).val() != v.code) {
            $(codebox).val(v.code);
            $(namebox).val(v.name);
            try {
                //TODO Outher function
                if (callBack) {
                    eval(callBack);
                }
            } catch (e) { }
        }
        $(namebox).val(v.name);
    });

    if (defaultValue == null || defaultValue == undefined) {
        defaultValue = "";
        $(codebox).val("");
        $(namebox).val("");
    }

    $(displaybox).typeahead('val', defaultValue);

    if (defaultValue != "") {
        var temp = defaultValue.split(":");
        var code = "";
        var name = "";
        if (temp.length > 1) {
            code = temp[0].trim();
            name = temp[1].trim();
        } else
        {
            code = temp[0].trim();
            name = "";
        }
        $(codebox).val(code);
        $(namebox).val(name);
    }

    $(".panel-autocomplete-" + clientid).AGWhiteLoading(false);
}



function getSearchResultAutocompleteGeneral(datas, str) {
    var selectCode = datas.select('*').where('code').match(str).fetch();
    var selectName = datas.select('*').where('name').match(str).fetch();
    var selectDisplay = datas.select('*').where('display').match(str).fetch();
    var selectDetail = datas.select('*').where('detail').match(str).fetch();
    var selectResult = [];

    if (selectCode.length > 0) {
        for (var i = 0; i < selectCode.length; i++) {
            selectResult.push(selectCode[i]);
        }
    }

    if (selectName.length > 0) {
        for (var i = 0; i < selectName.length; i++) {
            var hasCode = jQuery.grep(selectResult, function (element) {
                return element.code == selectName[i].code;
            });

            if (hasCode.length == 0) {
                selectResult.push(selectName[i]);
            }
        }
    }

    if (selectDisplay.length > 0) {
        for (var i = 0; i < selectDisplay.length; i++) {
            var hasCode = jQuery.grep(selectResult, function (element) {
                return element.code == selectDisplay[i].code;
            });

            if (hasCode.length == 0) {
                selectResult.push(selectDisplay[i]);
            }
        }
    }

    if (selectDetail.length > 0) {
        for (var i = 0; i < selectDetail.length; i++) {
            var hasCode = jQuery.grep(selectResult, function (element) {
                return element.code == selectDetail[i].code;
            });

            if (hasCode.length == 0) {
                selectResult.push(selectDetail[i]);
            }
        }
    }

    return selectResult;
}

//function loadWithoutConditionAutocompleteGeneral(clientid) {
//    var action = $(".panel-autocomplete-" + clientid + " .ActionCase").val();
//    var postData = {
//        actionCase: action
//    };

//    $.ajax({
//        type: "POST",
//        url: servictWebDomainName + "API/AutoCompleteGeneralAPI.aspx",
//        data: postData,
//        success: function (data) {
//            bindAutocompleteGeneral(clientid, data);
//        }
//    });
//}