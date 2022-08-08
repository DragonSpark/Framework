using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

public class ReceiveConnection : Instance<HubConnection>, IDisposable
{
	readonly HubConnection _instance;

	protected ReceiveConnection(IHubConnections connections, Uri location) : this(connections.Get(location)) {}

	protected ReceiveConnection(HubConnection instance) : base(instance) => _instance = instance;

	public void Dispose()
	{
		Task.Run(() => _instance.DisposeAsync().AsTask());
	}
}