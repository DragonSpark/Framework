using System;
using JetBrains.Annotations;
using SerilogTracing;

namespace DragonSpark.Diagnostics;

[method: MustDisposeResource]
public readonly struct Tracing(IDisposable disposable, LoggerActivity activity) : IDisposable
{
    public LoggerActivity Activity { get; } = activity;

    public void Dispose()
    {
        Activity.Dispose();
        disposable.Dispose();
    }
}
