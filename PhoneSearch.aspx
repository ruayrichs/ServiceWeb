<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/IncludeLibraryMaster.Master" AutoEventWireup="true" CodeBehind="PhoneSearch.aspx.cs" Inherits="ServiceWeb.PhoneSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IncludeLibraryPlaceHolder" runat="server">
    
    <script>
        function webOnLoad() {
            //clear old active
            //var onav = document.getElementsByClassName("nav-link active")[0].id;
            //document.getElementById(onav).className = "nav-link";
            //set new active
            //document.getElementById("nav-menu-phone").className = "nav-link active";
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
        function scrollToTable() {
            $('html,body').animate({
                scrollTop: $("#search-panel").offset().top - 50
            });
        }
    </script>
    <div style="width: 95%; padding-right: 5%; float:right" >
        <div>
            <%--<input type="type" name="name" value="" />--%>
            <asp:label CssClass="d-none" text="?ContactPhone=%20" runat="server" />
            <asp:TextBox value="" ID="txtContactPhone" CssClass="form-control form-control-sm d-none" runat="server" placeholder="Name" ClientIDMode="Static" />
            <asp:TextBox ID="txtContactName" CssClass="form-control form-control-sm d-none" runat="server" placeholder="Name" ClientIDMode="Static" />
            <asp:TextBox ID="txtContactNickName" CssClass="form-control form-control-sm d-none" runat="server" placeholder="Name" ClientIDMode="Static" />
            <asp:TextBox ID="txtContactPOSITION" CssClass="form-control form-control-sm d-none" runat="server" placeholder="Name" ClientIDMode="Static" />
            <asp:TextBox ID="txtContactRemark" CssClass="form-control form-control-sm d-none" runat="server" placeholder="Name" ClientIDMode="Static" />
            <asp:TextBox ID="txtContactEmail" CssClass="form-control form-control-sm d-none" runat="server" placeholder="Name" ClientIDMode="Static" />
            <asp:DropDownList ID="ddlContactAuthorization" CssClass="form-control form-control-sm d-none" runat="server"></asp:DropDownList>
            <asp:TextBox ID="txtPhone" CssClass="form-control form-control-sm d-none" runat="server" placeholder="Name" ClientIDMode="Static" />
            <asp:TextBox ID="txtAddress" CssClass="form-control form-control-sm d-none" runat="server" placeholder="Name" ClientIDMode="Static" />
            <asp:TextBox ID="txtEmail" CssClass="form-control form-control-sm d-none" runat="server" placeholder="Name" ClientIDMode="Static" />
            <asp:TextBox ID="txtTaxID" CssClass="form-control form-control-sm d-none" runat="server" placeholder="Name" ClientIDMode="Static" />
            <asp:TextBox ID="txtFirstname" CssClass="form-control form-control-sm d-none" runat="server" placeholder="Name" ClientIDMode="Static" />
            <asp:DropDownList ID="ddlCustomerGroup" CssClass="form-control form-control-sm d-none" runat="server"></asp:DropDownList>
            <asp:DropDownList ID="ddlSaleDistrict" CssClass="form-control form-control-sm d-none" runat="server"></asp:DropDownList>
            <asp:label text="&PhoneMobile=" CssClass="d-none" runat="server" />
            <asp:TextBox ID="txtPhoneMobile" CssClass="form-control form-control-sm d-none" runat="server" placeholder="Name" ClientIDMode="Static" />
            <asp:DropDownList ID="ddlCustomerActive" CssClass="form-control form-control-sm d-none" runat="server"></asp:DropDownList>
            <asp:DropDownList ID="ddlOwnerService" CssClass="form-control form-control-sm d-none" runat="server"></asp:DropDownList>

        </div>
        <div>
            <asp:button CssClass="d-none" id="btnSearchByPhone" text="Search" runat="server" OnClick="btnSearchByPhone_Click" />
        </div>
        <div id="showtxt">
        </div>


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

    <script>
        function activeRequireField() {
            $(".required").prop('required', true);
        }

        function inactiveRequireField() {
            $(".required").prop('required', false);
        }

        function checkRequireFieldCreate() {
            activeRequireField();
            <%--$("#<%= btnCheckRequireFieldCreate.ClientID %>").click();--%>
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
                <%--$("#<%= btnCreateCustomerDetail.ClientID %>").click();--%>
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

        function resetTextForSearch() {
            $(".panel-body-customer-search input[type=text], .panel-body-customer-search select").val("");
        }

        function afterSearch() {
            var CustomerList = JSON.parse($("#divJsonCustomerList").html());
            var data = [];
            for (var i = 0; i < CustomerList.length; i++) {
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

        function goToEdit(url) {

            var height = document.documentElement.clientHeight;
            window.open(url, '_blank', 'location=yes,height=' + height + ',width=1200,scrollbars=yes,status=yes');
        }
    </script>

</asp:Content>
