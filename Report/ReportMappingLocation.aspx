<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="ReportMappingLocation.aspx.cs" Inherits="ServiceWeb.Report.ReportMappingLocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-report-location").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Mapping Location Report</h5>
                </div>
                <div class="card-body PANEL-DEFAULT-BUTTON">
                    <div class="form-row">
                        <div class="col-lg-12">
                            <div class="form-row">
                                <div class="form-group col-sm-6">
                                    <label>CI Code</label>
                                    <div class="input-group">
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm"
                                            ID="txtCICode"
                                            placeholder="CI Code" />
                                    </div>
                                </div>
                                <div class="form-group col-sm-6">
                                    <label>CI Name</label>
                                    <div class="input-group">
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm"
                                            ID="txtCIName"
                                            placeholder="CI Name" />
                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="udpnProblem" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="form-row">
                                        <div class="form-group col-sm-6">
                                            <label>Location Master</label>
                                            <asp:DropDownList ID="Locationddl" CssClass="form-control form-control-sm" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group col-sm-6">
                                            <label>Employee</label>
                                            <asp:DropDownList ID="Employeeddl" CssClass="form-control form-control-sm" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div>
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpUpdate">
                                    <ContentTemplate>
                                        <br />
                                        <asp:Button Text="Search" runat="server" ID="Button1"
                                            CssClass="btn btn-info DEFAULT-BUTTON-CLICK"
                                            OnClick="btn_search"
                                            OnClientClick="AGLoading(true);" />
                                        <asp:Button runat="server" CssClass="btn btn-success"
                                            Text="Export Data" ID="ui_export_button"
                                            OnClick="ui_export_button_Click"
                                            OnClientClick="AGLoading(true);" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>


                </div>

                <div id="search-panel" style="padding: 18px;">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdateTableCusDetail">
                        <ContentTemplate>
                            <table id="rep" class="table table-bordered table-striped table-hover table-sm">
                                <thead>
                                    <tr>
                                        <th class="text-nowrap">CI_Code</th>
                                        <th class="text-nowrap">CI_Name</th>
                                        <th class="text-nowrap">Previos Plant</th>
                                        <th class="text-nowrap">Previos Location</th>
                                        <th class="text-nowrap">Current Plant</th>
                                        <th class="text-nowrap">Current Location</th>
                                        <th class="text-nowrap">Owner Assognment</th>
                                    </tr>
                                </thead>
                                <%--<asp:Repeater runat="server" ID="LocationReportDetail">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#Eval("EquipmentCode")%></td>
                                            <td class="text-nowrap"><%#Eval("EquipmentName")%></td>
                                            <td><%#Eval("Previos_Plant")%></td>
                                            <td><%#Eval("Previos_Location")%></td>
                                            <td><%#Eval("Current_Plant")%></td>
                                            <td><%#Eval("Current_Location")%></td>
                                            <td class="text-nowrap"><%#Eval("OwnerAssognment")%></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>--%>
                            </table>
                            <div runat="server" id="divDataJson" class="d-none"></div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

    </div>
    <a id="download-report-excel" class="hide" target="_blank"
        href="<%= Page.ResolveUrl("~/API/ExportExcelAPI.ashx") %>"></a>
    <br />

    <script>
        //$(document).ready(function () {
        //    $('#rep').DataTable({});
        //});
        function exportExcelAPI() {
            $("#download-report-excel")[0].click();
        }
        function scrollToTable() {
            $('html,body').animate({
                scrollTop: $("#search-panel").offset().top - 50
            });
        }
        function afterSearch() {
            var reportsList = JSON.parse($("#<%= divDataJson.ClientID %>").html());
            var data = [];
            for (var i = 0 ; i < reportsList.length ; i++) {
                var report = reportsList[i];
                data.push([
                    report.EquipmentCode,
                    report.EquipmentName,
                    report.Previos_Plant,
                    report.Previos_Location,
                    report.Current_Plant,
                    report.Current_Location,
                    report.OwnerAssognment
                ]);
            }

            //$("#search-panel").show();
            $("#rep").dataTable({
                data: data,
                deferRender: true,
                columnDefs: [{
                    //"orderable": false,
                    "targets": [0, 1, 2, 3, 4],
                    "createdCell": function (td, cellData, rowData, row, col) {
                        $(td).closest("tr").addClass("text-nowrap");
                    }
                }]
            });
            scrollToTable();
        }
    </script>
</asp:Content>
