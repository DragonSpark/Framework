using DragonSpark.Compose.Model;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Application.Compose.Store.Operations
{
	public sealed class StoreContext<TIn, TOut>
	{
		readonly OperationSelector<TIn, TOut> _subject;

		public StoreContext(OperationSelector<TIn, TOut> subject) => _subject = subject;

		public MemoryStoreContext<TIn, TOut> In(IMemoryCache memory)
			=> new MemoryStoreContext<TIn, TOut>(_subject.Get(), memory);
	}
}