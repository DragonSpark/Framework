using System;
using System.Linq.Expressions;

namespace DragonSpark.Model.Sequences.Query
{
	public sealed class InlineProjection<TIn, TOut> : IContent<TIn, TOut>
	{
		readonly Copy<TIn, TOut> _apply;
		readonly IBody<TIn>      _body;
		readonly Assigned<uint>  _limit;
		readonly IStores<TOut>   _stores;

		// ReSharper disable once TooManyDependencies
		public InlineProjection(IBody<TIn> body, Expression<Func<TIn, TOut>> select, IStores<TOut> stores,
		                        Assigned<uint> limit)
			: this(body, InlineSelections<TIn, TOut>.Default.Get(select).Compile(), stores, limit) {}

		// ReSharper disable once TooManyDependencies
		public InlineProjection(IBody<TIn> body, Copy<TIn, TOut> apply,
		                        IStores<TOut> stores, Assigned<uint> limit)
		{
			_body   = body;
			_apply  = apply;
			_stores = stores;
			_limit  = limit;
		}

		public Store<TOut> Get(Store<TIn> parameter)
		{
			var body = _body.Get(new ArrayView<TIn>(parameter.Instance, 0, parameter.Length));

			var result = _stores.Get(body.Length);

			var bodyStart = body.Start + _limit.Or(body.Length);
			_apply(body.Array, result.Instance, body.Start, bodyStart, 0);

			return result;
		}
	}
}