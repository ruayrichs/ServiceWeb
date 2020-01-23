<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="TierMaster.aspx.cs" Inherits="ServiceWeb.crm.Master.mastertier.TierMaster" %>

<%--<%@ Register Src="~/widget/usercontrol/SmartSearchMainDelegate.ascx" TagPrefix="uc1" TagName="SmartSearchMainDelegate" %>
<%@ Register Src="~/widget/usercontrol/SmartSearchOtherDelegate.ascx" TagPrefix="uc1" TagName="SmartSearchOtherDelegate" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <link href="<%= Page.ResolveUrl("~/widget/Lib/smart-search-1.1.css") %>" rel="stylesheet">
    <script src="<%= Page.ResolveUrl("~/widget/Lib/smart-search.js?vs=20190113") %>"></script>
    <link href="<%= Page.ResolveUrl("~/UserControl/AGapeGallery/lightGallery-master/dist/css/lightgallery.min.css") %>" rel="stylesheet" />
    <script src="<%= Page.ResolveUrl("~/UserControl/AGapeGallery/lightGallery-master/dist/js/lightgallery.min.js?vs=20190113") %>"></script>

    <style type="text/css">
        .hide {
            display: none !important;
        }
        .sidebar .sidebar-search {
            padding: 10px;
        }

        /* Fixes dropdown menus placed on the right side */
        .nav-item .dropdown-menu {
            left: auto !important;
            right: 0px;
        }
        .row {
            margin-bottom: 10px;
        }
        .row:last-child {
            margin-bottom: 0px;
        }
        
        .page-header {
            margin: 40px 0 20px;
            border-bottom: 1px solid #eee;
            font-size: 18px;
            color: #c96425;
            display: block;
            margin-bottom: 10px;
            margin-top: 3px;
            padding-bottom: 5px;
            font-weight: 600;
        }
        
        .image-box{
            background-position: center center;
            -webkit-background-size: cover;
            -moz-background-size: cover;
            background-size: cover;
            -o-background-size: cover;
            border-radius: 5px;
            border: 1px solid;
            border-color: #e5e6e9 #dfe0e4 #d0d1d5;
        }
    </style>
    
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-sla").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <script>
        function bindRequestTooltip() {
            $('[data-toggle="tooltip"]').tooltip();
        }

        function UpdateDescriptionTierCodeMaster() {
            $(".UpdateDescriptionTierCode").click(function (e) {
                $(".panel-update-tiercode-click").show();
                $(".panel-update-tiercode-click").offset({ left: e.pageX, top: e.pageY });

                var tiercode = $(this).closest('td').attr('data-TierCode');
                $("#txtModalTierMasterTierCode").val(tiercode);

                var tiername = $(this).closest('td').attr('data-Tiername');
                $("#txtDescritptionUpdateTierMaster").val(tiername);
            });
        }
        function swithitemTierGroupListToggle(obj) {
            var mode = $(obj).attr('data-TierGroupCode');
            $(obj).toggleClass('active');
            if ($(obj).hasClass('active')) {
                $(".item-group-" + mode).fadeOut();
            }
            else {
                $(".item-group-" + mode).fadeIn();
            }
        }
        function swithitemTierGroupListNodeToggle(obj) {
            var mode = $(obj).attr('data-TierGroupCode');
            $(".item-group-node-" + mode).fadeToggle();
        }

        function closemodalInitiativeAfterRebindData() {
            $('#btnRebindTierGroupMasterPage').click();
        }

    </script>
    <style>
        .th-header-tier {
            vertical-align: middle !important;
        }

        .tr-hight-header {
            height: 55px;
        }

        .pointer {
            cursor: pointer;
        }

        .td-contant-detail {
            color: #0044cc;
        }

        .config-size {
            font-size: 20px;
        }

        .item-color, .item-bg-color {
            color: rgba(0, 0, 0, 0.77);
        }
        /*.item-bg-color {
            background-color:rgba(238, 238, 238, 0.41);
        }*/
        .row-add-tier, table > tbody > tr.row-add-tier > th, table > tbody > tr.row-add-tier > td {
            padding-top: 0px !important;
            padding-bottom: 0px !important;
            color: #00a90a;
        }

        .border-item, table > tbody > tr.border-item > th, table > tbody > tr.border-item > td {
            border: none !important;
            padding-top: 1px !important;
            padding-bottom: 1px !important;
        }

        .title-item {
            color: #ccc !important;
            font-size: 14px;
            font-weight: 500;
        }

        .image-box-card {
            background-position: center center;
            -webkit-background-size: cover;
            -moz-background-size: cover;
            background-size: cover;
            -o-background-size: cover;
            background-repeat: no-repeat;
            width: 35px;
            height: 35px;
            border-radius: 50%;
        }

        .panel-update-tiercode-click {
            position: absolute;
            padding: 20px;
            /*padding-top: 10px;*/
            border-radius: 5px;
            border: 1px solid #ccc;
            background: #fff;
            z-index: 1;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.29);
            width: 300px;
        }
    </style>
    <div class="panel-update-tiercode-click" style="display: none;">
        <p style="border-bottom: 1px solid #ccc">Update Tier Description</p>
        <div class="row">
            <div class="col-lg-12">
                <input id="txtDescritptionUpdateTierMaster" type="text" class="form-control" />
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12 text-center">
                <span class="btn btn-primary" onclick="UpdateTierMasterDescription(this);">Save</span>
                <span class="btn btn-danger" onclick="$('.panel-update-tiercode-click').hide();">Cancel</span>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <h6 class="text-info">Tier Master</h6>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading PANEL-DEFAULT-BUTTON">
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-lg-4">
                    <asp:TextBox ID="txtSearchTierGroupMaster" placeholder="Tier group..." CssClass="form-control" runat="server" />
                </div>
                <div class="col-xs-12 col-sm-12 col-lg-4">
                    <span class="btn btn-info DEFAULT-BUTTON-CLICK" onclick="$('#btnRebindTierGroupMasterPage').click();"><i class="fa fa-search"></i>&nbsp; Search</span>
                    <span class="btn btn-success AUTH_MODIFY" onclick="showInitiativeModal('ModalCreateTierGroup');"><i class="fa fa-pencil"></i>&nbsp;Create Group</span>

                </div>
            </div>
        </div>
        <br />
        <div class="panel-body">
            <div class="row">
                <div class="col-sx-12 cl-sm-12 col-md-12 col-lg-12">
                    <div class="row hide">
                        <div class="col-lg-12">
                            <asp:UpdatePanel ID="udpButtonDataReBind" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnRebindTierGroupMasterPage" ClientIDMode="Static" OnClick="btnRebindTierGroupMasterPage_Click" OnClientClick="AGLoading(true);" Text="rebindGroup" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:UpdatePanel ID="udpDataTier" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <table class="table table-hover">
                                        <asp:Repeater ID="rptDataTierGroup" OnItemDataBound="rptDataTierGroup_ItemDataBound" runat="server">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hddTierGroupCode" Value='<%# Eval("TierGroupCode") %>' runat="server" />
                                                <tr class="info tr-hight-header">
                                                    <th class="th-header-tier" style="width: 30px;">
                                                        <i class="fa fa-list pointer" data-tiergroupcode="<%# Eval("TierGroupCode") %>" onclick="swithitemTierGroupListToggle(this)" aria-hidden="true"></i>

                                                    </th>
                                                    <th style="width: 40%;" class="th-header-tier">
                                                        <label data-toggle="tooltip" data-placement="top" title=" Group ">
                                                            <%# Eval("TierGroupDescription") %>
                                                        </label>
                                                    </th>
                                                    <th class="th-header-tier">
                                                        <label class="hide" data-toggle="tooltip" data-placement="top" title=" Create Group "><%# Eval("name")+" "+ Eval("LastName_TH") %></label>
                                                    </th>
                                                    <th class="th-header-tier">
                                                        <label class="hide"><%# Eval("Create_ON_Des") %> </label>
                                                    </th>
                                                    <th class="th-header-tier">
                                                        <%--<label></label>--%>
                                                    </th>
                                                </tr>

                                                <tr class="warning row-add-tier item-group-<%# Eval("TierGroupCode") %>">
                                                    <th></th>
                                                    <td colspan="4" style="padding-left: 20px;">
                                                        <i class="fa fa-plus-circle pointer hide AUTH_MODIFY"
                                                            data-tiergroupcodedes="<%# Eval("TierGroupDescription") %>"
                                                            data-tiergroupcode="<%# Eval("TierGroupCode") %>"
                                                            onclick="showInitiativeModalCreateTierMaster(this);" aria-hidden="true">&nbsp; Add Priority <%# Eval("TierGroupDescription") %>
                                                        </i>
                                                    </td>
                                                </tr>
                                                <asp:Repeater ID="rptDataTierMaster" OnItemDataBound="rptDataTierMaster_ItemDataBound" runat="server">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hddTierCode" Value='<%# Eval("TierCode") %>' runat="server" />
                                                        <tr class="warning item-group-<%# Eval("TierGroupCode") %> ">
                                                            <td></td>
                                                            <td class="td-contant-detail" style="padding-left: 20px;"
                                                                data-tiercode="<%# Eval("TierCode") %>"
                                                                data-tiername="<%# Eval("TierName") %>"
                                                                data-tiergroupcode="<%# Eval("TierGroupCode") %>">
                                                                <i class="fa fa-list pointer" data-tiergroupcode="<%# Eval("TierGroupCode")+""+Eval("TierCode") %>" onclick="swithitemTierGroupListNodeToggle(this)" aria-hidden="true"></i>
                                                                &nbsp;
                                                                <%--<i class="fa fa-pencil pointer item-color UpdateDescriptionTierCode AUTH_MODIFY"></i>
                                                                &nbsp;--%>
                                                                <%--<i class="fa fa-trash-o pointer <%= this.AUTH_DELETE %>" onclick="DeleteTierCodeMasterForSelectInGroup(this);" style="color: red;" aria-hidden="true"></i>
                                                                &nbsp;--%>
                                                                <span class="pointer" data-toggle="tooltip" data-placement="top" title=" Tier ">
                                                                    <%# Eval("TierName") %>
                                                                </span>
                                                            </td>
                                                            <td class="td-contant-detail">
                                                                <span data-toggle="tooltip" data-placement="top" title=" Create Tier " class="hide">
                                                                    <%# Eval("name")+" "+ Eval("LastName_TH") %>
                                                                </span>
                                                            </td>
                                                            <td class="td-contant-detail">
                                                                <span class="hide">
                                                                    <%# Eval("Create_ON_Des") %> 
                                                                </span>
                                                            </td>
                                                            <td class="td-contant-detail"><%# ConvertToTotalTime(Eval("TierGroupCode").ToString(), Eval("TierCode").ToString()) %> </td>
                                                        </tr>

                                                        <tr class="row-add-tier border-item item-group-<%# Eval("TierGroupCode") %> item-group-node-<%# Eval("TierGroupCode")+""+Eval("TierCode") %>"
                                                            data-tiergroupcode="<%# Eval("TierGroupCode") %>"
                                                            data-tiergroupcode-name="<%# Eval("TierGroupDescription") %>"
                                                            data-tiercode="<%# Eval("TierCode") %>"
                                                            data-tiercode-name="<%# Eval("TierName") %>">
                                                            <th class="item-color"></th>
                                                            <td class="item-color" style="max-height: 20px; padding-left: 50px;">
                                                                <i class="fa fa-plus-circle pointer AUTH_MODIFY"
                                                                    onclick="openModalForAddItemTierMasterForTier(this);" aria-hidden="true">&nbsp; Add Tier <%# Eval("TierName") %>
                                                                </i>
                                                            </td>
                                                            <td class="title-item">Main
                                                            </td>
                                                            <td class="title-item">Participant
                                                            </td>
                                                            <td class="title-item">Resolution Time
                                                            </td>
                                                        </tr>
                                                        <div class="sortable" style="list-style-type: none; padding: 0px;">
                                                            <asp:Repeater ID="rptDataTierMasterItem" runat="server">
                                                                <ItemTemplate>
                                                                    <tr class="item-bg-color border-item item-group-<%# Eval("TierGroupCode") %> item-group-node-<%# Eval("TierGroupCode")+""+Eval("TierCode") %>">
                                                                        <td></td>
                                                                        <td class="item-color" style="padding-left: 50px;"
                                                                            data-tiergroupcode="<%# Eval("TierGroupCode") %>"
                                                                            data-tiergroupdescription="<%# Eval("TierGroupDescription") %>"
                                                                            data-tiercode="<%# Eval("TierCode") %>"
                                                                            data-tiername="<%# Eval("TierName") %>"
                                                                            data-tieritem="<%# Eval("Tier") %>"
                                                                            data-tierdescription="<%# Eval("TierDescription") %>"
                                                                            data-role="<%# Eval("Role") %>"
                                                                            data-sequence="<%# Eval("sequence") %>"
                                                                            data-resolution="<%# Eval("Resolution") %>"
                                                                            data-requester="<%# Eval("Requester") %>"
                                                                            data-headshift="<%# Eval("HeadShift") %>"
                                                                            data-avpsale="<%# Eval("AVPSale") %>"
                                                                            data-svpsale="<%# Eval("SVPSale") %>"
                                                                            data-DynamicOwner="<%# Eval("DynamicOwner") %>" >
                                                                            <i onclick="openmodalDisplayTierItemForUpdate(this);" class="fa fa-pencil pointer item-color AUTH_MODIFY"></i>
                                                                            &nbsp;
                                                                        <i class="fa fa-trash-o pointer <%= this.AUTH_DELETE %>" onclick="deleteTierItemInTierCode(this)" style="color: red;" aria-hidden="true"></i>
                                                                            &nbsp;<%# "Level "+ (Container.ItemIndex+1) +" "+ Eval("TierDescription") %>
                                                                        </td>
                                                                        <td class="item-color list-emp-main">
                                                                            <%# getImagehtmlFormEmployee_V2(Convert.ToString(Eval("TierCode")), Convert.ToString(Eval("Tier")), "Main") %>
                                                                        </td>
                                                                        <td class="item-color list-emp-participant">
                                                                            <%# getImagehtmlFormEmployee_V2(Convert.ToString(Eval("TierCode")), Convert.ToString(Eval("Tier")), "Participant") %>
                                                                        </td>
                                                                        <td class="item-color">
                                                                            <%# ConvertToTime(Eval("Resolution").ToString()) %>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function minimalListEmployee() {
            $(".list-emp-participant,.list-emp-main").each(function () {
                var itemEmp = $(this).find("div");
                if (itemEmp.length > 3) {
                    for (var i = 3; i < itemEmp.length; i++) {
                        $(itemEmp[i]).addClass("item-emp-hide")
                    }
                    var panelMore = $("<div>", {
                        class: "panel-load-more",
                        html: "แสดงเพิ่มเติม..."
                    }).bind("click", function () {
                        $(this).parent().find(".item-emp-hide").removeClass("item-emp-hide");
                        $(this).remove();
                    });
                    $(this).append(panelMore);
                }
            });
        }
    </script>
    <!-- modal create Tier Master -->
    <style>
        .item-emp-hide {display: none}
        .panel-load-more {
            border: 1px dashed #bbb;
            margin-top: 10px;
            margin-bottom: 10px;
            text-align: center;
            cursor: pointer;
        }
        .panel-load-more:hover {
            color: #777;
        }

        .assignment-filter-checker {
            cursor: pointer;
        }

            .assignment-filter-checker .fa-square-o {
                display: initial;
            }

            .assignment-filter-checker .fa-check-square-o {
                display: none;
            }

            .assignment-filter-checker.active .fa-square-o {
                display: none;
            }

            .assignment-filter-checker.active .fa-check-square-o {
                display: initial;
            }

        .main, .Participant {
            width: 10%;
        }

        .no-chack {
            color: rgba(107, 106, 106, 0.8);
        }

        .check-select {
            color: #05be47;
        }
    </style>
    <script>

        function showInitiativeModalCreateTierMaster(obj) {
            clearItem();
            var tiergroupcode = $(obj).attr("data-tiergroupcode");
            $("#txtModalTierMasterTierGroup").val(tiergroupcode);

            var tiergroupcodeDes = $(obj).attr("data-tiergroupcodeDes");
            $("#txtModalTierMasterTierGroupDescript").val(tiergroupcodeDes);
            $(".TierItem").hide();

            showInitiativeModal('ModalCreateTierMaster');
            $("#btnModalAddTierMaster").show();
            $("#btnModalAddTierItem").hide();
            $("#btnModalUpdateTierItem").hide();
        }


        function IsValideForSaveTierMasterModalCreateTier(obj) {
            if ($("#txtModalTierMasterTierCodeDescript").val().split(' ').join() == "") {
                $("#txtModalTierMasterTierCodeDescript").focus();
                AGMessage("กรุณากรอก Priority Description");
                return;
            }
            //if ($("#txtModalTierMasterTierItemDescript").val().split(' ').join() == "") {
            //    $("#txtModalTierMasterTierItemDescript").focus();
            //    AGMessage("กรุณากรอก Tier Item Name");
            //    return;
            //}
            //if ($("[id*=ddlModalTierMasterRoleSelect]").val() == "") {
            //    AGMessage("กรุณากรอก Tier Description");
            //    return;
            //}

            //var checkMain = false;
            //checkMain = checkForSetRoleResourceUpdateTierMaster(obj);
            //if (!checkMain) {
            //    return false;
            //}

            if (AGConfirm(obj, "ยืนยันการบันทึก")) {
                $("#btnModalCreateTierMasterForSaveTierMaster").click();
                closeInitiativeModal('ModalCreateTierMaster');
                //closemodalInitiativeAfterRebindData();
            }
        }

        function openModalForAddItemTierMasterForTier(obj) {
            clearItem();
            var tr = $(obj).closest('tr');
            $("#txtModalTierMasterTierGroup").val($(tr).attr('data-tiergroupcode'));
            $("#txtModalTierMasterTierGroupDescript").val($(tr).attr('data-tiergroupcode-name'));

            $("#txtModalTierMasterTierCode").val($(tr).attr('data-tiercode'));
            $("#txtModalTierMasterTierCodeDescript").val($(tr).attr('data-tiercode-name'));
            $("#txtModalTierMasterTierCodeDescript").prop("disabled", true);
            $(".TierItem").show();

            setSelectDynamicRole(false);
            $(".ddlParticipants-role").val('');
            $(".chk-SelectOwner").prop("checked", false);
            closemodalInitiativeAfterRebindData();

            showInitiativeModal('ModalCreateTierMaster');
            $("#btnModalAddTierMaster").hide();
            $("#btnModalUpdateTierItem").hide();
            $("#btnModalAddTierItem").show();
        }

        function IsValidForSaveTierMasterModalAddItem(obj) {
            $("#txtModalTierMasterTierCodeDescript").prop("disabled", false);
            if ($("#txtModalTierMasterTierItemDescript").val().split(' ').join() == "") {
                $("#txtModalTierMasterTierItemDescript").focus();
                AGMessage("กรุณากรอก Tier Item Name");
                return;
            }
            if ($("[id*=ddlModalTierMasterRoleSelect]").val() == "") {
                AGMessage("กรุณาเลือก Role");
                return;
            }
            if ($("#txtResolutionTime").val() == "") {
                $("#txtResolutionTime").focus();
                AGMessage("กรุณากรอก Resolution Time");
                return;
            }
            if ($("#txtRequester").val() == "") {
                $("#txtRequester").focus();
                AGMessage("กรุณากรอก Requester");
                return;
            }
            if ($("#txtHeadShift").val() == "") {
                $("#txtHeadShift").focus();
                AGMessage("กรุณากรอก Head of shift");
                return;
            }
            if ($("#txtAVPSale").val() == "") {
                $("#txtAVPSale").focus();
                AGMessage("กรุณากรอก AVP Sale");
                return;
            }
            if ($("#txtSVPSale").val() == "") {
                $("#txtSVPSale").focus();
                AGMessage("กรุณากรอก SVP Sale");
                return;
            }

            var checkMain = false;
            checkMain = checkForSetRoleResourceUpdateTierMaster(obj);
            if (!checkMain) {
                return false;
            }
            if (AGConfirm(obj, "ยืนยันการบันทึก")) {
                $("#btnModalCreateTierMasterForSaveTierItem").click();
                //closeInitiativeModal('ModalCreateTierMaster');
            }
        }

        function setdataInTextBoxForResourceUpdate(obj) {
            var TierGroupCode = $(obj).parent().attr('data-TierGroupCode');
            var TierGroupDescription = $(obj).parent().attr('data-TierGroupDescription');
            var TierCode = $(obj).parent().attr('data-TierCode');
            var TierName = $(obj).parent().attr('data-TierName');
            var TierItem = $(obj).parent().attr('data-TierItem');
            var TierItemDescription = $(obj).parent().attr('data-TierDescription');
            var Role = $(obj).parent().attr('data-Role');
            var sequence = $(obj).parent().attr('data-sequence');
            var Resolution = $(obj).parent().attr('data-Resolution');
            var Requester = $(obj).parent().attr('data-Requester');
            var HeadShift = $(obj).parent().attr('data-HeadShift');
            var AVPSale = $(obj).parent().attr('data-AVPSale');
            var SVPSale = $(obj).parent().attr('data-SVPSale');
            var DynamicOwner = $(obj).parent().attr('data-DynamicOwner') == "TRUE";

            $("#txtModalTierMasterTierGroup").val(TierGroupCode);
            $("#txtModalTierMasterTierGroupDescript").val(TierGroupDescription);

            $("#txtModalTierMasterTierCode").val(TierCode);
            $("#txtModalTierMasterTierCodeDescript").val(TierName);

            $("#txtModalTierMasterTierItemCode").val(TierItem);
            $("#txtModalTierMasterTierItemDescript").val(TierItemDescription);
            //console.log("Old", Resolution);
            Resolution = parseInt(Resolution) / 60;
            //console.log("New", Resolution);
            $("#txtResolutionTime").val(Resolution);
            Requester = parseInt(Requester) / 60;
            $("#txtRequester").val(Requester);
            HeadShift = parseInt(HeadShift) / 60;
            $("#txtHeadShift").val(HeadShift);
            AVPSale = parseInt(AVPSale) / 60;
            $("#txtAVPSale").val(AVPSale);
            SVPSale = parseInt(SVPSale) / 60;
            $("#txtSVPSale").val(SVPSale);
            $("#ddlParticipantsDescription").val(Role);
            setSelectDynamicRole(DynamicOwner);
            //$("#chkDynamicRole").prop("checked", DynamicOwner);

            $("#txtTierResourceJsonDataForSave").val(Role);//set Role
            $("#txtHideTierItemSequence").val(sequence);
        }

        function openmodalDisplayTierItemForUpdate(obj) {
            clearItem();
            setdataInTextBoxForResourceUpdate(obj);
            $("#btnBindDataTierMasterForUpdateTierItem").click();
        }

        function deleteTierItemInTierCode(obj) {
            if (AGConfirm(obj, "ยืนยันลบรายการ!")) {
                clearItem();
                setdataInTextBoxForResourceUpdate(obj);
                $("#btnDeleteTierItemInTierMaster").click();
            }
        }

        function swithModeButtonForUpdateTierItem() {
            $("#btnModalAddTierMaster").hide();
            $("#btnModalAddTierItem").hide();
            $("#btnModalUpdateTierItem").show();
        }

        function IsValidForSaveTierMasterModalUpdateItem(obj) {
            if ($("#txtModalTierMasterTierItemDescript").val().split(' ').join() == "") {
                $("#txtModalTierMasterTierItemDescript").focus();
                AGMessage("กรุณากรอก Tier Item Name");
                return;
            }
            if ($("[id*=ddlModalTierMasterRoleSelect]").val() == "") {
                AGMessage("กรุณาเลือก Role");
                return;
            }
            if ($("#txtResolutionTime").val() == "") {
                $("#txtResolutionTime").focus();
                AGMessage("กรุณากรอก Resolution Time");
                return;
            }
            if ($("#txtRequester").val() == "") {
                $("#txtRequester").focus();
                AGMessage("กรุณากรอก Requester");
                return;
            }
            if ($("#txtHeadShift").val() == "") {
                $("#txtHeadShift").focus();
                AGMessage("กรุณากรอก Head of shift");
                return;
            }
            if ($("#txtAVPSale").val() == "") {
                $("#txtAVPSale").focus();
                AGMessage("กรุณากรอก AVP Sale");
                return;
            }
            if ($("#txtSVPSale").val() == "") {
                $("#txtSVPSale").focus();
                AGMessage("กรุณากรอก SVP Sale");
                return;
            }

            var IsUpdate = false;
            IsUpdate = checkForSetRoleResourceUpdateTierMaster(obj);
            if (!IsUpdate) {
                return false;
            }

            if (AGConfirm(obj, "ยืนยันการบันทึก")) {
                $("#btnModalCreateTierMasterForUpdateTierItem").click();
                closeInitiativeModal('ModalCreateTierMaster');
            }
        }

        function checkForSetRoleResourceUpdateTierMaster(obj) {
            return true;
            var jArr = [];
            var checkMain = false;
            var data = $(obj).closest('.initiative-model-control-body-content').find("#table-role-select").find('.active');
            for (var i = 0; i < data.length; i++) {
                var type = $(data[i]).attr('data-mode');
                if (type == "main") {
                    checkMain = true;
                }
                var jObj = {};
                jObj.emplayeeCode = $(data[i]).closest('tr').attr('data-people-code');
                jObj.type = type;
                jArr.push(jObj);
            }

            if (!checkMain) {
                AGMessage("กรุณาเลือก Default Main!");
                return false;
            }
            $("#txtTierResourceJsonDataForSave").val(JSON.stringify(jArr));
            return true;
        }

        function SwapCheckerSelectRole(obj, mode) {
            if (mode == "main") {
                var row = $(obj).closest('tr');
                $(row).find(".Participant").find(".mode-view").removeClass('active');

                $(obj).closest("table").find(".main-mode-view-tier").removeClass("active");
            }
            else {
                var row = $(obj).closest('tr');
                $(row).find(".main").find(".mode-view").removeClass('active');

            }
            $(obj).toggleClass("active");
        }

        function clearItem() {
            $("#txtModalTierMasterTierCode").val("");
            $("#txtModalTierMasterTierCodeDescript").val("");

            $("#txtModalTierMasterTierItemCode").val("");
            $("#txtModalTierMasterTierItemDescript").val("");

            $("#txtResolutionTime").val("0");
            $("#txtRequester").val("0");
            $("#txtHeadShift").val("0");
            $("#txtAVPSale").val("0");
            $("#txtSVPSale").val("0");
        }

        function UpdateTierMasterDescription(obj) {
            if (AGConfirm(obj, "ยืนยันการบันทึก")) {
                var descript = $(obj).closest('.panel-update-tiercode-click').find('#txtDescritptionUpdateTierMaster').val();
                $("#txtModalTierMasterTierCodeDescript").val(descript);
                $("#btnbtnUpdateTierMasterInTierGroup").click();
                $(".panel-update-tiercode-click").hide();
            }
        }

        function DeleteTierCodeMasterForSelectInGroup(obj) {
            var tiername = $(obj).closest('td').attr('data-Tiername');
            if (AGConfirm(obj, "ยืนยันการลบ Tier " + tiername)) {
                var tiercode = $(obj).closest('td').attr('data-TierCode');
                var tiergroupcode = $(obj).closest('td').attr('data-TierGroupCode');
                $("#txtModalTierMasterTierCode").val(tiercode);
                $("#txtModalTierMasterTierCodeDescript").val(tiername);
                $("#txtModalTierMasterTierGroup").val(tiergroupcode);
                $("#btnDeleteTierMasterInTierGroup").click();
            }
        }
    </script>
    <div class="initiative-model-control-slide-panel" id="ModalCreateTierMaster">
        <div class="initiative-model-control-body-content z-depth-3">
            <div>
                <div class="initiative-model-control-header">
                    <div class="mat-box-initaive-control">
                        <div class="pull-right">
                            <i class="fa fa-close hand" onclick="closeInitiativeModal('ModalCreateTierMaster');closemodalInitiativeAfterRebindData();"></i>
                        </div>
                        <div class="one-line">
                            <label class="text-warning">Create Group</label>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-control-contant">
                    <div class="panel-body-initiative-master">
                        <div class="panel-content-initiative-master">
                            <div class="mat-box-initaive-control tab-initiative-control">
                                <div class="TierCodeMasterControl">
                                    <asp:UpdatePanel ID="udpEventModalCreateTierMaster" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <label>Tier Group</label>
                                                    <asp:TextBox ID="txtModalTierMasterTierGroup" Style="display: none;" Enabled="false" ClientIDMode="Static" CssClass="form-control" runat="server" />
                                                    <asp:TextBox ID="txtModalTierMasterTierGroupDescript" Enabled="false" ClientIDMode="Static" CssClass="form-control" runat="server" />
                                                </div>
                                                <div class="col-lg-6">
                                                    <label>Priority Description</label>
                                                    <asp:TextBox ID="txtModalTierMasterTierCode" Style="display: none;" Enabled="false" ClientIDMode="Static" CssClass="form-control" runat="server" />
                                                    <asp:TextBox ID="txtModalTierMasterTierCodeDescript" placeholder="Text" ClientIDMode="Static" CssClass="form-control" runat="server" />
                                                </div>
                                            </div>
                                            <hr class="TierItem" />
                                            <div class="row TierItem">
                                                <div class="col-lg-6">
                                                    <label>Tier Item Name</label>
                                                    <asp:TextBox ID="txtModalTierMasterTierItemCode" Style="display: none;" Enabled="false" ClientIDMode="Static" CssClass="form-control" runat="server" />
                                                    <asp:TextBox ID="txtModalTierMasterTierItemDescript" placeholder="Text" ClientIDMode="Static" CssClass="form-control" runat="server" />
                                                </div>
                                                <div class="col-lg-6">
                                                    <label>Resolution Time (Minutes) &nbsp;<span class="errResolution" style="color: red"></span></label>
                                                    <asp:TextBox ID="txtResolutionTime" placeholder="Number" ClientIDMode="Static" Style="text-align: right;" CssClass="form-control" runat="server"
                                                        onkeypress="return isNumberic(event,'errResolution');" />
                                                </div>
                                            </div>
                                            <hr class="TierItem" />
                                            <div class="initiative-model-control-header">
                                                <div class="mat-box TierItem" style="background-color: #ffffe6">
                                                    <div class="one-line">
                                                        <label class="text-warning">Management Escalation</label>
                                                    </div>
                                                    <div class="row TierItem">
                                                        <div class="col-lg-6">
                                                            <label>Requester (Minutes) &nbsp;<span class="errRequester" style="color: red"></span></label>
                                                            <asp:TextBox ID="txtRequester" placeholder="Number" ClientIDMode="Static" Style="text-align: right;" CssClass="form-control" runat="server"
                                                                onkeypress="return isNumberic(event,'errRequester');" />
                                                        </div>
                                                        <div class="col-lg-6">
                                                            <label>Head of shift (Minutes) &nbsp;<span class="errHeadShift" style="color: red"></span></label>
                                                            <asp:TextBox ID="txtHeadShift" placeholder="Number" ClientIDMode="Static" Style="text-align: right;" CssClass="form-control" runat="server"
                                                                onkeypress="return isNumberic(event,'errHeadShift');" />
                                                        </div>
                                                        <div class="col-lg-6">
                                                            <label>AVP/VP Sale (Minutes) &nbsp;<span class="errAVPSale" style="color: red"></span></label>
                                                            <asp:TextBox ID="txtAVPSale" placeholder="Number" ClientIDMode="Static" Style="text-align: right;" CssClass="form-control" runat="server"
                                                                onkeypress="return isNumberic(event,'errAVPSale');" />
                                                        </div>
                                                        <div class="col-lg-6">
                                                            <label>SVP Sale (Minutes) &nbsp;<span class="errSVPSale" style="color: red"></span></label>
                                                            <asp:TextBox ID="txtSVPSale" placeholder="Number" ClientIDMode="Static" Style="text-align: right;" CssClass="form-control" runat="server"
                                                                onkeypress="return isNumberic(event,'errSVPSale');" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <hr class="TierItem" />
                                            <div class="row TierItem hide">
                                                <div class="col-xs-12 col-sm-12 col-lg-6">
                                                    <label>Select Role</label>
                                                    <%--<asp:DropDownList ID="ddlModalTierMasterRoleSelect" onchange="$('#btnModalBindPepleForRoleSelect').click();" CssClass="form-control" runat="server">
                                                    </asp:DropDownList>--%>
                                                </div>
                                                <div class="row hide">
                                                    <asp:TextBox ID="txtTierResourceJsonDataForSave" ClientIDMode="Static" runat="server" />
                                                    <%--<asp:Button ID="btnModalBindPepleForRoleSelect" ClientIDMode="Static" OnClick="btnModalBindPepleForRoleSelect_Click" OnClientClick="AGLoading(true);" Text="btnBindPepleForRoleSelect" runat="server" />--%>
                                                    <asp:Button ID="btnModalCreateTierMasterForSaveTierMaster" ClientIDMode="Static" OnClick="btnModalCreateTierMasterForSaveTierMaster_Click" OnClientClick="AGLoading(true);" Text="Save" runat="server" />
                                                    <asp:Button ID="btnModalCreateTierMasterForSaveTierItem" ClientIDMode="Static" OnClick="btnModalCreateTierMasterForSaveTierItem_Click" OnClientClick="AGLoading(true);" Text="Save" runat="server" />

                                                    <!-- bind data Role For Update Tier Item -->
                                                    <asp:TextBox ID="txtHideTierItemSequence" ClientIDMode="Static" runat="server" />
                                                    <asp:Button ID="btnBindDataTierMasterForUpdateTierItem" ClientIDMode="Static" OnClick="btnBindDataTierMasterForUpdateTierItem_Click" OnClientClick="AGLoading(true);" Text="Save" runat="server" />
                                                    <asp:Button ID="btnModalCreateTierMasterForUpdateTierItem" ClientIDMode="Static" OnClick="btnModalCreateTierMasterForUpdateTierItem_Click" OnClientClick="AGLoading(true);" Text="Save" runat="server" />

                                                    <!-- Delete Tieritem In TierMaster -->
                                                    <asp:Button ID="btnDeleteTierItemInTierMaster" OnClick="btnDeleteTierItemInTierMaster_Click" ClientIDMode="Static" OnClientClick="AGLoading(true);" Text="btnDeleteTierItemInTierMaster" runat="server" />

                                                    <!-- Update Tier Master -->
                                                    <asp:Button ID="btnbtnUpdateTierMasterInTierGroup" OnClick="btnbtnUpdateTierMasterInTierGroup_Click" OnClientClick="AGLoading(true);" ClientIDMode="Static" Text="btnUpdateTierMasterInTierGroup" runat="server" />
                                                    <asp:Button ID="btnDeleteTierMasterInTierGroup" OnClick="btnDeleteTierMasterInTierGroup_Click" OnClientClick="AGLoading(true);" ClientIDMode="Static" Text="btnDeleteTierMasterInTierGroup" runat="server" />
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div class="mat-box-initaive-control" style="margin-top: 30px;padding-top: 30px;">
                                        <div>
                                            <style>
                                                .switch {
                                                    position: relative;
                                                    display: inline-block;
                                                    width: 60px;
                                                    height: 34px;
                                                    zoom: 0.7;
                                                    margin-bottom: 0;
                                                }

                                                    .switch input {
                                                        display: none;
                                                    }

                                                .slider {
                                                    position: absolute;
                                                    cursor: pointer;
                                                    top: 0;
                                                    left: 0;
                                                    right: 0;
                                                    bottom: 0;
                                                    background-color: #ccc;
                                                    -webkit-transition: .4s;
                                                    transition: .4s;
                                                }

                                                    .slider:before {
                                                        position: absolute;
                                                        content: "";
                                                        height: 26px;
                                                        width: 26px;
                                                        left: 4px;
                                                        bottom: 4px;
                                                        background-color: white;
                                                        -webkit-transition: .4s;
                                                        transition: .4s;
                                                    }

                                                input:checked + .slider {
                                                    background-color: #2196F3;
                                                }

                                                input:focus + .slider {
                                                    box-shadow: 0 0 1px #2196F3;
                                                }

                                                input:checked + .slider:before {
                                                    -webkit-transform: translateX(26px);
                                                    -ms-transform: translateX(26px);
                                                    transform: translateX(26px);
                                                }

                                                /* Rounded sliders */
                                                .slider.round {
                                                    border-radius: 34px;
                                                }

                                                    .slider.round:before {
                                                        border-radius: 50%;
                                                    }
                                                .box-mode {
                                                    display: flex;
                                                    border: 1px solid #ccc;
                                                    padding: 6px 8px;
                                                    border-radius: 4px;
                                                    float: left;
                                                    margin-top: -50px;
                                                    background-color: #fff;
                                                    position: absolute;
                                                }

                                                .box {
                                                    border: 1px solid #ccc;
                                                    padding: 2px 4px;
                                                    border-radius: 4px;
                                                    margin-bottom: 3px;
                                                    transition: all 0.5s;
                                                    -webkit-transition: all 0.5s;
                                                    -moz-transition: all 0.5s;
                                                }

                                                    .box:hover {
                                                        background: #c8e4f8;
                                                        color: #000;
                                                        border: 1px solid #94a0b4;
                                                    }

                                                    .box.active {
                                                        border-color: #2196F3;
                                                        background-color: #cbe8ff;
                                                    }


                                            </style>
                                            
                                            <div class="pull-right box-mode " >
                                                <label class="switch">
                                                    <input type="checkbox" id="chkDynamicRole_Display" onchange="selectDynamicRole();">
                                                    <span class="slider round"></span>
                                                </label>
                                                <b>&nbsp;&nbsp;Dynamic Role</b>
                                                <asp:CheckBox Text="" runat="server" ID="chkDynamicRole" ClientIDMode="Static" CssClass="hide" />
                                            </div>

                                            <script>
                                                function selectDynamicRole() {
                                                    if ($("#chkDynamicRole_Display").prop("checked")) {
                                                        $("#chkDynamicRole").prop("checked", true);
                                                        $(".panel-role-default").hide();
                                                        $(".panel-role-dynamic").show();
                                                    } else {
                                                        $("#chkDynamicRole").prop("checked", false);
                                                        $(".panel-role-default").show();
                                                        $(".panel-role-dynamic").hide();
                                                    }
                                                }
                                                function setSelectDynamicRole(IsCheck) {
                                                    if (IsCheck) {
                                                        $("#chkDynamicRole_Display").prop("checked", true);
                                                        $("#chkDynamicRole").prop("checked", true);
                                                        $(".panel-role-default").hide();
                                                        $(".panel-role-dynamic").show();
                                                    } else {
                                                        $("#chkDynamicRole_Display").prop("checked", false);
                                                        $("#chkDynamicRole").prop("checked", false);
                                                        $(".panel-role-default").show();
                                                        $(".panel-role-dynamic").hide();
                                                    }
                                                }
                                            </script>
                                        </div>
                                        <div class="panel-role-default">
                                            <div class="row">
                                                <div class="col-sm-12 col-md-12">
                                                    <div style="margin-bottom: 10px;">
                                                        <asp:updatePanel runat="server" UpdateMode="Conditional" ID="updEventObject">
                                                            <ContentTemplate>
                                                                <label>
                                                                    Role
                                                                </label>
                                                                <asp:DropDownList ID="ddlParticipantsDescription" runat="server" ClientIDMode="Static" CssClass="form-control ddlParticipants-role"
                                                                    OnSelectedIndexChanged="btnChangeSubProjectObject_Click" onchange="AGLoading(true);" AutoPostBack="true"></asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:updatePanel>
                                                    </div>
                                                    <%--<div style="margin-bottom: 10px;">
                                                        <label>
                                                            Character Group
                                                        </label>
                                                        <div class="input-group" style="width: 100%">
                                                            <input type="text" readonly name="name" style="background: #fff;" onclick="openSubProjectHierarchyObject(event); bindHierarchyCharacterObject(this);" id="txtSubProjectDescriptionObject" runat="server" clientidmode="Static" class="form-control blue-require-style" />
                                                            <span style="cursor: pointer;" class="input-group-addon input-group-append" onclick="removeSubProjectSelectedObject();">
                                                                <i class="input-group-text fa fa-remove"></i>
                                                            </span>
                                                            <span style="cursor: pointer;" class="input-group-addon input-group-append" onclick="openSubProjectHierarchyObject(event);bindHierarchyCharacterObject(this);">
                                                                <i class="input-group-text fa fa-list"></i>
                                                            </span>
                                                            <div onclick="event.stopPropagation();" class="pane-subproject-container-object" style="display: none; z-index: 30000; position: absolute; padding: 10px; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.37); background: #fff; left: 0px; top: 100%; padding-top: 0px; width: 100%;">
                                                                <div>
                                                                    <i class="fa fa-remove pull-right" style="color: #aaa; margin-top: 10px; cursor: pointer;" onclick="$('.pane-subproject-container-object').hide();"></i>
                                                                </div>
                                                                <div class="pane-subproject-object" style="overflow-y: auto; padding: 10px; max-height: 250px; padding-top: 0px; margin-top: 35px; margin-bottom: 20px">
                                                                </div>
                                                                <div class="text-center pane-subproject-empty-object" style="padding: 10px 20px; border: 1px solid #ccc; margin-top: 35px">
                                                                    ไม่พบบทบาท
                                                                </div>
                                                            </div>
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <div style="display: none">

                                                                        <asp:TextBox runat="server" ClientIDMode="Static" CssClass="form-control" ID="txtChangeFolderSubProjectObject" meta:resourcekey="txtChangeFolderSubProjectResource1" />
                                                                        <asp:TextBox runat="server" ClientIDMode="Static" CssClass="form-control" ID="txtChangeFolderSubProject" meta:resourcekey="txtChangeFolderSubProjectResource1" />

                                                                        <asp:Button Text="text" ID="btnChangeSubProjectObject" OnClick="btnChangeSubProjectObject_Click"
                                                                            ClientIDMode="Static" runat="server" meta:resourcekey="btnChangeSubProjectResource1" />

                                                                        <asp:DropDownList onchange="subProjectChange();" runat="server" ID="ddlSubProjectObject" ClientIDMode="Static" CssClass="form-control"
                                                                            Width="100%" meta:resourcekey="ddlSubProjectResource1">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <script>
                                                                function openSubProjectHierarchyObject(e) {
                                                                    $(".pane-subproject-container-object").fadeIn();
                                                                    e.stopPropagation();
                                                                }
                                                                function loading() {
                                                                    $(".pane-subproject-container-object").AGWhiteLoading(true);
                                                                }
                                                                function hierarchyLoadingObject() {
                                                                    removeLoadingObject();
                                                                    $(".pane-subproject-container-object").AGWhiteLoading(true);
                                                                }
                                                                function removeLoadingObject() {
                                                                    $(".pane-subproject-container-object").AGWhiteLoading(false);
                                                                }
                                                                function bindHierarchyCharacterObject(evt) {
                                                                    var apiUrl = "/Accountability/API/HirearchyStructureAPI.aspx";
                                                                    var hierarchytype = $(evt).parent().parent().parent().find("#ddlParticipantsDescription").val()
                                                                    console.log(hierarchytype);
                                                                    hierarchyLoadingObject();
                                                                    $.ajax({
                                                                        url: apiUrl,
                                                                        data: {
                                                                            request: "list",
                                                                            hierarchyType: hierarchytype
                                                                        },
                                                                        success: function (datas) {
                                                                            if (datas != "") {
                                                                                $(".pane-subproject-empty-object").hide();
                                                                            }
                                                                            $(".pane-subproject-object").aGapeTreeMenu({
                                                                                data: datas,
                                                                                rootText: "Root",
                                                                                rootCode: "",
                                                                                rootCount: 0,
                                                                                navigateText: "Create structure",
                                                                                onlyFolder: false,
                                                                                share: false,
                                                                                moveItem: false,
                                                                                selecting: false,
                                                                                emptyFolder: true,
                                                                                onClick: function (result) {
                                                                                    if (result.id != "") {
                                                                                        $("#txtChangeFolderSubProjectObject").val(result.id);
                                                                                        $(".pane-subproject-container-object").hide();
                                                                                        $("#btnChangeSubProjectObject").click();
                                                                                    }
                                                                                }
                                                                            });
                                                                            removeLoadingObject();
                                                                        }
                                                                    });
                                                                }
                                                                function removeSubProjectSelectedObject() {
                                                                    if ($("#txtSubProjectDescriptionObject").val() != "ไม่พบบทบาท") {
                                                                        $("#txtSubProjectDescriptionObject").val("เลือกบทบาท");
                                                                    }
                                                                }
                                                            </script>
                                                        </div>
                                                    </div>--%>
                                                </div>
                                                <div class="col-sm-12 col-md-12">
                                                    <br />
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpMainDelegate">
                                                        <ContentTemplate>
                                                            <label>Main Delegate</label>
                                                            <table class="table table-sm table-striped">
                                                                <asp:Repeater runat="server" ID="rptMainDelegate">
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td style="width: 100px;">
                                                                                <img class='image-box-card z-depth-1' data-toggle='tooltip' data-placement='top' style="width: 45px; height: 45px;"
                                                                                    src='<%# ServiceWeb.Service.UserImageService.getImgProfile(Eval("EmployeeCode").ToString()).Image_64 %>' 
                                                                                    title='<%# Eval("FullName_TH") %> (<%# Eval("FullName_EN") %>)' />
                                                                            </td>
                                                                            <td>
                                                                                <%# Eval("FullName_TH") %> (<%# Eval("FullName_EN") %>)
                                                                                <div>
                                                                                    <small>Employee Code : <%# Eval("EmployeeCode") %></small>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>

                                                                <% if (rptMainDelegate.Items.Count == 0)
                                                                  { %>
                                                                      <tr>
                                                                          <td colspan="2" class="alert alert-info">ไม่มีผู้รับผิดชอบหลัก</td>
                                                                      </tr>
                                                                <% } %>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-sm-12 col-md-12">
                                                    <br />
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpParticipants">
                                                        <ContentTemplate>
                                                            <label>Participants Delegate</label>
                                                            <table class="table table-sm table-striped">
                                                                <asp:Repeater runat="server" ID="rptParticipants">
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td style="width: 100px;">
                                                                                <img class='image-box-card z-depth-1' data-toggle='tooltip' data-placement='top' style="width: 45px; height: 45px;"
                                                                                    src='<%# ServiceWeb.Service.UserImageService.getImgProfile(Eval("EmployeeCode").ToString()).Image_64 %>' 
                                                                                    title='<%# Eval("FullName_TH") %> (<%# Eval("FullName_EN") %>)' />
                                                                            </td>
                                                                            <td>
                                                                                <%# Eval("FullName_TH") %> (<%# Eval("FullName_EN") %>)
                                                                                <div>
                                                                                    <small>Employee Code : <%# Eval("EmployeeCode") %></small>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                                <% if (rptParticipants.Items.Count == 0)
                                                                  { %>
                                                                      <tr>
                                                                          <td colspan="2" class="alert alert-info">ไม่มีผู้รับผิดชอบรอง</td>
                                                                      </tr>
                                                                <% } %>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="panel-role-dynamic" style="display:none;">
                                            <div class="row">
                                                <div class="col-sm-12 col-md-12">
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpRoleDynamic">
                                                        <ContentTemplate>
                                                            <table class="table table-sm table-bordered">
                                                                <asp:Repeater runat="server" ID="rptRoleDynamic" OnItemDataBound="rptRoleDynamic_ItemDataBound">
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td style="width:400px;padding-top: 15px;">
                                                                                <div>
                                                                                    <asp:CheckBox Text='<%# Eval("OwnerGroupName") %>' runat="server" ID="chkSelectOwner" CssClass="chk-SelectOwner" />
                                                                                    <asp:HiddenField runat="server" ID="hddOwnerGroupCode" Value='<%# Eval("OwnerGroupCode") %>' />
                                                                                </div>
                                                                                <hr />
                                                                            <%--</td>
                                                                            <td style="width:500px;padding-top: 15px;">--%>
                                                                                <div>
                                                                                    <label>Role</label>
                                                                                    <asp:DropDownList runat="server" ID="ddlRoleParticipants" CssClass="form-control  ddlParticipants-role"  AutoPostBack="true"
                                                                                        OnSelectedIndexChanged="ddlRoleParticipants_SelectedIndexChanged" onchange="AGLoading(true);">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </td>
                                                                            <td style="padding-top: 15px;">
                                                                                <div class="row">
                                                                                    <div class="col-sm-12 col-md-12">
                                                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpMainDelegate_Dynamic">
                                                                                            <ContentTemplate>
                                                                                                <label>Main Delegate</label>
                                                                                                <table class="table table-sm table-striped">
                                                                                                    <asp:Repeater runat="server" ID="rptMainDelegate_Dynamic">
                                                                                                        <ItemTemplate>
                                                                                                            <tr>
                                                                                                                <td style="width: 100px;">
                                                                                                                    <img class='image-box-card z-depth-1' data-toggle='tooltip' data-placement='top' style="width: 45px; height: 45px;"
                                                                                                                        src='<%# ServiceWeb.Service.UserImageService.getImgProfile(Eval("EmployeeCode").ToString()).Image_64 %>' 
                                                                                                                        title='<%# Eval("FullName_TH") %> (<%# Eval("FullName_EN") %>)' />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <%# Eval("FullName_TH") %> (<%# Eval("FullName_EN") %>)
                                                                                                                    <div>
                                                                                                                        <small>Employee Code : <%# Eval("EmployeeCode") %></small>
                                                                                                                    </div>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:Repeater>
                                                                                                    
                                                                                                    <tr runat="server" id="tr_MainEmpty">
                                                                                                        <td colspan="2" class="alert alert-info">ไม่มีผู้รับผิดชอบหลัก</td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                    <div class="col-sm-12 col-md-12">
                                                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpParticipants_Dynamic">
                                                                                            <ContentTemplate>
                                                                                                <label>Participants Delegate</label>
                                                                                                <table class="table table-sm table-striped">
                                                                                                    <asp:Repeater runat="server" ID="rptParticipants_Dynamic">
                                                                                                        <ItemTemplate>
                                                                                                            <tr>
                                                                                                                <td style="width: 100px;">
                                                                                                                    <img class='image-box-card z-depth-1' data-toggle='tooltip' data-placement='top' style="width: 45px; height: 45px;"
                                                                                                                        src='<%# ServiceWeb.Service.UserImageService.getImgProfile(Eval("EmployeeCode").ToString()).Image_64 %>' 
                                                                                                                        title='<%# Eval("FullName_TH") %> (<%# Eval("FullName_EN") %>)' />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <%# Eval("FullName_TH") %> (<%# Eval("FullName_EN") %>)
                                                                                                                    <div>
                                                                                                                        <small>Employee Code : <%# Eval("EmployeeCode") %></small>
                                                                                                                    </div>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:Repeater>
                                                                                                    
                                                                                                    <tr runat="server" id="tr_ParticipantsEmpty">
                                                                                                        <td colspan="2" class="alert alert-info">ไม่มีผู้รับผิดชอบหลัก</td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                </div>
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
                                    </div>

                                    <%--<div class="row TierItem">
                                        <div class="col-sm-12 col-md-12">
                                            <label>Main</label>
                                            <uc1:SmartSearchMainDelegate runat="server" id="SmartSearchMainDelegate" isOnlyFriend="false" />
                                        </div>
                                        <div class="col-sm-12 col-md-12">
                                            <label>Participant</label>
                                            <uc1:SmartSearchOtherDelegate runat="server" id="SmartSearchOtherDelegate" isOnlyFriend="false" />
                                        </div>
                                    </div>--%>
                                    <br />
                                    <div class="row TierItem hide">
                                        <div class="col-lg-12">
                                            <%--<asp:UpdatePanel ID="udpTableRoleSelectPeople" UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <asp:Repeater ID="rptTableRoleSelectPeople" runat="server">
                                                        <HeaderTemplate>
                                                            <table id="table-role-select" class="table table-hover">
                                                                <tr class="info">
                                                                    <th>Main</th>
                                                                    <th>Participant</th>
                                                                    <th>Name-Surname</th>
                                                                </tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr data-people-code="<%# Eval("EmployeeCode") %>">
                                                                <th class="main">
                                                                    <span data-mode="main" class="<%# Eval("DefaultMain") %> mode-view assignment-filter-checker main-mode-view-tier" onclick="SwapCheckerSelectRole(this,'main');">
                                                                        <i class="fa fa-square-o  fa-2x no-chack"></i><i class="fa fa-check-square-o fa-2x check-select"></i>
                                                                    </span>
                                                                </th>
                                                                <th class="Participant">
                                                                    <span data-mode="Participant" class="<%# Eval("DefaultParticipant") %> mode-view assignment-filter-checker" onclick="SwapCheckerSelectRole(this,'Participant');">
                                                                        <i class="fa fa-square-o fa-2x no-chack"></i><i class="fa fa-check-square-o fa-2x check-select"></i>
                                                                    </span>
                                                                </th>
                                                                <th>
                                                                    <label style="font-size: 20px; font-weight: 600;"><%# Eval("name") +" "+Eval("LastName_TH") %></label>
                                                                </th>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                             </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-bottom">
                    <div class="text-right">
                        <span id="btnModalAddTierMaster" class="water-button" onclick="IsValideForSaveTierMasterModalCreateTier(this);"><i class="fa fa-save"></i>&nbsp;Save</span>
                        <span id="btnModalAddTierItem" class="water-button" onclick="IsValidForSaveTierMasterModalAddItem(this);"><i class="fa fa-save"></i>&nbsp;Save</span>
                        <span id="btnModalUpdateTierItem" class="water-button" onclick="IsValidForSaveTierMasterModalUpdateItem(this);"><i class="fa fa-save"></i>&nbsp;Save</span>
                        <a class="water-button" onclick="closeInitiativeModal('ModalCreateTierMaster');closemodalInitiativeAfterRebindData();"><i class="fa fa-close"></i>&nbsp;Close</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END modal create Master -->


    <!-- modal create group  -->
    <link href="<%= Page.ResolveUrl("~/Lib-tablemodel/FeasibilityFinancialProjection.css") %>" rel="stylesheet" />
    <style>
        .table-finan > tbody > tr > td input[type=text], .table-finan > tr > td input[type=text] {
            border: none;
        }

        .table-finan > tbody > tr > td.text {
            padding: 0px;
        }
    </style>
    <script>
        function IsValideSaveTierGroupMasterControl(obj) {
            var resource = $("#txtTierGroupDescription").val();
            if (resource.split(" ").join('') == "") {
                AGMessage("TierGroup Description ไม่เป็นค่าว่าง! ");
                return;
            }

            if (AGConfirm(obj, "ยืนยันการบันทึก")) {
                $(obj).next().click();
            }
        }

        function isNumberic(evt, span) {
            //Check numberic only
            var r = (evt.keyCode >= 48 && evt.keyCode <= 57)
            if (!r) {
                $("." + span).html("Digits Only").show().fadeOut("slow");
                return r;
            }

            return r;
        }

        function IsValideUpdateGroupMasterControl(obj) {
            var objThis = $(obj).closest('.TierGroupMasterControl');
            var updateTierGroup = $(objThis).find('.text-update-TierGroup');
            if (updateTierGroup.length <= 0) {
                AGMessage("ยังไม่มีการแก้ไขข้อมูล!");
                return;
            }
            if (AGConfirm(obj, "ยืนยันการแก้ไข")) {
                var jArr = [];
                for (var i = 0; i < updateTierGroup.length; i++) {
                    var jObj = {};
                    jObj.TierGroupCode = $(updateTierGroup[i]).attr('data-TierGroupCode');
                    jObj.TierGroupDescription = $(updateTierGroup[i]).val();
                    jArr.push(jObj);
                }
                $("#txtTierGroupMasterControlResource").val(JSON.stringify(jArr));
                $(obj).next().click();
            }
        }
    </script>
    <div class="initiative-model-control-slide-panel" id="ModalCreateTierGroup">
        <div class="initiative-model-control-body-content z-depth-3">
            <div>
                <div class="initiative-model-control-header">
                    <div class="mat-box-initaive-control">
                        <div class="pull-right">
                            <i class="fa fa-close hand" onclick="closeInitiativeModal('ModalCreateTierGroup');closemodalInitiativeAfterRebindData();"></i>
                        </div>
                        <div class="one-line">
                            <label class="text-warning">Create Group</label>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-control-contant">
                    <div class="panel-body-initiative-master">
                        <div class="panel-content-initiative-master">
                            <div class="mat-box-initaive-control tab-initiative-control">
                                <div class="TierGroupMasterControl">
                                    <asp:TextBox ID="txtTierGroupMasterControlResource" ClientIDMode="Static" Style="display: none;" runat="server" />
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>TierGroup Description</label>
                                            <asp:TextBox ID="txtTierGroupDescription" ClientIDMode="Static" CssClass="form-control" runat="server" />
                                        </div>
                                        <div class="col-md-4">
                                            <br />
                                            <asp:UpdatePanel ID="udpButonForTierGroupMaster" UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <span class="btn btn-info" onclick="$(this).next().click();"><i class="fa fa-search"></i>&nbsp;Search</span>
                                                    <asp:Button ID="btnSearchForTierGroup" OnClick="btnSearchForTierGroup_Click" OnClientClick="AGLoading(true);" Style="display: none;" Text="text" runat="server" />

                                                    <span class="btn btn-success" onclick="IsValideSaveTierGroupMasterControl(this);"><i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</span>
                                                    <asp:Button ID="btnSaveForTierGroupMater" OnClick="btnSaveForTierGroupMater_Click" OnClientClick="AGLoading(true);" Style="display: none;" Text="text" runat="server" />

                                                    <span id="btnUpdateForTierGroupMater" class="btn btn-warning" onclick="IsValideUpdateGroupMasterControl(this);"><i class="fa fa-refresh" aria-hidden="true"></i>&nbsp;Edit</span>
                                                    <asp:Button ID="btnUpdateForTierGroupMaterServer" OnClick="btnUpdateForTierGroupMaterServer_Click" OnClientClick="AGLoading(true);" Style="display: none;" Text="Update" runat="server" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="udpTierData" UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <asp:Repeater ID="rptTierData" runat="server">
                                                        <HeaderTemplate>
                                                            <table class="table-finan">
                                                                <tr>
                                                                    <th></th>
                                                                    <th>Tier Name</th>
                                                                    <th>Created By</th>
                                                                    <th>Created On</th>
                                                                </tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <th></th>
                                                                <td class="text">
                                                                    <input type="text" data-tiergroupcode="<%# Eval("TierGroupCode") %>" onchange="$(this).addClass('text-update-TierGroup');" value='<%# Eval("TierGroupDescription") %>' />
                                                                </td>
                                                                <td><%# Eval("name")+" "+ Eval("LastName_TH") %></td>
                                                                <td><%# Eval("Create_ON_Des") %></td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-bottom">
                    <div class="text-right">
                        <a class="water-button" onclick="closeInitiativeModal('ModalCreateTierGroup');closemodalInitiativeAfterRebindData();"><i class="fa fa-close"></i>&nbsp;Close</a>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- END modal create group  -->
</asp:Content>
