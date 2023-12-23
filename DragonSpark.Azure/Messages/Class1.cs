using Azure.Core;
using Azure.Messaging.ServiceBus;
using DragonSpark.Azure.Data;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messages;

public sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Register<ServiceBusConfiguration>().Start<ServiceBusClient>().Use<Client>().Singleton();
	}
}

public sealed class ServiceBusConfiguration
{
	public string Namespace { get; set; } = default!;

	public ServiceBusTransportType TransportType { get; set; } = ServiceBusTransportType.AmqpWebSockets;
}

public interface IClient : IResult<ServiceBusClient>;

sealed class Client : Instance<ServiceBusClient>, IClient
{
	public Client(ServiceBusConfiguration settings) : this(settings.Namespace, settings.TransportType) {}

	public Client(string @namespace, ServiceBusTransportType type)
		: this(@namespace, new() { TransportType = type }, DefaultCredential.Default) {}

	public Client(string @namespace, ServiceBusClientOptions options, TokenCredential credential)
		: base(new(@namespace, credential, options)) {}
}

public interface ISender : IResult<ServiceBusSender>;

public class Sender : Instance<ServiceBusSender>, ISender
{
	protected Sender(ServiceBusClient client, string name) : base(client.CreateSender(name)) {}
}

public interface ISend : IOperation<string>;

sealed class Send : ISend
{
	readonly ServiceBusSender                   _client;
	readonly ISelect<string, ServiceBusMessage> _message;

	public Send(ServiceBusSender client, TimeSpan? life, TimeSpan? fromNow)
		: this(client, new CreateMessageFromContent(life, fromNow)) {}

	public Send(ServiceBusSender client, ISelect<string, ServiceBusMessage> message)

	{
		_client  = client;
		_message = message;
	}

	public ValueTask Get(string parameter)
	{
		var message = _message.Get(parameter);
		return _client.SendMessageAsync(message).ToOperation();
	}
}

sealed class Message : IMessage
{
	readonly ServiceBusSender                         _client;
	readonly ISelect<MessageInput, ServiceBusMessage> _create;

	public Message(ServiceBusSender client) : this(client, CreateMessage.Default) {}

	public Message(ServiceBusSender client, ISelect<MessageInput, ServiceBusMessage> create)
	{
		_client = client;
		_create = create;
	}

	public ValueTask Get(MessageInput parameter)
	{
		var message = _create.Get(parameter);
		return _client.SendMessageAsync(message).ToOperation();
	}
}

public interface IMessage : IOperation<MessageInput>;

sealed class CreateMessageFromContent : ISelect<string, ServiceBusMessage>
{
	readonly TimeSpan?                                _life, _fromNow;
	readonly ISelect<MessageInput, ServiceBusMessage> _create;

	public CreateMessageFromContent(TimeSpan? life, TimeSpan? fromNow) : this(life, fromNow, CreateMessage.Default) {}

	public CreateMessageFromContent(TimeSpan? life, TimeSpan? fromNow, ISelect<MessageInput, ServiceBusMessage> create)
	{
		_life    = life;
		_fromNow = fromNow;
		_create  = create;
	}

	public ServiceBusMessage Get(string parameter) => _create.Get(new(parameter, _fromNow, _life));
}

sealed class CreateMessage : ISelect<MessageInput, ServiceBusMessage>
{
	public static CreateMessage Default { get; } = new();

	CreateMessage() : this(Time.Default) {}

	readonly ITime _time;

	public CreateMessage(ITime time) => _time = time;

	public ServiceBusMessage Get(MessageInput parameter)
	{
		var (content, visibility, life) = parameter;
		var result = new ServiceBusMessage(content);

		if (visibility is not null)
		{
			result.ScheduledEnqueueTime = _time.Get().Add(visibility.Value);
		}

		if (life is not null)
		{
			result.TimeToLive = life.Value;
		}

		return result;
	}
}