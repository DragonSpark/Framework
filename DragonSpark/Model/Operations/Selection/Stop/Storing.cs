using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection.Stop;

public class Storing<TIn, TOut> : IStopAware<TIn, TOut>
{
	readonly ITable<TIn, TOut>                _store;
	readonly Func<Stop<TIn>, ValueTask<TOut>> _source;

	protected Storing(ITable<TIn, TOut> store, Func<Stop<TIn>, ValueTask<TOut>> source)
	{
		_store  = store;
		_source = source;
	}

	public async ValueTask<TOut> Get(Stop<TIn> parameter)
	{
		if (_store.TryGet(parameter, out var result))
		{
			return result;
		}

		var source = await _source(parameter).Off();
		_store.Assign(parameter, source);
		return source;
	}
}

public class ReferenceStoring<TIn, TOut> : Storing<TIn, TOut> where TIn : class where TOut : class?
{
	protected ReferenceStoring(ISelect<Stop<TIn>, ValueTask<TOut>> previous) : this(previous.Get) {}

	protected ReferenceStoring(Func<Stop<TIn>, ValueTask<TOut>> source)
		: this(new ReferenceValueTable<TIn, TOut>(), source) {}

	protected ReferenceStoring(ITable<TIn, TOut> store, Func<Stop<TIn>, ValueTask<TOut>> source) : base(store, source) {}
}