<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="AssetMasterCriteria.aspx.cs" Inherits="ServiceWeb.crm.Master.Asset.AssetMasterCriteria" %>

<%--<%@ Register Src="~/widget/usercontrol/AssetAgendaCtrl.ascx" TagPrefix="uc1" TagName="AssetAgendaCtrl" %>--%>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEmployee.ascx" TagPrefix="uc1" TagName="AutoCompleteEmployee" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .card-body-sm {
            padding: 1rem;
            padding-bottom: 0;
        }
        .input-group > .input-group-main-employee-ddlOwner {
            padding: 0px 0px!important;
        }
        .delete-icon {
            font-size : 17px!important;
        }
    </style>
    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Asset Master Criteria</h5>
                </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="col-lg-12">
                            <div class="card border-primary" style="margin-bottom: 10px;">
                                <div class="card-body card-body-sm">
                                        <div class="form-row">
                                            <div class="form-group col-lg-6">
                                                <label>Asset Group</label>
                                                <asp:DropDownList ID="ddlAssetGroup" CssClass="form-control form-control-sm required" ClientIDMode="Static" runat="server" DataTextField="Description" DataValueField="AssetGroup">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group col-lg-6">
                                                <label> Branch</label>
                                                <asp:DropDownList ID="ddlBranch" CssClass="form-control form-control-sm required" ClientIDMode="Static" runat="server" DataTextField="Description" DataValueField="BusinessAreaCode">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                             <div class="form-group col-lg-6">
                                                <label> Asset Type</label>
                                                <asp:DropDownList ID="ddlAssetType" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="GroupName" DataValueField="GroupCode">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group col-lg-6">
                                                <label>Asset Category 1</label>
                                                <asp:DropDownList ID="ddlAssetCategory1" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="Description" DataValueField="AssetCategory">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                           <div class="form-group col-lg-6">
                                                <label>Asset Code</label>
                                                <asp:TextBox ID="txtAssetCode" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" />
                                            </div>
                                            <div class="form-group col-lg-6">
                                                <label>Asset Category 2</label>
                                                <asp:DropDownList ID="ddlAssetCategory2" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="Description" DataValueField="AssetCategory">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                             <div class="form-group col-lg-6">
                                                <label>Asset Name</label>
                                                <asp:TextBox ID="txtAssetName" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" />
                                            </div>
                                            <div class="form-group col-lg-6">
                                                <label>Location 1</label>
                                                <asp:DropDownList ID="ddlLocation1" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="Description" DataValueField="AssetLocation">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                             <div class="form-group col-lg-6">
                                                <label>Owner</label>
                                                 <uc1:AutoCompleteEmployee runat="server" id="ddlOwner" CssClass="form-control form-control-sm" />
                                            </div>
                                           <div class="form-group col-lg-6">
                                                <label>Location 2</label>
                                                <asp:DropDownList ID="ddlLocation2" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="Description" DataValueField="AssetLocation2">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                             <div class="form-group col-lg-6">
                                                <label>Department</label>
                                                <asp:DropDownList ID="ddlDepartment" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="departmentName" DataValueField="departmentCode">
                                                </asp:DropDownList>
                                            </div>
                                           <div class="form-group col-lg-6">
                                                <label>Room</label>
                                                <asp:DropDownList ID="ddlRoom" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="Description" DataValueField="AssetRoom">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="form-group col-lg-6">
                                                <label>Status</label>
                                                <asp:DropDownList ID="ddlStatus" CssClass="form-control form-control-sm" runat="server">
                                                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Active" Value="True" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="In-Active" Value="False"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                  
                                </div>
                            </div>
                        </div>
                    </div>

                    <button type="button" class="btn btn-info" onclick="inactiveRequireField();$('#<%= btnSearch.ClientID %>').click();"><i class="fa fa-search"></i>&nbsp;&nbsp;Search</button>
                    <button type="button" class="btn btn-success" onclick="activeRequireField();$('#<%= btnCreate.ClientID %>').click();"><i class="fa fa-file-o"></i>&nbsp;&nbsp;Create</button>
                    <button type="button" id="btnWarningCreate" class="d-none" onclick="warningCreate(this);"></button>

                    <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional" class="d-none">
                        <ContentTemplate>
                            <asp:Button ID="btnSearch" Text="Search" CssClass="btn btn-info" OnClientClick="AGLoading(true);" OnClick="btnSearch_Click" runat="server" />&nbsp&nbsp&nbsp
                            <asp:Button ID="btnCreate" Text="Create" CssClass="btn btn-success AUTH_CREATE" OnClick="btnCreate_Click" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                     <div id="divSearch" style="display: none;">

                        <hr />

                        <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="table-responsive">
                                    <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                        <thead>
                                            <tr>
                                                <th class="text-nowrap">Edit</th>
                                                <th class="text-nowrap">Delete</th>
                                                <th class="text-nowrap">Asset Group</th>
                                                <th class="text-nowrap">Asset Type</th>
                                                <th class="text-nowrap">Asset Code</th>
                                                <th class="text-nowrap">Asset Name</th>
                                                <th class="text-nowrap">Asset Value</th>
                                                <th class="text-nowrap">Currency</th>
                                                <th class="text-nowrap">Asset Owner</th>
                                                <th class="text-nowrap">Asset Status</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="gvAssetMaster" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td class="text-nowrap text-center">
                                                            <asp:LinkButton ID="LinkButton1" CssClass="fa fa-pencil-square text-dark AUTH_UPDATE" OnClick="Edit_ItemGV"
                                                                 runat="server" CommandName='<%# Eval("AssetCode") %>' ToolTip="Edit" />
                                                        </td>
                                                        <td class="text-nowrap text-center">
                                                             <asp:LinkButton ID="btnDelete" CssClass="fa fa-times-circle-o delete-icon AUTH_DELETE" OnClientClick="if(confirm('ยืนยันการยกเลิก')){ AGLoading(true); } else { return false; };" OnClick="Delete_ItemGV"
                                                                 runat="server" CommandName='<%# Eval("AssetCode") %>' ToolTip="In-Active" Visible='<%# Convert.ToBoolean(Eval("AssetStatus")) %>' />
                                                        </td>
                                                        <td class="text-nowrap">
                                                            <%# Eval("AssetGroup") + " : " + Eval("AssetGroupName") %>
                                                        </td>
                                                        <td class="text-nowrap">
                                                            <%# Eval("AssetType") + " : " + Eval("AssetTypeName") %>
                                                        </td>
                                                        <td class="text-nowrap">
                                                            <%# Eval("AssetCode") %>
                                                        </td>
                                                        <td class="text-truncate" style="max-width: 500px;">
                                                            <span title="<%# Eval("AssetSubCodeDescription") %>"><%# Eval("AssetSubCodeDescription") %></span>
                                                        </td>
                                                        <td class="text-nowrap text-right">
                                                             <%# convertToFormatNumber(Eval("NetValue")) %>
                                                        </td>
                                                        <td class="text-nowrap">
                                                            <%# Eval("CURRENCYCODE") %>
                                                        </td>
                                                        <td class="text-nowrap">
                                                             <%# String.IsNullOrEmpty(Eval("FirstName_TH").ToString()) ? Eval("AssetOwner") : Eval("AssetOwner") + " : " + Eval("FirstName_TH") + " " + Eval("LastName_TH") %>
                                                        </td>
                                                        <td class="text-nowrap">
                                                             <%# Eval("AssetStatus").ToString().ToLower() == "true" ? "Active" : "In-Active" %>
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
        </div>
    </div>

    <script>

        //function openModalAssetCalendar(btn) {
        //    var assetCode = $(btn).attr("data-asset-code");
        //    var assetName = $(btn).attr("data-asset-name");
        //    AGLoading(true);
        //    $("#modal-asset-calendar").modal('show');
        //    setTimeout(function () {
        //        restartAssetCalendar(assetCode, assetName);
        //    }, 1000);
        //}

        function afterSearch() {
            $("#divSearch").show();
            $("#tableItems").dataTable({
                columnDefs: [{
                    "orderable": false,
                    "targets": [0]
                }],
                "order": [[3, "desc"]]
            });

            $('html,body').animate({
                scrollTop: $("#divSearch").offset().top - 50
            });
        }

        function activeRequireField() {
            $(".required").prop('required', true);
        }

        function inactiveRequireField() {
            $(".required").prop('required', false);
        }

    </script>
   <%-- <div id="modal-asset-calendar" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">ปฏิทินการใช้อุปกรณ์</h4>
                </div>
                <div class="modal-body">
                    <uc1:AssetAgendaCtrl runat="server" id="AssetAgendaCtrl" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>--%>

</asp:Content>
