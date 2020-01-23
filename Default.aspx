<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ServiceWeb.Default" %>

<%@ Register Src="~/Accountability/UserControl/ApprovalListControl.ascx" TagPrefix="uc1" TagName="ApprovalListControl" %>
<%@ Register Src="~/Tier/UserControl/PopupCreateTierZeroControl.ascx" TagPrefix="uc1" TagName="PopupCreateTierZeroControl" %>
<%--<%@ Register Src="~/widget/usercontrol/SearchCustomerControl.ascx" TagPrefix="uc1" TagName="SearchCustomerControl" %>
<%@ Register Src="~/widget/usercontrol/SearchHelpCIControl.ascx" TagPrefix="uc1" TagName="SearchHelpCIControl" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        //var pageRefresh = 0;
        //var inter = window.setInterval(function () {
        //    if (pageRefresh = 1) {
        //        inter = clearInterval();
        //        pageRefresh = 0;
        //        location.reload();
        //    }
        //}, 5000);

        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-home").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    <div class="d-none">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpData">
            <ContentTemplate>
                <asp:Button Text="" runat="server" ID="btnRebindTierZeroList" ClientIDMode="Static"
                    OnClick="btnRebindTierZeroList_Click" OnClientClick="AGLoading(true);" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--<div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pb-2 mb-3 border-bottom">
        <h1 class="h2">Dashboard</h1>
        <div class="btn-toolbar mb-2 mb-md-0">
            <div class="btn-group mr-2">
                <button class="btn btn-sm btn-outline-secondary">Share</button>
                <button class="btn btn-sm btn-outline-secondary">Export</button>
            </div>
            <button class="btn btn-sm btn-outline-secondary dropdown-toggle">
                <span data-feather="calendar"></span>
                This week
             
            </button>
        </div>
    </div>--%>

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
        #tableItemsACI, #tableItemsMytask, #tableItemsOverdue{
            width: 100% !important;
        }
    </style>

    <% if (string.IsNullOrEmpty(ERPW.Lib.Authentication.ERPWAuthentication.Permission.RoleCode))
        { %>
    <div class="row">
        <div class="col-md-12">
            <div class="alert alert-danger" style="text-align: center;">
                ไม่สามารถใช้งานระบบ <span class="text-primary">Service Ticket</span> ได้ กรุณาติดต่อเจ้าหน้าที่เพื่อเปิดใช้สิทธิ์ในการใช้ระบบ !!
            </div>
        </div>
    </div>
    <% } %>

    <div class="row">
        <div class="col-md-12">

            <% if (ERPW.Lib.Authentication.ERPWAuthentication.Permission.IncidentView ||
                         ERPW.Lib.Authentication.ERPWAuthentication.Permission.RequestView ||
                         ERPW.Lib.Authentication.ERPWAuthentication.Permission.ProblemView ||
                         ERPW.Lib.Authentication.ERPWAuthentication.Permission.AllPermission)
                { %>

            <!-- Tier 0 : Un-Assign From Channel -->
            <div class="card mb-3">
                <div class="card-header">
                    <b>Tier 0 : Un-Assign From Channel</b>
                    <a class="btn btn-info btn-sm status ml-1" href="<%= Page.ResolveUrl("~/Tier/TierZeroList.aspx") %>">More ...</a>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table id="tableItems" class="table table-sm table-hover table-striped" style="margin-bottom: 0;">
                            <thead>
                                <tr>
                                    <th class="col-date text-nowrap">DateTime</th>
                                    <th class="col-ticket-type text-nowrap">Channel</th>
                                    <th class="col-subject">Subject</th>
                                    <th class="col-customer text-nowrap">Client</th>
                                    <th class="col-status text-nowrap"></th>
                                    <th class="col-status text-nowrap"></th>
                                    <th class="col-status text-nowrap"></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updTierZero">
                                    <ContentTemplate>
                                        <%--<asp:Repeater runat="server" ID="rptListTierZero">
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="col-date text-nowrap">
                                                        <%#Eval("CREATED_ON")%>
                                                    </td>
                                                    <td class="text-primary col-ticket-type text-nowrap">#<%#Eval("Channel")%></td>
                                                    <td class="col-subject text-truncate font-italic">
                                                        <%# Eval("Subject") %>                                       
                                                    </td>
                                                    <td class="text-truncate col-customer text-nowrap">
                                                        <span><%# Eval("CustomerName") %></span>
                                                    </td>
                                                    <td class="col-status text-nowrap">
                                                        <button type="button" class="btn btn-primary btn-sm status" onclick="createTierZeroTicket('<%# Eval("SEQ") %>');">Create Ticket</button>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>--%>
                                        
                                        <%-- <% if (rptListTierZero.Items.Count == 0)
                                            { %>
                                        <tr>
                                            <td class="text-nowrap text-center" colspan="5">ไม่พบข้อมูล
                                            </td>
                                        </tr>
                                        <% } %>--%>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </tbody>
                        </table>
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpJsonTierList">
                            <ContentTemplate>
                                <div style="display: none;" runat="server" id="divJsonTier0List" ClientIDMode="Static">[]</div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <script>
                function bindingDataTableJS() {
                    $("#tableItems").dataTable({
                        columnDefs: [{
                            "orderable": false,
                            "targets": [0]
                        }],
                        "order": [[1, "asc"]]
                    });
                }
            </script>

            <!-- Un-Assign-Configuration Item -->
            <div class="card border-warning mb-3 d-none">
                <div class="card-header bg-warning text-white">
                    <b>Un-Assign Configuration Item</b>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table id="tableItemsACI" class="table table-sm table-hover table-striped" style="margin-bottom: 0;">
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
                                <%--<asp:Repeater ID="rptUACI" runat="server">
                                    <ItemTemplate>
                                        <tr class="c-pointer" onclick="changeMyTask('<%# Eval("Doctype") %>', '<%# Eval("CallerID") %>', '<%# Eval("Fiscalyear") %>','<%# Eval("CustomerCode") %>');">
                                            <td class="col-date text-nowrap">
                                                <%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("StartDateTime").ToString().Substring(0, 8)) %>
                                            </td>
                                            <td class="text-primary col-ticket-type text-nowrap">
                                                <%# "#" + Eval("DocumentTypeDesc") %>
                                            </td>

                                            <td class="text-primary col-ticket-type text-nowrap">
                                                <%# Eval("TicketNoDisplay") %>
                                            </td>

                                            <td class="col-subject text-truncate font-italic">
                                                <%# Eval("HeaderText") %>
                                            </td>
                                            <td class="text-truncate col-customer text-nowrap">
                                                <%# Eval("CustomerName") %>
                                            </td>
                                            <td class="col-status text-nowrap">
                                                <div class="status <%# Eval("StatusCode") %>">
                                                    <%# Eval("StatusDesc") %>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>--%>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <script>
                function bindingDataTableJSACI() {
                    $("#tableItemsACI").dataTable({
                        columnDefs: [{
                            "orderable": false,
                            "targets": [0]
                        }],
                        "order": [[1, "asc"]]
                    });
                }
            </script>
            <!-- End Un-Assign-Configuration Item -->

            <!-- My Task -->
            <%--<div class="card border-info mb-3">
                <div class="card-header bg-info text-white">
                    <b>My Task</b>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table id="tableItemsMytask" class="table table-sm table-hover table-striped" style="margin-bottom: 0;">
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
                            <tbody>--%>
                                <%--<asp:Repeater ID="rptMyTask" runat="server">
                                    <ItemTemplate>
                                        <tr class="c-pointer" onclick="changeMyTask('<%# Eval("Doctype") %>', '<%# Eval("CallerID") %>', '<%# Eval("Fiscalyear") %>','<%# Eval("CustomerCode") %>');">
                                            <td class="col-date text-nowrap">
                                                <%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("StartDateTime").ToString().Substring(0, 8)) %>
                                            </td>
                                            <td class="text-primary col-ticket-type text-nowrap">
                                                <%# "#" + Eval("DocumentTypeDesc") %>
                                            </td>
                                            <td class="text-primary col-ticket-type text-nowrap">
                                                <%# Eval("TicketNoDisplay") %>
                                            </td>
                                            <td class="col-subject text-truncate font-italic">
                                                <%# Eval("HeaderText") %>
                                            </td>
                                            <td class="text-truncate col-customer text-nowrap">
                                                <%# Eval("CustomerName") %>
                                            </td>
                                            <td class="col-status text-nowrap">
                                                <div class="status <%# Eval("StatusCode") %>">
                                                    <%# Eval("StatusDesc") %>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>--%>
                            <%--</tbody>
                        </table>
                    </div>
                </div>
            </div>--%>

           <%-- <script>
                function bindingDataTableMyTask() {
                    $("#tableItemsMytask").dataTable({
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
               
            </script>--%>

            <%--<asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField ID="hdfChange" runat="server" ClientIDMode="Static" />
                    <asp:Button ID="btnChange" runat="server" ClientIDMode="Static" CssClass="d-none" OnClick="btnChange_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>--%>
            <!-- Delay - Risk -->
            <%--<div class="card border-danger mb-3">
                <div class="card-header bg-danger text-white">
                    <div class="row">
                        <div class="col-sm-9">
                            <b>Overdue</b>
                        </div>
                        <div class="col-sm-3">
                            <!--13/11/2561 add filter by owner group  (by born kk)-->
                            <asp:UpdatePanel ID="udpnOwnerService" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <!--<label>Owner Service</label>-->
                                    <asp:DropDownList ID="ddlOwnerGroupService" AutoPostBack="true" onchange="filterMyTaskOver(this);"
                                        CssClass="form-control form-control-sm col-sm-12 " runat="server">--%>
                                        <%--OnSelectedIndexChanged="OnSelectedIndexChanged"--%> 
                                    <%--</asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>--%>

                <%--<div class="card-body">

                    <asp:UpdatePanel ID="updateTableOverdue" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table id="tableItemsOverdue" class="table table-sm table-hover table-striped" style="margin-bottom: 0;">
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
                                    <tbody>--%>
                                        <%--<asp:Repeater ID="rptDelayRisk" runat="server">
                                            <ItemTemplate>
                                                <tr class="c-pointer" onclick="changeMyTask('<%# Eval("Doctype") %>', '<%# Eval("CallerID") %>', '<%# Eval("Fiscalyear") %>','<%# Eval("CustomerCode") %>');">
                                                    <td class="col-date text-nowrap">
                                                        <%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("StartDateTime").ToString().Substring(0, 8)) %>
                                                    </td>
                                                    <td class="text-primary col-ticket-type text-nowrap">
                                                        <%# "#" + Eval("DocumentTypeDesc") %>
                                                    </td>
                                                    <td class="text-primary col-ticket-type text-nowrap">
                                                        <span style="color: red;"><%# Eval("TicketNoDisplay") %></span>
                                                    </td>
                                                    <td class="col-subject text-truncate font-italic">
                                                        <%# Eval("HeaderText") %>
                                                    </td>
                                                    <td class="text-truncate col-customer text-nowrap">
                                                        <%# Eval("CustomerName") %>
                                                    </td>
                                                    <td class="col-status text-nowrap">
                                                        <div class="status <%# Eval("StatusCode") %>">
                                                            <%# Eval("StatusDesc") %>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>--%>
                                    <%--</tbody>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>--%>
            <%--<script>
                function bindingDataTableJSOverdue() {
                    $("#tableItemsOverdue").dataTable({
                        columnDefs: [{
                            "orderable": false,
                            "targets": [0]
                        }],
                        "order": [[1, "asc"]]
                    });
                }
            </script>--%>
            <% } %>

            <% if (ERPW.Lib.Authentication.ERPWAuthentication.Permission.ChangeOrderView ||
                         ERPW.Lib.Authentication.ERPWAuthentication.Permission.AllPermission)
                { %>
            <!-- Approval -->
            <uc1:ApprovalListControl runat="server" id="ApprovalListControl" showAllList="true" />
            <% } %>
        </div>

        <div class="col-md-4 d-none">
            <% if (ERPW.Lib.Authentication.ERPWAuthentication.Permission.IncidentView ||
                         ERPW.Lib.Authentication.ERPWAuthentication.Permission.RequestView ||
                         ERPW.Lib.Authentication.ERPWAuthentication.Permission.ProblemView ||
                         ERPW.Lib.Authentication.ERPWAuthentication.Permission.AllPermission)
                { %>

            <!-- Latest Update Tickets -->
            <div class="card mb-3 d-none">
                <div class="card-header">
                    <b>Latest Update Tickets</b>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table class="table table-sm table-hover table-striped" style="margin-bottom: 0;">
                            <tbody>
                                <tr>
                                    <td style="max-width: 8rem;">
                                        <div class="text-truncate" style="font-weight: 500;">
                                            <span class="text-primary">#TK-201805250002</span>
                                            Nunc pretium vestibulum orci eu condimentum. Donec suscipit eget justo sed facilisis. Aenean at neque ut quam vehicula lobortis in eu dolor. Phasellus id placerat quam. Mauris ut leo varius odio pulvinar varius. Aliquam tempus purus a neque pharetra mollis. Praesent tempus diam non erat rutrum consectetur.
                                        </div>
                                        <div class="text-truncate font-italic">Noted sir.</div>
                                        <div class="text-truncate font-italic text-info">Kunathip - 25/05/2018 09:47</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="max-width: 8rem;">
                                        <div class="text-truncate" style="font-weight: 500;">
                                            <span class="text-primary">#TK-201805250002</span>
                                            Nunc pretium vestibulum orci eu condimentum. Donec suscipit eget justo sed facilisis. Aenean at neque ut quam vehicula lobortis in eu dolor. Phasellus id placerat quam. Mauris ut leo varius odio pulvinar varius. Aliquam tempus purus a neque pharetra mollis. Praesent tempus diam non erat rutrum consectetur.
                                        </div>
                                        <div class="text-truncate font-italic">Hurry up.</div>
                                        <div class="text-truncate font-italic text-info">Theera - 25/05/2018 09:13</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="max-width: 8rem;">
                                        <div class="text-truncate" style="font-weight: 500;">
                                            <span class="text-primary">#TK-201805250001</span>
                                            In euismod luctus ipsum. Maecenas euismod euismod scelerisque. Curabitur ullamcorper nisi in ante congue pulvinar. Duis tincidunt ipsum libero, nec molestie felis varius fringilla. Donec condimentum ligula sed metus faucibus, quis dignissim nibh fermentum. Mauris at dictum sapien. Aenean eleifend quam nulla, vel faucibus sem finibus ut. Nunc eget erat dictum, volutpat massa non, finibus orci. Donec gravida eu elit id ullamcorper. Etiam egestas lacus at congue mollis. Nullam vitae malesuada velit. Mauris mollis tincidunt hendrerit.
                                        </div>
                                        <div class="text-truncate font-italic">Yes, I agree with you.</div>
                                        <div class="text-truncate font-italic text-info">Taywin - 24/05/2018 11:32</div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <% } %>
        </div>
    </div>

    <div class="modal fade" id="modal-weekly-status">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Weekly Status</h4>
                    <button type="button" class="close" data-dismiss="modal">×</button>
                </div>
                <div class="modal-body modal-lg">
                    <!-- Weekly Status -->
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpMyChart">
                        <ContentTemplate>
                            <%--<div class="card mb-3">
                                <div class="card-header">
                                    <b>Weekly Status</b>
                                </div>
                                <div class="card-body">
                                    
                                </div>
                            </div>--%>
                            <canvas id="myChart"></canvas>
                            <asp:Repeater runat="server" ID="rptMyChart">
                                <ItemTemplate>
                                    <div class="d-none myChart-open"><%# Eval("open") %></div>
                                    <div class="d-none myChart-close"><%# Eval("close") %></div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="d-none">
                        <script>
                            $('#modal-weekly-status').on('shown.bs.modal', function () {
                                //$('#myInput').trigger('focus')
                                LoadMyChart_Default();
                            })

                            function LoadMyChart_Default() {
                                if (!$("#chkIsLoadMyChart").prop("checked")) {
                                    $("#btnLoadMyChart").click();
                                }
                            }
                        </script>
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:CheckBox Text="" runat="server" ID="chkIsLoadMyChart" ClientIDMode="Static" />
                                <asp:Button Text="" runat="server" CssClass="d-none"  ClientIDMode="Static"
                                    ID="btnLoadMyChart" OnClick="btnLoadMyChart_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="tier0desc" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">Tier0 Detail</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
              <div class="form-row">
                  <div class="col-sm-3 font-weight-bold">SEQ</div>
                  <div class="col-sm-9"><label id="SEQ_txt"></label></div>
              </div>
              <div class="form-row">
                  <div class="col-sm-3 font-weight-bold">Channel</div>
                  <div class="col-sm-9"><label id="Channel_txt"></label></div>
              </div>
              <div class="form-row">
                  <div class="col-sm-3 font-weight-bold">EMail</div>
                  <div class="col-sm-9"><label id="EMail_txt"></label></div>
              </div>
              <div class="form-row">
                  <div class="col-sm-3 font-weight-bold">Status</div>
                  <div class="col-sm-9"><label id="Status_txt"></label></div>
              </div>
              <div class="form-row">
                  <div class="col-sm-3 font-weight-bold">Customer Code</div>
                  <div class="col-sm-9"><label id="CustomerCode_txt"></label></div>
              </div>
              <div class="form-row">
                  <div class="col-sm-3 font-weight-bold">Customer Name</div>
                  <div class="col-sm-9"><label id="CustomerName_txt"></label></div>
              </div>
              <div class="form-row">
                  <div class="col-sm-3 font-weight-bold">TelNO.</div>
                  <div class="col-sm-9"><label id="TelNO_txt"></label></div>
              </div>
              <div class="form-row">
                  <div class="col-sm-3 font-weight-bold">Equipment</div>
                  <div class="col-sm-9"><label id="Equipment_txt"></label></div>
              </div>
              <div class="form-row">
                  <div class="col-sm-3 font-weight-bold">Ticket Type</div>
                  <div class="col-sm-9"><label id="TicketType_txt"></label></div>
              </div>
              <div class="form-row">
                  <div class="col-sm-3 font-weight-bold">Ticket NO.</div>
                  <div class="col-sm-9"><label id="TicketNO_txt"></label></div>
              </div>
              <div class="form-row">
                  <div class="col-sm-3 font-weight-bold">Subject</div>
                  <div class="col-sm-9"><label id="Subject_txt"></label></div>
              </div>
              <div class="form-row">
                  <div class="col-sm-3 font-weight-bold">Detail</div>
                  <div class="col-sm-9"><label id="Detail_txt"></label></div>
              </div>
              <div class="form-row">
                  <div class="col-sm-3 font-weight-bold">Created By</div>
                  <div class="col-sm-9"><label id="CreatedBy_txt"></label></div>
              </div>
              <div class="form-row">
                  <div class="col-sm-3 font-weight-bold">Created On</div>
                  <div class="col-sm-9"><label id="CreatedOn_txt"></label></div>
              </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
          </div>
        </div>
      </div>
    </div>
    <!-- Graphs -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.1/Chart.min.js"></script>

    <script>
        function BindMyChart() {
            var ctx = document.getElementById("myChart");
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
            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
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
    </script>

    <uc1:PopupCreateTierZeroControl runat="server" id="PopupCreateTierZeroControl" />

    <%--    <style>
        .panel-create-tiarzero {
            position: relative;
        }
            .panel-create-tiarzero .tiarzero-icon {
                position: fixed;
                right: 10px;
                bottom: 10px;
                padding: 5px 12px;
                border: 1px solid #ffc107;
                border-radius: 50%;
                background-color: #fff04f;
                cursor: pointer;
                box-shadow: 0 5px 11px 0 rgba(0, 0, 0, 0.18), 0 4px 15px 0 rgba(0, 0, 0, 0.15);
                font-size: 25px;
            }
            .panel-create-tiarzero .tiarzero-content {
                width: 450px;
                height: 250px;
                right: 16px;
                bottom: 70px;
                position: fixed;
                border: 1px solid #b3b3b3;
                background-color: #fff;
                box-shadow: 0 5px 11px 0 rgba(0, 0, 0, 0.18), 0 4px 15px 0 rgba(0, 0, 0, 0.15);
                display: none;
            }
            .panel-create-tiarzero .tiarzero-content.open {
                display: block;
            }
            .panel-create-tiarzero .tiarzero-content::after {
                content: "";
                position: absolute;
                top: 100%;
                right: 40px;
                margin-left: -5px;
                border-top-width: 20px;
                border-bottom-width: 0px;
                border-right-width: 5px;
                border-left-width: 20px;
                border-style: solid;
                border-color: #fff transparent transparent transparent;
            }
            .panel-create-tiarzero .tiarzero-content input,
            .panel-create-tiarzero .tiarzero-content textarea
             {
                margin-bottom: 10px;
            }

        .content-group .content-item {
            display: none;
        }
        .content-group .content-item:first-child {
            display: block;
        }
        .bg-required {
            background-color: #ffdbdb;
        }
    </style>
    <div class="panel-create-tiarzero" style="">
        <div class="tiarzero-content">
            <div class="card" style="border: none;">
                <div class="card-header" style="padding: 7px;">
                    <i class="fa fa-times float-right text-danger c-pointer"
                        onclick="closeTiarZeroInform(this);"></i>
                    <b>Tier 0 : Inform a problem</b>
                </div>
                <div class="card-body" style="min-height: 166px;padding: 10px;">
                    <div class="content-group">
                        <div class="content-item">
                            <div class="form-row">
                                <div class="col-12">
                                    <input type="text" id="txtInformProblem_Subject" value="" 
                                        placeholder="Subject" class="form-control form-control-sm required" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="col-12">
                                    <textarea placeholder="Description" id="txtInformProblem_Description"
                                        class="form-control form-control-sm" rows="4" 
                                        style="resize: none;"></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="content-item">
                            <div class="form-row">
                                <div class="col-12">
                                    <input type="text" id="txtInformProblem_Customer" value="" 
                                        placeholder="Customer" class="form-control form-control-sm required" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="col-12">
                                    <input type="text" id="txtInformProblem_Email" value="" 
                                        placeholder="Email" class="form-control form-control-sm" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="col-12">
                                    <input type="text" id="txtInformProblem_TelNo" value="" 
                                        placeholder="TelNo" class="form-control form-control-sm" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card-footer" style="text-align: right;padding: 7px;">
                    <button type="button" class="btn btn-warning btn-sm btn-prev-item d-none" style="width: 90px;" data-nextprev="-1" onclick="nextPrevItem(this);">Previous</button>
                    <button type="button" class="btn btn-primary btn-sm btn-next-item" style="width: 90px;" data-nextprev="1" onclick="nextPrevItem(this);">Next</button>
                    <button type="button" class="btn btn-success btn-sm btn-submit-Inform d-none" style="width: 90px;" onclick="saveTierZerItem(this);">Submit</button>
                </div>
            </div>
        </div>
        <div class="tiarzero-icon" onclick="openTiarZeroInform(this);">
            <i class="fa fa-bullhorn"></i>
        </div>
    </div>
    <script>
        var numContentItem = 0;
        function openTiarZeroInform(obj) {
            $(obj).prev().toggleClass('open');
        }
        function closeTiarZeroInform(obj) {
            $(obj).closest('.tiarzero-content').removeClass('open');
            resetTiarZeroInform(obj);
        }
        function nextPrevItem(obj) {
            var target = $(obj).closest('.tiarzero-content');

            var IsAlowNext = true;
            target.find(".required:visible").each(function () {
                if ($(this).val() == '') {
                    IsAlowNext = false;
                    $(this).addClass('bg-required');
                }
            });
            if (!IsAlowNext) {
                return;
            }

            var item = target.find('.content-item');
            var xCount = item.length;
            var numPage = parseInt($(obj).attr('data-nextprev'));

            numContentItem = numContentItem + numPage;
            item.hide();
            $(item[numContentItem]).show();

            if (numContentItem == 0) {
                target.find('.btn-prev-item').addClass('d-none');
            } else {
                target.find('.btn-prev-item').removeClass('d-none');
            }

            if (numContentItem == xCount - 1) {
                target.find('.btn-next-item').addClass('d-none');
                target.find('.btn-submit-Inform').removeClass('d-none');
            } else {
                target.find('.btn-next-item').removeClass('d-none');
                target.find('.btn-submit-Inform').addClass('d-none');
            }
        }

        function saveTierZerItem(obj) {
            var target = $(obj).closest('.tiarzero-content');
            var IsAlowNext = true;
            target.find(".required:visible").each(function () {
                if ($(this).val() == '') {
                    IsAlowNext = false;
                    $(this).addClass('bg-required');
                }
            });
            if (!IsAlowNext) {
                return;
            }

            $(obj).closest('.tiarzero-content .card').AGWhiteLoading(true, 'Save Process..');
            var postData = {
                Channel: '2',
                EMail: $("#txtInformProblem_Email").val(),
                CustomerCode: $("#txtInformProblem_Customer").val(),
                CustomerName: $("#txtInformProblem_Customer").val(),
                TelNo: $("#txtInformProblem_TelNo").val(),
                Subject: $("#txtInformProblem_Subject").val(),
                Detail: $("#txtInformProblem_Description").val(),
                Status: '0',
                TicketNumber: '',
                TicketType: ''
            };
            $.ajax({
                type: "POST",
                url: servictWebDomainName + "API/TierZeroStructureAPI.aspx",
                data: postData,
                success: function (data) {
                    $(obj).closest('.tiarzero-content .card').AGWhiteLoading(false, 'Save Process..');
                    AGSuccess("Created Tiar 0 Success.");

                    resetTiarZeroInform(obj);
                    closeTiarZeroInform(obj);
                }
            });
        }
        function resetTiarZeroInform(obj) {
            var target = $(obj).closest('.tiarzero-content');
            var item = target.find('.content-item');
            numContentItem = 0;
            item.hide();
            $(item[numContentItem]).show();

            target.find('.btn-prev-item').addClass('d-none');
            target.find('.btn-next-item').removeClass('d-none');
            target.find('.btn-submit-Inform').addClass('d-none');
            target.find(".required").removeClass('bg-required');

            $("#txtInformProblem_Email").val('');
            $("#txtInformProblem_Customer").val('');
            $("#txtInformProblem_TelNo").val('');
            $("#txtInformProblem_Subject").val('');
            $("#txtInformProblem_Description").val('');

        }
    </script>--%>
    <%--<uc1:SearchCustomerControl runat="server" id="SearchCustomerControl" />
    <uc1:SearchHelpCIControl runat="server" id="SearchHelpCIControl" />--%>
    <script>
        var Tier0List = JSON.parse($("#divJsonTier0List").html());
        function setTier0List() {
            var Tier0List = JSON.parse($("#divJsonTier0List").html());
            var data = [];
            var cfs = "";
            for (var i = 0 ; i < Tier0List.length ; i++) {
                var Tier0 = Tier0List[i];
                if (Tier0.ControlDisplay == "True") {
                    cfs = "d-none"
                } else {
                    cfs = "";
                }
                data.push([
                    Tier0.CREATED_ON,
                    Tier0.Channel,
                    Tier0.Subject,
                    Tier0.CustomerName,
                    "<i class='fa fa-search' onclick=\"searchDesc('" + Tier0.SEQ + "');\" data-toggle='modal' data-target='#tier0desc'></i>",
                    "<i class='fa fa-times-circle-o fa-lg text-danger" + cfs + "' onclick=\"createTierZeroClose('" + Tier0.SEQ + "', '" + Tier0.Status + "'); \" ></i > ",
                    "<i class='fa fa-edit fa-lg text-dark' onclick=\"createTierZeroTicket('"+ Tier0.SEQ + "');\"></i>",
                ]);
            }

            //$("#search-panel").show();
            $("#tableItems").dataTable({
                data: data,
                deferRender: true,
                "order": [[1, "asc"]],
                columnDefs: [{
                    "orderable": false,
                    "targets": [0],
                    "createdCell": function (td, cellData, rowData, row, col) {
                        $(td).addClass("col-date text-nowrap");
                    }
                },{
                    "orderable": false,
                    "targets": [1],
                    "createdCell": function (td, cellData, rowData, row, col) {
                        $(td).addClass("text-primary col-ticket-type text-nowrap");
                    }
                },{
                    "orderable": false,
                    "targets": [2],
                    "createdCell": function (td, cellData, rowData, row, col) {
                        $(td).addClass("col-subject text-truncate font-italic");
                        }
                    },{
                    "orderable": false,
                    "targets": [3],
                    "createdCell": function (td, cellData, rowData, row, col) {
                        $(td).addClass("text-truncate col-customer text-nowrap");
                        }
                    }, {
                    "orderable": false,
                    "targets": [4],
                    "createdCell": function (td, cellData, rowData, row, col) {
                        $(td).addClass("text-center");
                        $(td).addClass("col-status text-nowrap");
                    }
                },{
                    "orderable": false,
                    "targets": [5],
                    "createdCell": function (td, cellData, rowData, row, col) {
                        $(td).addClass("text-center");
                        $(td).addClass("col-status text-nowrap");
                    }
                },{
                    "orderable": false,
                    "targets": [6],
                    "createdCell": function (td, cellData, rowData, row, col) {
                        $(td).addClass("text-center");
                        $(td).addClass("col-status text-nowrap");
                    }
                }]
            });
        };
    </script>
    <script>
        var result = "";
        var SEQ = "";
        function searchDesc(SEQ) {
            this.SEQ = SEQ;
            var data = {
                SEQ:this.SEQ
            };

            $.ajax({
                type: "GET",
                url: "<%= Page.ResolveUrl("~/API/Tier0API.aspx") %>",
                data: data,
                success: function (res) {
                    result = JSON.parse(res);
                    document.getElementById("SEQ_txt").innerHTML = result[0]["SEQ"];
                    document.getElementById("Channel_txt").innerHTML = result[0]["Channel"];
                    document.getElementById("EMail_txt").innerHTML = result[0]["EMail"];
                    document.getElementById("Status_txt").innerHTML = result[0]["Status"];
                    document.getElementById("CustomerCode_txt").innerHTML = result[0]["CustomerCode"];
                    document.getElementById("CustomerName_txt").innerHTML = result[0]["CustomerName"];
                    document.getElementById("TelNO_txt").innerHTML = result[0]["TelNo"];
                    document.getElementById("Equipment_txt").innerHTML = result[0]["EquipmentNo"];
                    document.getElementById("TicketType_txt").innerHTML = result[0]["TicketType"];
                    document.getElementById("TicketNO_txt").innerHTML = result[0]["TicketNumber"];
                    document.getElementById("Subject_txt").innerHTML = result[0]["Subject"];
                    document.getElementById("Detail_txt").innerHTML = result[0]["Detail"];
                    document.getElementById("CreatedBy_txt").innerHTML = result[0]["CREATED_BY"];
                    document.getElementById("CreatedOn_txt").innerHTML = result[0]["CREATED_ON"];
                    console.log(res);
                },
                error: function (err) {
                    console.log(err);
                }
            });
        };

        var objDatasMyTicket;
        var myTaskDataTable, overdueDataTable, unassignCIDataTable;
        $(document).ready(function () { 
            return;
            $("#tableItemsMytask").AGWhiteLoading(true, 'Loading...');
            $("#tableItemsOverdue").AGWhiteLoading(true, 'Loading...');
            //$("#tableItemsACI").AGWhiteLoading(true, 'Loading...');
            var postData = {
            };
            $.ajax({
                type: "POST",
                url: servictWebDomainName + "API/v1/GetTicketMyTaskAPI.aspx",
                data: postData,
                success: function (datas) {
                    objDatasMyTicket = JSON.parse(datas);

                    myTaskDataTable = bindListMyTicket($("#tableItemsMytask"), objDatasMyTicket);
                    $("#tableItemsMytask").AGWhiteLoading(false, 'Loading...');
                    
                    //overdueDataTable = bindListMyTicket($("#tableItemsOverdue"), objDatasMyTicket.delay_risk);
                    //$("#tableItemsOverdue").AGWhiteLoading(false, 'Loading...');
                    
                    //unassignCIDataTable = bindListMyTicket($("#tableItemsACI"), objDatasMyTicket.unassign_ci);
                    //$("#tableItemsACI").AGWhiteLoading(false, 'Loading...');
                }
            });

            
            $.ajax({
                type: "POST",
                url: servictWebDomainName + "API/v1/GetTicketMyOverDueAPI.aspx",
                data: postData,
                success: function (datas) {
                    objDatasMyTicket = JSON.parse(datas);

                    overdueDataTable = bindListMyTicket($("#tableItemsOverdue"), objDatasMyTicket);
                    $("#tableItemsOverdue").AGWhiteLoading(false, 'Loading...');
                }
            });
        });

        function filterMyTaskOver(obj) {
            $("#tableItemsOverdue").AGWhiteLoading(true, 'Loading...');
            overdueDataTable.DataTable().clear();

            var delay_risk_filter = [];
            var owner = $(obj).val();
            for (var i = 0; i < objDatasMyTicket.delay_risk.length; i++) {
                var ticket = objDatasMyTicket.delay_risk[i];
                
                var flag = '';
                if (ticket.CustomerCritical == 'CRITICAL') {
                    flag = '<img src="/images/icon/flag-red-512.png" width="20" height="20">&nbsp;';
                }

                if (ticket.OwnerGroupService == owner && owner != '') {
                    delay_risk_filter.push([
                        ticket.StartDateTime,
                        ticket.DocumentTypeDesc,
                        ticket.CallerID + "|" + ticket.TicketNoDisplay + "|" + ticket.Doctype + "|" + ticket.Fiscalyear + "|" + ticket.CustomerCode,
                        ticket.HeaderText,
                        flag + ticket.CustomerName,
                        ticket.StatusCode + "|" + ticket.StatusDesc
                    ]);
                } else if(owner == '') {
                    delay_risk_filter.push([
                        ticket.StartDateTime,
                        ticket.DocumentTypeDesc,
                        ticket.CallerID + "|" + ticket.TicketNoDisplay + "|" + ticket.Doctype + "|" + ticket.Fiscalyear + "|" + ticket.CustomerCode,
                        ticket.HeaderText,
                        flag + ticket.CustomerName,
                        ticket.StatusCode + "|" + ticket.StatusDesc
                    ]);
                }
            }
            overdueDataTable.DataTable().rows.add(delay_risk_filter);
            overdueDataTable.DataTable().draw();
            $("#tableItemsOverdue").AGWhiteLoading(false, 'Loading...');
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
                    ticket.StartDateTime,
                    ticket.DocumentTypeDesc,
                    ticket.CallerID + "|" + ticket.TicketNoDisplay + "|" + ticket.Doctype + "|" + ticket.Fiscalyear + "|" + ticket.CustomerCode,
                    ticket.HeaderText,
                    flag + ticket.CustomerName,
                    ticket.StatusCode + "|" + ticket.StatusDesc
                ]);
            }

            var dataTableResult = objTarget.dataTable({
                data: datasTicket,
                deferRender: true,
                "order": [[0, "asc"]],
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
                           $(td).addClass("text-primary col-ticket-type text-nowrap");
                           $(td).html(
                               '#' + cellData
                           );
                       }
                   },
                   {
                       'targets': 2,
                       'createdCell': function (td, cellData, rowData, row, col) {
                           $(td).addClass("text-primary col-ticket-type text-nowrap");
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
                       }
                   },
                   {
                       'targets': 5,
                       'createdCell': function (td, cellData, rowData, row, col) {
                           $(td).addClass("col-status text-nowrap");
                           $(td).html(
                               '<div class="status ' + cellData.split('|')[0] + ' ' + cellData.split('|')[1] +'">' +
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
</asp:Content>
