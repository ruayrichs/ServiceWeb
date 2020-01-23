<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkerAssignedControl.ascx.cs" Inherits="ServiceWeb.crm.usercontrol.WorkerAssignedControl" %>

<style>
    .img-box-ini-style {
        background-size: cover;
        background-position: center center;
        -webkit-background-size: cover;
        -moz-background-size: cover;
        background-size: cover;
        -o-background-size: cover;
        display: block;
        width: 70px;
        height: 70px;
        border-radius: 50px;
        border: 1px solid #ccc;
    }
</style>
<div class="hide">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCodectr">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hddWorkGroupCode" />
            <asp:HiddenField runat="server" ID="hddTierCode" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<asp:UpdatePanel ID="updWorkAssigned" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="row">
            <asp:Repeater ID="rptOperation" runat="server" OnItemDataBound="rptOperation_OnItemDataBound">
                <ItemTemplate>
                    <div class="mat-box" style="padding-bottom:0px;">
                        <div class="row">
                            <div class="col-md-12" style="text-align:right;color: #4f7ea7;">
                                <asp:Label ID="lbTierLevelName" runat="server" CssClass="col-md-12"><strong>หน่วยงานที่รับผิดชอบ</strong> : <%# Eval("TierDescription") %></asp:Label>
                            </div>
                            <hr style="margin-top:15px;margin-bottom:10px;"/>
                        </div>
                        <div class="row">
                            <asp:HiddenField ID="hddTier" runat="server" Value='<%# Eval("Tier") %>'></asp:HiddenField>
                            <div class="col-md-4">
                                <div>
                                    <label>ผู้รับมอบหมายหลัก</label>
                                </div>
                                <div class="mat-box" style="overflow-y: auto;background: aliceblue; border-left: 3px solid #4f7ea7;">
                                    <asp:Repeater ID="rptMainDelegate" runat="server">
                                        <ItemTemplate>
                                            <asp:HiddenField  runat="server" ID="hddMainLinkID" Value='<%# Eval("LINKID") %>' />
                                            <div class="row">
                                                <div class="col-md-2">
                                                    <div class="img-box-ini-style" style="float:none;width:38px;height:38px;border-radius: 0; background-image: url('<%# PublicAuthentication.FocusOneLinkProfileImageByEmployeeCode(Eval("EmployeeCode").ToString()) %>');"></div>
                                                </div>
                                                <div class="col-md-10">
                                                    <div class="one-line" style="font-weight: bold;">
                                                        <asp:Label ID="lbMainDelegate" runat="server" Title='<%# Eval("fullname") %>' Text='<%# Eval("fullname") %>'></asp:Label>
                                                    </div>
                                                    <div class="one-line">
                                                        <asp:Label ID="lbMainCharacter" runat="server" Title='<%# Eval("HierarchyDesc") %>' Text='<%# Eval("HierarchyDesc") %>'></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                            <div class="col-md-8">
                                <div>
                                    <label>ผู้รับมอบหมายอื่น</label>
                                </div>
                                <div class="mat-box" style="overflow-y: auto;background: lightgoldenrodyellow;border-left: 3px solid rgba(220, 220, 10, 0.45);">
                                    <asp:Panel ID="PanelShowOther" runat="server">
                                        <asp:Repeater ID="rptOtherDelegate" runat="server">
                                            <ItemTemplate>
                                                <div class="col-md-4 overview-init-owner" style="display:inline-block;">
                                                    <asp:HiddenField  runat="server" ID="hddParLinkID" Value='<%# Eval("LINKID") %>' />
                                                    <div class="row">
                                                        <div class="col-md-3">
                                                            <div class="img-box-ini-style" style="float:none;width:38px;height:38px;border-radius: 0; background-image: url('<%# PublicAuthentication.FocusOneLinkProfileImageByEmployeeCode(Eval("EmployeeCode").ToString()) %>');"></div>
                                                        </div>
                                                        <div class="col-md-9">
                                                            <div class="one-line" style="font-weight: bold;">
                                                                <asp:Label ID="lbOtherDelegate" runat="server" Title='<%# Eval("fullname") %>' Text='<%# Eval("fullname") %>'></asp:Label>
                                                            </div>
                                                            <div class="one-line">
                                                                <asp:Label ID="lbOtherCharacter" runat="server" Title='<%# Eval("HierarchyDesc") %>' Text='<%# Eval("HierarchyDesc") %>'></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </asp:Panel>
                                    <asp:Panel ID="PanelHideOther" runat="server">
                                        <div class="col-md-4">
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <div class="img-box-ini-style" style="float:none;width:38px;height:38px;border-radius: 0; background-image: url('<%= Page.ResolveUrl("~") %>images/user.png');"></div>
                                                </div>
                                                <div class="col-md-9">
                                                    <div class="one-line" style="font-weight: bold;">
                                                        <asp:Label ID="Label4" runat="server" Text='ไม่ได้ระบุ'></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>