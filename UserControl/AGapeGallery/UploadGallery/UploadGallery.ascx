<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadGallery.ascx.cs" Inherits="ServiceWeb.UserControl.AGapeGallery.UploadGallery.UploadGallery" %>

<script>
    var uploadFormDataAPI<%= ClientID %> = servictWebDomainName + "UserControl/AGapeGallery/API/FormUploadAPI.aspx";
</script>

<style>
    .image-box-<%= ClientID %> {
        width: <%= string.IsNullOrEmpty(PreviewWidth) ? "100px" : PreviewWidth %>;
        height: <%= string.IsNullOrEmpty(PreviewHeight) ? "100px" : PreviewHeight %>;
        background-position: center center;
        -webkit-background-size: cover;
        -moz-background-size: cover;
        background-size: cover;
        -o-background-size: cover;
        background-color: #FCFCFC;
        border: 1px solid #ccc;
        display: inline-block;
        margin-bottom: 5px;
        margin-right: 5px;
        position: relative;
    }

    .image-box-adder {
        border: 2px dashed #ccc;
        font-size: 20px;
        display: table;
        width: 100%;
        height: 100%;
    }

    .image-box-adder-inner {
        display: table-cell;
        vertical-align: middle;
        text-align: center;
        color: #aaa;
        cursor: pointer;
    }

        .image-box-adder-inner:hover {
            color: #777;
        }

    .image-box-remove {
        position: absolute;
        top: -5px;
        right: -5px;
        border-radius: 50%;
        background: #000;
        color: #fff;
        width: 20px;
        height: 20px;
        display: block;
        text-align: center;
        padding-top: 2px;
        cursor: pointer;
        z-index: 3;
    }

    .image-box-detail {
        position: absolute;
        top: 0;
        left: 0;
        bottom: 0px;
        right: 0;
        padding: 10px;
        overflow-x: hidden;
        overflow-y: hidden;
        text-align: center;
        white-space: normal;
        word-wrap: break-word;
        font-size: 12px;
        line-height: 20px;
    }
    .image-box-edit {
        position: absolute;
        top: -5px;
        left: -5px;
        border-radius: 50%;
        background: #000;
        color: #fff;
        width: 20px;
        height: 20px;
        display: block;
        text-align: center;
        padding-top: 2px;
        cursor: pointer;
        z-index: 3;
        background-color: yellow;
        color: black;
        border-color: black;
        border-width: thick;
    }
    .content-bg {
        padding: 16px;
        background: #FFF;
        border-radius: 8px;
        border: 1px solid #CCCCCC;
    }
    .image-box-progress{
        text-align:center;
        font-size:8px;
    }
    .image-box-progress .percent-progress-container{
        background:#ddd;
        border-radius:10px;
        margin:5px 0;
        height:8px;
    }
    .image-box-progress .percent-progress-bar{
        background:#009688;
        font-size:8px;
        border-radius:10px;
        width:11%;
        height:8px;
    }

    .image-box-div-edit {
        width: 450px;
        height: 250px;
        background-position: center center;
        -webkit-background-size: cover;
        -moz-background-size: cover;
        background-size: cover;
        -o-background-size: cover;
        background-color: #FCFCFC;
        border: 1px solid #ccc;
        display: inline-block;
        margin-bottom: 5px;
        margin-right: 5px;
        position: relative;
    }
    .image-box-div-edit-video {
         width: 450px;
        height: 250px;
    }
    .image-box-div-edit-image {
         width: 250px;
        height: 150px;
    }
    .image-box-div-edit-file {
         width: 250px;
        height: 150px;
    }

    .form-control-edit {
    display: block;
    width: 100%;
    height: 34px;
    padding: 6px 12px;
    font-size: 14px;
    line-height: 1.42857143;
    color: #555;
    background-color: #fff;
    background-image: none;
    border: 1px solid #ccc;
    border-radius: 4px;
    -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
    box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
    -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;
    -o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
    transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
}

     .preview-left-slide-panel{
            padding:15px; 
            width:400px;
            position:fixed;
            top:0px;
            left:400px;
            bottom:0;
            background:#fff;
            border:1px solid #aaa;
            -webkit-box-shadow: 0px 2px 10px rgba(0,0,0,.3),0px 0px 1px rgba(0,0,0,.1),inset 0px 1px 0px rgba(255,255,255,.25),inset 0px -1px 0px rgba(0,0,0,.15);
            -moz-box-shadow: 0px 2px 10px rgba(0,0,0,.3),0px 0px 1px rgba(0,0,0,.1),inset 0px 1px 0px rgba(255,255,255,.25),inset 0px -1px 0px rgba(0,0,0,.15);
            box-shadow: 0px 2px 10px rgba(0,0,0,.3),0px 0px 1px rgba(0,0,0,.1),inset 0px 1px 0px rgba(255,255,255,.25),inset 0px -1px 0px rgba(0,0,0,.15);
        }
     .can-preview
     {
         background-color:greenyellow;
     }
     .addon-icon{
            color: #333 !important;
            background: #FFFFFF !important;
            box-shadow: none !important;
            border: 1px solid #ccc !important;
            height: 34px;
            padding: 6px 15px;
            cursor:pointer;
            display:inline-block;
            font-size:16px;
        }
    .hide {
        display:none;
    }
