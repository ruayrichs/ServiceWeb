var ActivityInboxTotalRow = 0;
var ActivityInboxPhysicalTotalRow = 0;
var ActivityInboxLeastDate = "";
var ActivityInboxLazyLoadFlag = false;
var isInboxProject = false;
var isSendMailEvent = false;
var isMinimalMode = false;
var isSpecialDisplayPopup = false;

$(document).ready(function () {

    $(".xpanel-heading").click(function () {
        $(".xpanel-heading").removeClass("xpanel-heading-active");
        $(this).addClass("xpanel-heading-active");


        $(".data-content ").hide();
        var id = $(this).attr("id");
        $("." + id).fadeIn();
    });

    $("#p1").click();

    $(".tab-panel-menu").click(function () {
        $(".tab-panel-menu").removeClass("tab-panel-menu-active");
        $("#overflow-activity-container .activity-rows").removeClass("activity-rows-active");
        $(this).addClass("tab-panel-menu-active");
        if ($(this).attr("id") == "tab-panel-main") {
            $(".tab-panel-content").hide();
            $(".tab-panel-main").fadeIn();
            if ($(".activity-selection").length == 0) {
                StartActivityInbox();
            }
            else {
                if ($(".row-grouping").length == 0) {
                    groubByActivity();
                } else {
                    var groupingColSpan = $(".tr-fake td:visible").length;
                    $(".column-grouping").attr("colspan", groupingColSpan);
                }
            }
        }
    });

    $("#tab-workgroup").click(function (e) {
        $(".tab-panel-work").removeClass("tab-panel-work-active");
        $(this).addClass("tab-panel-work-active");
        $("#tab-panel-workgroup").show();
        $("#tab-panel-myfolder").hide();
    })

    $("#tab-myfolder").click(function (e) {
        $(".tab-panel-work").removeClass("tab-panel-work-active");
        $(this).addClass("tab-panel-work-active");
        $("#tab-panel-myfolder").show();
        $("#tab-panel-workgroup").hide();
    })
    
    $("#tab-panel-closer").click(function (e) {
        $("#tab-panel-main").click();
        $("#tab-panel-activity").hide();
        e.stopPropagation();
    });

    $(".search-all-activity").on("keyup", function () {
        $(this).searchActivity();
    });

    $(".select-activity-client-filter").change(function () {
        MainFilterChangeFeatureButtonGroup();
        $('#specialFilter').val('');
        StartActivityInbox();
    });

    $(".select-activity-type-filter").change(function () {
        StartActivityInbox();
    });

    $(".select-activity-category-filter").change(function () {
        StartActivityInbox();
    });

    $(".project-grouping").click(function () {
        $(".search-all-activity,.select-activity-client-filter,.select-activity-type-filter,.select-activity-category-filter").val("");
        $(".select-activity-client-filter").change();
        var projectCode = $(this).find("span").attr("class");
        $("#main-form-filter-header").html("Project : " + $(this).find("span").html()).focusElementByColor();
        $("#main-form-filter-code").html(projectCode);
    });

    startActivityLink();

    var loadInbox = true;
    try {
        loadInbox = ActivityManagementMainScriptLoadinboxFlag();
    } catch (e) { }

    if (loadInbox) {
        var MainReturn = ActivityManagementMainScript();
        if (MainReturn === false) {
            StartBindDataActivity();
        }
        else if (MainReturn === true) {
            isInboxProject = true;
            StartBindDataActivity();
        }
        else {
            FilterActivityStateGlobal(MainReturn);
        }
    }
    else {
        $("#tab-panel-main").addClass("waiting-load-inbox");
        loadActivityDetailLinked();
        if (!isMinimalMode) {
            bindHierarchyAllProject();
            bindHierarchyMyFolder();
        }
    }
});

function MainFilterChangeFeatureButtonGroup() {
    var val = $('.select-activity-client-filter').val();
    $(".btn-group-activity-filter .btn").removeClass("active");
    $(".btn-group-activity-filter .btn[data-btn-filter='" + val + "']").addClass("active");
    if ($(".btn-group-activity-filter .btn.active").length == 0) {
        $(".btn-group-activity-filter .btn:first").addClass("active");
    }
}

function StartBindDataActivity(startFlag) {
    if (enbleAutoBindActivity || startFlag) {
        try {
            bindHierarchyAllProject();
            bindHierarchyMyFolder();
        }
        catch (e) { }
        StartActivityInbox();
    }
}

function refreshAllNotification() {
    $("#btnRefreshNotiCount").click();
}

function FilterActivityStateGlobal(state) {
    $('.search-all-activity,.select-activity-type-filter,.select-activity-client-filter,.select-activity-category-filter').val("");
    $(".select-activity-client-filter").change();
    $("#main-form-filter-code").html("");
    $("#main-form-filter-subproject-code").html("");
    $("#main-form-filter-header").html("Inbox");

    var datas = {
        filter: state,
        type: "",
        searchKey: "",
        projectCode: "",
        subProjectCode: "",
        mainDelegateCode: "",
        assetCode: "",
        isContinueLazyLoad: false,
        newObjectLink: "",
        chooseMyFolderNodeID: "",
        category: ""
    };
    $("#activity-container").avtivityList(datas);
}

function StartActivityInboxByProjectAndSubProject(ProjectCode,ProjectName,SubprojectCode,SubProjectName) {
    $("#tab-panel-main").click();

    //$('.search-all-activity,.select-activity-type-filter,.select-activity-client-filter').val("");
    
    if (SubprojectCode != "" && SubprojectCode != "ALLINPROJECT") {
        ProjectName += " / " + SubProjectName;
    }

    $("#main-form-filter-header").html(ProjectName);
    $("#main-form-filter-code").html(ProjectCode);
    $("#main-form-filter-subproject-code").html(SubprojectCode);
    $("#txtHierarchyMyFolderSelectedNodeID").val("");
    StartActivityInbox();
}

function StartActivityInbox(newObjectLink) {
    //console.log("START_ACTIVITY_INBOX");
    var filter = $(".select-activity-client-filter").val();

    var specialFilter = $('#specialFilter').val();
    if (specialFilter != undefined && specialFilter != '') {
        filter = specialFilter;
    }
    var type = $(".select-activity-type-filter").val();
    var category = $(".select-activity-category-filter").val();
    var searchKey = $(".search-all-activity").val();
    var project = $("#main-form-filter-code").html();
    var subProject = $("#main-form-filter-subproject-code").html();

    var mainDelegateCode = $("#activity-box-main-delegate-code").html();
    var assetCode = $("#activity-box-asset-code").html();

    var chooseMyFolderNodeID = $("#txtHierarchyMyFolderSelectedNodeID").val();

    var datas = {
        filter: filter,
        type: type,
        searchKey: searchKey,
        projectCode: project,
        subProjectCode: subProject,
        mainDelegateCode: mainDelegateCode,
        assetCode: assetCode,
        isContinueLazyLoad: false,
        newObjectLink: newObjectLink,
        chooseMyFolderNodeID: chooseMyFolderNodeID,
        category: category
    };
    $("#activity-container").avtivityList(datas);
}

function ViewAllWorkGroup() {
    $("#main-form-filter-code").html("");
    $("#main-form-filter-subproject-code").html("");
    $("#main-form-filter-header").html("Inbox");
    $("#txtHierarchyMyFolderSelectedNodeID").val("");
    StartActivityInbox();
}

function ReStartActivityInbox() {
    $(".select-activity-client-filter").val("").change();
    
    $(".select-activity-type-filter").val("");
    $(".select-activity-category-filter").val("");
    $(".search-all-activity").val("");

    $("#main-form-filter-code").html("");
    $("#main-form-filter-subproject-code").html("");
    $("#main-form-filter-header").html("Inbox");

    $("#activity-box-main-delegate-code").html("");
    $("#activity-box-asset-code").html("");
    
    var filter = "";
    var specialFilter = $('#specialFilter').val();
    if (specialFilter != undefined && specialFilter != '') {
        filter = specialFilter;
    }

    var datas = {
        filter: filter,
        type: "",
        searchKey: "",
        projectCode: "",
        subProjectCode: "",
        mainDelegateCode: "",
        assetCode: "",
        isContinueLazyLoad: false,
        newObjectLink: "",
        chooseMyFolderNodeID: "",
        category: ""
    };
    $("#activity-container").avtivityList(datas);

    $("#txtHierarchyMyFolderSelectedNodeID").val("");

    $(".agape-show-node-name-all").addClass("focus-color");
    bindHierarchyAllProject();
    bindHierarchyMyFolder();
}

function StartActivityInboxLazyLoad() {
    if (!ActivityInboxLazyLoadFlag && ActivityInboxPhysicalTotalRow < ActivityInboxTotalRow) {
        ActivityInboxLazyLoadFlag = true;
        var filter = $(".select-activity-client-filter").val();
        var specialFilter = $('#specialFilter').val();
        if (specialFilter != undefined && specialFilter != '') {
            filter = specialFilter;
        }

        var type = $(".select-activity-type-filter").val();
        var category = $(".select-activity-category-filter").val();
        var searchKey = $(".search-all-activity").val();
        var project = $("#main-form-filter-code").html();
        var subProject = $("#main-form-filter-subproject-code").html();

        var mainDelegateCode = $("#activity-box-main-delegate-code").html();
        var assetCode = $("#activity-box-asset-code").html();

        var datas = {
            filter: filter,
            type: type,
            searchKey: searchKey,
            projectCode: project,
            subProjectCode: subProject,
            mainDelegateCode: mainDelegateCode,
            assetCode: assetCode,
            isContinueLazyLoad: true,
            newObjectLink: "",
            chooseMyFolderNodeID: "",
            category: category
        };
        $("#activity-container").avtivityList(datas);
    }
}

function getAllAobjectLink() {
    var arrAobj = [];
    $("tr.activity-selection").each(function () {
        var aobj = $(this).attr("data-row-aobjectlink");
        arrAobj.push(aobj);
    });

    return arrAobj.join(',');
}

$.fn.avtivityList = function (datasInput) {
    var filter = datasInput.filter == undefined ? "" : datasInput.filter;
    var type = datasInput.type == undefined ? "" : datasInput.type;
    var searchKey = datasInput.searchKey == undefined ? "" : datasInput.searchKey;
    var projectCode = datasInput.projectCode == undefined ? "" : datasInput.projectCode;
    var subProjectCode = datasInput.subProjectCode == undefined ? "" : datasInput.subProjectCode;
    var mainDelegateCode = datasInput.mainDelegateCode == undefined ? "" : datasInput.mainDelegateCode;
    var assetCode = datasInput.assetCode == undefined ? "" : datasInput.assetCode;
    var isContinueLazyLoad = datasInput.isContinueLazyLoad == undefined ? false : datasInput.isContinueLazyLoad;
    var newObjectLink = datasInput.newObjectLink == undefined ? "" : datasInput.newObjectLink;
    var chooseMyFolderNodeID = datasInput.chooseMyFolderNodeID == undefined ? "" : datasInput.chooseMyFolderNodeID;
    var category = datasInput.category == undefined ? "" : datasInput.category;

    var inboxMode = isInboxProject ? "inboxproject" : "inbox";


    var elt = $(this);
    if (!isContinueLazyLoad) {
        activityInboxLoading(true, "Loading inbox...");
    }
    else {
        activityInboxLoadingMore(true);
    }

    if (newObjectLink != undefined && typeof (newObjectLink) == "string" && newObjectLink != "") {
        AGLoading(true);
    }

    var orderBy = $(".select-activity-groupby").val();

    var postDatas = {
        filter: filter,
        type: type,
        searchKey: searchKey,
        projectCode: projectCode,
        subProjectCode: subProjectCode,
        mainDelegateCode: mainDelegateCode,
        assetCode: assetCode,
        allKeys: isContinueLazyLoad ? getAllAobjectLink() : "",
        myFolderCode: chooseMyFolderNodeID,
        category: category,
        leastDate: isContinueLazyLoad ? ActivityInboxLeastDate : "",
        orderBy: orderBy
    };

    $.ajax({
        type: "POST",
        url: servictWebDomainName + "TimeAttendance/TimeAttendance.ashx?q=" + inboxMode,
        data: postDatas,
        success: function (datas) {
            dataJsonExportToExcel = datas.ListActivity;

            activityInboxLoadingMore(false);

            if (ErrorAPIHandel(datas))
                return;

            ActivityInboxLeastDate = datas.LeastDate;
            var totalActs = datas.TotalActivity;
            datas = datas.ListActivity;
            ActivityInboxLazyLoadFlag = false;


            if (!isContinueLazyLoad) {
                ActivityInboxTotalRow = totalActs;
                elt.html("");
                elt.addClass("activity-container");
                elt.append(
                    '<tr style="height:1px;" class="tr-fake">' +
                        '<td class="task-color" style="height:1px;padding:0px;"></td>' +
                        '<td class="activity-image"></td>' +
                        '<td class="content" style="height:1px;padding:0px">&nbsp;</td>' +
                        '<td class="activity-leader" style="height:1px;padding:0px">&nbsp;</td>' +
                        '<td class="activity-xstatus">&nbsp;</td>' +
                        '<td class="activity-position"></td>' +
                        '<td class="activity-created-date" style="height:1px;padding:0px">&nbsp;</td>' +
                        '<td class="activity-date" style="height:1px;padding:0px">&nbsp;</td>' +
                        '<td class="activity-finish-date" style="height:1px;padding:0px">&nbsp;</td>' +
                        '<td class="activity-owner-group" style="height:1px;padding:0px">&nbsp;</td>' +
                        '<td class="activity-time" style="height:1px;padding:0px">&nbsp;</td>' +
                        '<td class="activity-project" style="height:1px;padding:0px">&nbsp;</td>' +
                        '<td class="activity-problem">&nbsp;</td>' +
                        '<td class="activity-subproject" style="height:1px;padding:0px">&nbsp;</td>' +
                        '<td class="activity-type" style="height:1px;padding:0px;display:none">&nbsp;</td>' +
                        '<td class="activity-priority">&nbsp;</td>' +
                        '<td class="activity-haste">&nbsp;</td>' +
                        '<td class="activity-percent"></td>' +
                        '<td class="activity-revenue">&nbsp;</td>' +
                        '<td class="activity-expense">&nbsp;</td>' +
                        '<td class="activity-revenue-actual">&nbsp;</td>' +
                        '<td class="activity-expense-actual">&nbsp;</td>' +
                        '<td class="activity-ticket">&nbsp;</td>' +
                        '<td class="activity-customer">&nbsp;</td>' +
                        '<td class="activity-contact">&nbsp;</td>' +
                        //'<td class="pad-d" style="height:1px;padding:0px"></td>' +
                        '<td class="color-picker" style="height:1px;padding:0px"></td>' +
                        '<td class="activity-status" style="display:none"></td>' +
                        '<td class="activity-initiative" style="display:none"></td>' +
                        
                    '</tr>'
                );
            }

           

            for (var i = 0; i < datas.length; i++) {
                var xRow = ActivityPrepareRow($(elt).attr('id'), datas[i], i);
                $(elt).append(xRow);
            }

            BindConfigEvenForActivity(elt);

            
            bindNanoScroller();
            groubByActivity(isContinueLazyLoad);

            $(".activity-container-empty").remove();
            if ($(".activity-selection").length == 0) {
                var divEmptyRow = $("<div/>",{
                    class: "activity-container-empty",
                    html:"<div style='text-align:center;color:red;padding:20px;background:#eee;'>ไม่มีรายการ</div>"
                });
                $("#activity-container").after(divEmptyRow);
            }
            $(".table-inbox-heading").toggle($(".activity-selection").length > 0);
            ActivityInboxPhysicalTotalRow = $(".activity-selection").length;
            var rowDesc = ActivityInboxPhysicalTotalRow + " of " + ActivityInboxTotalRow + " items";
            //var rowDesc = "Displayed " + ActivityInboxPhysicalTotalRow + " items";
            $(".activity-inbox-rows-desc").html(rowDesc);
            activityInboxLoading(false);

            //if (isInboxProject) {
            //    $(".activity-selection").each(function () {
            //        var content = $(this).find(".content span");
            //        var url = "/TimeAttendance/ActivityManagementReDesign.aspx";
            //        url += "?aobj=" + content.parent().parent().find(".task-color").attr("id").split(":")[0];
            //        url += "&snaid=" + content.parent().parent().find(".task-color").attr("id").split(":")[1];
            //        content.html("<a href='" + url + "' target='_blank'>" + content.html() + "</a>");
            //    });
            //}

            try {
                var _parent = window.parent;
                if (_parent != undefined) {
                    _parent.callBackLoadingSuccess();
                }
            } catch (e) { }

            //if (newObjectLink != undefined && typeof (newObjectLink) == "string" && newObjectLink != "") {
            //    $(".activity-selection.activity-" + newObjectLink + " .content").click();
            //}


        }
    });
}

function ActivityPrepareRow(containerID,data,index) {


    var createdDate = data.CREATED_ON.substring(6, 8) + '/' + data.CREATED_ON.substring(4, 6) + '/' + data.CREATED_ON.substring(0, 4);
    var dateOut = data.DATEOUT.substring(6, 8) + '/' + data.DATEOUT.substring(4, 6) + '/' + data.DATEOUT.substring(0, 4);
    var _taskStatusBox = "";
    var _taskPercent = "Excepting";
    var _barTaskPercent = '<td class="activity-percent">Excepting</td>';
    if (data.ItemTypeName == "Task" || data.ItemTypeName == "Invoice") {
        var _taskProgress = "<div class='pace pace-active'><div class='pace-progress' data-progress='" + data.TaskStatusPercent +
                            "' data-progress-text='" + data.TaskStatusPercent + "%' style='-webkit-transform: translate3d(" + data.TaskStatusPercent + "%, 0px, 0px);" +
                            " -ms-transform: translate3d(" + data.TaskStatusPercent + "%, 0px, 0px); transform: translate3d(" + data.TaskStatusPercent + "%, 0px, 0px);'>" +
                            " <div class='pace-progress-inner'></div></div><div class='pace-activity'></div></div>";
        _taskStatusBox = '<div style="zoom:0.5;">' + _taskProgress + '</div>' +
                            '<div style="zoom:0.5;position:absolute;margin-top:-23px;color:#2299DD">' + data.TaskStatusDesc + '</div>';
        _taskPercent = data.TaskStatusPercent + "%";
        _barTaskPercent =
            '<td class="activity-percent">' +
                '<div class="progress" style="margin-bottom:0" title="' + data.TaskStatusPercent + '% (' + data.TaskStatusDesc.replace('(', '').replace(')', '') + ')">' +
                    '<div class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width:' + data.TaskStatusPercent + '%;color:#333;"></div>' +
                    '<div class="progress-desc" style="padding: 4px;"><div class="one-line"><small style="font-size: 75%;">' + data.TaskStatusPercent + '% (' + data.TaskStatusDesc.replace('(', '').replace(')', '') + ')</small></div></div>' +
                '</div>' +
            '</td>';
    }

    var aStatus = data.AStatus == "Owner" ? "activity-owner" : (data.AStatus == "Require" ? "activity-require" : "");
    var xFilter = data.FilterClass == "markFilter" ? "activity-filter" : "";
    var clientFilter = data.ClientFilterClass;

    var activityClassArray = [];
    activityClassArray.push("activity-selection");
    activityClassArray.push("activity-" + data.AOBJECTLINK);
    activityClassArray.push(xFilter);
    activityClassArray.push(clientFilter);
    activityClassArray.push(aStatus);
    activityClassArray.push((data.PROJECTCODE == "" ? "notsetproject" : data.PROJECTCODE));
    activityClassArray.push("row-grouping-date-" + data.show_datetime.split(" ")[0].split("/").join(""));
    activityClassArray.push((data.isFavorite == 'True' ? 'activity-filter-state-favorite' : ''));
    activityClassArray.push((data.isReminder == 'True' ? 'activity-filter-state-reminder' : ''));
    activityClassArray.push((clientFilter.match('activity-client-filter-late-alarm') && data.ItemTypeName != "Note" && data.ItemType.toUpperCase() != "EMAIL" ? 'activity-filter-state-late' : ''));
    activityClassArray.push((parseInt(data.RecallCount) > 0 ? 'activity-filter-state-recall' : ''));


    var activityClass = activityClassArray.join(" ");

    var recallIcon = "";
    for (var rc = 0; rc < parseInt(data.RecallCount) ; rc++) {
        recallIcon += '<i class="fa fa-bullhorn activity-icon-alarm" style="margin-left:' + (rc == 0 ? '3px' : '5px') + '"></i>';
    }

    var strRevenue = data.Revenue < 0 ? "(" + data.Revenue + ")" : data.Revenue;
    var strCharges = data.Charges < 0 ? "(" + data.Charges + ")" : data.Charges;
    var strRevenueActual = data.RevenueActual < 0 ? "(" + data.RevenueActual + ")" : data.RevenueActual;
    var strChargesActual = data.ChargesActual < 0 ? "(" + data.ChargesActual + ")" : data.ChargesActual;

    var rowResult = $("<tr/>", {
        "data-row-aobjectlink": data.AOBJECTLINK,
        "data-row-snaid": data.COMPANYCODE,
        "data-row-index": index,
        "data-row-table-id": containerID,
        id: "activity-selection-" + index,
        title: data.JOBDESCRIPTION,
        class: activityClass
    });

    rowResult.append(
        '<td class="task-color" id="' + data.AOBJECTLINK + ":" + data.COMPANYCODE + '" style="background:' + data.TLID + ';background:none;"></td>' +

        '<td class="activity-image"><img onerror="DefaultUserImage(this);" src="' + data.profileImage + '" class="circular" /></td>' +
        '<td class="content ' + data.ActivityStatus + '" id="' +

                containerID + ":" +
                data.EMPCODE + "," +
                data.DATEIN + "," +
                data.SEQ + "," +
                data.COMPANYCODE +

        '">' +
            '<i class="fa ' + data.imgType + '" ></i><span' + (data.isReOpen == 'True' ? " class='reOpen-flash'" : "") + '>' + data.JOBDESCRIPTION + '</span>' +
        '</td>' +

        '<td class="activity-leader">' + data.Leader + '</td>' +
        '<td class="activity-xstatus" style="color: ' + data.ActivityOverviewStatusColor + ';">' + data.ActivityOverviewStatus + '</td>' +
        '<td class="activity-position">' + (data.APosition == "Owner" ? "เจ้าของงาน" : (data.APosition == "Main Delegate" ? "งานหลัก" : "งานแจ้งเพื่อทราบ")) + '</td>' +
        '<td class="activity-created-date">' + createdDate + '</td>' +
        '<td class="activity-date">' + data.show_datetime.split(' ')[0] + '</td>' +
        '<td class="activity-finish-date">' + dateOut + '</td>' +
        '<td class="activity-owner-group">' + data.Creator + '</td>' +
        '<td class="activity-time">' + data.show_datetime.split(' ')[1] + '</td>' +
        '<td class="activity-project">' + data.PROJECTNAME + '</td>' +
        '<td class="activity-subproject">' + data.PROJECTELEMENTNAME + '</td>' +
        '<td class="activity-problem">' + data.ProblemType + '</td>' +
        '<td class="activity-type" style="display:none">' + data.ItemTypeName + '</td>' +
        '<td class="activity-priority" style="color: ' + data.PriorityColor + ';">' + data.Priority + '</td>' +
        '<td class="activity-haste" style="color: ' + data.HasteColor + ';">' + data.Haste + '</td>' +
        //'<td class="activity-percent">' + _taskPercent + '</td>' +
        _barTaskPercent +

        '<td class="activity-revenue" style="text-align:right;color:blue;"><b>' + strRevenue + '</b></td>' +
        '<td class="activity-expense" style="text-align:right;color:red;"><b>' + strCharges + '</b></td>' +
        '<td class="activity-revenue-actual" style="text-align:right;color:blue;"><b>' + strRevenueActual + '</b></td>' +
        '<td class="activity-expense-actual" style="text-align:right;color:red;"><b>' + strChargesActual + '</b></td>' +
        '<td class="activity-ticket">' + data.TicketNO + '</td>' +
        '<td class="activity-customer">' + data.CustomerName + '</td>' +
        '<td class="activity-contact">' + data.ContactName + '</td>' +

        //'<td class="pad-d"><span style="display:none">' + data.show_content + '</span></td>' +
        '<td class="color-picker">' +
            '<i data-command="comment" class="activity-list-command-icon fa fa-comment"><span class="msg-require-read ' + CheckShow_MSG_RequireRead(data.MSG_RequireRead) + '">' + MSG_RequireReadMaxDisplay(data.MSG_RequireRead) + '</span></i>' +
            '<i data-command="favorite" class="activity-list-command-icon fa fa-star ' + (data.isFavorite == 'True' ? 'activity-favorite' : '') + '"></i>' +
            '<i data-command="reminder" class="activity-list-command-icon fa fa-bell ' + (data.isReminder == 'True' ? 'activity-reminder' : '') + '"></i>' +
            '<i data-command="myfolder" class="activity-list-command-icon fa fa-folder-open ' + (data.MyFolderCode != null && data.MyFolderCode != "" ? "is-in-my-folder" : "") + '"></i>' +
            '<i data-command="colorpicker" class="activity-list-command-icon fa fa-cog open-color-picker"></i>' +
        '</td>' +
        '<td class="activity-status" style="display:none">' + (data.AStatus == "Require" ? "ยังไม่ได้รับทราบ" : (data.AStatus == "Response" ? "รับทราบแล้ว" : (data.AStatus == "Owner" ? "เจ้าของงาน" : "เสร็จสิ้นแล้ว"))) + '</td>' +
        '<td class="activity-initiative" style="display:none"><b>' + data.initiative + '</b></td>'
    );

    return rowResult;
}

$.fn.bindDragAndDropEvent = function() {
    var row = $(this).hasClass(".activity-selection") ? $(this) : $(this).find(".activity-selection");

    row.attr("draggable",true);
    row.unbind("ondragend").bind("ondragend",function () {
        myFolder_RowDragEnd();
    });
    row.unbind("ondragstart").bind("ondragstart", function (e) {
        myFolder_RowDrag(e, this);
    });
}

function DefaultUserImage(obj) {
    $(obj).attr("src", "/images/user.png");
}

function activityInboxLoadingMore(flag) {
    $(".activity-inbox-loading-more").remove();
    if (flag) {
        $(".activity-container tbody").append($("<tr/>", {
            class: "activity-inbox-loading-more",
            html: "<td style='text-align:center;color:#000;padding:8px;background:#eee;' colspan='" + ($(".tr-fake td:visible").length) + "'>" +
                    "<img src='" + servictWebDomainName + "images/loadmessage.gif' style='width:30px;margin-right:10px'/><b><i>Loading more activities...</i></b>" +
                    "</td>"
        }));
    }
}

function activityInboxLoading(flag, msg) {
    if (msg != undefined) {
        $(".activity-inbox-loading-message").html(msg);
    }
    if (flag) {
        $(".activity-inbox-loading").show();
    }
    else {
        $(".activity-inbox-loading").fadeOut();
    }
}

function bindNanoScroller() {
    $('.nano').nanoScroller();
    //$(".nano").bind("scrollend", function (e) {
    //    if (!$(this).hasClass("workgroup-hierarchy-nano")) {
    //        if ($(e.currentTarget).find("#activity-container").length > 0) {
    //            StartActivityInboxLazyLoad();
    //        }
    //    }
    //});
    $("#nano-activity-list-table").unbind("scroll").bind("scroll", function () {
        var elem = $(this);
        if ((elem[0].scrollHeight - elem.scrollTop()) - 50 <= elem.outerHeight()) {
            StartActivityInboxLazyLoad();
        }
    });
}

function removeSearching() {
    $(".activity-search-result").hide();
    $(".activity-search-result-box").remove();
}

$.fn.searchActivity = function () {
    var searchKey = $(this).val().trim();

    $(".activity-search-result-loading-key").html(searchKey);

    var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx-'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });

    $(".activity-search-result,.activity-search-result-loading").show();
    $(".activity-search-result-box").remove();

    var resultBox = $("<div/>", {
        class: "activity-search-result-box",
        id: "search-" + guid
    });

    $(".activity-search-result").append(resultBox);
    
    setTimeout(function () {
        if ($("#search-" + guid).length > 0) {
            removeSearching();
            StartActivityInbox();
        }
    }, 3000);

}

