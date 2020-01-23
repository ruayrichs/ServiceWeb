<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/RedesignV2/DefaultMasterPageV2.master" AutoEventWireup="true" CodeBehind="AssetCategory1.aspx.cs" Inherits="ServiceWeb.crm.Master.Asset.Config.AssetCategory1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .gvpaging a {
            margin: 2px 2px;
            border-radius: 50%;
            background-color: #444;
            padding: 5px 10px 5px 10px;
            color: #fff;
            text-decoration: none;
            -o-box-shadow: 1px 1px 1px #111;
            -moz-box-shadow: 1px 1px 1px #111;
            -webkit-box-shadow: 1px 1px 1px #111;
            box-shadow: 1px 1px 1px #111;
        }

            .gvpaging a:hover {
                background-color: #337AB7;
                color: #fff;
            }

        .gvpaging span {
            background-color: #337AB7;
            color: #fff;
            -o-box-shadow: 1px 1px 1px #111;
            -moz-box-shadow: 1px 1px 1px #111;
            -webkit-box-shadow: 1px 1px 1px #111;
            box-shadow: 1px 1px 1px #111;
            border-radius: 50%;
            padding: 5px 10px 5px 10px;
        }
    </style>
    
    <h5>
        Asset Category 1 
    </h5>
    <div>
        <hr />
    </div>
    <div>
        <div class="row">
            <div class="col-lg-2">
                <asp:Label runat="server" ID="lblAssetCategory" Text="AssetCategory"></asp:Label>
            </div>
            <div class="col-lg-4">
                <asp:TextBox ID="txtAssetCategory" CssClass="form-control" runat="server" />
            </div>
            <div class="col-lg-2">
                <asp:Label ID="lblDescription" Text="Description" runat="server" />
            </div>
            <div class="col-lg-4">
                <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server" />
            </div>
        </div>
        <div class="row">
            <div class="col-lg-2">
                <asp:Label ID="lblAssetGroup" Text="AssetGroup" runat="server" />
            </div>
            <div class="col-lg-4">
                <asp:DropDownList ID="ddlAssetGroup" CssClass="form-control" ClientIDMode="Static" runat="server" DataTextField="Description" DataValueField="AssetGroup">
                </asp:DropDownList>
            </div>
            <div class="col-lg-2">

                <asp:Label ID="lblAssetType" Text="AssetType" runat="server" />
            </div>
            <div class="col-lg-4">
                <asp:DropDownList ID="ddlAssetType" CssClass="form-control" ClientIDMode="Static" runat="server" DataTextField="GroupName" DataValueField="GroupCode">
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnSreach" Text="Search" CssClass="btn btn-primary" OnClick="btnSreach_Click" runat="server" />
                        <asp:Button ID="btnCreate" Text="Create" CssClass="btn btn-success AUTH_CREATE" OnClick="btnCreate_Click" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-lg-12">
                <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView border="0" ID="gvData" runat="server" CssClass="table table-striped table-hover table-bordered" PagerStyle-CssClass="gvpaging"
                            AllowPaging="true" PageSize="10" AutoGenerateColumns="false" AlternatingRowStyle-VerticalAlign="NotSet"
                            EmptyDataText="No data to display" OnPageIndexChanging="gvData_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" CssClass="fa fa-pencil-square-o fa-2x AUTH_UPDATE" Style="color: #222222;"
                                            runat="server" OnClick="GridgvData_EditItem" CommandName='<%# Eval("AssetCategory")+","+ Eval("AssetGroup")+","+Eval("AssetType") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton2" CssClass="fa fa-trash fa-2x AUTH_DELETE" Style="color: Red;"
                                            runat="server" CommandName='<%# Eval("AssetCategory")+","+ Eval("AssetGroup")+","+Eval("AssetType") %>' OnClick="GridgvData_DeleteItem"
                                            OnClientClick="return confirm('ยืนยันการลบข้อมูล');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Asset Category" DataField="AssetCategory" />
                                <asp:BoundField HeaderText="Description" DataField="Description" />
                                <asp:BoundField HeaderText="Asset Group" DataField="AssetGroupName" />
                                <asp:BoundField HeaderText="Asset Type" DataField="AssetTypeName" />
                            </Columns>
                            <PagerStyle CssClass="gvpaging" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>


    <div class="modal fade" id="master-data">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:UpdatePanel ID="updatePanelModol" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                                &times;</button>
                            <h4 class="modal-title">Asset Category 1  </h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <label>
                                            <asp:Label runat="server" ID="label13" Text="Asset Category"></asp:Label>
                                        </label>
                                        <span class="requireItem">*</span>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-12">
                                        <asp:TextBox ID="txtAssetCategoryModal" CssClass="form-control interger-controlset" ClientIDMode="Static" runat="server" />

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-12">
                                        <label>
                                            <asp:Label runat="server" ID="label14" Text="Description"></asp:Label>
                                        </label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-12">
                                        <asp:TextBox ID="txtDescriptionModal" CssClass="form-control interger-control" ClientIDMode="Static" runat="server" />

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-12">
                                        <label>
                                            <asp:Label runat="server" ID="label1" Text="AssetGroup"></asp:Label>
                                        </label>
                                        <span class="requireItem">*</span>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-12">
                                        <asp:DropDownList ID="ddlAssetGroupModal" CssClass="form-control" ClientIDMode="Static" runat="server" DataTextField="Description" DataValueField="AssetGroup">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-12">
                                        <label>
                                            <asp:Label runat="server" ID="label2" Text=" AssetType "></asp:Label>
                                        </label>
                                        <span class="requireItem">*</span>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-12">
                                        <asp:DropDownList ID="ddlAssetTypeModal" CssClass="form-control" ClientIDMode="Static" runat="server" DataTextField="GroupName" DataValueField="GroupCode">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-default" data-dismiss="modal">
                                    ปิด</button>
                                <asp:Button Text="สร้าง" runat="server" ID="btnCreateAssetCategory1" CssClass="btn btn-success"
                                    OnClick="btnCreateAssetCategory1_Click" OnClientClick="return AGConfirm('ยืนยันการบันทึก');" />
                                <asp:Button Text="แก้ไข" runat="server" ID="btnEditAssetCategory1" CssClass="btn btn-success" OnClick="btnEditAssetCategory1_Click"
                                    Visible="false" OnClientClick="return AGConfirm('ยืนยันการบันทึก');" />
                            </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
