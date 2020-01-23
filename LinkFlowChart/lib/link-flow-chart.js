
var _myPaletteDivID = "";
//var _IsVisibleLinkDesc = false;

function LinkFlowChartInit(datas) {
    var paletteId = datas.paletteId;
    var chartId = datas.chartId;
    var saveButtonId = datas.saveButtonId;
    var paletteDatas = datas.paletteDatas;
    var chartDatas = datas.chartDatas;
    var relationNode = datas.relationNode;
    var _IsVisibleLinkDesc = datas._IsVisibleLinkDesc;
    var _IsCanUndo = datas._IsCanUndo;
    var _callbackOnLinkDrawn = datas._callbackOnLinkDrawn;
    var _callbackOnDropBox = datas._callbackOnDropBox;

    _myPaletteDivID = paletteId;

    if (window.goSamples)
        goSamples();  // init for these samples -- you don't need to call this


    //======================================================================


    var dragged = null; // A reference to the element currently being dragged

    // highlight stationary nodes during an external drag-and-drop into a Diagram
    function highlight(node) {  // may be null
        var oldskips = myDiagram.skipsUndoManager;
        myDiagram.skipsUndoManager = true;
        myDiagram.startTransaction("highlight");
        if (node !== null) {
            myDiagram.highlight(node);
        } else {
            myDiagram.clearHighlighteds();
        }
        myDiagram.commitTransaction("highlight");
        myDiagram.skipsUndoManager = oldskips;
    }

    document.addEventListener("dragstart", function (event) {
        if (event.target.className !== "palette-item") return;
        // Some data must be set to allow drag
        event.dataTransfer.setData("text", "");

        // store a reference to the dragged element
        dragged = event.target;

    }, false);

    document.addEventListener("dragend", function (event) {
        // reset the border of the dragged element
        dragged.style.border = "";
        highlight(null);
    }, false);

    var div = document.getElementById(chartId);

    div.addEventListener("dragenter", function (event) {
        // Here you could also set effects on the Diagram,
        // such as changing the background color to indicate an acceptable drop zone

        // Requirement in some browsers, such as Internet Explorer
        event.preventDefault();
    }, false);

    div.addEventListener("dragover", function (event) {
        // We call preventDefault to allow a drop
        // But on divs that already contain an element,
        // we want to disallow dropping

        if (this === myDiagram.div) {
            var can = event.target;
            var pixelratio = window.PIXELRATIO;

            // if the target is not the canvas, we may have trouble, so just quit:
            if (!(can instanceof HTMLCanvasElement)) return;

            var bbox = can.getBoundingClientRect();
            var bbw = bbox.width;
            if (bbw === 0) bbw = 0.001;
            var bbh = bbox.height;
            if (bbh === 0) bbh = 0.001;
            var mx = event.clientX - bbox.left * ((can.width / pixelratio) / bbw);
            var my = event.clientY - bbox.top * ((can.height / pixelratio) / bbh);
            var point = myDiagram.transformViewToDoc(new go.Point(mx, my));
            var curnode = myDiagram.findPartAt(point, true);
            if (curnode instanceof go.Node) {
                highlight(curnode);
            } else {
                highlight(null);
            }
        }

        if (event.target.className === "dropzone") {
            // Disallow a drop by returning before a call to preventDefault:
            return;
        }

        // Allow a drop on everything else
        event.preventDefault();
    }, false);

    div.addEventListener("dragleave", function (event) {
        // reset background of potential drop target
        if (event.target.className == "dropzone") {
            event.target.style.background = "";
        }
        highlight(null);
    }, false);

    div.addEventListener("drop", function (event) {
        console.log("On Drop Box");
        // prevent default action
        // (open as link for some elements in some browsers)
        event.preventDefault();

        // Dragging onto a Diagram
        if (this === myDiagram.div) {
            var can = event.target;
            var pixelratio = window.PIXELRATIO;
            pixelratio = pixelratio == undefined ? 1 : pixelratio;
            // if the target is not the canvas, we may have trouble, so just quit:
            if (!(can instanceof HTMLCanvasElement)) return;

            var bbox = can.getBoundingClientRect();
            var bbw = bbox.width;
            if (bbw === 0) bbw = 0.001;
            var bbh = bbox.height;
            if (bbh === 0) bbh = 0.001;


            var mx = event.clientX - bbox.left * ((can.width / pixelratio) / bbw);
            var my = event.clientY - bbox.top * ((can.height / pixelratio) / bbh);
            var point = myDiagram.transformViewToDoc(new go.Point(mx, my));


            myDiagram.startTransaction('new node');
            myDiagram.model.addNodeData({
                loc: point.x + " " + point.y,
                text: jQuery(dragged).attr("data-text"),
                code: jQuery(dragged).attr("data-code")
            });
            myDiagram.commitTransaction('new node');

            // remove dragged element from its old location
            dragged.parentNode.removeChild(dragged);
        }

        if (_callbackOnDropBox) {
            _callbackOnDropBox(event);
        }

        // If we were using drag data, we could get it here, ie:
        // var data = event.dataTransfer.getData('text');
    }, false);

    //======================================================================


    var $ = go.GraphObject.make;  // for conciseness in defining templates

    var diagramConfig =  {
        allowDragOut: true,  // to myGuests
        allowDrop: true,  // from myGuests
        allowClipboard: false,
        initialContentAlignment: go.Spot.Center,
        "undoManager.isEnabled": _IsCanUndo,
        "LinkDrawn": showLinkLabel,  // this DiagramEvent listener is defined below
        "LinkRelinked": showLinkLabel,
        "animationManager.duration": 800, // slightly longer than default (600ms) animation
        "undoManager.isEnabled": _IsCanUndo  // enable undo & redo
    };
    if (relationNode != "") {
        //diagramConfig.layout = $(go.ForceDirectedLayout);
        diagramConfig.layout = $(go.TreeLayout);
    }

    myDiagram = $(go.Diagram, chartId, diagramConfig);

    myDiagram.allowTextEdit = false;

    // to simulate a "move" from the Palette, the source Node must be deleted.
    //myDiagram.addDiagramListener("ExternalObjectsDropped", function (e) {
    //    // if any Tables were dropped, don't delete from myGuests
    //    console.log("ExternalObjectsDropped drop at " + e.diagram.lastInput.documentPoint);
    //});

    // when the document is modified, add a "*" to the title and enable the "Save" button
    myDiagram.addDiagramListener("Modified", function (e) {
        var button = document.getElementById(saveButtonId);
        if (button) button.disabled = !myDiagram.isModified;
        //var idx = document.title.indexOf("*");
        //if (myDiagram.isModified) {
        //    if (idx < 0) document.title = "* " + document.title;

        //} else {
        //    if (idx >= 0) document.title = document.title.substr(0, idx);
        //}
    });

    myDiagram.addDiagramListener("SelectionDeleting",
        function(e) {
            if (e.diagram.selection.count > 1) {
                e.cancel = true;
                alert("Cannot delete multiple selected parts");
            }
        }
    );

    myDiagram.addDiagramListener("SelectionDeleted", function (e) {
        onDeletedItem();
    });

    myDiagram.addDiagramListener("ObjectSingleClicked",
        function(e) {
        var part = e.subject.part;
        if (!(part instanceof go.Link)) 
            ObjectSingleClicked(part.data);
        });

    myDiagram.addDiagramListener("LinkDrawn", function (e) {
        var link = e.subject;
        if (_callbackOnLinkDrawn) {
            _callbackOnLinkDrawn(e);

        }
    });

    // helper definitions for node templates

    function nodeStyle() {

        return [
            // The Node.location comes from the "loc" property of the node data,
            // converted by the Point.parse static method.
            // If the Node.location is changed, it updates the "loc" property of the node data,
            // converting back using the Point.stringify static method.

           
            new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
            {
                // the Node.location is at the center of each node
                locationSpot: go.Spot.Center,
                //isShadowed: true,
                //shadowColor: "#888",
                // handle mouse enter/leave events to show/hide the ports
                mouseEnter: function (e, obj) { showPorts(obj.part, true); },
                mouseLeave: function (e, obj) { showPorts(obj.part, false); }
            }
        ];
    }

    // Define a function for creating a "port" that is normally transparent.
    // The "name" is used as the GraphObject.portId, the "spot" is used to control how links connect
    // and where the port is positioned on the node, and the boolean "output" and "input" arguments
    // control whether the user can draw links from or to the port.
    function makePort(name, spot, output, input) {
        // the port is basically just a small circle that has a white stroke when it is made visible
        return $(go.Shape, "Circle",
                    {
                        fill: "transparent",
                        stroke: null,  // this is changed to "white" in the showPorts function
                        desiredSize: new go.Size(8, 8),
                        alignment: spot, alignmentFocus: spot,  // align the port on the main Shape
                        portId: name,  // declare this object to be a "port"
                        fromSpot: spot, toSpot: spot,  // declare where links may connect at this port
                        fromLinkable: output, toLinkable: input,  // declare whether the user may draw links to/from here
                        cursor: "pointer"  // show a different cursor to indicate potential link point
                    });
    }

    // define the Node templates for regular nodes

    var lightText = 'whitesmoke';


    myDiagram.nodeTemplateMap.add("",  // the default category
        $(go.Node, "Spot", nodeStyle(),
        // the main object is a Panel that surrounds a TextBlock with a rectangular Shape
        $(go.Panel, "Auto",
            $(go.Shape, "Rectangle",
            { fill: "#337AB7", stroke: null },
            new go.Binding("figure", "figure")),
            $(go.TextBlock,
            {
                font: "11px Helvetica, Arial, sans-serif",
                stroke: lightText,
                margin: 8,
                maxSize: new go.Size(160, NaN),
                wrap: go.TextBlock.WrapFit,
                editable: true
            },
            new go.Binding("text").makeTwoWay())
        ),
        // four named ports, one on each side:
        makePort("T", go.Spot.Top, false, true),
        makePort("L", go.Spot.Left, true, true),
        makePort("R", go.Spot.Right, true, true),
        makePort("B", go.Spot.Bottom, true, false)
    ));


    myDiagram.nodeTemplateMap.add("active",  // the default category
        $(go.Node, "Spot", nodeStyle(),
        // the main object is a Panel that surrounds a TextBlock with a rectangular Shape
        $(go.Panel, "Auto",
            $(go.Shape, "Rectangle",
            { fill: "#E65041", stroke: null },
            new go.Binding("figure", "figure")),
            $(go.TextBlock,
            {
                font: "11px Helvetica, Arial, sans-serif",
                stroke: lightText,
                margin: 8,
                maxSize: new go.Size(160, NaN),
                wrap: go.TextBlock.WrapFit,
                editable: true
            },
            new go.Binding("text").makeTwoWay())
        ),
        // four named ports, one on each side:
        makePort("T", go.Spot.Top, false, true),
        makePort("L", go.Spot.Left, true, true),
        makePort("R", go.Spot.Right, true, true),
        makePort("B", go.Spot.Bottom, true, false)
    ));



    // replace the default Link template in the linkTemplateMap
    myDiagram.linkTemplate =
        $(go.Link,  // the whole link panel
        {
            routing: go.Link.AvoidsNodes,
            curve: go.Link.JumpOver,
            corner: 5, toShortLength: 4,
            relinkableFrom: true,
            relinkableTo: true,
            reshapable: true,
            resegmentable: true,
            // mouse-overs subtly highlight links:
            mouseEnter: function (e, link) { link.findObject("HIGHLIGHT").stroke = "rgba(30,144,255,0.2)"; },
            mouseLeave: function (e, link) { link.findObject("HIGHLIGHT").stroke = "transparent"; }
        },
        new go.Binding(relationNode != "" ? "" : "points").makeTwoWay(),
        $(go.Shape,  // the highlight shape, normally transparent
            { isPanelMain: true, strokeWidth: 8, stroke: "transparent", name: "HIGHLIGHT" }),
        $(go.Shape,  // the link path shape
            { isPanelMain: true, stroke: "gray", strokeWidth: 2 }),
        $(go.Shape,  // the arrowhead
            { toArrow: "standard", stroke: null, fill: "black" }),
        $(go.Panel, "Auto",  // the link label, normally not visible
            { visible: _IsVisibleLinkDesc, name: "LABEL", segmentIndex: 2, segmentFraction: 0.5 },
            new go.Binding("text", "text").makeTwoWay(),
            $(go.Shape, "RoundedRectangle",  // the label shape
            { fill: "#F8F8F8", stroke: null }),
            $(go.TextBlock, "Yes",  // the label
            {
                textAlign: "center",
                font: "10pt helvetica, arial, sans-serif",
                stroke: "#333333",
                editable: true
            },
            new go.Binding("text").makeTwoWay()))
        );

    // Make link labels visible if coming out of a "conditional" node.
    // This listener is called by the "LinkDrawn" and "LinkRelinked" DiagramEvents.
    function showLinkLabel(e) {
        var label = e.subject.findObject("LABEL");
        if (label !== null) label.visible = (e.subject.fromNode.data.figure === "Diamond");
    }

    // temporary links used by LinkingTool and RelinkingTool are also orthogonal:
    myDiagram.toolManager.linkingTool.temporaryLink.routing = go.Link.Orthogonal;
    myDiagram.toolManager.relinkingTool.temporaryLink.routing = go.Link.Orthogonal;

    // Load
    myDiagram.model = go.Model.fromJson(chartDatas);  // load an initial diagram from some JSON text


    //myPalette = $(go.Palette, paletteId,  // must name or refer to the DIV HTML element
    //    {
    //        "animationManager.duration": 800, // slightly longer than default (600ms) animation
    //        nodeTemplateMap: myDiagram.nodeTemplateMap,  // share the templates used by myDiagram
    //        model: new go.GraphLinksModel(paletteDatas)
    //    });


    function customFocus() {
        var x = window.scrollX || window.pageXOffset;
        var y = window.scrollY || window.pageYOffset;
        go.Diagram.prototype.doFocus.call(this);
        window.scrollTo(x, y);
    }

    myDiagram.doFocus = customFocus;

    resetPalette(paletteDatas);

} // end init