function moreFilter() {
    var filterContainer = $("#more-filter-container")
    if ($(filterContainer).is(":visible")) {
        $(".more-filter-container-content").hide();
        $(filterContainer).animate({
            width: 0,
            height: 0,
            marginTop: 0,
            padding: 0
        }, 300, function () {
            $(filterContainer).hide();
        });
    }
    else {
        $(filterContainer).show();
        $(filterContainer).animate({
            width: 400,
            height: 400,
            marginTop: -8,
            padding: 20
        }, 300, function () {
            $(".more-filter-container-content").show();
        });
    }
}

function selectAllFilterActivity(obj) {
    $(".chk-filter-activity").prop("checked", obj.checked);
    //FilterActivity();
}

var isFirstState = true;

function LoadActivityNotificationCount() {
    //$.ajax({
    //    url: servictWebDomainName + "TimeAttendance/TimeAttendance.ashx?q=tasknoti",
    //    success: function (datas) {
    //        if (ErrorAPIHandel(datas))
    //            return;
    //        for (var i = 0; i < datas.length; i++) {
    //            if (datas[i].Key == "Reminder")
    //                $(".main-activity-noti-reminder").html(datas[i].Value);
    //            if (datas[i].Key == "Recall")
    //                $(".main-activity-noti-recall").html(datas[i].Value);
    //            if (datas[i].Key == "LateAll")
    //                $(".main-activity-noti-late").html(datas[i].Value);
    //            if (datas[i].Key == "Favorite")
    //                $(".main-activity-noti-favorite").html(datas[i].Value);
    //        }
            
    //    }
    //});
}

function FilterActivityState(state) {
    if (!isFirstState)
        return;
    isFirstState = false;

    var stateClass = "";
    if (state == "Late") {
        stateClass = "activity-filter-state-late";
    }
    if (state == "Favorite") {
        stateClass = "activity-filter-state-favorite";
    }
    if (state == "Reminder") {
        stateClass = "activity-filter-state-reminder";
    }
    if (state == "Recall") {
        stateClass = "activity-filter-state-recall";
    }

    $(".activity-container .activity-selection:visible").each(function () {
        if (!$(this).hasClass(stateClass))
            $(this).hide();
    });

    

    $(".activity-container-empty").remove();
    if ($(".activity-selection:visible").length == 0) {
        $(".activity-container tbody").append($("<tr/>", {
            class: "activity-container-empty",
            html: "<td style='text-align:center;color:red;padding:20px;background:#eee;' colspan='" + ($(".tr-fake td:visible").length) + "'>ไม่มีรายการ</td>"
        }));
    }
}

function FilterActivity() {
    var SelectedClientFilter = $(".select-activity-client-filter").val();

    if (SelectedClientFilter != "") {
        $(".activity-container .activity-selection").hide();
        $("." + SelectedClientFilter).show();
    }
    else {
        $(".activity-container .activity-selection").show();
    }

    $(".activity-container-empty").remove();
    if ($(".activity-selection:visible").length == 0) {
        $(".activity-container tbody").append($("<tr/>", {
            class: "activity-container-empty",
            html: "<td style='text-align:center;color:red;padding:20px;background:#eee;' colspan='" + ($(".tr-fake td:visible").length) + "'>ไม่มีรายการ</td>"
        }));
    }
}

function animateProjectPanel() {
    var icon = $("#animate-project-icon i");
    if ($(icon).hasClass("fa-arrow-circle-left")) {
        $(".animate-left-panel").animate({
            marginLeft: "-225px"
        }, function () {
            $("#animate-project-icon").css("right", "-18px");
            $(icon).removeClass("fa-arrow-circle-left").addClass("fa-arrow-circle-right");
        });
        $(".animate-activity-panel").switchClass("col-lg-10", "col-lg-12", 400);
    }
    else {
        $(".animate-left-panel").animate({
            marginLeft: "0"
        }, function () {
            $("#animate-project-icon").css("right", "-5px");
            $(icon).removeClass("fa-arrow-circle-right").addClass("fa-arrow-circle-left");
        });
        $(".animate-activity-panel").switchClass("col-lg-12", "col-lg-10", 400);
    }
}

var firstLoad = true;

// ================== Activity Cookie ========
function createCookieActivity(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookieActivity(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function eraseCookieActivity(name) {
    createCookieLinkOnlineHelp(name, "", -1);
}

var cookieName = "link-activity-opened";

function getCookieName() {
    var _linkid = $("#txtHiddenUserLinkID").val();
    var _snaid = $("#txtHiddenUserSNAID").val();

    var result_cookiename = cookieName + _linkid + _snaid;
    return result_cookiename;
}

function GetActivityCookie() {
    try{
        var cookieArray = [];
        

        var cookieValue = readCookieActivity(getCookieName());
        if (cookieValue != null) {
            cookieArray = $.parseJSON(cookieValue);
        }
        return cookieArray;
    }
    catch (e) {
        return [];
    }
}
function SwitchActivityCookie(oldAobj, newAobj,newRowKey) {
    var cookies = GetActivityCookie();
    var temp = null;
    for (var i = 0; i < cookies.length; i++) {
        if (cookies[i].aobjLink == oldAobj) {
            temp = cookies[i];
            break;
        }
    }
    onRemoveActivityLink(oldAobj);
    if (temp != null) {
        rowkey = newRowKey;
        aobjectLink = newAobj;
        snaid = temp.snaid;
        position = temp.position;
        activityName = temp.activityName;
        onClickActivityLink(rowkey, aobjectLink, snaid, position, activityName);
        var temp = AppendActivityLinkElement(rowkey, aobjectLink, snaid, position, activityName);
        temp.click();
    }
}
function RemoveActivityCookie(aobjLink) {
    var cookieArray = GetActivityCookie();
    var cookieNewArray = [];
    for (var i = 0; i < cookieArray.length; i++) {
        if (cookieArray[i].aobjLink != aobjLink) {
            cookieNewArray.push(cookieArray[i]);
        }
    }
    createCookieActivity(getCookieName(), JSON.stringify(cookieNewArray), 365);
    
}
function AddActivityCookie(rowKey, aobjLink, snaid, position, activityName) {
    var cookieArray = GetActivityCookie();
    cookieArray.push({
        rowKey: rowKey,
        aobjLink: aobjLink,
        snaid: snaid,
        position: position,
        activityName: activityName
    });
    createCookieActivity(getCookieName(), JSON.stringify(cookieArray), 365);
}
function AppendActivityLinkElement(rowKey, aobjLink, snaid, position, activityName) {
    $("[data-aobjectlink='" + aobjLink + "']").remove();
    var temp = $("#tab-panel-activity").clone();
    temp.addClass("tab-panel-activity-act");
    temp.attr("data-rowkey", rowKey);
    temp.attr("data-aobjectlink", aobjLink);
    temp.attr("data-snaid", snaid);
    temp.attr("data-position", position);
    temp.find(".tab-menu-activity-name").html(activityName);
    $("#tab-panel-main").after(temp);
    temp.show();
    temp.click(function () {
        $(".tab-panel-menu").removeClass("tab-panel-menu-active");
        $("#overflow-activity-container .activity-rows").removeClass("activity-rows-active");
        $(this).addClass("tab-panel-menu-active");
        activityClicked(
            $(this).attr("data-rowkey"),
            $(this).attr("data-aobjectlink"),
            $(this).attr("data-snaid"),
            $(this).attr("data-position")
        );
    });

    temp.find(".fa-remove").click(function (e) {
        if ($(this).parent().hasClass("tab-panel-menu-active")) {
            $("#tab-panel-main").click();
        }
        onRemoveActivityLink($(this).parent().attr("data-aobjectlink"));
    });

    CheckActivityLinkLength();

    return temp;
}

function CloseAllTab() {
    $(".tab-panel-activity-act").find(".fa-remover").click();
}
function CloseAllTabIcon() {
    $(".close-all-tab-icon").toggle($(".tab-panel-activity-act .fa-remover").length > 0);
}
function onClickActivityLink(rowKey, aobjLink, snaid, position, activityName) {
    RemoveActivityCookie(aobjLink);
    AddActivityCookie(rowKey, aobjLink, snaid, position, activityName);
}

function onRemoveActivityLink(aobjLink) {
    RemoveActivityCookie(aobjLink);
    $("[data-aobjectlink='" + aobjLink + "']").remove();
    CheckActivityLinkLength();
}
function startActivityLink() {
    var cookies = GetActivityCookie();
    for (var i = 0; i < cookies.length; i++) {
        AppendActivityLinkElement(cookies[i].rowKey, cookies[i].aobjLink, cookies[i].snaid, cookies[i].position, cookies[i].activityName);
    }
}

function CheckActivityLinkLength() {
    var overflowIcon = $("#overflow-activity-icon");
    var overflowContainer = $("#overflow-activity-container");
    overflowContainer.html("");
    overflowIcon.hide();

    var container = $(".tab-panel-activity-container");
    var containerWidth = container.width();
    var tabWidth = 0;
    container.find(".tab-panel-activity-act").each(function () {
        var actElt = $(this);
        tabWidth += actElt.width();
        tabWidth += 5;
        if (actElt.attr("data-aobjectlink") == undefined || actElt.attr("data-aobjectlink") == null || actElt.attr("data-aobjectlink") == "") {
            return;
        }

        if (tabWidth > containerWidth) {
            overflowIcon.show();

            var li = $("<li/>");
            var a = $("<a/>", {
                css: {
                    cursor:"pointer"
                },
                class: "activity-rows"
            });
            a.html(actElt.find(".tab-menu-activity-name").html());
            a.attr("data-rowkey", actElt.attr("data-rowkey"));
            a.attr("data-aobjectlink", actElt.attr("data-aobjectlink"));
            a.attr("data-snaid", actElt.attr("data-snaid"));
            a.attr("data-position", actElt.attr("data-position"));
            a.click(function () {
                $(".tab-panel-menu").removeClass("tab-panel-menu-active");
                $("#overflow-activity-container .activity-rows").removeClass("activity-rows-active");
                $(this).addClass("activity-rows-active");
                activityClicked(
                    $(this).attr("data-rowkey"),
                    $(this).attr("data-aobjectlink"),
                    $(this).attr("data-snaid"),
                    $(this).attr("data-position")
                );
            });

            li.append(a);
            overflowContainer.append(li);

            var removeBtn = $("<i/>", {
                class: "fa fa-remove pull-right",
                css: {
                    position: "absolute",
                    right: 10,
                    top: 13
                },
                click: function (e) {
                    if ($(this).parent().hasClass("activity-rows-active")) {
                        $("#tab-panel-main").click();
                    }
                    onRemoveActivityLink($(this).parent().attr("data-aobjectlink"));
                    e.stopPropagation();
                }
            });

            a.append(removeBtn);
        }
    });

    CloseAllTabIcon();
}

$.fn.activityClick = function () {
    var content = $(this);
    $(content).unbind("click").bind("click", function (e) {
        //console.log("ROW_CLICKED");
        if ($("#txtHiddenRowkey").length > 0) {
            $(".activity-selection").removeClass("activity-selection-focus");
            $(this).parent().addClass("activity-selection-focus");

            var aobjectLink = $(this).parent().find(".task-color").attr("id").split(":")[0];
            var rowkey = $(this).attr("id").split(":")[1];
            var snaid = $(this).parent().find(".task-color").attr("id").split(":")[1];
            var position = $(this).parent().find(".activity-status").html();
            var activityName = $(this).find("span").html();
            
            onClickActivityLink(rowkey, aobjectLink, snaid, position, activityName);
            var temp = AppendActivityLinkElement(rowkey, aobjectLink, snaid, position, activityName);
            temp.click();
            $(this).parent().find('.msg-require-read').addClass('hide');
        }
        e.stopPropagation();
    });
}

function activityClicked(rowKey, aobjLink, snaid, position) {
    $("#sortDesc").css("display", "");
    $("#sortAsc").css("display", "none");
    $("#divDetail").css("display", "none");
    loadActivityDetail(aobjLink, '', true);
    $("#txtHiddenRowkey").val(rowKey);
    $("#txtHiddenAObjectlink").val(aobjLink);
    $("#txtHiddenCompanyCode").val(snaid);
    $("#txtHiddenAP").val(position);
}

// ================ Column Config ==========

function columnChange() {
    var delegate = $("#chk-cog-delegate").prop("checked");
    var status = $("#chk-cog-status").prop("checked");
    var priority = $("#chk-cog-priority").prop("checked");
    var haste = $("#chk-cog-haste").prop("checked");
    var problem = $("#chk-cog-problem").prop("checked");
    var date = $("#chk-cog-date").prop("checked");
    var time = $("#chk-cog-time").prop("checked");
    var revenue = $("#chk-cog-revenue").prop("checked");
    var expense = $("#chk-cog-expense").prop("checked");
    var revenueActual = $("#chk-cog-revenue-actual").prop("checked");
    var expenseActual = $("#chk-cog-expense-actual").prop("checked");
    var ticket = $("#chk-cog-ticket").prop("checked");
    var createdDate = $("#chk-cog-created-date").prop("checked");
    var finishDate = $("#chk-cog-finish-date").prop("checked"); 
    var owner = $("#chk-cog-owner").prop("checked");
    var workGroup = $("#chk-cog-work-group").prop("checked");
    var progrtess = $("#chk-cog-Progrtess").prop("checked");
    var customer = $("#chk-cog-customer").prop("checked");
    var contact = $("#chk-cog-contact").prop("checked");
    var subproject = $("#chk-cog-subproject").prop("checked");

    $(".activity-leader").toggle(delegate);
    $(".activity-xstatus").toggle(status);
    $(".activity-priority").toggle(priority);
    $(".activity-haste").toggle(haste);
    $(".activity-problem").toggle(problem);
    $(".activity-date").toggle(date);
    $(".activity-time").toggle(time);
    $(".activity-revenue").toggle(revenue);
    $(".activity-expense").toggle(expense);
    $(".activity-revenue-actual").toggle(revenueActual);
    $(".activity-expense-actual").toggle(expenseActual);
    $(".activity-ticket").toggle(ticket);
    $(".activity-created-date").toggle(createdDate);
    $(".activity-finish-date").toggle(finishDate);
    $(".activity-owner-group").toggle(owner);
    $(".activity-project").toggle(workGroup);
    $(".activity-percent").toggle(progrtess);
    $(".activity-customer").toggle(customer);
    $(".activity-contact").toggle(contact);
    $(".activity-subproject").toggle(subproject);

    var groupingColSpan = $(".tr-fake td:visible").length;
    $(".column-grouping").attr("colspan", groupingColSpan);


    var cogs = {
        delegate:delegate,
        status:status,
        priority:priority,
        haste:haste,
        problem: problem,
        date: date,
        time: time,
        revenue: revenue,
        expense: expense,
        revenueActual: revenueActual,
        expenseActual: expenseActual,
        ticket: ticket,
        createdDate: createdDate,
        finishDate: finishDate,
        owner: owner,
        workGroup: workGroup,
        progrtess: progrtess,
        customer: customer,
        contact: contact,
        subproject: subproject
    };
    createCookieActivity("link-activity-column-config",JSON.stringify(cogs),365);
}

function columnSet() {
    var cookieCog = readCookieActivity("link-activity-column-config");
    if (cookieCog != null) {
        var cog = $.parseJSON(cookieCog);
        $("#chk-cog-delegate").prop("checked", cog.delegate);
        $("#chk-cog-status").prop("checked", cog.status);
        $("#chk-cog-priority").prop("checked", cog.priority);
        $("#chk-cog-haste").prop("checked", cog.haste);
        $("#chk-cog-problem").prop("checked", cog.problem);
        $("#chk-cog-date").prop("checked", cog.date);
        $("#chk-cog-time").prop("checked", cog.time);
        $("#chk-cog-revenue").prop("checked", cog.revenue == undefined ? false : cog.revenue);
        $("#chk-cog-expense").prop("checked", cog.expense == undefined ? false : cog.expense);
        $("#chk-cog-revenue-actual").prop("checked", cog.revenueActual == undefined ? false : cog.revenueActual);
        $("#chk-cog-expense-actual").prop("checked", cog.expenseActual == undefined ? false : cog.expenseActual);
        $("#chk-cog-ticket").prop("checked", cog.ticket);
        columnChange();
    }
}


// ================ Group Sort ============

function sortResult(datas, type, sort) {

    datas = datas.sort(function (a, b) {
        var valueA = a.value == undefined ? "" : a.value;
        var valueB = b.value == undefined ? "" : b.value;
        if (type == "STARTDATE" || type == "CREATEDDATE" || type == "FINISHDATE") {
            var strA = valueA.split('/');
            var strB = valueB.split('/');
            var valueA = parseInt(strA[2] + "" + strA[1] + "" + strA[0]);
            var valueB = parseInt(strB[2] + "" + strB[1] + "" + strB[0]);
        }

        if (type == "PERCENT") {
            var strA = valueA == "Excepting" ? -1 : valueA.split("%").join("");
            var strB = valueB == "Excepting" ? -1 : valueB.split("%").join("");
            var valueA = parseInt(strA);
            var valueB = parseInt(strB);
        }


        if (sort == "MAX") {
            if (valueA > valueB) {
                return 1;
            }
            if (valueA < valueB) {
                return -1;
            }
            return 0;
        }
        else {
            if (valueA < valueB) {
                return 1;
            }
            if (valueA > valueB) {
                return -1;
            }
            return 0;
        }
    });
    return datas;
}

function groupSort(datas, type, isContinueLazyLoad) {
    var sort = isContinueLazyLoad ? "MIN" : "MAX";
    var colLength = $(".tr-fake td:visible").length;
    //datas = sortResult(datas, type, sort);
    for (var i = 0; i < datas.length; i++) {
        var groupObj = datas[i];

        var groupVal = (groupObj.value == null || groupObj.value == undefined || groupObj.value.trim() == "") ? "ไม่ระบุ" : groupObj.value;

        var groupShown = $(".row-grouping[data-group-value='" + groupObj.value + "']").length > 0;

        var groupRow = $("<tr/>", {
            "data-group": groupObj.key,
            "data-group-value": groupVal,
            id: groupObj.key,
            class: "row-grouping row-grouping-date " + (groupShown ? "hide" : ""),
            html: "<td class='column-grouping' colspan='" + (colLength) + "' style='padding:5px;padding-left:33px;'>" + groupVal + "</td>"
        });

        if (!isContinueLazyLoad && i == 0) {
            $(".tr-fake").after(groupRow);
        } else {
            $("#activity-container").append(groupRow);
        }

        var getDataRows = $("[row-grouped='false'][data-group='" + datas[i].key + "']");
        groupRow.after(getDataRows);
        getDataRows.attr("row-grouped", "true");
    }
}

// ================ Group By ==============

function groubByActivity(isContinueLazyLoad) {
    if (isContinueLazyLoad == undefined || !isContinueLazyLoad) {
        clearGroupByActivity();
    }
    if ($(".activity-selection").length > 0) {
        var type = $(".select-activity-groupby").val();
        var specialGroupBy = $('#specialGroupBy').val();
        if (specialGroupBy != undefined && specialGroupBy != '') {
            type = specialGroupBy;
        }
        var arrGroup = [];
        if (type == "STATUS") {
            groupByStatus(arrGroup);
        }
        if (type == "PRIORITY") {
            groupByPriority(arrGroup);
        }
        if (type == "HASTE") {
            groupByHaste(arrGroup);
        }
        if (type == "PROBLEM") {
            groupByProblem(arrGroup);
        }
        if (type == "STARTDATE") {
            groupByDate(arrGroup);
        }
        if (type == "DATETIME") {
            groupByDateTime(arrGroup);
        }
        if (type == "PERCENT") {
            groupByPercent(arrGroup);
        }
        if (type == 'PROJECT') {
            if ($("#main-form-filter-code").html() == "") {
                groupByProject(arrGroup);
            }
            else {
                groupBySubProject(arrGroup);
            }
        }
        if (type == 'CREATEDDATE') {
            groupByCreatedDate(arrGroup);
        }
        if (type == 'FINISHDATE') {
            groupByFinishDate(arrGroup);
        }
        if (type == 'OWNER') {
            groupByOwner(arrGroup);
        }
        if (type == 'RESPONSIBLEPERSON') {
            groupByResponsiblePerson(arrGroup);
        }
        if (type == 'INITIATIVE') {
            groupInitiative(arrGroup);
        }

        
        console.log(arrGroup);
        columnSet();
        groupSort(arrGroup, type, isContinueLazyLoad);
        //lastStepGroupBy(arrGroup);
        arrGroup = null;
    }
}

function isNotGroupByActivity() {
    return $(".row-grouping").length == 0;
}

function clearGroupByActivity() {
    $(".row-grouping").remove();
    $("[row-grouped]").removeAttr("row-grouped");
    $("#nano-activity-list-table").scrollTop(0);
}

function checkArrGroupGUID(arr, val) {
    var group = {};
    for (var i = 0; i < arr.length; i++) {
        if (arr[i].value == val) {
            return {
                hasGroup : true,
                object: arr[i]
            }
        }
    }
    var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx-'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });

    return {
        hasGroup: false,
        object: {
            key: guid + "group",
            value: val
        }
    };
}

$.fn.addRowGrouping = function (index, arr, val) {
    var row = $(this);
    var rGrouped = row.attr("row-grouped");
    if (rGrouped != undefined || val == undefined) {
        return arr;
    }
    
    if (!row.hasClass("tr-fake")) {
        var groupObj = checkArrGroupGUID(arr, val);
        if (!groupObj.hasGroup) {
            //    row.before("<tr data-group='" + groupObj.object.key + "' id='" + groupObj.object.key
            //        + "' class='row-grouping row-grouping-date'><td class='column-grouping' colspan='" + ($(".tr-fake td:visible").length) + "' style='padding:5px;padding-left:33px;'>" + val + "</td></tr>");
            arr.push(groupObj.object);
        }
        //else {
        //    $("#" + groupObj.object.key).after(row);
        //}
        row.attr("data-group", groupObj.object.key);
        row.attr("row-grouped", "false");
    }
    return arr;
}

// ============== Group By Date ================

function lastStepGroupBy(arrGroup) {
    //console.log("arrGroup", arrGroup);
}

function groupByStatus(arr) {
    $("#activity-container tr").each(function (index) {
        try {
            var val = $(this).find(".activity-xstatus").html();
            arr = $(this).addRowGrouping(index, arr, val);
        }
        catch (e) { }
    });
    return arr;
}

function groupByPriority(arr) {
    $("#activity-container tr").each(function (index) {
        try {
            var val = $(this).find(".activity-priority").html();
            arr = $(this).addRowGrouping(index, arr, val);
        }
        catch (e) { }
    });
    return arr;
}

function groupByHaste(arr) {
    $("#activity-container tr").each(function (index) {
        try {
            var val = $(this).find(".activity-haste").html();
            arr = $(this).addRowGrouping(index, arr, val);
        }
        catch (e) { }
    });
    return arr;
}

function groupByProblem(arr) {
    $("#activity-container tr").each(function (index) {
        try {
            var val = $(this).find(".activity-problem").html();
            arr = $(this).addRowGrouping(index, arr, val);
        }
        catch (e) { }
    });
    return arr;
}

function groupByDate(arr) {
    $("#activity-container tr").each(function (index) {
        try {
            var val = $(this).find(".activity-date").html();
            arr = $(this).addRowGrouping(index, arr, val);
        }
        catch (e) { }
    });
    return arr;
}

function groupByPercent(arr) {
    $("#activity-container tr").each(function (index) {
        try {
            var val = $(this).find(".activity-percent").html();
            arr = $(this).addRowGrouping(index, arr, val);
        }
        catch (e) { }
    });
    return arr;
}

function groupByDateTime(arr) {
    $("#activity-container tr").each(function (index) {
        try {
            var date = $(this).find(".activity-date").html();
            var val = $(this).find(".activity-time").html();
            val = date + " " + val;
            arr = $(this).addRowGrouping(index, arr, val);
        }
        catch (e) { }
    });
    return arr;
}

function groupByProject(arr) {
    $("#activity-container tr").each(function (index) {
        try {
            var val = $(this).find(".activity-project").html();
            arr = $(this).addRowGrouping(index, arr, val);
        }
        catch (e) { }
    });
    return arr;
}

function groupBySubProject(arr) {
    $("#activity-container tr").each(function (index) {
        try {
            var project = $(this).find(".activity-project").html();
            var val = $(this).find(".activity-subproject").html();
            if (val == "") {
                val = "ไม่ระบุขั้นตอน";
            }
            val = project + " / " + val;
            arr = $(this).addRowGrouping(index, arr, val);
        }
        catch (e) { }
    });
    return arr;
}

function groupByCreatedDate(arr) {
    $("#activity-container tr").each(function (index) {
        try {
            var val = $(this).find(".activity-created-date").html();
            arr = $(this).addRowGrouping(index, arr, val);
        }
        catch (e) { }
    });
    return arr;
}

function groupByFinishDate(arr) {
    $("#activity-container tr").each(function (index) {
        try {
            var val = $(this).find(".activity-finish-date").html();
            arr = $(this).addRowGrouping(index, arr, val);
        }
        catch (e) { }
    });
    return arr;
}

function groupByOwner(arr) {
    $("#activity-container tr").each(function (index) {
        try {
            var val = $(this).find(".activity-owner-group").html();
            arr = $(this).addRowGrouping(index, arr, val);
        }
        catch (e) { }
    });
    return arr;
}

function groupByResponsiblePerson(arr) {
    $("#activity-container tr").each(function (index) {
        try {
            var val = $(this).find(".activity-leader").html();
            arr = $(this).addRowGrouping(index, arr, val);
        }
        catch (e) { }
    });
    return arr;
}

function groupInitiative(arr) {
    $("#activity-container tr").each(function (index) {
        try {
            var val = $(this).find(".activity-initiative").html();
            arr = $(this).addRowGrouping(index, arr, val);
        }
        catch (e) { }
    });
    return arr;
}

// ============== End Group By ================

var ActivityIconAnimateTimedOut = null;
function StartActivityIconAnimate() {
    if (ActivityIconAnimateTimedOut != null) {
        clearTimeout(ActivityIconAnimateTimedOut);
    }

    ActivityIconAnimate(true);
}
function ActivityIconAnimate(flag) {
    ActivityIconAnimateTimedOut = setTimeout(function () {
        $(".activity-icon-alarm").css("color", (flag ? "red" : "orange"));
        ActivityIconAnimate(!flag);
    }, 800);
}

function hasChildsArray(arr, val) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] == val)
            return true;
    }
    return false;
}

$.fn.modalActivityDetailRemarkComment = function (aobj, subject, img) {
    var container = $(this);
    container.modal("show");
    container.find(".modal-title").html("").append(img,subject);

    var modalBody = container.find(".modal-body");
    modalBody.html("");

    var temp = container.find(".system-message-comment-container").clone();
    modalBody.append(temp);
    temp.show();

    loadActivityDetailRemarkComment(aobj, temp);
}

