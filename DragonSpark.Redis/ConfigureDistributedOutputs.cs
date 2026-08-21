using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.OutputCaching.StackExchangeRedis;

namespace DragonSpark.Redis;

sealed class ConfigureDistributedOutputs : ICommand<RedisOutputCacheOptions>
{
	readonly ManagedOptions _options;
	readonly string?        _instance;

	public ConfigureDistributedOutputs(ManagedOptions options, string? instance)
	{
		_options  = options;
		_instance = instance;
	}

	public void Execute(RedisOutputCacheOptions parameter)
	{
		parameter.ConfigurationOptions = _options.Get().GetAwaiter().GetResult();
		parameter.InstanceName         = _instance;
	}
}