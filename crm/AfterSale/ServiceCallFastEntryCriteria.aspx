<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="ServiceCallFastEntryCriteria.aspx.cs" Inherits="ServiceWeb.crm.AfterSale.ServiceCallFastEntryCriteria" %>

<%@ Register Src="~/widget/usercontrol/AttachFileUserControl.ascx" TagName="AttachFileUserControl" TagPrefix="sna" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEquipment.ascx" TagPrefix="ag" TagName="AutoCompleteEquipment" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteCustomer.ascx" TagPrefix="ag" TagName="AutoCompleteCustomer" %>
<%@ Register Src="~/widget/usercontrol/AutoCompleteControl.ascx" TagPrefix="ag" TagName="AutoCompleteControl" %>
<%@ Register Src="~/UserControl/AGapeGallery/UploadGallery/UploadGallery.ascx" TagPrefix="ag" TagName="UploadGallery" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            var pagaName = '<%= Session["TK_BusinessObject"]%>';
            //console.log(pagaName);
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            if (pagaName === 'INCIDENT') {
                document.getElementById("btnL_incident").className = "nav-link active";
            }
            else if (pagaName === 'PROBLEM') {
                document.getElementById("btnL_Problem").className = "nav-link active";
            }
            else if (pagaName === 'REQUEST') {
                document.getElementById("brnL_Request").className = "nav-link active";
            } else {
                //
            }
            
        };

        $(document).ready(function () {
            webOnLoad();
            $("#ddlImpact").val("02");
            $("#ddlUrgency").val("02");
            $("#ddlPriority").val("03");

            //loadEquipmentAndContactAuto();
        });        
        function UserLV(lv) {
            if (lv == '1') {
                $("#_ddl_sctype").prop("disabled", true);
                $("#ddlImpact").prop("disabled", true);
                $("#ddlUrgency").prop("disabled", true);
                $("#ddlPriority").prop("disabled", true);
                //hideTicketType();
            }
        };
        function hideTicketType() {
            $("#_ddl_sctype").parent().parent().parent().parent().parent().hide();
            $("#panel-client").parent().parent().parent().parent().attr('class', 'col-lg-12');
        };
    </script>
    <style>
        .card-body-sm {
            padding: 1rem;
            padding-bottom: 0;
        }

        .input-group > .autocomplete-box {
            flex: 1 1 auto;
            width: 1%;
            margin-bottom: 0;
        }
    </style>
    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Ticket Service</h5>
                </div>
                <div class="card-body">
                    <script>
                        function OpenLoadPanelConfigurationItem() {
                            //$("#<%= equipmentSelect.BoxSearchID %>").AGWhiteLoading(true);
                        }
                        function OpenLoadPanelClient() {
                            //$("#<%= CustomerSelect.BoxSearchID %>").AGWhiteLoading(true);
                        }
                        function OpenLoadPanelContact() {
                            //$("#<%= _ddl_contact_person.BoxSearchID %>").AGWhiteLoading(true);
                        }
                        function CloseloadAllPanel() {
                            //$("#<%= equipmentSelect.BoxSearchID %>").AGWhiteLoading(false);
                            //$("#<%= CustomerSelect.BoxSearchID %>").AGWhiteLoading(false);
                            //$("#<%= _ddl_contact_person.BoxSearchID %>").AGWhiteLoading(false);
                        }
                    </script>
                    <div class="form-row">

                        <div class="col-lg-6">

                            <div class="card border-primary" style="margin-bottom: 10px;">
                                <div class="card-body card-body-sm">
                                    <div class="form-row">
                                        <div class="form-group col-sm-12" >
                                            <label>Ticket Type</label>
                                            <asp:DropDownList ID="_ddl_sctype" runat="server"  class="form-control form-control-sm required disabled" ClientIDMode="Static"></asp:DropDownList>
                                        </div>
                                        <div id="panel-client" class="form-group col-lg-12 col-sm-12">
                                            <label>Client</label>
                                            <div class="input-group">
                                                <ag:AutoCompleteCustomer ID="CustomerSelect" runat="server" CssClass="form-control form-control-sm required"
                                                    AfterSelectedChange="loadEquipmentAndContact();">
                                                </ag:AutoCompleteCustomer>
                                                <div class="input-group-append">
                                                    <span class="input-group-text" onclick="setModalSearch('CREATE');$('#<%= btnOpenModalSelectCustomerCriteria.ClientID%>').click();"><i class="fa fa-search"></i></span>
                                                </div>
                                                <asp:TextBox ID="CustomerSelectNew" Enabled="false" runat="server" CssClass="form-control form-control-sm d-none"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div id="panel-contact" class="form-group col-lg-6 col-sm-12 d-none">
                                            <label>Contact</label>
                                            <ag:AutoCompleteControl runat="server" id="_ddl_contact_person"
                                                CustomViewCode="contact"
                                                TODO_FunctionJS="loadcontactDetailBySelected();" CssClass="form-control form-control-sm" />
                                        </div>
                                        <div class="col-lg-12 d-none">
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpContactDetail">
                                                <ContentTemplate>
                                                    <div class="form-row">
                                                        <div class="form-group col-lg-6 col-sm-12">
                                                            <label>Contact E-Mail</label>
                                                            <asp:TextBox ID="txtContactEmail" Enabled="false" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group col-lg-6 col-sm-12">
                                                            <label>Contact Phone</label>
                                                            <asp:TextBox ID="txtContactPhone" Enabled="false" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                        <div class="form-group col-lg-12">
                                                            <label>Contact Remark</label>
                                                            <asp:TextBox ID="txtContactremark" Enabled="false" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div id="panel-configuration-item" class="form-group col-lg-12">
                                            <label>Configuration Item</label>
                                            <div class="input-group">
                                                <ag:AutoCompleteEquipment ID="equipmentSelect" runat="server" CssClass="form-control form-control-sm"
                                                    AfterSelectedChange="loadCustomerByEquipment();">
                                                </ag:AutoCompleteEquipment>
                                                <div class="input-group-append c-pointer">
                                                    <span class="input-group-text" onclick="setModalSearch('CREATE');$('#<%= btnOpenModalSelectConfigurationItem.ClientID%>').click();"><i class="fa fa-search"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <asp:UpdatePanel ID="updContactCus" runat="server" UpdateMode="Conditional" class="d-none">
                                <ContentTemplate>
                                    <asp:Button ID="btnSelectContactBindDetail" CssClass="d-none" OnClick="btnSelectContactBindDetail_Click" runat="server" />
                                    <asp:Button ID="btnBindContactCus" runat="server" CssClass="d-none" OnClick="btnBindContactCus_Click" OnClientClick="OpenLoadPanelConfigurationItem();OpenLoadPanelContact();" />
                                    <asp:Button ID="btnBindContactCusAuto" runat="server" CssClass="d-none" OnClick="btnBindContactCusAuto_Click" OnClientClick="OpenLoadPanelConfigurationItem();OpenLoadPanelContact();" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <div class="col-lg-6">

                            <div class="card border-primary" style="margin-bottom: 10px;">
                                <div class="card-body card-body-sm">
                                    <div class="form-row">
                                        <%--<div class="form-group col-sm-12" >
                                            <label>Ticket Type</label>
                                            <asp:DropDownList ID="_ddl_sctype" runat="server"  class="form-control form-control-sm required disabled" ClientIDMode="Static"></asp:DropDownList>
                                        </div>--%>
                                        <div class="form-group col-sm-4 d-none">
                                            <label>Fiscal Year</label>
                                            <input id="_txt_year" type="text" class="form-control form-control-sm required" runat="server" clientidmode="Static"
                                                onkeypress='return event.charCode >= 48 && event.charCode <= 57' />
                                        </div>
                                    </div>
                                    <asp:UpdatePanel ID="udpnProblem" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="form-row">
                                                <div class="form-group col-sm-12">
                                                    <label>Impact</label>
                                                    <asp:DropDownList ID="ddlImpact" runat="server" class="form-control form-control-sm required"
                                                        onchange="$('#btnSelectCriteriaBindPriority').click();" ClientIDMode="Static">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="form-group col-sm-12">
                                                    <label>Urgency</label>
                                                    <asp:DropDownList ID="ddlUrgency" runat="server" class="form-control form-control-sm required"
                                                        onchange="$('#btnSelectCriteriaBindPriority').click();" ClientIDMode="Static">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="form-group col-sm-12">
                                                    <label>Priority</label>
                                                    <asp:DropDownList ID="ddlPriority" runat="server" class="form-control form-control-sm required"
                                                        DataTextField="Description" DataValueField="PriorityCode" ClientIDMode="Static">
                                                    </asp:DropDownList>
                                                    <%--PriorityCode--%>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="">
                        <div class="card border-primary" style="margin-bottom: 10px;">
                            <div class="card-body card-body-sm">
                                <div class="form-group control-max-length">
                                    <asp:Label runat="server" ID="labelProblem" Text="Subject"></asp:Label>
                                    <small style="color: #aaa;">&nbsp;(<span class="Count-MaxLength-Remark"><%= txt_problem_topic.Text.Length%></span>/1000)</small>
                                    <asp:TextBox ID="txt_problem_topic" runat="server" CssClass="form-control form-control-sm required ticket-allow-editor"
                                        onkeypress="return countMaxLengthRemark(event)" onkeyup="validateMaxLengthRemark(event);"
                                        Rows="1" Style="resize: none;" data-maxlength="1000"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hddOldValue_problem_topic" />
                                </div>

                                <div class="form-group control-max-length">
                                    <asp:Label runat="server" ID="labelDescription" Text="Description"></asp:Label>
                                    <small style="color: #aaa;">&nbsp;(<span class="Count-MaxLength-Remark"><%= tbEquipmentRemark.Text.Length%></span>/8000)</small>
                                    <asp:TextBox ID="tbEquipmentRemark" runat="server" CssClass="form-control form-control-sm ticket-allow-editor txt-ticket-Descript"
                                        onkeypress="return countMaxLengthRemark(event)" onkeyup="validateMaxLengthRemark(event);"
                                        TextMode="MultiLine" Rows="3" Style="resize: none; height: 250px;" data-maxlength="8000"></asp:TextBox>
                                    <asp:Panel ID="galleryLoad" runat="server" CssClass="d-none">
                                        <br />
                                        <ag:uploadgallery runat="server" id="UploadGallery" MultipleMode="true" />
                                        <label>Upload your images or files here.</label>
                                    </asp:Panel>                                                                        

                                    <asp:HiddenField runat="server" ID="hddOldValue_EquipmentRemark" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <button type="button" class="btn btn-success AUTH_MODIFY" onclick="createClick();"><i class="fa fa-file-o"></i>&nbsp;&nbsp;Create Ticket</button>
                    <button type="button" id="btnWarningCreate" class="d-none" onclick="warningCreate(this);"></button>
                    <button type="button" class="btn btn-info AUTH_MODIFY d-none" onclick="inactiveRefquireField();$('#<%= btnOpenModalSearch.ClientID%>').click();"><i class="fa fa-search"></i>&nbsp;&nbsp;Search</button>
                    <button type="reset" class="btn btn-warning" onclick="resetText();"><i class="fa fa-refresh"></i>&nbsp;&nbsp;Clear</button>

                    
                    <div id="search-panel" style="margin-top:18px;">
                        <hr />
                        <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="table-responsive">
                                    <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                        <thead>
                                            <tr>
                                                <th class="text-nowrap"></th>
                                                <th class="text-nowrap">Date</th>
                                                <th class="text-nowrap">Ticket Type</th>
                                                <th class="d-none">Ticket No.</th>
                                                <th class="text-nowrap">Ticket No.</th>
                                                <th class="text-nowrap">Status</th>
                                                <th class="text-nowrap">Subject</th>
                                                <th class="text-nowrap">Client</th>
                                                <th class="text-nowrap">Contact</th>
                                                <th class="text-nowrap">Configuration Item</th>
                                                <th class="text-nowrap">Impact</th>
                                                <th class="text-nowrap">Urgency</th>
                                                <th class="text-nowrap">Priority</th>
                                                <th class="text-nowrap">Owner Service</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptSearchSale" runat="server">
                                                <ItemTemplate>
                                                    <tr class="<%# GetRowColorAssign(Eval("CallStatus").ToString())%> c-pointer"
                                                        onclick="openTicket('<%# Eval("CallerID")%>');">
                                                        <td class="text-nowrap text-center align-middle">
                                                            <i class="fa fa-pencil-square fa-lg text-dark" title="Edit"></i>
                                                        </td>
                                                        <td class="text-nowrap">
                                                            <%# Eval("DOCDATE").ToString()%>
                                                        </td>
                                                        <td class="text-nowrap">
                                                            <%# Eval("DocumentTypeDesc")%>
                                                        </td>
                                                        <td class="d-none">
                                                            <%# Eval("CallerID")%>
                                                        </td>
                                                        <td class="text-nowrap">
                                                            <%# Eval("CallerIDDisplay")%>
                                                        </td>
                                                        <td class="text-nowrap">
                                                            <%# Eval("DocStatusDesc")%>
                                                        </td>
                                                        <td class="text-truncate" style="max-width: 500px;">
                                                            <span title="<%# Eval("HeaderText")%>"><%# Eval("HeaderText")%></span>
                                                        </td>
                                                        <td class="text-nowrap">
                                                            <span class="<%# Eval("CustomerCritical").ToString() == "CRITICAL" ? "" : "d-none" %>">
                                                                <img src="/images/icon/flag-red-512.png" width="20" height="20">
                                                                &nbsp;
                                                            </span>
                                                            <%# Eval("CustomerName")%>
                                                        </td>
                                                        <td class="text-nowrap">
                                                            <%# Eval("ContractPersonName")%>
                                                        </td>
                                                        <td class="text-nowrap">
                                                            <%# Eval("EquipmentName")%>
                                                        </td>
                                                        <td class="text-nowrap">
                                                            <%# Eval("ImpactName")%>
                                                        </td>
                                                        <td class="text-nowrap">
                                                            <%# Eval("UrgencyName")%>
                                                        </td>
                                                        <td class="text-nowrap">
                                                            <%# Eval("PriorityName")%>
                                                        </td>
                                                        <td class="text-nowrap">
                                                            <%# Eval("OwnerGroupName")%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional" class="d-none">
                        <ContentTemplate>
                            <asp:Button ID="btnLoadCustomerEquipment" runat="server" OnClick="btnLoadCustomerEquipment_Click" />
                            <asp:Button ID="btnOpenModalSearch" runat="server" OnClientClick="AGLoading(true);" OnClick="btnOpenModalSearch_Click" />
                            <asp:Button ID="btnSearch" runat="server" OnClientClick="AGLoading(true);" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnCreate" runat="server" OnClick="btnCreate_Click" />
                            <asp:Button ID="btnCreateWithWarning" runat="server" OnClick="btnCreateWithWarning_Click" />

                            <asp:Button ID="btnOpenModalSelectConfigurationItem" OnClick="btnOpenModalSelectConfigurationItem_Click" OnClientClick="AGLoading(true);" runat="server" />
                            <asp:Button ID="btnOpenModalSelectCustomerCriteria" OnClick="btnOpenModalSelectCustomerCriteria_Click" OnClientClick="AGLoading(true);" runat="server" />
                            <asp:HiddenField ID="hhdModeSearch" runat="server" />

                            <asp:Button ID="btnSelectCriteriaBindPriority" ClientIDMode="Static" OnClick="ddlSelectBindPriority_SelectedIndexChanged" Text="btnChangeSelctPriorityCode" runat="server" />

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>

    <div id="sale-after-attachfile" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <asp:UpdatePanel ID="updAttachFile" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title">
                                <asp:Label ID="lbHeadAttach" runat="server"></asp:Label>
                            </h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <sna:AttachFileUserControl ID="AttachFileUserControl" runat="server" display="true"></sna:AttachFileUserControl>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="d-none">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btnLinkTransactionSearch" runat="server" ClientIDMode="Static"
                    CssClass="d-none" OnClick="btnLinkTransactionSearch_Click" OnClientClick="AGLoading(true);" />
                <asp:HiddenField runat="server" ID="hddCallerID_Criteria" Value="" ClientIDMode="Static" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--modal search Ticket criteria--%>
   <%-- <div class="initiative-model-control-slide-panel" id="modalSearchHelpTicketCriteria">
        <div class="initiative-model-control-body-content z-depth-3">
            <div>
                <div class="initiative-model-control-header">
                    <div class="mat-box-initaive-control">
                        <div class="pull-right">
                            <i class="fa fa-close hand" onclick="closeInitiativeModal('modalSearchHelpTicketCriteria');"></i>
                        </div>
                        <div class="one-line">
                            <label class="text-warning">
                                Search Ticket Criteria
                            </label>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-control-contant">
                    <div class="panel-body-initiative-master">
                        <div class="panel-content-initiative-master">
                            <div class="mat-box-initaive-control tab-initiative-control PANEL-DEFAULT-BUTTON">

                                <div class="form-row">

                                    <div class="col-lg-6">

                                        <div class="card border-primary" style="margin-bottom: 10px;">
                                            <div class="card-body card-body-sm div-search-box">
                                                <div class="form-row">
                                                    <div class="form-group col-lg-12">
                                                        <label>Configuration Item</label>
                                                        <div class="input-group">
                                                            <ag:AutoCompleteEquipment ID="AutoEquipmentSearch" runat="server" CssClass="form-control form-control-sm"
                                                                AfterSelectedChange="loadMappingCustomerForSearchCriteria();">
                                                            </ag:AutoCompleteEquipment>
                                                            <div class="input-group-append c-pointer">
                                                                <span class="input-group-text" onclick="setModalSearch('SEARCH');$('#<%= btnOpenModalSelectConfigurationItem.ClientID%>').click();"><i class="fa fa-search"></i></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-lg-6 col-sm-12">
                                                        <label>Client</label>
                                                        <div class="input-group">
                                                            <ag:AutoCompleteCustomer ID="AutoCustomerSearch" runat="server" CssClass="form-control form-control-sm"
                                                                AfterSelectedChange="loadContactForSearchCriteria();">
                                                            </ag:AutoCompleteCustomer>
                                                            <div class="input-group-append">
                                                                <span class="input-group-text" onclick="setModalSearch('SEARCH');$('#<%= btnOpenModalSelectCustomerCriteria.ClientID%>').click();"><i class="fa fa-search"></i></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-lg-6 col-sm-12">
                                                        <label>Contact</label>
                                                        <ag:AutoCompleteControl runat="server" id="_ddl_contact_person_search"
                                                            CustomViewCode="contact"
                                                            TODO_FunctionJS="loadcontactDetailBySelectedSearch();" CssClass="form-control form-control-sm" />
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnSelectContactBindDetail_Search" CssClass="d-none" OnClick="btnSelectContactBindDetail_Search_Click" runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-lg-12">
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpContactDetailSearch">
                                                            <ContentTemplate>
                                                                <div class="form-row">
                                                                    <div class="form-group col-lg-6 col-sm-12">
                                                                        <label>Contact E-Mail</label>
                                                                        <asp:TextBox ID="txtContactEmail_search" Enabled="false" runat="server"
                                                                            CssClass="form-control form-control-sm"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group col-lg-6 col-sm-12">
                                                                        <label>Contact Phone</label>
                                                                        <asp:TextBox ID="txtContactPhone_search" Enabled="false" runat="server"
                                                                            CssClass="form-control form-control-sm"></asp:TextBox>
                                                                    </div>
                                                                    <div class="form-group col-lg-12">
                                                                        <label>Contact Remark</label>
                                                                        <asp:TextBox ID="txtContactremark_search" Enabled="false" runat="server"
                                                                            CssClass="form-control form-control-sm"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-6">
                                        <asp:UpdatePanel ID="udpDefauleSearch" UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>


                                                <div class="card border-primary" style="margin-bottom: 10px;">
                                                    <div class="card-body card-body-sm div-search-box">
                                                        <div class="form-row">
                                                            <div class="form-group col-sm-6">
                                                                <label>Ticket Type</label>
                                                                <asp:DropDownList ID="_ddl_sctype_search" runat="server" class="form-control form-control-sm disabled" ClientIDMode="Static"></asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-sm-6">
                                                                <label>Ticket No.</label>
                                                                <input id="_txt_docnumber" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static" />
                                                            </div>
                                                            <div class="form-group col-sm-4 d-none">
                                                                <label>Fiscal Year</label>
                                                                <input id="_txt_year_search" type="text" class="form-control form-control-sm" runat="server" clientidmode="Static"
                                                                    onkeypress='return event.charCode >= 48 && event.charCode <= 57' />
                                                            </div>
                                                        </div>
                                                        <div class="form-row">
                                                            <div class="form-group col-sm-4 d-none">
                                                                <label>Ticket Status</label>
                                                                <asp:DropDownList ID="_ddl_status" runat="server" class="form-control form-control-sm" ClientIDMode="Static">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-sm-6">
                                                                <label>Document Status</label>
                                                                <asp:DropDownList ID="_ddl_document_Doc_Status" onchange="changeDocstatusClearTicketStatus(this);" CssClass="form-control form-control-sm" runat="server">
                                                                    <asp:ListItem Value="" Text="" />
                                                                    <asp:ListItem Value="01" Text="Active" />
                                                                    <asp:ListItem Value="00" Text="Inactive" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="form-group col-sm-6">
                                                                <label>
                                                                    <asp:Label Text="Ticket Status" runat="server" ID="_lbl_TicketStatusTran" />
                                                                </label>
                                                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="_ddl_ticket_Doc_Status" ClientIDMode="Static">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="form-row">
                                                            <div class="form-group col-sm-6">
                                                                <label>Ticket Date From</label>
                                                                <div class="input-group">
                                                                    <asp:TextBox ID="ctrlDateFrom" runat="server" CssClass="form-control form-control-sm date-picker"></asp:TextBox>
                                                                    <div class="input-group-append">
                                                                        <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-group col-sm-6">
                                                                <label>To</label>
                                                                <div class="input-group">
                                                                    <asp:TextBox ID="ctrlDateTo" runat="server" CssClass="form-control form-control-sm date-picker"></asp:TextBox>
                                                                    <div class="input-group-append">
                                                                        <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-row">
                                                            <div class="form-group col-lg-12">
                                                                <label>Subject</label>
                                                                <asp:TextBox ID="txtSearchSubject" runat="server"
                                                                    CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>



                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>


                                <div class="card" style="margin-bottom: 10px;">
                                    <div class="card-body card-body-sm">
                                        <asp:UpdatePanel ID="udpDefauleSearch2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="form-row">
                                                    <div class="form-group col-sm-4">
                                                        <label>Impact</label>
                                                        <asp:DropDownList ID="ddlImpactSearch" runat="server" class="form-control form-control-sm"
                                                            OnSelectedIndexChanged="BindDefautlPrioritySearch_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-sm-4">
                                                        <label>Urgency</label>
                                                        <asp:DropDownList ID="ddlUrgencySearch" runat="server" class="form-control form-control-sm"
                                                            OnSelectedIndexChanged="BindDefautlPrioritySearch_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="form-group col-sm-4">
                                                        <label>Priority</label>
                                                        <asp:DropDownList ID="ddlPrioritySearch" runat="server" class="form-control form-control-sm"
                                                            DataTextField="Description" DataValueField="PriorityCode">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="card" style="margin-bottom: 10px;">
                                    <div class="card-body card-body-sm div-search-box">
                                        <div class="form-row">
                                            <div class="form-group col-sm-4">
                                                <label>
                                                    Incident Group
                                                </label>
                                                <ag:AutoCompleteControl runat="server" id="txtProblemGroup" TODO_FunctionJS="loadDataDetailByIncidentAreaForSelect('INCIDENT_GROUP');" CssClass="form-control form-control-sm" />
                                            </div>
                                            <div class="form-group col-sm-4">
                                                <label>
                                                    Incident Type
                                                </label>
                                                <ag:AutoCompleteControl runat="server" id="txtProblemType" TODO_FunctionJS="loadDataDetailByIncidentAreaForSelect('INCIDENT_TYPE');" CssClass="form-control form-control-sm" />
                                            </div>
                                            <div class="form-group col-sm-4">
                                                <label>
                                                    Incident Source
                                                </label>
                                                <ag:AutoCompleteControl runat="server" id="txtProblemSource" TODO_FunctionJS="loadDataDetailByIncidentAreaForSelect('INCIDENT_SOURCE');" CssClass="form-control form-control-sm" />
                                            </div>
                                            <div class="form-group col-sm-4">
                                                <label>Contact Source</label>
                                                <ag:AutoCompleteControl runat="server" id="txtContactSource" CssClass="form-control form-control-sm" />
                                            </div>
                                            <div class="form-group col-md-4">
                                                <asp:UpdatePanel ID="udpDefauleSearch3" UpdateMode="Conditional" runat="server">
                                                    <ContentTemplate>
                                                        <label>Owner Service</label>
                                                        <asp:DropDownList ID="ddlOwnerGroup" runat="server" class="form-control form-control-sm"
                                                            DataTextField="OwnerGroupName" DataValueField="OwnerGroupCode">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <button type="button" class="btn btn-info AUTH_MODIFY DEFAULT-BUTTON-CLICK" onclick="searchClick();"><i class="fa fa-search"></i>&nbsp;&nbsp;Search</button>
                                <button type="reset" class="btn btn-warning" onclick="resetTextForSearch();"><i class="fa fa-refresh"></i>&nbsp;&nbsp;Clear</button>
                                <div id="divSearch" style="display: none;">
                                    <hr />
                                </div>

                                <div class="d-none">
                                    <asp:UpdatePanel ID="udpBuutonEventSearchCriteria" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnBindContactForSearch" OnClick="btnBindContactForSearch_Click" Text="" runat="server" />
                                            <asp:Button ID="btnBindMappingCustomerForSearch" OnClick="btnBindMappingCustomerForSearch_Click" runat="server" />

                                            <asp:HiddenField ID="hhdModeEventFilter" runat="server" />
                                            <asp:Button ID="btnIncidentAreaFilter" OnClick="btnIncidentAreaFilter_Click" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-bottom">
                    <div class="text-right">
                        <a class="water-button" onclick="closeInitiativeModal('modalSearchHelpTicketCriteria');"><i class="fa fa-close"></i>&nbsp;Close</a>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>

    <%--modal ConfigurationItem --%>
    <div class="initiative-model-control-slide-panel" id="modalSearchHelpConfigurationItem">
        <div class="initiative-model-control-body-content z-depth-3">
            <div>
                <div class="initiative-model-control-header">
                    <div class="mat-box-initaive-control">
                        <div class="pull-right">
                            <i class="fa fa-close hand" onclick="closeInitiativeModal('modalSearchHelpConfigurationItem');"></i>
                        </div>
                        <div class="one-line">
                            <label class="text-warning">
                                Select Configuration Item
                            </label>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-control-contant">
                    <div class="panel-body-initiative-master">
                        <div class="panel-content-initiative-master">
                            <div class="mat-box-initaive-control tab-initiative-control PANEL-DEFAULT-BUTTON">
                                <asp:UpdatePanel ID="udpUpdateConfigurationCriteria" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
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
                                                <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtEquipmentCode" />
                                            </div>
                                            <div class="form-group col-md-6 col-sm-12">
                                                <label>
                                                    Configuration Item Name
                                                </label>
                                                <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="txtEquipmentName" />
                                            </div>
                                        </div>
                                        <div class="form-row">
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
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-row">
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

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <div class="form-row">
                                            <div class="form-group col-sm-12 col-md-6">
                                                <button type="button" class="btn btn-info DEFAULT-BUTTON-CLICK" onclick="$(this).next().click();"><i class="fa fa-search"></i>&nbsp;&nbsp;Search</button>
                                                <asp:Button ID="btnSearchDataConfigurationItem" CssClass="d-none" OnClick="btnSearchDataConfigurationItem_Click" OnClientClick="AGLoading(true);" runat="server" />
                                                <button type="reset" class="btn btn-warning" onclick="resetTextForConfigurationItemSearch();"><i class="fa fa-refresh"></i>&nbsp;&nbsp;Clear</button>

                                                <asp:HiddenField ID="hddConfigurationItemCode" runat="server" />
                                                <asp:Button ID="btnSelectConfigurationItem" CssClass="d-none" OnClick="btnSelectConfigurationItem_Click" OnClientClick="AGLoading(true);" runat="server" />
                                            </div>
                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                
                                <div class="form-row">
                                    <div class="form-group col-sm-12 col-md-12">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpSearchConfigurationItem">
                                            <ContentTemplate>
                                                <div class="table-responsive">
                                                    <table id="tableMasterConfigurationItem" class="table table-bordered table-striped table-hover table-sm" style="width: 100%;">
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
                                                    </table>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                    

                <div class="initiative-model-bottom">
                    <div class="text-right">
                        <%--  <a class="water-button AUTH_MODIFY" onclick="activeRequireField();"><i class="fa fa-save"></i>&nbsp;Save</a>--%>
                        <a class="water-button" onclick="closeInitiativeModal('modalSearchHelpConfigurationItem');"><i class="fa fa-close"></i>&nbsp;Close</a>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--modal Customer Search--%>
    <div class="initiative-model-control-slide-panel" id="modalSearchHelpCustomerDetail">
        <div class="initiative-model-control-body-content z-depth-3">
            <div>
                <div class="initiative-model-control-header">
                    <div class="mat-box-initaive-control">
                        <div class="pull-right">
                            <i class="fa fa-close hand" onclick="closeInitiativeModal('modalSearchHelpCustomerDetail');"></i>
                        </div>
                        <div class="one-line">
                            <label class="text-warning">
                                Select Client Detail
                            </label>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-control-contant">
                    <div class="panel-body-initiative-master">
                        <div class="panel-content-initiative-master">
                            <div class="mat-box-initaive-control tab-initiative-control PANEL-DEFAULT-BUTTON">
                                <asp:UpdatePanel ID="udpUpdateCustomerCriteria" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <div class="panel-body-customer-search">
                                            <div class="form-row">
                                                <div class="col-md-12">
                                                    <div class="card border-default" style="margin-bottom: 10px;">
                                                        <div class="card-body card-body-sm">
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
                                                                    <div class="form-row">
                                                                        <div class="form-group col-md-6">
                                                                            <label>Work Phone</label>
                                                                            <asp:TextBox ID="txtPhone" onkeypress="return isNumberKey(event);" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-group col-md-6">
                                                                            <label>Mobile Phone</label>
                                                                            <asp:TextBox ID="txtPhoneMobile" onkeypress="return isNumberKey(event);" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-group col-md-12">
                                                                            <label>E-mail</label>
                                                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-group col-md-12">
                                                                            <label>Owner Service</label>
                                                                            <asp:DropDownList ID="ddlOwnerService_SearchCustomer" CssClass="form-control form-control-sm" runat="server">
                                                                            </asp:DropDownList>
                                                                        </div>
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
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <div class="form-row">
                                            <div class="form-group col-sm-12 col-md-6">
                                                <button type="button" class="btn btn-info DEFAULT-BUTTON-CLICK" onclick="$(this).next().click();"><i class="fa fa-search"></i>&nbsp;&nbsp;Search</button>
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
                                                                <th>Tax ID</th>
                                                                <th>Work Phone</th>
                                                                <th>Moblie Phone</th>
                                                                <th>E-mail</th>
                                                                <th>Active</th>
                                                                <th>Owner Service</th>
                                                                <th>Responsible Organization</th>
                                                            </tr>
                                                        </thead>
                                                    </table>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-bottom">
                    <div class="text-right">
                        <a class="water-button" onclick="closeInitiativeModal('modalSearchHelpCustomerDetail');"><i class="fa fa-close"></i>&nbsp;Close</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        function setModalSearch(value) {
            $(".required").prop('required', false);
            $("#<%= hhdModeSearch.ClientID%>").val(value);
        }

