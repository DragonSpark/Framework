using DragonSpark.Model.Results;
using DragonSpark.Runtime;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Application.Components;

public sealed class ConnectionStartTime : Instance<DateTimeOffset>
{
	[UsedImplicitly]
	public ConnectionStartTime() : this(Time.Default) {}

	public ConnectionStartTime(ITime time) : base(time.Get()) {}
}