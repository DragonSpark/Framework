using DragonSpark.Model;

namespace DragonSpark.Presentation.Connections;

sealed class InitializeConnection : IInitializeConnection
{
	readonly IConnectionIdentifier _identifier;

	public InitializeConnection(IConnectionIdentifier identifier) => _identifier = identifier;

	public void Execute(None parameter)
	{
		_identifier.Get();
	}
}