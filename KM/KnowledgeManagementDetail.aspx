<%@ Page Title="" Language="C#" ValidateRequest="false"  MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="KnowledgeManagementDetail.aspx.cs" Inherits="ServiceWeb.KM.KnowledgeManagementDetail" %>

<%@ Register Src="~/widget/usercontrol/TimeLineControl.ascx" TagPrefix="sna" TagName="TimeLineControl" %>
<%@ Register Src="~/UserControl/ChangeLogControl.ascx" TagPrefix="sna" TagName="ChangeLogControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-knowledge").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <script>
        function confirmUpdate(obj) {
            if (AGConfirm(obj, "Confirm Update")) {
                AGLoading(true);
                return true;
            }
            return false;
        }

        function checkRequiredSave() {
            $(".required").prop('required', true);
            $("#btnCheckRequired").click();
        }

        function alinkServiceCallRefKM_click() {
            window.open("/crm/AfterSale/ServiceCallTransaction.aspx", '_blank');
        }
    </script>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <a class="btn btn-warning btn-sm mb-1" href="KnowledgeManagement.aspx"><i class="fa fa-arrow-circle-left"></i>&nbsp;&nbsp;Back</a>
                    <button type="button" runat="server" class="btn btn-primary btn-sm mb-1 AUTH_MODIFY" onclick="checkRequiredSave();">
                        <i class="fa fa-save"></i>&nbsp;&nbsp;Save
                    </button>
                    <asp:Button Text="บันทึก" runat="server" CssClass="d-none" ID="btnSaveKM"
                        OnClick="btnSaveKM_Click" ClientIDMode="Static" OnClientClick="return confirmUpdate(this);" />
                    <asp:Button ID="btnCheckRequired" ClientIDMode="Static" OnClick="btnCheckRequired_Click" Text="" CssClass="d-none" runat="server" />

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>
    <div class="card shadow">
        <div class="card-header">
            <h5 class="mb-0">Knowledge Management Detail</h5>
        </div>
        <div class="card-body">
            <div class="form-row">
                <div class="col-md-12">

                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCreateDetail">
                        <ContentTemplate>
                            <nav>
                                <div class="nav nav-tabs" id="nav-tab" role="tablist">
                                    <a class="nav-item nav-link active" id="nav-header-tab" data-toggle="tab" href="#nav-information" role="tab" aria-controls="nav-header" aria-selected="true">Information</a>
                                    <a class="nav-item nav-link" id="nav-item-tab" data-toggle="tab" href="#nav-attachfile" role="tab" aria-controls="nav-item" aria-selected="false">Attach File</a>
                                    <a class="nav-item nav-link" id="nav-ticket-tab" data-toggle="tab" href="#nav-ticket" role="tab" aria-controls="nav-ticket" aria-selected="false">Used Ticket</a>
                                    <a class="nav-item nav-link" id="nav-changelog-tab" data-toggle="tab" href="#nav-changelog" role="tab" aria-controls="nav-changelog" aria-selected="false">Change Log</a>

                                </div>
                            </nav>
                            <div class="tab-content p-3" id="nav-tabContent">
                                <div class="tab-pane fade show active" id="nav-information" role="tabpanel" aria-labelledby="nav-information-tab">
                                    <div class="form-row">
                                        <div class="form-group col-md-6">
                                            <label>Knowledge ID</label>
                                            <asp:TextBox ID="txtObjectID" runat="server" Enabled="false" CssClass="form-control form-control-sm" />
                                            <asp:HiddenField ID="hhdKeyAobjectlink" runat="server" />
                                            <asp:HiddenField ID="hhdObjectItem" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-row">

                                        <div class="form-group col-md-6">
                                            <label>Knowledge Group</label>
                                            <asp:DropDownList runat="server" Enabled="false" ID="ddlGroup" CssClass="form-control form-control-sm">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-6">
                                            <label>Keyword</label>
                                            <asp:TextBox ID="txtKeyword" placeholder="Text" CssClass="form-control form-control-sm" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-6">
                                            <label>Subject</label>
                                            <asp:TextBox ID="txtSubject" placeholder="Text" TextMode="MultiLine" Rows="3" CssClass="form-control form-control-sm required" runat="server" />
                                        </div>
                                        <div class="form-group col-md-6">
                                            <label>Detail</label>
                                            <asp:TextBox ID="txtDetail" placeholder="Text" TextMode="MultiLine" Rows="3" CssClass="form-control form-control-sm" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-6">
                                            <label>Symtom</label>
                                            <asp:TextBox ID="txtSymtom" placeholder="Text" TextMode="MultiLine" Rows="3" CssClass="form-control form-control-sm" runat="server" />
                                        </div>
                                        <div class="form-group col-md-6">
                                            <label>Cause</label>
                                            <asp:TextBox ID="txtCause" placeholder="Text" TextMode="MultiLine" Rows="3" CssClass="form-control form-control-sm" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-12">
                                            <label>Solution</label>
                                            <asp:TextBox ID="txtSolution" placeholder="Text" TextMode="MultiLine" Rows="3" CssClass="form-control form-control-sm" runat="server" />
                                        </div>
                                    </div>

                                </div>
                                <div class="tab-pane fade" id="nav-attachfile" role="tabpanel" aria-labelledby="nav-attachfile-tab">
                                    <div>
                                        <sna:timelinecontrol runat="server" id="TimeLineControl" />
                                    </div>
                                </div>
                                <div class="tab-pane fade" id="nav-changelog" role="tabpanel" aria-labelledby="nav-changelog-tab">
                                    <div>
                                        <sna:ChangeLogControl id="KMManageChangeLog" runat="server" />
                                    </div>
                                </div>
                                <div class="tab-pane fade" id="nav-ticket" role="tabpanel" aria-labelledby="nav-ticket-tab">
                                    <div>
                                        <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="table-responsive">
                                                    <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                                        <thead>
                                                            <tr>
                                                                <th></th>
                                                                <th class="text-nowrap" style="width:97%;">Ticket Type (Qty)</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td class="text-center" onclick="swishAllTicketRefKM(this);">
                                                                    <i class="fa fa-minus-circle c-pointer icon-data-all"></i>
                                                                </td>
                                                                <td class="text-nowrap">
                                                                    Ticket All ( <asp:Label ID="lblTicketRefKMAll" Text="0" runat="server" /> )
                                                                </td>
                                                            </tr>
                                                        <asp:Repeater ID="rptSearchSaleGroup" OnItemDataBound="rptSearchSaleGroup_ItemDataBound" runat="server">
                                                            <ItemTemplate>
                                                                <tr class="tr-group-ticket tr-group-ticket-header">
                                                                    <td></td>
                                                                    <td class="text-nowrap c-pointer" onclick="swishAllTicketRefKM_Item(this);">
                                                                       &nbsp; <i class="fa fa-plus-circle c-pointer icon-data-all-item"></i>
                                                                       &nbsp;&nbsp;&nbsp;<label class="text-primary"><%# Eval("DocumentTypeDesc") %> &nbsp;&nbsp;( <%# Convert.ToInt64(Eval("Qty")).ToString("#,###") %> )</label> 
                                                                    </td>
                                                                </tr>
                                                                <tr class="d-none tr-group-ticket">
                                                                    <td></td>
                                                                    <td style="padding:20px;">
                                                                        <div class="table-responsive">
                                                                        <table class="tableItems-table table table-bordered table-striped table-hover table-sm">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th class="text-nowrap"></th>
                                                                                    <th class="text-nowrap">Ticket Date</th>
                                                                                    <th class="text-nowrap">Ticket Type</th>
                                                                                    <th class="text-nowrap">Ticket No.</th>
                                                                                    <th class="text-nowrap">Ticket Status</th>
                                                                                    <th class="text-nowrap">Subject</th>
                                                                                    <th class="text-nowrap">Client</th>
                                                                                    <th class="text-nowrap">Contact</th>
                                                                                    <th class="text-nowrap">Configuration Item</th>
                                                                                    <th class="text-nowrap">Impact</th>
                                                                                    <th class="text-nowrap">Urgency</th>
                                                                                    <th class="text-nowrap">Priority</th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                                <asp:Repeater ID="rptSearchSale" runat="server">
                                                                                    <ItemTemplate>
                                                                                        <tr class="<%# GetRowColorAssign(Eval("CallStatus").ToString()) %> c-pointer" onclick="redirectPageToTicketDetailClick(this);"
                                                                                             data-docno="<%# Eval("CallerID") %>" data-year="<%# Eval("Fiscalyear") %>"
                                                                                                    data-type="<%# Eval("Doctype") %>"  data-cus="<%# Eval("CustomerCode") %>">
                                                                                            <td class="text-nowrap text-center align-middle">
                                                                                                <i class="fa fa-search text-dark c-pointer link-ticket-detail" title="View Ticket" ></i>
                                                                                            </td>
                                                                                            <td class="text-nowrap">
                                                                                                <%# Eval("DOCDATE").ToString() %>
                                                                                            </td>
                                                                                            <td class="text-nowrap">
                                                                                                <%# Eval("DocumentTypeDesc") %>
                                                                                            </td>
                                                                                            <td class="text-nowrap">
                                                                                                <%--CallerID--%>
                                                                                                <%# Eval("CallerIDDisplay") %>
                                                                                            </td>
                                                                                            <td class="text-nowrap">
                                                                                                <%# Eval("DocStatusDesc") %>
                                                                                            </td>
                                                                                            <td class="text-truncate" style="max-width: 500px;">
                                                                                                <span title="<%# Eval("HeaderText") %>"><%# Eval("HeaderText") %></span>
                                                                                            </td>
                                                                                            <td class="text-nowrap">
                                                                                                <%# Eval("CustomerName") %>
                                                                                            </td>
                                                                                            <td class="text-nowrap">
                                                                                                <%# Eval("ContractPersonName") %>
                                                                                            </td>
                                                                                            <td class="text-nowrap">
                                                                                                <%# Eval("EquipmentName") %>
                                                                                            </td>
                                                                                            <td class="text-nowrap">
                                                                                                <%# Eval("ImpactName") %>
                                                                                            </td>
                                                                                            <td class="text-nowrap">
                                                                                                <%# Eval("UrgencyName") %>
                                                                                            </td>
                                                                                            <td class="text-nowrap">
                                                                                                <%# Eval("PriorityName") %>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                </asp:Repeater>
                                                                            </tbody>
                                                                        </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:Updatepanel ID="udpHideValue" UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <asp:HiddenField ID="hddDoctype" ClientIDMode="Static" runat="server" />
                                                <asp:HiddenField ID="hddFiscalyear" ClientIDMode="Static" runat="server" />
                                                <asp:HiddenField ID="hddDocNumner" ClientIDMode="Static" runat="server" />
                                                <asp:HiddenField ID="hddCustomer" ClientIDMode="Static" runat="server" />
                                                <asp:Button ID="btnLinkTransactionRefKM" runat="server" ClientIDMode="Static" CssClass="d-none"
                                                      OnClick="btnLinkTransactionRedirect_Click" OnClientClick="AGLoading(true);"  />
                                            </ContentTemplate>
                                        </asp:Updatepanel>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <style>
        #tableItems .fa-minus-circle {
            color: rgba(0, 0, 0, 0.44);
        }
    </style>
    <script type="text/javascript">
        function afterSearch() {
            $(".tableItems-table").dataTable({
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
                "order": [[1, "desc"], [2, "asc"], [3, "desc"]]
            });
        }

        function swishAllTicketRefKM(obj)
        {
            var icondata = $(obj).find(".icon-data-all");
            var hasOpen = icondata.hasClass("fa-minus-circle");
            if (hasOpen)//สั่งปิด
            {
                icondata.removeClass("fa-minus-circle");
                icondata.addClass("fa-plus-circle");
                $("#tableItems .tr-group-ticket").addClass("d-none");
            }
            else //สั่งเปิด
            {
                icondata.removeClass("fa-plus-circle");
                icondata.addClass("fa-minus-circle");
                $("#tableItems .tr-group-ticket-header").removeClass("d-none");
                $("#tableItems .icon-data-all-item").removeClass("fa-minus-circle").addClass("fa-plus-circle");
            }
            
        }

        function swishAllTicketRefKM_Item(obj)
        {
            
            var icondata = $(obj).find(".icon-data-all-item");
            var hasOpen = icondata.hasClass("fa-minus-circle");
            if (hasOpen)
            {
                icondata.removeClass("fa-minus-circle");
                icondata.addClass("fa-plus-circle");
            } else
            {
                icondata.removeClass("fa-plus-circle");
                icondata.addClass("fa-minus-circle");
            }
            $(obj).parent().next().toggleClass('d-none');
        }

        function redirectPageToTicketDetailClick(obj)
        {
            $(".required").prop('required', false);
            $("#hddDocNumner").val($(obj).data("docno"));
            $("#hddFiscalyear").val($(obj).data("year"));
            $("#hddDoctype").val($(obj).data("type"));
            $("#hddCustomer").val($(obj).data("cus"));
            $("#btnLinkTransactionRefKM").click();
        }

    </script>
</asp:Content>
