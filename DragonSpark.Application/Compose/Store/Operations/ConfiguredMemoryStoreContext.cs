using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations;

public sealed class ConfiguredMemoryStoreContext<TIn, TOut> : MemoryStoreContext<TIn, TOut>
{
	readonly ICommand<ICacheEntry> _configure;

	public ConfiguredMemoryStoreContext(ISelect<TIn, ValueTask<TOut>> subject, IMemoryCache memory,
	                                    ICommand<ICacheEntry> configure)
		: base(subject, memory)
		=> _configure = configure;

	public DragonSpark.Compose.Model.Operations.OperationResultSelector<TIn, TOut> Using<T>(Func<TIn, string> key)
		=> Using(new Key<TIn>(A.Type<T>().AssemblyQualifiedName.Verify(), key).Get);

	public DragonSpark.Compose.Model.Operations.OperationResultSelector<TIn, TOut> Using(ISelect<TIn, object> key) => Using(key.Get);

	public DragonSpark.Compose.Model.Operations.OperationResultSelector<TIn, TOut> Using(Func<TIn, object> key)
		=> new Memory<TIn, TOut>(Memory,
		                         new Source<TIn, TOut>(Memory, Subject.Await, _configure.Execute).Await,
		                         key).Then();
}