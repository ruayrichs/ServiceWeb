var SIDForHideWorkgroup;
$.fn.aGapeTreeMenu = function (data) {
    var datas = data.data;
    var focusChart = data.focusChart;
    var focusOnClick = data.focusOnClick == undefined ? false : data.focusOnClick;
    var onlyFolder = data.onlyFolder == undefined ? false : data.onlyFolder;
    var onRename = data.onRename;
    var onClick = data.onClick;
    var onNewFolder = data.onNewFolder;
    var onMove = data.onMove;
    var onDelete = data.onDelete;
    var onWorkGroup = data.onWorkGroup;
    var onShare = data.onShare;
    var share = data.share == undefined ? false : data.share;
    var moveItem = data.moveItem == undefined ? false : data.moveItem;
    var emptyFolder = data.emptyFolder == undefined ? false : data.emptyFolder;
    var selecting = data.selecting == undefined ? false : data.selecting;
    var rootText = data.rootText == undefined ? "Root" : data.rootText;
    var rootCode = data.rootCode == undefined ? "" : data.rootCode;
    var rootCount = data.rootCount == undefined ? "" : data.rootCount;
    var focusColor = data.focusColor == undefined ? false : data.focusColor;
    var PossibleEntry = data.PossibleEntry == undefined ? [] : data.PossibleEntry;

    var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx-'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });

    var divCommand = $("<div/>", {
        class: "agape-tree-menu-command-container",
        css: {
            padding: 5,
            paddingTop: 10,
            paddingRight: 0,
            float: "right"
        }
    });

    var imgShare = $("<i/>", {
        class: "agape-tree-command agape-tree-command-share",
        title: "Share",
        css: {
            marginRight: 10
        },
        click: function () {
            if (onShare != undefined) {
                var sharedatas = [];

                $(".agape-tree-menu-checkbox-v").each(function () {
                    if ($(this).prop("checked")) {
                        sharedatas.push({
                            id: $(this).parent().find(".agape-hidden-node-id").html(),
                            name: $(this).parent().find(".agape-hidden-node-name").html(),
                            type: "v"
                        });
                    }
                });

                $(".agape-tree-menu-checkbox-s").each(function () {
                    if ($(this).prop("checked")) {
                        sharedatas.push({
                            id: $(this).parent().find(".agape-hidden-node-id").html(),
                            name: $(this).parent().find(".agape-hidden-node-name").html(),
                            type: "s"
                        });
                    }
                });

                onShare(sharedatas);
            }
        }
    });

    if (share)
        $(divCommand).prepend(imgShare);

    var divContainer = $("<div/>", {
        css: {
            padding: 10,
            paddingTop: 0,
            paddingLeft: 0
        }
    });
    var ulRoot = $("<ul/>", {
        class: "agape-tree-menu",
        css: {
            paddingLeft: 0,
            paddingTop: 10
        }
    });
    var liRoot = $("<li/>", {
        class: "agape-tree-menu-folder agape-tree-li-root",
        id: guid + ""
    });

    var RootShowName = $("<span/>", {
        class: "agape-show-node-name",
        html: rootText + (rootCount == "" ? "" : "  (" + rootCount + ")"),
        css: {
            cursor: "pointer"
        }
    });

    $(RootShowName).bind("mousedown", {
        id: "",
        type: "f",
        chart: "",
        name: rootText
    }, function (e) {
        if (e.data.type != "e" && e.button == 2) {
            $(this).appendRightClickCommand(e, undefined, onNewFolder, undefined);
        }
    });

    $(liRoot).append(RootShowName);

    $(liRoot).append($("<span/>", {
        class: "agape-hidden-folder-name",
        html: "",
        css: {
            display: "none"
        }
    }));

    var xSwitch = $("<i/>", {
        class: "agape-tree-menu-switch fa fa-minus-square-o",
        css: {
            fontSize: 12,
            marginRight: 10
        }
    });
    var imgRoot = $("<i/>", {
        class: "agape-tree-img agape-tree-open-folder"
    });


    $(liRoot).prepend(xSwitch, imgRoot);

    if (selecting) {
        var chk = $("<input/>", {
            type: "checkbox",
            css: {
                marginRight: 5,
                marginTop: 2
            },
            click: function (e) {
                e.stopPropagation();
            },
            class: "agape-tree-menu-checkbox agape-tree-menu-checkbox-f"
        });

        $(imgRoot).before(chk);
    }

    $(liRoot).aGapeAppendInnerTree({
        id: "",
        type: "f",
        chart: "",
        name: rootText
    }, datas, onClick, "", onRename, onDelete, onWorkGroup, focusChart, onlyFolder, focusOnClick, selecting, emptyFolder, onNewFolder, focusColor, rootCode, rootText, PossibleEntry);

    $(xSwitch).click(function (e) {
        $(this).parent().aGapeTreeMenuFolderClick();
        $(this).parent().find("ul").first().fadeToggle();
        e.stopPropagation();
    });
    $(RootShowName).click(function (e) {
        if (onClick != undefined) {
            if (focusOnClick) {
                $(this).FocusOnClick();
            }
            onClick({
                id: "",
                type: "f",
                name: rootText,
                rootCode: rootCode,
                rootText: rootText
            });
        }
        if (focusColor) {
            $(".agape-show-node-name").css("color", "#000");
            $(this).css("color", "cornflowerblue");
        }
        e.stopPropagation();
    });
    if (focusOnClick) {
        $(liRoot).FocusOnClick();
    }
    $(ulRoot).append(liRoot);
    $(divContainer).append(ulRoot);
    $(this).html("").append(divCommand, divContainer).on("contextmenu", function (evt) { evt.preventDefault(); });;
    //$(this).disableSelection();

    $(".agape-tree-menu-checkbox-f").click(function () {
        $(this).parent().find(".agape-tree-menu-checkbox").prop("checked", $(this).prop("checked"));
    });


    $(".agape-tree-li").each(function () {
        $(this).find(".agape-show-node-name-title").on("mouseover", function (e) {
            $(this).parent().find(".agape-tree-li-title").first().show();
            e.stopPropagation();
        });
    });

    $(".agape-tree-li").each(function () {
        $(this).find(".agape-show-node-name-title").on("mouseout", function (e) {
            $(this).parent().find(".agape-tree-li-title").first().hide();
            e.stopPropagation();
        });
    });

    if (moveItem) {
        $(".agape-tree-menu-child").sortable({
            items: "li:not(.agape-tree-menu-disable-dragable)",
            placeholder: "ui-state-highlight",
            connectWith: ".agape-tree-menu-child",
            start: function (event, ui) {
                $(".agape-tree-menu-empty-folder").show();
                $(ui.item).css({
                    border: "none"
                });
                $(ui.item).find(".agape-tree-menu-disable-dragable").hide();
            },
            over: function (event, ui) {
                $(".agape-tree-li,.agape-tree-li-root").css({
                    border: "none",
                    paddingBottom: 0
                });
                var droppable = event.target;
                $(droppable).parent().css({
                    border: "1px solid orange",
                    paddingBottom: 5
                });
            },
            stop: function (event, ui) {
                $(".agape-tree-menu-empty-folder").hide();
                var itemNode = $(ui.item).find(".agape-hidden-node-id").html();
                var itemName = $(ui.item).find(".agape-hidden-node-name").html();
                var itemType = $(ui.item).find(".agape-hidden-node-type").html();
                var oldParentNode = $(ui.item).find(".agape-hidden-node-old-parent").html();
                var newParentNode = $(ui.item).parent().parent().attr("id");
                newParentNode = newParentNode.split("-")[newParentNode.split("-").length - 1];

                if (onMove != undefined) {
                    onMove({
                        newParentNode: newParentNode,
                        itemNode: itemNode,
                        itemName: itemName,
                        itemType: itemType
                    });
                }
            }
        });
    }

    $(document).click(function (e) {
        $(".agape-tree-menu-right-click").remove();
    });
}

