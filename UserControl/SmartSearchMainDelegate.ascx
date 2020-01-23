<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmartSearchMainDelegate.ascx.cs" Inherits="ServiceWeb.UserControl.SmartSearchMainDelegate1" %>

<link href="<%= Page.ResolveUrl("~/widget/Lib/smart-search-1.1.css") %>" rel="stylesheet" />
<script src="<%= Page.ResolveUrl("~/widget/Lib/smart-search.js?vs=20190113") %>"></script>

<style>
    .blue-require-style {
        border-left: 3px solid #9FA8DA;
    }
</style>

<div class="smart-main-delegate-template-<%= ID %>" style="display: none;">
    <div class="col-sm-4 smart-main-delegate-temp" onclick="$('.input-group-main-delegate-<%= ID %> .form-control-smart-search').click();event.stopPropagation();" style="padding: 0 5px;">
        <div class="input-group" style="margin-bottom: 0px;margin-top:0px;">
            <input type="text" readonly class="form-control" style="background: #eee" name="name" value=" " />
            <span class="input-group-append hand" onclick="$(this).closest('.smart-main-delegate-temp').remove();$('.input-group-main-delegate-code-<%= ID %>,.input-group-main-delegate-value-<%= ID %>').val('');event.stopPropagation();" style="cursor: pointer">
                <i class="fa fa-remove input-group-text"></i>
            </span>
        </div>
    </div>
</div>
<div class="input-group">
    <div class="input-group-main-delegate-<%= ID %> form-control blue-require-style" onclick="$('.input-group-main-delegate-<%= ID %> .form-control-smart-search').click();event.stopPropagation();" style="padding: 5px 0px; height: auto;">
        <div class="smart-main-delegate-result" style="margin-bottom: 0px;margin-top:0px;">
            <div class="col-sm-8 smart-main-delegate-search" style="padding: 0">
                <input style="border: none; outline: none; background: none; box-shadow: none;" class="form-control form-control-smart-search" type="text" placeholder="Search..." />
            </div>
        </div>
        <div class="smart-search-popup">
            <div class="smart-search-popup-result">
                <asp:UpdatePanel ID="udpnRepeaterMainDelegate" runat="server" UpdateMode="Conditional" OnLoad="udpnRepeaterMainDelegate_Load">
                    <ContentTemplate>
                     <table class="table" style="table-layout: fixed;">
                    <asp:Repeater runat="server" ID="rptSearchMainDelegate">
                        <ItemTemplate>
                            <tr class="smart-search-popup-result-row" data-code="<%# Eval("LinkID") %>" data-value="<%# Eval("Firstname_TH") + " " + Eval("Lastname_TH") + " ("+ Eval("Firstname") + " " + Eval("Lastname") +")" %>">
                                <td style="width: 100px;" class="text-center">
                                    <div class="img-circle-box" style="background-image:url('<%# GetImage(Eval("EmployeeCode"),Eval("UPDATED_ON")) %>');">
                                    </div>
                                </td>
                                <td style="vertical-align: middle;">
                                    <p>
                                        <b style="color: #47a3da">Link ID : </b><b class="smart-search-popup-result-code"><%# Eval("LINKID") %></b>
                                        <b class="smart-search-popup-result-code-temp"><%# Eval("LINKID") %></b>
                                    </p>
                                    <p>
                                        <b style="color: #47a3da">Fullname : </b><b class="smart-search-popup-result-value"><%# Eval("Firstname_TH") + " " + Eval("Lastname_TH") + " ("+ Eval("Firstname") + " " + Eval("Lastname") +")" %></b>
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
    <span class="input-group-append hand" onclick="$('.input-group-main-delegate-<%= ID %> .form-control-smart-search').click();event.stopPropagation();" style="cursor: pointer;">
        <i class="fa fa-search input-group-text"></i>
    </span>
</div>

<div style="display:none">
    <asp:TextBox runat="server" ID="txtResultCode" />
    <asp:TextBox runat="server" ID="txtResultValue" />
</div>


<script>
    function bindSmartSearchMainDelegate<%= ID %>() {
        $(".input-group-main-delegate-<%= ID %>").smartSearch({
            width: "100%",
            onAfterSelected: function () {
                return "";
            },
            onSelected: function (code, value) {
                $(".input-group-main-delegate-<%= ID %> .smart-main-delegate-result .smart-main-delegate-temp").remove();
                var temp = $(".smart-main-delegate-template-<%= ID %> .smart-main-delegate-temp").clone();
                temp.find("input").val(value);
                temp.attr("data-code", code);
                $(".input-group-main-delegate-<%= ID %> .smart-main-delegate-result").append(temp, $(".input-group-main-delegate-<%= ID %> .smart-main-delegate-search"));
                $('.input-group-main-delegate-<%= ID %> .form-control-smart-search').click()

                $(".input-group-main-delegate-code-<%= ID %>").val(code);
                $(".input-group-main-delegate-value-<%= ID %>").val(value);
            },
            onKeyUp: function (searchText) {
                __doPostBack('<%= udpnRepeaterMainDelegate.ClientID %>', searchText);
            }
        });

        Set<%= ID %>();
    }

    function Set<%= ID %>() {
        if ($(".input-group-main-delegate-code-<%= ID %>").val() != "") {
            var arrCode = $(".input-group-main-delegate-code-<%= ID %>").val().split(',');
            for (var i = 0; i < arrCode.length; i++) {
                $(".input-group-main-delegate-<%= ID %> .smart-search-popup-result-row[data-code='" + arrCode[i] + "']").click();
            }
            $(".input-group-main-delegate-<%= ID %> .smart-search-popup").hide();
        }
    }

    function Clear<%= ID %>() {
        $(".input-group-main-delegate-<%= ID %> .smart-main-delegate-result  .smart-main-delegate-temp .input-group-addon").click();
    }

    $(document).ready(function () {
        bindSmartSearchMainDelegate<%= ID %>();
 });
</script>