namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class SelectedContent<TIn, TOut> : ISelectedContent<TIn, TOut>
	{
		readonly IContents<TIn, TOut> _contents;
		readonly IPartition<TIn>      _partition;
		readonly IStores<TOut>        _stores;

		public SelectedContent(IContents<TIn, TOut> contents, IStores<TOut> stores)
			: this(Partition<TIn>.Default, contents, stores) {}

		public SelectedContent(IPartition<TIn> partition, IContents<TIn, TOut> contents, IStores<TOut> stores)
		{
			_contents  = contents;
			_partition = partition;
			_stores    = stores;
		}

		public IContent<TIn, TOut> Get(Assigned<uint> parameter)
			=> _contents.Get(new Parameter<TIn, TOut>(_partition.Get(parameter), _stores, parameter));
	}
}