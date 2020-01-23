<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopupCreateTierZeroControl.ascx.cs" Inherits="ServiceWeb.Tier.UserControl.PopupCreateTierZeroControl" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEquipment.ascx" TagPrefix="ag" TagName="AutoCompleteEquipment" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteCustomer.ascx" TagPrefix="ag" TagName="AutoCompleteCustomer" %>


<div class="d-none">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpData">
        <ContentTemplate>
            <asp:Button ID="btnSelectCriteriaBindPriority" ClientIDMode="Static"
                OnClick="ddlSelectBindPriority_SelectedIndexChanged"
                Text="btnChangeSelctPriorityCode" runat="server" />
            <asp:HiddenField ID="hdfSEQ_Target" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdfCloseCase" runat="server" ClientIDMode="Static" />
            <asp:Button ID="btnCreateClose" runat="server" ClientIDMode="Static" CssClass="d-none" OnClick="btnCreateClose_Click" />
            <asp:Button ID="btnCreateTicket" runat="server" ClientIDMode="Static" CssClass="d-none" OnClick="btnCreateTicket_Click" />
            <asp:Button Text="" runat="server" CssClass="d-none" ID="btnCreatedTicket" 
                OnClick="btnCreatedTicket_Click" />
            <button type="button" id="btnWarningCreate" class="d-none" onclick="warningCreate(this);"></button>
            <asp:Button ID="btnCreateWithWarning" runat="server" OnClick="btnCreateWithWarning_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div>
    
    <div class="modal fade" id="modal_Close_Case">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <h4 class="modal-title">Close Case</h4>
                    <button type="button" class="close" data-dismiss="modal">×</button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="udpddlclosecase" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-12">
                                    <label>ระบุสาเหตุในการปิดเคส</label>
                                    <asp:HiddenField ID="hdfdllCloaseCase" runat="server" ClientIDMode="Static" />
                                    <asp:DropDownList ID="ddlCloaseCase" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                    <%--OnSelectedIndexChanged="ddlCloaseCaseChange" AutoPostBack="true"--%>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <button type="button" class="btn btn-primary" onclick="createTierZeroClose_Submit();">
                                <i class="fa fa-save"></i>&nbsp;บันทึก
                            </button>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                <i class="fa fa-close"></i>&nbsp;ปิด
                            </button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="modal_Create_Ticket">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Create Ticket</h4>
                    <button type="button" class="close" data-dismiss="modal">×</button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpSubject">
                        <ContentTemplate>
                            <div class="form-row">
                                <div class="col-md-12 col-sm-12">
                                    <label>Subject</label>
                                    <asp:TextBox runat="server" ID="txtSubject" TextMode="MultiLine" Rows="1" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="col-md-12 col-sm-12">
                                    <label>Detail</label>
                                </div>
                                <asp:TextBox runat="server" ID="txtDetail" TextMode="MultiLine" Rows="3" CssClass="form-control" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="form-row">
                        <div class="col-md-6 col-sm-12">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCI">
                                <ContentTemplate>
                                    <div class="form-row">
                                        <div class="col-12">
                                            <label>Configuration Item</label>
                                            <ag:AutoCompleteEquipment ID="equipmentSelect" runat="server" CssClass="form-control form-control-sm"
                                                AfterSelectedChange="loadCustomerByEquipment();">
                                            </ag:AutoCompleteEquipment>
                                        </div>
                                        <div class="col-12">
                                            <label>Client</label>
                                            <ag:AutoCompleteCustomer runat="server" id="AutoCompleteCustomer" CssClass="form-control form-control-sm required"
                                                AfterSelectedChange="loadEquipmentAndContact();" />
                                            <%--<asp:TextBox runat="server" ID="txtCustomer" CssClass="form-control form-control-sm" Enabled="false" />
                                            <asp:HiddenField runat="server" ID="hddCustomerCode" />
                                            <asp:HiddenField runat="server" ID="hddCustomerName" />--%>
                                            <asp:UpdatePanel ID="updContactCus" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnBindContactCus" runat="server" CssClass="d-none" OnClick="btnBindContactCus_Click" />
                                                    <asp:Button ID="btnLoadCustomerEquipment" runat="server" CssClass="d-none" OnClick="btnLoadCustomerEquipment_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="col-12">
                                        </div>
                                        <div class="col-12">
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-md-6 col-sm-12">
                            <asp:UpdatePanel ID="udpnProblem" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="form-row">
                                        <div class="col-12">
                                            <label>Ticket Type</label>
                                            <asp:DropDownList ID="_ddl_sctype" runat="server" class="form-control form-control-sm required" ClientIDMode="Static"></asp:DropDownList>
                                        </div>
                                        <div class="col-12">
                                            <label>Impact</label>
                                            <asp:DropDownList ID="ddlImpact" runat="server" class="form-control form-control-sm required"
                                                onchange="$('#btnSelectCriteriaBindPriority').click();" ClientIDMode="Static">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-12">
                                            <label>Urgency</label>
                                            <asp:DropDownList ID="ddlUrgency" runat="server" class="form-control form-control-sm required"
                                                onchange="$('#btnSelectCriteriaBindPriority').click();" ClientIDMode="Static">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-12">
                                            <label>Priority</label>
                                            <asp:DropDownList ID="ddlPriority" runat="server" class="form-control form-control-sm required"
                                                DataTextField="Description" DataValueField="PriorityCode" ClientIDMode="Static">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="col-md-6 col-sm-12">

                        </div>
                        <div class="col-md-6 col-sm-12">

                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <button type="button" class="btn btn-primary" onclick="createClick();">
                                <i class="fa fa-file-o"></i>&nbsp;&nbsp;Create Ticket
                            </button>

                            <button type="button" class="btn btn-danger" data-dismiss="modal">
                                <i class="fa fa-close"></i>&nbsp;Close
                            </button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

            </div>
        </div>
    </div>

    <script>
        function rebindMasterDataList() {
            if ($("#btnRebindTierZeroList").length > 0) {
                $("#btnRebindTierZeroList").click();
            }
        }

        function createClick() {
            activeRequireField();
            $("#<%= btnCreatedTicket.ClientID %>").click();
        }

        function warningClick(docType, customer, equipment) {
            var warnMsg = "Ticket Type : " + docType + " , Client : " + customer;

            if (equipment != "") {
                warnMsg += " , Configuration Item : " + equipment;
            }

            warnMsg += " has an open ticket. Do you want to create new ticket ?";

            $("#btnWarningCreate").val(warnMsg);
            $("#btnWarningCreate").click();
        }

        function warningCreate(sender) {
            var warnMsg = $(sender).val();

            if (AGConfirm(sender, warnMsg)) {
                $("#<%= btnCreateWithWarning.ClientID %>").click();
            }
        }

        function createTierZeroClose(row, status) {
            $("#hdfSEQ_Target").val(row);
            if (status == "0") {
                $("#modal_Close_Case").modal("show");
            } else {
                AGError("Case ไม่ได้อยู่ในสถานะ OPEN");
            }
        }
        function createTierZeroTicket(row) {
            AGLoading(true);
            $("#hdfSEQ_Target").val(row);
            $("#btnCreateTicket").click();
            //location.reload();
        }

        function createTierZeroClose_Submit() {
            $("#modal_Close_Case").modal("hide");
            AGLoading(true);
            $("#btnCreateClose").click();
        }

        function activeRequireField() {
            $(".required").prop('required', true);
        }


        function loadCustomerByEquipment() {
            inactiveRequireField();
            $("#<%= btnLoadCustomerEquipment.ClientID %>").click();
        }

        function loadEquipmentAndContact() {
            inactiveRequireField();
            $("#<%= btnBindContactCus.ClientID %>").click();
        }
    </script>
</div>