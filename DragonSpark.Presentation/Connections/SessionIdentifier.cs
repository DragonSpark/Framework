using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Presentation.Connections;

public sealed class SessionIdentifier : Instance<Guid>
{
	public SessionIdentifier() : base(Guid.NewGuid()) {}
}