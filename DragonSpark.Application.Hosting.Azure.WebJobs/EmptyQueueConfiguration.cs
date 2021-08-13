using DragonSpark.Model.Commands;
using Microsoft.Azure.WebJobs.Host;

namespace DragonSpark.Application.Hosting.Azure.WebJobs
{
	sealed class EmptyQueueConfiguration : ICommand<QueuesOptions>
	{
		public static EmptyQueueConfiguration Default { get; } = new EmptyQueueConfiguration();

		EmptyQueueConfiguration() {}

		public void Execute(QueuesOptions parameter) {}
	}
}