<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRMMasterPage.master" AutoEventWireup="true" CodeBehind="EquipmentClassDiagram.aspx.cs" Inherits="ServiceWeb.crm.Master.Equipment.EquipmentClassDiagram" %>

<%@ Register Src="~/LinkFlowChart/LinkFlowChartControl.ascx" TagPrefix="uc1" TagName="LinkFlowChartControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var _DiagramEvent;
        function openModalSelectRalationType(event) {
            _DiagramEvent = event;

            var linkData = _DiagramEvent.subject.data;
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

            $("#modal-relation-type").modal("show");
        }

        function hideModalSelectRalationType(isUndo) {
            if (isUndo) {
                var linkData = _DiagramEvent.subject.data;
                myDiagram.model.removeLinkData(linkData);
                //myDiagram.undoManager.undo();
            }
            //else {
            //    setRelationType();
            //}

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
    <uc1:LinkFlowChartControl CallbackSaveDatas="saveFlowChart" IsVisibleLinkDesc="true"
        runat="server" id="LinkFlowChartControl" CallbackOnLinkDrawn="openModalSelectRalationType" />
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
                    <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>--%>
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
