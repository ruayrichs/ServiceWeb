<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/RedesignV2/DefaultMasterPageV2.master" AutoEventWireup="true" CodeBehind="TierGroupMaster.aspx.cs" Inherits="ServiceWeb.crm.Master.mastertier.TierGroupMaster" %>

<%@ Register Src="~/crm/Master/mastertier/TierGroupMasterControl.ascx" TagPrefix="uc1" TagName="TierGroupMasterControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-primary">
        <div class="panel-heading">
            TierGroup Master
        </div>
        <div class="panel-body">
            <uc1:TierGroupMasterControl runat="server" id="TierGroupMasterControl" />
        </div>
    </div>
</asp:Content>
