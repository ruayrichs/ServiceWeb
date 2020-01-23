<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutoCompleteCustomer.ascx.cs" Inherits="ServiceWeb.UserControl.AutoComplete.AutoCompleteCustomer" %>

<div id="divAotuCompleteCustomer<%= ClientID %>" class="autocomplete-box">
<asp:TextBox ID="tbCustomer" runat="server" autocomplete="off"></asp:TextBox>
<asp:HiddenField ID="hdfCustomerCode" runat="server" />
<asp:HiddenField ID="hdfCustomerName" runat="server" />
</div>

<script>   


    var loadCustomerServer<%= ClientID %> = false;
    <% if (!NotAutoBindComplete)
       { %>
    $(document).ready(function () {
        loadCustomerWithoutCondition<%= ClientID %>();
    });    
    <% } %>
    function loadCustomerWithoutCondition() {
        loadCustomerWithoutCondition<%= ClientID %>();
    }
    function loadCustomerWithoutCondition<%= ClientID %>() {
        var postData = {
            actionCase: "customer"
        };

        $.ajax({
            type: "POST",
            url: servictWebDomainName + "API/AutoCompleteAPI.aspx",
            data: postData,
            success: function (data) {                
                bindAutoCompleteCustomer<%= ClientID %>(data);
            }
        });
    }

    function getSearchResultCustomer(datas, str)
    {
        getSearchResultCustomer<%= ClientID %>(datas, str);
    }

    function getSearchResultCustomer<%= ClientID %>(datas, str)
    {
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

    xCount = 0;
    function bindAutoCompleteCustomer(data, defaultValue) { bindAutoCompleteCustomer<%= ClientID %>(data, defaultValue); }
    function bindAutoCompleteCustomer<%= ClientID %>(data, defaultValue, isLoadNewData) {
        $("#divAotuCompleteCustomer<%= ClientID %>").AGWhiteLoading(true);
        $("#divAotuCompleteCustomer<%= ClientID %>").find(".agp-white-loading").find("label").html("");

        loadCustomerServer<%= ClientID %> = false;


        var LoadCustomerRefEvent = true;
        if (isLoadNewData != null && isLoadNewData != undefined)
        {
            LoadCustomerRefEvent = isLoadNewData;
        }

        var DB = new JQL(data);

        $("#<%= tbCustomer.ClientID %>").typeahead('destroy');
        $("#<%= tbCustomer.ClientID %>").typeahead({
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
                bindingDataAutoComplete<%= ClientID %>(str, callback, serverCallback, DB, LoadCustomerRefEvent);
            },
            <%--source: function (str, callback, serverCallback) {

                var selectResult = getSearchResultCustomer<%= ClientID %>(DB, str);

                // Search ข้อมูลใหม่ถ้า Select TOP (1000) แล้วไม่เจอ
                //if (selectResult.length == 0) {

                if (LoadCustomerRefEvent) {
                    setTimeout(function () {
                        $("#divAotuCompleteCustomer<%= ClientID %>").AGWhiteLoading(true);
                        $("#divAotuCompleteCustomer<%= ClientID %>").find(".agp-white-loading").find("label").html("");
                    }, 500);
                    var postData = {
                        actionCase: "customer",
                        keySearch: str
                    };

                    console.log(xCount++);
                    $.ajax({
                        type: "POST",
                        url: servictWebDomainName + "API/AutoCompleteAPI.aspx",
                        data: postData,
                        success: function (data) {
                            loadCustomerServer<%= ClientID %> = true;
                            DB = new JQL(data);
                            selectResult = getSearchResultCustomer<%= ClientID %>(DB, str);
                            serverCallback(selectResult);
                            setTimeout(function () {
                                $("#divAotuCompleteCustomer<%= ClientID %>").AGWhiteLoading(false);
                             }, 500);
                        }
                    });
                } else {
                    callback(selectResult);
                }
            },--%>
            display: function (data) {
                return data.display;
            }
        });

        $("#<%= tbCustomer.ClientID %>").bind('typeahead:change', function (e, v) {

            if (v.trim() == "") {
                if ($("#<%= hdfCustomerCode.ClientID %>").val() != "")
                {
                    $("#<%= hdfCustomerCode.ClientID %>").val("");
                    $("#<%= hdfCustomerName.ClientID %>").val("");
                    <%= !string.IsNullOrEmpty(AfterSelectedChange) ? AfterSelectedChange : "" %>
                }
                $("#<%= hdfCustomerName.ClientID %>").val("");
            }
        });

        $("#<%= tbCustomer.ClientID %>").bind('typeahead:select typeahead:autocomplete', function (e, v) {
             
            if ($("#<%= hdfCustomerCode.ClientID %>").val() != v.code)
            {
                $("#<%= hdfCustomerCode.ClientID %>").val(v.code);
                $("#<%= hdfCustomerName.ClientID %>").val(v.desc); 
                <%= !string.IsNullOrEmpty(AfterSelectedChange) ? AfterSelectedChange : "" %>
            }
            $("#<%= hdfCustomerName.ClientID %>").val(v.desc);
        });

        if (defaultValue == null || defaultValue == undefined) {
            defaultValue = "";
            $("#<%= hdfCustomerCode.ClientID %>").val("");
            $("#<%= hdfCustomerName.ClientID %>").val("");
        }

        $("#<%= tbCustomer.ClientID %>").typeahead('val', defaultValue);

        if (defaultValue != "") {
            var temp = defaultValue.split(":");
            var code = temp[0].trim();
            var desc = temp[1].trim();

            $("#<%= hdfCustomerCode.ClientID %>").val(code);
            $("#<%= hdfCustomerName.ClientID %>").val(desc);     
        }

        $("#divAotuCompleteCustomer<%= ClientID %>").AGWhiteLoading(false);
    }

    var delayRequest<%= ClientID %>;
    function bindingDataAutoComplete<%= ClientID %>(str, callback, serverCallback, DB, LoadCustomerRefEvent) {
        
        var selectResult = getSearchResultCustomer<%= ClientID %>(DB, str);

        if (LoadCustomerRefEvent) {
            clearTimeout(delayRequest<%= ClientID %>);

            delayRequest<%= ClientID %> = setTimeout(function () {
                setTimeout(function () {
                    $("#divAotuCompleteCustomer<%= ClientID %>").AGWhiteLoading(true);
                    $("#divAotuCompleteCustomer<%= ClientID %>").find(".agp-white-loading").find("label").html("");
                }, 500);

                var postData = {
                    actionCase: "customer",
                    keySearch: str
                };

                console.log(xCount++);
                $.ajax({
                    type: "POST",
                    url: servictWebDomainName + "API/AutoCompleteAPI.aspx",
                    data: postData,
                    success: function (data) {
                        loadCustomerServer<%= ClientID %> = true;
                        DB = new JQL(data);
                        selectResult = getSearchResultCustomer<%= ClientID %>(DB, str);
                        serverCallback(selectResult);
                        setTimeout(function () {
                            $("#divAotuCompleteCustomer<%= ClientID %>").AGWhiteLoading(false);
                        }, 500);
                    }
                });
            }, 700);
        } else {
            callback(selectResult);
        }
    }

    function setDataAutoCompleteCustomer<%= ClientID %>(value) {
        $("#divAotuCompleteCustomer<%= ClientID %>").AGWhiteLoading(true);
        $("#divAotuCompleteCustomer<%= ClientID %>").find(".agp-white-loading").find("label").html("");
        var postData = {
            keySearch: value,
            actionCase: "customer"
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
                    $("#<%= tbCustomer.ClientID %>").val(data[0].display);
                    $("#<%= tbCustomer.ClientID %>").typeahead('val', data[0].display);
                    $("#<%= hdfCustomerCode.ClientID %>").val(data[0].code);
                    $("#<%= hdfCustomerName.ClientID %>").val(data[0].desc);
                }
                $("#divAotuCompleteCustomer<%= ClientID %>").AGWhiteLoading(false);
            }
        });

    }

</script>
