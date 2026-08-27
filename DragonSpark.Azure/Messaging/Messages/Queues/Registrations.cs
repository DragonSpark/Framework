using Azure.Messaging.ServiceBus;
using DragonSpark.Azure.Messaging.Messages.Queues.Durable;
using DragonSpark.Composition;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Channels;

namespace DragonSpark.Azure.Messaging.Messages.Queues;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Register<ServiceBusConfiguration>()
		         .Start<ServiceBusClient>()
		         .Use<Client>()
		         .Singleton()
		         //
		         .Then.Start<Channel<DurableMessageProperties>>()
		         .Use<ProcessChannel>()
		         .Singleton()
		         .Then.Start<ChannelReader<DurableMessageProperties>>()
		         .Use<ProcessReader>()
		         .Singleton()
		         .Then.Start<ChannelWriter<DurableMessageProperties>>()
		         .Use<ProcessWriter>()
		         .Singleton()
		         //
		         .Then.Start<IWriteMessage>()
		         .Forward<WriteMessage>()
		         .Decorate<ProcessAwareWriteMessage>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
		         //
		         .Then.Start<ISendMessage>()
		         .Forward<SendMessage>()
		         .Decorate<NotificationAwareSendMessage>()
		         .Include(x => x.Dependencies)
		         .Singleton()
		         //
		         .Then.Start<ChannelProcessorBackgroundService>()
		         .And<OutboxSweeperBackgroundService>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
		         //
		         .Then.AddHostedService<ChannelProcessorBackgroundService>()
		         //.AddHostedService<OutboxSweeperBackgroundService>() // TODO: 
			;
	}
}