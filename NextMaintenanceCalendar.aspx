<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="NextMaintenanceCalendar.aspx.cs" Inherits="ServiceWeb.NextMaintenanceCalendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-calendar").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    <div id="calendar" />
    <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional" class="d-none">
        <ContentTemplate>        
            <asp:HiddenField ID="hhdModeCalendar" runat="server" />
            <asp:Button ID="btnCalendar" ClientIDMode="Static" OnClick="btnCalendar_Click" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
         var rawobj = <%= Newtonsoft.Json.JsonConvert.SerializeObject(calendarEvent)%>;
        const rawJsonString = JSON.parse(JSON.stringify(rawobj));

        $("#calendar").fullCalendar({
            header: {
                left: "prev,next today",
                center: "title",
                right: "month",
            },
            height: 500,
            events: rawJsonString,
            //timeFormat: 'h:mm A',
            displayEventTime: false,
            eventClick: function (events) {
                if (events.id) {
                    var value = events.doctype + "|" + events.docnumber + "|" + events.fiscalyear;
                    $("#<%= hhdModeCalendar.ClientID%>").val(value);
                    $("#<%= btnCalendar.ClientID%>").click();
                }         
            },
               
            eventMouseover: function (events, jsEvent) {
                //$(this).popover({
                //    title: events.tooltipTitle,
                //    placement: 'auto',
                //    trigger: 'hover',
                //    delay: { show: 200, hide: 100 },
                //    animation: true,
                //    container: '#calendar',
                //    html: true,
                //    content: events.tooltipContent,
                //}).popover('toggle');
                if (events.haveUrl) {
                    $(this).mouseover(function () {
                        console.log(events.haveUrl);
                    
                            $(this).css("background-color", "#6bff9f")
                            $(this).css("cursor", "pointer") 
                        });
                    $(this).mouseout(function () {
                            $(this).css("background-color", events.color);
                     });
                }
            },
         

        });
    </script>
</asp:Content>
