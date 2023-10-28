using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

public class ReceiveConnection : Result<HubConnection>, IReceiveConnection
{
	readonly ICurrentConnection _current;

	protected ReceiveConnection(IHubConnections connections, Uri location)
		: this(new CurrentConnection(connections, location)) {}

	protected ReceiveConnection(ICurrentConnection current) : base(current) => _current = current;

	public ValueTask DisposeAsync() => _current.DisposeAsync();
}