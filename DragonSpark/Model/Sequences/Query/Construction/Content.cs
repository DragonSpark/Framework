namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class Content<T> : IContent<T, T>
	{
		readonly IBody<T>   _body;
		readonly IStores<T> _stores;

		public Content(IBody<T> body) : this(body, Leases<T>.Default) {}

		public Content(IBody<T> body, IStores<T> stores)
		{
			_body   = body;
			_stores = stores;
		}

		public Store<T> Get(Store<T> parameter)
		{
			var view   = _body.Get(new ArrayView<T>(parameter.Instance, 0, parameter.Length));
			var result = view.Start > 0 || view.Length != parameter.Length ? view.ToStore(_stores) : view.Array;
			return result;
		}
	}

	sealed class Content<TIn, TOut> : IContent<TIn, TOut>
	{
		readonly IBody<TOut>         _body;
		readonly IContent<TIn, TOut> _content;
		readonly IStores<TOut>       _stores;

		public Content(IContent<TIn, TOut> content, IBody<TOut> body) : this(content, body, Stores<TOut>.Default) {}

		public Content(IContent<TIn, TOut> content, IBody<TOut> body, IStores<TOut> stores)
		{
			_content = content;
			_body    = body;
			_stores  = stores;
		}

		public Store<TOut> Get(Store<TIn> parameter)
		{
			var content = _content.Get(parameter);
			var view    = _body.Get(new ArrayView<TOut>(content.Instance, 0, content.Length));
			var result  = view.ToStore(_stores);
			return result;
		}
	}
}