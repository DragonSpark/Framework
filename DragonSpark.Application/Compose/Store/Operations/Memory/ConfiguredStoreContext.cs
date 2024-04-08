using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations.Memory;

public sealed class ConfiguredStoreContext<TIn, TOut> : StoreContext<TIn, TOut>
{
	readonly ICommand<ICacheEntry> _configure;

	public ConfiguredStoreContext(ISelect<TIn, ValueTask<TOut>> subject, IMemoryCache memory)
		: this(subject, memory, EmptyCommand<ICacheEntry>.Default) {}

	public ConfiguredStoreContext(ISelect<TIn, ValueTask<TOut>> subject, IMemoryCache memory,
	                              ICommand<ICacheEntry> configure)
		: base(subject, memory)
		=> _configure = configure;

	public DragonSpark.Compose.Model.Operations.OperationResultSelector<TIn, TOut> Using<T>()
		=> Using(A.Type<T>().AssemblyQualifiedName.Verify().Accept);

	public DragonSpark.Compose.Model.Operations.OperationResultSelector<TIn, TOut> Using<T>(Func<TIn, string> key)
		=> Using(new Key<TIn>(A.Type<T>().AssemblyQualifiedName.Verify(), key).Get);

	public DragonSpark.Compose.Model.Operations.OperationResultSelector<TIn, TOut> Using(ISelect<TIn, object> key)
		=> Using(key.Get);

	public DragonSpark.Compose.Model.Operations.OperationResultSelector<TIn, TOut> Using(Func<TIn, object> key)
		=> new Memory<TIn, TOut>(Memory, new Load<TIn, TOut>(Memory, Subject.Await, _configure.Execute).Await, key)
		   .Then()
		   .Protecting();
}