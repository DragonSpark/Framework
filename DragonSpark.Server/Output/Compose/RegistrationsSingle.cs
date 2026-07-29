using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Server.Output.Compose;

sealed class RegistrationsMany<TIn, TOut, TKey> : IRegistration
{
	readonly Func<TIn, TKey>                 _select;
	readonly IStopAware<TKey, Leasing<TOut>> _out;
	readonly Func<TOut, IEnumerable<TKey>>   _keys;

	public RegistrationsMany(Func<TIn, TKey> select, IStopAware<TKey, Leasing<TOut>> @out,
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

sealed class RegistrationsSingle<TIn, TOut, TKey> : IRegistration
{
	readonly Func<TIn, TKey>                 _select;
	readonly IStopAware<TKey, Leasing<TOut>> _out;
	readonly Func<TOut, TKey>                _key;

	public RegistrationsSingle(Func<TIn, TKey> select, IStopAware<TKey, Leasing<TOut>> @out, Func<TOut, TKey> key)
	{
		_select = select;
		_out    = @out;
		_key    = key;
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
				result.Add(k.Get(_key(item)));
			}
		}
	}
}