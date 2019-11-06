using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class Node<_, T> : INode<_, T>
	{
		readonly ISelect<_, Store<T>> _origin;
		readonly IPartition<T>        _partition;

		public Node(ISelect<_, Store<T>> origin, IBodyBuilder<T> builder)
			: this(origin, new Partition<T>(builder)) {}

		public Node(ISelect<_, Store<T>> origin) : this(origin, Partition<T>.Default) {}

		public Node(ISelect<_, Store<T>> origin, IPartition<T> partition)
		{
			_origin    = origin;
			_partition = partition;
		}

		public ISelect<_, T[]> Get() => new Exit<_, T>(_origin, _partition.Get(Assigned<uint>.Unassigned));

		public INode<_, T> Get(IPartition parameter)
			=> new Node<_, T>(_origin, _partition.Get(parameter));

		public INode<_, T> Get(IBodyBuilder<T> parameter)
			=> new Node<_, T>(_origin, new PartitionedBuilder<T>(_partition, parameter));

		public INode<_, TTo> Get<TTo>(IContents<T, TTo> parameter)
			=> new ContentNode<_, T, TTo>(_origin, new ContentContainer<T, TTo>(_partition, parameter));

		public ISelect<_, TTo> Get<TTo>(IReduce<T, TTo> parameter)
		{
			var content = new Content<T>(_partition.Get(parameter is ILimitAware limit ? limit.Get() : 2)).Returned();
			var result  = new Exit<_, T, T, TTo>(_origin, content, parameter);
			return result;
		}
	}
}