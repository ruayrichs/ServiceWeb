<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LinkFlowChartControl.ascx.cs" Inherits="ServiceWeb.LinkFlowChart.LinkFlowChartControl" %>


<script src="<%= Page.ResolveUrl("~/LinkFlowChart/lib/GoJS/js/go.js?vs=20190113") %>"></script>
<script src="<%= Page.ResolveUrl("~/LinkFlowChart/lib/link-flow-chart.js?vs=20190113") %>"></script>

<div>

    <style>
        .palettezone{
            height: calc(100vh - 135px);
            margin-top:35px;
            overflow-y:auto;
            overflow-x:hidden;
        }
        .palettezone .palette-item{
            padding:5px 10px;
            background:#009688;
            color:#fff;
            border:1px solid #ccc;
            cursor:pointer;
            white-space:normal;
            border-left:3px solid #185664;
            margin-bottom:5px;
            margin-right:5px;
            border-radius:5px;
            font-size:12px;
        }
        .palettezone .palette-item:hover{
            background:#6AC1BA;
        }
        .diagram-container{
            display: inline-block; 
            vertical-align: top; 
            width: 80%;
            position:relative
        }
        #myDiagramDivContainer.full-screen{
            position:fixed;
            left:0;
            right:0;
            bottom:0;
            top:0;
            z-index:2000;
            display:block;
            width:100%;
            background:#fff;
            height:100vh!important;
        }
        #myDiagramDivContainer.full-screen #myDiagramDiv{
            height:100vh!important;
        }
    </style>
    
    <div style="width: 100%; white-space: nowrap;">
        <script>

            var searchKeyPaletteTO = null;
            function searchKeyPalette(obj) {
                //if (searchKeyPaletteTO != null) {
                //    clearTimeout(searchKeyPaletteTO);
                //}

                if (obj.value.trim() == "") {
                    $("#myPaletteDiv .palette-item").show();
                }else{
                    $("#myPaletteDiv .palette-item").each(function () {
                        if ($(this).attr("data-text").toLowerCase().match(obj.value.toLowerCase())) {
                            $(this).show();
                        } else {
                            $(this).hide();
                        }
                    });
                }
            }
            function toggleDiagramFullscreen(obj) {
                $("#myDiagramDivContainer").toggleClass("full-screen");
                $(obj).toggleClass("active");
                $(window).resize();
            }
        </script>

        <span id="myPaletteDivContainer" class="diagram-container" style="width:20%">
            <div style="position:absolute;top:0;right:5px;left:0;z-index:999">
                <input type="text" onkeyup="searchKeyPalette(this);"  class="form-control input-sm" name="name" value="" placeholder="Search..." />
            </div>

            <div id="myPaletteDiv" class="palettezone"></div>
        </span>

        <span id="myDiagramDivContainer" class="diagram-container <%= !string.IsNullOrEmpty(Request["relationNode"]) ? "full-screen" : "" %>">
            <div class="<%= !string.IsNullOrEmpty(Request["relationNode"]) ? "hide" : "" %>" style="position:absolute;top:10px;right:20px;z-index:999">
                <button id="myBtnSave" disabled type="button" onclick="_LinkFlowChartSaveDatas();" class="btn btn-success btn-sm AUTH_UPDATE">
                    <i class="fa fa-save"></i>
                    บันทึกการแก้ไข
                </button>
                <button type="button"  class="btn btn-warning btn-sm" onclick="toggleDiagramFullscreen(this);">
                    <i class="fa fa-desktop"></i>
                </button>
            </div>
            <div id="myDiagramDiv" style="border: solid 1px #ccc; height: calc(100vh - 100px)"></div>
        </span>
    </div>

    <div class="hide">
        <asp:UpdatePanel runat="server" ID="udpBackupJSON" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtPaletteDataArray" ClientIDMode="Static" />
                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtNodeDataArray" ClientIDMode="Static" />
                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtLinkDataArray" ClientIDMode="Static" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script>
        //_IsVisibleLinkDesc = "<%= IsVisibleLinkDesc.ToString().ToLower() %>" == "true";
        //_callbackOnLinkDrawn = <%= !string.IsNullOrEmpty(CallbackOnLinkDrawn) ? CallbackOnLinkDrawn : "undefined" %>;

        var _LinkFlowChartInitDatas = {
            "class": "go.GraphLinksModel",
            "linkFromPortIdProperty": "fromPort",
            "linkToPortIdProperty": "toPort",
            "nodeDataArray": [],
            "linkDataArray": []
        };

        function _LinkFlowChartInit() {

            var paletteDataArray = $.parseJSON($("#txtPaletteDataArray").val());
            var nodeDataArray = $.parseJSON($("#txtNodeDataArray").val());
            var linkDataArray = $.parseJSON($("#txtLinkDataArray").val());

            _LinkFlowChartInitDatas.nodeDataArray = nodeDataArray;
            _LinkFlowChartInitDatas.linkDataArray = linkDataArray;
            
            LinkFlowChartInit({
                paletteId: "myPaletteDiv",
                chartId: "myDiagramDiv",
                saveButtonId: "myBtnSave",
                paletteDatas: paletteDataArray,
                chartDatas: _LinkFlowChartInitDatas,
                relationNode:"<%= Request["relationNode"] %>",
                _IsVisibleLinkDesc : "<%= IsVisibleLinkDesc.ToString().ToLower() %>" == "true",
                _IsCanUndo : "<%= IsCanUndo.ToString().ToLower() %>" == "true",
                _callbackOnLinkDrawn: <%= !string.IsNullOrEmpty(CallbackOnLinkDrawn) ? CallbackOnLinkDrawn : "undefined" %>,
                _callbackOnDropBox: <%= !string.IsNullOrEmpty(CallbackOnDropBox) ? CallbackOnDropBox : "undefined" %>
            });
        }

        function _LinkFlowChartSaveDatas() {
            var diagramDatas = $.parseJSON(LinkFlowChartSaveDatas());

            var nodeDataArray = diagramDatas.nodeDataArray;
            var linkDataArray = diagramDatas.linkDataArray;

            <%= !string.IsNullOrEmpty(CallbackSaveDatas) ? CallbackSaveDatas + "(nodeDataArray,linkDataArray);" : "" %>
        }

        $(document).ready(function () {
            try {
                window.parent.successIframeLoad();
            }
            catch (e) { }
        });

    </script>

</div>
