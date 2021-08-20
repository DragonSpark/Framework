using DragonSpark.Application.Entities.Queries.Materialize;

namespace DragonSpark.Application.Entities.Diagnostics
{
	public sealed class DurableMaterialization<T> : Materialization<T>
	{
		public static DurableMaterialization<T> Default { get; } = new DurableMaterialization<T>();

		DurableMaterialization()
			: this(PolicyAwareEntityQueries<T>.Default.Get(DurableApplicationContentPolicy.Default.Get())) {}

		public DurableMaterialization(Materialization<T> source) : this(source.Any, source.Counting, source.Sequences) {}

		public DurableMaterialization(IAny<T> any, Counting<T> counting, Sequences<T> sequences) :
			base(any, counting, sequences) {}
	}
}