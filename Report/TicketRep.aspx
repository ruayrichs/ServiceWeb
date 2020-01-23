<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="TicketRep.aspx.cs" Inherits="ServiceWeb.Report.TicketRep" %>

<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEmployee.ascx" TagPrefix="uc1" TagName="AutoCompleteEmployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-ticket-analysis").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    <style type="text/css">
        .wrap {
            white-space: normal;
            width: 100%;
        }
    </style>
    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Ticket Tracking</h5>
                </div>
                <div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="card lg-12 shadow-sm">
                                <div class="card-body">
                                    <div class="form-row">
                                        <div class="col-sm">
                                            <div class="form-group">
                                                <label>Created By</label>
                                                <uc1:autocompleteemployee runat="server" cssclass="form-control form-control-sm" id="AutoCompleteEmployee" />
                                            </div>
                                            <div class="form-group">
                                                <label>Page Mode</label>
                                                <asp:DropDownList ID="ddlPageModetype" CssClass="form-control form-control-sm" runat="server">
                                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                    <asp:ListItem Text="CREATED" Value="CREATED"></asp:ListItem>
                                                    <asp:ListItem Text="DISPLAY" Value="DISPLAY"></asp:ListItem>
                                                    <asp:ListItem Text="CHANGE" Value="CHANGE" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-sm">
                                            <div class="form-group">
                                                <label>Date Time In From</label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" onchange="$('#btnWarrantyNextMaintenanceRefStartDate').click();" CssClass="form-control form-control-sm date-picker"
                                                        ID="tbDateTimeIn"
                                                        placeholder="dd/mm/yyy" />
                                                    <span class="input-group-append hand">
                                                        <i class="fa fa-calendar input-group-text"></i>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label>Date Time In To</label>
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" onchange="$('#btnWarrantyNextMaintenanceRefEndDate').click();" CssClass="form-control form-control-sm date-picker"
                                                        ID="tbDateTimeOut"
                                                        placeholder="dd/mm/yyy" />
                                                    <span class="input-group-append hand">
                                                        <i class="fa fa-calendar input-group-text"></i>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    </br>
                                    <div class="form-row">
                                        <asp:Button Text="Search" runat="server" ID="Button1"
                                            CssClass="btn btn-info DEFAULT-BUTTON-CLICK"
                                            OnClick="btn_search"
                                            OnClientClick="AGLoading(true);" />
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="row">
                    <div class="col">
                        <div id="search-panel" style="padding: 18px;">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdateTable">
                                <ContentTemplate>
                                    <div class="table-responsive">
                                        <table id="repTB" class="table table-bordered table-striped table-hover table-sm">
                                            <thead>
                                                <tr>
                                                    <th class="text-nowrap">Employee Name</th>
                                                    <th class="text-nowrap">PageName</th>
                                                    <th class="text-nowrap">OS</th>
                                                    <th class="text-nowrap">Browser</th>
                                                    <th class="text-nowrap">Mobile</th>
                                                    <th class="text-nowrap">LiveOn</th>
                                                    <th class="text-nowrap ">Date Time In</th>
                                                    <th class="text-nowrap">Date Time Out</th>
                                                    <th class="text-nowrap">ReferenceID</th>
                                                    <th class="text-nowrap">Reference Page Mode</th>
                                                </tr>
                                            </thead>
                                            <%--<tbody>
                                                <asp:Repeater runat="server" ID="TicketRepeater">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><%#Eval("FirstName") + " " + Eval("LastName")%> </td>
                                                            <td><%#Eval("PageName")%></td>
                                                            <td><%#Eval("OS")%></td>
                                                            <td><%#Eval("Browser")%></td>
                                                            <td><%#Eval("Mobile")%></td>
                                                            <td><%#Eval("LiveOn")%></td>
                                                            <td><%# Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("DateTimeIn").ToString()) %></td>
                                                            <td class="col-status">
                                                                <span class=" 
                                                                     <%#Eval("ReferencePageMode").ToString().Equals("DISPLAY") || Eval("ReferencePageMode").ToString().Equals("CREATED")
                                                                        || !string.IsNullOrEmpty(Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("DateTimeOut").ToString()))? "d-none":"" %>">
                                                                    <button type="button" class="btn btn-danger btn-sm wrap" onclick="confirmChange(this);">Update Date Time Out</button>
                                                                    <asp:Button ID="btnDateTimeOut" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btn_udpDateTimeOut" CommandArgument='<%# Eval("Row_key") %>' />
                                                                </span>

                                                                <%#Agape.FocusOne.Utilities.Validation.Convert2DateTimeDisplay(Eval("DateTimeOut").ToString())%>
                                                            </td>
                                                            <td><%#Eval("ReferenceID")%></td>
                                                            <td><%#Eval("ReferencePageMode")%></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>--%>
                                        </table>
                                        <div runat="server" id="divDataJson" class="d-none"></div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="d-none">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="hddKey" />
                    <asp:Button ID="btnDateTimeOut" runat="server" CssClass="AUTH_MODIFY" OnClick="btn_udpDateTimeOut" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <script>
            function afterSearch() {
                var reportsList = JSON.parse($("#<%= divDataJson.ClientID %>").html());
                var data = [];
                for (var i = 0 ; i < reportsList.length ; i++) {
                    var report = reportsList[i];
                    data.push([
                        report.FirstName + " " + report.LastName,
                        report.PageName,
                        report.OS,
                        report.Browser,
                        report.Mobile,
                        report.LiveOn,
                        report.DateTimeIn,
                        report.ReferencePageMode + "|" + report.DateTimeOut + "|" + report.Row_key,
                        report.ReferenceID,
                        report.ReferencePageMode
                    ]);
                }
                console.log(data);

                //$("#search-panel").show();
                $("#repTB").dataTable({
                    data: data,
                    deferRender: true,
                    columnDefs: [{
                        //"orderable": false,
                        "targets": [0, 1, 2, 3, 4, 5, 6, 8, 9],
                        "createdCell": function (td, cellData, rowData, row, col) {
                            $(td).addClass("text-nowrap");
                        }
                    }, {
                        //"orderable": false,
                        "targets": [7],
                        "createdCell": function (td, cellData, rowData, row, col) {
                            var datas = cellData.split('|');
                            if ("CHANGE" == datas[0] && datas[1] == '') {
                                $(td).html(
                                    '<button type="button" class="btn btn-danger btn-sm wrap" data-key="' + datas[2] + '" '
                                    + ' onclick="confirmChange(this);">Update Date Time Out</button>'
                                );
                            } else {
                                $(td).html(datas[1]);
                            }
                            $(td).addClass("text-nowrap");
                            //$(td).closest("tr").addClass("text-nowrap");
                        }
                    }]
                });
                scrollToTable();
            }

            //$(document).ready(function () {
            //    $('#repTB').DataTable();
            //});
            function confirmChange(sender) {
                inactiveRequireField();
                if (AGConfirm(sender, "Update Date Time Out")) {
                    AGLoading(true);
                    $("#<%= hddKey.ClientID %>").val($(sender).attr('data-key'));
                    $("#<%= btnDateTimeOut.ClientID %>").click();
                }
            }
            function scrollToTable() {
                $('html,body').animate({
                    scrollTop: $("#search-panel").offset().top - 50
                });
            }
        </script>
    </div>
</asp:Content>
