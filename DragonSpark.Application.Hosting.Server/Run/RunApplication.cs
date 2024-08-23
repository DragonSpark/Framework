using DragonSpark.Application.Run;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Hosting.Server.Run;

public abstract class RunApplication(
	Func<IHostBuilder, IHostBuilder> select,
	ICommand<IHostedApplicationBuilder> builder,
	ICommand<IApplication> application)
	: DragonSpark.Application.Run.RunApplication(Start.A.Selection<string[]>()
	                                                  .By.Calling(x => new ApplicationBuilder(x))
	                                                  .Select(new SelectBuilder(select, builder)),
	                                             new ConfigureNewApplication(application))
{
	protected RunApplication(ICommand<IHostedApplicationBuilder> builder, ICommand<IApplication> application)
		: this(x => x, builder, application) {}
}