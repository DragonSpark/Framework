using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose;

public sealed class Dependencies : IServiceTypes
{
	public static Dependencies Default { get; } = new();

	Dependencies() {}

	public IRelatedTypes Get(IServiceCollection parameter) => new DependencyRelatedTypes(parameter);
}