</style>

<div>

    <div id="files-container-<%= ClientID %>" class="image-box-container">
        
        <% if(!DisableUpload){ %>
        <div class="image-box-<%= ClientID %> image-box-adder-container" style="vertical-align:top" id="image-box-adder-container-<%= ClientID %>" onclick="GenerateFormData<%= ClientID %>();">
            <div class="image-box-adder">
                <div class="image-box-adder-inner">
                    <i class="fa <%= AcceptTypeIcon %>"></i>
                </div>
            </div>
        </div>
        <% } %>
    </div>

    <div id="image-box-progress-temp-<%= ClientID %>" class="image-box-<%= ClientID %> image-box-preview image-box-progress hide">
        <div class="image-box-detail">
            Uploading <span class="percent-progress-files"></span> files...
            <div class='percent-progress-container'>
                <div class='percent-progress-bar'></div>
            </div>
            <span class="percent-progress-num">0%</span>
        </div>
    </div>

</div>

<script>
    
    function GenerateFormData<%= ClientID %>() {
        var guid = generateGUID<%= ClientID %>();

        $(".ajax-form-<%= ClientID %>").remove();
        var newForm = $("<form action='"+ uploadFormDataAPI<%= ClientID %> +"' method='post' enctype='multipart/form-data' class='ajax-form-<%= ClientID %>' style='display:none;' />");
        var accepttype = "<%= string.IsNullOrEmpty(AcceptType) ? "" : AcceptType.ToLower() + "/" %>*";
        if (accepttype.indexOf("video") > -1 )
        {
            accepttype = ".mp4, .webm, .ogg";
        }
       
        var fileUpload = $("<input type='file' class='upload-files' name='fileupload' <%= MultipleMode ? "multiple='multiple'" : "" %> accept='"+accepttype+"' />");
        newForm.append($("<input type='text' name='guid' class='upload-guid' value='"+ guid +"' />"));

        fileUpload.change(function () {
            var uploadForm = $(this).closest("form");
            var uploadGuid = uploadForm.find(".upload-guid").val();
            var uploadFilesCount = uploadForm.find(".upload-files").get(0).files.length;

            $(uploadForm).ajaxForm({
                beforeSend: function () {
                    StartUploadAjaxForm<%= ClientID %>(uploadGuid,uploadFilesCount);
                },
                uploadProgress: function (event, position, total, percentComplete) {
                    ProgressUploadAjaxForm<%= ClientID %>(uploadGuid,percentComplete);
                },
                complete: function (xhr) {
                    var datas = $.parseJSON(xhr.responseText);
                    var progress = $(".image-box-progress[data-upload-progress='"+ uploadGuid +"']");

                    if(datas.result == "S"){
                        progress.remove();
                        OnSelectFile<%= ClientID %>(datas.datas);
                    }else{

                    }
                }
            });
            uploadForm.submit();
            <% if (PreviewCertificate) { %>
                <% if (EventType.Equals("background")) { %>
            var reader = new FileReader();
            reader.onload = function (e) {
                document.getElementById("panelCertificate").style.backgroundImage = "url(" + e.target.result + ")";
            };
            reader.readAsDataURL(this.files[0]);
                <% } else if (EventType.Equals("Logo")) { %>
            var reader = new FileReader();
            reader.onload = function (e) {
                document.getElementById("imgLogo").src = e.target.result;
            };
            reader.readAsDataURL(this.files[0]);
                <% } else if (EventType.Equals("SignatureTeacher")) { %>
            var reader = new FileReader();
            reader.onload = function (e) {
                document.getElementById("imgSignatureTeacher").src = e.target.result;
            };
            reader.readAsDataURL(this.files[0]);
                <% } else if (EventType.Equals("SignatureCouse")) { %>
            var reader = new FileReader();
            reader.onload = function (e) {
                document.getElementById("imgSignatureCouse").src = e.target.result;
            };
            reader.readAsDataURL(this.files[0]);
            <% } %>
            <% } %>
        });
        newForm.append(fileUpload);
        $("body").append(newForm);
        fileUpload.click();
    }
    function ProgressUploadAjaxForm<%= ClientID %>(guid,percent) {
        var progress = $(".image-box-progress[data-upload-progress='"+ guid +"']");
        progress.find(".percent-progress-bar").css("width",percent + "%");

        var desc = percent + "%";
        if(percent == 100)
            desc = "เตรียมไฟล์..";
        progress.find(".percent-progress-num").html(desc);
    }
    function StartUploadAjaxForm<%= ClientID %>(guid,countFile) {
        var container = $("#files-container-<%= ClientID %>");
        var progress = $("#image-box-progress-temp-<%= ClientID %>").clone();
        progress.attr("data-upload-progress",guid);
        progress.removeClass("hide");
        progress.find(".percent-progress-files").html(countFile);
        container.prepend(progress);
    }
