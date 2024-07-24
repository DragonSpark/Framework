using DragonSpark.Application.Connections.Events;
using DragonSpark.Diagnostics;
using System.ClientModel;

namespace DragonSpark.Azure.Ai;

public sealed class DurableConnectionPolicy : DurableConnectionPolicyBase<ClientResultException>
{
	public static DurableConnectionPolicy Default { get; } = new();

	DurableConnectionPolicy() : base(DefaultRetryPolicy.Default) {}
}