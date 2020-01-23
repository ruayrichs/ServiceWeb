<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormPrintChange.aspx.cs" Inherits="ServiceWeb.crm.AfterSale.FormPrintChange" %>

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


</style>
</head>
<body>
    <form id="form1" runat="server">
        <page size="A4">
            <div class="PageSetup">

                <div>
                    <div style="width:50%; float:left">SALIL HOTEL GROUP</div>
                    <div style="width:50%; float:right; text-align:right;">P.R. NO.__________</div>
                </div>
                <div style="width:100%;">โรงแรมสลิล กรุ๊ป</div>
                <div style="width:100%; text-align:center;"><b>PURCHASE REQUEST</b></div>
                <br />
                <div>
                    <div style="width:50%; float:left">Branch/Dept.สาขาหรือแผนก__________</div>
                    <div style="width:50%; float:right; text-align:right;">Date/วันที่__________</div>
                </div>
                <br />
                <br />
                <table style="width:100%; font-size:12px;">
                    <thead style="background-color:lavender;">
                        <tr>
                            <th rowspan="1" colspan="1" style="width:auto; font-size:16px; text-align:center; font-size:13px;">
                                Quantity<br />จำนวน
                            </th>
                            <th rowspan="1" colspan="1" style="width:40%; font-size:16px; text-align:center; font-size:13px;">
                                Description in detail<br />รายละเอียด
                            </th>
                            <th rowspan="1" colspan="1" style="width:auto; font-size:16px; text-align:center; font-size:13px;">
                                Last Purchase<br />ขอซื้อคร้งก่อน
                            </th>
                            <th rowspan="1" colspan="1" style="width:auto; font-size:16px; text-align:center; font-size:13px;">
                                Unit Price<br />ราคา/หน่วย
                            </th>
                            <th rowspan="1" colspan="1" style="width:auto; font-size:16px; text-align:center; font-size:13px;">
                                ยอดคงเหลือ
                            </th>
                            <th rowspan="1" colspan="1" style="width:auto; font-size:16px; text-align:center; font-size:13px;">
                                หมายเหตุ
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <th>
                                <dfn>.............</dfn><br />
                                <dfn>.............</dfn><br />
                                <dfn>.............</dfn><br />
                                <dfn>.............</dfn><br />
                                <dfn>.............</dfn><br />
                                <dfn>.............</dfn><br />
                                <dfn>.............</dfn><br />
                                <dfn>.............</dfn><br />
                                <dfn>.............</dfn><br />
                                <dfn>.............</dfn><br />
                                <dfn>.............</dfn><br />
                                <dfn>.............</dfn><br />
                                <dfn>.............</dfn><br />
                                <dfn>.............</dfn><br />
                                <dfn>.............</dfn><br />
                                <dfn>.............</dfn><br />
                                <dfn>.............</dfn><br />
                             
                            </th>
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
                            <th>
                                <dfn>.............</dfn>
                                <dfn>.............</dfn>
                                <dfn>.............</dfn>
                            </th>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                          <td colspan="5" style="text-align:right;">TOTAL AMOUNT / ราคา&nbsp;&nbsp;&nbsp;</td>
                          <td></td>
                        </tr>
                    </tfoot>
                </table>
                <br />
                <div style="width:100%;">Purpose of purchase/จุดประสงค์ในการขอซื้อ</div>
                <br />
                
                <table style="width:100%; border:; font-size:12px;">
                    <tbody>
                        <tr>
                            <th style="border:hidden;">
                                <br />
                                <hr>
                            </th>
                            <th style="border:hidden; width:15%;"></th>
                            <th style="border:hidden; width:30%;">
                                <br />
                                <hr>
                            </th>
                        </tr>
                        <tr>
                            <th style="border:hidden; width:55%;">
                                <br />
                                <hr>
                            </th>
                            <th style="border:hidden; width:15%;"></th>
                            <th style="border:hidden; text-align:center;">
                                <sup style="font-size:12px">Purhasing Dept./ ฝ่ายจัดซื้อ</sup>
                            </th>
                        </tr>
                        <%--<tr>
                            <th style="border:hidden; width:55%;">
                                <br />
                                <hr>
                            </th>
                            <th style="border:hidden; width:15%;"></th>
                            <th style="border:hidden; text-align:center;">
                                <br />
                                <hr>
                            </th>
                        </tr>--%>
                        <tr>
                            <th style="border:hidden; width:55%;">
                                <br />
                                <hr>
                            </th>
                            <th style="border:hidden; width:15%;"></th>
                            <th style="border:hidden; text-align:center;">
                                <br />
                                <hr>
                            </th>
                        </tr>
                    </tbody>
                </table>
                <%--===============Table===============--%>
                <table style="width:100%; font-size:12px;">
                    <tbody>
                        <tr>
                            <th style="border:hidden; width:25%;">
                                Date Reqiuire / วันที่ต้องการใช้
                            </th>
                            <th style="border:hidden;">
                                <br />
                                <hr>
                            </th>
                            <th style="border:hidden; width:15%;"></th>
                            <th style="border:hidden; width:30%; text-align:center;">
                                <sup style="font-size:12px;">Authorized By : Chief Accountant</sup>
                            </th>
                        </tr>
                        <tr>
                            <th style="border:hidden; width:25%;">
                                Remark / หมายเหตุ
                            </th>
                            <th style="border:hidden;">
                                <br />
                                <hr>
                            </th>
                            <th style="border:hidden; width:15%;"></th>
                            <th style="border:hidden; width:30%;">
                                <br />
                                <hr>
                            </th>
                        </tr>
                        <tr>
                            <th style="border:hidden; width:25%;">
                                
                            </th>
                            <th style="border:hidden;">
                                <br />
                                <hr>
                            </th>
                            <th style="border:hidden; width:15%;"></th>
                            <th style="border:hidden; width:30%; text-align:center;">
                                <sup style="font-size:12px">Approved By : General Manager</sup>
                            </th>
                        </tr>
                        <tr>
                            <th style="border:hidden; width:25%;">
                                Request By / ขอซื้อโดย
                            </th>
                            <th style="border:hidden;">
                                <br />
                                <hr>
                            </th>
                            <th style="border:hidden; width:15%;"></th>
                            <th style="border:hidden; width:30%; text-align:center;">
                                <br />
                                <hr>
                            </th>
                        </tr>
                        <tr>
                            <th style="border:hidden; width:25%;">
                                Head / ห้วหน้าแผนก
                            </th>
                            <th style="border:hidden;">
                                <br />
                                <hr>
                            </th>
                            <th style="border:hidden; width:15%;"></th>
                            <th style="border:hidden; width:30%; text-align:center;">
                                <sup style="font-size:13px">Asst. Director</sup>
                            </th>
                        </tr>
                    </tbody>
                </table>
            </div>
        </page>
    </form>
</body>
</html>
