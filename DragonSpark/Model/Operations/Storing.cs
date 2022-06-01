using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Storing<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ITable<TIn, TOut>          _store;
	readonly Func<TIn, ValueTask<TOut>> _source;

	protected Storing(ITable<TIn, TOut> store, Func<TIn, ValueTask<TOut>> source)
	{
		_store  = store;
		_source = source;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		if (_store.TryGet(parameter, out var result))
		{
			return result;
		}

		var source = await _source(parameter).ConfigureAwait(false);
		_store.Assign(parameter, source);
		return source;
	}
}

public class Storing<T> : IResulting<T>
{
	readonly IMutationAware<T?> _store;
	readonly AwaitOf<T>        _source;

	public Storing(IResult<ValueTask<T>> source) : this(new Variable<T>(), source) {}

	public Storing(IMutable<T?> mutable, IResult<ValueTask<T>> source)
		: this(new AssignedAwareVariable<T>(mutable), source) {}

	public Storing(IMutationAware<T?> store, IResult<ValueTask<T>> source) : this(store, source.Await) {}

	public Storing(IMutationAware<T?> store, AwaitOf<T> source)
	{
		_store  = store;
		_source = source;
	}

	public async ValueTask<T> Get()
	{
		if (_store.IsSatisfiedBy())
		{
			return _store.Get().Verify();
		}

		var result = await _source();
		_store.Execute(result);
		return result;
	}
}