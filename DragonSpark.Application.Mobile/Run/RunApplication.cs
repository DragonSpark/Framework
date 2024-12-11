using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Uno.Extensions.Hosting;

namespace DragonSpark.Application.Mobile.Run;

public abstract class RunApplication : IAllocating<InitializeInput, IApplication>
{
	readonly Func<InitializeInput, IApplicationBuilder> _builder;
	readonly Func<IApplicationBuilder, Task<IHost>>     _host;

	protected RunApplication(Func<InitializeInput, IApplicationBuilder> builder,
	                         Func<IApplicationBuilder, Task<IHost>> host)
	{
		_host    = host;
		_builder = builder;
	}

	public async Task<IApplication> Get(InitializeInput parameter)
	{
		var builder = _builder(parameter);
		return new Application(builder, await _host(builder).Await());
	}
}

sealed class Application : IApplication
{
	readonly IApplicationBuilder _previous;

	public Application(IApplicationBuilder previous, IHost host)
	{
		Host      = host;
		_previous = previous;
	}

	public IApplicationBuilder Configure(Action<IHostBuilder> configureHost) => _previous.Configure(configureHost);

	[MustDisposeResource]
	public IHost Build() => _previous.Build();

	public Microsoft.UI.Xaml.Application App => _previous.App;

	public LaunchActivatedEventArgs Arguments => _previous.Arguments;

	public Window Window => _previous.Window;

	public IDictionary<object, object> Properties => _previous.Properties;

	public IHost Host { get; }
}

// TODO
public readonly record struct InitializeInput(Microsoft.UI.Xaml.Application Owner, LaunchActivatedEventArgs Arguments);
