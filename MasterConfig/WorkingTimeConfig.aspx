<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="WorkingTimeConfig.aspx.cs" Inherits="ServiceWeb.MasterConfig.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <style>
        .weekdays-list .weekdays-day {
            color:red;
            width:100%;
            text-align:center;
        }
        .weekdays-list .weekday-selected {
            color:#007bff;
            background-color:#d8d8d8bf;
        }
        .weekdays-list {
            box-shadow: 0 0 4px 0px grey;
        }
        .datepicker table {
            width:100%;
        }
        .datepicker-inline {
             width:100%;
             box-shadow: 0 0 3px grey;
        }
        .datepicker table tr td.active, .datepicker table tr td.active:hover, .datepicker table tr td.active.disabled, .datepicker table tr td.active.disabled:hover{
           background-image: -webkit-linear-gradient(top, #dc3545, #dc3545);
        }
    </style>
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-working-time-config").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
     <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
            <div class="pull-left">
                <asp:UpdatePanel runat="server" ID="udpbtn" UpdateMode="Conditional">
                    <ContentTemplate>
                        <button type="button" class="btn btn-success mb-2 AUTH_MODIFY" onclick="getdata()"> <i class="fa fa-save" >&nbsp;&nbsp;Save</i></button>
                        <asp:Button runat="server" Text="" CssClass="btn btn-success d-none" ID="btn_create" OnClick="Btn_Work_Config_Click" OnClientClick="AGLoading(true);" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </nav>
        <div class="card shadow">
            <div class="card-header">
                <h5 class="mb-0">Working Time Configure</h5>
            </div>

            <div class="card-body panel-body-customer-search PANEL-DEFAULT-BUTTON">
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="card border-default" style="margin-bottom: 10px;">
                            <div class="card-body card-body-sm">
                                <div>
                                    <label class="font-weight-bold" >Configure Time</label>
                                </div>
                                <div class="form-row">
                                    <div class="form-group col-sm-12">
                                        <div>
                                            <label>Select Workday</label>
                                        </div>
                                        <asp:HiddenField ID="hdfDaypickValue" runat="server" />
                                 
                                        <div id="weekdays" class="col-sm-12"> </div>
                                    </div>
                                    <div class="form-group col-sm-6">
                                        <div>
                                            <label>Start Time Working</label>
                                        </div>
                                        <div class="input-group-append">
                                            <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-control form-control-sm time-picker ticket-allow-editor required"></asp:TextBox>
                                            <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                        </div>
                                    </div>
                                    <div class="form-group col-sm-6">
                                        <div>
                                            <label>End Time Working</label>
                                        </div>
                                        <div class="input-group-append">
                                            <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-control form-control-sm time-picker ticket-allow-editor required"></asp:TextBox>
                                            <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                        </div>
                                    </div>
                                    <div class="form-group col-sm-12">
                                        <div>
                                            <label>Add Holiday</label>
                                        </div>
                                         <asp:HiddenField ID="holidayCchap" runat="server" />
                                 
                                        <div id="holiday" class="col-sm-12" data-date-format="yyyy/mm/dd"> </div>
                                    </div>
                               </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <script>
                $(document).ready(function () {
                    $('#holiday').datepicker({
                        multidate: true,
                        clearBtn: true,
                        format: "yyyy/mm/dd"
                    });
                   
                    
                });
               
                function getdata() {
                    var rawObj = $('#weekdays').selectedDays();
                    var arrObj = Object.values($('#weekdays').selectedDays());
                    var lstDay = []; 
                    var leng = rawObj.length;
                    var i;
                    for (i = 0 ; i < leng ; i++) {
                        lstDay.push(arrObj[i]);
                    }
                    var holidays = $("#holiday").data('datepicker').getFormattedDate('yyyymmdd');

                    $("#<%= holidayCchap.ClientID %>").val(holidays);
                    $("#<%= hdfDaypickValue.ClientID %>").val(lstDay);
                    $("#<%= btn_create.ClientID %>").click();
                }
            </script>
</asp:Content>