// Make all ports on a node visible when the mouse is over the node
function showPorts(node, show) {
    var diagram = node.diagram;
    if (!diagram || diagram.isReadOnly || !diagram.allowLink) return;
    node.ports.each(function (port) {
        port.stroke = (show ? "white" : null);
    });
}


// Show the diagram's model in JSON format that the user may edit
function LinkFlowChartSaveDatas() {
    myDiagram.isModified = false;
    return myDiagram.model.toJson();
}

var _deleteItems = null;
        
function ObjectSingleClicked(data){
    _deleteItems  = data;
}

function onDeletedItem() {
    if (_deleteItems == null) {
        return;
    }
    var container = jQuery("#" + _myPaletteDivID);
    var items = jQuery("<div  />", {
        class: "palette-item",
        "data-code": _deleteItems.code,
        "data-text": _deleteItems.text,
        html: _deleteItems.text
    });
    container.prepend(items);
    items.attr("draggable", "true");
}

var _allPaletteItems = [];

function resetPalette(newPaletteArray) {

    var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });

    var container = jQuery("#" + _myPaletteDivID);

    container.html("");

    for (var i = 0; i < newPaletteArray.length; i++) {
        var items = jQuery("<div  />", {
            class: "palette-item",
            "data-code": newPaletteArray[i].code,
            "data-text": newPaletteArray[i].text,
            html: newPaletteArray[i].text
        });
        container.append(items);
        items.attr("draggable", "true");
    }

    _allPaletteItems = newPaletteArray;

}
