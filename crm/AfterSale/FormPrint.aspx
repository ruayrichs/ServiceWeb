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
  /*border: 1.5px solid black;
  border-collapse: collapse;
  font-weight:100;*/
  /*border: 0;*/
}
.btnDrive {
    padding:50px;
    display:none;
}
.inputnone {border:0;outline:0;}
.inputnone:focus {outline:none!important;}

div {
    /*border: 1px solid red*/
}
table, th, tr, td {
    border: 0;
    border:hidden;
    font-weight:100;
  /*border: 3px solid red;*/
}
tr.noBorder td {
    border: 0;
  border:hidden;
}

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
                    <div style="width:100%; height:auto; float:left; font-size:12px;">
                        <div>
                            <br /><br /><br />
                            <div>
                                <table style="width:100%; border:hidden;">
                                    <thead style="border:hidden;">
                                        <tr>
                                            <th rowspan="1" colspan="1" style="font-size:8px; font-weight:500; text-align:right;">
                                                Reference No: M-SOP-ENG-X-023
                                            </th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                            <div>
                                <table style="width:100%; border:hidden;">
                                    <thead style="border:hidden;">
                                        <tr>
                                            <th rowspan="1" colspan="3" style="width:50%; font-size:20px; font-weight:500; text-align:center;">
                                                Thai Meiji Pharmaceutical Co.,Ltd.
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <th>
                                                <div style="text-align:left">&nbsp;</div>
                                                <div style="text-align:left">No.: .........../...........</div>
                                            </th>
                                            <th>
                                                <div style="text-align:center; font-size:14px;">REQUEST TO ENGINEERING SECTION</div>
                                                <div style="text-align:right">&nbsp;</div>
                                            </th>
                                            <th>
                                                <div style="text-align:right">Date: ............</div>
                                                <div style="text-align:right">Issuer Dept. / Sect: ............</div>
                                            </th>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
