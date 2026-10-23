using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Server.Output;

sealed class MaybeTag<TIn, TOut> : IStopAware<TIn, string?>
{
	readonly IStopAware<TIn, TOut?> _input;
	readonly Func<TOut, string>     _key;

	public MaybeTag(IStopAware<TIn, TOut?> input, Func<TOut, string> key)
	{
		_input = input;
		_key   = key;
	}

	public async ValueTask<string?> Get(Stop<TIn> parameter)
		=> await _input.Off(parameter) is {} input ? _key(input) : null;
}