<%--        function changeDocstatusClearTicketStatus(obj)
        {
            if ($(obj).val() != "")
            {
                $('#<%= _ddl_ticket_Doc_Status.ClientID%>').val('');
            }
        }--%>

<%--        function resetTextForSearch() {
            setTimeout(function () {
                $("#<%= ddlImpactSearch.ClientID%>").val('');
                $("#<%= ddlUrgencySearch.ClientID%>").val('');
                $("#<%= ddlPrioritySearch.ClientID%>").val('');
                $("#<%= _ddl_sctype_search.ClientID%>").val('');
                $("#<%= _ddl_status.ClientID%>").val('');
                $("#<%= _ddl_ticket_Doc_Status.ClientID%>").val('');
                $("#<%= ddlOwnerGroup.ClientID%>").val('');
                $(".div-search-box input[type=text], .div-search-box input[type=hidden]").not("#_txt_year_search").val('');
            }, 100);
        }--%>

        function resetTextForConfigurationItemSearch() {
            $("#<%= txtEquipmentCode.ClientID%>").val('');
            $("#<%= txtEquipmentName.ClientID%>").val('');
            $("#<%= ddlEquipmentType.ClientID%>").val('');
            $("#<%= ddlSearch_EMClass.ClientID%>").val('');
            $("#<%= ddlSearch_Category.ClientID%>").val('');
            $("#<%= ddlEquipmentStatus.ClientID%>").val('');
        }

        function resetTextForCustomerCriteriaSearch() {
            $(".panel-body-customer-search input[type=text], .panel-body-customer-search select").val("");
           <%-- $("#<%= txtFirstname.ClientID %>").val('');
            $("#<%= ddlCustomerGroup.ClientID %>").val('');
            $("#<%= ddlSaleDistrict.ClientID %>").val('');
            $("#<%= txtAddress.ClientID %>").val('');
            $("#<%= txtPhone.ClientID %>").val('');
            $("#<%= txtEmail.ClientID %>").val('');--%>
        }


