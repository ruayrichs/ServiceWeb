<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="MasterRole.aspx.cs" Inherits="ServiceWeb.MasterConfig.MasterRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-master-permission").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    <div class="">
        <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
            <div class="pull-left">
                <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <button type="button" data-toggle="modal" data-target="#myModal" class="btn btn-primary mb-1">
                            <i class="fa fa-plus-circle"></i>&nbsp;&nbsp;New Severity
                        </button>
                        <button type="button" class="btn btn-success mb-1" onclick="$(this).next().click();">
                            <i class="fa fa-save"></i>&nbsp;&nbsp;Update Data
                        </button>
                        <asp:Button ID="btnRoleCode" OnClick="btnUpdate_Click" class="btn btn-success mb-1 d-none" Text="Update" runat="server" />
                        <%--<button type="button" ID="btnRoleCode" onClick="btnUpdate_Click" class="btn btn-success" runat="server" ><i class="fa fa-plus-circle"></i>&nbsp;&nbsp;Update</button>--%>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </nav>
        <br />
        <div class="row">
            <div class="col">
                <div class="card shadow">
                    <div class="card-header">
                        <h5 class="mb-0">Master Role</h5>
                    </div>
                    <div class="card-body">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updateTable">
                            <ContentTemplate>
                                <div class="table-responsive">
                                    <table id="tableSort" border="1" class="table table-bordered table-striped table-hover table-md dataTable no-footer">
                                        <thead>
                                            <tr>
                                                <th rowspan="2"></th>
                                                <th rowspan="2" style="width: 200px;">Role List</th>
                                                <th rowspan="2">Level Control</th>
                                                <th rowspan="2">Default Page</th>
                                                <th rowspan="2" class="text-center">AllPermission</th>
                                                <th rowspan="1" class="text-center">Home</th>
                                                <th rowspan="1" class="text-center text-nowrap">My Queue</th>
                                                <th rowspan="1" class="text-center text-nowrap">My Group</th>
                                                <th rowspan="1" class="text-center">Overdue</th>
                                                <th colspan="3" class="text-center">Dashboard</th>
                                                <th colspan="1" class="text-center">Calendar</th>
                                                <th colspan="2" class="text-center">Tier 0</th>
                                                <th colspan="1" class="text-center">Searching</th>
                                                <th colspan="2" class="text-center">Incident</th>
                                                <th colspan="2" class="text-center">Request</th>
                                                <th colspan="2" class="text-center">Problem</th>
                                                <th colspan="2" class="text-center">Change Order</th>
                                                <th colspan="3" class="text-center">Configuration Item</th>
                                                <th colspan="2" class="text-center">Contact</th>
                                                <th colspan="2" class="text-center">Knowledge<br />
                                                    Mannagement</th>
                                                <th colspan="10" class="text-center">Report</th>
                                                <%
                                                    if (IsFilterOwner == true)
                                                    {
                                                %>
                                                <th colspan="2" class="text-center">SLA</th>
                                                <th colspan="2" class="text-center">Incident Area</th>
                                                <th colspan="2" class="text-center">User Management</th>
                                                <th colspan="2" class="text-center">Role Config</th>
                                                <% } %>
                                            </tr>
                                            <tr>

                                                <%--<th>All</th>--%>
                                                <th class="text-center">View</th>

                                                <th class="text-center">View</th>

                                                <th class="text-center">View</th>

                                                <th class="text-center">View</th>
                                                <%--<th>Modify</th>--%>

                                                <th>View</th>
                                                <th>ViewAll</th>
                                                <th>Modify</th>

                                                <th>View</th>


                                                <th>View</th>
                                                <th>Modify</th>

                                                <th class="text-center">View</th>
                                               <%-- <th>Modify</th>--%>

                                                <th>View</th>
                                                <th>Modify</th>

                                                <th>View</th>
                                                <th>Modify</th>

                                                <th>View</th>
                                                <th>Modify</th>

                                                <th>View</th>
                                                <th>Modify</th>

                                                <th>View</th>
                                                <th>Modify</th>
                                                <th>Attributes</th>

                                                <th>View</th>
                                                <th>Modify</th>

                                                <th>View</th>
                                                <th>Modify</th>

                                                <th class="d-none">View</th>
                                                <th>Modify</th>
                                                
                                                <th>Export Data</th>
                                                <th>User Analyst</th>
                                                <th>CI Maintenance</th>
                                                <th>Report Client</th>
                                                <th>Report Location</th>
                                                <th>MTTN</th>
                                                <th>MTTR</th>
                                                <th>Ticket Report</th>
                                                <th>Ticket Analysis</th>

                                                <% if (IsFilterOwner == true)
                                                   { %>
                                                <th>View</th>
                                                <th>Modify</th>

                                                <th>View</th>
                                                <th>Modify</th>

                                                <th>View</th>
                                                <th>Modify</th>

                                                <th>View</th>
                                                <th>Modify</th>
                                                <%} %>
                                            </tr>
                                        </thead>
                                        <asp:Repeater ID="tableData" OnItemDataBound="tableData_ItemDataBound" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="text-nowrap">
                                                        <%--<asp:Button onClientClick="return AGConfirm(this,'ต้องการบันทึกหรือไม่ !!');AGLoading(true);" ID="btnRoleCode" CommandArgument='<%#Eval("RoleCode")%>' onClick="btnUpdate_Click" class="btn btn-success btn-sm" Text="Update" runat="server" />--%>
                                                        <asp:Button OnClientClick="return AGConfirm(this,'ต้องการลบหรือไม่ !!');AGLoading(true);" ID="btnDel" CommandArgument='<%#Eval("RoleCode")%>' OnClick="btnDelete_Click" class="btn btn-outline-danger btn-sm font-weight-bold" BorderStyle="None" Text="×" runat="server" />

                                                        <asp:HiddenField runat="server" ID="hddRoleCode" Value='<%# Eval("RoleCode")%>' />
                                                        &nbsp;&nbsp;&nbsp;
                                                        <%# Eval("RoleCode")%>
                                                    </td>
                                                    <td style="width: 200px;" class="text-nowrap">
                                                        <%# Convert.ToString(Eval("RoleName")) %>
                                                    </td>
                                                    
                                                    <td style="width: 200px;">                                                        
                                                        <asp:DropDownList style="width: 100px;" runat="server" ID="ddLevelControl" class="form-control-sm">
                                                            <asp:ListItem Text="- Select -" Value="0"/>
                                                            <asp:ListItem Text="LV.1" Value="1" 
                                                                alt="line 1&#013;line 2" 
                                                                title="ServiceCallTransaction-show-menu&#013;- Overview&#013;- Configuration Item&#013;- Comment
                                                                &#013;ServiceCallTransaction-delete&#013;- Button Close Ticket
                                                                &#013;ServiceCallTransaction-disabled&#013;- Ticket Status
                                                                "/>
                                                            <asp:ListItem Text="LV.2" Value="2" 
                                                                alt="line 1&#013;line 2" 
                                                                title="ServiceCallTransaction-Edit&#013;- Impact&#013;- Urgency
                                                                "/>
                                                            <%--<asp:ListItem Text="LV.99" Value="99"/>--%>
                                                        </asp:DropDownList>
                                                    </td>

                                                    <td style="width: 200px;">                                                        
                                                        <asp:DropDownList style="width: 200px;" runat="server" ID="ddDefaultPage" class="form-control-sm">                                                                                                                 
                                                            <asp:ListItem Text="Home" Value="/Default.aspx"/>
                                                            <asp:ListItem Text="My Ticket" Value="/MyTicket.aspx"/>
                                                        </asp:DropDownList>
                                                    </td>

                                                    <td>
                                                        <asp:CheckBox ID="cbAllPermission" runat="server" Checked='<%# Convert.ToBoolean(Eval("AllPermission")) %>' />

                                                    </td>

                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("HomeView")) %>' ID="cbHomeView" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("MyQueueView")) %>' ID="cbMyQueueView" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("MyGroupView")) %>' ID="cbMyGroupView" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("MyOverDueView")) %>' ID="cbMyOverDueView" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("DashboardView")) %>' ID="cbDashBoardView" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("DashboardViewAll")) %>' ID="cbDashBoardViewAll" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("DashboardModify")) %>' ID="cbDashBoardModify" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("CalendarView")) %>' ID="cbCalendarView" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("TierZeroView")) %>' ID="cbTierZeroView" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("TierZeroModify")) %>' ID="cbTierZeroModify" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("SearchView")) %>' ID="cbSearchView" />
                                                    </td>                                              
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("IncidentView")) %>' ID="cbIncidentView" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("IncidentModify")) %>' ID="cbIncidentModify" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("RequestView")) %>' ID="cbRequestView" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("RequestModify")) %>' ID="cbRequestModify" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ProblemView")) %>' ID="cbProblemView" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ProblemModify")) %>' ID="cbProblemModify" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ChangeOrderView")) %>' ID="cbChangeOrderView" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ChangeOrderModify")) %>' ID="cbChangeOrderModify" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ConfigurationItemView")) %>' ID="cbConfigurationItemView" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ConfigurationItemModify")) %>' ID="cbConfigurationItemModify" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Eval("ConfigurationItemAttributes") == DBNull.Value ? Convert.ToBoolean("False") : Convert.ToBoolean(Eval("ConfigurationItemAttributes")) %>' ID="cbConfigurationItemAttributes" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ContactView")) %>' ID="cbContactView" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ContactModify")) %>' ID="cbContactModify" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("KM_View")) %>' ID="cbKM_View" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("KM_Modify")) %>' ID="cbKM_Modify" />
                                                    </td>
                                                    <td class="d-none">
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ReportView")) %>' ID="cbReportView" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ReportModify")) %>' ID="cbReportModify" />
                                                    </td>
                                                    
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ReportExportData")) %>' ID="cbReportExportData" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ReportUserAnalyst")) %>' ID="cbReportUserAnalyst" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ReportCIMaintenance")) %>' ID="cbReportCIMaintenance" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ReportReportClient")) %>' ID="cbReportReportClient" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ReportReportLocation")) %>' ID="cbReportReportLocation" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ReportMTTN")) %>' ID="cbReportMTTN" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ReportMTTR")) %>' ID="cbReportMTTR" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ReportTicketReport")) %>' ID="cbReportTicketReport" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("ReportTicketAnalysis")) %>' ID="cbReportTicketAnalysis" />
                                                    </td>

                                                    <td class="<%# IsFilterOwner ? "" : "d-none" %>">
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("SLAView")) %>' ID="cbSLAView" />
                                                    </td>
                                                    <td class="<%# IsFilterOwner ? "" : "d-none" %>">
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("SLAModify")) %>' ID="cbSLAModify" />
                                                    </td>
                                                    <td class="<%# IsFilterOwner ? "" : "d-none" %>">
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("IncidentAreaView")) %>' ID="cbIncidentAreaView" />
                                                    </td>
                                                    <td class="<%# IsFilterOwner ? "" : "d-none" %>">
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("IncidentAreaModify")) %>' ID="cbIncidentAreaModify" />
                                                    </td>
                                                    <td class="<%# IsFilterOwner ? "" : "d-none" %>">
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("UserManagementView")) %>' ID="cbUserManagementView" />
                                                    </td>
                                                    <td class="<%# IsFilterOwner ? "" : "d-none" %>">
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("UserManagementModify")) %>' ID="cbUserManagementModify" />
                                                    </td>
                                                    <td class="<%# IsFilterOwner ? "" : "d-none" %>">
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("RoleConfigView")) %>' ID="cbRoleConfigView" />
                                                    </td>
                                                    <td class="<%# IsFilterOwner ? "" : "d-none" %>">
                                                        <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("RoleConfigModify")) %>' ID="cbRoleConfigModify" />
                                                    </td>
                                                </tr>

                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="myModal" class="modal fade dialog-lg" role="dialog">
                            <div class="modal-dialog">
                                <!-- Modal content-->
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h4 class="modal-title">Modal Header</h4>
                                        <button type="button" class="close" data-dismiss="modal">×</button>
                                    </div>
                                    <div class="modal-body">

                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updateNewRole">
                                            <ContentTemplate>
                                                <div class="form-group row">
                                                    <label for="plant-input" class="col-6 col-form-label">Role Code</label>
                                                    <div class="col-12">
                                                        <asp:TextBox runat="server" ID="txBoxRoleCode" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group row">
                                                    <label for="plant-input" class="col-6 col-form-label">Role Name</label>
                                                    <div class="col-12">
                                                        <asp:TextBox runat="server" ID="txBoxRoleName" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group row">
                                                    <label for="plant-input" class="col-6 col-form-label">Default Page</label>
                                                    <div class="col-12">                                                        
                                                        <asp:DropDownList ID="drDownDefaultPage" runat="server" class="form-control">
                                                            <asp:ListItem Text="Home" Value="/Default.aspx"/>
                                                            <asp:ListItem Text="My Ticket" Value="/MyTicket.aspx"/>                                                            
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="btnButtonLocationPopup">
                                            <ContentTemplate>
                                                <asp:Button Text="New" OnClientClick="return AGConfirm(this,'ต้องการบันทึกหรือไม่ !!');AGLoading(true);" OnClick="btnSave_Click" runat="server" ID="btnNew" class="btn btn-success" />


                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#tableSort').DataTable();
        });
    </script>
</asp:Content>
