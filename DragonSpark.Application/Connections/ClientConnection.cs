using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections;

public class ClientConnection : FixedSelection<Uri, HubConnection>
{
	protected ClientConnection(IHubConnections connections, Uri location) : base(connections, location) {}
}