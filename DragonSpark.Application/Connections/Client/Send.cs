using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

public class Send : IOperation
{
	readonly Func<HubConnection> _connection;
	readonly string              _name;

	protected Send(IResult<HubConnection> connection, string name) : this(connection.Get, name) {}

	protected Send(Func<HubConnection> connection, string name)
	{
		_connection = connection;
		_name       = name;
	}

	public async ValueTask Get()
	{
		await using var connection = _connection();
		await connection.StartAsync();
		await connection.SendAsync(_name);
	}
}

public class Send<T> : IOperation<T>
{
	readonly Func<HubConnection> _connection;
	readonly string              _name;

	protected Send(IResult<HubConnection> connection, string name) : this(connection.Get, name) {}

	protected Send(Func<HubConnection> connection, string name)
	{
		_connection = connection;
		_name       = name;
	}

	public async ValueTask Get(T parameter)
	{
		await using var connection = _connection();
		await connection.StartAsync();
		await connection.SendAsync(_name, parameter);
	}
}