<%--        function loadMappingCustomerForSearchCriteria() {
            inactiveRequireField();
            $("#<%= btnBindMappingCustomerForSearch.ClientID%>").click();
        }--%>

<%--        function loadContactForSearchCriteria() {
            inactiveRequireField();
            $("#<%= btnBindContactForSearch.ClientID%>").click();
        }--%>

<%--        function loadcontactDetailBySelectedSearch() {
            inactiveRequireField();
            $("#<%= btnSelectContactBindDetail_Search.ClientID%>").click();
        }--%>

<%--        function loadDataDetailByIncidentAreaForSelect(value) {
            inactiveRequireField();
            $("#<%= hhdModeEventFilter.ClientID%>").val(value);
            $("#<%= btnIncidentAreaFilter.ClientID%>").click();
        }--%>




        function afterSearchBindEquipmentCriteria(dataArr, StatusList) {
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
                    Equipment.OwnerGroupName
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

            $("#tableMasterConfigurationItem").dataTable({
                data: data,
                deferRender: true,
                "order": [[3, "asc"]],
                'columnDefs': [
                   {
                       "orderable": false,
                       'targets': 0,
                       'createdCell': function (td, cellData, rowData, row, col) {
                           $(td).addClass("text-center text-nowrap");
                           $(td).data("key", rowData[0]);
                           $(td).html(
                               '<a href="JavaScript:;">' +
									'<i class="fa fa-cubes" aria-hidden="true"></i>' +
								'</a>'
                            );
                           $(td).bind("click", function () {
                               $("#<%= hddConfigurationItemCode.ClientID%>").val($(this).data("key"));
                               $("#<%= btnSelectConfigurationItem.ClientID%>").click();
                           });

                           $(td).closest("tr").addClass("c-pointer");
                           $(td).closest("tr").data("key", rowData[0]);
                           $(td).closest("tr").bind("click", function () {
                               $("#<%= hddConfigurationItemCode.ClientID%>").val($(this).data("key"));
                               $("#<%= btnSelectConfigurationItem.ClientID%>").click();
                           });
                       }
                   }
                ]
            });
            
           }

           function afterSearchBindCustomerCriteria(dataArr) {
               var data = [];
               for (var i = 0 ; i < dataArr.length ; i++) {
                   var Customer = dataArr[i];
                   data.push([
                       Customer.CustomerCode,
                       Customer.CustomerCode + " : " + Customer.FullName,
                       Customer.CustomerGroup + " : " + Customer.CustomerGroupDesc,
                       Customer.Address,
                       Customer.TaxID,
                       Customer.TelNo1,
                       Customer.Mobile,
                       Customer.EMail,
                       Customer.Active,
                       Customer.OwnerService,
                       Customer.ResponsibleOrganization
                   ]);
               }

               $("#tableMasterCustomerCriteria").dataTable({
                   data: data,
                   deferRender: true,
                   columnDefs: [{
                       "orderable": false,
                       "targets": [0],
                       "createdCell": function (td, cellData, rowData, row, col) {
                           $(td).addClass("text-center");
                           $(td).data("key", rowData[0]);
                           $(td).html(
                                 '<a href="JavaScript:;">' +
                                       '<i class="fa fa-user fa-lg text-dark c-pointer" aria-hidden="true"></i>' +
                                   '</a>'
                               );
                           $(td).bind("click", function () {
                               $("#<%= hddCustomerCodeSelected.ClientID%>").val($(td).data("key"));
                               $("#<%= btnSelectCustomerCriteria.ClientID%>").click();
                           });

                           $(td).closest("tr").addClass("c-pointer");
                           $(td).closest("tr").data("key", rowData[0]);
                           $(td).closest("tr").bind("click", function () {
                               $("#<%= hddCustomerCodeSelected.ClientID%>").val($(this).data("key"));
                               $("#<%= btnSelectCustomerCriteria.ClientID%>").click();
                           });
                       }
                   }]
               });
           }



    </script>
    <script type="text/javascript">
        function resetText() {
            setTimeout(function () {
                $("#ddlImpact").val('');
                $("#ddlUrgency").val('');
                $("#ddlPriority").val('');
                $(".clear-text").val('');
            }, 100);
        }

        function openTicket(CallerID) {
            $("#hddCallerID_Criteria").val(CallerID);
            $("#btnLinkTransactionSearch").click();
        }

        function afterSearch() {
            $("#divSearch").show();
            $("#tableItems").dataTable({
                columnDefs: [
                    {
                        "orderable": false,
                        "targets": [0]
                    },
                    {
                        "orderable": true,
                        'targets': 1,
                        'createdCell': function (td, cellData, rowData, row, col) {
                            var dataDB = cellData.substring(0, 8);
                            var dataDisplay = dataDB.substring(6, 8) + "/" + dataDB.substring(4, 6) + "/" + dataDB.substring(0, 4);

                            $(td).html(dataDisplay);
                        }
                    }
                ],
                "order": [[2, "asc"], [3, "desc"]]
            });

            //$('html,body').animate({
            //    scrollTop: $("#divSearch").offset().top - 50
            //});
            //$("#modalSearchHelpTicketCriteria .initiative-model-control-contant").scrollTop($("#divSearch").position().top);
           
        }

        function activeRequireField() {
            $(".required").prop('required', true);
        }

        function inactiveRequireField() {
            $(".required").prop('required', false);
        }

        function searchClick() {
            inactiveRequireField();
            $("#<%= btnSearch.ClientID%>").click();
        }

        function createClick() {
            activeRequireField();
            $("#<%= btnCreate.ClientID%>").click();
        }
        var _loadCustomerByEquipment = 0;
        function loadCustomerByEquipment() {
            inactiveRequireField();

            _loadCustomerByEquipment++;
            console.log("loadCustomerByEquipment : " + _loadCustomerByEquipment);

            $("#<%= btnLoadCustomerEquipment.ClientID%>").click();
        }

        var _loadEquipmentAndContact = 0;
        function loadEquipmentAndContact() {
            inactiveRequireField();

            _loadEquipmentAndContact++;
            console.log("loadEquipmentAndContact : " + _loadEquipmentAndContact);

            $("#<%= btnBindContactCus.ClientID%>").click();
        }

        function loadEquipmentAndContactAuto() {
            inactiveRequireField();

            _loadEquipmentAndContact++;
            console.log("loadEquipmentAndContact : " + _loadEquipmentAndContact);

            $("#<%= btnBindContactCusAuto.ClientID%>").click();
        }

        function loadcontactDetailBySelected() {
            $("#<%= btnSelectContactBindDetail.ClientID%>").click();
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
                $("#<%= btnCreateWithWarning.ClientID%>").click();
            }
        }

        

    </script>

</asp:Content>
