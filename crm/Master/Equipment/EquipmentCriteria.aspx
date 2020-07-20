<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRMMasterPage.master" AutoEventWireup="true" CodeBehind="EquipmentCriteria.aspx.cs" Inherits="ServiceWeb.crm.Master.Equipment.EquipmentCriteria" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-configuration-item").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <div class="card shadow">
        <div class="card-header">
            <h5 class="mb-0">Configuration Item Criteria</h5>
        </div>
        <div class="card-body">
            <div class="PANEL-DEFAULT-BUTTON">
                <div class="form-row">
                    <div class="form-group col-md-3 col-sm-12">
                        <label>
                            Owner Service
                        </label>
                        <asp:DropDownList ID="ddlOwnerService" CssClass="form-control form-control-sm" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-3 col-sm-12">
                        <label>
                            Configuration Item Code
                        </label>
                         <asp:TextBox ID="txtEquipmentCode" runat="server" CssClass="form-control form-control-sm" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6 col-sm-12">
                        <label>
                            Configuration Item Name
                        </label>
                        <asp:TextBox ID="txtEquipmentName" runat="server" CssClass="form-control form-control-sm" ClientIDMode="Static"></asp:TextBox>                   
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-6 col-sm-12">
                        <label>
                            Family
                        </label>
                        <asp:UpdatePanel runat="server" ID="udpddlEquipmenttype" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlEquipmentType" AutoPostBack="True"
                                    OnSelectedIndexChanged="SelectionCIFamily_Change"
                                    DataTextField="Description" DataValueField="MaterialGroupCode">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                    <div class="form-group col-md-6 col-sm-12">
                        <label>
                            Status
                        </label>
                         
                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlEquipmentStatus" >
                            <asp:ListItem Text="All" Value="" />
                            <asp:ListItem Text="New" Value="N" />
                        </asp:DropDownList>
                               

                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-sm-12 col-md-6">
                        <label>Class</label>
                        <asp:UpdatePanel runat="server" ID="updDdlEmclassSearch" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlSearch_EMClass" AutoPostBack="True"
                                    OnSelectedIndexChanged="SelectionCI_Change">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group col-sm-12 col-md-6">
                        <label>Category</label>
                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlSearch_Category">
                            <asp:ListItem Text="All" Value="" />
                            <asp:ListItem Text="Main Configuration Item" Value="00" />
                            <%--<asp:ListItem Text="Sub Configuration Item" Value="01" />--%>
                            <asp:ListItem Text="Virtual Configuration Item" Value="02" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-6 col-sm-12">
                        <label>Serial No.</label>
                        <asp:TextBox runat="server" ID="txtSerialNo" CssClass="form-control form-control-sm" />            
                    </div>
                    <div class="form-group col-md-6 col-sm-12">
                        <label>Attributes</label>
                        <asp:TextBox runat="server" ID="txtxValue001" CssClass="form-control form-control-sm" />            
                    </div>
                    <div class="form-group col-md-6 col-sm-12 d-none">
                        <label>Send Mail before Next Maintenance</label>
                        <asp:TextBox type="number" placeholder="Day" runat="server" ID="txtTimeSendMail" CssClass="form-control form-control-sm border border-success" />            
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-6 col-sm-12">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button Text="Search" runat="server" CssClass="btn btn-primary DEFAULT-BUTTON-CLICK"
                                    ID="btnSearchData" OnClick="btnSearchData_Click" OnClientClick="inactiveRequireField();AGLoading(true);" />

                                <input type="button" name="name" value="Create" class="btn btn-success AUTH_MODIFY"
                                    onclick="inactiveRequireField();$(this).next().click();" />
                                <asp:Button ID="btnOpenModalCreated" CssClass="d-none AUTH_MODIFY" OnClick="btnOpenModalCreated_Click" OnClientClick="AGLoading(true);" Text="" runat="server" />
                                <%--<span class="btn btn-primary btn-sm" >
                            New Equipment
                        </span>--%>
                                <asp:Button runat="server" Text="Export Data" ID="exportData" OnClick="exportData_Click" OnClientClick="AGLoading(true);" CssClass="btn btn-warning" />
                                <a id="download-report-excel" class="hide" target="_blank"
                                    href="/API/ExportExcelCI.ashx"></a>
                                <script>
                                    function exportExcelAPI() {
                                        $("#download-report-excel")[0].click();
                                    }
                                </script>             
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="card" style="margin-bottom: 10px; margin-top: 15px;">
                            <div class="card-body card-body-sm">
                                <div class="d-flex justify-content-between">
                                    <div class="col align-self-center">
                                      <asp:FileUpload runat="server" ID="fuCI" accept=".xls, .xlsx" />
                                    </div>
                                    <asp:Button runat="server" ID="btnUpload" Text="Import Data"
                                        OnClick="btnUpload_Click" 
                                        CssClass="btn btn-outline-primary" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-12 d-none">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="float-right">
                                    <asp:Button Text="Search Send Mail" runat="server" CssClass="btn btn-outline-primary DEFAULT-BUTTON-CLICK" ID="Button1" OnClick="btnSearchData_Click" OnClientClick="inactiveRequireField();AGLoading(true);" />

                                    <asp:Button Text="Update Send Mail" runat="server" CssClass="btn btn-outline-success DEFAULT-BUTTON-CLICK" ID="Button2" OnClick="btnSearchData_Click" OnClientClick="inactiveRequireField();AGLoading(true);" />
                                    <%--<asp:Button Text="Send Mail" runat="server" CssClass="btn btn-info" OnClick="btnSearchDataSendmail_Click" OnClientClick="inactiveRequireField();AGLoading(true);"/>--%>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                
            </div>
            <hr />
            <div id="divSearch" style="display: none;">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpListEquipment">
                    <ContentTemplate>
                        <div class="table-responsive">
                            <table id="tableItems" class="table table-bordered table-striped table-hover table-sm" style="width: 100%;">
                                <thead>
                                    <tr>
                                        <th class="text-nowrap">Edit</th>
                                        <th class="text-nowrap">Relation</th>
                                        <th class="text-nowrap">Configuration Item Code</th>
                                        <th>Configuration Item Name</th>
                                        <th class="text-nowrap">Configuration Item Type</th>
                                        <th class="text-nowrap">Configuration Item Class</th>
                                        <th class="text-nowrap">Configuration Item Category</th>
                                        <th class="text-nowrap">Status</th>
                                        <th class="text-nowrap">Owner Service</th>
                                        <th class="text-nowrap">xValue001</th>
                                        <th class="text-nowrap">xValue002</th>
                                        <th class="text-nowrap">xValue003</th>
                                        <th class="text-nowrap">xValue004</th>
                                        <th class="text-nowrap">xValue005</th>
                                        <th class="text-nowrap">Model</th>
                                        <th class="text-nowrap">Serial Number</th>
                                        <th class="text-nowrap">MA Start Date</th>
                                        <th class="text-nowrap">MA End Date</th>
                                        <th class="text-nowrap">Warranty Start Date</th>
                                        <th class="text-nowrap">Warranty End Date</th>
                                        <th class="text-nowrap">Location</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                        <div style="display: none;" runat="server" id="divTranslaterStatus" clientidmode="Static">[]</div>
                        <div style="display: none;" runat="server" id="divJsonEquipmentList" clientidmode="Static">[]</div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>

    <script>
        <%--function openPopUpWindows(EquipmentCode) {
            $("#<%= hddEquipmentCode.ClientID %>").val(EquipmentCode);
            let url = "<%= Page.ResolveUrl("~/crm/Master/Equipment/EquipmentDetail.aspx") %>";
            let params = 'resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no, status=yes';
            window.open(url, 'Configuration Item Details', params);
        }--%>

        function afterSearch() {
            var StatusList = JSON.parse($("#divTranslaterStatus").html());
            var EquipmentList = JSON.parse($("#divJsonEquipmentList").html());

            var data = [];
            for (var i = 0 ; i < EquipmentList.length ; i++) {
                var Equipment = EquipmentList[i];

                data.push([
                    Equipment.EquipmentCode,
                    Equipment.EquipmentCode,
                    Equipment.EquipmentCode,
                    Equipment.Description,
                    Equipment.EquipmentTypeName,
                    Equipment.EquipmentClassName,
                    TranslaterEMCategory(Equipment.CategoryCode),
                    TranslaterEMStatus(Equipment.Status),
                    Equipment.OwnerGroupName,
                    Equipment.xValue001,
                    Equipment.xValue002,
                    Equipment.xValue003,
                    Equipment.xValue004,
                    Equipment.xValue005,
                    Equipment.ModelNumber,
                    Equipment.ManufacturerSerialNO,
                    Equipment.BeginGuarantee,
                    Equipment.EndGuaranty,
                    Equipment.BeginWarrantee,
                    Equipment.EndWarrantee,
                    Equipment.CiLocation
                ]);
            }

            function TranslaterEMStatus(code) {
                for (var i = 0; i < StatusList.length; i++) {
                    if (code == StatusList[i].StatusCode) {
                        return StatusList[i].StatusName;
                    }
                }
                return code;
            }
            function TranslaterEMCategory(code) {
                if (code == "00") {
                    return "Main Configuration Item";
                }
                //if (code == "01") {
                //    return "Sub Configuration Item";
                //}
                if (code == "02") {
                    return "Virtual Configuration Item";
                }
                return code;
            }

            $("#divSearch").show();
            $("#tableItems").dataTable({
                data: data,
                deferRender: true,
                "order": [[3, "asc"]],
                'columnDefs': [
                   {
                       "orderable": false,
                       'targets': 0,
                       'createdCell': function (td, cellData, rowData, row, col) {
                           $(td).addClass("text-center text-nowrap");
                           $(td).html(
                               //'<a class="AUTH_MODIFY" href="EquipmentDetail.aspx?code=' + cellData + '&mode=Edit">' +
                               '<a class="AUTH_MODIFY" href="JavaScript:;">' +
									'<i class="fa fa-pencil-square-o"></i>' +
								'</a>'
                            );
                           //$(td).bind("click", function () { openEquipment(cellData, 'Edit') });
                           $(td).closest("tr").addClass("c-pointer");
                           $(td).closest("tr").bind("click", function () {
                               openEquipment_PageCriteria(cellData, 'Edit')
                               //openPopUpWindows(cellData);
                           });
                          
                       }
                   },
                   {
                       "orderable": false,
                       'targets': 1,
                       'createdCell': function (td, cellData, rowData, row, col) {
                           $(td).addClass("text-center text-nowrap");
                           $(td).bind("click", function (event) { event.stopPropagation(); });
                           $(td).html(
                               '<a href="EquipmentDiagramRelation.aspx?id=' + cellData + '" target="_blank">' +
								    '<i class="fa fa-sitemap"></i>' +
							    '</a>'
                            );
                       }
                   },
                   {
                       'targets': 5,
                       'createdCell': function (td, cellData, rowData, row, col) {
                           $(td).addClass("text-nowrap");
                           $(td).html(TranslaterStatus(cellData));
                       }
                   },
                   {
                       'targets': [2, 4],
                       'createdCell': function (td, cellData, rowData, row, col) {
                           $(td).addClass("text-nowrap");
                       }
                   }
                ]
            });

               $('html,body').animate({
                   scrollTop: $("#divSearch").offset().top - 50
               });

               function TranslaterStatus(Code) {
                   for (var i = 0; i < StatusList.length; i++)
                       if (Code == StatusList[i].StatusCode)
                           return StatusList[i].StatusName;

                   return Code;
               }
           }

           function showDiagram(code) {
               AGLoading(true);
               $("#modal-diagram").modal("show");
               $("#modal-diagram iframe").prop("src", "/crm/Master/Equipment/EquipmentDiagram.aspx?relationNode=" + code);
           }
           function successIframeLoad() {
               AGLoading(false);
           }

           function openEquipment_PageCriteria(EquipmentCode, Mode) {
               inactiveRequireField();
               $("#<%= hddEquipmentCode.ClientID %>").val(EquipmentCode);
               $("#<%= hddPage_Mode.ClientID %>").val(Mode);
               $("#<%= btnOpenDetailEquipment.ClientID %>").click();
           }

            function activeRequireField() {
                $(".required").prop('required', true);
            }

            function inactiveRequireField() {
                $(".required").prop('required', false);
            }

            function confirmSaveEquipment(obj)
            {
                if (AGConfirm(obj, "Confirm Create."))
                {
                    AGLoading(true);
                    return true;
                }
                return false;
            }

    </script>
    <div class="d-none">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hddEquipmentCode" />
                <asp:HiddenField runat="server" ID="hddPage_Mode" />
                <asp:Button Text="" runat="server" ID="btnOpenDetailEquipment" 
                    OnClick="btnOpenDetailEquipment_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!-- Modal -->
    <div id="modal-diagram" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-body">
                    <iframe style="border: none; height: calc(100vh - 150px); width: 100%" scrolling="no"></iframe>
                    <div class="text-right" style="margin-top: 10px;">
                        <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <div class="initiative-model-control-slide-panel" id="modalAddEquipment">
        <div class="initiative-model-control-body-content z-depth-3">
            <div>
                <div class="initiative-model-control-header">
                    <div class="mat-box-initaive-control">
                        <div class="pull-right">
                            <i class="fa fa-close hand" onclick="closeInitiativeModal('modalAddEquipment');"></i>
                        </div>
                        <div class="one-line">
                            <label class="text-warning">
                                Create Configuration Item
                            </label>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-control-contant">
                    <div class="panel-body-initiative-master">
                        <div class="panel-content-initiative-master">
                            <div class="mat-box-initaive-control tab-initiative-control">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCreateEquipment">
                                    <ContentTemplate>
                                        <div class="form-row">
                                             <div class="form-group col-sm-12 col-md-3">
                                                <label>
                                                    Owner Service
                                                </label>
                                                <asp:DropDownList ID="ddlOwnerService_Created" CssClass="form-control form-control-sm required" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group col-sm-12 col-md-3">
                                                <label>
                                                    Configuration Item Code
                                                </label>
                                                <asp:TextBox runat="server" ID="txtEquipmentCode_Created"
                                                    placeholder="Text"  />
                                            </div>
                                            <div class="form-group col-sm-12 col-md-6">
                                                <label>
                                                    Configuration Item Name
                                                </label>
                                                <asp:TextBox runat="server" ID="txtEquipmentName_Created" CssClass="form-control form-control-sm required"
                                                    placeholder="Text" />
                                            </div>
                                        </div>
                                        <div class="form-row d-none">
                                            <div class="form-group col-sm-12 col-md-6">
                                                <label>
                                                    Valid From
                                                </label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker required" ID="txtEquipmentDateFrom"
                                                        placeholder="dd/mm/yyy" />
                                                    <span class="input-group-append hand">
                                                        <i class="fa fa-calendar input-group-text"></i>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="form-group col-sm-12 col-md-6">
                                                <label>
                                                    Valid To
                                                </label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker required" ID="txtEquipmentDateTo"
                                                        placeholder="dd/mm/yyy" />
                                                    <span class="input-group-append hand">
                                                        <i class="fa fa-calendar input-group-text"></i>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="form-group col-sm-12 col-md-6">
                                                <label>
                                                    Family
                                                </label>
                                                <asp:DropDownList runat="server" ID="ddlEquipmentType_Created" CssClass="form-control form-control-sm required" AutoPostBack="True"
                                                    OnSelectedIndexChanged="SelectionCIFamilyCreate_Change"
                                                    DataTextField="Description" DataValueField="MaterialGroupCode" onchange="inactiveRequireField();$('#btnSetDoctypeSwitchComfigMode').click();">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group col-sm-12 col-md-6">
                                                <label>Class</label>
                                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm required" ID="ddlEMClass" AutoPostBack="True"
                                                    OnSelectedIndexChanged="SelectionCICreate_Change">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="form-group col-sm-12 col-md-6">
                                                <label>Category</label>
                                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm required" ID="ddlCategory">
                                                    <asp:ListItem Text="เลือก" Value="" />
                                                    <asp:ListItem Text="Main Configuration Item" Value="00" />
                                                    <%--<asp:ListItem Text="Sub Configuration Item" Value="01" />--%>
                                                    <asp:ListItem Text="Virtual Configuration Item" Value="02" />
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group col-sm-12 col-md-6">
                                                <label>Status</label>
                                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm required" ID="ddlStatus">
                                                    <asp:ListItem Text="New" Value="N" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="form-group col-sm-12 col-md-6">
                                                <label>Asset</label>
                                                <%--<asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlAsset">
                                                    
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
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <div class="hide">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCheckRequiredForSave" OnClick="btnCheckRequiredForSave_Click" ClientIDMode="Static" Text="" runat="server" CssClass='AUTH_MODIFY' />
                                <asp:Button Text="" runat="server" ID="btnSaveNewEquipment"
                                    ClientIDMode="Static" OnClick="btnSaveNewEquipment_Click"
                                    OnClientClick="return confirmSaveEquipment(this);" />
                                <asp:Button ID="btnSetDoctypeSwitchComfigMode" ClientIDMode="Static" OnClick="btnSetDoctypeSwitchComfigMode_Click" OnClientClick="AGLoading(true);" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="initiative-model-bottom">
                    <div class="text-right">
                        <a class="water-button AUTH_MODIFY" onclick="activeRequireField();$('#btnCheckRequiredForSave').click();"><i class="fa fa-save"></i>&nbsp;Save</a>
                        
                        <a class="water-button" onclick="closeInitiativeModal('modalAddEquipment');"><i class="fa fa-close"></i>&nbsp;Close</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function openSearchHelpAsset() {
            $("#tableAssetList").dataTable();
            $('#modal-searchHelpAsset').modal('show');
            $(".modal-backdrop.fade.show").css({ "z-index": 10000 });
        }
        function selectSearchHelpAssetSelect(obj) {
            var assetCode = $(obj).attr("data-code");
            var assetDesc = $(obj).attr("data-desc");
            $("#<%= hddAccountAssignmentBox_AssetCode.ClientID %>").val(assetCode);
            $("#<%= txtAccountAssignmentBox_AssetDesc.ClientID %>").val(assetCode + ' : ' + assetDesc);
        }
    </script>
    
    <div id="modal-searchHelpAsset" class="modal fade" role="dialog" style="z-index: 10001;">
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

</asp:Content>
