$.fn.genPluginCreateTierZero = function (h, p) {
    var domainNameTarget = h;
    var permissionKey = p;
    
    $('head').append('<link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" type="text/css" />');
    $('head').append('<link href="https://fonts.googleapis.com/css?family=Titillium+Web:400,600" rel="stylesheet" />');

    var panelPlugin = $("<div>", {
        class: "panel-create-tiarzero"
    });

    var icon = $("<div>", {
        class: "tiarzero-icon",
        html: '<i class="fa fa-bullhorn"></i>'
    }).on("click", function () {
        openTiarZeroInform(this);
    });

    var headerDescript = 'Tier 0 : Inform a problem';
    var htmlContent = '';
    htmlContent += '<div class="card">';
    htmlContent += '    <div class="card-header">';
    htmlContent += '        <i class="fa fa-times float-right text-danger c-pointer content-icon-close"></i>';
    htmlContent += '        <b>' + headerDescript + '</b>';
    htmlContent += '    </div>';
    htmlContent += '    <div class="card-body">';
    htmlContent += '        <div class="content-group">';
    htmlContent += '            <div class="content-item" style="display: block;">';
    htmlContent += '                <div class="form-row">';
    htmlContent += '                    <div class="col-12">';
    htmlContent += '                        <input type="text" name="txtInformProblem_Email" id="txtInformProblem_Email" value="" ';
    htmlContent += '                            placeholder="Email" class="form-control form-control-sm inform-required" />';
    htmlContent += '                    </div>';
    htmlContent += '                </div>';
    htmlContent += '                <div class="form-row">';
    htmlContent += '                    <div class="col-12">';
    htmlContent += '                        <input type="text" name="txtInformProblem_TelNo" id="txtInformProblem_TelNo" value="" ';
    htmlContent += '                            placeholder="TelNo" class="form-control form-control-sm" />';
    htmlContent += '                    </div>';
    htmlContent += '                </div>';
    htmlContent += '            </div>';
    //verify otp
    htmlContent += '            <div class="content-item" style="display: none;">';

    htmlContent += '                <div class="form-row password-input">';
    htmlContent += '                    <div class="col-12">';    
    htmlContent += '                        <input type="password" name="txtInformProblem_Password" id="txtInformProblem_Password" value="" ';
    htmlContent += '                            placeholder="<< Password >>" class="form-control form-control-sm inform-required" />';
    htmlContent += '                    </div>';
    htmlContent += '                </div>';
    htmlContent += '                <div class="form-row password-input">';
    htmlContent += '                    <div class="col-12">';
    htmlContent += '                        <label>If you don\'t have password or forget. Please request for new. You will get OTP via E-Mail.</label>';    
    htmlContent += '                    </div>';
    htmlContent += '                </div>';
    htmlContent += '                <div class="form-row password-input">';
    htmlContent += '                    <div class="col-12">';
    htmlContent += '                        <a class="btn-request-otp text-info" style="font-weight: 600; text-decoration: underline; cursor: pointer;">OTP Request</a>';
    htmlContent += '                    </div>';
    htmlContent += '                </div>';
    htmlContent += '                <div class="form-row otp-input d-none">';
    htmlContent += '                    <div class="col-12">';
    htmlContent += '                        <label>OTP from E-MAIL</label>';
    htmlContent += '                           <input type="text" name="txtInformProblem_OTP" id="txtInformProblem_OTP" value="" ';
    htmlContent += '                               placeholder="OTP" class="form-control form-control-sm inform-required" maxlength="4" />';
    htmlContent += '                         </div>';
    htmlContent += '                    <div class="col-12" style="margin-top: 10px;">';
    htmlContent += '                         <div class="text-info">You will recieve OTP 4 Digit from US, please check your Email</div>';
    htmlContent += '                    </div>';
    htmlContent += '                </div>';
    htmlContent += '            </div>';
    //create problem
    htmlContent += '            <div class="content-item content-item-inform-problem" style="display: none;">';

    htmlContent += '            </div>';
    htmlContent += '        </div>';
    htmlContent += '    </div>';
    htmlContent += '    <div class="card-footer">';   
    htmlContent += '        <button type="button" class="btn btn-success btn-sm btn-submit-account " style="width: 90px;" data-nextprev="1">Submit</button>';
    htmlContent += '        <button type="button" class="btn btn-success btn-sm btn-submit-password password-input d-none" style="width: 90px;" data-nextprev="1">Submit</button>';
    htmlContent += '        <button type="button" class="btn btn-success btn-sm btn-submit-otp otp-input d-none" style="width: 90px;" data-nextprev="1">Submit</button>';
    htmlContent += '        <button type="button" class="btn btn-success btn-sm btn-submit-Inform-problem d-none" style="width: 90px;">Submit</button>';
    htmlContent += '    </div>';
    htmlContent += '</div>';

    var content = $("<div>", {
        class: "tiarzero-content",
        html: htmlContent
    });

    content.attr('data-ls-h', domainNameTarget);
    content.attr('data-ls-p', permissionKey);

    content.find('.content-icon-close').on('click', function () {
        closeTiarZeroInform(this);
    });
      
    content.find('.btn-submit-account').on('click', function () {
        checkValidContact(this);
    });

    content.find('.btn-request-otp').on('click', function () {
        createOTPToEmailTierZeroItem(this);
    });

    content.find('.btn-submit-password').on('click', function () {
        verifyPasswordTierZeroItem(this);
    });

    content.find('.btn-submit-otp').on('click', function () {
        verifyOTPNumnerTierZeroItem(this);
    });

    content.find('.btn-submit-Inform-problem').on('click', function () {
        saveProblemTierZeroItem(this);
    });
    
    panelPlugin.append(content);
    panelPlugin.append(icon);
    $(this).append(panelPlugin);

    var fileUploadPlugin = new FormData();

    var numContentItem = 0;

    function openTiarZeroInform(obj) {
        $(obj).prev().toggleClass('open');
    }

    function closeTiarZeroInform(obj) {
        $(obj).closest('.tiarzero-content').removeClass('open').removeClass('inform');
        resetTiarZeroInform(obj);
    }

    function nextPrevItem(obj) {
        var target = $(obj).closest('.tiarzero-content');

        var IsAlowNext = true;
        target.find(".inform-required:visible").each(function () {
            if ($(this).val() == '') {
                IsAlowNext = false;
                $(this).addClass('bg-required');
            }
        });
        if (!IsAlowNext) {
            return;
        }

        var item = target.find('.content-item');
        var xCount = item.length;
        var numPage = parseInt($(obj).attr('data-nextprev'));

        numContentItem = numContentItem + numPage;
        item.hide();
        $(item[numContentItem]).show();
        
    }

    function resetTiarZeroInform(obj) {
        var target = $(obj).closest('.tiarzero-content');
        var item = target.find('.content-item');
        numContentItem = 0;
        item.hide();
        $(item[numContentItem]).show();

       
        target.find(".inform-required").removeClass('bg-required');

        $("#txtInformProblem_Email").val('');
        //$("#txtInformProblem_Customer").val('');
        $("#txtInformProblem_TelNo").val('');
        $("#txtInformProblem_Password").val('');
        $("#txtInformProblem_OTP").val('');
        target.find('.btn-submit-account').removeClass('d-none');
        target.find('.password-input').addClass('d-none');
        target.find('.otp-input').addClass('d-none');
        target.find('.btn-submit-otp').addClass('d-none');
        target.find('.btn-submit-Inform-problem').addClass('d-none');
        fileUploadPlugin = null;
        fileUploadPlugin = new FormData();
    }

    function checkValidContact(obj) {
        var target = $(obj).closest('.tiarzero-content');
        var IsAlowNext = true;
        target.find(".inform-required:visible").each(function () {
            if ($(this).val() == '') {
                IsAlowNext = false;
                $(this).addClass('bg-required');
            }
        });
        if (!IsAlowNext) {
            return;
        }

        $(obj).closest('.tiarzero-content .card').AGWhiteLoading(true, 'Authorization checking');

        var postData = {
            action: 'vaid_contact',
            sid: '555',
            companycode: 'INET',
            Token: '',
            email: $("#txtInformProblem_Email").val(),
            telno: $("#txtInformProblem_TelNo").val()
        };

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: target.attr('data-ls-h') + "/API/TierZeroProblemInformAPI.aspx",
            data: postData,
            success: function (data) {
                if (data.status == "E") {
                    $(obj).closest('.tiarzero-content .card').AGWhiteLoading(false);
                    alert(data.message);
                    return;
                }
                var result = JSON.parse(data.result);
                $(obj).closest('.tiarzero-content .card').AGWhiteLoading(false);
                $(obj).addClass("d-none");
                target.find('.password-input').removeClass('d-none');
                target.find('.btn-submit-password').removeClass('d-none');
                nextPrevItem(obj);
            },
            error: function (error) {
                $(obj).closest('.tiarzero-content .card').AGWhiteLoading(false);
                alert("Error \n" + JSON.stringify(error));
            }
        });
    }

    function createOTPToEmailTierZeroItem(obj) {
        var target = $(obj).closest('.tiarzero-content');
        var IsAlowNext = true;
        //target.find(".inform-required:visible").each(function () {
        //    if ($(this).val() == '') {
        //        IsAlowNext = false;
        //        $(this).addClass('bg-required');
        //    }
        //});
        //if (!IsAlowNext) {
        //    return;
        //}

        $(obj).closest('.tiarzero-content .card').AGWhiteLoading(true, 'Sending OTP');

        var postData = {
            action: 'otp_generate',
            sid: '555',
            companycode: 'INET',
            Token: '',
            email: $("#txtInformProblem_Email").val(),
            telno: $("#txtInformProblem_TelNo").val()
        };

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: target.attr('data-ls-h') + "/API/TierZeroProblemInformAPI.aspx",
            data: postData,
            success: function (data) {
                if (data.status == "E") {
                    $(obj).closest('.tiarzero-content .card').AGWhiteLoading(false);
                    alert(data.message);
                    return;
                }
                var result = JSON.parse(data.result);
                $(obj).closest('.tiarzero-content .card').AGWhiteLoading(false);

                //$(obj).addClass("d-none");
                target.find(".password-input").addClass("d-none");
                target.find('.otp-input').removeClass('d-none');                
                //nextPrevItem(obj);
            },
            error: function (error) {
                $(obj).closest('.tiarzero-content .card').AGWhiteLoading(false);
                alert("Error \n" + JSON.stringify(error));
            }
        });
    }

    function verifyPasswordTierZeroItem(obj) {
        var target = $(obj).closest('.tiarzero-content');
        var IsAlowNext = true;
        target.find(".inform-required:visible").each(function () {
            if ($(this).val() == '') {
                IsAlowNext = false;
                $(this).addClass('bg-required');
            }
        });
        if (!IsAlowNext) {
            return;
        }
        $(obj).closest('.tiarzero-content .card').AGWhiteLoading(true, 'Verifying password');

        var postData = {
            action: 'password_verify',
            sid: '555',
            companycode: 'INET',
            Token: '',
            email: $("#txtInformProblem_Email").val(),
            telno: $("#txtInformProblem_TelNo").val(),
            password: $("#txtInformProblem_Password").val()
        };

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: target.attr('data-ls-h') + "/API/TierZeroProblemInformAPI.aspx",
            data: postData,
            success: function (data) {
                if (data.status == "E") {
                    $(obj).closest('.tiarzero-content .card').AGWhiteLoading(false);
                    alert(data.message);
                    return;
                }
                openInfromProblemTiarZero(obj, data.result);
                nextPrevItem(obj);
                $(obj).addClass("d-none");
                target.find('.btn-submit-otp').addClass('d-none');
                target.find('.btn-submit-Inform-problem').removeClass('d-none');
                target.addClass('inform');
                $(obj).closest('.tiarzero-content .card').AGWhiteLoading(false);
            },
            error: function (error) {
                $(obj).closest('.tiarzero-content .card').AGWhiteLoading(false);
                alert("Error \n" + JSON.stringify(error));
            }
        });
    }

    function verifyOTPNumnerTierZeroItem(obj) {
        var target = $(obj).closest('.tiarzero-content');
        var IsAlowNext = true;
        target.find(".inform-required:visible").each(function () {
            if ($(this).val() == '') {
                IsAlowNext = false;
                $(this).addClass('bg-required');
            }
        });
        if (!IsAlowNext) {
            return;
        }
        $(obj).closest('.tiarzero-content .card').AGWhiteLoading(true, 'Verifying OPT');
       
        var postData = {
            action: 'otp_verify',
            sid: '555',
            companycode: 'INET',
            Token: '',
            email: $("#txtInformProblem_Email").val(),
            telno: $("#txtInformProblem_TelNo").val(),
            otpid: $("#txtInformProblem_OTP").val()
        };

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: target.attr('data-ls-h') + "/API/TierZeroProblemInformAPI.aspx",
            data: postData,
            success: function (data) {
                if (data.status == "E") {
                    $(obj).closest('.tiarzero-content .card').AGWhiteLoading(false);
                    alert(data.message);
                    return;
                }
                openInfromProblemTiarZero(obj, data.result);
                nextPrevItem(obj);
                $(obj).addClass("d-none");
                target.find('.btn-submit-otp').addClass('d-none');
                target.find('.btn-submit-Inform-problem').removeClass('d-none');
                target.addClass('inform');
                $(obj).closest('.tiarzero-content .card').AGWhiteLoading(false);
            },
            error: function (error) {
                $(obj).closest('.tiarzero-content .card').AGWhiteLoading(false);
                alert("Error \n" + JSON.stringify(error));
            }
        });
    }

    function saveProblemTierZeroItem(obj) {
        var target = $(obj).closest('.tiarzero-content');
        var IsAlowNext = true;
        target.find(".inform-required:visible").each(function () {
            if ($(this).val() == '') {
                IsAlowNext = false;
                $(this).addClass('bg-required');
            }
        });
        if (!IsAlowNext) {
            return;
        }

        $(obj).closest('.tiarzero-content .card').AGWhiteLoading(true, 'Save Process');
        fileUploadPlugin.append("uploadType", "FILE");
        fileUploadPlugin.append("message", "OPEN_TICKET_WEB_PLUGGIN");
        fileUploadPlugin.append("PermissionKey", target.attr('data-ls-p'));
        fileUploadPlugin.append("Channel", "2");
        fileUploadPlugin.append("otpid", target.find("#txtInformProblem_OTP").val());
        fileUploadPlugin.append("EMail", target.find("#txtInformProblem_PEmail").val());
        fileUploadPlugin.append("TelNo", target.find("#txtInformProblem_PMobilePhone").val());
        fileUploadPlugin.append("Product", target.find("#txtInformProblem_PProdoct").val());
        fileUploadPlugin.append("Subject", target.find("#txtInformProblem_PSubject").val());
        fileUploadPlugin.append("Detail", target.find("#txtInformProblem_PDescription").val());
        fileUploadPlugin.append("Status", "0");
        fileUploadPlugin.append("TicketNumber", "");
        fileUploadPlugin.append("TicketType", "");

        $.ajax({
            type: 'POST',
            //dataType: 'json',
            url: target.attr('data-ls-h') + "/API/TierZeroStructureAPI.aspx",
            data: fileUploadPlugin,
            cache: false,
            contentType: false,
            processData: false,
            success: function (datas) {
                var data;
                if (typeof (datas) == "string") {
                    data = JSON.parse(datas)[0];
                } else {
                    data = datas[0];
                }
                $(obj).closest('.tiarzero-content .card').AGWhiteLoading(false);

                if (data.CreatedSuccess == true) {
                    //AGSuccess(data.ResultMessage + ' <br /> ' + data.ResultTicket.ResultMessage);
                    alert("Success \n" + data.ResultMessage + ' \n' + data.ResultTicket.ResultMessage);
                } else {
                    //AGError(data.ResultMessage);
                    alert("Error \n" + data.ResultMessage);
                }
                closeTiarZeroInform(obj);
            },
            error: function (error) {
                $(obj).closest('.tiarzero-content .card').AGWhiteLoading(false);
                alert("Error \n" + JSON.stringify(error));
                //console.log(error);
            }
        });
    }

    function openInfromProblemTiarZero(obj, result) {
        var target = $(obj).closest('.tiarzero-content');
        var panelInform = target.find(".content-item-inform-problem");
        var isdata = panelInform.find("#txtInformProblem_PEmail");
        if (isdata == null || isdata == undefined || isdata.length <= 0) {
            $(panelInform).html("");

            var htmlProduct = '';
            if (result != null && result != undefined) {
                var objResult = jQuery.parseJSON(result);
                if (objResult.equipment != null && objResult.equipment.length > 0) {
                    for (var i = 0; i < objResult.equipment.length; i++) {
                        htmlProduct += ' <option value=' + objResult.equipment[i].code + '>' + objResult.equipment[i].display + '</option> ';
                    }
                }

            }
            var htmlContent = '';
            htmlContent += '    <div class="form-row">';
            htmlContent += '        <div class="col-3">';
            htmlContent += '            <label>E-Mail</label>';
            htmlContent += '       </div>';
            htmlContent += '        <div class="col-6">';
            htmlContent += '            <input type="text" name="txtInformProblem_PEmail" id="txtInformProblem_PEmail" value=""';
            htmlContent += '                placeholder="Email" class="form-control form-control-sm inform-required" />';
            htmlContent += '        </div>';
            htmlContent += '    </div>';
            htmlContent += '   <div class="form-row">';
            htmlContent += '        <div class="col-3">';
            htmlContent += '            <label>Phone No.</label>';
            htmlContent += '        </div>';
            htmlContent += '        <div class="col-6">';
            htmlContent += '            <input type="text" name="txtInformProblem_PMobilePhone" id="txtInformProblem_PMobilePhone" value=""';
            htmlContent += '                placeholder="Phone No." class="form-control form-control-sm" />';
            htmlContent += '        </div>';
            htmlContent += '    </div>';
            htmlContent += '    <div class="form-row">';
            htmlContent += '        <div class="col-3">';
            htmlContent += '            <label>Product</label>';
            htmlContent += '        </div>';
            htmlContent += '        <div class="col-9">';
            htmlContent += '           <select name="txtInformProblem_PProdoct" id="txtInformProblem_PProdoct" class="form-control form-control-sm">';
            htmlContent += '                <option value="">ไม่ระบุ</option>' + htmlProduct;
            htmlContent += '            </select>';
            htmlContent += '        </div>';
            htmlContent += '    </div>';
            htmlContent += '    <div class="form-row">';
            htmlContent += '        <div class="col-3">';
            htmlContent += '            <label>Subject</label>';
            htmlContent += '        </div>';
            htmlContent += '        <div class="col-9">';
            htmlContent += '            <input type="text" name="txtInformProblem_PSubject" id="txtInformProblem_PSubject" value=""';
            htmlContent += '                placeholder="Subject" class="form-control form-control-sm inform-required" />';
            htmlContent += '        </div>';
            htmlContent += '    </div>';
            htmlContent += '    <div class="form-row">';
            htmlContent += '       <div class="col-3">';
            htmlContent += '            <label>Details</label>';
            htmlContent += '        </div>';
            htmlContent += '        <div class="col-9">';
            htmlContent += '            <textarea placeholder="Description" name="txtInformProblem_PDescription" id="txtInformProblem_PDescription"';
            htmlContent += '                class="form-control form-control-sm" rows="4"';
            htmlContent += '                style="resize: none;"></textarea>';
            htmlContent += '        </div>';
            htmlContent += '    </div>';
            htmlContent += '    <div class="form-row">';
            htmlContent += '        <div class="col-3">';
            htmlContent += '            <label>Attach File</label>';
            htmlContent += '            <i class="fa fa-paperclip plugin-tier-attach-file" style="font-size: 19px;" onclick="$(this).next().click();" style="float: right;"></i>';
            htmlContent += '            <input type="file" name="txtInformProblem_PAttachFileList" id="txtInformProblem_PAttachFileList" value=""';
            htmlContent += '              multiple="multiple" placeholder="Attach File" class="plugin-tier-attach-file d-none" />';
            htmlContent += '        </div>';
            htmlContent += '        <div class="col-9">';
            htmlContent += '            <div id="plugin-tier-file-detail"></div>';
            htmlContent += '        </div>';
            htmlContent += '    </div>';
            $(panelInform).append(htmlContent);
            fileUploadPlugin = new FormData();
            setplunginticketfileupload(obj);
        }
        else
        {
           
            target.find("#txtInformProblem_PProdoct").val('');
            target.find("#txtInformProblem_PSubject").val('');
            target.find("#txtInformProblem_PDescription").val('');
            target.find("#txtInformProblem_PAttachFileList").val('');
        }
        fileUploadPlugin = new FormData();
        target.find("#txtInformProblem_PEmail").val(target.find("#txtInformProblem_Email").val());
        target.find("#txtInformProblem_PMobilePhone").val(target.find("#txtInformProblem_TelNo").val());
    }
    
    function setplunginticketfileupload(obj) {
        var target = $(obj).closest('.tiarzero-content');
        target.find("#txtInformProblem_PAttachFileList").change(function (ev) {
            var files = target.find("#txtInformProblem_PAttachFileList")[0].files; //Files[0] = 1st file
            if (files.length > 0) {
                for (var i = 0; i < files.length; i++) {
                    fileUploadPlugin.append('UploadedFiles', files[i], files[i].name);
                }
                var fileupload = fileUploadPlugin.getAll("UploadedFiles");
                target.find("#plugin-tier-file-detail").html("");
                for (var i = 0; i < fileupload.length; i++) {
                    var content = '';
                    content += '<div class="alert alert-info panel-tier-file-item" role="alert">';
                    content += '<button type="button" class="close plugin-tier-file-close" data-no="' + i + '" data-dismiss="alert" aria-label="Close">';
                    content += '<span aria-hidden="true">×</span>';
                    content += '</button>';
                    content += '<strong>' + fileupload[i].name + '</strong>';
                    content += '</div>';
                    target.find("#plugin-tier-file-detail").append(content);
                }
                target.find("#txtInformProblem_PAttachFileList").val("");
                target.find(".plugin-tier-file-close").on('click', function () {
                    deletefileattachfileplugintier(this);
                });
            }
        });
    }

    function deletefileattachfileplugintier(obj) {
        var target = $(obj).closest('.tiarzero-content');
        var index = parseInt($(obj).data('no'));
        var fileupload = fileUploadPlugin.getAll("UploadedFiles");
        fileUploadPlugin = null;
        fileUploadPlugin = new FormData();
        for (var i = 0; i < fileupload.length; i++) {
            if (index == i) { continue; }
            fileUploadPlugin.append('UploadedFiles', fileupload[i], fileupload[i].name);
        }

        fileupload = fileUploadPlugin.getAll("UploadedFiles");
        target.find("#plugin-tier-file-detail").html("");
        for (var i = 0; i < fileupload.length; i++) {
            var content = '';
            content += '<div class="alert alert-info panel-tier-file-item" role="alert">';
            content += '<button type="button" class="close plugin-tier-file-close" data-no="' + i + '" data-dismiss="alert" aria-label="Close">';
            content += '<span aria-hidden="true">×</span>';
            content += '</button>';
            content += '<strong>' + fileupload[i].name + '</strong>';
            content += '</div>';
            target.find("#plugin-tier-file-detail").append(content);
        }
        target.find(".plugin-tier-file-close").on('click', function () {
            deletefileattachfileplugintier(this);
        });
    }

    $.fn.AGWhiteLoading = function (flag, msg) {
        var elt = $(this);
        if (flag) {
            var loading = $("<div/>", {
                class: "agp-white-loading",
                css: {
                    zIndex: 10,
                    background: "rgba(255, 255, 255, 0.68)",
                    position: "absolute",
                    top: 0,
                    left: 0,
                    right: 0,
                    height: "100%",
                    width: "100%",                    
                    textAlign: "center"
                }
            });
            $(loading).append($("<img/>", {
                src: domainNameTarget + "/images/loadmessage.gif",
                css: {              
                    width: 20,
                    height: 20
                }
            }));
            $(loading).append($("<label/>", {
                html: (msg == undefined ? "Loading content" : msg) + "...",
                css: {
                    marginLeft: 10,
                    position: "absolute"
                }
            }));
            $(loading).css("padding-top", (elt.height() / 2) - 15);
            $(loading).css("padding-right", (elt.width() / 4) + 10);
            $(elt).append(loading);
            $(elt).addClass("position-relative");
        }
        else {
            elt.find(".agp-white-loading").remove();
            elt.removeClass("position-relative");
        }
    }
    
}

    
