<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActivityAgenda.ascx.cs" Inherits="ServiceWeb.UserControl.ActivityAgenda" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCalendar">
<contenttemplate>
<link href='<%= Page.ResolveUrl("~/framework/fullcalendar/fullcalendar.css") %>' rel='stylesheet' />
<link href='<%= Page.ResolveUrl("~/framework/fullcalendar/fullcalendar.print.css") %>' rel='stylesheet' media='print' />
<script src='<%= Page.ResolveUrl("~/framework/fullcalendar/lib/moment.min.js?vs=20190113") %>'></script>
<script src='<%= Page.ResolveUrl("~/framework/fullcalendar/fullcalendar.min.js?vs=20190113") %>'></script>
<script>

    $(document).ready(function () {
        loadActivityCalendar();
    });

    function restartCalendar(workGroupCode) {
        loadActivityCalendar(workGroupCode);
    }

    function loadActivityCalendar(workGroupCode) {
        var urlApi = "<%= Page.ResolveUrl("~/widget/ActivityAgendaAPI.aspx") %>";
        var customercode = '<%= Request["id"] %>';
        if (customercode.replace(' ') != '') {
            urlApi += '?customer=' + customercode.replace(' ', '');
        }
        if (workGroupCode != null && workGroupCode != undefined && workGroupCode != "") {
            urlApi += "?workgroupcode=" + workGroupCode;
        }
        $.ajax({
            url: urlApi,
            dataType:"JSON",
            success: function (datas) {
                StartCalendar(datas);
                try { bindListReportMode(datas, 'init'); } catch (e) { }

                if ($("#Activity").length > 0) {
                    $("#Activity").AGWhiteLoading(false, "กำลังโหลด...");
                }
                if ($('#divReloadCalendar').length > 0) {
                    $('#divReloadCalendar').AGWhiteLoading(false, "กำลังโหลด...");
                }
            }
        });
    }

    function StartCalendar(JSON) {
        var container = $("<div/>", {
        });
        $('#activity-agenda').html("").append(container);
        container.fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'
            },
            timeFormat: 'HH:mm',
            defaultDate: '<%= ToDay %>',
            eventLimit: true,
            events: JSON,
            eventClick: function (event) {
                if (event.url) {
                    window.open(event.url);
                    return false;
                }
            }
        });
        $(".fc-prev-button,.fc-next-button").click(function () {
            agendaTitle();
            bindBtnCreateActivty();
        });
        agendaTitle();
        bindBtnCreateActivty();
    }

    function bindBtnCreateActivty() {
        $(".fc-day-number").each(function () {
            //var _popup = $('.popup-activity').prop('outerHTML');
            var actContainer = $("<div/>", {
                css: {
                    float: "left",
                    cursor: "pointer"
                }
            });
            var act = $("<input/>", {
                type: "button",
                css: {
                    height: 3,
                    float: "left",
                    cursor: "pointer"
                },
                click: function (e) {
                    var sdate = $(this).closest('.fc-day-number').attr('data-date');
                    showPopupActivity(this, sdate);
                    e.stopPropagation();
                },
                value: ".."
            });

            actContainer.append(act);
            $(this).append(actContainer);
        });
        $(document).click(function () {
            $('.popup-activity').remove();
        });
    }

    function showPopupActivity(obj, startDate) {
        var _offtop = $(obj).offset().top;
        var _offleft = $(obj).offset().left;
        $('#popup-activity-template').css('top', _offtop);
        $('#popup-activity-template').css('left', _offleft);
        var _newPopup = $('#popup-activity-template').clone();
        _newPopup.addClass('popup-activity');
        _newPopup.show();
        _newPopup.find(".dropdown-create-activity").attr("data-date", startDate);
        $('.popup-activity').remove();
        var _newPopup_html = _newPopup.prop('outerHTML');
        $('body').append(_newPopup_html);
    }

    function agendaTitle() {
        $(".fc-title").each(function () {
            $(this).parent().attr("title", $(this).html());
        });

        var iMeeting = $("<i/>", {
            class: "fa fa-users",
            css: {
                marginRight: 5
            }
        });
        $(".agenda-meeting").find(".fc-time").prepend(iMeeting);

        var iTask = $("<i/>", {
            class: "fa fa-tasks",
            css: {
                marginRight: 5
            }
        });
        $(".agenda-task").find(".fc-time").prepend(iTask);
    }

