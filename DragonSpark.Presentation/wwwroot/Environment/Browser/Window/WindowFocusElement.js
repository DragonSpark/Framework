class WindowFocusElement {
    constructor(window, callback) {
        this.window = window;
        this.callback = callback;
    }
    Start() {
        this.window.addEventListener("focus", this.callback);
    }
    Stop() {
        this.window.removeEventListener("focus", this.callback);
    }
}
export function NewWindowFocusElement(component) {
    return new WindowFocusElement(window, _ => component.invokeMethodAsync("OnFocus"));
}
//# sourceMappingURL=WindowFocusElement.js.map