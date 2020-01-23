<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkStreamConfigControl.ascx.cs" Inherits="POSWeb.crm.usercontrol.WorkStreamConfigControl" %>

<div class="hide">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpDataCode">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hddCodeContact" />
            <asp:HiddenField runat="server" ID="hddWorkGroupCode" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="panel panel-success">
    <div class="panel-heading">
        <b>
            โครงสร้างการติดต่อ
            <asp:Label Text="" runat="server" ID="lblModeHeirarchy" />
        </b>
    </div>
    <div class="panel-body">
        <div class="row">
            <div class="col-md-12">
                <label>
                    เพิ่มส่วนการติดต่อ
                </label>
                <asp:DropDownList runat="server" ID="ddlListHierarchy" CssClass="form-control">
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button Text="Save" runat="server" CssClass="btn btn-primary" ID="btnSaveStructure"
                            OnClick="btnSaveStructure_Click" OnClientClick="AGLoading(false);" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:UpdatePanel runat="server" ID="udpListHierarchyMapping" UpdateMode="Conditional" >
                    <ContentTemplate>
                        <table class="table">
                            <tr>
                                <th>
                                    ลำดับโครงสร้าง
                                </th>
                                <th>
                                    โครงสร้างการติดต่อ
                                </th>
                                <th>
                                    Manage
                                </th>
                            </tr>
                            <asp:Repeater runat="server" ID="rptListRefStructure">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%# bindHeirarchy(Eval("CompanyStructureCode").ToString()) %>
                                        </td>
                                        <td>
                                            <%# Eval("StructureName") %>
                                        </td>
                                        <th style="text-align:center;">
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <i class="fa fa-trash-o" style="cursor:pointer;font-size:20px;color:red;" onclick="$(this).next().click();"></i>
                                                    <asp:Button Text="" runat="server" ID="btnRemove" CssClass="hide"
                                                        OnClick="btnRemove_Click" OnClientClick="AGLoading(false);"
                                                        CommandArgument='<%# Eval("CompanyStructureCode") %>' />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </th>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <% if (rptListRefStructure.Items.Count == 0)
                               { %>
                            <tr>
                                <td colspan="2">
                                    No data.
                                </td>
                            </tr>
                            <% } %>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</div>