<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CRMMasterPage.master" AutoEventWireup="true" CodeBehind="AssetStructure.aspx.cs" Inherits="ServiceWeb.crm.Master.Asset.AssetStructure" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/crm/Master/Asset/Lib/agape-tree-menu.css?vs=1.1" rel="stylesheet" />
    <script src="/crm/Master/Asset/Lib/agape-tree-menu.js?vs=1.1"></script>

    <style>
        .box-material{
            border-left: 6px solid #0088cc;
            box-shadow: 0 1px 2px rgba(0,0,0,0.15);
            -webkit-transition: all .6s cubic-bezier(0.165, 0.84, 0.44, 1);
            transition: all .6s cubic-bezier(0.165, 0.84, 0.44, 1);
        }
        .box-material:hover, .box-material:active {
            cursor:pointer;
            box-shadow: 0 5px 15px rgba(0,0,0,0.3);
            -webkit-transform: scale(1.05, 1.05);
            transform: scale(1.05, 1.05);
        }
        element.style {
    height: calc(100vh - 160px);
    overflow-x: hidden;
    overflow-y: scroll;
}

.mat-box {
    border-radius: 5px;
    border: 1px solid;
    border-color: #e5e6e9 #dfe0e4 #d0d1d5;
    background: #fff;
    padding: 15px;
    margin-bottom: 10px;
}
.mat-box {
    border-radius: 5px;
    border: 1px solid;
    border-color: #e5e6e9 #dfe0e4 #d0d1d5;
    background: #fff;
    padding: 15px;
    margin-bottom: 10px;
    </style>
    <div>
        <h5>
            Asset Structure Config
        </h5>
    </div>
    <div class="row">
        <div class="col-sm-12 col-md-8">
            <div  class="mat-box">
                <div style="min-height: 100px;" id="hierarchy"></div>
            </div>
        </div>
        <div class="col-sm-12 col-md-4">
            <div style="margin-bottom: 10px;">
                <div class="input-group">
                    <input type="text" class="form-control" placeholder="Input for search ..."
                        onkeyup="SearchDataInSetControlFilter();" id="txtSearch" >
                    <span class="input-group-text fa fa-search" style="padding-top:13px;">
                    </span>
                </div>
            </div>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpListMat">
                <ContentTemplate>
                    <div class="mat-box" id="panel-list-asset"
                        style="height: calc(100vh - 160px);overflow-x: hidden;overflow-y: scroll;">
                        <asp:Repeater runat="server" ID="rptListMat">
                            <ItemTemplate>
                                <div class="mat-box box-material <%= this.AUTH_CREATE %> " id="<%# Eval("AssetCode")%>" 
                                    draggable="true" ondragstart="dragStart(event);" ondragend="dragEnd();" 
                                    data-AssetSubCode="<%# Eval("AssetSubCode")%>">
                                    <span><%# Eval("AssetCode")%></span>
                                    <br />
                                    <span><%# Eval("AssetSubCodeDescription")%></span>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <% if (rptListMat.Items.Count == 0)
                           { %>
                        <div class="alert alert-info">
                            No data.
                        </div>       
                        <% } %>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button Text="" runat="server" ID="btnAddMatStructure" CssClass="hide"
                        OnClick="btnAddMatStructure_Click" OnClientClick="AGLoading(true);" />
                    <asp:Button Text="" runat="server" ID="btnRebindListAsset" CssClass="hide" 
                        OnClick="btnRebindListAsset_Click" OnClientClick="AGLoading(true);" />
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <asp:HiddenField runat="server" ID="hddAssetParentCode" />
            <asp:HiddenField runat="server" ID="hddAssetParentNoteLevel" />
            <asp:HiddenField runat="server" ID="hddAssetCode" />
            <asp:HiddenField runat="server" ID="hddAssetSubCode" />
        </div>
    </div>
    <script>
        function SearchDataInSetControlFilter() {
            var input, filter, panelOption, classList, keyWord1, keyWord2, i;
            input = document.getElementById('txtSearch');
            filter = input.value.toUpperCase();
            panelOption = document.getElementById("panel-list-asset");
            classList = panelOption.getElementsByClassName('box-material');
            for (i = 0; i < classList.length; i++) {
                keyWord1 = classList[i].getElementsByTagName('span')[0];
                keyWord2 = classList[i].getElementsByTagName('span')[1];
                if (keyWord1 || keyWord2) {
                    if (keyWord1.innerHTML.toUpperCase().indexOf(filter) > -1 ||
                        keyWord2.innerHTML.toUpperCase().indexOf(filter) > -1) {
                        classList[i].style.display = '';
                    } else {
                        classList[i].style.display = 'none';
                    }
                }
            }
        }

        function allowDrop(ev, obj) {
            ev.preventDefault();

            $(".agape-show-node-name").css({
                "color": "#000",
                "border": "none",
                "zoom": "1",
                "border-radius": "0px",
                "padding": "0px",
            });
            $(obj).css({
                "color": "#0088cc",
                "border": "1px solid #0088cc",
                "zoom": "1.1",
                "border-radius": "3px",
                "padding": "1px",
            });

        }

        function dragStart(ev) {
            ev.dataTransfer.setData("text", ev.target.id);
        }
        function dragEnd() {
            $(".agape-show-node-name").css({
                "color": "#000",
                "border": "none",
                "zoom": "1",
                "border-radius": "0px",
                "padding": "0px",
            });
        }

        function drop(ev, obj) {
            ev.preventDefault();
            var data = ev.dataTransfer.getData("text");
            var objectItem = document.getElementById(data);

            $("#<%= hddAssetParentCode.ClientID%>").val($(obj).attr("dataParentID"));
            $("#<%= hddAssetParentNoteLevel.ClientID%>").val($(obj).attr("NodeLevel"));
            $("#<%= hddAssetCode.ClientID%>").val(data);
            $("#<%= hddAssetSubCode.ClientID%>").val($(objectItem).attr("data-AssetSubCode"));
            $("#<%= btnAddMatStructure.ClientID%>").click();
        }

        var apiUrl = "/crm/Master/Asset/API/AssetStructureAPI.aspx";
        function bindHierarchy(IsHighlight) {
            $("#hierarchy").AGWhiteLoading(true, "กำลังดึงข้อมูล");
            $.ajax({
                url: apiUrl,
                data: {
                    request: "get_hierarchy"
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
                        onDelete: true,
                        IsHighlight: IsHighlight,
                        onClick: function (result) {

                        },
                        onDelete: function (result) {
                            hierarchyDoAjax({
                                request: "delete",
                                id: result.id
                            });
                            $("#<%= btnRebindListAsset.ClientID%>").click();
                        }
                    });
                    $("#hierarchy").AGWhiteLoading(false);
                }
            });
        }
        function hierarchyDoAjax(datas) {
            $("#hierarchy").AGWhiteLoading(true, "กำลังดึงข้อมูล");
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
