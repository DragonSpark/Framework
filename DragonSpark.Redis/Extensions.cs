using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Redis;

public static class Extensions
{
	public static IServiceCollection WithDistributedMemory<T>(this IServiceCollection @this)
		where T : ConfigureDistributedMemory
		=> Registrations<T>.Default.Parameter(@this);
}