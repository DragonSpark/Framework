using DragonSpark.Azure.Messaging.Messages;
using DragonSpark.Model.Commands;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class DefaultServiceConfiguration : ICommand<IServiceCollection>
{
	public static DefaultServiceConfiguration Default { get; } = new();

	DefaultServiceConfiguration() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.AddSingleton<INameResolver, NameResolver>()
		         .AddOptions<ServiceBusOptions>()
		         .Configure<ServiceBusConfiguration>((options, configuration) =>
		                                             {
			                                             options.MaxConcurrentCalls =
				                                             configuration
					                                             .MaxConcurrentCalls ??
				                                             options.MaxConcurrentCalls;
		                                             });
	}
}