// JavaScript for HeightTrackingComponent component
export function getElementHeight(element) {
    return element ? element.getBoundingClientRect().height : 0;
}

export function setElementMinHeight(element, height) {
    if (!element) return;
    element.style.minHeight = height > 0 ? `${height}px` : '';
}