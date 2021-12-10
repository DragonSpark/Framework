using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public interface IDeferring<T> : IResulting<T>, ICondition {}

public class Deferring<T> : IDeferring<T>
{
	readonly Lazy<Task<T>> _store;

	public Deferring(Func<ValueTask<T>> resulting) : this(resulting.Start().Then().Allocate()) {}

	public Deferring(Func<Task<T>> resulting) : this(new Lazy<Task<T>>(resulting)) {}

	public Deferring(Lazy<Task<T>> store) => _store = store;

	public bool Get(None parameter) => _store.IsValueCreated;

	public ValueTask<T> Get() => _store.Value.ToOperation();
}

/*public class Deferring<T> : IResulting<T>
{
	readonly IMutable<T?> _store;
	readonly AwaitOf<T>   _result;

	public Deferring(IMutable<T?> store, IResulting<T> result) : this(store, result.Await) {}

	public Deferring(IMutable<T?> store, AwaitOf<T> result)
	{
		_store  = store;
		_result = result;
	}

	public async ValueTask<T> Get()
	{
		var current = _store.Get();
		if (current is null)
		{
			var result = await _result();
			_store.Execute(result);
			return result;
		}
		return current;
	}
}*/