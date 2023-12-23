using Azure.Storage.Queues;
using DragonSpark.Model.Commands;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class DefaultQueueConfiguration : ICommand<QueuesOptions>
{
	public static DefaultQueueConfiguration Default { get; } = new();

	DefaultQueueConfiguration() : this(TimeSpan.FromSeconds(2)) {}

	readonly TimeSpan _poll;

	public DefaultQueueConfiguration(TimeSpan poll) => _poll = poll;

	public void Execute(QueuesOptions parameter)
	{
		parameter.NewBatchThreshold  = 16;
		parameter.MaxPollingInterval = _poll;
		parameter.MessageEncoding    = QueueMessageEncoding.None;
	}
}

// TODO

sealed class DefaultServiceBusConfiguration : ICommand<ServiceBusOptions>
{
	public static DefaultServiceBusConfiguration Default { get; } = new();

	DefaultServiceBusConfiguration() {}

	public void Execute(ServiceBusOptions parameter) {}
}