using System;
using DragonSpark.Application.Mobile.Run;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace DragonSpark.Application.Mobile.Presentation;


// ReSharper disable once MissingAnnotation
public abstract class ApplicationBase : Microsoft.UI.Xaml.Application, IApplication
{
	readonly IRunApplication _run;

	protected ApplicationBase(IRunApplication run) => _run = run;

	public void Execute(Run.Application parameter)
	{
		var (builder, host) = parameter;
		MainWindow          = builder.Window;
		Host                = host;
	}

	public Window MainWindow { get; private set; } = null!;

	public IHost Host { get; private set; } = null!;

	// ReSharper disable once AvoidAsyncVoid
	protected async override void OnLaunched(LaunchActivatedEventArgs args)
	{
		// ReSharper disable once AsyncApostle.AsyncAwaitMayBeElidedHighlighting
		await _run.Await(new(this, args));
	}
}

// TODO

sealed class CurrentServices : Result<IServiceProvider>, IServiceProvider
{
	public static CurrentServices Default { get; } = new();

	CurrentServices() : base(() => Microsoft.UI.Xaml.Application.Current.To<ApplicationBase>().Host.Services) {}

	public object? GetService(Type serviceType) => Get().GetService(serviceType);
}
