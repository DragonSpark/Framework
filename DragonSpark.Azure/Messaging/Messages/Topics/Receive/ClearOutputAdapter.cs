using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Server.Output;
using Microsoft.AspNetCore.OutputCaching;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

sealed class ClearOutputAdapter<T> : IStopAware<T>
{
	readonly IOutputCacheStore _output;
	readonly IOutputKey        _key;

	public ClearOutputAdapter(IOutputCacheStore output, IOutputKey key)
	{
		_output = output;
		_key    = key;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		var (_, stop) = parameter;
		var tag = _key.Get();
		await _output.EvictByTagAsync(tag, stop).Off();
	}
}