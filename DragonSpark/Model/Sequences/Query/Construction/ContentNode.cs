using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class ContentNode<_, TIn, TOut> : INode<_, TOut>
	{
		readonly IContentContainer<TIn, TOut> _container;
		readonly ISelect<_, Store<TIn>>       _origin;

		public ContentNode(ISelect<_, Store<TIn>> origin, IContentContainer<TIn, TOut> container)
		{
			_origin    = origin;
			_container = container;
		}

		public ISelect<_, TOut[]> Get() => new Exit<_, TIn, TOut>(_origin, _container.Get(Stores<TOut>.Default)
		                                                                             .Invoke()
		                                                                             .Returned());

		public INode<_, TOut> Get(IPartition parameter)
			=> new PartitionedContentNode<_, TIn, TOut>(_origin, _container, Partition<TOut>.Default.Get(parameter));

		public INode<_, TOut> Get(IBodyBuilder<TOut> parameter)
			=> new PartitionedContentNode<_, TIn, TOut>(_origin, _container, parameter);

		public INode<_, TTo> Get<TTo>(IContents<TOut, TTo> parameter)
		{
			var container = new LinkedContents<TIn, TOut, TTo>(_container.Get(Leases<TOut>.Default), parameter);
			var result    = new ContentNode<_, TIn, TTo>(_origin, container);
			return result;
		}

		public ISelect<_, TTo> Get<TTo>(IReduce<TOut, TTo> parameter)
		{
			var content = _container.Get(Leases<TOut>.Default)(parameter is ILimitAware limit ? limit.Get() : 2)
			                        .Returned();
			var result = new Exit<_, TIn, TOut, TTo>(_origin, content, parameter);
			return result;
		}
	}
}