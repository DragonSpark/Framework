using System;

namespace DragonSpark.Presentation.Environment.Browser.Time;

public sealed class ClientOffsetAssignedMessage : DragonSpark.Model.Results.Instance<TimeSpan>
{
	public ClientOffsetAssignedMessage(TimeSpan instance) : base(instance) {}
}