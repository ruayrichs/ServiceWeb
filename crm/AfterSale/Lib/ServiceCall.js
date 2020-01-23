
// --------------------------------------------------- Validate  --------------------------------------------------- //
function validateRequireField() {
    var isSuccess = true;
    $("#nav-header .required, #nav-item .required").each(function () {
        var input = $(this);
        if (input.val().trim() == "") {
            var text = input.closest(".form-group").find(">:first-child").html().trim();
            var message = text + " is required.";

            swal({
                title: "",
                text: message,
                type: "error",
                customClass: 'swal-wide',
                html: true
            }, function () {
                var tab = input.closest(".tab-pane").attr("aria-labelledby");
                $("#" + tab).click();
                setTimeout(function () {
                    input.focus();
                }, 500);
            });

            isSuccess = false;
            return false;
        }
    });

    return isSuccess;
}

function saveClick(sender, isNoteArea) {

    if (validateRequireField() && validateIncidentAreaField(isNoteArea)) {
        var mode = "create";

        try {
            if ($("#_txt_docnumberTran").val() != "") {
                mode = "update";
            }
        } catch (e) {

        }

        if (AGConfirm(sender, "Confirm " + mode + " ticket ?")) {
            AGLoading(true);
            $(sender).next().click();
        }
    }
}

function validateIncidentAreaField(isNoteArea) {
    if (isNoteArea == "True") {
        return true;
    }
    var IncidentArea = $("#hddIncidentAreaCode").val();
    if (IncidentArea == "") {
        $("#nav-item-tab").click();
        AGError("กรุณาเลือก Incident Area!");
        return false;
    }
    return true;
}

function confirmReOpenTicket(obj) {
    if (AGConfirm(obj, 'ยืนยันการ Re-Open Ticket หรือไม่')) {
        AGLoading(true);
        return true;
    }
    return false;
}
function confirmCancelTicket(obj) {
    if (AGConfirm(obj, 'Confirm cancel ticket ?')) {
        AGLoading(true);
        return true;
    }
    return false;
}

function confirmResetValueDefault(obj) {
    if (AGConfirm(obj, 'Confirm reset value to default ?')) {
        AGLoading(true);
        return true;
    }
    return false;
}


function confirmResetTicket(obj) {
    if (AGConfirm(obj, 'Confirm reset change order ?')) {
        $(obj).next().click();
        AGLoading(true);
        return true;
    }
    return false;
}


function confirmRollBackTicket(obj) {
    if (AGConfirm(obj, 'Confirm roll back change order ?')) {
        $(obj).next().click();
        AGLoading(true);
        return true;
    }
    return false;
}
// ------------------------------------------------- End Validate  ------------------------------------------------- //

// --------------------------------------------------- Hierarchy  --------------------------------------------------- //
var IncidentAreaFilterAbleCount = 0;
var IncidentAreaFilterAbleTimeout = null;

function bindHierarchyReferFrom(rootCount) {
    $("#hierarchyReferFrom").AGWhiteLoading(true, "กำลังดึงข้อมูล");
    var TicketNo = $("#hddDocnumberTran").val();

    $.ajax({
        url: servictWebDomainName + "API/ServiceTicketAPI/ReferFromTicketAPI.aspx",
        data: {
            request: "get_hierarchy",
            CustomerCode: CustomerCode,
            TicketType: TicketType,
            TicketNo: TicketNo
        },
        success: function (datas) {
            $("#hierarchyReferFrom").aGapeTreeMenu({
                data: datas,
                rootText: "Relation Tree",
                rootCode: "",
                rootCount: 0,
                navigateText: "Create structure",
                onlyFolder: false,
                share: false,
                moveItem: false,
                selecting: false,
                emptyFolder: true,
                removeSwitchEmptyFolder: true,
                onClick: function (result) {
                    if (result.id) {
                        var thisItem = $("#hierarchyReferFrom [data-node-id='" + result.id + "']");

                        $("#hddDoctype_OpenRelation").val(thisItem.attr("data-param-doctype"));
                        $("#hddFiscalYear_OpenRelation").val(thisItem.attr("data-param-fiscalyear"));
                        $("#hddTicketNo_OpenRelation").val(result.id);
                        $("#btnOpenTicketRelation").click();
                    }
                },
                //onMove: function (result) {
                //    if (result.newParentNode == result.oldParentNode || result.itemType == "e") {
                //        bindHierarchyReferFrom();
                //    }
                //    else {
                //        hierarchyReferFromDoAjax({
                //            request: "movenode",
                //            newParentNode: result.newParentNode,
                //            itemNode: result.itemNode,
                //            itemName: result.itemName,
                //            itemType: result.itemType,
                //            CustomerCode: CustomerCode,
                //            TicketType: TicketType
                //        });
                //    }
                //}
            });
            $("[data-node-id='" + TicketNo + "']").addClass("hierarchy-select");
            $("#hierarchyReferFrom").AGWhiteLoading(false);
        }
    });
}

function createdHierarchyRefer(name, ref) {
    AGLoading(true);
    var parentid = "";
    if (ref) {
        parentid = '';
    }
    var datas = {
        request: "newfolder",
        parentid: parentid,
        name: name,
        CustomerCode: CustomerCode,
        tk_ref: ref,
        TicketType: TicketType
    }
    $.ajax({
        url: servictWebDomainName + "API/ServiceTicketAPI/ReferFromTicketAPI.aspx",
        data: datas,
        success: function () {
            //bindHierarchyReferFrom();
            window.location.href = window.location.href;
        },
        error: function () {
            //bindHierarchyReferFrom();
            window.location.href = window.location.href;
        }
    });
}

