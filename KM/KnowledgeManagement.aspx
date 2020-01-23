<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="KnowledgeManagement.aspx.cs" Inherits="ServiceWeb.KM.KnowledgeManagement" %>
<%@ Register Src="~/widget/usercontrol/TimeLineControl.ascx" TagPrefix="sna" TagName="TimeLineControl" %>
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
         function bindDataModeView(obj) {
             inactiveRequireField();
             $('#btnSearchData').click();
         }
    </script>

    <div class="card shadow">
        <div class="card-header">
            <h5 class="mb-0">Knowledge Management</h5>
        </div>
        <div class="card-body PANEL-DEFAULT-BUTTON">
            <div class="form-row">
                <div class="form-group col-sm-6">
                     <label>Knowledge Group</label>
                        <asp:DropDownList runat="server" ID="ddlGroup" CssClass="form-control form-control-sm">
                        </asp:DropDownList>
                 </div>
                 <div class="form-group col-sm-6">
                    <label>Search Text </label>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
            </div>

            <button type="button" class="btn btn-info DEFAULT-BUTTON-CLICK" onclick="bindDataModeView(this);"><i class="fa fa-search"></i>&nbsp;&nbsp;Search</button>
            <button type="button" class="btn btn-success AUTH_MODIFY" onclick="$('#btnOpenModalCreate').click();"><i class="fa fa-file-o"></i>&nbsp;&nbsp;Create</button>
           
            <asp:UpdatePanel ID="udpsearchButton" runat="server" UpdateMode="Conditional" class="d-none">
                <ContentTemplate>
                    <asp:Button ID="btnSearchData" runat="server" ClientIDMode="Static" OnClick="btnSearchData_Click" OnClientClick="AGLoading(true);" />
                    <asp:Button ID="btnOpenModalCreate" runat="server" ClientIDMode="Static" OnClick="btnOpenModalCreate_Click" OnClientClick="AGLoading(true);" />

                    <asp:HiddenField ID="hddKnowledgeID" ClientIDMode="Static" runat="server" />
                    <asp:Button ID="btnSubmitRedirect" ClientIDMode="Static" OnClientClick="AGLoading(true);" OnClick="btnSubmitRedirect_Click" Text="" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div id="search-panel" style="display: none;">

                <hr />

                <asp:UpdatePanel ID="upPanelProfileList" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <!-- Mode Table View -->
                        <div class="table-responsive">
                            <table id="table-dataview" class="table table-bordered table-striped table-hover table-sm nowrap">
                                <thead>
                                    <tr>
                                        <th class="text-center text-nowrap"></th>
                                        <th>Knowledge Group</th>                                                                                    
                                        <th>Knowledge ID</th>
                                        <th>Keyword</th>                                                                                    
                                        <th>Subject</th>
                                        <th>Detail</th>         
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
    
    
    <!-- /.modal -->

    <script>

        function afterSearch(ListData) {
            var data = [];
            for (var i = 0 ; i < ListData.length ; i++) {
                var jArr = ListData[i];
                data.push([
                    jArr.ObjectID,
                    jArr.KMGroupName,
                    jArr.ObjectID,
                    jArr.PrimaryKeyWord,
                    jArr.Description,
                    jArr.Details
                ]);
            }

            $("#search-panel").show();
            $("#table-dataview").dataTable({
                data: data,
                deferRender: true,
                columnDefs: [{
                    "orderable": false,
                    "targets": [0],
                    "createdCell": function (td, cellData, rowData, row, col) {
                        $(td).addClass("text-center");
                        $(td).html(
                            "<a class='c-pointer' onclick=\"RedirectclickToDetail('" + rowData[0] + "');\" title=\"Edit Knowledge\">" +
                            "<i class=\"fa fa-pencil-square fa-lg text-dark\" aria-hidden=\"true\"></i>" +
                            "</a>"
                         );
                        $(td).closest("tr").addClass("c-pointer");
                        $(td).closest("tr").data("key", rowData[0]);
                        $(td).closest("tr").bind({
                            click: function () {
                                RedirectclickToDetail($(this).data("key"));
                                // Do something on click
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

        function activeRequireField() {
            $(".required").prop('required', true);
        }

        function inactiveRequireField() {
            $(".required").prop('required', false);
        }

        function submitCheckRequireField() {
            activeRequireField();
            $("#btnSubmitRequire").click();
        }

        function CreateDetailRefModalClick(obj)
        {            
            if (AGConfirm(obj, "Save Confirm")) {
                $("#btnCreateDetail").click();
            }
        }

        function RedirectclickToDetail(id)
        {
            inactiveRequireField();
            $("#hddKnowledgeID").val(id);
            $("#btnSubmitRedirect").click();
        }

    </script>

    <div class="initiative-model-control-slide-panel" id="modal-CreateData">
        <div class="initiative-model-control-body-content z-depth-3">
            <div>
                <div class="initiative-model-control-header">
                    <div class="mat-box-initaive-control">
                        <div class="pull-right">
                            <i class="fa fa-close hand" onclick="closeInitiativeModal('modal-CreateData');"></i>
                        </div>
                        <div class="one-line">
                            <label class="text-warning">
                               Create Knowledge Management
                            </label>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-control-contant">
                    <div class="panel-body-initiative-master">
                        <div class="panel-content-initiative-master">
                            <div class="mat-box-initaive-control tab-initiative-control">
                               
								<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCreateDetail">
									<ContentTemplate>
                                        <nav class="d-none">
                                            <div class="nav nav-tabs" id="nav-tab" role="tablist">
                                                <a class="nav-item nav-link active" id="nav-header-tab" data-toggle="tab" href="#nav-information" role="tab" aria-controls="nav-header" aria-selected="true">Information</a>
                                                <a class="nav-item nav-link" id="nav-item-tab" data-toggle="tab" href="#nav-attachfile" role="tab" aria-controls="nav-item" aria-selected="false">Attach File
                                                </a>
                                            </div>
                                        </nav>
                                        <div class="tab-content p-3" id="nav-tabContent">
                                            <div class="tab-pane fade show active" id="nav-information" role="tabpanel" aria-labelledby="nav-information-tab">
                                                <div class="form-row">
                                                    <asp:HiddenField ID="hhdKeyAobjectlink" runat="server" />
                                                    <div class="form-group col-md-6">
                                                        <label>Knowledge Group</label>
                                                         <asp:DropDownList runat="server" ID="ddlGroupModal" CssClass="form-control form-control-sm required">
                                                        </asp:DropDownList>
                                                    </div>
                                                     <div class="form-group col-md-6">
                                                        <label>Keyword</label>
                                                         <asp:TextBox ID="txtKeywordModal" placeholder="Text" CssClass="form-control form-control-sm" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-md-6">
                                                        <label>Subject</label>
                                                        <asp:TextBox ID="txtSubjectModal" placeholder="Text" TextMode="MultiLine" Rows="3" CssClass="form-control form-control-sm required" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-6">
                                                        <label>Detail</label>
                                                        <asp:TextBox ID="txtDetailModal" placeholder="Text" TextMode="MultiLine" Rows="3" CssClass="form-control form-control-sm" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="form-row">
                                                    <div class="form-group col-md-6">
                                                        <label>Symtom</label>
                                                        <asp:TextBox ID="txtSymtomModal" placeholder="Text" TextMode="MultiLine" Rows="3" CssClass="form-control form-control-sm" runat="server" />
                                                    </div>
                                                    <div class="form-group col-md-6">
                                                        <label>Cause</label>
                                                        <asp:TextBox ID="txtCauseModal" placeholder="Text" TextMode="MultiLine" Rows="3" CssClass="form-control form-control-sm" runat="server" />
                                                    </div>
                                                </div>
                                                 <div class="form-row">
                                                    <div class="form-group col-md-12">
                                                        <label>Solution</label>
                                                        <asp:TextBox ID="txtSolutionModal" placeholder="Text" TextMode="MultiLine" Rows="3" CssClass="form-control form-control-sm" runat="server" />
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="tab-pane fade" id="nav-attachfile" role="tabpanel" aria-labelledby="nav-attachfile-tab">
                                                <div>
                                                    <sna:TimeLineControl runat="server" id="TimeLineControl" />
                                                </div>
                                            </div>
                                        </div>
									</ContentTemplate>
								</asp:UpdatePanel>
								<asp:UpdatePanel runat="server" UpdateMode="Conditional">
									<ContentTemplate>
                                        <asp:Button runat="server" CssClass="d-none" ID="btnSubmitRequire"
											OnClick="btnSubmitRequire_Click" ClientIDMode="Static" />
                                        <button type="button" id="btnCreateClient" class="d-none" onclick="CreateDetailRefModalClick(this);"></button>
										<asp:Button Text="" runat="server" CssClass="d-none" ID="btnCreateDetail"
											OnClick="btnCreateDetail_Click" ClientIDMode="Static" OnClientClick="AGLoading(true);" />
									</ContentTemplate>
								</asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="initiative-model-bottom">
                    <div class="text-right">
						<span class="water-button AUTH_MODIFY" onclick="submitCheckRequireField();"><i class="fa fa-file-o"></i>&nbsp;Save</span>
                        <a class="water-button" onclick="closeInitiativeModal('modal-CreateData');"><i class="fa fa-close"></i>&nbsp;Close</a>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