<div style="border: 1px solid black">

                            <div>
                                <table style="width:100%; border:hidden;">
                                    <tbody style="border:hidden;">
                                        <tr>
                                            <th>
                                                <div style="text-align:left">1. Type of machines</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left"><img style="width:11px;height:11px;" src="https://image0.flaticon.com/icons/png/512/24/24396.png">&nbsp; 3rd Party Service</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left"><img style="width:11px;height:11px;" src="https://image0.flaticon.com/icons/png/512/24/24396.png">&nbsp; 3rd Party Service</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left"><img style="width:11px;height:11px;" src="https://image0.flaticon.com/icons/png/512/24/24396.png">&nbsp; 3rd Party Service</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th>
                                                <div style="text-align:left">2. Type of works</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left"><img style="width:11px;height:11px;" src="https://image0.flaticon.com/icons/png/512/24/24396.png">&nbsp; 3rd Party Service</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left"><img style="width:11px;height:11px;" src="https://image0.flaticon.com/icons/png/512/24/24396.png">&nbsp; 3rd Party Service</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left"><img style="width:11px;height:11px;" src="https://image0.flaticon.com/icons/png/512/24/24396.png">&nbsp; 3rd Party Service</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th>
                                                <div style="text-align:left">&nbsp;</div>
                                            </th>
                                            <th rowspan="1" colspan="3">
                                                <div style="text-align:left"><img style="width:11px;height:11px;" src="https://image0.flaticon.com/icons/png/512/24/24396.png">&nbsp; Other (Piease identify) .........................................................................</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="1" colspan="4">
                                                <div style="text-align:left">&nbsp;&nbsp;&nbsp;Machone/Equipment Name: ..............................................................................  ENG. Code/ Assel Code: ........................................</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="1" colspan="4">
                                                <div style="text-align:left">&nbsp;&nbsp;&nbsp;Location/Room name: .....................</div>
                                            </th>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div>
                                <table style="width:100%; border:hidden;">
                                    <tbody style="border:hidden;">
                                        <tr style="border:hidden;">
                                            <th rowspan="1" colspan="5">
                                                <div style="text-align:left">3. Cause of request or User Requirement Specication</div>
                                            </th>
                                        </tr>
                                        <tr style="border:hidden;">
                                            <th rowspan="1" colspan="5">
                                                <div style="text-align:left; border: 2px solid black;  border-radius: 8px; margin-left:10px; margin-right:10px;">&nbsp;&nbsp;&nbsp;Description:<br />&nbsp;<br />&nbsp;<br />&nbsp;<br />&nbsp;<br />&nbsp;<br />&nbsp;</div>
                                            </th>
                                        </tr>
                                        <tr style="border:hidden;">
                                            <th rowspan="1" colspan="4">
                                                <div style="text-align:left">&nbsp;ENG.Receiver/sect. Manager: .............................................../................................................ (Block Letter)</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left">Date: ..........................</div>
                                            </th>
                                        </tr>
                                        <tr style="border:hidden;">
                                            <th rowspan="1" colspan="4">
                                                <div style="text-align:left">&nbsp;ENG.Receiver/sect. Manager: .............................................../................................................</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left">Date: .........................</div>
                                            </th>
                                        </tr>
                                        <tr style="border:hidden;">
                                            <th rowspan="1" colspan="4">
                                                <div style="text-align:left">&nbsp;ENG.Receiver/sect. Manager: .............................................../................................................</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left">Date: .........................</div>
                                            </th>
                                        </tr>


                                        <tr style="border-top: 5px solid red;">
                                            <th rowspan="1" colspan="5" style="border-top: 1px solid black;">
                                                <div style="text-align:left; font-size:15px;"><u>Engineering Section:</u></div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="1" colspan="5">
                                                <div style="text-align:left">1. Opinnion of Engineering Section Manager / Engineering Section Chief:</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th>
                                                <div style="text-align:left">&nbsp;Type of service</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left"><img style="width:11px;height:11px;" src="https://image0.flaticon.com/icons/png/512/24/24396.png">&nbsp; 3rd Party Service</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left"><img style="width:11px;height:11px;" src="https://image0.flaticon.com/icons/png/512/24/24396.png">&nbsp; 3rd Party Service</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left"><img style="width:11px;height:11px;" src="https://image0.flaticon.com/icons/png/512/24/24396.png">&nbsp; 3rd Party Service</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left"><img style="width:11px;height:11px;" src="https://image0.flaticon.com/icons/png/512/24/24396.png">&nbsp; 3rd Party Service</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="1" colspan="5">
                                                <div style="text-align:left">&nbsp;&nbsp;&nbsp;Resson to repairing: ...............................................................................................................................</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="1" colspan="5">
                                                <div style="text-align:left">&nbsp;&nbsp;&nbsp;Invoice/PO Number: ..................................................................................................Service charge: ......................Baht</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="1" colspan="5">
                                                <div style="text-align:left">2 Operation details&nbsp;</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th>
                                                <div style="text-align:left">&nbsp;Type of service</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left"><img style="width:11px;height:11px;" src="https://image0.flaticon.com/icons/png/512/24/24396.png">&nbsp; 3rd Party Service</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left"><img style="width:11px;height:11px;" src="https://image0.flaticon.com/icons/png/512/24/24396.png">&nbsp; 3rd Party Service</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left"><img style="width:11px;height:11px;" src="https://image0.flaticon.com/icons/png/512/24/24396.png">&nbsp; 3rd Party Service</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left"><img style="width:11px;height:11px;" src="https://image0.flaticon.com/icons/png/512/24/24396.png">&nbsp; 3rd Party Service</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th>
                                                <div style="text-align:left">&nbsp;Spare Part Lisrs:</div>
                                            </th>
                                            <th rowspan="1" colspan="4">
                                                <div style="text-align:left">1. ...........................................................................Qty ..............................Price .......................Baht</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th>
                                                <div style="text-align:left">&nbsp;</div>
                                            </th>
                                            <th rowspan="1" colspan="4">
                                                <div style="text-align:left">2. ...........................................................................Qty ..............................Price .......................Baht</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th>
                                                <div style="text-align:left">&nbsp;</div>
                                            </th>
                                            <th rowspan="1" colspan="4">
                                                <div style="text-align:left">3. ...........................................................................Qty ..............................Price .......................Baht</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="1" colspan="3">
                                                <div style="text-align:left">&nbsp;</div>
                                            </th>
                                            <th rowspan="1" colspan="2">
                                                <div style="text-align:left">Totals Cost ................................... Baht</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="1" colspan="5">
                                                <div style="text-align:left">&nbsp;Start working date .......................................  Finish date ...........................................  Total service time ...................................... days</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="1" colspan="5">
                                                <div style="text-align:left">&nbsp;Detail of operation / Solution</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="1" colspan="5">
                                                <div style="text-align:left; border: 2px solid black;  border-radius: 8px; margin-left:10px; margin-right:10px;">&nbsp;&nbsp;&nbsp;Description:<br />&nbsp;<br />&nbsp;<br />&nbsp;<br />&nbsp;<br />&nbsp;<br />&nbsp;</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="1" colspan="4">
                                                <div style="text-align:left">&nbsp;Responsible person ...................................................................................................................(Block Letter)</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left">Date: .........................</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="1" colspan="4">
                                                <div style="text-align:left">&nbsp;Inspection person (Issuer) .........................................................................................................(Block Letter)</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left">Date: .........................</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="1" colspan="4">
                                                <div style="text-align:left">&nbsp;Dept./Sect. Manerger of Issuer. .......................................................................................................................</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left">Date: .........................</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="1" colspan="5">
                                                <div style="text-align:left">&nbsp;3. RecommendationPrevention (Engineering Section)</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="1" colspan="5">
                                                <div style="text-align:left; border: 2px solid black;  border-radius: 8px; margin-left:10px; margin-right:10px;">&nbsp;&nbsp;&nbsp;Description:<br />&nbsp;<br />&nbsp;<br />&nbsp;<br />&nbsp;<br />&nbsp;<br />&nbsp;</div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="1" colspan="4">
                                                <div style="text-align:left">&nbsp;Enginering Section Manager .....................................................................................................................</div>
                                            </th>
                                            <th>
                                                <div style="text-align:left">Date: .........................</div>
                                            </th>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
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
