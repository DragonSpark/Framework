using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public class ParsedIdentity : Allocated<string>
{
	public ParsedIdentity(IOperation<Guid> body)
		: base(Start.A.Selection<string>().By.Calling(Guid.Parse).Select(body).Then().Allocate()) {}
}