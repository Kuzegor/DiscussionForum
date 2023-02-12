// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showTopicsPopup() {
    let userPopup = document.getElementById("user-popup");
    let topicsPopup = document.getElementById("topics-popup");
    let blankCanvas = document.getElementById("blank-canvas");
    if (topicsPopup.style.visibility == "visible") {
        topicsPopup.style.visibility = "hidden";
        blankCanvas.style.visibility = "hidden";
    }
    else {
        topicsPopup.style.visibility = "visible";
        userPopup.style.visibility = "hidden";
        blankCanvas.style.visibility = "visible";
    }
}

function showUserPopup() {
    let userPopup = document.getElementById("user-popup");
    let topicsPopup = document.getElementById("topics-popup");
    let blankCanvas = document.getElementById("blank-canvas");
    if (userPopup.style.visibility == "visible") {
        userPopup.style.visibility = "hidden";
        blankCanvas.style.visibility = "hidden";
    }
    else {
        userPopup.style.visibility = "visible";
        topicsPopup.style.visibility = "hidden";
        blankCanvas.style.visibility = "visible";
    }
}

function hideAllPopups() {
    let userPopup = document.getElementById("user-popup");
    let topicsPopup = document.getElementById("topics-popup");
    let blankCanvas = document.getElementById("blank-canvas");
    userPopup.style.visibility = "hidden";
    topicsPopup.style.visibility = "hidden";
    blankCanvas.style.visibility = "hidden";
}
