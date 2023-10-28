using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections.Client;

sealed class CreateConnection : FixedSelection<Uri, HubConnection>
{
	public CreateConnection(IHubConnections select, Uri parameter) : base(select, parameter) {}
}