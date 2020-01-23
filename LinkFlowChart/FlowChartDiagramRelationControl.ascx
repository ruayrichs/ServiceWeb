<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlowChartDiagramRelationControl.ascx.cs" Inherits="ServiceWeb.LinkFlowChart.FlowChartDiagramRelationControl" %>
<script type="text/javascript" src="https://cdn.rawgit.com/asvd/dragscroll/master/dragscroll.js"></script>
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

    .one-line {
        overflow-x: hidden;
        white-space: nowrap;
        text-overflow: ellipsis;
    }

    #panelAddNewItems {
        margin: 0px 0px 10px 0px;
        max-height: 150px;
        overflow-x: hidden;
        overflow-y: auto;
        border-radius: 4px;
    }

    #btnSave {
        margin-right: 10px;
        padding: 6px 10px;
    }

    .tree {
        margin: 0 auto;
        display: table;
    }

        .tree ul {
            padding-top: 20px;
            position: relative;
            transition: all 0.5s;
            -webkit-transition: all 0.5s;
            -moz-transition: all 0.5s;
            margin-left: -40px;
            margin-top: -5px;
        }

        .tree li {
            float: left;
            text-align: center;
            list-style-type: none;
            position: relative;
            padding: 20px 0px 0px 0px;
            transition: all 0.5s;
            -webkit-transition: all 0.5s;
            -moz-transition: all 0.5s;
        }
            .tree li::before, .tree li::after {
                content: '';
                position: absolute;
                top: 0;
                right: 50%;
                border-top: 1px solid #ccc;
                width: 50%;
                height: 20px;
            }

            .tree li::after {
                right: auto;
                left: 50%;
                border-left: 1px solid #ccc;
            }

            .tree li:only-child::after, .tree li:only-child::before {
                display: none;
            }

            .tree li:only-child {
                padding-top: 0;
            }

            .tree li:first-child::before, .tree li:last-child::after {
                border: 0 none;
            }
            .tree li:last-child::before {
                border-right: 1px solid #ccc;
                border-radius: 0 5px 0 0;
                -webkit-border-radius: 0 5px 0 0;
                -moz-border-radius: 0 5px 0 0;
            }

            .tree li:first-child::after {
                border-radius: 5px 0 0 0;
                -webkit-border-radius: 5px 0 0 0;
                -moz-border-radius: 5px 0 0 0;
            }

        .tree ul ul::before {
            content: '';
            position: absolute;
            top: 0;
            left: calc(50% + 20px);
            border-left: 1px solid #ccc;
            width: 0;
            height: 20px;
        }

        .tree li a {
            border: 1px solid #ccc;
            padding: 5px 10px;
            text-decoration: none;
            color: #666;
            font-family: arial, verdana, tahoma;
            font-size: 11px;
            display: inline-block;
            border-radius: 5px;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            transition: all 0.5s;
            -webkit-transition: all 0.5s;
            -moz-transition: all 0.5s;
            width: 200px;
            overflow-x: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
            margin: 0 5px;
        }

            .tree li a:after {
                border: 100px solid #ccc;
            }

            .tree li a:hover, .tree li a:hover + ul li a {
                background: #c8e4f8;
                color: #000;
                border: 1px solid #94a0b4;
            }
                .tree li a:hover + ul li::after,
                .tree li a:hover + ul li::before,
                .tree li a:hover + ul::before,
                .tree li a:hover + ul ul::before {
                    border-color: #94a0b4;
                }


        .tree .node-active {
            background-color: #009688;
            color: #fff;
            border-color: #00776c;
        }

        .tree.tree-rotate {
            transform: rotate(180deg);
        }

            .tree.tree-rotate li a {
                transform: rotate(180deg);
            }
            .tree.tree-rotate li .relation-text {
                transform: rotate(180deg);
                top: 5px;
            }

        .tree .caret {
            position: absolute;
            left: calc(50% - 4px);
            font-size: 15px;
            z-index: 1;
            color: #ccc;
            float: left;
            margin-top: -10px;
        }

        .tree .remove {
            position: absolute;
            font-size: 15px;
            z-index: 1;
            float: left;
            left: calc(100% - 13px);
            margin-top: -5px;
            color: #FF5722;
            background-color: #fff;
            cursor: pointer;
            border-radius: 50%;
        }

        .tree.tree-rotate .caret {
            transform: rotate(180deg);
            left: calc(50% - 4px);
            margin-top: 24px;
        }

    .tree.tree-display-mode li[data-node-level='0']:after,
    .tree.tree-display-mode li[data-node-level='0']:before {
        border: none !important;
    }

    .tree.tree-display-mode li[data-node-level='0'] > .caret{
        display: none !important;
    }

    .relation-text {
        /*position: absolute;
        font-size: 12px;
        z-index: 1;
        float: left;
        margin-top: -21px;
        width: 100%;
        color: #777;*/
        position: absolute;
        font-size: 12px;
        z-index: 1;
        margin-top: -21px;
        width: 100%;
        color: #777;
        margin: -21px auto 0px auto;
        text-align: center;
        left: 0;
    }

    .diagram-full-screen {
        position: fixed;
        top: 0;
        left: 0;
        width: 100vw;
        height: 100vh;
        background-color: #fff;
        z-index: 10000;
        padding: 15px !important;
    }
    
    .diagram-full-screen .icon-control {
        right: 25px;
        font-size: 20px;
        position: fixed;
    }

    .icon-control {
        position: absolute;
        right: 15px;
        font-size: 20px;
        z-index: 10;
    }
    .icon-control i {
        cursor: pointer;
        margin-right: 10px;
    }
    
    .icon-control .display-full-screen{
        display: none;
    }
    .icon-control .hide-full-screen{
        display: initial;
    }
    .diagram-full-screen .display-full-screen {
        display: initial !important;
    }
    .diagram-full-screen .hide-full-screen {
        display: none !important;
    }
    .dragscroll {
        cursor: grab;
        cursor: -o-grab;
        cursor: -moz-grab;
        cursor: -webkit-grab;
    }
