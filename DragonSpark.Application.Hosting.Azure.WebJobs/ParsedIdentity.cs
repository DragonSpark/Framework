using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Stop;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public class ParsedIdentity : AllocatedStopAware<string>
{
	public ParsedIdentity(IStopAware<Guid> body)
		: base(Start.A.Selection<Stop<string>>()
		            .By.Calling(x => Guid.Parse(x).Stop(x))
		            .Select(body)
		            .Then()
		            .Allocate()) {}
}