using DragonSpark.Application.AspNet.Run;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Hosting.Server.Run;

public abstract class Program(
	Func<IHostBuilder, IHostBuilder> select,
	ICommand<IHostedApplicationBuilder> builder,
	ICommand<IApplication> application, IAllocated<IHost> run)
	: AspNet.Run.Program(Start.A.Selection<string[]>()
	                          .By.Calling(InitializeBuilder.Default.Get)
	                          .Select(new SelectBuilder(select, builder)),
	                     new ConfigureNewApplication(application), 
	                     run)
{
	protected Program(ICommand<IHostedApplicationBuilder> builder, ICommand<IApplication> application)
		: this(x => x, builder, application) {}

	public Program(Func<IHostBuilder, IHostBuilder> select,
	               ICommand<IHostedApplicationBuilder> builder,
	               ICommand<IApplication> application)
		: this(select, builder, application, RunInitializedProgram.Default) {}
}