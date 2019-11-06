using System;

namespace DragonSpark.Model.Sequences.Query
{
	public class Concatenation<T> : IContent<T, T>
	{
		readonly IBody<T>       _body;
		readonly Assigned<uint> _limit;
		readonly ISequence<T>   _others;
		readonly IStores<T>     _stores;

		// ReSharper disable once TooManyDependencies
		public Concatenation(IBody<T> body, ISequence<T> others, IStores<T> stores, Assigned<uint> limit)
		{
			_body   = body;
			_others = others;
			_stores = stores;
			_limit  = limit;
		}

		public Store<T> Get(Store<T> parameter)
		{
			var other  = _others.Get();
			var source = other.Instance;
			var appending = _limit.IsAssigned
				                ? Math.Min(_limit.Instance, other.Length)
				                : other.Length;

			var body = _body.Get(new ArrayView<T>(parameter.Instance, 0, parameter.Length));

			var @in    = body.Array;
			var length = body.Length;
			var result = _stores.Get(length + appending);

			var @out = @in.CopyInto(result.Instance, body.Start, body.Length);
			for (var i = 0; i < appending; i++)
			{
				@out[i + length] = source[i];
			}

			return result;
		}
	}
}