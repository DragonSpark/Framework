using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace DragonSpark.Redis;

public class ConfigureDistributedMemory : ICommand<RedisCacheOptions>
{
	readonly ManagedOptions _options;
	readonly string?        _instance;

	protected ConfigureDistributedMemory(ManagedOptions options, string? instance)
	{
		_options  = options;
		_instance = instance;
	}

	public void Execute(RedisCacheOptions parameter)
	{
		parameter.ConfigurationOptions = _options.Allocate().GetAwaiter().GetResult();
		parameter.InstanceName         = _instance;
	}
}