$.fn.aGepeAppendFolderPath = function (containerID) {
    var select = $(this);
    $(select).html("");
    $("#" + containerID).parent().find(".agape-tree-menu-folder").each(function () {
        $(select).append("<option value='" + this.id + "'>" + getTextOption(this) + "</option>");
    });
    function getTextOption(obj) {
        var text = "";
        text += getDeepSign($(obj).parents(".agape-tree-menu-folder").length);
        text += $(obj).find(".agape-hidden-folder-name").first().html()
        return text;
    }
    function getDeepSign(length) {
        var sign = " ";
        for (var i = 0; i < length; i++) {
            sign += "--";
        }
        return sign + " ";
    }
}

$.fn.aGapeAppendInnerTree = function (parentObject, treeObject, onClick, oldParentNode, onRename, onDelete, onWorkGroup, focusChart, onlyFolder, focusOnClick, selecting, emptyFolder, onNewFolder, focusColor, rootCode, rootText, PossibleEntry) {
    //treeObject = aGapeTreSortData(treeObject);
    var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx-'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });

    var innerUL = $("<ul/>", {
        class: "agape-tree-menu agape-tree-menu-child",
        css: {
            paddingLeft: 15
        },
        "data-test": guid
    });
    $(this).append(innerUL);

    for (var i = 0; i < treeObject.length; i++) {
        var type = treeObject[i].type;
        var name = treeObject[i].name;
        var id = treeObject[i].id;
        var countdata = treeObject[i].countdata;
        var chart = treeObject[i].chart == undefined ? "" : treeObject[i].chart;

        if (onlyFolder && type != "f") {
            continue;
        }

        var innerLI = $("<li/>", {
            class: "agape-tree-li",
            "data-node-id": id
        });

        $(innerUL).append(innerLI);

        var nodeLevel = recursiveNodeLevel(innerUL, 0, PossibleEntry);
        if(type == "f")
            innerLI.attr("posible-entry-node", nodeLevel);

        function filteredPossibleEntry(x) {
            return parseInt(x.NodeLevel) == parseInt(nodeLevel);
        }
        var posibleNode = PossibleEntry.filter(filteredPossibleEntry);

        var title = $("<div/>", {
            class: "agape-tree-li-title",
            html: name
        });

        var showName = $("<span/>", {
            class: "agape-show-node-name " + (type == "v" || type == "s" ? "agape-show-node-name-title" : ""),
            html: "<span class='node-possible-entry-name' style='color:#aaa'>" + (posibleNode.length > 0 ? ("[" + posibleNode[0].Name + "] ") : "") + "</span>" + name,
            css: {
                cursor: "pointer"
            }
        });

        $(showName).bind("click", {
            id: id,
            type: type,
            chart: chart,
            name: name,
            rootCode: rootCode,
            rootText: rootText
        }, function (e) {
            if (onClick != undefined) {
                if (focusOnClick) {
                    $(this).FocusOnClick();
                }
                onClick({
                    id: e.data.id,
                    type: e.data.type,
                    name: e.data.name,
                    rootCode: e.data.rootCode,
                    rootText: e.data.rootText
                });
            }
            if (focusColor) {
                $(".agape-show-node-name").css("color", "#000");
                $(this).css("color", "cornflowerblue");
            }
            e.stopPropagation();
        })

        $(showName).bind("mousedown", {
            id: id,
            type: type,
            chart: chart,
            name: name
        }, function (e) {
            if (e.data.type != "e" && e.button == 2) {
                $(this).appendRightClickCommand(e, onRename, onNewFolder, onDelete, onWorkGroup);
            }
        });

        $(innerLI).append(showName);

        if (countdata != '0' && type != "guide") {
            var countData = $("<span/>", {
                class: "agape-show-node-data ",
                html: ' (' + countdata + ')',
                css: {
                    cursor: "pointer"
                }
            });
            $(showName).append(countData);
        }

        if (type == "v" || type == "s") {
            $(innerLI).append(title);
        }

        $(innerLI).append($("<span/>", {
            class: "agape-hidden-folder-name",
            html: name,
            css: {
                display: "none"
            }
        }));

        $(innerLI).append($("<span/>", {
            class: "agape-hidden-node-id",
            html: id,
            css: {
                display: "none"
            }
        }));

        $(innerLI).append($("<span/>", {
            class: "agape-hidden-node-name",
            html: name,
            css: {
                display: "none"
            }
        }));

        $(innerLI).append($("<span/>", {
            class: "agape-hidden-node-type",
            html: type,
            css: {
                display: "none"
            }
        }));

        $(innerLI).append($("<span/>", {
            class: "agape-hidden-node-old-parent",
            html: oldParentNode,
            css: {
                display: "none"
            }
        }));

        if (focusChart != undefined && focusChart == chart) {
            $(showName).css({
                color: "orange"
            });
        }

        var imgClassType = "";
        if (type == "f") {
            imgClassType = "agape-tree-open-folder";
        }
        if (type == "v") {
            imgClassType = "agape-tree-file";
            imgClassType += getChartTypeClass(chart);
        }
        if (type == "s") {
            imgClassType = "agape-tree-file-share";
            imgClassType += getChartTypeClass(chart);
            $(showName).css({
                color: "blue"
            });
        }
        if (type == "e") {
            $(innerLI).addClass("agape-tree-menu-disable-dragable").addClass("agape-tree-menu-empty-folder");
        }
        if (type == "guide") {
            $(innerLI).addClass("agape-tree-menu-disable-dragable").addClass("agape-tree-menu-guid-folder")
        }

        var img = $("<i/>", {
            class: "agape-tree-img " + imgClassType
        });


        $(innerLI).prepend(img);

        if (selecting) {
            var chk = $("<input/>", {
                type: "checkbox",
                css: {
                    marginRight: 5,
                    marginTop: 2
                },
                click: function (e) {
                    e.stopPropagation();
                },
                class: "agape-tree-menu-checkbox agape-tree-menu-checkbox-" + type
            });

            if (type == "f") {
                $(img).before(chk);
            }
            else {
                $(innerLI).prepend(chk);
            }
        }

        if (type == "f") {
            $(innerLI).addClass("agape-tree-menu-folder").attr("id", guid + id);
            var xSwitch = $("<i/>", {
                class: "agape-tree-menu-switch fa fa-minus-square-o",
                css: {
                    fontSize: 12,
                    marginRight: 10
                }
            });
            $(innerLI).prepend(xSwitch);
            $(xSwitch).click(function (e) {
                $(this).parent().aGapeTreeMenuFolderClick();
                $(this).parent().find("ul").first().fadeToggle();
                e.stopPropagation();
            });

            if (treeObject[i].tree != undefined && treeObject[i].tree.length > 0) {
                $(innerLI).aGapeAppendInnerTree({
                    id: id,
                    type: type,
                    chart: chart,
                    name: name
                }, treeObject[i].tree, onClick, id, onRename, onDelete, onWorkGroup, focusChart, onlyFolder, focusOnClick, selecting, emptyFolder, onNewFolder, focusColor, rootCode, rootText, PossibleEntry)
            }
            else {
                if (emptyFolder) {
                    $(innerLI).aGapeAppendInnerTree({
                        id: id,
                        type: type,
                        chart: chart,
                        name: name
                    }, [{
                        id: "empty",
                        name: "Empty",
                        type: "e",
                        tree: []
                    }], undefined, id, onRename, onDelete, onWorkGroup, focusChart, onlyFolder, focusOnClick, selecting, emptyFolder, onNewFolder, focusColor, rootCode, rootText, PossibleEntry);
                }
            }


        }
    }

    innerUL.appendGuideLine(PossibleEntry, onNewFolder, parentObject, {
        id: id,
        type: type,
        chart: chart,
        name: name
    });

    function getChartTypeClass(chart) {
        chart = chart.trim();
        if (chart == "")
            return "";
        else {
            var chartClass = " agape-tree-chart ";

            if (chart == "line")
                chartClass += "agape-tree-chart-line";
            if (chart == "barline")
                chartClass += "agape-tree-chart-barline";
            if (chart == "pareto")
                chartClass += "agape-tree-chart-pareto";
            if (chart == "scatter")
                chartClass += "agape-tree-chart-scatter";
            if (chart == "bar")
                chartClass += "agape-tree-chart-bar";
            if (chart == "pie")
                chartClass += "agape-tree-chart-pie";
            if (chart == "waterfall")
                chartClass += "agape-tree-chart-waterfall";

            return chartClass;
        }

    }
}

