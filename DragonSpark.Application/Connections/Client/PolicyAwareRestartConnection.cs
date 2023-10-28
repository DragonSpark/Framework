using DragonSpark.Diagnostics;

namespace DragonSpark.Application.Connections.Client;

sealed class PolicyAwareRestartConnection : PolicyAwareOperation<IReceiveConnection>, IRestartConnection
{
	public PolicyAwareRestartConnection(IRestartConnection previous)
		: base(previous, DurableConnectionPolicy.Default) {}
}