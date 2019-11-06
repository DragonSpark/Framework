using System;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class ContentContainer<TIn, TOut> : IContentContainer<TIn, TOut>
	{
		readonly IContents<TIn, TOut> _contents;
		readonly IPartition<TIn>      _partition;

		public ContentContainer(IPartition<TIn> partition, IContents<TIn, TOut> contents)
		{
			_contents  = contents;
			_partition = partition;
		}

		public Func<Assigned<uint>, IContent<TIn, TOut>> Get(IStores<TOut> parameter)
			=> new SelectedContent<TIn, TOut>(_partition, _contents, parameter).Get;
	}
}