function hierarchyReferFromDoAjax(datas) {
    $("#hierarchyReferFrom").AGWhiteLoading(true, "กำลังดึงข้อมูล");
    $.ajax({
        url: servictWebDomainName + "API/ServiceTicketAPI/ReferFromTicketAPI.aspx",
        data: datas,
        success: function () {
            bindHierarchyReferFrom();
        },
        error: function () {
            bindHierarchyReferFrom();
        }
    });
}

function IncidentAreaFilterAble(obj) {
    var val = $(obj).val().trim().toLowerCase();
    var li = $("#hierarchyIncidentTicket .agape-tree-li,#hierarchyIncidentTicket .agape-tree-li-root");

    var count = 0;
    if (val != "") {
        li.each(function () {
            var node = $(this).find(".agape-show-node-name:first");
            var nodeName = node.html().toLowerCase();
            if (nodeName.match(val)) {
                node.addClass("search-highlight");
                count++;
            }
            else {
                node.removeClass("search-highlight");
            }
        });

        $(".search-result-box").toggle(count > 0);
        $("#card-incident-area").scrollTop(count > 0 ? 40 : 0);
        $(".search-result-command-at").html(count > 0 ? 1 : 0);
        $(".search-result-command-all").html(count);
    } else {
        li.find(".agape-show-node-name").removeClass("search-highlight");
        $(".search-result-box").hide();
        $("#card-incident-area").scrollTop(0);
    }

    IncidentAreaFilterAbleCount = count;
    $(".search-result").html("Search result : " + count);

    if (IncidentAreaFilterAbleTimeout != null) {
        clearTimeout(IncidentAreaFilterAbleTimeout);
    }
    setTimeout(function () {
        IncidentAreaFilterAbleFocus();
    }, 500);
}

function IncidentAreaFilterAbleFocusClick(mode) {
    var at = parseInt($(".search-result-command-at").html());
    var all = parseInt($(".search-result-command-all").html());
    if (mode == "NEXT") {
        at++;
    } else {
        at--;
    }

    if (at > all)
        at = 1;
    if (at == 0)
        at = all;

    $(".search-result-command-at").html(at);
    IncidentAreaFilterAbleFocus();
}

function IncidentAreaFilterAbleFocus() {
    try {
        if ($("#filter-able-IncidentArea").val() == "") {
            $("#card-incident-area").scrollTop(0);
        }
        var at = parseInt($(".search-result-command-at").html()) - 1;
        var hTop = $(".agape-show-node-name.search-highlight:eq(" + at + ")").position().top;
        $("#card-incident-area").scrollTop(hTop - 35);
    } catch (e) {

    }
}


function bindHierarchyIncidentTicket(rootCount) {
    $("#hierarchyIncidentTicket").AGWhiteLoading(true, "กำลังดึงข้อมูล");
    $.ajax({
        url: servictWebDomainName + "API/ServiceTicketAPI/IncidentTicketTicketAPI.aspx",
        data: {
            request: "get_hierarchy",
            AreaCode: AreaCode
        },
        success: function (datas) {
            setTimeout(function () {
                if ($("#hddIncidentAreaCode").val() != '') {
                    $("#hierarchyIncidentTicket [data-node-id='" + $("#hddIncidentAreaCode").val() + "']").addClass("hierarchy-select");
                    var dataTarget = $("#hierarchyIncidentTicket").find("[data-node-id='" + $("#hddIncidentAreaCode").val() + "']");
                    $("#lbl-incident-area-selected").html("(" + dataTarget.find(".agape-show-node-name").attr('title') + ")");
                } else {
                    $("#lbl-incident-area-selected").html("");
                }
            }, 1000);

            $("#hierarchyIncidentTicket").aGapeTreeMenu({
                data: datas,
                rootText: $("#ddlEquipmentNo :selected").text(),
                rootCode: "",
                rootCount: 0,
                navigateText: "Create structure",
                onlyFolder: false,
                share: false,
                moveItem: false,
                selecting: false,
                emptyFolder: true,
                removeSwitchEmptyFolder: true,
                onClick: function (result) {
                    if (result.id == '') {
                        return;
                    }
                    if ($("#ddlEquipmentNo").prop("disabled")) {
                        AGMessage('ไม่สามารถเปลียนแปลง Incident Area ได้');
                        return;
                    }

                    var dataTarget = $("#hierarchyIncidentTicket").find("[data-node-id='" + result.id + "']");
                    if (dataTarget.hasClass('hierarchy-select')) {
                        dataTarget.removeClass("hierarchy-select");

                        $("#lbl-incident-area-selected").html("");
                        $("#hddIncidentAreaCode").val('');
                        $("#btnSelectIncidentArea").click();
                    } else {
                        $(".agape-tree-menu-folder").removeClass("hierarchy-select");
                        dataTarget.addClass("hierarchy-select");

                        $("#lbl-incident-area-selected").html(result.name);
                        $("#hddIncidentAreaCode").val(result.id);
                        $("#btnSelectIncidentArea").click();
                    }
                }
            });
            $("#hierarchyIncidentTicket").AGWhiteLoading(false);
        }
    });
}

function hierarchyIncidentTicketDoAjax(datas) {
    $("#hierarchyIncidentTicket").AGWhiteLoading(true, "กำลังดึงข้อมูล");
    $.ajax({
        url: servictWebDomainName + "API/ServiceTicketAPI/IncidentTicketTicketAPI.aspx",
        data: datas,
        success: function () {
            bindHierarchyIncidentTicket();

        },
        error: function () {
            bindHierarchyIncidentTicket();
        }
    });
}
// ------------------------------------------------- End Hierarchy  ------------------------------------------------- //

function changeEquipment(sender) {
    $(sender).next().find("input[type='submit']").first().click();
}

