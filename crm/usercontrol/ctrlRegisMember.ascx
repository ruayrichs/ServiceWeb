<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlRegisMember.ascx.cs" Inherits="POSWeb.crm.usercontrol.ctrlRegisMember" %>


<style>
    .datepicker {
        z-index: 10000 !important;
    }
</style>

<script>
    function openListMember(obj) {
        $('#listMemberName').fadeIn();
        $("#btnclickSearch").click();
    }

    function closeListMember(obj) {
        $('#listMemberName').fadeOut();
    }

    function openListSerName(obj) {
        $('#listMemberSerName').fadeIn();
        $("#btnClickSearchSername").click();
    }

    function closeListSerName(obj) {
        $('#listMemberSerName').fadeOut();
    }

    function openListMobile(obj) {
        $('#listMemberMobile').fadeIn();
        $("#btnSearchNumberMobile").click();
    }

    function closeListMobile(obj) {
        $('#listMemberMobile').fadeOut();
    }

    function openListTell(obj) {
        $('#listMemberTell').fadeIn();
        $("#btnSearchNumberTell").click();
    }

    function closeListTell(obj) {
        $('#listMemberTell').fadeOut();
    }

    function openListEmail(obj) {
        $('#listMemberEmail').fadeIn();
        $("#btnSearchEmail").click();
    }

    function closeListEmail(obj) {
        $('#listMemberEmail').fadeOut();
    }


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
            <h6>ข้อมูลลูกค้าทั่วไป</h6>
        </a>
        <hr style="border: solid 1px #808080; width: 100%;" />
    </div>
    <div class="row">
        <div class="col-xs-0 col-sm-0 col-md-1 col-lg-1">
        </div>
        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-10">
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <label>เลขที่บัตรสมาชิก</label><span class="text-danger">&nbsp;*</span>
                <asp:TextBox ID="txtIdMember" CssClass="form-control" runat="server" placeholder="กรอกเลขที่บัตรสมาชิก" />
            </div>
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <label>ประเภทสมาชิก</label><span class="text-danger">&nbsp;*</span>
                <asp:DropDownList ID="ddlMemberType" CssClass="form-control require" runat="server" Style="cursor: pointer;">
                    <asp:ListItem Value="N" Text="Normal"></asp:ListItem>
                    <asp:ListItem Value="P" Text="Premium"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <label>สาขาที่สมัคร</label><span class="text-danger">&nbsp;*</span>
                <asp:DropDownList ID="ddlBranch" CssClass="form-control" runat="server" Style="cursor: pointer;">
                </asp:DropDownList>
            </div>
            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                <label>วันที่สมัคร</label><span class="text-danger">&nbsp;*</span>
                <div class="input-group">
                    <asp:TextBox runat="server" ID="txtStartDate" placeHolder="วันที่สมัคร(วัน/เดือน/ปี)" ClientIDMode="Static" CssClass="form-control require date-picker" />
                    <span class="input-group-addon hand" onclick="$('#txtBirthdatemember').focus();"><i class="fa fa-calendar"></i></span>
                    <%--<span class="input-group-addon hand" onclick="$('#txtBirthdatemember').val('');"><i class="fa fa-remove"></i></span>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                        <label>ระยะเวลาการเป็นสมาชิก</label><span class="text-danger">&nbsp;*</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtMembership" CssClass="form-control text-right" runat="server" placeholder="กรอกระยะเวลาการเป็นสมาชิก" />
                            <span class="input-group-btn">
                                <asp:DropDownList ID="ddlUnitsTime" CssClass="form-control" runat="server" Style="width: 76px; border-top-right-radius: 5px; border-bottom-right-radius: 5px; cursor: pointer;">
                                    <asp:ListItem Text="ปี" Value="Year" />
                                </asp:DropDownList>
                            </span>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>

