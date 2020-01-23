<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutoCompleteEmployee.ascx.cs" Inherits="ServiceWeb.UserControl.AutoComplete.AutoCompleteEmployee" %>

<asp:TextBox ID="tbEmployee" runat="server" autocomplete="off"></asp:TextBox>
<asp:HiddenField ID="hdfEmployeeCode" runat="server" />
<asp:HiddenField ID="hdfEmployeeName" runat="server" />
<asp:HiddenField ID="hddAfterSelectedChange" runat="server" />
<div class="d-none">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpOption">
        <ContentTemplate>
            <asp:HiddenField ID="hddAutoCompleteEnabled" runat="server" Value="true" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<script>   
    $(document).ready(function () {
        loadEmployeeWithoutCondition<%= ClientID %>();
    });

    var loadEmployeeServer<%= ClientID %> = false;

    function loadEmployeeWithoutCondition<%= ClientID %>() {
        var postData = {
            actionCase: "employee"
        };

        $.ajax({
            type: "POST",
            url: servictWebDomainName + "API/AutoCompleteAPI.aspx",
            data: postData,
            success: function (data) {                
                bindAutoCompleteEmployee<%= ClientID %>(data);
            }
        });
    }    

    function getSearchResultEmployee<%= ClientID %>(datas, str) {
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

    function bindAutoCompleteEmployee<%= ClientID %>(data, defaultValue) {
        loadEmployeeServer<%= ClientID %> = false;

        var DB = new JQL(data);

        $("#<%= tbEmployee.ClientID %>").typeahead('destroy');
        $("#<%= tbEmployee.ClientID %>").typeahead({
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

                var selectResult = getSearchResultEmployee<%= ClientID %>(DB, str);

                // Search ข้อมูลใหม่ถ้า Select TOP (1000) แล้วไม่เจอ
                if ((DB.data_source.length >= 1000 || loadEmployeeServer<%= ClientID %>) && selectResult.length == 0) {
                    var postData = {
                        actionCase: "employee",
                        keySearch: str
                    };

                    $.ajax({
                        type: "POST",
                        url: servictWebDomainName + "API/AutoCompleteAPI.aspx",
                        data: postData,
                        success: function (data) {
                            loadEmployeeServer<%= ClientID %> = true;
                            DB = new JQL(data);
                            selectResult = getSearchResultEmployee<%= ClientID %>(DB, str);
                            serverCallback(selectResult);
                        }
                    });
                } else {
                    callback(selectResult);
                }
            },
            display: function (data) {
                return data.display;
            }
        });

        $("#<%= tbEmployee.ClientID %>").bind('typeahead:change', function (e, v) {

            if (v.trim() == "") {
                $("#<%= hdfEmployeeCode.ClientID %>").val("");
                $("#<%= hdfEmployeeName.ClientID %>").val("");

                <%= !string.IsNullOrEmpty(AfterSelectedChange) ? AfterSelectedChange : "" %>
            }
        });

        $("#<%= tbEmployee.ClientID %>").bind('typeahead:select typeahead:autocomplete', function (e, v) {
            $("#<%= hdfEmployeeCode.ClientID %>").val(v.code);
            $("#<%= hdfEmployeeName.ClientID %>").val(v.desc);            

            <%= !string.IsNullOrEmpty(AfterSelectedChange) ? AfterSelectedChange : "" %>
        });

        if (defaultValue == null || defaultValue == undefined) {
            defaultValue = "";
            var empCode = $("#<%= hdfEmployeeCode.ClientID %>").val();
            if (empCode != "")
            {
                var arr = jQuery.grep(data, function (a) {
                    return a.code == empCode;
                });
                if(arr.length > 0)
                {
                    defaultValue = arr[0].display;
                }
            }
        }
        $("#<%= tbEmployee.ClientID %>").typeahead('val', defaultValue);

        if (defaultValue != "") {
            var temp = defaultValue.split(":");
            var code = temp[0].trim();
            var desc = temp[1].trim();

            $("#<%= hdfEmployeeCode.ClientID %>").val(code);
            $("#<%= hdfEmployeeName.ClientID %>").val(desc);     
        }
    }

    function RefreshValueAutoCompleteEmployee<%= ClientID %>(Value)
    {
        setTimeout(function () {
            $("#<%= tbEmployee.ClientID %>").typeahead('val', Value);
            $("#<%= hdfEmployeeCode.ClientID %>").val("");
            $("#<%= hdfEmployeeName.ClientID %>").val("");
            if (Value != "") {
                //var temp = defaultValue.split(":");
                //var code = temp[0].trim();
                //var desc = temp[1].trim();

                //$("#<%= hdfEmployeeCode.ClientID %>").val(code);
                //$("#<%= hdfEmployeeName.ClientID %>").val(desc);

                $("#<%= hdfEmployeeCode.ClientID %>").val(Value);

            }
        }, 500);
    }
</script>
