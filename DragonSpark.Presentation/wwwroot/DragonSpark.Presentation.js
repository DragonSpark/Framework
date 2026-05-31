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

/* ATTRIBUTION: https://stackoverflow.com/a/55187677/10340424 */
window.scrollToElementId = (elementId) => {
	var element = document.getElementById(elementId);
	if (!element) {
		console.warn('element was not found', elementId);
		return false;
	}
	element.scrollIntoView({ behavior: 'smooth' });
	return true;
}

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

(function () {
	const scrollCache = {};
	let lastKnownUrl = window.location.href;
	let targetScrollY = null;
	let isHistoryNavigation = false;
	let cacheVaultLocked = false;

	let lastDocumentHeight = 0;
	let stableFrameCount = 0;
	const REQUIRED_STABLE_FRAMES = 15;

	window.addEventListener('scroll', () => {
		if (cacheVaultLocked || targetScrollY !== null) return;
		if (window.scrollY > 0) {
			scrollCache[window.location.href] = window.scrollY;
		}
	}, true);

	window.addEventListener('popstate', () => {
		isHistoryNavigation = true;
		cacheVaultLocked = true;
	});

	window.addEventListener('beforeunload', () => {
		cacheVaultLocked = true;
	});

	window.addEventListener('click', (e) => {
		const anchor = e.target.closest('a');
		if (anchor && anchor.href) {
			cacheVaultLocked = true;
		}
	}, true);

	function checkUrlLifecycle() {
		const currentUrl = window.location.href;

		if (currentUrl !== lastKnownUrl) {
			lastKnownUrl = currentUrl;

			if (isHistoryNavigation && typeof scrollCache[currentUrl] !== 'undefined') {
				targetScrollY = scrollCache[currentUrl];
			} else {
				targetScrollY = 0;
				window.scrollTo(window.scrollX, 0);
			}

			lastDocumentHeight = 0;
			stableFrameCount = 0;
			isHistoryNavigation = false;

			setTimeout(() => { cacheVaultLocked = false; }, 100);
		}

		if (targetScrollY !== null) {
			const currentHeight = document.documentElement.scrollHeight;

			window.scrollTo(window.scrollX, targetScrollY);

			if (currentHeight === lastDocumentHeight && currentHeight > window.innerHeight) {
				stableFrameCount++;
			} else {
				stableFrameCount = 0;
				lastDocumentHeight = currentHeight;
			}

			if (stableFrameCount >= REQUIRED_STABLE_FRAMES) {
				targetScrollY = null;
				cacheVaultLocked = false;
			}
		}

		requestAnimationFrame(checkUrlLifecycle);
	}

	requestAnimationFrame(checkUrlLifecycle);
})();