</style>


<div class="form-row <%= AlowEditMode ? "" : " hide " %>>">
    <div class="col-md-12">
        <div class="pull-right box-mode">
            <label class="switch">
                <input type="checkbox" id="chkEnableEditMode" onchange="enableEditMode(this);" class="ticket-allow-editor">
                <span class="slider round"></span>
            </label>
            <b>&nbsp;&nbsp;Edit Mode</b>
        </div>
        <div class="pull-right">
            <button type="button" class="btn btn-success" id="btnSave" disabled="disabled" onclick="saveDataDiagram();">
                <i class="fa fa-save"></i>&nbsp;&nbsp;Save
            </button>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button Text="Save" runat="server" CssClass="hide" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="width: calc(100% - 233px);">
            <div class="input-group mb-3">
                <input type="text" name="txtSearchKey" id="txtSearchKey" value="" class="form-control ticket-allow-editor" 
                    disabled="disabled" onkeypress="searchItemNew(event);" />
                <div class="input-group-append" onclick="searchItemNew(null, true);">
                    <i class="fa fa-search input-group-text" style="font-size: 22px; cursor:pointer;"></i>
                </div>
            </div>

        </div>
    </div>
    <div class="col-md-12">
        <div class="" id="panelAddNewItems"></div>
    </div>
</div>
<hr style="margin-top:0;" />
<div style="overflow: auto; padding-bottom: 40px;" id="panel-bord" class="dragscroll">
    <div class="icon-control">
        <i class="fa fa-search-plus fa-fw display-full-screen" onclick="zoomDiagram(1);"></i>
        <i class="fa fa-search-minus fa-fw display-full-screen" onclick="zoomDiagram(-1);"></i>
        <i class="fa fa-times-circle-o fa-fw display-full-screen" onclick="diagramDefaultScreen();"></i>
        <i class="fa fa-television fa-fw hide-full-screen" onclick="diagramFullScreen();"></i>
    </div>
    <div id="panelTree">
    </div>
