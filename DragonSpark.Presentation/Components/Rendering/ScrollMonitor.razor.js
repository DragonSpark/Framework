// JavaScript Module for ScrollMonitor component
const scrollCache = {};
let lastKnownUrl = window.location.href;
let targetScrollY = null;
let isHistoryNavigation = false;
let cacheVaultLocked = false;

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

		isHistoryNavigation = false;
		setTimeout(() => { cacheVaultLocked = false; }, 100);
	}

	// Only apply targeted scroll correction if we are actively restoring history
	if (targetScrollY !== null) {
		const maxScrollable = document.documentElement.scrollHeight - window.innerHeight;

		// If the document hasn't grown tall enough yet to reach our target, 
		// pin to the current max or keep trying gently without forcing past content bounds
		if (maxScrollable >= targetScrollY) {
			window.scrollTo(window.scrollX, targetScrollY);
			// Release target once successfully applied
			targetScrollY = null;
			cacheVaultLocked = false;
		} else {
			// Interim position while massive grids are still rendering/expanding
			window.scrollTo(window.scrollX, Math.min(targetScrollY, maxScrollable));
		}
	}

	requestAnimationFrame(checkUrlLifecycle);
}

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

requestAnimationFrame(checkUrlLifecycle);