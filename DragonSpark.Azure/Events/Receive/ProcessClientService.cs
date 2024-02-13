using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events.Receive;

public class ProcessClientService : IHostedService
{
	readonly EventProcessorClient              _client;

	protected ProcessClientService(EventProcessorClient client, ProcessEvents events)
		: this(client, events.Get, events.Get) {}

	protected ProcessClientService(EventProcessorClient client, Func<ProcessEventArgs, Task> process,
	                               Func<ProcessErrorEventArgs, Task> error)
	{
		_client                   =  client;
		_client.ProcessEventAsync += process;
		_client.ProcessErrorAsync += error;
	}

	public Task StartAsync(CancellationToken cancellationToken) => _client.StartProcessingAsync(cancellationToken);

	public Task StopAsync(CancellationToken cancellationToken) => _client.StopProcessingAsync(cancellationToken);
}