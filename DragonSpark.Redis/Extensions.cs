using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Redis;

public static class Extensions
{
	public static IServiceCollection WithDistributedMemory<T>(this IServiceCollection @this)
		where T : ConfigureDistributedMemory
		=> MemoryRegistrations<T>.Default.Parameter(@this);

	public static IServiceCollection WithDistributedOutputs<T>(this IServiceCollection @this)
		where T : ConfigureDistributedOutputs
		=> OutputsRegistrations<T>.Default.Parameter(@this);
}