function getremarkservice(aobjectlink) {
    $("#ag-remark").AGActivityRemark(aobjectlink, FocusOneLinkProfileImage);
}

function bindDataTableInEmployeeParticipantSelected(data, TableID, AutoCompleteClientID) {
    if (data == null || data == "" || data.code == "") {
        return;
    }
    var item = $("#table-" + TableID + " [data-column='emp-code']");
    var isSave = true;
    item.each(function () {
        if ($(this).attr("data-value") == data.code) {
            if ($(this).parent().hasClass("row-default") && !$(this).parent().is(":visible")) {
                $(this).parent().removeClass('d-none');
                $(this).parent().attr("data-event", "DEFAULT")
                isSave = false;
                eval("RefreshValueAutoCompleteEmployee" + AutoCompleteClientID)("");
                return;
            } else {
                AGError("duplicate is " + data.code + " :" + data.desc);
                isSave = false;
                eval("RefreshValueAutoCompleteEmployee" + AutoCompleteClientID)("");
                return;
            }
        }
    });

    if (!isSave) {
        return;
    }

    appendSearchMaterialSelected(data.code, data.desc, TableID);
    eval("RefreshValueAutoCompleteEmployee" + AutoCompleteClientID)("");
}
function appendSearchMaterialSelected(empcode, empname, TableID) {
    var container = $("#table-" + TableID);

    var tr = $("<tr/>", {
        class: "row-emp row-new",
        "data-event": "ADD"
    });
    var td_1 = $("<td/>", {
        class: "text-center",
        html: '<input type="radio" name="rdo-TransferMain" class="ticket-allow-editor" value="' + empcode + '" data-default-main="false" />'
    });
    var td_2 = $("<td/>", {
        class: "text-center",
        html: '<span class="img-box-ini-style img-box-transfer" style="background-image: url(' + servictWebDomainName + 'images/user.png);"></span>'
    });
    var td_3 = $("<td/>", {
        "data-column": "emp-code",
        "data-name": empname,
        "data-value": empcode,
        class: "nowrap",
        html: empcode
    });
    var td_4 = $("<td/>", {
        html: empname
    });
    var td_5 = $("<td/>", {
        class: "text-center",
        html: '<a class="fa fa-trash" style="color: red;" href="javascript:;"></a>',
        "onClick": "removeRowEmpParticipant(this);"
    });

    tr.append(td_1, td_2, td_3, td_4, td_5);
    container.append(tr);
}

function removeRowEmpParticipant(obj) {
    if (AGConfirm(obj, "Do you want remove this employee ?")) {
        if ($(obj).parent().hasClass("row-default")) {
            $(obj).parent().addClass("d-none");
            $(obj).parent().attr("data-event", "REMOVE")
        } else {
            $(obj).parent().remove();
        }
        return;
    }
    return;
}

function prepareDateForSaveEmpParticipant(obj, TableID) {
    var item = $("#table-" + TableID).find("[data-column='emp-code']");
    if (item.length <= 0) {
        AGError("Please select participant.");
        return false;
    }

    if ($("#table-" + TableID + " input[type='radio']:checked:visible").length == 0) {
        AGError("Please select Main.");
        return false;
    }
    if (AGConfirm(obj, "Do you want save participant ?")) {
        AGLoading(true);
        var arrEmp = [];
        var TaskName = $("#hddTransfer_TaskName").val();

        $("#table-" + TableID).find("[data-column='emp-code']").each(function () {
            var EmpName = $(this).attr("data-name");
            var EmpCode = $(this).attr("data-value");
            var IsMain = $(this).parent().find("input[type='radio']").prop("checked");
            var Event = $(this).parent().attr("data-event");
            var DefaultMain = $(this).parent().find("input[type='radio']").attr("data-default-main").toLowerCase() == "true";
            var EventDesc = "";
            if (Event == "REMOVE") {
                EventDesc += "Remove '" + EmpCode + " :: " + EmpName + "' in tier name '" + TaskName + "'.";
            } else if (Event == "ADD") {
                EventDesc += "Add '" + EmpCode + " :: " + EmpName + "' in tier name '" + TaskName + "'.";
            }
            if (IsMain && !DefaultMain) {
                if (EventDesc) {
                    EventDesc += "\n";
                }
                EventDesc += "Assign '" + EmpCode + " :: " + EmpName + "' to main delegate in tier name '" + TaskName + "'.";
            }

            arrEmp.push({
                EmpName: EmpName,
                EmpCode: EmpCode,
                IsMain: IsMain,
                Event: Event,
                EventDesc: EventDesc,
                DefaultMain: DefaultMain
            });
        });
        $("#hddTransfer_ListEMPCode").val(JSON.stringify(arrEmp));

        return true;
    }

    return false;
}

