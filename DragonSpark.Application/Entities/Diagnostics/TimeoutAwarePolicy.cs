using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using Microsoft.Data.SqlClient;
using Polly;
using Policy = Polly.Policy;

namespace DragonSpark.Application.Entities.Diagnostics;

public sealed class TimeoutAwarePolicy : DeferredSingleton<IAsyncPolicy>
{
	public static TimeoutAwarePolicy Default { get; } = new();

	TimeoutAwarePolicy() : base(Policy.Handle<SqlException>(x => x.Number == -2)
	                                  .Start()
	                                  .Select(DefaultRetryPolicy.Default)) {}
}