using DragonSpark.Application.Entities.Queries.Materialize;
using Polly;

namespace DragonSpark.Application.Entities.Diagnostics
{
	sealed class DurableMaterialization<T> : Materialization<T>
	{
		public DurableMaterialization(IMaterialization<T> previous)
			: this(previous, DurableApplicationContentPolicy.Default.Get()) {}

		public DurableMaterialization(IMaterialization<T> previous, IAsyncPolicy policy)
			: base(new PolicyAwareMaterialization<T>(previous).Get(policy)) {}
	}
}