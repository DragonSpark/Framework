/* ATTRIBUTION: https://github.com/ShaunCurtis/CEC.Routing/blob/master/CEC.Routing/wwwroot/cec.routing.js */
window.cec_setEditorExitCheck = function (show) {
    if (show) {
        window.addEventListener("beforeunload", cec_showExitDialog);
    }
    else {
        window.removeEventListener("beforeunload", cec_showExitDialog);
    }
}

window.cec_showExitDialog = function (event) {
    event.preventDefault();
    event.returnValue = "There are unsaved changes on this page.  Do you want to leave?";
}