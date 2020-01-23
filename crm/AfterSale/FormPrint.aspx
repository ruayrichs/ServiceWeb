<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormPrint.aspx.cs" Inherits="ServiceWeb.crm.AfterSale.FormPrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
body {
  background: rgb(204,204,204); 
}
page {
  background: white;
  display: block;
  margin: 0 auto;
  margin-bottom: 0.5cm;
  box-shadow: 0 0 0.5cm rgba(0,0,0,0.5);
}
page[size="A4"] {  
  width: 21cm;
  height: 29.7cm; 
}
page[size="A4"][layout="landscape"] {
  width: 29.7cm;
  height: 21cm;  
}
page[size="A3"] {
  width: 29.7cm;
  height: 42cm;
}
page[size="A3"][layout="landscape"] {
  width: 42cm;
  height: 29.7cm;  
}
page[size="A5"] {
  width: 14.8cm;
  height: 21cm;
}
page[size="A5"][layout="landscape"] {
  width: 21cm;
  height: 14.8cm;  
}
@media print {
  body, page {
    margin: 0;
    box-shadow: 0;
  }
}
.PageSetup {
    padding-top: 0.5cm;
    padding-left: 2cm;
    padding-right: 1.5cm;
    padding-bottom: 0.5cm;
    text-align: left;    
}
div {
    /*border:1px solid black;*/
}
dfn {
    /*font-size:5px;*/
}
.ims {
    width: 100%;
    height: auto;
}
.logoSize {
    width: 20%;
}
table, th, td {
  /*border-left: 0px solid black;
  border-right: 0px solid black;*/
  border: 1.5px solid black;
  border-collapse: collapse;
  font-weight:100;
}
.btnDrive {
    padding:50px;
    display:none;
}
.inputnone {border:0;outline:0;}
.inputnone:focus {outline:none!important;}



</style>
    <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.4.1.min.js"></script>
    <%--PDF.JS--%>
    <%--<script src="h ttps://cdn.jsdelivr.net/npm/pdfjs-dist@2.1.266/build/pdf.min.js"></script>--%>
    <%--JSPDF--%>
    <%--<script src="h ttps://unpkg.com/jspdf@latest/dist/jspdf.min.js"></script>--%>
</head>
<body onLoad="myFunction()">
    <script>
        function myFunction() {
            $(document).ready(function () {
               // alert('sdf');
                
                //var loadingTask = pdfjsLib.getDocument('helloworld.pdf');
                //loadingTask.promise.then(function (pdf) {
                //    // you can now use *pdf* here
                //});
            });
        }
    </script>
    <script>
        var is_chrome = function () { return Boolean(window.chrome); }
        if (is_chrome) {
            var result = confirm("Want to Edit?");
            if (result) {
                
            } else {
                window.print();
                window.save();
                setTimeout(function () { window.close(); }, 10000);
                //give them 10 seconds to print, then close
            }            
        }
        else {
            window.print();
            window.close();
        }
