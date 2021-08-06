using DragonSpark.Application.Entities.Queries.Materialization;
using DragonSpark.Diagnostics;
using Polly;
using System.Linq;

namespace DragonSpark.Application.Entities.Diagnostics
{
	sealed class PolicyAwareMaterializer<T, TResult> : PolicyAwareSelecting<IQueryable<T>, TResult>, IMaterializer<T, TResult>

	{
		public PolicyAwareMaterializer(IMaterializer<T, TResult> previous, IAsyncPolicy policy) : base(previous, policy) {}
	}
}