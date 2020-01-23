<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/IncludeLibraryMaster.Master" AutoEventWireup="true" CodeBehind="CustomerForm.aspx.cs" Inherits="ServiceWeb.crm.AfterSale.CustomerForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="IncludeLibraryPlaceHolder" runat="server">

    <div class="container">
        <div class="ml-sm-auto py-3 px-3">
            <div class="row">
                <div class="col">
                    <div class="card">
                        <div class="card-header">
                            <b>Ticket Service Reply</b>
                        </div>
                        <div class="card-body">
                            <div id="panel-information" style="display: none;">
                                <asp:Label ID="lbMessage" runat="server" Font-Bold="true"></asp:Label>
                            </div>
                            <div id="panel-create">
                                <div class="form-group row">
                                    <label class="col-sm-2 col-lg-1 col-form-label-sm">Call ID</label>
                                    <div class="col-sm-10 col-lg-9">
                                        <asp:TextBox ID="tbSeq" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-2 col-lg-1 col-form-label-sm">Client</label>
                                    <div class="col-sm-10 col-lg-9">
                                        <asp:TextBox ID="tbCustomer" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-2 col-lg-1 col-form-label-sm">E-Mail</label>
                                    <div class="col-sm-10 col-lg-9">
                                        <asp:TextBox ID="tbEmail" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-2 col-lg-1 col-form-label-sm">Subject</label>
                                    <div class="col-sm-10 col-lg-9">
                                        <asp:TextBox ID="tbSubject" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-2 col-lg-1 col-form-label-sm">Details</label>
                                    <div class="col-sm-10 col-lg-9">
                                        <asp:TextBox ID="tbDetail" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Rows="4" Style="resize: none;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-2 col-lg-1 col-form-label-sm">Product</label>
                                    <div class="col-sm-10 col-lg-9">
                                        <asp:DropDownList ID="ddlEquipment" runat="server" CssClass="form-control form-control-sm">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-2 col-lg-1 col-form-label-sm"></label>
                                    <div class="col-sm-10 col-lg-9">
                                        <button type="button" class="btn btn-primary" onclick="confirmSubmitForm(this);">Submit</button>
                                        <asp:Button ID="btnSave" runat="server" CssClass="d-none" OnClick="btnSave_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function confirmSubmitForm(sender) {
            if (AGConfirm(sender, "Confirm submit form")) {
                AGLoading(true);
                $(sender).next().click();
            }
        }      
    </script>

</asp:Content>