</script>

<div style="display: none;" id="result-files-container-<%= ClientID %>">
    <div class="hidden-container-trigger">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hddEditFilesContainer" />
                <asp:Button ID="btnTriggerUpload" CssClass="btnTriggerUpload" OnClick="btnTriggerUpload_Click" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="hidden-container-remover hide">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:TextBox runat="server" ID="hddDeleteUploadedFile" CssClass="hddDeleteUploadedFile" />
                <asp:TextBox runat="server" ID="hddDeleteUploadedFileQue" CssClass="hddDeleteUploadedFileQue" />
                <asp:TextBox runat="server" ID="hddEditDescUploadedFileKey" CssClass="hddEditDescUploadedFileKey" />
                <asp:TextBox runat="server" ID="hddEditDescUploadedFileDesc" CssClass="hddEditDescUploadedFileDesc" />
                <asp:Button ID="btnDeleteUploadedFile" CssClass="btnDeleteUploadedFile" OnClick="btnDeleteUploadedFile_Click" runat="server" />
                <asp:Button ID="btnEditDescUploadedFile" CssClass="btnEditDescUploadedFile" OnClick="btnEditDescUploadedFile_Click" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <asp:TextBox runat="server" ID="txtConfig_PreviewWidth" />
    <asp:TextBox runat="server" ID="txtConfig_PreviewHeight" />
    <asp:TextBox runat="server" ID="txtConfig_AcceptType" />
    <asp:TextBox runat="server" ID="txtConfig_DisableUpload" Text="false" />
    <asp:TextBox runat="server" ID="txtConfig_MultipleMode" Text="false" />
    <asp:TextBox runat="server" ID="txtConfig_AutoUploadKM" Text="false" />
    <asp:TextBox runat="server" ID="txtConfig_PreviewCertificate" Text="false" />
    <asp:TextBox runat="server" ID="txtConfig_PreviewCertificate_Event" Text="" />
    <asp:TextBox runat="server" ID="txtConfig_KMUploadFileMessage" />
    <asp:TextBox runat="server" ID="txtConfig_KMObjectID" />

    <asp:TextBox runat="server" ID="txtConfig_KMBusinessType" />
    <asp:TextBox runat="server" ID="txtConfig_KMDocumentType" />
    <asp:TextBox runat="server" ID="txtConfig_KMFiscalYear" />

</div>

