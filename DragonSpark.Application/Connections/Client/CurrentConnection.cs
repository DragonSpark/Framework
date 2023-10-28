using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

sealed class CurrentConnection : Result<HubConnection>, ICurrentConnection
{
	readonly IAsyncDisposable         _store;
	readonly ICommand<HubConnection?> _assign;

	public CurrentConnection(IHubConnections connections, Uri location)
		: this(new ConnectionVariable(), connections, location) {}

	public CurrentConnection(ConnectionVariable store, IHubConnections connections, Uri location)
		: this(store, store, new ConnectionStore(store, connections, location)) {}

	public CurrentConnection(IAsyncDisposable store, ICommand<HubConnection?> assign, ConnectionStore connection) :
		base(connection)
	{
		_store  = store;
		_assign = assign;
	}

	public ValueTask DisposeAsync() => _store.DisposeAsync();

	public void Execute(None parameter)
	{
		_assign.Execute(null);
	}
}