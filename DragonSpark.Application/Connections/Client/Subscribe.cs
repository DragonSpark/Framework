using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

public class Subscribe : ISubscribe
{
	readonly IResult<HubConnection> _connection;
	readonly string    _name;

	protected Subscribe(IResult<HubConnection> connection, string name)
	{
		_connection = connection;
		_name       = name;
	}

	public ISubscription Get(Func<Task> parameter) => new Subscription(_connection, _name, parameter);
}

public class Subscribe<T> : ISubscribe<T>
{
	readonly IResult<HubConnection> _connection;
	readonly string        _name;

	protected Subscribe(IResult<HubConnection> connection, string name)
	{
		_connection = connection;
		_name       = name;
	}

	public ISubscription Get(Func<T, Task> parameter) => new Subscription<T>(_connection, _name, parameter);
}