$.fn.appendGuideLine = function (possibleEntry, onNewFolder, parentObject, nodeObject) {
    var innerUL = $(this);
    var nodeLevel = recursiveNodeLevel(innerUL, 0, possibleEntry);
    if (nodeLevel < possibleEntry.length) {
        var datas = parentObject;

        var innerLI = $("<li/>", {
            class: "agape-tree-li agape-tree-menu-folder agape-tree-menu-guideline"
        });

        $(innerUL).append(innerLI);

        var title = $("<div/>", {
            class: "agape-tree-li-title",
            html: possibleEntry[nodeLevel].Name
        });

        var showName = $("<span/>", {
            class: "agape-show-node-name ",
            html: "<i class='fa fa-plus-circle'></i> add " + possibleEntry[nodeLevel].Name + " in " + parentObject.name,
            css: {
                cursor: "pointer",
                color: "#009688"
            }
        });

        showName.bind("mousedown", {
            id: datas.id,
            type: datas.type,
            chart: datas.chart,
            name: datas.name
        }, function (e) {
            var name = prompt("เพิ่มข้อมูล " + possibleEntry[nodeLevel].Name + " (: " + e.data.name + ")", "เพิ่มข้อมูลใหม่");
            if (name != undefined && name != null && name.trim() != "" && name != e.data.name) {
                onNewFolder({
                    parentid: e.data.id,
                    name: name
                });
            }
            e.stopPropagation();
        });

        $(innerLI).append(showName);


    }

}

