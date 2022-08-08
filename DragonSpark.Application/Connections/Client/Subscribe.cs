using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

public class Subscribe : ISubscribe
{
	readonly HubConnection _connection;
	readonly string        _name;

	protected Subscribe(HubConnection connection, string name)
	{
		_connection = connection;
		_name       = name;
	}

	public ISubscription Get(Func<Task> parameter)
	{
		var disposable = _connection.On(_name, parameter);
		return new Subscription(_connection, disposable);
	}
}

public class Subscribe<T> : ISubscribe<T>
{
	readonly HubConnection _connection;
	readonly string        _name;

	protected Subscribe(HubConnection connection, string name)
	{
		_connection = connection;
		_name       = name;
	}

	public ISubscription Get(Func<T, Task> parameter)
	{
		var disposable = _connection.On(_name, parameter);
		return new Subscription(_connection, disposable);
	}
}