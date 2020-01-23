<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="modalAddNewContact.ascx.cs" Inherits="ServiceWeb.crm.usercontrol.modalAddNewContact" %>
<style>
    .customer-card {
        padding: 7px 15px;
        font-weight: 700;
    }

        .customer-card .customer-img {
            width: 52px;
            display: inline;
        }

            .customer-card .customer-img .image-box {
                background-color: #575757;
                border: 1px solid #aaa;
                float: left;
                width: 50px;
                height: 50px;
                margin-right: 10px;
                background-image: url('<%= Page.ResolveUrl("~") %>images/user.png');
                background-position: center center;
                -webkit-background-size: cover;
                -moz-background-size: cover;
                background-size: cover;
                -o-background-size: cover;
                border-radius: 5px;
            }

        .customer-card .customer-desc {
            width: calc(100% - 52px);
            display: inline;
        }

    .one-line {
        overflow-x: hidden;
        white-space: nowrap;
        text-overflow: ellipsis;
    }

    .lines {
        margin-bottom: 10px;
        padding-bottom: 5px;
        border-bottom: 1px solid #ccc;
    }

    .info-button {
        color: #007bff !important;
        padding: 5px 8px;
        border: 2px solid #007bff;
        cursor: pointer;
        font-weight: 600;
    }

    .card.card-contact {
        border: none;
    }
</style>
<script>
    function ConfirmSaveAddnewContact(obj) {
        if (AGConfirm(obj, "Confirm save contact.")) {
            AGLoading(true, "save processing...");
            return true;
        }
        return false;
    }

    function OpenModalAddNewContactRefCustomer(customerCode, ItemContact) {
        var titleDesc = "Update Contact";
        if (customerCode == null || customerCode == undefined) {
            customerCode = "";
        }
        if (ItemContact == null || ItemContact == undefined || ItemContact == "") {
            ItemContact = "";
            titleDesc = "Create Contact";
        }
        $("#modal-CreateNewContact").find(".modal-title").html(titleDesc);
        $("#<%= hdfCustomerCode.ClientID %>").val(customerCode);
        $("#<%= hdfContactItem.ClientID %>").val(ItemContact);
        $("#<%= btnSetData.ClientID %>").click();
    }

</script>

<div id="modal-CreateNewContact" class="modal fade" role="dialog" data-keyboard="false" data-backdrop="static">
    <div class="modal-dialog modal-xs">
        <div class="modal-content">
            <div class="modal-header">
                <div class="card card-contact ">
                    <div class="card-body customer-card" style="width: 50px; height: 50px; padding: 0;">
                        <div class="">
                            <div class="customer-img">
                                <div class="image-box"></div>
                            </div>
                            <div class="customer-desc">
                            </div>
                        </div>
                    </div>
                </div>
                <h4 class="modal-title" style="padding-left: 10px;">Create Contact</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body card-contact">
                <asp:UpdatePanel ID="udpAddNewContact" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="card-body" style="padding: 7px 15px; margin-bottom: 5px;">
                            <asp:HiddenField ID="hdfCustomerCode" runat="server" />
                            <asp:HiddenField ID="hdfContactItem" runat="server" />
                            <asp:HiddenField ID="hdfContactType" runat="server" />
                            <div>
                                <div class="form-row">
                                    <div class="col-md-12">
                                        <b>Name</b>
                                        <asp:TextBox ID="txtName" CssClass="form-control form-control-sm" runat="server"
                                            placeholder="ชื่อ" />
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="col-md-12">
                                        <b>Service</b>
                                        <asp:TextBox ID="txtNickName" CssClass="form-control form-control-sm" runat="server"
                                            placeholder="บริการ" />
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="col-md-12">
                                        <b>Position</b>
                                        <asp:TextBox ID="txtPOSITION" CssClass="form-control form-control-sm" runat="server"
                                            placeholder="ตำแหน่ง" />
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="col-md-12">
                                        <b>Phone</b>
                                        <asp:TextBox ID="txtPhone" CssClass="form-control form-control-sm" runat="server"
                                            placeholder="เบอร์โทร" />
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="col-md-12">
                                        <b>Email</b>
                                        <asp:TextBox ID="txtEmail" CssClass="form-control form-control-sm " runat="server"
                                            placeholder="อีเมล์" />
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="col-md-12">
                                         <b>Authorization Contact</b>
                                        <asp:DropDownList ID="ddlAuthorizationContact" CssClass="form-control form-control-sm required" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="col-md-12">
                                        <b>Remark</b>
                                        <asp:TextBox ID="txtRemark" TextMode="MultiLine" CssClass="form-control form-control-sm" Rows="5" runat="server"
                                            placeholder="หมายเหตุ" />
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="col-md-12">
                                         <b>Status</b>
                                        <asp:DropDownList ID="ddlActiveStatus" CssClass="form-control form-control-sm" runat="server">
                                            <asp:ListItem Text="Active" Value="TRUE" />
                                            <asp:ListItem Text="Inactive" Value="FALSE" />
                                        </asp:DropDownList>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="modal-footer">
                <div class="row">
                    <div class="col-md-12 text-right">
                        <asp:UpdatePanel ID="udpSaveContact" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <button type="button" class="btn btn-default mb-1" data-dismiss="modal">
                                    <i class="fa fa-times fa-fw"></i>
                                    Close
                                </button>
                                <button type="button" class="btn btn-success mb-1" onclick="$(this).next().click();">
                                    <i class="fa fa-save fa-fw"></i>
                                    Save
                                </button>
                                <asp:Button ID="btnAddNewContact" CssClass="d-none" OnClick="btnAddNewContact_Click" OnClientClick="return ConfirmSaveAddnewContact(this);" Text="save" runat="server" />
                                <asp:Button ID="btnSetData" CssClass="d-none  AUTH_MODIFY" OnClick="btnSetData_Click" OnClientClick="AGLoading(true);" Text="SetData" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
