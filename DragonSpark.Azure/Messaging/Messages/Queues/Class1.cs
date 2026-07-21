using System;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using DragonSpark.Application;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Application.AspNet.Workers;
using DragonSpark.Compose;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Selection.Stop.Conditions;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Runtime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MessageProperties = DragonSpark.Contracts.Messaging.MessageProperties;

namespace DragonSpark.Azure.Messaging.Messages.Queues;

// TODO:
public readonly record struct ScopedInput(TimeSpan? Visibility = null, TimeSpan? Life = null);

public interface IDistributedSender : IDispatch, ISelect<ScopedInput, IScopedDispatch>;

public abstract class DistributedSender : StopAware<MessageProperties>, IDistributedSender
{
	readonly IDispatch _dispatch;

	protected DistributedSender(string name, ServiceBusConfiguration configuration)
		: this(name, configuration.Audience) {}

	protected DistributedSender(string name, string? audience) : this(new Dispatch(name, audience)) {}

	protected DistributedSender(IDispatch dispatch) : base(dispatch) => _dispatch = dispatch;

	public IScopedDispatch Get(ScopedInput parameter)
	{
		var (visibility, life) = parameter;
		return new ScopedDispatch(_dispatch, visibility, life);
	}
}

sealed class ProcessChannel : Model.Results.Instance<Channel<DistributedMessageProperties>>
{
	public static ProcessChannel Default { get; } = new();

	ProcessChannel() : base(Channel.CreateUnbounded<DistributedMessageProperties>(new()
	{
		SingleReader = true, SingleWriter = true
	})) {}
}

public interface IProcess : IStopAware<DistributedMessageProperties>;

sealed class Process : IProcess
{
	readonly CreateProcessNotification _create;

	public Process(CreateProcessNotification create) => _create = create;

	public async ValueTask Get(Stop<DistributedMessageProperties> parameter)
	{
		await _create.Off(parameter);
	}
}

sealed class SendAwareProcess : Appending<Stop<DistributedMessageProperties>>, IProcess
{
	public SendAwareProcess(IProcess previous, ISendMessage send) : base(previous, send.Ambient().Out()) {}
}

sealed class CreateProcessNotification : Saving<DistributedMessageProperties, ProcessNotification>
{
	public CreateProcessNotification(NewProcessNotification compose, Save<ProcessNotification> add)
		: base(compose, add) {}
}

sealed class NewProcessNotification : IStopAware<DistributedMessageProperties, ProcessNotification>
{
	readonly LocateExternalProcessReference _process;
	readonly ITime                          _time;

	public NewProcessNotification(LocateExternalProcessReference process) : this(process, Time.Default) {}

	public NewProcessNotification(LocateExternalProcessReference process, ITime time)
	{
		_process = process;
		_time    = time;
	}

	public async ValueTask<ProcessNotification> Get(Stop<DistributedMessageProperties> parameter)
	{
		var ((identifier, _, destination, visibility, life), stop) = parameter;
		var now = _time.Get();
		return new()
		{
			Subject     = await _process.Off(new(identifier.Value(), stop)),
			Destination = destination,
			Created     = now,
			AvailableAt = visibility.HasValue ? now + visibility.Value : now,
			Lifetime    = life
		};
	}
}

public interface ISendMessage : IStopAware<DistributedMessageProperties>;

sealed class SendMessage : ISendMessage
{
	readonly Senders                                       _senders;
	readonly ISelect<MessageProperties, ServiceBusMessage> _create;

	public SendMessage(Senders senders) : this(senders, ComposeMessage.Default) {}

	public SendMessage(Senders senders, ISelect<MessageProperties, ServiceBusMessage> create)
	{
		_senders = senders;
		_create  = create;
	}

	public ValueTask Get(Stop<DistributedMessageProperties> parameter)
	{
		var ((identifier, message, destination, visibility, life), stop) = parameter;
		var input  = _create.Get(new(new(message, identifier), visibility, life));
		var sender = _senders.Get(destination);
		return sender.SendMessageAsync(input, stop).ToOperation();
	}
}

