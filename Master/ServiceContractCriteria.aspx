<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="ServiceContractCriteria.aspx.cs" Inherits="ServiceWeb.Master.ServiceContractCriteria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-service-contract").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <div class="row">
        <div class="col">
            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">Service Contract</h5>
                </div>
                <div class="card-body PANEL-DEFAULT-BUTTON">   
                    <div class="form-row">
                        <div class="form-group col-lg-6">
                            <label>Company</label>
                            <asp:TextBox ID="tbCompany" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                        </div>
                    </div> 

                    <div class="form-row">
                        <div class="col-sm-6">                                                         
                            <div class="form-row">
                                <div class="form-group col-lg-8">
                                    <label>Document Type</label>
                                    <asp:DropDownList ID="ddlDocumentType" runat="server" CssClass="form-control form-control-sm required"
                                        DataValueField="DocumentTypeCode"
                                        DataTextField="xDisplay"></asp:DropDownList>                                    
                                </div>
                                <div class="form-group col-lg-4">
                                    <label>Document Year</label>
                                    <asp:TextBox ID="tbFiscalYear" runat="server" CssClass="form-control form-control-sm required" MaxLength="4" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>                            
                            <div class="form-group">
                                <label>Client</label>
                                <asp:TextBox ID="tbCustomer" runat="server" CssClass="form-control form-control-sm required" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdfCustomerCode" runat="server" />
                                <asp:HiddenField ID="hdfCustomerName" runat="server" />
                            </div>                                                             
                        </div>
                        <div class="col-sm-6">        
                            <div class="form-row">
                                <div class="form-group col-md-6">
                                    <label>Start Date</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="tbStartDate" runat="server" CssClass="form-control form-control-sm date-picker"></asp:TextBox>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-6">
                                    <label>End Date</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="tbEndDate" runat="server" CssClass="form-control form-control-sm date-picker"></asp:TextBox>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label>Document Status</label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control form-control-sm"
                                    DataValueField="STATUSCODE"
                                    DataTextField="xDisplay"></asp:DropDownList>
                            </div>
                        </div>
                    </div>     
                                        
                    <button type="button" class="btn btn-info DEFAULT-BUTTON-CLICK" onclick="searchClick();"><i class="fa fa-search"></i>&nbsp;&nbsp;Search</button>
                    <button type="button" class="btn btn-success AUTH_MODIFY" onclick="createClick();"><i class="fa fa-file-o"></i>&nbsp;&nbsp;Create</button>                  
                    
                    <div class="d-none">
                        <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
                                <asp:Button CssClass="AUTH_MODIFY" ID="btnCreate" runat="server" OnClick="btnCreate_Click" />

                                <asp:HiddenField ID="hddKeyEdit" runat="server" />
                                <asp:Button ID="btnEditServiceContact" OnClick="btnEditServiceContact_Click" OnClientClick="inactiveRequireField();" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>                    
                                       
                    <div id="panel-items" style="display: none;">
                        <hr />                        
                        <asp:UpdatePanel ID="udpnItems" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="tableItems" class="table table-bordered table-striped table-hover table-sm">
                                    <thead>
                                        <tr>
                                            <th></th>
                                            <th class="text-nowrap">Document Type</th>
                                            <th class="text-nowrap">Document No.</th>
                                            <th class="text-nowrap">Year</th>
                                            <th class="text-nowrap">Client</th>
                                            <th class="text-nowrap">Start Date</th>
                                            <th class="text-nowrap">End Date</th>
                                            <th class="text-nowrap">Status</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptItems" runat="server">
                                            <ItemTemplate>
                                                <tr class="c-pointer" data-key="<%# Eval("DocumentType") + "|" + Eval("Fiscalyear") + "|" + Eval("ContractNo") + "|" + Eval("CustomerCode") %>"
                                                    onclick="linktoDetailForEdit(this);">
                                                    <td class="text-nowrap text-center">
                                                        <i class="fa fa-pencil-square fa-lg" title="Edit" onclick=""></i>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("DocumentType") + " : " + Eval("DocumentTypeDesc") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("ContractNo") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("Fiscalyear") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("CustomerCode") + " : " + Eval("CustomerName") %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("StartDate").ToString()) %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Agape.FocusOne.Utilities.Validation.Convert2DateDisplay(Eval("EndDate").ToString()) %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <%# Eval("DocStatus") + " : " + Eval("DocStatusDesc") %>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>                       
                    </div>                                                                            
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var tbCustomer = $("#<%= tbCustomer.ClientID %>");
        var hdfCustomerCode = $("#<%= hdfCustomerCode.ClientID %>");
        var hdfCustomerName = $("#<%= hdfCustomerName.ClientID %>");

        $(document).ready(function () {
            var postData = {
                actionCase: "customer"
            };

            var DB;

            $.ajax({
                type: "POST",
                url: servictWebDomainName + "API/AutoCompleteAPI.aspx",
                data: postData,
                success: function (data) {
                    DB = new JQL(data);

                    $(tbCustomer).typeahead({
                        hint: true,
                        highlight: true,
                        minLength: 0
                    }, {
                        limit: 20,
                        templates: {
                            empty: ' ',
                            suggestion: function (data) {                               
                                return '<div>' + data.display + '</div>';
                            }
                        },
                        source: function (str, callback) {                            
                            var selectCode = DB.select('*').where('code').match(str).fetch();
                            var selectName = DB.select('*').where('desc').match(str).fetch();
                            var selectDisplay = DB.select('*').where('display').match(str).fetch();
                            var selectResult = [];

                            if (selectCode.length > 0) {
                                for (var i = 0; i < selectCode.length; i++) {
                                    selectResult.push(selectCode[i]);
                                }
                            }

                            if (selectName.length > 0) {
                                for (var i = 0; i < selectName.length; i++) {
                                    var hasCode = jQuery.grep(selectResult, function (element) {
                                        return element.code == selectName[i].code;
                                    });

                                    if (hasCode.length == 0) {
                                        selectResult.push(selectName[i]);
                                    }
                                }
                            }

                            if (selectDisplay.length > 0) {
                                for (var i = 0; i < selectDisplay.length; i++) {
                                    var hasCode = jQuery.grep(selectResult, function (element) {
                                        return element.code == selectDisplay[i].code;
                                    });

                                    if (hasCode.length == 0) {
                                        selectResult.push(selectDisplay[i]);
                                    }
                                }
                            }

                            callback(selectResult);
                        },
                        display: function (data) {
                            return data.display;
                        }
                    });

                    $(tbCustomer).bind('typeahead:select typeahead:autocomplete', function (e, v) {
                        $(hdfCustomerCode).val(v.code);
                        $(hdfCustomerName).val(v.desc);
                    });                    
                }
            });
        });

        function activeRequireField() {
            $(".required").prop('required', true);
        }

        function inactiveRequireField() {
            $(".required").prop('required', false);
        }

        function searchClick() {
            inactiveRequireField();
            AGLoading(true);
            $("#<%= btnSearch.ClientID %>").click();
        }

        function afterSearch() {
            $("#panel-items").show();
            $("#tableItems").dataTable({
                columnDefs: [{
                    orderData: [
                        [1, 'asc'],
                        [2, 'desc']
                    ]
                }, {
                    "orderable": false,
                    "targets": [0]
                }]
            });
        }

        function createClick() {
            activeRequireField();
            $("#<%= btnCreate.ClientID %>").click();
        }

        function linktoDetailForEdit(obj)
        {
            $("#<%= hddKeyEdit.ClientID %>").val($(obj).data("key"));
            $("#<%= btnEditServiceContact.ClientID %>").click();
        }

    </script>

</asp:Content>
