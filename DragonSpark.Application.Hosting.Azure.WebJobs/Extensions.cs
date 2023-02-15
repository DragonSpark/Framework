using DragonSpark.Application.Compose;
using DragonSpark.Composition.Compose;
using Microsoft.Azure.WebJobs.Host;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public static class Extensions
{
	public static ApplicationProfileContext WithQueueApplication(this BuildHostContext @this)
		=> @this.WithQueueApplication(DefaultQueueConfiguration.Default.Execute);

	public static ApplicationProfileContext WithQueueApplication(this BuildHostContext @this,
	                                                             Action<QueuesOptions> configure)
		=> @this.Configure(new Hosting(configure)).Apply(DefaultApplicationProfile.Default);
}