using DragonSpark.Application.AspNet;
using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Composition.Compose;
using Microsoft.Azure.WebJobs.ServiceBus;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public static class Extensions
{
	extension(BuildHostContext @this)
	{
		public ApplicationProfileContext AsAzureApplication() => @this.AsAzureApplication(_ => {});

		public ApplicationProfileContext AsAzureApplication(Action<ServiceBusOptions> options)
			=> @this.Configure(new Hosting(options)).Apply(DefaultApplicationProfile.Default);
	}
}