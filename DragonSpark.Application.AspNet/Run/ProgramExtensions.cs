using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DragonSpark.Application.AspNet.Run;

public static class ProgramExtensions
{
	public static ICommand<IHostedApplicationBuilder> Adapt(this ICommand<IServiceCollection> @this)
		=> new ConfigureServicesAdapter(@this);

	public static IHostedApplicationBuilder Application(this IServiceCollection @this)
		=> @this.GetRequiredInstance<IHostedApplicationBuilder>();

	public static ICommand<IServiceCollection> Adapt(this ICommand<IHostedApplicationBuilder> @this)
		=> new ConfigureApplicationAdapter(@this);

	public static IProgram WithLogging<T>(this T @this) where T : IProgram => @this.WithLogging(x => x.WriteTo.Console());

	public static IProgram WithLogging<T>(this T @this, Alter<LoggerConfiguration> configure) where T : IProgram
		=> new BootstrappedProgram(new LoggedProgram<T>(@this), configure);
}