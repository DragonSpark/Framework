using DragonSpark.Compose.Model.Selection;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Application.Compose.Store;

public sealed class StoreContext<TIn, TOut>
{
	readonly Selector<TIn, TOut> _subject;

	public StoreContext(Selector<TIn, TOut> subject) => _subject = subject;

	public MemoryStoreContext<TIn, TOut> In(IMemoryCache memory)
		=> new MemoryStoreContext<TIn, TOut>(_subject.Get(), memory);
}