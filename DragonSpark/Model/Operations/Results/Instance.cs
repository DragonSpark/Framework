using DragonSpark.Compose;

namespace DragonSpark.Model.Operations.Results;

public class Instance<T> : IResulting<T>
{
	readonly T _instance;

	public Instance(T instance) => _instance = instance;

	public ValueTask<T> Get() => _instance.ToOperation();
}