using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

public class ReceiveConnection : Instance<HubConnection>, IAsyncDisposable
{
	readonly HubConnection   _connection;

	protected ReceiveConnection(IHubConnections hubs, Uri address) : this(hubs.Get(address)) {}

	protected ReceiveConnection(HubConnection connection) : base(connection) => _connection = connection;

	public async ValueTask DisposeAsync()
	{
		try
		{
			await _connection.StopAsync().ConfigureAwait(false);
		}
		finally
		{
			await _connection.DisposeAsync().ConfigureAwait(false);
		}
	}
}