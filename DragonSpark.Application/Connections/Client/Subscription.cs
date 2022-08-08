using DragonSpark.Compose;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

sealed class Subscription : ISubscription
{
	readonly HubConnection _connection;
	readonly IDisposable   _disposable;

	public Subscription(HubConnection connection, IDisposable disposable)
	{
		_connection = connection;
		_disposable = disposable;
	}

	public ValueTask Get()
	{
		switch (_connection.State)
		{
			case HubConnectionState.Disconnected:
				return _connection.StartAsync().ToOperation();
		}
		return ValueTask.CompletedTask;
	}

	public ValueTask DisposeAsync()
	{
		_disposable.Dispose();
		return ValueTask.CompletedTask;
	}
}