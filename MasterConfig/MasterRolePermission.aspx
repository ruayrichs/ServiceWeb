<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="MasterRolePermission.aspx.cs" Inherits="ServiceWeb.MasterConfig.MasterRolePermission" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <style>
        .divbtn{
            padding-bottom:16px;
        }
    </style>
    <script>
        function afterLoad() {
            $('#tableSort').DataTable();
        }
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            try {
                document.getElementById("nav-menu-master-role-permission").className = "nav-link active";
            } catch (err) {

            }
            afterLoad();            
        };
        $(document).ready(function () { webOnLoad(); })

        function prepare_update_btn_click() {
            document.getElementById('<%= update_btn.ClientID %>').click();
        };
    </script>
    <div id="body">
        <div class="divbtn">
            <asp:UpdatePanel ID="udpButton" runat="server">
                <ContentTemplate>
                    <button type="button" class="btn btn-primary"><i class="fa fa-plus-circle"></i>&nbsp;&nbsp;New Severity</button>
                    <button type="button" id="prepare_update_btn" class="btn btn-success" onclick="prepare_update_btn_click();"><i class="fa fa-save"></i>&nbsp;&nbsp;Update Data</button>
                    <asp:Button runat="server" CssClass="btn btn-success d-none" Text="Update Data" ID="update_btn" OnClick="update_btn_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="card shadow">
            <div class="card-header">
                <h5>Master Role Permission</h5>
            </div>
            <div class="card-body">
                <asp:UpdatePanel ID="udpTable" runat="server">
                    <ContentTemplate>
                        <div class="table-responsive-md">
                            <table id="tableSort"  border="1" class="table table-bordered table-striped table-hover table-md dataTable no-footer">
                                <thead>
                                    <tr>
                                        <th rowspan="2">Role Code</th>
                                        <th rowspan="2" style="width: 200px;">Role List</th>
                                        <th rowspan="2" colspan="1">AllPermission</th>
                                        <th colspan="2">SLA</th>
                                        <th colspan="2">Incident Area</th>
                                        <th colspan="2">User Management</th>
                                        <th colspan="2">Role Config</th>
                                    </tr>
                                    <tr>
                                        <%--<th>All</th>--%>
                                        <%--<th></th>
                                        <th></th>--%>
                                        
                                        <th>View</th>
                                        <th>Modify</th>
                                        
                                        <th>View</th>
                                        <th>Modify</th>
                                        
                                        <th>View</th>
                                        <th>Modify</th>
                                        
                                        <th>View</th>
                                        <th>Modify</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater runat="server" ID="itemsRPT">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:HiddenField runat="server" ID="hddRoleCode" Value='<%# Eval("RoleCode")%>' />
                                                    <label><%# Eval("RoleCode")%></label>
                                                </td>
                                                <td>
                                                    <label><%# Convert.ToString(Eval("RoleName")) %></label>
                                                </td>

                                                <td>
                                                    <asp:CheckBox ID="cbAllPermission" runat="server" Checked='<%# Convert.ToBoolean(Eval("AllPermission")) %>'  />
                                                </td>

                                                <div class="d-none">
                                                    <asp:CheckBox runat="server" Checked='<%# Convert.ToBoolean(Eval("DashboardView")) %>' ID="cbDashBoardView"  />
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("DashboardModify")) %>' ID="cbDashBoardModify"  />
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("TierZeroView")) %>' ID="cbTierZeroView"  />
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("TierZeroModify")) %>' ID="cbTierZeroModify"  />
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("IncidentView")) %>' ID="cbIncidentView"  />
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("IncidentModify")) %>' ID="cbIncidentModify"  />
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("RequestView")) %>' ID="cbRequestView"  />
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("RequestModify")) %>' ID="cbRequestModify"  />
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("ProblemView")) %>' ID="cbProblemView"  />
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("ProblemModify")) %>' ID="cbProblemModify"  />
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("ChangeOrderView")) %>' ID="cbChangeOrderView" />
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("ChangeOrderModify")) %>' ID="cbChangeOrderModify"  />
                                                        
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("ConfigurationItemView")) %>' ID="cbConfigurationItemView"  />
                                                        
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("ConfigurationItemModify")) %>' ID="cbConfigurationItemModify"  />
                                                        
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("ContactView")) %>' ID="cbContactView" />
                                                        
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("ContactModify")) %>' ID="cbContactModify"  />
                                                        
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("KM_View")) %>' ID="cbKM_View"  />
                                                        
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("KM_Modify")) %>' ID="cbKM_Modify" />
                                                        
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("ReportView")) %>' ID="cbReportView"  />
                                                        
                                                    <asp:CheckBox runat="server" CssClass="d-none" Checked='<%# Convert.ToBoolean(Eval("ReportModify")) %>' ID="cbReportModify"  />

                                                </div>

                                                <td>
                                                    <asp:CheckBox  runat="server" Checked='<%# Convert.ToBoolean(Eval("SLAView")) %>' ID="cbSLAView"  />
                                                </td>
                                                <td>
                                                    <asp:CheckBox  runat="server" Checked='<%# Convert.ToBoolean(Eval("SLAModify")) %>' ID="cbSLAModify"  />
                                                </td>

                                                <td>
                                                    <asp:CheckBox  runat="server" Checked='<%# Convert.ToBoolean(Eval("IncidentAreaView")) %>' ID="cbIncidentAreaView"  />
                                                </td>
                                                <td>
                                                    <asp:CheckBox  runat="server" Checked='<%# Convert.ToBoolean(Eval("IncidentAreaModify")) %>' ID="cbIncidentAreaModify"  />
                                                </td>

                                                <td>
                                                    <asp:CheckBox  runat="server" Checked='<%# Convert.ToBoolean(Eval("UserManagementView")) %>' ID="cbUserManagementView"  />
                                                </td>
                                                <td>
                                                    <asp:CheckBox  runat="server" Checked='<%# Convert.ToBoolean(Eval("UserManagementModify")) %>' ID="cbUserManagementModify"  />
                                                </td>

                                                <td>
                                                    <asp:CheckBox  runat="server" Checked='<%# Convert.ToBoolean(Eval("RoleConfigView")) %>' ID="cbRoleConfigView"  />
                                                </td>
                                                <td>
                                                    <asp:CheckBox  runat="server" Checked='<%# Convert.ToBoolean(Eval("RoleConfigModify")) %>' ID="cbRoleConfigModify"  />
                                                </td>
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
</asp:Content>
