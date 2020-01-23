<%@ Page Title="" Language="C#" MasterPageFile="~/Accountability/MasterPage/AccountabilityMaster.master" AutoEventWireup="true" CodeBehind="MasterCharacter.aspx.cs" Inherits="ServiceWeb.Accountability.Character.MasterCharacter" %>

<%@ Register Src="~/widget/usercontrol/SmartSearchMainDelegate.ascx" TagPrefix="uc1" TagName="SmartSearchMainDelegate" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-role-config").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); })
    </script>
    <style>
        .btn-add {
            padding: 3px 8px;
            border: 1px dashed #009688;
            display: block;
            text-align: center;
            border-radius: 3px;
            cursor: pointer;
        }

        .row {
            margin-bottom: 7px;
        }

            .row:last-child {
                margin-bottom: 0px;
            }

        .gvpaging > td {
            padding: 0 !important;
        }

            .gvpaging > td td {
                border: none !important;
                padding: .75rem 3px;
            }

                .gvpaging > td td > a {
                    margin: 0 !important;
                    border-radius: 0 !important;
                }

                .gvpaging > td td > span {
                    background-color: #00BCD4;
                    padding: 5px 10px 5px 10px;
                    color: #fff;
                    text-decoration: none;
                    -o-box-shadow: 1px 1px 1px #111;
                    -moz-box-shadow: 1px 1px 1px #111;
                    -webkit-box-shadow: 1px 1px 1px #008ea0;
                    box-shadow: 1px 1px 1px #008ea0;
                }
    </style>

    <div id="divHeirarchy">

        <span class="page-header">

            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <% if (ERPW.Lib.Authentication.ERPWAuthentication.Permission.AllPermission)
                       { %>
                    <button type="button" class="btn btn-primary mb-1 AUTH_MODIFY" onclick="$(this).next().click()"><i class="fa fa-plus-circle"></i>&nbsp;&nbsp;Add hierarchy from Role Hierarchy</button>
                    <asp:Button ID="btnShowPopup" runat="server" Text="Add hierarchy from Role Hierarchy" OnClick="showPopupClick" OnClientClick="AGLoading(true);" CssClass="btn btn-primary AUTH_MODIFY d-none" ClientIDMode="Static" />
                    <% } %>
                </ContentTemplate>
            </asp:UpdatePanel>
        </span>




        <div class="card shadow">
            <div class="card-header">
                <h5 class="mb-0">Role Configure</h5>
            </div>


            <div class="card-body panel-body-customer-search PANEL-DEFAULT-BUTTON">
                <div class="row">
                    <div class="col-lg-12">
                        <asp:UpdatePanel ID="gvUpdatePanel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvData" runat="server" CssClass="table table-sm table-hover table-striped" AllowPaging="true"
                                    PageSize="10" PagerStyle-CssClass="gvpaging" AutoGenerateColumns="false" EmptyDataText="No data to display"
                                    OnPageIndexChanging="gvData_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField HeaderText="Hierarchy Group" DataField="HIERARCHYGROUPNAME" />
                                        <asp:BoundField HeaderText="Hierarchy Type" DataField="HIERARCHYTYPENAME" />
                                        <asp:TemplateField HeaderText="Delete" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <% if (ERPW.Lib.Authentication.ERPWAuthentication.Permission.AllPermission)
                                                   { %>
                                                <asp:LinkButton ID="LinkbtnDelete" CssClass="fa fa-trash" Style="color: Red; font-size: 20px;"
                                                    runat="server" CommandArgument='<%# Eval("CharacterCode") %>' OnClick="LinkbtnDelete_Click" OnClientClick="return confirm('ยืนยันการลบข้อมูล');" />
                                                <% }
                                                   else
                                                   { %>
                                                        <a href="#" class="fa fa-trash" aria-disabled="true" Style="color: #aaa; font-size: 20px;"></a>
                                                   <% } %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Participants" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ClientIDMode="Static" ID="HierarchyFull" Value='<%# Eval("HIERARCHYGROUPNAME") + " : " + Eval("HIERARCHYTYPENAME") %>' />
                                                <asp:HiddenField runat="server" ClientIDMode="Static" ID="HierarchytTypeCode" Value='<%# Eval("HIERARCHYTYPE") %>' />
                                                <asp:HiddenField runat="server" ClientIDMode="Static" ID="Character" Value='<%# Eval("CharacterCode") %>' />
                                                <span class="fa fa-users" style="cursor: pointer; color: #0085A1; font-size: 20px;" onclick="$(this).next().click();"></span>
                                                <button id="btnManage" class="hide" onclick="clickManage(this);"></button>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

        </div>


    </div>

    <div id="divCharacter" style="display: none;">
        <div class="pull-right">
            <button class="btn btn-warning btn-sm" style="margin-bottom: 5px; width: 100px;" onclick="return backManage();">กลับ</button>
        </div>

        <asp:Label ID="lbHierarchyTypeHeader" runat="server" ClientIDMode="Static" CssClass="page-header"></asp:Label>

        <div class="row">
            <div class="col-sm-12 col-md-4">
                <div>
                    <div style="min-height: 100px; margin-top: -15px;" class="pane-folder"></div>
                </div>
            </div>
            <div class="col-sm-12 col-md-8">

                <div class="hide">
                    <asp:HiddenField runat="server" ID="hddStructureCode" ClientIDMode="Static" />
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button Text="text" ID="btnBindData" ClientIDMode="Static" OnClick="btnBindData_Click" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="alert alert-warning " id="panelFirstLoad" style="display: block;">
                    <div class="text-center">
                        <i class="fa fa-arrow-circle-left"></i>เลือก Structure
                    </div>
                </div>
                <div id="panelContent" style="display: none;">
                    <div class="panel panel-primary">
                        <div class="panel-heading" style="padding: 10px;">
                            <h5 style="margin: 0;">
                                <label id="HeaderName"></label>
                            </h5>
                        </div>
                        <div class="panel-body" style="padding: 10px;">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:UpdatePanel runat="server" ID="udpTableOwner" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table class="table">
                                                <asp:Repeater runat="server" ID="rptInitiativeOwner">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td style="width: 85px;">
                                                                <div class="img-circle-box" style="border-radius: 0; background-image: url('<%# ServiceWeb.Service.UserImageService.getImgProfile(Eval("EmployeeCode").ToString()).Image_128 %>'),url('/images/user.png'); background-size: cover;"></div>
                                                            </td>
                                                            <td>
                                                                <%# Eval("fullname") %>
                                                            </td>
                                                            <td style="width: 200px;">
                                                                <div>
                                                                    <asp:RadioButton Text="&nbsp;&nbsp;ผู้รับผิดชอบหลัก" runat="server" ID="chkMainDelegate"
                                                                        Checked='<%# Eval("MainDelegate").ToString() == "TRUE" %>' CssClass='<%# Eval("EmployeeCode") %>'
                                                                        OnCheckedChanged="chkMainDelegate_CheckedChanged" AutoPostBack="true" onchange="AGLoading(true);" />
                                                                </div>
                                                                <div>
                                                                    <asp:CheckBox Text="&nbsp;&nbsp;มีสิทธิ์ส่งต่องาน" runat="server" ID="chkAuthenTran"
                                                                        Checked='<%# Eval("AuthenTransferTask").ToString() == "TRUE" %>' CssClass='<%# Eval("EmployeeCode") %>'
                                                                        OnCheckedChanged="chkAuthenTran_CheckedChanged" AutoPostBack="true" onchange="AGLoading(true);" />
                                                                </div>
                                                                <div>
                                                                    <asp:CheckBox Text="&nbsp;&nbsp;มีสิทธิ์ปิดงาน" runat="server" ID="chkAuthenClose"
                                                                        Checked='<%# Eval("AuthenCloseTask").ToString() == "TRUE" %>' CssClass='<%# Eval("EmployeeCode") %>'
                                                                        OnCheckedChanged="chkAuthenClose_CheckedChanged" AutoPostBack="true" onchange="AGLoading(true);" />
                                                                </div>
                                                            </td>
                                                            <td style="width: 85px;">
                                                                <asp:Button Text="Delete" CssClass="btn btn-danger btn-sm" runat="server" Style="width: 80px;" OnClientClick="return confirm('ยืนยันการลบข้อมูล');"
                                                                    CommandArgument='<%# Eval("SID") + "," + Eval("WorkGroupCode") + "," + Eval("EmployeeCode") + "," + Eval("HierarchyCode") %>'
                                                                    ID="btnRemove" OnClick="btnRemove_Click" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <tr style="border: solid 1px #fff;">
                                                    <td colspan="4" style="background-color: #fff; border: none; padding: 10px 0px;">
                                                        <div class="btn-add" onclick="addEmpOwner();">Add participants</div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="master-character-modaler">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Add hierarchy</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="updModalHierarchy" ClientIDMode="Static" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <asp:Repeater runat="server" ID="rptHierarchyGroup" OnItemDataBound="rptHierarchy_ItemDataBound">
                                    <ItemTemplate>
                                        <div class="col-12" style="text-decoration: underline;">
                                            <label id="lbHierarchyGroupName"><%# Eval("HIERARCHYGROUPNAME") %></label>
                                            <asp:HiddenField runat="server" ID="hddHierarchyGroupCode" Value='<%# Eval("HIERARCHYGROUPCODE") %>' />
                                        </div>

                                        <div class="col-12">
                                            <asp:Repeater runat="server" ID="rptHierarchyType">
                                                <ItemTemplate>
                                                    <div class="row">
                                                        <div class="col-1 col-sm-1 col-md-1 col-lg-1">
                                                            &nbsp;
                                                        </div>
                                                        <div class="col-7 col-sm-8 col-md-9 col-lg-9">
                                                            <div>
                                                                <span>-  </span>
                                                                <asp:Label ID="lbHierarchyTypeName" runat="server" Text='<%# Eval("HIERARCHYTYPENAME") %>'></asp:Label>
                                                                <asp:HiddenField runat="server" ID="hddHierarchyGroupCode2" Value='<%# Eval("HIERARCHYGROUPCODE") %>' />
                                                                <asp:HiddenField runat="server" ID="hddHierarchyTypeCode" Value='<%# Eval("HIERARCHYTYPECODE") %>' />
                                                            </div>
                                                        </div>
                                                        <div class="col-3 col-sm-3 col-md-2 col-lg-2 text-right">
                                                            <asp:Button ID="btnAddHierarchyType" runat="server" CssClass="btn btn-primary btn-sm AUTH_MODIFY" OnClick="btnAddHierarchyType_Click" OnClientClick="AGLoading(true)" Text="ADD" />
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div id="modalAddEmp" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Choose participants</h4>
                    <button type="button" class="close" data-dismiss="modal">×</button>
                </div>
                <div class="modal-body">
                    <label>
                        เลือกผู้เกี่ยวข้อง
                    </label>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpSmartSearch">
                        <ContentTemplate>
                            <uc1:SmartSearchMainDelegate isOnlyFriend="false" runat="server" id="SmartSearchMainDelegate" />
                            <asp:HiddenField runat="server" ID="hddParticipantsCode" ClientIDMode="Static" />
                            <br />
                            <div>
                                <asp:CheckBox Text="&nbsp;&nbsp;ผู้รับผิดชอบหลัก" runat="server" ID="chkMainDelegate" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField runat="server" ClientIDMode="Static" ID="CharacterCode" />
                            <asp:Button Text="Save" CssClass="btn btn-primary" ID="btnSaveEmpOwner" OnClick="btnSaveEmpOwner_Click"
                                runat="server" ClientIDMode="Static" OnClientClick="AGLoading(true);" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <script>
        function showModalMasterCharacter() {
            $("#master-character-modaler").modal("show");
        }
        function hideModalMasterCharacter() {
            $("#master-character-modaler").modal("hide");
        }
        function addEmpOwner() {
            $("#modalAddEmp").modal("show");
        }
        function closeEmpOwner() {
            $("#modalAddEmp").modal("hide");
        }
        function clickManage(evt) {
            var hierarchyfull = $(evt).parent().find("#HierarchyFull").val()
            var hierarchytypeMasterCharacter = $(evt).parent().find("#HierarchytTypeCode").val()
            var character = $(evt).parent().find("#Character").val()
            $("#lbHierarchyTypeHeader").text(hierarchyfull);
            $("#CharacterCode").val(character);
            $("#divHeirarchy").hide();
            $("#divCharacter").show();
            bindHierarchy(hierarchytypeMasterCharacter);
        }
        function backManage() {
            $("#divCharacter").hide();
            $("#divHeirarchy").show();
            $("#panelFirstLoad").show();
            $("#panelContent").hide();

            return false;
        }
        function bindHierarchy(hierarchytypeMasterCharacter) {
            var apiUrl = "/Accountability/API/HirearchyStructureAPI.aspx";
            $(".pane-folder").AGWhiteLoading(true, "กำลังดึงข้อมูล");
            $.ajax({
                url: apiUrl,
                data: {
                    request: "list",
                    hierarchyType: hierarchytypeMasterCharacter
                },
                success: function (datas) {
                    var possibleentry = $("#txtPossibleEntry").val();
                    $(".pane-folder").aGapeTreeMenu({
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

                        onClick: function (result) {
                            console.log(result);
                            AGLoading(true);
                            $("#hddStructureCode").val(result.id);
                            $("#HeaderName").text(result.name);
                            $("#btnBindData").click();
                        },
                        onNewFolder: function (result) {
                            $.ajax({
                                url: apiUrl,
                                data: {
                                    request: "newfolder",
                                    folderName: result.name,
                                    folderParent: result.parentid,
                                    hierarchyType: hierarchytypeMasterCharacter,
                                    hierarchyGroup: 'CATALOG'
                                },
                                success: function (data) {
                                    if (data.status != "") {
                                        alert(data.status)
                                    }
                                    bindHierarchy(hierarchytypeMasterCharacter);
                                },
                                error: function () {
                                    bindHierarchy(hierarchytypeMasterCharacter);
                                }
                            });
                        },
                        onMove: function (result) {
                            if (result.newParentNode == result.oldParentNode || result.itemType == "e") {
                                bindHierarchy(hierarchytypeMasterCharacter);
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
                                        hierarchyType: hierarchytypeMasterCharacter
                                    },
                                    success: function () {
                                        bindHierarchy(hierarchytypeMasterCharacter);
                                    },
                                    error: function () {
                                        bindHierarchy(hierarchytypeMasterCharacter);
                                    }
                                });
                            }
                        },
                        onRename: function (result) {
                            $.ajax({
                                url: apiUrl,
                                data: {
                                    request: "rename",
                                    id: result.id,
                                    name: result.name,
                                    hierarchyType: hierarchytypeMasterCharacter
                                },
                                success: function () {
                                    bindHierarchy(hierarchytypeMasterCharacter);
                                },
                                error: function () {
                                    bindHierarchy(hierarchytypeMasterCharacter);
                                }
                            });
                        },
                        onDelete: function (result) {
                            $.ajax({
                                url: apiUrl,
                                data: {
                                    request: "deletefolder",
                                    id: result.id,
                                    name: result.name,
                                    type: result.type,
                                    hierarchyType: hierarchytypeMasterCharacter
                                },
                                success: function () {
                                    bindHierarchy(hierarchytypeMasterCharacter);
                                },
                                error: function () {
                                    bindHierarchy(hierarchytypeMasterCharacter);
                                }
                            });
                        }
                    });
                    $(".pane-folder").AGWhiteLoading(false);
                }

            });
        }
    </script>

</asp:Content>
