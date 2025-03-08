using System;
using DragonSpark.Application.AspNet.Run;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Hosting.Server.Run;

public abstract class Program(
	Func<IHostBuilder, IHostBuilder> select,
	ICommand<IHostedApplicationBuilder> builder,
	ICommand<IApplication> application)
	: AspNet.Run.Program(Start.A.Selection<string[]>()
	                                 .By.Calling(InitializeBuilder.Default.Get)
	                                 .Select(new SelectBuilder(select, builder)),
	                            new ConfigureNewApplication(application))
{
	protected Program(ICommand<IHostedApplicationBuilder> builder, ICommand<IApplication> application)
		: this(x => x, builder, application) {}
}