using DragonSpark.Application.Mobile.Uno.Presentation;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Hosting.Uno.Run;

sealed class ConfigureAwareBuilder : IApplicationBuilder
{
	readonly IApplicationBuilder          _previous;
	readonly IConfigureApplicationBuilder _configure;

	public ConfigureAwareBuilder(IApplicationBuilder previous, IApplication application)
		: this(previous, new ConfigureApplicationBuilder(application)) {}

	public ConfigureAwareBuilder(IApplicationBuilder previous, IConfigureApplicationBuilder configure)
	{
		_previous  = previous;
		_configure = configure;
		configure.Execute(_previous);
	}

	public IApplicationBuilder Configure(Action<IHostBuilder> configureHost) => _previous.Configure(configureHost);

	public IApplicationBuilder Configure(Action<IHostBuilder, Window> configureHost)
		=> _previous.Configure(configureHost);

	[MustDisposeResource]
	public IHost Build()
	{
		var result = _previous.Build();
		_configure.Execute(new Mobile.Uno.Run.Application(_previous, result));
		return result;
	}

	public Microsoft.UI.Xaml.Application App => _previous.App;

	public LaunchActivatedEventArgs Arguments => _previous.Arguments;

	public Window Window => _previous.Window;

	public IDictionary<object, object> Properties => _previous.Properties;
}
