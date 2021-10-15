using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using Polly;

namespace DragonSpark.Application.Entities.Diagnostics;

public static class QueryingExtensions
{
	public static IMaterializer<T, TResult> With<T, TResult>(this IMaterializer<T, TResult> @this,
	                                                         IAsyncPolicy policy)
		=> new PolicyAwareMaterializer<T, TResult>(@this, policy);
}