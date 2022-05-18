using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections;

public sealed class DefaultHubConnections : Select<Uri, HubConnection>, IHubConnections
{
	public static DefaultHubConnections Default { get; } = new();

	DefaultHubConnections() : base(new HubConnections(DefaultConfigureConnection.Default)) {}
}