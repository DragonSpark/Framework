export var jQuery = window["jQuery"];
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
    return new DocumentElement(jQuery(document.body));
}
//# sourceMappingURL=DocumentElement.js.map