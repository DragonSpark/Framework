using DragonSpark.Azure.Messaging.Messages;
using DragonSpark.Model.Commands;
using Microsoft.Azure.WebJobs.ServiceBus;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class DefaultServiceBusConfiguration : ICommand<ServiceBusOptions>
{
	readonly ushort? _maximum;

	public DefaultServiceBusConfiguration(ServiceBusConfiguration configuration)
		: this(configuration.MaxConcurrentCalls) {}

	public DefaultServiceBusConfiguration(ushort? maximum) => _maximum  = maximum;

	public void Execute(ServiceBusOptions parameter)
	{
		parameter.MaxConcurrentCalls = _maximum ?? parameter.MaxConcurrentCalls;
	}
}