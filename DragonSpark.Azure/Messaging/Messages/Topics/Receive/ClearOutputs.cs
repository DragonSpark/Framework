using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Server.Output;
using Microsoft.AspNetCore.OutputCaching;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

sealed class ClearOutputs<T> : IStopAware<T>
{
	readonly IOutputCacheStore _output;
	readonly IOutputKey        _key;

	public ClearOutputs(IOutputCacheStore output, IOutputKey key)
	{
		_output = output;
		_key    = key;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		var (_, stop) = parameter;
		var tag = _key.Get(None.Default);
		await _output.EvictByTagAsync(tag, stop).Off();
	}
}
