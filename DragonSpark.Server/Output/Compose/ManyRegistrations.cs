using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Server.Output.Compose;

sealed class ManyRegistrations<TIn, TOut, TKey> : IRegistration
{
	readonly Func<TIn, TKey>                 _select;
	readonly IStopAware<TKey, Leasing<TOut>> _out;
	readonly Func<TOut, IEnumerable<TKey>>   _keys;

	public ManyRegistrations(Func<TIn, TKey> select, IStopAware<TKey, Leasing<TOut>> @out,
	                         Func<TOut, IEnumerable<TKey>> keys)
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
			var       select = _select(@in);
			using var @out   = await _out.Off(new(select, stop));
			foreach (var item in @out)
			{
				foreach (var k1 in _keys(item))
				{
					result.Add(k.Get(k1));
				}
			}
		}
	}
}