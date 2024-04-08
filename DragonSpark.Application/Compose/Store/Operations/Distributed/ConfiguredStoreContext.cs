using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations.Distributed;

public sealed class ConfiguredStoreContext<TIn, TOut> : StoreContext<TIn, TOut>
{
	readonly ICommand<DistributedCacheEntryOptions> _configure;

	public ConfiguredStoreContext(ISelect<TIn, ValueTask<TOut>> subject, IDistributedCache memory,
	                              ICommand<DistributedCacheEntryOptions> configure)
		: base(subject, memory)
		=> _configure = configure;

	public DragonSpark.Compose.Model.Operations.OperationResultSelector<TIn, TOut> Using<T>()
		=> Using(A.Type<T>().FullName.Verify().Accept);

	public DragonSpark.Compose.Model.Operations.OperationResultSelector<TIn, TOut> Using<T>(Func<TIn, string> key)
		=> Using(new Key<TIn>(A.Type<T>().FullName.Verify(), key).Get);

	public DragonSpark.Compose.Model.Operations.OperationResultSelector<TIn, TOut> Using(ISelect<TIn, string> key)
		=> Using(key.Get);

	public DragonSpark.Compose.Model.Operations.OperationResultSelector<TIn, TOut> Using(Func<TIn, string> key)
		=> new Distributed<TIn, TOut>(Memory, key, new Load<TIn, TOut>(Memory, Subject.Await, _configure).Await).Then();
}