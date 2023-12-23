using DragonSpark.Model.Commands;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class Hosting : ICommand<IHostBuilder>
{
	readonly Action<QueuesOptions>     _queues;
	readonly Action<ServiceBusOptions> _bus;

	public Hosting(Action<QueuesOptions> queues, Action<ServiceBusOptions> bus)
	{
		_queues = queues;
		_bus    = bus;
	}

	public void Execute(IHostBuilder parameter)
	{
		parameter.ConfigureWebJobs(x => x.AddAzureStorageBlobs()
		                                 .AddAzureStorageQueues(_queues)
		                                 .AddServiceBus(_bus));
	}
}