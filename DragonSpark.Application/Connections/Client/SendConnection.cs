using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections.Client;

public class SendConnection : FixedSelection<Uri, HubConnection>
{
	protected SendConnection(IHubConnections connections, Uri location) : base(connections, location) {}
}