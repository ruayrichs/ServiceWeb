<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlRegisMemberForCompany.ascx.cs" Inherits="POSWeb.crm.usercontrol.ctrlRegisMemberForCompany" %>

<script>
    var zChar = new Array(' ', '(', ')', '-', '.');
    var maxphonelength = 12;
    var phonevalue1;
    var phonevalue2;
    var cursorposition;

    function ParseForNumber1(object) {
        phonevalue1 = ParseChar(object.value, zChar);
    }
    function ParseForNumber2(object) {
        phonevalue2 = ParseChar(object.value, zChar);
    }

    function backspacerUP(object, e) {
        if (e) {
            e = e
        } else {
            e = window.event
        }
        if (e.which) {
            var keycode = e.which
        } else {
            var keycode = e.keyCode
        }

        ParseForNumber1(object)

        if (keycode >= 48) {
            ValidatePhone(object)
        }
    }

    function backspacerDOWN(object, e) {
        if (e) {
            e = e
        } else {
            e = window.event
        }
        if (e.which) {
            var keycode = e.which
        } else {
            var keycode = e.keyCode
        }
        ParseForNumber2(object)
    }

    function GetCursorPosition() {

        var t1 = phonevalue1;
        var t2 = phonevalue2;
        var bool = false
        for (i = 0; i < t1.length; i++) {
            if (t1.substring(i, 1) != t2.substring(i, 1)) {
                if (!bool) {
                    cursorposition = i
                    bool = true
                }
            }
        }
    }

    function ValidatePhone(object) {

        var p = phonevalue1

        p = p.replace(/[^\d]*/gi, "")

        if (p.length < 3) {
            object.value = p
        } else if (p.length == 3) {
            pp = p;
            //d4 = p.indexOf('')
            d4 = p.indexOf('-')
            if (d4 == -1) {
                pp = "" + pp;
            }
            if (d4 == -1) {
                pp = pp + "-";
            }
            object.value = pp;
        } else if (p.length > 3 && p.length < 7) {
            p = "" + p;
            l30 = p.length;
            p30 = p.substring(0, 3);
            p30 = p30 + "-"

            p31 = p.substring(3, l30);
            pp = p30 + p31;

            object.value = pp;

        } else if (p.length >= 7) {
            p = "" + p;
            l30 = p.length;
            p30 = p.substring(0, 3);
            p30 = p30 + "-"

            p31 = p.substring(3, l30);
            pp = p30 + p31;

            l40 = pp.length;
            p40 = pp.substring(0, 7);
            p40 = p40 + "-"

            p41 = pp.substring(7, l40);
            ppp = p40 + p41;

            object.value = ppp.substring(0, maxphonelength);
        }

        GetCursorPosition()

        if (cursorposition >= 0) {
            if (cursorposition == 0) {
                cursorposition = 2
            } else if (cursorposition <= 2) {
                cursorposition = cursorposition + 1
            } else if (cursorposition <= 5) {
                cursorposition = cursorposition + 2
            } else if (cursorposition == 6) {
                cursorposition = cursorposition + 2
            } else if (cursorposition == 7) {
                cursorposition = cursorposition + 4
                e1 = object.value.indexOf('')
                e2 = object.value.indexOf('-')
                if (e1 > -1 && e2 > -1) {
                    if (e2 - e1 == 4) {
                        cursorposition = cursorposition - 1
                    }
                }
            } else if (cursorposition < 11) {
                cursorposition = cursorposition + 3
            } else if (cursorposition == 11) {
                cursorposition = cursorposition + 1
            } else if (cursorposition >= 12) {
                cursorposition = cursorposition
            }

            var txtRange = object.createTextRange();
            txtRange.moveStart("character", cursorposition);
            txtRange.moveEnd("character", cursorposition - object.value.length);
            txtRange.select();
        }

    }

    function ParseChar(sStr, sChar) {
        if (sChar.length == null) {
            zChar = new Array(sChar);
        }
        else zChar = sChar;

        for (i = 0; i < zChar.length; i++) {
            sNewStr = "";

            var iStart = 0;
            var iEnd = sStr.indexOf(sChar[i]);

            while (iEnd != -1) {
                sNewStr += sStr.substring(iStart, iEnd);
                iStart = iEnd + 1;
                iEnd = sStr.indexOf(sChar[i], iStart);
            }
            sNewStr += sStr.substring(sStr.lastIndexOf(sChar[i]) + 1, sStr.length);

            sStr = sNewStr;
        }

        return sNewStr;
    }
