<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchCustomerControl.ascx.cs" Inherits="ServiceWeb.widget.usercontrol.SearchCustomerControl" %>

 <asp:UpdatePanel ID="udpUpdateCustomerCriteria" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-row">
                <div class="col-lg-12">
                    <div class="form-row">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label>Client Code / Name</label>
                                <asp:TextBox ID="txtFirstname" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Client Group</label>
                                <asp:DropDownList runat="server" ID="ddlCustomerGroup" CssClass="form-control form-control-sm">
                                </asp:DropDownList>
                            </div>
                            <div class="form-row">
                                <div class="form-group col-md-6 col-sm-12">
                                    <label>Sale Area</label>
                                    <asp:DropDownList runat="server" ID="ddlSaleDistrict" CssClass="form-control form-control-sm"
                                        DataValueField="SAREACODE" DataTextField="CodeAndDesc">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group col-md-6 col-sm-12">
                                    <label>Provice</label>
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label>Work Phone</label>
                                <asp:TextBox ID="txtPhone" onkeypress="return isNumberKey(event);" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Mobile Phone</label>
                                <asp:TextBox ID="txtPhoneMobile" onkeypress="return isNumberKey(event);" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>E-mail</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-3">
                                <label>Contact Name</label>
                                <asp:TextBox ID="txtContactNameSearchCustomer" CssClass="form-control form-control-sm" runat="server"
                                    placeholder="ชื่อ" />
                            </div>
                            <div class="form-group col-md-3">
                                <label>Contact NickName</label>
                                <asp:TextBox ID="txtContactNickNameSearchCustomer" CssClass="form-control form-control-sm" runat="server"
                                    placeholder="ชื่อเล่น" />
                            </div>
                            <div class="form-group col-md-3">
                                <label>Contact Phone</label>
                                <asp:TextBox ID="txtContactPhoneSearchCustomer" onkeypress="return isNumberKey(event);" CssClass="form-control form-control-sm" runat="server"
                                    placeholder="เบอร์โทร" />
                            </div>
                            <div class="form-group col-md-3">
                                <label>Contact Email</label>
                                <asp:TextBox ID="txtContactEmailSearchCustomer" CssClass="form-control form-control-sm " runat="server"
                                    placeholder="อีเมล์" />
                            </div>
                            <div class="form-group col-md-3">
                                <label>Contact Position</label>
                                <asp:TextBox ID="txtContactPOSITIONSearchCustomer" CssClass="form-control form-control-sm" runat="server"
                                    placeholder="ตำแหน่ง" />
                            </div>
                            <div class="form-group col-md-3">
                                <label>Authorization Contact</label>
                                <asp:DropDownList ID="ddlContactAuthorization" CssClass="form-control form-control-sm" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Contact Remark</label>
                                <asp:TextBox ID="txtContactRemarkSearchCustomer" CssClass="form-control form-control-sm" runat="server"
                                    placeholder="หมายเหตุ" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-row">
                <div class="form-group col-sm-12 col-md-6">
                    <button type="button" class="btn btn-info" onclick="$(this).next().click();"><i class="fa fa-search"></i>&nbsp;&nbsp;Search</button>
                    <asp:Button ID="btnSearchCustomerCriteria" CssClass="d-none" OnClick="btnSearchCustomerCriteria_Click" OnClientClick="AGLoading(true);" runat="server" />
                    <button type="reset" class="btn btn-warning" onclick="resetTextForCustomerCriteriaSearch();"><i class="fa fa-refresh"></i>&nbsp;&nbsp;Clear</button>


                    <asp:HiddenField ID="hddCustomerCodeSelected" runat="server" />
                    <asp:Button ID="btnSelectCustomerCriteria" OnClick="btnSelectCustomerCriteria_Click" OnClientClick="AGLoading(true);" CssClass="d-none" runat="server" />
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="form-row">
        <div class="form-group col-sm-12 col-md-12">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpSearchCustomerCriteria">
                <ContentTemplate>
                    <div class="table-responsive">
                        <table id="tableMasterCustomerCriteria" class="table table-bordered table-striped table-hover table-sm" style="width: 100%;">
                            <thead>
                                <tr>
                                    <th class="text-center text-nowrap">Detail</th>
                                    <th>Client Code / Name</th>
                                    <th>Client Group</th>
                                    <th>Address</th>
                                    <th>Tel.</th>
                                    <th>E-mail</th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                    
    <div>
        <asp:TextBox runat="server" ID="txtSearchCusSelect" name="txtSearchCusSelect" ClientIDMode="Static" CssClass="d-none" />
          </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <script type="text/javascript">

        function afterSearchBindCustomerCriteria(dataArr) {
            var data = [];
            for (var i = 0 ; i < dataArr.length ; i++) {
                var Customer = dataArr[i];
                data.push([
                    Customer.CustomerCode,
                    Customer.CustomerCode + " : " + Customer.FullName,
                    Customer.CustomerGroup + " : " + Customer.CustomerGroupDesc,
                    Customer.Address,
                    Customer.TelNo1,
                    Customer.EMail
                ]);
            }

            $("#tableMasterCustomerCriteria").dataTable({
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
                                   var arrCUS = [];
                                   if ($("#txtSearchCusSelect").val() != "") {
                                       arrCUS = $("#txtSearchCusSelect").val().split(',');
                                   }
                                   if ($(this).prop("checked")) {
                                       arrCUS.push($(this).val());
                                   } else {
                                       var index = arrCUS.indexOf($(this).val());
                                       if (index > -1) {
                                           arrCUS.splice(index, 1);
                                       }
                                   }
                                   $("#txtSearchCusSelect").val(arrCUS.toString());
                               });
                               $(td).html('');
                               $(td).append(checkbox);
                               $(td).addClass("text-center");
                           }
                       }
                ]
            });
        }
    </script>
