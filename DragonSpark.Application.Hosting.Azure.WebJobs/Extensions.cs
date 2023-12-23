using DragonSpark.Application.Compose;
using DragonSpark.Composition.Compose;
using Microsoft.Azure.WebJobs.ServiceBus;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public static class Extensions
{
	public static ApplicationProfileContext AsAzureApplication(this BuildHostContext @this)
		=> @this.AsAzureApplication(DefaultServiceBusConfiguration.Default.Execute);

	public static ApplicationProfileContext AsAzureApplication(this BuildHostContext @this,
	                                                           Action<ServiceBusOptions> bus)
		=> @this.Configure(new Hosting(bus)).Apply(DefaultApplicationProfile.Default);
}