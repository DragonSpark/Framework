using DragonSpark.Model.Commands;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class Hosting : ICommand<IHostBuilder>
{
	readonly Func<IServiceCollection, Action<ServiceBusOptions>> _options;

	public Hosting(Func<IServiceCollection, Action<ServiceBusOptions>> options) => _options = options;

	public void Execute(IHostBuilder parameter)
	{
		parameter.ConfigureServices(x => x.AddSingleton<INameResolver, NameResolver>())
		         .ConfigureWebJobs(x => x.AddAzureStorageBlobs().AddServiceBus(_options(x.Services)));
	}
}