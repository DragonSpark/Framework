using Azure.Core;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Blobs;
using DragonSpark.Azure.Data;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events;

class Class1;

public sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Register<EventHubConfiguration>();
	}
}

public sealed class EventHubConfiguration
{
	public string Namespace { get; set; } = default!;

	public string Group { get; set; } = EventHubConsumerClient.DefaultConsumerGroupName;
}

public interface IProducer : IResult<EventHubProducerClient>;

public class Producer : Instance<EventHubProducerClient>, IProducer
{
	protected Producer(EventHubConfiguration settings, string name)
		: this(settings.Namespace, name, DefaultCredential.Default) {}

	protected Producer(string @namespace, string name, TokenCredential credential)
		: base(new(@namespace, name, credential)) {}
}

public class Send<T> : IOperation<T>
{
	readonly EventHubProducerClient _client;

	protected Send(EventHubProducerClient client) => _client = client;

	public ValueTask Get(T parameter)
	{
		var data = new EventData(BinaryData.FromObjectAsJson(parameter))
		{
			Properties = { [EventType.Default] = A.Type<T>().FullName }
		}.Yield();
		return _client.SendAsync(data).ToOperation();
	}
}

public class Send<T, U> : IOperation<T>
{
	readonly EventHubProducerClient _client;
	readonly Func<T, U>             _select;

	protected Send(EventHubProducerClient client, Func<T, U> select)
	{
		_client = client;
		_select = select;
	}

	public ValueTask Get(T parameter)
	{
		var message = _select(parameter);
		var data = new EventData(BinaryData.FromObjectAsJson(message))
		{
			Properties = { [EventType.Default] = A.Type<T>().FullName }
		}.Yield();
		return _client.SendAsync(data).ToOperation();
	}
}

public class ProcessorClient : Instance<EventProcessorClient>
{
	protected ProcessorClient(EventHubConfiguration configuration, BlobContainerClient container, string name)
		: base(new EventProcessorClient(container, configuration.Group,
		                                $"{configuration.Namespace}.servicebus.windows.net", name,
		                                DefaultCredential.Default)) {}
}

public class ProcessClientService : IHostedService
{
	readonly EventProcessorClient              _client;
	readonly Func<ProcessEventArgs, Task>      _process;
	readonly Func<ProcessErrorEventArgs, Task> _error;

	protected ProcessClientService(EventProcessorClient client, ProcessError error)
		: this(client, ProcessEvent.Default, error) {}

	protected ProcessClientService(EventProcessorClient client, ProcessEvent process, ProcessError error)
		: this(client, process.Get, error.Get) {}

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

	public Task StopAsync(CancellationToken cancellationToken)
	{
		_client.ProcessEventAsync -= _process;
		_client.ProcessErrorAsync -= _error;
		return _client.StopProcessingAsync(cancellationToken);
	}
}

public class RegistrationAwareProcessClientService : IHostedService
{
	readonly ProcessClientService            _previous;
	readonly IEnumerable<IEventRegistration> _registrations;
	readonly IEntries                        _entries;

	protected RegistrationAwareProcessClientService(ProcessClientService previous,
	                                                IEnumerable<IEventRegistration> registrations)
		: this(previous, registrations, Entries.Default) {}

	protected RegistrationAwareProcessClientService(ProcessClientService previous,
	                                                IEnumerable<IEventRegistration> registrations,
	                                                IEntries entries)
	{
		_previous      = previous;
		_registrations = registrations;
		_entries       = entries;
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		foreach (var registration in _registrations)
		{
			registration.Execute(_entries);
		}

		return _previous.StartAsync(cancellationToken);
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		_entries.Execute();
		return _previous.StopAsync(cancellationToken);
	}
}

public sealed class ProcessEvent : IAllocated<ProcessEventArgs>
{
	public static ProcessEvent Default { get; } = new();

	ProcessEvent() : this(HandleEvent.Default) {}

	readonly IOperation<ProcessEventArgs> _process;

	public ProcessEvent(IOperation<ProcessEventArgs> process) => _process = process;

	public Task Get(ProcessEventArgs parameter) => _process.Get(parameter).AsTask();
}

public sealed class ProcessError : IAllocated<ProcessErrorEventArgs>
{
	readonly Error _error;

	public ProcessError(Error error) => _error = error;

	public Task Get(ProcessErrorEventArgs parameter)
	{
		_error.Execute(parameter.Exception, parameter.PartitionId, parameter.Operation);
		return Task.CompletedTask;
	}

	public sealed class Error : LogErrorException<string, string>
	{
		public Error(ILogger<Error> logger)
			: base(logger, "An exception occurred on {Partition} while performing {Operation}") {}
	}
}

sealed class Consumer : IOperation<Guid>
{
	public ValueTask Get(Guid parameter)
	{
		return ValueTask.CompletedTask;
	}
}

sealed class EventType : Text.Text
{
	public static EventType Default { get; } = new();

	EventType() : base(nameof(EventType)) {}
}