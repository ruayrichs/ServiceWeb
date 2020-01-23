<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeSelect.ascx.cs" Inherits="POSWeb.crm.usercontrol.EmployeeSelect" %>

<link href="<%= Page.ResolveUrl("~/bootstrap-select/bootstrap-select.css") %>" rel="stylesheet" />
<script src="<%= Page.ResolveUrl("~/bootstrap-select/bootstrap-select.js?vs=20190113") %>"></script>
<style>
    .employee-select .btn.dropdown-toggle.btn-default{
        color:#333 !important;
        background:#FFFFFF !important;
        box-shadow:none !important;
        border: 1px solid #ccc !important;
    }
    .employee-select .btn.dropdown-toggle.btn-default:hover, 
    .employee-select .btn.dropdown-toggle.btn-default:focus,
    .employee-select .btn.dropdown-toggle.btn-default:active,
    .employee-select .btn-group.open .btn-default.dropdown-toggle{
        background:#f7f7f7  !important;
    }
    .employee-select .filter-option .not-show{
        display:none;
    }
</style>

<script>
    function setEmployeeSelect<%= ClientID %>(val) {
        $('.employee-select-<%= ClientID %> option').each(function () {
            if (val.match(this.value)) {
                $(this).attr("selected", true);
            }
        });
        $(".txtSelectedEmployeeValue").val(val);
        $('.employee-select-<%= ClientID %>').selectpicker('refresh');
    }
    function EmployeeSelect<%= ClientID %>() {
        $('.employee-select-<%= ClientID %>').on('change', function () {
            var val = $(this).val();
            $(".selected-employee-<%= ClientID %> .txtSelectedEmployeeValue").val(val);

            var text = $(this).find("option:selected").text();
            $(".selected-employee-<%= ClientID %> .txtSelectedText").val("");
            $(".selected-employee-<%= ClientID %> .txtSelectedText").val(text);
        }).selectpicker();
        $(".selected-employee-<%= ClientID %>").change();
    }

</script>
<div class="selected-employee-<%= ClientID %> hide">
    <asp:TextBox runat="server" ID="txtSelectedEmployeeValue" ClientIDMode="Static" CssClass="txtSelectedEmployeeValue" />
    <asp:TextBox runat="server" ID="txtSelectedText" ClientIDMode="Static" CssClass="txtSelectedText"/>
</div>

<select class="selectpicker employee-select employee-select-<%= ClientID %> form-control" 
    data-live-search="true" style="background-color:rgba(247, 247, 247, 0.00);"
    data-none-selected-text=" " >
    <asp:Repeater runat="server" id="rptEmployees">
        <ItemTemplate>
            <option 
                <%# txtSelectedEmployeeValue.Text.Contains(Eval("EmployeeCode").ToString()) ? "selected" : "" %>
                data-content="<div class='row'><span class='not-show'><div class='col-sm-4'> <%# Eval("EmployeeCode")+ "  </div></span><div class='col-sm-8'> "+ Eval("FirstName_TH") +" "+ Eval("LastName_TH") %> </div></div>"
                value="<%# Eval("EmployeeCode") %>"><%# Eval("FirstName_TH") +" "+ Eval("LastName_TH") %></option>
        </ItemTemplate>
    </asp:Repeater>
</select>