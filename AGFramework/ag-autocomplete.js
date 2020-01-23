$.fn.AG_RenderAutoComplete = function () {
	var data = JSON.parse($(this).html());

	var ControlID = data.ControlID;
	var AutoCompleteID = data.AutoCompleteID;
	var ValueID = data.ValueID;
	var TextID = data.TextID;
	var RenameID = data.RenameID;

	var IsCustomizeDisplay = data.IsCustomizeDisplay == undefined ? false : data.IsCustomizeDisplay;
	var IsNotAutoSearch = data.IsNotAutoSearch == undefined ? false : data.IsNotAutoSearch;
	var IsEnabled = data.IsEnabled == undefined ? false : data.IsEnabled;
	var IsRename = data.IsRename == undefined ? false : data.IsRename;

	var callBack = data.callBack == "" || data.callBack == null ? undefined : data.callBack;
	var CustomViewCode = data.CustomViewCode == undefined ? "" : data.CustomViewCode;

	function bindAutoCompleteData() {
		$(".divinputgroup" + ControlID).AGWhiteLoading(true, "กำลังเตรียมข้อมูล");
		var src = JSON.parse($("#strJson" + ControlID).html().replace(/&nbsp;/g, ' '));

		$("#" + AutoCompleteID).autocomplete({
			source: function (request, response) {
				//var results = $.ui.autocomplete.filter(src, request.term);
				var searchText = request.term.toLowerCase();
				var results = $.grep(src, function (s) {
					return (s.value.toLowerCase().indexOf(searchText) > -1) || (s.label.toLowerCase().indexOf(searchText) > -1) || (s.shortname.toLowerCase().indexOf(searchText) > -1)
						|| (s.other1.toLowerCase().indexOf(searchText) > -1) || (s.other2.toLowerCase().indexOf(searchText) > -1);
				});
				response(results.slice(0, 20));
			},
			autoFocus: true,
			minLength: 0,
			select: function (event, ui) {
				$("#" + ValueID).val(ui.item.value);
				$("#" + TextID).val(ui.item.label);
				$("#" + AutoCompleteID).val(ui.item.label);
				if (ui.item.shortname != undefined && ui.item.shortname != "") {
					$("#" + AutoCompleteID).attr("title", ui.item.shortname);
				}
				setmodeEditRenameAutoComplete();

				try {
					//TODO Outher function
					var item = {};
					item.value = ui.item.value;
					item.label = ui.item.label.split(item.value).join("").trim().replace(/^[\-]+|[\-]+$/g, "").trim();
					ui.item = item;
					if (callBack) {
						eval(callBack);
					}
				} catch (e) { }

				return false;
			},
			response: function (event, ui) {
				if (ui.content.length === 0) {
					$("#" + ValueID).val("");
					$("#" + TextID).val("");
					console.log("No results found");
				}
			},
			open: function () {
				$(".initiative-model-control-contant").css({ "overflow": "hidden" });
			},
			close: function (event) {
				$(".initiative-model-control-contant").css({ "overflow": "" });
			}
		}).focus(function () {
			if (!IsNotAutoSearch) {
				$(this).autocomplete('search');
			}
			$(this).select();
		}).focusout(function () {
			var memfilter = $(this).val();
			var filter = $(this).val().toUpperCase();
			if (filter == "") {
				$(this).val("");
				$("#" + ValueID).val("");
				$("#" + TextID).val("");
				setmodeEditRenameAutoComplete();
				try {
					//TODO Outher function
					var item = {};
					item.value = "";
					item.label = "";
					item.bind = false;
					var ui = {};
					ui.item = item;
					if (callBack) {
						eval(callBack);
					}
				} catch (e) { }

				return false;
			}
			for (var i = 0; i < src.length; i++) {
				//if (filter.toUpperCase().indexOf(src[i].label) > -1) {
				if (filter.toUpperCase() == src[i].label.toUpperCase()) {
					$(this).val(src[i].label);
					$("#" + ValueID).val(src[i].value);
					$("#" + TextID).val(src[i].label);
					return false;
				}
			}
			$(this).val("");
			setmodeEditRenameAutoComplete();
			$("#" + ValueID).val("");
			$("#" + TextID).val("");
			return false;
		});

		var customize = IsCustomizeDisplay;

		if (customize) {
			$("#" + AutoCompleteID).autocomplete("instance")._renderItem = function (ul, item) {
				var li = $("<li/>");
				var div = $("<div/>");
				var shortName = (item.shortname != null && item.shortname != undefined && item.shortname != "" ? "  (" + item.shortname + ")" : "");
				div.append("<b>" + item.label + shortName + "</b>");
				if (item.other1 != null && item.other1 != undefined && item.other1 != "") {
					div.append("<br><b>Tel </b>" + item.other1);
				}
				if (item.other2 != null && item.other2 != undefined && item.other2 != "") {
					div.append("<br><b>Address </b>" + item.other2);
				}
				li.append(div);
				return li.appendTo(ul);
			};
		}

		if (CustomViewCode == "contact" || CustomViewCode=="layout")
		{
		    $("#" + AutoCompleteID).autocomplete("instance")._renderItem = function (ul, item) {
		        var li = $("<li/>");
		        var div = $("<div/>");
		        div.append("<b>Name " + item.label + "</b>");
		        if (item.shortname != null && item.shortname != undefined && item.shortname.trim() != "") {
		            div.append("<br><b>Mail </b>" + item.shortname);
		        }
		        if (item.other1 != null && item.other1 != undefined && item.other1.trim() != "") {
		            div.append("<br><b>Tel </b>" + item.other1);
		        }
		        if (item.other2 != null && item.other2 != undefined && item.other2.trim() != "") {
		            div.append("<br><b>Remark </b>" + item.other2);
		        }
		        li.append(div);
		        return li.appendTo(ul);
		    };
		}

		showpanelRename(IsEnabled);
		$(".divinputgroup" + ControlID).AGWhiteLoading(false);
	}

	function showpanelRename(setEnabled) {
		if (setEnabled) {
			$(".showpanelRename" + ControlID).css({ "cursor": "no-drop" });
		} else {
			$(".showpanelRename" + ControlID).click(function (e) {
				if ($("#" + AutoCompleteID + "[disabled]").length <= 0) {
					var panelobj = $(this).closest(".panel-autocomplete-box").find(".autocomplete-set-rename-background");
					$(panelobj).show();
					var paneledit = $(this).closest(".panel-autocomplete-box").find(".autocomplete-set-rename");
					$(paneledit).offset({ left: e.pageX - 290, top: e.pageY - 20 });
					$("body").addClass("body-scroll");
				}
			});
		}


		$(".setRenameAutoCompleteData" + ControlID).click(function (e) {
			setRenameAutoCompleteData(this);
		});
		setmodeEditRenameAutoComplete();

	}

	function setRenameAutoCompleteData(obj) {
		var valuedata = $("#" + ValueID).val() + ' - ' + $("#" + RenameID).val();
		$("#" + TextID).val($("#" + RenameID).val());
		$("#" + AutoCompleteID).val(valuedata);

		if (callBack || IsRename) {
			try {
				//TODO Outher function
				var item = {};
				item.value = $("#" + ValueID).val();
				item.label = $("#" + RenameID).val();
				item.bind = false;
				var ui = {};
				ui.item = item;

				if (callBack) {
					eval(callBack);
				}
			} catch (e) { }
		}
		if (obj != undefined) {
			$(obj).closest('.autocomplete-set-rename-background').hide();
		}
		$("body").removeClass("body-scroll");

	}

	function setmodeEditRenameAutoComplete() {
		if (IsRename) {

			if ($("#" + AutoCompleteID).val() != "") {
				$(".divinputgroup" + ControlID).addClass("input-group");
				$(".showpanelRename" + ControlID).removeClass("hide");
			}
			else {
				$(".divinputgroup" + ControlID).removeClass("input-group");
				$(".showpanelRename" + ControlID).addClass("hide");
			}
		}
	}

	bindAutoCompleteData();
}

