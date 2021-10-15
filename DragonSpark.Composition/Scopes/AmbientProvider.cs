using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Scopes;

public sealed class AmbientProvider : IResult<IServiceProvider?>
{
	public static AmbientProvider Default { get; } = new();

	AmbientProvider() : this(LogicalScope.Default) {}

	readonly IResult<AsyncServiceScope?> _scope;

	public AmbientProvider(IResult<AsyncServiceScope?> scope) => _scope = scope;

	public IServiceProvider? Get() => _scope.Get()?.ServiceProvider;
}