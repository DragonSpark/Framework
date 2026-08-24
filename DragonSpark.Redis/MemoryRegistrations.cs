using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Redis;

sealed class MemoryRegistrations : ICommand<IServiceCollection>
{
	readonly string? _name;

	public MemoryRegistrations(string? name) => _name = name;

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<Connect>()
		         .Include(x => x.Dependencies)
		         .Singleton()
		         //
		         .Then.AddStackExchangeRedisCache(x => x.InstanceName = _name)
		         .AddOptions<RedisCacheOptions>()
		         .Configure<Connect>((to, connect) => to.ConnectionMultiplexerFactory = connect.Get);
	}
}