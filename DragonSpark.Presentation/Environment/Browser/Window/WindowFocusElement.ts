import { IDotNetReference } from "../../../IDotNetReference";

class WindowFocusElement {
    private window: Window;
    private callback: (this:Window, ev:FocusEvent) => any;

    constructor(window : Window, callback: (this:Window, ev:FocusEvent) => any) {
        this.window = window;
        this.callback = callback;
    }

    public Start() {
        this.window.addEventListener("focus", this.callback);
    }

	public Stop() {
        this.window.removeEventListener("focus", this.callback);
	}
}

export function NewWindowFocusElement(component: IDotNetReference): WindowFocusElement {
	return new WindowFocusElement(window, _ => component.invokeMethodAsync("OnFocus"));
}