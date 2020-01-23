<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="ServiceContract.aspx.cs" Inherits="ServiceWeb.Master.ServiceContract" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-service-contract").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <style type="text/css">
        #table-coverage > tbody > tr:nth-child(2) > td {
            padding-top: 1.25rem;
        }

        #table-coverage > tbody > tr > td {
            border-top: 0;
        }

            #table-coverage > tbody > tr > td.card-header {
                padding: .75rem .3rem;
            }

            #table-coverage > tbody > tr > td:first-child {
                padding-left: 1.25rem;
            }

            #table-coverage > tbody > tr > td:last-child {
                padding-right: 1.25rem;
            }
    </style>

    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <button type="button" class="btn btn-warning" onclick="backClick(this);"><i class="fa fa-arrow-circle-left"></i>&nbsp;&nbsp;Back</button>
            <button type="button" class="btn btn-success AUTH_MODIFY" onclick="validateSaveClick();"><i class="fa fa-save"></i>&nbsp;&nbsp;Save</button>
        </div>
    </nav>

    <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional" class="d-none">
        <ContentTemplate>
            <asp:Button CssClass="AUTH_MODIFY" ID="btnValidateSave" runat="server" OnClick="btnValidateSave_Click" />
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />

            <asp:Button ID="btnValidateAddItem" runat="server" OnClick="btnValidateSave_Click" />
            <asp:Button ID="btnAddItem" runat="server" CssClass="d-none" OnClick="btnAddItem_Click" />

            <button type="button" id="btnSaveClient" onclick="saveClick(this);"></button>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Service Contract</h5>
                </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="col-lg-6">
                            <div class="form-group">
                                <label>Company</label>
                                <asp:TextBox ID="tbCompany" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Client</label>
                                <asp:TextBox ID="tbCustomer" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                <asp:HiddenField ID="hdfCustomerCode" runat="server" />
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-row">
                                <div class="form-group col-sm-8">
                                    <label>Document Type</label>
                                    <asp:TextBox ID="tbDocumentType" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="form-group col-sm-4">
                                    <label>Document Year</label>
                                    <asp:TextBox ID="tbFiscalYear" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-group col-sm-8">
                                    <label>Document No.</label>
                                    <asp:TextBox ID="tbDocumentNo" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="form-group col-sm-4">
                                    <label>Document Status</label>
                                    <asp:TextBox ID="tbStatus" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-lg-6">
                            <label>Contact Person</label>
                            <asp:TextBox ID="tbContactPerson" placeholder="Text" runat="server" CssClass="form-control form-control-sm" autocomplete="off"></asp:TextBox>
                            <asp:HiddenField ID="hdfContactCode" runat="server" />
                            <asp:HiddenField ID="hdfContactDesc" runat="server" />
                        </div>
                        <div class="required-head form-group col-lg-2 col-sm-4">
                            <label>Document Date</label>
                            <div class="input-group">
                                <asp:TextBox ID="tbDocumentDate" placeholder="dd/mm/yyyy" runat="server" CssClass="form-control form-control-sm date-picker required" autocomplete="off"></asp:TextBox>
                                <div class="input-group-append">
                                    <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="required-head form-group col-lg-2 col-sm-4">
                            <label>Start Date</label>
                            <div class="input-group">
                                <asp:TextBox ID="tbStartDate" placeholder="dd/mm/yyyy" runat="server" CssClass="form-control form-control-sm date-picker required" autocomplete="off"></asp:TextBox>
                                <div class="input-group-append">
                                    <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="required-head form-group col-lg-2 col-sm-4">
                            <label>End Date</label>
                            <div class="input-group">
                                <asp:TextBox ID="tbEndDate" placeholder="dd/mm/yyyy" runat="server" CssClass="form-control form-control-sm date-picker required" autocomplete="off"></asp:TextBox>
                                <div class="input-group-append">
                                    <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label>Description</label>
                        <asp:TextBox ID="tbDescription" placeholder="Text" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                    <div class="card">
                        <div class="card-header">
                            <nav>
                                <div class="nav nav-tabs card-header-tabs" id="nav-tab" role="tablist">
                                    <a class="nav-item nav-link active" id="nav-equipment-tab" data-toggle="tab" href="#nav-equipment" role="tab" aria-controls="nav-equipment" aria-selected="true">Equipment</a>
                                    <a class="nav-item nav-link" id="nav-general-tab" data-toggle="tab" href="#nav-general" role="tab" aria-controls="nav-general" aria-selected="false">General</a>
                                    <a class="nav-item nav-link" id="nav-coverage-tab" data-toggle="tab" href="#nav-coverage" role="tab" aria-controls="nav-coverage" aria-selected="false">Coverage</a>
                                </div>
                            </nav>
                        </div>
                        <div class="card-body">
                            <div class="tab-content" id="nav-tabContent">
                                <div class="tab-pane fade show active" id="nav-equipment" role="tabpanel" aria-labelledby="nav-equipment-tab">
                                    <div class="form-group">
                                        <asp:TextBox ID="tbEquipment" runat="server" CssClass="form-control form-control-sm" autocomplete="off" placeholder="Add Equipment">
                                        </asp:TextBox>
                                        <asp:HiddenField ID="hdfEquipmentNo" runat="server" />
                                        <asp:HiddenField ID="hdfEquipmentDesc" runat="server" />
                                    </div>
                                    <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col">
                                                    <div class="table-responsive">
                                                        <table id="table-items" class="table table-bordered table-striped table-hover table-sm dataTable" style="margin-top: 0 !important; margin-bottom: 0 !important;">
                                                            <thead>
                                                                <tr>
                                                                    <th class="text-nowrap"></th>
                                                                    <th>Configuration Item</th>
                                                                    <th class="text-nowrap">Start Date</th>
                                                                    <th class="text-nowrap">End Date</th>
                                                                    <th class="text-nowrap">Item Status</th>
                                                                    <th class="text-nowrap">Active</th>
                                                                    <th class="text-nowrap">Hold</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:Repeater ID="rptItems" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td class="text-nowrap text-center align-middle">
                                                                                <i class="fa fa-times-circle fa-lg text-danger" title="Delete" onclick="deleteItemClick(this, '<%# Eval("EquipmentNo") %>', '<%# Eval("EquipmentDesc") %>');"></i>
                                                                                <asp:Button ID="tbDeleteItem" runat="server" CssClass="d-none"
                                                                                    CommandArgument='<%# Eval("EquipmentNo") %>'
                                                                                    OnClick="tbDeleteItem_Click" />
                                                                            </td>
                                                                            <td class="align-middle" data-column="equipment">
                                                                                <%# string.IsNullOrEmpty(Eval("EquipmentDesc").ToString()) ? Eval("EquipmentNo") : (Eval("EquipmentNo") + " : " + Eval("EquipmentDesc")) %>
                                                                                <asp:HiddenField ID="hdfEquipmentNo" runat="server" Value='<%# Eval("EquipmentNo") %>' />
                                                                                <asp:HiddenField ID="hdfEquipmentDesc" runat="server" Value='<%# Eval("EquipmentDesc") %>' />
                                                                            </td>
                                                                            <td class="text-nowrap align-middle">
                                                                                <%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("StartDate").ToString()) %>
                                                                            </td>
                                                                            <td class="text-nowrap align-middle">
                                                                                <%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("EndDate").ToString()) %>
                                                                            </td>
                                                                            <td class="text-nowrap align-middle">
                                                                                <%# Eval("Xstatus") %>
                                                                            </td>
                                                                            <td class="text-nowrap text-center align-middle">
                                                                                <%# Eval("Astatus").ToString() == "A" ? "<i class='fa fa-check-square-o'></i>" : "<i class='fa fa-square-o'></i>" %>
                                                                            </td>
                                                                            <td class="text-nowrap text-center align-middle">
                                                                                <%# Eval("Hstatus").ToString() != "U" ? "<i class='fa fa-check-square-o'></i>" : "<i class='fa fa-square-o'></i>" %>                                                                               
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <%--<FooterTemplate>
                                                                        <tr class="table-warning">
                                                                            <td colspan="7" class="text-center align-middle c-pointer" onclick="">
                                                                                <b><i class="fa fa-plus-circle"></i>&nbsp;Add new item</b>
                                                                            </td>
                                                                        </tr>
                                                                    </FooterTemplate>--%>
                                                                </asp:Repeater>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="tab-pane fade" id="nav-general" role="tabpanel" aria-labelledby="nav-general-tab">
                                    <div class="form-row">
                                        <div class="col-lg-6 col-md-12">
                                            <div class="form-row">
                                                <div class="col-sm-8">
                                                    <div class="form-group">
                                                        <label>Owner</label>
                                                        <asp:TextBox ID="tbOwner" placeholder="Text" runat="server" CssClass="form-control form-control-sm" autocomplete="off"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfOwnerCode" runat="server" />
                                                        <asp:HiddenField ID="hdfOwnerDesc" runat="server" />
                                                    </div>
                                                    <div class="form-group mb-lg-0">
                                                        <label>Template</label>
                                                        <asp:TextBox ID="tbTemplate"  placeholder="Text" runat="server" CssClass="form-control form-control-sm" autocomplete="off"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfTemplateCode" runat="server" />
                                                        <asp:HiddenField ID="hdfTemplateDesc" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-sm-4">
                                                    <div class="form-group">
                                                        <label>Response Time</label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="tbResponseTime" runat="server" CssClass="form-control form-control-sm" TextMode="Number"></asp:TextBox>
                                                            <div class="input-group-append">
                                                                <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Day(s)</button>
                                                                <div class="dropdown-menu">
                                                                    <a class="dropdown-item">Day(s)</a>
                                                                    <a class="dropdown-item">Hour(s)</a>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group mb-lg-0">
                                                        <label>Resolution Time</label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="tbResolutionTime" runat="server" CssClass="form-control form-control-sm" TextMode="Number"></asp:TextBox>
                                                            <div class="input-group-append">
                                                                <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Day(s)</button>
                                                                <div class="dropdown-menu">
                                                                    <a class="dropdown-item">Day(s)</a>
                                                                    <a class="dropdown-item">Hour(s)</a>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-md-12">
                                            <div class="form-group mb-0">
                                                <label>Remark</label>
                                                <asp:TextBox ID="tbRemark"  placeholder="Text" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Style="height: 107px; resize: none;"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane fade" id="nav-coverage" role="tabpanel" aria-labelledby="nav-coverage-tab">
                                    <div class="form-row">
                                        <div class="form-group col-lg-8 col-md-9 mb-md-0">
                                            <div class="card border-primary">
                                                <div class="card-body p-0" style="padding-bottom: 1.25rem !important;">
                                                    <div class="table-responsive">
                                                        <table id="table-coverage" class="table table-sm mb-0">
                                                            <tbody>
                                                                <tr>
                                                                    <td class="card-header"></td>
                                                                    <td class="card-header">
                                                                        <h6 class="mb-0">Start Time</h6>
                                                                    </td>
                                                                    <td class="card-header">
                                                                        <h6 class="mb-0">End Time</h6>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkMon" runat="server" CssClass="form-check" Text="Monday" />
                                                                    </td>
                                                                    <td style="min-width: 120px;">
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tbMonStartTime" runat="server" CssClass="form-control form-control-sm time-picker"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                    <td style="min-width: 120px;">
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tbMonEndTime" runat="server" CssClass="form-control form-control-sm time-picker"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkTue" runat="server" CssClass="form-check" Text="Tuesday" />
                                                                    </td>
                                                                    <td>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tbTueStartTime" runat="server" CssClass="form-control form-control-sm time-picker"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tbTueEndTime" runat="server" CssClass="form-control form-control-sm time-picker"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkWed" runat="server" CssClass="form-check" Text="Wednesday" />
                                                                    </td>
                                                                    <td>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tbWedStartTime" runat="server" CssClass="form-control form-control-sm time-picker"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tbWedEndTime" runat="server" CssClass="form-control form-control-sm time-picker"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkThu" runat="server" CssClass="form-check" Text="Thursday" />
                                                                    </td>
                                                                    <td>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tbThuStartTime" runat="server" CssClass="form-control form-control-sm time-picker"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tbThuEndTime" runat="server" CssClass="form-control form-control-sm time-picker"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkFri" runat="server" CssClass="form-check" Text="Friday" />
                                                                    </td>
                                                                    <td>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tbFriStartTime" runat="server" CssClass="form-control form-control-sm time-picker"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tbFriEndTime" runat="server" CssClass="form-control form-control-sm time-picker"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkSat" runat="server" CssClass="form-check" Text="Saturday" />
                                                                    </td>
                                                                    <td>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tbSatStartTime" runat="server" CssClass="form-control form-control-sm time-picker"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tbSatEndTime" runat="server" CssClass="form-control form-control-sm time-picker"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkSun" runat="server" CssClass="form-check" Text="Sunday" />
                                                                    </td>
                                                                    <td>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tbSunStartTime" runat="server" CssClass="form-control form-control-sm time-picker"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div class="input-group">
                                                                            <asp:TextBox ID="tbSunEndTime" runat="server" CssClass="form-control form-control-sm time-picker"></asp:TextBox>
                                                                            <div class="input-group-append">
                                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group col-lg-4 col-md-3 mb-0">
                                            <div class="card border-info">
                                                <div class="card-header">
                                                    <h6 class="mb-0">Include</h6>
                                                </div>
                                                <div class="card-body">
                                                    <asp:CheckBox ID="chkParts" runat="server" CssClass="form-check" Text="Parts" />
                                                    <asp:CheckBox ID="chkLabor" runat="server" CssClass="form-check" Text="Labor" />
                                                    <asp:CheckBox ID="chkTravel" runat="server" CssClass="form-check" Text="Travel" />
                                                    <asp:CheckBox ID="chkHolidays" runat="server" CssClass="form-check" Text="Holidays" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var equipmentDB;
        var tbEquipment = $("#<%= tbEquipment.ClientID %>");

        var contactDB;
        var tbContact = $("#<%= tbContactPerson.ClientID %>");
        var customerCode = "<%= CustomerCode %>";

        var ownerDB;
        var tbOwner = $("#<%= tbOwner.ClientID %>");

        var templateDB;
        var tbTemplate = $("#<%= tbTemplate.ClientID %>");

        $(document).ready(function () {
            var postEquipment = {
                actionCase: "equipment"
            };

            $.ajax({
                type: "POST",
                url: servictWebDomainName + "API/AutoCompleteAPI.aspx",
                data: postEquipment,
                success: function (data) {
                    equipmentDB = new JQL(data);
                    bindAutoComplete(tbEquipment, equipmentDB, afterSelectedEquipment);
                }
            });

            var postContact = {
                actionCase: "contact",
                customer: customerCode
            };

            $.ajax({
                type: "POST",
                url: servictWebDomainName + "API/AutoCompleteAPI.aspx",
                data: postContact,
                success: function (data) {
                    contactDB = new JQL(data);
                    bindAutoComplete(tbContact, contactDB);
                }
            });

            var postOwner = {
                actionCase: "employee"
            };

            $.ajax({
                type: "POST",
                url: servictWebDomainName + "API/AutoCompleteAPI.aspx",
                data: postOwner,
                success: function (data) {
                    ownerDB = new JQL(data);
                    bindAutoComplete(tbOwner, ownerDB);
                }
            });

            var postTemplate = {
                actionCase: "contractTemplate"
            };

            $.ajax({
                type: "POST",
                url: servictWebDomainName + "API/AutoCompleteAPI.aspx",
                data: postTemplate,
                success: function (data) {
                    templateDB = new JQL(data);
                    bindAutoComplete(tbTemplate, templateDB);
                }
            });
        });

        function afterSelectedEquipment() {
            var code = $(tbEquipment).parent().next().val();
            var display = $(tbEquipment).parent().next().next().val();
            var err = false;
            $("#table-items tbody tr td[data-column='equipment']").each(function () {
                var equipmentNo = $(this).find("input").first().val();
                if (equipmentNo == code) {
                    err = true;
                }
            });

            if (err) {
                AGError("มี Configuration Item [" + display + "] อยู่ในรายการแล้ว<br/>กรุณาเลือก Configuration Item อื่น");
                $(tbEquipment).parent().next().val("");
                $(tbEquipment).parent().next().next().val("");
            }
            else {
                validateAddItemClick();
            }

            $(tbEquipment).typeahead('val', "");
        }

        function afterSelectedTemplate() {

        }

        function bindAutoComplete(sender, db, afterSelected) {
            $(sender).typeahead({
                hint: true,
                highlight: true,
                minLength: 0
            }, {
                limit: 20,
                templates: {
                    empty: ' ',
                    suggestion: function (data) {
                        return '<div>' + data.display + '</div>';
                    }
                },
                source: function (str, callback) {
                    var selectCode = db.select('*').where('code').match(str).fetch();
                    var selectName = db.select('*').where('desc').match(str).fetch();
                    var selectDisplay = db.select('*').where('display').match(str).fetch();
                    var selectResult = [];

                    if (selectCode.length > 0) {
                        for (var i = 0; i < selectCode.length; i++) {
                            selectResult.push(selectCode[i]);
                        }
                    }

                    if (selectName.length > 0) {
                        for (var i = 0; i < selectName.length; i++) {
                            var hasCode = jQuery.grep(selectResult, function (element) {
                                return element.code == selectName[i].code;
                            });

                            if (hasCode.length == 0) {
                                selectResult.push(selectName[i]);
                            }
                        }
                    }

                    if (selectDisplay.length > 0) {
                        for (var i = 0; i < selectDisplay.length; i++) {
                            var hasCode = jQuery.grep(selectResult, function (element) {
                                return element.code == selectDisplay[i].code;
                            });

                            if (hasCode.length == 0) {
                                selectResult.push(selectDisplay[i]);
                            }
                        }
                    }

                    callback(selectResult);
                },
                display: function (data) {
                    return data.display;
                }
            });

            $(sender).bind('typeahead:select typeahead:autocomplete', function (e, v) {
                $(sender).parent().next().val(v.code);
                $(sender).parent().next().next().val(v.desc);

                console.log($(sender).typeahead('val'));

                if (afterSelected != null && afterSelected != undefined && afterSelected != "") {
                    afterSelected();
                }
            });
        }

        function activeRequireFieldHeader() {
            $(".required-head ").find(".required").prop('required', true);
        }

        function inactiveRequireFieldHeader() {
            $(".required-head ").find(".required").prop('required', false);
        }

        function backClick(sender) {
            if (AGConfirm(sender, "ต้องการกลับไปยังหน้าแรกของ Service Contract ใช่หรือไม่ ?")) {
                window.location.href = "ServiceContractCriteria.aspx";
            }
        }

        function validateSaveClick() {
            activeRequireFieldHeader();
            $("#<%= btnValidateSave.ClientID %>").click();
        }

        function saveClick(sender) {
            if (AGConfirm(sender, "ยืนยันการบันทึก")) {
                AGLoading(true);
                $("#<%= btnSave.ClientID %>").click();
            }
        }

        function validateAddItemClick() {
            activeRequireFieldHeader();
            $("#<%= btnValidateAddItem.ClientID %>").click();
        }

        function addNewItem() {
            AGLoading(true);
            $("#<%= btnAddItem.ClientID %>").click();
        }

        function deleteItemClick(sender, code, desc) {
            desc = desc == "" ? code : code + " : " + desc;
            if (AGConfirm(sender, "ต้องการลบ Configuration Item [" + desc + "] ใช่หรือไม่ ?")) {
                inactiveRequireFieldHeader();
                $(sender).next().click();
                AGLoading(true);
            }
        }
    </script>

</asp:Content>