$.fn.appendRightClickCommand = function (e, onRename, onNewFolder, onDelete, onWorkGroup) {

    $(".agape-tree-menu-right-click").remove();
    var rightClickCommand = $("<div/>", {
        class: "agape-tree-menu-right-click",
        html: "<p style='border-bottom:1px solid #ccc'>" + e.data.name + "</p>",
        click: function (e) {
            e.stopPropagation();
        }
    });

    // ================= New Folder ======================
    if (onNewFolder != undefined) {
        var NewFolder = $("<span/>", {
            class: "agape-tree-menu-right-click-newfolder btn btn-default btn-fit",
            title: "Create New Folder"
        });
        $(NewFolder).prepend($("<b/>", {
            class: "fa fa-folder-open"
        }));
        $(NewFolder).bind("click", {
            id: e.data.id,
            name: e.data.name,
            type: e.data.type
        }, function (event) {
            var name = prompt("Create New Folder (in " + e.data.name + ")", "New Folder");
            if (name != undefined && name != null && name.trim() != "" && name != event.data.name) {
                onNewFolder({
                    parentid: event.data.id,
                    name: name
                });
            }
        });
        $(rightClickCommand).append(NewFolder);
    }

    // ================= Rename ===========================

    if (onRename != undefined) {
        var RenameMenu = $("<span/>", {
            class: "agape-tree-menu-right-click-rename btn btn-default btn-fit",
            title: "Rename"
        });
        $(RenameMenu).prepend($("<b/>", {
            class: "fa fa-font"
        }));
        $(RenameMenu).bind("click", {
            id: e.data.id,
            name: e.data.name,
            type: e.data.type
        }, function (event) {
            var name = prompt("Rename", event.data.name);
            if (name != undefined && name != null && name.trim() != "" && name != event.data.name) {
                onRename({
                    id: event.data.id,
                    name: name,
                    type: event.data.type
                });
            }
        });
        $(rightClickCommand).append(RenameMenu);
    }
    // ================= Delete ===========================
    if (onDelete != undefined) {
        var DeleteMenu = $("<span/>", {
            class: "agape-tree-menu-right-click-rename btn btn-default btn-fit",
            title: "Delete"
        });
        $(DeleteMenu).prepend($("<i/>", {
            class: "fa fa-trash"
        }));
        $(DeleteMenu).bind("click", {
            id: e.data.id,
            name: e.data.name,
            type: e.data.type
        }, function (event) {
            if (confirm("ต้องการลบรายการหรือไม่")) {
                onDelete({
                    id: event.data.id,
                    name: e.data.name,
                    type: event.data.type
                });
            }
        });
        $(rightClickCommand).append(DeleteMenu);
    }
    // ================= Delete ===========================

    if (SIDForHideWorkgroup) {

        // ================= Add WorkGroup ===========================
        if (onWorkGroup != undefined) {
            var WorkGroupMenu = $("<span/>", {
                class: "agape-tree-menu-right-click-rename btn btn-default btn-fit",
                title: "WorkGroup",
                css: { fontSize: 12,fontWeight : 600
                     }
            });
            $(WorkGroupMenu).prepend("WG");
            $(WorkGroupMenu).bind("click", {
                id: e.data.id,
                name: e.data.name,
                type: e.data.type
            }, function (event) {
                //onWorkGroup({
                //    id: event.data.id,
                //    name: e.data.name,
                //    type: event.data.type
                //});
                openModalDataForSelectMapingWorkGroup(event.data.id, e.data.name, e.data.type);
            });
            $(rightClickCommand).append(WorkGroupMenu);
        }
        // ================= End WorkGroup ===========================
    }


    $(this).append(rightClickCommand);
}

