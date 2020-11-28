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

/* ATTRIBUTION: https://stackoverflow.com/a/55187677/10340424 */
window.scrollToElementId = (elementId) => {
    var element = document.getElementById(elementId);
    if(!element)
    {
        console.warn('element was not found', elementId);
        return false;
    }
    element.scrollIntoView({behavior:'smooth'});
    return true;
}
