(function () {
    //======================== Parameter ========================
    var _UrlPath = LINK_ANALYTICS_PATH;
    var ProgramID = ERPW_PROGRAM_ID ? ERPW_PROGRAM_ID : "";
    var ReferenceID = ERPW_REFERENCE_ID ? ERPW_REFERENCE_ID : "";
    var ReferencePageMode = ERPW_REFERENCE_PAGE_MODE ? ERPW_REFERENCE_PAGE_MODE : "";
    //var _UrlPath = "/Analytics/";
    var _UrlSend = _UrlPath + "Analytics.aspx";
    var _UrlUpdate = _UrlPath + "AnalyticsUpdate.aspx";
    var _UrlJquery = _UrlPath + "jquery-1.11.1.min.js";

    var livingTimer = 0;
    var pageName = (function () {
        var a = window.location.href,
            b = a.lastIndexOf("/");
        return a.substr(b + 1);
    }());
    var pathname = window.location.pathname;
    var nextUrl = "";
    var fromUrl = document.referrer;

    //======================== function ========================
    function timer() {
        setTimeout(function () {
            livingTimer++;
            timer();
        }, 1000);
    }


    function getNextUrl() {
        nextUrl = this.href;
    }


    //======================== OS and Browser ========================

    (function (window) {
        {
            var unknown = '-';

            // screen
            var screenSize = '';
            if (screen.width) {
                width = (screen.width) ? screen.width : '';
                height = (screen.height) ? screen.height : '';
                screenSize += '' + width + " x " + height;
            }

            //browser
            var nVer = navigator.appVersion;
            var nAgt = navigator.userAgent;
            var browser = navigator.appName;
            var version = '' + parseFloat(navigator.appVersion);
            var majorVersion = parseInt(navigator.appVersion, 10);
            var nameOffset, verOffset, ix;

            // Opera
            if ((verOffset = nAgt.indexOf('Opera')) != -1) {
                browser = 'Opera';
                version = nAgt.substring(verOffset + 6);
                if ((verOffset = nAgt.indexOf('Version')) != -1) {
                    version = nAgt.substring(verOffset + 8);
                }
            }
                // MSIE
            else if ((verOffset = nAgt.indexOf('MSIE')) != -1) {
                browser = 'Microsoft Internet Explorer';
                version = nAgt.substring(verOffset + 5);
            }
                // Chrome
            else if ((verOffset = nAgt.indexOf('Chrome')) != -1) {
                browser = 'Chrome';
                version = nAgt.substring(verOffset + 7);
            }
                // Safari
            else if ((verOffset = nAgt.indexOf('Safari')) != -1) {
                browser = 'Safari';
                version = nAgt.substring(verOffset + 7);
                if ((verOffset = nAgt.indexOf('Version')) != -1) {
                    version = nAgt.substring(verOffset + 8);
                }
            }
                // Firefox
            else if ((verOffset = nAgt.indexOf('Firefox')) != -1) {
                browser = 'Firefox';
                version = nAgt.substring(verOffset + 8);
            }
                // MSIE 11+
            else if (nAgt.indexOf('Trident/') != -1) {
                browser = 'Microsoft Internet Explorer';
                version = nAgt.substring(nAgt.indexOf('rv:') + 3);
            }
                // Other browsers
            else if ((nameOffset = nAgt.lastIndexOf(' ') + 1) < (verOffset = nAgt.lastIndexOf('/'))) {
                browser = nAgt.substring(nameOffset, verOffset);
                version = nAgt.substring(verOffset + 1);
                if (browser.toLowerCase() == browser.toUpperCase()) {
                    browser = navigator.appName;
                }
            }
            // trim the version string
            if ((ix = version.indexOf(';')) != -1) version = version.substring(0, ix);
            if ((ix = version.indexOf(' ')) != -1) version = version.substring(0, ix);
            if ((ix = version.indexOf(')')) != -1) version = version.substring(0, ix);

            majorVersion = parseInt('' + version, 10);
            if (isNaN(majorVersion)) {
                version = '' + parseFloat(navigator.appVersion);
                majorVersion = parseInt(navigator.appVersion, 10);
            }

            // mobile version
            var mobile = /Mobile|mini|Fennec|Android|iP(ad|od|hone)/.test(nVer);

            // cookie
            var cookieEnabled = (navigator.cookieEnabled) ? true : false;

            if (typeof navigator.cookieEnabled == 'undefined' && !cookieEnabled) {
                document.cookie = 'testcookie';
                cookieEnabled = (document.cookie.indexOf('testcookie') != -1) ? true : false;
            }

            // system
            var os = unknown;
            var clientStrings = [
                { s: 'Windows 3.11', r: /Win16/ },
                { s: 'Windows 95', r: /(Windows 95|Win95|Windows_95)/ },
                { s: 'Windows ME', r: /(Win 9x 4.90|Windows ME)/ },
                { s: 'Windows 98', r: /(Windows 98|Win98)/ },
                { s: 'Windows CE', r: /Windows CE/ },
                { s: 'Windows 2000', r: /(Windows NT 5.0|Windows 2000)/ },
                { s: 'Windows XP', r: /(Windows NT 5.1|Windows XP)/ },
                { s: 'Windows Server 2003', r: /Windows NT 5.2/ },
                { s: 'Windows Vista', r: /Windows NT 6.0/ },
                { s: 'Windows 7', r: /(Windows 7|Windows NT 6.1)/ },
                { s: 'Windows 8.1', r: /(Windows 8.1|Windows NT 6.3)/ },
                { s: 'Windows 8', r: /(Windows 8|Windows NT 6.2)/ },
                { s: 'Windows NT 4.0', r: /(Windows NT 4.0|WinNT4.0|WinNT|Windows NT)/ },
                { s: 'Windows ME', r: /Windows ME/ },
                { s: 'Android', r: /Android/ },
                { s: 'Open BSD', r: /OpenBSD/ },
                { s: 'Sun OS', r: /SunOS/ },
                { s: 'Linux', r: /(Linux|X11)/ },
                { s: 'iOS', r: /(iPhone|iPad|iPod)/ },
                { s: 'Mac OS X', r: /Mac OS X/ },
                { s: 'Mac OS', r: /(MacPPC|MacIntel|Mac_PowerPC|Macintosh)/ },
                { s: 'QNX', r: /QNX/ },
                { s: 'UNIX', r: /UNIX/ },
                { s: 'BeOS', r: /BeOS/ },
                { s: 'OS/2', r: /OS\/2/ },
                { s: 'Search Bot', r: /(nuhk|Googlebot|Yammybot|Openbot|Slurp|MSNBot|Ask Jeeves\/Teoma|ia_archiver)/ }
            ];
            for (var id in clientStrings) {
                var cs = clientStrings[id];
                if (cs.r.test(nAgt)) {
                    os = cs.s;
                    break;
                }
            }

            var osVersion = unknown;

            if (/Windows/.test(os)) {
                osVersion = /Windows (.*)/.exec(os)[1];
                os = 'Windows';
            }

            switch (os) {
                case 'Mac OS X':
                    osVersion = /Mac OS X (10[\.\_\d]+)/.exec(nAgt)[1];
                    break;

                case 'Android':
                    osVersion = /Android ([\.\_\d]+)/.exec(nAgt)[1];
                    break;

                case 'iOS':
                    osVersion = /OS (\d+)_(\d+)_?(\d+)?/.exec(nVer);
                    osVersion = osVersion[1] + '.' + osVersion[2] + '.' + (osVersion[3] | 0);
                    break;
            }

            // flash (you'll need to include swfobject)
            /* script src="-//ajax.googleapis.com/ajax/libs/swfobject/2.2/swfobject.js" */
            var flashVersion = 'no check';
            if (typeof swfobject != 'undefined') {
                var fv = swfobject.getFlashPlayerVersion();
                if (fv.major > 0) {
                    flashVersion = fv.major + '.' + fv.minor + ' r' + fv.release;
                }
                else {
                    flashVersion = unknown;
                }
            }
        }

        window.jscd = {
            screen: screenSize,
            browser: browser,
            browserVersion: version,
            mobile: mobile,
            os: os,
            osVersion: osVersion,
            cookies: cookieEnabled,
            flashVersion: flashVersion
        };
    }(this));

    //===================== location =====================
    
    var accuracy = "";

    function addJQuery() {
        try {
            var testJquery = $("head");
        }
        catch (e) {
            var script = document.createElement("script");
            script.type = "text/javascript";
            script.src = _UrlJquery;
            document.getElementsByTagName("head")[0].appendChild(script);
        }
    }

    //function clientInFo() {
    //    var script = document.createElement("script");
    //    script.type = "text/javascript";
    //    script.src = "//j.maxmind.com/app/geoip.js";
    //    document.getElementsByTagName("head")[0].appendChild(script);
    //}

    //function myIP() {
    //    if (window.XMLHttpRequest) xmlhttp = new XMLHttpRequest();
    //    else xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");

    //    xmlhttp.open("GET", "//api.hostip.info/get_html.php", false);
    //    xmlhttp.send();

    //    hostipInfo = xmlhttp.responseText.split("\n");

    //    for (i = 0; hostipInfo.length >= i; i++) {
    //        ipAddress = hostipInfo[i].split(":");
    //        if (ipAddress[0] == "IP") return ipAddress[1];
    //    }

    //    return false;
    //}

    //======================== Final ========================
    var row_key = "";
    var ipAddress = "";
    var latitude = "";
    var longtitude = "";
    var country_code = "";
    var country_name = "";
    var city = "";
    var region = "";
    var region_name = "";
    var language = window.navigator.userLanguage || window.navigator.language;

    function clientLocationInFo() {
        //country_code = geoip_country_code();
        //country_name = geoip_country_name();
        //city = geoip_city();
        //region = geoip_region();
        //region_name = geoip_region_name();
        //latitude = geoip_latitude();
        //longtitude = geoip_longitude();
    }

    function setRowKey() {
        var _d = new Date();
        var _dateTime = _d.getDate().toString() + _d.getMonth().toString() + _d.getFullYear().toString() + _d.getTime().toString();
        row_key = LINK_ANALYTICS + "-" + generateUUID() + "-" + _dateTime;

        if ($("#hddAnalytics_Row_Key").length > 0) {
            $("#hddAnalytics_Row_Key").val(row_key);
        }
    }

    function generateUUID() {
        var d = new Date().getTime();
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
        });
        return uuid;
    };

    try {
        addJQuery();
        timer();
        setRowKey();
        //clientInFo();
        //ipAddress = myIP();
    }
    catch (e) {
    }

    window.onunload = function () {
        updateToServer();
    };

    window.onbeforeunload = function () {
        updateToServer();
    };

    window.onload = function () {
        try {
            clientLocationInFo();

        }
        finally {
            setTimeout(function () {
                sendToServer();
            }, 2000);
        }
    }

    function updateToServer() {
        $.ajax({
            type: "POST",
            url: _UrlUpdate,
            data: {
                LiveOn:livingTimer,
                Row_key:row_key
            }
        });
    }

    function sendToServer(act) {
        $.ajax({
            type: "POST",
            url: _UrlSend,
            data:{
                PageName: pageName,
                PathName: pathname,
                Host: document.location.hostname,
                Port: document.location.port,
                FromUrl: document.referrer,
                OS: jscd.os + " " + jscd.osVersion,
                Browser: jscd.browser,
                BrowserVersion: jscd.browserVersion,
                Mobile: jscd.mobile,
                Flash: jscd.flashVersion,
                Cookies: jscd.cookies,
                Screen: jscd.screen,
                LiveOn: livingTimer,
                Lat: latitude,
                Long: longtitude,
                Acc: "",
                CountryCode: country_code,
                CountryName: country_name,
                City: city,
                Region: region,
                RegionName: region_name,
                Language: language,
                IP: "",
                ISP: "",
                Activity: act,
                Row_key: row_key,
                ProgramID: ProgramID,
                ReferenceID: ReferenceID,
                ReferencePageMode: ReferencePageMode
            },
            success: function (data) {
                if (data == "") {
                    return;
                }
                //console.log(data);
                var en = JSON.parse(data);
                if ($("#hddPageTicketMode").length > 0 && $("#btnRequestDisplayMode").length > 0) {
                    if (!en.IsAuthenEdit && $("#hddPageTicketMode").val() == "Change") {
                        AGError("Change Mode Lock By " + en.firstEmployeeName);
                        $("#btnRequestDisplayMode").click();
                    }
                }
            }
        });
    }
    

})(window)
