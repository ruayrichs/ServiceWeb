<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SalesVisitEventMasterControl.ascx.cs" Inherits="POSWeb.crm.usercontrol.SalesVisitEventMasterControl" %>
<%@ Register Src="~/widget/usercontrol/AutoCompleteControl.ascx" TagPrefix="uc1" TagName="AutoCompleteControl" %>
<div class="hide">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCode">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hddCustomerName" />
            <asp:HiddenField runat="server" ID="hddCustomerCode" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<style type="text/css">
    .font-color-000 {
        color: #000;
    }

    .text-edit {
        background-color: #fffce0;
        width: 100%;
        height: 38px;
        border: none;
        padding: 0px 3px;
    }

    .none-padding {
        padding: 0px !important;
    }

    .color-red {
        color: #e7505a;
    }

    .color-green {
        color: #288c00;
    }

    .overview-init-owner {
        padding: 7px;
        border: 1px solid #fff;
        margin-bottom: 1px;
    }

        .overview-init-owner.overview-init-create-mode:hover {
            background: #FAFFBD;
            cursor: pointer;
        }

        .overview-init-owner .selected-owner-desc {
            display: none;
            color: #4CAF50;
        }

        .overview-init-owner.selected-owner {
            border: 1px solid #4CAF50;
            border-radius: 5px;
        }

            .overview-init-owner.selected-owner .selected-owner-desc {
                display: block;
            }

    .img-box-ini-style {
        float: left;
        background-size: cover;
        background-position: center center;
        -webkit-background-size: cover;
        -moz-background-size: cover;
        background-size: cover;
        -o-background-size: cover;
        width: 60px;
        height: 60px;
        display: block;
        border: 1px solid #ccc;
        margin-right: 5px;
    }