function prepareDateForSaveEmpParticipant_Change(TableID) {
    var item = $("#table-" + TableID).find("[data-column='emp-code']");
    if (item.length <= 0) {
        AGError("Please select owner service.");
        $("#nav-participants-tab").click();
        return false;
    }

    if ($("#table-" + TableID + " input[type='radio']:checked").length == 0 ||
        $("#table-" + TableID + " input[type='radio']:checked").closest('tr').hasClass('d-none')) {
        AGError("Please select Main.");
        return false;
    }

    var arrEmp = [];
    var TaskName = $("#hddTransfer_TaskName").val();

    $("#table-" + TableID).find("[data-column='emp-code']").each(function () {
        var EmpName = $(this).attr("data-name");
        var EmpCode = $(this).attr("data-value");
        var IsMain = $(this).parent().find("input[type='radio']").prop("checked");
        var Event = $(this).parent().attr("data-event");
        var DefaultMain = $(this).parent().find("input[type='radio']").attr("data-default-main").toLowerCase() == "true";
        var EventDesc = "";
        if (Event == "REMOVE") {
            EventDesc += "Remove '" + EmpCode + " :: " + EmpName + "' in tier name '" + TaskName + "'.";
        } else if (Event == "ADD") {
            EventDesc += "Add '" + EmpCode + " :: " + EmpName + "' in tier name '" + TaskName + "'.";
        }
        if (IsMain && !DefaultMain) {
            if (EventDesc) {
                EventDesc += "\n";
            }
            EventDesc += "Assign '" + EmpCode + " :: " + EmpName + "' to main delegate in tier name '" + TaskName + "'.";
        }

        arrEmp.push({
            EmpName: EmpName,
            EmpCode: EmpCode,
            IsMain: IsMain,
            Event: Event,
            EventDesc: EventDesc,
            DefaultMain: DefaultMain
        });
    });
    $("#hddOwner_ListEMPCode").val(JSON.stringify(arrEmp));
    return true;

}

function prepareDateForSaveEmpParticipant_Changeorder(obj, TableID) {
    var item = $("#table-" + TableID).find("[data-column='emp-code']");
    if (item.length <= 0) {
        AGError("Please select participant.");
        return false;
    }

    if ($("#table-" + TableID + " input[type='radio']:checked:visible").length == 0) {
        AGError("Please select Main.");
        return false;
    }
    if (AGConfirm(obj, "Do you want save participant ?")) {
        AGLoading(true);
        var arrEmp = [];
        var TaskName = $("#hddTransferChangeOrder_TaskName").val();

        $("#table-" + TableID).find("[data-column='emp-code']").each(function () {
            var EmpName = $(this).attr("data-name");
            var EmpCode = $(this).attr("data-value");
            var IsMain = $(this).parent().find("input[type='radio']").prop("checked");
            var Event = $(this).parent().attr("data-event");
            var DefaultMain = $(this).parent().find("input[type='radio']").attr("data-default-main").toLowerCase() == "true";
            var EventDesc = "";
            if (Event == "REMOVE") {
                EventDesc += "Remove '" + EmpCode + " :: " + EmpName + "' in tier name '" + TaskName + "'.";
            } else if (Event == "ADD") {
                EventDesc += "Add '" + EmpCode + " :: " + EmpName + "' in tier name '" + TaskName + "'.";
            }
            if (IsMain && !DefaultMain) {
                if (EventDesc) {
                    EventDesc += "\n";
                }
                EventDesc += "Assign '" + EmpCode + " :: " + EmpName + "' to main delegate in tier name '" + TaskName + "'.";
            }

            arrEmp.push({
                EmpName: EmpName,
                EmpCode: EmpCode,
                IsMain: IsMain,
                Event: Event,
                EventDesc: EventDesc,
                DefaultMain: DefaultMain
            });
        });
        $("#hddTransferChangeOrder_ListEMPCode").val(JSON.stringify(arrEmp));

        return true;
    }

    return false;
}
// ---------------------------------------------------- ---------------------------------------------------- //

function addNewEquipment() {
    $("#btnAddNewEquipment").click();
}

function removeItemEquipment(sender, ci) {
    var msg = 'Do you want to remove ' + msg + ' ?';
    if (AGConfirm(sender, msg)) {
        AGLoading(true);
        $(sender).next().click();
        return true;
    }
    return false;
}


function openModalCreateNewTicket(equipmentCode) {
    if (equipmentCode) {
        $("#hddEquepmentCodeRef").val(equipmentCode);
    } else {
        $("#hddEquepmentCodeRef").val("");
    }
    showInitiativeModal('modal-CreateNewTicketReferent');
}

function newTicketRefModalClick(sender) {
    var msg = '';
    if ($("#hddEquepmentCodeRef").val() == '') {
        msg = 'Do you want to create new ticket ?';
    } else {
        msg = "Do you want to create new ticket with configuration item " + $("#hddEquepmentCodeRef").val() + " ?";
    }

    if (AGConfirm(sender, msg)) {
        AGLoading(true);
        $("#btnCreateNewTicketRef").click();
        return true;
    }
    return false;
}

function rebindWorkflowDetail() {
    $("#btnRebindWorkFlow").click();
}

$(document).ready(function () {
    var ref = getUrlParameter('ref');
    if (ref) {
        toRelyComment();
    }
});

function selectRow(xLineNo) {
    $(".row-equipment-select").removeClass("alert-info");
    $(".row-equipment-" + xLineNo).addClass("alert-info");
}

function clickCloseWork(obj) {
    var msg = '';

    var thicketRelation = $("#hierarchyReferFrom li[data-node-id='" + $("#hddDocnumberTran").val() + "'] ul.agape-tree-menu-child li.agape-tree-menu-folder");
    for (var i = 0; i < thicketRelation.length; i++) {
        if ($(thicketRelation[i]).attr("data-param-iscloseticket").toLowerCase() == "false") {
            msg += "Need to close ticket '" + $(thicketRelation[i]).attr("data-param-name") + "'.<br />";
        }
    }

    if (msg != '') {
        AGError(msg);
        return false;
    }
    

    if (AGConfirm(obj, 'Do you want to close ticket ?')) {
        AGLoading(true);
        $(obj).next().click();
        return true;
    } else {
        return false;
    }
}

