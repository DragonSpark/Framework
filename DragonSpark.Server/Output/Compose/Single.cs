using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Server.Output.Compose;

sealed class Single<TIn, TKey> : IRegistration
{
	readonly Func<TIn, TKey>        _select;
	readonly IStopAware<TKey, TKey> _out;

	public Single(Func<TIn, TKey> select, IStopAware<TKey, TKey> @out)
	{
		_select = select;
		_out    = @out;
	}

	public async ValueTask Get(Stop<ComposeTagsInput> parameter)
	{
		var ((input, key, result), stop) = parameter;
		if (input is TIn @in && key is IOutputKey<TKey> k)
		{
			var select = _select(@in);
			var @out   = await _out.Off(new(select, stop));
			result.Add(k.Get(@out));
		}
	}
}