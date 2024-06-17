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

	public ConfiguredStoreContext<TIn, TOut> Append(ICommand<ICacheEntry> parameter) => Append(parameter.Execute);

	public ConfiguredStoreContext<TIn, TOut> Append(Action<ICacheEntry> parameter)
		=> new(Subject, Memory, _configure.Then().Append(parameter).Get());

	public ConfiguredStoreContext<TIn, TOut> OnEviction(ICommand<PostEvictionInput> parameter)
		=> OnEviction(parameter.Execute);
	public ConfiguredStoreContext<TIn, TOut> OnEviction(Action<PostEvictionInput> parameter)
		=> Append(x => x.RegisterPostEvictionCallback(new Adapter(parameter).Execute));

	public DragonSpark.Compose.Model.Operations.OperationResultComposer<TIn, TOut> Using<T>()
		=> Using(A.Type<T>().AssemblyQualifiedName.Verify().Accept);

	public DragonSpark.Compose.Model.Operations.OperationResultComposer<TIn, TOut> Using<T>(Func<TIn, string> key)
		=> Using(new Key<TIn>(A.Type<T>().AssemblyQualifiedName.Verify(), key).Get);

	public DragonSpark.Compose.Model.Operations.OperationResultComposer<TIn, TOut> Using(ISelect<TIn, object> key)
		=> Using(key.Get);

	public DragonSpark.Compose.Model.Operations.OperationResultComposer<TIn, TOut> Using(Func<TIn, object> key)
		=> new Memory<TIn, TOut>(Memory, new Load<TIn, TOut>(Memory, Subject.Await, _configure.Execute).Await, key)
		   .Then()
		   .Protecting();
}

// TODO

sealed class Adapter : Command<PostEvictionInput>
{
	public Adapter(ICommand<PostEvictionInput> command) : base(command) {}

	public Adapter(Action<PostEvictionInput> command) : base(command) {}

	// ReSharper disable once TooManyArguments
	public void Execute(object key, object? value, EvictionReason reason, object? state)
	{
		Execute(new(key, value, reason, state));
	}
}

public readonly record struct PostEvictionInput(object Key, object? Value, EvictionReason Reason, object? State);