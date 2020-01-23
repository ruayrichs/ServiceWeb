<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/RedesignV2/DefaultMasterPageV2.master" AutoEventWireup="true" CodeBehind="AssetStructureDiagram.aspx.cs" Inherits="ServiceWeb.crm.Master.Asset.AssetStructureDiagram" %>

<%@ Register Src="~/LinkFlowChart/LinkFlowChartControl.ascx" TagPrefix="uc1" TagName="LinkFlowChartControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:LinkFlowChartControl CallbackSaveDatas="saveFlowChart" runat="server" id="LinkFlowChartControl" />

    <style>
        body, html {
            overflow: hidden;
        }
    </style>

    <div class="hide">
        <asp:UpdatePanel runat="server" ID="udpBackupJSON" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtSaveNodeDataArray" ClientIDMode="Static" />
                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtSaveLinkDataArray" ClientIDMode="Static" />
                <asp:Button Text="text" ID="btnSaveDataFlowChart" OnClick="btnSaveDataFlowChart_Click" runat="server" ClientIDMode="Static" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script>
        function saveFlowChart(items, connectors) {
            AGLoading(true);
            $("#txtSaveNodeDataArray").val(JSON.stringify(items));
            $("#txtSaveLinkDataArray").val(JSON.stringify(connectors));
            $("#btnSaveDataFlowChart").click();
        }
    </script>

</asp:Content>
