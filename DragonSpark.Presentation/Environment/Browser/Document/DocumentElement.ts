class DocumentElement {
    private body: any;

    constructor(body : any) { this.body = body; }

    public AddClass(name: string) {
        this.body.addClass(name);
    }

	public RemoveClass(name: string) {
        this.body.removeClass(name);
	}
}

export function NewDocumentElement(): DocumentElement {
	return new DocumentElement(window["jQuery"](document.body));
}