</div>
<script>
    var zoomDispaly = 100;
    function diagramFullScreen() {
        $("#panel-bord").addClass("diagram-full-screen");
    }
    function diagramDefaultScreen() {
        zoomDispaly = 100;
        zoomDiagram(0);
        $("#panel-bord").removeClass("diagram-full-screen");
    }

    function zoomDiagram(zoom) {
        zoomDispaly = zoomDispaly + zoom;
        $("#panelTree").css({ zoom: zoomDispaly + "%" });
    }

    function searchItemNew(e, autoSearch) {
        if ($("#txtSearchKey").prop("disabled")) {
            return;
        }
        if (autoSearch || e.keyCode == 13) {
            $("#panelAddNewItems").AGWhiteLoading(true);
            var actionCase = "";
            if ($("#<%= txtRelationType.ClientID %>").val().toLowerCase() == "equipment") {
                actionCase = "equipment_fordiagram";
            } else if ($("#<%= txtRelationType.ClientID %>").val().toLowerCase() == "class") {
                actionCase = "class_fordiagram";
            } else if ($("#<%= txtRelationType.ClientID %>").val().toLowerCase() == "ticket") {
                actionCase = "ticket_fordiagram";
            }
            var OtherKey = null;
            if ($("#<%= txtOtherKey.ClientID %>").val() != "") {
                OtherKey = $("#<%= txtOtherKey.ClientID %>").val();
            }
            var postData = {
                actionCase: actionCase,//"equipment",
                keySearch: $("#txtSearchKey").val(),
                IsRelation: "true",
                nodeCode: $("#<%= txtNodeActive.ClientID %>").val(),
                OtherKey: OtherKey
            };

            $.ajax({
                type: "GET",
                url: servictWebDomainName + "API/AutoCompleteAPI.aspx",
                data: postData,
                success: function (datas) {
                    $("#panelAddNewItems").html('');
                    var row = $("<div>", {
                        class: "form-row"
                    });
                    if (datas.length == 0) {
                        var col = $("<div>", {
                            class: "col-md-12"
                        });
                        var box = $("<div>", {
                            class: "alert alert-info text-center",
                            html: "No data."
                        });

                        col.append(box);
                        row.append(col);
                    }
                    for (var i = 0; i < datas.length; i++) {
                        if (i >= 200) {
                            continue;
                        }
                        var data = datas[i];

                        var class_AddItem = $("#<%= txtCssClass_Add.ClientID %>").val();
                        var col = $("<div>", {
                            class: class_AddItem
                        });

                        var box = $("<div>", {
                            class: "box one-line",
                            html: data.desc + "&nbsp;",
                            "data-code": data.code,
                            "data-desc": data.desc,
                            title: data.display
                        }).css({
                            cursor: "pointer"
                        });
                        if ($("#panelTree").find("li[data-node-code='" + data.code + "'][data-node-level='1']").length > 0) {
                            box.addClass("active");
                        }

                        box.bind("click", function () {
                            if ($("#panelTree").find("a.node-active").parent().find("ul").length == 0) {
                                var ul = $("<ul>", {});
                                $("#panelTree").find("a.node-active").parent().append(ul);
                            }

                            var target = $("#panelTree").find("a.node-active").next();
                            var ItemCode = $(this).attr("data-code");
                            var ParentItemCode = $("#<%= txtNodeActive.ClientID %>").val();
                            var ItemDescription = $(this).attr("data-desc");
                            var Level = 1;

                            $(this).toggleClass("active");
                            eltSelect = $(this);
                            if ($(this).hasClass("active")) {

                                var li = $("<li>", {
                                    "data-node-code": ItemCode,
                                    "data-node-parent-code": ParentItemCode,
                                    "data-node-description": ItemDescription,
                                    "data-node-relation-code": "",
                                    "data-node-level": Level
                                });

                                var label_relation = $("<span>", {
                                    class: "relation-text",
                                    "data-node-relation-code": '',
                                    html: ''
                                });

                                a_href = "JavaScript:;";
                                var a = $("<a>", {
                                    href: a_href,
                                    html: ItemDescription,
                                    title: ItemDescription
                                });

                                var icon_caret = $("<i>", {
                                    class: "fa fa-caret-down caret"
                                });

                                var icon_remove = $("<i>", {
                                    class: "fa fa-times-circle-o remove"
                                });
                                icon_remove.removeNode();

                                li.append(label_relation);
                                li.append(icon_caret);
                                li.append(icon_remove);
                                li.append(a);

                                eltTarget = target;
                                newElt_li = li;

                                var RequiredRelation = $("#<%= txtRequiredRelationType.ClientID %>").val().toLowerCase() == "true" ? true : false;
                                if (RequiredRelation) {
                                    $("#modal-relation-type").modal("show");
                                } else {
                                    var val = '';
                                    var text = '';

                                    newElt_li.find(".relation-text").attr("data-node-relation-code", val);
                                    newElt_li.find(".relation-text").html(text);
                                    newElt_li.attr("data-node-relation-code", val);
                                    eltTarget.append(newElt_li);

                                    AdjustWidthPanelTree();
                                    hideModalSelectRalationType(false);
                                }
                            } else {
                                target.find("li[data-node-code='" + ItemCode + "']").remove();
                                AdjustWidthPanelTree();
                            }
                        });

                        col.append(box);
                        row.append(col);
                    }
                    $("#panelAddNewItems").append(row);
                    $("#panelAddNewItems").AGWhiteLoading(false);
                }
            });
            return false;
        }
    }

    function enableEditMode(obj) {
        if ($(obj).prop("checked")) {
            $("#txtSearchKey").prop("disabled", false);
            $("#btnSave").prop("disabled", false);
            searchItemNew(null, true);

            if (typeof (controlDisplayEditTicketRelation) === "function") {
                controlDisplayEditTicketRelation(true);
            }
        } else {
            $("#txtSearchKey").prop("disabled", true);
            $("#btnSave").prop("disabled", true);
            $("#panelAddNewItems").html('');

            if (typeof (controlDisplayEditTicketRelation) === "function") {
                controlDisplayEditTicketRelation(false);
            }
        }
        initFlowChartDiagram();
    }

    function saveDataDiagram() {
        var ItemGroup = $("#<%= txtRelationType.ClientID %>").val();
        var dataSave = [];
        $("#panelTree").find("[data-node-level='1']").each(function () {
            dataSave.push({
                ItemGroup: ItemGroup,
                ItemCode: $(this).attr("data-node-code"),
                ItemDescription: $(this).attr("data-node-description"),
                ParentItemCode: $(this).attr("data-node-parent-code"),
                RelationCode: $(this).attr("data-node-relation-code"),
                Level: 1
            });
        });
        $("#<%= txtDataSave.ClientID %>").val(JSON.stringify(dataSave));
        try { $("#saveRelation").click(); } catch (e) { }
    }

    var datas = {};
    function initFlowChartDiagram() {
        datas = {
            config: "",
            AlowEditMode: "<%= AlowEditMode.ToString().ToLower() %>" == "true" ? true : false,
            editMode: $("#chkEnableEditMode").prop("checked"),
            nodeActive: $("#<%= txtNodeActive.ClientID %>").val(),
            parendDataSource: JSON.parse($("#<%= txtParendDataSource.ClientID %>").val()),
            childDataSource: JSON.parse($("#<%= txtChildDataSource.ClientID %>").val()),
            MaximumItemInLevel: parseInt("<%= MaximumItemInLevel %>"),
            MaximumLevel: parseInt("<%= MaximumLevel %>")
        }
        $("#panelTree").initFlowChartDiagram(datas);
    }

    $.fn.initFlowChartDiagram = function (datas) {
        var AlowEditMode = datas.AlowEditMode;
        var editMode = datas.editMode;
        $(this).html('');

        $.fn.genTreeDiagram = function (DataSource, nodeActive, IsRotate, editMode) {
            var panelTree = $("<div>", {
                class: "tree "//,
            });
            if (!AlowEditMode) {
                panelTree.addClass("tree-display-mode")
            }
            if (IsRotate) {
                panelTree.addClass("tree-rotate")
            }
            var ul = $("<ul>", {});

            for (var i = 0; i < DataSource.length; i++) {
                var data = DataSource[i];

                var li = $("<li>", {
                    "data-node-code": data.ItemCode,
                    "data-node-parent-code": data.ParentItemCode,
                    "data-node-description": data.ItemDescription,
                    "data-node-relation-code": data.RelationCode,
                    "data-node-level": data.Level
                });

                var a_href = "";
                if (editMode) {
                    a_href = "JavaScript:;";
                } else {
                    if (data.Level == 0 && AlowEditMode) {
                        a_href = "JavaScript:;";
                    } else {
                        a_href = "<%= URLNodeRedirect %>".replace("{#ID}", encodeURI(data.ItemCode));
                    }
                }
            var a = $("<a>", {
                href: a_href,
                html: data.ItemDescription,
                title: data.ItemDescription
            });

            var icon_caret = $("<i>", {
                class: "fa fa-caret-down caret"
            });

            var label_relation = $("<span>", {
                class: "relation-text",
                "data-node-relation-code": data.RelationCode,
                html: data.RelationDesc
            });

            if (data.ItemCode == nodeActive) {
                a.addClass("node-active");
                icon_caret.css({ left: "calc(50% - 5px)" });
            }

            li.append(label_relation);
            li.append(icon_caret);

            if (data.Level == 0) {
                li.append(a);
                ul.append(li);
                panelTree.append(ul);
            } else {
                if (editMode && data.Level > 1) {
                    continue;
                }
                if (editMode) {
                    var icon_remove = $("<i>", {
                        class: "fa fa-times-circle-o remove"
                    });
                    icon_remove.removeNode();
                    li.append(icon_remove);
                }

                var parentTarget = panelTree.find("[data-node-code='" + data.ParentItemCode + "'][data-node-level='" + (data.Level - 1) + "']");
                li.append(a);

                if (parentTarget.find("ul").length == 0) {
                    var ul_New = $("<ul>", {});
                    ul_New.append(li);
                    parentTarget.append(ul_New);
                } else {
                    parentTarget.find("ul").append(li);
                }
            }
        }

            $(this).append(panelTree);
        }

        if (!editMode) {
            $(this).genTreeDiagram(datas.parendDataSource, datas.nodeActive, true, editMode);
        }
        $(this).genTreeDiagram(datas.childDataSource, datas.nodeActive, false, editMode);

        if (datas.parendDataSource.length > 0 && !editMode) {
            $(this).find(".tree:first .node-active").prev().remove();
            $(this).find(".tree:first .node-active").remove();
            $(this).find(".tree:last").css({
                marginTop: -25
            });

            $(this).find(".tree:first li").each(function () {
                if ($(this).find("ul").length == 0) {
                    $(this).find(".caret").remove();
                }
            });

        }
        if (datas.parendDataSource.length <= 1 && !editMode) {
            $(this).find(".tree:last .node-active").prev().remove();
        }

        if (editMode) {
            $(this).find(".tree:last .node-active").prev().remove();
        }
        AdjustWidthPanelTree();
    }

    $.fn.removeNode = function () {
        $(this).bind("click", function () {
            var code = $(this).parent().attr("data-node-code");
            $("#panelAddNewItems").find(".box[data-code='" + code + "']").removeClass("active");
            $(this).parent().remove();
        });
    }

    function AdjustWidthPanelTree() {
        var width = 1000000;
        $("#panelTree").css({
            width: width,
            margin: "0 auto"
        });

        if ($("#panelTree").find(".tree:first").width() > $("#panelTree").find(".tree:last").width()) {
            width = $("#panelTree").find(".tree:first").width();
        } else {
            width = $("#panelTree").find(".tree:last").width();
        }
        width = width + 100;
        $("#panelTree").css({
            width: width
        });
        var scrollLeft = (width / 2) - ($("#panelTree").parent().width() / 2);
        $("#panelTree").parent().scrollLeft(scrollLeft);
    }

    var eltTarget, newElt_li, eltSelect;
    function setRelationType() {
        var val = $("#ddlRelationType").val();
        var text = $("#ddlRelationType option:selected").text();

        if (val == "") {
            AGMessage("กรุณาเลือก Relation Type!");
            return;
        }

        newElt_li.find(".relation-text").attr("data-node-relation-code", val);
        newElt_li.find(".relation-text").html(text);
        newElt_li.attr("data-node-relation-code", val);
        eltTarget.append(newElt_li);

        AdjustWidthPanelTree();
        hideModalSelectRalationType(false);
    }

    function hideModalSelectRalationType(undu) {
        if (undu) {
            eltSelect.removeClass("active");
        }
        $("#modal-relation-type").modal("hide");
    }