</style>
<div class="panel-view-mode">
    <div class="row hide">
        <a onclick="swithModeInsertData(this);" style="cursor: pointer;"><i class="fa fa-plus-circle"></i>&nbsp;Create Event Mapping</a>
    </div>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpAddNewItem">
        <ContentTemplate>
            <div class="row mode-insert-data" style="display: none;">
                <div class="row">
                    <div class="col-md-6 col-sm-12">
                        <label class="">Event Name</label>
                        <asp:DropDownList ID="ddlEventCode" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-md-6 col-sm-12">
                        <label class="">Account Ability Name</label>
                        <asp:DropDownList ID="ddlAccountAbility" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 col-sm-12">
                        <label class="">Owner Name</label>
                        <uc1:AutoCompleteControl runat="server" id="AutoCompleteOwnerControl" CssClass="form-control" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 col-sm-12">
                        <input type="button" class="btn btn-success" value="Save" onclick="return IsComfirmInsertArticleCatetigory(this);" />
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                            <ContentTemplate>
                                <asp:Button ID="btnsaveMaster" CssClass="hide btnSave" Text="Save" runat="server" OnClick="btnsaveMaster_Click" ClientIDMode="Static" OnClientClick="AGLoading(true);" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--<hr />--%>
    <div class="row">
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12 ">
                    <asp:UpdatePanel ID="udpTable" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Repeater ID="rptItemsMaster" runat="server" OnItemDataBound="rptItemsMaster_ItemDataBound">
                                <ItemTemplate>
                                    <table class="table table-bordered table-striped">
                                        <tr>
                                            <th style="vertical-align: middle;background-color: #009688;color: #fff;">
                                                <%# Eval("EVENT_CODE") + " : " +  Eval("EventName")%>
                                            </th>
                                            
                                            <th style="width: 65px;background-color: #009688;color: #fff;text-align: center;">
                                                <span onclick="toggleSwithModeEdit(this);" style="cursor: pointer;">
                                                    <i class="fa fa-pencil-square-o"></i> Edit
                                                </span>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td class="text-edit-tr" colspan="2">
                                                <div class="mode-show-desc" style="float: right;position: absolute;right: 10px;">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <span class="col-xs-12 <%# Convert.ToBoolean(Eval("flag")) ? " color-green " : " color-red " %>">
                                                                <i class="<%# Convert.ToBoolean(Eval("flag")) ? " fa fa-check-circle-o color-green" : " fa fa-times-circle-o color-red" %>"></i> <%# Convert.ToBoolean(Eval("flag")) ? "Active" : "In Active" %>
                                                            </span>
                                                            <asp:Button ID="btnUpdateFlag" CssClass="hide" runat="server" OnClick="btnUpdateFlag_Click" CommandArgument='<%# Eval("flag") %>' OnClientClick="AGLoading(true);" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-12 col-md-4">
                                                        <div class="mode-edit-desc hide">
                                                            <label>
                                                                Owner
                                                            </label>
                                                            <uc1:AutoCompleteControl runat="server" id="ddlOwnerItm" CssClass="form-control" />
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-12 col-md-8">
                                                        <div class="mode-edit-desc hide">
                                                            <label>
                                                                Account Ability Name
                                                            </label>
                                                            <asp:DropDownList ID="ddlAccountItm" CssClass="form-control" runat="server"></asp:DropDownList>
                                                            
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-12 col-md-12">
                                                        <div class="mode-edit-desc hide">
                                                            <br />
                                                            <div>
                                                                <asp:CheckBox Text=" Active" runat="server" ID="chkActive"
                                                                    Checked='<%# Convert.ToBoolean(Eval("flag")) %>' />
                                                            </div>
                                                            <div>
                                                                <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel2s" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:HiddenField ID="hhdEVENT_CODE" runat="server" Value='<%# Eval("EVENT_CODE") %>' />
                                                                        <asp:HiddenField ID="hhdCUSTOMER_CODE" runat="server" Value='<%# Eval("CUSTOMER_CODE") %>' />
                                                                        <asp:Button OnClientClick="return IsComfirmUpdateArticleCatetigory(this);AGLoading(true);"
                                                                            ID="btnUpdateItem" class="btn btn-success" Style="margin-top: 10px;" Text="Save" runat="server"
                                                                            OnClick="btnUpdateItem_Click" CommandArgument='<%# Eval("SID") %>' CommandName='<%# Eval("IsNew") %>' />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-12 col-md-12">
                                                        <div class="mode-show-desc">
                                                            <label>
                                                                <%# Eval("_ACCOUNTABILITY") %>
                                                                &nbsp;
                                                            </label>
                                                            <hr style="margin: 7px 0px;">
                                                            <div class="row">
                                                                <div class="col-xs-12 col-sm-12 col-md-3">
                                                                    <div>
                                                                        <label>
                                                                            Owner
                                                                        </label>
                                                                    </div>
                                                                    <div class="alert alert-info <%# Convert.ToBoolean(Eval("IsNew").ToString()) ? "" : " hide " %>">
                                                                        No data.
                                                                    </div>
                                                                    <div class="overview-init-owner <%# Convert.ToBoolean(Eval("IsNew").ToString()) ? " hide " : "" %>" 
                                                                        data-owner-linkid="<%# Eval("OWNER_LINKID") %>" style="border: 1px solid #dfe0e4; border-radius: 5px;">
                                                                        <table style="table-layout: fixed; width: 100%">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td style="width: 50px; vertical-align: top;">
                                                                                        <div class="img-box-ini-style" style="float: none; width: 38px; height: 38px; border-radius: 0; background-image: url('<%# "/images/profile/128/" + SID + "_" + CompanyCode + "_" + Eval("OWNER_LINKID").ToString() + ".png" %>'),url('/images/user.png');"></div>
                                                                                    </td>
                                                                                    <td style="vertical-align: top;">
                                                                                        <div class="one-line">
                                                                                            <span><%# Eval("_OWNER_LINKID") %></span>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </div>
                                                                    <label>
                                                                        Main
                                                                    </label>
                                                                    <div class="alert alert-info <%# Convert.ToBoolean(Eval("IsNew").ToString()) ? "" : " hide " %>">
                                                                        No data.
                                                                    </div>
                                                                    <asp:UpdatePanel ID="updInitiativeOwner" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <div class="hide">
                                                                                <asp:Button Text="text" ID="btnChangeSubProjectOverViewModelControl"
                                                                                    ClientIDMode="Static" runat="server" meta:resourcekey="btnChangeSubProjectResource1" />
                                                                            </div>
                                                                            <asp:Repeater runat="server" ID="rptInitiativeOwner">
                                                                                <ItemTemplate>
                                                                                    <div class="overview-init-owner <%# Container.ItemIndex > 0 ? " hide " : "  " %>" data-owner-linkid="<%# Eval("LINKID") %>" style="border: 1px solid #dfe0e4; border-radius: 5px;">
                                                                                        <asp:HiddenField runat="server" ID="hddIOLinkID" Value='<%# Eval("LINKID") %>' />
                                                                                        <table style="table-layout: fixed; width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 50px; vertical-align: top;">
                                                                                                    <div class="img-box-ini-style" style="float: none; width: 38px; height: 38px; border-radius: 0; background-image: url('<%# "/images/profile/128/" + SID + "_" + CompanyCode + "_" + Eval("EmployeeCode").ToString() + ".png" %>'),url('/images/user.png');"></div>
                                                                                                </td>
                                                                                                <td style="vertical-align: top;">
                                                                                                    <div class="one-line">
                                                                                                        <asp:Label ID="lbInitiativeOwner" runat="server" Text='<%# Eval("fullname") %>'></asp:Label>
                                                                                                    </div>
                                                                                                    <div class="selected-owner-desc">
                                                                                                        <b>
                                                                                                            <i class="fa fa-check-circle"></i>
                                                                                                            Initiative Owner
                                                                                                        </b>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                            <div class="hide">
                                                                                <asp:TextBox runat="server" ID="txtHiddenOverviewOwnerLinkID" ClientIDMode="Static" />
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                                <div class="col-xs-12 col-sm-12 col-md-9">
                                                                    <label>
                                                                        Participants
                                                                    </label>
                                                                    <div class="alert alert-info <%# Convert.ToBoolean(Eval("IsNew").ToString()) ? "" : " hide " %>">
                                                                        No data.
                                                                    </div>
                                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpParticipants">
                                                                        <ContentTemplate>
                                                                            <asp:Repeater runat="server" ID="rptStructureParticipant" OnItemDataBound="rptStructureParticipant_ItemDataBound">
                                                                                <ItemTemplate>
                                                                                    <div class="mat-box">
                                                                                        <label class="text-primary">
                                                                                            <%# Eval("StructureName") %>
                                                                                            <asp:HiddenField ID="hddStructureCode" runat="server" Value='<%# Eval("StructureCode") %>'></asp:HiddenField>
                                                                                        </label>
                                                                                        <div class="row">
                                                                                            <asp:Repeater runat="server" ID="rptStructureParticipantRole">
                                                                                                <ItemTemplate>
                                                                                                    <div class="col-sm-4">
                                                                                                        <div class="row">
                                                                                                            <asp:HiddenField runat="server" ID="hddParticipantLinkID" Value='<%# Eval("LINKID") %>' />
                                                                                                            <div class="col-xs-2 col-sm-2 col-md-2" style="margin: 5px 0px;">
                                                                                                                <div class="img-box-ini-style" style="float: none; width: 38px; height: 38px; border-radius: 0; background-image: url('<%# "/images/profile/128/" + SID + "_" + CompanyCode + "_" + Eval("EmployeeCode").ToString() + ".png" %>'),url('/images/user.png');"></div>
                                                                                                                <%--Todo FocusOneLinkProfileImageByEmployeeCode--%>
                                                                                                            </div>
                                                                                                            <div class="col-xs-10 col-sm-10 col-md-10" style="margin: 5px 0px;">
                                                                                                                <div class="one-line">
                                                                                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                                                                </div>
                                                                                                                <div class="text-success">
                                                                                                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("HierarchyDesc") %>'></asp:Label>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </ItemTemplate>
                                                                                            </asp:Repeater>
                                                                                        </div>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>

                                                                            <asp:Repeater runat="server" ID="rptParticipants">
                                                                                <ItemTemplate>
                                                                                    <div class="row">
                                                                                        <div style="padding: 7px; margin-bottom: 25px;">
                                                                                            <div class="col-lg-12">
                                                                                                <div class="img-box-ini-style" style="width: 38px; height: 38px; border-radius: 0; background-image: url('<%# "/images/profile/128/" + SID + "_" + CompanyCode + "_" + Eval("EmployeeCode").ToString() + ".png" %>'),url('/images/user.png');"></div>
                                                                                                <%--Todo FocusOneLinkProfileImageByEmployeeCode--%>
                                                                                                <div class="pull-left">
                                                                                                    <asp:Label ID="lbParticipants" runat="server" Text='<%# Eval("fullname") %>'></asp:Label>
                                                                                                    <br />
                                                                                                    <asp:Label ID="lbCharacter" runat="server" Style="color: #aaa;" Text='<%# Eval("HierarchyDesc") %>'></asp:Label>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</div>

