namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class Partition<T> : IPartition<T>
	{
		public static Partition<T> Default { get; } = new Partition<T>();

		Partition() : this(BodyBuilder<T>.Default) {}

		readonly IBodyBuilder<T> _builder;
		readonly Selection       _selection;

		public Partition(IBodyBuilder<T> builder) : this(builder, Selection.Default) {}

		public Partition(IBodyBuilder<T> builder, Selection selection)
		{
			_builder   = builder;
			_selection = selection;
		}

		public IBody<T> Get(Assigned<uint> parameter) => _builder.Get(new Partitioning(_selection, parameter));

		public IPartition<T> Get(IPartition parameter) => new Partition<T>(_builder, parameter.Get(_selection));
	}
}