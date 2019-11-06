using System;

namespace DragonSpark.Model.Results
{
	public class Mutable<T> : IMutable<T>
	{
		readonly Func<T>   _get;
		readonly Action<T> _set;

		public Mutable(IMutable<T> mutable) : this(mutable.Execute, mutable.Get) {}

		public Mutable(Action<T> set, Func<T> get)
		{
			_set = set;
			_get = get;
		}

		public T Get() => _get();

		public void Execute(T parameter)
		{
			_set(parameter);
		}
	}
}