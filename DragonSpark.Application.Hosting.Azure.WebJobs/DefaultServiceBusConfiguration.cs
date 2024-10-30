using DragonSpark.Azure.Messaging.Messages;
using DragonSpark.Model.Commands;
using Microsoft.Azure.WebJobs.ServiceBus;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class DefaultServiceBusConfiguration : ICommand<ServiceBusOptions>
{
	readonly ServiceBusConfiguration _configuration;

	public DefaultServiceBusConfiguration(ServiceBusConfiguration configuration) => _configuration = configuration;

	public void Execute(ServiceBusOptions parameter)
	{
		parameter.MaxConcurrentCalls = _configuration.MaxConcurrentCalls ?? parameter.MaxConcurrentCalls;
	}
}