function BindConfigEvenForActivity(elt) {

    $(elt).bindDragAndDropEvent();

    //============== Select activity click (click for show Activity detail)
    elt.find(".content").activityClick();

    elt.find(".activity-selection").unbind("click").bind("click", function () {
        $(this).find(".content").click();
    });

    elt.find(".color-picker").unbind("click").bind("click", function (e) {
        e.stopPropagation();
    });

    $("html").click(function (e) {
        $(".color-picker-panel").remove();
        //bindNanoScroller();
    });

    elt.find(".color-picker [data-command='myfolder']").unbind("click").bind("click", function (e) {
        var row = $(this).closest("tr");
        var key = row.attr("data-row-aobjectlink");
        var subject = row.find(".content span").html();
        var img = row.find(".content img").clone();
        OpenActivityMoveToMyFolder(key,subject,img);
    });

    elt.find(".color-picker [data-command='comment']").unbind("click").bind("click", function (e) {
        var row = $(this).closest("tr");
        var key = row.attr("data-row-aobjectlink");
        var subject = row.find(".content span").html();
        var img = row.find(".content img").clone();

        $("#modalRemark").modalActivityDetailRemarkComment(key, subject, img);
    });

    elt.find(".color-picker [data-command='favorite']").unbind("click").bind("click",function (e) {
        var row = $(this).closest("tr");
        var key = row.attr("data-row-aobjectlink");
        var snaid = row.attr("data-row-snaid");
        var flag = !$(this).hasClass("activity-favorite");
        
        if ($(this).hasClass("activity-favorite")) {
            $(this).parent().parent().removeClass("activity-filter-state-favorite");
            $(this).removeClass("activity-favorite");
        }
        else {
            $(this).parent().parent().addClass("activity-filter-state-favorite");
            $(this).addClass("activity-favorite");
        }

        SaveActivityFavorite(key, snaid, flag);
        
        e.stopPropagation();
    });

    elt.find(".color-picker [data-command='reminder']").unbind("click").bind("click",function (e) {
        var row = $(this).closest("tr");
        var key = row.attr("data-row-aobjectlink");
        var snaid = row.attr("data-row-snaid");
        var flag = !$(this).hasClass("activity-reminder");

        if ($(this).hasClass("activity-reminder")) {
            $(this).parent().parent().removeClass("activity-filter-state-reminder");
            $(this).removeClass("activity-reminder");
        }
        else {
            $(this).parent().parent().addClass("activity-filter-state-reminder");
            $(this).addClass("activity-reminder");
        }

        $.ajax({
            url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=activityreminder&key=' + key + '&snaid=' + snaid + '&flag=' + flag,
            success: function (datas) {
                if (ErrorAPIHandel(datas))
                    return;

                LoadActivityNotificationCount();
            }
        });
        e.stopPropagation();
    });

    elt.find(".color-picker [data-command='colorpicker']").unbind("click").bind("click",function (e) {
        $(".color-picker-panel").remove();

        var colorPicker = $('<div/>', {
            class: "color-picker-panel",
            id: "color-picker-panel-container",
            click: function (ev) {
                ev.stopPropagation();
            }

        }).append("<span class='color-picker-panel'>Color picker</span><hr style='margin-top:10px;margin-bottom:20px'/>");

        var ColorBox = $("<div/>", {
            class: "color-picker-panel",
            id: "color-picker-panel-color-box"
        });

        $(colorPicker).append(ColorBox);

        var colorList = ["none", "#B68542", "#E59B32", "#E6C832", "#AAC536", "#AEDA45", "#4ACCE6", "#49A5E6", "#5078F1", "#A15AE8", "#ED78E0", "#E7546E"];
        var colorListBox;
        var count = 0;
        for (var i = 0; i < colorList.length; i++) {

            if (count == 0 || count % 6 == 0) {
                colorListBox = $("<p/>", {
                    class: "color-picker-panel color-picker-panel-list"
                });
                $(ColorBox).append(colorListBox);
            }
            count++;

            var color = $('<span/>', {
                css: {
                    background: colorList[i],
                    padding: 5,
                    paddingLeft: 10,
                    paddingRight: 10,
                    textDecoration: "none",
                    outline: "1px solid #333"
                },
                html: "&nbsp;",
                class: "color-picker-panel color-choise"
            }).click(function () {
                var color = $(this).css("background-color");
                var row = $(this).closest("tr");
                row.find(".task-color").css({
                    background: color
                });
                var key = row.attr("data-row-aobjectlink");
                var snaid = row.attr("data-row-snaid");
                $.ajax({
                    url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=activitycolor&key=' + key + '&color=' + color + '&SNAID=' + snaid,
                    success: function (datas) {
                        if (ErrorAPIHandel(datas))
                            return;
                    }
                });
                $(this).parent().parent().parent().remove();
            });

            $(colorListBox).append(color);
        }
        $(this).after(colorPicker);
        $(colorPicker).hide().fadeIn();
        e.stopPropagation();
    });


    StartActivityIconAnimate();
}

function ActivityMoveToMyFolderInnerClick() {
    var key = $("#txtHiddenAObjectlink").val();
    var subject = $("#txtHiddenCompanyCode").val();
    OpenActivityMoveToMyFolder(key, subject, "");
}

function SaveActivityFavorite(key, snaid, flag) {
    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=activityfavorite&key=' + key + '&snaid=' + snaid + '&flag=' + flag,
        success: function (datas) {
            if (ErrorAPIHandel(datas))
                return;

            LoadActivityNotificationCount();
        }
    });
}

function ActivityFavoriteInnerClick(obj) {
    $("#detail_star").toggleClass("activity-favorite");
    var flag = $("#detail_star").hasClass("activity-favorite");
    var key = $("#txtHiddenAObjectlink").val();
    var snaid = $("#txtHiddenCompanyCode").val();
    var starInRow = $(".activity-" + key).find(".color-picker [data-command='favorite']");
    starInRow.removeClass("activity-favorite");
    if (flag) {
        starInRow.addClass("activity-favorite");
    }
    SaveActivityFavorite(
        key,
        snaid,
        flag
    );
}

function CallbackActivityEvent(event, newObjectLink) {
    //StartActivityInbox(newObjectLink);
    //refreshAllNotification();

    var aobj = $("#txtHiddenAObjectlink").val();
    var loadAobj = aobj;
    var isNew = false;
    if (newObjectLink != undefined && newObjectLink != "") {
        loadAobj = newObjectLink;
        isNew = true;
    }

    LoadAndReplaceActivityRow(loadAobj, aobj, isNew);

    if (event == "checkout" || event == "canceled" || event == "reopen" || event == "notetotask") {
        bindHierarchyAllProject();
        bindHierarchyMyFolder();
    }

}

function LoadAndReplaceActivityRow(loadAobj,oldRowAobj,isNew) {
    $.ajax({
        type: "POST",
        url: servictWebDomainName + "TimeAttendance/TimeAttendance.ashx?q=inboxrow",
        data: {
            aobj: loadAobj
        },
        success: function (datas) {


            if (ErrorAPIHandel(datas))
                return;

            datas = datas.ListActivity;

            //console.log("NEW_ROW", datas);

            var oldRow = $(".activity-selection[data-row-aobjectlink='" + oldRowAobj + "']");

            var newRow = ActivityPrepareRow(oldRow.attr('data-row-table-id'), datas[0], parseInt(oldRow.attr('data-row-index')));
            oldRow.after(newRow);
            oldRow.remove();
            BindConfigEvenForActivity(newRow.closest(".activity-container"));
            columnChange();

            if (isNew) {
                $(".activity-selection.activity-" + loadAobj + " .content").click();
            }
        }
    });
}

function dateformat(_in) {
    _rs = _in;
    if (_in.length == 8) {
        _rs = _in.substring(6, 9) + '/' + _in.substring(4, 6) + '/' + _in.substring(0, 4);
    }
    return _rs;
}

function timeformat(_in) {
    _rs = _in;
    if (_in.length == 6) {
        _rs = _in.substring(0, 2) + ':' + _in.substring(2, 4) + ':' + _in.substring(4, 6);
    }
    return _rs;
}

function confirmEmailRequireResponse(msg, fnc, obj) {

    var modalConfirm =
    '<div id="confirmModalRequireResponse" class="popup-dialog" style="z-index:50000;position:fixed;width:100%;height:100%;background:rgba(0, 0, 0, 0.5);top:0px;left:0px;display:none">' +
        '<div style="width: 530px;margin:auto;background:#fff;padding:20px;margin-top:150px;border:1px solid #ccc;border-radius:5px">' +
            '<div>' +
                '<div style="border-bottom:1px solid #ccc;padding-bottom:5px;">' +
                    '<span class="modal-title" style="text-align: left;" id="confirmModalRequireResponseMsg"></span>' +
                '</div>' +

                '<table style="margin-top:10px;vertical-align:middle" id="tabRequireResponse">' +
                    '<tr>' +
                        '<td style="width:20px;" valign="middle"><input type="checkbox" id="chkRequireResponse" checked="true"/></td>' +
                        '<td valign="middle" style="padding-top:8px"><label for="chkRequireResponse"> ระบุว่าคุณได้รับทราบกิจกรรมนี้แล้ว (ขณะนี้ยังไม่ได้ระบุว่ารับทราบ)</label></td>' +
                    '</tr>' +
                '</table>' +
                
                '<div style="text-align:center;padding-top:20px">' +
                    '<input type="button" name="name" id="btnconfirmModalRequireResponseSendEmail" value="ยืนยันและส่งอีเมล" class="btn btn-primary" style="margin-right:10px;width:150px"/>' +
                    '<input type="button" name="name" id="btnconfirmModalRequireResponseNoSendEmail" value="ยืนยัน" class="btn btn-success" style="margin-right:10px;width:150px"/>' +
                    '<input type="button" name="name" id="btnconfirmModalRequireResponseCancel" value="ยกเลิก" class="btn btn-danger" style="width:150px"/>' +
                '</div>' +
            '</div>' +
        '</div>' +
    '</div> ';

    $('body').append(modalConfirm);

    var sendResponse = false;
    if ($("#txtHiddenAP").val() == "ยังไม่ได้รับทราบ") {
        $("#tabRequireResponse").show();
        sendResponse = true;
    }
    else {
        $("#tabRequireResponse").hide();
    }

    $('#confirmModalRequireResponse').fadeIn();
    $('#confirmModalRequireResponse').find('#confirmModalRequireResponseMsg').html(msg);
    $('#confirmModalRequireResponse').find('#btnconfirmModalRequireResponseSendEmail').click(function () {
        readedClickRequireResponse(true, obj, fnc);
        closeModalRequireResponse();
        AGLoading(true);
    });
    $('#confirmModalRequireResponse').find('#btnconfirmModalRequireResponseNoSendEmail').click(function () {
        readedClickRequireResponse(false, obj, fnc);
        closeModalRequireResponse();
        AGLoading(true);
    });
    $('#confirmModalRequireResponse').find('#btnconfirmModalRequireResponseCancel').click(function () {
        closeModalRequireResponse();
    });
    function closeModalRequireResponse() {
        $('#confirmModalRequireResponse').fadeOut(function () {
            $('#confirmModalRequireResponse').remove();
        });
    }
    function readedClickRequireResponse(isSendMailEvent, objEvent, fncEvent) {
        if ($("#chkRequireResponse").prop("checked") && sendResponse) {
            if ($("#txtHiddenYouAre").val() == "MD") {
                $.ajax({
                    url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=eventleader&key=' + $("#txtHiddenAObjectlink").val() + '&val=READED' + '&rowkey=' + $("#txtHiddenRowkey").val() + '&SNAID=' + $("#txtHiddenCompanyCode").val() + '&sendmail=' + isSendMailEvent+'&prkey='+$("#txtHiddenkeyDataPR").val(),
                    success: function (data) {
                        if (ErrorAPIHandel(data))
                            return;
                        $("#txtHiddenAP").val("รับทราบแล้ว");
                        CallbackActivityEvent("readed");
                        fncEvent(isSendMailEvent, objEvent);
                    }
                });
            }
            if ($("#txtHiddenYouAre").val() == "OD") {
                $.ajax({
                    url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=eventother&key=' + $("#txtHiddenAObjectlink").val() + '&val=READED' + '&rowkey=' + $("#txtHiddenRowkey").val() + '&SNAID=' + $("#txtHiddenCompanyCode").val() + '&sendmail=' + isSendMailEvent + '&prkey=' + $("#txtHiddenkeyDataPR").val(),
                    success: function (data) {
                        if (ErrorAPIHandel(data))
                            return;
                        $("#txtHiddenAP").val("รับทราบแล้ว");
                        CallbackActivityEvent("readed");
                        fncEvent(isSendMailEvent, objEvent);
                    }
                });
            }
        }
        else {
            fncEvent(isSendMailEvent, objEvent);
        }
    }
}

function confirmEmail(msg, fnc, obj, isRequireRemark,remarkEventCode) {

    $('body').append(
        '<div id="confirmModal" class="popup-dialog" style="overflow: auto;z-index:50000;position:fixed;width:100%;height:100%;background:rgba(0, 0, 0, 0.7);top:0px;left:0px;display:none">' +
            '<div style="width: 530px;margin:auto;background:#fff;padding:20px;margin-top:150px;border:1px solid #ccc;border-radius:5px">' +
                '<div>' +

                    '<div style="border-bottom:1px solid #ccc;padding-bottom:5px;">' +
                        '<span class="modal-title" style="text-align: left;" id="confirmModalMsg"></span>' +
                    '</div>' +

                    '<div id="confirmModalRemarkBox" style="padding-bottom:5px;padding-top:5px;margin-top:5px;display:none">' +
                        '<label>หมายเหตุ</label>' +
                        '<textarea class="form-control" style="resize:none;height:150px" id="confirmModalRemark" placeholder="กรุณาระบุหมายเหตุ"></textarea>' +
                    '</div>' +

                    '<div id="confirmModalSaleMode" style="padding-bottom: 5px; padding-top: 5px; margin-top: 5px;display:none">' +
                        '<label>ความสำเร็จของกิจกรรม : </label>' +
                        ' <input type="radio" style="font-weight:400;margin:0 5px;" id="customerRadio1" name="customerRadio" class="customerRadio">' +
                        '<label for="customerRadio1" class="text-success">Win</label>' +
                        ' <input type="radio" style="font-weight:400;margin:0 5px;" id="customerRadio2" name="customerRadio" class="customerRadio">' +
                        '<label for="customerRadio2" class="text-danger">Lost</label>' +
                    '</div>' +

                    '<div class="row" style="margin-top:7px;margin-bottom:13px">' +
                        '<div class="col-xs-4">'+
                            '<input type="button" name="name" id="btnConfirmModalNoSendEmail" value="ยืนยัน" class="btn btn-success btn-block"/>' +
                        '</div>' +
                        '<div class="col-xs-4">' +
                            '<input type="button" name="name" id="btnConfirmModalSendEmail" value="ยืนยันและส่งอีเมล" class="btn btn-primary btn-block"/>' +
                        '</div>' +
                        '<div class="col-xs-4">' +
                            '<input type="button" name="name" id="btnConfirmModalCancel" value="ยกเลิก" class="btn btn-danger btn-block"/>' +
                        '</div>' +
                    '</div>' +

                    '<div style="border-top:1px solid #ccc;padding-top:5px;">' +
                        '<p style="margin-bottom: 5px;" class="text-danger">กรณีที่ต้องการส่งอีเมลไปยังที่อยู่เหล่านี้ ให้คลิกปุ่ม <b>"ยืนยันและส่งอีเมล"</b></p>' +
                        '<small style="display: block;">' +
                            '<div style="display: inline-block;vertical-align: top;">' +
                                'Email To : ' +
                            '</div>' +
                            '<div style="display: inline-block;" id="lblConfirmModalEmailTo">' +
                            '</div>' +
                        '</small>' +
                    '</div>' +

                '</div>' +
            '</div>' +
        '</div> '
    );
    $('#confirmModal').fadeIn();
    $('#confirmModal').find('#confirmModalMsg').html(msg);

    if (isRequireRemark != undefined && isRequireRemark) {
        $("#confirmModalRemarkBox").show();
    }

    if ($("#handleActivityDetailCustomer").is(":visible") && fnc.toString().match("checkOutClick")) {
        $("#confirmModalSaleMode").show();
    }

    $('#confirmModal').find('#btnConfirmModalSendEmail').unbind("click").bind("click", {
        remarkEventCode: remarkEventCode
    },function (e) {

        if ($("#confirmModalSaleMode").is(":visible") && $("#confirmModalSaleMode input:checked").length == 0) {
            alert("กรุณาระบุความสำเร็จของกิจกรรม");
            $("#confirmModalSaleMode input:first").focus();
            return;
        }

        if (isRequireRemark != undefined && isRequireRemark) {
            if ($("#confirmModalRemark").val().trim() == "") {
                alert("กรุณาระบุหมายเหตุ");
                $("#confirmModalRemark").focus();
                return;
            }
            else {
                var SaleModeResult = "";
                if ($("#confirmModalSaleMode").is(":visible")) {
                    SaleModeResult = $("#confirmModalSaleMode #customerRadio1").prop("checked") ? "WIN" : "LOST";
                }
                SendRemarkBeforeDoEvent(fnc, true, obj, "confirmModal", SaleModeResult, e.data.remarkEventCode);

                var n = $('#activity-content-box-container').height();
                $('#activity-content-box-container').animate({ scrollTop: n }, 1000);
            }
        }
        else {
            fnc(true, obj);
            closeModal();
        }
        AGLoading(true);
    });
    $('#confirmModal').find('#btnConfirmModalNoSendEmail').unbind("click").bind("click", {
        remarkEventCode: remarkEventCode
    },function (e) {
        if ($("#confirmModalSaleMode").is(":visible") && $("#confirmModalSaleMode input:checked").length == 0) {
            alert("กรุณาระบุความสำเร็จของกิจกรรม");
            $("#confirmModalSaleMode input:first").focus();
            return;
        }

        if (isRequireRemark != undefined && isRequireRemark) {
            if ($("#confirmModalRemark").val().trim() == "") {
                alert("กรุณาระบุหมายเหตุ");
                $("#confirmModalRemark").focus();
                return;
            }
            else {
                var SaleModeResult = "";
                if ($("#confirmModalSaleMode").is(":visible")) {
                    SaleModeResult = $("#confirmModalSaleMode #customerRadio1").prop("checked") ? "WIN" : "LOST";
                }
                SendRemarkBeforeDoEvent(fnc, false, obj, "confirmModal", SaleModeResult, e.data.remarkEventCode);

                var n = $('#activity-content-box-container').height();
                $('#activity-content-box-container').animate({ scrollTop: n }, 1000);
            }
        }
        else {
            fnc(false, obj);
            closeModal();
        }
        AGLoading(true);
    });
    $('#confirmModal').find('#btnConfirmModalCancel').click(function () {
        closeModal();
    });
    function closeModal() {
        $('#confirmModal').fadeOut(function () {
            $('#confirmModal').remove();
        });
    }
    objEmailTo($('#confirmModal').find('#lblConfirmModalEmailTo'))
}

var GlobalSendRemarkBeforeDoEventFunction;
var GlobalSendRemarkBeforeDoEventSendMail;
var GlobalSendRemarkBeforeDoEventObject;
var GlobalSendRemarkBeforeDoEventComfirmID;

function SendRemarkBeforeDoEvent(fnc, isSendMail, obj, confirmPanelID, SaleModeResult, remarkEventCode) {

    console.log("remarkEventCode", remarkEventCode);

    AGLoading(true);
    GlobalSendRemarkBeforeDoEventComfirmID = confirmPanelID;
    GlobalSendRemarkBeforeDoEventFunction = fnc;
    GlobalSendRemarkBeforeDoEventSendMail = isSendMail;
    GlobalSendRemarkBeforeDoEventObject = obj;

    var txtEmailTo = "";
    if (isSendMail) {
        txtEmailTo = "&nbsp;(Send Email To : ";
        var JsonEmailTo = JSON.parse($("#txtJsonEmailTo").val());
        for (var i = 0; i < JsonEmailTo.length; i++) {
            txtEmailTo += JsonEmailTo[i].name + '&nbsp;&lt;' + JsonEmailTo[i].Email + '&gt;';
            if (JsonEmailTo.length - 1 != i) {
                txtEmailTo += ",&nbsp;"
            }
        }
        txtEmailTo += ")";
    }

    $("#txtSaleModeWinLostResult").val(SaleModeResult == undefined ? "" : SaleModeResult);
    $("#txtRemarkEventCodeBeforeDoEvent").val(remarkEventCode);
    $("#txtRemarkContainerBeforeDoEvent").val(($("#confirmModalRemark").val() + txtEmailTo).split('<').join('&lt;').split('>').join('&gt;'));
    $("#btnRemarkContainerBeforeDoEvent").click();
}

function CallBackSendRemarkBeforeDoEvent() {
    AGLoading(false);
    GlobalSendRemarkBeforeDoEventFunction(GlobalSendRemarkBeforeDoEventSendMail, GlobalSendRemarkBeforeDoEventObject);
    $("#" + GlobalSendRemarkBeforeDoEventComfirmID).fadeOut(function () {
        $("#" + GlobalSendRemarkBeforeDoEventComfirmID).remove();
    });
}

function uploadNewAttachFile(isSendMail, obj) {
    prepareSaveAddAttach(isSendMail);
}

function prepareSaveAddAttach(isSendMail) {
    if (typeof (document.all.iframeAddAttachFile.contentWindow.doSaveFile) == "function")
        document.all.iframeAddAttachFile.contentWindow.doSaveFile(isSendMail);
    else
        AGMessage("Save file error.");
}

function successSaveAddAttach() {
    AGMessage("อัพโหลดไฟล์เสร็จสิ้นแล้ว");
    closeAddAttach();
    loadActivityDetail($("#txtHiddenAObjectlink").val());
}

function prepareSaveEditActivity(isSendMail, obj) {
    $("#txthiddenSendEmailForEditActivity").val(isSendMail);
    $("#btnPostRemark").click();
}

function prepareSaveCauseActivity(isSendMail, obj) {
    $("#txthiddenSendEmailForEditActivity").val(isSendMail);
    $("#btnSaveCauseDetail").click();
}

function prepareSaveSolutionActivity(isSendMail, obj) {
    $("#txthiddenSendEmailForEditActivity").val(isSendMail);
    $("#btnSaveSolutionDetail").click();
}

function cancelActivity(isSendMail, obj) {
    var remark = $("#confirmModalRemark").val() == undefined ? "" : $("#confirmModalRemark").val();
    var requstPost = {
        q: "cancelactivity",
        key: $("#txtHiddenAObjectlink").val(),
        rowkey: $("#txtHiddenRowkey").val(),
        SNAID: $("#txtHiddenCompanyCode").val(),
        sendmail: isSendMail.toString(),
        remark: remark
    };

    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx',
        data: requstPost,
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            closeActivityDetial();
            CallbackActivityEvent("canceled");
            LoadActivityNotificationCount();
            $("#panel-activity-detail").hide();
            if ($("#txtHiddenAObjectlink").length > 0 && $("#txtHiddenAObjectlink").val() != '') {
                loadActivityDetail($("#txtHiddenAObjectlink").val());
            }
            AGMessage("บันทึกยกเลิกเสร็จสิ้นแล้ว");
        }
    });
}

function checkOutClick(isSendMail, obj) {
    checkout(isSendMail, $("#txtHiddenRowkey").val());
    if ($("#txtHiddenAObjectlink").length > 0 && $("#txtHiddenAObjectlink").val() != '') {
        loadActivityDetail($("#txtHiddenAObjectlink").val());
    }
}

function recreateActivity(obj) {
    
    if (AGConfirm(obj,'ต้องการไปยังหน้าสร้างใหม่จากรายการเดิมหรือไม่'))
        openIfraneCreateActivity(document.getElementById("detail_type").innerText, $("#txtHiddenAObjectlink").val(), $("#txtHiddenRowkey").val(), $("#txtHiddenCompanyCode").val());
}

function convertNoteTotask(obj) {
    if (AGConfirm(obj,'ต้องการเปลี่ยนให้เป็น Task หรือไม่'))
        openIframeConvertNoteTotask();
}

function openIframeConvertNoteTotask() {
    var aobj = $("#txtHiddenAObjectlink").val();
    var rowkey = $("#txtHiddenRowkey").val();
    var snaid = $("#txtHiddenCompanyCode").val();
    var src = '/widget/PopupCreateActivityReDesign.aspx?aobj=' + aobj + '&rowkey=' + rowkey + '&snaid=' + snaid + '&mode=convertnote';
    
    if (window.parent.length > 0) {
        window.parent.createActivityStart(src);
    } else {
        var ifca = document.getElementById("iframeCreateActivity");
        ifca.src = src;
    }

}

function callBackLoadNewActivity(aobj, oldAobj) {
    $(".tab-panel-activity-act[aobjectlink='" + oldAobj + "'] .fa-remover").click();
    CallbackActivityEvent("notetotask", aobj);
}

function checkout(isSendMail, key) {
    keyArr = key.split(',');
    var remark = $("#confirmModalRemark").val() == undefined ? "" : $("#confirmModalRemark").val();
    var requstPost = {
        q: "checkout",
        DATEIN: keyArr[1],
        SEQ: keyArr[2],
        EMPCODE: keyArr[0],
        SNAID: keyArr[3],
        key: $("#txtHiddenAObjectlink").val(),
        rowkey: $("#txtHiddenRowkey").val(),
        sendmail: isSendMail.toString(),
        remark: remark
    };
    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx',
        data: requstPost,
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            closeActivityDetial();
            loadActivityDetail($("#txtHiddenAObjectlink").val());
            CallbackActivityEvent("checkout");
            LoadActivityNotificationCount();
            $("#panel-activity-detail").hide();
            AGMessage("บันทึกปิดงานเรียบร้อยแล้ว");
        }
    });
}

function recallActivity(isSendMail, obj) {
    var members = [];
    if ($("#detail_leader_readed span").html() == "ยังไม่รับทราบ")
        members.push($("#detail_leader_id").html());

    $("#table_detail_other tr").each(function () {
        var id = "";
        var read = "";
        $(this).find("td").each(function () {
            if ($(this).index() == 0)
                id = $(this).html();
            if ($(this).index() == 2)
                read = $(this).find("span").html();
        }).html();
        if (read == "ยังไม่รับทราบ")
            members.push(id);
    });

    var remark = $("#confirmModalRemark").val() == undefined ? "" : $("#confirmModalRemark").val();
    var requstPost = {
        q: "recall",
        member: members.toString(),
        key: $("#txtHiddenAObjectlink").val(),
        rowkey: $("#txtHiddenRowkey").val(),
        SNAID: $("#txtHiddenCompanyCode").val(),
        sendmail: isSendMail.toString(),
        remark: remark
    };

    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx',
        data: requstPost,
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            AGMessage("ส่งข้อความทวงถามเรียบร้อยแล้ว");
            CallbackActivityEvent("recall");
            LoadActivityNotificationCount();
            //closeActivityDetial();
            //loadActivityDetail($("#txtHiddenRowkey").val());
            loadActivityDetail($("#txtHiddenAObjectlink").val());

        }
    });
}

function successClick(isSendMail, obj) {
    if ($("#txtHiddenYouAre").val() == "MD") {
        var remark = $("#confirmModalRemark").val() == undefined ? "" : $("#confirmModalRemark").val();
        var requstPost = {
            q: "eventleader",
            key: $("#txtHiddenAObjectlink").val(),
            val: "SUCCESS",
            rowkey: $("#txtHiddenRowkey").val(),
            SNAID: $("#txtHiddenCompanyCode").val(),
            sendmail: isSendMail.toString(),
            remark: remark,
            prkey: $("#txtHiddenkeyDataPR").val()
        };
        $.ajax({
            url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx',
            data: requstPost,
            success: function (data) {
                if (ErrorAPIHandel(data))
                    return;
                AGMessage("บันทึกเสร็จสิ้นเรียบร้อยแล้ว");
                loadActivityDetail($("#txtHiddenAObjectlink").val());
                CallbackActivityEvent("success");
                $("#txtHiddenAP").val("เสร็จสิ้นแล้ว");
            }
        });
    }
    if ($("#txtHiddenYouAre").val() == "OD") {
        var remark = $("#confirmModalRemark").val() == undefined ? "" : $("#confirmModalRemark").val();
        var requstPost = {
            q: "eventother",
            key: $("#txtHiddenAObjectlink").val(),
            val: "SUCCESS",
            rowkey: $("#txtHiddenRowkey").val(),
            SNAID: $("#txtHiddenCompanyCode").val(),
            sendmail: isSendMail.toString(),
            remark: remark,
            prkey: $("#txtHiddenkeyDataPR").val()
        };
        $.ajax({
            url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx',
            data: requstPost,
            success: function (data) {
                if (ErrorAPIHandel(data))
                    return;
                AGMessage("บันทึกเสร็จสิ้นเรียบร้อยแล้ว");
                loadActivityDetail($("#txtHiddenAObjectlink").val());
                CallbackActivityEvent("success");
                $("#txtHiddenAP").val("เสร็จสิ้นแล้ว");
            }
        });
    }

}

