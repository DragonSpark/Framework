using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Server.Output.Compose;

sealed class Registrations<TIn, TOut, TKey> : IRegistration
{
	readonly Func<TIn, TKey>               _select;
	readonly IStopAware<TKey, TOut>        _out;
	readonly Func<TOut, IEnumerable<TKey>> _keys;

	public Registrations(Func<TIn, TKey> select, IStopAware<TKey, TOut> @out, Func<TOut, IEnumerable<TKey>> keys)
	{
		_select = select;
		_out    = @out;
		_keys   = keys;
	}

	public async ValueTask Get(Stop<ComposeTagsInput> parameter)
	{
		var ((input, key, result), stop) = parameter;
		if (input is TIn @in && key is IOutputKey<TKey> k)
		{
			var select = _select(@in);
			var @out   = await _out.Off(new(select, stop));
			foreach (var k1 in _keys(@out))
			{
				result.Add(k.Get(k1));
			}
		}
	}
}