</script>
<style>
    .popup-activity {
        position: absolute;
        z-index: 50000;
        border-radius: 5px;
        display: none;
        padding: 10px;
    }

    .dropdown-create-activity {
        top: 100%;
        left: 0;
        z-index: 1000;
        /* display: none; */
        float: left;
        min-width: 160px;
        padding: 5px 0;
        margin: 2px 0 0;
        list-style: none;
        font-size: 14px;
        background-color: #fff;
        border: 1px solid #ccc;
        border: 1px solid rgba(0,0,0,.15);
        border-radius: 4px;
        -webkit-box-shadow: 0 6px 12px rgba(0,0,0,.175);
        box-shadow: 0 6px 12px rgba(0,0,0,.175);
        background-clip: padding-box;
    }

    .dropdown-create-activity li a {
        padding: 10px 20px;
        font-weight: 600;
    }

    .dropdown-create-activity > li:hover {
        background-repeat: repeat-x;
        background-color: #e8e8e8;
        background-image: linear-gradient(to bottom,#f5f5f5 0,#9A9595 100%);
    }

    #activity-agenda {
        width: 100%;
        margin: 0 auto;
        font-size: 14px;
        font-family: 'Lucida Sans Unicode','Lucida Grande',arial,helvetica,sans-serif;
    }

    #activity-agenda .fc-more {
        color: orange;
        font-weight: 600;
    }

    #activity-agenda .fc-widget-header {
        background-image: linear-gradient(to bottom,#fff 0,#f3f3f3 100%);
        background-repeat: repeat-x;
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffffff', endColorstr='#fff3f3f3', GradientType=0);
    }

    #activity-agenda .fc-day-header {
        padding: 10px;
    }

    #activity-agenda .fc-content {
        font-size: 13px;
    }

    #activity-agenda .fc-content .fc-title {
        padding: 5px;
        padding-left: 3px;
    }

    #activity-agenda .fc-content .fc-time {
        padding: 5px;
        padding-right: 0px;
    }

    #activity-agenda h2 {
        margin-top: 5px;
        font-size: 18px;
    }

    #activity-agenda .fc-view {
    }

    #activity-agenda a {
        color: #fff;
    }

    .agenda-task {
    }

    .agenda-meeting {
        background: #ff6a00;
    }
</style>

<div id='activity-agenda' style="cursor: default"></div>
    
<div id='popup-activity-template' style="display: none;" onclick="event.stopPropagation();">
    <ul class="dropdown-create-activity" style="width: 100%">
        <%--<li>
            <a onclick="createActivity('task',$(this).closest('.dropdown-create-activity').attr('data-date'));$('.popup-activity').remove();">
                <i class="fa fa-edit" style="color: #5CB85C;"></i>&nbsp;Create task
            </a>
        </li>--%>
        <li>
            <a onclick="createActivity('meeting',$(this).closest('.dropdown-create-activity').attr('data-date'),'','<%= Request["id"] %>','<%= Request["doc"] %>','<%= Request["contactcode"] %>');$('.popup-activity').remove();">
                <i class="fa fa-users" style="color: #5CB85C;"></i>&nbsp;Create meeting
            </a>
        </li>
        <%--<li>
            <a onclick="createActivity('note',$(this).closest('.dropdown-create-activity').attr('data-date'));$('.popup-activity').remove();">
                <i class="fa fa-list" style="color: #5CB85C;"></i>&nbsp;Create note
            </a>
        </li>--%>
        <li>
            <a onclick="createActivity('sale',$(this).closest('.dropdown-create-activity').attr('data-date'),'','<%= Request["id"] %>','<%= Request["doc"] %>','<%= Request["contactcode"] %>');$('.popup-activity').remove();">
                <i class="fa fa-briefcase" style="color: #5CB85C;"></i>&nbsp;Create sales task
            </a>
        </li>
        <%--<li>
            <a onclick="relocationToTimeSheet(this,$(this).closest('.dropdown-create-activity').attr('data-date'));$('.popup-activity').remove();">
                <i class="fa fa-clock-o" style="color: #5CB85C;"></i>&nbsp;Time attendance
            </a>
        </li>--%>

    </ul>
    <%--<script>
        function relocationToTimeSheet(obj,date) {
            $(obj).prop("href", "/web/HR/HRTimeSheet.aspx?date=" + date);
            AGLoading(true);
        }
    </script>--%>
</div>
</contenttemplate>
</asp:UpdatePanel>