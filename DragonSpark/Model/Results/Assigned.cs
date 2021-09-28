using System;

namespace DragonSpark.Model.Results
{
	public class Assigned<T> : IAssigned<T>
	{
		readonly IMutable<T?> _store;
		readonly Func<T>      _result;

		public Assigned(IMutable<T?> store, IResult<T> result) : this(store, result.Get) {}

		public Assigned(IMutable<T?> store, Func<T> result)
		{
			_store  = store;
			_result = result;
		}

		public Assignment<T> Get()
		{
			var current = _store.Get();
			if (current is not null)
			{
				return new(current, false);
			}

			var result = _result();
			_store.Execute(result);
			return new(result, true);
		}
	}
}