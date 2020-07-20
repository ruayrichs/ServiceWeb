<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/ServiceTicketMasterPage.master" AutoEventWireup="true" CodeBehind="EquipmentDetail.aspx.cs" Inherits="ServiceWeb.crm.Master.Equipment.EquipmentDetail" %>

<%@ Register Src="~/widget/usercontrol/AttachFileUserControl.ascx" TagName="AttachFileUserControl" TagPrefix="sna" %>
<%--<%@ Register Src="~/UserControl/AGapeGallery/UploadGallery/UploadGallery.ascx" TagPrefix="uc1" TagName="UploadGallery" %>--%>
<%@ Register Src="~/widget/usercontrol/TimeLineControl.ascx" TagPrefix="sna" TagName="TimeLineControl" %>
<%@ Register Src="~/widget/usercontrol/AutoCompleteControl.ascx" TagPrefix="sna" TagName="AutoCompleteControl" %>
<%@ Register Src="~/LinkFlowChart/FlowChartDiagramRelationControl.ascx" TagPrefix="sna" TagName="FlowChartDiagramRelationControl" %>
<%@ Register Src="~/UserControl/ChangeLogControl.ascx" TagPrefix="sna" TagName="ChangeLogControl" %>
<%@ Register Src="~/UserControl/SmartPaging.ascx" TagPrefix="sna" TagName="SmartPaging" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-configuration-item").className = "nav-link active";
        };

        //$(document).ready(function () { webOnLoad(); }) 
        $(document).ready(function () {
            upDateCheckBoxDaySend();
            //setValueID();


            // QR
            var trial = $("#SizeQRSmall").attr('src');
            $("#SizeQRBig").attr("src", trial);

            $("#SizeQRSmall").click(function () {
                $("#SizeQRSmall").hide(500);
                $("#SizeQRBig").show(500);
            });
            $("#SizeQRBig").click(function () {
                $("#SizeQRBig").hide(500);
                $("#SizeQRSmall").show(500);
            });
            // \QR
        });
        function upDateCheckBoxDaySend() {            
            $('#<%= CheckBoxDaySend.ClientID%>').addClass("custom-control-input");
        }
        function setValueID() {
            //$('#IncludeLibraryPlaceHolder_ContentPlaceHolder1_txtdata_3').val($('#IncludeLibraryPlaceHolder_ContentPlaceHolder1_rptAttributes_txtdata_3').val());
            //$('#IncludeLibraryPlaceHolder_ContentPlaceHolder1_rptAttributes_xDisplay_3').addClass("d-none");

            //$('#IncludeLibraryPlaceHolder_ContentPlaceHolder1_txtdata_4').val($('#IncludeLibraryPlaceHolder_ContentPlaceHolder1_rptAttributes_txtdata_4').val());
            //$('#IncludeLibraryPlaceHolder_ContentPlaceHolder1_rptAttributes_xDisplay_4').addClass("d-none");
        }
        function sendValueID(val, addID) {
            //if (addID == 'IncludeLibraryPlaceHolder_ContentPlaceHolder1_rptAttributes_txtdata_3') {
            //    $('#IncludeLibraryPlaceHolder_ContentPlaceHolder1_rptAttributes_txtdata_3').val(val);
            //}

            //if (addID == 'IncludeLibraryPlaceHolder_ContentPlaceHolder1_rptAttributes_txtdata_4') {
            //    $('#IncludeLibraryPlaceHolder_ContentPlaceHolder1_rptAttributes_txtdata_4').val(val);
            //}
        }
    </script>
    <style>
        .hide {
            display: none !important;
        }
        .pagination {
            margin: auto;
            width: 40%;
            padding: 10px 0px;
        }

            .pagination > li > a, .pagination > li > span {
                position: relative;
                float: left;
                padding: 6px 12px;
                margin-left: -1px;
                line-height: 1.42857143;
                color: #009688 !important;
                text-decoration: none;
                background-color: #fff;
                border: 1px solid #ddd;
            }

            .pagination > .active > a, .pagination > .active > a:focus, .pagination > .active > a:hover, .pagination > .active > span, .pagination > .active > span:focus, .pagination > .active > span:hover {
                background-color: #009688;
                border-color: #009688;
            }

            .pagination > .active > a {
                z-index: 3;
                color: #fff !important;
                cursor: default;
            }
    </style>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px; top: 0px;">
        <div class="pull-left">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <%--<a class="btn btn-warning btn-sm mb-1" href="EquipmentCriteria.aspx"><i class="fa fa-arrow-circle-left"></i>&nbsp;&nbsp;Back</a>--%>
                    <button type="button" runat="server" class="btn btn-success btn-sm mb-1" onclick="$(this).next().click();">
                        <i class="fa fa-bar-chart"></i>&nbsp;&nbsp;Relation
                    </button>
                    <asp:Button Text="Diagram" runat="server" ID="btnOpenDiagram" CssClass="btn btn-success d-none"
                        OnClientClick="return showDiagram();" />
                    <button type="button" runat="server" class="btn btn-primary btn-sm mb-1" onclick="$(this).next().click();">
                        <i class="fa fa-save"></i>&nbsp;&nbsp;Save
                    </button>
                    <asp:Button Text="Save" runat="server" CssClass="btn btn-primary AUTH_UPDATE d-none" ID="btnSaveEquipment"
                        OnClick="btnSaveEquipment_Click" OnClientClick="AGLoading(true);" />

                    <asp:Button Text="" runat="server" CssClass="d-none" ID="btnReloadLog" ClientIDMode="Static"
                        OnClick="btnReloadLog_Click" OnClientClick="AGLoading(true);" />
                    
                    <asp:Button Text="" runat="server" CssClass="d-none" ID="btnLoadBindingOwnerAssignment" ClientIDMode="Static"
                        OnClick="btnLoadBindingOwnerAssignment_Click" OnClientClick="AGLoading(true);" />
                    <asp:Button Text="" runat="server" CssClass="d-none" ID="btnLoadBindingTicket" ClientIDMode="Static"
                        OnClick="btnLoadBindingTicket_Click" OnClientClick="AGLoading(true);" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>
    <div class="card shadow">
        <div class="card-header">
            <h5 class="mb-0">
                Configuration Item Detail
                <img class="float-right" id="SizeQRBig" style="height : 200px; Margin:-12px 2px -12px; display: none;" alt="QR code">
                <img class="float-right" id="SizeQRSmall" style="height : 48px; Margin:-12px 2px -12px;" src="https://chart.googleapis.com/chart?chs=200x200&amp;cht=qr&amp;chl=<%= qrcodeurl %>" alt="QR code">
                <%--<img class="float-right" id="SizeQRSmall" style="height : 48px; Margin:-12px 2px -12px;" src="https://chart.googleapis.com/chart?chs=200x200&amp;cht=qr&amp;chl=<%= txtEquipmentCode.Text %>" alt="QR code"> --%>
            </h5>
        </div>
        <div class="card-body">
            <style>
                .panel-detail-body {
                    border: 1px solid #ddd;
                    border-radius: 4px;
                    padding: 10px;
                    margin-top: -2px;
                }

                /*.panel-detail-equipment {
					display: none;
				}*/

                .fieldset-defult {
                    display: block;
                    margin-bottom: 10px;
                    -webkit-margin-start: 2px;
                    -webkit-margin-end: 2px;
                    -webkit-padding-before: 0.35em;
                    -webkit-padding-start: 0.75em;
                    -webkit-padding-end: 0.75em;
                    -webkit-padding-after: 0.625em;
                    min-width: -webkit-min-content;
                    border-width: 1px;
                    border-style: solid;
                    border-color: #ccc;
                    border-image: initial;
                    color: #000;
                }

                    .fieldset-defult .legend-defult {
                        display: block;
                        width: auto;
                        margin-bottom: 5px;
                        -webkit-padding-start: 6px;
                        -webkit-padding-end: 6px;
                        border-width: initial;
                        border-style: none;
                        border-color: initial;
                        border-image: initial;
                    }

                legend {
                    display: block;
                    width: 100%;
                    padding: 0;
                    margin-bottom: 20px;
                    font-size: 16px;
                    line-height: inherit;
                    color: #333;
                    border: 0;
                    border-bottom: 1px solid #e5e5e5;
                }

                .btn-group.bootstrap-select {
                    height: 25px !important;
                    box-shadow: none !important;
                    border: none !important;
                    border-radius: 0px !important;
                }

                .btn.dropdown-toggle {
                    height: 25px !important;
                    padding: 0px 4px !important;
                    box-shadow: none !important;
                    border: none !important;
                    border-radius: 0px !important;
                    font-weight: normal !important;
                    font-size: inherit !important;
                }

                .text-dark {
                    color: #343a40 !important;
                }
            </style>

            <%--<div class="mat-box">
            </div>
            <br />--%>
            <div class="form-row">
                <div class="col-xs-12 col-sm-4 col-md-3 col-lg-2">
                    <div class="panel-detail-body">
                        <ul class="nav flex-column nav-pills" role="tablist" aria-orientation="vertical">
                            <li class="nav-item">
                                <a class="nav-link active" href="#panelHeader" role="tab" data-toggle="tab">Header</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="#panelAttributes" role="tab" data-toggle="tab">Attributes</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="#panelGeneral" role="tab" data-toggle="tab">General</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="#panelLocation" role="tab" data-toggle="tab">Location</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="#panelOganization" role="tab" data-toggle="tab">Oganization</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="#panelOwnerAssignment" role="tab" data-toggle="tab" onclick="bindingOwnerAssignment();" >Owner Assignment</a>
                            </li>
                            <li class="nav-item hide">
                                <a class="nav-link" href="#panelSale" role="tab" data-toggle="tab">Sale</a>
                            </li>
                            <li class="nav-item hide">
                                <a class="nav-link" href="#panelSerialData" role="tab" data-toggle="tab">Serial Data</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="#panelWarranty" role="tab" data-toggle="tab">Warranty</a>
                            </li>
                            <%--<li class="nav-item">
                                <a class="nav-link" href="#panelAdditionalDate" role="tab" data-toggle="tab">Additional Date</a>
                            </li>--%>
                            <li class="nav-item">
                                <a class="nav-link" href="#panelPicture" role="tab" data-toggle="tab">File Attachment</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="#panelRelation" role="tab" data-toggle="tab" onclick="AdjustWidthPanelTreeWhenOpenTab();">Relation</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="#panelHistory" role="tab" data-toggle="tab">History</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="#panelChangeLog" role="tab" data-toggle="tab">Change Log</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="#panelTicket" role="tab" data-toggle="tab" onclick="bindingTicket();">Ticket</a>
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-8 col-md-9 col-lg-10">
                    <div class="panel-detail-body">
                        <div class="tab-content">
                            <div id="panelHeader" class="tab-pane in active">
                                <asp:UpdatePanel runat="server" ID="udppanelHeader" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="form-row d-none">
                                            <div class="form-group col-sm-12 col-md-12">
                                                <label>Company</label>
                                                <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtCompanyCode" Enabled="false" />
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="form-group col-sm-12 col-md-6">
                                                <label>Configuration Item Code</label>
                                                <asp:TextBox runat="server" placeholder="Number" CssClass="form-control form-control-sm" ID="txtEquipmentCode" Enabled="false" />
                                            </div>
                                            <div class="form-group col-sm-12 col-md-6">
                                                <label>Configuration Item Name</label>
                                                <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtEquipmentName" />
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="form-group col-sm-12 col-md-6">
                                                <label>Family</label>
                                                <asp:DropDownList runat="server" ID="ddlEquipmentType" CssClass="form-control form-control-sm" AutoPostBack="True"
                                                    OnSelectedIndexChanged="SelectionCIFamily_Change"
                                                    DataTextField="Description" DataValueField="MaterialGroupCode">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group col-sm-12 col-md-3">
                                                <label>Valid From</label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="txtEquipmentDateFrom" />
                                                    <span class="input-group-append hand">
                                                        <i class="input-group-text fa fa-calendar"></i>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="form-group col-sm-12 col-md-3">
                                                <label>Valid To</label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="txtEquipmentDateTo" />
                                                    <span class="input-group-append hand">
                                                        <i class="input-group-text fa fa-calendar"></i>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="form-group col-sm-12 col-md-4">
                                                <label>Class</label>
                                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlEMClass" AutoPostBack="True"
                                                OnSelectedIndexChanged="SelectionCI_Change">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group col-sm-12 col-md-4">
                                                <label>Category</label>
                                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlCategory">
                                                    <asp:ListItem Text="Main Configuration Item" Value="00" />
                                                    <%--<asp:ListItem Text="Sub Configuration Item" Value="01" />--%>
                                                    <asp:ListItem Text="Virtual Configuration Item" Value="02" />
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group col-sm-12 col-md-4">
                                                <label>Status</label>
                                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlStatus">
                                                    <asp:ListItem Text="New" Value="N" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="form-group col-sm-12 col-md-6">
                                                <label>
                                                    Owner Service
                                                </label>
                                                <asp:DropDownList ID="ddlOwnerService" CssClass="form-control form-control-sm required" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="panelAttributes" class="tab-pane">
                                <asp:UpdatePanel runat="server" ID="udpAttributes" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="fieldset-defult">
                                            <legend class="legend-defult">Attributes Box
                                            </legend>
                                            <div style="padding: 0px 8px;">
                                                <div class="form-row">
                                                    <asp:Repeater runat="server" ID="rptAttributes" OnItemDataBound="rptAttributes_ItemDataBound">
                                                        <ItemTemplate>
                                                            <div runat="server" class="form-group col-sm-12 col-md-6" id="xDisplay" style="margin-bottom: 10px;">
                                                                <label>
                                                                    <asp:Label ID="lbPrice" runat="server" Text='<%# Eval("Description")%>' /></label>
                                                                <asp:HiddenField ID="hddsid" runat="server" Value='<%# Eval("SID") %>' />
                                                                <asp:HiddenField ID="hddobjectid" runat="server" Value='<%# Eval("ObjectID") %>' />
                                                                <asp:HiddenField ID="hddpropertiescode" runat="server" Value='<%# Eval("PropertiesCode") %>' />
                                                                <asp:HiddenField ID="hddselectedcode" runat="server" Value='<%# Eval("SelectedCode") %>' />
                                                                <asp:TextBox runat="server" ID="txtdata" Text='<%# Eval("xValue") %>' Visible='<%# !isSelectedValue(Eval("SelectedCode")) %>'
                                                                    CssClass="form-control form-control-sm" placeholder="Text" />
                                                                <asp:DropDownList ID="ddlproperties" runat="server" CssClass="form-control form-control-sm" Style="display: none;"></asp:DropDownList>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="panelGeneral" class="tab-pane">
                                <asp:UpdatePanel ID="udpGeneral" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
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
                                                <div class="form-row d-none">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Material No</label>
                                                        <asp:TextBox runat="server" onchange="sendValueID(this.value, 'IncludeLibraryPlaceHolder_ContentPlaceHolder1_rptAttributes_txtdata_3')" placeholder="Text" CssClass="form-control form-control-sm" ID="txtdata_3" />
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Start-up Date</label>
                                                        <div class="input-group">
                                                            <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="txtGeneralBox_Start_UpDate" />
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
                                        </fieldset>
                                        <br />
                                        <fieldset class="fieldset-defult">
                                            <legend class="legend-defult">Reference Data Box
                                            </legend>
                                            <div style="padding: 0px 8px;">
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Acquisition Value</label>
                                                        <asp:TextBox runat="server" placeholder="Number" CssClass="form-control form-control-sm" ID="txtReferenceDataBox_AcquisitionValue" />
                                                    </div>
                                                    <%--for ITG--%>
                                                    <div class="form-group col-sm-12 col-md-3 d-none">
                                                        <label>Brand</label>
                                                        <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtReferenceDataBox_CategoryCode" />
                                                    </div>
                                                    <%--=====--%>
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Acquisition Date</label>
                                                        <div class="input-group">
                                                            <asp:TextBox runat="server" placeholder="dd/mm/yyyy" CssClass="form-control form-control-sm date-picker" ID="txtReferenceDataBox_AcquisitionDate" />
                                                            <span class="input-group-append hand">
                                                                <i class="input-group-text fa fa-calendar"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </fieldset>
                                        <br />
                                        <fieldset class="fieldset-defult">
                                            <legend class="legend-defult">Manufacturer Data Box
                                            </legend>
                                            <div style="padding: 0px 8px;">
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Manufacturer</label>
                                                        <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtManufacturerDataBox_Manufacturer" />
                                                    </div>
                                                    <%--for ITG--%>
                                                    <div class="form-group col-sm-12 col-md-3 d-none">
                                                        <label>Model</label>
                                                        <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtManufacturerDataBox_AuthorizeGroup" />
                                                    </div>
                                                    <%--=====--%>
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Manufacturer Country</label>
                                                        <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtManufacturerDataBox_ManufacturerCountry" />
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Model No.</label>
                                                        <asp:TextBox runat="server" placeholder="Number" CssClass="form-control form-control-sm" ID="txtManufacturerDataBox_ModelNo" />
                                                    </div>
                                                    <%--for ITG--%>
                                                    <div class="form-group col-sm-12 col-md-3 d-none">
                                                        <label>Adapter</label>
                                                        <asp:TextBox runat="server" placeholder="Number/Text" CssClass="form-control form-control-sm" ID="txtManufacturerDataBox_Reference" />
                                                    </div>
                                                    <%--=====--%>
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Constr.Yr/Mn</label>
                                                        <div class="form-row">
                                                            <div class="col-xs-12 col-sm-5">
                                                                <asp:TextBox runat="server" placeholder="Number" CssClass="form-control form-control-sm" ID="txtManufacturerDataBox_Constr_Yr" />
                                                            </div>
                                                            <div class="col-xs-12 col-sm-2" style="text-align: center;">
                                                                /
                                                            </div>
                                                            <div class="col-xs-12 col-sm-5">
                                                                <asp:TextBox runat="server" placeholder="Number" CssClass="form-control form-control-sm" ID="txtManufacturerDataBox_Constr_Mn" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Menu Part No.</label>
                                                        <asp:TextBox runat="server" placeholder="Number" CssClass="form-control form-control-sm" ID="txtManufacturerDataBox_MenuPartNo" />
                                                    </div>
                                                    <%--for ITG--%>
                                                    <div class="form-group col-sm-12 col-md-3 d-none">
                                                        <label>DCC</label>
                                                        <asp:TextBox runat="server" placeholder="Number" CssClass="form-control form-control-sm" ID="txtManufacturerDataBox_InventoryNO" />
                                                    </div>
                                                    <%--=====--%>
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Serial No.</label>
                                                        <asp:TextBox runat="server" placeholder="Number" CssClass="form-control form-control-sm" ID="txtManufacturerDataBox_SerialNo" />
                                                    </div>
                                                </div>
                                            </div>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <%--<div id="panelLocation" class="tab-pane">                                   
                                <div class="form-group col-sm-12">                                    
                                </div>
                                <fieldset class="fieldset-defult">
                                    <legend class="legend-defult">Location Data Box
                                    </legend>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            
                                   
                                            <asp:TextBox CssClass="d-none" ID="DropDownList1Hidden" runat="server" />                 
                                                    <div style="padding: 0px 8px;">
                                                         <label>Location List</label>
                                            <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="DropDownList1"
                                                AutoPostBack="True"
                                                OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
                                               >
                                            </asp:DropDownList>
                                                        <div class="form-row">
                                                            <div class="form-group col-sm-12 col-md-6">
                                                                <label>Plant</label>
                                                                <asp:TextBox runat="server" placeholder="Number" ID="txtPlantID" CssClass="form-control form-control-sm" ReadOnly="true" />                                                            
                                                            </div>
                                                            <div class="form-group col-sm-12 col-md-6">
                                                                <label>Location</label>
                                                                <asp:TextBox runat="server" placeholder="Number" ID="txtLocationID" CssClass="form-control form-control-sm" ReadOnly="true" />                                  
                                                             </div>
                                                        </div>
                                                        <div class="form-row">
                                                            <div class="form-group col-sm-12 col-md-6">
                                                                <label>Room</label>
                                                                <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtRoomID" ReadOnly="true" />
                                                            </div>
                                                            <div class="form-group col-sm-12 col-md-6">
                                                                <label>Shelf</label>
                                                                <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtShelfID" ReadOnly="true" />
                                                            </div>
                                                        </div>
                                                        <div class="form-row">
                                                            <div class="form-group col-sm-12 col-md-6">
                                                                <label>Work Center</label>
                                                                <asp:TextBox runat="server" placeholder="Number" ID="txtWorkCenterID" CssClass="form-control form-control-sm" ReadOnly="true" />
                                                            </div>
                                                            <div class="form-group col-sm-12 col-md-6">
                                                                <label>Slot</label>
                                                                <asp:TextBox runat="server" placeholder="Number" ID="txtSlotID" CssClass="form-control form-control-sm" ReadOnly="true" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <fieldset class="fieldset-defult">
                                                        <legend class="legend-defult">Address Box
                                                        </legend>
                                                        <div style="padding: 0px 8px;">
                                                            <div class="form-row">
                                                                <div class="form-group col-sm-12 col-md-12">
                                                                    <label>Name</label>
                                                                    <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtNameID" ReadOnly="true" />
                                                                </div>
                                                            </div>
                                                            <div class="form-row">
                                                                <div class="form-group col-sm-12 col-md-12">
                                                                    <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtNameID2" ReadOnly="true" />
                                                                </div>
                                                            </div>
                                                            <div class="form-row">
                                                                <div class="form-group col-sm-12 col-md-12">
                                                                    <label>Address</label>
                                                                </div>
                                                            </div>
                                                            <div class="form-row">
                                                                <div class="form-group col-sm-12 col-md-4">
                                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtAddressZip" placeholder="Zip" ReadOnly="true" />
                                                                </div>
                                                                <div class="form-group col-sm-12 col-md-4">
                                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtAddressCity" placeholder="City" ReadOnly="true" />
                                                                </div>
                                                                <div class="form-group col-sm-12 col-md-2">
                                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtAddressID1" placeholder="Code 1" ReadOnly="true" />
                                                                </div>
                                                                <div class="form-group col-sm-12 col-md-2">
                                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtAddressID2" placeholder="Code 2" ReadOnly="true" />
                                                                </div>
                                                            </div>
                                                            <div class="form-row">
                                                                <div class="form-group col-sm-12 col-md-4">
                                                                    <label>Street</label>
                                                                    <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtStreetID" ReadOnly="true" />
                                                                </div>
                                                                <div class="form-group col-sm-12 col-md-4">
                                                                    <label>Telephone</label>
                                                                    <asp:TextBox runat="server" placeholder="Number" CssClass="form-control form-control-sm" ID="txtTelephoneID" ReadOnly="true"/>
                                                                </div>
                                                                <div class="form-group col-sm-12 col-md-4">
                                                                    <label>Fax</label>
                                                                    <asp:TextBox runat="server" placeholder="Number" CssClass="form-control form-control-sm" ID="txtFaxID" ReadOnly="true" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </fieldset>
                                
                            </div>--%>
                            
                            <div id="panelLocation" class="tab-pane">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                    <fieldset class="fieldset-defult">
                                        <legend class="legend-defult">Location Data Box
                                        </legend>
                                        <div style="padding: 0px 8px;">
                                            <div class="form-row">
                                                <div class="form-group col-sm-12 col-md-6">
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <label>Location</label>
                                                            <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtLocationDataBox_Location"
                                                                 placeholder="Ex. BTT, TST, SRB" />
                                                            <%--<asp:DropDownList runat="server" CssClass="form-control form-control-sm d-none" ID="ddlLocationDataBox_Plant_Code"
                                                                OnSelectedIndexChanged="ddlLocationDataBox_Plant_Code_SelectedIndexChanged"
                                                                AutoPostBack="true" onchange="AGLoading(true);">
                                                            </asp:DropDownList>--%>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="form-group col-sm-12 col-md-6">
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpLocationDataBox_Location">
                                                        <ContentTemplate>
                                                            <label>Floor/Phase</label>
                                                            <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtLocationDataBox_Flow_Phase"
                                                                 placeholder="Ex. IDC TST FL.10, Phase 1" />
                                                            <%--<asp:DropDownList runat="server" CssClass="form-control form-control-sm d-none" ID="ddlLocationDataBox_Location_Code">
                                                            </asp:DropDownList>--%>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="form-group col-sm-12 col-md-6">
                                                    <label>Room</label>
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtLocationDataBox_Room" 
                                                        placeholder="Ex. Cloud" />
                                                </div>
                                                <div class="form-group col-sm-12 col-md-6">
                                                    <label>Cabinet</label>
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtLocationDataBox_Cabinet"
                                                         placeholder="Ex. Rack A10" />
                                                </div>
                                                <div class="form-group col-sm-12 col-md-3 d-none">
                                                    <label>Mode</label>
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtLocationDataBox_CategoryCode"
                                                         placeholder="Text" />
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <%--<div class="form-group col-sm-12 col-md-6">
                                                    <label>Work Center</label>
                                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlLocationDataBox_WorkCenter_Code">
                                                    </asp:DropDownList>
                                                </div>--%>
                                                <div class="form-group col-sm-12 col-md-6">
                                                    <label>Shelf</label>
                                                    <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtLocationDataBox_Shelf" />
                                                </div>
                                                <div class="form-group col-sm-12 col-md-6">
                                                    <label>Slot</label>
                                                    <asp:TextBox runat="server" placeholder="Number" ID="txtLocationDataBox_Slot" CssClass="form-control form-control-sm" />
                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <br />
                                    <fieldset class="fieldset-defult">
                                        <legend class="legend-defult">Address Box
                                        </legend>
                                        <div style="padding: 0px 8px;">
                                            <div class="form-row">
                                                <div class="form-group col-sm-12 col-md-12">
                                                    <label>Name</label>
                                                    <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtAddressBox_Name_1" />
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="form-group col-sm-12 col-md-12">
                                                    <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtAddressBox_Name_2" />
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="form-group col-sm-12 col-md-12">
                                                     <label>Address</label>
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="form-group col-sm-12 col-md-4">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtAddressBox_Address_Zip" placeholder="Zip" />
                                                </div>
                                                <div class="form-group col-sm-12 col-md-4">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtAddressBox_Address_City" placeholder="City" />
                                                </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtAddressBox_Address_Code_1" placeholder="Code 1" />
                                                </div>
                                                <div class="form-group col-sm-12 col-md-2">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtAddressBox_Address_Code_2" placeholder="Code 2" />
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="form-group col-sm-12 col-md-4">
                                                    <label>Street</label>
                                                    <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtAddressBox_Street" />
                                                </div>
                                                 <div class="form-group col-sm-12 col-md-4">
                                                    <label>Telephone</label>
                                                    <asp:TextBox runat="server" placeholder="Number" CssClass="form-control form-control-sm" ID="txtAddressBox_Telephone" />
                                                </div>
                                                <div class="form-group col-sm-12 col-md-4">
                                                    <label>Fax</label>
                                                    <asp:TextBox runat="server" placeholder="Number" CssClass="form-control form-control-sm" ID="txtAddressBox_Fax" />
                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="panelOganization" class="tab-pane">
                                <asp:UpdatePanel ID="udpOganization" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <fieldset class="fieldset-defult">
                                            <legend class="legend-defult">Account Assignment Box
                                            </legend>
                                            <div style="padding: 0px 8px;">
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-12" style="margin-bottom: 0px;">
                                                        <label>Company</label>
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" Enabled="false" ID="txtAccountAssignmentBox_CompanyCode" />
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" Enabled="false" ID="txtAccountAssignmentBox_CompanyName" />
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Business Area</label>
                                                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlAccountAssignmentBox_BusinessArea_Code">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Asset</label>
                                                        <%--<asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlAccountAssignmentBox_Asset">
                                                        </asp:DropDownList>--%>
                                                        <div class="input-group">
                                                            <asp:TextBox runat="server" ID="txtAccountAssignmentBox_AssetDesc" Enabled="false"
                                                                CssClass="form-control form-control-sm" style="background-color: #fff;" />
                                                            <span class="input-group-append hand" style="cursor: pointer;"
                                                                onclick="$('#<%= btnSearchAsset.ClientID %>').click();">
                                                                <i class="input-group-text fa fa-search"></i>
                                                            </span>
                                                        </div>
                                                        <asp:HiddenField runat="server" ID="hddAccountAssignmentBox_AssetCode" />
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button Text="" runat="server" ID="btnSearchAsset" CssClass="d-none" 
                                                                    OnClick="btnSearchAsset_Click" OnClientClick="AGLoading(true);" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Cost Center</label>
                                                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlAccountAssignmentBox_CoustCenter">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </fieldset>

                                        <fieldset class="fieldset-defult" style="display: none;">
                                            <legend class="legend-defult">Responsibilities
                                            </legend>
                                            <div style="padding: 0px 8px;">
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Planning Plant</label>
                                                        <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtResponsibilitiesBox_PlanningPlant" />
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Planner Group</label>
                                                        <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtResponsibilitiesBox_PlannerGroup" />
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <label>Main WorkCtr</label>
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtResponsibilitiesBox_MainWorkCtr_1" />
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-2" style="text-align: center;">
                                                        /
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtResponsibilitiesBox_MainWorkCtr_2" />
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Catalog Profile</label>
                                                        <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" ID="txtResponsibilitiesBox_CatalogProfile" />
                                                    </div>
                                                </div>
                                            </div>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="panelOwnerAssignment" class="tab-pane">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpTableOwnerAssignment">
                                    <ContentTemplate>
                                        <div class="table-responsive">
                                            <table id="tableOwnerAssignment" class="table table-bordered table-striped table-hover table-sm nowrap">
                                                <thead>
                                                    <tr>
                                                        <th class="text-center" style="width: 30px;">#</th>
                                                        <th class="text-center" style="width: 30px;">#</th>
                                                        <th style="width: 65px;">Item No</th>
                                                        <th style="width: 110px;">Owner Type</th>
                                                        <th>Owner Desc</th>
                                                        <th style="width: 100px;">Begin Date</th>
                                                        <th style="width: 100px;">End Date</th>
                                                        <th style="width: 100px;">SLA Group</th>
                                                        <th class="text-center" style="width: 60px;">Active</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <%--<asp:Repeater runat="server" ID="rptOwnerAssignmentList">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="text-align: center;">
                                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <i class="fa fa-times text-danger" style="cursor: pointer;" onclick="$(this).next().click();"></i>
                                                                            <asp:Button Text="" runat="server" CssClass="hide" ID="btnRemoveRowTableOwnerAssignment"
                                                                                OnClick="btnRemoveRowTableOwnerAssignment_Click" OnClientClick="AGLoading(true);"
                                                                                CommandArgument='<%# Eval("LineNumber") %>' />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td style="text-align: center;">
                                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <i class="fa fa-pencil-square-o text-success" style="cursor: pointer;" onclick="$(this).next().click();"></i>
                                                                            <asp:Button Text="" runat="server" CssClass="hide" ID="btnEditRowTableOwnerAssignment"
                                                                                OnClick="btnEditRowTableOwnerAssignment_Click" OnClientClick="AGLoading(true);"
                                                                                CommandArgument='<%# Eval("LineNumber") %>' />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td>
                                                                    <asp:Label Text='<%# Eval("LineNumber") %>' runat="server" ID="lblOwnerAssignmentBox_ItemNo" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label Text='<%# getDescriptionOwnerType(Eval("OwnerType").ToString()) %>' runat="server" ID="lblOwnerAssignmentBox_OwnerType" />
                                                                    <asp:HiddenField runat="server" ID="hddOwnerAssignmentBox_OwnerType" Value='<%# Eval("OwnerType")%>' />
                                                                </td>
                                                                <td>
                                                                    <asp:Label Text='<%# getDescriptionOwnerCode(Eval("OwnerType").ToString() ,Eval("OwnerCode").ToString()) %>' runat="server" ID="lblOwnerAssignmentBox_OwnerCode" />
                                                                    <asp:HiddenField runat="server" ID="hddOwnerAssignmentBox_OwnerCode" Value='<%# Eval("OwnerCode")%>' />
                                                                </td>
                                                                <td>
                                                                    <asp:Label Text='<%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("BeginDate").ToString()) %>' 
                                                                        runat="server" ID="lblOwnerAssignmentBox_BeginDate" />
                                                                    <asp:HiddenField runat="server" ID="hddOwnerAssignmentBox_BeginDate" Value='<%# Eval("BeginDate")%>' />
                                                                </td>
                                                                <td>
                                                                    <asp:Label Text='<%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("EndDate").ToString()) %>' 
                                                                        runat="server" ID="lblOwnerAssignmentBox_EndDate" />
                                                                    <asp:HiddenField runat="server" ID="hddOwnerAssignmentBox_EndDate" Value='<%# Eval("EndDate")%>' />
                                                                </td>
                                                                <td>
                                                                    <asp:Label Text='<%# getDescriptionSLAGroup(Eval("SLAGroupCode").ToString()) %>' runat="server" ID="lblSLAGroup" />
                                                                    <asp:HiddenField ID="hddOwnerAssignmentBox_SLAGroup" runat="server" Value='<%# Eval("SLAGroupCode")%>' />
                                                                </td>
                                                                <td style="text-align: center;">
                                                                    <asp:Label Text='<%# Eval("ActiveStatus") %>' runat="server" ID="lblActiveStatus" />
                                                                    <asp:HiddenField ID="hddOwnerAssignmentBox_ActiveStatus" runat="server" Value='<%# Eval("ActiveStatus")%>' />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>--%>
                                                    <tfoot>
                                                        <tr>
                                                            <td colspan="9">
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <div>
                                                                            <i class="fa fa-plus-square-o text-success fa-fw"
                                                                                style="cursor: pointer;" onclick="$(this).next().click();"></i>
                                                                            <asp:Button Text="" runat="server" CssClass="hide" ID="btnAddRowTableOwnerAssignment"
                                                                                OnClick="btnAddRowTableOwnerAssignment_Click" OnClientClick="AGLoading(true);" />
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                
                                                            </td>
                                                        </tr>
                                                    </tfoot>
                                                </tbody>
                                            </table>
                                            <div class="d-none">
                                                <%--<asp:TextBox runat="server" ID="txtDatasOwnerAssignment" />--%>
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button Text="" runat="server" CssClass="hide" ID="btnRemoveRowTableOwnerAssignment"
                                                            OnClick="btnRemoveRowTableOwnerAssignment_Click" OnClientClick="AGLoading(true);" />
                                                        <asp:Button Text="" runat="server" CssClass="hide" ID="btnEditRowTableOwnerAssignment"
                                                            OnClick="btnEditRowTableOwnerAssignment_Click" OnClientClick="AGLoading(true);" />
                                                        <asp:HiddenField runat="server" ID="hddOwnerAssign_LineNumber" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <div class="d-none" runat="server" id="panel_data_owner_assign"></div>
                                            </div>
                                        </div>
                                        <%--<div>
                                            <table class="table-finans-v2">
                                                <thead>
                                                    <tr>
                                                        <th class="text-center" style="width: 30px;">#</th>
                                                        <th style="width: 65px;">Item No</th>
                                                        <th style="width: 110px;">Owner Type</th>
                                                        <th>Owner Desc</th>
                                                        <th style="width: 100px;">Begin Date</th>
                                                        <th style="width: 100px;">End Date</th>
                                                        <th style="width: 100px;">SLA Group</th>
                                                        <th class="text-center" style="width: 60px;">Active</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater runat="server" ID="rptTableOwnerAssignment" OnItemDataBound="rptTableOwnerAssignment_ItemDataBound">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="text-align: center;">
                                                                    <i class="fa fa-times" style="cursor: pointer;" onclick="$(this).next().click();"></i>
                                                                    <asp:Button Text="" runat="server" CssClass="hide" ID="btnRemoveRowTableOwnerAssignment"
                                                                        OnClick="btnRemoveRowTableOwnerAssignment_Click" OnClientClick="AGLoading(true);"
                                                                        CommandArgument='<%# Eval("LineNumber")%>' />
                                                                </td>
                                                                <td>
                                                                    <asp:Label Text='<%# Eval("LineNumber")%>' runat="server" ID="lblOwnerAssignmentBox_ItemNo" />
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:DropDownList runat="server" ID="ddlOwnerAssignmentBox_OwnerType" onchange="$(this).next().click();">
                                                                                <asp:ListItem Text="เลือก" Value="" />
                                                                                <asp:ListItem Text="Customer" Value="01" />
                                                                                <asp:ListItem Text="Vendor" Value="02" />
                                                                                <asp:ListItem Text="Employee" Value="03" />
                                                                                <asp:ListItem Text="SoldToParty" Value="ST" />
                                                                                <asp:ListItem Text="ShipToParty" Value="SH" />
                                                                                <asp:ListItem Text="BillToParty" Value="BP" />
                                                                            </asp:DropDownList>
                                                                            <asp:Button Text="" runat="server" CssClass="hide" ID="btnOwnerAssignmentBox_LoadOwner"
                                                                                OnClick="btnOwnerAssignmentBox_LoadOwner_Click" OnClientClick="AGLoading(true);" />
                                                                            <asp:HiddenField runat="server" ID="hddOwnerAssignmentBox_OwnerType" Value='<%# Eval("OwnerType")%>' />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpBox_CustomerSelect">
                                                                        <ContentTemplate>
                                                                            <sna:AutoCompleteControl runat="server" id="Complete_OwnerAssignmentBox_CustomerSelect" />
                                                                            <asp:HiddenField runat="server" ID="hddOwnerAssignmentBox_OwnerCode" Value='<%# Eval("OwnerCode")%>' />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" CssClass="date-picker" ID="txtOwnerAssignmentBox_BeginDate" Text='<%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("BeginDate").ToString())%>' />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" CssClass="date-picker" ID="txtOwnerAssignmentBox_EndDate" Text='<%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("EndDate").ToString())%>' />
                                                                </td>
                                                                <td>
                                                                    <asp:HiddenField ID="hdfSLAGroup" runat="server" Value='<%# Eval("SLAGroupCode")%>' />
                                                                    <asp:DropDownList ID="ddlSLAGroup" runat="server" DataValueField="TierGroupCode" DataTextField="TierGroupDescription"></asp:DropDownList>
                                                                </td>
                                                                <td style="text-align: center;">
                                                                    <asp:CheckBox runat="server" ID="chkOwnerAssignmentBox_ActiveStatus" Checked='<%# Convert.ToBoolean(Eval("ActiveStatus"))%>' />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                                <tfoot>
                                                    <tr>
                                                        <td colspan="9">
                                                            <div>
                                                                <i class="fa fa-plus-square-o text-success fa-fw"
                                                                    style="cursor: pointer;" onclick="$(this).next().click();"></i>
                                                                <asp:Button Text="" runat="server" CssClass="hide" ID="btnAddRowTableOwnerAssignment"
                                                                    OnClick="btnAddRowTableOwnerAssignment_Click" OnClientClick="AGLoading(true);" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </tfoot>
                                            </table>
                                        </div>--%>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <%--<sna:SmartPaging runat="server" id="SmartPagingOwnerAssignment" />--%>
                            </div>
                            <div id="panelSale" class="tab-pane">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpPanelSale">
                                    <ContentTemplate>
                                        <fieldset class="fieldset-defult">
                                            <legend class="legend-defult">Sales and Distribution
                                            </legend>
                                            <div style="padding: 0px 8px;">
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        <label>Billing Doc Type</label>
                                                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlSalesAndDistribution_BillingDocTypeCode"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlSalesAndDistribution_BillingDocTypeCode_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        <label>Billing Doc Year</label>
                                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtSalesAndDistribution_BillingDocYear" />
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        <label>Billing Doc Number</label>
                                                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlSalesAndDistribution_BillingDocNumber"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlSalesAndDistribution_BillingDocNumber_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        <label>Sale Organization</label>
                                                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlSalesAndDistribution_SalseOrganization">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        <label>Sale Office</label>
                                                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlSalesAndDistribution_SaleOffice">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        <label>Sale Group</label>
                                                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlSalesAndDistribution_SaleGroup">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        <label>Dist Chanal</label>
                                                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlSalesAndDistribution_DistChanal">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-4">
                                                        <label>Division</label>
                                                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlSalesAndDistribution_Division">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </fieldset>


                                        <br />
                                        <fieldset class="fieldset-defult">
                                            <legend class="legend-defult">License Box
                                            </legend>
                                            <div style="padding: 0px 8px;">
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>License Number</label>
                                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtLicenseBox_LicenseNumber" />
                                                    </div>
                                                </div>
                                            </div>
                                        </fieldset>
                                        <br />
                                        <fieldset class="fieldset-defult">
                                            <legend class="legend-defult">Partner Data Box
                                            </legend>
                                            <div style="padding: 0px 8px;">
                                                <div class="form-row">
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Sold-To Party</label>
                                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtPartnerDataBox_Sold_ToParty" />
                                                    </div>
                                                    <div class="form-group col-sm-12 col-md-6">
                                                        <label>Ship-To Party</label>
                                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtPartnerDataBox_Ship_ToParty" />
                                                    </div>
                                                </div>
                                            </div>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="panelSerialData" class="tab-pane">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpTableSerialData">
                                    <ContentTemplate>
                                        <table class="table-finans-v2">
                                            <tr>
                                                <th style="width: 30px;">#</th>
                                                <th style="width: 60px;">Item No</th>
                                                <th>Material Name</th>
                                                <%--<th>Material Code</th>--%>
                                                <th style="width: 200px;">Serial No</th>
                                                <th style="width: 160px;">Effective From</th>
                                                <th style="width: 160px;">Effective To</th>
                                                <th style="width: 90px;">Active Status</th>
                                            </tr>
                                            <asp:Repeater runat="server" ID="rptTableSerialData" OnItemDataBound="rptTableSerialData_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: center;">
                                                            <i class="fa fa-times" style="cursor: pointer;" onclick="$(this).next().click();"></i>
                                                            <asp:Button Text="" runat="server" CssClass="hide" ID="btnRemoveRowTableSerialData"
                                                                OnClick="btnRemoveRowTableSerialData_Click" OnClientClick="AGLoading(true);"
                                                                CommandArgument='<%# Eval("LineNumber")%>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label Text='<%# Eval("LineNumber")%>' runat="server" ID="lblTableSerialData_ItemNo" />
                                                        </td>
                                                        <td>
                                                            <sna:AutoCompleteControl runat="server" id="Complete_TableSerialData_MaterialCode" />
                                                            <%--<asp:DropDownList runat="server" ID="ddlTableSerialData_MaterialCode" onchange="rebindDataDDL(this);"
																data-id-click='<%# (Container.FindControl("btnRebindSerialNo") as Button).ClientID %>'
																CssClass="selectpicker form-control form-control-sm ddl_TableSerialData_MaterialCode" data-live-search="true">
															</asp:DropDownList>--%>
                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udp_MaterialCode" class="hide">
                                                                <ContentTemplate>
                                                                    <asp:Button Text="" CssClass="btnRebindSerialNo" runat="server" ID="btnRebindSerialNo"
                                                                        OnClick="btnRebindSerialNo_Click" OnClientClick="AGLoading(true);" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <asp:HiddenField runat="server" ID="hddTableSerialData_MaterialCode" Value='<%# Eval("MaterialCode")%>' />
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpDDlSerialNo" style="height: 25px;">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList runat="server" ID="ddlTableSerialData_SerialNo">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <asp:HiddenField runat="server" ID="hddTableSerialData_SerialNo" Value='<%# Eval("LineNumber")%>' />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" CssClass="date-picker" ID="txtTableSerialData_EffectiveFrom" Text='<%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("EffectiveFrom").ToString())%>' />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" CssClass="date-picker" ID="txtTableSerialData_EffectiveTo" Text='<%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("EffectiveTo").ToString())%>' />
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:CheckBox runat="server" ID="chkTableSerialData_ActiveStatus" Checked='<%# Convert.ToBoolean(Eval("ActiveStatus"))%>' />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <tr>
                                                <td colspan="7">
                                                    <div>
                                                        <i class="fa fa-plus-square-o text-success fa-fw"
                                                            style="cursor: pointer;" onclick="$(this).next().click();"></i>
                                                        <asp:Button Text="" runat="server" CssClass="hide" ID="btnAddRowTableSerialData"
                                                            OnClick="btnAddRowTableSerialData_Click" OnClientClick="AGLoading(true);" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="panelWarranty" class="tab-pane">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpTableWarranty">
                                    <ContentTemplate>
                                        <fieldset class="fieldset-defult">
                                            <legend class="legend-defult">Warranty
                                            </legend>
                                            <div style="padding: 0px 8px;">
                                                <div class="form-row">
                                                    <div class="form-group col-md-6 col-sm-12">
                                                        <label>
                                                            Maintenance Start Date
                                                        </label>
                                                        <div class="input-group">
                                                            <asp:TextBox runat="server" onchange="$('#btnWarrantyNextMaintenanceRefStartDate').click();" CssClass="form-control form-control-sm date-picker"
                                                                ID="txtMaintenanceStartDate"
                                                                placeholder="dd/mm/yyy" />
                                                            <span class="input-group-append hand">
                                                                <i class="fa fa-calendar input-group-text"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-6 col-sm-12">
                                                        <label>
                                                            End
                                                        </label>
                                                        <div class="input-group">
                                                            <asp:TextBox runat="server" onchange="$('#btnWarrantyNextMaintenanceRefEndDate').click();" CssClass="form-control form-control-sm date-picker"
                                                                ID="txtMaintenanceEndDate"
                                                                placeholder="dd/mm/yyy" />
                                                            <span class="input-group-append hand">
                                                                <i class="fa fa-calendar input-group-text"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-md-4 col-sm-12">
                                                        <label>
                                                            Maintenance Type
                                                        </label>
                                                        <asp:DropDownList ID="ddlMaintenanceType" CssClass="form-control form-control-sm" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-md-2 col-sm-12">
                                                        <label>
                                                            Period (month)
                                                        </label>
                                                        <asp:TextBox ID="txtPeriod" onchange="$('#btnWarrantyNextMaintenanceRefPeriod').click();" placeholder="Number"
                                                            onkeypress="return isNumberKey(event);"
                                                            CssClass="form-control form-control-sm text-right" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-md-4 col-sm-12">
                                                        <label>
                                                            Last Maintenance
                                                        </label>
                                                        <div class="input-group">
                                                            <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker"
                                                                ID="txtLastMaintenanceDate"
                                                                placeholder="dd/mm/yyy" />
                                                            <span class="input-group-append hand">
                                                                <i class="fa fa-calendar input-group-text"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-2 col-sm-12">
                                                        <label>Time</label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtLastMaintenanceTime" runat="server"
                                                                placeholder="hh:ss"
                                                                CssClass="form-control form-control-sm time-picker "></asp:TextBox>
                                                            <div class="input-group-append">
                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-4 col-sm-12">
                                                        <label>
                                                            Next Maintenance
                                                        </label>
                                                        <div class="input-group">
                                                            <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker"
                                                                ID="txtNextMaintenanceDate"
                                                                placeholder="dd/mm/yyy" />
                                                            <span class="input-group-append hand">
                                                                <i class="fa fa-calendar input-group-text"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-2 col-sm-12">
                                                        <label>Time</label>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtNextMaintenanceTime" runat="server"
                                                                placeholder="hh:ss"
                                                                CssClass="form-control form-control-sm time-picker "></asp:TextBox>
                                                            <div class="input-group-append">
                                                                <span class="input-group-text"><i class="fa fa-clock-o"></i></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-md-4 col-sm-12">
                                                        <label>
                                                            Send Mail before Next Maintenance
                                                        </label>
                                                        <div class="input-group">
                                                            <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker"
                                                                ID="txtShowDaySend"
                                                                placeholder="dd/mm/yyy" disabled />
                                                            <span class="input-group-append hand">
                                                                <i class="fa fa-calendar input-group-text"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-2 col-sm-12">
                                                        <label>Day</label>
                                                        <asp:RegularExpressionValidator
                                                            Class="text-danger"
                                                            ID="RegularExpressionValidator1"
                                                            ControlToValidate="txtDaySend" runat="server"
                                                            ErrorMessage="***Only Numbers"
                                                            ValidationExpression="\d+">
                                                            </asp:RegularExpressionValidator>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtDaySend" runat="server"
                                                                placeholder="Day"
                                                                CssClass="form-control form-control-sm">
                                                            </asp:TextBox>
                                                            <div class="input-group-append">
                                                                <span class="input-group-text"><i class="fa fa-paper-plane-o"></i></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-4 col-sm-12">
                                                        <label>&nbsp;</label>
                                                        <div class="input-group">
                                                            <div class="custom-control custom-checkbox">
                                                              <asp:CheckBox ID="CheckBoxDaySend" runat="server" Text="" />
                                                              <label class="custom-control-label" for="<%= CheckBoxDaySend.ClientID%>">Active send mail</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-md-6 col-sm-12">
                                                        <label>
                                                            Warranty start date
                                                        </label>
                                                        <div class="input-group">
                                                            <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker"
                                                                ID="txtWarrantyStartDate"
                                                                placeholder="dd/mm/yyy" />
                                                            <span class="input-group-append hand">
                                                                <i class="fa fa-calendar input-group-text"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-6 col-sm-12">
                                                        <label>
                                                            End
                                                        </label>
                                                        <div class="input-group">
                                                            <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker"
                                                                ID="txtWarrantyEndDate"
                                                                placeholder="dd/mm/yyy" />
                                                            <span class="input-group-append hand">
                                                                <i class="fa fa-calendar input-group-text"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="udpWarrantyNextMaintenance" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnWarrantyNextMaintenanceRefStartDate" ClientIDMode="Static" OnClick="btnWarrantyNextMaintenanceRefStartDate_Click" CssClass="d-none" runat="server" />
                                        <asp:Button ID="btnWarrantyNextMaintenanceRefEndDate" ClientIDMode="Static" OnClick="btnWarrantyNextMaintenanceRefEndDate_Click" CssClass="d-none" runat="server" />
                                        <asp:Button ID="btnWarrantyNextMaintenanceRefPeriod" ClientIDMode="Static" OnClick="btnWarrantyNextMaintenanceRefPeriod_Click" CssClass="d-none" runat="server" />

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="panelAdditionalDate" class="tab-pane">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpTableAssitionalDate">
                                    <ContentTemplate>
                                        <table class="table-finans-v2">
                                            <tr>
                                                <th>Properties</th>
                                                <th>Descripttion</th>
                                                <th>Value</th>
                                                <th>Selected Value</th>
                                            </tr>
                                            <asp:Repeater runat="server" ID="rptTableAssitionalDate">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:Label Text="-" runat="server" ID="lblTableAssitionalDate_Properties" />
                                                        </td>
                                                        <td>
                                                            <asp:Label Text="-" runat="server" ID="lblTableAssitionalDate_Descripttion" />
                                                        </td>
                                                        <td>
                                                            <asp:Label Text="-" runat="server" ID="lblTableAssitionalDate_Value" />
                                                        </td>
                                                        <td>
                                                            <asp:Label Text="-" runat="server" ID="lblTableAssitionalDate_SelectedValue" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <% if (rptTableAssitionalDate.Items.Count == 0)
                                               { %>
                                            <tr>
                                                <td colspan="4" class="alert-info">No date.
                                                </td>
                                            </tr>
                                            <% } %>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="panelPicture" class="tab-pane">
                                <div>
                                    <sna:TimeLineControl runat="server" id="TimeLineControl" />
                                </div>
                            </div>
                            <div id="panelRelation" class="tab-pane">
                                <div>
                                    <sna:FlowChartDiagramRelationControl runat="server" id="FlowChartDiagramRelationControl" RelationType="EQUIPMENT" AlowEditMode="false" />
                                </div>
                                <%--<table class="table-finans-v2">
                                    <tr>
                                        <th>Equipment Parent</th>
                                        <th>Relation</th>
                                        <th>Equipment Child</th>
                                    </tr>
                                    <asp:Repeater runat="server" ID="rptRelationship">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%# Eval("parent") %></td>
                                                <td><%# Eval("relation") %></td>
                                                <td><%# Eval("child") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>--%>
                            </div>
                            <div id="panelHistory" class="tab-pane">
                                <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="table-responsive">
                                            <table id="tableItems" class="table table-bordered table-striped table-hover table-sm nowrap">
                                                <thead>
                                                    <tr>
                                                        <th class="text-nowrap"></th>
                                                        <th class="text-nowrap">หมายเลขใบแจ้งบริการ</th>
                                                        <th class="text-nowrap">ประเภทของปัญหา</th>
                                                        <th class="text-nowrap">หัวเรื่อง</th>
                                                        <th class="text-nowrap">วันที่เอกสาร</th>
                                                        <th class="text-nowrap">ลูกค้า</th>
                                                        <th class="text-nowrap">ผู้ติดต่อ</th>
                                                        <th class="text-nowrap">หมายเลขโทรศัพท์</th>
                                                        <th class="text-nowrap">E-Mail</th>
                                                        <th class="text-nowrap">สถานะ</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptSearchSale" runat="server">
                                                        <ItemTemplate>
                                                            <tr class="c-pointer" onclick="openTicket('<%# Eval("CallerID")%>');">
                                                                <td class="text-nowrap text-center align-middle">
                                                                    <i class="fa fa-pencil-square fa-lg text-dark" title="Edit" onclick="$(this).next().click();"></i>
                                                                    <%--<asp:Button ID="btnLinkTransactionSearch" runat="server" CssClass="d-none hide" OnClick="btnLinkTransactionSearch_Click"
                                                                        OnClientClick="AGLoading(true);" CommandArgument='<%# Eval("CallerID")%>' />--%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("CallerID")%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("issuestatus")%>
                                                                </td>
                                                                <td class="text-truncate" style="max-width: 500px;">
                                                                    <span title="<%# Eval("HeaderText")%>"><%# Eval("HeaderText")%></span>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("DOCDATE")%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("CustomerName")%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("ContractPersonName")%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("ContractPersonTel")%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("Email")%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("CallStatusName")%>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <div class="d-none">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnLinkTransactionSearch" runat="server" ClientIDMode="Static"
                                                CssClass="d-none" OnClick="btnLinkTransactionSearch_Click" OnClientClick="AGLoading(true);" />
                                            <asp:HiddenField runat="server" ID="hddCallerID_Criteria" Value="" ClientIDMode="Static" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <script>
                                    function openTicket(CallerID) {
                                        $("#hddCallerID_Criteria").val(CallerID);
                                        $("#btnLinkTransactionSearch").click();
                                    }
                                </script>
                            </div>
                            <div id="panelChangeLog" class="tab-pane">
                                <sna:ChangeLogControl id="EquipmentChangeLog" runat="server" />
                            </div>
                            <div id="panelTicket" class="tab-pane">
                                
                                <asp:UpdatePanel ID="udpListTicketItems" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>

                                        <div class="table-responsive">
                                            <table id="tableListTicketItems" class="table table-bordered table-striped table-hover table-sm">
                                                <thead>
                                                    <tr>
                                                        <th class="text-nowrap"></th>
                                                        <%--<th class="text-nowrap">จำนวนไฟล์แนบ</th>
                                                    <th class="text-nowrap">จำนวนข้อความแชท</th>--%>
                                                        <th class="text-nowrap">Ticket Date</th>
                                                        <th class="text-nowrap">Ticket Time</th>
                                                        <th class="text-nowrap">Ticket Type</th>
                                                        <th class="d-none">Ticket No.</th>
                                                        <th class="text-nowrap">Ticket No.</th>
                                                        <th class="text-nowrap">Ticket Status</th>
                                                        <th class="text-nowrap">Subject</th>
                                                        <th class="text-nowrap">Client</th>
                                                        <th class="text-nowrap">Contact</th>
                                                        <th class="text-nowrap">Configuration Item</th>
                                                        <th class="text-nowrap">Impact</th>
                                                        <th class="text-nowrap">Urgency</th>
                                                        <th class="text-nowrap">Priority</th>
                                                        <th class="text-nowrap">Owner Service</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptListTicketItems" runat="server">
                                                        <ItemTemplate>
                                                            <tr class="<%# GetRowColorAssign(Eval("CallStatus").ToString())%> c-pointer"
                                                                onclick="openTicketNewWindow('<%# Eval("CallerID")%>');">
                                                                <td class="text-nowrap text-center align-middle">
                                                                    <i class="fa fa-pencil-square fa-lg text-dark" title="Edit"></i>
                                                                </td>
                                                                <%--<td class="text-nowrap">
                                                                <span <%# Eval("total_attachfile") == "" ? "" : "onclick='$(this).next().click();' style='cursor:pointer;'"%>><%# Eval("total_attachfile") == "" ? "ไม่มีไฟล์แนบ" : "(" + Eval("total_attachfile") +")" %></span>
                                                                <asp:Button ID="btnLinkAttachSearch" runat="server" CssClass="d-none" OnClick="btnLinkAttachSearch_Click"
                                                                    OnClientClick="AGLoading(true);" CommandArgument='<%# Eval("CallerID")+","+ Eval("CallerID")+","+ Eval("HeaderText")%>' />
                                                            </td>
                                                            <td class="text-nowrap text-right">
                                                                <%# Eval("total_messagechat") == "" ? "0" : Eval("total_messagechat")%>
                                                            </td>--%>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("DOCDATE").ToString()%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("DOCTIME").ToString()%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("DocumentTypeDesc")%>
                                                                </td>
                                                                <td class="d-none">
                                                                    <%# Eval("CallerID")%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("CallerIDDisplay")%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("DocStatusDesc")%>
                                                                </td>
                                                                <td class="text-truncate" style="max-width: 500px;">
                                                                    <span title="<%# Eval("HeaderText")%>"><%# Eval("HeaderText")%></span>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("CustomerName")%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("ContractPersonName")%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("EquipmentName")%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("ImpactName")%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("UrgencyName")%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("PriorityName")%>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    <%# Eval("OwnerGroupName")%>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <div class="d-none">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnOpenTicketNewWindow" runat="server" ClientIDMode="Static"
                                                CssClass="d-none" OnClick="openTicketNewWindow_Click" OnClientClick="AGLoading(true);" />
                                            <asp:HiddenField runat="server" ID="hddCallerID" Value="" ClientIDMode="Static" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <script>
                function bindingOwnerAssignment() {
                    setTimeout(function () {
                        $("#<%= btnLoadBindingOwnerAssignment.ClientID %>").click();
                    }, 10);
                }

                function bindingTicket() {
                    setTimeout(function () {
                        $("#<%= btnLoadBindingTicket.ClientID %>").click();
                    }, 10);
                }
                
                //function tabPanelDetailEquipment(obj) {
                //	obj = $(obj).parent();

                //	if (!$(obj).hasClass("active")) {
                //		$(".panel-detail-equipment").fadeOut(300);
                //		$(".nav.nav-tabs").find("li").removeClass("active");

                //		var idPanel = $(obj).attr("data-panel");
                //		$(obj).addClass("active");
                //		setTimeout(function () {
                //			$("#" + idPanel).fadeIn(300);
                //		}, 301);
                //	}
                //}

                function rebindDataDDL(obj) {
                    var id = $(obj).attr("data-id-click");
                    $("#" + id).click();
                }

                function showDiagram() {
                    AGLoading(true);
                    //var code = $(obj).attr('data-code');
                    var code = "<%= Request["code"]%>";
                    if (code == "") {
                        code = $("#<%= txtEquipmentCode.ClientID %>").val();
                    }
                    $("#modal-diagram").modal("show");
                    $("#modal-diagram iframe").prop("src", "/crm/Master/Equipment/EquipmentDiagramRelation.aspx?id=" + code + "&noRedirect=true");

                    //$("#modal-diagram iframe").prop("src", "/crm/Master/Equipment/EquipmentDiagram.aspx?relationNode=" + code);

                    return false;
                }

                function modifyDisplayIframe(obj) {
                    var OJBIFRAME = $(obj).contents();
                    OJBIFRAME.find(".navbar.navbar-expand-md.navbar-dark").hide();
                    OJBIFRAME.find("#master-panel-menu").hide();
                    OJBIFRAME.find("#master-panel-menu").prev().hide();
                    AGLoading(false);
                }

                function successIframeLoad() {
                    AGLoading(false);
                }

                function afterSearch() {
                    $("#divSearch").show();
                    $("#tableItems").dataTable({
                        columnDefs: [{
                            "orderable": false,
                            "targets": [0]
                        }],
                        "order": [[3, "desc"]]
                    });

                    //$('html,body').animate({
                    //    scrollTop: $("#divSearch").offset().top - 50
                    //});
                }
                function AdjustWidthPanelTreeWhenOpenTab() {
                    setTimeout(function () {
                        try { AdjustWidthPanelTree(); } catch (e) { }
                    }, 100);
                }

                function bindingDataTableJSTicket() {
                    $("#tableListTicketItems").dataTable({
                        columnDefs: [{
                            "orderable": false,
                            "targets": [0]
                        }],
                        "order": [[1, "asc"]]
                    });
                }

                function openTicketNewWindow(CallerID) {
                    $("#hddCallerID").val(CallerID);
                    $("#btnOpenTicketNewWindow").click();
                }

                function bindingDataTableOwnerAssignment() {
                    var OwnerAssignList = JSON.parse($("#<%= panel_data_owner_assign.ClientID %>").html());

                    var data = [];
                    for (var i = 0 ; i < OwnerAssignList.length ; i++) {
                        var OwnerAssign = OwnerAssignList[i];

                        data.push([
                            OwnerAssign.LineNumber,
                            OwnerAssign.LineNumber,
                            OwnerAssign.LineNumber,
                            OwnerAssign.OwnerTypeDesc,
                            OwnerAssign.OwnerCodeDesc,
                            OwnerAssign.BeginDate,
                            OwnerAssign.EndDate,
                            OwnerAssign.SLAGroupDesc,
                            OwnerAssign.ActiveStatus,
                        ]);
                    }

                    $("#tableOwnerAssignment").dataTable({
                        data: data,
                        deferRender: true,
                        "order": [[2, "asc"]],
                        'columnDefs': [
                           {
                               "orderable": false,
                               'targets': 0,
                               'createdCell': function (td, cellData, rowData, row, col) {
                                   $(td).addClass("text-center text-nowrap");
                                   $(td).html(
                                       '<a class="AUTH_MODIFY" href="JavaScript:;">' +
                                            '<i class="fa fa-times text-danger"></i>' +
                                        '</a>'
                                    );
                                   $(td).addClass("c-pointer");
                                   $(td).bind("click", function () {
                                       $("#<%= hddOwnerAssign_LineNumber.ClientID %>").val(cellData);
                                       $("#<%= btnRemoveRowTableOwnerAssignment.ClientID %>").click();
                                       //openEquipment_PageCriteria(cellData, 'Edit')
                                   });
                               }
                           },
                           {
                               "orderable": false,
                               'targets': 1,
                               'createdCell': function (td, cellData, rowData, row, col) {
                                   $(td).addClass("text-center text-nowrap");
                                   $(td).html(
                                       '<a class="AUTH_MODIFY" href="JavaScript:;">' +
                                            '<i class="fa fa-pencil-square-o text-success"></i>' +
                                        '</a>'
                                    );
                                   $(td).addClass("c-pointer");
                                   $(td).bind("click", function () {
                                       $("#<%= hddOwnerAssign_LineNumber.ClientID %>").val(cellData);
                                       $("#<%= btnEditRowTableOwnerAssignment.ClientID %>").click();
                                   });
                               }
                           }
                        ]
                    });


                    $("#tableOwnerAssignment").dataTable();
                }

                function selectSearchHelpAssetSelect(obj) {
                    var assetCode = $(obj).attr("data-code");
                    var assetDesc = $(obj).attr("data-desc");
                    $("#<%= hddAccountAssignmentBox_AssetCode.ClientID %>").val(assetCode);
                    $("#<%= txtAccountAssignmentBox_AssetDesc.ClientID %>").val(assetCode + ' : ' + assetDesc);
                }
                function openSearchHelpAsset() {
                    $("#tableAssetList").dataTable();
                    $('#modal-searchHelpAsset').modal('show');
                }
            </script>
        </div>
    </div>
    <!-- Modal -->
    <div id="modal-diagram" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-body">
                    <iframe onload="modifyDisplayIframe(this);" style="border: none; height: calc(100vh - 150px); width: 100%" scrolling="no"></iframe>
                    <div class="text-right" style="margin-top: 10px;">
                        <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>

        </div>
    </div>
    
    <div id="modal-searchHelpAsset" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">

                <div class="modal-header">
                    <h4 class="modal-title">Search Help Asset</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpAssetList">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table id="tableAssetList" class="table table-bordered table-striped table-hover table-sm nowrap">
                                    <thead>
                                        <th>Select</th>
                                        <th>AssetCode</th>
                                        <th>Asset Description</th>
                                        <th>Quantity</th>
                                        <th>Uom Name</th>
                                        <th>Asset Value</th>
                                        <th>Receive Status</th>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater runat="server" ID="rptListAsset">
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="text-center">
                                                        <button type="button" class="btn btn-success btn-sm" data-dismiss="modal"
                                                            onclick="selectSearchHelpAssetSelect(this);"
                                                            data-code="<%# Eval("AssetCode") %>"
                                                            data-desc="<%# Eval("AssetSubCodeDescription") %>">
                                                            Select
                                                        </button>
                                                    </td>
                                                    <td><%# Eval("AssetCode") %></td>
                                                    <td><%# Eval("AssetSubCodeDescription") %></td>
                                                    <td class="text-right"><%# Convert.ToDecimal(Eval("Quantity")).ToString("#,##0.00") %></td>
                                                    <td><%# Eval("UomName") %></td>
                                                    <td class="text-right"><%# Convert.ToDecimal(Eval("AssetValue")).ToString("#,##0.00") %></td>
                                                    <td><%# Eval("StatusName") %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>


    <!-- The Modal -->
    <div class="modal" id="modal-owner-assignment">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <h4 class="modal-title">Owner Assignment</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>

                <!-- Modal body -->
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpDataOwnerAssign">
                        <ContentTemplate>
                            <div class="form-row">
                                <div class="form-group col-sm-12 col-md-12">
                                    <label>Item No.</label>
                                    <asp:TextBox Text="" runat="server" Enabled="false" ID="txtOwnerAssignmentBox_ItemNo" CssClass="form-control form-control-sm" />
                                </div>
                                <div class="form-group col-sm-12 col-md-6">
                                    <label>Owner Type</label>
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="ddlOwnerAssignmentBox_OwnerType" CssClass="form-control form-control-sm"
                                                onchange="$(this).next().click();">
                                                <asp:ListItem Text="เลือก" Value="" />
                                                <asp:ListItem Text="Customer" Value="01" />
                                                <asp:ListItem Text="Vendor" Value="02" />
                                                <asp:ListItem Text="Employee" Value="03" />
                                                <asp:ListItem Text="SoldToParty" Value="ST" />
                                                <asp:ListItem Text="ShipToParty" Value="SH" />
                                                <asp:ListItem Text="BillToParty" Value="BP" />
                                            </asp:DropDownList>
                                            <asp:Button Text="" runat="server" CssClass="hide" ID="btnOwnerAssignmentBox_LoadOwner"
                                                OnClick="btnOwnerAssignmentBox_LoadOwner_Click" OnClientClick="AGLoading(true);" />
                                            <asp:HiddenField runat="server" ID="hddOwnerAssignmentBox_OwnerType" Value="" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="form-group col-sm-12 col-md-6">
                                    <label>Owner Desc</label>
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpBox_CustomerSelect">
                                        <ContentTemplate>
                                            <sna:AutoCompleteControl runat="server" id="Complete_OwnerAssignmentBox_CustomerSelect" CssClass="form-control form-control-sm" />
                                            <%--<asp:HiddenField runat="server" ID="hddOwnerAssignmentBox_OwnerCode" Value="" />--%>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="form-group col-sm-12 col-md-6">
                                    <label>Begin Date</label>
                                    <div class="input-group">
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="txtOwnerAssignmentBox_BeginDate" Text="" />
                                        <span class="input-group-append hand">
                                            <i class="input-group-text fa fa-calendar"></i>
                                        </span>
                                    </div>
                                </div>
                                <div class="form-group col-sm-12 col-md-6">
                                    <label>End Date</label>
                                    <div class="input-group">
                                        <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="txtOwnerAssignmentBox_EndDate" Text="" />
                                        <span class="input-group-append hand">
                                            <i class="input-group-text fa fa-calendar"></i>
                                        </span>
                                    </div>
                                </div>
                                <div class="form-group col-sm-12 col-md-6">
                                    <label>SLA Group</label>
                                    <asp:DropDownList ID="ddlSLAGroup" runat="server" CssClass="form-control form-control-sm"
                                        DataValueField="Key" DataTextField="Value">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group col-sm-12 col-md-6">
                                    <label>&nbsp;</label>
                                    <div>
                                        <asp:CheckBox runat="server" ID="chkOwnerAssignmentBox_ActiveStatus" Text="Active" />
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <!-- Modal footer -->
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button Text="Confirm" ID="btnSaveOwnerAssignmentModal" runat="server" CssClass="btn btn-success"
                                OnClick="btnSaveOwnerAssignmentModal_Click" OnClientClick="AGLoading(true);" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

            </div>
        </div>
    </div>

    <div id="sale-after-attachfile" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <asp:UpdatePanel ID="updAttachFile" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title">
                                <asp:Label ID="lbHeadAttach" runat="server"></asp:Label>
                            </h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <sna:AttachFileUserControl ID="AttachFileUserControl" runat="server" display="true"></sna:AttachFileUserControl>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

</asp:Content>
