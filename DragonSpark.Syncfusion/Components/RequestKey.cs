using DragonSpark.Compose;
using DragonSpark.SyncfusionRendering.Queries;
using DragonSpark.Text;
using Syncfusion.Blazor;
using System;

namespace DragonSpark.SyncfusionRendering.Components;

public sealed class RequestKey : IFormatter<DataManagerRequest>
{
	readonly Func<string>                   _key;
	readonly IFormatter<DataManagerRequest> _previous;

	public RequestKey(string key) : this(key.Self) {}

	public RequestKey(Func<string> key) : this(key, DataManagerRequestFormatter.Default) {}

	public RequestKey(Func<string> key, IFormatter<DataManagerRequest> previous)
	{
		_key      = key;
		_previous = previous;
	}

	public string Get(DataManagerRequest parameter) => $"{_key()}+{_previous.Get(parameter)}";
}