function readedClick(isSendMail, obj) {
    if ($("#txtHiddenYouAre").val() == "MD") {
        $.ajax({
            url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=eventleader&key=' + $("#txtHiddenAObjectlink").val() + '&val=READED' + '&rowkey=' + $("#txtHiddenRowkey").val() + '&SNAID=' + $("#txtHiddenCompanyCode").val() + '&sendmail=' + isSendMail + '&prkey=' + $("#txtHiddenkeyDataPR").val(),
            success: function (data) {
                if (ErrorAPIHandel(data))
                    return;
                AGMessage("บันทึกรับทราบเรียบร้อยแล้ว");

                loadActivityDetail($("#txtHiddenAObjectlink").val());
                CallbackActivityEvent("readed");
                $("#txtHiddenAP").val("รับทราบแล้ว");
            }
        });
    }
    if ($("#txtHiddenYouAre").val() == "OD") {
        $.ajax({
            url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=eventother&key=' + $("#txtHiddenAObjectlink").val() + '&val=READED' + '&rowkey=' + $("#txtHiddenRowkey").val() + '&SNAID=' + $("#txtHiddenCompanyCode").val() + '&sendmail=' + isSendMail + '&prkey=' + $("#txtHiddenkeyDataPR").val(),
            success: function (data) {
                if (ErrorAPIHandel(data))
                    return;
                AGMessage("บันทึกรับทราบเรียบร้อยแล้ว");

                loadActivityDetail($("#txtHiddenAObjectlink").val());
                CallbackActivityEvent("readed");
                $("#txtHiddenAP").val("รับทราบแล้ว");
            }
        });
    }
}

function reOpenClick(isSendMail, obj) {
    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=taskreopen&key=' + $("#txtHiddenAObjectlink").val() + '&rowkey=' + $("#txtHiddenRowkey").val() + '&sendmail=' + isSendMail + '&xremark=' + $("#txtRemarkContainerBeforeDoEvent").val(),
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            AGMessage("เปิดงานใหม่เพื่อแก้ไขอีกครั้งเรียบร้อยแล้ว");
            loadActivityDetail($("#txtHiddenAObjectlink").val());
            CallbackActivityEvent("reopen");
        }
    });
}

function loadActivityDetailLinked() {
    if ($("#txtContinueActivityFromEmail").length > 0 && $("#txtContinueActivityFromEmail").val() != "") {
        var rowkey = $("#txtContinueActivityFromEmail").val().split(":")[0];
        var snaid = $("#txtContinueActivityFromEmail").val().split(":")[1];
        var aobjectLink = $("#txtContinueActivityFromEmail").val().split(":")[2];
        var position = "";
        var activityName = $("#txtContinueActivitySubjectFromEmail").val();
        onClickActivityLink(rowkey, aobjectLink, snaid, position, activityName);
        var temp = AppendActivityLinkElement(rowkey, aobjectLink, snaid, position, activityName);
        temp.click();
    }
}

function loadActivityDetailServerCall(aObjectLink) {
    $(".new-message-trigger[data-new-message-trigger-aobjectlink='" + aObjectLink + "']").click();
}

function ChangeRemarkControl() {
    $("#remark-control-panel ul").AGWhiteLoading(true,"Changing status");
    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx',
        type: "POST",
        data : {
            q: "taskremaekcontrol",
            aobj: $("#txtHiddenAObjectlink").val(),
            allowRemarkMaindelegate: $(".remark-control-check[data-ramek-control='MD']").prop("checked"),
            allowRemarkOtherDelegate: $(".remark-control-check[data-ramek-control='OD']").prop("checked"),
            allowRemarkOutsider: $(".remark-control-check[data-ramek-control='OS']").prop("checked")
        },
        success: function (data) {
            $("#remark-control-panel ul").AGWhiteLoading(false);
        }
    });
}

function loadActivityDetail(aObjectLink, mode, isLoadkm) {
    AGLoading(true);
    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=task&aObjectLink=' + aObjectLink + '&old=' + $("#txtHiddenAObjectlink").val(),
        success: function (data) {
            $(".panel-Revenue_Expense").hide();
            $('#detail_Revenue_Expense').removeClass('alert-warning');

            var ImageHeader = "<div class='image-box' style='border-radius: 0;width:110px; height:110px; margin:0px; background-image: url(" + data[0].IMAGE + ");'></div>";
            var progressPercent = data[0].TaskStatusPercent;
            var progressDescription = progressPercent + "% " + data[0].TaskStatusDesc;
            var ProgressHeader = "<div class='progress' style='margin-bottom:0' title='" + progressDescription + "'><div class='progress-bar progress-bar-success' role='progressbar' aria-valuenow='" + data[0].TaskStatusPercent + "' aria-valuemin='0' aria-valuemax='100' style='width:" + data[0].TaskStatusPercent + "%;" + (parseInt(progressPercent) == 0 ? "color:#333;" : "") + "'></div><div class='progress-desc'>" + progressDescription + "</div></div>";
            $(".modeValue").hide();
            $('#image-Show-Pic').html(ImageHeader);
            $('#ProgressWork').html(ProgressHeader);
            $("#txtJsonEmailTo").val('[]');

            //$("#checkMode").parent().next().removeClass("text-success");
            //if (data[0].MYID == data[0].CREATED_BY) {
            //    $("#checkMode").prop('checked', true);
            //    clickMode();
            //} else {
            //    $("#checkMode").prop('checked', false);
            //    $("#checkMode").parent().next().removeClass("text-success");
            //}

            $("#checkMode").prop('checked', true);
            $(".modeValue").show();

            if (ErrorAPIHandel(data))
                return;

            if (mode == 'display') {
                firstLoad = false;
                $("#div-btn-event-container").hide();
            } else {
                $("#div-btn-event-container").show();
            }
            $("#btn-special-reopen,.btn-closed-status").hide();
            if (data[0].checkOut_Date != "" || data[0].XSTATUS == "CANCELED") {
                $("#div-btn-event-container").hide();

                //if (data[0].MYID == data[0].CREATED_BY) {
                    $("#btn-special-reopen").show();
                //}

                if (data[0].checkOut_Date != "") {
                    $("#btn-detail-closed").show();
                }

                if (data[0].XSTATUS == "CANCELED") {
                    $("#btn-detail-canceled").show();
                }
            }

            //=============== About Item Type (Note Task Meeting Service)
            var ItemType = "";
            if(data[0].ItemTypeName == "Job"){
                ItemType = "Service";
            }
            else if(data[0].ItemTypeName == "LO"){
                ItemType = "Logistic";
            }
            else{
                ItemType = data[0].ItemTypeName
            }
            ItemType = ItemType == null || ItemType == undefined || ItemType == "" ? data[0].ItemType : ItemType;

            $('#detail_type').html(ItemType);
            $('#detail_type_code').html(data[0].ItemType);

            if (ItemType == "Service") {
                $(".service-panel").show();

                var cause = data[0].Cause == null ? "ยังไม่มีการสรุปสาเหตุ" : data[0].Cause.split("\n").join("<br/>");
                var solution = data[0].Solution == null ? "ยังไม่มีการสรุปสาเหตุ" : data[0].Solution.split("\n").join("<br/>");
                var causeText = data[0].Cause == null ? "" : data[0].Cause;
                var solutionText = data[0].Solution == null ? "" : data[0].Solution;

                if (data[0].MYID == data[0].EMPCODE && data[0].MYID != data[0].CREATED_BY) {
                    $("#txtSolutionDetail").val(solutionText);
                    $("#txtCauseDetail").val(causeText);
                    $(".service-result").hide();
                    $(".service-edit").show();
                }
                else {
                    $("#service-result-solution").html(solution);
                    $("#service-result-cause").html(cause);
                    $(".service-result").show();
                    $(".service-edit").hide();
                }
            }
            else {
                $(".service-panel").hide();
            }
            //============= Activity detail

            $('#hfProjectCode').val(data[0].PROJECTCODE);
            $('#hfSubProjectCode').val(data[0].PROJECTELEMENTCODE);

            //KENG 
            $("#detail_subject,#titleTaskName,#detail_subject_invoice,#poSubjectRight").html(data[0].JOBDESCRIPTION);
            $("#txtdetail_subject").val(data[0].JOBDESCRIPTION);
            $("#detail_subject,#titleTaskName,#detail_subject_invoice,#poSubjectRight").parent().prop("title", data[0].JOBDESCRIPTION);
            //if (data[0].DOCTYPE == "ACTCHAIN") {
            //    var arrJOBDESC = "";
            //    var Jarr = data[0].JOBDESCRIPTION.split(':');
            //    for (var i = 0; i < Jarr.length; i++) {
            //        if (arrJOBDESC != "")
            //        {
            //            arrJOBDESC += " : ";
            //        }
            //        arrJOBDESC += '[' + Jarr[i].split('[').join('').split(']').join('') + ']';
                    
            //    }
            //    $("#detail_subject").html(arrJOBDESC);
            //}
            
            $("#detail_star").removeClass("activity-favorite");
            if (data[0].isFavorite.toLowerCase() == "true") {
                $("#detail_star").addClass("activity-favorite");
            }

            $("#detail_folder").removeClass(".is-in-my-folder");
            if (data[0].MyFolderCode != null && data[0].MyFolderCode != "") {
                $("#detail_folder").addClass("is-in-my-folder");
            }

            //$("#activity-command-container").css({
            //    borderLeft: (data[0].ActivityColor == "" ? "none" : ("5px solid " + data[0].ActivityColor))
            //});

            var _projectDetail = data[0].PROJECTNAME == "" ? "ไม่ระบุกลุ่มงาน" : data[0].PROJECTNAME;

            if (data[0].PROJECTNAME != "" && data[0].PROJECTELEMENTNAME != "") {
                _projectDetail += " / " + data[0].PROJECTELEMENTNAME;
            }

            $('#detail_project').html(_projectDetail);            
            $('#detail_inv_project').html(_projectDetail);// for inv
            $('#detail_projectPO').html(_projectDetail);
            $('#detail_sub_project').html(data[0].PROJECTELEMENTNAME);            
            $('#detail_datetime_start').html(dateformat(data[0].DATEIN) + ' ' + timeformat(data[0].TIMEIN));
            $("#detail_call_startdate").html(dateformat(data[0].DATEIN) + ' ' + timeformat(data[0].TIMEIN));// call card
            $('#detail_datetime_end').html(dateformat(data[0].DATEOUT) + ' ' + timeformat(data[0].TIMEOUT));
            $("#detail_call_duedate").html(dateformat(data[0].DATEOUT) + ' ' + timeformat(data[0].TIMEOUT));// call card
            $('#detail_location').html(data[0].SITECODE == "" ? "ไม่ระบุ" : data[0].SITECODE);
            $('#detail_your_timePO').html(dateformat(data[0].DATEOUT));

            //============== Money 
            $("#detail_money_revenue").html(data[0].ShowRevenue);
            $("#detail_money_charges").html(data[0].ShowCharges);
            $(".detail_money_currency").html(data[0].Currency == "" || data[0].Currency == null ? "THB" : data[0].Currency);
            $("#detail_asset_value").html(data[0].ShowAsset);
            $("#detail_consumption_value").html(data[0].ShowConsumption);
            var PriorityStar = "";
            if (data[0].PriorityCode == "01") {
                PriorityStar += "<i class='fa fa-star-o range-star'></i>";
                PriorityStar += "<i class='fa fa-star-o range-star'></i>";
                PriorityStar += "<i class='fa fa-star-o range-star'></i>";
                PriorityStar += "<i class='fa fa-star-o range-star'></i>";
                PriorityStar += "<i class='fa fa-star-o range-star'></i>";
            }
            else if (data[0].PriorityCode == "02") {
                PriorityStar += "<i class='fa fa-star range-star'></i>";
                PriorityStar += "<i class='fa fa-star-half-empty range-star'></i>";
                PriorityStar += "<i class='fa fa-star-o range-star'></i>";
                PriorityStar += "<i class='fa fa-star-o range-star'></i>";
                PriorityStar += "<i class='fa fa-star-o range-star'></i>";
            }
            else if (data[0].PriorityCode == "03") {
                PriorityStar += "<i class='fa fa-star range-star'></i>";
                PriorityStar += "<i class='fa fa-star range-star'></i>";
                PriorityStar += "<i class='fa fa-star-half-empty range-star'></i>";
                PriorityStar += "<i class='fa fa-star-o range-star'></i>";
                PriorityStar += "<i class='fa fa-star-o range-star'></i>";
            }
            else if (data[0].PriorityCode == "04") {
                PriorityStar += "<i class='fa fa-star range-star'></i>";
                PriorityStar += "<i class='fa fa-star range-star'></i>";
                PriorityStar += "<i class='fa fa-star range-star'></i>";
                PriorityStar += "<i class='fa fa-star-half-empty range-star'></i>";
                PriorityStar += "<i class='fa fa-star-o range-star'></i>";
            }
            else if (data[0].PriorityCode == "05") {
                PriorityStar += "<i class='fa fa-star range-star'></i>";
                PriorityStar += "<i class='fa fa-star range-star'></i>";
                PriorityStar += "<i class='fa fa-star range-star'></i>";
                PriorityStar += "<i class='fa fa-star range-star'></i>";
                PriorityStar += "<i class='fa fa-star range-star'></i>";
            }
            if (data[0].CREATED_BY == data[0].MYID || data[0].EMPCODE == data[0].MYID) {
                $(".btnChangeOtherDetail").show();
            }
            else {
                $(".btnChangeOtherDetail").hide();
            }

            $('#detail_priority').html(PriorityStar + "(" + data[0].Priority + ")");           
            $('#detail_inv_priority').html(PriorityStar + "(" + data[0].Priority + ")");
            $('#detail_priorityPO').html(PriorityStar + "(" + data[0].Priority + ")");

            $("#priority-range-text").html(data[0].Priority);
            $("#priority-range-gate").val((isNaN(parseInt(data[0].PriorityCode)) ? "0" : parseInt(data[0].PriorityCode)));

            var HastStar = "";
            if (data[0].HasteCode == "01") {
                HastStar += "<i class='fa fa-star-o range-star'></i>";
                HastStar += "<i class='fa fa-star-o range-star'></i>";
                HastStar += "<i class='fa fa-star-o range-star'></i>";
                HastStar += "<i class='fa fa-star-o range-star'></i>";
                HastStar += "<i class='fa fa-star-o range-star'></i>";
            }
            else if (data[0].HasteCode == "02") {
                HastStar += "<i class='fa fa-star range-star'></i>";
                HastStar += "<i class='fa fa-star-half-empty range-star'></i>";
                HastStar += "<i class='fa fa-star-o range-star'></i>";
                HastStar += "<i class='fa fa-star-o range-star'></i>";
                HastStar += "<i class='fa fa-star-o range-star'></i>";
            }
            else if (data[0].HasteCode == "03") {
                HastStar += "<i class='fa fa-star range-star'></i>";
                HastStar += "<i class='fa fa-star range-star'></i>";
                HastStar += "<i class='fa fa-star-half-empty range-star'></i>";
                HastStar += "<i class='fa fa-star-o range-star'></i>";
                HastStar += "<i class='fa fa-star-o range-star'></i>";
            }
            else if (data[0].HasteCode == "04") {
                HastStar += "<i class='fa fa-star range-star'></i>";
                HastStar += "<i class='fa fa-star range-star'></i>";
                HastStar += "<i class='fa fa-star range-star'></i>";
                HastStar += "<i class='fa fa-star-half-empty range-star'></i>";
                HastStar += "<i class='fa fa-star-o range-star'></i>";
            }
            else if (data[0].HasteCode == "05") {
                HastStar += "<i class='fa fa-star range-star'></i>";
                HastStar += "<i class='fa fa-star range-star'></i>";
                HastStar += "<i class='fa fa-star range-star'></i>";
                HastStar += "<i class='fa fa-star range-star'></i>";
                HastStar += "<i class='fa fa-star range-star'></i>";
            }

            $('#detail_haste').html(HastStar + "(" + data[0].Haste + ")");
            $('#detail_inv_haste').html(HastStar + "(" + data[0].Haste + ")");
            $('#detail_hastePO').html(HastStar + "(" + data[0].Haste + ")");

            $("#haste-range-text").html(data[0].Haste);
            $("#haste-range-gate").val((isNaN(parseInt(data[0].HasteCode)) ? "0" : parseInt(data[0].HasteCode)));

            $('#detail_problem').html(data[0].ProblemType);
            $('#detail_inv_problem').html(data[0].ProblemType);
            $('#detail_problemPO').html(data[0].ProblemType);
            $('#detail_for').html(data[0].ActivityFor);
            $('#detail_create_by').html(data[0].CREATED_BY_FULLNAME);
            $('#detail_create_by').attr("data-createBy", data[0].CREATED_BY);
            $('#detail_create_by').attr("data-myLinkID", data[0].MYID);
            converJsonEmailTo(data[0].CREATED_BY_FULLNAME, data[0].MYID);
            $('#detail_inv_create_by').html(data[0].CREATED_BY_FULLNAME);//for inv

            //===================== Main delagate ======================
            $('#detail_leader_id').html(data[0].EMPCODE);
            $('#detail_leader_name').html(data[0].EMPNAME + " " + data[0].EMPSURNAME);
            var stamp = data[0].CompleteDate != "" ? data[0].CompleteDate : (data[0].ResponeDate != "" ? data[0].ResponeDate : "-");
            var delay = data[0].DELAY;

            $("#datail_response_date").html(data[0].ResponeDate != "" ? data[0].ResponeDate : "-");
            $("#datail_complete_date").html(data[0].CompleteDate != "" ? data[0].CompleteDate : "-");
            $("#detail_used_time").html(data[0].UsedTime);
            $("#detial_respones_rate").html(data[0].ResponseRate);

            $("#detail_your_time").html(data[0].YourTime);

            $("#detail_work_place").html(data[0].WorkPlace);

            // TK
            if (data[0].Showticket != null && data[0].Showticket != "") {
                $("#ticketDetailBox").show();
                $("#lblTicketNumber").html(data[0].Showticket);
                $("#lblRef_1").html(data[0].TK_ref1);
                $("#lblRef_2").html(data[0].TK_ref2);
            } else {
                $("#ticketDetailBox").hide();
            }
            

            $("#detail_ticket").html(data[0].Showticket);
            $("#txtticket").val(data[0].Showticket);
            $("#txtticketdetail").val(data[0].TKDetail);
            $("#txtTK_ref1").val(data[0].TK_ref1);
            $("#txtTK_ref2").val(data[0].TK_ref2);
            $("#txtTK_ref3").val(data[0].TK_ref3);
            $("#txtTK_ref4").val(data[0].TK_ref4);
            $("#txtTK_ref5").val(data[0].TK_ref5);
            $("#lblticket_popup").html(data[0].Showticket);
            $("#lblticketdetail_popup").html(data[0].TKDetail);
            $("#lblTK_ref1_popup").html(data[0].TK_ref1);
            $("#lblTK_ref2_popup").html(data[0].TK_ref2);
            //$("#lblTK_ref3_popup").html(data[0].TK_ref3);
            //$("#lblTK_ref4_popup").html(data[0].TK_ref4);
            //$("#lblTK_ref5_popup").html(data[0].TK_ref5);
            // end TK
            startCountDown(data[0].YourTimeCountDown, "detail_your_time_countdown_container");

            $("#detail_create_on").html(data[0].CREATE_ON_DISPLAY);
            $("#detail_call_assign_date").html(data[0].CREATE_ON_DISPLAY);
            $("#detail_inv_assign_date").html(data[0].CREATE_ON_DISPLAY);//for inv

            var _taskProgress = "<div class='pace pace-active'><div class='pace-progress' data-progress='" + data[0].TaskStatusPercent +
            "' data-progress-text='" + data[0].TaskStatusPercent + "%' style='-webkit-transform: translate3d(" + data[0].TaskStatusPercent + "%, 0px, 0px);" +
            " -ms-transform: translate3d(" + data[0].TaskStatusPercent + "%, 0px, 0px); transform: translate3d(" + data[0].TaskStatusPercent + "%, 0px, 0px);'>" +
            " <div class='pace-progress-inner'></div></div><div class='pace-activity'></div></div>";

            $("#detail_task_progress").html(_taskProgress);
            $("#detail_task_status").html(data[0].TaskStatusDesc);

            //$("#detail_task_status").html("<div class='pro-bar baw,r-50 color-clouds' data-pro-bar-percent='50'>" + data[0].TaskStatusDesc + "</div>");

            $('#detail_leader_delay').html("Delay : " + delay);

            var leaderReaded = "<span style='color:orange'>ยังไม่รับทราบ</span>";
            if (data[0].XSTATUS == "READED")
                leaderReaded = "<span style='color:green'>รับทราบแล้ว</span>"
            if (data[0].XSTATUS == "SUCCESS")
                leaderReaded = "<span style='color:red'>เสร็จสิ้น</span>"
            $('#detail_leader_readed').html(leaderReaded);

            //================= Set who am i
            $(".btn-activity-event").hide().removeAttr("disabled");
            $(".edit-delegate").show();
            $("#edit-main-delegate").hide();
            $(".disabled-edit-delegate").hide();
            $("#btnCheckout").hide();

            // Creator  
            if (data[0].MYID == data[0].CREATED_BY) {
                $("#txtHiddenYouAre").val("C");
                $("#btnRecall").show();
                $("#btnCheckout").show();
                $("#btnCancel").show();
                $("#btnRecreateActivity").show();
                $("#btnCancel").show();
                $("#edit-main-delegate").show();
                if (data[0].MYSNAID != data[0].COMPANYCODE) {
                    $("#btnRecreateActivity").attr("disabled", "");
                    $(".edit-delegate").hide();
                    $(".disabled-edit-delegate").show();
                }

                if (ItemType == "Note" || data[0].ItemType.toUpperCase() == "EMAIL") {
                    $("#btnConvertNote").show();
                }
            }

            // main delegate
            if (data[0].MYID == data[0].EMPCODE && data[0].MYID != data[0].CREATED_BY) { 
                $("#txtHiddenYouAre").val("MD");
                $("#btnCheckout").show();
                $("#btnResponse").show();
                $("#btnSuccess").show();
                if (ItemType == "Note" || data[0].ItemType.toUpperCase() == "EMAIL") {
                    $("#btnConvertNote").show();
                    $("#btnSuccess").hide();
                }
            }

            // other delegate
            if (data[0].MYID != data[0].EMPCODE && data[0].MYID != data[0].CREATED_BY) {
                $("#btnCheckout").show();
                $("#txtHiddenYouAre").val("OD"); 
                $("#btnResponse").show();
                //$("#btnSuccess").show();
                if (ItemType == "Note" || data[0].ItemType.toUpperCase() == "EMAIL") {
                    $("#btnConvertNote").show();
                    $("#btnSuccess").hide();
                }
            }

            if (ItemType == "Note" || data[0].ItemType.toUpperCase() == "EMAIL") {
                $("#btnCheckout").hide();
            }


            $('#imgChangeMainDelegateHistory').hide();
            $('#imgChangeMainDelegate').show();
            $('#imgInvChangeMainDelegateHistory').hide();//for inv
            $('#imgInvChangeMainDelegate').hide();// for inv

            if (data[0].OLD_DELEGATE_NAME != null && data[0].OLD_DELEGATE_NAME.trim() != "") {
                $('#old_detail_main_delegate').html(data[0].OLD_DELEGATE_NAME + " => ");
                $('#old_detail_inv_main_delegate').html(data[0].OLD_DELEGATE_NAME + " => ");//for inv
                if (mode == 'display') {
                    $('#imgChangeMainDelegateHistory').hide();
                    $('#imgChangeMainDelegate').hide();
                    $('#imgInvChangeMainDelegateHistory').hide();//for inv
                    $('#imgInvChangeMainDelegate').hide();// for inv
                } else {
                    $('#imgChangeMainDelegateHistory').show();
                    $('#imgChangeMainDelegate').hide();
                    if (data[0].MYID == data[0].CREATED_BY || data[0].MYID == data[0].EMPCODE) {
                        $('#imgChangeMainDelegate').show();
                    }
                    $('#imgInvChangeMainDelegateHistory').show();//for inv
                    $('#imgInvChangeMainDelegate').hide();// for inv
                }
            }
            else {
                $('#old_detail_main_delegate').html("");
                $('#old_detail_inv_main_delegate').html("");//for inv
                if (mode == 'display') {
                    $('#imgChangeMainDelegateHistory').hide();
                    $('#imgChangeMainDelegate').hide();
                    $('#imgInvChangeMainDelegateHistory').hide();//for inv
                    $('#imgInvChangeMainDelegate').hide();// for inv
                } else {
                    $('#imgChangeMainDelegateHistory').hide();
                    $('#imgChangeMainDelegate').show();
                    $('#imgInvChangeMainDelegateHistory').hide();//for inv
                    $('#imgInvChangeMainDelegate').show();// for inv
                }
            }


            if (data[0].CREATED_BY == data[0].EMPCODE) {
                //$('#detail_main_delegate').html("<span style='color:#000'>ไม่มีผู้รับมอบหมายหลัก</span>");
                $('#detail_inv_main_delegate').html("<span style='color:#000'>" + data[0].EMPNAME + " " + data[0].EMPSURNAME + "</span>");
                $('#detail_main_delegate').html("<span data-linkid='" + data[0].EMPCODE + "' style='color:#000'>" + data[0].EMPNAME + " " + data[0].EMPSURNAME + "</span>");//for inv
            }
            else {
                var readedTextColor = "#333";
                var readedTextTitle = "ยังไม่รับทราบ";
                if (data[0].XSTATUS == "READED") {
                    readedTextTitle = "รับทราบแล้ว";
                    readedTextColor = "#049dbd";
                }
                if (data[0].XSTATUS == "SUCCESS") {
                    readedTextTitle = "เสร็จสิ้นแล้ว";
                    readedTextColor = "#049dbd";
                }

                $('#detail_main_delegate').html("<span data-linkid='" + data[0].EMPCODE + "' title='" + readedTextTitle + "' style='color:" + readedTextColor + "'>" + data[0].EMPNAME + " " + data[0].EMPSURNAME + "</span>");
                $('#detail_inv_main_delegate').html("<span title='" + readedTextTitle + "' style='color:" + readedTextColor + "'>" + data[0].EMPNAME + " " + data[0].EMPSURNAME + "</span>");
                converJsonEmailTo(data[0].EMPNAME + " " + data[0].EMPSURNAME, data[0].EMPCODE);
            }

            //================ Disable button for Main delegate
            if (data[0].XSTATUS == "READED" && data[0].MYID == data[0].EMPCODE)
                $("#btnResponse").attr("disabled", "");
            if (data[0].XSTATUS == "SUCCESS" && data[0].MYID == data[0].EMPCODE) {
                $("#btnResponse").attr("disabled", "");
                $("#btnSuccess").attr("disabled", "");
            }

            //================ Disable Re-Open 
            if (data[0].ItemType == "004" && data[0].XSTATUS == "SUCCESS" && data[0].CompleteDate != "" && data[0].MYID == data[0].CREATED_BY) {
                $("#btnReOpen").show();
            }
            else {
                $("#btnReOpen").hide();
            }

            //================ Check for change task status

            if ((data[0].ItemType == "004" || data[0].ItemType == "009" || data[0].ItemType == "013") && (data[0].ResponeDate != "" || data[0].CompleteDate != "")) {
                $("#detailTaskStatus").show();
            }
            else {
                if ((data[0].ItemType == "004" || data[0].ItemType == "009" || data[0].ItemType == "013") && (data[0].ResponeDate == "" || data[0].CompleteDate == "") && data[0].EMPCODE == data[0].CREATED_BY) {
                    $("#detailTaskStatus").show();
                }
                else {
                    $("#detailTaskStatus").hide();
                }
            }

            if (data[0].ItemType == "009" || data[0].ItemType == "013") {
                $("#task-status-invoice").append($("#detailTaskStatus"));
            }
            else if (data[0].ItemType == "004") {
                $("#task-status-default").append($("#detailTaskStatus"));
            }
            else {
                $("#task-status-default").append($("#detailTaskStatus"));
            }


            var blackCol = "#333";
            var greenCol = "#009688";

            if (data[0].isHaveMOM == "true") {
                $("#momShortcut").css("color", blackCol);
            }
            else {
                $("#momShortcut").css("color", blackCol);
            }

            if (data[0].isHaveKM == "true") {
                $("#kmShortcut").css({
                    color: "#fff",
                    borderColor: greenCol,
                    background: "#1ABC9C"
                });
            }
            else {
                $("#kmShortcut").css({
                    color: "#333",
                    borderColor: "#aaa",
                    background:"none"
                });
            }

            if (parseInt(data[0].countLocation) > 0 ) {
                $("#locationShortcut").css("color", blackCol);
                $("#locationShortcut").prev().html(data[0].countLocation).show();
            }
            else {
                $("#locationShortcut").css("color", blackCol);
                $("#locationShortcut").prev().html(data[0].countLocation).hide();
            }

            if (parseInt(data[0].countMultimedia) > 0) {
                $("#multimediaShortcut").css("color", blackCol);
                $("#multimediaShortcut").prev().html(data[0].countMultimedia).show();
            }
            else {
                $("#multimediaShortcut").css("color", blackCol);
                $("#multimediaShortcut").prev().html(data[0].countMultimedia).hide();
            }

            //============= Load Sale Task Customer
            if (data[0].hasSaleCustomer.toUpperCase() == "TRUE") {
                $("#handleActivityDetailCustomer").show();

                loadActivityDetailSaleContact(data[0].AOBJECTLINK);
            }
            else {
                $("#handleActivityDetailCustomer").hide();                
            }

            //================ Load party , remark and behavior
            loadActivityDetailParty(data[0].MYID, data[0].CREATED_BY, data[0].EMPCODE, data[0].ItemType);
            loadActivityDetailBehavior();


            $("#description-table").html("");
            $("#remark-table").html("");
            $(".xpanel").show();
            if (data[0].allowRemark.toLowerCase() == "true") {
                loadActivityDetailRemark(data[0].AOBJECTLINK, data[0].MYID, data[0].CREATED_BY_FULLNAME, data[0].JOBREMARKS, data[0].CREATE_ON_DISPLAY, data[0].IMAGE, data[0].CREATED_BY);
                loadActivityDetailRemarkEmail(data[0].AOBJECTLINK, data[0].MYID, data[0].CREATED_BY_FULLNAME, data[0].JOBREMARKS, data[0].CREATE_ON_DISPLAY, data[0].IMAGE, data[0].CREATED_BY);
            } else {
                $(".xpanel").toggle($("#description-table").html() != "");
            }

            $("#remark-control-panel").toggle($("#txtHiddenYouAre").val() == "C" || $("#txtHiddenYouAre").val() == "MD");
            $(".remark-control-check[data-ramek-control='MD']").prop("checked", data[0].allowRemarkMaindelegate.toLowerCase() == "true");
            $(".remark-control-check[data-ramek-control='OD']").prop("checked", data[0].allowRemarkOtherDelegate.toLowerCase() == "true");
            $(".remark-control-check[data-ramek-control='OS']").prop("checked", data[0].allowRemarkOutsider.toLowerCase() == "true");
            
            //=========== Invoice Activity 

            $('.activity-panel-detail-default').hide();
            $('.activity-panel-detail-invoice').hide();
            $('.activity-panel-detail-purchase').hide();
            $('.rowInvoiceDetail').hide();
            $('#btnInvoiceSendMail').hide();

            //default เป็นปุ่มโหมด 'รับทราบ' ถ้าเป็น Type PR เป็นเป็นโหมด 'อนุมัติ'
            $("#btnResponse").html("<i class='fa fa-check'></i> รับทราบ ");
            $(".div-businessobject-pr").hide();
            $(".div-businessobj-default").show();
            $("#divDetailHide").show();
            $(".modeValue").show();
            $(".div-default-Ticket").show();
            $("#txtHiddenkeyDataPR").val("");
            //end

            if (data[0].ItemType == "009") {
                $('#txtHiddenActivityType').val(ItemType);
                $('.activity-panel-detail-invoice').show();
                $('.rowInvoiceDetail').show();
                $('#btnInvoiceSendMail').show();
                loadActivityInvoiceDetail(data[0].DOCNUMBER, data[0].DOCTYPE, data[0].DOCYEAR);
                //==== Load

            }
            else if (data[0].ItemType == "013") {
                $('.activity-panel-detail-invoice').show();
                loadActivityInvoiceDetail(data[0].DOCNUMBER, data[0].DOCTYPE, data[0].DOCYEAR, "SQ");
            }
            else if (data[0].ItemType == "004" && data[0].BUSINESSOBJECT == "PO") {
                $('.activity-panel-detail-purchase').show();

                loadActivityPODetail(data[0].DOCNUMBER, data[0].DOCTYPE, data[0].DOCYEAR, keyArr[1], keyArr[0], keyArr[2], keyArr[3]);
            }
            else if (data[0].ItemType == "004" && data[0].BUSINESSOBJECT == "PR")
            {
                //swith panel
                $(".div-businessobject-pr").show();
                $(".div-businessobj-default").hide();
                $("#divDetailHide").hide();
                $(".modeValue").hide();
                $(".div-default-Ticket").hide();
                //set value
                $("#btnResponse").html("<i class='fa fa-check'></i> อนุมัติ ");
                //$("#detail_other_delegatePR").html($("#detail_main_delegate").html() + "" + $("#detail_other_delegate").html());//ย้ายไปรวมกันในจังหวะ add detail_other_delegate
                $("#detail_datetime_startPR").html($("#detail_datetime_start").html());
                $("#detail_datetime_endPR").html($("#detail_datetime_end").html());
                $("#txtHiddenkeyDataPR").val('PR|'+data[0].DOCTYPE + '|' + data[0].DOCYEAR + '|' + data[0].DOCNUMBER);
                //set default flow
                $('.activity-panel-detail-default').show();
            }
            else {
                $('.activity-panel-detail-default').show();
            }
            //=============================================================

            if ((data[0].ItemType == "010" || data[0].ItemType == "011" || data[0].ItemType == "012") && data[0].DOCNUMBER != '')
            {
                var key = "";
                var href = "#";
                if (data[0].ItemType == "010") {
                    var _splitKey = data[0].DOCNUMBER.split('|');
                    var _sid = data[0].sid;
                    var _companyCode = data[0].companyCode;
                    var _doctype = data[0].DOCTYPE;
                    var _docnumber = _splitKey[0];
                    var _docyear = data[0].DOCYEAR;
                    var _production = _splitKey[1];
                    var _operation = _splitKey[2];
                    var _workcenter = _splitKey[3];

                    //JORD14OCT00019|14OCT00001|P500|S6
                    key = _sid + '|' + _companyCode + '|' + _doctype +'|'+_docnumber + '|' + _docyear + '|' + _production + '|' + _operation + '|' + _workcenter;
                    href = '/web/ProductionJobOrder/JobOrderDetail.aspx?KEY=' + key;
                }

                if (data[0].ItemType == "011") {
                    var _sid = data[0].sid;
                    var _companyCode = data[0].companyCode;
                    var _doctype = data[0].DOCTYPE;
                    var _docnumber = data[0].DOCNUMBER;
                    key = _sid + '|' + _companyCode + '|' + _doctype + '|' + _docnumber;
                    href = '/web/ProductionReservation/ProductionReservationDetail.aspx?KEY=' + key;
                }

                if (data[0].ItemType == "012") {
                    var _sid = data[0].sid;
                    var _companyCode = data[0].companyCode;
                    var _doctype = data[0].DOCTYPE;
                    var _docnumber = data[0].DOCNUMBER;
                    key = _sid + '|' + _companyCode + '|' + _doctype + '|' + _docnumber;
                    href = '/web/tfwarehousetooutsource/TFWTODetail.aspx?KEY=' + key;
                }              

                $('#refDocument').attr('href', href);
                $("#refDocumentName").html(data[0].JOBDESCRIPTION);
                $("#refDocumentRow").show();
            } else {
                $("#refDocumentRow").hide();
            }

            if (data[0].BUSINESSOBJECT == "PO" && data[0].DOCNUMBER != "") {

                var href = '/web/SupplyChain/POForCompanyDetail.aspx?mode=change&poNumber=' + data[0].DOCNUMBER + '&fiscalYear=' + data[0].DOCYEAR + '&poTypeCode=' + data[0].DOCTYPE + '&companyCode=' + data[0].companyCode;

                $('#refDocumentPO').attr('href', href);
                $("#refDocumentNamePO").html(data[0].JOBDESCRIPTION);
                $("#refDocumentRowPO").show();
            }
            else {
                $("#refDocumentRowPO").hide();
            }


            if (isLoadkm && typeof (initialKMafterSelectDocument) == "function") {
                initialKMafterSelectDocument('ACTIVITY', 'ACTIVITY', $("#txtHiddenAObjectlink").val(), '');
            }

            // ================== ACTIVITY CHAIN (Call User Control "/timeattendance/usercontrol/activitychain.ascx")
            try {
                ActivityChain(data[0].PROJECTCODE, data[0].DOCTYPE, data[0].DOCNUMBER, data[0].AOBJECTLINK);
            }
            catch (e) { }


            //================ call ativity menu control
            $(".activity-menu").click(function () {
                $(".activity-menu").removeClass("active");
                $(this).addClass("active");
                $(".content-container").hide();
                $("." + this.id).show();
                $('.nano').nanoScroller();
            });


            //================ Show right panel (activity detail)
            $(".tab-panel-main").hide();
            $(".tab-panel-activity").fadeIn();

            //================= Add row style
            var countRow = 0;
            $(".table-activity-detail tr").each(function () {
                if ($(this).is(":visible") && $(this).index() != 0) {
                    if (countRow % 2 == 0) {
                        $(this).css({
                            background: "#e8edf1"
                        });
                    }
                    else {
                        $(this).css({
                            background: "#bec8d0"
                        });
                    }
                    countRow++;
                }
            });

            //===================== Call Reminder Setting
            loadActivityDetailReminderSetting(data[0].AOBJECTLINK, data[0].COMPANYCODE);

            //===================== Call Attach File
            loadActivityDetailAttachFile(data[0].AOBJECTLINK, data[0].COMPANYCODE);

            //===================== Call Picture
            loadActivityDetailPicture(data[0].AOBJECTLINK, data[0].COMPANYCODE, data[0].JOBDESCRIPTION);

            //===================== set button send custom email
            $("#btnOpenSendCustomEmail").click(function () {
                sendCustomEmail(data[0].JOBDESCRIPTION, data[0].MYFULLNAME);
            });

            //===================== set button send custom email
            $("#btnOpenSendActivityEmail")
                .attr("send-mail-fullname", data[0].MYFULLNAME)
                .attr("send-mail-from", data[0].EMAIL_FROM)
                .click(function () {
                    sendActivityEmail(data[0].JOBDESCRIPTION, data[0].MYFULLNAME, data[0].EMAIL_FROM);
                });

            //===================== Set panel Add attach file
            closeAddAttach();

            $('.nano').nanoScroller();

            AGLoading(false);
            $("#btnReloadSystemMessage").click();

            //===================== mark read
            $.ajax({
                url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=markRead&aobjectid=' + data[0].AOBJECTLINK,
                success: function (data) {
                    if (ErrorAPIHandel(data))
                        return;
                }
            });


            //================= Edit Subject =============================================================
            $("#detail_subject_editor")
                //.toggle($("#txtHiddenYouAre").val() == "C")
                .unbind("click").bind("click", {
                aobj: data[0].AOBJECTLINK
            }, function (e) {
                var name = prompt("Change activity subject", $("#detail_subject").html());
                if (name != undefined && name != null && name.trim() != "" && name != $("#detail_subject").html()) {
                    AGLoading(true);
                    $.ajax({
                        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx',
                        data:{
                            q:"changeSubject",
                            aobj: e.data.aobj,
                            newSubject: name
                        },
                        type:"POST",
                        success: function (data) {
                            if (ErrorAPIHandel(data))
                                return;

                            if (data[0].Result == "S") {
                                $("[data-aobjectlink='" + data[0].AobjectLink + "'] .tab-menu-activity-name").html(data[0].NewSubject);
                                $("#detail_subject").html(data[0].NewSubject);
                                AGMessage("แก้ไขหัวข้อเรียบร้อยแล้ว");
                            }
                            else {
                                AGMessage("ไม่สามารถแก้ไขได้ เนื่องจากพบปัญหาในการบันทึก");
                            }
                            AGLoading(false);
                        }
                    });
                }
            });

            loadActivityURLToClipboard();


            //load more detail edit mode
            if ($("#btnLoadMoreDetailEditMode").length > 0 && $("#hddAobjForLoadMoreDetail").length > 0) {
                $("#hddAobjForLoadMoreDetail").val(data[0].AOBJECTLINK);
                $("#btnLoadMoreDetailEditMode").click();
            }
            //end load more detail edit mode

            //$("#btnLoadRevenue_Expense").click();
        }
    });   
}

