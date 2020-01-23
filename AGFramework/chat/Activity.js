var ActivityInboxTotalRow = 0;
var ActivityInboxPhysicalTotalRow = 0;
var ActivityInboxLazyLoadFlag = false;
var isInboxProject = false;

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
        $(this).addClass("tab-panel-menu-active");
        $(".tab-panel-content").hide();
        $("." + $(this).attr("id")).fadeIn();
        if ($(this).attr("id") == "tab-panel-activity")
            $(this).fadeIn();

        if ($(this).attr("id") == "tab-panel-main") {
            if ($(".row-grouping-date").length == 0) {
                groupByDate();
            }
            if ($(this).hasClass("waiting-load-inbox")) {
                $(this).removeClass("waiting-load-inbox")
                StartActivityInbox();
            }
        }
    });

    
    $("#tab-panel-closer").click(function (e) {
        $("#tab-panel-main").click();
        $("#tab-panel-activity").hide();
        e.stopPropagation();
    });

    $(".search-all-activity").on("keyup", function () {
        $(this).searchActivity();
    });

    $(".select-activity-client-filter").change(function () {
        $('#specialFilter').val('');
        StartActivityInbox();
    });

    $(".select-activity-type-filter").change(function () {
        StartActivityInbox();
    });

    $(".project-grouping").click(function () {
        $(".search-all-activity,.select-activity-client-filter,.select-activity-type-filter").val("");
        var projectCode = $(this).find("span").attr("class");
        $("#main-form-filter-header").html("Project : " + $(this).find("span").html()).focusElementByColor();
        $("#main-form-filter-code").html(projectCode);
    });

    var loadInbox = true;
    try {
        loadInbox = ActivityManagementMainScriptLoadinboxFlag();
    } catch (e) { }

    if (loadInbox) {
        var MainReturn = ActivityManagementMainScript();
        if (MainReturn === false) {
            StartActivityInbox();
        }
        else if (MainReturn === true) {
            isInboxProject = true;
            StartActivityInbox();
        }
        else {
            FilterActivityStateGlobal(MainReturn);
        }
    }
    else {
        $("#tab-panel-main").addClass("waiting-load-inbox");
        loadActivityDetailLinked();
    }
});

function refreshAllNotification() {
    $("#btnRefreshNotiCount").click();
}

function FilterActivityStateGlobal(state) {
    $('.search-all-activity,.select-activity-type-filter,.select-activity-client-filter').val("");
    $("#main-form-filter-code").html("");
    $("#main-form-filter-header").html("Inbox");
    $("#activity-container").avtivityList(state, "", "", "","","","", false);
}


function StartActivityInboxByProject(code, name) {
    animateProjectRightPanel();
    setTimeout(function () {
        $("#tab-panel-main").click();
        $("#main-form-filter-code").html("");
        $("#main-form-filter-header").html("Inbox");

        $('.search-all-activity,.select-activity-type-filter,.select-activity-client-filter').val("");
        $("#main-form-filter-code").html(code);
        $("#main-form-filter-header").html(name);
        $("#activity-container").avtivityList("", "", "", code, "","","", false);
    }, 500);
}

function StartActivityInboxByProjectAndSubProject(ProjectCode,ProjectName,SubprojectCode,SubProjectName) {
    $("#tab-panel-main").click();
    //$("#main-form-filter-code").html("");
    //$("#main-form-filter-header").html("Inbox");

    //$('.search-all-activity,.select-activity-type-filter,.select-activity-client-filter').val("");
    //$("#main-form-filter-code").html(code);
    //$("#main-form-filter-header").html(name);
    $("#activity-container").avtivityList("", "", "", ProjectCode, SubprojectCode, "", "", false);
}

function StartActivityInbox() {
    console.log("isInboxProject",isInboxProject);

    var filter = $(".select-activity-client-filter").val();

    var specialFilter = $('#specialFilter').val();
    if (specialFilter != undefined && specialFilter != '') {
        filter = specialFilter;
    }
    var type = $(".select-activity-type-filter").val();
    var searchKey = $(".search-all-activity").val();
    var project = $("#main-form-filter-code").html();
    var subProject = $("#main-form-filter-subproject-code").html();

    var mainDelegateCode = $("#activity-box-main-delegate-code").html();
    var assetCode = $("#activity-box-asset-code").html();

    $("#activity-container").avtivityList(filter, type, searchKey, project, subProject,mainDelegateCode,assetCode, false);
}

function StartActivityInboxLazyLoad() {
    if (!ActivityInboxLazyLoadFlag && ActivityInboxPhysicalTotalRow < ActivityInboxTotalRow) {
        ActivityInboxLazyLoadFlag = true;
        var filter = $(".select-activity-client-filter").val();
        var type = $(".select-activity-type-filter").val();
        var searchKey = $(".search-all-activity").val();
        var project = $("#main-form-filter-code").html();
        var subProject = $("#main-form-filter-subproject-code").html();

        var mainDelegateCode = $("#activity-box-main-delegate-code").html();
        var assetCode = $("#activity-box-asset-code").html();

        $("#activity-container").avtivityList(filter, type, searchKey, project,subProject,mainDelegateCode,assetCode, true);
    }
}

function getAllAobjectLink() {
    var arrAobj = [];
    $(".activity-selection").each(function () {
        var aobj = $(this).find(".task-color").attr("id").split(":")[0];
        arrAobj.push(aobj);
    });
    return arrAobj.join(',');
}

