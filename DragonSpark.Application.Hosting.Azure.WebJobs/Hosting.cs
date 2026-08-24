using DragonSpark.Model.Commands;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class Hosting : ICommand<IHostBuilder>
{
	readonly Action<ServiceBusOptions>  _options;
	readonly Action<IServiceCollection> _services;

	public Hosting(Action<ServiceBusOptions> options) : this(options, DefaultServiceConfiguration.Default.Execute) {}

	public Hosting(Action<ServiceBusOptions> options, Action<IServiceCollection> services)
	{
		_options  = options;
		_services = services;
	}

	public void Execute(IHostBuilder parameter)
	{
		parameter.ConfigureServices(_services ).ConfigureWebJobs(x => x.AddAzureStorageBlobs().AddServiceBus(_options));
	}
}