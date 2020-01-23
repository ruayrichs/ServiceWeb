<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="MasterDocType.aspx.cs" Inherits="ServiceWeb.MasterConfig.MasterDocType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-master-doctype").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <div>
        <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <button type="button" class="btn btn-success mb-2 AUTH_MODIFY" onclick="$(this).next().click();"> <i class="fa fa-save" >&nbsp;&nbsp;Save</i></button>
                <asp:Button OnClientClick="return AGConfirm(this,'Do you want to save !!');AGLoading(true);" class="btn btn-success mb-2 AUTH_MODIFY d-none" runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Master</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="udpMasterConfig" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table id="tableMaster" class="table table-striped table-bordered table-sm">
                                    <tr>
                                        <th>Ticket Type Code
                                        </th>
                                        <th>Ticket Type Description
                                        </th>
                                        <th>Business Object
                                        </th>
                                        <th>SLA Group
                                        </th>
                                    </tr>
                                    <asp:Repeater ID="tableData" runat="server" OnItemDataBound="tableData_ItemDataBound">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <%# Eval("DocumentTypeCode") %>
                                                </td>
                                                <td>
                                                    <%# Eval("Description") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" CssClass="d-none" ID="txtDocumentTypeCode" Text='<%# Eval("DocumentTypeCode") %>'></asp:TextBox>
                                                    <asp:TextBox runat="server" CssClass="d-none" Text='<%# Eval("BusinessObject") %>'></asp:TextBox>
                                                    <asp:DropDownList CssClass="form-control form-control-sm" OnSelectedIndexChanged="ddlYourDDL_DataBinding" ID="ddlBusinessOject" SelectedValue='<%# Eval("BusinessObject") %>' runat="server">
                                                        <asp:ListItem Enabled="true" Text="None" Value=""></asp:ListItem>
                                                        <asp:ListItem Text="Change" Value="CHANGE"></asp:ListItem>
                                                        <asp:ListItem Text="Incident" Value="INCIDENT"></asp:ListItem>
                                                        <asp:ListItem Text="Problem" Value="PROBLEM"></asp:ListItem>
                                                        <asp:ListItem Text="Request" Value="REQUEST"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <%--<asp:TextBox runat="server" CssClass="d-none" ID="txtddlSLAGroup" Text='<%# Eval("Default_SLAGroup") %>'></asp:TextBox>--%>
                                                    <asp:TextBox runat="server" CssClass="d-none" ID="txtSLAGroup" Text='<%# Eval("Default_SLAGroup") %>'></asp:TextBox>
                                                    <asp:DropDownList CssClass="form-control form-control-sm" OnSelectedIndexChanged="ddlSALGp_SelectedIndexChanged"
                                                        ID="ddlSALGp" runat="server"
                                                        AppendDataBoundItems="true">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
