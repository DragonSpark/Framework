using System;
using System.Threading.Tasks;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.AspNet.Run;

public abstract class RunApplication(
	Func<string[], IHostBuilder> builder,
	ConfigureNewApplication application,
	IAllocated<IHost> run)
	: IAllocated<string[]>
{
	readonly Func<string[], IHostBuilder> _builder     = builder;
	readonly ConfigureNewApplication      _application = application;
	readonly IAllocated<IHost>            _run         = run;

	protected RunApplication(Func<string[], IHostBuilder> builder, ConfigureNewApplication application)
		: this(builder, application, RunInitializedProgram.Default) {}

	public Task Get(string[] parameter)
	{
		var builder = _builder(parameter);
		var host    = builder.Build();
		var (@new, configure) = _application;
		var application = @new(host);
		configure.Execute(application);
		return _run.Get(application);
	}

	protected IHostBuilder Create(string[] parameter) => _builder(parameter);
}