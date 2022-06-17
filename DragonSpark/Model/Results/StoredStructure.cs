using DragonSpark.Compose;
using System;

namespace DragonSpark.Model.Results;

public class StoredStructure<T> : IResult<T> where T : struct
{
	readonly IMutationAware<T?> _store;
	readonly Func<T>            _result;

	public StoredStructure(IResult<T> result) : this(new Variable<T?>(), result) {}

	public StoredStructure(IMutable<T?> store, IResult<T> result)
		: this(new AssignedAwareVariable<T?>(store), result.Get) {}

	public StoredStructure(Func<T> result) : this(new Variable<T?>(), result) {}

	public StoredStructure(IMutable<T?> store, Func<T> result) : this(new AssignedAwareVariable<T?>(store), result) {}

	public StoredStructure(IMutationAware<T?> store, Func<T> result)
	{
		_store  = store;
		_result = result;
	}

	public T Get()
	{
		if (_store.IsSatisfiedBy())
		{
			return _store.Get().Value();
		}
		var result = _result();
		_store.Execute(result);
		return result;
	}
}