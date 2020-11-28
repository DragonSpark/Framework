using Azure.Storage.Queues;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Queues
{
	sealed class ValidatedQueueClients : Select<string, QueueClient>, IQueueClients
	{
		public ValidatedQueueClients(IQueueClients previous)
			: base(Start.A.Selection<string>().By.Calling(x => x.ToLowerInvariant()).Select(previous)) {}
	}
}