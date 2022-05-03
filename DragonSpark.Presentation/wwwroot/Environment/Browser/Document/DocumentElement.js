class DocumentElement {
    constructor(body) { this.body = body; }
    AddClass(name) {
        this.body.addClass(name);
    }
    RemoveClass(name) {
        this.body.removeClass(name);
    }
}
export function NewDocumentElement() {
    return new DocumentElement(window["$"](document.body));
}
//# sourceMappingURL=DocumentElement.js.map