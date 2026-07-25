using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Server.Output;
using Microsoft.AspNetCore.OutputCaching;

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
		var tag = _key.Get();
		await _output.EvictByTagAsync(tag, stop).Off();
	}
}

public class ClearOutputs<TIn, T> : IStopAware<T>
{
	readonly IOutputCacheStore _output;
	readonly Func<T, string>   _tag;

	protected ClearOutputs(IOutputCacheStore output, IOutputKey<TIn> key, Func<T, TIn> select)
		: this(output, select.Start().Select(key)) {}

	protected ClearOutputs(IOutputCacheStore output, Func<T, string> tag)
	{
		_output = output;
		_tag    = tag;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		var (subject, stop) = parameter;
		var tag = _tag(subject);
		await _output.EvictByTagAsync(tag, stop).Off();
	}
}