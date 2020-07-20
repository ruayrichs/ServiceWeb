<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="MasterAdConfig.aspx.cs" Inherits="ServiceWeb.MasterConfig.MasterAdConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
     <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-AD-config").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
            <div class="pull-left">
                <asp:UpdatePanel runat="server" ID="udpbtn" UpdateMode="Conditional">
                    <ContentTemplate>
                        <button type="button" class="btn btn-success mb-2 AUTH_MODIFY" onclick="$(this).next().click();"> <i class="fa fa-save" >&nbsp;&nbsp;Save</i></button>
                        <asp:Button runat="server" Text="" CssClass="btn btn-success d-none" ID="btn_create" OnClick="Btn_AD_Click" OnClientClick="AGLoading(true);" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </nav>
        <div class="card shadow">
            <div class="card-header">
                <h5 class="mb-0">Active Directory</h5>
            </div>
            
            <div class="card-body panel-body-customer-search PANEL-DEFAULT-BUTTON">
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="card border-default" style="margin-bottom: 10px;">
                            <div class="card-body card-body-sm">
                                <div>
                                    <label class="font-weight-bold" >Config Active Directory</label>
                                </div>
                                <div class="form-row">
                                    <div class="form-group col-sm-6">
                                        <div>
                                            <label>IP</label>
                                        </div>
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm required" ID="AdIP" />
                                    </div>
                                    <div class="form-group col-sm-6">
                                        <div>
                                            <label>Domain</label>
                                        </div>
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm required" ID="ADDomain" />
                                    </div>

                                    <div class="form-group col-lg-6">
                                        <div>
                                            <label>Port</label>
                                        </div>
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm required" ID="ADPort" />
                                    </div>
                                    <div class="form-group col-lg-6">
                                        <div>
                                            <label>BaseDN </label>
                                        </div>
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm required" ID="ADBaseDN" />
                                    </div>
                        </div>
                    </div>
                </div>
            </div>
                </div>
            </div>
            
            <div class="card-body panel-body-customer-search PANEL-DEFAULT-BUTTON">
                <div class="form-row">
                    <div class="col-md-12">
                        <div class="card border-default" style="margin-bottom: 10px;">
                            <div class="card-body card-body-sm">
                            <div >
                                <label class="font-weight-bold">For Check Connect </label>
                            </div>
                                <div class="form-row">
                                     <div class="form-group col-lg-6">
                                        <div>
                                            <label>Username</label>
                                        </div>
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm required" ID="ADUsername" />
                                    </div>
                                    <div class="form-group col-lg-6">
                                        <div>
                                            <label>Password</label>
                                        </div>
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm required" ID="ADPassword" Type="password" />
                                    </div>
                                </div>
                            </div>
                            <div class="card-body card-body-sm">
                                <div class="form-row">
                                    <div form-group col-lg-6>
                                        <asp:UpdatePanel runat="server" ID="CheckConect" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button runat="server" Text="Check Connect" CssClass="btn btn-warning " ID="btnCheck" OnClick="Check_Connect_AD" OnClientClick="AGLoading(true);" />
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
</asp:Content>
