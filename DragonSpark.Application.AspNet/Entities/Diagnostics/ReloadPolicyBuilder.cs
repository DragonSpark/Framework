using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

class ReloadPolicyBuilder : RetryPolicy
{
	protected ReloadPolicyBuilder(Func<(DbUpdateConcurrencyException, TimeSpan), ValueTask> reload)
		: base(Start.A.Selection<(Exception, TimeSpan)>()
		            .By.Calling(x => (x.Item1.To<DbUpdateConcurrencyException>(), x.Item2))
		            .Select(reload)
		            .Then()
		            .Allocate()
		            .Get()
		            .Get) {}
}