</script>

<div class="hide d-none">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpDataSource">
        <ContentTemplate>
            <asp:TextBox runat="server" ID="txtNodeActive" Text="" />
            <asp:TextBox runat="server" TextMode="MultiLine" ID="txtParendDataSource" Text="[]" />
            <asp:TextBox runat="server" TextMode="MultiLine" ID="txtChildDataSource" Text="[]" />
            <asp:TextBox runat="server" ID="txtAlowEditMode" Text="true" />
            <asp:TextBox runat="server" ID="txtRelationType" Text="EQUIPMENT" />
            <asp:TextBox runat="server" ID="txtRelationClass" Text="" />
            <asp:TextBox runat="server" TextMode="MultiLine" ID="txtDataSave" Text="[]" />
            <asp:TextBox runat="server" ID="txtURLNodeRedirect" Text="" />
            <asp:TextBox runat="server" ID="txtRequiredRelationType" Text="true" />
            <asp:TextBox runat="server" ID="txtOtherKey" Text="" />
            <asp:TextBox runat="server" ID="txtCssClass_Add" Text="col-xs-6 col-sm-4 col-md-3 col-lg-2" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<!-- Modal -->
<div class="modal fade" id="modal-relation-type" tabindex="-1" role="dialog" aria-labelledby="modalLabelHeader"
    aria-hidden="true" data-backdrop="false" data-keyboard="false">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="modalLabelHeader">Select Ralation Type
                </h5>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpDataRelation">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-12">
                                <label>
                                    Relation Type
                                </label>
                                <asp:DropDownList runat="server" ID="ddlRelationType" CssClass="form-control ticket-allow-editor" ClientIDMode="Static"
                                    DataValueField="RelationCode" DataTextField="RelationDescript">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" onclick="hideModalSelectRalationType(true);">Close</button>
                <button type="button" class="btn btn-primary" onclick="setRelationType();">Save changes</button>
            </div>
        </div>
    </div>
</div>

<%--TODO ปรับความเร็ว --%>
<%--
<ul>
    <li data-node-code="Expand" data-node-parent-code="Expand" data-node-description="Expand" 
        data-node-relation-code="Expand" data-node-level="4" style="left: calc(50% + -11px);">
        <a href="-/crm/Master/Equipment/EquipmentDiagramRelation.aspx?id=LH000106" title="Expand" style="
        width: 22px;
        border-radius: 50%;
        font-size: 18px;
        height: 22px;
        overflow: hidden;
        font-weight: 900;
        padding: 0;
        vertical-align: middle;
        display: table-cell;
        line-height: 0;
        margin: 0 auto;">
            +
        </a>
    </li>
</ul>
--%>