using DragonSpark.Model.Commands;
using Microsoft.Azure.WebJobs.Host;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs
{
	sealed class QueueConfiguration : ICommand<QueuesOptions>
	{
		public static QueueConfiguration Default { get; } = new QueueConfiguration();

		QueueConfiguration() : this(TimeSpan.FromSeconds(2)) {}

		readonly TimeSpan _poll;

		public QueueConfiguration(TimeSpan poll) => _poll = poll;

		public void Execute(QueuesOptions parameter)
		{
			parameter.MaxPollingInterval = _poll;
		}
	}
}