</script>

<div class="row">
    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
        <a>
            <h6>ข้อมูลนิติบุคคล</h6>
        </a>
        <hr style="border: solid 1px #808080; width: 100%;" />
    </div>
    <div class="row">
        <div class="col-xs-0 col-sm-0 col-md-1 col-lg-1">
        </div>
        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-10">
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <label>ชื่อบริษัท (ภาษาไทย)</label>
                <asp:TextBox ID="txtCompanyNameThai" CssClass="form-control" placeholder="กรอกชื่อบริษัท (ไทย)" runat="server" />
            </div>
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <label>ชื่อบริษัท (ภาษาอังกฤษ)</label>
                <asp:TextBox ID="txtCompanyNameEnglish" CssClass="form-control" placeholder="กรอกชื่อบริษัท (อังกฤษ)" runat="server" />
            </div>
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <label>เลขประจำตัวผู้เสียภาษี</label>
                <asp:TextBox ID="txtTaxNumber"
                     CssClass="form-control" runat="server" />
                
            </div>
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <label>เว็บไซด์</label>
                <asp:TextBox ID="txtWebsite" CssClass="form-control" placeholder="www.focusonesoftware.com" runat="server" />
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                        <label>เบอร์โทรติดต่อ 1</label>
                        <asp:TextBox ID="txtPhon1" onkeydown="javascript:backspacerDOWN(this,event);" onkeyup="javascript:backspacerUP(this,event);"
                             CssClass="form-control" placeholder="999-999-9999" runat="server" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                        <label>เบอร์โทรติดต่อ 2</label>
                        <asp:TextBox ID="txtPhon2" onkeydown="javascript:backspacerDOWN(this,event);" onkeyup="javascript:backspacerUP(this,event);"
                            placeholder="999-999-9999" CssClass="form-control" runat="server" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                        <label>โทรสาร</label>
                        <asp:TextBox ID="txtFax" onkeydown="javascript:backspacerDOWNFax(this,event);" onkeyup="javascript:backspacerUPFax(this,event);"
                            placeholder="074-000-000" CssClass="form-control" runat="server" />
                        <script>

                            var zChar = new Array(' ', '(', ')', '-', '.');
                            var maxphonelengthFax = 11;
                            var phonevalue1Fax;
                            var phonevalue2Fax;
                            var cursorpositionFax;

                            function ParseForNumber1Fax(object) {
                                phonevalue1Fax = ParseChar(object.value, zChar);
                            }
                            function ParseForNumber2Fax(object) {
                                phonevalue2Fax = ParseChar(object.value, zChar);
                            }

                            function backspacerUPFax(object, e) {
                                if (e) {
                                    e = e
                                } else {
                                    e = window.event
                                }
                                if (e.which) {
                                    var keycode = e.which
                                } else {
                                    var keycode = e.keyCode
                                }

                                ParseForNumber1Fax(object)

                                if (keycode >= 48) {
                                    ValidatePhoneFax(object)
                                }
                            }

                            function backspacerDOWNFax(object, e) {
                                if (e) {
                                    e = e
                                } else {
                                    e = window.event
                                }
                                if (e.which) {
                                    var keycode = e.which
                                } else {
                                    var keycode = e.keyCode
                                }
                                ParseForNumber2Fax(object)
                            }

                            function GetCursorPositionFax() {

                                var t1 = phonevalue1Fax;
                                var t2 = phonevalue2Fax;
                                var bool = false
                                for (i = 0; i < t1.length; i++) {
                                    if (t1.substring(i, 1) != t2.substring(i, 1)) {
                                        if (!bool) {
                                            cursorpositionFax = i
                                            bool = true
                                        }
                                    }
                                }
                            }

                            function ValidatePhoneFax(object) {

                                var p = phonevalue1Fax

                                p = p.replace(/[^\d]*/gi, "")

                                if (p.length < 3) {
                                    object.value = p
                                } else if (p.length == 3) {
                                    pp = p;
                                    //d4 = p.indexOf('')
                                    d4 = p.indexOf('-')
                                    if (d4 == -1) {
                                        pp = "" + pp;
                                    }
                                    if (d4 == -1) {
                                        pp = pp + "-";
                                    }
                                    object.value = pp;
                                } else if (p.length > 3 && p.length < 7) {
                                    p = "" + p;
                                    l30 = p.length;
                                    p30 = p.substring(0, 3);
                                    p30 = p30 + "-"

                                    p31 = p.substring(3, l30);
                                    pp = p30 + p31;

                                    object.value = pp;

                                } else if (p.length >= 7) {
                                    p = "" + p;
                                    l30 = p.length;
                                    p30 = p.substring(0, 3);
                                    p30 = p30 + "-"

                                    p31 = p.substring(3, l30);
                                    pp = p30 + p31;

                                    l40 = pp.length;
                                    p40 = pp.substring(0, 7);
                                    p40 = p40 + "-"

                                    p41 = pp.substring(7, l40);
                                    ppp = p40 + p41;

                                    object.value = ppp.substring(0, maxphonelengthFax);
                                }

                                GetCursorPositionFax()

                                if (cursorpositionFax >= 0) {
                                    if (cursorpositionFax == 0) {
                                        cursorpositionFax = 2
                                    } else if (cursorpositionFax <= 2) {
                                        cursorpositionFax = cursorpositionFax + 1
                                    } else if (cursorpositionFax <= 5) {
                                        cursorpositionFax = cursorpositionFax + 2
                                    } else if (cursorpositionFax == 6) {
                                        cursorpositionFax = cursorpositionFax + 2
                                    } else if (cursorpositionFax == 7) {
                                        cursorpositionFax = cursorpositionFax + 3
                                        e1 = object.value.indexOf('')
                                        e2 = object.value.indexOf('-')
                                        if (e1 > -1 && e2 > -1) {
                                            if (e2 - e1 == 3) {
                                                cursorpositionFax = cursorpositionFax - 1
                                            }
                                        }
                                    } else if (cursorpositionFax < 10) {
                                        cursorpositionFax = cursorpositionFax + 3
                                    } else if (cursorpositionFax == 10) {
                                        cursorpositionFax = cursorpositionFax + 1
                                    } else if (cursorpositionFax >= 11) {
                                        cursorpositionFax = cursorpositionFax
                                    }

                                    var txtRange = object.createTextRange();
                                    txtRange.moveStart("character", cursorpositionFax);
                                    txtRange.moveEnd("character", cursorpositionFax - object.value.length);
                                    txtRange.select();
                                }

                            }

                            function ParseChar(sStr, sChar) {
                                if (sChar.length == null) {
                                    zChar = new Array(sChar);
                                }
                                else zChar = sChar;

                                for (i = 0; i < zChar.length; i++) {
                                    sNewStr = "";

                                    var iStart = 0;
                                    var iEnd = sStr.indexOf(sChar[i]);

                                    while (iEnd != -1) {
                                        sNewStr += sStr.substring(iStart, iEnd);
                                        iStart = iEnd + 1;
                                        iEnd = sStr.indexOf(sChar[i], iStart);
                                    }
                                    sNewStr += sStr.substring(sStr.lastIndexOf(sChar[i]) + 1, sStr.length);

                                    sStr = sNewStr;
                                }

                                return sNewStr;
                            }


                        </script>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                <label>อีเมล์</label>
                <asp:TextBox ID="txtEmail" CssClass="form-control" placeholder="xxx@hostmail.com" runat="server" />
            </div>
        </div>
    </div>
</div>


<div class="row">
    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
        <a>
            <h6>เพิ่มเติม</h6>
        </a>
        <hr style="border: solid 1px #808080; width: 100%;" />
    </div>
    <div class="row">
        <div class="col-xs-0 col-sm-0 col-md-1 col-lg-1">
        </div>
        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-10">
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <label>หมายเหตุ 1</label>
                <asp:TextBox ID="txtRemark1" runat="server" TextMode="MultiLine" Rows="2" placeholder="รายละเอียดเพิ่มเติม" CssClass="form-control" />
            </div>
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <label>หมายเหตุ 2</label>
                <asp:TextBox ID="txtRemark2" runat="server" TextMode="MultiLine" Rows="2" placeholder="รายละเอียดเพิ่มเติม" CssClass="form-control" />
            </div>
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <label>หมายเหตุ 3</label>
                <asp:TextBox ID="txtRemark3" runat="server" TextMode="MultiLine" Rows="2" placeholder="รายละเอียดเพิ่มเติม" CssClass="form-control" />
            </div>
        </div>
    </div>
</div>
