using DragonSpark.Compose;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

class SubscriptionBase : ISubscription
{
	readonly IDisposable   _disposable;
	readonly HubConnection _connection;

	protected SubscriptionBase(HubConnection connection, IDisposable disposable)
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