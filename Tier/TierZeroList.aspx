<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" ValidateRequest="false"
    AutoEventWireup="true" CodeBehind="TierZeroList.aspx.cs" Inherits="ServiceWeb.Tier.TierZeroList" %>

<%@ Register Src="~/Tier/UserControl/PopupCreateTierZeroControl.ascx" TagPrefix="uc1" TagName="PopupCreateTierZeroControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-item").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    <div class="d-none">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpData">
            <ContentTemplate>
                <asp:Button Text="" runat="server" ID="btnRebindTierZeroList" ClientIDMode="Static"
                    OnClick="btnRebindList_Click" OnClientClick="AGLoading(true);" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpJsonTierList">
        <contenttemplate>
                <div style="display: none;" runat="server" id="divJsonTier0List" ClientIDMode="Static">[]</div>
            </contenttemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="col-12">
            <div class="card mb-3">
                <div class="card-header">
                    <b>Tier 0 : Un-Assign From Channel</b>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updFastService">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-10"></div>
                                <div class="col-lg-2" style="text-align: right; padding-bottom: 12px; right: 0;">
                                    <div>
                                        <label>Select Status&nbsp;&nbsp;&nbsp;</label></div>
                                    <asp:DropDownList runat="server" ID="ddlListType" AutoPostBack="true" OnSelectedIndexChanged="ddlListType_Change" CssClass="form-control form-control-sm">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="table-responsive" style="width: 100%; overflow: auto;">
                                <table id="tableItems" class="table table-sm table-hover table-striped" style="margin-bottom: 0;">
                                    <thead>
                                        <tr>
                                            <th class="text-nowrap">DateTime</th>
                                            <th class="text-nowrap">Channel</th>
                                            <th class="text-nowrap"></th>
                                            <th class="text-nowrap"></th>
                                            <th class="text-nowrap"></th>
                                            <th class="text-nowrap">E-mail</th>
                                            <th class="text-nowrap">Client Code</th>
                                            <th class="text-nowrap">Client Name</th>
                                            <th class="text-nowrap">Tel No.</th>
                                            <th class="text-nowrap">Subject</th>
                                            <%--<th class="text-nowrap">Detail</th>--%>
                                            <th class="text-nowrap">Status</th>
                                            <th class="text-nowrap">Ticket Number</th>
                                            <th class="text-nowrap">Ticket Type</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater runat="server" ID="rptListDataUpload">
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="text-nowrap"><%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(DataBinder.Eval(Container.DataItem,"CREATED_ON").ToString()) %></td>
                                                    <td class="text-nowrap">
                                                        <%# FormatChannel(DataBinder.Eval(Container.DataItem,"Channel").ToString()) %>
                                                        <asp:HiddenField runat="server" ID="hdfSEQ" Value='<%# Eval("SEQ") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfChannel" Value='<%# Eval("Channel") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfEMail" Value='<%# Eval("EMail") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfCustomerCode" Value='<%# Eval("CustomerCode") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfCustomerName" Value='<%# Eval("CustomerName") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfTelNo" Value='<%# Eval("TelNo") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfSubject" Value='<%# Eval("Subject") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfDetail" Value='<%# Eval("Detail") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("Status") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfTicketNumber" Value='<%# Eval("TicketNumber") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfTicketType" Value='<%# Eval("TicketType") %>' />
                                                    </td>
                                                    <td><i class="fa fa-search" onclick="searchDesc('<%# Eval("SEQ") %>');" data-toggle="modal" data-target="#tier0desc"></i></td>
                                                    <td class="text-nowrap">
                                                        <%--Original btn--%>
                                                        <%--<button type="button" class="btn btn-primary btn-sm status <%# !IsCanCreatedTicket(Convert.ToString(Eval("Status"))) ? "d-none" : "" %>" 
                                                      onclick="createTierZeroClose('<%# Eval("SEQ") %>','<%# Eval("Status") %>');">Close Case</button>--%>
                                                        <i class="fa fa-times-circle-o fa-lg text-danger mr-1 AUTH_MODIFY status <%# !IsCanCreatedTicket(Convert.ToString(Eval("Status"))) ? "d-none" : "" %>" onclick="createTierZeroClose('<%# Eval("SEQ") %>','<%# Eval("Status") %>');"></i>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%--Original btn--%>
                                                        <%--<button type="button" class="btn btn-primary btn-sm status <%# !IsCanCreatedTicket(Convert.ToString(Eval("Status"))) ? "d-none" : "" %>" 
                                                            onclick="createTierZeroTicket('<%# Eval("SEQ") %>');">Create Ticket</button>--%>
                                                        <i class="fa fa-edit fa-lg text-dark mx-1 AUTH_MODIFY status <%# !IsCanCreatedTicket(Convert.ToString(Eval("Status"))) ? "d-none" : "" %>" onclick="createTierZeroTicket('<%# Eval("SEQ") %>');"></i>
                                                    </td>
                                                    <td class="text-nowrap"><%# Eval("EMail") %></td>
                                                    <td class="text-truncate customer"><span><%# Eval("CustomerCode") %></span></td>
                                                    <td class="text-truncate customer"><span><%# Eval("CustomerName") %></span></td>
                                                    <td class="text-nowrap"><%# Eval("TelNo") %></td>
                                                    <td class="text-nowrap ticket-type"><%# Eval("Subject") %></td>
                                                    <%--<td class="text-subject text-truncate font-italic"><%# Eval("Detail") %></td>--%>
                                                    <td class="text-nowrap"><%# FormatStatus(DataBinder.Eval(Container.DataItem,"Status").ToString()) %></td>
                                                    <td class="text-nowrap"><%# Eval("TicketNumber") %></td>
                                                    <td class="text-nowrap"><%# Eval("TicketType") %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="tier0desc" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Tier0 Detail</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-row">
                        <div class="form-group col-sm-3 font-weight-bold">SEQ</div>
                        <div class="form-group col-sm-9">
                            <label id="SEQ_txt"></label>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-sm-3 font-weight-bold">Channel</div>
                        <div class="form-group col-sm-9">
                            <label id="Channel_txt"></label>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-sm-3 font-weight-bold">EMail</div>
                        <div class="form-group col-sm-9">
                            <label id="EMail_txt"></label>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-sm-3 font-weight-bold">Status</div>
                        <div class="form-group col-sm-9">
                            <label id="Status_txt"></label>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-sm-3 font-weight-bold">Customer Code</div>
                        <div class="form-group col-sm-9">
                            <label id="CustomerCode_txt"></label>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-sm-3 font-weight-bold">Customer Name</div>
                        <div class="form-group col-sm-9">
                            <label id="CustomerName_txt"></label>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-sm-3 font-weight-bold">TelNO.</div>
                        <div class="form-group col-sm-9">
                            <label id="TelNO_txt"></label>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-sm-3 font-weight-bold">Equipment</div>
                        <div class="form-group col-sm-9">
                            <label id="Equipment_txt"></label>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-sm-3 font-weight-bold">Ticket Type</div>
                        <div class="form-group col-sm-9">
                            <label id="TicketType_txt"></label>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-sm-3 font-weight-bold">Ticket NO.</div>
                        <div class="form-group col-sm-9">
                            <label id="TicketNO_txt"></label>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-sm-3 font-weight-bold">Subject</div>
                        <div class="form-group col-sm-9">
                            <label id="Subject_txt"></label>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-sm-3 font-weight-bold">Detail</div>
                        <div class="form-group col-sm-9">
                            <label id="Detail_txt"></label>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-sm-3 font-weight-bold">Created By</div>
                        <div class="form-group col-sm-9">
                            <label id="CreatedBy_txt"></label>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-sm-3 font-weight-bold">Created On</div>
                        <div class="form-group col-sm-9">
                            <label id="CreatedOn_txt"></label>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-sm-12">
                            <div class="table-responsive">
                                <table class="table table-bordered table-striped">
                                    <thead>
                                        <tr>
                                            <th style="width: 75px;">Item No</th>
                                            <th style="width: 120px;">Data Time</th>
                                            <th style="width: 100px;">Type</th>
                                            <th>Remark</th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbodyLogOtherRemark">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
            </div>
        </div>
    </div>
    <uc1:PopupCreateTierZeroControl runat="server" id="PopupCreateTierZeroControl" />
    <script>
        //$(document).ready(function () {
        //$('#tableItems').DataTable();
        //});
    </script>
    <script>
        var Tier0List = JSON.parse($("#divJsonTier0List").html());
        function test() {
            console.log("hello world");
        };
        function setTier0List() {
            var Tier0List = JSON.parse($("#divJsonTier0List").html());
            var data = [];
            var cfs = "";
            console.log(Tier0List);
            for (var i = 0 ; i < Tier0List.length ; i++) {
                var Tier0 = Tier0List[i];
                if (Tier0.ControlDisplay == "True") {
                    cfs = "d-none"
                } else {
                    cfs = "";
                }
                data.push([
                    Tier0.CREATED_ON,
                    Tier0.Channel,
                    "<i class='fa fa-search' onclick=\"searchDesc('" + Tier0.SEQ + "');\" data-toggle='modal' data-target='#tier0desc'></i>",
                    "<i class='fa fa-times-circle-o fa-lg text-danger mr-1 AUTH_MODIFY status " + cfs + "' onclick=\"createTierZeroClose('" + Tier0.SEQ + "', '" + Tier0.Status + "'); \" ></i > ",
                    "<i class='fa fa-edit fa-lg text-dark mx-1 AUTH_MODIFY status " + cfs + "' onclick=\"createTierZeroTicket('" + Tier0.SEQ + "');\"></i>",
                    Tier0.EMail,
                    Tier0.CustomerCode,
                    Tier0.CustomerName,
                    Tier0.TelNo,
                    Tier0.Subject,
                    Tier0.ControlDisplayStatus,
                    Tier0.TicketNumber,
                    Tier0.TicketType
                ]);
            }
            //$("#search-panel").show();
            $("#tableItems").dataTable({
                data: data,
                deferRender: true,
                "order": [[1, "asc"]],
                columnDefs: [{
                    "targets": ["_all"],
                    "createdCell": function (td, cellData, rowData, row, col) {
                        $(td).addClass("text-nowrap");
                    }
                }]
            });
        };
    </script>
    <script>
        var result = "";
        var SEQ = "";
        function searchDesc(SEQ) {
            this.SEQ = SEQ;
            var data = {
                SEQ: this.SEQ
            };

            $.ajax({
                type: "GET",
                url: "<%= Page.ResolveUrl("~/API/Tier0API.aspx") %>",
                data: data,
                success: function (res) {
                    result = JSON.parse(res);
                    document.getElementById("SEQ_txt").innerHTML = result[0]["SEQ"];
                    document.getElementById("Channel_txt").innerHTML = result[0]["Channel"];
                    document.getElementById("EMail_txt").innerHTML = result[0]["EMail"];
                    document.getElementById("Status_txt").innerHTML = result[0]["Status"];
                    document.getElementById("CustomerCode_txt").innerHTML = result[0]["CustomerCode"];
                    document.getElementById("CustomerName_txt").innerHTML = result[0]["CustomerName"];
                    document.getElementById("TelNO_txt").innerHTML = result[0]["TelNo"];
                    document.getElementById("Equipment_txt").innerHTML = result[0]["EquipmentNo"];
                    document.getElementById("TicketType_txt").innerHTML = result[0]["TicketType"];
                    document.getElementById("TicketNO_txt").innerHTML = result[0]["TicketNumber"];
                    document.getElementById("Subject_txt").innerHTML = result[0]["Subject"];
                    document.getElementById("Detail_txt").innerHTML = result[0]["Detail"];
                    document.getElementById("CreatedBy_txt").innerHTML = result[0]["CREATED_BY"];
                    document.getElementById("CreatedOn_txt").innerHTML = result[0]["CREATED_ON"];
                    //console.log(res);

                    var tableTarget = $("#tbodyLogOtherRemark");
                    tableTarget.html('');
                    if (result[0].datasOtherRemark.length > 0) {
                        for (var i = 0; i < result[0].datasOtherRemark.length; i++) {
                            var data = result[0].datasOtherRemark[i];

                            var tr = $("<tr>", {
                            });

                            var tdItemNo = $("<td>", {
                                html: data.ItemNo,
                                class: "text-nowrap"
                            });
                            var tdDataTimeDisplay = $("<td>", {
                                html: data.DataTimeDisplay,
                                class: "text-nowrap"
                            });
                            var tdType = $("<td>", {
                                html: data.RemarkType,
                                class: "text-nowrap"
                            });
                            var tdRemark = $("<td>", {
                                html: data.Remark.split('\n').join('<br />'),
                                class: "text-nowrap"
                            });

                            tr.append(tdItemNo, tdDataTimeDisplay, tdType, tdRemark);
                            tableTarget.append(tr);
                        }
                    } else {
                        var tr = $("<tr>", {
                        });

                        var tdNoData = $("<td>", {
                            html: "No date.",
                            class: "text-nowrap",
                            colspan: "4"
                        });
                        tr.append(tdNoData);
                        tableTarget.append(tr);
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            });
        };
    </script>
</asp:Content>
