function aGapeISS() {
    $(".a-gape-image").on("click", function () {
        var createConID = aGapeImageScreenShot(this);
        aGapeFadeInContainer(createConID);
    });

    $(".a-gape-image").css("cursor", "pointer");
    $(".a-gape-image").css("cursor", "hand");
}
function aGapeIFrameISS()
{
    $(".a-gape-image").on("click", function () {
        var createConID = window.parent.aGapeImageScreenShot(this);
        window.parent.aGapeFadeInContainer(createConID);
    });
    $(".a-gape-image").css("cursor", "pointer");
    $(".a-gape-image").css("cursor", "hand");
}
function aGapeFadeInContainer(id) {
    $("#" + id).fadeIn();
}
function aGapeRemoveContainer(EId) {
    return (EObj = document.getElementById(EId)) ? EObj.parentNode.removeChild(EObj) : false;
}
function aGapeGuid() {
    return 'xxxxxxxx_xxxx_4xxx_yxxx_xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
function aGapeImageScreenShot(obj) {
    var createID = aGapeGuid();

    var container = document.createElement("div");
    container.style.position = "fixed";
    container.style.background = "rgba(0, 0, 0, 0.50)";
    container.style.width = "100%";
    container.style.height = "100%";
    container.style.top = "0px";
    container.style.left = "0px";
    container.style.msUserSelect = "none";
    container.style.zIndex = 2000;
    container.style.display = "none";
    container.id = createID;
    
    
    var tableContainer = document.createElement("table");
    tableContainer.style.width = "100%";
    tableContainer.style.height = "100%";

    tableContainer.onclick = function (e) {
        $("#" + createID).fadeOut();
        setTimeout(function () {
            aGapeRemoveContainer(createID);
        }, 1000);
    };

    var row = tableContainer.insertRow(0);
    var cell = row.insertCell(0);
    cell.style
    cell.style.width = "100%";
    cell.style.height = "100%";
    cell.style.textAlign = "center";
    cell.style.verticalAlign = "middle";


    var ssImg = document.createElement("img");
    ssImg.src = obj.src;
    //ssImg.style.maxWidth = "90%";
    //ssImg.style.maxHeight = "90%";
    ssImg.style.minWidth = "10%";
    ssImg.style.minHeight = "10%";
    ssImg.style.padding = "10px";
    ssImg.style.background = "#fff";
    ssImg.style.border = "1px solid #000";
    ssImg.style.cursor = "pointer";
    ssImg.style.cursor = "hand";
    ssImg.id = "img" + createID;

    

    container.onmousewheel = function (e) {
        var evt = window.event || e;
        var delta = evt.detail ? evt.detail * (-120) : evt.wheelDelta;
        if (delta == 120) {
            if (ssImg.style.width == "") {
                ssImg.style.width = (ssImg.width + 50) + "px";
                ssImg.style.height = (ssImg.height + 50) + "px";
            }
            else {
                ssImg.style.width = (parseInt(ssImg.style.width.replace("px","")) + 30) + "px";
                ssImg.style.height = (parseInt(ssImg.style.height.replace("px", "")) + 30) + "px";
            }
        }
        else {
            if (ssImg.style.width == "") {
                ssImg.style.width = ssImg.width + "px";
                ssImg.style.height = ssImg.height + "px";
            }
            else {
                ssImg.style.width = (parseInt(ssImg.style.width.replace("px", "")) - 30) + "px";
                ssImg.style.height = (parseInt(ssImg.style.height.replace("px", "")) - 30) + "px";
            }
        }
    }

    var buttonContainer = document.createElement("div");
    buttonContainer.style.position = "fixed";
    buttonContainer.style.width = "100%";
    buttonContainer.style.bottom = "0px";
    buttonContainer.style.left = "0px";
    buttonContainer.style.padding = "20px";
    buttonContainer.style.textAlign = "center";
    buttonContainer.style.background = "#000";

    var buttonZoomIn = document.createElement("input");
    buttonZoomIn.type = "button";
    buttonZoomIn.value = "Zoom In";
    buttonZoomIn.style.width = "100px";
    buttonZoomIn.style.marginRight = "20px";
    buttonZoomIn.className = "btn btn-primary";
    buttonZoomIn.onclick = function () {
        if (ssImg.style.width == "") {
            ssImg.style.width = (ssImg.width + 50) + "px";
            ssImg.style.height = (ssImg.height + 50) + "px";
        }
        else {
            ssImg.style.width = (parseInt(ssImg.style.width.replace("px", "")) + 30) + "px";
            ssImg.style.height = (parseInt(ssImg.style.height.replace("px", "")) + 30) + "px";
        }
    };

    var buttonZoomOut = document.createElement("input");
    buttonZoomOut.type = "button";
    buttonZoomOut.value = "Zoom Out";
    buttonZoomOut.style.width = "100px";
    buttonZoomOut.style.marginRight = "20px";
    buttonZoomOut.className = "btn btn-primary";
    buttonZoomOut.onclick = function () {
        if (ssImg.style.width == "") {
            ssImg.style.width = ssImg.width + "px";
            ssImg.style.height = ssImg.height + "px";
        }
        else {
            ssImg.style.width = (parseInt(ssImg.style.width.replace("px", "")) - 30) + "px";
            ssImg.style.height = (parseInt(ssImg.style.height.replace("px", "")) - 30) + "px";
        }
    };


    var buttonClose = document.createElement("input");
    buttonClose.type = "button";
    buttonClose.value = "Close";
    buttonClose.style.width = "100px";
    buttonClose.style.marginRight = "20px";
    buttonClose.className = "btn btn-primary";
    buttonClose.onclick = function () {
        $("#" + createID).fadeOut();
        setTimeout(function () {
            aGapeRemoveContainer(createID);
        }, 1000);

    };

    buttonContainer.appendChild(buttonZoomIn);
    buttonContainer.appendChild(buttonZoomOut);
    buttonContainer.appendChild(buttonClose);

    var headerContainer = document.createElement("div");
    headerContainer.style.position = "fixed";
    headerContainer.style.width = "100%";
    headerContainer.style.top = "0px";
    headerContainer.style.left = "0px";
    headerContainer.style.padding = "20px";
    headerContainer.style.textAlign = "center";
    headerContainer.style.background = "#000";
    headerContainer.style.color = "#fff";
    headerContainer.innerHTML = obj.alt;

    cell.appendChild(ssImg);
    container.appendChild(tableContainer);
    container.appendChild(buttonContainer);
    container.appendChild(headerContainer);
    document.body.appendChild(container);

    return createID;
}