using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.OutputCaching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Redis;

sealed class OutputsRegistrations : ICommand<IServiceCollection>
{
	readonly string? _name;

	public OutputsRegistrations(string? name) => _name = name;

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<Connect>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
		         .Then.AddStackExchangeRedisOutputCache(x => x.InstanceName = _name)
		         .AddOptions<RedisOutputCacheOptions>()
		         .Configure<Connect>((to, connect) => to.ConnectionMultiplexerFactory = connect.Get);
	}
}