using DragonSpark.Application.AspNet;
using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Application.Compose;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public static class Extensions
{
	public static ApplicationProfileContext AsAzureApplication(this BuildHostContext @this)
		=> @this.AsAzureApplication(x => x.AddSingleton<DefaultServiceBusConfiguration>()
		                                  .Deferred<DefaultServiceBusConfiguration>()
		                                  .Assume());

	public static ApplicationProfileContext AsAzureApplication(this BuildHostContext @this,
	                                                           Func<IServiceCollection, Action<ServiceBusOptions>>
		                                                           options)
		=> @this.Configure(new Hosting(options)).Apply(DefaultApplicationProfile.Default);
}