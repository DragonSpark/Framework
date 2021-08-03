using DragonSpark.Compose.Model.Operations;
using DragonSpark.Model.Selection.Stores;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Application.Compose.Store.Operations
{
	public sealed class StoreContext<TIn, TOut>
	{
		readonly OperationResultSelector<TIn, TOut> _subject;

		public StoreContext(OperationResultSelector<TIn, TOut> subject) => _subject = subject;

		public MemoryStoreContext<TIn, TOut> In(IMemoryCache memory)
			=> new MemoryStoreContext<TIn, TOut>(_subject.Get(), memory);

		public TableStoreContext<TIn, TOut> In(ITable<string, object> storage)
			=> new TableStoreContext<TIn, TOut>(_subject.Get(), storage);
	}
}