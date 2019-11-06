namespace DragonSpark.Model.Results
{
	public class Instance<T> : IResult<T>
	{
		readonly T _instance;

		public Instance(T instance) => _instance = instance;

		public T Get() => _instance;

		public static implicit operator T(Instance<T> instance) => instance.Get();
	}
}