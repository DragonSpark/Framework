using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Query
{
	public class SelectMany<TIn, TOut> : IContent<TIn, TOut>
	{
		readonly IBody<TIn>                   _body;
		readonly IIterate<TOut>               _iterate;
		readonly Assigned<uint>               _limit;
		readonly Func<TIn, IEnumerable<TOut>> _project;
		readonly IStores<TOut>                _stores;

		// ReSharper disable once TooManyDependencies
		public SelectMany(IBody<TIn> body, Func<TIn, IEnumerable<TOut>> project, IStores<TOut> stores,
		                  Assigned<uint> limit)
			: this(body, project, stores, Iterate<TOut>.Default, limit) {}

		// ReSharper disable once TooManyDependencies
		public SelectMany(IBody<TIn> body, Func<TIn, IEnumerable<TOut>> project, IStores<TOut> stores,
		                  IIterate<TOut> iterate, Assigned<uint> limit)
		{
			_body    = body;
			_project = project;
			_stores  = stores;
			_iterate = iterate;
			_limit   = limit;
		}

		public Store<TOut> Get(Store<TIn> parameter)
		{
			var body = _body.Get(new ArrayView<TIn>(parameter.Instance, 0, parameter.Length));
			var length = body.Start + (_limit.IsAssigned
				                           ? Math.Min(_limit.Instance, body.Length)
				                           : body.Length);

			var @in   = body.Array;
			var store = new DynamicStore<TOut>(1024);
			for (var i = body.Start; i < length; i++)
			{
				var enumerable = _project(@in[i]);
				var page       = _iterate.Get(enumerable);
				store = store.Add(in page);
			}

			var result = store.Get(_stores);
			return result;
		}
	}
}