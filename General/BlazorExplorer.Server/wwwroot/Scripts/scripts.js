/**
* Open content and save to file.
* @param fileName The name of file to save.
* @param fileType The type of file to save.
* @param fileContent The content to save.
*/
window.openSaveFile = function (fileName, fileType, fileContent) {
    var filePath = fileName + "." + fileType.toLowerCase();
    popupContent(fileContent, filePath, fileType=='HTML').focus();
};

/**
 * Receives the download URL and file name, and that downloads form the URL to a file with a specified file name.
 * @param {any} options
 */
window.downloadFromUrl = function (options) {
    const { url, fileName } = options;
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}

window.expand = function (e) {
    var expandRight = document.querySelector(".oi-expand-right");
    var expandLeft = document.querySelector(".oi-expand-left");
    if (expandRight) {
        expandRight.parentElement.previousElementSibling.style.display = "none";
        expandRight.classList.remove("oi-expand-right");
        expandRight.classList.add("oi-expand-left");
        expandRight.parentElement.parentElement.parentElement.style.width = '30px';
    }
    if (expandLeft) {
        expandLeft.parentElement.previousElementSibling.style.display = "flex";
        expandLeft.classList.remove("oi-expand-left");
        expandLeft.classList.add("oi-expand-right");
        expandLeft.parentElement.parentElement.parentElement.style.width = '20%';
    }
}

/**
 * Open content and save to file from byte array
 * @param {any} options
 */
window.downloadFromByteArray = function (options) {
    const { byteArray, fileName, contentType } = options;
    // Convert base64 string to numbers array.
    const numArray = atob(byteArray).split('').map(c => c.charCodeAt(0));

    // Convert numbers array to Uint8Array object.
    const uint8Array = new Uint8Array(numArray);

    // Wrap it by Blob object.
    const blob = new Blob([uint8Array], { type: contentType });

    // Create "object URL" that is linked to the Blob object.
    const url = URL.createObjectURL(blob);

    // Invoke download helper function that implemented in 
    // the earlier section of this article.
    downloadFromUrl({ url: url, fileName: fileName });

    // At last, release unused resources.
    URL.revokeObjectURL(url);
}

/**
* Open content in new window.
* @param title The title of new window.
* @param content The content to open.
* @param isHTML This indicates the content will be loaded as HTML or text.
* @param forceNewWindow Force to open in new window alternative for tab.
*/
window.popupContent = function (content, title, isHTML, forceNewWindow) {
    var opts;
    if (forceNewWindow) {
        var w = screen.width * 70 / 100,
            h = screen.height * 70 / 100;
        opts = 'left=0,top=0,width=' + w + ',height=' + h + ',toolbar=0,scrollbars=0,status=0';
    }
    var newWindow = window.open('', '', opts);
    saveFileIn(content, title, newWindow);
    if (newWindow) {
        if (isHTML) {
            newWindow.document.write(content);
            newWindow.document.close();
        } else {
            newWindow.document.close();
            var setContent = function () {
                setTimeout(function () {
                    var body = newWindow.document.body;
                    if (body == null) setContent();
                    else body.innerText = content;
                }, 0);
            }
            setContent();
        }
        newWindow.document.title = title;
        return newWindow;
    } else {
        return window;
    }    
};

/**
* Saves content as text to file.
* @param filePath The path/name of file to save.
* @param fileContent The content to save.
*/
window.saveFile = function (fileContent, filePath) {    
    var blob = new Blob([fileContent],
        { type: "text/plain;charset=utf-8" });
    saveBlob(blob, filePath);   
}

/**
* Saves content to file in specific Window.
* @param filePath The path/name of file to save.
* @param fileContent The content to save.
* @param _window The window which file is saved from.
*/
window.saveFileIn = function (fileContent, filePath, _window) {
    _window = _window || window;
    if (!_window || _window == window) {
        saveFile(fileContent, filePath);
    } else {
        var html = '<script>(function(){ var saveBlob=' + saveBlob
                   + ';var saveFile=' + saveFile
                   + ';saveFile(' + JSON.stringify(fileContent) + ',"' + filePath.replace('"','_') + '");})();</script>';
        _window.document.write(html);
    }
}

/**
* Saves the Blob object as a file.
* @param blob The Blob object to save.
* @param fileName The name with which the file is saved.
*/
window.saveBlob =  function(blob, fileName) {
    if (!blob || !(blob instanceof Blob) || !fileName) {
        return;
    }
    if (navigator.msSaveBlob) {
        navigator.msSaveBlob(blob, fileName);
    }
    else {
        var link = document.createElement('a'),
            click = function (element) {
                var evnt = document.createEvent('MouseEvents');
                evnt.initMouseEvent('click', true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
                element.dispatchEvent(evnt);
            };
        if ("download" in link) {
            var url = window.URL || window.webkitURL || window, objUrl = url.createObjectURL(blob);
            link.href = objUrl;
            link.target = "_blank";
            link.download = fileName;
            click(link);
            link = null;
            window.setTimeout(function () {
                url.revokeObjectURL(objUrl);
            }, 30000);
        }
        else {
            var fr = new FileReader();
            // Save a blob using data URI scheme
            fr.onloadend = function (e) {
                link.download = fileName;
                link.href = fr.result;
                click(link);
                link = null;
            };
            fr.readAsDataURL(blob);
        }
    }
}

