function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}

var timer;
var statustimer;

var finished = false;

function checkProgress(progressId) {
    var result = readCookie("AudioConvertionFinished");
    if (1 == result) {
        finished = true;
        var progress = document.getElementById(progressId);
        if (null != progress) progress.style.display = "none";
        eraseCookie("AudioConvertionFinished");
    }
    else {
        timer = setTimeout("checkProgress('" + progressId + "')", 1000);    
    }
}

function ShowProgress(progressId) {
    finished = false;
    var progress = document.getElementById(progressId);
    if (null != progress) progress.style.display = "block";

    timer = setTimeout("checkProgress('" + progressId + "')", 1000);
}

