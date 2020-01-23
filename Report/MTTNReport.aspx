<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="MTTNReport.aspx.cs" Inherits="ServiceWeb.Report.MTTNReport" %>

<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteCustomer.ascx" TagPrefix="uc1" TagName="AutoCompleteCustomer" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEmployee.ascx" TagPrefix="uc1" TagName="AutoCompleteEmployee" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEquipment.ascx" TagPrefix="uc1" TagName="AutoCompleteEquipment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-mttn").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    <div class="">
        <div class="card mb-4 shadow-sm">
            <div class="card-header">
                <h4 class="my-0 font-weight-normal">MTTN</h4>
            </div>
            <div class="card-body PANEL-DEFAULT-BUTTON">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                    <ContentTemplate>
                        <div class="form-row">
                            <!-- create columns 1 -->
                            <div class="form-group col-lg-6">
                                <div class="card border-primary" style="margin-bottom: 10px;">
                                    <div class="card-body card-body-sm">
                                        <div>
                                            <label>Incidenct No.</label>
                                            <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="mttn_incident_no" />
                                        </div>
                                        <div>
                                            <label>Type</label>
                                            <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="_mttn_ddl_ticket_type" ClientIDMode="Static"></asp:DropDownList>
                                        </div>
                                        <div>
                                            <label>Client Name</label>
                                            <uc1:AutoCompleteCustomer runat="server" id="AutoCompleteCustomer" CssClass="form-control  form-control-sm" />
                                        </div>
                                        <!-- opendate -->
                                        <div class="form-row">
                                            <div class="form-group col-sm-6">
                                                <label>Open Date</label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="mttn_opendate" />
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group col-sm-6">
                                                <label>To</label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="mttn_opendate_to" />
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- end of opendate -->
                                    </div>
                                </div>
                            </div>
                            <!-- create columns 2 -->
                            <div class="form-group col-lg-6">
                                <div class="card border-primary" style="margin-bottom: 10px;">
                                    <div class="card-body card-body-sm">
                                        <!-- start -->
                                        <div class="form-row">
                                            <div class="form-group col-sm-6">
                                                <label>Responding Date</label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="mttn_responding_date" />
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group col-sm-6">
                                                <label>To</label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="mttn_responding_date_to" />
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div>
                                            <label>Assignee</label>
                                            <uc1:AutoCompleteEmployee runat="server" CssClass="form-control form-control-sm" id="AutoCompleteEmployee_Assignee" />
                                        </div>
                                        <div>
                                            <label>Group</label>
                                            <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="mttn_group" />
                                        </div>
                                        <div>
                                            <label>MTTN (Time)</label>
                                            <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="mttn_time" Enabled="false" />
                                        </div>
                                    </div>
                                    <!-- end -->
                                </div>
                            </div>
                            <!-- end of columns 2 -->
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpUpdateData">
                    <ContentTemplate>
                        <!-- button -->
                        <asp:Button runat="server" CssClass="btn btn-info DEFAULT-BUTTON-CLICK" Text="Search" ID="mttn_btn_search" OnClick="mttn_btn_search_Click" OnClientClick="AGLoading(true);" />
                        <asp:Button runat="server" CssClass="btn btn-warning" Text="Clear" />
                        <asp:Button runat="server" CssClass="btn btn-success" Text="Export Data" ID="ui_export_button" OnClick="ui_export_button_Click" OnClientClick="AGLoading(true);" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <a id="download-report-excel" class="hide" target="_blank" href="<%= Page.ResolveUrl("~/API/ExportExcelAPI.ashx") %>"></a>

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
                                        <th class="text-nowrap">Open Date</th>
                                        <th class="text-nowrap">Responding Date</th>
                                        <th class="text-nowrap">Assignee</th>
                                        <th class="text-nowrap">Group</th>
                                        <th class="text-nowrap">MTTN(Time)</th>
                                    </tr>
                                </thead>
                                <%--<tbody>
                                    <asp:Repeater ID="rptSearchSale" runat="server">
                                        <ItemTemplate>
                                            <tr class="table-hover">
                                                <td class="text-nowrap">
                                                    <%# Eval("IncidentNO")%>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("Type") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("CustomerName") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("OpenDate") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("Responding_Date") %>
                                                </td>
                                                <td class="text-truncate">
                                                    <%# Eval("Assignee") %>
                                                </td>
                                                <td class="text-nowrap">
                                                    <%# Eval("Group").ToString() %>
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

        //$(document).ready(function () {
        //    $('#tableItems').DataTable();
        //});


        function afterSearch() {
            var reportsList = JSON.parse($("#<%= divDataJson.ClientID %>").html());
            var data = [];
            for (var i = 0 ; i < reportsList.length ; i++) {
                var report = reportsList[i];
                data.push([
                    report.IncidentNO,
                    report.Type,
                    report.CustomerName,
                    report.OpenDate,
                    report.Responding_Date,
                    report.Assignee,
                    report.Group,
                    report.Time
                ]);
            }

            //$("#search-panel").show();
            $("#tableItems").dataTable({
                data: data,
                deferRender: true,
                columnDefs: [{
                    //"orderable": false,
                    "targets": [0, 1, 2, 3, 4, 5, 6, 7],
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
    </script>
</asp:Content>
