<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TierGroupMasterControl.ascx.cs" Inherits="ServiceWeb.crm.Master.mastertier.TierGroupMasterControl" %>
<link href="<%= Page.ResolveUrl("~/Lib-tablemodel/FeasibilityFinancialProjection.css") %>" rel="stylesheet" />
<style>
    .table-finan > tbody > tr > td input[type=text] , .table-finan > tr > td input[type=text] {
       border: none;
    }
    .table-finan > tbody > tr > td.text {
        padding:0px;
    }
</style>
<script>
    function IsValideSaveTierGroupMasterControl(obj)
    {
        var resource = $("#txtTierGroupDescription").val();
        if (resource.split(" ").join('') == "")
        {
            AGMessage("TierGroup Description ไม่เป็นค่าว่าง! ");
            return;
        }

        if (AGConfirm(obj, "ยืนยันการ
            
            "))
        {
            $(obj).next().click();
        }
    }

    function IsValideUpdateGroupMasterControl(obj)
    {
        var objThis = $(obj).closest('.TierGroupMasterControl');
        var updateTierGroup = $(objThis).find('.text-update-TierGroup');
        if (updateTierGroup.length <= 0)
        {
            AGMessage("ยังไม่มีการแก้ไขข้อมูล!");
            return;
        }
        if (AGConfirm(obj, "ยืนยันการแก้ไข"))
        {
            var jArr = [];
            for (var i = 0; i < updateTierGroup.length; i++) {
                var jObj = {};
                jObj.TierGroupCode = $(updateTierGroup[i]).attr('data-TierGroupCode');
                jObj.TierGroupDescription = $(updateTierGroup[i]).val();
                jArr.push(jObj);
            }
            $("#txtTierGroupMasterControlResource").val(JSON.stringify(jArr));
            $(obj).next().click();
        }
    }
</script>
<div class="TierGroupMasterControl">
    <asp:TextBox ID="txtTierGroupMasterControlResource" ClientIDMode="Static" style="display:none;" runat="server" />
<div class="row">
    <div class="col-md-4">
       <label> TierGroup Description</label>
        <asp:TextBox ID="txtTierGroupDescription" ClientIDMode="Static" CssClass="form-control" runat="server" />
    </div>
    <div class="col-md-4">
        <br />
        <asp:UpdatePanel ID="udpButonForTierGroupMaster" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <span class="btn btn-info" onclick="$(this).next().click();"><i class="fa fa-search"></i>&nbsp;Search</span>
                <asp:Button ID="btnSearchForTierGroup" OnClick="btnSearchForTierGroup_Click" OnClientClick="AGLoading(true);" style="display:none;" Text="text" runat="server" />
                 
                 <span class="btn btn-success" onclick="IsValideSaveTierGroupMasterControl(this);"><i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Save</span>
                 <asp:Button ID="btnSaveForTierGroupMater" OnClick="btnSaveForTierGroupMater_Click" OnClientClick="AGLoading(true);" style="display:none;" Text="text" runat="server" />
                 
                <span id="btnUpdateForTierGroupMater" class="btn btn-warning" onclick="IsValideUpdateGroupMasterControl(this);"><i class="fa fa-refresh" aria-hidden="true"></i>&nbsp;Edit</span>
                <asp:Button ID="btnUpdateForTierGroupMaterServer" OnClick="btnUpdateForTierGroupMaterServer_Click" OnClientClick="AGLoading(true);" style="display:none;" Text="Update" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<div class="row">
    <div class="col-md-12">
        <asp:UpdatePanel ID="udpTierData" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Repeater ID="rptTierData" runat="server">
                    <HeaderTemplate>
                        <table class="table-finan">
                            <tr>
                                <th></th>
                                <th>Tier Name</th>
                                <th>Created By</th>
                                <th>Created On</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <th></th>
                            <td class="text">
                               <input  type="text" data-TierGroupCode="<%# Eval("TierGroupCode") %>" onchange="$(this).addClass('text-update-TierGroup');"  value='<%# Eval("TierGroupDescription") %>'  />
                            </td>
                            <td><%# Eval("name")+" "+ Eval("LastName_TH") %></td>
                            <td><%# Eval("Create_ON_Des") %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                         </table>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>

 </div>
