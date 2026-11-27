const timers = new WeakMap();

export function getElementHeight(element) {
    return element ? element.getBoundingClientRect().height : 0;
}

export function setElementMinHeight(element, height, resetDelayMs = 1500) {
    if (!element) return;

    // Clear any pending reset from a previous rapid navigation
    const existingTimer = timers.get(element);
    if (existingTimer) {
        clearTimeout(existingTimer);
        timers.delete(element);
    }

    if (height > 0) {
        element.style.minHeight = `${height}px`;

        if (resetDelayMs > 0) {
            const timerId = setTimeout(() => {
                element.style.minHeight = '';
                timers.delete(element);
            }, resetDelayMs);

            timers.set(element, timerId);
        }
    } else {
        element.style.minHeight = '';
    }
}