$.fn.FocusOnClick = function () {
    $(".agape-show-node-name").css({
        background: "",
        color: "#000",
        padding: 0
    });
    $(this).find(".agape-show-node-name").first().css({
        background: "orange",
        color: "#fff",
        paddingLeft: 10,
        paddingRight: 10
    });
}

$.fn.aGapeTreeMenuFolderClick = function () {
    var imgIcon = $(this).find(".agape-tree-img").first();
    var xSwitch = $(this).find(".agape-tree-menu-switch").first();
    if ($(imgIcon).hasClass("agape-tree-close-folder")) {
        $(imgIcon).removeClass("agape-tree-close-folder").addClass("agape-tree-open-folder");
        $(xSwitch).removeClass("fa-plus-square-o").addClass("fa-minus-square-o");
    }
    else {
        $(imgIcon).removeClass("agape-tree-open-folder").addClass("agape-tree-close-folder");
        $(xSwitch).removeClass("fa-minus-square-o").addClass("fa-plus-square-o");
    }
}

function recursiveNodeLevel(elt, level, possibleEntry) {
    var closetTree = $(elt).parent().closest("ul.agape-tree-menu");
    if (closetTree.length > 0) {
        level++;
        level = recursiveNodeLevel(closetTree, level, possibleEntry);
    }
    return level;
}

function aGapeTreSortData(obj) {
    obj.sort(function (a, b) {
        if (a.index > b.index) {
            return 1;
        }
        if (a.index < b.index) {
            return -1;
        }
        return 0;
    });

    return obj;
}
