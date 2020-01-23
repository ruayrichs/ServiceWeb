<%@ Page Title="" Language="C#" MasterPageFile="~/Accountability/MasterPage/AccountabilityMaster.master" AutoEventWireup="true"  CodeBehind="CompanyStructure.aspx.cs" Inherits="ServiceWeb.Accountability.CompanyStructure.CompanyStructure" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="<%= Page.ResolveUrl("~/bootstrap-select/bootstrap-select.css") %>" rel="stylesheet" />
    <script src="<%= Page.ResolveUrl("~/bootstrap-select/bootstrap-select.js?vs=20190113") %>"></script>
    <script>
        function webOnLoad() {
            //clear old active
            var onav = document.getElementsByClassName("nav-link active")[0].id;
            document.getElementById(onav).className = "nav-link";
            //set new active
            document.getElementById("nav-menu-ws-config").className = "nav-link active";
        };

        $(document).ready(function () { webOnLoad(); }) 
    </script>
    <style>
        .possible-entry-row.active{
            color:orange;
        }
       
    </style>
    <script>
        <%= ERPW.Lib.Authentication.ERPWAuthentication.SID == "001" ?
        "SIDForHideWorkgroup = true" 
        : "SIDForHideWorkgroup = false" %>
    </script>
    
    <div class="row">
        <div class="col-md-4">
            <div >
                <div>
                    <span class="page-header">
                         Work Structure  Configure
                    </span>
                </div>
                <div class="mat-box">             
                    <ul>
                        <li class="possible-entry-row" onclick="possibleEntryClick(this);" data-node-level="-1" style="cursor:pointer;">
                            <span>Root</span>
                        </li>
                        <asp:Repeater runat="server" ID="rptConfigStructure">
                            <ItemTemplate>
                                <li class="possible-entry-row" onclick="possibleEntryClick(this);" data-node-level="<%# Eval("NodeLevel") %>"  style='margin-left:<%# (Convert.ToInt32(Eval("NodeLevel")) + 1) * 10 %>px;cursor:pointer;'>
                                    <span>
                                        <%# Eval("Name") %>
                                    </span>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <script>
                        $(".possible-entry-row:last").addClass("active");
                    </script>
                </div>
                <a href="<%= Page.ResolveUrl("~/Accountability/CompanyStructure/WorkStructurePossibleEntry.aspx") %>">
                    <i class="fa fa-arrow-circle-right"></i>
                    Go to Work Structure Possible Entry
                </a>
            </div>
        </div>
        <div class="col-md-8">
            <div>
                <div>
                    <%--<div class="pull-right" style="cursor:pointer;margin-top:5px;margin-left:10px;" onclick="UpdateBlackboard();"> 
                        <i class="fa fa-th-large "></i>
                        Blackboard
                    </div>--%>
                    <div class="pull-right" style="cursor:pointer;margin-top:5px;margin-left:10px;" onclick="showGuideLine(this);">
                        <i class="fa fa-check-square-o"></i>
                        Show new node guideline
                    </div>
                    <div class="pull-right" style="cursor:pointer;margin-top:5px;" onclick="showPossibleEntry(this);">
                        <i class="fa fa-check-square-o"></i>
                        Show possible entry
                    </div>

                    <span class="page-header">
                        Tree Structure
                    </span>
                </div>
                <div  class="mat-box" style="padding-top:0;">
                    
                    <script>
                        function possibleEntryClick(obj) {
                            obj = $(obj);
                            var level = parseInt(obj.attr("data-node-level"));
                            $(".agape-tree-li[posible-entry-node]").hide();
                            for (var i = level; i >= 0; i--) {
                                $(".agape-tree-li[posible-entry-node='" + i + "']").show();
                            }
                            $(".possible-entry-row").removeClass("active");
                            obj.addClass("active");
                        }
                        function showPossibleEntry(obj) {
                            obj = $(obj);
                            var gl = $(".node-possible-entry-name");
                            obj.find(".fa").toggleClass("fa-check-square-o").toggleClass("fa-square-o");
                            gl.fadeToggle();
                        }
                        function showGuideLine(obj) {
                            obj = $(obj);
                            var gl = $(".agape-tree-menu-guideline");
                            obj.find(".fa").toggleClass("fa-check-square-o").toggleClass("fa-square-o");
                            gl.fadeToggle();
                        }
                    </script>

                    <div style="min-height: 100px;" id="hierarchy"></div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="d-none">
                <%--<asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button Text="text" runat="server" ID="btnRebindHierarchyBlackboard" 
                            ClientIDMode="Static" OnClick="btnRebindHierarchyBlackboard_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>
        </div>
    </div>
    <asp:TextBox ID="txtPossibleEntry" runat="server" ClientIDMode="Static" CssClass="d-none"></asp:TextBox>

    <script>
        var apiUrl = "/Accountability/API/CompanyStructureAPI.aspx";
        function bindHierarchy(rootCount) {
            $("#hierarchy").AGWhiteLoading(true, "Loading data");
            $.ajax({
                url: apiUrl,
                data: {
                    request: "get_hierarchy",
                    WorkGroupCode: "<%= WorkGroupCode %>"
                },
                success: function (datas) {
                    var possibleentry = $("#txtPossibleEntry").val();
                    console.log(possibleentry);
                    console.log(datas);
                    $("#hierarchy").aGapeTreeMenu({
                        data: datas,
                        rootText: "Root",
                        rootCode: "",
                        rootCount: 0,
                        navigateText: "Create structure",
                        onlyFolder: false,
                        share: false,
                        moveItem: false,
                        selecting: false,
                        emptyFolder: true,
                        PossibleEntry: $.parseJSON(possibleentry),
                        onClick: function (result) {
                            console.log(result);
                        },
                        onMove: function (result) {
                            if (result.newParentNode == result.oldParentNode || result.itemType == "e") {
                                bindHierarchy();
                            }
                            else {
                                hierarchyDoAjax({
                                    request: "movenode",
                                    newParentNode: result.newParentNode,
                                    itemNode: result.itemNode,
                                    itemName: result.itemName,
                                    itemType: result.itemType,
                                    WorkGroupCode: "<%= WorkGroupCode %>"
                                });
                            }
                        },
                        onRename: function (result) {
                            hierarchyDoAjax({
                                request: "rename",
                                id: result.id,
                                name: result.name,
                                WorkGroupCode: "<%= WorkGroupCode %>"
                            });
                            //$("#btnRebindHierarchyBlackboard").click();
                        },
                        onNewFolder: function (result) {
                            hierarchyDoAjax({
                                request: "newfolder",
                                parentid: result.parentid,
                                name: result.name,
                                WorkGroupCode: "<%= WorkGroupCode %>"
                            });
                            //AGMessage(result);
                            //$("#btnRebindHierarchyBlackboard").click();
                        },
                        onDelete: function (result) {
                            hierarchyDoAjax({
                                request: "delete",
                                id: result.id,
                                WorkGroupCode: "<%= WorkGroupCode %>"
                            });
                            //$("#btnRebindHierarchyBlackboard").click();
                        },
                        onWorkGroup: function (result) {
                            hierarchyDoAjax({
                                request: "workgroup",
                                id: result.id,
                                WorkGroupCode: "<%= WorkGroupCode %>"
                                });
                            //$("#btnRebindHierarchyBlackboard").click();

                        }
                    });
                    $("#hierarchy").AGWhiteLoading(false);
                    //$(".smart-show-node-name").click(function () {
                    //    $(".smart-show-node-name").removeClass("focus-color");
                    //    $(this).addClass("focus-color");
                    //});
                }
            });
            }
            function hierarchyDoAjax(datas) {
                $("#hierarchy").AGWhiteLoading(true, "กำลังดึงข้อมูล");
                $.ajax({
                    url: apiUrl,
                    data: datas,
                    success: function () {
                        bindHierarchy();

                    },
                    error: function () {
                        bindHierarchy();
                    }
                });
            }
            bindHierarchy();
            //function UpdateBlackboard() {
            //    $('#myModal').modal("show");
            //    setTimeout(function () {
            //        load();
            //        diagramUpdate();
            //    }, 500);
            //    //$('#myModal').on('shown', function () {
            //    //    diagramUpdate();
            //    //});
            //}
    </script>

    <%--<div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog" style="width:90%">

            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">×</button>
                    <h4 class="modal-title">Structure Blackboard</h4>
                </div>
                <div class="modal-body">
                    <uc1:CompanyStructureBlackboard runat="server" id="CompanyStructureBlackboard" DisableSaveChange="true" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>--%>

    <style>
        .tab-cutomize-template {
            padding : 15px;
        }
       
      .divMap {
    position: relative;
}

