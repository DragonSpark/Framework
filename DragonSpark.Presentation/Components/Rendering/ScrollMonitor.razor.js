// JavaScript Module for ScrollMonitor component (v2.29.0-stable)
const scrollCache = {};
let lastKnownUrl = window.location.href;
let targetScrollY = null;
let isHistory = false, vaultLocked = false, restoring = false;
let lastHeight = 0, stableFrames = 0, lastAttempt = 0, lastRestored = 0;

if ('scrollRestoration' in history) history.scrollRestoration = 'manual';

function abortRestoration() {
	if (restoring && Date.now() - lastAttempt >= 80) {
		restoring = false;
		stableFrames = lastHeight = targetScrollY = 0;
		lastRestored = Date.now();
	}
}

function performRestoration(y) {
	restoring = true;
	stableFrames = lastHeight = 0;
	lastAttempt = Date.now();

	const attempt = () => {
		if (!restoring || targetScrollY === null) return;
		const h = document.documentElement.scrollHeight;
		const target = Math.max(0, Math.min(y, h - window.innerHeight));

		window.scrollTo(0, target);

		if (h === lastHeight && h > 0) stableFrames++;
		else { stableFrames = 0; lastHeight = h; }

		if ((Math.abs(window.scrollY - target) < 2 && stableFrames >= 15) || stableFrames > 60) {
			window.scrollTo(0, target);
			restoring = false;
			stableFrames = lastHeight = vaultLocked = targetScrollY = 0;
			lastRestored = Date.now();
			return;
		}
		requestAnimationFrame(attempt);
	};
	requestAnimationFrame(attempt);
}

function checkLifecycle() {
	const url = window.location.href;
	if (!restoring && url !== lastKnownUrl) {
		lastKnownUrl = url;
		if (isHistory && typeof scrollCache[url] !== 'undefined') {
			targetScrollY = scrollCache[url];
			performRestoration(targetScrollY);
		} else {
			targetScrollY = restoring = stableFrames = lastHeight = 0;
			window.scrollTo(0, 0);
		}
		isHistory = vaultLocked = false;
	}
	requestAnimationFrame(checkLifecycle);
}

window.addEventListener('scroll', (e) => {
	if (e.target !== document && e.target !== document.documentElement && e.target !== window) return;
	if (restoring) return;
	if (vaultLocked || Date.now() - lastRestored < 600) return;

	const y = window.scrollY;
	if (y > 0) scrollCache[window.location.href] = y;
}, true);

window.addEventListener('wheel', abortRestoration, { passive: true, capture: true });
window.addEventListener('touchstart', abortRestoration, { passive: true, capture: true });
window.addEventListener('popstate', () => { isHistory = true; vaultLocked = true; restoring = false; });
window.addEventListener('beforeunload', () => { vaultLocked = true; });
window.addEventListener('click', (e) => {
	const a = e.target.closest('a');
	if (a && a.href && !a.closest('.e-pager') && !a.getAttribute('href').startsWith('#') && !a.getAttribute('href').startsWith('javascript:')) {
		vaultLocked = true;
	}
}, true);

requestAnimationFrame(checkLifecycle);