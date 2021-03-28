using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities.Diagnostics
{
	sealed class ConcurrencyRetryPolicy : RetryPolicy
	{
		public static ConcurrencyRetryPolicy Default { get; } = new ConcurrencyRetryPolicy();

		ConcurrencyRetryPolicy() : base(10, new LinearRetryStrategy(TimeSpan.FromMilliseconds(100)).Get,
		                                Start.A.Selection<(Exception, TimeSpan)>()
		                                     .By.Calling(x => (x.Item1.To<DbUpdateConcurrencyException>(), x.Item2))
		                                     .Select(ReloadEntities.Default)
		                                     .Then()
		                                     .Allocate()
		                                     .Get()
		                                     .Get) {}
	}
}