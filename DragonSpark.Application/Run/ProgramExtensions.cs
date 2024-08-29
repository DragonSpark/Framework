using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Run;

public static class ProgramExtensions
{
	public static ICommand<IHostedApplicationBuilder> Adapt(this ICommand<IServiceCollection> @this)
		=> new ConfigureServicesAdapter(@this);
}