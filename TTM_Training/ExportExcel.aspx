<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="ExportExcel.aspx.cs" Inherits="ServiceWeb.TTM_Training.ExportExcel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button Text="Export Data" runat="server" ID="btnExpoet" 
                    OnClick="btnExpoet_Click" OnClientClick="AGLoading(true);" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <a id="download-report-excel" class="hide" target="_blank"
        href="/API/ExportExcelAPI.ashx"></a>
    <script>
        function exportExcelAPI() {
            $("#download-report-excel")[0].click();
        }
    </script>
</asp:Content>
