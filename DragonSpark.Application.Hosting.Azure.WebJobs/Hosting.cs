using DragonSpark.Model.Commands;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs
{
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
}