<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="Version.aspx.cs" Inherits="ServiceWeb.MasterConfig.Version" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">

    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-version").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel runat="server" ID="udpbtn2" UpdateMode="Conditional">
                <ContentTemplate>
                    <button type="button" class="btn btn-success mb-2 AUTH_MODIFY" onclick="$(this).next().click();"> <i class="fa fa-save" >&nbsp;&nbsp;Save</i></button>
                    <asp:Button runat="server" Text="" CssClass="btn btn-success d-none" ID="btn_create" OnClick="btn_update_Click" OnClientClick="AGLoading(true);"/>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>
    <div class="card shadow">
        <div class="card-header">
            <h5 class="mb-0">About Management</h5>
        </div>
        <div class="card-body panel-body-customer-search PANEL-DEFAULT-BUTTON">

            <%--==++ Product Specification ++==--%>    
            <div class="form-row">
                <div class="col-md-12">
                    <div class="card border-default" style="margin-bottom: 10px;">
                        <div class="card-body card-body-sm">
                            <legend class="legend-defult">Product Specification</legend>
                            <div class="form-row">
                                <div class="form-group col-sm-6">
                                    <div>
                                        <label>Release</label>
                                    </div>
                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtReleres" />
                                </div>

                                <div class="form-group col-lg-6">
                                    <div>
                                        <label>Version</label>
                                    </div>
                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtVersion" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-group col-sm-6">
                                    <div>
                                        <label>This product is licensed to</label>
                                    </div>
                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtLicensed" />
                                </div>

                                <div class="form-group col-lg-6">
                                    <div>
                                        <label>SITE ID</label>
                                    </div>
                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtSiteID" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-group col-sm-12">
                                    <div>
                                        <label>Product Specification Comment</label>
                                    </div>
                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtBoxProductSpecificationRemark" TextMode="multiline" Rows="5" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--==++ Techinical Support ++==--%>
            <div class="form-row">
                <div class="col-md-12">
                    <div class="card border-default" style="margin-bottom: 10px;">
                        <div class="card-body card-body-sm">
                            <legend class="legend-defult">Techinical Support</legend>

                            <div class="form-row">
                                <div class="form-group col-sm-12">
                                    <div>
                                        <label>Techinical Support Remark Contact</label>
                                    </div>
                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtBoxContactTechinicalSupport" TextMode="multiline" Rows="5" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-group col-sm-12">
                                    <div>
                                        <label>Techinical Support Remark Phone</label>
                                    </div>
                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtBoxTelephoneTechinicalSupport" TextMode="multiline" Rows="5" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--==++ Third Praty Notices ++==--%>
            <div class="form-row">
                <div class="col-md-12">
                    <div class="card border-default" style="margin-bottom: 10px;">
                        <div class="card-body card-body-sm">
                            <legend class="legend-defult">Third Praty Notices</legend>
                            <div class="form-row">
                                <div class="form-group col-sm-12">
                                    <div>
                                        <label>Third Praty Notices Comment</label>
                                    </div>
                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtBoxThirdPratyNoticesRemark" TextMode="multiline" Rows="5" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--==++ System Information ++==--%>
            <div class="form-row d-none">
                <div class="col-md-12">
                    <div class="card border-default" style="margin-bottom: 10px;">
                        <div class="card-body card-body-sm">
                            <legend class="legend-defult">System Information</legend>
                            <div class="form-row">
                                <div class="form-group col-sm-12">
                                    <div>
                                        <label>System Information Comment</label>
                                    </div>
                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtBoxSystemInformationRemark" TextMode="multiline" Rows="5" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>


    <%--<asp:Label Text="Version" runat="server" />
    <input type="type" name="name" value="" />
    <asp:Label Text="Release" runat="server" />
    <input type="type" name="name" value="" />



    <div class="footer">

        license

    </div>

    <fieldset class="fieldset-defult">
                                            <legend class="legend-defult">General Box
                                            </legend>
                                            <div style="padding: 0px 8px;">
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Weight</label>
                                                        <div class="form-row">
                                                            <div class="col-xs-12 col-sm-6">
                                                                <asp:TextBox runat="server" placeholder="Number" CssClass="form-control form-control-sm" ID="txtGeneralBox_Weight" />
                                                            </div>
                                                            <div class="col-xs-12 col-sm-6">
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlGeneralBox_Weight_WeightUnit">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Size/Dimension</label>
                                                        <asp:TextBox runat="server" placeholder="Number" CssClass="form-control form-control-sm" ID="txtGeneralBox_Size_Dimension" />
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Material No</label>
                                                        <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtGeneralBox_MaterialNo" />
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Start-up Date</label>
                                                        <div class="input-group">
                                                            <asp:TextBox runat="server" CssClass="form-control form-control-smdate-picker" ID="txtGeneralBox_Start_UpDate" />
                                                            <span class="input-group-append hand">
                                                                <i class="input-group-text fa fa-calendar"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Active By</label>
                                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" Enabled="false" ID="txtGeneralBox_ActiveBy" />
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Active Time</label>
                                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" Enabled="false" ID="txtGeneralBox_ActiveTime" />
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Active Date</label>
                                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" Enabled="false" ID="txtGeneralBox_ActiveDate" />
                                                    </div>
                                                </div>
                                            </div>
                                        </fieldset>--%>
    
</asp:Content>
