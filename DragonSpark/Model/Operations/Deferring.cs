using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Deferring<T> : IDeferring<T>
{
	readonly Lazy<Task<T>> _store;

	public Deferring(Func<ValueTask<T>> resulting) : this(resulting.Start().Then().Allocate()) {}

	public Deferring(Func<Task<T>> resulting) : this(new Lazy<Task<T>>(resulting)) {}

	public Deferring(Lazy<Task<T>> store) => _store = store;

	public bool Get(None parameter) => _store.IsValueCreated && _store.Value.IsCompletedSuccessfully;

	public ValueTask<T> Get() => _store.Value.ToOperation();
}