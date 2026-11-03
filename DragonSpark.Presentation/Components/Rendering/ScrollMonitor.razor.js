// JavaScript Module for ScrollMonitor component
const scrollCache = {};
let lastKnownUrl = window.location.href;
let targetScrollY = null;
let isHistoryNavigation = false;
let cacheVaultLocked = false;

let lastDocumentHeight = 0;
let stableFrameCount = 0;
const REQUIRED_STABLE_FRAMES = 5;

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