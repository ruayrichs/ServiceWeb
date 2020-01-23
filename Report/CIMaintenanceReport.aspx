<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="CIMaintenanceReport.aspx.cs" Inherits="ServiceWeb.MasterPage.CIMaintenanceReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-ci-maintenance").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Warranty</h5>
                </div>
                <div class="card-body PANEL-DEFAULT-BUTTON">

                    <div class="form-row">

                        <div class="col-lg-6">
                            <div class="card border-primary">
                                <div class="card-body card-body-sm">
                                    <div class="form-row">
                                        <div class="form-group col-sm-12">
                                            <label>CI Code</label>
                                            <div class="input-group">
                                                <asp:TextBox runat="server" CssClass="form-control form-control-sm"
                                                    ID="txtCICode"
                                                    placeholder="CI Code" />
                                            </div>
                                        </div>
                                        <div class="form-group col-sm-12">
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
                                                <div class="form-group col-sm-12">
                                                    <label>Maintenance Type</label>
                                                    <asp:DropDownList ID="ddlMaintenanceType" CssClass="form-control form-control-sm" runat="server">
                                                        <asp:ListItem Text="เลือก" Value="00"></asp:ListItem>
                                                        <asp:ListItem Text="Premium" Value="01"></asp:ListItem>
                                                        <asp:ListItem Text="Medium" Value="02"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-6">
                            <div class="card border-primary">
                                <div class="card-body card-body-sm">
                                    <div class="form-row">
                                        <div class="form-group col-lg-6 col-sm-12">
                                            <label>Maintenance Start Date</label>
                                            <div class="input-group">
                                                <asp:TextBox runat="server" onchange="$('#btnWarrantyNextMaintenanceRefStartDate').click();" CssClass="form-control form-control-sm date-picker"
                                                    ID="txtMaintenanceStartDateFrom"
                                                    placeholder="dd/mm/yyy" />
                                                <span class="input-group-append hand">
                                                    <i class="fa fa-calendar input-group-text"></i>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="form-group col-lg-6 col-sm-12">
                                            <label>To</label>
                                            <div class="input-group">
                                                <asp:TextBox runat="server" onchange="$('#btnWarrantyNextMaintenanceRefEndDate').click();" CssClass="form-control form-control-sm date-picker"
                                                    ID="txtMaintenanceStartDateTo"
                                                    placeholder="dd/mm/yyy" />
                                                <span class="input-group-append hand">
                                                    <i class="fa fa-calendar input-group-text"></i>
                                                </span>
                                            </div>

                                        </div>
                                        <div class="col-lg-12">
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpContactDetail">
                                                <ContentTemplate>
                                                    <div class="form-row">
                                                        <div class="form-group col-lg-6 col-sm-12">
                                                            <label>Maintenance End Date</label>
                                                            <div class="input-group">
                                                                <asp:TextBox runat="server" onchange="$('#btnWarrantyNextMaintenanceRefStartDate').click();" CssClass="form-control form-control-sm date-picker"
                                                                    ID="txtMaintenanceEndDateFrom"
                                                                    placeholder="dd/mm/yyy" />
                                                                <span class="input-group-append hand">
                                                                    <i class="fa fa-calendar input-group-text"></i>
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <div class="form-group col-lg-6 col-sm-12">
                                                            <label>To</label>
                                                            <div class="input-group">
                                                                <asp:TextBox runat="server" onchange="$('#btnWarrantyNextMaintenanceRefEndDate').click();" CssClass="form-control form-control-sm date-picker"
                                                                    ID="txtMaintenanceEndDateTo"
                                                                    placeholder="dd/mm/yyy" />
                                                                <span class="input-group-append hand">
                                                                    <i class="fa fa-calendar input-group-text"></i>
                                                                </span>
                                                            </div>
                                                        </div>

                                                        <div class="form-group col-sm-12">
                                                            <label>Next Maintenance Due Date</label>
                                                            <div class="input-group">
                                                                <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker"
                                                                    ID="txtNextMaintenanceDate"
                                                                    placeholder="dd/mm/yyy" />
                                                                <span class="input-group-append hand">
                                                                    <i class="fa fa-calendar input-group-text"></i>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="padding: 0px 8px;">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpUpdate">
                            <ContentTemplate>
                                <br />
                                <asp:Button Text="Search" runat="server" ID="Button1"
                                    CssClass="btn btn-info DEFAULT-BUTTON-CLICK"
                                    OnClick="btn_search"
                                    OnClientClick="AGLoading(true);" />
                                <asp:Button Text="Export Data" runat="server" ID="btnExpoet"
                                    CssClass="btn btn-success "
                                    OnClick="btnExport_Click" OnClientClick="AGLoading(true);" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div id="search-panel" style="padding: 18px;">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdateTableCusDetail">
                        <ContentTemplate>
                            <table id="rep" class="table table-striped table-bordered table-hover table-s" style="width: 400px">
                                <thead>
                                    <tr>
                                        <th class="text-nowrap">CI_Code</th>
                                        <th class="text-nowrap">CI_Name</th>
                                        <th class="text-nowrap">Owner Assignment</th>
                                        <th class="text-nowrap">Maintenance Start Date</th>
                                        <th class="text-nowrap">Maintenance End Date</th>
                                        <th class="text-nowrap">Maintenance Type</th>
                                        <th class="text-nowrap">Next Maintenance Due Date</th>
                                        <th class="text-nowrap">Last Maintenance Date</th>
                                    </tr>
                                </thead>
                                <%--<asp:Repeater runat="server" ID="TableCusDetail">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#Eval("EquipmentCode")%></td>
                                            <td class="text-nowrap"><%#Eval("Description")%></td>
                                            <td><%#Eval("FirstName")+ " " + Eval("LastName")%></td>
                                            <td><%# Eval("BeginGuarantee") %></td>
                                            <td><%# Eval("EndGuaranty") %></td>
                                            <td><%# Eval("CategoryCode")%></td>
                                            <td><%# Eval("NextMaintenanceDate") %></td>
                                            <td><%# Eval("LastMaintenanceDate") %></td>
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
                    report.Description,
                    report.FirstName + ' ' + report.LastName,
                    report.BeginGuarantee,
                    report.EndGuaranty,
                    report.CategoryCode,
                    report.NextMaintenanceDate,
                    report.LastMaintenanceDate
                ]);
            }

            //$("#search-panel").show();
            $("#rep").dataTable({
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
    </script>

</asp:Content>
