<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="PivotTableReportMaster.aspx.cs" Inherits="ServiceWeb.Report.PivotTableReportMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card border-info mb-3">
        <div class="card-header bg-info text-white">
            <b>Report list</b>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <table class="table table-sm table-hover table-striped" style="margin-bottom: 0;">
                    <thead>
                        <tr>
                            <th class="" style="width:50px;">View</th>
                            <th class="col-date">Report Type</th>
                            <th class="col-ticket-type">Report Name</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr class="c-pointer"  onclick="RedirectPageLinkToDetail('id=V20180809160846969001FOCUSONE&type=pivot');">
                            <td class="text-center">
                                <a href="PivotTableReport.aspx?id=V20180809160846969001FOCUSONE&type=pivot">
                                    <i class="fa fa-table"></i>
                                </a>
                            </td>
                            <td>Pivot Table</td>
                            <td>View Service Ticket and Average Troubleshooting Time (Minute) By Client</td>
                        </tr>
                        <tr class="c-pointer"  onclick="RedirectPageLinkToDetail('id=V20180809160702104001FOCUSONE&type=pivot');">
                            <td class="text-center">
                                <a href="PivotTableReport.aspx?id=V20180809160702104001FOCUSONE&type=pivot">
                                    <i class="fa fa-table"></i>
                                </a>
                            </td>
                            <td>Pivot Table</td>
                            <td>View Service Ticket and Average Troubleshooting Time (Minute) By Configuration Item</td>
                        </tr>
                        <tr class="c-pointer"  onclick="RedirectPageLinkToDetail('id=V20180809155804361001FOCUSONE&type=pivot');">
                            <td class="text-center">
                                <a href="PivotTableReport.aspx?id=V20180809155804361001FOCUSONE&type=pivot">
                                    <i class="fa fa-table"></i>
                                </a>
                            </td>
                            <td>Pivot Table</td>
                            <td>View Service Ticket By Client</td>
                        </tr>
                        <tr class="c-pointer"  onclick="RedirectPageLinkToDetail('id=V20180809161100365001FOCUSONE&type=pivot');">
                            <td class="text-center">
                                <a href="PivotTableReport.aspx?id=V20180809161100365001FOCUSONE&type=pivot">
                                    <i class="fa fa-table"></i>
                                </a>
                            </td>
                            <td>Pivot Table</td>
                            <td>View Service Ticket By Configuration Item Class : Month</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

    <script>
        function RedirectPageLinkToDetail(paramstring)
        {
            var dataKey = "";
            var Question = "";
            if (paramstring != null && paramstring != undefined && paramstring != "")
            {
                Question = "?";
            }
            var url = 'PivotTableReport.aspx' + Question + paramstring;
            var form = $('<form action="' + url + '" method="post">' +
              '<input type="text" name="api_url" value="' + dataKey + '" />' +
              '</form>');
            $('body').append(form);
            form.submit();
        }
    </script>
</asp:Content>
