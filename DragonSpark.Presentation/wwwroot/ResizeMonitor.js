class ResizeMonitor {
    constructor(references = new Map(), observer = new ResizeObserver(entries => {
        for (let entry of entries) {
            const target = entry.target;
            const reference = references.get(target.dataset["ResizeMonitor.UniqueIdentifier"]);
            if (reference) {
                const size = (entry.contentBoxSize
                    ? entry.contentBoxSize.length
                        ? entry.contentBoxSize[0]
                        : entry.contentBoxSize
                    : entry.contentBoxSize);
                const value = size ? size.inlineSize : entry.contentRect.width;
                reference[0].invokeMethodAsync("UpdateSize", value);
            }
        }
    })) {
        this.references = references;
        this.observer = observer;
    }
    Count() {
        return this.references.size;
    }
    Add(reference, identifier, element) {
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
    Remove(identifier) {
        const reference = this.references.get(identifier);
        if (reference && this.references.delete(identifier)) {
            const element = reference[1];
            this.observer.unobserve(element);
            delete (element.dataset["ResizeMonitor.UniqueIdentifier"]);
        }
    }
}
ResizeMonitor.Instance = new ResizeMonitor();
export function ResizeMonitorInstance() {
    return ResizeMonitor.Instance;
}
//# sourceMappingURL=ResizeMonitor.js.map