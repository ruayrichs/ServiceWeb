<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRMMasterPage.master" AutoEventWireup="true" CodeBehind="EquipmentDiagram.aspx.cs" Inherits="ServiceWeb.crm.Master.Equipment.EquipmentDiagram" %>
<%@ Register Src="~/LinkFlowChart/LinkFlowChartControl.ascx" TagPrefix="uc1" TagName="LinkFlowChartControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var _DiagramEvent;

        function validateDiagramChangeBox(event) {
            var EquipmentData, newItem, RelationData;
            newItem = myDiagram.model.nodeDataArray[myDiagram.model.nodeDataArray.length - 1];

            var LinkRelationArray = JSON.parse($("#txtLinkRelationArray").val());
            var EquipmentItemArray = JSON.parse($("#txtEquipmentItemArray").val());

            for (var i = 0; i < EquipmentItemArray.length; i++) {
                if (EquipmentItemArray[i].EquipmentCode == newItem.code) {
                    EquipmentData = EquipmentItemArray[i];
                }
            }

            for (var i = 0; i < LinkRelationArray.length; i++) {
                if (LinkRelationArray[i].ItemCode == EquipmentData.EquipmentClass) {
                    RelationData = LinkRelationArray[i];
                }
            }

            if (!RelationData) {
                AGMessage("รายการ Equipment " + EquipmentData.Description + "(" + EquipmentData.EquipmentCode + ") ไม่มีการกำหนด Class Relation.");
                setTimeout(function () {
                    myDiagram.undoManager.undo();
                }, 500);
                return;
            }
        }
        function validateDiagramChangeLink(event) {
            var EquipmentData, EquipmentParentData,
                RelationData, linkData,
                nodeData, nodeParentData,
                countLinkDuplicate;

            linkData = event.subject.data;
            if (linkData.from == linkData.to) {
                AGMessage("ไม่สามารถเชื่อมความสัมพันธ์กับตัวเองได้");
                myDiagram.model.removeLinkData(linkData);
                return;
            }

            countLinkDuplicate = 0;
            for (var i = 0; i < myDiagram.model.linkDataArray.length; i++) {
                if (myDiagram.model.linkDataArray[i].from == linkData.from &&
                    myDiagram.model.linkDataArray[i].to == linkData.to) {
                    countLinkDuplicate++;
                }
            }
            if (countLinkDuplicate > 1) {
                AGMessage("ไม่สามารถเชื่อมความสัมพันธ์ซ้ำกันได้");
                myDiagram.model.removeLinkData(linkData);
                return;
            }

            var EquipmentItemArray = JSON.parse($("#txtEquipmentItemArray").val());
            var LinkRelationArray = JSON.parse($("#txtLinkRelationArray").val());

            // find parent node detail in diagram
            for (var i = 0; i < myDiagram.model.nodeDataArray.length; i++) {
                if (myDiagram.model.nodeDataArray[i].key == linkData.from) {
                    nodeParentData = myDiagram.model.nodeDataArray[i];
                }
            }
            // find node detail in diagram
            for (var i = 0; i < myDiagram.model.nodeDataArray.length; i++) {
                if (myDiagram.model.nodeDataArray[i].key == linkData.to) {
                    nodeData = myDiagram.model.nodeDataArray[i];
                }
            }

            // find Parent and My equipment detail
            for (var i = 0; i < EquipmentItemArray.length; i++) {
                if (EquipmentItemArray[i].EquipmentCode == nodeParentData.code) {
                    EquipmentParentData = EquipmentItemArray[i];
                }
                if (EquipmentItemArray[i].EquipmentCode == nodeData.code) {
                    EquipmentData = EquipmentItemArray[i];
                }
            }

            // find Relation detail
            for (var i = 0; i < LinkRelationArray.length; i++) {
                if (LinkRelationArray[i].ItemCode == EquipmentData.EquipmentClass &&
                    LinkRelationArray[i].parentCode == EquipmentParentData.EquipmentClass) {
                    RelationData = LinkRelationArray[i];
                }
            }



            if (RelationData) {
                openModalSelectRalationType(event, RelationData.RelationType);

                //myDiagram.model.setDataProperty(linkData, "text", RelationData.RelationType);
                //myDiagram.rebuildParts();
            } else {
                AGMessage("ไม่สามารถเชื่อม " + EquipmentData.Description + "  ภายใต้  " + EquipmentParentData.EquipmentCode + " ได้");
                myDiagram.model.removeLinkData(linkData);
                return;
            }
        }
        function openModalSelectRalationType(event, RelationType) {
            _DiagramEvent = event;
            try {
                $("#ddlRelationType").val(RelationType.split(":")[0].trim());
            } catch (e) {

            }
            $("#modal-relation-type").modal("show");
        }

        function hideModalSelectRalationType(isUndo) {
            if (isUndo) {
                var linkData = _DiagramEvent.subject.data;
                myDiagram.model.removeLinkData(linkData);
            }

            $("#modal-relation-type").modal("hide");
        }
        function setRelationType() {
            var val = $("#ddlRelationType").val();
            var text = $("#ddlRelationType option:selected").text();

            if (val == "") {
                AGMessage("กรุณาเลือก Relation Type!");
                return;
            }
            var _DiagramDataItem = _DiagramEvent.subject.data;
            myDiagram.model.setDataProperty(_DiagramDataItem, "text", val + " : " + text);
            myDiagram.rebuildParts();

            hideModalSelectRalationType(false);
        }
    </script>

    <uc1:LinkFlowChartControl CallbackSaveDatas="saveFlowChart" runat="server" id="LinkFlowChartControl"
        IsVisibleLinkDesc="true" 
        CallbackOnLinkDrawn="validateDiagramChangeLink" 
        CallbackOnDropBox="validateDiagramChangeBox" />

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

                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtLinkRelationArray" ClientIDMode="Static" Text="{}" />
                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtEquipmentItemArray" ClientIDMode="Static" Text="{}" />

                <asp:Button Text="text" ID="btnSaveDataFlowChart" CssClass="AUTH_UPDATE" OnClick="btnSaveDataFlowChart_Click" runat="server" ClientIDMode="Static" />
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

    
    <!-- Modal -->
    <div class="modal fade" id="modal-relation-type" tabindex="-1" role="dialog" aria-labelledby="modalLabelHeader" 
        aria-hidden="true" data-backdrop="false" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabelHeader">
                        Select Ralation Type
                    </h5>
                </div>
                <div class="modal-body">
                    <asp:updatePanel runat="server" UpdateMode="Conditional" ID="udpDataRelation">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-12">
                                    <label>
                                        Relation Type
                                    </label>
                                    <asp:DropDownList runat="server" ID="ddlRelationType" CssClass="form-control" ClientIDMode="Static"
                                        DataValueField="RelationCode" DataTextField="RelationDescript">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:updatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="hideModalSelectRalationType(true);">Close</button>
                    <button type="button" class="btn btn-primary" onclick="setRelationType();">Save changes</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
