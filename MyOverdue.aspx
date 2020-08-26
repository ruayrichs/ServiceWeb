<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="MyOverdue.aspx.cs" Inherits="ServiceWeb.MyOverdue" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <script>            

            function webOnLoad() {
                //clear old active
                var onav = document.getElementsByClassName("nav-link active")[0].id;
                document.getElementById(onav).className = "nav-link";
                //set new active
                document.getElementById("nav-menu-myoverdue").className = "nav-link active";
            };

            $(document).ready(function () {
                webOnLoad();
            })
        </script>
        <style>
            .priority {
                padding: 10px;
                width: 30px;
            }

                .priority.very-hight {
                    background-color: #f44336;
                }

                .priority.hight {
                    background-color: #FF9800;
                }

                .priority.normal {
                    background-color: #00BCD4;
                }

                .priority.low {
                    background-color: #607D8B;
                }

                .priority.very-low {
                    background-color: #9E9E9E;
                }

            .col-date {
                width: 82px;
            }

            .col-ticket-type {
                width: 120px;
            }

            .col-subject {
                max-width: 3.4rem;
            }

            .col-customer {
                width: 130px;
                max-width: 130px;
            }

            .col-status {
                width: 100px;
            }

            .status {
                min-width: 90px;
                max-width: 124px;
                padding: 2px 4px;
                font-weight: 500;
                text-align: center;
            }

            button.status {
                font-size: 13px;
            }

            .status.unassign {
                background: #FF9800;
                color: white;
            }

            .status.open {
                background: #607D8B;
                color: white;
            }

            .status.inprogress {
                background: #17a2b8;
                color: white;
            }

            .status.inprogress.Resolve {
                background: #28a745;
                color: white;
            }

            .status.finish {
                background: #4CAF50;
                color: white;
            }

            .status.Closed.Closed {
                background: #28a745;
                color: white;
            }

            .status.Cancel.Cancel {
                background: #dc3545;
                color: white;
            }

            #tableItemsMyTask {
                width: 100% !important;
            }
        </style>
        <!-- My OverDue -->
        <div class="card border-danger mb-3">
            <div class="card-header bg-danger text-white">
                <b>My Overdue</b>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <div class="dataTables_wrapper">
                        <div class="row">
                            <div class="col-sm-12 col-md-6">
                                <div class="dataTables_length"></div>                                
                            </div>
                            <div class="col-sm-12 col-md-6 d-none">
                                <div class="dataTables_filter">
                                    <lable> Status:&nbsp;</lable>
                                    <select id="SelectStatus" class="form-control-sm" onchange="bindDataMyTicket();" style="width: 150px; margin-bottom: .5rem;">
                                        <option selected="selected" value="active">Active</option>                                    
                                        <option value="inactive">Inactive</option>                                     
                                        <option value="">All</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>  
                    <table id="tableItemsMyTask" class="table table-sm table-hover table-striped" style="margin-bottom: 0;">
                        <thead>
                            <tr>
                                <th class="col-date text-nowrap">Date</th>
                                <th class="col-Time text-center text-nowrap">Time</th>
                                <th class="col-Time text-center text-nowrap">Life Time</th>
                                <th class="col-ticket-type text-nowrap">Ticket Type</th>
                                <th class="col-ticket-type text-nowrap">Ticket No.</th>
                                <th class="col-subject">Subject</th>
                                <th class="col-Work-Flow-Status text-nowrap">Work Flow Status</th>
                                <th class="col-customer text-nowrap">Client</th>
                                <th class="col-status text-center text-nowrap">Status</th>
                            </tr>
                        </thead>
                        <tbody>

                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <script>            
            function bindingDataTableMyTask() {
                $("#tableItemsMyTask").dataTable({
                    columnDefs: [{
                        "orderable": false,
                        "targets": [0]
                    }],
                    "order": [[1, "asc"]]
                });
            }
            function changeMyTask(doctype, docnumber, fiscalyear, customerCode) {
                $("#hdfChange").val(doctype + "|" + docnumber + "|" + fiscalyear + "|" + customerCode);
                $("#btnChange").click();
            }
        </script>

        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hdfChange" runat="server" ClientIDMode="Static" />
                <asp:Button ID="btnChange" runat="server" ClientIDMode="Static" CssClass="d-none" OnClick="btnChange_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>


        <script>            
            var objDatasMyTicket;
            var myTaskDataTable, overdueDataTable, unassignCIDataTable;
            $(document).ready(function () {
                bindDataMyTicket();

                
            });

            function bindDataMyTicket() {
                $("#tableItemsMyTask").AGWhiteLoading(true, 'Loading...');
                var postData = {
                    dataStatus: $("#SelectStatus")[0].value
                };
                $.ajax({
                    type: "POST",
                    url: servictWebDomainName + "API/v1/GetTicketMyOverDueAPI.aspx",
                    data: postData,
                    success: function (datas) {
                        objDatasMyTicket = JSON.parse(datas);

                        if (myTaskDataTable) {
                            myTaskDataTable.fnDestroy();
                        }
                        myTaskDataTable = bindListMyTicket($("#tableItemsMyTask"), objDatasMyTicket);
                        $("#tableItemsMyTask").AGWhiteLoading(false, 'Loading...');
                    }
                });
            }

            //create row
            function bindListMyTicket(objTarget, datas) {

                var datasTicket = [];
                for (var i = 0; i < datas.length; i++) {
                    var ticket = datas[i];

                    var flag = '';
                    if (ticket.CustomerCritical == 'CRITICAL') {
                        flag = '<img src="/images/icon/flag-red-512.png" width="20" height="20">&nbsp;';
                    }

                    datasTicket.push([
                        ticket.StartDateTime,
                        ticket.StartDateTime.substring(8, 14),
                        liftTime(ticket.StartDateTime, ticket.StatusCode),
                        ticket.DocumentTypeDesc,
                        ticket.CallerID + "|" + convertToTicketNoDisplay(ticket.CallerID) + "|" + ticket.Doctype + "|" + ticket.Fiscalyear + "|" + ticket.CustomerCode,
                        ticket.HeaderText,
                        ticket.WorkFlowStatus,
                        flag + ticket.CustomerName,
                        ticket.StatusCode + "|" + ticket.StatusDesc
                    ]);
                }

                var dataTableResult = objTarget.dataTable({
                    data: datasTicket,
                    deferRender: true,
                    "order": [[0, "desc"]],
                    'columnDefs': [
                        {
                            'targets': 0,
                            'createdCell': function (td, cellData, rowData, row, col) {
                                $(td).addClass("col-date text-nowrap");
                                $(td).html(
                                    convertToDateDisplay(cellData)
                                );
                                $(td).closest("tr").addClass("c-pointer");
                                $(td).closest("tr").bind("click", function () {
                                    var datas = rowData[4].split('|');
                                    var doctype = datas[2];
                                    var docnumber = datas[0];
                                    var fiscalyear = datas[3];
                                    var customerCode = datas[4];
                                    changeMyTask(doctype, docnumber, fiscalyear, customerCode);
                                });
                            }
                        },
                        {
                            'targets': 1,
                            'createdCell': function (td, cellData, rowData, row, col) {
                                $(td).addClass("col-date text-nowrap");
                                var timeDB = cellData;
                                var dataDisplay = timeDB.substring(0, 2) + ":" + timeDB.substring(2, 4) + ":" + timeDB.substring(4, 8);
                                $(td).html(
                                    dataDisplay
                                );
                            }
                        },
                        {
                            'targets': 2,
                            'createdCell': function (td, cellData, rowData, row, col) {
                                var datas = cellData.split(" Day ");
                                if (cellData != "") {
                                    cellData = parseInt(datas[0]).toString() + " Day " + datas[1];
                                }
                                $(td).addClass("col-date text-nowrap");
                                $(td).html(
                                    cellData
                                );
                            }
                        },
                        {
                            'targets': 3,
                            'createdCell': function (td, cellData, rowData, row, col) {
                                $(td).addClass("text-danger col-ticket-type text-nowrap");
                                $(td).html(
                                    '#' + cellData
                                );
                            }
                        },
                        {
                            'targets': 4,
                            'createdCell': function (td, cellData, rowData, row, col) {
                                $(td).addClass("text-danger col-ticket-type text-nowrap");
                                $(td).html(
                                    cellData.split('|')[1]
                                );
                            }
                        },
                        {
                            'targets': 5,
                            'createdCell': function (td, cellData, rowData, row, col) {
                                $(td).attr('title', cellData);
                                $(td).addClass("col-subject text-truncate font-italic");
                            }
                        },
                        {
                            'targets': 6,
                            'createdCell': function (td, cellData, rowData, row, col) {
                                $(td).addClass("text-truncate col-Work-Flow-Status text-nowrap");
                            }
                        },
                        {
                            'targets': 7,
                            'createdCell': function (td, cellData, rowData, row, col) {
                                $(td).addClass("text-truncate col-customer text-nowrap");
                                if (cellData == 'null') { $(td).html(''); }
                            }
                        },
                        {
                            'targets': 8,
                            'createdCell': function (td, cellData, rowData, row, col) {
                                $(td).addClass("col-status text-nowrap");
                                $(td).html(
                                    '<div class="status ' + cellData.split('|')[0] + ' ' + cellData.split('|')[1] + '">' +
                                    cellData.split('|')[1] +
                                    '</div>'
                                );
                            }
                        }
                    ]
                });

                function convertToTicketNoDisplay(data) {
                    if (data != '') {
                        var CallerIDDisplay = data;
                        var rest = CallerIDDisplay.substring(0, CallerIDDisplay.lastIndexOf("-") + 1);
                        var last = CallerIDDisplay.substring(CallerIDDisplay.lastIndexOf("-") + 1, CallerIDDisplay.length);
                        last = parseInt(last).toString();
                        data = rest + last;
                    }
                    return data;
                }

                function convertToDateDisplay(data) {
                    if (data.length >= 8) {
                        data = data.substring(6, 8) + '/' + data.substring(4, 6) + '/' + data.substring(0, 4);
                    }
                    return data;
                }
                return dataTableResult;
            }
            function liftTime(strDate, strStatusCode) {

                if (strDate != "" && strStatusCode != "" && strStatusCode != "Cancel" && strStatusCode != "Closed" && strStatusCode != "Resolve") {
                    var today = new Date();
                    var dd = String(today.getDate()).padStart(2, '0');
                    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                    var yyyy = today.getFullYear();
                    var hours = today.getHours()
                    var minutes = today.getMinutes()
                    var seconds = today.getSeconds();

                    var _yyyy = parseInt(strDate.substring(0, 4));
                    var _mm = parseInt(strDate.substring(4, 6));
                    var _dd = parseInt(strDate.substring(6, 8));
                    var _hh = parseInt(strDate.substring(8, 10));
                    var _mi = parseInt(strDate.substring(10, 12));
                    var _ss = parseInt(strDate.substring(12, 14));

                    var t1 = new Date(yyyy, mm, dd, hours, minutes, seconds, 0);
                    var t2 = new Date(_yyyy, _mm, _dd, _hh, _mi, _ss, 0);
                    var dif = t1.getTime() - t2.getTime();

                    var Seconds_from_T1_to_T2 = dif / 1000;
                    var Seconds_Between_Dates = Math.abs(Seconds_from_T1_to_T2);
                    var hhCal = Seconds_Between_Dates / 60 / 60;
                    var ddDisplay = hhCal / 24;
                    var hhDisplay = parseFloat("0." + ddDisplay.toString().split('.')[1]) * 24;
                    var miDisplay = parseFloat("0." + hhDisplay.toString().split('.')[1]) * 60;
                    var ssDisplay = parseFloat("0." + miDisplay.toString().split('.')[1]) * 60;

                    strDate = parseInt(ddDisplay).toString().padStart(4, '0') + " Day " + parseInt(hhDisplay).toString().padStart(2, '0') + ":" + parseInt(miDisplay).toString().padStart(2, '0') + ":" + parseInt(ssDisplay).toString().padStart(2, '0');
                    return strDate;

                } else {
                    return strDate = '';
                }
            }
        </script>

    </div>
</asp:Content>
