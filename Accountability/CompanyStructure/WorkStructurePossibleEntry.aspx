<%@ Page Title="" Language="C#" MasterPageFile="~/Accountability/MasterPage/AccountabilityMaster.master" AutoEventWireup="true" CodeBehind="WorkStructurePossibleEntry.aspx.cs" Inherits="ServiceWeb.Accountability.CompanyStructure.WorkStructurePossibleEntry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-ws-possible").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <style>
        #headerLinkProjectCompanyStructure {
            border-bottom: 1px solid #ccc;
            margin-bottom: 15px;
            padding-bottom: 8px;
        }

        #sortable-blueprint {
            list-style-type: none;
            -webkit-padding-start: 0px;
        }

        .corner {
            border-bottom: 2px solid #ccc;
            border-left: 2px solid #ccc;
            width: 10px;
            height: 15px;
            margin-bottom: 0px;
            margin-right: 8px;
            margin-top: -3px;
            margin-left: 0px;
            display: block;
            float: left;
        }

        #sortable-blueprint li {
            min-height: 30px;
        }

        .new-structure .form-control {
            margin-bottom: 10px;
        }
    </style>

    <div>
        <asp:UpdatePanel ID="udpLinkProjectCompanyStructure" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <span class="page-header">
                    <%--<a class="pull-right" href="-/InitiativeManagement/CompanyStructure/CompanyStructure.aspx?WorkGroupCode=<%= Request["WorkGroupCode"] %>">
                        <i class="fa fa-arrow-circle-right"></i>
                        Go to Work Structure  Configure
                    </a>--%>

                    Work Structure Possible Entry
                </span>
                <ul id="sortable-blueprint">
                    <li>
                        <i class="corner"></i>
                        Root
                    </li>
                    <asp:Repeater ID="rptLinkProjectCompanyStructure" runat="server">
                        <ItemTemplate>
                            <li class="Levelnode" id='<%# Convert.ToInt32(Eval("StructureCode")) %>' style='padding-left: <%# (Convert.ToInt32(Eval("NodeLevel")) + 1) * 20 %>px' title='<%# Eval("Description") %>'>
                                <i class="corner"></i>
                                <%# Eval("Name") %>&nbsp;
                                    <asp:LinkButton class="fa fa-remove" ID="lbtnDelete" runat="server" OnClick="lbtnDelete_Click" OnClientClick="AGLoading(true,'กำลังลบ');" ClientIDMode="Static" CommandName='<%# Eval("StructureCode") %>'></asp:LinkButton>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                    
                </ul>
                <div class="new-structure" style='padding-left: <%= ((rptLinkProjectCompanyStructure.Items.Count) + 1 ) * 20 %>px'>
                        <i class="corner"></i>
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Panel runat="server" DefaultButton="lbtnSave">
                                    <asp:TextBox ID="txtName" CssClass="form-control require" placeholder="Name" runat="server" />

                                    <asp:TextBox ID="txtDescription" CssClass="form-control" TextMode="multiline" placeholder="Description" Rows="5" runat="server" />

                                    <asp:LinkButton ID="lbtnSave" CssClass="btn btn-primary active AUTH_MODIFY" runat="server" OnClientClick="AGLoading(true,'Saving');" OnClick="lbtnSave_Click">
                                        <i class="fa fa-plus" >&nbsp;Add</i>
                                    </asp:LinkButton>
                                    <asp:TextBox ID="txtHideUpdate" ClientIDMode="Static" runat="server" Style="display: none;" />
                                    <asp:Button ID="btnHideUpdate" ClientIDMode="Static" runat="server" Style="display: none;" OnClick="btnHideUpdate_Click" />
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        function bindBluePrintSortable() {
            $("#sortable-blueprint").sortable({
                stop: function (event, ui) {
                    var sortArr = [];
                    $(".Levelnode").each(function (index) {
                        sortArr.push({
                            id: $(this).attr("id"),
                            index: index
                        });
                    });
                    document.getElementById('txtHideUpdate').value = JSON.stringify(sortArr);
                    document.getElementById("btnHideUpdate").click();
                }
            });
            $("#sortable").disableSelection();
        }
    </script>

</asp:Content>