function OpenChatBox(obj, aobj, FlagCheck) {
    var isHideBoxPostRemark = $(obj).prev().val().toLowerCase() == 'true';
    $(obj).toggle();
    $(obj).next().toggle();
    if (isHideBoxPostRemark) {
        $(obj).parent().next().AGActivityRemark(aobj, FocusOneLinkProfileImage, postSuccessCallBack, hideBoxPostRemark);
    } else {
        if (FlagCheck == true) {
            $(obj).parent().next().AGActivityRemark(aobj, FocusOneLinkProfileImage, postSuccessCallBack, controlTextRemark);
        } else {
            $(obj).parent().next().AGActivityRemark(aobj, FocusOneLinkProfileImage, postSuccessCallBack, addControlTicketStatus);
        }
    }
}

function hideBoxPostRemark(a, b) {
    a.find('.system-message-comment-container-reply').hide();
    a.find(".fa-option-activity-remark").hide()
}

function CloseChatBox(obj) {
    $(obj).toggle();
    $(obj).prev().toggle();
    $(obj).parent().next().html("");
}

function newTicketClick(sender) {
    if (AGConfirm(sender, "Do you want to create new ticket ?")) {
        AGLoading(true);
        $(sender).next().click();
    }
}

function newParentTicketClick(sender, equipment) {
    if (AGConfirm(sender, "Do you want to create new ticket with configuration item " + equipment + " ?")) {
        AGLoading(true);
        $(sender).next().click();
    }
}

function updateTicketStatusClick(sender) {
    if (AGConfirm(sender, 'Do you want to update ticket status "' + $("#_ddl_ticket_Doc_Status :selected").text() + '" ?')) {
        $(sender).next().click();
    }
}

function closeTicketClick(sender) {
    if (AGConfirm(sender, 'Do you want to close ticket ?')) {
        $(sender).next().click();
    }
}

function resolveTicketClick(sender) {
    if (AGConfirm(sender, 'Confirm resolve ticket ?')) {
        AGLoading(true)
        return true;
    }
    return false;
}

function escalateTicketClick(sender) {
    if (AGConfirm(sender, 'Confirm escalate ticket ?')) {
        AGLoading(true)
        return true;
    }
    return false;
}

function controlInputDisable() {
    $("#panel-ticket-detail").find(
        "input[type='text']:disabled" +
        ",input[type='number']:disabled" +
        ",input[type='checkbox']:disabled" +
        ",input[type='radio']:disabled" +
        ",select:disabled" +
        ",textarea:disabled"
    ).addClass("ticket-default-disabled");

    $("#panel-ticket-detail").find(
        "input[type='text']" +
        ",input[type='number']" +
        ",input[type='checkbox']" +
        ",input[type='radio']" +
        ",select" +
        ",textarea"
    ).prop("disabled", true);

    var IsFirstView = $("#hddAuthenEdit").val().toLowerCase() == "true";
    if (IsFirstView) {
        $("#panel-ticket-detail").find(".ticket-allow-editor, .ticket-allow-editor input[type='checkbox']").prop("disabled", false);
        bindIconLockMode();
    } else {
        //submit
        $("#panel-ticket-detail").find(
            "input[type='button']:disabled" +
            ",input[type='submit']:disabled" +
            ",a[href*='javascript:']:disabled" +
            ",button:disabled"
        ).addClass("ticket-default-disabled");

        $("#panel-ticket-detail").find(
            "input[type='button']" +
            ",input[type='submit']" +
            ",a[href*='javascript:']" +
            ",button"
        ).prop("disabled", true);

        $("#panel-ticket-detail").find(".ticket-allow-editor-everyone").prop("disabled", false);
        bindIconLockMode();

        //$("#icon-lock-ticket").remove();
        //if ($("#nav-ticket-button-control #icon-lock-ticket").length == 0) {
        //    var modeLock = $("#hddPageTicketMode").val().toLowerCase() == "display" ? "lock" : "unlock";

        //    var btnLog = $("<div>", {
        //        id: "icon-lock-ticket",
        //        class: "pull-right " + modeLock
        //    });

        //    var descLog = $("<label>", {
        //        class: "description-lock-page",
        //        //html: "<b>Mode</b> : " + $("#hddEmployeeFirstView").val()
        //        html: "<b>Mode</b> : " + $("#hddPageTicketMode").val()
        //    });
        //    var iconLog = $("<i>", {
        //        class: "fa fa-" + modeLock + " " + modeLock
        //    });
        //    btnLog.append(descLog);
        //    btnLog.append(iconLog);
        //    $("#nav-ticket-button-control").append(btnLog);
        //}
    }


    $("#panel-ticket-detail").find(".ticket-default-disabled").prop("disabled", true);
}

function controlInputEnable() {
    var IsFirstView = $("#hddAuthenEdit").val().toLowerCase() == "true";
    if (!IsFirstView) {
        return;
    }

    $("#panel-ticket-detail").find(
        "input[type='text']:disabled" +
        ",input[type='number']:disabled" +
        ",input[type='checkbox']:disabled" +
        ",input[type='radio']:disabled" +
        ",select:disabled" +
        ",textarea:disabled"
    ).addClass("ticket-default-disabled");

    $("#panel-ticket-detail").find(
        "input[type='text']" +
        ",input[type='number']" +
        ",input[type='checkbox']" +
        ",input[type='radio']" +
        ",select" +
        ",textarea"
    ).prop("disabled", false);
    $("#panel-ticket-detail").find(".ticket-default-disabled").prop("disabled", true);
    //$("#icon-lock-ticket").remove();
    bindIconLockMode();
}

var panelTimeCountDown, momentOfTime, countDownDate, countdownfunction, isExpired;
isExpired = false;

