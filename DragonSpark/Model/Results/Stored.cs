using DragonSpark.Compose;
using System;

namespace DragonSpark.Model.Results;

public class Stored<T> : IResult<T>
{
	readonly IMutationAware<T?> _store;
	readonly Func<T>            _result;

	public Stored(IResult<T> result) : this(new Variable<T>(), result) {}

	public Stored(IMutable<T?> store, IResult<T> result) : this(new AssignedAwareVariable<T>(store), result.Get) {}

	public Stored(Func<T> result) : this(new Variable<T>(), result) {}

	public Stored(IMutable<T?> store, Func<T> result) : this(new AssignedAwareVariable<T>(store), result) {}

	public Stored(IMutationAware<T?> store, Func<T> result)
	{
		_store  = store;
		_result = result;
	}

	public T Get()
	{
		if (_store.IsSatisfiedBy())
		{
			return _store.Get().Verify();
		}
		var result = _result();
		_store.Execute(result);
		return result;
	}
}