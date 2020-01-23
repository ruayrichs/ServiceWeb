<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="ServiceWeb.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://canvasjs.com/assets/script/canvasjs.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.1/Chart.min.js"></script>
    <style>
        .lines {
            margin-bottom: 10px;
            padding-bottom: 5px;
            border-bottom: 1px solid #ccc;
        }
        
        .lines .fa-fw {
                border-right: 1px solid #ccc;
                margin-right: 3px;
                padding-right: 20px;
            }

        .one-line {
            overflow-x: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }

        .text-primary {
            color: #337ab7 !important;
        }

        .bg-primary {
            background: #337ab7 !important;
            border-color: #337ab7 !important;
            color: #fff !important;
        }

        .text-success {
            color: #009688 !important;
        }

        .bg-success {
            background: #009688 !important;
            border-color: #1aa98d !important;
            color: #fff !important;
        }

        .text-info {
            color: #00BCD4 !important;
        }

        .bg-info {
            background: #00BCD4 !important;
            border-color: #00BCD4 !important;
            color: #fff !important;
        }

        .text-danger {
            color: #ff3223 !important;
        }

        .bg-danger {
            background: #ff3223 !important;
            border-color: #ff3223 !important;
            color: #fff !important;
        }

        .text-warning {
            color: #f99500 !important;
        }

        .bg-warning {
            background: #f99500 !important;
            border-color: #f99500 !important;
            color: #fff !important;
        }

        .customer-card {
            padding: 7px 15px;
            font-weight: 700;
        }

            .customer-card .customer-img {
                width: 52px;
                display: inline;
            }

                .customer-card .customer-img .image-box {
                    background-color: #575757;
                    border: 1px solid #aaa;
                    float: left;
                    width: 50px;
                    height: 50px;
                    margin-right: 10px;
                    background-image: url('<%= Page.ResolveUrl("~") %>images/user.png');
                    background-position: center center;
                    -webkit-background-size: cover;
                    -moz-background-size: cover;
                    background-size: cover;
                    -o-background-size: cover;
                    border-radius: 5px;
                }

            .customer-card .customer-desc {
                width: calc(100% - 52px);
                display: inline;
            }

        .dashboard-card {
            padding: 5px;
            text-align: center;
        }

            .dashboard-card > span {
                font-size: 40px;
                font-weight: 700;
            }

        .panel-list-ticket {
            display: none;
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
            width: 90px;
            padding: 2px 0px;
            font-weight: 500;
            text-align: center;
            margin: 0 auto;
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

        .status.finish {
            background: #4CAF50;
            color: white;
        }

        .status.delay {
            background: #f44336;
            color: white;
        }

        .status.cancel {
            background: #FF9800;
            color: white;
        }

        /*checkbox slider*/
        .switch {
            position: relative;
            display: inline-block;
            width: 60px;
            height: 34px;
            float: left;
        }


            .switch input {
                display: none;
            }

        /* The slider */
        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #f44336;
            -webkit-transition: .4s;
            transition: .4s;
            box-shadow: 0 3px 6px 0 rgba(0, 0, 0, 0.2), 0 3px 10px 0 rgba(0, 0, 0, 0.19);
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 26px;
                width: 26px;
                left: 4px;
                bottom: 4px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input.danger:checked + .slider {
            background-color: #00ff4e;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }
        /*end checkboc slider*/
    </style>
    <style>
        .fix-width-50 {
            width: 50px !important;
        }
        .fix-width-100 {
            width: 100px !important;
        }
    </style>
    <style>
        .card {
            margin-top: 8px;
        }

        .label {
            display: inline;
        }

        .a_center {
            display: flex;
            justify-content: center;
            align-items: center;
            /*text-align:center;*/
        }

        .h1_nz {
            font-size: 450%
        }
    </style>

    <%--<div class="card">
        <div class="card-body">--%>
    <div class="form-row">
        <div class="form-group col">
            <div class="card c-pointer panel-dashboard" data-id="panelListTicketOpen">
                <div class="text-primary card-body dashboard-card">
                    <asp:Label Text="0" runat="server" ID="lblCountOpen" ClientIDMode="Static"/>
                </div>
                <div class="card-header bg-primary text-center">
                    <b>Open</b>
                </div>
            </div>
        </div>
        <%--<div class="form-group col">
            <div class="card c-pointer panel-dashboard" data-id="panelListTicketUnassigned">
                <div class="text-warning card-body dashboard-card">
                    <asp:Label Text="0" runat="server" ID="lblCountUnassigned" ClientIDMode="Static"/>
                </div>
                <div class="card-header bg-warning text-center">
                    <b>Unassigned</b>
                </div>
            </div>
        </div>--%>
        <div class="form-group col">
            <div class="card c-pointer panel-dashboard" data-id="panelListTicketDelay">
                <div class="text-danger card-body dashboard-card">
                    <asp:Label Text="0" runat="server" ID="lblCountDelay" ClientIDMode="Static"/>
                </div>
                <div class="card-header bg-danger text-center">
                    <b>Delay</b>
                </div>
            </div>
        </div>
        <div class="form-group col">
            <div class="card c-pointer panel-dashboard" data-id="panelListTicketSuccess">
                <div class="text-success card-body dashboard-card">
                    <asp:Label Text="0" runat="server" ID="lblCountSuccess" ClientIDMode="Static"/>
                </div>
                <div class="card-header bg-success text-center">
                    <b>Finish</b>
                </div>
            </div>
        </div>
        <div class="form-group col">
            <div class="card c-pointer panel-dashboard" data-id="panelListTicketAll">
                <div class="text-info card-body dashboard-card">
                    <asp:Label Text="0" runat="server" ID="lblCountAll" ClientIDMode="Static"/>
                </div>
                <div class="card-header bg-info text-center">
                    <b>All</b>
                </div>
            </div>
        </div>
    </div>

    <div class="d-none">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btnLinkTransactionSearch" runat="server" ClientIDMode="Static"
                    CssClass="d-none" OnClick="btnLinkTransactionSearch_Click" OnClientClick="AGLoading(true);" />
                <asp:HiddenField runat="server" ID="hddCallerID_Criteria" Value="" ClientIDMode="Static" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="panel-list-ticket" id="dataPanelListTicketOpen" style="display: block;">
        <!-- Open -->
        <div class="card mb-3">
            <div class="card-header bg-primary text-white c-pointer" id="headTicketOpen">
                <b>Open</b>
            </div>
            <div class="card-body">
                
                <div class="table-responsive">
                    <table id="table-Open" class="table table-bordered table-striped table-hover table-sm">
                        <thead>
                            <tr>
                                <th class="text-nowrap fix-width-50"></th>
                                <th class="text-nowrap fix-width-100">Ticket No.</th>
                                <th class="text-nowrap fix-width-100">Date</th>
                                <th class="text-nowrap fix-width-100">Ticket Type</th>
                                <th class="text-nowrap">Subject</th>
                                <th class="text-nowrap fix-width-100">Priority</th>
                                <th class="text-nowrap fix-width-100"">Assignee</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptOpenTask" runat="server">
                                <ItemTemplate>
                                    <tr class="c-pointer" onclick="openTicket('<%# Eval("TicketNo") %>');"> 
                                        <td class="text-nowrap text-center align-middle">
                                            <i class="fa fa-pencil-square fa-lg text-dark c-pointer" title="Edit" onclick="$(this).next().click();"></i>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# Eval("TicketNo") %>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# Eval("Date") %>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# "#" + Eval("TicketType") %>
                                        </td>
                                        <td class="text-nowrap" style="width: 50%;">
                                            <%# Eval("Subject") %>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# Eval("Priority")%>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# Eval("Assignee")%>
                                        </td>
                                        <%--<td class="col-status">
                                            <div class="status <%# Eval("StatusCode") %>">
                                                <%# Eval("StatusDesc") %>
                                            </div>
                                        </td>--%>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <% if (rptOpenTask.Items.Count == 0)
                                        { %>
                                    <tr>
                                        <td class="text-nowrap text-center" <%--colspan="6"--%>>ไม่พบข้อมูล																</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <% } %>
                                </FooterTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <%--<div class="panel-list-ticket" id="dataPanelListTicketUnassigned" style="display: block;">
        <!-- Unassigned -->
    </div>--%>

    <div class="panel-list-ticket" id="dataPanelListTicketDelay" style="display: block;">
        <!-- Delay -->
        <div class="card mb-3">
            <div class="card-header bg-danger text-white c-pointer" id="headTicketDelay">
                <b>Delay</b>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table id="table-Delay" class="table table-bordered table-striped table-hover table-sm">
                        <thead>
                            <tr>
                                <th class="text-nowrap fix-width-50"></th>
                                <th class="text-nowrap fix-width-100">Ticket No.</th>
                                <th class="text-nowrap fix-width-100">Date</th>
                                <th class="text-nowrap fix-width-100">Ticket Type</th>
                                <th class="text-nowrap">Subject</th>
                                <th class="text-nowrap fix-width-100">Priority</th>
                                <th class="text-nowrap fix-width-100"">Assignee</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="Repeater2" runat="server">
                                <ItemTemplate>
                                    <tr class="c-pointer" onclick="openTicket('<%# Eval("TicketNo") %>');"> 
                                        <td class="text-nowrap text-center align-middle">
                                            <i class="fa fa-pencil-square fa-lg text-dark c-pointer" title="Edit" onclick="$(this).next().click();"></i>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# Eval("TicketNo") %>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# Eval("Date") %>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# "#" + Eval("TicketType") %>
                                        </td>
                                        <td class="text-nowrap" style="width: 50%;">
                                            <%# Eval("Subject") %>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# Eval("Priority")%>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# Eval("Assignee")%>
                                        </td>
                                        <%--<td class="col-status">
                                            <div class="status <%# Eval("StatusCode") %>">
                                                <%# Eval("StatusDesc") %>
                                            </div>
                                        </td>--%>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <% if (rptOpenTask.Items.Count == 0)
                                        { %>
                                    <tr>
                                        <td class="text-nowrap text-center" <%--colspan="6"--%>>ไม่พบข้อมูล																</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <% } %>
                                </FooterTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div class="panel-list-ticket" id="dataPanelListTicketFinish" style="display: block;">
        <!-- Finish -->
        <div class="card mb-3">
            <div class="card-header bg-success text-white c-pointer" id="headTicketFinish">
                <b>Finish</b>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table id="table-Finish" class="table table-bordered table-striped table-hover table-sm">
                        <thead>
                            <tr>
                                <th class="text-nowrap fix-width-50"></th>
                                <th class="text-nowrap fix-width-100">Ticket No.</th>
                                <th class="text-nowrap fix-width-100">Date</th>
                                <th class="text-nowrap fix-width-100">Ticket Type</th>
                                <th class="text-nowrap">Subject</th>
                                <th class="text-nowrap fix-width-100">Priority</th>
                                <th class="text-nowrap fix-width-100"">Assignee</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="Repeater3" runat="server">
                                <ItemTemplate>
                                    <tr class="c-pointer" onclick="openTicket('<%# Eval("TicketNo") %>');"> 
                                        <td class="text-nowrap text-center align-middle">
                                            <i class="fa fa-pencil-square fa-lg text-dark c-pointer" title="Edit" onclick="$(this).next().click();"></i>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# Eval("TicketNo") %>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# Eval("Date") %>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# "#" + Eval("TicketType") %>
                                        </td>
                                        <td class="text-nowrap" style="width: 50%;">
                                            <%# Eval("Subject") %>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# Eval("Priority")%>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# Eval("Assignee")%>
                                        </td>
                                        <%--<td class="col-status">
                                            <div class="status <%# Eval("StatusCode") %>">
                                                <%# Eval("StatusDesc") %>
                                            </div>
                                        </td>--%>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <% if (rptOpenTask.Items.Count == 0)
                                        { %>
                                    <tr>
                                        <td class="text-nowrap text-center" <%--colspan="6"--%>>ไม่พบข้อมูล																</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <% } %>
                                </FooterTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div class="panel-list-ticket" id="dataPanelListTicketAll" style="display: block;">
        <!-- All -->
        <div class="card mb-3">
            <div class="card-header bg-info text-white c-pointer" id="headTicketAll">
                <b>All</b>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table id="table-All" class="table table-bordered table-striped table-hover table-sm">
                        <thead>
                            <tr>
                                <th class="text-nowrap fix-width-50"></th>
                                <th class="text-nowrap fix-width-100">Ticket No.</th>
                                <th class="text-nowrap fix-width-100">Date</th>
                                <th class="text-nowrap fix-width-100">Ticket Type</th>
                                <th class="text-nowrap">Subject</th>
                                <th class="text-nowrap fix-width-100">Priority</th>
                                <th class="text-nowrap fix-width-100"">Assignee</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="Repeater4" runat="server">
                                <ItemTemplate>
                                    <tr class="c-pointer" onclick="openTicket('<%# Eval("TicketNo") %>');"> 
                                        <td class="text-nowrap text-center align-middle">
                                            <i class="fa fa-pencil-square fa-lg text-dark c-pointer" title="Edit" onclick="$(this).next().click();"></i>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# Eval("TicketNo") %>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# Eval("Date") %>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# "#" + Eval("TicketType") %>
                                        </td>
                                        <td class="text-nowrap" style="width: 50%;">
                                            <%# Eval("Subject") %>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# Eval("Priority")%>
                                        </td>
                                        <td class="text-nowrap">
                                            <%# Eval("Assignee")%>
                                        </td>
                                        <%--<td class="col-status">
                                            <div class="status <%# Eval("StatusCode") %>">
                                                <%# Eval("StatusDesc") %>
                                            </div>
                                        </td>--%>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <% if (rptOpenTask.Items.Count == 0)
                                        { %>
                                    <tr>
                                        <td class="text-nowrap text-center" <%--colspan="6"--%>>ไม่พบข้อมูล																</td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <% } %>
                                </FooterTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <%--</div>
    </div>--%>

    <div class="form-row">
        <div class="col-xl-9">
            <div class="form-row">
                <div class="col-xl-6">
                    <!-- card 1 Open Incidents Map -->
                    <div class="card">
                        <div class="card-header bg-success" style="text-align: center;">
                            <b>All Service Open</b>
                        </div>
                        <div class="card-body">
                            <canvas id="open-incident-map" height="250"></canvas>
                        </div>
                    </div>
                    <!-- end card 1 Open Incident Map -->
                </div>
                <div class="col-xl-6">
                    <!-- card 6 Open Incidents By State -->
                    <div class="card">
                        <div class="card-header bg-info" style="text-align: center;">
                            <b>All Service Open/Close/Cancel</b>
                        </div>
                        <div class="card-body">
                            <!-- <h1>Graph</h1> -->
                            <canvas id="open-incident-by-state" height="250"></canvas>
                        </div>
                    </div>
                    <!-- end card 6 Open Incident By State -->
                </div>
            </div>
            <div class="form-row">
                <div class="col-12">
                    
                    <div class="card">
                        <div class="card-header bg-primary" style="text-align: center;">
                            <b>Problem Group</b>
                        </div>
                        <div class="card-body">
                            <canvas id="open-incident-line-chart" height="250"></canvas>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-row">
                <div class="col-xl-12">
                    <%--<div class="card">
                        <div class="card-body">--%>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpTS">
                        <ContentTemplate>
                            <div class="form-row">
                                <asp:Repeater ID="rptTS" runat="server">
                                    <ItemTemplate>
                                        <div class="col-xl-4">
                                            <!-- card 2 Open Incidents Storied-->
                                            <div class="card" style="height: 200px;">
                                                <div class="card-header bg-info" style="text-align: center;">
                                                    <h5><small>(<%# Eval("StatusType") %>)</small><br /><%# Eval("StatusDesc") %></h5>
                                                </div>
                                                <div class="card-body a_center">
                                                    <b>
                                                        <h1 class="h1_nz" id="<%# Eval("StatusCode") %>_value"><%# Eval("CountTicket") %></h1>
                                                    </b>
                                                </div>
                                            </div>
                                            <!-- end card 2 Open Incidents Storied -->
                                        </div>
                                     </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--</div>
                    </div>--%>
                </div>
            </div>
        </div>
        <div class="col-xl-3">
            <!-- zone 4 -->
            <div class="card">
                <div class="card-header bg-primary" style="text-align: center;">
                    <b>Incident Service Levels</b>
                </div>
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpPriority">
                    <ContentTemplate>
                        <asp:Repeater ID="rtpPriority" runat="server">
                            <ItemTemplate>
                                <div class="card-body">
                                    <%--<div>--%>
                                    <p><%# Eval("PiorityDesc") %></p>
                                    <table class="table table-striped table-bordered">
                                        <tr>
                                            <th>Type</th>
                                            <th>Count</th>
                                        </tr>
                                        <tr>
                                            <td>All Ticket</td>
                                            <td id="<%# Eval("PiorityCode") %>_All Ticket"><%# Eval("CountTicket_All") %></td>
                                        </tr>
                                        <tr>
                                            <td>Inprogress</td>
                                            <td id="<%# Eval("PiorityCode") %>_Inprogress"><%# Eval("CountTicket_InProgress") %></td>
                                        </tr>
                                        <tr>
                                            <td>Success</td>
                                            <td id="<%# Eval("PiorityCode") %>_Success"><%# Eval("CountTicket_Success") %></td>
                                        </tr>
                                    </table>
                                    <%--</div>--%>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!-- zone 4 -->
        </div>
    </div>

    <!-- setting -->

    <style>
        .rconfig{
                position: fixed;
                right: 80px;
                bottom: 20px;
                padding: 6px 12px;
                border: 1px solid dodgerblue;
                border-radius: 50%;
                background-color: dodgerblue;
                cursor: pointer;
                box-shadow: 0 5px 11px 0 rgba(0, 0, 0, 0.18), 0 4px 15px 0 rgba(0, 0, 0, 0.15);
                font-size: 21px;
                }
    </style>
    <div>
        <div class="rconfig" data-toggle="modal" data-target="#exampleModal"><i class="fa fa-cogs"></i></div>
        <!-- Modal -->
        <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
          <div class="modal-dialog" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">Page Configuration</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body">
                  <div class="row">
                      <div class="col-sm-6">
                          <div><label>Auto Refresh (seconds)</label></div>
                          <select id="refreshrate" class="form-control form-control-sm">
                              <option id="rr86400" value="86400">Not Refresh</option>
                              <option id="rr5" value="5">5</option>
                              <option id="rr30" value="30">30</option>
                              <option id="rr60" value="60">60</option>
                              <option id="rr300" value="300">300</option>
                          </select>
                      </div>
                      <div class="col-sm-6">
                          <div><label>Data Option</label></div>
                          <select id="optiondata" class="form-control form-control-sm">
                              <option id="ot_" value="">All</option>
                              <option id="ot_today" value="today">Today</option>
                              <option id="ot_week" value="week">This Week</option>
                              <option id="ot_month" value="month">This Month</option>
                              <option id="ot_year" value="year">This Year</option>
                          </select>
                      </div>
                  </div>
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-primary" data-dismiss="modal" onclick="AGLoading(true);autoRefreshPrepareData();">Save</button>
              </div>
            </div>
          </div>
        </div>
    </div>
    <!--
        set PieChart
        -->
    <script>
        
        function randomScalingFactor() {
            return (Math.floor(Math.random() * 6) + 1 );
        }
        var pie;
        var myPieChart;

        var lineChart;
        var mylineChart;

        function setLineChart(datas_problem_group, datas_incident_count, datas_problem_count, datas_request_count, datas_change_count) {
            lineChart = document.getElementById('open-incident-line-chart');
            mylineChart= new Chart(lineChart, {
                type: 'line',
                data: {
                    labels: datas_problem_group,
                    datasets: [{
                        label: 'Incident',
                        backgroundColor: "#29B0D0",
                        borderColor: "#29B0D0",
                        data: datas_incident_count,
                        fill: false,
                    }, {
                        label: 'Problem',
                        fill: false,
                        backgroundColor: "#2A516E",
                        borderColor: "#2A516E",
                        data: datas_problem_count,
                    }, {
                        label: 'Request',
                        fill: false,
                        backgroundColor: "#F07124",
                        borderColor: "#F07124",
                        data: datas_request_count,
                    }, {
                        label: 'Change Order',
                        fill: false,
                        backgroundColor: "#CBE0E3",
                        borderColor: "#CBE0E3",
                        data: datas_change_count,
                    }]
                },
                options: {
                    responsive: true,
                    title: {
                        display: true,
                        text: 'Chart.js Line Chart'
                    },
                    tooltips: {
                        mode: 'index',
                        intersect: false,
                    },
                    hover: {
                        mode: 'nearest',
                        intersect: true
                    },
                    scales: {
                        xAxes: [{
                            display: true,
                            scaleLabel: {
                                display: true,
                                labelString: 'Problem Group'
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                reverse: false,
                                //stepSize: 5
                            },
                            display: true,
                            scaleLabel: {
                                display: true,
                                labelString: 'Value'
                            }
                        }]
                    }
                }
            });
            //mylineChart.canvas.parentNode.style.height = '350px';
        }

        function setPieChart(incident_count, problem_count, request_count, change_count) {
            // open incidents map
            pie = document.getElementById('open-incident-map');
            myPieChart = new Chart(pie, {
                type: 'pie',
                data: {
                    labels: ['Incident', 'Problem', 'Request', 'Change Order'],
                    datasets: [{
                        label: '# of Votes',
                        data: [incident_count, problem_count, request_count, change_count],
                        backgroundColor: [
                            '#29B0D0',
                            '#2A516E',
                            '#F07124',
                            '#CBE0E3',
                            '#979193'
                        ]
                    }]
                },
                options: {
                    legend: {
                        display: false,
                        labels: {
                            display: false
                        }
                    }
                },
            });
        };
    </script>
    <!--
        set BarChart
        -->
    <script>
        var ctx;
        var myBarChart;
        function setBarChart(i_open_count, p_open_count, r_open_count, c_open_count,i_close_count, p_close_count, r_close_count, c_close_count,i_cancel_count, p_cancel_count, r_cancel_count, c_cancel_count) {
            // open incidents by state
            ctx = document.getElementById('open-incident-by-state').getContext('2d');
            var maxvalue = 52;
            var scale = 1; //default value
            if (maxvalue >= 1 && maxvalue <= 10) {
                scale = 1;
            }
            else if (maxvalue >= 11 && maxvalue <= 50) {
                scale = 5;
            }
            else if (maxvalue >= 51) {
                scale = 10;
            } else {
                scale = 1;
            }
            myBarChart = new Chart(ctx, {
                type: 'horizontalBar',
                data: {
                    labels: ['Incident', 'Problem', 'Request', 'Change Order'],
                    datasets: [
                        {
                            label: 'Inprogess',
                            data: [i_open_count, p_open_count, r_open_count, c_open_count],
                            backgroundColor: ['#ff9900','#ff9900','#ff9900','#ff9900']
                        },
                        {
                            label: 'Close',
                            data: [i_close_count, p_close_count, r_close_count, c_close_count],
                            backgroundColor: ['#009933', '#009933', '#009933', '#009933']
                        },
                        {
                            label: 'Cancel',
                            data: [i_cancel_count, p_cancel_count, r_cancel_count, c_cancel_count],
                            backgroundColor: ['#ff471a','#ff471a','#ff471a','#ff471a']
                        }]
                },
                options: {
                    legend: {
                        display: false,
                        labels: {
                            display: false
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                fontSize: 12,
                                max: 80
                            }
                        }],
                        xAxes: [{
                            ticks: {
                                beginAtZero: true,
                                fontSize: 11
                            }
                        }]
                    }
                }
            });
        };        
    </script>
    <!--
        initChart()
        -->
    <script>
        //set chart first time
        function initChart() {
            try {
                //pie
                var incident_count = <%= incident_count %>;
                var problem_count = <%= problem_count %>;
                var request_count = <%= request_count %>;
                var change_count = <%= change_count %>;
                //bar            
                var i_open_count = <%= i_open_count %>;
                var i_close_count = <%= i_close_count %>;
                var i_cancel_count = <%= i_cancel_count %>;
                //bar
                var p_open_count = <%= p_open_count %>;
                var p_close_count = <%= p_close_count %>;
                var p_cancel_count = <%= p_cancel_count %>;
                //bar
                var r_open_count = <%= r_open_count %>;
                var r_close_count = <%= r_close_count %>;
                var r_cancel_count = <%= r_cancel_count %>;
                //bar
                var c_open_count = <%= c_open_count %>;
                var c_close_count = <%= c_close_count %>;
                var c_cancel_count = <%= c_cancel_count %>;
                
                setPieChart(incident_count, problem_count, request_count, change_count);

                var datas_problem_group = <%= datasProblemGroup %>;
                var datas_incident_count = <%= dataCountTicketIncident %>;
                var datas_problem_count = <%= dataCountTicketProblem %>;
                var datas_request_count = <%= dataCountTicketRequest %>;
                var datas_change_count = <%= dataCountTicketChangeOrder %>;
                setLineChart(datas_problem_group, datas_incident_count, datas_problem_count, datas_request_count, datas_change_count);

                setBarChart(i_open_count, p_open_count, r_open_count, c_open_count,
                    i_close_count, p_close_count, r_close_count, c_close_count,
                    i_cancel_count, p_cancel_count, r_cancel_count, c_cancel_count);

                return "ok";
            } catch (err) {
                return ("not ok: "+err);
            }
        };
    </script>
    <!--
        refreshData(optiondata)
        autoRefresh(seconds,optiondata)
        autoRefreshPrepareData()
        -->
    <script>
        var inter = null;
        var result = null;
        var time = 86400;
        var optiondata = "today";
        var expiryDate = new Date();
        //

        var myTaskDataTable_Open;
        var myTaskDataTable_Unassigned;
        var myTaskDataTable_Delay;
        var myTaskDataTable_Finish;
        var myTaskDataTable_All;
        //
        function refreshData() {
            //AGLoading(true);
            var data = {
                optiondata:this.optiondata
            };

            $.ajax({
                type: "GET",
                url: "<%= Page.ResolveUrl("~/API/DashboardAPI.aspx") %>",
                data: data,
                success: function (res) {
                    result = JSON.parse(res);
                    //set overview
                    document.getElementById("lblCountOpen").innerHTML = result.OverviewDataReport.CountTicketOpen;
                    //document.getElementById("lblCountUnassigned").innerHTML = result.OverviewDataReport.CountTicketUnassigned;
                    document.getElementById("lblCountDelay").innerHTML = result.OverviewDataReport.CountTicketDelay;
                    document.getElementById("lblCountSuccess").innerHTML = result.OverviewDataReport.CountTicketFinish;
                    document.getElementById("lblCountAll").innerHTML = result.OverviewDataReport.CountTicketAll;

                    //set pie chart
                    myPieChart.data.datasets[0].data = [
                        result.PieChartDataReport.CountTicketIncident,
                        result.PieChartDataReport.CountTicketProblem,
                        result.PieChartDataReport.CountTicketRequest,
                        result.PieChartDataReport.CountTicketChangeOrder
                    ];
                    myPieChart.update();
                        
                    console.log(result.LineChartDataReport);
                    mylineChart.data.labels = result.LineChartDataReport.datasProblemGroup;
                    mylineChart.data.datasets[0].data = result.LineChartDataReport.dataCountTicketIncident;
                    mylineChart.data.datasets[1].data = result.LineChartDataReport.dataCountTicketProblem;
                    mylineChart.data.datasets[2].data = result.LineChartDataReport.dataCountTicketRequest;
                    mylineChart.data.datasets[3].data = result.LineChartDataReport.dataCountTicketChangeOrder;
                    mylineChart.update();

                    //console.log("update myPieChart success");
                    //set bar chart
                    myBarChart.data.datasets[0].data = [
                        result.BarChartDataReport.CountTicketIncident_InProgress,
                        result.BarChartDataReport.CountTicketProblem_InProgress,
                        result.BarChartDataReport.CountTicketRequest_InProgress,
                        result.BarChartDataReport.CountTicketChangeOrder_InProgress
                    ];
                    myBarChart.data.datasets[1].data = [
                        result.BarChartDataReport.CountTicketIncident_Close,
                        result.BarChartDataReport.CountTicketProblem_Close,
                        result.BarChartDataReport.CountTicketRequest_Close,
                        result.BarChartDataReport.CountTicketChangeOrder_Close
                    ];
                    myBarChart.data.datasets[2].data = [
                        result.BarChartDataReport.CountTicketIncident_Cancel,
                        result.BarChartDataReport.CountTicketProblem_Cancel,
                        result.BarChartDataReport.CountTicketRequest_Cancel,
                        result.BarChartDataReport.CountTicketChangeOrder_Cancel
                    ];
                    myBarChart.update();
                    //console.log("update myBarChart success");
                    for (let index = 0; index < result.StatusDataReport.length; index++) {
                        document.getElementById(result.StatusDataReport[index].StatusCode + "_value").innerHTML = result.StatusDataReport[index].CountTicket;
                        //console.log(result.StatusDataReport[index].StatusDesc + "_value : " + result.StatusDataReport[index].CountTicket);
                    }
                    //console.log("update StatusDataReport success");
                    for (let index = 0; index < result.PiorityDataReport.length; index++) {
                        document.getElementById(result.PiorityDataReport[index].PiorityCode + "_All Ticket").innerHTML = result.PiorityDataReport[index].CountTicket_All;
                        document.getElementById(result.PiorityDataReport[index].PiorityCode + "_Inprogress").innerHTML = result.PiorityDataReport[index].CountTicket_InProgress;
                        document.getElementById(result.PiorityDataReport[index].PiorityCode + "_Success").innerHTML = result.PiorityDataReport[index].CountTicket_Success;
                        //console.log(result.PiorityDataReport[index].PiorityCode + "_All Ticket : " + result.PiorityDataReport[index].CountTicket_All);
                        //console.log(result.PiorityDataReport[index].PiorityCode + "_Inprogress : " + result.PiorityDataReport[index].CountTicket_InProgress);
                        //console.log(result.PiorityDataReport[index].PiorityCode + "_Success : " + result.PiorityDataReport[index].CountTicket_Success);
                    }
                    //console.log("update PiorityDataReport success");
                    console.log(res);

                    AGLoading(false);
                    //---DEV for Open Ticket---
                    $("[data-id='panelListTicketOpen']").AGWhiteLoading(true, 'Loading...');

                    objDatasMyTicket = result.OverviewDataReportNo;
                    //objDatasMyTicketUnassigned = result.OverviewDataReportNoUnassigned;
                    objDatasMyTicketDelay = result.OverviewDataReportNoDelay;
                    objDatasMyTicketFinish = result.OverviewDataReportNoFinish;
                    objDatasMyTicketAll = result.OverviewDataReportNoAll;

                    $("#<%= lblCountOpen %>").html(objDatasMyTicket);
                    $("[data-id='panelListTicketOpen']").AGWhiteLoading(false, 'Loading...');
                    <%--$("#<%= lblCountUnassigned %>").html(objDatasMyTicketUnassigned);
                    $("[data-id='panelListTicketUnassigned']").AGWhiteLoading(false, 'Loading...');--%>

                    $("#table-Open").AGWhiteLoading(true, 'Loading...');
                    $("#table-Unassigned").AGWhiteLoading(true, 'Loading...');

                    if (myTaskDataTable_Open != undefined) {
                        myTaskDataTable_Open.fnDestroy();
                    }
                    if (myTaskDataTable_Unassigned != undefined) {
                        myTaskDataTable_Unassigned.fnDestroy();
                    }
                    if (myTaskDataTable_Delay != undefined) {
                        myTaskDataTable_Delay.fnDestroy();
                    }
                    if (myTaskDataTable_Finish != undefined) {
                        myTaskDataTable_Finish.fnDestroy();
                    }
                    if (myTaskDataTable_All != undefined) {
                        myTaskDataTable_All.fnDestroy();
                    }
                    myTaskDataTable_Open = bindListTicket_1($("#table-Open"), objDatasMyTicket);
                    $("#table-Open").AGWhiteLoading(false, 'Loading...');

                    //myTaskDataTable_Unassigned = bindListTicket_1($("#table-Unassigned"), objDatasMyTicketUnassigned);
                    //$("#table-Unassigned").AGWhiteLoading(false, 'Loading...');

                    myTaskDataTable_Delay = bindListTicket_1($("#table-Delay"), objDatasMyTicketDelay);
                    $("#table-Delay").AGWhiteLoading(false, 'Loading...');

                    myTaskDataTable_Finish = bindListTicket_1($("#table-Finish"), objDatasMyTicketFinish);
                    $("#table-Finish").AGWhiteLoading(false, 'Loading...');

                    myTaskDataTable_All = bindListTicket_1($("#table-All"), objDatasMyTicketAll);
                    $("#table-All").AGWhiteLoading(false, 'Loading...');
                },
                error: function (err) {
                        console.log(err);
                }
            });
        };
        //
        function autoRefresh(seconds) {
            clearInterval(this.inter);
            seconds = seconds * 1000;
            console.log("page will run every " + seconds + "/1000 seconds");
            this.inter = window.setInterval(refreshData, seconds);
        };
        //
        
        function autoRefreshPrepareData() {
            // get data from ddl
            this.time = parseInt(document.getElementById("refreshrate").value);
            this.optiondata = document.getElementById("optiondata").value;
            // set cookie data
            document.cookie = "time =" + time + "; expires=" + expiryDate.setMonth(expiryDate.getMonth() + 1);
            document.cookie = "optiondata =" + optiondata + "; expires=" + expiryDate.setMonth(expiryDate.getMonth() + 1);
            console.log("time: " + time + ", optiondata: " + optiondata);
            // get new data now
            refreshData();
            // get new data every [time] seconds
            autoRefresh(time);
        };
    </script>
    <!--
        fetchCookieData()
        webOnLoad() 
        -->
    <script>
        const rawTime = [86400, 5, 30, 60, 300];
        const rawOptionData = ["", "today", "week", "month", "year"];
        var cip = null;
        var _cip = null;
        //
        function fetchCookieData() {
            cip = document.cookie;
            _cip = cip.split("; ");
            for (let index = 0; index < _cip.length; index++) {
                if (_cip[index].indexOf("time") == 0) {
                    let t = _cip[index];
                    t = t.split("=");
                    this.time = t[1];
                }
                if (_cip[index].indexOf("optiondata") == 0) {
                    let op = _cip[index];
                    op = op.split("=");
                    this.optiondata = op[1];
                }
            }
            console.log("data from cookie: cookie[time]: " + time + " ,cookie[optiondata]: " + optiondata);
            //set new selected
            for (let index = 0; index < rawTime.length; index++) {
                //if (time == document.getElementById("rr" + rawTime[index]).value) {
                console.log("rawTime: " + rawTime[index]);
                if (this.time == rawTime[index]) {
                    document.getElementById("rr" + rawTime[index]).setAttribute("selected", "selected");
                    //document.getElementById("rr5").setAttribute("selected","selected");
                    console.log("set rr" + rawOptionData[index]);
                }
            }
            for (let index = 0; index < rawOptionData.length; index++) {
                console.log("rawOptionData: " + rawOptionData[index]);
                //if (time == document.getElementById("ot_" + rawOptionData[index]).value) {
                if (this.optiondata == rawOptionData[index]) {
                    document.getElementById("ot_" + rawOptionData[index]).setAttribute("selected", "selected");
                    console.log("set ot_" + rawOptionData[index]);
                }
            }
        };
        //
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-dashboard").className = "nav-link active";

            //set chart first time
            var code = initChart();
            console.log("chart status: " + code);
            //fetch old data from cookie
            fetchCookieData();
            //fetch defult data
            code = refreshData(this.optiondata);
            //console.log("api status: " + code);
        };

        $(document).ready(function () {
            webOnLoad();
            toggleCard();
        })

        ////////////////////////////////////////////
        function goToEdit(url) {
            var height = document.documentElement.clientHeight;
            window.open(url, '_blank', 'location=yes,height=' + height + ',width=1200,scrollbars=yes,status=yes');
        }
        function toggleCard() {
            $("#dataPanelListTicketOpen").hide();
            //$("#dataPanelListTicketUnassigned").hide();
            $("#dataPanelListTicketDelay").hide();
            $("#dataPanelListTicketFinish").hide();
            $("#dataPanelListTicketAll").hide();

            //$('#dataPanelListTicketOpen').trigger('click');
        }
        $("[data-id='panelListTicketOpen']").click(function () {
            var wasVisible = $("#dataPanelListTicketOpen").is(":visible");
            $("[id^=element]:visible").stop().slideUp("slow");
            if (!wasVisible) {
                if (myTaskDataTable_Open != undefined) {
                    //alert('Open');
                    myTaskDataTable_Open.fnDestroy();
                    $("#dataPanelListTicketOpen").slideToggle("slow");
                    myTaskDataTable_Open = bindListTicket_1($("#table-Open"), objDatasMyTicket);
                }
            } else {
                if (myTaskDataTable_Open != undefined) {
                    //alert('Closs');
                    $("#dataPanelListTicketOpen").slideToggle("slow");
                }
            }
            $("#table-Open").AGWhiteLoading(false, 'Loading...');
        });
        $("#headTicketOpen").click(function () {
            $("#dataPanelListTicketOpen").slideToggle("slow");
        });
        //$("[data-id='panelListTicketUnassigned']").click(function () {
        //    var wasVisible = $("#dataPanelListTicketUnassigned").is(":visible");
        //    $("[id^=element]:visible").stop().slideUp("slow");
        //    if (!wasVisible) {
        //        if (myTaskDataTable_Unassigned != undefined) {
        //            //alert('Open');
        //            myTaskDataTable_Unassigned.fnDestroy();
        //            $("#dataPanelListTicketUnassigned").slideToggle("slow");
        //            myTaskDataTable_Unassigned = bindListTicket_1($("#table-Unassigned"), objDatasMyTicketUnassigned);
        //        }
        //    } else {
        //        if (myTaskDataTable_Unassigned != undefined) {
        //            //alert('Closs');
        //            $("#dataPanelListTicketUnassigned").slideToggle("slow");
        //        }
        //    }
        //    $("#table-Unassigned").AGWhiteLoading(false, 'Loading...');
        //});
        //$("#headTicketUnassigned").click(function () {
        //    $("#dataPanelListTicketUnassigned").slideToggle("slow");
        //});
        $("[data-id='panelListTicketDelay']").click(function () {
            var wasVisible = $("#dataPanelListTicketDelay").is(":visible");
            $("[id^=element]:visible").stop().slideUp("slow");
            if (!wasVisible) {
                if (myTaskDataTable_Delay != undefined) {
                    //alert('Open');
                    myTaskDataTable_Delay.fnDestroy();
                    $("#dataPanelListTicketDelay").slideToggle("slow");
                    myTaskDataTable_Delay = bindListTicket_1($("#table-Delay"), objDatasMyTicketDelay);
                }
            } else {
                if (myTaskDataTable_Delay != undefined) {
                    //alert('Closs');
                    $("#dataPanelListTicketDelay").slideToggle("slow");
                }
            }
            $("#table-Delay").AGWhiteLoading(false, 'Loading...');
        });
        $("#headTicketDelay").click(function () {
            $("#dataPanelListTicketDelay").slideToggle("slow");
        });
        $("[data-id='panelListTicketSuccess']").click(function () {
            var wasVisible = $("#dataPanelListTicketFinish").is(":visible");
            $("[id^=element]:visible").stop().slideUp("slow");
            if (!wasVisible) {
                if (myTaskDataTable_Finish != undefined) {
                    //alert('Open');
                    myTaskDataTable_Finish.fnDestroy();
                    $("#dataPanelListTicketFinish").slideToggle("slow");
                    myTaskDataTable_Finish = bindListTicket_1($("#table-Finish"), objDatasMyTicketFinish);
                }
            } else {
                if (myTaskDataTable_Finish != undefined) {
                    //alert('Closs');
                    $("#dataPanelListTicketFinish").slideToggle("slow");
                }
            }
            $("#table-Finish").AGWhiteLoading(false, 'Loading...');
        });
        $("#headTicketFinish").click(function () {
            $("#dataPanelListTicketFinish").slideToggle("slow");
        });
        $("[data-id='panelListTicketAll']").click(function () {
            var wasVisible = $("#dataPanelListTicketAll").is(":visible");
            $("[id^=element]:visible").stop().slideUp("slow");
            if (!wasVisible) {
                if (myTaskDataTable_All != undefined) {
                    //alert('Open');
                    myTaskDataTable_All.fnDestroy();
                    $("#dataPanelListTicketAll").slideToggle("slow");
                    myTaskDataTable_All = bindListTicket_1($("#table-All"), objDatasMyTicketAll);
                }
            } else {
                if (myTaskDataTable_All != undefined) {
                    //alert('Closs');
                    $("#dataPanelListTicketAll").slideToggle("slow");
                }
            }
            $("#table-All").AGWhiteLoading(false, 'Loading...');
        });
        $("#headTicketAll").click(function () {
            $("#dataPanelListTicketAll").slideToggle("slow");
        });

        function bindListTicket_1(objTarget, datas) {

            var datasTicket = [];
            for (var i = 0; i < datas.length; i++) {
                var ticket = datas[i];

                datasTicket.push([
                    ticket.TicketNO,
                    ticket.TicketNo4Display,
                    ticket.StartDateTime,
                    "#" + ticket.TicketType,
                    ticket.Subject,
                    ticket.Priority,
                    ticket.Assignee
                    //ticket.StatusCode + "|" + ticket.StatusDesc
                ]);
            }

            var dataTableResult = objTarget.dataTable({
                data: datasTicket,
                deferRender: true,
                "order": [[2, "desc"]],
                'columnDefs': [
                    {
                        'targets': 0,
                        'createdCell': function (td, cellData, rowData, row, col) {
                            $(td).addClass("text-nowrap text-center align-middle");
                            $(td).html(
                                '<i class="fa fa-pencil-square fa-lg text-dark c-pointer" title="Edit"></i>'
                            );
                            $(td).closest("tr").addClass("c-pointer");
                            $(td).closest("tr").bind("click", function () {
                                var docnumber = cellData;
                                openTicket(docnumber);
                            });

                        }
                    },
                    {
                        'targets': [1, 3, 5, 6],
                        'createdCell': function (td, cellData, rowData, row, col) {
                            $(td).addClass("text-truncate text-nowrap");
                        }
                    },
                    {
                        'targets': [2],
                        'createdCell': function (td, cellData, rowData, row, col) {
                            $(td).addClass("text-truncate text-nowrap");
                            $(td).html(
                                convertToDateDisplay(cellData)
                            );
                        }
                    },
                    //{
                    //    'targets': 6,
                    //    'createdCell': function (td, cellData, rowData, row, col) {
                    //        $(td).addClass("col-status text-nowrap");
                    //        $(td).html(
                    //            '<div style="width: 100%;" class="status ' + cellData.split('|')[0] + '">' +
                    //            cellData.split('|')[1] +
                    //            '</div>'
                    //        );
                    //    }
                    //}
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

        function openTicket(CallerID) {
            $("#hddCallerID_Criteria").val(CallerID);
            $("#btnLinkTransactionSearch").click();
        }
        
        ///////////////////////////////////////////
    </script>
</asp:Content>
