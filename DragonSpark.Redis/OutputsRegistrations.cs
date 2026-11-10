using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.OutputCaching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Redis;

sealed class OutputsRegistrations : ICommand<IServiceCollection>
{
	readonly string?                                           _name;
	readonly IAlteration<IServiceCollection>                   _common;
	readonly Action<RedisOutputCacheOptions, IHostEnvironment> _instance;

	public OutputsRegistrations(string? name)
		: this(name, CommonRegistrations.Default, AssignOutputs.Default.Execute) {}

	public OutputsRegistrations(string? name, IAlteration<IServiceCollection> common,
	                            Action<RedisOutputCacheOptions, IHostEnvironment> instance)
	{
		_name     = name;
		_common   = common;
		_instance = instance;
	}

	public void Execute(IServiceCollection parameter)
	{
		_common.Get(parameter)
		       .AddStackExchangeRedisOutputCache(x => x.InstanceName = _name)
		       .AddOptions<RedisOutputCacheOptions>()
		       .Configure(_instance)
		       .Configure<Connect>((to, connect) => to.ConnectionMultiplexerFactory = connect.Get);
	}
}