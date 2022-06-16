using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Polly;
using Policy = Polly.Policy;

namespace DragonSpark.Application.Entities.Diagnostics;

public sealed class ConcurrencyAwarePolicy : Deferred<IAsyncPolicy>
{
	public static ConcurrencyAwarePolicy Default { get; } = new ConcurrencyAwarePolicy();

	ConcurrencyAwarePolicy() : base(Policy.Handle<DbUpdateConcurrencyException>()
	                                      .Start()
	                                      .Select(ConcurrencyRetryPolicy.Default)) {}
}