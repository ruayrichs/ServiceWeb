<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="EquipmentLocationMaster.aspx.cs" Inherits="ServiceWeb.MasterConfig.EquipmentLocationMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-master-ci-location").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <button type="button" class="btn btn-primary mb-1 AUTH_MODIFY" onclick="$(this).next().click();">
                        <i class="fa fa-plus-circle"></i>&nbsp;&nbsp;New Location
                    </button>
                    <asp:Button OnClick="btnNew_Click" Text="New" runat="server" class="btn btn-success d-none" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>
    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Equipment Location Master</h5>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="udpMasterConfig" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="">
                                <asp:GridView ID="tableDataLocation" class="table table-striped table-bordered table-sm" 
                                    runat="server" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="LocationCode" HeaderText="Location Code" HeaderStyle-Width="100" />
                                        <asp:BoundField DataField="LocationName" HeaderText="Location Name" />
                                        <asp:BoundField DataField="plant" HeaderText="Plant" />
                                        <asp:BoundField DataField="Location" HeaderText="Location" />
                                        <asp:BoundField DataField="LocationCategory" HeaderText="Location Category" />
                                        <asp:BoundField DataField="Room" HeaderText="Room" />
                                        <asp:BoundField DataField="Shelf" HeaderText="Shelf" />
                                        <asp:BoundField DataField="WorkCenter" HeaderText="WorkCenter" />
                                        <asp:TemplateField>
                                            <HeaderStyle Width="1" />
                                            <HeaderTemplate>
                                                Edit
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button Text="Edit" OnClick="btnEdit_Click" runat="server" type="button" class="btn btn-info btn-sm" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="1" />
                                            <HeaderTemplate>
                                                Copy
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button Text="Copy" OnClick="btnCopy_Click" runat="server" type="button" class="btn btn-info btn-sm" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderStyle Width="1" />
                                            <HeaderTemplate>
                                                Delete
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button OnClientClick="return AGConfirm(this,'ต้องการลบหรือไม่ !!');AGLoading(true);" 
                                                            Text="Delete" OnClick="btnDelete_Click" runat="server" type="button" class="btn btn-danger btn-sm" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                            </div>
                        </ContentTemplate>

                    </asp:UpdatePanel>

                </div>
                <!-- <button type="button" class="btn btn-info" data-toggle="modal" data-target="#myModal">Open Modal</button> -->
            </div>
        </div>
    </div>

    <div id="myModal" class="modal fade dialog-lg" role="dialog" >
        <div class="modal-dialog" >
            <!-- Modal content-->
            <div class="modal-content" style="width: 1300px; margin-left: -400px ">
                <div class="modal-header">
                    <h4 class="modal-title">Modal Header</h4>
                    <button type="button" class="close" data-dismiss="modal">×</button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="btnDataLocationPopup">
                        <ContentTemplate>
                            <div class="form-group row">
                                <label for="plant-input" class="col-2 col-form-label">Location description</label>
                                <div class="col-10">
                                    <asp:TextBox runat="server" class="form-control form-control-sm" CssClass="" ID="txtBoxLocationName" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="plant-input" class="col-2 col-form-label">Plant</label>
                                <div class="col-4">
                                    <asp:TextBox runat="server" class="form-control form-control-sm" CssClass="d-none" ID="txtBoxLocationId" />
                                    <asp:TextBox runat="server" ID="txtBoxPlant" class="form-control form-control-sm"></asp:TextBox>
                                </div>
                                <label for="location-input" class="col-2 col-form-label">Location</label>
                                <div class="col-4">
                                    <asp:TextBox runat="server" class="form-control form-control-sm" ID="txtBoxLocation" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="locationcate-input" class="col-2 col-form-label">Location Category</label>
                                <div class="col-4">
                                    <asp:TextBox runat="server" class="form-control form-control-sm" ID="txtBoxLocationCategory" />
                                </div>
                                <label for="room-input" class="col-2 col-form-label">Room</label>
                                <div class="col-4">
                                    <asp:TextBox runat="server" class="form-control form-control-sm" ID="txtBoxRoom" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="shelf-input" class="col-2 col-form-label">Shelf</label>
                                <div class="col-4">
                                    <asp:TextBox runat="server" class="form-control form-control-sm" ID="txtBoxShelf" />
                                </div>
                                <label for="work-center-input" class="col-2 col-form-label">Work Center</label>
                                <div class="col-4">
                                    <asp:TextBox runat="server" class="form-control form-control-sm" ID="txtBoxWorkCenter" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="slot-input" class="col-2 col-form-label">Slot</label>
                                <div class="col-4">
                                    <asp:TextBox runat="server" class="form-control form-control-sm" ID="txtBoxSlot" />
                                </div>
                                <label for="address-name-1-input" class="col-2 col-form-label">Address Name 1</label>
                                <div class="col-4">
                                    <asp:TextBox runat="server" class="form-control form-control-sm" ID="txtBoxAddressName1" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="address-name-2-input" class="col-2 col-form-label">Address Name 2</label>
                                <div class="col-4">
                                    <asp:TextBox runat="server" class="form-control form-control-sm" ID="txtBoxAddressName2" />
                                </div>
                                <label for="zip-code-input" class="col-2 col-form-label">Zip Code</label>
                                <div class="col-4">
                                    <asp:TextBox runat="server" class="form-control form-control-sm" ID="txtBoxZipcode" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="city-input" class="col-2 col-form-label">City</label>
                                <div class="col-4">
                                    <asp:TextBox runat="server" class="form-control form-control-sm" ID="txtBoxCity" />
                                </div>
                                <label for="street-input" class="col-2 col-form-label">Street</label>
                                <div class="col-4">
                                    <asp:TextBox runat="server" class="form-control form-control-sm" ID="txtBoxStreet" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="telephone-input" class="col-2 col-form-label">Telephone</label>
                                <div class="col-4">
                                    <asp:TextBox runat="server" class="form-control form-control-sm" ID="txtBoxTelephone" />
                                </div>
                                <label for="fax-input" class="col-2 col-form-label">Fax</label>
                                <div class="col-4">
                                    <asp:TextBox runat="server" class="form-control form-control-sm" ID="txtBoxFax" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="btnButtonLocationPopup">
                        <ContentTemplate>
                            <asp:Button Text="New" OnClientClick="return AGConfirm(this,'ต้องการบันทึกหรือไม่ !!');AGLoading(true);" OnClick="btnSave_Click" runat="server" ID="btnNew" class="btn btn-success" />
                            <asp:Button Text="Save" OnClientClick="return AGConfirm(this,'ต้องการบันทึกหรือไม่ !!');AGLoading(true);" runat="server" OnClick="btnUpdate_Click" ID="btnSave" class="btn btn-success" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <script>

        function bindingDataTableJS() {
            $("#<%= tableDataLocation.ClientID %>").dataTable({
                columnDefs: [{
                    "orderable": true,
                    "targets": [0]
                }],
                "order": [[0, "asc"]]
            });
        }

        function openModal(nameid) {
            $("#" + nameid).modal("show");
        }
        function closeModal(nameid) {
            $("#" + nameid).modal("hide");
        }
    </script>
</asp:Content>
