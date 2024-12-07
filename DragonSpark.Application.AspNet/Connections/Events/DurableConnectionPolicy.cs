using DragonSpark.Diagnostics;

namespace DragonSpark.Application.Connections.Events;

public sealed class DurableConnectionPolicy : DurableConnectionPolicyBase
{
	public static DurableConnectionPolicy Default { get; } = new();

	DurableConnectionPolicy() : base(DefaultRetryPolicy.Default) {}
}