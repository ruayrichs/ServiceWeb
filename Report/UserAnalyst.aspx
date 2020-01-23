<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="UserAnalyst.aspx.cs" Inherits="ServiceWeb.Report.UserAnalyst" %>

<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteEmployee.ascx" TagPrefix="uc1" TagName="AutoCompleteEmployee" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-user-analyst").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <script>
        function bindDataModeView(obj) {
                $('#btnSearchData').click();
            }
    </script>

    <div class="card shadow">
        <div class="card-header">
            <h5 class="mb-0">User Analyst</h5>
        </div>
        <div class="card-body PANEL-DEFAULT-BUTTON">
            <div class="form-row">
                <div class="form-group col-sm-6">
                    <label>Employee Code / Name</label>
                    <asp:TextBox ID="txtEmployeeCodeName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
                <div class="form-group col-sm-6">
                     <label>Employee Group</label>
                        <asp:DropDownList runat="server" ID="ddlEmployeeGroup" CssClass="form-control form-control-sm">
                        </asp:DropDownList>
                 </div>
            </div>

            <button type="button" class="btn btn-info DEFAULT-BUTTON-CLICK" onclick="bindDataModeView(this);"><i class="fa fa-search"></i>&nbsp;&nbsp;Search</button>
           
            <asp:UpdatePanel ID="udpsearchButton" runat="server" UpdateMode="Conditional" class="d-none">
                <ContentTemplate>
                    <asp:Button ID="btnSearchData" runat="server" ClientIDMode="Static" OnClick="btnSearchData_Click" OnClientClick="AGLoading(true);" />

                    <asp:HiddenField ID="hhdEmployeeCode" ClientIDMode="Static" runat="server" />
                    <asp:Button ID="btnRedirectPage" ClientIDMode="Static" OnClick="btnRedirectPage_Click" OnClientClick="AGLoading(true);"  Text="" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div id="search-panel" style="display: none;">

                <hr />

                <asp:UpdatePanel ID="upPanelProfileList" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <!-- Mode Table View -->
                        <div class="table-responsive">
                            <table id="table-employee" class="table table-bordered table-striped table-hover table-sm nowrap">
                                <thead>
                                    <tr>
                                        <th class="text-center text-nowrap">Detail</th>
                                        <th>Employee Code / Name</th>                                                                                    
                                        <th>Employee Group</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>

                        <div style="display: none;" runat="server" id="divJsonEmployeeList" ClientIDMode="Static">[]</div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
    
    
    <!-- /.modal -->


    <script>

        function afterSearch() {
            var EmployeeList = JSON.parse($("#divJsonEmployeeList").html());
            var data = [];
            for (var i = 0 ; i < EmployeeList.length ; i++) {
                var Employee = EmployeeList[i];
                data.push([
                    Employee.EmployeeCode,
                    Employee.EmployeeCode + " : " + Employee.FullName,
                    Employee.EmployeeGroup + " : " + Employee.EmployeeGroupDesc
                ]);
            }

            $("#search-panel").show();
            $("#table-employee").dataTable({
                data: data,
                deferRender: true,
                columnDefs: [{
                    "orderable": false,
                    "targets": [0],
                    "createdCell": function (td, cellData, rowData, row, col) {
                        $(td).addClass("text-center");
                        $(td).html(
                            '<span title="View User Detail">' +
                            '<i class="fa fa-user fa-lg text-dark c-pointer" aria-hidden="true"></i>' +
                            '</a>'
                         );
                        $(td).closest("tr").data("key", rowData[0]);
                        $(td).closest("tr").addClass("c-pointer");
                        $(td).closest("tr").bind({
                            click: function () {
                                btnRedirectPageDetail(this);
                            }
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

        function btnRedirectPageDetail(obj)
        {
            $("#hhdEmployeeCode").val($(obj).data("key"));
            $("#btnRedirectPage").click();
        }

        //function processSaveContact(sender, container) {
        //    if (AGValidator(sender, container)) {
        //        $("#btnSaveContact").click()
        //    }

        //    return false;
        //}

    </script>
</asp:Content>
