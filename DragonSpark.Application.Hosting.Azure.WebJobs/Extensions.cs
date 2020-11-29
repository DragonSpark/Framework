using DragonSpark.Application.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs
{
	public static class Extensions
	{
		public static ApplicationProfileContext WithQueueApplication<T>(this BuildHostContext @this) where T : QueueHost
			=> @this.WithQueueApplication<T>(QueueConfiguration.Default.Execute);

		public static ApplicationProfileContext WithQueueApplication<T>(this BuildHostContext @this,
		                                                                Action<QueuesOptions> configure)
			where T : QueueHost
			=> @this.Configure(new Hosting(configure)).Apply(QueueApplicationProfile<T>.Default);
	}

	sealed class Hosting : ICommand<IHostBuilder>
	{
		readonly Action<QueuesOptions> _queues;

		public Hosting(Action<QueuesOptions> queues) => _queues = queues;

		public void Execute(IHostBuilder parameter)
		{
			parameter.ConfigureWebJobs(x => x.AddAzureStorageCoreServices()
			                                 .AddAzureStorageBlobs()
			                                 .AddAzureStorageQueues(_queues));
		}
	}

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