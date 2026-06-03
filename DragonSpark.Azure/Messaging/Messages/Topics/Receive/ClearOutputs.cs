using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Server.Output;
using Microsoft.AspNetCore.OutputCaching;
using System;
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
		var tag = _key.Get();
		await _output.EvictByTagAsync(tag, stop).Off();
	}
}

public class ClearOutputs<TIn, T> : IStopAware<T>
{
	readonly IOutputCacheStore _output;
	readonly IOutputKey<TIn>   _key;
	readonly Func<T, TIn>      _select;

	protected ClearOutputs(IOutputCacheStore output, IOutputKey<TIn> key, Func<T, TIn> select)
	{
		_output = output;
		_key    = key;
		_select = select;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		var (subject, stop) = parameter;
		var tag = _key.Get(_select(subject));
		await _output.EvictByTagAsync(tag, stop).Off();
	}
}