import { IDotNetReference } from "./IDotNetReference";

class ResizeMonitor {
	static readonly Instance = new ResizeMonitor();

	private readonly observer: ResizeObserver;
	private readonly references: Map<string, [IDotNetReference, HTMLElement]>;

	constructor(references: Map<string, [IDotNetReference, HTMLElement]> = new Map<string, [IDotNetReference, HTMLElement]>(),
		observer: ResizeObserver = new ResizeObserver(entries => {
			for (let entry of entries) {
				const target = entry.target as HTMLElement;
				const reference = references.get(target.dataset["ResizeMonitor.UniqueIdentifier"]);
				if (reference) {
					const size = (entry.contentBoxSize
						? entry.contentBoxSize.length
						? entry.contentBoxSize[0]
						: entry.contentBoxSize
						: entry.contentBoxSize) as ResizeObserverSize;
					const value = size ? size.inlineSize : entry.contentRect.width;
					reference[0].invokeMethodAsync("UpdateSize", value);
				}
			}
		})) {

		this.references = references;
		this.observer = observer;
	}

	public Count(): number {
		return this.references.size;
	}

	public Add(reference: IDotNetReference, identifier: string, element: HTMLElement): boolean {
		if (element.dataset) {
			const current = element.dataset["ResizeMonitor.UniqueIdentifier"];
			if (current) {
				this.Remove(current);
			}
			element.dataset["ResizeMonitor.UniqueIdentifier"] = identifier;
			this.references.set(identifier, [reference, element]);
			this.observer.observe(element);
			return true;
		}
		return false;
	}

	public Remove(identifier: string): void {
		const reference = this.references.get(identifier);
		if (reference && this.references.delete(identifier)) {
			const element = reference[1];
			this.observer.unobserve(element);
			delete(element.dataset["ResizeMonitor.UniqueIdentifier"]);
		}
	}
}
export function ResizeMonitorInstance(): ResizeMonitor {
	return ResizeMonitor.Instance;
}