using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Presentation.Environment.Browser.Time;

sealed class InitializeClientTime : SelectingOperation<TimeSpan>
{
	public InitializeClientTime(ClientTimeOffsetStore store, ClientTimeOffset source) : base(source, store) {}
}