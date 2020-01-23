<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetAgendaCtrl.ascx.cs" Inherits="ServiceWeb.widget.usercontrol.AssetAgendaCtrl" %>
<div>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCalendar">
        <ContentTemplate>
            <link href="<%= Page.ResolveUrl("~/Framework/FullCalendar/fullcalendar.css") %>" rel="stylesheet" />
            <link href="<%= Page.ResolveUrl("~/Framework/FullCalendar/fullcalendar.print.cs") %>s" rel="stylesheet" />
            <script src="<%= Page.ResolveUrl("~/Framework/FullCalendar/lib/moment.min.js?vs=20190113") %>"></script>
            <script src="<%= Page.ResolveUrl("~/Framework/FullCalendar/fullcalendar.min.js?vs=20190113") %>"></script>

            <script>
                $(document).ready(function () {
                    //loadAssetCalendar();
                });

                function restartAssetCalendar(assetcode, assetname) {
                    loadAssetCalendar(assetcode);
                    $('#asset-name').html(assetcode + ' - ' + assetname);
                    $('#_txtAssetCode').val(assetcode);
                }

                function restartAssetCalendar(assetcode, assetname) {
                    loadAssetCalendar(assetcode);
                    $('#asset-name').html(assetcode + ' - ' + assetname);
                    $('#_txtAssetCode').val(assetcode);
                }

                function loadAssetCalendar(assetcode) {
                    var _assetcode = '';
                    if (assetcode != undefined) {
                        _assetcode = assetcode;
                    }

                    var _param = '<%= "?sid="+ ERPW.Lib.Authentication.ERPWAuthentication.SID + "&snaid=" + "&company=" + ERPW.Lib.Authentication.ERPWAuthentication.CompanyCode + "&asset_code=" %>' + _assetcode;
        $.ajax({
            url: servictWebDomainName + "widget/API/AssetAgendaAPI.aspx" + _param,
            dataType: "JSON",
            success: function (datas) {
                StartAssetCalendar(datas);
                AGLoading(false);
            }
        });
    }

    function StartAssetCalendar(JSON) {
        var container = $("<div/>", {
        });
        $('#asset-agenda').html("").append(container);
        container.fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'
            },
            timeFormat: 'HH:mm',
            defaultDate: '<%= ToDay %>',
            eventLimit: true,
            events: JSON
        });
        $(".fc-prev-button,.fc-next-button").click(function () {
            assetAgendaTitle();
            bindBtnAssetCreateActivty();
        });
        assetAgendaTitle();
        bindBtnAssetCreateActivty();
    }

    function bindBtnAssetCreateActivty() {
        $("#asset-agenda .fc-day-number").each(function () {
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
                    var assetCode = $('#_txtAssetCode').val();
                    showPopupAssetActivity(this, sdate, assetCode);
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

    function showPopupAssetActivity(obj, startDate, assetcode) {
        var _offtop = $(obj).offset().top;
        var _offleft = $(obj).offset().left;
        $('#popup-activity-template').css('top', _offtop);
        $('#popup-activity-template').css('left', _offleft);
        var _newPopup = $('#popup-activity-template').clone();
        _newPopup.addClass('popup-activity');
        _newPopup.show();
        _newPopup.find(".dropdown-create-activity").attr("data-date", startDate);
        _newPopup.find(".dropdown-create-activity").attr("data-asset-code", assetcode);
        $('.popup-activity').remove();
        var _newPopup_html = _newPopup.prop('outerHTML');
        $('body').append(_newPopup_html);
    }

    function assetAgendaTitle() {
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

                #asset-agenda {
                    width: 100%;
                    margin: 0 auto;
                    font-size: 14px;
                    font-family: 'Lucida Sans Unicode','Lucida Grande',arial,helvetica,sans-serif;
                }

                    #asset-agenda .fc-more {
                        color: orange;
                        font-weight: 600;
                    }

                    #asset-agenda .fc-widget-header {
                        background-image: linear-gradient(to bottom,#fff 0,#f3f3f3 100%);
                        background-repeat: repeat-x;
                        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffffff', endColorstr='#fff3f3f3', GradientType=0);
                    }

                    #asset-agenda .fc-day-header {
                        padding: 10px;
                    }

                    #asset-agenda .fc-content {
                        font-size: 13px;
                    }

                        #asset-agenda .fc-content .fc-title {
                            padding: 5px;
                            padding-left: 3px;
                        }

                        #asset-agenda .fc-content .fc-time {
                            padding: 5px;
                            padding-right: 0px;
                        }

                    #asset-agenda h2 {
                        margin-top: 5px;
                        font-size: 18px;
                    }

                    #asset-agenda .fc-view {
                    }

                    #asset-agenda a {
                        color: #fff;
                    }

                .agenda-task {
                }

                .agenda-meeting {
                    background: #ff6a00;
                }

                .agenda-owner {
                    outline: 2px solid #6dd83b;
                }
            </style>

            <div class="row" style="margin-top: -10px">
                <div class="col-lg-12 text-center">
                    <label id="asset-name" style="font-size: large; color: blue"></label>
                    <input id="_txtAssetCode" type="text" style="display: none" />
                    <input id="_txtAssetName" type="text" style="display: none" />
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <div id='asset-agenda' style="cursor: default"></div>
                </div>
            </div>

            <div id='popup-activity-template' style="display: none;" onclick="event.stopPropagation();">
                <ul class="dropdown-create-activity" style="width: 100%">
                    <li>
                        <a onclick="createActivity('task',$(this).closest('.dropdown-create-activity').attr('data-date') ,$(this).closest('.dropdown-create-activity').attr('data-asset-code') );$('.popup-activity').remove();">
                            <i class="fa fa-edit" style="color: #5CB85C;"></i>&nbsp;Create task
                        </a>
                    </li>
                    <li>
                        <a onclick="createActivity('meeting',$(this).closest('.dropdown-create-activity').attr('data-date') ,$(this).closest('.dropdown-create-activity').attr('data-asset-code'));$('.popup-activity').remove();">
                            <i class="fa fa-users" style="color: #5CB85C;"></i>&nbsp;Create meeting
                        </a>
                    </li>
                    <li>
                        <a onclick="createActivity('note',$(this).closest('.dropdown-create-activity').attr('data-date') ,$(this).closest('.dropdown-create-activity').attr('data-asset-code'));$('.popup-activity').remove();">
                            <i class="fa fa-list" style="color: #5CB85C;"></i>&nbsp;Create note
                        </a>
                    </li>
                    <li>
                        <a onclick="createActivity('sale',$(this).closest('.dropdown-create-activity').attr('data-date') ,$(this).closest('.dropdown-create-activity').attr('data-asset-code'));$('.popup-activity').remove();">
                            <i class="fa fa-briefcase" style="color: #5CB85C;"></i>&nbsp;Create sales task
                        </a>
                    </li>

                </ul>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
