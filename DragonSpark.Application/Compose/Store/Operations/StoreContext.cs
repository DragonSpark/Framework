using DragonSpark.Model.Selection.Stores;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Application.Compose.Store.Operations;

public sealed class StoreContext<TIn, TOut>
{
	readonly DragonSpark.Compose.Model.Operations.OperationResultSelector<TIn, TOut> _subject;

	public StoreContext(DragonSpark.Compose.Model.Operations.OperationResultSelector<TIn, TOut> subject)
		=> _subject = subject;

	public MemoryStoreContext<TIn, TOut> In(IMemoryCache memory) => new(_subject.Get(), memory);

	public TableStoreContext<TIn, TOut> In(ITable<string, TOut> storage) => new(_subject.Get(), storage);
}