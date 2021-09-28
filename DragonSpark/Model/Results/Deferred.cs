using System;

namespace DragonSpark.Model.Results
{
	public class Deferred<T> : IResult<T>
	{
		readonly IMutable<T?> _store;
		readonly Func<T>     _result;

		public Deferred(IMutable<T?> store, IResult<T> result) : this(store, result.Get) {}

		public Deferred(IMutable<T?> store, Func<T> result)
		{
			_store       = store;
			_result = result;
		}

		public T Get()
		{
			var current = _store.Get();
			if (current is null)
			{
				var result = _result();
				_store.Execute(result);
				return result;
			}
			return current;
		}
	}
}
