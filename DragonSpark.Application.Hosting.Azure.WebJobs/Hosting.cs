using DragonSpark.Model.Commands;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class Hosting : ICommand<IHostBuilder>
{
	readonly Action<ServiceBusOptions> _bus;

	public Hosting(Action<ServiceBusOptions> bus) => _bus = bus;

	public void Execute(IHostBuilder parameter)
	{
		parameter.ConfigureServices(x => x.AddSingleton<INameResolver, NameResolver>())
		         .ConfigureWebJobs(x => x.AddAzureStorageBlobs().AddServiceBus(_bus));
	}
}