sealed class NotificationAwareSendMessage : ISendMessage
{
	readonly ISendMessage _previous;
	readonly IScopes      _scopes;
	readonly ITime        _time;

	public NotificationAwareSendMessage(ISendMessage previous, IScopes scopes) : this(previous, scopes, Time.Default) {}

	public NotificationAwareSendMessage(ISendMessage previous, IScopes scopes, ITime time)
	{
		_previous = previous;
		_scopes   = scopes;
		_time     = time;
	}

	public async ValueTask Get(Stop<DistributedMessageProperties> parameter)
	{
		var ((identifier, _, _, _, _), stop) = parameter;
		await _previous.Off(parameter);
		using var scope = _scopes.Get();
		var       time  = _time.Get();
		await scope.Owner.Set<ProcessNotification>()
		           .Where(x => x.Id == identifier && x.Sent == null)
		           .ExecuteUpdateAsync(s => s.SetProperty(x => x.Sent, time), stop)
		           .Off();
	}
}

sealed class Senders : ReferenceValueStore<string, ServiceBusSender>
{
	public Senders(ServiceBusClient client)
		: base(Start.A.Selection<string>().By.Calling(string.Intern).Select(client.CreateSender)) {}
}

public interface IDispatch : IStopAware<MessageProperties>;

public interface IScopedDispatch : IStopAware<IdentifiedMessage>;

sealed class ScopedDispatch : IScopedDispatch
{
	readonly IDispatch _dispatch;
	readonly TimeSpan? _life, _visibility;

	public ScopedDispatch(IDispatch dispatch, TimeSpan? visibility = null, TimeSpan? life = null)
	{
		_dispatch   = dispatch;
		_life       = life;
		_visibility = visibility;
	}

	public ValueTask Get(Stop<IdentifiedMessage> parameter)
	{
		var (subject, stop) = parameter;
		return _dispatch.Get(new(new(subject, _visibility, _life), stop));
	}
}

sealed class Dispatch : IDispatch
{
	readonly string                                _name;
	readonly Channel<DistributedMessageProperties> _channel;

	public Dispatch(string name, ServiceBusConfiguration configuration) : this(name, configuration.Audience) {}

	public Dispatch(string name, string? audience) : this($"{name}{audience}", ProcessChannel.Default) {}

	public Dispatch(string name, Channel<DistributedMessageProperties> channel)
	{
		_name    = name;
		_channel = channel;
	}

	public ValueTask Get(Stop<MessageProperties> parameter)
	{
		var (((message, identifier), visibility, life), _) = parameter;
		_channel.Writer.TryWrite(new(identifier, message, _name, visibility, life));
		return ValueTask.CompletedTask;
	}
}

sealed class ChannelProcessorBackgroundService : BackgroundService
{
	readonly ChannelReader<DistributedMessageProperties> _reader;
	readonly ProcessMessage                              _process;

	public ChannelProcessorBackgroundService(ProcessMessage process)
		: this(ProcessChannel.Default, process) {}

	public ChannelProcessorBackgroundService(Channel<DistributedMessageProperties> channel, ProcessMessage process)
		: this(channel.Reader, process) {}

	public ChannelProcessorBackgroundService(ChannelReader<DistributedMessageProperties> reader, ProcessMessage process)
	{
		_reader  = reader;
		_process = process;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (await _reader.WaitToReadAsync(stoppingToken).Off())
		{
			while (_reader.TryRead(out var item) && await _process.Off(new(item, stoppingToken))) {}
		}
	}
}

sealed class ProcessMessage : IDepending<DistributedMessageProperties>
{
	readonly IProcess                _process;
	readonly ILogger<ProcessMessage> _logger;

	public ProcessMessage(IProcess process, ILogger<ProcessMessage> logger)
	{
		_process = process;
		_logger  = logger;
	}

