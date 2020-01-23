<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="MTTRReport.aspx.cs" Inherits="ServiceWeb.Report.MTTRReport" %>

<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteCustomer.ascx" TagPrefix="uc1" TagName="AutoCompleteCustomer" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEmployee.ascx" TagPrefix="uc1" TagName="AutoCompleteEmployee" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-mttr").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })

    </script>
    <div class="">
        <div class="card mb-4 shadow-sm">
            <div class="card-header">
                <h4 class="my-0 font-weight-normal">MTTR</h4>
            </div>
            <div class="card-body PANEL-DEFAULT-BUTTON">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                    <ContentTemplate>
                        <div class="form-row">
                            <!-- create columns 1 -->
                            <div class="form-group col-lg-6">
                                <div class="card border-primary" style="margin-bottom: 10px;">
                                    <div class="card-body card-body-sm">
                                        <div class="form-row">
                                            <div class="col-lg-6">
                                                <label>Incidenct No.</label>
                                                <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="mttr_incident_no" />
                                            </div>
                                            <div class="col-lg-6">
                                                <label>Type</label>
                                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="mttr_ticket_type" ClientIDMode="Static"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div>
                                            <label>Client Name</label>
                                            <uc1:AutoCompleteCustomer runat="server" id="AutoCompleteCustomer" CssClass="form-control form-control-sm" />
                                        </div>
                                        <div class="form-row">
                                            <div class="col-lg-6">
                                                <label>Close Log Date</label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="mttr_close_date_from" />
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6">
                                                <label>To</label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="mttr_close_date_to" />
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="form-group col-6">
                                                <label>Resolved Date</label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="mttr_resolved_date_from" />
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group col-6">
                                                <label>To</label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="mttr_resolved_date_to" />
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- create columns 2 -->
                            <div class="form-group col-lg-6">
                                <div class="card border-primary" style="margin-bottom: 10px;">
                                    <div class="card-body card-body-sm">
                                        <div class="form-row">
                                            <label>Assignee Resolved</label>
                                            <uc1:AutoCompleteEmployee runat="server" CssClass="form-control form-control-sm" id="AutoCompleteEmployee_Assignee_Resolved" />
                                        </div>
                                        <div class="form-row">
                                            <div class="col-lg-6">
                                                <label>Open Date</label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="mttr_opendate_from" />
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6">
                                                <label>To</label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="mttr_opendate_to" />
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="form-group col-12">
                                                <label>Resolved Group</label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="mttr_resolved_group" />

                                                </div>
                                                <label>MTRS (Time)</label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="mttr_time" Enabled="false" />
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpUpdateData">
                    <ContentTemplate>
                        <asp:Button runat="server" CssClass="btn btn-info DEFAULT-BUTTON-CLICK" Text="Search" ID="mttr_btn_search" OnClick="mttr_btn_search_Click" OnClientClick="AGLoading(true);" />
                        <asp:Button runat="server" CssClass="btn btn-warning" Text="Clear" />
                        <asp:Button runat="server" CssClass="btn btn-success" Text="Export Data" ID="ui_export_button" OnClick="ui_export_button_Click" OnClientClick="AGLoading(true);" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <a id="download-report-excel" class="hide" target="_blank"
                href="<%= Page.ResolveUrl("~/API/ExportExcelAPI.ashx") %>"></a>
            <div id="search-panel" style="padding: 18px;">
                <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="table-responsive">
                            <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                <thead>
                                    <tr>
                                        <th class="text-nowrap">Incident No.</th>
                                        <th class="text-nowrap">Type</th>
                                        <th class="text-nowrap">Client Name</th>
                                        <th class="text-nowrap">OwnerGroup Name</th>
                                        <th class="text-nowrap">PriorityDesc</th>
                                        <th class="text-nowrap">Open Date</th>
                                        <th class="text-nowrap">Resolved Date</th>
                                        <th class="text-nowrap">Close Log</th>
                                        <th class="text-nowrap">Assignee Resolved</th>
                                        <th class="text-nowrap">Resolved Group</th>
                                        <th class="text-nowrap">MTRS(Time)</th>
                                    </tr>
                                </thead>
                                <%--<tbody>
                                    <asp:Repeater ID="rptSearchSale" runat="server">
                                        <ItemTemplate>
                                            <tr class="table-hover">
                                                <td class="text-nowrap">
                                                    <%# Eval("IncidentNO") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("Type") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("Customer_Name") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("Open_Date") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("Resolved_Date") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("Close_Log") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("Assignee_Resolved") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("Resolved_Group").ToString() %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("Time").ToString() %>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>--%>
                            </table>
                        </div>
                        <div runat="server" id="divDataJson" class="d-none"></div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>

    </div>
    <script type="text/javascript">
        function exportExcelAPI() {
            $("#download-report-excel")[0].click();
        }

        function afterSearch() {
            var reportsList = JSON.parse($("#<%= divDataJson.ClientID %>").html());
            var data = [];
            for (var i = 0 ; i < reportsList.length ; i++) {
                var report = reportsList[i];
                data.push([
                    report.IncidentNO,
                    report.Type,
                    report.Customer_Name,
                    report.OwnerGroupName,
                    report.PriorityDesc,
                    report.Open_Date,
                    report.Resolved_Date,
                    report.Close_Log,
                    report.Assignee_Resolved,
                    report.Resolved_Group,
                    report.Time
                ]);
            }

            //$("#search-panel").show();
            $("#tableItems").dataTable({
                data: data,
                deferRender: true,
                columnDefs: [{
                    //"orderable": false,
                    "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8],
                    "createdCell": function (td, cellData, rowData, row, col) {
                        $(td).closest("tr").addClass("text-nowrap");
                    }
                }]
            });
            scrollToTable();
        }

        function scrollToTable() {
            $('html,body').animate({
                scrollTop: $("#search-panel").offset().top - 50
            });
        }

        //$(document).ready(function () {
        //    $('#tableItems').DataTable();
        //});
    </script>
</asp:Content>