</script>
    <form id="form1" runat="server">
        <script src="https://apis.google.com/js/platform.js" async defer></script>
        <div class="btnDrive" id="btnDrive" onclick="btnDrive">
            <div class="g-savetodrive"
               data-src="FormPrint.aspx"
               data-filename="MyStatement.pdf"
               data-sitename="Service One Solution">
            </div>
        </div>
        
        <page size="A4">
            <div class="PageSetup">
                <div style="width:100%; height:1081px;">

                    <div style="width:20%; float: left;">
                        <asp:Image id="ImageLogo" class="ims" runat="server" imageurl="http://www.itg.co.th/Images/Logo.png" />
                    </div>
                    <div style="width:80%; height:50px; float: left">
                        <div style="height:25%; text-align: center; bottom:0;"></div>
                        <div style="height:auto;">
                            <p class="text-justify" style="font-size: 11px">
                                <b style="font-size: 12px">INFORMATION TECTNOLOGY GROUP CO.,LTD.</b><br />
                                200 Moo 4, 18th floor, Unit 1801A and 25thFloor,Jasmine International Tower Chaengwattana Road, Pakkret,
                            </p>
                        </div>
                    </div>

                    <div style="width:100%; height:auto; float:left; font-size:12px;">
                        <div>
                            <%--///JOB ORDER///--%>
                            <div>
                                <table style="width:100%; border:hidden;">
                                    <thead style="border:hidden; background-color:lavender">
                                        <tr>
                                            <th rowspan="1" colspan="1" style="width:50%; font-size:18px; font-weight:900; text-align:center;">
                                                JOB ORDER / WORK OPENING SHEET / ใบสั่งงาน
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <th>
                                                <br />
                                                เลขที่ใบสั่งงาน<dfn>......<%= TicketNumber %>.......</dfn>
                                                ผู้เปิดงาน<dfn>.......<%= CreatedBy %>......</dfn>
                                                วันที่รับงาน<dfn>......<%= DateTimeCreated %>.......</dfn>
                                            </th>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div>
                                <table style="width:100%">
                                    <%--<thead style="background-color:lavender">
                                        <tr>
                                            <th rowspan="1" colspan="1" style="width:50%; font-size:16px; text-align:center;">
                                                ปัญหาที่รับแจ้ง / คำสั่งงาน / แนะนำวิธีแก้ไข
                                            </th>
                                        </tr>
                                    </thead>--%>
                                    <tbody>
                                        <tr>
                                            <th>
                                                ชื่อร้านค้า<dfn>.......<%= ClientName %>......</dfn>
                                                ชื่อผู้ติดต่อ<dfn>.......<%= ContactName %>......</dfn>
                                                เบอร์โทรศัพท์<dfn>.......<%= ContactPhone %>......</dfn>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden">
                                                ที่อยู่<dfn>.......<%= CustomerProfile.Address.Trim()%>......</dfn>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden">
                                                ประเภทงาน<dfn>.......<%= TicketType %>......</dfn>
                                                Owner Service<dfn>.......<%= OwnerServiceCI %>......</dfn>

                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden">
                                                ชื่อผู้แจ้ง<dfn>.......<%= ClientName %>......</dfn>
                                                เบอร์โทรศัพท์<dfn>.......<%= Request.QueryString["ctp"] %>......</dfn>                                                
                                                วันที่ดำเนินการ<dfn>.......<%= CallBackDateTime %>......</dfn>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden">
                                                เครื่องรุ่น<dfn>.......<%= CIName %>......</dfn>
                                                Serial No.<dfn>.......<%= SerialNo %>......</dfn>
                                                <%--Print S/N<dfn>.............</dfn>--%>
                                                Communication<dfn>.............</dfn>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden">
                                                <div id="panelAttributes" class="tab-pane">
                                                    <asp:Repeater runat="server" ID="rptAttributes">
                                                        <ItemTemplate>
                                                            App. Name / version<dfn>.......<%# Eval("xValue") %>......</dfn>&nbsp;
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </th>
                                        </tr>
                                        <%--<tr>
                                            <th style="border-top:hidden">
                                                App. Name / version<dfn>.............</dfn>
                                            </th>
                                        </tr>--%>
                                    </tbody>
                                </table>
                            </div>
                            <%--///ปัญหาที่แจ้ง///--%>
                            <div>
                                <table style="width:100%">
                                    <thead style="border-top:hidden; background-color:lavender">
                                        <tr>
                                            <th rowspan="1" colspan="1" style="width:50%; font-weight:700; font-size:16px; text-align:center;">
                                                ปัญหาที่รับแจ้ง / คำสั่งงาน / แนะนำวิธีแก้ไข
                                            </th>
                                        </tr>
                                    </thead>
                                    <%--<b>Subject : </b>--%><%--<%= Subject %>--%>
