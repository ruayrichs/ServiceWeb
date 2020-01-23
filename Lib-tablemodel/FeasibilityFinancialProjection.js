var thisTR;
$.fn.initRightClick = function () {
    var myDoc = this.context;

    function clickInsideElement(e, className) {
        var el = e.srcElement || e.target;
        if (el.classList.contains(className)) {
            return el;
        } else {
            while (el = el.parentNode) {
                if (el.classList && el.classList.contains(className)) {
                    return el;
                }
            }
        }

        return false;
    }

    /**
     * Get's exact position of event.
     * 
     * @param {Object} e The event passed in
     * @return {Object} Returns the x and y position
     */
    function getPosition(e) {
        var posx = 0;
        var posy = 0;

        if (!e) var e = window.event;

        if (e.pageX || e.pageY) {
            posx = e.pageX;
            posy = e.pageY;
        } else if (e.clientX || e.clientY) {
            posx = e.clientX + myDoc.body.scrollLeft + myDoc.documentElement.scrollLeft;
            posy = e.clientY + myDoc.body.scrollTop + myDoc.documentElement.scrollTop;
        }

        return {
            x: posx,
            y: posy
        }
    }

    var contextMenuClassName = "context-menu";
    var contextMenuItemClassName = "context-menu__item";
    var contextMenuLinkClassName = "context-menu__link";
    var contextMenuActive = "context-menu--active";

    var taskItemClassName = "row-click";
    var taskItemInContext;

    var clickCoords;
    var clickCoordsX;
    var clickCoordsY;

    var menu = myDoc.querySelector("#context-menu");
    var menuItems = menu.querySelectorAll(".context-menu__item");
    var menuState = 0;
    var menuWidth;
    var menuHeight;
    var menuPosition;
    var menuPositionX;
    var menuPositionY;

    var windowWidth;
    var windowHeight;

    /**
     * Initialise our application's code.
     */
    function init() {
        contextListener();
        clickListener();
        keyupListener();
        resizeListener();
    }

    /**
     * Listens for contextmenu events.
     */
    function contextListener() {
        myDoc.addEventListener("contextmenu", function (e) {
            taskItemInContext = clickInsideElement(e, taskItemClassName);
            console.log(taskItemInContext);
            if (taskItemInContext) {
                thisTR = taskItemInContext;
                e.preventDefault();
                toggleMenuOn();
                positionMenu(e);
            } else {
                taskItemInContext = null;
                toggleMenuOff();
            }
        });
    }

    /**
     * Listens for click events.
     */
    function clickListener() {
        myDoc.addEventListener("click", function (e) {
            var clickeElIsLink = clickInsideElement(e, contextMenuLinkClassName);

            if (clickeElIsLink) {
                e.preventDefault();
                menuItemListener(clickeElIsLink);
            } else {
                var button = e.which || e.button;
                if (button === 1) {
                    toggleMenuOff();
                }
            }
        });
    }

    /**
     * Listens for keyup events.
     */
    function keyupListener() {
        window.onkeyup = function (e) {
            if (e.keyCode === 27) {
                toggleMenuOff();
            }
        }
    }

    /**
     * Window resize event listener
     */
    function resizeListener() {
        window.onresize = function (e) {
            toggleMenuOff();
        };
    }

    /**
     * Turns the custom context menu on.
     */
    function toggleMenuOn() {
        if (menuState !== 1) {
            menuState = 1;
            menu.classList.add(contextMenuActive);
        }
    }

    /**
     * Turns the custom context menu off.
     */
    function toggleMenuOff() {
        if (menuState !== 0) {
            menuState = 0;
            menu.classList.remove(contextMenuActive);
        }
    }

    /**
     * Positions the menu properly.
     * 
     * @param {Object} e The event
     */
    function positionMenu(e) {
        clickCoords = getPosition(e);
        clickCoordsX = clickCoords.x;
        clickCoordsY = clickCoords.y;

        menuWidth = menu.offsetWidth + 4;
        menuHeight = menu.offsetHeight + 4;

        windowWidth = window.innerWidth;
        windowHeight = window.innerHeight;

        if ((windowWidth - clickCoordsX) < menuWidth) {
            menu.style.left = windowWidth - menuWidth + "px";
        } else {
            menu.style.left = clickCoordsX + "px";
        }

        if ((windowHeight - clickCoordsY) < menuHeight) {
            menu.style.top = windowHeight - menuHeight + "px";
        } else {
            menu.style.top = clickCoordsY + "px";
        }
    }

    /**
     * Dummy action function that logs an action when a menu item link is clicked
     * 
     * @param {HTMLElement} link The link that was clicked
     */
    function menuItemListener(link) {
        console.log("Task ID - " + taskItemInContext.getAttribute("data-id") + ", Task action - " + link.getAttribute("data-action"));
        var mode = link.getAttribute("data-action");
        if (mode == 'Insert') {
            InsertRowNew();
        } else if (mode == 'Delete') {
            thisTR.remove();
        }

        toggleMenuOff();
    }

    /**
     * Run the app.
     */
    init();
}

