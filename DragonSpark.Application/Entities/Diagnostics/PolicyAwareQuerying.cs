using DragonSpark.Application.Entities.Queries;
using DragonSpark.Diagnostics;
using Polly;
using System.Linq;

namespace DragonSpark.Application.Entities.Diagnostics
{
	sealed class PolicyAwareQuerying<T, TResult> : PolicyAwareSelecting<IQueryable<T>, TResult>, IQuerying<T, TResult>

	{
		public PolicyAwareQuerying(IQuerying<T, TResult> previous, IAsyncPolicy policy) : base(previous, policy) {}
	}
}