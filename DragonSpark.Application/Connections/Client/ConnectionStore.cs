using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections.Client;

sealed class ConnectionStore : Stored<HubConnection>
{
	public ConnectionStore(ConnectionVariable variable, IHubConnections connections, Uri location)
		: this(new ConnectionAware(variable), connections, location) {}

	public ConnectionStore(ConnectionAware store, IHubConnections connections, Uri location)
		: base(store, new CreateConnection(connections, location).Get) {}
}