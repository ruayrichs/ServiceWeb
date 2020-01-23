<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeBehind="PivotTableReport.aspx.cs" Inherits="ServiceWeb.Report.PivotTableReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="">
        <script>
            (function (d, s, id, h, con, ss, u, v) {
                    var js, fjs = d.getElementsByTagName(s)[0];
                    if (d.getElementById(id)) return; js = d.createElement(s); js.id = id;
                    js.setAttribute("data-iam-ss", ss); js.setAttribute("data-iam-u", u); js.setAttribute("data-iam-h", h);
                    js.src = h + "/plugin/lib/i_am_margin_core.js"; fjs.parentNode.insertBefore(js, fjs);
                }(document, 'script', '_iam_plugin_core', 'http://iammargin.p21kids.com', '_iam_container', '001', 'focusone')
            );

            function _iam_load() {
                _iam_initialFrame({
                    targetElement: "#chart_1",
                    chart: "<%= Request["type"] %>",
                    variantCode: "<%= Request["id"] %>",
                    success: function () {
                    }
                });
            }

        </script>
        <div id="chart_1"></div>
    </div>
</asp:Content>
