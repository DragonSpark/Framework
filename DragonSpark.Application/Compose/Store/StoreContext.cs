using DragonSpark.Compose.Model.Selection;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Application.Compose.Store;

public sealed class StoreContext<TIn, TOut>
{
	readonly Composer<TIn, TOut> _subject;

	public StoreContext(Composer<TIn, TOut> subject) => _subject = subject;

	public MemoryStoreContext<TIn, TOut> In(IMemoryCache memory) => new(_subject.Get(), memory);
}