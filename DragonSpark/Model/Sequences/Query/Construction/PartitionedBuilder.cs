namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class PartitionedBuilder<T> : IBodyBuilder<T>
	{
		readonly IBodyBuilder<T> _body;
		readonly IPartition<T>   _partition;

		public PartitionedBuilder(IPartition<T> partition, IBodyBuilder<T> body)
		{
			_partition = partition;
			_body      = body;
		}

		public IBody<T> Get(Partitioning parameter)
			=> new LinkedBody<T>(_partition.Get(parameter.Limit), _body.Get(parameter));
	}
}