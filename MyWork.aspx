<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="MyWork.aspx.cs" Inherits="ServiceWeb.MyWork" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <script>

            function webOnLoad() {
                //clear old active
                var onav = document.getElementsByClassName("nav-link active")[0].id;
                document.getElementById(onav).className = "nav-link";
                //set new active
                document.getElementById("nav-menu-mywork").className = "nav-link active";
            };

            $(document).ready(function () { webOnLoad(); })
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
                background: #FFC107;
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

            #tableItemsMyTicket {
                width: 100% !important;
            }
        </style>
        <!-- My Work -->
        <div class="card border-info mb-3">
            <div class="card-header bg-info text-white">
                <b>My Queue</b>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    
                    <div class="dataTables_wrapper">
                        <div class="row">
                            <div class="col-sm-12 col-md-6">
                                <div class="dataTables_length"></div>                                
                            </div>
                            <div class="col-sm-12 col-md-6">
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
                    
                    <table id="tableItemsMyTicket" class="table table-sm table-hover table-striped" style="margin-bottom: 0;">
                        
                        <thead>
                            <tr>
                                <th class="col-date text-nowrap">Date</th>
                                <th class="col-ticket-type text-nowrap">Ticket Type</th>
                                <th class="col-ticket-type text-nowrap">Ticket No.</th>
                                <th class="col-subject">Subject</th>
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
                $("#tableItemsMyTicket").dataTable({
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
                $("#tableItemsMyTicket").AGWhiteLoading(true, 'Loading...');
                var postData = {
                    dataStatus: $("#SelectStatus")[0].value
                };
                $.ajax({
                    type: "POST",
                    url: servictWebDomainName + "API/v1/GetTicketMyWorkAPI.aspx",
                    data: postData,
                    success: function (datas) {
                        console.log(datas);///////////////////////////////////////////
                        objDatasMyTicket = JSON.parse(datas);

                        if (myTaskDataTable) {
                            myTaskDataTable.fnDestroy();
                        }
                        myTaskDataTable = bindListMyTicket($("#tableItemsMyTicket"), objDatasMyTicket);
                        $("#tableItemsMyTicket").AGWhiteLoading(false, 'Loading...');
                    }
                });
            }

            function bindListMyTicket(objTarget, datas) {

                var datasTicket = [];
                for (var i = 0 ; i < datas.length ; i++) {
                    var ticket = datas[i];

                    var flag = '';
                    if (ticket.CustomerCritical == 'CRITICAL') {
                        flag = '<img src="/images/icon/flag-red-512.png" width="20" height="20">&nbsp;';
                    }

                    datasTicket.push([
                        ticket.StartDateTime + "|" + ticket.EndDateTime,
                        ticket.DocumentTypeDesc,
                        ticket.CallerID + "|" + ticket.TicketNoDisplay + "|" + ticket.Doctype + "|" + ticket.Fiscalyear + "|" + ticket.CustomerCode,
                        ticket.HeaderText,
                        flag + ticket.CustomerName,
                        ticket.StatusCode + "|" + ticket.StatusDesc
                    ]);
                }
                var startDATEend;
                var startdate;
                var enddate;
                var today = new Date();
                var dd = String(today.getDate()).padStart(2, '0');
                var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
                var yyyy = today.getFullYear();
                var hours = today.getHours()
                var minutes = today.getMinutes()
                var seconds = today.getSeconds();
                today = yyyy + mm + dd + hours + minutes + seconds;
                
                var dataTableResult = objTarget.dataTable({
                    data: datasTicket,
                    deferRender: true,
                    "order": [[0, "desc"]],
                    'columnDefs': [
                       {
                           'targets': 0,
                           'createdCell': function (td, cellData, rowData, row, col) {
                               $(td).addClass("col-date text-nowrap");
                               startDATEend = cellData.split('|');
                               startdate = startDATEend[0];
                               enddate = startDATEend[1];
                               $(td).html(
                                   //convertToDateDisplay(cellData)
                                   convertToDateDisplay(startdate)
                                );
                               $(td).closest("tr").addClass("c-pointer");
                               $(td).closest("tr").bind("click", function () {
                                   var datas = rowData[2].split('|');
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
                               if (enddate != "null") {
                                   if (enddate > today) {
                                       $(td).addClass("text-primary col-ticket-type text-nowrap");
                                   } else {
                                       $(td).addClass("text-danger col-ticket-type text-nowrap");
                                   }
                               } else {
                                   $(td).addClass("text-dark col-ticket-type text-nowrap");
                               }
                               $(td).html(
                                   '#' + cellData
                               );
                           }
                       },
                       {
                           'targets': 2,
                           'createdCell': function (td, cellData, rowData, row, col) {
                               if (enddate != "null") {
                                   if (enddate > today) {
                                       $(td).addClass("text-primary col-ticket-type text-nowrap");
                                   } else {
                                       $(td).addClass("text-danger col-ticket-type text-nowrap");
                                   }
                               } else {
                                   $(td).addClass("text-dark col-ticket-type text-nowrap");
                               }
                               $(td).html(
                                   cellData.split('|')[1]
                               );
                           }
                       },
                       {
                           'targets': 3,
                           'createdCell': function (td, cellData, rowData, row, col) {
                               $(td).addClass("col-subject text-truncate font-italic");
                           }
                       },
                       {
                           'targets': 4,
                           'createdCell': function (td, cellData, rowData, row, col) {
                               $(td).addClass("text-truncate col-customer text-nowrap");
                               if (cellData == 'null') { $(td).html(''); }
                           }
                       },
                       {
                           'targets': 5,
                           'createdCell': function (td, cellData, rowData, row, col) {
                               $(td).addClass("col-status text-nowrap");
                               $(td).html(
                                   '<div class="status ' + cellData.split('|')[0] +' ' + cellData.split('|')[1] + '">' +
                                        cellData.split('|')[1] +
                                   '</div>'
                               );
                           }
                       }
                    ]
                });

                function convertToDateDisplay(data) {
                    if (data.length >= 8) {
                        data = data.substring(6, 8) + '/' + data.substring(4, 6) + '/' + data.substring(0, 4);
                    }
                    return data;
                }

                return dataTableResult;
            }
        </script>
    </div>

</asp:Content>
