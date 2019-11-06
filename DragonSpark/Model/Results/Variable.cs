namespace DragonSpark.Model.Results
{
	public class Variable<T> : IMutable<T>
	{
		T _instance;

		public Variable(T instance = default) => _instance = instance;

		public T Get() => _instance;

		public void Execute(T parameter)
		{
			_instance = parameter;
		}
	}
}