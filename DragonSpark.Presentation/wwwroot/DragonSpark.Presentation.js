/* ATTRIBUTION: https://github.com/ShaunCurtis/CEC.Routing/blob/master/CEC.Routing/wwwroot/cec.routing.js */
window.cec_setEditorExitCheck = function(show) {
	if (show) {
		window.addEventListener("beforeunload", cec_showExitDialog);
	} else {
		window.removeEventListener("beforeunload", cec_showExitDialog);
	}
}

window.cec_showExitDialog = function(event) {
	event.preventDefault();
	event.returnValue = "There are unsaved changes on this page.  Do you want to leave?";
}

window.scrollToElementId = (elementId) => {
	const element = document.getElementById(elementId);
	if (!element) {
		console.warn('element was not found', elementId);
		return false;
	}

	element.scrollIntoView({ behavior: 'smooth', block: 'start' });

	const offset = window.innerHeight / 4;
	
	setTimeout(() => window.scrollBy({ top: -offset, behavior: 'smooth' }), 150);

	return true;
};


window.applyLocationHash = () => {
	var current = window.location.hash;
	if (current) {
		window.scrollToElementId(current.substring(1));
	}
}

// ATTRIBUTION: https://stackoverflow.com/a/71221325/10340424
function scrollToFirstValidationMessage() {
   new Promise(resolve => setTimeout(resolve, 250)).then(() => {
	   	const elements = document.getElementsByClassName("validation-message");
		if (elements != undefined) {
			const element = elements[0];
			if (element != null) {
				element.parentNode.scrollIntoView({ behavior: 'smooth' });
			}
		}

    });
}