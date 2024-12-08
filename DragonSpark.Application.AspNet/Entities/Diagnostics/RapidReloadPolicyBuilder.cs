using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

sealed class RapidReloadPolicyBuilder : RetryPolicy
{
	public static RapidReloadPolicyBuilder Default { get; } = new();

	RapidReloadPolicyBuilder()
		: base(10, new LinearRetryStrategy(TimeSpan.FromMilliseconds(75)).Get,
		       Start.A.Selection<(Exception, TimeSpan)>()
		            .By.Calling(x => (x.Item1.To<DbUpdateConcurrencyException>(), x.Item2))
		            .Select(ReloadEntities.Default)
		            .Then()
		            .Allocate()
		            .Get()
		            .Get) {}
}