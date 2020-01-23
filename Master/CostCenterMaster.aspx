<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRMMasterPage.master" AutoEventWireup="true" CodeBehind="CostCenterMaster.aspx.cs" Inherits="ServiceWeb.Master.CostCenterMaster" %>

<%@ Register Src="~/widget/usercontrol/AutoCompleteControl.ascx" TagPrefix="uc1" TagName="AutoCompleteControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="/Lib-tablemodel/FeasibilityFinancialProjectionV2.css?vs=20180720" rel="stylesheet">

    <style>
        .table-finans-none-cell {
            border-top: none !important;
            border-left: none !important;
            background-color: #fff !important;
        }

        .table-finans-set-cell {
            background-color: #fff !important;
            border: none !important;
        }

        .none-border-bottom {
            border-bottom: none !important;
        }

        #table-cost-center-header .row-tbody:last-child {
            border-bottom: 1px solid #ccc;
        }

        .form-check.form-check-inline > input {
            position: static;
            margin-top: 0;
            margin-right: .3125rem;
            margin-left: 0;
        }

        .twitter-typeahead {
            padding: 0 !important;
            height: 25px;
        }

        .tt-menu {
            white-space: normal !important;
            bottom: 100% !important;
            top: auto !important;
        }

        #panel-table {
            overflow-x: initial;
            -webkit-overflow-scrolling: initial;
            -ms-overflow-style: initial;
        }

        .input-group-mat > div {
            display: inline-block;
        }

            .input-group-mat > div:first-child {
                display: none;
                width: 30px;
                text-align: center;
                background-color: #fff;
                border-right: 1px solid #ccc;
            }

            .input-group-mat > div:last-child {
                width: calc(100% - 0px);
            }

        .input-group-mat.row-header > div:first-child {
            display: inline-block;
            height: 27px;
            padding-top: 5px;
            line-height: 0;
        }

        .input-group-mat.row-header > div:last-child {
            width: calc(100% - 35px);
        }
    </style>

    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <a class="btn btn-warning mb-1" href="CostCenterCriteria.aspx"><i class="fa fa-arrow-circle-left"></i>&nbsp;&nbsp;Back</a>
                    <button type="button" class="btn btn-primary mb-1 AUTH_MODIFY" onclick="saveDataCostCenter(this);"><i class="fa fa-save"></i>&nbsp;&nbsp;Save</button>
                    <asp:Button ID="btnSaveData" runat="server" CssClass="d-none"
                        OnClick="btnSaveData_Click" />
                    <button type="button" class="btn btn-success mb-1<%= string.IsNullOrEmpty(CostCenterID) ? " d-none" : "" %> AUTH_MODIFY" onclick="createBomClick();"><i class="fa fa-cubes"></i>&nbsp;&nbsp;<%= GetSaveBOMDescription() %></button>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>

    <div class="row">
        <div class="col">
            <div id="cost-sheet-content" class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">BOM Cost Sheet</h5>
                </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="form-group col-lg-3 col-sm-4">
                            <label>Cost Sheet ID</label>
                            <asp:TextBox ID="txtID" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="form-group col-lg-3 col-sm-4">
                            <label>Valid From</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtValidFrom" placeholder="dd/mm/yyyy" runat="server" CssClass="form-control form-control-sm date-picker required"></asp:TextBox>
                                <div class="input-group-append">
                                    <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-lg-3 col-sm-4">
                            <label>Valid To</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtValidTo" placeholder="dd/mm/yyyy" runat="server" CssClass="form-control form-control-sm date-picker required"></asp:TextBox>
                                <div class="input-group-append">
                                    <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label>Subject</label>
                        <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm required" ID="txtSubject" />
                    </div>
                    <div class="form-group">
                        <label>
                            Remark
                        </label>
                        <asp:TextBox runat="server" placeholder="Text" CssClass="form-control form-control-sm" TextMode="MultiLine" ID="txtRemark" />
                    </div>

                    <div class="form-row">
                        <div class="col-md-12">
                            <div class="table-responsive" id="panel-table">
                                <table class="table-finans-v2" id="table-cost-center-header" style="width: calc(100% - 1px);">
                                    <thead>
                                        <tr class="table-finans-set-cell">
                                            <th class="table-finans-set-cell" style="width: 30px;"></th>
                                            <th class="table-finans-set-cell" style="width: 30px;"></th>
                                            <th class="table-finans-set-cell"></th>
                                            <th class="table-finans-set-cell" style="width: 80px;"></th>
                                            <th class="table-finans-set-cell" style="width: 80px;"></th>
                                            <th class="table-finans-set-cell" style="width: 100px;"></th>
                                            <th class="table-finans-set-cell row-cost-master" style="width: 55px;"></th>
                                            <th class="table-finans-set-cell" style="width: 120px;"></th>
                                            <th class="table-finans-set-cell" style="width: 90px;"></th>
                                            <th class="table-finans-set-cell" style="width: 100px;"></th>
                                            <th class="table-finans-set-cell" style="width: 120px;"></th>
                                        </tr>
                                        <tr class="row-summary border-dark">
                                            <th class="table-finans-none-cell" colspan="3" style="border-bottom: none"></th>
                                            <th class="text-nowrap text-right bg-dark border-dark text-white" colspan="3">
                                                <span>Total Amount</span>
                                            </th>
                                            <th id="th-total-amount" class="text-right bg-dark border-dark text-white" style="padding: 0px; padding-right: 3px;" colspan="2">
                                                <asp:Label ID="lbTotalAmount" runat="server" CssClass="lbl-total-amount" Text="0.00"></asp:Label>
                                            </th>
                                            <th class="text-nowrap bg-dark border-dark text-white" colspan="3">
                                                <asp:Label ID="lbCurrency" runat="server" Text="THB"></asp:Label>
                                            </th>
                                        </tr>
                                        <tr class="row-cost-master border-secondary">
                                            <th class="table-finans-none-cell" colspan="3" style="border-bottom: none"></th>
                                            <th class="text-nowrap text-right bg-secondary border-secondary text-white" colspan="3">
                                                <span>Contract</span>
                                            </th>
                                            <th class="text-nowrap text-right bg-secondary border-secondary"></th>
                                            <th class="text-center border-secondary" style="padding: 0px;">
                                                <asp:TextBox runat="server" ID="txtContractMonth" CssClass="text-right txt-contract_month" ClientIDMode="Static"
                                                    Text="0" onkeypress="return isNumberKeyV2(event);" autocomplete="off" onchange="calculatorContractMonth(this);"
                                                    Style="height: 25px;" />
                                            </th>
                                            <th class="text-nowrap bg-secondary border-secondary text-white" colspan="3">
                                                <span>Month</span>
                                            </th>
                                        </tr>
                                        <tr class="">
                                            <th class="table-finans-none-cell align-middle" colspan="3">
                                                <asp:RadioButton ID="rdoOneTime" runat="server" GroupName="group1" Text="One Time" CssClass="form-check form-check-inline is-recurring" Checked="true" onclick="setIsRecurring();" />
                                                <asp:RadioButton ID="rdoRecurring" runat="server" GroupName="group1" Text="Recurring" CssClass="form-check form-check-inline is-recurring" onclick="setIsRecurring();" />
                                            </th>
                                            <th class="text-center" style="width: 80px;">Qty</th>
                                            <th class="text-center" style="width: 80px;">UOM</th>
                                            <th class="text-center" style="width: 100px;">Unit Price</th>
                                            <th class="text-center row-cost-master" style="width: 55px;">Per Month</th>
                                            <th class="text-center" style="width: 120px;">Amount</th>
                                            <th class="text-center" style="width: 90px;">Contribution Margin<%-- (%)--%></th>
                                            <th class="text-center" style="width: 100px;">Sell Unit Price</th>
                                            <th class="text-center" style="width: 120px; border-right: 1px solid #ccc;">Actual Cost</th>
                                        </tr>
                                    </thead>
                                    <tfoot class="btn-new-row-header">
                                        <tr>
                                            <td colspan="11">
                                                <div style="padding-left: 4.5px;">
                                                    <i class="fa fa-plus-square-o text-success fa-fw" data-level="0"
                                                        style="cursor: pointer;" onclick="insertRowHeader(this);"></i>
                                                </div>
                                            </td>
                                        </tr>
                                    </tfoot>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modal-create-bom" class="modal fade" role="dialog">
        <div class="modal-dialog" style="min-width: 95%; max-width: 95%;">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title"><%= GetSaveBOMDescription() %></h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="udpnBom" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>

                            <asp:Button ID="btnBindingBOM" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnBindingBOM_Click" />

                            <%--<div class="form-row">
                                <div class="form-group col-md-6">
                                    <label>Plant</label>
                                    <asp:DropDownList ID="ddlPlant" runat="server" CssClass="form-control form-control-sm required"></asp:DropDownList>
                                </div>
                                <div class="form-group col-md-2">
                                    <label>Version</label>
                                    <asp:TextBox ID="tbVersion" runat="server" CssClass="form-control form-control-sm required" Enabled="false"></asp:TextBox>
                                </div>
                            </div>--%>
                            <div class="form-row">
                                <div class="form-group col-md-4">
                                    <label>Current Price</label>
                                    <asp:DropDownList ID="ddlPrice" runat="server" CssClass="form-control form-control-sm">
                                        <asp:ListItem Value="00" Text="BOM"></asp:ListItem>
                                        <asp:ListItem Value="01" Text="Material"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group col-md-4">
                                    <label>Stock Keeping</label>
                                    <asp:DropDownList ID="ddlStock" runat="server" CssClass="form-control form-control-sm">
                                        <asp:ListItem Value="00" Text="BOM"></asp:ListItem>
                                        <asp:ListItem Value="01" Text="Material"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group col-md-4">
                                    <label>Material Cost</label>
                                    <asp:DropDownList ID="ddlCost" runat="server" CssClass="form-control form-control-sm">
                                        <asp:ListItem Value="00" Text="BOM"></asp:ListItem>
                                        <asp:ListItem Value="01" Text="Material"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <hr class="mt-0" />

                            <div class="table-responsive">
                                <table class="table-finans-v2" style="width: calc(100% - 1px);">
                                    <thead>
                                        <tr>
                                            <th class="table-finans-none-cell"></th>
                                            <th class="text-center" style="width: 80px;">Version</th>
                                            <th class="text-nowrap">Plant</th>
                                            <th class="text-center" style="width: 80px;">Qty</th>
                                            <th class="text-center" style="width: 80px;">UOM</th>
                                            <th class="text-center" style="width: 100px;">Unit Price</th>
                                            <th class="text-center" style="width: 120px;">Amount</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptBom" runat="server" OnItemDataBound="rptBom_ItemDataBound">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="hdfNodeCode" runat="server" Value='<%# Eval("NodeCode") %>' />
                                                        <asp:HiddenField ID="hdfNodeLevel" runat="server" Value='<%# Eval("NodeLevel") %>' />
                                                        <asp:HiddenField ID="hdfMaterial" runat="server" Value='<%# Eval("MaterialCode") %>' />
                                                        <asp:HiddenField ID="hdfPlant" runat="server" Value='<%# Eval("Plant") %>' />
                                                        <span style="<%# Eval("NodeLevel").ToString() == "0" ? "": "padding-left: 30px;" %>"><%# Eval("MaterialCode") + " : " + Eval("MaterialName") %></span>
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:Label ID="lbVersion" runat="server" Text='<%# Eval("NodeLevel").ToString() == "0" ? Eval("BomVersion") : "" %>'></asp:Label>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <asp:DropDownList ID="ddlPlant" runat="server" DataValueField="Plant" DataTextField="PlantDesc"></asp:DropDownList>
                                                    </td>
                                                    <td class="text-right">
                                                        <span><%# Eval("NodeLevel").ToString() == "0" ? "" : Eval("Unit", "{0:N2}") %></span>
                                                    </td>
                                                    <td>
                                                        <span><%# Eval("NodeLevel").ToString() == "0" ? "" : Eval("UOM") %></span>
                                                    </td>
                                                    <td class="text-right">
                                                        <span><%# Eval("NodeLevel").ToString() == "0" ? "" : Eval("UnitPrice", "{0:N2}") %></span>
                                                    </td>
                                                    <td class="text-right">
                                                        <span><%# Eval("Amount", "{0:N2}") %></span>
                                                    </td>
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
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <button type="button" class="btn btn-danger mb-1" data-dismiss="modal">
                                <i class="fa fa-times fa-fw"></i>
                                Close
                            </button>
                            <button type="button" class="btn btn-success mb-1" onclick="validateCreateBOM(this);">
                                <i class="fa fa-check-square-o fa-fw"></i>
                                Save
                            </button>
                            <asp:Button ID="btnCreateBOM" runat="server" CssClass="d-none" OnClick="btnCreateBOM_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="hide" id="row-template">
        <table class="table-finans-v2">
            <tbody class="row-tbody">
                <tr>
                    <td colspan="11" style="padding: 0; border-bottom: none; border-left: none;">
                        <table class="table-finans-v2" style="margin-left: 0px; width: calc(100% - 0px);">
                            <tbody>
                                <tr class="row-cost-item" style="border-right: none;"
                                    data-rowid="" data-rowlevel="" data-rowparentid="">
                                    <td class="text-nowrap text-center" style="width: 30px;">
                                        <i class="fa fa-plus-square-o text-success fa-fw btn-new-row" data-level=""
                                            style="cursor: pointer;" onclick="insertRowItem(this);"></i>
                                    </td>
                                    <td class="text-nowrap text-center" style="width: 30px;">
                                        <i class="fa fa-trash-o text-danger c-pointer" onclick="removeRowItem(this);"></i>
                                    </td>
                                    <td class="text-nowrap" style="padding: 0px;">
                                        <div class="input-group-mat">
                                            <div style="display: none;">
                                                <input type="checkbox" name="chkAmountManual" id="chkAmountManual" class="chkAmountManual" value="" onchange="checkedChangePriceHeader(this);" />
                                            </div>
                                            <div>
                                                <input type="text" name="AutoCompleteMaterial-Search" id="AutoCompleteMaterial-Search"
                                                    class="AutoCompleteMaterial-Search" value="" autocomplete="off" />
                                                <input type="hidden" name="AutoCompleteMaterial-Text" id="AutoCompleteMaterial-Text"
                                                    class="AutoCompleteMaterial-Text" value="" />
                                                <input type="hidden" name="AutoCompleteMaterial-Value" id="AutoCompleteMaterial-Value"
                                                    class="AutoCompleteMaterial-Value" value="" />
                                            </div>
                                        </div>
                                        <select name="ddlItemMaterial" id="ddlItemMaterial" class="ddl-mat hide"
                                            onchange="selectMat(this);">
                                            <option value="">เลือก</option>
                                            <asp:Repeater runat="server" ID="rptListMaterial">
                                                <ItemTemplate>
                                                    <option value="<%# Eval("ItmNumber") %>" data-uom="<%# Eval("BaseUOM") %>">
                                                        <%# Eval("ItmDescription") %>
                                                    </option>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </select>
                                    </td>
                                    <td class="text-nowrap text-right" style="width: 80px;">
                                        <input type="text" name="name" value="0.00" class="text-right txt-item-unit" onkeypress="return isNumberKeyV2(event);" autocomplete="off" onchange="calculatorRow(this);">
                                    </td>
                                    <td class="text-nowrap" style="width: 80px;">
                                        <span class="lbl-item-uom" style="padding: 2px 7px;"></span>
                                        <select class="ddl-item-uom" style="display: none;"></select>
                                    </td>
                                    <td class="text-nowrap text-right" style="width: 100px;">
                                        <span class="txt-item-unit_price">0.00</span>
                                        <%--<input type="text" name="name" value="0" class="text-right txt-item-unit_price" onkeypress="return isNumberKeyV2(event);" autocomplete="off" onchange="calculatorRow(this);">--%>
                                    </td>
                                    <td class="text-nowrap text-right row-cost-master" style="width: 55px;">
                                        <input type="text" value="0" class="text-right txt-item-permonth" onkeypress="return isNumberKeyV2(event);" autocomplete="off" onchange="calculatorRow(this);">
                                    </td>
                                    <td class="text-nowrap text-right" style="width: 120px;">
                                        <span class="lbl-item-price">0.00</span>
                                        <input type="text" value="0.00" class="text-right txt-item-price hide" autocomplete="off"
                                            onkeypress="return isNumberKeyV2(event);" onchange="validateAmountRowHeader(this);" />
                                    </td>
                                    <td class="text-nowrap text-right" style="width: 90px; border-right: none;">
                                        <%--<input type="text" value="0.00" class="text-right txt-item-contribution" onkeypress="return isNumberKeyV2(event, true);" autocomplete="off" onchange="calculatorRow(this);">--%>
                                        <span class="lbl-item-contribution">0.00</span>
                                    </td>
                                    <td class="text-nowrap text-right" style="width: 100px; border-right: none;">
                                        <%--<span class="lbl-item-sellunitprice">0.00</span>--%>
                                        <input type="text" value="0.00" class="text-right txt-item-sellunitprice" onkeypress="return isNumberKeyV2(event);" autocomplete="off" onchange="sellUnitPriceChange(this);">
                                    </td>
                                    <td class="text-nowrap text-right" style="width: 120px; border-right: none;">
                                        <span class="lbl-item-actual_cost">0.00</span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <div class="hide">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpDataSource">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hddJsonCostCenter" ClientIDMode="Static" />
                <asp:HiddenField runat="server" ID="hddJsonMat" ClientIDMode="Static" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script>

        $(document).ready(function () {
            setIsRecurring();
        });

        function saveDataCostCenter(sender) {
            var msg = "";
            $("#cost-sheet-content").find(".required").each(function () {
                if ($(this).val() == "") {
                    msg += msg == "" ? "" : "<br/>";
                    msg += "Please fill out " + $(this).closest(".form-group").find("label").first().html() + ".";
                }
            });

            if (msg != "") {
                AGError(msg);
            } else {
                if (AGConfirm(sender, 'Confirm save data')) {
                    AGLoading(true);
                    getJsonValue();
                    $(sender).next().click();
                }
            }
        }

        function selectMat(obj) {
            //var val = $(obj).val();
            //var text = $(obj).find(":selected").text();
            //var uom = $(obj).find(":selected").attr("data-uom");

            //var row = $(obj).closest('tr');
            //row.find(".lbl-item-uom").html(uom);
        }

        function getJsonValue() {
            var data = [];
            $("#table-cost-center-header .row-cost-item").each(function () {

                var NodeCode = $(this).attr("data-rowid");
                var NodeParentCode = $(this).attr("data-rowparentid");
                var NodeLevel = $(this).attr("data-rowlevel");
                var MaterialCode = $(this).find(".AutoCompleteMaterial-Value:first").val();
                var Unit = $(this).find(".txt-item-unit:first").val();
                var UnitPrice = $(this).find(".txt-item-unit_price:first").html();
                var UOM = $(this).find(".lbl-item-uom:first").html();
                ////var ContributionMargin = $(this).find(".txt-item-contribution:first").val();
                var ContributionMargin = $(this).find(".lbl-item-contribution:first").html();
                var ContractMonth = $(this).find(".txt-item-permonth:first").val();

                var Amount = $(this).find(".lbl-item-price:first").html();
                var IsAmountManual = $(this).find(".chkAmountManual:first").prop("checked");
                if (IsAmountManual) {
                    Amount = $(this).find(".txt-item-price:first").val();
                }

                data.push({
                    NodeCode: NodeCode,
                    NodeParentCode: NodeParentCode,
                    NodeLevel: NodeLevel,
                    MaterialCode: MaterialCode,
                    Unit: Unit,
                    UnitPrice: UnitPrice,
                    UOM: UOM,
                    ContributionMargin: ContributionMargin,
                    Amount: Amount,
                    IsAmountManual: IsAmountManual,
                    ContractMonth: ContractMonth
                });
            });

            $("#hddJsonCostCenter").val(JSON.stringify(data));
            return data;
        }

        function bindCostCenterStructure() {
            var datas = JSON.parse($("#hddJsonCostCenter").val());

            for (var i = 0; i < datas.length; i++) {
                var data = datas[i];

                var NewRow = $("#row-template .row-tbody").clone();
                NewRow.find(".btn-new-row").attr("data-level", data.NodeLevel + 1);
                NewRow.find(".row-cost-item").attr("data-rowid", data.NodeCode);
                NewRow.find(".row-cost-item").attr("data-rowlevel", data.NodeLevel);
                NewRow.find(".row-cost-item").attr("data-rowparentid", data.NodeParentCode);

                //$(NewRow).find(".ddl-mat:first").val(data.MaterialCode);

                $(NewRow).find(".txt-item-unit:first").val(data.Unit);
                $(NewRow).find(".lbl-item-uom:first").html(data.UOM);
                $(NewRow).find(".txt-item-unit_price:first").html(numberFormat(data.UnitPrice));
                $(NewRow).find(".txt-item-permonth:first").val(data.ContractMonth);
                ////$(NewRow).find(".txt-item-contribution:first").val(numberFormat(data.ContributionMargin));                
                $(NewRow).find(".lbl-item-contribution:first").html(numberFormat(data.ContributionMargin));

                selectMat($(NewRow).find(".ddl-mat")[0]);

                if (data.NodeLevel == 0) {
                    $(NewRow).find(".row-cost-item").find(".input-group-mat").addClass("row-header");
                    $(NewRow).find(".row-cost-item").find(".chkAmountManual").prop("checked", data.IsAmountManual);

                    if (data.IsAmountManual) {
                        $(NewRow).find(".row-cost-item").find(".txt-item-price").attr("data-value", data.Amount);
                        $(NewRow).find(".row-cost-item").find(".lbl-item-price").addClass('hide');
                        $(NewRow).find(".row-cost-item").find(".txt-item-price").removeClass('hide');
                    }

                    $(NewRow).find(".row-cost-item").find(".lbl-item-uom").first().hide();
                    $(NewRow).find(".row-cost-item").find(".lbl-item-uom").first().parent().append("<span></span>");
                    $(NewRow).find(".row-cost-item").find(".txt-item-unit").first().hide();
                    $(NewRow).find(".row-cost-item").find(".txt-item-unit").first().parent().append("<span></span>");
                    $(NewRow).find(".row-cost-item").find(".txt-item-unit_price").first().hide();
                    $(NewRow).find(".row-cost-item").find(".txt-item-unit_price").first().parent().append("<span></span>");
                    $(NewRow).find(".row-cost-item").find(".txt-item-permonth").first().hide();
                    $(NewRow).find(".row-cost-item").find(".txt-item-permonth").first().parent().append("<span></span>");
                    ////$(NewRow).find(".row-cost-item").find(".txt-item-contribution").first().hide();
                    ////$(NewRow).find(".row-cost-item").find(".txt-item-contribution").first().parent().append("<span></span>");
                    $(NewRow).find(".row-cost-item").find(".lbl-item-contribution").first().hide();
                    $(NewRow).find(".row-cost-item").find(".lbl-item-contribution").first().parent().append("<span></span>");
                    ////$(NewRow).find(".row-cost-item").find(".lbl-item-sellunitprice").first().hide();
                    ////$(NewRow).find(".row-cost-item").find(".lbl-item-sellunitprice").first().parent().append("<span></span>");
                    $(NewRow).find(".row-cost-item").find(".txt-item-sellunitprice").first().hide();
                    $(NewRow).find(".row-cost-item").find(".txt-item-sellunitprice").first().parent().append("<span></span>");
                    initAutoCompleteMaterial($(NewRow).find("#AutoCompleteMaterial-Search")[0], data.MaterialCode, loadBasePrice, true);

                    $("#table-cost-center-header").append(NewRow);
                } else {
                    initAutoCompleteMaterial($(NewRow).find("#AutoCompleteMaterial-Search")[0], data.MaterialCode, loadBasePrice);

                    NewRow.find("table").css({
                        "margin-left": "30px",
                        "width": "calc(100% - 30px)"
                    });

                    var target = $("#table-cost-center-header").find(".row-cost-item[data-rowid='" + data.NodeParentCode + "']");
                    $(target).closest("table").append(NewRow);
                }

            }

            calculatorContractMonth();
            initValidateAmountRowHeader();

            AdjustTableStyle();
        }

        function AdjustTableStyle() {
            $("#panel-table").find(".none-border-bottom").removeClass("none-border-bottom");

            $("#panel-table").find("table>.row-tbody").each(function () {
                if ($(this).find(".row-tbody").length > 0) {
                    $(this).find(".row-cost-item:last").find("td").each(function () {
                        $(this).addClass("none-border-bottom");
                    });
                }
            });

            function AdjustTableStyleLastRow(obj) {
                $(obj).find("table:first>.row-tbody:last").each(function () {
                    $(this).addClass("none-border-bottom");

                    if ($(this).find("table:first>.row-tbody").length > 0) {
                        AdjustTableStyleLastRow($(this).find("table:first>.row-tbody:last")[0]);
                    }
                });
            }

            AdjustTableStyleLastRow($("#panel-table")[0]);
        }

        function setIsRecurring() {
            var onetime = $("#<%= rdoOneTime.ClientID %>").is(":checked");
            var recurring = $("#<%= rdoRecurring.ClientID %>").is(":checked");

            if (onetime) {
                $(".row-cost-master").hide();
                $("#txtContractMonth").val("1");
                $(".txt-item-permonth").val("1");
                $("#th-total-amount").attr("colspan", "1");
            } else {
                $(".row-cost-master").show();
                $("#th-total-amount").attr("colspan", "2");
            }

            calculatorContractMonth();
        }

        function loadBasePrice(sender, isParent) {
            var materialCode = $(sender).val();
            var postData = {
                actionCase: "get_price_uom",
                "material_code": materialCode
            };

            $.ajax({
                type: "POST",
                url: "/API/CostSheetAPI.aspx",
                data: postData,
                success: function (data) {
                    var row = $(sender).closest('tr');

                    row.find(".lbl-item-uom").html(data.uom);

                    if (isParent) {
                        row.find(".lbl-item-uom").hide();

                        if (row.find(".lbl-item-uom").parent().find("span").length != 2) {
                            row.find(".lbl-item-uom").parent().append("<span></span>");
                        }
                    } else {
                        row.find(".txt-item-unit_price").html(numberFormat(data.base_price));

                        if (data.list_uom.length > 1) {
                            row.find(".ddl-item-uom").html("");
                            for (var i = 0; i < data.list_uom.length; i++) {
                                var option = $("<option/>", {
                                    value: data.list_uom[i]
                                });

                                option.append(data.list_uom[i]);

                                if (data.uom == data.list_uom[i]) {
                                    option.attr("selected", "selected");
                                }

                                row.find(".ddl-item-uom").append(option);
                            }
                            row.find(".lbl-item-uom").hide();
                            row.find(".ddl-item-uom").show();
                        } else {
                            row.find(".lbl-item-uom").show();
                            row.find(".ddl-item-uom").hide();
                        }

                        calculatorRow($(sender));
                    }
                }
            });
        }

        function loadPriceByUOM(sender) {
            var row = $(sender).closest('tr');
            var uom = $(sender).val();
            var materialCode = row.find("#ddlItemMaterial").val();

            row.find(".lbl-item-uom").html(uom);

            var postData = {
                actionCase: "get_price",
                "material_code": materialCode,
                "uom": uom
            };

            $.ajax({
                type: "POST",
                url: "/API/CostSheetAPI.aspx",
                data: postData,
                success: function (data) {
                    row.find(".txt-item-unit_price").html(numberFormat(data.base_price));

                    calculatorRow($(sender));
                }
            });
        }

        function checkedChangePriceHeader(obj) {
            if ($(obj).prop("checked")) {
                $(obj).closest('tr').find('.lbl-item-price').addClass('hide');
                $(obj).closest('tr').find('.txt-item-price').removeClass('hide');
            } else {
                $(obj).closest('tr').find('.txt-item-price').addClass('hide');
                $(obj).closest('tr').find('.lbl-item-price').removeClass('hide');
            }

        }
    </script>

    <script>
        /// ------------------------------------------------- row ------------------------------------------------- ///
        function insertRowHeader(obj) {
            var NewRow = $("#row-template .row-tbody").clone();
            var level = parseInt($(obj).attr("data-level"));
            NewRow.find(".btn-new-row").attr("data-level", "1");

            NewRow.find(".row-cost-item").attr("data-rowid", genNextRowID());
            NewRow.find(".row-cost-item").attr("data-rowlevel", '0');
            NewRow.find(".row-cost-item").attr("data-rowparentid", '');

            initAutoCompleteMaterial(NewRow.find("#AutoCompleteMaterial-Search")[0], "", loadBasePrice, true);

            //NewRow.find(".row-cost-item").find("#ddlItemMaterial").bind("change", function () {
            //    loadBasePrice(this);
            //});

            //NewRow.find(".row-cost-item").find(".ddl-item-uom").bind("change", function () {
            //    loadPriceByUOM(this);
            //});

            NewRow.find(".row-cost-item").find(".input-group-mat").addClass("row-header");
            NewRow.find(".row-cost-item").find(".txt-item-unit").first().hide();
            NewRow.find(".row-cost-item").find(".txt-item-unit").first().val("1.00");
            NewRow.find(".row-cost-item").find(".txt-item-unit").first().parent().append("<span></span>");
            NewRow.find(".row-cost-item").find(".txt-item-unit_price").first().hide();
            NewRow.find(".row-cost-item").find(".txt-item-unit_price").first().html("0.00");
            NewRow.find(".row-cost-item").find(".txt-item-unit_price").first().parent().append("<span></span>");
            NewRow.find(".row-cost-item").find(".txt-item-permonth").first().hide();
            NewRow.find(".row-cost-item").find(".txt-item-permonth").first().val($("#txtContractMonth").val());
            NewRow.find(".row-cost-item").find(".txt-item-permonth").first().parent().append("<span></span>");
            ////NewRow.find(".row-cost-item").find(".txt-item-contribution").first().hide();
            ////NewRow.find(".row-cost-item").find(".txt-item-contribution").first().val("0.00");
            ////NewRow.find(".row-cost-item").find(".txt-item-contribution").first().parent().append("<span></span>");
            NewRow.find(".row-cost-item").find(".lbl-item-contribution").first().hide();
            NewRow.find(".row-cost-item").find(".lbl-item-contribution").first().html("0.00");
            NewRow.find(".row-cost-item").find(".lbl-item-contribution").first().parent().append("<span></span>");
            ////NewRow.find(".row-cost-item").find(".lbl-item-sellunitprice").first().hide();
            ////NewRow.find(".row-cost-item").find(".lbl-item-sellunitprice").first().html("0.00");
            ////NewRow.find(".row-cost-item").find(".lbl-item-sellunitprice").first().parent().append("<span></span>");
            NewRow.find(".row-cost-item").find(".txt-item-sellunitprice").first().hide();
            NewRow.find(".row-cost-item").find(".txt-item-sellunitprice").first().val("0.00");
            NewRow.find(".row-cost-item").find(".txt-item-sellunitprice").first().parent().append("<span></span>");

            $("#table-cost-center-header").append(NewRow);

            AdjustTableStyle();
        }

        function insertRowItem(obj) {
            var NewRow = $("#row-template .row-tbody").clone();
            var level = parseInt($(obj).attr("data-level"));
            NewRow.find(".btn-new-row").attr("data-level", level + 1);
            NewRow.find("table").css({
                "margin-left": "30px",
                "width": "calc(100% - 30px)"
            });

            var rowParentID = $(obj).closest(".row-cost-item").attr("data-rowid");
            NewRow.find(".row-cost-item").attr("data-rowid", genNextRowID());
            NewRow.find(".row-cost-item").attr("data-rowlevel", level);
            NewRow.find(".row-cost-item").attr("data-rowparentid", rowParentID);

            initAutoCompleteMaterial(NewRow.find("#AutoCompleteMaterial-Search")[0], "", loadBasePrice);
            NewRow.find(".row-cost-item").find("#ddlItemMaterial").bind("change", function () {
                loadBasePrice(this);
            });

            NewRow.find(".row-cost-item").find(".ddl-item-uom").bind("change", function () {
                loadPriceByUOM(this);
            });

            NewRow.find(".row-cost-item").find(".txt-item-permonth").first().val($("#txtContractMonth").val());

            $(obj).closest("table").append(NewRow);

            AdjustTableStyle();
        }

        function removeRowItem(obj) {
            if (AGConfirm(obj, "ต้องการลบรายการใช่หรือไม่ ?")) {
                $(obj).closest('.row-tbody').remove();
            }
        }

        function genNextRowID() {
            var RowID = 0;
            $("#table-cost-center-header .row-cost-item").each(function () {
                var thisID = parseInt($(this).attr("data-rowid"));
                if (RowID < thisID) {
                    RowID = thisID;
                }
            });
            RowID++;

            var id = RowID.toString();
            for (var i = 0; i < 5 - RowID.toString().length; i++) {
                id = '0' + id;
            }
            return id;
        }

        /// ----------------------------------------------- end row ----------------------------------------------- ///
    </script>

    <script>
        /// ------------------------------------------------- cal ------------------------------------------------- ///

        function numberDefault(obj, zeroDigit) {
            if ($(obj).val() == '') {
                if (zeroDigit) {
                    $(obj).val('0');
                } else {
                    $(obj).val('0.00');
                }
                return true;
            }
            return false;
        }

        function numberFormat(val, zeroDigit) {
            var formatValue = new NumberFormat(val);

            if (zeroDigit) {
                formatValue.setPlaces(0);
            } else {
                formatValue.setPlaces(2);
            }

            return formatValue.toFormatted();
        }

        function isNumberKeyV2(evt, allowOperation) {
            evt = evt || window.event;
            var charCode = evt.which || evt.keyCode;
            var charTyped = String.fromCharCode(charCode);

            var digitChars = ".,01234567890";
            if (allowOperation)
                digitChars += "-+";

            return digitChars.indexOf(charTyped) !== -1;
        }

        function convertToDecimal(val) {
            return parseFloat(val.replace(/,/g, ''));
        }

        function sellUnitPriceChange(obj) {
            var row = $(obj).closest('tr');
            ////var txtContribution = row.find(".txt-item-contribution");
            var lblContribution = row.find(".lbl-item-contribution");

            numberDefault(obj);
            $(obj).val(numberFormat($(obj).val()));

            var sellPrice = convertToDecimal($(obj).val());
            var basePrice = convertToDecimal(row.find(".txt-item-unit_price").html());

            ////var margin = (((basePrice - sellPrice) * 100) / basePrice) * -1;
            var margin = sellPrice - basePrice;

            ////txtContribution.val(numberFormat(margin));
            lblContribution.html(numberFormat(margin));

            calculatorRow(obj);
        }

        function calculatorRow(obj) {
            numberDefault(obj);

            var month = convertToDecimal($("#txtContractMonth").val());

            var row = $(obj).closest('tr');

            var txtQty = row.find(".txt-item-unit");
            var txtPerMonth = row.find(".txt-item-permonth");
            ////var txtContribution = row.find(".txt-item-contribution");
            var lblContribution = row.find(".lbl-item-contribution");

            txtQty.val(numberFormat(txtQty.val()));
            txtPerMonth.val(numberFormat(txtPerMonth.val(), true));
            ////txtContribution.val(numberFormat(txtContribution.val()));
            lblContribution.html(numberFormat(lblContribution.html()));

            var unit = convertToDecimal(txtQty.val());
            var unit_price = convertToDecimal(row.find(".txt-item-unit_price").html());
            var perMonth = convertToDecimal(txtPerMonth.val());
            ////var contribution = convertToDecimal(txtContribution.val());
            var contribution = convertToDecimal(lblContribution.html());

            var amount = ((month / perMonth) * unit_price) * unit;

            var rowPrice = numberFormat(amount);

            ////var sellUnitPrice = numberFormat(unit_price + ((unit_price * contribution) / 100)); //0;            
            var sellUnitPrice = numberFormat(unit_price + contribution); //0;            

            //if (notCalSellPrice) {
            //    sellUnitPrice = numberFormat(row.find(".lbl-item-sellunitprice").first().val());
            //} else {
            //    sellUnitPrice = numberFormat(unit_price + ((unit_price * contribution) / 100));
            //}

            var actualCost = ((month / perMonth) * convertToDecimal(sellUnitPrice)) * unit;
            var rowActualCost = numberFormat(actualCost);

            row.find(".lbl-item-price").html(rowPrice);
            row.find(".txt-item-price").val(rowPrice);
            ////row.find(".lbl-item-sellunitprice").html(sellUnitPrice);
            row.find(".txt-item-sellunitprice").val(sellUnitPrice);
            row.find(".lbl-item-actual_cost").html(rowActualCost);

            row.closest(".row-cost-item[data-rowlevel='0']").find(".lbl-item-price:first").html(rowPrice);

            sumUp();

            row.find(".txt-item-price").removeClass("text-danger");
            //var totalAmount = 0;

            //$("#table-cost-center-header .row-cost-item").each(function () {
            //    totalAmount += convertToDecimal($(this).find(".lbl-item-price").html());
            //});

            //$(".lbl-total-amount").html(numberFormat(totalAmount));
        }

        function sumUp() {
            var total = 0;
            $(".row-cost-item[data-rowlevel='0']").each(function () {
                var rowId = $(this).attr("data-rowid");
                getChildValue(rowId, 0, 0, $(this));

                total += convertToDecimal($(this).find(".lbl-item-price").html());
            });

            $(".lbl-total-amount").html(numberFormat(total));
        }

        function getChildValue(parentId, total, actualCost, elementSetValue) {

            if ($(".row-cost-item[data-rowparentid='" + parentId + "']").length > 0) {
                $(".row-cost-item[data-rowparentid='" + parentId + "']").each(function () {
                    var rowId = $(this).attr("data-rowid");
                    total += convertToDecimal($(this).find(".lbl-item-price").html());
                    actualCost += convertToDecimal($(this).find(".lbl-item-actual_cost").html());
                    getChildValue(rowId, total, actualCost, elementSetValue);
                });
            } else {
                $(elementSetValue).find(".lbl-item-price").html(numberFormat(total));
                $(elementSetValue).find(".txt-item-price").val(numberFormat(total));
                $(elementSetValue).find(".lbl-item-actual_cost").html(numberFormat(actualCost));

                $(elementSetValue).find(".txt-item-price").removeClass("text-danger");
            }
        }

        function calculatorContractMonth(obj) {
            if (obj) {
                numberDefault(obj, true);
            }

            var month = numberFormat($("#txtContractMonth").val(), true);

            $("#table-cost-center-header .row-cost-item").each(function () {

                var itemMonth = $(this).find(".txt-item-permonth").val();

                if (itemMonth == "0" || itemMonth == "") {
                    $(this).find(".txt-item-permonth").val(month);
                }

                calculatorRow(this);
            });
        }

        function validateAmountRowHeader(obj) {
            if (numberDefault(obj)) {
                $(obj).val($(obj).prev().html());
            } else {
                $(obj).val(numberFormat($(obj).val()));
            }

            var Amount_Default = convertToDecimal($(obj).prev().html());
            var Amount_Change = convertToDecimal($(obj).val());

            if (Amount_Default > Amount_Change) {
                $(obj).addClass("text-danger");
            } else {
                $(obj).removeClass("text-danger");
            }
        }

        function initValidateAmountRowHeader() {
            $("[data-rowlevel='0']").each(function () {
                if ($(this).find(".chkAmountManual:first").prop("checked")) {
                    var value = $(this).find(".txt-item-price:first").attr("data-value");
                    $(this).find(".txt-item-price:first").val(numberFormat(value));
                    validateAmountRowHeader($(this).find(".txt-item-price")[0]);
                }
            });
        }
        /// ----------------------------------------------- end cal ----------------------------------------------- ///
    </script>

    <script>
        /// --------------------------------------------- autocomplete -------------------------------------------- ///
        function initAutoCompleteMaterial(obj, defaultValue, CallBack, isParent) {
            bindAutoCompleteMaterial(
                obj,
                getDataSourceMaterial(),
                defaultValue,
                CallBack,
                isParent
            );
        }

        function getDataSourceMaterial(keySearch) {
            var dataSource = [];
            var dataMat = JSON.parse($("#hddJsonMat").val());

            for (var i = 0; (dataSource.length < 1000 && dataMat.length > i) ; i++) {
                var data = dataMat[i];
                if (keySearch && !(data.code.indexOf(keySearch) < 0 || data.desc.indexOf(keySearch) < 0)) {
                    continue;
                }
                dataSource.push(data);
            }

            return dataSource;
        }

        function getSearchResultMaterial(datas, str) {
            var selectCode = datas.select('*').where('code').match(str).fetch();
            var selectName = datas.select('*').where('desc').match(str).fetch();
            var selectDisplay = datas.select('*').where('display').match(str).fetch();
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

            return selectResult;
        }

        function bindAutoCompleteMaterial(obj, data, defaultValue, CallBack, isParent) {
            var txtText = $(obj).closest("td").find("#AutoCompleteMaterial-Text").first();
            var txtValue = $(obj).closest("td").find("#AutoCompleteMaterial-Value").first();

            loadMaterialServer = false;

            var DB = new JQL(data);

            $(obj).typeahead('destroy');
            $(obj).typeahead({
                hint: true,
                highlight: true,
                minLength: 0
            }, {
                limit: 20,
                templates: {
                    pending: '<div class="text-danger" style="padding: 2px 10px; line-height: 24px;">Result not found.</div>',
                    suggestion: function (data) {
                        return '<div>' + data.display + '</div>';
                    }
                },
                source: function (str, callback, serverCallback) {
                    var selectResult = getSearchResultMaterial(DB, str);

                    // Search ข้อมูลใหม่ถ้า Select TOP (1000) แล้วไม่เจอ
                    if ((DB.data_source.length >= 1000 || loadMaterialServer) && selectResult.length == 0) {
                        datas = getDataSourceMaterial(str);
                        DB = new JQL(datas);
                        selectResult = getSearchResultMaterial(DB, str);
                        serverCallback(selectResult);

                    } else {
                        callback(selectResult);
                    }
                },
                display: function (data) {
                    return data.display;
                }
            });

            $(obj).bind('typeahead:change', function (e, v) {
                if (v.trim() == "") {
                    txtValue.val("");
                    txtText.val("");

                    // Todo Other Function
                    if (typeof (CallBack) === "function") {
                        CallBack(txtValue, isParent);
                    }
                }
            });

            $(obj).bind('typeahead:select typeahead:autocomplete', function (e, v) {
                txtValue.val(v.code);
                txtText.val(v.desc);

                // Todo Other Function
                if (typeof (CallBack) === "function") {
                    CallBack(txtValue, isParent);
                }
            });

            if (!defaultValue) {
                defaultValue = "";
            } else {
                defaultValue = getSearchResultMaterial(
                    new JQL(JSON.parse($("#hddJsonMat").val())),
                    defaultValue
                )[0].display;
            }

            $(obj).typeahead('val', defaultValue);

            if (defaultValue != "") {
                var temp = defaultValue.split(":");
                var code = temp[0].trim();
                var desc = temp[1].trim();

                txtValue.val(code);
                txtText.val(desc);
            }
        }

        /// ------------------------------------------- end autocomplete ------------------------------------------ ///
    </script>

    <%----------------------------------------------- BOM ----------------------------------------------%>
    <script>
        function createBomClick() {
            AGLoading(true);
            $("#<%= btnBindingBOM.ClientID %>").click();
        }

        function validateCreateBOM(sender) {
            var msg = "";
            $("#modal-create-bom").find(".required").each(function () {
                if ($(this).val() == "") {
                    msg += msg == "" ? "" : "<br/>";
                    msg += "Please fill out " + $(this).closest(".form-group").find("label").first().html() + ".";
                }
            });

            if (msg != "") {
                AGError(msg);
            } else {
                if (AGConfirm(sender, "Confirm save BOM")) {
                    AGLoading(true);
                    $(sender).next().click();
                }
            }
        }
    </script>

</asp:Content>
