<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="TicketReport.aspx.cs" Inherits="ServiceWeb.Report.TicketReport" %>

<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteCustomer.ascx" TagPrefix="uc1" TagName="AutoCompleteCustomer" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEquipment.ascx" TagPrefix="uc1" TagName="AutoCompleteEquipment" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEmployee.ascx" TagPrefix="uc1" TagName="AutoCompleteEmployee" %>
<%@ Register Src="~/widget/usercontrol/AutoCompleteControl.ascx" TagPrefix="ag" TagName="AutoCompleteControl" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-ticket-report").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>

    <style>
        .summary {
    display: block;
    text-overflow: ellipsis;
    font-weight: 400;
    height: 2.2em;
    overflow: hidden;
    white-space: normal;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
    display: -webkit-box!important;
}
    </style>
    <!-- start -->
    <div class="PANEL-DEFAULT-BUTTON">
        <!-- start card -->
        <div class="card mb-4 shadow-sm">
            <!-- start header -->
            <div class="card-header">
                <h4 class="my-0 font-weight-normal">Ticket Report</h4>
            </div>
            <!-- end header -->
            <!-- start card body -->
            <div class="card-body">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="update_search">
                    <ContentTemplate>
                        <!-- start card content -->
                        <div class="form-row">
                            <!-- start column 1 -->
                            <div class="col-lg-6">
                                <div class="card border-primary">
                                    <div class="card-body card-body-sm">
                                        <!-- start sub-row 1 -->
                                        <div class="form-row">
                                            <div class="col-lg-6">
                                                <label>Configuration Item</label>
                                                <uc1:AutoCompleteEquipment runat="server" CssClass="form-control form-control-sm" id="AutoCompleteEquipment" />
                                            </div>
                                            <div class="col-lg-6">
                                                <label>Ticket Type</label>
                                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ui_ticket_type"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <!-- end sub-row 1 -->
                                        <!-- start sub-row 2 -->
                                        <div class="form-row">
                                            <div class="col-lg-6">
                                                <label>Document Status</label>
                                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ui_document_status">
                                                    <asp:ListItem Value="" Text="" />
                                                    <asp:ListItem Value="01" Text="Active" />
                                                    <asp:ListItem Value="00" Text="Inactive" />
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-6">
                                                <label>Ticket Status</label>
                                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ui_ticket_status"></asp:DropDownList>
                                            </div>
                                        </div>
                                        
                                        <div class="form-row">
                                            <div class="col-lg-12">
                                                <label>Description</label>
                                                <asp:TextBox runat="server" id="txtDescription" CssClass="form-control form-control-sm" />
                                            </div>
                                        </div>

                                        <!-- end sub-row 2 -->
                                    </div>
                                </div>
                            </div>
                            <!-- end column 1 -->
                            <!-- start column 2 -->
                            <div class="col-lg-6">
                                <div class="card border-primary">
                                    <div class="card-body card-body-sm">
                                        <label>Client</label>
                                        <uc1:AutoCompleteCustomer runat="server" id="AutoCompleteCustomer" CssClass="form-control form-control-sm" />
                                        
                                        <div class="form-row">
                                            <div class="col-6">
                                                <label>Contact Email</label>
                                                <asp:TextBox runat="server" id="txtContactEmail" CssClass="form-control form-control-sm" />
                                            </div>
                                            <div class="col-6">
                                                <label>MNSP</label>
                                                <asp:TextBox runat="server" id="txtResponsibleOrganization" CssClass="form-control form-control-sm" />
                                            </div>
                                        </div>
                                        <!-- start sub-row 3 -->
                                        <div class="form-row">
                                            <div class="col-lg-6">
                                                <label>Ticket Date From</label>
                                                <!-- pick date -->
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="ui_opendate_from" />
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                                <!-- end pick date -->
                                            </div>
                                            <div class="col-lg-6">
                                                <label>To</label>
                                                <!-- pick date -->
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm date-picker" ID="ui_opendate_to" />
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                                <!-- end pick date -->
                                            </div>
                                        </div>
                                        <!-- end sub-row 3 -->
                                    </div>
                                </div>
                            </div>
                            <!-- end column 2 -->
                        </div>
                        <!-- end card content -->
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!-- end card body -->
        </div>
        <!-- end card -->

        <!-- start second form -->
        <div>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="update_search_2">
                <ContentTemplate>
                    <div class="card mb-4 shadow-sm">
                        <div class="card-body card-body-sm">
                            <div class="form-row">
                                <div class="col-4">
                                    <label>Impact</label>
                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ui_impact_code"></asp:DropDownList>
                                </div>
                                <div class="col-4">
                                    <label>Urgency</label>
                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ui_urgency_code"></asp:DropDownList>
                                </div>
                                <div class="col-4">
                                    <label>Priority</label>
                                    <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="ui_priority_code"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card mb-4 shadow-sm">
                        <div class="card-body card-body-sm">
                            <div class="form-row">
                                <div class="col-4">
                                    <label>Incident Group</label>
                                    <ag:AutoCompleteControl runat="server" id="txtProblemGroup" TODO_FunctionJS="loadDataDetailByIncidentAreaForSelect('INCIDENT_GROUP');" CssClass="form-control form-control-sm" />
                                </div>
                                <div class="col-4">
                                    <label>Incident Type</label>
                                    <ag:AutoCompleteControl runat="server" id="txtProblemType" TODO_FunctionJS="loadDataDetailByIncidentAreaForSelect('INCIDENT_TYPE');" CssClass="form-control form-control-sm" />
                                </div>
                                <div class="col-4">
                                    <label>Incident Source</label>
                                    <ag:AutoCompleteControl runat="server" id="txtProblemSource" TODO_FunctionJS="loadDataDetailByIncidentAreaForSelect('INCIDENT_SOURCE');" CssClass="form-control form-control-sm" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="col-4">
                                    <label>Content Source</label>
                                    <ag:AutoCompleteControl runat="server" id="txtContactSource" CssClass="form-control form-control-sm" />
                                </div>
                                <div class="col-4">
                                    <asp:UpdatePanel ID="udpDefauleSearch3" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>
                                            <label>Owner Service</label>
                                            <asp:DropDownList ID="ddlOwnerGroup" runat="server" class="form-control form-control-sm"
                                                DataTextField="OwnerGroupName" DataValueField="OwnerGroupCode">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="col-4">
                                    <label>Created By</label>
                                    <uc1:AutoCompleteEmployee runat="server" CssClass="form-control form-control-sm" id="AutoCompleteEmployee" />

                                    <%--<asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="tbcreatedby" />--%>
                                </div>
                            </div>
                            
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <!-- end second form -->

        <!-- group button -->
        <div>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="update_button">
                <ContentTemplate>
                    <!-- button -->
                    <asp:Button runat="server" CssClass="btn btn-info DEFAULT-BUTTON-CLICK" Text="Search" ID="ui_search_button" OnClick="ui_search_button_Click" OnClientClick="AGLoading(true);" />
                    <asp:Button runat="server" CssClass="btn btn-warning" Text="Clear" />
                    <asp:Button runat="server" CssClass="btn btn-success" Text="Export Data" ID="ui_export_button" OnClick="ui_export_button_Click" OnClientClick="AGLoading(true);" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <a id="download-report-excel" class="hide" target="_blank"
            href="<%= Page.ResolveUrl("~/API/ExportExcelAPI.ashx") %>"></a>
        <br />
        <div id="search-panel">
            <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="table-responsive">
                        <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                            <thead>
                                <tr>
                                    <th class="text-nowrap">Incident No.</th>
                                    <th class="text-nowrap">Priority</th>
                                    <th class="text-nowrap">Type</th>
                                    <th class="text-nowrap">Owner Service</th>
                                    <th class="text-nowrap">CI Name</th>
                                    <th class="text-nowrap">Status</th>
                                    <th class="text-nowrap">Area</th>
                                    <th class="text-nowrap">Open Date</th>
                                    <th class="text-nowrap">Responding Date</th>
                                    <th class="text-nowrap">Resolved Date</th>
                                    <th class="text-nowrap">Close Date</th>
                                    <th class="text-nowrap">Subject</th>
                                    <th class="text-nowrap">Description</th>
                                    <th class="text-nowrap">Close Log</th>
                                    <th class="text-nowrap">Assignee</th>
                                    <th class="text-nowrap">Group</th>
                                    <th class="text-nowrap">Assignee Closed</th>
                                    <th class="text-nowrap">Closed Group</th>
                                    <th class="text-nowrap">Assignee Resolved</th>
                                    <th class="text-nowrap">Resolved Group</th>
                                    <th class="text-nowrap">MTTNTime</th>
                                    <th class="text-nowrap">MTRSTime</th>
                                    <th class="text-nowrap">MTRSWTime</th>
                                    <th class="text-nowrap">MNSP</th>
                                    <th class="text-nowrap">Operator</th>
                                    <th class="text-nowrap">Stop Time</th>
                                    <th class="text-nowrap">Overdue Time</th>
                                    <th class="text-nowrap">CI Class</th>
                                    <th class="text-nowrap">Model</th>
                                    <th class="text-nowrap">S/N</th>
                                    <th class="text-nowrap">Attribute01</th>
                                    <th class="text-nowrap">Attribute02</th>
                                    <th class="text-nowrap">Attribute03</th>
                                    <th class="text-nowrap">CI Status</th>
                                    <th class="text-nowrap">Client Name</th>
                                    <th class="text-nowrap">Contact Name</th>
                                    <th class="text-nowrap">Contact Mail</th>
                                    <th class="text-nowrap">Contact Phone</th>
                                    <th class="text-nowrap">Affect</th>
                                    <th class="text-nowrap">Affect00</th>
                                    <th class="text-nowrap">Affect01</th>
                                </tr>
                            </thead>
                            <%--<tbody>
                                <asp:Repeater ID="rptSearchSale" runat="server">
                                    <ItemTemplate>
                                        <tr class="table-hover">
                                            <td class="text-nowrap">
                                                <%# Eval("IncidentNo") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("Priority") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("Type") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("Customer_Name") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("Status") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("Area") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("Open_Date") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("Responding_Date") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("Resolved_Date") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("Closed_Date") %>
                                            </td>
                                            <style>
                                                .the-column{
                                                    text-overflow: ellipsis;
                                                    overflow: hidden;
                                                    /*white-space: pre-wrap;*/
                                                    white-space: nowrap;
                                                    }
                                                </style>
                                            <td class="the-column" title=<%# Eval("Subject").ToString().Replace("<", "&lt;").Replace(">", "&gt;") %>> 
                                                <span><%# Eval("Subject").ToString().Replace("<", "&lt;").Replace(">", "&gt;") %></span>
                                            </td>
                                            <td class="text-nowrap" style="text-overflow: ellipsis;">
                                                <div class="two-line" title="<%# Eval("Description").ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "''") %>">
                                                    <%# Eval("Description").ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "</br>") %>
                                                </div>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("Assignee") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("Close_Log").ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "</br>") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("Group") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("Assignee_Closed") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("Closed_Group").ToString() %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("Assignee_Resolved") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("Resolved_Group") %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("MTTNTime").ToString() %>
                                            </td>
                                            <td class="text-nowrap">
                                                <%# Eval("MTRSTime").ToString() %>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>--%>
                        </table>
                    </div>
                    <div runat="server" id="divDataJson" class="d-none"></div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="d-none">
            <asp:UpdatePanel ID="udpBuutonEventSearchCriteria" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnBindContactForSearch" Text="" OnClick="btnBindContactForSearch_Click" runat="server" />
                    <asp:Button ID="btnBindMappingCustomerForSearch" OnClick="btnBindMappingCustomerForSearch_Click" runat="server" />
                    <asp:HiddenField ID="hhdModeEventFilter" runat="server" />
                    <asp:Button ID="btnIncidentAreaFilter" OnClick="btnIncidentAreaFilter_Click" runat="server" />

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </div>
    <!-- end -->
    <script type="text/javascript">
        function exportExcelAPI() {
            $("#download-report-excel")[0].click();
        }


        function afterSearch() {
            var reportsList = JSON.parse($("#<%= divDataJson.ClientID %>").html());
            var data = [];
            for (var i = 0 ; i < reportsList.length ; i++) {
                var report = reportsList[i];
                data.push([
                    report.IncidentNo,
                    report.Priority,
                    report.Type,
                    report.OwnerGroupName,
                    report.CIName.split(', \n').join("<br />"),
                    report.Status,
                    report.Area,
                    report.Open_Date,
                    report.Responding_Date,
                    report.Resolved_Date,
                    report.Closed_Date,
                    report.Subject,
                    report.Description,//.replace("\r\n", "</br>").replace("\n", "</br>"),
                    report.Close_Log,//.replace("\r\n", "</br></br>").replace("\n", "</br></br>"),
                    report.Assignee,
                    report.Group,
                    report.Assignee_Closed,
                    report.Closed_Group,
                    report.Assignee_Resolved,
                    report.Resolved_Group,
                    report.MTTNTime,
                    report.MTRSTime,
                    report.MTRSWTime,
                    report.ResponsibleOrganization,
                    report.Operator,
                    report.StopTime,
                    report.OverdueTime,
                    report.EquipmentClassName,
                    report.ModelNumber,
                    report.ManufacturerSerialNO,
                    report.PropertiesCode001,
                    report.PropertiesCode002,
                    report.PropertiesCode003,
                    report.CiStatus,
                    report.Customer_Name,
                    report.ContactName,
                    report.ContactMail,
                    report.ContactPhone,
                    report.Affect,
                    report.Affect00,
                    report.Affect01
                ]);
            }
            console.log(data);

            //$("#search-panel").show();
            $("#tableItems").dataTable({
                data: data,
                deferRender: true,
                columnDefs: [{
                    //"orderable": false,
                    "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40],
                    "createdCell": function (td, cellData, rowData, row, col) {
                        $(td).closest("tr").addClass("text-nowrap");
                    }
                },
                {
                    //"orderable": false,
                    "targets": [11, 12, 13],
                    "createdCell": function (td, cellData, rowData, row, col) {
                        var objPre = "<div class='six-line'><pre style=\"font-size: 100%;font-family: 'Titillium Web', sans-serif; margin: 0;\">"
                            + cellData + "</pre></div>"//, { html: cellData });
                        //$(td).addClass("six-line");
                        //$(td).addClass("text-nowrap");
                        $(td).html(objPre);
                    }
                }]
            });
            scrollToTable();
        }

        function scrollToTable() {
            $('html,body').animate({
                scrollTop: $("#search-panel").offset().top - 50
            });
        }

        //$(document).ready(function () {
        //    $('#tableItems').DataTable();
        //});
        function activeRequireField() {
            $(".required").prop('required', true);
        }

        function inactiveRequireField() {
            $(".required").prop('required', false);
        }

        function loadDataDetailByIncidentAreaForSelect(value) {
            inactiveRequireField();
            $("#<%= hhdModeEventFilter.ClientID %>").val(value);
            $("#<%= btnIncidentAreaFilter.ClientID %>").click();
        }
    </script>
</asp:Content>
