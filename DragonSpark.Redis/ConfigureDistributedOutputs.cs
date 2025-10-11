using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.OutputCaching.StackExchangeRedis;

namespace DragonSpark.Redis;

public class ConfigureDistributedOutputs : ICommand<RedisOutputCacheOptions>
{
	readonly ManagedOptions _options;
	readonly string?        _instance;

	protected ConfigureDistributedOutputs(ManagedOptions options, string? instance)
	{
		_options  = options;
		_instance = instance;
	}

	public void Execute(RedisOutputCacheOptions parameter)
	{
		parameter.ConfigurationOptions = _options.Allocate().GetAwaiter().GetResult();
		parameter.InstanceName         = _instance;
	}
}