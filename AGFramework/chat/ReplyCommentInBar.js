

$(document).ready(function (event) {
    loadReplyComent();
});

const Convert2DateTimeDisplay = dateTimeDB =>
    dateTimeDB.substring(6, 8) + '/' + dateTimeDB.substring(4, 6) + '/' + dateTimeDB.substring(0, 4) + ' ' +
    dateTimeDB.substring(8, 10) + ':' + dateTimeDB.substring(10, 12) + ':' + dateTimeDB.substring(12, 14);


function loadReplyComent() {
   
    var url = servictWebDomainName + "framework/ag-activity-remark/api/";
    var data = {
        q: "get-reply-in-bar"
    };
    $.ajax({
        url: url,
        data: data,
        success: function (datas) {
            console.log(datas);

            var TotalRemark = datas.length;
            
            if (TotalRemark > 0) {

                var panel = $("<div>", {
                    class: "text-info"
                }).css({
                    float: "right",
                    marginTop: "-45px",
                    cursor: "pointer"
                });

                var div = $("<div>", {
                    class: "btn-group"
                });

                var btn = $("<button>", {
                    type: "button",
                    class: "btn btn-warning btn-sm dropdown-toggle ticket-allow-editor ticket-allow-editor-everyone",
                    "data-toggle": "dropdown",
                    "aria-haspopup": "true",
                    "aria-expanded": "false",
                    html: " " + TotalRemark + " Reply"
                });

                var ddl = $("<div>", {
                    class: "dropdown-menu",
                   //,
                    //"x-placement": "left-start"
                }).css({
                    left: "auto",
                    right: "0px",
                    "max-height" : "340px",
                    "overflow-y" : "scroll"

                    });


                var res_list = $("#reply-list div > ul");
               /* var res_li = $("li", {
                    class:"nav-item"
                });*/
                if (TotalRemark > 10) {
                    
                    $("#res_num_reply").html("10+");
                    $("#num_reply").html("10+");
                    for (var i = 0; i < TotalRemark ; i++) {
                        var data = datas[i];
                        
                        var item = $("<a>", {
                            class: "dropdown-item",
                            href: "Javascript:;",
                            html: data.CreatorFullnameEN + ' ได้ตอบกลับข้อความของคุณเวลา ' + Convert2DateTimeDisplay(data.Reply_On),
                            "data-seq-reply": data.REPLY_SEQ,
                            onclick: "toTicket('" + data.CallerID + "','" + data.Doctype + "','" + data.Fiscalyear + "','" + data.REPLY_SEQ+"')"
                        });
                        ddl.append(item);
                        var res_li = $("<li>", {
                            class: "nav-item"
                        });
                        var res_item = $("<a>", {
                            class: "nav-link",
                            href: "Javascript:;",
                            html: data.CreatorFullnameEN + ' ได้ตอบกลับข้อความของคุณเวลา ' + Convert2DateTimeDisplay(data.Reply_On),
                            "data-seq-reply": data.REPLY_SEQ,
                            onclick: "toTicket('" + data.CallerID + "','" + data.Doctype + "','" + data.Fiscalyear + "','" + data.REPLY_SEQ + "')"
                        });
                        res_li.append(res_item);
                        res_list.append(res_li)
                    }



                } else {
                    $("#res_num_reply").html(TotalRemark);
                    $("#num_reply").html(TotalRemark);
                    for (var i = 0; i < TotalRemark; i++) {
                        var data = datas[i];

                        var item = $("<a>", {
                            class: "dropdown-item",
                            href: "Javascript:;",
                            html: data.CreatorFullnameEN + ' ได้ตอบกลับข้อความของคุณเวลา ' + Convert2DateTimeDisplay(data.Reply_On),
                            "data-seq-reply": data.REPLY_SEQ,
                            onclick: "toTicket('" + data.CallerID + "','" + data.Doctype + "','" + data.Fiscalyear + "','" + data.REPLY_SEQ +"')"
                        });
                        ddl.append(item);
                        var res_li = $("<li>", {
                            class: "nav-item"
                        });
                        var res_item = $("<a>", {
                            class: "nav-link",
                            href: "Javascript:;",
                            html: data.CreatorFullnameEN + ' ได้ตอบกลับข้อความของคุณเวลา ' + Convert2DateTimeDisplay(data.Reply_On),
                            "data-seq-reply": data.REPLY_SEQ,
                            onclick: "toTicket('" + data.CallerID + "','" + data.Doctype + "','" + data.Fiscalyear + "','" + data.REPLY_SEQ + "')"
                        });
                        res_li.append(res_item);
                        res_list.append(res_li)
                       
                    }
                }
               div.append(btn);
               div.append(ddl);
                $("#menu_reply").prepend(ddl);
            }
        }
    });
}


function toTicket(ticketNo, docType, Fiscalyear,refCode) {
    console.log(ticketNo + ":" + docType + ":" + Fiscalyear);
    $("#txtReply_TicketNo").val(ticketNo);
    $("#txtReply_DocType").val(docType);
    $("#txtReply_Filcalyear").val(Fiscalyear);
    $("#txtReply_RefCode").val(refCode);
    $("#btnToTicket").click();
}