function PriorityRangeChange(obj) {
    var ddl = $("#dllChangePriority");
    ddl.val("0" + obj.value);

    var desc = ddl.find("option:selected").text();
    if (desc.trim() == "")
        desc = "ไม่มีการกำหนด";

    $("#priority-range-text").html(desc);
}

function HasteRangeChange(obj) {

    var ddl = $("#dllChangeHaste");
    ddl.val("0" + obj.value);

    var desc = ddl.find("option:selected").text();
    if (desc.trim() == "")
        desc = "ไม่มีการกำหนด";

    $("#haste-range-text").html(desc);
}

function changeSubjectActivityRedesign() {
    AGLoading(true);
    var aObj = $("#hddAobjForLoadMoreDetail").val();
    var newSubject = $("#txtdetail_subject").val();
    var oldSubject = $("#detail_subject").html();
    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx',
        data: {
            q: "changeSubject",
            aobj: aObj,
            newSubject: newSubject,
            oldSubject: oldSubject
        },
        type: "POST",
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;

            if (data[0].Result == "S") {
                $("[data-aobjectlink='" + data[0].AobjectLink + "'] .tab-menu-activity-name").html(data[0].NewSubject);
                $("#detail_subject,#txtdetail_subject").html(data[0].NewSubject);
                AGMessage("Change subject successfully.");
            }
            else {
                AGMessage("Sorry,change subject failed.");
            }
            AGLoading(false);
        }
    });
}
function generateUUID() {
    var d = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
    return uuid;
};

function loadActivityDetailPicture(aobj, snaid, activitySubject) {
    $.ajax({
        url: servictWebDomainName + "TimeAttendance/TimeAttendance.ashx?q=taskpicture&obj=" + aobj,
        success: function (datas) {
            if (ErrorAPIHandel(datas))
                return;

            if (datas.length == 0) {
                $(".activity-gallery-empty").show();
                $(".activity-gallery").hide();
            }
            else {
                $(".activity-gallery-empty").hide();
                $(".activity-gallery").show();

                var container = $("<div/>", {
                    class: "row"
                });

                for (var i = 0; i < datas.length; i++) {
                    var imgCon = $("<div/>", {
                        class: "col-lg-3"
                    });

                    var img = $("<img/>", {
                        src: datas[i].PictureName,
                        alt: datas[i].created_on,
                        class: "activity-gallery-image",
                        css: {
                            width: "100%",
                            padding: 5,
                            border: "1px solid #ccc",
                            borderRadius: 5,
                            cursor: "pointer"
                        }
                    });

                    $(imgCon).append(img);
                    $(container).append(imgCon);
                    if ((i + 1) % 4 == 0)
                        $(container).append("<div style='clear:both'></div>");
                }

                $(".activity-gallery").html("").append(container);

                var allImg = [];
                $(".activity-gallery-image").each(function () {
                    allImg.push({
                        url: $(this).prop("src"),
                        detail: $(this).prop("alt")
                    });
                });

                $(".activity-gallery-image").each(function (index) {
                    $(this).click(function () {
                        $(".image-gallery-containe").remove();
                        var imgGallery = $("<div/>", {
                            class: "image-gallery-container"
                        });
                        $("body").append(imgGallery);
                        $(imgGallery).aGepeGallery({
                            images: allImg,
                            title: activitySubject,
                            index: index
                        });
                    });
                });
            }
        }
    });
}

function loadActivityDetailAttachFile(aobj, snaid) {
    $.ajax({
        url: servictWebDomainName + "TimeAttendance/TimeAttendance.ashx?q=taskattachfile&obj=" + aobj,
        success: function (datas) {
            if (ErrorAPIHandel(datas))
                return;

            $("#attachfile").html("");            
            $("#inv_attachfile").html("");
            $("#attachfilePO").html("");
            $("#pop_attachfile").html("");
            if (datas.length == 0) {
                $("#attachfile").append(
                       "<div style='color:#fa5949'>ไม่มีไฟล์แนบ</div>"
                );
                $("#detail_total_attach").html(0);

                $("#inv_attachfile").append(
                      "<div style='color:#fa5949'>ไม่มีไฟล์แนบ</div>"
               );
                $("#detail_inv_total_attach").html(0);

                $("#attachfilePO").append(
                      "<div style='color:#fa5949'>ไม่มีไฟล์แนบ</div>"
               );
                $("#detail_total_attachPO").html(0);
            }
            else {
                var maxFile = 3;
                var _tablePopup = "<table class='table table-striped table-hover table-bordered'>";
                _tablePopup += "<tr>";
                _tablePopup += "<th>ลบ</th>";
                _tablePopup += "<th>File Name</th>";
                _tablePopup += "<th>วันที่ เวลา</th>";
                _tablePopup += "<th>ขนาดไฟล์</th>";
                _tablePopup += "<th>uploader</th>";
                _tablePopup += "</tr>";


                for (var i = 0; i < datas.length; i++) {

                    var strDownloadElement = "";
                    strDownloadElement += "<a target='_blank' data-path='" + ("\\managefile\\" + datas[i].sid + datas[i].file_path) + "' data-name='" + datas[i].file_name + "' href='#' onclick='ActivityDownloadFileForm(this);return false;'>" + datas[i].file_name + "</a> ";
                    strDownloadElement += "<a target='_blank' class='activity-attachment-link' data-uploader='" + datas[i].created_by_name + "' data-file-name='" + datas[i].file_name + "' href='" + "\\managefile\\" + datas[i].sid + datas[i].file_path + "' style='color:#F1C513;'>(ตัวอย่าง)</a>";


                    _tablePopup += "<tr>";
                    _tablePopup += "<td>" + (datas[i].AllowDelete == "true" ? "<span style='color:red;cursor:pointer' onclick='deleteAttachFile(this,\"" + datas[i].key_object_link + "\");'>(ลบ)</span>" : "") + "</td>";
                    _tablePopup += "<td>" + strDownloadElement + "</td>";

                    _tablePopup += "<td>" + datas[i].created_on_display + "</td>";
                    _tablePopup += "<td>" + datas[i].display_size + "</td>";
                    _tablePopup += "<td>" + datas[i].created_by_name + "</td>";
                    _tablePopup += "</tr>";
                    
                    if (i > maxFile) {

                    }else
                    if (i == maxFile) {
                        var countMore = datas.length - maxFile;
                        $("#attachfile").append("<span style='color:#4FCE39;cursor:pointer' onclick='openShowAttachFile();'>+ more(" + countMore + ")</span>");
                        $("#inv_attachfile").append("<span style='color:#4FCE39;cursor:pointer' onclick='openShowAttachFile();'>+ more(" + countMore + ")</span>");
                        $("#attachfilePO").append("<span style='color:#4FCE39;cursor:pointer' onclick='openShowAttachFile();'>+ more(" + countMore + ")</span>");
                        continue;
                    } else {
                        $("#attachfile").append(
                            (i != 0 ? "" : "") +
                            strDownloadElement +
                            (datas[i].AllowDelete == "true" ? "<span style='color:red;cursor:pointer' onclick='deleteAttachFile(this,\"" + datas[i].key_object_link + "\");'>(ลบ)</span>" : "") +
                            "<br />"

                        );
                        $("#inv_attachfile").append(
                           (i != 0 ? "" : "") +
                           strDownloadElement +
                           (datas[i].AllowDelete == "true" ? "<span style='color:red;cursor:pointer' onclick='deleteAttachFile(this,\"" + datas[i].key_object_link + "\");'>(ลบ)</span>" : "") +
                           "<br />"

                       );
                        $("#attachfilePO").append(
                           (i != 0 ? "" : "") +
                           strDownloadElement +
                           (datas[i].AllowDelete == "true" ? "<span style='color:red;cursor:pointer' onclick='deleteAttachFile(this,\"" + datas[i].key_object_link + "\");'>(ลบ)</span>" : "") +
                           "<br />"

                       );
                    }
                }
                _tablePopup += "</table>";
                $("#pop_attachfile").append(_tablePopup);
                $("#detail_total_attach").html(datas.length);
                $("#detail_inv_total_attach").html(datas.length);
                $("#detail_total_attachPO").html(datas.length);
            }

        }
    });
}

function deleteAttachFile(obj,key) {
    if (AGConfirm(obj,"ต้องการลบไฟล์แนบนี้หรือไม่")) {
        $.ajax({
            url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=taskattachfiledelete&key=' + key,
            success: function (data) {
                if (ErrorAPIHandel(data))
                    return;
                AGMessage("ลบไฟล์แนบแล้ว");
                loadActivityDetailAttachFile($("#txtHiddenAObjectlink").val(), $("#txtHiddenCompanyCode").val());
            }
        });
    }
}

function loadActivityDetailBehavior() {
    $("#table-behavior").html("");
    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=taskbehavior&aobj=' + $("#txtHiddenAObjectlink").val(),
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;

            for (var i = 0; i < data.length; i++) {
                var Event = data[i].EVENT;
                if (data[i].EVENT == "CREATE")
                    Event = "สร้าง";
                if (data[i].EVENT == "CHECKOUT")
                    Event = "ปิดงาน";
                if (data[i].EVENT == "READED")
                    Event = "รับทราบ";
                if (data[i].EVENT == "SUCCESS")
                    Event = "เสร็จสิ้น";
                if (data[i].EVENT == "CANCELED")
                    Event = "ยกเลิก";
                if (data[i].EVENT == "REMARK")
                    Event = "เพิ่มเติมหมายเหตุ";
                if (data[i].EVENT == "RECALL")
                    Event = "ทวงถาม";
                if (data[i].EVENT == "ADD")
                    Event = "เพิ่มคุณเป็นผู้รับมอบหมาย";
                if (data[i].EVENT == "ADDOTHER")
                    Event = "เพิ่มผู้รับมอบหมายใหม่";
                if (data[i].EVENT == "RECREATE")
                    Event = "ยกเลิกและสร้างใหม่";
                if (data[i].EVENT == "ADDFILE")
                    Event = "แนบไฟล์เพิ่มเติม";
                if (data[i].EVENT == "EXTERNAL")
                    Event = "ส่งข้อมูล Activity ไปยังบุคคลภายนอก";
                if (data[i].EVENT == "CAUSE")
                    Event = "สรุปสาเหตุของปัญหา";
                if (data[i].EVENT == "SOLUTION")
                    Event = "สรุปวิธีการแก้ไขปัญหา";

                $("#table-behavior").append(
                    "<tr class='tr-behavior'>" +
                        "<td width='10%' align='center' valign='top'>" +
                            "<img id='img_contact' src='" + data[i].IMAGE + "' Width='40px' Height='40px' />" +
                        "</td>" +
                        "<td width='90%' valign='top'>" +
                            "<table width='100%' style='table-layout: fixed'>" +
                                "<tr>" +
                                    "<td style='width: 60%'>" +
                                        "<p style='color: rgb(0,32,96)'>" +
                                            "<b>" + data[i].CREATE_BY_NAME + "</b>" +
                                        "</p>" +
                                    "</td>" +
                                    "<td width='40%' align='right' valign='top'>" +
                                        "<span class='glyphicon glyphicon-time' style='color: #c8c7c7'></span> <span style='color: #c8c7c7'>" + data[i].CREATE_ON + "</span>" +
                                    "</td>" +
                                "</tr>" +
                            "</table>" +
                            "<p style='word-wrap: break-word;'>" +
                                "<span>" + Event + " " + data[i].SYSTEM_MESSAGE + "</span>" +
                            "</p>" +
                        "</td>" +
                    "</tr>"
                );
            }
            //================ Load nano scrollbar
            $('.nano').nanoScroller();
        }
    });
}

function loadActivityDetailRemark(objlink, myID, mainFullname, mainMessage, mainCreateOn, mainImage, mainCreateBy) {
    
    $("#remark-table").html("");
    var descTab = $("#description-table");
    descTab.html("");
    if (mainMessage != undefined && mainMessage != "") {
        descTab.activityRemarkBuilder({
            data: [{
                Key: "00",
                CreatedOn: mainCreateOn,
                CreatorFullname: mainFullname,
                MessageType: "DESCRIPTION",
                MessageText: mainMessage,
                Image: mainImage,
                CreatorLinkID: mainCreateBy,
                MyLinkID: myID
            }],
            aobjectlink: objlink,
            allowDivider: true,
            enableEditor: true,
            enableReply: false,
            pagemode: "ACTIVITY",
            onReplySuccess: function () {
                try {
                    $("html,body").scrollTop($("#remark-table .system-message-comment-container-remark").last().offset().top);
                } catch (e) { };
                $('.nano').nanoScroller();
                var _objlink = objlink;
                loadActivityDetailRemarkComment(_objlink);
            },
            postData: {
                url: servictWebDomainName + 'widget/SystemMessageFormAPI.aspx',
                key: {
                    type: "postremark",
                    aobj: objlink
                }
            },
            editData: {
                url: servictWebDomainName + "timeattendance/TimeAttendance.ashx",
                key: {
                    q: "editactivitydesc",
                    aobjectlink: objlink
                }
                //remarkKey : AUTO_POST
                //remarkMessage : AUTO_POST
            }
        });
    }
    
    loadActivityDetailRemarkComment(objlink);
}


function loadActivityDetailRemarkComment(objlink, container, callbackPickRemark, onRendered) { 
    var remarkContainer = container == undefined ? $("#remark-table") : $(container);
    remarkContainer.activityRemark({
        aobjectlink: objlink,
        pagemode: "ACTIVITY",
        allowDivider: true,
        isLazyLoad:true,
        onRendered: function () {
            if (onRendered != undefined) {
                onRendered();
            } else {
                if (isSendMailEvent) {
                    $("html,body").animate({
                        scrollTop: $("#remark-table .system-message-comment-container-remark").last().offset().top
                    }, 100);
                    isSendMailEvent = false;
                }
            }
        },
        onContinue: function () {
            $("html,body").animate({
                scrollTop: $("#remark-table .system-message-comment-container-remark").last().offset().top
            }, 100);
            $('.nano').nanoScroller();
        },
        getData: {
            url: servictWebDomainName + "framework/ag-activity-remark/api/",
            key: {
                q: "taskremark-lazyload",
                obj: objlink
            }
        },
        postData: {
            url: servictWebDomainName + 'framework/ag-activity-remark/api/',
            key: {
                type: "postremark",
                aobj: objlink
            }
            //isQuote : AUTO_POST
            //quoteMessage : AUTO_POST
            //quoteType : AUTO_POST
            //remarkMessage : AUTO_POST
            //remarkType : AUTO_POST
            //sendMail : AUTO_POST
        },
        editData: {
            url: servictWebDomainName + "framework/ag-activity-remark/api/",
            key: {
                q: "editremark"
            }
            //remarkKey : AUTO_POST
            //remarkMessage : AUTO_POST
        },
        getOnlineForm: {
            url: servictWebDomainName + "framework/ag-activity-remark/api/",
            key: {
                q: "getonlineform",
                aobj: objlink
            }
        },
        getRefEmailContent : {
            url: servictWebDomainName + 'widget/SystemMessageFormAPI.aspx',
            key: {
                type: "getrefemail",
                aobj: objlink
            }
            //refEmailCode : AUTO_POST
        },
        callBackRefEmailContent: function (datas) {
            var hiddenCodeBtn = $("#btnOpenSendActivityEmail");
            var fullName = hiddenCodeBtn.attr("send-mail-fullname");
            var mSubject = (datas.action == "REPLY" || datas.action == "FORWARD" ? "Re: " : "") + datas.subject;

            var mailTo = "";
            var mCC = extractEmails(datas.cc == undefined ? "" : datas.cc);
            var allCC = mCC == null ? [] : mCC;

            if (datas.action == "FORWARD") {
                var mTo = extractEmails(datas.to == undefined ? "" : datas.to);
                mailTo = mTo;
            } else {
                var mTo = extractEmails(datas.to == undefined ? "" : datas.to);
                if (mTo != null) {
                    allCC = allCC.concat(mTo)
                }
                mailTo = datas.from;
            }

            sendActivityEmail(mSubject, fullName, mailTo, allCC, {
                subject:mSubject,
                body:datas.detail
            });
        },
        callbackPickRemark: callbackPickRemark
    });
}

