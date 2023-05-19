using DragonSpark.SyncfusionRendering.Queries;
using DragonSpark.Text;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Components;

public sealed class RequestKey : IFormatter<DataManagerRequest>
{
	readonly string                         _key;
	readonly IFormatter<DataManagerRequest> _previous;

	public RequestKey(string key) : this(key, DataManagerRequestFormatter.Default) {}

	public RequestKey(string key, IFormatter<DataManagerRequest> previous)
	{
		_key      = key;
		_previous = previous;
	}

	public string Get(DataManagerRequest parameter) => $"{_key}+{_previous.Get(parameter)}";
}