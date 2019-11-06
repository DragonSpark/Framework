using DragonSpark.Model.Results;

namespace DragonSpark.Model.Sequences
{
	public class DeferredSequence<T> : ISequence<T>
	{
		readonly Array<IResult<T>> _results;
		readonly IStores<T>        _stores;

		public DeferredSequence(Array<IResult<T>> results) : this(results, Leases<T>.Default) {}

		public DeferredSequence(Array<IResult<T>> results, IStores<T> stores)
		{
			_results = results;
			_stores  = stores;
		}

		public Store<T> Get()
		{
			var length = _results.Length;
			var result = _stores.Get(length);
			for (var i = 0u; i < length; i++)
			{
				result.Instance[i] = _results[i].Get();
			}

			return result;
		}
	}
}