<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="DocumentReceiveCriteria.aspx.cs" Inherits="ServiceWeb.Transaction.DocumentReceiveCriteria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .hide {
            display: none;
        }
    </style>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
           
        </div>
    </nav>

    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">ใบตรวจรับงาน</h5>
                </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="form-group col-lg-6">
                            <label>Company</label>
                            <asp:TextBox ID="tbCompany" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="col-sm-6">
                            <div class="form-row">
                                <div class="form-group col-lg-12">
                                    <label>Document Type</label>
                                    <asp:DropDownList ID="ddlDocumentType" runat="server" CssClass="form-control form-control-sm"
                                        DataValueField="DocumentTypeCode"
                                        DataTextField="xDisplay">
                                    </asp:DropDownList>
                                </div>
                            </div>
                           
                            <div class="form-group">
                                <label>Document Status</label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control form-control-sm"
                                    DataValueField="STATUSCODE"
                                    DataTextField="xDisplay">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-row">
                                <div class="form-group col-md-6">
                                    <label>Start Date</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="tbStartDate" runat="server" CssClass="form-control form-control-sm date-picker"></asp:TextBox>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-6">
                                    <label>End Date</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="tbEndDate" runat="server" CssClass="form-control form-control-sm date-picker"></asp:TextBox>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                        </div>
                    </div>

                    <button type="button" class="btn btn-info" onclick="searchClick();"><i class="fa fa-search"></i>&nbsp;&nbsp;Search</button>
                    <button type="button" class="btn btn-success" onclick="showInitiativeModal('modalcreateDocumentPO');"><i class="fa fa-file-o"></i>&nbsp;&nbsp;Create</button>
                    
                    
                    <div class="d-none">
                        <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnCreate" runat="server" OnClick="btnCreate_Click" />
                                
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="panel-items" ><%--style="display: none;" class="table-responsive"--%>
                        <hr />
                        <asp:UpdatePanel ID="udpFirstList" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="table-Firstlist" class="table table-bordered table-striped table-hover table-sm">
                                        <thead>
                                            <tr>
                                                <th class="text-truncate">Select</th>
                                                <th class="text-truncate">วันที่เอกสาร</th>
                                                <th class="text-truncate">เลขที่เอกสาร</th>
                                                <th class="text-truncate">ประเภทเอกสาร</th>
                                                <th class="text-truncate">สถานะ</th>
                                                <th class="text-truncate">Ref1</th>
                                                <th class="text-truncate">NetValue</th>
                                                <th>คำอธิบายเพิ่มเติม</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptFisrtList" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td class="text-truncate">
                                                            <button type="button" class="btn btn-sm btn-success" onclick="$(this).next().click();"><i class="fa fa-search"></i>&nbsp;&nbsp;ดูรายละเอียด</button>
                                                            <asp:Button ID="btnSelectDoc" runat="server"  CssClass="hide" OnClick="btnSelectDoc_Click" CommandArgument='<%# Eval("DocNumber") %>' />
                                                            <asp:HiddenField id="hdfDocType" runat="server" value='<%# Eval("DocType") %>' />
                                                            <asp:HiddenField id="hdfFiscalYear" runat="server" value='<%# Eval("FiscalYear") %>' />
                                                        </td>
                                                        <td class="text-truncate"><span style="display:none";><%# Eval("DocDate") %></span><%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("DocDate").ToString()) %></td>
                                                        <td class="text-truncate"><%# Eval("DocNumber") %></td>
                                                        <td class="text-truncate"><%# Eval("DocType") +" - " +  Eval("DocTypeName")%></td>
                                                        <td class="text-truncate"><%# FormatDocStat(Eval("DocStatus").ToString()) %></td>
                                                        <td class="text-truncate"><%# Eval("Ref1") %></td>
                                                        <td class="text-truncate text-right"><%# Eval("NetValue","{0:#,0.00}") %></td>
                                                        <td class="text-truncate"><%# Eval("Remark") %></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table></ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    
                    </div>
                </div>
            </div>
        <%--</div>--%>
    </div>
    
    <%--modal search Ticket criteria--%>
    <div class="initiative-model-control-slide-panel" id="modalcreateDocumentPO">
        <div class="initiative-model-control-body-content z-depth-3">
            <div>
                <div class="initiative-model-control-header">
                    <div class="mat-box-initaive-control">
                        <div class="pull-right">
                            <i class="fa fa-close hand" onclick="closeInitiativeModal('modalcreateDocumentPO');"></i>
                        </div>
                        <div class="one-line">
                            <label class="text-warning">
                                Create Document By PO Number
                            </label>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-control-contant">
                    <div class="panel-body-initiative-master">
                        <div class="panel-content-initiative-master">
                            <div class="mat-box-initaive-control tab-initiative-control">

                                <div class="form-row" style="overflow:auto;">
                                    
                                        <asp:UpdatePanel ID="udpDocList" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="table-Doclist" class="table table-bordered table-striped table-hover table-sm">
                                        <thead>
                                            <tr>
                                                <th class="te">Select</th>
                                                <th class="text-truncate">FiscalYear</th>
                                                <th class="text-truncate">PONumber</th>
                                                <th class="text-truncate">Item</th>
                                                <th class="text-truncate">MaterialCode</th>
                                                <th class="text-truncate">ShortText</th>
                                                <th class="text-truncate">POQty</th>
                                                <th class="text-truncate">UOMDesc</th>
                                                <th class="text-truncate">VendorCode</th>
                                                <th class="text-truncate">VendorName</th>
                                                <th class="text-truncate">PlantCode</th>
                                                <th class="text-truncate">PlantDes</th>
                                                <th class="text-truncate">StorLocCode</th>
                                                <th class="text-truncate">StorageDesc</th>
                                                <th class="text-truncate">DocDate</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptDocList" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td class="text-center">
                                                            <asp:CheckBox ID="_chk_select" runat="server" CssClass="chkbox_List chkbox_item" OnClick="chk_select_po_item(this);"/> 
                                                            <asp:HiddenField ID="hddPONumber" runat="server" Value='<%# Eval("PONumber") %>' />
                                                            <asp:HiddenField ID="hddPODoctype" runat="server" Value='<%# Eval("DocumentTypeCode") %>' />
                                                            <asp:HiddenField ID="hddFiscalYear" runat="server" Value='<%# Eval("FiscalYear") %>' />
                                                        </td>
                                                        <td class="text-truncate"><%# Eval("FiscalYear") %></td>
                                                        <td class="text-truncate"><%# Eval("PONumber") %></td>
                                                        <td class="text-truncate"><%# Eval("Item") %></td>
                                                        <td class="text-truncate"><%# Eval("MaterialCode") %></td>
                                                        <td class="text-truncate"><%# Eval("ShortText") %></td>
                                                        <td class="text-truncate text-right"><%# Eval("POQty","{0:#,0}") %></td>
                                                        <td class="text-truncate"><%# Eval("UOMDesc") %></td>
                                                        <td class="text-truncate"><%# Eval("VendorCode") %></td>
                                                        <td class="text-truncate"><%# Eval("VendorName") %></td>
                                                        <td class="text-truncate"><%# Eval("PlantCode") %></td>
                                                        <td class="text-truncate"><%# Eval("PlantDesc") %></td>
                                                        <td class="text-truncate"><%# Eval("StorLocCode") %></td>
                                                        <td class="text-truncate"><%# Eval("StorageDesc") %></td>
                                                        <td class="text-truncate"><%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay( Eval("DocDate").ToString()) %></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
   
                                </div>
                                <button type="button" class="btn btn-info" onclick="createClick();"><i class="fa fa-search"></i>&nbsp;&nbsp;Select</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-bottom">
                    <div class="text-right">
                        <a class="water-button" onclick="closeInitiativeModal('modalcreateDocumentPO');"><i class="fa fa-close"></i>&nbsp;Close</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    

    <script>
        $(document).ready(function () {
            rebindTableFirstlist();
            $('#table-Doclist').DataTable({
                paging: false,
                info: false,
                searching: false,
            });
        });

        function rebindTableFirstlist()
        {
            $('#table-Firstlist').DataTable({
                paging: false,
                info: false,
                searching: false,
            });
        }

        function ISFloat(obj, event) {
            $(obj).val($(obj).val().replace(/[^0-9\.]/g, ''));
            if ((event.which != 46 || event.which == 8 || event.which == 9 || $(obj).val().indexOf('.') != -1)
                      && ((event.which < 48 || event.which > 57))) {
                event.preventDefault();
            }
        }
        function formatCurrency(obj, e) {
            $(obj).val(decimalFormat($(obj).val(), 2))
            return;
        }

        function showModal() {
            showInitiativeModal("master-dataProdesc");
        }
        function hideModal() {
            closeInitiativeModal("master-dataProdesc");
        }

        function searchClick() {
            //inactiveRequireField();
            AGLoading(true);
            $("#<%= btnSearch.ClientID %>").click();
        }
        function createClick() {
            //inactiveRequireField();
            AGLoading(true);
            $("#<%= btnCreate.ClientID %>").click();
        }

        function chk_select_po_item(obj) {
            var objCheckPO = $(obj).closest("tbody").find("input[type=checkbox]");
            for (var i = 0; i < objCheckPO.length; i++) {
                $(objCheckPO[i]).prop('checked', false);
            }
            $(obj).prop('checked', true);
        }

    </script>

</asp:Content>

