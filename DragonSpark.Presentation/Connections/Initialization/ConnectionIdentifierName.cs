namespace DragonSpark.Presentation.Connections.Initialization;

sealed class ConnectionIdentifierName : Text.Text
{
	public static ConnectionIdentifierName Default { get; } = new();

	ConnectionIdentifierName() : base(".DragonSpark.ConnectionIdentifier") {}
}