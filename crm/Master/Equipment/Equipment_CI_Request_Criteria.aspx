<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRMMasterPage.master" AutoEventWireup="true" CodeBehind="Equipment_CI_Request_Criteria.aspx.cs" Inherits="ServiceWeb.crm.Master.Equipment.Equipment_CI_Request_Criteria" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-create-ci").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <div class="card shadow">
        <div class="card-header">
            <h5 class="mb-0">Create CI From SO</h5>
        </div>
        <div class="card-body">
            <div>
                <div class="form-row hide">
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
                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtEquipmentCode" />
                    </div>
                    <div class="form-group col-md-6 col-sm-12">
                        <label>
                            Configuration Item Name
                        </label>
                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtEquipmentName" />
                    </div>
                </div>
                <div class="form-row hide">
                    <div class="form-group col-md-6 col-sm-12">
                        <label>
                            Family
                        </label>
                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlEquipmentType"
                            DataTextField="Description" DataValueField="MaterialGroupCode">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-6 col-sm-12">
                        <label>
                            Status
                        </label>
                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlEquipmentStatus">
                            <asp:ListItem Text="All" Value="" />
                            <asp:ListItem Text="New" Value="N" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-row hide">
                    <div class="form-group col-sm-12 col-md-6">
                        <label>Class</label>
                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlSearch_EMClass">
                        </asp:DropDownList>
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
                <div class="form-row hide">
                    <div class="form-group col-sm-12 col-md-6">
                        <label>Class</label>
                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="DropDownList1">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-sm-12 col-md-6">
                        <label>Category</label>
                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="DropDownList2">
                            <asp:ListItem Text="All" Value="" />
                            <asp:ListItem Text="Main Configuration Item" Value="00" />
                            <%--<asp:ListItem Text="Sub Configuration Item" Value="01" />--%>
                            <asp:ListItem Text="Virtual Configuration Item" Value="02" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-row">
                      <div class="form-group col-md-3 col-sm-12">
                        <label>
                            CIBatch
                        </label>
                       <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtCIBatch" />
                    </div>
                    <div class="form-group col-md-3 col-sm-12">
                        <label>
                            SOType
                        </label>
                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtSOType" />
                    </div>
                    <div class="form-group col-md-6 col-sm-12">
                        <label>
                            SONumber
                        </label>
                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtSONumber" />
                    </div>
                </div>
                <div class="form-row">
                      <div class="form-group col-md-3 col-sm-12">
                        <label>
                            SoItemNo
                        </label>
                       <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtSoItemNo" />
                    </div>
                    <div class="form-group col-md-3 col-sm-12">
                        <label>
                            ShipToCode
                        </label>
                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtShipToCode" />
                    </div>
                    <div class="form-group col-md-6 col-sm-12">
                        <label>
                            ShipToName
                        </label>
                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtShipToName" />
                    </div>
                </div>
                <div class="form-row">
                      <div class="form-group col-md-3 col-sm-12">
                        <label>
                            MaterialCode
                        </label>
                       <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtMaterialCode" />
                    </div>
                    <div class="form-group col-md-3 col-sm-12">
                        <label>
                            MeterialName
                        </label>
                        <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtMeterialName" />
                    </div>
                </div>

                <div class="form-row">
                    <div class="col-md-6 col-sm-12">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button Text="Search" runat="server" CssClass="btn btn-primary"
                                    ID="btnSearchData" OnClick="btnSearchData_Click" OnClientClick="inactiveRequireField();AGLoading(true);" />
                                <input type="button" ID="btn_OpenModalCreated" name="name" value="Create" class="btn btn-success AUTH_MODIFY"
                                    onclick="$(this).next().click();" />
                                <asp:Button ID="btnOpenModalCreated" ClientIDMode="Static" CssClass="d-none AUTH_MODIFY" OnClick="btnOpenModalCreated_Click" OnClientClick="AGLoading(true);" Text="" runat="server"  />
                                <%--<span class="btn btn-primary btn-sm" >
                            New Equipment
                        </span>--%>
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
                                        <th class="text-nowrap">Create</th>
                                        <%--<th class="text-nowrap">Relation</th>--%>
                                        <th class="text-nowrap">CIBatch</th>
                                        <th class="text-nowrap">SOType</th>
                                        <th class="text-nowrap">SONumber</th>
                                        <th class="text-nowrap">SoItemNo</th>
                                        <th class="text-nowrap">ShipToCode</th>
                                        <th class="text-nowrap">ShipToName</th>
                                        <th class="text-nowrap">MaterialCode</th>
                                        <th class="text-nowrap">MeterialName</th>
                                        <th class="text-nowrap">StartDate</th>
                                        <th class="text-nowrap">EndDate</th>
                                        <th class="text-nowrap">Status</th>
                                        <th class="text-nowrap">RequestType</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptItems" runat="server">
                                            <ItemTemplate>
                                                <tr class="c-pointer" data-CI="<%# Eval("CIBatch") %>" data-SO="<%# Eval("SONumber") %>" data-Type="<%# Eval("SOType") %>" data-item="<%# Eval("SoItemNo") %>" data-matname="<%# Eval("MeterialName") %>" >
                                                    <td class="text-nowrap text-center align-middle">
                                                        <%--<i class="fa fa-edit fa-lg text-dark mx-1 AUTH_MODIFY" title="Edit" onclick="openEditRow(this);"></i>--%>                                                        
                                                        <asp:CheckBox ID="_chk_select" runat="server" CssClass="chkbox_List chkbox_item" /> 
                                                        <asp:HiddenField ID="hddList_ci" runat="server" Value='<%# Eval("CIBatch") %>' />
                                                        <asp:HiddenField ID="hddList_so" runat="server" Value='<%# Eval("SONumber") %>' />
                                                        <asp:HiddenField ID="hddList_type" runat="server" Value='<%# Eval("SOType") %>' />
                                                        <asp:HiddenField ID="hddList_item" runat="server" Value='<%# Eval("SoItemNo") %>' />
                                                        <asp:HiddenField ID="hddList_FiscalYear" runat="server" Value='<%# Eval("FiscalYear") %>' />
                                                        <asp:HiddenField ID="hddList_ShipToCode" runat="server" Value='<%# Eval("ShipToCode") %>' />
                                                        <asp:HiddenField ID="hddList_ShipToName" runat="server" Value='<%# Eval("ShipToName") %>' />
                                                        <asp:HiddenField ID="hddList_startdate" runat="server" Value='<%# Eval("StartDate") %>' />
                                                        <asp:HiddenField ID="hddList_matname" runat="server" Value='<%# Eval("MeterialName") %>' />
                                                        <asp:HiddenField ID="hddList_enddate" runat="server" Value='<%# Eval("EndDate") %>' />
                                                        <%--<i class="fa fa-times-circle-o fa-lg text-danger mr-1 AUTH_MODIFY" title="Delete" onclick="confirmDelete(this);"></i>--%>
                                                        <%--<asp:Button ID="btnDelete" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnDelete_Click" CommandArgument='<%# Eval("ImpactCode") %>' />--%>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("CIBatch") %>
                                                    </td>
                                                    <td>
                                                        <%# Eval("SOType") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("SONumber") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("SoItemNo") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("ShipToCode") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("ShipToName") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("MaterialCode") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("MeterialName") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# !string.IsNullOrEmpty(Convert.ToString(Eval("StartDate"))) ? Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("StartDate").ToString()) : "" %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# !string.IsNullOrEmpty(Convert.ToString(Eval("EndDate"))) ? Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("EndDate").ToString()) : "" %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("Status") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("RequestType") %>
                                                    </td>
                                                    <%--<td class="text-nowrap" onclick="openEditRow(this);">
                                                        <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("CREATED_ON").ToString()) %>
                                                    </td>
                                                    <td class="text-nowrap" onclick="openEditRow(this);">
                                                        <%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("UPDATED_ON").ToString()) %>
                                                    </td>--%>
                                                </tr>
                                            </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                        <%--<div style="display: none;" runat="server" id="divTranslaterStatus" clientidmode="Static">[]</div>
                        <div style="display: none;" runat="server" id="divJsonEquipmentList" clientidmode="Static">[]</div>--%>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>

    <script>
        //function afterSearch() {
        //    var StatusList = JSON.parse($("#divTranslaterStatus").html());
        //    var EquipmentList = JSON.parse($("#divJsonEquipmentList").html());
        //    var data = [];
        //    for (var i = 0 ; i < EquipmentList.length ; i++) {
        //        var Equipment = EquipmentList[i];
        //        data.push([
        //            Equipment.EquipmentCode,
        //            Equipment.EquipmentCode,
        //            Equipment.EquipmentCode,
        //            Equipment.Description,
        //            Equipment.EquipmentTypeName,
        //            Equipment.EquipmentClassName,
        //            TranslaterEMCategory(Equipment.CategoryCode),
        //            TranslaterEMStatus(Equipment.Status),
        //            Equipment.OwnerGroupName
        //        ]);
        //    }
        //    function TranslaterEMStatus(code) {
        //        for (var i = 0; i < StatusList.length; i++) {
        //            if (code == StatusList[i].StatusCode) {
        //                return StatusList[i].StatusName;
        //            }
        //        }
        //        return code;
        //    }
        //    function TranslaterEMCategory(code) {
        //        if (code == "00") {
        //            return "Main Configuration Item";
        //        }
        //        //if (code == "01") {
        //        //    return "Sub Configuration Item";
        //        //}
        //        if (code == "02") {
        //            return "Virtual Configuration Item";
        //        }
        //        return code;
        //    }
        //    $("#divSearch").show();
        //    $("#tableItems").dataTable({
        //        data: data,
        //        deferRender: true,
        //        "order": [[3, "asc"]],
        //        'columnDefs': [
        //           {
        //               "orderable": false,
        //               'targets': 0,
        //               'createdCell': function (td, cellData, rowData, row, col) {
        //                   $(td).addClass("text-center text-nowrap");
        //                   $(td).html(
        //                       //'<a class="AUTH_MODIFY" href="EquipmentDetail.aspx?code=' + cellData + '&mode=Edit">' +
        //                       '<a class="AUTH_MODIFY" href="JavaScript:;">' +
		//							'<i class="fa fa-pencil-square-o"></i>' +
		//						'</a>'
        //                    );
        //                   //$(td).bind("click", function () { openEquipment(cellData, 'Edit') });
        //                   $(td).closest("tr").addClass("c-pointer");
        //                   $(td).closest("tr").bind("click", function () { openEquipment(cellData, 'Edit') });                  
        //               }
        //           },
        //           {
        //               "orderable": false,
        //               'targets': 1,
        //               'createdCell': function (td, cellData, rowData, row, col) {
        //                   $(td).addClass("text-center text-nowrap");
        //                   $(td).bind("click", function (event) { event.stopPropagation(); });
        //                   $(td).html(
        //                       '<a href="EquipmentDiagramRelation.aspx?id=' + cellData + '" target="_blank">' +
		//						    '<i class="fa fa-sitemap"></i>' +
		//					    '</a>'
        //                    );
        //               }
        //           },
        //           {
        //               'targets': 5,
        //               'createdCell': function (td, cellData, rowData, row, col) {
        //                   $(td).addClass("text-nowrap");
        //                   $(td).html(TranslaterStatus(cellData));
        //               }
        //           },
        //           {
        //               'targets': [2, 4],
        //               'createdCell': function (td, cellData, rowData, row, col) {
        //                   $(td).addClass("text-nowrap");
        //               }
        //           }
        //        ]
        //    });
        //       $('html,body').animate({
        //           scrollTop: $("#divSearch").offset().top - 50
        //       });
        //       function TranslaterStatus(Code) {
        //           for (var i = 0; i < StatusList.length; i++)
        //               if (Code == StatusList[i].StatusCode)
        //                   return StatusList[i].StatusName;
        //           return Code;
        //       }
        //   }

        function afterSearch() {
            AGLoading(false);
            $("#tableItems").dataTable({
                columnDefs: [{
                    "orderable": false,
                    "targets": [0]
                }],
                "order": [[1, "asc"]]
            });
            $("#divSearch").show();

        }
           function showDiagram(code) {
               AGLoading(true);
               $("#modal-diagram").modal("show");
               $("#modal-diagram iframe").prop("src", "/crm/Master/Equipment/EquipmentDiagram.aspx?relationNode=" + code);
           }
           function successIframeLoad() {
               AGLoading(false);
           }

           function openEquipment(EquipmentCode, Mode) {
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

            function openEdit(CI, TYPE, Number, item,matname) {
                inactiveRequireField();
                $("#<%= hddCIBatch.ClientID %>").val(CI);
                $("#<%= hddSOType.ClientID %>").val(TYPE);
                $("#<%= hddSONumber.ClientID %>").val(Number);
                $("#<%= hddSoItemNo.ClientID %>").val(item);
                $("#txtEquipmentCode").val(Number);
                $("#txtEquipmentName").val(CI); 
                $("#<%= txtEquipmentName_Created.ClientID %>").val(matname);
                showInitiativeModal('modalAddEquipment');
                $("#btn_OpenModalCreated").click();
            }
            function openEditRow(obj)
            {
                inactiveRequireField();
                var CI = $(obj).closest("tr").data("ci");
                var TYPE = $(obj).closest("tr").data("type");
                var Number = $(obj).closest("tr").data("so");
                var item = $(obj).closest("tr").data("item");
                var matname = $(obj).closest("tr").data("matname");
                openEdit(CI, TYPE, Number, item, matname);
            }

    </script>
    <div class="d-none">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hddEquipmentCode" />
                <asp:HiddenField runat="server" ID="hddPage_Mode" />
                <asp:HiddenField runat="server" ID="hddCIBatch" />
                <asp:HiddenField runat="server" ID="hddSOType" />
                <asp:HiddenField runat="server" ID="hddSONumber" />
                <asp:HiddenField runat="server" ID="hddSoItemNo" />
                <asp:HiddenField runat="server" ID="hddSetEdit" />
                <asp:Button Text="" runat="server" ID="btnOpenDetailEquipment" 
                    OnClick="btnOpenDetailEquipment_Click" OnClientClick="AGLoading(true);" />
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
                                                <asp:TextBox runat="server" ID="txtEquipmentCode_Created" CssClass="form-control form-control-sm disabled"
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
                                                <asp:DropDownList runat="server" ID="ddlEquipmentType_Created" CssClass="form-control form-control-sm required"
                                                    DataTextField="Description" DataValueField="MaterialGroupCode" onchange="inactiveRequireField();$('#btnSetDoctypeSwitchComfigMode').click();">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group col-sm-12 col-md-6">
                                                <label>Class</label>
                                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm required" ID="ddlEMClass">
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
    </script>
</asp:Content>
