<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlAddNewContact.ascx.cs" Inherits="ServiceWeb.crm.usercontrol.ctrlAddNewContact" %>


<div class="row">
    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
        <label>เบอร์โทรศัพท์ติดต่อ</label><span style="color: red;"> *</span>
        <asp:TextBox ID="txtTel" CssClass="form-control require input-lg" runat="server" placeholder="กรอกหมายเลขโทรศัพท์..." />
    </div>
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
        <label>ชื่อเล่น</label><span style="color: red;"> *</span>
        <asp:TextBox ID="txtNickName" CssClass="form-control input-lg require" runat="server" placeholder="กรอกชื่อเล่น..." />
    </div>
</div>
<div class="row">
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
        <label>ชื่อ</label>
        <asp:TextBox ID="txtFirstName" CssClass="form-control" runat="server" placeholder="กรอกชื่อจริง..." />
    </div>
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
        <label>นามสกุล</label>
        <asp:TextBox ID="txtLastName" CssClass="form-control" runat="server" placeholder="กรอกนามสกุล..." />
    </div>
</div>
<div class="row">
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
        <label>เพศ</label>
        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
            <asp:ListItem Text="ชาย" Value="M" />
            <asp:ListItem Text="หญิง" Value="F" />
        </asp:DropDownList>
    </div>
    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
        <label>
            ตำแหน่ง
        </label>
        <asp:TextBox ID="txtWorkPosition" runat="server" placeholder="กรอกตำแหน่งงาน..." CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="row">
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
        <label>
            ประเภทผู้ติดต่อ
        </label>
        <asp:DropDownList runat="server" ID="ddlContactType" CssClass="form-control">
        </asp:DropDownList>
    </div>
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
        <label>E-mail</label>
        <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" placeholder="Example@mail.com" />
    </div>
</div>
<div class="row">
    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
        <label>
            Facebook
        </label>
        <asp:TextBox ID="txtFaceBook" runat="server" ClientIDMode="static" placeholder="กรอกชื่อบัญชีของ facebook" CssClass="form-control "></asp:TextBox>
    </div>
    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
        <label>
            Instagram
        </label>
        <asp:TextBox ID="txtInstagram" runat="server" ClientIDMode="static" placeholder="กรอกชื่อบัญชีของ Instagram" CssClass="form-control "></asp:TextBox>
    </div>
</div>
<div class="row">
    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
        <label>
            Twitter
        </label>
        <asp:TextBox ID="txtTwitter" runat="server" ClientIDMode="static" placeholder="กรอกชื่อบัญชีของ Twitter" CssClass="form-control "></asp:TextBox>
    </div>
    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
        <label>
            Line
        </label>
        <asp:TextBox ID="txtLineID" runat="server" ClientIDMode="static" placeholder="กรอก Line ID" CssClass="form-control "></asp:TextBox>
    </div>
</div>
<div class="row">
    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
        <label>รายละเอียดเพิ่มเติม</label>
        <asp:TextBox TextMode="MultiLine" ID="txtRemark" Rows="1" CssClass="form-control dis" runat="server" placeholder="กรอกรายละเอียดเพิ่มเติม" />
    </div>
</div>
