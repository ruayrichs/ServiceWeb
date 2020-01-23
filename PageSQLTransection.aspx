<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/ServiceTicketMasterPage.master" AutoEventWireup="true" CodeBehind="PageSQLTransection.aspx.cs" Inherits="ServiceWeb.PageSQLTransection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-row">
        <div class="form-group col-md-12">
            <asp:TextBox runat="server" Text="" TextMode="MultiLine" Rows="12" CssClass="form-control form-control-sm"
                ID="txtSQL_Script" />
        </div>
    </div>
    <div class="form-row">
        <div class="form-group col-md-12">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpButton">
                <ContentTemplate>
                    <asp:Button Text="Select Data" runat="server" CssClass="btn btn-primary btn-sm"
                        ID="btnSelectData" OnClick="btnSelectData_Click" OnClientClick="AGLoading(true);" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <hr style="margin: 12px 0;">
    <div class="form-row">
        <div class="form-group col-md-12">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpData">
                <ContentTemplate>
                    <div runat="server" id="panelResult">
                    </div>
                    <%--<table class='table table-bordered'>
                        <thead>
                            <tr>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td></td>
                            </tr>
                        </tbody>
                    </table>--%>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <script>
        function myfunction() {

        }
    </script>
</asp:Content>
