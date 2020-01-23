<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerSelect.ascx.cs" Inherits="ServiceWeb.UserControl.CustomerSelect" %>

<style>
    .customer-select .btn.dropdown-toggle.btn-default{
        color:#333 !important;
        background:#FFFFFF !important;
        box-shadow:none !important;
        border: 1px solid #ccc !important;
        border-radius: 5px !important;
        font-weight: 100;
        color: blue !important;
    }
    .customer-select .btn.dropdown-toggle.btn-default:hover, 
    .customer-select .btn.dropdown-toggle.btn-default:focus,
    .customer-select .btn.dropdown-toggle.btn-default:active,
    .customer-select .btn-group.open .btn-default.dropdown-toggle{
        background:#f7f7f7  !important;
    }
    .customer-select .filter-option .not-show{
        display:none;
    }
    .bs-caret {
        margin-left: 5px !important;
    }
    .caret {
        position: initial !important;
        color: blue !important;
    }
    .dropdown-menu.open {
        width: 100% !important;
    }
</style>
<script>
    function setCustomerSelect<%= ClientID %>(val) {
        $('.customer-select-<%= ClientID %> option').each(function () {
            if (val.match(this.value)) {
                $(this).attr("selected", true);
            }
        });
        $(".txtSelectedCustomerValue").val(val);
        $('.customer-select-<%= ClientID %>').selectpicker('refresh');
    }
    function CustomerSelect<%= ClientID %>() {
        $('.customer-select-<%= ClientID %>').on('change', function () {
            var val = $(this).val();
            $(".selected-customer-<%= ClientID %> .txtSelectedCustomerValue").val(val);
            if (typeof clickContact == "function") {
                clickContact(); //it exists, call it
            }
            var text = $(this).find("option:selected").text();
            $(".selected-customer-<%= ClientID %> .txtSelectedCustomerText").val("");
            $(".selected-customer-<%= ClientID %> .txtSelectedCustomerText").val(text);
                
        }).selectpicker();
        $(".selected-customer-<%= ClientID %>").change();
    }

</script>
<div class="selected-customer-<%= ClientID %> hide">
    <asp:TextBox Runat="server" ID="txtSelectedValue"  CssClass="txtSelectedCustomerValue" />
    <asp:TextBox Runat="server" ID="txtSelectedText"  CssClass="txtSelectedCustomerText"/>
</div>
<select class="form-control selectpicker customer-select customer-select-<%= ClientID %>" data-live-search="true">
    <asp:Repeater Runat="server" ID="rptCustomers">
        <ItemTemplate>
            <option 
                <%# txtSelectedValue.Text.Contains(Eval("CustomerCode").ToString()) ? "selected" : "" %>
                data-content="<div class='row'><span class='not-show'></span><div class='col-sm-12 col-md-12' ><%# Eval("CustomerName") %></div></div>"
                value="<%# Eval("CustomerCode") %>"><%# Eval("CustomerName") %></option>
        </ItemTemplate>
    </asp:Repeater>
</select>