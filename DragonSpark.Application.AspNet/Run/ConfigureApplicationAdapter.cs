using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Run;

sealed class ConfigureApplicationAdapter : ICommand<IServiceCollection>
{
	readonly ICommand<IHostedApplicationBuilder> _previous;

	public ConfigureApplicationAdapter(ICommand<IHostedApplicationBuilder> previous) => _previous = previous;

	public void Execute(IServiceCollection parameter)
	{
		_previous.Execute(parameter.Application());
	}
}