<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeLogControl.ascx.cs" Inherits="ServiceWeb.UserControl.ChangeLogControl" %>

<script>
    function afterSearchChangLog() {
        $("#tableChangeLogItems").dataTable({
            "order": [[2, "desc"], [3, "desc"]],
            'columnDefs': [
                {                    
                    'targets': 2,
                    'createdCell': function (td, cellData, rowData, row, col) {
                        var dataDB = cellData.substring(0, 8);
                        var dataDisplay = dataDB.substring(6, 8) + "/" + dataDB.substring(4, 6) + "/" + dataDB.substring(0, 4);

                        $(td).html(dataDisplay);
                    }
                },
                {                    
                    'targets': 3,
                    'createdCell': function (td, cellData, rowData, row, col) {
                        var timeDB = cellData.substring(8, 14);
                        var timeDisplay = timeDB.substring(0, 2) + ":" + timeDB.substring(2, 4) + ":" + timeDB.substring(4, 6);

                        $(td).html(timeDisplay);
                    }
                }
            ]
        });
    }
</script>

<div>
    <style>
        #tableChangeLogItems .column-auto {
            width:auto!important;
        }
    </style>
    <asp:UpdatePanel ID="udpnChangeLog" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="table-responsive">
                <table id="tableChangeLogItems" class="table table-bordered table-striped table-hover table-sm">
                    <thead>
                        <tr>
                            <th class="text-nowrap">Event</th>
                            <th class="text-nowrap">Access By</th>
                            <th class="text-nowrap">Access Data</th>
                            <th class="text-nowrap">Access Time</th>
                            <th class="text-nowrap">Column Name</th>
                            <th class="column-auto">Old Value</th>
                            <th class="column-auto">New Value</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rptChangeLog">
                            <ItemTemplate>
                                <tr>
                                    <td class="text-nowrap">
                                        <%# Eval("accesscode") %>
                                    </td>
                                    <td class="text-nowrap">
                                        <%# Eval("access_by") %>
                                    </td>
                                    <td class="text-nowrap">
                                        <%# Eval("DateTime_DB") %>
                                    </td>
                                    <td class="text-nowrap">
                                        <%# Eval("DateTime_DB") %>
                                    </td>
                                    <td class="text-nowrap">
                                        <%# Eval("sfieldname") %>
                                    </td>
                                    <td><%# Eval("soldvalue") %></td>
                                    <td><%# Eval("snewvalue") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
