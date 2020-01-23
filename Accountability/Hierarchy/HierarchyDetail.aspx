<%@ Page Title="" Language="C#" MasterPageFile="~/Accountability/MasterPage/AccountabilityMaster.master" AutoEventWireup="true" CodeBehind="HierarchyDetail.aspx.cs" Inherits="ServiceWeb.Accountability.Hierarchy.HierarchyDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-role-hierarchy").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <style>
        .panel-structure{
            border-radius: 5px;
        }
        .hierarchy-loading{
            background:rgba(255, 255, 255, 0.68);
            position:absolute;
            top:0px;
            left:0px;
            right:0px;
            height:100%;
            width:100%;
            padding:20px;
            text-align:center;
            padding-top:150px;
        }
        .hierarchy-loading img{
            margin-top:-2px;
            width:20px;
            height:20px;
        }
        .pane-folder-container{
            position:relative;
            min-height:300px;
        }
        .pane-catalog-container{
            min-height:300px;
            border-left:1px solid #777;
        }
        
        .btn-hang-down{
            box-shadow:none;
            border-radius:0px 0px 10px 10px;
        }

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
        .possible-entry-row.active{
            color:orange;
        }
    </style>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="mat-box">
                <div>
                    <label class="page-header">
                        Hierarchy type & Possible entry
                    </label>
                </div>
                <div>
                    
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <label>
                                Hierarchy type
                            </label>
                            <span class="fa fa-cogs fa-fw" style="cursor:pointer;color: #0085A1;" onclick="$(this).next().click();"></span>
                            <asp:Button ID="btnShowPopup" runat="server" OnClick="showPopupClick" OnClientClick="AGLoading(true);" CssClass="hide" ClientIDMode="Static" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" CssClass="form-control" ID="ddlHierarchyType" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlHierarchyType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div>
                    <br />
                    
                    <script>
                        function manageHierarchyPossibleEntry(obj) {
                            if ($("#ddlHierarchyType").val() != null && $("#ddlHierarchyType").val() != "") {
                                $(obj).next().click();
                            } else {
                                AGError("Please choose hierarchy type");
                            }
                        }
                    </script>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <label>
                                Possible Entry
                            </label>
                            <span class="fa fa-cogs" style="cursor:pointer;color: #0085A1;" onclick="manageHierarchyPossibleEntry(this);"></span>
                            <asp:Button ID="btnstructure" runat="server" OnClick="showStructurClick" OnClientClick="AGLoading(true);" CssClass="hide" ClientIDMode="Static" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpPossibleEntry">
                        <ContentTemplate>
                            <div class="alert alert-warning text-center" runat="server" id="panelNoData">
                                <span>No Data.</span>
                            </div>
                            <div class="panel-structure" id="panelStructure" runat="server">
                                <ul style="padding-left: 20px;">
                                    <li class="possible-entry-row" onclick="possibleEntryClick(this);" data-node-level="-1" style="cursor:pointer;">
                                        <span>Root</span>
                                    </li>
                                    <asp:Repeater runat="server" ID="rptPossibleEntry" >
                                        <ItemTemplate>
                                            <li class="possible-entry-row" onclick="possibleEntryClick(this);" data-node-level="<%# Eval("NodeLevel") %>"   style='margin-left:<%# (20 * (Container.ItemIndex + 1)) %>px;cursor:pointer;'><%# Eval("Name") %></li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <script>
                        $(".possible-entry-row:last").addClass("active");
                    </script>
                </div>
            </div>
            <a href="<%= Page.ResolveUrl("~/Accountability/Hierarchy/HierarchyGroupList.aspx") %>">
                <i class="fa fa-arrow-circle-left"></i>
                Back to hierarchy master group
            </a>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-8">
            <div class="mat-box">
                <div>
                    <div class="pull-right" style="cursor:pointer;margin-top:5px;margin-left:10px;" onclick="showGuideLine(this);">
                        <i class="fa fa-check-square-o"></i>
                        Show new node guideline
                    </div>
                    <div class="pull-right" style="cursor:pointer;margin-top:5px;" onclick="showPossibleEntry(this);">
                        <i class="fa fa-check-square-o"></i>
                        Show possible entry
                    </div>

                    <label class="page-header">
                        Hierarchy
                    </label>
                </div>
                <div>
                    <script>
                        function possibleEntryClick(obj) {
                            obj = $(obj);
                            var level = parseInt(obj.attr("data-node-level"));
                            $(".agape-tree-li[posible-entry-node]").hide();
                            for (var i = level; i >= 0; i--) {
                                $(".agape-tree-li[posible-entry-node='" + i + "']").show();
                            }
                            $(".possible-entry-row").removeClass("active");
                            obj.addClass("active");
                        }
                        function showPossibleEntry(obj) {
                            obj = $(obj);
                            var gl = $(".node-possible-entry-name");
                            obj.find(".fa").toggleClass("fa-check-square-o").toggleClass("fa-square-o");
                            gl.fadeToggle();
                        }
                        function showGuideLine(obj) {
                            obj = $(obj);
                            var gl = $(".agape-tree-menu-guideline");
                            obj.find(".fa").toggleClass("fa-check-square-o").toggleClass("fa-square-o");
                            gl.fadeToggle();
                        }
                    </script>
                    <div style="min-height:100px;" class="pane-folder"></div>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
        <ContentTemplate>
            <asp:TextBox ID="txtPossibleEntry" runat="server" ClientIDMode="Static" CssClass="hide"></asp:TextBox>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="master-Hierarchy">
        <div class="modal-dialog" style="width: 800px;">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Hierarchy Type </h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                </div>
                <div class="modal-body">
                    <div id="divGetHierarchy">
                        <div style="margin-bottom: 15px;">
                            <button type="button" class="btn btn-primary pull-right" style="margin-top: -10px;margin-bottom: 10px;" onclick="addHierarchy();">
                                เพิ่มประเภท
                            </button>
                        </div>
                        <div>
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpList">
                                <ContentTemplate>
                                    <table class="table table-striped table-hover table-bordered">
                                        <tr>
                                            <th>
                                                Hierarchy type code
                                            </th>
                                            <th>
                                                Hierarchy type name
                                            </th>
                                            <th style="text-align:center;width:50px;">
                                                ลบ
                                            </th>
                                        </tr>
                                        <asp:Repeater runat="server" ID="rptHierarchy">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <%# Eval("HIERARCHYTYPECODE") %>
                                                    </td>
                                                    <td>
                                                        <%# Eval("HIERARCHYTYPENAME") %>
                                                    </td>
                                                    <td>
                                                        <%--<i style="cursor:pointer;color:red;width:30px;" class="fa fa-trash" onclick="$(this).next().click();"></i>--%>
                                                        <asp:Button Text="ลบ" runat="server" CssClass="btn btn-danger btn-sm" ID="btnDeleteType"
                                                             CommandArgument='<%# Eval("HIERARCHYGROUPCODE") + "," + Eval("HIERARCHYTYPECODE") %>' 
                                                            OnClick="btnDeleteType_Click" OnClientClick="return ConfirmDelete();" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div id="divAddHierarchy" style="display:none;">
                        <div class="row">
                            <div class="col-sm-12 col-md-6">
                                <label>
                                    Hierarchy type code
                                </label>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtCode" placeholder="Hierarchy type code" MaxLength="20" />
                            </div>
                            <div class="col-sm-12 col-md-6">
                                <label>
                                    Hierarchy type name
                                </label>
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtName" placeholder="Hierarchy type name" MaxLength="500" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-12 col-md-6">
                                <asp:Button Text="Save" runat="server" ID="btnAddType" CssClass="btn btn-primary" 
                                    OnClientClick="AGLoading(true);" OnClick="btnAddType_Click" />
                                <button type="button" class="btn btn-warning" onclick="backHierarchy();"> Back</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="master-PossibleEntry">
        <div class="modal-dialog" style="width: 800px;">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Possible Entry </h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <asp:UpdatePanel ID="udpLinkProjectCompanyStructure" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <ul id="sortable-blueprint">
                                        <asp:Repeater ID="rptHierarchyPossibleEntry" runat="server">
                                            <ItemTemplate>
                                                <li class="Levelnode" id='<%# Convert.ToInt32(Eval("PossibleEntryCode")) %>' style='padding-left: <%# Convert.ToInt32(Eval("NodeLevel")) * 20 %>px' title='<%# Eval("Description") %>'>
                                                    <i class="corner"></i>
                                                    <%# Eval("Name") %>&nbsp;
                                                        <span class="fa fa-remove" style="cursor:pointer;color:#0085A1;" onclick="$(this).next().click();"></span>
                                                        <asp:Button class="hide" ID="lbtnDelete" runat="server" OnClick="lbtnDelete_Click" OnClientClick="AGLoading(true,'กำลังลบ');" ClientIDMode="Static" CommandName='<%# Eval("PossibleEntryCode") %>' />
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                    <div class="new-structure" style='padding-left: <%= (rptHierarchyPossibleEntry.Items.Count) * 20 %>px'>
                                        <i class="corner"></i>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <asp:Panel ID="Panel1" runat="server" DefaultButton="lbtnSave">
                                                    <asp:TextBox ID="txtNameStructure" runat="server" CssClass="form-control require" placeholder="กรอกชื่อที่นี้"  />

                                                    <asp:TextBox ID="txtDescription" CssClass="form-control" TextMode="multiline" placeholder="กรอกคำอธิบายที่นี้" Rows="5" runat="server" />

                                                    <asp:LinkButton ID="lbtnSave" CssClass="btn btn-primary active" runat="server" OnClientClick="AGLoading(true);" OnClick="lbtnSave_Click">
                                                        <i class="fa fa-plus" >&nbsp;เพิ่ม</i>
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
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function showModal() {
            $("#master-Hierarchy").modal("show");
        }
        function hideModal() {
            $("#master-Hierarchy").modal("hide");
        }
        function showModalDetail() {
            $("#master-PossibleEntry").modal("show");
        }
        function hideModalDetail() {
            $("#master-PossibleEntry").modal("hide");
        }
        function addHierarchy() {
            $("#divGetHierarchy").hide();
            $("#divAddHierarchy").show();
        }
        function backHierarchy() {
            $("#divAddHierarchy").hide();
            $("#divGetHierarchy").show();
        }
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
        function ConfirmDelete() {
            if (AGConfirm('คุณต้องการลบใช่หรือไม่')) {
                AGLoading(true);
                return true;
            } else {
                return false;
            }
        }
        function hierarchyLoading() {
            removeLoading();
            $("body,html").css({
                //cursor: "wait"
            });
            var loading = $("<div/>", {
                class: "hierarchy-loading",
            });
            $(loading).append($("<img/>", {
                src: servictWebDomainName + "images/loadmessage.gif"
            }));
            $(loading).append($("<label/>", {
                html: "Loading hierarchy menu..",
                css: {
                    marginLeft: 10
                }
            }));
            $(".pane-folder-container").append(loading);
        }

        function catalogLoading() {
            removeLoading();
            $("body,html").css({
                //cursor: "wait"
            });
            var loading = $("<div/>", {
                class: "hierarchy-loading",
            });
            $(loading).append($("<img/>", {
                src: servictWebDomainName + "images/loadmessage.gif"
            }));
            $(loading).append($("<label/>", {
                html: "Loading hierarchy menu..",
                css: {
                    marginLeft: 10
                }
            }));
            $(".pane-catalog-container").append(loading);
        }

        function removeLoading() {
            $("body,html").css({
                //cursor: "default"
            });
            $(".hierarchy-loading").remove();
        }

        function bindHierarchy() {
            try {
                var ddl = document.getElementById('<%= ddlHierarchyType.ClientID %>');
                var hierarchyType = ddl.options[ddl.selectedIndex].value;

                var hierarchyTypeName = ddl.options[ddl.selectedIndex].text;
                $("#HierarchyTypeName").html(hierarchyTypeName);
                var apiUrl = "/Accountability/API//HirearchyStructureAPI.aspx";
                $(".pane-folder").AGWhiteLoading(true, "กำลังดึงข้อมูล");
                $.ajax({
                    url: apiUrl,
                    data: {
                        request: "list",
                        hierarchyType: hierarchyType
                    },
                    success: function (datas) {
                        //removeLoading();
                        var possibleentry = $("#txtPossibleEntry").val();
                        $(".pane-folder").aGapeTreeMenu({
                            //data: datas,
                            //rootText: hierarchyTypeName,
                            //onlyFolder: false,
                            //share: false,
                            //moveItem: true,
                            //selecting: false,
                            //emptyFolder: true,
                            data: datas,
                            rootText: "Root",
                            rootCode: "",
                            rootCount: 0,
                            navigateText: "Create structure",
                            onlyFolder: false,
                            share: false,
                            moveItem: false,
                            selecting: false,
                            emptyFolder: true,
                            PossibleEntry: $.parseJSON(possibleentry),

                            onClick: function (result) {
                            },
                            onNewFolder: function (result) {
                                hierarchyLoading();
                                $.ajax({
                                    url: apiUrl,
                                    data: {
                                        request: "newfolder",
                                        folderName: result.name,
                                        folderParent: result.parentid,
                                        hierarchyType: hierarchyType,
                                        hierarchyGroup: '<%= Request["groupcode"] %>'
                                    },
                                    success: function () {
                                        bindHierarchy();
                                    },
                                    error: function () {
                                        bindHierarchy();
                                    }
                                });
                            },
                            onMove: function (result) {
                                hierarchyLoading();
                                if (result.newParentNode == result.oldParentNode || result.itemType == "e") {
                                    bindHierarchy();
                                }
                                else {
                                    $.ajax({
                                        url: apiUrl,
                                        data: {
                                            request: "movenode",
                                            newParentNode: result.newParentNode,
                                            itemNode: result.itemNode,
                                            itemName: result.itemName,
                                            itemType: result.itemType,
                                            hierarchyType: hierarchyType
                                        },
                                        success: function () {
                                            bindHierarchy();
                                        },
                                        error: function () {
                                            bindHierarchy();
                                        }
                                    });
                                }
                            },
                            onRename: function (result) {
                                hierarchyLoading();
                                $.ajax({
                                    url: apiUrl,
                                    data: {
                                        request: "rename",
                                        id: result.id,
                                        name: result.name,
                                        hierarchyType: hierarchyType
                                    },
                                    success: function () {
                                        bindHierarchy();
                                    },
                                    error: function () {
                                        bindHierarchy();
                                    }
                                });
                            },
                            onDelete: function (result) {
                                hierarchyLoading();
                                $.ajax({
                                    url: apiUrl,
                                    data: {
                                        request: "deletefolder",
                                        id: result.id,
                                        name: result.name,
                                        type: result.type,
                                        hierarchyType: hierarchyType
                                    },
                                    success: function () {
                                        bindHierarchy();
                                    },
                                    error: function () {
                                        bindHierarchy();
                                    }
                                });
                            }
                        });
                        $(".pane-folder").AGWhiteLoading(false);
                    }

                });
            }
            catch (e) { }
        }
    </script>
</asp:Content>
