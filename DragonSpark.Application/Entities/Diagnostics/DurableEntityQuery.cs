using DragonSpark.Application.Entities.Queries;
using DragonSpark.Application.Entities.Queries.Materialization;

namespace DragonSpark.Application.Entities.Diagnostics
{
	public sealed class DurableEntityQuery<T> : EntityQuery<T>
	{
		public static DurableEntityQuery<T> Default { get; } = new DurableEntityQuery<T>();

		DurableEntityQuery()
			: this(PolicyAwareEntityQueries<T>.Default.Get(DurableApplicationContentPolicy.Default.Get())) {}

		public DurableEntityQuery(EntityQuery<T> source) : this(source.Any, source.Counting, source.Materializers) {}

		public DurableEntityQuery(IAny<T> any, Counting<T> counting, Materializers<T> materializers) :
			base(any, counting, materializers) {}
	}
}