<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AttachFileUserControl.ascx.cs" Inherits="ServiceWeb.widget.usercontrol.AttachFileUserControl" %>


<script type="text/javascript">
    function btnAddFile() {
        $('#_btn_add').click();
    }
    function rebindAttachFile() {
        $('#btn_refresh_attach').click();
    }
</script>

<asp:UpdatePanel ID="updatePanelAttachfile" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <triggers>           
            <asp:PostBackTrigger ControlID="_btn_add" />
        <asp:PostBackTrigger ControlID="btn_refresh_attach" />
        </triggers>
    <contenttemplate>
            

<asp:HiddenField ID="_hd_gobalVarsid" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="_hd_gobalVarbusiness_type" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="_hd_gobalVardoc_year" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="_hd_gobalVardoc_type" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="_hd_gobalVardescription" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="_hd_gobalVardoc_number" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="_hd_gobalVaritem_no" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="_hd_gobalVaraction" runat="server" ClientIDMode="Static" />

<div class="container-fluid">
    <panel id="isCreate" runat="server">
        <div class="alert alert-warning" style="width:400px">
          <a href="#" class="alert-link">Please save document before add file.</a>
        </div>
    </panel>
    <panel id="isChange" runat="server" visible="false">
    <br />
    <asp:Panel ID="pndd" runat="server">
        <div class="row" style="text-align: left;">
            <div class="col-lg-12">
                <asp:FileUpload ID="_file_attach" runat="server" CssClass="form-control" Style="cursor: pointer;" ClientIDMode="Static" BackColor="white" />
                <span class="help-block">
                    กรุณาเลือกไฟล์ที่ต้องการแนบ โดยกดปุ่ม เลือกไฟล์ จากนั้นกดปุ่ม Add File ด้านล่าง
                    <%--Please select the files you want to attach by pressing the Choose File button to add the document below.--%>
                </span>
            </div>
        </div>
        <div class="row" style="padding-top:10px; padding-bottom:10px; padding-left:10px;">
            <button id="btnAddFile" runat="server" type="button" class="btn btn-primary" onclick="btnAddFile();">
                <span class="glyphicon glyphicon-pap erclip"></span> Add File
            </button>
            <asp:Button ID="_btn_add" runat="server" Text="" CssClass="register-button hidden-button"
                ClientIDMode="Static" OnClick="_btn_add_Click" />
            <asp:Button ID="btn_refresh_attach" runat="server" Text="" CssClass="register-button hidden-button btn_refresh_attach"
                ClientIDMode="Static" OnClick="btn_refresh_attach_Click" style="display:none"/>
        </div>
    </asp:Panel>
    <div class="row" style="padding-left: 10px;">
        <asp:HiddenField ID="hddUPanel" runat="server" />
        <asp:GridView border="0"  ID="gv_attachfile" runat="server" AutoGenerateColumns="false" 
            ClientIDMode="Static" CssClass="table table-bordered table-striped table-hover" DataKeyNames="sid,business_type,key_object_link,item_no,file_path,file_name"
            OnRowDeleting="gv_attachfile_RowDeleted" OnRowDataBound="gv_attachfileRowDataBound">
            <Columns>
                <asp:CommandField HeaderText="ลบ" ShowDeleteButton="true" DeleteImageUrl="~/images/icon/delete.png"
                    DeleteText="Delete" ButtonType="Image" ControlStyle-Height="15px" ControlStyle-Width="15px"/>
                <asp:TemplateField HeaderText="File Name">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkFile" runat="server" OnClick="lnkFile_Click"  Text='<%# Eval("file_name") %>'
                            Enabled='<%# Eval("file_path") != null %>'></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <%--<asp:BoundField HeaderText="File Name" DataField="file_name"></asp:BoundField>--%>
                <asp:BoundField HeaderText="File extension" DataField="file_extension" ><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                <asp:BoundField HeaderText="File Size" DataField="file_size"><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                <asp:BoundField HeaderText="Description" DataField="description" Visible="false"></asp:BoundField>

                <asp:BoundField HeaderText="Created By" DataField="key_group" ></asp:BoundField>
                <asp:BoundField HeaderText="Created Date" DataField="createdOnFormatDateTime" ></asp:BoundField>

            </Columns>
            <HeaderStyle HorizontalAlign="Center" />
            <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First"
                LastPageText="Last" />
            <HeaderStyle Width="150px" />
        </asp:GridView>
    </div>
    </panel>
</div>

</contenttemplate>
</asp:UpdatePanel>