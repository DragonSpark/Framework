using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Run;

sealed class ConfigureServicesAdapter : ICommand<IHostedApplicationBuilder>
{
	readonly ICommand<IServiceCollection> _previous;

	public ConfigureServicesAdapter(ICommand<IServiceCollection> previous) => _previous = previous;

	public void Execute(IHostedApplicationBuilder parameter)
	{
		_previous.Execute(parameter.Services);
	}
}