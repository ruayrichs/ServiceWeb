<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="CustProfileList.aspx.cs" Inherits="POSWeb.crm.CustProfileList" %>

<%--<%@ Register Src="~/UserControl/SmartPaging.ascx" TagPrefix="uc1" TagName="SmartPaging" %>--%>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEmployee.ascx" TagPrefix="uc1" TagName="AutoCompleteEmployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <script>
       function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-customers").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
   </script>
    <script>
        function beforeSearch() {
            if (document.getElementById("txtFirstname").value == "'") {
                document.getElementById("txtFirstname").value = "''";
            }
            if (document.getElementById("txtPhone").value == "'") {
                document.getElementById("txtPhone").value = "''";
            }
            if (document.getElementById("txtPhoneMobile").value == "'") {
                document.getElementById("txtPhoneMobile").value = "''";
            }
            if (document.getElementById("txtEmail").value == "'") {
                document.getElementById("txtEmail").value = "''";
            }
            if (document.getElementById("txtContactName").value == "'") {
                document.getElementById("txtContactName").value = "''";
            }
            if (document.getElementById("txtContactNickName").value == "'") {
                document.getElementById("txtContactNickName").value = "''";
            }
            if (document.getElementById("txtContactPhone").value == "'") {
                document.getElementById("txtContactPhone").value = "''";
            }
            if (document.getElementById("txtContactEmail").value == "'") {
                document.getElementById("txtContactEmail").value = "''";
            }
            if (document.getElementById("txtContactPOSITION").value == "'") {
                document.getElementById("txtContactPOSITION").value = "''";
            }
            if (document.getElementById("txtContactRemark").value == "'") {
                document.getElementById("txtContactRemark").value = "''";
            }

        };
    </script>
    <script>
      
        function bindDataModeView(obj) {
            beforeSearch();
            inactiveRequireField();
            $('#btnSearchData').click();
        }

    </script>

    <div class="card shadow">
        <div class="card-header">
            <h5 class="mb-0">Clients</h5>
        </div>
        <div class="card-body panel-body-customer-search PANEL-DEFAULT-BUTTON">
            <div class="form-row">
                <div class="col-md-12">
                    <div class="card border-default" style="margin-bottom: 10px;">
                        <div class="card-body card-body-sm">
                            <div class="form-row">
                                <div class="col-sm-6">
                                    <div class="form-row">
                                        <div class="form-group col-md-6 col-sm-12">
                                            <label>Client Code / Name</label>
                                            <asp:TextBox ID="txtFirstname" runat="server" CssClass="form-control form-control-sm" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-6 col-sm-12">
                                            <label>Client Group</label>
                                            <asp:DropDownList runat="server" ID="ddlCustomerGroup" CssClass="form-control form-control-sm">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-6 col-sm-12">
                                            <label>Active</label>
                                            <asp:DropDownList ID="ddlCustomerActive" CssClass="form-control form-control-sm" runat="server">
                                                <asp:ListItem Value="" Text="All" />
                                                <asp:ListItem Value="True" Text="Active" Selected="True" />
                                                <asp:ListItem Value="False" Text="In active" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-6 col-sm-12">
                                            <label>Sale Area</label>
                                            <asp:DropDownList runat="server" ID="ddlSaleDistrict" CssClass="form-control form-control-sm"
                                                DataValueField="SAREACODE" DataTextField="CodeAndDesc">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-6 col-sm-12">
                                            <label>Provice</label>
                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-6 col-sm-12">
                                            <label>Owner Service</label>
                                            <asp:DropDownList ID="ddlOwnerService" CssClass="form-control form-control-sm" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                   
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label>Tax ID</label>
                                        <asp:TextBox ID="txtTaxID" runat="server" CssClass="form-control form-control-sm" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Work Phone</label>
                                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control form-control-sm" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Mobile Phone</label>
                                                <asp:TextBox ID="txtPhoneMobile" onkeypress="return isNumberKey(event);" runat="server" CssClass="form-control form-control-sm" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label>E-mail</label>
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-sm" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div class="form-row">
                <div class="col-md-12">
                    <div class="card border-default" style="margin-bottom: 10px;">
                        <div class="card-body card-body-sm">
                            <div class="form-row">
                                <div class="form-group col-md-3">
                                    <label>Contact Name</label>
                                    <asp:TextBox ID="txtContactName" CssClass="form-control form-control-sm" runat="server"
                                        placeholder="Name" ClientIDMode="Static" />
                                </div>
                                <div class="form-group col-md-3">
                                    <label>Contact Service</label>
                                    <asp:TextBox ID="txtContactNickName" CssClass="form-control form-control-sm" runat="server"
                                        placeholder="Service" ClientIDMode="Static" />
                                </div>
                                <div class="form-group col-md-3">
                                    <label>Contact Phone</label>
                                    <asp:TextBox ID="txtContactPhone" onkeypress="return isNumberKey(event);" CssClass="form-control form-control-sm" runat="server"
                                        placeholder="Phone numbers" ClientIDMode="Static" />
                                </div>
                                <div class="form-group col-md-3">
                                    <label>Contact Email</label>
                                    <asp:TextBox ID="txtContactEmail" CssClass="form-control form-control-sm " runat="server"
                                        placeholder="Email" ClientIDMode="Static" />
                                </div>
                                <div class="form-group col-md-3">
                                    <label>Contact Position</label>
                                    <asp:TextBox ID="txtContactPOSITION" CssClass="form-control form-control-sm" runat="server"
                                        placeholder="Position" ClientIDMode="Static" />
                                </div>
                                <div class="form-group col-md-3">
                                    <label>Authorization Contact</label>
                                    <asp:DropDownList ID="ddlContactAuthorization" CssClass="form-control form-control-sm" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group col-md-6">
                                    <label>Contact Remark</label>
                                    <asp:TextBox ID="txtContactRemark" CssClass="form-control form-control-sm" runat="server"
                                        placeholder="Note" ClientIDMode="Static" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <button type="button" class="btn btn-info DEFAULT-BUTTON-CLICK" onclick="bindDataModeView(this);"><i class="fa fa-search"></i>&nbsp;&nbsp;Search</button>
            <button type="reset" class="btn btn-warning" onclick="resetTextForSearch();"><i class="fa fa-refresh"></i>&nbsp;&nbsp;Clear</button>
            <% if(IsAllFeature) { %> 
            <button type="button" class="btn btn-success AUTH_MODIFY" onclick="inactiveRequireField();$('#<%= btnSetDataOpenModalCreate.ClientID %>').click();"><i class="fa fa-file-o"></i>&nbsp;&nbsp;Create Client</button>
            <% } %>

            <asp:UpdatePanel ID="udpsearchButton" runat="server" UpdateMode="Conditional" class="d-none">
                <ContentTemplate>
                    <asp:Button ID="btnSearchData" runat="server" ClientIDMode="Static" OnClick="btnSearchData_Click" OnClientClick="AGLoading(true);" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div id="search-panel" style="display: none;">

                <hr />

                <asp:UpdatePanel ID="upPanelProfileList" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <!-- Mode Table View -->
                        <div class="table-responsive">
                            <table id="table-customer" class="table table-bordered table-striped table-hover table-sm nowrap">
                                <thead>
                                    <tr>
                                        <th class="text-center text-nowrap">Detail</th>
                                        <th>Client Code / Name</th>                                                                                    
                                        <th>Client Group</th>
                                        <th>Address</th>
                                        <th>Tax ID</th>
                                        <th>Work Phone</th>
                                        <th>Moblie Phone</th>
                                        <th>E-mail</th>
                                        <th>Active</th>
                                    </tr>
                                </thead>
                                <%--<tbody>--%>
                                    <%--<asp:Repeater ID="rptCustProfileModeTable" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td class="text-center text-nowrap">
                                                    <a href="-/crm/CustomerProfileDetail.aspx?id=<%# Eval("CustomerCode") %>" title="View Customer Detail">
                                                        <i class="fa fa-user fa-lg text-dark c-pointer" aria-hidden="true"></i>
                                                    </a>
                                                </td>
                                                <td>
                                                    <%# Eval("CustomerCode") + (string.IsNullOrEmpty(Eval("FullName").ToString()) ? "" : " : " + Eval("FullName")) %>
                                                </td>                                                           
                                                <td>
                                                    <%# Eval("CustomerGroup") + (string.IsNullOrEmpty(Eval("CustomerGroupDesc").ToString()) ? "" : " : " + Eval("CustomerGroupDesc")) %>                                                    
                                                </td>                            
                                                <td>
                                                    <%# Eval("Address") %>                                                    
                                                </td>
                                                <td>
                                                    <%# Eval("TelNo1") %>                                                    
                                                </td>
                                                <td>
                                                    <%# ConvertEmailManage(Eval("EMail")) %>                                                    
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>--%>
                                <%--</tbody>--%>
                            </table>
                        </div>

                        <div style="display: none;" runat="server" id="divJsonCustomerList" ClientIDMode="Static">[]</div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
    
    <script>
        function activeRequireField() {
            $(".required").prop('required', true);
        }

        function inactiveRequireField() {
            $(".required").prop('required', false);
        }

        function checkRequireFieldCreate()
        {
            activeRequireField();
            $("#<%= btnCheckRequireFieldCreate.ClientID %>").click();
        }

        function updateCustomerDetailRefModalClick(sender) {
            var x = document.getElementsByClassName("txt-add-address");
            var className = "txt-add-address";
            var data = "";
            data = '{"address":[';
            for (var i = 0; i < x.length; i++) {
                data += '{"PropertyCode":"' + x[i].getAttribute("data-PropertyCode")
                    + '","Description":"' + x[i].getAttribute("data-Description")
                    + '","PropertyValue":"' + x[i].value + '"}'
                data += (i < x.length - 1) ? "," : "";
                //if (className == "txt-add-address") {
                //    x[i].value = "";
                //}
            }
            data += ']}';
            $("#hddJsonAddress").val(data);
            var msg = '';

            msg = "คุณต้องการสร้างข้อมูลลูกค้าหรือไม่ "
            if (AGConfirm(sender, msg)) {
                AGLoading(true);
                saveAddress(sender);
                $("#<%= btnCreateCustomerDetail.ClientID %>").click();
                return true;
            }
            return false;
        }

        function saveAddress(obj) {
            //, className, rowhide, AddressCodeEdit
            var x = document.getElementsByClassName("txt-add-address");
            var data = "";
            data = '{"address":[';
            for (var i = 0; i < x.length; i++) {
                data += '{"PropertyCode":"' + x[i].getAttribute("data-PropertyCode")
                    + '","Description":"' + x[i].getAttribute("data-Description")
                    + '","PropertyValue":"' + x[i].value + '"}'
                data += (i < x.length - 1) ? "," : "";
                //if (className == "txt-add-address") {
                //    x[i].value = "";
                //}
            }
            data += ']}';

            $("#hddJsonAddress").val(data);

        }
    </script>
     <div class="initiative-model-control-slide-panel" id="modal-EditCustomerDetail">
        <div class="initiative-model-control-body-content z-depth-3">
            <div>
                <div class="initiative-model-control-header">
                    <div class="mat-box-initaive-control">
                        <div class="pull-right">
                            <i class="fa fa-close hand" onclick="closeInitiativeModal('modal-EditCustomerDetail');"></i>
                        </div>
                        <div class="one-line">
                            <label class="text-warning">
                                Create New
                            </label>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-control-contant">
                    <div class="panel-body-initiative-master">
                        <div class="panel-content-initiative-master">
                            <div class="mat-box-initaive-control tab-initiative-control">
                               
								<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCustomerCreate">
									<ContentTemplate>
                                         <asp:HiddenField runat="server" ID="hddJsonAddress" ClientIDMode="Static" />
										<div class="form-row">
                                            <div class="col-lg-6">
                                                <div class="card border-primary" style="margin-bottom: 10px;">
                                                    <div class="card-body card-body-sm">
                                                        <div class="form-row">
                                                            <div class="form-group col-md-6 col-sm-6 col-xs-12">
                                                                <label>Client Group</label>
                                                                <asp:DropDownList CssClass="form-control form-control-sm required"
                                                                    runat="server" ID="_ddl_CD_CustomerGroup" onchange="inactiveRequireField();$('#btnSetCustomerCodeRefConfig').click();">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-md-6 col-sm-6 col-xs-12">
                                                                <asp:UpdatePanel ID="udpCustomerCodeRefConfig" UpdateMode="Conditional" runat="server">
                                                                    <ContentTemplate>
                                                                        <label>Client Code</label>
                                                                        <asp:TextBox ID="_txt_CD_CustomerCode" Enabled="false" CssClass="form-control form-control-sm" placeholder="Text" runat="server" />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                            <div class="form-group col-md-6 col-sm-6 col-xs-12">
                                                                <label>Client Name</label>
                                                                <asp:TextBox Text="" CssClass="form-control form-control-sm required" placeholder="Text TH" runat="server" ID="_txt_CD_CustomerName" />

                                                            </div>
                                                            <div class="form-group col-md-6 col-sm-6 col-xs-12">
                                                                <label>Foreign Name</label>
                                                                <asp:TextBox Text="" placeholder="Text EN" CssClass="form-control form-control-sm" runat="server" ID="_txt_CD_ForeignName" />
                                                            </div>
                                                            <div class="form-group col-md-6 col-sm-12 col-xs-12">
                                                                <label>Sales Employee</label>
                                                                <uc1:AutoCompleteEmployee runat="server" id="AutoCompleteEmployee" placeholder="Text" CssClass="form-control form-control-sm" />
                                                            </div>
                                                            <div class="form-group col-md-6 col-sm-12 col-xs-12">
                                                                <label>Owner Service</label>
                                                                <asp:DropDownList ID="ddlOwnerService_Created" CssClass="form-control form-control-sm" runat="server">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6">
                                                <div class="card border-default" style="margin-bottom: 10px;">
                                                    <div class="card-body card-body-sm">
                                                        <div class="form-row">
                                                            <div class="form-group col-md-12 col-sm-12 col-xs-12">
                                                                <label>Tax ID</label>
                                                                <input id="_txt_CD_CustomerTaxID" placeholder="Number" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static" />
                                                            </div>
                                                            <div class="form-group col-md-6 col-sm-6 col-xs-6">
                                                                <label>Work Phone</label>
                                                                <input id="_txt_CD_CustomerPhone" placeholder="Text" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static" />
                                                            </div>
                                                            <div class="form-group col-md-6 col-sm-6 col-xs-6">
                                                                <label>Moblie Phone</label>
                                                                <input id="_txt_CD_CustomerPhoneMoblie" placeholder="Number" onkeypress="return isNumberKey(event);" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static" />
                                                            </div>
                                                            <div class="form-group col-md-12 col-sm-12 col-xs-12">
                                                                <label>E-Mail</label>
                                                                <input id="_txt_CD_CustomerEmail" placeholder="Text EN" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
										</div>
                                        <div class="form-row">
                                            <div class="col-lg-12">
                                                <div class="card border-default" style="margin-bottom: 10px;">
                                                    <div class="card-body card-body-sm">
                                                        <div class="form-row">
                                                            <asp:Repeater runat="server" ID="rptAddAddress">
                                                                <ItemTemplate>
                                                                    <div class="form-group col-md-3 col-sm-6 col-xs-12 textbox-address-input">
                                                                        <label>
                                                                            <%# Eval("Description") %>
                                                                        </label>
                                                                        <input type="text" class="form-control txt-add-address form-control-sm " data-propertycode="<%# Eval("PropertyCode") %>"
                                                                            data-description="<%# Eval("Description") %>" value="<%# Eval("PropertyValue") %>" placeholder='<%# GetPlaceHolderAddress(Eval("PropertyCode")) %>' />
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
									</ContentTemplate>
								</asp:UpdatePanel>
								<asp:UpdatePanel runat="server" UpdateMode="Conditional">
									<ContentTemplate>
                                        <asp:Button Text="" runat="server" CssClass="d-none" ID="btnCheckRequireFieldCreate" OnClick="btnCheckRequireFieldCrate_Click" />
										<asp:Button Text="" runat="server" CssClass="d-none" ID="btnCreateCustomerDetail" OnClick="btnCreateCustomerDetail_Click" />
                                        <% if(IsAllFeature) { %> 
                                        <asp:Button Text="" runat="server" CssClass="d-none AUTH_MODIFY" ID="btnSetDataOpenModalCreate" OnClientClick="AGLoading(true);" OnClick="btnSetDataOpenModalCreate_Click" />
                                        <% } %>
                                        <asp:Button ID="btnSetCustomerCodeRefConfig" CssClass="d-none" ClientIDMode="Static" OnClick="btnSetCustomerCodeRefConfig_Click" OnClientClick="AGLoading(true);" Text="" runat="server" />
									</ContentTemplate>
								</asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-bottom">
                    <div class="text-right">
						<span class="water-button" onclick="checkRequireFieldCreate();"><i class="fa fa-file-o"></i>&nbsp;Save</span>
                        <a class="water-button" onclick="closeInitiativeModal('modal-EditCustomerDetail');"><i class="fa fa-close"></i>&nbsp;Close</a>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <!-- /.modal -->


    <script>

        $(function () {
            $("#txtFirstname").on("keyup", function (event) {
                var currentEl = $(this);
                var value = $(currentEl).val();
                value = value.replace(/[\'\"]/g, "\"");
                $(currentEl).val(value);
            });
        });

        function resetTextForSearch()
        {
            $(".panel-body-customer-search input[type=text], .panel-body-customer-search select").val("");
        }

        function afterSearch() {
            var CustomerList = JSON.parse($("#divJsonCustomerList").html());
            var data = [];
            for (var i = 0 ; i < CustomerList.length ; i++) {
                var Customer = CustomerList[i];
                var flag = '';
                if (Customer.CustomerCritical == 'CRITICAL') {
                    flag = '<img src="/images/icon/flag-red-512.png" width="20" height="20">&nbsp;';
                }
                data.push([
                    Customer.CustomerCode,
                    flag + Customer.CustomerCode + " : " + Customer.FullName,
                    Customer.CustomerGroup + " : " + Customer.CustomerGroupDesc,
                    Customer.Address,
                    Customer.TaxID,
                    Customer.TelNo1,
                    Customer.Mobile,
                    Customer.EMail,
                    Customer.Active
                ]);
            }

            $("#search-panel").show();
            $("#table-customer").dataTable({
                data: data,
                deferRender: true,
                columnDefs: [{
                    "orderable": false,
                    "targets": [0],
                    "createdCell": function (td, cellData, rowData, row, col) {
                       
                        $(td).addClass("text-center");
                        $(td).html(
                            '<a href="Javascript:;" title="View Customer Detail">' +
                            '<i class="fa fa-user fa-lg text-dark c-pointer" aria-hidden="true"></i>' +
                            '</a>'
                         );
                        $(td).closest("tr").addClass("c-pointer");
                        $(td).closest("tr").data("key", rowData[0]);
                        $(td).closest("tr").click(function () {

                            let url = "<%= Page.ResolveUrl("~/crm/CustomerProfileDetail.aspx") %>?id=" + rowData[0];
                            goToEdit(url);
                        });
                    }
                }]
            });
        }

        function scrollToTable() {
            $('html,body').animate({
                scrollTop: $("#search-panel").offset().top - 50
            });
        }
    </script>

</asp:Content>
