<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePass.ascx.cs"
    Inherits="ServiceWeb.UserProfile.usercontrol.ChangePass" %>

<div>
    <asp:UpdatePanel ID="upanelChangePassword" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <contenttemplate>
            <asp:UpdateProgress ID="upanelChangePasswordProgress" runat="server">
                <ProgressTemplate>
                    <div class="modal-backdrop fade in">
                    </div>
                    <asp:Image ID="imgUPanelLoading" ImageUrl="~/images/loadmessage.gif" Width="80px" Height="80px" runat="server" CssClass="img-loading-pos" />
                </ProgressTemplate>
            </asp:UpdateProgress>

            <div class="content-boxes">
                <asp:Panel ID="_Panel1" runat="server" CssClass="box">
                    <div class="form-group" style="text-align: left; width: 100%;">
                        <div style="padding-right: 20px; width: 50%;">
                            <div class="form-group">
                                <label for="_input_orderno">
                                    รหัสผ่านเดิม<span style="color:red !important;"> * </span>
                                </label>
                                <input id="txtOldPassword" type="password" class="form-control" runat="server" clientidmode="Static"
                                    placeholder="กรุณากรอก Password" />
                            </div>
                            <div class="form-group">
                                <label for="_input_orderno">
                                    รหัสผ่านใหม่<span style="color:red !important;"> * </span></label>
                                <input id="txtNewPassword" type="password" class="form-control" runat="server" clientidmode="Static"
                                    placeholder="กรุณากรอก Password" />
                            </div>
                            <div class="form-group">
                                <label for="_input_orderno">
                                    ยืนยันรหัสผ่าน<span style="color:red !important;"> * </span></label>
                                <input id="txtConfirmPassword" type="password" class="form-control" runat="server"
                                    clientidmode="Static" placeholder="กรุณากรอก Password" />
                            </div>
                            <div class="form-group">
                                <asp:Button ID="btnSave" runat="server" Text="ยืนยัน" CssClass="btn btn-success"
                                    ClientIDMode="Static" OnClick="btnSave_Click" Width="150px" OnClientClick="AGLoading(true);" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </contenttemplate>
    </asp:UpdatePanel>
</div>