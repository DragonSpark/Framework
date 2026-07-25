using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Connections;

sealed class ConnectionIdentifier : StoredStructure<Guid>, IConnectionIdentifier
{
	public ConnectionIdentifier(DetermineConnectionIdentifier result) : base(result) {}
}