namespace DragonSpark.Model.Results;

public class Instance<T> : IResult<T>
{
	public static implicit operator T(Instance<T> instance) => instance.Get();

	readonly T _instance;

	public Instance(T instance) => _instance = instance;

	public T Get() => _instance;
}