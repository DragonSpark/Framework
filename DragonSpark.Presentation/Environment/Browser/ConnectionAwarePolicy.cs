using DragonSpark.Compose;
using DragonSpark.Diagnostics;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class ConnectionAwarePolicy : Policy
{
	public static ConnectionAwarePolicy Default { get; } = new();

	ConnectionAwarePolicy() : base(ConnectionAwareBuilder.Default.Then().Select(IgnorePolicy.Default)) {}
}