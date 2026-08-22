using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Redis;

public static class Extensions
{
	public static IServiceCollection WithDistributedMemory(this IServiceCollection @this, string name)
		=> new MemoryRegistrations(name).Parameter(@this);

	public static IServiceCollection WithDistributedOutputs(this IServiceCollection @this, string name)
		=> new OutputsRegistrations(name).Parameter(@this);
}