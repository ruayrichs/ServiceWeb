<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="CostCenterCriteria.aspx.cs" Inherits="ServiceWeb.Master.CostCenterCriteria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <a href="CostCenterMaster.aspx" class="btn btn-primary mb-1 AUTH_MODIFY"><i class="fa fa-file-o"></i>&nbsp;&nbsp;Create</a>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>

    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">BOM Cost Sheet Criteria</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpListData">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table class="table table-bordered table-striped table-hover table-sm nowrap" id="table-list">
                                    <thead>
                                        <tr>
                                            <th style="width: 10px;"></th>
                                            <th>Code</th>
                                            <th>Subject</th>
                                            <th>Remark</th>
                                            <th style="width: 60px;">Contract Month</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater runat="server" ID="rptListData">
                                            <ItemTemplate>
                                                <tr class="c-pointer" data-key="<%# Eval("CostCenterID") %>">
                                                    <td class="text-center">
                                                        <%--href="CostCenterMaster.aspx?id=<%# Eval("CostCenterID") %>"--%>
                                                        <a>
                                                            <i class="fa fa-pencil"></i>
                                                        </a>
                                                    </td>
                                                    <td><%# Eval("CostCenterID") %></td>
                                                    <td><%# Eval("Subject") %></td>
                                                    <td><%# Eval("Remark") %></td>
                                                    <td class="text-right"><%# Eval("ContractMonth") %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>    

    <script>
        function initDataTable() {
            $("#table-list").dataTable({
                columnDefs: [{
                    "orderable": false,
                    "targets": [0],
                    'createdCell': function (td, cellData, rowData, row, col) {
                        $(td).addClass("text-center text-nowrap");
                        $(td).closest("tr").addClass("c-pointer");
                        $(td).closest("tr").bind("click", function () {
                            var dataKey = $(td).closest("tr").data("key");
                            var url = 'CostCenterMaster.aspx?id=' + dataKey;
                            var form = $('<form action="' + url + '" method="post">' +
                              '<input type="text" name="api_url" value="' + dataKey + '" />' +
                              '</form>');
                            $('body').append(form);
                            form.submit();
                        });
                    }
                }],
                "order": [[1, "asc"]]
            });
        }
    </script>

</asp:Content>