function bindIconLockMode() {
    $("#icon-lock-ticket").remove();
    if ($("#nav-ticket-button-control #icon-lock-ticket").length == 0) {
        var modeLock = $("#hddPageTicketMode").val().toLowerCase() == "display" ? "lock" : "unlock";

        var btnLog = $("<div>", {
            id: "icon-lock-ticket",
            class: "pull-right " + modeLock
        });

        var descLog = $("<label>", {
            class: "description-lock-page",
            //html: "<b>Mode</b> : " + $("#hddEmployeeFirstView").val()
            html: "<b>Mode</b> : " + $("#hddPageTicketMode").val()
        });
        var iconLog = $("<i>", {
            class: "fa fa-" + modeLock + " " + modeLock
        });
        btnLog.append(descLog);
        btnLog.append(iconLog);

        if (modeLock == "unlock") {
            var lifeTime = $("<div>", {
                id: "panel-TimeCountDown",
                class: "pull-right"
            }).css({
                "margin": "3px 0px 0px 0px",
                "width": "60px",
                "text-align": "right"
            });
            btnLog.append(lifeTime);
            //panelTimeCountDown = document.getElementById("panel-TimeCountDown");
            momentOfTime = new Date();
            myTimeSpan = 15 * 60 * 1000;
            momentOfTime.setTime(momentOfTime.getTime() + myTimeSpan);
            countDownDate = momentOfTime.getTime();

            if (isExpired) {
                clearInterval(countdownfunction);
            }

            countdownfunction = setInterval(function () {
                if (lifeTime.html() == "EXPIRED") {
                    clearInterval(countdownfunction);
                    return;
                }
                var now = new Date().getTime();
                var distance = countDownDate - now;

                //var days = Math.floor(distance / (1000 * 60 * 60 * 24));
                //var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                var seconds = Math.floor((distance % (1000 * 60)) / 1000);

                lifeTime.html(minutes + "m " + seconds + "s");

                //console.log(distance);

                if (distance < 0) {
                    clearInterval(countdownfunction);
                    isExpired = true;
                    lifeTime.html("EXPIRED");
                    $("#btnRequestDisplayMode").click();
                }
            }, 1000);
        }
        $("#nav-ticket-button-control").append(btnLog);
    }
}

function countMaxLengthRemark(e) {
    var target = $(e.target).closest('.control-max-length');
    var maxLength = parseInt($(e.target).attr("data-maxlength"));
    var textLength = e.currentTarget.value.length;

    target.find(".Count-MaxLength-Remark").html(textLength);
    if (textLength > maxLength) {
        return false;
    } else {
        return true;
    }
}

function validateMaxLengthRemark(e) {
    var target = $(e.target).closest('.control-max-length');
    var maxLength = parseInt($(e.target).attr("data-maxlength"));
    var textLength = e.currentTarget.value.length;

    target.find(".Count-MaxLength-Remark").html(textLength);

    if (textLength > maxLength) {
        target.find(".Count-MaxLength-Remark").html(maxLength);
        var text = e.currentTarget.value;
        e.currentTarget.value = text.substring(0, maxLength);
    }
}

function addControlTicketStatus(a, b) {
    onCallPostQuoteMessage = undefined;
    $("#hddCheckSaveChangeTicketStatus").val('false');

    controlTextRemark(a);
    var target_Remark = a.find('.system-message-comment-container-reply');

    var Elt_ddl = $("#ddlTicketStatus_Temp").clone();
    Elt_ddl.attr("id", "ddl_ticket_doc_status_remark");
    Elt_ddl.attr("name", "ddl_ticket_doc_status_remark");
    Elt_ddl.addClass("form-control form-control-sm btn btn-default btn-sm ddl_ticket_doc_status_remark");
    Elt_ddl.css({
        "width": "250px",
        "border-color": "rgb(204, 204, 204)"
    });
    a.find(".reply-attach").prepend(Elt_ddl);
    var spanDesc = $("<span>", {
        html: "Ticket Status",
        class: "btn btn-default btn-sm"
    }).css({
        "border-color": "rgb(204, 204, 204)",
        "background-color": "#eee",
        "color": "#444",
        "cursor": "default"
    });
    a.find(".reply-attach").prepend(spanDesc);

    var old_value = $("#hddTicketStatus_Old").val();
    var old_value_desc = Elt_ddl.find("option[value='" + old_value + "']").text();
    Elt_ddl.val(old_value);

    target_Remark.find(".btn-submit-to-post").hide();
    if (target_Remark.find(".btn-confirm-to-post").length > 0) {
        target_Remark.find(".btn-confirm-to-post").remove();
    }
    var btn_Confirm = $("<span>", {
        class: "btn btn-primary btn-sm pull-right activity-chatting-fullmode btn-confirm-to-post AUTH_MODIFY",
        html: "Post Comment",
        style: "box-shadow: none;",
        "data-aobjectlink": b.aobjectlink
    });

    btn_Confirm.on("click", function () {

        var msg_confirm = "";

        if (a.find(".system-message-comment-container-reply textarea").val().trim().length == 0) {
            AGMessage("Please fill out remark.");
            return;
        }

        if ($("#hddTicketStatus_Old").val() != $("#hddTicketStatus_New").val()) {
            msg_confirm = 'Confirm update ticket status to "' + Elt_ddl.find("option:selected").text().replace(Elt_ddl.val() + " : ", "") + '" ?';
            onCallPostQuoteMessage = function () {
                var oldstat = Elt_ddl.find("option[value='" + $("#hddTicketStatus_Old").val() + "']").text().replace($("#hddTicketStatus_Old").val() + " : ", "");
                var newstat = Elt_ddl.find("option:selected").text().replace(Elt_ddl.val() + " : ", "");

                var QuoteDatas = {
                    quoteType: "UPDATESTATUS",
                    quoteMessage: 'Update status from "' + oldstat + '" to "' + newstat + '"'
                };
                return QuoteDatas;
            };
        } else {
            msg_confirm = 'Do you want to post remark without update ticket status ?';
            onCallPostQuoteMessage = undefined;
        }

        if (AGConfirm(this, msg_confirm)) {
            AGLoading(true, 'Checking Current Tier and Save Datas...');
            var btnObj = this;
            $.ajax({
                url: servictWebDomainName + "Framework/ag-activity-remark/API/Default.aspx",
                data: {
                    q: "validate-current-tier",
                    tkno: $("#hddDocnumberTran").val(),
                    aobj: $(this).attr("data-aobjectlink")
                },
                success: function (datas) {
                    //AGLoading(false, 'Checking Current Tier...');

                    //AGLoading(true);
                    if (datas) {
                        $(btnObj).parent().find(".btn-submit-to-post").click();
                    } else {
                        AGMessage("There are changes in the tier, Please do the transaction again.");
                        $("#btnSelectIncidentArea").click();
                    }
                },
                error: function (xhr, error) {
                    console.log(xhr);
                    console.log(error);
                }
            });
            return;
        }

        return;
    });

    target_Remark.find(".btn-submit-to-post").parent().prepend(btn_Confirm);

    Elt_ddl.bind("change", function (e) {
        //var target_Remark = $(this).closest('.system-message-comment-container-reply');

        old_value = $("#hddTicketStatus_Old").val();
        old_value_desc = Elt_ddl.find("option[value='" + old_value + "']").text();

        var new_value = $(this).val();
        var new_value_desc = $(this).find("option:selected").text();

        onCallPostQuoteMessage = undefined;
        $("#hddCheckSaveChangeTicketStatus").val('false');
        $("#hddTicketStatus_New").val(new_value);

        if (old_value != new_value) {
            $("#hddCheckSaveChangeTicketStatus").val('true');

        }
    });

    focusRemarkBox(false);


    var IsFirstView = $("#hddAuthenEdit").val().toLowerCase() == "true";
    if (!IsFirstView) {
        Elt_ddl.prop("disabled", true);
    }

}

