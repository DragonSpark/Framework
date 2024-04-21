using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.OutputCaching.StackExchangeRedis;
using System;

namespace DragonSpark.Redis;

public class ConfigureDistributedOutputs : ICommand<RedisOutputCacheOptions>
{
	readonly Uri     _connection;
	readonly string? _instance;

	protected ConfigureDistributedOutputs(DistributedMemoryConnection connection, string? instance)
		: this(connection.Get(), instance) {}

	protected ConfigureDistributedOutputs(Uri connection, string? instance)
	{
		_connection = connection;
		_instance   = instance;
	}

	public void Execute(RedisOutputCacheOptions parameter)
	{
		parameter.Configuration = _connection.ToString();
		parameter.InstanceName  = _instance;
	}
}