using Azure.Storage.Queues;
using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Queues
{
	public class Queue : Result<QueueClient>, IQueue
	{
		public Queue(IQueueClients clients, string name) : base(clients.Then().Bind(name)) {}
	}
}