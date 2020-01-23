<%@ Page Title="" Language="C#" MasterPageFile="~/MasterConfig/MasterPage/MasterPageConfig.master" AutoEventWireup="true" CodeBehind="MailRecurringDetail.aspx.cs" Inherits="ServiceWeb.MasterConfig.MailRecurringDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderConfig" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-email-recurring").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <nav class="navbar nav-header-action sticky-top bg-white" style="margin-left: 1px; margin-right: 1px;">
        <div class="pull-left">
            <asp:UpdatePanel ID="udpnAction" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <a class="btn btn-warning mb-1" href="MailRecurring.aspx"><i class="fa fa-arrow-circle-left"></i>&nbsp;&nbsp;Back</a>
                    <button type="button" class="btn btn-success mb-1 AUTH_MODIFY" onclick="validateSave();"><i class="fa fa-save"></i>&nbsp;&nbsp;Save</button>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </nav>

    <asp:UpdatePanel runat="server" UpdateMode="Conditional" class="d-none">
        <ContentTemplate>
            <asp:Button ID="btnValidateSave" runat="server" OnClick="btnValidateSubmit_Click" />
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="row">
        <div class="col">            

            <div class="card shadow">
                <div class="card-header">
                    <h5 class="mb-0">E-Mail Recurring</h5>
                </div>
                <div class="card-body">

                    <!-- Inbox -->
                    <div class="card border-primary mb-3">
                        <div class="card-header bg-primary text-white">
                            <b>Inbox</b>
                        </div>
                        <div id="inbox-body" class="card-body">
                            <div class="form-row">
                                <div class="form-group col-md-10">
                                    <label>Objective</label>
                                    <asp:TextBox ID="tbObjective" runat="server" CssClass="form-control form-control-sm required" AutoCompleteType="Disabled"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-group col-md-6">
                                    <label>POP3 Server</label>
                                    <asp:TextBox ID="tbPop3Server" runat="server" CssClass="form-control form-control-sm required" AutoCompleteType="Disabled"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-2">
                                    <label>POP3 Port</label>
                                    <asp:TextBox ID="tbPop3Port" runat="server" CssClass="form-control form-control-sm required" AutoCompleteType="Disabled"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-2">
                                    <label>&nbsp;</label>
                                    <asp:CheckBox ID="chkUseSSL" runat="server" CssClass="form-check mt-1 ml-3" Text="Use SSL" />
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-group col-md-6">
                                    <label>POP3 User</label>
                                    <asp:TextBox ID="tbPop3User" runat="server" CssClass="form-control form-control-sm required" TextMode="Email" AutoCompleteType="Disabled"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-4">
                                    <label>POP3 Password</label>
                                    <asp:TextBox ID="tbPop3Password" runat="server" CssClass="form-control form-control-sm required" type="password" autocomplete="new-password"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Rule -->
                    <div class="card border-info mb-3">
                        <div class="card-header bg-info text-white">
                            <b>Rule</b>
                        </div>
                        <div id="rule-body" class="card-body">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="form-row">
                                        <div class="form-group col-md-1">
                                            <label>Sequence</label>
                                            <asp:TextBox ID="tbSequence" runat="server" CssClass="form-control form-control-sm required" AutoCompleteType="Disabled"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-md-5">
                                            <label>Mail From</label>
                                            <div class="input-group">
                                                <asp:TextBox ID="tbMailFrom" runat="server" CssClass="form-control form-control-sm required" TextMode="Email" AutoCompleteType="Disabled"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <asp:Button ID="btnValidateAddSeq" runat="server" CssClass="btn btn-sm btn-primary" Text="Add" OnClientClick="validateAddSequence();" OnClick="btnValidateSubmit_Click" />
                                                </div>
                                            </div>                                            
                                        </div>
                                    </div>            
                                                            
                                    <asp:Button ID="btnAddSequence" runat="server" CssClass="d-none" OnClick="btnAddSequence_Click" />                                    

                                    <div class="table-responsive">
                                        <table id="table-items" class="table table-bordered table-striped table-hover table-sm dataTable">
                                            <thead>
                                                <tr>
                                                    <th class="text-nowrap"></th>
                                                    <th class="text-nowrap text-center">Sequence</th>
                                                    <th class="text-nowrap">Rule Code</th>
                                                    <th>Mail From</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="rptItems" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td class="text-nowrap text-center">
                                                                <i class="fa fa-times-circle-o fa-lg text-danger c-pointer AUTH_MODIFY" title="Delete" onclick="confirmDeleteSequence(this);"></i>
                                                                <asp:Button ID="btnDeleteSequence" runat="server" CssClass="d-none AUTH_MODIFY" OnClick="btnDeleteSequence_Click" CommandArgument='<%# Eval("BATCH_ID") + "|" + Eval("RULE_CODE") %>' />
                                                            </td>
                                                            <td class="text-nowrap text-center"><%# Eval("SEQ") %></td>
                                                            <td class="text-nowrap"><%# Eval("RULE_CODE") %></td>
                                                            <td><%# Eval("MAIL_FROM") %></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
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
        function activeRequireField(obj) {
            $("#" + obj).find(".required").each(function () {
                $(this).prop('required', true);
            });
        }

        function inactiveRequireField(obj) {
            $("#" + obj).find(".required").each(function () {
                $(this).prop('required', false);
            });
        }

        function validateSave() {
            inactiveRequireField("rule-body");
            activeRequireField("inbox-body");
            $("#<%= btnValidateSave.ClientID %>").click();
        }

        function validateAddSequence() {
            inactiveRequireField("inbox-body");
            activeRequireField("rule-body");
        }

        function saveEmailRecurring() {
            AGLoading(true);
            $("#<%= btnSave.ClientID %>").click();            
        }

        function addSequence() {
            AGLoading(true);
            $("#<%= btnAddSequence.ClientID %>").click();
        }

        function confirmDeleteSequence(sender) {
            if (AGConfirm(sender, "Confirm Delete")) {
                $(sender).next().click();
            }
        }
    </script>

</asp:Content>
