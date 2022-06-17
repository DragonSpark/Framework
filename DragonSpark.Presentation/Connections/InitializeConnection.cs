using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Connections;

sealed class InitializeConnection : IInitializeConnection
{
	readonly IConnectionIdentifier    _identifier;

	public InitializeConnection(IConnectionIdentifier identifier) => _identifier = identifier;

	public void Execute(HttpContext parameter)
	{
		_identifier.Get();
	}
}