.map-top-command {
    position: absolute;
    right: 10px;
    top: 10px;
    z-index: 2;
}

    .map-top-command .form-control {
        display: inline-block;
    }

.googleMap {
    z-index: 1;
}
    </style>
   
    <script>
        function openModalDataForSelectMapingWorkGroup(id, name, type) {
            $(".agape-tree-menu-right-click").remove();
            $("#modal-data-customize-template-workgroup").css("width", "102%");
            $("#txtProjextCodeForCustomizeTemplate").val(id);
            $("#btnBindDataCustomizeTemplateForWorkGroup").click();
        }

        function setDropdowModeStatusPrijectSelect(projectCode, projectname) {
            $("[id*=ddlWorkGroup]").val(projectCode);
            $("[id*=ddlWorkGroup]").find('.filter-option').html(projectname);
        }

        function closeModalDataForSelectMapingWorkGroup() {
            $("#modal-data-customize-template-workgroup").css("width", "0");
        }

        function swithModeCustomizeTemplate(obj, mode) {
            $(".menu-tab-mode-customize").removeClass('active');
            $(obj).addClass('active');
            $(".tab-cutomize-template").hide();
            $("." + mode).fadeIn();
        }

        function selectprojectForMappingCustomize(obj) {
            $("#btnBindDataForSelectProjectCodeCustomizeTemplate").click();
        }

        function ConfirmSaveMappingProjectForTemplate(obj) {
            if (AGConfirm(obj, "ยืนยันการบันทึก")) {
                AGLoading(true);
                return true;
            }
            return false;
        }

        function setlatlongFortextboxDisplayMap(lat, long) {
            $("#txtLat").val(lat);
            $("#txtLng").val(long);

        }

    </script>
                              

    <div class="modal srarchHelp fade in" id="modal-data-customize-template-workgroup" data-backdrop="static" style="overflow: auto!important; padding-right: 0px; width: 0; display: block; transition: 1s; right: 0!important; left: 0; background-color: rgba(0, 0, 0, 0.39);">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="$(this).closest('.modal').css('width','0');" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title">Project Detail
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-2">
                           <span class="text-primary">Select Project</span> 
                        </div>
                        <div class="col-md-5">
                            <asp:DropDownList ID="ddlWorkGroup" onchange="selectprojectForMappingCustomize(this);" CssClass="form-control selectpicker" data-live-search="true" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                             <asp:Button ID="btnSaveMappingProject" CssClass="btn btn-success" OnClick="btnSaveMappingProject_Click" OnClientClick="return ConfirmSaveMappingProjectForTemplate(this);" Text="Save" runat="server" />
                        </div>
                    </div>

                    <asp:UpdatePanel ID="udpUpdateDataForProjectSelect" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                          <div id="panelProjectDataDetial" runat="server">
                             <ul class="nav nav-tabs pading-menu-mode-knowlwdge">
                                <li class="menu-tab-mode-customize active" style="cursor:pointer;" onclick="swithModeCustomizeTemplate(this,'tab-memu-detail-customize-template')"><a class="btn-tab-menu">Detail</a></li>
                                <li class="menu-tab-mode-customize" style="cursor:pointer;" onclick="swithModeCustomizeTemplate(this,'tab-memu-location-customize-template');openSuperGridGoogleMapMap(this);"><a class="btn-tab-menu">Location</a></li>
                            </ul>

                            <div class="tab-cutomize-template tab-memu-detail-customize-template">
                                <div class="row">
                                    <div class="col-md-3">
                                        <b>ประเภทโครงการ</b>
                                    </div>
                                    <div class="col-md-6">
                                         <b><asp:Label CssClass="text-primary" ID="txtProjectTypeCustomize" runat="server" /></b>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <b>ประเภทงบประมาณ (ทีมราชการ ใช้ศัพท์ราชการ)</b>
                                    </div>
                                    <div class="col-md-6">
                                         <b><asp:Label CssClass="text-primary" ID="txtBudgetsTypeCustomize" runat="server" /></b>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <b>มูลค่าโครงการ (ล้านบาท)</b>
                                    </div>
                                    <div class="col-md-6">
                                         <b><asp:Label CssClass="text-primary" ID="txtProjectValueCustomize" runat="server" /></b>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3">
                                        <b>จำนวนอาคาร</b>
                                    </div>
                                    <div class="col-md-6">
                                         <b><asp:Label CssClass="text-primary" ID="txtBuildingNumberCutomize" runat="server" /></b>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                         <b>จำนวนชั้นที่สูงสุด</b>
                                    </div>
                                    <div class="col-md-6">
                                         <b><asp:Label CssClass="text-primary" ID="txtMaximumNumberClassCutomize" runat="server" /></b>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <b>พื้นที่ใช้สอยอาคาร (ตร.ม.) </b>
                                    </div>
                                    <div class="col-md-6">
                                         <b><asp:Label CssClass="text-primary" ID="txtAreaBuildingCustomize" runat="server" /></b>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3">
                                        <b>การรับประกัน</b>
                                    </div>
                                    <div class="col-md-6">
                                         <b><asp:Label CssClass="text-primary" ID="txtInsuranceCustomize" runat="server" /></b>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <b>วันเริ่มรับประกัน</b>
                                    </div>
                                    <div class="col-md-6">
                                         <b><asp:Label CssClass="text-primary" ID="txtStartDateInsuranceCustomize" runat="server" /></b>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <b>Remark</b>
                                    </div>
                                    <div class="col-md-6">
                                         <b><asp:TextBox ID="txtRemarkCustomize" Enabled="false" TextMode="MultiLine" Rows="3" CssClass="form-control" runat="server" /></b>
                                    </div>
                                </div>
                            </div>
                              <div class="tab-cutomize-template tab-memu-location-customize-template" style="display: none;">
                                  <div class="row">
                                      <div class="col-md-4">
                                          <b>ประเทศ,สถานที่ตั้ง (Latitude,Longitude)</b>
                                      </div>
                                      <div class="col-md-4 text-primary">
                                          <b>
                                              <asp:Label CssClass="text-primary" ID="txtLatitudeLongitudeCustomize" runat="server" />
                                          </b>
                                      </div>
                                  </div>
                              </div>

                            
                            </div>
                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div  class="tab-cutomize-template tab-memu-location-customize-template" style="display:none;" >
                        <div class="divMap" >

                        <div class="map-top-command">

                            <input type="text" class="form-control input-sm" id="txtSearchText" placeholder="ค้นหาสถานที่" style="width: 400px" />
                            <input type="text" class="form-control input-sm" id="txtLat" value="13.778200" disabled style="width: 150px" />
                            <input type="text" class="form-control input-sm" id="txtLng" value="100.476958" disabled style="width: 150px" />

                        </div>
                        <div id="googleMap" class="googleMap" style="width: 100%; height: 475px;"></div>

                        <script>
                            var map = null;
                            var gmarkers = [];
                            var objTrigger;

                            //setTimeout คือ หน่วงเวลาการโชว์แผนที่ ครึ่งวินาที ??
                            function openSuperGridGoogleMapMap(obj) {
                                objTrigger = $(obj);
                                //$("#modal-google-map-").modal("show");
                                setTimeout(function () {
                                    if (map == null) {
                                        initGoogleMap(parseFloat($("#txtLat").val()), parseFloat($("#txtLng").val()));
                                    } else {
                                        googleMapResize();
                                    }
                                }, 500);
                            }
                            //initGoogleMap() คือการ map=null ให้สร้าง map ใหม่
                            //googleMapResize() กระตุ้นตัวเองโดยไม่ต้องใช้เน็ต ลดภาระในการโหลด googleMap ครั้งแรก


                            //chooseAndCloseMap() ฟังก์ชั่นสุดท้ายเมื่อกดยืนยันที่อยู่หรือ ปุ่ม Choose location
                            function chooseAndCloseMap() {
                                objTrigger.parent().find(".lat-lng").val($("#txtLat").val() + "," + $("#txtLng").val());
                                //$("#modal-google-map-").modal("hide"); //ปิดแผนที่ ผิดป็อบอัพไป ซ่อน=hide
                                $.ajax({
                                    url: "http://maps.googleapis.com/maps/api/geocode/json?latlng=" + $("#txtLat").val() + "," + $("#txtLng").val() + "&sensor=true&lang=th",
                                    success: function (datas) { //ส่งค่าแอดติจูด ลองติจูดไป ที่urlด้านบน แล้วจะออกมาเป็นค่า string คือ address ให้อีกที
                                        objTrigger.parent().find(".address").val(datas.results[0].formatted_address);
                                    }
                                });
                            }
                            //'#txtSearchText' ช่องค้นหาที่อยู่ เป็นฟังก์ชั่นของ google  searchMapFromKeyword
                            function initGoogleMap(lati, longi) {
                                $('#txtSearchText').on('keypress', function (event) {
                                    if (event.which === 13) {
                                        searchMapFromKeyword();
                                        return false;
                                    }
                                });

                                var mapProp = {
                                    center: new google.maps.LatLng(lati, longi),
                                    zoom: 12,
                                    disableDoubleClickZoom: true,
                                    //disableDoubleClickZoom: ไม่ให้ใช้ DoubleClick ในการ zoom ให้กดปุ่มเว้นวรรค บนคีย์บอร์ดแทน
                                    mapTypeId: google.maps.MapTypeId.ROADMAP
                                };

                                map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
                                createMarkerFromLatLng(lati, longi);
                                // createMarkerFromLatLng ใช้สร้าง Marker ครั้งแรกสร้างที่ประเทศไทย

                                google.maps.event.addListener(map, 'dblclick', function (event) {
                                    $("#txtLat").val(event.latLng.lat());
                                    $("#txtLng").val(event.latLng.lng());

                                    removeMarkers(); //เอา Marker ออก
                                    createMarkerFromLatLng(event.latLng.lat(), event.latLng.lng()); //ชี้ Marker ที่จุดใหม่ เมื่อทำการ DoubleClick
                                });
                            }
                            //googleMapResize() ปรับขนาด map
                            function googleMapResize() {
                                google.maps.event.trigger(map, 'resize');
                                map.setZoom(map.getZoom());
                                map.setCenter(new google.maps.LatLng($("#txtLat").val(), $("#txtLng").val()));
                            }
                            //DefaultLocation การตั้งค่าเริ่มต้น
                            function googleMapDefaultLocation(address) {
                                document.getElementById('txtSearchText').value = address;
                                searchMapFromKeyword(false);
                            }
                            //การค้นหาที่อยู่ ด้วย keyหรือที่อยู่ ที่เรากรอกลงไป
                            function searchMapFromKeyword(isSetDefault) {
                                var geocoder = new google.maps.Geocoder();

                                var address = document.getElementById('txtSearchText').value;
                                geocoder.geocode({ 'address': address }, function (results, status) {
                                    if (status === 'OK') {
                                        map.setCenter(results[0].geometry.location);
                                        createMarker(results[0]);
                                        $("#txtLat").val(results[0].geometry.location.lat());
                                        $("#txtLng").val(results[0].geometry.location.lng());

                                    } else {
                                        // do not alert if not found 
                                        //alert('ไม่พบข้อมูลที่ค้นหา [reason: ' + status + ']');
                                    }
                                });
                            }
                            // function createMarker ใช้สร้าง marker
                            function createMarker(place) {
                                var placeLoc = place.geometry.location;
                                var marker = new google.maps.Marker({
                                    map: map,
                                    position: place.geometry.location
                                });

                                gmarkers.push(marker);

                                google.maps.event.addListener(marker, 'click', function () {
                                    infowindow.setContent(place.name);
                                    infowindow.open(map, marker);
                                });
                            }

                            function createMarkerFromLatLng(lat, lng) {
                                var marker = new google.maps.Marker({
                                    map: map,
                                    draggable: true,
                                    position: new google.maps.LatLng(lat, lng)
                                });

                                //แอด event ให้ google marker เลื่อนจุดได้
                                marker.addListener('drag', handleEvent);
                                marker.addListener('dragend', handleEvent);
                                gmarkers.push(marker);
                            }

                            function handleEvent(event) {
                                $("#txtLat").val(event.latLng.lat());
                                $("#txtLng").val(event.latLng.lng());
                            }

                            function removeMarkers() {
                                for (i = 0; i < gmarkers.length; i++) {
                                    gmarkers[i].setMap(null);
                                }
                            }

                        </script>

                        <script src="http://maps.google.com/maps/api/js?libraries=places&key=AIzaSyB6IfRSZYP2F6AdKvsuPysU9pmnRZjCCnA"
                            type="text/javascript"></script>

                    </div>
                     </div>

                    <div class="d-none">
                        <asp:UpdatePanel ID="udpbuttonBindDataFortemplate" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtProjextCodeForCustomizeTemplate" ClientIDMode="Static" Style="display: none;" runat="server" />
                                <asp:Button ID="btnBindDataCustomizeTemplateForWorkGroup" ClientIDMode="Static" OnClick="btnBindDataCustomizeTemplateForWorkGroup_Click" OnClientClick="AGLoading(true);" runat="server" />
                                 <asp:Button ID="btnBindDataForSelectProjectCodeCustomizeTemplate" ClientIDMode="Static" OnClick="btnBindDataForSelectProjectCodeCustomizeTemplate_Click" OnClientClick="AGLoading(true);" runat="server" />

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
   
</asp:Content>
