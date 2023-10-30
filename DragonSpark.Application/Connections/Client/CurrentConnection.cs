using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

sealed class CurrentConnection : Result<HubConnection>, ICurrentConnection
{
	readonly IAsyncDisposable _store;

	public CurrentConnection(IHubConnections connections, Uri location)
		: this(new ConnectionVariable(), connections, location) {}

	public CurrentConnection(ConnectionVariable store, IHubConnections connections, Uri location)
		: this(store, new ConnectionStore(store, connections, location)) {}

	public CurrentConnection(IAsyncDisposable store, ConnectionStore connection) : base(connection)
		=> _store = store;

	public ValueTask DisposeAsync() => _store.DisposeAsync();
}