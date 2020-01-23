<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="UserAnalystDetail.aspx.cs" Inherits="ServiceWeb.Report.UserAnalystDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-user-analyst").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <a class="btn btn-warning btn-sm mb-1" href="UserAnalyst.aspx"><i class="fa fa-arrow-circle-left"></i>&nbsp;&nbsp;Back</a>
        </div>
    </nav>
    <div class="card shadow">
        <div class="card-header">
            <h5 class="mb-0">User Analyst Detail</h5>
        </div>
        <div class="card-body">
            <div class="form-row">
                <div class="col-md-12">
                    <div id="search-panel" style="display: none;">
                        <asp:UpdatePanel ID="upPanelProfileList" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <!-- Mode Table View -->
                                <div class="table-responsive">
                                    <table id="table-employee" class="table table-bordered table-striped table-hover table-sm nowrap">
                                        <thead>
                                            <tr>
                                                <th class="text-nowrap">Event</th>
                                                <th class="text-nowrap">Access By</th>
                                                <th class="text-nowrap">Access Data</th>
                                                <th class="text-nowrap">Access Time</th>
                                                <th class="text-nowrap">Programe Page</th>
                                                <th class="text-nowrap">Column Name</th>
                                                <th>Old Value</th>
                                                <th>New Value</th>
                                            </tr>
                                        </thead>

                                    </table>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function afterSearch(EmployeeList) {
            var data = [];
            for (var i = 0 ; i < EmployeeList.length ; i++) {
                var Employee = EmployeeList[i];
                data.push([
                    Employee.accesscode,
                    Employee.access_by,
                    Employee.access_date,
                    Employee.access_time,
                    Employee.programe,
                    Employee.sfieldname,
                    Employee.soldvalue,
                    Employee.snewvalue
                ]);
            }
            $("#search-panel").show();
            $("#table-employee").dataTable({
                data: data,
                //deferRender: true,
                "order": [[2, "asc"]],
                'columnDefs': [
                {
                    'targets': 2,
                    'createdCell': function (td, cellData, rowData, row, col) {
                        var dataDB = cellData.substring(0, 8);
                        var dataDisplay = dataDB.substring(6, 8) + "/" + dataDB.substring(4, 6) + "/" + dataDB.substring(0, 4);

                        $(td).html(dataDisplay);
                    }
                },
                {
                    'targets': 3,
                    'createdCell': function (td, cellData, rowData, row, col) {
                        var timeDB = cellData.substring(0,6);
                        var timeDisplay = timeDB.substring(0, 2) + ":" + timeDB.substring(2, 4) + ":" + timeDB.substring(4, 6);

                        $(td).html(timeDisplay);
                    }
                }
                ]
            });
        }

        function scrollToTable() {
            $('html,body').animate({
                scrollTop: $("#search-panel").offset().top - 50
            });
        }
    </script>
</asp:Content>
