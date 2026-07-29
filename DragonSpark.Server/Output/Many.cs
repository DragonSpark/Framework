using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using NetFabric.Hyperlinq;

namespace DragonSpark.Server.Output;

sealed class Many<TIn, TKey> : IRegistration
{
	readonly Func<TIn, TKey>               _select;
	readonly IStopAware<TKey, Lease<TKey>> _out;

	public Many(Func<TIn, TKey> select, IStopAware<TKey, Lease<TKey>> @out)
	{
		_select = select;
		_out    = @out;
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
				result.Add(k.Get(item));
			}
		}
	}
}