</div>
<div class="row">
    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
        <a>
            <h6>ข้อมูลส่วนตัว</h6>
        </a>
        <hr style="border: solid 1px #808080; width: 100%;" />
    </div>
    <div class="row">
        <div class="col-xs-0 col-sm-0 col-md-1 col-lg-1">
        </div>
        <div class="col-xs-12 col-sm-12 col-md-10 col-lg-10">
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <label>คำนำหน้าชื่อ</label><span class="text-danger">&nbsp;*</span>
                <asp:DropDownList ID="ddlTitle" CssClass="form-control require" runat="server" Style="cursor: pointer;">
                </asp:DropDownList>
            </div>
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">

                <label>ชื่อสมาชิก</label><span class="text-danger">&nbsp;*</span>
                <div class="input-group">
                    <asp:TextBox ID="txtFirstName" CssClass="form-control require" runat="server" placeholder="กรอกชื่อสมาชิก" />
                    <span class="input-group-addon" id="basic-addon2" onclick="openListMember(this);" style="cursor: pointer;">ตรวจสอบชื่อ</span>
                </div>
                <asp:UpdatePanel ID="udpBtnSearch" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnclickSearch" Style="display: none;" ClientIDMode="Static" OnClick="btnclickSearch_Click" Text="text" runat="server" />
                        <asp:Button ID="btnClickSearchSername" Style="display:none;" ClientIDMode="Static" OnClick="btnClickSearchSername_Click" Text="text" runat="server" />
                        
                        <!-- Search Mobile -->
                        <asp:Button ID="btnSearchNumberMobile" style="display:none;" ClientIDMode="Static" OnClick="btnSearchNumberMobile_Click" Text="text" runat="server" />

                        <!-- search Tell1 -->
                        <asp:Button ID="btnSearchNumberTell" style="display:none;" ClientIDMode="Static" OnClick="btnSearchNumberTell_Click" Text="text" runat="server" />
                        <!-- search Email -->
                        <asp:Button ID="btnSearchEmail" style="display:none;" ClientIDMode="Static" OnClick="btnSearchEmail_Click" Text="text" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div id="listMemberName" style="display: none; margin-top: 10px; margin-bottom: 10px; background-color: white; border: solid 1px green; -webkit-border-radius: 10px; -moz-border-radius: 10px; border-radius: 10px;">

                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                            <div class="text-right">
                                <span class="remove-icon" onclick="closeListMember(this);"></span>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <asp:UpdatePanel ID="udpListMember" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                
                                <div style="margin-left:15px;margin-bottom:15px;margin-right:15px;margin-top:-5px;">
                                    <div id="checkDataName" runat="server"></div>
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <asp:Repeater ID="rptlistMember" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-striped table-hover table-bordered">
                                            <tr>
                                                <th style="width:5%">ลำดับ</th>
                                                <th>ชื่อ-สกุล</th>
                                                <th>เบอร์โทรศัพท์มือถีอ</th>
                                                <th>บอร์โทรศัพท์</th>
                                                <th>E-Mail</th>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Container.ItemIndex+1 %></td>
                                            <td><%# Eval("customer") %></td>
                                            <td><%# Eval("Mobile") %></td>
                                            <td><%# Eval("TelNo1") %></td>
                                            <td ><%# convertCommaToSpace(Eval("EMail")) %></td>
                                        </tr>
                                       
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                        </div>
                                    </div>
                                   
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <label>นามสกุล</label><span class="text-danger">&nbsp;*</span>
                <div class="input-group">
                    <asp:TextBox ID="txtLastName" CssClass="form-control require" runat="server" placeholder="กรอกนามสกุล" />
                    <span class="input-group-addon" onclick="openListSerName(this);" style="cursor: pointer;">ตรวจสอบนามสกุล</span>
                </div>
                <div id="listMemberSerName" style="display: none; margin-top: 10px; margin-bottom: 10px; background-color: white; border: solid 1px green; -webkit-border-radius: 10px; -moz-border-radius: 10px; border-radius: 10px;">

                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                            <div class="text-right">
                                <span class="remove-icon" onclick="closeListSerName(this);"></span>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <asp:UpdatePanel ID="UdpListSerName" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <div  style="margin-left:15px;margin-bottom:15px;margin-right:15px;margin-top:-5px;">
                                  <div id="divLisSerName" runat="server"></div>
                                    <asp:Repeater ID="RptListSerName" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-striped table-hover table-bordered table-lg">
                                            <tr>
                                                <th>ลำดับ</th>
                                                <th>ชื่อ-สกุล</th>
                                                <th>เบอร์โทรศัพท์มือถีอ</th>
                                                <th>บอร์โทรศัพท์</th>
                                                <th>E-Mail</th>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Container.ItemIndex+1 %></td>
                                            <td><%# Eval("customer") %></td>
                                            <td><%# Eval("Mobile") %></td>
                                            <td><%# Eval("TelNo1") %></td>
                                            <td><%# convertCommaToSpace(Eval("EMail")) %></td>
                                        </tr>
                                       
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </div>
            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                <label>วันเกิด</label>
                <div class="input-group">
                    <asp:TextBox runat="server" ID="txtBirthDate" placeHolder="วันเกิด(วัน/เดือน/ปี)" ClientIDMode="Static" CssClass="form-control date-picker" />
                    <span class="input-group-addon hand" onclick="$('#txtBirthDate').focus();"><i class="fa fa-calendar"></i></span>
                    <%--<span class="input-group-addon hand" onclick="$('#txtBirthDate').val('');"><i class="fa fa-remove"></i></span>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <label>หมายเลขเบอร์โทรศัพท์มือถือ</label><span class="text-danger">&nbsp;*</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtPhone" onkeydown="javascript:backspacerDOWN(this,event);" onkeyup="javascript:backspacerUP(this,event);" ClientIDMode="Static" CssClass="form-control require number" runat="server" placeholder="กรอกหมายเลขเบอร์โทรศัพท์มือถือ" />
                            <span class="input-group-addon"onclick="openListMobile(this);" style="cursor: pointer;">ตรวจสอบเบอร์มือถือ</span>
                        </div>

                        <div id="listMemberMobile" style="display: none; margin-top: 10px; margin-bottom: 10px; background-color: white; border: solid 1px green; -webkit-border-radius: 10px; -moz-border-radius: 10px; border-radius: 10px;">

                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="text-right">
                                        <span class="remove-icon" onclick="closeListMobile(this);"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <asp:UpdatePanel ID="udpListMobile" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <div  style="margin-left: 15px; margin-bottom: 15px; margin-right: 15px; margin-top: -5px;">
                                            <div id="divListMobile" runat="server"></div>
                                            <asp:Repeater ID="rptListMobile" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-striped table-hover table-bordered table-lg">
                                                        <tr>
                                                            <th>ลำดับ</th>
                                                            <th>ชื่อ-สกุล</th>
                                                            <th>เบอร์โทรศัพท์มือถีอ</th>
                                                            <th>บอร์โทรศัพท์</th>
                                                            <th>E-Mail</th>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%# Container.ItemIndex+1 %></td>
                                                        <td><%# Eval("customer") %></td>
                                                        <td><%# Eval("Mobile") %></td>
                                                        <td><%# Eval("TelNo1") %></td>
                                                        <td><%# convertCommaToSpace(Eval("EMail")) %></td>
                                                    </tr>

                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <label>หมายเลขเบอร์โทรศัพท์</label>
                        <div class="input-group">
                            <asp:TextBox ID="txtCellPhone" onkeydown="javascript:backspacerDOWN(this,event);" onkeyup="javascript:backspacerUP(this,event);" CssClass="form-control number" runat="server" placeholder="กรอกหมายเลขเบอร์โทรศัพท์" />
                            <span class="input-group-addon" onclick="openListTell(this);" style="cursor: pointer;">ตรวจสอบเบอร์โทรศัพท์</span>
                        </div>

                          <div id="listMemberTell" style="display: none; margin-top: 10px; margin-bottom: 10px; background-color: white; border: solid 1px green; -webkit-border-radius: 10px; -moz-border-radius: 10px; border-radius: 10px;">

                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="text-right">
                                        <span class="remove-icon" onclick="closeListTell(this);"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <asp:UpdatePanel ID="udpListTell" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <div style="margin-left: 15px; margin-bottom: 15px; margin-right: 15px; margin-top: -5px;">
                                            <div id="divListTell" runat="server" ></div>
                                            <asp:Repeater ID="rptListTell" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-striped table-hover table-bordered">
                                                        <tr>
                                                            <th>ลำดับ</th>
                                                            <th>ชื่อ-สกุล</th>
                                                            <th>เบอร์โทรศัพท์มือถีอ</th>
                                                            <th>บอร์โทรศัพท์</th>
                                                            <th>E-Mail</th>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%# Container.ItemIndex+1 %></td>
                                                        <td><%# Eval("customer") %></td>
                                                        <td><%# Eval("Mobile") %></td>
                                                        <td><%# Eval("TelNo1") %></td>
                                                        <td><%# convertCommaToSpace(Eval("EMail")) %></td>
                                                    </tr>

                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <label>E-mail</label>
                <div class="input-group">
                    <asp:TextBox ID="txtEmail" CssClass="form-control email" runat="server" placeholder="Example@mail.com" />
                    <span class="input-group-addon" onclick="openListEmail(this);" style="cursor: pointer;">ตรวจสอบอีเมล์</span>
                </div>


                   <div id="listMemberEmail" style="display: none; margin-top: 10px; margin-bottom: 10px; background-color: white; border: solid 1px green; -webkit-border-radius: 10px; -moz-border-radius: 10px; border-radius: 10px;">

                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="text-right">
                                        <span class="remove-icon" onclick="closeListEmail(this);"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <asp:UpdatePanel ID="udpListEmail" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <div  style="margin-left: 15px; margin-bottom: 15px; margin-right: 15px; margin-top: -5px;">
                                            <div id="divListEmail" runat="server"></div>
                                            <asp:Repeater ID="rptListEmail" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-striped table-hover table-bordered">
                                                        <tr>
                                                            <th>ลำดับ</th>
                                                            <th>ชื่อ-สกุล</th>
                                                            <th>เบอร์โทรศัพท์มือถีอ</th>
                                                            <th>บอร์โทรศัพท์</th>
                                                            <th style="width:50px;">E-Mail</th>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%# Container.ItemIndex+1 %></td>
                                                        <td><%# Eval("customer") %></td>
                                                        <td><%# Eval("Mobile") %></td>
                                                        <td><%# Eval("TelNo1") %></td>
                                                        <td><%# convertCommaToSpace(Eval("EMail")) %></td>
                                                    </tr>

                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>


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
            <label>รายละเอียดเพิ่มเติม</label>
            <asp:TextBox TextMode="MultiLine" Rows="3" ID="txtcomment" CssClass="form-control dis" runat="server" placeholder="กรอกรายละเอียดเพิ่มเติม" />
        </div>
    </div>
</div>
