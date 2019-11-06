using System;

namespace DragonSpark.Model.Sequences.Query
{
	sealed class Select<TIn, TOut> : IContent<TIn, TOut>
	{
		readonly IBody<TIn>      _body;
		readonly Assigned<uint>  _limit;
		readonly Func<TIn, TOut> _select;
		readonly IStores<TOut>   _stores;

		// ReSharper disable once TooManyDependencies
		public Select(IBody<TIn> body, IStores<TOut> stores, Func<TIn, TOut> select, Assigned<uint> limit)
		{
			_body   = body;
			_stores = stores;
			_select = select;
			_limit  = limit;
		}

		public Store<TOut> Get(Store<TIn> parameter)
		{
			var body = _body.Get(new ArrayView<TIn>(parameter.Instance, 0, parameter.Length));
			var length = _limit.IsAssigned
				             ? Math.Min(_limit.Instance, body.Length)
				             : body.Length;
			var result = _stores.Get(length);
			var @in    = body.Array;
			var @out   = result.Instance;

			for (var i = 0; i < length; i++)
			{
				@out[i] = _select(@in[i + body.Start]);
			}

			return result;
		}
	}
}