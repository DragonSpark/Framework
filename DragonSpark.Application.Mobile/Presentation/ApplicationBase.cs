using DragonSpark.Application.Mobile.Run;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace DragonSpark.Application.Mobile.Presentation;

// ReSharper disable once MissingAnnotation
public abstract class ApplicationBase : Microsoft.UI.Xaml.Application, IApplication
{
    readonly IRunApplication           _run;
    readonly ICommand<Run.Application> _initialized;

    protected ApplicationBase(IRunApplication run) : this(run, ConfigureFromEnvironment.Default) {}

    protected ApplicationBase(IRunApplication run, ICommand<Run.Application> initialized)
    {
        _run         = run;
        _initialized = initialized;
    }

    public void Execute(Run.Application parameter)
    {
        var (builder, host) = parameter;
        MainWindow          = builder.Window;
        Host                = host;
        _initialized.Execute(parameter);
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