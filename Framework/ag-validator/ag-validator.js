var DisableChackUsernamePassword = false;

function AGValidator(sender, parent) {
    TrimAll();
    var msg = [];
    var index = 0;
    var elt = parent == undefined ? $(".require:visible") : $(parent).find(".require:visible");
    $(elt).each(function () {
        if ($(this).val().trim() == "") {
            index++;
            var html = "";
            if ($(this).parent().find("label").length > 0)
                html = $(this).parent().find("label").html();
            else
                html = $(this).attr("placeholder");

            msg.push(html);
        }
    });
    if (msg.length > 0) {
        swal('Please fill in all required fields.', msg.join("\n").split(":").join(""), 'warning');
        return false;
    }

    if (!DisableChackUsernamePassword) {
        if (!ValidateEmail(parent) || !ValidateUsername(parent) || !ValidatePassword(parent)) {
            return false;
        }
    }

    return AGConfirm(sender, "Comfirm to continue");
}

function ValidateEmail(parent) {
    var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
    var toReturn = true;
    var elt = parent == undefined ? $(".email:visible") : $(parent).find(".email:visible");
    $(elt).each(function () {
        if (!pattern.test($(this).val())) {
            $(this).focus();
            swal('Warning', "Please check email format.", 'warning');
            toReturn = false;
            return false;
        }
    });
    return toReturn;
}

function ValidatePassword(parent) {
    var eltPass = parent == undefined ? $(".password:visible") : $(parent).find(".password:visible");
    var eltRePass = parent == undefined ? $(".verify-password:visible") : $(parent).find(".verify-password:visible");
    var pass = $(eltPass).first().val();
    var rePass = $(eltRePass).first().val();
    if (pass == undefined || rePass == undefined) {
        return true;
    }
    if (pass.length < 6 || rePass.length < 6) {
        eltPass.focus();
        swal("", "Password must be 6 characters or more", 'warning');
        return false;
    }
    if (pass != rePass) {
        eltRePass.focus();
        swal("", "Password and verifypassword not match", 'warning');
        return false;
    }
    return true;
}

function ValidateUsername(parent) {
    var elt = parent == undefined ? $(".username:visible") : $(parent).find(".username:visible");
    var username = $(elt).first().val();
    if (username == undefined) {
        return true;
    }
    if (username.length < 6) {
        elt.focus();
        swal("", "username must be 6 characters or more", 'warning');
        return false;
    }
    return true;
}

function TrimAll() {
    $("input[type='text'],input[type='password'],textarea").each(function () {
        $(this).val($(this).val().trim());
    });
}