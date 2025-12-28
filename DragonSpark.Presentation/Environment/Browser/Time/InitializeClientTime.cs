using System;

namespace DragonSpark.Presentation.Environment.Browser.Time;

sealed class InitializeClientTime : DragonSpark.Model.Operations.Stop.SelectingOperation<TimeSpan>
{
	public InitializeClientTime(ClientTimeOffsetStore store, ClientTimeOffset source) : base(source, store) {}
}