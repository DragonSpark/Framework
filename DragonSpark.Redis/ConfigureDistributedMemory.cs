using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace DragonSpark.Redis;

public sealed class ConfigureDistributedMemory : ICommand<RedisCacheOptions>
{
	readonly ManagedOptions _options;
	readonly string?        _instance;

	public ConfigureDistributedMemory(ManagedOptions options, string? instance)
	{
		_options  = options;
		_instance = instance;
	}

	public void Execute(RedisCacheOptions parameter)
	{
		parameter.ConfigurationOptions = _options.Get().GetAwaiter().GetResult();
		parameter.InstanceName         = _instance;
	}
}

// TODO

sealed class Configurations : ISelect<string?, ConfigureDistributedMemory>
{
	readonly ManagedOptions _options;

	public Configurations(ManagedOptions options) => _options = options;

	public ConfigureDistributedMemory Get(string? parameter) => new(_options, parameter);
}