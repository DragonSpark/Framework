using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

class SubscriptionBase : ISubscription
{
	readonly IResult<HubConnection> _connection;
	readonly ICallback              _callback;

	protected SubscriptionBase(IResult<HubConnection> connection, ICallback callback)
	{
		_connection = connection;
		_callback   = callback;
	}

	public ValueTask Get()
	{
		var connection = _connection.Get();
		_callback.Execute(connection);
		return connection.State switch
		{
			HubConnectionState.Disconnected => connection.StartAsync().ToOperation(),
			_ => ValueTask.CompletedTask
		};
	}

	public ValueTask DisposeAsync()
	{
		_callback.Dispose();
		return ValueTask.CompletedTask;
	}
}