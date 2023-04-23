using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Scopes;

public sealed class AmbientProvider : IResult<IServiceProvider?>
{
	public static AmbientProvider Default { get; } = new();

	AmbientProvider() : this(LogicalScope.Default, LogicalProvider.Default) {}

	readonly IResult<AsyncServiceScope?> _scope;
	readonly IResult<IServiceProvider?>   _store;

	public AmbientProvider(IResult<AsyncServiceScope?> scope, IResult<IServiceProvider?> store)
	{
		_store = store;
		_scope = scope;
	}

	public IServiceProvider? Get() => _scope.Get()?.ServiceProvider ?? _store.Get();
}