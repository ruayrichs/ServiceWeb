<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormPrintAnaMai.aspx.cs" Inherits="ServiceWeb.crm.AfterSale.FormPrintAnaMai" %>

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
    /*border:1px solid red;*/
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
hr.type_3 {
border: 0;
height: 25px;
background-image: url(image/type_3.png);
background-repeat: no-repeat;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        
        <page size="A4">
            <div class="PageSetup">

                <div style="text-align:right; clear:both; width:100%;">เลขที่ใบแจ้งซ่อม..................</div>

                <table style="border:hidden" width="100%">
                    <tr style="border:hidden">
                        <td style="border:hidden" rowspan="3" width="28%">
                            <asp:Image id="ImageLogo" class="ims" runat="server" src="../../Images/f1-logo.png" />
                        </td>
                        <td height="16px"></td>
                    </tr>
                    <tr style="border:hidden">
                        <td><b>ใบแจ้งซ่อมอาคารสถานที่ และระบบสาธารณูปโภค</b></td>
                    </tr>
                    <tr style="border:hidden">
                        <td>อาคารพจน์ สารสิน มหาวิทยาลัยขอนแก่น</td>
                    </tr>
                </table>

                <hr style="border-top: 1px dashed black;">
                <br />
                <div style="width:100%;"><b><ins>ส่วนที่ 1 สำหรับผ้ใช้บริการ</ins></b></div>
                <div style="font-size:12px">
                <table style="width:100%;">
                    <tbody>
                        <tr>
                            <th style="width:50%; border:hidden;">
                                ชื่อผู้แจ้งซ่อม นาย/นาง/นางสาว
                                <dfn>.................</dfn>
                            </th>
                            <th style="border:hidden;">
                                ตำแหน่ง
                                <dfn>.............</dfn>
                            </th>
                            <th style="border:hidden;">
                                กลุ่มภาระกิจ
                                <dfn>.............</dfn>
                            </th>
                        </tr>
                    </tbody>
                </table>
                    <table style="width:100%;">
                    <tbody>
                        <tr>
                            <th style="width:37.5%; border:hidden;">
                                สะถานที่
                                <dfn>............</dfn>
                            </th>
                            <th style="width:12.5%; border:hidden;">
                                ห้อง
                                <dfn>........</dfn>
                            </th>
                            <th style="width:12.5%; border:hidden;">
                                ชั้น
                                <dfn>.......</dfn>
                            </th>
                            <th style="border:hidden;">
                                หมายเลขครุภัณฑ์
                                <dfn>.............</dfn>
                            </th>
                        </tr>
                    </tbody>
                </table>
                </div> 
                
                <%--<div style="width:100%;">ชื่อผู้แจ้งซ่อม นาย/นาง/นางสาว........</div>
                <div style="width:100%;">สถานที่............</div>--%>

                <%--รายการแจ้งซ่อม--%>
                <table style="width:100%">
                    <thead style="background-color:lavender">
                        <tr>
                            <th rowspan="1" colspan="1" style="width:auto; font-size:16px; text-align:center;">
                                รายการแจ้งซ่อม
                            </th>
                            <th rowspan="1" colspan="1" style="width:33.33333333333333%; font-size:16px; text-align:center;">
                                รายละเอียด
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <th>
                                <dfn>.............</dfn>
                                <dfn>.............</dfn>
                                <dfn>.............</dfn>
                            </th>
                            <th>
                                <dfn>.............</dfn>
                                <dfn>.............</dfn>
                                <dfn>.............</dfn>
                            </th>
                        </tr>
                    </tbody>
                </table>
                <%--ส่วนที่2--%>
                <table style="width:100%; border-top:hidden;">
                    <thead style="background-color:lavender">
                        <tr>
                            <th rowspan="1" colspan="1" style="width:auto; font-size:16px; text-align:center;">
                                <ins>ส่วนที่ 2 สำหรับผู้ปฎิบัติงาน</ins>
                            </th>
                            <th rowspan="1" colspan="1" style="width:33.33333333333333%; font-size:16px; text-align:center;">
                                <ins>ส่วนที่ 2 สำหรับผู้ปฎิบัติงาน</ins>
                            </th>
                            <th rowspan="1" colspan="1" style="width:33.33333333333333%; font-size:16px; text-align:center;">
                                <ins>ส่วนที่ 2 สำหรับผู้ปฎิบัติงาน</ins>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <th>
                                ชื่อร้านค้า<dfn>.............</dfn>
                                ชื่อผู้ติดต่อ<dfn>.............</dfn>
                                เบอร์โทรศัพท์<dfn>.............</dfn>
                            </th>
                            <th>
                                ชื่อร้านค้า<dfn>.............</dfn>
                                ชื่อผู้ติดต่อ<dfn>.............</dfn>
                                เบอร์โทรศัพท์<dfn>.............</dfn>
                            </th>
                            <th>
                                ชื่อร้านค้า<dfn>.............</dfn>
                                ชื่อผู้ติดต่อ<dfn>.............</dfn>
                                เบอร์โทรศัพท์<dfn>.............</dfn>
                            </th>
                        </tr>
                    </tbody>
                </table><br />
                <%--แบบสอบถาม--%>
                <div style="width:100%;"><b><ins>แบบสอบถามความพึงพอใจของผู้รับบริการ</ins></b></div>
                <table style="width:100%">
                    <thead style="font-size:12; text-align:center; background-color:lavender">
                        <tr>
                            <td rowspan="2">ประเด็นวัดความพึงพอใจ</td>
                            <td colspan="4">ระดับความพึงพอใจ</td>
                        </tr>
                        <tr>
                            <td style="width:11.11111111111111%">ดีมาก</td>
                            <td style="width:11.11111111111111%">ดี</td>
                            <td style="width:11.11111111111111%">พอใช้</td>
                            <td style="width:11.11111111111111%">พอใช้</td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <th colspan="5">&nbsp;1.</th>
                        </tr>
                        <tr>
                            <th><div style="margin-left:4%">
                                2&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                ปฏิบัติงานตามใบงาน
                                </div>
                            </th>
                            <th>1</th>
                            <th>1</th>
                            <th>1</th>
                            <th>1</th>
                        </tr>
                        <tr>
                            <th><div style="margin-left:4%">
                                2&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                ปฏิบัติงานตามใบงาน
                                </div>
                            </th>
                            <th>1</th>
                            <th>1</th>
                            <th>1</th>
                            <th>1</th>
                        </tr>
                        <tr>
                            <th colspan="5">&nbsp;2.</th>
                        </tr>
                        <tr>
                            <th><div style="margin-left:4%">
                                2&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                ปฏิบัติงานตามใบงาน
                                </div>
                            </th>
                            <th>1</th>
                            <th>1</th>
                            <th>1</th>
                            <th>1</th>
                        </tr>
                        <tr>
                            <th><div style="margin-left:4%">
                                2&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                ปฏิบัติงานตามใบงาน
                                </div>
                            </th>
                            <th>1</th>
                            <th>1</th>
                            <th>1</th>
                            <th>1</th>
                        </tr>
                        <tr>
                            <th colspan="5">&nbsp;3.</th>
                        </tr>
                        <tr>
                            <th><div style="margin-left:4%">
                                2&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                ปฏิบัติงานตามใบงาน
                                </div>
                            </th>
                            <th>1</th>
                            <th>1</th>
                            <th>1</th>
                            <th>1</th>
                        </tr>
                        <tr>
                            <th><div style="margin-left:4%">
                                2&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                ปฏิบัติงานตามใบงาน
                                </div>
                            </th>
                            <th>1</th>
                            <th>1</th>
                            <th>1</th>
                            <th>1</th>
                        </tr>
                    </tbody>
                </table>
                <div style="width:100%">ข้อเสนอแนะ.......................................</div>
                <div style="width:100%">................................................</div>
                
            </div>
        </page>
    </form>
</body>
</html>
