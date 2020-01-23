<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRMMasterPage.master" AutoEventWireup="true" CodeBehind="EquipmentClassDiagramRelation.aspx.cs" Inherits="ServiceWeb.crm.Master.Equipment.EquipmentClassDiagramRelation" %>

<%@ Register Src="~/LinkFlowChart/FlowChartDiagramRelationControl.ascx" TagPrefix="uc1" TagName="FlowChartDiagramRelationControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-class-relation").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <div class="row">
		<div class="col">
			<div class="card shadow">
				<div class="card-header">
					<h5 class="mb-0">Configuration Item Class Diagram Relation</h5>
				</div>
				<div class="card-body">
                    <uc1:FlowChartDiagramRelationControl runat="server" ID="FlowChartDiagramRelationControl" RelationType="CLASS" />
                    
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