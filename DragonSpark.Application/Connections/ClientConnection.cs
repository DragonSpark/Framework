using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections
{
	public class ClientConnection : Result<HubConnection>
	{
		public ClientConnection(IHubConnections connections, Uri location) : base(connections.Then().Bind(location)) {}
	}
}