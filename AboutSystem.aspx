<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="AboutSystem.aspx.cs" Inherits="ServiceWeb.AboutSystem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

    <div class="card shadow">
        <div class="card-header">
            <h5 class="mb-0">About</h5>
        </div>
        <div class="card-body panel-body-customer-search PANEL-DEFAULT-BUTTON">

            <%--==++ Product Specification ++==--%>    
            <div class="form-row">
                <div class="col-md-12">
                    <div  style="margin-bottom: 10px;">
                        <div class="card-body card-body-sm">
                            <legend class="legend-defult">Product Specification</legend>
                            <div class="row">
                                <div class="col-sm-12 col-md-5 col-lg-4 col-xl-3">
                                    <b>Release</b>
                                </div>
                                <div class="form-group col-sm-12 col-md-7 col-lg-8 col-xl-9">
                                    <asp:Label ID="txReleres" runat="server" />
                                </div>

                                <div class="col-sm-12 col-md-5 col-lg-4 col-xl-3">
                                    <b>Version</b>
                                </div>
                                <div class="form-group col-sm-12 col-md-7 col-lg-8 col-xl-9">
                                    <asp:Label ID="txVersion" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-5 col-lg-4 col-xl-3">
                                     <b>This product is licensed to</b>
                                </div>
                                <div class="form-group col-sm-12 col-md-7 col-lg-8 col-xl-9">
                                    <asp:Label ID="txLicensed" runat="server" />
                                </div>

                                <div class="col-sm-12 col-md-5 col-lg-4 col-xl-3">
                                     <b>SITE ID</b>
                                </div>
                                <div class="form-group col-sm-12 col-md-7 col-lg-8 col-xl-9">
                                    <asp:Label ID="txSiteID" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-5 col-lg-4 col-xl-3">
                                    <b>Product Specification Remark</b>
                                </div>
                                <div class="form-group col-sm-12 col-md-7 col-lg-8 col-xl-9">
                                    <asp:Label ID="txBoxProductSpecificationRemark" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <hr />
            <%--==++ Techinical Support ++==--%>
            <div class="form-row">
                <div class="col-md-12">
                    <div  style="margin-bottom: 10px;">
                        <div class="card-body card-body-sm">
                            <legend class="legend-defult">Techinical Support</legend>

                            <div class="row">
                                <div class="col-sm-12 col-md-5 col-lg-4 col-xl-3">
                                    <b>Techinical Support Contact</b>
                                </div>
                                <div class="form-group col-sm-12 col-md-7 col-lg-8 col-xl-9">
                                    <asp:Label ID="txBoxContactTechinicalSupport" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-5 col-lg-4 col-xl-3">
                                    <b>Techinical Support Phone</b>
                                </div>
                                <div class="form-group col-sm-12 col-md-7 col-lg-8 col-xl-9">
                                    <asp:Label ID="txBoxTelephoneTechinicalSupport" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <hr />
            <%--==++ Third Praty Notices ++==--%>
            <div class="form-row">
                <div class="col-md-12">
                    <div  style="margin-bottom: 10px;">
                        <div class="card-body card-body-sm">
                            <legend class="legend-defult">Third Praty Notices</legend>
                            <div class="form-row">
                                <div class="col-sm-12 col-md-5 col-lg-4 col-xl-3">
                                    <b>Third Praty Notices Remark</b>
                                </div>
                                <div class="form-group col-sm-12 col-md-7 col-lg-8 col-xl-9">
                                    <asp:Label ID="txBoxThirdPratyNoticesRemark" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--==++ System Information ++==--%>
            <div class="form-row d-none">
                <div class="col-md-12">
                    <div  style="margin-bottom: 10px;">
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
</asp:Content>
