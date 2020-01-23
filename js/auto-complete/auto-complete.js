//function loadDependencieScript() {
//    var zipjs = document.createElement("script");
//    zipjs.id = "zip_js_script";
//    zipjs.src = '/js/auto-complete/dependencies/zip.js/zip.js';
//    zipjs.type = 'text/javascript';
//    document.head.appendChild(zipjs);

//    var jqljs = document.createElement("script");
//    jqljs.id = "jql_js_script";
//    jqljs.src = '/js/auto-complete/dependencies/JQL.min.js';
//    jqljs.type = 'text/javascript';
//    document.head.appendChild(jqljs);
//}

function readTextInZip(path2zip, filenameinzip, callback) {   
    var xhr = new XMLHttpRequest();
    xhr.responseType = 'blob';
    xhr.onreadystatechange = function () {

        if (xhr.readyState === 4) {
            if (xhr.status === 200) {

                zip.createReader(new zip.BlobReader(xhr.response), function (zipReader) {

                    zipReader.getEntries(function (files) {

                        var hasfile = false;

                        for (var i = 0; i < files.length; i++) {

                            if (files[i].filename != filenameinzip) {
                                continue;
                            }

                            hasfile = true;

                            files[i].getData(new zip.BlobWriter(), function (blob) {

                                var reader = new FileReader();
                                reader.onload = function () {
                                    callback(reader.result);
                                };
                                reader.readAsText(blob);

                            });
                        }

                        if (!hasfile) {
                            console.log('File "' + filenameinzip + '" is not exists.');
                        }

                    });

                });

            } else {
                console.log('File "' + path2zip + '" is not exists.');
            }
        }
    };

    xhr.open('GET', path2zip);
    xhr.send();
};