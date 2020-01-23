<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRMMasterPage.master" AutoEventWireup="true" CodeBehind="EquipmentDiagramRelation.aspx.cs" Inherits="ServiceWeb.crm.Master.Equipment.EquipmentDiagramRelation" %>

<%@ Register Src="~/LinkFlowChart/FlowChartDiagramRelationControl.ascx" TagPrefix="uc1" TagName="FlowChartDiagramRelationControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="row">
		<div class="col">
			<div class="card shadow">
				<div class="card-header">
					<h5 class="mb-0">Configuration Item Diagram Relation</h5>
				</div>
				<div class="card-body">
                    <uc1:FlowChartDiagramRelationControl runat="server" ID="FlowChartDiagramRelationControl" RelationType="EQUIPMENT" />
                    
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button Text="save" runat="server" ID="saveRelation" CssClass="hide" ClientIDMode="Static"
                                OnClick="saveRelation_Click" OnClientClick="AGLoading(true);" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
