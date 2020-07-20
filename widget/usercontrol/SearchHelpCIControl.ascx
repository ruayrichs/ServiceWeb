<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchHelpCIControl.ascx.cs" Inherits="ServiceWeb.widget.usercontrol.SearchHelpCIControl" EnableViewState="true"%>

<div>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpHddCISelect">
        <ContentTemplate>
            <asp:TextBox runat="server" ID="txtSearchHelp_DataCISelect" ClientIDMode="Static" CssClass="d-none" />      
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCI">
        <ContentTemplate>
            <div>
                <div class="row">
                    <div class="col">
                        <div class="form-row">
                            <%-- start --%>
                            <div class="row">
                                <div class="form-group col-lg-4 col-md-12 col-sm-12">
                                    <div><b>Owner Service</b></div>
                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlOwnerService"></asp:DropDownList>

                                </div>
                                <div class="form-group col-lg-4 col-md-12 col-sm-12">
                                    <div><b>Configuration Item Code</b></div>
                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtEquipmentCode" />
                                </div>
                                <div class="form-group col-lg-4 col-md-12 col-sm-12">
                                    <div><b>Configuration Item Name</b></div>
                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtEquipmentName" />
                                </div>
                                <div class="form-group col-md-6 col-sm-6">
                                    <div><b>Family</b></div>
                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlEquipmentType"></asp:DropDownList>
                                </div>
                                <div class="form-group col-md-6 col-sm-6">
                                    <div><b>Status</b></div>
                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlEquipmentStatus"></asp:DropDownList>
                                </div>
                                <div class="form-group col-md-6 col-sm-6">
                                    <div><b>Class</b></div>
                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlSearch_EMClass"></asp:DropDownList>
                                </div>
                                <div class="form-group col-md-6 col-sm-6">
                                    <div><b>Category</b></div>
                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ddlSearch_Category">
                                        <asp:ListItem Text="All" Value="" />
                                        <asp:ListItem Text="Main Configuration Item" Value="00" />
                                        <%--<asp:ListItem Text="Sub Configuration Item" Value="01" />--%>
                                        <asp:ListItem Text="Virtual Configuration Item" Value="02" />
                                    </asp:DropDownList>
                                </div>
                            </div>



                            <%-- ebd --%>
                        </div>

                    </div>

                </div>
            </div>

            <div style="padding-top: 15px; padding-bottom: 15px;">
                <asp:Button runat="server" Text="Search" CssClass="btn btn-info" ID="search_ci_btn" OnClick="search_ci_btn_Click" OnClientClick="AGLoading(true);"/>
                <button onclick="resetTextForConfigurationItemSearch();" class="btn btn-warning">Clear</button>
                <asp:Button runat="server" CssClass="d-none" ID="clear_ci_btn" OnClick="clear_ci_btn_Click" />
            </div>

            <div class="form-row">
                <div class="form-group col-sm-12 col-md-12">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpSearchConfigurationItem">
                        <ContentTemplate>
                            <asp:HiddenField runat="server" ID="hddClearCheck" ClientIDMode="Static"/>
                            <div class="table-responsive">
                                <table id="tableItems" class="table table-bordered table-striped table-hover table-sm" style="width: 100%;">
                                    <thead>
                                        <tr>
                                            <th class="text-nowrap">Select</th>
                                            <th class="text-nowrap">Configuration Item Code</th>
                                            <th>Configuration Item Name</th>
                                            <th class="text-nowrap">Family</th>
                                            <th class="text-nowrap">Class</th>
                                            <th class="text-nowrap">Category</th>
                                            <th class="text-nowrap">Status</th>
                                            <th class="text-nowrap">Owner Service</th>
                                        </tr>
                                    </thead>
                                    <!--
                        <tbody>
                            <asp:Repeater ID="rptCI" runat="server">
                                <ItemTemplate>
                                    <tr class="table-hover">
                                        <td class="text-nowrap">
                                            <asp:CheckBox runat="server" CssClass="form-check form-check-label" Text='<%# Eval("EquipmentCode").ToString() %>' />
                                         </td>
                                         <td class="text-nowrap">
                                             <%# Eval("EquipmentCode") %>
                                         </td>
                                         <td class="text-nowrap">
                                             <%# Eval("Description") %>
                                         </td>
                                         <td class="text-nowrap">
                                             <%# Eval("EquipmentTypeName") %>
                                         </td>
                                         <td class="text-nowrap">
                                             <%# Eval("Status") %>
                                         </td>
                                         <td class="text-nowrap">
                                             <%# Eval("EquipmentClassName") %>
                                         </td>
                                         <td class="text-nowrap">
                                             <%# Eval("CategoryCode") %>
                                         </td>
                                         <td class="text-nowrap">
                                             <%# Eval("OwnerGroupName") %>
                                         </td>
                                     </tr>
                                 </ItemTemplate>
                             </asp:Repeater>
                         </tbody>
                        -->
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>     
            
            <div>
                <div class="row">
                    <div class="col-2">

                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<script type="text/javascript">
    //hiding checkbox label
    //$('input[type="checkbox"]').each(function(){
    //    var selectedID = $(this).attr('id');
    //    var value = $('label[for=' + selectedID + ']').css("display", "none");
    //});
    //

    function removeSelectedCIwithCIcode(cicode) {
        alert("testtttttt");
        var arrCI = [];
        console.log("ssssssssssssssarrCI", arrCI);
        if ($("#<%= txtSearchHelp_DataCISelect.ClientID %>").val() != "") {
            arrCI = $("#<%= txtSearchHelp_DataCISelect.ClientID %>").val().split(',');
            var index = arrCI.indexOf(cicode);
            if (index !== -1) {
                arrCI.splice(index, 1);
                $("#<%= txtSearchHelp_DataCISelect.ClientID %>").val(arrCI.toString());
            };
            console.log("arrCIresult", arrCI);
        }
    }

    function resetTextForConfigurationItemSearch() {
        $("#<%= txtEquipmentCode.ClientID %>").val('');
        $("#<%= txtEquipmentName.ClientID %>").val('');
        $("#<%= ddlEquipmentType.ClientID %>").val('');
        $("#<%= ddlSearch_EMClass.ClientID %>").val('');
        $("#<%= ddlSearch_Category.ClientID %>").val('');
        $("#<%= ddlEquipmentStatus.ClientID %>").val('');

        $("#<%= txtSearchHelp_DataCISelect.ClientID %>").val('');
        $("#<%= clear_ci_btn.ClientID %>").click();
        $("#<%= hddClearCheck.ClientID %>").val(true);
    }


    function afterSearchBindEquipmentCriteria(dataArr, StatusList) {
        console.log("StatusList", StatusList)
        var data = [];
        for (var i = 0; i < dataArr.length; i++) {
            var Equipment = dataArr[i];
            data.push([
                Equipment.EquipmentCode,
                Equipment.EquipmentCode,
                Equipment.Description,
                Equipment.EquipmentTypeName,
                Equipment.EquipmentClassName,
                TranslaterEMCategory(Equipment.CategoryCode),
                TranslaterEMStatus(Equipment.Status),
                Equipment.OwnerGroupName,
                Equipment.Selected
            ]);
        }

        console.log("data", data);
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

        $("#tableItems").dataTable(
            {
                data: data,
                deferRender: true,
                "order": [[3, "asc"]],
                'columnDefs': [
                    {
                        "orderable": false,
                        'targets': 0,
                        'createdCell': function (td, cellData, rowData, row, col) {
                            var checkbox = $("<input>", {
                                type: "checkbox",
                                value: rowData[0]
                            }).on("change", function () {
                                var arrCI = [];
                                if ($("#<%= txtSearchHelp_DataCISelect.ClientID %>").val() != "") {
                                   arrCI = $("#<%= txtSearchHelp_DataCISelect.ClientID %>").val().split(',');
                               }
                               if ($(this).prop("checked")) {
                                   arrCI.push($(this).val());
                               } else {
                                   var index = arrCI.indexOf($(this).val());
                                   if (index > -1) {
                                       arrCI.splice(index, 1);
                                   }
                               }
                               $("#<%= txtSearchHelp_DataCISelect.ClientID %>").val(arrCI.toString());

                           });
                            $(td).html('');
                            $(td).append(checkbox);
                            $(td).addClass("text-center");
                            if ($("#<%= hddClearCheck.ClientID %>").val() == 'false') {
                                if (rowData[8] === true) {
                                    checkbox.attr("checked", "checked");
                                    checkbox.addClass("checkbox_checked");

                                    var arrCI = [];
                                    if ($("#<%= txtSearchHelp_DataCISelect.ClientID %>").val() != "") {
                                        arrCI = $("#<%= txtSearchHelp_DataCISelect.ClientID %>").val().split(',');
                                    }
                                    arrCI.push(rowData[0]);
                                    $("#<%= txtSearchHelp_DataCISelect.ClientID %>").val(arrCI.toString());
                                } else {
                                    checkbox.addClass("checkbox_unchecked");
                                }
                            }
                            //$(td).addClass("text-center text-nowrap");
                            //$(td).data("key", rowData[0]);
                            //$(td).html(
                            //    '<a href="JavaScript:;">' +
                            //    '<i class="fa fa-cubes" aria-hidden="true"></i>' +
                            //    '</a>'
                            //);
                            //$(td).bind("click", function () {
                            //});

                            //$(td).closest("tr").addClass("c-pointer");
                            //$(td).closest("tr").data("key", rowData[0]);
                            //$(td).closest("tr").bind("click", function () {
                            //});
                        }
                    }
                ]
            }
        );
    }
</script>
