using DragonSpark.Application.Run;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Hosting.Server.Run;

public class RunApplication(
	Func<IHostBuilder, IHostBuilder> select,
	ICommand<IHostedApplicationBuilder> builder,
	ICommand<IApplication> application)
	: DragonSpark.Application.Run.RunApplication(x => new ApplicationBuilder(x), new SelectBuilder(select, builder).Get,
	                                             new ConfigureNewApplication(application))
{
	public RunApplication(ICommand<IHostedApplicationBuilder> builder, ICommand<IApplication> application)
		: this(x => x, builder, application) {}
}