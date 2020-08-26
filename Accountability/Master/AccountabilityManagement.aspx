<%@ Page Title="" Language="C#" MasterPageFile="~/Accountability/MasterPage/AccountabilityMaster.master" AutoEventWireup="true"  CodeBehind="AccountabilityManagement.aspx.cs" Inherits="ServiceWeb.Accountability.Master.AccountabilityManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-account").className = "nav-link active";
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

        .img-circle-boxs {
            width: 50px;
            height: 50px;
            border: 1px solid #ccc;
            background-size: 100% auto;
            overflow-x: hidden;
            overflow-y: hidden;
            margin: 0 auto;
        }
    </style>
    <script>
        function addEmpActor(code) {
        }
        function showPanelAddActor() {
            $("#panel-add-actor").slideUp(false);
            $("#panel-add-actor-txt").slideDown(true);
        }
        function hidePanelAddActor() {
            $("#panel-add-actor").slideDown(true);
            $("#panel-add-actor-txt").slideUp(false);
        }
        function showPanelEventObject() {
            $("#panel-eventobject").slideUp(false);
            $("#panel-eventobject-txt").slideDown(true);
        }
        function hidePanelEventObject() {
            $("#panel-eventobject").slideDown(true);
            $("#panel-eventobject-txt").slideUp(false);
        }
        function ConfirmDelete() {
            if (AGConfirm('Do you want to delete?')) {
                AGLoading(true);
                return true;
            } else {
                return false;
            }
        }
    </script>
    <div class="row">
        <div class="col-sm-12 col-md-4">
            <div>
                <div>
                    <span class="page-header">Tree Structure</span>
                </div>
                <div class="mat-box">
                    <div style="min-height: 100px;" id="hierarchy"></div>
                </div>
            </div>
        </div>

        <div class="col-sm-12 col-md-8">
            <div>
                <div>
                    <span class="page-header">Accountability Management</span>
                </div>
                <div class="d-none">
                    <asp:HiddenField runat="server" ID="hddStructureCode" ClientIDMode="Static" />
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button Text="text" ID="btnBindData" ClientIDMode="Static" OnClick="btnBindData_Click" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <%--<h5 style="margin-bottom:0px;" id="naneNode"></h5>--%>
                        <h6 style="margin-bottom: 0px;">
                            <asp:UpdatePanel runat="server" ID="udpTitle" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label Text="" runat="server" ID="lblTitle" CssClass="text-success" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </h6>
                    </div>
                </div>
                <div class="alert alert-warning " id="panelFirstLoad" style="display: block; margin-top: -11px;">
                    <div class="text-center">
                        <i class="fa fa-arrow-circle-left"></i>Select Structure
                    </div>
                </div>
                <div id="panelContent" style="display: none;">
                    <div style="margin-top: -23px;position: absolute;right: 15px;z-index: 10;">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpCheckAutoWorkflow">
                            <ContentTemplate>
                                <asp:CheckBox Text="Is Auto Start Workflow" runat="server" ID="chk_IsAutoWorkflow" 
                                    onchange="$(this).next().click();" />
                                <asp:Button Text="" CssClass="d-none" runat="server" ID="btnUpdateIsAutoWorkflow"
                                    OnClick="btnUpdateIsAutoWorkflow_Click" OnClientClick="AGLoading(true);" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <br />
                    <div class="panel panel-primary">
                        <div class="panel-heading" style="padding: 10px;">
                            <h5 style="margin: 0;">
                                Event Object
                            </h5>
                        </div>
                        <div class="panel-body" style="padding: 10px;">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updEventObject">
                                <ContentTemplate>
                                    <asp:Repeater runat="server" ID="rptEventLevel" OnItemDataBound="rptEventLevel_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:Panel runat="server" ID="panelEventObject">
                                                <table class="table table-striped table-bordered table-ag">
                                                    <tr>
                                                        <td style="width: 260px;">บทบาท
                                                        </td>
                                                        <td>ผู้อนุมัติ
                                                        </td>
                                                        <td>เป้าหมายการเปลี่ยนสถานะ 
                                                        </td>
                                                    </tr>
                                                    <label><b>Event Description :</b> <%# Eval("EventDesc") %></label>
                                                    <asp:Repeater runat="server" ID="rptEventObject" OnItemDataBound="rptEventObject_ItemDataBound">
                                                        <ItemTemplate>
                                                            <tr> 
                                                                <td>
                                                                    <div class="col-lg-8 col-md-7">
                                                                        <%# Eval("HierarchyDesc") %>
                                                                        <asp:HiddenField runat="server" ID="hddParticipantsCodes" Value='<%# Eval("ParticipantsCode") %>' />
                                                                        <asp:HiddenField runat="server" ID="hddHierarchyCode" Value='<%# Eval("HierarchyCode") %>' />
                                                                        <asp:HiddenField runat="server" ID="hddCharacterCode" Value='<%# Eval("CharacterCode") %>' />
                                                                    </div>
                                                                    <div class="col-lg-4 col-md-5 text-right">
                                                                        <span style="color: red; cursor: pointer;" onclick="$(this).next().click();"><i class="fa fa-pencil-square fa-fw"></i>ลบ</span>
                                                                        <asp:Button CssClass="d-none" Text="text" runat="server" OnClientClick="return ConfirmDelete();"
                                                                            CommandArgument='<%# Eval("CompanyStructureCode") + "," + Eval("ParticipantsCode") %>'
                                                                            ID="btnRemoveEventObject" OnClick="btnRemoveEventObject_Click" />
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <table class="table table-striped table-bordered table-ag" style="margin-bottom: 0px;">
                                                                        <asp:Repeater runat="server" ID="rptEventObjectDetail">
                                                                            <ItemTemplate>
                                                                                <tr>
                                                                                    <div class="row">
                                                                                        <div class="col-lg-2 col-md-3">
                                                                                            <div class="img-circle-boxs" style="border-radius: 0; background-image: url('<%# ServiceWeb.Service.UserImageService.getImgProfile(Eval("EmployeeCode").ToString()).Image_128 %>'),url('/images/user.png');background-size: cover;"></div>
                                                                                        </div>
                                                                                        <div class="col-lg-10 col-md-9">
                                                                                            <asp:Label runat="server" Text='<%# Eval("fullname").ToString() != "" ? Eval("fullname") : "ยังไม่ได้มีผู้รับบทบาท" %>'></asp:Label>
                                                                                        </div>
                                                                                    </div>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <p><%# getDescTicketStatus(Eval("TicketStatusCode").ToString()) %></p>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:Repeater>

                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div id="panel-eventobject" onclick="showPanelEventObject();" class="btn-add">เพิ่มผู้อนุมัติ</div>
                                            <div id="panel-eventobject-txt" style="display: none;">
                                                <div style="margin-bottom: 10px;">
                                                    <label>
                                                        Event Object
                                                    </label>
                                                    <asp:DropDownList ID="ddlEventObjec" runat="server" CssClass="form-control">
                                                        <asp:ListItem Text="-เลือกข้อมูล-" Value=""></asp:ListItem>
                                                        <asp:ListItem Text="Submited for IL1 approval" Value="01"></asp:ListItem>
                                                        <asp:ListItem Text="Submited for IL2 approval" Value="12"></asp:ListItem>
                                                        <asp:ListItem Text="Submited for IL3 approval" Value="23"></asp:ListItem>
                                                        <asp:ListItem Text="Submited for IL4 approval" Value="34"></asp:ListItem>
                                                        <asp:ListItem Text="Submited for IL5 approval" Value="45"></asp:ListItem>
                                                        <asp:ListItem Text="Cancel initiative" Value="Cancel"></asp:ListItem>
                                                        <asp:ListItem Text="Revise financial plan" Value="Revise"></asp:ListItem>
                                                        <asp:ListItem Text="Release financial plan" Value="Release"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div style="margin-bottom: 10px;">
                                                    <label>
                                                        Participants Description
                                                    </label>
                                                    <asp:DropDownList ID="ddlParticipantsDescription" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                                <div style="margin-bottom: 10px;">
                                                    <label>
                                                        Character Description
                                                    </label>
                                                    <div class="input-group" style="width: 100%">
                                                        <input type="text" readonly name="name" style="background: #fff;" onclick="openSubProjectHierarchyObject(event); bindHierarchyCharacterObject(this);" id="txtSubProjectDescriptionObject" runat="server" clientidmode="Static" class="form-control blue-require-style" />
                                                        <span style="cursor: pointer;" class="input-group-addon input-group-append" onclick="removeSubProjectSelectedObject();">
                                                            <i class="input-group-text fa fa-remove"></i>
                                                        </span>
                                                        <span style="cursor: pointer;" class="input-group-addon input-group-append" onclick="openSubProjectHierarchyObject(event);bindHierarchyCharacterObject(this);">
                                                            <i class="input-group-text fa fa-list"></i>
                                                        </span>
                                                        <div onclick="event.stopPropagation();" class="pane-subproject-container-object" style="display: none; z-index: 30000; position: absolute; padding: 10px; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.37); background: #fff; left: 0px; top: 100%; padding-top: 0px; width: 100%;">
                                                            <div>
                                                                <i class="fa fa-remove pull-right" style="color: #aaa; margin-top: 10px; cursor: pointer;" onclick="$('.pane-subproject-container-object').hide();"></i>
                                                            </div>
                                                            <div class="pane-subproject-object" style="overflow-y: auto; padding: 10px; max-height: 250px; padding-top: 0px; margin-top: 35px; margin-bottom: 20px">
                                                            </div>
                                                            <div class="text-center pane-subproject-empty-object" style="padding: 10px 20px; border: 1px solid #ccc; margin-top: 35px">
                                                                ไม่พบบทบาท
                                                            </div>
                                                        </div>
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div style="display: none">

                                                                    <asp:TextBox runat="server" ClientIDMode="Static" CssClass="form-control" ID="txtChangeFolderSubProjectObject" meta:resourcekey="txtChangeFolderSubProjectResource1" />
                                                                    <asp:TextBox runat="server" ClientIDMode="Static" CssClass="form-control" ID="txtChangeFolderSubProject" meta:resourcekey="txtChangeFolderSubProjectResource1" />

                                                                    <asp:Button Text="text" ID="btnChangeSubProjectObject" OnClick="btnChangeSubProjectObject_Click"
                                                                        ClientIDMode="Static" runat="server" meta:resourcekey="btnChangeSubProjectResource1" />

                                                                    <asp:DropDownList onchange="subProjectChange();" runat="server" ID="ddlSubProjectObject" ClientIDMode="Static" CssClass="form-control"
                                                                        Width="100%" meta:resourcekey="ddlSubProjectResource1">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <script>
                                                            function openSubProjectHierarchyObject(e) {
                                                                $(".pane-subproject-container-object").fadeIn();
                                                                e.stopPropagation();
                                                            }
                                                            function loading() {
                                                                $(".pane-subproject-container-object").AGWhiteLoading(true);
                                                            }
                                                            function hierarchyLoadingObject() {
                                                                removeLoadingObject();
                                                                $(".pane-subproject-container-object").AGWhiteLoading(true);
                                                            }
                                                            function removeLoadingObject() {
                                                                $(".pane-subproject-container-object").AGWhiteLoading(false);
                                                            }
                                                            function bindHierarchyCharacterObject(evt) {
                                                                var apiUrl = "/Accountability/API/HirearchyStructureAPI.aspx";
                                                                var hierarchytype = $(evt).parent().parent().parent().find("#ddlParticipantsDescription").val()
                                                                console.log(hierarchytype);
                                                                hierarchyLoadingObject();
                                                                $.ajax({
                                                                    url: apiUrl,
                                                                    data: {
                                                                        request: "list",
                                                                        hierarchyType: hierarchytype
                                                                    },
                                                                    success: function (datas) {
                                                                        if (datas != "") {
                                                                            $(".pane-subproject-empty-object").hide();
                                                                        }
                                                                        $(".pane-subproject-object").aGapeTreeMenu({
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
                                                                                if (result.id != "") {
                                                                                    $("#txtChangeFolderSubProjectObject").val(result.id);
                                                                                    $(".pane-subproject-container-object").hide();
                                                                                    $("#btnChangeSubProjectObject").click();
                                                                                }
                                                                            }
                                                                        });
                                                                        removeLoadingObject();
                                                                    }
                                                                });
                                                            }
                                                            function removeSubProjectSelectedObject() {
                                                                if ($("#txtSubProjectDescriptionObject").val() != "ไม่พบบทบาท") {
                                                                    $("#txtSubProjectDescriptionObject").val("เลือกบทบาท");
                                                                }
                                                            }
                                                        </script>
                                                    </div>
                                                </div>
                                                <div style="margin-bottom: 10px;">
                                                    <label>
                                                        Ticket Status
                                                    </label>
                                                    <asp:DropDownList ID="ddlTicketStatus" runat="server" ClientIDMode="Static" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="text-right">
                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Button Text="Save" CssClass="btn btn-primary AUTH_MODIFY" ID="btnEventObject" runat="server" OnClick="btnEventObject_Click" />
                                                            &nbsp;&nbsp;
                                                                <span onclick="hidePanelEventObject();" class="btn btn-warning">Cancle</span>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
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
        var apiUrl = "/Accountability/API/CompanyStructureAPI.aspx";
        function bindHierarchy(rootCount) {
            $("#hierarchy").AGWhiteLoading(true, "Loading data");
            $.ajax({
                url: apiUrl,
                data: {
                    request: "get_hierarchy",
                    WorkGroupCode: "<%= WorkGroupCode %>"
                },
                success: function (datas) {
                    $("#hierarchy").aGapeTreeMenu({
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
                            $("#hddStructureCode").val(result.id);
                            $("#btnBindData").click();
                            AGLoading(true);
                        }
                    });
                    $("#hierarchy").AGWhiteLoading(false);
                }
            });
        }
        function hierarchyDoAjax(datas) {
            $("#hierarchy").AGWhiteLoading(true, "Loading data");
            $.ajax({
                url: apiUrl,
                data: datas,
                success: function () {
                    bindHierarchy();

                },
                error: function () {
                    bindHierarchy();
                }
            });
        }
        bindHierarchy();

    </script>
</asp:Content>