function extractEmails(text) {
    return text.match(/([a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\.[a-zA-Z0-9._-]+)/gi);
}

function loadActivityDetailRemarkEmail(objlink, myID, mainFullname, mainMessage, mainCreateOn, mainImage, mainCreateBy) {

    //document.getElementById("remark-email-table").innerHTML = "";


    //$.ajax({
    //    url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=taskemail&obj=' + objlink,
    //    success: function (data) {
    //        if (ErrorAPIHandel(data))
    //            return;

    //        for (var i = 0; i < data.length; i++) {
    //            var tab = document.getElementById("remark-email-table");
    //            var row = tab.insertRow(tab.rows.length);
    //            row.style.background = data[i].LINKID == myID ? "#F3FCBB" : "";
    //            var cell0 = row.insertCell(0);
    //            var cell1 = row.insertCell(1);
    //            cell0.className = "remark-header";
    //             cell1.innerHTML = '<p><b>' + data[i].EMAIL_FROM + '</b><span class="remark-datetime">' + data[i].CREATE_DATE + '</span></p><p><span>' + data[i].EMAIL_MESSAGE.split("\n").join("<br>") + '</span></p>';
    //          //  cell1.innerHTML = '<p><b>' + data[i].EMAIL_FROM + '</b><span class="remark-datetime">' + data[i].CREATE_DATE + '</span></p><p><textarea  rows="2" cols="20" style="height:100px;width:100%;resize: none">' + data[i].EMAIL_MESSAGE + '</textarea></p>';



    //        }
    //        //================ Load nano scrollbar
    //        $('.nano').nanoScroller();
    //    }
    //});
}

function startQuotation(doctype, docnumber, fiscalyear) {
    AGLoading(true);
    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=startQuotation&doctype=' + doctype + '&docnumber=' + docnumber + '&fiscalyear=' + fiscalyear,
        success: function (data) {
            if (ErrorAPIHandel(data)) {
                return;
            }
        }
    });
}

function loadActivityInvoiceDetail(docunumber,docytype,fiscalyaer,business)
{
    var _url = servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=getInvoiceDetail&artype=' + docytype + '&aryear=' + fiscalyaer + '&arnumber=' + docunumber;

    if (business == "SQ") {
        _url = servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=getQuotationDetail&doctype=' + docytype + '&fiscalyaer=' + fiscalyaer + '&docunumber=' + docunumber;
    }

    $.ajax({
        url: _url,
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            if (data.length > 0)
            {
                $('#hdfContactMode').val("invContact");

                if ($('#detail_invoice_type').length > 0) {
                    $('#detail_invoice_type').html(data[0].DocumentType);
                }
                if ($('#detail_invoice_year').length > 0) {
                    $('#detail_invoice_year').html(data[0].FiscalYear);
                }
                if ($('#detail_invoice_no').length > 0) {
                    $('#detail_invoice_no').html(data[0].DocumentNo);

                    if (business == "SQ") {
                        var a = $("<a/>", {
                            css: {
                                cursor: "pointer"
                            }
                        });
                        a.html(data[0].DocumentNo);
                        a.attr("data-doctype", data[0].DocumentType);
                        a.attr("data-docnumber", data[0].DocumentNo);
                        a.attr("data-fiscalyear", data[0].FiscalYear);
                        a.click(function () {
                            if (AGConfirm('ต้องการไปยังหน้าเอกสารใบเสนอราคาใช่หรือไม่ ?')) {
                                //startQuotation($(this).attr("data-doctype"), $(this).attr("data-docnumber"), $(this).attr("data-fiscalyear"));
                                AGLoading(true);
                                window.location.href = "/web/Quotation/QuotationLoading.aspx?doctype=" + $(this).attr("data-doctype") + "&fiscalyear=" + $(this).attr("data-fiscalyear") + "&docnumber=" + $(this).attr("data-docnumber");
                            }
                            return false;                           
                        });

                        $('#detail_invoice_no').html("");
                        $('#detail_invoice_no').append(a);
                    }
                }

                var _invoiceKey = data[0].DocumentType + "|" + data[0].FiscalYear + "|" + data[0].DocumentNo;
                $("#hdfInvoiceKey").val(_invoiceKey);

                if ($('#detail_customer_code').length > 0) {
                    $('#detail_customer_code').html(data[0].CustomerCode + ' - ' + data[0].CustomerNameF1);
                    $('#hdfInvoiceCustomerCode').val(data[0].CustomerCode);
                }
                if ($('#detail_customer_adddress').length > 0) {
                    $('#detail_customer_adddress').html(data[0].CusAddress);
                }
                if ($('#detail_invoice_amount').length > 0) {
                    $('#detail_invoice_amount').html(data[0].ShowValue);
                }
                if ($('#detail_invoice_currency').length > 0) {
                    $('#detail_invoice_currency').html(data[0].Currency);
                }
                if ($('#detail_invoice_date').length > 0) {
                    $('#detail_invoice_date').html(data[0].DocumentDateDisplay);
                }
                if ($('#detail_invoice_paymentterm').length > 0) {
                    if (data[0].CollectionTerm == '') {
                        $('#detail_invoice_paymentterm').html('ไม่ระบุ');
                    }
                    else {
                        $('#detail_invoice_paymentterm').html(data[0].CollectionTerm + ' - ' + data[0].CollectionTermText);
                    }
                }
                if ($('#detail_invoice_statusqt').length > 0) {
                    $('#detail_invoice_statusqt').html(data[0].StatusDesc);
                }
                if ($('#detail_invoice_startdate').length > 0) {
                    $('#detail_invoice_startdate').html(data[0].ShowStartDate);
                }
                if ($('#detail_invoice_duedate').length > 0) {

                    var _showEndDate = data[0].ShowEndDate;

                    if (parseInt(data[0].CurrentDate) > parseInt(data[0].EndDate)) {
                        _showEndDate = "<span style='color: red;'>" + data[0].ShowEndDate + "</span>";
                    }

                    $('#detail_invoice_duedate').html(_showEndDate);
                }
                if ($('#detail_invoice_overdue_day').length > 0) {
                    var x = '<span> ' + data[0].DueDay + ' </span> วัน';

                    if (parseInt(data[0].DueDay) < 0) {
                        x = '<span style="color: red;"> ' + data[0].DueDay + ' วัน</span>';
                    }

                    $('#detail_invoice_overdue_day').html(x);
                }

                if (business != "SQ") {
                    // add attribute btnInvoiceSendMail
                    $('#btnInvoiceSendMail').click(function () {
                        AGLoading(true);
                        $(".tab-panel-header").removeClass("tab-panel-active");
                        $("#tab-mailContent").addClass("tab-panel-active");
                        $(".tab-panel-container-content").hide();
                        $(".tab-mailContent").fadeIn();
                        $("#btnRefreshContactEmail").click();
                    });

                    if ($('#detail_customer_contact').length > 0) {

                        var edit = "<img src='" + servictWebDomainName + "images/icon/Edit.png' width='18px' height='18px' title='แก้ไขผู้ติดต่อ' style='cursor: pointer;margin-left: -30px;' onclick=\"openModalSearchContact();\"></img>";
                        var display = "<span style='padding-left: 10px;'>ไม่ระบุ</span>";

                        if (data[0].ContactName != null && data[0].ContactName != undefined && data[0].ContactName != "") {
                            display = "<a style='cursor: pointer; padding-left: 10px;' onclick=\"window.open('/web/master/MasterConfig/ContactMaster.aspx?id=" + data[0].ContactID + "');\">" + data[0].ContactName + "</a>";
                        }

                        $('#detail_customer_contact').html(edit + display);
                    }
                    if ($('#detail_customer_contact_tel').length > 0) {
                        if (data[0].ContactPhone == "") {
                            $('#detail_customer_contact_tel').html('ไม่ระบุ');
                        } else {
                            $('#detail_customer_contact_tel').html(data[0].ContactPhone);
                        }

                    }
                    if ($('#detail_customer_contact_email').length > 0) {

                        if (data[0].ContactEmail == "") {
                            $('#detail_customer_contact_email').html('ไม่ระบุ');
                        } else {
                            $('#detail_customer_contact_email').html(data[0].ContactEmail);
                        }
                    }
                    if ($('#detail_customer_contact_position').length > 0) {

                        if (data[0].ContactPosition == "") {
                            $('#detail_customer_contact_position').html('ไม่ระบุ');
                        } else {
                            $('#detail_customer_contact_position').html(data[0].ContactPosition);
                        }
                    }
                    if ($('#detail_customer_contact_department').length > 0) {

                        if (data[0].ContactDepartment == "") {
                            $('#detail_customer_contact_department').html('ไม่ระบุ');
                        } else {
                            $('#detail_customer_contact_department').html(data[0].ContactDepartment);
                        }
                    }
                    if ($('#detail_customer_contact_type').length > 0) {

                        if (data[0].ContactType == "") {
                            $('#detail_customer_contact_type').html('ไม่ระบุ');
                        } else {
                            $('#detail_customer_contact_type').html(data[0].ContactType);
                        }
                    }
                    if ($('#detail_customer_contact_behavior').length > 0) {

                        if (data[0].ContactBehavior == "") {
                            $('#detail_customer_contact_behavior').html('ไม่ระบุ');
                        } else {
                            $('#detail_customer_contact_behavior').html(data[0].ContactBehavior);
                        }
                    }                    

                    var status = data[0].Status;
                    var color = "green";

                    if (parseInt(data[0].DueDay) < 0) {
                        status = "Overdue";
                        color = "red";
                    }
                    if ($('#detail_invoice_status').length > 0) {
                        var x = '<span class="invoice-state" style=";color:' + color + '"> ' + status.toUpperCase() + ' </span>';
                        $('#detail_invoice_status').html(x);
                    }                    
                    if ($('#detail_customer_contact_chat').length > 0) {

                        if (data[0].ContactName != null && data[0].ContactName != undefined && data[0].ContactName != "") {
                            var chatIcon = "<i class='fa-left-menu fa fa-comments-o hand' style='font-size: 18px; padding-left: 8px;' title='Chat Box' onclick=\"window.open('/web/master/MasterConfig/ContactMaster.aspx?id=" + data[0].ContactID + "&panel=chat');\"></i>";
                            var chatCount = "";
                            if (parseInt(data[0].ChatCount) > 0) {
                                chatCount = "<small class='badge badge-count' style='margin: 0; position: absolute; margin-top: -8px;'>" + data[0].ChatCount + "</small>";
                            }

                            $('#detail_customer_contact_chat').html(chatIcon + chatCount);
                        }
                        else {
                            $('#detail_customer_contact_chat').html('');
                        }
                    }

                    $('#btnRefreshContactManagement').click();
                }                                                               
            }
        }
    });
}

function loadActivityPODetail(docunumber, docytype, fiscalyaer, datein, empcode, seq, snaid) {
    $.ajax({        
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=getPODetail&potype=' + docytype + '&poyear=' + fiscalyaer + '&ponumber=' + docunumber + '&DATEIN=' + datein + '&SEQ=' + seq + '&EMPCODE=' + empcode + '&SNAID=' + snaid,
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            if (data.length > 0) {
                $('#detail_create_byPO').html(data[0].OwnerName);
                $('#datail_complete_datePO').html(data[0].poDate);
                $('#detial_respones_ratePO').html(data[0].leadTime);
                $('#detail_used_timePO').html(data[0].created_on);

                if (data[0].CREATED_BY == data[0].EMPCODE) {
                    $('#detail_main_delegate').html("<span style='color:#000'>" + data[0].OwnerName + "</span>");
                }
                else {
                    var readedTextColor = "#333";
                    var readedTextTitle = "ยังไม่รับทราบ";

                    if (data[0].XSTATUS == "READED") {
                        readedTextTitle = "รับทราบแล้ว";
                        readedTextColor = "#0085A1";
                    }

                    if (data[0].XSTATUS == "SUCCESS") {
                        readedTextTitle = "เสร็จสิ้นแล้ว";
                        readedTextColor = "green";
                    }

                    if (data[0].OldMainDelegateNamePO != "") {
                        $('#old_detail_main_delegatePO').html(data[0].OldMainDelegateNamePO);
                        $('#old_detail_main_delegatePO_2').addClass("hasOldDelegate");
                        $('#old_detail_main_delegatePO_2').html(" => ");
                        $('#imgChangeMainDelegateHistoryPO').show();
                        $('#imgChangeMainDelegatePO').hide();
                        $('#divnonedelegate').show();
                    }
                    else {
                        $('#old_detail_main_delegatePO').html("");

                        if ($('#old_detail_main_delegatePO_2').hasClass("hasOldDelegate")) {
                            $('#old_detail_main_delegatePO_2').removeClass("hasOldDelegate");
                        }

                        $('#old_detail_main_delegatePO_2').html("");
                        $('#imgChangeMainDelegateHistoryPO').hide();
                        $('#imgChangeMainDelegatePO').show();
                        $('#divnonedelegate').hide();
                    }

                    $('#detail_main_delegatePO').html("<span title='" + readedTextTitle + "' style='color:" + readedTextColor + "'>" + data[0].MainDelegateNamePO + "</span>");
                }

                loadActivityPOTrackingStatus();
            }
        }
    });
}

function loadActivityPOTrackingStatus() {
    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=checkPOTrackingStatus&aobjectlink=' + $("#txtHiddenAObjectlink").val(),
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            if (data.length > 0) {

                var trueStatus = "ทำรายการแล้ว";
                var falseStatus = "ยังไม่ทำรายการ";
                var releaseStatus = "ยืนยันรายการแล้ว";

                if (data[0].xPOConrifm == "true") {
                    $("#poConfirmStatus").prop("title", trueStatus);
                    $("#poConfirmStatus").css("background-color", "yellow");
                    $("#poConfirmStatus").css("color", "black");
                }
                else {
                    $("#poConfirmStatus").prop("title", falseStatus);
                    $("#poConfirmStatus").css("background-color", "grey");
                    $("#poConfirmStatus").css("color", "white");
                }

                if (data[0].xPRDStart == "true") {
                    $("#prdStartStatus").prop("title", trueStatus);
                    $("#prdStartStatus").css("background-color", "yellow");
                    $("#prdStartStatus").css("color", "black");
                }
                else {
                    $("#prdStartStatus").prop("title", falseStatus);
                    $("#prdStartStatus").css("background-color", "grey");
                    $("#prdStartStatus").css("color", "white");
                }

                if (data[0].xPRDEnd == "true") {
                    $("#prdEndStatus").prop("title", trueStatus);
                    $("#prdEndStatus").css("background-color", "yellow");
                    $("#prdEndStatus").css("color", "black");
                }
                else {
                    $("#prdEndStatus").prop("title", falseStatus);
                    $("#prdEndStatus").css("background-color", "grey");
                    $("#prdEndStatus").css("color", "white");
                }

                if (data[0].xShipStart == "true") {
                    $("#shipStartStatus").prop("title", trueStatus);
                    $("#shipStartStatus").css("background-color", "yellow");
                    $("#shipStartStatus").css("color", "black");
                }
                else {
                    $("#shipStartStatus").prop("title", falseStatus);
                    $("#shipStartStatus").css("background-color", "grey");
                    $("#shipStartStatus").css("color", "white");
                }

                if (data[0].xShipEnd == "true") {
                    $("#shipEndStatus").prop("title", trueStatus);
                    $("#shipEndStatus").css("background-color", "yellow");
                    $("#shipEndStatus").css("color", "black");
                }
                else {
                    $("#shipEndStatus").prop("title", falseStatus);
                    $("#shipEndStatus").css("background-color", "grey");
                    $("#shipEndStatus").css("color", "white");
                }

                if (data[0].xGR == "true") {
                    if (data[0].xGRRelease == "true") {
                        $("#grConfirmStatus").prop("title", releaseStatus);
                        $("#grConfirmStatus").css("background-color", "green");
                        $("#grConfirmStatus").css("color", "white");
                    }
                    else {
                        $("#grConfirmStatus").prop("title", trueStatus);
                        $("#grConfirmStatus").css("background-color", "yellow");
                        $("#grConfirmStatus").css("color", "black");
                    }                    
                }
                else {
                    $("#grConfirmStatus").prop("title", falseStatus);
                    $("#grConfirmStatus").css("background-color", "grey");
                    $("#grConfirmStatus").css("color", "white");
                }

                if (data[0].xQC == "true") {
                    $("#qcConfirmStatus").prop("title", trueStatus);
                    $("#qcConfirmStatus").css("background-color", "yellow");
                    $("#qcConfirmStatus").css("color", "black");
                }
                else {
                    $("#qcConfirmStatus").prop("title", falseStatus);
                    $("#qcConfirmStatus").css("background-color", "grey");
                    $("#qcConfirmStatus").css("color", "white");
                }
            }
        }
    });
}

function loadActivityDetailParty(myID, createdBy, leader, ItemType) {
    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=taskparty&obj=' + $("#txtHiddenAObjectlink").val() + '&SNAID=' + $("#txtHiddenCompanyCode").val(),
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;

            var isAboutMe = createdBy == myID || leader == myID;
            if (data.length > 0) {
                $("#table_detail_other").show();
                $("#table_detail_other_empty").hide();
                var tab = document.getElementById("table_detail_other");
                tab.innerHTML = "";

                var strOtherDelegate = "";

                for (var i = 0; i < data.length; i++) {
                    var readedTextColor = "#333";
                    var readedTextTitle = "ยังไม่รับทราบ";
                    if (data[i].XSTATUS == "READED") {
                        readedTextTitle = "รับทราบแล้ว";
                        readedTextColor = "#049dbd";
                    }
                    if (data[i].XSTATUS == "SUCCESS") {
                        readedTextTitle = "เสร็จสิ้นแล้ว";
                        readedTextColor = "#049dbd";
                    }


                    strOtherDelegate += strOtherDelegate == "" ? "" : ",";
                    strOtherDelegate += "<span data-linkid='"+  data[i].OBJECT  +"' title='" + readedTextTitle + "' style='color:" + readedTextColor + "'>" + data[i].OTHER_FULLNAME + "</span>";
                    converJsonEmailTo(data[i].OTHER_FULLNAME, data[i].OBJECT);

                    var rowIndex = tab.rows.length;
                    var row = tab.insertRow(rowIndex);

                    var cell1 = row.insertCell(0);
                    cell1.style.width = "80px";
                    cell1.innerHTML = data[i].OBJECT;
                    cell1.style.display = "none";

                    var cell2 = row.insertCell(1);
                    //cell2.style.width = "220px";
                    cell2.innerHTML = data[i].OTHER_FULLNAME;


                    var cell3 = row.insertCell(2);
                    cell3.style.width = "120px";


                    var cell4 = row.insertCell(3);
                    cell4.innerHTML = "Delay : " + data[0].DELAY;
                    cell4.style.width = "120px";

                    var readedText = "<span style='color:orange'>ยังไม่รับทราบ</span>";
                    if (data[i].XSTATUS == "READED")
                        readedText = "<span style='color:green'>รับทราบแล้ว</span>";
                    if (data[i].XSTATUS == "SUCCESS")
                        readedText = "<span style='color:red'>เสร็จสิ้น</span>";

                    //================ Disable button for Other delegate
                    if (data[i].XSTATUS == "READED" && data[i].OBJECT.trim() == myID.trim())
                        $("#btnResponse").attr("disabled", "");
                    if (data[i].XSTATUS == "SUCCESS" && data[i].OBJECT.trim() == myID.trim()) {
                        $("#btnResponse").attr("disabled", "");
                        $("#btnSuccess").attr("disabled", "");
                    }

                    cell3.innerHTML = readedText;

                    if (data[i].OBJECT.trim() == myID.trim()) {
                        isAboutMe = true;
                    }
                }

                $("#detail_other_delegate").html(strOtherDelegate);
                $("#detail_other_delegatePO").html(strOtherDelegate);
                $("#detail_other_delegatePR").html($("#detail_main_delegate").html() + "," + strOtherDelegate);
                
                if (ItemType == "009" || ItemType == "013") {
                    $("#detail_inv_other_delegate").html(strOtherDelegate);
                }
            }
            else {
                $(".table_detail_other").hide();
                $("#table_detail_other_empty").show();
                $("#detail_other_delegate").html("<span style='color:#000'>ไม่มีผู้รับมอบหมายอื่น</span>");
                $("#detail_other_delegatePO").html("<span style='color:#000'>ไม่มีผู้รับมอบหมายอื่น</span>");
                if (ItemType == "009" || ItemType == "013") {
                    $("#detail_inv_other_delegate").html("<span style='color:#000'>ไม่มีผู้รับมอบหมายอื่น</span>");
                }
            }

            if (!isAboutMe) {
                $("#div-btn-event-container").hide();
            }
            else {
                $(".btnChangeOtherDetail").show();
            }

            $('.nano').nanoScroller();
        }
    });
}

function createCustomerClick() {
    if (AGConfirm("ต้องการไปยังหน้า สร้างข้อมูลหลักลูกค้า ใช่หรือไม่ ?")) {
        AGLoading(true);
        $("#btnCreateCustomer").click();
    }    
}

function loadActivityDetailSaleContact(aobjectlink) {
    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=getSaleContactDetail&aobjectlink=' + aobjectlink,
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            if (data.length > 0) {

                $('#hdfContactMode').val("saleContact");

                $("#saleTaskWin,#saleTaskLost").hide();
                if (data[0].TaskResult == "WIN")
                    $("#saleTaskWin").show();
                if (data[0].TaskResult == "LOST")
                    $("#saleTaskLost").show();

                if ((data[0].isTempCustomer.toString().toUpperCase() == "TRUE")) {
                    $('.saleTaskCustomerDetail').hide();

                    var btnCreateCustomer = "<i class='fa fa-user-plus' style='margin-right: 5px; font-size: 16px; cursor: pointer;' onclick='createCustomerClick()' title='Create Customer'></i>";

                    $('#sale_customer_detail').html(btnCreateCustomer + data[0].TempCustomerName);
                    $('#hdfInvoiceCustomerCode').val(data[0].TempCustomerName);
                }
                else {
                    $('.saleTaskCustomerDetail').show();

                    if ($('#sale_customer_detail').length > 0) {
                        $('#sale_customer_detail').html(data[0].CustomerCode + ' - ' + data[0].CustomerNameF1);
                        $('#hdfInvoiceCustomerCode').val(data[0].CustomerCode);
                    }

                    if ($('#sale_customer_address').length > 0) {
                        $('#sale_customer_address').html(data[0].CusAddress);
                    }

                    if ($('#sale_customer_contact').length > 0) {

                        var edit = "<img src='" + servictWebDomainName + "images/icon/Edit.png' width='18px' height='18px' title='แก้ไขผู้ติดต่อ' style='cursor: pointer;margin-left: -30px;' onclick=\"openModalSearchContact();\"></img>";
                        var display = "<span style='padding-left: 10px;'>ไม่ระบุ</span>";

                        if (data[0].ContactName != null && data[0].ContactName != undefined && data[0].ContactName != "") {
                            display = "<a style='cursor: pointer; padding-left: 10px;' onclick=\"window.open('/web/master/MasterConfig/ContactMaster.aspx?id=" + data[0].ContactID + "');\">" + data[0].ContactName + "</a>";
                        }

                        $('#sale_customer_contact').html(edit + display);
                    }

                    if ($('#sale_customer_contact_chat').length > 0) {

                        if (data[0].ContactName != null && data[0].ContactName != undefined && data[0].ContactName != "") {
                            var chatIcon = "<i class='fa-left-menu fa fa-comments-o hand' style='font-size: 18px; padding-left: 8px;' title='Chat Box' onclick=\"window.open('/web/master/MasterConfig/ContactMaster.aspx?id=" + data[0].ContactID + "&panel=chat');\"></i>";
                            var chatCount = "";
                            if (parseInt(data[0].ChatCount) > 0) {
                                chatCount = "<small class='badge badge-count' style='margin: 0; position: absolute; margin-top: -8px;'>" + data[0].ChatCount + "</small>";
                            }

                            $('#sale_customer_contact_chat').html(chatIcon + chatCount);
                        }
                        else {
                            $('#sale_customer_contact_chat').html('');
                        }
                    }

                    if ($('#sale_customer_position').length > 0) {

                        if (data[0].ContactPosition == "") {
                            $('#sale_customer_position').html('ไม่ระบุ');
                        } else {
                            $('#sale_customer_position').html(data[0].ContactPosition);
                        }
                    }

                    if ($('#sale_customer_department').length > 0) {

                        if (data[0].ContactDepartment == "") {
                            $('#sale_customer_department').html('ไม่ระบุ');
                        } else {
                            $('#sale_customer_department').html(data[0].ContactDepartment);
                        }
                    }

                    if ($('#sale_customer_phone').length > 0) {
                        if (data[0].ContactPhone == "") {
                            $('#sale_customer_phone').html('ไม่ระบุ');
                        } else {
                            $('#sale_customer_phone').html(data[0].ContactPhone);
                        }

                    }
                    if ($('#sale_customer_mail').length > 0) {

                        if (data[0].ContactEmail == "") {
                            $('#sale_customer_mail').html('ไม่ระบุ');
                        } else {
                            $('#sale_customer_mail').html(data[0].ContactEmail);
                        }
                    }

                    $('#btnRefreshContactManagement').click();
                }
            }
        }
    });
}

$.fn.replaceSign = function () {
    $(this).val($(this).val().split("'").join("").split('"').join(''));
}

function replaceSC(str) {
    var result = "";
    for (var i = 0; i < str.length; i++) {
        result += str[i].replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, '').split(' ').join('');
    }
    return result;
};

function replaceOnlySign(str) {
    var result = "";
    for (var i = 0; i < str.length; i++) {
        result += str[i].replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, '');
    }
    return result;
};

function openAddAttach() {
    document.getElementById("iframeAddAttachFile").src = "/widget/PopupAddAttachFile.aspx?" +
                                                        "aobj=" + $("#txtHiddenAObjectlink").val() +
                                                        "&snaid=" + $("#txtHiddenCompanyCode").val() +
                                                        "&rowkey=" + $("#txtHiddenRowkey").val();
    $('.list-attach').hide();
    $('.add-attach').fadeIn();
}

function closeAddAttach() {
    $('.list-attach').fadeIn();
    $('.add-attach').hide();
}

function inboxClick() {
    $(".search-all-activity,.select-activity-client-filter,.select-activity-type-filter").val("");
    $(".select-activity-client-filter").change();
    $("#main-form-filter-header").html("Inbox").focusElementByColor();
    $("#main-form-filter-code").html("");
    $("#main-form-filter-subproject-code").html("");
}

function afterRemark() {
    $("#txthiddenSendEmailForEditActivity").val("false");
    $("#txt_detail_remark").val("");
}

function closeActivityDetial() {
    //$("#tab-panel-main").click();
    //$("#tab-panel-activity").hide();
}

$.fn.focusElementByColor = function () {
    var elt = $(this);
    setTimeout(function () {
        $(elt).css("color", "red");
        setTimeout(function () {
            $(elt).css("color", "black");
            setTimeout(function () {
                $(elt).css("color", "red");
                setTimeout(function () {
                    $(elt).css("color", "black");
                    setTimeout(function () {
                        $(elt).css("color", "red");
                        setTimeout(function () {
                            $(elt).css("color", "black");
                        }, 500);
                    }, 500);
                }, 500);
            }, 500);
        }, 500);
    },500);
    
};

