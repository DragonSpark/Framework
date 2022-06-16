using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class AssumedDeferring<T> : IDeferring<T>
{
	readonly IMutable<IDeferring<T>?> _store;
	readonly IResult<IDeferring<T>>   _result;

	public AssumedDeferring(Func<ValueTask<T>> resulting) : this(resulting.Start().Then().Allocate()) {}

	public AssumedDeferring(Func<Task<T>> resulting) : this(new Variable<IDeferring<T>?>(), resulting) {}

	public AssumedDeferring(IMutable<IDeferring<T>?> store, Func<ValueTask<T>> resulting)
		: this(store, resulting.Start().Then().Allocate()) {}

	public AssumedDeferring(IMutable<IDeferring<T>?> store, Func<Task<T>> resulting)
		: this(store, new Stored<IDeferring<T>>(store, () => new Deferring<T>(resulting))) {}

	public AssumedDeferring(IMutable<IDeferring<T>?> store, IResult<IDeferring<T>> result)
	{
		_store  = store;
		_result = result;
	}

	public ValueTask<T> Get() => _result.Instance();

	public bool Get(None parameter) => _store.Get()?.Get(None.Default) ?? false;
}