<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmartSearchOtherDelegate.ascx.cs" Inherits="ServiceWeb.widget.usercontrol.SmartSearchOtherDelegate" %>

<link href="<%= Page.ResolveUrl("~/widget/Lib/smart-search-1.1.css") %>" rel="stylesheet" />
<script src="<%= Page.ResolveUrl("~/widget/Lib/smart-search.js?vs=20190113") %>"></script>

<style>
    .blue-require-style {
        border-left: 3px solid #9FA8DA;
    }
</style>


<div class="smart-other-delegate-template-<%= ID %>" style="display: none;">
    <div class="col-sm-4 smart-other-delegate-temp" style="padding: 0 5px; padding-top: 5px;">
        <div class="input-group" style="margin-bottom: 0px;margin-top:0px;">
            <input type="text" readonly class="form-control" style="background: #eee" name="name" value=" " />
            <span class="input-group-addon input-group-append" onclick="$(this).closest('.smart-other-delegate-temp').remove();resultSelectedSmartOtherDelegate<%= ID %>();event.stopPropagation();" style="cursor: pointer">
                <i class="fa fa-remove input-group-text" style="padding-top: 9px;"></i>
            </span>
        </div>
    </div>
</div>
<div class="input-group">
    <div class="form-control input-group-other-delegate-<%= ID %> blue-require-style" style="padding: 5px 15px; padding-top: 0px; height: auto;" onclick="$('.input-group-other-delegate-<%= ID %> .form-control-smart').click();event.stopPropagation();">
        <div class="smart-other-delegate-result row" style="margin-bottom: 0px;margin-top:0px;">
            <div class="col-sm-4 input-group-other-delegate-container" style="padding: 0; padding-top: 5px;">
                <input  style="border: none; outline: none; background: none; box-shadow: none;" runat="server" class="form-control form-control-smart-search" type="text" placeholder="Search..." />
            </div>
        </div>
        <div class="smart-search-popup">
            <div class="smart-search-popup-result">
                <asp:UpdatePanel ID="udpnRepeaterOther" runat="server" UpdateMode="Conditional" OnLoad="udpnRepeaterOther_Load">
                    <ContentTemplate>

                    <table class="table" style="table-layout: fixed;">
                    <asp:HiddenField runat="server" ID="hddOtherDelegateName" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hddOtherDelegateLinkID" ClientIDMode="Static" />
                    <asp:Repeater runat="server" ID="rptSearchOtherDelegate">
                        <ItemTemplate>
                            <tr class="smart-search-popup-result-row" data-code="<%# Eval("EmployeeCode") %>" data-value="<%# Eval("Firstname_TH") + " " + Eval("Lastname_TH") + " ("+ Eval("Firstname") + " " + Eval("Lastname") +")" %>">
                                <td style="width: 100px;" class="text-center">
                                    <div class="img-circle-box" style="background-image: url('<%= Page.ResolveUrl("~") %>images/profile/128/<%# ERPW.Lib.Authentication.ERPWAuthentication.SID + "_" + ERPW.Lib.Authentication.ERPWAuthentication.CompanyCode + "_" + Eval("EmployeeCode") + ".png?vs=" + Eval("UPDATED_ON") %>') ,url('/images/user.png');">
                                    </div>
                                </td>
                                <td style="vertical-align: middle;">
                                    <p>
                                        <b style="color: #47a3da">Employee Code : </b><b class="smart-search-popup-result-code"><%# Eval("EmployeeCode") %></b>
                                        <b class="smart-search-popup-result-code-temp"><%# Eval("EmployeeCode") %></b>
                                    </p>
                                    <p>
                                        <i class="fa fa-user-plus enable-multiple-selection pull-right" style="font-size: 20px; margin-right: 10px;"></i>
                                        <b style="color: #47a3da">ชื่อ-นามสกุล : </b><b class="smart-search-popup-result-value"><%# Eval("Firstname_TH") + " " + Eval("Lastname_TH") + " ("+ Eval("Firstname") + " " + Eval("Lastname") +")" %></b>
                                        <b class="smart-search-popup-result-value-temp"><%# Eval("Firstname_TH") + " " + Eval("Lastname_TH") + " ("+ Eval("Firstname") + " " + Eval("Lastname") +")" %></b>
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
    <span class="input-group-addon input-group-append" style="cursor: pointer;" onclick="$('.input-group-other-delegate-<%= ID %> .form-control-smart-search').click();event.stopPropagation();">
        <i class="fa fa-search input-group-text" style="padding-top: 15px;"></i>
    </span>
</div>

<div style="display:none">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpData">
        <ContentTemplate>
        <asp:TextBox runat="server" ID="txtResultCode" />
        <asp:TextBox runat="server" ID="txtResultValue" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>



<script>
    function bindSmartSearchOtherDelegate<%= ID %>() {
        $(".smart-other-delegate-temp[data-code]").remove();
        $(".input-group-other-delegate-<%= ID %>").smartSearch({
            width: "100%",
            enableMutiSelect: true,
            onAfterSelected: function () {
                return "";
            },
            onSelected: function (code, value) {
                if ($(".input-group-other-delegate-<%= ID %> .smart-other-delegate-result [data-code='" + code + "']").length == 0) {
                    var temp = $(".smart-other-delegate-template-<%=ID %> .smart-other-delegate-temp").clone();
                    temp.find("input").val(value);
                    temp.attr("data-code", code);
                    temp.attr("data-value", value);
                    $(".input-group-other-delegate-<%= ID %> .smart-other-delegate-result").append(temp, $(".input-group-other-delegate-<%= ID %> .input-group-other-delegate-container"));

                    resultSelectedSmartOtherDelegate<%= ID %>();
                    $('.input-group-other-delegate-<%= ID %> .form-control-smart-search').click();
                }
            },
            onKeyUp: function (searchText) {
                __doPostBack('<%= udpnRepeaterOther.ClientID %>', searchText);
            }
        });

            Set<%= ID %>();
    }
    function Set<%= ID %>() {
        if ($(".input-group-other-delegate-code-<%= ID %>").val() != "") {
            var arrCode = $(".input-group-other-delegate-code-<%= ID %>").val().split(',');
            for (var i = 0; i < arrCode.length; i++) {
                $(".input-group-other-delegate-<%= ID %> .smart-search-popup-result-row[data-code='" + arrCode[i] + "']").click();
            }
            $(".input-group-other-delegate-<%= ID %> .smart-search-popup").hide();
        }
    }

    $(document).ready(function () {
        bindSmartSearchOtherDelegate<%= ID %>();
    });
    function resultSelectedSmartOtherDelegate<%= ID %>() {
        $(".input-group-other-delegate-code-<%= ID %>").val("");
        var arrCode = [];
        var arrValue = [];
        $(".input-group-other-delegate-<%= ID %> .smart-other-delegate-result .smart-other-delegate-temp").each(function () {
            arrCode.push($(this).attr("data-code"));
            arrValue.push($(this).attr("data-value"));
        });
        $(".input-group-other-delegate-code-<%= ID %>").val(arrCode.join(','));
        $(".input-group-other-delegate-value-<%= ID %>").val(arrValue.join(','));
    }
    function Clear<%= ID %>() {
        $(".input-group-other-delegate-<%= ID %> .smart-other-delegate-result  .smart-other-delegate-temp .input-group-addon").click();
    }
</script>