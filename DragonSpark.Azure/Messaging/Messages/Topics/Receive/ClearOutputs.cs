using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Text;
using Microsoft.AspNetCore.OutputCaching;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public class ClearOutputs<T> : ClearOutputs<T, T>
{
	public ClearOutputs(IOutputCacheStore output, IFormatter<T> key) : base(output, x => x, key) {}

	protected ClearOutputs(IOutputCacheStore output, Func<T, string> tag) : base(output, tag) {}
}

public class ClearOutputs<TIn, T> : IStopAware<T>
{
	readonly IOutputCacheStore _output;
	readonly Func<T, string>   _tag;

	protected ClearOutputs(IOutputCacheStore output, Func<T, TIn> select, IFormatter<TIn> key)
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