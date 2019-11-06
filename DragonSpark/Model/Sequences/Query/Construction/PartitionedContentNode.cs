using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class PartitionedContentNode<_, TIn, TOut> : INode<_, TOut>
	{
		readonly IPartition<TOut>             _body;
		readonly IContentContainer<TIn, TOut> _container;
		readonly ISelect<_, Store<TIn>>       _origin;

		public PartitionedContentNode(ISelect<_, Store<TIn>> origin, IContentContainer<TIn, TOut> container,
		                              IBodyBuilder<TOut> builder)
			: this(origin, container, new Partition<TOut>(builder)) {}

		public PartitionedContentNode(ISelect<_, Store<TIn>> origin, IContentContainer<TIn, TOut> container)
			: this(origin, container, Partition<TOut>.Default) {}

		public PartitionedContentNode(ISelect<_, Store<TIn>> origin, IContentContainer<TIn, TOut> container,
		                              IPartition<TOut> body)
		{
			_origin    = origin;
			_container = container;
			_body      = body;
		}

		public ISelect<_, TOut[]> Get() => new Exit<_, TIn, TOut>(_origin, Content(Assigned<uint>.Unassigned));

		public INode<_, TOut> Get(IPartition parameter)
			=> new PartitionedContentNode<_, TIn, TOut>(_origin, _container, _body.Get(parameter));

		public INode<_, TOut> Get(IBodyBuilder<TOut> parameter)
			=> new PartitionedContentNode<_, TIn, TOut>(_origin, _container,
			                                            new PartitionedBuilder<TOut>(_body, parameter));

		public INode<_, TTo> Get<TTo>(IContents<TOut, TTo> parameter)
		{
			var container = new LinkedContainer<TIn, TOut, TTo>(_container.Get(Leases<TOut>.Default),
			                                                    new ContentContainer<TOut, TTo>(_body, parameter));
			var result = new PartitionedContentNode<_, TIn, TTo>(_origin, container);
			return result;
		}

		public ISelect<_, TTo> Get<TTo>(IReduce<TOut, TTo> parameter)
			=> new Exit<_, TIn, TOut, TTo>(_origin, Content(parameter is ILimitAware limit ? limit.Get() : 2),
			                               parameter);

		IContent<TIn, TOut> Content(Assigned<uint> limit)
			=> new Content<TIn, TOut>(_container.Get(Leases<TOut>.Default)(Assigned<uint>.Unassigned),
			                          _body.Get(limit)).Returned();
	}
}