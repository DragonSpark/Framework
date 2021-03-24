using DragonSpark.Application.Entities.Queries;
using Polly;

namespace DragonSpark.Application.Entities.Diagnostics
{
	public static class QueryingExtensions
	{
		public static IQuerying<T, TResult> With<T, TResult>(this IQuerying<T, TResult> @this, IAsyncPolicy policy)
			=> new PolicyAwareQuerying<T, TResult>(@this, policy);
	}
}