<%--<b>Description : </b><%= Description %>--%>
                                    <tbody>
                                        <tr>
                                            <th style="height:auto; vertical-align:top;">
                                                <pre style="height:250px"><textarea style="border: none; overflow:hidden; resize:none; width:99.4%; height:250px;"><%= Subject %></textarea>
                                                </pre>                                                
                                            </th>                                             
                                        </tr>
                                        <%--<tr>
                                            <th style="border-top:hidden">
                                                2
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden">
                                                2
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden">
                                                2
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden">
                                                2
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden">
                                                2
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden">
                                                2
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden">
                                                2
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden">
                                                2
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden">
                                                2
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden">
                                                2
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden">
                                                2
                                            </th>
                                        </tr>--%>
                                    </tbody>
                                </table>
                            </div>
                            <%--///รายละเอียด///--%>
                            <div>
                                <table style="width:100%">
                                    <thead style="border-top:hidden; text-align:center;">
                                        <tr style="background-color:lavender; font-size:12px;">
                                            <th rowspan="1" colspan="4">
                                                <b>รายละเอียดของเครื่อง</b>
                                            </th>
                                            <th rowspan="1" colspan="5">
                                                <b>Partition</b>
                                            </th>
                                            <th rowspan="1" colspan="6">
                                                <b>Function</b>
                                            </th>
                                            <th rowspan="1" colspan="2">
                                                <b>TYPE</b>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="width:4.545454545454545%;">No.</th>
                                            <th>Owner Service</th>
                                            <th style="text-align:center;">Terminal ID</th>
                                            <th style="text-align:center;">Merchant ID</th>
                                            <th style="width:4.545454545454545%;">V</th>
                                            <th style="width:4.545454545454545%;">M</th>
                                            <th style="width:4.545454545454545%;">T</th>
                                            <th style="width:4.545454545454545%;">J</th>
                                            <th style="width:4.545454545454545%;">C</th>
                                            
                                            <th style="width:4.545454545454545%;">Key</th>
                                            <th style="width:4.545454545454545%;">Adj</th>
                                            <th style="width:4.545454545454545%;">Auth</th>
                                            <th style="width:4.545454545454545%;">Off</th>
                                            <th style="width:4.545454545454545%;">Ref</th>
                                            <th style="width:4.545454545454545%;">Tips</th>

                                            <th style="width:4.545454545454545%;">ON US</th>
                                            <th style="width:4.545454545454545%;">OFF US</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr style="text-align:center">
                                            <th>1</th>
                                            <th><%= OwnerServiceCI %></th>
                                            <th><%= TIDMethod() %></th>
                                            <th><%= MIDMethod() %></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                        <tr style="border-top:hidden; text-align:center">
                                            <th>2</th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                        <tr style="border-top:hidden; text-align:center">
                                            <th>3</th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                        <tr style="border-top:hidden; text-align:center">
                                            <th>4</th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                        <tr style="border-top:hidden; text-align:center">
                                            <th>5</th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <%--///check list///--%>
                            <div>
                                <table style="width:100%">
                                    <thead style="font-size:12; border-top:hidden; text-align:center; background-color:lavender">
                                        <tr>
                                            <th rowspan="1" colspan="1" >
                                                <b>ข้อมูลสำหรับช่าง : Check list</b>
                                            </th>
                                            <th rowspan="1" colspan="1" style="width:4.545454545454545%">
                                                <b>Y</b>
                                            </th>
                                            <th rowspan="1" colspan="1" style="width:4.545454545454545%">
                                                <b>N</b>
                                            </th>
                                            <th rowspan="1" colspan="1" style="width:50%">
                                                <b>สำหรับช่าง : รายละเอียดการตรวจสอบและการแก้ไข</b>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <th><div style="margin-left:4%">
                                                1&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                แนะนำตัว/ตรวจสอบชื่อร้าน
                                                </div>
                                            </th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden"><div style="margin-left:4%">
                                                2&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                ปฏิบัติงานตามใบงาน
                                                </div>
                                            </th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden"><div style="margin-left:4%">
                                                3&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                เช็คข้อมูลใน Slip (TID, MID, ชื่อร้าน, Config)
                                                </div>
                                            </th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden"><div style="margin-left:4%">
                                                4&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                Training ลูกค้า
                                                </div>
                                            </th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden"><div style="margin-left:4%">
                                                5&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                เขียนสรุปงาน
                                                </div>
                                            </th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden"><div style="margin-left:4%">
                                                6&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                ให้ร้านค้าลงนาม, ชื่อร้านค้า, เบอร์โทรศัพท์
                                                </div>
                                            </th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden"><div style="margin-left:4%">
                                                7&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                โทรปิดงานกับ CS Admin
                                                </div>
                                            </th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                        <tr>
                                            <th style="border-top:hidden"><div style="margin-left:4%">
                                                8&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                อื่น ๆ
                                                </div>
                                            </th>
                                            <th></th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <%--///ประเมินผล///--%>
                            <div>
                                <table style="width:100%">
                                    <thead style="font-size:12; text-align:center; border-top:hidden; background-color:lavender">
                                        <tr>
                                            <th rowspan="1" colspan="1" style="width:50%">
                                                <b>สำหรับร้านค้า : ประเมินผลการใช้บริการ</b>
                                            </th>
                                            <th rowspan="1" colspan="1">
                                                <b>สำหรับช่าง : สรุปผลการดำเนินการ</b>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <th rowspan="1" colspan="1">
                                                <div style="border:0px solid red">
                                                    <div style="border:0px solid blue; width:100%;">
                                                        <div style="border:0px solid blue; width:50%; float:left;">
                                                            <div style="border:1px solid black; width:20%; margin-left:10px; height:10px; float:left;"></div>
                                                            &nbsp;A ดีมาก (90-100)
                                                        </div>
                                                        <div style="border:0px solid blue; width:50%; float:right;">
                                                            <div style="border:1px solid black; width:20%; margin-left:10px; height:10px; float:left;"></div>
                                                            &nbsp;B ดี (80-89)
                                                        </div>
                                                    </div>
                                                </div>
                                                <div style="border:0px solid red">
                                                    <div style="border:0px solid blue; width:100%;">
                                                        <div style="border:0px solid blue; width:50%; float:left;">
                                                            <div style="border:1px solid black; width:20%; margin-left:10px; height:10px; float:left;"></div>
                                                            &nbsp;C พอใช้ (60-79)
                                                        </div>
                                                        <div style="border:0px solid blue; width:50%; float:right;">
                                                            <div style="border:1px solid black; width:20%; margin-left:10px; height:10px; float:left;"></div>
                                                            &nbsp;D ปรับปรุง (50-59)
                                                        </div>
                                                    </div>
                                                </div>
                                                <div style="border:0px solid red">
                                                    <div style="border:0px solid blue; width:100%;">
                                                        <div style="border:0px solid blue; width:100%; float:left;">
                                                            <div style="border:1px solid black; width:10%; margin-left:10px; height:10px; float:left;"></div>
                                                            &nbsp;ควรปรับปรุง (น้อยกว่า 49)<dfn>..................................................</dfn><br />
                                                            <dfn>...........................................................................................................</dfn><br />
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                            </th>
                                            <th>
                                               <div style="border:0px solid red">
                                                    <div style="border:0px solid blue; width:100%;">
                                                        <div style="border:0px solid blue; width:100%; float:left;">
                                                            <div style="border:1px solid black; width:10%; margin-left:10px; height:10px; float:left;"></div>
                                                            &nbsp;เสร็จสมบูรณ์
                                                        </div>
                                                    </div>
                                                </div>
                                                <div style="border:0px solid red">
                                                    <div style="border:0px solid blue; width:100%;">
                                                        <div style="border:0px solid blue; width:100%; float:left;">
                                                            <div style="border:1px solid black; width:10%; margin-left:10px; height:10px; float:left;"></div>
                                                            &nbsp;ไม่เสร็จเนื่องจาก<dfn>................................................................</dfn><br />
                                                            <dfn>...........................................................................................................</dfn><br />
                                                            <dfn>...........................................................................................................</dfn><br />
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                            </th>
                                            
                                        </tr>
                                        <tr>
                                            <th>
                                                ชื่อลูกค้า<dfn>...................................</dfn>เบอร์โทรศัพท์<dfn>...................................</dfn><br />
                                                ชื่อร้านค้า<dfn>..................................</dfn>วันที่ดำเนินการ<dfn>..................................</dfn><br />
                                            </th>
                                            <th>
                                                ชื่อผู้ปฏิบัติงาน<dfn>...................................................................................</dfn><br />
                                                วันที่<dfn>........................</dfn>เวลาเข้า<dfn>........................</dfn>เวลาออก<dfn>.......................</dfn><br />
                                            </th>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <%--///หมายเหตุ///--%>
                            <div>
                                <p style="font-size: 10px">***หมายเหตุ***</p>
                                <p style="font-size: 10px">
                                    หากท่านมีความคิดเห็นเพิ่มเติม ซึ่งอาจมีแสดงในเอกสารฉบับนี้ได้ สามารถแจ้งความคิดเห็นของท่านส่ง E-mail มายังบริษัทฯจักเป็นพระคุณยิ่ง<br />
                                    <b>E-mail </b>: ITG-ContactCenter@itg.co.th
                                </p>
                            </div>


                            <%--test--%>
                            <%--<asp:TextBox ID="txtContactPhonePrint" Enabled="false" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>--%>
                            <%--<ag:AutoCompleteControl runat="server" id="_ddl_contact_person" CustomViewCode="contact" TODO_FunctionJS="loadcontactDetailBySelectedFormServiceCallTransaction();" CssClass="form-control form-control-sm" />
                            <asp:UpdatePanel ID="udpContactRefresh" UpdateMode="Conditional" runat="server">--%>
                            <%--<asp:TextBox ID="lblCustomerCode" Enabled="false" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>--%>
                            <%--<div id="panelAttributes" class="tab-pane">--%>
                                <%--<asp:UpdatePanel runat="server" ID="udpAttributes" UpdateMode="Conditional">--%>
                                    <%--<ContentTemplate>
                                        <fieldset class="fieldset-defult">
                                            <legend class="legend-defult">Attributes Box
                                            </legend>
                                            <div style="padding: 0px 8px;">
                                                <div class="form-row">--%>
                                                    <%--<asp:Repeater runat="server" ID="rptAttributes">
                                                        <ItemTemplate>--%>
                                                            <%--<div class="form-group col-sm-12 col-md-6" style="margin-bottom: 10px;">--%>
                                                                <%--<%# Eval("Description") %> : <%# Eval("xValue") %>--%>
                                                            <%--</div>--%>
                                                        <%--</ItemTemplate>
                                                    </asp:Repeater>--%>
                                                <%--</div>
                                            </div>
                                        </fieldset>
                                    </ContentTemplate>--%>
                                <%--</asp:UpdatePanel>--%>
                            <%--</div>--%>



                        </div>
                    </div>
                    
                </div>

            </div>            
        </page>

    </form>
</body>
</html>
