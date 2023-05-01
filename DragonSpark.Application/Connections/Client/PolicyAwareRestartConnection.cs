using DragonSpark.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;

namespace DragonSpark.Application.Connections.Client;

sealed class PolicyAwareRestartConnection : PolicyAwareOperation<HubConnection>, IRestartConnection
{
	public PolicyAwareRestartConnection(IRestartConnection previous)
		: base(previous, DurableConnectionPolicy.Default) {}
}