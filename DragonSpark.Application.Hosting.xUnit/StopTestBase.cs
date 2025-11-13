using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using Xunit;

namespace DragonSpark.Application.Hosting.xUnit;

public abstract class StopTestBase : IAsyncLifetime
{
    readonly CancellationTokenSource _source;

    protected StopTestBase() : this(new()) {}

    protected StopTestBase(CancellationTokenSource source) : this(source, source.Token) {}

    protected StopTestBase(CancellationTokenSource source, CancellationToken stop)
    {
        _source = source;
        Stop    = stop;
    }

    protected CancellationToken Stop { get; }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    public virtual async Task DisposeAsync()
    {
        await _source.CancelAsync().Off();
        _source.Dispose();
    }
}