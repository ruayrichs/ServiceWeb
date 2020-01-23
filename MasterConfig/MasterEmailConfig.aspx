<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="MasterEmailConfig.aspx.cs" Inherits="ServiceWeb.MasterConfig.MasterEmailConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-email-config").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
        <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
            <div class="pull-left">
                <asp:UpdatePanel runat="server" ID="udpbtn" UpdateMode="Conditional">
                    <ContentTemplate>
                        <button type="button" class="btn btn-success mb-2 AUTH_MODIFY" onclick="$(this).next().click();"> <i class="fa fa-save" >&nbsp;&nbsp;Save</i></button>
                        <asp:Button runat="server" Text="" CssClass="btn btn-success d-none" ID="btn_create" OnClick="btn_create_Click" OnClientClick="AGLoading(true);" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </nav>
        <div class="card shadow">
            <div class="card-header">
                <h5 class="mb-0">Email Config</h5>
            </div>


            <div class="card-body panel-body-customer-search PANEL-DEFAULT-BUTTON">
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="card border-default" style="margin-bottom: 10px;">
                            <div class="card-body card-body-sm">
                                <div class="form-row">
                                    <div class="form-group col-sm-6">
                                        <div>
                                            <label>Host</label>
                                        </div>
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm required" ID="tbHost" />
                                    </div>

                                    <div class="form-group col-lg-6">
                                        <div>
                                            <label>Port</label>
                                        </div>
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm required" ID="tbPort" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="form-group col-lg-6">
                                        <div class="form-check form-check-inline">
                                            <asp:CheckBox ID="CBcredentials" runat="server" Text="Use Default Credentials" />
                                        </div>
                                        <div class="form-check form-check-inline" style="padding-left: 20px">
                                            <asp:CheckBox ID="CBsenders" runat="server" Text="Mail From Sender" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="card border-default" style="margin-bottom: 10px;">
                            <div class="card-body card-body-sm">
                                <div class="form-row">
                                    <div class="form-group col-sm-6">
                                        <div>
                                            <label>Username</label>
                                        </div>
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm required" ID="tbUsername" />
                                    </div>

                                    <div class="form-group col-lg-6">
                                        <div>
                                            <label>Password</label>
                                        </div>
                                        <asp:TextBox runat="server" type="password" CssClass="form-control form-control-sm required" ID="tbPassword" />
                                    </div>
                                    <div class="form-group col-lg-6">
                                        <div>
                                            <label>Default Mail From</label>
                                        </div>
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm required" ID="tbMailFrom" />
                                    </div>
                                    <div class="form-group col-lg-6">
                                        <div>
                                            <label>Default Mail From Display Name</label>
                                        </div>
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm required" ID="tbDisplay" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="form-group col-lg-6">
                                        <div class="form-check form-check-inline">
                                            <asp:CheckBox ID="CBEnableSSL" runat="server" Text="Enable SSL" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


                    <div class="card border-default" style="margin-bottom: 10px;">
                <div class="card-body">
                    <div class="form-group">
                        <div>
                            <label>Alert Event</label>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="container">
                            <div class="row">
                                <div class="col-sm">
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketOpen" runat="server" Text="Alert event ticket open" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketComment" runat="server" Text="Alert event ticket comment" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketUpdateStatus" runat="server" Text="Alert event ticket updatestatus" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketEscalate" runat="server" Text="Alert event ticket escalate" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketTransfer" runat="server" Text="Alert event ticket transfer" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketOverDue" runat="server" Text="Alert event ticket over due" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketResolve" runat="server" Text="Alert event ticket resolve" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketClose" runat="server" Text="Alert event ticket close" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketCancel" runat="server" Text="Alert event ticket cancel" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBEV_ChangeOrderApproval" runat="server" Text="Alert event change order approval" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBEV_EventToOwner" runat="server" Text="Alert event to owner" />
                                    </div>
                                </div>
                                <div class="col-sm">
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketOpen2Customer" runat="server" Text="Alert event ticket open to Client" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketComment2Customer" runat="server" Text="Alert event ticket comment to Client" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketUpdatestatus2Customer" runat="server" Text="Alert event ticket updatestatus to Client" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketEscalate2Customer" runat="server" Text="Alert event ticket escalate to Client" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketTransfer2Customer" runat="server" Text="Alert event ticket transfer to Client" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketOverDue2Customer" runat="server" Text="Alert event ticket over due to Client" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketResolve2Customer" runat="server" Text="Alert event ticket resolve to Client" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketClose2Customer" runat="server" Text="Alert event ticket close to Client" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketCancel2Customer" runat="server" Text="Alert event ticket cancel to Client" />
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBTicketApproval2Customer" runat="server" Text="Alert event before next maintenance to Owner" />
                                        <%--/Text="Alert event change order approval to Client" ยืมปุ่มไปใช้สำหรับ Next Maintenance /--%>
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="CBEV_RespondCustomer" runat="server" Text="Alert event respond Client" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    </div>
                </div>

            </div>
            
        </div>
</asp:Content>
