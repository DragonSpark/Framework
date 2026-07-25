using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public sealed class PerformInitialization : IStopAware
{
    public static PerformInitialization Default { get; } = new();

    PerformInitialization() : this(Initializing.Default, Initialization.Default, Initialized.Default) {}

    readonly IMutable<bool> _starting;
    readonly IStopAware     _previous;
    readonly IMutable<bool> _started;

    public PerformInitialization(IMutable<bool> starting, IStopAware previous, IMutable<bool> started)
    {
        _starting = starting;
        _previous = previous;
        _started  = started;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        _starting.Up();
        await _previous.Off(parameter);
        _started.Up();
    }
}