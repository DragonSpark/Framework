using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Events.Receive;

public class ProcessClientService : IHostedService
{
	readonly EventProcessorClient              _client;
	readonly Func<ProcessEventArgs, Task>      _process;
	readonly Func<ProcessErrorEventArgs, Task> _error;

	protected ProcessClientService(EventProcessorClient client, ProcessEvents events)
		: this(client, events.Get, events.Get) {}

	protected ProcessClientService(EventProcessorClient client, Func<ProcessEventArgs, Task> process,
	                               Func<ProcessErrorEventArgs, Task> error)
	{
		_client  = client;
		_process = process;
		_error   = error;
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		_client.ProcessEventAsync += _process;
		_client.ProcessErrorAsync += _error;
		return _client.StartProcessingAsync(cancellationToken);
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		await _client.StopProcessingAsync(cancellationToken).Await();
		_client.ProcessEventAsync -= _process;
		_client.ProcessErrorAsync -= _error;
	}
}