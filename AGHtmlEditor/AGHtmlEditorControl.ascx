<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AGHtmlEditorControl.ascx.cs" Inherits="ServiceWeb.AGHtmlEditor.AGHtmlEditorControl" %>
<link href="<%= Page.ResolveUrl("~/AGHtmlEditor/dist/summernote-bs4.css") %>" rel="stylesheet" />
<script src="<%= Page.ResolveUrl("~/AGHtmlEditor/dist/summernote-bs4.min.js?vs=20190113") %>"></script>
<style>
    .note-btn {
        height: 30px;
        box-shadow: none;
    }
</style>
<asp:Panel runat="server" ClientIDMode="AutoID">
    <div class="summer-note-container-<%= ClientID %>">
        <asp:TextBox runat="server" ID="txtSummerNoteTextTemp"
            TextMode="MultiLine" Rows="5" CssClass="summernote-text-blog-temp hide" Style="display: none;" />

        <div class="summernote-text-blog" id="summernote-text-blog-<%= ClientID %>">
            <%= txtSummerNoteTextTemp.Text %>
        </div>
    </div>
    <script>
        function summerNoteTextInit<%= ClientID %>() {

            var paramButton = function (context) {
                <%= hideParamButton ? "return;" : "" %>
                
                var ui = $.summernote.ui;

                // create button
                var event = ui.buttonGroup([
                    ui.button({
                        contents: 'Add Parameter <i class="fa fa-caret-down" aria-hidden="true"></i>',
                        tooltip: 'Event Data',
                        data: {
                            toggle: 'dropdown'
                        }
                    }),
                    ui.dropdown({
                        items: [
                            'SUBJECT'
                            , 'DESCRIPTION'
                            , 'TICKETNO'
                            , 'TICKETTYPE'
                            , 'CUSTOMER'
                            , 'INCIDENTAREA'
                            , 'STATUS'
                            , 'PRIORITY'
                            , 'AFFECTSLA'
                            , 'SUMMARYPROBLEM'
                            , 'SUMMARYCAUSE'
                            , 'SUMMARYRESOLUTION'
                            , 'ACCOUNTABILITY'
                            , 'DETAIL'
                            , 'DETAILDESC'
                            , 'TITLEDESC'
                            , 'COMMENT'
                            , 'UPDATESTATUS'
                            , 'EVENT'
                            , 'URL'
                        ],
                        callback: function (items) {
                            $(items).find('a').attr("href", "Javascript:;");
                            $(items).closest('.dropdown-menu').css({ "max-height": "350px", "overflow-x": "auto", "width": "auto" });
                            //$(items).find('a').css("cursor", "pointer");
                            $(items).find('a').on('click', function () {
                                context.invoke("editor.insertText", "{!#" + $(this).html() + "#!}");
                            });
                        }
                    })
                ]);

                return event.render();   // return button as jquery object
            }
            
            var summer = $("#summernote-text-blog-<%= ClientID %>");
            summer.summernote({
                height: "<%= _Height %>",
                toolbar: [
                    ['style', ['style']],
                    ['font', ['bold', 'italic', 'underline', 'clear']],
                    ['fontname', ['fontname']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'paragraph']],
                    ['table', ['table']],
                    ['insert', ['link', 'picture', 'video']],
                    ['view', ['fullscreen', 'codeview', 'help']],
                    ['mydropdow', ['event']]
                ],
                buttons: {
                    event: paramButton
                }
            });
            
            var container = $(".summer-note-container-<%= ClientID %>");
            container.find(".note-editable").blur(function () {
                var summer = $("#summernote-text-blog-<%= ClientID %>");
                var html = summer.summernote('code');
                html = html.split('<').join('SIGNLESSTHAN');
                summerNoteTextChange<%= ClientID %>(html);
            });;
        }
        function summerNoteTextChange<%= ClientID %>(html) {
            var container = $(".summer-note-container-<%= ClientID %>");
            container.find(".summernote-text-blog-temp").val(html);
        }
    </script>

</asp:Panel>