function postSuccessCallBack(a, b, c) {
    var old_value = $("#hddTicketStatus_Old").val();
    var new_value = $("#hddTicketStatus_New").val();

    if (old_value != new_value) {
        $("[name='ddl_ticket_doc_status_remark']").val(new_value);
        $("#btnUpdateStatus_FormPostRemark").click();
    } else {
        $("#btnUpdateLog_FormPostRemark").click();
    }
}

function controlTextRemark(a) {
    a.find(".system-message-comment-container-reply textarea").addClass("ticket-allow-editor");
    a.find(".system-message-comment-container-reply textarea").addClass("ticket-allow-editor-everyone");
}

var focusRemarkBoxTimeOut;
function focusRemarkBox(isScrollToBox) {
    var tabRemark, paneltabRemark;
    if (isPageChange) {
        paneltabRemark = "#nav-participants";
        tabRemark = "#nav-participants-tab";
    } else {
        paneltabRemark = "#nav-sla";
        tabRemark = "#nav-sla-tab";
    }
    if (!$(paneltabRemark).is(':visible')) {
        $(tabRemark).click();

        focusRemarkBoxTimeOut = setTimeout(function () {
            focusRemarkBox(isScrollToBox, !$(".ag-remarker .system-message-comment-container-reply").is(':visible'));
        }, 100);
        return;
    } else {
        if (!$(".feed-activity-command").is(':visible')) {
            console.log("No Panel Remark");
            return;
        }
    }

    if (!$(".ag-remarker .system-message-comment-container-reply").is(':visible')) {
        if ($("#panelFeedActivityComment div:first").is(':visible')) {
            $("#panelFeedActivityComment div:first").click()
        }

        if ($(".system-message-comment-container-reply textarea").length == 1 &&
            !$(".system-message-comment-container-reply textarea").is(':visible')) {
            console.log("No Text Box Remark");
            return;
        }

        focusRemarkBoxTimeOut = setTimeout(function () {
            focusRemarkBox(isScrollToBox, false);
        }, 100);
        return;
    }

    if (isScrollToBox) {


        $('html,body').animate({
            scrollTop: $(".ag-remarker .system-message-comment-container-reply").offset().top - 250
        });

        setTimeout(function () {
            $(".system-message-comment-container-reply textarea").focus();
        }, 500);

    }

    return;
}

function controlDisplayEditTicketRelation(IsEdit) {
    if (IsEdit) {
        //$("#chkEnableEditMode").prop('checked', false);
        //enableEditMode($("#chkEnableEditMode")[0]);
        $("#panel-bord").show();
        $("#hierarchyReferFrom").hide();
    } else {
        //$("#chkEnableEditMode").prop('checked', false);
        //enableEditMode($("#chkEnableEditMode")[0]);
        if ($("#chkEnableEditMode").prop('checked')) {
            $("#chkEnableEditMode").parent().click();
        }
        $("#panel-bord").hide();
        $("#hierarchyReferFrom").show();
    }
}

function showMoreParticipant(obj) {
    $(obj).hide();
    $(obj).next().show();
    $(obj).closest('.mat-box').find('.overview-init-owner.d-none-default').removeClass('d-none');
}

function hideMoreParticipant(obj) {
    $(obj).hide();
    $(obj).prev().show();
    $(obj).closest('.mat-box').find('.overview-init-owner.d-none-default').addClass('d-none');
}

