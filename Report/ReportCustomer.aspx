<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="ReportCustomer.aspx.cs" Inherits="ServiceWeb.Report.ReportCustomer" %>

<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteCustomer.ascx" TagPrefix="uc1" TagName="AutoCompleteCustomer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function initPage() {
            //ถ้าเป็น Tag ASP ให้ ใส่ ClientIDMode="Static" เข้าไปด้วยครับ
            var cd = new Date();
            console.log([cd.getMonth(), cd.getFullYear()]);
            document.getElementById("inp_month").value = cd.getMonth() + 1;
            document.getElementById("inp_year").value = cd.getFullYear();
        };
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-report-customer").className = "nav-link active";

            initPage();
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    <style>
        .btn-info {
        }

        #more {
            display: none;
        }

        #myBtn {
            color: cornflowerblue;
        }
    </style>
    <script>
        $(document).ready(function () {
            $('#tableItems').DataTable();
        });
    </script>
    <script>
        function myFunction() {
            var dots = document.getElementById("dots");
            var moreText = document.getElementById("more");
            var btnText = document.getElementById("myBtn");

            if (dots.style.display === "none") {
                dots.style.display = "inline";
                btnText.innerHTML = "Read more";
                moreText.style.display = "none";
            } else {
                dots.style.display = "none";
                btnText.innerHTML = "Read less";
                moreText.style.display = "inline";
            }
        }
    </script>
    <div>
        <div class="card">
            <div class="card-header">
                <h4>Report Client</h4>
            </div>
            <div class="card-body">
                <asp:UpdatePanel ID="udpForm" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-row">
                            <div class="col-4">
                                <div>
                                    <label>Client</label>
                                </div>
                                <div>
                                    <uc1:AutoCompleteCustomer runat="server" id="AutoCompleteCustomer" cssClass="form-control form-control-sm required" />
                                </div>
                            </div>
                            <div class="col-4">
                                <div>
                                    <label>Month</label>
                                </div>
                                <div>
                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="inp_month" ClientIDMode="Static">
                                        <asp:ListItem Value="" Text="" />
                                        <asp:ListItem Value="01" Text="January" />
                                        <asp:ListItem Value="02" Text="February" />
                                        <asp:ListItem Value="03" Text="March" />
                                        <asp:ListItem Value="04" Text="April" />
                                        <asp:ListItem Value="05" Text="May" />
                                        <asp:ListItem Value="06" Text="June" />
                                        <asp:ListItem Value="07" Text="July" />
                                        <asp:ListItem Value="08" Text="August" />
                                        <asp:ListItem Value="09" Text="September" />
                                        <asp:ListItem Value="10" Text="October" />
                                        <asp:ListItem Value="11" Text="November" />
                                        <asp:ListItem Value="12" Text="December" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-4">
                                <div>
                                    <label>Year</label>
                                </div>
                                <div>
                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="inp_year" ClientIDMode="Static" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                        ControlToValidate="inp_year" runat="server"
                                        ErrorMessage="Only Numbers allowed"
                                        ValidationExpression="\d+">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="udpButton" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-row">
                            <asp:Button runat="server" Text="Search" CssClass="btn btn-info" ID="btn_search" OnClick="btn_search_Click" OnClientClick="AGLoading(true);" />&nbsp;
                            <asp:Button runat="server" Text="Export" CssClass="btn btn-success" ID="btn_export" OnClick="btn_export_Click" OnClientClick="AGLoading(true);" />
                            <a id="download-report-excel" class="hide" target="_blank"
                                href="<%= Page.ResolveUrl("~/API/ExportExcelForReportCustomer.ashx") %>"></a>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <asp:UpdatePanel ID="udpCustomerDetail" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="margin-top: 8px;">
                    <table class="table table-borderless">
                        <tr>
                            <td>ชื่อบริษัท</td>
                            <td>
                                <asp:Label runat="server" ID="customerName_label" /></td>
                        </tr>
                        <tr>
                            <td>สถานที่</td>
                            <td>
                                <asp:Label runat="server" ID="address_label" /></td>
                        </tr>
                        <tr>
                            <td>ประเภทบริการ</td>
                            <td>
                                <asp:Label runat="server" ID="service_type_label" /></td>
                        </tr>
                        <tr>
                            <td>Carrier / Curcuit ID</td>
                            <td>
                                <asp:Label runat="server" ID="cc_label" /></td>
                        </tr>
                        <tr>
                            <td>รายงานประจำเดือน</td>
                            <td>
                                <asp:Label runat="server" ID="month_label" /></td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="search-panel">
            <asp:UpdatePanel ID="udpCustomer" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="table-responsive" style="margin-top: 8px;">
                        <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                            <thead>
                                <tr>
                                    <th class="text-nowrap">Incident No.</th>
                                    <th class="text-nowrap">Open Date</th>
                                    <th class="text-nowrap">Resolved Date</th>
                                    <th class="text-nowrap">Close Log</th>
                                    <th class="text-nowrap">Duration Time</th>
                                </tr>
                            </thead>
                            <%--<tbody>
                                <asp:Repeater ID="rtpReportCustomer" runat="server">
                                    <ItemTemplate>
                                        <tr class="table-hover">
                                            <td class="text-nowrap"><%# Eval("IncidentNO") %></td>
                                            <td class="text-nowrap"><%# Eval("Open_Date") %></td>
                                            <td class="text-nowrap"><%# Eval("Resolved_Date") %></td>
                                            <td class="text-nowrap"><%# Eval("Close_Log") %></td>
                                            <td class="text-nowrap"><%# Eval("Duration_Time") %></td>
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

    <script>
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
                    report.IncidentNO,
                    report.Open_Date,
                    report.Resolved_Date,
                    report.Close_Log,
                    report.Duration_Time
                ]);
            }

            //$("#search-panel").show();
            $("#tableItems").dataTable({
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
