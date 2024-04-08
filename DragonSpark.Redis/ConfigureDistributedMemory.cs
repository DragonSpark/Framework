using DragonSpark.Model.Commands;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System;

namespace DragonSpark.Redis;

public class ConfigureDistributedMemory : ICommand<RedisCacheOptions>
{
	readonly Uri     _connection;
	readonly string? _instance;

	protected ConfigureDistributedMemory(DistributedMemoryConnection connection, string? instance)
		: this(connection.Get(), instance) {}

	protected ConfigureDistributedMemory(Uri connection, string? instance)
	{
		_connection = connection;
		_instance   = instance;
	}

	public void Execute(RedisCacheOptions parameter)
	{
		parameter.Configuration = _connection.ToString();
		parameter.InstanceName  = _instance;
	}
}