using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Run;

public static class ProgramExtensions
{
	public static ICommand<IHostedApplicationBuilder> Adapt(this ICommand<IServiceCollection> @this)
		=> new ConfigureServicesAdapter(@this);

	public static IHostedApplicationBuilder Application(this IServiceCollection @this)
		=> @this.GetRequiredInstance<IHostedApplicationBuilder>();

	public static ICommand<IServiceCollection> Adapt(this ICommand<IHostedApplicationBuilder> @this)
		=> new ConfigureApplicationAdapter(@this);
}