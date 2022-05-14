using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public class QueueApplication : AllocatedOperation<string>
{
	public QueueApplication(IOperation<Guid> application)
		: base(Start.A.Selection<string>().By.Calling(Guid.Parse).Select(application).Then().Allocate()) {}
}