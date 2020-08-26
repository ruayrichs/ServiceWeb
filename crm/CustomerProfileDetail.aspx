<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/ServiceTicketMasterPage.master" AutoEventWireup="true" CodeBehind="CustomerProfileDetail.aspx.cs" Inherits="ServiceWeb.crm.CustomerProfileDetail" %>

<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEmployee.ascx" TagPrefix="uc1" TagName="AutoCompleteEmployee" %>
<%@ Register Src="~/crm/usercontrol/modalAddNewContact.ascx" TagPrefix="uc1" TagName="modalAddNewContact" %>
<%@ Register Src="~/UserControl/ChangeLogControl.ascx" TagPrefix="sna" TagName="ChangeLogControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            return;
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-customers").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
   </script>
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
                    background-image: url('images/user.png');
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
            background: #dc3545;
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

        .bg-cus-active {
            color: #fff !important;
            background: #26a991 !important;
            border-color: #26a991 !important;
        }
        .bg-cus-inactive {
            color: #fff !important;
            background: #c7c4bf !important;
            border-color: #c7c4bf !important;
        }
    </style>

    <div class="form-row">
        <div class="form-group col-xs-12 col-sm-5 col-md-4 col-lg-3">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCardCustomerDetail">
                <ContentTemplate>
                    <div class="card">
                        <div class="card-body bg-success customer-card">
                            <div class="">
                                <div class="customer-img">
                                    <div class="image-box"></div>
                                    <style>
                                        .flag-image{
                                            float:right;
                                        }
                                    </style>
                                    <% if (isCritical)
                                        { %>
                                    <div class="flag-image">
                                        <img src="../images/icon/flag-red-512.png" width="50" height="50"/>
                                    </div>
                                    <% } %>
                                </div>
                                <div class="customer-desc">
                                    <div class="one-line">
                                        <asp:Label Text="" runat="server" ID="lblCustomerCode" />
                                    </div>
                                    <div class="one-line">
                                        <asp:Label Text="" runat="server" ID="lblCustomerName" ToolTip="" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card">
                        <div class="card-header bg-success customer-card">
                            <i id="iconCustomerActiveStatus" title="สถานะการใช้งาน" style="border-radius: 50%; box-shadow: 0 4px 6px 0 rgba(0, 0, 0, 0.2), 0 6px 6px 0 rgba(0, 0, 0, 0.19);" class="fa fa-circle" runat="server"></i>
                            Client Detail
                            <div style="margin: 0 0 0 auto; display: inline-block; float: right;">
                                <i class="fa fa-pencil" onclick="showInitiativeModal('modal-EditCustomerDetail');" style="cursor: pointer;"></i>
                            </div>
                        </div>
                        <div class="card-body" style="padding: 7px 15px;">
                            <asp:Panel runat="server" CssClass="one-line lines" ToolTip="กลุ่มลูกค้า" ID="panelCustomerGroup">
                                <i class="fa fa-users fa-fw"></i>
                                <asp:Label Text="-" runat="server" ID="lblCustomerGroup" />
                            </asp:Panel>
                            <asp:Panel runat="server" CssClass="one-line lines" ToolTip="เซลล์ที่ดูแล" ID="panelCustomerSaleAdmin">
                                <i class="fa fa-user-secret fa-fw"></i>
                                <asp:Label Text="-" runat="server" ID="lblCustomerSaleAdmin" />
                            </asp:Panel>
                            <asp:Panel runat="server" CssClass="one-line lines" ToolTip="Tax ID" ID="panelCustomerTaxID">
                                <i class="fa fa-tag fa-fw"></i>
                                <asp:Label Text="-" runat="server" ID="lblCustomerTaxID" />
                            </asp:Panel>
                            <asp:Panel runat="server" CssClass="one-line lines" ToolTip="เบอร์โทร" ID="panelCustomerPhone">
                                <i class="fa fa-phone fa-fw"></i>
                                <asp:Label Text="-" runat="server" ID="lblCustomerPhone" />
                            </asp:Panel>
                            <asp:Panel runat="server" CssClass="one-line lines" ToolTip="เบอร์มือถือ" ID="panelCustomerPhoneMobile">
                                <i class="fa fa-mobile fa-fw"></i>
                                <asp:Label Text="-" runat="server" ID="lblCustomerPhoneMobile" />
                            </asp:Panel>
                            <asp:Panel runat="server" CssClass="one-line lines" ToolTip="อีเมล์" ID="panelCustomerEmail">
                                <i class="fa fa-envelope fa-fw"></i>
                                <asp:Label Text="-" runat="server" ID="lblCustomerEmail" />
                            </asp:Panel>
                            <asp:Panel runat="server" CssClass="one-line lines" ToolTip="ที่อยู่ลูกค้า" ID="panelCustomerAddress">
                                <i class="fa fa-home fa-fw"></i>
                                <asp:Label Text="-" runat="server" ID="lblCustomerAddress" />
                            </asp:Panel>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            
            <%--<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpticketAnalysis">
                <ContentTemplate>
                    <div class="card" id="pnlMyChartAnalytics">
                        <div class="card-header bg-success customer-card">
                            Ticket Analytics
                        </div>
                        <div class="card-body" style="padding: 7px 15px;">
                            <style>
                                #myChart {
                                    width: 100% !important;
                                    height: 250px !important;
                                }
                            </style>
                            <canvas id="myChart" class="myChart" height="250"></canvas>
                            <div class="d-none" id="pnlDataMyChart">
                            </div>
                            <asp:Repeater ID="rptMyChart" runat="server">
                                <ItemTemplate>
                                    <div>
                                        <div class="d-none myChart-open"><%# Eval("open") %></div>
                                        <div class="d-none myChart-close"><%# Eval("close") %></div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>--%>
            

            <br />
        </div>
        <div class="col-xs-12 col-sm-7 col-md-8 col-lg-9">
            <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
                <ul class="navbar-nav nav" role="tablist">
                    <li class="nav-item">
                        <a class="nav-link active" href="#panel-Dashboard" role="tab" data-toggle="tab">Dashboard <span class="sr-only">(current)</span></a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#panel-Contact" role="tab" data-toggle="tab">Contact</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#panel-changelog" role="tab" data-toggle="tab">Change Log</a>
                    </li>
                   <li class="nav-item">
						<a class="nav-link" href="#panel-Equipment" role="tab" data-toggle="tab">Configuration Items</a>
					</li>
                </ul>
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                    <ContentTemplate>
                        <asp:Button Text="" runat="server" CssClass="d-none" ID="btnExprotExcelContract"  OnClick="ExportExcelContactData_click" OnClientClick="AGLoading(true);"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div style="margin: 0 0 0 auto; display: inline-block;">
                    <button class="btn btn-outline-danger my-2 my-sm-0" type="button" onclick="exportexcel();">
                        <i class="fa fa-file-o"></i>&nbsp;&nbsp;Export Contact
                    </button>
                    
                    <a id="download-report-excel" class="hide" target="_blank"
        href="<%= Page.ResolveUrl("~/API/ExportExcelAPI.ashx") %>"></a>
                    <button class="btn btn-outline-success my-2 my-sm-0" type="button"
                        onclick="OpenModalAddNewContactRefCustomer('<%= CustomerCode %>','');">
                        <i class="fa fa-file-o"></i>&nbsp;&nbsp;Create Contact
                    </button>
                    <button class="btn btn-outline-warning my-2 my-sm-0" type="button"
                        onclick="showInitiativeModal('modal-CreateNewTicketReferent');">
                        <i class="fa fa-file-o"></i>&nbsp;&nbsp;Create Ticket
                    </button>
                </div>
            </nav>

            <div class="card" style="border-radius: 0;">
                <div class="card-body" style="min-height: calc(100vh - 140px);">
                    <div class="tab-content">
                        <div class="tab-pane in active" id="panel-Dashboard">
                            <div class="form-row">
                                <div class="form-group col-sm-6 col-md-3">
                                    <div class="card c-pointer panel-dashboard" data-id="panelListTicketOpen">
                                        <div class="text-primary card-body dashboard-card">
                                            <asp:Label Text="0" runat="server" ID="lblCountOpen" />
                                        </div>
                                        <div class="card-header bg-primary text-center">
                                            <b>Open</b>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-sm-6 col-md-3">
                                    <div class="card c-pointer panel-dashboard" data-id="panelListTicketDelay">
                                        <div class="text-danger card-body dashboard-card">
                                            <asp:Label Text="0" runat="server" ID="lblCountDelay" />
                                        </div>
                                        <div class="card-header bg-danger text-center">
                                            <b>Delay</b>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-sm-6 col-md-3">
                                    <div class="card c-pointer panel-dashboard" data-id="panelListTicketSuccess">
                                        <div class="text-success card-body dashboard-card">
                                            <asp:Label Text="0" runat="server" ID="lblCountSuccess" />
                                        </div>
                                        <div class="card-header bg-success text-center">
                                            <b>Finish</b>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-sm-6 col-md-3">
                                    <div class="card c-pointer panel-dashboard" data-id="panelListTicketAll">
                                        <div class="text-warning card-body dashboard-card">
                                            <asp:Label Text="0" runat="server" ID="lblCountAll" />
                                        </div>
                                        <div class="card-header bg-warning text-center">
                                            <b>All</b>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="panel-list-ticket" id="panelListTicketOpen" style="display: block;">
                                <!-- Open -->
                                <div class="card mb-3">
                                    <div class="card-header bg-primary text-white">
                                        <b>Open</b>
                                    </div>
                                    <div class="card-body">
                                        <div class="table-responsive">
                                            <table id="table-open" class="table table-bordered table-striped table-hover table-sm">
                                                <thead>
                                                    <tr>
                                                        <th class="text-nowrap"></th>
                                                        <th class="text-nowrap">Ticket No.</th>
                                                        <th class="text-nowrap">Date</th>
                                                        <th class="text-nowrap">Ticket Type</th>
                                                        <th class="text-nowrap">Subject</th>
                                                        <th class="text-nowrap">Priority</th>
                                                        <th class="text-nowrap">Status</th>
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
                                                                <td class="text-nowrap">
                                                                    <%# Eval("Subject") %>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("Priority")%>
                                                                </td>
                                                                <td class="col-status">
                                                                    <div class="status <%# Eval("StatusCode") %>">
                                                                        <%# Eval("StatusDesc") %>
                                                                    </div>
                                                                </td>
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
                            <div class="panel-list-ticket" id="panelListTicketDelay">
                                <!-- Delay -->
                                <div class="card mb-3">
                                    <div class="card-header bg-danger text-white">
                                        <b>Delay</b>
                                    </div>
                                    <div class="card-body">
                                        <div class="table-responsive">
                                            <table id="table-delay" class="table table-bordered table-striped table-hover table-sm">
                                                <thead>
                                                    <tr>
                                                        <th class="text-nowrap"></th>
                                                        <th class="text-nowrap">Ticket No.</th>
                                                        <th class="text-nowrap">Date</th>
                                                        <th class="text-nowrap">Ticket Type</th>
                                                        <th class="text-nowrap">Subject</th>
                                                        <th class="text-nowrap">Priority</th>
                                                        <th class="text-nowrap">Time Delay</th>
                                                        <th class="text-nowrap">Status</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptDelayRisk" runat="server">
                                                        <ItemTemplate>
                                                            <tr class="c-pointer" onclick="openTicket('<%# Eval("TicketNO") %>');">
                                                                <td class="text-nowrap text-center align-middle">
                                                                    <i class="fa fa-pencil-square fa-lg text-dark c-pointer" title="Edit" onclick="$(this).next().click();"></i>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("TicketNo4Display") %>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("Date") %>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# "#" + Eval("TicketType") %>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("Subject") %>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("Priority") %>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# ConvertToTime(Eval("EndDateTime").ToString()) %>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <% if (rptDelayRisk.Items.Count == 0)
                                                                { %>
                                                            <tr>
                                                                <td class="text-nowrap text-center" <%--colspan="6"--%>>ไม่พบข้อมูล
                                                                </td>
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
                            <div class="panel-list-ticket" id="panelListTicketSuccess">
                                <!-- Success -->
                                <div class="card mb-3">
                                    <div class="card-header bg-success text-white">
                                        <b>Finish</b>
                                    </div>
                                    <div class="card-body">
                                        <div class="table-responsive">
                                            <table id="table-success" class="table table-bordered table-striped table-hover table-sm">
                                                <thead>
                                                    <tr>
                                                        <th class="text-nowrap"></th>
                                                        <th class="text-nowrap">Ticket No.</th>
                                                        <th class="text-nowrap">Date</th>
                                                        <th class="text-nowrap">Ticket Type</th>
                                                        <th class="text-nowrap">Subject</th>
                                                        <th class="text-nowrap">Priority</th>
                                                        <th class="text-nowrap">Status</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptSuccessTask" runat="server">
                                                        <ItemTemplate>
                                                            <tr class="c-pointer" onclick="openTicket('<%# Eval("TicketNO") %>');">
                                                                <td class="text-nowrap text-center align-middle">
                                                                    <i class="fa fa-pencil-square fa-lg text-dark c-pointer" title="Edit" onclick="$(this).next().click();"></i>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("TicketNo4Display") %>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("Date") %>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# "#" + Eval("TicketType") %>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("Subject") %>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("Priority") %>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <% if (rptSuccessTask.Items.Count == 0)
                                                                { %>
                                                            <tr>
                                                                <td class="text-nowrap text-center" <%--colspan="5"--%>>ไม่พบข้อมูล
                                                                </td>
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
                            <div class="panel-list-ticket" id="panelListTicketAll">
                                <!-- All Ticket -->
                                <div class="card mb-3">
                                    <div class="card-header bg-warning text-white">
                                        <b>All</b>
                                    </div>
                                    <div class="card-body">
                                        <div class="table-responsive">
                                            <table id="table-all" class="table table-bordered table-striped table-hover table-sm">
                                                <thead>
                                                    <tr>
                                                        <th class="text-nowrap"></th>
                                                        <th class="text-nowrap">Ticket No.</th>
                                                        <th class="text-nowrap">Date</th>
                                                        <th class="text-nowrap">Ticket Type</th>
                                                        <th class="text-nowrap">Subject</th>
                                                        <th class="text-nowrap">Priority</th>
                                                        <th class="text-nowrap">Status</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptAllTask" runat="server">
                                                        <ItemTemplate>
                                                            <tr class="c-pointer" onclick="openTicket('<%# Eval("TicketNO") %>');">
                                                                <td class="text-nowrap text-center align-middle">
                                                                    <i class="fa fa-pencil-square fa-lg text-dark c-pointer" title="Edit" onclick="$(this).next().click();"></i>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("TicketNo4Display") %>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("Date") %>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# "#" + Eval("TicketType") %>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("Subject") %>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("Priority") %>
                                                                </td>
                                                                <td class="col-status">
                                                                    <div class="status <%# Eval("StatusCode") %>">
                                                                        <%# Eval("StatusDesc") %>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <% if (rptAllTask.Items.Count == 0)
                                                                { %>
                                                            <tr>
                                                                <td class="text-nowrap text-center" <%--colspan="6"--%>>ไม่พบข้อมูล
                                                                </td>
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

                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpListEquipment">
                                <ContentTemplate>
                                    <div runat="server" id="divDataJson" class="d-none"></div>
                                    <div class="row d-none" id="panel-dashbord-master">
                                        <div class="form-group col-sm-12 col-md-6">
                                            <div class="card" style="margin-bottom: 10px;">
                                                <div class="card-body bg-info customer-card">
                                                    <div class="">
                                                        <div class="customer-desc">
                                                            <div class="one-line">
                                                                <a class="AUTH_MODIFY" href="#" onclick="openEquipment('{#EquipmentCode-1#}')"
                                                                    title="{#Description-1#} ({#EquipmentCode-2#})" style="color: #fff;">
                                                                    {#Description-2#} ({#EquipmentCode-3#})
                                                                </a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="card-body" style="padding: 7px 15px;">
                                                    <div class="one-line lines" title="Class">
                                                        <b>Class : </b>
                                                        <span>{#EquipmentClass#}</span>
                                                    </div>
                                                    <div class="one-line lines" title="Begin Date">
                                                        <b>Begin Date : </b>
                                                        <span>{#BeginDate#}</span>
                                                    </div>
                                                    <div class="one-line lines" title="End Date">
                                                        <b>End Date : </b>
                                                        <span>{#EndDate#}</span>
                                                    </div>
                                                    <div>
                                                        <canvas id="equipment-chart-{#ItemIndex#}" class="equipment-chart-x {#EquipmentChartGroup#}" height="200"></canvas>
                                                        <div class="d-none equipment-chart-label"><%= TicketLabelAnalytic %></div>
                                                        <div class="d-none equipment-chart-value">{#DataTicketValueAnalytic-Equipment#}</div>
                                                        <div class="d-none equipment-chart-value-success">{#DataTicketValueAnalytic_Success-Equipment#}</div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    
                                        <script>
                                            var xCountBindList = 0;
                                            var xCountBindAround = 0;
                                            function bindListEquipment() {
                                                xCountBindAround++;

                                                var datas = JSON.parse($("#<%= divDataJson.ClientID %>").html());
                                                var target = $("#panel-list-equipment");
                                                var dashbordMaster = $("#panel-dashbord-master").html();

                                                var current = xCountBindList;
                                                for (var i = current; i < current + 10; i++) {
                                                    if (datas.length <= i) {
                                                        $("#panel-moreload").hide();
                                                        continue;
                                                    }

                                                    var data = datas[i];
                                                    var card = dashbordMaster
                                                        .replace('{#EquipmentChartGroup#}', "equipment-chart" + xCountBindAround)
                                                        .replace('{#ItemIndex#}', xCountBindList)
                                                        .replace('{#EquipmentCode-1#}', data.EquipmentCode)
                                                        .replace('{#EquipmentCode-2#}', data.EquipmentCode)
                                                        .replace('{#EquipmentCode-3#}', data.EquipmentCode)
                                                        .replace('{#Description-1#}', data.Description)
                                                        .replace('{#Description-2#}', data.Description)
                                                        .replace('{#EquipmentClass#}', data.EquipmentClass)
                                                        .replace('{#BeginDate#}', data.BeginDate)
                                                        .replace('{#EndDate#}', data.EndDate)
                                                        .replace('{#DataTicketValueAnalytic-Equipment#}', data.DataTicketValueAnalytic)
                                                        .replace('{#DataTicketValueAnalytic_Success-Equipment#}', data.DataTicketValueAnalytic_Success);
                                                    //console.log($(card));
                                                    target.append($(card));
                                                    xCountBindList++;
                                                    //length
                                                }

                                                setTimeout(function () {
                                                    bindEquipmentChartAnalytics_LazyLoad(xCountBindAround.toString());
                                                }, 500);
                                            }
                                            
                                            function bindEquipmentChartAnalytics_LazyLoad(groupChart) {
                                                $(".equipment-chart" + groupChart).each(function () {
                                                    var ctx = this;
                                                    var dataLabel = $(ctx).parent().find(".equipment-chart-label").html();
                                                    var dataValue = $(ctx).parent().find(".equipment-chart-value").html();
                                                    var dataValue_success = $(ctx).parent().find(".equipment-chart-value-success").html();

                                                    var chartvalue = JSON.parse(dataValue);
                                                    var maxvalue = Math.max(...chartvalue);
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

                                                    var myChart = new Chart(ctx, {
                                                        type: 'line',
                                                        data: {
                                                            labels: JSON.parse(dataLabel),
                                                            datasets: [{
                                                                label: "Open",
                                                                backgroundColor: "rgba(244,67,54,0.2)",
                                                                fillColor: "rgba(244,67,54,0.2)",
                                                                strokeColor: "rgba(244,67,54,1)",
                                                                pointColor: "rgba(244,67,54,1)",
                                                                pointStrokeColor: "#fff",
                                                                pointHighlightFill: "#fff",
                                                                pointHighlightStroke: "rgba(244,67,54,1)",
                                                                data: JSON.parse(dataValue)
                                                            }, {
                                                                label: "Close",
                                                                backgroundColor: "rgba(118,255,3,0.5)",
                                                                fillColor: "rgba(118,255,3,0.2)",
                                                                strokeColor: "rgba(118,255,3,1)",
                                                                pointColor: "rgba(118,255,3,1)",
                                                                pointStrokeColor: "#fff",
                                                                pointHighlightFill: "#fff",
                                                                pointHighlightStroke: "rgba(118,255,3,1)",
                                                                data: JSON.parse(dataValue_success)
                                                            }]
                                                        },
                                                        options: {
                                                            scales: {
                                                                yAxes: [{
                                                                    ticks: {
                                                                        beginAtZero: false,
                                                                        min: 0,
                                                                        stepSize: scale
                                                                    }
                                                                }]
                                                            },
                                                            elements: {
                                                                line: {
                                                                    tension: 0, // disables bezier curves
                                                                }
                                                            }
                                                        }
                                                    });
                                                });
                                            }
                                        </script>
                                    <div class="form-row d-none" id="panel-list-equipment">
                                        <asp:Repeater runat="server" ID="rptListEquipment">
                                            <ItemTemplate>
                                                <div class="form-group col-sm-12 col-md-6">
                                                    <div class="card" style="margin-bottom: 10px;">
                                                        <div class="card-body bg-info customer-card">
                                                            <div class="">
                                                                <div class="customer-desc">
                                                                    <div class="one-line">
                                                                        <a class="AUTH_MODIFY" href="#" onclick="openEquipment('<%# Eval("EquipmentCode") %>')"
                                                                            title="<%# Convert.ToString(Eval("Description")) %> (<%# Convert.ToString(Eval("EquipmentCode")) %>)" style="color: #fff;">
                                                                            <%# Convert.ToString(Eval("Description")) %> (<%# Convert.ToString(Eval("EquipmentCode")) %>)
                                                                        </a>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="card-body" style="padding: 7px 15px;">
                                                            <div class="one-line lines" title="Class">
                                                                <b>Class : </b>
                                                                <span><%# displayMember(Convert.ToString(Eval("EquipmentClass"))) %></span>
                                                            </div>
                                                            <div class="one-line lines" title="Begin Date">
                                                                <b>Begin Date : </b>
                                                                <span><%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("BeginDate").ToString()) %></span>
                                                            </div>
                                                            <div class="one-line lines" title="End Date">
                                                                <b>End Date : </b>
                                                                <span><%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("EndDate").ToString()) %></span>
                                                            </div>
                                                            <div>
                                                                <canvas id="equipment-chart-<%# Container.ItemIndex %>" class="equipment-chart" height="200"></canvas>
                                                                <div class="d-none equipment-chart-label"><%# TicketLabelAnalytic %></div>
                                                                <div class="d-none equipment-chart-value"><%# getDataTicketValueAnalytic(Convert.ToString(Eval("EquipmentCode"))) %></div>
                                                                <div class="d-none equipment-chart-value-success"><%# getDataTicketValueAnalytic_Success(Convert.ToString(Eval("EquipmentCode"))) %></div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <div class="form-row d-none" id="panel-moreload">
                                        <div class="form-group col-sm-12 alert alert-primary text-center" 
                                            style="cursor: pointer;" onclick="bindListEquipment();">
                                            โหลดรายการเพิ่ม.
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="tab-pane" id="panel-Contact">
                            <asp:UpdatePanel ID="udpContactData" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div class="form-row">
                                        <div class="container">
                                            <div class="float-right">
                                                <lable> Status:&nbsp;</lable>
                                                <select id="SelectStatus" class="form-control-sm" onchange="bindDataMyContactCard();" style="width: 150px; margin-bottom: .5rem;">
                                                    <option selected="selected" value="active">Active</option>                                    
                                                    <option value="inactive">Inactive</option>                                     
                                                    <option value="">All</option>
                                                </select>
                                            </div>
                                        </div>
                                        <asp:Repeater runat="server" ID="rptLisContact">
                                            <ItemTemplate>
                                                <div class="col-md-12">
                                                    <div class="ContactCard-<%# Eval("ActiveStatus") %>">
                                                        <div class="card" style="margin-bottom: 10px;">
                                                            <div class="card-body <%# Convert.ToBoolean(Eval("ActiveStatus")) ? "bg-cus-active" : "bg-cus-inactive" %> customer-card">
                                                                <div class="">
                                                                    <div class="customer-img">
                                                                        <div class="image-box"></div>
                                                                    </div>
                                                                    <div class="customer-desc">
                                                                        <div style="margin: 0 0 0 auto; display: inline-block; float: right;">
                                                                            <i class="fa fa-pencil" onclick="OpenModalAddNewContactRefCustomer('<%# Eval("BPCODE") %>','<%# Eval("ITEMNO") %>');" style="cursor: pointer;"></i>
                                                                        </div>
                                                                        <div class="one-line">
                                                                            <span title="Name : <%# displayMember(Convert.ToString(Eval("NAME1"))) %>">Name : <%# displayMember(Convert.ToString(Eval("NAME1"))) %>
                                                                            </span>
                                                                        </div>
                                                                        <div class="one-line">
                                                                            <span title="Service : <%# displayMember(Convert.ToString(Eval("NickName"))) %>">Service : <%# displayMember(Convert.ToString(Eval("NickName"))) %>
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="card-body" style="padding: 7px 15px;">
                                                                <div class="one-line lines" title="ตำแหน่ง">
                                                                    <i class="fa fa-sitemap fa-fw"></i>
                                                                    <span><%# displayMember(Convert.ToString(Eval("POSITION"))) %></span>
                                                                </div>
                                                                <div class="one-line lines" title="เบอร์โทร">
                                                                    <i class="fa fa-phone fa-fw"></i>
                                                                    <a style="color: #000; text-decoration: none;"
                                                                        href="<%# String.IsNullOrEmpty(Convert.ToString(Eval("phone")))? "#" : "tel:"+Convert.ToString(Eval("phone")).Trim() %>"
                                                                        onclick="disableAutoLoading(true);">
                                                                        <%# displayMember(Convert.ToString(Eval("phone"))) %>

                                                                    </a>
                                                                </div>
                                                                <div class="one-line lines" title="อีเมล์">
                                                                    <i class="fa fa-envelope fa-fw"></i>
                                                                    <a style="color: #000; text-decoration: none;"
                                                                        href="<%# String.IsNullOrEmpty(Convert.ToString(Eval("email")).Trim())? "#" : "mailto:"+Convert.ToString(Eval("email")).Trim() %>"
                                                                        onclick="disableAutoLoading(true);">
                                                                        <%# displayMember(Convert.ToString(Eval("email"))) %>
                                                                    </a>
                                                                </div>
                                                                <div class="one-line lines" title="Authorization Contact">
                                                                    <i class="fa fa-star fa-fw"></i>
                                                                    <a style="color: #000; text-decoration: none;"
                                                                        href="#">
                                                                        <%# displayMember(Convert.ToString(Eval("AUTH_CONTACT_NAME"))) %>
                                                                    </a>
                                                                </div>
                                                                <div class="one-line lines" title="หมายเหตุ">
                                                                    <i class="fa fa-asterisk fa-fw"></i>
                                                                    <a style="color: #000; text-decoration: none;"
                                                                        href="#"
                                                                        onclick="disableAutoLoading(true);">
                                                                        <%# displayMember(Convert.ToString(Eval("Remark"))) %>
                                                                    </a>
                                                                </div>
                                                                <div class="one-line lines <%# Convert.ToBoolean(Eval("ActiveStatus")) ? "" : " d-none " %>" title="สถานะ">
                                                                    <i class="fa fa-check-square-o fa-fw"></i>
                                                                    <a style="color: #000; text-decoration: none;"
                                                                        href="#"
                                                                        onclick="disableAutoLoading(true);">
                                                                        <%# Convert.ToBoolean(Eval("ActiveStatus")) ? "Active" : "Inactive" %>
                                                                    </a>
                                                                </div>
                                                                <div class="<%# !Convert.ToBoolean(Eval("ActiveStatus")) ? "" : " d-none " %>" title="สถานะ" style="position: relative;">
                                                                    <div style="position: absolute; transform: rotate(-30deg); font-size: 70px; bottom: 45px; right: 0; color: rgba(255, 0, 0, 1) !important;">
                                                                        <i class="fa fa-times-circle-o"></i>
                                                                        <span style="color: rgba(255, 0, 0, 1) !important;">
                                                                            Inactive
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <div class="tab-pane" id="panel-changelog">
                            <sna:ChangeLogControl id="CustomerChangeLog" runat="server" />
                        </div>
                        <div class="tab-pane" id="panel-Equipment">
                             <div class="table-responsive">
                                        <div id="tableArea">
                                            <asp:UpdatePanel ID="udpnListCIItems" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="table-responsive" style="width: 100%;">
                                                        <table id="tableListCIItems" style="width: 100%;"
                                                            class="table table-bordered table-striped table-hover table-sm">
                                                            <thead>
                                                                <tr>                                                             
                                                                    <th class="text-nowrap">CI Code</th>
                                                                    <th>CI Name</th>
                                                                    <th class="text-nowrap">CI Type</th>
                                                                    <th class="text-nowrap">CI Class</th>
                                                                    <th class="text-nowrap">CI Category</th>
                                                                    <th class="text-nowrap">Status</th>
                                                                    <th class="text-nowrap">Owner Service</th>
                                                               
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:Repeater ID="rptListCIItems" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="table-hover">
                                                                            <td class="text-nowrap"><%# Eval("EquipmentCode") %></td>
                                                                            <td class="text-nowrap"><%# Eval("Description") %></td>
                                                                            <td class="text-nowrap"><%# Eval("EquipmentTypeName") %></td>
                                                                            <td class="text-nowrap"><%# Eval("EquipmentClassName") %></td>
                                                                            <td class="text-nowrap"><%# Eval("CategoryDesc") %></td>
                                                                            <td class="text-nowrap"><%# Eval("StatusDesc") %></td>
                                                                            <td class="text-nowrap"><%# Eval("OwnerGroupName") %></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </tbody>
                                                        </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
						</div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="initiative-model-control-slide-panel" id="modal-CreateNewTicketReferent">
        <div class="initiative-model-control-body-content z-depth-3">
            <div>
                <div class="initiative-model-control-header">
                    <div class="mat-box-initaive-control">
                        <div class="pull-right">
                            <i class="fa fa-close hand" onclick="closeInitiativeModal('modal-CreateNewTicketReferent');"></i>
                        </div>
                        <div class="one-line">
                            <label class="text-warning">
                                Create Ticket.
                            </label>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-control-contant">
                    <div class="panel-body-initiative-master">
                        <div class="panel-content-initiative-master">
                            <div class="mat-box-initaive-control tab-initiative-control">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpPanelCreatedNew">
                                    <ContentTemplate>
                                        <div class="form-row">
                                            <div class="form-group col-md-4 col-sm-6 col-xs-12">
                                                <label>Client</label>
                                                <asp:Label Text="" CssClass="form-control form-control-sm required" Enabled="false"
                                                    runat="server" ID="lblCustomerDetail" />
                                            </div>
                                            <div class="form-group col-md-4 col-sm-6 col-xs-12">
                                                <label>Configuration Item</label>
                                                <asp:DropDownList CssClass="form-control form-control-sm required"
                                                    runat="server" ID="ddlEquipment">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group col-md-4 col-sm-6 col-xs-12">
                                                <label>Ticket Type</label>
                                                <asp:DropDownList ID="_ddl_sctype" runat="server" class="form-control form-control-sm required"></asp:DropDownList>
                                            </div>
                                            <div class="form-group col-md-4 col-sm-6 col-xs-12 d-none">
                                                <label>Fiscal Year</label>
                                                <input id="_txt_year" type="text" class="form-control form-control-sm required" runat="server" clientidmode="Static"
                                                    onkeypress='return event.charCode >= 48 && event.charCode <= 57' />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:HiddenField runat="server" ID="hddEquepmentCodeRef" />
                                        <asp:Button Text="" runat="server" CssClass="d-none" ID="btnCreateNewTicketRef"
                                            OnClick="btnCreateNewTicketRef_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-bottom">
                    <div class="text-right">
                        <span class="water-button" onclick="newTicketRefModalClick(this)"><i class="fa fa-file-o"></i>&nbsp;Create Ticket</span>
                        <a class="water-button" onclick="closeInitiativeModal('modal-CreateNewTicketReferent');"><i class="fa fa-close"></i>&nbsp;Close</a>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="initiative-model-control-slide-panel" id="modal-EditCustomerDetail">
        <div class="initiative-model-control-body-content z-depth-3">
            <div>
                <div class="initiative-model-control-header">
                    <div class="mat-box-initaive-control">
                        <div class="pull-right">
                            <i class="fa fa-close hand" onclick="closeInitiativeModal('modal-EditCustomerDetail');"></i>
                        </div>
                        <div class="one-line">
                            <label class="text-warning">
                                แก้ไขรายละเอียด
                            </label>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-control-contant">
                    <div class="panel-body-initiative-master">
                        <div class="panel-content-initiative-master">
                            <div class="mat-box-initaive-control tab-initiative-control">
                                <asp:HiddenField runat="server" ID="hddJsonAddress" ClientIDMode="Static" />
                                <asp:HiddenField runat="server" ID="hddAddressCodeEdit" ClientIDMode="Static" />
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                                    <ContentTemplate>
                                        <div class="form-row">
                                            <div class="col-lg-6">
                                                <div class="card border-primary" style="margin-bottom: 10px;">
                                                    <div class="card-body card-body-sm">
                                                        <div class="form-row">
                                                            <div class="form-group col-md-6 col-sm-6 col-xs-12">
                                                                <label>Client Group</label>
                                                                <asp:DropDownList Enabled="false" CssClass="form-control form-control-sm"
                                                                    runat="server" ID="_ddl_CD_CustomerGroup">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-6 col-sm-6 col-xs-12">
                                                                <label>Client Code</label>
                                                                <asp:TextBox ID="_txt_CD_CustomerCode" Enabled="false" CssClass="form-control form-control-sm" placeholder="Text" runat="server" />
                                                            </div>
                                                            <div class="form-group col-md-12 col-sm-12 col-xs-12">
                                                                <label>Client Name</label>
                                                                <asp:TextBox Enabled="false" Text="" CssClass="form-control form-control-sm required" placeholder="Text TH" runat="server" ID="_txt_CD_CustomerName" />

                                                            </div>
                                                            <div class="form-group col-md-12 col-sm-12 col-xs-12">
                                                                <label>Foreign Name</label>
                                                                <%--<label>Responsible Organization</label>--%>
                                                                <asp:TextBox Enabled="false" Text="" CssClass="form-control form-control-sm" placeholder="Text EN" runat="server" ID="_txt_CD_ForeignName" />
                                                            </div>
                                                            <div class="form-group col-md-6 col-sm-12 col-xs-12">
                                                                <label>Sales Employee</label>
                                                                <uc1:AutoCompleteEmployee runat="server" id="AutoCompleteEmployee" placeholder="Text" CssClass="form-control form-control-sm"  Enabled="false" />
                                                            </div>
                                                            <div class="form-group col-md-6 col-sm-12 col-xs-12">
                                                                <label>Owner Service</label>
                                                                <asp:DropDownList ID="ddlOwnerService" CssClass="form-control form-control-sm" runat="server">
                                                                </asp:DropDownList>
                                                            </div>
                                                             <%-- <div class="form-group col-md-4 col-sm-12 col-xs-12">
                                                                <label>Accountability</label>
                                                                <asp:DropDownList ID="ddlAccountability" CssClass="form-control form-control-sm" runat="server">
                                                                </asp:DropDownList>
                                                            </div> --%>
                                                            
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6">
                                                <div class="card border-default" style="margin-bottom: 10px;">
                                                    <div class="card-body card-body-sm">
                                                        <div class="form-row">
                                                            <div class="form-group col-md-2 col-sm-6 col-xs-12">
                                                                <label style="display:block;">Active</label>
                                                                <label class="switch" style="zoom: 0.7; display:block; opacity: 0.5;">
                                                                    <input id="chkCustomerActive" type="checkbox" class="danger" runat="server" />
                                                                    <span class="slider"></span>
                                                                </label>
                                                            </div>
                                                            <div class="form-group col-md-2 col-sm-6 col-xs-12">
                                                                <label style="display:block;">Critical</label>
                                                                <label class="switch" style="zoom: 0.7; display:block;">
                                                                    <input id="chkCriticalCustomer" type="checkbox" class="danger" runat="server" value="CRITICAL" />
                                                                    <span class="slider"></span>
                                                                </label>
                                                            </div>
                                                            <div class="form-group col-md-8 col-sm-12 col-xs-12">
                                                                <label>Responsible Organization</label>
                                                                <input id="_txt_CD_ResponsibleOrganization" placeholder="Text" type="text" 
                                                                    class="form-control form-control-sm" runat="server" clientidmode="Static" />
                                                            </div>
                                                            <div class="form-group col-md-12 col-sm-12 col-xs-12">
                                                                <label>Tax ID</label>
                                                                <input id="_txt_CD_CustomerTaxID" placeholder="Text" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static" />
                                                            </div>
                                                            <%--===For ITG===--%>
                                                            <div class="form-group col-md-6 col-sm-6 col-xs-6 d-none">
                                                                <label>TID</label>
                                                                <input id="_txt_CD_CustomerTID" placeholder="Text" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static" />
                                                            </div>
                                                            <%--===-------===--%>
                                                            <div class="form-group col-md-6 col-sm-6 col-xs-6">
                                                                <label>Work Phone</label>
                                                                <input id="_txt_CD_CustomerPhone" placeholder="Text" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static" />
                                                            </div>
                                                            <div class="form-group col-md-6 col-sm-6 col-xs-6">
                                                                <label>Moblie Phone</label>
                                                                <input id="_txt_CD_CustomerPhoneMoblie" placeholder="Number" onkeypress="return isNumberKey(event);" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static" />
                                                            </div>
                                                            <div class="form-group col-md-12 col-sm-12 col-xs-12">
                                                                <label>E-Mail</label>
                                                                <input id="_txt_CD_CustomerEmail" placeholder="Text EN" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="col-lg-12">
                                                <div class="card border-default" style="margin-bottom: 10px;">
                                                    <div class="card-body card-body-sm">
                                                        <div class="form-row">
                                                            <asp:Repeater runat="server" ID="rptAddAddress">
                                                                <ItemTemplate>
                                                                    <div class="form-group col-md-3 col-sm-6 col-xs-12 textbox-address-input">
                                                                        <label>
                                                                            <%# Eval("Description") %>
                                                                        </label>
                                                                        <input type="text" <%= IsAllFeature ? "" : "disabled='disabled'" %> class="form-control txt-add-address form-control-sm " data-propertycode="<%# Eval("PropertyCode") %>"
                                                                            data-description="<%# Eval("Description") %>" value="<%# Eval("PropertyValue") %>" placeholder='<%# GetPlaceHolderAddress(Eval("PropertyCode")) %>' />
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:HiddenField runat="server" ID="HiddenField1" />
                                        <asp:Button Text="" runat="server" CssClass="d-none AUTH_MODIFY" ID="btnUpdateCustomerDetail"
                                            OnClick="btnUpdateCustomerDetail_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-bottom">
                    <div class="text-right">
                        <span class="water-button AUTH_MODIFY" onclick="updateCustomerDetailRefModalClick(this)"><i class="fa fa-file-o"></i>&nbsp;Save</span>
                        <a class="water-button" onclick="closeInitiativeModal('modal-EditCustomerDetail');"><i class="fa fa-close"></i>&nbsp;Close</a>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <uc1:modalAddNewContact runat="server" id="modalAddNewContact" />


    <div class="d-none">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btnLinkTransactionSearch" runat="server" ClientIDMode="Static"
                    CssClass="d-none" OnClick="btnLinkTransactionSearch_Click" OnClientClick="AGLoading(true);" />
                <asp:HiddenField runat="server" ID="hddCallerID_Criteria" Value="" ClientIDMode="Static" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--=========click to CI==========--%>
    <div class="d-none">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hddEquipmentCode" />
                <asp:HiddenField runat="server" ID="hddPage_Mode" />
                <asp:Button Text="" runat="server" ID="btnOpenDetailEquipment"
                    OnClick="btnOpenDetailEquipment_Click" OnClientClick="AGLoading(true);" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    
    <script>
        function openEquipment_PageCriteria(EquipmentCode, Mode) {
            //inactiveRequireField();
            $("#<%= hddEquipmentCode.ClientID %>").val(EquipmentCode);
            $("#<%= hddPage_Mode.ClientID %>").val(Mode);
            $("#<%= btnOpenDetailEquipment.ClientID %>").click();
        }
    </script>
    <%--================================--%>

    
    <script>
        $(document).ready(function() {
            getCustomerDashboard();
            bindChartAnalytics();
            getCustomerCustomerMappingCI();
        });
        function getCustomerDashboard() 
        {
            $("[data-id='panelListTicketOpen']").AGWhiteLoading(true, 'Loading...');
            $("[data-id='panelListTicketDelay']").AGWhiteLoading(true, 'Loading...');
            $("[data-id='panelListTicketSuccess']").AGWhiteLoading(true, 'Loading...');
            $("[data-id='panelListTicketAll']").AGWhiteLoading(true, 'Loading...');

            getTicketListByCustomer();
                                    
            <%--var postData = {
            };

            $.ajax({
                type: "POST",
                url: servictWebDomainName + "API/v1/CustomerDashboardAPI.aspx?CustomerCode=<%= getCustomerCode %>",
                data: postData,
                success: function (datas) {
                    objDatasMyTicket = JSON.parse(datas);
                    console.log(objDatasMyTicket);

                    $("#<%= lblCountOpen.ClientID %>").html(objDatasMyTicket.CountTicketOpen);
                    $("#<%= lblCountDelay.ClientID %>").html(objDatasMyTicket.CountTicketDelay);
                    $("#<%= lblCountSuccess.ClientID %>").html(objDatasMyTicket.CountTicketFinish);
                    $("#<%= lblCountAll.ClientID %>").html(objDatasMyTicket.CountTicketAll);
                                            
                    $("[data-id='panelListTicketOpen']").AGWhiteLoading(false, 'Loading...');
                    $("[data-id='panelListTicketDelay']").AGWhiteLoading(false, 'Loading...');
                    $("[data-id='panelListTicketSuccess']").AGWhiteLoading(false, 'Loading...');
                    $("[data-id='panelListTicketAll']").AGWhiteLoading(false, 'Loading...');

                    getTicketListByCustomer();
                }
            });--%>
        }

        function getTicketListByCustomer() 
        {
            $("#table-open").AGWhiteLoading(true, 'Loading...');
            $("#table-delay").AGWhiteLoading(true, 'Loading...');
            $("#table-success").AGWhiteLoading(true, 'Loading...');
            $("#table-all").AGWhiteLoading(true, 'Loading...');
            var postData = {
            };

            $.ajax({
                type: "POST",
                url: servictWebDomainName + "API/v1/TicketListByCustomerAPI.aspx?CustomerCode=<%= getCustomerCode %>",
                data: postData,
                success: function (datas) {
                    objDatasMyTicket = JSON.parse(datas);
                    console.log(objDatasMyTicket);

                    $("#<%= lblCountOpen.ClientID %>").html(objDatasMyTicket.OpenTask.length);
                    $("#<%= lblCountDelay.ClientID %>").html(objDatasMyTicket.DelayRisk.length);
                    $("#<%= lblCountSuccess.ClientID %>").html(objDatasMyTicket.SuccessTask.length);
                    $("#<%= lblCountAll.ClientID %>").html(objDatasMyTicket.AllTask.length);

                    $("[data-id='panelListTicketOpen']").AGWhiteLoading(false, 'Loading...');
                    $("[data-id='panelListTicketDelay']").AGWhiteLoading(false, 'Loading...');
                    $("[data-id='panelListTicketSuccess']").AGWhiteLoading(false, 'Loading...');
                    $("[data-id='panelListTicketAll']").AGWhiteLoading(false, 'Loading...');

                    //table-open
                                            
                    myTaskDataTable = bindListTicket_1($("#table-open"), objDatasMyTicket.OpenTask);
                    $("#table-open").AGWhiteLoading(false, 'Loading...');
                                            
                    myTaskDataTable = bindListTicket_2($("#table-delay"), objDatasMyTicket.DelayRisk);
                    $("#table-delay").AGWhiteLoading(false, 'Loading...');
                                            
                    myTaskDataTable = bindListTicket_3($("#table-success"), objDatasMyTicket.SuccessTask);
                    $("#table-success").AGWhiteLoading(false, 'Loading...');
                                            
                    myTaskDataTable = bindListTicket_1($("#table-all"), objDatasMyTicket.AllTask);
                    $("#table-all").AGWhiteLoading(false, 'Loading...');

                    //$("#table-delay,#table-open,#table-success").dataTable({
                    //    columnDefs: [{
                    //        "orderable": false,
                    //        "targets": [0]
                    //    }],
                    //    "order": [[2, "asc"]],
                    //});

                    //$("#table-all").dataTable({
                    //    columnDefs: [{
                    //        "orderable": false,
                    //        "targets": [0]
                    //    }],
                    //    "order": [[2, "asc"]]
                    //});
                }
            });
        }
                                
        function bindListTicket_1(objTarget, datas) 
        {
            
            var datasTicket = [];
            for (var i = 0 ; i < datas.length ; i++) {
                var ticket = datas[i];

                datasTicket.push([
                    ticket.TicketNO,
                    ticket.TicketNo4Display,
                    ticket.StartDateTime,
                    "#" + ticket.TicketType,
                    ticket.Subject,
                    ticket.Priority,
                    ticket.StatusCode + "|" + ticket.StatusDesc
                ]);
            }

            var dataTableResult = objTarget.dataTable({
                data: datasTicket,
                deferRender: true,
                "order": [[2, "asc"]],
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
                        'targets': [1, 3, 5],
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
                    {
                        'targets': 6,
                        'createdCell': function (td, cellData, rowData, row, col) {
                            $(td).addClass("col-status text-nowrap");
                            $(td).html(
                                '<div style="width: 100%;" class="status '+ cellData.split('|')[0]+'">' +
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
                                
        function bindListTicket_2(objTarget, datas) {
            
            var datasTicket = [];
            for (var i = 0 ; i < datas.length ; i++) {
                var ticket = datas[i];

                datasTicket.push([
                    ticket.TicketNO,
                    ticket.TicketNo4Display,
                    ticket.StartDateTime,
                    "#" + ticket.TicketType,
                    ticket.Subject,
                    ticket.Priority,
                    ticket.EndDateTime,
                    ticket.StatusCode + "|" + ticket.StatusDesc
                ]);
            }

            var dataTableResult = objTarget.dataTable({
                data: datasTicket,
                deferRender: true,
                "order": [[2, "asc"]],
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
                        'targets': [1, 3, 5],
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
                    {
                        'targets': [6],
                        'createdCell': function (td, cellData, rowData, row, col) {
                            // calculation of no. of days between two date  

                            // To set two dates to two variables 
                            var date1 = new Date(cellData.replace(
                                /^(\d{4})(\d\d)(\d\d)(\d\d)(\d\d)(\d\d)$/,
                                '$4:$5:$6 $2/$3/$1'
                            ));
                            var date2 = new Date();

                            // To calculate the time difference of two dates 
                            var Difference_In_Time = date2.getTime() - date1.getTime();

                            // To calculate the no. of days between two dates 
                            var Difference_In_Days = Difference_In_Time / (1000 * 3600 * 24);

                            $(td).addClass("text-truncate text-nowrap");
                            $(td).html(
                                'End Date : ' +
                                convertToDateDisplay(cellData)
                                + '</br>Delay <label style="color:red">' + Difference_In_Days.toFixed(0) + '</label> day'
                            );
                        }
                    },
                    {
                        'targets': 7,
                        'createdCell': function (td, cellData, rowData, row, col) {
                            $(td).addClass("col-status text-nowrap");
                            $(td).html(
                                '<div style="width: 100%;" class="status ' + cellData.split('|')[0] + '">' +
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
                                
        function bindListTicket_3(objTarget, datas) {
            
            var datasTicket = [];
            for (var i = 0 ; i < datas.length ; i++) {
                var ticket = datas[i];

                datasTicket.push([
                    ticket.TicketNO,
                    ticket.TicketNo4Display,
                    ticket.StartDateTime,
                    "#" + ticket.TicketType,
                    ticket.Subject,
                    ticket.Priority,
                    ticket.StatusCode + "|" + ticket.StatusDesc
                ]);
            }

            var dataTableResult = objTarget.dataTable({
                data: datasTicket,
                deferRender: true,
                "order": [[2, "asc"]],
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
                        'targets': [1, 3, 5],
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
                    {
                        'targets': 6,
                        'createdCell': function (td, cellData, rowData, row, col) {
                            $(td).addClass("col-status text-nowrap");
                            $(td).html(
                                '<div style="width: 100%;" class="status ' + cellData.split('|')[0] + '">' +
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


        
        function getCustomerCustomerMappingCI() 
        {
            $("#tableListCIItems").AGWhiteLoading(true, 'Loading...');
                                    
            var postData = {
            };

            $.ajax({
                type: "POST",
                url: servictWebDomainName + "API/v1/CustomerMappingCIAPI.aspx?CustomerCode=<%= getCustomerCode %>",
                data: postData,
                success: function (datas) {
                    objDatasCI = JSON.parse(datas);

                    myTaskDataTable = bindListCI($("#tableListCIItems"), objDatasCI);
                    $("#tableListCIItems").AGWhiteLoading(false, 'Loading...');
                }
            });
        }

        function bindListCI(objTarget, datas) {
            var datasCI = [];
            for (var i = 0 ; i < datas.length ; i++) {
                var dataCI = datas[i];

                datasCI.push([
                    dataCI.EquipmentCode,
                    dataCI.Description,
                    dataCI.EquipmentTypeName,
                    dataCI.EquipmentClassName,
                    dataCI.CategoryDesc,
                    dataCI.StatusDesc,
                    dataCI.OwnerGroupName
                ]);
            }

            var dataTableResult = objTarget.dataTable({
                data: datasCI,
                deferRender: true,
                "order": [[2, "asc"]],
                'columnDefs': [
                    {
                        'targets': [0],
                        'createdCell': function (td, cellData, rowData, row, col) {
                            $(td).addClass("text-truncate text-nowrap");
                            $(td).parent().bind("click", function () {
                                openEquipment_PageCriteria(cellData, 'Edit')
                            });

                            $(td).parent().css({
                                cursor: "pointer"
                            });
                        }
                    },
                    {
                        'targets': [1, 2, 3, 4, 5, 6],
                        'createdCell': function (td, cellData, rowData, row, col) {
                            $(td).addClass("text-truncate text-nowrap").bind("click", function () {
                                //openPopUpWindows(cellData);
                            });
                        }
                    }
                ]
            });
            
            return dataTableResult;
        }
    </script>


    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.1/Chart.min.js"></script>
    <script>
        function openEquipment(EquipmentCode) {
            inactiveRequireField();
            $("#<%= hddEquipmentCode.ClientID %>").val(EquipmentCode);
            $("#<%= btnOpenDetailEquipment.ClientID %>").click();
        }


        function openTicket(CallerID) {
            $("#hddCallerID_Criteria").val(CallerID);
            $("#btnLinkTransactionSearch").click();
        }

        var slideTimeoutListTicket = null;

        $(document).ready(function () {
            //bindingDataTable();
            //bindChartAnalytics();
            $('.ContactCard-FALSE').hide();

            $(".panel-dashboard").bind("click", function () {
                var panelID, timeDelay, panelDisplay;
                panelID = $(this).attr("data-id");
                panelDisplay = $(".panel-list-ticket:visible");
                if (panelDisplay.length > 0) {
                    panelDisplay.slideUp();
                    timeDelay = 501;
                } else {
                    timeDelay = 0;
                }

                if (panelDisplay.attr("id") != panelID) {
                    clearTimeout(slideTimeoutListTicket);
                    slideTimeoutListTicket = setTimeout(function () {
                        $("#" + panelID).slideDown();
                        slideTimeoutListTicket == null;
                    }, timeDelay);
                }
            });
        });

        function bindingDataTable() {
            $("#table-delay,#table-open,#table-success").dataTable({
                columnDefs: [{
                    "orderable": false,
                    "targets": [0]
                }],
                "order": [[2, "asc"]],
            });

            $("#table-all").dataTable({
                columnDefs: [{
                    "orderable": false,
                    "targets": [0]
                }],
                "order": [[2, "asc"]]
            });
        }

        function bindEquipmentChartAnalytics(groupChart) {
            if (!groupChart) {
                groupChart = "";
            }
            $(".equipment-chart" + groupChart).each(function () {
                var ctx = this;
                var dataLabel = $(ctx).parent().find(".equipment-chart-label").html();
                var dataValue = $(ctx).parent().find(".equipment-chart-value").html();
                var dataValue_success = $(ctx).parent().find(".equipment-chart-value-success").html();

                var chartvalue = JSON.parse(dataValue);
                var maxvalue = Math.max(...chartvalue);
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

                var myChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: JSON.parse(dataLabel),
                        datasets: [{
                            label: "Open",
                            backgroundColor: "rgba(244,67,54,0.2)",
                            fillColor: "rgba(244,67,54,0.2)",
                            strokeColor: "rgba(244,67,54,1)",
                            pointColor: "rgba(244,67,54,1)",
                            pointStrokeColor: "#fff",
                            pointHighlightFill: "#fff",
                            pointHighlightStroke: "rgba(244,67,54,1)",
                            data: JSON.parse(dataValue)
                        }, {
                            label: "Close",
                            backgroundColor: "rgba(118,255,3,0.5)",
                            fillColor: "rgba(118,255,3,0.2)",
                            strokeColor: "rgba(118,255,3,1)",
                            pointColor: "rgba(118,255,3,1)",
                            pointStrokeColor: "#fff",
                            pointHighlightFill: "#fff",
                            pointHighlightStroke: "rgba(118,255,3,1)",
                            data: JSON.parse(dataValue_success)
                        }]
                    },
                    options: {
                        scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero: false,
                                    min: 0,
                                    stepSize: scale
                                }
                            }]
                        },
                        elements: {
                            line: {
                                tension: 0, // disables bezier curves
                            }
                        }
                    }
                });
            });
        }

        function bindChartAnalytics() {
            $("#pnlMyChartAnalytics").AGWhiteLoading(true, 'Loading...');
                         
            var postData = {
            };

            $.ajax({
                type: "POST",
                url: servictWebDomainName + "API/v1/CustomerTicketAnalyticsAPI.aspx?CustomerCode=<%= getCustomerCode %>",
                data: postData,
                success: function (datas) {
                    var objDatas = JSON.parse(datas);
                    //console.log(objDatas);

                    var target = $("#pnlDataMyChart");
                    for (var i = 0; i < objDatas.length; i++) {
                        var pnlItem = $("<div>");

                        var pnlOpen = $("<div>", { 
                            class: "d-none myChart-open",
                            html: objDatas[i].open
                        });
                        var pnlClose = $("<div>", { 
                            class: "d-none myChart-close",
                            html: objDatas[i].close
                        });

                        pnlItem.append(pnlOpen, pnlClose);
                        target.append(pnlItem);
                    }

                    initChartTicketAnalytics();
                    $("#pnlMyChartAnalytics").AGWhiteLoading(false, 'Loading...');
                }
            });

            function initChartTicketAnalytics() {
                var d = new Date();
                var weekday = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
                var n = weekday[d.getDay() + 7];
                var ctx = document.getElementById("myChart");
                //console.log(ctx);
                var openvalue = $(ctx).parent().find(".myChart-open").html();
                var closevalue = $(ctx).parent().find(".myChart-close").html();
                openvalue = JSON.parse(openvalue);
                closevalue = JSON.parse(closevalue);
                var maxvalue = Math.max(...openvalue);
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
                //console.log(openvalue);
                //console.log(closevalue);
                var myChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: [weekday[d.getDay() + 1], weekday[d.getDay() + 2], weekday[d.getDay() + 3], weekday[d.getDay() + 4], weekday[d.getDay() + 5], weekday[d.getDay() + 6], weekday[d.getDay() + 7]],
                        datasets: [{
                            label: "Open",
                            backgroundColor: "rgba(244,67,54,0.2)",
                            fillColor: "rgba(244,67,54,0.2)",
                            strokeColor: "rgba(244,67,54,1)",
                            pointColor: "rgba(244,67,54,1)",
                            pointStrokeColor: "#fff",
                            pointHighlightFill: "#fff",
                            pointHighlightStroke: "rgba(244,67,54,1)",
                            data: openvalue
                        }, {
                            label: "Close",
                            backgroundColor: "rgba(118,255,3,0.5)",
                            fillColor: "rgba(118,255,3,0.2)",
                            strokeColor: "rgba(118,255,3,1)",
                            pointColor: "rgba(118,255,3,1)",
                            pointStrokeColor: "#fff",
                            pointHighlightFill: "#fff",
                            pointHighlightStroke: "rgba(118,255,3,1)",
                            data: closevalue
                        }]
                    },
                    options: {
                        scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero: false,
                                    min: 0,
                                    stepSize: scale
                                }
                            }]
                        },
                        elements: {
                            line: {
                                tension: 0, // disables bezier curves
                            }
                        }
                    }
                });
            }
        }

        function newTicketRefModalClick(sender) {
            var msg = '';
            if ($("#<%= ddlEquipment.ClientID %>").val() != '') {
                           msg = "Do you want to create new ticket with equipment " + $("#<%= ddlEquipment.ClientID %>").val() + " ?";
                           //msg = 'Do you want to create new ticket ?';
                       } else {
                           AGError("กรุณาระบุ Configuration Item");
                           return;
                       }
                       if ($("#<%= _ddl_sctype.ClientID %>").val() == '') {
                           AGError("กรุณาระบุ Ticket Type");
                           return;
                       }
                       if ($("#<%= _txt_year.ClientID %>").val() == '') {
                           AGError("กรุณาระบุ Fiscal Year");
                           return;
                       }

                       if (AGConfirm(sender, msg)) {
                           AGLoading(true);
                           $("#<%= btnCreateNewTicketRef.ClientID %>").click();
                return true;
            }
            return false;
        }

        function updateCustomerDetailRefModalClick(sender) {
            var x = document.getElementsByClassName("txt-add-address");
            var className = "txt-add-address";
            var data = "";
            data = '{"address":[';
            for (var i = 0; i < x.length; i++) {
                data += '{"PropertyCode":"' + x[i].getAttribute("data-PropertyCode")
                    + '","Description":"' + x[i].getAttribute("data-Description")
                    + '","PropertyValue":"' + x[i].value + '"}'
                data += (i < x.length - 1) ? "," : "";
                //if (className == "txt-add-address") {
                //    x[i].value = "";
                //}
            }
            data += ']}';
            $("#hddJsonAddress").val(data);
            var msg = '';

            msg = "คุณต้องการให้อัพเดตข้อมูลลูกค้าหรือไม่ "
            if (AGConfirm(sender, msg)) {
                AGLoading(true);
                saveAddress(sender);
                $("#<%= btnUpdateCustomerDetail.ClientID %>").click();
                return true;
            }
            return false;
        }

        function saveAddress(obj) {
            //, className, rowhide, AddressCodeEdit
            var x = document.getElementsByClassName("txt-add-address");
            var data = "";
            data = '{"address":[';
            for (var i = 0; i < x.length; i++) {
                data += '{"PropertyCode":"' + x[i].getAttribute("data-PropertyCode")
                    + '","Description":"' + x[i].getAttribute("data-Description")
                    + '","PropertyValue":"' + x[i].value + '"}'
                data += (i < x.length - 1) ? "," : "";
                //if (className == "txt-add-address") {
                //    x[i].value = "";
                //}
            }
            data += ']}';

            $("#hddJsonAddress").val(data);

            //if (obj != '') {
            //    //hideEditAddress(rowhide);
            //    //$("#hddAddressCodeEdit").val(AddressCodeEdit)
            //    //obj.next().click();
            //    //$("#btnEdit").click();
            //} else {
            //    hideAddNewAddress();
            //}

            //AGLoading(true,'กำลังบันทึก');
        }


        function goToTicket() {
              window.open(url, '_blank', 'location=yes,height=840,width=1100,scrollbars=yes,status=yes');
        }   

        function exportexcel(obj) {

            
            $("#<%= btnExprotExcelContract.ClientID %>").click();
           
        }
        function downloadExcelContactData() {
            $("#download-report-excel")[0].click();
        }

        //function bindingDataTableJSCI() {
        //    $("#tableListCIItems").dataTable({
        //        columnDefs: [{
        //            "orderable": false,
        //            "targets": [0]
        //        }],
        //        "order": [[1, "asc"]]
        //    });
        //}

        function bindDataMyContactCard() {
            if ($("#SelectStatus")[0].value == "active") {
                $('.ContactCard-TRUE').show();
                $('.ContactCard-FALSE').hide();
            }
            $("#tableItemsMyTicket").AGWhiteLoading(true, 'Loading...');
            if ($("#SelectStatus")[0].value == "inactive") {
                $('.ContactCard-TRUE').hide();
                $('.ContactCard-FALSE').show();
            }
            if ($("#SelectStatus")[0].value == "") {
                $('.ContactCard-TRUE').show();
                $('.ContactCard-FALSE').show();
            }
        }
    </script>
</asp:Content>
