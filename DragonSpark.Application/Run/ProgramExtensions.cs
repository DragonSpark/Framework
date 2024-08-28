using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Run;

public static class ProgramExtensions
{
	public static ICommand<IHostedApplicationBuilder> Adapt(this ICommand<IServiceCollection> @this)
		=> new ConfigureServicesAdapter(@this);

	public static ICommand<IApplicationBuilder> Adapt(this ICommand<IHostApplicationBuilder> @this)
		=> new ApplicationConfigurationAdapter(@this);
}