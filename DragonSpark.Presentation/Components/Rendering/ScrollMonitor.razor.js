// JavaScript Module for ScrollMonitor component (v2.33.7-stable)
const scrollCache = {};
let activeNavigationId = 0;
let activeRestorationId = 0;
let lastRestoredTime = 0;
let previousUrl = window.location.href;
let isRestoring = false;

if ('scrollRestoration' in history) {
	history.scrollRestoration = 'manual';
}

function saveCurrentScrollForUrl(url) {
	if (isRestoring || Date.now() - lastRestoredTime < 500) return;

	if (url) {
		const currentY = window.scrollY;
		if (currentY > 0 || typeof scrollCache[url] === 'undefined') {
			scrollCache[url] = currentY;
			try {
				sessionStorage.setItem(`scroll_${url}`, currentY);
			} catch (e) { }
		}
	}
}

function getStoredScroll(url) {
	if (typeof scrollCache[url] !== 'undefined') {
		return scrollCache[url];
	}
	try {
		const saved = sessionStorage.getItem(`scroll_${url}`);
		if (saved !== null) {
			const val = parseFloat(saved);
			scrollCache[url] = val;
			return val;
		}
	} catch (e) { }
	return undefined;
}

// Continuously monitor URL changes (handles Blazor SPA navigation seamlessly)
setInterval(() => {
	const currentUrl = window.location.href;
	if (currentUrl !== previousUrl) {
		isRestoring = true;
		saveCurrentScrollForUrl(previousUrl);
		previousUrl = currentUrl;

		const targetY = getStoredScroll(currentUrl);
		const navId = ++activeNavigationId;

		if (typeof targetY !== 'undefined' && targetY > 5) {
			performRestoration(targetY, navId);
		} else {
			window.scrollTo(0, 0);
			lastRestoredTime = Date.now();
			isRestoring = false;
		}
	}
}, 50);

function performRestoration(targetY, navId) {
	const restorationId = ++activeRestorationId;
	isRestoring = true;
	let stableFrames = 0;
	let lastHeight = 0;

	const attemptRestoration = () => {
		if (navId !== activeNavigationId || restorationId !== activeRestorationId) {
			return;
		}

		const docHeight = document.documentElement.scrollHeight;
		const clampedTarget = Math.max(0, Math.min(targetY, docHeight - window.innerHeight));

		window.scrollTo(0, clampedTarget);
		const currentY = window.scrollY;

		if (docHeight === lastHeight && docHeight > 0) {
			stableFrames++;
		} else {
			stableFrames = 0;
			lastHeight = docHeight;
		}

		if ((Math.abs(currentY - clampedTarget) < 2 && stableFrames >= 15) || stableFrames > 60) {
			window.scrollTo(0, clampedTarget);
			lastRestoredTime = Date.now();
			isRestoring = false;
			return;
		}

		requestAnimationFrame(attemptRestoration);
	};

	requestAnimationFrame(() => requestAnimationFrame(attemptRestoration));
}

// Track scrolling on the current active URL
window.addEventListener('scroll', () => {
	if (isRestoring || Date.now() - lastRestoredTime < 500) return;
	saveCurrentScrollForUrl(window.location.href);
}, { passive: true, capture: true });

// Handle explicit browser back/forward buttons
window.addEventListener('popstate', () => {
	const currentUrl = window.location.href;

	isRestoring = true;
	saveCurrentScrollForUrl(previousUrl);
	previousUrl = currentUrl;

	const targetY = getStoredScroll(currentUrl);
	const navId = ++activeNavigationId;

	if (typeof targetY !== 'undefined' && targetY > 5) {
		performRestoration(targetY, navId);
	} else {
		window.scrollTo(0, 0);
		lastRestoredTime = Date.now();
		isRestoring = false;
	}
});

const userInterrupt = () => {
	if (Date.now() - lastRestoredTime < 400) {
		activeRestorationId++;
		isRestoring = false;
	}
};
window.addEventListener('wheel', userInterrupt, { passive: true, capture: true });
window.addEventListener('touchstart', userInterrupt, { passive: true, capture: true });