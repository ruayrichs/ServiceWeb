<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="AssetMasterDetail.aspx.cs" Inherits="ServiceWeb.crm.Master.Asset.AssetMasterDetail" %>

<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEmployee.ascx" TagPrefix="uc1" TagName="AutoCompleteEmployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/js/appleGallery/appleGallery.css" rel="stylesheet" />
    <script src="/js/appleGallery/appleGallery.js?vs=1.001"></script>
    <link href="/UserControl/AGapeGallery/agape-gallery-3.0.css" rel="stylesheet" />
    <script src="/UserControl/AGapeGallery/agape-gallery-3.0.js"></script>
    <style type="text/css">
        .ajax__tab_xp .ajax__tab_tab {
            height: auto;
        }

        .row {
            margin-top: 0px;
        }

        .col-item {
            border: 1px solid #E1E1E1;
            border-radius: 5px;
            background: #FFF;
        }

            .col-item .photo img {
                margin: 0 auto;
            }

            .col-item .info {
                padding: 10px;
                border-radius: 0 0 5px 5px;
                margin-top: 1px;
            }

            .col-item:hover .info {
                background-color: #F5F5DC;
            }

            .col-item .price {
                /*width: 50%;*/
                float: left;
                margin-top: 5px;
            }

                .col-item .price h5 {
                    line-height: 20px;
                    margin: 0;
                }

        .price-text-color {
            color: #219FD1;
        }

        .col-item .info .rating {
            color: #777;
        }

        .col-item .rating {
            /*width: 50%;*/
            float: left;
            font-size: 17px;
            text-align: right;
            line-height: 52px;
            margin-bottom: 10px;
            height: 52px;
        }

        .col-item .separator {
            border-top: 1px solid #E1E1E1;
        }

        .clear-left {
            clear: left;
        }

        .col-item .separator p {
            line-height: 20px;
            margin-bottom: 0;
            margin-top: 10px;
            text-align: center;
        }

            .col-item .separator p i {
                margin-right: 5px;
            }

        .col-item .btn-add {
            width: 50%;
            float: left;
        }

        .col-item .btn-add {
            border-right: 1px solid #E1E1E1;
        }

        .col-item .btn-details {
            width: 50%;
            float: left;
            padding-left: 10px;
        }

        .controls {
            margin-top: 20px;
        }

        .fa-fw {
            width: 2em;
            font-size: 1.5em;
        }

        form {
            background-color: White;
        }

        hr {
            margin-top: 0px;
        }

        .mainTopSpace {
            padding: 10px;
            background-color: White;
        }

        .content {
            background-color: White;
        }

        /*.white-box {
            padding: 20px;
            background: #fff;
            border: 1px solid #ccc;
            border-radius: 5px;
        }*/

        .glyphicon {
            padding-right: 10px;
        }

        .table-survey {
            width: 100%;
            table-layout: fixed;
            border: solid 1px #ccc;
            box-shadow: 5px 5px 5px #ccc;
        }

            .table-survey td, .table-survey th {
                padding: 10px;
            }

        .question-zone {
            background: #eee;
            width: 15%;
            padding-left: 15px !important;
        }

        .answer-zone {
            border-bottom: 1px solid #ccc;
            vertical-align: top;
        }

        .requireField {
            background: #f3fca1;
        }

        .grey-box {
            padding: 20px;
            background: #eee;
            border: 1px solid #ccc;
            border-radius: 5px;
        }

        .datepicker, .bootstrap-timepicker-widget {
            z-index: 1000000;
        }
    </style>

    <script type="text/javascript">
        function ini() {
            //$('#txtAssetReceiveDate').kendoDatePicker({ format: "dd/MM/yyyy" });

            var navItems = $('.admin-menu li > a');
            var navListItems = $('.admin-menu li');
            var allWells = $('.admin-content');
            var allWellsExceptFirst = $('.admin-content:not(:first)');

            allWellsExceptFirst.hide();
            navItems.click(function (e) {
                e.preventDefault();
                navListItems.removeClass('active');
                $(this).closest('li').addClass('active');
                var _id = $(this).closest('li').attr("class").replace('active', '').trim();

                $("#hdtab").val(_id);
                allWells.hide();
                var target = $(this).attr('data-target-id');
                $('#' + target).show();
            });
            navListItems.removeClass('active');

            if ($("#hdtab").val() != '') {
                $('.' + $("#hdtab").val()).addClass('active');
                allWells.hide();
                $('#' + $("#hdtab").val()).show();
            }
            else {
                $('[data-target-id="header"]').closest('li').addClass('active');
            }
        }
        $(function () {
            ini();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ini);
        });
        
        function openTaskClick(aobjectlink, snaid) {
            window.open('<%= Page.ResolveUrl("~/TimeAttendance/ActivityManagementReDesign.aspx?aobj=' + aobjectlink + '&snaid=' + snaid + '") %>');
        }
        function bindingGallery(data, aobjectlink) {
            $("#assetGellery").html("");
            $("#assetGellery").appleGallery({
                gelleryData: data,
                aobjectlink: aobjectlink,
                onAfterSave: function (docno) {
                    __doPostBack('<%= udpnPicture.ClientID %>', docno);
                }
            });
                $("#assetGellery").width("100%");
            }
            function openGallery(index, docno, title) {
                var images = [];
                $(".timeline-image-img-" + docno).each(function () {
                    images.push({
                        url: $(this).prop("src"),
                        detail: $(this).prop("alt")
                    });
                });
                var image_gallery = $("<div />", {
                    id: "image-gallery"
                });
                $("#image-gallery-parent").append(image_gallery);
                $(image_gallery).aGepeGallery({
                    title: title,
                    index: index,
                    images: images
                });
            }

            function convertFormatNumber(obj, digit) {
                var formatValue = new NumberFormat($(obj).val());
                formatValue.setPlaces(digit);
                $(obj).val(formatValue.toFormatted());
            }
    </script>

    <div id="image-gallery-parent">
        <%-- <div id="image-gallery"></div>--%>
    </div>
   

    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <a class="btn btn-warning mb-1" style="color:#212529;" href="AssetMasterCriteria.aspx"><i class="fa fa-arrow-circle-left"></i>&nbsp;&nbsp;Back</a>
                  
                    <%if(isCreateMode){ %>
                    <button type="button" id="btnCreateDataTran" runat="server" class="btn btn-success mb-1" onclick="$(this).next().click();">
                        <i class="fa fa-save"></i>&nbsp;&nbsp;Create
                    </button>
                     <asp:Button ID="btnCreateAsset" Text="Create" CssClass="btn btn-success mb-1 d-none" OnClick="btnSeveCreateAsset_Click" runat="server" OnClientClick="return AGConfirm('ยืนยันการบันทึก');" />
                    <% } else { %>
                    <button type="button" id="btnSaveDataTran" runat="server" class="btn btn-primary mb-1 " onclick="$(this).next().click();">
                        <i class="fa fa-save"></i>&nbsp;&nbsp;Save
                    </button>
                     <asp:Button ID="btnSaveAsset" Text="Save" CssClass="btn btn-primary mb-1 d-none" OnClick="btnSaveEditeAsset_Click" runat="server" OnClientClick="return AGConfirm('ยืนยันการบันทึก');" />

                    <% } %>

                    
                   
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>
    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                   <span class="h5 mb-0">Asset Master Detail</span>
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
                    <asp:HiddenField ID="hdtab" runat="server" ClientIDMode="Static" />
                    <nav>
                        <div class="nav nav-tabs" id="nav-tab" role="tablist">
                            <a class="nav-item nav-link active" id="nav-header-tab" data-toggle="tab" href="#header" role="tab" aria-controls="nav-header" aria-selected="true">General</a>
                            <a class="nav-item nav-link" id="nav-value-tab" data-toggle="tab" href="#Value" role="tab" aria-controls="nav-Value" aria-selected="true">Value</a>
                            <a class="nav-item nav-link" id="nav-pictureandfile-tab" data-toggle="tab" href="#PictureandFile" role="tab" aria-controls="nav-PictureandFile" aria-selected="true">Picture</a>
                            <a class="nav-item nav-link" id="nav-relation-tab" data-toggle="tab" href="#Relation" role="tab" aria-controls="nav-Relation" aria-selected="true">Relation</a>
                        </div>
                    </nav>
                    <div class="tab-content p-3" id="nav-tabContent">
                        <div class="tab-pane fade show active" id="header" role="tabpanel" aria-labelledby="nav-header-tab">
                            <asp:UpdatePanel ID="updateDataAssetMaster" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="form-row" style="margin: 10px; margin-top: 20px;">
                                        <div class="col-md-12">
                                            <div class="form-row">
                                                <div class="form-group col-lg-6">
                                                    <label>Asset Group</label>
                                                    <asp:DropDownList ID="ddlAssetGroup" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="Description" DataValueField="AssetGroup" Enabled="false">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="form-group col-lg-6">
                                                    <label>Branch</label>
                                                    <asp:DropDownList ID="ddlBranch" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="Description" DataValueField="BusinessAreaCode" Enabled="false">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="form-group col-lg-6">
                                                    <label>Asset Type</label>
                                                    <asp:DropDownList ID="ddlAssetType" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="GroupName" DataValueField="GroupCode">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="form-group col-lg-6">
                                                    <label>Asset Category 1</label>
                                                    <asp:DropDownList ID="ddlAssetCategory1" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="Description" DataValueField="AssetCategory">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="form-group col-lg-6">
                                                    <label>Asset Code</label>
                                                    <asp:TextBox ID="txtAssetCode" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" Enabled="false" />
                                                </div>
                                                <div class="form-group col-lg-6">
                                                    <label>Asset Category 2</label>
                                                    <asp:DropDownList ID="ddlAssetCategory2" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="Description" DataValueField="AssetCategory">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="form-group col-lg-6">
                                                    <label>Asset Name</label>
                                                    <asp:TextBox ID="txtAssetName" CssClass="form-control form-control-sm required" ClientIDMode="Static" runat="server" />
                                                </div>
                                                <div class="form-group col-lg-6">
                                                    <label>Location 1</label>
                                                    <asp:DropDownList ID="ddlLocation1" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="Description" DataValueField="AssetLocation">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="form-group col-lg-6">
                                                    <label>Owner</label>
                                                    <uc1:AutoCompleteEmployee runat="server" id="ddlOwner" CssClass="form-control form-control-sm" />
                                                </div>
                                                <div class="form-group col-lg-6">
                                                    <label>Location 2</label>
                                                    <asp:DropDownList ID="ddlLocation2" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="Description" DataValueField="AssetLocation2">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="form-group col-lg-6">
                                                    <label>Department</label>
                                                    <asp:DropDownList ID="ddlDepartment" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="departmentName" DataValueField="departmentCode">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="form-group col-lg-6">
                                                    <label>Room</label>
                                                    <asp:DropDownList ID="ddlRoom" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="Description" DataValueField="AssetRoom">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="form-group col-lg-6">
                                                    <label>Cost Center</label>
                                                     <asp:DropDownList ID="ddlCostCenter" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server" DataTextField="COSTCENTERNAME" DataValueField="COSCENTERCODE">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                        <div class="tab-pane fade" id="Value" role="tabpanel" aria-labelledby="nav-item-tab">
                            <asp:UpdatePanel ID="updatePanelValue" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="form-row" style="margin: 10px; margin-top: 10px;">
                                        <div class="col-md-12">
                                            <div class="form-row">
                                                <div class="form-group col-lg-6">
                                                    <label>Asset Value</label>
                                                    <asp:TextBox ID="txtAssetValue" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server"
                                                        onkeypress="return isNumberKey(event);" onchange="convertFormatNumber(this,2);" Style="text-align: right;" />
                                                </div>
                                                <div class="form-group col-lg-6">
                                                    <label>Currency</label>
                                                    <asp:DropDownList ID="ddlCurrency" CssClass="form-control form-control-sm" ClientIDMode="Static" runat="server"
                                                        DataTextField="Description" DataValueField="CurrencyCode">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="form-group col-lg-6">
                                                    <label>Asset Receive Date</label>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtAssetReceiveDate" ClientIDMode="Static" runat="server" CssClass="form-control form-control-sm required date-picker" />
                                                        <span class="input-group-append hand">
                                                            <i class="fa fa-calendar input-group-text"></i>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                        <div class="tab-pane fade" id="PictureandFile" role="tabpanel" aria-labelledby="nav-item-tab">
                            <asp:UpdatePanel ID="udpnPicture" runat="server" UpdateMode="Conditional" OnLoad="udpnPicture_Load">
                                <ContentTemplate>
                                    <div class="form-group col-md-12 content-bg">
                                        <div class="form-row" style="margin-top: 0px;">
                                            <div id="assetGellery"></div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                        <div class="tab-pane fade" id="Relation" role="tabpanel" aria-labelledby="nav-item-tab">
                            <style>
                                .relation-asset {
                                    position: absolute;
                                    border-left: 2px solid #aaa;
                                    border-bottom: 2px solid #aaa;
                                    height: 45px;
                                    width: 20px;
                                    margin-top: -10px;
                                    margin-left: 20px;
                                }
                            </style>

                            <asp:Repeater runat="server" ID="rptRelation">
                                <ItemTemplate>
                                    <div style="<%# LevelNodeRelation(Eval("Relation").ToString()) %>">
                                        <asp:LinkButton runat="server" ID="linkAssetNode" OnClick="linkAssetNode_Click"
                                            CommandName='<%# Eval("AssetCode") %>' Enabled='<%# !Eval("Relation").Equals("This") %>'>
                                        <div class="mat-box" style="border-left: 3px solid #aaa;">
                                            <div style="float:right;">
                                                <b><%# Eval("Relation") %></b>
                                            </div>
                                            <div class="one-line">
                                                <b>Asset Code : </b><%# Eval("AssetCode") %>
                                            </div>
                                            <div class="one-line">
                                                <b>Asset Name : </b><%# Eval("AssetName") %>
                                            </div>
                                        </div>
                                        </asp:LinkButton>
                                    </div>
                                    <div class="relation-asset" style="<%# LevelNodeRelationLine(Eval("Relation").ToString(), Container.ItemIndex) %>"></div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <% if (rptRelation.Items.Count == 0)
                               { %>
                            <div class="alert alert-info">
                                No Relation
                            </div>
                            <% } %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
