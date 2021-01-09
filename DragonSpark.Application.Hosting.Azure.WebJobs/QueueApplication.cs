using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs
{
	sealed class QueueApplication : AllocatedOperation<string>, IQueueApplication
	{
		public QueueApplication(IApplication application) : base(Start.A.Selection<string>()
		                                                              .By.Calling(Guid.Parse)
		                                                              .Select(application)
		                                                              .Then()
		                                                              .Allocate()) {}
	}
}