using DragonSpark.Application.Compose;
using DragonSpark.Composition.Compose;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public static class Extensions
{
	public static ApplicationProfileContext AsAzureApplication(this BuildHostContext @this)
		=> @this.AsAzureApplication(DefaultQueueConfiguration.Default.Execute,
		                            DefaultServiceBusConfiguration.Default.Execute);

	public static ApplicationProfileContext AsAzureApplication(this BuildHostContext @this,
	                                                           Action<QueuesOptions> configure,
	                                                           Action<ServiceBusOptions> bus)
		=> @this.Configure(new Hosting(configure, bus)).Apply(DefaultApplicationProfile.Default);
}