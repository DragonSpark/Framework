// ATTRIBUTION: https://github.com/meziantou/Meziantou.Framework/blob/5cc4602de6ca4a7cf0caf23c57259cc60e46cf83/src/Meziantou.AspNetCore.Components/wwwroot/InfiniteScrolling.ts
export function initialize(lastIndicator : HTMLElement, instance : any, direction: number) {
  const options = {
    root: findClosestScrollContainer(lastIndicator, direction),
    rootMargin: '0px',
    threshold: 0,
  };

  const observer = new IntersectionObserver(async (entries) => {
    for (const entry of entries) {
      if (entry.isIntersecting) {
        await instance.invokeMethodAsync("LoadMoreItems");
      }
    }
  }, options);

  observer.observe(lastIndicator);

  return {
    dispose: () => infiniteScollingDispose(observer),
    onNewItems: () => {
      observer.unobserve(lastIndicator);
      observer.observe(lastIndicator);
    },
  };
}

function findClosestScrollContainer(element : HTMLElement | null, direction: number) : HTMLElement | null {
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

function infiniteScollingDispose(observer : IntersectionObserver) {
  observer.disconnect();
}