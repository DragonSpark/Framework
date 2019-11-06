using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class StartNode<_, T> : Instance<ISelect<_, T[]>>, INode<_, T>
	{
		readonly IPartition<T> _partition;

		public StartNode(ISelect<_, T[]> origin) : this(origin, Partition<T>.Default) {}

		public StartNode(ISelect<_, T[]> origin, IPartition<T> partition) : base(origin) => _partition = partition;

		public INode<_, T> Get(IPartition parameter) => new Open<_, T>(Get(), _partition.Get(parameter));

		public INode<_, T> Get(IBodyBuilder<T> parameter)
			=> new Node<_, T>(new Enter<_, T>(Get(), Lease<T>.Default),
			                  new PartitionedBuilder<T>(_partition, parameter));

		public INode<_, TTo> Get<TTo>(IContents<T, TTo> parameter)
			=> new ContentNode<_, T, TTo>(new Enter<_, T>(Get()),
			                              new ContentContainer<T, TTo>(_partition, parameter));

		public ISelect<_, TTo> Get<TTo>(IReduce<T, TTo> parameter)
			=> new Exit<_, T, T, TTo>(new Enter<_, T>(Get()),
			                          new Content<T>(_partition.Get(parameter is ILimitAware aware ? aware.Get() : 2)),
			                          parameter);
	}
}