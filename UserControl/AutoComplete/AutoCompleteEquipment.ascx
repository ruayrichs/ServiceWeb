<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutoCompleteEquipment.ascx.cs" Inherits="ServiceWeb.UserControl.AutoComplete.AutoCompleteEquipment" %>

<div id="divAotuComplete<%= ClientID %>" class="autocomplete-box">
    <asp:TextBox ID="tbEquipment" runat="server" autocomplete="off"></asp:TextBox>
    <asp:HiddenField ID="hdfEquipmentCode" runat="server" />
    <asp:HiddenField ID="hdfEquipmentName" runat="server" />
</div>

<script>
    $(document).ready(function () {
        loadEquipmentWithoutCondition<%= ClientID %>();
    });

    var loadEquipmentServer = false;
    function loadEquipmentWithoutCondition() {
        loadEquipmentWithoutCondition<%= ClientID %>();
    }
    function loadEquipmentWithoutCondition<%= ClientID %>() {
        var postData = {
            actionCase: "equipment"
        };

        $.ajax({
            type: "POST",
            url: servictWebDomainName + "API/AutoCompleteAPI.aspx",
            data: postData,
            success: function (data) {
                bindAutoCompleteEquipment<%= ClientID %>(data);
            }
        });
        }

        function getSearchResultEquipment<%= ClientID %>(datas, str) {
        var selectCode = datas.select('*').where('code').match(str).fetch();
        var selectName = datas.select('*').where('desc').match(str).fetch();
        var selectDisplay = datas.select('*').where('display').match(str).fetch();
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

        return selectResult;
    }
    function bindAutoCompleteEquipment(data, defaultValue) {
        bindAutoCompleteEquipment<%= ClientID %>(data, defaultValue);
    }
    function bindAutoCompleteEquipment<%= ClientID %>(data, defaultValue, isLoadNewData) {

        $("#divAotuComplete<%= ClientID %>").AGWhiteLoading(true);
        $("#divAotuComplete<%= ClientID %>").find(".agp-white-loading").find("label").html("");

        loadEquipmentServer = false;
        var LoadCustomerRefEvent = true;
        if (isLoadNewData != null && isLoadNewData != undefined) {
            LoadCustomerRefEvent = isLoadNewData;
        }

        var DBEquipment = new JQL(data);

        $("#<%= tbEquipment.ClientID %>").typeahead('destroy');
        $("#<%= tbEquipment.ClientID %>").typeahead({
            hint: true,
            highlight: true,
            minLength: 0
        }, {
            limit: 20,
            templates: {
                pending: '<div class="text-danger" style="padding: 2px 10px; line-height: 24px;">Result not found.</div>',
                suggestion: function (data) {
                    return '<div>' + data.display + '</div>';
                }
            },
            source: function (str, callback, serverCallback) {
                bindingDataAutoComplete<%= ClientID %>(str, callback, serverCallback, DBEquipment, LoadCustomerRefEvent);
            },
            <%--source: function (str, equipmentCallback, equipmentAPICallback) {
                
                var selectResult = getSearchResultEquipment<%= ClientID %>(DBEquipment, str);

                // Search ข้อมูลใหม่ถ้า Select TOP (1000) แล้วไม่เจอ
                //if ((DBEquipment.data_source.length >= 1000 || loadEquipmentServer) && selectResult.length == 0) {
                if ((DBEquipment.data_source.length >= 1000 || loadEquipmentServer) && LoadCustomerRefEvent) {
                    setTimeout(function () {
                        $("#divAotuComplete<%= ClientID %>").AGWhiteLoading(true);
                        $("#divAotuComplete<%= ClientID %>").find(".agp-white-loading").find("label").html("");
                    }, 500);
                    var postData = {
                        actionCase: "equipment",
                        keySearch: str
                    };

                    $.ajax({    
                        type: "POST",
                        url: servictWebDomainName + "API/AutoCompleteAPI.aspx",
                        data: postData,
                        success: function (data) {
                            loadEquipmentServer = true;
                            DBEquipment = new JQL(data);
                            selectResult = getSearchResultEquipment<%= ClientID %>(DBEquipment, str);
                            equipmentAPICallback(selectResult);
                            setTimeout(function () {
                                $("#divAotuComplete<%= ClientID %>").AGWhiteLoading(false);
                            }, 500);
                        }
                    });
                } else {
                    equipmentCallback(selectResult);
                }                
            },--%>
            display: function (data) {
                return data.display;
            }
        });

        $("#<%= tbEquipment.ClientID %>").bind('typeahead:change', function (e, v) {

            if (v.trim() == "") {
                if ($("#<%= hdfEquipmentCode.ClientID %>").val() != v.code) {
                    $("#<%= hdfEquipmentCode.ClientID %>").val("");
                    $("#<%= hdfEquipmentName.ClientID %>").val("");

                  <%= !string.IsNullOrEmpty(AfterSelectedChange) ? AfterSelectedChange : "" %>
                }
                $("#<%= hdfEquipmentName.ClientID %>").val("");
            }
        });

        $("#<%= tbEquipment.ClientID %>").bind('typeahead:select typeahead:autocomplete', function (e, v) {
            if ($("#<%= hdfEquipmentCode.ClientID %>").val() != v.code) {
                $("#<%= hdfEquipmentCode.ClientID %>").val(v.code);
                $("#<%= hdfEquipmentName.ClientID %>").val(v.desc);
                <%= !string.IsNullOrEmpty(AfterSelectedChange) ? AfterSelectedChange : "" %>
            }
            $("#<%= hdfEquipmentName.ClientID %>").val(v.desc);
        });

        if (defaultValue == null || defaultValue == undefined) {
            defaultValue = "";
        }

        $("#<%= tbEquipment.ClientID %>").typeahead('val', defaultValue);

        if (defaultValue != "") {
            var temp = defaultValue.split(":");
            var code = temp[0].trim();
            var desc = temp[1].trim();

            $("#<%= hdfEquipmentCode.ClientID %>").val(code);
            $("#<%= hdfEquipmentName.ClientID %>").val(desc);
        }
        $("#divAotuComplete<%= ClientID %>").AGWhiteLoading(false);
    }

    var delayRequest<%= ClientID %>;
    function bindingDataAutoComplete<%= ClientID %>(str, equipmentCallback, equipmentAPICallback, DBEquipment, LoadCustomerRefEvent) {
        var callback = equipmentCallback;
        var serverCallback = equipmentAPICallback;


        var selectResult = getSearchResultEquipment<%= ClientID %>(DBEquipment, str);

        // Search ข้อมูลใหม่ถ้า Select TOP (1000) แล้วไม่เจอ
        //if ((DBEquipment.data_source.length >= 1000 || loadEquipmentServer) && selectResult.length == 0) {
        if ((DBEquipment.data_source.length >= 1000 || loadEquipmentServer) && LoadCustomerRefEvent) {
            clearTimeout(delayRequest<%= ClientID %>);
                delayRequest<%= ClientID %> = setTimeout(function () {
                    setTimeout(function () {
                        $("#divAotuComplete<%= ClientID %>").AGWhiteLoading(true);
                        $("#divAotuComplete<%= ClientID %>").find(".agp-white-loading").find("label").html("");
                    }, 500);
                    var postData = {
                        actionCase: "equipment",
                        keySearch: str
                    };

                    console.log(xCount++);
                    $.ajax({
                        type: "POST",
                        url: servictWebDomainName + "API/AutoCompleteAPI.aspx",
                        data: postData,
                        success: function (data) {
                            loadEquipmentServer = true;
                            DBEquipment = new JQL(data);
                            selectResult = getSearchResultEquipment<%= ClientID %>(DBEquipment, str);
                        serverCallback(selectResult);
                        setTimeout(function () {
                            $("#divAotuComplete<%= ClientID %>").AGWhiteLoading(false);
                        }, 500);
                    }
                });
                }, 700);
        } else {
            callback(selectResult);
        }
    }

    function setDataAutoCompleteEquipment<%= ClientID %>(value) {
        $("#divAotuComplete<%= ClientID %>").AGWhiteLoading(true);
        $("#divAotuComplete<%= ClientID %>").find(".agp-white-loading").find("label").html("");
        var postData = {
            keySearch: value,
            actionCase: "equipment"
        };

        $.ajax({
            type: "POST",
            url: servictWebDomainName + "API/AutoCompleteAPI.aspx",
            data: postData,
            success: function (data) {
                data = $.grep(data, function (v) {
                    return v.code == value;
                });
                if (data != undefined && data != null && data.length > 0) {
                    $("#<%= tbEquipment.ClientID %>").val(data[0].display);
                    $("#<%= tbEquipment.ClientID %>").typeahead('val', data[0].display);
                    $("#<%= hdfEquipmentCode.ClientID %>").val(data[0].code);
                    $("#<%= hdfEquipmentName.ClientID %>").val(data[0].desc);
                }
                $("#divAotuComplete<%= ClientID %>").AGWhiteLoading(false);
            }
        });

    }
</script>
