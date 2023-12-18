using DragonSpark.Diagnostics;

namespace DragonSpark.Application.Connections.Client;

public sealed class DurableConnectionPolicy : DurableConnectionPolicyBase
{
	public static DurableConnectionPolicy Default { get; } = new();

	DurableConnectionPolicy() : base(DefaultRetryPolicyBuilder.Default) {}
}