function dataTableLogAttachmentFile() {
    $("#TableLogAttachmentFile").dataTable({
        columnDefs: [
            {
                "orderable": false,
                "targets": [0]
            },
            {
                "orderable": false,
                "targets": [4]
            },
            {
                "orderable": true,
                'targets': 3,
                'createdCell': function (td, cellData, rowData, row, col) {
                    var dataDB = cellData.substring(0, 8);
                    var timeDB = cellData.substring(8, 14);
                    var dataDisplay = dataDB.substring(6, 8) + "/" + dataDB.substring(4, 6) + "/" + dataDB.substring(0, 4);
                    var timeDisplay = timeDB.substring(0, 2) + ":" + timeDB.substring(2, 4) + ":" + timeDB.substring(4, 6);

                    $(td).html(dataDisplay + " " + timeDisplay);
                }
            }
        ],
        "order": [[0, "desc"], [3, "desc"]]
    });
}

// knowledge management
function bindDataKnowledgeManagementRefTicketNumber(ListData) {
    //var ListData = JSON.parse($("#divJsonListDataKnowledgeManagement").html());
    var data = [];
    for (var i = 0; i < ListData.length; i++) {
        var jArr = ListData[i];
        data.push([
            jArr.ObjectID,
            jArr.KMGroupName,
            jArr.ObjectID,
            jArr.PrimaryKeyWord,
            jArr.Description,
            jArr.Details
        ]);
    }
    $("#table-dataview-KnowledgeManagement").dataTable({
        data: data,
        deferRender: true,
        columnDefs: [{
            "orderable": false,
            "targets": [0],
            "createdCell": function (td, cellData, rowData, row, col) {
                $(td).addClass("text-center");
                $(td).html(
                    "<a class='c-pointer' title=\"View Knowledge\">" +
                    "<i class=\"fa fa-search text-dark\" aria-hidden=\"true\"></i>" +
                    "</a>"
                );
                $(td).bind({
                    click: function () {
                        $("#hhdKnowledgeIDRefTicketNO").val(rowData[0]);
                        $("#btnRedirectKnowledgeIDRefTicketNO").click();
                        // Do something on click
                    }
                });
                $(td).closest("tr").addClass("c-pointer");
                $(td).closest("tr").bind({
                    click: function () {
                        $("#hhdKnowledgeIDRefTicketNO").val(rowData[0]);
                        $("#btnRedirectKnowledgeIDRefTicketNO").click();
                        // Do something on click
                    }
                });
            }
        }]
    });
}

function bindDataKnowledgeManagementRefTicketNumberForAdd() {
    $("#table-dataview-AddKnowledgeManagement").dataTable({
        columnDefs: [
            {
                "orderable": false,
                "targets": [0],
                "createdCell": function (td, cellData, rowData, row, col) {
                    var checkbox = $(td).find("input[type=checkbox]")
                    checkbox.bind({
                        click: function () {
                            var key = $(checkbox).closest("td").attr("data-key");
                            var JArr = JSON.parse($("#hhdDataSourceSelectAddKnowledgeManagement").val());
                            if ($(checkbox).is(':checked')) {
                                JArr.push(key);
                            }
                            else {
                                JArr = $.grep(JArr, function (v) {
                                    return v != key;
                                });
                            }
                            $("#hhdDataSourceSelectAddKnowledgeManagement").val(JSON.stringify(JArr));
                        }
                    });
                }
            }
        ],
        "order": [[1, "desc"], [2, "asc"], [3, "desc"]]
    });
    //$('html,body').animate({
    //    scrollTop: $("#divSearch").offset().top - 50
    //});
}


function redirectKnowledgeManagementNewPage(id) {
    window.open("/KM/KnowledgeManagementDetail.aspx?id=" + id, '_blank');
}

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};

function toRelyComment() {
    var toReply = true; 
    var ref = getUrlParameter('ref');
    console.log("ref : " + ref);
    
    var tabRemark, paneltabRemark;
    if (isPageChange) {
        paneltabRemark = "#nav-participants";
        tabRemark = "#nav-participants-tab";
    } else {
        paneltabRemark = "#nav-sla";
        tabRemark = "#nav-sla-tab";
    }
    if (!$(paneltabRemark).is(':visible')) {
        $(tabRemark).click();
        console.log("1");
        setTimeout(function () {
            toRelyComment( toReply, !$(".ag-remarker .system-message-comment-container-reply").is(':visible'));
            //$("[data-seq-reply=" + ref + "]").click();
        }, 100);
        return;
    }

    if (!$(".ag-remarker .system-message-comment-container-reply").is(':visible')) {
        if ($("#panelFeedActivityComment div:first").is(':visible')) {
            $("#panelFeedActivityComment div:first").click()
        }
        console.log("2");
        setTimeout(function () {
            toRelyComment(toReply, false);
            //$("[data-seq-reply=" + ref + "]").click();
        }, 100);
        return;
    }

   // $("[data-seq-reply=" + ref + "]").click();
    if (toReply) {
        console.log("3");
        setTimeout(function () {
            $(".tk-reply-remark[data-seq-reply=" + ref + "]").click();
            //toReply = false;
        }, 200);
       
        return;
    }

    
    return;
}

// ---------------------------------------------------- ---------------------------------------------------- //

$('a.header-tab[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    var target = $(e.target).attr("href") // activated tab
    var id = $(e.target).attr("id") // activated tab
    $("#hddIDCurentTabView").val(id);

    if (id == 'nav-sla-tab' || id == 'nav-participants-tab') {
        focusRemarkBox(true);
    } else {
        if (focusRemarkBoxTimeOut) {
            clearTimeout(focusRemarkBoxTimeOut);
        }
    }

    //console.log(id);
});

function loadCurentTabView() {
    var id = $("#hddIDCurentTabView").val();
    if (!id) {
        return;
    }

    $("#" + id).click();

    if (id == 'nav-sla-tab' || id == 'nav-participants-tab') {
        focusRemarkBox(true);
    }
}