<script class='javascript-<%= ClientID %>'>
    var globalFiles<%= ClientID %> = [];
    var globalCountFiles<%= ClientID %> = 0;
    var runtimeCountFiles<%= ClientID %> = 0;
    function generateGUID<%= ClientID %>() {
        var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
        return guid;
    }
    $.fn.removeFile<%= ClientID %> = function () {
        var remover = $(this);
        remover.parent().remove();
        var selectedGuid = remover.attr("data-guid");
        function globalFilesFiltered(value) {
            return selectedGuid != value.guid;
        }

        globalFiles<%= ClientID %> = globalFiles<%= ClientID %>.filter(globalFilesFiltered);
        globalCountFiles<%= ClientID %>--;
        ProcessResult<%= ClientID %>(true);
    }

    $.fn.removeUploadedFile<%= ClientID %> = function () {
        var remover = $(this);
        remover.parent().remove();
        var selectedGuid = remover.attr("data-guid");

        $("#result-files-container-<%= ClientID %> .hidden-container-remover .hddDeleteUploadedFile").val(selectedGuid);
        $("#result-files-container-<%= ClientID %> .hidden-container-remover .btnDeleteUploadedFile").click();
    }

    $.fn.appendPreviewFiles<%= ClientID %> = function (file) {
        try {
            var container = $(this);
            var guid = file.guid;
            var imgBlock = $("<div/>", {
                class: "image-box-<%= ClientID %> image-box-preview",
                css:{
                    border:"2px dashed red"
                }
            });

            var imgRemove = $("<i/>", {
                class: "fa fa-remove image-box-remove",
                "data-guid": guid,
                click: function () {
                    $(this).removeFile<%= ClientID %>();
                }
            });

            imgBlock.append(imgRemove);
            var imgEdit = $("<i/>", {
                class: "fa fa-pencil image-box-edit",
                "data-guid": guid,
                click: function () {
                    alert("helloabdyyyyyyyyyyyyyy" ); 
                      

                }
            });

            //imgBlock.append(imgEdit);

            var fileType = file.FileExtension;
            var fileUrl = file.FilePath;
            var fileName = file.FileName;

            globalFiles<%= ClientID %>.push({
                guid: guid,
                dataUrl: fileUrl,
                name: fileName
            });

            if (
                fileType == "jpg" ||
                fileType == "jpeg" ||
                fileType == "png" ||
                fileType == "bmp" ||
                fileType == "gif" ||
                fileType == "svg"
            ) {
                imgBlock.css("background-image", "url(" + fileUrl + ")");
            }
            else if(
                fileType == "mp4" ||
                fileType == "mov" ||
                fileType == "avi" ||
                fileType == "asf" ||
                fileType == "swf" ||
                fileType == "flv"
            ){
                var video = $("<video/>",{
                    css:{
                        position:"absolute",
                        width:"100%",
                        height:"100%",
                        left:0,
                        top:0,
                        background:"#000"
                    },
                    controls:"controls"
                });

                video.append('<source src="' + fileUrl + '" type="video/mp4" />');
                imgBlock.append(video);
            }
            else {
                var fName = $("<div/>", {
                    class: "image-box-detail",
                    html: "<i class='fa fa-download fa-2x'></i><br>" + fileName
                });
                imgBlock.append(fName);
            }

            ProcessResult<%= ClientID %>();
            container.prepend(imgBlock);
        }
        catch (e) {

        }
    }
    function appendUploadedFiles<%= ClientID %>(datas,type) {

        <% if(AutoUploadKM){ %>
        ClearAllFiles<%= ClientID %>();
        <% } %>

        var container = $("#files-container-<%= ClientID %>");

        for (var i = 0; i < datas.length; i++) {

            if($(".image-box-<%= ClientID %>[data-uploaded='"+ datas[i].guid +"']").length > 0)
                continue;

            var imgBlock = $("<div/>", {
                "data-uploaded":datas[i].guid,
                class: "image-box-<%= ClientID %> image-box-preview image-box-preview-uploaded"
            });
            imgBlock.css({
                "vertical-align": "top",
                "margin-bottom": "40px"
            });
            <% if(!DisableUpload){ %>
           

            var imgRemove = $("<i/>", {
                class: "fa fa-remove image-box-remove",
                "data-guid": datas[i].guid,
                click:function () {
                    $(this).removeUploadedFile<%= ClientID %>();
                }
            });

            imgBlock.append(imgRemove);

            var imgEdit = $("<i/>", {
                class: "fa fa-pencil image-box-edit",
                "data-guid": datas[i].guid,
                click: function (e) { 
                    e.stopPropagation();
                    var curGUID = $(this).attr("data-guid");
                    $(this).parent().hide();  
                    $(this).parent().parent().find(".div-edit-video-<%= ClientID %>[data-uploaded='"+curGUID+"']").fadeIn();

                }
            });
            //imgBlock.append(imgEdit);

            var divEditBlock = $("<div/>", {
                "data-uploaded":datas[i].guid,
                class: " image-box-div-edit image-box-div-edit-"+type+"  div-edit-video-<%= ClientID %> ",
            });
            divEditBlock.css({
                "display": "none"
            });

            var editRowNum = 1;
            var editRowColWidth = 1;
            if (type == "image") {
                editRowNum = 2;
                editRowColWidth = 2;
            }
            else if (type == "video") {
                editRowNum = 5;
                editRowColWidth = 4;
            }
            else  {
                editRowNum = 2;
                editRowColWidth = 2;
            }

           
            divEditBlock.after().html(
                  ' <div class="container" style="padding:15px;"> '+ 
                  '    <div class="row">'+
                  '         <div class="col-md-'+editRowColWidth+' col-sm-'+editRowColWidth+'"> '+
                  '             <label>แก้ไขรายละเอียด</label>' +
                  '             <textarea rows="'+editRowNum+'" class="form-control-edit" style="height: auto;" name="txtAttachDesc-<%= ClientID %>" id="txtAttachDesc-<%= ClientID %>"  data-uploaded="'+datas[i].guid+'"  >' +
                       datas[i].description +
                      '             </textarea> '+
                      '         </div> '+
                      '    </div> '+
                      '    <div class="row" style="padding-top:10px">'+
                      '         <div class="col-md-4 col-sm-4"> '+

                      '             <input type="button" class="btn btn-primary" value="บันทึก" onclick="processSaveEdit<%= ClientID %>(\''+datas[i].guid+'\')">' +
                       '             <input type="button" class="btn btn-danger" value="ยกเลิกการแก้ไข" onclick="closeDivEdit<%= ClientID %>(this)">' +
                      '         </div> '+
                      '    </div> '+
                      ' </div> ' 
                      );

                container.prepend(divEditBlock);
            //  imgBlock.append(divEditBlock);
                <% } %>

            var divKMDescMarginTop = 0;
            if (type == "image") {
                imgBlock.css({
                    "background-image": "url(" + servictWebDomainName.slice(0,-1) + datas[i].path + ")",
                    cursor:"pointer"
                });
                imgBlock.attr("data-image",datas[i].path);
                imgBlock.addClass("system-message-attach-image-block");
                divKMDescMarginTop = 160;
               
            }
            else if(type == "video"){
                var video = $("<video/>",{
                    css:{
                        position:"absolute",
                        width:"100%",
                        height:"100%",
                        left:0,
                        top:0,
                        background:"#000"
                    },
                    controls:"controls"
                });

                video.append('<source src="' + datas[i].path + '" type="video/mp4" />');
               

                imgBlock.append(video);
                divKMDescMarginTop = 260;
            }
            else {
                var canpreview = ( datas[i].name.indexOf(".pdf") > -1 )?"can-preview":"";
                var fName = $("<div/>", {
                    class: "image-box-detail system-message-attach-files " + canpreview,
                    "data-path": datas[i].path,
                    "data-name": datas[i].name,
                    html: "<i class='fa fa-download fa-2x'></i><br>" + datas[i].name,
                    css:{
                        padding:"18px 5px"
                    },
                    click: function () {
                        try { 
                            var name = $(this).attr("data-name");
                            if ( name.indexOf(".pdf") > -1 )
                            {
                                togglePreviewDoc<%= ClientID %>(this);
                                }
                                else
                                {
                                    ActivityDownloadFileForm(this);
                                }
                                
                                return false;
                            } catch (e) { }
                        }
                    });
                    imgBlock.css({
                        cursor:"pointer",
                        borderRadius:10
                    });
                    imgBlock.append(fName);
                    divKMDescMarginTop = 110;
                }
            var divKMDesc = $("<div/>", {
                css:{
                    "margin-top": divKMDescMarginTop + "px"
                },
                html: datas[i].description
            });
            imgBlock.append(divKMDesc);
            container.prepend(imgBlock);
        }

        try {
            container.aGepeGalleryContainer("");
        }
        catch (e) { }

        ToggleAdder<%= ClientID %>();
    }
    function processSaveEdit<%= ClientID %>(guid)
    {
        var descDiv =  $("#txtAttachDesc-<%= ClientID %>[data-uploaded='"+guid+"']").val();
        //   alert( descDiv );
       
        agroLoading(true);
        $("#result-files-container-<%= ClientID %> .hidden-container-remover .hddEditDescUploadedFileKey").val(guid);
        $("#result-files-container-<%= ClientID %> .hidden-container-remover .hddEditDescUploadedFileDesc").val(descDiv);
        $("#result-files-container-<%= ClientID %> .hidden-container-remover .btnEditDescUploadedFile").click();
    }
    function closeDivEdit<%= ClientID %>(obj)
    { 
        $(obj).closest(".container").parent().hide();
        $(obj).closest(".container").parent().parent().find(".image-box-preview").fadeIn();
  
    }
    function ClearAllFiles<%= ClientID %>() {
        globalFiles<%= ClientID %> = [];
        globalCountFiles<%= ClientID %> = 0;
        runtimeCountFiles<%= ClientID %> = 0;
        $("#files-container-<%= ClientID %> .image-box-preview").remove();
        $("#files-container-<%= ClientID %> .image-box-div-edit").remove();
        
        ToggleAdder<%= ClientID %>();
    }
    function ProcessResult<%= ClientID %>(notCheck) {
        if (notCheck || ((globalFiles<%= ClientID %>.length - globalCountFiles<%= ClientID %>) == runtimeCountFiles<%= ClientID %>)) {
            globalCountFiles<%= ClientID %> += runtimeCountFiles<%= ClientID %>;
            runtimeCountFiles<%= ClientID %> = 0;
            ToggleAdder<%= ClientID %>();

            $("#result-files-container-<%= ClientID %> .hidden-container-trigger input[type='hidden']").val(JSON.stringify(globalFiles<%= ClientID %>));
            $("#result-files-container-<%= ClientID %> .hidden-container-trigger .btnTriggerUpload").click();

        }
    }
    function ToggleAdder<%= ClientID %>() {
        var isSingle = <%= (!MultipleMode).ToString().ToLower() %>;
        if (isSingle) {
            if($("#files-container-<%= ClientID %> .image-box-preview").length == 0){
                $("#image-box-adder-container-<%= ClientID %>").removeClass("hide");
            }
            else{
                $("#image-box-adder-container-<%= ClientID %>").addClass("hide");
            }
        }
    }
    function OnSelectFile<%= ClientID %>(datas) {
        <%= MultipleMode ? "" : "ClearAllFiles"+ ClientID +"();" %>
        var files = datas;
        var fLength = files.length;
        runtimeCountFiles<%= ClientID %> = fLength;
        if (fLength > 0) {
            runtimeCountFiles<%= ClientID %> = fLength;
            for (var i = 0; i < fLength; i++) {
                var file = files[i];
                $("#files-container-<%= ClientID %>").appendPreviewFiles<%= ClientID %>(file);
            }
        }
    }
