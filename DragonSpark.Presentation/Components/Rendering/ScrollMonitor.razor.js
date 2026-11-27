const positions = new Map();
let activeUrl = window.location.href;
let isPopstate = false;
let isRestoring = false;

if ('scrollRestoration' in history) {
	history.scrollRestoration = 'manual';
}

function save(url, y) {
	if (!url || isRestoring) return;
	const value = Math.max(0, Math.round(y));
	positions.set(url, value);
}

function restore(targetY) {
	isRestoring = true;
	window.scrollTo(0, targetY);
	requestAnimationFrame(() => {
		window.scrollTo(0, targetY);
		requestAnimationFrame(() => {
			window.scrollTo(0, targetY);
			isRestoring = false;
		});
	});
}

function onUrlChange() {
	const nextUrl = window.location.href;
	const fromPop = isPopstate;
	isPopstate = false;
	if (nextUrl === activeUrl) return;

	if (!fromPop && !isRestoring) {
		save(activeUrl, window.scrollY);
	}

	activeUrl = nextUrl;
	const storedY = positions.get(nextUrl);

	if (fromPop && typeof storedY === 'number') {
		restore(storedY);
	} else {
		isRestoring = false;
		window.scrollTo(0, 0);
	}
}

window.addEventListener('scroll', () => save(window.location.href, window.scrollY), { passive: true });
window.addEventListener('popstate', () => { isPopstate = true; onUrlChange(); });
document.addEventListener('enhancedload', onUrlChange);