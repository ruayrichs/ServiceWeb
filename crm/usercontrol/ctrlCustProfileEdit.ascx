<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrlCustProfileEdit.ascx.cs" Inherits="POSWeb.crm.usercontrol.ctrlCustProfileEdit" %>
<%--<div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 text-center">
    <div class="circle-image" style="width: 72px; height: 72px;">
        <uc1:uploadgallery runat="server" id="UploadPictureTitle" accepttype="image" multiplemode="false" previewwidth="72px" previewheight="72px" />
    </div>
</div>--%>
<div class="row">
    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
        <label>เบอร์โทรศัพท์ติดต่อ</label><span class="text-danger"> *</span>
        <asp:TextBox ID="txtPhone" runat="server" ClientIDMode="static" placeholder="กรอกหมายเลขโทรศัพท์..." CssClass="form-control require number input-lg"></asp:TextBox>
    </div>
    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
        <label>ชื่อเล่น</label><span class="text-danger"> *</span>
        <asp:TextBox ID="txtNicknameEdit" runat="server" placeholder="กรอกชื่อเล่น..." CssClass="form-control require input-lg"></asp:TextBox>
    </div>
</div>
<div class="row">
    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
        <label>ชื่อ</label>
        <asp:TextBox ID="txtFirstNameEdit" runat="server" placeholder="กรอกชื่อจริง..." CssClass="form-control require"></asp:TextBox>
    </div>
    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
        <label>นามสกุล</label>
        <asp:TextBox ID="txtLastNameEdit" runat="server" placeholder="กรอกนามสกุล..." CssClass="form-control require"></asp:TextBox>
    </div>
</div>
<div class="row">
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
        <label>
            เพศ
        </label>
        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
            <asp:ListItem Text="ชาย" Value="M" />
            <asp:ListItem Text="หญิง" Value="F" />
        </asp:DropDownList>
    </div>
    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
        <label>
            ตำแหน่ง
        </label>
        <asp:TextBox ID="txtWorkPositionEdit" runat="server" placeholder="กรอกตำแหน่งงาน..." CssClass="form-control"></asp:TextBox>
    </div>
    
</div>
<div class="row">
    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
        <label>
            Email
        </label>
         <asp:TextBox ID="txtEmailEdit" runat="server" ClientIDMode="static" placeholder="Example@mail.com" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
        <label>
            Facebook
        </label>
        <asp:TextBox ID="txtFaceBookEdit" runat="server" ClientIDMode="static" placeholder="กรอกชื่อบัญชีของ facebook" CssClass="form-control "></asp:TextBox>
    </div>
    
</div>
<div class="row">
    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
        <label>
            Instagram
        </label>
        <asp:TextBox ID="txtInstagramEdit" runat="server" ClientIDMode="static" placeholder="กรอกชื่อบัญชีของ Instagram" CssClass="form-control "></asp:TextBox>
    </div>
    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
        <label>
            Twitter
        </label>
        <asp:TextBox ID="txtTwitterEdit" runat="server" ClientIDMode="static" placeholder="กรอกชื่อบัญชีของ Twitter" CssClass="form-control "></asp:TextBox>
    </div>
</div>
<div class="row">
    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
        <div class="text-right">
            <asp:Button ID="btnSaveProfileSocial" runat="server" CssClass="btn btn-primary" OnClientClick='return AGValidator(this);' OnClick="btnSaveProfileSocial_Click" Text="Save" />
        </div>
    </div>
</div>