	public async ValueTask<bool> Get(Stop<DistributedMessageProperties> parameter)
	{
		var (subject, stop) = parameter;
		try
		{
			await _process.Off(parameter);
		}
		catch (OperationCanceledException) when (stop.IsCancellationRequested)
		{
			return false;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex,
			                 "Failed to process distributed message for Notification ID {Id} targeting {Queue}",
			                 subject.Identifier, subject.Destination);
		}

		return true;
	}
}

sealed class OutboxSweeperBackgroundService : BackgroundService
{
	readonly Sweep                                   _sweep;
	readonly TimeSpan                                _pollingInterval;
	readonly ILogger<OutboxSweeperBackgroundService> _logger;

	public OutboxSweeperBackgroundService(Sweep sweep, ILogger<OutboxSweeperBackgroundService> logger)
		: this(sweep, TimeSpan.FromSeconds(15), logger) {}

	public OutboxSweeperBackgroundService(Sweep sweep, TimeSpan pollingInterval,
	                                      ILogger<OutboxSweeperBackgroundService> logger)
	{
		_sweep           = sweep;
		_pollingInterval = pollingInterval;
		_logger          = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using var timer = new PeriodicTimer(_pollingInterval);

		while (await timer.WaitForNextTickAsync(stoppingToken).Off())
		{
			try
			{
				await _sweep.Off(stoppingToken);
			}
			catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
			{
				break;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred during outbox database sweep");
			}
		}
	}
}

sealed class Sweep : IStopAware
{
	readonly ChannelWriter<DistributedMessageProperties> _writer;
	readonly EvaluateUnsentNotifications                 _unsent;
	readonly ILogger<Sweep>                              _logger;

	public Sweep(EvaluateUnsentNotifications unsent, ILogger<Sweep> logger)
		: this(ProcessChannel.Default, unsent, logger) {}

	public Sweep(Channel<DistributedMessageProperties> channel, EvaluateUnsentNotifications unsent,
	             ILogger<Sweep> logger)
		: this(channel.Writer, unsent, logger) {}

	public Sweep(ChannelWriter<DistributedMessageProperties> writer, EvaluateUnsentNotifications unsent,
	             ILogger<Sweep> logger)
	{
		_writer = writer;
		_unsent = unsent;
		_logger = logger;
	}

	public async ValueTask Get(CancellationToken parameter)
	{
		using var unsent = await _unsent.Off(new(Time.Default, parameter));
		foreach (var message in unsent)
		{
			if (!_writer.TryWrite(message))
			{
				_logger.LogWarning("ProcessChannel capacity reached while sweeping Notification ID {Id}",
				                   message.Identifier);
				break;
			}
		}
	}
}

sealed class EvaluateUnsentNotifications : EvaluateToLease<DateTimeOffset, DistributedMessageProperties>
{
	public EvaluateUnsentNotifications(IScopes scopes) : base(scopes, SelectUnsentNotifications.Default) {}
}

sealed class SelectUnsentNotifications
	: StartWhereSelection<DateTimeOffset, ProcessNotification, DistributedMessageProperties>
{
	public static SelectUnsentNotifications Default { get; } = new();

	SelectUnsentNotifications()
		: base((p, x) => x.Sent == null && x.AvailableAt <= p,
		       (d, p, q) => q.OrderBy(x => x.AvailableAt)
		                     .Take(100)
		                     .Select(x => new DistributedMessageProperties(x.Id, x.Destination,
		                                                                   x.AvailableAt - p, x.Lifetime))) {}
}

// TODO

public sealed class ProcessNotification
{
	public Guid Id { get; set; }

	public required string Destination { get; set; }

	public required ExternalProcess Subject { get; set; }

	public required DateTimeOffset Created { get; set; }

	public required DateTimeOffset AvailableAt { get; set; }

	public required TimeSpan? Lifetime { get; set; }

	public DateTimeOffset? Sent { get; set; }
}