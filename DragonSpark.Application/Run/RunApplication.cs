using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Run;

public abstract class RunApplication(
	Func<string[], IHostedApplicationBuilder> @new,
	Func<IHostedApplicationBuilder, IHostBuilder> configure,
	ConfigureNewApplication application)
	: IAllocated<string[]>
{
	readonly Func<string[], IHostedApplicationBuilder>     _new         = @new;
	readonly Func<IHostedApplicationBuilder, IHostBuilder> _configure   = configure;
	readonly ConfigureNewApplication                       _application = application;

	public Task Get(string[] parameter)
	{
		var @new        = _new(parameter);
		var builder     = _configure(@new);
		var host        = builder.Build();
		var application = _application.New(host);
		_application.Configure.Execute(application);
		return application.RunAsync();
	}

	protected IHostBuilder Create(string[] parameter)
	{
		var @new   = _new(parameter);
		var result = _configure(@new);
		return result;
	}
}