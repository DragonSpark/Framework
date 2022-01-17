using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class DeferringStore<T> : IDeferring<T>
{
	readonly IMutable<IDeferring<T>?> _store;
	readonly IResult<IDeferring<T>>   _result;

	public DeferringStore(Func<ValueTask<T>> resulting) : this(resulting.Start().Then().Allocate()) {}

	public DeferringStore(Func<Task<T>> resulting) : this(new Variable<IDeferring<T>?>(), resulting) {}

	public DeferringStore(IMutable<IDeferring<T>?> store, Func<ValueTask<T>> resulting)
		: this(store, resulting.Start().Then().Allocate()) {}

	public DeferringStore(IMutable<IDeferring<T>?> store, Func<Task<T>> resulting)
		: this(store, new Deferred<IDeferring<T>>(store, () => new Deferring<T>(resulting))) {}

	public DeferringStore(IMutable<IDeferring<T>?> store, IResult<IDeferring<T>> result)
	{
		_store  = store;
		_result = result;
	}

	public ValueTask<T> Get() => _result.Get().Get();

	public bool Get(None parameter) => _store.Get()?.Get(None.Default) ?? false;
}