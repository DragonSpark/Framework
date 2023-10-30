namespace DragonSpark.Application.Connections;

public sealed class SignedHubConnections : HubConnections
{
	public SignedHubConnections(IAssignSignedContent configure) : base(configure.Execute) {}
}