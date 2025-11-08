using DragonSpark.Model.Commands;
using Microsoft.Azure.WebJobs.ServiceBus;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class DefaultServiceBusConfiguration : ICommand<ServiceBusOptions>
{
	public static DefaultServiceBusConfiguration Default { get; } = new();

	DefaultServiceBusConfiguration() {}

	public void Execute(ServiceBusOptions parameter)
	{
		parameter.MaxConcurrentCalls = 4;
	}
}