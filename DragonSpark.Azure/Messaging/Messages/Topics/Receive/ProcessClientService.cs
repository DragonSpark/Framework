using Azure.Messaging.ServiceBus;
using DragonSpark.Compose;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public class ProcessClientService : IHostedService
{
	readonly ServiceBusProcessor                 _client;
	readonly Func<ProcessMessageEventArgs, Task> _process;
	readonly Func<ProcessErrorEventArgs, Task>   _error;

	protected ProcessClientService(ServiceBusProcessor client, ProcessEvents events)
		: this(client, events.Get, events.Get) {}

	protected ProcessClientService(ServiceBusProcessor client, Func<ProcessMessageEventArgs, Task> process,
	                               Func<ProcessErrorEventArgs, Task> error)
	{
		_client  = client;
		_process = process;
		_error   = error;
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		_client.ProcessMessageAsync += _process;
		_client.ProcessErrorAsync   += _error;
		return _client.StartProcessingAsync(cancellationToken);
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		await _client.StopProcessingAsync(cancellationToken).Await();
		_client.ProcessMessageAsync -= _process;
		_client.ProcessErrorAsync   -= _error;
	}
}