using DragonSpark.Compose;

namespace DragonSpark.Presentation.Connections;

sealed class ConnectionSessionKey : DragonSpark.Text.Text
{
	public static ConnectionSessionKey Default { get; } = new();

	ConnectionSessionKey() : base(A.Type<ConnectionSessionKey>().AssemblyQualifiedName.Verify()) {}
}