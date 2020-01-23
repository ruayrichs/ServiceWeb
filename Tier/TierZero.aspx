<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="TierZero.aspx.cs" Inherits="ServiceWeb.Tier.TierZero" %>
<%@ Register Src="~/UserControl/AutoComplete/AutoCompleteCustomer.ascx" TagPrefix="ag" TagName="AutoCompleteCustomer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-item").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <% if (!string.IsNullOrEmpty(AobjectlinkServiceFromActivity))
       { %>
    <style>
        .hide {
            display: none !important;
        }
        #initiative-left-bar {
            display: none;
        }

        .pipe-bar.z-depth-2 {
            display: none;
        }

        .container-wrapper-indent.container-wrapper-left-indent {
            padding: 0px !important;
        }

        .container-wrapper {
            padding: 0px;
        }

        .navbar.navbar-inverse.navbar-fixed-top {
            display: none !important;
        }

        .master-wrapper {
            margin: 0px !important;
        }
    </style>
    <% } %>
    <div class="row">
        <div class="card mb-3" style="width: 99%;">
            <div class="card-header">
                <b>Tier 0 : Un-Assign From Channel</b>
                <div style="float: right; display: inline;">
                    <%--<asp:Button ID="btnSaveFastService" runat="server" CssClass="btn btn-success btn-sm AUTH_CREATE" Text="บันทึก" style="width:100px;" OnClick="btnSaveFastService_Click" OnClientClick="AGLoading(true);" ClientIDMode="Static"/>--%>
                    <asp:Button ID="btnTireZeroNewSave" runat="server" CssClass="btn btn-success btn-sm" Text="บันทึก" Style="width: 100px;" OnClick="btnTireZeroNewSave_Click" OnClientClick="AGLoading(true);" ClientIDMode="Static" />
                    <asp:Button ID="btnNewCall" runat="server" CssClass="btn btn-primary btn-sm" Text="เคลียร์ค่า" Style="width: 100px;" OnClick="btnNewCall_Click" OnClientClick="AGLoading(true);" ClientIDMode="Static" />
                    <%--<asp:Button ID="btnClose" runat="server" CssClass="btn btn-warning btn-sm hide" Style="width: 100px;" Text="ปิด" OnClientClick="AfterSave();" ClientIDMode="Static" />--%>
                </div>
            </div>
            <div class="card-body" >
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updAddTierZero">
                    <ContentTemplate>
                        <div class="form-row">
                            <label class="col-lg-2 col-md-2 col-sm-2">ชื่อลูกค้า</label>
                            <div class="form-group col-lg-4 col-md-4 col-sm-4">
                                <ag:AutoCompleteCustomer ID="CustomerSelect" runat="server" CssClass="form-control form-control-sm required">  <%--AfterSelectedChange="$('#btnBindContactCus').click();"--%>
                                    </ag:AutoCompleteCustomer>
                            </div>
                            <label class="col-lg-2 col-md-2 col-sm-2">E-Mail</label>
                            <div class="form-group col-lg-4 col-md-4 col-sm-4">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-sm required"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-row">
                            <label class="col-lg-2 col-md-2 col-sm-2">หมายเลขโทรศัพท์</label>
                            <div class="form-group col-lg-4 col-md-4 col-sm-4">
                                <asp:TextBox ID="txtTelNo" runat="server" CssClass="form-control form-control-sm required" onkeypress="ISFloat(this,event);" ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-row">
                            <label class="col-lg-2 col-md-2 col-sm-2">ชื่อเรื่อง</label>
                            <div class="form-group col-lg-4 col-md-4 col-sm-4">
                                <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control form-control-sm required"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-row">
                            <label class="col-lg-2 col-md-2 col-sm-2">รายละเอียดปัญหา</label>
                            <div class="form-group col-lg-10 col-md-10 col-sm-10">
                                <asp:TextBox ID="txtProblemDetail" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Rows="2"></asp:TextBox>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <script>
        function clickContact() {
            $("#btnCustomerChange").click();
        }

        function AfterSave() {
            window.parent.closeIframeServiceCall();
        }

        function btnTireZeroNewSave_Click() {
            AGLoading(true);
            var postData = "";
        }
        function saveTierZerItem(Channel, EMail, CustomerCode, CustomerName, TelNo, Subject, Detail, Status, TicketNumber, TicketType) {
            var postData = {
                Channel: Channel,
                EMail: EMail,
                CustomerCode: CustomerCode,
                CustomerName: CustomerName,
                TelNo: TelNo,
                Subject: Subject,
                Detail: Detail,
                Status: Status,
                TicketNumber: TicketNumber,
                TicketType: TicketType
            };
            $.ajax({
                type: "POST",
                url: servictWebDomainName + "API/TierZeroStructureAPI.aspx",
                data: postData,
                success: function (data) {
                    console.log("success");
                }
            });
        }

        function ISFloat(obj, event) {
            $(obj).val($(obj).val().replace(/[^0-9\.]/g, ''));
            if ((event.which != 46 || event.which == 8 || event.which == 9 || $(obj).val().indexOf('.') != -1)
                      && ((event.which < 48 || event.which > 57))) {
                event.preventDefault();
            }
        }
        function formatCurrency(obj, e) {
            $(obj).val(decimalFormat($(obj).val(), 2))
            return;
        }
    </script>
</asp:Content>
