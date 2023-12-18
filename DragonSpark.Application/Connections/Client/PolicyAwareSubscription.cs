using DragonSpark.Diagnostics;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections.Client;

sealed class PolicyAwareSubscription : PolicyAwareOperation, ISubscription
{
	readonly ISubscription _previous;

	public PolicyAwareSubscription(ISubscription previous) : base(previous, ExtendedDurableConnectionPolicy.Default)
		=> _previous = previous;

	public ValueTask DisposeAsync() => _previous.DisposeAsync();
}