using DragonSpark.Compose;
using System;
using System.Threading;

namespace DragonSpark.Presentation.Environment.Browser.Time;

sealed class InitializeClientTime : DragonSpark.Model.Operations.Stop.SelectingOperation<TimeSpan>
{
	public InitializeClientTime(ClientTimeOffsetStore store, ClientTimeOffset source)
		: base(source.Then().Accept<CancellationToken>().Out(), store) {}
}