// for iframe create activity
function openIfraneCreateActivity(type, aobj, rowkey, snaid) {

    var src = "";
    if (aobj != undefined)
        src = '/widget/PopupCreateActivityRedesign.aspx?type=' + type + '&aobj=' + aobj + '&rowkey=' + rowkey + '&snaid=' + snaid + "&mode=recreate";
    else
        src = '/widget/PopupCreateActivityRedesign.aspx?project=' + $("#main-form-filter-code").html();

    if (window.parent.length > 0) {
        window.parent.createActivityStart(src);
    } else {
        var ifca = document.getElementById("iframeCreateActivity");
        ifca.src = src;
    }
}

function openIframeCreateQuickNote() {
    var ifca = document.getElementById("iframeCreateActivity");

    ifca.src = '/widget/PopupCreateActivityReDesign.aspx?mode=quicknote&project=' + $("#main-form-filter-code").html();
}

function openIframeCreateQuickTask() {
    var ifca = document.getElementById("iframeCreateActivity");

    ifca.src = '/widget/PopupCreateActivityReDesign.aspx?mode=quicktask&project=' + $("#main-form-filter-code").html();
}

function openIframeCreateNote() {
    var ifca = document.getElementById("iframeCreateActivity");

    ifca.src = '/widget/PopupCreateActivityReDesign.aspx?mode=createNote&project=' + $("#main-form-filter-code").html();
}

function openIframeCreateSaleTask() {
    var ifca = document.getElementById("iframeCreateActivity");

    ifca.src = '/widget/PopupCreateActivityReDesign.aspx?mode=createSales&project=' + $("#main-form-filter-code").html();
}

function openIframeCreateTask() {
    var ifca = document.getElementById("iframeCreateActivity");

    ifca.src = '/widget/PopupCreateActivityReDesign.aspx?mode=createTask&project=' + $("#main-form-filter-code").html();
}

function openIframeCreateMeeting() {
    var ifca = document.getElementById("iframeCreateActivity");

    ifca.src = '/widget/PopupCreateActivityReDesign.aspx?mode=createMeeting&project=' + $("#main-form-filter-code").html();
}

function openIframeCreateSalesTask() {
    var ifca = document.getElementById("iframeCreateActivity");

    ifca.src = '/widget/PopupCreateActivityReDesign.aspx?mode=createSales&project=' + $("#main-form-filter-code").html();
}


function closeIfraneCreateActivity() {
    var ifca = document.getElementById("iframeCreateActivity");
    $(ifca).fadeOut();
}

function showIfraneCreateActivity() {
    var ifca = document.getElementById("iframeCreateActivity");
    $(ifca).fadeIn();
}

function successIfraneCreateActivity(aobj,oldAobj) {
    AGMessage("บันทึกเรียบร้อยแล้ว");
    closeIfraneCreateActivity();
    closeActivityDetial();

    refreshAllNotification();
    StartActivityInbox();
}

//for iframe select other delegate
function openIfraneSelectOtherDelegate() {
    var aobj = $("#txtHiddenAObjectlink").val();
    var rowkey = $("#txtHiddenRowkey").val();
    var snaid = $("#txtHiddenCompanyCode").val();
    var ifca = document.getElementById("iframeSelectOtherDelegate");
    ifca.src = '/TimeAttendance/PopupSelectOtherDelegate.aspx?aobj=' + aobj + '&rowkey=' + rowkey + '&snaid=' + snaid;
}

function closeIfraneSelectOtherDelegate() {
    var ifca = document.getElementById("iframeSelectOtherDelegate");
    $(ifca).fadeOut();
}

function showIfraneSelectOtherDelegate() {
    var ifca = document.getElementById("iframeSelectOtherDelegate");
    $(ifca).fadeIn();
}

function successIfraneSelectOtherDelegate() {
    AGMessage("บันทึกเรียบร้อยแล้ว");
    closeIfraneSelectOtherDelegate();
    loadActivityDetail($("#txtHiddenAObjectlink").val());
}

//for ifram change detail
function openIframeChangeOtherDetail() {
    var aobj = $("#txtHiddenAObjectlink").val();
    var ifca = document.getElementById("iframeChangeOtherDetail");
    ifca.src = '/TimeAttendance/PopupChangeOtherDetail.aspx?aobj=' + aobj;
}

function closeIframeChangeOtherDetail() {
    var ifca = document.getElementById("iframeChangeOtherDetail");
    $(ifca).fadeOut();
}

function showIfranmChangeOtherDetail() {
    var ifca = document.getElementById("iframeChangeOtherDetail");
    $(ifca).fadeIn();
}

function successIframeChangeOtherDetail() {
    AGMessage("บันทึกเรียบร้อยแล้ว");
    closeIframeChangeOtherDetail();
    loadActivityDetail($("#txtHiddenAObjectlink").val());
    CallbackActivityEvent("other");
}

//for ifram change task status
function openIframeChangeTaskStatus(aobjectLink) {
    var aobj = aobjectLink == undefined ? $("#txtHiddenAObjectlink").val() : aobjectLink;
    var ifca = document.getElementById("iframeChangeTaskStatus");
    ifca.src = '/TimeAttendance/PopupChangeActivityDetail.aspx?aobj=' + aobj;
}

function closeIframeChangeTaskStatus() {
    var ifca = document.getElementById("iframeChangeTaskStatus");
    $(ifca).fadeOut();
}

function showIfranmChangeTaskStatus() {
    var ifca = document.getElementById("iframeChangeTaskStatus");
    $(ifca).fadeIn();
}

function successIframeChangeTaskStatus() {
    AGMessage("บันทึกเรียบร้อยแล้ว");
    //closeIframeChangeTaskStatus();//zaan-re
    if ($("#txtHiddenRowkey").val() != "")
        loadActivityDetail($("#txtHiddenAObjectlink").val());
    
    CallbackActivityEvent("taskstatus");
}

//for iframe select main delegate
function openIfraneSelectMainDelegate() {
    var aobj = $("#txtHiddenAObjectlink").val();
    var rowkey = $("#txtHiddenRowkey").val();
    var snaid = $("#txtHiddenCompanyCode").val();
    var ifca = document.getElementById("iframeSelectMainDelegate");
    ifca.src = '/TimeAttendance/PopupSelectMainDelegate.aspx?aobj=' + aobj + '&rowkey=' + rowkey + '&snaid=' + snaid;
}

function closeIfraneSelectMainDelegate() {
    var ifca = document.getElementById("iframeSelectMainDelegate");
    $(ifca).fadeOut();
}

function showIfraneSelectMainDelegate() {
    var ifca = document.getElementById("iframeSelectMainDelegate");
    $(ifca).fadeIn();
}

function successIfraneSelectMainDelegate(newAobj, newRowKey,oldAobj) {
    var oldAobj = $("#txtHiddenAObjectlink").val();
    
    $("#txtHiddenRowkey").val(newRowKey);
    $("#txtHiddenAObjectlink").val(newAobj);

    AGMessage("บันทึกเรียบร้อยแล้ว");
    closeIfraneSelectMainDelegate();
    //SwitchActivityCookie(oldAobj, newAobj, newRowKey);
    //loadActivityDetail($("#txtHiddenRowkey").val()); 
    loadActivityDetail($("#txtHiddenAObjectlink").val());
    CallbackActivityEvent("maindelegate");
}

//for iframe change history main delegate
function openIframeChangeMainDelegateHistory() {
    var aobj = $("#txtHiddenAObjectlink").val();

    var ifca = document.getElementById("iframeChangeMainDelegateHistory");
    ifca.src = '/TimeAttendance/PopupChangeMainDelegateHistory.aspx?aobj=' + aobj;
}

function closeIframeChangeMainDelegateHistory() {
    var ifca = document.getElementById("iframeChangeMainDelegateHistory");
    $(ifca).fadeOut();
}

function showIframeChangeMainDelegateHistory() {
    var ifca = document.getElementById("iframeChangeMainDelegateHistory");
    $(ifca).fadeIn();
}

function successIframeChangeMainDelegateHistory(newAobj, newRowKey) {
    $("#txtHiddenRowkey").val(newRowKey);
    $("#txtHiddenAObjectlink").val(newAobj);

    AGMessage("บันทึกเรียบร้อยแล้ว");
    closeIframeChangeMainDelegateHistory();
    loadActivityDetail($("#txtHiddenAObjectlink").val());
}

function sendActivityEmail(subject, myName, emailTo,emailCC, replyContent) {
    subject = subject == "" ? $("#detail_subject").html() : subject;
    $('#txtSubjectSendMail').val(subject);
    $("#divContainerEmail,#divContainerEmail_CC").html("");

    if (typeof (emailTo) == "string")
        emailTo = [emailTo];

    for (var i = 0; i < emailTo.length; i++) {
        if (emailTo[i] != "") {
            $("#txtAddEmail").val(emailTo[i]);
            $('#btnAddEmail').click();
        }
    }
    
    
    $("#txtRemarkSendMail").val("");
    $('#txtFromSendMail').val(myName);
    $("#labelTypeSendEmail").html("Activity");
    $("#hidTypeSendEmail").val("ACTIVITY");
    startModelSendEmail();

    $("#modelSendEmail .send-mail-file-badge").html(0).attr("data-number", 0);
    $("#modelSendEmail .activity-email-ref-source-files-box").html("");
    $("#modelSendEmail #txtAllSendMailRefFiles").val("");
    // Prepare value
    sendMailPrepareAllCode();

    if (emailCC != undefined) {
        var pushedCC = [];
        for (var i = 0; i < emailCC.length; i++) {
            var pushed = $.inArray(emailCC[i].toLowerCase(), pushedCC) > -1;
            var isInList = false;
            for (var j = 0; j < emailTo.length; j++) {
                if (emailTo[j].toLowerCase() == emailCC[i].toLowerCase() || pushed) {
                    isInList = true;
                    break;
                }
            }
            if (isInList)
                continue;

            pushedCC.push(emailCC[i].toLowerCase());
            $("#txtAddEmail_CC").val(emailCC[i]);
            $('#btnAddEmail_CC').click();
        }
    }

    if (replyContent != undefined && replyContent.body != "") {
        var prepareSentMail = prepareSendmailList();
        $('#modelSendEmail').unbind("shown.bs.modal").bind("shown.bs.modal", function () {
            var linkSignature = "<br><hr>";
            linkSignature += "<b>From:</b> Focusone Link (Reply mail system)<br>";
            //linkSignature += "<b>To:</b> " + prepareSentMail.to + "<br>";
            //linkSignature += "<b>CC:</b> " + prepareSentMail.cc + "<br>";
            linkSignature += "<b>Subject:</b> " + replyContent.subject + "<br>";
            linkSignature += "<br>";
            $("#modelSendEmail .note-editable").html(linkSignature + replyContent.body).focus();
        });
    } else {
        $('#modelSendEmail').unbind("shown.bs.modal").bind("shown.bs.modal", function () {
            $("#modelSendEmail .note-editable").html("").focus();
        });
    }
}

function sendCustomEmail(subject, myName) {
    startModelSendEmail();

    $('#txtSubjectSendMail').val(subject);
    $("#divContainerEmail").html("");
    $("#txtAddEmail").val("");

    $("#txtRemarkSendMail").val("");
    $('#txtFromSendMail').val(myName);
    $("#labelTypeSendEmail").html("Custom");
    $("#hidTypeSendEmail").val("CUSTOM");

    // Prepare value
    //sendMailPrepareAllCode();
}

function sendMailPrepareAllCode() {
    $(".txtHiddenAObjectlink_MAIL").val($("#txtHiddenAObjectlink").val());
    $(".txtHiddenCompanyCode_MAIL").val($("#txtHiddenCompanyCode").val());
    $(".txtHiddenRowkey_MAIL").val($("#txtHiddenRowkey").val());
}

function addMailObjectContainer(prefix,containerClass,mailClass,value,removeFunction) {
    var container = $("<div/>", {
        class: "col-md-4 form-group " + containerClass
    });
    var box = $("<div/>", {
        class: "one-line",
        css: {
            padding: 5,
            paddingRight: 20,
            paddingLeft: 15,
            border: "1px solid #ccc",
            marginTop: 5,
            borderRadius: 5,
            background: "#f7f7f7"
        },
        title: value,
        html: prefix
    });
    box.appendTo(container);
    var mailBox = $("<span/>", {
        class: mailClass,
        html: value
    });
    mailBox.appendTo(box);

    var remove = $("<i/>", {
        class:"fa fa-remove",
        css: {
            cursor: "pointer",
            position: "absolute",
            right: 15,
            top: 14
        },
        click: function () {
            removeFunction(this);
        }
    });
    remove.appendTo(box);

    return container;
}

function addMailToContainer() {
    if ($("#txtAddEmail").val().trim() != '') {
        $("#divEmail").show();

        var mailObject = addMailObjectContainer(
            "To: ",
            "to-email-list",
            "container-email-to",
            $("#txtAddEmail").val(),
            removeToContainerEmail
        );

        $("#divContainerEmail").append(mailObject);
    }
    $("#txtAddEmail").val("");
}

function addMailCCContainer() {
    if ($("#txtAddEmail_CC").val().trim() != '') {
        $("#divEmail_CC").show();

        var mailObject = addMailObjectContainer(
            "CC: ",
            "cc-email-list",
            "container-email-cc",
            $("#txtAddEmail_CC").val(),
            removeCCContainerEmail
        );

        $("#divContainerEmail_CC").append(mailObject);
    }
    $("#txtAddEmail_CC").val("");
}

function removeToContainerEmail(obj) {
    $(obj).closest(".to-email-list").remove();
    if ($(".container-email-to").length == 0) {
        $("#divEmail").hide();
    }
}

function removeCCContainerEmail(obj) {
    $(obj).closest(".cc-email-list").remove();
    if ($(".container-email-cc").length == 0) {
        $("#divEmail_CC").hide();
    }
}

function prepareSendmailList() {
    var mail = [];
    $(".container-email-to").each(function () {
        mail.push($(this).html());
    });
    $("#txtAllSendMailContainer").val(mail);

    var cc = [];
    $(".container-email-cc").each(function () {
        cc.push($(this).html());
    });
    $("#txtAllSendMailContainer_CC").val(cc);

    return {
        to: mail.join(';'),
        cc: cc.join(';')
    }
}

function prepareSendmail() {
    if ($(".container-email-to").length == 0) {
        AGMessage("กรุณาเพิ่มอีเมลที่คุณจะส่งถึง");
        return false;
    }
    if (confirm("ยืนยันจะส่งอีเมลนี้หรือไม่")) {
        prepareSendmailList();
        AGLoading(true);
    }
    else
        return false;

}

function successSendCustomEmail() {
    isSendMailEvent = true;
    AGMessage("ส่งอีเมลเรียบร้อยแล้ว");
    $('#modelSendEmail').modal('hide');
}

function ShowMap(latitude, longitude, updatedBy, updatedOn) {
    $("#modal-lat-value").html(latitude == "" ? "Not Set" : latitude);
    $("#modal-long-value").html(longitude == "" ? "Not Set" : longitude);

    var url = "/web/master/agriculturistmaster/report/ReportActionerResultPopupMap.aspx?lattitude=" + latitude + "&longtitude=" + longitude;
    $('.modalmap-title').html('แผนที่สถานที่ สำรวจโดย ' + updatedBy + ' [' + updatedOn + '] ');
    $('.modalmap-body').html('<iframe width="100%" height="100%"  frameborder="0" style="left: -320px; top: -220px" scrolling="yes" allowtransparency="true" src="' + url + '"></iframe>');


    $('#modalGoogleMap').modal('toggle');

    $('#modalGoogleMap').modal('show');

    return false;
}

function openModalChangeMoney() {
    $("#modal-money").modal("show");
    $("#txtMoneyRevenue").val($("#detail_money_revenue").html());
    $("#txtMoneyCharges").val($("#detail_money_charges").html());
    $("#txtMoneyCurrency").val($(".detail_money_currency:first").html());
}

function SaveActivityMony() {

    if (!confirm("ต้องการบันทึกหรือไม่"))
        return;

    var rev = $("#txtMoneyRevenue").val();
    var char = $("#txtMoneyCharges").val();
    var cur = $("#txtMoneyCurrency").val();
    var key = $("#txtHiddenAObjectlink").val()
    var snaid = $("#txtHiddenCompanyCode").val()

    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=activitymoney&key=' + key + '&SNAID=' + snaid + '&revenue=' + rev + '&charges=' + char + '&currency=' + cur,
        success: function (datas) {
            if (ErrorAPIHandel(datas))
                return;

            alert("บันทึกข้อมูลเสร็จสิ้นแล้ว");
            $("#detail_money_revenue").html(datas[0].Revenue);
            $("#detail_money_charges").html(datas[0].Charges);
            $(".detail_money_currency").html(datas[0].Currency);
            $("#modal-money").modal("hide");
        }
    });
}

function viewTimeline(type) {
    if (type == "multimedia") {
        $("#btnViewTimeLine").click();
    }
    else {
        $("#btnViewTimelineMap").click();
    }
}

function viewTimelineModal() {
    $("#popup-timeline").modal("show");
}

function closeTimeline() {
    $('#popup-timeline').modal("hide");
}

function changeOtherDelegate() {
    prepareChangeOtherDelegate();
    var aobjectlink = $("#txtHiddenAObjectlink").val();
    var linkid = $('#txtChangeOtherDelegate').val();
    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=changeOtherDelegate&aobjectid=' + aobjectlink + '&listLinkID=' + linkid,
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            $('#formAddDelegate').modal('hide');

            loadActivityDetail($("#txtHiddenAObjectlink").val());
            AGMessage("&#3610;&#3633;&#3609;&#3607;&#3638;&#3585;&#3612;&#3641;&#3657;&#3617;&#3629;&#3610;&#3627;&#3617;&#3634;&#3618;&#3629;&#3639;&#3656;&#3609;&#3648;&#3619;&#3637;&#3618;&#3610;&#3619;&#3657;&#3629;&#3618;&#3649;&#3621;&#3657;&#3623;");
        }
    });
}

function getChangeOtherDelegate() {
    var aobjectid = $("#txtHiddenAObjectlink").val();
    var parameter = "?q=getOtherDelegate&aobjectid=" + aobjectid;

    var _url = servictWebDomainName + "/TimeAttendance/TimeAttendance.ashx" + parameter

    $.ajax({
        url: _url,
        success: function (dataReport) {
            if (ErrorAPIHandel(dataReport))
                return;
            for (var i = 0; i < dataReport.length; i++) {
                addChangeactivityOtherDelegateWithOutDelete(dataReport[i].LINKID, dataReport[i].FullName_TH, dataReport[i].RTYPE, this);
            }
        }
    });

    $("#trChangeActivityOtherDelegate").fadeIn();
}

function openGvSearchChangeActivityOtherDelegate() {
    $("#gvChangeActivityAddOtherDelegate").html("");
    $("#trChangeActivityAddOtherDelegate").fadeIn();
    $("#trChangeActivityOtherDelegate").hide();
}

function closeGvSearchChangeActivityOtherDelegate() {
    $("#gvChangeActivityAddOtherDelegate").html("");
    $("#trChangeActivityAddOtherDelegate").hide();
    $("#trChangeActivityOtherDelegate").fadeIn();
    alertAcceptCh = false;
}

function cancelChangeActivityOtherDelegate() {
    $("#gvChangeActivityAddOtherDelegate").html("");
    $("#trChangeActivityAddOtherDelegate").hide();
    $("#trChangeActivityOtherDelegate").fadeIn();
    $("#tableChangeOtherDelegate").html("");
    $("#gvChangeActivityAddOtherDelegate").html("");
}

function prepareChangeOtherDelegate() {

    $("#txtChangeOtherDelegate").val("");
    var tab = document.getElementById("tableChangeOtherDelegate");
    var allId = "";
    for (var i = 0; i < tab.rows.length; i++) {
        if (allId == "")
            allId = tab.rows[i].cells[0].innerHTML.trim() + "|" + tab.rows[i].cells[3].innerHTML.trim();
        else
            allId += "," + tab.rows[i].cells[0].innerHTML.trim() + "|" + tab.rows[i].cells[3].innerHTML.trim();
    }
    $("#txtChangeOtherDelegate").val(allId);
}

var alertAcceptCh = false;

function alertAcceptOther() {
    if ($("#tableChangeOtherDelegate tr").length > 0 && alertAcceptCh) {
        if ($("#btnAcceptChangeAddOther").hasClass("btn-success"))
            $("#btnAcceptChangeAddOther").removeClass("btn-success").addClass("btn-warning");
        else
            $("#btnAcceptChangeAddOther").addClass("btn-success").removeClass("btn-warning");
    }
    else
        $("#btnAcceptChangeAddOther").removeClass("btn-warning").removeClass("btn-success").addClass("btn-success");

    setTimeout(function () {
        alertAcceptOther();
    }, 500);
}

//alertAcceptOther();

function openModalSearchContact() {
    AGLoading(true);
    $(".tab-panel-header").removeClass("tab-panel-active");
    $("#tab-changeContact").addClass("tab-panel-active");
    $(".tab-panel-container-content").hide();
    $(".tab-changeContact").fadeIn();
    $("#btnSearchContact").click();     
}

function confirmChangeContact() {
    var contactMode = $('#hdfContactMode').val();
    var contact = $(".input-group-contact-code-activityEditContactCtrl").val();

    if (contact == "" || contact == undefined) {
        AGError("กรุณาเลือกผู้ติดต่อ");
        return;
    }

    if (contactMode == "saleContact") {
        var aobjectlink = $("#txtHiddenAObjectlink").val();
        $.ajax({
            url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=confirmChangeContactSale&aobjectlink=' + aobjectlink + '&contact=' + contact,
            success: function (data) {
                if (ErrorAPIHandel(data))
                    return;
                $('#formChangeContact').modal('hide');

                loadActivityDetailSaleContact(aobjectlink);
                AGSuccess("แก้ไขผู้ติดต่อสำเร็จ");
            }
        });
    }
    else if (contactMode == "invContact") {
        var invkey = $("#hdfInvoiceKey").val();
        $.ajax({
            url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=confirmChangeContactInvoice&invkey=' + invkey + '&contact=' + contact,
            success: function (data) {
                if (ErrorAPIHandel(data))
                    return;
                $('#formChangeContact').modal('hide');

                loadActivityDetail($("#txtHiddenAObjectlink").val());
                AGSuccess("แก้ไขผู้ติดต่อสำเร็จ");
            }
        });
    }    
}

function continueSearchProject() {
    $("#divSearchProject").show();
    $("#divShowProject").hide();
}

function cancelSearchProject() {
    $("#divSearchProject").hide();
    $("#divShowProject").fadeIn();
}

function openSearchProject() {
    $("#gvProject").html("");
    $("#divSearchProject").fadeIn();
    $("#divShowProject").hide();
}

function selectProject(id, name, obj) {
    id = id.trim();
    name = name.trim();
    $("#txtProjectName").val(name);
    $(".btn-select-project").removeClass("btn-primary");
    $(".btn-select-project").addClass("btn-success");
    $(obj).removeClass("btn-success");
    $(obj).addClass("btn-primary");
    $("#ddlProject").val(id);
    $('#btnSearchSubProject').click();
    cancelSearchProject();
}

function openSubProjectHierarchy(e) {
    $(document).click(function () {
        $(".pane-subproject-container").hide();
    });
    $(".pane-subproject-container").fadeIn();
    e.stopPropagation();
}

function hierarchyLoading() {
    removeLoading();
    $("body,html").css({
        cursor: "wait"
    });
    var loading = $("<div/>", {
        class: "hierarchy-loading",
    });
    $(loading).append($("<img/>", {
        src: servictWebDomainName + "images/loadmessage.gif"
    }));
    $(loading).append($("<label/>", {
        html: "Loading hierarchy..",
        css: {
            marginLeft: 10
        }
    }));
    $(".pane-subproject-container").append(loading);
}

function removeLoading() {
    $("body,html").css({
        cursor: "default"
    });
    $(".hierarchy-loading").remove();
}

function bindHierarchy() {
    hierarchyLoading();
    var ProjectCode = $("#ddlProject").val();
    var apiUrl = servictWebDomainName + "web/master/masterconfig/ProjectConfigDetailAPI.aspx";
    $.ajax({
        url: apiUrl,
        data: {
            request: "list",
            ProjectCode: ProjectCode
        },
        success: function (datas) {
            $(".pane-subproject").aGapeTreeMenu({
                data: datas,
                rootText: $("#txtProjectName").val(),
                onlyFolder: false,
                share: false,
                moveItem: false,
                selecting: false,
                emptyFolder: false,
                onClick: function (result) {
                    if (result.id != "") {
                        $("#txtChangeFolderSubProject").val(result.id);
                        $(".pane-subproject-container").hide();
                        $("#btnChangeSubProject").click();
                    }
                }
            });
            removeLoading();
        }

    });
}

function confirmChangeProject() {
    var aobjectlink = $("#txtHiddenAObjectlink").val();
    var proj = $('#ddlProject').val();
    var subproj = $('#ddlSubProject').val();

    if (proj == "") {
        AGMessage("กรุณาเลือกกลุ่มงาน");
        return;
    }

    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=confirmChangeProject&aobjectid=' + aobjectlink + '&proj=' + proj + '&subproj=' + subproj,
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            $('#formChangeProject').modal('hide');

            loadActivityDetail($("#txtHiddenAObjectlink").val());
            AGMessage("แก้ไขกลุ่มงานสำเร็จ");
        }
    });
}

function updateTK() {
    var aobjectlink = $("#txtHiddenAObjectlink").val();
    var tk_no = $("#txtticket").val();
    var tk_detail = $("#txtticketdetail").val();
    var tk_ref1 = $("#txtTK_ref1").val();
    var tk_ref2 = $("#txtTK_ref2").val();
    var tk_ref3 = $("#txtTK_ref3").val();
    var tk_ref4 = $("#txtTK_ref4").val();
    var tk_ref5 = $("#txtTK_ref5").val();
    var postDatas = {
        aobjectid: aobjectlink,
        tk_no: tk_no,
        tk_detail: tk_detail,
        tk_ref1: tk_ref1,
        tk_ref2: tk_ref2,
        tk_ref3: tk_ref3,
        tk_ref4: tk_ref4,
        tk_ref5: tk_ref5
    };
    $.ajax({
        type: "POST",
        url: servictWebDomainName + "TimeAttendance/TimeAttendance.ashx?q=ChangeTicket",
        data : postDatas,
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            $('#modalticket').modal('hide');
            loadActivityDetail($("#txtHiddenAObjectlink").val());
            AGMessage("แก้ไข Ticket สำเร็จ");
        }
    });
}

function showModalKM() {
    //$('#popupKM').attr('src','#');
    $('.popupREFKM').modal('show');
}

function autoSaveKM(obj) {
    if (AGConfirm(obj, "คุณต้องการบันทึก KM ใช่หรือไม่ ?")) {
        //$('.popupREFKM').find("iframe").contentWindow
        $('.popupREFKM').find("#popupKM")[0].contentWindow.autoSaveKM();
        return true;
    }
    return false;
}
function callBackSaveKM(status, msg) {
    AGLoading(false);
    if (status) {
        AGSuccess(msg);
    } else {
        AGError(msg);
    }
}

function closeModalKM() {
    $('.popupREFKM').modal('hide');
    $("#kmShortcut").css({
        color: "#398439",
        borderColor: "#398439"
    });
}

function initialKMafterSelectDocument(refBusiness, refDocType, refDocNumber, refDocYear) {
    var src = "/web/KM/REFKMPage.aspx";
    $('#popupKM').attr('src', src + '?business=' + refBusiness + '&doctype=' + refDocType + '&docnumber=' + refDocNumber + '&docyear=' + refDocYear + '&autosave=true');
}

