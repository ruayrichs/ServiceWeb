<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master"  ValidateRequest="false" AutoEventWireup="true" CodeBehind="EmailMessageDetail.aspx.cs" Inherits="ServiceWeb.MasterConfig.EmailMessageDetail" %>

<%@ Register Src="~/AGHtmlEditor/AGHtmlEditorControl.ascx" TagPrefix="uc1" TagName="AGHtmlEditorControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-email-message").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
     <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <a class="btn btn-warning mb-1" href="EmailMessageCriteria.aspx"><i class="fa fa-arrow-circle-left"></i>&nbsp;&nbsp;Back</a>
                    <button type="button" class="btn btn-success mb-1 AUTH_MODIFY" onclick="validateSave();"><i class="fa fa-save"></i>&nbsp;&nbsp;Save</button>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>
     <asp:UpdatePanel runat="server" UpdateMode="Conditional" class="d-none">
        <ContentTemplate>
            <asp:Button ID="btnValidateSave" runat="server" OnClick="btnValidateSave_Click" />
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="return confirmSave(this);" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">ข้อมูลหลักข้อความในอีเมล์</h5>
                </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label>รหัสข้อความ</label>
                            <asp:TextBox ID="txtMessageCode" CssClass="form-control form-control-sm required" runat="server" />
                            <asp:Label ID="txtEventCode" CssClass="d-none" runat="server" />
                        </div>
                        <div class="form-group col-md-6">
                            <label>เหตุการณ์ที่ส่งเมล์</label>
                            <asp:TextBox ID="txtEventName" CssClass="form-control form-control-sm " runat="server" Enabled="false" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label>Remark</label>
                            <asp:TextBox ID="txtRemark" TextMode="MultiLine" Rows="3" CssClass="form-control form-control-sm " runat="server" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label>หัวเรื่องอีเมลล์</label>
                            <asp:TextBox ID="txtEmailSubject" CssClass="form-control form-control-sm required" runat="server" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label>ข้อความอีเมลล์</label>
                            <uc1:AGHtmlEditorControl runat="server" id="HtmlEditor" />
                        </div>
                    </div>

                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label>วันที่สร้าง</label>
                            <asp:TextBox ID="txtCreatedOn" CssClass="form-control form-control-sm " runat="server" Enabled="false" />
                        </div>
                        <div class="form-group col-md-6">
                            <label>สร้่างโดย</label>
                            <asp:TextBox ID="txtCreatedBy" CssClass="form-control form-control-sm " runat="server" Enabled="false" />
                        </div>
                    </div>

                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label>วันที่แก้ไข</label>
                            <asp:TextBox ID="txtUpdatedOn" CssClass="form-control form-control-sm " runat="server" Enabled="false" />
                        </div>
                        <div class="form-group col-md-6">
                            <label>แก้ไขโดย</label>
                            <asp:TextBox ID="txtUpdatedBy" CssClass="form-control form-control-sm " runat="server" Enabled="false" />
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>


    <script>
        function validateSave() {
            $(".required").prop('required', true);
            $("#<%= btnValidateSave.ClientID %>").click();
        }
        function saveEmailMessage() {
            $("#<%= btnSave.ClientID %>").click();
        }
        function confirmSave(obj) {
            if (AGConfirm(obj, "ยืนยันการบันทึก")) {
                AGLoading(true);
                return true;
            }
            return false;
        }
    </script>

</asp:Content>