<script>
    function OverviewSelectedOwner(linkID, isCreateMode) {
        if (isCreateMode) {
            $(".overview-init-owner").addClass("overview-init-create-mode").click(function () {
                OverviewChangeOwner(this);
            });
        }

        var findLinkID = $(".overview-init-owner[data-owner-linkid='" + linkID + "']");
        var obj = null;
        if (findLinkID.length > 0) {
            obj = findLinkID;
        }
        if (obj == null && isCreateMode) {
            obj = $(".overview-init-owner:first");
        }

        if (obj != null && obj.length > 0) {
            obj.addClass("selected-owner");
        }

        OverviewChangeOwnerLinkID();
    }
    function OverviewChangeOwner(obj) {
        $(".overview-init-owner").removeClass("selected-owner");
        $(obj).addClass("selected-owner");
        OverviewChangeOwnerLinkID();
    }
    function OverviewChangeOwnerLinkID() {
        var linkID = "";
        var selected = $(".overview-init-owner.selected-owner");
        if (selected.length > 0)
            linkID = $(selected).attr("data-owner-linkid");

        $("#txtHiddenOverviewOwnerLinkID").val(linkID);
    }
</script>
<script>
    function toggleSwithModeEdit(obj) {
        $(obj).closest('tr').next().find('.mode-show-desc').toggleClass('hide');
        $(obj).closest('tr').next().find('.mode-edit-desc').toggleClass('hide');
        //$(obj).closest('tr').next().find('.text-edit-tr').toggleClass('none-padding');
    }
    function IsComfirmUpdateArticleCatetigory(obj) {
        if (AGConfirm(obj, 'คุณต้องการแก้ไขรายการที่เลือกใช่หรือไม่')) {
            AGLoading(true);
            return true;
        } else {
            return false;
        }
    }
    function IsComfirmInsertArticleCatetigory(obj) {
        if (AGConfirm(obj, 'คุณต้องการบันทึกรายการใช่หรือไม่')) {
            $(".btnSave").click();
            return true;
        } else {
            return false;
        }
    }
    function swithModeInsertData(obj) {
        var modeAdd = $(".mode-insert-data");
        $(modeAdd).toggleClass('withmode');
        if ($(modeAdd).hasClass('withmode')) {
            $(".mode-insert-data").slideDown().fadeIn();
        }
        else {
            $(".mode-insert-data").slideUp().fadeOut();
        }
    }
</script>
