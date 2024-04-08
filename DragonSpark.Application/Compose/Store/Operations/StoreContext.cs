using DragonSpark.Model.Selection.Stores;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Application.Compose.Store.Operations;

public sealed class StoreContext<TIn, TOut>
{
	readonly DragonSpark.Compose.Model.Operations.OperationResultSelector<TIn, TOut> _subject;

	public StoreContext(DragonSpark.Compose.Model.Operations.OperationResultSelector<TIn, TOut> subject)
		=> _subject = subject;

	public Memory.StoreContext<TIn, TOut> In(IMemoryCache memory) => new(_subject.Get(), memory);

	public Distributed.StoreContext<TIn, TOut> In(IDistributedCache store) => new(_subject.Get(), store);

	public TableStoreContext<TIn, TOut> In(ITable<string, TOut> storage) => new(_subject.Get(), storage);
}