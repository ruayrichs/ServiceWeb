<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddressUsercontrol.ascx.cs" Inherits="POSWeb.crm.usercontrol.AddressUsercontrol" %>

<style>

</style>

<script>
    function showEditAddressModal(numrow) {
        $("#panel-add-profile-address-modal-" + numrow).slideUp(false);
        $("#panel-edit-profile-address-modal-" + numrow).slideDown(true);
    }
    function hideEditAddressModal(numrow) {
        $("#panel-add-profile-address-modal-" + numrow).slideDown(false);
        $("#panel-edit-profile-address-modal-" + numrow).slideUp(true);
    }
    function showAddNewAddressModal() {
        $("#panel-tab-add-new-profile-address-modal").slideUp(false);
        $("#panel-add-new-profile-address-modal").slideDown(true);
    }
    function hideAddNewAddressModal() {
        $("#panel-tab-add-new-profile-address-modal").slideDown(true);
        $("#panel-add-new-profile-address-modal").slideUp(false);
    }


   
    function saveAddressModal(obj, className, rowhide, AddressModalCodeEdit) {
        AGLoading(true, 'กำลังบันทึก');
        $("#hddJsonAddressModal").val("");
        var jlistTotol = [];
        var dataClass = $('.' + className);
        var jSonArrList = [];
        for (var i = 0; i < dataClass.length; i++) {
            var jEntity = {};
            jEntity.PropertyCode = $(dataClass[i]).attr('data-PropertyCode');
            jEntity.Description = $(dataClass[i]).attr('data-Description');
            jEntity.PropertyValue = $(dataClass[i]).val();
            jSonArrList.push(jEntity);
        }
        
        var jdataArr = {};
        jdataArr.address = jSonArrList;
        jlistTotol.push(jdataArr);

        $("#hddJsonAddressModal").val(JSON.stringify(jlistTotol));
        if (obj != '') {
            hideEditAddressModal(rowhide);
            $("#hddAddressModalCodeEdit").val(AddressModalCodeEdit);
            $("#btnEditMOdal").click();
        } else {
            hideAddNewAddressModal();
            $("#hddAddressModalCodeEdit").val(document.getElementById("ddlTypeAddressModalAddNew").value);
            $("#btnAddModal").click();
        }
    }

</script>


<div class="row">
    <asp:HiddenField runat="server" ID="hddJsonAddressModal" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hddAddressModalCodeEdit" ClientIDMode="Static" />
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button Text="เพิ่มข้อมูล" runat="server" ID="btnAddModal" ClientIDMode="Static" OnClick="btnAddModal_Click" CssClass="hide" />
            <asp:Button Text="แก้ไขข้อมูล" runat="server" ID="btnEditMOdal" ClientIDMode="Static" OnClick="btnEditMOdal_Click" CssClass="hide" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="udpAddressModal" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Repeater runat="server" ID="rptAddressModal" OnItemDataBound="rptAddressModal_ItemDataBound">
                <ItemTemplate>
                    <div class="col-md-12">
                        <label>
                            <%# Eval("AddressName") %>
                        </label>
                        <div class="pull-right" style="cursor: pointer;" onclick="showEditAddressModal('<%# (Container.ItemIndex + 1) %>');">
                            <span class="fa fa-cog fa-fw"></span>
                            <span>แก้ไข</span>
                        </div>
                    </div>
                    <asp:HiddenField runat="server" Value='<%# Eval("AddressCode") %>' ID="hddAddressModalCode" />
                    <!-- Display Address -->
                    <div id="panel-add-profile-address-modal-<%# (Container.ItemIndex + 1) %>" class="row">
                        <asp:Repeater runat="server" ID="rptDetail">
                            <ItemTemplate>
                                <div style="margin-bottom: 10px;">
                                    <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <label>
                                                <%# Eval("Description") %>
                                            </label>
                                        </div>
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                                            <asp:Label runat="server" CssClass="text-primary wordbreak" Text='<%# !string.IsNullOrEmpty(Eval("PropertyValue").ToString()) ? Eval("PropertyValue").ToString() : "-" %>' />
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <!-- Edit Address -->
                    <div id="panel-edit-profile-address-modal-<%# (Container.ItemIndex + 1) %>" style="display: none;">
                        <asp:Repeater runat="server" ID="rptAddressModalEdit">
                            <ItemTemplate>
                                <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                    <label>
                                        <%# Eval("Description") %>
                                    </label>
                                    <input type="text" class="form-control txt-edit-address-modal-<%# Eval("AddressCode") %>" data-propertycode="<%# Eval("PropertyCode") %>" placeholder="กรอก<%# Eval("Description") %> ที่นี้"
                                        data-description="<%# Eval("Description") %>" value="<%# Eval("PropertyValue") %>" />
                                    <div style="margin-bottom: 10px;"></div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <div class="col-md-12">
                            <div class="text-right">
                                <button type="button" class="btn btn-success" onclick="saveAddressModal($(this),'<%# "txt-edit-address-modal-" + Eval("AddressCode") %>','<%# (Container.ItemIndex + 1) %>','<%# Eval("AddressCode") %>');">Save</button>
                                &nbsp;&nbsp;
                                            <button type="button" class="btn btn-danger" onclick="hideEditAddressModal('<%# (Container.ItemIndex + 1) %>');">Cancel</button>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-12">
                        <hr />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- btn Add New Address -->
    <div id="panel-tab-add-new-profile-address-modal">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <div class="btn-dashed" onclick="showAddNewAddressModal();">
                        <a href="Javascript:;">
                            <i class="fa fa-plus fa-fw"></i>
                            เพิ่มที่อยู่
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Panel Add New Address -->
    <div id="panel-add-new-profile-address-modal" style="display: none;">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                    <label>
                        ประเภทที่อยู่
                    </label>
                    <asp:DropDownList runat="server" ID="ddlTypeAddressModalAddNew" ClientIDMode="Static" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <asp:Repeater runat="server" ID="rptAddAddressModal">
                    <ItemTemplate>
                        <div class="textbox-address-modal-input">
                            <div class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                                <label>
                                    <%# Eval("Description") %>
                                </label>
                                <input type="text" class="form-control txt-add-address-modal" data-propertycode="<%# Eval("PropertyCode") %>" placeholder="กรอก<%# Eval("Description") %> ที่นี้"
                                    data-description="<%# Eval("Description") %>" value="<%# Eval("PropertyValue") %>" />
                                <div style="margin-bottom: 10px;"></div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <div class="text-right">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField runat="server" ID="hddValueAddressModal" ClientIDMode="Static" />
                                <button type="button" ID="btnAddnewAddressModal" class="btn btn-primary"
                                    onclick="saveAddressModal('', 'txt-add-address-modal','','');" >Save</button>
                                &nbsp;&nbsp;
                                            <button type="button" class="btn btn-danger " onclick="hideAddNewAddressModal();">Cancel</button>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

