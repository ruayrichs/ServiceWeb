<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/RedesignV2/DefaultMasterPageV2.master" AutoEventWireup="true" CodeBehind="AssetGroup.aspx.cs" Inherits="ServiceWeb.crm.Master.Asset.Config.AssetGroup" %>

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
    <style type="text/css">
        .requireItem {
            color: red;
        }
    </style>
    <script type="text/javascript">
        function validateInput() {
            var start = document.getElementById('<%= popupStart.ClientID %>').value;
            var end = document.getElementById('<%= popupEnd.ClientID %>').value;

            if (start.length != end.length) {
                alertMessage('กรุณาระบุ Start และ End ให้อยู่ในช่วงเดียวกัน');
                return false;
            }

            return true;
        }

        function checkRequireItem() {
            var message = "";
            var count = 0;

            $('.requireItem').each(function () {
                var first = $(this).parent().next().children().first();
                if ($(this).parent().next().children().first().val() == "") {
                    message += 'กรุณาระบุ ' + $(this).prev().html() + '<br/>';
                }
            });

            if (message.length > 0) {
                alertMessage(message);
                return false;
            }

            if (!validateInput()) {
                return false;
            }

            if (confirm("ต้องการบันทึกหรือไม่")) {
                $('.modal-backdrop').remove(); $('body').css('overflow', 'auto'); $('.modal').modal('hide');
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <div>
        <h5>Asset AssetGroup Master 
        </h5>
        <hr />
        <div>
            <div class="row">
                <div class="col-lg-12">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
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
                                                runat="server" OnClick="GridgvData_EditItem" CommandName='<%# Eval("SID")+","+ Eval("CompanyCode")+","+Eval("AssetGroup") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton2" CssClass="fa fa-trash fa-2x AUTH_DELETE" Style="color: Red;"
                                                runat="server" CommandName='<%# Eval("SID")+","+ Eval("CompanyCode")+","+Eval("AssetGroup") %>' OnClick="GridgvData_DeleteItem"
                                                OnClientClick="return confirm('ยืนยันการลบข้อมูล');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="AssetGroup Code" DataField="AssetGroup" ItemStyle-Width="200px" />
                                    <asp:BoundField HeaderText="AssetGroup Name" DataField="Description" />
                                    <asp:BoundField HeaderText="Number RangeCode" DataField="NumberRangeCode" ItemStyle-Width="150px" />
                                    <asp:BoundField HeaderText="Active" DataField="xActive" />
                                    <asp:BoundField HeaderText="Start" DataField="xStart" ItemStyle-Width="150px" />
                                    <asp:BoundField HeaderText="End" DataField="xEnd" ItemStyle-Width="100px" />
                                </Columns>
                                <PagerStyle CssClass="gvpaging" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

            </div>
        </div>
    </div>


    <div class="modal fade formgvData" id="formgvData">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Asset AssetGroup Master</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="udpnPopup" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCreate" />
                            <asp:AsyncPostBackTrigger ControlID="btnEdit" />
                        </Triggers>
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-12 col-md-2">
                                    <label for="input">Asset Group</label><span class="requireItem">*</span>
                                </div>
                                <div class="col-sm-12 col-md-4">
                                    <asp:TextBox ID="txtAssetGroup" CssClass="form-control" ClientIDMode="Static" runat="server" />
                                </div>
                                <div class="col-sm-12 col-md-2">
                                    <label for="input">Asset Group Name</label><span class="requireItem">*</span>
                                </div>
                                <div class="col-sm-12 col-md-4">
                                    <asp:TextBox ID="txtAssetGroupDescription" CssClass="form-control" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-2">
                                    <label for="input">Number Range</label>
                                </div>
                                <div class="col-sm-12 col-md-4">
                                    <asp:TextBox ID="popupNumberRange" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-12 col-md-2">
                                    <label for="input">Prefix</label><span class="requireItem">*</span>
                                </div>
                                <div class="col-sm-12 col-md-4">
                                    <asp:TextBox ID="popupPrefix" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-2">
                                    <label for="input">AssetAccountCode</label>
                                </div>
                                <div class="col-sm-12 col-md-4">
                                    <asp:TextBox ID="txtAssetAccountCode" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-12 col-md-2">
                                    <label for="input">Depreciation AccCode</label>
                                </div>
                                <div class="col-sm-12 col-md-4">
                                    <asp:TextBox ID="txtDepreciationAccCode" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-2">
                                    <label for="input">AccumlateDepre AccCode</label>
                                </div>
                                <div class="col-sm-12 col-md-4">
                                    <asp:TextBox ID="txtAccumlateDepreAccCode" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-12 col-md-2">
                                    <label for="input">AssetClearing AccCode</label>
                                </div>
                                <div class="col-sm-12 col-md-4">
                                    <asp:TextBox ID="txtAssetClearingAccCode" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-2">
                                    <label for="input">Active</label>
                                </div>
                                <div class="col-sm-12 col-md-4">
                                    <asp:CheckBox ID="ChkxActive" runat="server" />
                                </div>
                                <div class="col-sm-12 col-md-2">
                                </div>
                                <div class="col-sm-12 col-md-4">
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-2">
                                    <label for="input">Start</label><span class="requireItem">*</span>
                                </div>
                                <div class="col-sm-12 col-md-4">
                                    <asp:TextBox ID="popupStart" onkeydown="return (!(event.keyCode>=65));" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-12 col-md-2">
                                    <label for="input">End</label><span class="requireItem">*</span>
                                </div>
                                <div class="col-sm-12 col-md-4">
                                    <asp:TextBox ID="popupEnd" onkeydown="return (!(event.keyCode>=65));" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-2">
                                    <label for="input">Suffix</label>
                                </div>
                                <div class="col-sm-12 col-md-4">
                                    <asp:TextBox ID="popupSuffix" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-12 col-md-2">
                                    <label for="input">Year</label><span class="requireItem">*</span>
                                </div>
                                <div class="col-sm-12 col-md-4">
                                    <asp:TextBox ID="popupYear" Text="*" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-2">
                                    <label for="input">ExternalOrNot</label>
                                </div>
                                <div class="col-sm-12 col-md-4">
                                    <asp:CheckBox ID="popupExternal" runat="server" />
                                </div>
                                <div class="col-sm-12 col-md-2">
                                    <label for="input">RequrstLocation</label>
                                </div>
                                <div class="col-sm-12 col-md-4">
                                    <asp:CheckBox ID="popupRequrst" runat="server" />
                                </div>
                            </div>
                            <div>
                                <hr />
                            </div>
                            <div class="row">
                                <div class="col-md-12" style="text-align:right">
                                    <asp:Button ID="btnCreateModal" runat="server" Text="บันทึก" OnClientClick="return checkRequireItem();" OnClick="btnCreateModal_Click" CssClass="btn btn-success AUTH_CREATE" ClientIDMode="Static" />
                                    <asp:Button ID="btnEdit" runat="server" Text="บันทึก" OnClientClick="return checkRequireItem();" OnClick="btnEditModal_Click" CssClass="btn btn-success AUTH_UPDATE" ClientIDMode="Static" />
                                    <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClientClick="$('.formgvData').modal('hide');" CssClass="btn btn-danger" ClientIDMode="Static" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
