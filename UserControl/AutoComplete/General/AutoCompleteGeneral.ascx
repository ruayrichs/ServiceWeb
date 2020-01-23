<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutoCompleteGeneral.ascx.cs" Inherits="ServiceWeb.UserControl.AutoComplete.General.AutoCompleteGeneral" %>


<div class="autocomplete-box panel-autocomplete-<%= ClientID %>">
<asp:TextBox ID="tbDescription" CssClass="Description" runat="server" autocomplete="off"></asp:TextBox>
<asp:TextBox ID="hdfCode" CssClass="Code" style="display:none;" runat="server" />
<asp:TextBox ID="hdfName" CssClass="Name" style="display:none;" runat="server" />

<asp:TextBox ID="hdfActionCase" CssClass="ActionCase" style="display:none;" runat="server" />
<asp:TextBox ID="hdfNotAutoBindComplete" CssClass="NotAutoBindComplete" style="display:none;" runat="server" />
<asp:TextBox ID="hdfAfterSelectedChange" CssClass="AfterSelectedChange" style="display:none;" runat="server" />
</div>
<%--<script src="-/UserControl/AutoComplete/General/autocomplete-ganeral.js"></script>--%>
