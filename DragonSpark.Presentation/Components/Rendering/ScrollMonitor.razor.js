// JavaScript Module for ScrollMonitor component
const scrollCache = {};
let lastKnownUrl = window.location.href;
let targetScrollY = null;
let isHistoryNavigation = false;
let cacheVaultLocked = false;
let isRestoringScroll = false;

// Stability tracking variables
let lastDocumentHeight = 0;
let stableFrameCount = 0;
const REQUIRED_STABLE_FRAMES = 10;

if ('scrollRestoration' in history) {
	history.scrollRestoration = 'manual';
}

function checkUrlLifecycle() {
	const currentUrl = window.location.href;

	if (currentUrl !== lastKnownUrl) {
		lastKnownUrl = currentUrl;

		if (isHistoryNavigation && typeof scrollCache[currentUrl] !== 'undefined') {
			targetScrollY = scrollCache[currentUrl];
			isRestoringScroll = true;
			window.scrollTo(window.scrollX, 0);
		} else {
			targetScrollY = null;
			isRestoringScroll = false;
			window.scrollTo(window.scrollX, 0);
		}

		lastDocumentHeight = 0;
		stableFrameCount = 0;
		isHistoryNavigation = false;

		requestAnimationFrame(() => {
			cacheVaultLocked = false;
		});
	}

	if (targetScrollY !== null) {
		const currentHeight = document.documentElement.scrollHeight;

		if (currentHeight >= targetScrollY) {
			window.scrollTo(window.scrollX, targetScrollY);

			// Measure height stability across animation frames
			if (currentHeight === lastDocumentHeight) {
				stableFrameCount++;
			} else {
				stableFrameCount = 0;
				lastDocumentHeight = currentHeight;
			}

			if (stableFrameCount >= REQUIRED_STABLE_FRAMES) {
				targetScrollY = null;
				cacheVaultLocked = false;
				isRestoringScroll = false;
			}
		}
	}

	requestAnimationFrame(checkUrlLifecycle);
}

window.addEventListener('scroll', () => {
	const currentY = window.scrollY;
	if (cacheVaultLocked || targetScrollY !== null || isRestoringScroll) return;

	if (currentY > 0) {
		scrollCache[window.location.href] = currentY;
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