function animateProjectRightPanel() {
    if ($("#animate-project-panel").hasClass("animate-hide")) {
        $("#animate-project-panel").animate({
            right: 0
        });
        $("#animate-project-panel").removeClass("animate-hide");

        if ($("#animate-activity-panel").hasClass("animate-hide")) {

        }
        else {
            $("#animate-activity-panel").animate({
                right: "-70%"
            });
            $("#animate-activity-panel").addClass("animate-hide");
        }
    }
    else {
        $("#animate-project-panel").animate({
            right: "-70%"
        });
        $("#animate-project-panel").addClass("animate-hide");
    }
}

function animateActivityRightPanel() {
    if ($("#animate-activity-panel").hasClass("animate-hide")) {

        //get data                
        var _url = servictWebDomainName + "TimeAttendance/TimeAttendance.ashx?q=getAvtivityCount";
        $.ajax({
            url: _url,
            success: function (dataReport) {
                if (ErrorAPIHandel(dataReport))
                    return;
                for (var i = 0; i < dataReport.length; i++) {
                    if (dataReport[i].Key == "All") {
                        $(".rightAllActivity").html(dataReport[i].Value.toString());
                    }
                    if (dataReport[i].Key == "NotRespond") {
                        $(".rightNotRespond").html(dataReport[i].Value.toString());
                    }
                    if (dataReport[i].Key == "NotFinish") {
                        $(".rightNotFinish").html(dataReport[i].Value.toString());
                    }
                    if (dataReport[i].Key == "Late") {
                        $(".rightWorkLate").html(dataReport[i].Value.toString());
                    }
                    if (dataReport[i].Key == "WaitForClose") {
                        $(".rightWaitForClose").html(dataReport[i].Value.toString());
                    }
                }
            }
        });

        $("#animate-activity-panel").animate({
            right: 0
        });
        $("#animate-activity-panel").removeClass("animate-hide");

        if ($("#animate-project-panel").hasClass("animate-hide")) {

        }
        else {
            $("#animate-project-panel").animate({
                right: -300
            });
            $("#animate-project-panel").addClass("animate-hide");
        }
    }
    else {
        $("#animate-activity-panel").animate({
            right: -300
        });
        $("#animate-activity-panel").addClass("animate-hide");
    }
}

function leftPanelTabClick(obj) {
    $(".left-panel-tab").removeClass("left-panel-tab-active");
    $(obj).addClass("left-panel-tab-active");
    $(".left-panel-tab-content-place").hide();
    $("." + $(obj).attr("id")).fadeIn();
}

function linkToActivity(rowkey, snaid, aobj) {
    if (aobj != "") {
        $("#txtHiddenRowkey").val(rowkey);
        $("#txtHiddenAObjectlink").val(aobj);
        $("#txtHiddenCompanyCode").val(snaid);
        loadActivityDetail(aobj);
    }
}

function rightPanelClick(status) {
    animateWorksInnerContainer();
    setTimeout(function () {
        $("#tab-panel-main").click();
        $("#main-form-filter-code").html("");
        $("#main-form-filter-subproject-code").html("");
        $("#main-form-filter-header").html("Inbox");

        $('.search-all-activity,.select-activity-type-filter').val("");
        $('.select-activity-client-filter').val(status);
        $(".select-activity-client-filter").change();
        StartActivityInbox();
    },300);
}

var _blinkerFlag = true;
function blinker() {
    _blinkerFlag = !_blinkerFlag;
    $('.reOpen-flash').css({
        color: (_blinkerFlag ? "#000" : "red")
    }, 800);
    setTimeout(function () {
        blinker();
    },800);
}

function momClick() {
    var _aobjectlink = $('#txtHiddenAObjectlink').val();
    window.open('/TimeAttendance/MinutesOfMeeting.aspx?aobjectlink=' + _aobjectlink);
}

blinker();

function searchChangeOtherDelegateLinkTeam(val) {
    var tab = document.getElementById("gvChangeActivityAddOtherDelegate");
    if (val.trim() != "") {
        var isMatch = false;
        for (var i = 1; i < tab.rows.length; i++) {
            var val1 = tab.rows[i].cells[1].innerHTML;
            var val2 = tab.rows[i].cells[2].innerHTML;
            var val3 = tab.rows[i].cells[3].innerHTML;
            var val4 = tab.rows[i].cells[4].innerHTML;
            if (val1.match(val) || val2.match(val) || val3.match(val) || val4.match(val)) {
                $(tab.rows[i]).show();
                isMatch = true;
            }
            else {
                $(tab.rows[i]).hide();
            }
        }
        if (!isMatch)
            AGMessage("&#3652;&#3617;&#3656;&#3614;&#3610;&#3619;&#3634;&#3618;&#3585;&#3634;&#3619;&#3607;&#3637;&#3656;&#3588;&#3657;&#3609;&#3627;&#3634;");
    }
}

function disibleChangeActivityButtonAddOtherDelegate() {
    var tab = document.getElementById("tableChangeOtherDelegate");
    for (var i = 0; i < tab.rows.length; i++) {
        //$("." + tab.rows[i].cells[0].innerHTML.trim() + "CAOD").attr("disabled", "");
        $("." + tab.rows[i].cells[0].innerHTML.trim() + "CAOD").removeClass("btn-success").addClass("btn-primary");
        $("." + tab.rows[i].cells[0].innerHTML.trim() + "CAOD").val('ลบ');
    }
    $("." + $("#txtHiddenUserLinkID").val() + "CAOD").attr("disabled", "");

    var aobjectid = $("#txtHiddenAObjectlink").val();
    var parameter = "?q=disabledLINKID&aobjectid=" + aobjectid;
    var _url = servictWebDomainName + "TimeAttendance/TimeAttendance.ashx" + parameter
    $.ajax({
        url: _url,
        success: function (dataReport) {
            if (ErrorAPIHandel(dataReport))
                return;
            for (var i = 0; i < dataReport.length; i++) {
                $("." + dataReport[i].LINKID + "CAOD").attr("disabled", "");
            }
        }
    });
}

function addChangeactivityOtherDelegateWithOutDelete(id, name, type, obj) {
    alertAcceptCh = true;
    var tab = document.getElementById("tableChangeOtherDelegate");
    for (var i = 0; i < tab.rows.length; i++) {
        if (id == tab.rows[i].cells[0].innerHTML) {
            AGMessage(name + " &#3617;&#3637;&#3619;&#3634;&#3618;&#3594;&#3639;&#3656;&#3629;&#3629;&#3618;&#3641;&#3656;&#3651;&#3609;&#3619;&#3634;&#3618;&#3585;&#3634;&#3619;&#3649;&#3621;&#3657;&#3623;");
            return false;
        }

    }
    var rowIndex = tab.rows.length;
    var row = tab.insertRow(rowIndex);
    row.id = "rowCAOD" + id;
    row.style.color = "#444";
    row.className = "newRowOtherDelegate";
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    var cell3 = row.insertCell(2);
    var cell4 = row.insertCell(3);
    cell1.innerHTML = id;
    cell1.className = "OtherDelegateID";
    cell1.style.display = "none";
    cell2.innerHTML = name;
    //cell3.innerHTML = "";
    cell3.innerHTML = "<a style='color:red' href='#' onclick='removeRowOtherDelegate(\"" + id + "\");'>เอาออก</a>";
    cell3.colSpan = 2;
    cell4.innerHTML = type;
    cell4.style.display = "none";
    $(obj).attr("disabled", "");
}

function addChangeactivityOtherDelegate(id, name, type, obj) {
    if ($(obj).hasClass('btn-primary')) {
        removeRowOtherDelegate(id);
        //$(obj).removeClass("btn-primary").addClass("btn-success");
        //$(obj).val('เพิ่ม');
    } else {
        alertAcceptCh = true;
        var tab = document.getElementById("tableChangeOtherDelegate");
        for (var i = 0; i < tab.rows.length; i++) {
            if (id == tab.rows[i].cells[0].innerHTML) {
                AGMessage(name + " &#3617;&#3637;&#3619;&#3634;&#3618;&#3594;&#3639;&#3656;&#3629;&#3629;&#3618;&#3641;&#3656;&#3651;&#3609;&#3619;&#3634;&#3618;&#3585;&#3634;&#3619;&#3649;&#3621;&#3657;&#3623;");
                return false;
            }

        }
        var rowIndex = tab.rows.length;
        var row = tab.insertRow(rowIndex);
        row.id = "rowCAOD" + id;
        row.style.color = "#444";
        row.className = "newRowOtherDelegate";
        var cell1 = row.insertCell(0);
        var cell2 = row.insertCell(1);
        var cell3 = row.insertCell(2);
        var cell4 = row.insertCell(3);
        cell1.innerHTML = id;
        cell1.className = "OtherDelegateID";
        cell1.style.display = "none";
        cell2.innerHTML = name;
        cell3.innerHTML = "<a style='color:red' href='Javascript:;' onclick='removeRowOtherDelegate(\"" + id + "\");'>เอาออก</a>";
        cell3.colSpan = 2;
        cell4.innerHTML = type;
        cell4.style.display = "none";
        //$(obj).attr("disabled", "");
        $(obj).removeClass("btn-success").addClass("btn-primary");
        $(obj).val('ลบ');
    }
}

function removeRowOtherDelegate(id) {
    $("." + id + "CAOD").removeClass("btn-primary").addClass("btn-success");
    $("." + id + "CAOD").val('เพิ่ม');
    id = "rowCAOD" + id;
    var tr = document.getElementById(id);
    if (tr) {
        if (tr.nodeName == 'TR') {
            var tbl = tr; // Look up the hierarchy for TABLE
            while (tbl != document && tbl.nodeName != 'TABLE') {
                tbl = tbl.parentNode;
            }

            if (tbl && tbl.nodeName == 'TABLE') {
                while (tr.hasChildNodes()) {
                    tr.removeChild(tr.lastChild);
                }
                tr.parentNode.removeChild(tr);
            }
        } else {
            alert('Specified document element is not a TR. id=' + id);
        }
    } else {
        alert('Specified document element is not found. id=' + id);
    }
}

function callIframeChangeSaveFile() {
    var aobj = $("#txtHiddenAObjectlink").val();
    var snaid = $("#txtHiddenUserSNAID").val();
    document.getElementById('changeAttachFileFrame').contentWindow.saveFile(aobj, snaid,true);
    //$('#formAddAttachFile').modal('hide');
}

function callIFrameNewInitFile() {
    document.getElementById('changeAttachFileFrame').contentWindow.initNewAttachFile();
    //$('#formAddAttachFile').modal('show'); //zaan-re
}
function successCreateActivity() {
    loadActivityDetail($("#txtHiddenAObjectlink").val());
    AGMessage("&#3648;&#3614;&#3636;&#3656;&#3617;&#3652;&#3615;&#3621;&#3660;&#3649;&#3609;&#3610;&#3648;&#3619;&#3637;&#3618;&#3610;&#3619;&#3657;&#3629;&#3618;&#3649;&#3621;&#3657;&#3623;");
    var iframe = document.getElementById('changeAttachFileFrame');
    iframe.src = iframe.src;
    displayActivitDetailMode($("#btnSaveAttachFileForActivity"), false);
}

// reminder 
function checkedChangeBox(i, id) {
    if (document.getElementById(id).checked) {
        document.getElementById("inputDay" + i).disabled = false;
        document.getElementById("inputHour" + i).disabled = false;
        document.getElementById("inputMinute" + i).disabled = false;
    }
    else {
        document.getElementById("inputDay" + i).disabled = true;
        document.getElementById("inputHour" + i).disabled = true;
        document.getElementById("inputMinute" + i).disabled = true;
    }
}
function checkedChangeDate(id, iddate, i) {
    if (document.getElementById(id).checked) {
        document.getElementById(iddate).disabled = false;
        $("#SpenRemove" + i).css('pointer-events', 'auto');
        $("#SpenCalendar" + i).css('pointer-events', 'auto');
    }
    else {
        document.getElementById(iddate).disabled = true;
        $("#SpenRemove" + i).css('pointer-events', 'none');
        $("#SpenCalendar" + i).css('pointer-events', 'none');
    }
}

//for iframe select Thanks
function openIfraneSelectThanks(elt) {
    var linkID = $(elt).attr("data-linkid");
    var aobj = $("#txtHiddenAObjectlink").val();
    var snaid = $("#txtHiddenCompanyCode").val();
    var createBy = $("#detail_create_by").attr("data-createBy");
    var myLinkID = $("#detail_create_by").attr("data-myLinkID");
    if(createBy == myLinkID && createBy != linkID)
    {
        var idivThank = document.getElementById("Dthanks");
        var ifca = document.getElementById("iframeSelectThanks");
        ifca.src = '/TimeAttendance/PopupSelectThanks.aspx?aobj=' + aobj + '&snaid=' + snaid + '&linkID=' + linkID;
        $(ifca).fadeIn();
        AGLoading(true);
    }
    else
    {
        AGError("Thanksได้เฉพาะเจ้าของงาน");
    }
}

function closeIfraneSelectThanks() {
    AGLoading(false);
    var ifca = document.getElementById("iframeSelectThanks");
    var idivThank = document.getElementById("Dthanks");
    $(idivThank).fadeOut();
    $(ifca).fadeOut();
}

function showIfraneSelectThanks() {
    var ifca = document.getElementById("iframeSelectThanks");
    var idivThank = document.getElementById("Dthanks");
    $(idivThank).fadeIn();
    $(ifca).fadeIn();
    AGLoading(false);
}

function successIfraneSelectThanks(newAobj, newRowKey,oldAobj) {
    var oldAobj = $("#txtHiddenAObjectlink").val();
    
    $("#txtHiddenRowkey").val(newRowKey);
    $("#txtHiddenAObjectlink").val(newAobj);

    AGMessage("บันทึกเรียบร้อยแล้ว");
    closeIfraneSelectThanks();
    //SwitchActivityCookie(oldAobj, newAobj, newRowKey);
    //loadActivityDetail($("#txtHiddenRowkey").val());
    CallbackActivityEvent("maindelegate");
}
//

//for iframe select ServiceCall
function openIframeServiceCall(elt) {
    var aobj = $("#txtHiddenAObjectlink").val();
    var idivServiceCall = document.getElementById("divServiceCall");
    var ifca = document.getElementById("iframeServiceCall");
    var crmUrl = $("#hddCrmURL").val();
    var spider = $("#hddSpiderData").val();
        ifca.src = crmUrl + '?aobj=' + aobj + spider;
        $(idivServiceCall).modal('show');
        $(ifca).fadeIn();
}

function closeIframeServiceCall() {
    var ifca = document.getElementById("iframeServiceCall");
    var idivServiceCall = document.getElementById("divServiceCall");
    $(idivServiceCall).modal('hide');
    $(ifca).fadeOut();
}

function showIframeServiceCall() {
    var ifca = document.getElementById("iframeServiceCall");
    var idivServiceCall = document.getElementById("divServiceCall");
    $(idivServiceCall).modal('show');
    $(ifca).fadeIn();
}
//

function reminderSetitingClick() {
    openReminderSetting();
    $(".form_datetime_remind").datetimepicker({
        format: "dd/mm/yyyy hh:ii",
        autoclose: true,
        todayBtn: true,
        minuteStep: 10
    });
}

function openReminderSetting() {
    $('#popup-reminder-setting').modal("show");
}

function closeReminderSetting() {
    $('#popup-reminder-setting').modal("hide");
}

function loadActivityDetailReminderSetting(aobj, snaid) {
    $.ajax({
        url: servictWebDomainName + "TimeAttendance/TimeAttendance.ashx?q=taskremindersetting&key=" + aobj + "&snaid=" + snaid,
        success: function (datas) {


            if (datas.length > 0) {

                $('chkUseRemindOwner').prop('checked', (datas[0].UseRemindOwner == 'True' ? true : false));
                $('chkUseRemindMainResponse').prop('checked', (datas[0].UseRemindMainResponse == 'True' ? true : false));
                $('chkUseRemindOtherResponse').prop('checked', (datas[0].UseRemindOtherResponse == 'True' ? true : false));

                if (datas[0].UseBeforeStart == 'True' || datas[0].UseBeforeStartCustomDate == 'True'
                    || datas[0].UseBeforeEnd == 'True' || datas[0].UseBeforeEndCustomDate == 'True') {
                    $("#reminderSettingShortcut").css("color", "#F70808");
                } else {
                    $("#reminderSettingShortcut").css("color", "");
                }


                $('#chkUseBeforeStart').prop('checked', (datas[0].UseBeforeStart == 'True' ? true : false));
                checkedChangeBox(1, 'chkUseBeforeStart');
                $('#inputDay1').val(datas[0].BeforeStartDay == '' ? '0' : datas[0].BeforeStartDay);
                $('#inputHour1').val(datas[0].BeforeStartHour == '' ? '0' : datas[0].BeforeStartHour);
                $('#inputMinute1').val(datas[0].BeforeStartMinute == '' ? '0' : datas[0].BeforeStartMinute);
                
                $('#chkUseBeforeStartCustomDate').prop('checked', (datas[0].UseBeforeStartCustomDate == 'True' ? true : false));
                checkedChangeDate('chkUseBeforeStartCustomDate', 'txtBeforeStartCustomDate', 1);

                $('#txtBeforeStartCustomDate').val(datas[0].BeforeStartCustomDateDisplay);
                $('#chkUseBeforeEnd').prop('checked', (datas[0].UseBeforeEnd == 'True' ? true : false));
                checkedChangeBox(2, 'chkUseBeforeEnd');
                $('#inputDay2').val(datas[0].BeforeEndDay == '' ? '0' : datas[0].BeforeEndDay);
                $('#inputHour2').val(datas[0].BeforeEndHour == '' ? '0' : datas[0].BeforeEndHour);
                $('#inputMinute2').val(datas[0].BeforeEndMinute == '' ? '0' : datas[0].BeforeEndMinute);
                $('#chkUseBeforeEndCustomDate').prop('checked', (datas[0].UseBeforeEndCustomDate == 'True' ? true : false));
                checkedChangeDate('chkUseBeforeEndCustomDate', 'txtBeforeEndCustomDate', 2);
                $('#txtBeforeEndCustomDate').val(datas[0].BeforeEndCustomDateDisplay);
                $('#chkUseRepeater').prop('checked', (datas[0].UseRepeater == 'True' ? true : false));
                checkedChangeBox(3, 'chkUseRepeater');
                $('#inputDay3').val(datas[0].RepeaterEveryDay == '' ? '0' : datas[0].RepeaterEveryDay);
                $('#inputHour3').val(datas[0].RepeaterEveryHour == '' ? '0' : datas[0].RepeaterEveryHour);
                $('#inputMinute3').val(datas[0].RepeaterEveryMinute == '' ? '0' : datas[0].RepeaterEveryMinute);

            } else {
                $("#reminderSettingShortcut").css("color", "");
                $('chkUseRemindOwner').prop('checked', true);
                $('chkUseRemindMainResponse').prop('checked', true);
                $('chkUseRemindOtherResponse').prop('checked', true);
                $('#chkUseBeforeStart').prop('checked', false);
                $('#inputDay1').val('0');
                $('#inputHour1').val('0');
                $('#inputMinute1').val('0');

                $('#chkUseBeforeStartCustomDate').prop('checked', false);
                $('#txtBeforeStartCustomDate').val('');
                $('#chkUseBeforeEnd').prop('checked', false);
                $('#inputDay2').val('0');
                $('#inputHour2').val('0');
                $('#inputMinute2').val('0');
                $('#chkUseBeforeEndCustomDate').prop('checked', false);
                $('#txtBeforeEndCustomDate').val('');
                $('#chkUseRepeater').prop('checked', false);
                $('#inputDay3').val('0');
                $('#inputHour3').val('0');
                $('#inputMinute3').val('0');
            }

           

        }
    });
}

var myFolder_RowDragKey = null;
var myFolder_RowDragTitle = null;
var apiUrlMyFolder = servictWebDomainName + "TimeAttendance/API/MyFolderAPI.aspx";
function bindHierarchyMyFolder(targetContainer, chooseMode, callbackChooseMode, callbakAfterLoad) {
    if (isSpecialDisplayPopup) {
        return;
    }
    var container = targetContainer == undefined ? $("#myfolder") : $(targetContainer);
    container.AGWhiteLoading(true, "<i>Loading My Folder</i>");
    $.ajax({
        url: apiUrlMyFolder,
        data: {
            request: "get_myfolder"
        },
        success: function (datas) {
            container.aGapeTreeMenu({
                data: datas,
                rootText: "Inbox",
                rootCode: "",
                rootCount: 0,
                navigateText: "Create structure",
                onlyFolder: false,
                share: false,
                moveItem: false,
                selecting: false,
                emptyFolder: true,
                onClick: function (result) {
                    if (chooseMode && callbackChooseMode != undefined) {
                        callbackChooseMode(result);
                    } else {
                        container.find(".agape-show-node-name").removeClass("focus-color");
                        if (result.id != "") {
                            container.find(".agape-hidden-node-id").closest(".agape-tree-li[data-node-id='" + result.id + "']")
                                .find(".agape-show-node-name").addClass("focus-color");
                        } else {
                            container.find(".agape-tree-li-root").find(".agape-show-node-name:first").addClass("focus-color");
                        }
                        hierarchyMyFolderClick(result.id);
                    }
                },
                onRename: chooseMode ? undefined : function (result) {
                    hierarchyMyFolderDoAjax({
                        request: "rename",
                        id: result.id,
                        name: result.name
                    });
                },
                onNewFolder: chooseMode ? undefined : function (result) {
                    hierarchyMyFolderDoAjax({
                        request: "newfolder",
                        parentid: result.parentid,
                        name: result.name
                    });
                },
                onDelete: chooseMode ? undefined : function (result) {
                    hierarchyMyFolderDoAjax({
                        request: "delete",
                        id: result.id
                    });
                }
            });

            if (callbakAfterLoad != undefined)
            {
                callbakAfterLoad();
            } else {
                if ($("#txtHierarchyMyFolderSelectedNodeID").val() == "") {
                    container.find(".agape-tree-li-root").find(".agape-show-node-name:first").addClass("focus-color");
                }
                else {
                    container.find(".agape-hidden-node-id").closest(".agape-tree-li[data-node-id='" + $("#txtHierarchyMyFolderSelectedNodeID").val() + "']")
                               .find(".agape-show-node-name").addClass("focus-color");
                }
            }

            $("#myfolder").AGWhiteLoading(false);
            $("#myfolder").find(".agape-show-node-name")
            .unbind("dragover").bind("dragover", function (e) {
                $(this).css({
                    outline: "2px dashed #E81123"
                });
                e.preventDefault();
            })
            .unbind("dragleave").bind("dragleave", function (e) {
                $(this).css("outline", "none");
            })
            .unbind("drop").bind("drop", function (e) {
                var tar = $(this).closest("li").attr("data-node-id");
                var aobj = myFolder_RowDragKey == null ? "" : myFolder_RowDragKey;
                $(this).css("outline", "none");
                $("#txtChooseFolderHierarchyCode").val(tar);
                $("#txtChooseFolderAobjectlink").val(aobj);
                $("#btnSaveChooseFolder").click();
                e.preventDefault();
                e.stopPropagation();
            });
        }
    });
}

function hierarchyMyFolderDoAjax(datas) {
    $.ajax({
        url: apiUrlMyFolder,
        data: datas,
        success: function () {
            bindHierarchyMyFolder();
        },
        error: function () {
            bindHierarchyMyFolder();
        }
    });
}

function hierarchyMyFolderClick(nodeId) {
    $("#tab-panel-main").click();

    $("#main-form-filter-code").html("");
    $("#main-form-filter-subproject-code").html("");
    $("#main-form-filter-header").html("Inbox");
    $("#txtHierarchyMyFolderSelectedNodeID").val(nodeId);
    StartActivityInbox();
}

function loadActivityURLToClipboard() {
    var longURL = $("#tmp-backup-current-host").html().trim() + "/timeattendance/activitymanagementredesign.aspx";
    longURL += "?aobj=" + $("#txtHiddenAObjectlink").val();
    longURL += "&snaid=" + $("#txtHiddenCompanyCode").val();

    $("#tmp-backup-copy-to-clipboard").html(longURL);

    //console.log("GOOGLE_SHORT_URL_LONG",longURL);

    gapi.client.setApiKey('AIzaSyD26dp-FrcWPYmDdr3NqMFrnnQ9WULKcFU');
    gapi.client.load('urlshortener', 'v1', function () {
        var request = gapi.client.urlshortener.url.insert({
            'resource': {
                'longUrl': longURL
            }
        });
        request.execute(function (response) {
            //console.log("GOOGLE_SHORT_URL",response);
            if (response.id != null) {
                var url = response.id;
                $("#tmp-backup-copy-to-clipboard").html(url);
            }
        });
    });

}
function copyActivityCodeToClipboard() {
    copyToClipboard($("#txtHiddenAObjectlink").val());
}
function copyActivityURLToClipboard() {
    copyToClipboard($("#tmp-backup-copy-to-clipboard").html().trim());
}
function copyToClipboard(text) {
    var textArea = $("<textarea/>", {
        class: "tmp-copy-to-clipboard",
        html: text
    });
    textArea.appendTo("body");
    textArea.select();
    try {
        var successful = document.execCommand('copy');
        AGSuccess('Copy <span class="text-success">' + text + '</span> to clipboard successfully');
    } catch (err) {
        window.prompt("Copy to clipboard: Ctrl+C, Enter", text);
    }
    textArea.remove();
}

function CheckShow_MSG_RequireRead(count) {
    var checkedIsShow = "";
    if (count == 0) {
        checkedIsShow = "hide";
    }
    return checkedIsShow;
}

function MSG_RequireReadMaxDisplay(count) {
    var displayMax = count.toString();
    if (count > 999) {
        displayMax = "999+";
    }
    return displayMax;
}
function converJsonEmailTo(name, LinkID) {
    var strJsonEmailTo = $("#txtJsonEmailTo").val();
    var JsonEmailTo = JSON.parse(strJsonEmailTo);
    JsonEmailTo.push({
        name: name,
        LinkID: LinkID,
        Email: ""
    });
    $("#txtJsonEmailTo").val(JSON.stringify(JsonEmailTo));
    return JsonEmailTo;
}

function objEmailTo(obj) {
    obj.html('&nbsp;<img src="' + servictWebDomainName + 'images/loadmessage.gif" style="width: 16px;">&nbsp;กำลังดึงข้อมูล...');
    var JsonEmailTo = JSON.parse($("#txtJsonEmailTo").val());
    var List = [];
    for (var i = 0; i < JsonEmailTo.length; i++) {
        List.push(JsonEmailTo[i].LinkID);
    }
    $.ajax({
        url: servictWebDomainName + 'TimeAttendance/TimeAttendance.ashx?q=getEmail&linkid=' + List.toString(),
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            obj.html("");
            for (var i = 0; i < JsonEmailTo.length; i++) {
                var Email = "";
                for (var j = 0; j < data.length; j++) {
                    if (JsonEmailTo[i].LinkID == data[j].LinkID) {
                        Email = data[j].Email;

                        obj.append(
                            '<div>' +
                                '<span style="text-transform: capitalize;">' + JsonEmailTo[i].name + '</span>' +
                                '&nbsp;&lt;' + Email + '&gt;' +
                            '</div>'
                        );
                    }
                }
                JsonEmailTo[i].Email = Email;
            }
            var itemsText = obj.find("div");
            if (itemsText.length > 3) {
                var i = 0;
                itemsText.each(function () {
                    if (i > 2) {
                        $(this).hide();
                    }
                    i++;
                });
                var seeMore = $("<a>", {
                    class: "text-center",
                    html: "แสดงทั้งหมด",
                    href: "Javascript:;"
                }).click(function () {
                    if (!$(itemsText[3]).is(":visible")) {
                        $(this).html('แสดงน้อยลง');
                        itemsText.show();
                    } else {
                        $(this).html('แสดงทั้งหมด');
                        var items = 0;
                        itemsText.each(function () {
                            if (items > 2) {
                                $(this).hide();
                            }
                            items++;
                        });
                    }
                });
                obj.append(seeMore);
            }
            $("#txtJsonEmailTo").val(JSON.stringify(JsonEmailTo))
        }
    });
}