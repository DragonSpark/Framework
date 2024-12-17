using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Hosting.Uno.Run;

sealed class Builder : IApplicationBuilder
{
	readonly IApplicationBuilder              _previous;
	readonly Func<IHostBuilder, IHostBuilder> _select;
	readonly List<Action<IHostBuilder>>       _delegates;

	public Builder(IApplicationBuilder previous, Func<IHostBuilder, IHostBuilder> select)
		: this(previous, select, []) {}

	public Builder(IApplicationBuilder previous, Func<IHostBuilder, IHostBuilder> select,
	               List<Action<IHostBuilder>> delegates)
	{
		_previous  = previous;
		_select    = select;
		_delegates = delegates;
	}

	[MustDisposeResource(false)]
	public IHost Build()
	{
		var @default = UnoHost.CreateDefaultBuilder(Environment.GetCommandLineArgs().Skip(1).ToArray());
		var builder  = _select(@default);
		foreach (var del in _delegates)
		{
			del(builder);
		}

		var result = builder.Build();
		return result;
	}

	public IApplicationBuilder Configure(Action<IHostBuilder> configureHost)
	{
		_delegates.Add(configureHost);
		return this;
	}

	public Microsoft.UI.Xaml.Application App => _previous.App;

	public LaunchActivatedEventArgs Arguments => _previous.Arguments;

	public Window Window => _previous.Window;

	public IDictionary<object, object> Properties => _previous.Properties;
}