using Azure.Storage.Queues;
using DragonSpark.Model.Commands;
using Microsoft.Azure.WebJobs.Host;

namespace DragonSpark.Application.Hosting.Azure.WebJobs
{
	sealed class DefaultQueueConfiguration : ICommand<QueuesOptions>
	{
		public static DefaultQueueConfiguration Default { get; } = new DefaultQueueConfiguration();

		DefaultQueueConfiguration() {}

		public void Execute(QueuesOptions parameter)
		{
			parameter.MessageEncoding = QueueMessageEncoding.None;
		}
	}
}