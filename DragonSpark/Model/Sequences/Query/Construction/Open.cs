using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class Open<_, T> : INode<_, T>
	{
		readonly ISelect<_, T[]> _origin;
		readonly IPartition<T>   _partition;

		public Open(ISelect<_, T[]> origin, IPartition<T> partition)
		{
			_origin    = origin;
			_partition = partition;
		}

		public ISelect<_, T[]> Get() => new Exit<_, T>(_origin, _partition.Get(Assigned<uint>.Unassigned));

		public INode<_, T> Get(IPartition parameter) => new Open<_, T>(_origin, _partition.Get(parameter));

		public INode<_, T> Get(IBodyBuilder<T> parameter)
			=> new Node<_, T>(new Enter<_, T>(_origin, Lease<T>.Default),
			                  new PartitionedBuilder<T>(_partition, parameter));

		public INode<_, TTo> Get<TTo>(IContents<T, TTo> parameter)
			=> new ContentNode<_, T, TTo>(new Enter<_, T>(_origin),
			                              new ContentContainer<T, TTo>(_partition, parameter));

		public ISelect<_, TTo> Get<TTo>(IReduce<T, TTo> parameter)
			=> new Exit<_, T, T, TTo>(new Enter<_, T>(_origin),
			                          new Content<T>(_partition.Get(parameter is ILimitAware limit ? limit.Get() : 2)),
			                          parameter);
	}
}