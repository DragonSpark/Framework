using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections.Client;

public class ReceiveConnection : IResult<HubConnection>, IDisposable
{
	readonly ICurrentConnection _result;

	protected ReceiveConnection(IHubConnections connections, Uri location)
		: this(new CurrentConnection(connections, location)) {}

	protected ReceiveConnection(ICurrentConnection result) => _result = result;

	public HubConnection Get() => _result.Get();

	public void Dispose()
	{
		_result.Dispose();
	}
}