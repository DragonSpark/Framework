using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Redis;

sealed class MemoryRegistrations : ICommand<IServiceCollection>
{
	readonly string?                                     _name;
	readonly IAlteration<IServiceCollection>             _common;
	readonly Action<RedisCacheOptions, IHostEnvironment> _instance;

	public MemoryRegistrations(string? name)
		: this(name, CommonRegistrations.Default, AssignMemory.Default.Execute) {}

	public MemoryRegistrations(string? name, IAlteration<IServiceCollection> common,
	                           Action<RedisCacheOptions, IHostEnvironment> instance)
	{
		_name     = name;
		_common   = common;
		_instance = instance;
	}

	public void Execute(IServiceCollection parameter)
	{
		_common.Get(parameter)
		       .AddStackExchangeRedisCache(x => x.InstanceName = _name)
		       .AddOptions<RedisCacheOptions>()
		       .Configure(_instance)
		       .Configure<Connect>((to, connect) => to.ConnectionMultiplexerFactory = connect.Get);
	}
}