<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmartPaging.ascx.cs" Inherits="POSWeb.UserControl.SmartPaging" %>


<div id="<%= ID %>">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpSmartPagging">
        <ContentTemplate>
            <div class="row text-center">
                <div class="col-lg-12">
                    <ul class="pagination">
                        <li class="<%= PageIndex == 1 ? "hide" : "hide" %>">
                            <a class="hand" onclick="$(this).next().click();AGLoading(true);"><i class="fa fa-fast-backward"></i></a>
                            <asp:Button Text="text" CssClass="hide" ID="btnFirstPage" OnClick="btnFirstPage_Click" runat="server" />
                        </li>
                        <li class="<%= PageIndex == 1 ? "hide" : "" %>">
                            <a class="hand" onclick="$(this).next().click();AGLoading(true);"><i class="fa fa-backward"></i></a>
                            <asp:Button Text="text" CssClass="hide" ID="btnPrevPage" OnClick="btnPrevPage_Click" runat="server" />
                        </li>
                        <asp:Repeater runat="server" ID="rptSmartPaging">
                            <ItemTemplate>
                                <li class="<%# PageIndex == (Container.ItemIndex + 1) ? "active" : "" %> <%# IsShowPageIndex(Container.ItemIndex + 1) ? "" : "hide" %>">
                                    <a class="hand" onclick="$(this).next().click();AGLoading(true);">
                                        <%# Container.ItemIndex + 1 %>
                                    </a>
                                    <asp:Button Text="text" CssClass="hide" CommandArgument='<%# Container.ItemIndex + 1 %>' ID="btnSelectPage" OnClick="btnSelectPage_Click" runat="server" />
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                        <li class="<%= PageIndex == TotalPage ? "hide" : "" %>">
                            <a class="hand" onclick="$(this).next().click();AGLoading(true);"><i class="fa fa-forward"></i></a>
                            <asp:Button Text="text" CssClass="hide" ID="btnNextPage" OnClick="btnNextPage_Click" runat="server" />
                        </li>
                        <li class="<%= PageIndex == TotalPage ? "hide" : "hide" %>">
                            <a class="hand" onclick="$(this).next().click();AGLoading(true);"><i class="fa fa-fast-forward"></i></a>
                            <asp:Button Text="text" CssClass="hide" ID="btnLastPage" OnClick="btnLastPage_Click" runat="server" />
                        </li>
                    </ul>
                </div>
            </div>
            <asp:HiddenField runat="server" ID="hddPageIndex" Value="1" />
            <asp:HiddenField runat="server" ID="hddTotalDataSourceRow" Value="0" />
            <asp:HiddenField runat="server" ID="hddDataPageSize" Value="0" />
            <script>
                function SmartPaging<%= ID %>(controlID) {
                    $("html,body").animate({
                        scrollTop: $("#" + controlID).offset().top - 100
                    });
                }
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
    <style>
        .pagination {

        }
        
        .pagination li a {
            padding: 5px 10px;
            margin: 0px 3px;
            cursor: pointer;
        }
    </style>
</div>
