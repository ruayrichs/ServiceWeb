<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="DocumentReceive.aspx.cs" Inherits="ServiceWeb.Transaction.DocumentReceive" %>

<%@ Register Src="~/UserControl/AutoComplete/General/AutoCompleteGeneral.ascx" TagPrefix="uc1" TagName="AutoCompleteGeneral" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEmployee.ascx" TagPrefix="uc1" TagName="AutoCompleteEmployee" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="/UserControl/AutoComplete/General/autocomplete-ganeral.js"></script>
    <style>
        .hide {
            display: none;
        }
        #table-Material > tbody > tr.info > td {
            background-color : rgba(39, 242, 255, 0.48)!important;
        }
    </style>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <a class="btn btn-warning mb-1" href="/Transaction/DocumentReceiveCriteria.aspx"><i class="fa fa-arrow-circle-left"></i>&nbsp;&nbsp;ย้อนกลับ</a>
                   <% if (isCreate || isDocNotRelease)
                      { %>
                     <button type="button" class="btn btn-success mb-1" onclick="$(this).next().click();">
                        <i class="fa fa-save"></i>&nbsp;&nbsp;บันทึก
                    </button>
                    <asp:Button runat="server" ID="btnSaveDoc" CssClass="hide" OnClick="btnSaveDoc_Click" OnClientClick="return AGDataConfirm(this,'ยืนยันการบันทึก');" />
                    <% } %>
                    <% if (isDocNotRelease)
                      { %>
                    <button type="button" class="btn btn-info mb-1 btn-mode-change" onclick="$(this).next().click();">
                        <i class="fa fa-check-circle"></i>&nbsp;&nbsp;ยืนยันรายการ
                    </button>
                    <asp:Button ID="btnReleaseDocument" CssClass="hide" OnClick="btnReleaseDocument_Click" OnClientClick="return isConfirmReleaseDocument(this);" Text="" runat="server" />
                    <% } %>
                    <% if (isDocNotRelease || isDocRelease)
                      { %>
                    <button type="button" class="btn btn-danger mb-1 btn-mode-change" onclick="$(this).next().click();" >
                        <i class="fa fa-ban"></i>&nbsp;&nbsp;ยกเลิกเอกสาร
                    </button>
                    <asp:Button ID="btnCancel" CssClass="hide" OnClick="btnCancel_Click" OnClientClick="return isConfirmCancelDocument(this);" Text="" runat="server" />
                    <% } %>
                   <%-- <button type="button" class="btn btn-secondary mb-1 btn-mode-change" onclick="$(this).next().click();">
                        <i class="fa fa-print"></i>&nbsp;&nbsp;พิมพ์เอกสาร
                    </button>--%>
                     <% if (isDocRelease && isHasAsset)
                        { %>
                     <button type="button" class="btn btn-success mb-1 btn-mode-change" onclick="$(this).next().click();">
                        <i class="fa fa-save"></i>&nbsp;&nbsp;สร้างสินทรัพย์
                    </button>
                    <asp:Button runat="server" ID="btnOpenModalCreateAsset" CssClass="hide" OnClick="btnOpenModalCreateAsset_Click"  OnClientClick="AGLoading(true);" />
                     <button type="button" class="btn btn-success mb-1 btn-mode-change" onclick="$(this).next().click();">
                        <i class="fa fa-save"></i>&nbsp;&nbsp;สร้าง CI
                     </button>
                     <asp:Button runat="server" ID="btnOpenModalCreateCI" CssClass="hide" OnClick="btnOpenModalCreateCI_Click" OnClientClick="AGLoading(true);" />
                    <% } %>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>

    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">ใบตรวจรับงาน</h5>
                </div>
                <div class="card-body">
                    <nav>
                        <div class="nav nav-tabs" id="nav-tab" role="tablist">
                            <a class="nav-item nav-link active" id="nav-header-tab" data-toggle="tab" href="#nav-header" role="tab" aria-controls="nav-header" aria-selected="true">Header
                            </a>
                            <a class="nav-item nav-link" id="nav-general-tab" data-toggle="tab" href="#nav-general" role="tab" aria-controls="nav-general" aria-selected="false">General
                            </a>
                            <a class="nav-item nav-link" id="nav-tax-tab" data-toggle="tab" href="#nav-tax" role="tab" aria-controls="nav-tax" aria-selected="false">Tax
                            </a>
                            <a class="nav-item nav-link" id="nav-prop-tab" data-toggle="tab" href="#nav-prop" role="tab" aria-controls="nav-prop" aria-selected="false">Properties
                            </a>
                            <a class="nav-item nav-link d-none" id="nav-attach-tab" data-toggle="tab" href="#nav-attach" role="tab" aria-controls="nav-attach" aria-selected="false">Attachment
                            </a>
                            <a class="nav-item nav-link d-none" id="nav-wf-tab" data-toggle="tab" href="#nav-wf" role="tab" aria-controls="nav-wf" aria-selected="false">Work Flow
                            </a>
                        </div>
                    </nav>
                    <asp:UpdatePanel ID="updnTabContent" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdfDocStatus" runat="server" />
                            <div class="tab-content m-3" id="nav-tabContent">
                                <div class="tab-pane fade show active" id="nav-header" role="tabpanel" aria-labelledby="nav-header-tab">
                                    <div class="form-row">
                                        <div class="form-group col-sm-6">
                                            <label>ประเภทเอกสาร</label>
                                            <asp:DropDownList ID="ddlDocumentType" runat="server" CssClass="form-control form-control-sm required">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group col-sm-6">
                                            <label>เลขที่เอกสาร</label>
                                            <asp:TextBox ID="tbDocumentNo" Enabled="false" placeholder="Text" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                            <asp:HiddenField ID="hdfRefPODocNo" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hdfrefPODocType" ClientIDMode="Static" runat="server" />
                                            <asp:HiddenField ID="hdfRefFiscalYear" ClientIDMode="Static" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-sm-6">
                                            <label>สถานะเอกสาร</label>
                                            <asp:DropDownList ID="ddlDocumentStatus" Enabled="false" runat="server" CssClass="form-control form-control-sm statuslock">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group col-sm-3 hide">
                                            <label>สถานะการอนุมัติ</label>
                                            <asp:DropDownList ID="ddlReleaseStatus" runat="server" CssClass="form-control form-control-sm statuslock">
                                                <asp:ListItem Text="N : Not Start" Value="N" />
                                                <asp:ListItem Text="S : Start" Value="S" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>เหตุผลการตรวจรับ</label>
                                                <asp:TextBox ID="tbRationale" runat="server" CssClass="form-control form-control-sm " Text=''></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane fade" id="nav-general" role="tabpanel" aria-labelledby="nav-general-tab">
                                    <div class="form-row">
                                        <div class="col-lg-6 col-md-12">
                                            <div class="form-row">
                                                <div class="form-group col-lg-12 col-md-6">
                                                    <label>Supplier</label>
                                                    <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control form-control-sm">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="form-group col-lg-6 col-md-3 col-sm-6">
                                                    <label>วันที่ส่งมอบ</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="tbReceiveDate" placeholder="dd/mm/yyyy" runat="server" CssClass="form-control form-control-sm date-picker" Text=""></asp:TextBox>
                                                        <div class="input-group-append">
                                                            <span class="input-group-text fa fa-calendar"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group col-lg-6 col-md-3 col-sm-6">
                                                    <label>วันที่เอกสาร</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="tbDocumentDate" placeholder="dd/mm/yyyy" runat="server" CssClass="form-control form-control-sm date-picker" Text=""></asp:TextBox>
                                                        <div class="input-group-append">
                                                            <span class="input-group-text fa fa-calendar"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="form-group col-sm-6">
                                                    <label>เอกสารอ้างอิง</label>
                                                    <asp:TextBox ID="tbReferenceDocument"  runat="server" CssClass="form-control form-control-sm statuslock" Text=""></asp:TextBox>
                                                </div>
                                                <div class="form-group col-sm-6">
                                                    <label>จำนวนเงิน</label>
                                                    <asp:TextBox ID="tbAmount" Enabled="false" placeholder="Number" runat="server" CssClass="form-control form-control-sm text-right statuslock" Text=""></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-12">
                                            <div class="form-row">
                                                <div class="form-group col-lg-12 col-md-6">
                                                    <label>สร้างเอกสารโดย</label>
                                                    <asp:TextBox ID="tbCreatedBy" runat="server" CssClass="form-control form-control-sm statuslock" Enabled="false" Text=""></asp:TextBox>
                                                    <asp:HiddenField ID="hdfCreatedBy" runat="server" Value="C1000416" />
                                                </div>
                                                <div class="form-group col-lg-12 col-md-3 col-sm-6">
                                                    <label>วันที่สร้างเอกสาร</label>
                                                    <asp:TextBox ID="tbCreatedOnDate" runat="server" CssClass="form-control form-control-sm statuslock" Enabled="false" Text=""></asp:TextBox>
                                                </div>
                                                <div class="form-group col-lg-12 col-md-3 col-sm-6">
                                                    <label>เวลาที่สร้างเอกสาร</label>
                                                    <asp:TextBox ID="tbCreatedOnTime" runat="server" CssClass="form-control form-control-sm statuslock" Enabled="false" Text=""></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-12">
                                            <div class="form-row">
                                                <div class="form-group col-lg-12 col-md-6">
                                                    <label>แก้ไขเอกสารโดย</label>
                                                    <asp:TextBox ID="tbUpdatedBy" runat="server" CssClass="form-control form-control-sm statuslock" Enabled="false" Text=""></asp:TextBox>
                                                    <asp:HiddenField ID="hdfUpdatedBy" runat="server" Value="C1000728" />
                                                </div>
                                                <div class="form-group col-lg-12 col-md-3 col-sm-6">
                                                    <label>วันที่แก้ไขเอกสาร</label>
                                                    <asp:TextBox ID="tbUpdatedOnDate" runat="server" CssClass="form-control form-control-sm statuslock" Enabled="false" Text=""></asp:TextBox>
                                                </div>
                                                <div class="form-group col-lg-12 col-md-3 col-sm-6">
                                                    <label>เวลาที่แก้ไขเอกสาร</label>
                                                    <asp:TextBox ID="tbUpdatedOnTime" runat="server" CssClass="form-control form-control-sm statuslock" Enabled="false" Text=""></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label>คำอธิบายเพิ่มเติม</label>
                                        <asp:TextBox ID="tbRemark" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Rows="3" Style="resize: none;"
                                            Text="">
                                        </asp:TextBox>
                                    </div>
                                </div>
                                <div class="tab-pane fade" id="nav-tax" role="tabpanel" aria-labelledby="nav-tax-tab">
                                    <div class="card">
                                        <div class="card-body" style="padding-top: 0.75rem;">
                                            <div class="row">
                                                <div class="col">
                                                    <div class="table-responsive">
                                                        <table id="table-items2" class="table table-sm table-striped table-bordered nowrap dataTable">
                                                            <thead>
                                                                <tr>
                                                                    <th>รอเอกสาร</th>
                                                                    <th>เลขที่ภาษี</th>
                                                                    <th>รหัสภาษี</th>
                                                                    <th>ฐานภาษี</th>
                                                                    <th>ภาษี</th>
                                                                    <th>จำนวนเงินรวมภาษี</th>
                                                                    <th>สกุลเงิน</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:UpdatePanel ID="udpTax" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:Repeater ID="rptTax" runat="server">
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <td class="text-nowrap text-center align-middle">
                                                                                        <asp:CheckBox ID="chkWaitDocument" runat="server" Checked='<%# (Eval("Status").ToString()).Equals("True") ? true : false  %>' />
                                                                                    </td>
                                                                                    <td class="text-nowrap">
                                                                                        <asp:TextBox ID="tbTaxNumber" placeholder="Text" runat="server" CssClass="form-control form-control-sm" Width="130" Text='<%# Eval("TaxNo")  %>'></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="text-nowrap">
                                                                                        <asp:DropDownList ID="ddlTaxCode" runat="server" CssClass="form-control form-control-sm" Width="180">
                                                                                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                                                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                                                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td class="text-right align-middle">
                                                                                        <%--130,047.50--%>
                                                                                        <asp:TextBox ID="txtTaxBase" placeholder="Number" runat="server" CssClass="form-control form-control-sm text-right" Width="130" onkeypress="ISFloat(this,event);" Text='<%# ConvertTypeDecimal(Eval("TaxBase")) %>' />
                                                                                        <%--Text="130,047.50"--%>
                                                                                    </td>
                                                                                    <td class="text-right align-middle"><%--9,103.32--%>
                                                                                        <asp:TextBox ID="txtTaxValue" placeholder="Number" runat="server" CssClass="form-control form-control-sm text-right" Width="130" onkeypress="ISFloat(this,event);" Text='<%# ConvertTypeDecimal(Eval("TaxValue")) %>' /><%--Text="9,103.32"--%>
                                                                                    </td>
                                                                                    <td class="text-right align-middle"><%--139,150.82--%>
                                                                                        <asp:TextBox ID="txtTaxTotal" placeholder="Number" runat="server" CssClass="form-control form-control-sm text-right" Width="130" onkeypress="ISFloat(this,event);" Text='<%# ConvertTypeDecimal(Eval("TaxTotal")) %>' />
                                                                                        <%--Text="139,150.82"--%>
                                                                                    </td>
                                                                                    <td class="align-middle">
                                                                                        <asp:TextBox ID="txtTaxCurrency" placeholder="Text" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("Currency") %>' Width="130" />
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </tbody>
                                                        </table>
                                                        <button type="button" class="btn btn-success mb-1" onclick="$(this).next().click();">
                                                            <i class="fa fa-plus"></i>&nbsp;&nbsp;เพิ่ม
                                                        </button>
                                                        <asp:Button runat="server" ID="Button1" CssClass="hide" OnClick="btnAddTax_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="tab-pane fade" id="nav-prop" role="tabpanel" aria-labelledby="nav-prop-tab">
                                    <div class="table-responsive">
                                        <table class="table table-sm table-striped table-bordered nowrap dataTable">
                                            <thead>
                                                <tr>
                                                    <th class="text-nowrap">รหัส</th>
                                                    <th class="text-nowrap">รายละเอียด</th>
                                                    <th>ระบุข้อมูล</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:UpdatePanel ID="udpProperties" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Repeater ID="rptProperties" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="text-nowrap align-middle">
                                                                        <asp:Label ID="lbProCode" runat="server" Text='<%# Eval("CODE") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdfProCode" Value='<%# Eval("CODE") %>' />

                                                                    </td>
                                                                    <td class="text-nowrap align-middle">
                                                                        <asp:Label ID="lbProName" runat="server" Text='<%# Eval("CODEDes") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdfProName" Value='<%# Eval("CODEDes") %>' />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtProDes" placeholder="Text" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("Description") %>' />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="tab-pane fade d-none" id="nav-attach" role="tabpanel" aria-labelledby="nav-attach-tab">
                                    <div style="display: none;" role="status" aria-hidden="true">
                                        <div class="modal-backdrop-udpn fade in">
                                        </div>
                                        <img class="img-loading-pos" src="/images/loading2.gif" style="border-width: 0px;">
                                    </div>
                                    <div class="container-fluid">
                                        <div class="row" style="text-align: left;">
                                            <label class="col-lg-2 col-md-2 col-sm-2">
                                                <h4>เลือกไฟล์</h4>
                                            </label>
                                            <div class="col-lg-10 col-md-10 col-sm-10">
                                                <asp:FileUpload runat="server" type="file" ID="file_attach" Style="background-color: LightYellow;" />
                                                <span class="help-block">กรุณาเลือกไฟล์ที่ต้องการแนบ โดยกดปุ่ม Choose File จากนั้นกดปุ่ม เพิ่มเอกสาร ด้านล่าง
                                                </span>
                                            </div>
                                        </div>
                                        <div class="row" style="padding-top: 10px; padding-bottom: 10px; padding-left: 10px;">
                                            <button type="submit" id="btn_add" class="btn btn-primary" onclick="btnAddFile();">
                                                <span class="fa fa-paperclip" aria-hidden="true"></span>เพิ่มเอกสาร  
                                            </button>

                                        </div>

                                    </div>
                                </div>
                                <div class="tab-pane fade d-none" id="nav-wf" role="tabpanel" aria-labelledby="nav-wf-tab">
                                    <div class="row">
                                        <label class="col-lg-2 col-md-2 col-sm-2" style="padding-bottom: 10px;">รหัสการอนุมัติ</label>
                                        <div class="col-lg-4 col-md-4 col-sm-4" style="padding-bottom: 10px;">
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-lg-2 col-md-2 col-sm-2" style="padding-bottom: 10px;">รหัสการอนุมัติ</label>
                                        <div class="col-lg-4 col-md-4 col-sm-4" style="padding-bottom: 10px;">
                                        </div>
                                        <label class="col-lg-2 col-md-2 col-sm-2" style="padding-bottom: 10px;">รายละเอียด</label>
                                        <div class="col-lg-4 col-md-4 col-sm-4" style="padding-bottom: 10px;">
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-lg-2 col-md-2 col-sm-2" style="padding-bottom: 10px;">วันที่</label>
                                        <div class="col-lg-4 col-md-4 col-sm-4" style="padding-bottom: 10px;">
                                        </div>
                                        <label class="col-lg-2 col-md-2 col-sm-2" style="padding-bottom: 10px;">เวลา</label>
                                        <div class="col-lg-4 col-md-4 col-sm-4" style="padding-bottom: 10px;">
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-lg-2 col-md-2 col-sm-2" style="padding-bottom: 10px;">ผู้ขออนุมัติ</label>
                                        <div class="col-lg-4 col-md-4 col-sm-4" style="padding-bottom: 10px;">
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12" style="padding-bottom: 10px;">

                                            <div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="card-body">
                    <div class="card">
                        <div class="card-body" style="padding-top: 0.75rem;">
                            <div class="row">
                                <div class="col">
                                    <label>รายการสินค้า</label>
                                    &nbsp;&nbsp;<i class="fa fa-square" style="font-size:20px;color:#b3b1b1d1;" aria-hidden="true"></i>&nbsp;Open
                                    &nbsp;&nbsp;<i class="fa fa-square" style="font-size:20px;color:#00ff9054;" aria-hidden="true"></i>&nbsp;Asset
                                    &nbsp;&nbsp;<i class="fa fa-square" style="font-size:20px;color:#fb8d3e6b;" aria-hidden="true"></i>&nbsp;CI
                                    <div class="table-responsive">

                                        <asp:UpdatePanel ID="udpReceiveMaterial" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="table-Material" class="table table-sm table-striped table-bordered nowrap dataTable">
                                                    <thead>
                                                        <tr>
                                                            <th>Asset/CI</th>
                                                            <th>ลำดับ</th>
                                                            <th>การกำหนดบัญชี</th>
                                                            <th>รหัสสินค้า</th>
                                                            <th>ชื่อสินค้า</th>
                                                            <th>จำนวน</th>
                                                            <th>จำนวน PO</th>
                                                            <th>หน่วยนับ</th>
                                                            <th>ชื่อหน่วยนับ</th>
                                                            <th>ราคาต่อหน่วย</th>
                                                            <th>ราคาสุทธิ</th>
                                                            <th>โรงงาน</th>
                                                            <th>ชื่อโรงงาน</th>
                                                            <th>พื้นที่จัดเก็บ</th>
                                                            <th>ชื่อพื้นที่จัดเก็บ</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater ID="rptReceiveMaterial" runat="server" OnItemDataBound="rptReceiveMaterial_ItemDataBound">
                                                            <ItemTemplate>
                                                                <tr data-objid="<%# Eval("ObjectID") %>" data-item="<%# Eval("ItemNo") %>"
                                                                    data-mat="<%# Eval("MaterialCode") %>" data-uom="<%# Eval("ReceiveUom") %>"
                                                                    data-bom-mat="<%# Eval("MaterialCode") %>"
                                                                    data-bom-matname="<%# Eval("MaterialName") %>"
                                                                    <%# IsMatAssetNotBom(Eval("AccAss"),Container.DataItem) ? " class='c-pointer' onclick='onclickToEditSpiteItemDetail(this);' " : "" %>>
                                                                    <td style="<%# GetBackgroundColor(Eval("AccAss"),Eval("ASSETCOMPLETE"),Eval("EQUIPMENTCOMPLETE"),Container.DataItem) %>" class="text-center">
                                                                        <asp:CheckBox ID="chkSelectAsset" runat="server" />
                                                                    </td>
                                                                    <td class="text-truncate"><%# Eval("ItemNo") %>
                                                                        <asp:HiddenField ID="hddDocNumber" runat="server" Value='<%# Eval("DocNumber") %>' />
                                                                        <asp:HiddenField ID="hddPONumber" runat="server" Value='<%# Eval("RefPODocNumber") %>' />
                                                                        <asp:HiddenField ID="hddAccAssCatCode" runat="server" Value='<%# Eval("AccAss") %>' />
                                                                        <asp:HiddenField ID="hddItem" runat="server" Value='<%# Eval("ItemNo") %>' />
                                                                        <asp:HiddenField ID="hddFiscalYear" runat="server" Value='<%# Eval("FiscalYear") %>' />
                                                                        <%--<asp:HiddenField ID="hddPeriod" runat="server" Value='<%# Eval("PeriodNo") %>' />--%>
                                                                        <asp:HiddenField ID="hddMaterialCode" runat="server" Value='<%# Eval("MaterialCode") %>' />
                                                                        <asp:HiddenField ID="hddObjectID" runat="server" Value='<%# Eval("ObjectID") %>' />
                                                                        <asp:HiddenField ID="hddUOM" runat="server" Value='<%# Eval("ReceiveUom") %>' />
                                                                        <asp:HiddenField ID="hddPlantCode" runat="server" Value='<%# Eval("PlantCode") %>' />
                                                                        <asp:HiddenField ID="hddStorageCode" runat="server" Value='<%# Eval("StorageCode") %>' />
                                                                        <asp:HiddenField ID="hddPOItem" runat="server" Value='<%# Eval("ItemNo") %>' />
                                                                        <asp:HiddenField ID="hddRefPOCompanycode" runat="server" Value='<%# Eval("RefPOCompanycode") %>' />
                                                                        <asp:HiddenField ID="hddRefPODocNumber" runat="server" Value='<%# Eval("RefPODocNumber") %>' />
                                                                        <asp:HiddenField ID="hddRefPODocType" runat="server" Value='<%# Eval("RefPODocType") %>' />
                                                                        <asp:HiddenField ID="hddRefPOItem" runat="server" Value='<%# Eval("RefPOItem") %>' />
                                                                        <asp:HiddenField ID="hddRefPOYear" runat="server" Value='<%# Eval("RefPOYear") %>' />
                                                                        <asp:HiddenField ID="hddpendingPOQTY" runat="server" Value='<%# Eval("pendingPOQTY") %>' />
                                                                        <asp:HiddenField ID="hddReceiveQty" runat="server" Value='<%# Eval("ReceiveQty") %>' />

                                                                        <asp:HiddenField ID="hddASSETCOMPLETE" Value='<%# Eval("ASSETCOMPLETE") %>' runat="server" />
                                                                        <asp:HiddenField ID="hddEQUIPMENTCOMPLETE" Value='<%# Eval("EQUIPMENTCOMPLETE") %>' runat="server" />
                                                                        <asp:HiddenField ID="hddMaterialName" Value='<%# Eval("MaterialName") %>' runat="server" />
                                                                        <asp:HiddenField ID="hddisItemDetail" Value='<%# Eval("isItemDetail") %>' runat="server" />
                                                                    </td>
                                                                    <%-- <td class="text-truncate"><%# Eval("PeriodNo") %>
                                                                    </td>--%>
                                                                    <td class="text-truncate"><%# FormatACCASSIGNMENT(Eval("AccAss").ToString()) %>
                                                                    </td>
                                                                    <td class="text-truncate"><%# Eval("MaterialCode") %>
                                                                    </td>
                                                                    <td class="text-truncate"><%# Eval("MaterialName") %></td>
                                                                    <td class="text-right" style="min-width:60px;">
                                                                        <asp:TextBox runat="server" ID="txtQty" placeholder="Number" CssClass='<%# Eval("AccAss").Equals("A")||Eval("AccAss").Equals("K") ? "form-control form-control-sm text-right" :"hide" %>' onkeypress="ISFloat(this,event);" Text='<%# Eval("ReceiveQty", "{0:#,0.00}") %>' onchange="Qtychange(this,event)" />
                                                                        <asp:Label runat="server" ID="lbQty" CssClass=' <%# Eval("AccAss").Equals("A")||Eval("AccAss").Equals("K") ? "hide" :"" %>' Text='<%# Eval("ReceiveQty", "{0:#,0.00}") %>' />
                                                                        <asp:TextBox runat="server" ID="txtPriceUnit" CssClass="hide PriceUnit_txt" Text='<%# Eval("UnitPrice") %>' />
                                                                        <asp:TextBox runat="server" ID="txtPOQTY" CssClass="hide POQty_txt" Text='<%# Eval("PendingPOQty","{0:#,0.00}") %>' />
                                                                    </td>
                                                                    <td class="text-truncate text-right "><%# Eval("PendingPOQty","{0:#,0.00}") %></td>
                                                                    <td class="text-truncate "><%# Eval("ReceiveUom") %></td>
                                                                    <td class="text-truncate"><%# Eval("ReceiveUomDesc") %></td>
                                                                    <td class="text-truncate UnitPrice text-right"><%# Eval("UnitPrice","{0:#,0.00}") %></td>
                                                                    <td class="text-truncate NetValue text-right text-primary"><%# Eval("NetValue","{0:#,0.00}") %></td>
                                                                    <td class="text-truncate"><%# Eval("PlantCode") %></td>
                                                                    <td class="text-truncate"><%# Eval("PlantName") %></td>
                                                                    <td class="text-truncate"><%# Eval("StorageCode") %></td>
                                                                    <td class="text-truncate"><%# Eval("StorageName") %></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <tr>
                                                            <td colspan="10">รวม</td>
                                                            <td class="text-truncate text-right PI_BalanceAmount text-primary"><asp:Label runat="server" ClientIDMode="Static" ID="lbPI_BalanceAmount"></asp:Label></td>
                                                            <td colspan="4" />
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--item Detail ref matbom--%>
                <div class="hide">
                    <asp:UpdatePanel ID="udpOpenItemDetailRefMatBom" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <%--for header--%>
                            <asp:Button runat="server" ID="btnrecalFifoPaymentPlan" ClientIDMode="Static" CssClass="hide" OnClick="btnrecalFifoPaymentPlan_Click" />

                            <%--for item detail--%>
                            <asp:HiddenField ID="hddItemDetailOBjectID" runat="server" />
                            <asp:HiddenField ID="hddItemDetailItemNO" runat="server" />
                            <asp:HiddenField ID="hddItemDetailMaterial" runat="server" />
                            <asp:HiddenField ID="hddItemDetailUOM" runat="server" />
                            <asp:HiddenField ID="hddItemDetailBomMaterialCode" runat="server" />    
                            <asp:HiddenField ID="hddItemDetailBomMaterialName" runat="server" />
                            <asp:Button ID="btnOpenItemDetailRefMatBom" OnClick="btnOpenItemDetailRefMatBom_Click"  runat="server" />
                            <asp:Button ID="btnSplitItemQty" OnClick="btnSplitItemQty_Click" OnClientClick="$('.panel-item-detail-refmatbom').AGWhiteLoading(false);" runat="server" />
                            <asp:Button ID="btnSaveSplitItemDetail" OnClick="btnSaveSplitItemDetail_Click" OnClientClick="return AGDataConfirm(this,'ยืนยันการบันทึก');" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                </div>
                <asp:UpdatePanel ID="udpItemDetailData" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card-body panel-item-detail-refmatbom <%= rptItemDetailRefBom.Items.Count <= 0 ? "hide" : ""  %>">
                            <div class="card">

                                <div class="card-body" style="padding-top: 0.75rem;">
                                    <div class="form-row">
                                        <div class="col-md-12">
                                            <div class="form-row">
                                                <div class="col-md-3">
                                                    <label class="text-primary">
                                                        รายละเอียดรายการ&nbsp;
                                                        <asp:Label ID="lblItemDetailTitle" Text="-" runat="server" />
                                                    </label>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:TextBox ID="txtSplitItemQty" placeholder="Number"
                                                            CssClass="form-control form-control-sm text-right"
                                                            onkeypress="return isNumberKey(event);" runat="server" />
                                                </div>
                                                <div class="col" id="DEV_BTNSAVEDETAIL_SPLIT" runat="server">
                                                    <a class="btn btn-sm btn-warning" onclick="$('#<%= btnSplitItemQty.ClientID  %>').click();"><i class="fa fa-save"></i>&nbsp;split</a>
                                                    <a class="btn btn-sm btn-success" onclick="$('#<%= btnSaveSplitItemDetail.ClientID  %>').click();"><i class="fa fa-save"></i>&nbsp;save</a>
                                                </div>
                                            </div>
                                            <div class="table-responsive">

                                                <table id="table-itemdetail-refmatbom" class="table table-sm table-striped table-bordered nowrap dataTable">
                                                    <thead>
                                                        <tr>
                                                            <th></th>
                                                            <th>Item</th>
                                                            <th>Asset Group</th>
                                                            <th>Asset Type</th>
                                                            <th>Asset Code</th>
                                                            <th>CI Code</th>
                                                            <th style="min-width: 200px;">Description</th>
                                                            <th>Acquisition Value</th>
                                                            <th style="min-width: 120px;">Asset Date</th>
                                                            <th style="min-width: 150px;">Location 1</th>
                                                            <th style="min-width: 190px;">Cost center</th>
                                                            <th style="min-width: 190px;">Project</th>
                                                            <th style="min-width: 100px;">Usage/Year</th>
                                                            <th style="min-width: 100px;">Usage/Month</th>
                                                            <th style="min-width: 120px;">Brand</th>
                                                            <th style="min-width: 120px;">Serial No.</th>
                                                            <th style="min-width: 120px;">Model</th>
                                                            <th style="min-width: 150px;">Remark 1</th>
                                                            <th style="min-width: 150px;">Remark 2</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater ID="rptItemDetailRefBom" OnItemDataBound="rptItemDetailRefBom_ItemDataBound" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                       <asp:Label ID="lblDataDetail" Text='<%# Newtonsoft.Json.JsonConvert.SerializeObject(Container.DataItem) %>' CssClass="hide" runat="server"/>
                                                                        <asp:Label ID="txtITEMNUMBER" Text='<%# Eval("ITEMNUMBER") %>' runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblASSETGROUP" Text='<%# Eval("ASSETGROUP") %>' runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblASSETTYPE" Text='<%# Eval("ASSETTYPE") %>' runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblASSETCODE" Text='<%# Eval("ASSETCODE") %>' runat="server" />
                                                                    </td>
                                                                    <td>
                                                                       <asp:Label ID="lblEquipmentCode" Text='<%# Eval("EquipmentCode") %>' runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:HiddenField ID="hddBomMaterialCode" Value='<%# Eval("BomMaterialCode") %>' runat="server" />
                                                                        <asp:TextBox ID="txtASSETDESCRIPTION" Text='<%# Eval("ASSETDESCRIPTION") %>' CssClass="form-control form-control-sm" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtACQUSITIONVALUE" Text='<%# Convert.ToDecimal(Eval("ACQUSITIONVALUE")).ToString("#,##0.00") %>'
                                                                            runat="server" CssClass="form-control form-control-sm text-right" placeholder="Number"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="txtACQUISITIONDATE" Text='<%# String.IsNullOrEmpty(Eval("ACQUISITIONDATE").ToString()) ? "" : Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("ACQUISITIONDATE").ToString()) %>'
                                                                                runat="server" CssClass="form-control form-control-sm date-picker" placeholder="dd/mm/yyyy"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <uc1:AutoCompleteGeneral runat="server" id="AutoCompleteLocation1"
                                                                            CssClass="form-control form-control-sm "
                                                                            NotAutoBindComplete="true" ActionCase="location" />
                                                                        
                                                                    </td>
                                                                    <td>
                                                                        
                                                                       <uc1:AutoCompleteGeneral runat="server" id="AutoCompleteCostcenter"
                                                                            CssClass="form-control form-control-sm"
                                                                            NotAutoBindComplete="true" ActionCase="costcenter" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtPROJECTCODE" Text='<%# Eval("PROJECTCODE") %>'
                                                                                runat="server" CssClass="form-control form-control-sm" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtUSAGEYEAR" MaxLength="4" placeholder="Number" Text='<%# Eval("USAGEYEAR") %>'
                                                                            CssClass="form-control form-control-sm text-right"
                                                                            onkeypress="return isNumberKey(event);" runat="server" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:TextBox ID="txtUSAGEMONTH" MaxLength="2" placeholder="Number" Text='<%# Eval("USAGEMONTH") %>'
                                                                            CssClass="form-control form-control-sm text-right"
                                                                            onkeypress="return isNumberKey(event);" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtBrand" Text='<%# Eval("Brand") %>' CssClass="form-control form-control-sm" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtSerialno" Text='<%# Eval("Serialno") %>' CssClass="form-control form-control-sm" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtModel" Text='<%# Eval("Model") %>' CssClass="form-control form-control-sm" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRemark1" Text='<%# Eval("Remark1") %>' CssClass="form-control form-control-sm" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRemark2" Text='<%# Eval("Remark2") %>' CssClass="form-control form-control-sm" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="card-body">
                    <div class="card">
                        <div class="card-body" style="padding-top: 0.75rem;">
                            <div class="row">
                                <div class="col">
                                    <label>งวดการชำระเงิน</label>
                                    <div class="table-responsive">

                                        <asp:UpdatePanel ID="udpContrac" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="table-items table-item-payment-period" class="table table-sm table-striped table-bordered nowrap dataTable">
                                                    <thead>
                                                        <tr>
                                                            <th></th>
                                                            <th>งวดที่</th>
                                                            <th>จำนวนเงิน</th>
                                                            <th>จำนวนเงินตั้งจ่ายแล้ว</th>
                                                            <th>จำนวนค้างจ่าย</th>
                                                            <th>ขออนุมัติจ่าย</th>
                                                            <th>จำนวนคงเหลือ</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater ID="rptCrad" OnItemDataBound="rptCrad_ItemDataBound" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="_chk_select" runat="server" CssClass="chkbox_List chkbox_item"  onclick="caltotalPay(this);" Checked='<%# ((Convert.ToDecimal(Eval("ReqApproveAmount"))) > 0 ? true : false) %>' />
                                                                         <asp:Label ID="lblPaymentPeriodData" Visible="false" Text='<%# Newtonsoft.Json.JsonConvert.SerializeObject(Container.DataItem) %>' runat="server" />

                                                                    </td>

                                                                    <td class="text-truncate"><%# Eval("PeriodNo") %>
                                                                    </td>
                                                                    <td class="text-truncate text-right Amount"><%# Eval("NetValue","{0:#,0.00}") %></td>
                                                                    <td class="text-truncate text-right AmountPay"><%# Eval("PayAmount","{0:#,0.00}") %></td>
                                                                    <td class="text-truncate text-right AmountRemain"><%# Eval("PendingAmount","{0:#,0.00}") %></td>

                                                                    <td class="text-truncate text-right ApprovePay">
                                                                        <asp:TextBox runat="server" ID="txtApprovePay" CssClass="form-control form-control-sm text-right txtApprovePay" onkeypress="ISFloat(this,event);" Text='<%# Eval("ReqApproveAmount", "{0:#,0.00}") %>' onchange="ApprovePaychange(this,event)" />
                                                                        <asp:TextBox runat="server" ID="txtAmount" CssClass="hide Amount_txt" Text='<%# Eval("NetValue","{0:#,0.00}") %>' />
                                                                        <asp:TextBox runat="server" ID="txtAmountPay" CssClass="hide AmountPay_txt" Text='<%# Eval("PayAmount","{0:#,0.00}") %>' />
                                                                        <asp:HiddenField ID="hddPendingAmount" runat="server" Value='<%# Eval("PendingAmount") %>' />
                                                                       
                                                                    </td>
                                                                    <td class="text-truncate text-right BalanceAmount"><%# Eval("BalanceAmount","{0:#,0.00}") %></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <tr>
                                                            <td colspan="2">รวม</td>
                                                            <td class="text-truncate text-right PT_Amount"><asp:Label runat="server" ClientIDMode="Static" ID="lbPT_Amount"></asp:Label></td>
                                                            <td class="text-truncate text-right PT_AmountPay"><asp:Label runat="server" ClientIDMode="Static" ID="lbPT_AmountPay"></asp:Label></td>
                                                            <td class="text-truncate text-right PT_AmountRemain"><asp:Label runat="server" ClientIDMode="Static" ID="lbPT_AmountRemain"></asp:Label></td>
                                                            <td class="text-truncate text-right PT_ApprovePay"><asp:Label runat="server" ClientIDMode="Static" ID="lbPT_ApprovePay"></asp:Label></td>
                                                            <td class="text-truncate text-right PT_BalanceAmount"><asp:Label runat="server" ClientIDMode="Static" ID="lbPT_BalanceAmount"></asp:Label></td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--modal create asset--%>
    <div class="initiative-model-control-slide-panel" id="modalCreateAssetMaster">
        <div class="initiative-model-control-body-content z-depth-3">
            <div>
                <div class="initiative-model-control-header">
                    <div class="mat-box-initaive-control">
                        <div class="pull-right">
                            <i class="fa fa-close hand" onclick="closeInitiativeModal('modalCreateAssetMaster');"></i>
                        </div>
                        <div class="one-line">
                            <label class="text-warning">Create Asset</label>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-control-contant" style="top: 77px;">
                    <div class="panel-body-initiative-master">
                        <div class="panel-content-initiative-master">
                            <div class="mat-box-initaive-control tab-initiative-control">
                                <asp:UpdatePanel ID="udpModalCreateAsset" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <div class="card border-default" style="margin-bottom: 10px;">
                                            <div class="card-body card-body-sm">
                                                <div class="form-row">
                                                    <div class="form-group col-sm-4">
                                                        <label>Company Code</label>
                                                        <asp:DropDownList ID="ddlAssetCompayCode" CssClass="form-control form-control-sm required"
                                                              runat="server" DataTextField="NAME" 
                                                             DataValueField="ID">
                                                         </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-sm-4">
                                                        <label>DocType</label>
                                                        <asp:TextBox ID="txtAssetDoctype" CssClass="form-control form-control-sm required" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card border-default" style="margin-bottom: 10px;">
                                            <div class="card-body card-body-sm">
                                                 <div class="form-row">
                                                    <div class="form-group col-sm-4">
                                                        <label>Bu Area</label>
                                                         <asp:DropDownList ID="ddlAssetBuArea" CssClass="form-control form-control-sm required"
                                                              runat="server" DataTextField="Description" 
                                                             DataValueField="BusinessAreaCode">
                                                         </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-sm-4">
                                                        <label>Asset Group</label>
                                                         <asp:DropDownList ID="ddlAssetGroup" CssClass="form-control form-control-sm required" runat="server" 
                                                             DataTextField="Description" DataValueField="AssetGroup">
                                                         </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-sm-4">
                                                        <label>Depreciation Method</label>
                                                          <asp:DropDownList ID="ddlDepreciationMethod" CssClass="form-control form-control-sm required" 
                                                               runat="server" DataTextField="calculation_method_description" DataValueField="calculation_code">
                                                          </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-sm-4">
                                                        <label>Asset Type</label>
                                                         <asp:DropDownList ID="ddlAssetType" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server"
                                                              DataTextField="GroupName" DataValueField="GroupCode">
                                                         </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-sm-4">
                                                        <label>Asset Category1</label>
                                                         <asp:DropDownList ID="ddlAssetCategory1" CssClass="form-control form-control-sm" 
                                                              runat="server" DataTextField="Description" DataValueField="AssetCategory">
                                                         </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-sm-4">
                                                        <label>Asset Category2</label>
                                                          <asp:DropDownList ID="ddlAssetCategory2" CssClass="form-control form-control-sm" 
                                                               runat="server" DataTextField="Description" DataValueField="AssetCategory">
                                                          </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-bottom">
                    
                        <asp:UpdatePanel ID="udpCreateAssetModal" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <div class="text-right">
                                <a class="water-button" style="margin-right:3px;" onclick="$(this).next().click();">
                                    <i class="fa fa-save"></i>&nbsp;Save
                                </a>
                                <asp:Button ID="btnCreateAsset" OnClick="btnCreateAsset_Click" OnClientClick="AGLoading(true);" CssClass="hide" runat="server" />
                                <a class="water-button" onclick="closeInitiativeModal('modalCreateAssetMaster');"><i class="fa fa-close"></i>&nbsp;Close</a>
                            </div>
                         </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                
            </div>
        </div>
    </div>

    
    <%--modal create CI--%>
     <div class="initiative-model-control-slide-panel" id="modalAddEquipment">
        <div class="initiative-model-control-body-content z-depth-3">
            <div>
                <div class="initiative-model-control-header">
                    <div class="mat-box-initaive-control">
                        <div class="pull-right">
                            <i class="fa fa-close hand" onclick="closeInitiativeModal('modalAddEquipment');"></i>
                        </div>
                        <div class="one-line">
                            <label class="text-warning">
                                Create Configuration Item
                            </label>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-control-contant">
                    <div class="panel-body-initiative-master">
                        <div class="panel-content-initiative-master">
                            <div class="mat-box-initaive-control tab-initiative-control">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCreateEquipment">
                                    <ContentTemplate>

                                        <%--<div class="table-responsive">--%>
                                            <table id="table-material-create-ci" class="table table-sm table-striped table-bordered nowrap dataTable">
                                                <thead>
                                                    <tr>
                                                        <th style="max-width:50px;"></th>
                                                        <th>รหัสสินค้า</th>
                                                        <th>ชื่อสินค้า</th>
                                                        <th>จำนวน</th>
                                                        <th>ราคาสุทธิ</th>
                                                        <th class="hide">CI Code</th>
                                                        <th>Family</th>
                                                        <th>Class</th>
                                                        <th>Category</th>
                                                        <th>Status</th>
                                                        <th>Owner Service</th>

                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptMateriaBOMCreateCI" OnItemDataBound="rptMateriaBOMCreateCI_ItemDataBound" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="text-center">
                                                                    <%# Container.ItemIndex + 1 %>
                                                                    <asp:Label ID="lblBOMCreateCI_Data" Text='<%# Newtonsoft.Json.JsonConvert.SerializeObject(Container.DataItem)  %>' CssClass="hide" runat="server" />
                                                                </td>
                                                                <td><%# Eval("BomMaterialCode") %></td>
                                                                <td><%# Eval("BomMaterialName") %></td>
                                                                <td class="text-right"><%# Eval("BomQty","{0:#,0.00}") %></td>
                                                                <td class="text-right"><%# Eval("NETVALUE","{0:#,0.00}") %></td>
                                                                <td class="hide">
                                                                     <asp:TextBox ID="txtCICode" Text="" Enabled="false" placeholder="Text" runat="server" CssClass="form-control form-control-sm"/>
                                                                </td>
                                                                <td>
                                                                    <uc1:AutoCompleteGeneral runat="server" id="AutoCompleteFamilyCI"
                                                                        ActionCase="ci_family" NotAutoBindComplete="true" 
                                                                        CssClass="form-control form-control-sm required" />
                                                                </td>
                                                                <td>
                                                                     <uc1:AutoCompleteGeneral runat="server" id="AutoCompleteClassCI"
                                                                        ActionCase="ci_class" NotAutoBindComplete="true" 
                                                                        CssClass="form-control form-control-sm required" />
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm required" ID="ddlCategory">
                                                                        <asp:ListItem Text="" Value="" />
                                                                        <asp:ListItem Text="Main Configuration Item" Value="00" />
                                                                        <asp:ListItem Text="Virtual Configuration Item" Value="02" />
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlEquipmentStatusCI"
                                                                        DataTextField="StatusName" DataValueField="StatusCode">
                                                                        <asp:ListItem Text="" Value="" />
                                                                        <asp:ListItem Text="New" Value="N" />
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                     <uc1:AutoCompleteGeneral runat="server" id="AutoCompleteOwnerServiceCI"
                                                                        ActionCase="ci_ownersevice" NotAutoBindComplete="true" 
                                                                        CssClass="form-control form-control-sm required" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        <%--</div>--%>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <div class="hide">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button Text="" runat="server" ID="btnCreateCI"
                                    ClientIDMode="Static" OnClick="btnCreateCI_Click"
                                    OnClientClick="return AGDataConfirm(this,'ยืนยันการสร้าง CI');" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="initiative-model-bottom">
                    <div class="text-right">
                        <a class="water-button <%= this.AUTH_CREATE %>" onclick="$('#btnCreateCI').click();"><i class="fa fa-save"></i>&nbsp;Save</a>
                        
                        <a class="water-button" onclick="closeInitiativeModal('modalAddEquipment');"><i class="fa fa-close"></i>&nbsp;Close</a>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--modal confirm document--%>
    <div class="initiative-model-control-slide-panel" id="modalDocuemntReceiveRelease">
        <div class="initiative-model-control-body-content z-depth-3">
            <div>
                <div class="initiative-model-control-header">
                    <div class="mat-box-initaive-control">
                        <div class="pull-right">
                            <i class="fa fa-close hand" onclick="closeInitiativeModal('modalDocuemntReceiveRelease');"></i>
                        </div>
                        <div class="one-line">
                            <label class="text-warning">
                               ยืนยันรายการ
                            </label>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-control-contant">
                    <div class="panel-body-initiative-master">
                        <div class="panel-content-initiative-master">
                            <div class="mat-box-initaive-control tab-initiative-control">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpDocReceiveRelease">
                                    <ContentTemplate>
                                        <div class="form-row">
                                            <div class="col-sm-12 col-md-3">
                                                <label>ประเภทการรับ</label>
                                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm required" ID="ddlDocTypeReceive">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-xs-12 col-sm-6 col-md-3">
                                                <label>
                                                    เหตุผล
                                                </label>
                                                <asp:DropDownList runat="server" ID="ddlOrderReasonReceive" CssClass="form-control form-control-sm required" >
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group col-md-5">
                                                <label>ผู้รับ</label>
                                                <uc1:AutoCompleteEmployee runat="server" id="AutoComEmployeeReceive"
                                                    CssClass="form-control form-control-sm required" />
                                            </div>
                                        </div>
                                        <div class="form-row">
                                           
                                                <div class="col-xs-12 col-sm-12 col-md-6">
                                                    <label>
                                                        หมายเหตุ
                                                    </label>
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Rows="3" ID="txtRemarkReceive" />
                                                </div>
                                           
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <div class="hide">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button Text="" runat="server" ID="btnConfirmDocuemntReceive"
                                    ClientIDMode="Static" OnClick="btnConfirmDocuemntReceive_Click"
                                    OnClientClick="return AGDataConfirm(this,'ยืนยันการ');" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="initiative-model-bottom">
                    <div class="text-right">
                        <a class="water-button <%= this.AUTH_CREATE %>" onclick="$('#btnConfirmDocuemntReceive').click();"><i class="fa fa-save"></i>&nbsp;Save</a>
                        
                        <a class="water-button" onclick="closeInitiativeModal('modalDocuemntReceiveRelease');"><i class="fa fa-close"></i>&nbsp;Close</a>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function AGDataConfirm(obj, description)
        {
            if (AGConfirm(obj, description))
            {
                AGLoading(true);
                return true;
            }
            return false;
        }


        function isConfirmReleaseDocument(obj)
        {
            if ($("#<%= tbRationale.ClientID %>").val().trim() == "")
            {
                AGError("กรุณาระบุเหตุผลการตรวจรับ!");
                return false;
            }
            if (AGConfirm(obj, 'บันทึกยืนยันรายการ'))
            {
                AGLoading(true);
                return true;
            }
            return false;
        }
        function isConfirmCancelDocument(obj) {
            if ($("#<%= tbRationale.ClientID %>").val().trim() == "") {
                AGError("กรุณาระบุเหตุผล!");
                return false;
            }
            if (AGConfirm(obj, 'ยืนยันยกเลิกรายการ')) {
                AGLoading(true);
                return true;
            }
            return false;
        }

        function onclickToEditSpiteItemDetail(obj)
        {
            $("#table-Material tr.info").removeClass("info");
            $(obj).addClass("info");
            $(".panel-item-detail-refmatbom").AGWhiteLoading(true, "โหลดรายละเอียดรายการ...");
            var objID = $(obj).data("objid");
            var item = $(obj).data("item");
            var mat = $(obj).data("mat");
            var uom = $(obj).data("uom");
            var bommat = $(obj).data("bom-mat");
            var bommatname = $(obj).data("bom-matname");
            $("#<%= hddItemDetailOBjectID.ClientID %>").val(objID);
            $("#<%= hddItemDetailItemNO.ClientID %>").val(item);
            $("#<%= hddItemDetailMaterial.ClientID %>").val(mat);
            $("#<%= hddItemDetailUOM.ClientID %>").val(uom);
            $("#<%= hddItemDetailBomMaterialCode.ClientID %>").val(bommat);
            $("#<%= hddItemDetailBomMaterialName.ClientID %>").val(bommatname);
            $("#<%= btnOpenItemDetailRefMatBom.ClientID %>").click();
        }

        function closeWhiteLoadingItemDetail()
        {
            var objid = $("#<%= hddItemDetailOBjectID.ClientID %>").val();
            var item = $("#<%= hddItemDetailItemNO.ClientID %>").val();
            var matbom = $("#<%= hddItemDetailBomMaterialCode.ClientID %>").val();
            setRowCurrentItemDetail(objid, item, matbom);
        }
        function setRowCurrentItemDetail(objid, item,matbom)
        {
            $("#table-Material tr.info").removeClass("info");
            $("tr[data-objid='" + objid + "'][data-item='" + item + "'][data-bom-mat='" + matbom + "']").addClass("info");
            $('html,body').animate({
                scrollTop: $(".panel-item-detail-refmatbom").offset().top - 300
            });
            $('.panel-item-detail-refmatbom').AGWhiteLoading(false);
        }
    </script>
    <script>
        $(document).ready(function () {
            $('#table-items').DataTable({
                paging: false,
                info: false,
                searching: false,
            });
            $('#table-items2').DataTable({
                paging: false,
                info: false,
                searching: false,
            });
        });

        function ISFloat(obj, event) {
            $(obj).val($(obj).val().replace(/[^0-9\.]/g, ''));
            if ((event.which != 46 || event.which == 8 || event.which == 9 || $(obj).val().indexOf('.') != -1)
                      && ((event.which < 48 || event.which > 57))) {
                event.preventDefault();
            }
        }
        function convertToDecimal(val) {
            //consol.log("convertToDecimal");
            return parseFloat(val.replace(/,/g, ''));
        }

        function formatCurrency(obj, e) {
            $(obj).val(decimalFormat($(obj).val(), 2))
            return;
        }


        function numberFormat(number, digit) {
            var formatValue = new NumberFormat(number);
            formatValue.setPlaces(digit);
            return formatValue.toFormatted();
        }

        function Qtychange(obj, e) {
            var Qty_str = $(obj).val();
            var POQty_str = $(obj).closest('td').find('.POQty_txt').val();
            var unitprice_str = $(obj).closest('td').find('.PriceUnit_txt').val();
            var Qty = convertToDecimal(Qty_str);
            var POQty = convertToDecimal(POQty_str);
            var unitprice = convertToDecimal(unitprice_str);
            var sum = 0;
            if (Qty > POQty) {
                AGError("จำนวนไม่ถูกต้อง");
                $(obj).val(numberFormat(POQty, 2));
                //numberFormat(obj, 2);
            } else {
                sum = Qty * unitprice;
                $(obj).val(numberFormat(Qty, 2));
                //$(obj).closest('tr').find("td.Amount").html(numberFormat(sum, 2));
                //$(obj).closest('tr').find("td.ApprovePay").html(sum);
                $(obj).closest('tr').find("td.NetValue").html(numberFormat(sum, 2));
                $("#btnrecalFifoPaymentPlan").click();
            }
        }

        function ApprovePaychange(obj, e) {
            var Approve_str = $(obj).val();
            var Amount_str = $(obj).closest('td').find('.Amount_txt').val();
            var AmountPay_str = $(obj).closest('td').find('.AmountPay_txt').val();
            var Approve = convertToDecimal(Approve_str);
            var Amount = convertToDecimal(Amount_str);
            var AmountPay = convertToDecimal(AmountPay_str);
            var sum = Amount - AmountPay;
            if (Approve > sum) {
                AGError("ยอดเงินไม่ถูกต้อง");
                //$(obj).val(numberFormat(sum, 2));
                numberFormat($(obj));
                $(obj).val(numberFormat(sum, 2));
            } else {
                var AmountRemain = sum - Approve;

                $(obj).val(numberFormat(Approve, 2));
                $(obj).closest('tr').find("td.BalanceAmount").html(numberFormat(AmountRemain, 2));
                //$(obj).closest('tr').find("td.ApprovePay").html(sum);
                $(obj).closest("tr").find(".chkbox_item").find("input[type='checkbox']").prop('checked', (Approve > 0));
            }

            
            caltotalPay(obj);
        }
        function caltotalPay(obj) {
            var element = $(obj);
            var _element = element.parents('tbody').find(".txtApprovePay");
            var total = 0;
            for (i = 0 ; i < _element.length; i++) {
                if ($(_element[i]).closest("tr").find(".chkbox_item").find("input[type='checkbox']").is(':checked') != true)
                {
                    continue;
                }
                total += parseFloat(_element.eq(i).val() == "" ? 0 : convertToDecimal(_element.eq(i).val()));
                //if (parseFloat(_element.eq(i).val() == "" ? 0 : convertToDecimal(_element.eq(i).val())) > 0) {
                //    var chkbox = _element.eq(i).parents('tr').find(".chkbox_item").find("input");
                //    chkbox.attr("checked", true);
                //}
            }

            $("#lbPT_ApprovePay").html(numberFormat(total, 2));
        }
        //table-item-payment-period

    </script>

</asp:Content>

