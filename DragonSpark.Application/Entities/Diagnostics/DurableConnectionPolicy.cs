using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using Microsoft.Data.SqlClient;
using Polly;
using System;
using Policy = Polly.Policy;

namespace DragonSpark.Application.Entities.Diagnostics;

public sealed class DurableConnectionPolicy : Deferred<IAsyncPolicy>
{
	public static DurableConnectionPolicy Default { get; } = new();

	DurableConnectionPolicy()
		: this(Start.A.Selection<SqlException>().By.Calling(x => x.Number).Select(RetryCodes.Default)) {}

	DurableConnectionPolicy(Func<SqlException, bool> code)
		: base(Policy.Handle(code)
		             .Start()
		             .Select(DefaultRetryPolicy.Default)) {}
}