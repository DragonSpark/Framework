using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections.Client;

sealed class CurrentConnection : ICurrentConnection
{
	readonly IDisposable     _store;
	readonly ConnectionStore _connection;

	public CurrentConnection(IHubConnections connections, Uri location)
		: this(new ConnectionVariable(), connections, location) {}

	public CurrentConnection(ConnectionVariable store, IHubConnections connections, Uri location)
		: this(store, new ConnectionStore(store, connections, location)) {}

	public CurrentConnection(IDisposable store, ConnectionStore connection)
	{
		_store      = store;
		_connection = connection;
	}

	public HubConnection Get() => _connection.Get();

	public void Dispose()
	{
		_store.Dispose();
	}
}