</script>


<div id="div-previewdoc-<%= ClientID %>" class="preview-left-slide-panel"  style="display:none">
    <div style=" position: relative;z-index: 1000000;">
        <iframe id="iframePreviewdoc-<%= ClientID %>" ></iframe>
    </div>
</div>

<div class="hide">
    <asp:UpdatePanel runat="server" ID="udpBackup" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:TextBox runat="server" Text="[]" ID="txtUploadedJSON" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<script>
    function togglePreviewDoc<%= ClientID %>(obj) {
        
      <%--  var shown = $("#div-previewdoc-<%= ClientID %>").is(":visible");
        if (shown) {
            $("#div-previewdoc-<%= ClientID %>").animate({
                left:-400
            }, function () {
                $("#div-previewdoc-<%= ClientID %>").hide();
            });
        } else {
            $("#div-previewdoc-<%= ClientID %>").show();
            $("#div-previewdoc-<%= ClientID %>").animate({
                left: 0
            });
        }

     
        
        document.getElementById("iframePreviewdoc-<%= ClientID %>" ).src = "/ViewerJS/#.."+path;
        window.frames["iframePreviewdoc-<%= ClientID %>" ].location.reload();

        document.getElementById('some_frame_id').contentWindow.location.reload();

  --%>
        var path = $(obj).attr("data-path");
        var name = $(obj).attr("data-name");
        window.open(
          "/ViewerJS/#.."+path,
          "_blank" // <- This is what makes it open in a new window.
        ); 
    }


</script>