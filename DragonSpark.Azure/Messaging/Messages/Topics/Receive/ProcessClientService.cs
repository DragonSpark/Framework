using Azure.Messaging.ServiceBus;
using DragonSpark.Compose;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public abstract class ProcessClientService(
	ServiceBusProcessor client,
	Func<ProcessMessageEventArgs, Task> process,
	Func<ProcessErrorEventArgs, Task> error)
{
	readonly ServiceBusProcessor                 _client  = client;
	readonly Func<ProcessMessageEventArgs, Task> _process = process;
	readonly Func<ProcessErrorEventArgs, Task>   _error   = error;

	protected ProcessClientService(ServiceBusProcessor client, ProcessEvents events)
		: this(client, events.Get, events.Get) {}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		_client.ProcessMessageAsync += _process;
		_client.ProcessErrorAsync   += _error;
		return _client.StartProcessingAsync(cancellationToken);
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		await _client.StopProcessingAsync(cancellationToken).Off();
		_client.ProcessMessageAsync -= _process;
		_client.ProcessErrorAsync   -= _error;
	}
}