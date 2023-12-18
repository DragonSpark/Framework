using DragonSpark.Diagnostics;

namespace DragonSpark.Application.Connections.Client;

public sealed class ExtendedDurableConnectionPolicy : DurableConnectionPolicyBase
{
	public static ExtendedDurableConnectionPolicy Default { get; } = new();

	ExtendedDurableConnectionPolicy() : base(ExtendedRetryPolicyBuilder.Default) {}
}