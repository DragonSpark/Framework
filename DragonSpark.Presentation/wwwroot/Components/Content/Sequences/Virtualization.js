var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
// ATTRIBUTION: https://github.com/meziantou/Meziantou.Framework/blob/5cc4602de6ca4a7cf0caf23c57259cc60e46cf83/src/Meziantou.AspNetCore.Components/wwwroot/InfiniteScrolling.ts
export function initialize(lastIndicator, instance, direction) {
    const options = {
        root: findClosestScrollContainer(lastIndicator, direction),
        rootMargin: '0px',
        threshold: 0,
    };
    const observer = new IntersectionObserver((entries) => __awaiter(this, void 0, void 0, function* () {
        for (const entry of entries) {
            if (entry.isIntersecting) {
                yield instance.invokeMethodAsync("LoadMoreItems");
            }
        }
    }), options);
    observer.observe(lastIndicator);
    return {
        dispose: () => infiniteScollingDispose(observer),
        onNewItems: () => {
            observer.unobserve(lastIndicator);
            observer.observe(lastIndicator);
        },
    };
}
function findClosestScrollContainer(element, direction) {
    while (element) {
        const style = getComputedStyle(element);
        var property = direction == 0 ? style.overflowY : style.overflowX;
        if (property !== 'visible') {
            return element;
        }
        element = element.parentElement;
    }
    return null;
}
function infiniteScollingDispose(observer) {
    observer.disconnect();
}
//# sourceMappingURL=Virtualization.js.map