$.fn.avtivityList = function (filter, type, searchKey, projectCode,subProjectCode,mainDelegateCode,assetCode, isContinueLazyLoad) {
    var inboxMode = isInboxProject ? "inboxproject" : "inbox";

    var elt = $(this);
    if (!isContinueLazyLoad) {
        activityInboxLoading(true, "Loading inbox...");
    }
    else {
        activityInboxLoadingMore(true);
    }

    $.ajax({
        type: "POST",
        url: "TimeAttendance.ashx?q=" + inboxMode,
        data: {
            filter: filter == undefined ? "" : filter,
            type: type == undefined ? "" : type,
            searchKey: searchKey == undefined ? "" : searchKey,
            projectCode: projectCode == undefined ? "" : projectCode,
            subProjectCode: subProjectCode == undefined ? "" : subProjectCode,
            mainDelegateCode: mainDelegateCode == undefined ? "" : mainDelegateCode,
            assetCode:assetCode == undefined ? "" : assetCode,
            allKeys: isContinueLazyLoad ? getAllAobjectLink() : ""
        },
        success: function (datas) {
            activityInboxLoadingMore(false);

            if (ErrorAPIHandel(datas))
                return;

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
                        '<td class="pad" style="height:1px;padding:0px"></td>' +
                        '<td class="activity-image"></td>' +
                        '<td class="content" style="height:1px;padding:0px">&nbsp;</td>' +
                        '<td class="activity-leader" style="height:1px;padding:0px">&nbsp;</td>' +
                        '<td class="activity-xstatus">&nbsp;</td>' +
                        '<td class="activity-position"></td>' +
                        '<td class="activity-date" style="height:1px;padding:0px">&nbsp;</td>' +
                        '<td class="activity-time" style="height:1px;padding:0px">&nbsp;</td>' +
                        '<td class="activity-type" style="height:1px;padding:0px;display:none">&nbsp;</td>' +
                        '<td class="activity-priority">&nbsp;</td>' +
                        '<td class="activity-haste">&nbsp;</td>' +
                        '<td class="activity-problem">&nbsp;</td>' +
                        '<td class="pad-d" style="height:1px;padding:0px"></td>' +
                        '<td class="color-picker" style="height:1px;padding:0px"></td>' +
                        '<td class="activity-status" style="display:none"></td>' +
                    '</tr>'
                );
            }

            for (var i = 0; i < datas.length; i++) {

                var _taskStatusBox = "";
                if (datas[i].ItemTypeName == "Task" || datas[i].ItemTypeName == "Invoice") {
                    var _taskProgress = "<div class='pace pace-active'><div class='pace-progress' data-progress='" + datas[i].TaskStatusPercent +
                                        "' data-progress-text='" + datas[i].TaskStatusPercent + "%' style='-webkit-transform: translate3d(" + datas[i].TaskStatusPercent + "%, 0px, 0px);" +
                                        " -ms-transform: translate3d(" + datas[i].TaskStatusPercent + "%, 0px, 0px); transform: translate3d(" + datas[i].TaskStatusPercent + "%, 0px, 0px);'>" +
                                        " <div class='pace-progress-inner'></div></div><div class='pace-activity'></div></div>";
                    _taskStatusBox = '<div style="zoom:0.5;">' + _taskProgress + '</div>' +
                                        '<div style="zoom:0.5;position:absolute;margin-top:-23px;color:#2299DD">' + datas[i].TaskStatusDesc + '</div>';
                }

                var aStatus = datas[i].AStatus == "Owner" ? "activity-owner" : (datas[i].AStatus == "Require" ? "activity-require" : "");
                var xFilter = datas[i].FilterClass == "markFilter" ? "activity-filter" : "";
                var clientFilter = datas[i].ClientFilterClass;

                var activityClassArray = [];
                activityClassArray.push("activity-selection");
                activityClassArray.push("activity-" + datas[i].AOBJECTLINK);
                activityClassArray.push(xFilter);
                activityClassArray.push(clientFilter);
                activityClassArray.push(aStatus);
                activityClassArray.push((datas[i].PROJECTCODE == "" ? "notsetproject" : datas[i].PROJECTCODE));
                activityClassArray.push("row-grouping-date-" + datas[i].show_datetime.split(" ")[0].split("/").join(""));
                activityClassArray.push((datas[i].isFavorite == 'True' ? 'activity-filter-state-favorite' : ''));
                activityClassArray.push((datas[i].isReminder == 'True' ? 'activity-filter-state-reminder' : ''));
                activityClassArray.push((clientFilter.match('activity-client-filter-late-alarm') && datas[i].ItemTypeName != "Note" ? 'activity-filter-state-late' : ''));
                activityClassArray.push((parseInt(datas[i].RecallCount) > 0 ? 'activity-filter-state-recall' : ''));


                var activityClass = activityClassArray.join(" ");

                var recallIcon = "";
                for (var rc = 0; rc < parseInt(datas[i].RecallCount) ; rc++) {
                    recallIcon += '<i class="fa fa-bullhorn activity-icon-alarm" style="margin-left:' + (rc == 0 ? '3px' : '5px') + '"></i>';
                }

                elt.append(
                '<tr id="activity-selection-' + i + '" title="' + datas[i].JOBDESCRIPTION + '" class="' + activityClass + '">' +
                    '<td class="task-color" id="' + datas[i].AOBJECTLINK + ":" + datas[i].COMPANYCODE + '" style="background:' + datas[i].TLID + '"></td>' +
                    '<td class="pad"></td>' +
                    '<td class="activity-image"><img onerror="DefaultUserImage(this);" src="' + datas[i].profileImage + '" class="circular" /></td>' +
                    '<td class="content ' + datas[i].ActivityStatus + '" id="' +

                            $(elt).attr('id') + ":" +
                            datas[i].EMPCODE + "," +
                            datas[i].DATEIN + "," +
                            datas[i].SEQ + "," +
                            datas[i].COMPANYCODE +

                    '">' +
                    '<i style="position:absolute;margin-top:-16px;padding-left:25px;">' + (clientFilter.match('activity-client-filter-late-alarm') && datas[i].ItemTypeName != "Note" ? '<i class="fa fa-clock-o activity-icon-alarm" ></i>' : '') + '  ' + recallIcon + '</i>' +
                    '<img src="' + servictWebDomainName + 'images/' + datas[i].imgType + '"style="width:20px;margin-right:5px"/><span style="font-weight:600;"' + (datas[i].isReOpen == 'True' ? "class='reOpen-flash'" : "") + '>' + datas[i].JOBDESCRIPTION + '</span></td>' +

                    '<td class="activity-leader">' +
                        _taskStatusBox +
                        '<p style="' + (_taskStatusBox == '' ? '' : 'margin-top:5px;') + 'margin-bottom:0px">' + datas[i].Leader + '</p>' +
                    '</td>' +

                    //'<td class="activity-xstatus" style="color: ' + (datas[i].markStatus == "Require" ? (datas[i].EMPCODE == datas[i].CREATED_BY ? "blue" : "red") : (datas[i].markStatus == "Response" ? (datas[i].isReOpen == 'True' ? "red" : "blue") : "green")) + ';">' + (datas[i].markStatus == "Require" ? (datas[i].EMPCODE == datas[i].CREATED_BY ? "รับทราบแล้ว" : "ยังไม่ได้รับทราบ") : (datas[i].markStatus == "Response" ? (datas[i].isReOpen == 'True' ? "Re-Open !!" : "รับทราบแล้ว") : "เสร็จสิ้นแล้ว")) + '</td>' +
                    '<td class="activity-xstatus" style="color: ' + datas[i].MainDelegateStatusColor + ';">' + datas[i].MainDelegateStatusText + '</td>' +
                    '<td class="activity-position">' + (datas[i].APosition == "Owner" ? "เจ้าของงาน" : (datas[i].APosition == "Main Delegate" ? "งานหลัก" : "งานแจ้งเพื่อทราบ")) + '</td>' +
                    '<td class="activity-date">' + datas[i].show_datetime.split(' ')[0] + '</td>' +
                    '<td class="activity-time">' + datas[i].show_datetime.split(' ')[1] + '</td>' +
                    '<td class="activity-type" style="display:none">' + datas[i].ItemTypeName + '</td>' +
                    '<td class="activity-priority" style="color: ' + datas[i].PriorityColor + ';">' + datas[i].Priority + '</td>' +
                    '<td class="activity-haste" style="color: ' + datas[i].HasteColor + ';">' + datas[i].Haste + '</td>' +
                    '<td class="activity-problem">' + datas[i].ProblemType + '</td>' +
                    '<td class="pad-d"><span style="display:none">' + datas[i].show_content + '</span></td>' +
                    '<td class="color-picker"><b class="glyphicon glyphicon-star ' + (datas[i].isFavorite == 'True' ? 'activity-favorite' : '') + '"></b> <u class="glyphicon glyphicon-bell ' + (datas[i].isReminder == 'True' ? 'activity-reminder' : '') + '"></u> <i class="glyphicon glyphicon-cog open-color-picker"></i></td>' +
                    '<td class="activity-status" style="display:none">' + (datas[i].AStatus == "Require" ? "ยังไม่ได้รับทราบ" : (datas[i].AStatus == "Response" ? "รับทราบแล้ว" : (datas[i].AStatus == "Owner" ? "เจ้าของงาน" : "เสร็จสิ้นแล้ว"))) + '</td>' +
                '</tr>'
                );
            }
            //============== Select activity click (click for show Activity detail)
            $(".activity-container .content").unbind("click");
            $(".activity-container .content").click(function () {
                if ($("#txtHiddenRowkey").length > 0) {
                    $(".activity-selection").removeClass("activity-selection-focus");
                    $(this).parent().addClass("activity-selection-focus");
                    loadActivityDetail($(this).parent().find(".task-color").attr("id").split(":")[0], '', true);
                    $("#txtHiddenRowkey").val($(this).attr("id").split(":")[1]);
                    $("#txtHiddenAObjectlink").val($(this).parent().find(".task-color").attr("id").split(":")[0]);
                    $("#txtHiddenCompanyCode").val($(this).parent().find(".task-color").attr("id").split(":")[1]);
                    var Require = $(this).parent().find(".activity-status").html();
                    $("#txtHiddenAP").val(Require);
                }
            });

            BindConfigEvenForActivity(elt);

            $("html").click(function (e) {
                $(".color-picker-panel").remove();
                bindNanoScroller();
            });

            bindNanoScroller();

            
            //if (firstLoad) {
            //    loadActivityDetailLinked();
            //    firstLoad = false;
            //}

            ActivityIconAnimate(true);

            groupByDate();

            if ($(".activity-selection").length == 0) {
                $(".activity-container tbody").append($("<tr/>", {
                    class: "activity-container-empty",
                    html: "<td style='text-align:center;color:red;padding:20px;background:#eee;' colspan='11'>ไม่มีรายการ</td>"
                }));
            }

            ActivityInboxPhysicalTotalRow = $(".activity-selection").length;

            var rowDesc = ActivityInboxPhysicalTotalRow + " of " + ActivityInboxTotalRow + " items";
            if (isContinueLazyLoad) {
                //rowDesc += " (" + totalActs + " items was added)";
            }
            $(".activity-inbox-rows-desc").html(rowDesc);

            activityInboxLoading(false);

            if (isInboxProject) {
                $(".activity-selection").each(function () {
                    var content = $(this).find(".content span");
                    var url = "/TimeAttendance/ActivityManagementReDesign.aspx";
                    url += "?aobj=" + content.parent().parent().find(".task-color").attr("id").split(":")[0];
                    url += "&snaid=" + content.parent().parent().find(".task-color").attr("id").split(":")[1];
                    content.html("<a href='" + url + "' target='_blank'>" + content.html() + "</a>");
                });
            }
            try {
                var _parent = window.parent;
                if (_parent != undefined) {
                    _parent.callBackLoadingSuccess();
                }
            } catch (e) { }
        }
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
            html: "<td style='text-align:center;color:#000;padding:20px;background:#eee;' colspan='11'>" +
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
    $(".nano").bind("scrollend", function (e) {
        if (!$(this).hasClass("workgroup-hierarchy-nano")) {
            if ($(e.currentTarget).find("#activity-container").length > 0) {
                StartActivityInboxLazyLoad();
            }
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
    //    url: "TimeAttendance.ashx?q=tasknoti",
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

    $(".row-grouping-date").each(function () {
        var id = $(this).attr("id");
        if ($("." + id + ":visible").length == 0)
            $(this).hide();
    });

    $(".activity-container-empty").remove();
    if ($(".activity-selection:visible").length == 0) {
        $(".activity-container tbody").append($("<tr/>", {
            class: "activity-container-empty",
            html: "<td style='text-align:center;color:red;padding:20px;background:#eee;' colspan='11'>ไม่มีรายการ</td>"
        }));
    }
}

function FilterActivity() {
    
    //$(".activity-search-mark").each(function () {
    //    $(this).before($(this).html());
    //});
    //$(".activity-search-mark").remove();

    //var SearchValue = $(".search-all-activity").val().trim().toLowerCase();
    var SelectedClientFilter = $(".select-activity-client-filter").val();
    //var SelectedTypeFilter = $(".select-activity-type-filter").val();

    if (SelectedClientFilter != "") {
        $(".activity-container .activity-selection").hide();
        $("." + SelectedClientFilter).show();
    }
    else {
        $(".activity-container .activity-selection").show();
    }

    //if (SelectedTypeFilter != "") {
    //    $(".activity-container .activity-selection:visible").each(function () {
    //        if($(this).find(".activity-type").html() != SelectedTypeFilter)
    //            $(this).hide();
    //    });
    //}

    //var projectClass = $("#main-form-filter-code").html();
    //if (projectClass != undefined && projectClass != "") {
    //    $(".activity-container .activity-selection:visible").each(function () {
    //        if (!$(this).hasClass(projectClass))
    //            $(this).hide();
    //    });
    //}

    //if (SearchValue != "") {
    //    $(".activity-container .activity-selection:visible").each(function () {
    //        var row = $(this);
    //        var subject = $(row).find(".content").find("span").html().toLowerCase();
    //        var leader = $(row).find(".activity-leader p").html().toLowerCase();
    //        if (subject.match(SearchValue) || leader.match(SearchValue)) {
    //            $(row).find(".content").find("span").html(subject.split(SearchValue).join("<b class='activity-search-mark' style='background:yellow'>" + SearchValue + "</b>"));
    //            $(row).find(".activity-leader p").html(leader.split(SearchValue).join("<b class='activity-search-mark' style='background:yellow'>" + SearchValue + "</b>"));
    //        }
    //        else {
    //            $(row).hide();
    //        }
    //    });
    //}

    //$(".row-grouping-date").show();
    //$(".row-grouping-date").each(function () {
    //    var id = $(this).attr("id");
    //    if ($("." + id + ":visible").length == 0)
    //        $(this).hide();
    //});

    $(".activity-container-empty").remove();
    if ($(".activity-selection:visible").length == 0) {
        $(".activity-container tbody").append($("<tr/>", {
            class: "activity-container-empty",
            html: "<td style='text-align:center;color:red;padding:20px;background:#eee;' colspan='11'>ไม่มีรายการ</td>"
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

function groupByDate() {
    $(".row-grouping-date").remove();
    var date = [];
    $("#activity-container tr").each(function () {
        try{
            var val = $(this).find(".activity-date").html();
            if ($(this).index() != 0 && !hasChildsArray(date, val) && $(this).is(":visible")) {
                $(this).before("<tr class='row-grouping-date' id='row-grouping-date-" + val.split('/').join('') + "'><td></td><td></td><td colspan='9' style='padding:5px;'>" + val + "</td></tr>");
                date.push(val);
            }
        }
        catch (e) { }
    });
    $(".activity-date").hide();
}

function clearGroupByDate() {
    $(".row-grouping-date").remove();
}

function ActivityIconAnimate(flag) {
    setTimeout(function () {
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

function BindConfigEvenForActivity(elt) {

    elt.find(".color-picker b").unbind("click").click(function (e) {
        var key = $(this).parent().parent().find(".task-color").attr("id").split(':')[0];
        var snaid = $(this).parent().parent().find(".task-color").attr("id").split(':')[1];
        var flag = !$(this).hasClass("activity-favorite");
        
        if ($(this).hasClass("activity-favorite")) {
            $(this).parent().parent().removeClass("activity-filter-state-favorite");
            $(this).removeClass("activity-favorite");
        }
        else {
            $(this).parent().parent().addClass("activity-filter-state-favorite");
            $(this).addClass("activity-favorite");
        }

        $.ajax({
            url: 'TimeAttendance.ashx?q=activityfavorite&key=' + key + '&snaid=' + snaid + '&flag=' + flag,
            success: function (datas) {
                if (ErrorAPIHandel(datas))
                    return;

                LoadActivityNotificationCount();
            }
        });
        e.stopPropagation();
    });

    elt.find(".color-picker u").unbind("click").click(function (e) {
        var key = $(this).parent().parent().find(".task-color").attr("id").split(':')[0];
        var snaid = $(this).parent().parent().find(".task-color").attr("id").split(':')[1];
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
            url: 'TimeAttendance.ashx?q=activityreminder&key=' + key + '&snaid=' + snaid + '&flag=' + flag,
            success: function (datas) {
                if (ErrorAPIHandel(datas))
                    return;

                LoadActivityNotificationCount();
            }
        });
        e.stopPropagation();
    });

    elt.find(".color-picker i").unbind("click").click(function (e) {
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
                $(this).parent().parent().parent().parent().parent().find(".task-color").css({
                    background: color
                });
                var key = $(this).parent().parent().parent().parent().parent().find(".task-color").attr("id").split(':')[0];
                var snaid = $(this).parent().parent().parent().parent().parent().find(".task-color").attr("id").split(':')[1];
                $.ajax({
                    url: 'TimeAttendance.ashx?q=activitycolor&key=' + key + '&color=' + color + '&SNAID=' + snaid,
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
}

function CallbackActivityEvent(event, oldAobj) {
    StartActivityInbox();
    refreshAllNotification();
    //var rowKey = $("#txtHiddenRowkey").val();
    //var aobj = $("#txtHiddenAObjectlink").val();
    //var activityRow = $(".activity-" + (oldAobj == undefined ? aobj : oldAobj));

    //if (event != "canceled" && event != "checkout") {
    //    LoadActivityInboxRow(aobj, oldAobj,false);
    //}
    //else {
    //    $(activityRow).remove();
    //    ActivityInboxTotalRow--;
    //    groupByDate();
    //}
}

function LoadActivityInboxRow(aobj, oldAobj,isNewActivity) {
    $.ajax({
        url: "TimeAttendance.ashx?q=inboxrow&aobj=" + aobj,
        success: function (datas) {
            if (ErrorAPIHandel(datas))
                return;

            datas = datas.ListActivity;

            var elt = $("#activity-container");
            var activityRow = $(".activity-" + (oldAobj == undefined ? aobj : oldAobj));
            for (var i = 0; i < datas.length; i++) {


                var _taskStatusBox = "";

                if (datas[i].ItemTypeName == "Task") {
                    var _taskProgress = "<div class='pace pace-active'><div class='pace-progress' data-progress='" + datas[i].TaskStatusPercent +
                                        "' data-progress-text='" + datas[i].TaskStatusPercent + "%' style='-webkit-transform: translate3d(" + datas[i].TaskStatusPercent + "%, 0px, 0px);" +
                                        " -ms-transform: translate3d(" + datas[i].TaskStatusPercent + "%, 0px, 0px); transform: translate3d(" + datas[i].TaskStatusPercent + "%, 0px, 0px);'>" +
                                        " <div class='pace-progress-inner'></div></div><div class='pace-activity'></div></div>";
                    _taskStatusBox = '<div style="zoom:0.5;">' + _taskProgress + '</div>' +
                                        '<div style="zoom:0.5;position:absolute;margin-top:-23px;color:#2299DD">' + datas[i].TaskStatusDesc + '</div>';
                }

                var aStatus = datas[i].AStatus == "Owner" ? "activity-owner" : (datas[i].AStatus == "Require" ? "activity-require" : "");
                var xFilter = datas[i].FilterClass == "markFilter" ? "activity-filter" : "";
                var clientFilter = datas[i].ClientFilterClass;

                var activityClassArray = [];
                activityClassArray.push("activity-selection");
                activityClassArray.push("activity-" + datas[i].AOBJECTLINK);
                activityClassArray.push(xFilter);
                activityClassArray.push(clientFilter);
                activityClassArray.push(aStatus);
                activityClassArray.push((datas[i].PROJECTCODE == "" ? "notsetproject" : datas[i].PROJECTCODE));
                activityClassArray.push("row-grouping-date-" + datas[i].show_datetime.split(" ")[0].split("/").join(""));
                activityClassArray.push((datas[i].isFavorite == 'True' ? 'activity-filter-state-favorite' : ''));
                activityClassArray.push((datas[i].isReminder == 'True' ? 'activity-filter-state-reminder' : ''));
                activityClassArray.push((clientFilter.match('activity-client-filter-late-alarm') && datas[i].ItemTypeName != "Note" ? 'activity-filter-state-late' : ''));
                activityClassArray.push((parseInt(datas[i].RecallCount) > 0 ? 'activity-filter-state-recall' : ''));

                var activityClass = activityClassArray.join(" ");

                var newRow = $("<tr/>", {
                    title: datas[i].JOBDESCRIPTION,
                    class: activityClass
                });

                if (!isNewActivity) {
                    $(newRow).attr("id",$(activityRow).attr("id"));
                    $(activityRow).after(newRow);
                }
                else {
                    var activityDate = datas[i].show_datetime.split(' ')[0].split('/').join('');

                    if ($("#row-grouping-date-" + activityDate).length > 0) {
                        $("#row-grouping-date-" + activityDate).after(newRow);
                    }
                    else {
                        if ($(".row-grouping-date").length > 0) {

                            var compareDate = datas[i].show_datetime.split(' ')[0].split('/');
                            var compareDateInt = parseInt(compareDate[2] + compareDate[1] + compareDate[0]);
                            var flagCheck = true;
                            $(".row-grouping-date").each(function () {
                                if (flagCheck) {
                                    var getDate = $(this).find("td:eq(2)").html().split('/');
                                    var getDateInt = parseInt(getDate[2] + getDate[1] + getDate[0]);
                                    //alert(compareDateInt + " : " + getDateInt);
                                    if (compareDateInt > getDateInt) {
                                        $(this).before(newRow);
                                        flagCheck = false;
                                    }
                                }
                            });
                        }
                        else {
                            $(".tr-fake").after(newRow);
                        }
                        $(newRow).before("<tr class='row-grouping-date' id='row-grouping-date-" + activityDate + "'><td></td><td></td><td colspan='9' style='padding:5px;'>" + datas[i].show_datetime.split(' ')[0] + "</td></tr>");
                    }

                    $(newRow).attr("id", "activity-selection-" + ($(".activity-selection").length + 1));
                }

                var recallIcon = "";
                for (var rc = 0; rc < parseInt(datas[0].RecallCount) ; rc++) {
                    recallIcon += '<i class="fa fa-bullhorn activity-icon-alarm" style="margin-left:' + (rc == 0 ? '3px' : '5px') + '"></i>';
                }

                $(newRow).append(

                    '<td class="task-color" id="' + datas[i].AOBJECTLINK + ":" + datas[i].COMPANYCODE + '" style="background:' + datas[i].TLID + '"></td>' +
                    '<td class="pad"></td>' +
                    '<td class="activity-image"><img src="' + datas[i].profileImage + '" class="circular" /></td>' +
                    '<td class="content" id="' +

                            $(elt).attr('id') + ":" +
                            datas[i].EMPCODE + "," +
                            datas[i].DATEIN + "," +
                            datas[i].SEQ + "," +
                            datas[i].COMPANYCODE +

                    '">' +
                    '<i style="position:absolute;margin-top:-16px;padding-left:25px;">' + (clientFilter.match('activity-client-filter-late-alarm') && datas[i].ItemTypeName != "Note" ? '<i class="fa fa-clock-o activity-icon-alarm" ></i>' : '') + '  ' + recallIcon + '</i>' +
                    '<img src="' + servictWebDomainName + 'images/' + datas[i].imgType + '"style="width:20px;margin-right:5px"/><span style="font-weight:600;"' + (datas[i].isReOpen == 'True' ? "class='reOpen-flash'" : "") + '>' + datas[i].JOBDESCRIPTION + '</span></td>' +

                    '<td class="activity-leader">' +
                        _taskStatusBox +
                        '<p style="' + (_taskStatusBox == '' ? '' : 'margin-top:5px;') + 'margin-bottom:0px">' + datas[i].Leader + '</p>' +
                    '</td>' +

                    //'<td class="activity-xstatus" style="color: ' + (datas[i].markStatus == "Require" ? (datas[i].EMPCODE == datas[i].CREATED_BY ? "blue" : "red") : (datas[i].markStatus == "Response" ? (datas[i].isReOpen == 'True' ? "red" : "blue") : "green")) + ';">' + (datas[i].markStatus == "Require" ? (datas[i].EMPCODE == datas[i].CREATED_BY ? "รับทราบแล้ว" : "ยังไม่ได้รับทราบ") : (datas[i].markStatus == "Response" ? (datas[i].isReOpen == 'True' ? "Re-Open !!" : "รับทราบแล้ว") : "เสร็จสิ้นแล้ว")) + '</td>' +
                    '<td class="activity-xstatus" style="color: ' + datas[i].MainDelegateStatusColor + ';">' + datas[i].MainDelegateStatusText + '</td>' +
                    '<td class="activity-position">' + (datas[i].APosition == "Owner" ? "เจ้าของงาน" : (datas[i].APosition == "Main Delegate" ? "งานหลัก" : "งานแจ้งเพื่อทราบ")) + '</td>' +
                    '<td class="activity-date" style="display:none">' + datas[i].show_datetime.split(' ')[0] + '</td>' +
                    '<td class="activity-time">' + datas[i].show_datetime.split(' ')[1] + '</td>' +
                    '<td class="activity-type" style="display:none">' + datas[i].ItemTypeName + '</td>' +
                    '<td class="activity-priority" style="color: ' + datas[i].PriorityColor + ';">' + datas[i].Priority + '</td>' +
                    '<td class="activity-haste" style="color: ' + datas[i].HasteColor + ';">' + datas[i].Haste + '</td>' +
                    '<td class="activity-problem">' + datas[i].ProblemType + '</td>' +
                    '<td class="pad-d"><span style="display:none">' + datas[i].show_content + '</span></td>' +
                    '<td class="color-picker"><b class="glyphicon glyphicon-star ' + (datas[i].isFavorite == 'True' ? 'activity-favorite' : '') + '"></b> <u class="glyphicon glyphicon-bell ' + (datas[i].isReminder == 'True' ? 'activity-reminder' : '') + '"></u> <i class="glyphicon glyphicon-cog open-color-picker"></i></td>' +
                    '<td class="activity-status" style="display:none">' + (datas[i].AStatus == "Require" ? "ยังไม่ได้รับทราบ" : (datas[i].AStatus == "Response" ? "รับทราบแล้ว" : (datas[i].AStatus == "Owner" ? "เจ้าของงาน" : "เสร็จสิ้นแล้ว"))) + '</td>'
                );

                $(newRow).find(".content").click(function () {
                    $(".activity-selection").removeClass("activity-selection-focus");
                    $(this).parent().addClass("activity-selection-focus");
                    loadActivityDetail($(this).parent().find(".task-color").attr("id").split(":")[0], '', true);
                    $("#txtHiddenRowkey").val($(this).attr("id").split(":")[1]);
                    $("#txtHiddenAObjectlink").val($(this).parent().find(".task-color").attr("id").split(":")[0]);
                    $("#txtHiddenCompanyCode").val($(this).parent().find(".task-color").attr("id").split(":")[1]);
                    var Require = $(this).parent().find(".activity-status").html();
                    $("#txtHiddenAP").val(Require);

                    //if (typeof (initialKMafterSelectDocument) == "function") {
                    //    initialKMafterSelectDocument('ACTIVITY', 'ACTIVITY', $("#txtHiddenAObjectlink").val(), '', $('#detail_subject').html(), globalJobRemark);
                    //}
                });
                
                BindConfigEvenForActivity(newRow);
            }

            if (!isNewActivity || (isNewActivity && oldAobj != undefined)) {
                $(activityRow).remove();
            }
            //FilterActivity();
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
    $('body').append(
        '<div id="confirmModalRequireResponse" class="popup-dialog" style="z-index:50000;position:fixed;width:100%;height:100%;background:rgba(0, 0, 0, 0.7);top:0px;left:0px;display:none">' +
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
        '</div> '
    );

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
        agroLoading(true);
    });
    $('#confirmModalRequireResponse').find('#btnconfirmModalRequireResponseNoSendEmail').click(function () {
        readedClickRequireResponse(false, obj, fnc);
        closeModalRequireResponse();
        agroLoading(true);
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
                    url: 'TimeAttendance.ashx?q=eventleader&key=' + $("#txtHiddenAObjectlink").val() + '&val=READED' + '&rowkey=' + $("#txtHiddenRowkey").val() + '&SNAID=' + $("#txtHiddenCompanyCode").val() + '&sendmail=' + isSendMailEvent,
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
                    url: 'TimeAttendance.ashx?q=eventother&key=' + $("#txtHiddenAObjectlink").val() + '&val=READED' + '&rowkey=' + $("#txtHiddenRowkey").val() + '&SNAID=' + $("#txtHiddenCompanyCode").val() + '&sendmail=' + isSendMailEvent,
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

function confirmEmail(msg, fnc, obj, isRequireRemark) {
    $('body').append(
        '<div id="confirmModal" class="popup-dialog" style="z-index:50000;position:fixed;width:100%;height:100%;background:rgba(0, 0, 0, 0.7);top:0px;left:0px;display:none">' +
            '<div style="width: 530px;margin:auto;background:#fff;padding:20px;margin-top:150px;border:1px solid #ccc;border-radius:5px">' +
                '<div>' +
                    '<div style="border-bottom:1px solid #ccc;padding-bottom:5px;">' +
                        '<span class="modal-title" style="text-align: left;" id="confirmModalMsg"></span>' +
                    '</div>' +

                    '<div id="confirmModalRemarkBox" style="padding-bottom:5px;padding-top:5px;margin-top:5px;display:none">' +
                        '<label>หมายเหตุ</label>' +
                        '<textarea class="form-control" style="resize:none;height:150px" id="confirmModalRemark" placeholder="กรุณาระบุหมายเหตุ"></textarea>' +
                    '</div>' +

                    '<div style="text-align:center;padding-top:20px">' +
                        '<input type="button" name="name" id="btnConfirmModalSendEmail" value="ยืนยันและส่งอีเมล" class="btn btn-primary" style="margin-right:10px;width:150px"/>' +
                        '<input type="button" name="name" id="btnConfirmModalNoSendEmail" value="ยืนยัน" class="btn btn-success" style="margin-right:10px;width:150px"/>' +
                        '<input type="button" name="name" id="btnConfirmModalCancel" value="ยกเลิก" class="btn btn-danger" style="width:150px"/>' +
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

    $('#confirmModal').find('#btnConfirmModalSendEmail').click(function () {
        if (isRequireRemark != undefined && isRequireRemark) {
            if ($("#confirmModalRemark").val().trim() == "") {
                alert("กรุณาระบุหมายเหตุ");
                $("#confirmModalRemark").focus();
                return;
            }
            else {
                SendRemarkBeforeDoEvent(fnc, true, obj, "confirmModal");
            }
        }
        else {
            fnc(true, obj);
            closeModal();
        }
        agroLoading(true);
    });
    $('#confirmModal').find('#btnConfirmModalNoSendEmail').click(function () {
        if (isRequireRemark != undefined && isRequireRemark) {
            if ($("#confirmModalRemark").val().trim() == "") {
                alert("กรุณาระบุหมายเหตุ");
                $("#confirmModalRemark").focus();
                return;
            }
            else {
                SendRemarkBeforeDoEvent(fnc, false, obj, "confirmModal");
            }
        }
        else {
            fnc(false, obj);
            closeModal();
        }
        agroLoading(true);
    });
    $('#confirmModal').find('#btnConfirmModalCancel').click(function () {
        closeModal();
    });
    function closeModal() {
        $('#confirmModal').fadeOut(function () {
            $('#confirmModal').remove();
        });
    }
}

var GlobalSendRemarkBeforeDoEventFunction;
var GlobalSendRemarkBeforeDoEventSendMail;
var GlobalSendRemarkBeforeDoEventObject;
var GlobalSendRemarkBeforeDoEventComfirmID;

function SendRemarkBeforeDoEvent(fnc, isSendMail, obj, confirmPanelID) {
    agroLoading(true);
    GlobalSendRemarkBeforeDoEventComfirmID = confirmPanelID;
    GlobalSendRemarkBeforeDoEventFunction = fnc;
    GlobalSendRemarkBeforeDoEventSendMail = isSendMail;
    GlobalSendRemarkBeforeDoEventObject = obj;
    $("#txtRemarkContainerBeforeDoEvent").val($("#confirmModalRemark").val().split("<").join(""));
    $("#btnRemarkContainerBeforeDoEvent").click();
}

function CallBackSendRemarkBeforeDoEvent() {
    agroLoading(false);
    $("#" + GlobalSendRemarkBeforeDoEventComfirmID).fadeOut(function () {
        $("#" + GlobalSendRemarkBeforeDoEventComfirmID).remove();
    });
    GlobalSendRemarkBeforeDoEventFunction(GlobalSendRemarkBeforeDoEventSendMail, GlobalSendRemarkBeforeDoEventObject);
}

function uploadNewAttachFile(isSendMail, obj) {
    prepareSaveAddAttach(isSendMail);
}

function prepareSaveAddAttach(isSendMail) {
    if (typeof (document.all.iframeAddAttachFile.contentWindow.doSaveFile) == "function")
        document.all.iframeAddAttachFile.contentWindow.doSaveFile(isSendMail);
    else
        alertMessage("Save file error.");
}

function successSaveAddAttach() {
    alertMessage("อัพโหลดไฟล์เสร็จสิ้นแล้ว");
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
    $.ajax({
        url: 'TimeAttendance.ashx?q=cancelactivity&key=' + $("#txtHiddenAObjectlink").val() + '&rowkey=' + $("#txtHiddenRowkey").val() + '&SNAID=' + $("#txtHiddenCompanyCode").val() + '&sendmail=' + isSendMail,
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            closeActivityDetial();
            CallbackActivityEvent("canceled");
            LoadActivityNotificationCount();
            $("#panel-activity-detail").hide();
            alertMessage("บันทึกยกเลิกเสร็จสิ้นแล้ว");
        }
    });
}

function checkOutClick(isSendMail, obj) {
    checkout(isSendMail, $("#txtHiddenRowkey").val());
}

function recreateActivity() {
    
    if (confirm('ต้องการไปยังหน้าสร้างใหม่จากรายการเดิมหรือไม่'))
        openIfraneCreateActivity($("#txtHiddenAObjectlink").val(), $("#txtHiddenRowkey").val(), $("#txtHiddenCompanyCode").val());
}

function convertNoteTotask() {
    if (confirm('ต้องการเปลี่ยนให้เป็น Task หรือไม่'))
        openIframeConvertNoteTotask();
}

function openIframeConvertNoteTotask() {
    var aobj = $("#txtHiddenAObjectlink").val();
    var rowkey = $("#txtHiddenRowkey").val();
    var snaid = $("#txtHiddenCompanyCode").val();
    var ifca = document.getElementById("iframeCreateActivity");
    ifca.src = '../widget/PopupCreateActivityReDesign.aspx?aobj=' + aobj + '&rowkey=' + rowkey + '&snaid=' + snaid + '&mode=convertnote';
}

function checkout(isSendMail, key) {
    keyArr = key.split(',');
    $.ajax({
        url: 'TimeAttendance.ashx?q=checkout&DATEIN=' + keyArr[1] + '&SEQ=' + keyArr[2] + '&EMPCODE=' + keyArr[0] + '&SNAID=' + keyArr[3] + '&key=' + $("#txtHiddenAObjectlink").val() + '&rowkey=' + $("#txtHiddenRowkey").val() + '&sendmail=' + isSendMail,
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            closeActivityDetial();
            CallbackActivityEvent("checkout");
            LoadActivityNotificationCount();
            $("#panel-activity-detail").hide();
            alertMessage("บันทึกเสร็จสิ้นงานเรียบร้อยแล้ว");
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
    $.ajax({
        url: 'TimeAttendance.ashx?q=recall&member=' + members + '&key=' + $("#txtHiddenAObjectlink").val() + '&rowkey=' + $("#txtHiddenRowkey").val() + '&SNAID=' + $("#txtHiddenCompanyCode").val() + '&sendmail=' + isSendMail,
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            alertMessage("ส่งข้อความทวงถามเรียบร้อยแล้ว");
            CallbackActivityEvent("recall");
            LoadActivityNotificationCount();
            //closeActivityDetial();
            //loadActivityDetail($("#txtHiddenRowkey").val());

        }
    });
}

function successClick(isSendMail, obj) {
    if ($("#txtHiddenYouAre").val() == "MD") {
        $.ajax({
            url: 'TimeAttendance.ashx?q=eventleader&key=' + $("#txtHiddenAObjectlink").val() + '&val=SUCCESS' + '&rowkey=' + $("#txtHiddenRowkey").val() + '&SNAID=' + $("#txtHiddenCompanyCode").val() + '&sendmail=' + isSendMail,
            success: function (data) {
                if (ErrorAPIHandel(data))
                    return;
                alertMessage("บันทึกเสร็จสิ้นเรียบร้อยแล้ว");
                loadActivityDetail($("#txtHiddenAObjectlink").val());
                CallbackActivityEvent("success");
                $("#txtHiddenAP").val("เสร็จสิ้นแล้ว");
            }
        });
    }
    if ($("#txtHiddenYouAre").val() == "OD") {
        $.ajax({
            url: 'TimeAttendance.ashx?q=eventother&key=' + $("#txtHiddenAObjectlink").val() + '&val=SUCCESS' + '&rowkey=' + $("#txtHiddenRowkey").val() + '&SNAID=' + $("#txtHiddenCompanyCode").val() + '&sendmail=' + isSendMail,
            success: function (data) {
                if (ErrorAPIHandel(data))
                    return;
                alertMessage("บันทึกเสร็จสิ้นเรียบร้อยแล้ว");
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
            url: 'TimeAttendance.ashx?q=eventleader&key=' + $("#txtHiddenAObjectlink").val() + '&val=READED' + '&rowkey=' + $("#txtHiddenRowkey").val() + '&SNAID=' + $("#txtHiddenCompanyCode").val() + '&sendmail=' + isSendMail,
            success: function (data) {
                if (ErrorAPIHandel(data))
                    return;
                alertMessage("บันทึกรับทราบเรียบร้อยแล้ว");

                loadActivityDetail($("#txtHiddenAObjectlink").val());
                CallbackActivityEvent("readed");
                $("#txtHiddenAP").val("รับทราบแล้ว");
            }
        });
    }
    if ($("#txtHiddenYouAre").val() == "OD") {
        $.ajax({
            url: 'TimeAttendance.ashx?q=eventother&key=' + $("#txtHiddenAObjectlink").val() + '&val=READED' + '&rowkey=' + $("#txtHiddenRowkey").val() + '&SNAID=' + $("#txtHiddenCompanyCode").val() + '&sendmail=' + isSendMail,
            success: function (data) {
                if (ErrorAPIHandel(data))
                    return;
                alertMessage("บันทึกรับทราบเรียบร้อยแล้ว");

                loadActivityDetail($("#txtHiddenAObjectlink").val());
                CallbackActivityEvent("readed");
                $("#txtHiddenAP").val("รับทราบแล้ว");
            }
        });
    }
}

function reOpenClick(isSendMail, obj) {
    $.ajax({
        url: 'TimeAttendance.ashx?q=taskreopen&key=' + $("#txtHiddenAObjectlink").val() + '&rowkey=' + $("#txtHiddenRowkey").val() + '&sendmail=' + isSendMail + '&xremark=' + $("#txtRemarkContainerBeforeDoEvent").val(),
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            alertMessage("เปิดงานใหม่เพื่อแก้ไขอีกครั้งเรียบร้อยแล้ว");

            loadActivityDetail($("#txtHiddenAObjectlink").val());
            CallbackActivityEvent("reopen");
        }
    });
}

function loadActivityDetailLinked() {
    if ($("#txtContinueActivityFromEmail").length > 0 && $("#txtContinueActivityFromEmail").val() != "") {
        var rowkey = $("#txtContinueActivityFromEmail").val().split(":")[0];
        var snaid = $("#txtContinueActivityFromEmail").val().split(":")[1];
        var aobj = $("#txtContinueActivityFromEmail").val().split(":")[2];
        $("#txtHiddenAObjectlink").val(aobj);
        $("#txtHiddenCompanyCode").val(snaid);
        $("#txtHiddenRowkey").val(rowkey);
        loadActivityDetail(aobj, '', true);
    }
}

function loadActivityDetail(aObjectLink,mode,isLoadkm) {
    agroLoading(true);
    $.ajax({
        url: 'TimeAttendance.ashx?q=task&aObjectLink=' + aObjectLink + '&old=' + $("#txtHiddenAObjectlink").val(),
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;

            //if (data[0].OLDAOBJECTLINK != "" && data[0].OLDAOBJECTLINK != data[0].AOBJECTLINK) {
            //    return;
            //}
            if (mode == 'display') {
                firstLoad = false;
                $("#div-btn-event-container").hide();
            } else {
                $("#div-btn-event-container").show();
            }
            $("#btn-special-reopen").hide();
            if (data[0].checkOut_Date != "" || data[0].XSTATUS == "CANCELED") {
                $("#div-btn-event-container").hide();

                if (data[0].MYID == data[0].CREATED_BY) {
                    $("#btn-special-reopen").show();
                }
            }

            //=============== About Item Type (Note Task Meeting Service)
            var ItemType = data[0].ItemTypeName == "Job" ? "Service" : data[0].ItemTypeName;
            $('#detail_type').html(ItemType);
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
            $("#detail_subject,#detail_subject_invoice,#tab-menu-activity-name,#poSubjectRight").html(data[0].JOBDESCRIPTION);
            $("#detail_subject,#detail_subject_invoice,#tab-menu-activity-name,#poSubjectRight").parent().prop("title", data[0].JOBDESCRIPTION);

            var _projectDetail = data[0].PROJECTNAME == "" ? "ไม่ระบุกลุ่มงาน" : data[0].PROJECTNAME;

            if (data[0].PROJECTNAME != "" && data[0].PROJECTELEMENTNAME != "") {
                _projectDetail += " / " + data[0].PROJECTELEMENTNAME;
            }

            $('#detail_project').html(_projectDetail);            
            $('#detail_inv_project').html(_projectDetail);// for inv
            $('#detail_projectPO').html(_projectDetail);
            $('#detail_sub_project').html(data[0].PROJECTELEMENTNAME);            
            $('#detail_datetime_start').html(dateformat(data[0].DATEIN) + ' ' + timeformat(data[0].TIMEIN));
            $('#detail_datetime_end').html(dateformat(data[0].DATEOUT) + ' ' + timeformat(data[0].TIMEOUT));
            $('#detail_location').html(data[0].SITECODE == "" ? "ไม่ระบุ" : data[0].SITECODE);
            $('#detail_your_timePO').html(dateformat(data[0].DATEOUT));

            //============== Money 
            $("#detail_money_revenue").html(data[0].ShowRevenue);
            $("#detail_money_charges").html(data[0].ShowCharges);
            $(".detail_money_currency").html(data[0].Currency == "" || data[0].Currency == null ? "THB" : data[0].Currency);
            $("#detail_asset_value").html(data[0].ShowAsset);
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
            $('#detail_problem').html(data[0].ProblemType);
            $('#detail_inv_problem').html(data[0].ProblemType);
            $('#detail_problemPO').html(data[0].ProblemType);
            $('#detail_for').html(data[0].ActivityFor);
            $('#detail_create_by').html(data[0].CREATED_BY_FULLNAME);
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

            startCountDown(data[0].YourTimeCountDown, "detail_your_time_countdown_container");

            $("#detail_create_on").html(data[0].CREATE_ON_DISPLAY);
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

                if (ItemType == "Note") {
                    $("#btnCheckout").hide();
                    $("#btnConvertNote").show();
                }
            }

            if (data[0].MYID == data[0].EMPCODE && data[0].MYID != data[0].CREATED_BY) { // main delegate
                $("#txtHiddenYouAre").val("MD");
                $("#btnResponse").show();
                $("#btnSuccess").show();
                if (ItemType == "Note") {
                    $("#btnConvertNote").show();
                    $("#btnSuccess").hide();
                }
            }

            if (data[0].MYID != data[0].EMPCODE && data[0].MYID != data[0].CREATED_BY) {
                $("#txtHiddenYouAre").val("OD"); // other delegate
                $("#btnResponse").show();
                //$("#btnSuccess").show();
            }


            $('#old_detail_main_delegate').html("");
            $('#old_detail_inv_main_delegate').html("");//for inv
            $('#imgChangeMainDelegateHistory').hide();
            $('#imgChangeMainDelegate').show();
            $('#imgInvChangeMainDelegateHistory').hide();//for inv
            $('#imgInvChangeMainDelegate').hide();// for inv


            if (data[0].CREATED_BY == data[0].EMPCODE) {
                //$('#detail_main_delegate').html("<span style='color:#000'>ไม่มีผู้รับมอบหมายหลัก</span>");
                $('#detail_inv_main_delegate').html("<span style='color:#000'>" + data[0].EMPNAME + " " + data[0].EMPSURNAME + "</span>");
                $('#detail_main_delegate').html("<span style='color:#000'>" + data[0].EMPNAME + " " + data[0].EMPSURNAME + "</span>");//for inv
            }
            else {
                var readedTextColor = "grey";
                var readedTextTitle = "ยังไม่รับทราบ";
                if (data[0].XSTATUS == "READED") {
                    readedTextTitle = "รับทราบแล้ว";
                    readedTextColor = "blue";
                }
                if (data[0].XSTATUS == "SUCCESS") {
                    readedTextTitle = "เสร็จสิ้นแล้ว";
                    readedTextColor = "green";
                }

                

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

                $('#detail_main_delegate').html("<span title='" + readedTextTitle + "' style='color:" + readedTextColor + "'>" + data[0].EMPNAME + " " + data[0].EMPSURNAME + "</span>");
                $('#detail_inv_main_delegate').html("<span title='" + readedTextTitle + "' style='color:" + readedTextColor + "'>" + data[0].EMPNAME + " " + data[0].EMPSURNAME + "</span>");
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

            if ((data[0].ItemType == "004" || data[0].ItemType == "009") && (data[0].ResponeDate != "" || data[0].CompleteDate != "")) {
                $("#detailTaskStatus").show();
            }
            else {
                if ((data[0].ItemType == "004" || data[0].ItemType == "009") && (data[0].ResponeDate == "" || data[0].CompleteDate == "") && data[0].EMPCODE == data[0].CREATED_BY) {
                    $("#detailTaskStatus").show();
                }
                else {
                    $("#detailTaskStatus").hide();
                }
            }

            if (data[0].ItemType == "009") {
                $("#task-status-invoice").append($("#detailTaskStatus"));
            }
            else if (data[0].ItemType == "004") {
                $("#task-status-default").append($("#detailTaskStatus"));
            }
            else {
                $("#task-status-default").append($("#detailTaskStatus"));
            }


            var blackCol = "#777";
            var greenCol = "#398439";

            if (data[0].isHaveMOM == "true") {
                $("#momShortcut").css("color", greenCol);
            }
            else {
                $("#momShortcut").css("color", blackCol);
            }

            if (data[0].isHaveKM == "true") {
                $("#kmShortcut").css({
                    color: greenCol,
                    borderColor: greenCol
                });
            }
            else {
                $("#kmShortcut").css({
                    color: blackCol,
                    borderColor: blackCol
                });
            }

            if (parseInt(data[0].countLocation) > 0 ) {
                $("#locationShortcut").css("color", greenCol);
                $("#locationShortcut").prev().html(data[0].countLocation).show();
            }
            else {
                $("#locationShortcut").css("color", blackCol);
                $("#locationShortcut").prev().html(data[0].countLocation).hide();
            }

            if (parseInt(data[0].countMultimedia) > 0) {
                $("#multimediaShortcut").css("color", greenCol);
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
            loadActivityDetailRemark(data[0].AOBJECTLINK, data[0].MYID, data[0].CREATED_BY_FULLNAME, data[0].JOBREMARKS, data[0].CREATE_ON_DISPLAY, data[0].IMAGE, data[0].CREATED_BY);
            loadActivityDetailRemarkEmail(data[0].AOBJECTLINK, data[0].MYID, data[0].CREATED_BY_FULLNAME, data[0].JOBREMARKS, data[0].CREATE_ON_DISPLAY, data[0].IMAGE, data[0].CREATED_BY);
            loadActivityDetailBehavior();

            //=========== Invoice Activity 

            $('.activity-panel-detail-default').hide();
            $('.activity-panel-detail-invoice').hide();
            $('.activity-panel-detail-purchase').hide();

            $('#btnInvoiceSendMail').hide();

            if (data[0].ItemType == "009") {
                $('#txtHiddenActivityType').val(ItemType);
                $('.activity-panel-detail-invoice').show();
                $('#btnInvoiceSendMail').show();
                loadActivityInvoiceDetail(data[0].DOCNUMBER, data[0].DOCTYPE, data[0].DOCYEAR);
                //==== Load

            }
            else if (data[0].ItemType == "004" && data[0].BUSINESSOBJECT == "PO") {
                $('.activity-panel-detail-purchase').show();                

                loadActivityPODetail(data[0].DOCNUMBER, data[0].DOCTYPE, data[0].DOCYEAR, keyArr[1], keyArr[0], keyArr[2], keyArr[3]);
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
                ActivityChain(data[0].PROJECTCODE, data[0].DOCTYPE, data[0].DOCNUMBER);
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
            $("#tab-panel-activity").click();

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

            //===================== Call Attach File
            loadActivityDetailAttachFile(data[0].AOBJECTLINK, data[0].COMPANYCODE);

            //===================== Call Picture
            loadActivityDetailPicture(data[0].AOBJECTLINK, data[0].COMPANYCODE, data[0].JOBDESCRIPTION);

            //===================== set button send custom email
            $("#btnOpenSendCustomEmail").click(function () {
                sendCustomEmail(data[0].JOBDESCRIPTION, data[0].MYFULLNAME);
            });

            //===================== set button send custom email
            $("#btnOpenSendActivityEmail").click(function () {

                sendActivityEmail(data[0].JOBDESCRIPTION, data[0].MYFULLNAME, data[0].EMAIL_FROM);
            });

            //===================== Set panel Add attach file
            closeAddAttach();

            $('.nano').nanoScroller();

            agroLoading(false);
            $("#btnReloadSystemMessage").click();

            //===================== mark read
            $.ajax({
                url: 'TimeAttendance.ashx?q=markRead&aobjectid=' + data[0].AOBJECTLINK,
                success: function (data) {
                    if (ErrorAPIHandel(data))
                        return;
                }
            });
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

            if (datas.length == 0) {
                $("#attachfile").append(
                       "<div style='color:orange'>ไม่มีไฟล์แนบ</div>"
                );
                $("#detail_total_attach").html(0);

                $("#inv_attachfile").append(
                      "<div style='color:orange'>ไม่มีไฟล์แนบ</div>"
               );
                $("#detail_inv_total_attach").html(0);

                $("#attachfilePO").append(
                      "<div style='color:orange'>ไม่มีไฟล์แนบ</div>"
               );
                $("#detail_total_attachPO").html(0);
            }
            else {
                for (var i = 0; i < datas.length; i++) {
                    $("#attachfile").append(
                        (i != 0 ? "" : "") +
                        "<a target='_blank' download='" + datas[i].file_name + "' href='" + "\\managefile\\"+datas[i].sid + datas[i].file_path + "'>" + datas[i].file_name + "</a>" +
                        "<a target='_blank' href='" + servictWebDomainName + "widget/DownloadFile.aspx?key=" + datas[i].key_object_link + "' style='color:orange;'>(ดาวน์โหลด)</a>" +
                        (datas[i].AllowDelete == "true" ? "<span style='color:red;cursor:pointer' onclick='deleteAttachFile(\"" + datas[i].key_object_link + "\");'>(ลบ)</a>" : "") +
                        "<br />"

                    );
                    $("#inv_attachfile").append(
                       (i != 0 ? "" : "") +
                       "<a target='_blank' href='" + "\\managefile\\" + datas[i].sid + datas[i].file_path + "'>" + datas[i].file_name + "</a>" +
                       "<a target='_blank' href='" + servictWebDomainName + "widget/DownloadFile.aspx?key=" + datas[i].key_object_link + "' style='color:orange;'>(ดาวน์โหลด)</a>" +
                       (datas[i].AllowDelete == "true" ? "<span style='color:red;cursor:pointer' onclick='deleteAttachFile(\"" + datas[i].key_object_link + "\");'>(ลบ)</a>" : "") +
                       "<br />"

                   );
                    $("#attachfilePO").append(
                       (i != 0 ? "" : "") +
                       "<a target='_blank' href='" + "\\managefile\\" + datas[i].sid + datas[i].file_path + "'>" + datas[i].file_name + "</a>" +
                       "<a target='_blank' href='" + servictWebDomainName + "widget/DownloadFile.aspx?key=" + datas[i].key_object_link + "' style='color:orange;'>(ดาวน์โหลด)</a>" +
                       (datas[i].AllowDelete == "true" ? "<span style='color:red;cursor:pointer' onclick='deleteAttachFile(\"" + datas[i].key_object_link + "\");'>(ลบ)</a>" : "") +
                       "<br />"

                   );
                }
                $("#detail_total_attach").html(datas.length);
                $("#detail_inv_total_attach").html(datas.length);
                $("#detail_total_attachPO").html(datas.length);
            }

        }
    });
}

function deleteAttachFile(key) {
    if (confirm("ต้องการลบไฟล์แนบนี้หรือไม่")) {
        $.ajax({
            url: 'TimeAttendance.ashx?q=taskattachfiledelete&key=' + key,
            success: function (data) {
                if (ErrorAPIHandel(data))
                    return;
                alertMessage("ลบไฟล์แนบแล้ว");
                loadActivityDetailAttachFile($("#txtHiddenAObjectlink").val(), $("#txtHiddenCompanyCode").val());
            }
        });
    }
}

function loadActivityDetailBehavior() {
    $("#table-behavior").html("");
    $.ajax({
        url: 'TimeAttendance.ashx?q=taskbehavior&aobj=' + $("#txtHiddenAObjectlink").val(),
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
            enableEditor: false,
            enableReply: false,
            pagemode: "ACTIVITY",
            onReplySuccess: function () {
                $("html,body").scrollTop($(".system-message-comment-container-remark").last().offset().top);
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
            }
        });
    }
    
    loadActivityDetailRemarkComment(objlink);
}

function loadActivityDetailRemarkComment(objlink) {
    $("#remark-table").activityRemark({
        aobjectlink:objlink,
        allowDivider: true,
        onContinue: function () {
            $("html,body").animate({
                scrollTop: $(".system-message-comment-container-remark").last().offset().top
            }, 100);
            $('.nano').nanoScroller();
        },
        getData:{
            url: servictWebDomainName + "TimeAttendance/TimeAttendance.ashx",
            key:{
                q: "taskremark",
                obj: objlink
            }
        },
        postData: {
            url: servictWebDomainName + 'widget/SystemMessageFormAPI.aspx',
            key:{
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
            url: servictWebDomainName + "timeattendance/TimeAttendance.ashx",
            key: {
                q: "editremark"
            }
            //remarkKey : AUTO_POST
            //remarkMessage : AUTO_POST
        }
    });
}

function loadActivityDetailRemarkEmail(objlink, myID, mainFullname, mainMessage, mainCreateOn, mainImage, mainCreateBy) {

    document.getElementById("remark-email-table").innerHTML = "";


    $.ajax({
        url: 'TimeAttendance.ashx?q=taskemail&obj=' + objlink,
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;

            for (var i = 0; i < data.length; i++) {
                var tab = document.getElementById("remark-email-table");
                var row = tab.insertRow(tab.rows.length);
                row.style.background = data[i].LINKID == myID ? "#F3FCBB" : "";
                var cell0 = row.insertCell(0);
                var cell1 = row.insertCell(1);
                cell0.className = "remark-header";
                 cell1.innerHTML = '<p><b>' + data[i].EMAIL_FROM + '</b><span class="remark-datetime">' + data[i].CREATE_DATE + '</span></p><p><span>' + data[i].EMAIL_MESSAGE.split("\n").join("<br>") + '</span></p>';
              //  cell1.innerHTML = '<p><b>' + data[i].EMAIL_FROM + '</b><span class="remark-datetime">' + data[i].CREATE_DATE + '</span></p><p><textarea  rows="2" cols="20" style="height:100px;width:100%;resize: none">' + data[i].EMAIL_MESSAGE + '</textarea></p>';



            }
            //================ Load nano scrollbar
            $('.nano').nanoScroller();
        }
    });
}

function loadActivityInvoiceDetail(docunumber,docytype,fiscalyaer)
{
    $.ajax({
        url: 'TimeAttendance.ashx?q=getInvoiceDetail&artype=' + docytype + '&aryear=' + fiscalyaer + '&arnumber=' + docunumber,
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
                }

                var _invoiceKey = data[0].DocumentType + "|" + data[0].FiscalYear + "|" + data[0].DocumentNo;
                $("#hdfInvoiceKey").val(_invoiceKey);

                if ($('#detail_customer_code').length > 0) {
                    $('#detail_customer_code').html(data[0].CustomerCode + ' - ' + data[0].CustomerNameF1);
                    $('#hdfInvoiceCustomerCode').val(data[0].CustomerCode);
                }

                // add attribute btnInvoiceSendMail
                $('#btnInvoiceSendMail').click(function () {
                    agroLoading(true);
                    $(".tab-panel-header").removeClass("tab-panel-active");
                    $("#tab-mailContent").addClass("tab-panel-active");
                    $(".tab-panel-container-content").hide();
                    $(".tab-mailContent").fadeIn();
                    $("#btnRefreshContactEmail").click();
                });

                //if ($('#detail_customer_name').length > 0) {
                //    $('#detail_customer_name').html(data[0].CustomerNameF1);
                //} 
                if ($('#detail_customer_adddress').length > 0) {
                    $('#detail_customer_adddress').html(data[0].CusAddress);
                }
                if ($('#detail_customer_contact').length > 0) {

                    var edit = "<img src='../images/icon/Edit.png' width='18px' height='18px' title='แก้ไขผู้ติดต่อ' style='cursor: pointer;margin-left: -30px;' onclick=\"openModalSearchContact();\"></img>";
                    var display = "<span style='padding-left: 10px;'>ไม่ระบุ</span>";

                    if (data[0].ContactName != null && data[0].ContactName != undefined && data[0].ContactName != "") {
                        display = "<a style='cursor: pointer; padding-left: 10px;' onclick=\"window.open('/web/master/MasterConfig/ContactMaster.aspx?id=" + data[0].ContactID + "');\">" + data[0].ContactName + "</a>";
                    } 

                    $('#detail_customer_contact').html(edit + display);
                }
                if ($('#detail_customer_contact_tel').length > 0) {
                    if (data[0].ContactPhone == "") {
                        $('#detail_customer_contact_tel').html('ไม่ระบุ');
                    } else
                    {
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
                if ($('#detail_invoice_overdue_day').length > 0) {
                    var x = '<span> ' + data[0].DueDay + ' </span> วัน';

                    if (parseInt(data[0].DueDay) < 0) {
                        x = '<span style="color: red;"> ' + data[0].DueDay + ' วัน</span>';
                    }

                    $('#detail_invoice_overdue_day').html(x);
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


                //$('#btnRefreshContactManagement').click();               
            }
        }
    });
}

function loadActivityPODetail(docunumber, docytype, fiscalyaer, datein, empcode, seq, snaid) {
    $.ajax({        
        url: 'TimeAttendance.ashx?q=getPODetail&potype=' + docytype + '&poyear=' + fiscalyaer + '&ponumber=' + docunumber + '&DATEIN=' + datein + '&SEQ=' + seq + '&EMPCODE=' + empcode + '&SNAID=' + snaid,
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
                    var readedTextColor = "grey";
                    var readedTextTitle = "ยังไม่รับทราบ";

                    if (data[0].XSTATUS == "READED") {
                        readedTextTitle = "รับทราบแล้ว";
                        readedTextColor = "blue";
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
        url: 'TimeAttendance.ashx?q=checkPOTrackingStatus&aobjectlink=' + $("#txtHiddenAObjectlink").val(),
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
        url: 'TimeAttendance.ashx?q=taskparty&obj=' + $("#txtHiddenAObjectlink").val() + '&SNAID=' + $("#txtHiddenCompanyCode").val(),
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
                    var readedTextColor = "grey";
                    var readedTextTitle = "ยังไม่รับทราบ";
                    if (data[i].XSTATUS == "READED") {
                        readedTextTitle = "รับทราบแล้ว";
                        readedTextColor = "blue";
                    }
                    if (data[i].XSTATUS == "SUCCESS") {
                        readedTextTitle = "เสร็จสิ้นแล้ว";
                        readedTextColor = "green";
                    }

                    strOtherDelegate += strOtherDelegate == "" ? "" : ",";
                    strOtherDelegate += "<span title='" + readedTextTitle + "' style='color:" + readedTextColor + "'>" + data[i].OTHER_FULLNAME + "</span>";

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
                if (ItemType == "009") {
                    $("#detail_inv_other_delegate").html(strOtherDelegate);
                }
            }
            else {
                $(".table_detail_other").hide();
                $("#table_detail_other_empty").show();
                $("#detail_other_delegate").html("<span style='color:#000'>ไม่มีผู้รับมอบหมายอื่น</span>");
                $("#detail_other_delegatePO").html("<span style='color:#000'>ไม่มีผู้รับมอบหมายอื่น</span>");
                if (ItemType == "009") {
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

function loadActivityDetailSaleContact(aobjectlink) {
    $.ajax({
        url: 'TimeAttendance.ashx?q=getSaleContactDetail&aobjectlink=' + aobjectlink,
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            if (data.length > 0) {

                $('#hdfContactMode').val("saleContact");

                if ($('#sale_customer_detail').length > 0) {
                    $('#sale_customer_detail').html(data[0].CustomerCode + ' - ' + data[0].CustomerNameF1);
                    $('#hdfInvoiceCustomerCode').val(data[0].CustomerCode);
                }

                if ($('#sale_customer_address').length > 0) {
                    $('#sale_customer_address').html(data[0].CusAddress);
                }

                if ($('#sale_customer_contact').length > 0) {

                    var edit = "<img src='../images/icon/Edit.png' width='18px' height='18px' title='แก้ไขผู้ติดต่อ' style='cursor: pointer;margin-left: -30px;' onclick=\"openModalSearchContact();\"></img>";
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

                //$('#btnRefreshContactManagement').click();
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
    $("#main-form-filter-header").html("Inbox").focusElementByColor();
    $("#main-form-filter-code").html("");
    //FilterActivity();
}

function afterRemark() {
    $("#txthiddenSendEmailForEditActivity").val("false");
    $("#txt_detail_remark").val("");
}

function closeActivityDetial() {
    $("#tab-panel-main").click();
    $("#tab-panel-activity").hide();
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
function openIfraneCreateActivity(aobj, rowkey, snaid) {
    var ifca = document.getElementById("iframeCreateActivity");
    if (aobj != undefined)
        ifca.src = '../widget/PopupCreateActivityRedesign.aspx?aobj=' + aobj + '&rowkey=' + rowkey + '&snaid=' + snaid;
    else
        ifca.src = '../widget/PopupCreateActivityRedesign.aspx?project=' + $("#main-form-filter-code").html();

}

function openIframeCreateQuickNote() {
    var ifca = document.getElementById("iframeCreateActivity");

    ifca.src = '../widget/PopupCreateActivityReDesign.aspx?mode=quicknote&project=' + $("#main-form-filter-code").html();
}

function openIframeCreateQuickTask() {
    var ifca = document.getElementById("iframeCreateActivity");

    ifca.src = '../widget/PopupCreateActivityReDesign.aspx?mode=quicktask&project=' + $("#main-form-filter-code").html();
}

function openIframeCreateNote() {
    var ifca = document.getElementById("iframeCreateActivity");

    ifca.src = '../widget/PopupCreateActivityReDesign.aspx?mode=createNote&project=' + $("#main-form-filter-code").html();
}

function openIframeCreateTask() {
    var ifca = document.getElementById("iframeCreateActivity");

    ifca.src = '../widget/PopupCreateActivityReDesign.aspx?mode=createTask&project=' + $("#main-form-filter-code").html();
}

function openIframeCreateMeeting() {
    var ifca = document.getElementById("iframeCreateActivity");

    ifca.src = '../widget/PopupCreateActivityReDesign.aspx?mode=createMeeting&project=' + $("#main-form-filter-code").html();
}

function openIframeCreateSalesTask() {
    var ifca = document.getElementById("iframeCreateActivity");

    ifca.src = '../widget/PopupCreateActivityReDesign.aspx?mode=createSales&project=' + $("#main-form-filter-code").html();
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
    alertMessage("บันทึกเรียบร้อยแล้ว");
    closeIfraneCreateActivity();
    closeActivityDetial();

    refreshAllNotification();
    StartActivityInbox();
    //if (oldAobj == undefined) {
    //    LoadActivityInboxRow(aobj, undefined, true);
    //}
    //else {
    //    LoadActivityInboxRow(aobj, oldAobj, true);
    //}
}

//for iframe select other delegate
function openIfraneSelectOtherDelegate() {
    var aobj = $("#txtHiddenAObjectlink").val();
    var rowkey = $("#txtHiddenRowkey").val();
    var snaid = $("#txtHiddenCompanyCode").val();
    var ifca = document.getElementById("iframeSelectOtherDelegate");
    ifca.src = '../TimeAttendance/PopupSelectOtherDelegate.aspx?aobj=' + aobj + '&rowkey=' + rowkey + '&snaid=' + snaid;
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
    alertMessage("บันทึกเรียบร้อยแล้ว");
    closeIfraneSelectOtherDelegate();
    loadActivityDetail($("#txtHiddenAObjectlink").val());
}

//for ifram change detail
function openIframeChangeOtherDetail() {
    var aobj = $("#txtHiddenAObjectlink").val();
    var ifca = document.getElementById("iframeChangeOtherDetail");
    ifca.src = '../TimeAttendance/PopupChangeOtherDetail.aspx?aobj=' + aobj;
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
    alertMessage("บันทึกเรียบร้อยแล้ว");
    closeIframeChangeOtherDetail();
    loadActivityDetail($("#txtHiddenAObjectlink").val());
    CallbackActivityEvent("other");
}

//for ifram change task status
function openIframeChangeTaskStatus(aobjectLink) {
    var aobj = aobjectLink == undefined ? $("#txtHiddenAObjectlink").val() : aobjectLink;
    var ifca = document.getElementById("iframeChangeTaskStatus");
    ifca.src = '../TimeAttendance/PopupChangeActivityDetail.aspx?aobj=' + aobj;
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
    alertMessage("บันทึกเรียบร้อยแล้ว");
    closeIframeChangeTaskStatus();
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
    ifca.src = '../TimeAttendance/PopupSelectMainDelegate.aspx?aobj=' + aobj + '&rowkey=' + rowkey + '&snaid=' + snaid;
}

function closeIfraneSelectMainDelegate() {
    var ifca = document.getElementById("iframeSelectMainDelegate");
    $(ifca).fadeOut();
}

function showIfraneSelectMainDelegate() {
    var ifca = document.getElementById("iframeSelectMainDelegate");
    $(ifca).fadeIn();
}

function successIfraneSelectMainDelegate(newAobj, newRowKey) {
    var oldAobj = $("#txtHiddenAObjectlink").val();
    
    $("#txtHiddenRowkey").val(newRowKey);
    $("#txtHiddenAObjectlink").val(newAobj);

    alertMessage("บันทึกเรียบร้อยแล้ว");
    closeIfraneSelectMainDelegate();
    loadActivityDetail($("#txtHiddenAObjectlink").val());
    CallbackActivityEvent("maindelegate", oldAobj);
}

//for iframe change history main delegate
function openIframeChangeMainDelegateHistory() {
    var aobj = $("#txtHiddenAObjectlink").val();

    var ifca = document.getElementById("iframeChangeMainDelegateHistory");
    ifca.src = '../TimeAttendance/PopupChangeMainDelegateHistory.aspx?aobj=' + aobj;
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

    alertMessage("บันทึกเรียบร้อยแล้ว");
    closeIframeChangeMainDelegateHistory();
    loadActivityDetail($("#txtHiddenAObjectlink").val());
}

function sendActivityEmail(subject, myName, emailfrom) {
    $('#modelSendEmail').modal('show');
    $('#txtSubjectSendMail').val(subject);
    $("#divContainerEmail").html("");
    $("#txtAddEmail").val(emailfrom);
    $('#btnAddEmail').click();
    //$('#txtAddEmail').attr('readonly', true);
    
    $("#txtRemarkSendMail").val("");
    $('#txtFromSendMail').val(myName);
    $("#labelTypeSendEmail").html("Activity");
    $("#hidTypeSendEmail").val("ACTIVITY");
}

function sendCustomEmail(subject, myName) {
    $('#modelSendEmail').modal('show');
    $('#txtSubjectSendMail').val(subject);
    $("#divContainerEmail").html("");
    $("#txtAddEmail").val("");

    $("#txtRemarkSendMail").val("");
    $('#txtFromSendMail').val(myName);
    $("#labelTypeSendEmail").html("Custom");
    $("#hidTypeSendEmail").val("CUSTOM");
}

function addMailToContainer() {
    if ($("#txtAddEmail").val().trim() != '')
        $("#divContainerEmail").append(
            "<div style='border:1px solid #ccc;padding:5px;padding-left:15px;margin-top:5px;border-radius:5px;background:#eee'><span class='container-email'>" + $("#txtAddEmail").val() +
            "</span><i class='glyphicon glyphicon-trash' style='float:right;cursor:pointer;cursor:hand' onclick='$(this).parent().remove();'></i></div>"
        );
    $("#txtAddEmail").val("");
}

function prepareSendmail() {
    if ($(".container-email").length == 0) {
        alertMessage("กรุณาเพิ่มอีเมลที่คุณจะส่งถึง");
        return false;
    }
    if (confirm("ยืนยันจะส่งอีเมลนี้หรือไม่")) {
        var mail = [];
        $(".container-email").each(function () {
            mail.push($(this).html());
        });
        $("#txtAllSendMailContainer").val(mail);
        agroLoading(true);
    }
    else
        return false;

}

function successSendCustomEmail() {
    alertMessage("ส่งอีเมลเรียบร้อยแล้ว");
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
        url: 'TimeAttendance.ashx?q=activitymoney&key=' + key + '&SNAID=' + snaid + '&revenue=' + rev + '&charges=' + char + '&currency=' + cur,
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
        url: 'TimeAttendance.ashx?q=changeOtherDelegate&aobjectid=' + aobjectlink + '&listLinkID=' + linkid,
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            $('#formAddDelegate').modal('hide');

            loadActivityDetail($("#txtHiddenAObjectlink").val());
            alertMessage("&#3610;&#3633;&#3609;&#3607;&#3638;&#3585;&#3612;&#3641;&#3657;&#3617;&#3629;&#3610;&#3627;&#3617;&#3634;&#3618;&#3629;&#3639;&#3656;&#3609;&#3648;&#3619;&#3637;&#3618;&#3610;&#3619;&#3657;&#3629;&#3618;&#3649;&#3621;&#3657;&#3623;");
        }
    });
}

function getChangeOtherDelegate() {
    var aobjectid = $("#txtHiddenAObjectlink").val();
    var parameter = "?q=getOtherDelegate&aobjectid=" + aobjectid;

    var _url = "/TimeAttendance/TimeAttendance.ashx" + parameter

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

alertAcceptOther();

function openModalSearchContact() {
    agroLoading(true);
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
            url: 'TimeAttendance.ashx?q=confirmChangeContactSale&aobjectlink=' + aobjectlink + '&contact=' + contact,
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
            url: 'TimeAttendance.ashx?q=confirmChangeContactInvoice&invkey=' + invkey + '&contact=' + contact,
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
        alertMessage("กรุณาเลือกกลุ่มงาน");
        return;
    }

    $.ajax({
        url: 'TimeAttendance.ashx?q=confirmChangeProject&aobjectid=' + aobjectlink + '&proj=' + proj + '&subproj=' + subproj,
        success: function (data) {
            if (ErrorAPIHandel(data))
                return;
            $('#formChangeProject').modal('hide');

            loadActivityDetail($("#txtHiddenAObjectlink").val());
            alertMessage("แก้ไขกลุ่มงานสำเร็จ");
        }
    });
}

function showModalKM() {
    //$('#popupKM').attr('src','#');
    $('.popupREFKM').modal('show');
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
    $('#popupKM').attr('src', src + '?business=' + refBusiness + '&doctype=' + refDocType + '&docnumber=' + refDocNumber + '&docyear=' + refDocYear);
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
        var _url = "/TimeAttendance/TimeAttendance.ashx?q=getAvtivityCount";
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
        $("#main-form-filter-header").html("Inbox");

        $('.search-all-activity,.select-activity-type-filter').val("");
        $('.select-activity-client-filter').val(status);
        StartActivityInbox();
    },300);
}

function blinker() {
    $('.reOpen-flash').fadeOut(800);
    $('.reOpen-flash').fadeIn(800);
}

function momClick() {
    var _aobjectlink = $('#txtHiddenAObjectlink').val();
    window.open('/TimeAttendance/MinutesOfMeeting.aspx?aobjectlink=' + _aobjectlink);
}

setInterval(blinker, 800);

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
            alertMessage("&#3652;&#3617;&#3656;&#3614;&#3610;&#3619;&#3634;&#3618;&#3585;&#3634;&#3619;&#3607;&#3637;&#3656;&#3588;&#3657;&#3609;&#3627;&#3634;");
    }
}

function disibleChangeActivityButtonAddOtherDelegate() {
    var tab = document.getElementById("tableChangeOtherDelegate");
    for (var i = 0; i < tab.rows.length; i++) {
        $("." + tab.rows[i].cells[0].innerHTML.trim() + "CAOD").attr("disabled", "");
    }
    $("." + $("#txtHiddenUserLinkID").val() + "CAOD").attr("disabled", "");

    var aobjectid = $("#txtHiddenAObjectlink").val();
    var parameter = "?q=disabledLINKID&aobjectid=" + aobjectid;
    var _url = "/TimeAttendance/TimeAttendance.ashx" + parameter
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
            alertMessage(name + " &#3617;&#3637;&#3619;&#3634;&#3618;&#3594;&#3639;&#3656;&#3629;&#3629;&#3618;&#3641;&#3656;&#3651;&#3609;&#3619;&#3634;&#3618;&#3585;&#3634;&#3619;&#3649;&#3621;&#3657;&#3623;");
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
    cell3.innerHTML = "";
    cell3.colSpan = 2;
    cell4.innerHTML = type;
    cell4.style.display = "none";
    $(obj).attr("disabled", "");
}

function addChangeactivityOtherDelegate(id, name, type, obj) {
    alertAcceptCh = true;
    var tab = document.getElementById("tableChangeOtherDelegate");
    for (var i = 0; i < tab.rows.length; i++) {
        if (id == tab.rows[i].cells[0].innerHTML) {
            alertMessage(name + " &#3617;&#3637;&#3619;&#3634;&#3618;&#3594;&#3639;&#3656;&#3629;&#3629;&#3618;&#3641;&#3656;&#3651;&#3609;&#3619;&#3634;&#3618;&#3585;&#3634;&#3619;&#3649;&#3621;&#3657;&#3623;");
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
    cell3.innerHTML = "<a style='color:red' href='#' onclick='removeRowOtherDelegate(\"" + id + "\");'>&#3621;&#3610;</a>";
    cell3.colSpan = 2;
    cell4.innerHTML = type;
    cell4.style.display = "none";
    $(obj).attr("disabled", "");
}

function removeRowOtherDelegate(id) {
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
    document.getElementById('changeAttachFileFrame').contentWindow.saveFile(aobj, snaid)

    $('#formAddAttachFile').modal('hide');
    loadActivityDetail($("#txtHiddenAObjectlink").val());
    alertMessage("&#3648;&#3614;&#3636;&#3656;&#3617;&#3652;&#3615;&#3621;&#3660;&#3649;&#3609;&#3610;&#3648;&#3619;&#3637;&#3618;&#3610;&#3619;&#3657;&#3629;&#3618;&#3649;&#3621;&#3657;&#3623;");
}

function callIFrameNewInitFile() {
    document.getElementById('changeAttachFileFrame').contentWindow.initNewAttachFile()
    $('#formAddAttachFile').modal('show');
}