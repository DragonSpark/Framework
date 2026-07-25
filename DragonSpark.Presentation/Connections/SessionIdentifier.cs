using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Connections;

public sealed class SessionIdentifier : Instance<Guid>
{
	public SessionIdentifier() : base(Guid.NewGuid()) {}
}