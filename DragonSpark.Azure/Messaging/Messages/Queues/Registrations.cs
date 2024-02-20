﻿using Azure.Messaging.ServiceBus;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Azure.Messaging.Messages.Queues;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Register<ServiceBusConfiguration>().Start<ServiceBusClient>().Use<Client>().Singleton();
	}
}