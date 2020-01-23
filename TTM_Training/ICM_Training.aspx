<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ICM_Training.aspx.cs" EnableEventValidation="false" MasterPageFile="~/MasterPage/MasterPage.master"
    Inherits="ServiceWeb.TTM_Training.ICM_Training" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1" ID="Content1">
    <div>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpPanelSearch">
            <ContentTemplate>
                <div class="form-row">
                    <div class="form-group col-3">
                        <label>
                            Doc Type
                        </label>
                        <asp:DropDownList runat="server" ID="ddlDocType" CssClass="form-control"
                            OnSelectedIndexChanged="ddlDocType_SelectedIndexChanged" AutoPostBack="true" onchange="AGLoading(true);">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-3">
                        <label>
                            Doc Number
                        </label>
                        <asp:DropDownList runat="server" ID="ddlListDocNumber" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-3">
                        <label>
                            Fiscal Year
                        </label>
                        <asp:TextBox runat="server" ID="txtFiscalYear" CssClass="form-control" TextMode="Number" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-12">
                        <asp:Button Text="Search" runat="server" ID="btnSearch" CssClass="btn btn-primary"
                            OnClick="btnSearch_Click" OnClientClick="AGLoading(true);" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpResultDetail">
            <ContentTemplate>
                <asp:Panel runat="server" ID="pnlResultDetail">
                    <div class="card">
                        <div class="card-body">
                            <div class="form-row">
                                <div class="form-group col-12">
                                    <label>Ticket Service Header</label>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-group col-12">
                                    <label>Subject</label>
                                    <asp:TextBox runat="server" ID="txtSubject" TextMode="MultiLine" Rows="3" CssClass="form-control" Enabled="false" />
                                </div>
                                <div class="form-group col-4">
                                    <label>Ticket Date</label>
                                    <asp:TextBox runat="server" ID="txtTicketDate" CssClass="form-control" Enabled="false"  />
                                </div>
                                <div class="form-group col-4">
                                    <label>Created By</label>
                                    <asp:TextBox runat="server" ID="txtCreatedBy" CssClass="form-control" Enabled="false"  />
                                </div>
                                <div class="form-group col-4">
                                    <label>Created On</label>
                                    <asp:TextBox runat="server" ID="txtCreatedOn" CssClass="form-control" Enabled="false"  />
                                </div>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="card">
                        <div class="card-body">
                            <div class="form-row">
                                <div class="form-group col-12">
                                    <label>Ticket Service Item</label>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="form-group col-12">
                                    <asp:Repeater runat="server" ID="rptEquipment">
                                        <ItemTemplate>
                                            <div class="card item-card-ci-select">
                                                <div class="card-body">
                                                    <div class="form-row">
                                                        <div class="form-group col-12">
                                                            <label>Configuration Item No.</label>
                                                            <asp:TextBox ID="txtEquipmentNo" runat="server" CssClass="form-control form-control-sm"
                                                                Enabled="false" Text='<%# Eval("EquipmentNo") + " - " + Eval("EquipmentDesc") %>'></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <% if (rptEquipment.Items.Count == 0)
                                       { %>
                                    <div class="alert alert-info">
                                        No data.
                                    </div>
                                    <% } %>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
