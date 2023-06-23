using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using System;

namespace DragonSpark.Presentation.Environment.Browser.Time;

sealed class ClientTimeOffset : Resulting<TimeSpan>
{
	public ClientTimeOffset(LoadModule<ClientTimeOffset> load) : this(load, GetClientTimeOffset.Default) {}

	public ClientTimeOffset(LoadModule<ClientTimeOffset> load, GetClientTimeOffset get)
		: base(load.Then().Select(get)) {}
}