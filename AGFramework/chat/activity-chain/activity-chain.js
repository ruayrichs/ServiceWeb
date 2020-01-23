$.fn.generateCircleProgressChart = function () {

    var el = $(this); // get canvas

    var options = {
        percent: el.attr('data-percent') || 25,
        size: el.attr('data-size') || 100,
        lineWidth: el.attr('data-line') || 5,
        rotate: el.attr('data-rotate') || 0
    }

    var canvas = document.createElement('canvas');
    var span = document.createElement('span');
    span.textContent = options.percent + '%';

    if (typeof (G_vmlCanvasManager) !== 'undefined') {
        G_vmlCanvasManager.initElement(canvas);
    }

    var ctx = canvas.getContext('2d');
    canvas.width = canvas.height = options.size;

    el.append(span);
    el.append(canvas);

    ctx.translate(options.size / 2, options.size / 2); // change center
    ctx.rotate((-1 / 2 + options.rotate / 180) * Math.PI); // rotate -90 deg

    //imd = ctx.getImageData(0, 0, 240, 240);
    var radius = (options.size - options.lineWidth) / 2;

    var drawCircle = function (color, lineWidth, percent) {
        percent = Math.min(Math.max(0, percent || 1), 1);
        ctx.beginPath();
        ctx.arc(0, 0, radius, 0, Math.PI * 2 * percent, false);
        ctx.strokeStyle = color;
        ctx.lineCap = 'round'; // butt, round or square
        ctx.lineWidth = lineWidth
        ctx.stroke();
    };

    var bar = options.percent / 100;
    drawCircle('#efefef', options.lineWidth, 100 / 100);
    drawCircle('#0085A1', options.lineWidth, (bar == 0 ? 0.0000001 : bar));
};

$.fn.generateScalableChart = function () {
    var elt = $(this);
    var container = $(".scalable-chart-container .bubble-container");

    var bubble = $("<div/>", {
        class: "bubble",
        "data-percent": elt.attr("data-percent"),
        "data-level": elt.attr("data-level"),
        "data-code": elt.attr("data-code"),
        "data-parent": elt.attr("data-parent"),
        "data-relation": elt.attr("data-relation")
    });

    bubble.click(function () {
        $(".bubble[data-parent='" + $(this).attr("data-code") + "']").fadeToggle();
    });

    var detail = $("<div/>", {
        class: "detail",
        html: elt.attr("data-template")
    });

    bubble.append(detail);
    container.append(bubble);
}

function bindCircleProgressChart() {
    $(".circle-progress-chart").each(function () {
        $(this).generateCircleProgressChart();
        $(this).generateScalableChart();
    });
    processBubbleScalale(0);
    $("#modal-chain-chart").modal("show");
    agroLoading(false);
}

function openProgressChartModal(id,subject) {
    agroLoading(true);
    $("#modal-chain-chart .modal-title").html(subject);
    $("#txtChartChainHeaderCode").val(id);
    $("#btnChartChainHeaderCode").click();
}

function processBubbleScalale(level) {
    var size = 60;
    size = size - (10 * level);

    var fontSize = 20;
    fontSize = fontSize - (5 * level);

    var elts = $(".bubble[data-level='" + level + "']");
    var inx = 1;
    var stackState = "";
    elts.each(function (inx) {

        var num = inx + 1;
        if (level > 0) {
            num = $(".bubble[data-code='" + $(this).attr("data-parent") + "']").attr("data-number") + "." + num;
        }

        $(this).append(num).css({
            width: size,
            height: size,
            "line-height": size + "px",
            fontSize: fontSize,
            cursor: ($(".bubble[data-parent='" + $(this).attr("data-code") + "']").length > 0 ? "pointer" : "")
        }).toggle(level == 0).attr("data-number", num);

        if (level == 0) {
            var percent = parseInt($(this).attr("data-percent"));
            if (stackState == "" || stackState == "success") {
                $(this).addClass("process");
                stackState = "process";
            }

            if (percent == 100) {
                $(this).addClass("success");
                stackState = "success";
            }
        }
        else {
            if ($(".bubble[data-code='" + $(this).attr("data-parent") + "']").hasClass("process")) {
                $(this).addClass("process");
            }
            if ($(".bubble[data-code='" + $(this).attr("data-parent") + "']").hasClass("success")) {
                $(this).addClass("success");
            }
        }

    });
    if (elts.length > 0) {
        processBubbleScalale(level + 1);
    }
}

function rebindDataGridUserControl(str) {
    console.log($("#usercontrol").length);
    $('#master-plan-table-usercontrol').datagrid();
    $("#usercontrol .panel-title").html(str + " Progress");
    console.log( $("#usercontrol .panel-title").html());
}