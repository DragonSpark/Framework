using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose;

sealed class RecursiveDependencies : IServiceTypes
{
	public static RecursiveDependencies Default { get; } = new();

	RecursiveDependencies() : this(Dependencies.Default) {}

	readonly IServiceTypes _types;

	public RecursiveDependencies(IServiceTypes types) => _types = types;

	public IRelatedTypes Get(IServiceCollection parameter) => new RecursiveRelatedTypes(_types.Get(parameter));
}