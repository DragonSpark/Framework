using Azure.Storage.Queues;
using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Queues
{
	public interface IQueueClients : ISelect<string, QueueClient> {}
}