function InsertRowNew() {
    var newTR = $(thisTR).clone();
    newTR.find("input").each(function () {
        $(this).val("");
    });
    newTR.insertAfter($(thisTR));
}

$.fn.bindingTableFeasibility = function (Data ,RowHeader) {
    var thisElement = this;
    var Table = $("<table>", {
        class: "table-finan"
    });
    var rowHeader = $("<tr>");
    for (var i = 0; i < RowHeader.length; i++) {
        var colHeader;
        if (typeof RowHeader[i] == "object") {
            colHeader = $("<th>", {
                html: RowHeader[i].lable,
                style: "width:" + RowHeader[i].width + "px"
            });
        } else {
            colHeader = $("<th>", {
                html: RowHeader[i]
            });
        }
        rowHeader.append(colHeader);
    }
    Table.append(rowHeader);

    for (var i = 0; i < Data.length; i++) {
        var rowGroup = $("<tr>");
        var colGroup = $("<th>", {
            html: Data[i].group,
            colspan: 8
        });
        rowGroup.append($("<th>"), colGroup);
        Table.append(rowGroup);
        for (var j = 0; j < Data[i].rowData.length; j++) {
            Table.appendRowDataFinan(Data[i].rowData[j].cell)
        }

        var rowSummery = $("<tr>", {
            class: "row-summery"
        })
        var cellSummeryTitle = $("<td>", {
            colspan: 6,
            html: "Total " + Data[i].group
        });

        var cellSummeryTotal = $("<td>");
        var lableTotal = $("<span>", {
            id: Data[i].group.split(' ')[0] + "_Total_Summery",
            html: "0.00"
        })

        var cellSummeryPersen = $("<td>");
        var lableTotalPersen = $("<span>", {
            id: Data[i].group.split(' ')[0] + "_Total_Persen",
            html: "0.00%"
        })

        cellSummeryTotal.append(lableTotal);
        cellSummeryPersen.append(lableTotalPersen);
        rowSummery.append(cellSummeryTitle, $("<td>"), cellSummeryTotal, cellSummeryPersen);
        Table.append(rowSummery);
    }
    $(this).append(Table);
}

$.fn.appendRowDataFinan = function (Data) {
    var rowData = $("<tr>", {
        class: "row-click"
    })
    for (var i = 0; i < Data.length; i++) {
        var classText = Data[i].classText != undefined ? Data[i].classText : ""
        var inTD;
        var colData = $("<td>");
        if (Data[i].inputType == 'text') {
            colData = $("<td>");
            inTD = $("<input>", {
                type: "text",
                value: Data[i].value,
                class: classText + " tablemodel-text",
            });
            colData.append(inTD);
        }
        else if (Data[i].inputType == 'number') {
            colData = $("<td>");
            inTD = $("<input>", {
                type: "text",
                value: (new NumberFormat(Data[i].value)).toFormatted(),
                style: "text-align:right;",
                class: classText
            });
            colData.append(inTD);
        }
        else if (Data[i].inputType == 'lable') {
            colData = $("<td>", {
                class: "cell-disable " + classText,
                style: "text-align:center"
            });
            inTD = $("<span>", {
                type: "text",
                html: Data[i].value
            });
            colData.append(inTD);
        }

        rowData.append(colData);
    }
    $(this).append(rowData);
}

$.extend($.expr[':'], {
    'containsi': function (elem, i, match, array) {
        return (elem.textContent || elem.innerText || '').toLowerCase()
        .indexOf((match[3] || "").toLowerCase()) >= 0;
    }
});

$.fn.filterRowsInTable = function (tableid) {
    //split the current value of searchInput
    var textbox = $(this);
    textbox.keyup(function () {
        var data = $(this).val();
        //create a jquery object of the rows
        var jo = $("#" + tableid).find("tbody tr");
        if (data == "") {
            jo.show();
            return;
        }
        //hide all the rows
        jo.hide();        

        //Recusively filter the jquery object to get results.
        jo.filter(function (i, v) {
            var $t = $(this);
            if ($t.find("td").is(":containsi('" + data + "')")) {
                return true;
            }
            return false;
        }).show();
    });
}