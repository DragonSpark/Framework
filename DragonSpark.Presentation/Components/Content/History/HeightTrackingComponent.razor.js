// JavaScript for HeightTrackingComponent component
export function getElementHeight(element) {
    if (!element) return 0;
    return element.getBoundingClientRect().height || element.scrollHeight || 0;
}