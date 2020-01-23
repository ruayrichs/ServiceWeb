<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="ExportDataReport.aspx.cs" Inherits="ServiceWeb.Report.ExportDataReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-export-data").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    
    <div class="card shadow">
        <div class="card-header">
            <h5 class="mb-0">Export Datas</h5>
        </div>
        <div class="card-body PANEL-DEFAULT-BUTTON">
            <div class="row">
                <div class="col-12">
                    <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <a href="Javascript:;" onclick="$(this).next().click();">
                                <div class="alert alert-info">
                                    <div class="row">
                                        <div class="col-12">
                                            <h1 style="margin: 0;">
                                                <i class="fa fa-download fa-1x fa-fw"></i>
                                                Export Customer Contact
                                            </h1>
                                        </div>
                                    </div>
                                </div>
                            </a>
                            <asp:Button runat="server" CssClass="btn btn-success d-none" Text="Export Data" ID="btnExportDataCustomer"
                                OnClick="btnExportDataCustomer_Click" OnClientClick="AGLoading(true);" />
                            <asp:HiddenField runat="server" ID="hddExportCustomerExclude" Value="C04,C05" />
                        </ContentTemplate>
                    </asp:UpdatePanel>


                </div>
            </div>
        </div>
    </div>
    <a id="download-report-excel" class="hide" target="_blank"
        href="<%= Page.ResolveUrl("~/API/ExportExcelAPI.ashx") %>"></a>
    <script>
        function exportExcelAPI() {
            $("#download-report-excel")[0].click();
        }
    </script>
</asp:Content>
