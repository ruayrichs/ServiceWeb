<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmartSearchEmployee.ascx.cs" Inherits="ServiceWeb.widget.usercontrol.SmartSearchEmployee" %>
<link href="<%= Page.ResolveUrl("~/widget/Lib/smart-search-1.1.css") %>" rel="stylesheet" />
<script src="<%= Page.ResolveUrl("~/widget/Lib/smart-search.js?vs=20190113") %>"></script>

<style>
    .blue-require-style {
        border-left: 3px solid #9FA8DA;
    }
</style>

<div class="smart-main-employee-template-<%= ID %>" style="display: none;">
    <div class="col-sm-6 smart-main-employee-temp" onclick="$('.input-group-main-employee-<%= ID %> .form-control-smart-search').click();event.stopPropagation();" style="padding: 0 5px;">
        <div class="input-group" style="margin-bottom: 0px; margin-top: 0px;">
            <input type="text" readonly class="form-control" style="background: #eee" name="name" value=" " />
            <span class="input-group-append" onclick="$(this).closest('.smart-main-employee-temp').remove();$('.input-group-main-delegate-code-<%= ID %>,.input-group-main-delegate-value-<%= ID %>').val('');event.stopPropagation();" style="cursor: pointer">
                <i class="fa fa-remove input-group-text"></i>
            </span>
        </div>
    </div>
</div>
<div class="input-group">
    <div class="input-group-main-employee-<%= ID %> form-control blue-require-style" onclick="$('.input-group-main-employee-<%= ID %> .form-control-smart-search').click();event.stopPropagation();" style="padding: 5px 0px; height: auto;">
        <div class="smart-main-employee-result" style="margin-bottom: 0px; margin-top: 0px;">
            <div class="col-sm-6 smart-main-employee-search" style="padding: 0">
                <input style="border: none; outline: none; background: none; box-shadow: none;" class="form-control form-control-smart-search" type="text" placeholder="Search..." />
            </div>
        </div>
        <div class="smart-search-popup">
            <div class="smart-search-popup-result">
                <asp:UpdatePanel ID="udpnRepeaterEmploy" runat="server" UpdateMode="Conditional" OnLoad="udpnRepeaterEmploy_Load">
                    <ContentTemplate>
                        <table class="table" style="table-layout: fixed;">
                            <asp:Repeater runat="server" ID="rptSearchMainDelegate">
                                <ItemTemplate>
                                    <tr class="smart-search-popup-result-row" data-code="<%# Eval("EmployeeCode") %>" data-value="<%# Eval("FirstName_TH") + " " + Eval("LastName_TH")  %>">
                                        <td style="vertical-align: middle;">
                                            <p>
                                                <b style="color: #47a3da">Employee Code : </b><b class="smart-search-popup-result-code"><%# Eval("EmployeeCode") %></b>
                                                <b class="smart-search-popup-result-code-temp"><%# Eval("EmployeeCode") %></b>
                                            </p>
                                            <p>
                                                <b style="color: #47a3da">Employee Name : </b><b class="smart-search-popup-result-value"><%# Eval("FirstName_TH") + " " + Eval("LastName_TH") %></b>
                                                <b class="smart-search-popup-result-value-temp"><%# Eval("FirstName_TH") + " " + Eval("LastName_TH") %></b>
                                            </p>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <span class="input-group-append" onclick="$('.input-group-main-employee-<%= ID %> .form-control-smart-search').click();event.stopPropagation();" style="cursor: pointer;">
        <i class="fa fa-search input-group-text"></i>
    </span>
</div>

<div style="display: none">
    <asp:TextBox runat="server" ID="txtResultCode" />
    <asp:TextBox runat="server" ID="txtResultValue" />

</div>



<script>
    function rebind<%= ID %>() {
        $(".input-group-main-employee-<%= ID %>").smartSearch({
            width: "100%",
            onAfterSelected: function () {
                return "";
            },
            onSelected: function (code, value) {
                $(".input-group-main-employee-<%= ID %> .smart-main-employee-result .smart-main-employee-temp").remove();
                var temp = $(".smart-main-employee-template-<%= ID %> .smart-main-employee-temp").clone();
                temp.find("input").val(value);
                temp.attr("data-code", code);
                $(".input-group-main-employee-<%= ID %> .smart-main-employee-result").append(temp, $(".input-group-main-employee-<%= ID %> .smart-main-employee-search"));
                $('.input-group-main-employee-<%= ID %> .form-control-smart-search').click()

                $(".input-group-main-employee-code-<%= ID %>").val(code);
                $(".input-group-main-employee-value-<%= ID %>").val(value);
            },
            onKeyUp: function (searchText) {
                __doPostBack('<%= udpnRepeaterEmploy.ClientID %>', searchText);
            }
        });

            if ($(".input-group-main-employee-code-<%= ID %>").val() != "") {
            var arrCode = $(".input-group-main-employee-code-<%= ID %>").val().split(',');
                for (var i = 0; i < arrCode.length; i++) {
                    $(".input-group-main-employee-<%= ID %> .smart-search-popup-result-row[data-code='" + arrCode[i] + "']").click();
            }
            $(".input-group-main-employee-<%= ID %> .smart-search-popup").hide();
            }
        }

        rebind<%= ID %>();
</script>

