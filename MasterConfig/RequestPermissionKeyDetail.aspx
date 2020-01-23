<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="PageDetail.aspx.cs" Inherits="ServiceWeb.MasterConfig.PageDetail" %>

<%@ Register Src="~/widget/usercontrol/AutoCompleteControl.ascx" TagPrefix="uc1" TagName="AutoCompleteControl" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEmployee.ascx" TagPrefix="uc1" TagName="AutoCompleteEmployee" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-request-permission-key").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <div>
        <div class="card mb-4 shadow-sm">
            <div class="card-header">
                <h4 class="my-0 font-weight-normal">Request Permission Key Detail</h4>
            </div>
            <div class="card-body">
                <div>
                    <asp:UpdatePanel ID="udpRPKD" runat="server">
                        <ContentTemplate>
                            <!-- Hidden Form -->
                            <asp:HiddenField runat="server" ID="mode_inp" Value="" />
                            <!-- Hidden Form -->
                            <div class="form-row">
                                <div class="form-group col-sm-6">
                                    <div>
                                        <label>IP Address</label>
                                    </div>
                                    <div class="form-row form-row-sm">
                                        <div class="col">
                                            <asp:TextBox runat="server" CssClass="form-control form-control-sm required" ID="ip_addr_inp1" OnTextChanged="IPAddress_OnChange" AutoPostBack="true" />
                                        </div>
                                        &nbsp;
                                        <div class="col">
                                            <asp:TextBox runat="server" CssClass="form-control form-control-sm required" ID="ip_addr_inp2" OnTextChanged="IPAddress_OnChange" AutoPostBack="true" />
                                        </div>
                                        &nbsp;
                                        <div class="col">
                                            <asp:TextBox runat="server" CssClass="form-control form-control-sm required" ID="ip_addr_inp3" OnTextChanged="IPAddress_OnChange" AutoPostBack="true" />
                                        </div>
                                        &nbsp;
                                        <div class="col">
                                            <asp:TextBox runat="server" CssClass="form-control form-control-sm required" ID="ip_addr_inp4" OnTextChanged="IPAddress_OnChange" AutoPostBack="true" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-sm-6">
                                    <div>
                                        <label>Program Name</label>
                                    </div>
                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm required" ID="program_name_inp" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-group col-lg-12">
                                    <div>
                                        <label>Permission Key</label>
                                    </div>
                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm required" Enabled="false" ID="pkey_inp" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-group col-lg-6">
                                    <div>
                                        <label>Start Date</label>
                                    </div>
                                    <div class="input-group">
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker required" ID="start_date_inp" />
                                        <div class="input-group-append">
                                            <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-lg-6">
                                    <div>
                                        <label>End Date</label>
                                    </div>
                                    <div class="input-group">
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker required" ID="end_date_inp" />
                                        <div class="input-group-append">
                                            <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-group col-lg-6">
                                    <div>
                                        <label>Channel Request</label>
                                    </div>
                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm required" ID="channel_request_inp">
                                        <asp:ListItem Value="1" Text="E-Mail" />
                                        <asp:ListItem Value="2" Text="Web" />
                                        <asp:ListItem Value="3" Text="System" />
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group col-lg-6">
                                    <div>
                                        <label>Status</label>
                                    </div>
                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm required" ID="status_inp">
                                        <asp:ListItem Value="true" Text="Active" />
                                        <asp:ListItem Value="false" Text="Inactive" />
                                    </asp:DropDownList>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="form-row">
                    <div class="form-group col-lg-6">
                        <div class="form-group">
                            <label for="sel2">Employee</label>
                            <uc1:AutoCompleteEmployee runat="server" id="AutoCompleteEmployee" CssClass="form-control form-control-sm required ticket-allow-editor" />
                        </div>
                    </div>
                        </div>
                    <asp:UpdatePanel ID="udpRemark" runat="server">
                        <ContentTemplate>
                            <div class="form-row">
                                <div class="form-group col-lg-12">
                                    <div>
                                        <label>Remark</label>
                                    </div>
                                    <asp:TextBox TextMode="multiline" Columns="50" Rows="5" runat="server" CssClass="form-control form-control-sm" ID="remark_inp" />
                                </div>
                            </div>
                            <div class="form-row" style="padding-top: 12px;">
                                <div class="form-group col-lg-12">
                                    <asp:Button runat="server" Text="Save" CssClass="btn btn-info" ID="btn_save" OnClick="btn_save_Click" OnClientClick="AGLoading(true);" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
