using DragonSpark.Application.Mobile.Run;
using DragonSpark.Compose;
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
