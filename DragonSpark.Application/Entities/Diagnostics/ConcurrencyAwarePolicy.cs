using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace DragonSpark.Application.Entities.Diagnostics
{
	public sealed class ConcurrencyAwarePolicy : DeferredSingleton<IAsyncPolicy>
	{
		public static ConcurrencyAwarePolicy Default { get; } = new ConcurrencyAwarePolicy();

		ConcurrencyAwarePolicy() : base(Policy.Handle<DbUpdateConcurrencyException>()
		                                      .Start()
		                                      .Select(ConcurrencyRetryPolicy.Default)) {}
	}
}